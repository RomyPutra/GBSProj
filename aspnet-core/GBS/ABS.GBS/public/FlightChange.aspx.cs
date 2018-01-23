using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//
using System.Data;
//
using ABS.Logic.GroupBooking.Agent;
using ABS.Logic.GroupBooking.Booking;
using SEAL.Data;
//
using DevExpress.Web;
using System.Collections;
using ABS.Logic.Shared;
//using log4net;
using ABS.Logic.GroupBooking;
using System.Globalization;
using System.Configuration;
using ABS.Navitaire.BookingManager;
using ABS.GBS.Log;
using StackExchange.Profiling;

namespace GroupBooking.Web.Booking
{
    public partial class FlightChange : System.Web.UI.Page
    {
        #region declaration
        UserSet MyUserSet;
        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
        ABS.Logic.GroupBooking.Agent.AgentControl objAgent = new ABS.Logic.GroupBooking.Agent.AgentControl();

        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
        List<BookingTransactionDetail> lstbookFlightInfo = new List<BookingTransactionDetail>();
        List<BookingTransactionDetail> lstRejectedbookDTLInfo = new List<BookingTransactionDetail>();
        List<BookingTransactionDetail> listDetailCombinePNR = new List<BookingTransactionDetail>();
        List<Bk_transssr> listbk_transssrinfo = new List<Bk_transssr>();
        BookingTransactionDetail bookDTLInfo = new BookingTransactionDetail();
        List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
        BookingTransactionMain bookHDRInfo = new BookingTransactionMain();
        decimal totalFlightFare, totalServiceFee, totalPaxFee, totalBaggageFare = 0, totalServVAT;
        ABS.Logic.GroupBooking.Agent.AgentProfile agent = new ABS.Logic.GroupBooking.Agent.AgentProfile();

        ABS.Logic.GroupBooking.Agent.AgentProfileControl objAgentProfile = new ABS.Logic.GroupBooking.Agent.AgentProfileControl();
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogControl log = new LogControl();
        SystemLog SystemLog = new SystemLog();
        ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition model;
        BookingEnquiry enqLogInfo = new BookingEnquiry();
        BookingSuspendList enqSusInfo = new BookingSuspendList();
        AgentBlacklistLog agBlacklistInfo = new AgentBlacklistLog();
        AgentActivity agActivityInfo = new AgentActivity();
        Settings newSys_Preft = new Settings();
        List<CODEMASTER> lstOpt = new List<CODEMASTER>();
        //added by Tyas 22/06/2015
        string bookingFrom = "";
        string bookingTo = "";
        string travelFrom = "";
        string travelTo = "";
        Boolean Active = false;
        string TransID = "";
        string PNR = "";

        string Currency = "USD";
        decimal APT = 0;
        decimal AVGFare = 0;
        string tranID = "";
        DataTable dtTaxFees = new DataTable();

        private static int num = 0;
        private static int departID = 0;
        private static string ReturnID = "";
        //end edded
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //return;
            string result = string.Empty;
            if (Session["AgentSet"] != null)
            { MyUserSet = (UserSet)Session["AgentSet"]; }
            else
            {
                if (!IsCallback)
                {
                    Response.Redirect(Shared.MySite.PublicPages.InvalidPage, false);
                }

            }
            TransID = Request.QueryString["TransID"];
            if (!IsPostBack && !IsCallback)
            {
                if (Request.QueryString["callback"] == null)
                {
                    tdBack.Style.Add("display", "none");
                    ClearSession();
                    ShowData();
                    //if (MyUserSet != null)
                    //{
                    //    objBooking.ClearExpiredJourney(MyUserSet.AgentID);
                    //}
                    //else
                    //{
                    //    Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.SessionExpired);
                    //}
                }

            }
            if (Request.QueryString["callback"] == "1")
            {
                tablePNR.Style.Add("display", "none");
                btBack.Style.Add("display", "inline-table");
                ShowData();

            }
           
            if (Request.QueryString["TransID"] != null && hfpnr.Value != "" && hfSearch.Value != "1")
            {
                tablePNR.Style.Add("display", "none");
                tdBack.Style.Add("display", "inline-table");
                PNR = hfpnr.Value;
                Session["PNR"] = hfpnr.Value;
                SearchData(Request.QueryString["TransID"].ToString(), hfpnr.Value);

            }
            else
            {
                hfSearch.Value = "0";
                btBack.Style.Add("display", "none");
            }
            if (Request.QueryString["mod"] != null && Request.QueryString["add"] != null && Request.QueryString["mod"].ToString() != "" && Request.QueryString["add"].ToString() != "")
            {
                SearchDate(Convert.ToInt16(Request.QueryString["mod"]), Convert.ToInt32(Request.QueryString["add"]));
            }
            else
            {
                InitializeForm();
            }

            initializeData();
            //ddlDeparture.Attributes.Add("onchange", "$.getScript('../Scripts/chosen.jquery.js', function () { SearchDatePanel.PerformCallback('loadReturnDate') });");
            //ddlReturn.Attributes.Add("onchange", "onChangeReturnSta();");



        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            ClearSession();
            initializeData();
            tablePNR.Style.Add("display", "block");
            tdBack.Style.Add("display", "none");
            btn_Next.Visible = false;
            btnContinueBottom.Visible = false;
            SelectFlightPanel.ClientVisible = false;
        }
        protected void ShowData()
        {
            try
            {
                //lblBookingID.Text = TransID;
                lblTransID.Text = TransID;

                DataTable dt;
                dt = objBooking.GetBK_AllPNR(TransID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    //LeftSide PNR List
                    Session["ListPNR"] = dt;
                    dt.DefaultView.RowFilter = "RecordLocator <> 'All' AND PNR <> 'All'";
                    rptPNR.DataSource = dt.DefaultView;
                    rptPNR.DataBind();
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                throw ex;
            }
            finally
            {
                objBooking = null;
            }
        }

        protected void InitializeForm()
        {
            HttpCookie cookie2 = Request.Cookies["cookieTemFlight"];
            if (cookie2 != null)
            {
                cookie2.Expires = DateTime.Today.AddDays(-1);
                Response.Cookies.Add(cookie2);
            }
            model = new ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition();
            //searchcondition model = new searchcondition();

            HttpCookie cookie = Request.Cookies["cookieSearchcondition"];
            if (cookie != null)
            {
                model.Carrier = cookie.Values["Carrier"];//   ViewState["Carrier"].ToString();
                model.Departure = cookie.Values["Departure"];// ViewState["Departure"].ToString();
                model.Arrival = cookie.Values["Arrival"];// ViewState["Arrival"].ToString();
                bool oneway = false;
                if (cookie.Values["ifOneWay"] == "TRUE")
                {
                    oneway = true;
                    tr_Return.Visible = false;
                }
                else
                {
                    model.ReturnDate = Convert.ToDateTime(cookie.Values["ReturnDate"]);
                }
                model.ifOneWay = oneway;//  Convert.ToBoolean(ViewState["ifOneWay"]);
                model.Currency = cookie.Values["Currency"];// ViewState["Currency"].ToString();
                model.DepartDate = Convert.ToDateTime(cookie.Values["DepartDate"]);// Convert.ToDateTime(ViewState["DepartDate"]);
                if (cookie.Values["ReturnDate"] != "")
                {
                    model.ReturnDate = Convert.ToDateTime(cookie.Values["ReturnDate"]);
                }//  Convert.ToDateTime(ViewState["ReturnDate"]);
                model.GuestNum = Convert.ToInt32(cookie.Values["GuestNum"]);//  Convert.ToInt32(ViewState["GuestNum"]);
                model.PaxNum = Convert.ToInt32(cookie.Values["PaxNum"]);
                model.ChildNum = Convert.ToInt32(cookie.Values["ChildNum"]);
                model.InfantNum = Convert.ToInt32(cookie.Values["InfantNum"]);

                string temp = "";
                temp = cookie.Values["DepartureDetail"];
                lbl_Go1.Text = temp.Replace("|", " | ");
                //lbl_Go1.Text = cookie.Values["DepartureDetail"];
                //temp = cookie.Values["ArrivalDetail"];
                //lbl_Go2.Text = temp.Replace("|", " | ");
                temp = cookie.Values["Arrival"];
                if (Session["dtCountryList"] != null)
                {
                    DataTable dt = (DataTable)Session["dtCountryList"];
                    DataRow[] resultBaggage = dt.Select("ArrivalStation = '" + temp + "'");
                    foreach (DataRow rows in resultBaggage)
                    {
                        if (cookie.Values["ArrivalDetail"] != "")
                        {
                            temp = cookie.Values["ArrivalDetail"] + " | " + rows[17].ToString();
                        }
                        else
                        {
                            temp = rows[19].ToString().Replace("|", " | ");
                        }

                        Session["Country"] = rows[9].ToString();
                    }
                }
                else
                {
                    DataTable dt = objGeneral.GetLookUpCity(cookie.Values["Departure"], Request.PhysicalApplicationPath);
                    DataRow[] resultBaggage = dt.Select("ArrivalStation = '" + temp + "'");
                    foreach (DataRow rows in resultBaggage)
                    {
                        if (cookie.Values["ArrivalDetail"] != "")
                        {
                            temp = cookie.Values["ArrivalDetail"] + " | " + rows[17].ToString();
                        }
                        else
                        {
                            temp = rows[19].ToString().Replace("|", " | ");
                        }

                        Session["Country"] = rows[9].ToString();
                    }
                }

                lbl_Go2.Text = temp;
                //lbl_Go2.Text = cookie.Values["ArrivalDetail"];

                lbl_Return1.Text = lbl_Go2.Text;
                //lbl_Return1.Text = cookie.Values["ArrivalDetail"];
                lbl_Return2.Text = lbl_Go1.Text;
                //lbl_Return2.Text = cookie.Values["DepartureDetail"];                
                objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                DataTable tempDt = objBooking.dtFlight();

                if (hfContinue.Value != "1")
                {
                    BindList1(model, tempDt);
                }
                if (oneway == false)
                {
                    if (hfContinue.Value != "1")
                    {
                        BindList2(model, tempDt);
                    }
                    if (dvSelectFlight.Items.Count <= 0 || gvSelectFlightReturn.Items.Count <= 0)
                    {
                        tr_Depart.Style.Add("display", "none");
                        tr_Return.Style.Add("display", "none");
                        tr_Return.Visible = false;
                        btn_Next.Visible = false;
                        btnContinueBottom.Visible = false;
                        //Response.Redirect(Shared.MySite.PublicPages.Selectflight);
                    }
                    else
                    {
                        tr_Depart.Style.Add("display", "block");
                        tr_Return.Style.Add("display", "block");
                        tr_Return.Visible = true;
                        btn_Next.Visible = true;
                        btnContinueBottom.Visible = true;
                    }
                }
                else
                {
                    if (dvSelectFlight.Items.Count <= 0)
                    {
                        tr_Depart.Style.Add("display", "none");
                        btn_Next.Visible = false;
                        btnContinueBottom.Visible = false;
                        //Response.Redirect(Shared.MySite.PublicPages.Selectflight);
                    }
                    else
                    {
                        tr_Depart.Style.Add("display", "block");
                        btn_Next.Visible = true;
                        btnContinueBottom.Visible = true;
                    }
                }

            }
        }

        private void initializeData()
        {
            //newSys_Preft = objGeneral.GetSingleSYS_PREFT(1, "AA", "RESTRICTIONNOTE");
            //lblRestriction.Text = newSys_Preft.SYSValue

            txt_GeustNum.Value = "";
            txt_ChildNum.Value = "";
            txt_InfantNum.Value = "";

            string monthNumeric = "";
            string monthString = "";
            string year = "";
            DateTime dateNow = DateTime.Now.AddHours(4); //change to be available before 2hrs flights
            DateTime tempDate = new DateTime();
            DateTime tempDate2 = new DateTime();
            divDate1.InnerHtml = "";
            divDate2.InnerHtml = "";

            daStart.MinDate = dateNow;

            #region initdatedepart
            divDate1.InnerHtml += "<table><tr><td><select id=\"ddlMarketDay1\" name=\"ddlMarketDay1\" >";

            for (int i = 1; i <= 31; i++)
            {
                if (i == dateNow.Day)
                {
                    divDate1.InnerHtml += "<option selected value=" + ConvTwoDigitDate(i.ToString()) + ">" + ConvTwoDigitDate(i.ToString()) + "</option>";
                }
                else
                {
                    divDate1.InnerHtml += "<option value=" + ConvTwoDigitDate(i.ToString()) + ">" + ConvTwoDigitDate(i.ToString()) + "</option>";
                }
                //divDate1.InnerHtml += "<option value=" + ConvTwoDigitDate(i.ToString()) + ">" + ConvTwoDigitDate(i.ToString()) + "</option>";
            }
            divDate1.InnerHtml += "</select></td>";
            divDate1.InnerHtml += "<td>&nbsp;</td><td><select class=\"wLrg2s\" id=\"ddlMarketMonth1\" name=\"ddlMarketMonth1\">";


            for (int i = 0; i <= 13; i++)
            {
                tempDate = dateNow.AddMonths(i);
                monthNumeric = tempDate.ToString("MM");
                monthString = tempDate.ToString("MMM");
                year = tempDate.ToString("yyyy");
                string val = year + "-" + monthNumeric;
                string disp = monthString + " " + year;
                divDate1.InnerHtml += "<option value=\"" + val + "\">" + disp + "</option>";
            }
            divDate1.InnerHtml += "</select></td>";
            divDate1.InnerHtml += "<td><input id=\"date_picker_id_1\" type=\"hidden\" name=\"date_picker\" value=\"\" /></td></tr></table>";
            #endregion

            #region initdatereturn

            tempDate = dateNow.AddDays(7);


            divDate2.InnerHtml += "<table><tr><td><select id=\"ddlMarketDay2\" name=\"ddlMarketDay2\">";
            for (int i = 1; i <= 31; i++)
            {

                if (i == tempDate.Day)
                {
                    divDate2.InnerHtml += "<option selected value=" + ConvTwoDigitDate(i.ToString()) + ">" + ConvTwoDigitDate(i.ToString()) + "</option>";
                }
                else
                {
                    divDate2.InnerHtml += "<option value=" + ConvTwoDigitDate(i.ToString()) + ">" + ConvTwoDigitDate(i.ToString()) + "</option>";
                }

                //divDate2.InnerHtml += "<option value=" + ConvTwoDigitDate(i.ToString()) + ">" + ConvTwoDigitDate(i.ToString()) + "</option>";
            }
            divDate2.InnerHtml += "</select></td>";
            divDate2.InnerHtml += "<td>&nbsp;</td><td><select class=\"wLrg2s\" id=\"ddlMarketMonth2\" name=\"ddlMarketMonth2\">";

            for (int i = 0; i <= 13; i++)
            {
                /*
                tempDate2 = dateNow.AddDays(7);
                tempDate = tempDate2.AddMonths(i);
                */
                tempDate2 = dateNow.AddMonths(i);
                monthNumeric = tempDate2.ToString("MM");
                monthString = tempDate2.ToString("MMM");
                year = tempDate2.ToString("yyyy");
                string val = year + "-" + monthNumeric;
                string disp = monthString + " " + year;

                if (tempDate.Month == Convert.ToInt16(monthNumeric) && tempDate.Year == Convert.ToInt16(year))
                {
                    //divDate2.InnerHtml += "<option selected value=\"" + val + "\">" + disp + "</option>";
                    divDate2.InnerHtml += "<option value=\"" + val + "\">" + disp + "</option>";
                }
                else
                {
                    divDate2.InnerHtml += "<option value=\"" + val + "\">" + disp + "</option>";
                }

            }
            divDate2.InnerHtml += "</select></td>";
            divDate2.InnerHtml += "<td><input id=\"date_picker_id_2\" type=\"hidden\" name=\"date_picker\" value=\"\" /></td></tr></table>";

            tdReturn.Style.Add("display", "block");
            Boolean returnFlight = false;
            objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
            returnFlight = objBooking.IsReturn(TransID, 0);
            if (returnFlight == false)
            {
                //tdReturn.Attributes["display"] = "none";
                tdReturn.Style.Add("display", "none");
                Session["returnFlight"] = "false";
            }
            else
            {
                Session["returnFlight"] = "true";
            }
            #endregion

            //UIClass.SetComboCustomStyle(ref ddlDeparture, UIClass.EnumDefineStyle.City, string.Empty, string.Empty, false);
            //UIClass.SetComboCustomStyle(ref ddlReturn, UIClass.EnumDefineStyle.City, string.Empty, ddlDeparture.SelectedItem.Value, false);

            DataTable dt = new DataTable();
            dt = objGeneral.GetLookUpCity("", Request.PhysicalApplicationPath);
            if (dt == null || dt.Rows.Count <= 0)
            {
                dt = objGeneral.ReturnAllCityCustom("");
            }
            //SEAL.WEB.UI.Control.SetComboCustomStyle(ref ddlDeparture, dt, "CustomState", "CityCode", "Select City");
            SEAL.WEB.UI.Control.SetComboCustomStyle(ref ddlDeparture, dt, "CustomState", "DepartureStation", "Select City");


            ddlDeparture.SelectedValue = model.Departure;
            hfArrival.Value = model.Arrival;
            bindComboBoxReturn();
            ddlReturn.SelectedValue = model.Arrival;
            daStart.Value = model.DepartDate;
            daEnd.Value = model.ReturnDate;
            txt_GeustNum.Value = model.GuestNum > 0 ? model.GuestNum.ToString() : "";
            txt_ChildNum.Value = model.ChildNum > 0 ? model.ChildNum.ToString() : "";
            txt_InfantNum.Value = model.InfantNum > 0 ? model.InfantNum.ToString() : "";
            //cb_OneWay.Checked = model.ifOneWay;
        }



        private string ConvTwoDigitDate(string date)
        {
            if (date.Length == 1) { date = "0" + date; }
            return date;
        }

        protected void ddlDeparture_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Boolean returnFlight = false;
                returnFlight = objBooking.IsReturn(TransID, 0);
                if (returnFlight == false)
                {
                    tdReturn.Style.Add("display", "none");
                    Session["returnFlight"] = "false";
                }
                else
                {
                    Session["returnFlight"] = "true";
                }
                bindComboBoxReturn();
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
        }

        protected void bindComboBoxReturn()
        {
            //UIClass.SetComboCustomStyle(ref ddlReturn, UIClass.EnumDefineStyle.City, string.Empty, ddlDeparture.SelectedItem.Value, false);
            DataTable dt = new DataTable();

            dt = objGeneral.GetLookUpCity(ddlDeparture.SelectedItem.Value, Request.PhysicalApplicationPath);
            if (dt != null && dt.Rows.Count > 0)
            {
                Session["dtCountryList"] = dt;
            }
            //SEAL.WEB.UI.Control.SetComboCustomStyle(ref ddlReturn, dt, "RCustomState", "RCityCode", "Select City");
            SEAL.WEB.UI.Control.SetComboCustomStyle(ref ddlReturn, dt, "RCustomState", "ArrivalStation", "Select City");
            if (dt == null || dt.Rows.Count <= 0)
            {
                //dt = objGeneral.ReturnAllCityCustom(ddlDeparture.SelectedItem.Value);
                //SEAL.WEB.UI.Control.SetComboCustomStyle(ref ddlReturn, dt, "CustomState", "CityCode", "Select City");
                //SEAL.WEB.UI.Control.SetComboCustomStyle(ref ddlReturn, dt, "CustomState", "DepartureStation", "Select City");
                return;
            }

            BindCurrency(ddlDeparture.SelectedItem.Value);
            ddlReturn.Focus();
        }

        private void BindCurrency(string Departure)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = objGeneral.GetLookUpCity("", Request.PhysicalApplicationPath);
                DataRow[] drs = dt.Select("DepartureStation='" + Departure + "'");
                if (drs != null)
                {
                    ddl_Currency.SelectedItem.Text = drs[0]["DepartureStationCurrencyCode"].ToString();
                    //ddl_Currency.SelectedItem.Text = objGeneral.GetCurrencyByDeparture(Departure);
                    hfCurrency.Value = drs[0]["DepartureStationCurrencyCode"].ToString();
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }

        }

        private void BindList1(ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition model, DataTable tempDt)
        {

            if (HttpContext.Current.Session["tempFlight"] != null)
            {
                string strExpr;
                string strSort;
                objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                DataTable dt = objBooking.dtFlight();

                strExpr = "TemFlightDeparture = '" + model.Departure + "' AND TemFlightArrival = '" + model.Arrival + "' AND TemFlightAgentName = '" + MyUserSet.LoginName + "' AND TemFlightIfReturn = false";

                strSort = "TemFlightStd ASC";

                dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                tempDt.Clear();

                foreach (DataRow row in foundRows)
                {
                    tempDt.ImportRow(row);
                }

                tempDt.DefaultView.Sort = "TemFlightStd ASC";

                dvSelectFlight.DataSource = tempDt;
                dvSelectFlight.DataBind();

                //begin, bind date tab
                string DepartContainer = "";
                int Mode = 0;
                DateTime StartDate;
                DateTime SelectedDate;
                DateTime MinDate = DateTime.Now.AddHours(4);

                StartDate = (DateTime)model.DepartDate.AddDays(-3);
                SelectedDate = (DateTime)model.DepartDate;
                DepartContainer += "<li>";
                DepartContainer += "<a href='flightchange.aspx?k=" + Request.QueryString["k"] + "&TransID=" + Request.QueryString["TransID"] + "&mod=" + Mode + "&add=" + (-2) + "' id ='A_10' >";
                DepartContainer += "<div class='leftArrow'>";
                DepartContainer += "</div>";
                DepartContainer += "</a>";
                DepartContainer += "</li>";

                //int cntDay = -3;
                int cntDay = 0;
                if (StartDate.Date <= MinDate.Date)
                    cntDay = Convert.ToInt32((MinDate - SelectedDate).TotalDays);
                else
                    cntDay = Convert.ToInt32((StartDate - SelectedDate).TotalDays);


                for (int i = 0; i < 7; i++)
                {
                    if (StartDate >= MinDate)
                    {
                        if (StartDate == SelectedDate)
                        {
                            DepartContainer += "<li class='selectedDate'>";
                            DepartContainer += "<a href='#'>";
                            DepartContainer += "<div id='fg_40'>";
                        }
                        else if (i == 0 || i == 4)
                        {
                            DepartContainer += "<li>";
                            DepartContainer += "<a href='flightchange.aspx?k=" + Request.QueryString["k"] + "&TransID=" + Request.QueryString["TransID"] + "&mod=" + Mode + "&add=" + cntDay + "' class='linkDayFlight'>";
                            DepartContainer += "<div>";
                        }
                        else
                        {
                            DepartContainer += "<li>";
                            DepartContainer += "<a href='flightchange.aspx?k=" + Request.QueryString["k"] + "&TransID=" + Request.QueryString["TransID"] + "&mod=" + Mode + "&add=" + cntDay + "' class='linkDayFlight'>";
                            DepartContainer += "<div class='tabSelection'>";
                        }
                        DepartContainer += "<h4>" + StartDate.ToString("ddd, dd MMMM");
                        DepartContainer += "</h4>";
                        DepartContainer += "</div>";
                        DepartContainer += "</a>";
                        DepartContainer += "</li>";

                        cntDay += 1;
                    }
                    StartDate = StartDate.AddDays(1);
                }
                DepartContainer += "<li>";
                DepartContainer += "<a href='flightchange.aspx?k=" + Request.QueryString["k"] + "&TransID=" + Request.QueryString["TransID"] + "&mod=" + Mode + "&add=" + cntDay + "' id ='A_63' >";
                DepartContainer += "<div class='rightArrow'>";
                DepartContainer += "</div>";
                DepartContainer += "</a>";
                DepartContainer += "</li>";

                departDateDiv.InnerHtml = DepartContainer;
                //end, bind date tab
            }
        }

        private void BindList2(ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition model, DataTable tempDt)
        {
            if (HttpContext.Current.Session["tempFlight"] != null)
            {
                string strExpr;
                string strSort;
                DataTable dt = new DataTable();

                strExpr = "TemFlightDeparture = '" + model.Arrival + "' AND TemFlightArrival = '" + model.Departure + "' AND TemFlightAgentName = '" + MyUserSet.LoginName + "' AND TemFlightIfReturn = true";

                strSort = "TemFlightStd ASC";
                // Use the Select method to find all rows matching the filter.

                dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                tempDt.Clear();

                foreach (DataRow row in foundRows)
                {
                    tempDt.ImportRow(row);
                }

                gvSelectFlightReturn.DataSource = tempDt;
                gvSelectFlightReturn.DataBind();

                //begin, bind date tab
                string ReturnContainer = "";
                int Mode = 1;
                DateTime StartDate;
                DateTime SelectedDate;
                DateTime MinDate = DateTime.Now.AddHours(4);

                StartDate = (DateTime)model.ReturnDate.AddDays(-3);
                SelectedDate = (DateTime)model.ReturnDate;
                ReturnContainer += "<li>";
                ReturnContainer += "<a href='flightchange.aspx?k=" + Request.QueryString["k"] + "&TransID=" + Request.QueryString["TransID"] + "&mod=" + Mode + "&add=" + (-2) + "' id ='A_10' >";
                ReturnContainer += "<div class='leftArrow'>";
                ReturnContainer += "</div>";
                ReturnContainer += "</a>";
                ReturnContainer += "</li>";

                //int cntDay = -3;
                int cntDay = 0;
                if (StartDate.Date <= MinDate.Date)
                    cntDay = Convert.ToInt32((MinDate - SelectedDate).TotalDays);
                else
                    cntDay = Convert.ToInt32((StartDate - SelectedDate).TotalDays);


                for (int i = 0; i < 7; i++)
                {
                    if (StartDate >= MinDate)
                    {
                        if (StartDate == SelectedDate)
                        {
                            ReturnContainer += "<li class='selectedDate'>";
                            ReturnContainer += "<a href='#'>";
                            ReturnContainer += "<div id='fg_40'>";
                        }
                        else if (i == 0 || i == 4)
                        {
                            ReturnContainer += "<li>";
                            ReturnContainer += "<a href='flightchange.aspx?k=" + Request.QueryString["k"] + "&TransID=" + Request.QueryString["TransID"] + "&mod=" + Mode + "&add=" + cntDay + "' class='linkDayFlight'>";
                            ReturnContainer += "<div>";
                        }
                        else
                        {
                            ReturnContainer += "<li>";
                            ReturnContainer += "<a href='flightchange.aspx?k=" + Request.QueryString["k"] + "&TransID=" + Request.QueryString["TransID"] + "&mod=" + Mode + "&add=" + cntDay + "' class='linkDayFlight'>";
                            ReturnContainer += "<div class='tabSelection'>";
                        }
                        ReturnContainer += "<h4>" + StartDate.ToString("ddd, dd MMMM");
                        ReturnContainer += "</h4>";
                        ReturnContainer += "</div>";
                        ReturnContainer += "</a>";
                        ReturnContainer += "</li>";

                        cntDay += 1;
                    }
                    StartDate = StartDate.AddDays(1);
                }
                ReturnContainer += "<li>";
                ReturnContainer += "<a href='javascript:flightchange.aspx?k=" + Request.QueryString["k"] + "&TransID=" + Request.QueryString["TransID"] + "&mod=" + Mode + "&add=" + cntDay + "' id ='A_63' >";
                ReturnContainer += "<div class='rightArrow'>";
                ReturnContainer += "</div>";
                ReturnContainer += "</a>";
                ReturnContainer += "</li>";

                returnDateDiv.InnerHtml = ReturnContainer;
                //end, bind date tab
            }
        }

        protected void img_Click(object sender, ImageClickEventArgs e)
        {//关闭返程航班
            tr_Return.Visible = false;
        }



        protected void ValidatePopup(object sender, CallbackEventArgs e)
        {
            MessageList msgList = new MessageList();
            try
            {
                if (e.Parameter != null && e.Parameter != "" && e.Parameter.Trim() == "search")
                {
                    ResearchFlight(3, 0);
                    e.Result = "search";
                    return;
                }

                if (Session["AgentSet"] == null)
                {
                    e.Result = msgList.Err100025;
                    return;
                }

                ArrayList aMsgList = new ArrayList();

                string list1ID = "";
                string list2ID = "";
                bool ifSelect = false;
                for (int i = 0; i < this.dvSelectFlight.Items.Count; i++)
                {
                    RadioButton rb = this.dvSelectFlight.FindItemControl("RadioButton1", dvSelectFlight.Items[i]) as RadioButton;
                    //HiddenField hf = this.dvSelectFlight.FindItemControl("hfChecked", dvSelectFlight.Items[i]) as HiddenField;
                    if (rb.Checked)
                    {
                        ifSelect = true;
                        HiddenField l = this.dvSelectFlight.FindItemControl("lbl_list1ID", dvSelectFlight.Items[i]) as HiddenField;
                        list1ID = l.Value;
                        break;
                    }

                }
                if (ifSelect)
                {
                    if (tr_Return.Visible == true)
                    {
                        ifSelect = false;
                        for (int i = 0; i < this.gvSelectFlightReturn.Items.Count; i++)
                        {
                            RadioButton rb = this.gvSelectFlightReturn.FindItemControl("RadioButton2", gvSelectFlightReturn.Items[i]) as RadioButton;
                            if (rb.Checked)
                            {
                                ifSelect = true;
                                HiddenField l = this.gvSelectFlightReturn.FindItemControl("lbl_list2ID", gvSelectFlightReturn.Items[i]) as HiddenField;
                                list2ID = l.Value;
                                break;
                            }
                        }
                    }
                    if (ifSelect)
                    {

                        HttpCookie cookie = Request.Cookies["cookieSearchcondition"];

                        //check available seat
                        bool oneWay = true;
                        bool available = false;
                        int availableGoing = 0;
                        int availableReturn = 0;

                        ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition model = new ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition();
                        ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition model2 = new ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition();

                        string strExpr;
                        string strSort;
                        DataTable dt = new DataTable();

                        strExpr = "TemFlightId = '" + list1ID + "'";

                        strSort = "";

                        dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                        DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                        FillModelFromDataRow(foundRows, ref  model);

                        availableGoing = objBooking.CheckAvailableFare(model, "");
                        if (availableGoing == 1)
                        {
                            available = true;
                        }

                        if (list2ID != "" && available == true) //check one way
                        {
                            oneWay = false;

                            strExpr = "TemFlightId = '" + list2ID + "'";

                            strSort = "";

                            dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                            foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                            FillModelFromDataRow(foundRows, ref  model2);

                            if (Session["GetBookingResponse"] != null)
                            {
                                GetBookingResponse resp = (GetBookingResponse)Session["GetBookingResponse"];
                                if (resp.Booking.Journeys[0].Segments.Length == 1)
                                {
                                    if (resp.Booking.Journeys[0].Segments[0].FlightDesignator.FlightNumber == model.FlightNumber && resp.Booking.Journeys[0].Segments[0].FlightDesignator.CarrierCode == model.Carrier && resp.Booking.Journeys[0].Segments[0].STD == model.DepartDate && resp.Booking.Journeys[1].Segments[0].FlightDesignator.FlightNumber == model2.FlightNumber && resp.Booking.Journeys[1].Segments[0].FlightDesignator.CarrierCode == model2.Carrier && resp.Booking.Journeys[1].Segments[0].STD == model2.DepartDate)
                                    {
                                        e.Result = msgList.Err100066;
                                        return; ;
                                    }
                                }
                                else
                                {
                                    if (resp.Booking.Journeys[1].Segments.Length > 1)
                                    {
                                        if (resp.Booking.Journeys[0].Segments[0].FlightDesignator.FlightNumber == model.FlightNumber && resp.Booking.Journeys[0].Segments[0].FlightDesignator.CarrierCode == model.Carrier && resp.Booking.Journeys[0].Segments[0].STD == model.DepartDate && resp.Booking.Journeys[1].Segments[1].FlightDesignator.FlightNumber == model2.FlightNumber && resp.Booking.Journeys[1].Segments[1].FlightDesignator.CarrierCode == model2.Carrier && resp.Booking.Journeys[1].Segments[1].STD == model2.DepartDate)
                                        {
                                            e.Result = msgList.Err100066;
                                            return; ;
                                        }
                                    }
                                    else
                                    {
                                        if (resp.Booking.Journeys[0].Segments[0].FlightDesignator.FlightNumber == model.FlightNumber && resp.Booking.Journeys[0].Segments[0].FlightDesignator.CarrierCode == model.Carrier && resp.Booking.Journeys[0].Segments[0].STD == model.DepartDate && resp.Booking.Journeys[1].Segments[0].FlightDesignator.FlightNumber == model2.FlightNumber && resp.Booking.Journeys[1].Segments[0].FlightDesignator.CarrierCode == model2.Carrier && resp.Booking.Journeys[1].Segments[0].STD == model2.DepartDate)
                                        {
                                            e.Result = msgList.Err100066;
                                            return; ;
                                        }
                                    }
                                }
                            }


                            availableReturn = objBooking.CheckAvailableFare(model2, "");

                            if (availableReturn == 1)
                            {
                                available = true;
                            }
                            else
                            {
                                available = false;
                            }
                        }
                        else
                        {
                            if (Session["GetBookingResponse"] != null)
                            {
                                GetBookingResponse resp = (GetBookingResponse)Session["GetBookingResponse"];
                                if (resp.Booking.Journeys[0].Segments[0].FlightDesignator.FlightNumber == model.FlightNumber && resp.Booking.Journeys[0].Segments[0].FlightDesignator.CarrierCode == model.Carrier && resp.Booking.Journeys[0].Segments[0].STD == model.DepartDate)
                                {
                                    e.Result = msgList.Err100066;
                                    return; ;
                                }


                            }
                        }

                        //check booking seat allowance

                        //get maxseat by carriercode
                        string seatQuota = objGeneral.getSysValueByKeyAndCarrierCode("SEATQUOTA", model.Carrier);
                        if (seatQuota == "")
                            seatQuota = "0";
                        int maxSeat = Convert.ToInt16(seatQuota);

                        //get optgroup by carriercode
                        string optGroup = objGeneral.getOPTGroupByCarrierCode(model.Carrier);

                        //get totalBooking for this day by optgroup
                        int totalBook = objBooking.GetTotalBookingByOptGroup(optGroup, model.Carrier, model.FlightNumber);

                        //get available seat
                        int availableSeat = maxSeat - totalBook;

                        if (model.PaxNum > availableSeat)
                        {
                            available = false;
                        }
                        //else
                        //{
                        //    available = true;
                        //}

                        if (available == true)
                        {
                            HttpCookie cookie2 = new HttpCookie("cookieTemFlight");
                            cookie2.Values.Add("list1ID", list1ID);
                            cookie2.Values.Add("ReturnID", list2ID);
                            cookie2.Values.Add("PaxNum", cookie.Values["PaxNum"]);
                            cookie2.Values.Add("GuestNum", cookie.Values["GuestNum"]);
                            cookie2.Values.Add("ChildNum", cookie.Values["ChildNum"]);

                            if (cookie.Values["InfantNum"] != "")
                            {
                                if (Convert.ToInt32(cookie.Values["InfantNum"]) != 0)
                                {
                                    HttpCookie cookie3 = new HttpCookie("AllPax");
                                    cookie3.Values.Add("PaxNum", cookie.Values["PaxNum"]);
                                    cookie3.Values.Add("GuestNum", cookie.Values["GuestNum"]);
                                    cookie3.Values.Add("ChildNum", cookie.Values["ChildNum"]);
                                    cookie3.Values.Add("InfantNum", cookie.Values["InfantNum"]);
                                    Response.AppendCookie(cookie3);
                                }
                            }

                            Response.AppendCookie(cookie2);

                            string ret = SellFlight();
                            if (ret != "" && ret != "Sell Journey failed")
                            {
                                if (model.PaxNum > 10)
                                {
                                    e.Result = "Failed to change flight, maximum pax for selected PNR is " + ret + ". Please Divide this PNR first";
                                }
                                else if (ret == msgList.Err100059)
                                {
                                    e.Result = msgList.Err100059;
                                }
                                else 
                                {
                                    e.Result = msgList.Err100065;
                                }
                            }
                            else if (ret == "Sell Journey failed")
                            {
                                e.Result = msgList.Err100062;
                            }
                            else
                            {
                                e.Result = "";
                            }
                        }
                        else
                        {
                            e.Result = msgList.Err100013;
                        }
                    }
                    else
                    {
                        e.Result = msgList.Err100014;
                    }
                }
                else
                {
                    e.Result = msgList.Err100015;

                }

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                e.Result = msgList.Err100031;
            }
        }

        protected void FillModelFromDataRow(DataRow[] foundRows, ref ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition model)
        {
            try
            {
                model.Arrival = foundRows[0]["TemFlightArrival"].ToString();
                model.Carrier = foundRows[0]["TemFlightCarrierCode"].ToString();
                model.ChildNum = Convert.ToInt16(foundRows[0]["TemFlightCHDNum"]);
                model.Currency = foundRows[0]["TemFlightCurrencyCode"].ToString();
                model.DepartDate = Convert.ToDateTime(foundRows[0]["TemFlightStd"]);
                model.Departure = foundRows[0]["TemFlightDeparture"].ToString();
                model.GuestNum = Convert.ToInt16(foundRows[0]["TemFlightADTNum"]);
                model.ifOneWay = Convert.ToBoolean(foundRows[0]["TemFlightIfReturn"]);
                model.PaxNum = Convert.ToInt16(foundRows[0]["TemFlightPaxNum"]);
                model.ReturnDate = Convert.ToDateTime(foundRows[0]["TemFlightSta"]);
                //added by ketee
                model.FlightNumber = foundRows[0]["TemFlightFlightNumber"].ToString();
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
        }

        protected void btn_Next_Click(object sender, EventArgs e)
        {

        }

        protected void ClearSessionData()
        {
            //HttpContext.Current.Session.Remove("TempFlight");
            //HttpContext.Current.Session.Remove("dataClass");
            //HttpContext.Current.Session.Remove("ErrorPayment");
            //HttpContext.Current.Session.Remove("dataClassTrans");
            if (Session["TransID"] != null)
            {
                if (Session["TransID"].ToString() != "")
                {
                    TransID = Session["TransID"].ToString();
                }
            }
            bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(TransID);
            objBooking.FillDataTableTransMain(bookHDRInfo);
            int transStatus = 0;

            //added by diana 20130923, update flight & passenger details

            bool callFunction = false;

            if (Session["generatePayment"] == null)
                callFunction = true;
            else if (Session["generatePayment"].ToString() == "")
                callFunction = true;

            if (Session["AgentSet"] != null)
            {
                objBooking.ClearExpiredJourney(MyUserSet.AgentID, TransID);
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
                    if (objBooking.UpdateAllBookingJourneyDetailsMoveFlight(lstTrans, lstTrans.AgentUserName.ToString(), lstTrans.AgentID.ToString(), true) == false)
                    {
                        log.Warning(this, "Fail to Get Latest Update for Transaction - flightchange.aspx.cs : " + lstTrans.TransID);
                        //if (ConfigurationManager.AppSettings["ErrorLogEmail"] != null && ConfigurationManager.AppSettings["ErrorLogEmail"] != "")
                        //{
                        //    eWS.CustomEmail(ConfigurationManager.AppSettings["ErrorLogEmail"].ToString(), lstTrans.TransID, "", "Errors Log :" + DateTime.Now.ToString() + " <br/> Fail to Get Latest Update for Transaction : " + lstTrans.TransID);
                        //}
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
                    objBooking.UpdatePassengerDetails(TransID, MyUserSet.AgentName, MyUserSet.AgentID, true);

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

                objBooking.FillDataTableTransDetail(listDetailCombinePNR);

                string PayScheme = "";
                string CurrencyCode = "";
                string CountryCode = "";
                string CarrierCode = "";
                List<BookingTransactionDetail> listBookingDetail = new List<BookingTransactionDetail>();
                List<BookingTransactionDetail> listBookingDetailCombine = new List<BookingTransactionDetail>();
                BookingTransactionMain bookingMain = new BookingTransactionMain();
                PaymentControl objPayment = new PaymentControl();
                PaymentInfo paymentInfo = new PaymentInfo();

                decimal CollectedAmount = 0;
                decimal ServiceChg = 0;
                decimal FullPrice = 0;
                decimal BaseFare = 0;
                decimal AmountDue = 0;

                DateTime PayDueDate1;
                decimal PayDueAmount1 = 0;
                DateTime PayDueDate2;
                decimal PayDueAmount2 = 0;
                DateTime PayDueDate3;
                decimal PayDueAmount3 = 0;

                DateTime PaymentDateEx1;
                decimal PaymentAmtEx1 = 0;
                DateTime PaymentDateEx2;
                decimal PaymentAmtEx2 = 0;
                DateTime PaymentDateEx3;
                decimal PaymentAmtEx3 = 0;

                int Quantity = 0;
                decimal FirstDeposit = 0;
                decimal CurrencyRate = 0;

                if (HttpContext.Current.Session["TempFlight"] != null)
                {
                    //Hashtable ht = (Hashtable)HttpContext.Current.Session["HashMain"];
                    //TransID = TransID;
                    bookingMain = objBooking.GetSingleBK_TRANSMAIN(TransID);
                    PayScheme = bookingMain.PayScheme;
                    CurrencyCode = bookingMain.Currency;
                }
                else if (HttpContext.Current.Session["TransMain"] != null)
                {
                    DataTable dtTransMain = objBooking.dtTransMain();
                    dtTransMain = (DataTable)HttpContext.Current.Session["TransMain"];
                    TransID = dtTransMain.Rows[0]["TransID"].ToString();
                    PayScheme = dtTransMain.Rows[0]["SchemeCode"].ToString();
                    CurrencyCode = dtTransMain.Rows[0]["Currency"].ToString();
                }

                //DataTable dtCountryInfo = objGeneral.GetCountryCodeByCurrency(CurrencyCode);
                //CountryCode = dtCountryInfo.Rows[0]["DefaultCurrencyCode"].ToString();
                if (Session["Country"] != null)
                    CountryCode = Session["Country"].ToString().Substring(0, 2);

                listBookingDetail = objBooking.GetAllBK_TRANSDTLFilterAll(TransID, 0);
                listBookingDetailCombine = objBooking.GetAllBK_TRANSDTLFlightGrp(TransID);

                //FullPrice = bookingMain.TransTotalAmt;
                CurrencyRate = bookingMain.ExchangeRate;

                string GroupName = objGeneral.getOPTGroupByCarrierCode(listBookingDetail[0].CarrierCode);
                string AgentCountryCode = "";
                if (Session["CountryCode"].ToString() != null)
                    AgentCountryCode = Session["CountryCode"].ToString();
                paymentInfo = objPayment.GetPaymentScheme(PayScheme, GroupName, TransID, CountryCode, CurrencyCode, AgentCountryCode);
                //paymentInfo = objPayment.GetPaymentScheme(PayScheme, GroupName, TransID, CountryCode, CurrencyCode);

                foreach (BookingTransactionDetail bookDetail in listBookingDetailCombine)
                {
                    FullPrice = bookDetail.LineTotal;
                    BaseFare = bookDetail.LineFlight;

                    int cnt = 0;
                    foreach (BookingTransactionDetail bkDetail in listBookingDetail)
                    {
                        if (bkDetail.Signature == bookDetail.Signature)
                        {
                            cnt += 1;
                        }
                    }

                    decimal paymentAttempt1 = 0;
                    decimal paymentAttempt2 = 0;
                    decimal paymentAttempt3 = 0;
                    decimal deposit = 0;

                    CollectedAmount += bookDetail.CollectedAmount;
                    ServiceChg += bookDetail.LineFee;
                    Quantity += bookDetail.PaxAdult + bookDetail.PaxChild;

                    int iIndex = -1;
                    if (cnt >= 2)
                        iIndex = listBookingDetail.FindIndex(p => p.Signature == bookDetail.Signature && p.SeqNo % 2 == 1);
                    else
                        iIndex = listBookingDetail.FindIndex(p => p.Signature == bookDetail.Signature);

                    int iIndex2 = -1;
                    if (cnt >= 2) iIndex2 = listBookingDetail.FindIndex(p => p.Signature == bookDetail.Signature && p.SeqNo % 2 == 0);

                    PayDueDate1 = bookingMain.BookingDate;
                    PaymentDateEx1 = PayDueDate1;
                    bookingMain.PaymentDateEx1 = PayDueDate1;
                    if (iIndex >= 0) listBookingDetail[iIndex].PayDueDate1 = PayDueDate1;
                    if (iIndex2 >= 0) listBookingDetail[iIndex2].PayDueDate1 = PayDueDate1;

                    if (paymentInfo.PaymentType == "DEPO")
                    {
                        //FirstDeposit = paymentInfo.FirstDeposit;
                        //if (CurrencyRate > 0) FirstDeposit = FirstDeposit / CurrencyRate;
                        //FirstDeposit = FirstDeposit * Quantity;

                        deposit = objGeneral.getDepositByDuration(TransID, BaseFare, bookDetail.PaxAdult + bookDetail.PaxChild, bookDetail.Currency, bookDetail.Origin, GroupName, bookDetail.FlightDuration, bookDetail.SellKey, bookDetail.Transit);

                        ServiceChg = 0;
                        PayDueAmount1 = ServiceChg;


                        paymentAttempt1 = Math.Round((deposit * paymentInfo.Percentage_1) / 100, 2);
                        if (paymentInfo.IsNominal_1 == 1)
                        {
                            if (deposit == 0)
                            {
                                //objBK_TRANSDTL_Info.Currency
                                deposit = objGeneral.getDepositByDuration(TransID, FullPrice, bookDetail.PaxAdult + bookDetail.PaxChild, bookDetail.Currency, bookDetail.Origin, GroupName, bookDetail.FlightDuration, bookDetail.SellKey, bookDetail.Transit);
                                //if (CurrencyRate > 0) deposit = deposit / CurrencyRate;
                            }
                            paymentAttempt1 = deposit + ServiceChg;
                            //add validation if full price less than deposit, set to paid in full price, 20170207, by ketee
                            if (FullPrice <= deposit)
                                PayDueAmount1 = FullPrice;
                            else
                                PayDueAmount1 = deposit;
                        }
                        else if (paymentInfo.Deposit_1 != 0)
                        {
                            if (deposit == 0)
                            {
                                deposit = objGeneral.getDeposit(TransID, bookDetail.TotalPax, bookDetail.Currency, bookDetail.Origin, bookDetail.Transit);
                            }
                            paymentAttempt1 = deposit + ServiceChg;
                            //add validation if full price less than deposit, set to paid in full price, 20170207, by ketee
                            //PayDueAmount1 = deposit;
                            if (FullPrice <= deposit)
                                PayDueAmount1 = FullPrice;
                            else
                                PayDueAmount1 = deposit;
                        }
                        else
                        {
                            PayDueAmount1 = paymentAttempt1 - ServiceChg;
                        }

                        if (paymentInfo.Code_1 == "DOB")
                        {
                            PayDueDate1 = bookingMain.BookingDate.AddHours(paymentInfo.Attempt_1);
                            PaymentDateEx1 = PayDueDate1;
                            bookingMain.PaymentDateEx1 = PayDueDate1;
                            if (iIndex >= 0 && PayDueAmount1 > 0) listBookingDetail[iIndex].PayDueDate1 = PayDueDate1;
                            if (iIndex2 >= 0 && PayDueAmount1 > 0) listBookingDetail[iIndex2].PayDueDate1 = PayDueDate1;
                        }
                        else if (paymentInfo.Code_1 == "STD")
                        {
                            PayDueDate1 = bookDetail.DepatureDate.AddHours(-paymentInfo.Attempt_1);
                            PaymentDateEx1 = PayDueDate1;
                            bookingMain.PaymentDateEx1 = PayDueDate1;
                            if (iIndex >= 0 && PayDueAmount1 > 0) listBookingDetail[iIndex].PayDueDate1 = PayDueDate1;
                            if (iIndex2 >= 0 && PayDueAmount1 > 0) listBookingDetail[iIndex2].PayDueDate1 = PayDueDate1;
                        }

                        //PayDueAmount2 = bookDetail.LineTotal - PayDueAmount1;

                        if (FullPrice > (PayDueAmount1) && paymentInfo.Percentage_2 == 0)
                        {
                            paymentAttempt2 = FullPrice - (PayDueAmount1);
                            PayDueAmount2 = paymentAttempt2;
                        }
                        else
                        {
                            paymentAttempt2 = Math.Round((deposit * paymentInfo.Percentage_2) / 100, 2);
                            PayDueAmount2 = paymentAttempt2;
                        }

                        if (paymentInfo.Code_2 == "DOB")
                        {
                            PayDueDate2 = bookingMain.BookingDate.AddHours(paymentInfo.Attempt_2);
                            PaymentDateEx2 = PayDueDate2;
                            bookingMain.PaymentDateEx2 = PayDueDate2;
                            if (iIndex >= 0 && PayDueAmount2 > 0) listBookingDetail[iIndex].PayDueDate2 = PayDueDate2;
                            if (iIndex2 >= 0 && PayDueAmount2 > 0) listBookingDetail[iIndex2].PayDueDate2 = PayDueDate2;
                        }
                        else if (paymentInfo.Code_2 == "STD")
                        {
                            PayDueDate2 = bookDetail.DepatureDate.AddHours(-paymentInfo.Attempt_2);
                            PaymentDateEx2 = PayDueDate2;
                            bookingMain.PaymentDateEx2 = PayDueDate2;
                            if (iIndex >= 0 && PayDueAmount2 > 0) listBookingDetail[iIndex].PayDueDate2 = PayDueDate2;
                            if (iIndex2 >= 0 && PayDueAmount2 > 0) listBookingDetail[iIndex2].PayDueDate2 = PayDueDate2;
                        }

                        if (FullPrice > (PayDueAmount1 + PayDueAmount2) && paymentInfo.Percentage_3 == 0)
                        {
                            paymentAttempt3 = FullPrice - (PayDueAmount1 + PayDueAmount2);
                            PayDueAmount3 = paymentAttempt3;
                        }
                        else
                        {
                            paymentAttempt3 = Math.Round((deposit * paymentInfo.Percentage_3) / 100, 2);
                            PayDueAmount3 = paymentAttempt3;
                        }

                        if (paymentInfo.Code_3 == "DOB")
                        {
                            PayDueDate3 = bookingMain.BookingDate.AddHours(paymentInfo.Attempt_3);
                            PaymentDateEx3 = PayDueDate3;
                            bookingMain.PaymentDateEx3 = PayDueDate3;
                            if (iIndex >= 0 && PayDueAmount3 > 0) listBookingDetail[iIndex].PayDueDate3 = PayDueDate3;
                            if (iIndex2 >= 0 && PayDueAmount3 > 0) listBookingDetail[iIndex2].PayDueDate3 = PayDueDate3;
                        }
                        else if (paymentInfo.Code_3 == "STD")
                        {
                            PayDueDate3 = bookDetail.DepatureDate.AddHours(-paymentInfo.Attempt_3);
                            PaymentDateEx3 = PayDueDate3;
                            bookingMain.PaymentDateEx3 = PayDueDate3;
                            if (iIndex >= 0 && PayDueAmount3 > 0) listBookingDetail[iIndex].PayDueDate3 = PayDueDate3;
                            if (iIndex2 >= 0 && PayDueAmount3 > 0) listBookingDetail[iIndex2].PayDueDate3 = PayDueDate3;
                        }
                    }
                    else
                    {
                        PayDueAmount1 = FullPrice;
                    }

                    if (PayDueAmount1 > 0) PaymentAmtEx1 += PayDueAmount1;
                    if (PayDueAmount2 > 0) PaymentAmtEx2 += PayDueAmount2;
                    if (PayDueAmount3 > 0) PaymentAmtEx3 += PayDueAmount3;

                    bookingMain.PaymentAmtEx1 = PaymentAmtEx1;
                    bookingMain.PaymentAmtEx2 = PaymentAmtEx2;
                    bookingMain.PaymentAmtEx3 = PaymentAmtEx3;

                    if (iIndex >= 0 && PayDueAmount1 > 0) listBookingDetail[iIndex].PayDueAmount1 = PayDueAmount1;
                    if (iIndex2 >= 0 && PayDueAmount1 > 0) listBookingDetail[iIndex2].PayDueAmount1 = PayDueAmount1;
                    if (iIndex >= 0 && PayDueAmount2 > 0) listBookingDetail[iIndex].PayDueAmount2 = PayDueAmount2;
                    if (iIndex2 >= 0 && PayDueAmount2 > 0) listBookingDetail[iIndex2].PayDueAmount2 = PayDueAmount2;
                    if (iIndex >= 0 && PayDueAmount3 > 0) listBookingDetail[iIndex].PayDueAmount3 = PayDueAmount3;
                    if (iIndex2 >= 0 && PayDueAmount3 > 0) listBookingDetail[iIndex2].PayDueAmount3 = PayDueAmount3;
                }

                objBooking.SaveHeaderDetail(bookingMain, listBookingDetail, CoreBase.EnumSaveType.Update);
                //end added by diana 20130923

            }

        }

        private string SellFlight()
        {
            MessageList msgList = new MessageList();
            int departID = 0;
            string ReturnID = "";
            int num = 0;

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
                FillModelFromDataRows(foundRows, ref  temFlight);

                string getfare = temFlight.TemFlightServiceCharge.ToString();
                if (ReturnID != "")
                {
                    strExpr = "TemFlightId = '" + ReturnID + "'";
                    strSort = "";

                    dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                    foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                    FillModelFromDataRows(foundRows, ref  temFlight2);

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
                    log.Info(this, "Entering return Flight Saving.");
                    HttpCookie cookie2 = Request.Cookies["cookieSearchcondition"];
                    string sessID = "";
                    int InfantNum = 0;
                    if (cookie2 != null)
                    {
                        PNR = Session["PNR"].ToString();
                        if (cookie2.Values["InfantNum"] != "")
                        {
                            InfantNum = Convert.ToInt32(cookie2.Values["InfantNum"]);
                        }

                        string ret = objBooking.UpdateTemFlightChange(temFlight, temFlight2, "", ref sessID);
                        if (ret != "")
                        {
                            return ret;
                        }
                        else
                        {
                            if (objBooking.SellJourneyMove(temFlight, temFlight2, LoginType, LoginName, LoginPWD, LoginDomain, sessID, PNR, InfantNum) == false)
                            {
                                if (Session["errormove"] != null)
                                {
                                    if (Session["errormove"].ToString().Contains("Sold") == true)
                                    {
                                        return msgList.Err100059;
                                    }
                                    else
                                    {
                                        return msgList.Err100064;
                                    }
                                }
                                else
                                {
                                    return msgList.Err100064;
                                }
                                
                            }
                        }

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
                    HttpCookie cookie2 = Request.Cookies["cookieSearchcondition"];
                    string sessID = "";
                    int InfantNum = 0;
                    if (cookie2 != null)
                    {
                        PNR = Session["PNR"].ToString();
                        if (cookie2.Values["InfantNum"] != "")
                        {
                            InfantNum = Convert.ToInt32(cookie2.Values["InfantNum"]);
                        }
                        string ret = objBooking.UpdateTemFlightChange(temFlight, temFlight2, "", ref sessID);
                        if (ret != "")
                        {
                            return ret;
                        }
                        else
                        {
                            //objBooking.UpdateTemFlight(temFlight, temFlight2, "", ref sessID);
                            if (objBooking.SellJourneyMoveOneWay(temFlight, temFlight2, LoginType, LoginName, LoginPWD, LoginDomain, sessID, PNR, InfantNum) == false)
                            {
                                if (Session["errormove"] != null)
                                {
                                    if (Session["errormove"].ToString().Contains("Sold") == true)
                                    {
                                        return msgList.Err100059;
                                    }
                                    else
                                    {
                                        return msgList.Err100064;
                                    }
                                }
                                else
                                {
                                    return msgList.Err100064;
                                }
                            }

                        }
                    }
                }

                HttpContext.Current.Session.Remove("Fare");
                Hashtable htFare = new Hashtable();
                decimal total = (temFlight.TemFlightTotalAmount + temFlight2.TemFlightTotalAmount);
                htFare.Add("Avg", (objGeneral.RoundUp(total / temFlight.TemFlightPaxNum)).ToString("N", nfi));
                htFare.Add("Dpt", temFlight.TemFlightTotalAmount);
                htFare.Add("Rtn", temFlight2.TemFlightTotalAmount);
                HttpContext.Current.Session.Add("Fare", htFare);

                DataTable dtPNR = (DataTable)Session["ListPNR"];
                object sumObject;
                sumObject = dtPNR.Compute("Count(RecordLocator)", "RecordLocator <> 'All' AND PNR <> 'All'");

                PNR = Session["PNR"].ToString();

                if (Convert.ToDecimal(sumObject) > 1)
                {
                    objBooking.FillChgHistory(TransID, PNR);
                    //objBooking.SaveHistoryBooking(TransID, PNR, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
                    SaveData();
                    //ClearSessionData();
                }
                else
                {
                    objBooking.FillChgHistory(TransID, PNR);
                    //objBooking.SaveHistoryBooking(TransID, PNR, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
                    UpdateData();
                    
                    //ClearSessionData();

                }

                string PayScheme = "";
                List<BookingTransactionDetail> listBookingDetail = new List<BookingTransactionDetail>();
                List<BookingTransactionDetail> listBookingDetailCombine = new List<BookingTransactionDetail>();
                BookingTransactionMain bookingMain = new BookingTransactionMain();
                PaymentControl objPayment = new PaymentControl();
                PaymentInfo paymentInfo = new PaymentInfo();

               
                if (Session["TransID"] != null)
                {
                    TransID = Session["TransID"].ToString();
                }

                bookingMain = objBooking.GetSingleBK_TRANSMAIN(TransID);
                if (bookingMain == null)
                {
                    bookingMain = objBooking.GetSingleBK_TRANSMAIN(Request.QueryString["TransID"].ToString());
                       
                }
                PayScheme = bookingMain.PayScheme;
                

                listBookingDetail = objBooking.GetAllBK_TRANSDTLFilterAll(TransID, 0);
                if (listBookingDetail == null)
                {
                    if (Session["ChglstbookDTLInfo"] != null)
                    {
                        listBookingDetail = (List<BookingTransactionDetail>)Session["ChglstbookDTLInfo"];

                        //to update pay due amount and pay due date
                        if (listBookingDetail != null && listBookingDetail.Count > 0)
                        {
                            List<BookingTransactionDetail> lstOldBookingDetails = new List<BookingTransactionDetail>();
                            lstOldBookingDetails = objBooking.GetAllBK_TRANSDTLFlightByPNR(listBookingDetail[0].RecordLocator);

                            if (lstOldBookingDetails != null && lstOldBookingDetails.Count > 0)
                            {
                                for (int i = 0; i < listBookingDetail.Count; i++)
                                {
                                    for (int y = 0; y < lstOldBookingDetails.Count; y++)
                                    {
                                        if (listBookingDetail[i].RecordLocator == lstOldBookingDetails[y].RecordLocator) //listBookingDetail[i].SeqNo == lstOldBookingDetails[y].SeqNo && 
                                        {
                                            listBookingDetail[i].PayDueAmount1 = lstOldBookingDetails[y].PayDueAmount1;
                                            listBookingDetail[i].PayDueAmount2 = lstOldBookingDetails[y].PayDueAmount2;
                                            listBookingDetail[i].PayDueAmount3 = lstOldBookingDetails[y].PayDueAmount3;
                                            listBookingDetail[i].PayDueDate1 = lstOldBookingDetails[y].PayDueDate1;
                                            listBookingDetail[i].PayDueDate2 = lstOldBookingDetails[y].PayDueDate2;
                                            listBookingDetail[i].PayDueDate3 = lstOldBookingDetails[y].PayDueDate3;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                listBookingDetailCombine = objBooking.GetAllBK_TRANSDTLFlightGrp(TransID);
                if (listBookingDetailCombine == null)
                {
                    listBookingDetailCombine = new List<BookingTransactionDetail>();
                    BookingTransactionDetail objBK_TRANSDTL_Info = new BookingTransactionDetail();
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
                    objBK_TRANSDTL_Info.PayDueAmount1 = listBookingDetail.Max(item => item.PayDueAmount1);
                    objBK_TRANSDTL_Info.PayDueAmount2 = listBookingDetail.Max(item => item.PayDueAmount2);
                    objBK_TRANSDTL_Info.PayDueAmount3 = listBookingDetail.Max(item => item.PayDueAmount3);
                    objBK_TRANSDTL_Info.PayDueDate1 = listBookingDetail.Max(item => item.PayDueDate1);
                    objBK_TRANSDTL_Info.PayDueDate2 = listBookingDetail.Max(item => item.PayDueDate2);
                    objBK_TRANSDTL_Info.PayDueDate3 = listBookingDetail.Max(item => item.PayDueDate3);

                    listBookingDetailCombine.Add(objBK_TRANSDTL_Info);
                }
                HttpContext.Current.Session.Remove("listBookingDetailCombine");
                HttpContext.Current.Session.Add("listBookingDetailCombine", listBookingDetailCombine);
                objBooking.FillDataTableTransDetail(listBookingDetailCombine);

                
                HttpContext.Current.Session.Remove("ChglistBookingDetailFlight");
                HttpContext.Current.Session.Add("ChglistBookingDetailFlight", listBookingDetail);

                HttpContext.Current.Session.Remove("ChgbookingMain");
                HttpContext.Current.Session.Add("ChgbookingMain", bookingMain);

                return "";
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                return ex.ToString();
               
            }
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
            HttpContext.Current.Session.Remove("GetssrAvailabilityResponseForBooking");
            HttpContext.Current.Session.Remove("GetssrAvailabilityResponseForBookingDrink");


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
            HttpContext.Current.Session.Remove("depart1");
            HttpContext.Current.Session.Remove("transit1");
            HttpContext.Current.Session.Remove("return1");
            HttpContext.Current.Session.Remove("depart2");
            HttpContext.Current.Session.Remove("transit2");
            HttpContext.Current.Session.Remove("return2");
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
            HttpContext.Current.Session.Remove("listBookingDetail");

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

            if (Request.Cookies["AllPax"] != null)
            {
                HttpCookie cookieTemp = Request.Cookies["AllPax"];
                cookieTemp.HttpOnly = true;
                if (cookieTemp != null)
                {
                    cookieTemp.Expires = DateTime.Today.AddDays(-1);
                    Response.Cookies.Add(cookieTemp);
                }
            }
            //txt_GeustNum.Text = "10";
        }

        protected void SearchData(string TransID, string PNR)
        {
            MessageList msgList = new MessageList();
            ArrayList aMsgList = new ArrayList();
            var profiler = MiniProfiler.Current;

            try
            {
                Boolean returnFlight = false;
                returnFlight = objBooking.IsReturn(TransID, 0);
                bool ifok = true;
                HttpContext.Current.Session.Remove("invalidreturnflight");
                if (objBooking.GetTotalSuspend((MyUserSet.AgentID)) >= Convert.ToInt16(MyUserSet.MaxSuspend))
                {
                    //e.Result = msgList.Err100012;
                }
                else
                {
                    if (ifok)
                    {
                        //try to retrieve previous cookie value, and insert into variable called "cookieTemp"
                        HttpCookie cookieTemp = null;
                        int InfantNum = 0;
                        int PaxNum = 0;
                        int GuestNum = 0;
                        int ChildNum = 0;
                        ABS.Navitaire.APIBooking APIBooking = new ABS.Navitaire.APIBooking("");
                        string SellSessionID = "";// APIBooking.AgentLogon();
                        using (profiler.Step("Navitaire:AgentLogon"))
                        {
                            SellSessionID = APIBooking.AgentLogon();
                        }
                        BookingControl bookingControl = new BookingControl();
                        GetBookingResponse resp = bookingControl.GetBookingByPNR(PNR, SellSessionID);
                        if (resp != null)
                        {
                            Session["GetBookingResponse"] = resp;
                            cookieTemp = new HttpCookie("cookieSearchcondition");
                            cookieTemp.HttpOnly = true;

                            //copy all previous cookieTemp value to new cookie value
                            //cookieTemp.Values.Add("Carrier", cookieTemp.Values["Carrier"]);
                            if (resp.Booking.Journeys.Length == 1)
                            {
                                if (resp.Booking.Journeys[0].Segments.Length == 1)
                                {
                                    cookieTemp.Values.Add("Carrier", resp.Booking.Journeys[0].Segments[0].FlightDesignator.CarrierCode);
                                    cookieTemp.Values.Add("Departure", resp.Booking.Journeys[0].Segments[0].DepartureStation);
                                    cookieTemp.Values.Add("Arrival", resp.Booking.Journeys[0].Segments[0].ArrivalStation);
                                    if (returnFlight)
                                    {
                                        cookieTemp.Values.Add("ifOneWay", "FALSE");
                                        cookieTemp.Values.Add("ReturnDate", resp.Booking.Journeys[0].Segments[0].STA.ToString());
                                    }
                                    else
                                    {
                                        cookieTemp.Values.Add("ifOneWay", "TRUE");

                                    }
                                    cookieTemp.Values.Add("Currency", resp.Booking.CurrencyCode);
                                    cookieTemp.Values.Add("DepartDate", resp.Booking.Journeys[0].Segments[0].STD.ToString());

                                    for (int i = 0; i < resp.Booking.Passengers.Length; i++)
                                    {
                                        if (resp.Booking.Passengers[i].PassengerTypeInfos[0].PaxType == "ADT")
                                        {
                                            GuestNum += 1;
                                        }
                                        if (resp.Booking.Passengers[i].PassengerTypeInfos[0].PaxType == "CHD")
                                        {
                                            ChildNum += 1;
                                        }
                                        if (resp.Booking.Passengers[i].PassengerInfants.Length > 0)
                                        {
                                            InfantNum += resp.Booking.Passengers[i].PassengerInfants.Length;
                                        }
                                        PaxNum += 1;
                                        //InfantNum += ;
                                    }


                                    cookieTemp.Values.Add("PaxNum", PaxNum.ToString());
                                    cookieTemp.Values.Add("GuestNum", GuestNum.ToString());
                                    cookieTemp.Values.Add("ChildNum", ChildNum.ToString());
                                    cookieTemp.Values.Add("InfantNum", InfantNum.ToString());

                                    DataTable dt = objGeneral.GetLookUpCity(resp.Booking.Journeys[0].Segments[0].DepartureStation, Request.PhysicalApplicationPath);
                                    DataRow[] result = dt.Select("ArrivalStation = '" + resp.Booking.Journeys[0].Segments[0].ArrivalStation + "'");
                                    foreach (DataRow rows in result)
                                    {
                                        cookieTemp.Values.Add("DepartureDetail", rows[7].ToString() + "(" + resp.Booking.Journeys[0].Segments[0].DepartureStation + ")");
                                        cookieTemp.Values.Add("ArrivalDetail", rows[10].ToString() + "(" + resp.Booking.Journeys[0].Segments[0].ArrivalStation + ")");
                                    }


                                }
                                else
                                {
                                    cookieTemp.Values.Add("Carrier", resp.Booking.Journeys[0].Segments[0].FlightDesignator.CarrierCode);
                                    cookieTemp.Values.Add("Departure", resp.Booking.Journeys[0].Segments[0].DepartureStation);
                                    cookieTemp.Values.Add("Arrival", resp.Booking.Journeys[0].Segments[1].ArrivalStation);
                                    if (returnFlight)
                                    {
                                        cookieTemp.Values.Add("ifOneWay", "FALSE");
                                        cookieTemp.Values.Add("ReturnDate", resp.Booking.Journeys[0].Segments[1].STA.ToString());
                                    }
                                    else
                                    {
                                        cookieTemp.Values.Add("ifOneWay", "TRUE");

                                    }
                                    cookieTemp.Values.Add("Currency", resp.Booking.CurrencyCode);
                                    cookieTemp.Values.Add("DepartDate", resp.Booking.Journeys[0].Segments[0].STD.ToString());


                                    for (int i = 0; i < resp.Booking.Passengers.Length; i++)
                                    {
                                        if (resp.Booking.Passengers[i].PassengerTypeInfos[0].PaxType == "ADT")
                                        {
                                            GuestNum += 1;
                                        }
                                        if (resp.Booking.Passengers[i].PassengerTypeInfos[0].PaxType == "CHD")
                                        {
                                            ChildNum += 1;
                                        }
                                        if (resp.Booking.Passengers[i].PassengerInfants.Length > 0)
                                        {
                                            InfantNum += resp.Booking.Passengers[i].PassengerInfants.Length;
                                        }
                                        PaxNum += 1;
                                        //InfantNum += ;
                                    }


                                    cookieTemp.Values.Add("PaxNum", PaxNum.ToString());
                                    cookieTemp.Values.Add("GuestNum", GuestNum.ToString());
                                    cookieTemp.Values.Add("ChildNum", ChildNum.ToString());
                                    cookieTemp.Values.Add("InfantNum", InfantNum.ToString());

                                    DataTable dt = objGeneral.GetLookUpCity(resp.Booking.Journeys[0].Segments[0].DepartureStation, Request.PhysicalApplicationPath);
                                    DataRow[] result = dt.Select("ArrivalStation = '" + resp.Booking.Journeys[0].Segments[1].ArrivalStation + "'");
                                    foreach (DataRow rows in result)
                                    {
                                        cookieTemp.Values.Add("DepartureDetail", rows[7].ToString() + "(" + resp.Booking.Journeys[0].Segments[0].DepartureStation + ")");
                                        cookieTemp.Values.Add("ArrivalDetail", rows[10].ToString() + "(" + resp.Booking.Journeys[0].Segments[1].ArrivalStation + ")");
                                    }

                                }
                            }
                            else
                            {
                                if (resp.Booking.Journeys[0].Segments.Length == 1)
                                {
                                    cookieTemp.Values.Add("Carrier", resp.Booking.Journeys[0].Segments[0].FlightDesignator.CarrierCode);
                                    cookieTemp.Values.Add("Departure", resp.Booking.Journeys[0].Segments[0].DepartureStation);
                                    cookieTemp.Values.Add("Arrival", resp.Booking.Journeys[0].Segments[0].ArrivalStation);
                                    if (returnFlight)
                                    {
                                        cookieTemp.Values.Add("ifOneWay", "FALSE");
                                        cookieTemp.Values.Add("ReturnDate", resp.Booking.Journeys[1].Segments[0].STD.ToString());
                                    }
                                    else
                                    {
                                        cookieTemp.Values.Add("ifOneWay", "TRUE");

                                    }
                                    cookieTemp.Values.Add("Currency", resp.Booking.CurrencyCode);
                                    cookieTemp.Values.Add("DepartDate", resp.Booking.Journeys[0].Segments[0].STD.ToString());

                                    for (int i = 0; i < resp.Booking.Passengers.Length; i++)
                                    {
                                        if (resp.Booking.Passengers[i].PassengerTypeInfos[0].PaxType == "ADT")
                                        {
                                            GuestNum += 1;
                                        }
                                        if (resp.Booking.Passengers[i].PassengerTypeInfos[0].PaxType == "CHD")
                                        {
                                            ChildNum += 1;
                                        }
                                        if (resp.Booking.Passengers[i].PassengerInfants.Length > 0)
                                        {
                                            InfantNum += resp.Booking.Passengers[i].PassengerInfants.Length;
                                        }
                                        PaxNum += 1;
                                        //InfantNum += ;
                                    }


                                    cookieTemp.Values.Add("PaxNum", PaxNum.ToString());
                                    cookieTemp.Values.Add("GuestNum", GuestNum.ToString());
                                    cookieTemp.Values.Add("ChildNum", ChildNum.ToString());
                                    cookieTemp.Values.Add("InfantNum", InfantNum.ToString());

                                    DataTable dt = objGeneral.GetLookUpCity(resp.Booking.Journeys[0].Segments[0].DepartureStation, Request.PhysicalApplicationPath);
                                    DataRow[] result = dt.Select("ArrivalStation = '" + resp.Booking.Journeys[0].Segments[0].ArrivalStation + "'");
                                    foreach (DataRow rows in result)
                                    {
                                        cookieTemp.Values.Add("DepartureDetail", rows[7].ToString() + "(" + resp.Booking.Journeys[0].Segments[0].DepartureStation + ")");
                                        cookieTemp.Values.Add("ArrivalDetail", rows[10].ToString() + "(" + resp.Booking.Journeys[0].Segments[0].ArrivalStation + ")");
                                    }


                                }
                                else
                                {
                                    cookieTemp.Values.Add("Carrier", resp.Booking.Journeys[0].Segments[0].FlightDesignator.CarrierCode);
                                    cookieTemp.Values.Add("Departure", resp.Booking.Journeys[0].Segments[0].DepartureStation);
                                    cookieTemp.Values.Add("Arrival", resp.Booking.Journeys[0].Segments[1].ArrivalStation);
                                    if (returnFlight)
                                    {
                                        cookieTemp.Values.Add("ifOneWay", "FALSE");
                                        cookieTemp.Values.Add("ReturnDate", resp.Booking.Journeys[1].Segments[0].STD.ToString());
                                    }
                                    else
                                    {
                                        cookieTemp.Values.Add("ifOneWay", "TRUE");

                                    }
                                    cookieTemp.Values.Add("Currency", resp.Booking.CurrencyCode);
                                    cookieTemp.Values.Add("DepartDate", resp.Booking.Journeys[0].Segments[0].STD.ToString());


                                    for (int i = 0; i < resp.Booking.Passengers.Length; i++)
                                    {
                                        if (resp.Booking.Passengers[i].PassengerTypeInfos[0].PaxType == "ADT")
                                        {
                                            GuestNum += 1;
                                        }
                                        if (resp.Booking.Passengers[i].PassengerTypeInfos[0].PaxType == "CHD")
                                        {
                                            ChildNum += 1;
                                        }
                                        if (resp.Booking.Passengers[i].PassengerInfants.Length > 0)
                                        {
                                            InfantNum += resp.Booking.Passengers[i].PassengerInfants.Length;
                                        }
                                        PaxNum += 1;
                                        //InfantNum += ;
                                    }


                                    cookieTemp.Values.Add("PaxNum", PaxNum.ToString());
                                    cookieTemp.Values.Add("GuestNum", GuestNum.ToString());
                                    cookieTemp.Values.Add("ChildNum", ChildNum.ToString());
                                    cookieTemp.Values.Add("InfantNum", InfantNum.ToString());

                                    DataTable dt = objGeneral.GetLookUpCity(resp.Booking.Journeys[0].Segments[0].DepartureStation, Request.PhysicalApplicationPath);
                                    DataRow[] result = dt.Select("ArrivalStation = '" + resp.Booking.Journeys[0].Segments[1].ArrivalStation + "'");
                                    foreach (DataRow rows in result)
                                    {
                                        cookieTemp.Values.Add("DepartureDetail", rows[7].ToString() + "(" + resp.Booking.Journeys[0].Segments[0].DepartureStation + ")");
                                        cookieTemp.Values.Add("ArrivalDetail", rows[10].ToString() + "(" + resp.Booking.Journeys[0].Segments[1].ArrivalStation + ")");
                                    }

                                }
                            }


                        }


                        //create new variable with same cookie name
                        HttpCookie cookie = new HttpCookie("cookieSearchcondition");
                        cookie.HttpOnly = true;

                        //copy all previous cookieTemp value to new cookie value
                        cookie.Values.Add("Carrier", cookieTemp.Values["Carrier"]);
                        cookie.Values.Add("Departure", cookieTemp.Values["Departure"]);
                        cookie.Values.Add("Arrival", cookieTemp.Values["Arrival"]);
                        cookie.Values.Add("ifOneWay", cookieTemp.Values["ifOneWay"]);
                        cookie.Values.Add("Currency", cookieTemp.Values["Currency"]);
                        cookie.Values.Add("DepartDate", cookieTemp.Values["DepartDate"]);
                        cookie.Values.Add("ReturnDate", cookieTemp.Values["ReturnDate"]);
                        cookie.Values.Add("PaxNum", cookieTemp.Values["PaxNum"]);
                        cookie.Values.Add("GuestNum", cookieTemp.Values["GuestNum"]);
                        cookie.Values.Add("ChildNum", cookieTemp.Values["ChildNum"]);
                        cookie.Values.Add("InfantNum", cookieTemp.Values["InfantNum"]);
                        cookie.Values.Add("DepartureDetail", cookieTemp.Values["DepartureDetail"]);
                        cookie.Values.Add("ArrivalDetail", cookieTemp.Values["ArrivalDetail"]);

                        //check if depart (mode=0), add the day to depart,
                        //check if return (mode=1), add the day to return,
                        //if (mode == 0)
                        //    cookie.Values["DepartDate"] = Convert.ToDateTime(cookie.Values["DepartDate"]).AddDays(days).ToString();
                        //else if (mode == 1)
                        //    cookie.Values["ReturnDate"] = Convert.ToDateTime(cookie.Values["ReturnDate"]).AddDays(days).ToString();

                        //delete cookieTemp (previous cookie)
                        if (cookieTemp != null)
                        {
                            cookieTemp.Expires = DateTime.Today.AddDays(-1);
                            Response.Cookies.Add(cookieTemp);
                        }

                        //add new cookie to Cookie
                        Response.AppendCookie(cookie);


                        //check available flight, this code is similar to search button code, to show the flight list
                        model = new ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition();

                        model.Carrier = cookie.Values["Carrier"];//   ViewState["Carrier"].ToString();
                        model.Departure = cookie.Values["Departure"];// ViewState["Departure"].ToString();
                        model.Arrival = cookie.Values["Arrival"];// ViewState["Arrival"].ToString();
                        bool oneway = false;
                        if (cookie.Values["ifOneWay"] == "TRUE")
                        {
                            oneway = true;
                        }
                        else
                        {
                            model.ReturnDate = Convert.ToDateTime(cookie.Values["ReturnDate"]);
                        }
                        model.ifOneWay = oneway;//  Convert.ToBoolean(ViewState["ifOneWay"]);
                        model.Currency = cookie.Values["Currency"];// ViewState["Currency"].ToString();
                        model.DepartDate = Convert.ToDateTime(cookie.Values["DepartDate"]);// Convert.ToDateTime(ViewState["DepartDate"]);
                        model.ReturnDate = Convert.ToDateTime(cookie.Values["ReturnDate"]);//  Convert.ToDateTime(ViewState["ReturnDate"]);
                        model.GuestNum = Convert.ToInt32(cookie.Values["GuestNum"]);//  Convert.ToInt32(ViewState["GuestNum"]);
                        model.PaxNum = Convert.ToInt32(cookie.Values["PaxNum"]);
                        model.ChildNum = Convert.ToInt32(cookie.Values["ChildNum"]);
                        model.InfantNum = Convert.ToInt32(cookie.Values["InfantNum"]);

                        //insert temFlight into session

                        // objBooking.tempFlight(model, MyUserSet.AgentName, "");

                        DataList dtModel1 = new DataList();
                        DataList dtModel2 = new DataList();

                        DataTable tempDt = new DataTable();
                        tempDt = objBooking.dtFlight();

                        //added by ketee, validate valid flight
                        if (HttpContext.Current.Session["invalidreturnflight"] != null && HttpContext.Current.Session["invalidreturnflight"].ToString() == "1")
                        {
                            ifok = false;
                            //e.Result = msgList.Err100041;
                        }

                        if (ifok)
                        {
                            if (oneway == false)
                            {
                                if (HttpContext.Current.Session["tempFlight"] != null)
                                {
                                    //
                                    string strExpr;
                                    string strSort;
                                    DataTable dt = new DataTable();

                                    strExpr = "TemFlightDeparture = '" + model.Departure + "' AND TemFlightArrival = '" + model.Arrival + "' AND TemFlightAgentName = '" + MyUserSet.LoginName + "' AND TemFlightIfReturn = false";

                                    strSort = "";
                                    // Use the Select method to find all rows matching the filter.

                                    dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                                    DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                                    tempDt.Clear();

                                    foreach (DataRow row in foundRows)
                                    {
                                        tempDt.ImportRow(row);
                                    }

                                    dtModel1.DataSource = tempDt;
                                    dtModel1.DataBind();

                                    strExpr = "TemFlightDeparture = '" + model.Arrival + "' AND TemFlightArrival = '" + model.Departure + "' AND TemFlightAgentName = '" + MyUserSet.LoginName + "' AND TemFlightIfReturn = true";

                                    strSort = "";

                                    foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                                    tempDt.Clear();
                                    foreach (DataRow row in foundRows)
                                    {
                                        tempDt.ImportRow(row);
                                    }

                                    dtModel2.DataSource = tempDt;
                                    dtModel2.DataBind();
                                    if (dtModel1.Items.Count <= 0 || dtModel2.Items.Count <= 0)
                                    {
                                        ifok = false;
                                        //e.Result = msgList.Err100013;
                                    }
                                }
                                else
                                {
                                    ifok = false;
                                    //e.Result = msgList.Err100013;
                                }  //end

                            }
                            else
                            {

                                //johan remark
                                if (HttpContext.Current.Session["tempFlight"] != null)
                                {
                                    string strExpr;
                                    string strSort;
                                    DataTable dt = new DataTable();

                                    strExpr = "TemFlightDeparture = '" + model.Departure + "' AND TemFlightArrival = '" + model.Arrival + "' AND TemFlightAgentName = '" + MyUserSet.LoginName + "' AND TemFlightIfReturn = false";

                                    strSort = "";
                                    // Use the Select method to find all rows matching the filter.

                                    dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                                    DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                                    tempDt.Clear();

                                    foreach (DataRow row in foundRows)
                                    {
                                        tempDt.ImportRow(row);
                                    }
                                    //

                                    //dtModel1.DataSource = (DataTable)HttpContext.Current.Session["tempFlightDepart"];
                                    dtModel1.DataSource = tempDt;
                                    dtModel1.DataBind();

                                    if (dtModel1.Items.Count <= 0)
                                    {
                                        ifok = false;
                                        //e.Result = msgList.Err100013;
                                    }
                                }
                                else
                                {
                                    ifok = false;
                                    //e.Result = msgList.Err100013;
                                }

                            }
                        }

                        //For Show List Flight Schedule When Click Other Date
                        if (cookie != null)
                        {
                            model.Carrier = cookie.Values["Carrier"];//   ViewState["Carrier"].ToString();
                            model.Departure = cookie.Values["Departure"];// ViewState["Departure"].ToString();
                            model.Arrival = cookie.Values["Arrival"];// ViewState["Arrival"].ToString();
                            oneway = false;
                            if (cookie.Values["ifOneWay"] == "TRUE")
                            {
                                oneway = true;
                                tr_Return.Visible = false;
                            }
                            else
                            {
                                model.ReturnDate = Convert.ToDateTime(cookie.Values["ReturnDate"]);
                            }
                            model.ifOneWay = oneway;//  Convert.ToBoolean(ViewState["ifOneWay"]);
                            model.Currency = cookie.Values["Currency"];// ViewState["Currency"].ToString();
                            model.DepartDate = Convert.ToDateTime(cookie.Values["DepartDate"]);// Convert.ToDateTime(ViewState["DepartDate"]);
                            model.ReturnDate = Convert.ToDateTime(cookie.Values["ReturnDate"]);//  Convert.ToDateTime(ViewState["ReturnDate"]);
                            model.GuestNum = Convert.ToInt32(cookie.Values["GuestNum"]);//  Convert.ToInt32(ViewState["GuestNum"]);
                            model.PaxNum = Convert.ToInt32(cookie.Values["PaxNum"]);
                            model.ChildNum = Convert.ToInt32(cookie.Values["ChildNum"]);
                            model.InfantNum = Convert.ToInt32(cookie.Values["InfantNum"]);


                        }

                    }
                }
                //InitializeForm();
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //e.Result = msgList.Err100031;
                log.Error(this, ex);
                //Response.Redirect(Shared.MySite.PublicPages.AgentMain, false);
            }
        }

        protected void SearchDate(int mode, int days)
        {
            MessageList msgList = new MessageList();
            ArrayList aMsgList = new ArrayList();

            try
            {
                bool ifok = true;
                HttpContext.Current.Session.Remove("invalidreturnflight");
                objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl(); 
                if (objBooking.GetTotalSuspend((MyUserSet.AgentID)) >= Convert.ToInt16(MyUserSet.MaxSuspend))
                {
                    //e.Result = msgList.Err100012;
                }
                else
                {
                    if (ifok)
                    {
                        //try to retrieve previous cookie value, and insert into variable called "cookieTemp"
                        HttpCookie cookieTemp = null;
                        if (Request.Cookies["cookieSearchcondition"] != null)
                        {
                            cookieTemp = Request.Cookies["cookieSearchcondition"];
                            cookieTemp.HttpOnly = true;

                        }
                        //else
                        //{
                        //int InfantNum = 0;
                        //int PaxNum = 0;
                        //int GuestNum = 0;
                        //int ChildNum = 0;
                        //ABS.Navitaire.APIBooking APIBooking = new ABS.Navitaire.APIBooking("");
                        //string SellSessionID = APIBooking.AgentLogon();
                        //BookingControl bookingControl = new BookingControl();
                        //GetBookingResponse resp = bookingControl.GetBookingByPNR(PNR, SellSessionID);
                        //if (resp != null)
                        //{
                        //    cookieTemp = new HttpCookie("cookieSearchcondition");
                        //    cookieTemp.HttpOnly = true;
                        //    Boolean returnFlight = false;
                        //    returnFlight = objBooking.IsReturn(TransID, 0);
                        //    //copy all previous cookieTemp value to new cookie value
                        //    //cookieTemp.Values.Add("Carrier", cookieTemp.Values["Carrier"]);
                        //    if (resp.Booking.Journeys[0].Segments.Length == 1)
                        //    {
                        //        cookieTemp.Values.Add("Carrier", resp.Booking.Journeys[0].Segments[0].FlightDesignator.CarrierCode);
                        //        cookieTemp.Values.Add("Departure", resp.Booking.Journeys[0].Segments[0].DepartureStation);
                        //        cookieTemp.Values.Add("Arrival", resp.Booking.Journeys[0].Segments[0].ArrivalStation);
                        //        if (returnFlight)
                        //        {
                        //            cookieTemp.Values.Add("ifOneWay", "TRUE");

                        //        }
                        //        else
                        //        {
                        //            cookieTemp.Values.Add("ifOneWay", "FALSE");
                        //            cookieTemp.Values.Add("ReturnDate", resp.Booking.Journeys[0].Segments[0].STA.ToString());
                        //        }
                        //        cookieTemp.Values.Add("Currency", resp.Booking.CurrencyCode);
                        //        cookieTemp.Values.Add("DepartDate", resp.Booking.Journeys[0].Segments[0].STD.ToString());

                        //        for (int i = 0; i < resp.Booking.Passengers.Length; i++)
                        //        {
                        //            if (resp.Booking.Passengers[i].PassengerTypeInfos[0].PaxType == "ADT")
                        //            {
                        //               GuestNum += 1;
                        //            }
                        //            if (resp.Booking.Passengers[i].PassengerTypeInfos[0].PaxType == "CHD")
                        //            {
                        //                ChildNum += 1;
                        //            }
                        //            if (resp.Booking.Passengers[i].PassengerInfants.Length > 0)
                        //            {
                        //                InfantNum = resp.Booking.Passengers[i].PassengerInfants.Length;
                        //            }
                        //            PaxNum += 1;
                        //            //InfantNum += ;
                        //        }


                        //        cookieTemp.Values.Add("PaxNum", PaxNum.ToString());
                        //        cookieTemp.Values.Add("GuestNum", GuestNum.ToString());
                        //        cookieTemp.Values.Add("ChildNum", ChildNum.ToString());
                        //        cookieTemp.Values.Add("InfantNum", InfantNum.ToString());

                        //        DataTable dt = objGeneral.GetLookUpCity(resp.Booking.Journeys[0].Segments[0].DepartureStation, Request.PhysicalApplicationPath);
                        //        DataRow[] result = dt.Select("ArrivalStation = '" + resp.Booking.Journeys[0].Segments[0].ArrivalStation + "'");
                        //        foreach (DataRow rows in result)
                        //        {
                        //                cookieTemp.Values.Add("DepartureDetail", rows[7].ToString() + "(" + resp.Booking.Journeys[0].Segments[0].DepartureStation + ")" );
                        //                cookieTemp.Values.Add("ArrivalDetail", rows[10].ToString() + "(" + resp.Booking.Journeys[0].Segments[0].ArrivalStation + ")");
                        //        }


                        //    }
                        //    else
                        //    {
                        //            cookieTemp.Values.Add("Carrier", resp.Booking.Journeys[0].Segments[0].FlightDesignator.CarrierCode);
                        //            cookieTemp.Values.Add("Departure", resp.Booking.Journeys[0].Segments[0].DepartureStation);
                        //            cookieTemp.Values.Add("Arrival", resp.Booking.Journeys[0].Segments[1].ArrivalStation);
                        //            if (returnFlight)
                        //            {
                        //                cookieTemp.Values.Add("ifOneWay", "TRUE");

                        //            }
                        //            else
                        //            {
                        //                cookieTemp.Values.Add("ifOneWay", "FALSE");
                        //                cookieTemp.Values.Add("ReturnDate", resp.Booking.Journeys[0].Segments[1].STA.ToString());
                        //            }
                        //            cookieTemp.Values.Add("Currency", resp.Booking.CurrencyCode);
                        //            cookieTemp.Values.Add("DepartDate", resp.Booking.Journeys[0].Segments[0].STD.ToString());


                        //            for (int i = 0; i < resp.Booking.Passengers.Length; i++)
                        //            {
                        //                if (resp.Booking.Passengers[i].PassengerTypeInfos[0].PaxType == "ADT")
                        //                {
                        //                    GuestNum += 1;
                        //                }
                        //                if (resp.Booking.Passengers[i].PassengerTypeInfos[0].PaxType == "CHD")
                        //                {
                        //                    ChildNum += 1;
                        //                }
                        //                if (resp.Booking.Passengers[i].PassengerInfants.Length > 0)
                        //                {
                        //                    InfantNum = resp.Booking.Passengers[i].PassengerInfants.Length;
                        //                }
                        //                PaxNum += 1;
                        //                //InfantNum += ;
                        //            }


                        //            cookieTemp.Values.Add("PaxNum", PaxNum.ToString());
                        //            cookieTemp.Values.Add("GuestNum", GuestNum.ToString());
                        //            cookieTemp.Values.Add("ChildNum", ChildNum.ToString());
                        //            cookieTemp.Values.Add("InfantNum", InfantNum.ToString());

                        //            DataTable dt = objGeneral.GetLookUpCity(resp.Booking.Journeys[0].Segments[0].DepartureStation, Request.PhysicalApplicationPath);
                        //            DataRow[] result = dt.Select("ArrivalStation = '" + resp.Booking.Journeys[0].Segments[1].ArrivalStation + "'");
                        //            foreach (DataRow rows in result)
                        //            {
                        //                cookieTemp.Values.Add("DepartureDetail", rows[7].ToString() + "(" + resp.Booking.Journeys[0].Segments[0].DepartureStation + ")");
                        //                cookieTemp.Values.Add("ArrivalDetail", rows[10].ToString() + "(" + resp.Booking.Journeys[0].Segments[1].ArrivalStation + ")");
                        //            }

                        //    }


                        //}
                        //}

                        //create new variable with same cookie name
                        HttpCookie cookie = new HttpCookie("cookieSearchcondition");
                        cookie.HttpOnly = true;

                        //copy all previous cookieTemp value to new cookie value
                        cookie.Values.Add("Carrier", cookieTemp.Values["Carrier"]);
                        cookie.Values.Add("Departure", cookieTemp.Values["Departure"]);
                        cookie.Values.Add("Arrival", cookieTemp.Values["Arrival"]);
                        cookie.Values.Add("ifOneWay", cookieTemp.Values["ifOneWay"]);
                        cookie.Values.Add("Currency", cookieTemp.Values["Currency"]);
                        cookie.Values.Add("DepartDate", cookieTemp.Values["DepartDate"]);
                        cookie.Values.Add("ReturnDate", cookieTemp.Values["ReturnDate"]);
                        cookie.Values.Add("PaxNum", cookieTemp.Values["PaxNum"]);
                        cookie.Values.Add("GuestNum", cookieTemp.Values["GuestNum"]);
                        cookie.Values.Add("ChildNum", cookieTemp.Values["ChildNum"]);
                        cookie.Values.Add("InfantNum", cookieTemp.Values["InfantNum"]);
                        cookie.Values.Add("DepartureDetail", cookieTemp.Values["DepartureDetail"]);
                        cookie.Values.Add("ArrivalDetail", cookieTemp.Values["ArrivalDetail"]);

                        //check if depart (mode=0), add the day to depart,
                        //check if return (mode=1), add the day to return,
                        if (mode == 0)
                            cookie.Values["DepartDate"] = Convert.ToDateTime(cookie.Values["DepartDate"]).AddDays(days).ToString();
                        else if (mode == 1)
                           cookie.Values["ReturnDate"] = Convert.ToDateTime(cookie.Values["ReturnDate"]).AddDays(days).ToString();

                        //delete cookieTemp (previous cookie)
                        if (cookieTemp != null)
                        {
                            cookieTemp.Expires = DateTime.Today.AddDays(-1);
                            Response.Cookies.Add(cookieTemp);
                        }

                        //add new cookie to Cookie
                        Response.AppendCookie(cookie);


                        //check available flight, this code is similar to search button code, to show the flight list
                        model = new ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition();

                        model.Carrier = cookie.Values["Carrier"];//   ViewState["Carrier"].ToString();
                        model.Departure = cookie.Values["Departure"];// ViewState["Departure"].ToString();
                        model.Arrival = cookie.Values["Arrival"];// ViewState["Arrival"].ToString();
                        bool oneway = false;
                        if (cookie.Values["ifOneWay"] == "TRUE")
                        {
                            oneway = true;
                        }
                        else
                        {
                            model.ReturnDate = Convert.ToDateTime(cookie.Values["ReturnDate"]);
                        }
                        model.ifOneWay = oneway;//  Convert.ToBoolean(ViewState["ifOneWay"]);
                        model.Currency = cookie.Values["Currency"];// ViewState["Currency"].ToString();
                        model.DepartDate = Convert.ToDateTime(cookie.Values["DepartDate"]);// Convert.ToDateTime(ViewState["DepartDate"]);
                        model.ReturnDate = Convert.ToDateTime(cookie.Values["ReturnDate"]);//  Convert.ToDateTime(ViewState["ReturnDate"]);
                        model.GuestNum = Convert.ToInt32(cookie.Values["GuestNum"]);//  Convert.ToInt32(ViewState["GuestNum"]);
                        model.PaxNum = Convert.ToInt32(cookie.Values["PaxNum"]);
                        model.ChildNum = Convert.ToInt32(cookie.Values["ChildNum"]);
                        model.InfantNum = Convert.ToInt32(cookie.Values["InfantNum"]);

                        //insert temFlight into session

                        objBooking.tempFlight(model, MyUserSet.AgentName, "");

                        DataList dtModel1 = new DataList();
                        DataList dtModel2 = new DataList();

                        DataTable tempDt = new DataTable();
                        tempDt = objBooking.dtFlight();

                        //added by ketee, validate valid flight
                        if (HttpContext.Current.Session["invalidreturnflight"] != null && HttpContext.Current.Session["invalidreturnflight"].ToString() == "1")
                        {
                            ifok = false;
                            //e.Result = msgList.Err100041;
                        }

                        if (ifok)
                        {
                            if (oneway == false)
                            {
                                if (HttpContext.Current.Session["tempFlight"] != null)
                                {
                                    //
                                    string strExpr;
                                    string strSort;
                                    DataTable dt = new DataTable();

                                    strExpr = "TemFlightDeparture = '" + model.Departure + "' AND TemFlightArrival = '" + model.Arrival + "' AND TemFlightAgentName = '" + MyUserSet.LoginName + "' AND TemFlightIfReturn = false";

                                    strSort = "";
                                    // Use the Select method to find all rows matching the filter.

                                    dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                                    DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                                    tempDt.Clear();

                                    foreach (DataRow row in foundRows)
                                    {
                                        tempDt.ImportRow(row);
                                    }

                                    dtModel1.DataSource = tempDt;
                                    dtModel1.DataBind();

                                    strExpr = "TemFlightDeparture = '" + model.Arrival + "' AND TemFlightArrival = '" + model.Departure + "' AND TemFlightAgentName = '" + MyUserSet.LoginName + "' AND TemFlightIfReturn = true";

                                    strSort = "";

                                    foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                                    tempDt.Clear();
                                    foreach (DataRow row in foundRows)
                                    {
                                        tempDt.ImportRow(row);
                                    }

                                    dtModel2.DataSource = tempDt;
                                    dtModel2.DataBind();
                                    if (dtModel1.Items.Count <= 0 || dtModel2.Items.Count <= 0)
                                    {
                                        ifok = false;
                                        //e.Result = msgList.Err100013;
                                    }
                                }
                                else
                                {
                                    ifok = false;
                                    //e.Result = msgList.Err100013;
                                }  //end

                            }
                            else
                            {

                                //johan remark
                                if (HttpContext.Current.Session["tempFlight"] != null)
                                {
                                    string strExpr;
                                    string strSort;
                                    DataTable dt = new DataTable();

                                    strExpr = "TemFlightDeparture = '" + model.Departure + "' AND TemFlightArrival = '" + model.Arrival + "' AND TemFlightAgentName = '" + MyUserSet.LoginName + "' AND TemFlightIfReturn = false";

                                    strSort = "";
                                    // Use the Select method to find all rows matching the filter.

                                    dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                                    DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                                    tempDt.Clear();

                                    foreach (DataRow row in foundRows)
                                    {
                                        tempDt.ImportRow(row);
                                    }
                                    //

                                    //dtModel1.DataSource = (DataTable)HttpContext.Current.Session["tempFlightDepart"];
                                    dtModel1.DataSource = tempDt;
                                    dtModel1.DataBind();

                                    if (dtModel1.Items.Count <= 0)
                                    {
                                        ifok = false;
                                        //e.Result = msgList.Err100013;
                                    }
                                }
                                else
                                {
                                    ifok = false;
                                    //e.Result = msgList.Err100013;
                                }

                            }
                        }

                        //For Show List Flight Schedule When Click Other Date
                        if (cookie != null)
                        {
                            model.Carrier = cookie.Values["Carrier"];//   ViewState["Carrier"].ToString();
                            model.Departure = cookie.Values["Departure"];// ViewState["Departure"].ToString();
                            model.Arrival = cookie.Values["Arrival"];// ViewState["Arrival"].ToString();
                            oneway = false;
                            if (cookie.Values["ifOneWay"] == "TRUE")
                            {
                                oneway = true;
                                tr_Return.Visible = false;
                            }
                            else
                            {
                                model.ReturnDate = Convert.ToDateTime(cookie.Values["ReturnDate"]);
                            }
                            model.ifOneWay = oneway;//  Convert.ToBoolean(ViewState["ifOneWay"]);
                            model.Currency = cookie.Values["Currency"];// ViewState["Currency"].ToString();
                            model.DepartDate = Convert.ToDateTime(cookie.Values["DepartDate"]);// Convert.ToDateTime(ViewState["DepartDate"]);
                            model.ReturnDate = Convert.ToDateTime(cookie.Values["ReturnDate"]);//  Convert.ToDateTime(ViewState["ReturnDate"]);
                            model.GuestNum = Convert.ToInt32(cookie.Values["GuestNum"]);//  Convert.ToInt32(ViewState["GuestNum"]);
                            model.PaxNum = Convert.ToInt32(cookie.Values["PaxNum"]);
                            model.ChildNum = Convert.ToInt32(cookie.Values["ChildNum"]);
                            model.InfantNum = Convert.ToInt32(cookie.Values["InfantNum"]);

                            string temp = "";
                            temp = cookie.Values["DepartureDetail"];
                            lbl_Go1.Text = temp.Replace("|", " | ");
                            //lbl_Go1.Text = cookie.Values["DepartureDetail"];
                            temp = cookie.Values["Arrival"];
                            if (Session["dtCountryList"] != null)
                            {
                                DataTable dt = (DataTable)Session["dtCountryList"];
                                DataRow[] resultBaggage = dt.Select("ArrivalStation = '" + temp + "'");
                                foreach (DataRow rows in resultBaggage)
                                {
                                    if (cookie.Values["ArrivalDetail"] != "")
                                    {
                                        temp = cookie.Values["ArrivalDetail"] + " | " + rows[17].ToString();
                                    }
                                    else
                                    {
                                        temp = rows[19].ToString().Replace("|", " | ");
                                    }

                                    Session["Country"] = rows[9].ToString();
                                }
                            }
                            else
                            {
                                DataTable dt = objGeneral.GetLookUpCity(cookie.Values["Departure"], Request.PhysicalApplicationPath);
                                DataRow[] resultBaggage = dt.Select("ArrivalStation = '" + temp + "'");
                                foreach (DataRow rows in resultBaggage)
                                {
                                    if (cookie.Values["ArrivalDetail"] != "")
                                    {
                                        temp = cookie.Values["ArrivalDetail"] + " | " + rows[17].ToString();
                                    }
                                    else
                                    {
                                        temp = rows[19].ToString().Replace("|", " | ");
                                    }

                                    Session["Country"] = rows[9].ToString();
                                }
                            }

                            lbl_Go2.Text = temp;
                            //lbl_Go2.Text = cookie.Values["ArrivalDetail"];

                            lbl_Return1.Text = lbl_Go2.Text;
                            //lbl_Return1.Text = cookie.Values["ArrivalDetail"];
                            lbl_Return2.Text = lbl_Go1.Text;
                            //lbl_Return2.Text = cookie.Values["DepartureDetail"];                

                            tempDt = objBooking.dtFlight();


                            BindList1(model, tempDt);
                            if (oneway == false)
                            {
                                BindList2(model, tempDt);
                                if (dvSelectFlight.Items.Count <= 0 || gvSelectFlightReturn.Items.Count <= 0)
                                {
                                    tr_Depart.Style.Add("display", "none");
                                    tr_Return.Style.Add("display", "none");
                                    tr_Return.Visible = false;
                                    btn_Next.Visible = false;
                                    btnContinueBottom.Visible = false;
                                    //Response.Redirect(Shared.MySite.PublicPages.Selectflight);
                                }
                                else
                                {
                                    tr_Depart.Style.Add("display", "block");
                                    tr_Return.Style.Add("display", "block");
                                    tr_Return.Visible = true;
                                    btn_Next.Visible = true;
                                    btnContinueBottom.Visible = true;
                                }
                            }
                            else
                            {
                                if (dvSelectFlight.Items.Count <= 0)
                                {
                                    tr_Depart.Style.Add("display", "none");
                                    btn_Next.Visible = false;
                                    btnContinueBottom.Visible = false;
                                    //Response.Redirect(Shared.MySite.PublicPages.Selectflight);
                                }
                                else
                                {
                                    tr_Depart.Style.Add("display", "block");
                                    btn_Next.Visible = true;
                                    btnContinueBottom.Visible = true;
                                }
                            }

                        }

                    }
                }
                //InitializeForm();
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //e.Result = msgList.Err100031;
                log.Error(this, ex);
                //Response.Redirect(Shared.MySite.PublicPages.AgentMain, false);
            }
        }

        protected void ResearchFlight(int mode, int days)
        {
            MessageList msgList = new MessageList();
            ArrayList aMsgList = new ArrayList();

            try
            {
                bool ifok = true;
                HttpContext.Current.Session.Remove("invalidreturnflight");
                if (objBooking.GetTotalSuspend((MyUserSet.AgentID)) >= Convert.ToInt16(MyUserSet.MaxSuspend))
                {
                    //e.Result = msgList.Err100012;
                }
                else
                {
                    if (ifok)
                    {
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

                        HttpCookie cookie = new HttpCookie("cookieSearchcondition");
                        cookie.HttpOnly = true;
                        int num = 0;
                        num = Convert.ToInt32(txt_GeustNum.Text) + Convert.ToInt32(txt_ChildNum.Text);


                        cookie.Values.Add("Departure", ddlDeparture.SelectedValue);
                        //cookie.Values.Add("Arrival", ddlReturn.SelectedValue);
                        cookie.Values.Add("Arrival", hfArrival.Value);
                        Boolean returnFlight = false;
                        returnFlight = objBooking.IsReturn(TransID, 0);
                        if (returnFlight == false)
                        {
                            cookie.Values.Add("ifOneWay", "TRUE");
                        }
                        else
                        {
                            cookie.Values.Add("ifOneWay", "FALSE");
                        }

                        cookie.Values.Add("Currency", hfCurrency.Value);
                        cookie.Values.Add("DepartDate", daStart.Value.ToString());
                        if (returnFlight)
                        {
                            cookie.Values.Add("ReturnDate", daEnd.Value.ToString());
                        }
                        cookie.Values.Add("PaxNum", num.ToString());
                        cookie.Values.Add("GuestNum", txt_GeustNum.Text);
                        cookie.Values.Add("ChildNum", txt_ChildNum.Text);
                        cookie.Values.Add("InfantNum", txt_InfantNum.Text);
                        cookie.Values.Add("DepartureDetail", ddlDeparture.SelectedItem.Text);
                        //cookie.Values.Add("ArrivalDetail", ddlReturn.SelectedItem.Text);
                        cookie.Values.Add("ArrivalDetail", hfArrivalText.Value);
                        Response.AppendCookie(cookie);
                        //check available flight

                        //ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition model = new ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition();

                        model = new ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition();
                        model.Carrier = cookie.Values["Carrier"];//   ViewState["Carrier"].ToString();
                        model.Departure = cookie.Values["Departure"];// ViewState["Departure"].ToString();
                        model.Arrival = cookie.Values["Arrival"];// ViewState["Arrival"].ToString();
                        bool oneway = false;
                        if (cookie.Values["ifOneWay"] == "TRUE")
                        {
                            oneway = true;
                        }
                        else
                        {
                            model.ReturnDate = Convert.ToDateTime(cookie.Values["ReturnDate"]);
                        }
                        model.ifOneWay = oneway;//  Convert.ToBoolean(ViewState["ifOneWay"]);
                        model.Currency = cookie.Values["Currency"];// ViewState["Currency"].ToString();
                        model.DepartDate = Convert.ToDateTime(cookie.Values["DepartDate"]);// Convert.ToDateTime(ViewState["DepartDate"]);
                        model.ReturnDate = Convert.ToDateTime(cookie.Values["ReturnDate"]);//  Convert.ToDateTime(ViewState["ReturnDate"]);
                        model.GuestNum = Convert.ToInt32(cookie.Values["GuestNum"]);//  Convert.ToInt32(ViewState["GuestNum"]);
                        model.PaxNum = Convert.ToInt32(cookie.Values["PaxNum"]);
                        model.ChildNum = Convert.ToInt32(cookie.Values["ChildNum"]);
                        model.InfantNum = Convert.ToInt32(cookie.Values["InfantNum"]);

                        //insert temFlight into session

                        objBooking.tempFlight(model, MyUserSet.AgentName, "");

                        DataList dtModel1 = new DataList();
                        DataList dtModel2 = new DataList();

                        DataTable tempDt = new DataTable();
                        tempDt = objBooking.dtFlight();

                        //added by ketee, validate valid flight
                        if (HttpContext.Current.Session["invalidreturnflight"] != null && HttpContext.Current.Session["invalidreturnflight"].ToString() == "1")
                        {
                            ifok = false;
                            //e.Result = msgList.Err100041;
                        }

                        if (ifok)
                        {
                            if (oneway == false)
                            {
                                if (HttpContext.Current.Session["tempFlight"] != null)
                                {
                                    //
                                    string strExpr;
                                    string strSort;
                                    DataTable dt = new DataTable();

                                    strExpr = "TemFlightDeparture = '" + model.Departure + "' AND TemFlightArrival = '" + model.Arrival + "' AND TemFlightAgentName = '" + MyUserSet.LoginName + "' AND TemFlightIfReturn = false";

                                    strSort = "";
                                    // Use the Select method to find all rows matching the filter.

                                    dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                                    DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                                    tempDt.Clear();

                                    foreach (DataRow row in foundRows)
                                    {
                                        tempDt.ImportRow(row);
                                    }

                                    dtModel1.DataSource = tempDt;
                                    dtModel1.DataBind();

                                    strExpr = "TemFlightDeparture = '" + model.Arrival + "' AND TemFlightArrival = '" + model.Departure + "' AND TemFlightAgentName = '" + MyUserSet.LoginName + "' AND TemFlightIfReturn = true";

                                    strSort = "";

                                    foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                                    tempDt.Clear();
                                    foreach (DataRow row in foundRows)
                                    {
                                        tempDt.ImportRow(row);
                                    }

                                    dtModel2.DataSource = tempDt;
                                    dtModel2.DataBind();
                                    if (dtModel1.Items.Count <= 0 || dtModel2.Items.Count <= 0)
                                    {
                                        ifok = false;
                                        //e.Result = msgList.Err100013;
                                    }
                                }
                                else
                                {
                                    ifok = false;
                                    //e.Result = msgList.Err100013;
                                }  //end

                            }
                            else
                            {

                                //johan remark
                                if (HttpContext.Current.Session["tempFlight"] != null)
                                {
                                    string strExpr;
                                    string strSort;
                                    DataTable dt = new DataTable();

                                    strExpr = "TemFlightDeparture = '" + model.Departure + "' AND TemFlightArrival = '" + model.Arrival + "' AND TemFlightAgentName = '" + MyUserSet.LoginName + "' AND TemFlightIfReturn = false";

                                    strSort = "";
                                    // Use the Select method to find all rows matching the filter.

                                    dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                                    DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                                    tempDt.Clear();

                                    foreach (DataRow row in foundRows)
                                    {
                                        tempDt.ImportRow(row);
                                    }
                                    //

                                    //dtModel1.DataSource = (DataTable)HttpContext.Current.Session["tempFlightDepart"];
                                    dtModel1.DataSource = tempDt;
                                    dtModel1.DataBind();

                                    if (dtModel1.Items.Count <= 0)
                                    {
                                        ifok = false;
                                        //e.Result = msgList.Err100013;
                                    }
                                }
                                else
                                {
                                    ifok = false;
                                    //e.Result = msgList.Err100013;
                                }

                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //e.Result = msgList.Err100031;
                log.Error(this, ex);
                //Response.Redirect(Shared.MySite.PublicPages.AgentMain, false);
            }
        }

        protected void SearchDate_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter != null && e.Parameter != "" && e.Parameter.Trim() == "loadReturnDate")
            {
                ddlDeparture_SelectedIndexChanged(sender, e);

                return;
            }
        }

        protected void SelectFlightPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            MessageList msgList = new MessageList();
            ArrayList aMsgList = new ArrayList();
            ClearSession();
            try
            {
                bool ifok = true;
                HttpContext.Current.Session.Remove("invalidreturnflight");
                if (objBooking.GetTotalSuspend((MyUserSet.AgentID)) >= Convert.ToInt16(MyUserSet.MaxSuspend))
                {
                    SelectFlightPanel.JSProperties["cp_result"] = msgList.Err100012;
                }
                else
                {

                    if (ddlDeparture.SelectedIndex == 0 || hfArrival.Value == "")
                    {
                        ifok = false;
                        SelectFlightPanel.JSProperties["cp_result"] = msgList.Err100017;
                    }
                    DateTime DepartDate = new DateTime();
                    try
                    {
                        //DepartDate = Convert.ToDateTime(ddl_DepartYear.SelectedItem.Text + "-" + ddl_DepartMonth.SelectedValue + "-" + ddl_DepartDay.SelectedItem.Text.PadLeft(2, '0'));
                        //DepartDate = Convert.ToDateTime(Request.Form["ddlMarketMonth1"].ToString() + "-" + Request.Form["ddlMarketDay1"].ToString().PadLeft(2, '0'));
                        if (hfdaStart.Value != "")
                        {
                            DepartDate = Convert.ToDateTime(hfdaStart.Value);
                            daStart.Date = DepartDate;
                        }
                        else
                        {
                            DepartDate = Convert.ToDateTime(daStart.Value);
                        }

                    }
                    catch
                    {
                        ifok = false;
                        SelectFlightPanel.JSProperties["cp_result"] = msgList.Err100002;

                    }
                    DateTime ReturnDate = new DateTime();
                    if (DepartDate.AddDays(2) < DateTime.Now && ifok == true)
                    {
                        ifok = false;
                        SelectFlightPanel.JSProperties["cp_result"] = msgList.Err100002;

                    }

                    Boolean returnFlight = false;
                    returnFlight = objBooking.IsReturn(TransID, 0);
                    if (returnFlight == true)
                    {
                        try
                        {
                            //ReturnDate = Convert.ToDateTime(Request.Form["ddlMarketMonth2"].ToString() + "-" + Request.Form["ddlMarketDay2"].ToString().PadLeft(2, '0'));
                            if (hfdaEnd.Value != "")
                            {
                                ReturnDate = Convert.ToDateTime(hfdaEnd.Value);
                                daEnd.Date = ReturnDate;
                            }
                            else
                            {
                                ReturnDate = Convert.ToDateTime(daEnd.Value);
                            }

                            ////added by ketee
                            //added by ketee, block travel period from : 1 March - 31 Oct 2015(425 days)
                            //added by ketee, block travel period from : 10 Jun 2015 - 17 JAN 2016(425 days)
                            //if (ReturnDate >= Convert.ToDateTime("2015-06-10") && ReturnDate <= Convert.ToDateTime("2016-01-17"))
                            //{
                            //    ifok = false;
                            //    e.Result = msgList.Err100050;
                            //}
                            //added by Tyas 13/08/2015
                            Restriction();

                            if (DateTime.Now >= Convert.ToDateTime(bookingFrom) && DateTime.Now <= Convert.ToDateTime(bookingTo) && Active == true)
                            {
                                if (ReturnDate >= Convert.ToDateTime(travelFrom) && ReturnDate <= Convert.ToDateTime(travelTo))
                                {
                                    newSys_Preft = objGeneral.GetSingleSYS_PREFT(1, "AA", "RESTRICTIONALERT");
                                    ifok = false;
                                    SelectFlightPanel.JSProperties["cp_result"] = newSys_Preft.SYSValue;
                                }
                            }
                            //remark by Tyas 19/06/2015
                            //if (System.Configuration.ConfigurationManager.AppSettings["RestrictionActive"] != null && System.Configuration.ConfigurationManager.AppSettings["RestrictionActive"].ToString() == "1")
                            //{
                            //    DateTime bookingFrom = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["BookingFrom"].ToString());
                            //    DateTime bookingTo = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["BookingTo"].ToString());
                            //    DateTime travelFrom = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["TravelFrom"].ToString());
                            //    DateTime travelTo = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["TravelTo"].ToString());
                            //    string restrictionMsg = System.Configuration.ConfigurationManager.AppSettings["RestrictionMsg"].ToString();


                            //}

                            ////ReturnDate = Convert.ToDateTime(ddl_ReturnYear.SelectedItem.Text + "-" + ddl_ReturnMonth.SelectedValue + "-" + ddl_ReturnDay.SelectedItem.Text.PadLeft(2, '0'));
                            //if (DepartDate > ReturnDate)
                            //{
                            //    ifok = false;
                            //    e.Result = msgList.Err100003;

                            //}
                        }
                        catch
                        {
                            ifok = false;
                            SelectFlightPanel.JSProperties["cp_result"] = msgList.Err100004;
                        }
                    }
                    else
                    {
                        try
                        {
                            Restriction();

                            if (DateTime.Now >= Convert.ToDateTime(bookingFrom) && DateTime.Now <= Convert.ToDateTime(bookingTo) && Active == true)
                            {
                                if (DepartDate >= Convert.ToDateTime(travelFrom) && DepartDate <= Convert.ToDateTime(travelTo))
                                {
                                    newSys_Preft = objGeneral.GetSingleSYS_PREFT(1, "AA", "RESTRICTIONALERT");
                                    ifok = false;
                                    SelectFlightPanel.JSProperties["cp_result"] = newSys_Preft.SYSValue;
                                }
                            }

                        }
                        catch
                        {
                            ifok = false;
                            SelectFlightPanel.JSProperties["cp_result"] = msgList.Err100004;
                        }
                    }

                    int num = 0;

                    if (txt_GeustNum.Text == "")
                    {
                        ifok = false;
                        SelectFlightPanel.JSProperties["cp_result"] = msgList.Err100005;
                    }

                    //if (IsNumeric(txt_GeustNum.Text) == false || (txt_ChildNum.Text != "" && IsNumeric(txt_ChildNum.Text) == false))
                    //{
                    //    ifok = false;
                    //    SelectFlightPanel.JSProperties["cp_result"] = msgList.Err100006;
                    //}

                    //added by ketee, block travel period from : 5 Jan - 31 July 2015(425 days)
                    //added by ketee, block travel period from : 1 March - 31 Oct 2015(425 days)
                    //added by ketee, block travel period from : 10 Jun 2015 - 17 JAN 2016(425 days)
                    //added by Tyas 22/06/2015
                    //lstOpt = objGeneral.GetAllCODEMASTERFilterCode("RST");
                    //for (int i = 0; i < lstOpt.Count; i++)
                    //{
                    //    CODEMASTER code = new CODEMASTER();
                    //    code = lstOpt[i];
                    //    if (code.Code == "BOOKFROM")
                    //    {
                    //       bookingFrom = DateTime.ParseExact(code.CodeDesc, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    //        //Session["bookingFrom"] = bookingFrom;
                    //    }
                    //    else if (code.Code == "BOOKTO")
                    //    {
                    //        bookingTo = DateTime.ParseExact(code.CodeDesc, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    //        //Session["bookingTo"] = bookingTo;
                    //    }
                    //    else if (code.Code == "TRAFROM")
                    //    {
                    //        travelFrom = DateTime.ParseExact(code.CodeDesc, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    //        //Session["travelFrom"] = travelFrom;
                    //    }
                    //    else if (code.Code == "TRATO")
                    //    {
                    //        travelTo = DateTime.ParseExact(code.CodeDesc, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    //        //Session["travelTo"] = travelTo;
                    //    }
                    //    else if (code.Code == "IND")
                    //    {
                    //        if (code.CodeDesc == "1")
                    //        {
                    //            Active = true;
                    //            //Session["Active"] = Active;
                    //        }
                    //        else
                    //        {
                    //            Active = false;
                    //        }
                    //    }

                    //}
                    //added by Tyas 13/08/2015
                    Restriction();

                    if (DateTime.Now >= Convert.ToDateTime(bookingFrom) && DateTime.Now <= Convert.ToDateTime(bookingTo) && Active == true)
                    {
                        if (ReturnDate >= Convert.ToDateTime(travelFrom) && ReturnDate <= Convert.ToDateTime(travelTo))
                        {
                            newSys_Preft = objGeneral.GetSingleSYS_PREFT(1, "AA", "RESTRICTIONALERT");
                            ifok = false;
                            SelectFlightPanel.JSProperties["cp_result"] = newSys_Preft.SYSValue;
                        }
                    }
                    //remark by Tyas 19/06/2015
                    //if ( System.Configuration.ConfigurationManager.AppSettings["RestrictionActive"] != null && System.Configuration.ConfigurationManager.AppSettings["RestrictionActive"].ToString() == "1")
                    //{
                    //    DateTime bookingFrom = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["BookingFrom"].ToString());
                    //    DateTime bookingTo = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["BookingTo"].ToString());
                    //    DateTime travelFrom = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["TravelFrom"].ToString());
                    //    DateTime travelTo = Convert.ToDateTime( System.Configuration.ConfigurationManager.AppSettings["TravelTo"].ToString());
                    //    string restrictionMsg = System.Configuration.ConfigurationManager.AppSettings["RestrictionMsg"].ToString();

                    //    if (DateTime.Now >= bookingFrom && DateTime.Now <= bookingTo)
                    //    {
                    //        if (DepartDate >= travelFrom && DepartDate <= travelTo)
                    //        {
                    //            ifok = false;
                    //            e.Result = restrictionMsg;
                    //        }
                    //    }
                    //}
                    //if (DepartDate >= Convert.ToDateTime("2015-06-10") && DepartDate <= Convert.ToDateTime("2016-01-17"))
                    //{
                    //    ifok = false;
                    //    e.Result = msgList.Err100050;
                    //}

                    DataTable dtRoute = new DataTable();
                    dtRoute = objBooking.GetAllSECTORSUSPEND(MyUserSet.OperationGroup, MyUserSet.AgentCategoryID, ddlDeparture.SelectedItem.Value.ToString(), hfArrival.Value.ToString(), DepartDate.Date);
                    if (dtRoute != null)
                    {
                        ifok = false;
                        SelectFlightPanel.JSProperties["cp_result"] = msgList.Err100038;
                    }
                    dtRoute = null;


                    int ChildNum = 0;

                    if (txt_ChildNum.Text != "")
                    {
                        ChildNum = Convert.ToInt32(txt_ChildNum.Text);
                    }
                    if (ifok)
                    {
                        if (Convert.ToInt32(txt_GeustNum.Text) == 0)
                        {
                            ifok = false;
                            SelectFlightPanel.JSProperties["cp_result"] = msgList.Err100027;
                        }

                        if (hfCurrency.Value == "")
                        {
                            ifok = false;
                            SelectFlightPanel.JSProperties["cp_result"] = msgList.Err100018;
                            return;
                        }
                        else
                        {
                            num = Convert.ToInt32(txt_GeustNum.Text) + ChildNum;
                        }
                        //if (num < 10 || num > 50)
                        //{
                        //    ifok = false;
                        //    SelectFlightPanel.JSProperties["cp_result"] = msgList.Err100006;
                        //}
                        if (Convert.ToInt32(txt_GeustNum.Text) > 50)
                        {
                            ifok = false;
                            SelectFlightPanel.JSProperties["cp_result"] = msgList.Err100007;
                        }
                        else if (ChildNum > 50)
                        {
                            ifok = false;
                            SelectFlightPanel.JSProperties["cp_result"] = msgList.Err100008;
                        }

                    }

                    //Added by ketee, to cater infant number 20170121
                    int infantNum = 0;
                    if (txt_InfantNum.Text != "")
                    {
                        infantNum = Convert.ToInt32(txt_InfantNum.Text);
                    }

                    if (ifok)
                    {
                        if (txt_InfantNum.Text != "" && infantNum > 10)
                        {
                            ifok = false;
                            SelectFlightPanel.JSProperties["cp_result"] = msgList.Err100051;
                        }
                    }

                    if (ifok)
                    {
                        Session["TotalPax"] = num;
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

                        HttpCookie cookie = new HttpCookie("cookieSearchcondition");
                        cookie.HttpOnly = true;
                        //cookie.Expires = DateTime.Today.AddMinutes(10);
                        //cookie.Values.Add("Carrier", ddl_Carrier.SelectedItem.Text);                       

                        cookie.Values.Add("Departure", ddlDeparture.SelectedValue);
                        cookie.Values.Add("Arrival", hfArrival.Value);
                        if (returnFlight == false)
                        {
                            cookie.Values.Add("ifOneWay", "TRUE");
                        }
                        else
                        {
                            cookie.Values.Add("ifOneWay", "FALSE");
                        }

                        cookie.Values.Add("Currency", hfCurrency.Value);
                        cookie.Values.Add("DepartDate", DepartDate.ToString());
                        cookie.Values.Add("ReturnDate", ReturnDate.ToString());
                        cookie.Values.Add("PaxNum", num.ToString());
                        cookie.Values.Add("GuestNum", txt_GeustNum.Text);
                        cookie.Values.Add("ChildNum", ChildNum.ToString());
                        cookie.Values.Add("InfantNum", infantNum.ToString());
                        cookie.Values.Add("DepartureDetail", ddlDeparture.SelectedItem.Text);
                        cookie.Values.Add("ArrivalDetail", hfArrivalText.Value);
                        Response.AppendCookie(cookie);

                        //check available flight

                        ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition model = new ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition();

                        model.Carrier = cookie.Values["Carrier"];//   ViewState["Carrier"].ToString();
                        model.Departure = cookie.Values["Departure"];// ViewState["Departure"].ToString();
                        model.Arrival = cookie.Values["Arrival"];// ViewState["Arrival"].ToString();
                        bool oneway = false;
                        if (cookie.Values["ifOneWay"] == "TRUE")
                        {
                            oneway = true;
                        }
                        else
                        {
                            model.ReturnDate = Convert.ToDateTime(cookie.Values["ReturnDate"]);
                        }
                        model.ifOneWay = oneway;//  Convert.ToBoolean(ViewState["ifOneWay"]);
                        model.Currency = cookie.Values["Currency"];// ViewState["Currency"].ToString();
                        model.DepartDate = Convert.ToDateTime(cookie.Values["DepartDate"]);// Convert.ToDateTime(ViewState["DepartDate"]);
                        model.ReturnDate = Convert.ToDateTime(cookie.Values["ReturnDate"]);//  Convert.ToDateTime(ViewState["ReturnDate"]);
                        model.GuestNum = Convert.ToInt32(cookie.Values["GuestNum"]);//  Convert.ToInt32(ViewState["GuestNum"]);
                        model.PaxNum = Convert.ToInt32(cookie.Values["PaxNum"]);
                        model.ChildNum = Convert.ToInt32(cookie.Values["ChildNum"]);
                        model.InfantNum = Convert.ToInt32(cookie.Values["InfantNum"]);

                        //insert temFlight into session

                        objBooking.tempFlight(model, MyUserSet.AgentName, "");

                        DataList dtModel1 = new DataList();
                        DataList dtModel2 = new DataList();

                        DataTable tempDt = new DataTable();
                        tempDt = objBooking.dtFlight();

                        //added by ketee, validate valid flight
                        if (HttpContext.Current.Session["invalidreturnflight"] != null && HttpContext.Current.Session["invalidreturnflight"].ToString() == "1")
                        {
                            ifok = false;
                            SelectFlightPanel.JSProperties["cp_result"] = msgList.Err100041;
                        }

                        if (ifok)
                        {
                            if (oneway == false)
                            {
                                if (HttpContext.Current.Session["tempFlight"] != null)
                                {
                                    //
                                    string strExpr;
                                    string strSort;
                                    DataTable dt = new DataTable();

                                    strExpr = "TemFlightDeparture = '" + model.Departure + "' AND TemFlightArrival = '" + model.Arrival + "' AND TemFlightAgentName = '" + MyUserSet.LoginName + "' AND TemFlightIfReturn = false";

                                    strSort = "";
                                    // Use the Select method to find all rows matching the filter.

                                    dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                                    DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                                    tempDt.Clear();

                                    foreach (DataRow row in foundRows)
                                    {
                                        tempDt.ImportRow(row);
                                    }

                                    dtModel1.DataSource = tempDt;
                                    dtModel1.DataBind();

                                    strExpr = "TemFlightDeparture = '" + model.Arrival + "' AND TemFlightArrival = '" + model.Departure + "' AND TemFlightAgentName = '" + MyUserSet.LoginName + "' AND TemFlightIfReturn = true";

                                    strSort = "";

                                    foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                                    tempDt.Clear();
                                    foreach (DataRow row in foundRows)
                                    {
                                        tempDt.ImportRow(row);
                                    }

                                    dtModel2.DataSource = tempDt;
                                    dtModel2.DataBind();
                                    if (dtModel1.Items.Count <= 0 || dtModel2.Items.Count <= 0)
                                    {
                                        ifok = false;
                                        SelectFlightPanel.JSProperties["cp_result"] = msgList.Err100013;
                                    }
                                }
                                else
                                {
                                    ifok = false;
                                    SelectFlightPanel.JSProperties["cp_result"] = msgList.Err100013;
                                }  //end

                            }
                            else
                            {

                                //johan remark
                                if (HttpContext.Current.Session["tempFlight"] != null)
                                {
                                    string strExpr;
                                    string strSort;
                                    DataTable dt = new DataTable();

                                    strExpr = "TemFlightDeparture = '" + model.Departure + "' AND TemFlightArrival = '" + model.Arrival + "' AND TemFlightAgentName = '" + MyUserSet.LoginName + "' AND TemFlightIfReturn = false";

                                    strSort = "";
                                    // Use the Select method to find all rows matching the filter.

                                    dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                                    DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                                    tempDt.Clear();

                                    foreach (DataRow row in foundRows)
                                    {
                                        tempDt.ImportRow(row);
                                    }
                                    //

                                    //dtModel1.DataSource = (DataTable)HttpContext.Current.Session["tempFlightDepart"];
                                    dtModel1.DataSource = tempDt;
                                    dtModel1.DataBind();

                                    if (dtModel1.Items.Count <= 0)
                                    {
                                        ifok = false;
                                        SelectFlightPanel.JSProperties["cp_result"] = msgList.Err100013;
                                    }
                                }
                                else
                                {
                                    ifok = false;
                                    SelectFlightPanel.JSProperties["cp_result"] = msgList.Err100013;
                                }

                            }
                        }

                        if (ifok)
                        {
                            //CheckSuspend
                            if (!CheckSuspend())
                            {
                                //AddEnquiry
                                if (AddEnquiry() == 1)
                                {
                                    //blacklist

                                    SelectFlightPanel.JSProperties["cp_result"] = msgList.Err100012;

                                }
                                else
                                {
                                    RefreshPaymentSession();
                                    SelectFlightPanel.JSProperties["cp_result"] = "";
                                }
                                //Response.Redirect("selectflight.aspx");
                            }
                            else
                            {
                                SelectFlightPanel.JSProperties["cp_result"] = msgList.Err100010;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                SelectFlightPanel.JSProperties["cp_result"] = msgList.Err100031;
                log.Error(this, ex);
                //Response.Redirect(Shared.MySite.PublicPages.AgentMain, false);
            }
        }

        public static bool IsNumeric(string s)
        {
            double result;
            return s != null && Double.TryParse(s, out result);
        }

        protected bool CheckSuspend()
        {
            if (objBooking.CheckStillSuspend(MyUserSet.AgentID, ddlDeparture.SelectedValue, hfArrival.Value))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void RefreshPaymentSession()
        {
            HttpContext.Current.Session.Remove("ErrorPayment");
        }

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

        protected int AddEnquiry()
        {
            int blacklist = 0;
            string enID = objBooking.CheckEnqExist(MyUserSet.AgentID, ddlDeparture.SelectedValue, hfArrival.Value);
            if (enID != "")
            {
                //counter enquiry
                enqLogInfo = objBooking.GetSingleEN_ENQUIRYLOG(enID, MyUserSet.AgentID, DateTime.Now);

                enqLogInfo.EnquiryDate = DateTime.Now;
                enqLogInfo.LastEnquiryDate = DateTime.Now;
                enqLogInfo.NoOfAttempt = enqLogInfo.NoOfAttempt + 1;
                enqLogInfo.SyncLastUpd = DateTime.Now;

                objBooking.SaveEN_ENQUIRYLOG(enqLogInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Update);
            }
            else
            {
                //insert new
                enID = DateTime.Now.ToString("yyyyMMddHHmmsss");

                enqLogInfo.AgentID = MyUserSet.AgentID;
                enqLogInfo.EnquiryID = enID;
                enqLogInfo.EnquiryDate = DateTime.Now;
                enqLogInfo.LastEnquiryDate = DateTime.Now;
                enqLogInfo.Origins = ddlDeparture.SelectedValue;
                enqLogInfo.Destination = hfArrival.Value;
                enqLogInfo.NoOfAttempt = 1;
                enqLogInfo.SyncCreate = DateTime.Now;
                enqLogInfo.SyncLastUpd = DateTime.Now;
                enqLogInfo.CreateBy = MyUserSet.AgentID;

                objBooking.SaveEN_ENQUIRYLOG(enqLogInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
            }
            // check max enquiry
            if (enqLogInfo.NoOfAttempt >= Convert.ToInt32(MyUserSet.MaxEnquiry))
            {
                //check suspend exist first  

                string susID = objBooking.CheckSuspExist(MyUserSet.AgentID, ddlDeparture.SelectedValue, hfArrival.Value);
                if (susID != "")
                {
                    enqSusInfo = objBooking.GetSingleEN_SUSPENDLIST(susID, MyUserSet.AgentID, ddlDeparture.SelectedValue, hfArrival.Value);
                    enqSusInfo.SuspendAttempt = enqSusInfo.SuspendAttempt + 1;
                    enqSusInfo.SuspendDate = DateTime.Now;
                    enqSusInfo.SuspendExpiry = DateTime.Now.AddHours(Convert.ToInt16(MyUserSet.SuspendDuration));
                    enqSusInfo.SyncLastUpd = DateTime.Now;
                    objBooking.SaveEN_SUSPENDLIST(enqSusInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
                }
                else
                {
                    //insert new suspend
                    susID = DateTime.Now.ToString("yyyyMMddHHmmsss");

                    enqSusInfo.SuspendID = susID;
                    enqSusInfo.AgentID = MyUserSet.AgentID;
                    enqSusInfo.Origins = ddlDeparture.SelectedValue;
                    enqSusInfo.Destination = hfArrival.Value;
                    enqSusInfo.LastEnquiryID = enID;
                    enqSusInfo.SuspendAttempt = 1;
                    enqSusInfo.CreateBy = MyUserSet.AgentID;
                    enqSusInfo.SuspendDate = DateTime.Now;
                    enqSusInfo.SuspendExpiry = DateTime.Now.AddHours(Convert.ToInt16(MyUserSet.SuspendDuration));
                    enqSusInfo.SyncCreate = DateTime.Now;
                    enqSusInfo.SyncLastUpd = DateTime.Now;
                    objBooking.SaveEN_SUSPENDLIST(enqSusInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
                }
                //roll back enquiry

                objBooking.RollBackEnquiry(enqLogInfo.EnquiryID);

                //update activity suspend
                agActivityInfo.AgentID = MyUserSet.AgentID;
                agActivityInfo.LastSuspend = enqSusInfo.SuspendDate;
                objAgent.SaveAG_ACTIVITY(agActivityInfo, ABS.Logic.GroupBooking.Agent.AgentControl.EnumSaveType.Update);


                //check max suspend

                if (objBooking.GetTotalSuspend((MyUserSet.AgentID)) >= Convert.ToInt16(MyUserSet.MaxSuspend))
                {
                    //blacklist agent
                    string blacklistID = "";
                    blacklistID = objAgent.CheckBlacklistExist(MyUserSet.AgentID);
                    if (blacklistID != "")
                    {
                        //blacklist exist
                        agBlacklistInfo = objAgent.GetSingleAG_BLACKLIST(MyUserSet.AgentID);
                        agBlacklistInfo.BlacklistDate = DateTime.Now;
                        agBlacklistInfo.BlacklistExpiryDate = DateTime.Now.AddDays(Convert.ToInt16(MyUserSet.BlacklistDuration));
                        agBlacklistInfo.SyncLastUpd = DateTime.Now;
                        agBlacklistInfo.LastSyncBy = MyUserSet.AgentID;
                        agBlacklistInfo.Remark = "Suspend reach limit.";

                        objAgent.SaveAG_BLACKLIST(agBlacklistInfo, ABS.Logic.GroupBooking.Agent.AgentControl.EnumSaveType.Update);
                    }
                    else
                    {
                        //insert new blacklist
                        blacklistID = DateTime.Now.ToString("yyyyMMddHHmmsss");
                        agBlacklistInfo.AgentID = MyUserSet.AgentID;
                        agBlacklistInfo.BlacklistDate = DateTime.Now;
                        agBlacklistInfo.BlacklistExpiryDate = DateTime.Now.AddDays(Convert.ToInt16(MyUserSet.BlacklistDuration));
                        agBlacklistInfo.BlacklistID = blacklistID;
                        agBlacklistInfo.LastSyncBy = MyUserSet.AgentID;
                        agBlacklistInfo.Status = 1;
                        agBlacklistInfo.SyncCreate = DateTime.Now;
                        agBlacklistInfo.SyncLastUpd = DateTime.Now;
                        agBlacklistInfo.Remark = "Suspend reach limit.";

                        objAgent.SaveAG_BLACKLIST(agBlacklistInfo, ABS.Logic.GroupBooking.Agent.AgentControl.EnumSaveType.Insert);
                    }

                    //update blacklist activity
                    agActivityInfo.AgentID = MyUserSet.AgentID;
                    agActivityInfo.LastBlacklist = agBlacklistInfo.BlacklistDate;
                    agActivityInfo.ExpiryBlacklistDate = agBlacklistInfo.BlacklistExpiryDate;
                    objAgent.SaveAG_ACTIVITY(agActivityInfo, ABS.Logic.GroupBooking.Agent.AgentControl.EnumSaveType.Update);

                    blacklist = 1;
                }
            }
            return blacklist;
        }

        protected void FillModelFromDataRows(DataRow[] foundRows, ref ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model)
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
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
        }

        protected bool SaveData()
        {

            ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();

            string strExpr;
            string strSort;
            int TotalInfantpax = 0;
            HttpCookie cookie2 = Request.Cookies["cookieSearchcondition"];
            if (cookie2 != null)
            {
                TotalInfantpax = Convert.ToInt32(cookie2.Values["InfantNum"]);
            }
            DataTable dt = new DataTable();
            Hashtable ht = new Hashtable();

            //payment control
            PaymentControl objPayment = new PaymentControl();

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

            strExpr = "TemFlightId = '" + departID + "'";
            strSort = "";
            DateTime departDate;
            dt = (DataTable)HttpContext.Current.Session["TempFlight"];
            DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
            FillModelFromDataRows(foundRows, ref  temFlight);

            departDate = Convert.ToDateTime(temFlight.TemFlightStd);

            Currency = temFlight.TemFlightCurrencyCode.Trim();

            agent = objAgentProfile.GetSingleAgentProfile(MyUserSet.AgentName);

            string LoginType = MyUserSet.AgentType.ToString();
            int count = 0;
            DataTable dtClass = objBooking.dtClass();
            //if (HttpContext.Current.Session["dataClass"] != null) { dtClass = (DataTable)HttpContext.Current.Session["dataClass"]; }
            count = dtClass.Rows.Count;
            List<BookingTransactionMain> TransMain = new List<BookingTransactionMain>();

            tranID = DateTime.Now.ToString("yyyyMMddHHmmsss");
            Session["TransID"] = tranID;
            

            #region newsavedetail
            objBooking.FillChgdataTrans(TransID, tranID, Session["PNR"].ToString(), Currency, TotalInfantpax, MyUserSet.AgentID, MyUserSet.AgentName, MyUserSet.AgentCategoryID, Convert.ToDateTime(temFlight.TemFlightStd), ReturnID, true);
            //List<PassengerData> listPassenger = new List<PassengerData>();
            //listPassenger = objBooking.GetAllBK_PASSENGERLIST(TransID);

            //HttpContext.Current.Session.Remove("ChglistPassenger");
            //HttpContext.Current.Session.Add("ChglistPassenger", listPassenger);

            //BookingTransTender TransTender = new BookingTransTender();
            //TransTender = objBooking.GetSingleBK_TRANSTENDER(TransID, 1);

            //HttpContext.Current.Session.Remove("ChgTransTender");
            //HttpContext.Current.Session.Add("ChgTransTender", TransTender);

            #endregion
            return true;

            ////added by ketee
            //BookingTransactionMain BookingMain = new BookingTransactionMain();
            ////BookingMain = objBooking.SaveBooking(bookHDRInfo, lstbookDTLInfo, transTaxInfo, lsttransFeesInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);

            //if (bookHDRInfo != null) //amended by diana 20140124 - check for equal total pax
            //{
            //    if (bookHDRInfo.TransTotalPAX < int.Parse(HttpContext.Current.Session["TotalPax"].ToString()))
            //    {
            //        Session["PaxStatus"] = "false";
            //        return false;
            //    }
            //    else
            //    {
            //        BookingMain = objBooking.SaveBooking(bookHDRInfo, lstbookDTLInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
            //        if (BookingMain != null && BookingMain.TransID != "")
            //        {
            //            objBooking.UpdatePassengerTransID(TransID, PNR, tranID);
            //            objBooking.UpdateTransTenderTransID(TransID, PNR, tranID);
            //            objBooking.DeletePrevTransaction(TransID, PNR);
            //            Session["TransID"] = BookingMain.TransID;
            //            return true;
            //        }
            //        else
            //            return false;
            //    }
            //}
            //return false;
            //objBooking.SaveBooking(bookHDRInfo, lstbookDTLInfo, transTaxInfo, lsttransFeesInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
        }

        protected bool UpdateData()
        {

            ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();

            string strExpr;
            string strSort;
            int TotalInfantpax = 0;
            HttpCookie cookie2 = Request.Cookies["cookieSearchcondition"];
            if (cookie2 != null)
            {
                TotalInfantpax = Convert.ToInt32(cookie2.Values["InfantNum"]);
            }
            DataTable dt = new DataTable();
            Hashtable ht = new Hashtable();
            //added by ketee 20130625

            //payment control
            PaymentControl objPayment = new PaymentControl();

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

            strExpr = "TemFlightId = '" + departID + "'";
            strSort = "";
            DateTime departDate;
            dt = (DataTable)HttpContext.Current.Session["TempFlight"];
            DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
            FillModelFromDataRows(foundRows, ref  temFlight);

            departDate = Convert.ToDateTime(temFlight.TemFlightStd);

            Currency = temFlight.TemFlightCurrencyCode.Trim();

            agent = objAgentProfile.GetSingleAgentProfile(MyUserSet.AgentName);

            string LoginType = MyUserSet.AgentType.ToString();

            int count = 0;
            DataTable dtClass = objBooking.dtClass();
            if (HttpContext.Current.Session["dataClass"] != null) { dtClass = (DataTable)HttpContext.Current.Session["dataClass"]; }
            count = dtClass.Rows.Count;

            List<BookingTransactionMain> TransMain = new List<BookingTransactionMain>();

            tranID = Request.QueryString["TransID"];

            #region newsavedetail
            objBooking.FillChgdataTrans(TransID, tranID, Session["PNR"].ToString(), Currency, TotalInfantpax, MyUserSet.AgentID, MyUserSet.AgentName, MyUserSet.AgentCategoryID, Convert.ToDateTime(temFlight.TemFlightStd), ReturnID, false);
            Session["TransID"] = tranID;
            return true;
            #endregion
            //objBooking.SaveBooking(bookHDRInfo, lstbookDTLInfo, transTaxInfo, lsttransFeesInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
        }
    }


}