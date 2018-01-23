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
using System.Threading;
using System.Text;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Configuration;

using ABS.Logic.GroupBooking;
using ABS.GBS.Log;

namespace GroupBooking.Web
{
    public partial class ProceedPaymentChange : System.Web.UI.Page
    {
        #region declaration
        private XmlNode _providers;
        private List<LinkButton> lnkButtons = new List<LinkButton>();
        UserSet MyUserSet;
        decimal totalPayment = 0, totalDepart = 0, totalReturn = 0, totalAmount = 0;
        string currency = "";
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogControl log = new LogControl();
        SystemLog SystemLog = new SystemLog();

        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
        ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");

        BookingTransactionDetail bookDTLInfo = new BookingTransactionDetail();
        BookingTransactionMain bookHDRInfo = new BookingTransactionMain();
        BookingTransTender bookTransTenderInfo = new BookingTransTender();
        List<PassengerData> lstPassenger = new List<PassengerData>();

        ABS.GBS.eService.ProcessServiceSoapClient eWS = new ABS.GBS.eService.ProcessServiceSoapClient();
        List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
        ABS.Logic.GroupBooking.Agent.AgentProfile agent = new ABS.Logic.GroupBooking.Agent.AgentProfile();
        ABS.Logic.GroupBooking.Agent.AgentProfileControl objAgentProfile = new ABS.Logic.GroupBooking.Agent.AgentProfileControl();
        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
        List<BookingTransactionDetail> lstbookFlightInfo = new List<BookingTransactionDetail>();

        private static int num = 0;
        private static int departID = 0;
        private static string ReturnID = "";

        bool ReturnOnly = false;

        //added by ketee
        private string PayScheme;

        public enum PaymentType
        {
            CreditCard = 0,
            AG = 1,
            CreditShell = 2,
            DirectDebit = 3
        }

        public string[] cityValue;
        public string[] cityText;
        public string combinedCityValue = "";
        public string combinedCityText = "";
        public string contactState = "";

        private bool paymentSuccess = false;
        #endregion

        #region Event
        //protected void Page_LoadComplete(object sender, EventArgs e)
        //{
        //    rdlPNR.Enabled = true;
        //}
        //protected void Page_PreInit(object sender, EventArgs e)
        //{
        //    rdlPNR.Enabled = false;
        //}

        //protected void Page_LoadComplete(object sender, EventArgs e)
        //{
        //    rdlPNR.Enabled = true;
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["ReturnOnly"].ToString() == "true")
                {
                    ReturnOnly = true;
                }

                //rdlPNR.Enabled = false;

                //added by diana 20131210 - for exception and invalid booking message
                HttpContext.Current.Session["ExceptionMessage"] = "";
                HttpContext.Current.Session["InvalidBooking"] = "false";

                contactState = txtContactState.Text;
                
                rdlPNR.Style["visibility"] = "hidden";
                txtPayAmount.Style["visibility"] = "hidden";

                lblMinPay.Text = txtMinPay.Text;

                Response.Buffer = true;
                Response.ExpiresAbsolute = DateTime.Now.AddDays(-1d);
                Response.Expires = -1500;
                Response.CacheControl = "no-cache";

                if (Session["AgentSet"] != null)
                { MyUserSet = (UserSet)Session["AgentSet"]; }
                else
                { Response.Redirect(Shared.MySite.PublicPages.AgentLogin, false); }

                SessionContext sesscon = new SessionContext();

                if (!IsPostBack)
                {
                    InitializeForm();
                    ViewState["savedMinPay"] = null;

                    insertCityValue();

                }
                else
                {
                    tblPayment.Style["visibility"] = "visible";
                    rdlPNR.Style["visibility"] = "visible";
                    txtPayAmount.Style["visibility"] = "visible";
                }

                SetScreen();

                //SetContactVisibility();
                if (Session["reloadPage"].ToString() == "true" && lblErrorBottom.Text == "")
                {
                    lblErrorBottom.Visible = true;
                    MessageList msgList = new MessageList();
                    lblErrorBottom.Text = msgList.Err100020;
                    imgError.Visible = true;
                }

                if (!IsPostBack && Decimal.Parse(lblAmountDue.Text) <= 0)
                {
                    lblErrorBottom.Text = "PNR has been fully paid. Kindly click submit to save flight changes.";
                    imgError.Visible = true;
                    lblErrorBottom.Visible = true;
                }
                Session["reloadPage"] = "";
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this,ex);
            }

        }
        #endregion

        #region Control

        protected void TabControl_ActiveTabChanged(object sender, TabControlEventArgs e)
        {
            SelectState();
        }

        protected void rdlPNR_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkSelectedRadioPNR();
        }

        protected void btnProceedPayment_Click(object sender, EventArgs e)
        {
            try
            {
                if (hError.Value == "1")
                {
                    //msgcontrol.MessageDisplay("Please fill in mandatory fields");
                    return;
                }


                //added by diana 20140211, if no due amount then commit and return
                if (Decimal.Parse(lblAmountDue.Text) <= 0)
                {
                    ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");
                    string errMessage = "";
                    absNavitaire.BookingCommitChange(Session["RecordLocator"].ToString(), Session["SellSessionID"].ToString(), ref errMessage, "", false, false);
                    
                    paymentSuccess = true;
                    string TransID = hID.Value;
                    string PNR = Session["RecordLocator"].ToString();
                    List<BookingTransactionDetail> oldBookingDetail = new List<BookingTransactionDetail>();
                    List<BookingTransactionDetail> newBookingDetail = new List<BookingTransactionDetail>();

                    oldBookingDetail = objBooking.GetAllBK_TRANSDTLFilterByPNR(TransID, PNR);//retrieve previous transdetails

                    //amended by diana 20140422, change to UpdateAllBookingJourneyDetails
                    ListTransaction TransactionDetail = new ListTransaction();
                    TransactionDetail.TransID = TransID;
                    //TransactionDetail.TransStatus = (byte)drRow["TransStatus"];
                    TransactionDetail.AgentID = agent.AgentID;
                    TransactionDetail.AgentUserName = agent.Username;

                    List<BookingTransactionDetail> VoidPNRs = new List<BookingTransactionDetail>();
                    List<BookingTransactionDetail> ExpiredPNRs = new List<BookingTransactionDetail>();
                    objBooking.UpdateAllBookingJourneyDetails(TransactionDetail, agent.Username, agent.AgentID, ref VoidPNRs, ref ExpiredPNRs, true);

                    newBookingDetail = objBooking.GetAllBK_TRANSDTLFilterByPNR(TransID, PNR);//retrieve new transdetails
                    objBooking.CopyTransaction(TransID, PNR, oldBookingDetail, newBookingDetail, ReturnOnly);

                    ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                    string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), hID.Value.ToString(), "");

                    Response.Redirect(Shared.MySite.PublicPages.BookingDetail + "?k=" + hashkey + "&TransID=" + hID.Value.ToString(), false);
                    return;
                }

                string tabname = TabControl.ActiveTabPage.Name;
                //switch (tabname)
                //{
                //    case "TabCredit":
                //        //ProceedPayment("TabCredit");
                //        break;
                //    case "TabAG":


                //        break;
                //    case "TabCreditAccount":


                //        break;
                //    case "TabDirectDebit":

                //        break;
                //}

                MessageList msgList = new MessageList();

                //if (HttpContext.Current.Session["ErrorPayment"] != null)
                //{
                //    //payment credit card failed
                //    msgcontrol.MessageDisplay(msgList.Err100034);
                //}
                //else
                //{

                lblMinPay.Text = txtMinPay.Text;
                if (txtPayAmount.IsValid && txtPayAmount.Text != "" && Convert.ToDecimal(txtPayAmount.Text) > 0)
                {
                    if (txtMinPay.Text == "")
                        txtMinPay.Text = "0.00";
                    lblMinPay.Text = txtMinPay.Text;
                    if (Convert.ToDecimal(txtPayAmount.Text) < Convert.ToDecimal(txtMinPay.Text))
                    {
                        //msgcontrol.MessageDisplay(msgList.Err100040);
                        lblErrorBottom.Text = msgList.Err100040;
                        imgError.Visible = true;
                        return;
                    }
                    if (Convert.ToDecimal(txtDueAmount.Text) < Convert.ToDecimal(txtPayAmount.Text))
                    {
                        //msgcontrol.MessageDisplay(msgList.Err100044);
                        lblErrorBottom.Text = msgList.Err100044;
                        imgError.Visible = true;
                        return;
                    }

                    //if (tabname == "TabCredit")//Added By Agus
                    //{
                    //    if (cmbCardType.SelectedItem.Value.ToString() == "AX")
                    //    {
                    //        if (txtCVV2.Text.Trim().Length.ToString() != "4")
                    //        {
                    //            //msgcontrol.MessageDisplay(msgList.Err100042);
                    //            lblErrorBottom.Text = msgList.Err100042;
                    //            imgError.Visible = true;
                    //            return;
                    //        }
                    //    }
                    //    if (cmbCardType.SelectedItem.Value.ToString() == "MC" || cmbCardType.SelectedItem.Value.ToString() == "VI")
                    //    {
                    //        if (txtCVV2.Text.Trim().Length.ToString() != "3")
                    //        {
                    //            //msgcontrol.MessageDisplay(msgList.Err100043);
                    //            lblErrorBottom.Text = msgList.Err100043;
                    //            imgError.Visible = true;
                    //            return;
                    //        }
                    //    }
                    //}

                    try
                    {

                        string status = "";
                        Boolean PNRExist = false;
                        string PNR = "";
                        string[] s = rdlPNR.SelectedItem.Value.ToString().Split(new char[] { ',' });
                        if (s.Length > 1 && s[1].ToString() != "" && s[1].ToString().Trim().Length >= 6)
                        {
                            PNRExist = true;
                            PNR = s[1].ToString();
                        }
                        else
                        {
                            PNR = s[0].ToString();
                        }

                        status = MakePayment(tabname, PNR, PNRExist);

                        //added by ketee, format error msg
                        if (status.ToString().ToLower().Contains("not enough funds available"))
                        {
                            string str = "";
                            string b = "";
                            String[] arr = status.Split(' ');
                            foreach (string a in arr)
                            {
                                b = a;
                                double dec;
                                bool res = double.TryParse(a, out dec);
                                if (res)
                                {
                                    b = String.Format("{0:0.00}", dec);
                                }
                                if (str == "")
                                {
                                    str = b;
                                }
                                else
                                {
                                    str += " " + b;
                                }
                            }
                            status = str;
                        }

                        log.Info(this,"Payment Status : " + status);
                        if (status != "")
                        {
                            if (checkPaymentAttempt())
                            {
                                //msgcontrol.MessageDisplay(status);
                                lblErrorBottom.Visible = true;

                                //added by diana 20131210 - add condition if booking is invalid
                                if (HttpContext.Current.Session["InvalidBooking"].ToString() == "true")
                                    status = msgList.Err100045;

                                if (status == msgList.Err100028 && paymentSuccess == true)
                                {
                                    status = msgList.Err100047;
                                    log.Info(this,"Reload:" + status);
                                    Session["reloadPage"] = "true";
                                    Response.Redirect(Shared.MySite.PublicPages.Payment, false);
                                    return;
                                }

                                lblErrorBottom.Text = HttpContext.Current.Session["ExceptionMessage"] + status;
                                imgError.Visible = true;
                                log.Info(this,"Info show from checkPaymentAttempt - not clearing data");
                                InitializeForm(false);

                                //added by diana 20140109 - if cc is declined, cannot proceed payment again
                                if (status == msgList.Err100046 || status == msgList.Err100047)
                                {
                                    tblPayment.Visible = false;
                                    TabControl.Visible = false;
                                    divContact.Visible = false;
                                    divCreditCard.Visible = false;
                                    divAG.Visible = false;
                                    btnProceedPayment.Visible = false;
                                }
                            }
                            else
                            {

                                //failedPayment(msgList.Err100034);
                                //msgcontrol.MessageDisplay(msgList.Err100028);

                                lblErrorBottom.Visible = true;
                                if (lblAmountPaid.Text == "0.00")
                                    lblErrorBottom.Text = "You have entered invalid credit card for maximum times. Your booking has been cancelled. Please kindly re-book your flight.";
                                else
                                    lblErrorBottom.Text = "You have entered invalid credit card for maximum times. Please kindly review your booking and generate valid payment details.";
                                imgError.Visible = true;
                                InitializeForm(false);

                                tblPayment.Visible = false;
                                TabControl.Visible = false;
                                divContact.Visible = false;
                                divCreditCard.Visible = false;
                                divAG.Visible = false;
                                btnProceedPayment.Visible = false;

                                //txtPayAmount.Enabled = false;
                                //TabControl.Enabled = false;
                                //rdlPNR.Enabled = false;
                                //btnProceedPayment.Enabled = false;

                                //Response.Write("<script type='text/javascript'>window.alert('You have entered invalid credit card data for 3 times.\nYour booking has been cancelled. Please kindly re-book your flight.')</script>");
                                //Response.Redirect("~/public/agentmain.aspx");
                            }
                        }
                        else
                        {
                            log.Info(this,"Payment succeed.");
                            //msgcontrol.MessageDisplay(msgList.Err100020);
                            lblErrorBottom.Visible = true;
                            lblErrorBottom.Text = msgList.Err100020;
                            imgError.Visible = true;
                            InitializeForm(true);
                        }

                    }
                    catch (Exception ex)
                    {
                        SystemLog.Notifier.Notify(ex);
                        log.Error(this,ex);
                        lblErrorBottom.Visible = true;
                        lblErrorBottom.Text = msgList.Err100021;
                        imgError.Visible = true;
                        //msgcontrol.MessageDisplay(msgList.Err100021);
                    }
                    finally
                    {

                    }
                }
                else
                {
                    if (checkPaymentAttempt())
                    {
                        //msgcontrol.MessageDisplay(msgList.Err100026);
                        lblErrorBottom.Text = msgList.Err100026;
                        imgError.Visible = true;
                    }
                    else
                    {
                        failedPayment(msgList.Err100034);
                    }
                }
                //}
            }
            catch (ApplicationException ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this,ex);

            }


        }

        protected void cmbCountryAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            UIClass.SetComboStyle(ref cmbState, UIClass.EnumDefineStyle.State, "", cmbCountryAddress.Value.ToString());
        }

        //load to city
        //protected void cmbContactState_Callback(object source, DevExpress.Web.ASPxCallback.CallbackEventArgs e)
        //{
        //    UIClass.SetComboStyle(ref cmbState, UIClass.EnumDefineStyle.State, "", cmbCountryAddress.Value.ToString());
        //}

        //protected void FillCityCombo(string country)
        //{
        //    if (string.IsNullOrEmpty(country)) return;

        //    AccessDataSourceCities.SelectParameters[0].DefaultValue = country;
        //    CmbCity.DataBind();

        //    // Select the current country capital in the CmbCity
        //    DataTable table = ((DataView)AccessDataSourceCountry.Select(DataSourceSelectArguments.Empty)).Table;
        //    DataRow[] foundRows = table.Select(string.Format("Country = '{0}'", country));
        //    if (foundRows.Length > 0)
        //        CmbCity.Value = (string)foundRows[0]["Capital"];
        //}
        //end load to city

        protected void cmbContactCountryAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            UIClass.SetComboStyle(ref cmbContactState, UIClass.EnumDefineStyle.State, "", cmbContactCountryAddress.Value.ToString());
            txtContactCountryAddress.Text = cmbContactCountryAddress.Value.ToString();
            txtContactState.Text = cmbContactState.Value.ToString();
        }

        #region Credit Card Control
        protected void cmbCardType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cmbCardType.SelectedItem.Text == "BIG Visa Card")
            //{
            //    panelProcessFee.Visible = false;
            //}
            //else
            //{
            //    panelProcessFee.Visible = true;
            //}

            //CheckProcessingFee();
            //if (cmbCardType.SelectedItem.Value.ToString() == "AX")
            //{
            //    txtCVV2.MaxLength = 4;
            //    txtCVV2.Text = "";
            //}
            //else
            //{
            //    txtCVV2.MaxLength = 3;
            //    txtCVV2.Text = "";
            //}


        }

        #endregion

        #endregion

        #region Function and Procedure

        #region General Function

        protected void checkSelectedRadioPNR()
        {
            if (rdlPNR.SelectedIndex >= 0)
            {
                Boolean PNRExist = false;
                string PNR = "";
                //log.Info(this,"PNR radio selection is " + rdlPNR.SelectedItem.Value.ToString());

                string[] s = rdlPNR.SelectedItem.Value.ToString().Split(new char[] { ',' });
                if (s.Length > 1 && s[1].ToString() != "")
                {
                    PNRExist = true;
                    PNR = s[1].ToString();
                }
                else
                {
                    PNR = s[0].ToString();
                }
                txtMinPay.Text = SetMinAmount(hID.Value, PNR, PNRExist).ToString("N", nfi);
                lblMinPay.Text = lblMinPay.Text;

                if (Convert.ToDecimal(txtMinPay.Text) <= 0)
                {
                    lblMinCap.Text = "No Min. Payment is Required";
                    lblMinPay.Text = "";
                    txtMinPay.Text = "";
                }
                //txtPayAmount.Text = rdlPNR.SelectedItem.Text;
                txtPayAmount.Text = SetMinAmount(hID.Value, PNR, PNRExist, true).ToString("N", nfi).Replace(",", "");
            }
        }

        public void InitializeForm(Boolean Validate = false)
        {
            HttpContext.Current.Session.Remove("TotalProcessFee");
            HttpContext.Current.Session.Remove("MemberData");

            // commented by diana 20130919
            //HttpContext.Current.Session.Remove("PaymentAttempt");
            //HttpContext.Current.Session.Remove("PaymentMaxAttempt");
            // end commented by diana 20130919
            DataTable dtTransMain = objBooking.dtTransMain();

            //added by ketee
            rdlPNR.SelectedIndex = -1;
            rdlContactPNR.SelectedIndex = -1;

            lblMinPayCurrency.Text = "";
            lblMinCap.Text = "Min. Payment: ";
            lblMinPay.Text = "0.00";
            txtMinPay.Text = "0.00";
            txtDueAmount.Text = "0.00";
            txtPayAmount.Text = "";
            lblAmountPaidCurrency.Text = "";

            //if (cmbCardType.SelectedItem.Value.ToString() == "AX")
            //{
            //    txtCVV2.MaxLength = 4;
            //}
            //else
            //{
            //    txtCVV2.MaxLength = 3;
            //}
            if (Validate)
            {
                ResetCreditCardDetails();
            }

            if (HttpContext.Current.Session["ErrorPayment"] != null)
            {
                lblErrorTop.Visible = false;
                lblErrorBottom.Visible = false;
            }

            HttpCookie cookie;

            if (Request.Cookies["cookieTemFlight"] != null)
            {
                cookie = Request.Cookies["cookieTemFlight"];
                cookie.HttpOnly = true;
            }
            else
                cookie = null;

            if (cookie != null)
            {
                if (Int32.TryParse(cookie.Values["list1ID"], out departID) == false)
                {
                    departID = -1;
                }

                ReturnID = cookie.Values["ReturnID"];
                num = Convert.ToInt32(cookie.Values["PaxNum"]);
            }

            if (HttpContext.Current.Session["HashMain"] != null)
            {
                Hashtable ht = (Hashtable)HttpContext.Current.Session["HashMain"];
                hID.Value = ht["TransID"].ToString();
            }
            if (HttpContext.Current.Session["TransMain"] != null)
            {
                dtTransMain = (DataTable)HttpContext.Current.Session["TransMain"];
                hID.Value = dtTransMain.Rows[0]["TransID"].ToString();
            }
            //get latest transmain info
            if (hID.Value != "")
            {
                //LoadData(hID.Value);
                //LoadRightPanel(hID.Value);
                //update transaction details

                //commented by diana 20140210
                //objBooking.UpdatePaymentDetails(hID.Value, MyUserSet.AgentName, MyUserSet.AgentID);
                //LoadData(hID.Value);

                //added by diana 20140422, for reload if error happen, need to be checked
                if (Session["reloadPage"] != null)
                {
                    if (Session["reloadPage"].ToString() == "true")
                    {
                        objBooking.CheckInvalidPNRByTransID(hID.Value, true);
                        //add param value true - in testing
                        objBooking.UpdatePaymentDetails(hID.Value, MyUserSet.AgentName, MyUserSet.AgentID, true);
                    }
                }
                else
                {
                    Session["reloadPage"] = "";
                }


                if (Session["reloadPage"].ToString() == "true")
                {
                    sendEmail(hID.Value);
                }
                else
                {
                    //commented, need to be checked
                    //objBooking.UpdatePaymentDetails(hID.Value, MyUserSet.AgentName, MyUserSet.AgentID);
                }
                //end added by diana 20140422, for reload if error happen

                LoadRightPanel(hID.Value);
            }
            if (ReturnID != "")
            {
            }
            else
            {
                trDepart.Style["display"] = "none";
                trReturn.Style["display"] = "none";
            }
            if (HttpContext.Current.Session["TransMain"] != null)
            {
                dtTransMain = (DataTable)HttpContext.Current.Session["TransMain"];
                totalDepart = Convert.ToDecimal(dtTransMain.Rows[0]["TotalDepart"]);
                totalReturn = Convert.ToDecimal(dtTransMain.Rows[0]["TotalReturn"]);
                currency = dtTransMain.Rows[0]["Currency"].ToString();
                totalAmount = totalDepart + totalReturn;
                lblCurrentTotal.Text = totalAmount.ToString("N", nfi);
                lblCurrentTotalCurrency.Text = currency;

                //amended by diana 20140422, get total paid from PNR
                decimal TotalAmountPaid = Convert.ToDecimal(objBooking.GetTotalCollectedAmount(Session["RecordLocator"].ToString(),ReturnOnly));
                decimal TotalAmountDue = totalAmount - TotalAmountPaid;
                lblAmountPaid.Text = objGeneral.RoundUp(TotalAmountPaid).ToString("N", nfi);
                lblAmountDue.Text = objGeneral.RoundUp(TotalAmountDue).ToString("N", nfi);

                //lblAmountPaid.Text = objGeneral.RoundUp(Convert.ToDecimal(dtTransMain.Rows[0]["CollectedAmt"])).ToString("N", nfi);
                //lblAmountDue.Text = objGeneral.RoundUp(Convert.ToDecimal(dtTransMain.Rows[0]["TotalDue"])).ToString("N", nfi);// +" " + dtTransMain.Rows[0]["Currency"].ToString();
                
                ////lblAGCreditCurrency.Text = currency;
                lblAmountPaidCurrency.Text = dtTransMain.Rows[0]["CurrencyPaid"].ToString();
                lblAmountDueCurrency.Text = currency;
                lblDepartTotal.Text = totalDepart.ToString("N", nfi);
                lblDepartTotalCurrency.Text = currency;
                lblReturnTotal.Text = totalReturn.ToString("N", nfi);
                lblReturnTotalCurrency.Text = currency;

                lblTransactionID.Text = dtTransMain.Rows[0]["TransID"].ToString();

                //added by diana 20140211
                
                //if (!IsPostBack)
                //{
                //    if (Decimal.Parse(lblAmountDue.Text) <= 0)
                //    {
                //        ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");
                //        string errMessage = "";
                //        absNavitaire.BookingCommitChange(Session["RecordLocator"].ToString(), Session["SellSessionID"].ToString(), ref errMessage, "", false, false);
                //    }
                //}

                if (!IsPostBack && Decimal.Parse(lblAmountDue.Text) <= 0)
                {
                    tblPayment.Visible = false;
                    TabControl.Visible = false;
                    divContact.Visible = false;
                    divCreditCard.Visible = false;
                    divAG.Visible = false;

                    lblErrorBottom.Text = "PNR has been fully paid. Kindly click submit to save flight changes.";
                    imgError.Visible = true;
                    lblErrorBottom.Visible = true;
                }
                else
                {
                    GeneratePNRsPaymentDetails(Validate);
                }
                //set merchant payment gateway
                //SetPaymentGateWay(currency);
            }

            //added by diana 20131116 - if status = 1 then hide contact details
            //SetContactVisibility();
            //end added by diana 20131116 - if status = 1 then hide contact details

        }

        protected void LoadData(string TransID)
        {
            List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
            List<BookingTransactionDetail> listDetailCombinePNR = new List<BookingTransactionDetail>();

            bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(TransID);
            objBooking.FillDataTableTransMain(bookHDRInfo);

            //amended by diana 20131102 - hide disapproved record locator
            if (Session["NewBooking"].ToString() == "true")
                listDetailCombinePNR = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 0);
            else
                listDetailCombinePNR = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 0, "LEN(RecordLocator) >= 6 AND ");

            objBooking.FillDataTableTransDetail(listDetailCombinePNR);
        }

        private void GeneratePNRsPaymentDetails(Boolean Validate = false)
        {
            DataTable dataClass = objBooking.dtClass();
            //clear list PNR
            rdlPNR.Items.Clear();
            rdlContactPNR.Items.Clear();

            //added by diana 20131118 - to get agent profile from db
            agent = objAgentProfile.GetSingleAgentProfile(MyUserSet.AgentName);

            Boolean AllPNRExist = true;

            if (HttpContext.Current.Session["TransDetail"] != null)
            {
                DataTable dtDetail = objBooking.dtTransDetail();
                dtDetail = (DataTable)HttpContext.Current.Session["TransDetail"];

                for (int i = 0; i < dtDetail.Rows.Count; i++)
                {
                    if (dtDetail.Rows[i]["RecordLocator"].ToString().Trim().Length < 6)
                    {
                        AllPNRExist = false;
                        break;
                    }
                }
            }
            else
            {
                AllPNRExist = false;
            }


            if (HttpContext.Current.Session["TempFlight"] != null && AllPNRExist == false)
            {
                if (HttpContext.Current.Session["dataClassTrans"] != null)
                {
                    dataClass = (DataTable)HttpContext.Current.Session["dataClassTrans"];
                    decimal AmountPaid = 0;
                    decimal TotalAmount = 0;
                    string param = ",";
                    Boolean EmptyPNR = false;

                    //added by diana 20131106 - check whether any transdtl still do not have PNR
                    Boolean leftEmptyPNR = false;

                    //cityValue = new string[dataClass.Rows.Count];
                    //cityText = new string[dataClass.Rows.Count];

                    for (int i = 0; i < dataClass.Rows.Count; i++)
                    {
                        string PNR = "To Be Confirmed";
                        decimal AmountDue = Convert.ToDecimal(dataClass.Rows[i]["FullPrice"].ToString()) - Convert.ToDecimal(dataClass.Rows[i]["DetailCollectedAmt"].ToString());
                        log.Info(this,"PNR : " + dataClass.Rows[i]["RecordLocator"].ToString());
                        if (dataClass.Rows[i]["RecordLocator"].ToString() != "")
                        {
                            EmptyPNR = false;
                            //if (dataClass.Rows[i]["RecordLocator"].ToString() != dataClass.Rows[i]["SellSignature"].ToString())
                            //{
                            PNR = dataClass.Rows[i]["RecordLocator"].ToString();
                            param = "," + PNR;
                            //}
                            //else
                            //{
                            //    PNR = "Declined";
                            //}
                        }
                        else
                        {
                            leftEmptyPNR = true;
                            EmptyPNR = true;
                        }

                        decimal minPay;

                        if (EmptyPNR)
                        {
                            minPay = Convert.ToDecimal(SetMinAmount(hID.Value, dataClass.Rows[i]["SellSignature"].ToString()));
                        }
                        else
                        {
                            //if (dataClass.Rows[i]["RecordLocator"].ToString() == dataClass.Rows[i]["RecordLocator"].ToString())
                            //{
                            //    minPay = 0;
                            //}
                            //else
                            //{
                            bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(hID.Value);
                            objBooking.FillDataTableTransMain(bookHDRInfo);

                            //amended by diana 20131102 - hide disapproved record locator
                            if (Session["NewBooking"].ToString() == "true")
                                lstbookFlightInfo = objBooking.GetAllBK_TRANSDTLCombinePNR(hID.Value);
                            else
                                lstbookFlightInfo = objBooking.GetAllBK_TRANSDTLCombinePNR(hID.Value, 0, "LEN(RecordLocator) >= 6 AND ");
                            //log.Info(this, "Session New Booking : " + Session["NewBooking"].ToString());
                            objBooking.FillDataTableTransDetail(lstbookFlightInfo);

                            minPay = Convert.ToDecimal(SetMinAmount(hID.Value, dataClass.Rows[i]["RecordLocator"].ToString(), true));
                            //}
                        }

                        //added by diana 20131118 - set contact session
                        if (HttpContext.Current.Session["Title_" + PNR] == null) HttpContext.Current.Session.Add("Title_" + PNR, agent.Title); else if (HttpContext.Current.Session["Title_" + PNR].ToString() == "") HttpContext.Current.Session["Title_" + PNR] = agent.Title;
                        if (HttpContext.Current.Session["FirstName_" + PNR] == null) HttpContext.Current.Session.Add("FirstName_" + PNR, agent.ContactFirstName); else if (HttpContext.Current.Session["FirstName_" + PNR].ToString() == "") HttpContext.Current.Session["FirstName_" + PNR] = agent.ContactFirstName;
                        if (HttpContext.Current.Session["LastName_" + PNR] == null) HttpContext.Current.Session.Add("LastName_" + PNR, agent.ContactLastName); else if (HttpContext.Current.Session["LastName_" + PNR].ToString() == "") HttpContext.Current.Session["LastName_" + PNR] = agent.ContactLastName;
                        if (HttpContext.Current.Session["Email_" + PNR] == null) HttpContext.Current.Session.Add("Email_" + PNR, agent.Email); else if (HttpContext.Current.Session["Email_" + PNR].ToString() == "") HttpContext.Current.Session["Email_" + PNR] = agent.Email;
                        if (HttpContext.Current.Session["PhoneNo_" + PNR] == null) HttpContext.Current.Session.Add("PhoneNo_" + PNR, agent.PhoneNo); else if (HttpContext.Current.Session["PhoneNo_" + PNR].ToString() == "") HttpContext.Current.Session["PhoneNo_" + PNR] = agent.PhoneNo;
                        if (HttpContext.Current.Session["Address_" + PNR] == null) HttpContext.Current.Session.Add("Address_" + PNR, agent.Address1); else if (HttpContext.Current.Session["Address_" + PNR].ToString() == "") HttpContext.Current.Session["Address_" + PNR] = agent.Address1;
                        if (HttpContext.Current.Session["Town_" + PNR] == null) HttpContext.Current.Session.Add("Town_" + PNR, agent.City); else if (HttpContext.Current.Session["Town_" + PNR].ToString() == "") HttpContext.Current.Session["Town_" + PNR] = agent.City;
                        if (HttpContext.Current.Session["Country_" + PNR] == null) HttpContext.Current.Session.Add("Country_" + PNR, agent.Country); else if (HttpContext.Current.Session["Country_" + PNR].ToString() == "") HttpContext.Current.Session["Country_" + PNR] = agent.Country;
                        if (HttpContext.Current.Session["State_" + PNR] == null) HttpContext.Current.Session.Add("State_" + PNR, agent.State); else if (HttpContext.Current.Session["State_" + PNR].ToString() == "") HttpContext.Current.Session["State_" + PNR] = agent.State;
                        if (HttpContext.Current.Session["ZipCode_" + PNR] == null) HttpContext.Current.Session.Add("ZipCode_" + PNR, agent.Postcode); else if (HttpContext.Current.Session["ZipCode_" + PNR].ToString() == "") HttpContext.Current.Session["ZipCode_" + PNR] = agent.Postcode;
                        //added by diana 20131118 - set contact session

                        //insertCityValue();

                        if (minPay > 0)
                        {
                            rdlPNR.Items.Add("PNR:" + PNR + " (total passenger:" + dataClass.Rows[i]["Quantity"].ToString() + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")", dataClass.Rows[i]["SellSignature"].ToString() + param);
                            rdlContactPNR.Items.Add("Title: " + HttpContext.Current.Session["Title_" + PNR] + ";FirstName: " + HttpContext.Current.Session["FirstName_" + PNR] + ";LastName: " + HttpContext.Current.Session["LastName_" + PNR] + ";Email: " + HttpContext.Current.Session["Email_" + PNR] + ";PhoneNo: " + HttpContext.Current.Session["PhoneNo_" + PNR] + ";Address: " + HttpContext.Current.Session["Address_" + PNR] + ";Town: " + HttpContext.Current.Session["Town_" + PNR] + ";Country: " + HttpContext.Current.Session["Country_" + PNR] + ";State: " + HttpContext.Current.Session["State_" + PNR] + ";ZipCode: " + HttpContext.Current.Session["ZipCode_" + PNR]);
                        }
                        else
                        {
                            rdlPNR.Items.Add("PNR:" + PNR + " (total passenger:" + dataClass.Rows[i]["Quantity"].ToString() + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  No Min. Payment is Required", dataClass.Rows[i]["SellSignature"].ToString() + param);
                            rdlContactPNR.Items.Add("Title: " + HttpContext.Current.Session["Title_" + PNR] + ";FirstName: " + HttpContext.Current.Session["FirstName_" + PNR] + ";LastName: " + HttpContext.Current.Session["LastName_" + PNR] + ";Email: " + HttpContext.Current.Session["Email_" + PNR] + ";PhoneNo: " + HttpContext.Current.Session["PhoneNo_" + PNR] + ";Address: " + HttpContext.Current.Session["Address_" + PNR] + ";Town: " + HttpContext.Current.Session["Town_" + PNR] + ";Country: " + HttpContext.Current.Session["Country_" + PNR] + ";State: " + HttpContext.Current.Session["State_" + PNR] + ";ZipCode: " + HttpContext.Current.Session["ZipCode_" + PNR]);
                        }
                        AmountPaid += Convert.ToDecimal(dataClass.Rows[i]["DetailCollectedAmt"].ToString());
                        TotalAmount += Convert.ToDecimal(dataClass.Rows[i]["FullPrice"].ToString());
                        param = ",";
                    }

                    //combinedCityValue = "[" + String.Join(",", cityValue) + "]";
                    //combinedCityText = "[" + String.Join(",", cityText) + "]";

                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "varCity", "<script type='text/javascript'>var city_value = " + combinedCityValue + "; var city_text = " + combinedCityText + ";</script>");

                    //lblAmountPaid.Text = AmountPaid.ToString("N", nfi);
                    //lblAmountDue.Text = (totalAmount - AmountPaid).ToString("N", nfi);
                    if (TotalAmount == AmountPaid && IsPostBack)
                    {
                        LoadData(hID.Value);

                        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                        string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), hID.Value.ToString(), "");

                        Response.Redirect(Shared.MySite.PublicPages.BookingDetail + "?k=" + hashkey + "&TransID=" + hID.Value.ToString(), false);
                    }

                    if ((leftEmptyPNR && Validate) || (leftEmptyPNR && Session["reloadPage"].ToString() == "true"))
                    {
                        //msgcontrol.MessageDisplay("Please take note that some PNR still not yet be confirmed, \n please select and pay at least minimum payment in order to secure your booking.\n Otherwise the unconfirmed booking will be expired.");
                        lblErrorBottom.Text = "Please take note that some PNR still not yet be confirmed, please select and pay at least minimum payment in order to secure your booking. Otherwise the unconfirmed booking will be expired.";
                        imgError.Visible = true;
                        lblErrorBottom.Visible = true;
                    }
                    else if (Session["reloadPage"].ToString() == "true")
                    {
                        lblErrorBottom.Visible = true;
                        MessageList msgList = new MessageList();
                        lblErrorBottom.Text = msgList.Err100020;
                        imgError.Visible = true;
                    }
                }
            }
            else
            {
                if (HttpContext.Current.Session["TransMain"] != null)
                {
                    DataTable dtTransMain = objBooking.dtTransMain();
                    dtTransMain = (DataTable)HttpContext.Current.Session["TransMain"];

                    DataTable dtTransDetail = objBooking.dtTransDetail();
                    dtTransDetail = (DataTable)HttpContext.Current.Session["TransDetail"];

                    //cityValue = new string[dtTransDetail.Rows.Count];
                    //cityText = new string[dtTransDetail.Rows.Count];

                    for (int i = 0; i < dtTransDetail.Rows.Count; i++)
                    {

                        //added by diana 20131118 - set contact session
                        string PNR = dtTransDetail.Rows[i]["RecordLocator"].ToString();
                        if (HttpContext.Current.Session["Title_" + PNR] == null) HttpContext.Current.Session.Add("Title_" + PNR, agent.Title); else if (HttpContext.Current.Session["Title_" + PNR].ToString() == "") HttpContext.Current.Session["Title_" + PNR] = agent.Title;
                        if (HttpContext.Current.Session["FirstName_" + PNR] == null) HttpContext.Current.Session.Add("FirstName_" + PNR, agent.ContactFirstName); else if (HttpContext.Current.Session["FirstName_" + PNR].ToString() == "") HttpContext.Current.Session["FirstName_" + PNR] = agent.ContactFirstName;
                        if (HttpContext.Current.Session["LastName_" + PNR] == null) HttpContext.Current.Session.Add("LastName_" + PNR, agent.ContactLastName); else if (HttpContext.Current.Session["LastName_" + PNR].ToString() == "") HttpContext.Current.Session["LastName_" + PNR] = agent.ContactLastName;
                        if (HttpContext.Current.Session["Email_" + PNR] == null) HttpContext.Current.Session.Add("Email_" + PNR, agent.Email); else if (HttpContext.Current.Session["Email_" + PNR].ToString() == "") HttpContext.Current.Session["Email_" + PNR] = agent.Email;
                        if (HttpContext.Current.Session["PhoneNo_" + PNR] == null) HttpContext.Current.Session.Add("PhoneNo_" + PNR, agent.PhoneNo); else if (HttpContext.Current.Session["PhoneNo_" + PNR].ToString() == "") HttpContext.Current.Session["PhoneNo_" + PNR] = agent.PhoneNo;
                        if (HttpContext.Current.Session["Address_" + PNR] == null) HttpContext.Current.Session.Add("Address_" + PNR, agent.Address1); else if (HttpContext.Current.Session["Address_" + PNR].ToString() == "") HttpContext.Current.Session["Address_" + PNR] = agent.Address1;
                        if (HttpContext.Current.Session["Town_" + PNR] == null) HttpContext.Current.Session.Add("Town_" + PNR, agent.City); else if (HttpContext.Current.Session["Town_" + PNR].ToString() == "") HttpContext.Current.Session["Town_" + PNR] = agent.City;
                        if (HttpContext.Current.Session["Country_" + PNR] == null) HttpContext.Current.Session.Add("Country_" + PNR, agent.Country); else if (HttpContext.Current.Session["Country_" + PNR].ToString() == "") HttpContext.Current.Session["Country_" + PNR] = agent.Country;
                        if (HttpContext.Current.Session["State_" + PNR] == null) HttpContext.Current.Session.Add("State_" + PNR, agent.State); else if (HttpContext.Current.Session["State_" + PNR].ToString() == "") HttpContext.Current.Session["State_" + PNR] = agent.State;
                        if (HttpContext.Current.Session["ZipCode_" + PNR] == null) HttpContext.Current.Session.Add("ZipCode_" + PNR, agent.Postcode); else if (HttpContext.Current.Session["ZipCode_" + PNR].ToString() == "") HttpContext.Current.Session["ZipCode_" + PNR] = agent.Postcode;
                        //added by diana 20131118 - set contact session



                        decimal AmountDue = Convert.ToDecimal(dtTransDetail.Rows[i]["AmountDue"].ToString());
                        decimal minPay = Convert.ToDecimal(SetMinAmount(hID.Value, dtTransDetail.Rows[i]["RecordLocator"].ToString(), true));
                        int Pax = Convert.ToInt32(dtTransDetail.Rows[i]["PaxAdult"].ToString()) + Convert.ToInt32(dtTransDetail.Rows[i]["PaxChild"].ToString());
                        if (minPay > 0)
                        {
                            string actualstatus = "";
                            if (dtTransMain.Rows.Count > 0) { actualstatus = dtTransMain.Rows[0]["TransStatus"].ToString(); }
                            if (actualstatus == "3") //amended by diana 20140108 - check for actual status - hide expiry date is actual status is confirmed
                            {
                                rdlPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")", dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                            }
                            else
                            {
                                rdlPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")", dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                            }
                            rdlContactPNR.Items.Add("Title: " + HttpContext.Current.Session["Title_" + PNR] + ";FirstName: " + HttpContext.Current.Session["FirstName_" + PNR] + ";LastName: " + HttpContext.Current.Session["LastName_" + PNR] + ";Email: " + HttpContext.Current.Session["Email_" + PNR] + ";PhoneNo: " + HttpContext.Current.Session["PhoneNo_" + PNR] + ";Address: " + HttpContext.Current.Session["Address_" + PNR] + ";Town: " + HttpContext.Current.Session["Town_" + PNR] + ";Country: " + HttpContext.Current.Session["Country_" + PNR] + ";State: " + HttpContext.Current.Session["State_" + PNR] + ";ZipCode: " + HttpContext.Current.Session["ZipCode_" + PNR]);
                        }
                        else
                        {
                            rdlPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  No Min. Payment is Required", dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                            rdlContactPNR.Items.Add("Title: " + HttpContext.Current.Session["Title_" + PNR] + ";FirstName: " + HttpContext.Current.Session["FirstName_" + PNR] + ";LastName: " + HttpContext.Current.Session["LastName_" + PNR] + ";Email: " + HttpContext.Current.Session["Email_" + PNR] + ";PhoneNo: " + HttpContext.Current.Session["PhoneNo_" + PNR] + ";Address: " + HttpContext.Current.Session["Address_" + PNR] + ";Town: " + HttpContext.Current.Session["Town_" + PNR] + ";Country: " + HttpContext.Current.Session["Country_" + PNR] + ";State: " + HttpContext.Current.Session["State_" + PNR] + ";ZipCode: " + HttpContext.Current.Session["ZipCode_" + PNR]);
                        }
                    }

                    //combinedCityValue = "[" + String.Join(",", cityValue) + "]";
                    //combinedCityText = "[" + String.Join(",", cityText) + "]";

                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "varCity", "<script type='text/javascript'>var city_value = " + combinedCityValue + "; var city_text = " + combinedCityText + ";</script>");

                    if (IsPostBack && Convert.ToDecimal(dtTransMain.Rows[0]["CollectedAmt"].ToString()) == Convert.ToDecimal(dtTransMain.Rows[0]["TotalTrans"].ToString()))
                    {
                        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                        string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), hID.Value.ToString(), "");

                        Response.Redirect(Shared.MySite.PublicPages.BookingDetail + "?k=" + hashkey + "&TransID=" + hID.Value.ToString(), false);
                    }
                }
            }
        }

        private void insertCityValue()
        {

            ABS.Logic.GroupBooking.GeneralControl CountryBase = new ABS.Logic.GroupBooking.GeneralControl();
            List<Country_Info> listCountry = new List<Country_Info>();
            listCountry = CountryBase.GetAllCountry();

            int i = 0;

            cityValue = new string[listCountry.Count];
            cityText = new string[listCountry.Count];

            foreach (Country_Info countryItem in listCountry)
            {
                cityValue[i] = "[";
                cityText[i] = "[";
                List<Country_Info> listCity = new List<Country_Info>();
                listCity = CountryBase.GetAllState(countryItem.countrycode);
                int countIndex = 0;

                foreach (Country_Info cityItem in listCity)
                {
                    if (countIndex > 0)
                    {
                        cityValue[i] += ",";
                        cityText[i] += ",";
                    }
                    cityValue[i] += "'" + cityItem.provincestatecode + "'";
                    cityText[i] += "'" + cityItem.provinceStateName + "'";

                    countIndex += 1;
                }
                cityValue[i] += "]";
                cityText[i] += "]";

                i += 1;
            }


            combinedCityValue = "[" + String.Join(",", cityValue) + "]";
            combinedCityText = "[" + String.Join(",", cityText) + "]";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "varCity", "<script type='text/javascript'>var city_value = " + combinedCityValue + "; var city_text = " + combinedCityText + ";</script>");


        }
        private void SetPayScheme()
        {
            if (HttpContext.Current.Session["TempFlight"] != null)
            {
                Hashtable ht = (Hashtable)HttpContext.Current.Session["HashMain"];
                bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(ht["TransID"].ToString());
                PayScheme = bookHDRInfo.PayScheme;
            }
            else if (HttpContext.Current.Session["TransMain"] != null)
            {
                DataTable dtTransMain = objBooking.dtTransMain();
                dtTransMain = (DataTable)HttpContext.Current.Session["TransMain"];
                PayScheme = dtTransMain.Rows[0]["SchemeCode"].ToString();
            }
        }

        private string GetXMLString(object Obj)
        {
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Obj.GetType());
            System.IO.StringWriter writer = new System.IO.StringWriter();
            x.Serialize(writer, Obj);

            return writer.ToString();
        }

        private Decimal SetMinAmount(string TransID, string SessionID, Boolean PNRExist = false, Boolean NonZeroAmount = false)
        {
            PaymentControl objPayment = new PaymentControl();
            PaymentInfo paymentInfo = new PaymentInfo();
            SetPayScheme();
            string GroupName = objGeneral.getOPTGroupByCarrierCode(hCarrierCode.Value);
            paymentInfo = objPayment.GetPaymentScheme(PayScheme, GroupName, hID.Value); //unremarked by diana 20140120
            //set all to AA group as AAX now follow AA settings
            //paymentInfo = objPayment.GetPaymentScheme(PayScheme, "AA");
            decimal CollectedAmount = 0;
            decimal ServiceChg = 0;
            decimal FullPrice = 0;
            decimal AmountDue = 0;
            DataTable dt = objBooking.dtClass();
            //if (HttpContext.Current.Session["TempFlight"] != null && PNRExist == false)
            //{
            //    if (HttpContext.Current.Session["dataClassTrans"] != null)
            //    {
            //        dt = (DataTable)HttpContext.Current.Session["dataClassTrans"];
            //        DataRow[] dtrows = dt.Select("SellSignature = '" + SessionID + "'");

            //        //BookingControl booking = new BookingControl();
            //        //ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking("");
            //        //ABS.Navitaire.BookingManager.Booking book = apiBooking.GetBookingFromState(SessionID);
            //        //string xml = GetXMLString(book);

            //        if (dtrows.Length == 1)
            //        {
            //            CollectedAmount = Convert.ToDecimal(dtrows[0]["DetailCollectedAmt"].ToString());
            //            ServiceChg = Convert.ToDecimal(dtrows[0]["ServChrg"].ToString());
            //            FullPrice = Convert.ToDecimal(dtrows[0]["FullPrice"].ToString());

            //            /// amended by diana 20130913
            //            //decimal paymentAttempt1 = 0;
            //            //paymentAttempt1 = ((FullPrice - ServiceChg) * paymentInfo.Percentage_1) / 100;
            //            if (paymentInfo.PaymentType == "SVCF")
            //            {
            //                if ((ServiceChg - CollectedAmount) >= 0)
            //                {
            //                    return ServiceChg - CollectedAmount;
            //                }
            //            }
            //            else
            //            {
            //                return FullPrice;
            //            }
            //            /// end amended by diana 20130913

            //            /// commented by diana 20130913
            //            //if (PayScheme == "B2M" || PayScheme == "W2M" || PayScheme == "W1M" )
            //            //{
            //            //    if ((ServiceChg - CollectedAmount) >= 0)
            //            //    {
            //            //        return ServiceChg - CollectedAmount;
            //            //    }
            //            //}
            //            //else
            //            //{
            //            //    return FullPrice;
            //            //}
            //            /// end commented by diana 20130913
            //        }
            //    }
            //}
            //else
            {
                if (HttpContext.Current.Session["TransMain"] != null)
                {
                    DataTable dtTransMain = objBooking.dtTransMain();
                    dtTransMain = (DataTable)HttpContext.Current.Session["TransMain"];
                    DateTime STD = Convert.ToDateTime(dtTransMain.Rows[0]["STDDate"].ToString());
                    DateTime BookingDate = Convert.ToDateTime(dtTransMain.Rows[0]["BookingDate"].ToString());
                    DateTime todays = DateTime.Now;
                    //int remainingdays = STD.Subtract(todays).Days;
                    //int BookingDays = todays.Subtract(BookingDate).Days;

                    double remainingHrs = (STD.Subtract(todays).TotalHours);
                    double BookingHrs = todays.Subtract(BookingDate).TotalHours;

                    //decimal paymentIn48HR = 0;
                    //decimal PaymentIn2ndAtp = 0;
                    //decimal PaymentIn3rdAtp = 0;

                    DataTable dtTransDetail = objBooking.dtTransDetail();
                    dtTransDetail = (DataTable)HttpContext.Current.Session["TransDetail"];
                    DataRow[] dtrows = dtTransDetail.Select("RecordLocator = '" + SessionID + "'");
                    if (dtrows.Length == 1)
                    {
                        CollectedAmount = Convert.ToDecimal(dtrows[0]["DetailCollectedAmount"].ToString());
                        AmountDue = Convert.ToDecimal(dtrows[0]["AmountDue"].ToString());
                        ServiceChg = Convert.ToDecimal(dtrows[0]["LineFee"].ToString());
                        FullPrice = Convert.ToDecimal(dtrows[0]["LineTotal"].ToString());

                        /// amended by diana 20130913
                        decimal paymentAttempt1 = 0;
                        decimal paymentAttempt2 = 0;
                        decimal paymentAttempt3 = 0;

                        //added by diana 20140121 - retrieve currency, origin, transit details
                        BookingTransactionDetail objBK_TRANSDTL_Info = new BookingTransactionDetail();
                        objBK_TRANSDTL_Info = objBooking.GetBK_TRANSDTLFlightByPNR(SessionID);
                        Decimal deposit = 0;
                        //paymentAttempt1 = ((FullPrice - ServiceChg) * paymentInfo.Percentage_1) / 100 + ServiceChg;
                        //paymentAttempt2 = ((FullPrice - ServiceChg) * paymentInfo.Percentage_2) / 100 + paymentAttempt1;
                        //paymentAttempt3 = ((FullPrice - ServiceChg) * paymentInfo.Percentage_3) / 100 + paymentAttempt2;

                        //amended by diana 20140121 - add if for every attempt to check for deposit
                        paymentAttempt1 = (FullPrice * paymentInfo.Percentage_1) / 100;
                        if (paymentInfo.Deposit_1 != 0)
                        {
                            if (deposit == 0)
                            {
                                deposit = objGeneral.getDeposit(hID.Value, objBK_TRANSDTL_Info.TotalPax, objBK_TRANSDTL_Info.Currency, objBK_TRANSDTL_Info.Origin, objBK_TRANSDTL_Info.Transit);
                            }
                            paymentAttempt1 = deposit + ServiceChg;
                        }

                        //amended by diana 20140121 - add condition to differentiate deposit and non deposit
                        if (paymentInfo.Deposit_1 != 0)
                            paymentAttempt2 = (FullPrice * paymentInfo.Percentage_2) / 100;
                        else
                            paymentAttempt2 = (FullPrice * paymentInfo.Percentage_2) / 100 + paymentAttempt1;

                        if (paymentInfo.Deposit_2 != 0)
                        {
                            if (deposit == 0)
                            {
                                deposit = objGeneral.getDeposit(hID.Value, objBK_TRANSDTL_Info.TotalPax, objBK_TRANSDTL_Info.Currency, objBK_TRANSDTL_Info.Origin, objBK_TRANSDTL_Info.Transit);
                            }
                            paymentAttempt2 = deposit + paymentAttempt1;
                        }

                        //amended by diana 20140121 - add condition to differentiate deposit and non deposit
                        if (paymentInfo.Deposit_2 != 0)
                            paymentAttempt3 = (FullPrice * paymentInfo.Percentage_3) / 100;
                        else
                            paymentAttempt3 = (FullPrice * paymentInfo.Percentage_3) / 100 + paymentAttempt2;
                        if (paymentInfo.Deposit_3 != 0)
                        {
                            if (deposit == 0)
                            {
                                deposit = objGeneral.getDeposit(hID.Value, objBK_TRANSDTL_Info.TotalPax, objBK_TRANSDTL_Info.Currency, objBK_TRANSDTL_Info.Origin, objBK_TRANSDTL_Info.Transit);
                            }
                            paymentAttempt3 = deposit + paymentAttempt2;
                        }


                        bool doneattempt1 = true;
                        bool doneattempt2 = true;
                        bool doneattempt3 = true;

                        if (paymentInfo.Code_1 == "DOB")
                        {
                            //if (BookingHrs <= paymentInfo.Attempt_1)
                            //{
                            if (paymentAttempt1 > CollectedAmount)
                            {
                                doneattempt1 = false;
                            }
                            //}
                        }
                        else if (paymentInfo.Code_1 == "STD")
                        {
                            //if (remainingHrs <= paymentInfo.Attempt_1)
                            //{
                            if (paymentAttempt1 > CollectedAmount)
                            {
                                doneattempt1 = false;
                            }
                            //}
                        }
                        if (doneattempt1 == true)
                        {
                            if (paymentInfo.Code_2 == "DOB")
                            {
                                //if (BookingHrs <= paymentInfo.Attempt_2)
                                //{
                                if (paymentAttempt2 > CollectedAmount)
                                {
                                    doneattempt2 = false;
                                }
                                //}
                            }
                            else if (paymentInfo.Code_2 == "STD")
                            {
                                //if (remainingHrs <= paymentInfo.Attempt_2)
                                //{
                                if (paymentAttempt2 > CollectedAmount)
                                {
                                    doneattempt2 = false;
                                }
                                //}
                            }

                            if (doneattempt2 == true)
                            {
                                if (paymentInfo.Code_3 == "DOB")
                                {
                                    //if (BookingHrs <= paymentInfo.Attempt_3)
                                    //{
                                    if (paymentAttempt3 > CollectedAmount)
                                    {
                                        doneattempt3 = false;
                                    }
                                    //}
                                }
                                else if (paymentInfo.Code_3 == "STD")
                                {
                                    //if (remainingHrs <= paymentInfo.Attempt_3)
                                    //{
                                    if (paymentAttempt3 > CollectedAmount)
                                    {
                                        doneattempt3 = false;
                                    }
                                    //}
                                }
                            }
                        }

                        if (doneattempt1 == false)
                            return paymentAttempt1 - CollectedAmount;
                        else if (doneattempt2 == false)
                            return paymentAttempt2 - CollectedAmount;
                        else if (doneattempt3 == false)
                            return paymentAttempt3 - CollectedAmount;
                        else if (PayScheme == "W1W")
                            return FullPrice - CollectedAmount;
                        /// end here
                        /// end amended by diana 20130913
                        
                    }
                    else
                    {
                        //if collected amount = 0 or not > service charge

                        if (HttpContext.Current.Session["dataClassTrans"] != null)
                        {
                            dt = (DataTable)HttpContext.Current.Session["dataClassTrans"];
                            DataRow[] dtrows2 = dt.Select("RecordLocator = '" + SessionID + "'");
                            if (dtrows2.Length == 1)
                            {
                                CollectedAmount = Convert.ToDecimal(dtrows2[0]["DetailCollectedAmt"].ToString());
                                ServiceChg = Convert.ToDecimal(dtrows2[0]["ServChrg"].ToString());
                                FullPrice = Convert.ToDecimal(dtrows2[0]["FullPrice"].ToString());
                                /// amended by diana 20130913
                                //decimal paymentAttempt1 = 0;
                                //paymentAttempt1 = ((FullPrice - ServiceChg) * paymentInfo.Percentage_1) / 100;
                                if (paymentInfo.PaymentType == "SVCF")
                                {
                                    if ((ServiceChg - CollectedAmount) >= 0)
                                    {
                                        return ServiceChg - CollectedAmount;
                                    }
                                }
                                else
                                {
                                    return FullPrice;
                                }
                                /// end amended by diana 20130913

                                
                            }
                        }

                    }
                    
                }
            }
            if (NonZeroAmount == true)
            {
                return FullPrice - CollectedAmount;
            }
            else
                return 0;
        }

        public void SetScreen()
        {
            try
            {
                //outputDirectDebitProviders();

                if (!IsPostBack)
                {
                    LoadCreditCardDefaultDate(true);
                    LoadIssuingCountry();
                    LoadCountryStateAddress();
                    LoadContactCountryStateAddress();
                    LoadContactDetails();
                }
                else
                    LoadCreditCardDefaultDate();

                //lblPNR.Text = Session["RecordLocator"].ToString();
                lblRecordLocator.Text = Session["RecordLocator"].ToString();

                CheckErrorSession();

                //LoadProcessingFee();

                //CheckProcessingFee();

                //to hide panel of processing fee
                panelProcessFee.Visible = false;

                if (MyUserSet.AgentType.ToString().ToLower() == "skyagent")
                {
                    ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse creditResponse = GetAGCredit();

                    if (creditResponse != null)
                    {
                        lblAGCreditAmount.Text = objGeneral.RoundUp(creditResponse.AvailableCreditResponse.Account.ForeignAmount).ToString("N", nfi);

                        lblAGCreditCurrency.Text = creditResponse.AvailableCreditResponse.Account.ForeignCurrencyCode;
                    }
                    else
                    {
                        lblAGCreditAmount.Text = "0";
                    }

                    TabControl.TabPages.FindByName("TabAG").Visible = true;
                    //TabControl.TabPages.FindByName("TabCreditAccount").Visible = false;
                }
                else
                {
                    TabControl.TabPages.FindByName("TabAG").Visible = false;
                    //TabControl.TabPages.FindByName("TabCreditAccount").Visible = true;
                }

                //if (HttpContext.Current.Session["MemberData"] != null) //load credit balance
                //{
                //    DataTable dtMember = objBooking.dtMemberData();
                //    dtMember = (DataTable)HttpContext.Current.Session["MemberData"];
                //    lblCreditAccount.Text = Math.Round(Convert.ToDecimal(dtMember.Rows[0]["Amount"]), 2).ToString("N", nfi);
                //    lblCreditAccountCurrency.Text = dtMember.Rows[0]["Currency"].ToString();
                //}
                //else
                //{
                //    lblCreditAccount.Text = "0";
                //    lblCreditAccountCurrency.Text = lblCurrentTotalCurrency.Text;
                //}
                lblErrorTop.Text = "";
                lblErrorBottom.Text = "";
                imgError.Visible = false;

                if (Session["reloadPage"].ToString() != "true")
                {
                    lblErrorTop.Text = "";
                    lblErrorBottom.Text = "";
                    imgError.Visible = false;
                }

                //rdlPNR.Enabled = true;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
        }

        //public void SetContactVisibility()
        //{
        //    //added by diana 20131116 - if status 1 then hide contact details
        //    BookingTransactionMain bookMain = new BookingTransactionMain();
        //    bookMain = objBooking.GetSingleBK_TRANSMAIN(hID.Value);
        //    if (bookMain != null)
        //    {
        //        if (bookMain.TransStatus == 0) { }
        //        else
        //        {

        //            divContact.Style["display"] = "none";
        //            if (Session["ContactAddress"] != null) txtContactAddress.Text = Session["ContactAddress"].ToString(); else txtContactAddress.Text = "";
        //            if (Session["ContactTown"] != null) txtContactAddress.Text = Session["ContactTown"].ToString(); txtContactTown.Text = "-";
        //            if (Session["ContactZipCode"] != null) txtContactAddress.Text = Session["ContactZipCode"].ToString(); txtContactZipCode.Text = "-";
        //            if (Session["ContactCountryAddress"] != null) cmbContactCountryAddress.Value = Session["ContactCountry"].ToString();
        //            if (Session["ContactState"] != null) cmbContactState.Value = Session["ContactState"].ToString();

        //        }
        //    }
        //    //end added by diana 20131116 - if status 1 then hide contact details
        //}

        public string GetCurrency()
        {
            try
            {
                DataTable dt = objBooking.dtFlight();
                HttpCookie cookie = Request.Cookies["cookieTemFlight"];
                string OutID = cookie.Values["list1ID"];
                currency = "";
                if (OutID != "")
                {
                    string strExpr;
                    string strSort;

                    strExpr = "TemFlightId = '" + OutID + "'";

                    strSort = "";

                    dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                    DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                    currency = foundRows[0]["TemFlightCurrencyCode"].ToString();

                }
                return currency;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                return currency;
            }
        }

        public decimal GetTotalDepart()
        {
            try
            {
                DataTable dt = objBooking.dtFlight();
                HttpCookie cookie = Request.Cookies["cookieTemFlight"];

                totalAmount = 0;

                string OutID = cookie.Values["list1ID"];

                if (OutID != "")
                {
                    string strExpr;
                    string strSort;

                    strExpr = "TemFlightId = '" + OutID + "'";

                    strSort = "";

                    dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                    DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                    totalDepart = Convert.ToDecimal(foundRows[0]["TemFlightTotalAmount"]);

                    return totalDepart;

                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                return 0;
            }

        }

        public decimal GetTotalReturn()
        {
            try
            {
                DataTable dt = objBooking.dtFlight();
                HttpCookie cookie = Request.Cookies["cookieTemFlight"];

                totalAmount = 0;

                string InID = cookie.Values["ReturnID"];

                if (InID != "")
                {
                    string strExpr;
                    string strSort;

                    strExpr = "TemFlightId = '" + InID + "'";

                    strSort = "";

                    dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                    DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                    totalReturn = Convert.ToDecimal(foundRows[0]["TemFlightTotalAmount"]);

                    return totalReturn;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                return 0;
            }
        }

        public void LoadIssuingCountry()
        {
            UIClass.SetComboStyle(ref cmbIssuingCountry, UIClass.EnumDefineStyle.CountryCard);

        }
        public void LoadCountryStateAddress()
        {

            UIClass.SetComboStyle(ref cmbCountryAddress, UIClass.EnumDefineStyle.Country);
            UIClass.SetComboStyle(ref cmbState, UIClass.EnumDefineStyle.State, "", cmbCountryAddress.Value.ToString());
        }
        public void LoadContactCountryStateAddress()
        {

            UIClass.SetComboStyle(ref cmbContactCountryAddress, UIClass.EnumDefineStyle.Country);
            UIClass.SetComboStyle(ref cmbContactState, UIClass.EnumDefineStyle.State, "", cmbContactCountryAddress.Value.ToString());
        }
        public void LoadContactDetails()
        {
            //if (Session["ContactAddress"] != null)
            //{
            //    if (Session["ContactTitle"] != null) cmbContactTitle.Value = Session["ContactTitle"].ToString();
            //    if (Session["ContactFirstName"] != null) txtContactFirstName.Text = Session["ContactFirstName"].ToString(); txtContactFirstName.Text = "";
            //    if (Session["ContactLastName"] != null) txtContactLastName.Text = Session["ContactLastName"].ToString(); txtContactLastName.Text = "";
            //    if (Session["ContactEmail"] != null) txtContactEmail.Text = Session["ContactEmail"].ToString(); else txtContactEmail.Text = "";
            //    if (Session["ContactPhoneNo"] != null) txtContactPhone.Text = Session["ContactPhoneNo"].ToString(); else txtContactPhone.Text = "";
            //    if (Session["ContactAddress"] != null) txtContactAddress.Text = Session["ContactAddress"].ToString(); else txtContactAddress.Text = "";
            //    if (Session["ContactTown"] != null) txtContactTown.Text = Session["ContactTown"].ToString(); txtContactTown.Text = "";
            //    if (Session["ContactZipCode"] != null) txtContactZipCode.Text = Session["ContactZipCode"].ToString(); txtContactZipCode.Text = "";
            //    if (Session["ContactCountryAddress"] != null) cmbContactCountryAddress.Value = Session["ContactCountry"].ToString();
            //    if (Session["ContactState"] != null) cmbContactState.Value = Session["ContactState"].ToString();
            //}
            //else
            //{
            agent = objAgentProfile.GetSingleAgentProfile(MyUserSet.AgentName);
            if (agent.Title != null)
            {
                if (agent.Title.ToString() != "")
                {
                    cmbContactTitle.Value = agent.Title;
                }
            }
            txtContactFirstName.Text = agent.ContactFirstName;
            txtContactLastName.Text = agent.ContactLastName;
            txtContactEmail.Text = agent.Email;
            txtContactPhone.Text = agent.PhoneNo;
            txtContactAddress.Text = agent.Address1;
            txtContactTown.Text = agent.City;
            if (agent.Country != null)
            {
                if (agent.Country.ToString() != "")
                {
                    cmbContactCountryAddress.Value = agent.Country;
                }
            }
            if (agent.State != null)
            {
                if (agent.State.ToString() != "")
                {
                    cmbContactState.Value = agent.State;
                }
            }
            txtContactZipCode.Text = agent.Postcode;
            //}
        }
        public void LoadCreditCardDefaultDate(Boolean DefaultMonth = false)
        {
            DateTime dtYear = DateTime.Now;
            ListItem itemYear = new ListItem();
            UIClass.SetComboStyle(ref cmbExpiryYear, UIClass.EnumDefineStyle.YearsPlus);
            if (DefaultMonth)
            {
                cmbExpiryMonth.Items.FindByText(dtYear.Month.ToString()).Selected = true;
                cmbExpiryYear.SelectedIndex = 0;
            }
        }

        public void LoadRightPanel(string TransID)
        {
            try
            {
                lstbookFlightInfo = new List<BookingTransactionDetail>();
                lstbookFlightInfo = (List<BookingTransactionDetail>)HttpContext.Current.Session["DetailList"];
                //lstbookFlightInfo = objBooking.GetAllBK_TRANSDTLFlight(TransID);

                int lastIndex = lstbookFlightInfo.Count - 1;

                if (ReturnOnly == true)
                {
                    lblDepart.Text = "Return";
                }

                //added by ketee
                hCarrierCode.Value = lstbookFlightInfo[0].CarrierCode;

                lblDepartOrigin.Text = objGeneral.GetCityNameByCode(lstbookFlightInfo[0].Origin) + "(" + lstbookFlightInfo[0].Origin + ")";
                lblDepartDestination.Text = objGeneral.GetCityNameByCode(lstbookFlightInfo[0].Destination) + "(" + lstbookFlightInfo[0].Destination + ")";

                lblDateDepart.Text = lstbookFlightInfo[0].CarrierCode + " " + lstbookFlightInfo[0].FlightNo + " - " + String.Format("{0:dd MMM yyyy}", lstbookFlightInfo[0].DepatureDate);

                lblDepartStd.Text = String.Format("{0:HHmm}", lstbookFlightInfo[0].DepatureDate);

                DateTime tempdate1 = lstbookFlightInfo[0].ArrivalDate;
                DateTime tempdate2 = lstbookFlightInfo[0].DepatureDate;
                if (lstbookFlightInfo[0].Transit != "")
                    tempdate1 = lstbookFlightInfo[0].ArrivalDate2;
                TimeSpan ts = tempdate1.Date - tempdate2.Date;
                string temp = "";
                if (ts.Days > 0)
                {
                    temp = " (+" + ts.TotalDays.ToString() + ")";
                }

                if (lstbookFlightInfo[0].Transit != "")
                {
                    lblTransitDepart.Text = objGeneral.GetCityNameByCode(lstbookFlightInfo[0].Transit) + "(" + lstbookFlightInfo[0].Transit + ")";
                    lblDateTransitDepart.Text = String.Format("{0:dd MMM yyyy}", lstbookFlightInfo[0].DepatureDate2) + " " + String.Format("{0:HHmm}", lstbookFlightInfo[0].DepatureDate2) + " - " + String.Format("{0:HHmm}", lstbookFlightInfo[0].ArrivalDate2);
                    lblDepartSta.Text = String.Format("{0:HHmm}", lstbookFlightInfo[0].ArrivalDate2) + temp;
                }
                else
                {
                    lblTransitDepart.Text = "";
                    lblDateTransitDepart.Text = "";
                    lblTextTransitDepart.Text = "";
                    lblDepartSta.Text = String.Format("{0:HHmm}", lstbookFlightInfo[0].ArrivalDate) + temp;
                }

                //load carrier code
                if (HttpContext.Current.Session["PaymentMaxAttempt"] == null)
                {
                    string carrierSetting = objGeneral.getSysValueByKeyAndCarrierCode("PAYMENTSUSPEND", lstbookFlightInfo[0].CarrierCode);
                    HttpContext.Current.Session.Add("PaymentMaxAttempt", carrierSetting);
                }

                bool returnFlight = false;
                if (lstbookFlightInfo.Count > 1)
                {
                    if (lstbookFlightInfo[0].Origin.ToString() != lstbookFlightInfo[lastIndex].Origin.ToString())
                    {
                        returnFlight = true;
                    }
                }
                if (returnFlight)
                {
                    lblReturnOrigin.Text = objGeneral.GetCityNameByCode(lstbookFlightInfo[lastIndex].Origin) + "(" + lstbookFlightInfo[lastIndex].Origin + ")";
                    lblReturnDestination.Text = objGeneral.GetCityNameByCode(lstbookFlightInfo[lastIndex].Destination) + "(" + lstbookFlightInfo[lastIndex].Destination + ")";

                    lblDateReturn.Text = lstbookFlightInfo[lastIndex].CarrierCode + " " + lstbookFlightInfo[lastIndex].FlightNo + " - " + String.Format("{0:dd MMM yyyy}", lstbookFlightInfo[lastIndex].DepatureDate);

                    lblReturnStd.Text = String.Format("{0:HHmm}", lstbookFlightInfo[lastIndex].DepatureDate);

                    tempdate1 = lstbookFlightInfo[lastIndex].ArrivalDate;
                    tempdate2 = lstbookFlightInfo[lastIndex].DepatureDate;
                    if (lstbookFlightInfo[lastIndex].Transit != "")
                        tempdate1 = lstbookFlightInfo[lastIndex].ArrivalDate2;
                    ts = tempdate1.Date - tempdate2.Date;
                    temp = "";
                    if (ts.Days > 0)
                    {
                        temp = " (+" + ts.TotalDays.ToString() + ")";
                    }

                    if (lstbookFlightInfo[lastIndex].Transit != "")
                    {
                        lblTransitReturn.Text = objGeneral.GetCityNameByCode(lstbookFlightInfo[lastIndex].Transit) + "(" + lstbookFlightInfo[lastIndex].Transit + ")";
                        lblTransitDateReturn.Text = String.Format("{0:dd MMM yyyy}", lstbookFlightInfo[lastIndex].DepatureDate2) + " " + String.Format("{0:HHmm}", lstbookFlightInfo[lastIndex].DepatureDate2) + " - " + String.Format("{0:HHmm}", lstbookFlightInfo[lastIndex].ArrivalDate2);
                        lblReturnSta.Text = String.Format("{0:HHmm}", lstbookFlightInfo[lastIndex].ArrivalDate2) + temp;
                    }
                    else
                    {
                        lblTransitReturn.Text = "";
                        lblTransitDateReturn.Text = "";
                        lblTextTransitReturn.Text = "";
                        lblReturnSta.Text = String.Format("{0:HHmm}", lstbookFlightInfo[lastIndex].ArrivalDate) + temp;
                    }

                }
                else
                {
                    pnlReturn.Visible = false;
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);

            }


        }

        public void ResetCreditCardDetails()
        {
            cmbCardType.SelectedIndex = 0;
            LoadCreditCardDefaultDate(true);
            LoadIssuingCountry();
            LoadCountryStateAddress();
            txtCardNumber.Text = "";
            txtCardIssuer.Text = "";
            txtCardHolderName.Text = "";
            txtCVV2.Text = "";

            txtAddress.Text = "";
            txtTown.Text = "";
            txtZipCode.Text = "";

            lblErrorTop.Focus();
        }

        public ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse GetAGCredit()
        {
            try
            {
                string psDomain = ConfigurationManager.AppSettings["signature_domain"].ToString();
                string psName = ConfigurationManager.AppSettings["signature_username"].ToString();
                string psPwd = ConfigurationManager.AppSettings["signature_password"].ToString();

                string signature = absNavitaire.AgentLogon("public", psDomain, psName, psPwd);
                //ABS.Navitaire.AgentManager.FindAgentsResponse agentresp = absNavitaire.FindingAgent(MyUserSet.AgentName, signature);
                //string accountReference = agentresp.FindAgentResponseData.FindAgentList[0].AgentIdentifier.OrganizationCode;
                string accountReference = Session["OrganizationCode"].ToString();

                ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse accResp = absNavitaire.GetCreditByAccountReference(accountReference, lblCurrentTotalCurrency.Text, signature);
                return accResp;

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                return null;
            }
        }

        public void LoadProcessingFee()
        {
            ABS.Navitaire.BookingManager.GetBookingResponse bookingResponse = new ABS.Navitaire.BookingManager.GetBookingResponse();
            if (HttpContext.Current.Session["dataClassTrans"] != null)
            {
                if (HttpContext.Current.Session["TotalProcessFee"] != null)
                {
                    decimal processingFee = Convert.ToDecimal(HttpContext.Current.Session["TotalProcessFee"]);
                    lblProcessFee.Text = processingFee.ToString("N", nfi);
                    lblCurrencyProcessFee.Text = lblCurrentTotalCurrency.Text;
                    //updateTotalWithProcessingFee(Convert.ToDecimal(lblAmountDue.Text), processingFee);
                }
                else
                {
                    DataTable dataClass = objBooking.dtClassTrans();
                    dataClass = (DataTable)HttpContext.Current.Session["dataClassTrans"];
                    decimal processingFee = getTotalProceessFee(dataClass, lblCurrentTotalCurrency.Text);
                    lblProcessFee.Text = processingFee.ToString("N", nfi);
                    lblCurrencyProcessFee.Text = lblCurrentTotalCurrency.Text;

                    HttpContext.Current.Session.Remove("TotalProcessFee");
                    HttpContext.Current.Session.Add("TotalProcessFee", processingFee);

                    //updateTotalWithProcessingFee(Convert.ToDecimal(lblAmountDue.Text), processingFee);
                }
            }
            else
            {
                if (HttpContext.Current.Session["TotalProcessFee"] != null)
                {
                    decimal processingFee = Convert.ToDecimal(HttpContext.Current.Session["TotalProcessFee"]);
                    lblProcessFee.Text = processingFee.ToString("N", nfi);
                    lblCurrencyProcessFee.Text = lblCurrentTotalCurrency.Text;
                    //updateTotalWithProcessingFee(Convert.ToDecimal(lblAmountDue.Text), processingFee);
                }
                else
                {
                    if (HttpContext.Current.Session["TransDetail"] != null)
                    {
                        DataTable dtTransDetail = objBooking.dtTransDetail();
                        dtTransDetail = (DataTable)HttpContext.Current.Session["TransDetail"];
                        DataTable dtProcessFee = new DataTable();
                        dtProcessFee.Columns.Add("SellSignature");
                        dtProcessFee.Columns.Add("FullPrice");
                        string errMessage = "";
                        for (int idx = 0; idx < dtTransDetail.Rows.Count; idx++)
                        {
                            DataRow row;
                            row = dtProcessFee.NewRow();
                            row["SellSignature"] = absNavitaire.GetBookingByPNR(dtTransDetail.Rows[idx]["RecordLocator"].ToString(), ref errMessage, ref bookingResponse);

                            //row["FullPrice"] = Convert.ToDecimal(dtTransDetail.Rows[idx]["LineTotal"]);
                            row["FullPrice"] = Convert.ToDecimal(dtTransDetail.Rows[idx]["AmountDue"]);

                            dtProcessFee.Rows.Add(row);
                        }

                        decimal processingFee = getTotalProceessFee(dtProcessFee, lblCurrentTotalCurrency.Text);
                        lblProcessFee.Text = processingFee.ToString("N", nfi);
                        lblCurrencyProcessFee.Text = lblCurrentTotalCurrency.Text;

                        HttpContext.Current.Session.Remove("TotalProcessFee");
                        HttpContext.Current.Session.Add("TotalProcessFee", processingFee);

                        //updateTotalWithProcessingFee(Convert.ToDecimal(lblAmountDue.Text), processingFee);

                    }
                }
            }
            bookingResponse = null;
        }

        protected decimal getTotalProceessFee(DataTable dataClass, string currency)
        {
            try
            {
                decimal totalProcess = 0;

                for (int i = 0; i < dataClass.Rows.Count; i++)
                {

                    ABS.Navitaire.BookingManager.GetPaymentFeePriceResponse feeResponse = absNavitaire.GetProcessingFee(dataClass.Rows[i]["SellSignature"].ToString(), currency, Convert.ToDecimal(dataClass.Rows[i]["FullPrice"]));
                    totalProcess += getSingleProcessingFeeByResponse(feeResponse);
                }

                return totalProcess;
            }
            catch
            {
                return 0;
            }

        }

        protected decimal getSingleProcessingFeeByResponse(ABS.Navitaire.BookingManager.GetPaymentFeePriceResponse feeResponse)
        {
            decimal totalProcess = 0;
            if (feeResponse.paymentFeePriceRespData.PassengerFees.Length > 0)
            {
                if (feeResponse.paymentFeePriceRespData.PassengerFees[0].ServiceCharges.Length > 0)
                {
                    for (int i = 0; i < feeResponse.paymentFeePriceRespData.PassengerFees.Length; i++)
                    {
                        totalProcess += feeResponse.paymentFeePriceRespData.PassengerFees[i].ServiceCharges[0].ForeignAmount;
                    }
                }
            }
            return totalProcess;
        }

        public void CheckErrorSession()
        {
            if (HttpContext.Current.Session["ErrorPayment"] != null)
            {
                MessageList msgList = new MessageList();
                lblErrorTop.Text = msgList.Err100034;
                lblErrorBottom.Text = msgList.Err100034;
                imgError.Visible = true;
                lblErrorTop.Visible = true;
                lblErrorTop.Visible = true;
            }
            else
            {
                lblErrorTop.Text = "";
                lblErrorBottom.Text = "";
                imgError.Visible = false;
                lblErrorTop.Visible = false;
                lblErrorTop.Visible = false;
            }
        }

        protected Boolean checkPaymentAttempt()
        {
            log.Info(this,"Entering checkPaymentAttempt()");
            int attempt = 0; //default 
            if (HttpContext.Current.Session["PaymentAttempt"] != null)
            {
                attempt = Convert.ToInt32(HttpContext.Current.Session["PaymentAttempt"]);
            }

            int maxTry = Convert.ToInt32(HttpContext.Current.Session["PaymentMaxAttempt"]);
            attempt += 1;
            HttpContext.Current.Session.Remove("PaymentAttempt");
            HttpContext.Current.Session.Add("PaymentAttempt", attempt);

            log.Info(this,"Payment Attempt : " + attempt + " and Max Try : " + maxTry);
            if (attempt > maxTry)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        protected void failedPayment(string errMsg)
        {
            //ClearSession();
            HttpContext.Current.Session.Remove("ErrorPayment");
            HttpContext.Current.Session.Add("ErrorPayment", "error");
            //msgcontrol.MessageDisplay(errMsg);

            lblErrorBottom.Visible = true;
            lblErrorBottom.Text = errMsg;
            imgError.Visible = true;

            //Response.Write("<script type='text/javascript'>window.alert('You have entered invalid credit card data for 3 times.\nYour booking has been cancelled. Please kindly re-book your flight.')</script>");
            //Response.Redirect("~/public/agentmain.aspx");
        }

        protected void SelectState()
        {
            string tabname = TabControl.ActiveTabPage.Name;
            divCreditCard.Visible = false;
            divAG.Visible = false;
            switch (tabname)
            {
                case "TabCredit":
                    divCreditCard.Visible = true;
                    //if (ViewState["savedMinPay"] != null)
                    //{
                    //    lblMinPay.Text = ViewState["MinPaymeny"].ToString();
                    //    txtMinPay.Text = ViewState["MinPaymeny"].ToString();
                    //    txtPayAmount.Text = ViewState["MinPaymeny"].ToString();

                    //    //lblMinPay.Text = txtMinPay.Text;
                    //    //txtMinPay.Text = txtMinPay.Text;
                    //    //txtPayAmount.Text = txtMinPay.Text;
                    //    ViewState["savedMinPay"] = null;
                    //}
                    
                    lblMinPay.Text = txtMinimumPayment.Text;
                    txtMinPay.Text = txtMinimumPayment.Text;
                    txtPayAmount.Text = txtMinimumPayment.Text;
                    ViewState["MinPaymeny"] = txtMinimumPayment.Text;
                    ViewState["savedMinPay"] = null;

                    break;
                case "TabAG":
                    if (ViewState["savedMinPay"] == null && rdlPNR.SelectedIndex != -1 && txtMinPay.Text != "")
                    {
                        ViewState["MinPaymeny"] = txtMinPay.Text;

                        ViewState["savedMinPay"] = "1";

                        lblMinPay.Text = txtDueAmount.Text;
                        txtMinPay.Text = txtDueAmount.Text;
                        txtPayAmount.Text = txtDueAmount.Text;
                    }
                    divAG.Visible = true;
                    break;
                case "TabCreditAccount":

                    break;
                case "TabDirectDebit":

                    break;
            }
        }

        private void UpdateSessiondataClassTrans(string SessionID, decimal CollectedAmount, string PNR)
        {
            string str = "";

            if (HttpContext.Current.Session["dataClassTrans"] != null)
            {
                DataTable dt = objBooking.dtClass();
                dt = (DataTable)HttpContext.Current.Session["dataClassTrans"];
                foreach (DataRow dr in dt.Rows)
                {
                    str += "Session = " + SessionID + "; SellSignature = " + dr["SellSignature"] + ";\n";
                    if (SessionID != "" && dr["SellSignature"].ToString() == SessionID)
                    {
                        dr["RecordLocator"] = PNR;
                        dr["DetailCollectedAmt"] = CollectedAmount + Convert.ToDecimal(dr["DetailCollectedAmt"].ToString());
                    }
                    else
                    {
                        if (PNR != "" && dr["RecordLocator"].ToString() == PNR)
                        {
                            dr["DetailCollectedAmt"] = CollectedAmount + Convert.ToDecimal(dr["DetailCollectedAmt"].ToString());
                        }
                    }
                }

                //if (PNR.ToString().Trim().Length >= 6 && PNR.ToString().Trim().Length <= 10) //amended by diana 20140109 - update transdetail only if PNR is 6 chars
                //{
                //added by ketee, update transdetails
                if (HttpContext.Current.Session["TransMain"] != null && HttpContext.Current.Session["TransDetail"] != null)
                {
                    DataTable dtTransMain = objBooking.dtTransMain();
                    if (HttpContext.Current.Session["TransMain"] != null)
                        dtTransMain = (DataTable)HttpContext.Current.Session["TransMain"];
                    DataTable dtTransDetail = objBooking.dtTransDetail();
                    if (HttpContext.Current.Session["TransDetail"] != null)
                        dtTransDetail = (DataTable)HttpContext.Current.Session["TransDetail"];
                    for (int i = 0; i < dtTransDetail.Rows.Count; i++)
                    {
                        if (dtTransDetail.Rows[i]["SellKey"].ToString() == SessionID)
                        {
                            dtTransDetail.Rows[i]["RecordLocator"] = PNR;

                        }
                    }
                    HttpContext.Current.Session.Remove("TransDetail");
                    HttpContext.Current.Session.Add("TransDetail", dtTransDetail);
                }
                //}
                if (dt.Rows.Count > 0)
                {
                    HttpContext.Current.Session.Remove("dataClassTrans");
                    HttpContext.Current.Session.Add("dataClassTrans", dt);
                }
            }
            log.Info(this,"UpdateSessionDataClassTrans : " + str);
        }

        #endregion

        #region Credit Card Function
        protected void CheckProcessingFee()
        {
            //commented by diana 20131031
            panelProcessFee.Visible = false;
            //if (Convert.ToDouble(lblProcessFee.Text) <= 0)
            //{
            //    panelProcessFee.Visible = false;
            //}
            //else
            //{
            //    panelProcessFee.Visible = true;
            //}
        }
        #endregion

        protected string MakePayment(string PaymentType, string SessionID, Boolean PNRExist = false)
        {
            paymentSuccess = false;
            Session["generatePayment"] = "true";
            DataTable dataClass = objBooking.dtClassTrans();
            MessageList msgList = new MessageList();
            DataTable dttemp = new DataTable();
            decimal lineTotal = 0, paymentAmount = 0, detailTotal = 0, detailCollected = 0, totalPaid = 0, TransTotal = 0;
            string sellSignature = "", TransID = "", errMessage = "", PNR = "";
            Boolean AddPaymentStatus = false;
            lstbookDTLInfo = new List<BookingTransactionDetail>();
            bookDTLInfo = new BookingTransactionDetail();
            bookTransTenderInfo = new BookingTransTender();
            string status = "", pass = "";
            string accNumber = "";
            long accID = 0;

            //added by diana 2013119 - to insert hidden value to combobox - contact county and state


            if (txtContactCountryAddress.Text != "")
                cmbContactCountryAddress.Value = txtContactCountryAddress.Text;
            if (contactState != "")
                cmbContactState.Value = contactState;
            txtContactState.Text = contactState;

            //added by diana 2013119 - to insert hidden value to combobox - contact county and state

            try
            {
                switch (PaymentType)
                {
                    case "TabAG":
                        ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse creditResponse = AGCredit();
                        accNumber = creditResponse.AvailableCreditResponse.Account.AccountReference; //not use
                        accID = creditResponse.AvailableCreditResponse.Account.AccountID;
                        pass = creditResponse.AvailableCreditResponse.Account.Password.ToString();
                        break;
                }

                agent = objAgentProfile.GetSingleAgentProfile(MyUserSet.AgentName);
                paymentAmount = Convert.ToDecimal(txtPayAmount.Text);

                //if (HttpContext.Current.Session["dataClassTrans"] != null && PNRExist == false) //first time payment
                //{
                //    log.Info(this,"Entering (1)");
                //    dataClass = (DataTable)HttpContext.Current.Session["dataClassTrans"];
                //    //sellSignature = dataClass.Rows[0]["SellSignature"].ToString();

                //    log.Info(this,"Counting dataClassTrans : " + dataClass.Rows.Count);
                //    for (int i = 0; i < dataClass.Rows.Count; i++)
                //    {
                //        sellSignature = dataClass.Rows[i]["SellSignature"].ToString();

                //        try
                //        {
                //            decimal FullPrice = Convert.ToDecimal(dataClass.Rows[i]["FullPrice"].ToString());
                //            decimal CollectedAmount = Convert.ToDecimal(dataClass.Rows[i]["DetailCollectedAmt"].ToString());
                //            BookingTransactionDetail SingleDTL = new BookingTransactionDetail();
                //            SingleDTL = objBooking.GetSingle_TRANSDTLBySellKey(sellSignature);
                //            string prevPNR = "";
                //            if (SingleDTL != null)
                //                prevPNR = SingleDTL.RecordLocator.Trim();

                //            log.Info(this,"SellSignature = " + sellSignature + "; SessionID = " + SessionID);
                //            if (sellSignature == SessionID)
                //            {
                //                switch (PaymentType)
                //                {
                //                    case "TabCredit":
                //                        AddPaymentStatus = absNavitaire.AddPaymentCreditCard(cmbCardType.SelectedItem.Value.ToString(), txtCardNumber.Text, cmbExpiryYear.SelectedItem.Value.ToString() + "-" + cmbExpiryMonth.SelectedItem.Value.ToString() + "-" + "01", lblCurrentTotalCurrency.Text, paymentAmount, txtCVV2.Text, txtCardHolderName.Text, txtCardIssuer.Text, cmbIssuingCountry.Value.ToString(), sellSignature,
                //                            txtAddress.Text.ToString(), txtTown.Text.ToString(), cmbCountryAddress.Value.ToString(), cmbState.Value.ToString(), txtZipCode.Text.ToString(), ref errMessage);
                //                        break;
                //                    case "TabAG":
                //                        if (paymentAmount >= FullPrice - CollectedAmount)
                //                        {
                //                            AddPaymentStatus = absNavitaire.AddAgencyPayment(paymentAmount, lblCurrentTotalCurrency.Text, accNumber, pass, sellSignature, accID, ref errMessage);
                //                        }
                //                        else
                //                        {
                //                            errMessage = msgList.Err100039;
                //                        }
                //                        break;
                //                }
                //                log.Info(this,"Value of AddPaymentStatus = " + AddPaymentStatus);
                //                if (AddPaymentStatus)
                //                {
                //                    PNR = objBooking.BookingCommit(agent, Convert.ToInt16(dataClass.Rows[i]["Quantity"]), Convert.ToDecimal(dataClass.Rows[i]["FarePrice"]), dataClass.Rows[i]["SellSignature"].ToString(), ref errMessage, cmbContactTitle.Value.ToString(), txtContactFirstName.Text.ToString(), txtContactLastName.Text.ToString(), txtContactEmail.Text.ToString(), txtContactPhone.Text.ToString(), txtContactAddress.Text.ToString(), txtContactTown.Text.ToString(), cmbContactCountryAddress.Value.ToString(), cmbContactState.Value.ToString(), txtContactZipCode.Text.ToString());
                //                    //ongoing commented by diana 20140109
                //                    //UpdateSessiondataClassTrans(sellSignature, 0, PNR); //update PNR to session class

                //                    log.Info(this,"PNR After BookingCommit: " + PNR);

                //                    if (PNR != "") //added by diana 20140127, execute only if PNR is not empty
                //                    {
                //                        int paySeq = 0;
                //                        string actualPaymentStatus = absNavitaire.GetLastPaymentStatusByPNR(PNR, ref errMessage, ref paySeq);
                //                        if (actualPaymentStatus == "success")
                //                        {
                //                            log.Info(this,"GetLastPaymentSatusByPNR = success");
                //                            //errMessage = "";
                //                            //string prevPNR = (i + 1).ToString();
                //                            string[] arrstr = absNavitaire.GetPassengerByPNR(PNR, ref errMessage);
                //                            objBooking.UpdatePassengerPNR(prevPNR, PNR, hID.Value, arrstr);
                //                            UpdateSessiondataClassTrans(sellSignature, paymentAmount, PNR); // update payment amount to session class
                //                        }
                //                        else
                //                        {
                //                            log.Info(this,"GetLastPaymentSatusByPNR = " + errMessage);
                //                            if (PNR != "")
                //                            {
                //                                if (paySeq == 0 && errMessage == "declined") //amended by diana 20140109 - add condition for 1st payment if declined, then cancel PNR
                //                                {
                //                                    string msg = "";
                //                                    string signature = absNavitaire.AgentLogon();
                //                                    absNavitaire.CancelJourney(PNR, -paymentAmount, "", signature, ref msg); //cancel journey to api
                //                                    signature = absNavitaire.AgentLogon();
                //                                    absNavitaire.ClearJourney(sellSignature, ref msg);
                //                                    //UpdateSessiondataClassTrans(sellSignature, paymentAmount, sellSignature); // update payment amount to session class
                //                                }
                //                                else
                //                                {
                //                                    UpdateSessiondataClassTrans(sellSignature, 0, PNR); //update PNR to session class - on going
                //                                    string[] arrstr = absNavitaire.GetPassengerByPNR(PNR, ref errMessage);
                //                                    objBooking.UpdatePassengerPNR(prevPNR, PNR, hID.Value, arrstr);
                //                                }
                //                            }
                //                            status = errMessage;
                //                            if (status == "declined")
                //                            {
                //                                errMessage = msgList.Err100046;
                //                                status = errMessage;
                //                            }
                //                        }
                //                    }
                //                    else
                //                    {
                //                        errMessage = msgList.Err100028;
                //                        status = errMessage;
                //                        break;
                //                    }

                //                }
                //                else
                //                {
                //                    status = errMessage;
                //                    break;
                //                }
                //                //skip the rest if found
                //                break;
                //            }
                //        }
                //        //added by diana 20131210 - try catch to check for valid booking
                //        catch (TimeoutException ex) { log.Error(this,ex); objBooking.CheckJourneyExist(ex.Message.ToString(), sellSignature); }
                //        catch (OutOfMemoryException ex) { log.Error(this,ex); objBooking.CheckJourneyExist(ex.Message.ToString(), sellSignature); }
                //        catch (IndexOutOfRangeException ex) { log.Error(this,ex); objBooking.CheckJourneyExist(ex.Message.ToString(), sellSignature); }
                //        catch (ThreadInterruptedException ex) { log.Error(this,ex); objBooking.CheckJourneyExist(ex.Message.ToString(), sellSignature); }
                //        catch (NullReferenceException ex) { log.Error(this,ex); objBooking.CheckJourneyExist(ex.Message.ToString(), sellSignature); }
                //        catch (StackOverflowException ex) { log.Error(this,ex); objBooking.CheckJourneyExist(ex.Message.ToString(), sellSignature); }
                //        catch (ApplicationException ex) { log.Error(this,ex); objBooking.CheckJourneyExist(ex.Message.ToString(), sellSignature); }
                //        catch (Exception ex) { log.Error(this,ex); objBooking.CheckJourneyExist(ex.Message.ToString(), sellSignature); }


                //    }
                //}
                //else
                {
                    log.Info(this,"Entering(2)");
                    if (HttpContext.Current.Session["TransMain"] != null && HttpContext.Current.Session["TransDetail"] != null)
                    {
                        DataTable dtTransMain = objBooking.dtTransMain();
                        if (HttpContext.Current.Session["TransMain"] != null)
                            dtTransMain = (DataTable)HttpContext.Current.Session["TransMain"];
                        DataTable dtTransDetail = objBooking.dtTransDetail();
                        if (HttpContext.Current.Session["TransDetail"] != null)
                            dtTransDetail = (DataTable)HttpContext.Current.Session["TransDetail"];

                        int iCount = 0;
                        log.Info(this,"Current SessionID = " + SessionID);
                        for (int i = 0; i < dtTransDetail.Rows.Count; i++)
                        {
                            PNR = dtTransDetail.Rows[i]["RecordLocator"].ToString().Trim();
                            log.Info(this,"Compare PNR(" + PNR + ") with SessionID(" + SessionID + ")");
                            if (PNR == SessionID)
                            {
                                iCount += 1;
                                //string signatureDetail = absNavitaire.GetBookingByPNR(dtTransDetail.Rows[i]["RecordLocator"].ToString(), ref errMessage);
                                string signatureDetail = HttpContext.Current.Session["SellSessionID"].ToString();
                                decimal FullPrice = Convert.ToDecimal(dtTransDetail.Rows[i]["LineTotal"].ToString());
                                decimal CollectedAmount = Convert.ToDecimal(dtTransDetail.Rows[i]["DetailCollectedAmount"].ToString());
                                switch (PaymentType)
                                {
                                    case "TabCredit":
                                        AddPaymentStatus = absNavitaire.AddPaymentCreditCard(cmbCardType.SelectedItem.Value.ToString(), txtCardNumber.Text, cmbExpiryYear.SelectedItem.Value.ToString() + "-" + cmbExpiryMonth.SelectedItem.Value.ToString() + "-" + "01", lblCurrentTotalCurrency.Text, paymentAmount, txtCVV2.Text, txtCardHolderName.Text, txtCardIssuer.Text, cmbIssuingCountry.SelectedItem.Value.ToString(), signatureDetail,
                                            txtAddress.Text.ToString(), txtTown.Text.ToString(), cmbCountryAddress.Value.ToString(), cmbState.Value.ToString(), txtZipCode.Text.ToString(), "", ref errMessage);
                                        break;
                                    case "TabAG":
                                        //if (paymentAmount >= FullPrice - CollectedAmount)
                                        //{
                                        AddPaymentStatus = absNavitaire.AddAgencyPayment(paymentAmount, lblCurrentTotalCurrency.Text, accNumber, pass, signatureDetail, accID, "", ref errMessage);
                                        //}
                                        //else
                                        //{
                                        //    errMessage = msgList.Err100039;
                                        //}
                                        break;
                                }

                                if (AddPaymentStatus)
                                {
                                    if (absNavitaire.BookingCommit(PNR, signatureDetail, ref errMessage, "", false, false, agent.Username, agent.AgentID, cmbContactTitle.Value.ToString(), txtContactFirstName.Text.ToString(), txtContactLastName.Text.ToString(), txtContactEmail.Text.ToString(), txtContactPhone.Text.ToString(), txtContactAddress.Text.ToString(), txtContactTown.Text.ToString(), cmbContactCountryAddress.Value.ToString(), cmbContactState.Value.ToString(), txtContactZipCode.Text.ToString()))
                                    {
                                        log.Info(this,"PNR - BookingCommit(2):" + PNR);
                                        int paySeq = 0;
                                        string actualPaymentStatus = absNavitaire.GetLastPaymentStatusByPNR(PNR, ref errMessage, ref paySeq);
                                        if (actualPaymentStatus == "success")
                                        {
                                            paymentSuccess = true;
                                            TransID = hID.Value;
                                            List<BookingTransactionDetail> oldBookingDetail = new List<BookingTransactionDetail>();
                                            List<BookingTransactionDetail> newBookingDetail = new List<BookingTransactionDetail>();

                                            oldBookingDetail = objBooking.GetAllBK_TRANSDTLFilterByPNR(TransID, PNR);//retrieve previous transdetails
                                            
                                            //amended by diana 20140422, change to UpdateAllBookingJourneyDetails
                                            ListTransaction TransactionDetail = new ListTransaction();
                                            TransactionDetail.TransID = TransID;
                                            //TransactionDetail.TransStatus = (byte)drRow["TransStatus"];
                                            TransactionDetail.AgentID = agent.AgentID;
                                            TransactionDetail.AgentUserName = agent.Username;

                                            List<BookingTransactionDetail> VoidPNRs = new List<BookingTransactionDetail>();
                                            List<BookingTransactionDetail> ExpiredPNRs = new List<BookingTransactionDetail>();
                                            objBooking.UpdateAllBookingJourneyDetails(TransactionDetail, agent.Username, agent.AgentID, ref VoidPNRs, ref ExpiredPNRs, true);
                                            //objBooking.UpdateBookingJourneyDetails(TransID, agent.Username, agent.AgentID, true);
                                            //objBooking.UpdatePaymentDetails(TransID, agent.Username, agent.AgentID, true);

                                            newBookingDetail = objBooking.GetAllBK_TRANSDTLFilterByPNR(TransID, PNR);//retrieve new transdetails
                                            objBooking.CopyTransaction(TransID, PNR, oldBookingDetail, newBookingDetail, ReturnOnly);

                                            ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                                            string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), hID.Value.ToString(), "");

                                            Response.Redirect(Shared.MySite.PublicPages.BookingDetail + "?k=" + hashkey + "&TransID=" + hID.Value.ToString(), false);
                                            return "";

                                            //commented by diana 20140211
                                            //if (HttpContext.Current.Session["dataClassTrans"] != null)
                                            //{
                                            //    UpdateSessiondataClassTrans("", paymentAmount, PNR);
                                            //}
                                        }
                                        else
                                        {
                                            status = errMessage;
                                        }
                                    }
                                    else
                                    {
                                        errMessage = msgList.Err100028;
                                    }
                                }
                                else
                                {
                                    status = errMessage;
                                }
                                break;
                            }
                        }
                        if (iCount == 0)
                        {
                            errMessage = msgList.Err100028;
                        }
                    }
                    else
                    {
                        errMessage = msgList.Err100028;
                    }
                }

                if (errMessage == "")
                {
                    //commented by diana 20140211
                    #region "Update Details"
                    //DataTable dtKeyField = new DataTable();
                    //if (HttpContext.Current.Session["dataClassTrans"] != null && PNRExist == false)
                    //{
                    //    dtKeyField = GetKeyFieldBySignature(sellSignature);
                    //}

                    //if (dtKeyField == null || dtKeyField.Rows.Count <= 0)
                    //{
                    //    dtKeyField = GetKeyFieldByPNR(PNR);
                    //}

                    //if (dtKeyField.Rows.Count > 0)
                    //{
                    //    decimal leftPayDetail = 0; //check total amount each sequence

                    //    for (int ctrDetail = 0; ctrDetail < dtKeyField.Rows.Count; ctrDetail++)
                    //    {
                    //        string RecordLocator = dtKeyField.Rows[ctrDetail]["RecordLocator"].ToString();
                    //        TransID = dtKeyField.Rows[ctrDetail]["TransID"].ToString();
                    //        byte SeqNo = Convert.ToByte(dtKeyField.Rows[ctrDetail]["SeqNo"]);

                    //        bookDTLInfo = objBooking.GetSingleBK_TRANSDTL(RecordLocator, TransID, SeqNo);

                    //        if (paymentAmount > bookDTLInfo.LineTotal)
                    //        {
                    //            leftPayDetail = paymentAmount - bookDTLInfo.LineTotal;
                    //            paymentAmount = bookDTLInfo.LineTotal;
                    //            AssignTransDetail(dtKeyField.Rows[ctrDetail]["RecordLocator"].ToString(), dtKeyField.Rows[ctrDetail]["TransID"].ToString(), Convert.ToByte(dtKeyField.Rows[ctrDetail]["SeqNo"]), MyUserSet.AgentName, paymentAmount + bookDTLInfo.CollectedAmount, PNR);

                    //            totalPaid += paymentAmount;
                    //            paymentAmount = leftPayDetail;
                    //        }
                    //        else
                    //        {
                    //            AssignTransDetail(dtKeyField.Rows[ctrDetail]["RecordLocator"].ToString(), dtKeyField.Rows[ctrDetail]["TransID"].ToString(), Convert.ToByte(dtKeyField.Rows[ctrDetail]["SeqNo"]), MyUserSet.AgentName, paymentAmount + bookDTLInfo.CollectedAmount, PNR);
                    //            totalPaid += paymentAmount;
                    //            paymentAmount = 0;
                    //        }
                    //    }
                    //}

                    ////update transmain
                    //DataTable dtKeyFieldMain = GetKeyFieldMainByTransID(TransID);
                    //DataTable dtTransMain = objBooking.dtTransMain();
                    //dtTransMain = (DataTable)HttpContext.Current.Session["TransMain"];
                    //TransTotal = Convert.ToDecimal(dtTransMain.Rows[0]["TotalTrans"]);
                    //decimal totalTrans = totalPaid + Convert.ToDecimal(dtTransMain.Rows[0]["CollectedAmt"]);
                    //byte TransStatus = 1;
                    //int PreviousStatus = Convert.ToInt16(dtTransMain.Rows[0]["TransStatus"]);
                    //if (totalTrans == TransTotal)
                    //{
                    //    TransStatus = 2;
                    //}
                    //if (dtKeyFieldMain.Rows.Count > 0)
                    //{
                    //    AssignTransMain(TransID, dtKeyFieldMain.Rows[0]["AgentID"].ToString(), dtKeyFieldMain.Rows[0]["AgentCatgID"].ToString(), Convert.ToByte(dtKeyFieldMain.Rows[0]["TransType"]), Convert.ToDateTime(dtKeyFieldMain.Rows[0]["BookingDate"]), TransStatus, MyUserSet.AgentName, 2, totalTrans, GetCurrency(), lblCurrentTotalCurrency.Text);
                    //}

                    ////update transaction tender
                    //switch (PaymentType)
                    //{
                    //    case "TabCredit":
                    //        if (txtCardNumber.Text != string.Empty)
                    //        { bookTransTenderInfo.RefNo = getEncryptedCredit(txtCardNumber.Text); }
                    //        else { bookTransTenderInfo.RefNo = ""; }
                    //        AssignPayment(hID.Value, Convert.ToByte(objBooking.getSeqByTransID(hID.Value, PNR)), totalPaid, lblCurrentTotalCurrency.Text, lblCurrentTotalCurrency.Text, objBooking.getTenderIDbyDesc(cmbCardType.SelectedItem.Value.ToString()), bookTransTenderInfo.RefNo, MyUserSet.AgentName, PNR, 0, "insert");
                    //        break;
                    //    case "TabAG":
                    //        AssignPayment(hID.Value, Convert.ToByte(objBooking.getSeqByTransID(hID.Value, PNR)), totalPaid, lblCurrentTotalCurrency.Text, lblCurrentTotalCurrency.Text, objBooking.getTenderIDbyDesc("AG"), "", MyUserSet.AgentName, PNR, 0, "insert");
                    //        break;
                    //}

                    ////execute batch
                    //if (lstbookDTLInfo.Count > 0 && errMessage == "")
                    //{
                    //    //added by diana 20140120 - to retrieve GroupName
                    //    GeneralControl objGeneral = new GeneralControl();
                    //    string GroupName = objGeneral.getOPTGroupByCarrierCode(lstbookDTLInfo[0].CarrierCode);

                    //    // begin amended by diana 20130918
                    //    if (totalTrans != TransTotal)
                    //    {
                    //        SetPayScheme();
                    //        PaymentInfo paymentInfo = new PaymentInfo();
                    //        PaymentControl objPay = new PaymentControl();

                    //        paymentInfo = objPay.GetPaymentScheme(PayScheme, GroupName); //amended by diana 20140120 - change GroupName from 'AA'

                    //        if (paymentInfo.Code_1 == "DOB")
                    //        {
                    //            bookHDRInfo.PaymentDateEx1 = bookHDRInfo.BookingDate.AddHours(paymentInfo.Attempt_1);
                    //            bookHDRInfo.ExpiryDate = bookHDRInfo.BookingDate.AddHours(paymentInfo.Attempt_1);
                    //        }
                    //        else if (paymentInfo.Code_1 == "STD")
                    //        {
                    //            bookHDRInfo.PaymentDateEx1 = bookHDRInfo.STDDate.AddHours(-paymentInfo.Attempt_1);
                    //            bookHDRInfo.ExpiryDate = bookHDRInfo.STDDate.AddHours(-paymentInfo.Attempt_1);
                    //        }
                    //        if (paymentInfo.Code_2 == "DOB")
                    //        {
                    //            bookHDRInfo.PaymentDateEx2 = bookHDRInfo.BookingDate.AddHours(paymentInfo.Attempt_2);
                    //        }
                    //        else if (paymentInfo.Code_2 == "STD")
                    //        {
                    //            bookHDRInfo.PaymentDateEx2 = bookHDRInfo.STDDate.AddHours(-paymentInfo.Attempt_2);
                    //        }
                    //        if (paymentInfo.Code_3 == "DOB")
                    //        {
                    //            bookHDRInfo.PaymentDateEx3 = bookHDRInfo.BookingDate.AddHours(paymentInfo.Attempt_3);
                    //        }
                    //        else if (paymentInfo.Code_3 == "STD")
                    //        {
                    //            bookHDRInfo.PaymentDateEx3 = bookHDRInfo.STDDate.AddHours(-paymentInfo.Attempt_3);
                    //        }
                    //    }
                    //    // end amended by diana 20130918
                    //    if (objBooking.SaveHeaderDetailTransFirst(bookHDRInfo, lstbookDTLInfo, bookTransTenderInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Update) == false)
                    //    {
                    //        status = msgList.Err100028; // transaction failed
                    //    }
                    //    else
                    //    {
                    //        string emailContact = txtContactEmail.Text;
                    //        string agentEmail = agent.Email;
                    //        if (agentEmail == null)
                    //            agentEmail = emailContact;
                    //        else if (agentEmail == "")
                    //            agentEmail = emailContact;

                    //        //send email deposit payment
                    //        //string msg = eServices.GroupBookingEmailing(4, agent.AgentID, agent.Email, TransID, Convert.ToDouble(totalPaid));
                    //        DateTime ExpiryDate = DateTime.Now;
                    //        decimal NextDueAmount = 0;
                    //        SetPayScheme();
                    //        if (objBooking.UpdateBookingDueDate(MyUserSet.AgentName, GroupName, PayScheme, PNR, TransID, ref ExpiryDate, ref NextDueAmount) == false)
                    //        {
                    //            log.Info(this,"update Booking Due Date Failed");
                    //        }
                    //        else
                    //        {
                    //            log.Info(this,"Entering (3)");
                    //            try
                    //            {
                    //                if (TransStatus == 1 && NextDueAmount > 0)
                    //                {
                    //                    string msg = eWS.GroupBookingEmailing(7, agent.AgentID, agentEmail, TransID, Convert.ToDouble(totalPaid), PNR);
                    //                    if (msg != "")
                    //                    {
                    //                        log.Info(msg);
                    //                    }
                    //                }
                    //                if (TransStatus == 2 && PreviousStatus != 3)
                    //                {
                    //                    //verify whole booking, cancel invalid PNRs


                    //                    string msg = eWS.GroupBookingEmailing(5, agent.AgentID, agentEmail, TransID, Convert.ToDouble(totalPaid), PNR);
                    //                    if (msg != "")
                    //                    {
                    //                        log.Info(msg);
                    //                    }

                    //                    List<BookingTransactionDetail> listDetailCombinePNR = new List<BookingTransactionDetail>();
                    //                    listDetailCombinePNR = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 0, "LEN(RecordLocator) >= 6 AND ");

                    //                    foreach (BookingTransactionDetail TransDetail in listDetailCombinePNR)
                    //                    {
                    //                        msg = eWS.GroupBookingEmailing(6, agent.AgentID, agentEmail, TransID, Convert.ToDouble(totalPaid), TransDetail.RecordLocator);
                    //                        if (msg != "")
                    //                        {
                    //                            log.Info(msg);
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                log.Info(this,"Email is error.");
                    //                log.Error(this,ex);
                    //            }
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    status = errMessage;
                    //}
                    #endregion
                }
                else
                {
                    status = errMessage;
                }

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this,ex);
                return msgList.Err100028;
            }
            finally
            {

            }
            if (status == "failed") status = msgList.Err100028; //added by diana 20140109 - show better text status
            if (status == "declined") status = msgList.Err100033; //added by diana 20140109 - show better text status
            return status;
        }

        public void sendEmail(string TransID)
        {
            string email = HttpContext.Current.Session["AgentEmail"].ToString();
            string PNR = HttpContext.Current.Session["PaidPNR"].ToString();
            int TransStatus = int.Parse(HttpContext.Current.Session["TransStatus"].ToString());
            string totalPaid = HttpContext.Current.Session["TotalPaid"].ToString();

            string emailContact = email;
            string agentEmail = agent.Email;
            if (agentEmail == null)
                agentEmail = emailContact;
            else if (agentEmail == "")
                agentEmail = emailContact;

            if (TransStatus == 1) // && NextDueAmount > 0
            {
                string msg = eWS.GroupBookingEmailing(7, agent.AgentID, agentEmail, TransID, Convert.ToDouble(totalPaid), PNR);
                if (msg != "")
                {
                    log.Warning(this,msg);
                }
            }
            if (TransStatus == 2) // && PreviousStatus != 3
            {
                //verify whole booking, cancel invalid PNRs


                string msg = eWS.GroupBookingEmailing(5, agent.AgentID, agentEmail, TransID, Convert.ToDouble(totalPaid), PNR);
                if (msg != "")
                {
                    log.Warning(this,msg);
                }

                List<BookingTransactionDetail> listDetailCombinePNR = new List<BookingTransactionDetail>();
                listDetailCombinePNR = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 0, "LEN(RecordLocator) >= 6 AND ");

                foreach (BookingTransactionDetail TransDetail in listDetailCombinePNR)
                {
                    msg = eWS.GroupBookingEmailing(6, agent.AgentID, agentEmail, TransID, Convert.ToDouble(totalPaid), TransDetail.RecordLocator);
                    if (msg != "")
                    {
                        log.Warning(this,msg);
                    }
                }
                ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), hID.Value.ToString(), "");

                Response.Redirect(Shared.MySite.PublicPages.BookingDetail + "?k=" + hashkey + "&TransID=" + TransID, false);

            }
        }

        public void CheckJourneyExist(string sellSignature)
        {
            //test to get booking response
            ABS.Navitaire.APIBooking api = new ABS.Navitaire.APIBooking("");
            ABS.Navitaire.BookingManager.Booking booking = new ABS.Navitaire.BookingManager.Booking();
            booking = api.GetBookingFromState(sellSignature);
            if (booking != null)
            {
                if (booking.Journeys.Length <= 0)
                {
                    log.Warning(this,"Journey is not exist : " + sellSignature);
                }
            }
            else
            {
                log.Warning(this,"Journey is not exist : " + sellSignature);
            }
            //end test to get booking reponse
        }

        public DataTable GetKeyFieldBySignature(string signature)
        {
            DataTable dt;
            dt = objBooking.GetKeyFieldDetailBySignature(signature);
            return dt;
        }

        public DataTable GetKeyFieldByPNR(string PNR)
        {
            DataTable dt;
            dt = objBooking.GetKeyFieldDetailByPNR(PNR);
            return dt;
        }

        public DataTable GetKeyFieldMainByTransID(string TransID)
        {
            DataTable dt;
            dt = objBooking.GetKeyFieldMainByTransID(TransID);
            return dt;
        }

        public void AssignTransMain(string transID, string agentID, string agentCatgID, byte TransType, DateTime bookingDate, byte transStatus, string agentName, byte reminderType, decimal collectAmt, string currency, string currencyPaid, int totalPax = 0, decimal totalTransAmount = 0, decimal totalTransFee = 0, decimal totalTransTax = 0, decimal totalTransOth = 0, decimal totalTransSub = 0)
        {
            bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(transID, TransType, agentID, agentCatgID, bookingDate);
            if (totalPax > 0)
                bookHDRInfo.TransTotalPAX = totalPax;
            if (totalTransAmount > 0)
                bookHDRInfo.TransTotalAmt = totalTransAmount;
            if (totalTransFee > 0)
                bookHDRInfo.TransTotalFee = totalTransFee;
            if (totalTransTax > 0)
                bookHDRInfo.TransTotalTax = totalTransTax;
            if (totalTransOth > 0)
                bookHDRInfo.TransTotalOth = totalTransOth;
            if (totalTransSub > 0)
                bookHDRInfo.TransSubTotal = totalTransSub;
            if (bookHDRInfo.TransStatus < transStatus)
                bookHDRInfo.TransStatus = transStatus;
            bookHDRInfo.SyncLastUpd = DateTime.Now;
            bookHDRInfo.LastSyncBy = agentName;
            bookHDRInfo.ReminderType = reminderType;
            bookHDRInfo.CollectedAmt = collectAmt;
            bookHDRInfo.Currency = currency;
            bookHDRInfo.CurrencyPaid = currencyPaid;
            bookHDRInfo.PaymentDate = DateTime.Now;
        }

        public void AssignPayment(string transID, byte seqNo, decimal payAmt, string currency, string currencyPaid, string tenderID, string feeType, string cardNumber, string agentName, string PNR, byte transVoid, string commandType)
        {
            bookTransTenderInfo = new BookingTransTender();
            bookTransTenderInfo.TransID = transID;
            bookTransTenderInfo.SeqNo = seqNo;
            bookTransTenderInfo.SyncCreate = DateTime.Now;
            bookTransTenderInfo.TenderAmt = payAmt;
            bookTransTenderInfo.TenderDue = payAmt;
            bookTransTenderInfo.TransDate = DateTime.Now;
            bookTransTenderInfo.Transvoid = transVoid;
            bookTransTenderInfo.ExchgRate = 1;
            bookTransTenderInfo.Currency = currency;
            bookTransTenderInfo.CurrencyPaid = currencyPaid;
            bookTransTenderInfo.TenderID = tenderID;

            string FeeTypeVal = "";
            if (feeType.Contains("#"))
            {
                string[] payText = feeType.Split('#');
                FeeTypeVal = payText[payText.Length - 1];
            }
            bookTransTenderInfo.FeeType = FeeTypeVal;

            bookTransTenderInfo.RefNo = cardNumber;
            bookTransTenderInfo.CreateBy = agentName;
            bookTransTenderInfo.RecordLocator = PNR;
            bookTransTenderInfo.CommandType = commandType;
        }

        public void AssignPaymentList(string transID, byte seqNo, decimal payAmt, decimal feeAmt, decimal totPayAmt, string currency, string currencyPaid, string tenderID, string feeType, string cardNumber, string agentName, string PNR, byte transVoid, string commandType, ref List<BookingTransTender> listTransTender)
        {

            BookingTransTender bookTransTenderInfos = new BookingTransTender();
            bookTransTenderInfos.TransID = transID;
            bookTransTenderInfos.SeqNo = seqNo;
            bookTransTenderInfos.SyncCreate = DateTime.Now;
            bookTransTenderInfos.TenderAmt = payAmt;
            bookTransTenderInfos.TenderDue = payAmt;
            bookTransTenderInfos.FeeAmt = feeAmt;
            bookTransTenderInfos.PayAmt = totPayAmt;
            bookTransTenderInfos.TransDate = DateTime.Now;
            bookTransTenderInfos.Transvoid = transVoid;
            bookTransTenderInfos.ExchgRate = 1;
            bookTransTenderInfos.Currency = currency;
            bookTransTenderInfos.CurrencyPaid = currencyPaid;
            bookTransTenderInfos.TenderID = tenderID;

            string FeeTypeVal = "";
            if (feeType.Contains("#"))
            {
                string[] payText = feeType.Split('#');
                FeeTypeVal = payText[payText.Length - 1];
            }
            bookTransTenderInfos.FeeType = FeeTypeVal;

            bookTransTenderInfos.RefNo = cardNumber;
            bookTransTenderInfos.CreateBy = agentName;
            bookTransTenderInfos.RecordLocator = PNR;
            bookTransTenderInfos.CommandType = commandType;
            listTransTender.Add(bookTransTenderInfos);
        }

        public void AssignTransDetail(string recordlocatordb, string transID, byte seqNo, string agentName, decimal collectedAmt, string pnr, byte transVoid = 0)
        {
            bookDTLInfo = objBooking.GetSingleBK_TRANSDTL(recordlocatordb, transID, seqNo);
            bookDTLInfo.TransVoid = transVoid;
            bookDTLInfo.LastSyncBy = agentName;
            bookDTLInfo.SyncLastUpd = DateTime.Now;
            bookDTLInfo.CollectedAmount = collectedAmt;
            bookDTLInfo.RecordLocator = pnr;


            lstbookDTLInfo.Add(bookDTLInfo);
        }

        //public Boolean UpdatePaymentDetails(string TransID, Boolean VerifyDetails = false)
        //{
        //    try
        //    {
        //        string PNR;
        //        Decimal totalPaid = 0;
        //        int totalPax = 0;
        //        decimal totalTransAmount = 0;
        //        decimal totalAmountGoing = 0;
        //        decimal totalAmountReturn = 0;
        //        decimal totalTransSubTotal = 0;
        //        decimal totalTransTotalFee = 0;
        //        decimal totalTransTotalTax = 0;
        //        decimal totalTransTotalOth = 0;

        //        List<PaymentContainer> listPaymentContainers = new List<PaymentContainer>();
        //        List<BookingTransTender> listBookTransTenderInfo = new List<BookingTransTender>();


        //        if (HttpContext.Current.Session["TransMain"] != null && HttpContext.Current.Session["TransDetail"] != null)
        //        {
        //            DataTable dtTransMain = objBooking.dtTransMain();
        //            if (HttpContext.Current.Session["TransMain"] != null)
        //                dtTransMain = (DataTable)HttpContext.Current.Session["TransMain"];
        //            DataTable dtTransDetail = objBooking.dtTransDetail();
        //            if (HttpContext.Current.Session["TransDetail"] != null)
        //                dtTransDetail = (DataTable)HttpContext.Current.Session["TransDetail"];

        //            List<BookingTransTender> listTransTender = new List<BookingTransTender>();
        //            listTransTender = objBooking.GetAllBK_TRANSTENDERFilter(TransID);

        //            lstbookDTLInfo = new List<BookingTransactionDetail>();

        //            //update transmain
        //            DataTable dtKeyFieldMain = GetKeyFieldMainByTransID(TransID);

        //            for (int i = 0; i < dtTransDetail.Rows.Count; i++)
        //            {
        //                PNR = dtTransDetail.Rows[i]["RecordLocator"].ToString();
        //                ////added by ketee, verify from daataclasstrans
        //                //if (PNR.Length < 6)
        //                //{
        //                //    if (HttpContext.Current.Session["dataClassTrans"] != null)
        //                //    {
        //                //        DataTable dataClass = objBooking.dtClassTrans();
        //                //        dataClass = (DataTable)HttpContext.Current.Session["dataClassTrans"];
        //                //        for (int k = 0; i < dataClass.Rows.Count; i++)
        //                //        {
        //                //            string sellSignature = dataClass.Rows[k]["SellSignature"].ToString();
        //                //            if (sellSignature == dtTransDetail.Rows[i]["SellKey"].ToString())
        //                //            {

        //                //            }
        //                //        }
        //                //    }
        //                //}

        //                if (objBooking.GetPaymentDetailsByPNR(PNR))
        //                {
        //                    //load all available record in 1 transaction id
        //                    if (VerifyDetails)
        //                    {
        //                        totalPax += Convert.ToInt16(dtTransDetail.Rows[i]["PaxAdult"].ToString()) + Convert.ToInt16(dtTransDetail.Rows[i]["PaxChild"].ToString());
        //                        totalTransAmount += Convert.ToDecimal(dtTransDetail.Rows[i]["LineTotal"].ToString());
        //                        totalTransTotalFee += Convert.ToDecimal(dtTransDetail.Rows[i]["ServiceCharge"].ToString());
        //                        totalTransTotalOth += Convert.ToDecimal(dtTransDetail.Rows[i]["OtherTax"].ToString());
        //                        totalTransTotalTax += Convert.ToDecimal(dtTransDetail.Rows[i]["ServiceTax"].ToString());
        //                        totalTransSubTotal += Convert.ToDecimal(dtTransDetail.Rows[i]["LineTotal"].ToString());
        //                    }
        //                    if (HttpContext.Current.Session["PaymentContainers"] != null)
        //                    {
        //                        listPaymentContainers = (List<PaymentContainer>)HttpContext.Current.Session["PaymentContainers"];
        //                        DataTable dtKeyField = new DataTable();
        //                        dtKeyField = GetKeyFieldByPNR(PNR);
        //                        decimal paymentAmount = 0, leftPayDetail = 0, totalPNRAmount = 0;

        //                        foreach (PaymentContainer rowPayment in listPaymentContainers)
        //                        {
        //                            int paymentStatus = 0;
        //                            string accountNumber = "";
        //                            long accountNumberID = 0;
        //                            byte transVoid = 1;
        //                            paymentAmount = rowPayment.CollectedAmount;
        //                            paymentStatus = rowPayment.PaymentStatus;
        //                            accountNumber = rowPayment.AccountNumber;
        //                            accountNumberID = rowPayment.AccountNumberID;
        //                            //totalPaid += paymentAmount;
        //                            if (rowPayment.PaymentStatus == 3)
        //                            {
        //                                transVoid = 0;
        //                                totalPaid += paymentAmount;
        //                                totalPNRAmount += paymentAmount;
        //                            }

        //                            int iIndex = listTransTender.FindIndex(p => p.SeqNo == Convert.ToInt32(rowPayment.PaymentNumber) && p.RecordLocator == rowPayment.RecordLocator);
        //                            if (iIndex >= 0)
        //                            {
        //                                BookingTransTender bookingTender = listTransTender[iIndex];
        //                                AssignPaymentList(TransID, Convert.ToByte(bookingTender.SeqNo), paymentAmount, rowPayment.CurrencyCode, rowPayment.CurrencyCode, bookingTender.TenderID, bookingTender.RefNo, bookingTender.CreateBy, PNR, transVoid, "update", ref listBookTransTenderInfo);
        //                            }
        //                            else
        //                            {
        //                                AssignPaymentList(TransID, Convert.ToByte(rowPayment.PaymentNumber), paymentAmount, rowPayment.CurrencyCode, rowPayment.CurrencyCode, objBooking.getTenderIDbyDesc(rowPayment.PaymentMethodCode), rowPayment.AccountNumber, MyUserSet.AgentName, PNR, transVoid, "insert", ref listBookTransTenderInfo);
        //                            }
        //                        }

        //                        for (int ctrDetail = 0; ctrDetail < dtKeyField.Rows.Count; ctrDetail++)
        //                        {
        //                            string RecordLocator = dtKeyField.Rows[ctrDetail]["RecordLocator"].ToString();
        //                            //TransID = dtKeyField.Rows[ctrDetail]["TransID"].ToString();
        //                            byte SeqNo = Convert.ToByte(dtKeyField.Rows[ctrDetail]["SeqNo"]);

        //                            bookDTLInfo = new BookingTransactionDetail();
        //                            bookDTLInfo = objBooking.GetSingleBK_TRANSDTL(RecordLocator, TransID, SeqNo);

        //                            if (totalPNRAmount > bookDTLInfo.LineTotal)
        //                            {
        //                                leftPayDetail = totalPNRAmount - bookDTLInfo.LineTotal;
        //                                totalPNRAmount = bookDTLInfo.LineTotal;
        //                                AssignTransDetail(dtKeyField.Rows[ctrDetail]["RecordLocator"].ToString(), dtKeyField.Rows[ctrDetail]["TransID"].ToString(), Convert.ToByte(dtKeyField.Rows[ctrDetail]["SeqNo"]), MyUserSet.AgentName, totalPNRAmount, PNR);
        //                                //totalPaid += paymentAmount;
        //                                totalPNRAmount = leftPayDetail;
        //                            }
        //                            else
        //                            {
        //                                AssignTransDetail(dtKeyField.Rows[ctrDetail]["RecordLocator"].ToString(), dtKeyField.Rows[ctrDetail]["TransID"].ToString(), Convert.ToByte(dtKeyField.Rows[ctrDetail]["SeqNo"]), MyUserSet.AgentName, totalPNRAmount, PNR);
        //                                //totalPaid += paymentAmount;
        //                                totalPNRAmount = 0;
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        return false;
        //                    }
        //                }
        //                else
        //                {
        //                    if (VerifyDetails)
        //                    {
        //                        DataTable dtKeyField = new DataTable();
        //                        dtKeyField = GetKeyFieldBySignature(dtTransDetail.Rows[i]["SellKey"].ToString());
        //                        //decimal paymentAmount = 0, leftPayDetail = 0, totalPNRAmount = 0;
        //                        for (int ctrDetail = 0; ctrDetail < dtKeyField.Rows.Count; ctrDetail++)
        //                        {
        //                            string RecordLocator = dtKeyField.Rows[ctrDetail]["RecordLocator"].ToString();
        //                            //TransID = dtKeyField.Rows[ctrDetail]["TransID"].ToString();
        //                            byte SeqNo = Convert.ToByte(dtKeyField.Rows[ctrDetail]["SeqNo"]);

        //                            bookDTLInfo = new BookingTransactionDetail();
        //                            bookDTLInfo = objBooking.GetSingleBK_TRANSDTL(RecordLocator, TransID, SeqNo);

        //                            //cancel PNR
        //                            objBooking.CancelTransactionByPNR(TransID, RecordLocator, MyUserSet.AgentID);
        //                            AssignTransDetail(dtKeyField.Rows[ctrDetail]["RecordLocator"].ToString(), dtKeyField.Rows[ctrDetail]["TransID"].ToString(), Convert.ToByte(dtKeyField.Rows[ctrDetail]["SeqNo"]), MyUserSet.AgentName, 0, PNR, 1);
        //                            //totalPaid += paymentAmount;
        //                            //totalPNRAmount = 0;


        //                        }
        //                    }
        //                }
        //            }

        //            //decimal TotalTrans = Convert.ToDecimal(dtTransMain.Rows[0]["TotalTrans"]);
        //            byte TransStatus = 1;
        //            if (totalTransAmount == totalPaid)
        //            {
        //                TransStatus = 2;
        //            }
        //            //if (totalPaid == TotalTrans)
        //            //{
        //            //    TransStatus = 2;
        //            //}
        //            if (dtKeyFieldMain.Rows.Count > 0)
        //            {
        //                AssignTransMain(TransID, dtKeyFieldMain.Rows[0]["AgentID"].ToString(), dtKeyFieldMain.Rows[0]["AgentCatgID"].ToString(), Convert.ToByte(dtKeyFieldMain.Rows[0]["TransType"]), Convert.ToDateTime(dtKeyFieldMain.Rows[0]["BookingDate"]), TransStatus, MyUserSet.AgentName, 2, totalPaid, GetCurrency(), GetCurrency(),totalPax, totalTransAmount, totalTransTotalFee, totalTransTotalTax, totalTransTotalOth, totalTransSubTotal);
        //            }

        //            if (lstbookDTLInfo.Count > 0 && bookHDRInfo != null && bookTransTenderInfo != null)
        //            {
        //                if (objBooking.UpdateHeaderDetailTrans(bookHDRInfo, lstbookDTLInfo, listBookTransTenderInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Update) == false)
        //                {
        //                    return false; // transaction failed
        //                }
        //                else
        //                {
        //                    return true;
        //                }
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        log.Error(this,ex);
        //        return false;
        //    }
        //    finally
        //    {

        //    }
        //}

        public string getEncryptedCredit(string ccNumber)
        {
            if (ccNumber.Length > 4)
            {
                string fourDigit = ccNumber.Substring(ccNumber.Length - 4, 4);
                string result = "";
                for (int i = 0; i < ccNumber.Length - 4; i++)
                {
                    result += "X";
                }
                result += fourDigit;
                return result;
            }
            else return ccNumber;
        }

        public ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse AGCredit()
        {
            try
            {
                string psDomain = ConfigurationManager.AppSettings["signature_domain"].ToString();
                string psName = ConfigurationManager.AppSettings["signature_username"].ToString();
                string psPwd = ConfigurationManager.AppSettings["signature_password"].ToString();

                string signature = absNavitaire.AgentLogon("public", psDomain, psName, psPwd);
                //ABS.Navitaire.AgentManager.FindAgentsResponse agentresp = absNavitaire.FindingAgent(MyUserSet.AgentName, signature);
                //string accountReference = agentresp.FindAgentResponseData.FindAgentList[0].AgentIdentifier.OrganizationCode;
                string accountReference = Session["OrganizationCode"].ToString();

                ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse accResp = absNavitaire.GetCreditByAccountReference(accountReference, lblCurrentTotalCurrency.Text, signature);
                return accResp;

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                return null;
            }
        }

        #endregion



    }
}