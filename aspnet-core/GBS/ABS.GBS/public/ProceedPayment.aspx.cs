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
using ABS.Navitaire.BookingManager;
using ABS.GBS.Log;
using ABS.GBS;
using StackExchange.Profiling;

namespace GroupBooking.Web
{
    public partial class ProceedPayment : System.Web.UI.Page
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
        BookingTransactionMain bookHDRInfos = new BookingTransactionMain();
        BookingTransTender bookTransTenderInfo = new BookingTransTender();
        List<PassengerData> lstPassenger = new List<PassengerData>();

        ABS.GBS.eService.ProcessServiceSoapClient eWS = new ABS.GBS.eService.ProcessServiceSoapClient();
        List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
        List<BookingTransactionDetail> lstbookDTLInfos = new List<BookingTransactionDetail>();
        ABS.Logic.GroupBooking.Agent.AgentProfile agent = new ABS.Logic.GroupBooking.Agent.AgentProfile();
        ABS.Logic.GroupBooking.Agent.AgentProfileControl objAgentProfile = new ABS.Logic.GroupBooking.Agent.AgentProfileControl();
        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
        List<BookingTransactionDetail> lstbookFlightInfo = new List<BookingTransactionDetail>();

        ABS.GBS.GetPlan GetPlan = new ABS.GBS.GetPlan();//added by Romy, 20170830, Insure
        PurchaseResponse RespPurchase = new PurchaseResponse();

        private static int num = 0;
        private static int departID = 0;
        private static string ReturnID = "";
        private static int no = 0;

        //added by ketee
        private string PayScheme;

        private string CountryCode = "";
        private string CurrencyCode = "";

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
            //return;
            //added by romy for profiler
            var profiler = MiniProfiler.Current;
            try
            {
                if (Session["reloadPage"] == null) //added by diana 20140605, avoid error of object reference
                {
                    Session["reloadPage"] = "";
                }

                //rdlPNR.Enabled = false;

                //added by diana 20131210 - for exception and invalid booking message
                HttpContext.Current.Session["ExceptionMessage"] = "";
                HttpContext.Current.Session["InvalidBooking"] = "false";

                contactState = txtContactState.Text;

                //rdlPNR.Style["visibility"] = "hidden";
                //txtPayAmount.Style["visibility"] = "hidden";

                lblMinPay.Text = txtMinPay.Text;

                Response.Buffer = true;
                Response.ExpiresAbsolute = DateTime.Now.AddDays(-1d);
                Response.Expires = -1500;
                Response.CacheControl = "no-cache";

                if (Session["AgentSet"] != null)
                { MyUserSet = (UserSet)Session["AgentSet"]; }
                else
                { Response.Redirect(Shared.MySite.PublicPages.AgentLogin, false); }

                //SessionContext sesscon = new SessionContext();

                if (!IsPostBack)
                {
                    using (profiler.Step("InitializeForm"))
                    {
                        InitializeForm();
                    }
                    ViewState["savedMinPay"] = null;

                    //insertCityValue();

                    using (profiler.Step("LoadProcessingFee"))
                    {
                        LoadProcessingFee();
                    }

                }
                else
                {
                    tblPayment.Style["visibility"] = "visible";
                    rdlPNR.Style["visibility"] = "visible";
                    txtPayAmount.Style["visibility"] = "visible";
                }

                //added by ketee, 20170210, to keep alive session id
                //KeepAlive();


                SetScreen();
                //InitializeForm();
                //CheckProcessingFee();
                //SetContactVisibility();
                if (Session["reloadPage"].ToString() == "true" && lblErrorBottom.Text == "")
                {
                    lblErrorBottom.Visible = true;
                    MessageList msgList = new MessageList();
                    lblErrorBottom.Text = msgList.Err100020;
                    imgError.Visible = true;
                }
                Session["reloadPage"] = "";
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //log.Error(this,ex);
                log.Error(this, ex);
            }

        }
        #endregion

        #region Control

        protected void TabControl_ActiveTabChanged(object sender, TabControlEventArgs e)
        {
            string tabname = TabControl.ActiveTabPage.Name;

            SelectState();
            if (tabActive.Value != "ag")
            {
                cbAllPNR.Checked = true;
                GeneratePNRsPaymentDetails();
            }
            else
            {
                tabActive.Value = "";
            }

            //switch (tabname)
            //{
            //    case "TabAG":
            //        for (int i = 0; i < cblPNR.Items.Count; i++)
            //        {
            //            if (cblPNR.Items[i].Text.IndexOf("No Min. Payment is Required") >= 0)
            //            {
            //                cblPNR.Set(i, true);
            //            }

            //        }
            //        break;
            //}

        }



        protected void rdlPNR_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkSelectedRadioPNR();
        }

        //Added by Ellis 20170307, to make it possible to use cookies for contact details
        protected void RememberMe_CheckedChanged(object sender, EventArgs e)
        {
            LoadContactDetails();
        }

        protected void btnProceedPayment_Click(object sender, EventArgs e)
        {
            MessageList msgList = new MessageList();
            var profiler = MiniProfiler.Current;
            try
            {
                if (hError.Value.ToString() == "1")
                {
                    //msgcontrol.MessageDisplay("Please fill in mandatory fields");
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

                    switch (tabname)
                    {
                        case "TabCredit":
                            if (Convert.ToDecimal(txtDueAmount.Text) < Convert.ToDecimal(txtPayAmount.Text))
                            {
                                //msgcontrol.MessageDisplay(msgList.Err100044);
                                lblErrorBottom.Text = msgList.Err100044;
                                imgError.Visible = true;
                                return;
                            }
                            break;
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

                        switch (tabname)
                        {
                            case "TabAG":
                                string minAmount = "";
                                for (int i = 0; i < cblPNR.Items.Count; i++)
                                {
                                    if (cblPNR.Items[i].Selected)
                                    {
                                        string[] s = cblPNR.Items[i].Value.ToString().Split(new char[] { ',' });
                                        if (s.Length > 1 && s[1].ToString() != "" && s[1].ToString().Trim().Length >= 6)
                                        {
                                            PNRExist = true;
                                            PNR = s[1].ToString();
                                        }
                                        else
                                        {
                                            PNRExist = false;
                                            PNR = s[0].ToString();
                                        }
                                        if (cbFullPayment.Checked)
                                        {
                                            string[] indexMinPay = cblPNR.Items[i].Text.ToString().Split(new char[] { ':' });
                                            if (indexMinPay.Length >= 0)
                                            {
                                                string[] indexAmount = indexMinPay[3].ToString().Split(new char[] { '(' });
                                                minAmount = indexAmount[0].ToString();
                                            }
                                        }
                                        else if (cblPNR.Items[i].Text.IndexOf("No Min. Payment is Required") < 0)
                                        {
                                            string[] indexMinPay = cblPNR.Items[i].Text.ToString().Split(new char[] { ':' });
                                            if (indexMinPay.Length >= 0)
                                            {
                                                string[] indexAmount = indexMinPay[4].ToString().Split(new char[] { '(' });
                                                minAmount = indexAmount[0].ToString();
                                            }
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                        using (profiler.Step("MakePayment"))
                                        {
                                            status = MakePayment(tabname, PNR, PNRExist, minAmount);
                                        }
                                    }
                                }
                                //added by romy for optimize
                                Session["AccResp"] = null;
                                break;
                            case "TabCredit":
                                string[] a = rdlPNR.SelectedItem.Value.ToString().Split(new char[] { ',' });
                                if (a.Length > 1 && a[1].ToString() != "" && a[1].ToString().Trim().Length >= 6)
                                {
                                    PNRExist = true;
                                    PNR = a[1].ToString();
                                }
                                else
                                {
                                    PNR = a[0].ToString();
                                }
                                using (profiler.Step("MakePayment"))
                                {
                                    status = MakePayment(tabname, PNR, PNRExist);
                                }
                                break;
                        }





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

                        log.Info(this, "Payment Status : " + status);

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
                                    log.Info(this, "Reload:" + status);
                                    Session["reloadPage"] = "true";
                                    Response.Redirect(Shared.MySite.PublicPages.Payment, false);
                                    return;
                                }

                                lblErrorBottom.Text = HttpContext.Current.Session["ExceptionMessage"] + status;
                                imgError.Visible = true;
                                log.Info(this, "Info show from checkPaymentAttempt - not clearing data");
                                Session["reloadPage"] = "";
                                using (profiler.Step("InitializeForm"))
                                {
                                    InitializeForm(false);
                                }

                                //added by diana 20140109 - if cc is declined, cannot proceed payment again
                                if (status == msgList.Err100046 || status == msgList.Err100047)
                                {
                                    tblPayment.Visible = false;
                                    TabControl.Visible = false;
                                    divContact.Visible = false;
                                    divCreditCard.Visible = false;
                                    divAG.Visible = false;
                                    btnProceedPayment.Visible = false;
                                    //Added by Ellis 20170316, if proceedpayment upper is hidden, then need hide bottom also
                                    btnProceedPaymentBottom.Visible = false;
                                }
                            }
                            else
                            {

                                //failedPayment(msgList.Err100034);
                                //msgcontrol.MessageDisplay(msgList.Err100028);

                                lblErrorBottom.Visible = true;
                                if (lblAmountPaid.Text == "0.00")
                                    lblErrorBottom.Text = msgList.Err999988;
                                else
                                    lblErrorBottom.Text = msgList.Err999989;
                                imgError.Visible = true;
                                Session["reloadPage"] = "";
                                InitializeForm(false);

                                tblPayment.Visible = false;
                                TabControl.Visible = false;
                                divContact.Visible = false;
                                divCreditCard.Visible = false;
                                divAG.Visible = false;
                                btnProceedPayment.Visible = false;
                                //Added by Ellis 20170316, if proceedpayment upper is hidden, then need hide bottom also
                                btnProceedPaymentBottom.Visible = false;

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
                            log.Info(this, "Payment succeed.");
                            Session["reloadPage"] = "";
                            HttpContext.Current.Session.Remove("PaymentAttempt");
                            HttpContext.Current.Session.Add("PaymentAttempt", 0);
                            //msgcontrol.MessageDisplay(msgList.Err100020);
                            lblErrorBottom.Visible = true;
                            lblErrorBottom.Text = msgList.Err100020;
                            imgError.Visible = true;
                            //Added by Ellis 20170307, to make it possible to use cookies for contact details
                            if (chkRememberMe.Checked)
                            {
                                Response.Cookies["RememberMe"]["AgentID"] = MyUserSet.AgentID;
                                Response.Cookies["RememberMe"]["Title"] = cmbContactTitle.Value.ToString();
                                Response.Cookies["RememberMe"]["FirstName"] = txtContactFirstName.Text;
                                Response.Cookies["RememberMe"]["LastName"] = txtContactLastName.Text;
                                Response.Cookies["RememberMe"]["Email"] = txtContactEmail.Text;
                                Response.Cookies["RememberMe"]["PhoneNo"] = txtContactPhone.Text;
                                Response.Cookies["RememberMe"]["Address1"] = txtContactAddress.Text;
                                Response.Cookies["RememberMe"]["Address2"] = txtContactAddress2.Text;
                                Response.Cookies["RememberMe"]["City"] = txtContactTown.Text;
                                Response.Cookies["RememberMe"]["Country"] = cmbContactCountryAddress.Value.ToString();
                                Response.Cookies["RememberMe"]["State"] = cmbContactState.Value.ToString();
                                Response.Cookies["RememberMe"]["PostCode"] = txtContactZipCode.Text;
                                //Added by Tyas 20170609
                                Response.Cookies["RememberMe"]["OrgID"] = MyUserSet.OrganicationID;
                            }
                            else
                            {
                                if (Request.Cookies["RememberMe"] != null)
                                {
                                    Response.Cookies["RememberMe"].Expires = DateTime.Now.AddDays(-1);
                                }
                            }
                            //End of Added by Ellis 20170307
                            using (profiler.Step("InitializeForm"))
                            {
                                InitializeForm(true);
                            }

                            //20170616 - Sienny (refresh amount due on left-side bar)
                            Response.Redirect(HttpContext.Current.Request.Path, false);

                            SetScreen();
                            no = 0;
                        }

                    }
                    catch (Exception ex)
                    {
                        SystemLog.Notifier.Notify(ex);
                        log.Error(this, ex);
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
                    using (profiler.Step("CheckPaymentAttempt"))
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
                }
                //}
            }
            catch (ApplicationException ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);

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
            MessageList msgList = new MessageList();
            if (rdlPNR.SelectedIndex >= 0)
            {
                Boolean PNRExist = false;
                string PNR = "";
                log.Info(this, "PNR radio selection is " + rdlPNR.SelectedItem.Value.ToString());

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
                txtMinPay.Text = ValidateMinAmount(hID.Value, PNR, PNRExist).ToString("N", nfi);
                lblMinPay.Text = lblMinPay.Text;

                if (Convert.ToDecimal(txtMinPay.Text) <= 0)
                {
                    lblMinCap.Text = msgList.Err100069;
                    lblMinPay.Text = "";
                    txtMinPay.Text = "";
                }
                //txtPayAmount.Text = rdlPNR.SelectedItem.Text;
                txtPayAmount.Text = ValidateMinAmount(hID.Value, PNR, PNRExist, true).ToString("N", nfi).Replace(",", "");
                //added by ketee, 20170118
                ////decimal totalamountpay = Convert.ToDecimal(lblProcessFee.Text.ToString()) + Convert.ToDecimal(txtPayAmount.Text.ToString());
                ////lblTotalAmountPay.Text = totalamountpay.ToString("N", nfi);
            }
        }

        public void InitializeForm(Boolean Validate = false)
        {
            MessageList msgList = new MessageList();
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;

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
            lblMinCap.Text = msgList.Err100070;
            lblMinPay.Text = "0.00";
            txtMinPay.Text = "0.00";
            txtDueAmount.Text = "0.00";
            lblFullPay.Text = "0.00";
            //added by ketee, 20170118, validate if not check full payment, not need to reset payment amount,
            if (cbFullPayment.Checked == false) txtPayAmount.Text = "";
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

                if (Int32.TryParse(cookie.Values["list1ID"], out departID) == false)
                {
                    departID = -1;
                }

                ReturnID = cookie.Values["ReturnID"];
                num = Convert.ToInt32(cookie.Values["PaxNum"]);
            }
            else
                cookie = null;

            if (HttpContext.Current.Session["HashMain"] != null)
            {
                Hashtable ht = (Hashtable)HttpContext.Current.Session["HashMain"];
                hID.Value = ht["TransID"].ToString();
                hIsOverride.Value = ht["IsOverride"].ToString();
            }
            else if (HttpContext.Current.Session["ChgTransMain"] != null)
            {
                dtTransMain = (DataTable)HttpContext.Current.Session["ChgTransMain"];
                hID.Value = dtTransMain.Rows[0]["TransID"].ToString();
                hIsOverride.Value = dtTransMain.Rows[0]["IsOverride"].ToString();
            }
            //else if (HttpContext.Current.Session["ChgbookHDRInfo"] != null)
            //{
            //    bookHDRInfo = (BookingTransactionMain)HttpContext.Current.Session["ChgbookHDRInfo"];
            //    hID.Value = bookHDRInfo.TransID;
            //    hIsOverride.Value =  bookHDRInfo.IsOverride.ToString();
            //}
            else if (HttpContext.Current.Session["TransMain"] != null)
            {
                dtTransMain = (DataTable)HttpContext.Current.Session["TransMain"];
                hID.Value = dtTransMain.Rows[0]["TransID"].ToString();
                hIsOverride.Value = dtTransMain.Rows[0]["IsOverride"].ToString();
            }

            //get latest transmain info
            if (hID.Value.ToString() != "")
            {
                using (profiler.Step("LoadPaymentSchedule"))
                {
                    LoadPaymentSchedule(hID.Value);
                }
                //LoadData(hID.Value);
                //LoadRightPanel(hID.Value);
                //update transaction details

                if (Session["reloadPage"] != null)
                {
                    if (Session["reloadPage"].ToString() == "true")
                    {
                        using (profiler.Step("CheckInvalidPNRByTransID"))
                        {
                            objBooking.CheckInvalidPNRByTransID(hID.Value, true);
                        }
                        //add param value true - in testing
                        using (profiler.Step("UpdatePaymentDetails"))
                        {
                            objBooking.UpdatePaymentDetails(hID.Value, MyUserSet.AgentName, MyUserSet.AgentID, true);
                        }

                        Session["reloadPage"] = "";
                    }
                }
                else
                {
                    Session["reloadPage"] = "";
                }


                if (Session["reloadPage"].ToString() == "true" && Session["ChgMode"] == null)
                {
                    using (profiler.Step("sendEmail"))
                    {
                        sendEmail(hID.Value);
                    }
                }
                else
                {
                    if (IsPostBack)
                    {
                        using (profiler.Step("UpdatePaymentDetails"))
                        {
                            objBooking.UpdatePaymentDetails(hID.Value, MyUserSet.AgentName, MyUserSet.AgentID);
                        }
                    }

                }

                //amended by diana 20170324, load data only if not in change mode
                if (HttpContext.Current.Session["ChgTransMain"] == null)
                {
                    LoadData(hID.Value);
                }

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

            if (HttpContext.Current.Session["ChgTransMain"] != null)
            {
                dtTransMain = (DataTable)HttpContext.Current.Session["ChgTransMain"];
                totalDepart = Convert.ToDecimal(dtTransMain.Rows[0]["TotalDepart"]);
                totalReturn = Convert.ToDecimal(dtTransMain.Rows[0]["TotalReturn"]);
                currency = dtTransMain.Rows[0]["Currency"].ToString();
                totalAmount = Convert.ToDecimal(dtTransMain.Rows[0]["TotalTrans"]);

                lblCurrentTotal.Text = totalAmount.ToString("N", nfi);

                lblCurrentTotalCurrency.Text = currency;
                lblAmountPaid.Text = objGeneral.RoundUp(Convert.ToDecimal(dtTransMain.Rows[0]["CollectedAmt"])).ToString("N", nfi);
                lblAmountDue.Text = objGeneral.RoundUp(Convert.ToDecimal(dtTransMain.Rows[0]["TotalDue"])).ToString("N", nfi);
                lblAmountPaidCurrency.Text = dtTransMain.Rows[0]["CurrencyPaid"].ToString();
                lblAmountDueCurrency.Text = currency;
                lblDepartTotal.Text = totalDepart.ToString("N", nfi);
                lblDepartTotalCurrency.Text = currency;
                lblReturnTotal.Text = totalReturn.ToString("N", nfi);
                lblReturnTotalCurrency.Text = currency;

                //20170530 - Sienny (put amount due to session)
                Session["TotalAmountDue"] = lblAmountDue.Text;
                Session["TotalAmountDueCurr"] = lblAmountDueCurrency.Text;

                hfAmountDue.Value = lblAmountDue.Text;

                lblTransID.Text = dtTransMain.Rows[0]["TransID"].ToString();
                lblTransactionID.Text = dtTransMain.Rows[0]["TransID"].ToString();
                using (profiler.Step("GeneratePNRsPaymentDetails"))
                {
                    GeneratePNRsPaymentDetails(Validate);
                }
            }

            else if (HttpContext.Current.Session["TransMain"] != null)
            {
                dtTransMain = (DataTable)HttpContext.Current.Session["TransMain"];
                totalDepart = Convert.ToDecimal(dtTransMain.Rows[0]["TotalDepart"]);
                totalReturn = Convert.ToDecimal(dtTransMain.Rows[0]["TotalReturn"]);
                currency = dtTransMain.Rows[0]["Currency"].ToString();
                //totalAmount = totalDepart + totalReturn;
                totalAmount = Convert.ToDecimal(dtTransMain.Rows[0]["TotalTrans"]);

                lblCurrentTotal.Text = totalAmount.ToString("N", nfi);

                lblCurrentTotalCurrency.Text = currency;
                lblAmountPaid.Text = objGeneral.RoundUp(Convert.ToDecimal(dtTransMain.Rows[0]["CollectedAmt"])).ToString("N", nfi);
                lblAmountDue.Text = objGeneral.RoundUp(Convert.ToDecimal(dtTransMain.Rows[0]["TotalDue"])).ToString("N", nfi);// +" " + dtTransMain.Rows[0]["Currency"].ToString();
                ////lblAGCreditCurrency.Text = currency;
                lblAmountPaidCurrency.Text = dtTransMain.Rows[0]["CurrencyPaid"].ToString();
                lblAmountDueCurrency.Text = currency;
                lblDepartTotal.Text = totalDepart.ToString("N", nfi);
                lblDepartTotalCurrency.Text = currency;
                lblReturnTotal.Text = totalReturn.ToString("N", nfi);
                lblReturnTotalCurrency.Text = currency;

                //20170530 - Sienny (put amount due to session)
                Session["TotalAmountDue"] = lblAmountDue.Text;
                Session["TotalAmountDueCurr"] = lblAmountDueCurrency.Text;

                hfAmountDue.Value = lblAmountDue.Text;

                lblTransID.Text = dtTransMain.Rows[0]["TransID"].ToString();
                lblTransactionID.Text = dtTransMain.Rows[0]["TransID"].ToString();
                using (profiler.Step("GeneratePNRsPaymentDetails"))
                {
                    GeneratePNRsPaymentDetails(Validate);
                }

                //HttpContext.Current.Session["TransMain"] = dtTransMain;
                //set merchant payment gateway
                //SetPaymentGateWay(currency);
            }

            //added by diana 20131116 - if status = 1 then hide contact details
            //SetContactVisibility();
            //end added by diana 20131116 - if status = 1 then hide contact details

        }



        protected void LoadData(string TransID)
        {
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;

            List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
            List<BookingTransactionDetail> listDetailCombinePNR = new List<BookingTransactionDetail>();

            using (profiler.Step("GetSingleBK_TRANSMAIN"))
            {
                bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(TransID);
            }
            if (bookHDRInfo == null)
            {
                bookHDRInfo = (BookingTransactionMain)Session["ChgbookHDRInfo"];
            }
            using (profiler.Step("FillDataTableTransMain"))
            {
                objBooking.FillDataTableTransMain(bookHDRInfo);
            }

            //20170419 - Sienny (get info total pax)
            lbl_num.Text = bookHDRInfo.TransTotalPAX.ToString();

            //amended by diana 20131102 - hide disapproved record locator
            if (Session["NewBooking"].ToString() == "true")
                using (profiler.Step("GetAllBK_TRANSDTLCombinePNR"))
                {
                    listDetailCombinePNR = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 0);
                }
            else
                using (profiler.Step("GetAllBK_TRANSDTLCombinePNR"))
                {
                    listDetailCombinePNR = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 0, "LEN(RecordLocator) >= 6 AND ");
                }

            if (listDetailCombinePNR == null)
            {
                listDetailCombinePNR = (List<BookingTransactionDetail>)Session["listBookingDetailCombine"];
            }
            using (profiler.Step("FillDataTableTransDetail"))
            {
                objBooking.FillDataTableTransDetail(listDetailCombinePNR);
            }
        }

        protected void LoadPaymentSchedule(string TransID)
        {
            MessageList msgList = new MessageList();
            List<BookingTransactionDetail> listDetailCombinePNR = new List<BookingTransactionDetail>();
            if (Session["listBookingDetailCombine"] != null)
            {
                listDetailCombinePNR = (List<BookingTransactionDetail>)Session["listBookingDetailCombine"];
                foreach (BookingTransactionDetail bookDTLInfo in listDetailCombinePNR)
                {
                    string strHTML = "";
                    strHTML += "<table class='table table-bordered'><tr><td><div class='labelDate'>Payment Expiry Date</div></td><td><span class='labelDate'>Min.Payment</span></td><td><span class='labelDate'>Status</span></td></tr>";

                    if (bookDTLInfo.PayDueAmount1 != 0)
                    {
                        strHTML += "<tr><td>";
                        strHTML += "<div class='labelDate'>" + String.Format("{0:MMM dd, yyyy HH:mm}", bookDTLInfo.PayDueDate1) + "</div></td>";
                        strHTML += "<td><span class='labelDate'>" + String.Format("{0:#,0.00}", bookDTLInfo.PayDueAmount1) + " " + bookDTLInfo.Currency + "</span></td>";
                        strHTML += "<td><span class='labelDate'>";
                        if (bookDTLInfo.CollectedAmount >= (bookDTLInfo.PayDueAmount1))
                            strHTML += msgList.Err100071;
                        else
                            strHTML += msgList.Err100072;
                        strHTML += "</span></td></tr>";
                    }
                    if (bookDTLInfo.PayDueAmount2 != 0)
                    {
                        strHTML += "<tr><td>";
                        strHTML += "<div class='labelDate'>" + String.Format("{0:MMM dd, yyyy HH:mm}", bookDTLInfo.PayDueDate2) + "</div></td>";
                        strHTML += "<td><span class='labelDate'>" + String.Format("{0:#,0.00}", bookDTLInfo.PayDueAmount2) + " " + bookDTLInfo.Currency + "</span></td>";
                        strHTML += "<td><span class='labelDate'>";
                        if (bookDTLInfo.CollectedAmount >= (bookDTLInfo.PayDueAmount1 + bookDTLInfo.PayDueAmount2))
                            strHTML += msgList.Err100071;
                        else
                            strHTML += msgList.Err100072;
                        strHTML += "</span></td></tr>";
                    }
                    if (bookDTLInfo.PayDueAmount3 != 0)
                    {
                        strHTML += "<tr><td>";
                        strHTML += "<div class='labelDate'>" + String.Format("{0:MMM dd, yyyy HH:mm}", bookDTLInfo.PayDueDate3) + "</div></td>";
                        strHTML += "<td><span class='labelDate'>" + String.Format("{0:#,0.00}", bookDTLInfo.PayDueAmount3) + " " + bookDTLInfo.Currency + "</span></td>";
                        strHTML += "<td><span class='labelDate'>";
                        if (bookDTLInfo.CollectedAmount >= (bookDTLInfo.PayDueAmount1 + bookDTLInfo.PayDueAmount2 + bookDTLInfo.PayDueAmount3))
                            strHTML += msgList.Err100071;
                        else
                            strHTML += msgList.Err100072;
                        strHTML += "</span></td></tr>";
                    }

                    //to show additional charges
                    //decimal AddCharge = bookDTLInfo.LineTotal - bookDTLInfo.PayDueAmount1 - bookDTLInfo.PayDueAmount2 - bookDTLInfo.PayDueAmount3;
                    //if (AddCharge != 0)
                    //{
                    //    strHTML += "<tr><td>";
                    //    strHTML += "<div class='labelDate'>" + "Additional Charges" + "</div></td>";
                    //    strHTML += "<td><span class='labelDate'>" + String.Format("{0:#,0.00}", AddCharge) + " " + bookDTLInfo.Currency + "</span></td>";
                    //    strHTML += "<td><span class='labelDate'>" + msgList.Err100071;
                    //    strHTML += "</span></td></tr>";
                    //}
                    if ((bookDTLInfo.LineTotal - (bookDTLInfo.PayDueAmount1 + bookDTLInfo.PayDueAmount2 + bookDTLInfo.PayDueAmount3)) != 0)
                    {
                        strHTML += "<tr><td>";
                        strHTML += "<div class='labelDate'>" + "Additional Charges" + "</div></td>";
                        strHTML += "<td><span class='labelDate'>" + String.Format("{0:#,0.00}", (bookDTLInfo.CollectedAmount - (bookDTLInfo.PayDueAmount1 + bookDTLInfo.PayDueAmount2 + bookDTLInfo.PayDueAmount3))) + " " + bookDTLInfo.Currency + "</span></td>";
                        strHTML += "<td><span class='labelDate'>";
                        strHTML += msgList.Err100071;
                        strHTML += "</span></td></tr>";

                        strHTML += "<tr><td>";
                        strHTML += "<div class='labelDate'>" + "Immediate Payment" + "</div></td>";
                        strHTML += "<td><span class='labelDate'>" + String.Format("{0:#,0.00}", (bookDTLInfo.LineTotal - bookDTLInfo.CollectedAmount)) + " " + bookDTLInfo.Currency + "</span></td>";
                        strHTML += "<td><span class='labelDate'>";
                        strHTML += msgList.Err100072;
                        strHTML += "</span></td></tr>";

                    }
                    strHTML += "</table>";

                    divPaymentSchedule.InnerHtml = strHTML;
                }
            }
            else
            {

                if (HttpContext.Current.Session["bookingMain"] != null)
                {
                    bookHDRInfo = (BookingTransactionMain)HttpContext.Current.Session["bookingMain"];
                }
                else
                {
                    bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(TransID);
                }

                //added by diana 20170518, to go back to review booking page if there is no payment schedule
                if (bookHDRInfo.TransStatus == 0 && bookHDRInfo.PaymentAmtEx1 <= 0)
                {
                    Response.Redirect(Shared.MySite.PublicPages.ReviewBooking, false);
                }

                string strHTML = "";
                strHTML += "<table class='table table-bordered'><tr><td><div class='labelDate'>Payment Expiry Date</div></td><td><span class='labelDate'>Min.Payment</span></td><td><span class='labelDate'>Status</span></td></tr>";

                if (bookHDRInfo.PaymentAmtEx1 != 0)
                {
                    strHTML += "<tr><td>";
                    strHTML += "<div class='labelDate'>" + String.Format("{0:MMM dd, yyyy HH:mm}", bookHDRInfo.PaymentDateEx1) + "</div></td>";
                    strHTML += "<td><span class='labelDate'>" + String.Format("{0:#,0.00}", bookHDRInfo.PaymentAmtEx1) + " " + bookHDRInfo.Currency + "</span></td>";
                    strHTML += "<td><span class='labelDate'>";
                    if (bookHDRInfo.CollectedAmt >= (bookHDRInfo.PaymentAmtEx1))
                        strHTML += msgList.Err100071;
                    else
                        strHTML += msgList.Err100072;
                    strHTML += "</span></td></tr>";
                }
                if (bookHDRInfo.PaymentAmtEx2 != 0)
                {
                    strHTML += "<tr><td>";
                    strHTML += "<div class='labelDate'>" + String.Format("{0:MMM dd, yyyy HH:mm}", bookHDRInfo.PaymentDateEx2) + "</div></td>";
                    strHTML += "<td><span class='labelDate'>" + String.Format("{0:#,0.00}", bookHDRInfo.PaymentAmtEx2) + " " + bookHDRInfo.Currency + "</span></td>";
                    strHTML += "<td><span class='labelDate'>";
                    if ((bookHDRInfo.CollectedAmt - bookHDRInfo.ForfeitedAmount) >= (bookHDRInfo.PaymentAmtEx1 + bookHDRInfo.PaymentAmtEx2))
                        strHTML += msgList.Err100071;
                    else
                        strHTML += msgList.Err100072;
                    strHTML += "</span></td></tr>";
                }
                if (bookHDRInfo.PaymentAmtEx3 != 0)
                {
                    strHTML += "<tr><td>";
                    strHTML += "<div class='labelDate'>" + String.Format("{0:MMM dd, yyyy HH:mm}", bookHDRInfo.PaymentDateEx3) + "</div></td>";
                    strHTML += "<td><span class='labelDate'>" + String.Format("{0:#,0.00}", bookHDRInfo.PaymentAmtEx3) + " " + bookHDRInfo.Currency + "</span></td>";
                    strHTML += "<td><span class='labelDate'>";
                    if ((bookHDRInfo.CollectedAmt - bookHDRInfo.ForfeitedAmount) >= (bookHDRInfo.PaymentAmtEx1 + bookHDRInfo.PaymentAmtEx2 + bookHDRInfo.PaymentAmtEx3))
                        strHTML += msgList.Err100071;
                    else
                        strHTML += msgList.Err100072;
                    strHTML += "</span></td></tr>";
                }

                decimal forfeitedAmount = 0;
                decimal AddCharge = 0;
                //if ((bookHDRInfo.CollectedAmt - bookHDRInfo.ForfeitedAmount) >= (bookHDRInfo.PaymentAmtEx1 + bookHDRInfo.PaymentAmtEx2 + bookHDRInfo.PaymentAmtEx3))
                //{
                //    AddCharge = bookHDRInfo.TransTotalAmt - bookHDRInfo.PaymentAmtEx1 - bookHDRInfo.PaymentAmtEx2 - bookHDRInfo.PaymentAmtEx3 - (bookHDRInfo.TransTotalAmt - bookHDRInfo.CollectedAmt);
                //}
                //else
                //{
                AddCharge = bookHDRInfo.TransTotalAmt - bookHDRInfo.PaymentAmtEx1 - bookHDRInfo.PaymentAmtEx2 - bookHDRInfo.PaymentAmtEx3;
                //}

                if (bookHDRInfo.ForfeitedAmount != 0)
                {
                    forfeitedAmount = bookHDRInfo.ForfeitedAmount;
                    AddCharge = AddCharge - forfeitedAmount;
                }
                if (AddCharge != 0 && bookHDRInfo.TransTotalAmt > bookHDRInfo.CollectedAmt)
                {

                    if (Session["ChgMode"] == null)
                    {
                        strHTML += "<tr><td>";
                        strHTML += "<div class='labelDate'>" + "Additional Charges" + "</div></td>";
                        strHTML += "<td><span class='labelDate'>" + String.Format("{0:#,0.00}", AddCharge) + " " + bookHDRInfo.Currency + "</span></td>";
                        strHTML += "<td><span class='labelDate'>";
                        strHTML += msgList.Err100071;
                        strHTML += "</span></td></tr>";

                        if ((bookHDRInfo.TransTotalAmt - bookHDRInfo.CollectedAmt) > 0 && (bookHDRInfo.CollectedAmt - bookHDRInfo.ForfeitedAmount) >= (bookHDRInfo.PaymentAmtEx1 + bookHDRInfo.PaymentAmtEx2 + bookHDRInfo.PaymentAmtEx3))
                        {
                            strHTML += "<tr><td>";
                            strHTML += "<div class='labelDate'>" + "Pending for Payment" + "</div></td>";
                            strHTML += "<td><span class='labelDate'>" + String.Format("{0:#,0.00}", (bookHDRInfo.TransTotalAmt - bookHDRInfo.CollectedAmt)) + " " + bookHDRInfo.Currency + "</span></td>";
                            strHTML += "<td><span class='labelDate'>" + msgList.Err100072;
                            strHTML += "</span></td></tr>";
                        }

                    }
                    else
                    {
                        if ((bookHDRInfo.CollectedAmt - bookHDRInfo.ForfeitedAmount - bookHDRInfo.PaymentAmtEx1 - bookHDRInfo.PaymentAmtEx2 - bookHDRInfo.PaymentAmtEx3) > 0)
                        {
                            strHTML += "<tr><td>";
                            strHTML += "<div class='labelDate'>" + "Additional Charges" + "</div></td>";
                            strHTML += "<td><span class='labelDate'>" + String.Format("{0:#,0.00}", (bookHDRInfo.CollectedAmt - bookHDRInfo.ForfeitedAmount - bookHDRInfo.PaymentAmtEx1 - bookHDRInfo.PaymentAmtEx2 - bookHDRInfo.PaymentAmtEx3)) + " " + bookHDRInfo.Currency + "</span></td>";
                            strHTML += "<td><span class='labelDate'>";
                            strHTML += msgList.Err100071;
                            strHTML += "</span></td></tr>";
                        }

                        strHTML += "<tr><td>";
                        strHTML += "<div class='labelDate'>" + "Immediate Payment" + "</div></td>";
                        strHTML += "<td><span class='labelDate'>" + String.Format("{0:#,0.00}", (bookHDRInfo.TransTotalAmt - bookHDRInfo.CollectedAmt)) + " " + bookHDRInfo.Currency + "</span></td>";
                        strHTML += "<td><span class='labelDate'>";
                        strHTML += msgList.Err100072;
                        strHTML += "</span></td></tr>";
                    }
                    if (forfeitedAmount != 0)
                    {
                        strHTML += "<tr><td>";
                        strHTML += "<div class='labelDate'>" + "Forfeited Amount" + "</div></td>";
                        strHTML += "<td><span class='labelDate'>" + String.Format("{0:#,0.00}", forfeitedAmount) + " " + bookHDRInfo.Currency + "</span></td>";
                        strHTML += "<td><span class='labelDate'>" + msgList.Err100071;
                        strHTML += "</span></td></tr>";
                    }

                    strHTML += "</span></td></tr>";
                }

                strHTML += "</table>";
                divPaymentSchedule.InnerHtml = strHTML;
            }

        }

        private void GeneratePNRsPaymentDetails(Boolean Validate = false)
        {
            MessageList msgList = new MessageList();
            var profiler = MiniProfiler.Current;
            DataTable dataClass = objBooking.dtClass();
            //clear list PNR
            rdlPNR.Items.Clear();
            cblPNR.Items.Clear();
            rdlContactPNR.Items.Clear();
            rdlProcessFee.Items.Clear();
            string tabname = TabControl.ActiveTabPage.Name;

            //added by diana 20131118 - to get agent profile from db
            agent = (ABS.Logic.GroupBooking.Agent.AgentProfile)Session["agProfileInfo"];

            Boolean AllPNRExist = true;

            //added by aian 2017324, change mode = use session ChgTransDetail
            if (HttpContext.Current.Session["ChgTransDetail"] != null)
            {
                DataTable dtDetail = objBooking.dtTransDetail();
                dtDetail = (DataTable)HttpContext.Current.Session["ChgTransDetail"];
                if (dtDetail.Select("LEN(TRIM([RecordLocator])) < 6").Count() > 0)
                {
                    AllPNRExist = false;
                }
            }
            else if (HttpContext.Current.Session["TransDetail"] != null)
            {
                DataTable dtDetail = objBooking.dtTransDetail();
                dtDetail = (DataTable)HttpContext.Current.Session["TransDetail"];

                if (dtDetail.Select("LEN(TRIM([RecordLocator])) < 6").Count() > 0)
                {
                    AllPNRExist = false;

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

                    DataTable dtClass = dataClass.Clone();
                    dtClass.Columns["Quantity"].DataType = typeof(Int32);
                    dtClass.Columns["FullPrice"].DataType = typeof(Decimal);
                    foreach (DataRow row in dataClass.Rows)
                    {
                        dtClass.ImportRow(row);
                    }

                    DataView dataView = new DataView(dtClass);
                    dataView.Sort = "Quantity DESC, FullPrice DESC";
                    //dataView.Sort = "Quantity ASC";
                    dataClass = dataView.ToTable();

                    decimal AmountPaid = 0;
                    decimal TotalAmount = 0;
                    string param = ",";
                    Boolean EmptyPNR = false;

                    //added by diana 20131106 - check whether any transdtl still do not have PNR
                    Boolean leftEmptyPNR = false;

                    //cityValue = new string[dataClass.Rows.Count];
                    //cityText = new string[dataClass.Rows.Count];

                    int iIndex = 0;
                    int MaxPax = 0;
                    decimal MaxAmount = 0;
                    decimal MinPay = 0;
                    decimal AmountDue = 0;
                    bool IsPrevEmpty = false;

                    bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(hID.Value);

                    objBooking.FillDataTableTransMain(bookHDRInfo);

                    for (int i = 0; i < dataClass.Rows.Count; i++)
                    {
                        string PNR = "To Be Confirmed";

                        AmountDue = Convert.ToDecimal(dataClass.Rows[i]["FullPrice"].ToString()) - Convert.ToDecimal(dataClass.Rows[i]["DetailCollectedAmt"].ToString());
                        log.Info(this, "PNR : " + dataClass.Rows[i]["RecordLocator"].ToString());
                        if (dataClass.Rows[i]["RecordLocator"].ToString() != "")
                        {
                            EmptyPNR = false;
                            PNR = dataClass.Rows[i]["RecordLocator"].ToString();
                            param = "," + PNR;
                        }
                        else
                        {
                            leftEmptyPNR = true;
                            EmptyPNR = true;
                        }

                        decimal minPay;
                        string PNRValue;

                        if (EmptyPNR)
                        {
                            minPay = Convert.ToDecimal(ValidateMinAmount(hID.Value, dataClass.Rows[i]["SellSignature"].ToString()));
                            PNRValue = dataClass.Rows[i]["SellSignature"].ToString();
                        }
                        else
                        {
                            //amended by diana 20131102 - hide disapproved record locator
                            if (Session["NewBooking"].ToString() == "true")
                                lstbookFlightInfo = objBooking.GetAllBK_TRANSDTLCombinePNR(hID.Value);
                            else
                                lstbookFlightInfo = objBooking.GetAllBK_TRANSDTLCombinePNR(hID.Value, 0, "LEN(RecordLocator) >= 6 AND ");
                            log.Info(this, "Session New Booking : " + Session["NewBooking"].ToString());
                            objBooking.FillDataTableTransDetail(lstbookFlightInfo);

                            minPay = Convert.ToDecimal(ValidateMinAmount(hID.Value, dataClass.Rows[i]["RecordLocator"].ToString(), true));
                            PNRValue = PNR;
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


                        //added by diana 20170211, to store processing fee to radio button
                        decimal ProcessFee = getTotalProceessFee(dataClass, PNRValue, lblAmountDueCurrency.Text);

                        if (minPay > 0)
                        {
                            rdlProcessFee.Items.Add(ProcessFee.ToString("N", nfi), ProcessFee);
                            switch (tabname)
                            {
                                case "TabCredit":
                                    rdlPNR.Items.Add("PNR:" + PNR + " (total passenger:" + dataClass.Rows[i]["Quantity"].ToString() + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")", dataClass.Rows[i]["SellSignature"].ToString() + param);
                                    cblPNR.ValidationSettings.RequiredField.IsRequired = false;
                                    cblPNR.Visible = false;
                                    rdlPNR.Visible = true;
                                    break;
                                case "TabAG":
                                    cblPNR.Items.Add("PNR:" + PNR + " (total passenger:" + dataClass.Rows[i]["Quantity"].ToString() + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")", dataClass.Rows[i]["SellSignature"].ToString() + param);
                                    rdlPNR.ValidationSettings.RequiredField.IsRequired = false;
                                    rdlPNR.Visible = false;
                                    cblPNR.Visible = true;
                                    break;
                            }
                            //rdlPNR.Items.Add("PNR:" + PNR + " (total passenger:" + dataClass.Rows[i]["Quantity"].ToString() + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")", dataClass.Rows[i]["SellSignature"].ToString() + param);
                            rdlContactPNR.Items.Add("Title: " + HttpContext.Current.Session["Title_" + PNR] + ";FirstName: " + HttpContext.Current.Session["FirstName_" + PNR] + ";LastName: " + HttpContext.Current.Session["LastName_" + PNR] + ";Email: " + HttpContext.Current.Session["Email_" + PNR] + ";PhoneNo: " + HttpContext.Current.Session["PhoneNo_" + PNR] + ";Address: " + HttpContext.Current.Session["Address_" + PNR] + ";Town: " + HttpContext.Current.Session["Town_" + PNR] + ";Country: " + HttpContext.Current.Session["Country_" + PNR] + ";State: " + HttpContext.Current.Session["State_" + PNR] + ";ZipCode: " + HttpContext.Current.Session["ZipCode_" + PNR]);
                        }
                        else
                        {
                            rdlProcessFee.Items.Add(ProcessFee.ToString("N", nfi), ProcessFee);
                            switch (tabname)
                            {
                                case "TabCredit":
                                    rdlPNR.Items.Add("PNR:" + PNR + " (total passenger:" + dataClass.Rows[i]["Quantity"].ToString() + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  No Min. Payment is Required", dataClass.Rows[i]["SellSignature"].ToString() + param);
                                    cblPNR.ValidationSettings.RequiredField.IsRequired = false;
                                    cblPNR.Visible = false;
                                    break;
                                case "TabAG":
                                    cblPNR.Items.Add("PNR:" + PNR + " (total passenger:" + dataClass.Rows[i]["Quantity"].ToString() + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  No Min. Payment is Required", dataClass.Rows[i]["SellSignature"].ToString() + param);
                                    rdlPNR.ValidationSettings.RequiredField.IsRequired = false;
                                    rdlPNR.Visible = false;
                                    //cblPNR.Enabled = false;
                                    break;
                            }
                            //rdlPNR.Items.Add("PNR:" + PNR + " (total passenger:" + dataClass.Rows[i]["Quantity"].ToString() + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  No Min. Payment is Required", dataClass.Rows[i]["SellSignature"].ToString() + param);
                            rdlContactPNR.Items.Add("Title: " + HttpContext.Current.Session["Title_" + PNR] + ";FirstName: " + HttpContext.Current.Session["FirstName_" + PNR] + ";LastName: " + HttpContext.Current.Session["LastName_" + PNR] + ";Email: " + HttpContext.Current.Session["Email_" + PNR] + ";PhoneNo: " + HttpContext.Current.Session["PhoneNo_" + PNR] + ";Address: " + HttpContext.Current.Session["Address_" + PNR] + ";Town: " + HttpContext.Current.Session["Town_" + PNR] + ";Country: " + HttpContext.Current.Session["Country_" + PNR] + ";State: " + HttpContext.Current.Session["State_" + PNR] + ";ZipCode: " + HttpContext.Current.Session["ZipCode_" + PNR]);
                        }

                        if (IsPrevEmpty == true && EmptyPNR == false)
                        {

                        }
                        else if (IsPrevEmpty == false && EmptyPNR == true)
                        {
                            MaxPax = Convert.ToInt32(dataClass.Rows[i]["Quantity"].ToString());
                            MaxAmount = Convert.ToDecimal(String.Format("{0:0.00}", AmountDue));
                            MinPay = Convert.ToDecimal(String.Format("{0:0.00}", minPay));
                            iIndex = i;
                            IsPrevEmpty = EmptyPNR;
                        }
                        else
                        {
                            if (Convert.ToInt32(dataClass.Rows[i]["Quantity"].ToString()) > MaxPax)
                            {
                                MaxPax = Convert.ToInt32(dataClass.Rows[i]["Quantity"].ToString());
                                MaxAmount = Convert.ToDecimal(String.Format("{0:0.00}", AmountDue));
                                MinPay = Convert.ToDecimal(String.Format("{0:0.00}", minPay));
                                iIndex = i;
                                IsPrevEmpty = EmptyPNR;
                            }
                            else if (MaxPax == Convert.ToInt32(dataClass.Rows[i]["Quantity"].ToString()) && AmountDue > MaxAmount)
                            {
                                MaxPax = Convert.ToInt32(dataClass.Rows[i]["Quantity"].ToString());
                                MaxAmount = Convert.ToDecimal(String.Format("{0:0.00}", AmountDue));
                                MinPay = Convert.ToDecimal(String.Format("{0:0.00}", minPay));
                                iIndex = i;
                                IsPrevEmpty = EmptyPNR;
                            }
                        }

                        AmountPaid += Convert.ToDecimal(dataClass.Rows[i]["DetailCollectedAmt"].ToString());
                        TotalAmount += Convert.ToDecimal(dataClass.Rows[i]["FullPrice"].ToString());
                        param = ",";
                    }

                    if (iIndex >= 0 && iIndex < dataClass.Rows.Count)
                    {
                        switch (tabname)
                        {
                            case "TabCredit":
                                rdlPNR.Items[iIndex].Selected = true;
                                txtMinimumPayment.Value = MinPay;
                                txtMinPay.Value = MinPay;
                                lblMinPay.Value = MinPay.ToString("N", nfi);
                                txtDueAmount.Value = MaxAmount;
                                lblFullPay.Value = MaxAmount.ToString("N", nfi);
                                if (cbFullPayment.Checked)
                                {
                                    txtPayAmount.Value = MaxAmount.ToString("N", nfi);
                                }
                                else
                                {
                                    txtPayAmount.Value = MinPay.ToString("N", nfi);
                                }
                                break;
                                //case "TabAG":
                                //    //cblPNR.Items[iIndex].Selected = true;
                                //    txtMinimumPayment.Value = 0;
                                //    txtMinPay.Value = 0;
                                //    lblMinPay.Value = 0;
                                //    txtDueAmount.Value = 0;
                                //    lblFullPay.Value = 0;
                                //    txtPayAmount.Value = 0;
                                //    break;
                        }


                    }

                    if (TotalAmount == AmountPaid)
                    {
                        LoadData(hID.Value);

                        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                        string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), hID.Value.ToString(), "");

                        Response.Redirect(Shared.MySite.PublicPages.BookingComplete + "?k=" + hashkey + "&TransID=" + hID.Value.ToString() + "&payment=1", false);
                    }

                    if ((leftEmptyPNR && Validate) || (leftEmptyPNR && Session["reloadPage"].ToString() == "true"))
                    {
                        //msgcontrol.MessageDisplay("Please take note that some PNR still not yet be confirmed, \n please select and pay at least minimum payment in order to secure your booking.\n Otherwise the unconfirmed booking will be expired.");
                        lblErrorBottom.Text = msgList.Err999990;
                        imgError.Visible = true;
                        lblErrorBottom.Visible = true;
                    }
                    else if (Session["reloadPage"].ToString() == "true")
                    {
                        lblErrorBottom.Visible = true;
                        lblErrorBottom.Text = msgList.Err100020;
                        imgError.Visible = true;
                    }
                }
            }
            //added by diana 20170324, for changes after confirm booking
            else if (HttpContext.Current.Session["ChgTransMain"] != null)
            {
                DataTable dtTransMain = objBooking.dtTransMain();
                dtTransMain = (DataTable)HttpContext.Current.Session["ChgTransMain"];

                DataTable dtTransDetail = objBooking.dtTransDetail();
                dtTransDetail = (DataTable)HttpContext.Current.Session["ChgTransDetail"];

                //20170420 - Sienny (get info total pax)
                int totalPAX = 0;

                for (int i = 0; i < dtTransDetail.Rows.Count; i++)
                {
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

                    decimal AmountDue = Convert.ToDecimal(dtTransDetail.Rows[i]["AmountDue"].ToString());
                    decimal minPay = AmountDue; //Convert.ToDecimal(ValidateMinAmount(hID.Value, dtTransDetail.Rows[i]["RecordLocator"].ToString(), true));
                    int Pax = Convert.ToInt32(dtTransDetail.Rows[i]["PaxAdult"].ToString()) + Convert.ToInt32(dtTransDetail.Rows[i]["PaxChild"].ToString());
                    decimal ProcessFee = 0;

                    //20170420 - Sienny (get info total pax)
                    totalPAX += Pax;

                    if (minPay > 0)
                    {
                        string actualstatus = "";
                        if (dtTransMain.Rows.Count > 0) { actualstatus = dtTransMain.Rows[0]["TransStatus"].ToString(); }
                        rdlProcessFee.Items.Add(ProcessFee.ToString("N", nfi), ProcessFee);
                        if (actualstatus == "3") //amended by diana 20140108 - check for actual status - hide expiry date is actual status is confirmed
                        {
                            if (Request.IsLocal || HttpContext.Current.IsDebuggingEnabled)
                            {
                                BookingControl booking = new BookingControl();
                                ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking("");
                                List<BookingTransactionDetail> objListBK_TRANSDTL_Infos = (List<BookingTransactionDetail>)HttpContext.Current.Session["objListBK_TRANSDTL_Infos"];
                                ABS.Navitaire.BookingManager.Booking book = new ABS.Navitaire.BookingManager.Booking();// apiBooking.GetBookingFromState(objListBK_TRANSDTL_Infos.Where(x => x.RecordLocator == dtTransDetail.Rows[i]["RecordLocator"].ToString()).FirstOrDefault().Signature);
                                using (profiler.Step("Navitaire:GetBookingFromState"))
                                {
                                    book = apiBooking.GetBookingFromState(objListBK_TRANSDTL_Infos.Where(x => x.RecordLocator == dtTransDetail.Rows[i]["RecordLocator"].ToString()).FirstOrDefault().Signature);
                                }
                                switch (tabname)
                                {
                                    case "TabCredit":
                                        rdlPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ") Actual Amount = " + objGeneral.RoundUp(book.BookingSum.BalanceDue).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")", dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                                        break;
                                    case "TabAG":
                                        cblPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ") Actual Amount = " + objGeneral.RoundUp(book.BookingSum.BalanceDue).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")", dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                                        break;

                                }
                            }
                            else
                            {
                                switch (tabname)
                                {
                                    case "TabCredit":
                                        rdlPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")", dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                                        break;
                                    case "TabAG":
                                        cblPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")", dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                                        break;
                                }
                            }

                            //cblPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")", dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                        }
                        else
                        {
                            if (Session["ChgMode"] != null)
                            {
                                if (Request.IsLocal || HttpContext.Current.IsDebuggingEnabled)
                                {
                                    BookingControl booking = new BookingControl();
                                    ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking("");
                                    List<BookingTransactionDetail> objListBK_TRANSDTL_Infos = (List<BookingTransactionDetail>)HttpContext.Current.Session["objListBK_TRANSDTL_Infos"];
                                    ABS.Navitaire.BookingManager.Booking book = new ABS.Navitaire.BookingManager.Booking();// apiBooking.GetBookingFromState(objListBK_TRANSDTL_Infos.Where(x => x.RecordLocator == dtTransDetail.Rows[i]["RecordLocator"].ToString()).FirstOrDefault().Signature);
                                    using (profiler.Step("Navitaire:GetBookingFromState"))
                                    {
                                        book = apiBooking.GetBookingFromState(objListBK_TRANSDTL_Infos.Where(x => x.RecordLocator == dtTransDetail.Rows[i]["RecordLocator"].ToString()).FirstOrDefault().Signature);
                                    }
                                    switch (tabname)
                                    {
                                        case "TabCredit":
                                            rdlPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ") Actual Amount = " + objGeneral.RoundUp(book.BookingSum.BalanceDue).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")", dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                                            break;
                                        case "TabAG":
                                            cblPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ") Actual Amount = " + objGeneral.RoundUp(book.BookingSum.BalanceDue).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")", dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (tabname)
                                    {
                                        case "TabCredit":
                                            rdlPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")", dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                                            break;
                                        case "TabAG":
                                            cblPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")", dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                                            break;
                                    }

                                }
                                //cblPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")", dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                            }
                            else
                            {
                                switch (tabname)
                                {
                                    case "TabCredit":
                                        rdlPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")" + " Expiry Date: " + String.Format("{0:dd MMM yyyy HH:mm}", DateTime.Parse(dtTransDetail.Rows[i]["NextDueDate"].ToString())), dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                                        //cblPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")" + " Expiry Date: " + String.Format("{0:dd MMM yyyy HH:mm}", DateTime.Parse(dtTransDetail.Rows[i]["NextDueDate"].ToString())), dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                                        break;
                                    case "TabAG":
                                        cblPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")" + " Expiry Date: " + String.Format("{0:dd MMM yyyy HH:mm}", DateTime.Parse(dtTransDetail.Rows[i]["NextDueDate"].ToString())), dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                                        //cblPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")" + " Expiry Date: " + String.Format("{0:dd MMM yyyy HH:mm}", DateTime.Parse(dtTransDetail.Rows[i]["NextDueDate"].ToString())), dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                                        break;
                                }
                            }

                        }
                        rdlContactPNR.Items.Add("Title: " + HttpContext.Current.Session["Title_" + PNR] + ";FirstName: " + HttpContext.Current.Session["FirstName_" + PNR] + ";LastName: " + HttpContext.Current.Session["LastName_" + PNR] + ";Email: " + HttpContext.Current.Session["Email_" + PNR] + ";PhoneNo: " + HttpContext.Current.Session["PhoneNo_" + PNR] + ";Address: " + HttpContext.Current.Session["Address_" + PNR] + ";Town: " + HttpContext.Current.Session["Town_" + PNR] + ";Country: " + HttpContext.Current.Session["Country_" + PNR] + ";State: " + HttpContext.Current.Session["State_" + PNR] + ";ZipCode: " + HttpContext.Current.Session["ZipCode_" + PNR]);
                    }
                    else
                    {
                        rdlProcessFee.Items.Add(ProcessFee.ToString("N", nfi), ProcessFee);
                        switch (tabname)
                        {
                            case "TabCredit":
                                rdlPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  No Min. Payment is Required", dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                                break;
                            case "TabAG":
                                cblPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  No Min. Payment is Required", dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                                //cblPNR.Enabled = false;
                                break;
                        }
                        //cblPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  No Min. Payment is Required", dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                        rdlContactPNR.Items.Add("Title: " + HttpContext.Current.Session["Title_" + PNR] + ";FirstName: " + HttpContext.Current.Session["FirstName_" + PNR] + ";LastName: " + HttpContext.Current.Session["LastName_" + PNR] + ";Email: " + HttpContext.Current.Session["Email_" + PNR] + ";PhoneNo: " + HttpContext.Current.Session["PhoneNo_" + PNR] + ";Address: " + HttpContext.Current.Session["Address_" + PNR] + ";Town: " + HttpContext.Current.Session["Town_" + PNR] + ";Country: " + HttpContext.Current.Session["Country_" + PNR] + ";State: " + HttpContext.Current.Session["State_" + PNR] + ";ZipCode: " + HttpContext.Current.Session["ZipCode_" + PNR]);
                    }

                }

                //20170420 - Sienny (get info total pax)
                lbl_num.Text = totalPAX.ToString();

                if (Convert.ToDecimal(dtTransMain.Rows[0]["CollectedAmt"].ToString()) == Convert.ToDecimal(dtTransMain.Rows[0]["TotalTrans"].ToString()))
                {

                    ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                    string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), hID.Value.ToString(), "");

                    Response.Redirect(Shared.MySite.PublicPages.BookingComplete + "?k=" + hashkey + "&TransID=" + hID.Value.ToString() + "&payment=1", false);
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

                    //20170420 - Sienny (get info total pax)
                    int totalPAX = 0;

                    //changed by tyas
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
                        decimal minPay = Convert.ToDecimal(ValidateMinAmount(hID.Value, dtTransDetail.Rows[i]["RecordLocator"].ToString(), true));
                        int Pax = Convert.ToInt32(dtTransDetail.Rows[i]["PaxAdult"].ToString()) + Convert.ToInt32(dtTransDetail.Rows[i]["PaxChild"].ToString());
                        decimal ProcessFee = 0;
                        DateTime ExpiryDate = Convert.ToDateTime(dtTransMain.Rows[0]["ExpiryDate"].ToString());

                        //20170420 - Sienny (get info total pax)
                        totalPAX += Pax;

                        if (minPay > 0)
                        {
                            string actualstatus = "";
                            if (dtTransMain.Rows.Count > 0) { actualstatus = dtTransMain.Rows[0]["TransStatus"].ToString(); }
                            rdlProcessFee.Items.Add(ProcessFee.ToString("N", nfi), ProcessFee);
                            if (actualstatus == "3") //amended by diana 20140108 - check for actual status - hide expiry date is actual status is confirmed
                            {
                                switch (tabname)
                                {
                                    case "TabCredit":
                                        rdlPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")", dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                                        break;
                                    case "TabAG":
                                        cblPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")", dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                                        break;
                                }
                                //cblPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")", dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                            }
                            else
                            {
                                if (Session["ChgMode"] != null || ExpiryDate < DateTime.Now)
                                {
                                    switch (tabname)
                                    {
                                        case "TabCredit":
                                            rdlPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")", dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                                            break;
                                        case "TabAG":
                                            cblPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")", dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                                            break;
                                    }
                                    //cblPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")", dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                                }
                                else
                                {
                                    switch (tabname)
                                    {
                                        case "TabCredit":
                                            rdlPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")" + " Expiry Date: " + String.Format("{0:dd MMM yyyy HH:mm}", DateTime.Parse(dtTransDetail.Rows[i]["NextDueDate"].ToString())), dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                                            break;
                                        case "TabAG":
                                            cblPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")" + " Expiry Date: " + String.Format("{0:dd MMM yyyy HH:mm}", DateTime.Parse(dtTransDetail.Rows[i]["NextDueDate"].ToString())), dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                                            break;
                                    }
                                    //cblPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  Min. Payment:" + objGeneral.RoundUp(minPay).ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")" + " Expiry Date: " + String.Format("{0:dd MMM yyyy HH:mm}", DateTime.Parse(dtTransDetail.Rows[i]["NextDueDate"].ToString())), dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                                }

                            }
                            rdlContactPNR.Items.Add("Title: " + HttpContext.Current.Session["Title_" + PNR] + ";FirstName: " + HttpContext.Current.Session["FirstName_" + PNR] + ";LastName: " + HttpContext.Current.Session["LastName_" + PNR] + ";Email: " + HttpContext.Current.Session["Email_" + PNR] + ";PhoneNo: " + HttpContext.Current.Session["PhoneNo_" + PNR] + ";Address: " + HttpContext.Current.Session["Address_" + PNR] + ";Town: " + HttpContext.Current.Session["Town_" + PNR] + ";Country: " + HttpContext.Current.Session["Country_" + PNR] + ";State: " + HttpContext.Current.Session["State_" + PNR] + ";ZipCode: " + HttpContext.Current.Session["ZipCode_" + PNR]);
                        }
                        else
                        {
                            rdlProcessFee.Items.Add(ProcessFee.ToString("N", nfi), ProcessFee);
                            switch (tabname)
                            {
                                case "TabCredit":
                                    rdlPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  No Min. Payment is Required", dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                                    break;
                                case "TabAG":
                                    cblPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  No Min. Payment is Required", dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                                    //cblPNR.Enabled = false;
                                    break;
                            }
                            //cblPNR.Items.Add("PNR:" + dtTransDetail.Rows[i]["RecordLocator"].ToString() + " (total passenger:" + Pax + ") Amount Due:" + AmountDue.ToString("N", nfi) + "(" + lblAmountDueCurrency.Text + ")  No Min. Payment is Required", dtTransDetail.Rows[i]["RecordLocator"].ToString() + "," + dtTransDetail.Rows[i]["RecordLocator"].ToString());
                            rdlContactPNR.Items.Add("Title: " + HttpContext.Current.Session["Title_" + PNR] + ";FirstName: " + HttpContext.Current.Session["FirstName_" + PNR] + ";LastName: " + HttpContext.Current.Session["LastName_" + PNR] + ";Email: " + HttpContext.Current.Session["Email_" + PNR] + ";PhoneNo: " + HttpContext.Current.Session["PhoneNo_" + PNR] + ";Address: " + HttpContext.Current.Session["Address_" + PNR] + ";Town: " + HttpContext.Current.Session["Town_" + PNR] + ";Country: " + HttpContext.Current.Session["Country_" + PNR] + ";State: " + HttpContext.Current.Session["State_" + PNR] + ";ZipCode: " + HttpContext.Current.Session["ZipCode_" + PNR]);
                        }

                    }

                    //20170420 - Sienny (get info total pax)
                    lbl_num.Text = totalPAX.ToString();

                    if (Convert.ToDecimal(dtTransMain.Rows[0]["CollectedAmt"].ToString()) == Convert.ToDecimal(dtTransMain.Rows[0]["TotalTrans"].ToString()))
                    {

                        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                        string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), hID.Value.ToString(), "");

                        Response.Redirect(Shared.MySite.PublicPages.BookingComplete + "?k=" + hashkey + "&TransID=" + hID.Value.ToString() + "&payment=1", false);
                    }

                }
            }
        }

        private void insertCityValue()
        {

            if (Session["combinedCity"] != null)
            {
                string[] combinedCity = (string[])Session["combinedCity"];
                Page.ClientScript.RegisterStartupScript(this.GetType(), "varCity", "<script type='text/javascript'>var city_value = " + combinedCity[0] + "; var city_text = " + combinedCity[1] + ";</script>");
            }
        }

        private void SetPayScheme()
        {
            if (HttpContext.Current.Session["TempFlight"] != null)
            {
                Hashtable ht = (Hashtable)HttpContext.Current.Session["HashMain"];
                bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(ht["TransID"].ToString());
                PayScheme = bookHDRInfo.PayScheme;
                CurrencyCode = bookHDRInfo.Currency;
            }
            else if (HttpContext.Current.Session["ChgTransMain"] != null)
            {
                DataTable dtTransMain = objBooking.dtTransMain();
                dtTransMain = (DataTable)HttpContext.Current.Session["ChgTransMain"];
                PayScheme = dtTransMain.Rows[0]["SchemeCode"].ToString();
                CurrencyCode = dtTransMain.Rows[0]["Currency"].ToString();
            }
            else if (HttpContext.Current.Session["TransMain"] != null)
            {
                DataTable dtTransMain = objBooking.dtTransMain();
                dtTransMain = (DataTable)HttpContext.Current.Session["TransMain"];
                PayScheme = dtTransMain.Rows[0]["SchemeCode"].ToString();
                CurrencyCode = dtTransMain.Rows[0]["Currency"].ToString();
            }


            //if (Session["Country"] != null)
            //    CountryCode = Session["Country"].ToString();
        }

        //private string GetXMLString(object Obj)
        //{
        //    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Obj.GetType());
        //    System.IO.StringWriter writer = new System.IO.StringWriter();
        //    x.Serialize(writer, Obj);

        //    return writer.ToString();
        //}

        private Decimal ValidateMinAmount(string TransID, string SessionID, Boolean PNRExist = false, Boolean NonZeroAmount = false)
        {
            if (hIsOverride.Value.ToString() == "1")
                return SetMinAmount(TransID, SessionID, PNRExist, NonZeroAmount);
            else
                return SetMinAmount(SessionID, PNRExist, NonZeroAmount);
        }

        private Decimal SetMinAmount(string TransID, string SessionID, Boolean PNRExist = false, Boolean NonZeroAmount = false)
        {
            PaymentControl objPayment = new PaymentControl();
            PaymentInfo paymentInfo = new PaymentInfo();
            string AgentCountryCode = "";
            SetPayScheme();
            List<BookingTransactionDetail> ListOneTransDTL = objBooking.GetOneBK_TRANSDTLFilterAll(hID.Value, 0);
            if (ListOneTransDTL.Count > 0)
            {
                CountryCode = objGeneral.GetCountryCodeByCode(ListOneTransDTL[0].Origin);
            }
            string GroupName = objGeneral.getOPTGroupByCarrierCode(hCarrierCode.Value);
            if (Session["CountryCode"] != null)
                AgentCountryCode = Session["CountryCode"].ToString();
            paymentInfo = objPayment.GetPaymentScheme(PayScheme, GroupName, TransID, CountryCode, CurrencyCode, AgentCountryCode);
            //paymentInfo = objPayment.GetPaymentScheme(PayScheme, GroupName, TransID, CountryCode, CurrencyCode); //unremarked by diana 20140120

            //set all to AA group as AAX now follow AA settings
            //paymentInfo = objPayment.GetPaymentScheme(PayScheme, "AA");
            decimal CollectedAmount = 0;
            decimal ServiceChg = 0;
            decimal FullPrice = 0;
            decimal BaseFare = 0;
            decimal LineProcess = 0;
            decimal AmountDue = 0;
            DateTime PayDueDate1 = DateTime.Now;
            decimal PayDueAmount1 = 0;
            DateTime PayDueDate2 = DateTime.Now;
            decimal PayDueAmount2 = 0;
            DateTime PayDueDate3 = DateTime.Now;
            decimal PayDueAmount3 = 0;
            int Quantity = 0;
            decimal FirstDeposit = 0;
            decimal CurrencyRate = 0;
            DataTable dt = objBooking.dtClass();
            if (HttpContext.Current.Session["TempFlight"] != null && PNRExist == false)
            {
                if (HttpContext.Current.Session["dataClassTrans"] != null)
                {
                    dt = (DataTable)HttpContext.Current.Session["dataClassTrans"];
                    DataRow[] dtrows = dt.Select("SellSignature = '" + SessionID + "'");

                    BookingControl booking = new BookingControl();
                    ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking("");
                    //ABS.Navitaire.BookingManager.Booking book = apiBooking.GetBookingFromState(SessionID);//remark by romy for optimize
                    //string xml = GetXMLString(book);

                    if (dtrows.Length == 1)
                    {
                        //string tabname = TabControl.ActiveTabPage.Name;
                        //if (tabname == "TabCredit")//Added By Agus
                        //{
                        //    if (Session["TotalProcessFee"] != null)
                        //    {
                        //        dtrows[0]["DetailCollectedAmt"] = Convert.ToDecimal(dtrows[0]["DetailCollectedAmt"]) + Convert.ToDecimal(Session["TotalProcessFee"]);
                        //    }
                        //}
                        CollectedAmount = Convert.ToDecimal(dtrows[0]["DetailCollectedAmt"].ToString());
                        ServiceChg = Convert.ToDecimal(dtrows[0]["ServChrg"].ToString());
                        FullPrice = Convert.ToDecimal(dtrows[0]["FullPrice"].ToString());
                        Quantity = Convert.ToInt32(dtrows[0]["Quantity"].ToString());
                        CurrencyRate = Convert.ToDecimal(dtrows[0]["CurrencyRate"].ToString());
                        BaseFare = Convert.ToDecimal(dtrows[0]["FarePrice"].ToString());



                        /// amended by diana 20130913
                        //decimal paymentAttempt1 = 0;
                        //paymentAttempt1 = ((FullPrice - ServiceChg) * paymentInfo.Percentage_1) / 100;
                        if (paymentInfo.PaymentType == "SVCF")
                        {
                            if ((ServiceChg - CollectedAmount) >= 0)
                            {
                                return ServiceChg - CollectedAmount;
                            }

                            //FirstDeposit = paymentInfo.FirstDeposit;
                            //if (CurrencyRate > 0) FirstDeposit = FirstDeposit / CurrencyRate;
                            //FirstDeposit = FirstDeposit * Quantity;
                            ////PayDueDate1 = 
                            //if ((FirstDeposit - CollectedAmount) >= 0)
                            //{
                            //    return FirstDeposit - CollectedAmount;
                            //}
                        }
                        else if (paymentInfo.PaymentType == "DEPO")
                        {
                            decimal paymentAttempt1 = FullPrice;
                            BookingTransactionDetail bkDetail = new BookingTransactionDetail();
                            bkDetail = objBooking.GetSingle_TRANSDTLBySellKey(SessionID);

                            if (bkDetail != null)
                            {
                                paymentAttempt1 = bkDetail.PayDueAmount1;
                            }

                            if ((paymentAttempt1 - CollectedAmount) >= 0)
                            {
                                return paymentAttempt1 - CollectedAmount;
                            }
                        }
                        else
                        {
                            return FullPrice;
                        }
                        /// end amended by diana 20130913

                    }

                    //HttpContext.Current.Session["dataClassTrans"] = dt;
                }
            }
            else if (HttpContext.Current.Session["ChgTransMain"] != null)
            {
                DataTable dtTransDetail = objBooking.dtTransDetail();
                dtTransDetail = (DataTable)HttpContext.Current.Session["ChgTransDetail"];
                DataRow[] dtrows = dtTransDetail.Select("RecordLocator = '" + SessionID + "'");
                if (dtrows.Length == 1)
                {
                    CollectedAmount = Convert.ToDecimal(dtrows[0]["DetailCollectedAmount"].ToString());
                    AmountDue = Convert.ToDecimal(dtrows[0]["AmountDue"].ToString());
                    ServiceChg = Convert.ToDecimal(dtrows[0]["LineFee"].ToString());
                    FullPrice = Convert.ToDecimal(dtrows[0]["LineTotal"].ToString());
                    BaseFare = Convert.ToDecimal(dtrows[0]["LineFlight"].ToString());
                    LineProcess = Convert.ToDecimal(dtrows[0]["LineProcess"].ToString());
                    return FullPrice - CollectedAmount;
                }
                else
                {
                    if (HttpContext.Current.Session["dataClassTrans"] != null)
                    {
                        dt = (DataTable)HttpContext.Current.Session["dataClassTrans"];
                        DataRow[] dtrows2 = dt.Select("RecordLocator = '" + SessionID + "'");
                        if (dtrows2.Length == 1)
                        {
                            CollectedAmount = Convert.ToDecimal(dtrows2[0]["DetailCollectedAmt"].ToString());
                            ServiceChg = Convert.ToDecimal(dtrows2[0]["ServChrg"].ToString());
                            FullPrice = Convert.ToDecimal(dtrows2[0]["FullPrice"].ToString());
                            BaseFare = Convert.ToDecimal(dtrows[0]["FarePrice"].ToString());

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
                        }
                    }

                }

            }
            else
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
                    //Added by ketee, expiry date and details expiry date
                    DateTime ExpiryDate = Convert.ToDateTime(dtTransMain.Rows[0]["ExpiryDate"].ToString());

                    double remainingHrs = (STD.Subtract(todays).TotalHours);
                    double BookingHrs = todays.Subtract(BookingDate).TotalHours;
                    CurrencyRate = Convert.ToDecimal(dtTransMain.Rows[0]["ExchangeRate"].ToString());

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
                        BaseFare = Convert.ToDecimal(dtrows[0]["LineFlight"].ToString());
                        LineProcess = Convert.ToDecimal(dtrows[0]["LineProcess"].ToString());
                        PayDueAmount1 = Convert.ToDecimal(dtrows[0]["PayDueAmount1"].ToString());
                        PayDueAmount2 = Convert.ToDecimal(dtrows[0]["PayDueAmount2"].ToString());
                        PayDueAmount3 = Convert.ToDecimal(dtrows[0]["PayDueAmount3"].ToString());
                        PayDueDate1 = Convert.ToDateTime(dtrows[0]["PayDueDate1"].ToString());
                        PayDueDate2 = Convert.ToDateTime(dtrows[0]["PayDueDate2"].ToString());
                        PayDueDate3 = Convert.ToDateTime(dtrows[0]["PayDueDate3"].ToString());

                        if (ExpiryDate < todays)
                        {
                            return FullPrice - CollectedAmount;
                        }
                        else if (LineProcess != 0)
                        {
                            if ((FullPrice != PayDueAmount1 + PayDueAmount2 + PayDueAmount3 + LineProcess))
                            {
                                paymentInfo.PaymentType = "FULL";
                            }
                        }
                        else
                        {
                            if ((FullPrice != PayDueAmount1 + PayDueAmount2 + PayDueAmount3 + (FullPrice - PayDueAmount1 - PayDueAmount2 - PayDueAmount3)) || (((bookHDRInfo.TransTotalAmt - bookHDRInfo.CollectedAmt) > 0 && (bookHDRInfo.CollectedAmt - bookHDRInfo.ForfeitedAmount) >= (bookHDRInfo.PaymentAmtEx1 + bookHDRInfo.PaymentAmtEx2 + bookHDRInfo.PaymentAmtEx3))))
                            {
                                paymentInfo.PaymentType = "FULL";
                            }

                        }

                        ServiceChg = 0;
                        /// amended by diana 20130913
                        decimal paymentAttempt1 = 0;
                        decimal paymentAttempt2 = 0;
                        decimal paymentAttempt3 = 0;

                        //added by diana 20140121 - retrieve currency, origin, transit details
                        BookingTransactionDetail objBK_TRANSDTL_Info = new BookingTransactionDetail();
                        objBK_TRANSDTL_Info = objBooking.GetBK_TRANSDTLFlightByPNR(SessionID);

                        Decimal deposit = 0;

                        if (paymentInfo.PaymentType == "FULL")
                        {
                            return FullPrice - CollectedAmount;
                        }
                        else
                        {
                            //deposit = objGeneral.getDepositByDuration(TransID, BaseFare, objBK_TRANSDTL_Info.PaxAdult + objBK_TRANSDTL_Info.PaxChild, objBK_TRANSDTL_Info.Currency, objBK_TRANSDTL_Info.Origin, GroupName, objBK_TRANSDTL_Info.FlightDuration, objBK_TRANSDTL_Info.SellKey, objBK_TRANSDTL_Info.Transit);
                            //amended by diana 20140121 - add if for every attempt to check for deposit
                            //added by ketee, rounding 2 decimal places, 20150723
                            paymentAttempt1 = PayDueAmount1;
                            if (paymentInfo.IsNominal_1 == 1)
                            {
                                if (deposit == 0)
                                {
                                    //objBK_TRANSDTL_Info.Currency
                                    deposit = objGeneral.getDepositByDuration(hID.Value, FullPrice, objBK_TRANSDTL_Info.TotalPax, objBK_TRANSDTL_Info.Currency, objBK_TRANSDTL_Info.Origin, GroupName, objBK_TRANSDTL_Info.FlightDuration, objBK_TRANSDTL_Info.SellKey, objBK_TRANSDTL_Info.Transit);
                                    //if (CurrencyRate > 0) deposit = deposit / CurrencyRate;
                                }
                                paymentAttempt1 = deposit + ServiceChg; // CollectedAmount;
                                //PayDueAmount2 = deposit;
                            }
                            else if (paymentInfo.Deposit_1 != 0)
                            {
                                if (deposit == 0)
                                {
                                    deposit = objGeneral.getDeposit(hID.Value, objBK_TRANSDTL_Info.TotalPax, objBK_TRANSDTL_Info.Currency, objBK_TRANSDTL_Info.Origin, objBK_TRANSDTL_Info.Transit);
                                }
                                paymentAttempt1 = deposit + ServiceChg;
                                //PayDueAmount2 = deposit;
                            }

                            //if (paymentInfo.Code_1 == "DOB")
                            //{
                            //    PayDueDate1 = BookingDate.AddHours(paymentInfo.Attempt_1);
                            //}
                            //else if (paymentInfo.Code_1 == "STD")
                            //{
                            //    PayDueDate1 = STD.AddHours(-paymentInfo.Attempt_1);
                            //}

                            //amended by diana 20140121 - add condition to differentiate deposit and non deposit
                            //if (paymentInfo.IsNominal_1 == 1)
                            //    paymentAttempt2 = Math.Round((FullPrice * paymentInfo.Percentage_2) / 100, 2);
                            //else if (paymentInfo.Deposit_2 != 0)
                            //    paymentAttempt2 = Math.Round((FullPrice * paymentInfo.Percentage_2) / 100, 2);
                            //else
                            //{
                            //    if (FullPrice > (PayDueAmount1) && paymentInfo.Percentage_2 == 0)
                            //    {
                            //        paymentAttempt2 = FullPrice - (PayDueAmount1 + ((FullPrice - PayDueAmount1 - PayDueAmount2 - PayDueAmount3))) + paymentAttempt1;
                            //    }
                            //    else
                            //    {
                            //        paymentAttempt2 = Math.Round((deposit * paymentInfo.Percentage_2) / 100, 2) + paymentAttempt1;
                            //    }
                            //}

                            paymentAttempt2 = PayDueAmount2 + paymentAttempt1;

                            if (paymentInfo.Deposit_2 != 0)
                            {
                                if (deposit == 0)
                                {
                                    deposit = objGeneral.getDeposit(hID.Value, objBK_TRANSDTL_Info.TotalPax, objBK_TRANSDTL_Info.Currency, objBK_TRANSDTL_Info.Origin, objBK_TRANSDTL_Info.Transit);
                                    if (CurrencyRate > 0) deposit = deposit / CurrencyRate;
                                }
                                paymentAttempt2 = deposit + paymentAttempt1;
                            }

                            //if (paymentInfo.Code_2 == "DOB")
                            //{
                            //    PayDueDate2 = BookingDate.AddHours(paymentInfo.Attempt_2);
                            //}
                            //else if (paymentInfo.Code_2 == "STD")
                            //{
                            //    PayDueDate2 = STD.AddHours(-paymentInfo.Attempt_2);
                            //}

                            //amended by diana 20140121 - add condition to differentiate deposit and non deposit
                            //if (paymentInfo.Deposit_2 != 0)
                            //    paymentAttempt3 = Math.Round((FullPrice * paymentInfo.Percentage_3) / 100, 2);
                            //else
                            //{
                            //    if (FullPrice > (PayDueAmount1 + PayDueAmount2) && paymentInfo.Percentage_3 == 0)
                            //    {
                            //        paymentAttempt3 = FullPrice - (PayDueAmount1 + PayDueAmount2 + ((FullPrice - PayDueAmount1 - PayDueAmount2 - PayDueAmount3))) + paymentAttempt2;
                            //    }
                            //    else
                            //    {
                            //        paymentAttempt3 = Math.Round((deposit * paymentInfo.Percentage_3) / 100, 2) + paymentAttempt2;
                            //    }
                            //}
                            //if (paymentInfo.Deposit_3 != 0)
                            //{
                            //    if (deposit == 0)
                            //    {
                            //        deposit = objGeneral.getDeposit(objBK_TRANSDTL_Info.TotalPax, objBK_TRANSDTL_Info.Currency, objBK_TRANSDTL_Info.Origin, objBK_TRANSDTL_Info.Transit);
                            //    }
                            //    paymentAttempt3 = deposit + paymentAttempt2;
                            //}

                            paymentAttempt3 = PayDueAmount3 + paymentAttempt2;

                            //if (paymentInfo.Code_3 == "DOB")
                            //{
                            //    PayDueDate3 = BookingDate.AddHours(paymentInfo.Attempt_3);
                            //}
                            //else if (paymentInfo.Code_3 == "STD")
                            //{
                            //    PayDueDate3 = STD.AddHours(-paymentInfo.Attempt_3);
                            //}


                            bool doneattempt1 = true;
                            bool doneattempt2 = true;
                            bool doneattempt3 = true;

                            if (paymentInfo.Code_1 == "DOB")
                            {
                                //if (BookingHrs <= paymentInfo.Attempt_1)
                                //{
                                if (paymentAttempt1 > CollectedAmount)
                                {
                                    //Added by ketee, to check payment expiry, 20160223
                                    if (PayDueDate2 <= BookingDate.AddHours(paymentInfo.Attempt_1)) //ExpiryDate <= BookingDate.AddHours(paymentInfo.Attempt_1))
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
                                    if (ExpiryDate <= STD.AddHours(-paymentInfo.Attempt_1))
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
                                        if (PayDueDate2 <= BookingDate.AddHours(paymentInfo.Attempt_2)) //ExpiryDate <= BookingDate.AddHours(paymentInfo.Attempt_2))
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
                                        if (PayDueDate2 <= STD.AddHours(-paymentInfo.Attempt_2)) //ExpiryDate <= STD.AddHours(-paymentInfo.Attempt_2))
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
                                            if (PayDueDate3 <= BookingDate.AddHours(paymentInfo.Attempt_3))
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
                                            if (PayDueDate3 <= STD.AddHours(-paymentInfo.Attempt_3))
                                                doneattempt3 = false;
                                        }
                                        //}
                                    }
                                }
                            }

                            if (doneattempt1 == false)
                                if (LineProcess == 0)
                                {
                                    return paymentAttempt1 - (CollectedAmount - (FullPrice - PayDueAmount1 - PayDueAmount2 - PayDueAmount3));
                                }
                                else
                                {
                                    return paymentAttempt1 - (CollectedAmount - LineProcess);
                                }
                            else if (doneattempt2 == false)
                                if (LineProcess == 0)
                                {
                                    return paymentAttempt2 - (CollectedAmount - (FullPrice - PayDueAmount1 - PayDueAmount2 - PayDueAmount3));
                                }
                                else
                                {
                                    return paymentAttempt2 - (CollectedAmount - LineProcess);
                                }
                            //return paymentAttempt2 - (CollectedAmount - LineProcess);
                            else if (doneattempt3 == false)
                                if (LineProcess == 0)
                                {
                                    return paymentAttempt3 - (CollectedAmount - (FullPrice - PayDueAmount1 - PayDueAmount2 - PayDueAmount3));
                                }
                                else
                                {
                                    return paymentAttempt3 - (CollectedAmount - LineProcess);
                                }
                            //else if (PayScheme == "W1W" || PayScheme == "W1M")
                            else if (paymentInfo.PaymentType == "FULL")
                                return FullPrice - CollectedAmount;
                            /// end here
                            /// end amended by diana 20130913
                        }


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

        //added by ketee, old set minimum amount , 20170310
        private Decimal SetMinAmount(string SessionID, Boolean PNRExist = false, Boolean NonZeroAmount = false)
        {
            PaymentControl objPayment = new PaymentControl();
            PaymentInfo paymentInfo = new PaymentInfo();
            SetPayScheme();
            string GroupName = objGeneral.getOPTGroupByCarrierCode(hCarrierCode.Value);

            paymentInfo = objPayment.GetPaymentScheme(PayScheme, GroupName); //unremarked by diana 20140120
            //set all to AA group as AAX now follow AA settings
            //paymentInfo = objPayment.GetPaymentScheme(PayScheme, "AA");
            decimal CollectedAmount = 0;
            decimal ServiceChg = 0;
            decimal FullPrice = 0;
            decimal AmountDue = 0;
            DataTable dt = objBooking.dtClass();
            if (HttpContext.Current.Session["TempFlight"] != null && PNRExist == false)
            {
                if (HttpContext.Current.Session["dataClassTrans"] != null)
                {
                    dt = (DataTable)HttpContext.Current.Session["dataClassTrans"];
                    DataRow[] dtrows = dt.Select("SellSignature = '" + SessionID + "'");

                    //BookingControl booking = new BookingControl();
                    //ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking("");
                    //ABS.Navitaire.BookingManager.Booking book = apiBooking.GetBookingFromState(SessionID);
                    //string xml = GetXMLString(book);

                    if (dtrows.Length == 1)
                    {
                        CollectedAmount = Convert.ToDecimal(dtrows[0]["DetailCollectedAmt"].ToString());
                        ServiceChg = Convert.ToDecimal(dtrows[0]["ServChrg"].ToString());
                        FullPrice = Convert.ToDecimal(dtrows[0]["FullPrice"].ToString());

                        /// amended by diana 20130913
                        //decimal paymentAttempt1 = 0;
                        //paymentAttempt1 = ((FullPrice - ServiceChg) * paymentInfo.Percentage_1) / 100;
                        if (paymentInfo.PaymentType == "SVCF")
                        {
                            if ((ServiceChg - CollectedAmount) >= 0)
                            {
                                return getRounding(ServiceChg - CollectedAmount);
                            }
                        }
                        else
                        {
                            return FullPrice;
                        }
                        /// end amended by diana 20130913

                        /// commented by diana 20130913
                        //if (PayScheme == "B2M" || PayScheme == "W2M" || PayScheme == "W1M" )
                        //{
                        //    if ((ServiceChg - CollectedAmount) >= 0)
                        //    {
                        //        return ServiceChg - CollectedAmount;
                        //    }
                        //}
                        //else
                        //{
                        //    return FullPrice;
                        //}
                        /// end commented by diana 20130913
                    }
                }
            }

            else if (HttpContext.Current.Session["ChgTransMain"] != null)
            {
                DataTable dtTransDetail = objBooking.dtTransDetail();
                dtTransDetail = (DataTable)HttpContext.Current.Session["ChgTransDetail"];
                DataRow[] dtrows = dtTransDetail.Select("RecordLocator = '" + SessionID + "'");
                if (dtrows.Length == 1)
                {
                    CollectedAmount = Convert.ToDecimal(dtrows[0]["DetailCollectedAmount"].ToString());
                    AmountDue = Convert.ToDecimal(dtrows[0]["AmountDue"].ToString());
                    ServiceChg = Convert.ToDecimal(dtrows[0]["LineFee"].ToString());
                    FullPrice = Convert.ToDecimal(dtrows[0]["LineTotal"].ToString());

                    return FullPrice - CollectedAmount;
                }
                else
                {
                    if (HttpContext.Current.Session["dataClassTrans"] != null)
                    {
                        dt = (DataTable)HttpContext.Current.Session["dataClassTrans"];
                        DataRow[] dtrows2 = dt.Select("RecordLocator = '" + SessionID + "'");
                        if (dtrows2.Length == 1)
                        {
                            CollectedAmount = Convert.ToDecimal(dtrows2[0]["DetailCollectedAmt"].ToString());
                            ServiceChg = Convert.ToDecimal(dtrows2[0]["ServChrg"].ToString());
                            FullPrice = Convert.ToDecimal(dtrows2[0]["FullPrice"].ToString());

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
                        }
                    }

                }

            }
            else
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
                    //Added by ketee, expiry date and details expiry date
                    DateTime ExpiryDate = Convert.ToDateTime(dtTransMain.Rows[0]["ExpiryDate"].ToString());
                    DateTime NextDueDate;
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
                        NextDueDate = Convert.ToDateTime(dtrows[0]["NextDueDate"].ToString());
                        if (NextDueDate < ExpiryDate) ExpiryDate = NextDueDate;
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
                        //added by ketee, rounding 2 decimal places, 20150723
                        //modify by ketee, get rounding 20170116
                        paymentAttempt1 = Math.Round((FullPrice * paymentInfo.Percentage_1) / 100, 2);
                        if (paymentInfo.Deposit_1 != 0)
                        {
                            if (deposit == 0)
                            {
                                deposit = objGeneral.getDeposit(objBK_TRANSDTL_Info.TotalPax, objBK_TRANSDTL_Info.Currency, objBK_TRANSDTL_Info.Origin, objBK_TRANSDTL_Info.Transit);
                            }
                            paymentAttempt1 = deposit + ServiceChg;
                        }

                        //amended by diana 20140121 - add condition to differentiate deposit and non deposit
                        if (paymentInfo.Deposit_1 != 0)
                        {
                            //modify by ketee, get rounding 20170116
                            paymentAttempt2 = Math.Round((FullPrice * paymentInfo.Percentage_2) / 100, 2);
                        }
                        else
                        {
                            //modify by ketee, get rounding 20170116
                            paymentAttempt2 = Math.Round((FullPrice * paymentInfo.Percentage_2) / 100, 2) + paymentAttempt1;
                        }
                        if (paymentInfo.Deposit_2 != 0)
                        {
                            if (deposit == 0)
                            {
                                deposit = objGeneral.getDeposit(objBK_TRANSDTL_Info.TotalPax, objBK_TRANSDTL_Info.Currency, objBK_TRANSDTL_Info.Origin, objBK_TRANSDTL_Info.Transit);
                            }
                            paymentAttempt2 = deposit + paymentAttempt1;
                        }

                        //amended by diana 20140121 - add condition to differentiate deposit and non deposit
                        if (paymentInfo.Deposit_2 != 0)
                        {
                            //modify by ketee, get rounding 20170116
                            paymentAttempt3 = Math.Round((FullPrice * paymentInfo.Percentage_3) / 100, 2);
                        }
                        else
                        {
                            //modify by ketee, get rounding 20170116
                            paymentAttempt3 = Math.Round((FullPrice * paymentInfo.Percentage_3) / 100, 2) + paymentAttempt2;
                        }
                        if (paymentInfo.Deposit_3 != 0)
                        {
                            if (deposit == 0)
                            {
                                deposit = objGeneral.getDeposit(objBK_TRANSDTL_Info.TotalPax, objBK_TRANSDTL_Info.Currency, objBK_TRANSDTL_Info.Origin, objBK_TRANSDTL_Info.Transit);
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
                                //Added by ketee, to check payment expiry, 20160223
                                if (ExpiryDate <= BookingDate.AddHours(paymentInfo.Attempt_1))
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
                                if (ExpiryDate <= STD.AddHours(-paymentInfo.Attempt_1))
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
                                    if (ExpiryDate <= BookingDate.AddHours(paymentInfo.Attempt_2))
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
                                    if (ExpiryDate <= STD.AddHours(-paymentInfo.Attempt_2))
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
                                        if (ExpiryDate <= BookingDate.AddHours(paymentInfo.Attempt_3))
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
                                        if (ExpiryDate <= STD.AddHours(-paymentInfo.Attempt_3))
                                            doneattempt3 = false;
                                    }
                                    //}
                                }
                            }
                        }

                        if (doneattempt1 == false)
                            return getRounding(paymentAttempt1 - CollectedAmount);
                        else if (doneattempt2 == false)
                            return getRounding(paymentAttempt2 - CollectedAmount);
                        else if (doneattempt3 == false)
                        {
                            //for last payment , no rounding, must pay full, 20170226 by ketee
                            return paymentAttempt3 - CollectedAmount;
                        }
                        else if (PayScheme == "W1W")
                        {
                            //for immediate payment , no rounding, must pay full, 20170226 by ketee
                            return FullPrice - CollectedAmount;
                        }


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
                                        return getRounding(ServiceChg - CollectedAmount);
                                    }
                                }
                                else
                                {
                                    return FullPrice;
                                }
                                /// end amended by diana 20130913

                                /// commented by diana 20130913
                                //if (PayScheme == "B2M" || PayScheme == "W2M" || PayScheme == "W1M")
                                //{
                                //    if ((ServiceChg - CollectedAmount) >= 0)
                                //    {
                                //        return ServiceChg - CollectedAmount;
                                //    }
                                //}
                                //else
                                //{
                                //    return FullPrice;
                                //}
                                /// end commented by diana 20130913
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

        //added by ketee, roudning amount , 20170116
        public decimal getRounding(decimal value)
        {
            decimal rounding = decimal.Zero;
            if (value.ToString().Split('.').Length > 1)
            {
                switch (value.ToString().Split('.')[1].Substring(1, 1))
                {
                    case "1":
                    case "6":
                        rounding = value + (decimal)-0.01;
                        break;
                    case "2":
                    case "7":
                        rounding = value + (decimal)-0.02;
                        break;
                    case "3":
                    case "8":
                        rounding = value + (decimal)0.02;
                        break;
                    case "4":
                    case "9":
                        rounding = value + (decimal)0.01;
                        break;
                    default:
                        return value;
                }
            }
            return rounding;

        }

        private void KeepAlive()
        {
            DataTable dt;
            string sellSignature;
            if (HttpContext.Current.Session["dataClassTrans"] != null)
            {
                dt = (DataTable)HttpContext.Current.Session["dataClassTrans"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        sellSignature = dr["SellSignature"].ToString();

                        absNavitaire.KeepAlive(sellSignature);
                    }
                }
            }
        }

        public void SetScreen()
        {
            var profiler = MiniProfiler.Current;
            try
            {
                //outputDirectDebitProviders();

                if (!IsPostBack)
                {
                    using (profiler.Step("LoadCreditCard"))
                    {
                        LoadCreditCardDefaultDate(true);
                    }
                    using (profiler.Step("LoadIssuingCountry"))
                    {
                        LoadIssuingCountry();
                    }
                    using (profiler.Step("LoadCountry"))
                    {
                        LoadCountryStateAddress();
                    }
                    using (profiler.Step("LoadContactCountry"))
                    {
                        LoadContactCountryStateAddress();
                    }
                    using (profiler.Step("LoadContact"))
                    {
                        LoadContactDetails();
                    }
                    using (profiler.Step("LoadState"))
                    {
                        LoadStateAddress();
                    }
                    using (profiler.Step("LoadContactState"))
                    {
                        LoadContactStateAddress();
                    }

                }
                else
                    using (profiler.Step("LoadCreditCard"))
                    {
                        LoadCreditCardDefaultDate();
                    }
                using (profiler.Step("CheckError"))
                {
                    CheckErrorSession();
                }
                using (profiler.Step("LoadProcessingFee"))
                {
                    LoadProcessingFee();
                }
                using (profiler.Step("CheckProcessingFee"))
                {
                    CheckProcessingFee();
                }

                //to hide panel of processing fee
                //panelProcessFee.Visible = false;

                if (MyUserSet.AgentType.ToString().ToLower() == "skyagent")
                {
                    using (profiler.Step("creditResponse"))
                    {
                        //ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse creditResponse = GetAGCredit();
                        ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse creditResponse = AGCredit();

                        if (creditResponse != null)
                        {
                            if (creditResponse.AvailableCreditResponse.Account != null) //amended by diana 20140409, show only if account is not null
                            {
                                lblAGCreditAmount.Text = objGeneral.RoundUp(creditResponse.AvailableCreditResponse.Account.ForeignAmount).ToString("N", nfi);
                                lblAGCreditCurrency.Text = creditResponse.AvailableCreditResponse.Account.ForeignCurrencyCode;
                            }
                            else
                            {
                                lblAGCreditAmount.Text = "0";
                            }
                        }
                        else
                        {
                            lblAGCreditAmount.Text = "0";
                        }

                        TabControl.TabPages.FindByName("TabAG").Visible = true;
                        //TabControl.TabPages.FindByName("TabCreditAccount").Visible = false;
                    }
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
                Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.SessionExpired);
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
        }
        public void LoadContactCountryStateAddress()
        {

            UIClass.SetComboStyle(ref cmbContactCountryAddress, UIClass.EnumDefineStyle.Country);
        }
        public void LoadStateAddress()
        {
            UIClass.SetComboStyle(ref cmbState, UIClass.EnumDefineStyle.State, "", cmbCountryAddress.Value.ToString());
            if (Session["cmbState"] != null)
                cmbState.Value = Session["cmbState"].ToString();
        }
        public void LoadContactStateAddress()
        {

            UIClass.SetComboStyle(ref cmbContactState, UIClass.EnumDefineStyle.State, "", cmbContactCountryAddress.Value.ToString());
            if (Session["cmbState"] != null)
                cmbContactState.Value = Session["cmbState"].ToString();
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
            //Added and amended by Ellis 20170307, to make it possible to use cookies for contact details
            agent = objAgentProfile.GetSingleAgentProfile(MyUserSet.AgentName);
            if (chkRememberMe.Checked && Request.Cookies["RememberMe"] != null && Request.Cookies["RememberMe"]["AgentID"] != null && Request.Cookies["RememberMe"]["AgentID"].ToString() == agent.AgentID)
            {
                if (Request.Cookies["RememberMe"] != null)
                {
                    if (Request.Cookies["RememberMe"]["AgentID"].ToString() == agent.AgentID)
                    {
                        if (Request.Cookies["RememberMe"]["Title"] != null)
                        {
                            if (Request.Cookies["RememberMe"]["Title"].ToString() != "")
                            {
                                cmbContactTitle.Value = Request.Cookies["RememberMe"]["Title"].ToString();
                            }
                        }
                        txtContactFirstName.Text = Request.Cookies["RememberMe"]["FirstName"].ToString();
                        txtContactLastName.Text = Request.Cookies["RememberMe"]["LastName"].ToString();
                        txtContactEmail.Text = Request.Cookies["RememberMe"]["Email"].ToString();
                        txtContactPhone.Text = Request.Cookies["RememberMe"]["PhoneNo"].ToString();
                        txtContactAddress.Text = Request.Cookies["RememberMe"]["Address1"].ToString();
                        txtContactAddress2.Text = Request.Cookies["RememberMe"]["Address2"].ToString();
                        txtContactTown.Text = Request.Cookies["RememberMe"]["City"].ToString();
                        //txtTown.Text = Request.Cookies["RememberMe"]["City"].ToString();
                        //txtAddress.Text = Request.Cookies["RememberMe"]["Address1"].ToString();
                        if (Request.Cookies["RememberMe"]["Country"] != null)
                        {
                            if (Request.Cookies["RememberMe"]["Country"].ToString() != "")
                            {
                                cmbContactCountryAddress.Value = Request.Cookies["RememberMe"]["Country"].ToString();
                                cmbIssuingCountry.Value = Request.Cookies["RememberMe"]["Country"].ToString();
                                cmbCountryAddress.Value = Request.Cookies["RememberMe"]["Country"].ToString();
                            }
                        }
                        if (Request.Cookies["RememberMe"]["State"] != null)
                        {
                            if (Request.Cookies["RememberMe"]["State"].ToString() != "")
                            {
                                cmbContactState.Value = Request.Cookies["RememberMe"]["State"].ToString();
                                cmbState.Value = Request.Cookies["RememberMe"]["State"].ToString();
                                Session["cmbState"] = Request.Cookies["RememberMe"]["State"].ToString();
                            }
                        }
                        txtContactZipCode.Text = Request.Cookies["RememberMe"]["PostCode"].ToString();
                        txtZipCode.Text = Request.Cookies["RememberMe"]["PostCode"].ToString();
                    }
                }
            }
            else
            {
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
                txtContactAddress2.Text = agent.Address2;
                txtContactTown.Text = agent.City;
                if (agent.Country != null)
                {
                    if (agent.Country.ToString() != "")
                    {
                        cmbContactCountryAddress.Value = agent.Country;
                        cmbIssuingCountry.Value = agent.Country;
                        cmbCountryAddress.Value = agent.Country;
                    }
                }
                if (agent.State != null)
                {
                    if (agent.State.ToString() != "")
                    {
                        cmbContactState.Value = agent.State;
                        cmbState.Value = agent.State;
                        Session["cmbState"] = agent.State;
                    }
                }
                txtContactZipCode.Text = agent.Postcode;
                txtZipCode.Text = agent.Postcode;
            }
            //agent = objAgentProfile.GetSingleAgentProfile(MyUserSet.AgentName);
            //if (agent.Title != null)
            //{
            //    if (agent.Title.ToString() != "")
            //    {
            //        cmbContactTitle.Value = agent.Title;
            //    }
            //}
            //txtContactFirstName.Text = agent.ContactFirstName;
            //txtContactLastName.Text = agent.ContactLastName;
            //txtContactEmail.Text = agent.Email;
            //txtContactPhone.Text = agent.PhoneNo;
            //txtContactAddress.Text = agent.Address1;
            //txtContactTown.Text = agent.City;
            //if (agent.Country != null)
            //{
            //    if (agent.Country.ToString() != "")
            //    {
            //        cmbContactCountryAddress.Value = agent.Country;
            //    }
            //}
            //if (agent.State != null)
            //{
            //    if (agent.State.ToString() != "")
            //    {
            //        cmbContactState.Value = agent.State;
            //    }
            //}
            //txtContactZipCode.Text = agent.Postcode;
            //End of Added and amended by Ellis 20170307
            //}
        }

        public void LoadContactDetailsState()
        {

            //Added and amended by Ellis 20170307, to make it possible to use cookies for contact details
            //agent = objAgentProfile.GetSingleAgentProfile(MyUserSet.AgentName);
            if (chkRememberMe.Checked && Request.Cookies["RememberMe"] != null && Request.Cookies["RememberMe"]["AgentID"] != null && Request.Cookies["RememberMe"]["AgentID"].ToString() == agent.AgentID)
            {
                if (Request.Cookies["RememberMe"] != null)
                {
                    if (Request.Cookies["RememberMe"]["AgentID"].ToString() == agent.AgentID)
                    {
                        if (Request.Cookies["RememberMe"]["Title"] != null)
                        {
                            if (Request.Cookies["RememberMe"]["Title"].ToString() != "")
                            {
                                cmbContactTitle.Value = Request.Cookies["RememberMe"]["Title"].ToString();
                            }
                        }
                        txtContactFirstName.Text = Request.Cookies["RememberMe"]["FirstName"].ToString();
                        txtContactLastName.Text = Request.Cookies["RememberMe"]["LastName"].ToString();
                        txtContactEmail.Text = Request.Cookies["RememberMe"]["Email"].ToString();
                        txtContactPhone.Text = Request.Cookies["RememberMe"]["PhoneNo"].ToString();
                        txtContactAddress.Text = Request.Cookies["RememberMe"]["Address1"].ToString();
                        txtContactAddress2.Text = Request.Cookies["RememberMe"]["Address2"].ToString();
                        txtContactTown.Text = Request.Cookies["RememberMe"]["City"].ToString();
                        //txtTown.Text = Request.Cookies["RememberMe"]["City"].ToString();
                        //txtAddress.Text = Request.Cookies["RememberMe"]["Address1"].ToString();
                        if (Request.Cookies["RememberMe"]["Country"] != null)
                        {
                            if (Request.Cookies["RememberMe"]["Country"].ToString() != "")
                            {
                                cmbContactCountryAddress.Value = Request.Cookies["RememberMe"]["Country"].ToString();
                                cmbIssuingCountry.Value = Request.Cookies["RememberMe"]["Country"].ToString();
                                cmbCountryAddress.Value = Request.Cookies["RememberMe"]["Country"].ToString();
                            }
                        }
                        if (Request.Cookies["RememberMe"]["State"] != null)
                        {
                            if (Request.Cookies["RememberMe"]["State"].ToString() != "")
                            {
                                cmbContactState.Value = Request.Cookies["RememberMe"]["State"].ToString();
                                cmbState.Value = Request.Cookies["RememberMe"]["State"].ToString();
                            }
                        }
                        txtContactZipCode.Text = Request.Cookies["RememberMe"]["PostCode"].ToString();
                        txtZipCode.Text = Request.Cookies["RememberMe"]["PostCode"].ToString();
                    }
                }
            }
            else
            {
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
                txtContactAddress2.Text = agent.Address2;
                txtContactTown.Text = agent.City;
                if (agent.Country != null)
                {
                    if (agent.Country.ToString() != "")
                    {
                        cmbContactCountryAddress.Value = agent.Country;
                        cmbIssuingCountry.Value = agent.Country;
                        cmbCountryAddress.Value = agent.Country;
                    }
                }
                if (agent.State != null)
                {
                    if (agent.State.ToString() != "")
                    {
                        cmbContactState.Value = agent.State;
                        cmbState.Value = agent.State;
                    }
                }
                txtContactZipCode.Text = agent.Postcode;
                txtZipCode.Text = agent.Postcode;
            }
            //End of Added and amended by Ellis 20170307
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
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;
            try
            {
                lstbookFlightInfo = new List<BookingTransactionDetail>();
                using (profiler.Step("GetAllBK_TRANSDTLFlight"))
                {
                    lstbookFlightInfo = objBooking.GetAllBK_TRANSDTLFlight(TransID);
                }
                if (lstbookFlightInfo == null)
                {
                    lstbookFlightInfo = (List<BookingTransactionDetail>)Session["ChglstbookDTLInfo"];
                }

                if (lstbookFlightInfo != null)
                {
                    //int lastIndex = lstbookFlightInfo.Count - 1;

                    //added by ketee
                    hCarrierCode.Value = lstbookFlightInfo[0].CarrierCode;
                }

                //lblDepartOrigin.Text = lstbookFlightInfo[0].Origin; // objGeneral.GetCityNameByCode(lstbookFlightInfo[0].Origin) + "(" + lstbookFlightInfo[0].Origin + ")";
                //lblDepartDestination.Text = lstbookFlightInfo[0].Destination; // objGeneral.GetCityNameByCode(lstbookFlightInfo[0].Destination) + "(" + lstbookFlightInfo[0].Destination + ")";

                //lblDateDepart.Text = lstbookFlightInfo[0].CarrierCode + " " + lstbookFlightInfo[0].FlightNo; // + " - " + String.Format("{0:dd MMM yyyy}", lstbookFlightInfo[0].DepatureDate);

                //lblDepartStd.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", lstbookFlightInfo[0].DepatureDate);

                //DateTime tempdate1 = lstbookFlightInfo[0].ArrivalDate;
                //DateTime tempdate2 = lstbookFlightInfo[0].DepatureDate;
                //if (lstbookFlightInfo[0].Transit != "")
                //    tempdate1 = lstbookFlightInfo[0].ArrivalDate2;
                //TimeSpan ts = tempdate1.Date - tempdate2.Date;
                //string temp = "";
                //if (ts.Days > 0)
                //{
                //    temp = " (+" + ts.TotalDays.ToString() + ")";
                //}

                ////if (lstbookFlightInfo[0].Transit != "")
                ////{
                ////    lblTransitDepart.Text = lstbookFlightInfo[0].Transit; // objGeneral.GetCityNameByCode(lstbookFlightInfo[0].Transit) + "(" + lstbookFlightInfo[0].Transit + ")";
                ////    lblDateTransitDepart.Text = String.Format("{0:dd MMM yyyy}", lstbookFlightInfo[0].DepatureDate2) + " " + String.Format("{0:HHmm}", lstbookFlightInfo[0].DepatureDate2) + " - " + String.Format("{0:HHmm}", lstbookFlightInfo[0].ArrivalDate2);
                ////    lblDepartSta.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", lstbookFlightInfo[0].ArrivalDate2) + temp;
                ////}
                ////else
                ////{
                ////    lblTransitDepart.Text = "";
                ////    lblDateTransitDepart.Text = "";
                ////    lblTextTransitDepart.Text = "";
                ////    lblDepartSta.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", lstbookFlightInfo[0].ArrivalDate) + temp;
                ////}

                //load carrier code
                if (HttpContext.Current.Session["PaymentMaxAttempt"] == null)
                {
                    string carrierSetting;// = objGeneral.getSysValueByKeyAndCarrierCode("PAYMENTSUSPEND", lstbookFlightInfo[0].CarrierCode);
                    using (profiler.Step("getSysValueByKeyAndCarrierCode"))
                    {
                        carrierSetting = objGeneral.getSysValueByKeyAndCarrierCode("PAYMENTSUSPEND", lstbookFlightInfo[0].CarrierCode);
                    }
                    HttpContext.Current.Session.Add("PaymentMaxAttempt", carrierSetting);
                }

                //bool returnFlight = false;
                //if (lstbookFlightInfo.Count > 1)
                //{
                //    if (lstbookFlightInfo[0].Origin.ToString() != lstbookFlightInfo[lastIndex].Origin.ToString())
                //    {
                //        returnFlight = true;
                //    }
                //}
                //if (returnFlight)
                //{
                //    //lblReturnOrigin.Text = lstbookFlightInfo[lastIndex].Origin; // objGeneral.GetCityNameByCode(lstbookFlightInfo[lastIndex].Origin) + "(" + lstbookFlightInfo[lastIndex].Origin + ")";
                //    //lblReturnDestination.Text = lstbookFlightInfo[lastIndex].Destination; // objGeneral.GetCityNameByCode(lstbookFlightInfo[lastIndex].Destination) + "(" + lstbookFlightInfo[lastIndex].Destination + ")";

                //    //lblDateReturn.Text = lstbookFlightInfo[lastIndex].CarrierCode + " " + lstbookFlightInfo[lastIndex].FlightNo; // + " - " + String.Format("{0:dd MMM yyyy}", lstbookFlightInfo[lastIndex].DepatureDate);

                //    //lblReturnStd.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", lstbookFlightInfo[lastIndex].DepatureDate);

                //    tempdate1 = lstbookFlightInfo[lastIndex].ArrivalDate;
                //    tempdate2 = lstbookFlightInfo[lastIndex].DepatureDate;
                //    if (lstbookFlightInfo[lastIndex].Transit != "")
                //        tempdate1 = lstbookFlightInfo[lastIndex].ArrivalDate2;
                //    ts = tempdate1.Date - tempdate2.Date;
                //    temp = "";
                //    if (ts.Days > 0)
                //    {
                //        temp = " (+" + ts.TotalDays.ToString() + ")";
                //    }

                //    //if (lstbookFlightInfo[lastIndex].Transit != "")
                //    //{
                //    //    lblTransitReturn.Text = objGeneral.GetCityNameByCode(lstbookFlightInfo[lastIndex].Transit) + "(" + lstbookFlightInfo[lastIndex].Transit + ")";
                //    //    lblTransitDateReturn.Text = String.Format("{0:dd MMM yyyy}", lstbookFlightInfo[lastIndex].DepatureDate2) + " " + String.Format("{0:HHmm}", lstbookFlightInfo[lastIndex].DepatureDate2) + " - " + String.Format("{0:HHmm}", lstbookFlightInfo[lastIndex].ArrivalDate2);
                //    //    lblReturnSta.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", lstbookFlightInfo[lastIndex].ArrivalDate2) + temp;
                //    //}
                //    //else
                //    //{
                //    //    lblTransitReturn.Text = "";
                //    //    lblTransitDateReturn.Text = "";
                //    //    lblTextTransitReturn.Text = "";
                //    //    lblReturnSta.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", lstbookFlightInfo[lastIndex].ArrivalDate) + temp;
                //    //}

                //}
                //else
                //{
                //    //pnlReturn.Visible = false;
                //    //trReturnIcon.Visible = false;
                //}
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

                ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse accResp = new ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse();
                if (Session["AccResp"] == null)
                {
                    accResp = absNavitaire.GetCreditByAccountReference(accountReference, lblCurrentTotalCurrency.Text, signature);
                    return accResp;
                }
                else
                {
                    accResp = (ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse)Session["AccResp"];
                    return accResp;
                }
                //ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse accResp = absNavitaire.GetCreditByAccountReference(accountReference, lblCurrentTotalCurrency.Text, signature);
                //return accResp;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                return null;
            }
        }

        public void LoadProcessingFee()
        {
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;

            ABS.Navitaire.BookingManager.GetBookingResponse bookingResponse = new ABS.Navitaire.BookingManager.GetBookingResponse();
            lblProcessFee.Text = "0.00";
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


                    Boolean PNRExist = false;
                    string PNR = "";
                    if (rdlPNR.SelectedItem != null)
                    {
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

                        //amended by diana 20170211, to retrieve processing fee from radio button
                        decimal processingFee = decimal.Parse(rdlProcessFee.Items[rdlPNR.SelectedIndex].Value.ToString()); // getTotalProceessFee(dataClass, PNR, lblCurrentTotalCurrency.Text);
                        lblProcessFee.Text = rdlProcessFee.Items[rdlPNR.SelectedIndex].Text; //processingFee.ToString("N", nfi);
                        lblCurrencyProcessFee.Text = lblCurrentTotalCurrency.Text;

                        HttpContext.Current.Session.Remove("TotalProcessFee");
                        HttpContext.Current.Session.Add("TotalProcessFee", processingFee);
                    }
                    //updateTotalWithProcessingFee(Convert.ToDecimal(lblAmountDue.Text), processingFee);
                }
            }
            else if (HttpContext.Current.Session["ChgTransMain"] != null)
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
                    if (HttpContext.Current.Session["ChgTransDetail"] != null)
                    {
                        DataTable dtTransDetail = objBooking.dtTransDetail();
                        dtTransDetail = (DataTable)HttpContext.Current.Session["ChgTransDetail"];
                        DataTable dtProcessFee = new DataTable();
                        dtProcessFee.Columns.Add("SellSignature");
                        dtProcessFee.Columns.Add("FullPrice");
                        string errMessage = "";
                        for (int idx = 0; idx < dtTransDetail.Rows.Count; idx++)
                        {
                            DataRow row;
                            row = dtProcessFee.NewRow();
                            using (profiler.Step("Navitaire:GetBooking"))
                            {
                                row["SellSignature"] = absNavitaire.GetBookingByPNR(dtTransDetail.Rows[idx]["RecordLocator"].ToString(), ref errMessage, ref bookingResponse);
                            }

                            //row["FullPrice"] = Convert.ToDecimal(dtTransDetail.Rows[idx]["LineTotal"]);
                            row["FullPrice"] = Convert.ToDecimal(dtTransDetail.Rows[idx]["AmountDue"]);


                            dtProcessFee.Rows.Add(row);
                        }

                        Boolean PNRExist = false;
                        string PNR = "";
                        if (rdlPNR.SelectedItem != null && rdlPNR.SelectedIndex > 0 && rdlProcessFee.Items.Count > 0)
                        {
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

                            //amended by diana 20170211, to retrieve processing fee from radio button
                            decimal processingFee = decimal.Parse(rdlProcessFee.Items[rdlPNR.SelectedIndex].Value.ToString()); // getTotalProceessFee(dtProcessFee, PNR, lblCurrentTotalCurrency.Text);
                            lblProcessFee.Text = rdlProcessFee.Items[rdlPNR.SelectedIndex].Text; //processingFee.ToString("N", nfi);
                            lblCurrencyProcessFee.Text = lblCurrentTotalCurrency.Text;

                            HttpContext.Current.Session.Remove("TotalProcessFee");
                            HttpContext.Current.Session.Add("TotalProcessFee", processingFee);
                        }
                        //updateTotalWithProcessingFee(Convert.ToDecimal(lblAmountDue.Text), processingFee);

                    }
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
                            using (profiler.Step("Navitaire:GetBooking"))
                            {
                                row["SellSignature"] = absNavitaire.GetBookingByPNR(dtTransDetail.Rows[idx]["RecordLocator"].ToString(), ref errMessage, ref bookingResponse);
                            }

                            //row["FullPrice"] = Convert.ToDecimal(dtTransDetail.Rows[idx]["LineTotal"]);
                            row["FullPrice"] = Convert.ToDecimal(dtTransDetail.Rows[idx]["AmountDue"]);

                            dtProcessFee.Rows.Add(row);
                        }

                        Boolean PNRExist = false;
                        string PNR = "";
                        if (rdlPNR.SelectedItem != null && rdlPNR.SelectedIndex > 0 && rdlProcessFee.Items.Count > 0)
                        {
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

                            //amended by diana 20170211, to retrieve processing fee from radio button
                            decimal processingFee = decimal.Parse(rdlProcessFee.Items[rdlPNR.SelectedIndex].Value.ToString()); // getTotalProceessFee(dtProcessFee, PNR, lblCurrentTotalCurrency.Text);
                            lblProcessFee.Text = rdlProcessFee.Items[rdlPNR.SelectedIndex].Text; //processingFee.ToString("N", nfi);
                            lblCurrencyProcessFee.Text = lblCurrentTotalCurrency.Text;

                            HttpContext.Current.Session.Remove("TotalProcessFee");
                            HttpContext.Current.Session.Add("TotalProcessFee", processingFee);
                        }
                        //updateTotalWithProcessingFee(Convert.ToDecimal(lblAmountDue.Text), processingFee);

                    }
                }
            }
            bookingResponse = null;
        }

        protected decimal getTotalProceessFee(DataTable dataClass, string SessionID, string currency)
        {
            var profiler = MiniProfiler.Current;
            try
            {
                decimal totalProcess = 0;

                for (int i = 0; i < dataClass.Rows.Count; i++)
                {
                    if (dataClass.Rows[i]["SellSignature"].ToString() == SessionID)
                    {
                        ABS.Navitaire.BookingManager.GetPaymentFeePriceResponse feeResponse = new GetPaymentFeePriceResponse();// absNavitaire.GetProcessingFee(dataClass.Rows[i]["SellSignature"].ToString(), currency, Convert.ToDecimal(dataClass.Rows[i]["FullPrice"]));
                        using (profiler.Step("Navitaire:AgentLogon"))
                        {
                            feeResponse = absNavitaire.GetProcessingFee(dataClass.Rows[i]["SellSignature"].ToString(), currency, Convert.ToDecimal(dataClass.Rows[i]["FullPrice"]));
                        }
                        totalProcess = getSingleProcessingFeeByResponse(feeResponse);

                    }
                }

                //added by ketee, temp get processing fees if return 0
                //if (totalProcess == 0)
                //{
                //    string signature = absNavitaire.AgentLogon();
                //    for (int i = 0; i < dataClass.Rows.Count; i++)
                //    {
                //        if (dataClass.Rows[i]["SellSignature"].ToString() == SessionID)
                //        {
                //            //ABS.Navitaire.BookingManager.GetBookingResponse response = absNavitaire.GetBookingResponseByPNRSignature(dataClass.Rows[i]["RecordLocator"].ToString(), signature);
                //            ABS.Navitaire.BookingManager.GetPaymentFeePriceResponse feeResponse = absNavitaire.GetProcessingFee(signature, currency, Convert.ToDecimal(dataClass.Rows[i]["FullPrice"]));
                //            totalProcess += getSingleProcessingFeeByResponse(feeResponse);
                //        }
                //    }
                //}

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
                        for (int a = 0; a < feeResponse.paymentFeePriceRespData.PassengerFees[i].ServiceCharges.Length; a++)
                        {
                            totalProcess += feeResponse.paymentFeePriceRespData.PassengerFees[i].ServiceCharges[a].ForeignAmount;
                        }

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
            }
            else if (Session["reloadPage"].ToString() != "true")
            {
                lblErrorTop.Text = "";
                lblErrorBottom.Text = "";
                imgError.Visible = false;
                lblErrorTop.Visible = false;
            }
        }

        protected Boolean checkPaymentAttempt()
        {
            log.Info(this, "Entering checkPaymentAttempt()");
            int attempt = 0; //default 
            if (HttpContext.Current.Session["PaymentAttempt"] != null)
            {
                attempt = Convert.ToInt32(HttpContext.Current.Session["PaymentAttempt"]);
            }

            int maxTry = Convert.ToInt32(HttpContext.Current.Session["PaymentMaxAttempt"]);

            if (paymentSuccess == false)
                attempt += 1;
            HttpContext.Current.Session.Remove("PaymentAttempt");
            HttpContext.Current.Session.Add("PaymentAttempt", attempt);

            log.Info(this, "Payment Attempt : " + attempt + " and Max Try : " + maxTry);
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
                    if (cbFullPayment.Checked == false || txtPayAmount.Text.ToString() == "0")
                        txtPayAmount.Text = txtMinimumPayment.Text;
                    ViewState["MinPaymeny"] = txtMinimumPayment.Text;
                    ViewState["savedMinPay"] = null;
                    break;
                case "TabAG":
                    divAG.Visible = true;


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
                    if (cbFullPayment.Checked == false || txtPayAmount.Text.ToString() == "0")
                        txtPayAmount.Text = txtMinimumPayment.Text;
                    ViewState["MinPaymeny"] = txtMinimumPayment.Text;
                    ViewState["savedMinPay"] = null;

                    break;
                //case "TabAG":
                //    if (ViewState["savedMinPay"] == null && rdlPNR.SelectedIndex != -1 && txtMinPay.Text != "")
                //    {
                //        ViewState["MinPaymeny"] = txtMinPay.Text;

                //        ViewState["savedMinPay"] = "1";

                //        lblMinPay.Text = txtDueAmount.Text;
                //        txtMinPay.Text = txtDueAmount.Text;
                //        txtPayAmount.Text = txtDueAmount.Text;
                //    }
                //    divAG.Visible = true;
                //    break;
                case "TabCreditAccount":

                    break;
                case "TabDirectDebit":

                    break;

            }

            ////SetScreen();
            ////InitializeForm();
            ////CheckProcessingFee();
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
            log.Info(this, "UpdateSessionDataClassTrans : " + str);
        }

        #endregion

        #region Credit Card Function
        protected void CheckProcessingFee()
        {
            //commented by diana 20131031
            //panelProcessFee.Visible = false;
            string tabname = TabControl.ActiveTabPage.Name;
            if (tabname == "TabCredit")//Added By Agus
            {
                ////trProcessingFee.Style.Add("display", "block");

            }
            else
            {
                ////trProcessingFee.Style.Add("display", "none");
            }
        }
        #endregion

        protected string MakePayment(string PaymentType, string SessionID, Boolean PNRExist = false, string amount = "")
        {
            paymentSuccess = false;
            Session["generatePayment"] = "true";
            DataTable dataClass = objBooking.dtClassTrans();
            MessageList msgList = new MessageList();
            DataTable dttemp = new DataTable();
            decimal paymentAmount = 0, totalPaid = 0, TransTotal = 0;
            string sellSignature = "", TransID = "", errMessage = "", PNR = "";
            Boolean AddPaymentStatus = false;
            lstbookDTLInfo = new List<BookingTransactionDetail>();
            bookDTLInfo = new BookingTransactionDetail();
            bookTransTenderInfo = new BookingTransTender();
            string status = "", pass = "";
            string accNumber = "";
            long accID = 0;
            ABS.Navitaire.BookingManager.GetBookingResponse bookingResponse = new ABS.Navitaire.BookingManager.GetBookingResponse();
            ABS.Navitaire.BookingManager.Booking booking = new ABS.Navitaire.BookingManager.Booking();

            //added by diana 2013119 - to insert hidden value to combobox - contact county and state


            if (txtContactCountryAddress.Text != "")
                cmbContactCountryAddress.Value = txtContactCountryAddress.Text;
            if (contactState != "")
                cmbContactState.Value = contactState;
            txtContactState.Text = contactState;

            //added by diana 2013119 - to insert hidden value to combobox - contact county and state
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;
            try
            {
                Decimal Payamount = 0;
                string AgentName = "";


                if (txtPayAmount.Text != null) Payamount = Convert.ToDecimal(txtPayAmount.Text.ToString());
                if (MyUserSet != null) AgentName = MyUserSet.AgentName.ToString();
                switch (PaymentType)
                {
                    case "TabAG":
                        Payamount = Convert.ToDecimal(amount);
                        ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse creditResponse = AGCredit();
                        if (creditResponse != null && creditResponse.AvailableCreditResponse != null && creditResponse.AvailableCreditResponse.Account != null)
                        {
                            accNumber = creditResponse.AvailableCreditResponse.Account.AccountReference; //not use
                            accID = creditResponse.AvailableCreditResponse.Account.AccountID;
                            pass = creditResponse.AvailableCreditResponse.Account.Password.ToString();
                        }
                        break;
                }

                using (profiler.Step("GetSingleAgentProfile"))
                {
                    agent = objAgentProfile.GetSingleAgentProfile(AgentName);
                }

                paymentAmount = Convert.ToDecimal(Payamount);

                if (HttpContext.Current.Session["dataClassTrans"] != null && PNRExist == false) //first time payment
                {
                    log.Info(this, "Entering (1)");
                    dataClass = (DataTable)HttpContext.Current.Session["dataClassTrans"];
                    //sellSignature = dataClass.Rows[0]["SellSignature"].ToString();

                    log.Info(this, "Counting dataClassTrans : " + dataClass.Rows.Count);
                    for (int i = 0; i < dataClass.Rows.Count; i++)
                    {
                        sellSignature = dataClass.Rows[i]["SellSignature"].ToString();

                        try
                        {
                            string CardType = "", CardNumber = "", ExpiryYear = "", ExpiryMonth = "", CurrentTotalCurrency = "", CVV2 = "", CardHolderName = "", CardIssuer = "", IssuingCountry = "";
                            if (cmbCardType.SelectedItem.Value != null) CardType = cmbCardType.SelectedItem.Value.ToString();
                            if (txtCardNumber.Text != null) CardNumber = txtCardNumber.Text.ToString();
                            if (cmbExpiryYear.SelectedItem.Value != null) ExpiryYear = cmbExpiryYear.SelectedItem.Value.ToString();
                            if (cmbExpiryMonth.SelectedItem.Value != null) ExpiryMonth = cmbExpiryMonth.SelectedItem.Value.ToString();
                            if (lblCurrentTotalCurrency.Text != null) CurrentTotalCurrency = lblCurrentTotalCurrency.Text.ToString();
                            if (txtCVV2.Text != null) CVV2 = txtCVV2.Text.ToString();
                            if (txtCardHolderName.Text != null) CardHolderName = txtCardHolderName.Text.ToString();
                            if (txtCardIssuer.Text != null) CardIssuer = txtCardIssuer.Text.ToString();
                            if (cmbIssuingCountry.Value != null) IssuingCountry = cmbIssuingCountry.Value.ToString();

                            string Address = "", Town = "", CountryAddress = "", States = "", ZipCode = "";

                            if (txtAddress.Text != null) Address = txtAddress.Text.ToString();
                            if (txtTown.Text != null) Town = txtTown.Text.ToString();
                            if (cmbCountryAddress.Value != null) CountryAddress = cmbCountryAddress.Value.ToString();
                            if (cmbState.Value != null) States = cmbState.Value.ToString();
                            if (txtZipCode.Text != null) ZipCode = txtZipCode.Text.ToString();


                            decimal FullPrice = Convert.ToDecimal(dataClass.Rows[i]["FullPrice"].ToString());
                            decimal CollectedAmount = Convert.ToDecimal(dataClass.Rows[i]["DetailCollectedAmt"].ToString());
                            BookingTransactionDetail SingleDTL = new BookingTransactionDetail();
                            using (profiler.Step("GetSingle_TRANSDTLBySellKey"))
                            {
                                SingleDTL = objBooking.GetSingle_TRANSDTLBySellKey(sellSignature);
                            }
                            string prevPNR = "";
                            if (SingleDTL != null)
                                prevPNR = SingleDTL.RecordLocator.Trim();

                            log.Info(this, "SellSignature = " + sellSignature + "; SessionID = " + SessionID);


                            if (sellSignature == SessionID)
                            {
                                using (profiler.Step("Navitaire:GetBookingFromState"))
                                {
                                    booking = absNavitaire.GetBookingFromState(SessionID, 2);
                                }



                                //added by ketee, validate if navitaire is not over paid, 20170321

                                if (booking != null && booking.BookingSum.BalanceDue < paymentAmount)
                                {
                                    status = msgList.Err100073;
                                    errMessage = msgList.Err100073A;
                                }
                                else
                                {
                                    if (absNavitaire.RemovePaymentfromBooking(booking, SessionID))
                                    {
                                        switch (PaymentType)
                                        {

                                            case "TabCredit":
                                                AddPaymentStatus = absNavitaire.AddPaymentCreditCard(CardType, CardNumber, ExpiryYear + "-" + ExpiryMonth + "-" + "01", CurrentTotalCurrency, paymentAmount, CVV2, CardHolderName, CardIssuer, IssuingCountry, sellSignature,
                                                    Address, Town, CountryAddress, States, ZipCode, "", ref errMessage);
                                                break;
                                            case "TabAG":
                                                //if (paymentAmount >= FullPrice - CollectedAmount)
                                                //{
                                                using (profiler.Step("Navitaire:AddPaymentToBooking"))
                                                {
                                                    AddPaymentStatus = absNavitaire.AddAgencyPayment(paymentAmount, CurrentTotalCurrency, accNumber, pass, sellSignature, accID, "", ref errMessage);
                                                }
                                                //}
                                                //else
                                                //{
                                                //    errMessage = msgList.Err100039;
                                                //}
                                                break;
                                        }
                                        log.Info(this, "Value of AddPaymentStatus = " + AddPaymentStatus);
                                        if (AddPaymentStatus)
                                        {
                                            string contactTitle = "", contactFirstName = "", contactlastName = "", contactEmail = "", contactPhone = "", contactAddress = "", contactTown = "", contactCountryAddress = "", contactStates = "", contactZipCode = "", OrganizationName = "", OrgID = "";
                                            if (cmbContactTitle.Value != null) contactTitle = cmbContactTitle.Value.ToString();
                                            if (txtContactFirstName.Text != null) contactFirstName = txtContactFirstName.Text.ToString();
                                            if (txtContactLastName.Text != null) contactlastName = txtContactLastName.Text.ToString();
                                            if (txtContactEmail.Text != null) contactEmail = txtContactEmail.Text.ToString();
                                            if (txtContactPhone.Text != null) contactPhone = txtContactPhone.Text.ToString();
                                            if (txtContactAddress.Text != null) contactAddress = txtContactAddress.Text.ToString();
                                            if (txtContactTown.Text != null) contactTown = txtContactTown.Text.ToString();
                                            if (cmbContactCountryAddress.Value != null) contactCountryAddress = cmbContactCountryAddress.Value.ToString();
                                            if (cmbContactState.Value != null) contactStates = cmbContactState.Value.ToString();
                                            if (txtContactZipCode.Text != null) contactZipCode = txtContactZipCode.Text.ToString();
                                            if (agent != null) OrgID = agent.OrgID.ToString();
                                            if (MyUserSet != null) OrganizationName = MyUserSet.OrganizationName.ToString();

                                            //PNR = objBooking.BookingCommitAddInfant(booking, agent, Convert.ToInt16(dataClass.Rows[i]["Quantity"]), Convert.ToDecimal(dataClass.Rows[i]["FarePrice"]), dataClass.Rows[i]["SellSignature"].ToString(), ref errMessage, contactTitle, contactFirstName, contactlastName, contactEmail, contactPhone, contactAddress, contactTown, contactCountryAddress, contactStates, contactZipCode, OrganizationName + "/" + OrgID, OrgID);

                                            //remark by romy for optimize
                                            HttpCookie cookie2 = Request.Cookies["cookieSearchcondition"];
                                            if (dataClass.Rows[i]["Quantity"] != null)
                                                log.Info(this, "dataClass.Rows[i][Quantity] " + Convert.ToInt16(dataClass.Rows[i]["Quantity"]));
                                            if (dataClass.Rows[i]["FarePrice"] != null)
                                                log.Info(this, "dataClass.Rows[i][FarePrice] " + Convert.ToDecimal(dataClass.Rows[i]["FarePrice"]));
                                            if (dataClass.Rows[i]["SellSignature"] != null)
                                                log.Info(this, "dataClass.Rows[i][SellSignature] " + dataClass.Rows[i]["SellSignature"].ToString());

                                            if (cookie2 != null)
                                            {
                                                if (Convert.ToInt32(cookie2.Values["InfantNum"]) != 0)
                                                {
                                                    PNR = objBooking.BookingCommitAddInfant(booking, Convert.ToInt32(cookie2.Values["InfantNum"]), agent, Convert.ToInt16(dataClass.Rows[i]["Quantity"]), Convert.ToDecimal(dataClass.Rows[i]["FarePrice"]), dataClass.Rows[i]["SellSignature"].ToString(), ref errMessage, contactTitle, contactFirstName, contactlastName, contactEmail, contactPhone, contactAddress, contactTown, contactCountryAddress, contactStates, contactZipCode, OrganizationName + "/" + OrgID, OrgID);
                                                }
                                                else
                                                {
                                                    PNR = objBooking.BookingCommit(agent, Convert.ToInt16(dataClass.Rows[i]["Quantity"]), Convert.ToDecimal(dataClass.Rows[i]["FarePrice"]), dataClass.Rows[i]["SellSignature"].ToString(), ref errMessage, contactTitle, contactFirstName, contactlastName, contactEmail, contactPhone, contactAddress, contactTown, contactCountryAddress, contactStates, contactZipCode, OrganizationName + "/" + OrgID, OrgID);
                                                }
                                            }
                                            else
                                            {
                                                PNR = objBooking.BookingCommit(agent, Convert.ToInt16(dataClass.Rows[i]["Quantity"]), Convert.ToDecimal(dataClass.Rows[i]["FarePrice"]), dataClass.Rows[i]["SellSignature"].ToString(), ref errMessage, contactTitle, contactFirstName, contactlastName, contactEmail, contactPhone, contactAddress, contactTown, contactCountryAddress, contactStates, contactZipCode, OrganizationName + "/" + OrgID, OrgID);
                                            }

                                            //ongoing commented by diana 20140109
                                            //UpdateSessiondataClassTrans(sellSignature, 0, PNR); //update PNR to session class

                                            log.Info(this, "PNR After BookingCommit: " + PNR);

                                            //PNR = ""; //for testing purpose
                                            if (PNR == "") //added by diana 20140409, if PNR is blank, try check using signature
                                            {
                                                PNR = objBooking.CheckInvalidPNRBySignature(sellSignature);
                                                log.Info(this, "PNR After CheckInvalidPNRBySignature:" + PNR);
                                            }

                                            if (PNR != "") //added by diana 20140127, execute only if PNR is not empty
                                            {
                                                int paySeq = 0;
                                                string actualPaymentStatus = "";// absNavitaire.GetLastPaymentStatusByPNR(PNR, ref errMessage, ref paySeq);
                                                using (profiler.Step("Navitaire:GetLastPaymentStatus"))
                                                {
                                                    actualPaymentStatus = absNavitaire.GetLastPaymentStatusByPNR(PNR, ref errMessage, ref paySeq);
                                                }
                                                if (actualPaymentStatus == "success")
                                                {
                                                    paymentSuccess = true;

                                                    HttpContext.Current.Session.Remove("AgentEmail"); //for sending email purpose
                                                    HttpContext.Current.Session.Add("AgentEmail", txtContactEmail.Text);
                                                    HttpContext.Current.Session.Remove("PaidPNR"); //for sending email purpose
                                                    HttpContext.Current.Session.Add("PaidPNR", PNR);

                                                    log.Info(this, "GetLastPaymentSatusByPNR = success");
                                                    //errMessage = "";
                                                    //string prevPNR = (i + 1).ToString();
                                                    string[] arrstr = absNavitaire.GetPassengerByPNR(PNR, ref errMessage);
                                                    //string[] arrstrInfant = absNavitaire.GetPassengerInfantByPNR(PNR, ref errMessage);
                                                    using (profiler.Step("UpdatePassengerPNR"))
                                                    {
                                                        objBooking.UpdatePassengerPNR(prevPNR, PNR, hID.Value, arrstr);
                                                    }
                                                    //amended by ellis, 20170306, update pnr if infant length more than 0
                                                    //if (arrstrInfant.Length >= 1)
                                                    //{
                                                    //    objBooking.UpdatePassengerInfantPNR(prevPNR, PNR, hID.Value, arrstrInfant, arrstr);
                                                    //}
                                                    //change to new add-On table, Tyas
                                                    //objBooking.UpdateSSRPNR(prevPNR, PNR, hID.Value, arrstr);
                                                    using (profiler.Step("UpdateNewSSRPNR"))
                                                    {
                                                        objBooking.UpdateNewSSRPNR(prevPNR, PNR, hID.Value, arrstr);
                                                    }

                                                    using (profiler.Step("UpdateTransFeesPNR"))
                                                    {
                                                        //20170707 - Sienny
                                                        objBooking.UpdateTransFeesPNR(prevPNR, PNR, hID.Value, arrstr);
                                                    }

                                                    UpdateSessiondataClassTrans(sellSignature, paymentAmount, PNR); // update payment amount to session class



                                                    //added by ketee, update processing fees, 20170122
                                                    switch (PaymentType)
                                                    {
                                                        case "TabCredit":
                                                            //Retrieve booking by PNR
                                                            decimal othFees = 0;
                                                            decimal linetotal = 0;
                                                            List<BookingTransactionDetail> listBookingDetail = objBooking.GetAllBK_TRANSDTLFilterAll(hID.Value, 0);
                                                            List<BookingTransactionDetail> newListbookingDetal = new List<BookingTransactionDetail>();
                                                            objBooking.RetrieveBookingDetailsByPNR(absNavitaire, ref bookingResponse, PNR, hID.Value, 3);

                                                            //bool isOneWay = false;
                                                            //if (listBookingDetail.Count >= 2)
                                                            //{
                                                            //    if (listBookingDetail[0].Origin == listBookingDetail[1].Origin) isOneWay = true;
                                                            //}
                                                            //else
                                                            //{
                                                            //    isOneWay = true;
                                                            //}

                                                            //bool isDepart = listBookingDetail.Count % 2 == 1 ? true : false;
                                                            //bool isReturn = listBookingDetail.Count % 2 == 0 ? true : false;

                                                            if (HttpContext.Current.Session["BookingJourneyContainers"] != null) //grabbing journey details
                                                            {
                                                                List<BookingJourneyContainer> listBookingJourneyContainers = (List<BookingJourneyContainer>)HttpContext.Current.Session["BookingJourneyContainers"];
                                                                foreach (BookingJourneyContainer rowBookingJourney in listBookingJourneyContainers)
                                                                {
                                                                    int iIndexDepart = -1;
                                                                    //if (isOneWay)
                                                                    //    iIndexDepart = listBookingDetail.FindIndex(p => p.RecordLocator.Trim() == prevPNR.Trim());
                                                                    //else if (isDepart)
                                                                    //    iIndexDepart = listBookingDetail.FindIndex(p => p.RecordLocator.Trim() == prevPNR.Trim() && p.Origin.Trim() == rowBookingJourney.DepartureStation.Trim());
                                                                    //else
                                                                    //    iIndexDepart = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 1 && p.RecordLocator.Trim() == prevPNR.Trim());

                                                                    iIndexDepart = listBookingDetail.FindIndex(p => p.Signature.Trim() == sellSignature.Trim() && p.Origin.Trim() == rowBookingJourney.DepartureStation.Trim());
                                                                    //if (iIndexDepart >= 0)
                                                                    //{
                                                                    //    listBookingDetail[iIndexDepart].LineOth = rowBookingJourney.OtherFee;
                                                                    //}
                                                                    if (iIndexDepart >= 0)
                                                                    {
                                                                        BookingTransactionDetail bookingJourney = listBookingDetail[iIndexDepart];
                                                                        objBooking.AssignBookingJourneyList(hID.Value, Convert.ToByte(bookingJourney.SeqNo), rowBookingJourney, prevPNR, "update", ref newListbookingDetal);
                                                                        continue;
                                                                    }
                                                                    //othFees += rowBookingJourney.OtherFee;
                                                                    //linetotal += rowBookingJourney.FarePerPax * (bookDTLInfo.PaxAdult + bookDTLInfo.PaxChild) + bookDTLInfo.LineTax + bookDTLInfo.LineOth + bookDTLInfo.LineDisc + bookDTLInfo.LineFee + bookDTLInfo.LineVAT + bookDTLInfo.LineCharge + bookDTLInfo.LineSSR + bookDTLInfo.LineSeat;
                                                                }

                                                                if (newListbookingDetal != null && newListbookingDetal.Count > 0)
                                                                {
                                                                    objBooking.UpdateTransactionDetails(hID.Value, newListbookingDetal);

                                                                }
                                                                //end recalculate pax details and insert into dtTransDetail

                                                            }
                                                            else
                                                            {
                                                                //return false;
                                                            }


                                                            break;
                                                    }
                                                }
                                                else
                                                {
                                                    log.Info(this, "GetLastPaymentSatusByPNR = " + errMessage);
                                                    if (PNR != "")
                                                    {
                                                        if (paySeq == 0 && errMessage == "declined") //amended by diana 20140109 - add condition for 1st payment if declined, then cancel PNR
                                                        {
                                                            string msg = "";
                                                            string signature = "";// absNavitaire.AgentLogon();
                                                            using (profiler.Step("Navitaire:AgentLogon"))
                                                            {
                                                                signature = absNavitaire.AgentLogon();
                                                            }
                                                            using (profiler.Step("Navitaire:CancelJourney"))
                                                            {
                                                                absNavitaire.CancelJourney(PNR, -paymentAmount, "", signature, ref msg); //cancel journey to api
                                                            }
                                                            //signature = absNavitaire.AgentLogon();
                                                            using (profiler.Step("Navitaire:ClearJourney"))
                                                            {
                                                                absNavitaire.ClearJourney(sellSignature, ref msg);
                                                            }
                                                            //UpdateSessiondataClassTrans(sellSignature, paymentAmount, sellSignature); // update payment amount to session class
                                                        }
                                                        else
                                                        {
                                                            UpdateSessiondataClassTrans(sellSignature, 0, PNR); //update PNR to session class - on going
                                                            string[] arrstr = absNavitaire.GetPassengerByPNR(PNR, ref errMessage);
                                                            string[] arrstrInfant = absNavitaire.GetPassengerInfantByPNR(PNR, ref errMessage);
                                                            objBooking.UpdatePassengerPNR(prevPNR, PNR, hID.Value, arrstr);
                                                            //amended by ellis, 20170306, update pnr if infant length more than 0
                                                            if (arrstrInfant.Length >= 1)
                                                            {
                                                                objBooking.UpdatePassengerInfantPNR(prevPNR, PNR, hID.Value, arrstrInfant, arrstr);
                                                            }
                                                            //change to new add-On table, Tyas
                                                            //objBooking.UpdateSSRPNR(prevPNR, PNR, hID.Value, arrstr);
                                                            objBooking.UpdateNewSSRPNR(prevPNR, PNR, hID.Value, arrstr);

                                                            //20170707 - Sienny
                                                            objBooking.UpdateTransFeesPNR(prevPNR, PNR, hID.Value, arrstr);

                                                        }
                                                    }
                                                    status = errMessage;
                                                    if (status == "declined")
                                                    {
                                                        errMessage = msgList.Err100046;
                                                        status = errMessage;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                errMessage = msgList.Err100028; //if booking commit and PNR is blank
                                                status = errMessage;
                                                break;
                                            }

                                        }
                                        else
                                        {
                                            status = errMessage;
                                            break;
                                        }
                                    }
                                }


                                //skip the rest if found
                                break;
                            }
                        }
                        //added by diana 20131210 - try catch to check for valid booking
                        catch (TimeoutException ex) { log.Error(this, ex); objBooking.CheckJourneyExist(ex.Message.ToString(), sellSignature); }
                        catch (OutOfMemoryException ex) { log.Error(this, ex); objBooking.CheckJourneyExist(ex.Message.ToString(), sellSignature); }
                        catch (IndexOutOfRangeException ex) { log.Error(this, ex); objBooking.CheckJourneyExist(ex.Message.ToString(), sellSignature); }
                        catch (ThreadInterruptedException ex) { log.Error(this, ex); objBooking.CheckJourneyExist(ex.Message.ToString(), sellSignature); }
                        catch (NullReferenceException ex) { log.Error(this, ex); objBooking.CheckJourneyExist(ex.Message.ToString(), sellSignature); }
                        catch (StackOverflowException ex) { log.Error(this, ex); objBooking.CheckJourneyExist(ex.Message.ToString(), sellSignature); }
                        catch (ApplicationException ex) { log.Error(this, ex); objBooking.CheckJourneyExist(ex.Message.ToString(), sellSignature); }
                        catch (Exception ex) { log.Error(this, ex); objBooking.CheckJourneyExist(ex.Message.ToString(), sellSignature); }


                    }
                }
                else if (HttpContext.Current.Session["ChgTransMain"] != null && HttpContext.Current.Session["ChgTransDetail"] != null)
                {
                    log.Info(this, "Entering(2)");
                    if (HttpContext.Current.Session["ChgTransMain"] != null && HttpContext.Current.Session["ChgTransDetail"] != null)
                    {
                        DataTable dtTransMain = objBooking.dtTransMain();
                        if (HttpContext.Current.Session["ChgTransMain"] != null)
                            dtTransMain = (DataTable)HttpContext.Current.Session["ChgTransMain"];
                        DataTable dtTransDetail = objBooking.dtTransDetail();
                        if (HttpContext.Current.Session["ChgTransDetail"] != null)
                            dtTransDetail = (DataTable)HttpContext.Current.Session["ChgTransDetail"];

                        int iCount = 0;
                        log.Info(this, "Current SessionID = " + SessionID);
                        for (int i = 0; i < dtTransDetail.Rows.Count; i++)
                        {
                            PNR = dtTransDetail.Rows[i]["RecordLocator"].ToString().Trim();
                            log.Info(this, "Compare PNR(" + PNR + ") with SessionID(" + SessionID + ")");
                            if (PNR == SessionID)
                            {
                                iCount += 1;

                                string signatureDetail = "";
                                if (HttpContext.Current.Session["objListBK_TRANSDTL_Infos"] != null)
                                {
                                    string SellSessionID = "";
                                    List<BookingTransactionDetail> objListBK_TRANSDTL_Infos = (List<BookingTransactionDetail>)HttpContext.Current.Session["objListBK_TRANSDTL_Infos"];
                                    foreach (BookingTransactionDetail bkDetail in objListBK_TRANSDTL_Infos)
                                    {
                                        if (bkDetail.RecordLocator == PNR)
                                        {
                                            signatureDetail = bkDetail.Signature;
                                            SellSessionID = bkDetail.Signature;
                                            using (profiler.Step("Navitaire:GetBookingFromState"))
                                            {
                                                booking = absNavitaire.GetBookingFromState(SellSessionID, 2);
                                            }
                                            break;
                                        }
                                    }
                                }


                                //string signatureDetail = absNavitaire.GetBookingByPNR(dtTransDetail.Rows[i]["RecordLocator"].ToString(), ref errMessage, ref bookingResponse);
                                decimal FullPrice = Convert.ToDecimal(dtTransDetail.Rows[i]["LineTotal"].ToString());
                                decimal CollectedAmount = Convert.ToDecimal(dtTransDetail.Rows[i]["DetailCollectedAmount"].ToString());

                                if (absNavitaire.RemovePaymentfromBooking(booking, signatureDetail))
                                {
                                    //if (bookingResponse != null && bookingResponse.Booking.BookingSum.BalanceDue < paymentAmount)
                                    if (booking != null && booking.BookingSum.BalanceDue < paymentAmount)
                                    {
                                        status = msgList.Err100073;
                                        errMessage = msgList.Err100073A;
                                    }
                                    else
                                    {
                                        string paycomment = "";
                                        if (Session["ChgMode"] != null)
                                        {
                                            paycomment = "#0" + Session["ChgMode"].ToString();
                                        }

                                        switch (PaymentType)
                                        {
                                            case "TabCredit":
                                                AddPaymentStatus = absNavitaire.AddPaymentCreditCard(cmbCardType.SelectedItem.Value.ToString(), txtCardNumber.Text, cmbExpiryYear.SelectedItem.Value.ToString() + "-" + cmbExpiryMonth.SelectedItem.Value.ToString() + "-" + "01", lblCurrentTotalCurrency.Text, paymentAmount, txtCVV2.Text, txtCardHolderName.Text, txtCardIssuer.Text, cmbIssuingCountry.SelectedItem.Value.ToString(), signatureDetail,
                                                    txtAddress.Text.ToString(), txtTown.Text.ToString(), cmbCountryAddress.Value.ToString(), cmbState.Value.ToString(), txtZipCode.Text.ToString(), paycomment, ref errMessage);
                                                break;
                                            case "TabAG":
                                                //if (paymentAmount >= FullPrice - CollectedAmount)
                                                //{
                                                using (profiler.Step("Navitaire:AddPaymentToBooking"))
                                                {
                                                    AddPaymentStatus = absNavitaire.AddAgencyPayment(paymentAmount, lblCurrentTotalCurrency.Text, accNumber, pass, signatureDetail, accID, paycomment, ref errMessage);
                                                }
                                                //}
                                                //else
                                                //{
                                                //    errMessage = msgList.Err100039;
                                                //}
                                                break;
                                        }

                                        if (AddPaymentStatus)
                                        {
                                            Boolean IsCommitted = false;
                                            if (Session["ChgMode"] != null && Session["ChgMode"].ToString() == "1")
                                            {
                                                bool IsInternational = (bool)HttpContext.Current.Session["IsInternationalFlight"];
                                                List<PassengerData> PaxList = (List<PassengerData>)HttpContext.Current.Session["lstPassengerData"];
                                                List<PassengerData> InfantList = (List<PassengerData>)HttpContext.Current.Session["lstPassInfantData"];
                                                IsCommitted = objBooking.CommitUpdatePax(agent, PNR, PaxList, InfantList, IsInternational, ref errMessage, cmbContactTitle.Value.ToString(), txtContactFirstName.Text.ToString(), txtContactLastName.Text.ToString(), txtContactEmail.Text.ToString(), txtContactPhone.Text.ToString(), txtContactAddress.Text.ToString(), txtContactTown.Text.ToString(), cmbContactCountryAddress.Value.ToString(), cmbContactState.Value.ToString(), txtContactZipCode.Text.ToString());
                                            }
                                            else if (Session["ChgMode"] != null && Session["ChgMode"].ToString() == "2" || Session["ChgMode"].ToString() == "3" || Session["ChgMode"].ToString() == "4" || Session["ChgMode"].ToString() == "5")//edited by romy for insure
                                            {

                                                ABS.Navitaire.APIBooking APIBooking = new ABS.Navitaire.APIBooking("");
                                                string msg = "";

                                                if (Session["ChgMode"].ToString() == "2" || Session["ChgMode"].ToString() == "5")//edited by romy for insure
                                                {
                                                    IsCommitted = objBooking.BookingCommitChangeInfant(PNR, signatureDetail, ref msg, lblCurrentTotalCurrency.Text, true, true);
                                                }
                                                else
                                                {
                                                    using (profiler.Step("Navitaire:BookingCommit"))
                                                    {
                                                        IsCommitted = APIBooking.BookingCommitChange(PNR, signatureDetail, ref msg, lblCurrentTotalCurrency.Text, true, true);
                                                    }
                                                }

                                            }

                                            if (IsCommitted == true)
                                            //if (absNavitaire.BookingCommit(PNR, signatureDetail, ref errMessage, "", false, false, agent.Username, agent.AgentID, cmbContactTitle.Value.ToString(), txtContactFirstName.Text.ToString(), txtContactLastName.Text.ToString(), txtContactEmail.Text.ToString(), txtContactPhone.Text.ToString(), txtContactAddress.Text.ToString(), txtContactTown.Text.ToString(), cmbContactCountryAddress.Value.ToString(), cmbContactState.Value.ToString(), txtContactZipCode.Text.ToString()))
                                            {
                                                log.Info(this, "PNR - BookingCommit(2):" + PNR);
                                                int paySeq = 0;
                                                string actualPaymentStatus = "";// absNavitaire.GetLastPaymentStatusByPNR(PNR, ref errMessage, ref paySeq);
                                                using (profiler.Step("Navitaire:GetLastPaymentStatus"))
                                                {
                                                    actualPaymentStatus = absNavitaire.GetLastPaymentStatusByPNR(PNR, ref errMessage, ref paySeq);
                                                }
                                                if (actualPaymentStatus == "success")
                                                {
                                                    paymentSuccess = true;
                                                    if (HttpContext.Current.Session["dataClassTrans"] != null)
                                                    {
                                                        UpdateSessiondataClassTrans("", paymentAmount, PNR);
                                                    }


                                                    //remarked by diana 20170412, ssr amount + details are update at the bottom of function
                                                    //if (Session["ChgMode"] != null && Session["ChgMode"].ToString() == "2")
                                                    //{
                                                    //    if (Session["ChgTransSSR"] != null)
                                                    //    {
                                                    //        List<ABS.Logic.GroupBooking.Booking.Bk_transssr> listAll = new List<Bk_transssr>();
                                                    //        listAll = (List<Bk_transssr>)Session["ChgTransSSR"];
                                                    //        objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                                                    //        objBooking.SaveSSRManageCommit(listAll, CoreBase.EnumSaveType.Update, PNR, "", true);

                                                    //    }
                                                    //    if (Session["listBookingDetail"] != null && Session["bookingMain"] != null)
                                                    //    {
                                                    //        BookingTransactionMain bookingMain = new BookingTransactionMain();
                                                    //        bookingMain = (BookingTransactionMain)Session["bookingMain"];
                                                    //        List<BookingTransactionDetail> listBookingDetail = new List<BookingTransactionDetail>();
                                                    //        listBookingDetail = (List<BookingTransactionDetail>)Session["listBookingDetail"];
                                                    //        objBooking.UpdateTotalSSR(TransID, bookingMain, listBookingDetail);
                                                    //    }
                                                    //}
                                                    //else 
                                                    if (Session["ChgMode"] != null && Session["ChgMode"].ToString() == "3")
                                                    {
                                                        if (Session["ChgHistory"] != null)
                                                        {
                                                            DataTable dtTransDetails = objBooking.dtTransDetail();
                                                            dtTransDetails = new DataTable();
                                                            dtTransDetails = (DataTable)Session["ChgHistory"];
                                                            objBooking.SaveHistoryBooking(dtTransDetails.Rows[0]["TransID"].ToString(), dtTransDetails.Rows[0]["RecordLocator"].ToString(), ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
                                                        }
                                                        if (Session["Chgsave"] != null && Session["ChgbookHDRInfo"] != null && Session["ChglstbookDTLInfo"] != null)
                                                        {
                                                            BookingTransactionMain BookingMain = new BookingTransactionMain();
                                                            lstbookDTLInfos = (List<BookingTransactionDetail>)Session["ChglstbookDTLInfo"];
                                                            bookHDRInfos = (BookingTransactionMain)Session["ChgbookHDRInfo"];
                                                            ArrayList save = (ArrayList)Session["Chgsave"];
                                                            List<BookingTransactionDetail> listBookingDetail = new List<BookingTransactionDetail>();
                                                            List<BookingTransactionDetail> listBookingDetailCombine = new List<BookingTransactionDetail>();
                                                            BookingTransactionMain bookingMain = new BookingTransactionMain();
                                                            PaymentControl objPayment = new PaymentControl();
                                                            PaymentInfo paymentInfo = new PaymentInfo();
                                                            Session["listTransFees"] = null;
                                                            if ((Boolean)save[2] == true)
                                                            {

                                                                BookingMain = objBooking.SaveBooking(bookHDRInfos, lstbookDTLInfos, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
                                                                if (BookingMain != null && BookingMain.TransID != "")
                                                                {
                                                                    //objBooking.UpdatePassengerTransID(save[1].ToString(), lstbookDTLInfos[0].RecordLocator, save[0].ToString());
                                                                    objBooking.UpdateTransTenderTransID(save[1].ToString(), lstbookDTLInfos[0].RecordLocator, save[0].ToString());
                                                                    objBooking.DeletePrevTransaction(save[1].ToString(), lstbookDTLInfos[0].RecordLocator);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                BookingMain = objBooking.SaveBooking(bookHDRInfos, lstbookDTLInfos, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Update);
                                                            }
                                                            objBooking.SaveHeaderDetail(bookHDRInfos, lstbookDTLInfos, CoreBase.EnumSaveType.Update);
                                                        }
                                                        if (Session["ChgTransSSR"] != null)
                                                        {
                                                            //change to new add-On table, Tyas
                                                            int cnt = 0;
                                                            //List<ABS.Logic.GroupBooking.Booking.bk_transssr> listAll = new List<bk_transssr>();
                                                            //listAll = (List<bk_transssr>)Session["ChgTransSSR"];

                                                            //foreach (Bk_transssr b in listAll)
                                                            //{
                                                            //    cnt++;
                                                            //    objBooking.SaveSSR(b, CoreBase.EnumSaveType.Update, "", true, cnt == listAll.Count ? true : false);
                                                            //}

                                                            List<ABS.Logic.GroupBooking.Booking.Bk_transaddon> listAll = new List<Bk_transaddon>();
                                                            listAll = (List<Bk_transaddon>)Session["ChgTransSSR"];
                                                            foreach (Bk_transaddon b in listAll)
                                                            {
                                                                cnt++;
                                                                if (objBooking.Update(b, "", true, cnt == listAll.Count ? true : false) == false)
                                                                {
                                                                    log.Error(this, "update BK_TRANSADDON from payment page failed = " + TransID);
                                                                    return msgList.Err999999;
                                                                }
                                                            }
                                                        }

                                                        //if (Session["listBookingDetail"] != null && Session["bookingMain"] != null)
                                                        //{
                                                        //    BookingTransactionMain bookingMain = new BookingTransactionMain();
                                                        //    bookingMain = (BookingTransactionMain)Session["bookingMain"];
                                                        //    List<BookingTransactionDetail> listBookingDetail = new List<BookingTransactionDetail>();
                                                        //    listBookingDetail = (List<BookingTransactionDetail>)Session["listBookingDetail"];
                                                        //    objBooking.UpdateTotalSSR(TransID, bookingMain, listBookingDetail);
                                                        //}
                                                    }
                                                    else if (Session["ChgMode"] != null && (Session["ChgMode"].ToString() == "2" || Session["ChgMode"].ToString() == "5"))//edited by romy for insure
                                                    {
                                                        if (Session["lstPassInfantData"] != null)
                                                        {
                                                            List<PassengerData> lstPassInfantData = new List<PassengerData>();
                                                            lstPassInfantData = (List<PassengerData>)Session["lstPassInfantData"];
                                                            if (lstPassInfantData.Count > 0)
                                                            {
                                                                objBooking.SaveBK_PASSENGERLIST(lstPassInfantData, CoreBase.EnumSaveType.Insert);
                                                            }
                                                        }

                                                    }


                                                    //added by ketee, update processing fees, 20170122
                                                    switch (PaymentType)
                                                    {
                                                        case "TabCredit":
                                                            //Retrieve booking by PNR
                                                            decimal othFees = 0;
                                                            decimal linetotal = 0;
                                                            List<BookingTransactionDetail> listBookingDetail = objBooking.GetAllBK_TRANSDTLFilterAll(hID.Value, 0);
                                                            List<BookingTransactionDetail> newListbookingDetal = new List<BookingTransactionDetail>();
                                                            objBooking.RetrieveBookingDetailsByPNR(absNavitaire, ref bookingResponse, PNR, hID.Value, 3);

                                                            bool isDepart = listBookingDetail.Count % 2 == 1 ? true : false;
                                                            bool isReturn = listBookingDetail.Count % 2 == 0 ? true : false;

                                                            if (HttpContext.Current.Session["BookingJourneyContainers"] != null) //grabbing journey details
                                                            {
                                                                List<BookingJourneyContainer> listBookingJourneyContainers = (List<BookingJourneyContainer>)HttpContext.Current.Session["BookingJourneyContainers"];
                                                                foreach (BookingJourneyContainer rowBookingJourney in listBookingJourneyContainers)
                                                                {
                                                                    int iIndexDepart = -1;
                                                                    if (isDepart)
                                                                        iIndexDepart = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 1 && p.RecordLocator.Trim() == PNR.Trim());
                                                                    else
                                                                        iIndexDepart = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 0 && p.RecordLocator.Trim() == PNR.Trim());
                                                                    //if (iIndexDepart >= 0)
                                                                    //{
                                                                    //    listBookingDetail[iIndexDepart].LineOth = rowBookingJourney.OtherFee;
                                                                    //}
                                                                    if (iIndexDepart >= 0)
                                                                    {
                                                                        BookingTransactionDetail bookingJourney = listBookingDetail[iIndexDepart];
                                                                        //if (Session["ChgMode"] != null && Session["ChgMode"].ToString() == "2")
                                                                        //{

                                                                        //}
                                                                        objBooking.AssignBookingJourneyList(hID.Value, Convert.ToByte(bookingJourney.SeqNo), rowBookingJourney, PNR, "update", ref newListbookingDetal);
                                                                        continue;
                                                                    }
                                                                    //othFees += rowBookingJourney.OtherFee;
                                                                    //linetotal += rowBookingJourney.FarePerPax * (bookDTLInfo.PaxAdult + bookDTLInfo.PaxChild) + bookDTLInfo.LineTax + bookDTLInfo.LineOth + bookDTLInfo.LineDisc + bookDTLInfo.LineFee + bookDTLInfo.LineVAT + bookDTLInfo.LineCharge + bookDTLInfo.LineSSR + bookDTLInfo.LineSeat;
                                                                }

                                                                if (newListbookingDetal != null && newListbookingDetal.Count > 0)
                                                                {
                                                                    objBooking.UpdateTransactionDetails(hID.Value, newListbookingDetal);

                                                                }
                                                                //end recalculate pax details and insert into dtTransDetail

                                                            }
                                                            else
                                                            {
                                                                //return false;
                                                            }


                                                            break;
                                                    }
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
                                    }

                                    break;
                                }
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

                    //HttpContext.Current.Session.Remove("ChgTransMain");
                    //HttpContext.Current.Session.Remove("ChgTransDetail");
                    //LoadData(hID.Value);
                }
                else
                {
                    log.Info(this, "Entering(2)");
                    if (HttpContext.Current.Session["TransMain"] != null && HttpContext.Current.Session["TransDetail"] != null)
                    {
                        DataTable dtTransMain = objBooking.dtTransMain();
                        if (HttpContext.Current.Session["TransMain"] != null)
                            dtTransMain = (DataTable)HttpContext.Current.Session["TransMain"];
                        DataTable dtTransDetail = objBooking.dtTransDetail();
                        if (HttpContext.Current.Session["TransDetail"] != null)
                            dtTransDetail = (DataTable)HttpContext.Current.Session["TransDetail"];

                        int iCount = 0;
                        log.Info(this, "Current SessionID = " + SessionID);
                        for (int i = 0; i < dtTransDetail.Rows.Count; i++)
                        {
                            PNR = dtTransDetail.Rows[i]["RecordLocator"].ToString().Trim();
                            log.Info(this, "Compare PNR(" + PNR + ") with SessionID(" + SessionID + ")");
                            if (PNR == SessionID)
                            {
                                iCount += 1;
                                string signatureDetail = "";// absNavitaire.GetBookingByPNR(dtTransDetail.Rows[i]["RecordLocator"].ToString(), ref errMessage, ref bookingResponse);
                                using (profiler.Step("Navitaire:GetBooking"))
                                {
                                    signatureDetail = absNavitaire.GetBookingByPNR(dtTransDetail.Rows[i]["RecordLocator"].ToString(), ref errMessage, ref bookingResponse);
                                }
                                decimal FullPrice = Convert.ToDecimal(dtTransDetail.Rows[i]["LineTotal"].ToString());
                                decimal CollectedAmount = Convert.ToDecimal(dtTransDetail.Rows[i]["DetailCollectedAmount"].ToString());

                                if (absNavitaire.RemovePaymentfromBooking(bookingResponse.Booking, signatureDetail))
                                {
                                    if (bookingResponse != null && bookingResponse.Booking.BookingSum.BalanceDue < paymentAmount)
                                    {
                                        status = msgList.Err100073;
                                        errMessage = msgList.Err100073A;
                                    }
                                    else
                                    {
                                        switch (PaymentType)
                                        {
                                            case "TabCredit":
                                                AddPaymentStatus = absNavitaire.AddPaymentCreditCard(cmbCardType.SelectedItem.Value.ToString(), txtCardNumber.Text, cmbExpiryYear.SelectedItem.Value.ToString() + "-" + cmbExpiryMonth.SelectedItem.Value.ToString() + "-" + "01", lblCurrentTotalCurrency.Text, paymentAmount, txtCVV2.Text, txtCardHolderName.Text, txtCardIssuer.Text, cmbIssuingCountry.SelectedItem.Value.ToString(), signatureDetail,
                                                    txtAddress.Text.ToString(), txtTown.Text.ToString(), cmbCountryAddress.Value.ToString(), cmbState.Value.ToString(), txtZipCode.Text.ToString(), "", ref errMessage);
                                                break;
                                            case "TabAG":
                                                //if (paymentAmount >= FullPrice - CollectedAmount)
                                                //{
                                                using (profiler.Step("Navitaire:AddPaymentToBooking"))
                                                {
                                                    AddPaymentStatus = absNavitaire.AddAgencyPayment(paymentAmount, lblCurrentTotalCurrency.Text, accNumber, pass, signatureDetail, accID, "", ref errMessage);
                                                }
                                                //}
                                                //else
                                                //{
                                                //    errMessage = msgList.Err100039;
                                                //}
                                                break;
                                        }

                                        if (AddPaymentStatus)
                                        {
                                            if (absNavitaire.BookingCommit(PNR, signatureDetail, ref errMessage, "", false, false, agent.Username, agent.AgentID, cmbContactTitle.Value.ToString(), txtContactFirstName.Text.ToString(), txtContactLastName.Text.ToString(), txtContactEmail.Text.ToString(), txtContactPhone.Text.ToString(), txtContactAddress.Text.ToString(), txtContactTown.Text.ToString(), cmbContactCountryAddress.Value.ToString(), cmbContactState.Value.ToString(), txtContactZipCode.Text.ToString(), MyUserSet.OrganizationName.ToString() + "/" + agent.OrgID.ToString(), agent.OrgID.ToString()))
                                            {
                                                log.Info(this, "PNR - BookingCommit(2):" + PNR);
                                                int paySeq = 0;
                                                string actualPaymentStatus = "";// absNavitaire.GetLastPaymentStatusByPNR(PNR, ref errMessage, ref paySeq);
                                                using (profiler.Step("Navitaire:GetLastPaymentStatus"))
                                                {
                                                    actualPaymentStatus = absNavitaire.GetLastPaymentStatusByPNR(PNR, ref errMessage, ref paySeq);
                                                }
                                                if (actualPaymentStatus == "success")
                                                {
                                                    paymentSuccess = true;
                                                    if (HttpContext.Current.Session["dataClassTrans"] != null)
                                                    {
                                                        UpdateSessiondataClassTrans("", paymentAmount, PNR);
                                                    }

                                                    //added by ketee, update processing fees, 20170122

                                                    switch (PaymentType)
                                                    {
                                                        case "TabCredit":
                                                            //Retrieve booking by PNR
                                                            decimal othFees = 0;
                                                            decimal linetotal = 0;
                                                            List<BookingTransactionDetail> listBookingDetail = objBooking.GetAllBK_TRANSDTLFilterAll(hID.Value, 0);
                                                            List<BookingTransactionDetail> newListbookingDetal = new List<BookingTransactionDetail>();
                                                            objBooking.RetrieveBookingDetailsByPNR(absNavitaire, ref bookingResponse, PNR, hID.Value, 3);

                                                            bool isDepart = listBookingDetail.Count % 2 == 1 ? true : false;
                                                            bool isReturn = listBookingDetail.Count % 2 == 0 ? true : false;

                                                            if (HttpContext.Current.Session["BookingJourneyContainers"] != null) //grabbing journey details
                                                            {
                                                                List<BookingJourneyContainer> listBookingJourneyContainers = (List<BookingJourneyContainer>)HttpContext.Current.Session["BookingJourneyContainers"];
                                                                foreach (BookingJourneyContainer rowBookingJourney in listBookingJourneyContainers)
                                                                {
                                                                    int iIndexDepart = -1;
                                                                    if (isDepart)
                                                                        iIndexDepart = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 1 && p.RecordLocator.Trim() == PNR.Trim());
                                                                    else
                                                                        iIndexDepart = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 0 && p.RecordLocator.Trim() == PNR.Trim());
                                                                    //if (iIndexDepart >= 0)
                                                                    //{
                                                                    //    listBookingDetail[iIndexDepart].LineOth = rowBookingJourney.OtherFee;
                                                                    //}
                                                                    if (iIndexDepart >= 0)
                                                                    {
                                                                        BookingTransactionDetail bookingJourney = listBookingDetail[iIndexDepart];
                                                                        objBooking.AssignBookingJourneyList(hID.Value, Convert.ToByte(bookingJourney.SeqNo), rowBookingJourney, PNR, "update", ref newListbookingDetal);
                                                                        continue;
                                                                    }
                                                                    //othFees += rowBookingJourney.OtherFee;
                                                                    //linetotal += rowBookingJourney.FarePerPax * (bookDTLInfo.PaxAdult + bookDTLInfo.PaxChild) + bookDTLInfo.LineTax + bookDTLInfo.LineOth + bookDTLInfo.LineDisc + bookDTLInfo.LineFee + bookDTLInfo.LineVAT + bookDTLInfo.LineCharge + bookDTLInfo.LineSSR + bookDTLInfo.LineSeat;
                                                                }

                                                                if (newListbookingDetal != null && newListbookingDetal.Count > 0)
                                                                {
                                                                    objBooking.UpdateTransactionDetails(hID.Value, newListbookingDetal);

                                                                }
                                                                //end recalculate pax details and insert into dtTransDetail

                                                            }
                                                            else
                                                            {
                                                                //return false;
                                                            }


                                                            break;
                                                    }
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
                                    }

                                    break;
                                }
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
                    //update transdetails

                    DataTable dtKeyField = new DataTable();
                    if (HttpContext.Current.Session["dataClassTrans"] != null && PNRExist == false)
                    {
                        dtKeyField = GetKeyFieldBySignature(sellSignature);
                    }

                    if (dtKeyField == null || dtKeyField.Rows.Count <= 0)
                    {
                        dtKeyField = GetKeyFieldByPNR(PNR);
                    }

                    //dtKeyField = null;//for testing purpose, to have an error in the middle of process updating details to db

                    if (dtKeyField.Rows.Count > 0)
                    {
                        decimal leftPayDetail = 0; //check total amount each sequence

                        //to log columns
                        string colName = "";
                        foreach (DataColumn col in dtKeyField.Columns)
                        {
                            colName += col.ColumnName + ";";
                        }
                        log.Info(this, colName);
                        //to log columns

                        ArrayList strPNR = new ArrayList();
                        DataTable dtDetail = objBooking.dtTransDetail();
                        if (HttpContext.Current.Session["ChgTransDetail"] != null) dtDetail = (DataTable)HttpContext.Current.Session["ChgTransDetail"];

                        if (dtKeyField.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                if (dr["RecordLocator"].ToString() == PNR)
                                {
                                    dr["DetailCollectedAmount"] = paymentAmount;
                                    break;
                                }
                            }
                        }
                        for (int ctrDetail = 0; ctrDetail < dtKeyField.Rows.Count; ctrDetail++)
                        {
                            string RecordLocator = dtKeyField.Rows[ctrDetail]["RecordLocator"].ToString();
                            TransID = dtKeyField.Rows[ctrDetail]["TransID"].ToString();
                            byte SeqNo = Convert.ToByte(dtKeyField.Rows[ctrDetail]["SeqNo"]);

                            int cnt = 0;
                            bookDTLInfo = objBooking.GetSingleBK_TRANSDTL(RecordLocator, TransID, SeqNo);
                            if (HttpContext.Current.Session["ChgTransDetail"] != null)
                            {
                                //if (!strPNR.Contains(RecordLocator))
                                //{
                                cnt = 0;
                                foreach (DataRow dr in dtDetail.Rows)
                                {
                                    if (dr["RecordLocator"].ToString() == RecordLocator)
                                    {
                                        //strPNR.Add(RecordLocator);
                                        bookDTLInfo.LineTotal = Convert.ToDecimal(dr["LineTotal"]);
                                        bookDTLInfo.SellKey = dr["SellKey"].ToString();
                                        bookDTLInfo.LineTax = Convert.ToDecimal(dr["LineTax"]);
                                        bookDTLInfo.LinePaxFee = Convert.ToDecimal(dr["LinePaxFee"]);
                                        bookDTLInfo.LineOth = Convert.ToDecimal(dr["LineOth"]);
                                        bookDTLInfo.LineProcess = Convert.ToDecimal(dr["LineProcess"]);
                                        bookDTLInfo.LineSSR = Convert.ToDecimal(dr["LineSSR"]);
                                        bookDTLInfo.LineInfant = Convert.ToDecimal(dr["LineInfant"]);
                                        bookDTLInfo.LineSeat = Convert.ToDecimal(dr["LineSeat"]);
                                        bookDTLInfo.LineNameChange = Convert.ToDecimal(dr["LineNameChange"]);
                                        bookDTLInfo.LineDisc = Convert.ToDecimal(dr["LineDisc"]);
                                        bookDTLInfo.LinePromoDisc = Convert.ToDecimal(dr["LinePromoDisc"]);
                                        bookDTLInfo.LineNameChange = Convert.ToDecimal(dr["LineNameChange"]);


                                        dtDetail.Rows[cnt]["DetailCollectedAmount"] = Convert.ToDecimal(dtDetail.Rows[cnt]["DetailCollectedAmount"].ToString()) + bookDTLInfo.CollectedAmount;
                                        break;
                                    }
                                    cnt += 1;
                                }

                                //}
                            }

                            if (paymentAmount > bookDTLInfo.LineTotal)
                            {
                                leftPayDetail = paymentAmount - bookDTLInfo.LineTotal;
                                paymentAmount = bookDTLInfo.LineTotal;
                                using (profiler.Step("AssignTransDetail"))
                                {
                                    AssignTransDetail(dtKeyField.Rows[ctrDetail]["RecordLocator"].ToString(), dtKeyField.Rows[ctrDetail]["TransID"].ToString(), Convert.ToByte(dtKeyField.Rows[ctrDetail]["SeqNo"]), MyUserSet.AgentName, paymentAmount + bookDTLInfo.CollectedAmount, PNR);
                                }

                                totalPaid += paymentAmount;
                                paymentAmount = leftPayDetail;
                            }
                            else
                            {
                                using (profiler.Step("AssignTransDetail"))
                                {
                                    AssignTransDetail(dtKeyField.Rows[ctrDetail]["RecordLocator"].ToString(), dtKeyField.Rows[ctrDetail]["TransID"].ToString(), Convert.ToByte(dtKeyField.Rows[ctrDetail]["SeqNo"]), MyUserSet.AgentName, paymentAmount + bookDTLInfo.CollectedAmount, PNR);
                                }
                                totalPaid += paymentAmount;
                                paymentAmount = 0;
                            }


                        }
                        if (HttpContext.Current.Session["ChgTransDetail"] != null)
                        {
                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                dr["AmountDue"] = decimal.Parse(dr["LineTotal"].ToString()) - decimal.Parse(dr["DetailCollectedAmount"].ToString());
                            }

                            HttpContext.Current.Session.Remove("ChgTransDetail");
                            HttpContext.Current.Session.Add("ChgTransDetail", dtDetail);
                        }
                    }

                    //update transmain
                    DataTable dtKeyFieldMain = GetKeyFieldMainByTransID(TransID);
                    DataTable dtTransMain = objBooking.dtTransMain();

                    if (HttpContext.Current.Session["ChgTransMain"] != null)
                        dtTransMain = (DataTable)HttpContext.Current.Session["ChgTransMain"];
                    else if (HttpContext.Current.Session["TransMain"] != null)
                        dtTransMain = (DataTable)HttpContext.Current.Session["TransMain"];

                    TransTotal = Convert.ToDecimal(dtTransMain.Rows[0]["TotalTrans"]);
                    decimal totalTrans = totalPaid + Convert.ToDecimal(dtTransMain.Rows[0]["CollectedAmt"]);
                    byte TransStatus = 1;
                    int PreviousStatus = Convert.ToInt16(dtTransMain.Rows[0]["TransStatus"]);
                    if (totalTrans == TransTotal)
                    {
                        TransStatus = 2;
                    }

                    //update transmain added by ketee, 
                    DataTable dtTransDetail = objBooking.dtTransDetail();
                    int totalPax = 0;
                    decimal totalTransAmountAll = 0;
                    decimal totalTransAmount = 0;
                    decimal totalAmountGoing = 0;
                    decimal totalAmountReturn = 0;
                    decimal totalTransSubTotal = 0;
                    decimal totalTransPaxFee = 0;
                    decimal totalTransTotalFee = 0;
                    decimal totalTransTotalTax = 0;
                    decimal totalTransTotalOth = 0;
                    decimal totalTransTotalSSR = 0;
                    decimal totalTransTotalSeat = 0;
                    decimal totalTransTotalInfant = 0;
                    decimal totalTransTotalDisc = 0;
                    decimal totalTransTotalPromoDisc = 0;
                    decimal totalTransNew = 0;
                    decimal AverageFare = 0;


                    if (HttpContext.Current.Session["ChgTransMain"] == null)
                        LoadData(TransID);

                    if (HttpContext.Current.Session["ChgTransDetail"] != null)
                    {
                        dtTransDetail = (DataTable)HttpContext.Current.Session["ChgTransDetail"];
                    }
                    else if (HttpContext.Current.Session["TransDetail"] != null)
                    {
                        dtTransDetail = (DataTable)HttpContext.Current.Session["TransDetail"];
                    }
                    

                    if (dtTransDetail != null && dtTransDetail.Rows.Count > 0)
                    {
                        //string origin = "";
                        //int goingreturn;
                        for (int k = 0; k < dtTransDetail.Rows.Count; k++)
                        {
                            bool NewVal = true;
                            if (Session["ChgMode"] != null && (Session["ChgMode"].ToString() == "1" || Session["ChgMode"].ToString() == "2" || Session["ChgMode"].ToString() == "5"))//edited by romy for insure
                            {
                                if (dtTransDetail.Rows[k]["RecordLocator"].ToString() != PNR)
                                    NewVal = false;
                            }
                            totalTransNew += Convert.ToDecimal(dtTransDetail.Rows[k]["LineTotal"].ToString());

                            if (NewVal)
                            {
                                totalPax += Convert.ToInt16(dtTransDetail.Rows[k]["PaxAdult"].ToString()) + Convert.ToInt16(dtTransDetail.Rows[k]["PaxChild"].ToString());
                                totalTransAmount += Convert.ToDecimal(dtTransDetail.Rows[k]["LineTotal"].ToString());
                                totalTransPaxFee += Convert.ToDecimal(dtTransDetail.Rows[k]["LinePaxFee"].ToString());
                                totalTransTotalFee += Convert.ToDecimal(dtTransDetail.Rows[k]["LineFee"].ToString());
                                totalTransTotalOth += Convert.ToDecimal(dtTransDetail.Rows[k]["LineOth"].ToString());
                                totalTransTotalSSR += Convert.ToDecimal(dtTransDetail.Rows[k]["LineSSR"].ToString());
                                totalTransTotalSeat += Convert.ToDecimal(dtTransDetail.Rows[k]["LineSeat"].ToString());
                                totalTransTotalInfant += Convert.ToDecimal(dtTransDetail.Rows[k]["LineInfant"].ToString());
                                totalTransTotalDisc += Convert.ToDecimal(dtTransDetail.Rows[k]["LineDisc"].ToString());
                                totalTransTotalPromoDisc += Convert.ToDecimal(dtTransDetail.Rows[k]["LinePromoDisc"].ToString());
                                totalTransTotalTax += Convert.ToDecimal(dtTransDetail.Rows[k]["LineTax"].ToString());
                                totalTransSubTotal += Convert.ToDecimal(dtTransDetail.Rows[k]["LineTotal"].ToString());
                                totalTransAmountAll += Convert.ToDecimal(dtTransDetail.Rows[k]["LineTotal"].ToString());
                            }
                            else
                            {
                                totalPax += Convert.ToInt16(dtTransDetail.Rows[k]["PaxAdult"].ToString()) + Convert.ToInt16(dtTransDetail.Rows[k]["PaxChild"].ToString());
                                totalTransAmount += Convert.ToDecimal(dtTransDetail.Rows[k]["OldLineTotal"].ToString());
                                totalTransPaxFee += Convert.ToDecimal(dtTransDetail.Rows[k]["OldLinePaxFee"].ToString());
                                totalTransTotalFee += Convert.ToDecimal(dtTransDetail.Rows[k]["OldLineFee"].ToString());
                                totalTransTotalOth += Convert.ToDecimal(dtTransDetail.Rows[k]["OldLineOth"].ToString());
                                totalTransTotalSSR += Convert.ToDecimal(dtTransDetail.Rows[k]["OldLineSSR"].ToString());
                                totalTransTotalSeat += Convert.ToDecimal(dtTransDetail.Rows[k]["OldLineSeat"].ToString());
                                totalTransTotalInfant += Convert.ToDecimal(dtTransDetail.Rows[k]["OldLineInfant"].ToString());
                                totalTransTotalDisc += Convert.ToDecimal(dtTransDetail.Rows[k]["OldLineDisc"].ToString());
                                totalTransTotalPromoDisc += Convert.ToDecimal(dtTransDetail.Rows[k]["OldLinePromoDisc"].ToString());
                                totalTransTotalTax += Convert.ToDecimal(dtTransDetail.Rows[k]["OldLineTax"].ToString());
                                totalTransSubTotal += Convert.ToDecimal(dtTransDetail.Rows[k]["OldLineTotal"].ToString());
                                totalTransAmountAll += Convert.ToDecimal(dtTransDetail.Rows[k]["OldLineTotal"].ToString());
                            }

                            //if (origin == "") origin = dtTransDetail.Rows[k]["origin"].ToString();
                            //if (dtTransDetail.Rows[k]["origin"].ToString() == origin) goingreturn = 0;
                            //else goingreturn = 1;

                            //if (goingreturn == 0)
                            //{
                            //    totalAmountGoing += Convert.ToDecimal(dtTransDetail.Rows[k]["LineTotal"].ToString());
                            //}
                            //else
                            //{
                            //    totalAmountReturn += Convert.ToDecimal(dtTransDetail.Rows[k]["LineTotal"].ToString());
                            //}

                        }
                    }


                    //if (HttpContext.Current.Session["ChgTransMain"] != null)
                    //{
                    //    totalAmountGoing = Convert.ToDecimal(dtTransMain.Rows[0]["TotalDepart"].ToString());
                    //    totalAmountReturn = Convert.ToDecimal(dtTransMain.Rows[0]["TotalReturn"].ToString());
                    //}
                    //else
                    //{

                    //}

                    //update total going and return
                    List<BookingTransactionDetail> listBookingDetail = new List<BookingTransactionDetail>();
                    listBookingDetail = objBooking.GetAllBK_TRANSDTLFilterAll(TransID, 0);
                    if (listBookingDetail != null && listBookingDetail.Count > 0)
                    {
                        string origin = listBookingDetail[0].Origin;
                        foreach (BookingTransactionDetail bookDetail in listBookingDetail)
                        {
                            if (HttpContext.Current.Session["ChgTransDetail"] != null && bookDetail.RecordLocator == PNR)
                            {
                                DataTable ChgTransDetail = (DataTable)HttpContext.Current.Session["ChgTransDetail"];
                                decimal LineTotal = 0;
                                foreach (DataRow row in ChgTransDetail.Rows)
                                {
                                    if (row["RecordLocator"].ToString() == PNR) LineTotal += Convert.ToDecimal(row["LineTotal"].ToString());
                                }
                                if (bookDetail.Origin == origin)
                                    totalAmountGoing += LineTotal;
                                else
                                    totalAmountReturn += LineTotal;
                            }
                            else
                            {
                                if (bookDetail.Origin == origin)
                                    totalAmountGoing += bookDetail.LineTotal;
                                else
                                    totalAmountReturn += bookDetail.LineTotal;
                            }
                        }
                    }


                    string paycomment = "";
                    if (HttpContext.Current.Session["ChgTransMain"] != null && Session["ChgMode"] != null)
                    {
                        paycomment = "#0" + Session["ChgMode"].ToString();
                    }

                    //update transaction tender
                    switch (PaymentType)
                    {
                        case "TabCredit":
                            if (txtCardNumber.Text != string.Empty)
                            { bookTransTenderInfo.RefNo = getEncryptedCredit(txtCardNumber.Text); }
                            else { bookTransTenderInfo.RefNo = ""; }
                            decimal processingFee = Convert.ToDecimal(HttpContext.Current.Session["TotalProcessFee"]);
                            using (profiler.Step("AssignPayment"))
                            {
                                AssignPayment(hID.Value, Convert.ToByte(objBooking.getSeqByTransID(hID.Value, PNR)), totalPaid, processingFee, lblCurrentTotalCurrency.Text, lblCurrentTotalCurrency.Text, objBooking.getTenderIDbyDesc(cmbCardType.SelectedItem.Value.ToString()), paycomment, bookTransTenderInfo.RefNo, MyUserSet.AgentName, PNR, 0, "insert");
                            }
                            break;
                        case "TabAG":
                            using (profiler.Step("AssignPayment"))
                            {
                                AssignPayment(hID.Value, Convert.ToByte(objBooking.getSeqByTransID(hID.Value, PNR)), totalPaid, 0, lblCurrentTotalCurrency.Text, lblCurrentTotalCurrency.Text, objBooking.getTenderIDbyDesc("AG"), paycomment, "", MyUserSet.AgentName, PNR, 0, "insert");
                            }
                            break;
                    }

                    //execute batch
                    if (lstbookDTLInfo.Count > 0 && errMessage == "")
                    {
                        //added by diana 20140120 - to retrieve GroupName
                        GeneralControl objGeneral = new GeneralControl();
                        string GroupName = objGeneral.getOPTGroupByCarrierCode(lstbookDTLInfo[0].CarrierCode);
                        decimal CollectedAmount = lstbookDTLInfo.Where(item => item.RecordLocator == PNR).Sum(item => item.CollectedAmount);
                        decimal PayDueAmount2 = lstbookDTLInfo.Where(item => item.RecordLocator == PNR).Max(item => item.PayDueAmount2);
                        decimal PayDueAmount1 = lstbookDTLInfo.Where(item => item.RecordLocator == PNR).Max(item => item.PayDueAmount1);
                        DateTime PayDueDate1 = lstbookDTLInfo.Where(item => item.RecordLocator == PNR).Max(item => item.PayDueDate1);
                        DateTime PayDueDate2 = lstbookDTLInfo.Where(item => item.RecordLocator == PNR).Max(item => item.PayDueDate2);
                        DateTime PayDueDate3 = lstbookDTLInfo.Where(item => item.RecordLocator == PNR).Max(item => item.PayDueDate3);
                        string sellkey = lstbookDTLInfo.Where(item => item.RecordLocator == PNR).Max(item => item.Signature);

                        lstbookDTLInfo.Where(w => w.RecordLocator == PNR && CollectedAmount >= PayDueAmount1).ToList().ForEach(s => s.NextDueDate = PayDueDate2);
                        lstbookDTLInfo.Where(w => w.RecordLocator == PNR && CollectedAmount >= (PayDueAmount1 + PayDueAmount2)).ToList().ForEach(s => s.NextDueDate = PayDueDate3);

                        //dtTransMain.Rows[0]["ExpiryDate"] = lstbookDTLInfo.Where(item => item.RecordLocator == PNR).Max(item => item.NextDueDate);
                        // begin amended by diana 20130918
                        if (totalTrans != TransTotal)
                        {
                            SetPayScheme();
                            if (Session["ChgMode"] == null)
                            {
                                PaymentInfo paymentInfo = new PaymentInfo();
                                PaymentControl objPay = new PaymentControl();
                                string AgentCountryCode = "";
                                if (Session["CountryCode"].ToString() != null)
                                    AgentCountryCode = Session["CountryCode"].ToString();
                                paymentInfo = objPay.GetPaymentScheme(PayScheme, GroupName, hID.Value, CountryCode, CurrencyCode, AgentCountryCode);
                                //paymentInfo = objPay.GetPaymentScheme(PayScheme, GroupName, hID.Value, CountryCode, CurrencyCode); //amended by diana 20140120 - change GroupName from 'AA'

                                if (paymentInfo.Code_1 == "DOB")
                                {
                                    bookHDRInfo.PaymentDateEx2 = bookHDRInfo.BookingDate.AddHours(paymentInfo.Attempt_1);
                                    bookHDRInfo.ExpiryDate = bookHDRInfo.BookingDate.AddHours(paymentInfo.Attempt_1);
                                }
                                else if (paymentInfo.Code_1 == "STD")
                                {
                                    bookHDRInfo.PaymentDateEx2 = bookHDRInfo.STDDate.AddHours(-paymentInfo.Attempt_1);
                                    bookHDRInfo.ExpiryDate = bookHDRInfo.STDDate.AddHours(-paymentInfo.Attempt_1);
                                }
                                if (paymentInfo.Code_2 == "DOB")
                                {
                                    bookHDRInfo.PaymentDateEx3 = bookHDRInfo.BookingDate.AddHours(paymentInfo.Attempt_2);
                                }
                                else if (paymentInfo.Code_2 == "STD")
                                {
                                    bookHDRInfo.PaymentDateEx3 = bookHDRInfo.STDDate.AddHours(-paymentInfo.Attempt_2);
                                }
                            }
                        }

                        //added by ketee, update total amount with processing fees
                        if (dtKeyFieldMain.Rows.Count > 0)
                        {
                            using (profiler.Step("AssignTransMain"))
                            {
                                //change currency from GetCurrency() to lblCurrentTotalCurrency.Text
                                AssignTransMain(TransID, dtKeyFieldMain.Rows[0]["AgentID"].ToString(), dtKeyFieldMain.Rows[0]["AgentCatgID"].ToString(),
                                Convert.ToByte(dtKeyFieldMain.Rows[0]["TransType"]), Convert.ToDateTime(dtKeyFieldMain.Rows[0]["BookingDate"]), TransStatus,
                                MyUserSet.AgentName, 2, totalTrans, lblCurrentTotalCurrency.Text, lblCurrentTotalCurrency.Text,
                                totalPax, totalTransAmount, totalTransPaxFee, totalTransTotalFee, totalTransTotalTax, totalTransTotalOth, totalTransSubTotal,
                                totalAmountGoing, totalAmountReturn);
                            }
                            if (Session["ChgMode"] != null)
                            {
                                dtTransMain.Rows[0]["CollectedAmt"] = totalTrans;
                                dtTransMain.Rows[0]["TotalDue"] = TransTotal - totalTrans;
                            }
                        }


                        // end amended by diana 20130918
                        if (objBooking.SaveHeaderDetailTransFirst(bookHDRInfo, lstbookDTLInfo, bookTransTenderInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Update) == false)
                        {
                            status = msgList.Err100028;
                        }
                        else
                        {

                            if (Session["ChgMode"] != null)
                            {
                                for (int k = 0; k < dtTransDetail.Rows.Count; k++)
                                {
                                    bool NewVal = true;
                                    if (Session["ChgMode"].ToString() == "1" || Session["ChgMode"].ToString() == "2" || Session["ChgMode"].ToString() == "5")//edited by romy for insure
                                    {
                                        if (dtTransDetail.Rows[k]["RecordLocator"].ToString() == PNR)
                                        {

                                            dtTransDetail.Rows[k]["OldLineTotal"] = Convert.ToDecimal(dtTransDetail.Rows[k]["OldLineTotal"].ToString());
                                            dtTransDetail.Rows[k]["OldLinePaxFee"] = Convert.ToDecimal(dtTransDetail.Rows[k]["OldLinePaxFee"].ToString());
                                            dtTransDetail.Rows[k]["OldLineFee"] = Convert.ToDecimal(dtTransDetail.Rows[k]["OldLineFee"].ToString());
                                            dtTransDetail.Rows[k]["OldLineOth"] = Convert.ToDecimal(dtTransDetail.Rows[k]["OldLineOth"].ToString());
                                            dtTransDetail.Rows[k]["OldLineSSR"] = Convert.ToDecimal(dtTransDetail.Rows[k]["OldLineSSR"].ToString());
                                            dtTransDetail.Rows[k]["OldLineSeat"] = Convert.ToDecimal(dtTransDetail.Rows[k]["OldLineSeat"].ToString());
                                            dtTransDetail.Rows[k]["OldLineInfant"] = Convert.ToDecimal(dtTransDetail.Rows[k]["OldLineInfant"].ToString());
                                            dtTransDetail.Rows[k]["OldLineDisc"] = Convert.ToDecimal(dtTransDetail.Rows[k]["OldLineDisc"].ToString());
                                            dtTransDetail.Rows[k]["OldLinePromoDisc"] = Convert.ToDecimal(dtTransDetail.Rows[k]["OldLinePromoDisc"].ToString());
                                            dtTransDetail.Rows[k]["OldLineTax"] = Convert.ToDecimal(dtTransDetail.Rows[k]["OldLineTax"].ToString());
                                        }
                                    }
                                }

                                if (Session["ChgMode"].ToString() == "1")
                                {
                                    //if mode = name change
                                    //HttpContext.Current.Session["lstPassengerData"] != null
                                    List<PassengerData> PaxList = (List<PassengerData>)HttpContext.Current.Session["lstPassengerData"];
                                    List<PassengerData> InfantList = (List<PassengerData>)HttpContext.Current.Session["lstPassInfantData"];

                                    if (PaxList != null)
                                    {
                                        PaxList = PaxList.FindAll(item => item.RecordLocator == PNR);
                                        objBooking.SaveBK_PASSENGERLIST(PaxList, CoreBase.EnumSaveType.Update);
                                    }
                                    if (InfantList != null && InfantList.Count > 0)
                                    {
                                        InfantList = InfantList.FindAll(item => item.RecordLocator == PNR);
                                        objBooking.SaveBK_PASSENGERLISTINFT(InfantList, CoreBase.EnumSaveType.Update);
                                    }

                                    //ClearSessionData();
                                    //LoadData(hID.Value);
                                }
                                else if (Session["ChgMode"].ToString() == "2")
                                {
                                    if (Session["ChgTransSSR"] != null)
                                    {
                                        //change to new add-On table, Tyas
                                        //List<ABS.Logic.GroupBooking.Booking.Bk_transssr> listAll = (List<Bk_transssr>)Session["ChgTransSSR"];
                                        List<ABS.Logic.GroupBooking.Booking.Bk_transaddon> listAll = (List<Bk_transaddon>)Session["ChgTransSSR"];
                                        if (listAll != null)
                                        {
                                            listAll = listAll.FindAll(item => item.RecordLocator == PNR);
                                            //objBooking.SaveSSRManageCommit(listAll, CoreBase.EnumSaveType.Update, PNR, "", true);
                                            if (objBooking.UpdateManage(listAll, PNR, "", true) == false)
                                            {
                                                log.Error(this, "save BK_TRANSADDON failed = " + listAll[0].TransID);
                                                return msgList.Err999999;
                                            }
                                        }


                                    }
                                }
                                //added by romy for insure
                                else if (Session["ChgMode"].ToString() == "5")
                                {
                                    List<PassengerData> PaxList = (List<PassengerData>)HttpContext.Current.Session["lstPassengerData"];

                                    if (PaxList != null)
                                    {
                                        PaxList = PaxList.FindAll(item => item.RecordLocator == PNR);
                                        RespPurchase = GetPlan.Purchase(PNR, "2");
                                        if (RespPurchase.ErrorCode == "00000")
                                        {
                                            objBooking.SaveBK_PASSENGERLIST(PaxList, CoreBase.EnumSaveType.Update);
                                        }
                                        //else { break; }
                                    }
                                    //if (Session["ChgTransSSR"] != null)
                                    //{
                                    //    List<ABS.Logic.GroupBooking.Booking.Bk_transaddon> listAll = (List<Bk_transaddon>)Session["ChgTransSSR"];
                                    //    if (listAll != null)
                                    //    {
                                    //        listAll = listAll.FindAll(item => item.RecordLocator == PNR);
                                    //        if (objBooking.UpdateManage(listAll, PNR, "", true) == false)
                                    //        {
                                    //            log.Error(this, "save BK_TRANSADDON failed = " + listAll[0].TransID);
                                    //            return "failed";
                                    //        }
                                    //    }
                                    //}
                                }
                                HttpContext.Current.Session.Remove("ChgTransMain");
                                HttpContext.Current.Session.Add("ChgTransMain", dtTransMain);

                                HttpContext.Current.Session.Remove("ChgTransDetail");
                                HttpContext.Current.Session.Add("ChgTransDetail", dtTransDetail);

                                bookHDRInfo.TransSubTotal = totalTransNew;
                                bookHDRInfo.TransTotalAmt = totalTransNew;
                                HttpContext.Current.Session.Remove("bookingMain");
                                HttpContext.Current.Session.Add("bookingMain", bookHDRInfo);
                            }
                            LoadPaymentSchedule(TransID);


                            //added by ketee, set checkbox pay full = false , 20170119
                            //remarked by Tyas
                            //cbFullPayment.Checked = false;

                            string emailContact = txtContactEmail.Text;
                            string agentEmail = agent.Email;
                            if (agentEmail == null)
                                agentEmail = emailContact;
                            else if (agentEmail == "")
                                agentEmail = emailContact;

                            //send email deposit payment
                            //string msg = eServices.GroupBookingEmailing(4, agent.AgentID, agent.Email, TransID, Convert.ToDouble(totalPaid));
                            DateTime ExpiryDate = DateTime.Now;
                            decimal NextDueAmount = 0;
                            SetPayScheme();
                            if (objBooking.UpdateBookingDueDate(MyUserSet.AgentName, GroupName, PayScheme, PNR, TransID, bookHDRInfo.ExchangeRate, ref ExpiryDate, ref NextDueAmount) == false)
                            {
                                log.Info(this, "update Booking Due Date Failed");
                            }
                            else
                            {
                                log.Info(this, "Entering (3)");
                                try
                                {
                                    if (TransStatus == 1 && NextDueAmount > 0)
                                    {
                                        string msg = eWS.GroupBookingEmailing(7, agent.AgentID, agentEmail, TransID, Convert.ToDouble(totalPaid), PNR);
                                        if (msg != "")
                                        {
                                            log.Info(this, msg);
                                        }
                                    }
                                    if (TransStatus == 2 && PreviousStatus != 3)
                                    {
                                        //verify whole booking, cancel invalid PNRs


                                        string msg = eWS.GroupBookingEmailing(5, agent.AgentID, agentEmail, TransID, Convert.ToDouble(totalPaid), PNR);
                                        if (msg != "")
                                        {
                                            log.Info(this, msg);
                                        }

                                        List<BookingTransactionDetail> listDetailCombinePNR = new List<BookingTransactionDetail>();
                                        listDetailCombinePNR = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 0, "LEN(RecordLocator) >= 6 AND ");

                                        foreach (BookingTransactionDetail TransDetail in listDetailCombinePNR)
                                        {
                                            msg = eWS.GroupBookingEmailing(6, agent.AgentID, agentEmail, TransID, Convert.ToDouble(totalPaid), TransDetail.RecordLocator);
                                            if (msg != "")
                                            {
                                                log.Info(this, msg);
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    SystemLog.Notifier.Notify(ex);
                                    log.Info(this, "Email is error.");
                                    log.Error(this, ex);
                                }
                            }
                        }
                    }
                    else
                    {
                        status = errMessage;
                    }
                }
                else
                {
                    status = errMessage;
                }

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                return msgList.Err100028;
            }
            finally
            {

            }
            if (status == "failed") status = msgList.Err100028; //added by diana 20140109 - show better text status
            if (status == "declined") status = msgList.Err100033; //added by diana 20140109 - show better text status
            if (status == "overpaid") status = msgList.Err100052; //added by ketee 20170321 - show better text status
            return status;
        }

        //added by ketee, verify if booking is valid to proceed payment
        private Boolean IsBookingUnderPaid(string PNR, decimal paymentAmt)
        {
            ABS.Navitaire.BookingManager.GetBookingResponse response = new ABS.Navitaire.BookingManager.GetBookingResponse();
            if (objBooking.GetBookingByPNR(PNR, response))
            {

            }
            else
            {

            }

            return true;
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

            try
            {
                if (TransStatus == 1) // && NextDueAmount > 0
                {
                    string msg = eWS.GroupBookingEmailing(7, agent.AgentID, agentEmail, TransID, Convert.ToDouble(totalPaid), PNR);
                    if (msg != "")
                    {
                        log.Info(this, msg);
                    }
                }
                if (TransStatus == 2) // && PreviousStatus != 3
                {
                    //verify whole booking, cancel invalid PNRs


                    string msg = eWS.GroupBookingEmailing(5, agent.AgentID, agentEmail, TransID, Convert.ToDouble(totalPaid), PNR);
                    if (msg != "")
                    {
                        log.Info(this, msg);
                    }

                    List<BookingTransactionDetail> listDetailCombinePNR = new List<BookingTransactionDetail>();
                    listDetailCombinePNR = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 0, "LEN(RecordLocator) >= 6 AND ");

                    foreach (BookingTransactionDetail TransDetail in listDetailCombinePNR)
                    {
                        msg = eWS.GroupBookingEmailing(6, agent.AgentID, agentEmail, TransID, Convert.ToDouble(totalPaid), TransDetail.RecordLocator);
                        if (msg != "")
                        {
                            log.Info(this, msg);
                        }
                    }
                    ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                    string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), hID.Value.ToString(), "");

                    Response.Redirect(Shared.MySite.PublicPages.BookingComplete + "?k=" + hashkey + "&TransID=" + TransID + "&payment=1", false);


                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
            finally
            {

            }

        }

        public void CheckJourneyExist(string sellSignature)
        {
            //test to get booking response
            ABS.Navitaire.APIBooking api = new ABS.Navitaire.APIBooking("");
            ABS.Navitaire.BookingManager.Booking booking = new ABS.Navitaire.BookingManager.Booking();
            var profiler = MiniProfiler.Current;
            using (profiler.Step("Navitaire:GetBookingFromState"))
            {
                booking = api.GetBookingFromState(sellSignature);
            }
            if (booking != null)
            {
                if (booking.Journeys.Length <= 0)
                {
                    log.Info(this, "Journey is not exist : " + sellSignature);
                }
            }
            else
            {
                log.Info(this, "Journey is not exist : " + sellSignature);
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

        public void AssignTransMain(string transID, string agentID, string agentCatgID, byte TransType, DateTime bookingDate, byte transStatus,
            string agentName, byte reminderType, decimal collectAmt, string currency, string currencyPaid, int totalPax = 0, decimal totalTransAmount = 0,
            decimal totalTransPaxFee = 0, decimal totalTransFee = 0, decimal totalTransTax = 0, decimal totalTransOth = 0, decimal totalTransSub = 0, decimal totalAmountGoing = 0, decimal totalAmountReturn = 0)
        {
            bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(transID, TransType, agentID, agentCatgID, bookingDate);
            if (totalPax > 0)
                bookHDRInfo.TransTotalPAX = totalPax;
            if (totalTransAmount > 0)
                bookHDRInfo.TransTotalAmt = totalTransAmount;
            if (totalTransPaxFee > 0)
                bookHDRInfo.TransTotalPaxFee = totalTransPaxFee;
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

            if (totalTransAmount > 0 && totalPax > 0)
                bookHDRInfo.TotalAmtAVG = Convert.ToDecimal((Math.Round(totalTransAmount / totalPax, 2)).ToString());
            if (totalAmountGoing > 0)
                bookHDRInfo.TotalAmtGoing = totalAmountGoing;
            if (totalAmountReturn > 0)
                bookHDRInfo.TotalAmtReturn = totalAmountReturn;
        }

        public void AssignPayment(string transID, byte seqNo, decimal payAmt, decimal feeAmt, string currency, string currencyPaid, string tenderID, string feeType, string cardNumber, string agentName, string PNR, byte transVoid, string commandType)
        {
            bookTransTenderInfo = new BookingTransTender();
            bookTransTenderInfo.TransID = transID;
            bookTransTenderInfo.SeqNo = seqNo;
            bookTransTenderInfo.SyncCreate = DateTime.Now;
            bookTransTenderInfo.TenderAmt = payAmt;
            bookTransTenderInfo.TenderDue = payAmt;
            bookTransTenderInfo.FeeAmt = feeAmt;
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
            //temp remarked by diana 20170325
            //bookDTLInfo = objBooking.GetSingleBK_TRANSDTL(recordlocatordb, transID, seqNo);
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
            var profiler = MiniProfiler.Current;
            try
            {
                string psDomain = ConfigurationManager.AppSettings["signature_domain"].ToString();
                string psName = ConfigurationManager.AppSettings["signature_username"].ToString();
                string psPwd = ConfigurationManager.AppSettings["signature_password"].ToString();

                string signature = "";// absNavitaire.AgentLogon("public", psDomain, psName, psPwd);
                using (profiler.Step("Navitaire:AgentLogon"))
                {
                    signature = absNavitaire.AgentLogon();
                }
                //ABS.Navitaire.AgentManager.FindAgentsResponse agentresp = absNavitaire.FindingAgent(MyUserSet.AgentName, signature);

                //string accountReference = agentresp.FindAgentResponseData.FindAgentList[0].AgentIdentifier.OrganizationCode;

                string accountReference = Session["OrganizationCode"].ToString();
                ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse accResp = new ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse();
                if (Session["AccResp"] == null)
                {
                    using (profiler.Step("Navitaire:GetAvailableCreditByReference"))
                    {
                        accResp = absNavitaire.GetCreditByAccountReference(accountReference, lblCurrentTotalCurrency.Text, signature);
                    }
                    return accResp;
                }
                else
                {
                    accResp = (ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse)Session["AccResp"];
                    return accResp;
                }
                //ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse accResp = absNavitaire.GetCreditByAccountReference(accountReference, lblCurrentTotalCurrency.Text, signature);
                //return accResp;

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                return null;
            }
        }

        protected void ValidateAmountDue(object sender, CallbackEventArgs e)
        {
            hfAmountDue.Value = lblAmountDue.Text;
        }

        #endregion

    }
}