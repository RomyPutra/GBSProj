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
using ABS.GBS.Log;
using StackExchange.Profiling;

namespace GroupBooking.Web.Booking
{
    public partial class SelectAllFlight : System.Web.UI.Page
    {
        #region declaration
        UserSet MyUserSet;
        MessageList msgList = new MessageList();
        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
        ABS.Logic.GroupBooking.Agent.AgentControl objAgent = new ABS.Logic.GroupBooking.Agent.AgentControl();
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
        //end edded

        DataTable dtSysPreft, dtSysPrefttry;
        string organizationID = "";
        string custommessage = "";//added by romy for custom message
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;

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
                else
                {
                    DevExpress.Web.ASPxWebControl.RedirectOnCallback(Shared.MySite.PublicPages.InvalidPage);
                }
            }

            if (!IsPostBack && !IsCallback)
            {
                if (Request.QueryString["callback"] == null)
                {
                    using (profiler.Step("GBS:ClearSession"))
                    {
                        ClearSession();
                    }
                    if (MyUserSet != null)
                    {
                        if (Session["soldout"] != null)
                        {
                            Hidden1.Value = msgList.Err100059;
                            Session["soldout"] = null;
                            HttpContext.Current.Session["errormsg"] = null;
                        }
                        if (Session["overlap"] != null)
                        {
                            Hidden1.Value = msgList.Err100061;
                            Session["overlap"] = null;
                            HttpContext.Current.Session["errormsg"] = null;
                        }
                        if (Session["error"] != null)
                        {
                            Hidden1.Value = Session["error"].ToString();
                            Session["error"] = null;
                            HttpContext.Current.Session["errormsg"] = null;
                        }
                        using (profiler.Step("API:ClearExpiredJourney"))
                        {
                            objBooking.ClearExpiredJourney(MyUserSet.AgentID);
                        }
                        //using (profiler.Step("Cancel10MntBook"))
                        //{
                        //    Cancel10MntBook();
                        //}
                    }
                    else
                    {
                        Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.SessionExpired);
                    }
                }
                if (Request.QueryString["mod"] != null && Request.QueryString["add"] != null && Request.QueryString["mod"].ToString() != "" && Request.QueryString["add"].ToString() != "")
                {
                    using (profiler.Step("GBS:SearchDate"))
                    {
                        SearchDate(Convert.ToInt16(Request.QueryString["mod"]), Convert.ToInt32(Request.QueryString["add"]));
                    }
                }
                else
                {
                    using (profiler.Step("GBS:InitializeForm"))
                    {
                        InitializeForm();
                    }
                }
                using (profiler.Step("GBS:initializeData"))
                {
                    initializeData();
                }

                ddlDeparture.Attributes.Add("onchange", "$.getScript('../Scripts/chosen.jquery.js', function () { SearchDatePanel.PerformCallback('loadReturnDate') });");
                ddlReturn.Attributes.Add("onchange", "onChangeReturnSta();");
            }
        }


        private void Cancel10MntBook()
        {
            //return;
            List<BookingTransactionMain> transmainInfo;
            BookingControl objBookingControl = new BookingControl();
            transmainInfo = objBookingControl.GetBK_TRANSMAIN10(0, Convert.ToInt32(MyUserSet.AgentID));
            ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");
            if (transmainInfo != null && transmainInfo.Count > 0)
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

        protected void daStart_Init(object sender, EventArgs e)
        {
            ASPxDateEdit dateEdit = sender as ASPxDateEdit;
            dateEdit.MinDate = DateTime.Now.AddDays(-1);
        }

        protected void InitializeForm()
        {
            var profiler = MiniProfiler.Current;
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
                model.ReturnDate = Convert.ToDateTime(cookie.Values["ReturnDate"]);//  Convert.ToDateTime(ViewState["ReturnDate"]);
                model.GuestNum = Convert.ToInt32(cookie.Values["GuestNum"]);//  Convert.ToInt32(ViewState["GuestNum"]);
                model.PaxNum = Convert.ToInt32(cookie.Values["PaxNum"]);
                model.ChildNum = Convert.ToInt32(cookie.Values["ChildNum"]);
                //model.InfantNum = Convert.ToInt32(cookie.Values["InfantNum"]);
                model.PromoCode = cookie.Values["PromoCode"];

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
                    using (profiler.Step("GBS:GetLookUpCity"))
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
                }

                lbl_Go2.Text = temp;
                //lbl_Go2.Text = cookie.Values["ArrivalDetail"];

                lbl_Return1.Text = lbl_Go2.Text;
                //lbl_Return1.Text = cookie.Values["ArrivalDetail"];
                lbl_Return2.Text = lbl_Go1.Text;
                //lbl_Return2.Text = cookie.Values["DepartureDetail"];                

                DataTable tempDt = objBooking.dtFlight();

                using (profiler.Step("GBS:BindList1"))
                {
                    BindList1(model, tempDt);
                }
                if (oneway == false)
                {
                    using (profiler.Step("GBS:BindList2"))
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
            var profiler = MiniProfiler.Current;
            //newSys_Preft = objGeneral.GetSingleSYS_PREFT(1, "AA", "RESTRICTIONNOTE");
            //lblRestriction.Text = newSys_Preft.SYSValue
            //added by romy for GB4,GB2
            using (profiler.Step("GBS:GetSysPreftByPromoCodeALLOWPAXSETTING"))
            {
                if (Session["OrganizationCode"] != null)
                    dtSysPrefttry = objGeneral.GetPaxSetting(Session["OrganizationCode"].ToString());
                if (dtSysPrefttry != null && dtSysPrefttry.Rows.Count > 0)
                {
                    Session["dtSysPrefttry"] = dtSysPrefttry;
                }
                else
                {

                    Session["dtSysPrefttry"] = null;
                }

                if (dtSysPrefttry != null && MyUserSet.AgentID == "17573449")
                {
                    Session["dtSysPrefttry"] = dtSysPrefttry;
                }
            }
            using (profiler.Step("GBS:GetSysPreftByPromoCodePROMOCODE"))
            {
                dtSysPreft = objBooking.GetSysPreftByPromoCode("PROMOCODE");
                if (dtSysPreft != null && dtSysPreft.Rows.Count > 0)
                    Session["dtSysPreft"] = dtSysPreft;
            }
            txt_PromoCode.Text = "";

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

            //daStart.MinDate = dateNow;

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

            tdReturn.Attributes["display"] = "";
            if (cb_OneWay.Checked == true || hOneWay.Value == "1")
            {
                tdReturn.Attributes["display"] = "none";
            }
            #endregion

            //UIClass.SetComboCustomStyle(ref ddlDeparture, UIClass.EnumDefineStyle.City, string.Empty, string.Empty, false);
            //UIClass.SetComboCustomStyle(ref ddlReturn, UIClass.EnumDefineStyle.City, string.Empty, ddlDeparture.SelectedItem.Value, false);

            DataTable dt = new DataTable();
            using (profiler.Step("GBS:GetLookUpCity"))
            {
                dt = objGeneral.GetLookUpCity("", Request.PhysicalApplicationPath);
                if (dt == null || dt.Rows.Count <= 0)
                {
                    dt = objGeneral.ReturnAllCityCustom("");
                }
                //SEAL.WEB.UI.Control.SetComboCustomStyle(ref ddlDeparture, dt, "CustomState", "CityCode", "Select City");
                SEAL.WEB.UI.Control.SetComboCustomStyle(ref ddlDeparture, dt, "CustomState", "DepartureStation", "Select City");

            }
            ddlDeparture.SelectedValue = model.Departure;
            hfArrival.Value = model.Arrival;
            bindComboBoxReturn();
            ddlReturn.SelectedValue = model.Arrival;
            if (ddlReturn.SelectedIndex > 0)
            {
                string[] Arrival = ddlReturn.SelectedItem.Text.Split('|');
                if (Arrival.Length > 0) hfArrivalText.Value = Arrival[0].Trim();
            }

            daStart.Value = model.DepartDate;
            daEnd.Value = model.ReturnDate;
            txt_GeustNum.Value = model.GuestNum > 0 ? model.GuestNum.ToString() : "";
            txt_ChildNum.Value = model.ChildNum > 0 ? model.ChildNum.ToString() : "";
            //txt_InfantNum.Value = model.InfantNum > 0 ? model.InfantNum.ToString() : "";
            cb_OneWay.Checked = model.ifOneWay;
            txt_PromoCode.Text = model.PromoCode;
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
                if (cb_OneWay.Checked == true || hOneWay.Value == "1")
                {
                    tdReturn.Attributes["display"] = "none";
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
            var profiler = MiniProfiler.Current;
            //UIClass.SetComboCustomStyle(ref ddlReturn, UIClass.EnumDefineStyle.City, string.Empty, ddlDeparture.SelectedItem.Value, false);
            DataTable dt = new DataTable();
            using (profiler.Step("GBS:GetLookUpCity"))
            {
                dt = objGeneral.GetLookUpCity(ddlDeparture.SelectedItem.Value, Request.PhysicalApplicationPath);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Session["dtCountryList"] = dt;
                }
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

            using (profiler.Step("GBS:BindCurrency"))
            {
                BindCurrency(ddlDeparture.SelectedItem.Value);
            }
            ddlReturn.Focus();
        }

        private void BindCurrency(string Departure)
        {
            var profiler = MiniProfiler.Current;
            try
            {
                DataTable dt = new DataTable();
                using (profiler.Step("GBS:GetLookUpCity"))
                {
                    dt = objGeneral.GetLookUpCity("", Request.PhysicalApplicationPath);
                }
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
                if (StartDate < DateTime.Now.AddHours(4))
                {
                    StartDate = (DateTime)model.DepartDate.AddDays(-Convert.ToInt32(((DateTime)model.DepartDate - DateTime.Now.AddHours(4)).TotalDays));
                }
                SelectedDate = (DateTime)model.DepartDate;
                DepartContainer += "<li>";
                //DepartContainer += "<a href='selectflight.aspx?mod=" + Mode + "&add=" + (-2) + "' id ='A_10' >";
                DepartContainer += "<a href='selectflight.aspx?mod=" + Mode + "&add=" + (-7) + "' id ='A_10' >"; //20170324 - Sienny (back 7 days)
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
                    if (StartDate.Date >= MinDate.Date)
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
                            DepartContainer += "<a href='selectflight.aspx?mod=" + Mode + "&add=" + cntDay + "' class='linkDayFlight'>";
                            DepartContainer += "<div>";
                        }
                        else
                        {
                            DepartContainer += "<li>";
                            DepartContainer += "<a href='selectflight.aspx?mod=" + Mode + "&add=" + cntDay + "' class='linkDayFlight'>";
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
                //DepartContainer += "<a href='selectflight.aspx?mod=" + Mode + "&add=" + cntDay + "' id ='A_63' >";
                DepartContainer += "<a href='selectflight.aspx?mod=" + Mode + "&add=" + 7 + "' id ='A_63' >"; //20170324 - Sienny (forward 7 days)
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
                if (StartDate < DateTime.Now.AddHours(4))
                {
                    StartDate = (DateTime)model.ReturnDate.AddDays(-Convert.ToInt32(((DateTime)model.ReturnDate - DateTime.Now.AddHours(4)).TotalDays));
                }
                SelectedDate = (DateTime)model.ReturnDate;
                ReturnContainer += "<li>";
                //ReturnContainer += "<a href='selectflight.aspx?mod=" + Mode + "&add=" + (-2) + "' id ='A_10' >";
                ReturnContainer += "<a href='selectflight.aspx?mod=" + Mode + "&add=" + (-7) + "' id ='A_10' >"; //20170324 - Sienny (back 7 days)
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
                            ReturnContainer += "<a href='selectflight.aspx?mod=" + Mode + "&add=" + cntDay + "' class='linkDayFlight'>";
                            ReturnContainer += "<div>";
                        }
                        else
                        {
                            ReturnContainer += "<li>";
                            ReturnContainer += "<a href='selectflight.aspx?mod=" + Mode + "&add=" + cntDay + "' class='linkDayFlight'>";
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
                //ReturnContainer += "<a href='javascript:selectflight.aspx?mod=" + Mode + "&add=" + cntDay + "' id ='A_63' >";
                ReturnContainer += "<a href='javascript:selectflight.aspx?mod=" + Mode + "&add=" + 7 + "' id ='A_63' >"; //20170324 - Sienny (forward 7 days)
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
            ClearSession(false);
            MessageList msgList = new MessageList();
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;
            try
            {
                if (e.Parameter != null && e.Parameter != "" && e.Parameter.Trim() == "search")
                {
                    using (profiler.Step("ResearchFlight"))
                    {
                        ResearchFlight(3, 0);
                    }
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

                        using (profiler.Step("FillModelFromDataRow"))
                        {
                            FillModelFromDataRow(foundRows, ref model);
                        }

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

                            using (profiler.Step("FillModelFromDataRow"))
                            {
                                FillModelFromDataRow(foundRows, ref model2);
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

                        if (!oneWay)
                        {
                            if (model.ReturnDate >= model2.DepartDate)
                            {
                                e.Result = msgList.Err100054;
                                return;
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

                            //if (cookie.Values["InfantNum"] != "")
                            //{
                            //    if (Convert.ToInt32(cookie.Values["InfantNum"]) != 0)
                            //    {
                            //        HttpCookie cookie3 = new HttpCookie("AllPax");
                            //        cookie3.Values.Add("PaxNum", cookie.Values["PaxNum"]);
                            //        cookie3.Values.Add("GuestNum", cookie.Values["GuestNum"]);
                            //        cookie3.Values.Add("ChildNum", cookie.Values["ChildNum"]);
                            //        cookie3.Values.Add("InfantNum", cookie.Values["InfantNum"]);
                            //        Response.AppendCookie(cookie3);
                            //    }
                            //}

                            Response.AppendCookie(cookie2);

                            e.Result = "";
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

                model.PromoCode = foundRows[0]["TemFlightPromoCode"].ToString();
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

        private void ClearSession(bool ClearCookie = true)
        {
            if (!ClearCookie && MyUserSet != null && HttpContext.Current.Session["dataClassTrans"] != null)
            {
                DataTable dataClass = (DataTable)HttpContext.Current.Session["dataClassTrans"];

                ArrayList SellKey = new ArrayList();
                for (int i = 0; i < dataClass.Rows.Count; i++)
                {
                    SellKey.Add(dataClass.Rows[i]["SellSignature"].ToString());
                }
                objBooking.ClearExpiredSession(SellKey);
            }
            //HttpContext.Current.Session.Remove("TempFlight");
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
            HttpContext.Current.Session.Remove("GetssrAvailabilityResponseForBooking");
            HttpContext.Current.Session.Remove("GetssrAvailabilityResponseForBookingDrink");
            HttpContext.Current.Session.Remove("depart1");
            HttpContext.Current.Session.Remove("transit1");
            HttpContext.Current.Session.Remove("return1");
            HttpContext.Current.Session.Remove("depart2");
            HttpContext.Current.Session.Remove("transit2");
            HttpContext.Current.Session.Remove("return2");
            Session["IsNew"] = "True";
            //Seat
            HttpContext.Current.Session.Remove("btnSelected");
            HttpContext.Current.Session.Remove("TransID");
            HttpContext.Current.Session.Remove("signature");
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


            if (ClearCookie)
            {

                //added by diana 20140124 - store total pax
                HttpContext.Current.Session.Remove("TotalPax");
                HttpContext.Current.Session.Add("TotalPax", 0);

                HttpContext.Current.Session.Remove("TempFlight");
                HttpContext.Current.Session.Remove("tempFlight");
                HttpContext.Current.Session.Remove("invalidreturnflight");
                Session["IsNew"] = "true";
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
            }
            //txt_GeustNum.Text = "10";
        }

        protected void SearchDate(int mode, int days)
        {
            var profiler = MiniProfiler.Current;
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
                        //try to retrieve previous cookie value, and insert into variable called "cookieTemp"
                        HttpCookie cookieTemp = null;
                        if (Request.Cookies["cookieSearchcondition"] != null)
                        {
                            cookieTemp = Request.Cookies["cookieSearchcondition"];
                            cookieTemp.HttpOnly = true;

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
                        //cookie.Values.Add("InfantNum", cookieTemp.Values["InfantNum"]);
                        cookie.Values.Add("DepartureDetail", cookieTemp.Values["DepartureDetail"]);
                        cookie.Values.Add("ArrivalDetail", cookieTemp.Values["ArrivalDetail"]);
                        cookie.Values.Add("PromoCode", cookieTemp.Values["PromoCode"]);

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
                        //model.InfantNum = Convert.ToInt32(cookie.Values["InfantNum"]);
                        model.PromoCode = cookie.Values["PromoCode"];

                        //insert temFlight into session
                        using (profiler.Step("API:tempFlight"))
                        {
                            objBooking.tempFlight(model, MyUserSet.AgentName, "");
                        }

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
                            //model.InfantNum = Convert.ToInt32(cookie.Values["InfantNum"]);
                            model.PromoCode = cookie.Values["PromoCode"];

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
                                        temp = rows[18].ToString().Replace("|", " | ");
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

                            using (profiler.Step("GBS:BindList1"))
                            {
                                BindList1(model, tempDt);
                            }
                            if (oneway == false)
                            {
                                using (profiler.Step("GBS:BindList2"))
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
                        cookie.Values.Add("ifOneWay", cb_OneWay.Checked.ToString().ToUpper());
                        cookie.Values.Add("Currency", hfCurrency.Value);
                        if (daStart.Date == DateTime.Now.Date)
                        {
                            daStart.Value = DateTime.Now.AddHours(4);
                        }
                        cookie.Values.Add("DepartDate", daStart.Value.ToString());
                        if (cb_OneWay.Checked.ToString().ToUpper() == "FALSE")
                        {
                            cookie.Values.Add("ReturnDate", daEnd.Value.ToString());
                        }
                        cookie.Values.Add("PaxNum", num.ToString());
                        cookie.Values.Add("GuestNum", txt_GeustNum.Text);
                        cookie.Values.Add("ChildNum", txt_ChildNum.Text);
                        //cookie.Values.Add("InfantNum", txt_InfantNum.Text);
                        cookie.Values.Add("DepartureDetail", ddlDeparture.SelectedItem.Text);
                        //cookie.Values.Add("ArrivalDetail", ddlReturn.SelectedItem.Text);
                        cookie.Values.Add("ArrivalDetail", hfArrivalText.Value);

                        //cookie.Values.Add("PromoCode", txt_PromoCode.Text);
                        if (Session["dtSysPreft"] != null)
                        {
                            dtSysPreft = (DataTable)Session["dtSysPreft"];
                            cookie.Values.Add("PromoCode", dtSysPreft.Rows[0]["SYSValue"].ToString());
                        }
                        else
                            cookie.Values.Add("PromoCode", "");

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
                        //model.InfantNum = Convert.ToInt32(cookie.Values["InfantNum"]);
                        model.PromoCode = cookie.Values["PromoCode"];

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
                //added by romy for GB4,GB2
                dtSysPrefttry = (DataTable)Session["dtSysPrefttry"];
                var args = new string[4];
                int nopax = 0;
                //var args1 = new string[3];


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
                        DepartDate = Convert.ToDateTime(daStart.Value);
                        if (DepartDate.Date == DateTime.Now.Date)
                        {
                            DepartDate = DateTime.Now.AddHours(4);
                        }
                        hfdeStart.Value = DepartDate.ToString(); //amended by diana 20140128, moved from page load
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
                    if (cb_OneWay.Checked == false)
                    {
                        try
                        {
                            //ReturnDate = Convert.ToDateTime(Request.Form["ddlMarketMonth2"].ToString() + "-" + Request.Form["ddlMarketDay2"].ToString().PadLeft(2, '0'));
                            ReturnDate = Convert.ToDateTime(daEnd.Value);
                            hfdeEnd.Value = ReturnDate.ToString(); //amended by diana 20140128, moved from page load

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
                        txt_GeustNum.Text = "0";
                        //ifok = false;
                        //SelectFlightPanel.JSProperties["cp_result"] = msgList.Err100005;
                    }

                    //added by romy for GB4,GB2
                    //if ((args[0] != null && args[0].ToString() == Session["CountryCode"].ToString() && args[1].ToString() == ddlDeparture.SelectedItem.Value.ToString() &&
                    //    args[2].ToString() == Session["OrganizationCode"].ToString()) || (args1[0] != null && args1[0].ToString() == Session["CountryCode"].ToString() &&
                    //    args1[1].ToString() == ddlDeparture.SelectedItem.Value.ToString() && args1[2].ToString() == Session["OrganizationCode"].ToString()))
                    


                    if (dtSysPrefttry != null && dtSysPrefttry.Select("CountryCode = '" + Session["CountryCode"].ToString() + "' AND Origin = '" + ddlDeparture.SelectedItem.Value.ToString() + "' AND Status = 1 AND EffectiveDate <= '" + DateTime.Now.Date + "' AND ExpiryDate >= '" + DateTime.Now.Date + "' AND (AgentID = '" + MyUserSet.AgentID + "' OR AgentID = '')").Length > 0)
                    {
                        nopax = Convert.ToInt16(dtSysPrefttry.Select("CountryCode = '" + Session["CountryCode"].ToString() + "' AND Origin = '" + ddlDeparture.SelectedItem.Value.ToString() + "' AND Status = 1 AND EffectiveDate <= '" + DateTime.Now.Date + "' AND ExpiryDate >= '" + DateTime.Now.Date + "' AND (AgentID = '" + MyUserSet.AgentID + "' OR AgentID = '')")[0]["NoofPax"]);
                        if (txt_GeustNum.Text != "" && IsNumeric(txt_GeustNum.Text) == false || (txt_ChildNum.Text != "" && IsNumeric(txt_ChildNum.Text) == false))
                        {
                            ifok = false;
                            custommessage = "";//added by romy for custom message
                            custommessage = msgList.Err800001.Replace("string", nopax.ToString());
                            SelectFlightPanel.JSProperties["cp_result"] = custommessage;
                            //SelectFlightPanel.JSProperties["cp_result"] = "Pax number(Adult + Child pax) must be between 10 and 50. Please verify the number of passengers";
                        }
                    }
                    else
                    {
                        if (txt_GeustNum.Text != "" && IsNumeric(txt_GeustNum.Text) == false || (txt_ChildNum.Text != "" && IsNumeric(txt_ChildNum.Text) == false))
                        {
                            ifok = false;
                            SelectFlightPanel.JSProperties["cp_result"] = msgList.Err100006;
                        }
                    }

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

                        //added by romy for GB4,GB2

                        //if ((args[0] != null && args[0].ToString() == Session["CountryCode"].ToString() && args[1].ToString() == ddlDeparture.SelectedItem.Value.ToString() && args[2].ToString() == Session["OrganizationCode"].ToString()) || (args1[0] != null && args1[0].ToString() == Session["CountryCode"].ToString() && args1[1].ToString() == ddlDeparture.SelectedItem.Value.ToString() && args1[2].ToString() == Session["OrganizationCode"].ToString()))
                        if (dtSysPrefttry != null && dtSysPrefttry.Select("CountryCode = '" + Session["CountryCode"].ToString() + "' AND Origin = '" + ddlDeparture.SelectedItem.Value.ToString() + "' AND Status = 1 AND EffectiveDate <= '" + DateTime.Now.Date + "' AND ExpiryDate >= '" + DateTime.Now.Date + "' AND (AgentID = '" + MyUserSet.AgentID + "' OR AgentID = '')").Length > 0)
                        {
                            nopax = Convert.ToInt16(dtSysPrefttry.Select("CountryCode = '" + Session["CountryCode"].ToString() + "' AND Origin = '" + ddlDeparture.SelectedItem.Value.ToString() + "' AND Status = 1 AND EffectiveDate <= '" + DateTime.Now.Date + "' AND ExpiryDate >= '" + DateTime.Now.Date + "' AND (AgentID = '" + MyUserSet.AgentID + "' OR AgentID = '')")[0]["NoofPax"]);
                            if (num < nopax) //edited by ketee, 20171019, should verify base on system setting
                            {
                                ifok = false;
                                custommessage = "";//added by romy for custom message
                                custommessage = msgList.Err800001.Replace("string", nopax.ToString());
                                SelectFlightPanel.JSProperties["cp_result"] = custommessage;
                            }
                            else if (Convert.ToInt32(txt_GeustNum.Text) < 1)
                            {
                                ifok = false;
                                SelectFlightPanel.JSProperties["cp_result"] = msgList.Err100027;
                            }
                            else if (num > 50)
                            {
                                ifok = false;
                                SelectFlightPanel.JSProperties["cp_result"] = msgList.Err100067;
                            }
                            else if (Convert.ToInt32(txt_GeustNum.Text) > 50)
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
                        else
                        {
                            if (num < 10 || num > 50)
                            {
                                ifok = false;
                                SelectFlightPanel.JSProperties["cp_result"] = msgList.Err100006;
                            }
                            else if (Convert.ToInt32(txt_GeustNum.Text) > 50)
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
                        cookie.Values.Add("ifOneWay", cb_OneWay.Checked.ToString().ToUpper());
                        cookie.Values.Add("Currency", hfCurrency.Value);
                        cookie.Values.Add("DepartDate", DepartDate.ToString());
                        cookie.Values.Add("ReturnDate", ReturnDate.ToString());
                        cookie.Values.Add("PaxNum", num.ToString());
                        cookie.Values.Add("GuestNum", txt_GeustNum.Text);
                        cookie.Values.Add("ChildNum", ChildNum.ToString());
                        //cookie.Values.Add("InfantNum", infantNum.ToString());
                        cookie.Values.Add("DepartureDetail", ddlDeparture.SelectedItem.Text);
                        cookie.Values.Add("ArrivalDetail", hfArrivalText.Value);

                        //cookie.Values.Add("PromoCode", txt_PromoCode.Text);
                        if (Session["dtSysPreft"] != null)
                        {
                            dtSysPreft = (DataTable)Session["dtSysPreft"];
                            cookie.Values.Add("PromoCode", dtSysPreft.Rows[0]["SYSValue"].ToString());
                        }
                        else
                            cookie.Values.Add("PromoCode", "");

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
                        //model.InfantNum = Convert.ToInt32(cookie.Values["InfantNum"]);
                        model.PromoCode = cookie.Values["PromoCode"];

                        //insert temFlight into session
                        //string SessionID = "";
                        //ABS.Navitaire.APIBooking APIBooking = new ABS.Navitaire.APIBooking(SessionID);
                        //ABS.Navitaire.BookingManager.GetAvailabilityResponse response = APIBooking.GetAvailability(model.Arrival, model.DepartDate, model.Currency, model.Departure, model.PaxNum, ref SessionID, model.PromoCode);
                        //if (response == null)
                        //{
                        //    cookie.Values["PromoCode"] = "";
                        //    model.PromoCode = "";
                        //}

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
            if (lstOpt != null && lstOpt.Count > 0)
            {
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
        }

        protected int AddEnquiry()
        {
            int blacklist = 0;
            string enID = objBooking.CheckEnqExist(MyUserSet.AgentID, ddlDeparture.SelectedValue, hfArrival.Value);
            if (enID != "")
            {
                //counter enquiry
                enqLogInfo = objBooking.GetSingleEN_ENQUIRYLOG(enID, MyUserSet.AgentID, DateTime.Now);
                //Amended by Tyas 20170920 to fix Airbrake issue check if null
                if (enqLogInfo != null)
                {
                    enqLogInfo.EnquiryDate = DateTime.Now;
                    enqLogInfo.LastEnquiryDate = DateTime.Now;
                    enqLogInfo.NoOfAttempt = enqLogInfo.NoOfAttempt + 1;
                    enqLogInfo.SyncLastUpd = DateTime.Now;

                    objBooking.SaveEN_ENQUIRYLOG(enqLogInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Update);
                }
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
            if (enqLogInfo == null)
            {
                enqLogInfo = objBooking.GetSingleEN_ENQUIRYLOG(enID, MyUserSet.AgentID, DateTime.Now);
            }

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
    }
}