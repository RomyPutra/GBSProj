using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ABS.Logic.GroupBooking.Agent;
using ABS.Logic.GroupBooking.Booking;
using SEAL.Data;
using DevExpress.Web;
using System.Collections;
using ABS.Logic.Shared;
using System.Globalization;
using System.Data;
using SEAL.WEB.Common;
using System.IO;
using SEAL.WEB.UI;
using System.Text.RegularExpressions;
using ABS.Navitaire.BookingManager;
using System.Configuration;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Extensions;
using ABS.GBS.Log;
using StackExchange.Profiling;
using System.Net;
//using log4net;

namespace GroupBooking.Web
{
    public partial class passengerdetail : System.Web.UI.Page
    {
        UserSet AgentSet;
        List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain> TransMainData = new List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>();
        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        BookingTransactionDetail bookDTLInfo = new BookingTransactionDetail();
        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
        List<BookingTransactionDetail> lstbookFlightInfo = new List<BookingTransactionDetail>();
        List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
        List<BookingTransactionDetail> lstRejectedbookDTLInfo = new List<BookingTransactionDetail>();
        List<BookingTransactionDetail> listDetailCombinePNR = new List<BookingTransactionDetail>();
        BookingTransactionMain bookHDRInfo = new BookingTransactionMain();
        int pos;
        int change = 0;
        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
        BookingTransactionTax transTaxInfo = new BookingTransactionTax();
        List<PassengerData> lstPassengerData = new List<PassengerData>();
        List<PassengerData> lstPassInfantData = new List<PassengerData>();
        PassengerData PassData = new PassengerData();
        PassengerData PassData2 = new PassengerData();
        BookingTaxFeesControl taxFeesControlInfo = new BookingTaxFeesControl();
        DataTable dtPass;
        DataTable dtPassOld;
        DataTable dtInfant;
        BookingTransactionFees transFeesInfo = new BookingTransactionFees();
        List<BookingTransactionFees> lsttransFeesInfo = new List<BookingTransactionFees>();
        ABS.GBS.eService.ProcessServiceSoapClient eWS = new ABS.GBS.eService.ProcessServiceSoapClient();
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogControl log = new LogControl();
        SystemLog SystemLog = new SystemLog();

        string TransID;
        static int NameChangeMax = 0, ChangeLimit = 0;
        static decimal NameChangeLimit1 = 0;
        static decimal NameChangeLimit2 = 0;
        static decimal NameChangeUsed = 0;

        decimal MinTime = 0, MaxTime = 0, LimitTime = 0, TimeDiff = 0, ActiveMinTime = 0, ActiveMaxTime = 0;

        //20170321 - Sienny
        List<ABS.Logic.GroupBooking.Booking.BookingTransactionDetail> BookingTransactionDetail;
        static bool IsInternationalFlight = false;

        //20170613 - Sienny
        string AgentCountryCode;
        string AgentCurrencyCode;
        string CurrencyCode;
        AgentProfile agProfileInfo = new AgentProfile();
        int IsFree1 = 0, IsFree2 = 0, IsFree3 = 0;
        int MaxFreeTime1 = 0, MaxFreeTime2 = 0, MaxFreeTime3 = 0;
        decimal MaxPax1 = 0, MaxPax2 = 0, MaxPax3 = 0;
        int MaxChangeTime = 0;
        DateTime EffectiveDate;

        ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");

        string custommessage = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //added by romy for profiler
            var profiler = MiniProfiler.Current;

            lblMsg.Text = "";
            TransID = Request.QueryString["TransID"];
            string keySent = Request.QueryString["k"];

            //ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
            //string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), TransID.ToString(), "");

            //if (hashkey != keySent)
            //{
            //    Response.Redirect("~/public/agentlogin.aspx");
            //}

            if (Session["AgentSet"] != null)
            {
                AgentSet = (UserSet)Session["AgentSet"];
                if (!IsPostBack)
                {
                    using (profiler.Step("InitValue"))
                    {
                        InitValue();
                    }
                    using (profiler.Step("LoadData"))
                    {
                        LoadData();
                    }
                    using (profiler.Step("LoadGridView"))
                    {
                        LoadGridView();
                    }
                    HttpContext.Current.Session["SellSessionID"] = null;
                    Session["objListBK_TRANSDTL_Infos"] = null;
                    Session["arrayerror"] = null;
                    HttpContext.Current.Session["BalanceNameChange"] = null;

                    change = 0;
                }

                if (IsCallback)
                    using (profiler.Step("LoadGridViewCallBack"))
                    {
                        LoadGridViewCallBack();
                    }
                if (Session["CurStatus"] != null)
                {
                    int tempstatus = (int)Session["CurStatus"];
                    if (tempstatus == 3)
                    {
                        using (profiler.Step("LoadTotalAmount"))
                        {
                            LoadTotalAmount();
                        }
                    }
                }
            }
            else
            {
                Response.Redirect(Shared.MySite.PublicPages.InvalidPage);
            }
        }

        protected void LoadData()
        {
            MessageList msgList = new MessageList();
            bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(TransID);
            int tempstatus = bookHDRInfo.TransStatus;
            string tempdate1 = String.Format("{0:MM/dd/yyyy}", bookHDRInfo.STDDate);
            string tempdate2 = String.Format("{0:MM/dd/yyyy}", DateTime.Now);
            TimeSpan ts = Convert.ToDateTime(tempdate1) - Convert.ToDateTime(tempdate2);
            int temphour = Convert.ToInt32(ts.TotalHours.ToString());
            NameChangeMax = bookHDRInfo.NameChangeMax;
            NameChangeLimit1 = bookHDRInfo.NameChangeLimit1;
            NameChangeLimit2 = bookHDRInfo.NameChangeLimit2;

            string paxfree = "0";

            Session["bookHDRInfo"] = bookHDRInfo;

            //if (NameChangeMax == 1) NameChangeUsed = NameChangeLimit1;
            //else NameChangeUsed = NameChangeLimit2;


            //if (tempstatus == 3 && temphour >= 48 && temphour <= 168)
            //    gvPassenger.Columns["Details"].Visible = true;
            //else if (tempstatus == 3 && temphour >= 6 && temphour < 48)
            //    gvPassenger.Columns["Details"].Visible = true;
            //else if (tempstatus == 2 && temphour <= 168)
            //    gvPassenger.Columns["Details"].Visible = true;
            //else if (tempstatus == 3 && temphour < 6)
            //{
            //    gvPassenger.Columns["Details"].Visible = false;
            //    btnUpload.Enabled = false;
            //    btConfirm.Enabled = false;
            //}


            Session["CurStatus"] = tempstatus;


            List<ABS.Logic.GroupBooking.Booking.BookingTransactionDetail> BookingTransactionDetail;
            BookingTransactionDetail = objBooking.Get_TRANSDTL(bookHDRInfo.AgentID, TransID);

            lstbookDTLInfo = objBooking.GetAllBK_TRANSDTLFilterAll(TransID, 0);
            if (lstbookDTLInfo != null && lstbookDTLInfo.Count > 0)
            {
                MinTime = 0; MaxTime = 0; LimitTime = 0;
                DateTime DepDate = lstbookDTLInfo[0].DepatureDate;
                TimeDiff = decimal.Parse((DepDate - DateTime.Now).TotalHours.ToString());

                string GroupName = objGeneral.getOPTGroupByCarrierCode(lstbookDTLInfo[0].CarrierCode);

                //20170613 - Sienny
                MaxFreeTime1 = 0; MaxFreeTime2 = 0; MaxFreeTime3 = 0;
                MaxPax1 = 0; MaxPax2 = 0; MaxPax3 = 0;
                IsFree1 = 0; IsFree2 = 0; IsFree3 = 0;
                MaxChangeTime = 0; ChangeLimit = 0;
                if (Session["CountryCode"].ToString() != null)
                    AgentCountryCode = Session["CountryCode"].ToString();
                AgentCurrencyCode = AgentSet.Currency;
                CurrencyCode = BookingTransactionDetail[0].Currency;
                DataTable dtFeeSetting = new DataTable();
                dtFeeSetting = objBooking.GetFeeSettingByGroupCountryCurrencyCode(GroupName, AgentCountryCode, CurrencyCode);
                if (dtFeeSetting != null && dtFeeSetting.Rows.Count > 0)
                {
                    EffectiveDate = Convert.ToDateTime(dtFeeSetting.Rows[0]["EffectiveDate"].ToString());
                    IsFree1 = Convert.ToInt32(dtFeeSetting.Rows[0]["IsFree1"].ToString());
                    IsFree2 = Convert.ToInt32(dtFeeSetting.Rows[0]["IsFree2"].ToString());
                    IsFree3 = Convert.ToInt32(dtFeeSetting.Rows[0]["IsFree3"].ToString());
                    MaxChangeTime = Convert.ToInt32(dtFeeSetting.Rows[0]["MaxChangeTime"].ToString());
                    ChangeLimit = Convert.ToInt32(dtFeeSetting.Rows[0]["ChangeLimit"].ToString());
                    MaxPax1 = Convert.ToDecimal(dtFeeSetting.Rows[0]["MaxPax1"].ToString());
                    MaxPax2 = Convert.ToDecimal(dtFeeSetting.Rows[0]["MaxPax2"].ToString());
                    MaxPax3 = Convert.ToDecimal(dtFeeSetting.Rows[0]["MaxPax3"].ToString());
                    if (IsFree1 == 1)
                        MaxFreeTime1 = Convert.ToInt32(dtFeeSetting.Rows[0]["MaxFreeTime1"].ToString());
                    else
                        MaxFreeTime1 = 0;
                    Session["EffectiveDate"] = EffectiveDate;
                    Session["IsFree1"] = IsFree1;
                    Session["IsFree2"] = IsFree2;
                    Session["MaxFreeTime1"] = MaxFreeTime1;
                    Session["MaxFreeTime2"] = MaxFreeTime2;
                    Session["MaxChangeTime"] = MaxChangeTime;
                    Session["ChangeLimit"] = ChangeLimit;
                    Session["MaxPax1"] = MaxPax1;
                    Session["MaxPax2"] = MaxPax2;
                }


                //if (GroupName.ToLower().Trim() == "aax")
                //{
                //    ABS.Logic.GroupBooking.Settings objSYS_PREFT_Info = new ABS.Logic.GroupBooking.Settings();

                //    objSYS_PREFT_Info = objGeneral.GetSingleSYS_PREFT(1, GroupName.ToUpper().Trim(), "NAMECHGLONGMIN");
                //    if (objSYS_PREFT_Info != null)
                //    {
                //        MinTime = decimal.Parse(objSYS_PREFT_Info.SYSValue);
                //        Session["MinTime"] = MinTime;
                //        ActiveMinTime = (int)objSYS_PREFT_Info.SYSSet;
                //        Session["ActiveMinTime"] = ActiveMinTime;
                //    }

                //    objSYS_PREFT_Info = objGeneral.GetSingleSYS_PREFT(1, GroupName.ToUpper().Trim(), "NAMECHGLONGMAX");
                //    if (objSYS_PREFT_Info != null)
                //    {
                //        MaxTime = decimal.Parse(objSYS_PREFT_Info.SYSValue);
                //        Session["MaxTime"] = MaxTime;
                //        ActiveMaxTime = (int)objSYS_PREFT_Info.SYSSet;
                //        Session["ActiveMaxTime"] = ActiveMaxTime;
                //    }

                //    objSYS_PREFT_Info = objGeneral.GetSingleSYS_PREFT(1, GroupName.ToUpper().Trim(), "NAMECHGLONGLIMITTIME");
                //    if (objSYS_PREFT_Info != null)
                //    {
                //        LimitTime = decimal.Parse(objSYS_PREFT_Info.SYSValue);
                //        Session["LimitTime"] = LimitTime;
                //    }
                //}
                //else
                //{
                //    ABS.Logic.GroupBooking.Settings objSYS_PREFT_Info = new ABS.Logic.GroupBooking.Settings();

                //    objSYS_PREFT_Info = objGeneral.GetSingleSYS_PREFT(1, GroupName.ToUpper().Trim(), "NAMECHGSHORTMIN");
                //    if (objSYS_PREFT_Info != null)
                //    {
                //        MinTime = decimal.Parse(objSYS_PREFT_Info.SYSValue);
                //        Session["MinTime"] = MinTime;
                //        ActiveMinTime = (int)objSYS_PREFT_Info.SYSSet;
                //        Session["ActiveMinTime"] = ActiveMinTime;

                //    }

                //    objSYS_PREFT_Info = objGeneral.GetSingleSYS_PREFT(1, GroupName.ToUpper().Trim(), "NAMECHGSHORTMAX");
                //    if (objSYS_PREFT_Info != null)
                //    {
                //        MaxTime = decimal.Parse(objSYS_PREFT_Info.SYSValue);
                //        Session["MaxTime"] = MaxTime;
                //        ActiveMaxTime = (int)objSYS_PREFT_Info.SYSSet;
                //        Session["ActiveMaxTime"] = ActiveMaxTime;
                //    }

                //    objSYS_PREFT_Info = objGeneral.GetSingleSYS_PREFT(1, GroupName.ToUpper().Trim(), "NAMECHGSHORTLIMITTIME");
                //    if (objSYS_PREFT_Info != null)
                //    {
                //        LimitTime = decimal.Parse(objSYS_PREFT_Info.SYSValue);
                //        Session["LimitTime"] = LimitTime;
                //    }
                //}

                if (TimeDiff < MaxChangeTime)
                //if (TimeDiff < LimitTime)
                {
                    Session["IsAllow"] = 0;
                    btConfirm.ClientVisible = false;
                }
                else
                {
                    Session["IsAllow"] = 1;
                }
            }


            //amended by diana 20170405, show return column only if round trip flight
            //if (BookingTransactionDetail.Count > 1)
            Boolean returnFlight = false;
            returnFlight = objBooking.IsReturn(TransID, 0);
            //change to new add-On table, Tyas
            //dtPass = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTable(TransID);
            dtPass = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTableNewManage(TransID);
            paxfree = Convert.ToString((dtPass.Rows.Count * 0.3).ToString().Split('.')[0]);
            LoadComboCountry();
            Session["dtGridPass"] = dtPass;


            //20170406 - Sienny (fix redundant code)
            if (BookingTransactionDetail != null && BookingTransactionDetail.Count > 0)
            {
                if (returnFlight == true)
                {
                    gvPassenger.Columns["ReturnSeat"].Visible = true;
                    if (Regex.Replace(BookingTransactionDetail[0].Transit, @"\s+", "") != "")
                    {
                        gvPassenger.Columns["DepartConnectingSeat"].Visible = true;
                        gvPassenger.Columns["ReturnConnectingSeat"].Visible = true;

                        gvPassenger.Columns["DepartSeat"].Caption = BookingTransactionDetail[0].Origin + " - " + BookingTransactionDetail[0].Transit;
                        gvPassenger.Columns["DepartConnectingSeat"].Caption = BookingTransactionDetail[0].Transit + " - " + BookingTransactionDetail[0].Destination;

                        gvPassenger.Columns["ReturnConnectingSeat"].Caption = BookingTransactionDetail[0].Destination + " - " + BookingTransactionDetail[0].Transit;
                        gvPassenger.Columns["ReturnSeat"].Caption = BookingTransactionDetail[0].Transit + " - " + BookingTransactionDetail[0].Origin;
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

                //20170321 - Sienny
                BookingTransactionDetail = objBooking.Get_TRANSDTL(AgentSet.AgentID, TransID);
                //20170405 - Sienny (domestic flight hide PassportNo and ExpiryDate field)
                if (BookingTransactionDetail != null && BookingTransactionDetail.Count > 0)
                {
                    IsInternationalFlight = objGeneral.IsInternationalFlight(BookingTransactionDetail[0].Origin, BookingTransactionDetail[0].Destination, Request.PhysicalApplicationPath);
                }

                //added by romy for insure
                if (Application["InsureEnable"] != null && Convert.ToBoolean(Application["InsureEnable"]) == true)
                {
                    gvPassenger.Columns["DepartInsure"].Visible = true;
                }
                else
                {
                    gvPassenger.Columns["DepartInsure"].Visible = false;
                }

                //added by romy
                if (!IsInternationalFlight)
                {
                    gvPassenger.Columns["PassportNo"].Visible = false;
                    gvPassenger.Columns["ExpiryDate"].Visible = false;
                }
                else
                {
                    gvPassenger.Columns["PassportNo"].Visible = true;
                    gvPassenger.Columns["ExpiryDate"].Visible = true;
                }

                //20170405 - Sienny (domestic flight hide PassportNo and ExpiryDate field)
                //IsInternationalFlight = objGeneral.IsInternationalFlight(BookingTransactionDetail[0].Origin, BookingTransactionDetail[0].Destination, Request.PhysicalApplicationPath);

                //if (!IsInternationalFlight)
                //{
                //    int numberOfRecords = dtPass.Select("(PassportNo = '' OR PassportNo = 'TBA') AND ExpiryDate IS NULL").Length;
                //    //cek if passportno and expirydate are not empty
                //    if (numberOfRecords == dtPass.Rows.Count)
                //    {
                //        gvPassenger.Columns["PassportNo"].Visible = false;
                //        gvPassenger.Columns["ExpiryDate"].Visible = false;
                //    }
                //    else
                //    {
                //        gvPassenger.Columns["PassportNo"].Visible = true;
                //        gvPassenger.Columns["ExpiryDate"].Visible = true;
                //    }
                //}
                //else
                //{
                //    gvPassenger.Columns["PassportNo"].Visible = true;
                //    gvPassenger.Columns["ExpiryDate"].Visible = true;
                //}
            }


            dtInfant = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTableInfant(TransID);
            if (dtInfant != null && dtInfant.Rows.Count > 0)
            {
                if (dtPass != null && dtPass.Rows.Count > 0)
                {
                    for (int i = 0; i < dtInfant.Rows.Count; i++)
                    {
                        DataRow[] PaxNo = dtPass.Select("PassengerID = '" + dtInfant.Rows[i]["PassengerID"].ToString() + "' AND PNR = '" + dtInfant.Rows[i]["RecordLocator"].ToString() + "'");
                        dtInfant.Rows[i]["PaxNo"] = PaxNo[0]["RowNo"];
                        dtInfant.Rows[i]["ParentFirstName"] = PaxNo[0]["FirstName"];
                        dtInfant.Rows[i]["ParentlastName"] = PaxNo[0]["lastName"];
                        dtInfant.Rows[i]["Nationality"] = PaxNo[0]["Nationality"];
                        dtInfant.Rows[i]["IssuingCountry"] = PaxNo[0]["IssuingCountry"];
                    }
                }
                Session["dtInfant"] = dtInfant;

                ////20170330 - Sienny (domestic flight hide PassportNo and ExpiryDate field)
                if (!IsInternationalFlight)
                {
                    //20170405 - Sienny (change method for hide PassportNo and ExpiryDate field)
                    int numberOfRecords = dtInfant.Select("(PassportNo = '' OR PassportNo = 'TBA') AND ExpiryDate IS NULL").Length;
                    //cek if passportno and expirydate are not empty
                    if (numberOfRecords == dtInfant.Rows.Count)
                    {
                        gvInfant.Columns["PassportNo"].Visible = false;
                        gvInfant.Columns["ExpiryDate"].Visible = false;
                    }
                    else
                    {
                        gvInfant.Columns["PassportNo"].Visible = true;
                        gvInfant.Columns["ExpiryDate"].Visible = true;
                    }
                }
                else
                {
                    gvInfant.Columns["PassportNo"].Visible = true;
                    gvInfant.Columns["ExpiryDate"].Visible = true;
                }
            }
            else
            {
                gvInfant.Visible = false;
                divInfant.Style.Add("display", "none");
            }

            if (tempstatus == 3)
            {
                // gvPassenger.Columns["Details"].Visible = true;
                mainContainer.Visible = false;
                lblPassengerNote.Visible = true;
                gvPassenger.Columns["ChangeCnt"].Visible = true;
                btnDlGuide.Visible = false;
                btnUpload.Visible = false;
                btnDl.Visible = false;
                //int infantcount = 0;
                //if (Session["dtInfant"] != null)
                //{
                //    dtInfant = (DataTable)Session["dtInfant"];
                //    if (dtInfant.Rows.Count > 0)
                //    {

                //        for (int i = 0; i < dtInfant.Rows.Count; i++)
                //        {
                //            if (dtInfant.Rows[i]["FirstName"].ToString() == "Infant" && dtInfant.Rows[i]["Lastname"].ToString() == "Infant")
                //            {
                //                infantcount += 1;
                //            }
                //        }
                //    }
                //}
                //Session["infantcount"] = infantcount;
                //if (infantcount == 0)
                //{
                btConfirm.ClientVisible = false;
                spanTitle.InnerText = "Name Change";
                lblPassengerDetailsSub.Text = "Name Change";
                divTotalAmount.Style.Add("display", "block");

                custommessage = "";//added by romy for custom message
                custommessage = msgList.Err800009.Replace("pax", paxfree);
                if (paxfree == "0")
                {
                    custommessage = msgList.Err800010;
                }
                indicator.Text = custommessage;
                //}
                //else
                //{
                //    Session["CurStatus"] = 2;
                //}
                //btConfirm.Enabled = false;
                //added by ketee, disable the upload panel if passenger uploaded 
                divUploadPanel.Visible = false;
            }
            else if (tempstatus == 2)
            {
                // gvPassenger.Columns["Details"].Visible = true;
                gvPassenger.Columns["ChangeCnt"].Visible = false;
                mainContainer.Visible = true;
                lblPassengerNote.Visible = true;
                btnUpload.Enabled = true;
                btConfirm.Enabled = true;
                Session["IsAllow"] = 1;
                divindicator.Style.Add("visibility", "hidden");
            }
            else
            {
                Response.Redirect(Shared.MySite.PublicPages.InvalidPage);
            }

            //for (int i = 0; i < dtPass.Rows.Count;i++ )
            //{
            //    if (objBooking.AddNameChangeFees(dtPass.Rows[i]["SessionID"].ToString(), dtPass.Rows[i]["Currency"].ToString(), Convert.ToInt32(dtPass.Rows[i]["PassengerID"]), dtPass.Rows[i]["PNR"].ToString()) == true)
            //    //if (objBooking.GetPaymentFee(dtPass.Rows[i]["Currency"].ToString(), Convert.ToDecimal(bookHDRInfo.TransTotalAmt), dtPass.Rows[i]["PNR"].ToString()) == true)
            //    {

            //    }
            //}
        }

        //added by diana 20170306, remove any session related to payment
        protected void ClearSessionData()
        {
            //HttpContext.Current.Session.Remove("TempFlight");
            //HttpContext.Current.Session.Remove("dataClass");
            //HttpContext.Current.Session.Remove("ErrorPayment");
            //HttpContext.Current.Session.Remove("dataClassTrans");

            bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(TransID);
            objBooking.FillDataTableTransMain(bookHDRInfo);
            int transStatus = 0;

            //added by diana 20130923, update flight & passenger details

            bool callFunction = false;

            if (Session["generatePayment"] == null)
                callFunction = true;
            else if (Session["generatePayment"].ToString() == "")
                callFunction = true;

            if (Session["modePage"] != null && Session["modePage"].ToString() == "agent")
            {
                if (Session["AgentSet"] != null)
                {
                    objBooking.ClearExpiredJourney(AgentSet.AgentID, TransID);
                }
                if (Session["AgentSet"] != null && callFunction == true)
                {
                    //commented by diana 20131114

                    //temp remarked navitaire update
                    //objBooking.UpdateBookingJourneyDetails(TransID, AgentSet.AgentName, AgentSet.AgentID, true);
                    //objBooking.UpdatePaymentDetails(TransID, AgentSet.AgentName, AgentSet.AgentID, true);

                    //replace the new get booking from Navitaire
                    List<ListTransaction> AllTransaction = new List<ListTransaction>();
                    AllTransaction = objBooking.GetTransactionDetails(TransID);
                    if (AllTransaction != null && AllTransaction.Count > 0)
                    {
                        ListTransaction lstTrans = AllTransaction[0];
                        List<BookingTransactionDetail> VoidPNRs = new List<BookingTransactionDetail>();
                        List<BookingTransactionDetail> ExpiredPNRs = new List<BookingTransactionDetail>();
                        if (objBooking.UpdateAllBookingJourneyDetails(lstTrans, lstTrans.AgentUserName.ToString(), lstTrans.AgentID.ToString(), ref VoidPNRs, ref ExpiredPNRs, true) == false)
                        {
                            log.Warning(this, "Fail to Get Latest Update for Transaction - passengerdetail.aspx.cx : " + lstTrans.TransID);
                            if (ConfigurationManager.AppSettings["ErrorLogEmail"] != null && ConfigurationManager.AppSettings["ErrorLogEmail"] != "")
                            {
                                eWS.CustomEmail(ConfigurationManager.AppSettings["ErrorLogEmail"].ToString(), lstTrans.TransID, "", "Errors Log :" + DateTime.Now.ToString() + " <br/> Fail to Get Latest Update for Transaction : " + lstTrans.TransID);
                            }
                        }
                    }

                }
            }
            Session["generatePayment"] = "";

            byte TransStatus = 0;

            TransStatus = objBooking.GetTransStatus(TransID);
            if (TransStatus != 0)
            {
                bookHDRInfo.TransStatus = TransStatus;
            }

            if (Session["modePage"].ToString() == "agent")
            {
                if (Session["AgentSet"] != null && (bookHDRInfo.TransStatus == 2 || bookHDRInfo.TransStatus == 3))
                {
                    //temp remarked navitaire update
                    objBooking.UpdatePassengerDetails(TransID, AgentSet.AgentName, AgentSet.AgentID, true);

                    if (bookHDRInfo.TransStatus == 2)
                    {
                        //added by diana 20140605, if passenger complete, then status should be 3 or confirmed
                        bool GetPassengerComplete = false;
                        GetPassengerComplete = objBooking.CheckCompletePassenger(TransID);
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
                // lstbookFlightInfo = objBooking.GetAllBK_TRANSDTLFlight(TransID);
            }

            //lstbookPaymentInfo = objBooking.GetAllBK_TRANSTENDERFilter(TransID, " BK_TRANSTENDER.TransVoid=0 AND ");

            //Session["dtGridDetail"] = lstbookDTLInfo;
            //Session["dtRejectedGridDetail"] = lstRejectedbookDTLInfo;

            //Boolean returnFlight = false;
            //returnFlight = objBooking.IsReturn(TransID, 0);
            //lstFareBreakdown = objBooking.GetAllBK_TRANSDTLFareBreakdown(TransID, 0);
            //dtFareBreakdown = objBooking.GetAllBK_TRANSDTLFareBreakdown3(TransID, 0, RecordLocator, returnFlight);
            //Session["dtFareBreakdown"] = dtFareBreakdown;
            //lstFareBreakdownReturn = objBooking.GetAllBK_TRANSDTLFlightGrpNoSellKey1(TransID, 0);

            //dtPass = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTable(TransID);
            //Session["dtPassenger"] = dtPass;

            //dtAddOn = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTableManage2(TransID);
            //Session["dtAddOn"] = dtAddOn;
        }

        protected void InitValue()
        {
            Session["infantcount"] = null;
            Session["CurStatus"] = 0;
            Session["dtInfant"] = null;
            Session["dtGridPass"] = null;
            Session["dtNewGridPass"] = null;
            Session["pos"] = null;
            Session["PassengerID"] = null;
            Session["Posted"] = null;
            Session["InsureCode"] = null;//added by romy for insure

            Session["MinTime"] = null;
            Session["MaxTime"] = null;
            Session["LimitTime"] = null;
            Session["TimeDiff"] = null;
            Session["IsAllow"] = null;

            Session["EffectiveDate"] = null;
            Session["IsFree1"] = null;
            Session["IsFree2"] = null;
            Session["MaxFreeTime1"] = null;
            Session["MaxFreeTime2"] = null;
            Session["MaxChangeTime"] = null;
            Session["ChangeLimit"] = null;
            Session["MaxPax1"] = null;
            Session["MaxPax2"] = null;

            Session["bookHDRInfo"] = null;

            objBooking.ClearSessionData();
        }

        protected void LoadGridView()
        {
            gvPassenger.DataSource = dtPass;
            gvPassenger.DataBind();
            if (Session["error"] != null)
            {
                gvPassenger.Columns["ErrorMsg"].Visible = true;
            }
            else
            {
                gvPassenger.Columns["ErrorMsg"].Visible = false;
            }

            gvInfant.DataSource = dtInfant;
            gvInfant.DataBind();
        }

        protected void ActionDataPanel_PerformCallback(object sender, CallbackEventArgsBase e)
        {
            lblData.InnerText = "Incorrect Data";
            if (Session["counterror"] != null)
                countData.Text = Session["counterror"].ToString();
            Session["counterror"] = null;
            ActionDataPanel.ClientVisible = true;
        }

        protected void LoadGridViewCallBack()
        {
            if (Session["dtNewGridPass"] != null)
                gvPassenger.DataSource = (DataTable)Session["dtNewGridPass"];
            else
                gvPassenger.DataSource = (DataTable)Session["dtGridPass"];

            gvPassenger.DataBind();
            if (Session["error"] != null)
            {
                gvPassenger.Columns["ErrorMsg"].Visible = true;
                //Session["error"] = null;
            }
            else
            {
                gvPassenger.Columns["ErrorMsg"].Visible = false;
            }

            dtInfant = (DataTable)Session["dtInfant"];
            gvInfant.DataSource = (DataTable)Session["dtInfant"];
            gvInfant.DataBind();
        }

        protected void LoadTotalAmount()
        {
            DataTable dtPass = (DataTable)Session["dtGridPass"];
            object sumObject;
            sumObject = dtPass.Compute("Sum(ChangeFee)", "");
            object sumObject2;
            sumObject2 = dtPass.Compute("Sum(ChangeFee2)", "");
            //var col = dtPass.Columns["PaxNo"];
            //foreach (DataRow row in dtPass.Rows)
            //{
            //    row["PaxNo"] = 0;
            //}
            lblTotalAmount.Text = "0.00"; //(Convert.ToDecimal(sumObject) + Convert.ToDecimal(sumObject2)).ToString("N", nfi);

            lblCurrency.Text = dtPass.Rows[0]["Currency"].ToString();
        }

        protected void LoadComboCountry()
        {
            UIClass.SetComboStyle(ref cmbNation, UIClass.EnumDefineStyle.CountryCard);
            UIClass.SetComboStyle(ref cmbPassCountry, UIClass.EnumDefineStyle.CountryCard);
            // cmbNation.ValueField = "countryName";
            // cmbPassCountry.ValueField = "countryName";
            // cmbNation.DataBind();
            // cmbPassCountry.DataBind();
        }

        protected void callbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            MessageList msgList = new MessageList();
            LoadComboCountry();
            dtPass = (DataTable)Session["dtGridPass"];
            int i = 0;
            foreach (DataRow dr in dtPass.Rows)
            {
                if (dr["PassengerID"].ToString() == (e.Parameter).ToString())
                {
                    pos = i;
                    Session["pos"] = pos;
                }
                i++;
            }
            pos = (int)Session["pos"];

            ClearDetail();

            litText.Text = (e.Parameter).ToString() + pos.ToString();

            if (dtPass.Rows[pos]["Title"].ToString() != "")
            {
                string a = dtPass.Rows[pos]["Title"].ToString();
                if (a.Length > 1)
                {
                    a = a.Substring(0, 1).ToUpper() + a.Substring(1, a.Length - 1).ToLower();
                    cmbTitle.Items.FindByValue(a.ToString()).Selected = true;
                }
            }
            if (dtPass.Rows[pos]["Gender"].ToString() != "")
            {
                string a = dtPass.Rows[pos]["Gender"].ToString();
                if (a.Length > 1)
                {
                    a = a.Substring(0, 1).ToUpper() + a.Substring(1, a.Length - 1).ToLower();
                    cmbGender.Items.FindByValue(a.ToString()).Selected = true;
                }
            }
            if (dtPass.Rows[pos]["Nationality"].ToString() != "")
            {
                string a = dtPass.Rows[pos]["Nationality"].ToString();
                if (a.Length > 1)
                {
                    //a = a.Substring(0, 1).ToUpper() + a.Substring(1, a.Length - 1).ToLower();
                    cmbNation.Items.FindByValue(a.ToString()).Selected = true;
                }
            }
            if (dtPass.Rows[pos]["IssuingCountry"].ToString() != "")
            {
                string a = dtPass.Rows[pos]["IssuingCountry"].ToString();
                if (a.Length > 1)
                {
                    //a = a.Substring(0, 1).ToUpper() + a.Substring(1, a.Length - 1).ToLower();
                    cmbPassCountry.Items.FindByValue(a.ToString()).Selected = true;
                }
            }
            if (dtPass.Rows[pos]["DOB"].ToString() != "")
            {
                txtDOB.Value = Convert.ToDateTime(dtPass.Rows[pos]["DOB"].ToString());
            }
            if (dtPass.Rows[pos]["ExpiryDate"].ToString() != "")
            {
                txtExpired.Value = Convert.ToDateTime(dtPass.Rows[pos]["ExpiryDate"].ToString()); ;
            }

            txt_FirstName.Text = dtPass.Rows[pos]["FirstName"].ToString();
            txt_LastName.Text = dtPass.Rows[pos]["LastName"].ToString();

            txt_PrevFirstName.Value = dtPass.Rows[pos]["FirstName"].ToString();
            txt_PrevLastName.Value = dtPass.Rows[pos]["LastName"].ToString();
            txt_PrevFirstName1.Value = dtPass.Rows[pos]["PrevFirstName1"].ToString();
            txt_PrevLastName1.Value = dtPass.Rows[pos]["PrevLastName1"].ToString();
            txt_PrevFirstName2.Value = dtPass.Rows[pos]["PrevFirstName2"].ToString();
            txt_PrevLastName2.Value = dtPass.Rows[pos]["PrevLastName2"].ToString();

            txtPassportNo.Text = dtPass.Rows[pos]["PassportNo"].ToString();
            if (Session["Save"] != null)
            {
                if (Session["Save"].ToString() == "1")
                {
                    litText.Visible = true;
                    litText.Text = msgList.Err999991;
                    imgSuccess.Visible = true;
                    lblErrorPass.Visible = false;
                    Session["Save"] = 0;
                }
                else
                {
                    litText.Visible = false;
                    imgSuccess.Visible = false;
                }
            }
            else
            {
                litText.Visible = false;
                imgSuccess.Visible = false;
            }

            //allow name change
            if ((int)Session["CurStatus"] == 2) //(dtPass.Rows[pos]["FirstName"].ToString() == "" && dtPass.Rows[pos]["LastName"].ToString() == "")
            {
                SetReadOnly(false, false);
            }
            else
            {
                if ((int)dtPass.Rows[pos]["ChangeCount"] < ChangeLimit)
                    //if ((int)dtPass.Rows[pos]["ChangeCount"] < NameChangeMax)
                    SetReadOnly(false, true);
                else
                    SetReadOnly(true, true);
            }

            //added by ketee 20140611, verify if pax already update from Navitaire, name change is nor allowed.
            //if (dtPass.Rows[pos]["ChangeCount"].ToString() == "1")
            //{
            //    SetEditable(false);
            //}
            //else
            //{
            //    SetEditable(true);
            //}

            ltrNumber.Text = "Passenger " + (pos + 1).ToString() + " of " + dtPass.Rows.Count;
            gvPassenger.DataSource = dtPass;
            gvPassenger.DataBind();
            gvInfant.DataSource = (DataTable)Session["dtInfant"];
            gvInfant.DataBind();
            if (Session["error"] != null)
            {
                gvPassenger.Columns["ErrorMsg"].Visible = true;
            }
        }

        protected void ClearDetail()
        {
            cmbTitle.SelectedIndex = -1;
            cmbGender.SelectedIndex = -1;
            cmbNation.SelectedIndex = -1;
            cmbPassCountry.SelectedIndex = -1;
            txtDOB.Value = null;
            txtExpired.Value = null;
            txtPassportNo.Text = "";
            bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(TransID);
            //txtExpired.MinDate = bookHDRInfo.STDDate;
            txtDOB.MinDate = DateTime.Parse("1900-01-01");
            txtDOB.MaxDate = bookHDRInfo.STDDate.AddDays(-9);
            txtAdt.MinDate = DateTime.Parse("1900-01-01");
            txtAdt.MaxDate = bookHDRInfo.STDDate.AddYears(-12);
            txtChd.MinDate = bookHDRInfo.STDDate.AddYears(-12);
            txtChd.MaxDate = bookHDRInfo.STDDate.AddDays(-9);
        }

        protected void SetReadOnly(bool enablename, bool enableother)
        {
            txt_FirstName.ReadOnly = enablename;
            txt_LastName.ReadOnly = enablename;

            cmbTitle.ReadOnly = enableother;
            cmbGender.ReadOnly = enableother;
            cmbNation.ReadOnly = enableother;
            cmbPassCountry.ReadOnly = enableother;
            txtPassportNo.ReadOnly = enableother;
            txtDOB.ReadOnly = enableother;
            txtExpired.ReadOnly = enableother;
            txtDOB.ReadOnly = enableother;
            txtDOB.ReadOnly = enableother;
            txtAdt.ReadOnly = enableother;
            txtAdt.ReadOnly = enableother;
            txtChd.ReadOnly = enableother;
            txtChd.ReadOnly = enableother;
        }

        //added by ketee
        //protected void SetEditable(Boolean Open)
        //{
        //    cmbTitle.Enabled = Open;
        //    cmbGender.Enabled = Open;
        //    cmbNation.Enabled = Open;
        //    cmbPassCountry.Enabled = Open;
        //    txtDOB.Enabled = Open;
        //    txtExpired.Enabled = Open;
        //    txtDOB.Enabled = Open;
        //    txtDOB.Enabled = Open;
        //    txtAdt.Enabled = Open;
        //    txtAdt.Enabled = Open;
        //    txtChd.Enabled = Open;
        //    txtChd.Enabled = Open;
        //    btSave.Visible = Open;
        //    txt_FirstName.Enabled = Open;
        //    txt_LastName.Enabled = Open;
        //    txtPassportNo.Enabled = Open;
        //}

        protected void gvPassenger_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                if (e.Parameters != null && e.Parameters == "confirm")
                {
                    confirmAction();
                }
                else if (e.Parameters != null && e.Parameters == "download")
                {
                    string appPath = "";
                    if (Session["DirPathUpload"] != null)
                    {
                        appPath = Session["DirPathUpload"].ToString();
                    }
                    string excelDictionary = appPath + "ErrorPassengerUpload\\" + TransID;
                    if (!Directory.Exists(excelDictionary))
                    {
                        System.IO.Directory.CreateDirectory(excelDictionary);
                    }
                    var excelFileName = Path.Combine(excelDictionary, "ErrorPassengerList.xls");

                    using (SpreadsheetDocument document = SpreadsheetDocument.Create(excelFileName, SpreadsheetDocumentType.Workbook))
                    {
                        WorkbookPart workbookPart = document.AddWorkbookPart();
                        workbookPart.Workbook = new Workbook();

                        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                        var sheetData = new SheetData();
                        worksheetPart.Worksheet = new Worksheet(sheetData);

                        Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                        Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };

                        sheets.Append(sheet);

                        WorkbookStylesPart stylesheet = document.WorkbookPart.AddNewPart<WorkbookStylesPart>();
                        stylesheet.Stylesheet = new Stylesheet();

                        stylesheet.Stylesheet.Fonts = new Fonts();
                        stylesheet.Stylesheet.Fonts.Count = 1;
                        stylesheet.Stylesheet.Fonts.AppendChild(new Font());

                        // create fills
                        stylesheet.Stylesheet.Fills = new Fills();

                        // create a solid red fill
                        var solidRed = new PatternFill() { PatternType = PatternValues.Solid };
                        //solidRed.ForegroundColor = new ForegroundColor { Rgb = HexBinaryValue.FromString("f9a2d2") }; // red fill
                        solidRed.ForegroundColor = new ForegroundColor { Rgb = HexBinaryValue.FromString("BD081C") };
                        solidRed.BackgroundColor = new BackgroundColor { Indexed = 64 };

                        stylesheet.Stylesheet.Fills.AppendChild(new Fill { PatternFill = new PatternFill { PatternType = PatternValues.None } }); // required, reserved by Excel
                        stylesheet.Stylesheet.Fills.AppendChild(new Fill { PatternFill = new PatternFill { PatternType = PatternValues.Gray125 } }); // required, reserved by Excel
                        stylesheet.Stylesheet.Fills.AppendChild(new Fill { PatternFill = solidRed });
                        stylesheet.Stylesheet.Fills.Count = 3;

                        // blank border list
                        stylesheet.Stylesheet.Borders = new Borders();
                        DocumentFormat.OpenXml.Spreadsheet.Border border2 = new DocumentFormat.OpenXml.Spreadsheet.Border();

                        LeftBorder leftBorder2 = new LeftBorder() { Style = BorderStyleValues.Thin };
                        Color color1 = new Color() { Indexed = (UInt32Value)64U };

                        leftBorder2.Append(color1);

                        RightBorder rightBorder2 = new RightBorder() { Style = BorderStyleValues.Thin };
                        Color color2 = new Color() { Indexed = (UInt32Value)64U };

                        rightBorder2.Append(color2);

                        TopBorder topBorder2 = new TopBorder() { Style = BorderStyleValues.Thin };
                        Color color3 = new Color() { Indexed = (UInt32Value)64U };

                        topBorder2.Append(color3);

                        BottomBorder bottomBorder2 = new BottomBorder() { Style = BorderStyleValues.Thin };
                        Color color4 = new Color() { Indexed = (UInt32Value)64U };

                        bottomBorder2.Append(color4);
                        DiagonalBorder diagonalBorder2 = new DiagonalBorder();

                        border2.Append(leftBorder2);
                        border2.Append(rightBorder2);
                        border2.Append(topBorder2);
                        border2.Append(bottomBorder2);
                        border2.Append(diagonalBorder2);


                        //stylesheet.Stylesheet.Borders.Count = 1;
                        stylesheet.Stylesheet.Borders.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Border());
                        stylesheet.Stylesheet.Borders.AppendChild(border2);
                        stylesheet.Stylesheet.Borders.Count = 1;

                        Font font1 = new Font(
                            new Bold(),
                            new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 11 },
                            new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                            new FontName() { Val = "Calibri" });
                        stylesheet.Stylesheet.Fonts.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Font());
                        stylesheet.Stylesheet.Fonts.AppendChild(font1);
                        stylesheet.Stylesheet.Fonts.Count = 1;
                        // blank cell format list
                        stylesheet.Stylesheet.CellStyleFormats = new CellStyleFormats();
                        stylesheet.Stylesheet.CellStyleFormats.Count = 1;
                        stylesheet.Stylesheet.CellStyleFormats.AppendChild(new CellFormat());

                        // cell format list
                        stylesheet.Stylesheet.CellFormats = new CellFormats();
                        // empty one for index 0, seems to be required
                        stylesheet.Stylesheet.CellFormats.AppendChild(new CellFormat());
                        // cell format references style format 0, font 0, border 0, fill 2 and applies the fill pink
                        stylesheet.Stylesheet.CellFormats.AppendChild(new CellFormat { FormatId = 0, FontId = 0, BorderId = 1, FillId = 2, ApplyFill = true, ApplyBorder = true }).AppendChild(new Alignment { Horizontal = HorizontalAlignmentValues.Center, ShrinkToFit = true });
                        // cell format references style format 0, font 0, border 0, fill 0 and applies the fill default
                        stylesheet.Stylesheet.CellFormats.AppendChild(new CellFormat { FormatId = 0, FontId = 0, BorderId = 1, FillId = 0, ApplyFill = true, ApplyBorder = true }).AppendChild(new Alignment { Horizontal = HorizontalAlignmentValues.Center, ShrinkToFit = true });

                        stylesheet.Stylesheet.CellFormats.AppendChild(new CellFormat { FormatId = 0, FontId = 0, BorderId = 1, FillId = 0, ApplyFill = true, ApplyBorder = true }).AppendChild(new Alignment { WrapText = true });

                        stylesheet.Stylesheet.CellFormats.AppendChild(new CellFormat { FormatId = 0, FontId = 1, BorderId = 1, FillId = 0, ApplyFill = true, ApplyBorder = true }).AppendChild(new Alignment { Horizontal = HorizontalAlignmentValues.Center, ShrinkToFit = true });

                        stylesheet.Stylesheet.CellFormats.AppendChild(new CellFormat { FormatId = 0, FontId = 1, BorderId = 1, FillId = 0, ApplyFill = true, ApplyBorder = true }).AppendChild(new Alignment { Horizontal = HorizontalAlignmentValues.Center, WrapText = true });

                        stylesheet.Stylesheet.CellFormats.Count = 6;

                        stylesheet.Stylesheet.Save();

                        Row headerRow = new Row();
                        DataTable dtExport = (DataTable)Session["dtGridPass"];
                        List<String> columns = new List<string>();


                        foreach (System.Data.DataColumn column in dtExport.Columns)
                        {
                            if (gvExport.Columns[column.ColumnName] != null && column.ColumnName != "IssuingCountry" && column.ColumnName != "Nationality")
                            {
                                columns.Add(column.ColumnName);
                                Cell cell = new Cell();
                                if (column.ColumnName == "DOB" || column.ColumnName == "ExpiryDate")
                                {
                                    cell.DataType = CellValues.Date;
                                }
                                else
                                {
                                    cell.DataType = CellValues.String;
                                }
                                if (column.ColumnName == "ErrorMsg")
                                {
                                    cell.CellValue = new CellValue("ErrorDescription");
                                    cell.StyleIndex = Convert.ToUInt32(5);
                                }
                                else if (column.ColumnName == "IssuingCountryName")
                                {
                                    cell.CellValue = new CellValue("IssuingCountry");
                                    cell.StyleIndex = Convert.ToUInt32(4);
                                }
                                else if (column.ColumnName == "CountryName")
                                {
                                    cell.CellValue = new CellValue("Nationality");
                                    cell.StyleIndex = Convert.ToUInt32(4);
                                }
                                else
                                {
                                    cell.CellValue = new CellValue(column.ColumnName);
                                    cell.StyleIndex = Convert.ToUInt32(4);
                                }

                                headerRow.AppendChild(cell);
                            }
                        }

                        sheetData.AppendChild(headerRow);

                        ArrayList arrayerror = new ArrayList();
                        if (Session["arrayerror"] != null)
                        {
                            arrayerror = (ArrayList)Session["arrayerror"];
                        }
                        var format = "d/M/yyyy hh:mm:ss tt";
                        string namecol = "";
                        foreach (DataRow dsrow in dtExport.Rows)
                        {
                            Row newRow = new Row();
                            foreach (String col in columns)
                            {
                                if (col == "CountryName")
                                {
                                    namecol = "Nationality";
                                }
                                else if (col == "IssuingCountryName")
                                {
                                    namecol = "IssuingCountry";
                                }
                                else
                                {
                                    namecol = col;
                                }
                                Cell cell = new Cell();
                                //if (col == "DOB" || col == "ExpiryDate")
                                //{
                                //    cell.DataType = CellValues.Date;
                                //}
                                //else
                                //{
                                cell.DataType = CellValues.String;
                                //}


                                if (arrayerror.Count > 0)
                                {
                                    for (int i = 0; i < arrayerror.Count; i++)
                                    {
                                        string[] field = arrayerror[i].ToString().Split(';');
                                        if (namecol == field[0].ToString() && Convert.ToInt16(dsrow[11]) == Convert.ToInt16(field[1]) + 1)
                                        {
                                            cell.StyleIndex = Convert.ToUInt32(1);
                                        }

                                        else if (namecol == "ErrorMsg")
                                        {
                                            cell.StyleIndex = Convert.ToUInt32(3);
                                        }
                                        else if (!arrayerror.Contains(namecol + ";" + (Convert.ToInt16(dsrow[11]) - 1)))
                                        {
                                            cell.StyleIndex = Convert.ToUInt32(2);
                                        }
                                    }
                                }


                                if (col == "DOB" || col == "ExpiryDate")
                                {
                                    //log.Info(this, "dsrow[col].ToString() : " + dsrow[col].ToString());
                                    DateTime temp;
                                    if (DateTime.TryParse(dsrow[col].ToString(), out temp))
                                    {
                                        var d = Convert.ToDateTime(dsrow[col].ToString());
                                        //var d = DateTime.ParseExact(dsrow[col].ToString(), format, System.Globalization.CultureInfo.InvariantCulture);
                                        cell.CellValue = new CellValue(d.ToString("yyyy-MM-dd"));
                                    }
                                    else
                                    {
                                        cell.CellValue = new CellValue("");
                                    }
                                }
                                else
                                {
                                    cell.CellValue = new CellValue(dsrow[col].ToString());
                                }
                                //cell.StyleIndex = Convert.ToUInt32(2);
                                newRow.AppendChild(cell);
                            }

                            sheetData.AppendChild(newRow);
                        }

                        //for (int i = 1; i < 10; i++)
                        //{
                        //    SetColumnWidth(worksheetPart.Worksheet, Convert.ToUInt32(i),100,true);
                        //}
                        ////SetColumnWidth(worksheetPart.Worksheet, Convert.ToUInt32(10), 50, true);
                        //worksheetPart.Worksheet.AppendChild(sheetData);
                        //worksheetPart.Worksheet.Save();

                        workbookPart.Workbook.Save();

                    }

                    string CopyFromPath = excelFileName.ToString();
                    int LengthFiles = excelFileName.ToString().Split('\\').Length;
                    string filename = excelFileName.ToString().Split('\\')[LengthFiles - 1];
                    //Dim CopyToPath As String = HttpContext.Current.Server.MapPath("~/Temp/" & filename)
                    string CopyToFolder = Session["DirPathUpload"].ToString() + "Temp\\" + objGeneral.GenerateRandom(6);
                    if (!Directory.Exists(CopyToFolder))
                    {
                        System.IO.Directory.CreateDirectory(CopyToFolder);
                    }

                    var CopyToPath = Path.Combine(CopyToFolder, "ErrorPassengerList.xls");
                    if (!System.IO.File.Exists(CopyToPath))
                    {
                        System.IO.File.Copy(CopyFromPath, CopyToPath, true);
                    }
                    gvPassenger.JSProperties["cp_result"] = "download|" + CopyToPath;
                    Session["error"] = null;

                }
                else
                {
                    gvPassenger.DataSource = (DataTable)Session["dtGridPass"];
                    gvPassenger.DataBind();
                    gvInfant.DataSource = (DataTable)Session["dtInfant"];
                    gvInfant.DataBind();
                    if (Session["error"] != null)
                    {
                        gvPassenger.Columns["ErrorMsg"].Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
        }

        static void SetColumnWidth(Worksheet worksheet, uint Index, DoubleValue dwidth, bool hidden = false)
        {
            DocumentFormat.OpenXml.Spreadsheet.Columns cs = worksheet.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Columns>();
            if (cs != null)
            {
                IEnumerable<DocumentFormat.OpenXml.Spreadsheet.Column> ic = cs.Elements<DocumentFormat.OpenXml.Spreadsheet.Column>().Where(r => r.Min == Index).Where(r => r.Max == Index);
                if (ic.Count() > 0)
                {
                    DocumentFormat.OpenXml.Spreadsheet.Column c = ic.First();
                    c.Width = dwidth;
                }
                else
                {
                    cs = new DocumentFormat.OpenXml.Spreadsheet.Columns();
                    DocumentFormat.OpenXml.Spreadsheet.Column c = new DocumentFormat.OpenXml.Spreadsheet.Column() { Min = Index, Max = Index, Hidden = hidden, Width = dwidth, CustomWidth = true };
                    cs.Append(c);
                    worksheet.InsertAfter(cs, worksheet.GetFirstChild<SheetProperties>());
                }
            }
            else
            {
                cs = new DocumentFormat.OpenXml.Spreadsheet.Columns();
                DocumentFormat.OpenXml.Spreadsheet.Column c = new DocumentFormat.OpenXml.Spreadsheet.Column() { Min = Index, Max = Index, Hidden = hidden, Width = dwidth, CustomWidth = true };
                cs.Append(c);
                worksheet.InsertAfter(cs, worksheet.GetFirstChild<SheetProperties>());
            }
        }

        protected void gvInfant_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters != null && e.Parameters == "confirm")
            {
                confirmAction();
            }
            else
            {
                gvInfant.DataSource = (DataTable)Session["dtInfant"];
                gvInfant.DataBind();
            }
        }

        protected void gvPassenger_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            object rowKey = null;

            if (e.ButtonID == "editBtn")
            {
                rowKey = gvPassenger.GetRowValues(e.VisibleIndex, "PassengerID");

                Session["PassengerID"] = rowKey;

                // DevExpress.Web.ASPxWebControl.RedirectOnCallback("../Admin/agentdetail.aspx?optmode=2");
            }
        }

        protected void btNext_Click(object sender, EventArgs e)
        {
            assignPos();
            if (pos < dtPass.Rows.Count - 1)
                pos++;
            loadPopup();
        }

        protected void btPrev_Click(object sender, EventArgs e)
        {
            assignPos();
            if (pos > 0)
                pos--;
            loadPopup();
        }

        protected void btConfirm_Click(object sender, EventArgs e)
        {
            confirmAction();
        }

        public bool IsValidName(string Name)
        {
            string chars = ":@#$%&_+*-=~^`',.:;!?|/(){}[]<>0123456789";
            int charlength = chars.Length;
            for (int x = 0; x < charlength; x++)
            {
                if (Name.StartsWith(chars.Substring(x, 1)))
                    return false;
            }
            return true;
        }

        protected void confirmAction()
        {
            MessageList msgList = new MessageList();
            imgError.Visible = false;
            imgSuccessMsg.Visible = false;
            decimal number;
            MessageList msglst = new MessageList();
            Boolean rValue = true;

            ////20170321 - Sienny
            //BookingTransactionDetail = objBooking.Get_TRANSDTL(AgentSet.AgentID, TransID);
            ////20170405 - Sienny (domestic flight hide PassportNo and ExpiryDate field)
            //if (BookingTransactionDetail != null && BookingTransactionDetail.Count > 0)
            //{
            //    IsInternationalFlight = objGeneral.IsInternationalFlight(BookingTransactionDetail[0].Origin, BookingTransactionDetail[0].Destination, Request.PhysicalApplicationPath);
            //}

            Session["dtOldGridPass"] = Session["dtGridPass"];
            if ((int)Session["CurStatus"] == 3 || Session["dtNewGridPass"] != null)
            {
                dtPass = Session["dtNewGridPass"] as DataTable;
                if (Session["dtNewGridPass"] != null)
                {
                    Session["dtGridPass"] = Session["dtNewGridPass"];
                }
            }
            if (dtPass == null)
            {
                if (Session["dtGridPass"] != null)
                    dtPass = (DataTable)Session["dtGridPass"];
                else
                {
                    hCommand.Value = msgList.Err200095;
                    imgError.Visible = true;
                    imgSuccessMsg.Visible = false;
                    return;
                }
                if (dtPass == null)
                {
                    hCommand.Value = msgList.Err200095;
                    imgError.Visible = true;
                    imgSuccessMsg.Visible = false;
                    return;
                }
            }
            if (dtInfant == null)
            {
                if (Session["dtInfant"] != null)
                    dtInfant = (DataTable)Session["dtInfant"];
            }
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;
            try
            {
                using (profiler.Step("assignPos"))
                {
                    assignPos();
                }
                DateTime dateValue;
                DateTime dtNow = DateTime.Now;
                bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(TransID);
                int tempstatus = bookHDRInfo.TransStatus;
                string tempdate1 = String.Format("{0:MM/dd/yyyy}", bookHDRInfo.STDDate);
                string tempdate2 = String.Format("{0:MM/dd/yyyy}", DateTime.Now);
                TimeSpan ts = Convert.ToDateTime(tempdate1) - Convert.ToDateTime(tempdate2);
                int temphour = Convert.ToInt32(ts.TotalHours.ToString());
                if (temphour > 5)
                {
                    //lstbookDTLInfo = objBooking.GetAllBK_TRANSDTLFilterOrderByPNR(TransID, 0);
                    lstbookDTLInfo = objBooking.GetAllBK_TRANSDTLCombinePNROrderByPNR(TransID);
                    DataTable dtPassenger = new DataTable();
                    dtPassenger = objBooking.GetAllBK_PASSENGERLISTDataTable(TransID);
                    List<PassengerData> lstPassengerData2 = new List<PassengerData>();
                    List<PassengerData> lstPassInfantData2 = new List<PassengerData>();
                    string strErrorTitle = msgList.Err200077;
                    string strErrorDOB = msgList.Err200078;
                    string strErrorDOBADT = msgList.Err200079;
                    string strErrorDOBCHD = msgList.Err200080;
                    string strErrorExp = msgList.Err200081;
                    string strErrorInfantDOB = msgList.Err200082;
                    string strErrorFirstName = msgList.Err200083;
                    string strErrorLastName = msgList.Err200084;
                    string strErrorFirstNameInfant = msgList.Err200085;
                    string strErrorLastNameInfant = msgList.Err200086;
                    string strErrorDOBInft = msgList.Err200088;
                    string strErrorNati = msgList.Err200089;
                    string strErrorInfantReq = msgList.Err200090;
                    string strErrorCoun = msgList.Err200091;
                    string strErrorExpPass = msgList.Err200092;
                    string strErrorExpInfant = msgList.Err200093;
                    //string strErrorInfantDOB2 = "Failed to save Infant data, Please fill in DOB for Infant ";
                    int isErrorInfantReq = 0, isErrorNati = 0, isErrorCoun = 0, isErrorTitle = 0, isErrorDOB = 0, isErrorDOBInft = 0, isErrorDOBADT = 0, isErrorDOBCHD = 0, isErrorExp = 0, isErrorExpPass = 0, isErrorExpInfant = 0, isErrorFirstName = 0, isErrorLastName = 0, sErrorFirstNameInfant = 0, isErrorLastNameInfant = 0, intPassNo = 1;

                    //Added by Ellis 20170322
                    string strErrorNameEmpty = msgList.Err200087;
                    int isErrorNameEmpty = 0;

                    string strErrorNameInfantEmpty = msgList.Err200094;
                    int isErrorNameInfantEmpty = 0;

                    int index = 0;
                    foreach (DataRow drRow in dtPass.Rows)
                    {
                        drRow["ChangeDate"] = DateTime.Now;
                        drRow["SyncLastUpd"] = DateTime.Now;
                        drRow["LastSyncBy"] = AgentSet.AgentID;
                        PassData = new PassengerData();
                        PassData.TransID = (string)drRow["TransID"];

                        //PassData.RecordLocator = (string)drRow["RecordLocator"];
                        PassData.RecordLocator = (string)drRow["PNR"];

                        PassData.PassengerID = (string)drRow["PassengerID"];
                        PassData.Title = (string)drRow["Title"].ToString().ToUpper();
                        PassData.Gender = (string)drRow["Gender"].ToString().ToUpper();
                        PassData.FirstName = (string)drRow["FirstName"].ToString().ToUpper();
                        PassData.LastName = (string)drRow["LastName"].ToString().ToUpper();
                        PassData.PrevFirstName1 = (string)drRow["PrevFirstName1"];
                        PassData.PrevLastName1 = (string)drRow["PrevLastName1"];
                        if (DateTime.TryParse(drRow["PrevDOB1"].ToString(), out dateValue))
                        {
                            PassData.PrevDOB1 = (DateTime)drRow["PrevDOB1"];
                        }
                        if (DateTime.TryParse(drRow["PrevExpiryDate1"].ToString(), out dateValue))
                        {
                            PassData.PrevExpiryDate1 = (DateTime)drRow["PrevExpiryDate1"];
                        }
                        PassData.PrevGender1 = (string)drRow["PrevGender1"];
                        PassData.PrevTitle1 = (string)drRow["PrevTitle1"];
                        PassData.PrevNationality1 = (string)drRow["PrevNationality1"];
                        PassData.PrevIssuingCountry1 = (string)drRow["PrevIssuingCountry1"];
                        PassData.PrevPassportNo1 = (string)drRow["PrevPassportNo1"];
                        if (DateTime.TryParse(drRow["PrevDOB2"].ToString(), out dateValue))
                        {
                            PassData.PrevDOB2 = (DateTime)drRow["PrevDOB2"];
                        }
                        if (DateTime.TryParse(drRow["PrevExpiryDate2"].ToString(), out dateValue))
                        {
                            PassData.PrevExpiryDate2 = (DateTime)drRow["PrevExpiryDate2"];
                        }
                        PassData.PrevGender2 = (string)drRow["PrevGender2"];
                        PassData.PrevTitle2 = (string)drRow["PrevTitle2"];
                        PassData.PrevNationality2 = (string)drRow["PrevNationality2"];
                        PassData.PrevIssuingCountry2 = (string)drRow["PrevIssuingCountry2"];
                        PassData.PrevPassportNo2 = (string)drRow["PrevPassportNo2"];
                        PassData.PrevFirstName2 = (string)drRow["PrevFirstName2"];
                        PassData.PrevLastName2 = (string)drRow["PrevLastName2"];

                        PassData.Nationality = (string)drRow["Nationality"];
                        //PassData.Nationality = (string)drRow["countryName"];

                        if (dtPassenger != null && dtPassenger.Rows.Count > index)
                        {
                            if (dtPassenger.Rows[index]["Title"].ToString().ToUpper() == "CHD" && PassData.Title.ToUpper() != "CHD")
                            {
                                strErrorTitle += intPassNo + ", ";
                                isErrorTitle = 1;
                            }
                            else if (dtPassenger.Rows[index]["Title"].ToString().ToUpper() != "CHD" && PassData.Title.ToUpper() == "CHD")
                            {
                                strErrorTitle += intPassNo + ", ";
                                isErrorTitle = 1;
                            }

                            //Added by Ellis 20170322
                            if (PassData.FirstName == String.Empty || PassData.LastName == String.Empty)
                            {
                                strErrorNameEmpty += intPassNo + ", ";
                                isErrorNameEmpty = 1;
                            }
                            else
                            {
                                //added by diana 20170323, to check valid name, may not begin with non alphabet character
                                if (IsValidName(PassData.FirstName) == false)
                                {
                                    strErrorFirstName += intPassNo + ", ";
                                    isErrorFirstName = 1;
                                }
                                if (IsValidName(PassData.LastName) == false)
                                {
                                    strErrorLastName += intPassNo + ", ";
                                    isErrorLastName = 1;
                                }
                            }
                        }


                        if (DateTime.TryParse(drRow["DOB"].ToString(), out dateValue))
                        {
                            PassData.DOB = (DateTime)drRow["DOB"];
                            //Start add by agus : adding DOB and expired date validation
                            if (PassData.DOB.AddDays(9) > bookHDRInfo.STDDate)
                            {
                                strErrorDOB += intPassNo + ", ";
                                isErrorDOB = 1;
                            }
                            else if (dtPassenger.Rows[index]["Title"].ToString().ToUpper() != "CHD" && PassData.DOB > dtNow.AddYears(-12))
                            {
                                strErrorDOBADT += intPassNo + ", ";
                                isErrorDOBADT = 1;
                            }
                            else if (dtPassenger.Rows[index]["Title"].ToString().ToUpper() == "CHD" && PassData.DOB < dtNow.AddYears(-12))
                            {
                                strErrorDOBCHD += intPassNo + ", ";
                                isErrorDOBCHD = 1;
                            }
                            //End add by agus
                        }

                        PassData.IssuingCountry = (string)drRow["IssuingCountry"];

                        //PassData.PassportNo = (string)drRow["PassportNo"];
                        //if (DateTime.TryParse(drRow["ExpiryDate"].ToString(), out dateValue))
                        //{
                        //    PassData.ExpiryDate = (DateTime)drRow["ExpiryDate"];
                        //    //Start add by agus : adding DOB and expired date validation
                        //    if (PassData.ExpiryDate < bookHDRInfo.STDDate)
                        //    {
                        //        strErrorExp += intPassNo + ", ";
                        //        isErrorExp = 1;
                        //    }
                        //    //End add by agus
                        //}


                        //20170321 - Sienny
                        if (!IsInternationalFlight)
                        {
                            System.Diagnostics.Debug.WriteLine("DOMESTIC");
                            PassData.PassportNo = "";
                            PassData.ExpiryDate = Convert.ToDateTime(null);
                        }
                        else
                        {
                            PassData.PassportNo = (string)drRow["PassportNo"];
                            if (DateTime.TryParse(drRow["ExpiryDate"].ToString(), out dateValue))
                            {
                                PassData.ExpiryDate = (DateTime)drRow["ExpiryDate"];
                                //Start add by agus : adding DOB and expired date validation
                                if (PassData.ExpiryDate < bookHDRInfo.STDDate)
                                {
                                    strErrorExp += intPassNo + ", ";
                                    isErrorExp = 1;
                                }
                                //End add by agus
                            }
                        }

                        index++;

                        PassData.ChangeCount = (int)drRow["ChangeCount"];
                        //change first by Eko 
                        PassData.MaxChange = 1;
                        PassData.MaxPax1 = (int)drRow["MaxPax1"];
                        PassData.MaxPax2 = (int)drRow["MaxPax2"];

                        //Added by romy for insurance
                        PassData.InsureFee = (decimal)drRow["InsureFee"];

                        //PassData.rowguid = (Guid)drRow["rowguid"];
                        if (DateTime.TryParse(drRow["SyncCreate"].ToString(), out dateValue)) PassData.SyncCreate = (DateTime)drRow["SyncCreate"];
                        if (DateTime.TryParse(drRow["SyncLastUpd"].ToString(), out dateValue)) PassData.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
                        PassData.LastSyncBy = (string)drRow["LastSyncBy"];

                        if (decimal.TryParse(drRow["ChangeFee"].ToString(), out number)) PassData.ChangeFee = System.Convert.ToDecimal(drRow["ChangeFee"]);
                        if (DateTime.TryParse(drRow["ChangeDate"].ToString(), out dateValue)) PassData.ChangeDate = (DateTime)drRow["ChangeDate"];
                        if (decimal.TryParse(drRow["ChangeFee2"].ToString(), out number)) PassData.ChangeFee2 = System.Convert.ToDecimal(drRow["ChangeFee2"]);
                        if (DateTime.TryParse(drRow["ChangeDate2"].ToString(), out dateValue)) PassData.ChangeDate2 = (DateTime)drRow["ChangeDate2"];

                        lstPassengerData.Add(PassData);
                        if (drRow["FirstName"].ToString() != "" && drRow["FirstName"].ToString() != "TBA") //&& drRow["ChangeCount"].ToString() != "1")
                        {
                            lstPassengerData2.Add(PassData);
                        }
                        intPassNo++;
                    }

                    intPassNo = 1;
                    if (dtInfant != null)
                    {
                        foreach (DataRow drRow in dtInfant.Rows)
                        {
                            if (drRow["FirstName"].ToString() == "Infant" || drRow["LastName"].ToString() == "Infant" || drRow["FirstName"].ToString() == "TBA" || drRow["LastName"].ToString() == "TBA")
                            {
                                strErrorInfantReq += intPassNo + ", ";
                                isErrorInfantReq = 1;
                            }
                            else
                            {
                                drRow["SyncLastUpd"] = DateTime.Now;
                                drRow["LastSyncBy"] = AgentSet.AgentID;
                                PassData2 = new PassengerData();
                                PassData2.TransID = (string)drRow["TransID"];
                                PassData2.RecordLocator = (string)drRow["RecordLocator"];
                                PassData2.PassengerID = (string)drRow["PassengerID"];
                                PassData2.Title = (string)drRow["Title"].ToString().ToUpper();
                                PassData2.Gender = (string)drRow["Gender"].ToString().ToUpper();
                                PassData2.FirstName = (string)drRow["FirstName"].ToString().ToUpper();
                                PassData2.LastName = (string)drRow["LastName"].ToString().ToUpper();
                                if (drRow["Nationality"] != null && (string)drRow["Nationality"] != "")
                                {
                                    PassData2.Nationality = (string)drRow["Nationality"];
                                }
                                else
                                {
                                    strErrorNati += intPassNo + ", ";
                                    isErrorNati = 1;
                                }

                                if (drRow["DOB"] != null && DateTime.TryParse(drRow["DOB"].ToString(), out dateValue))
                                {
                                    PassData2.DOB = (DateTime)drRow["DOB"];
                                    //Start add by agus : adding DOB and expired date validation
                                    if (PassData2.DOB.AddDays(9) > bookHDRInfo.STDDate)
                                    {
                                        strErrorDOBInft += intPassNo + ", ";
                                        isErrorDOBInft = 1;
                                    }
                                    else if (PassData2.DOB < DateTime.Now.AddMonths(-24))
                                    {
                                        strErrorInfantDOB += intPassNo + ", ";
                                        isErrorDOBInft = 1;
                                        gvPassenger.JSProperties["cp_result"] = strErrorInfantDOB;
                                        return;
                                    }
                                    //End add by agus
                                }
                                if (drRow["IssuingCountry"] != null && (string)drRow["IssuingCountry"] != "")
                                {
                                    PassData2.IssuingCountry = (string)drRow["IssuingCountry"];
                                }
                                else
                                {
                                    strErrorCoun += intPassNo + ", ";
                                    isErrorCoun = 1;
                                }

                                if (PassData2.FirstName == "Infant" || PassData2.LastName == "Infant" || PassData2.FirstName == "TBA" || PassData2.LastName == "TBA")
                                {
                                    strErrorNameInfantEmpty += intPassNo + ", ";
                                    isErrorNameInfantEmpty = 1;
                                }
                                else
                                {
                                    //added by diana 20170323, to check valid name, may not begin with non alphabet character
                                    if (IsValidName(PassData2.FirstName) == false)
                                    {
                                        strErrorFirstNameInfant += intPassNo + ", ";
                                        sErrorFirstNameInfant = 1;
                                    }
                                    if (IsValidName(PassData2.LastName) == false)
                                    {
                                        strErrorLastNameInfant += intPassNo + ", ";
                                        isErrorLastNameInfant = 1;
                                    }
                                }
                                //PassData2.PassportNo = (string)drRow["PassportNo"];
                                //if (DateTime.TryParse(drRow["ExpiryDate"].ToString(), out dateValue))
                                //{
                                //    PassData2.ExpiryDate = (DateTime)drRow["ExpiryDate"];
                                //}


                                //20170321 - Sienny
                                if (!IsInternationalFlight)
                                {
                                    //System.Diagnostics.Debug.WriteLine("DOMESTIC");
                                    PassData2.PassportNo = "";
                                    PassData2.ExpiryDate = Convert.ToDateTime(null);
                                }
                                else
                                {
                                    if (drRow["PassportNo"] != null && drRow["PassportNo"].ToString().Trim() == "")
                                    {
                                        strErrorExpPass += intPassNo + ", ";
                                        isErrorExpPass = 1;
                                    }
                                    else
                                    {
                                        PassData2.PassportNo = (string)drRow["PassportNo"];
                                    }

                                    if (drRow["ExpiryDate"] != null && DateTime.TryParse(drRow["ExpiryDate"].ToString(), out dateValue))
                                    {
                                        PassData2.ExpiryDate = (DateTime)drRow["ExpiryDate"];
                                        //Start add by agus : adding DOB and expired date validation
                                        if (PassData2.ExpiryDate < bookHDRInfo.STDDate)
                                        {
                                            strErrorExpInfant += intPassNo + ", ";
                                            isErrorExpInfant = 1;
                                        }
                                        //End add by agus
                                    }
                                    else
                                    {
                                        strErrorExpInfant += intPassNo + ", ";
                                        isErrorExpInfant = 1;
                                    }
                                }
                            }


                            //PassData.rowguid = (Guid)drRow["rowguid"];
                            if (DateTime.TryParse(drRow["SyncCreate"].ToString(), out dateValue)) PassData2.SyncCreate = (DateTime)drRow["SyncCreate"];
                            if (DateTime.TryParse(drRow["SyncLastUpd"].ToString(), out dateValue)) PassData2.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
                            PassData2.LastSyncBy = (string)drRow["LastSyncBy"];

                            lstPassInfantData.Add(PassData2);
                            if (drRow["FirstName"].ToString() != "" && drRow["FirstName"].ToString() != "TBA") //&& drRow["ChangeCount"].ToString() != "1")
                            {
                                lstPassInfantData2.Add(PassData2);
                            }
                            intPassNo++;
                        }
                    }

                    lblErrorPass.Visible = true;
                    //isErrorNameEmpty == 1 Added by Ellis 20170322
                    if (isErrorInfantReq == 1 || isErrorCoun == 1 || isErrorNati == 1 || isErrorTitle == 1 || isErrorFirstName == 1 || isErrorExpPass == 1 || isErrorExpInfant == 1 || isErrorLastName == 1 || isErrorDOB == 1 || isErrorDOBADT == 1 || isErrorDOBCHD == 1 || isErrorExp == 1 || isErrorNameEmpty == 1 || isErrorNameInfantEmpty == 1 || sErrorFirstNameInfant == 1 || isErrorLastNameInfant == 1 || isErrorDOBInft == 1)
                    {
                        imgError.Visible = true;
                        imgSuccessMsg.Visible = false;

                        string ErrorPass = "";
                        string cpResult = "";
                        if (isErrorTitle == 1)
                        {
                            if (ErrorPass != "") ErrorPass += "\n";
                            if (cpResult != "") cpResult += " ; ";

                            ErrorPass += "<font color=\"red\"> <B>" + strErrorTitle + "</B></font>";
                            cpResult += strErrorTitle;
                        }
                        if (isErrorInfantReq == 1)
                        {
                            if (ErrorPass != "") ErrorPass += "\n";
                            if (cpResult != "") cpResult += " ; ";

                            ErrorPass += "<font color=\"red\"> <B>" + strErrorInfantReq + "</B></font>";
                            cpResult += strErrorInfantReq;
                        }
                        //Added by Ellis 20170322
                        if (isErrorNameEmpty == 1)
                        {
                            if (ErrorPass != "") ErrorPass += "<br />";
                            if (cpResult != "") cpResult += " ; ";

                            ErrorPass += "<font color=\"red\"> <B>" + strErrorNameEmpty + "</B></font>";
                            cpResult += strErrorNameEmpty;
                        }
                        if (isErrorNati == 1)
                        {
                            if (ErrorPass != "") ErrorPass += "<br />";
                            if (cpResult != "") cpResult += " ; ";

                            ErrorPass += "<font color=\"red\"> <B>" + strErrorNati + "</B></font>";
                            cpResult += strErrorNati;
                        }
                        if (isErrorCoun == 1)
                        {
                            if (ErrorPass != "") ErrorPass += "<br />";
                            if (cpResult != "") cpResult += " ; ";

                            ErrorPass += "<font color=\"red\"> <B>" + strErrorCoun + "</B></font>";
                            cpResult += strErrorCoun;
                        }
                        if (isErrorNameInfantEmpty == 1)
                        {
                            if (ErrorPass != "") ErrorPass += "<br />";
                            if (cpResult != "") cpResult += " ; ";

                            ErrorPass += "<font color=\"red\"> <B>" + strErrorNameEmpty + "</B></font>";
                            cpResult += strErrorNameInfantEmpty;
                        }
                        //End added by Ellis 20170322
                        if (isErrorFirstName == 1)
                        {
                            if (ErrorPass != "") ErrorPass += "\n";
                            if (cpResult != "") cpResult += " ; ";

                            ErrorPass += "<font color=\"red\"> <B>" + strErrorFirstName + "</B></font>";
                            cpResult += strErrorFirstName;
                        }
                        if (sErrorFirstNameInfant == 1)
                        {
                            if (ErrorPass != "") ErrorPass += "\n";
                            if (cpResult != "") cpResult += " ; ";

                            ErrorPass += "<font color=\"red\"> <B>" + strErrorFirstName + "</B></font>";
                            cpResult += strErrorFirstNameInfant;
                        }
                        if (isErrorLastName == 1)
                        {
                            if (ErrorPass != "") ErrorPass += "\n";
                            if (cpResult != "") cpResult += " ; ";

                            ErrorPass += "<font color=\"red\"> <B>" + strErrorLastName + "</B></font>";
                            cpResult += strErrorLastName;
                        }
                        if (isErrorLastNameInfant == 1)
                        {
                            if (ErrorPass != "") ErrorPass += "\n";
                            if (cpResult != "") cpResult += " ; ";

                            ErrorPass += "<font color=\"red\"> <B>" + strErrorLastName + "</B></font>";
                            cpResult += strErrorLastNameInfant;
                        }
                        if (isErrorDOB == 1)
                        {
                            if (ErrorPass != "") ErrorPass += "<br />";
                            if (cpResult != "") cpResult += " ; ";

                            ErrorPass += "<font color=\"red\"> <B>" + strErrorDOB + "</B></font>";
                            cpResult += strErrorDOB;
                        }
                        if (isErrorDOBInft == 1)
                        {
                            if (ErrorPass != "") ErrorPass += "<br />";
                            if (cpResult != "") cpResult += " ; ";

                            ErrorPass += "<font color=\"red\"> <B>" + strErrorDOB + "</B></font>";
                            cpResult += strErrorDOBInft;
                        }
                        if (isErrorDOBADT == 1)
                        {
                            if (ErrorPass != "") ErrorPass += "<br />";
                            if (cpResult != "") cpResult += " ; ";

                            ErrorPass += "<font color=\"red\"> <B>" + strErrorDOBADT + "</B></font>";
                            cpResult += strErrorDOBADT;
                        }
                        if (isErrorDOBCHD == 1)
                        {
                            if (ErrorPass != "") ErrorPass += "<br />";
                            if (cpResult != "") cpResult += " ; ";

                            ErrorPass += "<font color=\"red\"> <B>" + strErrorDOBCHD + "</B></font>";
                            cpResult += strErrorDOBCHD;
                        }
                        if (isErrorExp == 1)
                        {
                            if (ErrorPass != "") ErrorPass += "<br />";
                            if (cpResult != "") cpResult += " ; ";

                            ErrorPass += "<font color=\"red\"> <B>" + strErrorExp + "</B></font>";
                            cpResult += strErrorExp;
                        }
                        if (isErrorExpInfant == 1)
                        {
                            if (ErrorPass != "") ErrorPass += "<br />";
                            if (cpResult != "") cpResult += " ; ";

                            ErrorPass += "<font color=\"red\"> <B>" + strErrorExpInfant + "</B></font>";
                            cpResult += strErrorExpInfant;
                        }
                        if (isErrorExpPass == 1)
                        {
                            if (ErrorPass != "") ErrorPass += "<br />";
                            if (cpResult != "") cpResult += " ; ";

                            ErrorPass += "<font color=\"red\"> <B>" + strErrorExpPass + "</B></font>";
                            cpResult += strErrorExpPass;
                        }
                        lblErrorPass.Text = ErrorPass;
                        gvPassenger.JSProperties["cp_result"] = cpResult;

                        if (Session["dtOldGridPass"] != null)
                        {
                            Session["dtGridPass"] = Session["dtOldGridPass"];
                        }
                        //LoadGridViewCallBack(); //added by diana 20170323, to show back last values
                        return;
                    }
                    else
                    {
                        lblErrorPass.Visible = false;
                    }

                    //changes by ketee
                    string err = string.Empty;
                    List<PassengerData> rLstPassenger = new List<PassengerData>();
                    List<PassengerData> rLstPassengerInfant = new List<PassengerData>();

                    //amended by diana 20170324, update to navitaire only when payment is completed
                    bool SavePax = false;
                    if ((int)Session["CurStatus"] != 3)
                    {
                        using (profiler.Step("LstPassenger"))
                        {
                            rLstPassenger = objBooking.SaveBK_PASSENGERLIST(lstPassengerData2, CoreBase.EnumSaveType.Update);
                        }

                        if (lstPassInfantData.Count > 0)
                        {
                            using (profiler.Step("LstPassengerInfant"))
                            {
                                rLstPassengerInfant = objBooking.SaveBK_PASSENGERLISTINFT(lstPassInfantData, CoreBase.EnumSaveType.Update);
                            }
                        }
                    }
                    else
                    {
                        Dictionary<string, decimal> BalanceAmount = new Dictionary<string, decimal>();
                        BalanceAmount = (Dictionary<string, decimal>)Session["BalanceAmount"];

                        if (BalanceAmount != null)
                        {
                            foreach (var Key in BalanceAmount.Keys)
                            {
                                if (BalanceAmount[Key] <= 0)
                                {
                                    List<PassengerData> PaxList = lstPassengerData2;
                                    List<PassengerData> InfantList = lstPassInfantData;

                                    if (PaxList != null)
                                    {
                                        PaxList = PaxList.FindAll(item => item.RecordLocator == Key);
                                        using (profiler.Step("LstPassenger"))
                                        {
                                            rLstPassenger = objBooking.SaveBK_PASSENGERLIST(PaxList, CoreBase.EnumSaveType.Update);
                                        }
                                    }
                                    if (InfantList != null && InfantList.Count > 0)
                                    {
                                        InfantList = InfantList.FindAll(item => item.RecordLocator == Key);
                                        using (profiler.Step("LstPassengerInfant"))
                                        {
                                            rLstPassengerInfant = objBooking.SaveBK_PASSENGERLISTINFT(InfantList, CoreBase.EnumSaveType.Update);
                                        }
                                    }
                                }
                            }
                        }
                        //else
                        //{
                        //    if (lstPassInfantData.Count > 0)
                        //    {
                        //        using (profiler.Step("LstPassengerInfant"))
                        //        {
                        //            rLstPassengerInfant = objBooking.SaveBK_PASSENGERLISTINFT(lstPassInfantData, CoreBase.EnumSaveType.Update);
                        //        }
                        //    }
                        //}
                    }

                    if (lstPassengerData2 != null && lstPassengerData2.Count > 0)
                    {
                        //amended by diana 20170324, update to navitaire only when payment is completed
                        if ((int)Session["CurStatus"] != 3)
                        {
                            if (rLstPassenger == null || rLstPassenger.Count <= 0)
                            {
                                rValue = false;
                                gvPassenger.JSProperties["cp_result"] = msglst.Err100030;
                                return;
                                //throw new Exception(msglst.Err100030);
                            }
                            foreach (BookingTransactionDetail bookdtl in lstbookDTLInfo)
                            {
                                //20170321 - Sienny
                                //parameter added (IsInternationalFlight) in UpdatePassengersList to validate passportno and expirydate field only if international

                                //List<PassengerData> lstPassengerData3 = (List < PassengerData >) lstPassengerData2.Select(a => a.RecordLocator = bookdtl.RecordLocator);
                                if (objBooking.UpdatePassengersList(lstPassengerData2, lstPassInfantData, bookdtl.RecordLocator, ref err, IsInternationalFlight) == false)
                                {
                                    rValue = false;
                                    throw new Exception(err);
                                }
                            }
                        }
                        else
                        {
                            Dictionary<string, decimal> BalanceAmount = new Dictionary<string, decimal>();
                            BalanceAmount = (Dictionary<string, decimal>)Session["BalanceAmount"];

                            //if (rLstPassenger == null || rLstPassenger.Count <= 0)
                            //{
                            //    throw new Exception(msglst.Err100030);
                            //}
                            foreach (BookingTransactionDetail bookdtl in lstbookDTLInfo)
                            {
                                if (BalanceAmount == null || BalanceAmount[bookdtl.RecordLocator] <= 0)
                                {
                                    List<PassengerData> PaxList = lstPassengerData2;
                                    List<PassengerData> InfantList = lstPassInfantData;

                                    if (PaxList != null)
                                    {
                                        PaxList = PaxList.FindAll(item => item.RecordLocator == bookdtl.RecordLocator);
                                        objBooking.SaveBK_PASSENGERLIST(PaxList, CoreBase.EnumSaveType.Update);
                                    }
                                    if (InfantList != null && InfantList.Count > 0)
                                    {
                                        InfantList = InfantList.FindAll(item => item.RecordLocator == bookdtl.RecordLocator);
                                        objBooking.SaveBK_PASSENGERLISTINFT(InfantList, CoreBase.EnumSaveType.Update);
                                    }
                                    if (objBooking.UpdatePassengersList(PaxList, InfantList, bookdtl.RecordLocator, ref err, IsInternationalFlight) == false)
                                    {
                                        rValue = false;
                                        throw new Exception(err);
                                    }
                                }

                            }
                        }


                        if (rLstPassenger.FindIndex(item => item.FirstName.ToString().ToUpper() == "TBA") < 0 && rValue)
                        {
                            bookHDRInfo.TransStatus = 3;

                            //tyas
                            if (Session["BalanceNameChange"] != null)
                            {
                                bookHDRInfo.TransTotalAmt = bookHDRInfo.TransTotalAmt + Convert.ToDecimal(Session["BalanceNameChange"]);
                                bookHDRInfo.TransTotalFee = bookHDRInfo.TransTotalFee + Convert.ToDecimal(Session["BalanceNameChange"]);
                                bookHDRInfo.TransSubTotal = bookHDRInfo.TransSubTotal + Convert.ToDecimal(Session["BalanceNameChange"]);
                                if (Convert.ToDecimal(bookHDRInfo.TotalAmtReturn) == 0)
                                {
                                    bookHDRInfo.TotalAmtGoing = bookHDRInfo.TotalAmtGoing + Convert.ToDecimal(Session["BalanceNameChange"]);
                                }
                                else
                                {
                                    bookHDRInfo.TotalAmtGoing = bookHDRInfo.TotalAmtGoing + (Convert.ToDecimal(Session["BalanceNameChange"]) / 2);
                                    bookHDRInfo.TotalAmtReturn = bookHDRInfo.TotalAmtReturn + (Convert.ToDecimal(Session["BalanceNameChange"]) / 2);
                                }
                                bookHDRInfo.TotalAmtAVG = bookHDRInfo.TotalAmtAVG + (Convert.ToDecimal(Session["BalanceNameChange"]) / bookHDRInfo.TransTotalPAX);

                                objBooking.FillChgTransMain(bookHDRInfo);
                                HttpContext.Current.Session.Remove("bookingMain");
                                HttpContext.Current.Session.Add("bookingMain", bookHDRInfo);

                                ArrayList PNR = new ArrayList();
                                List<BookingTransactionDetail> lstBookDTL = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 0, "LEN(RecordLocator) >= 6 AND ");
                                List<BookingTransactionDetail> OldlstBookDTL = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 0, "LEN(RecordLocator) >= 6 AND ");
                                decimal totalsum = 0;
                                foreach (BookingTransactionDetail bkDTL in lstBookDTL)
                                {
                                    //if (!PNR.Contains(bkDTL.RecordLocator))
                                    //{
                                    PNR.Add(bkDTL.RecordLocator);
                                    object sumObject;
                                    sumObject = dtPass.Compute("Sum(ChangeFee)", "PNR = '" + bkDTL.RecordLocator + "' AND PaxNo = 1");
                                    object sumObject2;
                                    sumObject2 = dtPass.Compute("Sum(ChangeFee2)", "PNR = '" + bkDTL.RecordLocator + "' AND PaxNo = 1");

                                    if ((!(sumObject is DBNull)) && (!(sumObject2 is DBNull)))
                                    {
                                        totalsum = (decimal)sumObject + (decimal)sumObject2;
                                    }
                                    else
                                    {
                                        totalsum = 0;
                                    }
                                    bkDTL.LineTotal += totalsum;
                                    bkDTL.LineNameChange += totalsum;
                                    //}
                                }
                                objBooking.FillChgTransDetail(lstBookDTL, OldlstBookDTL);
                            }

                            //Session["CurStatus"] = 3;
                            //gvPassenger.Columns["Details"].Visible = false;
                            btConfirm.Enabled = false;
                            btnUpload.Enabled = false;
                            //Response.RedirectLocation = Request.RawUrl;
                            if ((int)Session["CurStatus"] != 3)
                            {
                                resendItinerary(); //20170719 - Sienny (send itinerary after passengerlist complete)

                                objBooking.SaveBK_TRANSMAIN(bookHDRInfo, CoreBase.EnumSaveType.Update);
                                gvPassenger.JSProperties["cp_result"] = msglst.Err200060;
                                return;
                                //Response.Redirect("bookingdetail.aspx?k=" + Request.QueryString["k"] + "&TransID=" + Request.QueryString["TransID"], false);
                            }

                        }
                        else
                        {
                            //tyas
                            if (Session["BalanceNameChange"] != null)
                            {
                                bookHDRInfo.TransTotalAmt = bookHDRInfo.TransTotalAmt + Convert.ToDecimal(Session["BalanceNameChange"]);
                                bookHDRInfo.TransTotalFee = bookHDRInfo.TransTotalFee + Convert.ToDecimal(Session["BalanceNameChange"]);
                                bookHDRInfo.TransSubTotal = bookHDRInfo.TransSubTotal + Convert.ToDecimal(Session["BalanceNameChange"]);
                                if (Convert.ToDecimal(bookHDRInfo.TotalAmtReturn) == 0)
                                {
                                    bookHDRInfo.TotalAmtGoing = bookHDRInfo.TotalAmtGoing + Convert.ToDecimal(Session["BalanceNameChange"]);
                                }
                                else
                                {
                                    bookHDRInfo.TotalAmtGoing = bookHDRInfo.TotalAmtGoing + (Convert.ToDecimal(Session["BalanceNameChange"]) / 2);
                                    bookHDRInfo.TotalAmtReturn = bookHDRInfo.TotalAmtReturn + (Convert.ToDecimal(Session["BalanceNameChange"]) / 2);
                                }
                                bookHDRInfo.TotalAmtAVG = bookHDRInfo.TotalAmtAVG + (Convert.ToDecimal(Session["BalanceNameChange"]) / bookHDRInfo.TransTotalPAX);
                                //objBooking.SaveBK_TRANSMAIN(bookHDRInfo, CoreBase.EnumSaveType.Update); //pending remarked

                                objBooking.FillChgTransMain(bookHDRInfo);
                                HttpContext.Current.Session.Remove("bookingMain");
                                HttpContext.Current.Session.Add("bookingMain", bookHDRInfo);

                                ArrayList PNR = new ArrayList();
                                List<BookingTransactionDetail> lstBookDTL = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 0, "LEN(RecordLocator) >= 6 AND ");
                                foreach (BookingTransactionDetail bkDTL in lstBookDTL)
                                {
                                    //if (!PNR.Contains(bkDTL.RecordLocator))
                                    //{
                                    PNR.Add(bkDTL.RecordLocator);
                                    object sumObject;
                                    sumObject = dtPass.Compute("Sum(ChangeFee)", "PNR = '" + bkDTL.RecordLocator + "'");
                                    object sumObject2;
                                    sumObject2 = dtPass.Compute("Sum(ChangeFee2)", "PNR = '" + bkDTL.RecordLocator + "'");

                                    decimal totalsum = (decimal)sumObject + (decimal)sumObject2;
                                    bkDTL.LineTotal += totalsum;
                                    bkDTL.LineNameChange += totalsum;
                                    //}
                                }
                                objBooking.FillChgTransDetail(lstBookDTL);
                            }
                        }
                        //gvPassenger.JSProperties["cp_result"] = msglst.Err100029;
                        imgError.Visible = false;
                        imgSuccessMsg.Visible = true;

                        foreach (DataRow row in dtPass.Rows)
                        {
                            row["PaxNo"] = 0;
                        }
                        //Response.RedirectLocation = Request.RawUrl;
                        //Amended by Ellis 20170317, eliminate Message "Object not set to reference" when only manual fill 1 passenger
                        //string a = Session["BalanceNameChange"].ToString();

                        if (Session["BalanceNameChange"] != null && Convert.ToDecimal(Session["BalanceNameChange"].ToString()) > 0)
                        {
                            //Response.RedirectLocation = "bookingdetail.aspx?k=" + Request.QueryString["k"] + "&TransID=" + Request.QueryString["TransID"];

                            //ClearSessionData();

                            Session["ChgMode"] = "1"; //1= Name Change
                            HttpContext.Current.Session.Remove("lstPassengerData");
                            HttpContext.Current.Session.Add("lstPassengerData", lstPassengerData2);

                            HttpContext.Current.Session.Remove("lstPassInfantData");
                            HttpContext.Current.Session.Add("lstPassInfantData", lstPassInfantData);

                            HttpContext.Current.Session.Remove("IsInternationalFlight");
                            HttpContext.Current.Session.Add("IsInternationalFlight", IsInternationalFlight);

                            ASPxCallback.RedirectOnCallback(GroupBooking.Web.Shared.MySite.PublicPages.Payment);

                            //begin, pending added by diana 20170307, to redirect to payment page
                            //ClearSessionData();
                            //bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(bookHDRInfo.TransID);
                            //objBooking.FillDataTableTransMain(bookHDRInfo);
                            //Response.RedirectLocation = (Shared.MySite.PublicPages.Payment3);
                            //end, pending added by diana 20170307, to redirect to payment page
                        }
                        //Amended by Ellis 20170317, if there's convertion of session balancenamechange then should check if the session exist or not
                        //else if (Convert.ToDecimal(Session["BalanceNameChange"].ToString()) == 0)
                        else if (Session["BalanceNameChange"] != null && Convert.ToDecimal(Session["BalanceNameChange"].ToString()) == 0)
                        {
                            resendItinerary(); //20170720 - Sienny (send itinerary after passengerlist complete)

                            Response.RedirectLocation = "bookingdetail.aspx?k=" + Request.QueryString["k"] + "&TransID=" + Request.QueryString["TransID"];
                            //Response.RedirectLocation = Request.RawUrl;
                        }
                        else
                        {
                            Response.RedirectLocation = "bookingdetail.aspx?k=" + Request.QueryString["k"] + "&TransID=" + Request.QueryString["TransID"];
                        }
                    }
                    else
                    {
                        gvPassenger.JSProperties["cp_result"] = msglst.Err100030;
                        //throw new Exception(msglst.Err100030);
                        return;
                    }

                    lblMsg.Visible = true;
                    imgError.Visible = false;
                    imgSuccessMsg.Visible = true;


                }
                else
                {
                    gvPassenger.JSProperties["cp_result"] = msglst.Err100037;
                    lblMsg.Visible = true;
                    imgError.Visible = false;
                    imgSuccessMsg.Visible = true;
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                gvPassenger.JSProperties["cp_result"] = ex.Message.ToString();
                lblMsg.Visible = true;
                imgError.Visible = false;
                imgSuccessMsg.Visible = true;
                log.Error(this, ex);
            }
            finally
            {

            }
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            assignPos();
            if (ASPxEdit.AreEditorsValid(callbackPanel))
            {
                dtPass.Rows[pos]["Title"] = cmbTitle.SelectedItem.Value.ToString();
                dtPass.Rows[pos]["Gender"] = cmbGender.SelectedItem.Value.ToString();
                dtPass.Rows[pos]["Nationality"] = cmbNation.SelectedItem.Value.ToString();
                dtPass.Rows[pos]["CountryName"] = cmbNation.SelectedItem.Text;
                dtPass.Rows[pos]["IssuingCountry"] = cmbPassCountry.SelectedItem.Value.ToString();
                dtPass.Rows[pos]["IssuingCountryName"] = cmbPassCountry.SelectedItem.Text;
                dtPass.Rows[pos]["DOB"] = txtDOB.Value;
                dtPass.Rows[pos]["ExpiryDate"] = txtExpired.Value;
                dtPass.Rows[pos]["FirstName"] = txt_FirstName.Text;
                dtPass.Rows[pos]["LastName"] = txt_LastName.Text;

                if ((int)Session["CurStatus"] == 3)
                {
                    if (txt_FirstName.Text != txt_PrevFirstName.Value || txt_LastName.Text != txt_PrevLastName.Value)
                    {
                        dtPass.Rows[pos]["FirstName"] = txt_FirstName.Text;
                        dtPass.Rows[pos]["LastName"] = txt_LastName.Text;
                        if ((int)dtPass.Rows[pos]["InitChange"] == 0)
                        {
                            dtPass.Rows[pos]["PrevFirstName1"] = txt_PrevFirstName.Value;
                            dtPass.Rows[pos]["ChangeCount"] = 1;
                            dtPass.Rows[pos]["ChangeFee"] = 0;
                            dtPass.Rows[pos]["ChangeDate"] = DateTime.Now;

                            dtPass.Rows[pos]["PrevLastName1"] = txt_PrevLastName.Value;
                            dtPass.Rows[pos]["ChangeCount"] = 1;
                            dtPass.Rows[pos]["ChangeFee"] = 0;
                            dtPass.Rows[pos]["ChangeDate"] = DateTime.Now;
                        }
                        else if ((int)dtPass.Rows[pos]["InitChange"] >= 1)
                        {
                            dtPass.Rows[pos]["PrevFirstName2"] = txt_PrevFirstName.Value;
                            dtPass.Rows[pos]["ChangeCount"] = 2;
                            dtPass.Rows[pos]["ChangeFee2"] = 0;
                            dtPass.Rows[pos]["ChangeDate2"] = DateTime.Now;

                            dtPass.Rows[pos]["PrevLastName2"] = txt_PrevLastName.Value;
                            dtPass.Rows[pos]["ChangeCount"] = 2;
                            dtPass.Rows[pos]["ChangeFee2"] = 0;
                            dtPass.Rows[pos]["ChangeDate2"] = DateTime.Now;
                        }
                    }

                }

                dtPass.Rows[pos]["PassportNo"] = txtPassportNo.Text;
                loadPopup();
                Session["Save"] = 1;
            }
        }

        protected void assignPos()
        {
            if (Session["pos"] != null)
                pos = (int)Session["pos"];

            dtPass = (DataTable)Session["dtGridPass"];
        }

        protected void loadPopup()
        {
            Session["dtGridPass"] = dtPass;
            Session["pos"] = pos;
        }

        protected void uplImage_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            MessageList msgList = new MessageList();
            try
            {
                e.ErrorText = "";
                Session["error"] = null;
                Session["arrayerror"] = null;
                e.CallbackData = SavePostedFile(e.UploadedFile);
                if (lblMsg.Text == string.Empty)
                {
                    if (Session["error"] != null)
                    {
                        e.CallbackData = msgList.Err999992;
                    }
                    else
                    {
                        e.CallbackData = msgList.Err999993;
                    }
                }
                else
                    e.ErrorText = lblMsg.Text;
                // Here you can implement your logic to save the uploaded file (for instance, using the SaveAs method).
                // The following line is intentionally commented to avoid saving a file in this demo in order to not overfill the free space on the server.
                // file.SaveAs(file.FileName);
            }
            catch (System.IO.IOException ex)
            {
                log.Error(this, ex);
                e.ErrorText = ex.Message;
                if (lblMsg.Text != string.Empty)
                    e.ErrorText = lblMsg.Text;
                e.CallbackData = msgList.Err999994;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
        }

        public DateTime LastDayOfMonthFromDateTime(DateTime dateTime)
        {
            DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }

        protected string SavePostedFile(UploadedFile uploadedFile)
        {
            MessageList msgList = new MessageList();
            ArrayList arrayerror = new ArrayList();
            string getFirstNamePass = "";
            string getLastNamePass = "";
            DateTime dtNow = DateTime.Now;

            TransID = Request.QueryString["TransID"];
            if (Session["bookHDRInfo"] != null) bookHDRInfo = (BookingTransactionMain)Session["bookHDRInfo"];

            CultureInfo culture;
            culture = CultureInfo.CreateSpecificCulture("en-GB");
            try
            {
                if (dtInfant == null)
                {
                    if (Session["dtInfant"] != null)
                        dtInfant = (DataTable)Session["dtInfant"];
                }

                if (!uploadedFile.IsValid)
                    return string.Empty;

                DateTime lastDateOfMonth = LastDayOfMonthFromDateTime(DateTime.Now);

                //string fileName = Path.Combine(MapPath(UploadDirectory), ThumbnailFileName);
                string fileType = string.Empty;
                fileType = uploadedFile.FileName.Remove(0, uploadedFile.FileName.Count() - 3);
                string errorMessage = "";
                int counterror = 0;
                string filePath = string.Empty;

                filePath = Request.PhysicalApplicationPath + "Upload";
                if (Directory.Exists(filePath) == false) Directory.CreateDirectory(filePath);
                filePath = filePath + "\\" + Guid.NewGuid().ToString();
                uploadedFile.SaveAs(filePath);
                DataSet ds = new DataSet();
                DataTable dttemp = new DataTable();
                //ds = ImportExcel.ImportExcelXLS( , false);
                assignPos();
                int totalPassengers = 0;
                int importedRows = 0;
                if (dtPass != null && dtPass.Rows.Count > 0)
                {
                    totalPassengers = dtPass.Rows.Count;
                    var col = dtPass.Columns["ErrorMsg"];
                    foreach (DataRow row in dtPass.Rows) row[col] = "";
                    DataTable dtPassenger = new DataTable();
                    dtPassenger = objBooking.GetAllBK_PASSENGERLISTDataTable(TransID);
                    int index = 0;
                    //ds = Csv.GetDataSet(filePath);

                    if (fileType.ToLower() == "xls")
                    {
                        //dtPass.Rows[1]["Title"] = "1";
                        //log.Info(this, filePath);
                        ds = SEAL.WEB.Common.Extension.ImportExcel.ImportExcelbyFilePath(filePath.Trim(), true);

                        //ds = ImportExcelXLS(filePath.Trim(), true);
                        //dtPass.Rows[1]["Title"] = "2";
                        //lblHeader.Text = "";
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            //added by ketee, delete blank rows 
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                if (!string.IsNullOrEmpty(dr[0].ToString()))
                                    importedRows++;
                            }

                            //check by counted rows instead of the uploaded list
                            if (importedRows == totalPassengers)
                            {
                                DataTable dt = new DataTable();
                                /*
                                dt.Columns.Add("Title");
                                dt.Columns.Add("Gender");
                                dt.Columns.Add("Nationality");
                                dt.Columns.Add("countryName");
                                dt.Columns.Add("IssuingCountry");
                                dt.Columns.Add("DOB");
                                dt.Columns.Add("ExpiryDate");
                                dt.Columns.Add("FirstName");
                                dt.Columns.Add("LastName");
                                dt.Columns.Add("PassportNo");
                                */

                                dt.Columns.Add("Nationality");
                                dt.Columns.Add("IssuingCountry");
                                dt.Columns.Add("Title");
                                dt.Columns.Add("Gender");
                                dt.Columns.Add("FirstName");
                                dt.Columns.Add("LastName");
                                dt.Columns.Add("DOB");
                                //dt.Columns.Add("PassportNo");
                                //dt.Columns.Add("ExpiryDate");

                                //20170321 - Sienny
                                if (!IsInternationalFlight)
                                {
                                }
                                else
                                {
                                    dt.Columns.Add("PassportNo");
                                    dt.Columns.Add("ExpiryDate");
                                }

                                DataRow drRow;
                                DataTable dtName = new DataTable();


                                int i = 0;

                                DateTime dateValue;
                                int dstablecnt = pos;
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    errorMessage = string.Empty;
                                    //dstablecnt++;
                                    //if (dstablecnt == 1)
                                    //    continue;

                                    if (i == totalPassengers)
                                        break;

                                    DateTime DOB = new DateTime();
                                    DateTime ExpiryDate = new DateTime();
                                    drRow = dt.NewRow();
                                    int MainField = 0;
                                    if (IsInternationalFlight)
                                    {
                                        MainField = 9;
                                    }
                                    else
                                    {
                                        MainField = 7;
                                    }
                                    //added by kete, verify if changecount = 1 then cannot modify
                                    //if ((string)dtPass.Rows[i]["FirstName"] != "" || Convert.ToInt32(dtPass.Rows[i]["ChangeCount"].ToString()) >= Convert.ToInt32(dtPass.Rows[i]["MaxChange"].ToString()))
                                    if (Convert.ToInt32(dtPass.Rows[i]["ChangeCount"].ToString()) >= ChangeLimit)
                                    {
                                        MainField = 0;
                                    }
                                    else
                                    {
                                        string title = "";
                                        int k = 1;
                                        string dtl = "";
                                        //Edited by romy, 20170927
                                        for (int j = 0; j < dt.Columns.Count; j++)
                                        {
                                            //if (dr[j].ToString() == "")
                                            //{
                                            //    MainField--;
                                            //    continue;
                                            //}
                                            if (k < ds.Tables[0].Columns.Count)
                                            {
                                                dtl = ds.Tables[0].Columns[k].ColumnName.ToString().Trim();
                                                dtl = dtl.Replace(" ", "");
                                            }
                                            string dtl1 = dt.Columns[j].ColumnName.ToString();
                                            if (dtl != dtl1)
                                            {
                                                switch (dtl1)
                                                {
                                                    case "Title":
                                                        title = "";
                                                        errorMessage += msgList.Err200063; ;
                                                        arrayerror.Add("Title;" + i);
                                                        counterror += 1;
                                                        break;

                                                    case "Gender":
                                                        errorMessage += msgList.Err200064;
                                                        arrayerror.Add("Gender;" + i);
                                                        counterror += 1;
                                                        break;

                                                    case "Nationality":
                                                        errorMessage += msgList.Err200065;
                                                        arrayerror.Add("Nationality;" + i);
                                                        counterror += 1;
                                                        break;

                                                    case "IssuingCountry":
                                                        errorMessage += msgList.Err200066;
                                                        arrayerror.Add("IssuingCountry;" + i);
                                                        counterror += 1;
                                                        break;

                                                    case "DOB":
                                                        errorMessage += msgList.Err200067;
                                                        arrayerror.Add("DOB;" + i);
                                                        counterror += 1;
                                                        break;

                                                    case "ExpiryDate":
                                                        if (IsInternationalFlight)
                                                        {
                                                            errorMessage += msgList.Err200068;
                                                            arrayerror.Add("ExpiryDate;" + i);
                                                            counterror += 1;
                                                        }
                                                        else
                                                        {
                                                            MainField--;
                                                            continue;
                                                        }

                                                        break;

                                                    case "FirstName":
                                                        MainField--;
                                                        break;

                                                    case "LastName":
                                                        MainField--;
                                                        break;

                                                    case "PassportNo":
                                                        if (IsInternationalFlight)
                                                        {
                                                            errorMessage += msgList.Err200069;
                                                            arrayerror.Add("PassportNo;" + i);
                                                            counterror += 1;
                                                        }
                                                        else
                                                        {
                                                            MainField--;
                                                        }
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                if (IsInternationalFlight)
                                                {
                                                    switch (dtl)
                                                    {
                                                        case "Title":
                                                            if (dtPassenger != null && dtPassenger.Rows.Count > i && dtPassenger.Rows[i]["Title"].ToString().ToUpper().Trim() != "CHD" && (dr[k].ToString().ToLower().Trim() == "mr" || dr[k].ToString().ToLower().Trim() == "ms" || dr[k].ToString().ToLower().Trim() == "mrs"))
                                                            {
                                                                dtPass.Rows[i]["Title"] = char.ToUpper(dr[k].ToString()[0]) + dr[k].ToString().Substring(1);
                                                                title = dr[k].ToString();
                                                                MainField--;
                                                            }
                                                            else if (dtPassenger != null && dtPassenger.Rows.Count > i && dtPassenger.Rows[i]["Title"].ToString().ToUpper().Trim() == "CHD" && (dr[k].ToString().ToLower().Trim() == "chd"))
                                                            {
                                                                dtPass.Rows[i]["Title"] = char.ToUpper(dr[k].ToString()[0]) + dr[k].ToString().Substring(1);
                                                                title = dr[k].ToString();
                                                                MainField--;
                                                            }
                                                            else
                                                            {
                                                                //errorMessage += "|Passenger" + i + " Title|";
                                                                title = "";
                                                                dtPass.Rows[i]["Title"] = char.ToUpper(dr[k].ToString()[0]) + dr[k].ToString().Substring(1);
                                                                if (dtPassenger != null && dtPassenger.Rows.Count > i && dtPassenger.Rows[i]["Title"].ToString().ToUpper().Trim() == "CHD" && (dr[k].ToString().ToLower().Trim() == "mr" || dr[k].ToString().ToLower().Trim() == "ms" || dr[k].ToString().ToLower().Trim() == "mrs"))
                                                                {
                                                                    errorMessage += msgList.Err200070;
                                                                }
                                                                else if (dtPassenger != null && dtPassenger.Rows.Count > i && dtPassenger.Rows[i]["Title"].ToString().ToUpper().Trim() != "CHD" && (dr[k].ToString().ToLower().Trim() == "chd"))
                                                                {
                                                                    errorMessage += msgList.Err200071;
                                                                }
                                                                else
                                                                {
                                                                    errorMessage += msgList.Err200063; ;
                                                                }
                                                                arrayerror.Add("Title;" + i);
                                                                counterror += 1;
                                                            }
                                                            break;

                                                        case "Gender":
                                                            if ((dr[k].ToString().ToLower().Trim() == "male" || dr[k].ToString().ToLower().Trim() == "female"))
                                                            {
                                                                if (dtPassenger != null && dtPassenger.Rows.Count > i && dtPassenger.Rows[i]["Title"].ToString().ToUpper().Trim() != "CHD" && dr[k].ToString().ToLower().Trim() == "male" && (title.ToString().ToLower().Trim() == "ms" || title.ToString().ToLower().Trim() == "mrs"))
                                                                {
                                                                    dtPass.Rows[i]["Gender"] = "Female";
                                                                    MainField--;
                                                                    //errorMessage += "If Title is Ms, Gender must be Female\n";
                                                                }
                                                                else if (dtPassenger != null && dtPassenger.Rows.Count > i && dtPassenger.Rows[i]["Title"].ToString().ToUpper().Trim() != "CHD" && dr[k].ToString().ToLower().Trim() == "female" && title.ToString().ToLower().Trim() == "mr")
                                                                {
                                                                    dtPass.Rows[i]["Gender"] = "Male";
                                                                    MainField--;
                                                                    //errorMessage += "If Title is Mr, Gender must be Male\n";
                                                                }
                                                                else
                                                                {
                                                                    dtPass.Rows[i]["Gender"] = char.ToUpper(dr[k].ToString()[0]) + dr[k].ToString().Substring(1);
                                                                    MainField--;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //errorMessage += "|Passenger" + i + " Gender|";
                                                                dtPass.Rows[i]["Gender"] = char.ToUpper(dr[k].ToString()[0]) + dr[k].ToString().Substring(1);
                                                                errorMessage += msgList.Err200064;
                                                                arrayerror.Add("Gender;" + i);
                                                                counterror += 1;
                                                            }
                                                            break;

                                                        case "Nationality":
                                                            if (dr[k].ToString() != "")
                                                            {
                                                                dtName = objGeneral.GetCountryCodeByName(dr[k].ToString());
                                                                if (dtName != null)
                                                                {
                                                                    dtPass.Rows[i]["Nationality"] = dtName.Rows[0]["CountryCode"].ToString();
                                                                    dtPass.Rows[i]["CountryName"] = dr[k].ToString();
                                                                    MainField--;

                                                                    if (dtInfant != null && dtInfant.Rows.Count > 0)
                                                                    {
                                                                        foreach (DataRow dI in dtInfant.Rows)
                                                                        {
                                                                            if (dI["RecordLocator"].ToString() == dtPass.Rows[i]["PNR"].ToString() && dI["PassengerID"].ToString() == dtPass.Rows[i]["PassengerID"].ToString())
                                                                            {
                                                                                dI["Nationality"] = dtName.Rows[0]["CountryCode"].ToString();
                                                                                dI["CountryName"] = dr[k].ToString();
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    dtPass.Rows[i]["Nationality"] = dr[k].ToString();
                                                                    dtPass.Rows[i]["CountryName"] = dr[k].ToString();
                                                                    errorMessage += "Nationality " + dr[k].ToString() + " not found\n";
                                                                    arrayerror.Add("Nationality;" + i);
                                                                    counterror += 1;
                                                                
                                                                }

                                                                
                                                            }
                                                            else
                                                            {
                                                                dtPass.Rows[i]["Nationality"] = dr[k].ToString();
                                                                dtPass.Rows[i]["CountryName"] = dr[k].ToString();
                                                                errorMessage += msgList.Err200065;
                                                                arrayerror.Add("Nationality;" + i);
                                                                counterror += 1;
                                                            }
                                                            break;

                                                        case "IssuingCountry":
                                                            if (dr[k].ToString() != "")
                                                            {
                                                                dtName = objGeneral.GetCountryCodeByName(dr[k].ToString());
                                                                if (dtName != null)
                                                                {
                                                                    dtPass.Rows[i]["IssuingCountry"] = dtName.Rows[0]["CountryCode"].ToString();
                                                                    dtPass.Rows[i]["IssuingCountryName"] = dr[k].ToString();
                                                                    MainField--;

                                                                    if (dtInfant != null && dtInfant.Rows.Count > 0)
                                                                    {
                                                                        foreach (DataRow dI in dtInfant.Rows)
                                                                        {
                                                                            if (dI["RecordLocator"].ToString() == dtPass.Rows[i]["PNR"].ToString() && dI["PassengerID"].ToString() == dtPass.Rows[i]["PassengerID"].ToString())
                                                                            {
                                                                                dI["IssuingCountry"] = dtName.Rows[0]["CountryCode"].ToString();
                                                                                dI["IssuingCountryName"] = dr[k].ToString();
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    dtPass.Rows[i]["IssuingCountry"] = dr[k].ToString();
                                                                    dtPass.Rows[i]["IssuingCountryName"] = dr[k].ToString();
                                                                    errorMessage += "IssuingCountry " + dr[k].ToString() + " not found\n";
                                                                    arrayerror.Add("IssuingCountry;" + i);
                                                                    counterror += 1;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                dtPass.Rows[i]["IssuingCountry"] = dr[k].ToString();
                                                                dtPass.Rows[i]["IssuingCountryName"] = dr[k].ToString();
                                                                errorMessage += msgList.Err200066;
                                                                arrayerror.Add("IssuingCountry;" + i);
                                                                counterror += 1;
                                                            }
                                                            break;

                                                        case "DOB":
                                                            if (dr[k].ToString() != "")
                                                            {
                                                                if (DateTime.TryParse(dr[k].ToString(), out DOB))
                                                                {
                                                                    DateTime stddate = bookHDRInfo.STDDate;
                                                                    DateTime maxaddult = stddate.AddYears(-2);
                                                                    DateTime maxchild = stddate.AddDays(-9);
                                                                    if (title.ToString().ToLower().Trim() == "chd" || title.ToString().ToLower().Trim() == "mr" || title.ToString().ToLower().Trim() == "ms" || title.ToString().ToLower().Trim() == "mrs")
                                                                    {
                                                                        if (title.ToString().ToLower().Trim() != "chd" && DateTime.Parse(dr[k].ToString()) > dtNow.AddYears(-12))
                                                                        {
                                                                            dtPass.Rows[i]["DOB"] = dr[k].ToString();
                                                                            errorMessage += msgList.Err200072;
                                                                            arrayerror.Add("DOB;" + i);
                                                                            counterror += 1;
                                                                        }
                                                                        else if (title.ToString().ToLower().Trim() == "chd" && DateTime.Parse(dr[k].ToString()) < dtNow.AddYears(-12))
                                                                        {
                                                                            dtPass.Rows[i]["DOB"] = dr[k].ToString();
                                                                            errorMessage += msgList.Err200073;
                                                                            arrayerror.Add("DOB;" + i);
                                                                            counterror += 1;
                                                                        }
                                                                        else if (DateTime.Parse(dr[k].ToString()) >= DateTime.Parse("1900-01-01"))
                                                                        {
                                                                            dtPass.Rows[i]["DOB"] = dr[k].ToString();
                                                                            MainField--;
                                                                        }
                                                                        else
                                                                        {
                                                                            dtPass.Rows[i]["DOB"] = dr[k].ToString();
                                                                            errorMessage += "Invalid DOB, DOB should be between 1 Jan 1900 and " + String.Format("{0:d MMM yyyy}", maxaddult) + "\n";
                                                                            arrayerror.Add("DOB;" + i);
                                                                            counterror += 1;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    dtPass.Rows[i]["DOB"] = DBNull.Value;
                                                                    errorMessage += msgList.Err200067;
                                                                    arrayerror.Add("DOB;" + i);
                                                                    counterror += 1;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                errorMessage += msgList.Err200067;
                                                                arrayerror.Add("DOB;" + i);
                                                                counterror += 1;
                                                            }
                                                            break;

                                                        case "ExpiryDate":
                                                            if (IsInternationalFlight)
                                                            {
                                                                if (dr[k].ToString() != "")
                                                                {
                                                                    if (DateTime.TryParse(dr[k].ToString(), out ExpiryDate))
                                                                    {
                                                                        if (Convert.ToDateTime(dr[k].ToString()) < bookHDRInfo.STDDate)
                                                                        {
                                                                            dtPass.Rows[i]["ExpiryDate"] = dr[k].ToString();
                                                                            errorMessage += msgList.Err200074;
                                                                            arrayerror.Add("ExpiryDate;" + i);
                                                                            counterror += 1;
                                                                        }
                                                                        else
                                                                        {
                                                                            dtPass.Rows[i]["ExpiryDate"] = dr[k].ToString();
                                                                            MainField--;
                                                                        }

                                                                    }
                                                                    else
                                                                    {
                                                                        errorMessage += msgList.Err200068;
                                                                        arrayerror.Add("ExpiryDate;" + i);
                                                                        counterror += 1;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    errorMessage += msgList.Err200068;
                                                                    arrayerror.Add("ExpiryDate;" + i);
                                                                    counterror += 1;

                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (dr[k] != null && dr[k].ToString() != "")
                                                                {
                                                                    if (DateTime.TryParse(dr[k].ToString(), out ExpiryDate))
                                                                    {
                                                                        dtPass.Rows[i]["ExpiryDate"] = dr[k].ToString();
                                                                        MainField--;
                                                                    }
                                                                    else
                                                                    {
                                                                        MainField--;
                                                                        continue;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    MainField--;
                                                                    continue;
                                                                }
                                                            }

                                                            break;

                                                        case "FirstName":
                                                            //add by romy for infant parent
                                                            if (dtInfant != null)
                                                                foreach (DataRow dI in dtInfant.Rows)
                                                                {
                                                                    if (dI["RecordLocator"].ToString() == dtPass.Rows[i]["PNR"].ToString() && dI["PassengerID"].ToString() == dtPass.Rows[i]["PassengerID"].ToString())
                                                                    {
                                                                        dI["ParentFirstName"] = dr[k].ToString();
                                                                    }
                                                                }
                                                            dtPass.Rows[i]["FirstName"] = dr[k].ToString();

                                                            getFirstNamePass = dr[k].ToString();

                                                            MainField--;
                                                            break;

                                                        case "LastName":
                                                            //add by romy for infant parent
                                                            if (dtInfant != null)
                                                                foreach (DataRow dI in dtInfant.Rows)
                                                                {
                                                                    if (dI["RecordLocator"].ToString() == dtPass.Rows[i]["PNR"].ToString() && dI["PassengerID"].ToString() == dtPass.Rows[i]["PassengerID"].ToString())
                                                                    {
                                                                        dI["ParentLastName"] = dr[k].ToString();
                                                                    }
                                                                }
                                                            dtPass.Rows[i]["LastName"] = dr[k].ToString();

                                                            getLastNamePass = dr[k].ToString();

                                                            MainField--;
                                                            break;

                                                        case "PassportNo":
                                                            if (IsInternationalFlight)
                                                            {
                                                                if (dr[k].ToString().Trim() == "")
                                                                {
                                                                    dtPass.Rows[i]["PassportNo"] = dr[k].ToString();
                                                                    errorMessage += msgList.Err200069;
                                                                    arrayerror.Add("PassportNo;" + i);
                                                                    counterror += 1;
                                                                }
                                                                else
                                                                {
                                                                    dtPass.Rows[i]["PassportNo"] = dr[k].ToString();
                                                                    MainField--;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (dr[k] != null && dr[k].ToString() != "" && dr[k].ToString() != "TBA")
                                                                {
                                                                    dtPass.Rows[i]["PassportNo"] = dr[k].ToString();
                                                                    MainField--;
                                                                }
                                                                else
                                                                {
                                                                    MainField--;
                                                                    continue;
                                                                }
                                                            }
                                                            break;
                                                    }
                                                }
                                                else
                                                {
                                                    switch (dtl)
                                                    {
                                                        case "Title":
                                                            if (dtPassenger != null && dtPassenger.Rows.Count > i && dtPassenger.Rows[i]["Title"].ToString().ToUpper().Trim() != "CHD" && (dr[k].ToString().ToLower().Trim() == "mr" || dr[k].ToString().ToLower().Trim() == "ms" || dr[k].ToString().ToLower().Trim() == "mrs"))
                                                            {
                                                                dtPass.Rows[i]["Title"] = char.ToUpper(dr[k].ToString()[0]) + dr[k].ToString().Substring(1);
                                                                title = dr[k].ToString();
                                                                MainField--;
                                                            }
                                                            else if (dtPassenger != null && dtPassenger.Rows.Count > i && dtPassenger.Rows[i]["Title"].ToString().ToUpper().Trim() == "CHD" && (dr[k].ToString().ToLower().Trim() == "chd"))
                                                            {
                                                                dtPass.Rows[i]["Title"] = char.ToUpper(dr[k].ToString()[0]) + dr[k].ToString().Substring(1);
                                                                title = dr[k].ToString();
                                                                MainField--;
                                                            }
                                                            else
                                                            {
                                                                //errorMessage += "|Passenger" + i + " Title|";
                                                                title = "";
                                                                dtPass.Rows[i]["Title"] = char.ToUpper(dr[k].ToString()[0]) + dr[k].ToString().Substring(1);
                                                                if (dtPassenger != null && dtPassenger.Rows.Count > i && dtPassenger.Rows[i]["Title"].ToString().ToUpper().Trim() == "CHD" && (dr[k].ToString().ToLower().Trim() == "mr" || dr[k].ToString().ToLower().Trim() == "ms" || dr[k].ToString().ToLower().Trim() == "mrs"))
                                                                {
                                                                    errorMessage += msgList.Err200070;
                                                                }
                                                                else if (dtPassenger != null && dtPassenger.Rows.Count > i && dtPassenger.Rows[i]["Title"].ToString().ToUpper().Trim() != "CHD" && (dr[k].ToString().ToLower().Trim() == "chd"))
                                                                {
                                                                    errorMessage += msgList.Err200071;
                                                                }
                                                                else
                                                                {
                                                                    errorMessage += msgList.Err200063; ;
                                                                }
                                                                arrayerror.Add("Title;" + i);
                                                                counterror += 1;
                                                            }
                                                            break;

                                                        case "Gender":
                                                            if ((dr[k].ToString().ToLower().Trim() == "male" || dr[k].ToString().ToLower().Trim() == "female"))
                                                            {
                                                                if (dtPassenger != null && dtPassenger.Rows.Count > i && dtPassenger.Rows[i]["Title"].ToString().ToUpper().Trim() != "CHD" && dr[k].ToString().ToLower().Trim() == "male" && (title.ToString().ToLower().Trim() == "ms" || title.ToString().ToLower().Trim() == "mrs"))
                                                                {
                                                                    dtPass.Rows[i]["Gender"] = "Female";
                                                                    MainField--;
                                                                    //errorMessage += "If Title is Ms, Gender must be Female\n";
                                                                }
                                                                else if (dtPassenger != null && dtPassenger.Rows.Count > i && dtPassenger.Rows[i]["Title"].ToString().ToUpper().Trim() != "CHD" && dr[k].ToString().ToLower().Trim() == "female" && title.ToString().ToLower().Trim() == "mr")
                                                                {
                                                                    dtPass.Rows[i]["Gender"] = "Male";
                                                                    MainField--;
                                                                    //errorMessage += "If Title is Mr, Gender must be Male\n";
                                                                }
                                                                else
                                                                {
                                                                    dtPass.Rows[i]["Gender"] = char.ToUpper(dr[k].ToString()[0]) + dr[k].ToString().Substring(1);
                                                                    MainField--;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //errorMessage += "|Passenger" + i + " Gender|";
                                                                dtPass.Rows[i]["Gender"] = char.ToUpper(dr[k].ToString()[0]) + dr[k].ToString().Substring(1);
                                                                errorMessage += msgList.Err200064;
                                                                arrayerror.Add("Gender;" + i);
                                                                counterror += 1;
                                                            }
                                                            break;

                                                        case "Nationality":
                                                            if (dr[k].ToString() != "")
                                                            {
                                                                dtName = objGeneral.GetCountryCodeByName(dr[k].ToString());
                                                                if (dtName != null)
                                                                {
                                                                    dtPass.Rows[i]["Nationality"] = dtName.Rows[0]["CountryCode"].ToString();
                                                                    dtPass.Rows[i]["CountryName"] = dr[k].ToString();
                                                                    MainField--;
                                                                }
                                                                else
                                                                {
                                                                    dtPass.Rows[i]["Nationality"] = dr[k].ToString();
                                                                    dtPass.Rows[i]["CountryName"] = dr[k].ToString();
                                                                    errorMessage += "Nationality " + dr[k].ToString() + " not found\n";
                                                                    arrayerror.Add("Nationality;" + i);
                                                                    counterror += 1;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                dtPass.Rows[i]["Nationality"] = dr[k].ToString();
                                                                dtPass.Rows[i]["CountryName"] = dr[k].ToString();
                                                                errorMessage += msgList.Err200065;
                                                                arrayerror.Add("Nationality;" + i);
                                                                counterror += 1;
                                                            }
                                                            break;

                                                        case "IssuingCountry":
                                                            if (dr[k].ToString() != "")
                                                            {
                                                                dtName = objGeneral.GetCountryCodeByName(dr[k].ToString());
                                                                if (dtName != null)
                                                                {
                                                                    dtPass.Rows[i]["IssuingCountry"] = dtName.Rows[0]["CountryCode"].ToString();
                                                                    dtPass.Rows[i]["IssuingCountryName"] = dr[k].ToString();
                                                                    MainField--;
                                                                }
                                                                else
                                                                {
                                                                    dtPass.Rows[i]["IssuingCountry"] = dr[k].ToString();
                                                                    dtPass.Rows[i]["IssuingCountryName"] = dr[k].ToString();
                                                                    errorMessage += "IssuingCountry " + dr[k].ToString() + " not found\n";
                                                                    arrayerror.Add("IssuingCountry;" + i);
                                                                    counterror += 1;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                dtPass.Rows[i]["IssuingCountry"] = dr[k].ToString();
                                                                dtPass.Rows[i]["IssuingCountryName"] = dr[k].ToString();
                                                                errorMessage += msgList.Err200066;
                                                                arrayerror.Add("IssuingCountry;" + i);
                                                                counterror += 1;
                                                            }
                                                            break;

                                                        case "DOB":
                                                            if (dr[k].ToString() != "")
                                                            {
                                                                if (DateTime.TryParse(dr[k].ToString(), out DOB))
                                                                {
                                                                    DateTime stddate = bookHDRInfo.STDDate;
                                                                    DateTime maxaddult = stddate.AddYears(-2);
                                                                    DateTime maxchild = stddate.AddDays(-9);
                                                                    if (title.ToString().ToLower().Trim() == "chd" || title.ToString().ToLower().Trim() == "mr" || title.ToString().ToLower().Trim() == "ms" || title.ToString().ToLower().Trim() == "mrs")
                                                                    {
                                                                        if (title.ToString().ToLower().Trim() != "chd" && DateTime.Parse(dr[k].ToString()) > dtNow.AddYears(-12))
                                                                        {
                                                                            dtPass.Rows[i]["DOB"] = dr[k].ToString();
                                                                            errorMessage += msgList.Err200072;
                                                                            arrayerror.Add("DOB;" + i);
                                                                            counterror += 1;
                                                                        }
                                                                        else if (title.ToString().ToLower().Trim() == "chd" && DateTime.Parse(dr[k].ToString()) < dtNow.AddYears(-12))
                                                                        {
                                                                            dtPass.Rows[i]["DOB"] = dr[k].ToString();
                                                                            errorMessage += msgList.Err200073;
                                                                            arrayerror.Add("DOB;" + i);
                                                                            counterror += 1;
                                                                        }
                                                                        else if (DateTime.Parse(dr[k].ToString()) >= DateTime.Parse("1900-01-01"))
                                                                        {
                                                                            dtPass.Rows[i]["DOB"] = dr[k].ToString();
                                                                            MainField--;
                                                                        }
                                                                        else
                                                                        {
                                                                            dtPass.Rows[i]["DOB"] = dr[k].ToString();
                                                                            errorMessage += "Invalid DOB, DOB should be between 1 Jan 1900 and " + String.Format("{0:d MMM yyyy}", maxaddult) + "\n";
                                                                            arrayerror.Add("DOB;" + i);
                                                                            counterror += 1;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    dtPass.Rows[i]["DOB"] = DBNull.Value;
                                                                    errorMessage += msgList.Err200067;
                                                                    arrayerror.Add("DOB;" + i);
                                                                    counterror += 1;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                errorMessage += msgList.Err200067;
                                                                arrayerror.Add("DOB;" + i);
                                                                counterror += 1;
                                                            }
                                                            break;
                                                        case "FirstName":
                                                            //add by romy for infant parent
                                                            if (dtInfant != null)
                                                                foreach (DataRow dI in dtInfant.Rows)
                                                                {
                                                                    if (dI["RecordLocator"].ToString() == dtPass.Rows[i]["PNR"].ToString() && dI["PassengerID"].ToString() == dtPass.Rows[i]["PassengerID"].ToString())
                                                                    {
                                                                        dI["ParentFirstName"] = dr[k].ToString();
                                                                    }
                                                                }
                                                            dtPass.Rows[i]["FirstName"] = dr[k].ToString();

                                                            getFirstNamePass = dr[k].ToString();

                                                            MainField--;
                                                            break;

                                                        case "LastName":
                                                            //add by romy for infant parent
                                                            if (dtInfant != null)
                                                                foreach (DataRow dI in dtInfant.Rows)
                                                                {
                                                                    if (dI["RecordLocator"].ToString() == dtPass.Rows[i]["PNR"].ToString() && dI["PassengerID"].ToString() == dtPass.Rows[i]["PassengerID"].ToString())
                                                                    {
                                                                        dI["ParentLastName"] = dr[k].ToString();
                                                                    }
                                                                }
                                                            dtPass.Rows[i]["LastName"] = dr[k].ToString();

                                                            getLastNamePass = dr[k].ToString();

                                                            MainField--;
                                                            break;
                                                    }
                                                }
                                            }
                                            k++;
                                        }
                                    }

                                    //verify edited row with full mandatory fields
                                    if (MainField > 0)
                                    {
                                        //errorMessage += "Please review data row number (" + (i + 1) + ")\n";
                                        //errorMessage = "Data not match on row " + (i + 1) + ", " + errorMessage;    //20170526 - Sienny
                                        dtPass.Rows[i]["ErrorMsg"] = errorMessage;

                                        Session["error"] = true;
                                        Session["arrayerror"] = arrayerror;
                                        Session["counterror"] = counterror;
                                        //MainField = 0;
                                        //break;
                                    }

                                    if (errorMessage != "")
                                        //break;
                                        //else
                                        log.Warning(this, errorMessage);

                                    loadPopup();

                                    i++;
                                }
                            }
                            else
                            {
                                errorMessage = msgList.Err200061;
                                lblMsg.Text = errorMessage;
                            }
                        }
                    }
                    //ammended by Ellis 20170306, removing dropdown list file type
                    //else if (fileType.ToLower() == "csv")
                    //{
                    //    System.IO.StreamReader rdr = new System.IO.StreamReader(filePath.Trim()); //Stream reader reads a file. File path and name is supplied from where to read the file. 
                    //    string inputLine = "";
                    //    DataTable dt = new DataTable();
                    //    /*
                    //    dt.Columns.Add("Title");
                    //    dt.Columns.Add("Gender");
                    //    dt.Columns.Add("Nationality");
                    //    dt.Columns.Add("countryName");
                    //    dt.Columns.Add("IssuingCountry");
                    //    dt.Columns.Add("DOB");
                    //    dt.Columns.Add("ExpiryDate");
                    //    dt.Columns.Add("FirstName");
                    //    dt.Columns.Add("LastName");
                    //    dt.Columns.Add("PassportNo");
                    //    */
                    //    dt.Columns.Add("Nationality");
                    //    dt.Columns.Add("IssuingCountry");
                    //    dt.Columns.Add("Title");
                    //    dt.Columns.Add("Gender");
                    //    dt.Columns.Add("FirstName");
                    //    dt.Columns.Add("LastName");
                    //    dt.Columns.Add("DOB");
                    //    dt.Columns.Add("PassportNo");
                    //    dt.Columns.Add("ExpiryDate");

                    //    errorMessage = string.Empty;
                    //    DateTime dateValue;
                    //    DataRow drRow;
                    //    int i = 0;
                    //    DataTable dtName = new DataTable();
                    //    string[] arrColumn = new string[1];
                    //    while ((inputLine = rdr.ReadLine()) != null) //Read while the line is not null
                    //    {
                    //        if (i - 1 == totalPassengers)
                    //            break;
                    //        assignPos();
                    //        string[] arr;
                    //        arr = inputLine.Split(',');

                    //        drRow = dt.NewRow();
                    //        if (i == 0)
                    //            arrColumn = inputLine.Split(',');
                    //        else
                    //        {
                    //            DateTime DOB = new DateTime();
                    //            DateTime ExpiryDate = new DateTime();
                    //            int MainField = 9;
                    //            //added by ketee, verify if changecount = 1 then cannot modify
                    //            if ((string)dtPass.Rows[i - 1]["FirstName"] != "" || Convert.ToInt32(dtPass.Rows[i - 1]["ChangeCount"].ToString()) >= Convert.ToInt32(dtPass.Rows[i - 1]["MaxChange"].ToString()))
                    //            {
                    //                MainField = 0;
                    //            }
                    //            else
                    //            {
                    //                string title = "";
                    //                for (int j = 0; j < arr.Count(); j++)
                    //                {
                    //                    if (arr[j].ToString() == "")
                    //                    {
                    //                        MainField--;
                    //                        continue;
                    //                    }

                    //                    string dtl = arrColumn[j].Trim();
                    //                    dtl = dtl.Replace(" ", "");
                    //                    switch (dtl)
                    //                    {
                    //                        case "Title":
                    //                            if (arr[j].ToString().ToLower() == "mr" || arr[j].ToString().ToLower() == "ms")
                    //                            {
                    //                                dtPass.Rows[i - 1]["Title"] = char.ToUpper(arr[j].ToString()[0]) + arr[j].ToString().Substring(1);
                    //                                title = arr[j].ToString();
                    //                                MainField--;
                    //                            }
                    //                            else
                    //                            {
                    //                                //errorMessage += "|Passenger" + i + " Title|";
                    //                                errorMessage += "Title must be Mr/Ms\n";
                    //                            }
                    //                            break;
                    //                        case "Gender":
                    //                            if (arr[j].ToString().ToLower() == "male" || arr[j].ToString().ToLower() == "female")
                    //                            {
                    //                                if (arr[j].ToString().ToLower() == "male" && title.ToString().ToLower() == "ms")
                    //                                {
                    //                                    errorMessage += "If Title is Ms, Gender must be Female\n";
                    //                                }
                    //                                else if (arr[j].ToString().ToLower() == "female" && title.ToString().ToLower() == "mr")
                    //                                {
                    //                                    errorMessage += "If Title is Mr, Gender must be Male\n";
                    //                                }
                    //                                else
                    //                                {
                    //                                    dtPass.Rows[i - 1]["Gender"] = char.ToUpper(arr[j].ToString()[0]) + arr[j].ToString().Substring(1);
                    //                                    MainField--;
                    //                                }
                    //                            }
                    //                            else
                    //                            {
                    //                                //errorMessage += "|Passenger" + i + " Gender|";
                    //                                errorMessage += msgList.Err200064;
                    //                            }
                    //                            break;
                    //                        case "Nationality":
                    //                            dtName = objGeneral.GetCountryCodeByName(arr[j].ToString());
                    //                            if (dtName != null)
                    //                            {
                    //                                dtPass.Rows[i - 1]["Nationality"] = dtName.Rows[0]["CountryCode"].ToString();
                    //                                dtPass.Rows[i - 1]["countryName"] = arr[j].ToString();
                    //                                MainField--;
                    //                            }
                    //                            else
                    //                            {
                    //                                //errorMessage += "|Passenger" + i + " Country|";
                    //                                errorMessage += "Nationality :" + arr[j].ToString() + " not found\n";
                    //                            }
                    //                            break;

                    //                        case "IssuingCountry":
                    //                            dtName = objGeneral.GetCountryCodeByName(arr[j].ToString());
                    //                            if (dtName != null)
                    //                            {
                    //                                dtPass.Rows[i - 1]["IssuingCountry"] = dtName.Rows[0]["CountryCode"].ToString();
                    //                                dtPass.Rows[i - 1]["IssuingCountryName"] = arr[j].ToString();
                    //                                MainField--;
                    //                            }
                    //                            else
                    //                            {
                    //                                //errorMessage += "|Passenger" + i + " IssuingCountry|";
                    //                                errorMessage += "IssuingCountry :" + arr[j].ToString() + " not found\n";
                    //                            }

                    //                            break;
                    //                        case "DOB":
                    //                            if (DateTime.TryParse(arr[j].ToString(), out DOB))
                    //                            {
                    //                                DateTime stddate = bookHDRInfo.STDDate;
                    //                                DateTime maxaddult = stddate.AddYears(-2);
                    //                                DateTime maxchild = stddate.AddDays(-9);
                    //                                if (title.ToString().ToLower() == "mr" || title.ToString().ToLower() == "ms")
                    //                                {
                    //                                    if (DateTime.Parse(arr[j].ToString()) >= DateTime.Parse("1900-01-01"))
                    //                                    {
                    //                                        dtPass.Rows[i - 1]["DOB"] = arr[j].ToString();
                    //                                        MainField--;
                    //                                    }
                    //                                    else
                    //                                    {
                    //                                        errorMessage += "Invalid DOB, DOB should be between 1 Jan 1900 and " + String.Format("{0:d MMM yyyy}", maxaddult) + "\n";
                    //                                    }
                    //                                }
                    //                                //else
                    //                                //{
                    //                                //    if (DateTime.Parse(arr[j].ToString()) >= maxaddult && DateTime.Parse(arr[j].ToString()) <= maxchild)
                    //                                //    {
                    //                                //        dtPass.Rows[i - 1]["DOB"] = arr[j].ToString();
                    //                                //        MainField--;
                    //                                //    }
                    //                                //    else
                    //                                //    {
                    //                                //        errorMessage += "Invalid DOB, for kid, DOB should be between " + String.Format("{0:d MMM yyyy}", maxaddult) + " and " + String.Format("{0:d MMM yyyy}", maxchild) + "\n";
                    //                                //    }
                    //                                //}
                    //                            }
                    //                            else
                    //                            {
                    //                                //errorMessage += "|Passenger" + i + " DOB|";
                    //                                errorMessage += msgList.Err200067;
                    //                            }
                    //                            break;
                    //                        case "ExpiryDate":
                    //                            //if (DateTime.TryParse(arr[j].ToString(), new CultureInfo("en-GB"), DateTimeStyles.AssumeLocal, out dateValue))
                    //                            if (DateTime.TryParse(arr[j].ToString(), out ExpiryDate))
                    //                            {
                    //                                dtPass.Rows[i - 1]["ExpiryDate"] = arr[j].ToString();
                    //                                MainField--;
                    //                            }
                    //                            else
                    //                            {
                    //                                // errorMessage += "|Passenger" + i + " ExpiryDate|";
                    //                                errorMessage += msgList.Err200068;
                    //                            }

                    //                            //if (ExpiryDate <= DOB || ExpiryDate <= DateTime.Today)
                    //                            //{
                    //                            //    errorMessage += "Invalid ExpiryDate\n";
                    //                            //    break;
                    //                            //}

                    //                            break;
                    //                        case "FirstName":
                    //                            dtPass.Rows[i - 1]["FirstName"] = arr[j].ToString();
                    //                            MainField--;
                    //                            break;
                    //                        case "LastName":
                    //                            dtPass.Rows[i - 1]["LastName"] = arr[j].ToString();
                    //                            MainField--;
                    //                            break;
                    //                        case "PassportNo":
                    //                            dtPass.Rows[i - 1]["PassportNo"] = arr[j].ToString();
                    //                            MainField--;
                    //                            break;
                    //                    }
                    //                }
                    //            }


                    //            //verify edited row with full mandatory fields
                    //            if (MainField > 0)
                    //            {
                    //                errorMessage += "Please review data row number (" + i + ")\n";
                    //                break;
                    //            }

                    //            if (errorMessage != string.Empty)
                    //                break;

                    //            loadPopup();
                    //        }
                    //        i++;
                    //    }
                    //}
                    else
                    {
                        errorMessage = msgList.Err200062;
                        lblMsg.Text = errorMessage;
                        return "";
                    }
                }
                else
                {
                    errorMessage = msgList.Err200062;
                    lblMsg.Text = errorMessage;
                    return "";
                }

                if (errorMessage != string.Empty)
                {
                    string TID = Request.QueryString["TransID"];

                    //change to new add-On table, Tyas
                    //dtPass = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTable(TID);
                    //dtPass = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTableNewManage(TID);
                    //Session["dtGridPass"] = dtPass;

                    //log.Warning(this, errorMessage);
                    //lblMsg.Text = errorMessage;
                    //lblMsg.Text = errorMessage + "contain invalid data";
                    return "";
                }

                if (dtPass != null && dtPass.Rows.Count > 0 && dtPass.Rows.Count <= totalPassengers)
                {
                    Session["dtGridPass"] = dtPass;
                    Session["dtInfant"] = dtInfant;//add by romy for infant parent
                    //LoadGridViewCallBack();
                }
                else
                {
                    lblMsg.Text = msgList.Err200075;
                    lblMsg.Visible = true;
                }

                //if (errorMessage != "")
                //    lblMsg.Text = errorMessage + " contain invalid data";
                return "";
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                lblMsg.Text = ex.Message.ToString();
                return "";
            }
        }

        protected void btnDl_Click(object sender, EventArgs e)
        {
            MessageList msgList = new MessageList();
            try
            {
                //Response.ContentType = "xls";
                //Response.AppendHeader("Content-Disposition", "attachment; filename=Sample.xls");
                //Response.TransmitFile(Server.MapPath("../public/Sample.xls"));
                //Response.End();

                DataTable dt = new DataTable();
                DataTable dtNew = new DataTable();

                if (Session["dtGridPass"] != null)
                {
                    dt = (DataTable)Session["dtGridPass"];
                    dtNew = dt.Clone();

                    foreach (DataColumn dc in dtNew.Columns)
                    {
                        if (dc.DataType == typeof(DateTime))
                        {
                            dc.DataType = typeof(string);
                        }
                    }

                    foreach (DataRow dr in dt.Rows)
                    {
                        dtNew.ImportRow(dr);
                    }

                    foreach (DataRow dr in dtNew.Rows)
                    {
                        DateTime dateOut;

                        if (DateTime.TryParse(dr["DOB"].ToString(), out dateOut))
                            //dr["DOB"] = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(dr["DOB"])).ToString();
                            dr["DOB"] = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dr["DOB"])).ToString();
                        if (DateTime.TryParse(dr["ExpiryDate"].ToString(), out dateOut))
                            //dr["ExpiryDate"] = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(dr["ExpiryDate"])).ToString();
                            dr["ExpiryDate"] = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dr["ExpiryDate"])).ToString();
                    }



                    gvExport.DataSource = dtNew;
                    gvExport.DataBind();

                    if (!IsInternationalFlight)
                    {
                        gvExport.Columns["PassportNo"].Visible = false;
                        gvExport.Columns["ExpiryDate"].Visible = false;
                    }

                    exporter.GridViewID = "gvExport";
                    exporter.FileName = "PassengerList";

                    //ammended by Ellis 20170306, removing dropdown list file type
                    //if (cmbType.SelectedItem.Value.ToString() == "xls")
                    //{
                    exporter.WriteXlsToResponse();
                    //}
                    //if (cmbType.SelectedItem.Value.ToString() == "csv")
                    //{
                    //    exporter.WriteCsvToResponse();
                    //}
                }
                else
                {
                    lblMsg.Text = msgList.Err200076;
                }

            }
            catch (Exception ex)
            {
                //SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
        }

        protected void btnDl2_Click(object sender, EventArgs e)
        {
            MessageList msgList = new MessageList();
            try
            {
                //Response.ContentType = "xls";
                //Response.AppendHeader("Content-Disposition", "attachment; filename=Sample.xls");
                //Response.TransmitFile(Server.MapPath("../public/Sample.xls"));
                //Response.End();

                DataTable dt = new DataTable();
                DataTable dtNew = new DataTable();

                if (Session["dtGridPass"] != null)
                {
                    dt = (DataTable)Session["dtGridPass"];
                    dtNew = dt.Clone();

                    foreach (DataColumn dc in dtNew.Columns)
                    {
                        if (dc.DataType == typeof(DateTime))
                        {
                            dc.DataType = typeof(string);
                        }
                    }

                    foreach (DataRow dr in dt.Rows)
                    {
                        dtNew.ImportRow(dr);
                    }

                    foreach (DataRow dr in dtNew.Rows)
                    {
                        DateTime dateOut;

                        if (DateTime.TryParse(dr["DOB"].ToString(), out dateOut))
                            dr["DOB"] = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(dr["DOB"])).ToString();
                        if (DateTime.TryParse(dr["ExpiryDate"].ToString(), out dateOut))
                            dr["ExpiryDate"] = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(dr["ExpiryDate"])).ToString();
                    }

                    gvExport.Columns["ErrorMsg"].Visible = true;


                    gvExport.DataSource = dtNew;
                    gvExport.DataBind();

                    exporter.GridViewID = "gvExport";
                    exporter.FileName = "Error_PassengerList";

                    //ammended by Ellis 20170306, removing dropdown list file type
                    //if (cmbType.SelectedItem.Value.ToString() == "xls")
                    //{
                    exporter.WriteXlsToResponse();
                    //}
                    //if (cmbType.SelectedItem.Value.ToString() == "csv")
                    //{
                    //    exporter.WriteCsvToResponse();
                    //}
                }
                else
                {
                    lblMsg.Text = msgList.Err200076;
                }

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
        }



        #region batchediting
        protected void ActionPanel_PerformCallback(object sender, CallbackEventArgsBase e)
        {
            int TotalPax = 0;
            int SeqNo = 1;
            ArrayList SellSessionID;
            //DataTable dtPassOld;
            try
            {
                if (Session["dtNewGridPass"] != null)
                {
                    dtPass = (DataTable)Session["dtNewGridPass"];

                    if (Session["dtGridPass"] != null)
                    {
                        dtPassOld = (DataTable)Session["dtGridPass"];
                        //foreach (DataRow dr in dtPassOld.Rows)
                        //{
                        //    //if (Convert.ToInt32(dr["ChangeCount"]) >= 1)
                        //    //SeqNo += 1;
                        //}
                    }

                    lstbookDTLInfo = objBooking.GetAllBK_TRANSDTLFilterAll(TransID, 0);
                    //objBooking.GetAllBK_TRANSDTLFilter(TransID, 0);

                    //CEK RETURN FLIGHT
                    Boolean returnFlight = false;
                    returnFlight = objBooking.IsReturn(TransID, 0);
                    decimal Limit = 0;
                    if (NameChangeMax == 1) Limit = NameChangeLimit1;
                    else if (NameChangeMax == 2) Limit = NameChangeLimit2;

                    Dictionary<string, decimal> BalanceAmount = new Dictionary<string, decimal>();
                    ArrayList PNR = new ArrayList();
                    string LatestPNR = "";
                    foreach (BookingTransactionDetail bookDTL in lstbookDTLInfo)
                    {
                        if (!PNR.Contains(bookDTL.RecordLocator))
                        {
                            TotalPax += bookDTL.TotalPax;
                            PNR.Add(bookDTL.RecordLocator);
                            BalanceAmount.Add(bookDTL.RecordLocator, 0);
                        }

                        CurrencyCode = bookDTL.Currency;
                    }

                    //edit by ketee, replace to check long haul and short haul from faresellkey to optgroup
                    string GroupName = objGeneral.getOPTGroupByCarrierCode(lstbookDTLInfo[0].CarrierCode);

                    //20170613 - Sienny
                    if (Session["CountryCode"].ToString() != null)
                        AgentCountryCode = Session["CountryCode"].ToString();
                    AgentCurrencyCode = AgentSet.Currency;
                    MaxFreeTime1 = 0; MaxFreeTime2 = 0; MaxPax1 = 0; MaxPax2 = 0; IsFree1 = 0; IsFree2 = 0; MaxChangeTime = 0; ChangeLimit = 0;
                    DataTable dtFeeSetting = new DataTable();
                    dtFeeSetting = objBooking.GetFeeSettingByGroupCountryCurrencyCode(GroupName, AgentCountryCode, CurrencyCode);
                    if (dtFeeSetting != null && dtFeeSetting.Rows.Count > 0)
                    {
                        EffectiveDate = Convert.ToDateTime(dtFeeSetting.Rows[0]["EffectiveDate"].ToString());
                        IsFree1 = Convert.ToInt32(dtFeeSetting.Rows[0]["IsFree1"].ToString());
                        IsFree2 = Convert.ToInt32(dtFeeSetting.Rows[0]["IsFree2"].ToString());
                        IsFree3 = Convert.ToInt32(dtFeeSetting.Rows[0]["IsFree3"].ToString());
                        MaxChangeTime = Convert.ToInt32(dtFeeSetting.Rows[0]["MaxChangeTime"].ToString());
                        ChangeLimit = Convert.ToInt32(dtFeeSetting.Rows[0]["ChangeLimit"].ToString());
                        MaxPax1 = Convert.ToDecimal(dtFeeSetting.Rows[0]["MaxPax1"].ToString());
                        MaxPax2 = Convert.ToDecimal(dtFeeSetting.Rows[0]["MaxPax2"].ToString());
                        MaxPax3 = Convert.ToDecimal(dtFeeSetting.Rows[0]["MaxPax3"].ToString());
                        if (IsFree1 == 1)
                            MaxFreeTime1 = Convert.ToInt32(dtFeeSetting.Rows[0]["MaxFreeTime1"].ToString());
                        else
                            MaxFreeTime1 = 0;
                    }

                    //to retrieve departure date
                    DateTime DepDate = DateTime.Now;
                    if (lstbookDTLInfo != null && lstbookDTLInfo.Count > 0) DepDate = lstbookDTLInfo[0].DepatureDate;

                    //foreach (DataRow drRow in dtPass.Rows)
                    //{
                    if ((int)Session["CurStatus"] == 3)
                    {
                        if (HttpContext.Current.Session["objListBK_TRANSDTL_Infos"] != null)
                        {
                            List<BookingTransactionDetail> objListBK_TRANSDTL_Infos = (List<BookingTransactionDetail>)HttpContext.Current.Session["objListBK_TRANSDTL_Infos"];
                            foreach (BookingTransactionDetail bkDetail in objListBK_TRANSDTL_Infos)
                            {
                                objBooking.CancelSellRequest(bkDetail.Signature);
                            }
                            HttpContext.Current.Session["objListBK_TRANSDTL_Infos"] = null;
                            HttpContext.Current.Session["SellSessionID"] = null;
                        }

                        Session["BalanceNameChange"] = 0;
                        decimal TotalFee = 0;
                        //if (objGeneral.IsLongHaulFlightbySellKey(lstbookDTLInfo[0].FareSellKey, Request.PhysicalApplicationPath, lstbookDTLInfo[0].OverridedFareSellKey))
                        if (GroupName.ToLower().Trim() == "aax")
                        {
                            SeqNo = 1 + objBooking.GetChangeCount(TransID);
                            //MinTime = 0; MaxTime = 0; LimitTime = 0; ActiveMinTime = 0; ActiveMaxTime = 0;
                            //if (Session["MinTime"] != null) MinTime = decimal.Parse(Session["MinTime"].ToString());
                            //if (Session["MaxTime"] != null) MaxTime = decimal.Parse(Session["MaxTime"].ToString());
                            //if (Session["ActiveMinTime"] != null) ActiveMinTime = decimal.Parse(Session["ActiveMinTime"].ToString());
                            //if (Session["ActiveMaxTime"] != null) ActiveMaxTime = decimal.Parse(Session["ActiveMaxTime"].ToString());
                            //if (Session["LimitTime"] != null) LimitTime = decimal.Parse(Session["LimitTime"].ToString());
                            TimeDiff = decimal.Parse((DepDate - DateTime.Now).TotalHours.ToString());

                            foreach (DataRow drRow in dtPass.Rows)
                            {
                                PassData.ChangeFee = 0;
                                //drRow["ChangeFee2"] = PassData.ChangeFee2;
                                object sumObject;
                                sumObject = dtPass.Compute("Sum(ChangeFee)", "PNR = '" + drRow["PNR"].ToString() + "'");
                                object sumObject2;
                                sumObject2 = dtPass.Compute("Sum(ChangeFee2)", "PNR = '" + drRow["PNR"].ToString() + "'");

                                decimal totalsum = (decimal)sumObject + (decimal)sumObject2;
                                //begin AAX

                                if (DepDate >= EffectiveDate)
                                //if (ActiveMinTime == 1 && ActiveMaxTime == 1)
                                {

                                    if (IsFree1 == 1 && TimeDiff <= MaxFreeTime1 && Convert.ToInt32(drRow["ChangeCount"]) == 1 && Convert.ToInt32(drRow["PaxNo"]) == 1 && ChangeLimit == 1)
                                    {
                                        SellResponse response = objBooking.AddNameChangeFees("", drRow["Currency"].ToString(), Convert.ToInt32(drRow["PassengerID"]), drRow["PNR"].ToString());
                                        if (response.BookingUpdateResponseData.Success != null)
                                        {
                                            //if (drRow["ChangeFee"].ToString() == "0") //if (drRow["ChangeFee2"].ToString() == "0")
                                            //{
                                            PassData.ChangeFee = Convert.ToDecimal(response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue - TotalFee);
                                            PassData.ChangeDate = DateTime.Now;
                                            drRow["ChangeFee"] = PassData.ChangeFee;
                                            //}
                                            BalanceAmount[drRow["PNR"].ToString()] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;
                                            //Session["BalanceNameChange"] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;

                                            TotalFee += Convert.ToDecimal(drRow["ChangeFee"].ToString());
                                        }
                                        else
                                        {
                                        }
                                        SeqNo += 1;
                                    }
                                    else if (IsFree1 == 0 && Convert.ToInt32(drRow["ChangeCount"]) == 1 && Convert.ToInt32(drRow["PaxNo"]) == 1 && ChangeLimit == 1)
                                    {
                                        if (MaxPax1 != 0)
                                        {
                                            if (SeqNo > Convert.ToDecimal(TotalPax * (MaxPax1 / 100)))
                                            {
                                                SellResponse response = objBooking.AddNameChangeFees("", drRow["Currency"].ToString(), Convert.ToInt32(drRow["PassengerID"]), drRow["PNR"].ToString());
                                                if (response.BookingUpdateResponseData.Success != null)
                                                {
                                                    //if (drRow["ChangeFee"].ToString() == "0") //if (drRow["ChangeFee2"].ToString() == "0")
                                                    //{
                                                    PassData.ChangeFee = Convert.ToDecimal(response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue - TotalFee);
                                                    PassData.ChangeDate = DateTime.Now;
                                                    drRow["ChangeFee"] = PassData.ChangeFee;
                                                    //}
                                                    BalanceAmount[drRow["PNR"].ToString()] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;
                                                    //Session["BalanceNameChange"] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;

                                                    TotalFee += Convert.ToDecimal(drRow["ChangeFee"].ToString());
                                                }
                                                else
                                                {
                                                }
                                            }
                                        }
                                        else
                                        {
                                            SellResponse response = objBooking.AddNameChangeFees("", drRow["Currency"].ToString(), Convert.ToInt32(drRow["PassengerID"]), drRow["PNR"].ToString());
                                            if (response.BookingUpdateResponseData.Success != null)
                                            {
                                                //if (drRow["ChangeFee"].ToString() == "0") //if (drRow["ChangeFee2"].ToString() == "0")
                                                //{
                                                PassData.ChangeFee = Convert.ToDecimal(response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue - TotalFee);
                                                PassData.ChangeDate = DateTime.Now;
                                                drRow["ChangeFee"] = PassData.ChangeFee;
                                                //}
                                                BalanceAmount[drRow["PNR"].ToString()] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;
                                                //Session["BalanceNameChange"] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;

                                                TotalFee += Convert.ToDecimal(drRow["ChangeFee"].ToString());
                                            }
                                            else
                                            {
                                            }
                                        }
                                        SeqNo += 1;
                                    }
                                    else if (IsFree1 == 1 && TimeDiff <= MaxFreeTime1 && Convert.ToInt32(drRow["ChangeCount"]) > 1 && Convert.ToInt32(drRow["PaxNo"]) > 1 && ChangeLimit == 2)
                                    {
                                        SellResponse response = objBooking.AddNameChangeFees("", drRow["Currency"].ToString(), Convert.ToInt32(drRow["PassengerID"]), drRow["PNR"].ToString());
                                        if (response.BookingUpdateResponseData.Success != null)
                                        {
                                            //if (drRow["ChangeFee2"].ToString() == "0")
                                            //{
                                            //response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue - Convert.ToDecimal(Session["BalanceNameChange"].ToString());
                                            PassData.ChangeFee2 = Convert.ToDecimal(response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue) - TotalFee;
                                            PassData.ChangeDate2 = DateTime.Now;
                                            drRow["ChangeFee2"] = PassData.ChangeFee2;
                                            //}
                                            BalanceAmount[drRow["PNR"].ToString()] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;
                                            //Session["BalanceNameChange"] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;

                                            TotalFee += Convert.ToDecimal(drRow["ChangeFee"].ToString());
                                        }
                                        else
                                        {
                                        }
                                        SeqNo += 1;
                                    }
                                    else if (IsFree1 == 0 && Convert.ToInt32(drRow["ChangeCount"]) > 1 && Convert.ToInt32(drRow["PaxNo"]) > 1 && ChangeLimit == 2)
                                    {
                                        if (MaxPax2 != 0)
                                        {
                                            if (SeqNo > Convert.ToDecimal(TotalPax * (MaxPax2)))
                                            {
                                                SellResponse response = objBooking.AddNameChangeFees("", drRow["Currency"].ToString(), Convert.ToInt32(drRow["PassengerID"]), drRow["PNR"].ToString());
                                                if (response.BookingUpdateResponseData.Success != null)
                                                {
                                                    //if (drRow["ChangeFee2"].ToString() == "0")
                                                    //{
                                                    //response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue - Convert.ToDecimal(Session["BalanceNameChange"].ToString());
                                                    PassData.ChangeFee2 = Convert.ToDecimal(response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue) - TotalFee;
                                                    PassData.ChangeDate2 = DateTime.Now;
                                                    drRow["ChangeFee2"] = PassData.ChangeFee2;
                                                    //}
                                                    BalanceAmount[drRow["PNR"].ToString()] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;
                                                    //Session["BalanceNameChange"] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;

                                                    TotalFee += Convert.ToDecimal(drRow["ChangeFee"].ToString());
                                                }
                                                else
                                                {
                                                }
                                            }
                                        }
                                        else
                                        {
                                            SellResponse response = objBooking.AddNameChangeFees("", drRow["Currency"].ToString(), Convert.ToInt32(drRow["PassengerID"]), drRow["PNR"].ToString());
                                            if (response.BookingUpdateResponseData.Success != null)
                                            {
                                                //if (drRow["ChangeFee2"].ToString() == "0")
                                                //{
                                                //response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue - Convert.ToDecimal(Session["BalanceNameChange"].ToString());
                                                PassData.ChangeFee2 = Convert.ToDecimal(response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue) - TotalFee;
                                                PassData.ChangeDate2 = DateTime.Now;
                                                drRow["ChangeFee2"] = PassData.ChangeFee2;
                                                //}
                                                BalanceAmount[drRow["PNR"].ToString()] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;
                                                //Session["BalanceNameChange"] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;

                                                TotalFee += Convert.ToDecimal(drRow["ChangeFee"].ToString());
                                            }
                                            else
                                            {
                                            }
                                        }
                                        SeqNo += 1;
                                    }
                                }
                                else if (Convert.ToInt32(drRow["ChangeCount"]) == 1 && Convert.ToInt32(drRow["PaxNo"]) == 1 && ChangeLimit == 1)
                                //else if (Convert.ToInt32(drRow["ChangeCount"]) == 1 && Convert.ToInt32(drRow["PaxNo"]) == 1 && NameChangeMax >= 1)
                                //if (Convert.ToInt32(drRow["ChangeCount"]) == 2 && Convert.ToInt32(drRow["PaxNo"]) == 2)
                                {
                                    if (IsFree1 == 0 && MaxPax1 != 0)
                                    //if (NameChangeLimit1 != 0)
                                    {
                                        SeqNo = 1 + objBooking.GetChangeCount(TransID);

                                        if (SeqNo > Convert.ToDecimal(TotalPax * (MaxPax1 / 100)))
                                        //if (SeqNo > Convert.ToDecimal(TotalPax * (NameChangeLimit1)))
                                        {
                                            SellResponse response = objBooking.AddNameChangeFees("", drRow["Currency"].ToString(), Convert.ToInt32(drRow["PassengerID"]), drRow["PNR"].ToString());
                                            if (response.BookingUpdateResponseData.Success != null)
                                            {
                                                //if (drRow["ChangeFee"].ToString() == "0") //if (drRow["ChangeFee2"].ToString() == "0")
                                                //{
                                                PassData.ChangeFee = Convert.ToDecimal(response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue - TotalFee);
                                                PassData.ChangeDate = DateTime.Now;
                                                drRow["ChangeFee"] = PassData.ChangeFee;
                                                //}
                                                BalanceAmount[drRow["PNR"].ToString()] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;
                                                //Session["BalanceNameChange"] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;

                                                TotalFee += Convert.ToDecimal(drRow["ChangeFee"].ToString());
                                            }
                                            else
                                            {
                                            }
                                        }
                                    }
                                    else if (IsFree1 == 1 && TimeDiff <= MaxFreeTime1)
                                    {
                                        SellResponse response = objBooking.AddNameChangeFees("", drRow["Currency"].ToString(), Convert.ToInt32(drRow["PassengerID"]), drRow["PNR"].ToString());
                                        if (response.BookingUpdateResponseData.Success != null)
                                        {
                                            //if (drRow["ChangeFee"].ToString() == "0") //if (drRow["ChangeFee2"].ToString() == "0")
                                            //{
                                            PassData.ChangeFee = Convert.ToDecimal(response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue - TotalFee);
                                            PassData.ChangeDate = DateTime.Now;
                                            drRow["ChangeFee"] = PassData.ChangeFee;
                                            //}
                                            BalanceAmount[drRow["PNR"].ToString()] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;
                                            //Session["BalanceNameChange"] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;

                                            TotalFee += Convert.ToDecimal(drRow["ChangeFee"].ToString());
                                        }
                                        else
                                        {
                                        }
                                    }
                                    SeqNo += 1;
                                }
                                else if (Convert.ToInt32(drRow["ChangeCount"]) > 1 && Convert.ToInt32(drRow["PaxNo"]) > 1 && ChangeLimit == 2)
                                //else if (Convert.ToInt32(drRow["ChangeCount"]) > 1 && Convert.ToInt32(drRow["PaxNo"]) > 1 && NameChangeMax == 2)
                                //if (Convert.ToInt32(drRow["ChangeCount"]) == 2 && Convert.ToInt32(drRow["PaxNo"]) == 2)
                                {
                                    if (IsFree2 == 0 && MaxPax2 != 0)
                                    //if (NameChangeLimit2 != 0)
                                    {
                                        SeqNo = 1 + objBooking.GetChangeCount(TransID);

                                        if (SeqNo > Convert.ToDecimal(TotalPax * (MaxPax2 / 100)))
                                        //if (SeqNo > Convert.ToDecimal(TotalPax * (NameChangeLimit2)))
                                        {
                                            SellResponse response = objBooking.AddNameChangeFees("", drRow["Currency"].ToString(), Convert.ToInt32(drRow["PassengerID"]), drRow["PNR"].ToString());
                                            if (response.BookingUpdateResponseData.Success != null)
                                            {
                                                //if (drRow["ChangeFee2"].ToString() == "0")
                                                //{
                                                //response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue - Convert.ToDecimal(Session["BalanceNameChange"].ToString());
                                                PassData.ChangeFee2 = Convert.ToDecimal(response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue) - TotalFee;
                                                PassData.ChangeDate2 = DateTime.Now;
                                                drRow["ChangeFee2"] = PassData.ChangeFee2;
                                                //}
                                                BalanceAmount[drRow["PNR"].ToString()] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;
                                                //Session["BalanceNameChange"] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;

                                                TotalFee += Convert.ToDecimal(drRow["ChangeFee"].ToString());
                                            }
                                            else
                                            {
                                            }
                                        }
                                    }
                                    else if (IsFree2 == 1 && TimeDiff <= MaxFreeTime2)
                                    {
                                        SellResponse response = objBooking.AddNameChangeFees("", drRow["Currency"].ToString(), Convert.ToInt32(drRow["PassengerID"]), drRow["PNR"].ToString());
                                        if (response.BookingUpdateResponseData.Success != null)
                                        {
                                            //if (drRow["ChangeFee2"].ToString() == "0")
                                            //{
                                            //response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue - Convert.ToDecimal(Session["BalanceNameChange"].ToString());
                                            PassData.ChangeFee2 = Convert.ToDecimal(response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue) - TotalFee;
                                            PassData.ChangeDate2 = DateTime.Now;
                                            drRow["ChangeFee2"] = PassData.ChangeFee2;
                                            //}
                                            BalanceAmount[drRow["PNR"].ToString()] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;
                                            //Session["BalanceNameChange"] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;

                                            TotalFee += Convert.ToDecimal(drRow["ChangeFee"].ToString());
                                        }
                                        else
                                        {
                                        }
                                    }
                                    SeqNo += 1;
                                }
                                //end AAX
                            }

                        }
                        else
                        {
                            SeqNo = 1 + objBooking.GetChangeCount(TransID);
                            //MinTime = 0; MaxTime = 0; LimitTime = 0; ActiveMinTime = 0; ActiveMaxTime = 0;
                            //if (Session["MinTime"] != null) MinTime = decimal.Parse(Session["MinTime"].ToString());
                            //if (Session["MaxTime"] != null) MaxTime = decimal.Parse(Session["MaxTime"].ToString());
                            //if (Session["ActiveMinTime"] != null) ActiveMinTime = decimal.Parse(Session["ActiveMinTime"].ToString());
                            //if (Session["ActiveMaxTime"] != null) ActiveMaxTime = decimal.Parse(Session["ActiveMaxTime"].ToString());
                            //if (Session["LimitTime"] != null) LimitTime = decimal.Parse(Session["LimitTime"].ToString());
                            TimeDiff = decimal.Parse((DepDate - DateTime.Now).TotalHours.ToString());

                            foreach (DataRow drRow in dtPass.Rows)
                            {
                                //PassData.ChangeFee = 0;
                                //drRow["ChangeFee"] = PassData.ChangeFee;

                                object sumObject;
                                sumObject = dtPass.Compute("Sum(ChangeFee)", "PNR = '" + drRow["PNR"].ToString() + "'");
                                object sumObject2;
                                sumObject2 = dtPass.Compute("Sum(ChangeFee2)", "PNR = '" + drRow["PNR"].ToString() + "'");

                                //object sumObject1;
                                //sumObject1 = dtPassOld.Compute("Sum(ChangeFee)", "PNR = '" + drRow["PNR"].ToString() + "'");
                                //object sumObject21;
                                //sumObject21 = dtPassOld.Compute("Sum(ChangeFee2)", "PNR = '" + drRow["PNR"].ToString() + "'");

                                decimal totalsum = (decimal)sumObject + (decimal)sumObject2;
                                //begin AA

                                if (DepDate >= EffectiveDate)
                                //if (ActiveMinTime == 1 && ActiveMaxTime == 1)
                                {
                                    if (IsFree1 == 1 && TimeDiff <= MaxFreeTime1 && Convert.ToInt32(drRow["ChangeCount"]) == 1 && Convert.ToInt32(drRow["PaxNo"]) == 1 && ChangeLimit == 1)
                                    {
                                        SellResponse response = objBooking.AddNameChangeFees("", drRow["Currency"].ToString(), Convert.ToInt32(drRow["PassengerID"]), drRow["PNR"].ToString());
                                        if (response.BookingUpdateResponseData.Success != null)
                                        {
                                            //if (drRow["ChangeFee"].ToString() == "0") //if (drRow["ChangeFee2"].ToString() == "0")
                                            //{
                                            PassData.ChangeFee = Convert.ToDecimal(response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue - TotalFee);
                                            PassData.ChangeDate = DateTime.Now;
                                            drRow["ChangeFee"] = PassData.ChangeFee;
                                            //PassData.ChangeFee2 = Convert.ToDecimal(response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue - totalsum);
                                            //PassData.ChangeDate2 = DateTime.Now;
                                            //drRow["ChangeFee2"] = PassData.ChangeFee2;
                                            //}
                                            BalanceAmount[drRow["PNR"].ToString()] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;
                                            //Session["BalanceNameChange"] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;

                                            TotalFee += Convert.ToDecimal(drRow["ChangeFee"].ToString());
                                        }
                                        else
                                        {

                                        }
                                        SeqNo += 1;
                                    }
                                    else if (IsFree1 == 0 && Convert.ToInt32(drRow["ChangeCount"]) == 1 && Convert.ToInt32(drRow["PaxNo"]) == 1 && ChangeLimit == 1)
                                    //if (TimeDiff >= MinTime && TimeDiff <= MaxTime && Convert.ToInt32(drRow["ChangeCount"]) == 1 && Convert.ToInt32(drRow["PaxNo"]) == 1 && NameChangeMax >= 1)
                                    //if (Convert.ToInt32(drRow["ChangeCount"]) == 2 && Convert.ToInt32(drRow["PaxNo"]) == 2)
                                    {
                                        //remarked by diana
                                        //object sumPass;
                                        //sumPass = dtPass.Compute("Count(ChangeCount)", "ChangeCount = 2");

                                        if (MaxPax1 != 0)
                                        //if (NameChangeLimit1 != 0)
                                        {
                                            if (SeqNo > Convert.ToDecimal(TotalPax * (MaxPax1 / 100)))
                                            //if (SeqNo > Convert.ToDecimal(TotalPax * (NameChangeLimit1)))
                                            {
                                                SellResponse response = objBooking.AddNameChangeFees("", drRow["Currency"].ToString(), Convert.ToInt32(drRow["PassengerID"]), drRow["PNR"].ToString());
                                                if (response.BookingUpdateResponseData.Success != null)
                                                {
                                                    BalanceAmount[drRow["PNR"].ToString()] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;

                                                    //if (drRow["ChangeFee"].ToString() == "0") //if (drRow["ChangeFee2"].ToString() == "0")
                                                    //{
                                                    PassData.ChangeFee = Convert.ToDecimal(response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue - TotalFee);
                                                    PassData.ChangeDate = DateTime.Now;
                                                    drRow["ChangeFee"] = PassData.ChangeFee;
                                                    //}

                                                    //Session["BalanceNameChange"] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;

                                                    TotalFee += Convert.ToDecimal(drRow["ChangeFee"].ToString());
                                                }
                                                else
                                                {

                                                }
                                            }
                                        }
                                        else
                                        {
                                            SellResponse response = objBooking.AddNameChangeFees("", drRow["Currency"].ToString(), Convert.ToInt32(drRow["PassengerID"]), drRow["PNR"].ToString());
                                            if (response.BookingUpdateResponseData.Success != null)
                                            {
                                                //if (drRow["ChangeFee"].ToString() == "0") //if (drRow["ChangeFee2"].ToString() == "0")
                                                //{
                                                PassData.ChangeFee = Convert.ToDecimal(response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue - TotalFee);
                                                PassData.ChangeDate = DateTime.Now;
                                                drRow["ChangeFee"] = PassData.ChangeFee;
                                                //PassData.ChangeFee2 = Convert.ToDecimal(response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue - totalsum);
                                                //PassData.ChangeDate2 = DateTime.Now;
                                                //drRow["ChangeFee2"] = PassData.ChangeFee2;
                                                //}
                                                BalanceAmount[drRow["PNR"].ToString()] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;
                                                //Session["BalanceNameChange"] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;

                                                TotalFee += Convert.ToDecimal(drRow["ChangeFee"].ToString());
                                            }
                                            else
                                            {

                                            }
                                        }
                                        SeqNo += 1;
                                    }
                                    else if (IsFree1 == 1 && TimeDiff <= MaxFreeTime1 && Convert.ToInt32(drRow["ChangeCount"]) > 1 && Convert.ToInt32(drRow["PaxNo"]) > 1 && ChangeLimit == 2)
                                    {
                                        SellResponse response = objBooking.AddNameChangeFees("", drRow["Currency"].ToString(), Convert.ToInt32(drRow["PassengerID"]), drRow["PNR"].ToString());
                                        if (response.BookingUpdateResponseData.Success != null)
                                        {
                                            //if (drRow["ChangeFee2"].ToString() == "0")
                                            //{
                                            //response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue - Convert.ToDecimal(Session["BalanceNameChange"].ToString());
                                            PassData.ChangeFee2 = Convert.ToDecimal(response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue) - TotalFee;
                                            PassData.ChangeDate2 = DateTime.Now;
                                            drRow["ChangeFee2"] = PassData.ChangeFee2;
                                            //}
                                            BalanceAmount[drRow["PNR"].ToString()] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;
                                            //Session["BalanceNameChange"] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;

                                            TotalFee += Convert.ToDecimal(drRow["ChangeFee"].ToString());
                                        }
                                        else
                                        {

                                        }
                                        SeqNo += 1;
                                    }
                                    else if (IsFree1 == 0 && Convert.ToInt32(drRow["ChangeCount"]) > 1 && Convert.ToInt32(drRow["PaxNo"]) > 1 && ChangeLimit == 2)
                                    //else if (TimeDiff >= MinTime && TimeDiff <= MaxTime && Convert.ToInt32(drRow["ChangeCount"]) > 1 && Convert.ToInt32(drRow["PaxNo"]) > 1 && NameChangeMax == 2)
                                    //if (Convert.ToInt32(drRow["ChangeCount"]) == 2 && Convert.ToInt32(drRow["PaxNo"]) == 2)
                                    {
                                        //remarked by diana
                                        //object sumPass;
                                        //sumPass = dtPass.Compute("Count(ChangeCount)", "ChangeCount = 2");

                                        if (MaxPax2 != 0)
                                        //if (NameChangeLimit2 != 0)
                                        {
                                            if (SeqNo > Convert.ToDecimal(TotalPax * (MaxPax2 / 100)))
                                            //if (SeqNo > Convert.ToDecimal(TotalPax * (NameChangeLimit2)))
                                            {
                                                SellResponse response = objBooking.AddNameChangeFees("", drRow["Currency"].ToString(), Convert.ToInt32(drRow["PassengerID"]), drRow["PNR"].ToString());
                                                if (response.BookingUpdateResponseData.Success != null)
                                                {
                                                    //if (drRow["ChangeFee2"].ToString() == "0")
                                                    //{
                                                    //response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue - Convert.ToDecimal(Session["BalanceNameChange"].ToString());
                                                    PassData.ChangeFee2 = Convert.ToDecimal(response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue) - TotalFee;
                                                    PassData.ChangeDate2 = DateTime.Now;
                                                    drRow["ChangeFee2"] = PassData.ChangeFee2;
                                                    //}
                                                    BalanceAmount[drRow["PNR"].ToString()] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;
                                                    //Session["BalanceNameChange"] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;

                                                    TotalFee += Convert.ToDecimal(drRow["ChangeFee"].ToString());
                                                }
                                                else
                                                {

                                                }
                                            }
                                        }
                                        else
                                        {
                                            SellResponse response = objBooking.AddNameChangeFees("", drRow["Currency"].ToString(), Convert.ToInt32(drRow["PassengerID"]), drRow["PNR"].ToString());
                                            if (response.BookingUpdateResponseData.Success != null)
                                            {
                                                //if (drRow["ChangeFee2"].ToString() == "0")
                                                //{
                                                //response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue - Convert.ToDecimal(Session["BalanceNameChange"].ToString());
                                                PassData.ChangeFee2 = Convert.ToDecimal(response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue) - TotalFee;
                                                PassData.ChangeDate2 = DateTime.Now;
                                                drRow["ChangeFee2"] = PassData.ChangeFee2;
                                                //}
                                                BalanceAmount[drRow["PNR"].ToString()] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;
                                                //Session["BalanceNameChange"] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;

                                                TotalFee += Convert.ToDecimal(drRow["ChangeFee"].ToString());
                                            }
                                            else
                                            {

                                            }
                                        }
                                        SeqNo += 1;
                                    }
                                }
                                else if (Convert.ToInt32(drRow["ChangeCount"]) == 1 && Convert.ToInt32(drRow["PaxNo"]) == 1 && ChangeLimit == 1)
                                //else if (Convert.ToInt32(drRow["ChangeCount"]) == 1 && Convert.ToInt32(drRow["PaxNo"]) == 1 && NameChangeMax >= 1)
                                {
                                    if (IsFree1 == 0 && MaxPax1 != 0)
                                    //if (NameChangeLimit1 != 0)
                                    {
                                        if (SeqNo > Convert.ToDecimal(TotalPax * (MaxPax1 / 100)))
                                        //if (SeqNo > Convert.ToDecimal(TotalPax * (NameChangeLimit1)))
                                        {
                                            SellResponse response = objBooking.AddNameChangeFees("", drRow["Currency"].ToString(), Convert.ToInt32(drRow["PassengerID"]), drRow["PNR"].ToString());
                                            if (response.BookingUpdateResponseData.Success != null)
                                            {
                                                //if (drRow["ChangeFee"].ToString() == "0")
                                                //{
                                                //PassData.ChangeFee = Convert.ToDecimal(response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue) - Convert.ToDecimal(Session["BalanceNameChange"].ToString());
                                                PassData.ChangeFee = Convert.ToDecimal(response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue) - TotalFee;
                                                PassData.ChangeDate = DateTime.Now;

                                                drRow["ChangeFee"] = PassData.ChangeFee;
                                                //}
                                                BalanceAmount[drRow["PNR"].ToString()] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;
                                                //Session["BalanceNameChange"] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;

                                                TotalFee += Convert.ToDecimal(drRow["ChangeFee"].ToString());
                                            }
                                            else
                                            {

                                            }
                                        }
                                    }
                                    else if (IsFree1 == 1 && TimeDiff <= MaxFreeTime1)
                                    {
                                        SellResponse response = objBooking.AddNameChangeFees("", drRow["Currency"].ToString(), Convert.ToInt32(drRow["PassengerID"]), drRow["PNR"].ToString());
                                        if (response.BookingUpdateResponseData.Success != null)
                                        {
                                            //if (drRow["ChangeFee"].ToString() == "0")
                                            //{
                                            //PassData.ChangeFee = Convert.ToDecimal(response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue) - Convert.ToDecimal(Session["BalanceNameChange"].ToString());
                                            PassData.ChangeFee = Convert.ToDecimal(response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue) - TotalFee;
                                            PassData.ChangeDate = DateTime.Now;

                                            drRow["ChangeFee"] = PassData.ChangeFee;
                                            //}
                                            BalanceAmount[drRow["PNR"].ToString()] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;
                                            //Session["BalanceNameChange"] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;

                                            TotalFee += Convert.ToDecimal(drRow["ChangeFee"].ToString());
                                        }
                                        else
                                        {

                                        }
                                    }
                                    SeqNo += 1;
                                }
                                else if (Convert.ToInt32(drRow["ChangeCount"]) > 1 && Convert.ToInt32(drRow["PaxNo"]) == 2 && ChangeLimit == 2)
                                //else if (Convert.ToInt32(drRow["ChangeCount"]) > 1 && Convert.ToInt32(drRow["PaxNo"]) == 2 && NameChangeMax == 2)
                                {
                                    if (IsFree2 == 0 && MaxPax2 != 0)
                                    //if (NameChangeLimit2 != 0)
                                    {
                                        if (SeqNo > Convert.ToDecimal(TotalPax * (MaxPax2 / 100)))
                                        //if (SeqNo > Convert.ToDecimal(TotalPax * (NameChangeLimit2)))
                                        {
                                            SellResponse response = objBooking.AddNameChangeFees("", drRow["Currency"].ToString(), Convert.ToInt32(drRow["PassengerID"]), drRow["PNR"].ToString());
                                            if (response.BookingUpdateResponseData.Success != null)
                                            {
                                                //if (drRow["ChangeFee2"].ToString() == "0")
                                                //{
                                                //response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue - Convert.ToDecimal(Session["BalanceNameChange"].ToString());
                                                PassData.ChangeFee2 = Convert.ToDecimal(response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue) - TotalFee;
                                                PassData.ChangeDate2 = DateTime.Now;
                                                drRow["ChangeFee2"] = PassData.ChangeFee2;
                                                //}
                                                BalanceAmount[drRow["PNR"].ToString()] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;
                                                //Session["BalanceNameChange"] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;

                                                TotalFee += Convert.ToDecimal(drRow["ChangeFee"].ToString());
                                            }
                                            else
                                            {

                                            }
                                        }
                                    }
                                    else if (IsFree2 == 1 && TimeDiff <= MaxFreeTime2)
                                    {
                                        SellResponse response = objBooking.AddNameChangeFees("", drRow["Currency"].ToString(), Convert.ToInt32(drRow["PassengerID"]), drRow["PNR"].ToString());
                                        if (response.BookingUpdateResponseData.Success != null)
                                        {
                                            //if (drRow["ChangeFee2"].ToString() == "0")
                                            //{
                                            //response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue - Convert.ToDecimal(Session["BalanceNameChange"].ToString());
                                            PassData.ChangeFee2 = Convert.ToDecimal(response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue) - TotalFee;
                                            PassData.ChangeDate2 = DateTime.Now;
                                            drRow["ChangeFee2"] = PassData.ChangeFee2;
                                            //}
                                            BalanceAmount[drRow["PNR"].ToString()] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;
                                            //Session["BalanceNameChange"] = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;

                                            TotalFee += Convert.ToDecimal(drRow["ChangeFee"].ToString());
                                        }
                                        else
                                        {

                                        }
                                    }
                                    SeqNo += 1;
                                }
                                else
                                {

                                }
                                //end AA

                            }
                        }

                        decimal TotalBalanceAmount = 0;

                        foreach (var AmtBalance in BalanceAmount.Values)
                        {
                            TotalBalanceAmount += AmtBalance;
                        }
                        Session["BalanceNameChange"] = TotalBalanceAmount;
                        Session["BalanceAmount"] = BalanceAmount;

                    }
                    //}
                }

                object sumObjects;
                sumObjects = dtPass.Compute("Sum(ChangeFee)", "");
                object sumObjects2;
                sumObjects2 = dtPass.Compute("Sum(ChangeFee2)", "");

                //lblTotalAmount.Text = (Convert.ToDecimal(sumObjects) + Convert.ToDecimal(sumObjects2) + Convert.ToDecimal(sumObjects1) + Convert.ToDecimal(sumObjects21)).ToString("N", nfi);
                lblTotalAmount.Text = Convert.ToDecimal(Session["BalanceNameChange"]).ToString("N", nfi);// (Convert.ToDecimal(sumObjects) + Convert.ToDecimal(sumObjects2)).ToString("N", nfi);

                lblCurrency.Text = dtPass.Rows[0]["Currency"].ToString();
                if (TimeDiff >= MaxChangeTime)
                //if (TimeDiff >= LimitTime)
                {
                    btConfirm.Visible = true;
                    Session["IsAllow"] = 1;
                }
                else
                {
                    btConfirm.Visible = false;
                    Session["IsAllow"] = 0;
                }

                //foreach (DataRow row in dtPass.Rows)
                //{
                //    row["PaxNo"] = 0;
                //}

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void gvPassenger_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            MessageList msgList = new MessageList();
            try
            {

                //foreach (var args in e.UpdateValues)
                //{
                //    string FirstChar = "";
                //    int num = 0;
                //    if (args.NewValues["FirstName"].ToString() != "")
                //    {

                //        FirstChar = args.NewValues["FirstName"].ToString().Substring(0,1);
                //        if (int.TryParse(FirstChar, out num) == true)
                //        {
                //            e.Handled = false;
                //        }
                //    }
                //}

                //20170321 - Sienny
                if (Session["bookHDRInfo"] != null) bookHDRInfo = (BookingTransactionMain)Session["bookHDRInfo"];
                Session["error"] = null;
                ArrayList arrayerror = new ArrayList();
                string errorMessage = "";
                //int counterror = 0;
                string title = "";
                BookingTransactionDetail = objBooking.Get_TRANSDTL(AgentSet.AgentID, TransID);

                DataTable data = Session["dtGridPass"] as DataTable;
                if (Session["dtNewGridPass"] == null) Session["dtNewGridPass"] = data.Copy();
                DataTable dataNew = Session["dtNewGridPass"] as DataTable;

                int index = 0;
                if ((int)Session["CurStatus"] == 3)
                {
                    foreach (DataRow drRow in dataNew.Rows)
                    {
                        if (drRow["FirstName"].ToString().ToUpper() == data.Rows[index]["FirstName"].ToString().ToUpper() && drRow["LastName"].ToString().ToUpper() == data.Rows[index]["LastName"].ToString().ToUpper() && drRow["IssuingCountry"].ToString().ToUpper() == data.Rows[index]["IssuingCountry"].ToString().ToUpper() && drRow["Nationality"].ToString().ToUpper() == data.Rows[index]["Nationality"].ToString().ToUpper() && drRow["Title"].ToString().ToUpper() == data.Rows[index]["Title"].ToString().ToUpper() && drRow["Gender"].ToString().ToUpper() == data.Rows[index]["Gender"].ToString().ToUpper() && Convert.ToDateTime(drRow["DOB"]) == Convert.ToDateTime(data.Rows[index]["DOB"]) && drRow["PassportNo"].ToString().ToUpper() == data.Rows[index]["PassportNo"].ToString().ToUpper() && (drRow["ExpiryDate"] != DBNull.Value && Convert.ToDateTime(drRow["ExpiryDate"]) == Convert.ToDateTime(data.Rows[index]["ExpiryDate"])))
                        {
                            drRow["ChangeFee"] = data.Rows[index]["ChangeFee"];
                            drRow["ChangeDate"] = data.Rows[index]["ChangeDate"];
                            drRow["ChangeFee2"] = data.Rows[index]["ChangeFee2"];
                            drRow["ChangeDate2"] = data.Rows[index]["ChangeDate2"];
                            drRow["ChangeCount"] = data.Rows[index]["ChangeCount"];
                            drRow["PaxNo"] = data.Rows[index]["PaxNo"];
                        }
                        index += 1;
                    }
                }

                string countpa = "", countfe = "", countma = "", countcon = "", countdob = "", countnat = "", name = "";
                foreach (var args in e.UpdateValues)
                {
                    int i;
                    DateTime DOB = new DateTime();
                    //data.PrimaryKey = new DataColumn[] { (data.Columns["PassengerID"]), (data.Columns["PNR"]) };
                    //dataNew.PrimaryKey = new DataColumn[] { (dataNew.Columns["PassengerID"]), (dataNew.Columns["PNR"]) };

                    object[] findTheseVals = new object[2];

                    // Set the values of the keys to find.
                    findTheseVals[0] = (args.Keys["PassengerID"]);
                    findTheseVals[1] = (args.Keys["PNR"]);
                    DataRow rowNew = dataNew.Select("PassengerID='" + findTheseVals[0] + "' AND PNR='" + findTheseVals[1] + "'").FirstOrDefault();

                    //20170321 - Sienny
                    DataRow rowNewDuplicate = null;// = dataNew.Select("FirstName like '" + args.NewValues["FirstName"].ToString() + "' AND LastName like '" + args.NewValues["LastName"].ToString() + "' AND PNR='" + findTheseVals[1] + "'").FirstOrDefault();
                    ////20170405 - Sienny (domestic flight hide PassportNo and ExpiryDate field)
                    //if (BookingTransactionDetail != null && BookingTransactionDetail.Count > 0)
                    //{
                    //    IsInternationalFlight = objGeneral.IsInternationalFlight(BookingTransactionDetail[0].Origin, BookingTransactionDetail[0].Destination, Request.PhysicalApplicationPath);

                    //    if (IsInternationalFlight)
                    //    {
                    //        if (args.NewValues["ExpiryDate"] != null && args.NewValues["PassportNo"].ToString() != "TBA")
                    //        {
                    //            rowNewDuplicate = dataNew.Select(
                    //                "FirstName LIKE '" + args.NewValues["FirstName"].ToString() + "' AND LastName LIKE '" + args.NewValues["LastName"].ToString() +
                    //                "' AND IssuingCountry = '" + args.NewValues["IssuingCountry"].ToString() + "' AND Nationality = '" + args.NewValues["Nationality"].ToString() +
                    //                "' AND Title = '" + args.NewValues["Title"].ToString() + "' AND Gender = '" + args.NewValues["Gender"].ToString() +
                    //                "' AND DOB = '" + args.NewValues["DOB"].ToString() + "' AND PassportNo = '" + args.NewValues["PassportNo"].ToString() +
                    //                "' AND ExpiryDate = '" + args.NewValues["ExpiryDate"].ToString() + "' AND PNR='" + findTheseVals[1] + "'").FirstOrDefault();
                    //        }
                    //        else
                    //        {
                    //            //arrayerror.Add("ExpiryDate;" + (Convert.ToInt16(args.NewValues["RowNo"].ToString()) - 1));
                    //            //arrayerror.Add("PassportNo;" + (Convert.ToInt16(args.NewValues["RowNo"].ToString()) - 1));
                    //            //errorMessage += "Passport No and Expired Date is Required\n";
                    //            //counterror += 1;
                    //            countpa = args.NewValues["RowNo"].ToString();
                    //            name = args.NewValues["FirstName"].ToString();
                    //            errorMessage += "Data not match on row " + countpa + ", Passport No and Expired Date is Required, for Passenger [" + name + "]\n";
                    //            //break;
                    //            //gvPassenger.JSProperties["cp_result"] = "Passport No and Expired Date is Required";
                    //        }
                    //    }
                    //    else
                    //    {
                    //        rowNewDuplicate = dataNew.Select(
                    //            "FirstName LIKE '" + args.NewValues["FirstName"].ToString() + "' AND LastName LIKE '" + args.NewValues["LastName"].ToString() +
                    //            "' AND IssuingCountry = '" + args.NewValues["IssuingCountry"].ToString() + "' AND Nationality = '" + args.NewValues["Nationality"].ToString() +
                    //            "' AND Title = '" + args.NewValues["Title"].ToString() + "' AND Gender = '" + args.NewValues["Gender"].ToString() +
                    //            "' AND DOB = '" + args.NewValues["DOB"].ToString() + "' AND PassportNo = '' AND ExpiryDate IS NULL " +
                    //            " AND PNR='" + findTheseVals[1] + "'").FirstOrDefault();
                    //    }
                    //}

                    //edited by romy
                    if (BookingTransactionDetail != null && BookingTransactionDetail.Count > 0)
                    {
                        IsInternationalFlight = objGeneral.IsInternationalFlight(BookingTransactionDetail[0].Origin, BookingTransactionDetail[0].Destination, Request.PhysicalApplicationPath);
                    }

                    if (IsInternationalFlight)
                    {
                        if (args.NewValues["ExpiryDate"] != null && args.NewValues["PassportNo"].ToString() != "TBA")
                        {
                            rowNewDuplicate = dataNew.Select(
                                "FirstName LIKE '" + args.NewValues["FirstName"].ToString() + "' AND LastName LIKE '" + args.NewValues["LastName"].ToString() +
                                "' AND IssuingCountry = '" + args.NewValues["IssuingCountry"].ToString() + "' AND Nationality = '" + args.NewValues["Nationality"].ToString() +
                                "' AND Title = '" + args.NewValues["Title"].ToString() + "' AND Gender = '" + args.NewValues["Gender"].ToString() +
                                "' AND DOB = '" + args.NewValues["DOB"].ToString() + "' AND PNR='" + findTheseVals[1] + "'").FirstOrDefault();
                        }
                        else
                        {
                            countpa = args.NewValues["RowNo"].ToString();
                            name = args.NewValues["FirstName"].ToString();
                            errorMessage += "Data not match on row " + countpa + ", Passport No and Expired Date is Required, for Passenger [" + name + "]\n";
                        }
                    }
                    else
                    {
                        rowNewDuplicate = dataNew.Select(
                            "FirstName LIKE '" + args.NewValues["FirstName"].ToString() + "' AND LastName LIKE '" + args.NewValues["LastName"].ToString() +
                            "' AND IssuingCountry = '" + args.NewValues["IssuingCountry"].ToString() + "' AND Nationality = '" + args.NewValues["Nationality"].ToString() +
                            "' AND Title = '" + args.NewValues["Title"].ToString() + "' AND Gender = '" + args.NewValues["Gender"].ToString() +
                            "' AND DOB = '" + args.NewValues["DOB"].ToString() + "' AND PNR='" + findTheseVals[1] + "'").FirstOrDefault();
                    }

                    if (rowNewDuplicate != null && rowNewDuplicate.ItemArray.Length > 0)
                    {
                        //dataTbl = dtrow.CopyToDataTable(); //dataTbl is the DataTable
                        errorMessage += "Cannot add duplicate name on row " + rowNewDuplicate["RowNo"] + " and " + args.NewValues["RowNo"].ToString() + " for Passenger [" + args.NewValues["FirstName"].ToString() + "] </br>";
                        //return;
                    }
                    else
                    {
                        if ((int)Session["CurStatus"] == 3)
                        {
                            DataRow row = data.Select("PassengerID='" + findTheseVals[0] + "' AND PNR='" + findTheseVals[1] + "'").FirstOrDefault();
                            //i = System.Convert.ToInt32(args.Keys["PassengerID"]);

                            //remarked
                            if ((int)row["ChangeCount"] == 0)
                            {
                                //if (row["FirstName"].ToString().ToUpper() != args.NewValues["FirstName"].ToString().ToUpper() || row["LastName"].ToString().ToUpper() != args.NewValues["LastName"].ToString().ToUpper())
                                if (row["FirstName"].ToString().ToUpper() != args.NewValues["FirstName"].ToString().ToUpper().Trim() || row["LastName"].ToString().ToUpper() != args.NewValues["LastName"].ToString().ToUpper().Trim() || row["IssuingCountry"].ToString().ToUpper() != args.NewValues["IssuingCountry"].ToString().ToUpper().Trim() || row["Nationality"].ToString().ToUpper() != args.NewValues["Nationality"].ToString().ToUpper().Trim() || row["Title"].ToString().ToUpper() != args.NewValues["Title"].ToString().ToUpper().Trim() || row["Gender"].ToString().ToUpper() != args.NewValues["Gender"].ToString().ToUpper().Trim() || Convert.ToDateTime(row["DOB"]) != Convert.ToDateTime(args.NewValues["DOB"]) || row["PassportNo"].ToString().ToUpper() != args.NewValues["PassportNo"].ToString().ToUpper().Trim() || (row["ExpiryDate"] != DBNull.Value && Convert.ToDateTime(row["ExpiryDate"]) != Convert.ToDateTime(args.NewValues["ExpiryDate"]))) //Check for space (20170317 - Sienny)
                                {
                                    rowNew["PrevFirstName1"] = row["FirstName"];
                                    rowNew["ChangeCount"] = 1;
                                    rowNew["ChangeCnt"] = "1x";
                                    rowNew["ChangeFee"] = 0;
                                    rowNew["ChangeDate"] = DateTime.Now;

                                    rowNew["PrevLastName1"] = row["LastName"];
                                    rowNew["ChangeCount"] = 1;
                                    rowNew["ChangeCnt"] = "1x";
                                    rowNew["ChangeFee"] = 0;
                                    rowNew["ChangeDate"] = DateTime.Now;

                                    rowNew["PrevIssuingCountry1"] = row["IssuingCountry"];
                                    rowNew["ChangeCount"] = 1;
                                    rowNew["ChangeCnt"] = "1x";
                                    rowNew["ChangeFee"] = 0;
                                    rowNew["ChangeDate"] = DateTime.Now;

                                    rowNew["PrevNationality1"] = row["Nationality"];
                                    rowNew["ChangeCount"] = 1;
                                    rowNew["ChangeCnt"] = "1x";
                                    rowNew["ChangeFee"] = 0;
                                    rowNew["ChangeDate"] = DateTime.Now;

                                    rowNew["PrevTitle1"] = row["Title"];
                                    rowNew["ChangeCount"] = 1;
                                    rowNew["ChangeCnt"] = "1x";
                                    rowNew["ChangeFee"] = 0;
                                    rowNew["ChangeDate"] = DateTime.Now;

                                    rowNew["PrevGender1"] = row["Gender"];
                                    rowNew["ChangeCount"] = 1;
                                    rowNew["ChangeCnt"] = "1x";
                                    rowNew["ChangeFee"] = 0;
                                    rowNew["ChangeDate"] = DateTime.Now;

                                    rowNew["PrevDOB1"] = row["DOB"];
                                    rowNew["ChangeCount"] = 1;
                                    rowNew["ChangeCnt"] = "1x";
                                    rowNew["ChangeFee"] = 0;
                                    rowNew["ChangeDate"] = DateTime.Now;

                                    rowNew["PrevPassportNo1"] = row["PassportNo"];
                                    rowNew["ChangeCount"] = 1;
                                    rowNew["ChangeCnt"] = "1x";
                                    rowNew["ChangeFee"] = 0;
                                    rowNew["ChangeDate"] = DateTime.Now;

                                    rowNew["PrevExpiryDate1"] = row["ExpiryDate"];
                                    rowNew["ChangeCount"] = 1;
                                    rowNew["ChangeCnt"] = "1x";
                                    rowNew["ChangeFee"] = 0;
                                    rowNew["ChangeDate"] = DateTime.Now;
                                    rowNew["PaxNo"] = 1;
                                }
                                else
                                {
                                    rowNew["PrevFirstName1"] = row["PrevFirstName1"];
                                    rowNew["ChangeCount"] = row["ChangeCount"];
                                    rowNew["ChangeCnt"] = row["ChangeCnt"];
                                    rowNew["ChangeFee"] = row["ChangeFee"];
                                    rowNew["ChangeDate"] = row["ChangeDate"];

                                    rowNew["PrevLastName1"] = row["PrevLastName1"];
                                    rowNew["ChangeCount"] = row["ChangeCount"];
                                    rowNew["ChangeCnt"] = row["ChangeCnt"];
                                    rowNew["ChangeFee"] = row["ChangeFee"];
                                    rowNew["ChangeDate"] = row["ChangeDate"];

                                    rowNew["PrevIssuingCountry1"] = row["PrevIssuingCountry1"];
                                    rowNew["ChangeCount"] = row["ChangeCount"];
                                    rowNew["ChangeCnt"] = row["ChangeCnt"];
                                    rowNew["ChangeFee"] = row["ChangeFee"];
                                    rowNew["ChangeDate"] = row["ChangeDate"];

                                    rowNew["PrevNationality1"] = row["PrevNationality1"];
                                    rowNew["ChangeCount"] = row["ChangeCount"];
                                    rowNew["ChangeCnt"] = row["ChangeCnt"];
                                    rowNew["ChangeFee"] = row["ChangeFee"];
                                    rowNew["ChangeDate"] = row["ChangeDate"];

                                    rowNew["PrevTitle1"] = row["PrevTitle1"];
                                    rowNew["ChangeCount"] = row["ChangeCount"];
                                    rowNew["ChangeCnt"] = row["ChangeCnt"];
                                    rowNew["ChangeFee"] = row["ChangeFee"];
                                    rowNew["ChangeDate"] = row["ChangeDate"];

                                    rowNew["PrevGender1"] = row["PrevGender1"];
                                    rowNew["ChangeCount"] = row["ChangeCount"];
                                    rowNew["ChangeCnt"] = row["ChangeCnt"];
                                    rowNew["ChangeFee"] = row["ChangeFee"];
                                    rowNew["ChangeDate"] = row["ChangeDate"];

                                    rowNew["PrevDOB1"] = row["PrevDOB1"];
                                    rowNew["ChangeCount"] = row["ChangeCount"];
                                    rowNew["ChangeCnt"] = row["ChangeCnt"];
                                    rowNew["ChangeFee"] = row["ChangeFee"];
                                    rowNew["ChangeDate"] = row["ChangeDate"];

                                    rowNew["PrevPassportNo1"] = row["PrevPassportNo1"];
                                    rowNew["ChangeCount"] = row["ChangeCount"];
                                    rowNew["ChangeCnt"] = row["ChangeCnt"];
                                    rowNew["ChangeFee"] = row["ChangeFee"];
                                    rowNew["ChangeDate"] = row["ChangeDate"];

                                    rowNew["PrevExpiryDate1"] = row["PrevExpiryDate1"];
                                    rowNew["ChangeCount"] = row["ChangeCount"];
                                    rowNew["ChangeCnt"] = row["ChangeCnt"];
                                    rowNew["ChangeFee"] = row["ChangeFee"];
                                    rowNew["ChangeDate"] = row["ChangeDate"];

                                    rowNew["PaxNo"] = row["PaxNo"];
                                }
                            }
                            else if ((int)row["ChangeCount"] >= 1)
                            {
                                //if (row["FirstName"].ToString().ToUpper() != args.NewValues["FirstName"].ToString().ToUpper() || row["LastName"].ToString().ToUpper() != args.NewValues["LastName"].ToString().ToUpper())
                                if (row["FirstName"].ToString().ToUpper() != args.NewValues["FirstName"].ToString().ToUpper().Trim() || row["LastName"].ToString().ToUpper() != args.NewValues["LastName"].ToString().ToUpper().Trim() || row["IssuingCountry"].ToString().ToUpper() != args.NewValues["IssuingCountry"].ToString().ToUpper().Trim() || row["Nationality"].ToString().ToUpper() != args.NewValues["Nationality"].ToString().ToUpper().Trim() || row["Title"].ToString().ToUpper() != args.NewValues["Title"].ToString().ToUpper().Trim() || row["Gender"].ToString().ToUpper() != args.NewValues["Gender"].ToString().ToUpper().Trim() || Convert.ToDateTime(row["DOB"]) != Convert.ToDateTime(args.NewValues["DOB"]) || row["PassportNo"].ToString().ToUpper() != args.NewValues["PassportNo"].ToString().ToUpper().Trim() || (row["ExpiryDate"] != DBNull.Value && Convert.ToDateTime(row["ExpiryDate"]) != Convert.ToDateTime(args.NewValues["ExpiryDate"]))) //Check for space (20170317 - Sienny)
                                {
                                    rowNew["PrevFirstName2"] = row["FirstName"];
                                    rowNew["ChangeCount"] = 2;
                                    rowNew["ChangeCnt"] = "2x";
                                    rowNew["CountChanged"] = "-1";
                                    rowNew["ChangeFee2"] = 0;
                                    rowNew["ChangeDate2"] = DateTime.Now;

                                    rowNew["PrevLastName2"] = row["LastName"];
                                    rowNew["ChangeCount"] = 2;
                                    rowNew["ChangeCnt"] = "2x";
                                    rowNew["CountChanged"] = "-1";
                                    rowNew["ChangeFee2"] = 0;
                                    rowNew["ChangeDate2"] = DateTime.Now;

                                    rowNew["PrevIssuingCountry2"] = row["IssuingCountry"];
                                    rowNew["ChangeCount"] = 2;
                                    rowNew["ChangeCnt"] = "2x";
                                    rowNew["CountChanged"] = "-1";
                                    rowNew["ChangeFee2"] = 0;
                                    rowNew["ChangeDate2"] = DateTime.Now;

                                    rowNew["PrevNationality2"] = row["Nationality"];
                                    rowNew["ChangeCount"] = 2;
                                    rowNew["ChangeCnt"] = "2x";
                                    rowNew["CountChanged"] = "-1";
                                    rowNew["ChangeFee2"] = 0;
                                    rowNew["ChangeDate2"] = DateTime.Now;

                                    rowNew["PrevTitle2"] = row["Title"];
                                    rowNew["ChangeCount"] = 2;
                                    rowNew["ChangeCnt"] = "2x";
                                    rowNew["CountChanged"] = "-1";
                                    rowNew["ChangeFee2"] = 0;
                                    rowNew["ChangeDate2"] = DateTime.Now;

                                    rowNew["PrevGender2"] = row["Gender"];
                                    rowNew["ChangeCount"] = 2;
                                    rowNew["ChangeCnt"] = "2x";
                                    rowNew["CountChanged"] = "-1";
                                    rowNew["ChangeFee2"] = 0;
                                    rowNew["ChangeDate2"] = DateTime.Now;

                                    rowNew["PrevDOB2"] = row["DOB"];
                                    rowNew["ChangeCount"] = 2;
                                    rowNew["ChangeCnt"] = "2x";
                                    rowNew["CountChanged"] = "-1";
                                    rowNew["ChangeFee2"] = 0;
                                    rowNew["ChangeDate2"] = DateTime.Now;

                                    rowNew["PrevPassportNo2"] = row["PassportNo"];
                                    rowNew["ChangeCount"] = 2;
                                    rowNew["ChangeCnt"] = "2x";
                                    rowNew["CountChanged"] = "-1";
                                    rowNew["ChangeFee2"] = 0;
                                    rowNew["ChangeDate2"] = DateTime.Now;

                                    rowNew["PrevExpiryDate2"] = row["ExpiryDate"];
                                    rowNew["ChangeCount"] = 2;
                                    rowNew["ChangeCnt"] = "2x";
                                    rowNew["CountChanged"] = "-1";
                                    rowNew["ChangeFee2"] = 0;
                                    rowNew["ChangeDate2"] = DateTime.Now;

                                    rowNew["PaxNo"] = 2;
                                }
                                else
                                {
                                    rowNew["PrevFirstName1"] = row["PrevFirstName1"];
                                    rowNew["ChangeCount"] = row["ChangeCount"];
                                    rowNew["ChangeCnt"] = row["ChangeCnt"];
                                    rowNew["ChangeFee"] = row["ChangeFee"];
                                    rowNew["ChangeDate"] = row["ChangeDate"];

                                    rowNew["PrevLastName1"] = row["PrevLastName1"];
                                    rowNew["ChangeCount"] = row["ChangeCount"];
                                    rowNew["ChangeCnt"] = row["ChangeCnt"];
                                    rowNew["ChangeFee"] = row["ChangeFee"];
                                    rowNew["ChangeDate"] = row["ChangeDate"];

                                    rowNew["PrevIssuingCountry1"] = row["PrevIssuingCountry1"];
                                    rowNew["ChangeCount"] = row["ChangeCount"];
                                    rowNew["ChangeCnt"] = row["ChangeCnt"];
                                    rowNew["ChangeFee"] = row["ChangeFee"];
                                    rowNew["ChangeDate"] = row["ChangeDate"];

                                    rowNew["PrevNationality1"] = row["PrevNationality1"];
                                    rowNew["ChangeCount"] = row["ChangeCount"];
                                    rowNew["ChangeCnt"] = row["ChangeCnt"];
                                    rowNew["ChangeFee"] = row["ChangeFee"];
                                    rowNew["ChangeDate"] = row["ChangeDate"];

                                    rowNew["PrevTitle1"] = row["PrevTitle1"];
                                    rowNew["ChangeCount"] = row["ChangeCount"];
                                    rowNew["ChangeCnt"] = row["ChangeCnt"];
                                    rowNew["ChangeFee"] = row["ChangeFee"];
                                    rowNew["ChangeDate"] = row["ChangeDate"];

                                    rowNew["PrevGender1"] = row["PrevGender1"];
                                    rowNew["ChangeCount"] = row["ChangeCount"];
                                    rowNew["ChangeCnt"] = row["ChangeCnt"];
                                    rowNew["ChangeFee"] = row["ChangeFee"];
                                    rowNew["ChangeDate"] = row["ChangeDate"];

                                    rowNew["PrevDOB1"] = row["PrevDOB1"];
                                    rowNew["ChangeCount"] = row["ChangeCount"];
                                    rowNew["ChangeCnt"] = row["ChangeCnt"];
                                    rowNew["ChangeFee"] = row["ChangeFee"];
                                    rowNew["ChangeDate"] = row["ChangeDate"];

                                    rowNew["PrevPassportNo1"] = row["PrevPassportNo1"];
                                    rowNew["ChangeCount"] = row["ChangeCount"];
                                    rowNew["ChangeCnt"] = row["ChangeCnt"];
                                    rowNew["ChangeFee"] = row["ChangeFee"];
                                    rowNew["ChangeDate"] = row["ChangeDate"];

                                    rowNew["PrevExpiryDate1"] = row["PrevExpiryDate1"];
                                    rowNew["ChangeCount"] = row["ChangeCount"];
                                    rowNew["ChangeCnt"] = row["ChangeCnt"];
                                    rowNew["ChangeFee"] = row["ChangeFee"];
                                    rowNew["ChangeDate"] = row["ChangeDate"];
                                    rowNew["PaxNo"] = row["PaxNo"];
                                }

                            }
                            //remarked
                        }


                        object[] findTheseVal = new object[2];

                        // Set the values of the keys to find.
                        //findTheseVal[0] = (args.Keys["PassengerID"]);
                        //findTheseVal[1] = (args.Keys["PNR"]);

                        //DataRow rows = dataNew.Rows.Find(findTheseVal);
                        //DataRow rows = data.Rows.Find(args.Keys["PassengerID"]);

                        rowNew["PassengerID"] = args.Keys["PassengerID"];
                        rowNew["IssuingCountry"] = args.NewValues["IssuingCountry"];
                        rowNew["Nationality"] = args.NewValues["Nationality"];

                        //20170614 - Sienny
                        DataTable dtCountryName = objGeneral.GetAllCountryCard();
                        DataRow rowIssuingCountryName = dtCountryName.Select("CountryCode like '" + args.NewValues["IssuingCountry"].ToString() + "'").FirstOrDefault();
                        DataRow rowCountryName = dtCountryName.Select("CountryCode like '" + args.NewValues["Nationality"].ToString() + "'").FirstOrDefault();
                        if (rowIssuingCountryName == null)
                        {
                            custommessage = "";
                            countcon = args.NewValues["RowNo"].ToString();
                            name = args.NewValues["FirstName"].ToString();
                            custommessage = msgList.Err800002.Replace("countcon", countcon).Replace("name", name);
                            errorMessage += custommessage;// "Data not match on row " + countcon + ", IssuingCountry not found, for Passenger [" + name + "]\n";
                            //arrayerror.Add("IssuingCountry;" + (Convert.ToInt16(args.NewValues["RowNo"].ToString()) - 1));
                            //errorMessage += "IssuingCountry not found\n";
                            //counterror += 1;
                        }
                        else
                        {
                            rowNew["IssuingCountryName"] = rowIssuingCountryName["Name"].ToString();
                        }
                        if (rowCountryName == null)
                        {
                            custommessage = "";
                            countnat = args.NewValues["RowNo"].ToString();
                            name = args.NewValues["FirstName"].ToString();
                            custommessage = msgList.Err800003.Replace("countcon", countcon).Replace("name", name);
                            errorMessage += custommessage;//"Data not match on row " + countnat + ", Nationality not found, for Passenger [" + name + "]\n";
                            //arrayerror.Add("Nationality;" + (Convert.ToInt16(args.NewValues["RowNo"].ToString()) - 1));
                            //errorMessage += "Nationality not found\n";
                            //counterror += 1;
                        }
                        else
                        {
                            rowNew["CountryName"] = rowCountryName["Name"].ToString();
                        }

                        if (((args.NewValues["Title"].ToString().ToUpper() == "MS" && args.OldValues["Title"].ToString().ToUpper() != "MS") || (args.NewValues["Title"].ToString().ToUpper() == "MRS" && args.OldValues["Title"].ToString().ToUpper() != "MRS")) && args.NewValues["Gender"].ToString().ToUpper() != "FEMALE")
                        {
                            //numfe = 2;
                            //countfe = args.NewValues["RowNo"].ToString();
                            //name = args.NewValues["FirstName"].ToString();

                            rowNew["Title"] = args.NewValues["Title"];
                            rowNew["Gender"] = "Female";
                            title = args.NewValues["Title"].ToString().ToLower();
                        }
                        else if (((args.NewValues["Title"].ToString().ToUpper() == "MS" && args.OldValues["Title"].ToString().ToUpper() == "MS") || (args.NewValues["Title"].ToString().ToUpper() == "MRS" && args.OldValues["Title"].ToString().ToUpper() == "MRS")) && args.NewValues["Gender"].ToString().ToUpper() != "FEMALE")
                        {
                            //numfe = 2;
                            //countfe = args.NewValues["RowNo"].ToString();
                            //name = args.NewValues["FirstName"].ToString();

                            rowNew["Title"] = "MR";
                            rowNew["Gender"] = "Male";
                            title = args.NewValues["Title"].ToString().ToLower();
                        }
                        else if (args.NewValues["Title"].ToString().ToUpper() == "MR" && args.OldValues["Title"].ToString().ToUpper() != "MR" && args.NewValues["Gender"].ToString().ToUpper() != "MALE")
                        {
                            //numma = 3;
                            //countma = args.NewValues["RowNo"].ToString();
                            //name = args.NewValues["FirstName"].ToString();

                            rowNew["Title"] = args.NewValues["Title"];
                            rowNew["Gender"] = "Male";
                            title = args.NewValues["Title"].ToString().ToLower();
                        }
                        else if (args.NewValues["Title"].ToString().ToUpper() == "MR" && args.OldValues["Title"].ToString().ToUpper() == "MR" && args.NewValues["Gender"].ToString().ToUpper() != "MALE")
                        {
                            //numma = 3;
                            //countma = args.NewValues["RowNo"].ToString();
                            //name = args.NewValues["FirstName"].ToString();

                            rowNew["Title"] = "MS";
                            rowNew["Gender"] = args.NewValues["Gender"].ToString();
                            title = args.NewValues["Title"].ToString().ToLower();
                        }
                        else if (args.NewValues["Title"].ToString().ToUpper() != "MR" && args.NewValues["Title"].ToString().ToUpper() != "MS" && args.NewValues["Title"].ToString().ToUpper() == "MRS" && args.NewValues["Title"].ToString().ToUpper() != "CHD")
                        {
                            custommessage = "";
                            countfe = args.NewValues["RowNo"].ToString();
                            name = args.NewValues["FirstName"].ToString();
                            custommessage = msgList.Err800004.Replace("countfe", countfe).Replace("name", name);
                            errorMessage += custommessage;// "Data not match on row " + countfe + ", Title must be 'Mr/Ms' for Adult and 'Chd' for Child, for Passenger [" + name + "]\n";
                            //arrayerror.Add("Title;" + (Convert.ToInt16(args.NewValues["RowNo"].ToString()) - 1));
                            //errorMessage += msgList.Err200063;;
                            //counterror += 1;
                        }
                        else if (args.NewValues["Gender"].ToString().ToUpper() != "MALE" && args.NewValues["Gender"].ToString().ToUpper() != "FEMALE")
                        {
                            custommessage = "";
                            countma = args.NewValues["RowNo"].ToString();
                            name = args.NewValues["FirstName"].ToString();
                            custommessage = msgList.Err800005.Replace("countma", countma).Replace("name", name);
                            errorMessage += custommessage;// "Data not match on row " + countma + ", Gender must be Male/Female, for Passenger [" + name + "]\n";
                            //arrayerror.Add("Gender;" + (Convert.ToInt16(args.NewValues["RowNo"].ToString()) - 1));
                            //errorMessage += msgList.Err200064;
                            //counterror += 1;
                        }
                        else
                        {
                            rowNew["Title"] = args.NewValues["Title"];
                            rowNew["Gender"] = args.NewValues["Gender"];
                            title = args.NewValues["Title"].ToString().ToLower();
                        }
                        //rowNew["FirstName"] = args.NewValues["FirstName"];
                        //rowNew["LastName"] = args.NewValues["LastName"];

                        //Remove space (20170317 - Sienny)
                        rowNew["FirstName"] = args.NewValues["FirstName"].ToString().Trim();
                        rowNew["LastName"] = args.NewValues["LastName"].ToString().Trim();

                        if (DateTime.TryParse(args.NewValues["DOB"].ToString(), out DOB))
                        {
                            DateTime stddate = bookHDRInfo.STDDate;
                            DateTime maxaddult = stddate.AddYears(-2);
                            DateTime maxchild = stddate.AddDays(-9);
                            if (title.ToString().ToLower() == "chd" || title.ToString().ToLower() == "mr" || title.ToString().ToLower() == "ms" || title.ToString().ToLower() == "mrs")
                            {
                                if (DateTime.Parse(args.NewValues["DOB"].ToString()) >= DateTime.Parse("1900-01-01"))
                                {
                                    rowNew["DOB"] = args.NewValues["DOB"].ToString();
                                }
                                else
                                {
                                    custommessage = "";
                                    countdob = args.NewValues["RowNo"].ToString();
                                    name = args.NewValues["FirstName"].ToString();
                                    custommessage = msgList.Err800006.Replace("countdob", countdob).Replace("maxaddult", String.Format("{0:d MMM yyyy}", maxaddult)).Replace("name", name);
                                    errorMessage += custommessage;// "Data not match on row " + countdob + ", Invalid DOB, DOB should be between 1 Jan 1900 and " + String.Format("{0:d MMM yyyy}", maxaddult) + ", for Passenger [" + name + "]\n";
                                    //errorMessage += "Invalid DOB, DOB should be between 1 Jan 1900 and " + String.Format("{0:d MMM yyyy}", maxaddult) + "\n";
                                    //arrayerror.Add("DOB;" + (Convert.ToInt16(args.NewValues["RowNo"].ToString()) - 1));
                                    //counterror += 1;
                                }
                            }
                        }
                        else
                        {
                            custommessage = "";
                            countdob = args.NewValues["RowNo"].ToString();
                            name = args.NewValues["FirstName"].ToString();
                            custommessage = msgList.Err800007.Replace("countdob", countdob).Replace("name", name);
                            errorMessage += custommessage;// "Data not match on row " + countdob + ", Invalid DOB, date format [yyyy-MM-dd], eg: 1999-12-31, for Passenger [" + name + "]\n";
                            //arrayerror.Add("DOB;" +  (Convert.ToInt16(args.NewValues["RowNo"].ToString()) - 1));
                            //counterror += 1;
                        }


                        if (!IsInternationalFlight) //20170322 - Sienny (check for domestic/international flight)
                        {
                            rowNew["PassportNo"] = "";
                            rowNew["ExpiryDate"] = Convert.ToDateTime(null);
                        }
                        else
                        {
                            if (args.NewValues["ExpiryDate"] != null && args.NewValues["PassportNo"].ToString() != "TBA")
                            {
                                rowNew["PassportNo"] = args.NewValues["PassportNo"];
                                rowNew["ExpiryDate"] = args.NewValues["ExpiryDate"];
                            }
                        }
                        dataNew = rowNew.Table;
                    }
                }

                if (errorMessage != string.Empty)
                {
                    gvPassenger.JSProperties["cp_result"] = errorMessage;
                }

                else
                {
                    if ((int)Session["CurStatus"] == 3)
                    {
                        Session["dtGridPass"] = data;
                    }
                    else
                    {
                        Session["dtGridPass"] = dataNew;
                    }
                }

                Session["dtNewGridPass"] = dataNew;
                e.Handled = true;

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
            finally
            {
                gvPassenger.DataSource = Session["dtNewGridPass"];
                gvPassenger.DataBind();
                gvInfant.DataSource = (DataTable)Session["dtInfant"];
                gvInfant.DataBind();

                if (Session["error"] != null)
                {
                    gvPassenger.Columns["ErrorMsg"].Visible = true;
                }
                else
                {
                    gvPassenger.Columns["ErrorMsg"].Visible = false;
                }
                if (Session["arrayerror"] != null)
                {
                    Session["arrayerror"] = null;
                }
            }
        }

        protected void gvInfant_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            try
            {
                foreach (var args in e.UpdateValues)
                {
                    int i;
                    DataTable data = Session["dtInfant"] as DataTable;
                    DataTable dtGridPass = Session["dtGridPass"] as DataTable;
                    data.PrimaryKey = new DataColumn[] { (data.Columns["PassengerID"]), (data.Columns["RecordLocator"]) };
                    object[] findTheseVals = new object[2];

                    // Set the values of the keys to find.
                    findTheseVals[0] = (args.Keys["PassengerID"]);
                    findTheseVals[1] = (args.Keys["RecordLocator"]);

                    DataRow row = data.Rows.Find(findTheseVals);
                    //i = System.Convert.ToInt32(args.Keys["PassengerID"]);

                    object[] findTheseVal = new object[2];

                    // Set the values of the keys to find.
                    findTheseVal[0] = (args.Keys["PassengerID"]);
                    findTheseVal[1] = (args.Keys["RecordLocator"]);

                    DataRow rows = data.Rows.Find(findTheseVal);
                    //DataRow rows = data.Rows.Find(args.Keys["PassengerID"]);
                    rows["PassengerID"] = args.Keys["PassengerID"];
                    //rows["RecordLocator"] = dtGridPass.Rows[0]["PNR"];

                    //20170614 - Sienny
                    DataTable dtCountryName = objGeneral.GetAllCountryCard();

                    if (args.NewValues["IssuingCountry"] != null)
                    {
                        rows["IssuingCountry"] = args.NewValues["IssuingCountry"];
                        //20170614 - Sienny
                        DataRow rowIssuingCountryName = dtCountryName.Select("CountryCode like '" + args.NewValues["IssuingCountry"].ToString() + "'").FirstOrDefault();
                        rows["IssuingCountryName"] = rowIssuingCountryName["Name"].ToString();
                    }
                    if (args.NewValues["Nationality"] != null)
                    {
                        rows["Nationality"] = args.NewValues["Nationality"];
                        //20170614 - Sienny
                        DataRow rowCountryName = dtCountryName.Select("CountryCode like '" + args.NewValues["Nationality"].ToString() + "'").FirstOrDefault();
                        rows["CountryName"] = rowCountryName["Name"].ToString();
                    }
                    if (args.NewValues["Gender"] != null)
                    {
                        rows["Gender"] = args.NewValues["Gender"];
                    }
                    rows["FirstName"] = args.NewValues["FirstName"];
                    rows["LastName"] = args.NewValues["LastName"];
                    if (args.NewValues["DOB"] != null)
                    {
                        rows["DOB"] = args.NewValues["DOB"];
                    }
                    if (args.NewValues["PassportNo"] != null)
                    {
                        rows["PassportNo"] = args.NewValues["PassportNo"];
                    }
                    if (args.NewValues["ExpiryDate"] != null)
                    {
                        rows["ExpiryDate"] = args.NewValues["ExpiryDate"];
                    }

                    Session["dtInfant"] = data;
                }

                e.Handled = true;

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
            finally
            {
                gvInfant.DataSource = Session["dtInfant"];
                gvInfant.DataBind();
            }
        }

        protected void gvPassenger_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                if (Session["infantcount"] != null && Convert.ToInt16(Session["infantcount"]) > 0)
                {
                    e.Cell.Attributes.Add("onclick", "event.cancelBubble = true");
                }
                else
                {
                    ArrayList arrayerror = new ArrayList();
                    if (Session["arrayerror"] != null)
                    {
                        arrayerror = (ArrayList)Session["arrayerror"];
                    }

                    if (arrayerror.Count > 0)
                    {
                        for (int i = 0; i < arrayerror.Count; i++)
                        {
                            string[] field = arrayerror[i].ToString().Split(';');
                            if (e.DataColumn.FieldName == field[0].ToString() && Convert.ToInt16(gvPassenger.GetRowValues(e.VisibleIndex, "RowNo")) == Convert.ToInt16(field[1]) + 1)
                            {
                                //e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#f9a2d2");
                                e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#BD081C");
                                e.Cell.ForeColor = System.Drawing.Color.White;
                                e.Cell.Font.Bold = true;
                            }
                            else if (e.DataColumn.FieldName == "ErrorMsg" && Convert.ToInt16(gvPassenger.GetRowValues(e.VisibleIndex, "RowNo")) == Convert.ToInt16(field[1]) + 1)
                            {
                                //e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#f9a2d2");
                                e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#BD081C");
                                e.Cell.ForeColor = System.Drawing.Color.White;
                                e.Cell.Font.Bold = true;
                            }
                        }
                    }
                    if ((int)Session["CurStatus"] > 2)
                    {
                        ////set to uneditable column outside FirstName and LastName especially RowNo because this column is called before CountChanged
                        //if (e.DataColumn.FieldName == "RowNo")
                        //{
                        //    e.Cell.Attributes.Add("onclick", "event.cancelBubble = true");
                        //    e.DataColumn.ReadOnly = true;
                        //}

                        if (e.DataColumn.FieldName == "CountChanged")
                        {
                            if (e.CellValue != null)
                            {
                                Session["Posted"] = e.CellValue.ToString();
                            }
                        }
                        //added by romy for insure
                        //if (e.DataColumn.FieldName == "DepartInsure")
                        //{
                        //    if (e.CellValue != null)
                        //    {
                        //        Session["InsureCode"] = e.CellValue.ToString();
                        //    }
                        //}

                        if (Session["Posted"] != null)
                        {
                            //change by tyas
                            if (decimal.Parse(Session["Posted"].ToString()) == 0 || Session["IsAllow"] == null || Session["IsAllow"].ToString() == "0" || (gvPassenger.GetRowValues(e.VisibleIndex, "DepartInsure") != null && gvPassenger.GetRowValues(e.VisibleIndex, "DepartInsure").ToString() != "")) //check if still allow to change
                            //if (Convert.ToInt32(drRow["PaxNo"]) == 0)
                            {
                                e.Cell.Attributes.Add("onclick", "event.cancelBubble = true");
                                e.DataColumn.ReadOnly = true;
                            }
                            else
                            {
                                if ((e.DataColumn.FieldName == "RowNo"))
                                //|| (e.DataColumn.FieldName == "Nationality")
                                //|| (e.DataColumn.FieldName == "IssuingCountry")
                                //|| (e.DataColumn.FieldName == "Title")
                                //|| (e.DataColumn.FieldName == "Gender")
                                //|| (e.DataColumn.FieldName == "DOB")
                                //|| (e.DataColumn.FieldName == "PassportNo")
                                //|| (e.DataColumn.FieldName == "ExpiryDate"))
                                {
                                    e.Cell.Attributes.Add("onclick", "event.cancelBubble = true");
                                    e.DataColumn.ReadOnly = true;
                                }
                                else
                                {
                                    e.Cell.Attributes.Add("onclick", "event.cancelBubble = false");
                                }
                            }
                        }
                        else
                        {
                            //Session["CurStatus"] => TransStatus value from TransMain
                            //Session["Posted"] => FirstName and LastName will be editable for name change
                            //set to unable editable outside FirstName and LastName
                            e.Cell.Attributes.Add("onclick", "event.cancelBubble = true");
                            e.DataColumn.ReadOnly = true;
                        }
                    }
                    else
                    {
                        e.Cell.Attributes.Add("onclick", "event.cancelBubble = false");
                    }

                    if (e.DataColumn.FieldName == "DepartSeat")
                        if (e.CellValue != null && gvPassenger.VisibleRowCount > 0)
                            e.Cell.ToolTip = e.DataColumn.Caption + Environment.NewLine + "Meal : " + gvPassenger.GetRowValues(e.VisibleIndex, "DepartMeal").ToString()
                                + Environment.NewLine + "Baggage : " + gvPassenger.GetRowValues(e.VisibleIndex, "DepartBaggage").ToString()
                                + Environment.NewLine + "Sport Equipment : " + gvPassenger.GetRowValues(e.VisibleIndex, "DepartSport").ToString()
                                + Environment.NewLine + "Comfort Kit : " + gvPassenger.GetRowValues(e.VisibleIndex, "DepartComfort").ToString();
                    //+ Environment.NewLine + "Duty Free : " + gvPassenger.GetRowValues(e.VisibleIndex, "DepartDuty").ToString();

                    if (e.DataColumn.FieldName == "ReturnSeat")
                        if (e.CellValue != null && gvPassenger.VisibleRowCount > 0)
                            e.Cell.ToolTip = e.DataColumn.Caption + Environment.NewLine + "Meal : " + gvPassenger.GetRowValues(e.VisibleIndex, "ReturnMeal").ToString()
                                + Environment.NewLine + "Baggage : " + gvPassenger.GetRowValues(e.VisibleIndex, "ReturnBaggage").ToString()
                                + Environment.NewLine + "Sport Equipment : " + gvPassenger.GetRowValues(e.VisibleIndex, "ReturnSport").ToString()
                                + Environment.NewLine + "Comfort Kit : " + gvPassenger.GetRowValues(e.VisibleIndex, "ReturnComfort").ToString();
                    //+ Environment.NewLine + "Duty Free : " + gvPassenger.GetRowValues(e.VisibleIndex, "ReturnDuty").ToString();

                    if (e.DataColumn.FieldName == "DepartConnectingSeat")
                        if (e.CellValue != null && gvPassenger.VisibleRowCount > 0)
                            e.Cell.ToolTip = e.DataColumn.Caption + Environment.NewLine + "Meal : " + gvPassenger.GetRowValues(e.VisibleIndex, "ConDepartMeal").ToString()
                                + Environment.NewLine + "Baggage : " + gvPassenger.GetRowValues(e.VisibleIndex, "ConDepartBaggage").ToString()
                                + Environment.NewLine + "Sport Equipment : " + gvPassenger.GetRowValues(e.VisibleIndex, "ConDepartSport").ToString()
                                + Environment.NewLine + "Comfort Kit : " + gvPassenger.GetRowValues(e.VisibleIndex, "ConDepartComfort").ToString();
                    //+ Environment.NewLine + "Duty Free : " + gvPassenger.GetRowValues(e.VisibleIndex, "ConDepartDuty").ToString();

                    if (e.DataColumn.FieldName == "ReturnConnectingSeat")
                        if (e.CellValue != null && gvPassenger.VisibleRowCount > 0)
                            e.Cell.ToolTip = e.DataColumn.Caption + Environment.NewLine + "Meal : " + gvPassenger.GetRowValues(e.VisibleIndex, "ConReturnMeal").ToString()
                                + Environment.NewLine + "Baggage : " + gvPassenger.GetRowValues(e.VisibleIndex, "ConReturnBaggage").ToString()
                                + Environment.NewLine + "Sport Equipment : " + gvPassenger.GetRowValues(e.VisibleIndex, "ConReturnSport").ToString()
                                + Environment.NewLine + "Comfort Kit : " + gvPassenger.GetRowValues(e.VisibleIndex, "ConReturnComfort").ToString();


                    //+ Environment.NewLine + "Duty Free : " + gvPassenger.GetRowValues(e.VisibleIndex, "ConReturnDuty").ToString();
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
            finally
            {

            }
        }

        protected void gvPassenger_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (Session["CurStatus"] != null && (int)Session["CurStatus"] > 2)
            {
                if ((e.Column.FieldName == "RowNo"))
                //|| (e.Column.FieldName == "Nationality")
                //|| (e.Column.FieldName == "IssuingCountry")
                //|| (e.Column.FieldName == "Title")
                //|| (e.Column.FieldName == "Gender")
                //|| (e.Column.FieldName == "DOB")
                //|| (e.Column.FieldName == "PassportNo")
                //|| (e.Column.FieldName == "ExpiryDate"))
                {
                    e.Editor.ReadOnly = true;
                    e.Column.ReadOnly = true;
                    e.Editor.Enabled = false;
                }
            }
        }

        protected void gvInfant_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                if ((e.DataColumn.FieldName == "PaxNo")
                    || (e.DataColumn.FieldName == "ParentFirstName")
                    || (e.DataColumn.FieldName == "ParentLastName"))
                {
                    e.Cell.Attributes.Add("onclick", "event.cancelBubble = true");
                    e.DataColumn.ReadOnly = true;
                }

                //if ((int)Session["CurStatus"] > 2)
                //{
                //    e.Cell.Attributes.Add("onclick", "event.cancelBubble = true");
                //    e.DataColumn.ReadOnly = true;
                //}
            }

            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
            finally
            {

            }
        }

        protected void resendItinerary()
        {
            lstbookDTLInfo = objBooking.GetAllBK_TRANSDTLFilterAll(TransID, 0);
            foreach (BookingTransactionDetail bookDTL in lstbookDTLInfo)
            {
                SendItineraryResponse respSI = absNavitaire.SendItineraryByPNR(bookDTL.RecordLocator);
            }
        }


        #endregion

        //protected void gvPassengerNameChange_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        //{
        //    //try
        //    //{
        //    //    if ((int)Session["CurStatus"] > 2)
        //    //    {
        //    //        if ((e.Column.FieldName == "Nationality")
        //    //                    || (e.Column.FieldName == "IssuingCountry")
        //    //                    || (e.Column.FieldName == "Title")
        //    //                    || (e.Column.FieldName == "Gender")
        //    //                    || (e.Column.FieldName == "DOB")
        //    //                    || (e.Column.FieldName == "PassportNo")
        //    //                    || (e.Column.FieldName == "ExpiryDate"))
        //    //        {
        //    //            e.Editor.ReadOnly = true;
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        e.Editor.ReadOnly = false;
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{

        //    //}
        //    //finally
        //    //{

        //    //}
        //}
    }
}