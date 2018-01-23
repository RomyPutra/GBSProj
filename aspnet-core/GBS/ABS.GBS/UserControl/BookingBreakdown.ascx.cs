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
using System.Globalization;
//using log4net;
using DevExpress.Data;
using DevExpress.XtraGrid;

using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Globalization;
using System.Configuration;
using ABS.GBS.Log;

namespace ABS.GBS.UserControl
{
    public partial class BookingBreakdown : System.Web.UI.UserControl
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
        List<BookingTransactionDetail> lstbookFlightInfo = new List<BookingTransactionDetail>();
        List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
        BookingTransactionMain bookHDRInfo = new BookingTransactionMain();
        ABS.Logic.GroupBooking.Agent.AgentControl objAgent = new ABS.Logic.GroupBooking.Agent.AgentControl();
        ABS.Logic.GroupBooking.Agent.AgentProfileControl objAgentProfile = new ABS.Logic.GroupBooking.Agent.AgentProfileControl();
        ABS.Logic.GroupBooking.Agent.AgentProfile agent = new ABS.Logic.GroupBooking.Agent.AgentProfile();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AgentSet"] != null)
            {
                MyUserSet = (UserSet)Session["AgentSet"];
                if (MyUserSet == null) //added by ketee 20170909
                {
                    if (Page.IsCallback)
                        ASPxCallback.RedirectOnCallback(GroupBooking.Web.Shared.MySite.PublicPages.SessionExpired);
                    else
                        Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.SessionExpired, false);
                }
            }

            //Changed by Tyas 20171011
            if (!IsPostBack)
            {
                if (MyUserSet.Currency != null && MyUserSet.Currency != "")
                {
                    lblAGCurr.Text = MyUserSet.Currency;
                    Session["AGLimit"] = null;
                    GetAGCredit(MyUserSet.Currency);
                }
            }
            InitializeForm();
        }

        protected void InitializeForm()
        {
            
            if (Session["AGLimit"] != null && Session["AGLimit"].ToString() != "'")
            {
                lblAGLimit.Text = Session["AGLimit"].ToString();
            }
            else
                lblAGLimit.Text = "0";

            if (Session["TotalAmountDue"] != null)
                lblAmountDue.Text = Session["TotalAmountDue"].ToString();

            if (Session["TotalAmountDueCurr"] != null)
                lblAmountDueCurr.Text = Session["TotalAmountDueCurr"].ToString();

            if (Session["TotalAmountDue"] != null && Session["TotalAmountDueCurr"] != null)
                lnkAmountDue.Text = Session["TotalAmountDue"].ToString() + " " + Session["TotalAmountDueCurr"];
        }

        public void GetAGCredit(string cur)
        {
            ABS.Navitaire.APIBooking apiNavitaire = new ABS.Navitaire.APIBooking("");
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            try
            {
                string accountReference = "";
                string psDomain = ConfigurationManager.AppSettings["signature_domain"].ToString();
                string psName = ConfigurationManager.AppSettings["signature_username"].ToString();
                string psPwd = ConfigurationManager.AppSettings["signature_password"].ToString();

                //string signature = apiNavitaire.AgentLogon("public", psDomain, psName, psPwd);
                //ABS.Navitaire.AgentManager.FindAgentsResponse agentresp = absNavitaire.FindingAgent(MyUserSet.AgentName, signature);
                //string accountReference = agentresp.FindAgentResponseData.FindAgentList[0].AgentIdentifier.OrganizationCode;
                if (Session["OrganizationCode"] != null)
                {
                    accountReference = Session["OrganizationCode"].ToString();
                }
                
                //edited by romy for optimize
                ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse accResp = new Navitaire.AccountManager.GetAvailableCreditByReferenceResponse();
                if (Session["AccResp"] == null)
                {
                    string signature = apiNavitaire.AgentLogon("public", psDomain, psName, psPwd);
                    if (accountReference == "")
                    {
                        ABS.Navitaire.APIAgent apiAgent = new ABS.Navitaire.APIAgent();
                        ABS.Navitaire.AgentManager.Agent ag = new ABS.Navitaire.AgentManager.Agent();// apiAgent.GetAgentByID(Username, signature, AgentID);
                        ag = apiAgent.GetAgentByID(MyUserSet.UserName, signature, MyUserSet.AgentID);
                        Session["OrganizationCode"] = ag.AgentIdentifier.OrganizationCode;
                    }
                    accResp = apiNavitaire.GetCreditByAccountReference(accountReference, cur, signature);
                    Session["AccResp"] = accResp;
                    if (accResp != null)
                    {
                        if (accResp.AvailableCreditResponse.Account != null)
                        {
                            Session["AGLimit"] = objGeneral.RoundUp(accResp.AvailableCreditResponse.Account.ForeignAmount).ToString("N", nfi);
                        }
                        else
                        {
                            Session["AGLimit"] = 0;
                        }
                    }
                    //return accResp;
                }
                else
                {
                    accResp = (ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse)Session["AccResp"];
                    Session["AGLimit"] = objGeneral.RoundUp(accResp.AvailableCreditResponse.Account.ForeignAmount).ToString("N", nfi);
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //return null;
            }
            finally
            {
                apiNavitaire = null;
                objGeneral = null;
                nfi = null;
            }
        }

        protected void lblAmountDue_OnClick(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();

        }

        protected void lnkAmountDue_OnClick(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();


        }
    }
}