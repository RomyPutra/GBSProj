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
using ABS.GBS.Log;

namespace GroupBooking.Web.Booking
{
    public partial class ReviewFareChange : System.Web.UI.Page
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

        string SellSessionID = "";
        bool ReturnOnly = false;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ReturnOnly = false;//added by diana 20140424, selling return only, testing

            if (Session["ReturnOnly"].ToString() == "true")
            {
                ReturnOnly = true;
            }
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
                setValue();
                CheckReturnForSaving();
                SavingProcess();
            }
            else
            {
                setValue();
                CheckReturnForSaving();// add by agus
            }
            //lblPNR.Text = Session["RecordLocator"].ToString();
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
                tmrCount.Enabled = false;
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
                    objBooking.UpdateTemFlight(model, model2, "", ref sessID, true, decimal.Parse(Session["totalPrevious"].ToString()), decimal.Parse(Session["totalReturnPrevious"].ToString()), ReturnOnly);
                    Session["signatureSess"] = sessID;
                    string signature = Session["signatureSess"].ToString();
                    SellFlight();

                    BindData(OutID, InID, sessID);

                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this,ex);
            }
        }

        private void SellFlight()
        {
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

                ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
                ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight2 = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();

                string strExpr;
                string strSort;
                DataTable dt = new DataTable();
                strExpr = "TemFlightId = '" + departID + "'";
                strSort = "";

                dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                FillModelFromDataRow(foundRows, ref  temFlight);

                string getfare = temFlight.TemFlightServiceCharge.ToString();

                if (ReturnID != "")
                {
                    strExpr = "TemFlightId = '" + ReturnID + "'";
                    strSort = "";

                    dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                    foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                    FillModelFromDataRow(foundRows, ref  temFlight2);

                    string LoginType = "PublicAgent";
                    string LoginName = MyUserSet.AgentName;
                    string LoginPWD = "";
                    string LoginDomain = "";

                    if (LoginType == "SkyAgent")
                    {
                        SellSessionID = absNavitaire.AgentLogon("SkyAgent", LoginDomain, LoginName, LoginPWD);
                    }
                    else
                    {
                        SellSessionID = absNavitaire.AgentLogon();
                    }
                    HttpContext.Current.Session.Remove("SellSessionID");
                    HttpContext.Current.Session.Add("SellSessionID", SellSessionID);//insert into Session

                    //getbooking, cancel, sell, and commit using same session
                    absNavitaire.GetBookingResponseByPNRSignature(Session["RecordLocator"].ToString(), SellSessionID);//getbooking

                    string errMessage = "";

                    absNavitaire.CancelJourney(Session["RecordLocator"].ToString(), -200, "MYR", SellSessionID, ref errMessage, false, ReturnOnly); //cancel journey using same session
                    Session["PaxMatch"] = "true";

                    bool SellJourney;
                    if (ReturnOnly == false)
                    {
                        SellJourney = objBooking.SellJourney(temFlight, temFlight2, LoginType, LoginName, LoginPWD, LoginDomain, SellSessionID, "", true);
                    }
                    else
                    {
                        SellJourney = objBooking.SellJourneyReturn(temFlight, temFlight2, LoginType, LoginName, LoginPWD, LoginDomain, SellSessionID, true, ReturnOnly);
                    }
                    if (SellJourney == false)//sell journey using same session
                    {
                        sErrmsg.InnerHtml = "No fare available for agent rules.";
                        btShowModal.Enabled = false;
                        btShowModal.Visible = false;
                        Session["PaxMatch"] = "false";
                        Response.Redirect(Shared.MySite.PublicPages.SelectFlightChange, false);
                    }
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
                    if (LoginType == "SkyAgent")
                    {
                        SellSessionID = absNavitaire.AgentLogon("SkyAgent", LoginDomain, LoginName, LoginPWD);
                    }
                    else
                    {
                        SellSessionID = absNavitaire.AgentLogon();
                    }
                    HttpContext.Current.Session.Remove("SellSessionID");
                    HttpContext.Current.Session.Add("SellSessionID", SellSessionID);//insert into Session

                    //getbooking, cancel, sell, and commit using same session
                    absNavitaire.GetBookingResponseByPNRSignature(Session["RecordLocator"].ToString(), SellSessionID);//getbooking

                    string errMessage = "";

                    absNavitaire.CancelJourney(Session["RecordLocator"].ToString(), -200, "MYR", SellSessionID, ref errMessage, false); //cancel journey using same session
                    if (objBooking.SellFlightByTem(temFlight, LoginType, LoginName, LoginPWD, LoginDomain, SellSessionID, "", true) == false)
                    {
                        sErrmsg.InnerHtml = "No fare available for agent rules.";
                        btShowModal.Enabled = false;
                        btShowModal.Visible = false;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "PostbackKey", "<script type='text/javascript'>window.alert('Requested fare is being adjusted, kindly try again.');</script>");
                        Response.Redirect(Shared.MySite.PublicPages.AgentMain, false);
                    }

                }
            }
            catch
            {

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
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this,ex);
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
            if (MyUserSet.CounterTimer != null)
            {
                ctrHdn.Value = MyUserSet.CounterTimer.ToString();
            }
            else
            {
                //default 
                ctrHdn.Value = "30";
            }

            HttpCookie cookie = Request.Cookies["cookieTemFlight"];
            if (cookie != null)
            {
                //load value review                
                string DepartID = cookie.Values["list1ID"].ToString();
                if (DepartID != string.Empty)
                {

                    ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();

                    temFlight = GetTemFlight(DepartID);

                    lblDepartFare.Text = (objGeneral.RoundUp(temFlight.TemFlightTotalAmount)).ToString("N", nfi);
                    lblDepartCurrency.Text = temFlight.TemFlightCurrencyCode.ToString();
                }
                else
                {
                    Response.Redirect(Shared.MySite.PublicPages.Searchflight);
                }

                if (cookie.Values["ReturnID"] != "")
                {
                    string ReturnID = cookie.Values["ReturnID"].ToString();
                    if (ReturnID != string.Empty)
                    {

                        ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();

                        temFlight = GetTemFlight(ReturnID);

                        lblReturnFare.Text = (objGeneral.RoundUp(temFlight.TemFlightTotalAmount)).ToString("N", nfi);
                        lblReturnCurrency.Text = temFlight.TemFlightCurrencyCode.ToString();
                    }
                }
                else
                {
                    lblReturnFare.Text = "0";
                    lblReturnCurrency.Text = lblDepartCurrency.Text;
                }

                lblTotPax.Text = cookie.Values["PaxNum"].ToString();
                if (lbl_ChildNumout.Text == "") lbl_ChildNumout.Text = "0";
                if (Convert.ToInt32(lbl_ChildNumout.Text) > 0)
                {
                    ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
                    temFlight = GetTemFlight(DepartID);
                    //if (temFlight.TemFlightDeparture.ToString().ToUpper() == "HKG" || temFlight.TemFlightArrival.ToString().ToUpper() == "HKG")
                    //{
                        trChildTax.Visible = true;
                        lblTextTaxFareDepart.Text = "Airport Tax (Adult) : ";
                        lblTextTaxFareReturn.Text = "Airport Tax (Adult) : ";
                    //}
                    lblDetailPax.Text = lbl_GuestNumout.Text + " Adult / " + lbl_ChildNumout.Text + " Child";
                }
                else
                {
                    lblDetailPax.Text = lbl_GuestNumout.Text + " Adult";
                }
            }

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
                log.Error(this,ex);
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

            string tempdate1 = String.Format("{0:MM/dd/yyyy}", model.TemFlightSta);
            if (model.TemFlightTransit != "")
                tempdate1 = String.Format("{0:MM/dd/yyyy}", model.TemFlightSta2);
            string tempdate2 = String.Format("{0:MM/dd/yyyy}", model.TemFlightStd);
            lblDateDepart.Text = String.Format("{0:dddd, dd MMMM yyyy}", model.TemFlightStd);
            TimeSpan ts = Convert.ToDateTime(tempdate1) - Convert.ToDateTime(tempdate2);
            string temp = "";
            if (ts.Days > 0)
            {
                if (ts.Days == 1) temp = " (+" + ts.TotalDays.ToString() + " day)"; else temp = " (+" + ts.TotalDays.ToString() + " days)";
            }
            if (model.TemFlightTransit != "")
                lbl_ArrivalDateout.Text = String.Format("{0:HHmm}", model.TemFlightSta2) + temp;
            else
                lbl_ArrivalDateout.Text = String.Format("{0:HHmm}", model.TemFlightSta) + temp;

            lbl_ArrivalOut.Text = objGeneral.GetCityNameByCode(model.TemFlightArrival) + "(" + model.TemFlightArrival + ")";

            lbl_CarrierCodeOut.Text = model.TemFlightCarrierCode;
            lbl_ChildNumout.Text = model.TemFlightCHDNum.ToString();

            lbl_DepartureDateout.Text = String.Format("{0:HHmm}", model.TemFlightStd);
            lbl_Departureout.Text = objGeneral.GetCityNameByCode(model.TemFlightDeparture) + "(" + model.TemFlightDeparture + ")";

            lbl_FlightnumberOut.Text = model.TemFlightFlightNumber;
            lbl_GuestNumout.Text = model.TemFlightADTNum.ToString();
            lbl_PaxNumout.Text = model.TemFlightPaxNum.ToString();

            ///amended by Diana,
            ///added divide with num of passenger to show single amount

            //breakdown charge and tax
            DataTable dtBDFee = objBooking.dtBreakdownFee();

            if (ReturnOnly == false)
            {
                dtBDFee = (DataTable)HttpContext.Current.Session["dataBDFeeDepart"];

                lblPaxFareDepart.Text = Convert.ToDecimal(dtBDFee.Rows[0]["Fare"]).ToString("N", nfi);

                //commented by diana 20131104
                //if (Convert.ToInt32(lbl_ChildNumout.Text) != 0)
                //{
                //    lblPaxFareDepart.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Fare"]) / 2).ToString("N", nfi);
                //}
                //else
                //{
                //    lblPaxFareDepart.Text = Convert.ToDecimal(dtBDFee.Rows[0]["Fare"]).ToString("N", nfi);
                //}

                lblTaxDepart.Text = Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]).ToString("N", nfi);
                if (Convert.ToInt32(lbl_ChildNumout.Text) != 0)
                {
                    lblTaxDepartChild.Text = Convert.ToDecimal(dtBDFee.Rows[0]["ChApt"]).ToString("N", nfi);
                }

                lblFuelDepart.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"]) / model.TemFlightPaxNum).ToString("N", nfi);
                lblSvcDepart.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Svc"]) / model.TemFlightPaxNum).ToString("N", nfi);
                lblOthDepart.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) / model.TemFlightPaxNum + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"]) / model.TemFlightPaxNum).ToString("N", nfi);
                //added by ketee
                lblVATDepart.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) / model.TemFlightPaxNum).ToString("N", nfi);
                if (Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) > 0)
                {
                    trVAT.Style["display"] = "";
                }
                //lblDetQtyDepart.Text = dtBDFee.Rows[0]["Qty"].ToString();

                /*
                lblPaxFareDepart.Text = Convert.ToDecimal(model.temFlightfarePrice).ToString("N", nfi);
                lblTaxDepart.Text = Convert.ToDecimal(model.TemFlightApt).ToString("N", nfi);
            
                lblFuelDepart.Text = Convert.ToDecimal(model.TemFlightFuel).ToString("N", nfi);
                lblSvcDepart.Text = Convert.ToDecimal(model.TemFlightServiceCharge).ToString("N", nfi);
                lblOthDepart.Text = Convert.ToDecimal(model.TemFlightOth).ToString("N", nfi);
                */

                if (decimal.Parse(lblSvcDepart.Text) <= 0)
                {
                    freeSVCF = true;
                }
            }
            else //if return only, then hide depart label and text
            {
                //tr_Depart.Visible = false;
                td_Depart.Visible = false;
                td_Depart2.Visible = false;
                td_Depart3.Visible = false;

                lblPaxFareDepart.Visible = false;
                lblTextPaxFareDepart.Visible = false;
                lblTextTaxFareDepart.Visible = false;
                lblTaxDepart.Visible = false;
                lblTextTaxFareDepartChild.Visible = false;
                lblTaxDepartChild.Visible = false;
                lblTextFuelDepart.Visible = false;
                lblFuelDepart.Visible = false;
                lblTextSvcDepart.Visible = false;
                lblSvcDepart.Visible = false;
                lblTextOthDepart.Visible = false;
                lblOthDepart.Visible = false;
                lblDepartFareText.Visible = false;
                lblDepartFare.Visible = false;
                lblDepartCurrency.Visible = false;
                lblTextVATDepart.Visible = false;
                lblVATDepart.Visible = false;
            }
            if (model.TemFlightTransit != "")
            {
                tempdate1 = String.Format("{0:MM/dd/yyyy}", model.TemFlightSta2);
                tempdate2 = String.Format("{0:MM/dd/yyyy}", model.TemFlightStd2);
                ts = Convert.ToDateTime(tempdate1) - Convert.ToDateTime(tempdate2);

                temp = "";
                if (ts.Days > 0)
                    temp = " " + ts.TotalDays.ToString() + " day";

                string tempDateStd = String.Format("{0:HHmm}", model.TemFlightStd2);
                string tempDateSta = String.Format("{0:HHmm}", model.TemFlightSta2) + temp;
                string transitAt = objGeneral.GetCityNameByCode(model.TemFlightTransit);
                LblTransitDepart.Text = "Transit At " + transitAt + " (" + tempDateStd + " - " + tempDateSta + ")" + " Flight " + model.TemFlightCarrierCode2 + model.TemFlightFlightNumber2;
            }

            total = model.TemFlightTotalAmount;

            lblAverageFare.Text = (objGeneral.RoundUp(total / model.TemFlightPaxNum)).ToString("N", nfi);

            if (InID != "")
            {
                //tdreturnfare.Visible = true;
                lblReturnFareText.Visible = true;
                lblReturnFare.Visible = true;
                lblReturnCurrency.Visible = true;
                td_Return.Visible = true;
                lblPaxFareReturn.Visible = true;
                lblTextPaxFareReturn.Visible = true;
                lblTextTaxFareReturn.Visible = true;
                lblTaxReturn.Visible = true;
                lblTaxReturnChild.Visible = true;
                lblTextTaxFareReturnChild.Visible = true;
                lblTextFuelReturn.Visible = true;
                lblFuelReturn.Visible = true;
                lblTextSvcReturn.Visible = true;
                lblSvcReturn.Visible = true;
                lblTextOthReturn.Visible = true;
                lblOthReturn.Visible = true;

                model2 = GetTemFlight(InID);

                if (model2.TemFlightTransit != "")
                    tempdate1 = String.Format("{0:MM/dd/yyyy}", model2.TemFlightSta2);
                else
                    tempdate1 = String.Format("{0:MM/dd/yyyy}", model2.TemFlightSta);
                tempdate2 = String.Format("{0:MM/dd/yyyy}", model2.TemFlightStd);
                ts = Convert.ToDateTime(tempdate1) - Convert.ToDateTime(tempdate2);
                lblDateReturn.Text = String.Format("{0:dddd, dd MMMM yyyy}", model2.TemFlightStd);
                temp = "";
                if (ts.Days > 0)
                {
                    if (ts.Days == 1) temp = " (+" + ts.TotalDays.ToString() + " day)"; else temp = " (+" + ts.TotalDays.ToString() + " days)";
                }
                if (model2.TemFlightTransit != "")
                    lbl_ArrivalDateIN.Text = String.Format("{0:HHmm}", model2.TemFlightSta2) + temp;
                else
                    lbl_ArrivalDateIN.Text = String.Format("{0:HHmm}", model2.TemFlightSta) + temp;

                lbl_ArrivalIN.Text = objGeneral.GetCityNameByCode(model2.TemFlightArrival) + "(" + model2.TemFlightArrival + ")";

                lbl_CarrierCodeIN.Text = model2.TemFlightCarrierCode;
                lbl_ChildNumIN.Text = model2.TemFlightCHDNum.ToString();

                lbl_DepartureDateIN.Text = String.Format("{0:HHmm}", model2.TemFlightStd);

                lbl_DepartureIN.Text = objGeneral.GetCityNameByCode(model2.TemFlightDeparture) + "(" + model2.TemFlightDeparture + ")";

                lbl_FlightnumberIN.Text = model2.TemFlightFlightNumber;
                lbl_GuestNumIN.Text = model2.TemFlightADTNum.ToString();
                lbl_PaxNumIN.Text = model2.TemFlightPaxNum.ToString();

                ///amended by Diana,
                ///added divide with num of passenger to show single amount

                //breakdown tax and charge                
                dtBDFee = (DataTable)HttpContext.Current.Session["dataBDFeeReturn"];

                lblPaxFareReturn.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Fare"])).ToString("N", nfi);
                //commented by diana 20131104
                //if (Convert.ToInt32(lbl_ChildNumIN.Text) != 0)
                //{
                //    lblPaxFareReturn.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Fare"]) / 2).ToString("N", nfi);
                //}
                //else
                //{
                //    lblPaxFareReturn.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Fare"])).ToString("N", nfi);
                //}  

                lblTaxReturn.Text = Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]).ToString("N", nfi);
                if (Convert.ToInt32(lbl_ChildNumIN.Text) != 0)
                {
                    lblTaxReturnChild.Text = Convert.ToDecimal(dtBDFee.Rows[0]["ChApt"]).ToString("N", nfi);
                }

                lblFuelReturn.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"]) / model2.TemFlightPaxNum).ToString("N", nfi);
                lblSvcReturn.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Svc"]) / model2.TemFlightPaxNum).ToString("N", nfi);
                lblOthReturn.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) / model2.TemFlightPaxNum + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"]) / model2.TemFlightPaxNum).ToString("N", nfi);
                //added byektee
                lblVATReturn.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) / model2.TemFlightPaxNum).ToString("N", nfi);
                //lblDetQtyReturn.Text = dtBDFee.Rows[0]["Qty"].ToString();
                //lblPaxFareReturn.Text = Convert.ToDecimal(model2.temFlightfarePrice).ToString("N", nfi);
                //lblTaxReturn.Text = Convert.ToDecimal(model2.TemFlightApt).ToString("N", nfi);

                /*
                lblTaxChargeReturn.Text = Convert.ToDecimal(model2.TemFlightApt).ToString("N", nfi);
                lblFuelChargeReturn.Text = Convert.ToDecimal(model2.TemFlightFuel).ToString("N", nfi);
                lblServiceChargeReturn.Text = Convert.ToDecimal(model2.TemFlightServiceCharge).ToString("N", nfi);
                lblOtherChargeReturn.Text = Convert.ToDecimal(model2.TemFlightOth).ToString("N", nfi);
                */

                if (freeSVCF)
                {
                    if (decimal.Parse(lblSvcReturn.Text) > 0)
                    {
                        freeSVCF = false;
                    }
                }

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
                    string transitAt = objGeneral.GetCityNameByCode(model2.TemFlightTransit);
                    LblTransitReturn.Text = "Transit At " + transitAt + " (" + tempDateStd + " - " + tempDateSta + ")" + " Flight " + model2.TemFlightCarrierCode2 + model2.TemFlightFlightNumber2;
                }

                //model2.TemFlightTotalAmount = totAmountReturn;
                //total += model2.TemFlightTotalAmount;
                total += model2.TemFlightTotalAmount;

                lblAverageFare.Text = objGeneral.RoundUp(total / model.TemFlightPaxNum).ToString("N", nfi);
            }
            else
            {
                tr_Return.Visible = false;
                td_Return.Visible = false;
                lblPaxFareReturn.Visible = false;
                lblTextPaxFareReturn.Visible = false;
                lblTextTaxFareReturn.Visible = false;
                lblTaxReturn.Visible = false;
                lblTextTaxFareReturnChild.Visible = false;
                lblTaxReturnChild.Visible = false;
                lblTextFuelReturn.Visible = false;
                lblFuelReturn.Visible = false;
                lblTextSvcReturn.Visible = false;
                lblSvcReturn.Visible = false;
                lblTextOthReturn.Visible = false;
                lblOthReturn.Visible = false;
                lblReturnFareText.Visible = false;
                lblReturnFare.Visible = false;
                lblReturnCurrency.Visible = false;
                lblTextVATReturn.Visible = false;
                lblVATReturn.Visible = false;
            }

            lblTotalFare.Text = objGeneral.RoundUp(total).ToString("N", nfi);
            lblTotalCurrency.Text = model.TemFlightCurrencyCode;
            lblTotFareCurrency.Text = model.TemFlightCurrencyCode;

            #region prevTax
            /*
            //create temporary table for tax and charge
            DataTable dtTaxCharge = new DataTable();
            dtTaxCharge.Columns.Add("TemFlightId");
            dtTaxCharge.Columns.Add("TemFlightHeader");
            dtTaxCharge.Columns.Add("TemFlightCurrencyCode");
            dtTaxCharge.Columns.Add("TemFlightAPT");
            dtTaxCharge.Columns.Add("TemFlightFuel");
            dtTaxCharge.Columns.Add("TemFlightServiceCharge");
            dtTaxCharge.Columns.Add("TemFlightOth");
            dtTaxCharge.Columns.Add("TemFlightFarePrice");
            dtTaxCharge.Columns.Add("TemFlightFullTax");

            DataRow row;
            row = dtTaxCharge.NewRow();
            row["TemFlightId"] = model.TemFlightId;
            row["TemFlightHeader"] = "Depart";
            row["TemFlightCurrencyCode"] = model.TemFlightCurrencyCode;
            row["TemFlightAPT"] = Convert.ToDecimal(model.TemFlightApt).ToString("N", nfi);
            row["TemFlightFuel"] = Convert.ToDecimal(model.TemFlightFuel).ToString("N", nfi);
            row["TemFlightServiceCharge"] = Convert.ToDecimal(model.TemFlightServiceCharge).ToString("N", nfi);
            row["TemFlightOth"] = Convert.ToDecimal(model.TemFlightOth).ToString("N", nfi);
            row["TemFlightFarePrice"] = Convert.ToDecimal(model.temFlightfarePrice).ToString("N", nfi);
            row["TemFlightFullTax"] = Convert.ToDecimal(model.TemFlightApt + model.TemFlightFuel + model.TemFlightServiceCharge + model.TemFlightOth).ToString("N", nfi);
            dtTaxCharge.Rows.Add(row);

            if (InID != "")
            {
                row = dtTaxCharge.NewRow();
                row["TemFlightId"] = model2.TemFlightId;
                row["TemFlightHeader"] = "Return";
                row["TemFlightCurrencyCode"] = model2.TemFlightCurrencyCode;
                row["TemFlightAPT"] = Convert.ToDecimal(model2.TemFlightApt).ToString("N", nfi);
                row["TemFlightFuel"] = Convert.ToDecimal(model2.TemFlightFuel).ToString("N", nfi);
                row["TemFlightServiceCharge"] = Convert.ToDecimal(model2.TemFlightServiceCharge).ToString("N", nfi);
                row["TemFlightOth"] = Convert.ToDecimal(model2.TemFlightOth).ToString("N", nfi);
                row["TemFlightFarePrice"] = Convert.ToDecimal(model.temFlightfarePrice).ToString("N", nfi);
                row["TemFlightFullTax"] = Convert.ToDecimal(model2.TemFlightApt + model2.TemFlightFuel + model2.TemFlightServiceCharge + model2.TemFlightOth).ToString("N", nfi);
                dtTaxCharge.Rows.Add(row);
            }
          
            HttpContext.Current.Session.Remove("dtTax");
            HttpContext.Current.Session.Add("dtTax", dtTaxCharge);
            */
            #endregion

            //remarked by diana 20140425, return got no SVCF
            //if (freeSVCF)
            //{
            //    log.Info(this,"Depart SVCF : " + lblSvcDepart.Text + "; Return SVCF : " + lblSvcReturn.Text);
            //    Session["SVCFAvailable"] = "false";
            //    Response.Redirect(Shared.MySite.PublicPages.Searchflight, false);
            //}

            HttpContext.Current.Session.Remove("Fare");
            Hashtable htFare = new Hashtable();
            htFare.Add("Avg", lblAverageFare.Text);
            htFare.Add("Dpt", model.TemFlightTotalAmount);
            htFare.Add("Rtn", model2.TemFlightTotalAmount);
            HttpContext.Current.Session.Add("Fare", htFare);
        }

        protected void btn_Next_Click(object sender, EventArgs e)
        {
            Response.Redirect(Shared.MySite.PublicPages.ReviewBooking);
        }

        protected void SavingProcess()
        {
            try
            {


                /*
                if (SaveData())
                    e.Result = "";
                else
                    e.Result = msgList.Err100031;
                */

                SaveData();
                //Session["PaxStatus"] = "false"; //testing purpose
                //added by diana 20140124 - check if pax total <> initial pax, then do not proceed further
                if (Session["PaxStatus"] != null)
                {
                    if (Session["PaxStatus"].ToString() == "false")
                    {
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "PostbackKey", "<script type='text/javascript'>window.alert('Total Seat is not enough for your booking pax. Kindly rebook the flight.');</script>");
                        Response.Redirect(Shared.MySite.PublicPages.Searchflight, false);
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //e.Result = msgList.Err100013;
            }
        }

        protected bool SaveData()
        {

            ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();

            string strExpr;
            string strSort;
            string keyCarrier = "";
            decimal totalOth = 0; //service charge total
            decimal totalDisc = 0; //discount charge total

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
            DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
            FillModelFromDataRow(foundRows, ref  temFlight);

            departDate = Convert.ToDateTime(temFlight.TemFlightStd);

            Currency = temFlight.TemFlightCurrencyCode.Trim();

            agent = objAgentProfile.GetSingleAgentProfile(MyUserSet.AgentName);

            string LoginType = MyUserSet.AgentType.ToString();

            int m = 0;
            int count = 0;
            DataTable dtClass = objBooking.dtClass();
            if (HttpContext.Current.Session["dataClass"] != null) { dtClass = (DataTable)HttpContext.Current.Session["dataClass"]; }
            count = dtClass.Rows.Count;

            byte seqNo = 1;
            List<BookingTransactionMain> TransMain = new List<BookingTransactionMain>();

            //tranID = DateTime.Now.ToString("yyyyMMddHHmmsss");
            tranID = Session["TransID"].ToString();

            #region newsavedetail
            //Datatable Process 

            //string PNR = book.BookingCommit(agent, temClass.TemClassPersonNumber, temClass.TemClassClassPrice.Value, temClass.TemClassSellSignature);

            ArrayList SellSignature = new ArrayList();
            List<BookingTransactionDetail> listBookDTLInfo = new List<BookingTransactionDetail>();

            DataTable dataClass = objBooking.dtClass();
            dataClass = (DataTable)HttpContext.Current.Session["dataClass"];
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
                totalDisc += bookDTLInfo.LineDisc;

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
                if (dr["ServVAT"] == null)
                {
                    dr["ServVAT"] = 0;
                }
                else if (dr["ServVAT"].ToString() == "")
                {
                    dr["ServVAT"] = 0;
                }
                bookDTLInfo.LineVAT = Convert.ToDecimal(dr["ServVAT"].ToString());
                totalServVAT += bookDTLInfo.LineVAT;

                totalServiceFee += bookDTLInfo.LineFee;

                bookDTLInfo.FareClass = dr["FareClass"].ToString();
                bookDTLInfo.PaxAdult = Convert.ToInt16(dr["AdultPax"].ToString());
                bookDTLInfo.PaxChild = Convert.ToInt16(dr["ChildPax"].ToString());
                bookDTLInfo.LineTotal = Convert.ToDecimal(dr["FullPrice"].ToString());
                bookDTLInfo.SyncLastUpd = DateTime.Now;
                bookDTLInfo.LastSyncBy = MyUserSet.AgentID;

                //bookDTLInfo.FarePerPax = Convert.ToDecimal(dr["FarePrice"].ToString());
                bookDTLInfo.FarePerPax = Convert.ToDecimal(dr["FarePrice"].ToString()) / Convert.ToDecimal(dr["Quantity"].ToString());

                bookDTLInfo.LineTax = Convert.ToDecimal(dr["TaxChrg"].ToString()) + Convert.ToDecimal(dr["FuelChrg"].ToString()); //apt + fuel

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

                bookDTLInfo.CollectedAmount = 0;
                bookDTLInfo.Signature = dr["SellSignature"].ToString();

                //added by ketee 20130625
                //midchange = from CurrencyRate to ExchgRate

                if (dr["CurrencyRate"] == null)
                {
                    dr["CurrencyRate"] = 1;
                }
                else if (dr["CurrencyRate"].ToString() == "")
                {
                    dr["CurrencyRate"] = 1;
                }
                currencyRate = Convert.ToDecimal(dr["CurrencyRate"]);

                // objBooking.SaveBK_TRANSDTL(bookDTLInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
                lstbookDTLInfo.Add(bookDTLInfo);

                APT += bookDTLInfo.LineTax;

                int index = SellSignature.IndexOf(bookDTLInfo.Signature);
                if (index >= 0)
                {
                    listBookDTLInfo[index].LineTotal += bookDTLInfo.LineTotal;
                    listBookDTLInfo[index].CollectedAmount += bookDTLInfo.CollectedAmount;
                    listBookDTLInfo[index].LineFee += bookDTLInfo.LineFee;
                    listBookDTLInfo[index].LineTax += bookDTLInfo.LineTax;
                    listBookDTLInfo[index].LineOth += bookDTLInfo.LineOth;
                    listBookDTLInfo[index].LineDisc += bookDTLInfo.LineDisc;
                }
                else
                {
                    SellSignature.Add(bookDTLInfo.Signature);
                    listBookDTLInfo.Add(bookDTLInfo);
                }
            }
            // end datatable
            #endregion

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

            ht.Add("Expiry", bookHDRInfo.ExpiryDate);

            bookHDRInfo.TransTotalPAX = Convert.ToInt16(lblTotPax.Text);
            bookHDRInfo.CollectedAmt = 0;

            bookHDRInfo.TransTotalAmt = totalFlightFare;
            bookHDRInfo.TransSubTotal = totalFlightFare;
            bookHDRInfo.TransTotalTax = APT;
            bookHDRInfo.TransTotalFee = totalServiceFee;
            bookHDRInfo.TransTotalOth = totalOth;
            bookHDRInfo.TransTotalDisc = totalDisc;
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

            //load fare

            if (HttpContext.Current.Session["Fare"] != null)
            {
                string a = "";
            }
            Hashtable htFare = (Hashtable)HttpContext.Current.Session["Fare"];
            decimal avg = Convert.ToDecimal(htFare["Avg"]);
            decimal dpt = Convert.ToDecimal(htFare["Dpt"]);
            decimal rtn = Convert.ToDecimal(htFare["Rtn"]);

            //bookHDRInfo.TotalAmtAVG = Convert.ToDecimal(lblAverageFare.Text);
            bookHDRInfo.TotalAmtAVG = avg;

            bookHDRInfo.TotalAmtGoing = dpt;
            if (LblReturn.Text != "")
            { bookHDRInfo.TotalAmtReturn = rtn; }
            else
            { bookHDRInfo.TotalAmtReturn = 0; }

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

            HttpContext.Current.Session.Remove("HashMain");
            HttpContext.Current.Session.Add("HashMain", ht);

            //end save header

            //added by ketee
            BookingTransactionMain BookingMain = new BookingTransactionMain();
            //BookingMain = objBooking.SaveBooking(bookHDRInfo, lstbookDTLInfo, transTaxInfo, lsttransFeesInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);

            if (bookHDRInfo != null) //amended by diana 20140124 - check for equal total pax
            {
                //if (bookHDRInfo.TransTotalPAX < int.Parse(HttpContext.Current.Session["TotalPax"].ToString()))
                //{
                //    Session["PaxStatus"] = "false";
                //    return false;
                //}
                //else
                //{
                    string TransID = Session["TransID"].ToString();
                    BookingTransactionMain previousBookHDRInfo = new BookingTransactionMain();
                    previousBookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(TransID);
                    objBooking.FillDataTableTransMain(bookHDRInfo, previousBookHDRInfo);

                    List<BookingTransactionDetail> listDetailCombinePNR = new List<BookingTransactionDetail>();
                    listDetailCombinePNR = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID,-1," RecordLocator='" + Session["RecordLocator"].ToString() + "' AND ",ReturnOnly);
                    objBooking.FillDataTableTransDetail(listBookDTLInfo, listDetailCombinePNR);

                    HttpContext.Current.Session.Remove("DetailList");
                    HttpContext.Current.Session.Add("DetailList", lstbookDTLInfo);

                    //BookingMain = objBooking.SaveBooking(bookHDRInfo, lstbookDTLInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
                    //if (BookingMain != null && BookingMain.TransID != "")
                    //    return true;
                    //else
                    //    return false;
                    return true;
                //}
            }
            return false;
            //objBooking.SaveBooking(bookHDRInfo, lstbookDTLInfo, transTaxInfo, lsttransFeesInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            //saving process
            tmrCount.Enabled = false;
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



    }
}