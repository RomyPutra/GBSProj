using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Data;
using SEAL.Model;
using SEAL.Data;
using SEAL.Security.AccessControl;

namespace ABS.Logic.Shared
{        
    public enum EnumAgentType
    {
        PublicAgent = 0,
        SkyAgent = 1
    }
    public enum EnumUserType
    {
        Normal = 0,
        Supervisor = 1,
        Manager = 2
    }

    public partial class CoreBase : SEAL.Model.Moyenne.CoreBase 
    {
        DataAccess objConn;
        public enum EnumSaveType
        {
            Insert = 0, 
            Update = 1
        }
        public CoreBase()
        {
            objConn = new DataAccess();
            //objConn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString();
			objConn.ConnectionString = "Data Source = 172.20.145.11; Initial Catalog = GBSPILOT; Persist Security Info = True; User ID = gbs; Password = p@ssw0rd; connection timeout = 60; Application Name = GBS";

			SetConnection(ref objConn);
            StartSQLControl();
        }
        //ConfigurationManager.ConnectionStrings["ConnStr"].ToString()
    }

    public class UserSet : SEAL.Security.AccessControl.UserSet
    {
        public string AgentID;
        public string AgentName;
        public string LoginName;
        public Shared.EnumAgentType AgentType;
        public int MaxEnquiry;
        public int MaxSuspend;
        public int SuspendDuration;
        public int BlacklistDuration;
        public int CounterTimer;
        public string AgentCategoryID;
        public string OperationGroup;
        public DateTime LastLogin;
        public string OrganizationName;
        public string OrganicationID;
        public string Email;
        public string Contact;
        public string AgLimit;
        public string Currency;
        public string Password;

    }

    public class AdminSet : SEAL.Security.AccessControl.UserSet
    {
        public string AdminID;
        public string AdminName;
        public string RefID;
        public byte RefType;
        public string GroupCode;
        public byte AccessLevel;
        public int AppID;
        public string GroupName;
        public string OperationGroup;
        public string OrganizationName;
    }
}
