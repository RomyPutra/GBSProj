using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using DevExpress.Web;

//using log4net;
using SEAL.Data;

using ABS.Logic.GroupBooking.Agent;
using ABS.Logic.GroupBooking.Booking;
using ABS.Logic.GroupBooking;
using ABS.Logic.Shared;
using SEAL.WEB;

using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Globalization;
using System.Configuration;
using ABS.GBS.Log;
using StackExchange.Profiling;

namespace GroupBooking.Web
{
    public partial class SearchFlight : System.Web.UI.Page
    {
        #region declaration
        UserSet MyUserSet;
        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        ABS.Logic.GroupBooking.Agent.AgentControl objAgent = new ABS.Logic.GroupBooking.Agent.AgentControl();
        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();

        ABS.Logic.GroupBooking.BookingMGR objBookingMGR = new ABS.Logic.GroupBooking.BookingMGR();

        BookingEnquiry enqLogInfo = new BookingEnquiry();
        BookingSuspendList enqSusInfo = new BookingSuspendList();
        AgentBlacklistLog agBlacklistInfo = new AgentBlacklistLog();
        AgentActivity agActivityInfo = new AgentActivity();
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogControl log = new LogControl();
        SystemLog SystemLog = new SystemLog();
        Settings newSys_Preft = new Settings();
        List<CODEMASTER> lstOpt = new List<CODEMASTER>();
        //added by Tyas 22/06/2015
        string bookingFrom = "";
        string bookingTo = "";
        string travelFrom = "";
        string travelTo = "";
        Boolean Active = false;
        //end edded

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            string result = string.Empty;
            var profiler = MiniProfiler.Current;

            //if (Application["appLocation"] != null)
            //    lbltest.Text = Application["appLocation"].ToString();
            //else
            //    lbltest.Text = "~O_O~";

            using (profiler.Step("UserAgentSet"))
            {
                if (Session["AgentSet"] != null)
                { MyUserSet = (UserSet)Session["AgentSet"]; }
                else
                {
                    if (IsCallback)
                        ASPxCallback.RedirectOnCallback(GroupBooking.Web.Shared.MySite.PublicPages.SessionExpired);
                    else
                        Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.SessionExpired);
                }
            }
            
            if (IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "PostbackKey", "<script type='text/javascript'>var isPostBack = true;</script>");
            }
            else
            {
                using (profiler.Step("ClearSession"))
                {
                    ClearSession();
                }
                //added by diana 20131107 - to clear journey
                objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                using (profiler.Step("ClearExpiredJourney"))
                {
                    objBooking.ClearExpiredJourney(MyUserSet.AgentID);
                }

                //using (profiler.Step("Cancel10MntBook"))
                //{
                //    Cancel10MntBook();
                //}
                using (profiler.Step("RegisterStartupScript"))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "PostbackKey", "<script type='text/javascript'>var isPostBack = false;</script>");
                }
                ////InitializeForm();
                lblAgentName.Text = MyUserSet.AgentName;
                lblAgentEmail.Text = MyUserSet.Email;
                lblContact.Text = MyUserSet.Contact;
                if (Session["OrganizationName"] != null)
                {
                    lblAgentOrg.Text = Session["OrganizationName"].ToString();
                }
                if ( MyUserSet.Currency != null && MyUserSet.Currency != "")
                {
                    lblAGCurr.Text = MyUserSet.Currency;
                    Session["AGLimit"] = null;

                    using (profiler.Step("GetAGCredit"))
                    {
                        GetAGCredit(MyUserSet.Currency);
                    }
                    
                }
                if (Session["AGLimit"] != null && Session["AGLimit"].ToString() != "'")
                {
                    lblAGLimit.Text = Session["AGLimit"].ToString();
                }
                else
                    lblAGLimit.Text = "0";
            }

            using (profiler.Step("PaxStatus"))
            {
                if (Session["PaxStatus"] != null) //added by diana 20140127 - to check for TotalPax of Saving<> TotalPax of Booking
                {
                    if (Session["PaxStatus"].ToString() == "false")
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "Alert", "<script type='text/javascript'>window.alert('Total Seat is not enough for your booking pax. Kindly rebook the flight.');</script>");
                        Session["PaxStatus"] = "";
                    }
                }
            }
            
        }
        
        //addded by agus
        // not yet used because we need PNR to cancel, and the clear function cannot be done even throught with same signature as request seat

        private void Cancel10MntBook()
        {
            return;
            List<BookingTransactionMain> transmainInfo;
            BookingControl objBookingControl = new BookingControl();
            transmainInfo = objBookingControl.GetBK_TRANSMAIN10(0, Convert.ToInt32(MyUserSet.AgentID));
            ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");
            return;
            if(transmainInfo != null && transmainInfo.Count > 0)
                foreach (BookingTransactionMain btm in transmainInfo)
                {
                    List<BookingTransactionDetail> detailDatas = new List<BookingTransactionDetail>();

                    detailDatas = objBooking.BookingDetailFilter(btm.TransID);
                    string errMessage = "";
                    int intError = 0;
                    foreach (BookingTransactionDetail detail in detailDatas)
                    {
                        
                        string signature = absNavitaire.AgentLogon();
                        absNavitaire.CancelJourney(detail.RecordLocator, -detail.CollectedAmount, detail.Currency, signature, ref errMessage); //cancel journey to api
                        if (errMessage == "")
                        {
                            objBooking.CancelTransaction(btm.TransID, btm.AgentID, ref intError, ref errMessage);
                        }
                        else
                        {
                            break;
                        }

                    }
                }
            int a = 0213123;
        }

        private void ClearSession()
        {
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
            HttpContext.Current.Session.Remove("dtSpor");
            HttpContext.Current.Session.Remove("dtComfort");
            HttpContext.Current.Session.Remove("dtDuty");
            HttpContext.Current.Session.Remove("dtGridPass");
            HttpContext.Current.Session.Remove("dtGridPass2");
            Session["IsNew"] = "true";
            //Add-on
            HttpContext.Current.Session.Remove("PaxStatus");
            HttpContext.Current.Session.Remove("dtBaggageDepart");
            HttpContext.Current.Session.Remove("dtBaggageReturn");
            HttpContext.Current.Session.Remove("dtSportReturn");
            HttpContext.Current.Session.Remove("dtSportReturn");
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
            HttpContext.Current.Session.Remove("qtyComfort");
            HttpContext.Current.Session.Remove("qtyDuty");
            HttpContext.Current.Session.Remove("qtyMeal2");
            HttpContext.Current.Session.Remove("qtyBaggage2");
            HttpContext.Current.Session.Remove("qtySport2");
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

        private void ClearCityPair()
        {
            HttpContext.Current.Session.Remove("CityPairDepart");
            HttpContext.Current.Session.Remove("CityPairAll");
        }

        public static bool IsNumeric(string s)
        {
            double result;
            return s != null && Double.TryParse(s, out result);
        }

        public void GetAGCredit(string cur)
        {
            ABS.Navitaire.APIBooking apiNavitaire = new ABS.Navitaire.APIBooking("");
            GeneralControl objGeneral = new GeneralControl();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            var profiler = MiniProfiler.Current;
            try
            {
                string psDomain = ConfigurationManager.AppSettings["signature_domain"].ToString();
                string psName = ConfigurationManager.AppSettings["signature_username"].ToString();
                string psPwd = ConfigurationManager.AppSettings["signature_password"].ToString();

                string signature = "";// apiNavitaire.AgentLogon("public", psDomain, psName, psPwd);
                using (profiler.Step("AgentLogon:Navitaire"))
                {
                    signature = apiNavitaire.AgentLogon("public", psDomain, psName, psPwd);
                }
                //ABS.Navitaire.AgentManager.FindAgentsResponse agentresp = absNavitaire.FindingAgent(MyUserSet.AgentName, signature);
                //string accountReference = agentresp.FindAgentResponseData.FindAgentList[0].AgentIdentifier.OrganizationCode;
                string accountReference = Session["OrganizationCode"].ToString();

                //added by romy for optimize
                ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse accResp = new ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse();
                if (Session["AccResp"] == null)
                {
                    using (profiler.Step("Navitaire:GetCredit"))
                    {
                        accResp = apiNavitaire.GetCreditByAccountReference(accountReference, cur, signature);
                    }
                    Session["AccResp"] = accResp;
                    if (accResp != null)
                    {
                        if (accResp.AvailableCreditResponse.Account != null)
                        {
                            Session["AGLimit"] = objGeneral.RoundUp(accResp.AvailableCreditResponse.Account.ForeignAmount).ToString("N", nfi);
                        }
                        else
                        {
                            Session["AGLimit"] = 0;
                        }
                    }
                    //return accResp;
                }
                else
                {
                    accResp = (ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse)Session["AccResp"];
                    Session["AGLimit"] = objGeneral.RoundUp(accResp.AvailableCreditResponse.Account.ForeignAmount).ToString("N", nfi);
                }
                //ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse accResp = apiNavitaire.GetCreditByAccountReference(accountReference, cur, signature);
                //if (accResp != null)
                //{
                //    if (accResp.AvailableCreditResponse.Account != null)
                //    {
                //        Session["AGLimit"] = objGeneral.RoundUp(accResp.AvailableCreditResponse.Account.ForeignAmount).ToString("N", nfi);
                //    }
                //    else
                //    {
                //        Session["AGLimit"] = 0;
                //    }
                //}
                ////return accResp;

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //return null;
            }
            finally
            {
                apiNavitaire = null;
                objGeneral = null;
                nfi = null;
            }
        }

        //protected void ValidatePopup(object sender, CallbackEventArgs e)
        //{
        //    MessageList msgList = new MessageList();
        //    ArrayList aMsgList = new ArrayList();

        //    try
        //    {
        //        bool ifok = true;
        //        HttpContext.Current.Session.Remove("invalidreturnflight");
        //        if (objBooking.GetTotalSuspend((MyUserSet.AgentID)) >= Convert.ToInt16(MyUserSet.MaxSuspend))
        //        {
        //            e.Result = msgList.Err100012;
        //        }
        //        else
        //        {

        //            if (ddlDeparture.SelectedIndex == 0 || ddlReturn.SelectedIndex == 0)
        //            {
        //                ifok = false;
        //                e.Result = msgList.Err100017;
        //            }
        //            DateTime DepartDate = new DateTime();
        //            try
        //            {
        //                //DepartDate = Convert.ToDateTime(ddl_DepartYear.SelectedItem.Text + "-" + ddl_DepartMonth.SelectedValue + "-" + ddl_DepartDay.SelectedItem.Text.PadLeft(2, '0'));
        //                //DepartDate = Convert.ToDateTime(Request.Form["ddlMarketMonth1"].ToString() + "-" + Request.Form["ddlMarketDay1"].ToString().PadLeft(2, '0'));
        //                DepartDate = Convert.ToDateTime(daStart.Value);
        //                hDepart.Value = DepartDate.ToString(); //amended by diana 20140128, moved from page load
        //            }
        //            catch
        //            {
        //                ifok = false;
        //                e.Result = msgList.Err100002;

        //            }
        //            DateTime ReturnDate = new DateTime();
        //            if (DepartDate.AddDays(2) < DateTime.Now && ifok == true)
        //            {
        //                ifok = false;
        //                e.Result = msgList.Err100002;

        //            }
        //            if (cb_OneWay.Checked == false)
        //            {
        //                try
        //                {
        //                    //ReturnDate = Convert.ToDateTime(Request.Form["ddlMarketMonth2"].ToString() + "-" + Request.Form["ddlMarketDay2"].ToString().PadLeft(2, '0'));
        //                    ReturnDate = Convert.ToDateTime(daEnd.Value);
        //                    hReturn.Value = ReturnDate.ToString(); //amended by diana 20140128, moved from page load

        //                    ////added by ketee
        //                    //added by ketee, block travel period from : 1 March - 31 Oct 2015(425 days)
        //                    //added by ketee, block travel period from : 10 Jun 2015 - 17 JAN 2016(425 days)
        //                    //if (ReturnDate >= Convert.ToDateTime("2015-06-10") && ReturnDate <= Convert.ToDateTime("2016-01-17"))
        //                    //{
        //                    //    ifok = false;
        //                    //    e.Result = msgList.Err100050;
        //                    //}
        //                    //added by Tyas 13/08/2015
        //                    Restriction();

        //                    if (DateTime.Now >= Convert.ToDateTime(bookingFrom) && DateTime.Now <= Convert.ToDateTime(bookingTo) && Active == true)
        //                    {
        //                        if (ReturnDate >= Convert.ToDateTime(travelFrom) && ReturnDate <= Convert.ToDateTime(travelTo))
        //                        {
        //                            newSys_Preft = objGeneral.GetSingleSYS_PREFT(1, "AA", "RESTRICTIONALERT");
        //                            ifok = false;
        //                            e.Result = newSys_Preft.SYSValue;
        //                        }
        //                    }
        //                    //remark by Tyas 19/06/2015
        //                    //if (System.Configuration.ConfigurationManager.AppSettings["RestrictionActive"] != null && System.Configuration.ConfigurationManager.AppSettings["RestrictionActive"].ToString() == "1")
        //                    //{
        //                    //    DateTime bookingFrom = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["BookingFrom"].ToString());
        //                    //    DateTime bookingTo = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["BookingTo"].ToString());
        //                    //    DateTime travelFrom = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["TravelFrom"].ToString());
        //                    //    DateTime travelTo = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["TravelTo"].ToString());
        //                    //    string restrictionMsg = System.Configuration.ConfigurationManager.AppSettings["RestrictionMsg"].ToString();


        //                    //}

        //                    ////ReturnDate = Convert.ToDateTime(ddl_ReturnYear.SelectedItem.Text + "-" + ddl_ReturnMonth.SelectedValue + "-" + ddl_ReturnDay.SelectedItem.Text.PadLeft(2, '0'));
        //                    //if (DepartDate > ReturnDate)
        //                    //{
        //                    //    ifok = false;
        //                    //    e.Result = msgList.Err100003;

        //                    //}
        //                }
        //                catch
        //                {
        //                    ifok = false;
        //                    e.Result = msgList.Err100004;
        //                }
        //            }
        //            else
        //            {
        //                try
        //                {
        //                    Restriction();

        //                    if (DateTime.Now >= Convert.ToDateTime(bookingFrom) && DateTime.Now <= Convert.ToDateTime(bookingTo) && Active == true)
        //                    {
        //                        if (DepartDate >= Convert.ToDateTime(travelFrom) && DepartDate <= Convert.ToDateTime(travelTo))
        //                        {
        //                            newSys_Preft = objGeneral.GetSingleSYS_PREFT(1, "AA", "RESTRICTIONALERT");
        //                            ifok = false;
        //                            e.Result = newSys_Preft.SYSValue;
        //                        }
        //                    }
                            
        //                }
        //                catch
        //                {
        //                    ifok = false;
        //                    e.Result = msgList.Err100004;
        //                }
        //            }

        //            int num = 0;

        //            if (txt_GuestNum.Text == "" )
        //            {
        //                ifok = false;
        //                e.Result = msgList.Err100005;
        //            }

        //            if (IsNumeric(txt_GuestNum.Text) == false || (txt_ChildNum.Text != "" && IsNumeric(txt_ChildNum.Text) == false) )
        //            {
        //                ifok = false;
        //                e.Result = msgList.Err100006;
        //            }

        //            //added by ketee, block travel period from : 5 Jan - 31 July 2015(425 days)
        //            //added by ketee, block travel period from : 1 March - 31 Oct 2015(425 days)
        //            //added by ketee, block travel period from : 10 Jun 2015 - 17 JAN 2016(425 days)
        //            //added by Tyas 22/06/2015
        //            //lstOpt = objGeneral.GetAllCODEMASTERFilterCode("RST");
        //            //for (int i = 0; i < lstOpt.Count; i++)
        //            //{
        //            //    CODEMASTER code = new CODEMASTER();
        //            //    code = lstOpt[i];
        //            //    if (code.Code == "BOOKFROM")
        //            //    {
        //            //       bookingFrom = DateTime.ParseExact(code.CodeDesc, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        //            //        //Session["bookingFrom"] = bookingFrom;
        //            //    }
        //            //    else if (code.Code == "BOOKTO")
        //            //    {
        //            //        bookingTo = DateTime.ParseExact(code.CodeDesc, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        //            //        //Session["bookingTo"] = bookingTo;
        //            //    }
        //            //    else if (code.Code == "TRAFROM")
        //            //    {
        //            //        travelFrom = DateTime.ParseExact(code.CodeDesc, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        //            //        //Session["travelFrom"] = travelFrom;
        //            //    }
        //            //    else if (code.Code == "TRATO")
        //            //    {
        //            //        travelTo = DateTime.ParseExact(code.CodeDesc, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        //            //        //Session["travelTo"] = travelTo;
        //            //    }
        //            //    else if (code.Code == "IND")
        //            //    {
        //            //        if (code.CodeDesc == "1")
        //            //        {
        //            //            Active = true;
        //            //            //Session["Active"] = Active;
        //            //        }
        //            //        else
        //            //        {
        //            //            Active = false;
        //            //        }
        //            //    }

        //            //}
        //            //added by Tyas 13/08/2015
        //            Restriction();

        //            if (DateTime.Now >= Convert.ToDateTime(bookingFrom) && DateTime.Now <= Convert.ToDateTime(bookingTo) && Active == true)
        //            {
        //                if (ReturnDate >= Convert.ToDateTime(travelFrom) && ReturnDate <= Convert.ToDateTime(travelTo))
        //                {
        //                    newSys_Preft = objGeneral.GetSingleSYS_PREFT(1, "AA", "RESTRICTIONALERT");
        //                    ifok = false;
        //                    e.Result = newSys_Preft.SYSValue;
        //                }
        //            }
        //            //remark by Tyas 19/06/2015
        //            //if ( System.Configuration.ConfigurationManager.AppSettings["RestrictionActive"] != null && System.Configuration.ConfigurationManager.AppSettings["RestrictionActive"].ToString() == "1")
        //            //{
        //            //    DateTime bookingFrom = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["BookingFrom"].ToString());
        //            //    DateTime bookingTo = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["BookingTo"].ToString());
        //            //    DateTime travelFrom = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["TravelFrom"].ToString());
        //            //    DateTime travelTo = Convert.ToDateTime( System.Configuration.ConfigurationManager.AppSettings["TravelTo"].ToString());
        //            //    string restrictionMsg = System.Configuration.ConfigurationManager.AppSettings["RestrictionMsg"].ToString();

        //            //    if (DateTime.Now >= bookingFrom && DateTime.Now <= bookingTo)
        //            //    {
        //            //        if (DepartDate >= travelFrom && DepartDate <= travelTo)
        //            //        {
        //            //            ifok = false;
        //            //            e.Result = restrictionMsg;
        //            //        }
        //            //    }
        //            //}
        //            //if (DepartDate >= Convert.ToDateTime("2015-06-10") && DepartDate <= Convert.ToDateTime("2016-01-17"))
        //            //{
        //            //    ifok = false;
        //            //    e.Result = msgList.Err100050;
        //            //}

        //            DataTable dtRoute = new DataTable();
        //            dtRoute = objBooking.GetAllSECTORSUSPEND(MyUserSet.OperationGroup, MyUserSet.AgentCategoryID, ddlDeparture.SelectedItem.Value.ToString(), ddlReturn.SelectedItem.Value.ToString(), DepartDate.Date);
        //            if (dtRoute != null)
        //            {
        //                ifok = false;
        //                e.Result = msgList.Err100038;
        //            }
        //            dtRoute = null;


        //            int ChildNum = 0;
        //            if (txt_ChildNum.Text != "")
        //            {
        //                ChildNum = Convert.ToInt32(txt_ChildNum.Text);
        //            }
        //            if (ifok)
        //            {
        //                if (Convert.ToInt32(txt_GuestNum.Text) == 0)
        //                {
        //                    ifok = false;
        //                    e.Result = msgList.Err100027;
        //                }

        //                if (ddl_Currency.SelectedItem.Text == "")
        //                {
        //                    ifok = false;
        //                    e.Result = msgList.Err100018;
        //                    return;
        //                }
        //                else
        //                {
        //                    num = Convert.ToInt32(txt_GuestNum.Text) + ChildNum;
        //                }
        //                if (num < 10 || num > 50)
        //                {
        //                    ifok = false;
        //                    e.Result = msgList.Err100006;
        //                }
        //                else if (Convert.ToInt32(txt_GuestNum.Text) > 50)
        //                {
        //                    ifok = false;
        //                    e.Result = msgList.Err100007;
        //                }
        //                else if (ChildNum > 50)
        //                {
        //                    ifok = false;
        //                    e.Result = msgList.Err100008;
        //                }
        //            }

                    
        //            if (ifok)
        //            {
        //                Session["TotalPax"] = num;
        //                if (Request.Cookies["cookieSearchcondition"] != null)
        //                {
        //                    HttpCookie cookieTemp = Request.Cookies["cookieSearchcondition"];
        //                    cookieTemp.HttpOnly = true;
        //                    if (cookieTemp != null)
        //                    {
        //                        cookieTemp.Expires = DateTime.Today.AddDays(-1);
        //                        Response.Cookies.Add(cookieTemp);
        //                    }
        //                }

        //                HttpCookie cookie = new HttpCookie("cookieSearchcondition");
        //                cookie.HttpOnly = true;
        //                //cookie.Values.Add("Carrier", ddl_Carrier.SelectedItem.Text);                       

        //                cookie.Values.Add("Departure", ddlDeparture.SelectedValue);
        //                cookie.Values.Add("Arrival", ddlReturn.SelectedValue);
        //                cookie.Values.Add("ifOneWay", cb_OneWay.Checked.ToString().ToUpper());
        //                cookie.Values.Add("Currency", ddl_Currency.SelectedItem.Text);
        //                cookie.Values.Add("DepartDate", DepartDate.ToString());
        //                cookie.Values.Add("ReturnDate", ReturnDate.ToString());
        //                cookie.Values.Add("PaxNum", num.ToString());
        //                cookie.Values.Add("GuestNum", txt_GuestNum.Text);
        //                cookie.Values.Add("ChildNum", ChildNum.ToString());
        //                cookie.Values.Add("DepartureDetail", ddlDeparture.SelectedItem.Text);
        //                cookie.Values.Add("ArrivalDetail", ddlReturn.SelectedItem.Text);
        //                Response.AppendCookie(cookie);

        //                //check available flight

        //                ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition model = new ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition();

        //                model.Carrier = cookie.Values["Carrier"];//   ViewState["Carrier"].ToString();
        //                model.Departure = cookie.Values["Departure"];// ViewState["Departure"].ToString();
        //                model.Arrival = cookie.Values["Arrival"];// ViewState["Arrival"].ToString();
        //                bool oneway = false;
        //                if (cookie.Values["ifOneWay"] == "TRUE")
        //                {
        //                    oneway = true;
        //                }
        //                else
        //                {
        //                    model.ReturnDate = Convert.ToDateTime(cookie.Values["ReturnDate"]);
        //                }
        //                model.ifOneWay = oneway;//  Convert.ToBoolean(ViewState["ifOneWay"]);
        //                model.Currency = cookie.Values["Currency"];// ViewState["Currency"].ToString();
        //                model.DepartDate = Convert.ToDateTime(cookie.Values["DepartDate"]);// Convert.ToDateTime(ViewState["DepartDate"]);
        //                model.ReturnDate = Convert.ToDateTime(cookie.Values["ReturnDate"]);//  Convert.ToDateTime(ViewState["ReturnDate"]);
        //                model.GuestNum = Convert.ToInt32(cookie.Values["GuestNum"]);//  Convert.ToInt32(ViewState["GuestNum"]);
        //                model.PaxNum = Convert.ToInt32(cookie.Values["PaxNum"]);
        //                model.ChildNum = Convert.ToInt32(cookie.Values["ChildNum"]);

        //                //insert temFlight into session

        //                objBooking.tempFlight(model, MyUserSet.AgentName, "");

        //                DataList dtModel1 = new DataList();
        //                DataList dtModel2 = new DataList();

        //                DataTable tempDt = new DataTable();
        //                tempDt = objBooking.dtFlight();

        //                //added by ketee, validate valid flight
        //                if (HttpContext.Current.Session["invalidreturnflight"] != null && HttpContext.Current.Session["invalidreturnflight"].ToString() == "1")
        //                {
        //                    ifok = false;
        //                    e.Result = msgList.Err100041;
        //                }

        //                if (ifok)
        //                {
        //                    if (oneway == false)
        //                    {
        //                        if (HttpContext.Current.Session["tempFlight"] != null)
        //                        {
        //                            //
        //                            string strExpr;
        //                            string strSort;
        //                            DataTable dt = new DataTable();

        //                            strExpr = "TemFlightDeparture = '" + model.Departure + "' AND TemFlightArrival = '" + model.Arrival + "' AND TemFlightAgentName = '" + MyUserSet.LoginName + "' AND TemFlightIfReturn = false";

        //                            strSort = "";
        //                            // Use the Select method to find all rows matching the filter.

        //                            dt = (DataTable)HttpContext.Current.Session["TempFlight"];

        //                            DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

        //                            tempDt.Clear();

        //                            foreach (DataRow row in foundRows)
        //                            {
        //                                tempDt.ImportRow(row);
        //                            }

        //                            dtModel1.DataSource = tempDt;
        //                            dtModel1.DataBind();

        //                            strExpr = "TemFlightDeparture = '" + model.Arrival + "' AND TemFlightArrival = '" + model.Departure + "' AND TemFlightAgentName = '" + MyUserSet.LoginName + "' AND TemFlightIfReturn = true";

        //                            strSort = "";

        //                            foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

        //                            tempDt.Clear();
        //                            foreach (DataRow row in foundRows)
        //                            {
        //                                tempDt.ImportRow(row);
        //                            }

        //                            dtModel2.DataSource = tempDt;
        //                            dtModel2.DataBind();
        //                            if (dtModel1.Items.Count <= 0 || dtModel2.Items.Count <= 0)
        //                            {
        //                                ifok = false;
        //                                e.Result = msgList.Err100013;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            ifok = false;
        //                            e.Result = msgList.Err100013;
        //                        }  //end

        //                    }
        //                    else
        //                    {

        //                        //johan remark
        //                        if (HttpContext.Current.Session["tempFlight"] != null)
        //                        {
        //                            string strExpr;
        //                            string strSort;
        //                            DataTable dt = new DataTable();

        //                            strExpr = "TemFlightDeparture = '" + model.Departure + "' AND TemFlightArrival = '" + model.Arrival + "' AND TemFlightAgentName = '" + MyUserSet.LoginName + "' AND TemFlightIfReturn = false";

        //                            strSort = "";
        //                            // Use the Select method to find all rows matching the filter.

        //                            dt = (DataTable)HttpContext.Current.Session["TempFlight"];

        //                            DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

        //                            tempDt.Clear();

        //                            foreach (DataRow row in foundRows)
        //                            {
        //                                tempDt.ImportRow(row);
        //                            }
        //                            //

        //                            //dtModel1.DataSource = (DataTable)HttpContext.Current.Session["tempFlightDepart"];
        //                            dtModel1.DataSource = tempDt;
        //                            dtModel1.DataBind();

        //                            if (dtModel1.Items.Count <= 0)
        //                            {
        //                                ifok = false;
        //                                e.Result = msgList.Err100013;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            ifok = false;
        //                            e.Result = msgList.Err100013;
        //                        }

        //                    }
        //                }

        //                if (ifok)
        //                {
        //                    //CheckSuspend
        //                    if (!CheckSuspend())
        //                    {
        //                        //AddEnquiry
        //                        if (AddEnquiry() == 1)
        //                        {
        //                            //blacklist

        //                            e.Result = msgList.Err100012;

        //                        }
        //                        else
        //                        {
        //                            RefreshPaymentSession();
        //                            e.Result = "";
        //                        }
        //                        //Response.Redirect("selectflight.aspx");
        //                    }
        //                    else
        //                    {
        //                        e.Result = msgList.Err100010;
        //                    }

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        e.Result = msgList.Err100031;
        //        log.Error(this,ex);
        //        //Response.Redirect(Shared.MySite.PublicPages.AgentMain, false);
        //    }
        //}

        //added by Tyas 13/08/2015
        private void Restriction()
        {
            lstOpt = objGeneral.GetAllCODEMASTERFilterCode("RST");
            for (int i = 0; i < lstOpt.Count; i++)
            {
                CODEMASTER code = new CODEMASTER();
                code = lstOpt[i];
                if (code.Code == "BOOKFROM")
                {
                    bookingFrom = DateTime.ParseExact(code.CodeDesc, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm:ss");
                    //Session["bookingFrom"] = bookingFrom;
                }
                else if (code.Code == "BOOKTO")
                {
                    bookingTo = DateTime.ParseExact(code.CodeDesc + " " + "23:59:59", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm:ss");
                    //Session["bookingTo"] = bookingTo;
                }
                else if (code.Code == "TRAFROM")
                {
                    travelFrom = DateTime.ParseExact(code.CodeDesc, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm:ss");
                    //Session["travelFrom"] = travelFrom;
                }
                else if (code.Code == "TRATO")
                {
                    travelTo = DateTime.ParseExact(code.CodeDesc + " " + "23:59:59", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm:ss");
                    //Session["travelTo"] = travelTo;
                }
                else if (code.Code == "IND")
                {
                    if (code.CodeDesc == "1")
                    {
                        Active = true;
                        //Session["Active"] = Active;
                    }
                    else
                    {
                        Active = false;
                        Session["Active"] = false;
                    }
                }

            }
        }

        private void RefreshPaymentSession()
        {
            HttpContext.Current.Session.Remove("ErrorPayment");
        }

        //private void InitializeForm()
        //{
        //    HttpCookie cookieLogin = Request.Cookies["cookieLoginName"];

        //    initializeData();
        //    HttpCookie cookie = Request.Cookies["cookieSearchcondition"];
        //    if (cookie != null)
        //    {
        //        cookie.Expires = DateTime.Today.AddDays(-1);
        //        Response.Cookies.Add(cookie);
        //    }
        //    ClearSession();
        //    lstOpt = objGeneral.GetAllCODEMASTERFilterCode("RST");
        //    for (int i = 0; i < lstOpt.Count; i++)
        //    {
        //        CODEMASTER code = new CODEMASTER();
        //        code = lstOpt[i];
        //        if (code.Code == "BOOKFROM")
        //        {
        //            bookingFrom = DateTime.ParseExact(code.CodeDesc, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm:ss");
        //            //Session["bookingFrom"] = bookingFrom;
        //        }
        //        else if (code.Code == "BOOKTO")
        //        {
        //            bookingTo = DateTime.ParseExact(code.CodeDesc + " " + "23:59:59", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm:ss");
        //            //Session["bookingTo"] = bookingTo;
        //        }
        //        else if (code.Code == "TRAFROM")
        //        {
        //            travelFrom = DateTime.ParseExact(code.CodeDesc, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm:ss");
        //            //Session["travelFrom"] = travelFrom;
        //        }
        //        else if (code.Code == "TRATO")
        //        {
        //            travelTo = DateTime.ParseExact(code.CodeDesc + " " + "23:59:59", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm:ss");
        //            //Session["travelTo"] = travelTo;
        //        }
        //        else if (code.Code == "IND")
        //        {
        //             if (code.CodeDesc == "1")
        //             {
        //                Active = true;
        //                                //Session["Active"] = Active;
        //             }
        //             else
        //             {
        //                Active = false;
                                        
        //             }
        //         }
                                
        //    }
        //    if (Active == true)
        //    {
        //        if (DateTime.Now >= Convert.ToDateTime(bookingFrom) && DateTime.Now <= Convert.ToDateTime(bookingTo))
        //        {
        //            newSys_Preft = objGeneral.GetSingleSYS_PREFT(1, "AA", "RESTRICTIONNOTE");
        //            divRestriction.Style.Add("display", "block");
        //            divRestriction.InnerHtml = newSys_Preft.SYSValue;
        //            divRestriction.Visible = true;
        //                 //if (DateTime.Now >= Convert.ToDateTime(travelFrom) && DateTime.Now <= Convert.ToDateTime(travelTo))
        //                 //{
        //                 //    newSys_Preft = objGeneral.GetSingleSYS_PREFT(1, "AA", "RESTRICTIONNOTE");
        //                 //    divRestriction.Style.Add("display", "block");
        //                 //    divRestriction.InnerHtml = newSys_Preft.SYSValue;
        //                 //    divRestriction.Visible = true;
        //                 //}
        //        }
        //        else
        //        {
        //                 divRestriction.Visible = false;
        //        }
               
        //    }
        //    else
        //    {
        //        divRestriction.Visible = false;
        //    }
                            
                
               
            
            
        //    //if (System.Configuration.ConfigurationManager.AppSettings["RestrictionActive"] != null && System.Configuration.ConfigurationManager.AppSettings["RestrictionActive"].ToString() == "1")
        //    //{
        //    //    string restrictionTitle = System.Configuration.ConfigurationManager.AppSettings["RestrictionTitle"].ToString();
        //    //    divRestriction.Style.Add("display", "block");
        //    //    divRestriction.InnerHtml = restrictionTitle;
        //    //    divRestriction.Visible = true;
        //    //}
        //    //else
        //    //{
        //    //    divRestriction.Visible = false;
        //    //}
        //    //ClearCityPair();
        //}

        //private string ConvTwoDigitDate(string date)
        //{
        //    if (date.Length == 1) { date = "0" + date; }
        //    return date;
        //}

        //private void initializeData()
        //{
        //    //newSys_Preft = objGeneral.GetSingleSYS_PREFT(1, "AA", "RESTRICTIONNOTE");
        //    //lblRestriction.Text = newSys_Preft.SYSValue

        //    txt_GuestNum.Text = "";
        //    txt_ChildNum.Text = "";
        //    txt_InfantNum.Text = "";

        //    string monthNumeric = "";
        //    string monthString = "";
        //    string year = "";
        //    DateTime dateNow = DateTime.Now.AddHours(2); //change to be available before 2hrs flights
        //    DateTime tempDate = new DateTime();
        //    DateTime tempDate2 = new DateTime();
        //    divDate1.InnerHtml = "";
        //    divDate2.InnerHtml = "";

        //    daStart.MinDate = dateNow;

        //    #region initdatedepart
        //    divDate1.InnerHtml += "<table><tr><td><select id=\"ddlMarketDay1\" name=\"ddlMarketDay1\" >";
            
        //    for (int i = 1; i <= 31; i++)
        //    {
        //        if (i == dateNow.Day)
        //        {
        //            divDate1.InnerHtml += "<option selected value=" + ConvTwoDigitDate(i.ToString()) + ">" + ConvTwoDigitDate(i.ToString()) + "</option>";
        //        }
        //        else
        //        {
        //            divDate1.InnerHtml += "<option value=" + ConvTwoDigitDate(i.ToString()) + ">" + ConvTwoDigitDate(i.ToString()) + "</option>";
        //        }
        //        //divDate1.InnerHtml += "<option value=" + ConvTwoDigitDate(i.ToString()) + ">" + ConvTwoDigitDate(i.ToString()) + "</option>";
        //    }
        //    divDate1.InnerHtml += "</select></td>";
        //    divDate1.InnerHtml += "<td>&nbsp;</td><td><select class=\"wLrg2s\" id=\"ddlMarketMonth1\" name=\"ddlMarketMonth1\">";

            
        //    for (int i = 0; i <= 13; i++)
        //    {
        //        tempDate = dateNow.AddMonths(i);
        //        monthNumeric = tempDate.ToString("MM");
        //        monthString = tempDate.ToString("MMM");
        //        year = tempDate.ToString("yyyy");
        //        string val = year + "-" + monthNumeric;
        //        string disp = monthString + " " + year;
        //        divDate1.InnerHtml += "<option value=\"" + val + "\">" + disp + "</option>";
        //    }
        //    divDate1.InnerHtml += "</select></td>";
        //    divDate1.InnerHtml += "<td><input id=\"date_picker_id_1\" type=\"hidden\" name=\"date_picker\" value=\"\" /></td></tr></table>";
        //    #endregion

        //    #region initdatereturn

        //    tempDate = dateNow.AddDays(7);


        //    divDate2.InnerHtml += "<table><tr><td><select id=\"ddlMarketDay2\" name=\"ddlMarketDay2\">";
        //    for (int i = 1; i <= 31; i++)
        //    {

        //        if (i == tempDate.Day)
        //        {
        //            divDate2.InnerHtml += "<option selected value=" + ConvTwoDigitDate(i.ToString()) + ">" + ConvTwoDigitDate(i.ToString()) + "</option>";
        //        }
        //        else
        //        {
        //            divDate2.InnerHtml += "<option value=" + ConvTwoDigitDate(i.ToString()) + ">" + ConvTwoDigitDate(i.ToString()) + "</option>";
        //        }

        //        //divDate2.InnerHtml += "<option value=" + ConvTwoDigitDate(i.ToString()) + ">" + ConvTwoDigitDate(i.ToString()) + "</option>";
        //    }
        //    divDate2.InnerHtml += "</select></td>";
        //    divDate2.InnerHtml += "<td>&nbsp;</td><td><select class=\"wLrg2s\" id=\"ddlMarketMonth2\" name=\"ddlMarketMonth2\">";

        //    for (int i = 0; i <= 13; i++)
        //    {
        //        /*
        //        tempDate2 = dateNow.AddDays(7);
        //        tempDate = tempDate2.AddMonths(i);
        //        */
        //        tempDate2 = dateNow.AddMonths(i);
        //        monthNumeric = tempDate2.ToString("MM");
        //        monthString = tempDate2.ToString("MMM");
        //        year = tempDate2.ToString("yyyy");
        //        string val = year + "-" + monthNumeric;
        //        string disp = monthString + " " + year;

        //        if (tempDate.Month == Convert.ToInt16(monthNumeric) && tempDate.Year == Convert.ToInt16(year))
        //        {
        //            //divDate2.InnerHtml += "<option selected value=\"" + val + "\">" + disp + "</option>";
        //            divDate2.InnerHtml += "<option value=\"" + val + "\">" + disp + "</option>";
        //        }
        //        else
        //        {
        //            divDate2.InnerHtml += "<option value=\"" + val + "\">" + disp + "</option>";
        //        }

        //    }
        //    divDate2.InnerHtml += "</select></td>";
        //    divDate2.InnerHtml += "<td><input id=\"date_picker_id_2\" type=\"hidden\" name=\"date_picker\" value=\"\" /></td></tr></table>";

        //    tdReturn.Attributes["display"] = "";
        //    if (cb_OneWay.Checked == true)
        //    {
        //        tdReturn.Attributes["display"] = "none";
        //    }
        //    #endregion

        //    //UIClass.SetComboCustomStyle(ref ddlDeparture, UIClass.EnumDefineStyle.City, string.Empty, string.Empty, false);
        //    //UIClass.SetComboCustomStyle(ref ddlReturn, UIClass.EnumDefineStyle.City, string.Empty, ddlDeparture.SelectedItem.Value, false);

        //    GeneralControl CountryBase = new GeneralControl();
        //    DataTable dt = new DataTable();
        //    dt = CountryBase.GetLookUpCity("", Request.PhysicalApplicationPath);
        //    if (dt == null || dt.Rows.Count <= 0)
        //    {
        //        dt = CountryBase.ReturnAllCityCustom("");
        //    }
        //    //SEAL.WEB.UI.Control.SetComboCustomStyle(ref ddlDeparture, dt, "CustomState", "CityCode", "Select City");
        //    SEAL.WEB.UI.Control.SetComboCustomStyle(ref ddlDeparture, dt, "CustomState", "DepartureStation", "Select City");

        //}


        //private void BindCurrency(string Departure)
        //{
        //    try
        //    {
        //        GeneralControl CountryBase = new GeneralControl();
        //        DataTable dt = new DataTable();
        //        dt = CountryBase.GetLookUpCity("", Request.PhysicalApplicationPath);
        //        DataRow[] drs = dt.Select("DepartureStation='" + Departure + "'");
        //        if (drs != null)
        //        {
        //            ddl_Currency.SelectedItem.Text = drs[0]["DepartureStationCurrencyCode"].ToString();
        //            //ddl_Currency.SelectedItem.Text = objGeneral.GetCurrencyByDeparture(Departure);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(this,ex);
        //    }

        //}

        //protected bool CheckSuspend()
        //{
        //    if (objBooking.CheckStillSuspend(MyUserSet.AgentID, ddlDeparture.SelectedValue, ddlReturn.SelectedValue))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //protected int AddEnquiry()
        //{
        //    int blacklist = 0;
        //    string enID = objBooking.CheckEnqExist(MyUserSet.AgentID, ddlDeparture.SelectedValue, ddlReturn.SelectedValue);
        //    if (enID != "")
        //    {
        //        //counter enquiry
        //        enqLogInfo = objBooking.GetSingleEN_ENQUIRYLOG(enID, MyUserSet.AgentID, DateTime.Now);

        //        enqLogInfo.EnquiryDate = DateTime.Now;
        //        enqLogInfo.LastEnquiryDate = DateTime.Now;
        //        enqLogInfo.NoOfAttempt = enqLogInfo.NoOfAttempt + 1;
        //        enqLogInfo.SyncLastUpd = DateTime.Now;

        //        objBooking.SaveEN_ENQUIRYLOG(enqLogInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Update);
        //    }
        //    else
        //    {
        //        //insert new
        //        enID = DateTime.Now.ToString("yyyyMMddHHmmsss");

        //        enqLogInfo.AgentID = MyUserSet.AgentID;
        //        enqLogInfo.EnquiryID = enID;
        //        enqLogInfo.EnquiryDate = DateTime.Now;
        //        enqLogInfo.LastEnquiryDate = DateTime.Now;
        //        enqLogInfo.Origins = ddlDeparture.SelectedValue;
        //        enqLogInfo.Destination = ddlReturn.SelectedValue;
        //        enqLogInfo.NoOfAttempt = 1;
        //        enqLogInfo.SyncCreate = DateTime.Now;
        //        enqLogInfo.SyncLastUpd = DateTime.Now;
        //        enqLogInfo.CreateBy = MyUserSet.AgentID;

        //        objBooking.SaveEN_ENQUIRYLOG(enqLogInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
        //    }
        //    // check max enquiry
        //    if (enqLogInfo.NoOfAttempt >= Convert.ToInt32(MyUserSet.MaxEnquiry))
        //    {
        //        //check suspend exist first  

        //        string susID = objBooking.CheckSuspExist(MyUserSet.AgentID, ddlDeparture.SelectedValue, ddlReturn.SelectedValue);
        //        if (susID != "")
        //        {
        //            enqSusInfo = objBooking.GetSingleEN_SUSPENDLIST(susID, MyUserSet.AgentID, ddlDeparture.SelectedValue, ddlReturn.SelectedValue);
        //            enqSusInfo.SuspendAttempt = enqSusInfo.SuspendAttempt + 1;
        //            enqSusInfo.SuspendDate = DateTime.Now;
        //            enqSusInfo.SuspendExpiry = DateTime.Now.AddHours(Convert.ToInt16(MyUserSet.SuspendDuration));
        //            enqSusInfo.SyncLastUpd = DateTime.Now;
        //            objBooking.SaveEN_SUSPENDLIST(enqSusInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
        //        }
        //        else
        //        {
        //            //insert new suspend
        //            susID = DateTime.Now.ToString("yyyyMMddHHmmsss");

        //            enqSusInfo.SuspendID = susID;
        //            enqSusInfo.AgentID = MyUserSet.AgentID;
        //            enqSusInfo.Origins = ddlDeparture.SelectedValue;
        //            enqSusInfo.Destination = ddlReturn.SelectedValue;
        //            enqSusInfo.LastEnquiryID = enID;
        //            enqSusInfo.SuspendAttempt = 1;
        //            enqSusInfo.CreateBy = MyUserSet.AgentID;
        //            enqSusInfo.SuspendDate = DateTime.Now;
        //            enqSusInfo.SuspendExpiry = DateTime.Now.AddHours(Convert.ToInt16(MyUserSet.SuspendDuration));
        //            enqSusInfo.SyncCreate = DateTime.Now;
        //            enqSusInfo.SyncLastUpd = DateTime.Now;
        //            objBooking.SaveEN_SUSPENDLIST(enqSusInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
        //        }
        //        //roll back enquiry

        //        objBooking.RollBackEnquiry(enqLogInfo.EnquiryID);

        //        //update activity suspend
        //        agActivityInfo.AgentID = MyUserSet.AgentID;
        //        agActivityInfo.LastSuspend = enqSusInfo.SuspendDate;
        //        objAgent.SaveAG_ACTIVITY(agActivityInfo, ABS.Logic.GroupBooking.Agent.AgentControl.EnumSaveType.Update);


        //        //check max suspend

        //        if (objBooking.GetTotalSuspend((MyUserSet.AgentID)) >= Convert.ToInt16(MyUserSet.MaxSuspend))
        //        {
        //            //blacklist agent
        //            string blacklistID = "";
        //            blacklistID = objAgent.CheckBlacklistExist(MyUserSet.AgentID);
        //            if (blacklistID != "")
        //            {
        //                //blacklist exist
        //                agBlacklistInfo = objAgent.GetSingleAG_BLACKLIST(MyUserSet.AgentID);
        //                agBlacklistInfo.BlacklistDate = DateTime.Now;
        //                agBlacklistInfo.BlacklistExpiryDate = DateTime.Now.AddDays(Convert.ToInt16(MyUserSet.BlacklistDuration));
        //                agBlacklistInfo.SyncLastUpd = DateTime.Now;
        //                agBlacklistInfo.LastSyncBy = MyUserSet.AgentID;
        //                agBlacklistInfo.Remark = "Suspend reach limit.";

        //                objAgent.SaveAG_BLACKLIST(agBlacklistInfo, ABS.Logic.GroupBooking.Agent.AgentControl.EnumSaveType.Update);
        //            }
        //            else
        //            {
        //                //insert new blacklist
        //                blacklistID = DateTime.Now.ToString("yyyyMMddHHmmsss");
        //                agBlacklistInfo.AgentID = MyUserSet.AgentID;
        //                agBlacklistInfo.BlacklistDate = DateTime.Now;
        //                agBlacklistInfo.BlacklistExpiryDate = DateTime.Now.AddDays(Convert.ToInt16(MyUserSet.BlacklistDuration));
        //                agBlacklistInfo.BlacklistID = blacklistID;
        //                agBlacklistInfo.LastSyncBy = MyUserSet.AgentID;
        //                agBlacklistInfo.Status = 1;
        //                agBlacklistInfo.SyncCreate = DateTime.Now;
        //                agBlacklistInfo.SyncLastUpd = DateTime.Now;
        //                agBlacklistInfo.Remark = "Suspend reach limit.";

        //                objAgent.SaveAG_BLACKLIST(agBlacklistInfo, ABS.Logic.GroupBooking.Agent.AgentControl.EnumSaveType.Insert);
        //            }

        //            //update blacklist activity
        //            agActivityInfo.AgentID = MyUserSet.AgentID;
        //            agActivityInfo.LastBlacklist = agBlacklistInfo.BlacklistDate;
        //            agActivityInfo.ExpiryBlacklistDate = agBlacklistInfo.BlacklistExpiryDate;
        //            objAgent.SaveAG_ACTIVITY(agActivityInfo, ABS.Logic.GroupBooking.Agent.AgentControl.EnumSaveType.Update);

        //            blacklist = 1;
        //        }
        //    }
        //    return blacklist;
        //}

        //protected void ddlDeparture_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //UIClass.SetComboCustomStyle(ref ddlReturn, UIClass.EnumDefineStyle.City, string.Empty, ddlDeparture.SelectedItem.Value, false);
        //        GeneralControl CountryBase = new GeneralControl();
        //        DataTable dt = new DataTable();

        //        dt = CountryBase.GetLookUpCity(ddlDeparture.SelectedItem.Value, Request.PhysicalApplicationPath);
        //        //SEAL.WEB.UI.Control.SetComboCustomStyle(ref ddlReturn, dt, "RCustomState", "RCityCode", "Select City");
        //        SEAL.WEB.UI.Control.SetComboCustomStyle(ref ddlReturn, dt, "RCustomState", "ArrivalStation", "Select City");
        //        if (dt == null || dt.Rows.Count <= 0)
        //        {
        //            //dt = CountryBase.ReturnAllCityCustom(ddlDeparture.SelectedItem.Value);
        //            //SEAL.WEB.UI.Control.SetComboCustomStyle(ref ddlReturn, dt, "CustomState", "CityCode", "Select City");
        //            //SEAL.WEB.UI.Control.SetComboCustomStyle(ref ddlReturn, dt, "CustomState", "DepartureStation", "Select City");
        //            return;
        //        }
        //        BindCurrency(ddlDeparture.SelectedItem.Value);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(this,ex);
        //    }
        //}

        //private void BindDDL()
        //{
        //    string departure = ddlDeparture.SelectedItem.Value;
        //    if (departure == "KUL")
        //        ddl_Currency.SelectedItem.Text = "Malaysian Ringgit (MYR)";
        //    else if (departure == "SZX")
        //        ddl_Currency.SelectedItem.Text = "Chinese Renminbi Yuan (CNY)";
        //    else ddl_Currency.SelectedItem.Value = "US Dollar (USD)";
        //}

        //protected void cb_OneWay_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (cb_OneWay.Checked == true)
        //    {
        //        tdReturn.Visible = false;
        //    }
        //    else
        //    {
        //        tdReturn.Visible = true;
        //    }
        //}

    }
}