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
using ABS.GBS.Log;

namespace GroupBooking.Web
{
	public partial class agentsearchflightchange : System.Web.UI.Page
    {
        #region declaration
        UserSet MyUserSet;
        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        ABS.Logic.GroupBooking.Agent.AgentControl objAgent = new ABS.Logic.GroupBooking.Agent.AgentControl();
        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
        BookingEnquiry enqLogInfo = new BookingEnquiry();
        BookingSuspendList enqSusInfo = new BookingSuspendList();
        AgentBlacklistLog agBlacklistInfo = new AgentBlacklistLog();
        AgentActivity agActivityInfo = new AgentActivity();
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogControl log = new LogControl();
        SystemLog SystemLog = new SystemLog();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            string result = string.Empty;
            if (Session["AgentSet"] != null)
            { MyUserSet = (UserSet)Session["AgentSet"]; }

            ClientScript.RegisterClientScriptBlock(GetType(), "IsPostBack", "var isPostBack = true;", true);

            if (IsPostBack)
            {

            }
            else
            {
                InitializeForm();
            }
        }

        private void ClearSession()
        {
            HttpContext.Current.Session.Remove("tempFlight");
            HttpContext.Current.Session.Remove("dataClass");
        }

        public static bool IsNumeric(string s)
        {
            double result;
            return s != null && Double.TryParse(s, out result);
        }

        protected void ValidatePopup(object sender, CallbackEventArgs e)
        {
            MessageList msgList = new MessageList();
            ArrayList aMsgList = new ArrayList();
            BookingTransactionMain bookingMain = new BookingTransactionMain();

            int maxDay = Convert.ToInt32(objGeneral.getSysValueByKeyAndGroupID("CHGFLIGHTDURA", "AA"));
            bookingMain = (BookingTransactionMain)HttpContext.Current.Session["TransMainData"];
            DateTime maxDate = bookingMain.STDDate.AddDays(maxDay);
            DateTime minDate = bookingMain.STDDate.AddDays(-maxDay);
            try
            {
                bool ifok = true;

                if (ddlDeparture.SelectedIndex == 0 || ddlReturn.SelectedIndex == 0)
                {
                    ifok = false;
                    e.Result = msgList.Err100017;
                }
                DateTime DepartDate = new DateTime();
                try
                {
                    DepartDate = Convert.ToDateTime(Request.Form["ddlMarketMonth1"].ToString() + "-" + Request.Form["ddlMarketDay1"].ToString().PadLeft(2, '0'));
                }
                catch
                {
                    ifok = false;
                    e.Result = msgList.Err100002;

                }

                if (DepartDate > maxDate)
                {
                    ifok = false;
                    e.Result = "Maximum Depart Date : " + String.Format("{0:MM/dd/yyyy}", maxDate);
                }

                if (DepartDate < minDate)
                {
                    ifok = false;
                    e.Result = "Minimum Depart Date : " + String.Format("{0:MM/dd/yyyy}", minDate);
                }

                DateTime ReturnDate = new DateTime();
                if (DepartDate.AddDays(2) < DateTime.Now && ifok == true)
                {
                    ifok = false;
                    e.Result = msgList.Err100002;

                }
                if (cb_OneWay.Checked == false)
                {
                    try
                    {
                        ReturnDate = Convert.ToDateTime(Request.Form["ddlMarketMonth2"].ToString() + "-" + Request.Form["ddlMarketDay2"].ToString().PadLeft(2, '0'));
                        if (DepartDate > ReturnDate)
                        {
                            ifok = false;
                            e.Result = msgList.Err100003;

                        }
                    }
                    catch
                    {
                        ifok = false;
                        e.Result = msgList.Err100004;
                    }
                }
                int num = 0;

                if (ifok)
                {
                    if (ddl_Currency.SelectedItem.Text == "")
                    {
                        ifok = false;
                        e.Result = msgList.Err100005;
                    }
                    else
                    {
                        num = Convert.ToInt32(bookingMain.TransTotalPAX);
                    }
                }

                if (ifok)
                {
                    if (Request.Cookies["cookieSearchcondition"] != null)
                    {
                        HttpCookie cookieTemp = Request.Cookies["cookieSearchcondition"];
                        if (cookieTemp != null)
                        {
                            cookieTemp.Expires = DateTime.Today.AddDays(-1);
                            Response.Cookies.Add(cookieTemp);
                        }
                    }

                    HttpCookie cookie = new HttpCookie("cookieSearchcondition");
                    cookie.HttpOnly = true;
                    //cookie.Values.Add("Carrier", ddl_Carrier.SelectedItem.Text);                       

                    cookie.Values.Add("Departure", ddlDeparture.SelectedValue);
                    cookie.Values.Add("Arrival", ddlReturn.SelectedValue);
                    cookie.Values.Add("ifOneWay", cb_OneWay.Checked.ToString().ToUpper());
                    cookie.Values.Add("Currency", ddl_Currency.SelectedItem.Text);
                    cookie.Values.Add("DepartDate", DepartDate.ToString());
                    cookie.Values.Add("ReturnDate", ReturnDate.ToString());
                    cookie.Values.Add("PaxNum", num.ToString());
                    cookie.Values.Add("GuestNum", num.ToString());
                    cookie.Values.Add("ChildNum", "0");
                    cookie.Values.Add("DepartureDetail", ddlDeparture.SelectedItem.Text);
                    cookie.Values.Add("ArrivalDetail", ddlReturn.SelectedItem.Text);
                    Response.AppendCookie(cookie);

                    //check available flight

                    ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition model = new ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition();

                    model.Carrier = cookie.Values["Carrier"];
                    model.Departure = cookie.Values["Departure"];
                    model.Arrival = cookie.Values["Arrival"];
                    bool oneway = false;
                    if (cookie.Values["ifOneWay"] == "TRUE")
                    {
                        oneway = true;
                    }
                    else
                    {
                        model.ReturnDate = Convert.ToDateTime(cookie.Values["ReturnDate"]);
                    }
                    model.ifOneWay = oneway;
                    model.Currency = cookie.Values["Currency"];
                    model.DepartDate = Convert.ToDateTime(cookie.Values["DepartDate"]);
                    model.ReturnDate = Convert.ToDateTime(cookie.Values["ReturnDate"]);
                    model.GuestNum = Convert.ToInt32(cookie.Values["GuestNum"]);
                    model.PaxNum = Convert.ToInt32(cookie.Values["PaxNum"]);
                    model.ChildNum = Convert.ToInt32(cookie.Values["ChildNum"]);

                    //insert temFlight into session

                    objBooking.tempFlight(model, bookingMain.AgentName, "");

                    DataList dtModel1 = new DataList();
                    DataList dtModel2 = new DataList();

                    DataTable tempDt = new DataTable();
                    tempDt = objBooking.dtFlight();

                    if (oneway == false)
                    {
                        if (HttpContext.Current.Session["tempFlight"] != null)
                        {
                            //
                            string strExpr;
                            string strSort;
                            DataTable dt = new DataTable();

                            strExpr = "TemFlightDeparture = '" + model.Departure + "' AND TemFlightArrival = '" + model.Arrival + "' AND TemFlightAgentName = '" + bookingMain.AgentName + "' AND TemFlightIfReturn = false";

                            strSort = "";

                            dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                            DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                            tempDt.Clear();

                            foreach (DataRow row in foundRows)
                            {
                                tempDt.ImportRow(row);
                            }

                            dtModel1.DataSource = tempDt;
                            dtModel1.DataBind();

                            strExpr = "TemFlightDeparture = '" + model.Arrival + "' AND TemFlightArrival = '" + model.Departure + "' AND TemFlightAgentName = '" + bookingMain.AgentName + "' AND TemFlightIfReturn = true";

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
                                e.Result = msgList.Err100013;
                            }
                        }
                        else
                        {
                            ifok = false;
                            e.Result = msgList.Err100013;
                        }

                    }
                    else
                    {
                        if (HttpContext.Current.Session["tempFlight"] != null)
                        {
                            string strExpr;
                            string strSort;
                            DataTable dt = new DataTable();

                            strExpr = "TemFlightDeparture = '" + model.Departure + "' AND TemFlightArrival = '" + model.Arrival + "' AND TemFlightAgentName = '" + bookingMain.AgentName + "' AND TemFlightIfReturn = false";

                            strSort = "";

                            dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                            DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                            tempDt.Clear();

                            foreach (DataRow row in foundRows)
                            {
                                tempDt.ImportRow(row);
                            }

                            dtModel1.DataSource = tempDt;
                            dtModel1.DataBind();

                            if (dtModel1.Items.Count <= 0)
                            {
                                ifok = false;
                                e.Result = msgList.Err100013;
                            }
                        }
                        else
                        {
                            ifok = false;
                            e.Result = msgList.Err100013;
                        }

                    }

                    if (ifok)
                    {
                        RefreshPaymentSession();
                        e.Result = "";
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                e.Result = msgList.Err100025;
                log.Error(this,ex);
                Response.Redirect(Shared.MySite.PublicPages.AgentSearchFlightChange, false);
            }
        }

        private void RefreshPaymentSession()
        {
            HttpContext.Current.Session.Remove("ErrorPayment");
        }

        private void InitializeForm()
        {
            HttpCookie cookieLogin = Request.Cookies["cookieLoginName"];

            initializeData();
            HttpCookie cookie = Request.Cookies["cookieSearchcondition"];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Today.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
            ClearSession();
            SetCheckBoxOneWay();
            SetDepartReturnData();
        }

        private void SetDepartReturnData()
        {
            if (HttpContext.Current.Session["ListDetailData"] != null)
            {
                List<BookingTransactionDetail> listDetail = (List<BookingTransactionDetail>)HttpContext.Current.Session["ListDetailData"];
                string Origin = listDetail.ElementAt(0).Origin.ToString();
                string Destination = listDetail.ElementAt(0).Destination.ToString();
                ddlDeparture.Items.FindByValue(Origin).Selected = true;

                GeneralControl CountryBase = new GeneralControl();
                DataTable dt = new DataTable();
                dt = CountryBase.ReturnAllCityCustom(ddlDeparture.SelectedItem.Value);
                SEAL.WEB.UI.Control.SetComboCustomStyle(ref ddlReturn, dt, "CustomState", "CityCode", "Select City");
                BindCurrency(ddlDeparture.SelectedItem.Value);

                ddlReturn.Items.FindByValue(Destination).Selected = true;
                ddlDeparture.Enabled = false;
                ddlReturn.Enabled = false;
            }
        }

        private void SetCheckBoxOneWay()
        {
            if (HttpContext.Current.Session["flagOneWay"] != null)
            {
                Boolean flagOneWay = (Boolean)HttpContext.Current.Session["flagOneWay"];
                if (flagOneWay) { cb_OneWay.Checked = true; }
                else { cb_OneWay.Checked = false; }
                cb_OneWay.Enabled = false;
                if (cb_OneWay.Checked == true)
                {
                    trReturn.Visible = false;
                }
                else
                {
                    trReturn.Visible = true;
                }
            }
        }

        private string ConvTwoDigitDate(string date)
        {
            if (date.Length == 1) { date = "0" + date; }
            return date;
        }

        private void initializeData()
        {
            string monthNumeric = "";
            string monthString = "";
            string year = "";
            DateTime dateNow = DateTime.Now;
            DateTime tempDate = new DateTime();
            DateTime tempDate2 = new DateTime();
            //divDate1.InnerHtml = "";
            //divDate2.InnerHtml = "";

            #region initdatedepart
            divDate1.InnerHtml += "<select id=\"ddlMarketDay1\" name=\"ddlMarketDay1\" >";
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
            divDate1.InnerHtml += "</select>";
            divDate1.InnerHtml += "<select class=\"wLrg2s\" id=\"ddlMarketMonth1\" name=\"ddlMarketMonth1\">";

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
            divDate1.InnerHtml += "</select>";
            divDate1.InnerHtml += "<input id=\"date_picker_id_1\" type=\"hidden\" name=\"date_picker\" value=\"\" />";
            #endregion

            #region initdatereturn

            tempDate = dateNow.AddDays(7);

            divDate2.InnerHtml += "<select id=\"ddlMarketDay2\" name=\"ddlMarketDay2\">";
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
            divDate2.InnerHtml += "</select>";
            divDate2.InnerHtml += "<select class=\"wLrg2s\" id=\"ddlMarketMonth2\" name=\"ddlMarketMonth2\">";

            for (int i = 0; i <= 13; i++)
            {
                /*
                tempDate2 = dateNow.AddDays(7);
                tempDate = tempDate2.AddMonths(i);
                */
                tempDate = dateNow.AddMonths(i);
                monthNumeric = tempDate.ToString("MM");
                monthString = tempDate.ToString("MMM");
                year = tempDate.ToString("yyyy");
                string val = year + "-" + monthNumeric;
                string disp = monthString + " " + year;

                if (tempDate.Month == Convert.ToInt16(monthNumeric))
                {
                    divDate2.InnerHtml += "<option selected value=\"" + val + "\">" + disp + "</option>";
                }
                else
                {
                    divDate2.InnerHtml += "<option value=\"" + val + "\">" + disp + "</option>";
                }

            }
            divDate2.InnerHtml += "</select>";
            divDate2.InnerHtml += "<input id=\"date_picker_id_2\" type=\"hidden\" name=\"date_picker\" value=\"\" />";
            #endregion

            GeneralControl CountryBase = new GeneralControl();
            DataTable dt = new DataTable();
            dt = CountryBase.ReturnAllCityCustom("");
            SEAL.WEB.UI.Control.SetComboCustomStyle(ref ddlDeparture, dt, "CustomState", "CityCode", "Select City");

        }

        private void BindCurrency(string Departure)
        {
            try
            {
                ddl_Currency.SelectedItem.Text = objGeneral.GetCurrencyByDeparture(Departure);
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this,ex);
            }

        }

        protected void ddlDeparture_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GeneralControl CountryBase = new GeneralControl();
                DataTable dt = new DataTable();
                dt = CountryBase.ReturnAllCityCustom(ddlDeparture.SelectedItem.Value);
                SEAL.WEB.UI.Control.SetComboCustomStyle(ref ddlReturn, dt, "CustomState", "CityCode", "Select City");
                BindCurrency(ddlDeparture.SelectedItem.Value);
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this,ex);
            }
        }

        protected void cb_OneWay_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_OneWay.Checked == true)
            {
                trReturn.Visible = false;
            }
            else
            {
                trReturn.Visible = true;
            }
        }

    }
}