using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;

using DevExpress.Web;
using ABS.Logic.GroupBooking.Agent;
using ABS.Logic.GroupBooking.Booking;
using SEAL.Data;
using ABS.Logic.Shared;
//using log4net;
using System.Globalization;
using System.Collections;
using ABS.Navitaire.BookingManager;
using ABS.Logic.GroupBooking;
using ABS.GBS.Log;
using StackExchange.Profiling;
using System.Reflection;

namespace GroupBooking.Web.Booking
{
    public partial class selectFlightAverage : System.Web.UI.Page
    {
        #region declaration
        UserSet MyUserSet;
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogControl log = new LogControl();
        SystemLog SystemLog = new SystemLog();

        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
        ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");
        Bk_transssr BK_TRANSSSRInfo = new Bk_transssr();
        List<Bk_transssr> listbk_transssrinfo = new List<Bk_transssr>();
        BookingTransactionDetail bookDTLInfo = new BookingTransactionDetail();
        List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
        BookingTransactionMain bookHDRInfo = new BookingTransactionMain();
        ABS.Logic.GroupBooking.Agent.AgentControl objAgent = new ABS.Logic.GroupBooking.Agent.AgentControl();
        ABS.Logic.GroupBooking.Agent.AgentProfileControl objAgentProfile = new ABS.Logic.GroupBooking.Agent.AgentProfileControl();
        decimal totalFlightFare, totalServiceFee, totalPaxFee, totalBaggageFare = 0, totalServVAT;
        ABS.Logic.GroupBooking.Agent.AgentProfile agent = new ABS.Logic.GroupBooking.Agent.AgentProfile();

        BookingTransactionFees bookFEEInfo = new BookingTransactionFees();
        List<BookingTransactionFees> lstbookFEEInfo = new List<BookingTransactionFees>();

        string Currency = "USD";
        decimal APT = 0;
        decimal AVGFare = 0;
        string tranID = "";
        DataTable dtTaxFees = new DataTable();
        DataTable dtDataFeeCode = new DataTable();
        string departIDTem = "";
        string returnIDTem = "";

        private static int num = 0;
        private static int departID = 0;
        private static string ReturnID = "";

        //20170411 - Sienny
        string organizationID = "";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //added by ketee, performance monitoring
            var profiler = MiniProfiler.Current;

            Session["PaxStatus"] = "";
            if (Session["AgentSet"] != null)
            { MyUserSet = (UserSet)Session["AgentSet"]; }
            else
            {
                if (IsCallback)
                    ASPxCallback.RedirectOnCallback(GroupBooking.Web.Shared.MySite.PublicPages.SessionExpired);
                else
                    Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.SessionExpired);
                return;
            }

            SessionContext sesscon = new SessionContext();
            //sesscon.ValidateAgentLogin();
            if (!IsPostBack)
            {
                Session["bookHDRInfo"] = null;
                Session["lstbookDTLInfo"] = null;
                Session["gbstimer"] = DateTime.Now.AddSeconds(99);
                //end added
                using (profiler.Step("InitializeForm"))
                {
                    InitializeForm();
                }

                //added by ketee
                setValue();
                using (profiler.Step("CheckReturnForSaving"))
                {
                    CheckReturnForSaving();
                }

                using (profiler.Step("LoadingProcess"))
                {
                    LoadingProcess();
                }

                //20170721 - Sienny
                using (profiler.Step("BindFeesData"))
                {
                    BindFeesData();
                }

                using (profiler.Step("HideZeroValue"))
                {
                    HideZeroValue();
                }

                //Timer1.Enabled = true;
            }

        }
        protected void CheckReturnForSaving()
        {
            //because on review booking does not have LblReturn. so I decide to put on session for checking on reviewbooking page.
            Session["is2Way"] = 0;
            if (LblReturn.Text != "")
            { Session["is2Way"] = 1; }
            else
            { Session["is2Way"] = 0; }
        }
        protected void InitializeForm()
        {
            var profiler = MiniProfiler.Current;
            try
            {
                //tmrCount.Enabled = false;
                string OutID = "";
                string InID = "";
                int GuestNum = 0;
                int ChildNum = 0;
                int PaxNum = 0;
                HttpCookie cookie = Request.Cookies["cookieTemFlight"];
                if (cookie != null)
                {
                    OutID = cookie.Values["list1ID"];
                    InID = cookie.Values["ReturnID"];
                    GuestNum = Convert.ToInt32(cookie.Values["GuestNum"]);
                    PaxNum = Convert.ToInt32(cookie.Values["PaxNum"]);
                    ChildNum = Convert.ToInt32(cookie.Values["ChildNum"]);

                    departIDTem = OutID;
                    returnIDTem = InID;

                    ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
                    ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model2 = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();

                    string strExpr;
                    string strSort;
                    DataTable dt = objBooking.dtFlight();

                    strExpr = "TemFlightId = '" + OutID + "'";

                    strSort = "";

                    dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                    DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                    //added by ketee, 20170916, performance checking
                    using (profiler.Step("FillModel"))
                    {
                        if (foundRows.Length > 0)
                        {
                            FillModelFromDataRow(foundRows, ref model);
                        }
                        else
                        {
                            Response.Redirect(Shared.MySite.PublicPages.SessionExpired, false);
                        }

                        if (InID != "")
                        {
                            strExpr = "TemFlightId = '" + InID + "'";

                            strSort = "";

                            dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                            foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                            if (foundRows.Length > 0)
                            {
                                FillModelFromDataRow(foundRows, ref model2);
                            }
                            else
                            {
                                Response.Redirect(Shared.MySite.PublicPages.SessionExpired, false);
                            }

                        }
                    }


                    //Update temFlight DataTable
                    string sessID = "";
                    int count = 0;
                    //added by ketee, 20170916, performance checking
                    using (profiler.Step("UpdateTemFlight"))
                    {
                        objBooking.UpdateTemFlight(model, model2, "", ref sessID);
                    }


                    if (HttpContext.Current.Session["errormsgTimeZone"] != null)
                    {
                        Session["error"] = HttpContext.Current.Session["errormsgTimeZone"].ToString();
                        HttpContext.Current.Session["errormsgTimeZone"] = null;
                        Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                    }
                    else
                    {
                        Session["signatureSess"] = sessID;
                        string signature = Session["signatureSess"].ToString();

                        using (profiler.Step("SellFlight"))
                        {
                            if (SellFlight())
                            {
                                using (profiler.Step("BindData"))
                                {
                                    BindData(OutID, InID, sessID);
                                }
                            }
                            else
                                Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                Response.Redirect("~/SessionExpired.aspx?msg=Err100060");
            }
        }

        private Boolean SellFlight()
        {
            ABS.Logic.GroupBooking.Agent.AgentProfile agent = new ABS.Logic.GroupBooking.Agent.AgentProfile();
            String OrgID = "";
            agent = (ABS.Logic.GroupBooking.Agent.AgentProfile)Session["agProfileInfo"];
            var profiler = MiniProfiler.Current;
            try
            {
                HttpCookie cookie = Request.Cookies["cookieTemFlight"];
                if (cookie != null)
                {
                    if (IsNumeric(cookie.Values["list1ID"]))
                    {
                        departID = Convert.ToInt32(cookie.Values["list1ID"]);
                    }
                    else
                    {
                        departID = -1;
                    }

                    ReturnID = cookie.Values["ReturnID"];
                    num = Convert.ToInt32(cookie.Values["PaxNum"]);
                }

                //remark by ketee, since MyUserSet Storing the OrganizationID, 20170916
                //agent = objAgentProfile.GetSingleAgentProfile(MyUserSet.AgentName);
                //if (agent != null)
                //{
                //    OrgID = agent.OrgID;
                //}

                OrgID = MyUserSet.OrganicationID;

                ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
                ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight2 = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();

                string strExpr;
                string strSort;
                DataTable dt = new DataTable();
                strExpr = "TemFlightId = '" + departID + "'";
                strSort = "";

                dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                if (foundRows.Length > 0)
                {
                    FillModelFromDataRow(foundRows, ref temFlight);
                }
                else
                {
                    Response.Redirect(Shared.MySite.PublicPages.SessionExpired, false);
                }

                string getfare = temFlight.TemFlightServiceCharge.ToString();
                if (agent != null) OrgID = agent.OrgID.ToString();

                if (ReturnID != "")
                {
                    strExpr = "TemFlightId = '" + ReturnID + "'";
                    strSort = "";

                    dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                    foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                    if (foundRows.Length > 0)
                    {
                        FillModelFromDataRow(foundRows, ref temFlight2);
                    }
                    else
                    {
                        Response.Redirect(Shared.MySite.PublicPages.SessionExpired, false);
                    }


                    //string LoginType = MyUserSet.AgentType.ToString();  //Session["LoginType"].ToString();
                    string LoginType = "PublicAgent";
                    string LoginName = MyUserSet.AgentName;
                    string LoginPWD = "";
                    string LoginDomain = "";
                    /* remark to ag payment process
                    if (LoginType == "SkyAgent")
                    {
                        LoginPWD = Session["LoginPWD"].ToString();
                        LoginDomain = Session["LoginDomain"].ToString();
                    }*/
                    //objBooking.SellFlightByTem(temFlight, temFlight2, LoginType, LoginName, LoginPWD, LoginDomain, "");
                    //log.Info(this, "Entering return Flight Saving.");


                    using (profiler.Step("objBooking.GetItinerary"))
                    {
                        if (objBooking.GetItinerary(temFlight, temFlight2, LoginType, LoginName, LoginPWD, LoginDomain, "", OrgID) == false)
                        //if (objBooking.SellJourney(temFlight, temFlight2, LoginType, LoginName, LoginPWD, LoginDomain, "", OrgID) == false)
                        {

                            //Page.ClientScript.RegisterStartupScript(this.GetType(), "PostbackKey", "<script type='text/javascript'>document.getElementById('ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg').innerHTML = 'eko';window.location.href='../public/selectflight.aspx';</script>");
                            if (HttpContext.Current.Session["errormsg"] != null && Session["errormsg"].ToString().ToLower().Contains("sold"))
                            {
                                Session["soldout"] = true;
                                Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                                return false;
                            }
                            else if (HttpContext.Current.Session["errormsg"] != null && Session["errormsg"].ToString().ToLower().Contains("overlap"))
                            {
                                Session["overlap"] = true;
                                Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                                return false;
                            }
                            else
                            {
                                Session["error"] = Session["errormsg"].ToString();
                                Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                                return false;
                            }

                        }
                    }

                    /* remark by romy for optimize
                    HttpCookie cookie2 = Request.Cookies["cookieSearchcondition"];
                    if (cookie2 != null)
                    {
                        if (cookie2.Values["InfantNum"] != "")
                        {
                            HttpCookie cookie3 = Request.Cookies["AllPax"];
                            if (cookie3 != null)
                            if (cookie3 == null)
                            {
                                //remark 1st ya
                                if (objBooking.SellJourneyAddInfant(Convert.ToInt32(cookie3.Values["InfantNum"]), temFlight, temFlight2, LoginType, LoginName, LoginPWD, LoginDomain, "") == false)
                                {

                                    if (HttpContext.Current.Session["errormsg"] != null && Session["errormsg"].ToString().ToLower().Contains("sold"))
                                    {
                                        Session["soldout"] = true;
                                        Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                                        return false;
                                    }
                                    else if (HttpContext.Current.Session["errormsg"] != null && Session["errormsg"].ToString().ToLower().Contains("overlap"))
                                    {
                                        Session["overlap"] = true;
                                        Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                                        return false;
                                    }
                                    else
                                    {
                                        Session["error"] = Session["errormsg"].ToString();
                                        Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                                        return false;
                                    }

                                }


                            }
                            else
                            {
                                using (profiler.Step("objBooking.SellJourney"))
                                {
                                    if (objBooking.SellJourney(temFlight, temFlight2, LoginType, LoginName, LoginPWD, LoginDomain, "", OrgID) == false)
                                    {

                                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "PostbackKey", "<script type='text/javascript'>document.getElementById('ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg').innerHTML = 'eko';window.location.href='../public/selectflight.aspx';</script>");
                                        if (HttpContext.Current.Session["errormsg"] != null && Session["errormsg"].ToString().ToLower().Contains("sold"))
                                        {
                                            Session["soldout"] = true;
                                            Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                                            return false;
                                        }
                                        else if (HttpContext.Current.Session["errormsg"] != null && Session["errormsg"].ToString().ToLower().Contains("overlap"))
                                        {
                                            Session["overlap"] = true;
                                            Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                                            return false;
                                        }
                                        else
                                        {
                                            Session["error"] = Session["errormsg"].ToString();
                                            Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                                            return false;
                                        }

                                    }
                                }

                            }
                        }
                        else
                        {
                            if (objBooking.SellJourney(temFlight, temFlight2, LoginType, LoginName, LoginPWD, LoginDomain, "", OrgID) == false)
                            {

                                if (HttpContext.Current.Session["errormsg"] != null && Session["errormsg"].ToString().ToLower().Contains("sold"))
                                {
                                    Session["soldout"] = true;
                                    Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                                    return false;
                                }
                                else if (HttpContext.Current.Session["errormsg"] != null && Session["errormsg"].ToString().ToLower().Contains("overlap"))
                                {
                                    Session["overlap"] = true;
                                    Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                                    return false;
                                }
                                else
                                {
                                    Session["error"] = Session["errormsg"].ToString();
                                    Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                                    return false;
                                }
                            }
                        }
                    }*/

                }
                else
                {
                    //string LoginType = MyUserSet.AgentType.ToString();
                    string LoginType = "PublicAgent";
                    string LoginName = MyUserSet.AgentName;
                    string LoginPWD = "";
                    string LoginDomain = "";
                    /* remark to ag payment process
                    if (LoginType == "SkyAgent")
                    {
                        LoginPWD = Session["LoginPWD"].ToString();
                        LoginDomain = Session["LoginDomain"].ToString();
                    }*/
                    using (profiler.Step("objBooking.GetItineraryByTem"))
                    {
                        if (objBooking.GetItineraryByTem(temFlight, LoginType, LoginName, LoginPWD, LoginDomain, "", OrgID) == false)
                        //if (objBooking.SellFlightByTem(temFlight, LoginType, LoginName, LoginPWD, LoginDomain, "", OrgID) == false)
                        {
                            if (HttpContext.Current.Session["errormsg"] != null && Session["errormsg"].ToString().ToLower().Contains("sold"))
                            {
                                Session["soldout"] = true;
                                Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                                return false;
                            }
                            else if (HttpContext.Current.Session["errormsg"] != null && Session["errormsg"].ToString().ToLower().Contains("overlap"))
                            {
                                Session["overlap"] = true;
                                Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                                return false;
                            }
                            else
                            {
                                Session["error"] = Session["errormsg"].ToString();
                                Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                                return false;
                            }
                        }
                    }

                    /* remark by romy for optimize
                    HttpCookie cookie2 = Request.Cookies["cookieSearchcondition"];
                    if (cookie2 != null)
                    {
                        if (cookie2.Values["InfantNum"] != "")
                        {
                            HttpCookie cookie3 = Request.Cookies["AllPax"];
                            if (cookie3 != null)
                                if (cookie3 == null)
                                {
                                    //remark 1st ya
                                    if (objBooking.SellFlightByTemAddInfant(Convert.ToInt32(cookie3.Values["InfantNum"]), temFlight, LoginType, LoginName, LoginPWD, LoginDomain, "") == false)
                                    {
                                        if (HttpContext.Current.Session["errormsg"] != null && Session["errormsg"].ToString().ToLower().Contains("sold"))
                                        {
                                            Session["soldout"] = true;
                                            Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                                            return false;
                                        }
                                        else if (HttpContext.Current.Session["errormsg"] != null && Session["errormsg"].ToString().ToLower().Contains("overlap"))
                                        {
                                            Session["overlap"] = true;
                                            Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                                            return false;
                                        }
                                        else
                                        {
                                            Session["error"] = Session["errormsg"].ToString();
                                            Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                                            return false;
                                        }
                                    }

                                }
                                else
                                {
                                    if (objBooking.SellFlightByTem(temFlight, LoginType, LoginName, LoginPWD, LoginDomain, "", OrgID) == false)
                                    {
                                        if (HttpContext.Current.Session["errormsg"] != null && Session["errormsg"].ToString().ToLower().Contains("sold"))
                                        {
                                            Session["soldout"] = true;
                                            Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                                            return false;
                                        }
                                        else if (HttpContext.Current.Session["errormsg"] != null && Session["errormsg"].ToString().ToLower().Contains("overlap"))
                                        {
                                            Session["overlap"] = true;
                                            Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                                            return false;
                                        }
                                        else
                                        {
                                            Session["error"] = Session["errormsg"].ToString();
                                            Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                                            return false;
                                        }
                                    }
                                }
                        }
                        else
                        {
                            if (objBooking.SellFlightByTem(temFlight, LoginType, LoginName, LoginPWD, LoginDomain, "", OrgID) == false)
                            {
                                if (HttpContext.Current.Session["errormsg"] != null && Session["errormsg"].ToString().ToLower().Contains("sold"))
                                {
                                    Session["soldout"] = true;
                                    Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                                    return false;
                                }
                                else if (HttpContext.Current.Session["errormsg"] != null && Session["errormsg"].ToString().ToLower().Contains("overlap"))
                                {
                                    Session["overlap"] = true;
                                    Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                                    return false;
                                }
                                else
                                {
                                    Session["error"] = Session["errormsg"].ToString();
                                    Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                                    return false;
                                }
                            }
                        }
                    }*/

                }

                return true;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                return false;
            }
        }

        protected void FillModelFromDataRow(DataRow[] foundRows, ref ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model)
        {
            try
            {
                if (IsNumeric(foundRows[0]["TemFlightId"].ToString()))
                { model.TemFlightId = Convert.ToInt16(foundRows[0]["TemFlightId"]); }
                model.TemFlightFlightNumber = foundRows[0]["TemFlightFlightNumber"].ToString();
                model.TemFlightDate = Convert.ToDateTime(foundRows[0]["TemFlightDate"]);
                model.TemFlightArrival = foundRows[0]["TemFlightArrival"].ToString();
                model.TemFlightCarrierCode = foundRows[0]["TemFlightCarrierCode"].ToString();
                model.TemFlightInternational = foundRows[0]["TemFlightInternational"].ToString();
                //model.TemFlightJourneySellKey = foundRows[0]["TemFlightJourneySellKey"].ToString();
                model.TemFlightCHDNum = Convert.ToInt16(foundRows[0]["TemFlightCHDNum"]);
                model.TemFlightCurrencyCode = foundRows[0]["TemFlightCurrencyCode"].ToString();
                model.TemFlightStd = Convert.ToDateTime(foundRows[0]["TemFlightStd"]);
                model.TemFlightDeparture = foundRows[0]["TemFlightDeparture"].ToString();
                model.TemFlightADTNum = Convert.ToInt16(foundRows[0]["TemFlightADTNum"]);
                model.TemFlightIfReturn = Convert.ToBoolean(foundRows[0]["TemFlightIfReturn"]);
                model.TemFlightPaxNum = Convert.ToInt16(foundRows[0]["TemFlightPaxNum"]);
                model.TemFlightSta = Convert.ToDateTime(foundRows[0]["TemFlightSta"]);
                model.TemFlightAgentName = foundRows[0]["TemFlightAgentName"].ToString();
                if (IsNumeric(foundRows[0]["TemFlightAveragePrice"].ToString()))
                { model.TemFlightAveragePrice = Convert.ToDecimal(foundRows[0]["TemFlightAveragePrice"]); }
                if (IsNumeric(foundRows[0]["TemFlightTotalAmount"].ToString()))
                { model.TemFlightTotalAmount = Convert.ToDecimal(foundRows[0]["TemFlightTotalAmount"]); }
                //amended by diana 20131103 - to insert each fare
                if (IsNumeric(foundRows[0]["TemFlightFarePrice"].ToString()))
                { model.temFlightfarePrice = Convert.ToDecimal(foundRows[0]["TemFlightFarePrice"]); }
                if (IsNumeric(foundRows[0]["TemFlightAPT"].ToString()))
                { model.TemFlightApt = Convert.ToDecimal(foundRows[0]["TemFlightAPT"]); }
                if (IsNumeric(foundRows[0]["TemFlightFuel"].ToString()))
                { model.TemFlightFuel = Convert.ToDecimal(foundRows[0]["TemFlightFuel"]); }

                if (IsNumeric(foundRows[0]["TemFlightOth"].ToString()))
                { model.TemFlightOth = Convert.ToDecimal(foundRows[0]["TemFlightOth"]); }
                if (IsNumeric(foundRows[0]["TemFlightDisc"].ToString()))
                { model.TemFlightDisc = Convert.ToDecimal(foundRows[0]["TemFlightDisc"]); }

                if (IsNumeric(foundRows[0]["TemFlightPromoDisc"].ToString()))
                { model.TemFlightPromoDisc = Convert.ToDecimal(foundRows[0]["TemFlightPromoDisc"]); }

                if (IsNumeric(foundRows[0]["TemFlightServiceCharge"].ToString()))
                { model.TemFlightServiceCharge = Convert.ToDecimal(foundRows[0]["TemFlightServiceCharge"]); }

                model.TemFlightTransit = foundRows[0]["TemFlightTransit"].ToString();
                DateTime sta2;
                if (DateTime.TryParse(foundRows[0]["TemFlightSta2"].ToString(), out sta2))
                    model.TemFlightSta2 = sta2;
                DateTime std2;
                if (DateTime.TryParse(foundRows[0]["TemFlightStd2"].ToString(), out std2))
                    model.TemFlightStd2 = std2;

                model.TemFlightCarrierCode2 = foundRows[0]["TemFlightCarrierCode2"].ToString();
                model.TemFlightFlightNumber2 = foundRows[0]["TemFlightFlightNumber2"].ToString();

                model.TemFlightPromoCode = foundRows[0]["TemFlightPromoCode"].ToString();
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
        }

        public static bool IsNumeric(string s)
        {
            double result;
            return s != null && Double.TryParse(s, out result);
        }

        private void setValue()
        {
            //load from database here
            //if (MyUserSet.CounterTimer != null)
            //{
            //    ctrHdn.Value = MyUserSet.CounterTimer.ToString();
            //}
            //else
            //{
            //    //default 
            //    ctrHdn.Value = "30";
            //}

            HttpCookie cookie = Request.Cookies["cookieTemFlight"];
            if (cookie != null)
            {
                //load value review                
                string DepartID = cookie.Values["list1ID"].ToString();
                if (DepartID != string.Empty)
                {

                    ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();

                    temFlight = GetTemFlight(DepartID);
                    lblDepartFare.Text = (objGeneral.RoundUp(temFlight.TemFlightTotalAmount / temFlight.TemFlightPaxNum)).ToString("N", nfi);
                    lblDepartCurrency.Text = temFlight.TemFlightCurrencyCode.ToString();
                }
                else
                {
                    Response.Redirect(Shared.MySite.PublicPages.Selectflight);
                }

                if (cookie.Values["ReturnID"] != "")
                {
                    string ReturnID = cookie.Values["ReturnID"].ToString();
                    if (ReturnID != string.Empty)
                    {

                        ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();

                        temFlight = GetTemFlight(ReturnID);

                        lblReturnFare.Text = (objGeneral.RoundUp(temFlight.TemFlightTotalAmount / temFlight.TemFlightPaxNum)).ToString("N", nfi);
                        lblReturnCurrency.Text = temFlight.TemFlightCurrencyCode.ToString();


                        ShowReturnColumn(true);
                    }
                }
                else
                {
                    lblReturnFare.Text = "0";
                    lblReturnCurrency.Text = lblDepartCurrency.Text;

                    ShowReturnColumn(false);
                }

                //lblTotPax.Text = cookie.Values["PaxNum"].ToString();

                if (lbl_ChildNumout.Text == "") lbl_ChildNumout.Text = "0";
                if (Convert.ToInt32(lbl_ChildNumout.Text) > 0)
                {
                    ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
                    temFlight = GetTemFlight(DepartID);
                    //if (temFlight.TemFlightDeparture.ToString().ToUpper() == "HKG" || temFlight.TemFlightArrival.ToString().ToUpper() == "HKG")
                    //{
                    //trChildTax.Visible = true;
                    //lblTextTaxFareDepart.Text = "Airport Tax (Adult) : ";
                    //lblTextTaxFareReturn.Text = "Airport Tax (Adult) : ";
                    //}
                    //lblDetailPax.Text = lbl_GuestNumout.Text + " Adult / " + lbl_ChildNumout.Text + " Child";
                    lblTotPax.Text = temFlight.TemFlightPaxNum.ToString();
                    HttpCookie cookie2 = Request.Cookies["cookieSearchcondition"];
                    if (cookie2 != null)
                    {
                        if (cookie2.Values["InfantNum"] != "")
                        {
                            HttpCookie cookie3 = Request.Cookies["AllPax"];
                            if (cookie3 != null)
                            {

                                lblTotInfant.Text = cookie2.Values["InfantNum"];
                            }
                            else
                            {
                                trInfantTotal.Style.Add("display", "none");
                            }
                        }
                        else
                        {
                            trInfantTotal.Style.Add("display", "none");
                        }
                    }
                    //amended by diana 20141004, replace cookie 
                }
                else
                {
                    //lblDetailPax.Text = lbl_GuestNumout.Text + " Adult";
                    lblTotPax.Text = lbl_GuestNumout.Text; //amended by diana 20141004, replace cookie
                    HttpCookie cookie2 = Request.Cookies["cookieSearchcondition"];
                    if (cookie2 != null)
                    {
                        if (cookie2.Values["InfantNum"] != "")
                        {
                            HttpCookie cookie3 = Request.Cookies["AllPax"];
                            if (cookie3 != null)
                            {

                                lblTotInfant.Text = cookie2.Values["InfantNum"];
                            }
                            else
                            {
                                trInfantTotal.Style.Add("display", "none");
                            }
                        }
                        else
                        {
                            trInfantTotal.Style.Add("display", "none");
                        }
                    }
                }
            }

        }

        private void ShowReturnColumn(bool show)
        {
            tdReturnText.Visible = show;
            tdReturnTitle.Visible = show;
            tdReturn.Visible = show;

        }

        private ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight GetTemFlight(string ID)
        {
            try
            {
                string strExpr;
                string strSort;
                DataTable dt = objBooking.dtFlight();
                ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();

                strExpr = "TemFlightId = '" + ID + "'";

                strSort = "";
                // Use the Select method to find all rows matching the filter.

                dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                if (foundRows.Length > 0)
                {
                    FillModelFromDataRow(foundRows, ref model);
                }
                else
                {
                    Response.Redirect(Shared.MySite.PublicPages.SessionExpired, false);
                }

                return model;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                return null;
            }
        }

        /*
        private ABS.Logic.GroupBooking.Booking.BookingControl.TaxCharge GetTaxCharge(string origin, string destination)
        {
            try
            {
                string strExpr;
                string strSort;
                DataTable dt = objBooking.dtClass();
                ABS.Logic.GroupBooking.Booking.BookingControl.TemClassofService model = new ABS.Logic.GroupBooking.Booking.BookingControl.TemClassofService();

                strExpr = "Origin = '" + origin + "' AND Destination = '" + destination + "'";

                strSort = "";
                // Use the Select method to find all rows matching the filter.

                dt = (DataTable)HttpContext.Current.Session["dataClass"];

                DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                FillModelFromDataRow(foundRows, ref model);

                return model;
            }
            catch (Exception ex)
            {
                log.Error(this,ex);
                return null;
            }      
        }
        */

        private void BindData(string OutID, string InID, string sessID)
        {
            ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
            ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model2 = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();

            decimal total = 0;
            model = GetTemFlight(OutID);

            bool freeSVCF = false;

            HttpCookie cookie = Request.Cookies["cookieSearchcondition"];

            lbl_FlightTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(model.temFlightfarePrice) * num).ToString("N", nfi);

            //lbl_currency0.Text = model.TemFlightCurrencyCode.ToString();
            lbl_currency1.Text = model.TemFlightCurrencyCode.ToString();
            lbl_currency2.Text = model.TemFlightCurrencyCode.ToString();
            lbl_currency21.Text = model.TemFlightCurrencyCode.ToString();
            lbl_currency3.Text = model.TemFlightCurrencyCode.ToString();
            lbl_currency31.Text = model.TemFlightCurrencyCode.ToString();
            lbl_InNum21.Text = num.ToString();
            if (Convert.ToDecimal(model.TemFlightCHDNum) != 0) // && (model.TemFlightDeparture.ToString().ToUpper() == "HKG" || model.TemFlightArrival.ToString().ToUpper() == "HKG"))
            {
                lbl_currency2CHD.Text = model.TemFlightCurrencyCode.ToString();
                lbl_currency3CHD.Text = model.TemFlightCurrencyCode.ToString();
                lbl_num2.Text = model.TemFlightADTNum.ToString() + " Adult Airport Tax @ ";
                lbl_num2CHD.Text = model.TemFlightCHDNum.ToString();
                lbl_InNum2.Text = model.TemFlightADTNum.ToString() + " Adult Airport Tax @ ";
                lbl_InNum2CHD.Text = model.TemFlightCHDNum.ToString();
            }
            else
            {
                lbl_num2.Text = num.ToString() + " Airport Tax @ ";
                lbl_InNum2.Text = num.ToString() + " Airport Tax @ ";
            }
            lbl_currency4.Text = model.TemFlightCurrencyCode.ToString();
            lbl_currency5.Text = model.TemFlightCurrencyCode.ToString();
            lbl_currency6.Text = model.TemFlightCurrencyCode.ToString();
            lbl_currency7.Text = model.TemFlightCurrencyCode.ToString();
            lblCurrFuelDepart.Text = model.TemFlightCurrencyCode.ToString();
            lblCurrOthDepart.Text = model.TemFlightCurrencyCode.ToString();
            lblCurrSvcDepart.Text = model.TemFlightCurrencyCode.ToString();
            lblCurrVATDepart.Text = model.TemFlightCurrencyCode.ToString();
            lblCurrPromoDiscDepart.Text = model.TemFlightCurrencyCode.ToString();
            lbl_currency9.Text = model.TemFlightCurrencyCode.ToString();

            lbl_currency2.Text = model.TemFlightCurrencyCode;
            lbl_currency21.Text = model.TemFlightCurrencyCode;
            lblCurrAVLDepart.Text = model.TemFlightCurrencyCode;
            lblCurrPSFDepart.Text = model.TemFlightCurrencyCode;
            lblCurrSCFDepart.Text = model.TemFlightCurrencyCode;
            lblCurrConnectingDepart.Text = model.TemFlightCurrencyCode;
            lblCurrDiscDepart.Text = model.TemFlightCurrencyCode;
            lblCurrKlia2Depart.Text = model.TemFlightCurrencyCode;
            lblCurrGSTDepart.Text = model.TemFlightCurrencyCode;
            lblCurrSPLDepart.Text = model.TemFlightCurrencyCode;
            lblCurrAPSDepart.Text = model.TemFlightCurrencyCode;
            lblCurrIADFDepart.Text = model.TemFlightCurrencyCode;
            lblCurrACFDepart.Text = model.TemFlightCurrencyCode;
            lblCurrCSTDepart.Text = model.TemFlightCurrencyCode;
            lblCurrCUTDepart.Text = model.TemFlightCurrencyCode;
            lblCurrSGIDepart.Text = model.TemFlightCurrencyCode;
            lblCurrSSTDepart.Text = model.TemFlightCurrencyCode;
            lblCurrUDFDepart.Text = model.TemFlightCurrencyCode;
            lblCurrASCDepart.Text = model.TemFlightCurrencyCode;
            lblCurrBCLDepart.Text = model.TemFlightCurrencyCode;
            lblCurrIWJRDepart.Text = model.TemFlightCurrencyCode;
            lblCurrVATChargeDepart.Text = model.TemFlightCurrencyCode;

            lbl_num1.Text = num.ToString();
            lbl_num3.Text = num.ToString();
            lbl_num4.Text = num.ToString();
            lbl_num5.Text = num.ToString();
            lbl_num6.Text = num.ToString();
            lbl_InNum.Text = num.ToString();
            lbl_InNum3.Text = num.ToString();
            lbl_InNum4.Text = num.ToString();
            lbl_InNum5.Text = num.ToString();
            lbl_InNum6.Text = num.ToString();

            //string tempdate1 = String.Format("{0:MM/dd/yyyy}", model.TemFlightSta);
            //if (model.TemFlightTransit != "")
            //    tempdate1 = String.Format("{0:MM/dd/yyyy}", model.TemFlightSta2);
            //string tempdate2 = String.Format("{0:MM/dd/yyyy}", model.TemFlightStd);
            //lblDateDepart.Text = String.Format("{0:dddd, dd MMMM yyyy}", model.TemFlightStd);
            //TimeSpan ts = Convert.ToDateTime(tempdate1) - Convert.ToDateTime(tempdate2);
            //string temp = "";
            //if (ts.Days > 0)
            //{
            //    if (ts.Days == 1) temp = " (+" + ts.TotalDays.ToString() + " day)"; else temp = " (+" + ts.TotalDays.ToString() + " days)";
            //}
            //if (model.TemFlightTransit != "")
            //    lbl_ArrivalDateout.Text = String.Format("{0:HHmm}", model.TemFlightSta2) + temp;
            //else
            //    lbl_ArrivalDateout.Text = String.Format("{0:HHmm}", model.TemFlightSta) + temp;

            //lbl_ArrivalOut.Text = objGeneral.GetCityNameByCode(model.TemFlightArrival) + "(" + model.TemFlightArrival + ")";            

            //lbl_CarrierCodeOut.Text = model.TemFlightCarrierCode;
            lbl_ChildNumout.Text = model.TemFlightCHDNum.ToString();

            //lbl_DepartureDateout.Text = String.Format("{0:HHmm}", model.TemFlightStd);
            //lbl_Departureout.Text = objGeneral.GetCityNameByCode(model.TemFlightDeparture) + "(" + model.TemFlightDeparture + ")"; 

            //lbl_FlightnumberOut.Text = model.TemFlightFlightNumber;
            lbl_GuestNumout.Text = model.TemFlightADTNum.ToString();
            lbl_PaxNumout.Text = model.TemFlightPaxNum.ToString();

            ///amended by Diana,
            ///added divide with num of passenger to show single amount

            //breakdown charge and tax
            DataTable dtBDFee = objBooking.dtBreakdownFee();
            dtBDFee = (DataTable)HttpContext.Current.Session["dataBDFeeDepart"];

            if (dtBDFee != null && dtBDFee.Rows.Count > 0)
            {
                lblPaxFareDepart.Text = Convert.ToDecimal(dtBDFee.Rows[0]["Fare"]).ToString("N", nfi);

                lblTaxDepart.Text = Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]).ToString("N", nfi);
                lblPaxFeeDepart.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["PaxFee"]) / model.TemFlightPaxNum).ToString("N", nfi);
                if (Convert.ToInt32(model.TemFlightCHDNum.ToString()) != 0) // && (model.TemFlightDeparture.ToString().ToUpper() == "HKG" || model.TemFlightArrival.ToString().ToUpper() == "HKG"))
                {

                    lblTaxDepartChild.Text = Convert.ToDecimal(dtBDFee.Rows[0]["ChApt"]).ToString("N", nfi);
                    lblTaxDepart.Text = objGeneral.RoundUp(((Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * Convert.ToDecimal(model.TemFlightADTNum)) + (Convert.ToDecimal(dtBDFee.Rows[0]["ChApt"]) * Convert.ToDecimal(model.TemFlightCHDNum))) / (Convert.ToDecimal(model.TemFlightADTNum) + Convert.ToDecimal(model.TemFlightCHDNum))).ToString("N", nfi);
                    lbl_taxTotalCHD.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["ChApt"]) * Convert.ToDecimal(model.TemFlightCHDNum)).ToString("N", nfi);
                }
                else
                {
                    lbl_taxTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * num).ToString("N", nfi);
                }
                //HttpCookie cookie2 = Request.Cookies["cookieSearchcondition"];
                //if (cookie2 != null)
                //{
                //    if (cookie2.Values["InfantNum"] != "")
                //    {
                //        HttpCookie cookie3 = Request.Cookies["AllPax"];
                //        if (cookie3 != null)
                //        {
                //            //if (Convert.ToDecimal(dtBDFee.Rows[0]["Infant"]) != 0)
                //            //{
                //            //    //lblInfantDepart.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Infant"]) / Convert.ToDecimal(cookie3.Values["InfantNum"])).ToString("N", nfi);
                //            //    lblInfantDepart.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Infant"]) / model.TemFlightPaxNum).ToString("N", nfi);
                //            //    lbl_currency8.Text = model.TemFlightCurrencyCode.ToString();
                //            //}
                //            //else
                //            //{
                //            //    trInfant.Style.Add("display", "none");
                //            //}
                //        }
                //        else
                //        {
                //            trInfant.Style.Add("display", "none");
                //        }
                //    }
                //    else
                //    {
                //        trInfant.Style.Add("display", "none");
                //    }
                //}

                lblFuelDepart.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"]) / model.TemFlightPaxNum).ToString("N", nfi);
                lblSvcDepart.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Svc"]) / model.TemFlightPaxNum).ToString("N", nfi);
                lblOthDepart.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) / model.TemFlightPaxNum + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"]) / model.TemFlightPaxNum).ToString("N", nfi);
                lblPromoDiscDepart.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"]) / model.TemFlightPaxNum).ToString("N", nfi);
                //added by ketee
                lblVATDepart.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) / model.TemFlightPaxNum).ToString("N", nfi);

                lblFuelPriceTotalDepart.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"])).ToString("N", nfi);
                lblSvcChargeTotalDepart.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Svc"])).ToString("N", nfi);
                lblOthTotalDepart.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"])).ToString("N", nfi);
                lblPromoDiscTotalDepart.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"])).ToString("N", nfi);
                //added by ketee
                lblVATDepart.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) / num).ToString("N", nfi);
                lblVATTotalDepart.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["VAT"])).ToString("N", nfi);

                if (Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"]) < 0)
                {
                    //trPromoDisc.Visible = true;
                }
                else
                {
                    //trPromoDisc.Visible = false;
                }


                if (Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) > 0)
                {
                    //trVAT.Style["display"] = "";
                }

                if (decimal.Parse(lblSvcDepart.Text) <= 0)
                {
                    freeSVCF = true;
                }
            }
            //if (model.TemFlightTransit != "")
            //{
            //    tempdate1 = String.Format("{0:MM/dd/yyyy}", model.TemFlightSta2);
            //    tempdate2 = String.Format("{0:MM/dd/yyyy}", model.TemFlightStd2);
            //    ts = Convert.ToDateTime(tempdate1) - Convert.ToDateTime(tempdate2);

            //    temp = "";
            //    if (ts.Days > 0)
            //        temp = " " + ts.TotalDays.ToString() + " day";

            //    string tempDateStd = String.Format("{0:HHmm}", model.TemFlightStd2);
            //    string tempDateSta = String.Format("{0:HHmm}", model.TemFlightSta2) + temp;
            //    string transitAt = objGeneral.GetCityNameByCode(model.TemFlightTransit);
            //    LblTransitDepart.Text = "Transit At " + transitAt + " (" + tempDateStd + " - " + tempDateSta + ")" + " Flight " + model.TemFlightCarrierCode2 + model.TemFlightFlightNumber2 ; 
            //}



            total = model.TemFlightTotalAmount;

            lblAverageFare.Text = (objGeneral.RoundUp(total / model.TemFlightPaxNum)).ToString("N", nfi);

            if (InID != "")
            {
                //tdreturnfare.Visible = true;
                //lblReturnFareText.Visible = true;  
                lblReturnFare.Visible = true;
                lblReturnCurrency.Visible = true;
                //td_Return.Visible = true;  
                lblPaxFareReturn.Visible = true;
                //lblTextPaxFareReturn.Visible = true;  
                //lblTextTaxFareReturn.Visible = true;  
                lblTaxReturn.Visible = true;
                lblPaxFeeReturn.Visible = true;
                lblTaxReturnChild.Visible = true;
                //lblTextTaxFareReturnChild.Visible = true;  
                //lblTextFuelReturn.Visible = true;  
                lblFuelReturn.Visible = true;
                //lblTextSvcReturn.Visible = true;  
                lblSvcReturn.Visible = true;
                //lblTextOthReturn.Visible = true;
                lblOthReturn.Visible = true;
                lblPromoDiscReturn.Visible = true;

                model2 = GetTemFlight(InID);

                //if (model2.TemFlightTransit != "")
                //    tempdate1 = String.Format("{0:MM/dd/yyyy}", model2.TemFlightSta2);
                //else
                //    tempdate1 = String.Format("{0:MM/dd/yyyy}", model2.TemFlightSta);
                //tempdate2 = String.Format("{0:MM/dd/yyyy}", model2.TemFlightStd);
                //ts = Convert.ToDateTime(tempdate1) - Convert.ToDateTime(tempdate2);
                //lblDateReturn.Text = String.Format("{0:dddd, dd MMMM yyyy}", model2.TemFlightStd);
                //temp = "";
                //if (ts.Days > 0)
                //{
                //    if (ts.Days == 1) temp = " (+" + ts.TotalDays.ToString() + " day)"; else temp = " (+" + ts.TotalDays.ToString() + " days)";
                //}
                //if (model2.TemFlightTransit != "")
                //    lbl_ArrivalDateIN.Text = String.Format("{0:HHmm}", model2.TemFlightSta2) + temp;
                //else
                //    lbl_ArrivalDateIN.Text = String.Format("{0:HHmm}", model2.TemFlightSta) + temp;

                //lbl_ArrivalIN.Text = objGeneral.GetCityNameByCode(model2.TemFlightArrival) + "(" + model2.TemFlightArrival + ")";               

                //lbl_CarrierCodeIN.Text = model2.TemFlightCarrierCode;
                //lbl_ChildNumIN.Text = model2.TemFlightCHDNum.ToString();                

                //lbl_DepartureDateIN.Text = String.Format("{0:HHmm}", model2.TemFlightStd);

                //lbl_DepartureIN.Text = objGeneral.GetCityNameByCode(model2.TemFlightDeparture) + "(" + model2.TemFlightDeparture + ")"; 

                //lbl_FlightnumberIN.Text = model2.TemFlightFlightNumber;
                //lbl_GuestNumIN.Text = model2.TemFlightADTNum.ToString();
                //lbl_PaxNumIN.Text = model2.TemFlightPaxNum.ToString();

                ///amended by Diana,
                ///added divide with num of passenger to show single amount

                //breakdown tax and charge                
                dtBDFee = (DataTable)HttpContext.Current.Session["dataBDFeeReturn"];

                lblPaxFareReturn.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Fare"])).ToString("N", nfi);

                lblTaxReturn.Text = Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]).ToString("N", nfi);
                lblPaxFeeReturn.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["PaxFee"]) / model2.TemFlightPaxNum).ToString("N", nfi);
                if (Convert.ToInt32(model2.TemFlightCHDNum.ToString()) != 0)
                {
                    lblTaxReturnChild.Text = Convert.ToDecimal(dtBDFee.Rows[0]["ChApt"]).ToString("N", nfi);
                    lblTaxReturn.Text = objGeneral.RoundUp(((Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * Convert.ToDecimal(model.TemFlightADTNum)) + (Convert.ToDecimal(dtBDFee.Rows[0]["ChApt"]) * Convert.ToDecimal(model.TemFlightCHDNum))) / (Convert.ToDecimal(model.TemFlightADTNum) + Convert.ToDecimal(model.TemFlightCHDNum))).ToString("N", nfi);
                }
                //HttpCookie cookies = Request.Cookies["cookieSearchcondition"];
                //if (cookies != null)
                //{
                //    if (cookies.Values["InfantNum"] != "")
                //    {
                //        HttpCookie cookie3 = Request.Cookies["AllPax"];
                //        if (cookie3 != null)
                //        {
                //            if (Convert.ToDecimal(dtBDFee.Rows[0]["Infant"]) != 0)
                //            {
                //                //lblInfantReturn.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Infant"]) / Convert.ToDecimal(cookie3.Values["InfantNum"])).ToString("N", nfi);
                //                lblInfantReturn.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Infant"]) / model2.TemFlightPaxNum).ToString("N", nfi);
                //                lbl_InCurrency8.Text = model2.TemFlightCurrencyCode.ToString();
                //            }
                //            else
                //            {
                //                trInfant.Style.Add("display", "none");
                //            }
                //        }
                //        else
                //        {
                //            trInfant.Style.Add("display", "none");
                //        }
                //    }
                //    else
                //    {
                //        trInfant.Style.Add("display", "none");
                //    }
                //}
                lblFuelReturn.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"]) / model2.TemFlightPaxNum).ToString("N", nfi);
                lblSvcReturn.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Svc"]) / model2.TemFlightPaxNum).ToString("N", nfi);
                lblOthReturn.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) / model2.TemFlightPaxNum + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"]) / model2.TemFlightPaxNum).ToString("N", nfi);
                lblPromoDiscReturn.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"]) / model2.TemFlightPaxNum).ToString("N", nfi);
                //added byektee
                lblVATReturn.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) / model2.TemFlightPaxNum).ToString("N", nfi);
                lblVATTotalReturn.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["VAT"])).ToString("N", nfi);

                lbl_InCurrency0.Text = model2.TemFlightCurrencyCode.ToString();
                lbl_Incurrency1.Text = model2.TemFlightCurrencyCode.ToString();
                lbl_Incurrency2.Text = model2.TemFlightCurrencyCode.ToString();
                lbl_Incurrency21.Text = model2.TemFlightCurrencyCode.ToString();
                lbl_InCurrency3.Text = model2.TemFlightCurrencyCode.ToString();
                lbl_InCurrency31.Text = model2.TemFlightCurrencyCode.ToString();
                if (Convert.ToDecimal(model2.TemFlightCHDNum) != 0) // && (model2.TemFlightDeparture.ToString().ToUpper() == "HKG" || model2.TemFlightArrival.ToString().ToUpper() == "HKG"))
                {
                    lbl_Incurrency2CHD.Text = model2.TemFlightCurrencyCode.ToString();
                    lbl_InCurrency3CHD.Text = model2.TemFlightCurrencyCode.ToString();
                    lbl_IntaxTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * Convert.ToDecimal(model.TemFlightADTNum)).ToString("N", nfi);
                    lbl_IntaxTotalCHD.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["ChApt"]) * Convert.ToDecimal(model.TemFlightCHDNum)).ToString("N", nfi);
                }
                else
                {

                    lbl_IntaxTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * num).ToString("N", nfi);
                }
                lbl_InPaxFeeTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["PaxFee"]) * num).ToString("N", nfi);

                lbl_InCurrency4.Text = model2.TemFlightCurrencyCode.ToString();
                lbl_InCurrency5.Text = model2.TemFlightCurrencyCode.ToString();
                lbl_InCurrency6.Text = model2.TemFlightCurrencyCode.ToString();
                lbl_InCurrency7.Text = model2.TemFlightCurrencyCode.ToString();
                lblCurrFuelReturn.Text = model2.TemFlightCurrencyCode.ToString();
                lblCurrOthReturn.Text = model2.TemFlightCurrencyCode.ToString();
                lblCurrSvcReturn.Text = model2.TemFlightCurrencyCode.ToString();
                lblCurrVATReturn.Text = model2.TemFlightCurrencyCode.ToString();
                lblCurrPromoDiscReturn.Text = model2.TemFlightCurrencyCode.ToString();
                lbl_InCurrency9.Text = model2.TemFlightCurrencyCode.ToString();

                lbl_Incurrency2.Text = model2.TemFlightCurrencyCode;
                lbl_Incurrency21.Text = model2.TemFlightCurrencyCode;
                lblCurrAVLReturn.Text = model2.TemFlightCurrencyCode;
                lblCurrPSFReturn.Text = model2.TemFlightCurrencyCode;
                lblCurrSCFReturn.Text = model2.TemFlightCurrencyCode;
                lblCurrConnectingReturn.Text = model2.TemFlightCurrencyCode;
                lblCurrDiscReturn.Text = model2.TemFlightCurrencyCode;
                lblCurrKlia2Return.Text = model2.TemFlightCurrencyCode;
                lblCurrGSTReturn.Text = model2.TemFlightCurrencyCode;
                lblCurrSPLReturn.Text = model2.TemFlightCurrencyCode;
                lblCurrAPSReturn.Text = model2.TemFlightCurrencyCode;
                lblCurrIADFReturn.Text = model2.TemFlightCurrencyCode;
                lblCurrACFReturn.Text = model2.TemFlightCurrencyCode;
                lblCurrCSTReturn.Text = model2.TemFlightCurrencyCode;
                lblCurrCUTReturn.Text = model2.TemFlightCurrencyCode;
                lblCurrSGIReturn.Text = model2.TemFlightCurrencyCode;
                lblCurrSSTReturn.Text = model2.TemFlightCurrencyCode;
                lblCurrUDFReturn.Text = model2.TemFlightCurrencyCode;
                lblCurrASCReturn.Text = model2.TemFlightCurrencyCode;
                lblCurrBCLReturn.Text = model2.TemFlightCurrencyCode;
                lblCurrIWJRReturn.Text = model2.TemFlightCurrencyCode;
                lblCurrVATChargeReturn.Text = model2.TemFlightCurrencyCode;

                lblFuelTotalReturn.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"])).ToString("N", nfi);
                lblSvcTotalReturn.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Svc"])).ToString("N", nfi);
                lblOthTotalReturn.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"])).ToString("N", nfi);
                lblPromoDiscTotalReturn.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"])).ToString("N", nfi);


                lbl_InFlightTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(model2.temFlightfarePrice) * model2.TemFlightPaxNum).ToString("N", nfi);

                if (freeSVCF)
                {
                    if (decimal.Parse(lblSvcReturn.Text) > 0)
                    {
                        freeSVCF = false;
                    }
                }

                //if (model2.TemFlightTransit != "")
                //{
                //    tempdate1 = String.Format("{0:MM/dd/yyyy}", model2.TemFlightSta2);
                //    tempdate2 = String.Format("{0:MM/dd/yyyy}", model2.TemFlightStd2);
                //    ts = Convert.ToDateTime(tempdate1) - Convert.ToDateTime(tempdate2);

                //    temp = "";
                //    if (ts.Days > 0)
                //        temp = " " + ts.TotalDays.ToString() + " day";

                //    string tempDateStd = String.Format("{0:HHmm}", model2.TemFlightStd2);
                //    string tempDateSta = String.Format("{0:HHmm}", model2.TemFlightSta2) + temp;
                //    string transitAt = objGeneral.GetCityNameByCode(model2.TemFlightTransit);
                //    LblTransitReturn.Text = "Transit At " + transitAt + " (" + tempDateStd + " - " + tempDateSta + ")" + " Flight " + model2.TemFlightCarrierCode2 + model2.TemFlightFlightNumber2;
                //}

                //model2.TemFlightTotalAmount = totAmountReturn;
                //total += model2.TemFlightTotalAmount;
                total += model2.TemFlightTotalAmount;

                lblAverageFare.Text = objGeneral.RoundUp(total / model.TemFlightPaxNum).ToString("N", nfi);
            }
            else
            {
                //tr_Return.Visible = false;
                //td_Return.Visible = false;
                lblPaxFareReturn.Visible = false;
                //lblTextPaxFareReturn.Visible = false;
                //lblTextTaxFareReturn.Visible = false;
                lblTaxReturn.Visible = false;
                lblPaxFeeReturn.Visible = false;
                //lblTextTaxFareReturnChild.Visible = false;
                lblTaxReturnChild.Visible = false;
                //lblTextFuelReturn.Visible = false;
                lblFuelReturn.Visible = false;
                //lblTextSvcReturn.Visible = false;
                lblSvcReturn.Visible = false;
                //lblTextOthReturn.Visible = false;
                lblOthReturn.Visible = false;
                //lblReturnFareText.Visible = false;
                lblReturnFare.Visible = false;
                lblReturnCurrency.Visible = false;
                //lblTextVATReturn.Visible = false;
                lblVATReturn.Visible = false;
                lblPromoDiscReturn.Visible = false;
            }


            lblTotalFare.Text = objGeneral.RoundUp(total).ToString("N", nfi);
            lblTotalCurrency.Text = model.TemFlightCurrencyCode;
            lblTotFareCurrency.Text = model.TemFlightCurrencyCode;

            //20170530 - Sienny (put amount due to session)
            Session["TotalAmountDue"] = lblTotalFare.Text;
            Session["TotalAmountDueCurr"] = lblTotFareCurrency.Text;

            //remarked by diana 20170201, no more service fee
            //if (freeSVCF)
            //{
            //    log.Info(this,"Depart SVCF : " + lblSvcDepart.Text + "; Return SVCF : " + lblSvcReturn.Text);
            //    Session["SVCFAvailable"] = "false";
            //    Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
            //}

            HttpContext.Current.Session.Remove("Fare");
            Hashtable htFare = new Hashtable();
            htFare.Add("Avg", lblAverageFare.Text);
            htFare.Add("Dpt", model.TemFlightTotalAmount);
            htFare.Add("Rtn", model2.TemFlightTotalAmount);
            HttpContext.Current.Session.Add("Fare", htFare);


            //string strExpr;
            //string strSort;
            //DataTable dt = objBooking.dtFlight();
            //dt = (DataTable)HttpContext.Current.Session["TempFlight"];

            //strExpr = "TemFlightId = '" + OutID + "'";
            //strSort = "";
            //DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
            //foundRows[0]["TemFlightTotalAmount"] = model.TemFlightTotalAmount;
            //HttpContext.Current.Session["TempFlight"] = dt;

            //dt = (DataTable)HttpContext.Current.Session["TempFlight"];
            //strExpr = "TemFlightId = '" + InID + "'";
            //strSort = "";
            //DataRow[] foundRowsIn = dt.Select(strExpr, strSort, DataViewRowState.Added);
            //foundRows[0]["TemFlightTotalAmount"] = model.TemFlightTotalAmount;
            //HttpContext.Current.Session["TempFlight"] = dt;

        }

        //20170721 - Sienny (retrieve all fees from db to show it)
        protected void BindFeesData()
        {
            ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
            ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model2 = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();

            model = GetTemFlight(departIDTem);

            //if (Session["dtTransFee"] != null)
            //    dtDataFeeCode = (DataTable)Session["dtTransFee"];
            dtDataFeeCode = objGeneral.GetAllFeesData(tranID);
            DataTable dtDataFeeCodeCopy = new DataTable();

            decimal OthDepart = 0;
            decimal OthReturn = 0;
            decimal klia2TotalDepart = 0, GSTTotalDepart = 0, AVLTotalDepart = 0, PSFTotalDepart = 0, SCFTotalDepart = 0, ConnectingDepartTotal = 0, DiscTotalDepart = 0, SPLTotalDepart = 0, APSTotalDepart = 0, IADFTotalDepart = 0, ACFTotalDepart = 0, CSTTotalDepart = 0, CUTTotalDepart = 0, SGITotalDepart = 0, SSTTotalDepart = 0, UDFTotalDepart = 0, ASCTotalDepart = 0, BCLTotalDepart = 0, IWJRTotalDepart = 0, VATTotalDepart = 0;
            decimal klia2TotalReturn = 0, GSTTotalReturn = 0, AVLTotalReturn = 0, PSFTotalReturn = 0, SCFTotalReturn = 0, ConnectingReturnTotal = 0, DiscTotalReturn = 0, SPLTotalReturn = 0, APSTotalReturn = 0, IADFTotalReturn = 0, ACFTotalReturn = 0, CSTTotalReturn = 0, CUTTotalReturn = 0, SGITotalReturn = 0, SSTTotalReturn = 0, UDFTotalReturn = 0, ASCTotalReturn = 0, BCLTotalReturn = 0, IWJRTotalReturn = 0, VATTotalReturn = 0;
            if (dtDataFeeCode != null && dtDataFeeCode.Rows.Count > 0)
            {
                DataRow[] rowsFeeCode = dtDataFeeCode.Select("Origin = '" + model.TemFlightDeparture + "'");
                if (rowsFeeCode.Length > 0)
                {
                    dtDataFeeCodeCopy = dtDataFeeCode.Select("Origin = '" + model.TemFlightDeparture + "'").CopyToDataTable();
                    rptFeeDepart.DataSource = dtDataFeeCodeCopy;
                    rptFeeDepart.DataBind();
                    foreach (RepeaterItem item in rptFeeDepart.Items)
                    {
                        Label lblFeeCurrDepart = item.FindControl("lblFeeCurrDepart") as Label;
                        lblFeeCurrDepart.Text = model.TemFlightCurrencyCode;
                    }
                }

                if (returnIDTem != "")
                {
                    model2 = GetTemFlight(returnIDTem);
                    rowsFeeCode = dtDataFeeCode.Select("Origin = '" + model2.TemFlightDeparture + "'");
                    if (rowsFeeCode.Length > 0)
                    {
                        dtDataFeeCodeCopy = dtDataFeeCode.Select("Origin = '" + model2.TemFlightDeparture + "'").CopyToDataTable();
                        rptFeeReturn.DataSource = dtDataFeeCodeCopy;
                        rptFeeReturn.DataBind();
                        foreach (RepeaterItem item in rptFeeReturn.Items)
                        {
                            Label lblFeeCurrReturn = item.FindControl("lblFeeCurrReturn") as Label;
                            lblFeeCurrReturn.Text = model2.TemFlightCurrencyCode;
                        }
                    }
                }

                //for (int i = 0; i < dtDataFeeCode.Rows.Count; i++)
                //{
                //    DataRow rowFeeCode = dtDataFeeCode.Select("FeeCode='" + dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() + "'").FirstOrDefault();
                //    if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "AVL" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model.TemFlightDeparture.ToString())
                //    {
                //        if (rowFeeCode != null) lblAVLInfoDepart.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //        //tdDepartAVL.Visible = true;
                //        AVLTotalDepart = (Convert.ToDecimal(AVLTotalDepart) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //        OthDepart += Convert.ToDecimal(AVLTotalDepart);
                //    }
                //    else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "" && dtDataFeeCode.Rows[i]["FeeType"].ToString() == "ConnectionAdjustmentAmount" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model.TemFlightDeparture.ToString())
                //    {
                //        //tdConnectingDepart.Visible = true;
                //        ConnectingDepartTotal = (Convert.ToDecimal(ConnectingDepartTotal) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //        //OthDepart += Convert.ToDecimal(ConnectingDepartTotal);
                //    }
                //    else if (dtDataFeeCode.Rows[i]["FeeType"].ToString().ToUpper() == "DISCOUNT")
                //    {
                //        //tdDepartSCF.Visible = true;
                //        DiscTotalDepart = (Convert.ToDecimal(DiscTotalDepart) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //        //OthDepart += Convert.ToDecimal(DiscTotalDepart);
                //    }
                //    else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "APS" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model.TemFlightDeparture.ToString())
                //    {
                //        if (rowFeeCode != null) lblAPSInfoDepart.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //        //tdDepartSCF.Visible = true;
                //        APSTotalDepart = (Convert.ToDecimal(APSTotalDepart) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //        OthDepart += Convert.ToDecimal(APSTotalDepart);
                //    }
                //    else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "ACF" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model.TemFlightDeparture.ToString())
                //    {
                //        if (rowFeeCode != null) lblACFInfoDepart.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //        //tdDepartSCF.Visible = true;
                //        ACFTotalDepart = (Convert.ToDecimal(ACFTotalDepart) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //        OthDepart += Convert.ToDecimal(ACFTotalDepart);
                //    }
                //    else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "ASC" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model.TemFlightDeparture.ToString())
                //    {
                //        if (rowFeeCode != null) lblASCInfoDepart.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //        //tdDepartSCF.Visible = true;
                //        ASCTotalDepart = (Convert.ToDecimal(ASCTotalDepart) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //        OthDepart += Convert.ToDecimal(ASCTotalDepart);
                //    }
                //    else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "BCL" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model.TemFlightDeparture.ToString())
                //    {
                //        if (rowFeeCode != null) lblBCLInfoDepart.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //        //tdDepartSCF.Visible = true;
                //        BCLTotalDepart = (Convert.ToDecimal(BCLTotalDepart) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //        OthDepart += Convert.ToDecimal(BCLTotalDepart);
                //    }
                //    else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "CGST" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model.TemFlightDeparture.ToString())
                //    {
                //        if (rowFeeCode != null) lblCSTInfoDepart.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //        //tdDepartSCF.Visible = true;
                //        CSTTotalDepart = (Convert.ToDecimal(CSTTotalDepart) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //        OthDepart += Convert.ToDecimal(CSTTotalDepart);
                //    }
                //    else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "CUTE" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model.TemFlightDeparture.ToString())
                //    {
                //        if (rowFeeCode != null) lblCUTInfoDepart.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //        //tdDepartSCF.Visible = true;
                //        CUTTotalDepart = (Convert.ToDecimal(CUTTotalDepart) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //        OthDepart += Convert.ToDecimal(CUTTotalDepart);
                //    }
                //    else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "GST" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model.TemFlightDeparture.ToString())
                //    {
                //        if (rowFeeCode != null) lblGSTInfoDepart.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //        //tdDepartGST.Visible = true;
                //        GSTTotalDepart = (Convert.ToDecimal(GSTTotalDepart) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //        OthDepart += Convert.ToDecimal(GSTTotalDepart);
                //    }
                //    else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "IADF" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model.TemFlightDeparture.ToString())
                //    {
                //        if (rowFeeCode != null) lblIADFInfoDepart.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //        //tdDepartSCF.Visible = true;
                //        IADFTotalDepart = (Convert.ToDecimal(IADFTotalDepart) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //        OthDepart += Convert.ToDecimal(IADFTotalDepart);
                //    }
                //    else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "IWJR" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model.TemFlightDeparture.ToString())
                //    {
                //        if (rowFeeCode != null) lblIWJRInfoDepart.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //        //tdDepartSCF.Visible = true;
                //        IWJRTotalDepart = (Convert.ToDecimal(IWJRTotalDepart) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //        OthDepart += Convert.ToDecimal(IWJRTotalDepart);
                //    }
                //    else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "KLIA2" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model.TemFlightDeparture.ToString())
                //    {
                //        if (rowFeeCode != null) lblKLIA2InfoDepart.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //        //tdDepartKlia2.Visible = true;
                //        klia2TotalDepart = (Convert.ToDecimal(klia2TotalDepart) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //        OthDepart += Convert.ToDecimal(klia2TotalDepart);
                //    }
                //    else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "PSF" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model.TemFlightDeparture.ToString())
                //    {
                //        if (rowFeeCode != null) lblPSFInfoDepart.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //        //tdDepartPSF.Visible = true;
                //        PSFTotalDepart = (Convert.ToDecimal(PSFTotalDepart) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //        OthDepart += Convert.ToDecimal(PSFTotalDepart);
                //    }
                //    else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "SCF" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model.TemFlightDeparture.ToString())
                //    {
                //        if (rowFeeCode != null) lblSCFInfoDepart.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //        //tdDepartSCF.Visible = true;
                //        SCFTotalDepart = (Convert.ToDecimal(SCFTotalDepart) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //        OthDepart += Convert.ToDecimal(SCFTotalDepart);
                //    }
                //    else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "SGI" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model.TemFlightDeparture.ToString())
                //    {
                //        if (rowFeeCode != null) lblSGIInfoDepart.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //        //tdDepartSCF.Visible = true;
                //        SGITotalDepart = (Convert.ToDecimal(SGITotalDepart) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //        OthDepart += Convert.ToDecimal(SGITotalDepart);
                //    }
                //    else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "SGST" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model.TemFlightDeparture.ToString())
                //    {
                //        if (rowFeeCode != null) lblSSTInfoDepart.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //        //tdDepartSCF.Visible = true;
                //        SSTTotalDepart = (Convert.ToDecimal(SSTTotalDepart) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //        OthDepart += Convert.ToDecimal(SSTTotalDepart);
                //    }
                //    else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "SPL" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model.TemFlightDeparture.ToString())
                //    {
                //        if (rowFeeCode != null) lblSPLInfoDepart.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //        //tdDepartSCF.Visible = true;
                //        SPLTotalDepart = (Convert.ToDecimal(SPLTotalDepart) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //        OthDepart += Convert.ToDecimal(SPLTotalDepart);
                //    }
                //    else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "UDF" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model.TemFlightDeparture.ToString())
                //    {
                //        if (rowFeeCode != null) lblUDFInfoDepart.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //        //tdDepartSCF.Visible = true;
                //        UDFTotalDepart = (Convert.ToDecimal(UDFTotalDepart) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //        OthDepart += Convert.ToDecimal(UDFTotalDepart);
                //    }
                //    else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "VAT" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model.TemFlightDeparture.ToString())
                //    {
                //        if (rowFeeCode != null) lblVATInfoDepart.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //        //tdDepartSCF.Visible = true;
                //        VATTotalDepart = (Convert.ToDecimal(VATTotalDepart) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //        OthDepart += Convert.ToDecimal(VATTotalDepart);
                //    }

                //    else if (returnIDTem != "")
                //    {
                //        model2 = GetTemFlight(returnIDTem);

                //        if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "KLIA2" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model2.TemFlightDeparture.ToString())
                //        {
                //            if (rowFeeCode != null) lblKLIA2InfoReturn.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //            //tdReturnKlia2.Visible = true;
                //            klia2TotalReturn = (Convert.ToDecimal(klia2TotalReturn) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //            OthReturn += Convert.ToDecimal(klia2TotalReturn);
                //        }
                //        else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "GST" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model2.TemFlightDeparture.ToString())
                //        {
                //            if (rowFeeCode != null) lblGSTInfoReturn.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //            //tdReturnGST.Visible = true;
                //            GSTTotalReturn = (Convert.ToDecimal(GSTTotalReturn) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //            OthReturn += Convert.ToDecimal(GSTTotalReturn);
                //        }
                //        else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "AVL" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model2.TemFlightDeparture.ToString())
                //        {
                //            if (rowFeeCode != null) lblAVLInfoReturn.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //            //tdReturnAVL.Visible = true;
                //            AVLTotalReturn = (Convert.ToDecimal(AVLTotalReturn) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //            OthReturn += Convert.ToDecimal(AVLTotalReturn);
                //        }
                //        else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "PSF" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model2.TemFlightDeparture.ToString())
                //        {
                //            if (rowFeeCode != null) lblPSFInfoReturn.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //            //tdReturnPSF.Visible = true;
                //            PSFTotalReturn = (Convert.ToDecimal(PSFTotalReturn) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //            OthReturn += Convert.ToDecimal(PSFTotalReturn);
                //        }
                //        else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "SCF" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model2.TemFlightDeparture.ToString())
                //        {
                //            if (rowFeeCode != null) lblSCFInfoReturn.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //            //tdReturnSCF.Visible = true;
                //            SCFTotalReturn = (Convert.ToDecimal(SCFTotalReturn) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //            OthReturn += Convert.ToDecimal(SCFTotalReturn);
                //        }
                //        else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "CGST" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model2.TemFlightDeparture.ToString())
                //        {
                //            if (rowFeeCode != null) lblCSTInfoReturn.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //            //tdDepartSCF.Visible = true;
                //            CSTTotalReturn = (Convert.ToDecimal(CSTTotalReturn) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //            OthReturn += Convert.ToDecimal(CSTTotalReturn);
                //        }
                //        else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "CUTE" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model2.TemFlightDeparture.ToString())
                //        {
                //            if (rowFeeCode != null) lblCUTInfoReturn.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //            //tdDepartSCF.Visible = true;
                //            CUTTotalReturn = (Convert.ToDecimal(CUTTotalReturn) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //            OthReturn += Convert.ToDecimal(CUTTotalReturn);
                //        }
                //        else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "SGI" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model2.TemFlightDeparture.ToString())
                //        {
                //            if (rowFeeCode != null) lblSGIInfoReturn.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //            //tdDepartSCF.Visible = true;
                //            SGITotalReturn = (Convert.ToDecimal(SGITotalReturn) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //            OthReturn += Convert.ToDecimal(SGITotalReturn);
                //        }
                //        else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "SGST" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model2.TemFlightDeparture.ToString())
                //        {
                //            if (rowFeeCode != null) lblSSTInfoReturn.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //            //tdDepartSCF.Visible = true;
                //            SSTTotalReturn = (Convert.ToDecimal(SSTTotalReturn) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //            OthReturn += Convert.ToDecimal(SSTTotalReturn);
                //        }
                //        else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "UDF" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model2.TemFlightDeparture.ToString())
                //        {
                //            if (rowFeeCode != null) lblUDFInfoReturn.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //            //tdDepartSCF.Visible = true;
                //            UDFTotalReturn = (Convert.ToDecimal(UDFTotalReturn) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //            OthReturn += Convert.ToDecimal(UDFTotalReturn);
                //        }
                //        else if (i != 0 && dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "" && dtDataFeeCode.Rows[i]["FeeType"].ToString() == "ConnectionAdjustmentAmount" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model2.TemFlightDeparture.ToString())
                //        {
                //            //tdConnectingReturn.Visible = true;
                //            ConnectingReturnTotal = (Convert.ToDecimal(ConnectingReturnTotal) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //            //OthReturn += Convert.ToDecimal(ConnectingReturnTotal);
                //        }
                //        else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "DISCOUNT" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model2.TemFlightDeparture.ToString())
                //        {
                //            //tdDepartSCF.Visible = true;
                //            DiscTotalReturn = (Convert.ToDecimal(DiscTotalReturn) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //            //OthReturn += Convert.ToDecimal(DiscTotalReturn);
                //        }
                //        else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "SPL" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model2.TemFlightDeparture.ToString())
                //        {
                //            if (rowFeeCode != null) lblSPLInfoReturn.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //            //tdDepartSCF.Visible = true;
                //            SPLTotalReturn = (Convert.ToDecimal(SPLTotalReturn) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //            OthReturn += Convert.ToDecimal(SPLTotalReturn);
                //        }
                //        else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "APS" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model2.TemFlightDeparture.ToString())
                //        {
                //            if (rowFeeCode != null) lblAPSInfoReturn.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //            //tdDepartSCF.Visible = true;
                //            APSTotalReturn = (Convert.ToDecimal(APSTotalReturn) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //            OthReturn += Convert.ToDecimal(APSTotalReturn);
                //        }
                //        else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "IADF" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model2.TemFlightDeparture.ToString())
                //        {
                //            if (rowFeeCode != null) lblIADFInfoReturn.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //            //tdDepartSCF.Visible = true;
                //            IADFTotalReturn = (Convert.ToDecimal(IADFTotalReturn) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //            OthReturn += Convert.ToDecimal(IADFTotalReturn);
                //        }
                //        else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "ACF" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model2.TemFlightDeparture.ToString())
                //        {
                //            if (rowFeeCode != null) lblACFInfoReturn.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //            //tdDepartSCF.Visible = true;
                //            ACFTotalReturn = (Convert.ToDecimal(ACFTotalReturn) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //            OthReturn += Convert.ToDecimal(ACFTotalReturn);
                //        }
                //        else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "ASC" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model2.TemFlightDeparture.ToString())
                //        {
                //            if (rowFeeCode != null) lblASCInfoReturn.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //            //tdDepartSCF.Visible = true;
                //            ASCTotalReturn = (Convert.ToDecimal(ASCTotalReturn) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //            OthReturn += Convert.ToDecimal(ASCTotalReturn);
                //        }
                //        else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "BCL" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model2.TemFlightDeparture.ToString())
                //        {
                //            if (rowFeeCode != null) lblBCLInfoReturn.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //            //tdDepartSCF.Visible = true;
                //            BCLTotalReturn = (Convert.ToDecimal(BCLTotalReturn) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //            OthReturn += Convert.ToDecimal(BCLTotalReturn);
                //        }
                //        else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "IWJR" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model2.TemFlightDeparture.ToString())
                //        {
                //            if (rowFeeCode != null) lblIWJRInfoReturn.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //            //tdDepartSCF.Visible = true;
                //            IWJRTotalReturn = (Convert.ToDecimal(IWJRTotalReturn) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //            OthReturn += Convert.ToDecimal(IWJRTotalReturn);
                //        }
                //        else if (dtDataFeeCode.Rows[i]["FeeCode"].ToString().ToUpper() == "VAT" && dtDataFeeCode.Rows[i]["Origin"].ToString() == model2.TemFlightDeparture.ToString())
                //        {
                //            if (rowFeeCode != null) lblVATInfoReturn.Text = rowFeeCode["CodeDesc"].ToString() + " Charge per Pax";

                //            //tdDepartSCF.Visible = true;
                //            VATTotalReturn = (Convert.ToDecimal(VATTotalReturn) + Convert.ToDecimal(dtDataFeeCode.Rows[i]["FeeAmt"]));
                //            OthReturn += Convert.ToDecimal(VATTotalReturn);
                //        }
                //    }
                //}
            }

            //lbl_klia2Total.Text = (klia2TotalDepart / num).ToString("N", nfi);
            //lbl_GSTTotal.Text = (GSTTotalDepart / num).ToString("N", nfi);
            //lblAVLTotalDepart.Text = (AVLTotalDepart / num).ToString("N", nfi);
            //lblPSFTotalDepart.Text = (PSFTotalDepart / num).ToString("N", nfi);
            //lblSCFTotalDepart.Text = (SCFTotalDepart / num).ToString("N", nfi);
            ////lblConnectingDepartTotal.Text = (ConnectingDepartTotal / num).ToString("N", nfi);
            ////lblDiscTotalDepart.Text = (DiscTotalDepart / num).ToString("N", nfi);
            //lblSPLTotalDepart.Text = (SPLTotalDepart / num).ToString("N", nfi);
            //lblAPSTotalDepart.Text = (APSTotalDepart / num).ToString("N", nfi);
            //lblIADFTotalDepart.Text = (IADFTotalDepart / num).ToString("N", nfi);
            //lblACFTotalDepart.Text = (ACFTotalDepart / num).ToString("N", nfi);
            //lblCSTTotalDepart.Text = (CSTTotalDepart / num).ToString("N", nfi);
            //lblCUTTotalDepart.Text = (CUTTotalDepart / num).ToString("N", nfi);
            //lblSGITotalDepart.Text = (SGITotalDepart / num).ToString("N", nfi);
            //lblSSTTotalDepart.Text = (SSTTotalDepart / num).ToString("N", nfi);
            //lblUDFTotalDepart.Text = (UDFTotalDepart / num).ToString("N", nfi);
            //lblASCTotalDepart.Text = (ASCTotalDepart / num).ToString("N", nfi);
            //lblBCLTotalDepart.Text = (BCLTotalDepart / num).ToString("N", nfi);
            //lblIWJRTotalDepart.Text = (IWJRTotalDepart / num).ToString("N", nfi);
            //lblVATChargeTotalDepart.Text = (VATTotalDepart / num).ToString("N", nfi);

            //lbl_Inklia2Total.Text = (klia2TotalReturn / num).ToString("N", nfi);
            //lbl_InGSTTotal.Text = (GSTTotalReturn / num).ToString("N", nfi);
            //lblAVLTotalReturn.Text = (AVLTotalReturn / num).ToString("N", nfi);
            //lblPSFTotalReturn.Text = (PSFTotalReturn / num).ToString("N", nfi);
            //lblSCFTotalReturn.Text = (SCFTotalReturn / num).ToString("N", nfi);
            ////lblConnectingReturnTotal.Text = (ConnectingReturnTotal / num).ToString("N", nfi);
            ////lblDiscTotalReturn.Text = (DiscTotalReturn / num).ToString("N", nfi);
            //lblSPLTotalReturn.Text = (SPLTotalReturn / num).ToString("N", nfi);
            //lblAPSTotalReturn.Text = (APSTotalReturn / num).ToString("N", nfi);
            //lblIADFTotalReturn.Text = (IADFTotalReturn / num).ToString("N", nfi);
            //lblACFTotalReturn.Text = (ACFTotalReturn / num).ToString("N", nfi);
            //lblCSTTotalReturn.Text = (CSTTotalReturn / num).ToString("N", nfi);
            //lblCUTTotalReturn.Text = (CUTTotalReturn / num).ToString("N", nfi);
            //lblSGITotalReturn.Text = (SGITotalReturn / num).ToString("N", nfi);
            //lblSSTTotalReturn.Text = (SSTTotalReturn / num).ToString("N", nfi);
            //lblUDFTotalReturn.Text = (UDFTotalReturn / num).ToString("N", nfi);
            //lblASCTotalReturn.Text = (ASCTotalReturn / num).ToString("N", nfi);
            //lblBCLTotalReturn.Text = (BCLTotalReturn / num).ToString("N", nfi);
            //lblIWJRTotalReturn.Text = (IWJRTotalReturn / num).ToString("N", nfi);
            //lblVATChargeTotalReturn.Text = (VATTotalReturn / num).ToString("N", nfi);

            if (ReturnID != "")
            {
                ConnectingDepartTotal = ConnectingDepartTotal / 2;
                lblConnectingDepartTotal.Text = (ConnectingDepartTotal / num).ToString("N", nfi);
                lblConnectingReturnTotal.Text = (ConnectingDepartTotal / num).ToString("N", nfi);

                DiscTotalDepart = DiscTotalDepart / 2;
                lblDiscTotalDepart.Text = (DiscTotalDepart / num).ToString("N", nfi);
                lblDiscTotalReturn.Text = (DiscTotalDepart / num).ToString("N", nfi);
            }
            else
            {
                lblConnectingDepartTotal.Text = (ConnectingDepartTotal / num).ToString("N", nfi);
                lblDiscTotalDepart.Text = (DiscTotalDepart / num).ToString("N", nfi);
            }
        }

        protected void HideZeroValue()
        {
            if (lblPaxFareDepart.Text == "" || lblPaxFareDepart.Text == "0.00")
            {
                trPaxfareDepart.Visible = false;
            }
            if (lblTaxDepart.Text == "" || lblTaxDepart.Text == "0.00")
            {
                trAirportTaxDepart.Visible = false;
            }
            if (lblPaxFeeDepart.Text == "" || lblPaxFeeDepart.Text == "0.00")
            {
                trPaxServChargeDepart.Visible = false;
            }
            if (lblFuelDepart.Text == "" || lblFuelDepart.Text == "0.00")
            {
                trFuelTaxDepart.Visible = false;
            }
            if (lblSvcDepart.Text == "" || lblSvcDepart.Text == "0.00")
            {
                trServChargeDepart.Visible = false;
            }
            if (lblVATDepart.Text == "" || lblVATDepart.Text == "0.00")
            {
                trVATDepart.Visible = false;
            }
            if (lblOthDepart.Text == "" || lblOthDepart.Text == "0.00")
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
            if (lblACFTotalDepart.Text == "" || lblACFTotalDepart.Text == "0.00")
            {
                trACFChargeDepart.Visible = false;
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
            if (lblDiscTotalDepart.Text == "" || lblDiscTotalDepart.Text == "0.00")
            {
                trDiscountChargeDepart.Visible = false;
            }
            if (lblPromoDiscDepart.Text == "" || lblPromoDiscDepart.Text == "0.00")
            {
                trPromoDiscountDepart.Visible = false;
            }
            if (lblSPLTotalDepart.Text == "" || lblSPLTotalDepart.Text == "0.00")
            {
                trSPLChargeDepart.Visible = false;
            }
            if (lblAPSTotalDepart.Text == "" || lblAPSTotalDepart.Text == "0.00")
            {
                trAPSChargeDepart.Visible = false;
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
            if (lblIWJRTotalDepart.Text == "" || lblIWJRTotalDepart.Text == "0.00")
            {
                trIWJRChargeDepart.Visible = false;
            }
            if (lblVATChargeTotalDepart.Text == "" || lblVATChargeTotalDepart.Text == "0.00")
            {
                trVATChargeDepart.Visible = false;
            }
            if (lblAPFTotalDepart.Text == "" || lblAPFTotalDepart.Text == "0.00")
            {
                trAPFChargeDepart.Visible = false;
            }
            if (lblAPFCTotalDepart.Text == "" || lblAPFCTotalDepart.Text == "0.00")
            {
                trAPFCChargeDepart.Visible = false;
            }
            if (lblIPSCTotalDepart.Text == "" || lblIPSCTotalDepart.Text == "0.00")
            {
                trIPSCChargeDepart.Visible = false;
            }
            if (lblISFTotalDepart.Text == "" || lblISFTotalDepart.Text == "0.00")
            {
                trISFChargeDepart.Visible = false;
            }
            if (lblPSCTotalDepart.Text == "" || lblPSCTotalDepart.Text == "0.00")
            {
                trPSCChargeDepart.Visible = false;
            }

            if (lblPaxFareReturn.Text == "" || lblPaxFareReturn.Text == "0.00")
            {
                trPaxfareReturn.Visible = false;
            }
            if (lblTaxReturn.Text == "" || lblTaxReturn.Text == "0.00")
            {
                trAirportTaxReturn.Visible = false;
            }
            if (lblPaxFeeReturn.Text == "" || lblPaxFeeReturn.Text == "0.00")
            {
                trPaxServChargeReturn.Visible = false;
            }
            if (lblFuelReturn.Text == "" || lblFuelReturn.Text == "0.00")
            {
                trFuelTaxReturn.Visible = false;
            }
            if (lblSvcReturn.Text == "" || lblSvcReturn.Text == "0.00")
            {
                trServChargeReturn.Visible = false;
            }
            if (lblVATReturn.Text == "" || lblVATReturn.Text == "0.00")
            {
                trVATReturn.Visible = false;
            }
            if (lblACFTotalReturn.Text == "" || lblACFTotalReturn.Text == "0.00")
            {
                trACFChargeReturn.Visible = false;
            }
            if (lblOthReturn.Text == "" || lblOthReturn.Text == "0.00")
            {
                trOthChargeReturn.Visible = false;
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
            if (lblDiscTotalReturn.Text == "" || lblDiscTotalReturn.Text == "0.00")
            {
                trDiscountChargeReturn.Visible = false;
            }
            if (lblPromoDiscReturn.Text == "" || lblPromoDiscReturn.Text == "0.00")
            {
                trPromoDiscountReturn.Visible = false;
            }
            if (lblSPLTotalReturn.Text == "" || lblSPLTotalReturn.Text == "0.00")
            {
                trSPLChargeReturn.Visible = false;
            }
            if (lblAPSTotalReturn.Text == "" || lblAPSTotalReturn.Text == "0.00")
            {
                trAPSChargeReturn.Visible = false;
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
            if (lblIWJRTotalReturn.Text == "" || lblIWJRTotalReturn.Text == "0.00")
            {
                trIWJRChargeReturn.Visible = false;
            }
            if (lblVATChargeTotalReturn.Text == "" || lblVATChargeTotalReturn.Text == "0.00")
            {
                trVATChargeReturn.Visible = false;
            }
            if (lblAPFTotalReturn.Text == "" || lblAPFTotalReturn.Text == "0.00")
            {
                trAPFChargeReturn.Visible = false;
            }
            if (lblAPFCTotalReturn.Text == "" || lblAPFCTotalReturn.Text == "0.00")
            {
                trAPFCChargeReturn.Visible = false;
            }
            if (lblIPSCTotalReturn.Text == "" || lblIPSCTotalReturn.Text == "0.00")
            {
                trIPSCChargeReturn.Visible = false;
            }
            if (lblISFTotalReturn.Text == "" || lblISFTotalReturn.Text == "0.00")
            {
                trISFChargeReturn.Visible = false;
            }
            if (lblPSCTotalReturn.Text == "" || lblPSCTotalReturn.Text == "0.00")
            {
                trPSCChargeReturn.Visible = false;
            }

        }

        protected void btn_Next_Click(object sender, EventArgs e)
        {
            Response.Redirect(Shared.MySite.PublicPages.ReviewBooking);
        }

        protected void LoadingProcess()
        {
            MessageList msgList = new MessageList();
            try
            {


                /*
                if (SaveData())
                    e.Result = "";
                else
                    e.Result = msgList.Err100031;
                */

                if (LoadData() == false)
                {
                    Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                }
                //Session["PaxStatus"] = "false"; //testing purpose
                //added by diana 20140124 - check if pax total <> initial pax, then do not proceed further
                if (Session["PaxStatus"] != null)
                {
                    if (Session["PaxStatus"].ToString() == "false")
                    {
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "PostbackKey", "<script type='text/javascript'>window.alert('Total Seat is not enough for your booking pax. Kindly rebook the flight.');</script>");
                        Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //e.Result = msgList.Err100031;
            }
        }

        protected void SavingProcess()
        {
            MessageList msgList = new MessageList();
            try
            {


                /*
                if (SaveData())
                    e.Result = "";
                else
                    e.Result = msgList.Err100031;
                */

                if (SaveData() == false)
                {
                    Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                }
                //Session["PaxStatus"] = "false"; //testing purpose
                //added by diana 20140124 - check if pax total <> initial pax, then do not proceed further
                if (Session["PaxStatus"] != null)
                {
                    if (Session["PaxStatus"].ToString() == "false")
                    {
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "PostbackKey", "<script type='text/javascript'>window.alert('Total Seat is not enough for your booking pax. Kindly rebook the flight.');</script>");
                        Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //e.Result = msgList.Err100031;
            }
        }

        ////protected void Timer1_Tick(object sender, EventArgs e)
        ////{
        ////    TimeSpan time1 = new TimeSpan();
        ////    time1 = (DateTime)Session["gbstimer"] - DateTime.Now;
        ////    if (time1.Seconds <= 0)
        ////    {
        ////        Label1.Text = "TimeOut!";
        ////    }
        ////    else
        ////    {
        ////        Label1.Text = time1.Seconds.ToString();
        ////    }
        ////}

        //protected void tTimer_Tick(object sender, EventArgs e)
        //{
        //    TimeSpan time1 = new TimeSpan();
        //    time1 = (DateTime)Session["gbstimer"] - DateTime.Now;
        //    if (time1.Seconds <= 0)
        //    {
        //        lblTimer.Text = "TimeOut!";
        //    }
        //    else
        //    {
        //        lblTimer.Text = time1.Seconds.ToString();
        //    }
        //}

        protected bool LoadData()
        {
            ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();

            string strExpr;
            string strSort;
            string keyCarrier = "";
            decimal totalOth = 0; //service charge total
            decimal totalDisc = 0; //discount charge total
            decimal totalPromoDisc = 0;
            decimal totalInfant = 0;
            int TotalInfantpax = 0;
            try
            {
                HttpCookie cookie2 = Request.Cookies["cookieSearchcondition"];
                if (cookie2 != null)
                {
                    if (cookie2.Values["InfantNum"] != "")
                    {
                        HttpCookie cookie3 = Request.Cookies["AllPax"];
                        if (cookie3 != null)
                        {
                            TotalInfantpax = Convert.ToInt32(cookie2.Values["InfantNum"]);
                        }
                    }
                }
                DataTable dt = new DataTable();
                Hashtable ht = new Hashtable();
                //added by ketee 20130625
                decimal currencyRate = 1;

                //payment control
                PaymentControl objPayment = new PaymentControl();

                strExpr = "TemFlightId = '" + departID + "'";
                strSort = "";
                DateTime departDate;
                dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                DataRow[] foundRows = new DataRow[1];
                if (dt != null)
                {
                    foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                }
                if (foundRows.Length > 0)
                {
                    FillModelFromDataRow(foundRows, ref temFlight);
                }
                else
                {
                    Response.Redirect(Shared.MySite.PublicPages.SessionExpired, false);
                }

                //Amended by Tyas 20170920 to fix Airbrake issue temFlight null
                if (temFlight != null)
                {
                    departDate = Convert.ToDateTime(temFlight.TemFlightStd);

                    Currency = temFlight.TemFlightCurrencyCode.Trim();
                }
                else
                {
                    Response.Redirect(Shared.MySite.PublicPages.SessionExpired, false);
                }

                if (MyUserSet.AgentName != null)
                    agent = objAgentProfile.GetSingleAgentProfile(MyUserSet.AgentName);

                string LoginType = MyUserSet.AgentType.ToString();

                int m = 0;
                int count = 0;
                DataTable dtClass = objBooking.dtClass();
                if (HttpContext.Current.Session["dataClass"] != null) { dtClass = (DataTable)HttpContext.Current.Session["dataClass"]; }
                count = dtClass.Rows.Count;

                byte seqNo = 1;
                List<BookingTransactionMain> TransMain = new List<BookingTransactionMain>();

                tranID = DateTime.Now.ToString("yyyyMMddHHmmsss");

                #region newsavedetail
                //Datatable Process 

                //string PNR = book.BookingCommit(agent, temClass.TemClassPersonNumber, temClass.TemClassClassPrice.Value, temClass.TemClassSellSignature);

                DataTable dataClass = objBooking.dtClass();
                dataClass = (DataTable)HttpContext.Current.Session["dataClass"];

                //DataTable dataTransFees = new DataTable();
                //dataTransFees = (DataTable)HttpContext.Current.Session["dtTempTransFees"];

                //DataTable dataTransFees2 = new DataTable();
                //dataTransFees2 = (DataTable)HttpContext.Current.Session["dtTempTransFees2"];

                Session["dataTFOth"] = null;
                DataTable dtTFOth = new DataTable();
                if (HttpContext.Current.Session["dataTFOthSellFlightByTem"] != null)
                    Session["dataTFOth"] = HttpContext.Current.Session["dataTFOthSellFlightByTem"];
                else if (HttpContext.Current.Session["dataTFOthSellFlightByTemAddInfant"] != null)
                    Session["dataTFOth"] = HttpContext.Current.Session["dataTFOthSellFlightByTemAddInfant"];
                else if (HttpContext.Current.Session["dataTFOthSellJourney"] != null)
                    Session["dataTFOth"] = HttpContext.Current.Session["dataTFOthSellJourney"];
                else if (HttpContext.Current.Session["dataTFOthSellJourneyAddInfant"] != null)
                    Session["dataTFOth"] = HttpContext.Current.Session["dataTFOthSellJourneyAddInfant"];

                HttpContext.Current.Session.Remove("dataTFOthSellFlightByTem");
                HttpContext.Current.Session.Remove("dataTFOthSellFlightByTemAddInfant");
                HttpContext.Current.Session.Remove("dataTFOthSellJourney");
                HttpContext.Current.Session.Remove("dataTFOthSellJourneyAddInfant");
                if (Session["dataTFOth"] != null)
                    dtTFOth = (DataTable)Session["dataTFOth"];
                int rowdttemp = 0;

                foreach (DataRow dr in dataClass.Rows)
                {
                    bookDTLInfo = new BookingTransactionDetail();
                    string PNR = seqNo.ToString();
                    bookDTLInfo.RecordLocator = PNR;
                    bookDTLInfo.TransID = tranID;
                    bookDTLInfo.SeqNo = seqNo;

                    if (seqNo == 1)
                    {
                        keyCarrier = bookDTLInfo.CarrierCode = dr["CarrierCode"].ToString();
                        ht.Add("keyCarrier", keyCarrier);
                    }

                    //service charge pax
                    //decimal svcCharge = Convert.ToDecimal(objGeneral.getSysValueByKeyAndCarrierCode("SVCCHARGEPAX", keyCarrier)) / 100;
                    bookDTLInfo.LineOth = Convert.ToDecimal(dr["OthChrg"].ToString());
                    totalOth += bookDTLInfo.LineOth;

                    bookDTLInfo.LineDisc = Convert.ToDecimal(dr["DiscChrg"].ToString());
                    bookDTLInfo.LinePaxFee = Convert.ToDecimal(dr["PaxFeeChrg"].ToString());
                    totalDisc += bookDTLInfo.LineDisc;

                    bookDTLInfo.LinePromoDisc = Convert.ToDecimal(dr["PromoDiscChrg"].ToString());
                    totalPromoDisc += bookDTLInfo.LinePromoDisc;


                    ////20170630 - Sienny (transfee)
                    //bookFEEInfo = new BookingTransactionFees();
                    //bookFEEInfo.TransID = tranID;
                    //bookFEEInfo.RecordLocator = PNR;
                    //bookFEEInfo.SeqNo = seqNo;
                    //bookFEEInfo.Origin = dr["Origin"].ToString();
                    //bookFEEInfo.Transit = dr["TemClassTransit"].ToString();
                    //bookFEEInfo.Destination = dr["Destination"].ToString();
                    if (dtTFOth != null && dtTFOth.Rows.Count > 0)
                    {
                        decimal totalAmtTF = 0;
                        for (int i = rowdttemp; i < dtTFOth.Rows.Count; i++)
                        {
                            DataRow drTF = dtTFOth.Rows[i];
                            drTF["TransID"] = tranID;
                            drTF["RecordLocator"] = PNR;
                            drTF["SeqNo"] = i + 1;
                            drTF["CarrierCode"] = dr["CarrierCode"].ToString();
                            drTF["FlightNumber"] = dr["FlightNumber"].ToString();

                            totalAmtTF += Convert.ToDecimal(dtTFOth.Rows[i]["FeeAmt"].ToString());

                            if (totalAmtTF == bookDTLInfo.LineOth)
                            {
                                rowdttemp = i + 1;
                                break;
                            }
                        }
                    }
                    //bookFEEInfo.PaxType = "";
                    //bookFEEInfo.FeeType = "";
                    //bookFEEInfo.FeeQty = 0;
                    //bookFEEInfo.FeeRate = 0;
                    //bookFEEInfo.FeeAmt = 0;
                    //bookFEEInfo.Transvoid = 0;
                    //bookFEEInfo.CreateBy = MyUserSet.AgentID;
                    //bookFEEInfo.SyncCreate = DateTime.Now;
                    //bookFEEInfo.SyncLastUpd = DateTime.Now;
                    //bookFEEInfo.LastSyncBy = MyUserSet.AgentID;


                    seqNo += 1;
                    bookDTLInfo.Currency = Currency;
                    bookDTLInfo.CarrierCode = dr["CarrierCode"].ToString();
                    bookDTLInfo.FlightNo = dr["FlightNumber"].ToString();
                    bookDTLInfo.DepatureDate = Convert.ToDateTime(dr["DepartureDate"].ToString());
                    bookDTLInfo.ArrivalDate = Convert.ToDateTime(dr["ArrivalDate"].ToString());
                    bookDTLInfo.Origin = dr["Origin"].ToString();
                    bookDTLInfo.Destination = dr["Destination"].ToString();

                    //bookDTLInfo.LineFee = Convert.ToDecimal(dr["FuelChrg"].ToString());
                    bookDTLInfo.LineFee = Convert.ToDecimal(dr["ServChrg"].ToString());
                    //added by ketee
                    bookDTLInfo.LineVAT = Convert.ToDecimal(dr["ServVAT"] == DBNull.Value ? 0 : dr["ServVAT"]);

                    bookDTLInfo.LineInfant = Convert.ToDecimal(dr["InfantChrg"].ToString());
                    if (cookie2 != null)
                    {
                        if (cookie2.Values["InfantNum"] != "")
                        {
                            HttpCookie cookie3 = Request.Cookies["AllPax"];
                            if (cookie3 != null)
                            {
                                totalInfant = Convert.ToDecimal(dr["InfantChrg"].ToString()) / TotalInfantpax;
                            }
                            else
                            {
                                totalInfant = 0;
                            }
                        }
                        else
                        {
                            totalInfant = 0;
                        }
                    }
                    totalServVAT += bookDTLInfo.LineVAT;

                    totalServiceFee += bookDTLInfo.LineFee;
                    totalPaxFee += bookDTLInfo.LinePaxFee;

                    bookDTLInfo.FareClass = dr["FareClass"].ToString();
                    bookDTLInfo.PaxAdult = Convert.ToInt16(dr["AdultPax"].ToString());
                    bookDTLInfo.PaxChild = Convert.ToInt16(dr["ChildPax"].ToString());
                    bookDTLInfo.LineTotal = Convert.ToDecimal(dr["FullPrice"].ToString());
                    bookDTLInfo.SyncLastUpd = DateTime.Now;
                    bookDTLInfo.LastSyncBy = MyUserSet.AgentID;

                    //bookDTLInfo.FarePerPax = Convert.ToDecimal(dr["FarePrice"].ToString());
                    bookDTLInfo.FarePerPax = Convert.ToDecimal(dr["FarePrice"].ToString()) / Convert.ToDecimal(dr["Quantity"].ToString());

                    if (dr["ChdTaxChrg"].ToString() == "") dr["ChdTaxChrg"] = 0; // || (dr["Origin"].ToString() != "HKG" && dr["Destination"].ToString() != "HKG")) dr["ChdTaxChrg"] = 0;
                                                                                 //if (dr["ChdFuelChrg"].ToString() == "") dr["ChdFuelChrg"] = 0; // || (dr["Origin"].ToString() != "HKG" && dr["Destination"].ToString() != "HKG")) dr["ChdFuelChrg"] = 0;
                    bookDTLInfo.LineTax = Convert.ToDecimal(dr["TaxChrg"].ToString()) + Convert.ToDecimal(dr["FuelChrg"].ToString()) + Convert.ToDecimal(dr["ChdTaxChrg"].ToString()); // + Convert.ToDecimal(dr["ChdFuelChrg"].ToString()); //apt + fuel

                    //totalFlightFare += bookDTLInfo.LineTotal + bookDTLInfo.LineOth; //include service charge
                    totalFlightFare += bookDTLInfo.LineTotal; //include service charge

                    bookDTLInfo.TransVoid = 0;
                    bookDTLInfo.CreateBy = MyUserSet.AgentID;
                    bookDTLInfo.SyncCreate = DateTime.Now;

                    bookDTLInfo.Transit = dr["TemClassTransit"].ToString();
                    bookDTLInfo.SellKey = dr["FareSellKey"].ToString();

                    if (bookDTLInfo.Transit != "")
                    {
                        bookDTLInfo.ArrivalDate2 = Convert.ToDateTime(dr["TemClassSta2"].ToString());
                        bookDTLInfo.DepatureDate2 = Convert.ToDateTime(dr["TemClassStd2"].ToString());
                        bookDTLInfo.OverridedSellKey = dr["FareSellKey2"].ToString();
                    }

                    bookDTLInfo.FlightDura = Convert.ToDecimal(dr["FlightDura"].ToString());
                    bookDTLInfo.FlightDura2 = Convert.ToDecimal(dr["FlightDura2"].ToString());
                    bookDTLInfo.CollectedAmount = 0;
                    bookDTLInfo.Signature = dr["SellSignature"].ToString();

                    //added by ketee 20130625
                    //midchange = from CurrencyRate to ExchgRate
                    //currencyRate = Convert.ToDecimal(dr["CurrencyRate"]);
                    currencyRate = Convert.ToDecimal(dr["CurrencyRate"] == DBNull.Value ? 1 : dr["CurrencyRate"]);
                    // objBooking.SaveBK_TRANSDTL(bookDTLInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
                    lstbookDTLInfo.Add(bookDTLInfo);

                    //Amended by Tyas 20170920 to fix Airbrake issue
                    APT += bookDTLInfo.LineTax != null ? Convert.ToDecimal(bookDTLInfo.LineTax) : 0;
                }
                // end datatable
                #endregion


                //20170707 - Sienny (transfee)
                #region SaveTransFees
                if (dtTFOth != null)
                {
                    foreach (DataRow drFee in dtTFOth.Rows)
                    {
                        bookFEEInfo = new BookingTransactionFees();
                        bookFEEInfo.TransID = drFee["TransID"].ToString();
                        bookFEEInfo.RecordLocator = drFee["RecordLocator"].ToString();
                        bookFEEInfo.SeqNo = Convert.ToByte(drFee["SeqNo"].ToString());
                        bookFEEInfo.Origin = drFee["Origin"].ToString();
                        bookFEEInfo.Transit = drFee["Transit"].ToString();
                        bookFEEInfo.Destination = drFee["Destination"].ToString();

                        bookFEEInfo.PaxType = drFee["PaxType"].ToString();
                        bookFEEInfo.FeeCode = drFee["FeeCode"].ToString();
                        bookFEEInfo.FeeDesc = drFee["FeeDesc"].ToString();
                        bookFEEInfo.FeeType = drFee["FeeType"].ToString();

                        string cekFeeCode = "";
                        if (bookFEEInfo.FeeCode == "")
                        {
                            if (bookFEEInfo.FeeType.Length > 10)
                                cekFeeCode = objGeneral.getCodeName("FEE", bookFEEInfo.FeeType.Substring(0, 10).Trim());
                            else
                                cekFeeCode = objGeneral.getCodeName("FEE", bookFEEInfo.FeeType.Trim());
                        }
                        else
                            cekFeeCode = objGeneral.getCodeName("FEE", bookFEEInfo.FeeCode);

                        bookFEEInfo.FeeQty = Convert.ToDouble(drFee["FeeQty"].ToString());
                        bookFEEInfo.FeeRate = Convert.ToDecimal(drFee["FeeRate"].ToString());
                        bookFEEInfo.FeeAmt = Convert.ToDecimal(drFee["FeeAmt"].ToString());
                        bookFEEInfo.Transvoid = 0;
                        bookFEEInfo.CreateBy = MyUserSet.AgentID;
                        bookFEEInfo.SyncCreate = DateTime.Now;
                        bookFEEInfo.SyncLastUpd = DateTime.Now;
                        bookFEEInfo.LastSyncBy = MyUserSet.AgentID;

                        lstbookFEEInfo.Add(bookFEEInfo);
                    }
                    Session["listTransFees"] = lstbookFEEInfo;

                    //if (Application["dtCodeMasterFee"] != null)
                    //{
                    //    DataTable dtCodeMasterFee = (DataTable)Application["dtCodeMasterFee"];
                    //    var TransFee = (from dataRows1 in dtTFOth.AsEnumerable()
                    //                    join dataRows2 in dtCodeMasterFee.AsEnumerable()
                    //                    on dataRows1.Field<string>("FeeCode") equals dataRows2.Field<string>("Code")
                    //                    into outer
                    //                    from dataRows3 in outer.DefaultIfEmpty()
                    //                    //where dataRows3.Field<string>("CodeType") == "FEE" || dataRows3.Field<string>("CodeType") == ""
                    //                      select new
                    //                     {
                    //                         TransID = dataRows1.Field<string>("TransID"),
                    //                         FeeCode = (dataRows3 != null) ? dataRows1.Field<string>("FeeCode") : "",
                    //                         CodeType = (dataRows3 == null) ? "" : dataRows3.Field<string>("CodeType"),
                    //                         FeeType = dataRows1.Field<string>("FeeType"),
                    //                         CodeDesc = (dataRows3 == null) ? dataRows1.Field<string>("FeeType") : dataRows3.Field<string>("CodeDesc"),
                    //                         FeeDesc = dataRows1.Field<string>("FeeDesc"),
                    //                         Origin = dataRows1.Field<string>("Origin"),
                    //                         Transit = dataRows1.Field<string>("Transit"),
                    //                         Destination = dataRows1.Field<string>("Destination"),
                    //                         FeeAmt = objGeneral.RoundUp(Convert.ToDecimal(dataRows1.Field<string>("FeeAmt"))),
                    //                         FeeAmtPerPax = objGeneral.RoundUp(Convert.ToDecimal(dataRows1.Field<string>("FeeAmt")) / Convert.ToDecimal(dataRows1.Field<string>("FeeQty")))
                    //                     }).ToList();

                    //    DataTable dtTransFee = LINQResultToDataTable(TransFee);
                    //    Session["dtTransFee"] = dtTransFee;


                    //}
                    #endregion
                }

                //save booking header

                bookHDRInfo.TransID = bookDTLInfo.TransID;

                ht.Add("TransID", bookHDRInfo.TransID);

                bookHDRInfo.TransType = 0;
                bookHDRInfo.AgentID = MyUserSet.AgentID;
                bookHDRInfo.AgentCatgID = MyUserSet.AgentCategoryID;
                bookHDRInfo.BookingDate = DateTime.Now;
                bookHDRInfo.STDDate = Convert.ToDateTime(temFlight.TemFlightStd);

                string expirySetting = objGeneral.getSysValueByKeyAndCarrierCode("STDEXPIRY", keyCarrier);

                int sysValue = 0;
                if (expirySetting != "")
                {
                    sysValue = Convert.ToInt16(expirySetting);
                }


                //set expirydate after scheme is assign in savebooking 
                //bookHDRInfo.ExpiryDate = departDate.Date.AddDays(-sysValue);

                /*
                string tempdate1 = String.Format("{0:MM/dd/yyyy}", departDate.Date );
                string tempdate2 = String.Format("{0:MM/dd/yyyy}", DateTime.Now);
                TimeSpan ts = Convert.ToDateTime(tempdate1) - Convert.ToDateTime(tempdate2);

                int tempday = Convert.ToInt32(ts.TotalDays.ToString());
                if (tempday < 2)
                    bookHDRInfo.ExpiryDate = DateTime.Now;
                else
                    bookHDRInfo.ExpiryDate = departDate.Date.AddDays(-sysValue);
                */

                //Amended by Tyas 20170920 to fix Airbrake issue
                if (bookHDRInfo.ExpiryDate != null)
                    ht.Add("Expiry", Convert.ToDateTime(bookHDRInfo.ExpiryDate));

                bookHDRInfo.TransTotalPAX = Convert.ToInt16(temFlight.TemFlightADTNum) + Convert.ToInt16(temFlight.TemFlightCHDNum);
                bookHDRInfo.CollectedAmt = 0;

                bookHDRInfo.TransTotalAmt = totalFlightFare;
                bookHDRInfo.TransSubTotal = totalFlightFare;
                bookHDRInfo.TransTotalPaxFee = totalPaxFee;
                bookHDRInfo.TransTotalTax = APT;
                bookHDRInfo.TransTotalFee = totalServiceFee;
                bookHDRInfo.TransTotalOth = totalOth;
                bookHDRInfo.TransTotalDisc = totalDisc;
                bookHDRInfo.TransTotalPromoDisc = totalPromoDisc;

                HttpCookie cookie = Request.Cookies["cookieSearchcondition"];
                if (cookie != null)
                {
                    if (totalPromoDisc < 0)
                        bookHDRInfo.PromoCode = cookie.Values["PromoCode"].ToString().ToUpper();
                    else
                        bookHDRInfo.PromoCode = "";
                }

                bookHDRInfo.TransTotalInfant = totalInfant;
                //addede by ketee
                bookHDRInfo.TransTotalVAT = totalServVAT;

                bookHDRInfo.Currency = Currency;
                bookHDRInfo.CurrencyPaid = Currency;

                bookHDRInfo.TransStatus = 0;
                bookHDRInfo.CreateBy = MyUserSet.AgentID;
                bookHDRInfo.SyncCreate = DateTime.Now;
                bookHDRInfo.SyncLastUpd = DateTime.Now;
                bookHDRInfo.LastSyncBy = MyUserSet.AgentName;

                //added by ketee 20130625, currencyRate
                bookHDRInfo.ExchangeRate = currencyRate;

                //added by ketee, 20170307, all new booking will default the isoverride = 1 mean is GBS2
                bookHDRInfo.IsOverride = 1;
                //load fare

                //20170411 - Sienny (organizationID added to BK_TRANSMAIN)
                if (Session["OrganizationCode"] != null) organizationID = Session["OrganizationCode"].ToString();
                bookHDRInfo.OrganizationID = organizationID;

                decimal avg = 0;
                decimal dpt = 0;
                decimal rtn = 0;
                if (HttpContext.Current.Session["Fare"] != null)
                {

                    Hashtable htFare = (Hashtable)HttpContext.Current.Session["Fare"];
                    avg = (htFare["Avg"] != null) ? Convert.ToDecimal(htFare["Avg"]) : 0;
                    dpt = Convert.ToDecimal(htFare["Dpt"]);
                    rtn = Convert.ToDecimal(htFare["Rtn"]);
                }

                //bookHDRInfo.TotalAmtAVG = Convert.ToDecimal(lblAverageFare.Text);
                bookHDRInfo.TotalAmtAVG = avg;

                bookHDRInfo.TotalAmtGoing = dpt + totalInfant;

                if (LblReturn.Text != "")
                { bookHDRInfo.TotalAmtReturn = rtn + totalInfant; }
                else
                { bookHDRInfo.TotalAmtReturn = 0; }

                //Added by Tyas 20170607
                string GroupName = objGeneral.getOPTGroupByCarrierCode(keyCarrier);
                if (GroupName != "")
                {
                    string haul = "";
                    if (GroupName == "AA") haul = "SHORT";
                    else if (GroupName == "AAX") haul = "LONG";
                    DataTable dtNameChange = objBooking.GetFeeSettingbyGrpID(GroupName);
                    if (dtNameChange != null && dtNameChange.Rows.Count > 0)
                    {
                        bookHDRInfo.NameChangeMax = Convert.ToInt32(dtNameChange.Rows[0]["ChangeLimit"]);
                        //string[] tokens = dtNameChange.Rows[0]["SYSValueEx"].ToString().Split(';');
                        //bookHDRInfo.NameChangeLimit1 = Convert.ToDecimal(tokens[0]);
                        //bookHDRInfo.NameChangeLimit2 = Convert.ToDecimal(tokens[1]);
                    }
                }

                /*
                bookHDRInfo.TotalAmtGoing = Convert.ToDecimal(lblDepartFare.Text);
                if (LblReturn.Text != "")
                { bookHDRInfo.TotalAmtReturn = Convert.ToDecimal(lblReturnFare.Text); }
                else
                { bookHDRInfo.TotalAmtReturn = 0; }
                */

                string reminder = objGeneral.getSysValueByKeyAndCarrierCode("REMINDDURA1", keyCarrier);

                if (reminder != "")
                {
                    sysValue = Convert.ToInt16(reminder);
                }
                //bookHDRInfo.CurReminderDate = bookHDRInfo.ExpiryDate.AddDays(-sysValue);

                reminder = objGeneral.getSysValueByKeyAndCarrierCode("REMINDDURA2", keyCarrier);

                if (reminder != "")
                {
                    sysValue = Convert.ToInt16(reminder);
                }
                //bookHDRInfo.NextReminderDate = bookHDRInfo.ExpiryDate.AddDays(-sysValue);
                bookHDRInfo.ReminderType = 1;

                //load max failed payment try
                string maxPaymentFail = objGeneral.getSysValueByKeyAndCarrierCode("PAYMENTSUSPEND", keyCarrier);
                ht.Add("PaymentSuspend", maxPaymentFail);

                //added by ketee, 20170310, for new booking, set isoverride = 1
                ht.Add("IsOverride", "1");

                HttpContext.Current.Session.Remove("HashMain");
                HttpContext.Current.Session.Add("HashMain", ht);

                //end save header

                //added by ketee
                BookingTransactionMain BookingMain = new BookingTransactionMain();
                //BookingMain = objBooking.SaveBooking(bookHDRInfo, lstbookDTLInfo, transTaxInfo, lsttransFeesInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);

                if (bookHDRInfo != null) //amended by diana 20140124 - check for equal total pax
                {
                    if (bookHDRInfo.TransTotalPAX < int.Parse(HttpContext.Current.Session["TotalPax"].ToString()))
                    {
                        Session["PaxStatus"] = "false";
                        return false;
                    }
                    else
                    {
                        Session["bookHDRInfo"] = bookHDRInfo;
                        Session["lstbookDTLInfo"] = lstbookDTLInfo;
                        if (objBooking.SaveTransFee(ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert))
                        {
                            Session["listTransFees"] = null;
                            Session["TransID"] = bookHDRInfo.TransID;
                        return true;
                        }
                        //else
                        //return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                return false;
            }
            //objBooking.SaveBooking(bookHDRInfo, lstbookDTLInfo, transTaxInfo, lsttransFeesInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
        }

        protected bool SaveData()
        {
                ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();

                string strExpr;
                string strSort;
                string keyCarrier = "";
                decimal totalOth = 0; //service charge total
                decimal totalDisc = 0; //discount charge total
                decimal totalPromoDisc = 0;
                decimal totalInfant = 0;
                int TotalInfantpax = 0;
                try
                {
                    HttpCookie cookie2 = Request.Cookies["cookieSearchcondition"];
                    if (cookie2 != null)
                    {
                        if (cookie2.Values["InfantNum"] != "")
                        {
                            HttpCookie cookie3 = Request.Cookies["AllPax"];
                            if (cookie3 != null)
                            {
                                TotalInfantpax = Convert.ToInt32(cookie2.Values["InfantNum"]);
                            }
                        }
                    }
                    DataTable dt = new DataTable();
                    Hashtable ht = new Hashtable();
                    //added by ketee 20130625
                    decimal currencyRate = 1;

                    //payment control
                    PaymentControl objPayment = new PaymentControl();

                    strExpr = "TemFlightId = '" + departID + "'";
                    strSort = "";
                    DateTime departDate;
                    dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                    DataRow[] foundRows = new DataRow[1];
                    if (dt != null)
                    {
                        foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                    }
                    if (foundRows.Length > 0)
                    {
                        FillModelFromDataRow(foundRows, ref temFlight);
                    }
                    else
                    {
                        Response.Redirect(Shared.MySite.PublicPages.SessionExpired, false);
                    }

                    //Amended by Tyas 20170920 to fix Airbrake issue temFlight null
                    if (temFlight != null)
                    {
                        departDate = Convert.ToDateTime(temFlight.TemFlightStd);

                        Currency = temFlight.TemFlightCurrencyCode.Trim();
                    }
                    else
                    {
                        Response.Redirect(Shared.MySite.PublicPages.SessionExpired, false);
                    }

                    if (MyUserSet.AgentName != null)
                        agent = objAgentProfile.GetSingleAgentProfile(MyUserSet.AgentName);

                    string LoginType = MyUserSet.AgentType.ToString();

                    int m = 0;
                    int count = 0;
                    DataTable dtClass = objBooking.dtClass();
                    if (HttpContext.Current.Session["dataClass"] != null) { dtClass = (DataTable)HttpContext.Current.Session["dataClass"]; }
                    count = dtClass.Rows.Count;

                    byte seqNo = 1;
                    List<BookingTransactionMain> TransMain = new List<BookingTransactionMain>();

                    if (Session["TransID"] != null)
                        tranID = Session["TransID"].ToString();

                    #region newsavedetail
                        //Datatable Process 

                        //string PNR = book.BookingCommit(agent, temClass.TemClassPersonNumber, temClass.TemClassClassPrice.Value, temClass.TemClassSellSignature);

                    DataTable dataClass = objBooking.dtClass();
                    dataClass = (DataTable)HttpContext.Current.Session["dataClass"];

                    //DataTable dataTransFees = new DataTable();
                    //dataTransFees = (DataTable)HttpContext.Current.Session["dtTempTransFees"];

                    //DataTable dataTransFees2 = new DataTable();
                    //dataTransFees2 = (DataTable)HttpContext.Current.Session["dtTempTransFees2"];

                    Session["dataTFOth"] = null;
                    DataTable dtTFOth = new DataTable();
                    if (HttpContext.Current.Session["dataTFOthSellFlightByTem"] != null)
                        Session["dataTFOth"] = HttpContext.Current.Session["dataTFOthSellFlightByTem"];
                    else if (HttpContext.Current.Session["dataTFOthSellFlightByTemAddInfant"] != null)
                        Session["dataTFOth"] = HttpContext.Current.Session["dataTFOthSellFlightByTemAddInfant"];
                    else if (HttpContext.Current.Session["dataTFOthSellJourney"] != null)
                        Session["dataTFOth"] = HttpContext.Current.Session["dataTFOthSellJourney"];
                    else if (HttpContext.Current.Session["dataTFOthSellJourneyAddInfant"] != null)
                        Session["dataTFOth"] = HttpContext.Current.Session["dataTFOthSellJourneyAddInfant"];

                    HttpContext.Current.Session.Remove("dataTFOthSellFlightByTem");
                    HttpContext.Current.Session.Remove("dataTFOthSellFlightByTemAddInfant");
                    HttpContext.Current.Session.Remove("dataTFOthSellJourney");
                    HttpContext.Current.Session.Remove("dataTFOthSellJourneyAddInfant");
                    if (Session["dataTFOth"] != null)
                        dtTFOth = (DataTable)Session["dataTFOth"];
                    int rowdttemp = 0;

                    foreach (DataRow dr in dataClass.Rows)
                    {
                        bookDTLInfo = new BookingTransactionDetail();
                        string PNR = seqNo.ToString();
                        bookDTLInfo.RecordLocator = PNR;
                        bookDTLInfo.TransID = tranID;
                        bookDTLInfo.SeqNo = seqNo;

                        if (seqNo == 1)
                        {
                            keyCarrier = bookDTLInfo.CarrierCode = dr["CarrierCode"].ToString();
                            ht.Add("keyCarrier", keyCarrier);
                        }

                        //service charge pax
                        //decimal svcCharge = Convert.ToDecimal(objGeneral.getSysValueByKeyAndCarrierCode("SVCCHARGEPAX", keyCarrier)) / 100;
                        bookDTLInfo.LineOth = Convert.ToDecimal(dr["OthChrg"].ToString());
                        totalOth += bookDTLInfo.LineOth;

                        bookDTLInfo.LineDisc = Convert.ToDecimal(dr["DiscChrg"].ToString());
                        bookDTLInfo.LinePaxFee = Convert.ToDecimal(dr["PaxFeeChrg"].ToString());
                        totalDisc += bookDTLInfo.LineDisc;

                        bookDTLInfo.LinePromoDisc = Convert.ToDecimal(dr["PromoDiscChrg"].ToString());
                        totalPromoDisc += bookDTLInfo.LinePromoDisc;


                        ////20170630 - Sienny (transfee)
                        //bookFEEInfo = new BookingTransactionFees();
                        //bookFEEInfo.TransID = tranID;
                        //bookFEEInfo.RecordLocator = PNR;
                        //bookFEEInfo.SeqNo = seqNo;
                        //bookFEEInfo.Origin = dr["Origin"].ToString();
                        //bookFEEInfo.Transit = dr["TemClassTransit"].ToString();
                        //bookFEEInfo.Destination = dr["Destination"].ToString();
                        if (dtTFOth != null && dtTFOth.Rows.Count > 0)
                        {
                            decimal totalAmtTF = 0;
                            for (int i = rowdttemp; i < dtTFOth.Rows.Count; i++)
                            {
                                DataRow drTF = dtTFOth.Rows[i];
                                drTF["TransID"] = tranID;
                                drTF["RecordLocator"] = PNR;
                                drTF["SeqNo"] = i + 1;
                                drTF["CarrierCode"] = dr["CarrierCode"].ToString();
                                drTF["FlightNumber"] = dr["FlightNumber"].ToString();

                                totalAmtTF += Convert.ToDecimal(dtTFOth.Rows[i]["FeeAmt"].ToString());

                                if (totalAmtTF == bookDTLInfo.LineOth)
                                {
                                    rowdttemp = i + 1;
                                    break;
                                }
                            }
                        }
                        //bookFEEInfo.PaxType = "";
                        //bookFEEInfo.FeeType = "";
                        //bookFEEInfo.FeeQty = 0;
                        //bookFEEInfo.FeeRate = 0;
                        //bookFEEInfo.FeeAmt = 0;
                        //bookFEEInfo.Transvoid = 0;
                        //bookFEEInfo.CreateBy = MyUserSet.AgentID;
                        //bookFEEInfo.SyncCreate = DateTime.Now;
                        //bookFEEInfo.SyncLastUpd = DateTime.Now;
                        //bookFEEInfo.LastSyncBy = MyUserSet.AgentID;


                        seqNo += 1;
                        bookDTLInfo.Currency = Currency;
                        bookDTLInfo.CarrierCode = dr["CarrierCode"].ToString();
                        bookDTLInfo.FlightNo = dr["FlightNumber"].ToString();
                        bookDTLInfo.DepatureDate = Convert.ToDateTime(dr["DepartureDate"].ToString());
                        bookDTLInfo.ArrivalDate = Convert.ToDateTime(dr["ArrivalDate"].ToString());
                        bookDTLInfo.Origin = dr["Origin"].ToString();
                        bookDTLInfo.Destination = dr["Destination"].ToString();

                        //bookDTLInfo.LineFee = Convert.ToDecimal(dr["FuelChrg"].ToString());
                        bookDTLInfo.LineFee = Convert.ToDecimal(dr["ServChrg"].ToString());
                        //added by ketee
                        bookDTLInfo.LineVAT = Convert.ToDecimal(dr["ServVAT"] == DBNull.Value ? 0 : dr["ServVAT"]);

                        bookDTLInfo.LineInfant = Convert.ToDecimal(dr["InfantChrg"].ToString());
                        if (cookie2 != null)
                        {
                            if (cookie2.Values["InfantNum"] != "")
                            {
                                HttpCookie cookie3 = Request.Cookies["AllPax"];
                                if (cookie3 != null)
                                {
                                    totalInfant = Convert.ToDecimal(dr["InfantChrg"].ToString()) / TotalInfantpax;
                                }
                                else
                                {
                                    totalInfant = 0;
                                }
                            }
                            else
                            {
                                totalInfant = 0;
                            }
                        }
                        totalServVAT += bookDTLInfo.LineVAT;

                        totalServiceFee += bookDTLInfo.LineFee;
                        totalPaxFee += bookDTLInfo.LinePaxFee;

                        bookDTLInfo.FareClass = dr["FareClass"].ToString();
                        bookDTLInfo.PaxAdult = Convert.ToInt16(dr["AdultPax"].ToString());
                        bookDTLInfo.PaxChild = Convert.ToInt16(dr["ChildPax"].ToString());
                        bookDTLInfo.LineTotal = Convert.ToDecimal(dr["FullPrice"].ToString());
                        bookDTLInfo.SyncLastUpd = DateTime.Now;
                        bookDTLInfo.LastSyncBy = MyUserSet.AgentID;

                        //bookDTLInfo.FarePerPax = Convert.ToDecimal(dr["FarePrice"].ToString());
                        bookDTLInfo.FarePerPax = Convert.ToDecimal(dr["FarePrice"].ToString()) / Convert.ToDecimal(dr["Quantity"].ToString());

                        if (dr["ChdTaxChrg"].ToString() == "") dr["ChdTaxChrg"] = 0; // || (dr["Origin"].ToString() != "HKG" && dr["Destination"].ToString() != "HKG")) dr["ChdTaxChrg"] = 0;
                                                                                     //if (dr["ChdFuelChrg"].ToString() == "") dr["ChdFuelChrg"] = 0; // || (dr["Origin"].ToString() != "HKG" && dr["Destination"].ToString() != "HKG")) dr["ChdFuelChrg"] = 0;
                        bookDTLInfo.LineTax = Convert.ToDecimal(dr["TaxChrg"].ToString()) + Convert.ToDecimal(dr["FuelChrg"].ToString()) + Convert.ToDecimal(dr["ChdTaxChrg"].ToString()); // + Convert.ToDecimal(dr["ChdFuelChrg"].ToString()); //apt + fuel

                        //totalFlightFare += bookDTLInfo.LineTotal + bookDTLInfo.LineOth; //include service charge
                        totalFlightFare += bookDTLInfo.LineTotal; //include service charge

                        bookDTLInfo.TransVoid = 0;
                        bookDTLInfo.CreateBy = MyUserSet.AgentID;
                        bookDTLInfo.SyncCreate = DateTime.Now;

                        bookDTLInfo.Transit = dr["TemClassTransit"].ToString();
                        bookDTLInfo.SellKey = dr["FareSellKey"].ToString();

                        if (bookDTLInfo.Transit != "")
                        {
                            bookDTLInfo.ArrivalDate2 = Convert.ToDateTime(dr["TemClassSta2"].ToString());
                            bookDTLInfo.DepatureDate2 = Convert.ToDateTime(dr["TemClassStd2"].ToString());
                            bookDTLInfo.OverridedSellKey = dr["FareSellKey2"].ToString();
                        }

                        bookDTLInfo.FlightDura = Convert.ToDecimal(dr["FlightDura"].ToString());
                        bookDTLInfo.FlightDura2 = Convert.ToDecimal(dr["FlightDura2"].ToString());
                        bookDTLInfo.CollectedAmount = 0;
                        bookDTLInfo.Signature = dr["SellSignature"].ToString();

                        //added by ketee 20130625
                        //midchange = from CurrencyRate to ExchgRate
                        //currencyRate = Convert.ToDecimal(dr["CurrencyRate"]);
                        currencyRate = Convert.ToDecimal(dr["CurrencyRate"] == DBNull.Value ? 1 : dr["CurrencyRate"]);
                        // objBooking.SaveBK_TRANSDTL(bookDTLInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
                        lstbookDTLInfo.Add(bookDTLInfo);

                        //Amended by Tyas 20170920 to fix Airbrake issue
                        APT += bookDTLInfo.LineTax != null ? Convert.ToDecimal(bookDTLInfo.LineTax) : 0;
                    }
                    // end datatable
                    #endregion


                    //20170707 - Sienny (transfee)
                    #region SaveTransFees
                    //if (dtTFOth != null)
                    //{
                    //    foreach (DataRow drFee in dtTFOth.Rows)
                    //    {
                    //        bookFEEInfo = new BookingTransactionFees();
                    //        bookFEEInfo.TransID = drFee["TransID"].ToString();
                    //        bookFEEInfo.RecordLocator = drFee["RecordLocator"].ToString();
                    //        bookFEEInfo.SeqNo = Convert.ToByte(drFee["SeqNo"].ToString());
                    //        bookFEEInfo.Origin = drFee["Origin"].ToString();
                    //        bookFEEInfo.Transit = drFee["Transit"].ToString();
                    //        bookFEEInfo.Destination = drFee["Destination"].ToString();

                    //        bookFEEInfo.PaxType = drFee["PaxType"].ToString();
                    //        bookFEEInfo.FeeCode = drFee["FeeCode"].ToString();
                    //        bookFEEInfo.FeeDesc = drFee["FeeDesc"].ToString();
                    //        bookFEEInfo.FeeType = drFee["FeeType"].ToString();

                    //        string cekFeeCode = "";
                    //        if (bookFEEInfo.FeeCode == "")
                    //        {
                    //            if (bookFEEInfo.FeeType.Length > 10)
                    //                cekFeeCode = objGeneral.getCodeName("FEE", bookFEEInfo.FeeType.Substring(0, 10).Trim());
                    //            else
                    //                cekFeeCode = objGeneral.getCodeName("FEE", bookFEEInfo.FeeType.Trim());
                    //        }
                    //        else
                    //            cekFeeCode = objGeneral.getCodeName("FEE", bookFEEInfo.FeeCode);

                    //        bookFEEInfo.FeeQty = Convert.ToDouble(drFee["FeeQty"].ToString());
                    //        bookFEEInfo.FeeRate = Convert.ToDecimal(drFee["FeeRate"].ToString());
                    //        bookFEEInfo.FeeAmt = Convert.ToDecimal(drFee["FeeAmt"].ToString());
                    //        bookFEEInfo.Transvoid = 0;
                    //        bookFEEInfo.CreateBy = MyUserSet.AgentID;
                    //        bookFEEInfo.SyncCreate = DateTime.Now;
                    //        bookFEEInfo.SyncLastUpd = DateTime.Now;
                    //        bookFEEInfo.LastSyncBy = MyUserSet.AgentID;

                    //        lstbookFEEInfo.Add(bookFEEInfo);
                    //    }
                    //    Session["listTransFees"] = lstbookFEEInfo;

                        //if (Application["dtCodeMasterFee"] != null)
                        //{
                        //    DataTable dtCodeMasterFee = (DataTable)Application["dtCodeMasterFee"];
                        //    var TransFee = (from dataRows1 in dtTFOth.AsEnumerable()
                        //                    join dataRows2 in dtCodeMasterFee.AsEnumerable()
                        //                    on dataRows1.Field<string>("FeeCode") equals dataRows2.Field<string>("Code")
                        //                    into outer
                        //                    from dataRows3 in outer.DefaultIfEmpty()
                        //                    //where dataRows3.Field<string>("CodeType") == "FEE" || dataRows3.Field<string>("CodeType") == ""
                        //                      select new
                        //                     {
                        //                         TransID = dataRows1.Field<string>("TransID"),
                        //                         FeeCode = (dataRows3 != null) ? dataRows1.Field<string>("FeeCode") : "",
                        //                         CodeType = (dataRows3 == null) ? "" : dataRows3.Field<string>("CodeType"),
                        //                         FeeType = dataRows1.Field<string>("FeeType"),
                        //                         CodeDesc = (dataRows3 == null) ? dataRows1.Field<string>("FeeType") : dataRows3.Field<string>("CodeDesc"),
                        //                         FeeDesc = dataRows1.Field<string>("FeeDesc"),
                        //                         Origin = dataRows1.Field<string>("Origin"),
                        //                         Transit = dataRows1.Field<string>("Transit"),
                        //                         Destination = dataRows1.Field<string>("Destination"),
                        //                         FeeAmt = objGeneral.RoundUp(Convert.ToDecimal(dataRows1.Field<string>("FeeAmt"))),
                        //                         FeeAmtPerPax = objGeneral.RoundUp(Convert.ToDecimal(dataRows1.Field<string>("FeeAmt")) / Convert.ToDecimal(dataRows1.Field<string>("FeeQty")))
                        //                     }).ToList();

                        //    DataTable dtTransFee = LINQResultToDataTable(TransFee);
                        //    Session["dtTransFee"] = dtTransFee;


                        //}
                        #endregion
                    //}

                    //save booking header

                    bookHDRInfo.TransID = bookDTLInfo.TransID;

                    ht.Add("TransID", bookHDRInfo.TransID);

                    bookHDRInfo.TransType = 0;
                    bookHDRInfo.AgentID = MyUserSet.AgentID;
                    bookHDRInfo.AgentCatgID = MyUserSet.AgentCategoryID;
                    bookHDRInfo.BookingDate = DateTime.Now;
                    bookHDRInfo.STDDate = Convert.ToDateTime(temFlight.TemFlightStd);

                    string expirySetting = objGeneral.getSysValueByKeyAndCarrierCode("STDEXPIRY", keyCarrier);

                    int sysValue = 0;
                    if (expirySetting != "")
                    {
                        sysValue = Convert.ToInt16(expirySetting);
                    }


                    //set expirydate after scheme is assign in savebooking 
                    //bookHDRInfo.ExpiryDate = departDate.Date.AddDays(-sysValue);

                    /*
                    string tempdate1 = String.Format("{0:MM/dd/yyyy}", departDate.Date );
                    string tempdate2 = String.Format("{0:MM/dd/yyyy}", DateTime.Now);
                    TimeSpan ts = Convert.ToDateTime(tempdate1) - Convert.ToDateTime(tempdate2);

                    int tempday = Convert.ToInt32(ts.TotalDays.ToString());
                    if (tempday < 2)
                        bookHDRInfo.ExpiryDate = DateTime.Now;
                    else
                        bookHDRInfo.ExpiryDate = departDate.Date.AddDays(-sysValue);
                    */

                    //Amended by Tyas 20170920 to fix Airbrake issue
                    if (bookHDRInfo.ExpiryDate != null)
                        ht.Add("Expiry", Convert.ToDateTime(bookHDRInfo.ExpiryDate));

                    bookHDRInfo.TransTotalPAX = Convert.ToInt16(temFlight.TemFlightADTNum) + Convert.ToInt16(temFlight.TemFlightCHDNum);
                    bookHDRInfo.CollectedAmt = 0;

                    bookHDRInfo.TransTotalAmt = totalFlightFare;
                    bookHDRInfo.TransSubTotal = totalFlightFare;
                    bookHDRInfo.TransTotalPaxFee = totalPaxFee;
                    bookHDRInfo.TransTotalTax = APT;
                    bookHDRInfo.TransTotalFee = totalServiceFee;
                    bookHDRInfo.TransTotalOth = totalOth;
                    bookHDRInfo.TransTotalDisc = totalDisc;
                    bookHDRInfo.TransTotalPromoDisc = totalPromoDisc;

                    HttpCookie cookie = Request.Cookies["cookieSearchcondition"];
                    if (cookie != null)
                    {
                        if (totalPromoDisc < 0)
                            bookHDRInfo.PromoCode = cookie.Values["PromoCode"].ToString().ToUpper();
                        else
                            bookHDRInfo.PromoCode = "";
                    }

                    bookHDRInfo.TransTotalInfant = totalInfant;
                    //addede by ketee
                    bookHDRInfo.TransTotalVAT = totalServVAT;

                    bookHDRInfo.Currency = Currency;
                    bookHDRInfo.CurrencyPaid = Currency;

                    bookHDRInfo.TransStatus = 0;
                    bookHDRInfo.CreateBy = MyUserSet.AgentID;
                    bookHDRInfo.SyncCreate = DateTime.Now;
                    bookHDRInfo.SyncLastUpd = DateTime.Now;
                    bookHDRInfo.LastSyncBy = MyUserSet.AgentName;

                    //added by ketee 20130625, currencyRate
                    bookHDRInfo.ExchangeRate = currencyRate;

                    //added by ketee, 20170307, all new booking will default the isoverride = 1 mean is GBS2
                    bookHDRInfo.IsOverride = 1;
                    //load fare

                    //20170411 - Sienny (organizationID added to BK_TRANSMAIN)
                    if (Session["OrganizationCode"] != null) organizationID = Session["OrganizationCode"].ToString();
                    bookHDRInfo.OrganizationID = organizationID;

                    decimal avg = 0;
                    decimal dpt = 0;
                    decimal rtn = 0;
                    if (HttpContext.Current.Session["Fare"] != null)
                    {

                        Hashtable htFare = (Hashtable)HttpContext.Current.Session["Fare"];
                        avg = (htFare["Avg"] != null) ? Convert.ToDecimal(htFare["Avg"]) : 0;
                        dpt = Convert.ToDecimal(htFare["Dpt"]);
                        rtn = Convert.ToDecimal(htFare["Rtn"]);
                    }

                    //bookHDRInfo.TotalAmtAVG = Convert.ToDecimal(lblAverageFare.Text);
                    bookHDRInfo.TotalAmtAVG = avg;

                    bookHDRInfo.TotalAmtGoing = dpt + totalInfant;

                    if (LblReturn.Text != "")
                    { bookHDRInfo.TotalAmtReturn = rtn + totalInfant;
                    }
                    else
                    { bookHDRInfo.TotalAmtReturn = 0;
                    }

                    //Added by Tyas 20170607
                    string GroupName = objGeneral.getOPTGroupByCarrierCode(keyCarrier);
                    if (GroupName != "")
                    {
                        string haul = "";
                        if (GroupName == "AA") haul = "SHORT";
                        else if (GroupName == "AAX") haul = "LONG";
                        DataTable dtNameChange = objBooking.GetFeeSettingbyGrpID(GroupName);
                        if (dtNameChange != null && dtNameChange.Rows.Count > 0)
                        {
                            bookHDRInfo.NameChangeMax = Convert.ToInt32(dtNameChange.Rows[0]["ChangeLimit"]);
                            //string[] tokens = dtNameChange.Rows[0]["SYSValueEx"].ToString().Split(';');
                            //bookHDRInfo.NameChangeLimit1 = Convert.ToDecimal(tokens[0]);
                            //bookHDRInfo.NameChangeLimit2 = Convert.ToDecimal(tokens[1]);
                        }
                    }

                    /*
                    bookHDRInfo.TotalAmtGoing = Convert.ToDecimal(lblDepartFare.Text);
                    if (LblReturn.Text != "")
                    { bookHDRInfo.TotalAmtReturn = Convert.ToDecimal(lblReturnFare.Text); }
                    else
                    { bookHDRInfo.TotalAmtReturn = 0; }
                    */

                    string reminder = objGeneral.getSysValueByKeyAndCarrierCode("REMINDDURA1", keyCarrier);

                    if (reminder != "")
                    {
                        sysValue = Convert.ToInt16(reminder);
                    }
                    //bookHDRInfo.CurReminderDate = bookHDRInfo.ExpiryDate.AddDays(-sysValue);

                    reminder = objGeneral.getSysValueByKeyAndCarrierCode("REMINDDURA2", keyCarrier);

                    if (reminder != "")
                    {
                        sysValue = Convert.ToInt16(reminder);
                    }
                    //bookHDRInfo.NextReminderDate = bookHDRInfo.ExpiryDate.AddDays(-sysValue);
                    bookHDRInfo.ReminderType = 1;

                    //load max failed payment try
                    string maxPaymentFail = objGeneral.getSysValueByKeyAndCarrierCode("PAYMENTSUSPEND", keyCarrier);
                    ht.Add("PaymentSuspend", maxPaymentFail);

                    //added by ketee, 20170310, for new booking, set isoverride = 1
                    ht.Add("IsOverride", "1");

                    HttpContext.Current.Session.Remove("HashMain");
                    HttpContext.Current.Session.Add("HashMain", ht);

                //    if (Session["bookHDRInfo"] != null)
                //    bookHDRInfo = (BookingTransactionMain)Session["bookHDRInfo"];
                //if (Session["lstbookDTLInfo"] != null)
                //    lstbookDTLInfo = (List<BookingTransactionDetail>)Session["lstbookDTLInfo"];

                BookingTransactionMain BookingMain = new BookingTransactionMain();
                //BookingMain = objBooking.SaveBooking(bookHDRInfo, lstbookDTLInfo, transTaxInfo, lsttransFeesInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);

                if (bookHDRInfo != null) //amended by diana 20140124 - check for equal total pax
                {
                    if (bookHDRInfo.TransTotalPAX < int.Parse(HttpContext.Current.Session["TotalPax"].ToString()))
                    {
                        Session["PaxStatus"] = "false";
                        return false;
                    }
                    else
                    {
                        BookingMain = objBooking.SaveBooking(bookHDRInfo, lstbookDTLInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
                        if (BookingMain != null && BookingMain.TransID != "")
                        {
                            Session["TransID"] = BookingMain.TransID;
                            return true;
                        }
                        else
                            return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                return false;
            }
            //objBooking.SaveBooking(bookHDRInfo, lstbookDTLInfo, transTaxInfo, lsttransFeesInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
        }

        public DataTable LINQResultToDataTable<T>(IEnumerable<T> Linqlist)
        {
            DataTable dt = new DataTable();


            PropertyInfo[] columns = null;

            if (Linqlist == null) return dt;

            foreach (T Record in Linqlist)
            {

                if (columns == null)
                {
                    columns = ((Type)Record.GetType()).GetProperties();
                    foreach (PropertyInfo GetProperty in columns)
                    {
                        Type IcolType = GetProperty.PropertyType;

                        if ((IcolType.IsGenericType) && (IcolType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            IcolType = IcolType.GetGenericArguments()[0];
                        }

                        dt.Columns.Add(new DataColumn(GetProperty.Name, IcolType));
                    }
                }

                DataRow dr = dt.NewRow();

                foreach (PropertyInfo p in columns)
                {
                    dr[p.Name] = p.GetValue(Record, null) == null ? DBNull.Value : p.GetValue
                    (Record, null);
                }

                dt.Rows.Add(dr);
            }
            return dt;
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            //saving process
            //tmrCount.Enabled = false;
            //SavingProcess() is moved to reviewbooking 
            /*
             * this is to save our time instead of creating new table to save the session details, 
             * after this amendment, system will save the booking details into db with status = 0 
             * even the user still not yet proceed to confirm booking, 
             * so that we can detect and clear the booking that more that 10mins .
             */
            //SavingProcess();
            Response.Redirect(Shared.MySite.PublicPages.ReviewBooking, false);
        }

        protected void ValidatePopup(object sender, CallbackEventArgs e)
        {
            MessageList msgList = new MessageList();
            string errorMessage = string.Empty;
            var profiler = MiniProfiler.Current;
            try
            {
                ABS.Logic.GroupBooking.Agent.AgentProfile agent = new ABS.Logic.GroupBooking.Agent.AgentProfile();
                String OrgID = "";
                agent = (ABS.Logic.GroupBooking.Agent.AgentProfile)Session["agProfileInfo"];
                HttpCookie cookie = Request.Cookies["cookieTemFlight"];
                if (cookie != null)
                {
                    if (IsNumeric(cookie.Values["list1ID"]))
                    {
                        departID = Convert.ToInt32(cookie.Values["list1ID"]);
                    }
                    else
                    {
                        departID = -1;
                    }

                    ReturnID = cookie.Values["ReturnID"];
                    num = Convert.ToInt32(cookie.Values["PaxNum"]);
                }


                OrgID = MyUserSet.OrganicationID;

                ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
                ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight2 = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();

                string strExpr;
                string strSort;
                DataTable dt = new DataTable();
                strExpr = "TemFlightId = '" + departID + "'";
                strSort = "";

                dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                if (foundRows.Length > 0)
                {
                    FillModelFromDataRow(foundRows, ref temFlight);
                }
                else
                {
                    Response.Redirect(Shared.MySite.PublicPages.SessionExpired, false);
                }

                string getfare = temFlight.TemFlightServiceCharge.ToString();
                if (agent != null) OrgID = agent.OrgID.ToString();

                if (ReturnID != "")
                {
                    strExpr = "TemFlightId = '" + ReturnID + "'";
                    strSort = "";

                    dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                    foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                    if (foundRows.Length > 0)
                    {
                        FillModelFromDataRow(foundRows, ref temFlight2);
                    }
                    else
                    {
                        Response.Redirect(Shared.MySite.PublicPages.SessionExpired, false);
                    }


                    //string LoginType = MyUserSet.AgentType.ToString();  //Session["LoginType"].ToString();
                    string LoginType = "PublicAgent";
                    string LoginName = MyUserSet.AgentName;
                    string LoginPWD = "";
                    string LoginDomain = "";

                    //objBooking.SellFlightByTem(temFlight, temFlight2, LoginType, LoginName, LoginPWD, LoginDomain, "");
                    log.Info(this, "Entering return Flight Saving.");

                    using (profiler.Step("objBooking.SellJourney"))
                    {
                        if (objBooking.SellJourney(temFlight, temFlight2, LoginType, LoginName, LoginPWD, LoginDomain, "", OrgID) == false)
                        {

                            //Page.ClientScript.RegisterStartupScript(this.GetType(), "PostbackKey", "<script type='text/javascript'>document.getElementById('ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg').innerHTML = 'eko';window.location.href='../public/selectflight.aspx';</script>");
                            if (HttpContext.Current.Session["errormsg"] != null && Session["errormsg"].ToString().ToLower().Contains("sold"))
                            {
                                Session["soldout"] = true;
                                Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                            }
                            else if (HttpContext.Current.Session["errormsg"] != null && Session["errormsg"].ToString().ToLower().Contains("overlap"))
                            {
                                Session["overlap"] = true;
                                Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);

                            }
                            else
                            {
                                Session["error"] = Session["errormsg"].ToString();
                                Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                            }

                        }
                    }

                }
                else
                {
                    string LoginType = "PublicAgent";
                    string LoginName = MyUserSet.AgentName;
                    string LoginPWD = "";
                    string LoginDomain = "";
                    using (profiler.Step("objBooking.SellFlightByTem"))
                    {
                        if (objBooking.SellFlightByTem(temFlight, LoginType, LoginName, LoginPWD, LoginDomain, "", OrgID) == false)
                        {
                            if (HttpContext.Current.Session["errormsg"] != null && Session["errormsg"].ToString().ToLower().Contains("sold"))
                            {
                                Session["soldout"] = true;
                                Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                            }
                            else if (HttpContext.Current.Session["errormsg"] != null && Session["errormsg"].ToString().ToLower().Contains("overlap"))
                            {
                                Session["overlap"] = true;
                                Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                            }
                            else
                            {
                                Session["error"] = Session["errormsg"].ToString();
                                Response.Redirect(Shared.MySite.PublicPages.Selectflight, false);
                            }
                        }
                    }
                }

                using (profiler.Step("SavingProcess"))
                {
                    SavingProcess();
                }

                //Timer1.Enabled = false;

                DataTable dataClass = objBooking.dtClass();
                dataClass = (DataTable)HttpContext.Current.Session["dataClassTrans"];
                if (dataClass != null)
                {
                    for (int i = 0; i < dataClass.Rows.Count; i++)
                    {
                        ABS.Navitaire.APIBooking APIBooking = new ABS.Navitaire.APIBooking(dataClass.Rows[i]["SellSignature"].ToString());
                        ABS.Navitaire.BookingManager.Booking books = new ABS.Navitaire.BookingManager.Booking();// APIBooking.GetBookingFromState(dataClass.Rows[i]["SellSignature"].ToString());
                        using (profiler.Step("Navitaire:GetBookingFromState"))
                        {
                            books = APIBooking.GetBookingFromState(dataClass.Rows[i]["SellSignature"].ToString());
                        }

                        if (books == null || books.Journeys.Length <= 0 || books.Journeys[0].Segments.Length <= 0)
                        {
                            e.Result = msgList.Err100068;
                            hCommand.Value = errorMessage;
                        }
                    }
                }

                e.Result = "";

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                errorMessage = ex.Message;
                e.Result = ex.Message;
                hCommand.Value = errorMessage;
                //lblErr.Text = ex.ToString();
            }


        }


    }
}