using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using ABS.Logic.GroupBooking.Agent;
using ABS.Logic.GroupBooking;
using ABS.Logic.Shared;
using DevExpress.Web;
using ABS.Logic.GroupBooking.Booking;
using System.Globalization;
//using log4net;
using System.Configuration;
using ABS.GBS.Log;

namespace GroupBooking.Web.admin
{
    public partial class adminmain : System.Web.UI.Page
    {
        AdminSet AdminSet;
        List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain> TransMainData = new List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>();
        //added by ketee, for expiry transaction
        List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain> TransExpiryData = new List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>();
        //added by romy, for FailEmail
        List<ABS.Logic.GroupBooking.Booking.FailEmailList> FailEmailData = new List<ABS.Logic.GroupBooking.Booking.FailEmailList>();

        BookingTransactionMain ApprovalTransData = new BookingTransactionMain();
        List<BookingTransactionDetail> ApprovalTransDetailData = new List<BookingTransactionDetail>();
        DataTable AgProfile = new DataTable();
        AgentControl objAgent = new AgentControl();
        List<ABS.Logic.GroupBooking.Booking.RequestApp> RequestData = new List<ABS.Logic.GroupBooking.Booking.RequestApp>();
        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        ABS.Logic.GroupBooking.Booking.RequestApp ReqInfo = new ABS.Logic.GroupBooking.Booking.RequestApp();
        ABS.Logic.GroupBooking.Agent.RequestApp ReqInfoAgent = new ABS.Logic.GroupBooking.Agent.RequestApp();
        ABS.Logic.GroupBooking.Settings SetInfo = new ABS.Logic.GroupBooking.Settings();
        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
        AgentBlacklistLog agBlacklistInfo = new AgentBlacklistLog();
        AgentActivity agActivityInfo = new AgentActivity();
        ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");
        string optmode = "";

        List<ABS.Logic.GroupBooking.Booking.RequestApp> ReasonHistory = new List<ABS.Logic.GroupBooking.Booking.RequestApp>();

        DataTable ReqDt;
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogControl log = new LogControl();
        SystemLog SystemLog = new SystemLog();

        ABS.GBS.eService.ProcessServiceSoapClient eWS = new ABS.GBS.eService.ProcessServiceSoapClient();

        string InfoLogLocation = System.Configuration.ConfigurationManager.AppSettings["InfoLogLocation"].ToString();
        string ErrorLogLocation = System.Configuration.ConfigurationManager.AppSettings["ErrorLogLocation"].ToString();

        enum ControlState
        {
            Operator = 0,
            Supervisor = 1,
            Manager = 2, 
            Viewer = 3 //20170522 - Sienny (new role)
        }
        ControlState _myState;

        protected void Assign_gvReasonHistory()
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string msg = "";
            try
            {
                Session["modePage"] = "admin";
                optmode = Request.QueryString["optmode"];
                if (Session["AdminSet"] != null)
                {
                    AdminSet = (AdminSet)Session["AdminSet"];
                    //added by ketee
                    if (AdminSet.AdminName.ToLower() == "gbsadmin")
                    {
                        divExpiry.Style.Add("display", "");
                    }

                    //20170522 - Sienny (new role shows blank page)
                    if (AdminSet.GroupName.ToLower() == "viewer")
                        divAllContents.Visible = false;
                    
                    string action = Request.QueryString["action"];
                    string keySent = Request.QueryString["k"];
                    String TransID = Request.QueryString["TransID"];
                    if (action != null && action.ToLower() == "getlatest")
                    {
                        string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), TransID.ToString(), "");
                        if (hashkey == keySent)
                        {
                            List<ListTransaction> AllTransaction = new List<ListTransaction>();
                            AllTransaction = objBooking.GetTransactionDetails(TransID);
                            if (AllTransaction != null && AllTransaction.Count > 0)
                            {
                                ListTransaction lstTrans = AllTransaction[0];
                                //added by ketee, use latest update flight and payment function
                                //if (objBooking.UpdateBookingJourney(lstTrans.TransID, lstTrans.AgentUserName, lstTrans.AgentID, true) == false)
                                //{
                                //    log.Warning(this, "Fail to Get Latest Update for Transaction : " + lstTrans.TransID);
                                //    if (ConfigurationManager.AppSettings["ErrorLogEmail"] != null && ConfigurationManager.AppSettings["ErrorLogEmail"] != "")
                                //    {
                                //        eWS.CustomEmail(ConfigurationManager.AppSettings["ErrorLogEmail"].ToString(), lstTrans.TransID, "", "Errors Log :" + DateTime.Now.ToString() + " <br/> Fail to Get Latest Update for Transaction : " + lstTrans.TransID);
                                //    }
                                //}
                                //if (objBooking.UpdatePaymentTransaction(lstTrans.TransID, lstTrans.AgentUserName, lstTrans.AgentID, true) == false)
                                //{
                                //    log.Warning(this, "Fail to Get Latest Update Payment for Transaction : " + lstTrans.TransID);
                                //    if (ConfigurationManager.AppSettings["ErrorLogEmail"] != null && ConfigurationManager.AppSettings["ErrorLogEmail"] != "")
                                //    {
                                //        eWS.CustomEmail(ConfigurationManager.AppSettings["ErrorLogEmail"].ToString(), lstTrans.TransID, "", "Errors Log :" + DateTime.Now.ToString() + " <br/> Fail to Get Latest Update Payment for Transaction : " + lstTrans.TransID);
                                //    }
                                //}
                                List<BookingTransactionDetail> VoidPNRs = new List<BookingTransactionDetail>();
                                List<BookingTransactionDetail> ExpiredPNRs = new List<BookingTransactionDetail>();
                                if (objBooking.UpdateAllBookingJourneyDetails(lstTrans, lstTrans.AgentUserName.ToString(), lstTrans.AgentID.ToString(), ref VoidPNRs, ref ExpiredPNRs, true) == false)
                                {
                                    log.Warning(this, "Fail to Get Latest Update for Transaction : " + lstTrans.TransID);
                                    if (ConfigurationManager.AppSettings["ErrorLogEmail"] != null && ConfigurationManager.AppSettings["ErrorLogEmail"] != "")
                                    {
                                        eWS.CustomEmail(ConfigurationManager.AppSettings["ErrorLogEmail"].ToString(), lstTrans.TransID, "", "Errors Log :" + DateTime.Now.ToString() + " <br/> Fail to Get Latest Update for Transaction : " + lstTrans.TransID);
                                    }
                                }
                                else
                                {
                                    msg = objBooking.GetLatestReminderbyTransID(lstTrans.TransID);
                                    if (msg == "")
                                    {
                                        log.Info(this, "Payment/Upload passenger Reminder sending for Transaction : " + lstTrans.TransID);
                                        eWS.ResendReminder(lstTrans.TransID);
                                        msg = "";
                                    }
                                }
                            }
                        }
                    }

                    if (action != null && action.ToLower() == "getlatestall")
                    {
                        string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), TransID.ToString(), "");
                        if (hashkey == keySent)
                        {
                            List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain> AllTransaction = new List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>();

                            AllTransaction = objBooking.GetAllBK_TRANSMAINTransactionExpiry("", "", "", "1,2", false, 2);

                            foreach (ABS.Logic.GroupBooking.Booking.BookingTransactionMain b in AllTransaction)
                            {
                                UpdateLatesDetails(b.TransID);
                            }
                        }
                    }

                    if (action != null && action.ToLower() == "cancellationall")
                    {
                        string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), TransID.ToString(), "");
                        if (hashkey == keySent)
                        {
                            if (objBooking.CancelUpToDateExpiryTransaction("") == false)
                            {
                                eWS.CustomEmail(ConfigurationManager.AppSettings["ErrorLogEmail"].ToString(), "", "Cancellation Fail", " <br/> Fail to Cancel payment expiry Transaction at: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            }
                        }
                    }

                    if (action != null && action.ToLower() == "cancellationallconfirm")
                    {
                        string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), TransID.ToString(), "");
                        if (hashkey == keySent)
                        {
                            if (objBooking.CancelUpToDateExpiryTransaction("aax") == false)
                            {
                                eWS.CustomEmail(ConfigurationManager.AppSettings["ErrorLogEmail"].ToString(), "", "Cancellation Fail", " <br/> Fail to Cancel payment expiry Transaction at: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            }
                            
                        }
                    }


                    if (action != null && action.ToLower() == "CancellationAllConfirmAndPax")
                    {
                        if (objBooking.CancelUpToDateExpiryTransaction("aaxx") == false)
                        {
                            eWS.CustomEmail(ConfigurationManager.AppSettings["ErrorLogEmail"].ToString(), "", "Cancellation Fail", " <br/> Fail to Cancel payment expiry Transaction at: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        }
                    }



                    if (!IsPostBack)
                    {
                        InitState();
                        assignDefaultValue();
                        //added by ketee, update un-confirmBooking
                        UpdateUnconfirmBooking();
                    }
                    else
                        LoadGridView();

                }
                else
                {
                    Response.Redirect("~/admin/adminlogin.aspx", false);
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
            finally
            {
                //objBooking = null;
                //objGeneral = null;
            }

        }
        protected void InitState()
        {
            switch (AdminSet.GroupCode)
            {
                case "01":
                    _myState = ControlState.Operator;
                    break;
                case "02":
                    _myState = ControlState.Supervisor;
                    break;
                case "03":
                    _myState = ControlState.Manager;
                    break;
                //20170522 - Sienny (new role)
                case "04":
                    _myState = ControlState.Viewer;
                    break;
            }
        }

        //added by ketee, update validate and Un-ConfirmedBooking, 20170416
        protected Boolean UpdateUnconfirmBooking()
        {
            //added by ketee, update unconfirm booking , 20170416
            List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain> AllUnConfirmedTransaction = new List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>();

            using (objBooking)
            {
                AllUnConfirmedTransaction = objBooking.GetUnConfirmedTransaction();
                if (AllUnConfirmedTransaction != null)
                {
                    foreach(BookingTransactionMain trans in AllUnConfirmedTransaction)
                    {
                        if (objBooking.SetTransHeaderStatus(trans.TransID, trans.RecordLocator) == false)
                            log.Error(this, "UpdateUnconfirmBooking Fail: Transid: " + trans.TransID + " PNR: " + trans.RecordLocator);
                        else {
                            log.Info(this, "UpdateUnconfirmBooking Success: Transid: " + trans.TransID + " PNR: " + trans.RecordLocator);
                            UpdateLatesDetails(trans.TransID);
                        }
                    }
                }
            }
            

            return true;
        }

        //added by ketee, validate divid booking PNR if not existed, add into booking
        protected Boolean ValidateDivideBooking(string PNR)
        {
            return true;
        }


        protected void UpdateLatesDetails(string TransID)
        {
            string msg = string.Empty;
            List<ListTransaction> SingleTransaction = new List<ListTransaction>();
            SingleTransaction = objBooking.GetTransactionDetails(TransID);
            if (SingleTransaction != null && SingleTransaction.Count > 0)
            {
                ListTransaction lstTrans = SingleTransaction[0];

                List<BookingTransactionDetail> VoidPNRs = new List<BookingTransactionDetail>();
                List<BookingTransactionDetail> ExpiredPNRs = new List<BookingTransactionDetail>();
                if (objBooking.UpdateAllBookingJourneyDetails(lstTrans, lstTrans.AgentUserName.ToString(), lstTrans.AgentID.ToString(), ref VoidPNRs, ref ExpiredPNRs, true) == false)
                {
                    log.Warning(this, "Fail to Get Latest Update for Transaction - adminmain.aspx.cs : " + lstTrans.TransID);
                    if (ConfigurationManager.AppSettings["ErrorLogEmail"] != null && ConfigurationManager.AppSettings["ErrorLogEmail"] != "")
                    {
                        eWS.CustomEmail(ConfigurationManager.AppSettings["ErrorLogEmail"].ToString(), lstTrans.TransID, "", "Errors Log :" + DateTime.Now.ToString() + " <br/> Fail to Get Latest Update for Transaction : " + lstTrans.TransID);
                    }
                }
                else
                {
                    msg = objBooking.GetLatestReminderbyTransID(lstTrans.TransID);
                    if (msg == "")
                    {
                        log.Info(this, "Payment/Upload passenger Reminder sending for Transaction : " + lstTrans.TransID);
                        eWS.ResendReminder(lstTrans.TransID);
                        msg = "";
                    }
                }
            }
        }

        protected void assignDefaultValue()
        {
            Session["dtGrid"] = null;
            Session["optmode"] = null;
            Session["agid"] = null;
            Session["dtReqHistory"] = null;
            LoadDefaultGridView();
        }
        protected void LoadDefaultGridView()
        {
            assignGrid();
            if (optmode == null)
            {
                lblGridHead.Text = "Pending for Payment";
                gvFinishBooking.Columns["DueDay"].Visible = false;
            }
            else
            {
                lblGridHead.Text = "Pending for Passenger";
                gvFinishBooking.Columns["DueAmt"].Visible = false;
            }
            gvRequest.DataSource = RequestData;
            gvRequest.DataBind();
            if (_myState == ControlState.Operator)
            {
                gvRequest.Columns["Approve"].Visible = false;
            }
            if (_myState == ControlState.Supervisor || _myState == ControlState.Manager)
            {
                gvRequest.Columns["Approve"].Visible = true;
            }

            gvFinishBooking.DataSource = TransMainData;
            gvFinishBooking.DataBind();
            //added by ketee, for expiry data
            if (TransExpiryData != null && TransExpiryData.Count > 0)
            {
                gvList.DataSource = TransExpiryData;
                gvList.DataBind();
                lblTotalExpiry.Text = "Total expiry transaction : " + TransExpiryData.Count;
            }
            //added by romy, for FailEmail
            if (FailEmailData != null && FailEmailData.Count > 0)
            {
                gvpendingemail.DataSource = FailEmailData;
                gvpendingemail.DataBind();
                lblTotalMail.Text = "Total Fail Send Email : " + FailEmailData.Count;
            }
        }

        protected void LoadGridView()
        {
            assignGrid();
            Session["dtGrid"] = TransMainData;
            Session["dtGridReq"] = RequestData;
            if (Session["dtReqHistory"] != null)
            {
                gvReasonHistory.DataSource = (DataTable)Session["dtReqHistory"];
                gvReasonHistory.DataBind();
            }
            gvRequest.DataSource = (List<ABS.Logic.GroupBooking.Booking.RequestApp>)Session["dtGridReq"];
            gvRequest.DataBind();
            gvFinishBooking.DataSource = (List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>)Session["dtGrid"];
            gvFinishBooking.DataBind();
            //added by ketee, for expiry data
            if (TransExpiryData != null && TransExpiryData.Count > 0)
            {
                gvList.DataSource = TransExpiryData;
                gvList.DataBind();
                lblTotalExpiry.Text = "Total expiry transaction : " + TransExpiryData.Count;
            }
            //added by romy, for FailEmail
            if (FailEmailData != null && FailEmailData.Count > 0)
            {
                gvpendingemail.DataSource = FailEmailData;
                gvpendingemail.DataBind();
                lblTotalMail.Text = "Total Fail Send Email : " + FailEmailData.Count;
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            assignGrid();
            Session["dtGrid"] = TransMainData;
        }
        protected void assignGrid()
        {

            if (optmode == null)
            {
                if (cmbField.SelectedItem.Value.ToString() == "AgentID")
                    TransMainData = objBooking.GetAllBK_TRANSMAINStatusAdmin(txtAgentID.Text, "", "", 1);
                if (cmbField.SelectedItem.Value.ToString() == "AgentName")
                    TransMainData = objBooking.GetAllBK_TRANSMAINStatusAdmin("", txtAgentID.Text, "", 1);
                if (cmbField.SelectedItem.Value.ToString() == "RecordLocator")
                    TransMainData = objBooking.GetAllBK_TRANSMAINStatusAdmin("", "", txtAgentID.Text, 1);
                lblGridHead.Text = "Pending for Payment";
            }
            else
            {
                if (cmbField.SelectedItem.Value.ToString() == "AgentID")
                    TransMainData = objBooking.GetAllBK_TRANSMAINStatusAdmin(txtAgentID.Text, "", "", 2);
                if (cmbField.SelectedItem.Value.ToString() == "AgentName")
                    TransMainData = objBooking.GetAllBK_TRANSMAINStatusAdmin("", txtAgentID.Text, "", 2);
                if (cmbField.SelectedItem.Value.ToString() == "RecordLocator")
                    TransMainData = objBooking.GetAllBK_TRANSMAINStatusAdmin("", "", txtAgentID.Text, 2);
                lblGridHead.Text = "Pending for Passenger";
            }
            //added by ketee, for expiry data
            //TransExpiryData = objBooking.GetAllBK_TRANSMAINStatusAdmin("", "", "", 11);
            TransExpiryData = objBooking.GetAllBK_TRANSMAINBookingExpiry("", "", "", "1,2", false, 2);

            //added by romy, for FailedEmail
            FailEmailData = objBooking.GetAllAG_EMAILFail("");

            RequestData = objBooking.GetAllREQAPPL();

        }
        protected void gvFinishBooking_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            object rowKey = null;

            if (e.ButtonID == "viewBtnFinish")
            {
                rowKey = gvFinishBooking.GetRowValues(e.VisibleIndex, "TransID");
                Session["TransID"] = rowKey;

                ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), rowKey.ToString(), "");
              
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("../admin/adminbookingdetail.aspx?k=" + hashkey + "&TransID=" + rowKey);
            }
        }

        protected void gvList_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            object rowKey = null;

            if (e.ButtonID == "ViewBtnExpiry")
            {
                rowKey = gvList.GetRowValues(e.VisibleIndex, "TransID");
                Session["TransID"] = rowKey;

                ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), rowKey.ToString(), "");
              
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("../admin/adminbookingdetail.aspx?k=" + hashkey + "&TransID=" + rowKey);
            }
            if (e.ButtonID == "GetLatestBtnExpiry")
            {
                rowKey = gvList.GetRowValues(e.VisibleIndex, "TransID");
                Session["TransID"] = rowKey;

                ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), rowKey.ToString(), "");

                DevExpress.Web.ASPxWebControl.RedirectOnCallback("../admin/adminmain.aspx?action=getLatest&k=" + hashkey + "&TransID=" + rowKey);
            }
            //added by ketee, to trigger reminder - 2016-02-02

            if (e.ButtonID == "ResendReminder")
            {
                rowKey = gvList.GetRowValues(e.VisibleIndex, "TransID");
                try
                {
                    log.Info(this, "Payment/Upload passenger Reminder sending for Transaction : " + rowKey.ToString());
                    eWS.ResendReminder(rowKey.ToString());
                }
                catch (Exception ex)
                {
                    SystemLog.Notifier.Notify(ex);
                    eWS.CustomEmail(ConfigurationManager.AppSettings["ErrorLogEmail"].ToString(), rowKey.ToString(), "", "Errors Log :" + DateTime.Now.ToString() + " <br/> Fail to resend reminder for Transaction : " + rowKey.ToString());
                    log.Error(this, ex, "Resend Reminder");
                }
            }
        }

        protected void callbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            memoRemarks.Text = "";
            lblTransID.Text = (e.Parameter).ToString();
            Session["TransReqTransID"] = lblTransID.Text;
        }

        protected void callbackPanelReq_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.ToString() != "")
            {
                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
                ReqInfo = objBooking.GetSingleREQAPPL(e.Parameter.ToString());
                lblApproveID.Text = ReqInfo.TransID;
                MemoApprove.Text = ReqInfo.Remark;
                Session["TransReqTransID"] = lblApproveID.Text;
                Session["TransReq"] = e.Parameter.ToString();
                string tempflight = "";
                string tempjourney = "";
                string temprecord = "";
                int i = 0;

                System.Collections.ArrayList allOrigin;

                switch (ReqInfo.RequestDesc)
                {
                    case "Payment Expiry":
                        txtPaymentExpiry.Value = DateTime.Now.AddDays(1);
                        lblHeadApp.Text = "Payment Expiry Approval";
                        lblApproveIDText.Text = "Transaction ID";

                        tablePaymentExpiry.Visible = true;
                        tableFlight.Visible = true;
                        tableAmountPenalty.Visible = false;
                        tableAgent.Visible = false;
                        tableBlackList.Visible = false;
                        tableHistory.Visible = false;

                        ApprovalTransData = objBooking.GetSingleBK_TRANSMAIN(lblApproveID.Text);
                        ApprovalTransDetailData = objBooking.GetAllBK_TRANSDTLFlight(lblApproveID.Text);
                        lblTotal.Text = ApprovalTransData.TransTotalAmt.ToString("N", nfi) + " " + ApprovalTransData.Currency;
                        txtPaymentExpiry.Value = ApprovalTransData.ExpiryDate;
                        txtPaymentExpiry.MinDate = ApprovalTransData.ExpiryDate.AddDays(0);
                        ABS.Logic.GroupBooking.Settings SetInfo = new ABS.Logic.GroupBooking.Settings();
                        SetInfo = objGeneral.GetSingleSYS_PREFT(1, "AA", "REQEXPIRY");
                        lblCurrentExpiryDate.Text = String.Format("{0:dd MMM yyyy HH:mm}",ApprovalTransData.ExpiryDate);
                        if (ApprovalTransData.STDDate.AddDays(-7) <= DateTime.Now.Date)
                        {
                            txtPaymentExpiry.MaxDate = (ApprovalTransData.ExpiryDate).AddDays(1);
                        }
                        else
                        {
                            txtPaymentExpiry.MaxDate = (ApprovalTransData.ExpiryDate).AddDays(Convert.ToInt16(SetInfo.SYSValue));
                        }
                        

                        tempflight = "Depart : ";
                        tempjourney = "Depart : ";
                        temprecord = "";
                        i = 0;
                        
                        allOrigin = new System.Collections.ArrayList();

                        foreach (BookingTransactionDetail trans in ApprovalTransDetailData)
                        {
                            if (trans.RecordLocator.ToString().Trim().Length >= 6)
                            {
                                if (allOrigin.IndexOf(trans.Origin) < 0)
                                {
                                    allOrigin.Add(trans.Origin);
                                    if (i == 1)
                                    {
                                        tempflight += " / Return : ";
                                        tempjourney += " / Return : ";
                                    }
                                    tempflight += trans.CarrierCode + trans.FlightNo;
                                    tempjourney += trans.Origin + "-" + trans.Destination;

                                    i += 1;
                                }

                                if (temprecord.Contains(trans.RecordLocator) == false)
                                {
                                    if (temprecord != "")
                                        temprecord += ", ";
                                    temprecord += trans.RecordLocator;
                                }
                            }
                        }

                        lblFlightNo.Text = tempflight;
                        lblJourney.Text = tempjourney;
                        lblPNR.Text = temprecord;
                        lblStdDate.Text = ApprovalTransData.STDDate.ToString("ddd, dd MMM yyyy");
                        break;
                    case "Change Flight":
                        txtPaymentExpiry.Value = DateTime.Now.AddDays(1);
                        lblHeadApp.Text = "Change Flight Approval";
                        lblApproveIDText.Text = "Transaction ID";

                        tablePaymentExpiry.Visible = false;
                        tableFlight.Visible = true;
                        tableAmountPenalty.Visible = false;
                        tableAgent.Visible = false;
                        tableBlackList.Visible = false;
                        tableHistory.Visible = false;

                        ApprovalTransData = objBooking.GetSingleBK_TRANSMAIN(lblApproveID.Text);
                        ApprovalTransDetailData = objBooking.GetAllBK_TRANSDTLFlight(lblApproveID.Text);
                        lblTotal.Text = ApprovalTransData.TransTotalAmt.ToString("N", nfi) + " " + ApprovalTransData.Currency;
                        tempflight = "Depart : ";
                        tempjourney = "Depart : ";
                        temprecord = "";
                        i = 0;
                        
                        allOrigin = new System.Collections.ArrayList();

                        foreach (BookingTransactionDetail trans in ApprovalTransDetailData)
                        {
                            if (trans.RecordLocator.ToString().Trim().Length >= 6)
                            {
                                if (allOrigin.IndexOf(trans.Origin) < 0)
                                {
                                    allOrigin.Add(trans.Origin);
                                    if (i == 1)
                                    {
                                        tempflight += " / Return : ";
                                        tempjourney += " / Return : ";
                                    }
                                    tempflight += trans.CarrierCode + trans.FlightNo;
                                    tempjourney += trans.Origin + "-" + trans.Destination;

                                    i += 1;
                                }

                                if (temprecord.Contains(trans.RecordLocator) == false)
                                {
                                    if (temprecord != "")
                                        temprecord += ", ";
                                    temprecord += trans.RecordLocator;
                                }
                            }
                        }

                        lblFlightNo.Text = tempflight;
                        lblJourney.Text = tempjourney;
                        lblPNR.Text = temprecord;
                        lblStdDate.Text = ApprovalTransData.STDDate.ToString("ddd, dd MMM yyyy");
                        break;
                    case "Whitelist":
                        lblHeadApp.Text = "Whitelist Approval";
                        lblApproveIDText.Text = "Agent ID";
                        tableAgent.Visible = true;
                        tableBlackList.Visible = true;
                        tableAmountPenalty.Visible = false;
                        tableFlight.Visible = false;
                        tableHistory.Visible = true;
                        tablePaymentExpiry.Visible = false;
                        AgProfile = objAgent.SearchAgentBlacklistData(lblApproveID.Text);
                        lblAgentName.Text = AgProfile.Rows[0]["FullName"].ToString();
                        lblAddress.Text = AgProfile.Rows[0]["Address1"].ToString();
                        lblBlackListDate.Text = String.Format(AgProfile.Rows[0]["BlacklistDate"].ToString(), "ddd, dd MMM yyyy");
                        lblJoinDate.Text = String.Format(AgProfile.Rows[0]["JoinDate"].ToString(), "ddd, dd MMM yyyy");
                        lblPhone.Text = AgProfile.Rows[0]["PhoneNo"].ToString();
                        lblBlackListReason.Text = AgProfile.Rows[0]["Remark"].ToString();
                        ReqDt = objAgent.GetAllBlackWhite(lblApproveID.Text);
                        gvReasonHistory.DataSource = ReqDt;
                        gvReasonHistory.DataBind();
                        Session["dtReqHistory"] = ReqDt;

                        break;
                    case "Blacklist":
                        lblHeadApp.Text = "Blacklist Approval";
                        lblApproveIDText.Text = "Agent ID";
                        tableAgent.Visible = true;
                        tableHistory.Visible = true;
                        tableFlight.Visible = false;
                        tableAmountPenalty.Visible = false;
                        tablePaymentExpiry.Visible = false;
                        AgProfile = objAgent.SearchAgentData("AgentID", lblApproveID.Text);
                        lblAgentName.Text = AgProfile.Rows[0]["FullName"].ToString();
                        lblAddress.Text = AgProfile.Rows[0]["Address1"].ToString();
                        lblJoinDate.Text = String.Format("{0:ddd, dd MMM yyyy}", DateTime.Parse(AgProfile.Rows[0]["JoinDate"].ToString()));
                        lblPhone.Text = AgProfile.Rows[0]["PhoneNo"].ToString();
                        ReqDt = objAgent.GetAllBlackWhite(lblApproveID.Text);
                        gvReasonHistory.DataSource = ReqDt;
                        gvReasonHistory.DataBind();
                        Session["dtReqHistory"] = ReqDt;
                        break;
                    case "Cancel":
                        lblHeadApp.Text = "Cancel Approval";
                        lblApproveIDText.Text = "Transaction ID";
                        tableFlight.Visible = true;
                        tableAmountPenalty.Visible = false;
                        tableAgent.Visible = false;
                        tableBlackList.Visible = false;
                        tableHistory.Visible = false;
                        tablePaymentExpiry.Visible = false;
                        ApprovalTransData = objBooking.GetSingleBK_TRANSMAIN(lblApproveID.Text);
                        ApprovalTransDetailData = objBooking.GetAllBK_TRANSDTLFlight(lblApproveID.Text);
                        lblTotal.Text = ApprovalTransData.TransTotalAmt.ToString("N", nfi) + " " + ApprovalTransData.Currency;
                        tempflight = "Depart : ";
                        tempjourney = "Depart : ";
                        temprecord = "";
                        i = 0;
                        
                        allOrigin = new System.Collections.ArrayList();

                        foreach (BookingTransactionDetail trans in ApprovalTransDetailData)
                        {
                            if (trans.RecordLocator.ToString().Trim().Length >= 6)
                            {
                                if (allOrigin.IndexOf(trans.Origin) < 0)
                                {
                                    allOrigin.Add(trans.Origin);
                                    if (i == 1)
                                    {
                                        tempflight += " / Return : ";
                                        tempjourney += " / Return : ";
                                    }
                                    tempflight += trans.CarrierCode + trans.FlightNo;
                                    tempjourney += trans.Origin + "-" + trans.Destination;

                                    i += 1;
                                }

                                if (temprecord.Contains(trans.RecordLocator) == false)
                                {
                                    if (temprecord != "")
                                        temprecord += ", ";
                                    temprecord += trans.RecordLocator;
                                }
                            }
                        }

                        lblFlightNo.Text = tempflight;
                        lblJourney.Text = tempjourney;
                        lblPNR.Text = temprecord;
                        lblStdDate.Text = ApprovalTransData.STDDate.ToString("ddd, dd MMM yyyy");
                        break;
                    case "Waive Fee":
                        lblHeadApp.Text = "Waive Fee Approval";
                        lblApproveIDText.Text = "Transaction ID";
                        tableAmountPenalty.Visible = true;
                        tableFlight.Visible = true;
                        tableAgent.Visible = false;
                        tableBlackList.Visible = false;
                        tableHistory.Visible = false;
                        tablePaymentExpiry.Visible = false;

                        ApprovalTransData = objBooking.GetSingleBK_TRANSMAIN(lblApproveID.Text);
                        ApprovalTransDetailData = objBooking.GetAllBK_TRANSDTLFlight(lblApproveID.Text);
                        lblTotal.Text = ApprovalTransData.TransTotalAmt.ToString("N", nfi) + " " + ApprovalTransData.Currency;
                        lblAmPenalty.Text = ApprovalTransData.TransTotalOth.ToString("N", nfi) + " " + ApprovalTransData.Currency;
                        string tempflight2 = "Depart : ";
                        string tempjourney2 = "Depart : ";
                        string temprecord2 = "Depart : ";
                        int x = 0;

                        allOrigin = new System.Collections.ArrayList();

                        foreach (BookingTransactionDetail trans in ApprovalTransDetailData)
                        {
                            if (trans.RecordLocator.ToString().Trim().Length >= 6)
                            {
                                if (allOrigin.IndexOf(trans.Origin) < 0)
                                {
                                    allOrigin.Add(trans.Origin);
                                    if (x == 1)
                                    {
                                        tempflight2 += " / Return : ";
                                        tempjourney2 += " / Return : ";
                                        temprecord2 += " / Return : ";
                                    }
                                    tempflight2 += trans.CarrierCode + trans.FlightNo;
                                    tempjourney2 += trans.Origin + "-" + trans.Destination;

                                    x += 1;
                                }
                                if (temprecord2.Contains(trans.RecordLocator) == false)
                                {
                                    if (temprecord2 != "")
                                        temprecord2 += ", ";
                                    temprecord2 += trans.RecordLocator;
                                }
                            }
                        }
                        lblFlightNo.Text = tempflight2;
                        lblJourney.Text = tempjourney2;
                        lblPNR.Text = temprecord2;
                        lblStdDate.Text = ApprovalTransData.STDDate.ToString("ddd, dd MMM yyyy");
                        break;
                }
            }
        }
        protected void WaiveProcess()
        {
            string errMessage = "";
            BookingTransactionMain headerData = new BookingTransactionMain();
            List<BookingTransactionDetail> detailDatas = new List<BookingTransactionDetail>();
            List<BookingTransactionDetail> listDetailDatas = new List<BookingTransactionDetail>();
            BookingTransactionDetail detailInfo = new BookingTransactionDetail();
            List<BookingTransTender> paymentData = new List<BookingTransTender>();
            string TransID = Session["TransReqTransID"].ToString();
            try
            {

                detailDatas = objBooking.BookingDetailFilter(TransID);


                foreach (BookingTransactionDetail detail in detailDatas)
                {
                    // string signature = absNavitaire.AgentLogon();
                    // absNavitaire.CancelJourney(detail.RecordLocator, -detail.CollectedAmount, detail.Currency, signature, ref errMessage); //cancel journey to api
                    if (errMessage == "")
                    {
                        detail.LastSyncBy = AdminSet.UserName;
                        detail.SyncLastUpd = DateTime.Now;
                        detail.TransVoid = 1;
                        listDetailDatas.Add(detail);
                    }
                    else
                    {
                        break;
                    }
                }

                if (listDetailDatas.Count > 0)
                {
                    //update status on local
                    headerData = objBooking.GetSingleBK_TRANSMAIN(TransID);
                    headerData.TransStatus = 4;
                    headerData.TransTotalOth = 0;

                    paymentData = objBooking.GetAllBK_TRANSTENDERFilter(TransID);
                    AdminSet = (AdminSet)Session["AdminSet"];


                    ReqInfo.Remark = memoRemarks.Text;
                    ReqInfo.ReqType = "F";
                    ReqInfo.LastSyncBy = AdminSet.AdminID;
                    ReqInfo.ApprovedBy = AdminSet.AdminID;
                    ReqInfo.ApprovedDate = DateTime.Now;
                    if (objBooking.UpdateTransMainPenaltyWaiveApprove(headerData, ReqInfo))
                    {
                        //must success
                    }
                    else
                    {
                        //failed sql
                    }
                }
                else
                {
                    //failed, no record sent to api

                }
                //objBooking.CancelTransaction(TransID, AgentSet.AgentID, ref intError, ref strErrorDesc);
            }
            catch
            {

            }

        }
        protected void SaveRequest()
        {
            string temp = "";
            AdminSet = (AdminSet)Session["AdminSet"];
            SetInfo = objGeneral.GetSingleSYS_PREFT(1, "AA", "REQEXPIRY");
            temp = DateTime.Now.ToString("yyyyMMddHHmmsss");
            ReqInfo.ReqID = temp;
            ReqInfo.Remark = memoRemarks.Text;
            ReqInfo.ReqType = "C";
            ReqInfo.TransID = Session["TransReqTransID"].ToString();
            ReqInfo.UserID = AdminSet.AdminID;
            ReqInfo.LastSyncBy = AdminSet.AdminID;
            ReqInfo.RequestDate = DateTime.Now;
            ReqInfo.ExpiryDate = (DateTime.Now).AddDays(Convert.ToInt16(SetInfo.SYSValue));
            objBooking.SaveREQAPPL(ReqInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
        }
        protected void gvRequest_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            RequestData = objBooking.GetAllREQAPPL();
            Session["dtGridReq"] = RequestData;
            gvRequest.DataSource = (List<ABS.Logic.GroupBooking.Booking.RequestApp>)Session["dtGridReq"];
            gvRequest.DataBind();
        }

        protected void btnRequest_Click(object sender, EventArgs e)
        {
            if (ASPxEdit.AreEditorsValid(callbackPanel))
            {
                SaveRequest();
                if (optmode == null)
                {
                    if (cmbField.SelectedItem.Value.ToString() == "AgentID")
                        TransMainData = objBooking.GetAllBK_TRANSMAINStatusAdmin(txtAgentID.Text, "", "", 1);
                    if (cmbField.SelectedItem.Value.ToString() == "AgentName")
                        TransMainData = objBooking.GetAllBK_TRANSMAINStatusAdmin("", txtAgentID.Text, "", 1);
                    if (cmbField.SelectedItem.Value.ToString() == "RecordLocator")
                        TransMainData = objBooking.GetAllBK_TRANSMAINStatusAdmin("", "", txtAgentID.Text, 1);
                    lblGridHead.Text = "Pending for Payment";
                }
                else
                {
                    if (cmbField.SelectedItem.Value.ToString() == "AgentID")
                        TransMainData = objBooking.GetAllBK_TRANSMAINStatusAdmin(txtAgentID.Text, "", "", 2);
                    if (cmbField.SelectedItem.Value.ToString() == "AgentName")
                        TransMainData = objBooking.GetAllBK_TRANSMAINStatusAdmin("", txtAgentID.Text, "", 2);
                    if (cmbField.SelectedItem.Value.ToString() == "RecordLocator")
                        TransMainData = objBooking.GetAllBK_TRANSMAINStatusAdmin("", "", txtAgentID.Text, 2);
                    lblGridHead.Text = "Pending for Passenger";
                }
                Session["dtGrid"] = TransMainData;
                gvFinishBooking.DataSource = (List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>)Session["dtGrid"];
                gvFinishBooking.DataBind();
                RequestData = objBooking.GetAllREQAPPL();
                Session["dtGridReq"] = RequestData;
                gvRequest.DataSource = (List<ABS.Logic.GroupBooking.Booking.RequestApp>)Session["dtGridReq"];
                gvRequest.DataBind();
            }

        }

        protected void btnPayment_Click(object sender, EventArgs e)
        {
            Response.Redirect(Shared.MySite.AdminPages.AdminMain, false);
        }
        protected void btnPassenger_Click(object sender, EventArgs e)
        {
            Response.Redirect(Shared.MySite.AdminPages.AdminMain + "?optmode=1", false);
        }

        protected void CancelProcess()
        {
            string errMessage = "";
            BookingTransactionMain headerData = new BookingTransactionMain();
            List<BookingTransactionDetail> detailDatas = new List<BookingTransactionDetail>();
            List<BookingTransactionDetail> listDetailDatas = new List<BookingTransactionDetail>();
            BookingTransactionDetail detailInfo = new BookingTransactionDetail();
            List<BookingTransTender> paymentData = new List<BookingTransTender>();
            string TransID = Session["TransReqTransID"].ToString();
            try
            {

                detailDatas = objBooking.BookingDetailFilter(TransID, -1, 1);


                foreach (BookingTransactionDetail detail in detailDatas)
                {
                     string signature = absNavitaire.AgentLogon();
                     absNavitaire.CancelJourney(detail.RecordLocator, -detail.CollectedAmount, detail.Currency, signature, ref errMessage); //cancel journey to api
                    if (errMessage == "")
                    {
                        detail.LastSyncBy = AdminSet.UserName;
                        detail.SyncLastUpd = DateTime.Now;
                        detail.TransVoid = 1;
                        listDetailDatas.Add(detail);
                    }
                    else
                    {
                        //commented by diana 20131113 - to continue looping
                        //break;
                    }
                }

                if (listDetailDatas.Count > 0)
                {
                    //update status on local
                    headerData = objBooking.GetSingleBK_TRANSMAIN(TransID);
                    headerData.TransStatus = 4;
                    headerData.CancelDate = DateTime.Now;
                    
                    headerData.TransRemark1 = memoRemarks.Text;

                    paymentData = objBooking.GetAllBK_TRANSTENDERFilter(TransID);
                    AdminSet = (AdminSet)Session["AdminSet"];


                    ReqInfo.Remark = memoRemarks.Text;
                    ReqInfo.ReqType = "C";
                    ReqInfo.LastSyncBy = AdminSet.AdminID;
                    ReqInfo.ApprovedBy = AdminSet.AdminID;
                    ReqInfo.ApprovedDate = DateTime.Now;
                    if (objBooking.UpdateTransMainPaymentCancelApprove(headerData, listDetailDatas, paymentData, ReqInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Update))
                    {
                        //must success
                    }
                    else
                    {
                        //failed sql
                    }
                }
                else
                {
                    //failed, no record sent to api

                }
                //objBooking.CancelTransaction(TransID, AgentSet.AgentID, ref intError, ref strErrorDesc);
            }
            catch
            {

            }

        }
        protected void SetBlacklist()
        {
            string blacklistID = "";
            blacklistID = objAgent.CheckBlacklistExist(Session["TransReqTransID"].ToString());
            AgProfile = objAgent.SearchAgentData("AgentID", lblApproveID.Text);
            ReqInfoAgent.Remark = memoRemarks.Text;
            ReqInfoAgent.ReqType = "B";
            ReqInfoAgent.LastSyncBy = AdminSet.AdminID;
            ReqInfoAgent.ApprovedBy = AdminSet.AdminID;
            ReqInfoAgent.ApprovedDate = DateTime.Now;
            if (blacklistID != "")
            {
                //blacklist exist
                agBlacklistInfo = objAgent.GetSingleAG_BLACKLIST(Session["TransReqTransID"].ToString());
                agBlacklistInfo.BlacklistDate = DateTime.Now;
                agBlacklistInfo.BlacklistExpiryDate = DateTime.Now.AddDays(Convert.ToInt16(AgProfile.Rows[0]["BlacklistDuration"].ToString()));
                agBlacklistInfo.SyncLastUpd = DateTime.Now;
                agBlacklistInfo.LastSyncBy = AdminSet.AdminID;
                agBlacklistInfo.Remark = "Blacklisted by Admin";
                agActivityInfo.AgentID = Session["TransReqTransID"].ToString();
                agActivityInfo.LastBlacklist = agBlacklistInfo.BlacklistDate;
                agActivityInfo.ExpiryBlacklistDate = agBlacklistInfo.BlacklistExpiryDate;
                objAgent.SaveBlacklistApprove(agBlacklistInfo, agActivityInfo, ReqInfoAgent, ABS.Logic.GroupBooking.Agent.AgentControl.EnumSaveType.Update);
            }
            else
            {
                //insert new blacklist
                blacklistID = DateTime.Now.ToString("yyyyMMddHHmmsss");
                agBlacklistInfo.AgentID = Session["TransReqTransID"].ToString();
                agBlacklistInfo.BlacklistDate = DateTime.Now;
                agBlacklistInfo.BlacklistExpiryDate = DateTime.Now.AddDays(Convert.ToInt16(AgProfile.Rows[0]["BlacklistDuration"].ToString()));
                agBlacklistInfo.BlacklistID = blacklistID;
                agBlacklistInfo.LastSyncBy = AdminSet.AdminID;
                agBlacklistInfo.Status = 1;
                agBlacklistInfo.SyncCreate = DateTime.Now;
                agBlacklistInfo.SyncLastUpd = DateTime.Now;
                agBlacklistInfo.Remark = "Blacklisted by Admin";
                agActivityInfo.AgentID = Session["TransReqTransID"].ToString();
                agActivityInfo.LastBlacklist = agBlacklistInfo.BlacklistDate;
                agActivityInfo.ExpiryBlacklistDate = agBlacklistInfo.BlacklistExpiryDate;
                objAgent.SaveBlacklistApprove(agBlacklistInfo, agActivityInfo, ReqInfoAgent, ABS.Logic.GroupBooking.Agent.AgentControl.EnumSaveType.Insert);
            }
        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            ReqInfoAgent = objAgent.GetSingleREQAPPL(Session["TransReq"].ToString());
            ReqInfo = objBooking.GetSingleREQAPPL(Session["TransReq"].ToString());
            switch (ReqInfoAgent.RequestDesc)
            {
                case "Whitelist":
                    AdminSet = (AdminSet)Session["AdminSet"];


                    ReqInfoAgent.Remark = memoRemarks.Text;
                    ReqInfoAgent.ReqType = "W";
                    ReqInfoAgent.LastSyncBy = AdminSet.AdminID;
                    ReqInfoAgent.ApprovedBy = AdminSet.AdminID;
                    ReqInfoAgent.ApprovedDate = DateTime.Now;
                    objAgent.SetWhiteListApprove(Session["TransReqTransID"].ToString(), AdminSet.AdminID, ReqInfoAgent);
                    break;
                case "Blacklist":
                    SetBlacklist();
                    break;
            }
            switch (ReqInfo.RequestDesc)
            {
                case "Cancel":
                    CancelProcess();

                    break;
                case "Waive Fee":
                    WaiveProcess();
                    break;
                case "Payment Expiry":
                    PaymentExpiry();
                    break;
                case "Change Flight":
                    ChangeFlight();
                    break;
            }
            RequestData = objBooking.GetAllREQAPPL();
            Session["dtGridReq"] = RequestData;
            Session["dtGrid"] = TransMainData;
            gvRequest.DataSource = (List<ABS.Logic.GroupBooking.Booking.RequestApp>)Session["dtGridReq"];
            gvRequest.DataBind();
            gvFinishBooking.DataSource = (List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>)Session["dtGrid"];
            gvFinishBooking.DataBind();

            //added by diana 20131113 - to refresh the page to get latest status/date
            Response.Redirect(Shared.MySite.AdminPages.AdminMain, false);

        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            ReqInfoAgent = objAgent.GetSingleREQAPPL(Session["TransReq"].ToString());
            ReqInfo = objBooking.GetSingleREQAPPL(Session["TransReq"].ToString());
            AdminSet = (AdminSet)Session["AdminSet"];
            ReqInfoAgent.Remark = memoRemarks.Text;
            ReqInfoAgent.LastSyncBy = AdminSet.AdminID;
            ReqInfoAgent.ApprovedBy = AdminSet.AdminID;
            ReqInfoAgent.ExpiryDate = DateTime.Now;
            objAgent.SetRejectApprove(ReqInfoAgent);
            assignGrid();
            Session["dtGridReq"] = RequestData;
            Session["dtGrid"] = TransMainData;
            gvRequest.DataSource = (List<ABS.Logic.GroupBooking.Booking.RequestApp>)Session["dtGridReq"];
            gvRequest.DataBind();
            gvFinishBooking.DataSource = (List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>)Session["dtGrid"];
            gvFinishBooking.DataBind();

        }
        protected void PaymentExpiry()
        {
            string errMessage = "";
            BookingTransactionMain headerData = new BookingTransactionMain();
            List<BookingTransactionDetail> detailDatas = new List<BookingTransactionDetail>();
            List<BookingTransactionDetail> listDetailDatas = new List<BookingTransactionDetail>();
            BookingTransactionDetail detailInfo = new BookingTransactionDetail();
            List<BookingTransTender> paymentData = new List<BookingTransTender>();
            string TransID = Session["TransReqTransID"].ToString();
            try
            {
                detailDatas = objBooking.BookingDetailFilter(TransID, -1, 1);
                Boolean flagSuccess = true;
                string sign = "";
                foreach (BookingTransactionDetail detail in detailDatas)
                {                    
                    if (flagSuccess)
                    {

                        flagSuccess = absNavitaire.AddPaymentExtension(detail.RecordLocator, Convert.ToDateTime(txtPaymentExpiry.Value), ref errMessage,ref sign);

                        if (flagSuccess)
                        { flagSuccess = absNavitaire.BookingCommit(detail.RecordLocator, sign, ref errMessage); }
                        else
                        { break; }

                        /*
                        detail.LastSyncBy = AdminSet.UserName;
                        detail.SyncLastUpd = DateTime.Now;
                        detail.TransVoid = 1;
                        listDetailDatas.Add(detail);
                        */
                    }
                    else
                    {
                        break;
                    }
                }

                if (flagSuccess)
                {
                    //update status on local
                    headerData = objBooking.GetSingleBK_TRANSMAIN(TransID);
                    
                    //headerData.CancelDate = DateTime.Now;
                    headerData.TransRemark1 = memoRemarks.Text;
                    headerData.ExpiryDate = Convert.ToDateTime(txtPaymentExpiry.Value);

                    AdminSet = (AdminSet)Session["AdminSet"];

                    ReqInfo.Remark = memoRemarks.Text;
                    ReqInfo.ReqType = "PE";
                    ReqInfo.LastSyncBy = AdminSet.AdminID;
                    ReqInfo.ApprovedBy = AdminSet.AdminID;
                    ReqInfo.ApprovedDate = DateTime.Now;
                    if (objBooking.UpdateTransMainReqInfo(headerData, ReqInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Update))
                    {
                        
                    }                    
                }                                
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
            }
        }

        protected void ChangeFlight()
        {
            /*
            string errMessage = "";
            BookingTransactionMain headerData = new BookingTransactionMain();
            List<BookingTransactionDetail> detailDatas = new List<BookingTransactionDetail>();
            List<BookingTransactionDetail> listDetailDatas = new List<BookingTransactionDetail>();
            BookingTransactionDetail detailInfo = new BookingTransactionDetail();
            List<BookingTransTender> paymentData = new List<BookingTransTender>();
            string TransID = Session["TransReqTransID"].ToString();
            try
            {
                detailDatas = objBooking.BookingDetailFilter(TransID);
                Boolean flagSuccess = true;
                foreach (BookingTransactionDetail detail in detailDatas)
                {
                    if (flagSuccess)
                    {
                        //flagSuccess = absNavitaire.MoveJourney(detail.RecordLocator, Convert.ToDateTime(txtPaymentExpiry.Value), ref errMessage);                        
                    }
                    else
                    {
                        break;
                    }
                }

                if (flagSuccess)
                {
                    //update status on local
                    headerData = objBooking.GetSingleBK_TRANSMAIN(TransID);
                    
                    headerData.TransRemark1 = memoRemarks.Text;                    

                    AdminSet = (AdminSet)Session["AdminSet"];

                    ReqInfo.Remark = memoRemarks.Text;
                    ReqInfo.ReqType = "CF";
                    ReqInfo.LastSyncBy = AdminSet.AdminID;
                    ReqInfo.ApprovedBy = AdminSet.AdminID;
                    ReqInfo.ApprovedDate = DateTime.Now;
                    if (objBooking.UpdateTransMainReqInfo(headerData, ReqInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Update))
                    { }                    
                }
            }
            catch (Exception ex)
            { }*/
        }

        protected void btnGetLatest_Click(object sender, EventArgs e)
        {
            string key = DateTime.Now.ToString();
            string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), key.ToString(), "");

            Response.Redirect("../admin/adminmain.aspx?action=getLatestall&k=" + hashkey + "&TransID=" + key, false);
        }

        protected void btnCancellationAll_Click(object sender, EventArgs e)
        {
            string key = DateTime.Now.ToString();
            string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), key.ToString(), "");
            if (txtPassKey.Text.Trim().Contains("aax"))
            {
                Response.Redirect("../admin/adminmain.aspx?action=CancellationAllConfirm&k=" + hashkey + "&TransID=" + key, false);
            }
            else if (txtPassKey.Text.Trim().Contains("aaxx"))
            {
                Response.Redirect("../admin/adminmain.aspx?action=CancellationAllConfirmAndPax&k=" + hashkey + "&TransID=" + key, false);
            }
            else
            {
                Response.Redirect("../admin/adminmain.aspx?action=CancellationAll&k=" + hashkey + "&TransID=" + key, false);
            }
        }

        protected void btnLog_Click(object sender, EventArgs e)
        {
            string logFileName = "Log" + string.Format("{0:yyyyMMdd}", System.DateTime.Now) + ".txt";
            string logFilePath = InfoLogLocation + logFileName;
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment; filename=InfoLog.txt");
            Response.WriteFile(Server.MapPath(logFilePath));
            Response.End();
        }

        protected void btnErrorLog_Click(object sender, EventArgs e)
        {
            string logFileName = "Log" + string.Format("{0:yyyyMMdd}", System.DateTime.Now) + ".txt";
            string logFilePath = ErrorLogLocation + logFileName;
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment; filename=ErrorLog.txt");
            Response.WriteFile(Server.MapPath(logFilePath));
            Response.End();
        }
    }    
}