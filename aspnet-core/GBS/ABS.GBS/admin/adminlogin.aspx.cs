using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GroupBooking.Web.Shared;

using ABS.Logic.GroupBooking.Agent;
using DevExpress.Web;
using System.Threading;

namespace GroupBooking.Web.Admin
{
    public partial class adminlogin : System.Web.UI.Page
    {
        public string iserror = string.Empty;

        AgentControl.StrucAgentSet AgentSet;
        AdminControl.StrucAdminSet AdminSet;
        ABS.Logic.GroupBooking.Agent.AgentControl objAgent = new ABS.Logic.GroupBooking.Agent.AgentControl();
        ABS.Logic.GroupBooking.Agent.AdminControl objAdmin = new ABS.Logic.GroupBooking.Agent.AdminControl();
        ABS.Logic.GroupBooking.Agent.AgentProfileControl objAgentProfile = new ABS.Logic.GroupBooking.Agent.AgentProfileControl();
        USRPROFILE_Info UsrInfo = new USRPROFILE_Info();
        USRAPP_Info UsrAppInfo = new USRAPP_Info();
      
        AgentActivity agActivityInfo = new AgentActivity();
        //GroupBooking.Logic.AgentActivity agActivityLogic = new GroupBooking.Logic.AG_ACTIVITY_Logic();

        AgentCategory agCategoryInfo = new AgentCategory();
        //GroupBooking.Logic.AG_CATEGORY_Logic agCategoryLogic = new GroupBooking.Logic.AG_CATEGORY_Logic();

        AgentProfile agProfileInfo = new AgentProfile();        
        //ABS.Logic.GroupBooking.Agent.AG_PROFILE_Logic agProfileLogic = new ABS.Logic.GroupBooking.Agent.AG_PROFILE_Logic();        

        AgentBlacklistLog agBlacklistInfo = new AgentBlacklistLog();
        //GroupBooking.Logic.AG_BLACKLIST_Logic agBlacklistLogic = new GroupBooking.Logic.AG_BLACKLIST_Logic();

        AgentBankInfo agBankInfo = new AgentBankInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            int optMode = 0;
            string result = string.Empty;
            try
            {
                optMode = Convert.ToInt32(Request.QueryString["optmode"]);
            }
            catch
            { optMode = 0; }
            
            switch (optMode)
            {
                case 1:
                    if (HttpContext.Current.Session["Logout"] == null)
                    {
                        result = PerformLogin();
                        msgcontrol.MessageDisplay(result);
                    }
                    if (HttpContext.Current.Session["Logout"] != null)
                    {
                        HttpContext.Current.Session.Remove("Logout");
                        msgcontrol.MessageDisplay("You have successfully logout!");
                    }
                    break;
                case 2:
                    HttpContext.Current.Session["Logout"] = "1";
                    PerformLogout();
                    break;
                default:
                    if (IsPostBack)
                    {
                        result = PerformLogin();
                        msgcontrol.MessageDisplay(result);
                    }
                    break;
            }


            if (!IsPostBack)
            {
                Session["AdminSet"] = null;
                HttpCookie cookie = Request.Cookies["cookieLogin"];
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Today.AddDays(-1);
                    Response.Cookies.Add(cookie);
                }
            }

            if (IsCallback)
            {
                

            }
        }

        private string PerformLogin()
        {
            bool blnAuth;
            int code = 0;
            string result = "Unknown permission error, please contact the system administrator.";
            MessageList msgList = new MessageList();
            ArrayList aMsgList = new ArrayList();

            if (txtAgent.Text.Trim() != "")
            {
                blnAuth = (AuthAdmin(txtAgent.Text, txtPassword.Text));
                if (blnAuth == true)
                {
                    result = "";
                    Response.Redirect(Shared.MySite.AdminPages.AdminMain);
                }
                else
                if (code != 100) { }
                return result;
            }
            else { return  "Please fill in login name and password" ; }
        }

        private void PerformLogout()
        {
            Session["AdminSet"] = null;
            Response.Redirect(string.Concat(Shared.MySite.AdminPages.AdminLogin, "?optmode=1"));
        }

        private Boolean AuthAdmin(string Username, string Password)
        {
            if (objAdmin.ValidateAdmin(Username, Password) == true)
            {
                //added by diana 20131115 - save lastlogintime
                DateTime timeNow = DateTime.Now;

                UsrInfo = objAdmin.GetSingleUSRPROFILE(Username,"USRPROFILE.UserName");
                UsrInfo.LastLogin = DateTime.Now;
                UsrInfo.SyncLastUpd = DateTime.Now;
                UsrInfo.LastSyncBy = UsrInfo.UserID;
                UsrInfo = objAdmin.SaveUserProfile(UsrInfo, null, ABS.Logic.GroupBooking.Agent.AdminControl.EnumSaveType.Update);

                Session["SessionLogin"] = UsrInfo.LastLogin;
                //end added by diana 20131115 - save lastlogintime

                //check blacklist

                AdminSet = objAdmin.AdminSet;
                Shared.Common.SetAdminSession(AdminSet);
                    //Response.Redirect("../public/agentmain.aspx",false);

                    return true;
                
            }
            else
            {
                return false;
            }
        }

        protected void ValidateLogin(object sender, CallbackEventArgs e)
        {
            MessageList msgList = new MessageList();
            ArrayList aMsgList = new ArrayList();
            iserror = "yes, please verify..";

            if (txtAgent.Text.Trim() != "")
            {
                if (AuthAdmin(txtAgent.Text, txtPassword.Text) == false)
                {
                    e.Result = "Invalid Username";
                }
                else
                {
                    
                    e.Result = "";
                }
            }
            else
            {
                e.Result = "Please fill in login name and password";
            }
             
        }



    }    
}