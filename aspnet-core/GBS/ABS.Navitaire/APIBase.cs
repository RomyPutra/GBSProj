using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
//using log4net;
using ABS.Navitaire.SessionManager;
using System.Configuration;
using ABS.Logic.Shared;
//added by ketee 20160104
using System.Web;
using System.Data;
using ABS.GBS.Log;
using StackExchange.Profiling;

namespace ABS
{
    public abstract partial class APIBase : Component
    {
        private string _Sign = string.Empty;
        private string _RtnMsg = string.Empty;
        private string _UserName = string.Empty;
        private string _Password = string.Empty;
        private string _Domain = string.Empty;
        //Added by ketee 20160104
        private string _aceSessionURL = string.Empty;
        private string _aceLookUpURL = string.Empty;
        private int _Contract = 1; //amended by diana 20131226 - set to 342, previously is 1
        private bool _Discon = false;

        //public ILog log = LogManager.GetLogger("Logging.Info");
        public LogControl log = new LogControl();
        public SystemLog SystemLog = new SystemLog();

        public APIBase()
        {
            InitializeComponent();
            //_Domain = Properties.Settings.Default.DomainCode;
            //_UserName = Properties.Settings.Default.DomainCode;
            //_Password = Properties.Settings.Default.DomainCode;
            //_Contract = Properties.Settings.Default.ContractVersion;
            _Domain = ConfigurationManager.AppSettings["signature_domain"].ToString();
            _UserName = ConfigurationManager.AppSettings["signature_username"].ToString();
            _Password = ConfigurationManager.AppSettings["signature_password"].ToString();
            _Contract = Convert.ToInt16(ConfigurationManager.AppSettings["ContractVersion"].ToString());
            _aceSessionURL = ConfigurationManager.AppSettings["ACESession"].ToString();
            _aceLookUpURL = ConfigurationManager.AppSettings["ACELookUp"].ToString();
        }

        public APIBase(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public bool Disconnected
        {
            set { _Discon = value; }
            get { return _Discon; }
        }

        public string ReturnMessage
        {
            set { _RtnMsg = value; }
            get { return _RtnMsg; }
        }

        public string Signature
        {
            set { _Sign = value; }
            get { return _Sign; }
        }

        public string Username
        {
            set { _UserName = value; }
            get { return _UserName; }
        }

        public string Password
        {
            set { _Password = value; }
            get { return _Password; }
        }

        public string Domain
        {
            set { _Domain = value; }
            get { return _Domain; }
        }

        public string AceSessionURL
        {
            set { _aceSessionURL = value; }
            get { return _aceSessionURL; }
        }

        public string AceLookUpURL
        {
            set { _aceLookUpURL = value; }
            get { return _aceLookUpURL; }
        }

        public int ContractVersion
        {
            set { _Contract = value; }
            get { return _Contract; }
        }

        public bool APILogon()
        {
            try
            {
                ISessionManager sessionManager = new SessionManagerClient();
                LogonRequest logonRequest = new LogonRequest();
                

                logonRequest.logonRequestData = new LogonRequestData();
                logonRequest.logonRequestData.DomainCode = Domain;
                logonRequest.logonRequestData.AgentName = Username;
                logonRequest.logonRequestData.Password = Password;
                
                LogonResponse logonResponse = sessionManager.Logon(logonRequest);
                if (logonResponse != null && logonResponse.Signature.ToString() != string.Empty)
                {
                    _Sign = logonResponse.Signature;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                _RtnMsg = ex.Message;
                return false;
            }

        }

        public bool VerifySignature()
        {
            bool Logged;
            try
            {
                if (_Discon != true)
                {
                    if (_Sign != null && _Sign != string.Empty)
                    {
                        return true;
                    }
                    else
                    {
                        Logged = APILogon();
                        return Logged;
                    }
                }
                else
                {
                    _Sign = string.Empty;
                    return true;
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                _RtnMsg = ex.Message;
                log.Error(this,ex);
                return false;
            }
        }

        //public string GetXMLString(object Obj)
        //{
        //    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Obj.GetType());
        //    System.IO.StringWriter writer = new System.IO.StringWriter();
        //    x.Serialize(writer, Obj);

        //    return writer.ToString();
        //}

        public string baseAgentLogon(string AgentType = "public", string domain = "", string username = "", string password = "")
        {
            var profiler = MiniProfiler.Current;
            try
            {
                ISessionManager sessionManager = new SessionManagerClient();
                LogonRequest logonRequest = new LogonRequest();

                switch (AgentType.ToLower())
                {
                    case "public":
                        domain = Domain;
                        username = Username;
                        password = Password;
                        break;
                    case "skyagent":
                        break;
                }
                logonRequest.logonRequestData = new LogonRequestData();
                logonRequest.logonRequestData.DomainCode = domain;
                logonRequest.logonRequestData.AgentName = username;
                logonRequest.logonRequestData.Password = password;

                ContractVersion = 3413;
                logonRequest.ContractVersion = ContractVersion;

                //string requeststring = GetXMLString(logonRequest);

                LogonResponse logonResponse;// = sessionManager.Logon(logonRequest);
                using (profiler.Step("logonResponse"))
                {
                    logonResponse = sessionManager.Logon(logonRequest);
                }
                if (logonResponse != null && logonResponse.Signature.ToString() != string.Empty)
                {
                    return logonResponse.Signature;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                _RtnMsg = ex.Message;
                return string.Empty;
            }

        }

        public string baseAgentLogout(string signature = "")
        {
            var profiler = MiniProfiler.Current;
            try
            {
                ISessionManager sessionManager = new SessionManagerClient();
                LogoutRequest logoutRequest = new LogoutRequest();

                ContractVersion = 3413;
                logoutRequest.Signature = signature;
                logoutRequest.ContractVersion = ContractVersion;

                LogoutResponse logoutResponse;// = sessionManager.Logon(logonRequest);
                using (profiler.Step("logoutResponse"))
                {
                    logoutResponse = sessionManager.Logout(logoutRequest);
                }
                if (logoutResponse != null && logoutResponse.ToString() != string.Empty)
                {
                    return logoutResponse.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                _RtnMsg = ex.Message;
                return string.Empty;
            }

        }

    }
}