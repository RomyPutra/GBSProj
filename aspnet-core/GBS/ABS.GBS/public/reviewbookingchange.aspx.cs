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
//using log4net;
using System.Globalization;
using ABS.GBS.Log;

namespace GroupBooking.Web.Booking
{
    public partial class ReviewBookingChange : System.Web.UI.Page
    {
        #region declaration

        UserSet MyUserSet;
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogControl log = new LogControl();
        SystemLog SystemLog = new SystemLog();

        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
        decimal totalFlightFare, totalFUEFare, totalBaggageFare = 0;

        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
        ABS.Logic.GroupBooking.Agent.AgentControl objAgent = new ABS.Logic.GroupBooking.Agent.AgentControl();
        ABS.Logic.GroupBooking.Agent.AgentProfileControl objAgentProfile = new ABS.Logic.GroupBooking.Agent.AgentProfileControl();

        BookingTransactionDetail bookDTLInfo = new BookingTransactionDetail();
        List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
        BookingTransactionMain bookHDRInfo = new BookingTransactionMain();

        //BookingTransactionTax transTaxInfo = new BookingTransactionTax();

        //BookingTaxFeesControl taxFeesControlInfo = new BookingTaxFeesControl();

        //BookingTransactionFees transFeesInfo = new BookingTransactionFees();
        //List<BookingTransactionFees> lsttransFeesInfo = new List<BookingTransactionFees>();

        ABS.Logic.GroupBooking.Agent.AgentProfile agent = new ABS.Logic.GroupBooking.Agent.AgentProfile();

        string Currency = "USD";
        decimal TotalAmount = 0;
        decimal APT = 0;
        decimal AVGFare = 0;
        string tranID = "";
        DataTable dtTaxFees = new DataTable();

        private static int num = 0;
        private static int departID = 0;
        private static string ReturnID = "";
        private static decimal TotalMoney = 0;

        bool ReturnOnly = false;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["ReturnOnly"].ToString() == "true")
            {
                ReturnOnly = true;
            }

            Session["NewBooking"] = "true";
            if (Session["AgentSet"] != null)
            {
                MyUserSet = (UserSet)Session["AgentSet"];
            }

            //setTaxCode();
            if (!IsPostBack)
            {
                //SavingProcess();
                InitializeForm();
            }
        }

        protected void setTaxCode()
        {
            List<BookingTaxFeesControl> listTaxFeesControl = new List<BookingTaxFeesControl>();
            listTaxFeesControl = objBooking.GetAllBK_TAXFEESCONTROL();
            if (listTaxFeesControl.Count > 0)
            {
                dtTaxFees = new DataTable();
                dtTaxFees.Columns.Add("TaxFeesCode");
                dtTaxFees.Columns.Add("TaxFeesGroup");
                dtTaxFees.Columns.Add("TaxFeesRate");

                int ctr = 0;

                foreach (BookingTaxFeesControl objtaxfees in listTaxFeesControl)
                {
                    DataRow row;
                    row = dtTaxFees.NewRow();
                    row["TaxFeesCode"] = objtaxfees.TaxFeesCode.ToString();
                    row["TaxFeesGroup"] = objtaxfees.TaxFeesGroup.ToString();
                    row["TaxFeesRate"] = objtaxfees.TaxFeesRate.ToString();
                    dtTaxFees.Rows.Add(row);
                    ctr += 1;
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

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this,ex);
            }
        }

        protected void ValidatePopup(object sender, CallbackEventArgs e)
        {
            MessageList msgList = new MessageList();
            ArrayList aMsgList = new ArrayList();
            try
            {
                #region prev
                //ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
                //ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight2 = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();

                //string strExpr;
                //string strSort;
                //DataTable dt = new DataTable();
                //strExpr = "TemFlightId = '" + departID + "'";
                //strSort = "";

                //dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                //DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                //FillModelFromDataRow(foundRows, ref  temFlight);

                //if (ReturnID != "")
                //{
                //    strExpr = "TemFlightId = '" + ReturnID + "'";
                //    strSort = "";

                //    dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                //    foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                //    FillModelFromDataRow(foundRows, ref  temFlight2);

                //    //string LoginType = MyUserSet.AgentType.ToString();  //Session["LoginType"].ToString();
                //    string LoginType = "PublicAgent";
                //    string LoginName = MyUserSet.AgentName;
                //    string LoginPWD = "";
                //    string LoginDomain = "";
                //    /* remark to ag payment process
                //    if (LoginType == "SkyAgent")
                //    {
                //        LoginPWD = Session["LoginPWD"].ToString();
                //        LoginDomain = Session["LoginDomain"].ToString();
                //    }*/
                //    //objBooking.SellFlightByTem(temFlight, temFlight2, LoginType, LoginName, LoginPWD, LoginDomain, "");
                //    objBooking.SellJourney(temFlight, temFlight2, LoginType, LoginName, LoginPWD, LoginDomain, "");
                //}
                //else
                //{                    
                //    //string LoginType = MyUserSet.AgentType.ToString();
                //    string LoginType = "PublicAgent";
                //    string LoginName = MyUserSet.AgentName;
                //    string LoginPWD = "";
                //    string LoginDomain = "";
                //    /* remark to ag payment process
                //    if (LoginType == "SkyAgent")
                //    {
                //        LoginPWD = Session["LoginPWD"].ToString();
                //        LoginDomain = Session["LoginDomain"].ToString();
                //    }*/
                //    objBooking.SellFlightByTem(temFlight, LoginType, LoginName, LoginPWD, LoginDomain, "");

                //}

                //if (SaveData())
                //    e.Result = "";
                //else
                //    e.Result = msgList.Err100031;
                #endregion

                e.Result = "";
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                e.Result = msgList.Err100013;
            }
        }

        protected static bool IsNumeric(string s)
        {
            double result;
            return s != null && Double.TryParse(s, out result);
        }

        #region prevsavedata
        //protected bool SaveData()
        //{

        //    ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();

        //    string strExpr;
        //    string strSort;
        //    string keyCarrier = "";
        //    decimal totalOth = 0; //service charge total
        //    DataTable dt = new DataTable();
        //    Hashtable ht = new Hashtable();

        //    strExpr = "TemFlightId = '" + departID + "'";
        //    strSort = "";
        //    DateTime departDate;
        //    dt = (DataTable)HttpContext.Current.Session["TempFlight"];
        //    DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
        //    FillModelFromDataRow(foundRows, ref  temFlight);

        //    departDate = Convert.ToDateTime(temFlight.TemFlightStd);

        //    Currency = temFlight.TemFlightCurrencyCode.Trim();

        //    if (IsNumeric(lbl_TotalAmount.Text))
        //    {
        //        TotalMoney = Convert.ToDecimal(lbl_TotalAmount.Text);
        //        TotalAmount = Convert.ToDecimal(lbl_TotalAmount.Text);
        //    }

        //    agent = objAgentProfile.GetSingleAgentProfile(MyUserSet.AgentName);

        //    string LoginType = MyUserSet.AgentType.ToString();

        //    int m = 0;
        //    int count = 0;
        //    DataTable dtClass = objBooking.dtClass();
        //    if (HttpContext.Current.Session["dataClass"] != null) { dtClass = (DataTable)HttpContext.Current.Session["dataClass"]; }
        //    count = dtClass.Rows.Count;

        //    byte seqNo = 1;
        //    List<BookingTransactionMain> TransMain = new List<BookingTransactionMain>();

        //    tranID = DateTime.Now.ToString("yyyyMMddHHmmsss");

        //    #region newsavedetail
        //    //Datatable Process 

        //    //string PNR = book.BookingCommit(agent, temClass.TemClassPersonNumber, temClass.TemClassClassPrice.Value, temClass.TemClassSellSignature);

        //    DataTable dataClass = objBooking.dtClass();
        //    dataClass = (DataTable)HttpContext.Current.Session["dataClass"];
        //    foreach (DataRow dr in dataClass.Rows)
        //    {
        //        bookDTLInfo = new BookingTransactionDetail();
        //        string PNR = seqNo.ToString();
        //        bookDTLInfo.RecordLocator = PNR;
        //        bookDTLInfo.TransID = tranID;
        //        bookDTLInfo.SeqNo = seqNo;

        //        if (seqNo == 1)
        //        {
        //            keyCarrier = bookDTLInfo.CarrierCode = dr["CarrierCode"].ToString();
        //            ht.Add("keyCarrier", keyCarrier);
        //        }

        //        //service charge pax
        //        decimal svcCharge = Convert.ToDecimal(objGeneral.getSysValueByKeyAndCarrierCode("SVCCHARGEPAX", keyCarrier)) / 100;
        //        bookDTLInfo.LineOth = svcCharge * Convert.ToDecimal(dr["FullPrice"].ToString());
        //        totalOth += bookDTLInfo.LineOth;

        //        seqNo += 1;
        //        bookDTLInfo.Currency = Currency;
        //        bookDTLInfo.CarrierCode = dr["CarrierCode"].ToString();
        //        bookDTLInfo.FlightNo = dr["FlightNumber"].ToString();
        //        bookDTLInfo.DepatureDate = Convert.ToDateTime(dr["DepartureDate"].ToString());
        //        bookDTLInfo.ArrivalDate = Convert.ToDateTime(dr["ArrivalDate"].ToString());
        //        bookDTLInfo.Origin = dr["Origin"].ToString();
        //        bookDTLInfo.Destination = dr["Destination"].ToString();
        //        bookDTLInfo.LineFee = Convert.ToDecimal(dr["FuelChrg"].ToString());
        //        bookDTLInfo.FareClass = dr["FareClass"].ToString();
        //        bookDTLInfo.PaxAdult = Convert.ToInt16(dr["AdultPax"].ToString());
        //        bookDTLInfo.PaxChild = Convert.ToInt16(dr["ChildPax"].ToString());
        //        bookDTLInfo.LineTotal = Convert.ToDecimal(dr["FullPrice"].ToString());
        //        bookDTLInfo.SyncLastUpd = DateTime.Now;
        //        bookDTLInfo.LastSyncBy = MyUserSet.AgentID;
        //        bookDTLInfo.FarePerPax = Convert.ToDecimal(dr["FarePrice"].ToString());
        //        bookDTLInfo.LineTax = Convert.ToDecimal(dr["TaxChrg"].ToString());                
        //        //totalFlightFare += bookDTLInfo.LineTotal;
        //        totalFlightFare += bookDTLInfo.LineTotal + bookDTLInfo.LineOth; //include service charge

        //        bookDTLInfo.TransVoid = 0;
        //        bookDTLInfo.CreateBy = MyUserSet.AgentID;
        //        bookDTLInfo.SyncCreate = DateTime.Now;

        //        bookDTLInfo.Transit = dr["TemClassTransit"].ToString();
        //        if (bookDTLInfo.Transit != "")
        //        {
        //            bookDTLInfo.ArrivalDate2 = Convert.ToDateTime(dr["TemClassSta2"].ToString());
        //            bookDTLInfo.DepatureDate2 = Convert.ToDateTime(dr["TemClassStd2"].ToString());
        //        }

        //        bookDTLInfo.CollectedAmount = 0;
        //        bookDTLInfo.Signature = dr["SellSignature"].ToString();

        //        // objBooking.SaveBK_TRANSDTL(bookDTLInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
        //        lstbookDTLInfo.Add(bookDTLInfo);

        //        APT += bookDTLInfo.LineTax;
        //    }
        //    // end datatable
        //    #endregion

        //        //save booking header

        //        bookHDRInfo.TransID = bookDTLInfo.TransID;

        //        ht.Add("TransID", bookHDRInfo.TransID);                               

        //        bookHDRInfo.TransType = 0;
        //        bookHDRInfo.AgentID = MyUserSet.AgentID;
        //        bookHDRInfo.AgentCatgID = MyUserSet.AgentCategoryID;
        //        bookHDRInfo.BookingDate = DateTime.Now;
        //        bookHDRInfo.STDDate = Convert.ToDateTime(temFlight.TemFlightStd);

        //        string expirySetting = objGeneral.getSysValueByKeyAndCarrierCode("STDEXPIRY", keyCarrier);                

        //        int sysValue = 0;
        //        if (expirySetting != "")
        //        {
        //            sysValue = Convert.ToInt16(expirySetting);
        //        }

        //        bookHDRInfo.ExpiryDate = departDate.Date.AddDays(-sysValue);

        //        ht.Add("Expiry", bookHDRInfo.ExpiryDate);

        //        bookHDRInfo.TransTotalPAX = Convert.ToInt16(lbl_num.Text);
        //        bookHDRInfo.CollectedAmt = 0; 

        //        bookHDRInfo.TransTotalAmt = totalFlightFare;
        //        bookHDRInfo.TransSubTotal = totalFlightFare;
        //        bookHDRInfo.TransTotalTax = APT;
        //        bookHDRInfo.TransTotalFee = totalFUEFare;
        //        bookHDRInfo.TransTotalOth = totalOth;                
        //        bookHDRInfo.Currency = Currency;
        //        bookHDRInfo.CurrencyPaid = Currency;

        //        bookHDRInfo.TransStatus = 0;
        //        bookHDRInfo.CreateBy = MyUserSet.AgentID;
        //        bookHDRInfo.SyncCreate = DateTime.Now;
        //        bookHDRInfo.SyncLastUpd = DateTime.Now;
        //        bookHDRInfo.LastSyncBy = MyUserSet.AgentID;
        //        bookHDRInfo.TotalAmtAVG = Convert.ToDecimal(lblAverageFare.Text);
        //        bookHDRInfo.TotalAmtGoing = Convert.ToDecimal(lbl_Total.Text);
        //        if (lbl_InTotal.Text != "")
        //        { bookHDRInfo.TotalAmtReturn = Convert.ToDecimal(lbl_InTotal.Text); }
        //        else
        //        { bookHDRInfo.TotalAmtReturn = 0; }


        //        string reminder = objGeneral.getSysValueByKeyAndCarrierCode("REMINDDURA1", keyCarrier);                

        //        if (reminder != "")
        //        {
        //            sysValue = Convert.ToInt16(reminder);
        //        }
        //        bookHDRInfo.CurReminderDate = bookHDRInfo.ExpiryDate.AddDays(-sysValue);

        //        reminder = objGeneral.getSysValueByKeyAndCarrierCode("REMINDDURA2", keyCarrier);                

        //        if (reminder != "")
        //        {
        //            sysValue = Convert.ToInt16(reminder);
        //        }
        //        bookHDRInfo.NextReminderDate = bookHDRInfo.ExpiryDate.AddDays(-sysValue);
        //        bookHDRInfo.ReminderType = 1;

        //        //load max failed payment try
        //        string maxPaymentFail = objGeneral.getSysValueByKeyAndCarrierCode("PAYMENTSUSPEND", keyCarrier);
        //        ht.Add("PaymentSuspend", maxPaymentFail);

        //        HttpContext.Current.Session.Remove("HashMain");
        //        HttpContext.Current.Session.Add("HashMain", ht);

        //        //end save header

        //        //save APTtax into TransTax

        //        /*    
        //        string tCode = "";
        //        decimal tAmount = 0;

        //        for (int i = 0; i < dtTaxFees.Rows.Count; i++)
        //        {
        //            if (dtTaxFees.Rows[i]["TaxFeesGroup"].ToString() == "APT")
        //            {
        //                tCode = dtTaxFees.Rows[i]["TaxFeesCode"].ToString();
        //                tAmount = Convert.ToDecimal(dtTaxFees.Rows[i]["TaxFeesRate"]);
        //            }
        //        }

        //        transTaxInfo.TransID = bookHDRInfo.TransID;

        //        transTaxInfo.TaxCode = Convert.ToInt16(tCode);
        //        transTaxInfo.TaxRate = tAmount;
        //        transTaxInfo.TaxAmt = APT;
        //        transTaxInfo.TransVoid = 0;
        //        transTaxInfo.SyncCreate = DateTime.Now;
        //        transTaxInfo.CreateBy = MyUserSet.AgentID;

        //        //save service charge, fuel charge and baggage charge into transFees

        //        transFeesInfo.TransID = bookHDRInfo.TransID;
        //        transFeesInfo.Transvoid = 0;
        //        transFeesInfo.SyncCreate = DateTime.Now;
        //        transFeesInfo.CreateBy = MyUserSet.AgentID;
        //        for (int i = 0; i < dtTaxFees.Rows.Count; i++)
        //        {
        //            if (dtTaxFees.Rows[i]["TaxFeesGroup"].ToString() != "APT")
        //            {
        //                transFeesInfo.FeesCode = Convert.ToInt16(dtTaxFees.Rows[i]["TaxFeesCode"]);
        //                transFeesInfo.FeesRate = Convert.ToDecimal(dtTaxFees.Rows[i]["TaxFeesRate"]);
        //                if (dtTaxFees.Rows[i]["TaxFeesGroup"].ToString() == "SVC")
        //                {
        //                    transFeesInfo.FeesAmt = TotalMoney * (Convert.ToDecimal(dtTaxFees.Rows[i]["TaxFeesRate"]) / 100);
        //                }
        //                else
        //                    if (dtTaxFees.Rows[i]["TaxFeesGroup"].ToString() == "FUE")
        //                    {
        //                        transFeesInfo.FeesAmt = totalFUEFare;
        //                    }
        //                    else
        //                        if (dtTaxFees.Rows[i]["TaxFeesGroup"].ToString() == "BGG")
        //                        {
        //                            transFeesInfo.FeesAmt = totalBaggageFare;
        //                        }

        //                //objBooking.SaveBK_TRANSFEES(transFeesInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
        //                lsttransFeesInfo.Add(transFeesInfo);
        //            }
        //        }
        //        */

        //        //end save tax and charge
        //        //added by ketee
        //        BookingTransactionMain BookingMain = new BookingTransactionMain();
        //        //BookingMain = objBooking.SaveBooking(bookHDRInfo, lstbookDTLInfo, transTaxInfo, lsttransFeesInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
        //        BookingMain = objBooking.SaveBooking(bookHDRInfo, lstbookDTLInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
        //        if (BookingMain != null && BookingMain.TransID != "")
        //            return true;
        //        else
        //            return false;
        //        //objBooking.SaveBooking(bookHDRInfo, lstbookDTLInfo, transTaxInfo, lsttransFeesInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
        //}
        #endregion

        protected void InitializeForm()
        {

            HttpCookie cookie3 = Request.Cookies["cookieTran"];
            if (cookie3 != null)
            {
                cookie3.Expires = DateTime.Today.AddDays(-1);
                Response.Cookies.Add(cookie3);
            }

            SetCookie();
            Bind();
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

        private void Bind()
        {
            if (departID != -1)
            {

                ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
                string strExpr;
                string strSort;
                DataTable dt = objBooking.dtFlight();
                strExpr = "TemFlightId = '" + departID + "'";
                strSort = "";
                dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                FillModelFromDataRow(foundRows, ref  temFlight);

                lbl_Arrival.Text = objGeneral.GetCityNameByCode(temFlight.TemFlightArrival) + "(" + temFlight.TemFlightArrival + ")";

                lbl_Average.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight.temFlightfarePrice)).ToString("N", nfi);

                //commented by diana 20131104
                //if (Convert.ToDecimal(temFlight.TemFlightCHDNum) != 0)
                //    lbl_Average.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight.temFlightfarePrice)/2).ToString("N", nfi);
                //else
                //    lbl_Average.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight.temFlightfarePrice)).ToString("N", nfi);

                DataTable dtBDFee = objBooking.dtBreakdownFee();
                decimal total = 0, totalApt = 0;
                if (ReturnOnly == false) //if return only, then hide depart
                {
                    tr_depart.Visible = true;
                    tr_depart2.Visible = true;

                    lbl_currency.Text = temFlight.TemFlightCurrencyCode.ToString();
                    lbl_currency0.Text = temFlight.TemFlightCurrencyCode.ToString();
                    lbl_currency1.Text = temFlight.TemFlightCurrencyCode.ToString();
                    lbl_currency2.Text = temFlight.TemFlightCurrencyCode.ToString();
                    lbl_currency3.Text = temFlight.TemFlightCurrencyCode.ToString();
                    if (Convert.ToDecimal(temFlight.TemFlightCHDNum) != 0) // && (temFlight.TemFlightDeparture.ToString().ToUpper() == "HKG" || temFlight.TemFlightArrival.ToString().ToUpper() == "HKG"))
                    {
                        lbl_currency2CHD.Text = temFlight.TemFlightCurrencyCode.ToString();
                        lbl_currency3CHD.Text = temFlight.TemFlightCurrencyCode.ToString();
                    }
                    lbl_currency4.Text = temFlight.TemFlightCurrencyCode.ToString();
                    lbl_currency5.Text = temFlight.TemFlightCurrencyCode.ToString();
                    lbl_currency6.Text = temFlight.TemFlightCurrencyCode.ToString();
                    lbl_currency7.Text = temFlight.TemFlightCurrencyCode.ToString();
                    lblCurrFuelDepart.Text = temFlight.TemFlightCurrencyCode.ToString();
                    lblCurrOthDepart.Text = temFlight.TemFlightCurrencyCode.ToString();
                    lblCurrSvcDepart.Text = temFlight.TemFlightCurrencyCode.ToString();

                    lbl_departure.Text = objGeneral.GetCityNameByCode(temFlight.TemFlightDeparture) + "(" + temFlight.TemFlightDeparture + ")";

                    totalApt = Convert.ToDecimal(num * temFlight.TemFlightApt);

                    lbl_FlightTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight.temFlightfarePrice) * num).ToString("N", nfi);

                    //commented by diana 20131104
                    //if (Convert.ToDecimal(temFlight.TemFlightCHDNum) != 0)
                    //    lbl_FlightTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight.temFlightfarePrice) * num / 2).ToString("N", nfi);
                    //else
                    //    lbl_FlightTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight.temFlightfarePrice) * num).ToString("N", nfi);

                    lbl_num.Text = num.ToString();
                    if (Convert.ToDecimal(temFlight.TemFlightCHDNum) != 0) // && (temFlight.TemFlightDeparture.ToString().ToUpper() == "HKG" || temFlight.TemFlightArrival.ToString().ToUpper() == "HKG"))
                    {
                        lbl_num2.Text = temFlight.TemFlightADTNum.ToString() + " Adult Airport Tax @ ";
                        lbl_num2CHD.Text = temFlight.TemFlightCHDNum.ToString();
                    }
                    else
                    {
                        lbl_num2.Text = num.ToString() + " Airport Tax @ ";
                    }
                    lbl_num3.Text = num.ToString();
                    lbl_num4.Text = num.ToString();
                    lbl_num5.Text = num.ToString();
                    lbl_num6.Text = num.ToString();
                    //lbl_numtax.Text = num.ToString();

                    //lbl_taxPrice.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight.TemFlightApt)).ToString("N", nfi);

                    //lbl_taxTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(num * temFlight.TemFlightApt)).ToString("N", nfi);
                    lbl_Total.Text = objGeneral.RoundUp(temFlight.TemFlightTotalAmount).ToString("N", nfi);
                    total = temFlight.TemFlightTotalAmount;

                    AVGFare = (objGeneral.RoundUp(total / temFlight.TemFlightPaxNum));

                    dtBDFee = (DataTable)HttpContext.Current.Session["dataBDFeeDepart"];

                    ///amended by Diana,
                    ///added divide with num of passenger to show single amount 

                    if (Convert.ToDecimal(temFlight.TemFlightCHDNum) != 0) // && (temFlight.TemFlightDeparture.ToString().ToUpper() == "HKG" || temFlight.TemFlightArrival.ToString().ToUpper() == "HKG"))
                    {
                        trAptCHD.Visible = true;
                        lbl_taxPrice.Text = Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]).ToString("N", nfi);
                        lbl_taxTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * Convert.ToDecimal(temFlight.TemFlightADTNum)).ToString("N", nfi);
                        lbl_taxPriceCHD.Text = Convert.ToDecimal(dtBDFee.Rows[0]["ChApt"]).ToString("N", nfi);
                        lbl_taxTotalCHD.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["ChApt"]) * Convert.ToDecimal(temFlight.TemFlightCHDNum)).ToString("N", nfi);
                    }
                    else
                    {
                        trAptCHD.Visible = false;
                        lbl_taxPrice.Text = Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]).ToString("N", nfi);
                        lbl_taxTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * num).ToString("N", nfi);
                    }

                    lblFuelPriceOneDepart.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"]) / num).ToString("N", nfi);
                    lblFuelPriceTotalDepart.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"])).ToString("N", nfi);

                    lblSvcChargeOneDepart.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Svc"]) / num).ToString("N", nfi);
                    lblSvcChargeTotalDepart.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Svc"])).ToString("N", nfi);

                    lblOthOneDepart.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) / num + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"]) / num).ToString("N", nfi);
                    lblOthTotalDepart.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"])).ToString("N", nfi);

                    //added by ketee
                    lblVATDepart.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) / num).ToString("N", nfi);
                    lblVATTotalDepart.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["VAT"])).ToString("N", nfi);
                    lblCurrVATDepart.Text = temFlight.TemFlightCurrencyCode.ToString();

                }
                else
                {
                    tr_depart.Visible = false;
                    tr_depart2.Visible = false;
                }
                
                if (ReturnID != "")
                {
                    tr_return.Visible = true;
                    tr_return2.Visible = true;

                    strExpr = "TemFlightId = '" + ReturnID + "'";
                    strSort = "";
                    dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                    foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                    FillModelFromDataRow(foundRows, ref  temFlight);

                    lbl_InArrival.Text = objGeneral.GetCityNameByCode(temFlight.TemFlightArrival) + "(" + temFlight.TemFlightArrival + ")";
                    lbl_InAverage.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight.temFlightfarePrice)).ToString("N", nfi);

                    //if (Convert.ToDecimal(temFlight.TemFlightCHDNum) != 0)
                    //    lbl_InAverage.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight.temFlightfarePrice) / 2).ToString("N", nfi);
                    //else
                    //    lbl_InAverage.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight.temFlightfarePrice)).ToString("N", nfi);

                    lbl_InCurrency.Text = temFlight.TemFlightCurrencyCode.ToString();
                    lbl_InCurrency0.Text = temFlight.TemFlightCurrencyCode.ToString();
                    lbl_Incurrency1.Text = temFlight.TemFlightCurrencyCode.ToString();
                    lbl_Incurrency2.Text = temFlight.TemFlightCurrencyCode.ToString();
                    lbl_InCurrency3.Text = temFlight.TemFlightCurrencyCode.ToString();
                    if (Convert.ToDecimal(temFlight.TemFlightCHDNum) != 0) // && (temFlight.TemFlightDeparture.ToString().ToUpper() == "HKG" || temFlight.TemFlightArrival.ToString().ToUpper() == "HKG"))
                    {
                        lbl_Incurrency2CHD.Text = temFlight.TemFlightCurrencyCode.ToString();
                        lbl_InCurrency3CHD.Text = temFlight.TemFlightCurrencyCode.ToString();
                    }
                    lbl_InCurrency4.Text = temFlight.TemFlightCurrencyCode.ToString();
                    lbl_InCurrency5.Text = temFlight.TemFlightCurrencyCode.ToString();
                    lbl_InCurrency6.Text = temFlight.TemFlightCurrencyCode.ToString();
                    lbl_InCurrency7.Text = temFlight.TemFlightCurrencyCode.ToString();
                    lblCurrFuelReturn.Text = temFlight.TemFlightCurrencyCode.ToString();
                    lblCurrOthReturn.Text = temFlight.TemFlightCurrencyCode.ToString();
                    lblCurrSvcReturn.Text = temFlight.TemFlightCurrencyCode.ToString();

                    lbl_InDeparture.Text = objGeneral.GetCityNameByCode(temFlight.TemFlightDeparture) + "(" + temFlight.TemFlightDeparture + ")";

                    totalApt = Convert.ToDecimal(num * temFlight.TemFlightApt);

                    lbl_InFlightTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight.temFlightfarePrice) * num).ToString("N", nfi);

                    //commented by diana 20131104
                    //if (Convert.ToDecimal(temFlight.TemFlightCHDNum) != 0)
                    //    lbl_InFlightTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight.temFlightfarePrice) * num / 2).ToString("N", nfi);
                    //else
                    //    lbl_InFlightTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight.temFlightfarePrice) * num).ToString("N", nfi);

                    lbl_InNum.Text = num.ToString();

                    if (Convert.ToDecimal(temFlight.TemFlightCHDNum) != 0) // && (temFlight.TemFlightDeparture.ToString().ToUpper() == "HKG" || temFlight.TemFlightArrival.ToString().ToUpper() == "HKG"))
                    {
                        lbl_InNum2.Text = temFlight.TemFlightADTNum.ToString() + " Adult Airport Tax @ ";
                        lbl_InNum2CHD.Text = temFlight.TemFlightCHDNum.ToString();
                    }
                    else
                    {
                        lbl_InNum2.Text = num.ToString() + " Airport Tax @ ";
                    }
                    lbl_InNum3.Text = num.ToString();
                    lbl_InNum4.Text = num.ToString();
                    lbl_InNum5.Text = num.ToString();
                    lbl_InNum6.Text = num.ToString();
                    //lbl_InNumTax.Text = num.ToString();

                    dtBDFee = (DataTable)HttpContext.Current.Session["dataBDFeeReturn"];
                    //lbl_IntaxPrice.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight.TemFlightApt)).ToString("N", nfi);
                    //lbl_IntaxTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(num * temFlight.TemFlightApt)).ToString("N", nfi);

                    if (Convert.ToDecimal(temFlight.TemFlightCHDNum) != 0) // && (temFlight.TemFlightDeparture.ToString().ToUpper() == "HKG" || temFlight.TemFlightArrival.ToString().ToUpper() == "HKG"))
                    {
                        trInAptCHD.Visible = true;
                        lbl_IntaxPrice.Text = Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]).ToString("N", nfi);
                        lbl_IntaxTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * Convert.ToDecimal(temFlight.TemFlightADTNum)).ToString("N", nfi);
                        lbl_IntaxPriceCHD.Text = Convert.ToDecimal(dtBDFee.Rows[0]["ChApt"]).ToString("N", nfi);
                        lbl_IntaxTotalCHD.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["ChApt"]) * Convert.ToDecimal(temFlight.TemFlightCHDNum)).ToString("N", nfi);
                    }
                    else
                    {
                        trInAptCHD.Visible = false;
                        lbl_IntaxPrice.Text = Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]).ToString("N", nfi);
                        lbl_IntaxTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * num).ToString("N", nfi);
                    }

                    ///amended by Diana,
                    ///added divide with num of passenger to show single amount

                    lblFuelOneReturn.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"]) / num).ToString("N", nfi);
                    lblFuelTotalReturn.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"])).ToString("N", nfi);

                    lblSvcOneReturn.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Svc"]) / num).ToString("N", nfi);
                    lblSvcTotalReturn.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Svc"])).ToString("N", nfi);

                    lblOthOneReturn.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) / num + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"]) / num).ToString("N", nfi);
                    lblOthTotalReturn.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"])).ToString("N", nfi);

                    //added by ketee
                    lblVATReturn.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) / num).ToString("N", nfi);
                    lblVATTotalReturn.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["VAT"])).ToString("N", nfi);
                    lblCurrVATReturn.Text = temFlight.TemFlightCurrencyCode.ToString();

                    lbl_InTotal.Text = objGeneral.RoundUp(temFlight.TemFlightTotalAmount).ToString("N", nfi);
                    total += temFlight.TemFlightTotalAmount;

                    AVGFare = objGeneral.RoundUp(total / temFlight.TemFlightPaxNum);
                }
                else
                {
                    tr_return.Visible = false;
                    tr_return2.Visible = false;
                }
                lbl_TotalAmount.Text = objGeneral.RoundUp(total).ToString("N", nfi);
                lbl_TotalCurrency.Text = temFlight.TemFlightCurrencyCode.ToString();
                lblAverageFare.Text = objGeneral.RoundUp(AVGFare).ToString("N", nfi);
                lblAverageCurrency.Text = lbl_TotalCurrency.Text;
            }
        }

        protected void btn_Next_Click(object sender, EventArgs e)
        {

        }

        //protected void btn_UpdateCurrency_Click(object sender, EventArgs e)
        //{

        //}

        //moved from reiewfare by Agus
        decimal totalServiceFee;
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

            tranID = DateTime.Now.ToString("yyyyMMddHHmmsss");

            #region newsavedetail
            //Datatable Process 

            //string PNR = book.BookingCommit(agent, temClass.TemClassPersonNumber, temClass.TemClassClassPrice.Value, temClass.TemClassSellSignature);

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
                totalServiceFee += bookDTLInfo.LineFee;

                bookDTLInfo.FareClass = dr["FareClass"].ToString();
                bookDTLInfo.PaxAdult = Convert.ToInt16(dr["AdultPax"].ToString());
                bookDTLInfo.PaxChild = Convert.ToInt16(dr["ChildPax"].ToString());
                bookDTLInfo.LineTotal = Convert.ToDecimal(dr["FullPrice"].ToString());
                bookDTLInfo.SyncLastUpd = DateTime.Now;
                bookDTLInfo.LastSyncBy = MyUserSet.AgentID;

                bookDTLInfo.FarePerPax = Convert.ToDecimal(dr["FarePrice"].ToString());
                //bookDTLInfo.FarePerPax = Convert.ToDecimal(dr["FarePrice"].ToString()) / Convert.ToDecimal(dr["Quantity"].ToString());

                bookDTLInfo.LineTax = Convert.ToDecimal(dr["TaxChrg"].ToString()) + Convert.ToDecimal(dr["FuelChrg"].ToString()); //apt + fuel

                //totalFlightFare += bookDTLInfo.LineTotal + bookDTLInfo.LineOth; //include service charge
                totalFlightFare += bookDTLInfo.LineTotal; //include service charge

                bookDTLInfo.TransVoid = 0;
                bookDTLInfo.CreateBy = MyUserSet.AgentID;
                bookDTLInfo.SyncCreate = DateTime.Now;

                bookDTLInfo.Transit = dr["TemClassTransit"].ToString();
                if (bookDTLInfo.Transit != "")
                {
                    bookDTLInfo.ArrivalDate2 = Convert.ToDateTime(dr["TemClassSta2"].ToString());
                    bookDTLInfo.DepatureDate2 = Convert.ToDateTime(dr["TemClassStd2"].ToString());
                }

                bookDTLInfo.CollectedAmount = 0;
                bookDTLInfo.Signature = dr["SellSignature"].ToString();

                // objBooking.SaveBK_TRANSDTL(bookDTLInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
                lstbookDTLInfo.Add(bookDTLInfo);

                APT += bookDTLInfo.LineTax;
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


            //bookHDRInfo.TransTotalPAX = Convert.ToInt16(.Text);
            //change to
            HttpCookie cookie = Request.Cookies["cookieTemFlight"];
            bookHDRInfo.TransTotalPAX = Convert.ToInt16(cookie.Values["PaxNum"].ToString());

            bookHDRInfo.CollectedAmt = 0;

            bookHDRInfo.TransTotalAmt = totalFlightFare;
            bookHDRInfo.TransSubTotal = totalFlightFare;
            bookHDRInfo.TransTotalTax = APT;
            bookHDRInfo.TransTotalFee = totalServiceFee;
            bookHDRInfo.TransTotalOth = totalOth;
            bookHDRInfo.TransTotalDisc = totalDisc;

            bookHDRInfo.Currency = Currency;
            bookHDRInfo.CurrencyPaid = Currency;

            bookHDRInfo.TransStatus = 0;
            bookHDRInfo.CreateBy = MyUserSet.AgentID;
            bookHDRInfo.SyncCreate = DateTime.Now;
            bookHDRInfo.SyncLastUpd = DateTime.Now;
            bookHDRInfo.LastSyncBy = MyUserSet.AgentName;

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
            int is2Way = Convert.ToInt32(Session["is2Way"].ToString());
            if (is2Way == 1)
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

            BookingMain = objBooking.SaveBooking(bookHDRInfo, lstbookDTLInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
            if (BookingMain != null && BookingMain.TransID != "")
                return true;
            else
                return false;
            //objBooking.SaveBooking(bookHDRInfo, lstbookDTLInfo, transTaxInfo, lsttransFeesInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
        }




    }
}