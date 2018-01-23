using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using ABS.Logic.GroupBooking.Agent;
using ABS.Logic.GroupBooking;
using ABS.Logic.Shared;
using ABS.Navitaire.AccountManager;
using ABS.Navitaire.AgentManager;
using ABS.Logic.GroupBooking.Booking;
//using log4net;
using DevExpress.Web;
using System.Data;
using System.Reflection;
using ABS.GBS.Log;
using StackExchange.Profiling;

namespace ABS.GBS.UserControl
{
    public partial class agentmain : System.Web.UI.UserControl
    {

        #region Declaration
        UserSet AgentSet;
        List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain> TransMainData = new List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>();
        List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain> TransMainDataNotif = new List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>();
        List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain> TransMainDataPNR = new List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>();
        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogControl log = new LogControl();
        SystemLog SystemLog = new SystemLog();

        DataTable dtNotification;

        #endregion
        
        protected void Page_Load(object sender, EventArgs e)
        {
            var profiler = MiniProfiler.Current;

            gvFinishBooking.JSProperties["cpIsUpdated"] = "";
            Session["generatePayment"] = "";
            Session["PaxStatus"] = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
           
            if (Session["AgentSet"] != null)
            {
                AgentSet = (UserSet)Session["AgentSet"];

                if (!IsPostBack)
                {
                    objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();

                    String TransID = Request.QueryString["TransID"];
                    string keySent = Request.QueryString["k"];
                    string action = Request.QueryString["action"];
                    ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();

                    if (TransID != null && keySent != null)
                    {
                        string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), TransID.ToString(), "");
                        if (hashkey != keySent)
                        {
                            Response.Redirect("~/Invalid.aspx");
                            return;
                        }
                        else
                        {
                            if (action.ToLower() == "getlatest")
                            {
                                List<ListTransaction> AllTransaction = new List<ListTransaction>();
                                using (profiler.Step("(AgentMain)GetTransactionDetails"))
                                {
                                    AllTransaction = objBooking.GetTransactionDetails(TransID);
                                }
                                if (AllTransaction != null && AllTransaction.Count > 0)
                                {
                                    ListTransaction lstTrans = AllTransaction[0];

                                    List<BookingTransactionDetail> VoidPNRs = new List<BookingTransactionDetail>();
                                    List<BookingTransactionDetail> ExpiredPNRs = new List<BookingTransactionDetail>();
                                    using (profiler.Step("(AgentMain)UpdateAllBookingJourneyDetails"))
                                    {
                                        objBooking.UpdateAllBookingJourneyDetails(lstTrans, AgentSet.AgentName.ToString(), AgentSet.AgentID.ToString(), ref VoidPNRs, ref ExpiredPNRs, true);
                                    }
                                    //Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.Searchflight, false);
                                }
                            }
                        }
                    }

                    //load counters
                    System.Data.DataTable dt;
                    List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain> TransExpiryData;
                    //dt=objBooking.GetNearestTransExpiry();
                    //TransExpiryData = objBooking.GetNearestBookingExpiry(AgentSet.AgentID.ToString(), "", "", "1,2", false, 2);
                    ////Amended by Ellis 20170320, change daybeforeexpiry to 14 (two weeks)
                    //TransExpiryData = objBooking.GetNearestBookingExpiry(AgentSet.AgentID.ToString(), "", "", "1,2", false, 14);
                    //if (TransExpiryData !=null && TransExpiryData.Count > 0)
                    //{
                    //    DateTime Today = DateTime.Now;
                    //    lblExpiryDate.Text = Today.AddDays(14).ToString("dd-MMM-yyyy");
                    //    lblBookingExpiry.Text = TransExpiryData.Count.ToString();
                    //}
                    //else
                    //{
                    //    lblBookingExpiry.Text = "0";
                    //}
                    //20170404 - Sienny (retrieve the 1st upcoming payment expiry booking)
                    using (profiler.Step("(AgentMain)GetUpcomingBookingExpiry"))
                    {
                        dt = objBooking.GetUpcomingBookingExpiry(AgentSet.AgentID.ToString());
                    }
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DateTime expDate = Convert.ToDateTime(dt.Rows[0]["ExpiryDate"].ToString());
                        lblExpiryDate.Text = expDate.ToString("yyyy-MM-dd hh:mm:ss");
                        //lblBookingExpiry.Text = dt.Rows[0]["TransID"].ToString();

                        //Show 1st 3 RecordLocator of TransID Upcoming Booking Expiry
                        System.Data.DataTable dtPNRExp;// = objBooking.GetUpcomingBookingExpiryPNR(dt.Rows[0]["TransID"].ToString());
                        using (profiler.Step("(AgentMain)GetUpcomingBookingExpiry"))
                        {
                            dtPNRExp = objBooking.GetUpcomingBookingExpiryPNR(dt.Rows[0]["TransID"].ToString());
                        }
                        if (dtPNRExp != null && dtPNRExp.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtPNRExp.Rows.Count; i++)
                            {
                                if (i == 0)
                                    lblBookingExpiry.Text = dtPNRExp.Rows[i]["RecordLocator"].ToString();
                                else
                                {
                                    if (i < 3)
                                        lblBookingExpiry.Text += " " + dtPNRExp.Rows[i]["RecordLocator"].ToString();
                                    else
                                        lblBookingExpiry.Text += " ...";
                                }
                            }
                        }
                    }

                    using (profiler.Step("(AgentMain)GetCountPendingPayment"))
                    {
                        dt = objBooking.GetCountPendingPayment(AgentSet.AgentID.ToString());
                    }
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        lblPendingPayment.Text = dt.Rows[0]["PendingPayment"].ToString();
                    }
                    //20170404 - Sienny (RestAmount)
                    using (profiler.Step("(AgentMain)GetRestAmountOfPendingPayment"))
                    {
                        dt = objBooking.GetRestAmountOfPendingPayment(AgentSet.AgentID.ToString());
                    }
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        double restAmt = Convert.ToDouble(dt.Rows[0]["RestAmount"].ToString());
                        lblRestAmount.Text = restAmt.ToString("N2");
                    }

                    using (profiler.Step("(AgentMain)GetCountPendingPassUpload"))
                    {
                        dt = objBooking.GetCountPendingPassUpload(AgentSet.AgentID.ToString());
                    }
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        lblPendingPassUpload.Text = dt.Rows[0]["PendingPassUpload"].ToString();
                    }

                    //20170331 - Sienny (Upcoming Flight)
                    using (profiler.Step("(AgentMain)GetCountUpcomingFlight"))
                    {
                        dt = objBooking.GetCountUpcomingFlight(AgentSet.AgentID.ToString());
                    }
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        lblUpcomingFlight.Text = dt.Rows[0]["UpcomingFlight"].ToString();
                    }

                    using (profiler.Step("(AgentMain)GetCountCanceledTrans"))
                    {
                        dt = objBooking.GetCountCanceledTrans(AgentSet.AgentID.ToString());
                    }
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        lblCanceled.Text = dt.Rows[0]["CancelledTrans"].ToString();
                    }
                    
                    dt = null;
                    using (profiler.Step("(AgentMain)ClearExpiredJourney"))
                    {
                        objBooking.ClearExpiredJourney(AgentSet.AgentID);
                    }
                    using (profiler.Step("(AgentMain)assignDefaultValue"))
                    {
                        assignDefaultValue();
                    }
                }
                else
                {
                    //LoadGridView();
                    using (profiler.Step("(AgentMain)assignDefaultValue"))
                    {
                        assignDefaultValue();
                    }
                }

                if (Session["complete"] != null)
                {
                    if (Session["complete"].ToString() == "false")
                    {
                        msgcontrol.MessageDisplay("Agent profile is not completed and will affect the payment process. Kindly update your profile, thank you.");
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "PostbackKey", "<script type='text/javascript'>window.alert('Agent profile is not completed and will affect the payment process. Kindly update your profile, thank you.');</script>");
                        Session["complete"] = "";
                    }
                }

            }
            else
            {
                Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.InvalidPage);
            }
            
        }

        protected void assignDefaultValue()
        {
            Session["dtGrid"] = null;
            Session["optemode"] = null;
            Session["agid"] = null;
            Session["AdminSet"] = null;
            Session["lstNotification"] = null;
            Session["dtNotification"] = null;
            LoadDefaultGridView();
            LoadNotification();
        }

        protected void gvDetail_BeforePerformDataSelect(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;

            /*
            TransMainData = objBooking.GetAllBK_TRANSMAINStatus(AgentSet.AgentID, "", 1);
            grid.DataSource = TransMainData;
            grid.DataBind();
            */
        }

        protected void LoadDefaultGridView()
        {
            //added by ketee, update all transaction before laoded into transaction main
            //objBooking.UpdateAllTransByAgent(AgentSet.AgentID, AgentSet.AgentName);
            var profiler = MiniProfiler.Current;

            using (profiler.Step("(AgentMain)GetAllBK_TRANSMAINStatus"))
            {
                TransMainData = objBooking.GetAllBK_TRANSMAINStatus(AgentSet.AgentID, "", 11);
            }
            Session["dtGrid"] = TransMainData;
            //DataTable dt = new DataTable();
            //dt = CreateDataTable(TransMainData);
            gvPendingBooking.DataSource = TransMainData;
            gvPendingBooking.DataBind();


            using (profiler.Step("(AgentMain)GetAllBK_TRANSMAINStatus"))
            {
                TransMainData = objBooking.GetAllBK_TRANSMAINStatus(AgentSet.AgentID, "", 2);
            }
            Session["dtGridFinish"] = TransMainData;
            gvFinishBooking.DataSource = TransMainData;
            gvFinishBooking.DataBind();


            //20170331 - Sienny (Upcoming Flight)
            using (profiler.Step("(AgentMain)GetAllBK_TRANSMAINStatus"))
            {
                TransMainData = objBooking.GetAllBK_TRANSMAINStatus(AgentSet.AgentID, "", 123);
            }
            Session["dtGridUpcoming"] = TransMainData;
            gvUpcomingBooking.DataSource = TransMainData;
            gvUpcomingBooking.DataBind();
        }

        protected void LoadNotification()
        {
            string NotificationContainer = "", TransLast="",PNRLast="";
            var profiler = MiniProfiler.Current;

            using (profiler.Step("(AgentMain)GetAllBK_TRANSMAINNotification"))
            {
                TransMainDataNotif = objBooking.GetAllBK_TRANSMAINNotification(AgentSet.AgentID, "", 11);
            }
            //Edited by romy for optimize
            //using (profiler.Step("(AgentMain)GetAllBK_TRANSMAINNotification2"))
            //{
            //    dtNotification = objBooking.GetAllBK_TRANSMAINNotification2(AgentSet.AgentID, "", 11);
            //}

            Session["lstNotification"] = TransMainData;
            //Session["dtNotification"] = dtNotification;

            if (TransMainDataNotif != null)
            {
                foreach (BookingTransactionMain transmainnotif in TransMainDataNotif)
                {
                    if (transmainnotif.Status == "was confirmed") NotificationContainer += "<li class='activity-success'>";
                    else if (transmainnotif.Status.Contains("was created")) NotificationContainer += "<li class='activity-purple'>";
                    else if (transmainnotif.Status.Contains("was pending")) NotificationContainer += "<li class='activity-primary'>";
                    else NotificationContainer += "<li class='activity-warning'>";

                    NotificationContainer += "<p class='mt-10 mb-0'>Booking ";

                    GeneralControl objGeneral = new GeneralControl();
                    string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), transmainnotif.TransID, "");
                    //DevExpress.Web.ASPxWebControl.RedirectOnCallback("../public/bookingdetail.aspx?k=" + hashkey + "&TransID=" + transmainnotif.TransID);

                    NotificationContainer += "<a href='../public/bookingdetail.aspx?k=" + hashkey + "&TransID=" + transmainnotif.TransID + "'";

                    if (transmainnotif.Status == "was confirmed") NotificationContainer += "<span class='label label-success'>";
                    else if (transmainnotif.Status.Contains("payment")) NotificationContainer += "<span class='setBgStatus1'>";
                    else if (transmainnotif.Status.Contains("passenger upload")) NotificationContainer += "<span class='setBgStatus2'>";
                    else NotificationContainer += "<span>";

                    NotificationContainer += transmainnotif.TransID;
                    NotificationContainer += "</span> ";                    
                    NotificationContainer += "</a>";
                    NotificationContainer += transmainnotif.Status;
                    NotificationContainer += " [";

                    //added by romy for optimize
                    NotificationContainer += transmainnotif.RecordLocator;

                    //using (profiler.Step("(AgentMain)GetAllBK_TRANSMAINNotificationPNR"))
                    //{
                    //    TransMainDataPNR = objBooking.GetAllBK_TRANSMAINNotificationPNR(AgentSet.AgentID, "", 11, transmainnotif.TransID);
                    //}
                    //if (TransMainDataPNR != null)
                    //{
                    //    foreach (BookingTransactionMain transmainnotifpnr in TransMainDataPNR)
                    //    {
                    //        if (TransMainDataPNR.Count > 0)
                    //        {
                    //            if (TransMainDataPNR.IndexOf(transmainnotifpnr) == 0)
                    //            {
                    //                NotificationContainer += transmainnotifpnr.RecordLocator;
                    //            }
                    //            else
                    //            {
                    //                NotificationContainer += " | " + transmainnotifpnr.RecordLocator;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            NotificationContainer += transmainnotifpnr.RecordLocator;
                    //        }
                    //    }
                    //}
                    NotificationContainer += " ]";
                    NotificationContainer += "</p>";
                    NotificationContainer += "<time datetime='2015-12-10T20:50:48+07:00' class='fs-12 text-muted'>";

                    //var startDate = new DateTime(2007, 3, 24);
                    //var endDate = new DateTime(2009, 6, 26);
                    //var dateDiff = endDate.Subtract(startDate);
                    //var date = string.Format("{0} years {1} months {2} days", (int)dateDiff.TotalDays / 365,
                    //(int)(dateDiff.TotalDays % 365) / 30, (int)(dateDiff.TotalDays % 365) / 30);
                    //Console.WriteLine(date);

                    //20170407 - Sienny (days,hours,minutes on notification panel)
                    TimeSpan tmspanbooking = (DateTime.Now - transmainnotif.BookingDate);
                    var howlong = string.Format("{0} days, {1} hours, {2} minutes, {3} seconds", tmspanbooking.Days, tmspanbooking.Hours, tmspanbooking.Minutes, tmspanbooking.Seconds);
                    System.Diagnostics.Debug.WriteLine(howlong);

                    //NotificationContainer += transmainnotif.HowLongBookingDate;
                    if (tmspanbooking.Days > 0)
                    {
                        if (tmspanbooking.Days > 1)
                            NotificationContainer += tmspanbooking.Days + " days ago";
                        else
                            NotificationContainer += tmspanbooking.Days + " day ago";
                    }
                    else
                    {
                        if (tmspanbooking.Hours > 0)
                        {
                            if (tmspanbooking.Hours > 1)
                                NotificationContainer += tmspanbooking.Hours + " hours ago";
                            else
                                NotificationContainer += tmspanbooking.Hours + " hour ago";
                        }
                        else
                        {
                            if (tmspanbooking.Minutes > 0)
                            {
                                if (tmspanbooking.Minutes > 1)
                                    NotificationContainer += tmspanbooking.Minutes + " minutes ago";
                                else
                                    NotificationContainer += tmspanbooking.Minutes + " minute ago";
                            }
                        }
                    }

                    NotificationContainer += "</time>";
                    NotificationContainer += "</li>";
                }
            }

            NotificationDiv.InnerHtml = NotificationContainer;
        }

        public DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity, null);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        protected void CancelOldTransDetails()
        {
            List<ABS.Logic.GroupBooking.Booking.BookingTransactionDetail> BookingTransactionDetail;

            List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain> LstBookingTransMain;
            var profiler = MiniProfiler.Current;

            using (profiler.Step("(AgentMain)GetBK_TRANSMAIN10"))
            {
                LstBookingTransMain = objBooking.GetBK_TRANSMAIN10(0, Convert.ToInt32(AgentSet.AgentID), "BookingDate < DATEADD(MINUTE, -20, GETDATE())");
            }

            if (LstBookingTransMain != null)
            {
                foreach (ABS.Logic.GroupBooking.Booking.BookingTransactionMain transmain in LstBookingTransMain)
                {
                    using (profiler.Step("(AgentMain)GetAllBK_TRANSDTLTOCANCEL"))
                    {
                        BookingTransactionDetail = objBooking.GetAllBK_TRANSDTLTOCANCEL(AgentSet.AgentID, transmain.TransID);
                    }
                    if (BookingTransactionDetail != null)
                    {
                        foreach (ABS.Logic.GroupBooking.Booking.BookingTransactionDetail transdtl in BookingTransactionDetail)
                        {
                            string Signature = "";
                            string errMsg = "";
                            Signature = transdtl.Signature;
                            using (profiler.Step("(AgentMain)ClearJourney"))
                            {
                                absNavitaire.ClearJourney(Signature, ref errMsg);
                            }
                            if (errMsg != null && errMsg != "")
                            {
                                return;
                            }

                        }


                    }
                    using (profiler.Step("(AgentMain)UpdateTransactionStatus"))
                    {
                        if (objBooking.UpdateTransactionStatus(transmain.TransID, "5") == false)
                        {
                            return;
                        }
                    }
                }


            }
        }

        protected void CancelExpiryTransaction()
        {
            List<ABS.Logic.GroupBooking.Booking.BookingTransactionDetail> BookingTransactionDetail;
            List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain> LstBookingTransMain;
            List<string> TransIDs = objBooking.GetAllExpiryTrans_Dtl_NextDueDate(AgentSet.AgentID);
            string TransFound = "";
            string TransDeleted = "";
            string ProcessTransID = "";
            int TransDeletedCount = 0;
            if (TransIDs != null)
            {
                try
                {
                    log.Info(this, "Expiry Transaction Found : " + TransIDs.Count + " records");
                    foreach (string TransID in TransIDs)
                    {
                        ProcessTransID = TransID;
                        BookingTransactionDetail = objBooking.Get_TRANSDTL(AgentSet.AgentID, TransID);
                        if (BookingTransactionDetail != null)
                        {
                            foreach (ABS.Logic.GroupBooking.Booking.BookingTransactionDetail transdtl in BookingTransactionDetail)
                            {
                                if (transdtl.RecordLocator.Trim().Length < 6)
                                {
                                    TransFound += ": " + transdtl.RecordLocator;
                                }
                                else
                                {
                                    //added by ketee, verify booking status before cancelled
                                    ABS.Navitaire.BookingManager.GetBookingResponse resp = absNavitaire.GetBookingResponseByPNR(transdtl.RecordLocator.Trim());
                                    if (resp != null && resp.Booking.Journeys.Length > 0)
                                    {
                                        if (resp.Booking.BookingInfo.BookingStatus.ToString().ToLower() != "confirmed")
                                        {
                                            if (objBooking.CancelProcess(BookingTransactionDetail, AgentSet.AgentID))
                                            {
                                                if (TransFound != "")
                                                {
                                                    TransFound += ", " + TransID;
                                                }
                                                else
                                                {
                                                    TransFound += TransID;
                                                }
                                                //foreach (ABS.Logic.GroupBooking.Booking.BookingTransactionDetail transdtl in BookingTransactionDetail)
                                                //{
                                                //string Signature = "";
                                                //string errMsg = "";
                                                //Signature = transdtl.Signature;
                                                //absNavitaire.ClearJourney(Signature, ref errMsg);
                                                //if (errMsg != null && errMsg != "")
                                                //{
                                                //    return;
                                                //}
                                                foreach (ABS.Logic.GroupBooking.Booking.BookingTransactionDetail transdtl2 in BookingTransactionDetail)
                                                {
                                                    TransFound += ": " + transdtl2.RecordLocator;
                                                }
                                                //}
                                                if (objBooking.UpdateTransactionStatus(TransID, "6") == false)
                                                {
                                                    log.Info(this, "Update Expiry Transaction Failed.");
                                                    //return;
                                                }
                                                else
                                                {
                                                    if (TransDeleted != "")
                                                    {
                                                        TransDeleted += ", " + TransID;
                                                    }
                                                    else
                                                    {
                                                        TransDeleted += TransID;
                                                    }
                                                    TransDeletedCount++;
                                                }
                                            }
                                            else
                                            {
                                                log.Info(this, "Cancel Transaction " + TransID + " Failed.");
                                                //return;
                                            }
                                        }
                                        else
                                        {
                                            log.Info(this, "Cancel Confirmed Transaction [" + TransID + "]. Delay Action, Double verified.");

                                        }
                                    }

                                }
                            }
                        }


                    }
                    log.Info(this, "Expiry Transaction Found: " + TransFound);
                    log.Info(this, "Expiry Transaction Deleted (" + TransDeletedCount + "): " + TransDeleted);
                }
                catch (Exception ex)
                {
                    SystemLog.Notifier.Notify(ex);
                    log.Error(this, ex, " TransID:" + ProcessTransID);
                }

            }
        }

        protected void CancelExpiryPassengerUploadTransaction()
        {
            List<ABS.Logic.GroupBooking.Booking.BookingTransactionDetail> BookingTransactionDetail;
            List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain> LstBookingTransMain;
            List<string> TransIDs = objBooking.GetAllExpiryUploadPassengersTrans(AgentSet.AgentID);

            string TransFound = "";
            string TransDeleted = "";
            string ProcessTransID = "";
            int TransDeletedCount = 0;
            if (TransIDs != null)
            {
                try
                {
                    log.Info(this, "Expiry Transaction Found : " + TransIDs.Count + " records");
                    foreach (string TransID in TransIDs)
                    {
                        ProcessTransID = TransID;
                        BookingTransactionDetail = objBooking.Get_TRANSDTL(AgentSet.AgentID, TransID);
                        if (BookingTransactionDetail != null)
                        {
                            foreach (ABS.Logic.GroupBooking.Booking.BookingTransactionDetail transdtl in BookingTransactionDetail)
                            {
                                if (transdtl.RecordLocator.Trim().Length < 6)
                                {
                                    TransFound += ": " + transdtl.RecordLocator;
                                }
                                else
                                {
                                    if (objBooking.CancelProcess(BookingTransactionDetail, AgentSet.AgentID))
                                    {
                                        if (TransFound != "")
                                        {
                                            TransFound += ", " + TransID;
                                        }
                                        else
                                        {
                                            TransFound += TransID;
                                        }
                                       
                                        foreach (ABS.Logic.GroupBooking.Booking.BookingTransactionDetail transdtl2 in BookingTransactionDetail)
                                        {
                                            TransFound += ": " + transdtl2.RecordLocator;
                                        }
                                        //}
                                        if (objBooking.UpdateTransactionStatus(TransID, "7") == false)
                                        {
                                            log.Warning(this, "Update Expiry Transaction Failed.");
                                            return;
                                        }
                                        else
                                        {
                                            if (TransDeleted != "")
                                            {
                                                TransDeleted += ", " + TransID;
                                            }
                                            else
                                            {
                                                TransDeleted += TransID;
                                            }
                                            TransDeletedCount++;
                                        }
                                    }
                                    else
                                    {
                                        log.Warning(this, "Cancel Transaction " + TransID + " Failed.");
                                        return;
                                    }
                                }
                            }
                        }


                    }
                    log.Info(this, "Expiry Transaction Found: " + TransFound);
                    log.Info(this, "Expiry Transaction Deleted (" + TransDeletedCount + "): " + TransDeleted);
                }
                catch (Exception ex)
                {
                    SystemLog.Notifier.Notify(ex);
                    log.Error(this, ex, " TransID:" + ProcessTransID);
                }

            }
        }

        protected void LoadGridView()
        {
            //if (Session["dtGrid"] == null)
            //{
            //    Session["dtGrid"] = objAgent.GetAllActiveAgent();
            //}

            if (Session["dtGridFinish"] != null)
            {
                gvFinishBooking.DataSource = (List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>)Session["dtGridFinish"];
                gvFinishBooking.DataBind();
            }
            if (Session["dtGrid"] != null)
            {
                gvPendingBooking.DataSource = (List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>)Session["dtGrid"];
                gvPendingBooking.DataBind();
            }

            //20170331-Sienny (Upcoming Flight)
            if (Session["dtGridUpcoming"] != null)
            {
                gvUpcomingBooking.DataSource = (List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>)Session["dtGridUpcoming"];
                gvUpcomingBooking.DataBind();
            }
        }

        //added by agus
        protected void gvPendingBooking_CustomColumnGroup(object sender, DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "Origin")
            {

                e.DisplayText = "------";
            }
        }
        //end added by agus

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //TransMainData = objBooking.GetAllBK_TRANSMAINFilter(Convert.ToDateTime(txtStartDate.Text),Convert.ToDateTime(txtEndDate.Text));
            //Session["dtGrid"] = TransMainData;
        }

        protected void gvPendingBooking_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            object rowKey = null;

            if (e.ButtonID == "viewBtn")
            {
                rowKey = gvPendingBooking.GetRowValues(e.VisibleIndex, "TransID");
                Session["TransID"] = rowKey;

                ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), rowKey.ToString(), "");

                DevExpress.Web.ASPxWebControl.RedirectOnCallback("../public/bookingdetail.aspx?k=" + hashkey + "&TransID=" + rowKey);
            }
            if (e.ButtonID == "changeBtn")
            {
                object TransID = gvPendingBooking.GetRowValues(e.VisibleIndex, "TransID");
                Session["TransID"] = TransID;

                rowKey = gvPendingBooking.GetRowValues(e.VisibleIndex, "RecordLocator");
                Session["RecordLocator"] = rowKey;

                ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), rowKey.ToString(), "");

                DevExpress.Web.ASPxWebControl.RedirectOnCallback("../public/searchflightchange.aspx?k=" + hashkey + "&TransID=" + TransID + "&RecordLocator=" + rowKey);
            }
            //added by ketee
            if (e.ButtonID == "getBtn")
            {
                object TransID = gvPendingBooking.GetRowValues(e.VisibleIndex, "TransID");

                ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), TransID.ToString(), "");

                DevExpress.Web.ASPxWebControl.RedirectOnCallback("../public/SearchFlight.aspx?action=getlatest&k=" + hashkey + "&TransID=" + TransID + "&RecordLocator=" + rowKey);
            }
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

                DevExpress.Web.ASPxWebControl.RedirectOnCallback("../public/bookingdetail.aspx?k=" + hashkey + "&TransID=" + rowKey);
            }
            if (e.ButtonID == "changeBtnFinish")
            {
                ((ASPxGridView)sender).JSProperties["cpIsUpdated"] = "";
                object TransID = gvFinishBooking.GetRowValues(e.VisibleIndex, "TransID");
                DateTime hDepartureDate = Convert.ToDateTime(gvFinishBooking.GetRowValues(e.VisibleIndex, "DepartureDate").ToString());
                Session["TransID"] = TransID;

                rowKey = gvFinishBooking.GetRowValues(e.VisibleIndex, "RecordLocator");
                int seqNo = 0;
                List<BookingTransactionDetail> listTransDetail = new List<BookingTransactionDetail>();
                listTransDetail = objBooking.GetAllBK_TRANSDTLFilterByPNR(TransID.ToString(), rowKey.ToString());
                int count = listTransDetail.Count;

                int ind = 0, index = 0, seqNoOther = 0;
                DateTime OtherDepartureDate = DateTime.Now;

                foreach (BookingTransactionDetail bkTransDetail in listTransDetail)
                {
                    if (bkTransDetail.DepatureDate != hDepartureDate)
                    {
                        seqNoOther = Convert.ToInt32(bkTransDetail.SeqNo.ToString());
                        OtherDepartureDate = bkTransDetail.DepatureDate;
                    }
                    else
                    {
                        seqNo = Convert.ToInt32(bkTransDetail.SeqNo.ToString());
                        index = ind;
                    }
                    ind += 1;
                }

                DateTime DepartureDate, ArrivalDate;
                bool allowChange = false;
                string msg = "", ReturnOnly = "false";
                TimeSpan timeSpan;

                if (count <= 1) //is one way
                {
                    DepartureDate = listTransDetail[0].DepatureDate;
                    ArrivalDate = listTransDetail[0].ArrivalDate;

                    timeSpan = DepartureDate - DateTime.Now;
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "PostbackKey", "<script type='text/javascript'>window.alert('" + timeSpan + "');</script>");

                    if (timeSpan.TotalDays >= 14)
                    {
                        allowChange = true;
                    }
                    ReturnOnly = "false";
                }
                else if (Convert.ToInt32(seqNo.ToString()) < seqNoOther) //is two way and change going flight
                {
                    DepartureDate = listTransDetail[index].DepatureDate;
                    ArrivalDate = listTransDetail[index].ArrivalDate;

                    timeSpan = DepartureDate - DateTime.Now;
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "PostbackKey", "<script type='text/javascript'>window.alert('" + timeSpan + "');</script>");
                    if (timeSpan.TotalDays >= 14)
                    {
                        allowChange = true;
                    }
                    ReturnOnly = "false";

                }
                else //is two way and change returning flight
                {
                    DepartureDate = listTransDetail[index].DepatureDate;
                    ArrivalDate = listTransDetail[index].ArrivalDate;

                    timeSpan = OtherDepartureDate.AddDays(2) - DateTime.Now;
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "PostbackKey", "<script type='text/javascript'>window.alert('" + timeSpan + "');</script>");
                    if (timeSpan.TotalDays >= 0)
                    {
                        allowChange = true;
                    }
                    ReturnOnly = "true";
                }
                //allowChange = false; // for testing
                if (allowChange == false)
                {
                    if (ReturnOnly == "false")
                    {
                        ((ASPxGridView)sender).JSProperties["cpIsUpdated"] = "PNR for going flight cannot be changed anymore. Changes on both segments are only allowed within 14 days before STD.";
                    }
                    else
                    {
                        ((ASPxGridView)sender).JSProperties["cpIsUpdated"] = "PNR for return flight cannot be changed anymore. Changes on return segment are only allowed within 2 days after STD.";
                    }
                }
                else
                {
                    Session["RecordLocator"] = rowKey;

                    ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                    string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), rowKey.ToString(), "");

                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("../public/searchflightchange.aspx?k=" + hashkey + "&TransID=" + TransID + "&RecordLocator=" + rowKey + "&Return=" + ReturnOnly);
                }
            }
            //added by ketee
            if (e.ButtonID == "getLatestBtn")
            {
                object TransID = gvFinishBooking.GetRowValues(e.VisibleIndex, "TransID");

                ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), TransID.ToString(), "");

                DevExpress.Web.ASPxWebControl.RedirectOnCallback("../public/SearchFlight.aspx?action=getlatest&k=" + hashkey + "&TransID=" + TransID + "&RecordLocator=" + rowKey);
            }
        }

        //20170331-Sienny (Upcoming Flight)
        protected void gvUpcomingBooking_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            object rowKey = null;

            if (e.ButtonID == "viewButton")
            {
                rowKey = gvUpcomingBooking.GetRowValues(e.VisibleIndex, "TransID");
                Session["TransID"] = rowKey;

                ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), rowKey.ToString(), "");

                DevExpress.Web.ASPxWebControl.RedirectOnCallback("../public/bookingdetail.aspx?k=" + hashkey + "&TransID=" + rowKey);
            }

            if (e.ButtonID == "changeButton")
            {
                object TransID = gvUpcomingBooking.GetRowValues(e.VisibleIndex, "TransID");
                Session["TransID"] = TransID;

                rowKey = gvUpcomingBooking.GetRowValues(e.VisibleIndex, "RecordLocator");
                Session["RecordLocator"] = rowKey;

                ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), rowKey.ToString(), "");

                DevExpress.Web.ASPxWebControl.RedirectOnCallback("../public/searchflightchange.aspx?k=" + hashkey + "&TransID=" + TransID + "&RecordLocator=" + rowKey);
            }

            //added by ketee
            if (e.ButtonID == "getLatestButton")
            {
                object TransID = gvUpcomingBooking.GetRowValues(e.VisibleIndex, "TransID");

                ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), TransID.ToString(), "");

                DevExpress.Web.ASPxWebControl.RedirectOnCallback("../public/SearchFlight.aspx?action=getlatest&k=" + hashkey + "&TransID=" + TransID + "&RecordLocator=" + rowKey);
            }
        }

        protected void lb_DownLoad_Click(object sender, EventArgs e)
        {
            string fileName = "passengerList.xls";
            string filePath = Server.MapPath("PassengerFile/template.xls");//路径

            FileStream fs = new FileStream(filePath, FileMode.Open);
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment;  filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();

        }

        protected void gvPendingBooking_PageIndexChanged(object sender, EventArgs e)
        {
            // ScriptManager1.AddHistoryPoint("pageIndex", gvPendingBooking.PageIndex.ToString(),
            // "Page #: " + gvPendingBooking.PageIndex.ToString());
        }


        protected void ScriptManager1_Navigate(object sender, HistoryEventArgs e)
        {
            /*
            string pageIndexStr = e.State["pageIndex"];
            if (String.IsNullOrEmpty(pageIndexStr))
            {
                gvPendingBooking.PageIndex = 0;
                gvFinishBooking.PageIndex = 0;
            }
            else
            {
                int pageIndex = Convert.ToInt32(pageIndexStr);
                gvPendingBooking.PageIndex = pageIndex;
                gvFinishBooking.PageIndex = pageIndex;
                pageIndexStr = Convert.ToString(pageIndex + 1);
            }
            Page.Title = pageIndexStr;*/
        }

        protected void gvFinishBooking_PageIndexChanged(object sender, EventArgs e)
        {
            //ScriptManager1.AddHistoryPoint("pageIndex", gvFinishBooking.PageIndex.ToString(),
            //"Page #: " + gvFinishBooking.PageIndex.ToString());
        }

        protected void gvUpcomingBooking_PageIndexChanged(object sender, EventArgs e)
        {
            //ScriptManager1.AddHistoryPoint("pageIndex", gvUpcomingBooking.PageIndex.ToString(),
            //"Page #: " + gvUpcomingBooking.PageIndex.ToString());
        }

        protected void btnTopup_Click(object sender, EventArgs e)
        {
            string signature = "";
            //ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse cs = CreditShell(ref signature);
            //absNavitaire.TopUpAGCredit(cs.AvailableCreditResponse.Account.AccountID, signature);
            var profiler = MiniProfiler.Current;

            ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse ag = AGCredit(ref signature);
            using (profiler.Step("(AgentMain)TopUpAGCredit"))
            {
                absNavitaire.TopUpAGCredit(ag.AvailableCreditResponse.Account.AccountID, signature);
            }
        }

        public ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse AGCredit(ref string sig)
        {
            var profiler = MiniProfiler.Current;
            try
            {
                //sig = absNavitaire.AgentLogon("SkyAgent", "EXT", AgentSet.AgentName, Session["LoginPWD"].ToString());
                //sig = absNavitaire.AgentLogon("", "WWW", "pangmeeleng@airasia.com", "pangmeeleng1");
                //ABS.Navitaire.AgentManager.FindAgentsResponse agentresp = absNavitaire.FindingAgent(AgentSet.AgentName, sig);

                //string accountReference = agentresp.FindAgentResponseData.FindAgentList[0].AgentIdentifier.OrganizationCode;
                HttpCookie cookieLogin = Request.Cookies["cookieLoginName"];
                if (cookieLogin != null)
                {
                    sig = cookieLogin.Values["Signature"].ToString();
                }
                        //sig = absNavitaire.AgentLogon();
                        string accountReference = Session["OrganizationCode"].ToString();

                ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse accResp;// = absNavitaire.GetCreditByAccountReference(accountReference, "MYR", sig);
                using (profiler.Step("(AgentMain)GetCreditByAccountReference"))
                {
                    accResp = absNavitaire.GetCreditByAccountReference(accountReference, "MYR", sig);
                }
                //ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse accResp = absNavitaire.GetCreditByAccountReference(AgentSet.AgentID, "MYR", sig);
                return accResp;

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                return null;
            }
        }

        public ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse CreditShell(ref string sig)
        {
            try
            {
                //string signature = absNavitaire.AgentLogon("", "WWW", "pangmeeleng@airasia.com", "pangmeeleng");
                string signature = absNavitaire.AgentLogon();
                sig = signature;
                ABS.Navitaire.PersonManager.FindPersonsResponse personResp = absNavitaire.FindPerson("pangmeeleng@airasia.com", signature);
                if (personResp != null)
                {
                    string accountReference = personResp.FindPersonResponseData.FindPersonList[0].CustomerNumber;
                    ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse accResp = absNavitaire.GetCreditByAccountReference(accountReference, "MYR", signature);
                    return accResp;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                return null;
            }
        }

        protected void searchLink_Click(object sender, EventArgs e)
        {

            Session["SearchFilter"] = "PendingPassengerUpload|2";
            Response.Redirect("../public/bookingmanagement.aspx", false);

        }
        protected void searchPendingLink_Click(object sender, EventArgs e)
        {

            Session["SearchFilter"] = "Pending|1";
            Response.Redirect("../public/bookingmanagement.aspx", false);

        }

        protected void searchCancelLink_Click(object sender, EventArgs e)
        {

            Session["SearchFilter"] = "Cancel|4";
            Response.Redirect("../public/bookingmanagement.aspx", false);

        }

        protected void searchUpcomingLink_Click(object sender, EventArgs e)
        {

            Session["SearchFilter"] = "Upcoming|123";
            Response.Redirect("../public/bookingmanagement.aspx", false);

        }

        protected void searchExpiryLink_Click(object sender, EventArgs e)
        {
            Session["SearchFilter"] = "Expiry|" + lblExpiryDate.Text;
            Response.Redirect("../public/bookingmanagement.aspx", false);

        }

    }
}