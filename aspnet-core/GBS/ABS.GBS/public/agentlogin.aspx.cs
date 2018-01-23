using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ABS.Logic.GroupBooking.Agent;
using DevExpress.Web;
using System.Threading;

namespace GroupBooking.Web
{
    public partial class Agentlogin : System.Web.UI.Page
    {
        #region declaration
        AgentCategory AgentCat;
        AgentControl.StrucAgentSet AgentSet;

        ABS.Logic.GroupBooking.Agent.AgentControl objAgent = new ABS.Logic.GroupBooking.Agent.AgentControl();
        ABS.Logic.GroupBooking.Agent.AgentProfileControl objAgentProfile = new ABS.Logic.GroupBooking.Agent.AgentProfileControl();
        AgentCategory agCategoryInfo = new AgentCategory();

        AgentProfile agProfileInfo = new AgentProfile();
        //ABS.Logic.GroupBooking.Agent.AG_PROFILE_Logic agProfileLogic = new ABS.Logic.GroupBooking.Agent.AG_PROFILE_Logic();        

        public string iserror = string.Empty;
        #endregion
        
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session["AgentSet"] != null)
            {
                Response.Redirect("~/public/agentmain.aspx", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int optMode = 0;
            string result = string.Empty;
            try
            {
                optMode = Convert.ToInt32(Request.QueryString["optmode"]);
                if (Session["optMode"] != null && Session["optMode"].ToString() != "" && Session["optMode"].ToString() == "3")
                {
                    Session["optMode"] = null;
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.close()", true);
                    return;
                }
            }
            catch
            { optMode = 0; }

            switch (optMode)
            {
                case 1:
                    //msgcontrol.MessageDisplay("You have successfully logout!"); 
                    Session["optMode"] = null;
                    break;
                case 2:
                    Session["optMode"] = null;
                    PerformLogout();
                    break;
                case 3:
                    CloseWindow();
                    break;
                default:
                    //temp remark this
                    //Session["optMode"] = null;
                    //if (IsPostBack)
                    //{
                    //    result = PerformLogin();
                    //    msgcontrol.MessageDisplay(result);
                    //}
                    break;
            }

            if (IsCallback)
            {
                // Intentionally pauses server-side processing,
                // to demonstrate the Loading Panel functionality. 
                //Thread.Sleep(500);
            }
        }

        private string PerformLogin()
        {
            int code = 0;
            string result = "Unknown permission error, please contact the system administrator.";
            MessageList msgList = new MessageList();
            ArrayList aMsgList = new ArrayList();

            if (txtAgent.Text.Trim() != "")
            {
                if (ddl_AgentType.SelectedItem.Value.ToString() == "PublicAgent")
                {
                    code = Shared.Common.AuthPublicAgent(txtAgent.Text, txtPassword.Text, ddl_AgentType.SelectedItem.Value.ToString(),"");
                    if (code == 100)
                    {
                        result = "";
                        Response.Redirect(Shared.MySite.PublicPages.AgentMain);
                    }
                    else
                        if (code == 101)
                        {
                            result = msgList.Err100012;
                        }
                        else
                        {
                            if (code == 102)
                            {
                                // check if inactive
                                code = Shared.Common.CheckInactive(txtAgent.Text, txtPassword.Text);
                                if (code == 100)
                                    result = msgList.Err100016;
                                else
                                    result = msgList.Err100011;

                            }
                        }
                    if (code != 100) { }
                }
                else
                {
                    //Sky Agent
                    code = AuthSkyAgent(txtAgent.Text, txtPassword.Text, ddl_AgentType.SelectedItem.Value.ToString());

                    if (code == 100)
                    {
                        code = Shared.Common.AuthPublicAgent(txtAgent.Text, txtPassword.Text, ddl_AgentType.SelectedItem.Value.ToString(),"");
                        result = "";
                        if (code == 100)
                            Response.Redirect(Shared.MySite.PublicPages.AgentMain);
                    }

                    if (code == 101)
                    {
                        result = msgList.Err100012;
                    }
                    else
                    {
                        if (code == 102)
                        {
                            result = msgList.Err100011;
                        }
                    }

                }
                return result;
            }
            else
            {
                return "Please fill in login name and password";
            }
        }

        private void PerformLogout()
        {
            Session["AgentSet"] = null;
            Session["optMode"] = null;
            Response.Redirect(string.Concat(Shared.MySite.PublicPages.AgentLogin, ""));
        }

        private void CloseWindow()
        {
            Session["optMode"] = "3";
            Session["AgentSet"] = null;
        }

        private int AuthSkyAgent(string Username, string Password, string Type)
        {
            try
            {
                
                string psDomain = "EXT";
                string psName = Username;
                //string signature = SessionManager.Logon(psName, Password.Trim(), psDomain);
                ABS.Navitaire.APIBooking apiNavitaire = new ABS.Navitaire.APIBooking("");
                string signature = apiNavitaire.AgentLogon("SkyAgent", psDomain, psName, txtPassword.Text);
                if (signature != "")
                {
                    //signature = SessionManager.Logon();//
                    Session["LoginName"] = psName;
                    Session["LoginType"] = "SkyAgent";
                    Session["LoginPWD"] = txtPassword.Text.Trim();
                    Session["LoginDomain"] = psDomain;
                    if (Request.Cookies["cookieLogin"] == null)
                    {
                        HttpCookie cookie = new HttpCookie("cookieLoginName");
                        cookie.HttpOnly = true;
                        cookie.Values.Add("LoginName", psName);
                        cookie.Values.Add("PWD", txtPassword.Text.Trim());
                        cookie.Values.Add("Domain", psDomain);
                        cookie.Values.Add("Signature", signature);
                        Response.AppendCookie(cookie);
                    }

                    //check exist on database

                    if (objAgentProfile.CheckRecordExist(txtAgent.Text) == 1)
                    {

                        agProfileInfo = objAgentProfile.GetSingleAgentProfile(psName);
                        if (agProfileInfo != null)
                        {
                            AgentSet.AgentID = agProfileInfo.AgentID;
                            AgentSet.AgentCategoryID = agProfileInfo.AgentCatgID;
                            AgentSet.AgentName = agProfileInfo.Username;
                            AgentSet.Password = agProfileInfo.Password;
                            AgentSet.AgentType = ABS.Logic.Shared.EnumAgentType.SkyAgent;
                        }

                        //check blacklist
                        if (objAgent.VerifyBlackList(AgentSet.AgentID) == true)
                        {
                            //?Error
                            //added by ketee, show error msgbox
                            //MessageList msgList = new MessageList();
                            //ArrayList getmsg = new ArrayList();

                            //getmsg.Add(msgList.Testing.ToString());
                            //msgcontrol.ListMessage = getmsg;

                            return 101;//blacklisted
                        }
                        else
                        {
                            if (agProfileInfo != null)
                            {
                                AgentCat = objAgent.GetAgentCategory(AgentSet.AgentCategoryID);
                                AgentSet.BlacklistDuration = AgentCat.BlacklistDuration;
                                AgentSet.CounterTimer = AgentCat.CounterTimer;
                                AgentSet.MaxEnquiry = AgentCat.MaxEnquiry;
                                AgentSet.MaxSuspend = AgentCat.MaxSuspend;
                                AgentSet.SuspendDuration = AgentCat.SuspendDuration;
                                AgentSet.LoginName = AgentSet.AgentName;
                                AgentSet.Password = agProfileInfo.Password;
                                Session["AgentSet"] = AgentSet;

                                return 100;//success
                            }
                            else
                            {
                                return 104;
                            }
                        }
                    }
                    else
                    {
                        //find agent
                        ABS.Navitaire.APIAgent apiAgent = new ABS.Navitaire.APIAgent();
                        //Agent_Manage agentAPI = new Agent_Manage();
                        ABS.Navitaire.AgentManager.FindAgentsResponse agentresp = apiAgent.NewFindingAgent(psName, signature);
                        if (agentresp != null)
                        {                            
                            agProfileInfo.Address1 = "SkyAgent";
                            agProfileInfo.Address2 = "SkyAgent";
                            agProfileInfo.AgentCatgID = "99"; //default for skyagent
                            agProfileInfo.AgentID = agentresp.FindAgentResponseData.FindAgentList[0].AgentID.ToString();
                            agProfileInfo.City = "SkyAgent";
                            agProfileInfo.ContactFirstName = agentresp.FindAgentResponseData.FindAgentList[0].Name.FirstName;
                            agProfileInfo.ContactLastName = agentresp.FindAgentResponseData.FindAgentList[0].Name.LastName;                                
                            agProfileInfo.Country = "MY";
                            agProfileInfo.Email = "test@yahoo.com";
                            agProfileInfo.Fax = "SkyAgent";
                            agProfileInfo.Flag = 0;
                            agProfileInfo.JoinDate = DateTime.Now;
                            agProfileInfo.LastModifyDate = DateTime.Now;
                            agProfileInfo.LastSyncBy = Username;
                            agProfileInfo.LicenseNo = "SkyAgent";
                            agProfileInfo.MobileNo = "SkyAgent";
                            agProfileInfo.PhoneNo = "SkyAgent";
                            agProfileInfo.Postcode = "SkyAgent";
                            agProfileInfo.State = "KUL";
                            agProfileInfo.Title = "Mr";
                            agProfileInfo.Username = Username;
                            agProfileInfo.Password = Password;
                            agProfileInfo.Status = 1;
                            agProfileInfo.SyncCreate = DateTime.Now;
                            agProfileInfo.SyncLastUpd = DateTime.Now;

                            //agBankInfo.AccountName = Username;
                            //agBankInfo.AccountNo = "1";
                            //agBankInfo.Address1 = "SkyAgent";
                            //agBankInfo.Address2 = "SkyAgent";
                            //agBankInfo.Address3 = "SkyAgent";
                            //agBankInfo.AgentID = Username;
                            //agBankInfo.BankName = "SkyAgent";
                            //agBankInfo.City = "KUL";
                            //agBankInfo.Country = "MY";
                            //agBankInfo.LastSyncBy = Username;
                            //agBankInfo.Postcode = "";
                            //agBankInfo.State = "";
                            //agBankInfo.SyncCreate = DateTime.Now;
                            //agBankInfo.SyncLastUpd = DateTime.Now;
                        }
                        //sky agent still not exist on database

                        agProfileInfo = objAgentProfile.SaveAgentProfile(agProfileInfo, null, ABS.Logic.GroupBooking.Agent.AgentControl.EnumSaveType.Insert);
                        if (agProfileInfo != null)
                        {
                            AgentSet.AgentID = agProfileInfo.AgentID;
                            AgentSet.AgentCategoryID = agProfileInfo.AgentCatgID;
                            AgentSet.AgentName = agProfileInfo.Username;
                            AgentSet.AgentType = ABS.Logic.Shared.EnumAgentType.SkyAgent;
                            AgentCat = objAgent.GetAgentCategory(AgentSet.AgentCategoryID);
                            AgentSet.BlacklistDuration = AgentCat.BlacklistDuration;
                            AgentSet.CounterTimer = AgentCat.CounterTimer;
                            AgentSet.MaxEnquiry = AgentCat.MaxEnquiry;
                            AgentSet.MaxSuspend = AgentCat.MaxSuspend;
                            AgentSet.SuspendDuration = AgentCat.SuspendDuration;
                            AgentSet.LoginName = AgentSet.AgentName;
                            Session["AgentSet"] = AgentSet;
                            return 100;//success
                        }
                        else
                        {
                            return 104;
                        }
                    }

                }
                else
                {
                    return 102;//invalid username or password
                }
            }
            catch
            {
                return 102;//invalid username or password
            }
        }


        protected void ValidateLogin(object sender, CallbackEventArgs e)
        {
            MessageList msgList = new MessageList();
            ArrayList aMsgList = new ArrayList();

            if (txtAgent.Text.Trim() != "")
            {
                if (ddl_AgentType.SelectedItem.Value.ToString() == "PublicAgent")
                {
                    int code = Shared.Common.AuthPublicAgent(txtAgent.Text, txtPassword.Text, ddl_AgentType.SelectedItem.Value.ToString(),"");
                    if (code == 100)
                    {
                        e.Result = "";
                        //Response.Redirect(Shared.MySite.PublicPages.AgentMain);
                    }
                    else
                        if (code == 101)
                        {
                            e.Result = msgList.Err100012;
                        }
                        else
                        {
                            if (code == 102)
                            {
                                e.Result = msgList.Err100011;
                            }
                        }
                }
                else
                {
                    //Sky Agent
                    int code = AuthSkyAgent(txtAgent.Text, txtPassword.Text, ddl_AgentType.SelectedItem.Value.ToString());
                    if (code == 100)
                    {
                        e.Result = "";
                    }
                    else
                    {
                        if (code == 101)
                        {
                            e.Result = msgList.Err100012;
                        }
                        else
                        {
                            if (code == 102)
                            {
                                e.Result = msgList.Err100011;
                            }
                        }
                    }
                }
            }

        }

        protected void lb_forgetPwd_Click(object sender, EventArgs e)
        {
            Response.Redirect(Shared.MySite.PublicPages.PasswordRetrieve);
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Session["AgentSet"] = null;
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.open('','_parent','');window.close();", true);
        }

    }
}