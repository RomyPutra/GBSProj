using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Collections;
using System.Web.UI.WebControls;
using ABS.Logic.Shared;
using DevExpress.Web;
using System.Globalization;
//using log4net;
using ABS.Logic.GroupBooking.Booking;
using ABS.Navitaire.AccountManager;
using ABS.Navitaire.AgentManager;
using ABS.Navitaire.BookingManager;
using System.Threading;
using System.Text;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Configuration;
using ABS.Logic.GroupBooking;
using ABS.GBS.Log;
using StackExchange.Profiling;

namespace GroupBooking.Web
{
    public partial class SelectSeat : System.Web.UI.Page
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

        BookingTransactionDetail bookDTLInfo = new BookingTransactionDetail();
        List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
        List<BookingTransactionDetail> listBookingDetail = new List<BookingTransactionDetail>();
        BookingTransactionMain bookHDRInfo = new BookingTransactionMain();
        ABS.Logic.GroupBooking.Agent.AgentControl objAgent = new ABS.Logic.GroupBooking.Agent.AgentControl();
        ABS.Logic.GroupBooking.Agent.AgentProfileControl objAgentProfile = new ABS.Logic.GroupBooking.Agent.AgentProfileControl();
        decimal totalFlightFare, totalServiceFee, totalBaggageFare = 0, totalServVAT;
        ABS.Logic.GroupBooking.Agent.AgentProfile agent = new ABS.Logic.GroupBooking.Agent.AgentProfile();

        string Currency = "USD";
        decimal APT = 0;
        decimal AVGFare = 0;
        string tranID = "";
        DataTable dtTaxFees = new DataTable();

        private static int num = 0;
        private static int departID = 0;
        private static string ReturnID = "";
        DataTable dtTransMain;
        string TransId;
        string Signature = "";

        EnumFlight eFlight, eFlight2;

        ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model = null;
        ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model2 = null;

        //added by ketee, 20170120, save total connecting flight seat fees
        ////string[,] totalConnectingSeatFees;
        ////string[,] totalConnectingSeatFees2;

        BookingTransactionFees bookFEEInfo = new BookingTransactionFees();
        List<BookingTransactionFees> lstbookFEEInfo = new List<BookingTransactionFees>();

        public enum EnumFlight
        {
            DirectFlight = 1,
            ConnectingFlight = 2
        }

        public enum EnumFlightType
        {
            DepartFlight = 1,
            ReturnFlight = 2,
            DepartConnectingFlight = 3,
            ReturnConnectingFlight = 4,
            DepartConnectingFlight2 = 5,
            ReturnConnectingFlight2 = 6
        }
        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Expires = 0;
            Response.Cache.SetNoStore();
            Response.AppendHeader("Pragma", "no-cache");
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;
            try
            {
                using (profiler.Step("GBS:InitializeForm"))
                {
                    InitializeForm();
                }
                if (!SetAccess())
                {
                    return;
                }
                if (IsPostBack == false)
                {
                    Session["dtInfant"] = null;
                    using (profiler.Step("GBS:intSeatTabSession"))
                    {
                        intSeatTabSession();
                    }
                    if (Session["btnSelected"] == null)
                    {
                        Session["btnSelected"] = 0;
                    }
                }

                if ((int)Session["btnSelected"] == 0 || (int)Session["btnSelected"] == 1)
                    using (profiler.Step("GBS:FillFlight"))
                    {
                        FillFlight(model.TemFlightJourneySellKey, (int)Session["btnSelected"]);
                    }
                else if (((int)Session["btnSelected"] == 2 || (int)Session["btnSelected"] == 3) && model2 != null)
                    using (profiler.Step("FillFlight"))
                    {
                        FillFlight(model2.TemFlightJourneySellKey, (int)Session["btnSelected"]);
                    }

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
                Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.InvalidPage);
            }
        }
        #endregion

        #region Control
        protected void btnSeatDepart1_ServerClick(object sender, EventArgs e)
        {
            pnlErr.Visible = false;
            Session["btnSelected"] = 0;
            Session["ErrorMsg"] = null;
            //'ReservedSeat()
            if (Request.QueryString["change"] != null)
            {
                Response.Redirect("selectseat.aspx?change=true");
            }
            else
            {
                Response.Redirect("selectseat.aspx");
            }
        }

        protected void btnSeatDepart2_ServerClick(object sender, EventArgs e)
        {
            pnlErr.Visible = false;
            Session["btnSelected"] = 1;
            Session["ErrorMsg"] = null;
            //'ReservedSeat()
            if (Request.QueryString["change"] != null)
            {
                Response.Redirect("selectseat.aspx?change=true");
            }
            else
            {
                Response.Redirect("selectseat.aspx");
            }
        }
        protected void btnSeatReturn1_ServerClick(object sender, EventArgs e)
        {
            pnlErr.Visible = false;
            Session["btnSelected"] = 2;
            Session["ErrorMsg"] = null;
            //'ReservedSeat()
            if (Request.QueryString["change"] != null)
            {
                Response.Redirect("selectseat.aspx?change=true");
            }
            else
            {
                Response.Redirect("selectseat.aspx");
            }
        }

        protected void btnSeatReturn2_ServerClick(object sender, EventArgs e)
        {
            pnlErr.Visible = false;
            Session["btnSelected"] = 3;
            Session["ErrorMsg"] = null;
            //'ReservedSeat()
            if (Request.QueryString["change"] != null)
            {
                Response.Redirect("selectseat.aspx?change=true");
            }
            else
            {
                Response.Redirect("selectseat.aspx");
            }
        }
        #endregion

        #region Function and Procedure
        protected void InitializeForm()
        {
            var profiler = MiniProfiler.Current;
            try
            {
                if (Session["TransID"] != null)
                {
                    TransId = (string)Session["TransID"];
                }
                using (profiler.Step("GBS:SetCookie"))
                {
                    SetCookie();
                }
                using (profiler.Step("GBS:BindModel"))
                {
                    BindModel();
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
        }
        private bool SetAccess()
        {
            try
            {
                if (model == null && model2 == null)
                {
                    //lblErr.Text = "Flight not found, please contact HelpDesk for further information.";
                    return false;
                }

                //Amended by Tyas 20170920 to fix Airbrake issue
                if (model != null && model.TemFlightJourneySellKey != null)
                {
                    if (model.TemFlightJourneySellKey.Contains("^"))
                    {
                        eFlight = EnumFlight.ConnectingFlight;
                    }
                    else
                    {
                        eFlight = EnumFlight.DirectFlight;
                    }
                }

                if (model2 != null && model2.TemFlightJourneySellKey != null)
                {
                    if (model2.TemFlightJourneySellKey.Contains("^"))
                    {
                        eFlight2 = EnumFlight.ConnectingFlight;
                    }
                    else
                    {
                        eFlight2 = EnumFlight.DirectFlight;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                return false;
            }
        }

        private void SetCookie()
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
        }

        private void BindModel()
        {
            if (departID != -1)
            {
                model = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
                ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
                string strExpr;
                string strSort;
                DataTable dt = objBooking.dtFlight();
                strExpr = "TemFlightId = '" + departID + "'";
                strSort = "";
                //Amended by Tyas 20170920 to fix Airbrake issue
                if (Session["TempFlight"] != null) dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                FillModelFromDataRow(foundRows, ref model);


                decimal total = temFlight.TemFlightTotalAmount;


                if (ReturnID != "")
                {
                    model2 = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
                    strExpr = "TemFlightId = '" + ReturnID + "'";
                    strSort = "";
                    //Amended by Tyas 20170920 to fix Airbrake issue
                    if (Session["TempFlight"] != null) dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                    foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                    FillModelFromDataRow(foundRows, ref model2);

                    total += temFlight.TemFlightTotalAmount;

                }

                //20170530 - Sienny (put amount due to session)
                if (Session["TotalAmountDue"] != null)
                {
                    Session["TotalAmountDue"] = objGeneral.RoundUp(Convert.ToDecimal(Session["TotalAmountDue"].ToString()) + total).ToString("N", nfi);
                }
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
                model.TemFlightJourneySellKey = foundRows[0]["TemFlightJourneySellKey"].ToString();
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
                if (IsNumeric(foundRows[0]["TemFlightFarePrice"].ToString()))
                { model.temFlightfarePrice = Convert.ToDecimal(foundRows[0]["TemFlightFarePrice"]); }
                if (IsNumeric(foundRows[0]["TemFlightAPT"].ToString()))
                { model.TemFlightApt = Convert.ToDecimal(foundRows[0]["TemFlightAPT"]); }
                if (IsNumeric(foundRows[0]["TemFlightFuel"].ToString()))
                { model.TemFlightFuel = Convert.ToDecimal(foundRows[0]["TemFlightFuel"]); }
                model.TemFlightTransit = foundRows[0]["TemFlightTransit"].ToString();
                DateTime sta2;
                if (DateTime.TryParse(foundRows[0]["TemFlightSta2"].ToString(), out sta2))
                    model.TemFlightSta2 = sta2;
                DateTime std2;
                if (DateTime.TryParse(foundRows[0]["TemFlightStd2"].ToString(), out std2))
                    model.TemFlightStd2 = std2;
                model.TemFlightCarrierCode2 = foundRows[0]["TemFlightCarrierCode2"].ToString();
                model.TemFlightFlightNumber2 = foundRows[0]["TemFlightFlightNumber2"].ToString();
                model.TemFlightOpSuffix = foundRows[0]["TemFlightOpSuffix"].ToString();
                model.TemFlightOpSuffix2 = foundRows[0]["TemFlightOpSuffix2"].ToString();
                model.TemFlightSignature = foundRows[0]["TemFlightSignature"].ToString();

                model.TemFlightPromoCode = foundRows[0]["TemFlightPromoCode"].ToString();
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
        }

        private void intSeatTabSession()
        {
            switch (eFlight)
            {
                case EnumFlight.ConnectingFlight:
                    if (model == null == false)
                    {
                        if (CheckDefaultSeatExist(EnumFlightType.DepartConnectingFlight))
                        {
                            btnSeatDepart1.Value = model.TemFlightDeparture + " - " + model.TemFlightTransit;
                            btnSeatDepart2.Value = model.TemFlightTransit + " - " + model.TemFlightArrival;
                            btnSeatDepart1.Visible = true;
                            btnSeatDepart2.Visible = true;
                        }
                        else
                        {
                            btnSeatDepart1.Visible = false;
                            btnSeatDepart2.Visible = false;
                        }
                    }
                    else
                    {
                        //added by ketee
                        btnSeatDepart1.Visible = false;
                        btnSeatDepart2.Visible = false;
                    }
                    break;
                case EnumFlight.DirectFlight:
                    if (model == null == false)
                    {
                        if (CheckDefaultSeatExist(EnumFlightType.DepartFlight))
                        {
                            btnSeatDepart1.Value = model.TemFlightDeparture + " - " + model.TemFlightArrival;
                            btnSeatDepart1.Visible = true;
                        }
                        else
                        {
                            btnSeatDepart1.Visible = false;
                        }

                    }
                    else
                    {
                        //added by ketee
                        btnSeatDepart1.Visible = false;
                    }
                    btnSeatDepart2.Visible = false;
                    btnSeatReturn1.Visible = false;
                    btnSeatReturn2.Visible = false;
                    break;
            }
            switch (eFlight2)
            {
                case EnumFlight.ConnectingFlight:
                    if (model2 == null == false)
                    {
                        if (CheckDefaultSeatExist(EnumFlightType.ReturnConnectingFlight))
                        {
                            btnSeatReturn1.Value = model2.TemFlightDeparture + " - " + model2.TemFlightTransit;
                            btnSeatReturn2.Value = model2.TemFlightTransit + " - " + model2.TemFlightArrival;
                            btnSeatReturn1.Visible = true;
                            btnSeatReturn2.Visible = true;
                        }
                        else
                        {
                            btnSeatReturn1.Visible = false;
                            btnSeatReturn2.Visible = false;
                        }

                    }
                    else
                    {
                        btnSeatReturn1.Visible = false;
                        btnSeatReturn2.Visible = false;
                    }
                    break;
                case EnumFlight.DirectFlight:
                    if (model2 == null == false)
                    {
                        if (CheckDefaultSeatExist(EnumFlightType.ReturnFlight))
                        {
                            btnSeatReturn1.Value = model2.TemFlightDeparture + " - " + model2.TemFlightArrival;
                            btnSeatReturn1.Visible = true;
                        }
                        else
                        {
                            btnSeatReturn1.Visible = false;
                        }
                    }
                    else
                    {
                        btnSeatReturn1.Visible = false;
                    }
                    btnSeatReturn2.Visible = false;
                    break;
                default:
                    btnSeatReturn1.Visible = false;
                    btnSeatReturn2.Visible = false;
                    break;

            }

        }

        private void FillFlight(string RecordLocator, int btnSelected = 0)
        {
            var profiler = MiniProfiler.Current;
            ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");
            MessageList msgList = new MessageList();
            try
            {
                //dison
                ////if ((int)Session["LiftStatusDepart"] == 2)
                ////{
                ////    Session["DepartFlightInfo"] = null;
                ////}
                ////if ((int)Session["LiftStatusReturn"] == 2)
                ////{
                ////    Session["ReturnFlightInfo"] = null;
                ////}


                //call function
                string DepartXml = "";
                //"GetSeatAvailability_Response.xml"
                string ReturnXml = "";
                //"GetSeatAvailability_Response.xml"
                int DepartPax = 0;
                int ReturnPax = 0;
                string DepartDefaultSeat = "";
                //0_Y_1_2D,0_Y_1_3D
                string ReturnDefaultSeat = "";
                string DepartFromTo = "";
                //YOGYAKARTA (JOG) - JAKARTA (CGK)
                string DepartFromToShort = "";
                //JOG - CGK
                string DepartFromTo2 = "";
                //YOGYAKARTA (JOG) - JAKARTA (CGK)
                string DepartFromToShort2 = "";
                //QZ 7342
                string ReturnFromTo = "";
                string ReturnFromToShort = "";
                string ReturnFromTo2 = "";
                string ReturnFromToShort2 = "";
                string PassengerHeader = "";
                string PassengerSum = "";
                //added by ketee
                string hotseat = "";
                string Signature = "";

                //Flight_Info pDepartFlightInfo = new Flight_Info();
                //Flight_Info pReturnFlightInfo = new Flight_Info();
                SeatAvailabilityResponse pAvailableSeatInfo = new SeatAvailabilityResponse();

                //if (Session["signature"] != null)
                //{
                //    string Signature = (string)Session["signature"];
                //}
                //Added by Tyas, 20170404 to get from Session["dataClassTrans"]
                if (HttpContext.Current.Session["dataClassTrans"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)HttpContext.Current.Session["dataClassTrans"];

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        Signature = dt.Rows[0]["SellSignature"].ToString();
                    }
                }

                EnumFlight eFlights = eFlight;
                BookingControl.TemFlight models = model;
                int selectedBtn = btnSelected;
                if (selectedBtn >= 2)
                {
                    if (eFlight2 == EnumFlight.ConnectingFlight) eFlights = eFlight2;
                    models = model2;

                    //if (eFlights == EnumFlight.DirectFlight)
                    //{
                    //    if (selectedBtn == 2)
                    //        selectedBtn = 0;
                    //    else if (selectedBtn == 3)
                    //        selectedBtn = 1;
                    //}
                }

                switch (btnSelected)
                {
                    case 0:
                        btnSeatDepart1.Style[HtmlTextWriterStyle.BackgroundImage] = "../images/CBackground2.png";
                        break;
                    case 1:
                        btnSeatDepart2.Style[HtmlTextWriterStyle.BackgroundImage] = "../images/CBackground2.png";
                        break;
                    case 2:
                        btnSeatReturn1.Style[HtmlTextWriterStyle.BackgroundImage] = "../images/CBackground2.png";
                        break;
                    case 3:
                        btnSeatReturn2.Style[HtmlTextWriterStyle.BackgroundImage] = "../images/CBackground2.png";
                        break;
                }
                Boolean IsInternationalFlight = objGeneral.IsInternationalFlight(model.TemFlightDeparture, model.TemFlightArrival, Request.PhysicalApplicationPath);
                hfIsInternational.Value = IsInternationalFlight.ToString();
                switch (eFlights)
                {
                    case EnumFlight.DirectFlight:

                        if ((models != null))
                        {
                            if (selectedBtn == 0)
                            {
                                if (Session["SeatInfo0Xml"] == null == false)
                                {
                                    DeleteXML((string)Session["SeatInfo0Xml"]);

                                }
                                if (models.TemFlightTransit.Trim() != "")
                                    using (profiler.Step("API:GetSeatAvailability"))
                                    {
                                        pAvailableSeatInfo = absNavitaire.GetSeatAvailability(RecordLocator, 0, Signature, models.TemFlightCarrierCode, models.TemFlightFlightNumber,
                                    models.TemFlightOpSuffix, models.TemFlightDeparture, models.TemFlightTransit, (DateTime)models.TemFlightStd, Request.PhysicalApplicationPath);
                                    }
                                else
                                    using (profiler.Step("API:GetSeatAvailability"))
                                    {
                                        pAvailableSeatInfo = absNavitaire.GetSeatAvailability(RecordLocator, 0, Signature, models.TemFlightCarrierCode, models.TemFlightFlightNumber,
                                    models.TemFlightOpSuffix, models.TemFlightDeparture, models.TemFlightArrival, (DateTime)models.TemFlightStd, Request.PhysicalApplicationPath);
                                    }
                                Session["pAvailableSeatInfo0"] = pAvailableSeatInfo;
                                //btnSeatDepart1.Style[HtmlTextWriterStyle.BackgroundImage] = "../images/CBackground2.png";
                            }
                            else if (selectedBtn == 1)
                            {
                                if (Session["SeatInfo1Xml"] == null == false)
                                {
                                    DeleteXML((string)Session["SeatInfo1Xml"]);

                                }
                                if (models.TemFlightTransit.Trim() != "")
                                    using (profiler.Step("API:GetSeatAvailability"))
                                    {
                                        pAvailableSeatInfo = absNavitaire.GetSeatAvailability(RecordLocator, 0, Signature, models.TemFlightCarrierCode, models.TemFlightFlightNumber,
                                    models.TemFlightOpSuffix, models.TemFlightTransit, models.TemFlightArrival, (DateTime)models.TemFlightStd, Request.PhysicalApplicationPath);
                                    }
                                else
                                    using (profiler.Step("API:GetSeatAvailability"))
                                    {
                                        pAvailableSeatInfo = absNavitaire.GetSeatAvailability(RecordLocator, 0, Signature, models.TemFlightCarrierCode, models.TemFlightFlightNumber,
                                    models.TemFlightOpSuffix, models.TemFlightDeparture, models.TemFlightArrival, (DateTime)models.TemFlightStd, Request.PhysicalApplicationPath);
                                    }
                                Session["pAvailableSeatInfo1"] = pAvailableSeatInfo;
                                //btnSeatDepart2.Style[HtmlTextWriterStyle.BackgroundImage] = "../images/CBackground2.png";
                            }
                            else if (selectedBtn == 2)
                            {
                                if (Session["SeatInfo2Xml"] == null == false)
                                {
                                    using (profiler.Step("GBS:DeleteXML"))
                                    {
                                        DeleteXML((string)Session["SeatInfo2Xml"]);
                                    }
                                }
                                if (models.TemFlightTransit.Trim() != "")
                                    using (profiler.Step("API:GetSeatAvailability"))
                                    {
                                        pAvailableSeatInfo = absNavitaire.GetSeatAvailability(RecordLocator, 0, Signature, models.TemFlightCarrierCode, models.TemFlightFlightNumber, models.TemFlightOpSuffix,
                                                                                        models.TemFlightDeparture, models.TemFlightTransit, (DateTime)models.TemFlightStd, Request.PhysicalApplicationPath);
                                    }
                                else
                                    using (profiler.Step("API:GetSeatAvailability"))
                                    {
                                        pAvailableSeatInfo = absNavitaire.GetSeatAvailability(RecordLocator, 0, Signature, models.TemFlightCarrierCode, models.TemFlightFlightNumber, models.TemFlightOpSuffix,
                                                                                        models.TemFlightDeparture, models.TemFlightArrival, (DateTime)models.TemFlightStd, Request.PhysicalApplicationPath);
                                    }
                                //Session["Click"] = Nothing
                                Session["pAvailableSeatInfo2"] = pAvailableSeatInfo;
                                //btnSeatReturn1.Style[HtmlTextWriterStyle.BackgroundImage] = "images/CBackground2.png";
                            }
                            else if (selectedBtn == 3)
                            {
                                if (Session["SeatInfo3Xml"] == null == false)
                                {
                                    using (profiler.Step("GBS:DeleteXML"))
                                    {
                                        DeleteXML((string)Session["SeatInfo3Xml"]);
                                    }

                                }
                                if (models.TemFlightTransit.Trim() != "")
                                    using (profiler.Step("API:GetSeatAvailability"))
                                    {
                                        pAvailableSeatInfo = absNavitaire.GetSeatAvailability(RecordLocator, 0, Signature, models.TemFlightCarrierCode2, models.TemFlightFlightNumber2, models.TemFlightOpSuffix2,
                                                                                        models.TemFlightTransit, models.TemFlightArrival, (DateTime)models.TemFlightStd2, Request.PhysicalApplicationPath);
                                    }
                                else
                                    using (profiler.Step("API:GetSeatAvailability"))
                                    {
                                        pAvailableSeatInfo = absNavitaire.GetSeatAvailability(RecordLocator, 0, Signature, models.TemFlightCarrierCode2, models.TemFlightFlightNumber2, models.TemFlightOpSuffix2,
                                                                                        models.TemFlightDeparture, models.TemFlightArrival, (DateTime)models.TemFlightStd2, Request.PhysicalApplicationPath);
                                    }
                                //Session["Click"] = Nothing
                                Session["pAvailableSeatInfo3"] = pAvailableSeatInfo;
                                //btnSeatReturn2.Style[HtmlTextWriterStyle.BackgroundImage] = "images/CBackground2.png";
                            }


                            if ((pAvailableSeatInfo != null))
                            {

                                if (!string.IsNullOrEmpty(absNavitaire.DepartXmlUrl))
                                {
                                    DepartXml = absNavitaire.DepartXmlUrl;
                                    if (selectedBtn == 0)
                                    {
                                        DepartPax = models.TemFlightPaxNum;
                                        if (models.TemFlightTransit.Trim() != "")
                                        {
                                            DepartFromTo = models.TemFlightDeparture + "-" + models.TemFlightTransit;
                                            DepartFromToShort = models.TemFlightDeparture + "-" + models.TemFlightTransit;
                                        }
                                        else
                                        {
                                            DepartFromTo = models.TemFlightDeparture + "-" + models.TemFlightArrival;
                                            DepartFromToShort = models.TemFlightDeparture + "-" + models.TemFlightArrival;
                                        }
                                    }
                                    else if (selectedBtn == 1)
                                    {
                                        DepartPax = models.TemFlightPaxNum;
                                        if (models.TemFlightTransit.Trim() != "")
                                        {
                                            DepartFromTo2 = models.TemFlightTransit + "-" + models.TemFlightArrival;
                                            DepartFromToShort2 = models.TemFlightTransit + "-" + models.TemFlightArrival;
                                        }
                                        else
                                        {
                                            DepartFromTo2 = models.TemFlightDeparture + "-" + models.TemFlightArrival;
                                            DepartFromToShort2 = models.TemFlightDeparture + "-" + models.TemFlightArrival;
                                        }
                                    }
                                    else if (selectedBtn == 2)
                                    {
                                        DepartPax = models.TemFlightPaxNum;
                                        if (models.TemFlightTransit.Trim() != "")
                                        {
                                            ReturnFromTo = models.TemFlightDeparture + "-" + models.TemFlightTransit;
                                            ReturnFromToShort = models.TemFlightDeparture + "-" + models.TemFlightTransit;
                                        }
                                        else
                                        {
                                            ReturnFromTo = models.TemFlightDeparture + "-" + models.TemFlightArrival;
                                            ReturnFromToShort = models.TemFlightDeparture + "-" + models.TemFlightArrival;
                                        }
                                    }
                                    else
                                    {
                                        DepartPax = models.TemFlightPaxNum;
                                        if (models.TemFlightTransit.Trim() != "")
                                        {
                                            ReturnFromTo2 = models.TemFlightTransit + "-" + models.TemFlightArrival;
                                            ReturnFromToShort2 = models.TemFlightTransit + "-" + models.TemFlightArrival;
                                        }
                                        else
                                        {
                                            ReturnFromTo2 = models.TemFlightDeparture + "-" + models.TemFlightArrival;
                                            ReturnFromToShort2 = models.TemFlightDeparture + "-" + models.TemFlightArrival;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                lblErr.Text = msgList.Err999998;
                                pnlErr.Visible = true;
                                return;
                            }
                            //End If


                        }
                        else
                        {
                        }
                        if (!string.IsNullOrEmpty(DepartXml))
                        {
                            ctlDepart.Style["display"] = "";
                            PassengerHeader = "<div id='passengerListHeader" + btnSelected + "' class='redSectionHeader'>";
                            PassengerHeader += "<div>Seat summary</div></div>";
                            PassengerHeader += "<div id='passengerListBody" + btnSelected + "' class='sectionBody'><br/>";
                            if (selectedBtn == 0)
                            {
                                using (profiler.Step("GBS:CreateSeatControl"))
                                {
                                    PassengerSum = CreateSeatControl(DepartPax, DepartFromToShort, AssignSeatInfo(EnumFlightType.DepartFlight, RecordLocator, (SeatAvailabilityResponse)Session["pAvailableSeatInfo0"]), EnumFlightType.DepartFlight);
                                }
                            }
                            else if (selectedBtn == 1)
                            {
                                using (profiler.Step("GBS:CreateSeatControl"))
                                {
                                    PassengerSum = CreateSeatControl(DepartPax, DepartFromToShort2, AssignSeatInfo(EnumFlightType.DepartConnectingFlight, RecordLocator, (SeatAvailabilityResponse)Session["pAvailableSeatInfo1"]), EnumFlightType.DepartConnectingFlight);
                                }
                            }
                            else if (selectedBtn == 2)
                            {
                                using (profiler.Step("GBS:CreateSeatControl"))
                                {
                                    PassengerSum = CreateSeatControl(ReturnPax, ReturnFromToShort, AssignSeatInfo(EnumFlightType.ReturnFlight, RecordLocator, (SeatAvailabilityResponse)Session["pAvailableSeatInfo2"]), EnumFlightType.ReturnFlight);
                                }
                            }
                            else if (selectedBtn == 3)
                            {
                                using (profiler.Step("GBS:CreateSeatControl"))
                                {
                                    PassengerSum = CreateSeatControl(ReturnPax, ReturnFromToShort2, AssignSeatInfo(EnumFlightType.ReturnConnectingFlight, RecordLocator, (SeatAvailabilityResponse)Session["pAvailableSeatInfo3"]), EnumFlightType.ReturnConnectingFlight);
                                }
                            }

                            if (!string.IsNullOrEmpty(PassengerSum))
                            {
                                PassengerSummary.Style["display"] = "";
                                PassengerSummary.InnerHtml = PassengerHeader + PassengerSum + "</div>";
                            }

                            if (selectedBtn == 0)
                            {
                                ss.SeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"];

                                Session["SeatInfo0Xml"] = DepartXml;
                            }
                            else if (selectedBtn == 1)
                            {
                                ss.SeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"];

                                Session["SeatInfo1Xml"] = DepartXml;
                            }
                            else if (selectedBtn == 2)
                            {
                                ss.SeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"];

                                Session["SeatInfo2Xml"] = ReturnXml;
                            }
                            else if (selectedBtn == 3)
                            {
                                ss.SeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo3"];

                                Session["SeatInfo3Xml"] = ReturnXml;
                            }

                            if ((ss.SeatInfo != null))
                            {
                                foreach (ABS.Logic.GroupBooking.SeatInfo RowSeatInfo in ss.SeatInfo)
                                {
                                    if (string.IsNullOrEmpty(DepartDefaultSeat))
                                    {
                                        DepartDefaultSeat = "0_" + RowSeatInfo.CompartmentDesignator + "_" + RowSeatInfo.Deck + "_" + RowSeatInfo.SelectedSeat;
                                        //DepartDefaultSeat = "0_" + RowSeatInfo.CompartmentDesignator + "_" + RowSeatInfo.Deck + "_";
                                        //added by ketee hot seat
                                        hotseat = RowSeatInfo.IsHotSeat.ToString();
                                    }
                                    else
                                    {
                                        DepartDefaultSeat += ",0_" + RowSeatInfo.CompartmentDesignator + "_" + RowSeatInfo.Deck + "_" + RowSeatInfo.SelectedSeat;
                                        //DepartDefaultSeat += ",0_" + RowSeatInfo.CompartmentDesignator + "_" + RowSeatInfo.Deck + "_";
                                        //added by ketee
                                        hotseat += "," + RowSeatInfo.IsHotSeat;
                                    }
                                }
                            }
                            ss.defaultseat = DepartDefaultSeat;
                            //'remark by ketee, no default seat assign
                            ss.hotseat = hotseat;
                            ss.numberofpassenger = DepartPax;
                            ss.overwritepassengerindex = DepartPax;
                            ss.xmlurl = DepartXml;
                        }

                        break;
                    case EnumFlight.ConnectingFlight:
                        if (btnSeatDepart1.Visible == false & btnSeatDepart2.Visible == false)
                        {
                            if (selectedBtn == 0)
                            {
                                selectedBtn = 2;
                            }
                        }
                        if (selectedBtn < 2)
                        {

                            if (models != null)
                            {
                                //pDepartFlightInfo = Session["DepartFlightInfo"];

                                if (selectedBtn == 0)
                                {
                                    if (Session["SeatInfo0Xml"] == null == false)
                                    {
                                        DeleteXML((string)Session["SeatInfo0Xml"]);

                                    }
                                    if (models.TemFlightTransit.Trim() != "")
                                        using (profiler.Step("API:GetSeatAvailability"))
                                        {
                                            pAvailableSeatInfo = absNavitaire.GetSeatAvailability(RecordLocator, 0, Signature, models.TemFlightCarrierCode, models.TemFlightFlightNumber, models.TemFlightOpSuffix,
                                                                                            models.TemFlightDeparture, models.TemFlightTransit, (DateTime)models.TemFlightStd, Request.PhysicalApplicationPath);

                                        }
                                    else
                                        using (profiler.Step("API:GetSeatAvailability"))
                                        {
                                            pAvailableSeatInfo = absNavitaire.GetSeatAvailability(RecordLocator, 0, Signature, models.TemFlightCarrierCode, models.TemFlightFlightNumber, models.TemFlightOpSuffix,
                                                                                                models.TemFlightDeparture, models.TemFlightArrival, (DateTime)models.TemFlightStd, Request.PhysicalApplicationPath);
                                        }
                                    
                                            //Session["Click"] = Nothing
                                    Session["pAvailableSeatInfo0"] = pAvailableSeatInfo;
                                    //btnSeatDepart1.Style[HtmlTextWriterStyle.BackgroundImage] = "images/CBackground2.png";
                                }
                                else if (selectedBtn == 1)
                                {
                                    if (Session["SeatInfo1Xml"] == null == false)
                                    {
                                        DeleteXML((string)Session["SeatInfo1Xml"]);

                                    }
                                    if (models.TemFlightTransit.Trim() != "")
                                        using (profiler.Step("API:GetSeatAvailability"))
                                        {
                                            pAvailableSeatInfo = absNavitaire.GetSeatAvailability(RecordLocator, 0, Signature, models.TemFlightCarrierCode2, models.TemFlightFlightNumber2, models.TemFlightOpSuffix2,
                                                                                            models.TemFlightTransit, models.TemFlightArrival, (DateTime)models.TemFlightStd2, Request.PhysicalApplicationPath);
                                        }
                                    else
                                        using (profiler.Step("API:GetSeatAvailability"))
                                        {
                                            pAvailableSeatInfo = absNavitaire.GetSeatAvailability(RecordLocator, 0, Signature, models.TemFlightCarrierCode2, models.TemFlightFlightNumber2, models.TemFlightOpSuffix2,
                                                                                            models.TemFlightDeparture, models.TemFlightArrival, (DateTime)models.TemFlightStd2, Request.PhysicalApplicationPath);
                                        }
                                    //Session["Click"] = Nothing
                                    Session["pAvailableSeatInfo1"] = pAvailableSeatInfo;
                                    //btnSeatDepart2.Style[HtmlTextWriterStyle.BackgroundImage] = "images/CBackground2.png";
                                }


                                if ((pAvailableSeatInfo != null))
                                {

                                    if (!string.IsNullOrEmpty(absNavitaire.DepartXmlUrl))
                                    {
                                        DepartXml = absNavitaire.DepartXmlUrl;
                                        //pDepartFlightInfo.XmlURL = DepartXml;
                                        //DepartXml = "test4.xml"
                                        DepartPax = models.TemFlightPaxNum;

                                        if (models.TemFlightTransit.Trim() != "")
                                        {
                                            DepartFromTo = models.TemFlightDeparture + "-" + models.TemFlightTransit;
                                            DepartFromTo2 = models.TemFlightTransit + "-" + models.TemFlightArrival;
                                            DepartFromToShort = models.TemFlightDeparture + "-" + models.TemFlightTransit;
                                            DepartFromToShort2 = models.TemFlightTransit + "-" + models.TemFlightArrival;
                                        }
                                        else
                                        {
                                            DepartFromTo = models.TemFlightDeparture + "-" + models.TemFlightArrival;
                                            DepartFromTo2 = models.TemFlightDeparture + "-" + models.TemFlightArrival;
                                            DepartFromToShort = models.TemFlightDeparture + "-" + models.TemFlightArrival;
                                            DepartFromToShort2 = models.TemFlightDeparture + "-" + models.TemFlightArrival;
                                        }
                                    }
                                }
                                else
                                {
                                    lblErr.Text = msgList.Err999998;
                                    pnlErr.Visible = true;
                                    return;
                                }
                                //End If



                            }
                            else
                            {
                            }


                            if (!string.IsNullOrEmpty(DepartXml))
                            {
                                ctlDepart.Style["display"] = "";


                                PassengerHeader = "<div id='passengerListHeader'" + btnSelected + " class='redSectionHeader'>";
                                PassengerHeader += "<div>Seat summary</div></div>";
                                PassengerHeader += "<div id='passengerListBody'" + btnSelected + " class='sectionBody'><br/>";
                                if (selectedBtn == 0)
                                {
                                    using (profiler.Step("GBS:CreateSeatControl"))
                                    {
                                        PassengerSum = CreateSeatControl(DepartPax, DepartFromToShort, AssignSeatInfo(EnumFlightType.DepartFlight, RecordLocator, (SeatAvailabilityResponse)Session["pAvailableSeatInfo0"]), EnumFlightType.DepartFlight);
                                    }
                                }
                                else if (selectedBtn == 1)
                                {
                                    using (profiler.Step("GBS:CreateSeatControl"))
                                    {
                                        PassengerSum = CreateSeatControl(DepartPax, DepartFromToShort2, AssignSeatInfo(EnumFlightType.DepartConnectingFlight, RecordLocator, (SeatAvailabilityResponse)Session["pAvailableSeatInfo1"]), EnumFlightType.DepartConnectingFlight);
                                    }
                                }
                                else if (selectedBtn == 2)
                                {
                                    using (profiler.Step("GBS:CreateSeatControl"))
                                    {
                                        PassengerSum = CreateSeatControl(ReturnPax, ReturnFromToShort, AssignSeatInfo(EnumFlightType.ReturnFlight, RecordLocator, (SeatAvailabilityResponse)Session["pAvailableSeatInfo2"]), EnumFlightType.ReturnFlight);
                                    }
                                }
                                else if (selectedBtn == 3)
                                {
                                    using (profiler.Step("GBS:CreateSeatControl"))
                                    {
                                        PassengerSum = CreateSeatControl(ReturnPax, ReturnFromToShort2, AssignSeatInfo(EnumFlightType.ReturnConnectingFlight, RecordLocator, (SeatAvailabilityResponse)Session["pAvailableSeatInfo3"]), EnumFlightType.ReturnConnectingFlight);
                                    }
                                }

                                if (!string.IsNullOrEmpty(PassengerSum))
                                {
                                    PassengerSummary.Style["display"] = "";
                                    PassengerSummary.InnerHtml = PassengerHeader + PassengerSum + "</div>";
                                }

                                if (selectedBtn == 0)
                                {
                                    ss.SeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"];

                                    Session["SeatInfo0Xml"] = DepartXml;
                                }
                                else if (selectedBtn == 1)
                                {
                                    ss.SeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"];

                                    Session["SeatInfo1Xml"] = DepartXml;
                                }
                                else if (selectedBtn == 2)
                                {
                                    ss.SeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"];

                                    Session["SeatInfo2Xml"] = ReturnXml;
                                }
                                else if (selectedBtn == 3)
                                {
                                    ss.SeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo3"];

                                    Session["SeatInfo3Xml"] = ReturnXml;
                                }
                                if ((ss.SeatInfo != null))
                                {
                                    foreach (ABS.Logic.GroupBooking.SeatInfo RowSeatInfo in ss.SeatInfo)
                                    {
                                        //RowSeatInfo = RowSeatInfo_loopVariable;
                                        if (string.IsNullOrEmpty(DepartDefaultSeat))
                                        {
                                            //DepartDefaultSeat = "0_" & RowSeatInfo.CompartmentDesignator & "_1_" & RowSeatInfo.SelectedSeat
                                            DepartDefaultSeat = "0_" + RowSeatInfo.CompartmentDesignator + "_" + RowSeatInfo.Deck + "_" + RowSeatInfo.SelectedSeat;
                                            //added by ketee
                                            //hotseat = RowSeatInfo.IsHotSeat;
                                        }
                                        else
                                        {
                                            //DepartDefaultSeat &= ",0_" & RowSeatInfo.CompartmentDesignator & "_1_" & RowSeatInfo.SelectedSeat
                                            DepartDefaultSeat += ",0_" + RowSeatInfo.CompartmentDesignator + "_" + RowSeatInfo.Deck + "_" + RowSeatInfo.SelectedSeat;
                                            //added by ketee
                                            //hotseat += "," + RowSeatInfo.IsHotSeat;
                                        }
                                    }
                                }
                                ss.defaultseat = DepartDefaultSeat;
                                //'remark by ketee, no default seat assign
                                ss.hotseat = hotseat;
                                ss.numberofpassenger = DepartPax;
                                ss.overwritepassengerindex = DepartPax;
                                ss.xmlurl = DepartXml;


                            }

                        }
                        else if (selectedBtn > 1)
                        {
                            if ((models != null))
                            {
                                //pReturnFlightInfo = Session["ReturnFlightInfo"];
                                if (selectedBtn == 2)
                                {
                                    if (Session["SeatInfo2Xml"] == null == false)
                                    {
                                        DeleteXML((string)Session["SeatInfo2Xml"]);

                                    }
                                    if (models.TemFlightTransit.Trim() != "")
                                        using (profiler.Step("API:GetSeatAvailability"))
                                        {
                                            pAvailableSeatInfo = absNavitaire.GetSeatAvailability(RecordLocator, 0, Signature, models.TemFlightCarrierCode, models.TemFlightFlightNumber, models.TemFlightOpSuffix,
                                                                                            models.TemFlightDeparture, models.TemFlightTransit, (DateTime)models.TemFlightStd, Request.PhysicalApplicationPath);
                                        }
                                    else
                                        using (profiler.Step("API:GetSeatAvailability"))
                                        {
                                            pAvailableSeatInfo = absNavitaire.GetSeatAvailability(RecordLocator, 0, Signature, models.TemFlightCarrierCode, models.TemFlightFlightNumber, models.TemFlightOpSuffix,
                                                                                            models.TemFlightDeparture, models.TemFlightArrival, (DateTime)models.TemFlightStd, Request.PhysicalApplicationPath);
                                        }
                                            //Session["Click"] = Nothing
                                    Session["pAvailableSeatInfo2"] = pAvailableSeatInfo;
                                    //btnSeatReturn1.Style[HtmlTextWriterStyle.BackgroundImage] = "images/CBackground2.png";
                                }
                                else if (selectedBtn == 3)
                                {
                                    if (Session["SeatInfo3Xml"] == null == false)
                                    {
                                        DeleteXML((string)Session["SeatInfo3Xml"]);

                                    }
                                    if (models.TemFlightTransit.Trim() != "")
                                        using (profiler.Step("API:GetSeatAvailability"))
                                        {
                                            pAvailableSeatInfo = absNavitaire.GetSeatAvailability(RecordLocator, 0, Signature, models.TemFlightCarrierCode2, models.TemFlightFlightNumber2, models.TemFlightOpSuffix2,
                                                                                            models.TemFlightTransit, models.TemFlightArrival, (DateTime)models.TemFlightStd2, Request.PhysicalApplicationPath);
                                        }
                                    else
                                        using (profiler.Step("API:GetSeatAvailability"))
                                        {
                                            pAvailableSeatInfo = absNavitaire.GetSeatAvailability(RecordLocator, 0, Signature, models.TemFlightCarrierCode2, models.TemFlightFlightNumber2, models.TemFlightOpSuffix2,
                                                                                            models.TemFlightDeparture, models.TemFlightArrival, (DateTime)models.TemFlightStd2, Request.PhysicalApplicationPath);
                                        }
                                            //Session["Click"] = Nothing
                                    Session["pAvailableSeatInfo3"] = pAvailableSeatInfo;
                                    //btnSeatReturn2.Style[HtmlTextWriterStyle.BackgroundImage] = "images/CBackground2.png";
                                }

                                if ((pAvailableSeatInfo != null))
                                {
                                    if (!string.IsNullOrEmpty(absNavitaire.DepartXmlUrl))
                                    {
                                        ReturnXml = absNavitaire.DepartXmlUrl;
                                        //pReturnFlightInfo.XmlURL = ReturnXml;
                                        //DepartXml = "test4.xml"
                                        ReturnPax = models.TemFlightPaxNum;
                                        if (selectedBtn == 0)
                                        {
                                            DepartPax = models.TemFlightPaxNum;
                                            if (models.TemFlightTransit.Trim() != "")
                                            {
                                                DepartFromTo = models.TemFlightDeparture + "-" + models.TemFlightTransit;
                                                DepartFromToShort = models.TemFlightDeparture + "-" + models.TemFlightTransit;
                                            }
                                            else
                                            {
                                                DepartFromTo = models.TemFlightDeparture + "-" + models.TemFlightArrival;
                                                DepartFromToShort = models.TemFlightDeparture + "-" + models.TemFlightArrival;
                                            }
                                        }
                                        else if (selectedBtn == 1)
                                        {
                                            DepartPax = models.TemFlightPaxNum;
                                            if (models.TemFlightTransit.Trim() != "")
                                            {
                                                DepartFromTo2 = models.TemFlightTransit + "-" + models.TemFlightArrival;
                                                DepartFromToShort2 = models.TemFlightTransit + "-" + models.TemFlightArrival;
                                            }
                                            else
                                            {
                                                DepartFromTo2 = models.TemFlightDeparture + "-" + models.TemFlightArrival;
                                                DepartFromToShort2 = models.TemFlightDeparture + "-" + models.TemFlightArrival;
                                            }
                                        }
                                        else if (selectedBtn == 2)
                                        {
                                            DepartPax = models.TemFlightPaxNum;
                                            if (models.TemFlightTransit.Trim() != "")
                                            {
                                                ReturnFromTo = models.TemFlightDeparture + "-" + models.TemFlightTransit;
                                                ReturnFromToShort = models.TemFlightDeparture + "-" + models.TemFlightTransit;
                                            }
                                            else
                                            {
                                                ReturnFromTo = models.TemFlightDeparture + "-" + models.TemFlightArrival;
                                                ReturnFromToShort = models.TemFlightDeparture + "-" + models.TemFlightArrival;
                                            }
                                        }
                                        else
                                        {
                                            DepartPax = models.TemFlightPaxNum;
                                            if (models.TemFlightTransit.Trim() != "")
                                            {
                                                ReturnFromTo2 = models.TemFlightTransit + "-" + models.TemFlightArrival;
                                                ReturnFromToShort2 = models.TemFlightTransit + "-" + models.TemFlightArrival;
                                            }
                                            else
                                            {
                                                ReturnFromTo2 = models.TemFlightDeparture + "-" + models.TemFlightArrival;
                                                ReturnFromToShort2 = models.TemFlightDeparture + "-" + models.TemFlightArrival;
                                            }
                                        }

                                        //ReturnFromTo = models.TemFlightDeparture + "-" + models.TemFlightTransit;
                                        //ReturnFromTo2 = models.TemFlightTransit + "-" + models.TemFlightArrival;
                                        //ReturnFromToShort = models.TemFlightDeparture + "-" + models.TemFlightTransit;
                                        //ReturnFromToShort2 = models.TemFlightTransit + "-" + models.TemFlightArrival;
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                                else
                                {
                                    lblErr.Text = msgList.Err999998;
                                    pnlErr.Visible = true;
                                    return;
                                }
                            }

                            if (!string.IsNullOrEmpty(ReturnXml))
                            {
                                ctlDepart.Style["display"] = "";


                                PassengerHeader = "<div id=\"passengerListHeader" + btnSelected + " class=\"redSectionHeader\">";
                                PassengerHeader += "<div>Seat summary</div></div>";
                                PassengerHeader += "<div id=\"passengerListBody" + btnSelected + "class=\"sectionBody\"><br/>";
                                if (selectedBtn == 2)
                                {
                                    using (profiler.Step("GBS:CreateSeatControl"))
                                    {
                                        PassengerSum = CreateSeatControl(ReturnPax, ReturnFromToShort, AssignSeatInfo(EnumFlightType.ReturnFlight, RecordLocator, (SeatAvailabilityResponse)Session["pAvailableSeatInfo2"]), EnumFlightType.ReturnFlight);
                                    }
                                }
                                else if (selectedBtn == 3)
                                {
                                    using (profiler.Step("GBS:CreateSeatControl"))
                                    {
                                        PassengerSum = CreateSeatControl(ReturnPax, ReturnFromToShort2, AssignSeatInfo(EnumFlightType.ReturnConnectingFlight, RecordLocator, (SeatAvailabilityResponse)Session["pAvailableSeatInfo3"]), EnumFlightType.ReturnConnectingFlight);
                                    }
                                }

                                if (!string.IsNullOrEmpty(PassengerSum))
                                {
                                    PassengerSummary.Style["display"] = "";
                                    PassengerSummary.InnerHtml = PassengerHeader + PassengerSum + "</div>";
                                }

                                if (selectedBtn == 2)
                                {
                                    ss.SeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"];

                                    Session["SeatInfo2Xml"] = ReturnXml;
                                }
                                else if (selectedBtn == 3)
                                {
                                    ss.SeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo3"];

                                    Session["SeatInfo3Xml"] = ReturnXml;
                                }
                                if ((ss.SeatInfo != null))
                                {
                                    foreach (ABS.Logic.GroupBooking.SeatInfo RowSeatInfo in ss.SeatInfo)
                                    {
                                        //RowSeatInfo = RowSeatInfo_loopVariable;
                                        if (string.IsNullOrEmpty(ReturnDefaultSeat))
                                        {
                                            ReturnDefaultSeat = "0_" + RowSeatInfo.CompartmentDesignator + "_" + RowSeatInfo.Deck + "_" + RowSeatInfo.SelectedSeat;
                                        }
                                        else
                                        {
                                            ReturnDefaultSeat += ",0_" + RowSeatInfo.CompartmentDesignator + "_" + RowSeatInfo.Deck + "_" + RowSeatInfo.SelectedSeat;
                                        }
                                    }
                                }
                                ss.defaultseat = ReturnDefaultSeat;
                                //'remark by ketee, no default seat assign
                                ss.hotseat = hotseat;
                                ss.numberofpassenger = ReturnPax;
                                ss.overwritepassengerindex = ReturnPax;
                                ss.xmlurl = ReturnXml;

                            }
                        }
                        break;
                }



                if (Session["ErrorMsg"] == null == false)
                {
                    lblErr.Text = Session["ErrorMsg"].ToString();
                    pnlErr.Visible = true;
                    Session["ErrorMsg"] = null;
                }
                else
                {
                    pnlErr.Visible = false;
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
                Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.InvalidPage, false);
            }
        }

        private List<ABS.Logic.GroupBooking.SeatInfo> AssignSeatInfo(EnumFlightType flightType, string recordLocator, SeatAvailabilityResponse pAvailableSeatInfo)
        {
            var profiler = MiniProfiler.Current;
            List<ABS.Logic.GroupBooking.SeatInfo> seatInfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
            ABS.Logic.GroupBooking.SeatInfo seatInfo1 = new ABS.Logic.GroupBooking.SeatInfo();
            DataTable dtPassenger = default(DataTable);
            DataTable dtInfant = default(DataTable);
            ////AirAsia.Logic.FI_Booking_Logic FI_Booking_Logic = new AirAsia.Logic.FI_Booking_Logic();
            //dtPassenger = FI_Booking_Logic.getAllBookingPassenger(recordLocator);
            using (profiler.Step("GBS:GetAllBK_PASSENGERLISTInitDataTable"))
            {
                dtPassenger = objBooking.GetAllBK_PASSENGERLISTInitDataTable(TransId, true);
            }
            using (profiler.Step("GBS:GetAllBK_PASSENGERLISTWithSSRDataTableInfant"))
            {
                dtInfant = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTableInfant(TransId);
            }
            if (dtPassenger == null)
            {
                if (Session["Chgsave"] != null)
                {
                    ArrayList save = (ArrayList)Session["Chgsave"];
                    DataTable dtTransDetail = objBooking.dtTransDetail();
                    if (HttpContext.Current.Session["TransDetailAll"] != null)
                        dtTransDetail = (DataTable)HttpContext.Current.Session["TransDetailAll"];
                    DataTable dtPassClone = new DataTable();
                    dtPassClone = objBooking.GetAllBK_PASSENGERLISTInitDataTable(save[1].ToString(), true);
                    dtInfant = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTableInfant(save[1].ToString());
                    dtPassenger = dtPassClone.Clone();
                    int x = 0;
                    for (int ii = 0; ii < dtPassClone.Rows.Count; ii++)
                    {
                        if (dtPassClone.Rows[ii]["PNR"].ToString() == dtTransDetail.Rows[0]["RecordLocator"].ToString())
                        {

                            dtPassenger.ImportRow(dtPassClone.Rows[ii]);
                            dtPassenger.Rows[x]["TransID"] = save[0].ToString();
                            x += 1;
                        }
                    }
                }
            }

            if (dtInfant != null && dtInfant.Rows.Count > 0)
            {
                Session["dtInfant"] = dtInfant;
            }

            string CompartmentDesignator = "", Deck = "", SeatSet = "";
            int IsHotSeat = 0;

            int i = 0;

            EnumFlight eFlights = eFlight;
            if (eFlight2 == EnumFlight.ConnectingFlight)
            {
                eFlights = eFlight2;
            }
            switch (eFlights)
            {
                case EnumFlight.ConnectingFlight:
                    switch (flightType)
                    {
                        case EnumFlightType.DepartFlight:
                            if ((dtPassenger != null))
                            {
                                foreach (DataRow drRow in dtPassenger.Rows)
                                {
                                    //drRow = drRow_loopVariable;
                                    seatInfo1 = new ABS.Logic.GroupBooking.SeatInfo();
                                    seatInfo1.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                    switch (drRow["Title"].ToString().ToUpper())
                                    {
                                        case "INFT":
                                            seatInfo1.PaxType = "INFT";
                                            break;
                                        case "CHD":
                                            seatInfo1.PaxType = "CHD";
                                            break;
                                        default:
                                            seatInfo1.PaxType = "ADT";
                                            break;
                                    }
                                    seatInfo1.PassengerID = drRow["PassengerID"].ToString();
                                    seatInfo1.PassengerNumber = Convert.ToInt32(drRow["PassengerID"].ToString());
                                    seatInfo1.Seq = i;
                                    seatInfo1.RecordLocator = drRow["PNR"].ToString();

                                    CompartmentDesignator = "";
                                    Deck = "";
                                    SeatSet = "";
                                    IsHotSeat = 0;
                                    using (profiler.Step("GBS:getPassengerDefaultSeat"))
                                    {
                                        seatInfo1.SelectedSeat = getPassengerDefaultSeat(drRow["PNR"].ToString(), seatInfo1.PassengerID, EnumFlightType.DepartFlight, (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"], ref CompartmentDesignator, ref Deck, ref SeatSet, ref IsHotSeat, i);
                                    }
                                    seatInfo1.CompartmentDesignator = CompartmentDesignator;
                                    seatInfo1.Deck = Deck;
                                    seatInfo1.SeatSet = SeatSet;
                                    seatInfo1.IsHotSeat = IsHotSeat;
                                    //assign default seat
                                    //With pAvailableSeatInfo
                                    //    For Each pSeatInfo As AABookingManager.SeatInfo In .EquipmentInfos(0).Compartments(0).Seats
                                    //        With pSeatInfo
                                    //            If .Assignable = True And .SeatAvailability.ToString.ToUpper = "OPEN" Then
                                    //                Dim SeatDesignator As String = .SeatDesignator
                                    //                seatInfo1.CompartmentDesignator = pAvailableSeatInfo.EquipmentInfos(0).Compartments(0).CompartmentDesignator
                                    //                seatInfo1.SelectedSeat = SeatDesignator

                                    //                Exit For
                                    //            End If
                                    //        End With
                                    //    Next
                                    //End With


                                    seatInfo.Add(seatInfo1);
                                    i = i + 1;
                                }
                            }
                            else
                            {
                                return null;
                            }

                            ss.SeatInfo = seatInfo;
                            Session["SeatInfo0"] = seatInfo;
                            break;
                        case EnumFlightType.DepartConnectingFlight:
                            if ((dtPassenger != null))
                            {
                                foreach (DataRow drRow in dtPassenger.Rows)
                                {
                                    //drRow = drRow_loopVariable;
                                    seatInfo1 = new ABS.Logic.GroupBooking.SeatInfo();
                                    seatInfo1.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                    switch (drRow["Title"].ToString().ToUpper())
                                    {
                                        case "INFT":
                                            seatInfo1.PaxType = "INFT";
                                            break;
                                        case "CHD":
                                            seatInfo1.PaxType = "CHD";
                                            break;
                                        default:
                                            seatInfo1.PaxType = "ADT";
                                            break;
                                    }
                                    seatInfo1.PassengerID = drRow["PassengerID"].ToString();
                                    seatInfo1.PassengerNumber = Convert.ToInt32(drRow["PassengerID"].ToString());
                                    seatInfo1.Seq = i;
                                    seatInfo1.RecordLocator = drRow["PNR"].ToString();

                                    CompartmentDesignator = "";
                                    Deck = "";
                                    SeatSet = "";
                                    IsHotSeat = 0;
                                    using (profiler.Step("GBS:getPassengerDefaultSeat"))
                                    {
                                        seatInfo1.SelectedSeat = getPassengerDefaultSeat(drRow["PNR"].ToString(), seatInfo1.PassengerID, EnumFlightType.DepartConnectingFlight, (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"], ref CompartmentDesignator, ref Deck, ref SeatSet, ref IsHotSeat, i);
                                    }
                                    seatInfo1.CompartmentDesignator = CompartmentDesignator;
                                    seatInfo1.Deck = Deck;
                                    seatInfo1.SeatSet = SeatSet;
                                    seatInfo1.IsHotSeat = IsHotSeat;

                                    //assign default seat
                                    //With pAvailableSeatInfo
                                    //    For Each pSeatInfo As AABookingManager.SeatInfo In .EquipmentInfos(0).Compartments(0).Seats
                                    //        With pSeatInfo
                                    //            If .Assignable = True And .SeatAvailability.ToString.ToUpper = "OPEN" Then
                                    //                Dim SeatDesignator As String = .SeatDesignator
                                    //                seatInfo1.CompartmentDesignator = pAvailableSeatInfo.EquipmentInfos(0).Compartments(0).CompartmentDesignator
                                    //                seatInfo1.SelectedSeat = SeatDesignator

                                    //                Exit For
                                    //            End If
                                    //        End With
                                    //    Next
                                    //End With

                                    seatInfo.Add(seatInfo1);
                                    i = i + 1;
                                }
                            }
                            else
                            {
                                return null;
                            }

                            ss.SeatInfo = seatInfo;
                            Session["SeatInfo1"] = seatInfo;
                            break;
                        case EnumFlightType.ReturnFlight:
                            if ((dtPassenger != null))
                            {
                                foreach (DataRow drRow in dtPassenger.Rows)
                                {
                                    //drRow = drRow_loopVariable;
                                    seatInfo1 = new ABS.Logic.GroupBooking.SeatInfo();
                                    seatInfo1.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                    switch (drRow["Title"].ToString().ToUpper())
                                    {
                                        case "INFT":
                                            seatInfo1.PaxType = "INFT";
                                            break;
                                        case "CHD":
                                            seatInfo1.PaxType = "CHD";
                                            break;
                                        default:
                                            seatInfo1.PaxType = "ADT";
                                            break;
                                    }
                                    seatInfo1.PassengerID = drRow["PassengerID"].ToString();
                                    seatInfo1.PassengerNumber = Convert.ToInt32(drRow["PassengerID"].ToString());
                                    seatInfo1.Seq = i;
                                    seatInfo1.RecordLocator = drRow["PNR"].ToString();

                                    CompartmentDesignator = "";
                                    Deck = "";
                                    SeatSet = "";
                                    IsHotSeat = 0;
                                    using (profiler.Step("GBS:getPassengerDefaultSeat"))
                                    {
                                        seatInfo1.SelectedSeat = getPassengerDefaultSeat(drRow["PNR"].ToString(), seatInfo1.PassengerID, EnumFlightType.ReturnFlight, (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"], ref CompartmentDesignator, ref Deck, ref SeatSet, ref IsHotSeat, i);
                                    }
                                    seatInfo1.CompartmentDesignator = CompartmentDesignator;
                                    seatInfo1.Deck = Deck;
                                    seatInfo1.SeatSet = SeatSet;
                                    seatInfo1.IsHotSeat = IsHotSeat;

                                    ////seatInfo1.SelectedSeat = getPassengerDefaultSeat(recordLocator, _QueueCode, seatInfo1.PassengerID, Common.EnumFlightType.ReturnFlight, Session["SeatInfo2"], seatInfo1.CompartmentDesignator, seatInfo1.Deck, seatInfo1.SeatSet, seatInfo1.IsHotSeat, i);
                                    //assign default seat
                                    //With pAvailableSeatInfo
                                    //    For Each pSeatInfo As AABookingManager.SeatInfo In .EquipmentInfos(0).Compartments(0).Seats
                                    //        With pSeatInfo
                                    //            If .Assignable = True And .SeatAvailability.ToString.ToUpper = "OPEN" Then
                                    //                Dim SeatDesignator As String = .SeatDesignator
                                    //                seatInfo1.CompartmentDesignator = pAvailableSeatInfo.EquipmentInfos(0).Compartments(0).CompartmentDesignator
                                    //                seatInfo1.SelectedSeat = SeatDesignator

                                    //                Exit For
                                    //            End If
                                    //        End With
                                    //    Next
                                    //End With

                                    seatInfo.Add(seatInfo1);
                                    i = i + 1;
                                }

                            }
                            else
                            {
                                return null;
                            }

                            ss.SeatInfo = seatInfo;
                            Session["SeatInfo2"] = seatInfo;
                            break;
                        case EnumFlightType.ReturnConnectingFlight:
                            if ((dtPassenger != null))
                            {
                                foreach (DataRow drRow in dtPassenger.Rows)
                                {
                                    //drRow = drRow_loopVariable;
                                    seatInfo1 = new ABS.Logic.GroupBooking.SeatInfo();
                                    seatInfo1.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                    switch (drRow["Title"].ToString().ToUpper())
                                    {
                                        case "INFT":
                                            seatInfo1.PaxType = "INFT";
                                            break;
                                        case "CHD":
                                            seatInfo1.PaxType = "CHD";
                                            break;
                                        default:
                                            seatInfo1.PaxType = "ADT";
                                            break;
                                    }
                                    seatInfo1.PassengerID = drRow["PassengerID"].ToString();
                                    seatInfo1.PassengerNumber = Convert.ToInt32(drRow["PassengerID"].ToString());
                                    seatInfo1.Seq = i;
                                    seatInfo1.RecordLocator = drRow["PNR"].ToString();

                                    CompartmentDesignator = "";
                                    Deck = "";
                                    SeatSet = "";
                                    IsHotSeat = 0;
                                    using (profiler.Step("GBS:getPassengerDefaultSeat"))
                                    {
                                        seatInfo1.SelectedSeat = getPassengerDefaultSeat(drRow["PNR"].ToString(), seatInfo1.PassengerID, EnumFlightType.ReturnConnectingFlight, (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo3"], ref CompartmentDesignator, ref Deck, ref SeatSet, ref IsHotSeat, i);
                                    }
                                    seatInfo1.CompartmentDesignator = CompartmentDesignator;
                                    seatInfo1.Deck = Deck;
                                    seatInfo1.SeatSet = SeatSet;
                                    seatInfo1.IsHotSeat = IsHotSeat;

                                    ////seatInfo1.SelectedSeat = getPassengerDefaultSeat(recordLocator, _QueueCode, seatInfo1.PassengerID, Common.EnumFlightType.ReturnConnectingFlight, Session["SeatInfo3"], seatInfo1.CompartmentDesignator, seatInfo1.Deck, seatInfo1.SeatSet, seatInfo1.IsHotSeat, i);
                                    //assign default seat
                                    //With pAvailableSeatInfo
                                    //    For Each pSeatInfo As AABookingManager.SeatInfo In .EquipmentInfos(0).Compartments(0).Seats
                                    //        With pSeatInfo
                                    //            If .Assignable = True And .SeatAvailability.ToString.ToUpper = "OPEN" Then
                                    //                Dim SeatDesignator As String = .SeatDesignator
                                    //                seatInfo1.CompartmentDesignator = pAvailableSeatInfo.EquipmentInfos(0).Compartments(0).CompartmentDesignator
                                    //                seatInfo1.SelectedSeat = SeatDesignator

                                    //                Exit For
                                    //            End If
                                    //        End With
                                    //    Next
                                    //End With

                                    seatInfo.Add(seatInfo1);
                                    i = i + 1;
                                }
                            }
                            else
                            {
                                return null;
                            }

                            ss.SeatInfo = seatInfo;
                            Session["SeatInfo3"] = seatInfo;
                            break;
                    }
                    break;
                case EnumFlight.DirectFlight:
                    switch (flightType)
                    {
                        case EnumFlightType.DepartFlight:
                            if ((dtPassenger != null))
                            {
                                foreach (DataRow drRow in dtPassenger.Rows)
                                {
                                    //drRow = drRow_loopVariable;
                                    seatInfo1 = new ABS.Logic.GroupBooking.SeatInfo();
                                    seatInfo1.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                    switch (drRow["Title"].ToString().ToUpper())
                                    {
                                        case "INFT":
                                            seatInfo1.PaxType = "INFT";
                                            break;
                                        case "CHD":
                                            seatInfo1.PaxType = "CHD";
                                            break;
                                        default:
                                            seatInfo1.PaxType = "ADT";
                                            break;
                                    }
                                    seatInfo1.PassengerID = drRow["PassengerID"].ToString();
                                    seatInfo1.PassengerNumber = Convert.ToInt32(drRow["PassengerID"].ToString());
                                    seatInfo1.Seq = i;
                                    seatInfo1.RecordLocator = drRow["PNR"].ToString();

                                    CompartmentDesignator = "";
                                    Deck = "";
                                    SeatSet = "";
                                    IsHotSeat = 0;
                                    using (profiler.Step("GBS:getPassengerDefaultSeat"))
                                    {
                                        seatInfo1.SelectedSeat = getPassengerDefaultSeat(drRow["PNR"].ToString(), seatInfo1.PassengerID, EnumFlightType.DepartFlight, (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"], ref CompartmentDesignator, ref Deck, ref SeatSet, ref IsHotSeat, i);
                                    }
                                    seatInfo1.CompartmentDesignator = CompartmentDesignator;
                                    seatInfo1.Deck = Deck;
                                    seatInfo1.SeatSet = SeatSet;
                                    seatInfo1.IsHotSeat = IsHotSeat;

                                    ////seatInfo1.SelectedSeat = getPassengerDefaultSeat(recordLocator, seatInfo1.PassengerID, EnumFlightType.DepartFlight, Session["SeatInfo0"], seatInfo1.CompartmentDesignator, seatInfo1.Deck, seatInfo1.SeatSet, seatInfo1.IsHotSeat, i);

                                    seatInfo.Add(seatInfo1);
                                    i = i + 1;
                                }
                            }
                            else
                            {
                                return null;
                            }

                            ss.SeatInfo = seatInfo;
                            Session["SeatInfo0"] = seatInfo;
                            break;

                        case EnumFlightType.ReturnFlight:
                            if ((dtPassenger != null))
                            {
                                foreach (DataRow drRow in dtPassenger.Rows)
                                {
                                    //drRow = drRow_loopVariable;
                                    seatInfo1 = new ABS.Logic.GroupBooking.SeatInfo();
                                    seatInfo1.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                    switch (drRow["Title"].ToString().ToUpper())
                                    {
                                        case "INFT":
                                            seatInfo1.PaxType = "INFT";
                                            break;
                                        case "CHD":
                                            seatInfo1.PaxType = "CHD";
                                            break;
                                        default:
                                            seatInfo1.PaxType = "ADT";
                                            break;
                                    }
                                    seatInfo1.PassengerID = drRow["PassengerID"].ToString();
                                    seatInfo1.PassengerNumber = Convert.ToInt32(drRow["PassengerID"].ToString());
                                    seatInfo1.Seq = i;
                                    seatInfo1.RecordLocator = drRow["PNR"].ToString();

                                    CompartmentDesignator = "";
                                    Deck = "";
                                    SeatSet = "";
                                    IsHotSeat = 0;
                                    using (profiler.Step("GBS:getPassengerDefaultSeat"))
                                    {
                                        seatInfo1.SelectedSeat = getPassengerDefaultSeat(drRow["PNR"].ToString(), seatInfo1.PassengerID, EnumFlightType.ReturnFlight, (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"], ref CompartmentDesignator, ref Deck, ref SeatSet, ref IsHotSeat, i);
                                    }
                                    seatInfo1.CompartmentDesignator = CompartmentDesignator;
                                    seatInfo1.Deck = Deck;
                                    seatInfo1.SeatSet = SeatSet;
                                    seatInfo1.IsHotSeat = IsHotSeat;

                                    ////seatInfo1.SelectedSeat = getPassengerDefaultSeat(recordLocator, _QueueCode, seatInfo1.PassengerID, Common.EnumFlightType.ReturnFlight, Session["SeatInfo1"], seatInfo1.CompartmentDesignator, seatInfo1.Deck, seatInfo1.SeatSet, seatInfo1.IsHotSeat, i);

                                    seatInfo.Add(seatInfo1);
                                    i = i + 1;
                                }

                            }
                            else
                            {
                                return null;
                            }

                            ss.SeatInfo = seatInfo;
                            Session["SeatInfo2"] = seatInfo;
                            break;

                    }
                    break;
            }



            return seatInfo;
        }

        private string getPassengerDefaultSeat(string RecordLocator, string PassengerID, EnumFlightType FlightType, List<ABS.Logic.GroupBooking.SeatInfo> SessionSeatInfo,
            ref string CompartmentDesignator, ref string Deck, ref string SeatSet, ref int HotSeat, int PsgNumber)
        {
            try
            {
                ////AABookingManager.Booking BookingInfo = new AABookingManager.Booking();
                int PassengerNumber = 0;
                DataTable dtPassengerSeat = new DataTable();
                ////dtFlightDetails = FI_Booking_Logic.getAllBookingDetails_byRecordLocator(RecordLocator, QueueCode);

                if (SessionSeatInfo == null == false)
                {
                    foreach (ABS.Logic.GroupBooking.SeatInfo drSeatInfo in SessionSeatInfo)
                    {
                        //drSeatInfo = drSeatInfo_loopVariable;
                        if (drSeatInfo.PassengerID == PassengerID && RecordLocator.Trim() == drSeatInfo.RecordLocator.Trim())
                        {
                            PassengerNumber = drSeatInfo.PassengerNumber;
                            Deck = drSeatInfo.Deck;
                            CompartmentDesignator = drSeatInfo.CompartmentDesignator;
                            HotSeat = drSeatInfo.IsHotSeat;
                            ////HotSeatMap = drSeatInfo.HotSeatMap;
                            return drSeatInfo.SelectedSeat;
                            // TODO: might not be correct. Was : Exit For
                        }
                    }

                }

                return "";

                ////////if ((dtFlightDetails != null) & dtFlightDetails.Rows.Count > 0)
                ////////{
                ////////    switch (FlightType)
                ////////    {
                ////////        case Common.EnumFlightType.DepartFlight:
                ////////            dtPassengerSeat = FI_Booking_Logic.getAllBookingDetails_byJourneySellKey(RecordLocator, dtFlightDetails.Rows(0)("JourneySellKey"));
                ////////            break;
                ////////        case Common.EnumFlightType.DepartConnectingFlight:
                ////////            dtPassengerSeat = FI_Booking_Logic.getAllBookingDetails_byJourneySellKey(RecordLocator, dtFlightDetails.Rows(2)("JourneySellKey"));
                ////////            break;
                ////////        case Common.EnumFlightType.ReturnFlight:
                ////////            dtPassengerSeat = FI_Booking_Logic.getAllBookingDetails_byJourneySellKey(RecordLocator, dtFlightDetails.Rows(1)("JourneySellKey"));
                ////////            break;
                ////////        case Common.EnumFlightType.ReturnConnectingFlight:
                ////////            dtPassengerSeat = FI_Booking_Logic.getAllBookingDetails_byJourneySellKey(RecordLocator, dtFlightDetails.Rows(3)("JourneySellKey"));
                ////////            break;
                ////////    }

                ////////    if (SessionSeatInfo == null == false)
                ////////    {
                ////////        foreach (void drSeatInfo_loopVariable in SessionSeatInfo)
                ////////        {
                ////////            drSeatInfo = drSeatInfo_loopVariable;
                ////////            if (drSeatInfo.PassengerID == PassengerID)
                ////////            {
                ////////                PassengerNumber = drSeatInfo.PassengerNumber;
                ////////                Deck = drSeatInfo.Deck;
                ////////                CompartmentDesignator = drSeatInfo.CompartmentDesignator;
                ////////                HotSeat = drSeatInfo.IsHotSeat;
                ////////                HotSeatMap = drSeatInfo.HotSeatMap;
                ////////                return drSeatInfo.SelectedSeat;
                ////////                break; // TODO: might not be correct. Was : Exit For
                ////////            }
                ////////        }

                ////////    }
                ////////    else
                ////////    {
                ////////        if ((dtPassengerSeat != null) && dtPassengerSeat.Rows.Count > 0)
                ////////        {
                ////////            CompartmentDesignator = dtPassengerSeat.Rows(PsgNumber)("CompartmentDesignator");
                ////////            Deck = dtPassengerSeat.Rows(PsgNumber)("Deck");
                ////////            //HotSeat = IsHotSeat(dtPassengerSeat.Rows(PsgNumber)("CarrierCode"), "320", dtPassengerSeat.Rows(PsgNumber)("UnitDesignator"))
                ////////            HotSeat = IsHotSeat(dtPassengerSeat.Rows(PsgNumber)("CarrierCode"), dtPassengerSeat.Rows(PsgNumber)("EquipmentType"), dtPassengerSeat.Rows(PsgNumber)("UnitDesignator"));
                ////////            string str = FormHotSeatString(dtPassengerSeat.Rows(PsgNumber)("CarrierCode"), dtPassengerSeat.Rows(PsgNumber)("EquipmentType"));
                ////////            HotSeatMap = str;
                ////////            //Return dtPassengerSeat.Rows(PsgNumber)("UnitDesignator")
                ////////            return "";
                ////////        }
                ////////        else
                ////////        {
                ////////            return "";
                ////////        }
                ////////    }
                ////////}
                ////////else
                ////////{
                ////////    switch (FlightType)
                ////////    {
                ////////        case Common.EnumFlightType.DepartFlight:
                ////////            iType = 0;
                ////////            iJourney = 0;
                ////////            break;
                ////////        case Common.EnumFlightType.DepartConnectingFlight:
                ////////            iType = 0;
                ////////            iJourney = 1;
                ////////            break;
                ////////        case Common.EnumFlightType.ReturnFlight:
                ////////            iType = 1;
                ////////            iJourney = 0;
                ////////            break;
                ////////        case Common.EnumFlightType.ReturnConnectingFlight:
                ////////            iType = 1;
                ////////            iJourney = 1;
                ////////            break;
                ////////    }

                ////////    if (SessionSeatInfo == null == false)
                ////////    {
                ////////        foreach (void drSeatInfo_loopVariable in SessionSeatInfo)
                ////////        {
                ////////            drSeatInfo = drSeatInfo_loopVariable;
                ////////            if (drSeatInfo.PassengerID == PassengerID)
                ////////            {
                ////////                PassengerNumber = drSeatInfo.PassengerNumber;
                ////////                Deck = drSeatInfo.Deck;
                ////////                CompartmentDesignator = drSeatInfo.CompartmentDesignator;

                ////////                return drSeatInfo.SelectedSeat;
                ////////                break; // TODO: might not be correct. Was : Exit For
                ////////            }
                ////////        }


                ////////    }
                ////////    else
                ////////    {
                ////////        BookingInfo = API.GetBooking(_RecordLocator);
                ////////        if ((BookingInfo != null))
                ////////        {
                ////////            for (int i = 0; i <= BookingInfo.Passengers.Count - 1; i++)
                ////////            {
                ////////                if (BookingInfo.Passengers(i).PassengerID == PassengerID)
                ////////                {
                ////////                    PassengerNumber = BookingInfo.Passengers(i).PassengerNumber;
                ////////                    break; // TODO: might not be correct. Was : Exit For
                ////////                }
                ////////            }

                ////////            for (int i = 0; i <= BookingInfo.Journeys(iType).Segments(iJourney).PaxSeats.Count - 1; i++)
                ////////            {
                ////////                if (BookingInfo.Journeys(iType).Segments(iJourney).PaxSeats(i).PassengerNumber == PassengerNumber)
                ////////                {
                ////////                    CompartmentDesignator = BookingInfo.Journeys(iType).Segments(iJourney).PaxSeats(i).CompartmentDesignator;
                ////////                    //ketee
                ////////                    Deck = BookingInfo.Journeys(iType).Segments(iJourney).PaxSeats(i).PaxSeatInfo.Deck;
                ////////                    //_SeatSet = BookingInfo.Journeys(iType).Segments(0).PaxSeats(i).Penalty
                ////////                    return BookingInfo.Journeys(iType).Segments(iJourney).PaxSeats(i).UnitDesignator;
                ////////                    break; // TODO: might not be correct. Was : Exit For
                ////////                }
                ////////            }

                ////////        }
                ////////        else
                ////////        {
                ////////            return "";
                ////////        }
                ////////    }
                ////////}




            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                return "";
            }
        }

        private string CreateSeatControl(int Pax, string FromTo, List<ABS.Logic.GroupBooking.SeatInfo> _seatInfo, EnumFlightType Type)
        {
            try
            {
                string str = "";
                int i = 0;
                string seatbox = "";
                string selectedBox = "";

                DataTable dtInfant = new DataTable();

                seatbox = "BPassengerNumber_";
                selectedBox = "APassengerNumber_";

                str += "<table class=\"clearTableHeaders\"><tbody><tr class=\"market\">";
                str += "<th scope=\"col\">" + FromTo + "</th><th scope=\"col\"></th><th scope=\"col\"></th></tr>";

                int index = 0;
                if (_seatInfo != null)
                {
                    foreach (ABS.Logic.GroupBooking.SeatInfo seatInfo in _seatInfo)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            //if (!string.IsNullOrEmpty(seatInfo.SelectedSeat))
                            {
                                string PaxType = seatInfo.PaxType;
                                if (Session["dtInfant"] != null)
                                {
                                    dtInfant = (DataTable)Session["dtInfant"];
                                    DataRow drInfant = dtInfant.Select("PassengerID = '" + seatInfo.PassengerID + "' AND RecordLocator = '" + seatInfo.RecordLocator.Trim() + "'").FirstOrDefault();
                                    if (drInfant != null && drInfant.ItemArray.Length > 0)
                                    {
                                        if (PaxType.Trim() != "") PaxType = " (" + PaxType + " + INFT )";
                                    }
                                    else
                                    {
                                        if (PaxType.Trim() != "") PaxType = " (" + PaxType + ")";
                                    }
                                }
                                else
                                {
                                    if (PaxType.Trim() != "") PaxType = " (" + PaxType + ")";
                                }
                                if (Request.QueryString["change"] == null)
                                {
                                    if (i == 0 || (i != 0 && Convert.ToInt16(seatInfo.RecordLocator.Trim()) > Convert.ToInt16(_seatInfo[i - 1].RecordLocator.Trim())))
                                    {
                                        index += 1;
                                        str += "<tr><td class=\"pnr\" colspan=\"3\" style=\"background-color: #e3e3e3;font-size: small;font-weight: 600;padding: 3px;padding-left: 15px;color: #000;\">  PNR : (" + index + ")</td></tr>";
                                    }
                                }
                                else
                                {
                                    if (i == 0 || (i != 0 && (seatInfo.RecordLocator.Trim()) != (_seatInfo[i - 1].RecordLocator.Trim())))
                                    {
                                        index += 1;
                                        str += "<tr><td class=\"pnr\" colspan=\"3\" style=\"background-color: #e3e3e3;font-size: small;font-weight: 600;padding: 3px;padding-left: 15px;color: #000;\">  PNR : (" + seatInfo.RecordLocator + ")</td></tr>";
                                    }
                                }

                                str += "<tr><td class=\"passenger\">" + (i + 1) + ". " + seatInfo.PaxName + PaxType + "</td>";
                                str += "<td class=\"seatSelect\">";
                                str += "<input type=\"hidden\" name=\"PassengerNumber_" + seatInfo.Seq + "\" id=\"PassengerNumber_" + seatInfo.Seq + "\" value=\"" + seatInfo.PassengerID + "\" class=\"\" >";
                                if (i == 0)
                                {
                                    str += "<input name=\"" + seatbox + seatInfo.Seq + "\" type=\"text\" id=\"" + seatbox + seatInfo.Seq + "\" readonly=\"\" class=\"activeUnitInput\" ></td>";
                                }
                                else
                                {
                                    str += "<input name=\"" + seatbox + seatInfo.Seq + "\" type=\"text\" id=\"" + seatbox + seatInfo.Seq + "\" readonly=\"\" class=\"\" ></td>";
                                }
                                str += "<td><input name=\"" + seatbox + seatInfo.Seq + "_Fee\" type=\"hidden\" id=\"" + seatbox + seatInfo.Seq + "_Fee\" readonly=\"\" class=\"\" >";
                                str += "<td><input name=\"" + seatbox + seatInfo.Seq + "_HidFee\" type=\"hidden\" id=\"" + seatbox + seatInfo.Seq + "_HidFee\" readonly=\"\" class=\"\" ></td>";
                                str += "</tr>";
                                //remark by ketee, 20170117
                                ////str += "<tr><td style=\"vertical-align:top;\">";
                                ////str += "<input id=\"" + selectedBox + seatInfo.Seq + "_Reselect\" type=\"button\" class=\"button_1\" value=\"Reselect\">";
                                //////str &= "<input id=""APassengerNumber_" & seatInfo.Seq & "_Remove"" type=""button"" class=""button_1"" value=""Remove""></td>"
                                ////str += "</td><td style=\"vertical-align:top;\">" + (seatInfo.IsHotSeat == 1 ? "<img src=\"../images/JetAircraft_NS_Open_0_HS.gif\" class=\"unitGroupKey\">" : "<img src=\"../images/JetAircraft_NS_Open_0.gif\" class=\"unitGroupKey\">") + "</td></tr>";
                            }

                        }
                        i += 1;
                    }

                    str += "</tbody></table>";

                    return str;
                }
                else
                {
                    //Amended by Tyas 20170920 to fix Airbrake issue
                    Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.SessionExpired, false);
                    return "";
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                return "";
            }
        }

        //protected void LinkButtonAssignUnit_Click(object sender, EventArgs e)
        protected string ValidateSeat()
        {
            var profiler = MiniProfiler.Current;
            MessageList msgList = new MessageList();
            bool assignSeatDone = false;
            try
            {
                pnlErr.Visible = false;
                int SeatInfo0Checking = 0;
                int SeatInfo1Checking = 0;
                int SeatInfo2Checking = 0;
                int SeatInfo3Checking = 0;

                bool IsOneWay = false;

                EnumFlight eFlights = eFlight;
                if (eFlight2 == EnumFlight.ConnectingFlight)
                {
                    eFlights = eFlight2;
                }

                switch (eFlights)
                {
                    case EnumFlight.ConnectingFlight:
                        if (model == null == false)
                        {
                            Boolean IsInternationalFlight = objGeneral.IsInternationalFlight(model.TemFlightDeparture, model.TemFlightArrival, Request.PhysicalApplicationPath);
                            //Flight_Info pDepartFlightInfo = new Flight_Info();
                            //pDepartFlightInfo = Session["DepartFlightInfo"];

                            if (IsInternationalFlight != true)
                            {
                                SeatInfo0Checking = 0;
                            }
                            else
                            {
                                if (Session["SeatInfo0"] == null == false)
                                {
                                    if (((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"]).Where(x => string.IsNullOrEmpty(x.SelectedSeat)).Count() > 0)
                                    {
                                        //lblErr.Text = "Please select the seat(s) for Depart."
                                        //pnlErr.Visible = True
                                        //Session["ErrorMsg"] = lblErr.Text
                                        SeatInfo0Checking = SeatInfo0Checking + 1;
                                        //Response.Redirect("~/SeatConnected.aspx")
                                        //Exit Sub
                                    }
                                    if (SeatInfo0Checking == 0)
                                    {
                                        //if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"], model, EnumFlightType.DepartFlight))
                                        //{
                                        //    DeleteXML((string)Session["SeatInfo0Xml"]);
                                        //    Session["DepartSeatInfo"] = Session["SeatInfo0"];
                                        //}
                                    }

                                }
                                else
                                {
                                    SeatInfo0Checking = SeatInfo0Checking + 1;
                                    //lblErr.Text = "Please select Depart seat before proceed."
                                    //pnlErr.Visible = True
                                    //Session["ErrorMsg"] = lblErr.Text
                                    //Response.Redirect("~/SeatConnected.aspx")
                                }
                            }


                            if (IsInternationalFlight != true)
                            {
                                SeatInfo1Checking = 0;
                            }
                            else
                            {
                                if (Session["SeatInfo1"] == null == false)
                                {
                                    if (((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"]).Where(x => string.IsNullOrEmpty(x.SelectedSeat)).Count() > 0)
                                    {
                                        //lblErr.Text = "Please select the seat(s) for Depart."
                                        //pnlErr.Visible = True
                                        //Session["ErrorMsg"] = lblErr.Text
                                        SeatInfo1Checking = SeatInfo1Checking + 1;
                                        //Response.Redirect("~/SeatConnected.aspx")
                                        //Exit Sub
                                    }
                                    if (SeatInfo1Checking == 0)
                                    {
                                        //if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"], model, EnumFlightType.DepartConnectingFlight))
                                        //{
                                        //    DeleteXML((string)Session["SeatInfo1Xml"]);
                                        //    Session["DepartConnectingSeatInfo"] = Session["SeatInfo1"];
                                        //}
                                    }


                                }
                                else if (model.TemFlightTransit.Trim() == "")
                                {
                                    SeatInfo1Checking = 0;
                                }
                                else
                                {
                                    SeatInfo1Checking = SeatInfo1Checking + 1;
                                    //lblErr.Text = "Please select Depart seat before proceed."
                                    //pnlErr.Visible = True
                                    //Session["ErrorMsg"] = lblErr.Text
                                    //Response.Redirect("~/SeatConnected.aspx")

                                }
                            }
                        }
                        else
                        {
                            if (Session["ReturnFlightInfo"] == null)
                            {
                                Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.InvalidPage);
                            }
                        }

                        //handle if empty seat on return connecting flight (Sienny 20170308)
                        if (model2 == null == false)
                        //if (Session["ReturnFlightInfo"] == null == false)
                        {
                            Boolean IsInternationalFlight = objGeneral.IsInternationalFlight(model2.TemFlightDeparture, model2.TemFlightArrival, Request.PhysicalApplicationPath);
                            //Flight_Info pReturnFlightInfo = new Flight_Info();
                            //pReturnFlightInfo = Session["ReturnFlightInfo"];
                            if (IsInternationalFlight != true)
                            {
                                SeatInfo2Checking = 0;
                            }
                            else
                            {
                                if (Session["SeatInfo2"] == null == false)
                                {
                                    if (((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"]).Where(x => string.IsNullOrEmpty(x.SelectedSeat)).Count() > 0)
                                    {
                                        //lblErr.Text = "Please select the seat(s) for Return."
                                        //pnlErr.Visible = True

                                        //Session["ErrorMsg"] = lblErr.Text
                                        SeatInfo2Checking = SeatInfo2Checking + 1;
                                        //Response.Redirect("~/SeatConnected.aspx")
                                        //Exit Sub
                                    }
                                    if (SeatInfo2Checking == 0)
                                    {
                                        //if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"], model2, EnumFlightType.ReturnFlight))
                                        //{
                                        //    DeleteXML((string)Session["SeatInfo2Xml"]);
                                        //    Session["ReturnSeatInfo"] = Session["SeatInfo2"];
                                        //}
                                    }
                                }
                                else
                                {
                                    SeatInfo2Checking = SeatInfo2Checking + 1;
                                    // lblErr.Text = "Please select Return seat before proceed."
                                    // pnlErr.Visible = True

                                    // Session["ErrorMsg"] = lblErr.Text
                                    //Response.Redirect("~/SeatConnected.aspx")
                                    //Exit Sub
                                }
                            }

                            if (IsInternationalFlight != true)
                            {
                                SeatInfo3Checking = 0;
                            }
                            else
                            {
                                if (Session["SeatInfo3"] == null == false)
                                {

                                    if (((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo3"]).Where(x => string.IsNullOrEmpty(x.SelectedSeat)).Count() > 0)
                                    {
                                        //lblErr.Text = "Please select the seat(s) for Return."
                                        //pnlErr.Visible = True
                                        // Session["ErrorMsg"] = lblErr.Text
                                        SeatInfo3Checking = SeatInfo3Checking + 1;
                                        //Response.Redirect("~/SeatConnected.aspx")
                                        //return;
                                    }

                                    if (SeatInfo3Checking == 0)
                                    {
                                        //if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo3"], model2, EnumFlightType.ReturnConnectingFlight))
                                        //{
                                        //    DeleteXML((string)Session["SeatInfo3Xml"]);
                                        //    Session["ReturnConnectingSeatInfo"] = Session["SeatInfo3"];
                                        //}
                                    }


                                }
                                else if (model2.TemFlightTransit.Trim() == "")
                                {
                                    SeatInfo3Checking = 0;
                                }
                                else
                                {
                                    SeatInfo3Checking = SeatInfo3Checking + 1;
                                    //lblErr.Text = "Please select Return seat before proceed."
                                    //pnlErr.Visible = True

                                    // Session["ErrorMsg"] = lblErr.Text
                                    //Response.Redirect("~/SeatConnected.aspx")
                                    //Exit Sub
                                }
                            }
                        }
                        Page.Validate("PrimaryMandatory");
                        if (Page.IsValid)
                        {
                            ClearSeatFeeValue();
                            if (SeatInfo0Checking == 0 & SeatInfo1Checking == 0 & SeatInfo2Checking == 0 & SeatInfo3Checking == 0)
                            {
                                //API.DeleteXML(Session["SeatInfo0Xml"])
                                //API.DeleteXML(Session["SeatInfo1Xml"])
                                //API.DeleteXML(Session["SeatInfo2Xml"])
                                //API.DeleteXML(Session["SeatInfo3Xml"])
                                if (SeatInfo0Checking == 0 && model.TemFlightTransit != "")
                                {
                                    List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = null;
                                    if (Session["DepartConnectingExistingSeatInfo"] != null)
                                    {
                                        unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartConnectingExistingSeatInfo"];
                                    }
                                    using (profiler.Step("GBS:AssignSeat"))
                                    {
                                        if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"], unassignSeatinfo, model, EnumFlightType.DepartConnectingFlight))
                                        {
                                            DeleteXML((string)Session["SeatInfo0Xml"]);
                                            Session["DepartSeatInfo"] = Session["SeatInfo0"];
                                            List<ABS.Logic.GroupBooking.SeatInfo> ListDepartSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartSeatInfo"];
                                            List<ABS.Logic.GroupBooking.SeatInfo> ListDepartExistingSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartConnectingExistingSeatInfo"];
                                            if (ListDepartSeatInfo != null && ListDepartExistingSeatInfo != null)
                                            {
                                                HashSet<string> diffids = new HashSet<string>(ListDepartSeatInfo.Select(s => s.SelectedSeat));
                                                var results = ListDepartExistingSeatInfo.Where(m => !diffids.Contains(m.SelectedSeat)).ToList();
                                                if (results != null && results.Count > 0)
                                                {
                                                    return msgList.Err1000000;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            return msgList.Err999997;
                                        }
                                    }
                                }
                                else if (SeatInfo0Checking == 0)
                                {
                                    List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = null;
                                    if (Session["DepartExistingSeatInfo"] != null)
                                    {
                                        unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
                                    }

                                    using (profiler.Step("GBS:AssignSeat"))
                                    {
                                        if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"], unassignSeatinfo, model, EnumFlightType.DepartFlight))
                                        {
                                            DeleteXML((string)Session["SeatInfo0Xml"]);
                                            Session["DepartSeatInfo"] = Session["SeatInfo0"];
                                            List<ABS.Logic.GroupBooking.SeatInfo> ListDepartSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartSeatInfo"];
                                            List<ABS.Logic.GroupBooking.SeatInfo> ListDepartExistingSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
                                            if (ListDepartSeatInfo != null && ListDepartExistingSeatInfo != null)
                                            {
                                                HashSet<string> diffids = new HashSet<string>(ListDepartSeatInfo.Select(s => s.SelectedSeat));
                                                var results = ListDepartExistingSeatInfo.Where(m => !diffids.Contains(m.SelectedSeat)).ToList();
                                                if (results != null && results.Count > 0)
                                                {
                                                    return msgList.Err1000000;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            return msgList.Err999997;
                                        }
                                    }
                                }
                                if (SeatInfo1Checking == 0 && model.TemFlightTransit != "")
                                {
                                    List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = null;
                                    if (Session["DepartConnectingExistingSeatInfo2"] != null)
                                    {
                                        unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartConnectingExistingSeatInfo2"];
                                    }

                                    using (profiler.Step("GBS:AssignSeat"))
                                    {
                                        if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"], unassignSeatinfo, model, EnumFlightType.DepartConnectingFlight2))
                                        {
                                            DeleteXML((string)Session["SeatInfo1Xml"]);
                                            Session["DepartConnectingSeatInfo"] = Session["SeatInfo1"];
                                            List<ABS.Logic.GroupBooking.SeatInfo> ListDepartSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartConnectingSeatInfo"];
                                            List<ABS.Logic.GroupBooking.SeatInfo> ListDepartExistingSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartConnectingExistingSeatInfo2"];
                                            if (ListDepartSeatInfo != null && ListDepartExistingSeatInfo != null)
                                            {
                                                HashSet<string> diffids = new HashSet<string>(ListDepartSeatInfo.Select(s => s.SelectedSeat));
                                                var results = ListDepartExistingSeatInfo.Where(m => !diffids.Contains(m.SelectedSeat)).ToList();
                                                if (results != null && results.Count > 0)
                                                {
                                                    return msgList.Err1000000;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            return msgList.Err999997;
                                        }
                                    }
                                }
                                if (SeatInfo2Checking == 0 && model2 != null && model2.TemFlightTransit != "")
                                {
                                    List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = null;
                                    if (Session["ReturnConnectingExistingSeatInfo"] != null)
                                    {
                                        unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnConnectingExistingSeatInfo"];
                                    }

                                    using (profiler.Step("GBS:AssignSeat"))
                                    {
                                        if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"], unassignSeatinfo, model, EnumFlightType.ReturnConnectingFlight))
                                        {
                                            DeleteXML((string)Session["SeatInfo2Xml"]);
                                            Session["ReturnSeatInfo"] = Session["SeatInfo2"];
                                            List<ABS.Logic.GroupBooking.SeatInfo> ListReturnSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnSeatInfo"];
                                            List<ABS.Logic.GroupBooking.SeatInfo> ListReturnExistingSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnConnectingExistingSeatInfo"];
                                            if (ListReturnSeatInfo != null && ListReturnExistingSeatInfo != null)
                                            {
                                                HashSet<string> diffids = new HashSet<string>(ListReturnSeatInfo.Select(s => s.SelectedSeat));
                                                var results = ListReturnExistingSeatInfo.Where(m => !diffids.Contains(m.SelectedSeat)).ToList();
                                                if (results != null && results.Count > 0)
                                                {
                                                    return msgList.Err1000000;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            return msgList.Err999997;
                                        }
                                    }
                                }
                                else if (SeatInfo2Checking == 0)
                                {
                                    List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = null;
                                    if (Session["ReturnExistingSeatInfo"] != null)
                                    {
                                        unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnExistingSeatInfo"];
                                    }

                                    using (profiler.Step("GBS:AssignSeat"))
                                    {
                                        if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"], unassignSeatinfo, model, EnumFlightType.ReturnFlight))
                                        {
                                            DeleteXML((string)Session["SeatInfo2Xml"]);
                                            Session["ReturnSeatInfo"] = Session["SeatInfo2"];
                                            List<ABS.Logic.GroupBooking.SeatInfo> ListReturnSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnSeatInfo"];
                                            List<ABS.Logic.GroupBooking.SeatInfo> ListReturnExistingSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnExistingSeatInfo"];
                                            if (ListReturnSeatInfo != null && ListReturnExistingSeatInfo != null)
                                            {
                                                HashSet<string> diffids = new HashSet<string>(ListReturnSeatInfo.Select(s => s.SelectedSeat));
                                                var results = ListReturnExistingSeatInfo.Where(m => !diffids.Contains(m.SelectedSeat)).ToList();
                                                if (results != null && results.Count > 0)
                                                {
                                                    return msgList.Err1000000;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            return msgList.Err999997;
                                        }
                                    }
                                }
                                if (SeatInfo3Checking == 0 && model2 != null && model2.TemFlightTransit != "")
                                {
                                    List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = null;
                                    if (Session["ReturnConnectingExistingSeatInfo2"] != null)
                                    {
                                        unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnConnectingExistingSeatInfo2"];
                                    }

                                    using (profiler.Step("GBS:AssignSeat"))
                                    {
                                        if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo3"], unassignSeatinfo, model, EnumFlightType.ReturnConnectingFlight2))
                                        {
                                            DeleteXML((string)Session["SeatInfo3Xml"]);
                                            Session["ReturnConnectingSeatInfo"] = Session["SeatInfo3"];
                                            List<ABS.Logic.GroupBooking.SeatInfo> ListReturnSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnConnectingSeatInfo"];
                                            List<ABS.Logic.GroupBooking.SeatInfo> ListReturnExistingSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnConnectingExistingSeatInfo2"];
                                            if (ListReturnSeatInfo != null && ListReturnExistingSeatInfo != null)
                                            {
                                                HashSet<string> diffids = new HashSet<string>(ListReturnSeatInfo.Select(s => s.SelectedSeat));
                                                var results = ListReturnExistingSeatInfo.Where(m => !diffids.Contains(m.SelectedSeat)).ToList();
                                                if (results != null && results.Count > 0)
                                                {
                                                    return msgList.Err1000000;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            return msgList.Err999997;
                                        }
                                    }
                                }

                                //Response.Redirect("~/pages/SeatSummary.aspx");
                            }
                            else
                            {
                                return msgList.Err999996;
                                //lblErr.Text = msgList.Err999996;
                                //pnlErr.Visible = true;
                                //Session["ErrorMsg"] = lblErr.Text;
                                //Response.Redirect("~/seats.aspx");
                                //FillFlight(Session["akey"], 0)
                            }

                            assignSeatDone = true;
                            //return "";

                        }
                        break;
                    case EnumFlight.DirectFlight:
                        if (model == null == false)
                        {
                            Boolean IsInternationalFlight = objGeneral.IsInternationalFlight(model.TemFlightDeparture, model.TemFlightArrival, Request.PhysicalApplicationPath);
                            //Flight_Info pDepartFlightInfo = new Flight_Info();
                            //pDepartFlightInfo = Session["DepartFlightInfo"];

                            if (IsInternationalFlight != true)
                            {
                                SeatInfo0Checking = 0;
                            }
                            else
                            {
                                if (Session["SeatInfo0"] == null == false)
                                {
                                    if (((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"]).Where(x => string.IsNullOrEmpty(x.SelectedSeat)).Count() > 0)
                                    {
                                        SeatInfo0Checking = SeatInfo0Checking + 1;
                                    }
                                    if (SeatInfo0Checking == 0)
                                    {
                                        //if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"], model, EnumFlightType.DepartFlight))
                                        //{
                                        //    DeleteXML((string)Session["SeatInfo0Xml"]);
                                        //    Session["DepartSeatInfo"] = Session["SeatInfo0"];
                                        //}
                                    }

                                }
                                else
                                {
                                    SeatInfo0Checking = SeatInfo0Checking + 1;
                                }
                            }
                        }
                        else
                        {
                            //Response.Redirect("~/pages/InvalidPage.aspx")
                            SeatInfo0Checking = 0;
                        }

                        if (model2 == null == false)
                        {
                            Boolean IsInternationalFlight = objGeneral.IsInternationalFlight(model2.TemFlightDeparture, model2.TemFlightArrival, Request.PhysicalApplicationPath);
                            //Flight_Info pReturnFlightInfo = new Flight_Info();
                            //pReturnFlightInfo = Session["ReturnFlightInfo"];

                            if (IsInternationalFlight != true)
                            {
                                SeatInfo2Checking = 0;
                            }
                            else
                            {
                                if (Session["SeatInfo2"] == null == false)
                                {
                                    if (((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"]).Where(x => string.IsNullOrEmpty(x.SelectedSeat)).Count() > 0)
                                    {
                                        //lblErr.Text = "Please select the seat(s) for Return."
                                        //pnlErr.Visible = True

                                        //Session["ErrorMsg"] = lblErr.Text
                                        SeatInfo2Checking = SeatInfo2Checking + 1;
                                        //Response.Redirect("~/SeatConnected.aspx")
                                        //Exit Sub
                                    }
                                    if (SeatInfo2Checking == 0)
                                    {
                                        //if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"], model2, EnumFlightType.ReturnFlight))
                                        //{
                                        //    DeleteXML((string)Session["SeatInfo2Xml"]);
                                        //    Session["ReturnSeatInfo"] = Session["SeatInfo2"];
                                        //}
                                    }
                                }
                                else
                                {
                                    SeatInfo2Checking = SeatInfo2Checking + 1;
                                    // lblErr.Text = "Please select Return seat before proceed."
                                    // pnlErr.Visible = True

                                    // Session["ErrorMsg"] = lblErr.Text
                                    //Response.Redirect("~/SeatConnected.aspx")
                                    //Exit Sub
                                }
                            }

                        }
                        else
                        {
                            SeatInfo2Checking = 0;
                        }
                        Page.Validate("PrimaryMandatory");
                        if (Page.IsValid)
                        {
                            if (SeatInfo0Checking == 0 & SeatInfo2Checking == 0)
                            {
                                ClearSeatFeeValue();
                                if (SeatInfo0Checking == 0 && model != null)
                                {
                                    List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = null;
                                    if (Session["DepartExistingSeatInfo"] != null)
                                    {
                                        unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
                                    }

                                    using (profiler.Step("GBS:AssignSeat"))
                                    {
                                        if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"], unassignSeatinfo, model, EnumFlightType.DepartFlight))
                                        {
                                            DeleteXML((string)Session["SeatInfo0Xml"]);
                                            Session["DepartSeatInfo"] = Session["SeatInfo0"];
                                            List<ABS.Logic.GroupBooking.SeatInfo> ListDepartSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartSeatInfo"];
                                            List<ABS.Logic.GroupBooking.SeatInfo> ListDepartExistingSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
                                            if (ListDepartSeatInfo != null && ListDepartExistingSeatInfo != null)
                                            {
                                                HashSet<string> diffids = new HashSet<string>(ListDepartSeatInfo.Select(s => s.SelectedSeat));
                                                var results = ListDepartExistingSeatInfo.Where(m => !diffids.Contains(m.SelectedSeat)).ToList();
                                                if (results != null && results.Count > 0)
                                                {
                                                    return msgList.Err1000000;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            return msgList.Err999997;
                                        }
                                    }
                                }

                                if (SeatInfo2Checking == 0 && model2 != null)
                                {
                                    List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = null;
                                    if (Session["ReturnExistingSeatInfo"] != null)
                                    {
                                        unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnExistingSeatInfo"];
                                    }

                                    using (profiler.Step("GBS:AssignSeat"))
                                    {
                                        if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"], unassignSeatinfo, model2, EnumFlightType.ReturnFlight))
                                        {
                                            DeleteXML((string)Session["SeatInfo2Xml"]);
                                            Session["ReturnSeatInfo"] = Session["SeatInfo2"];
                                            List<ABS.Logic.GroupBooking.SeatInfo> ListReturnSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnSeatInfo"];
                                            List<ABS.Logic.GroupBooking.SeatInfo> ListReturnExistingSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnExistingSeatInfo"];
                                            if (ListReturnSeatInfo != null && ListReturnExistingSeatInfo != null)
                                            {
                                                HashSet<string> diffids = new HashSet<string>(ListReturnSeatInfo.Select(s => s.SelectedSeat));
                                                var results = ListReturnExistingSeatInfo.Where(m => !diffids.Contains(m.SelectedSeat)).ToList();
                                                if (results != null && results.Count > 0)
                                                {
                                                    return msgList.Err1000000;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            return msgList.Err999997;
                                        }
                                    }
                                }



                                //end, update total amount
                                assignSeatDone = true;
                                //return "";
                                //Response.Redirect("~/pages/SeatSummary.aspx");
                            }
                            else
                            {
                                return msgList.Err999996;
                                //lblErr.Text = msgList.Err999996;
                                //pnlErr.Visible = true;
                                //Session["ErrorMsg"] = lblErr.Text;
                                //Response.Redirect("~/seats.aspx");
                            }
                        }


                        break;
                }

                if (assignSeatDone)
                {
                    //begin, update total amount
                    if (Session["TransID"] != null)
                    {
                        decimal numValue;
                        decimal TotalSeatDepart = 0;
                        decimal TotalSeatReturn = 0;

                        string TransID = (string)Session["TransID"];
                        //tyas'
                        //List<BookingTransactionDetail> listBookingDetail = new List<BookingTransactionDetail>();
                        //listBookingDetail = objBooking.GetAllBK_TRANSDTLFilterAll(TransID, 0);
                        //tyas
                        listBookingDetail = Session["listBookingDetail"] as List<BookingTransactionDetail>;


                        //if (ReturnID != "")
                        //{
                        //    int iIndexDepart = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 1 && p.Signature == SessionID);
                        //    int iIndexReturn = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 0 && p.Signature == SessionID);
                        //    if (iIndexDepart >= 0) listBookingDetail[iIndexDepart].LineSeat = TotalSeatDepart;
                        //    if (iIndexReturn >= 0) listBookingDetail[iIndexReturn].LineSeat = TotalSeatReturn;
                        //}
                        //else
                        //{
                        //    int iIndexDepart = listBookingDetail.FindIndex(p => p.Signature == SessionID);
                        //    if (iIndexDepart >= 0) listBookingDetail[iIndexDepart].LineSeat = TotalSeatDepart;
                        //}

                        //tyas
                        //int iIndexDepart = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 1);
                        //int iIndexReturn = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 0);
                        if (Session["DepartFlightSeatFees"] != null)
                        {
                            if (decimal.TryParse(Session["DepartFlightSeatFees"].ToString(), out numValue)) TotalSeatDepart = TotalSeatDepart + Convert.ToDecimal(Session["DepartFlightSeatFees"].ToString());
                        }
                        if (Session["DepartConnectingFlightSeatFees"] != null)
                        {
                            if (decimal.TryParse(Session["DepartConnectingFlightSeatFees"].ToString(), out numValue)) TotalSeatDepart = TotalSeatDepart + Convert.ToDecimal(Session["DepartConnectingFlightSeatFees"].ToString());
                        }
                        if (Session["DepartConnectingFlightSeatFees2"] != null)
                        {
                            if (decimal.TryParse(Session["DepartConnectingFlightSeatFees2"].ToString(), out numValue)) TotalSeatDepart = TotalSeatDepart + Convert.ToDecimal(Session["DepartConnectingFlightSeatFees2"].ToString());
                        }

                        if (Session["ReturnFlightSeatFees"] != null)
                        {
                            if (decimal.TryParse(Session["ReturnFlightSeatFees"].ToString(), out numValue)) TotalSeatReturn = TotalSeatReturn + Convert.ToDecimal(Session["ReturnFlightSeatFees"].ToString());
                        }
                        if (Session["ReturnConnectingFlightSeatFees"] != null)
                        {
                            if (decimal.TryParse(Session["ReturnConnectingFlightSeatFees"].ToString(), out numValue)) TotalSeatReturn = TotalSeatReturn + Convert.ToDecimal(Session["ReturnConnectingFlightSeatFees"].ToString());
                        }
                        if (Session["ReturnConnectingFlightSeatFees2"] != null)
                        {
                            if (decimal.TryParse(Session["ReturnConnectingFlightSeatFees2"].ToString(), out numValue)) TotalSeatReturn = TotalSeatReturn + Convert.ToDecimal(Session["ReturnConnectingFlightSeatFees2"].ToString());
                        }

                        decimal TotalAmountGoing = 0;
                        decimal TotalAmountReturn = 0;

                        if (Request.QueryString["change"] == null)
                        {
                            using (profiler.Step("GBS:UpdateTotalAmount"))
                            {
                                UpdateTotalAmount(TotalSeatDepart, TotalSeatReturn, ref TotalAmountGoing, ref TotalAmountReturn);
                            }
                        }

                        //if (iIndexDepart >= 0) listBookingDetail[iIndexDepart].LineSeat = TotalSeatDepart;
                        //if (iIndexReturn >= 0) listBookingDetail[iIndexReturn].LineSeat = TotalSeatReturn;
                        if (Request.QueryString["change"] != null)
                        {

                            //if (Session["ChgbookHDRInfo"] != null && Session["ChglstbookDTLInfo"] != null)
                            //{

                            //    UpdateAllBookingJourneyDetail();
                            //}
                        }
                        else
                        {
                            BookingTransactionMain bookingMain = new BookingTransactionMain();
                            bookingMain = objBooking.GetSingleBK_TRANSMAIN(TransID);
                            bookingMain.TransTotalSeat = TotalSeatDepart + TotalSeatReturn;
                            bookingMain.TotalAmtGoing = TotalAmountGoing;
                            bookingMain.TotalAmtReturn = TotalAmountReturn;
                            bookingMain.TransSubTotal = TotalAmountGoing + TotalAmountReturn;
                            bookingMain.TransTotalAmt = TotalAmountGoing + TotalAmountReturn;

                            //added by ketee, for conencting flight, total up the seat fees from the connecting seat fees columns
                            foreach (BookingTransactionDetail a in listBookingDetail)
                            {
                                if (a.LineConnectingSeat != null && a.LineConnectingSeat > 0 && a.LineConnectingSeat2 != null && a.LineConnectingSeat2 > 0)
                                {
                                    a.LineSeat = a.LineConnectingSeat + a.LineConnectingSeat2;
                                }
                            }
                            using (profiler.Step("GBS:UpdateTotalSeat"))
                            {
                                objBooking.UpdateTotalSeat(TransID, bookingMain, listBookingDetail);
                            }
                        }

                        if (Request.QueryString["change"] != null)
                        {

                            if (Session["ChgbookHDRInfo"] != null && Session["ChglstbookDTLInfo"] != null)
                            {
                                using (profiler.Step("GBS:UpdateAllBookingJourneyDetail"))
                                {
                                    UpdateAllBookingJourneyDetail();
                                }
                            }
                        }
                        return "";
                    }
                    else
                    {
                        return msgList.Err999995;
                    }
                }
                else
                {
                    return msgList.Err999997;
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                return ex.Message;
                //lblErr.Text = ex.ToString();
            }
        }

        protected string ValidateSeatRandomSeat()
        {
            var profiler = MiniProfiler.Current;
            MessageList msgList = new MessageList();
            bool assignSeatDone = false;
            try
            {
                pnlErr.Visible = false;
                int SeatInfo0Checking = 0;
                int SeatInfo1Checking = 0;
                int SeatInfo2Checking = 0;
                int SeatInfo3Checking = 0;

                bool IsOneWay = false;

                EnumFlight eFlights = eFlight;
                if (eFlight2 == EnumFlight.ConnectingFlight)
                {
                    eFlights = eFlight2;
                }

                switch (eFlights)
                {
                    case EnumFlight.ConnectingFlight:
                        if (model == null == false)
                        {
                            Boolean IsInternationalFlight = objGeneral.IsInternationalFlight(model.TemFlightDeparture, model.TemFlightArrival, Request.PhysicalApplicationPath);
                            //Flight_Info pDepartFlightInfo = new Flight_Info();
                            //pDepartFlightInfo = Session["DepartFlightInfo"];

                            if (IsInternationalFlight != true)
                            {
                                SeatInfo0Checking = 0;
                            }
                            else
                            {
                                if (Session["SeatInfo0"] == null == false)
                                {
                                    if (((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"]).Where(x => string.IsNullOrEmpty(x.SelectedSeat)).Count() > 0)
                                    {
                                        //lblErr.Text = "Please select the seat(s) for Depart."
                                        //pnlErr.Visible = True
                                        //Session["ErrorMsg"] = lblErr.Text
                                        SeatInfo0Checking = SeatInfo0Checking + 1;
                                        //Response.Redirect("~/SeatConnected.aspx")
                                        //Exit Sub
                                    }
                                    if (SeatInfo0Checking == 0)
                                    {
                                        //if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"], model, EnumFlightType.DepartFlight))
                                        //{
                                        //    DeleteXML((string)Session["SeatInfo0Xml"]);
                                        //    Session["DepartSeatInfo"] = Session["SeatInfo0"];
                                        //}
                                    }

                                }
                                else
                                {
                                    SeatInfo0Checking = SeatInfo0Checking + 1;
                                    //lblErr.Text = "Please select Depart seat before proceed."
                                    //pnlErr.Visible = True
                                    //Session["ErrorMsg"] = lblErr.Text
                                    //Response.Redirect("~/SeatConnected.aspx")
                                }
                            }


                            if (IsInternationalFlight != true)
                            {
                                SeatInfo1Checking = 0;
                            }
                            else
                            {
                                if (Session["SeatInfo1"] == null == false)
                                {
                                    if (((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"]).Where(x => string.IsNullOrEmpty(x.SelectedSeat)).Count() > 0)
                                    {
                                        //lblErr.Text = "Please select the seat(s) for Depart."
                                        //pnlErr.Visible = True
                                        //Session["ErrorMsg"] = lblErr.Text
                                        SeatInfo1Checking = SeatInfo1Checking + 1;
                                        //Response.Redirect("~/SeatConnected.aspx")
                                        //Exit Sub
                                    }
                                    if (SeatInfo1Checking == 0)
                                    {
                                        //if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"], model, EnumFlightType.DepartConnectingFlight))
                                        //{
                                        //    DeleteXML((string)Session["SeatInfo1Xml"]);
                                        //    Session["DepartConnectingSeatInfo"] = Session["SeatInfo1"];
                                        //}
                                    }


                                }
                                else if (model.TemFlightTransit.Trim() == "")
                                {
                                    SeatInfo1Checking = 0;
                                }
                                else
                                {
                                    SeatInfo1Checking = SeatInfo1Checking + 1;
                                    //lblErr.Text = "Please select Depart seat before proceed."
                                    //pnlErr.Visible = True
                                    //Session["ErrorMsg"] = lblErr.Text
                                    //Response.Redirect("~/SeatConnected.aspx")

                                }
                            }
                        }
                        else
                        {
                            if (Session["ReturnFlightInfo"] == null)
                            {
                                Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.InvalidPage);
                            }
                        }

                        //handle if empty seat on return connecting flight (Sienny 20170308)
                        if (model2 == null == false)
                        //if (Session["ReturnFlightInfo"] == null == false)
                        {
                            Boolean IsInternationalFlight = objGeneral.IsInternationalFlight(model2.TemFlightDeparture, model2.TemFlightArrival, Request.PhysicalApplicationPath);
                            //Flight_Info pReturnFlightInfo = new Flight_Info();
                            //pReturnFlightInfo = Session["ReturnFlightInfo"];
                            if (IsInternationalFlight != true)
                            {
                                SeatInfo2Checking = 0;
                            }
                            else
                            {
                                if (Session["SeatInfo2"] == null == false)
                                {
                                    if (((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"]).Where(x => string.IsNullOrEmpty(x.SelectedSeat)).Count() > 0)
                                    {
                                        //lblErr.Text = "Please select the seat(s) for Return."
                                        //pnlErr.Visible = True

                                        //Session["ErrorMsg"] = lblErr.Text
                                        SeatInfo2Checking = SeatInfo2Checking + 1;
                                        //Response.Redirect("~/SeatConnected.aspx")
                                        //Exit Sub
                                    }
                                    if (SeatInfo2Checking == 0)
                                    {
                                        //if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"], model2, EnumFlightType.ReturnFlight))
                                        //{
                                        //    DeleteXML((string)Session["SeatInfo2Xml"]);
                                        //    Session["ReturnSeatInfo"] = Session["SeatInfo2"];
                                        //}
                                    }
                                }
                                else
                                {
                                    SeatInfo2Checking = SeatInfo2Checking + 1;
                                    // lblErr.Text = "Please select Return seat before proceed."
                                    // pnlErr.Visible = True

                                    // Session["ErrorMsg"] = lblErr.Text
                                    //Response.Redirect("~/SeatConnected.aspx")
                                    //Exit Sub
                                }
                            }

                            if (IsInternationalFlight != true)
                            {
                                SeatInfo3Checking = 0;
                            }
                            else
                            {
                                if (Session["SeatInfo3"] == null == false)
                                {

                                    if (((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo3"]).Where(x => string.IsNullOrEmpty(x.SelectedSeat)).Count() > 0)
                                    {
                                        //lblErr.Text = "Please select the seat(s) for Return."
                                        //pnlErr.Visible = True
                                        // Session["ErrorMsg"] = lblErr.Text
                                        SeatInfo3Checking = SeatInfo3Checking + 1;
                                        //Response.Redirect("~/SeatConnected.aspx")
                                        //return;
                                    }

                                    if (SeatInfo3Checking == 0)
                                    {
                                        //if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo3"], model2, EnumFlightType.ReturnConnectingFlight))
                                        //{
                                        //    DeleteXML((string)Session["SeatInfo3Xml"]);
                                        //    Session["ReturnConnectingSeatInfo"] = Session["SeatInfo3"];
                                        //}
                                    }


                                }
                                else if (model2.TemFlightTransit.Trim() == "")
                                {
                                    SeatInfo3Checking = 0;
                                }
                                else
                                {
                                    SeatInfo3Checking = SeatInfo3Checking + 1;
                                    //lblErr.Text = "Please select Return seat before proceed."
                                    //pnlErr.Visible = True

                                    // Session["ErrorMsg"] = lblErr.Text
                                    //Response.Redirect("~/SeatConnected.aspx")
                                    //Exit Sub
                                }
                            }
                        }
                        Page.Validate("PrimaryMandatory");
                        if (Page.IsValid)
                        {
                            ClearSeatFeeValue();
                            if (SeatInfo0Checking == 0 & SeatInfo1Checking == 0 & SeatInfo2Checking == 0 & SeatInfo3Checking == 0)
                            {
                                //API.DeleteXML(Session["SeatInfo0Xml"])
                                //API.DeleteXML(Session["SeatInfo1Xml"])
                                //API.DeleteXML(Session["SeatInfo2Xml"])
                                //API.DeleteXML(Session["SeatInfo3Xml"])
                                if (SeatInfo0Checking == 0 && model.TemFlightTransit != "")
                                {
                                    List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = null;
                                    if (Session["DepartConnectingExistingSeatInfo"] != null)
                                    {
                                        unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartConnectingExistingSeatInfo"];
                                    }
                                    using (profiler.Step("GBS:AssignSeat"))
                                    {
                                        if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"], unassignSeatinfo, model, EnumFlightType.DepartConnectingFlight))
                                        {
                                            DeleteXML((string)Session["SeatInfo0Xml"]);
                                            Session["DepartSeatInfo"] = Session["SeatInfo0"];
                                            //List<ABS.Logic.GroupBooking.SeatInfo> ListDepartSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartSeatInfo"];
                                            //List<ABS.Logic.GroupBooking.SeatInfo> ListDepartExistingSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartConnectingExistingSeatInfo"];
                                            //if (ListDepartSeatInfo != null && ListDepartExistingSeatInfo != null)
                                            //{
                                            //    HashSet<string> diffids = new HashSet<string>(ListDepartSeatInfo.Select(s => s.SelectedSeat));
                                            //    var results = ListDepartExistingSeatInfo.Where(m => !diffids.Contains(m.SelectedSeat)).ToList();
                                            //    if (results != null && results.Count > 0)
                                            //    {
                                            //        return msgList.Err1000000;
                                            //    }
                                            //}
                                        }
                                        else
                                        {
                                            return msgList.Err999997;
                                        }
                                    }
                                }
                                else if (SeatInfo0Checking == 0)
                                {
                                    List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = null;
                                    if (Session["DepartExistingSeatInfo"] != null)
                                    {
                                        unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
                                    }

                                    using (profiler.Step("GBS:AssignSeat"))
                                    {
                                        if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"], unassignSeatinfo, model, EnumFlightType.DepartFlight))
                                        {
                                            DeleteXML((string)Session["SeatInfo0Xml"]);
                                            Session["DepartSeatInfo"] = Session["SeatInfo0"];
                                            //List<ABS.Logic.GroupBooking.SeatInfo> ListDepartSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartSeatInfo"];
                                            //List<ABS.Logic.GroupBooking.SeatInfo> ListDepartExistingSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
                                            //if (ListDepartSeatInfo != null && ListDepartExistingSeatInfo != null)
                                            //{
                                            //    HashSet<string> diffids = new HashSet<string>(ListDepartSeatInfo.Select(s => s.SelectedSeat));
                                            //    var results = ListDepartExistingSeatInfo.Where(m => !diffids.Contains(m.SelectedSeat)).ToList();
                                            //    if (results != null && results.Count > 0)
                                            //    {
                                            //        return msgList.Err1000000;
                                            //    }
                                            //}
                                        }
                                        else
                                        {
                                            return msgList.Err999997;
                                        }
                                    }
                                }
                                if (SeatInfo1Checking == 0 && model.TemFlightTransit != "")
                                {
                                    List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = null;
                                    if (Session["DepartConnectingExistingSeatInfo2"] != null)
                                    {
                                        unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartConnectingExistingSeatInfo2"];
                                    }

                                    using (profiler.Step("GBS:AssignSeat"))
                                    {
                                        if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"], unassignSeatinfo, model, EnumFlightType.DepartConnectingFlight2))
                                        {
                                            DeleteXML((string)Session["SeatInfo1Xml"]);
                                            Session["DepartConnectingSeatInfo"] = Session["SeatInfo1"];
                                            //List<ABS.Logic.GroupBooking.SeatInfo> ListDepartSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartConnectingSeatInfo"];
                                            //List<ABS.Logic.GroupBooking.SeatInfo> ListDepartExistingSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartConnectingExistingSeatInfo2"];
                                            //if (ListDepartSeatInfo != null && ListDepartExistingSeatInfo != null)
                                            //{
                                            //    HashSet<string> diffids = new HashSet<string>(ListDepartSeatInfo.Select(s => s.SelectedSeat));
                                            //    var results = ListDepartExistingSeatInfo.Where(m => !diffids.Contains(m.SelectedSeat)).ToList();
                                            //    if (results != null && results.Count > 0)
                                            //    {
                                            //        return msgList.Err1000000;
                                            //    }
                                            //}
                                        }
                                        else
                                        {
                                            return msgList.Err999997;
                                        }
                                    }
                                }
                                if (SeatInfo2Checking == 0 && model2 != null && model2.TemFlightTransit != "")
                                {
                                    List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = null;
                                    if (Session["ReturnConnectingExistingSeatInfo"] != null)
                                    {
                                        unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnConnectingExistingSeatInfo"];
                                    }

                                    using (profiler.Step("GBS:AssignSeat"))
                                    {
                                        if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"], unassignSeatinfo, model, EnumFlightType.ReturnConnectingFlight))
                                        {
                                            DeleteXML((string)Session["SeatInfo2Xml"]);
                                            Session["ReturnSeatInfo"] = Session["SeatInfo2"];
                                            //List<ABS.Logic.GroupBooking.SeatInfo> ListReturnSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnSeatInfo"];
                                            //List<ABS.Logic.GroupBooking.SeatInfo> ListReturnExistingSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnConnectingExistingSeatInfo"];
                                            //if (ListReturnSeatInfo != null && ListReturnExistingSeatInfo != null)
                                            //{
                                            //    HashSet<string> diffids = new HashSet<string>(ListReturnSeatInfo.Select(s => s.SelectedSeat));
                                            //    var results = ListReturnExistingSeatInfo.Where(m => !diffids.Contains(m.SelectedSeat)).ToList();
                                            //    if (results != null && results.Count > 0)
                                            //    {
                                            //        return msgList.Err1000000;
                                            //    }
                                            //}
                                        }
                                        else
                                        {
                                            return msgList.Err999997;
                                        }
                                    }
                                }
                                else if (SeatInfo2Checking == 0)
                                {
                                    List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = null;
                                    if (Session["ReturnExistingSeatInfo"] != null)
                                    {
                                        unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnExistingSeatInfo"];
                                    }

                                    using (profiler.Step("GBS:AssignSeat"))
                                    {
                                        if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"], unassignSeatinfo, model, EnumFlightType.ReturnFlight))
                                        {
                                            DeleteXML((string)Session["SeatInfo2Xml"]);
                                            Session["ReturnSeatInfo"] = Session["SeatInfo2"];
                                            //List<ABS.Logic.GroupBooking.SeatInfo> ListReturnSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnSeatInfo"];
                                            //List<ABS.Logic.GroupBooking.SeatInfo> ListReturnExistingSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnExistingSeatInfo"];
                                            //if (ListReturnSeatInfo != null && ListReturnExistingSeatInfo != null)
                                            //{
                                            //    HashSet<string> diffids = new HashSet<string>(ListReturnSeatInfo.Select(s => s.SelectedSeat));
                                            //    var results = ListReturnExistingSeatInfo.Where(m => !diffids.Contains(m.SelectedSeat)).ToList();
                                            //    if (results != null && results.Count > 0)
                                            //    {
                                            //        return msgList.Err1000000;
                                            //    }
                                            //}
                                        }
                                        else
                                        {
                                            return msgList.Err999997;
                                        }
                                    }
                                }
                                if (SeatInfo3Checking == 0 && model2 != null && model2.TemFlightTransit != "")
                                {
                                    List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = null;
                                    if (Session["ReturnConnectingExistingSeatInfo2"] != null)
                                    {
                                        unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnConnectingExistingSeatInfo2"];
                                    }

                                    using (profiler.Step("GBS:AssignSeat"))
                                    {
                                        if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo3"], unassignSeatinfo, model, EnumFlightType.ReturnConnectingFlight2))
                                        {
                                            DeleteXML((string)Session["SeatInfo3Xml"]);
                                            Session["ReturnConnectingSeatInfo"] = Session["SeatInfo3"];
                                            //List<ABS.Logic.GroupBooking.SeatInfo> ListReturnSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnConnectingSeatInfo"];
                                            //List<ABS.Logic.GroupBooking.SeatInfo> ListReturnExistingSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnConnectingExistingSeatInfo2"];
                                            //if (ListReturnSeatInfo != null && ListReturnExistingSeatInfo != null)
                                            //{
                                            //    HashSet<string> diffids = new HashSet<string>(ListReturnSeatInfo.Select(s => s.SelectedSeat));
                                            //    var results = ListReturnExistingSeatInfo.Where(m => !diffids.Contains(m.SelectedSeat)).ToList();
                                            //    if (results != null && results.Count > 0)
                                            //    {
                                            //        return msgList.Err1000000;
                                            //    }
                                            //}
                                        }
                                        else
                                        {
                                            return msgList.Err999997;
                                        }
                                    }
                                }

                                //Response.Redirect("~/pages/SeatSummary.aspx");
                            }
                            else
                            {
                                return msgList.Err999996;
                                //lblErr.Text = msgList.Err999996;
                                //pnlErr.Visible = true;
                                //Session["ErrorMsg"] = lblErr.Text;
                                //Response.Redirect("~/seats.aspx");
                                //FillFlight(Session["akey"], 0)
                            }

                            assignSeatDone = true;
                            //return "";

                        }
                        break;
                    case EnumFlight.DirectFlight:
                        if (model == null == false)
                        {
                            Boolean IsInternationalFlight = objGeneral.IsInternationalFlight(model.TemFlightDeparture, model.TemFlightArrival, Request.PhysicalApplicationPath);
                            //Flight_Info pDepartFlightInfo = new Flight_Info();
                            //pDepartFlightInfo = Session["DepartFlightInfo"];

                            if (IsInternationalFlight != true)
                            {
                                SeatInfo0Checking = 0;
                            }
                            else
                            {
                                if (Session["SeatInfo0"] == null == false)
                                {
                                    if (((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"]).Where(x => string.IsNullOrEmpty(x.SelectedSeat)).Count() > 0)
                                    {
                                        SeatInfo0Checking = SeatInfo0Checking + 1;
                                    }
                                    if (SeatInfo0Checking == 0)
                                    {
                                        //if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"], model, EnumFlightType.DepartFlight))
                                        //{
                                        //    DeleteXML((string)Session["SeatInfo0Xml"]);
                                        //    Session["DepartSeatInfo"] = Session["SeatInfo0"];
                                        //}
                                    }

                                }
                                else
                                {
                                    SeatInfo0Checking = SeatInfo0Checking + 1;
                                }
                            }
                        }
                        else
                        {
                            //Response.Redirect("~/pages/InvalidPage.aspx")
                            SeatInfo0Checking = 0;
                        }

                        if (model2 == null == false)
                        {
                            Boolean IsInternationalFlight = objGeneral.IsInternationalFlight(model2.TemFlightDeparture, model2.TemFlightArrival, Request.PhysicalApplicationPath);
                            //Flight_Info pReturnFlightInfo = new Flight_Info();
                            //pReturnFlightInfo = Session["ReturnFlightInfo"];

                            if (IsInternationalFlight != true)
                            {
                                SeatInfo2Checking = 0;
                            }
                            else
                            {
                                if (Session["SeatInfo2"] == null == false)
                                {
                                    if (((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"]).Where(x => string.IsNullOrEmpty(x.SelectedSeat)).Count() > 0)
                                    {
                                        //lblErr.Text = "Please select the seat(s) for Return."
                                        //pnlErr.Visible = True

                                        //Session["ErrorMsg"] = lblErr.Text
                                        SeatInfo2Checking = SeatInfo2Checking + 1;
                                        //Response.Redirect("~/SeatConnected.aspx")
                                        //Exit Sub
                                    }
                                    if (SeatInfo2Checking == 0)
                                    {
                                        //if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"], model2, EnumFlightType.ReturnFlight))
                                        //{
                                        //    DeleteXML((string)Session["SeatInfo2Xml"]);
                                        //    Session["ReturnSeatInfo"] = Session["SeatInfo2"];
                                        //}
                                    }
                                }
                                else
                                {
                                    SeatInfo2Checking = SeatInfo2Checking + 1;
                                    // lblErr.Text = "Please select Return seat before proceed."
                                    // pnlErr.Visible = True

                                    // Session["ErrorMsg"] = lblErr.Text
                                    //Response.Redirect("~/SeatConnected.aspx")
                                    //Exit Sub
                                }
                            }

                        }
                        else
                        {
                            SeatInfo2Checking = 0;
                        }
                        Page.Validate("PrimaryMandatory");
                        if (Page.IsValid)
                        {
                            if (SeatInfo0Checking == 0 & SeatInfo2Checking == 0)
                            {
                                ClearSeatFeeValue();
                                if (SeatInfo0Checking == 0 && model != null)
                                {
                                    List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = null;
                                    if (Session["DepartExistingSeatInfo"] != null)
                                    {
                                        unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
                                    }

                                    using (profiler.Step("GBS:AssignSeat"))
                                    {
                                        if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"], unassignSeatinfo, model, EnumFlightType.DepartFlight))
                                        {
                                            DeleteXML((string)Session["SeatInfo0Xml"]);
                                            Session["DepartSeatInfo"] = Session["SeatInfo0"];
                                            //List<ABS.Logic.GroupBooking.SeatInfo> ListDepartSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartSeatInfo"];
                                            //List<ABS.Logic.GroupBooking.SeatInfo> ListDepartExistingSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
                                            //if (ListDepartSeatInfo != null && ListDepartExistingSeatInfo != null)
                                            //{
                                            //    HashSet<string> diffids = new HashSet<string>(ListDepartSeatInfo.Select(s => s.SelectedSeat));
                                            //    var results = ListDepartExistingSeatInfo.Where(m => !diffids.Contains(m.SelectedSeat)).ToList();
                                            //    if (results != null && results.Count > 0)
                                            //    {
                                            //        return msgList.Err1000000;
                                            //    }
                                            //}
                                        }
                                        else
                                        {
                                            return msgList.Err999997;
                                        }
                                    }
                                }

                                if (SeatInfo2Checking == 0 && model2 != null)
                                {
                                    List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = null;
                                    if (Session["ReturnExistingSeatInfo"] != null)
                                    {
                                        unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnExistingSeatInfo"];
                                    }

                                    using (profiler.Step("GBS:AssignSeat"))
                                    {
                                        if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"], unassignSeatinfo, model2, EnumFlightType.ReturnFlight))
                                        {
                                            DeleteXML((string)Session["SeatInfo2Xml"]);
                                            Session["ReturnSeatInfo"] = Session["SeatInfo2"];
                                            //List<ABS.Logic.GroupBooking.SeatInfo> ListReturnSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnSeatInfo"];
                                            //List<ABS.Logic.GroupBooking.SeatInfo> ListReturnExistingSeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnExistingSeatInfo"];
                                            //if (ListReturnSeatInfo != null && ListReturnExistingSeatInfo != null)
                                            //{
                                            //    HashSet<string> diffids = new HashSet<string>(ListReturnSeatInfo.Select(s => s.SelectedSeat));
                                            //    var results = ListReturnExistingSeatInfo.Where(m => !diffids.Contains(m.SelectedSeat)).ToList();
                                            //    if (results != null && results.Count > 0)
                                            //    {
                                            //        return msgList.Err1000000;
                                            //    }
                                            //}
                                        }
                                        else
                                        {
                                            return msgList.Err999997;
                                        }
                                    }
                                }



                                //end, update total amount
                                assignSeatDone = true;
                                //return "";
                                //Response.Redirect("~/pages/SeatSummary.aspx");
                            }
                            else
                            {
                                return msgList.Err999996;
                                //lblErr.Text = msgList.Err999996;
                                //pnlErr.Visible = true;
                                //Session["ErrorMsg"] = lblErr.Text;
                                //Response.Redirect("~/seats.aspx");
                            }
                        }


                        break;
                }

                if (assignSeatDone)
                {
                    //begin, update total amount
                    if (Session["TransID"] != null)
                    {
                        decimal numValue;
                        decimal TotalSeatDepart = 0;
                        decimal TotalSeatReturn = 0;

                        string TransID = (string)Session["TransID"];
                        //tyas'
                        //List<BookingTransactionDetail> listBookingDetail = new List<BookingTransactionDetail>();
                        //listBookingDetail = objBooking.GetAllBK_TRANSDTLFilterAll(TransID, 0);
                        //tyas
                        listBookingDetail = Session["listBookingDetail"] as List<BookingTransactionDetail>;


                        //if (ReturnID != "")
                        //{
                        //    int iIndexDepart = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 1 && p.Signature == SessionID);
                        //    int iIndexReturn = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 0 && p.Signature == SessionID);
                        //    if (iIndexDepart >= 0) listBookingDetail[iIndexDepart].LineSeat = TotalSeatDepart;
                        //    if (iIndexReturn >= 0) listBookingDetail[iIndexReturn].LineSeat = TotalSeatReturn;
                        //}
                        //else
                        //{
                        //    int iIndexDepart = listBookingDetail.FindIndex(p => p.Signature == SessionID);
                        //    if (iIndexDepart >= 0) listBookingDetail[iIndexDepart].LineSeat = TotalSeatDepart;
                        //}

                        //tyas
                        //int iIndexDepart = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 1);
                        //int iIndexReturn = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 0);
                        if (Session["DepartFlightSeatFees"] != null)
                        {
                            if (decimal.TryParse(Session["DepartFlightSeatFees"].ToString(), out numValue)) TotalSeatDepart = TotalSeatDepart + Convert.ToDecimal(Session["DepartFlightSeatFees"].ToString());
                        }
                        if (Session["DepartConnectingFlightSeatFees"] != null)
                        {
                            if (decimal.TryParse(Session["DepartConnectingFlightSeatFees"].ToString(), out numValue)) TotalSeatDepart = TotalSeatDepart + Convert.ToDecimal(Session["DepartConnectingFlightSeatFees"].ToString());
                        }
                        if (Session["DepartConnectingFlightSeatFees2"] != null)
                        {
                            if (decimal.TryParse(Session["DepartConnectingFlightSeatFees2"].ToString(), out numValue)) TotalSeatDepart = TotalSeatDepart + Convert.ToDecimal(Session["DepartConnectingFlightSeatFees2"].ToString());
                        }

                        if (Session["ReturnFlightSeatFees"] != null)
                        {
                            if (decimal.TryParse(Session["ReturnFlightSeatFees"].ToString(), out numValue)) TotalSeatReturn = TotalSeatReturn + Convert.ToDecimal(Session["ReturnFlightSeatFees"].ToString());
                        }
                        if (Session["ReturnConnectingFlightSeatFees"] != null)
                        {
                            if (decimal.TryParse(Session["ReturnConnectingFlightSeatFees"].ToString(), out numValue)) TotalSeatReturn = TotalSeatReturn + Convert.ToDecimal(Session["ReturnConnectingFlightSeatFees"].ToString());
                        }
                        if (Session["ReturnConnectingFlightSeatFees2"] != null)
                        {
                            if (decimal.TryParse(Session["ReturnConnectingFlightSeatFees2"].ToString(), out numValue)) TotalSeatReturn = TotalSeatReturn + Convert.ToDecimal(Session["ReturnConnectingFlightSeatFees2"].ToString());
                        }

                        decimal TotalAmountGoing = 0;
                        decimal TotalAmountReturn = 0;

                        if (Request.QueryString["change"] == null)
                        {
                            using (profiler.Step("GBS:UpdateTotalAmount"))
                            {
                                UpdateTotalAmount(TotalSeatDepart, TotalSeatReturn, ref TotalAmountGoing, ref TotalAmountReturn);
                            }
                        }

                        //if (iIndexDepart >= 0) listBookingDetail[iIndexDepart].LineSeat = TotalSeatDepart;
                        //if (iIndexReturn >= 0) listBookingDetail[iIndexReturn].LineSeat = TotalSeatReturn;
                        if (Request.QueryString["change"] != null)
                        {

                            //if (Session["ChgbookHDRInfo"] != null && Session["ChglstbookDTLInfo"] != null)
                            //{

                            //    UpdateAllBookingJourneyDetail();
                            //}
                        }
                        else
                        {
                            BookingTransactionMain bookingMain = new BookingTransactionMain();
                            bookingMain = objBooking.GetSingleBK_TRANSMAIN(TransID);
                            bookingMain.TransTotalSeat = TotalSeatDepart + TotalSeatReturn;
                            bookingMain.TotalAmtGoing = TotalAmountGoing;
                            bookingMain.TotalAmtReturn = TotalAmountReturn;
                            bookingMain.TransSubTotal = TotalAmountGoing + TotalAmountReturn;
                            bookingMain.TransTotalAmt = TotalAmountGoing + TotalAmountReturn;

                            //added by ketee, for conencting flight, total up the seat fees from the connecting seat fees columns
                            foreach (BookingTransactionDetail a in listBookingDetail)
                            {
                                if (a.LineConnectingSeat != null && a.LineConnectingSeat > 0 && a.LineConnectingSeat2 != null && a.LineConnectingSeat2 > 0)
                                {
                                    a.LineSeat = a.LineConnectingSeat + a.LineConnectingSeat2;
                                }
                            }
                            using (profiler.Step("GBS:UpdateTotalSeat"))
                            {
                                objBooking.UpdateTotalSeat(TransID, bookingMain, listBookingDetail);
                            }
                        }

                        if (Request.QueryString["change"] != null)
                        {

                            if (Session["ChgbookHDRInfo"] != null && Session["ChglstbookDTLInfo"] != null)
                            {
                                using (profiler.Step("GBS:UpdateAllBookingJourneyDetail"))
                                {
                                    UpdateAllBookingJourneyDetail();
                                }
                            }
                        }
                        return "";
                    }
                    else
                    {
                        return msgList.Err999995;
                    }
                }
                else
                {
                    return msgList.Err999997;
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                return ex.Message;
                //lblErr.Text = ex.ToString();
            }
        }

        public Boolean UpdateAllBookingJourneyDetail()
        {
            var profiler = MiniProfiler.Current;
            //ABS.Navitaire.APIBooking ApiBook = new ABS.Navitaire.APIBooking("");
            ABS.Navitaire.BookingManager.GetBookingResponse Response = new ABS.Navitaire.BookingManager.GetBookingResponse();
            ABS.Navitaire.APIBooking APIBooking = new ABS.Navitaire.APIBooking(Signature);
            try
            {
                string TransID = Session["TransID"].ToString();
                string PNR = "";
                DateTime expiredDate = DateTime.Now.AddDays(1);
                DateTime stdDate = DateTime.Now;

                string strOrigin = "";
                int goingreturn = 0;

                int totalPax = 0, Adt = 0, Chd = 0;
                decimal totalTransAmountAll = 0;
                decimal totalTransAmount = 0;
                decimal totalAmountGoing = 0;
                decimal totalAmountReturn = 0;
                decimal totalTransSubTotal = 0;
                decimal totalTransTotalFee = 0;
                decimal totalTransTotalTax = 0;
                decimal totalTransTotalPaxFee = 0;
                decimal totalTransTotalOth = 0;
                decimal totalTransTotalProcess = 0;
                decimal totalTransTotalSSR = 0;
                decimal totalTransTotalSeat = 0;
                decimal totalTransTotalNameChange = 0;
                decimal totalTransTotalInfant = 0;
                decimal totalTransTotalDisc = 0;
                decimal totalTransTotalPromoDisc = 0;

                BookingContainer BookingContainers = new BookingContainer(); //for booking details
                List<BookingJourneyContainer> listBookingJourneyContainers = new List<BookingJourneyContainer>(); //for journey list
                List<PaymentContainer> listPaymentContainers = new List<PaymentContainer>(); //for payment list
                List<PaymentContainer> lstPaymentContainer = new List<PaymentContainer>();

                List<BookingTransactionDetail> listBookingJourney = new List<BookingTransactionDetail>();
                List<BookingTransTender> listBookTransTenderInfo = new List<BookingTransTender>();
                List<PassengerContainer> lstPassengerContainer = new List<PassengerContainer>();
                PassengerInfantContainer objPassengerInfantModel = new PassengerInfantContainer();
                List<PassengerInfantContainer> lstPassengerInfantModel = new List<PassengerInfantContainer>();
                BookingTransactionDetail lstBooking = new BookingTransactionDetail();

                BookingTransactionDetail bookDTLInfo = new BookingTransactionDetail();
                BookingTransactionMain bookHDRInfo = new BookingTransactionMain();
                BookingTransTender bookTransTenderInfo = new BookingTransTender();

                BookingJourneyContainer objBookingJourneyContainer = new BookingJourneyContainer();
                List<BookingTransactionDetail> lstbookDTLInfoPrev = new List<BookingTransactionDetail>();
                List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
                List<BookingTransactionDetail> listDetailCombinePNR = new List<BookingTransactionDetail>();
                List<BookingJourneyContainer> lstBookingJourneyContainer = new List<BookingJourneyContainer>();
                bookHDRInfo = new BookingTransactionMain();

                PassengerContainer objPassengerContainer = new PassengerContainer();

                bookHDRInfo = (BookingTransactionMain)Session["ChgbookHDRInfo"];
                listDetailCombinePNR = (List<BookingTransactionDetail>)Session["listBookingDetailCombine"];

                if (HttpContext.Current.Session["ChgbookHDRInfo"] != null && HttpContext.Current.Session["TransDetail"] != null)
                {
                    //DataTable dtTransMain = objBooking.dtTransMain();
                    if (HttpContext.Current.Session["ChgbookHDRInfo"] != null)
                        bookHDRInfo = (BookingTransactionMain)HttpContext.Current.Session["ChgbookHDRInfo"];//insert transmain into datatable

                    DataTable dtTransDetail = objBooking.dtTransDetail();
                    if (HttpContext.Current.Session["TransDetailAll"] != null)
                        dtTransDetail = (DataTable)HttpContext.Current.Session["TransDetailAll"];//insert transdtlcombinePNR into datatable

                    //if (HttpContext.Current.Session["ChglstbookDTLInfo"] != null)
                    //    lstbookDTLInfo = (List<BookingTransactionDetail>)HttpContext.Current.Session["ChglstbookDTLInfo"];//insert transdtlcombinePNR into datatable


                    List<BookingTransactionDetail> listBookingDetail = new List<BookingTransactionDetail>();
                    if (Session["ChglstbookDTLInfo"] != null)
                    {
                        listBookingDetail = (List<BookingTransactionDetail>)Session["ChglstbookDTLInfo"];
                    }
                    //List<BookingTransTender> listTransTender = new List<BookingTransTender>();
                    //listTransTender = GetAllBK_TRANSTENDERFilter(TransID);//grab tender list

                    //lstbookDTLInfo = new List<BookingTransactionDetail>();

                    int rowBookingJourneySeqNo = 1;
                    int cnt = 0;
                    int z = 0;


                    PNR = dtTransDetail.Rows[0]["RecordLocator"].ToString();
                    if (PNR.Trim().Length < 6)//will not continue if PNR is not valid
                    {
                        rowBookingJourneySeqNo += Convert.ToInt16(dtTransDetail.Rows[0]["CntRec"].ToString());
                    }

                    ABS.Navitaire.BookingManager.Booking book = new ABS.Navitaire.BookingManager.Booking();// APIBooking.GetBookingFromState(Signature);
                    using (profiler.Step("Navitaire:GetBookingFromState"))
                    {
                        book = APIBooking.GetBookingFromState(Signature);
                    }


                    //if (Response.Booking.Journeys[0].Segments[0].PaxSSRs 
                    #region "Load Passenger Fee"
                    //retrieve arrival, departure
                    string Departure = "", Arrival = "", Transit = "";
                    for (int j = 0; j < book.Journeys.Length; j++)
                    {
                        for (int k = 0; k < book.Journeys[j].Segments.Length; k++)
                        {
                            switch (j)
                            {
                                case 0:
                                    if (k == 0)
                                    {
                                        Departure = book.Journeys[j].Segments[k].DepartureStation;
                                        Arrival = book.Journeys[j].Segments[k].ArrivalStation;
                                    }
                                    else if (k == 1)
                                    {
                                        Transit = book.Journeys[j].Segments[k].DepartureStation;
                                        Arrival = book.Journeys[j].Segments[k].ArrivalStation;
                                    }
                                    break;
                                case 1:
                                    if (k == 0)
                                    {
                                        Arrival = book.Journeys[j].Segments[k].DepartureStation;
                                    }
                                    else if (k == 1)
                                    {

                                    }
                                    break;
                                default:
                                    break;

                            }
                        }
                    }

                    #endregion
                    foreach (Passenger itemPassenger in book.Passengers)
                    {
                        if (itemPassenger.PassengerTypeInfos[0].PaxType == "ADT")
                        {
                            objBookingJourneyContainer.AdtPax++;
                        }
                        else
                        {
                            objBookingJourneyContainer.ChdPax++;
                        }
                    }
                    totalPax = objBookingJourneyContainer.AdtPax + objBookingJourneyContainer.ChdPax;
                    Adt = objBookingJourneyContainer.AdtPax;
                    Chd = objBookingJourneyContainer.ChdPax;

                    ArrayList save = (ArrayList)Session["Chgsave"];
                    lstbookDTLInfoPrev = objBooking.GetAllBK_TRANSDTLFilterByPNR(save[1].ToString(), PNR);

                    decimal SSRFee1 = 0, SSRFee2 = 0, SeatFee1 = 0, SeatFee2 = 0, InfantFee1 = 0, InfantFee2 = 0;
                    objBookingJourneyContainer.AdtDiscChrg = 0;
                    objBookingJourneyContainer.AdtPromoDiscChrg = 0;
                    objBookingJourneyContainer.ChdDiscChrg = 0;
                    objBookingJourneyContainer.ChdPromoDiscChrg = 0;

                    int inft = 0;
                    foreach (Passenger itemPassenger in book.Passengers)
                    {

                        foreach (PassengerFee itemPassengerFee in itemPassenger.PassengerFees)
                        {
                            if (itemPassengerFee.SSRCode == "INFT")
                            {
                                inft = 1;
                            }
                            else
                            {
                                inft = 0;
                            }
                            foreach (BookingServiceCharge itemServiceCharge in itemPassengerFee.ServiceCharges)
                            {
                                string feeCode = itemServiceCharge.ChargeCode;
                                decimal feeAmount = itemServiceCharge.Amount;
                                string feeChargeType = itemServiceCharge.ChargeType.ToString();

                                if (feeChargeType.ToUpper() == "DISCOUNT")
                                {
                                    if (itemPassenger.PassengerTypeInfos[0].PaxType == "ADT")
                                    {
                                        objBookingJourneyContainer.AdtDiscChrg -= feeAmount;
                                    }
                                    else
                                    {
                                        objBookingJourneyContainer.ChdDiscChrg -= feeAmount;
                                    }
                                }
                                else if (feeChargeType.ToUpper() == "PROMOTIONDISCOUNT")
                                {
                                    if (itemPassenger.PassengerTypeInfos[0].PaxType == "ADT")
                                    {
                                        objBookingJourneyContainer.AdtPromoDiscChrg -= feeAmount;
                                    }
                                    else
                                    {
                                        objBookingJourneyContainer.ChdPromoDiscChrg -= feeAmount;
                                    }
                                }
                                else if (itemPassengerFee.FeeType == FeeType.SSRFee)
                                {
                                    if (itemPassengerFee.FlightReference != "")
                                    {
                                        if (itemPassengerFee.FlightReference.Substring(16, 3) == Departure || itemPassengerFee.FlightReference.Substring(16, 6) == Departure + Transit || itemPassengerFee.FlightReference.Substring(16, 6) == Transit + Arrival)
                                        {
                                            if (inft == 1)
                                                InfantFee1 += feeAmount;
                                            else
                                                SSRFee1 += feeAmount;
                                        }
                                        else if (itemPassengerFee.FlightReference.Substring(16, 3) == Arrival || itemPassengerFee.FlightReference.Substring(16, 6) == Transit + Departure || itemPassengerFee.FlightReference.Substring(16, 6) == Arrival + Transit)
                                        {
                                            if (inft == 1)
                                                InfantFee2 += feeAmount;
                                            else
                                                SSRFee2 += feeAmount;
                                        }
                                    }
                                }
                                else if (itemPassengerFee.FeeType == FeeType.SeatFee)
                                {
                                    if (itemPassengerFee.FlightReference != "")
                                    {
                                        if (itemPassengerFee.FlightReference.Substring(16, 3) == Departure || itemPassengerFee.FlightReference.Substring(16, 6) == Departure + Transit || itemPassengerFee.FlightReference.Substring(16, 6) == Transit + Arrival)
                                            SeatFee1 += feeAmount;
                                        else if (itemPassengerFee.FlightReference.Substring(16, 3) == Arrival || itemPassengerFee.FlightReference.Substring(16, 6) == Transit + Departure || itemPassengerFee.FlightReference.Substring(16, 6) == Arrival + Transit)
                                            SeatFee2 += feeAmount;
                                    }
                                }
                                else if (itemPassengerFee.FeeType == FeeType.PaymentFee)
                                {
                                    objBookingJourneyContainer.ProcessFee += feeAmount;
                                }
                                else if (itemPassengerFee.FeeCode == "NCF")
                                {
                                    objBookingJourneyContainer.NameChangeChrg += feeAmount;
                                }
                                else
                                {
                                    switch (feeCode)
                                    {
                                        case "SVCF":
                                            objBookingJourneyContainer.ServiceFee -= feeAmount;
                                            break;
                                        case "VAT":
                                            objBookingJourneyContainer.VATFee += feeAmount;
                                            break;
                                        case "CHG":
                                            objBookingJourneyContainer.ChargeFee += feeAmount;
                                            break;
                                        case "SPL":
                                            objBookingJourneyContainer.SPLFee += feeAmount;
                                            break;
                                        default:
                                            objBookingJourneyContainer.OtherFee += feeAmount;
                                            break;
                                    }
                                }
                            }
                        }
                    }

                    strOrigin = "";
                    totalAmountGoing = 0;
                    totalAmountReturn = 0;


                    foreach (BookingTransactionDetail bookDTLInfos in listBookingDetail)
                    {

                        bookDTLInfos.LineOth = bookDTLInfos.LineOth + objBookingJourneyContainer.OtherFee + objBookingJourneyContainer.SPLFee;
                        bookDTLInfos.LineDisc = ((objBookingJourneyContainer.AdtDiscChrg * objBookingJourneyContainer.AdtPax) + (objBookingJourneyContainer.ChdDiscChrg * objBookingJourneyContainer.ChdPax)) + bookDTLInfos.LineDisc;
                        bookDTLInfos.LinePromoDisc = ((objBookingJourneyContainer.AdtPromoDiscChrg * objBookingJourneyContainer.AdtPax) + (objBookingJourneyContainer.ChdPromoDiscChrg * objBookingJourneyContainer.ChdPax)) + bookDTLInfos.LinePromoDisc;
                        bookDTLInfos.LineNameChange = objBookingJourneyContainer.NameChangeChrg;
                        bookDTLInfos.PaxAdult = objBookingJourneyContainer.AdtPax;
                        bookDTLInfos.PaxChild = objBookingJourneyContainer.ChdPax;
                        bookDTLInfos.LineFee = objBookingJourneyContainer.ServiceFee;
                        bookDTLInfos.LineVAT = objBookingJourneyContainer.VATFee;
                        bookDTLInfos.LineCharge = objBookingJourneyContainer.ChargeFee;
                        //bookDTLInfos.LineOth = objBookingJourneyContainer.OtherFee + objBookingJourneyContainer.SPLFee;
                        bookDTLInfos.LineProcess = objBookingJourneyContainer.ProcessFee;
                        if (strOrigin == "") strOrigin = bookDTLInfos.Origin;
                        if (bookDTLInfos.Origin == strOrigin) goingreturn = 0;
                        else goingreturn = 1;
                        if (goingreturn == 0)
                        {
                            bookDTLInfos.LineSSR = SSRFee1;
                            bookDTLInfos.LineSeat = SeatFee1;
                            bookDTLInfos.LineInfant = InfantFee1;
                        }
                        else
                        {
                            bookDTLInfos.LineSSR = SSRFee2;
                            bookDTLInfos.LineSeat = SeatFee2;
                            bookDTLInfos.LineInfant = InfantFee2;
                        }


                        bookDTLInfos.Currency = objBookingJourneyContainer.CurrencyCode;
                        bookDTLInfos.CommandType = "update";
                        bookDTLInfos.NextDueDate = DateTime.Now.AddMinutes(10);
                        bookDTLInfos.AttemptCount = 0;
                        bookDTLInfos.TransVoid = 0;
                        if (strOrigin == "") strOrigin = bookDTLInfos.Origin;
                        if (bookDTLInfos.Origin == strOrigin) goingreturn = 0;
                        else goingreturn = 1;
                        if (goingreturn == 0)
                        {
                            bookDTLInfos.LineTotal = (bookDTLInfos.FarePerPax * (bookDTLInfos.PaxAdult + bookDTLInfos.PaxChild)) + bookDTLInfos.LineTax + bookDTLInfos.LinePaxFee + bookDTLInfos.LineOth + bookDTLInfos.LineProcess + bookDTLInfos.LineDisc + bookDTLInfos.LinePromoDisc + bookDTLInfos.LineFee + bookDTLInfos.LineVAT + bookDTLInfos.LineCharge + bookDTLInfos.LineSSR + bookDTLInfos.LineSeat + bookDTLInfos.LineNameChange + bookDTLInfos.LineInfant;
                            totalAmountGoing += bookDTLInfos.LineTotal;

                        }
                        else
                        {
                            bookDTLInfos.LineTotal = (bookDTLInfos.FarePerPax * (bookDTLInfos.PaxAdult + bookDTLInfos.PaxChild)) + bookDTLInfos.LineTax + bookDTLInfos.LinePaxFee + bookDTLInfos.LineOth + bookDTLInfos.LineSSR + bookDTLInfos.LineSeat + bookDTLInfos.LineInfant;
                            totalAmountReturn += bookDTLInfos.LineTotal;

                        }
                    }

                    if (listBookingDetail.Count > 0)
                    {

                        for (int i = cnt; i < listBookingDetail.Count; i++)
                        {
                            dtTransDetail.Rows[i]["PaxAdult"] = 0;
                            dtTransDetail.Rows[i]["PaxChild"] = 0;
                            dtTransDetail.Rows[i]["LineTotal"] = 0;
                            dtTransDetail.Rows[i]["LineFee"] = 0;
                            dtTransDetail.Rows[i]["LineOth"] = 0;
                            dtTransDetail.Rows[i]["LineProcess"] = 0;
                            dtTransDetail.Rows[i]["LineSSR"] = 0;
                            dtTransDetail.Rows[i]["LineSeat"] = 0;
                            dtTransDetail.Rows[i]["LineNameChange"] = 0;
                            dtTransDetail.Rows[i]["LineInfant"] = 0;
                            dtTransDetail.Rows[i]["LineDisc"] = 0;
                            dtTransDetail.Rows[i]["LinePromoDisc"] = 0;
                            dtTransDetail.Rows[i]["LineTax"] = 0;
                            dtTransDetail.Rows[i]["LinePaxFee"] = 0;
                            BookingTransactionDetail pBookingTransDetail = new BookingTransactionDetail();
                            pBookingTransDetail = listBookingDetail[i];

                            dtTransDetail.Rows[i]["PaxAdult"] = Convert.ToInt16(dtTransDetail.Rows[i]["PaxAdult"].ToString()) + Convert.ToInt16(pBookingTransDetail.PaxAdult.ToString());
                            dtTransDetail.Rows[i]["PaxChild"] = Convert.ToInt16(dtTransDetail.Rows[i]["PaxChild"].ToString()) + Convert.ToInt16(pBookingTransDetail.PaxChild.ToString());
                            dtTransDetail.Rows[i]["LineTotal"] = Convert.ToDecimal(dtTransDetail.Rows[i]["LineTotal"].ToString()) + Convert.ToDecimal(pBookingTransDetail.LineTotal.ToString());
                            dtTransDetail.Rows[i]["LineFee"] = Convert.ToDecimal(dtTransDetail.Rows[i]["LineFee"].ToString()) + Convert.ToDecimal(pBookingTransDetail.LineFee.ToString());
                            dtTransDetail.Rows[i]["LineOth"] = Convert.ToDecimal(dtTransDetail.Rows[i]["LineOth"].ToString()) + Convert.ToDecimal(pBookingTransDetail.LineOth.ToString());
                            dtTransDetail.Rows[i]["LineProcess"] = Convert.ToDecimal(dtTransDetail.Rows[i]["LineProcess"].ToString()) + Convert.ToDecimal(pBookingTransDetail.LineProcess.ToString());
                            dtTransDetail.Rows[i]["LineSSR"] = Convert.ToDecimal(dtTransDetail.Rows[i]["LineSSR"].ToString()) + Convert.ToDecimal(pBookingTransDetail.LineSSR.ToString());
                            dtTransDetail.Rows[i]["LineSeat"] = Convert.ToDecimal(dtTransDetail.Rows[i]["LineSeat"].ToString()) + Convert.ToDecimal(pBookingTransDetail.LineSeat.ToString());
                            dtTransDetail.Rows[i]["LineNameChange"] = Convert.ToDecimal(dtTransDetail.Rows[i]["LineNameChange"].ToString()) + Convert.ToDecimal(pBookingTransDetail.LineNameChange.ToString());
                            dtTransDetail.Rows[i]["LineInfant"] = Convert.ToDecimal(dtTransDetail.Rows[i]["LineInfant"].ToString()) + Convert.ToDecimal(pBookingTransDetail.LineInfant.ToString());
                            dtTransDetail.Rows[i]["LineDisc"] = Convert.ToDecimal(dtTransDetail.Rows[i]["LineDisc"].ToString()) + Convert.ToDecimal(pBookingTransDetail.LineDisc.ToString());
                            dtTransDetail.Rows[i]["LinePromoDisc"] = Convert.ToDecimal(dtTransDetail.Rows[i]["LinePromoDisc"].ToString()) + Convert.ToDecimal(pBookingTransDetail.LinePromoDisc.ToString());
                            dtTransDetail.Rows[i]["LineTax"] = Convert.ToDecimal(dtTransDetail.Rows[i]["LineTax"].ToString()) + Convert.ToDecimal(pBookingTransDetail.LineTax.ToString());
                            dtTransDetail.Rows[i]["LinePaxFee"] = Convert.ToDecimal(dtTransDetail.Rows[i]["LinePaxFee"].ToString()) + Convert.ToDecimal(pBookingTransDetail.LinePaxFee.ToString());

                            totalPax = Convert.ToInt16(dtTransDetail.Rows[i]["PaxAdult"].ToString()) + Convert.ToInt16(dtTransDetail.Rows[i]["PaxChild"].ToString());
                            totalTransAmount += Convert.ToDecimal(dtTransDetail.Rows[i]["LineTotal"].ToString());
                            totalTransTotalFee += Convert.ToDecimal(dtTransDetail.Rows[i]["LineFee"].ToString());
                            totalTransTotalOth += Convert.ToDecimal(dtTransDetail.Rows[i]["LineOth"].ToString());
                            totalTransTotalProcess += Convert.ToDecimal(dtTransDetail.Rows[i]["LineProcess"].ToString());
                            totalTransTotalSSR += Convert.ToDecimal(dtTransDetail.Rows[i]["LineSSR"].ToString());
                            totalTransTotalSeat += Convert.ToDecimal(dtTransDetail.Rows[i]["LineSeat"].ToString());
                            totalTransTotalNameChange += Convert.ToDecimal(dtTransDetail.Rows[i]["LineNameChange"].ToString());
                            totalTransTotalInfant += Convert.ToDecimal(dtTransDetail.Rows[i]["LineInfant"].ToString());
                            totalTransTotalDisc += Convert.ToDecimal(dtTransDetail.Rows[i]["LineDisc"].ToString());
                            totalTransTotalPromoDisc += Convert.ToDecimal(dtTransDetail.Rows[i]["LinePromoDisc"].ToString());
                            totalTransTotalTax += Convert.ToDecimal(dtTransDetail.Rows[i]["LineTax"].ToString());
                            totalTransTotalPaxFee += Convert.ToDecimal(dtTransDetail.Rows[i]["LinePaxFee"].ToString());
                            totalTransSubTotal += Convert.ToDecimal(dtTransDetail.Rows[i]["LineTotal"].ToString());

                            totalTransAmountAll += Convert.ToDecimal(dtTransDetail.Rows[i]["LineTotal"].ToString());
                            cnt = i;
                        }
                        HttpContext.Current.Session["TransDetailAll"] = dtTransDetail;
                    }


                    //end recalculate pax details and insert into dtTransDetail


                    #endregion

                    #region "Payment"


                    //bookHDRInfo = GetSingleBK_TRANSMAIN(transID, TransType, agentID, agentCatgID, bookingDate);
                    if (totalPax > 0)
                        bookHDRInfo.TransTotalPAX = totalPax;
                    if (totalTransAmount > 0)
                        bookHDRInfo.TransTotalAmt = totalTransAmount;
                    if (totalTransTotalFee > 0)
                        bookHDRInfo.TransTotalFee = totalTransTotalFee;
                    if (totalTransTotalTax > 0)
                        bookHDRInfo.TransTotalTax = totalTransTotalTax;
                    if (totalTransTotalPaxFee > 0)
                        bookHDRInfo.TransTotalPaxFee = totalTransTotalPaxFee;
                    if (totalTransTotalOth > 0)
                        bookHDRInfo.TransTotalOth = totalTransTotalOth;
                    if (totalTransTotalSSR > 0)
                        bookHDRInfo.TransTotalSSR = totalTransTotalSSR;
                    if (totalTransTotalSeat > 0)
                        bookHDRInfo.TransTotalSeat = totalTransTotalSeat;
                    if (totalTransTotalNameChange > 0)
                        bookHDRInfo.TransTotalNameChange = totalTransTotalNameChange;
                    if (totalTransTotalProcess > 0)
                        bookHDRInfo.TransTotalProcess = totalTransTotalProcess;
                    if (totalTransTotalInfant > 0)
                        bookHDRInfo.TransTotalInfant = totalTransTotalInfant;
                    if (totalTransTotalDisc > 0)
                        bookHDRInfo.TransTotalDisc = totalTransTotalDisc;
                    if (totalTransTotalPromoDisc > 0)
                        bookHDRInfo.TransTotalPromoDisc = totalTransTotalPromoDisc;
                    if (totalTransSubTotal > 0)
                        bookHDRInfo.TransSubTotal = totalTransSubTotal;
                    if (totalTransAmount > 0 && totalPax > 0)
                        bookHDRInfo.TotalAmtAVG = Convert.ToDecimal((Math.Round(totalTransAmount / totalPax, 2)).ToString());
                    if (totalAmountGoing > 0)
                        bookHDRInfo.TotalAmtGoing = totalAmountGoing;
                    if (totalAmountReturn > 0)
                        bookHDRInfo.TotalAmtReturn = totalAmountReturn;
                    bookHDRInfo.CollectedAmt = bookHDRInfo.TransSubTotal - book.BookingSum.BalanceDue;
                    bookHDRInfo.TransStatus = bookHDRInfo.TransStatus;

                    if (bookHDRInfo.TransTotalAmt > bookHDRInfo.CollectedAmt) //amended by diana 20140104 - only if amount > collected
                        bookHDRInfo.NeedPaymentStatus = 1;
                    else
                        bookHDRInfo.NeedPaymentStatus = 0;
                    bookHDRInfo.SyncLastUpd = DateTime.Now;
                    bookHDRInfo.LastSyncBy = model.TemFlightAgentName;
                    bookHDRInfo.ReminderType = 2;

                    //HttpContext.Current.Session.Remove("ChgbookHDRInfo");
                    //HttpContext.Current.Session.Add("ChgbookHDRInfo", bookHDRInfo);

                    List<BookingTransactionDetail> listBookingDetailCombine = new List<BookingTransactionDetail>();
                    listBookingDetailCombine = (List<BookingTransactionDetail>)Session["listBookingDetailCombine"];
                    foreach (BookingTransactionDetail objBK_TRANSDTL_Info in listBookingDetailCombine)
                    {
                        objBK_TRANSDTL_Info.RecordLocator = listBookingDetail.Min(item => item.RecordLocator);
                        objBK_TRANSDTL_Info.CarrierCode = listBookingDetail.Min(item => item.CarrierCode);
                        objBK_TRANSDTL_Info.FlightNo = listBookingDetail.Min(item => item.FlightNo);
                        objBK_TRANSDTL_Info.DepatureDate = listBookingDetail.Min(item => item.DepatureDate);
                        objBK_TRANSDTL_Info.Origin = listBookingDetail.Min(item => item.Origin);
                        objBK_TRANSDTL_Info.Destination = listBookingDetail.Min(item => item.Destination);
                        objBK_TRANSDTL_Info.Currency = listBookingDetail.Min(item => item.Currency);
                        objBK_TRANSDTL_Info.ArrivalDate = listBookingDetail.Min(item => item.ArrivalDate);
                        objBK_TRANSDTL_Info.Transit = listBookingDetail.Min(item => item.Transit);
                        objBK_TRANSDTL_Info.DepatureDate2 = listBookingDetail.Min(item => item.DepatureDate2);
                        objBK_TRANSDTL_Info.ArrivalDate2 = listBookingDetail.Min(item => item.ArrivalDate2);
                        objBK_TRANSDTL_Info.SellKey = listBookingDetail.Min(item => item.SellKey);
                        objBK_TRANSDTL_Info.Signature = listBookingDetail[0].Signature;
                        objBK_TRANSDTL_Info.PaxChild = listBookingDetail.Max(item => item.PaxChild);
                        objBK_TRANSDTL_Info.PaxAdult = listBookingDetail.Max(item => item.PaxAdult);
                        objBK_TRANSDTL_Info.FarePerPax = listBookingDetail.Sum(item => item.FarePerPax);
                        objBK_TRANSDTL_Info.LineTotal = listBookingDetail.Sum(item => item.LineTotal);
                        objBK_TRANSDTL_Info.LineTax = listBookingDetail.Sum(item => item.LineTax);
                        objBK_TRANSDTL_Info.LinePaxFee = listBookingDetail.Sum(item => item.LinePaxFee);
                        objBK_TRANSDTL_Info.LineFee = listBookingDetail.Sum(item => item.LineFee);
                        objBK_TRANSDTL_Info.LineOth = listBookingDetail.Sum(item => item.LineOth);
                        objBK_TRANSDTL_Info.LineProcess = listBookingDetail.Sum(item => item.LineProcess);
                        objBK_TRANSDTL_Info.LineSSR = listBookingDetail.Sum(item => item.LineSSR);
                        objBK_TRANSDTL_Info.LineSeat = listBookingDetail.Sum(item => item.LineSeat);
                        objBK_TRANSDTL_Info.LineNameChange = listBookingDetail.Sum(item => item.LineNameChange);
                        objBK_TRANSDTL_Info.LineInfant = listBookingDetail.Sum(item => item.LineInfant);
                        objBK_TRANSDTL_Info.LineDisc = listBookingDetail.Sum(item => item.LineDisc);
                        objBK_TRANSDTL_Info.LinePromoDisc = listBookingDetail.Sum(item => item.LinePromoDisc);
                        objBK_TRANSDTL_Info.NextDueDate = DateTime.Now.AddMinutes(10);
                        objBK_TRANSDTL_Info.CollectedAmount = objBK_TRANSDTL_Info.LineTotal - book.BookingSum.BalanceDue;
                        objBK_TRANSDTL_Info.PayDueAmount1 = listBookingDetail.Min(item => item.PayDueAmount1);
                        objBK_TRANSDTL_Info.PayDueAmount2 = listBookingDetail.Min(item => item.PayDueAmount2);
                        objBK_TRANSDTL_Info.PayDueAmount3 = listBookingDetail.Min(item => item.PayDueAmount3);
                        objBK_TRANSDTL_Info.PayDueDate1 = listBookingDetail.Min(item => item.PayDueDate1);
                        objBK_TRANSDTL_Info.PayDueDate2 = listBookingDetail.Min(item => item.PayDueDate2);
                        objBK_TRANSDTL_Info.PayDueDate3 = listBookingDetail.Min(item => item.PayDueDate3);
                        HttpContext.Current.Session.Remove("BookingDetailCombine");
                        HttpContext.Current.Session.Add("BookingDetailCombine", objBK_TRANSDTL_Info);
                    }

                    bookHDRInfo.PaymentAmtEx1 = listBookingDetail.Min(item => item.PayDueAmount1);
                    bookHDRInfo.PaymentAmtEx2 = listBookingDetail.Min(item => item.PayDueAmount2);
                    bookHDRInfo.PaymentAmtEx3 = listBookingDetail.Min(item => item.PayDueAmount3);
                    //bookHDRInfo.PaymentAmtEx3 = (listBookingDetail.Min(item => item.PayDueAmount3) + book.BookingSum.BalanceDue);
                    bookHDRInfo.PaymentDateEx1 = listBookingDetail.Min(item => item.PayDueDate1);
                    bookHDRInfo.PaymentDateEx2 = listBookingDetail.Min(item => item.PayDueDate2);
                    bookHDRInfo.PaymentDateEx3 = listBookingDetail.Min(item => item.PayDueDate3);



                    HttpContext.Current.Session.Remove("ChglstbookDTLInfo");
                    HttpContext.Current.Session.Add("ChglstbookDTLInfo", listBookingDetail);

                    HttpContext.Current.Session.Remove("listBookingDetailCombine");
                    HttpContext.Current.Session.Add("listBookingDetailCombine", listBookingDetailCombine);


                    objBooking.FillDataTableTransDetail(listBookingDetailCombine);
                    objBooking.FillChgTransDetail(listBookingDetailCombine);

                    HttpContext.Current.Session.Remove("ChgbookHDRInfo");
                    HttpContext.Current.Session.Add("ChgbookHDRInfo", bookHDRInfo);
                    objBooking.FillChgTransMain(bookHDRInfo);

                    //objBooking.FillDataTableTransMain(bookingMain);

                }

                return true;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                return false;
            }
            finally
            {

            }
        }

        private bool AssignSeat(List<ABS.Logic.GroupBooking.SeatInfo> _seatInfo, List<ABS.Logic.GroupBooking.SeatInfo> _usassignSeatInfo,
            BookingControl.TemFlight model, EnumFlightType pFlightType)
        {
            var profiler = MiniProfiler.Current;
            BookingControl bookingControl = new BookingControl();
            //string Signature = (string)Session["signature"];

            DataTable dataClass = (DataTable)HttpContext.Current.Session["dataClassTrans"];

            DataTable dtPassenger = default(DataTable);
            List<ABS.Logic.GroupBooking.SeatInfo> existingSeatInfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
            ABS.Logic.GroupBooking.SeatInfo seatInfo;
            using (profiler.Step("GBS:GetAllBK_PASSENGERLISTInitDataTable"))
            {
                dtPassenger = objBooking.GetAllBK_PASSENGERLISTInitDataTable(TransId, true);
            }
            if (dtPassenger == null)
            {
                if (Session["Chgsave"] != null)
                {
                    ArrayList save = (ArrayList)Session["Chgsave"];
                    DataTable dtTransDetail = objBooking.dtTransDetail();
                    if (HttpContext.Current.Session["TransDetailAll"] != null)
                        dtTransDetail = (DataTable)HttpContext.Current.Session["TransDetailAll"];
                    DataTable dtPassClone = new DataTable();
                    dtPassClone = objBooking.GetAllBK_PASSENGERLISTInitDataTable(save[1].ToString(), true);
                    dtPassenger = dtPassClone.Clone();
                    int x = 0;
                    for (int ii = 0; ii < dtPassClone.Rows.Count; ii++)
                    {
                        if (dtPassClone.Rows[ii]["PNR"].ToString() == dtTransDetail.Rows[0]["RecordLocator"].ToString())
                        {

                            dtPassenger.ImportRow(dtPassClone.Rows[ii]);
                            dtPassenger.Rows[x]["TransID"] = save[0].ToString();
                            x += 1;
                        }
                    }
                }
            }
            decimal SeatFee = 0;

            if (Session["listBookingDetail"] == null)
            {
                if (Request.QueryString["change"] != null)
                {
                    if (Session["ChglstbookDTLInfo"] != null)
                    {
                        listBookingDetail = (List<BookingTransactionDetail>)Session["ChglstbookDTLInfo"];
                    }
                }
                else
                {
                    using (profiler.Step("GBS:GetAllBK_TRANSDTLFilterAll"))
                    {
                        listBookingDetail = objBooking.GetAllBK_TRANSDTLFilterAll(TransId, 0);
                    }
                }

            }
            else
            {
                if (Request.QueryString["change"] != null)
                {
                    if (Session["ChglstbookDTLInfo"] != null)
                    {
                        listBookingDetail = (List<BookingTransactionDetail>)Session["ChglstbookDTLInfo"];
                    }
                }
                else
                {
                    listBookingDetail = Session["listBookingDetail"] as List<BookingTransactionDetail>;
                }
            }

            if (dataClass != null)
            {
                for (int j = 0; j < dataClass.Rows.Count; j++)
                {
                    SeatFee = 0;
                    if (dataClass.Rows[j]["SellSignature"] != null)
                    {
                        Signature = dataClass.Rows[j]["SellSignature"].ToString();
                        BookingTransactionDetail BookingDetail = new BookingTransactionDetail();
                        if (Request.QueryString["change"] != null)
                        {
                            if (Session["BookingDetailCombine"] != null)
                            {
                                BookingDetail = (BookingTransactionDetail)Session["BookingDetailCombine"];
                            }
                        }
                        else
                        {
                            using (profiler.Step("GBS:GetBK_TRANSDTLFlightBySellKey"))
                            {
                                BookingDetail = objBooking.GetBK_TRANSDTLFlightBySellKey(Signature, 0);
                            }
                        }

                        if (BookingDetail == null)
                        {
                            return false;
                        }

                        try
                        {
                            //Dim pDepartFlightInfo As AirAsia.Base.Flight_Info
                            //Dim pReturnFlightInfo As AirAsia.Base.Flight_Info
                            BookingUpdateResponseData UnassignResponse = new BookingUpdateResponseData();
                            BookingUpdateResponseData AssignResponse = new BookingUpdateResponseData();
                            //pDepartFlightInfo = Session["DepartFlightInfo"]
                            //pReturnFlightInfo = Session["ReturnFlightInfo"]

                            //Assign New Seat
                            if (_seatInfo == null)
                            {
                                continue;
                            }

                            int selectedSeatRow = _seatInfo.Count(a => a.SelectedSeat.Trim() != "" && a.RecordLocator.Trim() == BookingDetail.RecordLocator.Trim());

                            if (selectedSeatRow <= 0)
                            {
                                continue;
                            }

                            int SeatInfoCount1 = selectedSeatRow - 1;
                            //Dim SeatInfoCount1 As Integer = Session["SeatInfo0"].count - 1
                            //Dim SeatInfoCount2 As Integer = Session["SeatInfo2"].count - 1
                            int i = 0;
                            int[] aPassengerNumber = new int[SeatInfoCount1 + 1];
                            int[] aPassengerID = new int[SeatInfoCount1 + 1];
                            string[] aUnitDisignator = new string[SeatInfoCount1 + 1];
                            string[] acompartmentDesignator = new string[SeatInfoCount1 + 1];
                            string[] aPNR = new string[SeatInfoCount1 + 1];

                            ABS.Logic.GroupBooking.SeatInfo seatInfoWithAmount = new ABS.Logic.GroupBooking.SeatInfo();
                            //Dim bPassengerNumber(SeatInfoCount2) As Integer
                            //Dim bPassengerID(SeatInfoCount2) As Integer
                            //Dim bUnitDisignator(SeatInfoCount2) As String
                            //Dim bcompartmentDesignator(SeatInfoCount2) As String
                            foreach (ABS.Logic.GroupBooking.SeatInfo rSeatInfo in _seatInfo)
                            {
                                //rSeatInfo = rSeatInfo_loopVariable;
                                //ReDim aPassengerNumber(i)
                                //ReDim aPassengerID(i)
                                //ReDim aUnitDisignator(i)
                                //ReDim acompartmentDesignator(i)
                                if (BookingDetail.RecordLocator.Trim() == rSeatInfo.RecordLocator.Trim() && rSeatInfo.SelectedSeat.Trim() != "")
                                {
                                    aPassengerNumber[i] = rSeatInfo.PassengerNumber;
                                    aPassengerID[i] = Convert.ToInt32(rSeatInfo.PassengerID);
                                    aUnitDisignator[i] = rSeatInfo.SelectedSeat;
                                    acompartmentDesignator[i] = rSeatInfo.CompartmentDesignator;
                                    i += 1;
                                }

                            }
                            //End Assign New Seat

                            //Unassign Seat
                            int k = 0;
                            int[] aunassignPassengerNumber = null;
                            int[] aunassignPassengerID = null;
                            string[] aunassignUnitDisignator = null;
                            string[] aunassigncompartmentDesignator = null;
                            string[] aunassignPNR = null;
                            if (_usassignSeatInfo != null)
                            {

                                int unassignSeatInfoCount1 = _usassignSeatInfo.Count(a => a.SelectedSeat.Trim() != "" && a.RecordLocator.Trim() == BookingDetail.RecordLocator.Trim());
                                aunassignPassengerNumber = new int[unassignSeatInfoCount1];
                                aunassignPassengerID = new int[unassignSeatInfoCount1];
                                aunassignUnitDisignator = new string[unassignSeatInfoCount1];
                                aunassigncompartmentDesignator = new string[unassignSeatInfoCount1];
                                aunassignPNR = new string[unassignSeatInfoCount1];

                                foreach (ABS.Logic.GroupBooking.SeatInfo rSeatInfo in _usassignSeatInfo)
                                {
                                    if (BookingDetail.RecordLocator.Trim() == rSeatInfo.RecordLocator.Trim() && rSeatInfo.SelectedSeat.Trim() != "")
                                    {
                                        aunassignPassengerNumber[k] = rSeatInfo.PassengerNumber;
                                        aunassignPassengerID[k] = Convert.ToInt32(rSeatInfo.PassengerNumber);
                                        aunassignUnitDisignator[k] = rSeatInfo.SelectedSeat;
                                        aunassigncompartmentDesignator[k] = rSeatInfo.CompartmentDesignator;
                                        k += 1;
                                    }
                                }
                            }
                            //End Unassign Seat


                            string STD = "";

                            switch (pFlightType)
                            {
                                #region "Depart Flight"
                                case EnumFlightType.DepartFlight:
                                    STD = model.TemFlightStd.ToString();

                                    if (_usassignSeatInfo != null && _usassignSeatInfo.Count > 0)
                                    {
                                        using (profiler.Step("API:UnAssignSeats"))
                                        {
                                            UnassignResponse = absNavitaire.AssignSeats(true, Signature, model.TemFlightDeparture, model.TemFlightArrival, STD,
                                            aunassignPassengerNumber, aunassignPassengerID, aunassignUnitDisignator, aunassigncompartmentDesignator,
                                            model.TemFlightCarrierCode, model.TemFlightFlightNumber, model.TemFlightOpSuffix);
                                        }
                                            if (UnassignResponse.Success != null)
                                        {
                                            Session["DepartExistingSeatInfo"] = null;
                                        }
                                        else
                                        {
                                            return false;
                                        }

                                    }
                                    using (profiler.Step("API:AssignSeats"))
                                    {
                                        AssignResponse = absNavitaire.AssignSeats(false, Signature, model.TemFlightDeparture, model.TemFlightArrival, STD, aPassengerNumber, aPassengerID, aUnitDisignator, acompartmentDesignator, model.TemFlightCarrierCode, model.TemFlightFlightNumber, model.TemFlightOpSuffix);
                                    }
                                    if (AssignResponse.Success != null)
                                    {
                                        ABS.Navitaire.BookingManager.Booking booking = new ABS.Navitaire.BookingManager.Booking();// absNavitaire.GetBookingFromState(Signature, 3);
                                        using (profiler.Step("Navitaire:GetBookingFromState"))
                                        {
                                            booking = absNavitaire.GetBookingFromState(Signature, 3);
                                        }

                                        var route = model.TemFlightDeparture + model.TemFlightArrival;
                                        decimal totalSeatFees = 0;
                                        if (booking != null)
                                        {
                                            if (booking.Passengers.Length > 0)
                                            {

                                                totalSeatFees = booking.Passengers.Select(p => p.PassengerFees.Where(fee => fee.FeeType == FeeType.SeatFee && fee.FlightReference.Contains(route)).Select(charge => charge.ServiceCharges.Select(x => x.Amount).Sum()).Sum()).Sum();
                                                //foreach (Passenger p in booking.Passengers)
                                                //{
                                                //    if (p.PassengerFees.Length > 0)
                                                //    {
                                                //        foreach (PassengerFee fee in p.PassengerFees)
                                                //        {
                                                //            if (fee.FeeType == FeeType.SeatFee && fee.FlightReference.Contains(route))
                                                //            {
                                                //                if (fee.ServiceCharges.Length > 0)
                                                //                {
                                                //                    foreach (BookingServiceCharge charge in fee.ServiceCharges)
                                                //                    {
                                                //                        //if (charge.ChargeCode == "SEAT" || charge.ChargeCode == "GST")
                                                //                        //{
                                                //                        //    //switch (charge.ChargeCode)
                                                //                        //    //{
                                                //                        //    //    case "SEAT":
                                                //                        //    //        totalSeatFees += charge.Amount;
                                                //                        //    //        break;
                                                //                        //    //    case "GST":
                                                //                        //    //        totalSeatFees += charge.Amount;
                                                //                        //    //        break;
                                                //                        //    //}
                                                //                        totalSeatFees += charge.Amount;
                                                //                        //}
                                                //                    }

                                                //                }
                                                //            }
                                                //        }
                                                //    }
                                                //}
                                            }
                                            if (booking.Journeys.Length > 0 && booking.Journeys[0].Segments.Length > 0 && booking.Journeys[0].Segments[0].PaxSeats.Length > 0)
                                            {
                                                foreach (PaxSeat pax in booking.Journeys[0].Segments[0].PaxSeats)
                                                {
                                                    seatInfo = new ABS.Logic.GroupBooking.SeatInfo();
                                                    //seatInfo.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                                    seatInfo.PassengerID = pax.PassengerNumber.ToString();
                                                    seatInfo.SelectedSeat = pax.UnitDesignator;
                                                    seatInfo.CompartmentDesignator = pax.CompartmentDesignator;
                                                    seatInfo.Deck = pax.PaxSeatInfo.Deck.ToString();
                                                    seatInfo.RecordLocator = BookingDetail.RecordLocator.ToString();
                                                    existingSeatInfo.Add(seatInfo);
                                                }
                                                log.Info(this, "Class of Service 1(depart) : " + booking.Journeys[0].Segments[0].Fares[0].ClassOfService + " requested Seats : " + booking.Journeys[0].Segments[0].PaxSeats.Length + " SellSessionID : " + Signature);
                                            }
                                        }
                                        if (totalSeatFees > 0)
                                        {
                                            Session["DepartFlightSeatFees"] = Convert.ToDecimal(Session["DepartFlightSeatFees"].ToString()) + totalSeatFees;
                                            Session["DepartExistingSeatInfo"] = existingSeatInfo;
                                            SeatFee = totalSeatFees;
                                            
                                            //tyas
                                            int iIndexDepart = listBookingDetail.FindIndex(p => p.Origin == model.TemFlightDeparture && p.Signature == Signature);
                                            if (iIndexDepart >= 0) listBookingDetail[iIndexDepart].LineSeat = totalSeatFees;


                                        }
                                    }
                                    else
                                    {

                                        return false;
                                    }



                                    break;
                                #endregion

                                #region "Return Flight"
                                case EnumFlightType.ReturnFlight:
                                    STD = model2.TemFlightStd.ToString();

                                    if (_usassignSeatInfo != null && _usassignSeatInfo.Count > 0)
                                    {
                                        using (profiler.Step("API:UnAssignSeats"))
                                        {
                                            UnassignResponse = absNavitaire.AssignSeats(true, Signature, model2.TemFlightDeparture, model2.TemFlightArrival, STD,
                                            aunassignPassengerNumber, aunassignPassengerID, aunassignUnitDisignator, aunassigncompartmentDesignator,
                                            model2.TemFlightCarrierCode, model2.TemFlightFlightNumber, model2.TemFlightOpSuffix);
                                        }
                                            if (UnassignResponse.Success != null)
                                        {
                                            Session["ReturnExistingSeatInfo"] = null;
                                        }
                                        else
                                        {
                                            return false;
                                        }

                                    }

                                    using (profiler.Step("API:AssignSeats"))
                                    {
                                        AssignResponse = absNavitaire.AssignSeats(false, Signature, model2.TemFlightDeparture, model2.TemFlightArrival, STD, aPassengerNumber, aPassengerID, aUnitDisignator, acompartmentDesignator, model2.TemFlightCarrierCode, model2.TemFlightFlightNumber, model2.TemFlightOpSuffix);
                                    }
                                    if (AssignResponse.Success != null)
                                    {
                                        ABS.Navitaire.BookingManager.Booking booking2 = new ABS.Navitaire.BookingManager.Booking();// absNavitaire.GetBookingFromState(Signature, 3);
                                        using (profiler.Step("Navitaire:GetBookingFromState"))
                                        {
                                            booking2 = absNavitaire.GetBookingFromState(Signature, 3);
                                        }
                                        var route2 = model2.TemFlightDeparture + model2.TemFlightArrival;
                                        decimal totalSeatFees2 = 0;
                                        if (booking2 != null)
                                        {
                                            if (booking2.Passengers.Length > 0)
                                            {
                                                totalSeatFees2 = booking2.Passengers.Select(p => p.PassengerFees.Where(fee => fee.FeeType == FeeType.SeatFee && fee.FlightReference.Contains(route2)).Select(charge => charge.ServiceCharges.Select(x => x.Amount).Sum()).Sum()).Sum();
                                                //foreach (Passenger p in booking2.Passengers)
                                                //{
                                                //    if (p.PassengerFees.Length > 0)
                                                //    {
                                                //        foreach (PassengerFee fee in p.PassengerFees)
                                                //        {
                                                //            if (fee.FeeType == FeeType.SeatFee && fee.FlightReference.Contains(route2))
                                                //            {
                                                //                if (fee.ServiceCharges.Length > 0)
                                                //                {
                                                //                    foreach (BookingServiceCharge charge in fee.ServiceCharges)
                                                //                    {
                                                //                        //if (charge.ChargeCode == "SEAT" || charge.ChargeCode == "GST")
                                                //                        //{
                                                //                        //    //switch (charge.ChargeCode)
                                                //                        //    //{
                                                //                        //    //    case "SEAT":
                                                //                        //    //        totalSeatFees += charge.Amount;
                                                //                        //    //        break;
                                                //                        //    //    case "GST":
                                                //                        //    //        totalSeatFees += charge.Amount;
                                                //                        //    //        break;
                                                //                        //    //}
                                                //                        //    totalSeatFees2 += charge.Amount;
                                                //                        //}
                                                //                        totalSeatFees2 += charge.Amount;
                                                //                    }
                                                //                }
                                                //            }
                                                //        }
                                                //    }
                                                //}
                                            }

                                            if (booking2.Journeys.Length > 1 && booking2.Journeys[1].Segments.Length > 0 && booking2.Journeys[1].Segments[0].PaxSeats.Length > 0)
                                            {
                                                foreach (PaxSeat pax in booking2.Journeys[1].Segments[0].PaxSeats)
                                                {
                                                    seatInfo = new ABS.Logic.GroupBooking.SeatInfo();
                                                    //seatInfo.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                                    seatInfo.PassengerID = pax.PassengerNumber.ToString();
                                                    seatInfo.SelectedSeat = pax.UnitDesignator;
                                                    seatInfo.CompartmentDesignator = pax.CompartmentDesignator;
                                                    seatInfo.Deck = pax.PaxSeatInfo.Deck.ToString();
                                                    seatInfo.RecordLocator = BookingDetail.RecordLocator.ToString();
                                                    existingSeatInfo.Add(seatInfo);
                                                }
                                                log.Info(this, "Class of Service 1(return) : " + booking2.Journeys[1].Segments[0].Fares[0].ClassOfService + " Requested Seats : " + booking2.Journeys[1].Segments[0].PaxSeats.Length + " SellSessionID : " + Signature);
                                            }
                                        }
                                        if (totalSeatFees2 > 0)
                                        {
                                            Session["ReturnFlightSeatFees"] = Convert.ToDecimal(Session["ReturnFlightSeatFees"].ToString()) + totalSeatFees2;
                                            Session["ReturnExistingSeatInfo"] = existingSeatInfo;
                                            SeatFee = totalSeatFees2;

                                            //tyas
                                            int iIndexReturn = listBookingDetail.FindIndex(p => p.Origin == model2.TemFlightDeparture && p.Signature == Signature);
                                            if (iIndexReturn >= 0) listBookingDetail[iIndexReturn].LineSeat = totalSeatFees2;

                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }

                                    break;
                                #endregion

                                #region "Depart Connecting Flight"
                                case EnumFlightType.DepartConnectingFlight:
                                    STD = model.TemFlightStd.ToString();

                                    if (_usassignSeatInfo != null && _usassignSeatInfo.Count > 0)
                                    {
                                        using (profiler.Step("API:UnAssignSeats"))
                                        {
                                            UnassignResponse = absNavitaire.AssignSeats(true, Signature, model.TemFlightDeparture, model.TemFlightTransit, STD,
                                            aunassignPassengerNumber, aunassignPassengerID, aunassignUnitDisignator, aunassigncompartmentDesignator,
                                            model.TemFlightCarrierCode, model.TemFlightFlightNumber, model.TemFlightOpSuffix);
                                        }
                                            if (UnassignResponse.Success != null)
                                        {
                                            Session["DepartConnectingExistingSeatInfo"] = null;
                                        }
                                        else
                                        {
                                            return false;
                                        }

                                    }
                                    using (profiler.Step("API:AssignSeats"))
                                    {
                                        AssignResponse = absNavitaire.AssignSeats(false, Signature, model.TemFlightDeparture, model.TemFlightTransit, STD, aPassengerNumber, aPassengerID, aUnitDisignator, acompartmentDesignator, model.TemFlightCarrierCode, model.TemFlightFlightNumber, model.TemFlightOpSuffix);
                                    }
                                    if (AssignResponse.Success != null)
                                    {
                                        ABS.Navitaire.BookingManager.Booking booking2 = new ABS.Navitaire.BookingManager.Booking();// absNavitaire.GetBookingFromState(Signature, 3);
                                        using (profiler.Step("Navitaire:GetBookingFromState"))
                                        {
                                            booking2 = absNavitaire.GetBookingFromState(Signature, 3);
                                        }
                                        var route = model.TemFlightDeparture + model.TemFlightTransit;
                                        decimal totalSeatFees2 = 0;
                                        if (booking2 != null)
                                        {
                                            if (booking2.Passengers.Length > 0)
                                            {
                                                totalSeatFees2 = booking2.Passengers.Select(p => p.PassengerFees.Where(fee => fee.FeeType == FeeType.SeatFee && fee.FlightReference.Contains(route)).Select(charge => charge.ServiceCharges.Select(x => x.Amount).Sum()).Sum()).Sum();
                                                //foreach (Passenger p in booking2.Passengers)
                                                //{
                                                //    if (p.PassengerFees.Length > 0)
                                                //    {
                                                //        foreach (PassengerFee fee in p.PassengerFees)
                                                //        {
                                                //            if (fee.FeeType == FeeType.SeatFee && fee.FlightReference.Contains(route))
                                                //            {
                                                //                if (fee.ServiceCharges.Length > 0)
                                                //                {
                                                //                    foreach (BookingServiceCharge charge in fee.ServiceCharges)
                                                //                    {
                                                //                        //if ( charge.ChargeType== ChargeType.ServiceCharge && (charge.ChargeCode == "SEAT" || charge.ChargeCode == "GST"))
                                                //                        //{
                                                //                        //    totalSeatFees2 += charge.Amount;
                                                //                        //}
                                                //                        totalSeatFees2 += charge.Amount;
                                                //                    }
                                                //                }
                                                //            }
                                                //        }
                                                //    }
                                                //}
                                            }

                                            if (booking2.Journeys.Length > 0 && booking2.Journeys[0].Segments.Length > 1 && booking2.Journeys[0].Segments[0].PaxSeats.Length > 0)
                                            {
                                                foreach (PaxSeat pax in booking2.Journeys[0].Segments[0].PaxSeats)
                                                {
                                                    seatInfo = new ABS.Logic.GroupBooking.SeatInfo();
                                                    //seatInfo.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                                    seatInfo.PassengerID = pax.PassengerNumber.ToString();
                                                    seatInfo.SelectedSeat = pax.UnitDesignator;
                                                    seatInfo.CompartmentDesignator = pax.CompartmentDesignator;
                                                    seatInfo.Deck = pax.PaxSeatInfo.Deck.ToString();
                                                    seatInfo.RecordLocator = BookingDetail.RecordLocator.ToString();
                                                    existingSeatInfo.Add(seatInfo);
                                                }
                                                log.Info(this, "Class of Service 1(depart) : " + booking2.Journeys[0].Segments[0].Fares[0].ClassOfService + " requested Seats : " + booking2.Journeys[0].Segments[0].PaxSeats.Length + " SellSessionID : " + Signature);
                                            }

                                        }
                                        if (totalSeatFees2 > 0)
                                        {
                                            Session["DepartConnectingFlightSeatFees"] = Convert.ToDecimal(Session["DepartConnectingFlightSeatFees"].ToString()) + totalSeatFees2;
                                            Session["DepartConnectingExistingSeatInfo"] = existingSeatInfo;
                                            SeatFee = totalSeatFees2;

                                            //tyas
                                            int iIndexDepart = listBookingDetail.FindIndex(p => p.Origin == model.TemFlightDeparture && p.Signature == Signature);
                                            //if (iIndexDepart >= 0) listBookingDetail[iIndexDepart].LineSeat = totalSeatFees2;
                                            //enhance by ketee, add seat fees to specific connecting flight field, 20170120
                                            if (iIndexDepart >= 0) listBookingDetail[iIndexDepart].LineConnectingSeat = totalSeatFees2;
                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }

                                    break;
                                #endregion

                                #region "Depart Connecting Flight 2"
                                case EnumFlightType.DepartConnectingFlight2:
                                    STD = model.TemFlightStd2.ToString();

                                    if (_usassignSeatInfo != null && _usassignSeatInfo.Count > 0)
                                    {
                                        using (profiler.Step("API:UnAssignSeats"))
                                        {
                                            UnassignResponse = absNavitaire.AssignSeats(true, Signature, model.TemFlightTransit, model.TemFlightArrival, STD,
                                            aunassignPassengerNumber, aunassignPassengerID, aunassignUnitDisignator, aunassigncompartmentDesignator,
                                            model.TemFlightCarrierCode2, model.TemFlightFlightNumber2, model.TemFlightOpSuffix2);
                                        }
                                            if (UnassignResponse.Success != null)
                                        {
                                            Session["DepartConnectingExistingSeatInfo2"] = null;
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }

                                    using (profiler.Step("API:AssignSeats"))
                                    {
                                        AssignResponse = absNavitaire.AssignSeats(false, Signature, model.TemFlightTransit, model.TemFlightArrival, STD, aPassengerNumber, aPassengerID, aUnitDisignator, acompartmentDesignator, model.TemFlightCarrierCode2, model.TemFlightFlightNumber2, model.TemFlightOpSuffix2);
                                    }

                                    if (AssignResponse.Success != null)
                                    {
                                        ABS.Navitaire.BookingManager.Booking booking2 = new ABS.Navitaire.BookingManager.Booking();// absNavitaire.GetBookingFromState(Signature, 3);
                                        using (profiler.Step("Navitaire:GetBookingFromState"))
                                        {
                                            booking2 = absNavitaire.GetBookingFromState(Signature, 3);
                                        }
                                        var route = model.TemFlightTransit + model.TemFlightArrival;
                                        decimal totalSeatFees2 = 0;
                                        if (booking2 != null)
                                        {
                                            if (booking2.Passengers.Length > 0)
                                            {
                                                totalSeatFees2 = booking2.Passengers.Select(p => p.PassengerFees.Where(fee => fee.FeeType == FeeType.SeatFee && fee.FlightReference.Contains(route)).Select(charge => charge.ServiceCharges.Select(x => x.Amount).Sum()).Sum()).Sum();
                                                //foreach (Passenger p in booking2.Passengers)
                                                //{
                                                //    if (p.PassengerFees.Length > 0)
                                                //    {
                                                //        foreach (PassengerFee fee in p.PassengerFees)
                                                //        {
                                                //            if (fee.FeeType == FeeType.SeatFee && fee.FlightReference.Contains(route))
                                                //            {
                                                //                if (fee.ServiceCharges.Length > 0)
                                                //                {
                                                //                    foreach (BookingServiceCharge charge in fee.ServiceCharges)
                                                //                    {
                                                //                        //if (charge.ChargeCode == "SEAT" || charge.ChargeCode == "GST")
                                                //                        //{
                                                //                        //    //switch (charge.ChargeCode)
                                                //                        //    //{
                                                //                        //    //    case "SEAT":
                                                //                        //    //        totalSeatFees += charge.Amount;
                                                //                        //    //        break;
                                                //                        //    //    case "GST":
                                                //                        //    //        totalSeatFees += charge.Amount;
                                                //                        //    //        break;
                                                //                        //    //}
                                                //                        //    totalSeatFees2 += charge.Amount;
                                                //                        //}
                                                //                        totalSeatFees2 += charge.Amount;
                                                //                    }
                                                //                }
                                                //            }
                                                //        }
                                                //    }
                                                //}
                                            }

                                            if (booking2.Journeys.Length > 0 && booking2.Journeys[0].Segments.Length > 1 && booking2.Journeys[0].Segments[1].PaxSeats.Length > 0)
                                            {
                                                foreach (PaxSeat pax in booking2.Journeys[0].Segments[1].PaxSeats)
                                                {
                                                    seatInfo = new ABS.Logic.GroupBooking.SeatInfo();
                                                    //seatInfo.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                                    seatInfo.PassengerID = pax.PassengerNumber.ToString();
                                                    seatInfo.SelectedSeat = pax.UnitDesignator;
                                                    seatInfo.CompartmentDesignator = pax.CompartmentDesignator;
                                                    seatInfo.Deck = pax.PaxSeatInfo.Deck.ToString();
                                                    seatInfo.RecordLocator = BookingDetail.RecordLocator.ToString();
                                                    existingSeatInfo.Add(seatInfo);
                                                }
                                                log.Info(this, "Class of Service 2(depart) : " + booking2.Journeys[0].Segments[1].Fares[0].ClassOfService + " requested Seats : " + booking2.Journeys[0].Segments[1].PaxSeats.Length + "SellSessionID : " + Signature);
                                            }
                                        }
                                        if (totalSeatFees2 > 0)
                                        {
                                            Session["DepartConnectingFlightSeatFees2"] = Convert.ToDecimal(Session["DepartConnectingFlightSeatFees2"].ToString()) + totalSeatFees2;
                                            Session["DepartConnectingExistingSeatInfo2"] = existingSeatInfo;
                                            SeatFee = totalSeatFees2;

                                            //tyas
                                            int iIndexDepart = listBookingDetail.FindIndex(p => p.Origin == model.TemFlightDeparture && p.Signature == Signature);
                                            //if (iIndexDepart >= 0) listBookingDetail[iIndexDepart].LineSeat = totalSeatFees2;
                                            //enhance by ketee, add seat fees to specific connecting flight field, 20170120
                                            if (iIndexDepart >= 0) listBookingDetail[iIndexDepart].LineConnectingSeat2 = totalSeatFees2;
                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }

                                    break;
                                #endregion

                                #region "Return Connecting Flight"
                                case EnumFlightType.ReturnConnectingFlight:
                                    STD = model2.TemFlightStd.ToString();

                                    if (_usassignSeatInfo != null && _usassignSeatInfo.Count > 0)
                                    {
                                        using (profiler.Step("API:UnAssignSeats"))
                                        {
                                            UnassignResponse = absNavitaire.AssignSeats(true, Signature, model2.TemFlightDeparture, model2.TemFlightTransit, STD,
                                            aunassignPassengerNumber, aunassignPassengerID, aunassignUnitDisignator, aunassigncompartmentDesignator,
                                            model2.TemFlightCarrierCode, model2.TemFlightFlightNumber, model2.TemFlightOpSuffix);
                                        }
                                            if (UnassignResponse.Success != null)
                                        {
                                            Session["ReturnConnectingExistingSeatInfo"] = null;
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }

                                    using (profiler.Step("API:AssignSeats"))
                                    {
                                        AssignResponse = absNavitaire.AssignSeats(false, Signature, model2.TemFlightDeparture, model2.TemFlightTransit, STD, aPassengerNumber, aPassengerID, aUnitDisignator, acompartmentDesignator, model2.TemFlightCarrierCode, model2.TemFlightFlightNumber, model2.TemFlightOpSuffix);
                                    }

                                    if (AssignResponse.Success != null)
                                    {
                                        ABS.Navitaire.BookingManager.Booking booking2 = new ABS.Navitaire.BookingManager.Booking();// absNavitaire.GetBookingFromState(Signature, 3);
                                        using (profiler.Step("Navitaire:GetBookingFromState"))
                                        {
                                            booking2 = absNavitaire.GetBookingFromState(Signature, 3);
                                        }
                                        var route2 = model2.TemFlightDeparture + model2.TemFlightTransit;
                                        decimal totalSeatFees2 = 0;
                                        if (booking2 != null)
                                        {
                                            if (booking2.Passengers.Length > 0)
                                            {
                                                totalSeatFees2 = booking2.Passengers.Select(p => p.PassengerFees.Where(fee => fee.FeeType == FeeType.SeatFee && fee.FlightReference.Contains(route2)).Select(charge => charge.ServiceCharges.Select(x => x.Amount).Sum()).Sum()).Sum();
                                                //foreach (Passenger p in booking2.Passengers)
                                                //{
                                                //    if (p.PassengerFees.Length > 0)
                                                //    {
                                                //        foreach (PassengerFee fee in p.PassengerFees)
                                                //        {
                                                //            if (fee.FeeType == FeeType.SeatFee && fee.FlightReference.Contains(route2))
                                                //            {
                                                //                if (fee.ServiceCharges.Length > 0)
                                                //                {
                                                //                    foreach (BookingServiceCharge charge in fee.ServiceCharges)
                                                //                    {
                                                //                        //if (charge.ChargeCode == "SEAT" || charge.ChargeCode == "GST")
                                                //                        //{
                                                //                        //    //switch (charge.ChargeCode)
                                                //                        //    //{
                                                //                        //    //    case "SEAT":
                                                //                        //    //        totalSeatFees += charge.Amount;
                                                //                        //    //        break;
                                                //                        //    //    case "GST":
                                                //                        //    //        totalSeatFees += charge.Amount;
                                                //                        //    //        break;
                                                //                        //    //}
                                                //                        //    totalSeatFees2 += charge.Amount;
                                                //                        //}
                                                //                        totalSeatFees2 += charge.Amount;
                                                //                    }
                                                //                }
                                                //            }
                                                //        }
                                                //    }
                                                //}
                                            }

                                            if (booking2.Journeys.Length > 1 && booking2.Journeys[1].Segments.Length > 1 && booking2.Journeys[1].Segments[1].PaxSeats.Length > 0)
                                            {
                                                foreach (PaxSeat pax in booking2.Journeys[1].Segments[0].PaxSeats)
                                                {
                                                    seatInfo = new ABS.Logic.GroupBooking.SeatInfo();
                                                    //seatInfo.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                                    seatInfo.PassengerID = pax.PassengerNumber.ToString();
                                                    seatInfo.SelectedSeat = pax.UnitDesignator;
                                                    seatInfo.CompartmentDesignator = pax.CompartmentDesignator;
                                                    seatInfo.Deck = pax.PaxSeatInfo.Deck.ToString();
                                                    seatInfo.RecordLocator = BookingDetail.RecordLocator.ToString();
                                                    existingSeatInfo.Add(seatInfo);
                                                }
                                                log.Info(this, "Class of Service 1(return) : " + booking2.Journeys[1].Segments[0].Fares[0].ClassOfService + " requested Seats : " + booking2.Journeys[1].Segments[0].PaxSeats.Length + " SellSessionID : " + Signature);
                                            }
                                        }
                                        if (totalSeatFees2 > 0)
                                        {
                                            Session["ReturnConnectingFlightSeatFees"] = Convert.ToDecimal(Session["ReturnConnectingFlightSeatFees"].ToString()) + totalSeatFees2;
                                            Session["ReturnConnectingExistingSeatInfo"] = existingSeatInfo;
                                            SeatFee = totalSeatFees2;

                                            //tyas
                                            int iIndexReturn = listBookingDetail.FindIndex(p => p.Origin == model2.TemFlightDeparture && p.Signature == Signature);
                                            //if (iIndexReturn >= 0) listBookingDetail[iIndexReturn].LineSeat = totalSeatFees2;
                                            //enhance by ketee, add seat fees to specific connecting flight field, 20170120
                                            if (iIndexReturn >= 0) listBookingDetail[iIndexReturn].LineConnectingSeat = totalSeatFees2;
                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }

                                    break;
                                #endregion

                                #region "Return Connecting Flight2"
                                case EnumFlightType.ReturnConnectingFlight2:
                                    STD = model2.TemFlightStd2.ToString();

                                    if (_usassignSeatInfo != null && _usassignSeatInfo.Count > 0)
                                    {
                                        using (profiler.Step("API:UnAssignSeats"))
                                        {
                                            UnassignResponse = absNavitaire.AssignSeats(true, Signature, model2.TemFlightTransit, model2.TemFlightArrival, STD,
                                            aunassignPassengerNumber, aunassignPassengerID, aunassignUnitDisignator, aunassigncompartmentDesignator,
                                            model2.TemFlightCarrierCode2, model2.TemFlightFlightNumber2, model2.TemFlightOpSuffix2);
                                        }
                                            if (UnassignResponse.Success != null)
                                        {
                                            Session["ReturnConnectingExistingSeatInfo2"] = null;
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }

                                    using (profiler.Step("API:AssignSeats"))
                                    {
                                        AssignResponse = absNavitaire.AssignSeats(false, Signature, model2.TemFlightTransit, model2.TemFlightArrival, STD, aPassengerNumber, aPassengerID, aUnitDisignator, acompartmentDesignator, model2.TemFlightCarrierCode2, model2.TemFlightFlightNumber2, model2.TemFlightOpSuffix2);
                                    }

                                    if (AssignResponse.Success != null)
                                    {
                                        ABS.Navitaire.BookingManager.Booking booking2 = new ABS.Navitaire.BookingManager.Booking();// absNavitaire.GetBookingFromState(Signature, 3);
                                        using (profiler.Step("Navitaire:GetBookingFromState"))
                                        {
                                            booking2 = absNavitaire.GetBookingFromState(Signature, 3);
                                        }
                                        var route2 = model2.TemFlightTransit + model2.TemFlightArrival;
                                        decimal totalSeatFees2 = 0;
                                        if (booking2 != null)
                                        {
                                            if (booking2.Passengers.Length > 0)
                                            {
                                                foreach (Passenger p in booking2.Passengers)
                                                {
                                                    if (p.PassengerFees.Length > 0)
                                                    {
                                                        foreach (PassengerFee fee in p.PassengerFees)
                                                        {
                                                            if (fee.FeeType == FeeType.SeatFee && fee.FlightReference.Contains(route2))
                                                            {
                                                                if (fee.ServiceCharges.Length > 0)
                                                                {
                                                                    foreach (BookingServiceCharge charge in fee.ServiceCharges)
                                                                    {
                                                                        //if (charge.ChargeCode == "SEAT" || charge.ChargeCode == "GST")
                                                                        //{
                                                                        //    //switch (charge.ChargeCode)
                                                                        //    //{
                                                                        //    //    case "SEAT":
                                                                        //    //        totalSeatFees += charge.Amount;
                                                                        //    //        break;
                                                                        //    //    case "GST":
                                                                        //    //        totalSeatFees += charge.Amount;
                                                                        //    //        break;
                                                                        //    //}
                                                                        //    totalSeatFees2 += charge.Amount;
                                                                        //}
                                                                        totalSeatFees2 += charge.Amount;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            if (booking2.Journeys.Length > 1 && booking2.Journeys[1].Segments.Length > 1 && booking2.Journeys[1].Segments[1].PaxSeats.Length > 0)
                                            {
                                                foreach (PaxSeat pax in booking2.Journeys[1].Segments[1].PaxSeats)
                                                {
                                                    seatInfo = new ABS.Logic.GroupBooking.SeatInfo();
                                                    //seatInfo.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                                    seatInfo.PassengerID = pax.PassengerNumber.ToString();
                                                    seatInfo.SelectedSeat = pax.UnitDesignator;
                                                    seatInfo.CompartmentDesignator = pax.CompartmentDesignator;
                                                    seatInfo.Deck = pax.PaxSeatInfo.Deck.ToString();
                                                    seatInfo.RecordLocator = BookingDetail.RecordLocator.ToString();
                                                    existingSeatInfo.Add(seatInfo);
                                                }
                                                log.Info(this, "Class of Service 2(return) : " + booking2.Journeys[1].Segments[1].Fares[0].ClassOfService + " requested Seats : " + booking2.Journeys[1].Segments[1].PaxSeats.Length + " SellSessionID : " + Signature);
                                            }
                                        }
                                        if (totalSeatFees2 > 0)
                                        {
                                            Session["ReturnConnectingFlightSeatFees2"] = Convert.ToDecimal(Session["ReturnConnectingFlightSeatFees2"].ToString()) + totalSeatFees2;
                                            Session["ReturnConnectingExistingSeatInfo2"] = existingSeatInfo;
                                            SeatFee = totalSeatFees2;

                                            //tyas
                                            int iIndexReturn = listBookingDetail.FindIndex(p => p.Origin == model2.TemFlightDeparture && p.Signature == Signature);
                                            //if (iIndexReturn >= 0) listBookingDetail[iIndexReturn].LineSeat = totalSeatFees2;
                                            //enhance by ketee, add seat fees to specific connecting flight field, 20170120
                                            if (iIndexReturn >= 0) listBookingDetail[iIndexReturn].LineConnectingSeat2 = totalSeatFees2;
                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }

                                    break;
                                    #endregion
                            }

                            if (Session["TransID"] != null)
                            {
                                //update passenger seats
                                List<PassengerData> lstPassenger = new List<PassengerData>();
                                if (HttpContext.Current.Session["listPassengers"] != null)
                                {
                                    lstPassenger = (List<PassengerData>)HttpContext.Current.Session["listPassengers"];
                                    int MaxChange = lstPassenger.Select(pax => pax.MaxChange).FirstOrDefault();
                                    if (Request.QueryString["change"] != null)
                                    {
                                        MaxChange = 1;
                                    }


                                    //foreach (PassengerData pax in lstPassenger)
                                    //{
                                    //    if (Request.QueryString["change"] != null)
                                    //    {
                                    //        pax.MaxChange = 1;
                                    //    }
                                    foreach (ABS.Logic.GroupBooking.SeatInfo rSeatInfo in _seatInfo)
                                    {
                                        if (lstPassenger.Select(pax => pax.RecordLocator.Trim() == rSeatInfo.RecordLocator.Trim() && pax.PassengerID == rSeatInfo.PassengerID && rSeatInfo.SelectedSeat.Trim() != "").Count() > 0)
                                        {
                                            switch (pFlightType)
                                            {
                                                case EnumFlightType.DepartFlight:
                                                case EnumFlightType.DepartConnectingFlight:
                                                    if (rSeatInfo.SelectedSeat.Trim() != "")
                                                    {
                                                        lstPassenger.FirstOrDefault(pax => pax.RecordLocator.Trim() == rSeatInfo.RecordLocator.Trim() && pax.PassengerID == rSeatInfo.PassengerID && rSeatInfo.SelectedSeat.Trim() != "").DepartSeat = "0_" + rSeatInfo.CompartmentDesignator + "_" + rSeatInfo.Deck + "_" + rSeatInfo.SelectedSeat;
                                                    }
                                                    break;
                                                case EnumFlightType.DepartConnectingFlight2:
                                                    if (rSeatInfo.SelectedSeat.Trim() != "")
                                                    {
                                                        lstPassenger.FirstOrDefault(pax => pax.RecordLocator.Trim() == rSeatInfo.RecordLocator.Trim() && pax.PassengerID == rSeatInfo.PassengerID && rSeatInfo.SelectedSeat.Trim() != "").DepartConnectingSeat = "0_" + rSeatInfo.CompartmentDesignator + "_" + rSeatInfo.Deck + "_" + rSeatInfo.SelectedSeat;
                                                    }
                                                    break;
                                                case EnumFlightType.ReturnFlight:
                                                case EnumFlightType.ReturnConnectingFlight:
                                                    if (rSeatInfo.SelectedSeat.Trim() != "")
                                                    {
                                                        lstPassenger.FirstOrDefault(pax => pax.RecordLocator.Trim() == rSeatInfo.RecordLocator.Trim() && pax.PassengerID == rSeatInfo.PassengerID && rSeatInfo.SelectedSeat.Trim() != "").ReturnSeat = "0_" + rSeatInfo.CompartmentDesignator + "_" + rSeatInfo.Deck + "_" + rSeatInfo.SelectedSeat;
                                                    }
                                                    break;
                                                case EnumFlightType.ReturnConnectingFlight2:
                                                    if (rSeatInfo.SelectedSeat.Trim() != "")
                                                    {
                                                        lstPassenger.FirstOrDefault(pax => pax.RecordLocator.Trim() == rSeatInfo.RecordLocator.Trim() && pax.PassengerID == rSeatInfo.PassengerID && rSeatInfo.SelectedSeat.Trim() != "").ReturnConnectingSeat = "0_" + rSeatInfo.CompartmentDesignator + "_" + rSeatInfo.Deck + "_" + rSeatInfo.SelectedSeat;
                                                    }
                                                    break;
                                            }
                                        }
                                        //}
                                    }

                                    Session["listPassengers"] = lstPassenger;

                                }
                                else
                                {

                                }
                            }


                            //begin, update dataclasstrans here
                            dataClass.Rows[j]["SeatChrg"] = Convert.ToDecimal(dataClass.Rows[j]["SeatChrg"].ToString()) + SeatFee;
                            dataClass.Rows[j]["FullPrice"] = Convert.ToDecimal(dataClass.Rows[j]["FullPrice"].ToString()) + SeatFee;
                            HttpContext.Current.Session["dataClassTrans"] = dataClass;
                            //end, update dataclasstrans here

                        }
                        catch (Exception ex)
                        {
                            SystemLog.Notifier.Notify(ex);
                            //sTraceLog(ex.ToString);
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                Session["listBookingDetail"] = listBookingDetail;

                return true;
            }
            else
            {
                return false;
            }
        }

        protected void ClearSeatFeeValue()
        {
            DataTable dataClass = (DataTable)HttpContext.Current.Session["dataClassTrans"];
            if (dataClass != null)
            {
                ////totalConnectingSeatFees = null;
                ////totalConnectingSeatFees = new string[dataClass.Rows.Count, dataClass.Rows.Count];
                ////totalConnectingSeatFees2 = null;
                ////totalConnectingSeatFees2 = new string[dataClass.Rows.Count, dataClass.Rows.Count];

                for (int j = 0; j < dataClass.Rows.Count; j++)
                {
                    dataClass.Rows[j]["FullPrice"] = Convert.ToDecimal(dataClass.Rows[j]["FullPrice"].ToString()) - Convert.ToDecimal(dataClass.Rows[j]["SeatChrg"].ToString());
                    dataClass.Rows[j]["SeatChrg"] = 0;

                    ////totalConnectingSeatFees[j, 0] = dataClass.Rows[j]["SellSignature"].ToString();
                    ////totalConnectingSeatFees2[j, 0] = dataClass.Rows[j]["SellSignature"].ToString();
                }
            }
            HttpContext.Current.Session["dataClassTrans"] = dataClass;

            Session["DepartFlightSeatFees"] = 0;
            Session["ReturnFlightSeatFees"] = 0;
            Session["DepartConnectingFlightSeatFees"] = 0;
            Session["DepartConnectingFlightSeatFees2"] = 0;
            Session["ReturnConnectingFlightSeatFees"] = 0;
            Session["ReturnConnectingFlightSeatFees2"] = 0;
        }

        protected void UpdateTotalAmount(decimal TotalSeatDepart, decimal TotalSeatReturn, ref decimal TotalAmountGoing, ref decimal TotalAmountReturn)
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

            //update fee
            string strExpr;
            string strSort;
            DataTable dt = new DataTable();
            DataRow[] foundRows;

            //depart
            decimal TotalAmount;
            DataTable dtBDFee = objBooking.dtBreakdownFee();
            dtBDFee = (DataTable)HttpContext.Current.Session["dataBDFeeDepart"];
            dtBDFee.Rows[0]["Seat"] = Convert.ToDecimal(TotalSeatDepart);

            TotalAmount = Convert.ToDecimal(dtBDFee.Rows[0]["Fare"]) * model.TemFlightPaxNum;
            if (dtBDFee.Rows[0]["Infant"] != DBNull.Value)
            {
                TotalAmount += Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * model.TemFlightADTNum + Convert.ToDecimal(dtBDFee.Rows[0]["PaxFee"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Svc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) + Convert.ToDecimal(dtBDFee.Rows[0]["SSR"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Seat"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Infant"]);
            }
            else
            {
                TotalAmount += Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * model.TemFlightADTNum + Convert.ToDecimal(dtBDFee.Rows[0]["PaxFee"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Svc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) + Convert.ToDecimal(dtBDFee.Rows[0]["SSR"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Seat"]);
            }
            //TotalAmount += Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * model.TemFlightADTNum + Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Svc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) + Convert.ToDecimal(dtBDFee.Rows[0]["SSR"]) + +Convert.ToDecimal(dtBDFee.Rows[0]["Seat"]);
            if (Convert.ToInt32(model.TemFlightCHDNum.ToString()) != 0)
            {
                TotalAmount += Convert.ToDecimal(dtBDFee.Rows[0]["ChApt"]) * model.TemFlightCHDNum;
            }
            TotalAmountGoing = TotalAmount;
            HttpContext.Current.Session["dataBDFeeDepart"] = dtBDFee;

            strExpr = "TemFlightId = '" + departID + "'";
            strSort = "";
            dt = (DataTable)HttpContext.Current.Session["TempFlight"];
            foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
            foundRows[0]["TemFlightTotalAmount"] = (TotalAmount);
            HttpContext.Current.Session["TempFlight"] = dt;

            //return
            if (model2 == null == false)
            {
                dtBDFee = objBooking.dtBreakdownFee();
                dtBDFee = (DataTable)HttpContext.Current.Session["dataBDFeeReturn"];
                dtBDFee.Rows[0]["Seat"] = Convert.ToDecimal(TotalSeatReturn);

                TotalAmount = Convert.ToDecimal(dtBDFee.Rows[0]["Fare"]) * model.TemFlightPaxNum;
                if (dtBDFee.Rows[0]["Infant"] != DBNull.Value)
                {
                    TotalAmount += Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * model.TemFlightADTNum + Convert.ToDecimal(dtBDFee.Rows[0]["PaxFee"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Svc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) + Convert.ToDecimal(dtBDFee.Rows[0]["SSR"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Seat"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Infant"]);
                }
                else
                {
                    TotalAmount += Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * model.TemFlightADTNum + Convert.ToDecimal(dtBDFee.Rows[0]["PaxFee"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Svc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) + Convert.ToDecimal(dtBDFee.Rows[0]["SSR"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Seat"]);
                }
                //TotalAmount += Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * model.TemFlightADTNum + Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Svc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) + Convert.ToDecimal(dtBDFee.Rows[0]["SSR"]) + +Convert.ToDecimal(dtBDFee.Rows[0]["Seat"]);
                if (Convert.ToInt32(model.TemFlightCHDNum.ToString()) != 0)
                {
                    TotalAmount += Convert.ToDecimal(dtBDFee.Rows[0]["ChApt"]) * model.TemFlightCHDNum;
                }
                TotalAmountReturn = TotalAmount;
                HttpContext.Current.Session["dataBDFeeReturn"] = dtBDFee;

                strExpr = "TemFlightId = '" + ReturnID + "'";
                strSort = "";
                dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                foundRows[0]["TemFlightTotalAmount"] = (TotalAmount);
                HttpContext.Current.Session["TempFlight"] = dt;
            }

            //update fee

        }

        protected void assignSeatCallBack_Callback(object source, CallbackEventArgs e)
        {
            if (e.Parameter != null && e.Parameter.ToLower() == "back")
            {
                ASPxWebControl.RedirectOnCallback("SelectAddOn.aspx");
                return;
            }
            else if (e.Parameter != null && e.Parameter.ToLower() == "selectrandom")
            {
                hResult.Value = ValidateSeatRandomSeat();
                e.Result = hResult.Value; 
            }
            else
            {
                hResult.Value = ValidateSeat();
                e.Result = hResult.Value;
            }
        }

        public bool DeleteXML(string URL)
        {
            try
            {
                string logFilePath = "";
                logFilePath = Request.PhysicalApplicationPath + URL;
                if (System.IO.File.Exists(logFilePath))
                {
                    System.IO.File.Delete(logFilePath);

                }
                return true;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                return false;
            }
        }

        public bool CheckDefaultSeatExist(EnumFlightType FlightType)
        {
            try
            {
                return true;
                //DataTable dtPassengerDetails = null;
                //dtPassengerDetails = PassengerSeatDetails(FlightType);
                //if ((dtPassengerDetails != null) && dtPassengerDetails.Rows.Count > 0)
                //{
                //    foreach (DataRow rowPassenger in dtPassengerDetails.Rows)
                //    {
                //        if (!string.IsNullOrEmpty(rowPassenger.Item("UnitDesignator").ToString))
                //        {
                //            return true;
                //            break; // TODO: might not be correct. Was : Exit For
                //        }
                //    }
                //    return false;
                //}
                //else
                //{
                //    return false;
                //}

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                return false;
            }
        }

        protected static bool IsNumeric(string s)
        {
            double result;
            return s != null && Double.TryParse(s, out result);
        }

        #endregion



    }
}