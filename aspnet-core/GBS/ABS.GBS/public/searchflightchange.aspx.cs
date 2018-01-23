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
using ABS.GBS.Log;

namespace GroupBooking.Web
{
    public partial class SearchFlightChange : System.Web.UI.Page
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

        string RecordLocator = ""; //added by diana 20140122 - for storing recordlocator to be changed
        List<BookingTransactionDetail> ListTransactionDetail;

        //public DateTime GoingDate;
        //public DateTime ReturnDate;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            string result = string.Empty;
            if (Session["AgentSet"] != null)
            { MyUserSet = (UserSet)Session["AgentSet"]; }

            //ClientScript.RegisterClientScriptBlock(GetType(), "IsPostBack", "var isPostBack = true;", true);

            //testing

            //testing

            //added by diana 20140423, not displaying value of date
            //GoingDate.Style.Add(HtmlTextWriterStyle.Display, "none");
            //ReturningDate.Style.Add(HtmlTextWriterStyle.Display, "none");

            if (IsPostBack)
            {
                ClearSession();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "PostbackKey", "<script type='text/javascript'>var isPostBack = true;</script>");
                DateTime DepartDate, ReturnDate;
                DepartDate = Convert.ToDateTime(Request.Form["ddlMarketMonth1"].ToString() + "-" + Request.Form["ddlMarketDay1"].ToString().PadLeft(2, '0'));
                if (cb_OneWay.Checked == false)
                {
                    ReturnDate = Convert.ToDateTime(Request.Form["ddlMarketMonth2"].ToString() + "-" + Request.Form["ddlMarketDay2"].ToString().PadLeft(2, '0'));
                    hReturn.Value = ReturnDate.ToString();
                }
                hDepart.Value = DepartDate.ToString();
            }
            else
            {
                //added by diana 20131107 - to clear journey
                objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                objBooking.ClearExpiredJourney(MyUserSet.AgentID);

                Cancel10MntBook();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "PostbackKey", "<script type='text/javascript'>var isPostBack = false;</script>");

                //added by diana 20140122 - grab PNR from url
                string keySent = Request.QueryString["k"];
                Session["TransID"] = Request.QueryString["TransID"];

                RecordLocator = Request.QueryString["RecordLocator"];
                Session["RecordLocator"] = RecordLocator;
                ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), RecordLocator.ToString(), "");
                if (hashkey != keySent)
                {
                    Response.Redirect("~/public/agentlogin.aspx");
                }

                Session["ReturnOnly"] = Request.QueryString["Return"]; //for return only purpose
                hReturnOnly.Value = Session["ReturnOnly"].ToString();

                InitializeForm();

                lblPNR.Text = Session["RecordLocator"].ToString();
                
                ListTransactionDetail = new List<BookingTransactionDetail>();
                ListTransactionDetail = objBooking.GetAllBK_TRANSDTLFlightByPNR(RecordLocator);
                int index = 0;
                foreach (BookingTransactionDetail TransDetail in ListTransactionDetail)
                {
                    if (index == 0)
                    {
                        ddlDeparture.SelectedValue = TransDetail.Origin;
                        LoadReturnCity(TransDetail.Origin);
                        ddlReturn.SelectedValue = TransDetail.Destination;
                        
                        txt_ChildNum.Text = TransDetail.PaxChild.ToString();
                        txt_GuestNum.Text = TransDetail.PaxAdult.ToString();

                        hDepart.Value = TransDetail.DepatureDate.ToString();
                    }
                    else if (index == 1)
                    {
                        hReturn.Value = TransDetail.DepatureDate.ToString();

                    }

                    index += 1;
                }
                if (index == 1)
                {
                    cb_OneWay.Checked = true;
                }
            }

            if (Session["SVCFAvailable"] != null)
            {
                if (Session["SVCFAvailable"].ToString() == "false")
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Alert", "<script type='text/javascript'>window.alert('There is no SCVF in this flight route. Kindly choose another flight route.');</script>");
                    Session["SVCFAvailable"] = "";
                }
            }
            //lblPNR.Text = Session["RecordLocator"].ToString();
            
        }
        //addded by agus
        // not yet used because we need PNR to cancel, and the clear function cannot be done even throught with same signature as request seat
        ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");
        private void Cancel10MntBook()
        {
            return;
            List<BookingTransactionMain> transmainInfo;
            BookingControl objBookingControl = new BookingControl();
            transmainInfo = objBookingControl.GetBK_TRANSMAIN10(0, Convert.ToInt32(MyUserSet.AgentID));
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

        protected void ValidatePopup(object sender, CallbackEventArgs e)
        {
            bool ReturnOnly = false;
            if (Session["ReturnOnly"].ToString() == "true")
            {
                ReturnOnly = true;
            }

            MessageList msgList = new MessageList();
            ArrayList aMsgList = new ArrayList();
            
            try
            {
                bool ifok = true;
                HttpContext.Current.Session.Remove("invalidreturnflight");
                if (objBooking.GetTotalSuspend((MyUserSet.AgentID)) >= Convert.ToInt16(MyUserSet.MaxSuspend))
                {
                    e.Result = msgList.Err100012;
                }
                else
                {

                    if (ddlDeparture.SelectedIndex == 0 || ddlReturn.SelectedIndex == 0)
                    {
                        ifok = false;
                        e.Result = msgList.Err100017;
                    }
                    DateTime DepartDate = new DateTime();
                    try
                    {
                        //DepartDate = Convert.ToDateTime(ddl_DepartYear.SelectedItem.Text + "-" + ddl_DepartMonth.SelectedValue + "-" + ddl_DepartDay.SelectedItem.Text.PadLeft(2, '0'));
                        DepartDate = Convert.ToDateTime(Request.Form["ddlMarketMonth1"].ToString() + "-" + Request.Form["ddlMarketDay1"].ToString().PadLeft(2, '0'));
                    }
                    catch
                    {
                        ifok = false;
                        e.Result = msgList.Err100002;

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
                            //ReturnDate = Convert.ToDateTime(ddl_ReturnYear.SelectedItem.Text + "-" + ddl_ReturnMonth.SelectedValue + "-" + ddl_ReturnDay.SelectedItem.Text.PadLeft(2, '0'));
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

                    if (txt_GuestNum.Text == "" )
                    {
                        ifok = false;
                        e.Result = msgList.Err100005;
                    }

                    if (IsNumeric(txt_GuestNum.Text) == false || (txt_ChildNum.Text != "" && IsNumeric(txt_ChildNum.Text) == false) )
                    {
                        ifok = false;
                        e.Result = msgList.Err100006;
                    }

                    DataTable dtRoute = new DataTable();
                    dtRoute = objBooking.GetAllSECTORSUSPEND(MyUserSet.OperationGroup, MyUserSet.AgentCategoryID, ddlDeparture.SelectedItem.Value.ToString(), ddlReturn.SelectedItem.Value.ToString(), DepartDate.Date);
                    if (dtRoute != null)
                    {
                        ifok = false;
                        e.Result = msgList.Err100038;
                    }
                    dtRoute = null;


                    int ChildNum = 0;
                    if (txt_ChildNum.Text != "")
                    {
                        ChildNum = Convert.ToInt32(txt_ChildNum.Text);
                    }
                    if (ifok)
                    {
                        if (Convert.ToInt32(txt_GuestNum.Text) == 0)
                        {
                            ifok = false;
                            e.Result = msgList.Err100027;
                        }

                        if (ddl_Currency.SelectedItem.Text == "")
                        {
                            ifok = false;
                            e.Result = msgList.Err100018;
                            return;
                        }
                        else
                        {
                            num = Convert.ToInt32(txt_GuestNum.Text) + ChildNum;
                        }
                        //if (num < 10 || num > 50)
                        //{
                        //    ifok = false;
                        //    e.Result = msgList.Err100006;
                        //}
                        //else if (Convert.ToInt32(txt_GuestNum.Text) > 50)
                        //{
                        //    ifok = false;
                        //    e.Result = msgList.Err100007;
                        //}
                        //else if (ChildNum > 50)
                        //{
                        //    ifok = false;
                        //    e.Result = msgList.Err100008;
                        //}
                    }

                    
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
                        //cookie.Values.Add("Carrier", ddl_Carrier.SelectedItem.Text);                       

                        cookie.Values.Add("Departure", ddlDeparture.SelectedValue);
                        cookie.Values.Add("Arrival", ddlReturn.SelectedValue);
                        cookie.Values.Add("ifOneWay", cb_OneWay.Checked.ToString().ToUpper());
                        cookie.Values.Add("Currency", ddl_Currency.SelectedItem.Text);
                        cookie.Values.Add("DepartDate", DepartDate.ToString());
                        cookie.Values.Add("ReturnDate", ReturnDate.ToString());
                        cookie.Values.Add("PaxNum", num.ToString());
                        cookie.Values.Add("GuestNum", txt_GuestNum.Text);
                        cookie.Values.Add("ChildNum", ChildNum.ToString());
                        cookie.Values.Add("DepartureDetail", ddlDeparture.SelectedItem.Text);
                        cookie.Values.Add("ArrivalDetail", ddlReturn.SelectedItem.Text);
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

                        //insert temFlight into session

                        decimal totalPrevious = objBooking.GetTotalCharge(Session["RecordLocator"].ToString(), model.Departure, model.Arrival);
                        Session["totalPrevious"] = totalPrevious;

                        decimal totalReturnPrevious = objBooking.GetTotalCharge(Session["RecordLocator"].ToString(), model.Arrival, model.Departure);
                        Session["totalReturnPrevious"] = totalReturnPrevious;

                        objBooking.tempFlight(model, MyUserSet.AgentName, "", true, decimal.Parse(Session["totalPrevious"].ToString()), decimal.Parse(Session["totalReturnPrevious"].ToString()), ReturnOnly);

                        DataList dtModel1 = new DataList();
                        DataList dtModel2 = new DataList();

                        DataTable tempDt = new DataTable();
                        tempDt = objBooking.dtFlight();

                        //added by ketee, validate valid flight
                        if (HttpContext.Current.Session["invalidreturnflight"] != null && HttpContext.Current.Session["invalidreturnflight"].ToString() == "1")
                        {
                            ifok = false;
                            e.Result = msgList.Err100041;
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
                                    if (ReturnOnly == true) //if return only, then check for 2nd datatable only
                                    {
                                        if (dtModel2.Items.Count <= 0)
                                        {
                                            ifok = false;
                                            e.Result = msgList.Err100013;
                                        }
                                    }
                                    else if (dtModel1.Items.Count <= 0 || dtModel2.Items.Count <= 0)
                                    {
                                        ifok = false;
                                        e.Result = msgList.Err100013;
                                    }
                                }
                                else
                                {
                                    ifok = false;
                                    e.Result = msgList.Err100013;
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
                                        e.Result = msgList.Err100013;
                                    }
                                }
                                else
                                {
                                    ifok = false;
                                    e.Result = msgList.Err100013;
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

                                    e.Result = msgList.Err100012;

                                }
                                else
                                {
                                    RefreshPaymentSession();
                                    e.Result = "";
                                }
                                //Response.Redirect("selectflightchange.aspx");
                            }
                            else
                            {
                                e.Result = msgList.Err100010;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                e.Result = msgList.Err100031;
                log.Error(this,ex);
                //Response.Redirect(Shared.MySite.PublicPages.AgentMain, false);
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
            //ClearCityPair();
        }

        private void LoadReturnCity(string Origin)
        {
            try
            {
                GeneralControl CountryBase = new GeneralControl();
                DataTable dt = new DataTable();

                dt = CountryBase.GetLookUpCity(Origin, Request.PhysicalApplicationPath);
                SEAL.WEB.UI.Control.SetComboCustomStyle(ref ddlReturn, dt, "RCustomState", "RCityCode", "Select City");
                if (dt == null || dt.Rows.Count <= 0)
                {
                    dt = CountryBase.ReturnAllCityCustom(Origin);
                    SEAL.WEB.UI.Control.SetComboCustomStyle(ref ddlReturn, dt, "CustomState", "CityCode", "Select City");
                }
                BindCurrency(Origin);
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this,ex);
            }
        }
        private string ConvTwoDigitDate(string date)
        {
            if (date.Length == 1) { date = "0" + date; }
            return date;
        }

        private void initializeData()
        {
            txt_GuestNum.Text = "10";

            string monthNumeric = "";
            string monthString = "";
            string year = "";
            DateTime dateNow = DateTime.Now.AddDays(-20); //change from +2 to -20

            DateTime tempDate = new DateTime();
            DateTime tempDate2 = new DateTime();
            divDate1.InnerHtml = "";
            divDate2.InnerHtml = "";

            #region initdatedepart

            if (Session["ReturnOnly"].ToString() == "true")
            {
                divDate1.InnerHtml += "<table><tr><td><select id=\"ddlMarketDay1\" name=\"ddlMarketDay1\" disabled>";
            }
            else
            {
                divDate1.InnerHtml += "<table><tr><td><select id=\"ddlMarketDay1\" name=\"ddlMarketDay1\" >";
            }
            
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

            if (Session["ReturnOnly"].ToString() == "true")
            {
                divDate1.InnerHtml += "<td>&nbsp;</td><td><select class=\"wLrg2s\" id=\"ddlMarketMonth1\" name=\"ddlMarketMonth1\" disabled>";
            }
            else
            {
                divDate1.InnerHtml += "<td>&nbsp;</td><td><select class=\"wLrg2s\" id=\"ddlMarketMonth1\" name=\"ddlMarketMonth1\">";
            }

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
            if (cb_OneWay.Checked == true)
            {
                tdReturn.Attributes["display"] = "none";
            }
            #endregion

            //UIClass.SetComboCustomStyle(ref ddlDeparture, UIClass.EnumDefineStyle.City, string.Empty, string.Empty, false);
            //UIClass.SetComboCustomStyle(ref ddlReturn, UIClass.EnumDefineStyle.City, string.Empty, ddlDeparture.SelectedItem.Value, false);

            GeneralControl CountryBase = new GeneralControl();
            DataTable dt = new DataTable();
            dt = CountryBase.GetLookUpCity("", Request.PhysicalApplicationPath);
            if (dt == null || dt.Rows.Count <= 0)
            {
                dt = CountryBase.ReturnAllCityCustom("");
            }
            SEAL.WEB.UI.Control.SetComboCustomStyle(ref ddlDeparture, dt, "CustomState", "CityCode", "Select City");

        }

        private void BindCurrency(string Departure)
        {
            try
            {
                GeneralControl CountryBase = new GeneralControl();
                DataTable dt = new DataTable();
                dt = CountryBase.GetLookUpCity("", Request.PhysicalApplicationPath);
                DataRow[] drs = dt.Select("CityCode='" + Departure + "'");
                if (drs != null)
                {
                    ddl_Currency.SelectedItem.Text = drs[0]["Currency"].ToString();
                    //ddl_Currency.SelectedItem.Text = objGeneral.GetCurrencyByDeparture(Departure);

                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this,ex);
            }

        }

        protected bool CheckSuspend()
        {
            if (objBooking.CheckStillSuspend(MyUserSet.AgentID, ddlDeparture.SelectedValue, ddlReturn.SelectedValue))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected int AddEnquiry()
        {
            int blacklist = 0;
            string enID = objBooking.CheckEnqExist(MyUserSet.AgentID, ddlDeparture.SelectedValue, ddlReturn.SelectedValue);
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
                enqLogInfo.Destination = ddlReturn.SelectedValue;
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

                string susID = objBooking.CheckSuspExist(MyUserSet.AgentID, ddlDeparture.SelectedValue, ddlReturn.SelectedValue);
                if (susID != "")
                {
                    enqSusInfo = objBooking.GetSingleEN_SUSPENDLIST(susID, MyUserSet.AgentID, ddlDeparture.SelectedValue, ddlReturn.SelectedValue);
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
                    enqSusInfo.Destination = ddlReturn.SelectedValue;
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

        protected void ddlDeparture_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadReturnCity(ddlDeparture.SelectedItem.Value);
        }

        private void BindDDL()
        {
            string departure = ddlDeparture.SelectedItem.Value;
            if (departure == "KUL")
                ddl_Currency.SelectedItem.Text = "Malaysian Ringgit (MYR)";
            else if (departure == "SZX")
                ddl_Currency.SelectedItem.Text = "Chinese Renminbi Yuan (CNY)";
            else ddl_Currency.SelectedItem.Value = "US Dollar (USD)";
        }

        protected void cb_OneWay_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_OneWay.Checked == true)
            {
                tdReturn.Visible = false;
            }
            else
            {
                tdReturn.Visible = true;
            }
        }

    }
}