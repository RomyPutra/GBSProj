using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.UI.HtmlControls;

    public class SessionContext
    {
        #region "Configuration"
        public string GetConnectionString()
        {
            if (HttpContext.Current.Session["ConnectionString"] != null)
            {
                string returnURL = (string)HttpContext.Current.Session["ConnectionString"];
                return returnURL;
            }
            return null;
        }

        public void SetConnectionString(string ConnectionString)
        {
            HttpContext.Current.Session.Remove("ConnectionString");
            HttpContext.Current.Session.Add("ConnectionString", ConnectionString);
        }
        #endregion

        #region "Session"
        public string GetLogonDomain()
        {
            if (HttpContext.Current.Session["LogonDomain"] != null)
            {
                string returnURL = (string)HttpContext.Current.Session["LogonDomain"];
                return returnURL;
            }
            return null;
        }

        public void SetLogonDomain(string LogonDomain)
        {
            HttpContext.Current.Session.Remove("LogonDomain");
            HttpContext.Current.Session.Add("LogonDomain", LogonDomain);
        }

        public string GetLogonUserName()
        {
            if (HttpContext.Current.Session["LogonUserName"] != null)
            {
                string returnURL = (string)HttpContext.Current.Session["LogonUserName"];
                return returnURL;
            }
            return null;
        }

        public void SetLogonUserName(string LogonUserName)
        {
            HttpContext.Current.Session.Remove("LogonUserName");
            HttpContext.Current.Session.Add("LogonUserName", LogonUserName);
        }

        public string GetLogonPassword()
        {
            if (HttpContext.Current.Session["LogonPassword"] != null)
            {
                string returnURL = (string)HttpContext.Current.Session["LogonPassword"];
                return returnURL;
            }
            return null;
        }

        public void SetLogonPassword(string LogonPassword)
        {
            HttpContext.Current.Session.Remove("LogonPassword");
            HttpContext.Current.Session.Add("LogonPassword", LogonPassword);
        }

        public string GetSessionID()
        {
            if (HttpContext.Current.Session["SessionID"] != null)
            {
                string returnURL = (string)HttpContext.Current.Session["SessionID"];
                return returnURL;
            }
            return null;
        }

        public void SetSessionID(string SessionID)
        {
            HttpContext.Current.Session.Remove("SessionID");
            HttpContext.Current.Session.Add("SessionID", SessionID);
        }

        public Boolean ValidateAgentLogin()
        {
            if (HttpContext.Current.Session["AgentSet"] != null)
            {
                return true;
            }
            else
            {
                HttpContext.Current.Response.Redirect("../Message.aspx?msgID=400", false);
                return false;
            }

        }

        #endregion
    }
