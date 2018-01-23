using System;
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
using System.Globalization;
using System.Configuration;
using ABS.GBS.Log;
using StackExchange.Profiling;
//using GroupBooking.Common.APIAccountService;

namespace GroupBooking.Web
{
    public partial class BookingManagement : System.Web.UI.Page
    {
        #region declaration
        UserSet AgentSet;
        List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain> TransMainData = new List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>();
        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");
        SystemLog SystemLog = new SystemLog();
        #endregion
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["generatePayment"] = "";
            //SessionContext sesscon = new SessionContext();
            //sesscon.ValidateAgentLogin();
            if (Session["AgentSet"] != null)
            {
                AgentSet = (UserSet)Session["AgentSet"];
                if (!IsPostBack)
                {
                    assignDefaultValue();
                    lblAgentName.Text = AgentSet.AgentName;
                    lblAgentEmail.Text = AgentSet.Email;
                    lblContact.Text = AgentSet.Contact;
                    if (Session["OrganizationName"] != null)
                    {
                        lblAgentOrg.Text = Session["OrganizationName"].ToString();
                    }
                    if (AgentSet.Currency != null && AgentSet.Currency != "")
                    {
                        lblAGCurr.Text = AgentSet.Currency;
                        Session["AGLimit"] = null;
                        GetAGCredit(AgentSet.Currency);
                    }
                    if (Session["AGLimit"] != null && Session["AGLimit"].ToString() != "'")
                    {
                        lblAGLimit.Text = Session["AGLimit"].ToString();
                    }
                    else
                        lblAGLimit.Text = "0";
                }
                else
                    LoadGridView();

            }
            else
            {
                Response.Redirect("~/public/agentlogin.aspx");
            }

        }

        protected void assignDefaultValue()
        {
            txtStartDate.Value = DateTime.Now.AddDays(-3);
            txtEndDate.Value = DateTime.Now;
            Session["dtGrid"] = null;
            Session["optemode"] = null;
            Session["agid"] = null;
            if (Session["SearchFilter"] != null)
            {
                string SearchFilter=Session["SearchFilter"].ToString();
                string[] SearchFilterSplit = SearchFilter.Split('|');

                if (SearchFilterSplit[0] == "PNR")
                {
                    hfSearchFilter.Value = "PNR";
                    if (SearchFilterSplit[1].ToString().Length > 6)
                        txtTransID.Text = SearchFilterSplit[1].ToString();
                    else
                        txtRecordLocator.Text = SearchFilterSplit[1].ToString();
                }
                else if (SearchFilterSplit[0] == "PendingPassengerUpload")
                {
                    hfSearchFilter.Value = "PendingPassengerUpload";
                    cmbStatus.Value = Convert.ToInt16(SearchFilterSplit[1].ToString());
                    cmbStatus.SelectedIndex = Convert.ToInt16(SearchFilterSplit[1].ToString());
                    //Added by Ellis 20170320
                    txtStartDate.Value = null;
                    txtEndDate.Value = null;
                    LoadDefaultGridViewForBookingFilter();
                    Session["SearchFilter"] = null;
                    return;
                }
                else if (SearchFilterSplit[0] == "Pending")
                {
                    hfSearchFilter.Value = "Pending";
                    cmbStatus.Value = Convert.ToInt16(SearchFilterSplit[1].ToString());
                    cmbStatus.SelectedIndex = Convert.ToInt16(SearchFilterSplit[1].ToString());
                    //Added by Ellis 20170320
                    txtStartDate.Value = null;
                    txtEndDate.Value = null;
                    LoadDefaultGridViewForBookingFilter();
                    Session["SearchFilter"] = null;
                    return;
                }
                else if (SearchFilterSplit[0] == "Cancel")
                {
                    hfSearchFilter.Value = "Cancel";
                    cmbStatus.Value = Convert.ToInt16(SearchFilterSplit[1].ToString());
                    cmbStatus.SelectedIndex = Convert.ToInt16(SearchFilterSplit[1].ToString());
                    //Added by Ellis 20170320
                    txtStartDate.Value = null;
                    txtEndDate.Value = null;
                    LoadDefaultGridViewForBookingFilter();
                    Session["SearchFilter"] = null;
                    return;
                }
                else if (SearchFilterSplit[0] == "Expiry")
                {
                    hfSearchFilter.Value = "Expiry";
                    //Added by Ellis 20170320
                    //txtEndDate.Value = SearchFilterSplit[1];
                    txtStartDate.Value = null;
                    txtEndDate.Value = null;
                    lblExpiryDate.Text = SearchFilterSplit[1];
                    LoadDefaultGridViewForBookingFilter();
                    Session["SearchFilter"] = null;
                    return;
                }
                //20170331-Sienny (Upcoming Flight)
                else if (SearchFilterSplit[0] == "Upcoming")
                {
                    hfSearchFilter.Value = "Upcoming";
                    cmbStatus.Value = Convert.ToInt16(SearchFilterSplit[1].ToString());
                    //Added by Ellis 20170320
                    txtStartDate.Value = null;
                    txtEndDate.Value = null;
                    LoadDefaultGridViewForBookingFilter();
                    cmbStatus.SelectedIndex = Convert.ToInt16(0);
                    Session["SearchFilter"] = null;
                    return;
                }
            }
            
            LoadDefaultGridView();
            Session["SearchFilter"] = null;
            //LoadAgentCredit();
        }

        public void GetAGCredit(string cur)
        {
            ABS.Navitaire.APIBooking apiNavitaire = new ABS.Navitaire.APIBooking("");
            GeneralControl objGeneral = new GeneralControl();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            var profiler = MiniProfiler.Current;
            try
            {
                ABS.Navitaire.APIAgent apiAgent = new ABS.Navitaire.APIAgent();
                string psDomain = ConfigurationManager.AppSettings["signature_domain"].ToString();
                string psName = ConfigurationManager.AppSettings["signature_username"].ToString();
                string psPwd = ConfigurationManager.AppSettings["signature_password"].ToString();

                string signature = apiNavitaire.AgentLogon("public", psDomain, psName, psPwd);
                //ABS.Navitaire.AgentManager.FindAgentsResponse agentresp = absNavitaire.FindingAgent(MyUserSet.AgentName, signature);
                //string accountReference = agentresp.FindAgentResponseData.FindAgentList[0].AgentIdentifier.OrganizationCode;
                
                //Amended by Tyas 20170920 to fix Airbrake issue
                string accountReference = "";
                if (Session["OrganizationCode"] != null)
                {
                    accountReference = Session["OrganizationCode"].ToString();
                }
                else
                {
                    //ABS.Navitaire.AgentManager.FindAgentsResponse agentresp = absNavitaire.FindingAgent(AgentSet.AgentName, signature);

                    ABS.Navitaire.AgentManager.Agent ag = new ABS.Navitaire.AgentManager.Agent();// apiAgent.GetAgentByID(AgentSet.AgentName, signature, AgentSet.AgentID);
                    using (profiler.Step("Navitaire:GetAgent"))
                    {
                        ag = apiAgent.GetAgentByID(AgentSet.AgentName, signature, AgentSet.AgentID);
                    }
                    Session["OrganizationCode"] = ag.AgentIdentifier.OrganizationCode;
                    accountReference = Session["OrganizationCode"].ToString();
                }


                ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse accResp = apiNavitaire.GetCreditByAccountReference(accountReference, cur, signature);
                if (accResp != null && accResp.AvailableCreditResponse != null && accResp.AvailableCreditResponse.Account != null)
                {
                    Session["AGLimit"] = objGeneral.RoundUp(accResp.AvailableCreditResponse.Account.ForeignAmount).ToString("N", nfi);
                }
                //return accResp;

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

        /*
        protected void LoadAgentCredit()
        {
            try
            {                
                string signature = absNavitaire.AgentLogon("SkyAgent", "EXT", AgentSet.AgentName, Session["LoginPWD"].ToString());
                ABS.Navitaire.AgentManager.FindAgentsResponse agentresp = absNavitaire.FindingAgent(AgentSet.AgentName, signature);
                //Common.APIAgentService.FindAgentsResponse agentresp = agentAPI.NewFindingAgent(AgentSet.AgentName , signature);
                string accountReference = agentresp.FindAgentResponseData.FindAgentList[0].AgentIdentifier.OrganizationCode;

                ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse accResp = absNavitaire.GetCreditByAccountReference(accountReference, "MYR", signature);
                txtAmount.Text = accResp.AvailableCreditResponse.Account.AccountCredits[0].Available.ToString() + " " + accResp.AvailableCreditResponse.Account.AccountCredits[0].CurrencyCode;
            }
            catch (Exception ex)
            { 
            
            }
            
        }
        */

        protected void LoadDefaultGridView()
        {
            //if (Session["dtGrid"] == null)
            //{
            //    Session["dtGrid"] = objAgent.GetAllActiveAgent();
            //}
            TransMainData = objBooking.GetAllBK_TRANSMAINFilterManagementBooking(Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text), txtTransID.Text, AgentSet.AgentID, Convert.ToInt16(cmbStatus.Value.ToString()), txtRecordLocator.Text);
            gridAgent.DataSource = TransMainData;
            gridAgent.DataBind();
        }

        //Added by Ellis 20170320
        protected void LoadDefaultGridViewForBookingFilter()
        {
            TransMainData = objBooking.GetBKTransWithFilter(lblExpiryDate.Text, AgentSet.AgentID, "", Convert.ToInt16(cmbStatus.Value.ToString()), "");
            gridAgent.DataSource = TransMainData;
            gridAgent.DataBind();
        }

        protected void LoadGridView()
        {
            //if (Session["dtGrid"] == null)
            //{
            //    Session["dtGrid"] = objAgent.GetAllActiveAgent();
            //}
            if (txtStartDate.Value == null && txtEndDate.Value == null)
            {
                if (hfSearchFilter.Value == "Upcoming")
                    cmbStatus.Value = 123;
                TransMainData = objBooking.GetBKTransWithFilter(lblExpiryDate.Text, AgentSet.AgentID, "", Convert.ToInt16(cmbStatus.Value.ToString()), "");
            }
            else
            {
                TransMainData = objBooking.GetAllBK_TRANSMAINFilterManagementBooking(Convert.ToDateTime(txtStartDate.Value), Convert.ToDateTime(txtEndDate.Value), txtTransID.Text, AgentSet.AgentID, Convert.ToInt16(cmbStatus.Value.ToString()), txtRecordLocator.Text);
            }
            Session["dtGrid"] = TransMainData;
            gridAgent.DataSource = (List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>)Session["dtGrid"];
            gridAgent.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            TransMainData = objBooking.GetAllBK_TRANSMAINFilterManagementBooking(Convert.ToDateTime(txtStartDate.Value), Convert.ToDateTime(txtEndDate.Value), txtTransID.Text, AgentSet.AgentID, Convert.ToInt16(cmbStatus.Value.ToString()));
            Session["dtGrid"] = TransMainData;
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
        protected void gridAgent_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            object rowKey = null;

            if (e.ButtonID == "editBtn")
            {
                rowKey = gridAgent.GetRowValues(e.VisibleIndex, "TransID");
                Session["TransID"] = rowKey;
                Session["PNR"] = null;
                //if (Session["PNR"] == null) Session["PNR"] = "ALL";

                GeneralControl objGeneral = new GeneralControl();
                string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(),rowKey.ToString(),"");
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("../public/bookingdetail.aspx?k=" + hashkey + "&TransID="+rowKey );
            }
        }
        
        /*
        protected void btnPay_Click(object sender, EventArgs e)
        {
            string signature = absNavitaire.AgentLogon("SkyAgent", "EXT", AgentSet.AgentName, Session["LoginPWD"].ToString());
            ABS.Navitaire.AgentManager.FindAgentsResponse agentresp = absNavitaire.FindingAgent(AgentSet.AgentName, signature);
            
            string accountReference = agentresp.FindAgentResponseData.FindAgentList[0].AgentIdentifier.OrganizationCode;

            ABS.Navitaire.AccountManager.GetAvailableCreditByReferenceResponse accResp = absNavitaire.GetCreditByAccountReference(accountReference, "MYR", signature);
            absNavitaire.TopUpAGCredit(accResp.AvailableCreditResponse.Account.AccountID, signature); 

            
            //string errMsg = "";
            //string sig = absNavitaire.GetBookingByPNR(txtPNR.Text);

            //if (absNavitaire.AddPaymentByVI("4444333322221111", "2012" + "-" + "12" + "-" + "01", "MYR", Convert.ToDecimal(txtAmount.Text), "123", "AA Test", "MAL", "MAL", sig, ref errMsg))
            //{
            //    absNavitaire.BookingCommit(txtPNR.Text,sig);
            //}
        }*/
        /*
        protected void btnCreditFilePayment_Click(object sender, EventArgs e)
        {
            //GetAccountRequest accRequest = new GetAccountRequest();
            

            string errMsg = "";            
            absNavitaire.GetCreditFile(txtMemberID.Text, txtPassword.Text);
        }
        */

    }
}
