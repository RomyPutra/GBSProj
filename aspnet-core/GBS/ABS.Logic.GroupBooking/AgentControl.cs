using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Data;
using SEAL.Data;
using ABS.GBS.Log;

namespace ABS.Logic.GroupBooking.Agent
{
    #region Model Containers
    public class AgentCategory
    {
        private string _agentCatgID = String.Empty;

        private string _agentCatgDesc = String.Empty;
        private int _maxEnquiry;
        private int _counterTimer;
        private int _maxSuspend;
        private int _suspendDuration;
        private int _blacklistDuration;
        private byte _status;
        private Guid _rowguid = Guid.Empty;
        private DateTime _syncCreate;
        private DateTime _syncLastUpd;
        private string _lastSyncBy = String.Empty;

        #region Public Properties
        public string AgentCatgID
        {
            get { return _agentCatgID; }
            set { _agentCatgID = value; }
        }
        public string AgentCatgDesc
        {
            get { return _agentCatgDesc; }
            set { _agentCatgDesc = value; }
        }

        public int MaxEnquiry
        {
            get { return _maxEnquiry; }
            set { _maxEnquiry = value; }
        }

        public int CounterTimer
        {
            get { return _counterTimer; }
            set { _counterTimer = value; }
        }

        public int MaxSuspend
        {
            get { return _maxSuspend; }
            set { _maxSuspend = value; }
        }

        public int SuspendDuration
        {
            get { return _suspendDuration; }
            set { _suspendDuration = value; }
        }

        public int BlacklistDuration
        {
            get { return _blacklistDuration; }
            set { _blacklistDuration = value; }
        }

        public byte Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public Guid rowguid
        {
            get { return _rowguid; }
            set { _rowguid = value; }
        }

        public DateTime SyncCreate
        {
            get { return _syncCreate; }
            set { _syncCreate = value; }
        }

        public DateTime SyncLastUpd
        {
            get { return _syncLastUpd; }
            set { _syncLastUpd = value; }
        }

        public string LastSyncBy
        {
            get { return _lastSyncBy; }
            set { _lastSyncBy = value; }
        }
        #endregion

    }

    public class USRAPP_Info
    {
        private string _userID = String.Empty;
        private int _appID;

        private string _accessCode = String.Empty;
        private byte _isInherit;
        private Guid _rowguid = Guid.Empty;
        private DateTime _syncCreate;
        private DateTime _syncLastUpd;
        private byte _isHost;
        private string _lastSyncBy = String.Empty;

        #region Public Properties
        public string UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }
        public int AppID
        {
            get { return _appID; }
            set { _appID = value; }
        }
        public string AccessCode
        {
            get { return _accessCode; }
            set { _accessCode = value; }
        }

        public byte IsInherit
        {
            get { return _isInherit; }
            set { _isInherit = value; }
        }

        public Guid rowguid
        {
            get { return _rowguid; }
            set { _rowguid = value; }
        }

        public DateTime SyncCreate
        {
            get { return _syncCreate; }
            set { _syncCreate = value; }
        }

        public DateTime SyncLastUpd
        {
            get { return _syncLastUpd; }
            set { _syncLastUpd = value; }
        }

        public byte IsHost
        {
            get { return _isHost; }
            set { _isHost = value; }
        }

        public string LastSyncBy
        {
            get { return _lastSyncBy; }
            set { _lastSyncBy = value; }
        }
        #endregion

    }

    public class USRPROFILE_Info
    {
        private string _userID = String.Empty;

        private string _userName = String.Empty;
        private string _password = String.Empty;
        private string _refID = String.Empty;
        private byte _refType;
        private string _parentID = String.Empty;
        private byte _status;
        private byte _logged;
        private string _logStation = String.Empty;
        private DateTime _lastLogin;
        private DateTime _lastLogout;
        private DateTime _createDate;
        private string _createBy = String.Empty;
        private DateTime _lastUpdate;
        private string _updateBy = String.Empty;
        private Guid _rowguid = Guid.Empty;
        private DateTime _syncCreate;
        private DateTime _syncLastUpd;
        private byte _isHost;
        private string _lastSyncBy = String.Empty;
        private string _syncCreateBy = String.Empty;
        private string _operationGroup = String.Empty;

        #region Public Properties
        public string UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string RefID
        {
            get { return _refID; }
            set { _refID = value; }
        }

        public byte RefType
        {
            get { return _refType; }
            set { _refType = value; }
        }

        public string ParentID
        {
            get { return _parentID; }
            set { _parentID = value; }
        }

        public byte Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public byte Logged
        {
            get { return _logged; }
            set { _logged = value; }
        }

        public string LogStation
        {
            get { return _logStation; }
            set { _logStation = value; }
        }

        public DateTime LastLogin
        {
            get { return _lastLogin; }
            set { _lastLogin = value; }
        }

        public DateTime LastLogout
        {
            get { return _lastLogout; }
            set { _lastLogout = value; }
        }

        public DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }

        public string CreateBy
        {
            get { return _createBy; }
            set { _createBy = value; }
        }

        public DateTime LastUpdate
        {
            get { return _lastUpdate; }
            set { _lastUpdate = value; }
        }

        public string UpdateBy
        {
            get { return _updateBy; }
            set { _updateBy = value; }
        }

        public Guid rowguid
        {
            get { return _rowguid; }
            set { _rowguid = value; }
        }

        public DateTime SyncCreate
        {
            get { return _syncCreate; }
            set { _syncCreate = value; }
        }

        public DateTime SyncLastUpd
        {
            get { return _syncLastUpd; }
            set { _syncLastUpd = value; }
        }

        public byte IsHost
        {
            get { return _isHost; }
            set { _isHost = value; }
        }

        public string LastSyncBy
        {
            get { return _lastSyncBy; }
            set { _lastSyncBy = value; }
        }

        public string SyncCreateBy
        {
            get { return _syncCreateBy; }
            set { _syncCreateBy = value; }
        }
        public string OperationGroup
        {
            get { return _operationGroup; }
            set { _operationGroup = value; }
        }
        #endregion

    }

    public partial class AgentBankInfo
    {
        private string _agentID = String.Empty;
        private string _bankName = String.Empty;
        private string _address1 = String.Empty;
        private string _address2 = String.Empty;
        private string _address3 = String.Empty;
        private string _country = String.Empty;
        private string _city = String.Empty;
        private string _state = String.Empty;
        private string _postcode = String.Empty;
        private string _accountName = String.Empty;
        private string _accountNo = String.Empty;
        private Guid _rowguid = Guid.Empty;
        private DateTime _syncCreate;
        private DateTime _syncLastUpd;
        private string _lastSyncBy = String.Empty;

        #region Public Properties
        public string AgentID
        {
            get { return _agentID; }
            set { _agentID = value; }
        }
        public string BankName
        {
            get { return _bankName; }
            set { _bankName = value; }
        }

        public string Address1
        {
            get { return _address1; }
            set { _address1 = value; }
        }

        public string Address2
        {
            get { return _address2; }
            set { _address2 = value; }
        }

        public string Address3
        {
            get { return _address3; }
            set { _address3 = value; }
        }

        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }

        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        public string State
        {
            get { return _state; }
            set { _state = value; }
        }

        public string Postcode
        {
            get { return _postcode; }
            set { _postcode = value; }
        }

        public string AccountName
        {
            get { return _accountName; }
            set { _accountName = value; }
        }

        public string AccountNo
        {
            get { return _accountNo; }
            set { _accountNo = value; }
        }

        public Guid rowguid
        {
            get { return _rowguid; }
            set { _rowguid = value; }
        }

        public DateTime SyncCreate
        {
            get { return _syncCreate; }
            set { _syncCreate = value; }
        }

        public DateTime SyncLastUpd
        {
            get { return _syncLastUpd; }
            set { _syncLastUpd = value; }
        }

        public string LastSyncBy
        {
            get { return _lastSyncBy; }
            set { _lastSyncBy = value; }
        }
        #endregion
    }

    public class AgentProfile
    {
        private string _agentID = String.Empty;

        private string _agentCatgID = String.Empty;
        private string _username = String.Empty;
        private string _licenseNo = String.Empty;
        private string _password = String.Empty;
        private string _address1 = String.Empty;
        private string _address2 = String.Empty;
        private string _address3 = String.Empty;
        private string _country = String.Empty;
        private string _city = String.Empty;
        private string _state = String.Empty;
        private string _postcode = String.Empty;
        private string _title = String.Empty;
        private string _contactFirstName = String.Empty;
        private string _contactLastName = String.Empty;
        private string _phoneNo = String.Empty;
        private string _mobileNo = String.Empty;
        private string _fax = String.Empty;
        private string _email = String.Empty;
        private DateTime _joinDate;
        private DateTime _lastModifyDate;
        private byte _status;
        private byte _flag;
        private Guid _rowguid = Guid.Empty;
        private DateTime _syncCreate;
        private DateTime _syncLastUpd;
        private string _lastSyncBy = String.Empty;
        private string _operationGroup = String.Empty;

        //20170411 - Sienny
        private string _orgID = String.Empty;
        private string _orgName = String.Empty;

        #region Public Properties
        public string AgentID
        {
            get { return _agentID; }
            set { _agentID = value; }
        }
        public string AgentCatgID
        {
            get { return _agentCatgID; }
            set { _agentCatgID = value; }
        }

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public string LicenseNo
        {
            get { return _licenseNo; }
            set { _licenseNo = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string Address1
        {
            get { return _address1; }
            set { _address1 = value; }
        }

        public string Address2
        {
            get { return _address2; }
            set { _address2 = value; }
        }
        public string Address3
        {
            get { return _address3; }
            set { _address3 = value; }
        }

        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }

        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        public string State
        {
            get { return _state; }
            set { _state = value; }
        }

        public string Postcode
        {
            get { return _postcode; }
            set { _postcode = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string ContactFirstName
        {
            get { return _contactFirstName; }
            set { _contactFirstName = value; }
        }

        public string ContactLastName
        {
            get { return _contactLastName; }
            set { _contactLastName = value; }
        }

        public string PhoneNo
        {
            get { return _phoneNo; }
            set { _phoneNo = value; }
        }

        public string MobileNo
        {
            get { return _mobileNo; }
            set { _mobileNo = value; }
        }

        public string Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public DateTime JoinDate
        {
            get { return _joinDate; }
            set { _joinDate = value; }
        }

        public DateTime LastModifyDate
        {
            get { return _lastModifyDate; }
            set { _lastModifyDate = value; }
        }

        public byte Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public byte Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }

        public Guid rowguid
        {
            get { return _rowguid; }
            set { _rowguid = value; }
        }

        public DateTime SyncCreate
        {
            get { return _syncCreate; }
            set { _syncCreate = value; }
        }

        public DateTime SyncLastUpd
        {
            get { return _syncLastUpd; }
            set { _syncLastUpd = value; }
        }

        public string LastSyncBy
        {
            get { return _lastSyncBy; }
            set { _lastSyncBy = value; }
        }

        public string OperationGroup
        {
            get { return _operationGroup; }
            set { _operationGroup = value; }
        }

        //20170411 - Sienny
        public string OrgID
        {
            get { return _orgID; }
            set { _orgID = value; }
        }

        public string OrgName
        {
            get { return _orgName; }
            set { _orgName = value; }
        }
        #endregion

    }

    public class AgentActivity
    {
        private string _agentID = String.Empty;

        private DateTime _lastLoginDate;
        private DateTime _lastFailedLoginDate;
        private int _lastFailedLoginAttempt;
        private int _totalFailedLoginAttempt;
        private DateTime _expiryBlacklistDate;
        private DateTime _lastSuspend;
        private DateTime _lastBlacklist;
        private Guid _rowguid = Guid.Empty;
        private DateTime _syncCreate;
        private DateTime _syncLastUpd;

        #region Public Properties
        public string AgentID
        {
            get { return _agentID; }
            set { _agentID = value; }
        }
        public DateTime LastLoginDate
        {
            get { return _lastLoginDate; }
            set { _lastLoginDate = value; }
        }

        public DateTime LastFailedLoginDate
        {
            get { return _lastFailedLoginDate; }
            set { _lastFailedLoginDate = value; }
        }

        public int LastFailedLoginAttempt
        {
            get { return _lastFailedLoginAttempt; }
            set { _lastFailedLoginAttempt = value; }
        }

        public int TotalFailedLoginAttempt
        {
            get { return _totalFailedLoginAttempt; }
            set { _totalFailedLoginAttempt = value; }
        }

        public DateTime ExpiryBlacklistDate
        {
            get { return _expiryBlacklistDate; }
            set { _expiryBlacklistDate = value; }
        }

        public DateTime LastSuspend
        {
            get { return _lastSuspend; }
            set { _lastSuspend = value; }
        }

        public DateTime LastBlacklist
        {
            get { return _lastBlacklist; }
            set { _lastBlacklist = value; }
        }

        public Guid rowguid
        {
            get { return _rowguid; }
            set { _rowguid = value; }
        }

        public DateTime SyncCreate
        {
            get { return _syncCreate; }
            set { _syncCreate = value; }
        }

        public DateTime SyncLastUpd
        {
            get { return _syncLastUpd; }
            set { _syncLastUpd = value; }
        }
        #endregion

    }

    public class AgentBlacklistLog
    {
        private string _blacklistID = String.Empty;
        private string _agentID = String.Empty;

        private DateTime _blacklistDate;
        private DateTime _blacklistExpiryDate;
        private byte _status;
        private Guid _rowguid = Guid.Empty;
        private DateTime _syncCreate;
        private DateTime _syncLastUpd;
        private string _lastSyncBy = String.Empty;
        private string _remark = String.Empty;

        #region Public Properties
        public string BlacklistID
        {
            get { return _blacklistID; }
            set { _blacklistID = value; }
        }
        public string AgentID
        {
            get { return _agentID; }
            set { _agentID = value; }
        }
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        public DateTime BlacklistDate
        {
            get { return _blacklistDate; }
            set { _blacklistDate = value; }
        }

        public DateTime BlacklistExpiryDate
        {
            get { return _blacklistExpiryDate; }
            set { _blacklistExpiryDate = value; }
        }

        public byte Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public Guid rowguid
        {
            get { return _rowguid; }
            set { _rowguid = value; }
        }

        public DateTime SyncCreate
        {
            get { return _syncCreate; }
            set { _syncCreate = value; }
        }

        public DateTime SyncLastUpd
        {
            get { return _syncLastUpd; }
            set { _syncLastUpd = value; }
        }

        public string LastSyncBy
        {
            get { return _lastSyncBy; }
            set { _lastSyncBy = value; }
        }
        #endregion

    }

    public class RequestApp
    {
        private string _reqID = String.Empty;

        private string _userID = String.Empty;
        private string _reqType = String.Empty;
        private string _transID = String.Empty;
        private DateTime _requestDate;
        private DateTime _expiryDate;
        private string _requestDesc = String.Empty;
        private string _userName = String.Empty;
        private string _remark = String.Empty;
        private string _approvedBy = String.Empty;
        private DateTime _approvedDate;
        private Guid _rowguid = Guid.Empty;
        private DateTime _syncCreate;
        private DateTime _syncLastUpd;
        private string _lastSyncBy = String.Empty;

        #region Public Properties
        public string ReqID
        {
            get { return _reqID; }
            set { _reqID = value; }
        }
        public string UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public string RequestDesc
        {
            get { return _requestDesc; }
            set { _requestDesc = value; }
        }

        public string ReqType
        {
            get { return _reqType; }
            set { _reqType = value; }
        }

        public string TransID
        {
            get { return _transID; }
            set { _transID = value; }
        }

        public DateTime RequestDate
        {
            get { return _requestDate; }
            set { _requestDate = value; }
        }

        public DateTime ExpiryDate
        {
            get { return _expiryDate; }
            set { _expiryDate = value; }
        }

        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

        public string ApprovedBy
        {
            get { return _approvedBy; }
            set { _approvedBy = value; }
        }

        public DateTime ApprovedDate
        {
            get { return _approvedDate; }
            set { _approvedDate = value; }
        }

        public Guid rowguid
        {
            get { return _rowguid; }
            set { _rowguid = value; }
        }

        public DateTime SyncCreate
        {
            get { return _syncCreate; }
            set { _syncCreate = value; }
        }

        public DateTime SyncLastUpd
        {
            get { return _syncLastUpd; }
            set { _syncLastUpd = value; }
        }

        public string LastSyncBy
        {
            get { return _lastSyncBy; }
            set { _lastSyncBy = value; }
        }
        #endregion

    }

    #endregion

    public partial class AdminProfileControl : Shared.CoreBase
    {
        public AdminProfileControl()
        {
        }
    }

    public partial class AgentProfileControl : Shared.CoreBase
    {
        SystemLog SystemLog = new SystemLog();
        public AgentProfileControl()
        {
        }

        public List<AgentProfile> GetAllAg_Email(string strEmail)
        {
            AgentProfile objAG_PROFILE_Info;
            List<AgentProfile> objListAG_PROFILE_Info = new List<AgentProfile>();
            DataTable dt;
            DateTime dateValue;
            String strSQL = string.Empty;
            string strFields = string.Empty;
            List<string> lstFields = new List<string>();
            try
            {
                lstFields.Add("AG_PROFILE.AgentID");
                lstFields.Add("AG_PROFILE.AgentCatgID");
                lstFields.Add("AG_PROFILE.Username");
                lstFields.Add("AG_PROFILE.LicenseNo");
                lstFields.Add("AG_PROFILE.Password");
                lstFields.Add("AG_PROFILE.Address1");
                lstFields.Add("AG_PROFILE.Address2");
                lstFields.Add("AG_PROFILE.Address3");
                lstFields.Add("AG_PROFILE.Country");
                lstFields.Add("AG_PROFILE.City");
                lstFields.Add("AG_PROFILE.State");
                lstFields.Add("AG_PROFILE.Postcode");
                lstFields.Add("AG_PROFILE.Title");
                lstFields.Add("AG_PROFILE.ContactFirstName");
                lstFields.Add("AG_PROFILE.ContactLastName");
                lstFields.Add("AG_PROFILE.PhoneNo");
                lstFields.Add("AG_PROFILE.MobileNo");
                lstFields.Add("AG_PROFILE.Fax");
                lstFields.Add("AG_PROFILE.Email");
                lstFields.Add("AG_PROFILE.JoinDate");
                lstFields.Add("AG_PROFILE.LastModifyDate");
                lstFields.Add("AG_PROFILE.Status");
                lstFields.Add("AG_PROFILE.Flag");
                lstFields.Add("AG_PROFILE.rowguid");
                lstFields.Add("AG_PROFILE.SyncCreate");
                lstFields.Add("AG_PROFILE.SyncLastUpd");
                lstFields.Add("AG_PROFILE.LastSyncBy");
                lstFields.Add("AG_PROFILE.OperationGroup");

                //20170411 - Sienny
                lstFields.Add("AG_PROFILE.OrgID");
                lstFields.Add("AG_PROFILE.OrgName");

                strFields = GetSqlFields(lstFields);
                strSQL = "SELECT " + strFields + " FROM AG_PROFILE WHERE Email='" + strEmail + "'";
                dt = objDCom.Execute(strSQL, CommandType.Text, true);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow drRow in dt.Rows)
                    {
                        objAG_PROFILE_Info = new AgentProfile();
                        objAG_PROFILE_Info.AgentID = (string)drRow["AgentID"];
                        objAG_PROFILE_Info.AgentCatgID = (string)drRow["AgentCatgID"];
                        objAG_PROFILE_Info.Username = (string)drRow["Username"];
                        objAG_PROFILE_Info.LicenseNo = (string)drRow["LicenseNo"];
                        objAG_PROFILE_Info.Password = (string)drRow["Password"];
                        objAG_PROFILE_Info.Address1 = (string)drRow["Address1"];
                        objAG_PROFILE_Info.Address2 = (string)drRow["Address2"];
                        objAG_PROFILE_Info.Address3 = (string)drRow["Address3"];
                        objAG_PROFILE_Info.Country = (string)drRow["Country"];
                        objAG_PROFILE_Info.City = (string)drRow["City"];
                        objAG_PROFILE_Info.State = (string)drRow["State"];
                        objAG_PROFILE_Info.Postcode = (string)drRow["Postcode"];
                        objAG_PROFILE_Info.Title = (string)drRow["Title"];
                        objAG_PROFILE_Info.ContactFirstName = (string)drRow["ContactFirstName"];
                        objAG_PROFILE_Info.ContactLastName = (string)drRow["ContactLastName"];
                        objAG_PROFILE_Info.PhoneNo = (string)drRow["PhoneNo"];
                        objAG_PROFILE_Info.MobileNo = (string)drRow["MobileNo"];
                        objAG_PROFILE_Info.Fax = (string)drRow["Fax"];
                        objAG_PROFILE_Info.Email = (string)drRow["Email"];
                        if (DateTime.TryParse(drRow["JoinDate"].ToString(), out dateValue)) objAG_PROFILE_Info.JoinDate = (DateTime)drRow["JoinDate"];
                        if (DateTime.TryParse(drRow["LastModifyDate"].ToString(), out dateValue)) objAG_PROFILE_Info.LastModifyDate = (DateTime)drRow["LastModifyDate"];
                        objAG_PROFILE_Info.Status = (byte)drRow["Status"];
                        objAG_PROFILE_Info.Flag = (byte)drRow["Flag"];
                        objAG_PROFILE_Info.rowguid = (Guid)drRow["rowguid"];
                        if (DateTime.TryParse(drRow["SyncCreate"].ToString(), out dateValue)) objAG_PROFILE_Info.SyncCreate = (DateTime)drRow["SyncCreate"];
                        if (DateTime.TryParse(drRow["SyncLastUpd"].ToString(), out dateValue)) objAG_PROFILE_Info.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
                        objAG_PROFILE_Info.LastSyncBy = (string)drRow["LastSyncBy"];
                        objAG_PROFILE_Info.OperationGroup = (string)drRow["OperationGroup"];

                        //20170411 - Sienny
                        objAG_PROFILE_Info.OrgID = (string)drRow["OrgID"];
                        objAG_PROFILE_Info.OrgName = (string)drRow["OrgName"];

                        objListAG_PROFILE_Info.Add(objAG_PROFILE_Info);
                    }
                    return objListAG_PROFILE_Info;
                }
                else
                {
                    return null;
                    throw new ApplicationException("Email does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }

        public List<AgentProfile> GetAllUsername(string strUser)
        {
            AgentProfile objAG_PROFILE_Info;
            List<AgentProfile> objListAG_PROFILE_Info = new List<AgentProfile>();
            DataTable dt;
            DateTime dateValue;
            String strSQL = string.Empty;
            //String strFields = string.Empty;
            string strFields = string.Empty;
            List<string> lstFields = new List<string>();
            try
            {
                lstFields.Add("AG_PROFILE.AgentID");
                lstFields.Add("AG_PROFILE.AgentCatgID");
                lstFields.Add("AG_PROFILE.Username");
                lstFields.Add("AG_PROFILE.LicenseNo");
                lstFields.Add("AG_PROFILE.Password");
                lstFields.Add("AG_PROFILE.Address1");
                lstFields.Add("AG_PROFILE.Address2");
                lstFields.Add("AG_PROFILE.Address3");
                lstFields.Add("AG_PROFILE.Country");
                lstFields.Add("AG_PROFILE.City");
                lstFields.Add("AG_PROFILE.State");
                lstFields.Add("AG_PROFILE.Postcode");
                lstFields.Add("AG_PROFILE.Title");
                lstFields.Add("AG_PROFILE.ContactFirstName");
                lstFields.Add("AG_PROFILE.ContactLastName");
                lstFields.Add("AG_PROFILE.PhoneNo");
                lstFields.Add("AG_PROFILE.MobileNo");
                lstFields.Add("AG_PROFILE.Fax");
                lstFields.Add("AG_PROFILE.Email");
                lstFields.Add("AG_PROFILE.JoinDate");
                lstFields.Add("AG_PROFILE.LastModifyDate");
                lstFields.Add("AG_PROFILE.Status");
                lstFields.Add("AG_PROFILE.Flag");
                lstFields.Add("AG_PROFILE.rowguid");
                lstFields.Add("AG_PROFILE.SyncCreate");
                lstFields.Add("AG_PROFILE.SyncLastUpd");
                lstFields.Add("AG_PROFILE.LastSyncBy");
                lstFields.Add("AG_PROFILE.OperationGroup");

                //20170411 - Sienny
                lstFields.Add("AG_PROFILE.OrgID");
                lstFields.Add("AG_PROFILE.OrgName");

                strFields = GetSqlFields(lstFields);

                strSQL = "SELECT " + strFields + " FROM AG_PROFILE WHERE Username='" + strUser + "'";
                dt = objDCom.Execute(strSQL, CommandType.Text, true);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow drRow in dt.Rows)
                    {
                        objAG_PROFILE_Info = new AgentProfile();
                        objAG_PROFILE_Info.AgentID = (string)drRow["AgentID"];
                        objAG_PROFILE_Info.AgentCatgID = (string)drRow["AgentCatgID"];
                        objAG_PROFILE_Info.Username = (string)drRow["Username"];
                        objAG_PROFILE_Info.LicenseNo = (string)drRow["LicenseNo"];
                        objAG_PROFILE_Info.Password = (string)drRow["Password"];
                        objAG_PROFILE_Info.Address1 = (string)drRow["Address1"];
                        objAG_PROFILE_Info.Address2 = (string)drRow["Address2"];
                        objAG_PROFILE_Info.Address3 = (string)drRow["Address3"];
                        objAG_PROFILE_Info.Country = (string)drRow["Country"];
                        objAG_PROFILE_Info.City = (string)drRow["City"];
                        objAG_PROFILE_Info.State = (string)drRow["State"];
                        objAG_PROFILE_Info.Postcode = (string)drRow["Postcode"];
                        objAG_PROFILE_Info.Title = (string)drRow["Title"];
                        objAG_PROFILE_Info.ContactFirstName = (string)drRow["ContactFirstName"];
                        objAG_PROFILE_Info.ContactLastName = (string)drRow["ContactLastName"];
                        objAG_PROFILE_Info.PhoneNo = (string)drRow["PhoneNo"];
                        objAG_PROFILE_Info.MobileNo = (string)drRow["MobileNo"];
                        objAG_PROFILE_Info.Fax = (string)drRow["Fax"];
                        objAG_PROFILE_Info.Email = (string)drRow["Email"];
                        if (DateTime.TryParse(drRow["JoinDate"].ToString(), out dateValue)) objAG_PROFILE_Info.JoinDate = (DateTime)drRow["JoinDate"];
                        if (DateTime.TryParse(drRow["LastModifyDate"].ToString(), out dateValue)) objAG_PROFILE_Info.LastModifyDate = (DateTime)drRow["LastModifyDate"];
                        objAG_PROFILE_Info.Status = (byte)drRow["Status"];
                        objAG_PROFILE_Info.Flag = (byte)drRow["Flag"];
                        objAG_PROFILE_Info.rowguid = (Guid)drRow["rowguid"];
                        if (DateTime.TryParse(drRow["SyncCreate"].ToString(), out dateValue)) objAG_PROFILE_Info.SyncCreate = (DateTime)drRow["SyncCreate"];
                        if (DateTime.TryParse(drRow["SyncLastUpd"].ToString(), out dateValue)) objAG_PROFILE_Info.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
                        objAG_PROFILE_Info.LastSyncBy = (string)drRow["LastSyncBy"];
                        objAG_PROFILE_Info.OperationGroup = (string)drRow["OperationGroup"];

                        //20170411 - Sienny
                        objAG_PROFILE_Info.OrgID = (string)drRow["OrgID"];
                        objAG_PROFILE_Info.OrgName = (string)drRow["OrgName"];

                        objListAG_PROFILE_Info.Add(objAG_PROFILE_Info);
                    }
                    return objListAG_PROFILE_Info;
                }
                else
                {
                    return null;
                    throw new ApplicationException("Username does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }

        //public DataTable GetBlacklistAgent()
        //{
        //    DataTable dt = new DataTable();
        //    String strSQL = string.Empty;
        //    try
        //    {
        //        strSQL = "SELECT ap.AgentID, ab.BlacklistDate,ab.BlacklistExpiryDate,ap.Username, ap.ContactFirstName,ap.ContactLastName,ap.Email,ap.Country FROM AG_PROFILE ap (NOLOCK) JOIN AG_BLACKLIST ab (NOLOCK) ON ap.AGENTID = ab.AGENTID WHERE ab.BlacklistExpiryDate >= GETDATE() and ap.flag = 0";
        //        dt = objDCom.Execute(strSQL, CommandType.Text, true);
        //        return dt;
        //    }
        //    catch
        //    {
        //        return dt;
        //    }

        //}

        private static String GetSqlFields(List<string> Fields)
        {
            String strFields = string.Empty;
            if (Fields != null)
            {
                foreach (string sField in Fields)
                {
                    if (strFields == string.Empty)
                    {
                        strFields = sField;
                    }
                    else
                    {
                        strFields += ", " + sField;
                    }
                }
            }
            return strFields;
        }

        public List<AgentProfile> GetAllAgentProfile()
        {
            AgentProfile objAG_PROFILE_Info;
            List<AgentProfile> objListAG_PROFILE_Info = new List<AgentProfile>();
            DataTable dt;
            DateTime dateValue;
            String strSQL = string.Empty;

            try
            {
                strSQL = "SELECT * FROM AG_PROFILE ";
                dt = objDCom.Execute(strSQL, CommandType.Text, true);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow drRow in dt.Rows)
                    {
                        objAG_PROFILE_Info = new AgentProfile();
                        objAG_PROFILE_Info.AgentID = (string)drRow["AgentID"];
                        objAG_PROFILE_Info.AgentCatgID = (string)drRow["AgentCatgID"];
                        objAG_PROFILE_Info.Username = (string)drRow["Username"];
                        objAG_PROFILE_Info.LicenseNo = (string)drRow["LicenseNo"];
                        objAG_PROFILE_Info.Password = (string)drRow["Password"];
                        objAG_PROFILE_Info.Address1 = (string)drRow["Address1"];
                        objAG_PROFILE_Info.Address2 = (string)drRow["Address2"];
                        objAG_PROFILE_Info.Address3 = (string)drRow["Address3"];
                        objAG_PROFILE_Info.Country = (string)drRow["Country"];
                        objAG_PROFILE_Info.City = (string)drRow["City"];
                        objAG_PROFILE_Info.State = (string)drRow["State"];
                        objAG_PROFILE_Info.Postcode = (string)drRow["Postcode"];
                        objAG_PROFILE_Info.Title = (string)drRow["Title"];
                        objAG_PROFILE_Info.ContactFirstName = (string)drRow["ContactFirstName"];
                        objAG_PROFILE_Info.ContactLastName = (string)drRow["ContactLastName"];
                        objAG_PROFILE_Info.PhoneNo = (string)drRow["PhoneNo"];
                        objAG_PROFILE_Info.MobileNo = (string)drRow["MobileNo"];
                        objAG_PROFILE_Info.Fax = (string)drRow["Fax"];
                        objAG_PROFILE_Info.Email = (string)drRow["Email"];
                        if (DateTime.TryParse(drRow["JoinDate"].ToString(), out dateValue)) objAG_PROFILE_Info.JoinDate = (DateTime)drRow["JoinDate"];
                        if (DateTime.TryParse(drRow["LastModifyDate"].ToString(), out dateValue)) objAG_PROFILE_Info.LastModifyDate = (DateTime)drRow["LastModifyDate"];
                        objAG_PROFILE_Info.Status = (byte)drRow["Status"];
                        objAG_PROFILE_Info.Flag = (byte)drRow["Flag"];
                        objAG_PROFILE_Info.rowguid = (Guid)drRow["rowguid"];
                        if (DateTime.TryParse(drRow["SyncCreate"].ToString(), out dateValue)) objAG_PROFILE_Info.SyncCreate = (DateTime)drRow["SyncCreate"];
                        if (DateTime.TryParse(drRow["SyncLastUpd"].ToString(), out dateValue)) objAG_PROFILE_Info.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
                        objAG_PROFILE_Info.LastSyncBy = (string)drRow["LastSyncBy"];
                        objAG_PROFILE_Info.OperationGroup = (string)drRow["OperationGroup"];

                        //20170411 - Sienny
                        objAG_PROFILE_Info.OrgID = (string)drRow["OrgID"];
                        objAG_PROFILE_Info.OrgName = (string)drRow["OrgName"];

                        objListAG_PROFILE_Info.Add(objAG_PROFILE_Info);
                    }
                    return objListAG_PROFILE_Info;
                }
                else
                {
                    return null;
                    throw new ApplicationException("AG_PROFILE does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }

        public AgentProfile GetSingleAgentProfile(string username, string agentId = "")
        {
            AgentProfile objAG_PROFILE_Info;
            DataTable dt;
            DateTime dateValue;
            String strSQL = string.Empty;
            String strFields = string.Empty;
            String strFilter = string.Empty;
            List<string> lstFields = new List<string>();
            try
            {
                lstFields.Add("AG_PROFILE.AgentID");
                lstFields.Add("AG_PROFILE.AgentCatgID");
                lstFields.Add("AG_PROFILE.Username");
                lstFields.Add("AG_PROFILE.LicenseNo");
                lstFields.Add("AG_PROFILE.Password");
                lstFields.Add("AG_PROFILE.Address1");
                lstFields.Add("AG_PROFILE.Address2");
                lstFields.Add("AG_PROFILE.Address3");
                lstFields.Add("AG_PROFILE.Country");
                lstFields.Add("AG_PROFILE.City");
                lstFields.Add("AG_PROFILE.State");
                lstFields.Add("AG_PROFILE.Postcode");
                lstFields.Add("AG_PROFILE.Title");
                lstFields.Add("AG_PROFILE.ContactFirstName");
                lstFields.Add("AG_PROFILE.ContactLastName");
                lstFields.Add("AG_PROFILE.PhoneNo");
                lstFields.Add("AG_PROFILE.MobileNo");
                lstFields.Add("AG_PROFILE.Fax");
                lstFields.Add("AG_PROFILE.Email");
                lstFields.Add("AG_PROFILE.JoinDate");
                lstFields.Add("AG_PROFILE.LastModifyDate");
                lstFields.Add("AG_PROFILE.Status");
                lstFields.Add("AG_PROFILE.Flag");
                lstFields.Add("AG_PROFILE.rowguid");
                lstFields.Add("AG_PROFILE.SyncCreate");
                lstFields.Add("AG_PROFILE.SyncLastUpd");
                lstFields.Add("AG_PROFILE.LastSyncBy");
                lstFields.Add("AG_PROFILE.OperationGroup");

                //20170411 - Sienny
                lstFields.Add("AG_PROFILE.OrgID");
                lstFields.Add("AG_PROFILE.OrgName");

                strFields = GetSqlFields(lstFields);
                //strFilter = "WHERE AG_PROFILE.Username='" + username + "' and AG_profile.Status=1 and AG_PROFILE.flag = 0 ";
                strFilter = "WHERE AG_PROFILE.Username='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, username) + "' and AG_PROFILE.flag = 0 ";
                if (agentId.Trim() != "")
                {
                    strFilter = "WHERE AG_PROFILE.AgentId='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, agentId) + "' and AG_PROFILE.flag = 0 ";
                    //strFilter = "WHERE AG_PROFILE.AgentId='" + agentId + "' and AG_profile.Status=1 and AG_PROFILE.flag = 0 ";
                }
                strSQL = "SELECT " + strFields + " FROM AG_PROFILE WITH (NOLOCK) " + strFilter;
                dt = objDCom.Execute(strSQL, CommandType.Text, true);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow drRow = dt.Rows[0];

                    objAG_PROFILE_Info = new AgentProfile();
                    objAG_PROFILE_Info.AgentID = (string)drRow["AgentID"];
                    objAG_PROFILE_Info.AgentCatgID = (string)drRow["AgentCatgID"];
                    objAG_PROFILE_Info.Username = (string)drRow["Username"];
                    objAG_PROFILE_Info.LicenseNo = (string)drRow["LicenseNo"];
                    objAG_PROFILE_Info.Password = (string)drRow["Password"];
                    objAG_PROFILE_Info.Address1 = (string)drRow["Address1"];
                    objAG_PROFILE_Info.Address2 = (string)drRow["Address2"];
                    objAG_PROFILE_Info.Address3 = (string)drRow["Address3"];
                    objAG_PROFILE_Info.Country = (string)drRow["Country"];
                    objAG_PROFILE_Info.City = (string)drRow["City"];
                    objAG_PROFILE_Info.State = (string)drRow["State"];
                    objAG_PROFILE_Info.Postcode = (string)drRow["Postcode"];
                    objAG_PROFILE_Info.Title = (string)drRow["Title"];
                    objAG_PROFILE_Info.ContactFirstName = (string)drRow["ContactFirstName"];
                    objAG_PROFILE_Info.ContactLastName = (string)drRow["ContactLastName"];
                    objAG_PROFILE_Info.PhoneNo = (string)drRow["PhoneNo"];
                    objAG_PROFILE_Info.MobileNo = (string)drRow["MobileNo"];
                    objAG_PROFILE_Info.Fax = (string)drRow["Fax"];
                    objAG_PROFILE_Info.Email = (string)drRow["Email"];
                    objAG_PROFILE_Info.OperationGroup = (string)drRow["OperationGroup"];
                    if (DateTime.TryParse(drRow["JoinDate"].ToString(), out dateValue)) objAG_PROFILE_Info.JoinDate = (DateTime)drRow["JoinDate"];
                    if (DateTime.TryParse(drRow["LastModifyDate"].ToString(), out dateValue)) objAG_PROFILE_Info.LastModifyDate = (DateTime)drRow["LastModifyDate"];
                    objAG_PROFILE_Info.Status = (byte)drRow["Status"];
                    objAG_PROFILE_Info.Flag = (byte)drRow["Flag"];
                    objAG_PROFILE_Info.rowguid = (Guid)drRow["rowguid"];
                    if (DateTime.TryParse(drRow["SyncCreate"].ToString(), out dateValue)) objAG_PROFILE_Info.SyncCreate = (DateTime)drRow["SyncCreate"];
                    if (DateTime.TryParse(drRow["SyncLastUpd"].ToString(), out dateValue)) objAG_PROFILE_Info.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
                    objAG_PROFILE_Info.LastSyncBy = (string)drRow["LastSyncBy"];
                    objAG_PROFILE_Info.OperationGroup = (string)drRow["OperationGroup"];

                    //20170411 - Sienny
                    objAG_PROFILE_Info.OrgID = (string)drRow["OrgID"];
                    objAG_PROFILE_Info.OrgName = (string)drRow["OrgName"];

                    return objAG_PROFILE_Info;
                }
                else
                {
                    return null;
                    throw new ApplicationException("AG_PROFILE does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }

        public AgentProfile GetSingleAgentProfileByID(string AgentID)
        {
            AgentProfile objAG_PROFILE_Info;
            DataTable dt;
            DateTime dateValue;
            String strSQL = string.Empty;
            String strFields = string.Empty;
            String strFilter = string.Empty;
            List<string> lstFields = new List<string>();
            try
            {
                lstFields.Add("AG_PROFILE.AgentID");
                lstFields.Add("AG_PROFILE.AgentCatgID");
                lstFields.Add("AG_PROFILE.Username");
                lstFields.Add("AG_PROFILE.LicenseNo");
                lstFields.Add("AG_PROFILE.Password");
                lstFields.Add("AG_PROFILE.Address1");
                lstFields.Add("AG_PROFILE.Address2");
                lstFields.Add("AG_PROFILE.Address3");
                lstFields.Add("AG_PROFILE.Country");
                lstFields.Add("AG_PROFILE.City");
                lstFields.Add("AG_PROFILE.State");
                lstFields.Add("AG_PROFILE.Postcode");
                lstFields.Add("AG_PROFILE.Title");
                lstFields.Add("AG_PROFILE.ContactFirstName");
                lstFields.Add("AG_PROFILE.ContactLastName");
                lstFields.Add("AG_PROFILE.PhoneNo");
                lstFields.Add("AG_PROFILE.MobileNo");
                lstFields.Add("AG_PROFILE.Fax");
                lstFields.Add("AG_PROFILE.Email");
                lstFields.Add("AG_PROFILE.JoinDate");
                lstFields.Add("AG_PROFILE.LastModifyDate");
                lstFields.Add("AG_PROFILE.Status");
                lstFields.Add("AG_PROFILE.Flag");
                lstFields.Add("AG_PROFILE.rowguid");
                lstFields.Add("AG_PROFILE.SyncCreate");
                lstFields.Add("AG_PROFILE.SyncLastUpd");
                lstFields.Add("AG_PROFILE.LastSyncBy");
                lstFields.Add("AG_PROFILE.OperationGroup");

                //20170411 - Sienny
                lstFields.Add("AG_PROFILE.OrgID");
                lstFields.Add("AG_PROFILE.OrgName");

                strFields = GetSqlFields(lstFields);
                strFilter = "WHERE AG_PROFILE.AgentID='" + AgentID + "' and AG_PROFILE.flag = 0 ";
                strSQL = "SELECT " + strFields + " FROM AG_PROFILE " + strFilter;
                dt = objDCom.Execute(strSQL, CommandType.Text, true);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow drRow = dt.Rows[0];

                    objAG_PROFILE_Info = new AgentProfile();
                    objAG_PROFILE_Info.AgentID = (string)drRow["AgentID"];
                    objAG_PROFILE_Info.AgentCatgID = (string)drRow["AgentCatgID"];
                    objAG_PROFILE_Info.Username = (string)drRow["Username"];
                    objAG_PROFILE_Info.LicenseNo = (string)drRow["LicenseNo"];
                    objAG_PROFILE_Info.Password = (string)drRow["Password"];
                    objAG_PROFILE_Info.Address1 = (string)drRow["Address1"];
                    objAG_PROFILE_Info.Address2 = (string)drRow["Address2"];
                    objAG_PROFILE_Info.Address3 = (string)drRow["Address3"];
                    objAG_PROFILE_Info.Country = (string)drRow["Country"];
                    objAG_PROFILE_Info.City = (string)drRow["City"];
                    objAG_PROFILE_Info.State = (string)drRow["State"];
                    objAG_PROFILE_Info.Postcode = (string)drRow["Postcode"];
                    objAG_PROFILE_Info.Title = (string)drRow["Title"];
                    objAG_PROFILE_Info.ContactFirstName = (string)drRow["ContactFirstName"];
                    objAG_PROFILE_Info.ContactLastName = (string)drRow["ContactLastName"];
                    objAG_PROFILE_Info.PhoneNo = (string)drRow["PhoneNo"];
                    objAG_PROFILE_Info.MobileNo = (string)drRow["MobileNo"];
                    objAG_PROFILE_Info.Fax = (string)drRow["Fax"];
                    objAG_PROFILE_Info.OperationGroup = (string)drRow["OperationGroup"];
                    objAG_PROFILE_Info.Email = (string)drRow["Email"];
                    if (DateTime.TryParse(drRow["JoinDate"].ToString(), out dateValue)) objAG_PROFILE_Info.JoinDate = (DateTime)drRow["JoinDate"];
                    if (DateTime.TryParse(drRow["LastModifyDate"].ToString(), out dateValue)) objAG_PROFILE_Info.LastModifyDate = (DateTime)drRow["LastModifyDate"];
                    objAG_PROFILE_Info.Status = (byte)drRow["Status"];
                    objAG_PROFILE_Info.Flag = (byte)drRow["Flag"];
                    objAG_PROFILE_Info.rowguid = (Guid)drRow["rowguid"];
                    if (DateTime.TryParse(drRow["SyncCreate"].ToString(), out dateValue)) objAG_PROFILE_Info.SyncCreate = (DateTime)drRow["SyncCreate"];
                    if (DateTime.TryParse(drRow["SyncLastUpd"].ToString(), out dateValue)) objAG_PROFILE_Info.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
                    objAG_PROFILE_Info.LastSyncBy = (string)drRow["LastSyncBy"];
                    objAG_PROFILE_Info.OperationGroup = (string)drRow["OperationGroup"];

                    //20170411 - Sienny
                    objAG_PROFILE_Info.OrgID = (string)drRow["OrgID"];
                    objAG_PROFILE_Info.OrgName = (string)drRow["OrgName"];

                    return objAG_PROFILE_Info;
                }
                else
                {
                    return null;
                    throw new ApplicationException("AG_PROFILE does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }

        public int CheckRecordExist(string username, string agentId = "")
        {
            DataTable dt;
            String strSQL = string.Empty;
            String strFields = string.Empty;
            String strFilter = string.Empty;
            List<string> lstFields = new List<string>();
            try
            {

                strFilter = "WHERE AG_PROFILE.Username='" + username + "' and AG_PROFILE.flag = 0";
                if (agentId.Trim() != "")
                {
                    strFilter = "WHERE AG_PROFILE.agentid='" + agentId.Trim() + "' and AG_PROFILE.flag = 0";
                }

                strSQL = "SELECT AG_PROFILE.AgentID FROM AG_PROFILE  WITH (NOLOCK)  " + strFilter;



                dt = objDCom.Execute(strSQL, CommandType.Text, true);

                if (dt != null && dt.Rows.Count > 0)
                {
                    //strFilter = "WHERE AG_PROFILE.flag = 0 and AG_PROFILE.Username='" + username + "' and AG_PROFILE.Password = '" + pPassword + "'";
                    //strSQL = "SELECT AG_PROFILE.AgentID FROM AG_PROFILE " + strFilter;
                    dt = objDCom.Execute(strSQL, CommandType.Text, true);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return 102; // invalid password
                        throw new ApplicationException("Invalid Password");
                    }
                }
                else
                {
                    return 101; // username not exist
                    throw new ApplicationException("AG_PROFILE does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return 105;
            }
        }

        public int CheckUsernamePasswordExist(string username, string password)
        {
            DataTable dt;
            String strSQL = string.Empty;
            String strFields = string.Empty;
            String strFilter = string.Empty;
            List<string> lstFields = new List<string>();
            try
            {

                strFilter = "WHERE AG_PROFILE.Username='" + username + "' and AG_PROFILE.Password='" + password + "' and AG_PROFILE.flag = 0";
                strSQL = "SELECT AG_PROFILE.AgentID FROM AG_PROFILE " + strFilter;
                dt = objDCom.Execute(strSQL, CommandType.Text, true);

                if (dt != null && dt.Rows.Count > 0)
                {
                    return 1;
                }
                else
                {
                    return 101; // username not exist
                    //throw new ApplicationException("AG_PROFILE does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                return 105;
            }
        }

        public AgentProfile SaveAgentProfile(AgentProfile AgentProfile, AgentBankInfo AgentBankInfo, EnumSaveType SaveType)
        {
            bool rValue = false;
            ArrayList lstSQL = new ArrayList();
            objSQL.ClearFields();
            objSQL.ClearCondtions();
            string strSQL = string.Empty;
            try
            {
                
                objSQL.AddField("AgentID", AgentProfile.AgentID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("AgentCatgID", AgentProfile.AgentCatgID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Username", AgentProfile.Username, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty);
                objSQL.AddField("LicenseNo", AgentProfile.LicenseNo, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty);
                objSQL.AddField("Password", AgentProfile.Password, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty);
                objSQL.AddField("Address1", AgentProfile.Address1, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty);
                objSQL.AddField("Address2", AgentProfile.Address2, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty);
                objSQL.AddField("Address3", AgentProfile.Address3, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty);
                objSQL.AddField("Country", AgentProfile.Country, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty);
                objSQL.AddField("City", AgentProfile.City, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty);
                objSQL.AddField("State", AgentProfile.State, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty);
                objSQL.AddField("Postcode", AgentProfile.Postcode, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty);
                objSQL.AddField("Title", AgentProfile.Title, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty);
                objSQL.AddField("ContactFirstName", AgentProfile.ContactFirstName, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty);
                objSQL.AddField("ContactLastName", AgentProfile.ContactLastName, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty);
                objSQL.AddField("PhoneNo", AgentProfile.PhoneNo, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty);
                objSQL.AddField("MobileNo", AgentProfile.MobileNo, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty);
                objSQL.AddField("Fax", AgentProfile.Fax, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty);
                objSQL.AddField("Email", AgentProfile.Email, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty);
                objSQL.AddField("JoinDate", AgentProfile.JoinDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastModifyDate", AgentProfile.LastModifyDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Status", AgentProfile.Status, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Flag", AgentProfile.Flag, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncCreate", AgentProfile.SyncCreate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncLastUpd", AgentProfile.SyncLastUpd, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastSyncBy", AgentProfile.LastSyncBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("OperationGroup", AgentProfile.OperationGroup, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);

                //20170411 - Sienny
                objSQL.AddField("OrgID", AgentProfile.OrgID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("OrgName", AgentProfile.OrgName, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);

                switch (SaveType)
                {
                    case EnumSaveType.Insert:
                        if (GetSingleAgentProfileByID(AgentProfile.AgentID) != null)
                        {
                            strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "AG_PROFILE", "AG_PROFILE.AgentID='" + AgentProfile.AgentID + "'"); 
                        }
                        else
                        {
                            strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert, "AG_PROFILE", string.Empty);
                        }
                        break;
                    case EnumSaveType.Update:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "AG_PROFILE", "AG_PROFILE.AgentID='" + AgentProfile.AgentID + "'");
                        break;
                }
                lstSQL.Add(strSQL);

                //add to save bank info;
                if (AgentBankInfo != null)
                {
                    objSQL.AddField("AgentID", AgentBankInfo.AgentID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("BankName", AgentBankInfo.BankName, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty);
                    objSQL.AddField("Address1", AgentBankInfo.Address1, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty);
                    objSQL.AddField("Address2", AgentBankInfo.Address2, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty);
                    objSQL.AddField("Address3", AgentBankInfo.Address3, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty);
                    objSQL.AddField("Country", AgentBankInfo.Country, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty);
                    objSQL.AddField("City", AgentBankInfo.City, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty);
                    objSQL.AddField("State", AgentBankInfo.State, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty);
                    objSQL.AddField("Postcode", AgentBankInfo.Postcode, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty);
                    objSQL.AddField("AccountName", AgentBankInfo.AccountName, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty);
                    objSQL.AddField("AccountNo", AgentBankInfo.AccountNo, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty);
                    objSQL.AddField("SyncCreate", AgentBankInfo.SyncCreate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("SyncLastUpd", AgentBankInfo.SyncLastUpd, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("LastSyncBy", AgentBankInfo.LastSyncBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    switch (SaveType)
                    {
                        case EnumSaveType.Insert:
                            strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert, "AG_BANKINFO", string.Empty);
                            break;
                        case EnumSaveType.Update:
                            strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "AG_BANKINFO", "AG_BANKINFO.AgentID='" + AgentBankInfo.AgentID + "'");
                            break;
                    }
                    lstSQL.Add(strSQL);
                }

                rValue = objDCom.BatchExecute(lstSQL, CommandType.Text, true, false);
                if (rValue == false)
                {
                    return null;
                }
                return GetSingleAgentProfileByID(AgentProfile.AgentID);

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                return null;
            }
        }

        public string SaveUserProfile(USRPROFILE_Info pUSRPROFILE_Info, USRAPP_Info pUSRAPP_Info, EnumSaveType SaveType)
        {
            bool rValue = false;
            ArrayList lstSQL = new ArrayList();
            objSQL.ClearFields();
            objSQL.ClearCondtions();
            string strSQL = string.Empty;
            try
            {
                objSQL.AddField("UserID", pUSRPROFILE_Info.UserID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("UserName", pUSRPROFILE_Info.UserName, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Password", pUSRPROFILE_Info.Password, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("RefID", pUSRPROFILE_Info.RefID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("RefType", pUSRPROFILE_Info.RefType, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("ParentID", pUSRPROFILE_Info.ParentID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Status", pUSRPROFILE_Info.Status, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Logged", pUSRPROFILE_Info.Logged, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LogStation", pUSRPROFILE_Info.LogStation, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastLogin", pUSRPROFILE_Info.LastLogin, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastLogout", pUSRPROFILE_Info.LastLogout, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("CreateDate", pUSRPROFILE_Info.CreateDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("CreateBy", pUSRPROFILE_Info.CreateBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastUpdate", pUSRPROFILE_Info.LastUpdate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("UpdateBy", pUSRPROFILE_Info.UpdateBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncCreate", pUSRPROFILE_Info.SyncCreate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncLastUpd", pUSRPROFILE_Info.SyncLastUpd, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("IsHost", pUSRPROFILE_Info.IsHost, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastSyncBy", pUSRPROFILE_Info.LastSyncBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncCreateBy", pUSRPROFILE_Info.SyncCreateBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                //objSQL.AddField("OperationGroup", pUSRPROFILE_Info.SyncCreateBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                //Amended by ellis 20170310
                objSQL.AddField("OperationGroup", pUSRPROFILE_Info.OperationGroup, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                switch (SaveType)
                {
                    case EnumSaveType.Insert:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert, "USRPROFILE", string.Empty);
                        break;
                    case EnumSaveType.Update:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "USRPROFILE", "USRPROFILE.UserID='" + pUSRPROFILE_Info.UserID + "'");
                        break;
                }
                lstSQL.Add(strSQL);

                //add to save bank info;
                objSQL.AddField("UserID", pUSRAPP_Info.UserID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("AppID", pUSRAPP_Info.AppID, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("AccessCode", pUSRAPP_Info.AccessCode, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("IsInherit", pUSRAPP_Info.IsInherit, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncCreate", pUSRAPP_Info.SyncCreate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncLastUpd", pUSRAPP_Info.SyncLastUpd, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("IsHost", pUSRAPP_Info.IsHost, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastSyncBy", pUSRAPP_Info.LastSyncBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                switch (SaveType)
                {
                    case EnumSaveType.Insert:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert, "USRAPP", string.Empty);
                        break;
                    case EnumSaveType.Update:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "USRAPP", "USRAPP.UserID='" + pUSRAPP_Info.UserID + "' AND " + "USRAPP.AppID='" + pUSRAPP_Info.AppID + "'");
                        break;
                }
                lstSQL.Add(strSQL);

                rValue = objDCom.BatchExecute(lstSQL, CommandType.Text, true, false);
                if (rValue == false)
                {
                    return null;
                }
                return pUSRPROFILE_Info.UserID;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                return null;
            }
        }

        public DataTable GetCountryAndState(string AgentID)
        {
            DataTable dt;
            String strSQL = string.Empty;
            String strFields = string.Empty;
            String strFilter = string.Empty;
            List<string> lstFields = new List<string>();
            try
            {
                strSQL = "SELECT AG_PROFILE.Country, AG_PROFILE.State FROM AG_PROFILE WHERE AG_PROFILE.AgentID ='" + AgentID + "'";
                dt = objDCom.Execute(strSQL, CommandType.Text, true);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt;
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

        public DataTable GetBankCountryAndState(string AgentID)
        {
            DataTable dt;
            String strSQL = string.Empty;
            String strFields = string.Empty;
            String strFilter = string.Empty;
            List<string> lstFields = new List<string>();
            try
            {
                strSQL = "SELECT AG_BANKINFO.Country, AG_BANKINFO.State FROM AG_BANKINFO INNER JOIN " +
                         " AG_PROFILE ON AG_BANKINFO.AgentID = AG_PROFILE.AgentID AND AG_PROFILE.AgentID ='" + AgentID + "'";
                dt = objDCom.Execute(strSQL, CommandType.Text, true);

                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt;
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

        public string GetTitle(string AgentID)
        {
            DataTable dt;
            String strSQL = string.Empty;
            String strFields = string.Empty;
            String strFilter = string.Empty;
            try
            {
                strSQL = "SELECT TOP 1 AG_PROFILE.Title FROM AG_PROFILE WHERE AG_PROFILE.AgentID ='" + AgentID + "'";
                dt = objDCom.Execute(strSQL, CommandType.Text, true);

                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["Title"].ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); 
                return "";
            }
        }

        public AgentBankInfo GetAgentBankInfo(string AgentID)
        {

            AgentBankInfo objAG_BANKINFO_Info;
            DataTable dt;
            DateTime dateValue;
            String strSQL = string.Empty;
            String strFields = string.Empty;
            String strFilter = string.Empty;
            List<string> lstFields = new List<string>();
            try
            {
                lstFields.Add("AG_BANKINFO.AgentID");
                lstFields.Add("AG_BANKINFO.BankName");
                lstFields.Add("AG_BANKINFO.Address1");
                lstFields.Add("AG_BANKINFO.Address2");
                lstFields.Add("AG_BANKINFO.Address3");
                lstFields.Add("AG_BANKINFO.Country");
                lstFields.Add("AG_BANKINFO.City");
                lstFields.Add("AG_BANKINFO.State");
                lstFields.Add("AG_BANKINFO.Postcode");
                lstFields.Add("AG_BANKINFO.AccountName");
                lstFields.Add("AG_BANKINFO.AccountNo");
                lstFields.Add("AG_BANKINFO.rowguid");
                lstFields.Add("AG_BANKINFO.SyncCreate");
                lstFields.Add("AG_BANKINFO.SyncLastUpd");
                lstFields.Add("AG_BANKINFO.LastSyncBy");

                strFields = GetSqlFields(lstFields);
                strFilter = " WHERE AG_BANKINFO.AgentID='" + AgentID + "'";
                strSQL = "SELECT " + strFields + " FROM AG_BANKINFO " + strFilter;
                dt = objDCom.Execute(strSQL, CommandType.Text, true);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow drRow = dt.Rows[0];

                    objAG_BANKINFO_Info = new AgentBankInfo();
                    objAG_BANKINFO_Info.AgentID = (string)drRow["AgentID"];
                    objAG_BANKINFO_Info.BankName = (string)drRow["BankName"];
                    objAG_BANKINFO_Info.Address1 = (string)drRow["Address1"];
                    objAG_BANKINFO_Info.Address2 = (string)drRow["Address2"];
                    objAG_BANKINFO_Info.Address3 = (string)drRow["Address3"];
                    objAG_BANKINFO_Info.Country = (string)drRow["Country"];
                    objAG_BANKINFO_Info.City = (string)drRow["City"];
                    objAG_BANKINFO_Info.State = (string)drRow["State"];
                    objAG_BANKINFO_Info.Postcode = (string)drRow["Postcode"];
                    objAG_BANKINFO_Info.AccountName = (string)drRow["AccountName"];
                    objAG_BANKINFO_Info.AccountNo = (string)drRow["AccountNo"];
                    objAG_BANKINFO_Info.rowguid = (Guid)drRow["rowguid"];
                    if (DateTime.TryParse(drRow["SyncCreate"].ToString(), out dateValue)) objAG_BANKINFO_Info.SyncCreate = (DateTime)drRow["SyncCreate"];
                    if (DateTime.TryParse(drRow["SyncLastUpd"].ToString(), out dateValue)) objAG_BANKINFO_Info.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
                    objAG_BANKINFO_Info.LastSyncBy = (string)drRow["LastSyncBy"];
                    return objAG_BANKINFO_Info;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }
    }

    public partial class AdminControl : Shared.CoreBase
    {
        SystemLog SystemLog = new SystemLog();
        StrucAdminSet _AdminSet = new StrucAdminSet();
        public AdminControl()
        {
        }

        public enum SaveType
        {
            Insert = 0,
            Update = 1
        }

        public AdminControl(IContainer container)
        {
            container.Add(this);

        }

        public enum EnumActivityType
        {
            LoggedIn = 0,
            LoginFail = 1,
        }

        public struct StrucAdminSet
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
        }

        public StrucAdminSet AdminSet
        {
            get { return _AdminSet; }
        }

        private static String GetSqlFields(List<string> Fields)
        {
            String strFields = string.Empty;
            if (Fields != null)
            {
                foreach (string sField in Fields)
                {
                    if (strFields == string.Empty)
                    {
                        strFields = sField;
                    }
                    else
                    {
                        strFields += ", " + sField;
                    }
                }
            }
            return strFields;
        }

        public USRPROFILE_Info GetSingleUSRPROFILE(string pUserID, string fieldName = "USRPROFILE.UserID")
        {
            objSQL.ClearFields();
            objSQL.ClearCondtions();
            USRPROFILE_Info objUSRPROFILE_Info;
            DataTable dt;
            DateTime dateValue;
            String strSQL = string.Empty;
            String strFields = string.Empty;
            String strFilter = string.Empty;
            List<string> lstFields = new List<string>();
            try
            {
                lstFields.Add("USRPROFILE.UserID");
                lstFields.Add("USRPROFILE.UserName");
                lstFields.Add("USRPROFILE.Password");
                lstFields.Add("USRPROFILE.RefID");
                lstFields.Add("USRPROFILE.RefType");
                lstFields.Add("USRPROFILE.ParentID");
                lstFields.Add("USRPROFILE.Status");
                lstFields.Add("USRPROFILE.Logged");
                lstFields.Add("USRPROFILE.LogStation");
                lstFields.Add("USRPROFILE.LastLogin");
                lstFields.Add("USRPROFILE.LastLogout");
                lstFields.Add("USRPROFILE.CreateDate");
                lstFields.Add("USRPROFILE.CreateBy");
                lstFields.Add("USRPROFILE.LastUpdate");
                lstFields.Add("USRPROFILE.UpdateBy");
                lstFields.Add("USRPROFILE.rowguid");
                lstFields.Add("USRPROFILE.SyncCreate");
                lstFields.Add("USRPROFILE.SyncLastUpd");
                lstFields.Add("USRPROFILE.IsHost");
                lstFields.Add("USRPROFILE.LastSyncBy");
                lstFields.Add("USRPROFILE.SyncCreateBy");
                lstFields.Add("USRPROFILE.OperationGroup");

                strFields = GetSqlFields(lstFields);
                strFilter = "WHERE " + fieldName + "='" + pUserID + "'";
                strSQL = "SELECT " + strFields + " FROM USRPROFILE " + strFilter;
                dt = objDCom.Execute(strSQL, CommandType.Text, true);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow drRow = dt.Rows[0];

                    objUSRPROFILE_Info = new USRPROFILE_Info();
                    objUSRPROFILE_Info.UserID = (string)drRow["UserID"];
                    objUSRPROFILE_Info.UserName = (string)drRow["UserName"];
                    objUSRPROFILE_Info.Password = (string)drRow["Password"];
                    objUSRPROFILE_Info.RefID = (string)drRow["RefID"];
                    objUSRPROFILE_Info.RefType = (byte)drRow["RefType"];
                    objUSRPROFILE_Info.ParentID = (string)drRow["ParentID"];
                    objUSRPROFILE_Info.Status = (byte)drRow["Status"];
                    objUSRPROFILE_Info.Logged = (byte)drRow["Logged"];
                    objUSRPROFILE_Info.LogStation = (string)drRow["LogStation"];
                    if (DateTime.TryParse(drRow["LastLogin"].ToString(), out dateValue)) objUSRPROFILE_Info.LastLogin = (DateTime)drRow["LastLogin"];
                    if (DateTime.TryParse(drRow["LastLogout"].ToString(), out dateValue)) objUSRPROFILE_Info.LastLogout = (DateTime)drRow["LastLogout"];
                    if (DateTime.TryParse(drRow["CreateDate"].ToString(), out dateValue)) objUSRPROFILE_Info.CreateDate = (DateTime)drRow["CreateDate"];
                    objUSRPROFILE_Info.CreateBy = (string)drRow["CreateBy"];
                    if (DateTime.TryParse(drRow["LastUpdate"].ToString(), out dateValue)) objUSRPROFILE_Info.LastUpdate = (DateTime)drRow["LastUpdate"];
                    objUSRPROFILE_Info.UpdateBy = (string)drRow["UpdateBy"];
                    objUSRPROFILE_Info.rowguid = (Guid)drRow["rowguid"];
                    if (DateTime.TryParse(drRow["SyncCreate"].ToString(), out dateValue)) objUSRPROFILE_Info.SyncCreate = (DateTime)drRow["SyncCreate"];
                    if (DateTime.TryParse(drRow["SyncLastUpd"].ToString(), out dateValue)) objUSRPROFILE_Info.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
                    objUSRPROFILE_Info.IsHost = (byte)drRow["IsHost"];
                    objUSRPROFILE_Info.LastSyncBy = (string)drRow["LastSyncBy"];
                    objUSRPROFILE_Info.SyncCreateBy = (string)drRow["SyncCreateBy"];
                    objUSRPROFILE_Info.OperationGroup = (string)drRow["OperationGroup"];
                    return objUSRPROFILE_Info;
                }
                else
                {
                    return null;
                    throw new ApplicationException("USRPROFILE does not exist.");
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public USRPROFILE_Info SaveUserProfile(USRPROFILE_Info pUSRPROFILE_Info, USRAPP_Info pUSRAPP_Info, EnumSaveType SaveType)
        {
            bool rValue = false;
            ArrayList lstSQL = new ArrayList();
            objSQL.ClearFields();
            objSQL.ClearCondtions();
            string strSQL = string.Empty;
            try
            {
                objSQL.AddField("UserID", pUSRPROFILE_Info.UserID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("UserName", pUSRPROFILE_Info.UserName, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastLogin", pUSRPROFILE_Info.LastLogin, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastLogin", pUSRPROFILE_Info.LastLogin, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncLastUpd", pUSRPROFILE_Info.SyncLastUpd, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastSyncBy", pUSRPROFILE_Info.LastSyncBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                switch (SaveType)
                {
                    case EnumSaveType.Insert:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert, "USRPROFILE", string.Empty);
                        break;
                    case EnumSaveType.Update:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "USRPROFILE", "USRPROFILE.UserID='" + pUSRPROFILE_Info.UserID + "'");
                        break;
                }
                lstSQL.Add(strSQL);

                rValue = objDCom.BatchExecute(lstSQL, CommandType.Text, true, false);
                if (rValue == false)
                {
                    return null;
                }
                return GetSingleUSRPROFILE(pUSRPROFILE_Info.UserID);
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }

        public bool ValidateAdmin(string Username, string Password)
        {
            String strSQL = string.Empty;
            String strFields = string.Empty;
            String strFilter = string.Empty;
            DataTable dt;
            if (Username == string.Empty) { return false; }
            try
            {
                if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
                {
                    StartSQLControl();
                    strFilter = " WHERE USRPROFILE.Username='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, Username) +
                                "' AND USRPROFILE.Status = 1 AND USRGROUP.Status = 1 AND USRPROFILE.Password ='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, Password) + "'";
                    strSQL = "SELECT DISTINCT  USRPROFILE.UserID, USRPROFILE.UserName, USRPROFILE.Password, USRPROFILE.RefID, USRPROFILE.RefType, USRPROFILE.ParentID, USRPROFILE.Status, USRPROFILE.Logged, " +
                    "USRPROFILE.LogStation, USRPROFILE.LastLogin, USRPROFILE.LastLogout, USRPROFILE.CreateDate, USRPROFILE.CreateBy, USRPROFILE.LastUpdate, USRPROFILE.UpdateBy,USRPROFILE.rowguid, USRPROFILE.SyncCreate, " +
                    "USRPROFILE.SyncLastUpd,USRPROFILE.OperationGroup, USRPROFILE.IsHost, USRPROFILE.LastSyncBy,USRGROUP.AccessLevel,USRAPP.AppID, USRPROFILE.SyncCreateBy,USRAPP.AccessCode,USRGROUP.GroupCode ,USRGROUP.GroupName FROM USRPROFILE JOIN USRAPP (NOLOCK) on USRAPP.UserID=USRPROFILE.UserID JOIN " +
                    "USRGROUP (NOLOCK) on USRAPP.AccessCode=USRGROUP.GroupCode " + strFilter;
                    dt = objDCom.Execute(strSQL, CommandType.Text, true);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow drRow = dt.Rows[0];
                        _AdminSet.AdminID = (string)drRow["UserID"];
                        _AdminSet.AdminName = (string)drRow["UserName"];
                        _AdminSet.RefID = (string)drRow["RefID"];
                        _AdminSet.RefType = (byte)drRow["RefType"];
                        _AdminSet.AccessLevel = (byte)drRow["AccessLevel"];
                        _AdminSet.AppID = (int)drRow["AppID"];
                        _AdminSet.GroupName = (string)drRow["GroupName"];
                        _AdminSet.GroupCode = (string)drRow["GroupCode"];
                        _AdminSet.OperationGroup = (string)drRow["OperationGroup"];
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return false;
            }
        }
    }

    public partial class AgentControl : Shared.CoreBase
    {
        SystemLog SystemLog = new SystemLog();
        StrucAgentSet _AgentSet = new StrucAgentSet();
        public AgentControl()
        {
        }

        public enum SaveType
        {
            Insert = 0,
            Update = 1
        }

        public AgentControl(IContainer container)
        {
            container.Add(this);

        }


        public enum EnumActivityType
        {
            LoggedIn = 0,
            LoginFail = 1,
        }

        public struct StrucAgentSet
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
            public string Password;
            public string OperationGroup;
            public DateTime LastLogin;
            public string OrganizationName;
            public string Email;
            public string Contact;
            public string AgLimit;
            public string Currency;
        }

        public StrucAgentSet AgentSet
        {
            get { return _AgentSet; }
        }

        private static String GetSqlFields(List<string> Fields)
        {
            String strFields = string.Empty;
            if (Fields != null)
            {
                foreach (string sField in Fields)
                {
                    if (strFields == string.Empty)
                    {
                        strFields = sField;
                    }
                    else
                    {
                        strFields += ", " + sField;
                    }
                }
            }
            return strFields;
        }

        public AgentCategory GetAgentCategory(string CategoryID)
        {
            AgentCategory AgentCatModel;
            DataTable dt;
            String strSQL = string.Empty;
            String strFields = string.Empty;
            String strFilter = string.Empty;
            List<string> lstFields = new List<string>();
            try
            {
                lstFields.Add("AG_CATEGORY.AgentCatgID");
                lstFields.Add("AG_CATEGORY.AgentCatgDesc");
                lstFields.Add("AG_CATEGORY.MaxEnquiry");
                lstFields.Add("AG_CATEGORY.CounterTimer");
                lstFields.Add("AG_CATEGORY.MaxSuspend");
                lstFields.Add("AG_CATEGORY.SuspendDuration");
                lstFields.Add("AG_CATEGORY.BlacklistDuration");
                lstFields.Add("AG_CATEGORY.Status");
                lstFields.Add("AG_CATEGORY.rowguid");
                lstFields.Add("AG_CATEGORY.SyncCreate");
                lstFields.Add("AG_CATEGORY.SyncLastUpd");
                lstFields.Add("AG_CATEGORY.LastSyncBy");

                strFields = GetSqlFields(lstFields);
                strFilter = " WHERE AG_CATEGORY.AgentCatgID='" + CategoryID + "'";
                strSQL = "SELECT " + strFields + " FROM AG_CATEGORY " + strFilter;
                dt = objDCom.Execute(strSQL, CommandType.Text, true);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow drRow = dt.Rows[0];

                    AgentCatModel = new AgentCategory();
                    AgentCatModel.AgentCatgID = (string)drRow["AgentCatgID"];
                    AgentCatModel.AgentCatgDesc = (string)drRow["AgentCatgDesc"];
                    AgentCatModel.MaxEnquiry = (int)drRow["MaxEnquiry"];
                    AgentCatModel.CounterTimer = (int)drRow["CounterTimer"];
                    AgentCatModel.MaxSuspend = (int)drRow["MaxSuspend"];
                    AgentCatModel.SuspendDuration = (int)drRow["SuspendDuration"];
                    AgentCatModel.BlacklistDuration = (int)drRow["BlacklistDuration"];
                    AgentCatModel.Status = (byte)drRow["Status"];

                    _AgentSet.AgentCategoryID = (string)drRow["AgentCatgID"];
                    _AgentSet.BlacklistDuration = (int)drRow["BlacklistDuration"];
                    _AgentSet.CounterTimer = (int)drRow["CounterTimer"];
                    _AgentSet.MaxEnquiry = (int)drRow["MaxEnquiry"];
                    _AgentSet.MaxSuspend = (int)drRow["MaxSuspend"];
                    _AgentSet.SuspendDuration = (int)drRow["SuspendDuration"];
                    _AgentSet.BlacklistDuration = (int)drRow["BlacklistDuration"];
                    //_AgentSet.AgentType = "PublicAgent";

                    _AgentSet.LoginName = _AgentSet.AgentName;
                    return AgentCatModel;
                }
                else
                {
                    return null;
                    throw new ApplicationException("Agent category does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }
        public DataTable GetAllAgentCategory()
        {
            DataTable dt;
            String strSQL = string.Empty;
            String strFields = string.Empty;
            String strFilter = string.Empty;
            List<string> lstFields = new List<string>();
            try
            {
                lstFields.Add("AG_CATEGORY.AgentCatgID");
                lstFields.Add("AG_CATEGORY.AgentCatgDesc");
                lstFields.Add("AG_CATEGORY.MaxEnquiry");
                lstFields.Add("AG_CATEGORY.CounterTimer");
                lstFields.Add("AG_CATEGORY.MaxSuspend");
                lstFields.Add("AG_CATEGORY.SuspendDuration");
                lstFields.Add("AG_CATEGORY.BlacklistDuration");
                lstFields.Add("AG_CATEGORY.Status");

                strFields = GetSqlFields(lstFields);
                strFilter = "";
                strSQL = "SELECT " + strFields + " FROM AG_CATEGORY " + strFilter;
                dt = objDCom.Execute(strSQL, CommandType.Text, true);

                if (dt != null && dt.Rows.Count > 0)
                {

                    return dt;
                }
                else
                {
                    return null;
                    throw new ApplicationException("Agent category does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }

        public AgentCategory SaveAgentCategory(AgentCategory pAG_CATEGORY_Info, SaveType pSaveType)
        {
            bool rValue = false;
            ArrayList lstSQL = new ArrayList();
            string strSQL = string.Empty;
            AUDITLOG AUDITLOGInfo = new AUDITLOG();
            GeneralControl AuditLogBase = new GeneralControl();
            List<AUDITLOG> lstAuditLog = new List<AUDITLOG>();
            try
            {
                AgentCategory agCat = GetAgentCategory(pAG_CATEGORY_Info.AgentCatgID);
                bool flag = true;
                if (agCat.MaxEnquiry != pAG_CATEGORY_Info.MaxEnquiry || agCat.CounterTimer != pAG_CATEGORY_Info.CounterTimer || agCat.MaxEnquiry != pAG_CATEGORY_Info.MaxEnquiry || agCat.MaxSuspend != pAG_CATEGORY_Info.MaxSuspend)
                    flag = false;
                if (agCat.SuspendDuration != pAG_CATEGORY_Info.SuspendDuration || agCat.BlacklistDuration != pAG_CATEGORY_Info.BlacklistDuration || agCat.Status != pAG_CATEGORY_Info.Status)
                    flag = false;

                objSQL.AddField("AgentCatgID", pAG_CATEGORY_Info.AgentCatgID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("AgentCatgDesc", pAG_CATEGORY_Info.AgentCatgDesc, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("MaxEnquiry", pAG_CATEGORY_Info.MaxEnquiry, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("CounterTimer", pAG_CATEGORY_Info.CounterTimer, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("MaxSuspend", pAG_CATEGORY_Info.MaxSuspend, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SuspendDuration", pAG_CATEGORY_Info.SuspendDuration, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("BlacklistDuration", pAG_CATEGORY_Info.BlacklistDuration, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Status", pAG_CATEGORY_Info.Status, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncCreate", pAG_CATEGORY_Info.SyncCreate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncLastUpd", pAG_CATEGORY_Info.SyncLastUpd, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastSyncBy", pAG_CATEGORY_Info.LastSyncBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                switch (pSaveType)
                {
                    case SaveType.Insert:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert, "AG_CATEGORY", string.Empty);
                        AUDITLOGInfo.Action = 0;
                        break;
                    case SaveType.Update:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "AG_CATEGORY", "AG_CATEGORY.AgentCatgID='" + pAG_CATEGORY_Info.AgentCatgID + "'");
                        AUDITLOGInfo.Action = 1;
                        break;
                }
                lstSQL.Add(strSQL);

                AUDITLOGInfo.TransID = DateTime.Now.ToString("yyyyMMddHHmmsss"); ;
                AUDITLOGInfo.SeqNo = 0;
                AUDITLOGInfo.RefCode = "";
                AUDITLOGInfo.Table_Name = "AG_CATEGORY";
                AUDITLOGInfo.SQL = strSQL;
                AUDITLOGInfo.CreatedBy = pAG_CATEGORY_Info.LastSyncBy;
                AUDITLOGInfo.CreatedDate = DateTime.Now;
                AUDITLOGInfo.Priority = 0;
                lstAuditLog.Add(AUDITLOGInfo);

                rValue = objDCom.BatchExecute(lstSQL, CommandType.Text, true, false);
                if (rValue == false)
                {
                    return null;
                }
                if (flag == false)
                    AuditLogBase.SaveSYS_AUDITLOG(lstAuditLog);

                return GetAgentCategory(pAG_CATEGORY_Info.AgentCatgID);

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }
        public bool ValidateAgent(string Username, string Password, Shared.EnumAgentType AgentType)
        {
            String strSQL = string.Empty;
            String strFields = string.Empty;
            String strFilter = string.Empty;
            DataTable dt;
            if (Username == string.Empty) { return false; }
            try
            {
                if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
                {
                    StartSQLControl();
                    if (Password == "")
                    {
                        strFilter = " WHERE AG_PROFILE.Username='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, Username) +
                                "' AND AG_PROFILE.flag = 0 ";
                    }
                    else
                    {
                        strFilter = " WHERE AG_PROFILE.Username='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, Username) +
                                    "' AND AG_PROFILE.flag = 0 AND AG_PROFILE.Password ='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, Password) + "'";
                    }
                    if (AgentType.ToString() == "PublicAgent")
                    {
                        strFilter += " AND AG_CATEGORY.AgentCatgID != 99 ";
                    }
                    strSQL = "SELECT AG_PROFILE.AgentID, AG_PROFILE.Username, AG_CATEGORY.MaxEnquiry,AG_PROFILE.OperationGroup, AG_CATEGORY.MaxSuspend, AG_CATEGORY.SuspendDuration, AG_PROFILE.AgentCatgID," +
                                " AG_CATEGORY.BlacklistDuration, AG_CATEGORY.CounterTimer FROM AG_PROFILE INNER JOIN AG_CATEGORY ON AG_PROFILE.AgentCatgID = AG_CATEGORY.AgentCatgID " + strFilter;

                    dt = objDCom.Execute(strSQL, CommandType.Text, true);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow drRow = dt.Rows[0];
                        _AgentSet.AgentID = (string)drRow["AgentID"];
                        _AgentSet.AgentName = (string)drRow["Username"];
                        _AgentSet.AgentCategoryID = (string)drRow["AgentCatgID"];
                        _AgentSet.OperationGroup = (string)drRow["OperationGroup"];
                        _AgentSet.Password = Password;
                        _AgentSet.AgentType = AgentType;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return false;
            }
        }

        public bool SetWhiteList(string AgentID, string UserID)
        {
            ArrayList lstSQL = new ArrayList();
            String strSQL = string.Empty;
            bool rValue = false;
            if (AgentID == string.Empty) { return false; }
            try
            {
                if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
                {
                    StartSQLControl();
                    strSQL = "UPDATE AG_BLACKLIST SET AG_BLACKLIST.BlacklistExpiryDate = DATEADD(day,-1,GETDATE()), AG_BLACKLIST.LastSyncBy = '" +
                             objSQL.ParseValue(SQLControl.EnumDataType.dtString, UserID) + "' WHERE AG_BLACKLIST.AgentID = '" +
                             objSQL.ParseValue(SQLControl.EnumDataType.dtString, AgentID) + "'";
                    lstSQL.Add(strSQL);
                    strSQL = "Update EN_ENQUIRYLOG set NoOfAttempt = 0 where EnquiryDate >= DATEADD(day,-1,GetDate()) and AgentID = '" +
                             objSQL.ParseValue(SQLControl.EnumDataType.dtString, AgentID) + "'";
                    lstSQL.Add(strSQL);
                    //added by agus to restore suspend after whitelist -> SuspendExpiry = DATEADD(day,-1,GETDATE())
                    strSQL = "Update EN_SUSPENDLIST set SuspendAttempt = 0, SuspendExpiry = DATEADD(day,-1,GETDATE()) where SuspendDate >= DATEADD(day,-1,GetDate()) and AgentID = '" +
                             objSQL.ParseValue(SQLControl.EnumDataType.dtString, AgentID) + "'";
                    lstSQL.Add(strSQL);
                    rValue = objDCom.BatchExecute(lstSQL, CommandType.Text, true, false);
                    return rValue;
                }
                return rValue;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return rValue;
            }
        }

        public DataTable SearchAgentData(string fieldName, string filter)
        {
            String strSQL = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
                {
                    StartSQLControl();
                    if (fieldName == "All")
                    {
                        fieldName = "AG_PROFILE.AgentID";
                        filter = "";
                    }
                    else
                    {
                        if (fieldName.ToString() != "countryName")
                            fieldName = "AG_PROFILE." + fieldName;
                        else
                            fieldName = "COUNTRYCODE." + fieldName;
                    }

                    strSQL = "SELECT DISTINCT AG_PROFILE.AgentID,(AG_PROFILE.ContactFirstName+' '+AG_PROFILE.ContactLastName) as FullName,AG_PROFILE.Address1,AG_PROFILE.JoinDate,AG_PROFILE.Username,AG_PROFILE.ContactFirstName,BlacklistDuration,AG_PROFILE.ContactLastName,AG_PROFILE.Email, " +
                             "AG_PROFILE.Country,COUNTRYCODE.countryName,AG_PROFILE.LicenseNo,AG_PROFILE.MobileNo,AG_PROFILE.PhoneNo FROM AG_PROFILE,COUNTRYCODE,AG_CATEGORY " +
                             "WHERE " + fieldName + " LIKE '" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, filter) + "%' " +
                             "AND AG_PROFILE.AgentCatgID = AG_CATEGORY.AgentCatgID AND AG_CATEGORY.AgentCatgID != 99 AND AG_PROFILE.flag = 0 AND AG_PROFILE.Status = 1 AND AG_PROFILE.Country = COUNTRYCODE.CountryCode";
                    dt = objDCom.Execute(strSQL, System.Data.CommandType.Text, false);
                    if (dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }

        public DataTable SearchInactBlackAgent(string filter)
        {
            String strSQL = string.Empty;
            DataTable dt = new DataTable();
            if (filter == "Active")
            {
                try
                {
                    if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
                    {

                        StartSQLControl();
                        strSQL = "SELECT DISTINCT AG_PROFILE.AgentID,AG_PROFILE.Username,BlacklistDate,BlacklistExpiryDate,Remark,(AG_PROFILE.ContactFirstName+' '+AG_PROFILE.ContactLastName) as FullName,AG_PROFILE.ContactFirstName,AG_PROFILE.ContactLastName,AG_PROFILE.Email," +
                        "AG_PROFILE.Country,COUNTRYCODE.countryName,AG_PROFILE.LicenseNo,AG_PROFILE.MobileNo,AG_PROFILE.PhoneNo FROM AG_PROFILE,AG_BLACKLIST,COUNTRYCODE," +
                        "AG_CATEGORY WHERE  AG_PROFILE.flag = 0 AND AG_PROFILE.Status = 1 AND AG_BLACKLIST.AgentID = AG_PROFILE.AgentID " +
                        "AND AG_PROFILE.AgentCatgID = AG_CATEGORY.AgentCatgID AND AG_CATEGORY.AgentCatgID != 99 AND AG_BLACKLIST.BlacklistExpiryDate >= GETDATE() AND AG_PROFILE.Country = COUNTRYCODE.CountryCode";
                        dt = objDCom.Execute(strSQL, System.Data.CommandType.Text, false);
                        if (dt.Rows.Count > 0)
                        {
                            return dt;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    SystemLog.Notifier.Notify(ex); return null;
                }
            }
            else
            {
                try
                {
                    if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
                    {

                        StartSQLControl();
                        strSQL = "SELECT DISTINCT AG_PROFILE.AgentID,AG_PROFILE.Username,BlacklistDate,BlacklistExpiryDate,Remark,(AG_PROFILE.ContactFirstName+' '+AG_PROFILE.ContactLastName) as FullName,AG_PROFILE.ContactFirstName,AG_PROFILE.ContactLastName,AG_PROFILE.Email," +
                        "AG_PROFILE.Country,COUNTRYCODE.countryName,AG_PROFILE.LicenseNo,AG_PROFILE.MobileNo,AG_PROFILE.PhoneNo FROM AG_PROFILE,AG_BLACKLIST,COUNTRYCODE," +
                        "AG_CATEGORY WHERE  AG_PROFILE.flag = 0 AND AG_PROFILE.Status = 0 AND AG_BLACKLIST.AgentID = AG_PROFILE.AgentID " +
                        "AND AG_PROFILE.AgentCatgID = AG_CATEGORY.AgentCatgID AND AG_CATEGORY.AgentCatgID != 99 AND AG_BLACKLIST.BlacklistExpiryDate >= GETDATE() AND AG_PROFILE.Country = COUNTRYCODE.CountryCode";
                        dt = objDCom.Execute(strSQL, System.Data.CommandType.Text, false);
                        if (dt.Rows.Count > 0)
                        {
                            return dt;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    SystemLog.Notifier.Notify(ex); return null;
                }
            }
        }




        public DataTable SearchInactAgent(string filter)
        {
            string strSQL = string.Empty;
            DataTable dt = new DataTable();
            if (filter == "Active")
            {
                try
                {
                    if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
                    {
                        StartConnection();
                        strSQL = "SELECT DISTINCT AG_PROFILE.AgentID,AG_PROFILE.Username,AG_PROFILE.ContactFirstName,AG_PROFILE.ContactLastName,AG_PROFILE.Email, " +
                                "AG_PROFILE.Country,COUNTRYCODE.countryName,AG_PROFILE.LicenseNo,AG_PROFILE.MobileNo,AG_PROFILE.PhoneNo FROM AG_PROFILE " +
                                 "JOIN COUNTRYCODE (NOLOCK) on AG_PROFILE.Country = COUNTRYCODE.CountryCode JOIN AG_CATEGORY (NOLOCK) on AG_PROFILE.AgentCatgID = AG_CATEGORY.AgentCatgID " +
                                 " WHERE AG_PROFILE.AgentID NOT IN (SELECT R.TransID FROM REQAPPL R, AG_PROFILE A WHERE A.AgentID =R.TransID AND ReqType='B' AND (ApprovedBy IS  NULL OR ExpiryDate<GETDATE()))" +
                                "  AND AG_PROFILE.AgentID NOT IN(SELECT B.AgentID FROM AG_BLACKLIST B,AG_PROFILE A WHERE A.AgentID=B.AgentID AND B.BlacklistExpiryDate > GETDATE())" +
                                "  AND AG_PROFILE.Status =1 ";
                        dt = objDCom.Execute(strSQL, System.Data.CommandType.Text, false);
                        if (dt.Rows.Count > 0)
                        {
                            return dt;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    return dt;
                }
                catch (Exception ex)
                {
                    SystemLog.Notifier.Notify(ex); return null;
                }

            }
            else
            {
                try
                {
                    if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
                    {
                        StartConnection();
                        strSQL = "SELECT DISTINCT AG_PROFILE.AgentID,AG_PROFILE.Username,AG_PROFILE.ContactFirstName,AG_PROFILE.ContactLastName,AG_PROFILE.Email, " +
                                "AG_PROFILE.Country,COUNTRYCODE.countryName,AG_PROFILE.LicenseNo,AG_PROFILE.MobileNo,AG_PROFILE.PhoneNo FROM AG_PROFILE " +
                                 "JOIN COUNTRYCODE (NOLOCK) on AG_PROFILE.Country = COUNTRYCODE.CountryCode JOIN AG_CATEGORY (NOLOCK) on AG_PROFILE.AgentCatgID = AG_CATEGORY.AgentCatgID " +
                                 " WHERE AG_PROFILE.AgentID NOT IN (SELECT R.TransID FROM REQAPPL R, AG_PROFILE A WHERE A.AgentID =R.TransID AND ReqType='B' AND (ApprovedBy IS  NULL OR ExpiryDate<GETDATE()))" +
                                "  AND AG_PROFILE.AgentID NOT IN(SELECT B.AgentID FROM AG_BLACKLIST B,AG_PROFILE A WHERE A.AgentID=B.AgentID AND B.BlacklistExpiryDate > GETDATE())" +
                                "  AND AG_PROFILE.Status =0 ";
                        dt = objDCom.Execute(strSQL, System.Data.CommandType.Text, false);
                        if (dt.Rows.Count > 0)
                        {
                            return dt;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    return dt;
                }
                catch (Exception ex)
                {
                    SystemLog.Notifier.Notify(ex); return null;
                }

            }

        }




        public DataTable SearchAgentDataAdmin(string fieldName, string filter)
        {
            String strSQL = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
                {
                    StartSQLControl();
                    if (fieldName == "All")
                    {
                        /// edited by jiakang 2/8/2012 

                        strSQL = "SELECT DISTINCT AG_PROFILE.AgentID,AG_PROFILE.Username,AG_PROFILE.ContactFirstName,AG_PROFILE.ContactLastName,AG_PROFILE.Email, " +
                             "AG_PROFILE.Country,COUNTRYCODE.countryName,AG_PROFILE.LicenseNo,AG_PROFILE.MobileNo,AG_PROFILE.PhoneNo FROM AG_PROFILE " +
                              "JOIN COUNTRYCODE (NOLOCK) on AG_PROFILE.Country = COUNTRYCODE.CountryCode JOIN AG_CATEGORY (NOLOCK) on AG_PROFILE.AgentCatgID = AG_CATEGORY.AgentCatgID " +
                              " WHERE AG_PROFILE.AgentID NOT IN (SELECT R.TransID FROM REQAPPL R, AG_PROFILE A WHERE A.AgentID =R.TransID AND ReqType='B' AND (ApprovedBy IS  NULL OR ExpiryDate<GETDATE()))" +
                             "  AND AG_PROFILE.AgentID NOT IN(SELECT B.AgentID FROM AG_BLACKLIST B,AG_PROFILE A WHERE A.AgentID=B.AgentID AND B.BlacklistExpiryDate > GETDATE())" +
                                "  AND AG_PROFILE.Status =1 ";

                    }
                    else
                    {
                        if (fieldName.ToString() != "countryName")
                        {
                            fieldName = "AG_PROFILE." + fieldName;
                            strSQL = "SELECT DISTINCT AG_PROFILE.AgentID,AG_PROFILE.Username,AG_PROFILE.ContactFirstName,AG_PROFILE.ContactLastName,AG_PROFILE.Email, " +
                             "AG_PROFILE.Country,COUNTRYCODE.countryName,AG_PROFILE.LicenseNo,AG_PROFILE.MobileNo,AG_PROFILE.PhoneNo FROM AG_PROFILE " +
                              "JOIN COUNTRYCODE (NOLOCK) on AG_PROFILE.Country = COUNTRYCODE.CountryCode JOIN AG_CATEGORY (NOLOCK) on AG_PROFILE.AgentCatgID = AG_CATEGORY.AgentCatgID " +
                              "LEFT JOIN REQAPPL (NOLOCK) on AG_PROFILE.AgentID=TransID AND ReqType='B' AND ApprovedBy IS NULL AND ExpiryDate>GETDATE() " +
                              "WHERE " + fieldName + " LIKE '" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, filter) + "%' " +
                             "AND AG_CATEGORY.AgentCatgID != 99 AND AG_PROFILE.flag = 0 AND AG_PROFILE.Status = 1  AND AG_PROFILE.AgentID NOT IN (SELECT R.TransID FROM REQAPPL R, AG_PROFILE A WHERE A.AgentID =R.TransID AND ReqType='B' AND ApprovedBy IS  NULL) " +
                            "  AND AG_PROFILE.AgentID NOT IN(SELECT B.AgentID FROM AG_BLACKLIST B,AG_PROFILE A WHERE A.AgentID=B.AgentID AND B.BlacklistExpiryDate > GETDATE())";
                        }
                        else
                        {
                            fieldName = "COUNTRYCODE." + fieldName;
                            strSQL = "SELECT DISTINCT AG_PROFILE.AgentID,AG_PROFILE.Username,AG_PROFILE.ContactFirstName,AG_PROFILE.ContactLastName,AG_PROFILE.Email, " +
                                 "AG_PROFILE.Country,COUNTRYCODE.countryName,AG_PROFILE.LicenseNo,AG_PROFILE.MobileNo,AG_PROFILE.PhoneNo FROM AG_PROFILE " +
                                  "JOIN COUNTRYCODE (NOLOCK) on AG_PROFILE.Country = COUNTRYCODE.CountryCode JOIN AG_CATEGORY (NOLOCK) on AG_PROFILE.AgentCatgID = AG_CATEGORY.AgentCatgID " +
                                  "LEFT JOIN REQAPPL (NOLOCK) on AG_PROFILE.AgentID=TransID AND ReqType='B'  AND ExpiryDate>GETDATE() " +
                                  "WHERE " + fieldName + " LIKE '" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, filter) + "%' " +
                                 "AND AG_CATEGORY.AgentCatgID != 99 AND AG_PROFILE.flag = 0 AND AG_PROFILE.Status = 1   AND AG_PROFILE.AgentID NOT IN (SELECT R.TransID FROM REQAPPL R, AG_PROFILE A WHERE A.AgentID =R.TransID AND ReqType='B' AND ApprovedBy IS  NULL) " +
                                 "AND AG_PROFILE.AgentID NOT IN(SELECT B.AgentID FROM AG_BLACKLIST B,AG_PROFILE A WHERE A.AgentID=B.AgentID AND B.BlacklistExpiryDate > GETDATE())";
                        }
                    }

                    dt = objDCom.Execute(strSQL, System.Data.CommandType.Text, false);
                    if (dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }

        public bool SetRejectApprove(RequestApp pREQAPPL_Info)
        {
            ArrayList lstSQL = new ArrayList();
            String strSQL = string.Empty;
            bool rValue = false;
            try
            {
                if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
                {
                    StartSQLControl();
                    objSQL.AddField("ReqID", pREQAPPL_Info.ReqID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("UserID", pREQAPPL_Info.UserID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("ReqType", pREQAPPL_Info.ReqType, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("TransID", pREQAPPL_Info.TransID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("RequestDate", pREQAPPL_Info.RequestDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("ExpiryDate", pREQAPPL_Info.ExpiryDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("Remark", pREQAPPL_Info.Remark, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("ApprovedBy", pREQAPPL_Info.ApprovedBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("ApprovedDate", pREQAPPL_Info.ApprovedDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("SyncCreate", pREQAPPL_Info.SyncCreate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("SyncLastUpd", pREQAPPL_Info.SyncLastUpd, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("LastSyncBy", pREQAPPL_Info.LastSyncBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);

                    strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "REQAPPL", "REQAPPL.ReqID='" + pREQAPPL_Info.ReqID + "'");

                    lstSQL.Add(strSQL);

                    rValue = objDCom.BatchExecute(lstSQL, CommandType.Text, true, false);
                    return rValue;
                }
                return rValue;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return rValue;
            }
        }

        public AgentBlacklistLog SaveBlacklistApprove(AgentBlacklistLog pAG_BLACKLIST_Info, AgentActivity pAG_ACTIVITY_Info, RequestApp pREQAPPL_Info, EnumSaveType pType)
        {
            bool rValue = false;
            ArrayList lstSQL = new ArrayList();
            string strSQL = string.Empty;
            try
            {
                objSQL.AddField("BlacklistID", pAG_BLACKLIST_Info.BlacklistID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("AgentID", pAG_BLACKLIST_Info.AgentID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("BlacklistDate", pAG_BLACKLIST_Info.BlacklistDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("BlacklistExpiryDate", pAG_BLACKLIST_Info.BlacklistExpiryDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Status", pAG_BLACKLIST_Info.Status, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncCreate", pAG_BLACKLIST_Info.SyncCreate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncLastUpd", pAG_BLACKLIST_Info.SyncLastUpd, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastSyncBy", pAG_BLACKLIST_Info.LastSyncBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Remark", pAG_BLACKLIST_Info.Remark, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                switch (pType)
                {
                    case EnumSaveType.Insert:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert, "AG_BLACKLIST", string.Empty);
                        break;
                    case EnumSaveType.Update:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "AG_BLACKLIST", "AG_BLACKLIST.BlacklistID='" + pAG_BLACKLIST_Info.BlacklistID + "' and AG_BLACKLIST.AgentID='" + pAG_BLACKLIST_Info.AgentID + "'");
                        break;
                }
                lstSQL.Add(strSQL);

                objSQL.AddField("AgentID", pAG_ACTIVITY_Info.AgentID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastLoginDate", pAG_ACTIVITY_Info.LastLoginDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastFailedLoginDate", pAG_ACTIVITY_Info.LastFailedLoginDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastFailedLoginAttempt", pAG_ACTIVITY_Info.LastFailedLoginAttempt, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("TotalFailedLoginAttempt", pAG_ACTIVITY_Info.TotalFailedLoginAttempt, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("ExpiryBlacklistDate", pAG_ACTIVITY_Info.ExpiryBlacklistDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastSuspend", pAG_ACTIVITY_Info.LastSuspend, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastBlacklist", pAG_ACTIVITY_Info.LastBlacklist, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncCreate", pAG_ACTIVITY_Info.SyncCreate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncLastUpd", pAG_ACTIVITY_Info.SyncLastUpd, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);

                strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "AG_ACTIVITY", "AG_ACTIVITY.AgentID='" + pAG_ACTIVITY_Info.AgentID + "'");

                lstSQL.Add(strSQL);

                objSQL.AddField("ReqID", pREQAPPL_Info.ReqID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("UserID", pREQAPPL_Info.UserID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("ReqType", pREQAPPL_Info.ReqType, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("TransID", pREQAPPL_Info.TransID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("RequestDate", pREQAPPL_Info.RequestDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("ExpiryDate", pREQAPPL_Info.ExpiryDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Remark", pREQAPPL_Info.Remark, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("ApprovedBy", pREQAPPL_Info.ApprovedBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("ApprovedDate", pREQAPPL_Info.ApprovedDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncCreate", pREQAPPL_Info.SyncCreate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncLastUpd", pREQAPPL_Info.SyncLastUpd, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastSyncBy", pREQAPPL_Info.LastSyncBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);

                strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "REQAPPL", "REQAPPL.ReqID='" + pREQAPPL_Info.ReqID + "'");

                lstSQL.Add(strSQL);

                rValue = objDCom.BatchExecute(lstSQL, CommandType.Text, true, false);
                if (rValue == false)
                {
                    return null;
                }
                return GetSingleAG_BLACKLIST(pAG_BLACKLIST_Info.AgentID);
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }

        //added by ketee , Save New BlackListAgent
        public AgentBlacklistLog SaveNewBlacklistApprove(AgentBlacklistLog pAG_BLACKLIST_Info, AgentActivity pAG_ACTIVITY_Info, RequestApp pREQAPPL_Info, EnumSaveType pType)
        {
            bool rValue = false;
            ArrayList lstSQL = new ArrayList();
            string strSQL = string.Empty;
            try
            {
                objSQL.AddField("BlacklistID", pAG_BLACKLIST_Info.BlacklistID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("AgentID", pAG_BLACKLIST_Info.AgentID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("BlacklistDate", pAG_BLACKLIST_Info.BlacklistDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("BlacklistExpiryDate", pAG_BLACKLIST_Info.BlacklistExpiryDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Status", pAG_BLACKLIST_Info.Status, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncCreate", pAG_BLACKLIST_Info.SyncCreate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncLastUpd", pAG_BLACKLIST_Info.SyncLastUpd, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastSyncBy", pAG_BLACKLIST_Info.LastSyncBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Remark", pAG_BLACKLIST_Info.Remark, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                switch (pType)
                {
                    case EnumSaveType.Insert:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert, "AG_BLACKLIST", string.Empty);
                        break;
                    case EnumSaveType.Update:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "AG_BLACKLIST", "AG_BLACKLIST.BlacklistID='" + pAG_BLACKLIST_Info.BlacklistID + "' and AG_BLACKLIST.AgentID='" + pAG_BLACKLIST_Info.AgentID + "'");
                        break;
                }
                lstSQL.Add(strSQL);

                objSQL.AddField("AgentID", pAG_ACTIVITY_Info.AgentID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastLoginDate", pAG_ACTIVITY_Info.LastLoginDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastFailedLoginDate", pAG_ACTIVITY_Info.LastFailedLoginDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastFailedLoginAttempt", pAG_ACTIVITY_Info.LastFailedLoginAttempt, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("TotalFailedLoginAttempt", pAG_ACTIVITY_Info.TotalFailedLoginAttempt, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("ExpiryBlacklistDate", pAG_ACTIVITY_Info.ExpiryBlacklistDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastSuspend", pAG_ACTIVITY_Info.LastSuspend, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastBlacklist", pAG_ACTIVITY_Info.LastBlacklist, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncCreate", pAG_ACTIVITY_Info.SyncCreate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncLastUpd", pAG_ACTIVITY_Info.SyncLastUpd, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);

                strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert, "AG_ACTIVITY", string.Empty);

                lstSQL.Add(strSQL);

                objSQL.AddField("ReqID", pREQAPPL_Info.ReqID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("UserID", pREQAPPL_Info.UserID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("ReqType", pREQAPPL_Info.ReqType, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("TransID", pREQAPPL_Info.TransID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("RequestDate", pREQAPPL_Info.RequestDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("ExpiryDate", pREQAPPL_Info.ExpiryDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Remark", pREQAPPL_Info.Remark, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("ApprovedBy", pREQAPPL_Info.ApprovedBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("ApprovedDate", pREQAPPL_Info.ApprovedDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncCreate", pREQAPPL_Info.SyncCreate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncLastUpd", pREQAPPL_Info.SyncLastUpd, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastSyncBy", pREQAPPL_Info.LastSyncBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);

                strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert, "REQAPPL", string.Empty);

                lstSQL.Add(strSQL);

                rValue = objDCom.BatchExecute(lstSQL, CommandType.Text, true, false);
                if (rValue == false)
                {
                    return null;
                }
                return GetSingleAG_BLACKLIST(pAG_BLACKLIST_Info.AgentID);
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }

        public bool SetWhiteListApprove(string AgentID, string UserID, RequestApp pREQAPPL_Info)
        {
            ArrayList lstSQL = new ArrayList();
            String strSQL = string.Empty;
            bool rValue = false;
            if (AgentID == string.Empty) { return false; }
            try
            {
                if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
                {
                    StartSQLControl();
                    strSQL = "UPDATE AG_BLACKLIST SET AG_BLACKLIST.BlacklistExpiryDate = DATEADD(day,-1,GETDATE()), AG_BLACKLIST.LastSyncBy = '" +
                             objSQL.ParseValue(SQLControl.EnumDataType.dtString, UserID) + "' WHERE AG_BLACKLIST.AgentID = '" +
                             objSQL.ParseValue(SQLControl.EnumDataType.dtString, AgentID) + "'";
                    lstSQL.Add(strSQL);
                    strSQL = "Update EN_ENQUIRYLOG set NoOfAttempt = 0 where EnquiryDate >= DATEADD(day,-1,GetDate()) and AgentID = '" +
                             objSQL.ParseValue(SQLControl.EnumDataType.dtString, AgentID) + "'";
                    lstSQL.Add(strSQL);
                    //added by agus to restore suspend after whitelist -> SuspendExpiry = DATEADD(day,-1,GETDATE())
                    strSQL = "Update EN_SUSPENDLIST set SuspendAttempt = 0, SuspendExpiry = DATEADD(day,-1,GETDATE()) where SuspendDate >= DATEADD(day,-1,GetDate()) and AgentID = '" +
                             objSQL.ParseValue(SQLControl.EnumDataType.dtString, AgentID) + "'";
                    lstSQL.Add(strSQL);

                    objSQL.AddField("ReqID", pREQAPPL_Info.ReqID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("UserID", pREQAPPL_Info.UserID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("ReqType", pREQAPPL_Info.ReqType, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("TransID", pREQAPPL_Info.TransID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("RequestDate", pREQAPPL_Info.RequestDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("ExpiryDate", pREQAPPL_Info.ExpiryDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("Remark", pREQAPPL_Info.Remark, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("ApprovedBy", pREQAPPL_Info.ApprovedBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("ApprovedDate", pREQAPPL_Info.ApprovedDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("SyncCreate", pREQAPPL_Info.SyncCreate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("SyncLastUpd", pREQAPPL_Info.SyncLastUpd, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("LastSyncBy", pREQAPPL_Info.LastSyncBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);

                    strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "REQAPPL", "REQAPPL.ReqID='" + pREQAPPL_Info.ReqID + "'");

                    lstSQL.Add(strSQL);

                    rValue = objDCom.BatchExecute(lstSQL, CommandType.Text, true, false);
                    return rValue;
                }
                return rValue;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return rValue;
            }
        }
        public bool SetAdminWhiteList(string AgentID, string UserID, RequestApp pREQAPPL_Info)
        {
            ArrayList lstSQL = new ArrayList();
            String strSQL = string.Empty;
            bool rValue = false;
            if (AgentID == string.Empty) { return false; }
            try
            {
                if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
                {
                    StartSQLControl();
                    strSQL = "UPDATE AG_BLACKLIST SET AG_BLACKLIST.BlacklistExpiryDate = DATEADD(day,-1,GETDATE()), AG_BLACKLIST.LastSyncBy = '" +
                             objSQL.ParseValue(SQLControl.EnumDataType.dtString, UserID) + "' WHERE AG_BLACKLIST.AgentID = '" +
                             objSQL.ParseValue(SQLControl.EnumDataType.dtString, AgentID) + "'";
                    lstSQL.Add(strSQL);
                    strSQL = "Update EN_ENQUIRYLOG set NoOfAttempt = 0 where EnquiryDate >= DATEADD(day,-1,GetDate()) and AgentID = '" +
                             objSQL.ParseValue(SQLControl.EnumDataType.dtString, AgentID) + "'";
                    lstSQL.Add(strSQL);
                    //added by agus to restore suspend after whitelist -> SuspendExpiry = DATEADD(day,-1,GETDATE())
                    strSQL = "Update EN_SUSPENDLIST set SuspendAttempt = 0, SuspendExpiry = DATEADD(day,-1,GETDATE()) where SuspendDate >= DATEADD(day,-1,GetDate()) and AgentID = '" +
                             objSQL.ParseValue(SQLControl.EnumDataType.dtString, AgentID) + "'";
                    lstSQL.Add(strSQL);


                    objSQL.AddField("ReqID", pREQAPPL_Info.ReqID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("UserID", pREQAPPL_Info.UserID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("ReqType", pREQAPPL_Info.ReqType, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("TransID", pREQAPPL_Info.TransID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("RequestDate", pREQAPPL_Info.RequestDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("ExpiryDate", pREQAPPL_Info.ExpiryDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("Remark", pREQAPPL_Info.Remark, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("ApprovedBy", pREQAPPL_Info.ApprovedBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("ApprovedDate", pREQAPPL_Info.ApprovedDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("SyncCreate", pREQAPPL_Info.SyncCreate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("SyncLastUpd", pREQAPPL_Info.SyncLastUpd, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("LastSyncBy", pREQAPPL_Info.LastSyncBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);

                    strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert, "REQAPPL", "REQAPPL.ReqID='" + pREQAPPL_Info.ReqID + "'");

                    lstSQL.Add(strSQL);

                    rValue = objDCom.BatchExecute(lstSQL, CommandType.Text, true, false);
                    return rValue;
                }
                return rValue;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return rValue;
            }
        }
        public DataTable GetAllBlackWhite(string pAgID)
        {

            DataTable dt;

            String strSQL = string.Empty;

            try
            {
                strSQL = "SELECT req.ReqID, req.ReqType, CODEMASTER.CodeDesc,req.RequestDate, req.Remark,req.TransID,req.UserID,req.ApprovedDate,req.ApprovedBy " +
                  "FROM REQAPPL req JOIN CODEMASTER (NOLOCK) on Code=ReqType AND CodeType='REQ' " +
                  "WHERE TransID='" + pAgID + "' AND (req.ReqType='W' OR req.ReqType='B')";
                dt = objDCom.Execute(strSQL, CommandType.Text, true);
                if (dt != null && dt.Rows.Count > 0)
                {

                    return dt;
                }
                else
                {
                    return null;
                    throw new ApplicationException("Agent does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }


        public RequestApp GetSingleREQAPPL(
               string pReqID)
        {

            RequestApp objREQAPPL_Info;
            DataTable dt;
            DateTime dateValue;
            String strSQL = string.Empty;
            String strFields = string.Empty;
            String strFilter = string.Empty;
            List<string> lstFields = new List<string>();
            try
            {
                lstFields.Add("REQAPPL.ReqID");
                lstFields.Add("REQAPPL.UserID");
                lstFields.Add("REQAPPL.ReqType");
                lstFields.Add("REQAPPL.TransID");
                lstFields.Add("REQAPPL.RequestDate");
                lstFields.Add("REQAPPL.ExpiryDate");
                lstFields.Add("REQAPPL.Remark");
                lstFields.Add("REQAPPL.ApprovedBy");
                lstFields.Add("REQAPPL.ApprovedDate");
                lstFields.Add("REQAPPL.rowguid");
                lstFields.Add("REQAPPL.SyncCreate");
                lstFields.Add("REQAPPL.SyncLastUpd");
                lstFields.Add("REQAPPL.LastSyncBy");
                lstFields.Add("CODEMASTER.CodeDesc");
                strFields = GetSqlFields(lstFields);
                strFilter = "WHERE REQAPPL.ReqID='" + pReqID + "'";
                strSQL = "SELECT " + strFields + " FROM REQAPPL JOIN CODEMASTER (NOLOCK) on Code=REQAPPL.ReqType AND CodeType='REQ' " + strFilter;
                dt = objDCom.Execute(strSQL, CommandType.Text, true);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow drRow = dt.Rows[0];

                    objREQAPPL_Info = new RequestApp();
                    objREQAPPL_Info.ReqID = (string)drRow["ReqID"];
                    objREQAPPL_Info.UserID = (string)drRow["UserID"];
                    objREQAPPL_Info.ReqType = (string)drRow["ReqType"];
                    objREQAPPL_Info.RequestDesc = (string)drRow["CodeDesc"];
                    objREQAPPL_Info.TransID = (string)drRow["TransID"];
                    if (DateTime.TryParse(drRow["RequestDate"].ToString(), out dateValue)) objREQAPPL_Info.RequestDate = (DateTime)drRow["RequestDate"];
                    if (DateTime.TryParse(drRow["ExpiryDate"].ToString(), out dateValue)) objREQAPPL_Info.ExpiryDate = (DateTime)drRow["ExpiryDate"];
                    objREQAPPL_Info.Remark = (string)drRow["Remark"];
                    objREQAPPL_Info.ApprovedBy = drRow["ApprovedBy"].ToString();
                    if (DateTime.TryParse(drRow["ApprovedDate"].ToString(), out dateValue)) objREQAPPL_Info.ApprovedDate = (DateTime)drRow["ApprovedDate"];
                    objREQAPPL_Info.rowguid = (Guid)drRow["rowguid"];
                    if (DateTime.TryParse(drRow["SyncCreate"].ToString(), out dateValue)) objREQAPPL_Info.SyncCreate = (DateTime)drRow["SyncCreate"];
                    if (DateTime.TryParse(drRow["SyncLastUpd"].ToString(), out dateValue)) objREQAPPL_Info.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
                    objREQAPPL_Info.LastSyncBy = drRow["LastSyncBy"].ToString();
                    return objREQAPPL_Info;
                }
                else
                {
                    return null;
                    throw new ApplicationException("REQAPPL does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }

        public DataTable SearchAgentBlacklist(string fieldName, string filter)
        {
            String strSQL = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
                {
                    if (fieldName == "All")
                    {
                        fieldName = "AG_PROFILE.AgentID";
                        filter = "";
                    }
                    else
                    {
                        fieldName = "AG_PROFILE." + fieldName;
                    }
                    StartSQLControl();
                    strSQL = "SELECT DISTINCT AG_PROFILE.AgentID,AG_PROFILE.Username,BlacklistDate,BlacklistExpiryDate,Remark,(AG_PROFILE.ContactFirstName+' '+AG_PROFILE.ContactLastName) as FullName,AG_PROFILE.ContactFirstName,AG_PROFILE.ContactLastName,AG_PROFILE.Email," +
                    "AG_PROFILE.Country,COUNTRYCODE.countryName,AG_PROFILE.LicenseNo,AG_PROFILE.MobileNo,AG_PROFILE.PhoneNo FROM AG_PROFILE,AG_BLACKLIST,COUNTRYCODE," +
                    "AG_CATEGORY WHERE " + fieldName + " like '%" + filter + "%' and AG_PROFILE.flag = 0 AND AG_PROFILE.Status = 1 AND AG_BLACKLIST.AgentID = AG_PROFILE.AgentID " +
                    "AND AG_PROFILE.AgentCatgID = AG_CATEGORY.AgentCatgID AND AG_CATEGORY.AgentCatgID != 99 AND AG_BLACKLIST.BlacklistExpiryDate >= GETDATE() AND AG_PROFILE.Country = COUNTRYCODE.CountryCode";
                    dt = objDCom.Execute(strSQL, System.Data.CommandType.Text, false);
                    if (dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }

        public DataTable SearchAgentBlacklistData(string AgentID)
        {
            String strSQL = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
                {
                    StartSQLControl();
                    strSQL = "SELECT DISTINCT AG_PROFILE.AgentID,AG_PROFILE.Address1,AG_PROFILE.JoinDate,AG_PROFILE.Username,BlacklistDate,BlacklistExpiryDate,Remark,(AG_PROFILE.ContactFirstName+' '+AG_PROFILE.ContactLastName) as FullName,AG_PROFILE.ContactFirstName,AG_PROFILE.ContactLastName,AG_PROFILE.Email," +
                    "AG_PROFILE.Country,COUNTRYCODE.countryName,AG_PROFILE.LicenseNo,AG_PROFILE.MobileNo,AG_PROFILE.PhoneNo FROM AG_PROFILE,AG_BLACKLIST,COUNTRYCODE," +
                    "AG_CATEGORY WHERE AG_PROFILE.flag = 0 AND AG_PROFILE.Status = 1 AND AG_BLACKLIST.AgentID = AG_PROFILE.AgentID " +
                    "AND AG_PROFILE.AgentCatgID = AG_CATEGORY.AgentCatgID AND AG_CATEGORY.AgentCatgID != 99 AND AG_BLACKLIST.BlacklistExpiryDate >= GETDATE() AND AG_PROFILE.Country = COUNTRYCODE.CountryCode AND AG_PROFILE.AgentID='" + AgentID + "'";
                    dt = objDCom.Execute(strSQL, System.Data.CommandType.Text, false);
                    if (dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }

        public DataTable SearchAgentBlacklistRequest(string fieldName, string filter)
        {
            String strSQL = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
                {
                    if (fieldName == "All")
                    {
                        fieldName = "AG_PROFILE.AgentID";
                        filter = "";
                    }
                    else
                    {
                        fieldName = "AG_PROFILE." + fieldName;
                    }
                    StartSQLControl();
                    strSQL = "SELECT DISTINCT AG_PROFILE.AgentID,AG_PROFILE.Username,BlacklistDate,TransID,BlacklistExpiryDate,AG_BLACKLIST.Remark,(AG_PROFILE.ContactFirstName+' '+AG_PROFILE.ContactLastName) as FullName,AG_PROFILE.ContactFirstName,AG_PROFILE.ContactLastName,AG_PROFILE.Email," +
                    "AG_PROFILE.Country,COUNTRYCODE.countryName,AG_PROFILE.LicenseNo,AG_PROFILE.MobileNo,AG_PROFILE.PhoneNo FROM AG_PROFILE JOIN " +
                    "AG_BLACKLIST (NOLOCK) on AG_BLACKLIST.AgentID = AG_PROFILE.AgentID JOIN COUNTRYCODE (NOLOCK) on AG_PROFILE.Country = COUNTRYCODE.CountryCode JOIN " +
                    "AG_CATEGORY (NOLOCK) on AG_PROFILE.AgentCatgID = AG_CATEGORY.AgentCatgID LEFT JOIN REQAPPL (NOLOCK) on AG_PROFILE.AgentID=TransID AND ReqType='W' AND ApprovedBy IS NULL AND ExpiryDate>GETDATE() WHERE " + fieldName + " like '%" + filter + "%' AND AG_PROFILE.flag = 0 AND AG_PROFILE.Status = 1 " +
                    " AND AG_BLACKLIST.BlacklistExpiryDate > GETDATE() AND TransID IS NULL";
                    //AND AG_CATEGORY.AgentCatgID != 99
                    dt = objDCom.Execute(strSQL, System.Data.CommandType.Text, false);
                    if (dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }


        public DataTable SearchAdminList(string fieldName, string filter)
        {
            String strSQL = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
                {
                    if (fieldName == "All")
                    {
                        fieldName = "USRPROFILE.UserID";
                        filter = "";
                    }
                    else
                    {
                        fieldName = "USRPROFILE." + fieldName;
                    }
                    StartSQLControl();
                    strSQL = "SELECT USRPROFILE.UserID, USRPROFILE.UserName, USRPROFILE.Password, USRPROFILE.RefID, USRPROFILE.RefType, " +
                             "USRPROFILE.ParentID, USRPROFILE.Status, USRPROFILE.Logged, USRPROFILE.LogStation, USRPROFILE.LastLogin, " +
                             "USRPROFILE.LastLogout, USRPROFILE.CreateDate, USRPROFILE.CreateBy, USRPROFILE.LastUpdate, USRPROFILE.UpdateBy, " +
                             "USRPROFILE.OperationGroup,USRAPP.AccessCode,USRGROUP.GroupName FROM USRPROFILE JOIN USRAPP (NOLOCK) ON USRAPP.UserID=USRPROFILE.UserID JOIN " +
                             "USRGROUP (NOLOCK) ON USRAPP.AccessCode=USRGROUP.GroupCode WHERE " + fieldName + " like '%" + filter + "%' AND USRPROFILE.Status = 1";
                    dt = objDCom.Execute(strSQL, System.Data.CommandType.Text, false);
                    if (dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }

        public DataTable GetSingleAdmin(string UserID)
        {
            String strSQL = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
                {
                    StartSQLControl();
                    strSQL = "SELECT DISTINCT  USRPROFILE.UserID, USRPROFILE.UserName, USRPROFILE.Password, USRPROFILE.RefID, USRPROFILE.RefType, USRPROFILE.ParentID, USRPROFILE.Status, USRPROFILE.Logged, USRPROFILE.LogStation, USRPROFILE.LastLogin, USRPROFILE.LastLogout, USRPROFILE.CreateDate, USRPROFILE.CreateBy, USRPROFILE.LastUpdate, USRPROFILE.UpdateBy,USRPROFILE.rowguid, USRPROFILE.SyncCreate,USRPROFILE.OperationGroup, USRPROFILE.SyncLastUpd, USRPROFILE.IsHost, USRPROFILE.LastSyncBy, USRPROFILE.SyncCreateBy,USRAPP.AccessCode,USRGROUP.GroupName,USRPROFILE.OperationGroup FROM USRPROFILE JOIN USRAPP (NOLOCK) on USRAPP.UserID=USRPROFILE.UserID JOIN USRGROUP (NOLOCK) on USRAPP.AccessCode=USRGROUP.GroupCode WHERE USRPROFILE.UserID='" + UserID + "'";
                    dt = objDCom.Execute(strSQL, System.Data.CommandType.Text, false);
                    if (dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }



        public bool VerifyBlackList(string AgentID)
        {
            String strSQL = string.Empty;
            String strFields = string.Empty;
            String strFilter = string.Empty;
            int intCount;
            if (AgentID == string.Empty) { return false; }
            try
            {
                strFilter = " WHERE AG_BLACKLIST.AgentID='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, AgentID)
                            + "' AND AG_BLACKLIST.BlacklistExpiryDate >= GETDATE()";
                strSQL = "SELECT COUNT(*) FROM AG_BLACKLIST " + strFilter;
                intCount = Convert.ToInt32(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtScalar, System.Data.CommandType.Text));
                if (intCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return false;
            }
        }

        public string CheckBlacklistExist(
        string pAgentID)
        {
            objSQL.ClearFields();
            objSQL.ClearCondtions();
            AgentBlacklistLog objAG_BLACKLIST_Info;
            DataTable dt;
            DateTime dateValue;
            String strSQL = string.Empty;
            String strFields = string.Empty;
            String strFilter = string.Empty;
            List<string> lstFields = new List<string>();
            try
            {
                lstFields.Add("AG_BLACKLIST.BlacklistID");

                strFields = GetSqlFields(lstFields);
                strFilter = " WHERE AG_BLACKLIST.AgentID='" + pAgentID + "'";
                strSQL = "SELECT " + strFields + " FROM AG_BLACKLIST " + strFilter;
                dt = objDCom.Execute(strSQL, CommandType.Text, true);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow drRow = dt.Rows[0];
                    return (string)drRow["BlacklistID"];
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return "";
            }
        }

        //public List<AgentActivity> GetAllAG_ACTIVITY()
        //{
        //    AgentActivity objAG_ACTIVITY_Info;
        //    List<AgentActivity> objListAG_ACTIVITY_Info = new List<AgentActivity>();
        //    DataTable dt;
        //    DateTime dateValue;
        //    String strSQL = string.Empty;

        //    try
        //    {
        //        strSQL = "SELECT * FROM AG_ACTIVITY ";
        //        dt = objDCom.Execute(strSQL, CommandType.Text, true);
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow drRow in dt.Rows)
        //            {
        //                objAG_ACTIVITY_Info = new AgentActivity();
        //                objAG_ACTIVITY_Info.AgentID = (string)drRow["AgentID"];
        //                if (DateTime.TryParse(drRow["LastLoginDate"].ToString(), out dateValue)) objAG_ACTIVITY_Info.LastLoginDate = (DateTime)drRow["LastLoginDate"];
        //                if (DateTime.TryParse(drRow["LastFailedLoginDate"].ToString(), out dateValue)) objAG_ACTIVITY_Info.LastFailedLoginDate = (DateTime)drRow["LastFailedLoginDate"];
        //                objAG_ACTIVITY_Info.LastFailedLoginAttempt = (int)drRow["LastFailedLoginAttempt"];
        //                objAG_ACTIVITY_Info.TotalFailedLoginAttempt = (int)drRow["TotalFailedLoginAttempt"];
        //                if (DateTime.TryParse(drRow["ExpiryBlacklistDate"].ToString(), out dateValue)) objAG_ACTIVITY_Info.ExpiryBlacklistDate = (DateTime)drRow["ExpiryBlacklistDate"];
        //                if (DateTime.TryParse(drRow["LastSuspend"].ToString(), out dateValue)) objAG_ACTIVITY_Info.LastSuspend = (DateTime)drRow["LastSuspend"];
        //                if (DateTime.TryParse(drRow["LastBlacklist"].ToString(), out dateValue)) objAG_ACTIVITY_Info.LastBlacklist = (DateTime)drRow["LastBlacklist"];
        //                objAG_ACTIVITY_Info.rowguid = (Guid)drRow["rowguid"];
        //                if (DateTime.TryParse(drRow["SyncCreate"].ToString(), out dateValue)) objAG_ACTIVITY_Info.SyncCreate = (DateTime)drRow["SyncCreate"];
        //                if (DateTime.TryParse(drRow["SyncLastUpd"].ToString(), out dateValue)) objAG_ACTIVITY_Info.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
        //                objListAG_ACTIVITY_Info.Add(objAG_ACTIVITY_Info);
        //            }
        //            return objListAG_ACTIVITY_Info;
        //        }
        //        else
        //        {
        //            return null;
        //            throw new ApplicationException("AG_ACTIVITY does not exist.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //public bool CheckIsExist(
        //        string pAgentID)
        //{
        //    objSQL.ClearFields();
        //    objSQL.ClearCondtions();
        //    AgentActivity objAG_ACTIVITY_Info;
        //    DataTable dt;
        //    DateTime dateValue;
        //    String strSQL = string.Empty;
        //    String strFields = string.Empty;
        //    String strFilter = string.Empty;
        //    List<string> lstFields = new List<string>();
        //    try
        //    {
        //        strFilter = "WHERE AG_ACTIVITY.AgentID='" + pAgentID + "'";
        //        strSQL = "SELECT AG_ACTIVITY.AgentID FROM AG_ACTIVITY " + strFilter;
        //        dt = objDCom.Execute(strSQL, CommandType.Text, true);

        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //            throw new ApplicationException("AG_ACTIVITY does not exist.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        //public int GetTotalFailedLogin(
        //        string pAgentID)
        //{
        //    DataTable dt;
        //    String strSQL = string.Empty;
        //    String strFields = string.Empty;
        //    String strFilter = string.Empty;
        //    List<string> lstFields = new List<string>();
        //    try
        //    {
        //        strFilter = "WHERE AG_ACTIVITY.AgentID='" + pAgentID + "'";
        //        strSQL = "SELECT AG_ACTIVITY.TotalFailedLoginAttempt FROM AG_ACTIVITY " + strFilter;
        //        dt = objDCom.Execute(strSQL, CommandType.Text, true);

        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            return Convert.ToInt16(dt.Rows[0]["TotalFailedLoginAttempt"]);
        //        }
        //        else
        //        {
        //            return 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //}

        public AgentActivity GetSingleAG_ACTIVITY(
                string pAgentID)
        {
            AgentActivity objAG_ACTIVITY_Info;
            DataTable dt;
            DateTime dateValue;
            String strSQL = string.Empty;
            String strFields = string.Empty;
            String strFilter = string.Empty;
            List<string> lstFields = new List<string>();
            try
            {
                lstFields.Add("AG_ACTIVITY.AgentID");
                lstFields.Add("AG_ACTIVITY.LastLoginDate");
                lstFields.Add("AG_ACTIVITY.LastFailedLoginDate");
                lstFields.Add("AG_ACTIVITY.LastFailedLoginAttempt");
                lstFields.Add("AG_ACTIVITY.TotalFailedLoginAttempt");
                lstFields.Add("AG_ACTIVITY.ExpiryBlacklistDate");
                lstFields.Add("AG_ACTIVITY.LastSuspend");
                lstFields.Add("AG_ACTIVITY.LastBlacklist");
                lstFields.Add("AG_ACTIVITY.rowguid");
                lstFields.Add("AG_ACTIVITY.SyncCreate");
                lstFields.Add("AG_ACTIVITY.SyncLastUpd");

                //strFields = GetSqlFields(lstFields);
                strFilter = "WHERE AG_ACTIVITY.AgentID='" + pAgentID + "'";
                strSQL = "SELECT " + strFields + " FROM AG_ACTIVITY " + strFilter;
                dt = objDCom.Execute(strSQL, CommandType.Text, true);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow drRow = dt.Rows[0];

                    objAG_ACTIVITY_Info = new AgentActivity();
                    objAG_ACTIVITY_Info.AgentID = (string)drRow["AgentID"];
                    if (DateTime.TryParse(drRow["LastLoginDate"].ToString(), out dateValue)) objAG_ACTIVITY_Info.LastLoginDate = (DateTime)drRow["LastLoginDate"];
                    if (DateTime.TryParse(drRow["LastFailedLoginDate"].ToString(), out dateValue)) objAG_ACTIVITY_Info.LastFailedLoginDate = (DateTime)drRow["LastFailedLoginDate"];
                    objAG_ACTIVITY_Info.LastFailedLoginAttempt = (int)drRow["LastFailedLoginAttempt"];
                    objAG_ACTIVITY_Info.TotalFailedLoginAttempt = (int)drRow["TotalFailedLoginAttempt"];
                    if (DateTime.TryParse(drRow["ExpiryBlacklistDate"].ToString(), out dateValue)) objAG_ACTIVITY_Info.ExpiryBlacklistDate = (DateTime)drRow["ExpiryBlacklistDate"];
                    if (DateTime.TryParse(drRow["LastSuspend"].ToString(), out dateValue)) objAG_ACTIVITY_Info.LastSuspend = (DateTime)drRow["LastSuspend"];
                    if (DateTime.TryParse(drRow["LastBlacklist"].ToString(), out dateValue)) objAG_ACTIVITY_Info.LastBlacklist = (DateTime)drRow["LastBlacklist"];
                    objAG_ACTIVITY_Info.rowguid = (Guid)drRow["rowguid"];
                    if (DateTime.TryParse(drRow["SyncCreate"].ToString(), out dateValue)) objAG_ACTIVITY_Info.SyncCreate = (DateTime)drRow["SyncCreate"];
                    if (DateTime.TryParse(drRow["SyncLastUpd"].ToString(), out dateValue)) objAG_ACTIVITY_Info.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
                    return objAG_ACTIVITY_Info;
                }
                else
                {
                    return null;
                    throw new ApplicationException("AG_ACTIVITY does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }

        public AgentActivity SaveAG_ACTIVITY(AgentActivity pAG_ACTIVITY_Info, EnumSaveType pType)
        {
            bool rValue = false;
            ArrayList lstSQL = new ArrayList();
            string strSQL = string.Empty;
            try
            {
                objSQL.AddField("AgentID", pAG_ACTIVITY_Info.AgentID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastLoginDate", pAG_ACTIVITY_Info.LastLoginDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastFailedLoginDate", pAG_ACTIVITY_Info.LastFailedLoginDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastFailedLoginAttempt", pAG_ACTIVITY_Info.LastFailedLoginAttempt, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("TotalFailedLoginAttempt", pAG_ACTIVITY_Info.TotalFailedLoginAttempt, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("ExpiryBlacklistDate", pAG_ACTIVITY_Info.ExpiryBlacklistDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastSuspend", pAG_ACTIVITY_Info.LastSuspend, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastBlacklist", pAG_ACTIVITY_Info.LastBlacklist, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncCreate", pAG_ACTIVITY_Info.SyncCreate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncLastUpd", pAG_ACTIVITY_Info.SyncLastUpd, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                switch (pType)
                {
                    case EnumSaveType.Insert:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert, "AG_ACTIVITY", string.Empty);
                        break;
                    case EnumSaveType.Update:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "AG_ACTIVITY", "AG_ACTIVITY.AgentID='" + pAG_ACTIVITY_Info.AgentID + "'");
                        break;
                }
                lstSQL.Add(strSQL);
                rValue = objDCom.BatchExecute(lstSQL, CommandType.Text, true, false);
                if (rValue == false)
                {
                    return null;
                }
                return GetSingleAG_ACTIVITY(pAG_ACTIVITY_Info.AgentID);
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }

        public List<AgentBlacklistLog> GetAllAG_BLACKLIST()
        {
            AgentBlacklistLog objAG_BLACKLIST_Info;
            List<AgentBlacklistLog> objListAG_BLACKLIST_Info = new List<AgentBlacklistLog>();
            DataTable dt;
            DateTime dateValue;
            String strSQL = string.Empty;

            try
            {
                strSQL = "SELECT * FROM AG_BLACKLIST ";
                dt = objDCom.Execute(strSQL, CommandType.Text, true);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow drRow in dt.Rows)
                    {
                        objAG_BLACKLIST_Info = new AgentBlacklistLog();
                        objAG_BLACKLIST_Info.BlacklistID = (string)drRow["BlacklistID"];
                        objAG_BLACKLIST_Info.AgentID = (string)drRow["AgentID"];
                        if (DateTime.TryParse(drRow["BlacklistDate"].ToString(), out dateValue)) objAG_BLACKLIST_Info.BlacklistDate = (DateTime)drRow["BlacklistDate"];
                        if (DateTime.TryParse(drRow["BlacklistExpiryDate"].ToString(), out dateValue)) objAG_BLACKLIST_Info.BlacklistExpiryDate = (DateTime)drRow["BlacklistExpiryDate"];
                        objAG_BLACKLIST_Info.Status = (byte)drRow["Status"];
                        objAG_BLACKLIST_Info.rowguid = (Guid)drRow["rowguid"];
                        if (DateTime.TryParse(drRow["SyncCreate"].ToString(), out dateValue)) objAG_BLACKLIST_Info.SyncCreate = (DateTime)drRow["SyncCreate"];
                        if (DateTime.TryParse(drRow["SyncLastUpd"].ToString(), out dateValue)) objAG_BLACKLIST_Info.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
                        objAG_BLACKLIST_Info.LastSyncBy = (string)drRow["LastSyncBy"];
                        objAG_BLACKLIST_Info.Remark = (string)drRow["Remark"];
                        objListAG_BLACKLIST_Info.Add(objAG_BLACKLIST_Info);
                    }
                    return objListAG_BLACKLIST_Info;
                }
                else
                {
                    return null;
                    throw new ApplicationException("AG_BLACKLIST does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }


        public AgentBlacklistLog GetSingleAG_BLACKLIST(string pAgentID)
        {
            AgentBlacklistLog objAG_BLACKLIST_Info;
            DataTable dt;
            DateTime dateValue;
            String strSQL = string.Empty;
            String strFields = string.Empty;
            String strFilter = string.Empty;
            List<string> lstFields = new List<string>();
            try
            {
                lstFields.Add("AG_BLACKLIST.BlacklistID");
                lstFields.Add("AG_BLACKLIST.AgentID");
                lstFields.Add("AG_BLACKLIST.BlacklistDate");
                lstFields.Add("AG_BLACKLIST.BlacklistExpiryDate");
                lstFields.Add("AG_BLACKLIST.Status");
                lstFields.Add("AG_BLACKLIST.rowguid");
                lstFields.Add("AG_BLACKLIST.SyncCreate");
                lstFields.Add("AG_BLACKLIST.SyncLastUpd");
                lstFields.Add("AG_BLACKLIST.LastSyncBy");
                lstFields.Add("AG_BLACKLIST.Remark");

                strFields = GetSqlFields(lstFields);
                strFilter = " WHERE AG_BLACKLIST.AgentID='" + pAgentID + "'";
                strSQL = "SELECT " + strFields + " FROM AG_BLACKLIST " + strFilter;
                dt = objDCom.Execute(strSQL, CommandType.Text, true);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow drRow = dt.Rows[0];

                    objAG_BLACKLIST_Info = new AgentBlacklistLog();
                    objAG_BLACKLIST_Info.BlacklistID = (string)drRow["BlacklistID"];
                    objAG_BLACKLIST_Info.AgentID = (string)drRow["AgentID"];
                    if (DateTime.TryParse(drRow["BlacklistDate"].ToString(), out dateValue)) objAG_BLACKLIST_Info.BlacklistDate = (DateTime)drRow["BlacklistDate"];
                    if (DateTime.TryParse(drRow["BlacklistExpiryDate"].ToString(), out dateValue)) objAG_BLACKLIST_Info.BlacklistExpiryDate = (DateTime)drRow["BlacklistExpiryDate"];
                    objAG_BLACKLIST_Info.Status = (byte)drRow["Status"];
                    objAG_BLACKLIST_Info.rowguid = (Guid)drRow["rowguid"];
                    if (DateTime.TryParse(drRow["SyncCreate"].ToString(), out dateValue)) objAG_BLACKLIST_Info.SyncCreate = (DateTime)drRow["SyncCreate"];
                    if (DateTime.TryParse(drRow["SyncLastUpd"].ToString(), out dateValue)) objAG_BLACKLIST_Info.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
                    objAG_BLACKLIST_Info.LastSyncBy = (string)drRow["LastSyncBy"];
                    objAG_BLACKLIST_Info.Remark = (string)drRow["Remark"];
                    return objAG_BLACKLIST_Info;
                }
                else
                {
                    return null;
                    throw new ApplicationException("AG_BLACKLIST does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }

        public AgentBlacklistLog SaveAG_BLACKLIST(AgentBlacklistLog pAG_BLACKLIST_Info, EnumSaveType pType)
        {
            bool rValue = false;
            ArrayList lstSQL = new ArrayList();
            string strSQL = string.Empty;
            try
            {
                objSQL.AddField("BlacklistID", pAG_BLACKLIST_Info.BlacklistID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("AgentID", pAG_BLACKLIST_Info.AgentID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("BlacklistDate", pAG_BLACKLIST_Info.BlacklistDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("BlacklistExpiryDate", pAG_BLACKLIST_Info.BlacklistExpiryDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Status", pAG_BLACKLIST_Info.Status, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncCreate", pAG_BLACKLIST_Info.SyncCreate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncLastUpd", pAG_BLACKLIST_Info.SyncLastUpd, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastSyncBy", pAG_BLACKLIST_Info.LastSyncBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Remark", pAG_BLACKLIST_Info.Remark, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                switch (pType)
                {
                    case EnumSaveType.Insert:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert, "AG_BLACKLIST", string.Empty);
                        break;
                    case EnumSaveType.Update:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "AG_BLACKLIST", "AG_BLACKLIST.BlacklistID='" + pAG_BLACKLIST_Info.BlacklistID + "' and AG_BLACKLIST.AgentID='" + pAG_BLACKLIST_Info.AgentID + "'");
                        break;
                }
                lstSQL.Add(strSQL);
                rValue = objDCom.BatchExecute(lstSQL, CommandType.Text, true, false);
                if (rValue == false)
                {
                    return null;
                }
                return GetSingleAG_BLACKLIST(pAG_BLACKLIST_Info.AgentID);
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }

        public DataTable GetAllActiveAgent()
        {
            String strSQL = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
                {
                    StartSQLControl();

                    strSQL = "SELECT DISTINCT AG_PROFILE.AgentID,AG_PROFILE.Username,AG_PROFILE.ContactFirstName,AG_PROFILE.ContactLastName,AG_PROFILE.Email, " +
                             "AG_PROFILE.Country,COUNTRYCODE.countryName,AG_PROFILE.LicenseNo,AG_PROFILE.MobileNo,AG_PROFILE.PhoneNo FROM AG_PROFILE,COUNTRYCODE " +
                             "WHERE AG_PROFILE.flag = 0 AND AG_PROFILE.Country = COUNTRYCODE.CountryCode";

                    //strSQL = "SELECT AgentID, AgentCatgID, Username, LicenseNo, Password, Address1, Address2, Country, City, State, Postcode, Title, ContactFirstName, ContactLastName," +
                    //         " PhoneNo, MobileNo, Fax, Email, JoinDate, LastModifyDate, Status, Flag, rowguid, SyncCreate, SyncLastUpd, LastSyncBy" +
                    //         " FROM AG_PROFILE WHERE flag = 0";
                    dt = objDCom.Execute(strSQL, System.Data.CommandType.Text, false);
                    if (dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }



    }

    public partial class AgentBankControl : Shared.CoreBase
    {
        SystemLog SystemLog = new SystemLog();
        public enum SaveType
        {
            Insert = 0,
            Update = 1
        }

        public List<AgentBankInfo> GetAllAG_BANKINFO()
        {
            AgentBankInfo objAG_BANKINFO_Info;
            List<AgentBankInfo> objListAG_BANKINFO_Info = new List<AgentBankInfo>();
            DataTable dt;
            DateTime dateValue;
            String strSQL = string.Empty;

            try
            {
                strSQL = "SELECT * FROM AG_BANKINFO ";
                dt = objDCom.Execute(strSQL, CommandType.Text, true);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow drRow in dt.Rows)
                    {
                        objAG_BANKINFO_Info = new AgentBankInfo();
                        objAG_BANKINFO_Info.AgentID = (string)drRow["AgentID"];
                        objAG_BANKINFO_Info.BankName = (string)drRow["BankName"];
                        objAG_BANKINFO_Info.Address1 = (string)drRow["Address1"];
                        objAG_BANKINFO_Info.Address2 = (string)drRow["Address2"];
                        objAG_BANKINFO_Info.Address3 = (string)drRow["Address3"];
                        objAG_BANKINFO_Info.Country = (string)drRow["Country"];
                        objAG_BANKINFO_Info.City = (string)drRow["City"];
                        objAG_BANKINFO_Info.State = (string)drRow["State"];
                        objAG_BANKINFO_Info.Postcode = (string)drRow["Postcode"];
                        objAG_BANKINFO_Info.AccountName = (string)drRow["AccountName"];
                        objAG_BANKINFO_Info.AccountNo = (string)drRow["AccountNo"];
                        objAG_BANKINFO_Info.rowguid = (Guid)drRow["rowguid"];
                        if (DateTime.TryParse(drRow["SyncCreate"].ToString(), out dateValue)) objAG_BANKINFO_Info.SyncCreate = (DateTime)drRow["SyncCreate"];
                        if (DateTime.TryParse(drRow["SyncLastUpd"].ToString(), out dateValue)) objAG_BANKINFO_Info.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
                        objAG_BANKINFO_Info.LastSyncBy = (string)drRow["LastSyncBy"];
                        objListAG_BANKINFO_Info.Add(objAG_BANKINFO_Info);
                    }
                    return objListAG_BANKINFO_Info;
                }
                else
                {
                    return null;
                    throw new ApplicationException("AG_BANKINFO does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }

        public AgentBankInfo GetAgentBankInfo(string pAgentID)
        {
            AgentBankInfo objAG_BANKINFO_Info;
            DataTable dt;
            DateTime dateValue;
            String strSQL = string.Empty;
            String strFields = string.Empty;
            String strFilter = string.Empty;
            List<string> lstFields = new List<string>();
            try
            {
                lstFields.Add("AG_BANKINFO.AgentID");
                lstFields.Add("AG_BANKINFO.BankName");
                lstFields.Add("AG_BANKINFO.Address1");
                lstFields.Add("AG_BANKINFO.Address2");
                lstFields.Add("AG_BANKINFO.Address3");
                lstFields.Add("AG_BANKINFO.Country");
                lstFields.Add("AG_BANKINFO.City");
                lstFields.Add("AG_BANKINFO.State");
                lstFields.Add("AG_BANKINFO.Postcode");
                lstFields.Add("AG_BANKINFO.AccountName");
                lstFields.Add("AG_BANKINFO.AccountNo");
                lstFields.Add("AG_BANKINFO.rowguid");
                lstFields.Add("AG_BANKINFO.SyncCreate");
                lstFields.Add("AG_BANKINFO.SyncLastUpd");
                lstFields.Add("AG_BANKINFO.LastSyncBy");

                strFields = GetSqlFields(lstFields);
                strFilter = " WHERE AG_BANKINFO.AgentID='" + pAgentID + "'";
                strSQL = "SELECT " + strFields + " FROM AG_BANKINFO " + strFilter;
                dt = objDCom.Execute(strSQL, CommandType.Text, true);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow drRow = dt.Rows[0];

                    objAG_BANKINFO_Info = new AgentBankInfo();
                    objAG_BANKINFO_Info.AgentID = (string)drRow["AgentID"];
                    objAG_BANKINFO_Info.BankName = (string)drRow["BankName"];
                    objAG_BANKINFO_Info.Address1 = (string)drRow["Address1"];
                    objAG_BANKINFO_Info.Address2 = (string)drRow["Address2"];
                    objAG_BANKINFO_Info.Address3 = (string)drRow["Address3"];
                    objAG_BANKINFO_Info.Country = (string)drRow["Country"];
                    objAG_BANKINFO_Info.City = (string)drRow["City"];
                    objAG_BANKINFO_Info.State = (string)drRow["State"];
                    objAG_BANKINFO_Info.Postcode = (string)drRow["Postcode"];
                    objAG_BANKINFO_Info.AccountName = (string)drRow["AccountName"];
                    objAG_BANKINFO_Info.AccountNo = (string)drRow["AccountNo"];
                    objAG_BANKINFO_Info.rowguid = (Guid)drRow["rowguid"];
                    if (DateTime.TryParse(drRow["SyncCreate"].ToString(), out dateValue)) objAG_BANKINFO_Info.SyncCreate = (DateTime)drRow["SyncCreate"];
                    if (DateTime.TryParse(drRow["SyncLastUpd"].ToString(), out dateValue)) objAG_BANKINFO_Info.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
                    objAG_BANKINFO_Info.LastSyncBy = (string)drRow["LastSyncBy"];
                    return objAG_BANKINFO_Info;
                }
                else
                {
                    return null;
                    throw new ApplicationException("AG_BANKINFO does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }

        public AgentBankInfo SaveAG_BANKINFO(AgentBankInfo pAG_BANKINFO_Info, SaveType pType)
        {
            bool rValue = false;
            ArrayList lstSQL = new ArrayList();
            try
            {
                objSQL.AddField("AgentID", pAG_BANKINFO_Info.AgentID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("BankName", pAG_BANKINFO_Info.BankName, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Address1", pAG_BANKINFO_Info.Address1, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Address2", pAG_BANKINFO_Info.Address2, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Address3", pAG_BANKINFO_Info.Address3, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Country", pAG_BANKINFO_Info.Country, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("City", pAG_BANKINFO_Info.City, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty);
                objSQL.AddField("State", pAG_BANKINFO_Info.State, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Postcode", pAG_BANKINFO_Info.Postcode, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("AccountName", pAG_BANKINFO_Info.AccountName, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("AccountNo", pAG_BANKINFO_Info.AccountNo, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncCreate", pAG_BANKINFO_Info.SyncCreate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncLastUpd", pAG_BANKINFO_Info.SyncLastUpd, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastSyncBy", pAG_BANKINFO_Info.LastSyncBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                switch (pType)
                {
                    case SaveType.Insert:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert, "AG_BANKINFO", string.Empty);
                        break;
                    case SaveType.Update:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "AG_BANKINFO", "AG_BANKINFO.AgentID='" + pAG_BANKINFO_Info.AgentID + "'");
                        break;
                }
                lstSQL.Add(strSQL);
                rValue = objDCom.BatchExecute(lstSQL, CommandType.Text, true, false);
                if (rValue == false)
                {
                    return null;
                }
                return GetAgentBankInfo(pAG_BANKINFO_Info.AgentID);
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex); return null;
            }
        }

        private static String GetSqlFields(List<string> Fields)
        {
            String strFields = string.Empty;
            if (Fields != null)
            {
                foreach (string sField in Fields)
                {
                    if (strFields == string.Empty)
                    {
                        strFields = sField;
                    }
                    else
                    {
                        strFields += ", " + sField;
                    }
                }
            }
            return strFields;
        }
    }

}
