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
using ABS.GBS.Log;

namespace GroupBooking.Web.UserControl
{
    public partial class flightdetail : System.Web.UI.UserControl
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
        List<BookingTransactionDetail> lstbookFlightInfo = new List<BookingTransactionDetail>();
        List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["PaxStatus"] = "";
            if (Session["AgentSet"] != null)
            { MyUserSet = (UserSet)Session["AgentSet"]; }

            SessionContext sesscon = new SessionContext();
            //sesscon.ValidateAgentLogin();
            if (!IsPostBack)
            {

                //end added
                InitializeForm();
                //added by ketee
                //setValue();
                CheckReturnForSaving();
                //SavingProcess();
            }
            else
            {
                //setValue();
                //CheckReturnForSaving();// add by agus
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

                    ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
                    ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model2 = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();

                    string strExpr;
                    string strSort;
                    DataTable dt = objBooking.dtFlight();

                    strExpr = "TemFlightId = '" + OutID + "'";

                    strSort = "";

                    dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                    if (dt != null)
                    {
                        DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);


                        FillModelFromDataRow(foundRows, ref  model);

                        if (InID != "")
                        {
                            strExpr = "TemFlightId = '" + InID + "'";

                            strSort = "";

                            dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                            foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                            FillModelFromDataRow(foundRows, ref  model2);

                        }

                        //Update temFlight DataTable
                        string sessID = "";
                        //objBooking.UpdateTemFlight(model, model2, "", ref sessID);
                        //Session["signatureSess"] = sessID;
                        //string signature = Session["signatureSess"].ToString();
                        //SellFlight();

                        lblAgentName.Text = MyUserSet.AgentName;
                        if (Session["OrganizationName"] != null)
                        {
                            lblAgentOrg.Text = Session["OrganizationName"].ToString();
                        }
                        BindData(OutID, InID, sessID);
                    }
                    else
                    {
                        LoadData();
                    }

                }
                else
                {
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
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

                FillModelFromDataRow(foundRows, ref model);

                return model;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                return null;
            }
        }

        private void BindData(string OutID, string InID, string sessID)
        {
            ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
            ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model2 = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();

            decimal total = 0;
            model = GetTemFlight(OutID);

            bool freeSVCF = false;

            HttpCookie cookie = Request.Cookies["cookieSearchcondition"];

            string tempdate1 = String.Format("{0:MM/dd/yyyy}", model.TemFlightSta);
            if (model.TemFlightTransit != "")
                tempdate1 = String.Format("{0:MM/dd/yyyy}", model.TemFlightSta2);
            string tempdate2 = String.Format("{0:MM/dd/yyyy}", model.TemFlightStd);
            //lblDateDepart.Text = String.Format("{0:dddd, dd MMMM yyyy}", model.TemFlightStd);
            TimeSpan ts = Convert.ToDateTime(tempdate1) - Convert.ToDateTime(tempdate2);
            string temp = "";
            if (ts.Days > 0)
            {
                if (ts.Days == 1) temp = " (+" + ts.TotalDays.ToString() + " day)"; else temp = " (+" + ts.TotalDays.ToString() + " days)";
            }
            if (model.TemFlightTransit != "")
            {
                //Show flight detail info with connecting flight on newly added control (20170208 - Sienny)
                lbl_CarrierCodeOut.Text = model.TemFlightCarrierCode;
                lbl_FlightnumberOut.Text = model.TemFlightFlightNumber; 
                lbl_DepartureOut.Text = model.TemFlightDeparture;
                lbl_DepartureDateOut.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", model.TemFlightStd);
                lbl_ArrivalOut.Text = model.TemFlightTransit;
                lbl_ArrivalDateOut.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", model.TemFlightSta);

                lbl_CarrierCodeOut2.Text = model.TemFlightCarrierCode2;
                lbl_FlightnumberOut2.Text = model.TemFlightFlightNumber2;
                lbl_DepartureOut2.Text = model.TemFlightTransit;
                lbl_DepartureDateOut2.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", model.TemFlightStd2);
                lbl_ArrivalOut2.Text = model.TemFlightArrival;
                lbl_ArrivalDateOut2.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", model.TemFlightSta2) + temp;
            }
            else
            {
                //Show flight detail info without connecting flight on newly added control (20170208 - Sienny)
                connectWrapperDepart.Visible = false;

                lbl_CarrierCodeOut.Text = model.TemFlightCarrierCode;
                lbl_FlightnumberOut.Text = model.TemFlightFlightNumber;
                lbl_DepartureOut.Text = model.TemFlightDeparture;
                lbl_DepartureDateOut.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", model.TemFlightStd);
                lbl_ArrivalOut.Text = model.TemFlightArrival;
                lbl_ArrivalDateOut.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", model.TemFlightSta) + temp;
            }

            //lbl_ArrivalOut.Text = model.TemFlightArrival; // objGeneral.GetCityNameByCode(model.TemFlightArrival) + "(" + model.TemFlightArrival + ")";

            //lbl_CarrierCodeOut.Text = model.TemFlightCarrierCode;
            //lbl_FlightnumberOut.Text = model.TemFlightFlightNumber;

            //lbl_DepartureDateOut.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", model.TemFlightStd);
            //lbl_DepartureOut.Text = model.TemFlightDeparture; // objGeneral.GetCityNameByCode(model.TemFlightDeparture) + "(" + model.TemFlightDeparture + ")";

            lbl_GuestNumout.Text = model.TemFlightADTNum.ToString();
            lbl_PaxNumout.Text = model.TemFlightPaxNum.ToString();
            lbl_ChildNumout.Text = model.TemFlightCHDNum.ToString();


            promoCodeFlight.Visible = false;
            DataTable dtBDFee = objBooking.dtBreakdownFee();
            dtBDFee = (DataTable)HttpContext.Current.Session["dataBDFeeDepart"];
            if (cookie != null)
            {
                if (dtBDFee != null && dtBDFee.Rows.Count > 0)
                {
                    if ((Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"]) / model.TemFlightPaxNum) < 0)
                    {
                        promoCodeFlight.Visible = true;
                        lbl_PromoCode.Text = "PROMO " + model.TemFlightPromoCode.ToUpper();
                    }
                    else
                    {
                        promoCodeFlight.Visible = false;
                        lbl_PromoCode.Text = "";
                    }
                }
            }


            ///amended by Diana,
            ///added divide with num of passenger to show single amount

            if (InID != "")
            {
                //tdreturnfare.Visible = true;

                //Flight detail info on newly added control (20170208 - Sienny)
                mainReturnConnectWrapper.Visible = true;

                //tr_Return.Visible = true;
                //td_Return.Visible = true;
                //trReturnIcon.Visible = true;

                model2 = GetTemFlight(InID);

                if (model2.TemFlightTransit != "")
                    tempdate1 = String.Format("{0:MM/dd/yyyy}", model2.TemFlightSta2);
                else
                    tempdate1 = String.Format("{0:MM/dd/yyyy}", model2.TemFlightSta);
                tempdate2 = String.Format("{0:MM/dd/yyyy}", model2.TemFlightStd);
                ts = Convert.ToDateTime(tempdate1) - Convert.ToDateTime(tempdate2);
                //lblDateReturn.Text = String.Format("{0:dddd, dd MMMM yyyy}", model2.TemFlightStd);
                temp = "";
                if (ts.Days > 0)
                {
                    if (ts.Days == 1) temp = " (+" + ts.TotalDays.ToString() + " day)"; else temp = " (+" + ts.TotalDays.ToString() + " days)";
                }
                if (model2.TemFlightTransit != "")
                {
                    //Show flight detail info with connecting flight on newly added control (20170208 - Sienny)
                    lbl_CarrierCodeIN.Text = model2.TemFlightCarrierCode;
                    lbl_FlightnumberIN.Text = model2.TemFlightFlightNumber;
                    lbl_DepartureIN.Text = model2.TemFlightDeparture;
                    lbl_DepartureDateIN.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", model2.TemFlightStd);
                    lbl_ArrivalIN.Text = model2.TemFlightTransit;
                    lbl_ArrivalDateIN.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", model2.TemFlightSta);

                    lbl_CarrierCodeIN2.Text = model2.TemFlightCarrierCode2;
                    lbl_FlightnumberIN2.Text = model2.TemFlightFlightNumber2;
                    lbl_DepartureIN2.Text = model2.TemFlightTransit;
                    lbl_DepartureDateIN2.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", model2.TemFlightStd2);
                    lbl_ArrivalIN2.Text = model2.TemFlightArrival;
                    lbl_ArrivalDateIN2.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", model2.TemFlightSta2) + temp;
                }
                else
                {
                    //Show flight detail info without connecting flight on newly added control (20170208 - Sienny)
                    connectWrapperReturn.Visible = false;

                    lbl_CarrierCodeIN.Text = model2.TemFlightCarrierCode;
                    lbl_FlightnumberIN.Text = model2.TemFlightFlightNumber;
                    lbl_DepartureIN.Text = model2.TemFlightDeparture;
                    lbl_DepartureDateIN.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", model2.TemFlightStd);
                    lbl_ArrivalIN.Text = model2.TemFlightArrival;
                    lbl_ArrivalDateIN.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", model2.TemFlightSta) + temp;
                }

                //lbl_ArrivalIN.Text = model2.TemFlightArrival; // objGeneral.GetCityNameByCode(model2.TemFlightArrival) + "(" + model2.TemFlightArrival + ")";

                //lbl_CarrierCodeIN.Text = model2.TemFlightCarrierCode;

                //lbl_DepartureDateIN.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", model2.TemFlightStd);

                //lbl_DepartureIN.Text = model2.TemFlightDeparture; // objGeneral.GetCityNameByCode(model2.TemFlightDeparture) + "(" + model2.TemFlightDeparture + ")";

                lbl_FlightnumberIN.Text = model2.TemFlightFlightNumber;
                lbl_GuestNumIN.Text = model2.TemFlightADTNum.ToString();
                lbl_PaxNumIN.Text = model2.TemFlightPaxNum.ToString();
                lbl_ChildNumIN.Text = model2.TemFlightCHDNum.ToString();

                if (model2.TemFlightTransit != "")
                {
                    tempdate1 = String.Format("{0:MM/dd/yyyy}", model2.TemFlightSta2);
                    tempdate2 = String.Format("{0:MM/dd/yyyy}", model2.TemFlightStd2);
                    ts = Convert.ToDateTime(tempdate1) - Convert.ToDateTime(tempdate2);

                    temp = "";
                    if (ts.Days > 0)
                        temp = " " + ts.TotalDays.ToString() + " day";

                    string tempDateStd = String.Format("{0:HHmm}", model2.TemFlightStd2);
                    string tempDateSta = String.Format("{0:HHmm}", model2.TemFlightSta2) + temp;
                    string transitAt = model2.TemFlightTransit; // objGeneral.GetCityNameByCode(model2.TemFlightTransit);
                    LblTransitReturn.Text = "Transit At " + transitAt + " (" + tempDateStd + " - " + tempDateSta + ")" + " Flight " + model2.TemFlightCarrierCode2 + model2.TemFlightFlightNumber2;
                }

            }
            else
            {
                //Flight detail info on newly added control (20170208 - Sienny)
                mainReturnConnectWrapper.Visible = false;

                //tr_Return.Visible = false;
                //td_Return.Visible = false;
                //trReturnIcon.Visible = false;
            }

        }

        protected void LoadData()
        {
            String TransID = "";
            lblAgentName.Text = MyUserSet.AgentName;
            if (Session["OrganizationName"] != null)
            {
                lblAgentOrg.Text = Session["OrganizationName"].ToString();
            }

            if (HttpContext.Current.Session["TransMain"] != null)
            {
                DataTable dtTransMain = (DataTable)HttpContext.Current.Session["TransMain"];
                TransID = dtTransMain.Rows[0]["TransID"].ToString();
            }
            lstbookFlightInfo = objBooking.GetAllBK_TRANSDTLFlightGrpNoSellKey1(TransID, 1);
            Session["lstbookFlightInfo"] = lstbookFlightInfo;
            //rptFlightDetails.DataSource = Session["lstbookFlightInfo"];
            //rptFlightDetails.DataBind();


            mainReturnConnectWrapper.Visible = false;
            //check is not nothing
            if (lstbookFlightInfo != null && lstbookFlightInfo.Count > 0)
            {
                //print all depart
                BookingTransactionDetail DepartDetail = lstbookFlightInfo[0];

                string tempdateA = String.Format("{0:MM/dd/yyyy}", DepartDetail.ArrivalDate);
                if (DepartDetail.Transit != "")
                    tempdateA = String.Format("{0:MM/dd/yyyy}", DepartDetail.ArrivalDate2);
                string tempdateD = String.Format("{0:MM/dd/yyyy}", DepartDetail.DepatureDate);
                TimeSpan ts = Convert.ToDateTime(tempdateA) - Convert.ToDateTime(tempdateD);
                string temp = "";
                if (ts.Days > 0)
                {
                    if (ts.Days == 1) temp = " (+" + ts.TotalDays.ToString() + " day)"; else temp = " (+" + ts.TotalDays.ToString() + " days)";
                }

                lbl_CarrierCodeOut.Text = DepartDetail.CarrierCode;
                lbl_FlightnumberOut.Text = DepartDetail.FlightNo;

                //check if transit
                if (DepartDetail.Transit != "")
                {
                    connectWrapperDepart.Visible = true;

                    lbl_DepartureOut.Text = DepartDetail.Origin;
                    lbl_DepartureDateOut.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", DepartDetail.DepatureDate);
                    lbl_ArrivalOut.Text = DepartDetail.Transit;
                    lbl_ArrivalDateOut.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", DepartDetail.ArrivalDate);

                    lbl_DepartureOut2.Text = DepartDetail.Transit;
                    lbl_DepartureDateOut2.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", DepartDetail.DepatureDate2);
                    lbl_ArrivalOut2.Text = DepartDetail.Destination;
                    lbl_ArrivalDateOut2.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", DepartDetail.ArrivalDate2) + temp;
                }
                else
                {
                    connectWrapperDepart.Visible = false;

                    lbl_DepartureOut.Text = DepartDetail.Origin;
                    lbl_DepartureDateOut.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", DepartDetail.DepatureDate);
                    lbl_ArrivalOut.Text = DepartDetail.Destination;
                    lbl_ArrivalDateOut.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", DepartDetail.ArrivalDate) + temp;
                }

                //check if have return
                if (lstbookFlightInfo.Count > 1)
                {
                    mainReturnConnectWrapper.Visible = true;

                    BookingTransactionDetail ReturnDetail = lstbookFlightInfo[1];

                    if (ReturnDetail.Transit != "")
                        tempdateA = String.Format("{0:MM/dd/yyyy}", ReturnDetail.ArrivalDate2);
                    else
                        tempdateA = String.Format("{0:MM/dd/yyyy}", ReturnDetail.ArrivalDate);
                    tempdateD = String.Format("{0:MM/dd/yyyy}", ReturnDetail.DepatureDate);
                    ts = Convert.ToDateTime(tempdateA) - Convert.ToDateTime(tempdateD);
                    //lblDateReturn.Text = String.Format("{0:dddd, dd MMMM yyyy}", model2.TemFlightStd);
                    temp = "";
                    if (ts.Days > 0)
                    {
                        if (ts.Days == 1) temp = " (+" + ts.TotalDays.ToString() + " day)"; else temp = " (+" + ts.TotalDays.ToString() + " days)";
                    }

                    lbl_CarrierCodeIN.Text = ReturnDetail.CarrierCode;
                    lbl_FlightnumberIN.Text = ReturnDetail.FlightNo;

                    //check if transit
                    if (ReturnDetail.Transit != "")
                    {
                        connectWrapperReturn.Visible = true;

                        lbl_DepartureIN.Text = ReturnDetail.Origin;
                        lbl_DepartureDateIN.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", ReturnDetail.DepatureDate);
                        lbl_ArrivalIN.Text = ReturnDetail.Transit;
                        lbl_ArrivalDateIN.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", ReturnDetail.ArrivalDate);

                        lbl_DepartureIN2.Text = ReturnDetail.Transit;
                        lbl_DepartureDateIN2.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", ReturnDetail.DepatureDate2);
                        lbl_ArrivalIN2.Text = ReturnDetail.Destination;
                        lbl_ArrivalDateIN2.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", ReturnDetail.ArrivalDate2) + temp;
                    }
                    else
                    {
                        connectWrapperReturn.Visible = false;

                        lbl_DepartureIN.Text = ReturnDetail.Origin;
                        lbl_DepartureDateIN.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", ReturnDetail.DepatureDate);
                        lbl_ArrivalIN.Text = ReturnDetail.Destination;
                        lbl_ArrivalDateIN.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", ReturnDetail.ArrivalDate) + temp;
                    }
                }
            }
        }

    }
}