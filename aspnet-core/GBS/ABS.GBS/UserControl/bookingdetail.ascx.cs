using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using ABS.Logic.GroupBooking.Agent;
using ABS.Logic.GroupBooking.Booking;
using SEAL.Data;
using DevExpress.Web;
using System.Collections;
using ABS.Logic.Shared;
using System.Globalization;
//using log4net;
using DevExpress.Data;
using DevExpress.XtraGrid;
using System.Text.RegularExpressions;
using System.Configuration;

using ABS.Navitaire.BookingManager;
using ABS.GBS.Log;
using StackExchange.Profiling;

namespace GroupBooking.Web.UserControl
{
    public partial class bookingdetail : System.Web.UI.UserControl
    {
        ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogControl log = new LogControl();

        UserSet AgentSet;
        AdminSet AdminSet;
        List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain> TransMainData = new List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>();
        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        BookingTransactionDetail bookDTLInfo = new BookingTransactionDetail();
        List<BookingTransactionDetail> lstbookFlightInfo = new List<BookingTransactionDetail>();
        List<BookingTransTender> lstbookPaymentInfo = new List<BookingTransTender>();
        List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
        List<BookingTransactionDetail> lstRejectedbookDTLInfo = new List<BookingTransactionDetail>();
        List<BookingTransactionDetail> listDetailCombinePNR = new List<BookingTransactionDetail>();
        List<BookingTransactionDetail> lstFareBreakdown = new List<BookingTransactionDetail>();
        List<BookingTransactionDetail> lstFareBreakdownReturn = new List<BookingTransactionDetail>();

        SystemLog SystemLog = new SystemLog();

        BookingTransactionMain bookHDRInfo = new BookingTransactionMain();
        DataTable dtFareBreakdown;
        DataTable dtPass;
        DataTable dtInfant;
        DataTable dtAddOn;
        DataTable dtTotPax;
        ABS.GBS.eService.ProcessServiceSoapClient eWS = new ABS.GBS.eService.ProcessServiceSoapClient();
        bool IsAllowSeatChg, IsAllowAddOnChg, IsAllowDivideBooking;
        Boolean newGBS = true;

        string TransID, RecordLocator, FilterPNR;

        //20170321 - Sienny
        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();

        //20170406 - Sienny
        bool IsInternationalFlight = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;

            objBooking.ClearSessionData();

            gvClass.JSProperties["cpIsUpdated"] = "";

            Session["NewBooking"] = "false";
            //gvClass.GroupSummarySortInfo.AddRange(new ASPxGroupSummarySortInfo("Journey", gvClass.GroupSummary["SeqNo"], ColumnSortOrder.Ascending));
            TransID = Request.QueryString["TransID"];

            if (Session["PNR"] != null)
                RecordLocator = Session["PNR"].ToString();
            else
                RecordLocator = "";

            string keySent = Request.QueryString["k"];
            ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
            string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), TransID.ToString(), "");

            DateTime bookingDate = Convert.ToDateTime("1900-01-01");
            DateTime CheckDate = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["CheckDate"].ToString());
            string OldGBS = System.Configuration.ConfigurationManager.AppSettings["OLDGBS"].ToString();

            if (hashkey != keySent)
            {
                Response.Redirect("~/Invalid.aspx");
            }
            //Added by ketee, redirect to old GBS if booking date less tha config date

            bookingDate = objGeneral.getBookingDate(TransID, ref newGBS);

            if (Session["AgentSet"] != null)
            {
                AgentSet = (UserSet)Session["AgentSet"];

                if (bookingDate < CheckDate && newGBS == false)
                {
                    Response.Redirect(OldGBS + "?hashkey=" + objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), AgentSet.AgentID, AgentSet.AgentName) + "&aid=" + AgentSet.AgentID + "&aName=" + AgentSet.AgentName);
                    return;
                }

                //added by ketee 20131111
                objBooking.CheckInvalidPNRByTransID(TransID);

                // added by diana 20130918
                ////objBooking.UpdateAllTransByAgent(AgentSet.AgentID, AgentSet.AgentName, TransID);
                // end added by diana 20130918
            }


            if (Session["AdminSet"] != null)
            {
                if (AgentSet != null)
                {
                    objBooking.UpdateAllTransByAgent(AgentSet.AgentID, AgentSet.AgentName, TransID);
                }
                else
                {
                    objBooking.UpdateAllTransByAgent("", "", TransID);
                }
                bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(TransID);
                AdminSet = (AdminSet)Session["AdminSet"];
                btPayFull.Visible = false;
                btPassenger.Visible = false;
                //tyas
                btDivide.Visible = false;
                btFlightChange.Visible = false;
                btManageAddOn.Visible = false;
                //Added by Ellis 20170308, manage seats
                btManageSeats.Visible = false;
                //20170719 - Sienny
                btResendItinerary.Visible = false;
                btnManageInsure.Visible = false;//added by romy for insure
            }
            if (!IsPostBack)
            {
                Session["dtGridDetail"] = null;
                Session["dtRejectedGridDetail"] = null;
                Session["dtPassenger"] = null;
                Session["dtAddOn"] = null;
                Session["dtFareBreakdown"] = null;
                Session["PNR"] = null; //initially should be null
                Session["IsReturnFlight"] = null;
                ClearSession();
                using (profiler.Step("LoadData"))
                {
                    LoadData();
                }
                using (profiler.Step("assignDefaultValue"))
                {
                    assignDefaultValue();
                }
                using (profiler.Step("LoadGridView"))
                {
                    LoadGridView();
                }
                using (profiler.Step("InsuranceLoad"))
                {
                    InsuranceLoad();
                }
            }
            else //if (Page.IsCallback)
            {
                using (profiler.Step("LoadGridView"))
                {
                    LoadGridViewCallBack();
                }
                using (profiler.Step("LoadRejectedGridView"))
                {
                    LoadRejectedGridViewCallBack();
                }
                using (profiler.Step("LoadPassengerGridView"))
                {
                    LoadPassengerGridViewCallBack();
                }
                using (profiler.Step("LoadAddOnGridView"))
                {
                    LoadAddOnGridViewCallBack();
                }
                using (profiler.Step("LoadPaymentHistory"))
                {
                    LoadPaymentHistory(TransID);
                }
                using (profiler.Step("LoadFareBreakdown"))
                {
                    LoadFareBreakdown(RecordLocator);
                }
            }
            using (profiler.Step("LoadPaymentSchedule"))
            {
                LoadPaymentSchedule(TransID);
            }
            if (Session["AgentSet"] != null)
                using (profiler.Step("SetScreen1"))
                {
                    SetScreen(TransID, AgentSet.AgentID);
                }
            if (Session["AdminSet"] != null)
                using (profiler.Step("SetScreen2"))
                {
                    SetScreen(TransID, bookHDRInfo.AgentID);
                }

        }

        //added by romy for insure
        private void InsuranceLoad()
        {
            if (Application["InsureEnable"] != null && Convert.ToBoolean(Application["InsureEnable"]) == true)
            {
                btnManageInsure.Visible = true;
                gvPassenger.Columns["Insurance"].Visible = true;
            }
            else
            {
                btnManageInsure.Visible = false;
                gvPassenger.Columns["Insurance"].Visible = false;
                InsureBreakdown.Style.Add("display", "none");
            }
        }

        private void ClearSession()
        {
            HttpContext.Current.Session.Remove("TotalProcessFee");
            HttpContext.Current.Session.Remove("tempFlight");
            HttpContext.Current.Session.Remove("dataClass");
            HttpContext.Current.Session.Remove("PaymentContainers");
            HttpContext.Current.Session.Remove("HashMain");
            HttpContext.Current.Session.Remove("TransMain");
            HttpContext.Current.Session.Remove("dataClassTrans");
            HttpContext.Current.Session.Remove("TransDetail");
            HttpContext.Current.Session.Remove("PaymentMaxAttempt");
            HttpContext.Current.Session.Remove("TotalProcessFee");
            HttpContext.Current.Session.Remove("ErrorPayment");
            HttpContext.Current.Session.Remove("PaymentAttempt");
            HttpContext.Current.Session.Remove("dataBDFeeDepart");
            HttpContext.Current.Session.Remove("dataBDFeeReturn");
            HttpContext.Current.Session.Remove("Fare");
            //HttpContext.Current.Session.Remove("CityPairDepart");
            //HttpContext.Current.Session.Remove("CityPairAll");
            HttpContext.Current.Session.Remove("invalidreturnflight");

            //added by diana 20140124 - store total pax
            HttpContext.Current.Session.Remove("TotalPax");
            HttpContext.Current.Session.Add("TotalPax", 0);

            HttpContext.Current.Session.Remove("qtyMeal");
            HttpContext.Current.Session.Remove("qtyBaggage");
            HttpContext.Current.Session.Remove("qtySport");
            HttpContext.Current.Session.Remove("qtyComfort");
            HttpContext.Current.Session.Remove("qtyDuty");
            HttpContext.Current.Session.Remove("dtMeal");
            HttpContext.Current.Session.Remove("dtBaggage");
            HttpContext.Current.Session.Remove("dtSport");
            HttpContext.Current.Session.Remove("dtInsure");
            HttpContext.Current.Session.Remove("dtComfort");
            HttpContext.Current.Session.Remove("dtDuty");
            HttpContext.Current.Session.Remove("dtGridPass");
            HttpContext.Current.Session.Remove("dtGridPass2");
            Session["IsNew"] = "true";
            //Add-on
            HttpContext.Current.Session.Remove("PaxStatus");
            HttpContext.Current.Session.Remove("dtBaggageDepart");
            HttpContext.Current.Session.Remove("dtBaggageReturn");
            HttpContext.Current.Session.Remove("dtSportDepart");
            HttpContext.Current.Session.Remove("dtSportReturn");
            HttpContext.Current.Session.Remove("dtInsureDepart");
            HttpContext.Current.Session.Remove("dtInsureReturn");
            HttpContext.Current.Session.Remove("dtMealDepart");
            HttpContext.Current.Session.Remove("dtMealDepart2");
            HttpContext.Current.Session.Remove("dtComfortDepart");
            HttpContext.Current.Session.Remove("dtDutyDepart");
            HttpContext.Current.Session.Remove("dtMealReturn");
            HttpContext.Current.Session.Remove("dtMealReturn2");
            HttpContext.Current.Session.Remove("dtComfortReturn");
            HttpContext.Current.Session.Remove("dtDutyReturn");
            HttpContext.Current.Session.Remove("qtyMeal");
            HttpContext.Current.Session.Remove("qtyBaggage");
            HttpContext.Current.Session.Remove("qtySport");
            HttpContext.Current.Session.Remove("qtyInsure");
            HttpContext.Current.Session.Remove("qtyComfort");
            HttpContext.Current.Session.Remove("qtyDuty");
            HttpContext.Current.Session.Remove("qtyMeal2");
            HttpContext.Current.Session.Remove("qtyBaggage2");
            HttpContext.Current.Session.Remove("qtySport2");
            HttpContext.Current.Session.Remove("qtyInsure2");
            HttpContext.Current.Session.Remove("qtyComfort2");
            HttpContext.Current.Session.Remove("qtyDuty2");
            //Seat
            HttpContext.Current.Session.Remove("btnSelected");
            HttpContext.Current.Session.Remove("TransID");
            HttpContext.Current.Session.Remove("signature");
            HttpContext.Current.Session.Remove("TempFlight");
            HttpContext.Current.Session.Remove("SeatInfo0Xml");
            HttpContext.Current.Session.Remove("SeatInfo1Xml");
            HttpContext.Current.Session.Remove("SeatInfo2Xml");
            HttpContext.Current.Session.Remove("SeatInfo3Xml");
            HttpContext.Current.Session.Remove("pAvailableSeatInfo0");
            HttpContext.Current.Session.Remove("pAvailableSeatInfo1");
            HttpContext.Current.Session.Remove("pAvailableSeatInfo2");
            HttpContext.Current.Session.Remove("pAvailableSeatInfo2");
            HttpContext.Current.Session.Remove("SeatInfo0");
            HttpContext.Current.Session.Remove("SeatInfo1");
            HttpContext.Current.Session.Remove("SeatInfo2");
            HttpContext.Current.Session.Remove("SeatInfo3");
            HttpContext.Current.Session.Remove("ErrorMsg");
            HttpContext.Current.Session.Remove("DepartSeatInfo");
            HttpContext.Current.Session.Remove("ReturnSeatInfo");
            HttpContext.Current.Session.Remove("DepartConnectingSeatInfo");
            HttpContext.Current.Session.Remove("ReturnConnectingSeatInfo");
            HttpContext.Current.Session.Remove("DepartConnectingExistingSeatInfo");
            HttpContext.Current.Session.Remove("DepartConnectingExistingSeatInfo2");
            HttpContext.Current.Session.Remove("ReturnConnectingExistingSeatInfo");
            HttpContext.Current.Session.Remove("ReturnConnectingExistingSeatInfo2");
            HttpContext.Current.Session.Remove("DepartExistingSeatInfo");
            HttpContext.Current.Session.Remove("ReturnExistingSeatInfo");
            HttpContext.Current.Session.Remove("forfeitedamount");
            HttpContext.Current.Session.Remove("back");

            if (Request.Cookies["cookieSearchcondition"] != null)
            {
                HttpCookie cookieTemp = Request.Cookies["cookieSearchcondition"];
                cookieTemp.HttpOnly = true;
                if (cookieTemp != null)
                {
                    cookieTemp.Expires = DateTime.Today.AddDays(-1);
                    Response.Cookies.Add(cookieTemp);
                }
            }
        }

        protected bool CheckLimitTime(string TransID, string Criteria)
        {
            lstbookDTLInfo = objBooking.GetAllBK_TRANSDTLFilterAll(TransID, 0);
            if (lstbookDTLInfo != null && lstbookDTLInfo.Count > 0)
            {
                decimal LimitTime = 0;
                DateTime DepDate = lstbookDTLInfo[0].DepatureDate;
                decimal TimeDiff = decimal.Parse((DepDate - DateTime.Now).TotalHours.ToString());

                string CriteriaStr = "";
                switch (Criteria)
                {
                    case "SEAT":
                        CriteriaStr = "SEATCHG";
                        break;
                    case "ADDON":
                        CriteriaStr = "ADDONCHG";
                        break;
                    case "DIVIDEBOOKING":
                        CriteriaStr = "DIVIDEBOOKING";
                        break;

                }

                string GroupName = objGeneral.getOPTGroupByCarrierCode(lstbookDTLInfo[0].CarrierCode);

                ABS.Logic.GroupBooking.Settings objSYS_PREFT_Info = new ABS.Logic.GroupBooking.Settings();

                if (GroupName.ToLower().Trim() == "aax")
                {
                    objSYS_PREFT_Info = objGeneral.GetSingleSYS_PREFT(1, GroupName.ToUpper().Trim(), CriteriaStr + "LONGLIMITTIME");
                    if (objSYS_PREFT_Info != null)
                    {
                        LimitTime = decimal.Parse(objSYS_PREFT_Info.SYSValue);
                    }
                }
                else
                {
                    objSYS_PREFT_Info = objGeneral.GetSingleSYS_PREFT(1, GroupName.ToUpper().Trim(), CriteriaStr + "SHORTLIMITTIME");
                    if (objSYS_PREFT_Info != null)
                    {
                        LimitTime = decimal.Parse(objSYS_PREFT_Info.SYSValue);
                    }
                }

                if (TimeDiff < LimitTime)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        protected void SetScreen(string TransID, string agID)
        {
            DateTime dateValue;
            DataTable dtLimit = new DataTable();
            if (Session["dtLimit"] != null)
            {
                dtLimit = (DataTable)Session["dtLimit"];
            }
            DataRow[] resultSeat = dtLimit.Select("Syskey LIKE 'SEAT%'");
            DataRow[] resultAddOn = dtLimit.Select("Syskey LIKE 'ADDON%'");
            DataRow[] resultDivide = dtLimit.Select("Syskey LIKE 'DIVIDE%'");
            DataRow[] resultFlight = dtLimit.Select("Syskey LIKE 'FLIGHT%'");
            DataRow[] resultName = dtLimit.Select("Syskey LIKE 'NAME%'");
            DataRow[] resultUpload = dtLimit.Select("Syskey LIKE 'UPLOAD%'");
            gvClass.Columns[0].Visible = false; //set change flight to false initially

            try
            {
                DataTable dt = new DataTable();
                string transStatus = "";
                string actualStatus = "";
                string needPaymentStatus = ""; //added by diana 20140108 - check for needpaymentstatus
                dt = objBooking.GetTransStatusByID(TransID, agID);
                if (dt != null)
                {
                    transStatus = dt.Rows[0]["TransStatus"].ToString();
                    actualStatus = dt.Rows[0]["TransStatus"].ToString();
                }
                lblNote.Visible = false;
                if (transStatus != "") //record found
                {
                    lblReason.Text = dt.Rows[0]["TransRemark1"].ToString();

                    //added by diana 20140108 - check for needpaymentstatus
                    needPaymentStatus = dt.Rows[0]["NeedPaymentStatus"].ToString();
                    if (transStatus == "2" || transStatus == "3")
                    {
                        if (needPaymentStatus == "1") transStatus = "1";
                    }
                    //end added by diana 20140108 - check for needpaymentstatus

                    //lblRemarks.Text = "Remarks :";
                    switch (transStatus) //setting screen based on status
                    {
                        case "0":
                            btPayFull.Visible = false;
                            btPassenger.Visible = false;
                            btCancel.Visible = false;
                            btDivide.Visible = false;
                            btFlightChange.Visible = false;
                            btManageAddOn.Visible = false;
                            btManageSeats.Visible = false;
                            btnManageInsure.Visible = false;//added by romy for insure
                            //Added by Ellis 20170308, manage seats

                            if (System.Configuration.ConfigurationManager.AppSettings["ACESession"].ToString().Contains("stgace"))
                            {
                                btManageSeats.Visible = false;
                            }

                            //20170719 - Sienny
                            btResendItinerary.Visible = false;

                            break;
                        case "1":
                            btPayFull.Visible = true;
                            btDivide.Visible = false;
                            btFlightChange.Visible = false;
                            btnManageInsure.Visible = false;//added by romy for insure
                            //btManageAddOn.Visible = false;
                            btCancel.Visible = false;
                            //Added by Ellis 20170308, manage seats
                            foreach (DataRow row in resultSeat)
                            {
                                if (row[4].ToString().Trim() == "STD")
                                {
                                    if (DateTime.Now < Convert.ToDateTime(dt.Rows[0]["STDDate"]).AddHours(-Convert.ToInt32(row[3])))
                                    {
                                        btManageSeats.Visible = true;
                                    }
                                    else
                                    {
                                        btManageSeats.Visible = false;
                                    }
                                }
                            }
                            btPassenger.Visible = false;
                            //if (actualStatus == "3") //added by diana 20140108 - hide cancel button if actual status is 3 = approved
                            //{
                            //    btCancel.Visible = false;
                            //}
                            //else if (DateTime.Now > Convert.ToDateTime(dt.Rows[0]["STDDate"]))
                            if (DateTime.Now > Convert.ToDateTime(dt.Rows[0]["STDDate"]))
                            {
                                if (actualStatus != "2" && actualStatus != "3") //amended by diana 20140108 - hide only if actual status is not 2 or 3
                                {
                                    btCancel.Visible = false;
                                    btPayFull.Visible = false;
                                }
                            }
                            /// needed to be edited to date again
                            else if (DateTime.Now >= Convert.ToDateTime(dt.Rows[0]["ExpiryDate"]))
                            {
                                btCancel.Visible = false; //not allow to cancel anymore, 20170321
                                btPayFull.Visible = true;
                                //lblNote.Visible = true;
                            }
                            if (Session["AgentSet"] != null)
                                btCancel.Visible = false; //not allow to cancel anymore for agent, 20170321

                            //if (System.Configuration.ConfigurationManager.AppSettings["ACESession"].ToString().Contains("stgace"))
                            //hide by Tyas 1st, 20170527    
                            //btManageAddOn.Visible = true;
                            foreach (DataRow row in resultAddOn)
                            {
                                if (row[4].ToString().Trim() == "STD")
                                {
                                    if (DateTime.Now < Convert.ToDateTime(dt.Rows[0]["STDDate"]).AddHours(-Convert.ToInt32(row[3])))
                                    {
                                        //hide by Tyas 1st, 20170527    
                                        btManageAddOn.Visible = true;
                                        //btManageAddOn.Visible = false;
                                    }
                                    else
                                    {
                                        btManageAddOn.Visible = false;
                                    }
                                }
                            }
                            //btManageAddOn.Visible = false;

                            if (Session["AdminSet"] != null)
                            {
                                btCancel.Visible = true;
                            }

                            //20170719 - Sienny
                            btResendItinerary.Visible = false;

                            break;
                        case "2":
                            btPayFull.Visible = false;
                            btPassenger.Visible = true;
                            btDivide.Visible = true;//edited by romy for divide before upload
                            btFlightChange.Visible = false;
                            btnManageInsure.Visible = false;//added by romy for insure
                            //hide by Tyas 1st, 20170527    
                            btManageAddOn.Visible = true;
                            //btManageAddOn.Visible = false;
                            //Added by Ellis 20170308, manage seats

                            //IsAllowSeatChg = CheckLimitTime(TransID, "SEAT");
                            //IsAllowAddOnChg = CheckLimitTime(TransID, "SEAT");
                            //IsAllowDivideBooking = CheckLimitTime(TransID, "SEAT");


                            //20170421 - Sienny
                            foreach (DataRow row in resultUpload)
                            {
                                if (row[4].ToString().Trim() == "STD")
                                {
                                    if (DateTime.Now < Convert.ToDateTime(dt.Rows[0]["STDDate"]).AddHours(-Convert.ToInt32(row[3])))
                                    {
                                        btPassenger.Visible = true;
                                    }
                                    else
                                    {
                                        btPassenger.Visible = false;
                                    }
                                }
                            }
                            btPassenger.Visible = true;

                            foreach (DataRow row in resultSeat)
                            {
                                if (row[4].ToString().Trim() == "STD")
                                {
                                    if (DateTime.Now < Convert.ToDateTime(dt.Rows[0]["STDDate"]).AddHours(-Convert.ToInt32(row[3])))
                                    {
                                        btManageSeats.Visible = true;
                                    }
                                    else
                                    {
                                        btManageSeats.Visible = false;
                                    }
                                }
                            }
                            //btManageSeats.Visible = true;


                            foreach (DataRow row in resultAddOn)
                            {
                                if (row[4].ToString().Trim() == "STD")
                                {
                                    if (DateTime.Now < Convert.ToDateTime(dt.Rows[0]["STDDate"]).AddHours(-Convert.ToInt32(row[3])))
                                    {
                                        //hide by Tyas 1st, 20170527    
                                        btManageAddOn.Visible = true;
                                        //btManageAddOn.Visible = false;
                                    }
                                    else
                                    {
                                        btManageAddOn.Visible = false;
                                    }
                                }
                            }
                            //if (IsAllowAddOnChg == true)
                            //    btManageAddOn.Visible = true;
                            //else
                            //    btManageAddOn.Visible = false;

                            //remark by Tyas 20170329
                            //if (IsAllowDivideBooking == true)
                            //    btDivide.Visible = true;
                            //else
                            //    btDivide.Visible = false;

                            if (DateTime.Now > Convert.ToDateTime(dt.Rows[0]["STDDate"]))
                            {
                                btCancel.Visible = false;
                                btPassenger.Visible = false;
                                //btManageAddOn.Visible = false;
                                //btManageSeats.Visible = false;
                            }
                            else
                            {
                                btCancel.Visible = false; //not allow to cancel anymore
                                //Added by Ellis 20170308, manage seats
                                //if (System.Configuration.ConfigurationManager.AppSettings["ACESession"].ToString().Contains("stgace"))
                                //{
                                //    btManageSeats.Visible = true;
                                //}
                            }

                            if (Session["AdminSet"] != null)
                            {
                                btCancel.Visible = true;
                            }

                            //20170719 - Sienny
                            btResendItinerary.Visible = false;

                            break;
                        case "3":
                            btPayFull.Visible = false;
                            foreach (DataRow row in resultName)
                            {
                                if (row[4].ToString().Trim() == "STD")
                                {
                                    if (DateTime.Now < Convert.ToDateTime(dt.Rows[0]["STDDate"]).AddHours(-Convert.ToInt32(row[3])))
                                    {
                                        btPassenger.Visible = true;
                                    }
                                    else
                                    {
                                        btPassenger.Visible = false;
                                    }
                                }
                            }
                            //btPassenger.Visible = true;
                            btPassenger.Text = "Passenger List";

                            //btManageAddOn.Visible = false;
                            //Added by Ellis 20170308, manage seats
                            btManageSeats.Visible = false;
                            //btCancel.Visible = false; // remark by ketee, 20170312
                            //remark by ketee, not yet ready
                            //btDivide.Visible = true; // Is ready now


                            //IsAllowSeatChg = CheckLimitTime(TransID, "SEAT");
                            //IsAllowAddOnChg = CheckLimitTime(TransID, "ADDON");
                            //IsAllowDivideBooking = CheckLimitTime(TransID, "DIVIDEBOOKING");

                            //DataRow[] resultSeat = dtLimit.Select("Syskey LIKE 'SEAT%'");
                            foreach (DataRow row in resultSeat)
                            {
                                if (row[4].ToString().Trim() == "STD") //remove checking staging , && System.Configuration.ConfigurationManager.AppSettings["ACESession"].ToString().Contains("stgace"), allow to manage seat on production now, 20170421
                                {
                                    if (DateTime.Now < Convert.ToDateTime(dt.Rows[0]["STDDate"]).AddHours(-Convert.ToInt32(row[3])))
                                    {
                                        btManageSeats.Visible = true;
                                    }
                                    else
                                    {
                                        btManageSeats.Visible = false;
                                    }
                                }
                            }

                            //if (IsAllowSeatChg == true && System.Configuration.ConfigurationManager.AppSettings["ACESession"].ToString().Contains("stgace"))
                            //    btManageSeats.Visible = true;
                            //else
                            //    btManageSeats.Visible = false;

                            foreach (DataRow row in resultAddOn)
                            {
                                if (row[4].ToString().Trim() == "STD")
                                {
                                    if (DateTime.Now < Convert.ToDateTime(dt.Rows[0]["STDDate"]).AddHours(-Convert.ToInt32(row[3])))
                                    {
                                        //hide by Tyas 1st, 20170527    
                                        btManageAddOn.Visible = true;
                                        //btManageAddOn.Visible = false;
                                    }
                                    else
                                    {
                                        btManageAddOn.Visible = false;
                                    }
                                }
                            }

                            foreach (DataRow row in resultDivide)
                            {
                                if (row[4].ToString().Trim() == "STD")
                                {
                                    if (DateTime.Now < Convert.ToDateTime(dt.Rows[0]["STDDate"]).AddHours(-Convert.ToInt32(row[3])))
                                    {
                                        btDivide.Visible = true;
                                    }
                                    else
                                    {
                                        btDivide.Visible = false;
                                    }
                                }
                            }

                            //if (IsAllowDivideBooking == true)
                            //    btDivide.Visible = true;
                            //else
                            //    btDivide.Visible = false;

                            if (DateTime.Now > Convert.ToDateTime(dt.Rows[0]["STDDate"]))
                            {
                                btCancel.Visible = false;
                                btPassenger.Visible = false;
                                //btManageAddOn.Visible = false;
                                //btDivide.Visible = false;
                            }
                            else
                            {
                                if (Session["AdminSet"] != null)
                                {
                                    btCancel.Visible = true; //not allow to cancel anymore
                                }
                                else
                                    btCancel.Visible = false; //not allow to cancel anymore

                                foreach (DataRow row in resultDivide)
                                {
                                    if (row[4].ToString().Trim() == "STD")
                                    {
                                        if (DateTime.Now < Convert.ToDateTime(dt.Rows[0]["STDDate"]).AddHours(-Convert.ToInt32(row[3])))
                                        {
                                            btDivide.Visible = true;
                                        }
                                        else
                                        {
                                            btDivide.Visible = false;
                                        }
                                    }
                                }
                                //btDivide.Visible = true;
                                //Added by Ellis 20170308, manage seats
                                //btManageAddOn.Visible = true;
                                foreach (DataRow row in resultFlight)
                                {
                                    if (row[4].ToString().Trim() == "STD" && (System.Configuration.ConfigurationManager.AppSettings["ACESession"].ToString().Contains("stgace") || AgentSet.AgentName.Trim().ToString().Contains("29292929")))
                                    {
                                        if (DateTime.Now < Convert.ToDateTime(dt.Rows[0]["STDDate"]).AddHours(-Convert.ToInt32(row[3])))
                                        {
                                            btFlightChange.Visible = true;
                                        }
                                        else
                                        {
                                            btFlightChange.Visible = false;
                                        }
                                    }
                                }
                                //if (System.Configuration.ConfigurationManager.AppSettings["ACESession"].ToString().Contains("stgace") || AgentSet.AgentName.Trim().ToString().Contains("29292929"))
                                //{
                                //    //btManageSeats.Visible = true;
                                //    btFlightChange.Visible = true;
                                //}

                            }

                            //if (Request.QueryString["div"] != null)
                            //{
                            //    btDivide.Visible = true;
                            //}
                            //else
                            //{
                            //    btDivide.Visible = false;
                            //}

                            //20170719 - Sienny
                            btResendItinerary.Visible = true;

                            break;
                        case "4":
                        case "5":
                        case "6":
                        case "7":
                            btPayFull.Visible = false;
                            btPassenger.Visible = false;
                            btCancel.Visible = false;
                            btGetLatest.Visible = false;
                            lblRemarks.Text = "Reason";
                            btDivide.Visible = false;
                            btFlightChange.Visible = false;
                            btManageAddOn.Visible = false;
                            btnManageInsure.Visible = false;//added by romy for insure
                            //Added by Ellis 20170308, manage seats
                            btManageSeats.Visible = false;
                            btResendItinerary.Visible = false;
                            break;
                            //case "5":
                            //    btPayFull.Visible = false;
                            //    btPassenger.Visible = false;
                            //    btCancel.Visible = false;
                            //    break;
                    }
                    if (Session["modePage"].ToString() == "admin")
                    {
                        if (Session["AdminSet"] != null)
                        {
                            btPayFull.Visible = false;
                            btPassenger.Visible = false;
                            btManageAddOn.Visible = false;
                            btManageSeats.Visible = false;
                            btFlightChange.Visible = false;
                            btDivide.Visible = false;
                            btnManageInsure.Visible = false;//added by romy for insure

                            //20170719 - Sienny
                            btResendItinerary.Visible = false;
                        }
                    }
                    //else if (transStatus == "2" || transStatus == "3") //hide flight change column
                    //{
                    //    gvClass.Columns[0].Visible = true;
                    //}

                }
                else
                {
                    if (Session["AgentSet"] != null)
                    {
                        Response.Redirect(Shared.MySite.PublicPages.Searchflight);
                    }
                    //transaction not found or agent not authorized
                }
            }
            catch
            {

            }
        }

        private void ShowReturnColumn(bool show)
        {
            tdReturnTitle.Visible = show;
            tdReturnFare.Visible = show;
            tdReturn.Visible = show;
            //tdReturnAPT.Visible = show;
            ////tdReturnAPTChd.Visible = show;
            ////tdReturnFuel.Visible = show;
            //tdReturnPaxFee.Visible = show;
            //tdReturnSvc.Visible = show;
            ////tdReturnVAT.Visible = show;

            //tdReturnSSR.Visible = show;
            ////tdReturnBaggage.Visible = show;
            ////tdReturnMeal.Visible = show;
            ////tdReturnSport.Visible = show;
            ////tdReturnComfort.Visible = show;
            //tdReturnInfant.Visible = show;

            //tdReturnSeat.Visible = show;
            //tdReturnOther.Visible = show;
            ////tdReturnInfant.Visible = show;
            //tdReturnPromoDisc.Visible = show;
        }

        protected void LoadPaymentSchedule(string TransID)
        {
            bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(TransID);

            string strHTML = "";
            strHTML += "<table class='table table-bordered'><tr><td><div class='labelDate'>Payment Expiry Date</div></td><td><span class='labelDate'>Min.Payment</span></td><td><span class='labelDate'>Status</span></td></tr>";

            if (bookHDRInfo.PaymentAmtEx1 != 0)
            {
                strHTML += "<tr><td>";
                strHTML += "<div class='labelDate'>" + String.Format("{0:MMM dd, yyyy HH:mm}", bookHDRInfo.PaymentDateEx1) + "</div></td>";
                strHTML += "<td><span class='labelDate'>" + String.Format("{0:#,0.00}", bookHDRInfo.PaymentAmtEx1) + " " + bookHDRInfo.Currency + "</span></td>";
                strHTML += "<td><span class='labelDate'>";
                if (bookHDRInfo.CollectedAmt >= (bookHDRInfo.PaymentAmtEx1))
                    strHTML += "Paid";
                else
                    strHTML += "Pending";
                strHTML += "</span></td></tr>";
            }
            if (bookHDRInfo.PaymentAmtEx2 != 0)
            {
                strHTML += "<tr><td>";
                strHTML += "<div class='labelDate'>" + String.Format("{0:MMM dd, yyyy HH:mm}", bookHDRInfo.PaymentDateEx2) + "</div></td>";
                strHTML += "<td><span class='labelDate'>" + String.Format("{0:#,0.00}", bookHDRInfo.PaymentAmtEx2) + " " + bookHDRInfo.Currency + "</span></td>";
                strHTML += "<td><span class='labelDate'>";
                if ((bookHDRInfo.CollectedAmt - bookHDRInfo.ForfeitedAmount) >= (bookHDRInfo.PaymentAmtEx1 + bookHDRInfo.PaymentAmtEx2))
                    strHTML += "Paid";
                else
                    strHTML += "Pending";
                strHTML += "</span></td></tr>";
            }
            if (bookHDRInfo.PaymentAmtEx3 != 0)
            {
                strHTML += "<tr><td>";
                strHTML += "<div class='labelDate'>" + String.Format("{0:MMM dd, yyyy HH:mm}", bookHDRInfo.PaymentDateEx3) + "</div></td>";
                strHTML += "<td><span class='labelDate'>" + String.Format("{0:#,0.00}", bookHDRInfo.PaymentAmtEx3) + " " + bookHDRInfo.Currency + "</span></td>";
                strHTML += "<td><span class='labelDate'>";
                if ((bookHDRInfo.CollectedAmt - bookHDRInfo.ForfeitedAmount) >= (bookHDRInfo.PaymentAmtEx1 + bookHDRInfo.PaymentAmtEx2 + bookHDRInfo.PaymentAmtEx3))
                    strHTML += "Paid";
                else
                    strHTML += "Pending";
                strHTML += "</span></td></tr>";
            }

            //decimal TotAddCharge = objBooking.GetTotalAddFee(TransID);
            decimal forfeitedAmount = 0;
            decimal AddCharge = 0;
            if ((bookHDRInfo.TransTotalAmt - bookHDRInfo.CollectedAmt) > 0 && (bookHDRInfo.CollectedAmt - bookHDRInfo.ForfeitedAmount) >= (bookHDRInfo.PaymentAmtEx1 + bookHDRInfo.PaymentAmtEx2 + bookHDRInfo.PaymentAmtEx3))
            {
                AddCharge = bookHDRInfo.TransTotalAmt - bookHDRInfo.PaymentAmtEx1 - bookHDRInfo.PaymentAmtEx2 - bookHDRInfo.PaymentAmtEx3 - (bookHDRInfo.TransTotalAmt - bookHDRInfo.CollectedAmt);
            }
            else
            {
                AddCharge = bookHDRInfo.TransTotalAmt - bookHDRInfo.PaymentAmtEx1 - bookHDRInfo.PaymentAmtEx2 - bookHDRInfo.PaymentAmtEx3;
            }
            if (bookHDRInfo.ForfeitedAmount != 0)
            {
                forfeitedAmount = bookHDRInfo.ForfeitedAmount;
                AddCharge = AddCharge - forfeitedAmount;
            }
            if (AddCharge != 0)
            {
                strHTML += "<tr><td>";
                strHTML += "<div class='labelDate'>" + "Additional Charges" + "</div></td>";
                strHTML += "<td><span class='labelDate'>" + String.Format("{0:#,0.00}", AddCharge) + " " + bookHDRInfo.Currency + "</span></td>";
                strHTML += "<td><span class='labelDate'>" + "Paid";
                strHTML += "</span></td></tr>";
            }
            if (forfeitedAmount != 0)
            {
                strHTML += "<tr><td>";
                strHTML += "<div class='labelDate'>" + "Forfeited Amount" + "</div></td>";
                strHTML += "<td><span class='labelDate'>" + String.Format("{0:#,0.00}", forfeitedAmount) + " " + bookHDRInfo.Currency + "</span></td>";
                strHTML += "<td><span class='labelDate'>" + "Paid";
                strHTML += "</span></td></tr>";
            }
            if ((bookHDRInfo.TransTotalAmt - bookHDRInfo.CollectedAmt) > 0 && (bookHDRInfo.CollectedAmt - bookHDRInfo.ForfeitedAmount) >= (bookHDRInfo.PaymentAmtEx1 + bookHDRInfo.PaymentAmtEx2 + bookHDRInfo.PaymentAmtEx3))
            {
                strHTML += "<tr><td>";
                strHTML += "<div class='labelDate'>" + "Pending for Payment" + "</div></td>";
                strHTML += "<td><span class='labelDate'>" + String.Format("{0:#,0.00}", (bookHDRInfo.TransTotalAmt - bookHDRInfo.CollectedAmt)) + " " + bookHDRInfo.Currency + "</span></td>";
                strHTML += "<td><span class='labelDate'>" + "Pending";
                strHTML += "</span></td></tr>";
            }
            strHTML += "</table>";
            divPaymentSchedule.InnerHtml = strHTML;
        }

        protected void LoadData()
        {
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;

            string GroupName = "";
            bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(TransID);
            objBooking.FillDataTableTransMain(bookHDRInfo);
            int transStatus = 0;

            //added by diana 20130923, update flight & passenger details

            bool callFunction = false;

            if (Session["generatePayment"] == null)
                callFunction = true;
            else if (Session["generatePayment"].ToString() == "")
                callFunction = true;

            if (Session["modePage"].ToString() == "agent")
            {
                if (Session["AgentSet"] != null)
                {
                    using (profiler.Step("ClearExpiredJourney"))
                    {
                        objBooking.ClearExpiredJourney(AgentSet.AgentID, TransID);
                    }
                }
                if (Session["AgentSet"] != null && callFunction == true)
                {
                    //commented by diana 20131114

                    //temp remarked navitaire update
                    //objBooking.UpdateBookingJourneyDetails(TransID, AgentSet.AgentName, AgentSet.AgentID, true);
                    //objBooking.UpdatePaymentDetails(TransID, AgentSet.AgentName, AgentSet.AgentID, true);

                    //replace the new get booking from Navitaire
                    List<ListTransaction> AllTransaction = new List<ListTransaction>();
                    using (profiler.Step("GetTransactionDetails"))
                    {
                        AllTransaction = objBooking.GetTransactionDetails(TransID);
                    }
                    if (AllTransaction != null && AllTransaction.Count > 0)
                    {
                        ListTransaction lstTrans = AllTransaction[0];
                        //change to new add-On table, Tyas
                        List<BookingTransactionDetail> VoidPNRs = new List<BookingTransactionDetail>();
                        List<BookingTransactionDetail> ExpiredPNRs = new List<BookingTransactionDetail>();
                        using (profiler.Step("UpdateAllBookingJourneyDetails"))
                        {
                            if (objBooking.UpdateAllBookingJourneyDetails(lstTrans, lstTrans.AgentUserName.ToString(), lstTrans.AgentID.ToString(), ref VoidPNRs, ref ExpiredPNRs, true, false) == false)
                            {
                                log.Warning(this, "Fail to Get Latest Update for Transaction - bookingdetail.ascx.cs : " + lstTrans.TransID);
                                if (ConfigurationManager.AppSettings["ErrorLogEmail"] != null && ConfigurationManager.AppSettings["ErrorLogEmail"] != "")
                                {
                                    eWS.CustomEmail(ConfigurationManager.AppSettings["ErrorLogEmail"].ToString(), lstTrans.TransID, "", "Errors Log :" + DateTime.Now.ToString() + " <br/> Fail to Get Latest Update for Transaction : " + lstTrans.TransID);
                                }
                            }
                        }
                        //if (objBooking.UpdateAllBookingJourneyDetailsNew(lstTrans, lstTrans.AgentUserName.ToString(), lstTrans.AgentID.ToString(), true) == false)
                        //{
                        //    log.Warning(this, "Fail to Get Latest Update for Transaction - bookingdetail.ascx.cs : " + lstTrans.TransID);
                        //    if (ConfigurationManager.AppSettings["ErrorLogEmail"] != null && ConfigurationManager.AppSettings["ErrorLogEmail"] != "")
                        //    {
                        //        eWS.CustomEmail(ConfigurationManager.AppSettings["ErrorLogEmail"].ToString(), lstTrans.TransID, "", "Errors Log :" + DateTime.Now.ToString() + " <br/> Fail to Get Latest Update for Transaction : " + lstTrans.TransID);
                        //    }
                        //}
                    }

                }
            }
            Session["generatePayment"] = "";

            byte TransStatus = 0;

            using (profiler.Step("GetTransStatus"))
            {
                TransStatus = objBooking.GetTransStatus(TransID);
            }
            if (TransStatus != 0)
            {
                bookHDRInfo.TransStatus = TransStatus;
            }

            if (Session["modePage"].ToString() == "agent")
            {
                if (Session["AgentSet"] != null && (bookHDRInfo.TransStatus == 2 || bookHDRInfo.TransStatus == 3))
                {
                    //temp remarked navitaire update
                    //objBooking.UpdatePassengerDetails(TransID, AgentSet.AgentName, AgentSet.AgentID, true);

                    //if (bookHDRInfo.TransStatus == 2)
                    {
                        //added by diana 20170405, check whether international or domestic
                        bool IsInt = false;
                        List<ABS.Logic.GroupBooking.Booking.BookingTransactionDetail> BKTransDetail = new List<ABS.Logic.GroupBooking.Booking.BookingTransactionDetail>();
                        if (Session["AgentSet"] != null)
                            using (profiler.Step("Get_TRANSDTL"))
                            {
                                BKTransDetail = objBooking.Get_TRANSDTL(AgentSet.AgentID, TransID);
                            }
                        else if (Session["AdminSet"] != null)
                            BKTransDetail = objBooking.Get_TRANSDTL(bookHDRInfo.AgentID, TransID);

                        if (BKTransDetail != null && BKTransDetail.Count > 0)
                        {
                            using (profiler.Step("IsInternationalFlight"))
                            {
                                IsInt = objGeneral.IsInternationalFlight(BKTransDetail[0].Origin, BKTransDetail[0].Destination, Request.PhysicalApplicationPath);
                            }
                            Session["BKTransDetail"] = BKTransDetail;
                        }


                        //added by diana 20140605, if passenger complete, then status should be 3 or confirmed
                        bool GetPassengerComplete = false;
                        //GetPassengerComplete = objBooking.CheckCompletePassenger(TransID, IsInt);
                        GetPassengerComplete = objBooking.CheckCompletePassenger70(TransID, IsInt);
                        if (GetPassengerComplete == true)
                        {
                            objBooking.UpdateTransMainStatus(TransID, 3);

                            TransStatus = 0;
                            TransStatus = objBooking.GetTransStatus(TransID);
                            if (TransStatus != 0)
                            {
                                bookHDRInfo.TransStatus = TransStatus;
                            }
                        }
                    }

                }
            }
            //end added by diana 20130923


            if (bookHDRInfo.TransStatus == 4 || bookHDRInfo.TransStatus == 6 || bookHDRInfo.TransStatus == 7)
            {
                //lstbookDTLInfo = objBooking.GetAllBK_TRANSDTLFilter(TransID, 0);
                lstRejectedbookDTLInfo = objBooking.GetAllBK_TRANSDTLFilter(TransID, 1);
                listDetailCombinePNR = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 1, "LEN(RecordLocator) >= 6 AND ");

                //FillDataTableTransDetail(lstbookDTLInfo); 
                objBooking.FillDataTableTransDetail(listDetailCombinePNR);
                lstbookFlightInfo = objBooking.GetAllBK_TRANSDTLFlight(TransID, 1);
                Session["BKTransDetail"] = lstbookFlightInfo;
            }
            else
            {
                if (bookHDRInfo.TransStatus == 9)
                {
                    lstRejectedbookDTLInfo = objBooking.GetAllBK_TRANSDTLFilter(TransID, 1);
                }
                else
                {
                    lstbookDTLInfo = objBooking.GetAllBK_TRANSDTLFilter(TransID, 0);
                }
                if (lstbookDTLInfo == null)
                {
                    lstRejectedbookDTLInfo = objBooking.GetAllBK_TRANSDTLFilter(TransID, 1);
                    listDetailCombinePNR = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 1, "LEN(RecordLocator) >= 6 AND ");
                }
                else
                    listDetailCombinePNR = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 0, "LEN(RecordLocator) >= 6 AND ");


                //FillDataTableTransDetail(lstbookDTLInfo); 
                objBooking.FillDataTableTransDetail(listDetailCombinePNR);
                lstbookFlightInfo = objBooking.GetAllBK_TRANSDTLFlight(TransID);

                Session["BKTransDetail"] = lstbookFlightInfo;
            }

            GroupName = objGeneral.getOPTGroupByCarrierCode(lstbookFlightInfo[0].CarrierCode);
            DataTable dtLimit = objBooking.GetSysPreftbyGrpID(GroupName, "LIMITTIME");
            if (dtLimit != null && dtLimit.Rows.Count > 0)
            {
                Session["dtLimit"] = dtLimit;
            }
            lstbookPaymentInfo = objBooking.GetAllBK_TRANSTENDERFilter(TransID, " BK_TRANSTENDER.TransVoid=0 AND ");

            Session["dtGridDetail"] = lstbookDTLInfo;
            Session["dtRejectedGridDetail"] = lstRejectedbookDTLInfo;

            Boolean returnFlight = false;
            returnFlight = objBooking.IsReturn(TransID, 0);
            Session["IsReturnFlight"] = returnFlight;

            lstFareBreakdown = objBooking.GetAllBK_TRANSDTLFareBreakdown(TransID, 0);
            //change to new add-On table, Tyas
            //dtFareBreakdown = objBooking.GetAllBK_TRANSDTLFareBreakdown3(TransID, 0, RecordLocator, returnFlight);
            dtFareBreakdown = objBooking.GetAllBK_TRANSDTLFareBreakdownNew(TransID, 0, RecordLocator, returnFlight);
            Session["dtFareBreakdown"] = dtFareBreakdown;
            lstFareBreakdownReturn = objBooking.GetAllBK_TRANSDTLFlightGrpNoSellKey1(TransID, 0);
            //change to new add-On table, Tyas
            //dtPass = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTable(TransID);
            dtPass = objBooking.GetAllBK_PASSENGERLISTDataTableNew(TransID,false);
            Session["dtPassenger"] = dtPass;

            dtInfant = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTableInfant(TransID);
            Session["dtInfant"] = dtInfant;

            //20170421 - Sienny (add info total pax)
            dtTotPax = objBooking.GetBK_TotalPax(TransID);
            if (dtTotPax != null && dtTotPax.Rows.Count > 0)
                lbl_num.Text = dtTotPax.Rows[0]["TotalPax"].ToString();
            else
                lbl_num.Text = "0";

            //change to new add-On table, Tyas
            //dtAddOn = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTableManage2(TransID, false, "", returnFlight);
            using (profiler.Step("GetAllBK_PASSENGERLISTWithSSRDataTableNewManage"))
            {
                dtAddOn = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTableNewManage(TransID, false, "");
            }
            Session["dtAddOn"] = dtAddOn;
        }

        protected void assignDefaultValue()
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(bookHDRInfo.TransID); //added by diana 20140108 - grab newest details
            lblBookingNum.Text = bookHDRInfo.TransID;
            lblTransID.Text = bookHDRInfo.TransID;

            int tempstatus = bookHDRInfo.TransStatus;
            int actualstatus = bookHDRInfo.TransStatus; //store actual status

            //added by diana 20140108 - check for needpaymentstatus
            if (tempstatus >= 2 && tempstatus <= 3)
            {
                if (bookHDRInfo.NeedPaymentStatus == 1) tempstatus = 1;
            }
            //end added by diana 20140108 - check for needpaymentstatus

            switch (tempstatus)
            {
                case 0:
                    lblStatus.Text = "Pending";
                    break;
                case 1:
                    lblStatus.Text = "Pending";
                    break;
                case 2:
                    lblStatus.Text = "Pending for Passenger Upload";
                    break;
                case 3:
                    lblStatus.Text = "Confirmed";
                    break;
                case 4:
                case 5:
                case 6:
                case 7:
                    lblStatus.Text = "Cancelled";
                    break;
            }
            if (bookHDRInfo.TransStatus >= 2 && bookHDRInfo.TransStatus <= 7)
                divPayment.Visible = true;

            //02 Feb 2017 - Sienny
            lblAmountPaid.Text = bookHDRInfo.CollectedAmt.ToString("N", nfi);
            lblAmountPaidCurrency.Text = bookHDRInfo.Currency;
            lblCurrentTotal.Text = bookHDRInfo.TransTotalAmt.ToString("N", nfi);
            lblCurrentTotalCurrency.Text = bookHDRInfo.Currency;

            lblTotalAmount.Text = (bookHDRInfo.TransTotalAmt - bookHDRInfo.CollectedAmt).ToString("N", nfi) + " " + bookHDRInfo.Currency;
            //02 Feb 2017 - Sienny


            //if (bookHDRInfo.TransTotalPAX != 0)
            //{
            //    lblAverageFare.Text = (bookHDRInfo.TransTotalAmt / bookHDRInfo.TransTotalPAX).ToString("N", nfi) + " " + bookHDRInfo.Currency;
            //}
            //else
            //{
            //    lblAverageFare.Text = "0.00" + bookHDRInfo.Currency;
            //}

            //lblDueAmount.Text = (bookHDRInfo.TransTotalAmt - bookHDRInfo.CollectedAmt).ToString("N", nfi) + " " + bookHDRInfo.Currency;
            //lblPax.Text = bookHDRInfo.TransTotalPAX.ToString();
            //lblAddOnCharge.Text = bookHDRInfo.TransTotalSSR.ToString("N", nfi) + " " + bookHDRInfo.Currency;

            //amended by diana 20131119 - to show booking date and expiry date
            lblBookingDate.Text = String.Format("{0:dd MMM yyyy HH:mm}", bookHDRInfo.BookingDate);
            if (actualstatus != 3) //check actual status - change tempstatus to actualstatus
                lblExpiryDate.Text = String.Format("{0:dd MMM yyyy HH:mm}", bookHDRInfo.ExpiryDate);
            else
                lblExpiryDate.Text = "-";
            //end amended by diana 20131119 - to show booking date and expiry date

            LoadFlight();

        }

        protected void LoadFlight()
        {
            ltrFlight.Text = "";
            ltrSummary.Text = "";
            ltrPayment.Text = "";
            string arrival = "";

            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            ltrFlight.Text += "<tr><td></td><td>&nbsp;&nbsp;<font color='red'>Departing &nbsp;</font></td><td>&nbsp;&nbsp<font color='red'>Arriving&nbsp;&nbsp;&nbsp;</font></td>";
            if (lstbookFlightInfo != null)
            {
                foreach (BookingTransactionDetail flight in lstbookFlightInfo)
                {
                    if (flight.RecordLocator.ToString().Trim().Length >= 6)
                    {
                        ltrFlight.Text += "<tr><td width='100px'>&nbsp;&nbsp;" + flight.CarrierCode + "&nbsp;" + flight.FlightNo + "</td>";
                        ltrFlight.Text += "<td> &nbsp;&nbsp;<b>" + flight.Origin + "</b>&nbsp;&nbsp;&nbsp";
                        if (flight.Transit != "")
                        {
                            arrival = flight.Destination;
                        }
                        else
                        {
                            arrival = flight.Destination;
                        }
                        //ltrFlight.Text += flight.DepatureDate.ToString("(dddd, dd MMM yyyy / HHmm)") + "&nbsp;<td> &nbsp;&nbsp;<b>" + flight.Destination + "</b>&nbsp;&nbsp;&nbsp;" + flight.ArrivalDate.ToString("(dddd, dd MMM yyyy / HHmm)") + "</td></td></tr>";
                        if (flight.Transit != "")
                        {
                            ltrFlight.Text += flight.DepatureDate.ToString("(dddd, dd MMM yyyy / HHmm)") + "&nbsp;<td> &nbsp;&nbsp;<b>" + arrival + "</b>&nbsp;&nbsp;&nbsp;" + flight.ArrivalDate2.ToString("(dddd, dd MMM yyyy / HHmm)") + "</td></td></tr>";
                            ltrFlight.Text += "<tr><td></td><td colspan = '2'>&nbsp;&nbsp;<font color='red'>Transit At " + flight.Transit + " ( " + flight.DepatureDate2.ToString("(dddd, dd MMM yyyy / HHmm)") + " - " + flight.ArrivalDate2.ToString("(dddd, dd MMM yyyy / HHmm)") + " )</font> </td></tr>";
                            //ltrFlight.Text += "<tr><td></td><td colspan = '2'>&nbsp;&nbsp;<b>" + flight.Origin + "</b> " + flight.DepatureDate.ToString("(dddd, dd MMM yyyy / HHmm)") + " - <b>" + flight.Transit + "</b> " + flight.ArrivalDate.ToString("(dddd, dd MMM yyyy / HHmm)") + "<br />&nbsp;&nbsp;<b>" + flight.Transit + "</b> " + flight.DepatureDate2.ToString("(dddd, dd MMM yyyy / HHmm)") + " - <b>" + flight.Destination + "</b>" + flight.ArrivalDate2.ToString("(dddd, dd MMM yyyy / HHmm)") + "</td></tr>";
                        }
                        else
                        {
                            ltrFlight.Text += flight.DepatureDate.ToString("(dddd, dd MMM yyyy / HHmm)") + "&nbsp;<td> &nbsp;&nbsp;<b>" + arrival + "</b>&nbsp;&nbsp;&nbsp;" + flight.ArrivalDate.ToString("(dddd, dd MMM yyyy / HHmm)") + "</td></td></tr>";
                        }


                        ltrSummary.Text += "<tr><td>&nbsp;&nbsp;<b>" + flight.Origin + "-" + flight.Destination + "</b> </td><td></td></tr>";
                        ltrSummary.Text += "<tr><td>&nbsp;&nbsp;Flight Fare</td><td align='right'>" + flight.LineFlight.ToString("N", nfi) + " " + flight.Currency + "&nbsp;&nbsp;</td></tr>";
                        ltrSummary.Text += "<tr><td>&nbsp;&nbsp;Taxes & Fees</td><td align='right'>" + flight.LineTax.ToString("N", nfi) + " " + flight.Currency + "&nbsp;&nbsp;</td></tr>";
                        ltrSummary.Text += "<tr><td>&nbsp;&nbsp;</td><td align='right'><b>" + (Convert.ToDecimal(flight.LineTotal)).ToString("N", nfi) + " " + flight.Currency + "</b>&nbsp;&nbsp;</td></tr>";
                    }
                }
            }
            LoadPaymentHistory(bookHDRInfo.TransID);

            ltrSummary.Text += "<tr><td>&nbsp;&nbsp;<b>Total Booking</b></td><td align='right'><b>" + bookHDRInfo.TransTotalAmt.ToString("N", nfi) + " " + bookHDRInfo.Currency + "</b>&nbsp;&nbsp;</td></tr>";

        }

        protected void gridCustomers_CustomColumnSort(object sender, EventArgs e)
        {
            gvClass.GroupSummarySortInfo.Clear();

            ASPxGroupSummarySortInfo sortInfo = new ASPxGroupSummarySortInfo();

            sortInfo.SortOrder = ColumnSortOrder.Ascending;

            sortInfo.SummaryItem = gvClass.GroupSummary["SeqNo", SummaryItemType.Max];

            sortInfo.GroupColumn = "Journey";

            gvClass.GroupSummarySortInfo.AddRange(sortInfo);
            gvClass.GroupSummarySortInfo.AddRange(new ASPxGroupSummarySortInfo("Journey", gvClass.GroupSummary["SeqNo"], ColumnSortOrder.Ascending));
        }

        protected void gvRejectedClass_CustomColumnSort(object sender, EventArgs e)
        {
            gvRejectedClass.GroupSummarySortInfo.Clear();

            ASPxGroupSummarySortInfo sortInfo = new ASPxGroupSummarySortInfo();

            sortInfo.SortOrder = ColumnSortOrder.Ascending;

            sortInfo.SummaryItem = gvRejectedClass.GroupSummary["SeqNo", SummaryItemType.Max];

            sortInfo.GroupColumn = "Journey";

            gvRejectedClass.GroupSummarySortInfo.AddRange(sortInfo);
            gvRejectedClass.GroupSummarySortInfo.AddRange(new ASPxGroupSummarySortInfo("Journey", gvRejectedClass.GroupSummary["SeqNo"], ColumnSortOrder.Ascending));
        }

        protected void LoadGridView()
        {
            try
            {
                gvClass.DataSource = lstbookDTLInfo;
                gvClass.DataBind();
                gvClass.ExpandAll();

                //load rejected transaction details
                if (lstRejectedbookDTLInfo != null)
                {
                    //Added by Ellis 20170316, to hide Rejected Fare Details if got no data
                    if (lstRejectedbookDTLInfo.Count > 0)
                    {
                        divRejectedFare.Style["display"] = "";
                        gvRejectedClass.DataSource = lstRejectedbookDTLInfo;
                        gvRejectedClass.DataBind();
                        gvRejectedClass.ExpandAll();
                    }
                }

                //load gvPassenger
                if (dtPass != null)
                {
                    gvPassenger.DataSource = dtPass;
                    gvPassenger.DataBind();
                    gvPassenger.ExpandAll();
                }
                //load gvInfant
                if (dtInfant != null)
                {
                    InfantDiv.Style.Add("display","block");
                    gvInfant.DataSource = dtInfant;
                    gvInfant.DataBind();
                    gvInfant.ExpandAll();
                }
                //load gvAddOn
                if (dtAddOn != null)
                {
                    gvAddOn.DataSource = dtAddOn;
                    gvAddOn.DataBind();
                    gvAddOn.ExpandAll();
                }

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                throw ex;
            }
        }

        protected void LoadGridViewCallBack()
        {
            // gvClass.GroupSummarySortInfo.AddRange(new ASPxGroupSummarySortInfo("Journey", gvClass.GroupSummary["SeqNo"], ColumnSortOrder.Ascending));
            gvClass.DataSource = (List<BookingTransactionDetail>)Session["dtGridDetail"];
            gvClass.DataBind();
            gvClass.ExpandAll();
        }

        protected void LoadRejectedGridViewCallBack()
        {
            // gvClass.GroupSummarySortInfo.AddRange(new ASPxGroupSummarySortInfo("Journey", gvClass.GroupSummary["SeqNo"], ColumnSortOrder.Ascending));
            gvRejectedClass.DataSource = (List<BookingTransactionDetail>)Session["dtRejectedGridDetail"];
            gvRejectedClass.DataBind();
            gvRejectedClass.ExpandAll();
        }

        protected void LoadPassengerGridViewCallBack()
        {
            //added by bernad 20170118
            List<ABS.Logic.GroupBooking.Booking.BookingTransactionDetail> BookingTransactionDetail;
            //BookingTransactionDetail = objBooking.Get_TRANSDTL(AgentSet.AgentID, TransID);
            //BookingTransactionDetail = objBooking.Get_TRANSDTL("", TransID);
            BookingTransactionDetail = null;
            Session["BookingTransactionDetail"] = BookingTransactionDetail;
            if (Session["AgentSet"] != null)
                BookingTransactionDetail = objBooking.Get_TRANSDTL(AgentSet.AgentID, TransID);
            if (Session["AdminSet"] != null)
                BookingTransactionDetail = objBooking.Get_TRANSDTL(bookHDRInfo.AgentID, TransID);

            Boolean returnFlight = false;
            returnFlight = objBooking.IsReturn(TransID, 0);

            if (BookingTransactionDetail != null && BookingTransactionDetail.Count > 0)
            {
                if (returnFlight)
                {
                    gvPassenger.Columns["ReturnSeat"].Visible = true;
                    if (Regex.Replace(BookingTransactionDetail[0].Transit, @"\s+", "") != "")
                    {
                        gvPassenger.Columns["DepartConnectingSeat"].Visible = true;
                        gvPassenger.Columns["ReturnConnectingSeat"].Visible = true;

                        gvPassenger.Columns["DepartSeat"].Caption = BookingTransactionDetail[0].Origin + " - " + BookingTransactionDetail[0].Transit;
                        gvPassenger.Columns["DepartConnectingSeat"].Caption = BookingTransactionDetail[0].Transit + " - " + BookingTransactionDetail[0].Destination;

                        gvPassenger.Columns["ReturnSeat"].Caption = BookingTransactionDetail[0].Destination + " - " + BookingTransactionDetail[0].Transit;
                        gvPassenger.Columns["ReturnConnectingSeat"].Caption = BookingTransactionDetail[0].Transit + " - " + BookingTransactionDetail[0].Origin;

                    }
                    else
                    {
                        gvPassenger.Columns["DepartConnectingSeat"].Visible = false;
                        gvPassenger.Columns["ReturnConnectingSeat"].Visible = false;

                        gvPassenger.Columns["DepartSeat"].Caption = BookingTransactionDetail[0].Origin + " - " + BookingTransactionDetail[0].Destination;
                        gvPassenger.Columns["ReturnSeat"].Caption = BookingTransactionDetail[0].Destination + " - " + BookingTransactionDetail[0].Origin;
                    }
                }
                else
                {
                    gvPassenger.Columns["ReturnSeat"].Visible = false;
                    if (Regex.Replace(BookingTransactionDetail[0].Transit, @"\s+", "") != "")
                    {
                        gvPassenger.Columns["DepartConnectingSeat"].Visible = true;
                        gvPassenger.Columns["ReturnConnectingSeat"].Visible = false;

                        gvPassenger.Columns["DepartSeat"].Caption = BookingTransactionDetail[0].Origin + " - " + BookingTransactionDetail[0].Transit;
                        gvPassenger.Columns["DepartConnectingSeat"].Caption = BookingTransactionDetail[0].Transit + " - " + BookingTransactionDetail[0].Destination;
                    }
                    else
                    {
                        gvPassenger.Columns["DepartConnectingSeat"].Visible = false;
                        gvPassenger.Columns["ReturnConnectingSeat"].Visible = false;

                        gvPassenger.Columns["DepartSeat"].Caption = BookingTransactionDetail[0].Origin + " - " + BookingTransactionDetail[0].Destination;
                    }
                }


                //20170406 - Sienny (domestic flight hide PassportNo and ExpiryDate field)
                IsInternationalFlight = objGeneral.IsInternationalFlight(BookingTransactionDetail[0].Origin, BookingTransactionDetail[0].Destination, Request.PhysicalApplicationPath);
                DataTable dtPsgr = (DataTable)Session["dtPassenger"];
                if (!IsInternationalFlight)
                {
                    if (dtPsgr != null && dtPsgr.Rows.Count > 0)
                    {
                        int numberOfRecords = dtPsgr.Select("PassportNo = '' AND ExpiryDate IS NULL").Length;
                        //cek if passportno and expirydate are not empty
                        if (numberOfRecords == dtPsgr.Rows.Count)
                        {
                            gvPassenger.Columns["PassportNo"].Visible = false;
                            gvPassenger.Columns["ExpiryDate"].Visible = false;
                        }
                        else
                        {
                            gvPassenger.Columns["PassportNo"].Visible = true;
                            gvPassenger.Columns["ExpiryDate"].Visible = true;
                        }
                    }
                }
                else
                {
                    gvPassenger.Columns["PassportNo"].Visible = true;
                    gvPassenger.Columns["ExpiryDate"].Visible = true;
                }
            }

            //end added

            gvPassenger.DataSource = (DataTable)Session["dtPassenger"];
            gvPassenger.DataBind();
            gvPassenger.ExpandAll();

            if ((DataTable)Session["dtInfant"] != null)
               InfantDiv.Style.Add("display", "block");
            gvInfant.DataSource = (DataTable)Session["dtInfant"];
            gvInfant.DataBind();
        }

        protected void LoadAddOnGridViewCallBack()
        {
            System.Data.DataTable GetConfirm = null;//added by romy, 20170815, Insurance
            List<ABS.Logic.GroupBooking.Booking.BookingTransactionDetail> BookingTransactionDetail = new List<BookingTransactionDetail>();
            //BookingTransactionDetail = objBooking.Get_TRANSDTL(AgentSet.AgentID, TransID);
            if (Session["BookingTransactionDetail"] != null)
            {
                BookingTransactionDetail = (List<ABS.Logic.GroupBooking.Booking.BookingTransactionDetail>)Session["BookingTransactionDetail"];
            }
            else
            {
                if (Session["AgentSet"] != null)
                    BookingTransactionDetail = objBooking.Get_TRANSDTL(AgentSet.AgentID, TransID);
                if (Session["AdminSet"] != null)
                    BookingTransactionDetail = objBooking.Get_TRANSDTL(bookHDRInfo.AgentID, TransID);

            }
            //fixed by ketee, 20171015
            if (Session["AgentSet"] != null)
                GetConfirm = objBooking.GetUpcomingFlightInsure(AgentSet.AgentID.ToString(), TransID.ToString());
            if (Session["AdminSet"] != null && bookHDRInfo != null)
                GetConfirm = objBooking.GetUpcomingFlightInsure(bookHDRInfo.AgentID, TransID.ToString());//added by romy, 20170815, Insurance

            //if (AgentSet != null && TransID.ToString() != "")
            //{
            //    GetConfirm = objBooking.GetUpcomingFlightInsure(AgentSet.AgentID.ToString(), TransID.ToString());//added by romy, 20170815, Insurance
            //}

            if (GetConfirm == null)
            {
                gvAddOn.Columns["DepartInsure"].Visible = false;
                gvAddOn.Columns["ReturnInsure"].Visible = false;
            }
            else if (GetConfirm != null && GetConfirm.Rows[0]["UpcomingFlight"].ToString() != "3")
            {
                gvAddOn.Columns["DepartInsure"].Visible = false;
                gvAddOn.Columns["ReturnInsure"].Visible = false;
            }
            //added by diana 20170331, to hide return columns if one way flight
            if (Session["IsReturnFlight"] != null && (Boolean)Session["IsReturnFlight"] == false)
            {
                gvAddOn.Columns["ReturnMeal"].Visible = false;
                gvAddOn.Columns["ReturnBaggage"].Visible = false;
                gvAddOn.Columns["ReturnSport"].Visible = false;
                gvAddOn.Columns["ReturnInsure"].Visible = false;
                gvAddOn.Columns["ReturnComfort"].Visible = false;
                gvAddOn.Columns["ReturnInfant"].Visible = false;
            }
            if (BookingTransactionDetail[0].Transit.ToString().Trim() != "")
            {
                gvAddOn.Columns["ConDepartMeal"].Visible = true;
            }
            else
            {
                gvAddOn.Columns["ConDepartMeal"].Visible = false;
            }
            if (Session["IsReturnFlight"] != null && (Boolean)Session["IsReturnFlight"] != false && BookingTransactionDetail[1].Transit.ToString().Trim() != "")
            {
                gvAddOn.Columns["ConReturnMeal"].Visible = true;
            }
            else
            {
                gvAddOn.Columns["ConReturnMeal"].Visible = false;
            }
            gvAddOn.DataSource = (DataTable)Session["dtAddOn"];
            gvAddOn.DataBind();
            gvAddOn.ExpandAll();
        }

        protected void LoadFareBreakdown(string PNR)
        {
            //gvFareBreakdown.DataSource = (DataTable)Session["dtFareBreakdown"];
            //gvFareBreakdown.DataBind();
            //gvFareBreakdown.ExpandAll();
            Boolean returnFlight = false;
            DataTable dtOth = new DataTable();
            returnFlight = objBooking.IsReturn(TransID, 0);
            if (Session["PNR"] != null)
                PNR = Session["PNR"].ToString();
            else
                PNR = "";

            if (PNR == "" || PNR == "ALL")
            {
                //lstFareBreakdown = objBooking.GetAllBK_TRANSDTLFareBreakdown(TransID, 0);
                //change to new add-On table, Tyas
                //dtFareBreakdown = objBooking.GetAllBK_TRANSDTLFareBreakdown3(TransID, 0, "", returnFlight);
                dtFareBreakdown = objBooking.GetAllBK_TRANSDTLFareBreakdownNew(TransID, 0, "", returnFlight);

            }
            else
            {
                //load fare by PNR
                //lstFareBreakdown = objBooking.GetAllBK_TRANSDTLFareBreakdown(TransID, 0);
                //change to new add-On table, Tyas
                //dtFareBreakdown = objBooking.GetAllBK_TRANSDTLFareBreakdown3(TransID, 0, PNR, returnFlight);
                dtFareBreakdown = objBooking.GetAllBK_TRANSDTLFareBreakdownNew(TransID, 0, PNR, returnFlight);
            }
            Session["dtFareBreakdown"] = dtFareBreakdown;
            //dtOth = objBooking.GetTransFees(TransID);
            dtOth = objGeneral.GetAllFeesData(TransID);
            Session["dtOth"] = dtOth;
            DataTable dtDataFeeCodeCopy = new DataTable();

            List<ABS.Logic.GroupBooking.Booking.BookingTransactionDetail> BKTransDetail = new List<ABS.Logic.GroupBooking.Booking.BookingTransactionDetail>();
            if (Session["BKTransDetail"] != null)
            {
                BKTransDetail = (List<ABS.Logic.GroupBooking.Booking.BookingTransactionDetail>)Session["BKTransDetail"];
            }
            //DataRow[] foundRows = dtFareBreakdown.Select("", "", DataViewRowState.Added);

            int num = 0;
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            if (dtFareBreakdown != null)
            {
                divFareBreakdown.Visible = true;

                if (dtFareBreakdown.Rows.Count > 0)
                {
                    num = Convert.ToInt32(dtFareBreakdown.Rows[0]["PaxAdultDepart"].ToString()) + Convert.ToInt32(dtFareBreakdown.Rows[0]["PaxChildDepart"].ToString());

                    lbl_num1.Text = num.ToString();
                    //if (dtFareBreakdown.Rows[0]["NameChangeFee"] != DBNull.Value)
                    //{
                    //    //lblSSRTotalDepart.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineSSRDepart"])).ToString("N", nfi);
                    //    lbl_NameChangeTotal.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["NameChangeFee"])).ToString("N", nfi);
                    //}
                    //else
                    //{
                    //    lbl_NameChangeTotal.Text = "0.0";
                    //}
                    lbl_NameChangeTotal.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineNameChangeDepart"])).ToString("N", nfi);
                    lbl_currency01.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    if (lbl_NameChangeTotal.Text == "0.00")
                    {
                        NameChangeBreakdown.Style.Add("display", "none");
                    }

                    //added by romy for insure
                    lbl_InsureFeeTotal.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineInsureFee"])).ToString("N", nfi);
                    lbl_currency02.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    if (lbl_InsureFeeTotal.Text == "0.00")
                    {
                        InsureBreakdown.Style.Add("display", "none");
                    }

                    //if (Convert.ToDecimal(dtFareBreakdown.Rows[0]["PaxChild"]) != 0) // && (temFlight.TemFlightDeparture.ToString().ToUpper() == "HKG" || temFlight.TemFlightArrival.ToString().ToUpper() == "HKG"))
                    //{
                    //    lbl_num2.Text = "Adult Airport Tax ";
                    //    lbl_num2CHD.Text = dtFareBreakdown.Rows[0]["PaxChild"].ToString();
                    //    trAptCHD.Visible = true;
                    //}
                    //else
                    //{
                    //    lbl_num2.Text = "Airport Tax ";
                    //    trAptCHD.Visible = false;
                    //}
                    //lbl_num3.Text = num.ToString();
                    //lbl_num21.Text = num.ToString();
                    lbl_num4.Text = num.ToString();
                    //lbl_num5.Text = num.ToString();
                    lbl_num6.Text = num.ToString();

                    lbl_FlightTotal.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["FareAmountDepart"])).ToString("N", nfi);
                    lbl_Average.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["FarePerPaxDepart"])).ToString("N", nfi);

                    lbl_taxPrice.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineTaxDepart"])).ToString("N", nfi);
                    lbl_taxTotal.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineTaxDepart"])).ToString("N", nfi);

                    lbl_PaxFeePrice.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LinePaxFeeDepart"]) / num).ToString("N", nfi);
                    lbl_PaxFeeTotal.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LinePaxFeeDepart"])).ToString("N", nfi);

                    //lblFuelPriceOneDepart.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineCharge"]) / num).ToString("N", nfi);
                    //lblFuelPriceTotalDepart.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineCharge"])).ToString("N", nfi);

                    lblSvcChargeOneDepart.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineFeeDepart"]) / num).ToString("N", nfi);
                    lblSvcChargeTotalDepart.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineFeeDepart"])).ToString("N", nfi);

                    //lblVATDepart.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineVat"]) / num).ToString("N", nfi);
                    //lblVATTotalDepart.Text = Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineVat"]).ToString("N", nfi);

                    lblSSRTotalDepart.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineSSRDepart"])).ToString("N", nfi);
                    //if (dtFareBreakdown.Rows[0]["PriceDepartBaggage"] != DBNull.Value)
                    //{
                    //    lblBaggageTotalDepart.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["PriceDepartBaggage"]) * num).ToString("N", nfi);
                    //}
                    //else
                    //{
                    //    lblBaggageTotalDepart.Text = "0.0";
                    //}

                    //if (dtFareBreakdown.Rows[0]["PriceDepartMeal"] != DBNull.Value)
                    //{
                    //    lblMealTotalDepart.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["PriceDepartMeal"]) * num).ToString("N", nfi);
                    //}
                    //else
                    //{
                    //    lblMealTotalDepart.Text = "0.0";
                    //}


                    //if (dtFareBreakdown.Rows[0]["PriceDepartSport"] != DBNull.Value)
                    //{
                    //    lblSportTotalDepart.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["PriceDepartSport"]) * num).ToString("N", nfi);
                    //}
                    //else
                    //{
                    //    lblSportTotalDepart.Text = "0.0";
                    //}


                    //if (dtFareBreakdown.Rows[0]["PriceDepartComfort"] != DBNull.Value)
                    //{
                    //    lblComfortTotalDepart.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["PriceDepartComfort"]) * num).ToString("N", nfi);
                    //}
                    //else
                    //{
                    //    lblComfortTotalDepart.Text = "0.0";
                    //}

                    if (dtFareBreakdown.Rows[0]["LineInfantDepart"] != DBNull.Value)
                    {
                        lbl_InfantTotal.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineInfantDepart"])).ToString("N", nfi);
                    }
                    else
                    {
                        lbl_InfantTotal.Text = "0.0";
                    }

                    lblSeatTotalDepart.Text = Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineSeatDepart"]).ToString("N", nfi);

                    lblPromoDiscOneDepart.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LinePromoDiscDepart"]) / num).ToString("N", nfi);
                    lblPromoDiscTotalDepart.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LinePromoDiscDepart"])).ToString("N", nfi);

                    if (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LinePromoDiscDepart"]) < 0)
                    {
                        trPromoDiscDepart.Visible = true;
                    }
                    else
                    {
                        trPromoDiscDepart.Visible = false;
                    }

                    lblOthOneDepart.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineOthDepart"]) / num + Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineDiscDepart"]) / num).ToString("N", nfi);
                    lblOthTotalDepart.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineOthDepart"]) + Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineProcessDepart"]) + Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineDiscDepart"])).ToString("N", nfi);
                    lblProcessTotalDepart.Text = Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineProcessDepart"]).ToString("N", nfi);
                    decimal TotalDepart = 0;
                    if (dtFareBreakdown.Rows[0]["LineInfantDepart"] != DBNull.Value)
                    {
                        TotalDepart = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineInfantDepart"])) + (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineNameChangeDepart"])) + (Convert.ToDecimal(dtFareBreakdown.Rows[0]["FareAmountDepart"])) + (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineTaxDepart"])) + (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LinePaxFeeDepart"])) + (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineFeeDepart"])) + (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineSSRDepart"])) + Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineSeatDepart"]) + (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineOthDepart"]) + Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineProcessDepart"]) + Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineDiscDepart"]) + Convert.ToDecimal(dtFareBreakdown.Rows[0]["LinePromoDiscDepart"]));
                    }
                    else
                    {
                        TotalDepart = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineNameChangeDepart"])) + (Convert.ToDecimal(dtFareBreakdown.Rows[0]["FareAmountDepart"])) + (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineTaxDepart"])) + (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LinePaxFeeDepart"])) + (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineFeeDepart"])) + (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineSSRDepart"])) + Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineSeatDepart"]) + (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineOthDepart"]) + Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineProcessDepart"]) + Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineDiscDepart"]) + Convert.ToDecimal(dtFareBreakdown.Rows[0]["LinePromoDiscDepart"]));
                    }

                    if (TotalDepart != 0 && Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineTotalDepart"]) != TotalDepart)
                    {
                        lbl_Total.Text = TotalDepart.ToString("N", nfi);
                    }
                    else
                    {
                        lbl_Total.Text = Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineTotalDepart"]).ToString("N", nfi);
                    }

                    lbl_currency14.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();

                    lbl_currency.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lbl_currency0.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    //lbl_currency1.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lbl_currency2.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lbl_currency21.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lbl_currency3.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lbl_currency31.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    //if (Convert.ToDecimal(temFlight.TemFlightCHDNum) != 0)
                    //{
                    //    lbl_currency2CHD.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    //    lbl_currency3CHD.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    //}
                    //lbl_currency4.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    Label4.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lbl_currency12.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lbl_currency5.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    //lbl_currency6.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lbl_currency7.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    //lblCurrFuelDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lblCurrOthDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lblCurrSvcDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    //lblCurrVATDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();

                    lblCurrSSRDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    //lblCurrBaggageDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    //lblCurrMealDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    //lblCurrSportDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    //lblCurrComfortDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();

                    lblCurrSeatDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lblCurrSeatReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();

                    lblCurrPromoDiscDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();

                    lbl_currency2.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lbl_currency21.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lblCurrAVLDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lblCurrPSFDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lblCurrSCFDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lblCurrConnectingDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lblCurrDiscDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lblCurrKlia2Depart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lblCurrGSTDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lblCurrSPLDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lblCurrCHGTotalChargeDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lblCurrProcessDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lblCurrSPLDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lblCurrAPSDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lblCurrIADFDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lblCurrCSTDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lblCurrCUTDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lblCurrSGIDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lblCurrSSTDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lblCurrUDFDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lblCurrASCDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    lblCurrBCLDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();

                    //RETURN
                    if (returnFlight)
                    {
                        ShowReturnColumn(true);

                        lbl_num1.Text = num.ToString();
                        //lbl_NameChangeTotal.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["NameChangeFee"])).ToString("N", nfi);

                        //if (Convert.ToDecimal(dtFareBreakdown.Rows[1]["PaxChild"]) != 0) // && (temFlight.TemFlightDeparture.ToString().ToUpper() == "HKG" || temFlight.TemFlightArrival.ToString().ToUpper() == "HKG"))
                        //{
                        //    lbl_num2.Text = "Adult Airport Tax ";
                        //    lbl_num2CHD.Text = dtFareBreakdown.Rows[1]["PaxChild"].ToString();
                        //    trAptCHD.Visible = true;
                        //}
                        //else
                        //{
                        //    lbl_num2.Text = "Airport Tax ";
                        //    trAptCHD.Visible = false;
                        //}
                        //lbl_num3.Text = num.ToString();
                        lbl_num4.Text = num.ToString();
                        //lbl_num5.Text = num.ToString();
                        lbl_num6.Text = num.ToString();
                        if (dtFareBreakdown.Rows[0]["FareAmountReturn"] != DBNull.Value)
                        {
                            lbl_InFlightTotal.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["FareAmountReturn"])).ToString("N", nfi);
                        }
                        if (dtFareBreakdown.Rows[0]["FarePerPaxReturn"] != DBNull.Value)
                        {
                            lbl_InAverage.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["FarePerPaxReturn"])).ToString("N", nfi);
                        }
                        if (dtFareBreakdown.Rows[0]["LineTaxReturn"] != DBNull.Value)
                        {
                            lbl_IntaxPrice.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineTaxReturn"])).ToString("N", nfi);
                            lbl_IntaxTotal.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineTaxReturn"])).ToString("N", nfi);
                        }

                        //lblFuelOneReturn.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[1]["LineCharge"]) / num).ToString("N", nfi);
                        //lblFuelTotalReturn.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[1]["LineCharge"])).ToString("N", nfi);
                        if (dtFareBreakdown.Rows[0]["LinePaxFeeReturn"] != DBNull.Value)
                        {
                            lbl_InPaxFeePrice.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LinePaxFeeReturn"]) / num).ToString("N", nfi);
                            lbl_InPaxFeeTotal.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LinePaxFeeReturn"])).ToString("N", nfi);
                        }

                        if (dtFareBreakdown.Rows[0]["LineFeeReturn"] != DBNull.Value)
                        {
                            lblSvcOneReturn.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineFeeReturn"]) / num).ToString("N", nfi);
                            lblSvcTotalReturn.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineFeeReturn"])).ToString("N", nfi);
                        }

                        //lblVATReturn.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[1]["LineVat"]) / num).ToString("N", nfi);
                        //lblVATTotalReturn.Text = Convert.ToDecimal(dtFareBreakdown.Rows[1]["LineVat"]).ToString("N", nfi);
                        if (dtFareBreakdown.Rows[0]["LineSSRReturn"] != DBNull.Value)
                        {
                            lblSSRTotalReturn.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineSSRReturn"])).ToString("N", nfi);
                        }
                        //if (dtFareBreakdown.Rows[0]["PriceReturnBaggage"] != DBNull.Value)
                        //{
                        //    lblBaggageTotalReturn.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["PriceReturnBaggage"]) * num).ToString("N", nfi);
                        //}
                        //else
                        //{
                        //    lblBaggageTotalReturn.Text = "0.0";
                        //}

                        //if (dtFareBreakdown.Rows[0]["PriceReturnMeal"] != DBNull.Value)
                        //{
                        //    lblMealTotalReturn.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["PriceReturnMeal"]) * num).ToString("N", nfi);

                        //}
                        //else
                        //{
                        //    lblMealTotalReturn.Text = "0.0";
                        //}

                        //if (dtFareBreakdown.Rows[0]["PriceReturnSport"] != DBNull.Value)
                        //{

                        //    lblSportTotalReturn.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["PriceReturnSport"]) * num).ToString("N", nfi);

                        //}
                        //else
                        //{
                        //    lblSportTotalReturn.Text = "0.0";
                        //}

                        //if (dtFareBreakdown.Rows[0]["PriceReturnComfort"] != DBNull.Value)
                        //{
                        //    lblComfortTotalReturn.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["PriceReturnComfort"]) * num).ToString("N", nfi);
                        //}
                        //else
                        //{
                        //    lblComfortTotalReturn.Text = "0.0";
                        //}

                        if (dtFareBreakdown.Rows[0]["LineInfantReturn"] != DBNull.Value)
                        {
                            lbl_InInfantTotal.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineInfantReturn"])).ToString("N", nfi);
                        }
                        else
                        {
                            lbl_InInfantTotal.Text = "0.0";
                        }

                        if (dtFareBreakdown.Rows[0]["LineSeatReturn"] != DBNull.Value)
                        {
                            lblSeatTotalReturn.Text = Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineSeatReturn"]).ToString("N", nfi);
                        }

                        if (dtFareBreakdown.Rows[0]["LinePromoDiscReturn"] != DBNull.Value)
                        {
                            lblPromoDiscOneReturn.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LinePromoDiscReturn"]) / num).ToString("N", nfi);
                            lblPromoDiscTotalReturn.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LinePromoDiscReturn"])).ToString("N", nfi);
                        }

                        if (dtFareBreakdown.Rows[0]["LinePromoDiscReturn"] != DBNull.Value)
                        {
                            if (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LinePromoDiscReturn"]) < 0)
                            {
                                trPromoDiscReturn.Visible = true;
                            }
                            else
                            {
                                trPromoDiscReturn.Visible = false;
                            }
                        }

                        if (dtFareBreakdown.Rows[0]["LineOthReturn"] != DBNull.Value)
                        {
                            lblOthOneReturn.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineOthReturn"]) / num + Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineDiscReturn"]) / num).ToString("N", nfi);
                            lblOthTotalReturn.Text = (Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineOthReturn"]) + Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineProcessReturn"]) + Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineDiscReturn"])).ToString("N", nfi);
                        }
                        if (dtFareBreakdown.Rows[0]["LineInfantReturn"] != DBNull.Value)
                        {
                            lbl_InTotal.Text = Convert.ToDecimal(dtFareBreakdown.Rows[0]["LineTotalReturn"]).ToString("N", nfi);
                        }

                        lbl_InCurrency14.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();

                        lbl_InCurrency.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        lbl_InCurrency0.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        lbl_Incurrency1.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        lbl_Incurrency2.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        lbl_Incurrency21.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        lbl_InCurrency3.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        lbl_InCurrency31.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        lbl_Incurrency12.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        //if (Convert.ToDecimal(temFlight.TemFlightCHDNum) != 0) // && (temFlight.TemFlightDeparture.ToString().ToUpper() == "HKG" || temFlight.TemFlightArrival.ToString().ToUpper() == "HKG"))
                        //{
                        //    lbl_Incurrency2CHD.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        //    lbl_InCurrency3CHD.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        //}
                        //lbl_InCurrency4.Text = dtFareBreakdown.Rows[1]["Currency"].ToString();
                        lbl_InCurrency5.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        //lbl_InCurrency6.Text = dtFareBreakdown.Rows[1]["Currency"].ToString();
                        lbl_InCurrency7.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        //lblCurrFuelReturn.Text = dtFareBreakdown.Rows[1]["Currency"].ToString();
                        lblCurrOthReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        lblCurrSvcReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        //lblCurrVATReturn.Text = dtFareBreakdown.Rows[1]["Currency"].ToString();

                        lblCurrSSRReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        //lblCurrBaggageReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        //lblCurrMealReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        //lblCurrSportReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        //lblCurrComfortReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        //lblCurrBaggageReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        //lblCurrMealReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        //lblCurrSportReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        //lblCurrComfortReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();

                        lblCurrPromoDiscReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();

                        lbl_Incurrency2.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        lbl_Incurrency21.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        lblCurrAVLReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        lblCurrPSFReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        lblCurrSCFReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        lblCurrConnectingReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        lblCurrDiscReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        lblCurrKlia2Return.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        lblCurrGSTReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        //lblCurrSPLReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        lblCurrCHGTotalReturnn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        lblCurrSPLReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        lblCurrAPSReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        lblCurrIADFReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        lblCurrCSTReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        lblCurrCUTReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        lblCurrSGIReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        lblCurrSSTReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        lblCurrUDFReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        lblCurrASCReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                        lblCurrBCLReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                    }
                    else
                    {
                        ShowReturnColumn(false);
                    }

                    if (dtOth != null && dtOth.Rows.Count > 0)
                    {
                        DataRow[] rowsFeeCode = dtOth.Select("Origin = '" + BKTransDetail[0].Origin.ToString() + "'");
                        if (rowsFeeCode.Length > 0)
                        {
                            dtDataFeeCodeCopy = dtOth.Select("Origin = '" + BKTransDetail[0].Origin.ToString() + "'").CopyToDataTable();
                            rptFeeDepart.DataSource = dtDataFeeCodeCopy;
                            rptFeeDepart.DataBind();
                            foreach (RepeaterItem item in rptFeeDepart.Items)
                            {
                                Label lblFeeCurrDepart = item.FindControl("lblFeeCurrDepart") as Label;
                                lblFeeCurrDepart.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                            }
                        }

                        if (returnFlight)
                        {
                            rowsFeeCode = dtOth.Select("Origin = '" + BKTransDetail[0].Destination.ToString() + "'");
                            if (rowsFeeCode.Length > 0)
                            {
                                dtDataFeeCodeCopy = dtOth.Select("Origin = '" + BKTransDetail[0].Destination.ToString() + "'").CopyToDataTable();
                                rptFeeReturn.DataSource = dtDataFeeCodeCopy;
                                rptFeeReturn.DataBind();
                                foreach (RepeaterItem item in rptFeeReturn.Items)
                                {
                                    Label lblFeeCurrReturn = item.FindControl("lblFeeCurrReturn") as Label;
                                    lblFeeCurrReturn.Text = dtFareBreakdown.Rows[0]["Currency"].ToString();
                                }
                            }
                        }

                        //for (int i = 0; i < dtOth.Rows.Count; i++)
                        //{
                        //    if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "KLIA2" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Origin.ToString())
                        //    {
                        //        //tdDepartKlia2.Visible = true;
                        //        lbl_klia2Total.Text = (Convert.ToDecimal(lbl_klia2Total.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //    }
                        //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "GST" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Origin.ToString())
                        //    {
                        //        //tdDepartGST.Visible = true;
                        //        lbl_GSTTotal.Text = (Convert.ToDecimal(lbl_GSTTotal.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //    }
                        //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "AVL" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Origin.ToString())
                        //    {
                        //        //tdDepartAVL.Visible = true;
                        //        lblAVLTotalDepart.Text = (Convert.ToDecimal(lblAVLTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //    }
                        //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "PSF" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Origin.ToString())
                        //    {
                        //        //tdDepartPSF.Visible = true;
                        //        lblPSFTotalDepart.Text = (Convert.ToDecimal(lblPSFTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //    }
                        //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "SCF" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Origin.ToString())
                        //    {
                        //        //tdDepartSCF.Visible = true;
                        //        lblSCFTotalDepart.Text = (Convert.ToDecimal(lblSCFTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //    }
                        //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "" && dtOth.Rows[i]["FeeType"].ToString() == "ConnectionAdjustmentAmount" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Origin.ToString())
                        //    {
                        //        //tdConnectingDepart.Visible = true;
                        //        lblConnectingDepartTotal.Text = (Convert.ToDecimal(lblConnectingDepartTotal.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //    }
                        //    else if (dtOth.Rows[i]["FeeType"].ToString().ToUpper() == "DISCOUNT" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Origin.ToString())
                        //    {
                        //        //tdDepartSCF.Visible = true;
                        //        lblDiscTotalDepart.Text = (Convert.ToDecimal(lblDiscTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //    }
                        //    //else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "SPL" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Origin.ToString())
                        //    //{
                        //    //    //tdDepartSCF.Visible = true;
                        //    //    lblSPLTotalDepart.Text = (Convert.ToDecimal(lblSPLTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //    //}
                        //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "CHG" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Origin.ToString())
                        //    {
                        //        //tdDepartSCF.Visible = true;
                        //        lblCHGTotalChargeDepart.Text = (Convert.ToDecimal(lblCHGTotalChargeDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //    }
                        //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "CGST" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Origin.ToString())
                        //    {
                        //        //tdDepartSCF.Visible = true;
                        //        lblCSTTotalDepart.Text = (Convert.ToDecimal(lblCSTTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //    }
                        //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "CUTE" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Origin.ToString())
                        //    {
                        //        //tdDepartSCF.Visible = true;
                        //        lblCUTTotalDepart.Text = (Convert.ToDecimal(lblCUTTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //    }
                        //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "SGI" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Origin.ToString())
                        //    {
                        //        //tdDepartSCF.Visible = true;
                        //        lblSGITotalDepart.Text = (Convert.ToDecimal(lblSGITotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //    }
                        //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "SGST" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Origin.ToString())
                        //    {
                        //        //tdDepartSCF.Visible = true;
                        //        lblSSTTotalDepart.Text = (Convert.ToDecimal(lblSSTTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //    }
                        //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "UDF" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Origin.ToString())
                        //    {
                        //        //tdDepartSCF.Visible = true;
                        //        lblUDFTotalDepart.Text = (Convert.ToDecimal(lblUDFTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //    }
                        //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "SPL" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Origin.ToString())
                        //    {
                        //        //tdDepartSCF.Visible = true;
                        //        lblSPLTotalDepart.Text = (Convert.ToDecimal(lblSPLTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //    }
                        //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "APS" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Origin.ToString())
                        //    {
                        //        //tdDepartSCF.Visible = true;
                        //        lblAPSTotalDepart.Text = (Convert.ToDecimal(lblAPSTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //    }
                        //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "IADF" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Origin.ToString())
                        //    {
                        //        //tdDepartSCF.Visible = true;
                        //        lblIADFTotalDepart.Text = (Convert.ToDecimal(lblIADFTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //    }
                        //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "ACF" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Origin.ToString())
                        //    {
                        //        //tdDepartSCF.Visible = true;
                        //        lblACFTotalDepart.Text = (Convert.ToDecimal(lblACFTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //    }
                        //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "ASC" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Origin.ToString())
                        //    {
                        //        //tdDepartSCF.Visible = true;
                        //        lblASCTotalDepart.Text = (Convert.ToDecimal(lblASCTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //    }
                        //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "BCL" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Origin.ToString())
                        //    {
                        //        //tdDepartSCF.Visible = true;
                        //        lblBCLTotalDepart.Text = (Convert.ToDecimal(lblBCLTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //    }

                        //    if (returnFlight)
                        //    {
                        //        if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "KLIA2" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Destination.ToString())
                        //        {
                        //            //tdReturnKlia2.Visible = true;
                        //            lbl_Inklia2Total.Text = (Convert.ToDecimal(lbl_Inklia2Total.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //        }
                        //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "GST" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Destination.ToString())
                        //        {
                        //            //tdReturnGST.Visible = true;
                        //            lbl_InGSTTotal.Text = (Convert.ToDecimal(lbl_InGSTTotal.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //        }
                        //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "AVL" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Destination.ToString())
                        //        {
                        //            //tdReturnAVL.Visible = true;
                        //            lblAVLTotalReturn.Text = (Convert.ToDecimal(lblAVLTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //        }
                        //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "PSF" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Destination.ToString())
                        //        {
                        //            //tdReturnPSF.Visible = true;
                        //            lblPSFTotalReturn.Text = (Convert.ToDecimal(lblPSFTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //        }
                        //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "SCF" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Destination.ToString())
                        //        {
                        //            //tdReturnSCF.Visible = true;
                        //            lblSCFTotalReturn.Text = (Convert.ToDecimal(lblSCFTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //        }
                        //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "" && dtOth.Rows[i]["FeeType"].ToString() == "ConnectionAdjustmentAmount" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Destination.ToString())
                        //        {
                        //            //tdConnectingReturn.Visible = true;
                        //            lblConnectingReturnTotal.Text = (Convert.ToDecimal(lblConnectingReturnTotal.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //        }
                        //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "DISCOUNT" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Destination.ToString())
                        //        {
                        //            //tdDepartSCF.Visible = true;
                        //            lblDiscTotalReturn.Text = (Convert.ToDecimal(lblDiscTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //        }
                        //        //else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "SPL" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Destination.ToString())
                        //        //{
                        //        //    //tdDepartSCF.Visible = true;
                        //        //    lblSPLTotalReturn.Text = (Convert.ToDecimal(lblSPLTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //        //}
                        //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "CHG" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Destination.ToString())
                        //        {
                        //            //tdDepartSCF.Visible = true;
                        //            lblCHGTotalReturn.Text = (Convert.ToDecimal(lblCHGTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //        }
                        //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "SPL" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Destination.ToString())
                        //        {
                        //            //tdDepartSCF.Visible = true;
                        //            lblSPLTotalReturn.Text = (Convert.ToDecimal(lblSPLTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //        }
                        //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "APS" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Destination.ToString())
                        //        {
                        //            //tdDepartSCF.Visible = true;
                        //            lblAPSTotalReturn.Text = (Convert.ToDecimal(lblAPSTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //        }
                        //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "IADF" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Destination.ToString())
                        //        {
                        //            //tdDepartSCF.Visible = true;
                        //            lblIADFTotalReturn.Text = (Convert.ToDecimal(lblIADFTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //        }
                        //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "CGST" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Destination.ToString())
                        //        {
                        //            //tdDepartSCF.Visible = true;
                        //            lblCSTTotalReturn.Text = (Convert.ToDecimal(lblCSTTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //        }
                        //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "ASC" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Destination.ToString())
                        //        {
                        //            //tdDepartSCF.Visible = true;
                        //            lblASCTotalReturn.Text = (Convert.ToDecimal(lblASCTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //        }
                        //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "BCL" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Destination.ToString())
                        //        {
                        //            //tdDepartSCF.Visible = true;
                        //            lblBCLTotalReturn.Text = (Convert.ToDecimal(lblBCLTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //        }
                        //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "CUTE" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Destination.ToString())
                        //        {
                        //            //tdReturnSCF.Visible = true;
                        //            lblCUTTotalReturn.Text = (Convert.ToDecimal(lblCUTTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //        }
                        //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "SGI" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Destination.ToString())
                        //        {
                        //            //tdReturnSCF.Visible = true;
                        //            lblSGITotalReturn.Text = (Convert.ToDecimal(lblSGITotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //        }
                        //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "SGST" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Destination.ToString())
                        //        {
                        //            //tdReturnSCF.Visible = true;
                        //            lblSSTTotalReturn.Text = (Convert.ToDecimal(lblSSTTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //        }
                        //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "UDF" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Destination.ToString())
                        //        {
                        //            //tdReturnSCF.Visible = true;
                        //            lblUDFTotalReturn.Text = (Convert.ToDecimal(lblUDFTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //        }
                        //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "ACF" && dtOth.Rows[i]["Origin"].ToString() == BKTransDetail[0].Destination.ToString())
                        //        {
                        //            //tdDepartSCF.Visible = true;
                        //            lblACFTotalReturn.Text = (Convert.ToDecimal(lblACFTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                        //        }
                        //    }
                        //}
                    }

                    HideZeroValue();
                }
                else
                {
                    ShowReturnColumn(false);
                }
            }
            else
                divFareBreakdown.Visible = false;
        }

        protected void HideZeroValue()
        {
            if (lbl_FlightTotal.Text == "" || lbl_FlightTotal.Text == "0.00")
            {
                trDepartFare.Visible = false;
            }
            if (lblProcessTotalDepart.Text == "" || lblProcessTotalDepart.Text == "0.00")
            {
                trProcessDepart.Visible = false;
            }
            if (lbl_InfantTotal.Text == "" || lbl_InfantTotal.Text == "0.00")
            {
                trInfantFareDepart.Visible = false;
            }
            if (lbl_taxTotal.Text == "" || lbl_taxTotal.Text == "0.00")
            {
                trAirportTaxDepart.Visible = false;
            }
            if (lbl_PaxFeeTotal.Text == "" || lbl_PaxFeeTotal.Text == "0.00")
            {
                trPaxServChargeDepart.Visible = false;
            }
            if (lblFuelPriceTotalDepart.Text == "" || lblFuelPriceTotalDepart.Text == "0.00")
            {
                trFuelTaxDepart.Visible = false;
            }
            if (lblSvcChargeTotalDepart.Text == "" || lblSvcChargeTotalDepart.Text == "0.00")
            {
                trServChargeDepart.Visible = false;
            }
            if (lblVATDepart.Text == "" || lblVATDepart.Text == "0.00")
            {
                trVATDepart.Visible = false;
            }
            //if (lblBaggageTotalDepart.Text == "" || lblBaggageTotalDepart.Text == "0.00")
            //{
            //    trBaggageChargeDepart.Visible = false;
            //}
            //if (lblMealTotalDepart.Text == "" || lblMealTotalDepart.Text == "0.00")
            //{
            //    trMealChargeDepart.Visible = false;
            //}
            //if (lblSportTotalDepart.Text == "" || lblSportTotalDepart.Text == "0.00")
            //{
            //    trSportChargeDepart.Visible = false;
            //}
            //if (lblComfortTotalDepart.Text == "" || lblComfortTotalDepart.Text == "0.00")
            //{
            //    trComfortChargeDepart.Visible = false;
            //}
            if (lblSSRTotalDepart.Text == "" || lblSSRTotalDepart.Text == "0.00")
            {
                trSSRChargeDepart.Visible = false;
            }
            if (lblSeatTotalDepart.Text == "" || lblSeatTotalDepart.Text == "0.00")
            {
                trSeatChargeDepart.Visible = false;
            }
            if (lblOthTotalDepart.Text == "" || lblOthTotalDepart.Text == "0.00")
            {
                trOthChargeDepart.Visible = false;
            }
            if (lblConnectingDepartTotal.Text == "" || lblConnectingDepartTotal.Text == "0.00")
            {
                trConnectingChargeDepart.Visible = false;
            }
            if (lbl_klia2Total.Text == "" || lbl_klia2Total.Text == "0.00")
            {
                trKlia2FeeDepart.Visible = false;
            }
            if (lbl_GSTTotal.Text == "" || lbl_GSTTotal.Text == "0.00")
            {
                trGSTChargeDepart.Visible = false;
            }
            if (lblAVLTotalDepart.Text == "" || lblAVLTotalDepart.Text == "0.00")
            {
                trAVLChargeDepart.Visible = false;
            }
            if (lblPSFTotalDepart.Text == "" || lblPSFTotalDepart.Text == "0.00")
            {
                trPSFChargeDepart.Visible = false;
            }
            if (lblSCFTotalDepart.Text == "" || lblSCFTotalDepart.Text == "0.00")
            {
                trSCFChargeDepart.Visible = false;
            }
            if (lblSPLTotalDepart.Text == "" || lblSPLTotalDepart.Text == "0.00")
            {
                trSPLChargeDepart.Visible = false;
            }
            if (lblAPSTotalDepart.Text == "" || lblAPSTotalDepart.Text == "0.00")
            {
                trAPSChargeDepart.Visible = false;
            }
            if (lblACFTotalDepart.Text == "" || lblACFTotalDepart.Text == "0.00")
            {
                trACFChargeDepart.Visible = false;
            }
            if (lblIADFTotalDepart.Text == "" || lblIADFTotalDepart.Text == "0.00")
            {
                trIADFChargeDepart.Visible = false;
            }
            if (lblASCTotalDepart.Text == "" || lblASCTotalDepart.Text == "0.00")
            {
                trASCChargeDepart.Visible = false;
            }
            if (lblBCLTotalDepart.Text == "" || lblBCLTotalDepart.Text == "0.00")
            {
                trBCLChargeDepart.Visible = false;
            }
            if (lblCHGTotalChargeDepart.Text == "" || lblCHGTotalChargeDepart.Text == "0.00")
            {
                trCHGChargeDepart.Visible = false;
            }
            if (lblDiscTotalDepart.Text == "" || lblDiscTotalDepart.Text == "0.00")
            {
                trDiscountChargeDepart.Visible = false;
            }
            if (lblPromoDiscTotalDepart.Text == "" || lblPromoDiscTotalDepart.Text == "0.00")
            {
                trPromoDiscDepart.Visible = false;
            }
            if (lblCSTTotalDepart.Text == "" || lblCSTTotalDepart.Text == "0.00")
            {
                trCSTChargeDepart.Visible = false;
            }
            if (lblCUTTotalDepart.Text == "" || lblCUTTotalDepart.Text == "0.00")
            {
                trCUTChargeDepart.Visible = false;
            }
            if (lblSGITotalDepart.Text == "" || lblSGITotalDepart.Text == "0.00")
            {
                trSGIChargeDepart.Visible = false;
            }
            if (lblSSTTotalDepart.Text == "" || lblSSTTotalDepart.Text == "0.00")
            {
                trSSTChargeDepart.Visible = false;
            }
            if (lblUDFTotalDepart.Text == "" || lblUDFTotalDepart.Text == "0.00")
            {
                trUDFChargeDepart.Visible = false;
            }

            if (lbl_InFlightTotal.Text == "" || lbl_InFlightTotal.Text == "0.00")
            {
                trReturnfare.Visible = false;
            }
            if (lbl_InInfantTotal.Text == "" || lbl_InInfantTotal.Text == "0.00")
            {
                trInfantfareReturn.Visible = false;
            }
            if (lbl_IntaxTotal.Text == "" || lbl_IntaxTotal.Text == "0.00")
            {
                trAirportTaxReturn.Visible = false;
            }
            if (lbl_InPaxFeeTotal.Text == "" || lbl_InPaxFeeTotal.Text == "0.00")
            {
                PaxServChargeReturn.Visible = false;
            }
            if (lblFuelTotalReturn.Text == "" || lblFuelTotalReturn.Text == "0.00")
            {
                trFuelTaxReturn.Visible = false;
            }
            if (lblSvcTotalReturn.Text == "" || lblSvcTotalReturn.Text == "0.00")
            {
                trServChargeReturn.Visible = false;
            }
            if (lblVATReturn.Text == "" || lblVATReturn.Text == "0.00")
            {
                trVATReturn.Visible = false;
            }
            //if (lblBaggageTotalReturn.Text == "" || lblBaggageTotalReturn.Text == "0.00")
            //{
            //    trBagggageChargeReturn.Visible = false;
            //}
            //if (lblMealTotalReturn.Text == "" || lblMealTotalReturn.Text == "0.00")
            //{
            //    trMealChargeReturn.Visible = false;
            //}
            //if (lblSportTotalReturn.Text == "" || lblSportTotalReturn.Text == "0.00")
            //{
            //    trSportChargeReturn.Visible = false;
            //}
            //if (lblComfortTotalReturn.Text == "" || lblComfortTotalReturn.Text == "0.00")
            //{
            //    trComfortChargeReturn.Visible = false;
            //}
            if (lblSSRTotalReturn.Text == "" || lblSSRTotalReturn.Text == "0.00")
            {
                trSSRChargeReturn.Visible = false;
            }
            if (lblSeatTotalReturn.Text == "" || lblSeatTotalReturn.Text == "0.00")
            {
                trSeatChargeReturn.Visible = false;
            }
            if (lblOthTotalReturn.Text == "" || lblOthTotalReturn.Text == "0.00")
            {
                trOthChargeReturn.Visible = false;
            }
            if (lblCSTTotalReturn.Text == "" || lblCSTTotalReturn.Text == "0.00")
            {
                trCSTChargeReturn.Visible = false;
            }
            if (lblCUTTotalReturn.Text == "" || lblCUTTotalReturn.Text == "0.00")
            {
                trCUTChargeReturn.Visible = false;
            }
            if (lblSGITotalReturn.Text == "" || lblSGITotalReturn.Text == "0.00")
            {
                trSGIChargeReturn.Visible = false;
            }
            if (lblSSTTotalReturn.Text == "" || lblSSTTotalReturn.Text == "0.00")
            {
                trSSTChargeReturn.Visible = false;
            }
            if (lblUDFTotalReturn.Text == "" || lblUDFTotalReturn.Text == "0.00")
            {
                trUDFChargeReturn.Visible = false;
            }
            if (lblConnectingReturnTotal.Text == "" || lblConnectingReturnTotal.Text == "0.00")
            {
                trConnectingChargeReturn.Visible = false;
            }
            if (lbl_Inklia2Total.Text == "" || lbl_Inklia2Total.Text == "0.00")
            {
                trKlia2FeeReturn.Visible = false;
            }
            if (lbl_InGSTTotal.Text == "" || lbl_InGSTTotal.Text == "0.00")
            {
                trGSTChargeReturn.Visible = false;
            }
            if (lblAVLTotalReturn.Text == "" || lblAVLTotalReturn.Text == "0.00")
            {
                trAVLChargeReturn.Visible = false;
            }
            if (lblPSFTotalReturn.Text == "" || lblPSFTotalReturn.Text == "0.00")
            {
                trPSFChargeReturn.Visible = false;
            }
            if (lblSCFTotalReturn.Text == "" || lblSCFTotalReturn.Text == "0.00")
            {
                trSCFChargeReturn.Visible = false;
            }
            if (lblSPLTotalReturn.Text == "" || lblSPLTotalReturn.Text == "0.00")
            {
                trSPLChargeReturn.Visible = false;
            }
            if (lblAPSTotalReturn.Text == "" || lblAPSTotalReturn.Text == "0.00")
            {
                trAPSChargeReturn.Visible = false;
            }
            if (lblACFTotalReturn.Text == "" || lblACFTotalReturn.Text == "0.00")
            {
                trACFChargeReturn.Visible = false;
            }
            if (lblIADFTotalReturn.Text == "" || lblIADFTotalReturn.Text == "0.00")
            {
                trIADFChargeReturn.Visible = false;
            }
            if (lblASCTotalReturn.Text == "" || lblASCTotalReturn.Text == "0.00")
            {
                trASCChargeReturn.Visible = false;
            }
            if (lblBCLTotalReturn.Text == "" || lblBCLTotalReturn.Text == "0.00")
            {
                trBCLChargeReturn.Visible = false;
            }
            if (lblCHGTotalReturn.Text == "" || lblCHGTotalReturn.Text == "0.00")
            {
                trCHGChargeReturn.Visible = false;
            }
            if (lblDiscTotalReturn.Text == "" || lblDiscTotalReturn.Text == "0.00")
            {
                trDiscountChargeReturn.Visible = false;
            }
            if (lblPromoDiscTotalReturn.Text == "" || lblPromoDiscTotalReturn.Text == "0.00")
            {
                trPromoDiscReturn.Visible = false;
            }
        }

        protected void LoadPaymentHistory(string TransID)
        {
            lstbookPaymentInfo = objBooking.GetAllBK_TRANSTENDERFilter(TransID, " BK_TRANSTENDER.TransVoid=0 AND ");
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            if (lstbookPaymentInfo != null)
            {
                divPayment.Visible = true;
                foreach (BookingTransTender payment in lstbookPaymentInfo)
                {
                    ltrPayment.Text += "<tr><td width='100px'>&nbsp;&nbsp;" + payment.RecordLocator + "&nbsp;" + "</td>";
                    ltrPayment.Text += "<td width='150px'>&nbsp;&nbsp;" + payment.TenderDesc + "&nbsp;" + "</td>";
                    ltrPayment.Text += "<td width='200px'>" + payment.TenderAmt.ToString("N", nfi) + "&nbsp;" + payment.CurrencyPaid + "</td>";
                    //ltrPayment.Text += "<td width='200px'>" + payment.TransDate.ToString("dddd, dd MMM yyyy / HHmm") + "&nbsp;" + "</td>";
                    ltrPayment.Text += "<td width='200px'>" + payment.TransDate.ToString("dddd, dd MMM yyyy") + "&nbsp;" + "</td>";
                    ltrPayment.Text += "<td width='150px'>" + payment.RefNo + "&nbsp;" + "</td>";
                    string TransStatus = "Approved";
                    if (payment.Transvoid == 1)
                        TransStatus = "Declined";
                    ltrPayment.Text += "<td width='100px'>" + TransStatus + "&nbsp;" + "</td>";
                    ltrPayment.Text += "<td width='200px'>" + payment.FeeTypeDesc + "</td>";
                    ltrPayment.Text += "</tr> ";
                }
            }
            else
                divPayment.Visible = false;
        }

        protected void CancelProcess()
        {
            string errMessage = "";
            BookingTransactionMain headerData = new BookingTransactionMain();
            List<BookingTransactionDetail> detailDatas = new List<BookingTransactionDetail>();
            List<BookingTransactionDetail> listDetailDatas = new List<BookingTransactionDetail>();
            BookingTransactionDetail detailInfo = new BookingTransactionDetail();
            List<BookingTransTender> paymentData = new List<BookingTransTender>();
            try
            {
                detailDatas = objBooking.BookingDetailFilter(TransID, -1, 1);
                foreach (BookingTransactionDetail detail in detailDatas)
                {
                    string signature = absNavitaire.AgentLogon();
                    //absNavitaire.SellSegment(detail.RecordLocator, signature, ref errMessage); //cancel journey to api
                    absNavitaire.CancelJourney(detail.RecordLocator, -detail.CollectedAmount, detail.Currency, signature, ref errMessage); //cancel journey to api
                    if (errMessage == "")
                    {
                        if (AgentSet != null)
                        {
                            detail.LastSyncBy = AgentSet.AgentID;
                        }
                        else
                        {
                            detail.LastSyncBy = AdminSet.AdminID;
                        }
                        detail.SyncLastUpd = DateTime.Now;
                        detail.TransVoid = 1;
                        listDetailDatas.Add(detail);
                    }
                    else
                    {
                        log.Warning(this, "Cancellation fail, PNR:" + detail.RecordLocator + "; TransID:" + TransID + " , ERRMSG: " + errMessage);
                        //commented by diana 20131113 - to continue looping
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "PostbackKey", "<script type='text/javascript'>window.alert('" + errMessage + "');</script>");
                        //return;
                    }
                    //if (objBooking.CancelProcess(detailDatas, AgentSet.AgentID))
                    //{
                    //    if (AgentSet != null)
                    //    {
                    //        detail.LastSyncBy = AgentSet.AgentID;
                    //    }
                    //    else
                    //    {
                    //        detail.LastSyncBy = AdminSet.AdminID;
                    //    }
                    //    detail.SyncLastUpd = DateTime.Now;
                    //    detail.TransVoid = 1;
                    //    listDetailDatas.Add(detail);
                    //}
                    //else
                    //{
                    //    log.Error("Cancellation fail, PNR:" + detail.RecordLocator + "; TransID:" + TransID);
                    //    Page.ClientScript.RegisterStartupScript(this.GetType(), "PostbackKey", "<script type='text/javascript'>window.alert('" + errMessage + "');</script>");
                    //    return;
                    //}
                }

                if (listDetailDatas.Count > 0 && errMessage == "")
                {
                    //update status on local
                    headerData = objBooking.GetSingleBK_TRANSMAIN(TransID);
                    headerData.TransStatus = 4;
                    headerData.CancelDate = DateTime.Now;
                    if (txtReason.Text == string.Empty)
                        headerData.TransRemark1 = "Cancel by Agent";
                    else
                        headerData.TransRemark1 = txtReason.Text;
                    if (AgentSet != null)
                    {
                        headerData.LastSyncBy = AgentSet.AgentID;
                    }
                    else
                    {
                        headerData.LastSyncBy = AdminSet.AdminID;
                    }
                    paymentData = objBooking.GetAllBK_TRANSTENDERFilter(TransID);

                    if (objBooking.UpdateTransMainPaymentCancel(headerData, listDetailDatas, paymentData, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Update))
                    {
                        //must success
                    }
                    else
                    {
                        //failed sql
                    }
                }
                else
                {
                    //failed, no record sent to api
                }
                //objBooking.CancelTransaction(TransID, AgentSet.AgentID, ref intError, ref strErrorDesc);
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
        }

        public bool IsChangeFlight(int SeqNo, string RecordLocator)
        {
            return false;
        }

        protected void gvClass_Callback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            gvClass.DataBind();
            gvClass.ExpandAll();
        }

        protected void gvClass_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            object rowKey = null;
            object seqNo = 0;

            if (e.ButtonID == "changeBtnFinish")
            {
                ((ASPxGridView)sender).JSProperties["cpIsUpdated"] = "";
                //object TransID = gvClass.GetRowValues(e.VisibleIndex, "TransID");
                Session["TransID"] = TransID;

                rowKey = gvClass.GetRowValues(e.VisibleIndex, "RecordLocator");
                seqNo = gvClass.GetRowValues(e.VisibleIndex, "SeqNo");
                List<BookingTransactionDetail> listTransDetail = new List<BookingTransactionDetail>();
                listTransDetail = objBooking.GetAllBK_TRANSDTLFilterByPNR(TransID, rowKey.ToString());
                int count = listTransDetail.Count;

                int ind = 0, index = 0, seqNoOther = 0;
                DateTime OtherDepartureDate = DateTime.Now;

                foreach (BookingTransactionDetail bkTransDetail in listTransDetail)
                {
                    if (bkTransDetail.SeqNo.ToString() != seqNo.ToString())
                    {
                        seqNoOther = Convert.ToInt32(bkTransDetail.SeqNo.ToString());
                        OtherDepartureDate = bkTransDetail.DepatureDate;
                    }
                    else
                    {
                        index = ind;
                    }
                    ind += 1;
                }

                DateTime DepartureDate, ArrivalDate;
                bool allowChange = false;
                string msg = "", ReturnOnly = "false";
                TimeSpan timeSpan;

                if (count <= 1) //is one way
                {
                    DepartureDate = listTransDetail[0].DepatureDate;
                    ArrivalDate = listTransDetail[0].ArrivalDate;

                    timeSpan = DepartureDate - DateTime.Now;
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "PostbackKey", "<script type='text/javascript'>window.alert('" + timeSpan + "');</script>");

                    if (timeSpan.TotalDays >= 14)
                    {
                        allowChange = true;
                    }
                    ReturnOnly = "false";
                }
                else if (Convert.ToInt32(seqNo.ToString()) < seqNoOther) //is two way and change going flight
                {
                    DepartureDate = listTransDetail[index].DepatureDate;
                    ArrivalDate = listTransDetail[index].ArrivalDate;

                    timeSpan = DepartureDate - DateTime.Now;
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "PostbackKey", "<script type='text/javascript'>window.alert('" + timeSpan + "');</script>");
                    if (timeSpan.TotalDays >= 14)
                    {
                        allowChange = true;
                    }
                    ReturnOnly = "false";

                }
                else //is two way and change returning flight
                {
                    DepartureDate = listTransDetail[index].DepatureDate;
                    ArrivalDate = listTransDetail[index].ArrivalDate;

                    timeSpan = OtherDepartureDate.AddDays(2) - DateTime.Now;
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "PostbackKey", "<script type='text/javascript'>window.alert('" + timeSpan + "');</script>");
                    if (timeSpan.TotalDays >= 0)
                    {
                        allowChange = true;
                    }
                    ReturnOnly = "true";
                }
                //allowChange = true;
                if (allowChange == false)
                {
                    if (ReturnOnly == "false")
                    {
                        ((ASPxGridView)sender).JSProperties["cpIsUpdated"] = "PNR for going flight cannot be changed anymore. Changes on both segments are only allowed within 14 days before STD.";
                    }
                    else
                    {
                        ((ASPxGridView)sender).JSProperties["cpIsUpdated"] = "PNR for return flight cannot be changed anymore. Changes on return segment are only allowed within 2 days after STD.";
                    }
                }
                else
                {
                    Session["RecordLocator"] = rowKey;

                    ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                    string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), rowKey.ToString(), "");

                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("../public/searchflightchange.aspx?k=" + hashkey + "&TransID=" + TransID + "&RecordLocator=" + rowKey + "&Return=" + ReturnOnly);
                }
            }
        }

        protected void gvRejectedClass_Callback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            gvRejectedClass.DataBind();
            gvRejectedClass.ExpandAll();
        }

        protected void gvPassenger_Callback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            gvPassenger.DataBind();
            gvPassenger.ExpandAll();
        }

        protected void gvInfant_Callback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            gvInfant.DataBind();
            gvInfant.ExpandAll();
        }

        protected void btYes_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtReason.Text.Trim()))
                    return;
                CancelProcess();
                if (AgentSet != null)
                    Response.Redirect("../Message.aspx?msgID=108", false);
                else
                    Response.Redirect("../Message.aspx?msgID=110", false);

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                lblMsg.Text = "Cancel Failed";
            }
        }

        protected void btPassenger_Click(object sender, EventArgs e)
        {
            ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
            string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), TransID.ToString(), "");

            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "var Mleft = (screen.width/2)-(760/2);var Mtop = (screen.height/2)-(700/2);window.open( 'passengerdetail.aspx?k=" + hashkey + "&TransID=" + TransID + "', null, 'height=700,width=760,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );", true);
            Response.Redirect("passengerdetail.aspx?k=" + hashkey + "&TransID=" + TransID, false);
        }

        protected void btDivide_Click(object sender, EventArgs e)
        {
            ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
            string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), TransID.ToString(), "");

            Response.Redirect("divide.aspx?k=" + hashkey + "&TransID=" + TransID, false);
        }

        protected void btFlightChange_Click(object sender, EventArgs e)
        {
            ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
            string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), TransID.ToString(), "");

            Response.Redirect("FlightChange.aspx?k=" + hashkey + "&TransID=" + TransID, false);
        }

        protected void btManageAddOn_Click(object sender, EventArgs e)
        {
            ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
            string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), TransID.ToString(), "");
            //change to new add-On table, Tyas
            Response.Redirect("manageaddons.aspx?k=" + hashkey + "&TransID=" + TransID, false);
            //Response.Redirect("manageaddon.aspx?k=" + hashkey + "&TransID=" + TransID, false);
        }

        protected void btManageInsure_Click(object sender, EventArgs e)
        {
            ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
            string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), TransID.ToString(), "");
            //change to new add-On table, Tyas
            Response.Redirect("insure.aspx?k=" + hashkey + "&TransID=" + TransID, false);
        }

        protected void btManageSeats_Click(object sender, EventArgs e)
        {
            ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
            string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), TransID.ToString(), "");

            Response.Redirect("manageseat.aspx?k=" + hashkey + "&TransID=" + TransID, false);
        }

        protected void ClearSessionData()
        {
            HttpContext.Current.Session.Remove("TempFlight");
            HttpContext.Current.Session.Remove("dataClass");
            HttpContext.Current.Session.Remove("ErrorPayment");
            HttpContext.Current.Session.Remove("dataClassTrans");
        }

        protected void btPayFull_Click(object sender, EventArgs e)
        {
            ClearSessionData();
            Response.Redirect(Shared.MySite.PublicPages.Payment2);
        }

        protected void btGetLatest_Click(object sender, EventArgs e)
        {
            List<ListTransaction> AllTransaction = new List<ListTransaction>();
            AllTransaction = objBooking.GetTransactionDetails(TransID);
            if (AllTransaction != null && AllTransaction.Count > 0)
            {
                ListTransaction lstTrans = AllTransaction[0];
                if (Session["AgentSet"] != null)
                {
                    //objBooking.UpdateAllTransByAgent(AgentSet.AgentID, AgentSet.AgentName, TransID);

                    List<BookingTransactionDetail> VoidPNRs = new List<BookingTransactionDetail>();
                    List<BookingTransactionDetail> ExpiredPNRs = new List<BookingTransactionDetail>();
                    objBooking.UpdateAllBookingJourneyDetails(lstTrans, AgentSet.AgentName.ToString(), AgentSet.AgentID.ToString(), ref VoidPNRs, ref ExpiredPNRs, true);
                }
                else
                {
                    objBooking.UpdateAllTransByAgent("", "", TransID);
                }

            }
            ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
            string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), TransID, "");
            DevExpress.Web.ASPxWebControl.RedirectOnCallback("../public/bookingdetail.aspx?k=" + hashkey + "&TransID=" + TransID);
        }

        protected void cbFareBreakdown_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.ToString().Split('|');
            //Session["PNR"] = null;
            if (param[0].ToLower() == "load")
            {
                string PNR = param[1];

                Session["PNR"] = PNR;
                LoadFareBreakdown(PNR);
            }
        }

        protected void btResendItinerary_Click(object sender, EventArgs e)
        {
            

        }

        protected void ValidatePopup(object sender, CallbackEventArgs e)
        {
            e.Result = "";
            try
            {
                ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), TransID.ToString(), "");

                if (resendItinerary())
                    e.Result = "Re-sent Itinerary Successfully";
                else
                    e.Result = "";

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                e.Result = ex.Message;
            }
        }

        protected Boolean resendItinerary()
        {
            int totPNR = 0;
            int cekResp = 0;
            lstbookDTLInfo = objBooking.GetAllBK_TRANSDTLFilterAll(TransID, 0);
            totPNR = lstbookDTLInfo.Count;
            foreach (BookingTransactionDetail bookDTL in lstbookDTLInfo)
            {
                SendItineraryResponse respSI = absNavitaire.SendItineraryByPNR(bookDTL.RecordLocator);
                
                if (respSI != null) cekResp += 1;
            }

            if (cekResp == totPNR) return true;
            else return false;
        }

    }
}