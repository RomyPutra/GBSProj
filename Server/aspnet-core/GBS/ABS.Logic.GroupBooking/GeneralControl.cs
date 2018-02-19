using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SEAL.Data;
using System.Web;
//using log4net;
using System.Security.Cryptography;
using System.Data.SqlClient;
//using System.ServiceModel;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;
using ABS.GBS.Log;
using StackExchange.Profiling;

namespace ABS.Logic.GroupBooking
{
    public partial class Country_Info
    {
        private int _id;
        private string _countrycode = String.Empty;
        private string _countryName = String.Empty;
        private string _provincestatecode = String.Empty;
        private string _provinceStateName = String.Empty;
        private string _customState = string.Empty;
        private string _cityCode = string.Empty;
        private string _currencyCode = string.Empty;

        #region Public Properties
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string countrycode
        {
            get { return _countrycode; }
            set { _countrycode = value; }
        }

        public string countryName
        {
            get { return _countryName; }
            set { _countryName = value; }
        }

        public string provincestatecode
        {
            get { return _provincestatecode; }
            set { _provincestatecode = value; }
        }

        public string provinceStateName
        {
            get { return _provinceStateName; }
            set { _provinceStateName = value; }
        }

        public string CustomState
        {
            get { return _customState; }
            set { _customState = value; }
        }

        public string CityCode
        {
            get { return _cityCode; }
            set { _cityCode = value; }
        }
        #endregion

        public string CurrencyCode
        {
            get { return _currencyCode; }
            set { _currencyCode = value; }
        }

    }

    public class LocationContainer
    {
        private string _locationCode = String.Empty;
        private string _name = String.Empty;
        private string _locationType = String.Empty;
        private long _paymentGroupID;
        private string _cultureCode = String.Empty;
        private string _currencyCode = String.Empty;
        private bool _inActive;
        private long _createdAgentID;
        private DateTime _createdDate;
        private long _modifiedAgentID;
        private DateTime _modifiedDate;

        #region Public Properties
        public string LocationCode
        {
            get { return _locationCode; }
            set { _locationCode = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string LocationType
        {
            get { return _locationType; }
            set { _locationType = value; }
        }

        public long PaymentGroupID
        {
            get { return _paymentGroupID; }
            set { _paymentGroupID = value; }
        }

        public string CultureCode
        {
            get { return _cultureCode; }
            set { _cultureCode = value; }
        }

        public string CurrencyCode
        {
            get { return _currencyCode; }
            set { _currencyCode = value; }
        }

        public bool InActive
        {
            get { return _inActive; }
            set { _inActive = value; }
        }

        public long CreatedAgentID
        {
            get { return _createdAgentID; }
            set { _createdAgentID = value; }
        }

        public DateTime CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }

        public long ModifiedAgentID
        {
            get { return _modifiedAgentID; }
            set { _modifiedAgentID = value; }
        }

        public DateTime ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; }
        }
        #endregion
    }

    public class CODEMASTER
    {
        private string _codeType = String.Empty;
        private string _code = String.Empty;

        private string _codeDesc = String.Empty;
        private int _codeSeq;
        private byte _sysCode;
        private Guid _rowguid = Guid.Empty;
        private DateTime _syncCreate;
        private DateTime _syncLastUpd;
        private byte _isHost;

        #region Public Properties
        public string CodeType
        {
            get { return _codeType; }
            set { _codeType = value; }
        }
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }
        public string CodeDesc
        {
            get { return _codeDesc; }
            set { _codeDesc = value; }
        }

        public int CodeSeq
        {
            get { return _codeSeq; }
            set { _codeSeq = value; }
        }

        public byte SysCode
        {
            get { return _sysCode; }
            set { _sysCode = value; }
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
        #endregion

    }

    public class GB4SETTING
    {
        private int _AppID;
        private string _CountryCode = String.Empty;
        private string _CountryName = String.Empty;
        private string _Origin = String.Empty;
        private string _OrgID = String.Empty;
        private string _AgentID = String.Empty;
        private int _NoofPax;
        private byte _status;
        private Guid _rowguid = Guid.Empty;
        private DateTime _syncCreate;
        private DateTime _syncLastUpd;
        private string _LastSyncBy = String.Empty;
        private DateTime _EffectiveDate;
        private DateTime _ExpiryDate;

        #region Public Properties
        public int AppID
        {
            get { return _AppID; }
            set { _AppID = value; }
        }
        public string CountryCode
        {
            get { return _CountryCode; }
            set { _CountryCode = value; }
        }
        public string CountryName
        {
            get { return _CountryName; }
            set { _CountryName = value; }
        }
        public string Origin
        {
            get { return _Origin; }
            set { _Origin = value; }
        }
        public string OrgID
        {
            get { return _OrgID; }
            set { _OrgID = value; }
        }
        public string AgentID
        {
            get { return _AgentID; }
            set { _AgentID = value; }
        }
        public int NoofPax
        {
            get { return _NoofPax; }
            set { _NoofPax = value; }
        }
        public byte status
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
            get { return _LastSyncBy; }
            set { _LastSyncBy = value; }
        }
        public DateTime EffectiveDate
        {
            get { return _EffectiveDate; }
            set { _EffectiveDate = value; }
        }
        public DateTime ExpiryDate
        {
            get { return _ExpiryDate; }
            set { _ExpiryDate = value; }
        }
        #endregion

    }

    #region SYS_AUDITLOG
    /// <summary>
    /// This object represents the properties and methods of a SYS_AUDITLOG.
    /// </summary>
    public class AUDITLOG
    {
        private Guid _rowguid = Guid.Empty;

        private string _transID = String.Empty;
        private int _seqNo;
        private byte _action;
        private string _refCode = String.Empty;
        private string _table_Name = String.Empty;
        private string _sQL = String.Empty;
        private DateTime _createdDate;
        private string _createdBy = String.Empty;
        private byte _priority;
        private byte _flag;

        #region Public Properties
        public Guid rowguid
        {
            get { return _rowguid; }
            set { _rowguid = value; }
        }
        public string TransID
        {
            get { return _transID; }
            set { _transID = value; }
        }
        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }
        public int SeqNo
        {
            get { return _seqNo; }
            set { _seqNo = value; }
        }

        public byte Action
        {
            get { return _action; }
            set { _action = value; }
        }

        public string RefCode
        {
            get { return _refCode; }
            set { _refCode = value; }
        }

        public string Table_Name
        {
            get { return _table_Name; }
            set { _table_Name = value; }
        }

        public string SQL
        {
            get { return _sQL; }
            set { _sQL = value; }
        }

        public DateTime CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }

        public byte Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        public byte Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }
        #endregion

    }




    #endregion

    #region Settings
    public class Settings
    {
        private short _appID;
        private string _GRPID = String.Empty;
        private string _sYSKey = String.Empty;
        private string _sYSDesc = String.Empty;

        private string _sYSValue = String.Empty;
        private string _sYSValueEx = String.Empty;
        private byte _sYSSet;
        private Guid _rowguid = Guid.Empty;
        private DateTime _syncCreate;
        private DateTime _syncLastUpd;
        private byte _isHost;
        private string _lastSyncBy = String.Empty;

        #region Public Properties
        public short AppID
        {
            get { return _appID; }
            set { _appID = value; }
        }
        public string GRPID
        {
            get { return _GRPID; }
            set { _GRPID = value; }
        }
        public string SYSKey
        {
            get { return _sYSKey; }
            set { _sYSKey = value; }
        }
        public string SYSDesc
        {
            get { return _sYSDesc; }
            set { _sYSDesc = value; }
        }
        public string SYSValue
        {
            get { return _sYSValue; }
            set { _sYSValue = value; }
        }

        public string SYSValueEx
        {
            get { return _sYSValueEx; }
            set { _sYSValueEx = value; }
        }

        public byte SYSSet
        {
            get { return _sYSSet; }
            set { _sYSSet = value; }
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
    #endregion



    public partial class GeneralControl : Shared.CoreBase
    {
        LogControl log = new LogControl();
        SystemLog SystemLog = new SystemLog();
		//ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		//AALookUpWS.LookupWSSoapClient aws = new AALookUpWS.LookupWSSoapClient();
		//Added by ketee, 20160104, ACE web services replace ARMS
		//ACE.SessionManager.SessionServiceClient aceSession = new ACE.SessionManager.SessionServiceClient();
		//ACE.LookUpManager.LookupServiceClient aceLookup = new ACE.LookUpManager.LookupServiceClient();

		//string ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ToString();
		string ConnStr = "Data Source=172.20.145.11;Initial Catalog = GBSPILOT; Persist Security Info=True;User ID = gbs; Password=p@ssw0rd; connection timeout = 60; Application Name = GBS";

		public enum SaveType
        {
            Insert = 0,
            Update = 1
        }

        public List<Country_Info> GetAllCountryCode()
        {
            Country_Info objCountry_Info;
            List<Country_Info> objListCountry_Info = new List<Country_Info>();
            DataTable dt = new DataTable();
            String strSQL = string.Empty;

            try
            {
                using (var connection = new SqlConnection(ConnStr))
                {
                    connection.Open();
                    strSQL = "SELECT * FROM CountryCode ";
                    SqlCommand cmd = new SqlCommand(strSQL, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    connection.Close();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow drRow in dt.Rows)
                        {
                            objCountry_Info = new Country_Info();
                            objCountry_Info.Id = (int)drRow["Id"];
                            objCountry_Info.countrycode = (string)drRow["countrycode"];
                            objCountry_Info.countryName = (string)drRow["countryName"];
                            objCountry_Info.provincestatecode = (string)drRow["provincestatecode"];
                            objCountry_Info.provinceStateName = (string)drRow["provinceStateName"];
                            objListCountry_Info.Add(objCountry_Info);
                        }
                        return objListCountry_Info;
                    }
                    else
                    {
                        log.Info(this, "CountryCode does not exist."); //added, for log purpose
                        return null;
                        //throw new ApplicationException("CountryCode does not exist.");
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex); //added, for log purpose
                return null;
            }
        }

        int cc = 0;
        public Country_Info GetCountryInfoByCode(string CountryCode, string CurrencyCode)
        {
            Country_Info objCountry_Info = new Country_Info();
            List<Country_Info> objListCountry_Info = new List<Country_Info>();
            DataTable dt = new DataTable();
            String strSQL = string.Empty;

            try
            {

                using (var connection = new SqlConnection(ConnStr))
                {
                    connection.Open();
                    if (CountryCode.Length > 2)
                    {
                        strSQL = "SELECT * FROM COUNTRY WHERE CountryCode='" + CountryCode.Substring(0, 2).ToString() + "' ";
                    }
                    else
                    {
                        strSQL = "SELECT * FROM COUNTRY WHERE CountryCode='" + CountryCode.ToString() + "' ";
                    }
                    SqlCommand cmd = new SqlCommand(strSQL, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    connection.Close();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow drRow in dt.Rows)
                        {
                            objCountry_Info = new Country_Info();
                            objCountry_Info.countrycode = (string)drRow["CountryCode"];
                            objCountry_Info.countryName = (string)drRow["Name"];
                            objCountry_Info.CurrencyCode = (string)drRow["DefaultCurrencyCode"];
                        }
                        return objCountry_Info;
                    }
                    else
                    {
                        connection.Open();
                        strSQL = "SELECT * FROM COUNTRY WHERE DefaultCurrencyCode='" + CurrencyCode + "' ";
                        cmd = new SqlCommand(strSQL, connection);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        connection.Close();

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow drRow in dt.Rows)
                            {
                                objCountry_Info = new Country_Info();
                                objCountry_Info.countrycode = (string)drRow["CountryCode"];
                                objCountry_Info.countryName = (string)drRow["Name"];
                                objCountry_Info.CurrencyCode = (string)drRow["DefaultCurrencyCode"];
                            }
                            return objCountry_Info;
                        }
                        else
                        {
                            log.Info(this, "CountryCode does not exist."); //added, for log purpose
                            return null;
                            //throw new ApplicationException("CountryCode does not exist.");
                        }
                    }
                }
                return null;

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex, strSQL); //added, for log purpose
                return null;
            }
            finally
            {
                dt = null;
            }

        }

        //public Country_Info GetCountryInfoByCode(string CountryCode, string CurrencyCode)
        //{
        //    Country_Info objCountry_Info = new Country_Info();
        //    List<Country_Info> objListCountry_Info = new List<Country_Info>();
        //    DataTable dt = new DataTable();
        //    String strSQL = string.Empty;

        //    //log.Info(this, CountryCode + "a" + CurrencyCode);

        //    //using(objDCom = new DataAccess())
        //    {

        //        try
        //        {
        //            //objDCom.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ToString();
        //            StartConnection(EnumIsoState.StateUpdatetable, true);
        //            //if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
        //            {
        //                StartSQLControl();

        //                strSQL = "SELECT * FROM COUNTRY WHERE CountryCode='" + CountryCode.Substring(0, 2).ToString() + "' ";

        //                //log.Info(this, String.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + " | " + cc + " | " + strSQL);
        //                //cc++;
        //                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
        //                if (dt != null && dt.Rows.Count > 0)
        //                {
        //                    foreach (DataRow drRow in dt.Rows)
        //                    {
        //                        objCountry_Info = new Country_Info();
        //                        objCountry_Info.countrycode = (string)drRow["CountryCode"];
        //                        objCountry_Info.countryName = (string)drRow["Name"];
        //                    }
        //                    return objCountry_Info;
        //                }
        //                else
        //                {
        //                    strSQL = "SELECT * FROM COUNTRY WHERE DefaultCurrencyCode='" + CurrencyCode + "' ";
        //                    dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
        //                    if (dt != null && dt.Rows.Count > 0)
        //                    {
        //                        foreach (DataRow drRow in dt.Rows)
        //                        {
        //                            objCountry_Info = new Country_Info();
        //                            objCountry_Info.countrycode = (string)drRow["CountryCode"];
        //                            objCountry_Info.countryName = (string)drRow["Name"];
        //                        }
        //                        return objCountry_Info;
        //                    }
        //                    else
        //                    {
        //                        log.Info(this, "CountryCode does not exist."); //added, for log purpose
        //                        return null;
        //                        //throw new ApplicationException("CountryCode does not exist.");
        //                    }
        //                }
        //            }
        //            return null;

        //        }
        //        catch (Exception ex)
        //        {
        //            log.Error(this, ex, strSQL); //added, for log purpose
        //            return null;
        //        }
        //        finally
        //        {
        //            dt = null;
        //            //objDCom.CloseConnection();
        //            EndSQLControl();
        //            EndConnection();
        //        }
        //    }

        //}

        //GetCountryInfoByCode Backup
        public Country_Info GetCountryInfoByCodeOld(string CountryCode, string CurrencyCode)
        {
            Country_Info objCountry_Info = new Country_Info();
            List<Country_Info> objListCountry_Info = new List<Country_Info>();
            DataTable dt = new DataTable();
            String strSQL = string.Empty;

            try
            {
                strSQL = "SELECT * FROM COUNTRY WHERE CountryCode='" + CountryCode.Substring(0, 2).ToString() + "' ";

                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow drRow in dt.Rows)
                    {
                        objCountry_Info = new Country_Info();
                        objCountry_Info.countrycode = (string)drRow["CountryCode"];
                        objCountry_Info.countryName = (string)drRow["Name"];
                    }
                    return objCountry_Info;
                }
                else
                {
                    strSQL = "SELECT * FROM COUNTRY WHERE DefaultCurrencyCode='" + CurrencyCode + "' ";
                    dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow drRow in dt.Rows)
                        {
                            objCountry_Info = new Country_Info();
                            objCountry_Info.countrycode = (string)drRow["CountryCode"];
                            objCountry_Info.countryName = (string)drRow["Name"];
                        }
                        return objCountry_Info;
                    }
                    else
                    {
                        log.Info(this, "CountryCode does not exist."); //added, for log purpose
                        return null;
                        //throw new ApplicationException("CountryCode does not exist.");
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex, strSQL); //added, for log purpose
                return null;
            }
            finally
            {
                dt = null;
                //objDCom.CloseConnection();
            }
        }

        public DataTable GetPaxSetting(string OrgID)
        {

            DataTable dt = new DataTable();
            String strSQL = string.Empty;

            try
            {
                using (var connection = new SqlConnection(ConnStr))
                {
                    connection.Open();
                    strSQL = "SELECT CountryCode, Origin, OrgID, AgentID, NoofPax, Status, CONVERT(date, EffectiveDate) EffectiveDate, CONVERT(date, ExpiryDate) ExpiryDate FROM GB4SETTING WHERE OrgID='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, OrgID) + "'";
                    SqlCommand cmd = new SqlCommand(strSQL, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    connection.Close();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                    else
                    {
                        log.Info(this, "GB4SETTING does not exist."); //added, for log purpose
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex); //added, for log purpose
                return null;
            }
        }

        public DataTable GetPaxSettingAll()
        {

            DataTable dt = new DataTable();
            String strSQL = string.Empty;

            try
            {
                using (var connection = new SqlConnection(ConnStr))
                {
                    connection.Open();
                    strSQL = "SELECT DISTINCT CountryCode, EffectiveDate, ExpiryDate, CountryName, Origin, AG.OrgID, AG.OrgName, GB.AgentID, CASE WHEN GB.AgentID = '' THEN '' WHEN GB.AgentID <> '' THEN (SELECT Username FROM AG_PROFILE WHERE AgentID = GB.AgentID) END Username, NoofPax, GB.Status FROM GB4SETTING GB LEFT JOIN AG_PROFILE AG WITH (NOLOCK) ON GB.OrgID = AG.OrgID";
                    SqlCommand cmd = new SqlCommand(strSQL, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    connection.Close();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                    else
                    {
                        log.Info(this, "GB4SETTING does not exist."); //added, for log purpose
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex); //added, for log purpose
                return null;
            }
        }

        public DataTable GetSinglePaxSetting(string CountryCode, string Origin, string OrgID, string AgentID)
        {

            DataTable dt = new DataTable();
            String strSQL = string.Empty;

            try
            {
                using (var connection = new SqlConnection(ConnStr))
                {
                    connection.Open();
                    strSQL = "SELECT * FROM GB4SETTING WHERE CountryCode = '" + CountryCode + "' AND Origin = '" + Origin + "' AND OrgID = '" + OrgID + "' AND AgentID  = '" + AgentID + "'";
                    SqlCommand cmd = new SqlCommand(strSQL, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    connection.Close();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                    else
                    {
                        log.Info(this, "GB4SETTING does not exist."); //added, for log purpose
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex); //added, for log purpose
                return null;
            }
        }

        public DataTable GetCountryNameByCode(string CountryCode)
        {

            DataTable dt = new DataTable();
            String strSQL = string.Empty;

            try
            {
                using (var connection = new SqlConnection(ConnStr))
                {
                    connection.Open();
                    strSQL = "SELECT DISTINCT Name FROM COUNTRY WHERE CountryCode='" + CountryCode + "'";
                    SqlCommand cmd = new SqlCommand(strSQL, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    connection.Close();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                    else
                    {
                        log.Info(this, "Country does not exist."); //added, for log purpose
                        return null;
                    }
                }
                return null;

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex); //added, for log purpose
                return null;
            }
        }



        //20170510 - Sienny
        public DataTable GetCountryCodeByCurrency(string CurrencyCode)
        {

            DataTable dt = new DataTable();
            String strSQL = string.Empty;

            try
            {
                using (var connection = new SqlConnection(ConnStr))
                {
                    connection.Open();
                    strSQL = "SELECT CountryCode, Name, CountryCode3C, DefaultCurrencyCode FROM COUNTRY WHERE DefaultCurrencyCode='" + CurrencyCode + "'";
                    SqlCommand cmd = new SqlCommand(strSQL, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    connection.Close();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                    else
                    {
                        log.Info(this, "Country does not exist."); //added, for log purpose
                        return null;
                    }
                }
                return null;

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex); //added, for log purpose
                return null;
            }
        }

        public List<Country_Info> GetAllCountry()
        {
            Country_Info objCountry_Info;
            List<Country_Info> objListCountry_Info = new List<Country_Info>();
            DataTable dt = new DataTable();
            String strSQL = string.Empty;

            try
            {
                using (var connection = new SqlConnection(ConnStr))
                {
                    connection.Open();
                    strSQL = "SELECT distinct  countrycode, countryName FROM CountryCode order by countryName";
                    SqlCommand cmd = new SqlCommand(strSQL, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    connection.Close();

                    log.Info(this, strSQL);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow drRow in dt.Rows)
                        {
                            objCountry_Info = new Country_Info();
                            objCountry_Info.countrycode = (string)drRow["countrycode"];
                            objCountry_Info.countryName = (string)drRow["countryName"];
                            objListCountry_Info.Add(objCountry_Info);
                        }
                        return objListCountry_Info;
                    }
                    else
                    {
                        log.Info(this, "GetAllCountry() - CountryCode does not exist."); //added, for log purpose
                        return null;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex); //added, for log purpose
                return null;
            }
        }

        public DataTable GetAllCountryCard()
        {

            DataTable dt = new DataTable();
            String strSQL = string.Empty;

            try
            {
                using (var connection = new SqlConnection(ConnStr))
                {
                    connection.Open();
                    strSQL = "SELECT DISTINCT CountryCode, Name, CountryCode3C FROM COUNTRY order by Name";
                    SqlCommand cmd = new SqlCommand(strSQL, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    connection.Close();
                    
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                    else
                    {
                        log.Info(this, "Country does not exist."); //added, for log purpose
                        return null;
                        throw new ApplicationException("Country does not exist.");
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                return null;
            }
        }


        public List<Country_Info> GetAllState(string code)
        {
            Country_Info objCountry_Info;
            List<Country_Info> objListCountry_Info = new List<Country_Info>();
            DataTable dt = new DataTable();
            String strSQL = string.Empty;

            try
            {
                using (var connection = new SqlConnection(ConnStr))
                {
                    connection.Open();
                    strSQL = "SELECT distinct provincestatecode, provinceStateName FROM CountryCode where countrycode='" + code + "' ORDER BY provinceStateName";
                    SqlCommand cmd = new SqlCommand(strSQL, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    connection.Close();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow drRow in dt.Rows)
                        {
                            objCountry_Info = new Country_Info();
                            objCountry_Info.provincestatecode = (string)drRow["provincestatecode"];
                            objCountry_Info.provinceStateName = (string)drRow["provinceStateName"];
                            objListCountry_Info.Add(objCountry_Info);
                        }
                        return objListCountry_Info;
                    }
                    else
                    {
                        log.Info(this, "State does not exist."); //added, for log purpose
                        return null;
                        //throw new ApplicationException("State does not exist.");
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex);
                return null;
            }
        }

        public string GetCurrencyByDeparture(string departure)
        {
            try
            {
                //if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
                //{
                DataTable dt;
                strSQL = "SELECT Country.DefaultCurrencyCode FROM City INNER JOIN";
                strSQL += " COUNTRY ON City.CountryCode = COUNTRY.CountryCode";
                strSQL += " WHERE City.CityCode = '" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, departure) + "'";
                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["DefaultCurrencyCode"].ToString();
                }
                else
                {
                    return "";
                }
                //}
                //return "";
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex); //added, for log purpose
                return "";
            }
            //finally
            //{
            //    EndSQLControl();
            //    EndConnection();
            //}
        }

        //added by ketee
        public DataTable ReturnAllCityCustom(string DepartCity)
        {
            //Country_Info objCountry_Info;
            List<Country_Info> objListCountry_Info = new List<Country_Info>();
            DataTable dt;
            string strCond;
            string strStation;
            try
            {
                if (DepartCity != string.Empty)
                {
                    strStation = "CityPair.ArrivalStation";
                }
                else
                {
                    strStation = "CityPair.DepartureStation";
                }

                strSQL = "SELECT DISTINCT CI.CityCode, CI.Name + ' (' + CI.CityCode + ')' + '|' + CT.Name AS CustomState, CT.Name, CI.Name AS CityName FROM CityPair INNER JOIN " +
                         "City AS CI WITH (nolock) ON " + strStation + " = CI.CityCode INNER JOIN Country AS CT WITH (nolock) ON " +
                         "CT.CountryCode = CI.CountryCode ";
                strCond = " WHERE  (CI.InActive = 0) AND (CT.InActive = 0) ";
                if (DepartCity != string.Empty)
                {
                    strCond = string.Concat(strCond, " AND DepartureStation = '", objSQL.ParseValue(SQLControl.EnumDataType.dtString, DepartCity), "'");
                }
                strSQL = string.Concat(strSQL, strCond, " ORDER BY CT.Name, CityName");
                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt;
                }
                else
                {
                    log.Info(this, "State does not exist."); //added, for log purpose
                    return null;
                    //throw new ApplicationException("State does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex); //added, for log purpose
                return null;
            }
        }

        public DataTable GetLookUpCity(string CityDepart, string PhysicalApplicationPath)
        {
            var profiler = MiniProfiler.Current;
            GetRouteListResponse AllCityPairResponse = new GetRouteListResponse();
            GetRouteListResponse DepartCityPairResponse = new GetRouteListResponse();

            try
            {
                DataTable dt = new DataTable();
                DataTable dtDepart = new DataTable();
                DataTable dtresp = new DataTable();
                dt = CountryInfoStructure();
                dtDepart = CountryInfoStructure();
                dtresp = CountryInfoStructure();

                if (HttpContext.Current.Session["CityPairAll"] != null)
                    dt = (DataTable)HttpContext.Current.Session["CityPairAll"];
                else
                {
                    using (profiler.Step("ACE:LookUpAllCity"))
                    {
                        AllCityPairResponse = LookUpAllCity(PhysicalApplicationPath);
                    }
                    if (AllCityPairResponse != null)
                    {
                        dt = CreateDataTable(AllCityPairResponse.RouteListExtend.OrderBy(a => a.DepartureCountry).ToList());
                        HttpContext.Current.Session.Add("CityPairAll", dt);
                    }
                }

                if (HttpContext.Current.Session["CityPairDepart"] != null)
                    dtDepart = (DataTable)HttpContext.Current.Session["CityPairDepart"];
                else
                {
                    using (profiler.Step("ACE:LookUpCity"))
                    {
                        DepartCityPairResponse = LookUpCity("", PhysicalApplicationPath);
                    }
                    if (DepartCityPairResponse != null)
                    {
                        dtDepart = CreateDataTable(DepartCityPairResponse.RouteListExtend.OrderBy(a => a.DepartureCountry).ToList());
                        HttpContext.Current.Session.Add("CityPairDepart", dtDepart);
                    }

                }

                if (dt == null || dt.Rows.Count == 0 || dtDepart == null || dtDepart.Rows.Count == 0)
                {
                    using (profiler.Step("ACE:SetCityPair"))
                    {
                        SetCityPair(PhysicalApplicationPath);
                    }
                    AllCityPairResponse = LookUpAllCity(PhysicalApplicationPath);
                    if (AllCityPairResponse != null)
                    {
                        dt = CreateDataTable(AllCityPairResponse.RouteListExtend.OrderBy(a => a.DepartureCountry).ToList());
                        HttpContext.Current.Session.Add("CityPairAll", dt);
                    }

                    DepartCityPairResponse = LookUpCity("", PhysicalApplicationPath);
                    if (DepartCityPairResponse != null)
                    {
                        dtDepart = CreateDataTable(DepartCityPairResponse.RouteListExtend.OrderBy(a => a.DepartureCountry).ToList());
                        HttpContext.Current.Session.Add("CityPairDepart", dtDepart);
                    }

                }

                if (HttpContext.Current.Session["CityPairAll"] != null)
                    dt = (DataTable)HttpContext.Current.Session["CityPairAll"];

                if (HttpContext.Current.Session["CityPairDepart"] != null)
                    dtDepart = (DataTable)HttpContext.Current.Session["CityPairDepart"];


                if (dt != null || dt.Rows.Count != 0)
                {
                    if (CityDepart != "")
                    {
                        dtresp = dt.Clone();
                        //DataRow[] drs = dt.Select("CityCode = '" + CityDepart + "'", "RName");
                        DataRow[] drs = dt.Select("DepartureStation = '" + CityDepart + "'", "ArrivalCountry");
                        if (drs != null)
                        {
                            foreach (DataRow dr in drs)
                            {
                                dtresp.ImportRow(dr);
                            }
                        }
                    }
                    else
                    {
                        DataView dv = dtDepart.DefaultView;
                        //dv.Sort = "Name";
                        dv.Sort = "DepartureCountry";
                        return dv.ToTable();
                    }

                    if (dtresp != null)
                    {
                        return dtresp;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex);
                return null;
            }
        }

        public DataTable GetLookUpCitybyCountry(string Country, string PhysicalApplicationPath)
        {
            var profiler = MiniProfiler.Current;
            GetRouteListResponse AllCityPairResponse = new GetRouteListResponse();
            GetRouteListResponse DepartCityPairResponse = new GetRouteListResponse();

            try
            {
                DataTable dt = new DataTable();
                DataTable dtDepart = new DataTable();
                DataTable dtresp = new DataTable();
                dt = CountryInfoStructure();
                dtDepart = CountryInfoStructure();
                dtresp = CountryInfoStructure();

                if (HttpContext.Current.Session["CityPairAll"] != null)
                    dt = (DataTable)HttpContext.Current.Session["CityPairAll"];
                else
                {
                    using (profiler.Step("ACE:LookUpAllCity"))
                    {
                        AllCityPairResponse = LookUpAllCity(PhysicalApplicationPath);
                    }
                    if (AllCityPairResponse != null)
                    {
                        dt = CreateDataTable(AllCityPairResponse.RouteListExtend.OrderBy(a => a.DepartureCountry).ToList());
                        HttpContext.Current.Session.Add("CityPairAll", dt);
                    }
                }

                if (HttpContext.Current.Session["CityPairDepart"] != null)
                    dtDepart = (DataTable)HttpContext.Current.Session["CityPairDepart"];
                else
                {
                    using (profiler.Step("ACE:LookUpCity"))
                    {
                        DepartCityPairResponse = LookUpCity("", PhysicalApplicationPath);
                    }
                    if (DepartCityPairResponse != null)
                    {
                        dtDepart = CreateDataTable(DepartCityPairResponse.RouteListExtend.OrderBy(a => a.DepartureCountry).ToList());
                        HttpContext.Current.Session.Add("CityPairDepart", dtDepart);
                    }

                }

                if (dt == null || dt.Rows.Count == 0 || dtDepart == null || dtDepart.Rows.Count == 0)
                {
                    using (profiler.Step("ACE:SetCityPair"))
                    {
                        SetCityPair(PhysicalApplicationPath);
                    }
                    AllCityPairResponse = LookUpAllCity(PhysicalApplicationPath);
                    if (AllCityPairResponse != null)
                    {
                        dt = CreateDataTable(AllCityPairResponse.RouteListExtend.OrderBy(a => a.DepartureCountry).ToList());
                        HttpContext.Current.Session.Add("CityPairAll", dt);
                    }

                    DepartCityPairResponse = LookUpCity("", PhysicalApplicationPath);
                    if (DepartCityPairResponse != null)
                    {
                        dtDepart = CreateDataTable(DepartCityPairResponse.RouteListExtend.OrderBy(a => a.DepartureCountry).ToList());
                        HttpContext.Current.Session.Add("CityPairDepart", dtDepart);
                    }

                }

                if (HttpContext.Current.Session["CityPairAll"] != null)
                    dt = (DataTable)HttpContext.Current.Session["CityPairAll"];

                if (HttpContext.Current.Session["CityPairDepart"] != null)
                    dtDepart = (DataTable)HttpContext.Current.Session["CityPairDepart"];


                if (dt != null || dt.Rows.Count != 0)
                {
                    if (Country != "")
                    {
                        dtresp = dt.Clone();
                        //DataRow[] drs = dt.Select("CityCode = '" + CityDepart + "'", "RName");
                        DataRow[] drs = dt.Select("DepartureCountry = '" + Country + "'", "DepartureCountry");
                        if (drs != null)
                        {
                            foreach (DataRow dr in drs)
                            {
                                dtresp.ImportRow(dr);
                            }
                        }
                    }
                    else
                    {
                        DataView dv = dtDepart.DefaultView;
                        //dv.Sort = "Name";
                        dv.Sort = "DepartureCountry";
                        return dv.ToTable();
                    }

                    if (dtresp != null)
                    {
                        return dtresp;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex);
                return null;
            }
        }

        //added by ketee, compare depart return station country
        public Boolean IsLongHaulFlightbySellKey(string FareSellKey, string PhysicalApplicationPath, string OverridedFareSellKey)
        {
            //GetRouteListResponse AllCityPairResponse = new GetRouteListResponse();

            try
            {
                //validate is domestic/International flight by origin and destination
                //AllCityPairResponse = LookUpAllCity(PhysicalApplicationPath);
                //if (AllCityPairResponse != null)
                //{
                //    string departCountry = AllCityPairResponse.RouteListExtend.First(a => a.DepartureStation == departureStation).DepartureCountry;
                //    string arriveCountry = AllCityPairResponse.RouteListExtend.First(a => a.ArrivalStation == ArrivalStation).ArrivalCountry;
                //    if (departCountry.Trim().ToLower().Equals(arriveCountry.Trim().ToLower()))
                //    {
                //        return false;
                //    }
                //    else
                //    {
                //        return true;
                //    }
                //}
                //else
                //{
                //    return true;
                //}
                //validate flight by long short haul
                if (FareSellKey.Split('~')[2].ToString().EndsWith("H"))
                {
                    return true;
                }
                else
                {
                    if (OverridedFareSellKey != null)
                    {
                        if (OverridedFareSellKey.Split('~')[2].ToString().EndsWith("H"))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                return true;
            }
            finally
            {
                //AllCityPairResponse = null;
            }
        }

        public Boolean IsInternationalFlight(string departureStation, string ArrivalStation, string PhysicalApplicationPath)
        {
            GetRouteListResponse AllCityPairResponse = new GetRouteListResponse();

            try
            {
                //validate is domestic/International flight by origin and destination
                AllCityPairResponse = LookUpAllCity(PhysicalApplicationPath);
                if (AllCityPairResponse != null)
                {
                    string departCountry = AllCityPairResponse.RouteListExtend.First(a => a.DepartureStation == departureStation).DepartureCountry;
                    string arriveCountry = AllCityPairResponse.RouteListExtend.First(a => a.ArrivalStation == ArrivalStation).ArrivalCountry;
                    if (departCountry.Trim().ToLower().Equals(arriveCountry.Trim().ToLower()))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
                //validate flight by long short haul
                //if (departureStation.Split('~')[2].ToString().EndsWith("H")) return true;
                //else return false;

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                return true;
            }
            finally
            {
                AllCityPairResponse = null;
            }
        }

        public BaseResource LoadSAOXML(string XmlFilePath, string fileName)
        {
            //ACEGeneralManager generalManager = new ACEGeneralManager();
            //SHARED.ACELookupService.GetRouteListResponse cityPair;
            BaseResource result = null;
            XmlFilePath = XmlFilePath + "\\TuneSAO\\" + fileName.ToString() + ".xml";

            if (File.Exists(XmlFilePath) == false)
            {
                return null;
            }
            else
            {
                StreamReader xmlStream = new StreamReader(XmlFilePath);
                XmlSerializer serializer = new XmlSerializer(typeof(BaseResource));
                result = (BaseResource)serializer.Deserialize(xmlStream);
            }

            if (result != null && result.Insurances.Length > 0)
                return result;
            else
                return null;
        }
        //public void SetCityPair()
        //{
        //    try
        //    {
        //        ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking("");

        //        AALookUpWS.LogonRequest LogonReq = new AALookUpWS.LogonRequest();
        //        LogonReq.Username = apiBooking.Username;
        //        LogonReq.Password = apiBooking.Password;

        //        DataTable dtAll = new DataTable();
        //        DataTable dtDepart = new DataTable();

        //        dtAll = CountryInfoStructure();
        //        dtDepart = CountryInfoStructure();

        //        if (HttpContext.Current.Session["CityPairAll"] != null)
        //        {
        //            dtAll = (DataTable)HttpContext.Current.Session["CityPairAll"];
        //            //return dtAll;
        //        }

        //        if (HttpContext.Current.Session["CityPairDepart"] != null)
        //        {
        //            dtDepart = (DataTable)HttpContext.Current.Session["CityPairDepart"];
        //            //return dtAll;
        //        }

        //        if (dtAll == null || dtAll.Rows.Count == 0 || dtDepart == null || dtDepart.Rows.Count == 0)
        //        {
        //            AALookUpWS.LogonResponse resp = aws.Login(LogonReq);
        //            AALookUpWS.RouteInfoResponse CityPair = new AALookUpWS.RouteInfoResponse();
        //            if (resp != null)
        //            {
        //                CityPair = aws.GetRouteList("", "", resp.SessionID);
        //                string xml = GetXMLString(CityPair);
        //            }
        //            if (CityPair != null)
        //            {
        //                foreach (AALookUpWS.RouteInfo rInfo in CityPair.RouteInfo)
        //                {
        //                    DataRow dr = null;
        //                    DataRow drDepart = null;
        //                    DataRow[] foundRow = null;
        //                    DataRow[] foundRowDepart = null;
        //                    Country_Info CT = new Country_Info();
        //                    Country_Info CTR = new Country_Info();

        //                    //dtAll.Rows.Find(rInfo.DepartureStation,rInfo.ArrivalStation);
        //                    foundRow = dtAll.Select("CityCode = '" + rInfo.DepartureStation + "' AND RCityCode = '" + rInfo.ArrivalStation + "'");
        //                    foundRowDepart = dtAll.Select("CityCode = '" + rInfo.DepartureStation + "'");
        //                    CTR = GetCountryInfoByCode(rInfo.ArrivalTimeZoneCode, rInfo.ArrivalStationCurrencyCode);
        //                    CT = GetCountryInfoByCode(rInfo.DepartureTimeZoneCode, rInfo.DepartureStationCurrencyCode);

        //                    if (CT != null && CTR != null)
        //                    {
        //                        dr = dtAll.NewRow();
        //                        dr["CityCode"] = rInfo.DepartureStation;
        //                        dr["CustomState"] = rInfo.DepartureStationName + "(" + rInfo.DepartureStation + ")" + '|' + CT.countryName;
        //                        dr["Name"] = CT.countryName;
        //                        dr["CityName"] = rInfo.DepartureStationName;
        //                        dr["RCityCode"] = rInfo.ArrivalStation;
        //                        dr["RCustomState"] = rInfo.ArrivalStationName + "(" + rInfo.ArrivalStation + ")" + '|' + CTR.countryName;
        //                        dr["RName"] = CTR.countryName;
        //                        dr["RCityName"] = rInfo.ArrivalStationName;

        //                        drDepart = dtDepart.NewRow();
        //                        drDepart["CityCode"] = rInfo.DepartureStation;
        //                        drDepart["CustomState"] = rInfo.DepartureStationName + "(" + rInfo.DepartureStation + ")" + '|' + CT.countryName;
        //                        drDepart["Name"] = CT.countryName;
        //                        drDepart["CityName"] = rInfo.DepartureStationName;
        //                        drDepart["RCityCode"] = rInfo.ArrivalStation;
        //                        drDepart["RCustomState"] = rInfo.ArrivalStationName + "(" + rInfo.ArrivalStation + ")" + '|' + CTR.countryName;
        //                        drDepart["RName"] = CTR.countryName;
        //                        drDepart["RCityName"] = rInfo.ArrivalStationName;
        //                        drDepart["Currency"] = rInfo.DepartureStationCurrencyCode;
        //                    }

        //                    if (dr != null && dr["CityCode"].ToString() != "" && foundRow.Length == 0)
        //                    {
        //                        dtAll.Rows.Add(dr);
        //                    }

        //                    if (drDepart != null && drDepart["CityCode"].ToString() != "" && foundRowDepart.Length == 0)
        //                    {
        //                        dtDepart.Rows.Add(drDepart);
        //                    }

        //                }

        //                if (dtAll != null)
        //                {
        //                    HttpContext.Current.Session.Add("CityPairAll", dtAll);
        //                }

        //                if (dtDepart != null)
        //                {
        //                    HttpContext.Current.Session.Add("CityPairDepart", dtDepart);
        //                }
        //            }
        //            //return dtAll;
        //        }
        //        //return null;
        //    }


        //    catch (Exception ex)
        //    {
        //        log.Error(this, ex);
        //        //return null;
        //    }
        //}

        //Edit by ketee, 20160104, replace ARMC lookup services with ACE lookup services

        public void SetCityPair(string PhysicalApplicationPath)
        {
            var profiler = MiniProfiler.Current;
            GetRouteListResponse CityPairResponse = new global::GetRouteListResponse();
            GetRouteListResponse AllCityPairResponse = new GetRouteListResponse();
            GetRouteListResponse DepartCityPairResponse = new GetRouteListResponse();
            List<RouteListExtend> lstAllRouteList = new List<RouteListExtend>();
            List<RouteListExtend> lstDepartRouteList = new List<RouteListExtend>();

            int iAllCityPairResponse = 0;
            int iDepartCityPairResponse = 0;
            Boolean CityPairXmlLoaded = false;

            try
            {
                ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking("");

                ACE.SessionManager.LogonRequest LogonReq = new ACE.SessionManager.LogonRequest();
                LogonReq.Username = apiBooking.Username;
                LogonReq.Password = apiBooking.Password;

                DataTable dtAll = new DataTable();
                DataTable dtDepart = new DataTable();

                dtAll = CountryInfoStructure();
                dtDepart = CountryInfoStructure();

                //aceSession.Endpoint.Address = new System.ServiceModel.EndpointAddress(apiBooking.AceSessionURL);
                //aceLookup.Endpoint.Address = new System.ServiceModel.EndpointAddress(apiBooking.AceLookUpURL);

                //aceLookup.Endpoint.Behaviors.Remove(typeof(CallbackBehaviorAttribute));
                //aceLookup.Endpoint.Behaviors.Add(new CallbackBehaviorAttribute() { MaxItemsInObjectGraph = 2147483647 });

                if (HttpContext.Current.Session["CityPairAll"] != null)
                {
                    dtAll = (DataTable)HttpContext.Current.Session["CityPairAll"];
                    //return dtAll;
                }

                if (HttpContext.Current.Session["CityPairDepart"] != null)
                {
                    dtDepart = (DataTable)HttpContext.Current.Session["CityPairDepart"];
                    //return dtAll;
                }

                //added by ketee,
                if (LoadCityPairXML(PhysicalApplicationPath, XMLParam.FileName.ALLCITYPAIR) != null
                && LoadCityPairXML(PhysicalApplicationPath, XMLParam.FileName.DEPARTCITYPAIR) != null)
                {
                    CityPairXmlLoaded = true;
                }

                if ((dtAll == null || dtAll.Rows.Count == 0 || dtDepart == null || dtDepart.Rows.Count == 0) && CityPairXmlLoaded == false)
                {
                    using (profiler.Step("ACE:Logon"))
                    {
                        //ACE.SessionManager.LogonResponse resp = aceSession.Logon(LogonReq);

                        ACE.LookUpManager.GetRouteListResponse CityPair = new ACE.LookUpManager.GetRouteListResponse();
                        //if (resp != null)
                        //{
                        //    using (profiler.Step("ACE:GetRouteList"))
                        //    {
                        //        CityPair = aceLookup.GetRouteList("", "", "en-US", resp.SessionID);
                        //        //string xml = GetXMLString(CityPair);
                        //    }
                        //    if (CityPair != null)
                        //    {

                        //        if (SaveXml(CityPair, XMLParam.FileName.CITYPAIR, PhysicalApplicationPath) == "")
                        //        {
                        //            return;
                        //        }
                        //        else
                        //        {
                        //            using (profiler.Step("ACE:LoadCityPairXML"))
                        //            {
                        //                CityPairResponse = LoadCityPairXML(PhysicalApplicationPath, XMLParam.FileName.CITYPAIR);
                        //            }
                        //        }
                        //    }
                        //}
                        if (CityPair != null)
                        {
                            foreach (RouteListExtend rInfo in CityPairResponse.RouteListExtend)
                            {
                                RouteListExtend routeListExtend = new RouteListExtend();

                                DataRow dr = null;
                                DataRow drDepart = null;
                                DataRow[] foundRow = null;
                                DataRow[] foundRowDepart = null;
                                Country_Info CT = new Country_Info();
                                Country_Info CTR = new Country_Info();

                                //dtAll.Rows.Find(rInfo.DepartureStation,rInfo.ArrivalStation);
                                foundRow = dtAll.Select("CityCode = '" + rInfo.DepartureStation + "' AND RCityCode = '" + rInfo.ArrivalStation + "'");
                                foundRowDepart = dtAll.Select("CityCode = '" + rInfo.DepartureStation + "'");
                                using (profiler.Step("GBS:GetCountryInfoByCodeArrival"))
                                {
                                    CTR = GetCountryInfoByCode(rInfo.ArrivalTimeZoneCode, rInfo.ArrivalStationCurrencyCode);
                                }
                                using (profiler.Step("GBS:GetCountryInfoByCodeDeparture"))
                                {
                                    CT = GetCountryInfoByCode(rInfo.DepartureTimeZoneCode, rInfo.DepartureStationCurrencyCode);
                                }
                                if (CT != null && CTR != null)
                                {
                                    dr = dtAll.NewRow();
                                    dr["CityCode"] = rInfo.DepartureStation;
                                    dr["CustomState"] = rInfo.DepartureStationName + "(" + rInfo.DepartureStation + ")" + '|' + CT.countryName;
                                    dr["Name"] = CT.countryName;
                                    dr["CityName"] = rInfo.DepartureStationName;
                                    dr["RCityCode"] = rInfo.ArrivalStation;
                                    dr["RCustomState"] = rInfo.ArrivalStationName + "(" + rInfo.ArrivalStation + ")" + '|' + CTR.countryName;
                                    dr["RName"] = CTR.countryName;
                                    dr["RCityName"] = rInfo.ArrivalStationName;

                                    drDepart = dtDepart.NewRow();
                                    drDepart["CityCode"] = rInfo.DepartureStation;
                                    drDepart["CustomState"] = rInfo.DepartureStationName + "(" + rInfo.DepartureStation + ")" + '|' + CT.countryName;
                                    drDepart["Name"] = CT.countryName;
                                    drDepart["CityName"] = rInfo.DepartureStationName;
                                    drDepart["RCityCode"] = rInfo.ArrivalStation;
                                    drDepart["RCustomState"] = rInfo.ArrivalStationName + "(" + rInfo.ArrivalStation + ")" + '|' + CTR.countryName;
                                    drDepart["RName"] = CTR.countryName;
                                    drDepart["RCityName"] = rInfo.ArrivalStationName;
                                    drDepart["Currency"] = rInfo.DepartureStationCurrencyCode;

                                    routeListExtend = rInfo;
                                    routeListExtend.DepartureCountry = CT.countryName;
                                    routeListExtend.ArrivalCountry = CTR.countryName;
                                    routeListExtend.CustomState = rInfo.DepartureStationName + "(" + rInfo.DepartureStation + ")" + '|' + CT.countryName;
                                    routeListExtend.RCustomState = rInfo.ArrivalStationName + "(" + rInfo.ArrivalStation + ")" + '|' + CTR.countryName;
                                }

                                if (dr != null && dr["CityCode"].ToString() != "" && foundRow.Length == 0)
                                {
                                    dtAll.Rows.Add(dr);
                                    lstAllRouteList.Add(routeListExtend);
                                }

                                if (drDepart != null && drDepart["CityCode"].ToString() != "" && foundRowDepart.Length == 0)
                                {
                                    dtDepart.Rows.Add(drDepart);
                                    lstDepartRouteList.Add(routeListExtend);
                                }

                            }

                            if (dtAll != null)
                            {
                                HttpContext.Current.Session.Add("CityPairAll", dtAll);
                                AllCityPairResponse.RouteListExtend = new RouteListExtend[lstAllRouteList.Count];
                                foreach (RouteListExtend a in lstAllRouteList)
                                {
                                    AllCityPairResponse.RouteListExtend[iAllCityPairResponse] = new RouteListExtend();
                                    AllCityPairResponse.RouteListExtend[iAllCityPairResponse] = a;
                                    iAllCityPairResponse++;
                                }
                            }

                            if (dtDepart != null)
                            {
                                HttpContext.Current.Session.Add("CityPairDepart", dtDepart);
                                DepartCityPairResponse.RouteListExtend = new RouteListExtend[lstDepartRouteList.Count];
                                foreach (RouteListExtend a in lstDepartRouteList)
                                {
                                    DepartCityPairResponse.RouteListExtend[iDepartCityPairResponse] = new RouteListExtend();
                                    DepartCityPairResponse.RouteListExtend[iDepartCityPairResponse] = a;
                                    iDepartCityPairResponse++;
                                }
                            }

                            //added by ketee,
                            using (profiler.Step("GBS:SaveXml"))
                            {
                                SaveXml(AllCityPairResponse, XMLParam.FileName.ALLCITYPAIR, PhysicalApplicationPath);
                                SaveXml(DepartCityPairResponse, XMLParam.FileName.DEPARTCITYPAIR, PhysicalApplicationPath);
                            }
                        }
                    }
                    
                    //return dtAll;
                }
                //return null;
            }


            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex);
                //return null;
            }
        }

        public string SaveXml(object Obj, XMLParam.FileName fileName, string physicalPath)
        {
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Obj.GetType());
            System.IO.StringWriter writer = new System.IO.StringWriter();

            System.Xml.XmlWriter xmlWriter = null;
            //SerializeContainer((System.Xml.XmlWriter)xmlWriter, (Container)Obj);

            x.Serialize(writer, Obj);

            String RandomID = Guid.NewGuid().ToString();
            System.IO.StreamWriter logger = null;
            string logFilePath = string.Empty;
            string filePath = string.Empty;
            try
            {
                switch (fileName)
                {
                    case XMLParam.FileName.CITYPAIR:
                        logFilePath = physicalPath + "\\XML\\" + XMLParam.FileName.CITYPAIR.ToString();
                        filePath = logFilePath + "\\" + XMLParam.FileName.CITYPAIR.ToString() + ".xml";
                        break;
                    default:
                        logFilePath = physicalPath + "\\XML\\" + fileName.ToString();
                        filePath = logFilePath + "\\" + fileName.ToString() + ".xml";
                        break;
                }

                if (System.IO.Directory.Exists(logFilePath) == false) System.IO.Directory.CreateDirectory(logFilePath);
                string xmlstring = writer.ToString();
                System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create);
                logger = new System.IO.StreamWriter(fs, System.Text.Encoding.Unicode);
                //logger = System.IO.File.CreateText(logFilePath);
                logger.WriteLine(writer.ToString());
                //System.Threading.Thread.Sleep(500);
                return filePath;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex);
                //Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                //BasePage.StatusLog = Enums.StatusLog.ERRORLOG;
                //BasePage.MessageLog = ex.Message;
                return "";
            }
            finally
            {
                if (logger != null)
                {
                    logger.Close();
                    logger.Dispose();
                }
                //x = null;
                //writer.Dispose();
            }
        }

        public GetRouteListResponse LookUpCity(string CityDepart, string PhysicalApplicationPath)
        {
            var profiler = MiniProfiler.Current;
            GetRouteListResponse AllCityPairResponse = new GetRouteListResponse();
            GetRouteListResponse DepartCityPairResponse = new GetRouteListResponse();
            try
            {
                using (profiler.Step("ACE:LoadCityPairXMLALLCITYPAIR"))
                {
                    AllCityPairResponse = LoadCityPairXML(PhysicalApplicationPath, XMLParam.FileName.ALLCITYPAIR);
                }
                using (profiler.Step("ACE:LoadCityPairXMLDEPARTCITYPAIR"))
                {
                    DepartCityPairResponse = LoadCityPairXML(PhysicalApplicationPath, XMLParam.FileName.DEPARTCITYPAIR);
                }
                if ((DepartCityPairResponse != null && DepartCityPairResponse.RouteListExtend != null && DepartCityPairResponse.RouteListExtend.Length > 0)
                    || (AllCityPairResponse != null && AllCityPairResponse.RouteListExtend != null && AllCityPairResponse.RouteListExtend.Length > 0))
                {
                    if (CityDepart != "")
                    {
                        AllCityPairResponse.RouteListExtend.Select(a => a.DepartureStation == CityDepart);
                        AllCityPairResponse.RouteListExtend.OrderBy(a => a.ArrivalStationName);
                        return AllCityPairResponse;
                    }
                    else
                    {
                        DepartCityPairResponse.RouteListExtend.OrderBy(a => a.DepartureStationName);
                        return DepartCityPairResponse;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                throw new Exception(ex.Message, ex);
            }
            finally
            {
                AllCityPairResponse = null;
                DepartCityPairResponse = null;
            }
        }

        public GetRouteListResponse LookUpAllCity(string PhysicalApplicationPath)
        {
            var profiler = MiniProfiler.Current;
            GetRouteListResponse AllCityPairResponse = new GetRouteListResponse();
            try
            {
                using (profiler.Step("ACE:LoadCityPairXML"))
                {
                    AllCityPairResponse = LoadCityPairXML(PhysicalApplicationPath, XMLParam.FileName.ALLCITYPAIR);
                }
                if (AllCityPairResponse != null && AllCityPairResponse.RouteListExtend != null && AllCityPairResponse.RouteListExtend.Length > 0)
                {
                    return AllCityPairResponse;
                }

                return null;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                throw new Exception(ex.Message, ex);
            }
            finally
            {
                AllCityPairResponse = null;
            }
        }

        public GetRouteListResponse LoadCityPairXML(string XmlFilePath, XMLParam.FileName fileName)
        {
            //ACEGeneralManager generalManager = new ACEGeneralManager();
            //SHARED.ACELookupService.GetRouteListResponse cityPair;
            GetRouteListResponse result = null;
            XmlFilePath = XmlFilePath + "\\XML\\" + fileName.ToString() + "\\" + fileName.ToString() + ".xml";

            if (File.Exists(XmlFilePath) == false)
            {
                return null;
            }
            else
            {
                StreamReader xmlStream = new StreamReader(XmlFilePath);
                XmlSerializer serializer = new XmlSerializer(typeof(GetRouteListResponse));
                result = (GetRouteListResponse)serializer.Deserialize(xmlStream);
            }

            if (result != null && result.RouteListExtend.Length > 0)
                return result;
            else
                return null;
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

        public DataTable CountryInfoStructure()
        {
            DataTable dt = new DataTable();
            DataColumn[] keys = new DataColumn[2];
            DataColumn column;
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "CityCode";

            // Add the column to the DataTable.Columns collection.
            dt.Columns.Add(column);
            // Add the column to the array.
            keys[0] = column;

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "CustomState";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Name";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "CityName";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "RCityCode";
            dt.Columns.Add(column);
            keys[1] = column;

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "RCustomState";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "RName";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "RCityName";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Currency";
            dt.Columns.Add(column);

            dt.PrimaryKey = keys;
            return dt;
        }

        //        public DataTable GetLookUpCity(string CityDepart)
        //        {
        //            try
        //            {
        //                ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking("");

        //                AALookUpWS.LogonRequest LogonReq = new AALookUpWS.LogonRequest();
        //                LogonReq.Username = apiBooking.Username ;
        //                LogonReq.Password = apiBooking.Password ;


        //                DataTable dt = new DataTable();
        //                DataTable dtAll = new DataTable();

        //                //CI.CityCode, CI.Name + ' (' + CI.CityCode + ')' + '|' + CT.Name AS CustomState, CT.Name, CI.Name AS CityName
        //                DataColumn[] keys = new DataColumn[1];
        //                DataColumn column;
        //                column = new DataColumn();
        //                column.DataType = System.Type.GetType("System.String");
        //                column.ColumnName = "CityCode";

        //                // Add the column to the DataTable.Columns collection.
        //                dt.Columns.Add(column);
        //                dtAll.Columns.Add(column);
        //                // Add the column to the array.
        //                keys[0] = column;

        //                column = new DataColumn();
        //                column.DataType = System.Type.GetType("System.String");
        //                column.ColumnName = "CustomState";
        //                dt.Columns.Add(column);
        //                dtAll.Columns.Add(column);

        //                column = new DataColumn();
        //                column.DataType = System.Type.GetType("System.String");
        //                column.ColumnName = "Name";
        //                dt.Columns.Add(column);
        //                dtAll.Columns.Add(column);

        //                column = new DataColumn();
        //                column.DataType = System.Type.GetType("System.String");
        //                column.ColumnName = "CityName";
        //                dt.Columns.Add(column);
        //                dtAll.Columns.Add(column);

        //                column = new DataColumn();
        //                column.DataType = System.Type.GetType("System.String");
        //                column.ColumnName = "RCityCode";
        //                dt.Columns.Add(column);
        //                dtAll.Columns.Add(column);

        //                column = new DataColumn();
        //                column.DataType = System.Type.GetType("System.String");
        //                column.ColumnName = "RCustomState";
        //                dt.Columns.Add(column);
        //                dtAll.Columns.Add(column);

        //                column = new DataColumn();
        //                column.DataType = System.Type.GetType("System.String");
        //                column.ColumnName = "RName";
        //                dt.Columns.Add(column);
        //                dtAll.Columns.Add(column);

        //                column = new DataColumn();
        //                column.DataType = System.Type.GetType("System.String");
        //                column.ColumnName = "RCityName";
        //                dt.Columns.Add(column);
        //                dtAll.Columns.Add(column);

        //                dt.PrimaryKey = keys;
        //                dtAll.PrimaryKey = keys;

        //                if (HttpContext.Current.Session["CityPairDepart"] != null)
        //                    dt = (DataTable)HttpContext.Current.Session["CityPairDepart"];

        //                if (HttpContext.Current.Session["CityPairAll"] != null)
        //                    dtAll = (DataTable)HttpContext.Current.Session["CityPairAll"];

        //                if (dt == null || dt.Rows.Count == 0 || dtAll == null || dtAll.Rows.Count == 0)
        //                {
        //                    AALookUpWS.LogonResponse resp = aws.Login(LogonReq);
        //                    AALookUpWS.RouteInfoResponse CityPair = new AALookUpWS.RouteInfoResponse();
        //                    if (resp != null)
        //                    {
        //                        CityPair = aws.GetRouteList(CityDepart, "", resp.SessionID);
        //                    }
        //                    if (CityPair != null)
        //                    {
        //                        foreach (AALookUpWS.RouteInfo rInfo in CityPair.RouteInfo)
        //                        {
        //                            DataRow dr = dt.NewRow();
        //                            DataRow foundRow = null;
        //                            Country_Info CT = new Country_Info();
        //                            Country_Info CTR = new Country_Info();

        //CTR = GetCountryInfoByCode(rInfo.ArrivalTimeZoneCode);
        //CT = GetCountryInfoByCode(rInfo.DepartureTimeZoneCode);
        //                            if (CityDepart != "")
        //                            {

        //                                foundRow = dt.Rows.Find(rInfo.ArrivalStation);

        //                                if (CTR != null && foundRow == null)
        //                                {
        //                                    dr["CityCode"] = rInfo.ArrivalStation;
        //                                    dr["CustomState"] = rInfo.ArrivalStationName + "(" + rInfo.ArrivalStation + ")" + CTR.countryName;
        //                                    dr["Name"] = CTR.countryName;
        //                                    dr["CityName"] = rInfo.ArrivalStationName;
        //                                }
        //                            }
        //                            else
        //                            {

        //                                foundRow = dt.Rows.Find(rInfo.DepartureStation);
        //                                if (CT != null)
        //                                {
        //                                    dr["CityCode"] = rInfo.DepartureStation;
        //                                    dr["CustomState"] = rInfo.DepartureStationName + "(" + rInfo.DepartureStation + ")" + CT.countryName;
        //                                    dr["Name"] = CT.countryName;
        //                                    dr["CityName"] = rInfo.DepartureStationName;
        //                                    dr["RCityCode"] = rInfo.ArrivalStation;
        //                                    dr["RCustomState"] = rInfo.ArrivalStationName + "(" + rInfo.ArrivalStation + ")" + CT.countryName;
        //                                    dr["RName"] = CT.countryName;
        //                                    dr["RCityName"] = rInfo.ArrivalStationName;


        //                                }

        //                                if (foundRow == null)
        //                                {
        //                                    if (dr != null && dr["CityCode"].ToString() != "")
        //                                    {
        //                                        dt.Rows.Add(dr);
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    if (dr != null && dr["CityCode"].ToString() != "")
        //                                    {
        //                                        dtAll.Rows.Add(dr);
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        return dt;
        //                    }
        //                }
        //                else
        //                {

        //                }
        //                return null;
        //            }
        //            catch (Exception ex)
        //            {
        //                log.Error(this,ex);
        //                return null;
        //            }
        //        }

        public List<Country_Info> GetAllCityCustom(string CityDepart)
        {
            Country_Info objCountry_Info;
            List<Country_Info> objListCountry_Info = new List<Country_Info>();
            DataTable dt = new DataTable();
            string strCond;
            string strStation;
            try
            {
                if (CityDepart != string.Empty)
                {
                    strStation = "CityPair.ArrivalStation";
                }
                else
                {
                    strStation = "CityPair.DepartureStation";
                }

                strSQL = "SELECT DISTINCT CI.CityCode, CI.Name + ' (' + CI.CityCode + ')' + '|' + CT.Name AS CustomState, CT.Name, CI.Name AS CityName FROM CityPair INNER JOIN " +
                         "City AS CI WITH (nolock) ON " + strStation + " = CI.CityCode INNER JOIN Country AS CT WITH (nolock) ON " +
                         "CT.CountryCode = CI.CountryCode ";
                strCond = "WHERE  (CI.InActive = 0) AND (CT.InActive = 0) ";
                if (CityDepart != string.Empty)
                {
                    strCond = string.Concat(strCond, " AND DepartureStation = '", objSQL.ParseValue(SQLControl.EnumDataType.dtString, CityDepart), "'");
                }
                strSQL = string.Concat(strSQL, strCond, " ORDER BY CT.Name, CityName");

                using (var connection = new SqlConnection(ConnStr))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(strSQL, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    connection.Close();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow drRow in dt.Rows)
                        {
                            objCountry_Info = new Country_Info();
                            objCountry_Info.CityCode = (string)drRow["CityCode"];
                            //objCountry_Info.provinceStateName = (string)drRow["provinceStateName"];
                            objCountry_Info.CustomState = (string)drRow["CustomState"];
                            objListCountry_Info.Add(objCountry_Info);
                        }
                        return objListCountry_Info;
                    }
                    else
                    {
                        log.Info(this, "State does not exist."); //added, for log purpose
                        return null;
                        //throw new ApplicationException("State does not exist.");
                    }
                }
                return null;

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);



                log.Error(this, ex); //added, for log purpose
                return null;
            }
        }

        public string GetCityNameByCode(string cityCode)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var connection = new SqlConnection(ConnStr))
                {
                    connection.Open();
                    strSQL = "SELECT CityCode, Name, CountryCode FROM City";
                    strSQL += " WHERE City.CityCode = '" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, cityCode) + "'";
                    SqlCommand cmd = new SqlCommand(strSQL, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    connection.Close();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.Rows[0]["Name"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex); //added, for log purpose
                return "";
            }

        }

        public string GetCountryCodeByCode(string cityCode)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var connection = new SqlConnection(ConnStr))
                {
                    connection.Open();
                    strSQL = "SELECT CountryCode FROM City";
                    strSQL += " WHERE City.CityCode = '" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, cityCode) + "'";
                    SqlCommand cmd = new SqlCommand(strSQL, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    connection.Close();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.Rows[0]["CountryCode"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex); //added, for log purpose
                return "";
            }

        }

        public DataTable GetCountryCodeByName(string CountryName)
        {

            DataTable dt = new DataTable();
            String strSQL = string.Empty;

            try
            {

                using (var connection = new SqlConnection(ConnStr))
                {
                    connection.Open();
                    strSQL = "SELECT DISTINCT CountryCode FROM COUNTRY WHERE Name='" + CountryName + "'";
                    SqlCommand cmd = new SqlCommand(strSQL, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    connection.Close();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                    else
                    {
                        log.Info(this, "Country does not exist."); //added, for log purpose
                        return null;
                        //throw new ApplicationException("Country does not exist.");
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex); //added, for log purpose
                return null;
            }
        }

        public string GetOrgIDCodeByOrgName(string OrgName)
        {

            DataTable dt = new DataTable();
            String strSQL = string.Empty;

            try
            {

                using (var connection = new SqlConnection(ConnStr))
                {
                    connection.Open();
                    strSQL = "SELECT OrgID FROM AG_PROFILE WHERE OrgName='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, OrgName) + "'";
                    SqlCommand cmd = new SqlCommand(strSQL, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    connection.Close();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.Rows[0]["OrgID"].ToString();
                    }
                    else
                    {
                        log.Info(this, "Country does not exist."); //added, for log purpose
                        return "";
                        //throw new ApplicationException("Country does not exist.");
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex); //added, for log purpose
                return "";
            }
        }

        public Country_Info GetSingleCountryCode(int pId)
        {
            Country_Info objCountry_Info;
            DataTable dt;
            DateTime dateValue;
            String strSQL = string.Empty;
            String strFields = string.Empty;
            String strFilter = string.Empty;
            List<string> lstFields = new List<string>();
            try
            {
                lstFields.Add("CountryCode.Id");
                lstFields.Add("CountryCode.countrycode");
                lstFields.Add("CountryCode.countryName");
                lstFields.Add("CountryCode.provincestatecode");
                lstFields.Add("CountryCode.provinceStateName");

                strFields = GetSqlFields(lstFields);
                strFilter = "WHERE CountryCode.Id='" + pId + "'";
                strSQL = "SELECT " + strFields + " FROM CountryCode " + strFilter;
                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow drRow = dt.Rows[0];

                    objCountry_Info = new Country_Info();
                    objCountry_Info.Id = (int)drRow["Id"];
                    objCountry_Info.countrycode = (string)drRow["countrycode"];
                    objCountry_Info.countryName = (string)drRow["countryName"];
                    objCountry_Info.provincestatecode = (string)drRow["provincestatecode"];
                    objCountry_Info.provinceStateName = (string)drRow["provinceStateName"];
                    return objCountry_Info;
                }
                else
                {
                    log.Info(this, "CountryCode does not exist."); //added, for log purpose
                    return null;
                    //throw new ApplicationException("CountryCode does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex); //added, for log purpose
                return null;
            }
        }


        public Country_Info SaveCountryCode(Country_Info pCountry_Info, SaveType pType)
        {
            objSQL.ClearFields();
            objSQL.ClearCondtions();
            bool rValue = false;
            ArrayList lstSQL = new ArrayList();
            string strSQL = string.Empty;
            try
            {
                objSQL.AddField("Id", pCountry_Info.Id, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("countrycode", pCountry_Info.countrycode, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("countryName", pCountry_Info.countryName, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("provincestatecode", pCountry_Info.provincestatecode, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("provinceStateName", pCountry_Info.provinceStateName, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                switch (pType)
                {
                    case SaveType.Insert:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert, "CountryCode", string.Empty);
                        break;
                    case SaveType.Update:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "CountryCode", "CountryCode.Id='" + pCountry_Info.Id + "'");
                        break;
                }
                lstSQL.Add(strSQL);
                rValue = objDCom.BatchExecute(lstSQL, CommandType.Text, true, false);
                if (rValue == false)
                {
                    return null;
                }
                return GetSingleCountryCode(pCountry_Info.Id);
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                return null;
            }
        }

        public CODEMASTER GetSingleCodeMasterFilter(string Code)
        {
            CODEMASTER objCODEMASTER;
            DataTable dt;
            DateTime dateValue;
            String strSQL = string.Empty;
            String strFields = string.Empty;
            String strFilter = string.Empty;
            List<string> lstFields = new List<string>();
            try
            {
                lstFields.Add("CODEMASTER.CodeType");
                lstFields.Add("CODEMASTER.Code");
                lstFields.Add("CODEMASTER.CodeDesc");
                lstFields.Add("CODEMASTER.CodeSeq");
                lstFields.Add("CODEMASTER.SysCode");
                lstFields.Add("CODEMASTER.SyncCreate");
                lstFields.Add("CODEMASTER.SyncLastUpd");
                lstFields.Add("CODEMASTER.IsHost");

                strFields = GetSqlFields(lstFields);
                strFilter = "WHERE CODEMASTER.Code ='" + Code + "'";
                strSQL = "SELECT " + strFields + " FROM CODEMASTER " + strFilter;
                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow drRow = dt.Rows[0];

                    objCODEMASTER = new CODEMASTER();
                    objCODEMASTER.CodeType = (string)drRow["CodeType"];
                    objCODEMASTER.Code = (string)drRow["Code"];
                    objCODEMASTER.CodeDesc = (string)drRow["CodeDesc"];
                    objCODEMASTER.CodeSeq = (int)drRow["CodeSeq"];
                    objCODEMASTER.SysCode = (byte)drRow["SysCode"];
                    objCODEMASTER.SyncCreate = (DateTime)drRow["SyncCreate"];
                    objCODEMASTER.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
                    objCODEMASTER.IsHost = (byte)drRow["IsHost"];
                    return objCODEMASTER;
                }
                else
                {
                    log.Info(this, "CODEMASTER does not exist."); //added, for log purpose
                    return null;
                    //throw new ApplicationException("CountryCode does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex); //added, for log purpose
                return null;
            }
        }

        public CODEMASTER SaveAllCodeMaster(CODEMASTER pCodeMaster, SaveType pType)
        {
            objSQL.ClearFields();
            objSQL.ClearCondtions();
            bool rValue = false;
            ArrayList lstSQL = new ArrayList();
            string strSQL = string.Empty;
            try
            {
                objSQL.AddField("CodeType", pCodeMaster.CodeType, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Code", pCodeMaster.Code, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("CodeDesc", pCodeMaster.CodeDesc, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("CodeSeq", pCodeMaster.CodeSeq, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SysCode", pCodeMaster.SysCode, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncCreate", pCodeMaster.SyncCreate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncLastUpd", pCodeMaster.SyncLastUpd, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("IsHost", pCodeMaster.IsHost, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                switch (pType)
                {
                    case SaveType.Insert:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert, "CODEMASTER", string.Empty);
                        break;
                    case SaveType.Update:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "CODEMASTER", "CodeType ='RST'" + " AND Code = '" + pCodeMaster.Code + "'");
                        break;
                }
                lstSQL.Add(strSQL);
                rValue = objDCom.BatchExecute(lstSQL, CommandType.Text, true, false);
                if (rValue == false)
                {
                    return null;
                }
                return GetSingleCodeMasterFilter(pCodeMaster.CodeType);
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);

                return null;
            }
        }

        public List<CODEMASTER> GetAllCODEMASTERFilterCode(string code)
        {
            CODEMASTER objCODEMASTER_Info;
            List<CODEMASTER> objListCODEMASTER_Info = new List<CODEMASTER>();
            DataTable dt;
            DateTime dateValue;
            String strSQL = string.Empty;
            String strfilter = string.Empty;
            try
            {
                //if (code != string.Empty)
                //{
                //    strfilter = " AND Code='" + code + "'";
                //}
                strSQL = "SELECT * FROM CODEMASTER WHERE CODETYPE='" + code + "'";
                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow drRow in dt.Rows)
                    {
                        objCODEMASTER_Info = new CODEMASTER();
                        objCODEMASTER_Info.CodeType = (string)drRow["CodeType"];
                        objCODEMASTER_Info.Code = (string)drRow["Code"];
                        objCODEMASTER_Info.CodeDesc = (string)drRow["CodeDesc"];
                        objCODEMASTER_Info.CodeSeq = (int)drRow["CodeSeq"];
                        objCODEMASTER_Info.SysCode = (byte)drRow["SysCode"];
                        objCODEMASTER_Info.rowguid = (Guid)drRow["rowguid"];
                        if (DateTime.TryParse(drRow["SyncCreate"].ToString(), out dateValue)) objCODEMASTER_Info.SyncCreate = (DateTime)drRow["SyncCreate"];
                        if (DateTime.TryParse(drRow["SyncLastUpd"].ToString(), out dateValue)) objCODEMASTER_Info.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
                        objCODEMASTER_Info.IsHost = (byte)drRow["IsHost"];
                        objListCODEMASTER_Info.Add(objCODEMASTER_Info);
                    }
                    return objListCODEMASTER_Info;
                }
                else
                {
                    return null;
                    throw new ApplicationException("CODEMASTER does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                return null;
            }
        }

        public List<CODEMASTER> GetAllCODEMASTERFilter(string code)
        {
            CODEMASTER objCODEMASTER_Info;
            List<CODEMASTER> objListCODEMASTER_Info = new List<CODEMASTER>();
            DataTable dt;
            DateTime dateValue;
            String strSQL = string.Empty;
            String strfilter = string.Empty;
            try
            {
                if (code != string.Empty)
                {
                    strfilter = " AND Code='" + code + "'";
                }
                strSQL = "SELECT * FROM CODEMASTER WHERE CODETYPE='OPT' " + strfilter;
                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow drRow in dt.Rows)
                    {
                        objCODEMASTER_Info = new CODEMASTER();
                        objCODEMASTER_Info.CodeType = (string)drRow["CodeType"];
                        objCODEMASTER_Info.Code = (string)drRow["Code"];
                        objCODEMASTER_Info.CodeDesc = (string)drRow["CodeDesc"];
                        objCODEMASTER_Info.CodeSeq = (int)drRow["CodeSeq"];
                        objCODEMASTER_Info.SysCode = (byte)drRow["SysCode"];
                        objCODEMASTER_Info.rowguid = (Guid)drRow["rowguid"];
                        if (DateTime.TryParse(drRow["SyncCreate"].ToString(), out dateValue)) objCODEMASTER_Info.SyncCreate = (DateTime)drRow["SyncCreate"];
                        if (DateTime.TryParse(drRow["SyncLastUpd"].ToString(), out dateValue)) objCODEMASTER_Info.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
                        objCODEMASTER_Info.IsHost = (byte)drRow["IsHost"];
                        objListCODEMASTER_Info.Add(objCODEMASTER_Info);
                    }
                    return objListCODEMASTER_Info;
                }
                else
                {
                    return null;
                    throw new ApplicationException("CODEMASTER does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);

                return null;
            }
        }

        //20170721 - Sienny
        public DataTable GetCodeDescByCodeName(string codename = "")
        {
            DataTable dt;
            String strSQL = string.Empty;

            try
            {
                string strfilter = "";
                if (codename != "") strfilter = " AND Code='" + codename + "'";
                strSQL = "SELECT * FROM CODEMASTER WHERE CodeType='FEE'" + strfilter;
                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt;
                }
                else
                {
                    return null;
                    throw new ApplicationException("CODEMASTER does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                return null;
            }
        }

        //20170721 - Sienny
        public string getCodeName(string CodeType, string CodeName)
        {
            String strSQL = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                strSQL = "SELECT CodeType, Code, CodeDesc FROM CODEMASTER WHERE CodeType='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, CodeType.ToUpper()) + "' AND Code='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, CodeName.ToUpper()) + "'";
                dt = objDCom.Execute(strSQL, System.Data.CommandType.Text, true);
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["Code"].ToString();
                }
                else
                {
                    //if fee code not found then insert new
                    CODEMASTER codeMaster = new CODEMASTER();
                    strSQL = "SELECT CodeType, Code, CodeDesc, CodeSeq, SysCode FROM CODEMASTER ORDER BY Code";
                    dt = objDCom.Execute(strSQL, System.Data.CommandType.Text, true);
                    if (dt.Rows.Count > 0)
                    {
                        codeMaster.CodeType = CodeType.ToUpper();
                        codeMaster.Code = CodeName.ToUpper();
                        codeMaster.CodeDesc = CodeName.ToUpper();
                        codeMaster.CodeSeq = 0;
                        codeMaster.SysCode = 1;

                        codeMaster = SaveAllCodeMaster(codeMaster, SaveType.Insert);

                        if (codeMaster != null)
                        {
                            return codeMaster.Code.ToString();
                        }
                    }

                    log.Warning(this, "CodeType [" + CodeType.ToUpper() + "], Code [" + CodeName.ToUpper() + "]");
                    return " ";
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                return "";
            }

        }

        //20170721 - Sienny
        public DataTable GetAllFeesData(string TransID = "")
        {
            DataTable dt;
            String strSQL = string.Empty;

            try
            {
                string strfilter = "";
                if (TransID != "") strfilter = " AND bf.TransID='" + TransID + "' ";
                strSQL = "SELECT DISTINCT bf.TransID, ISNULL(cm.CodeType, '') CodeType, bf.FeeType, bf.FeeCode, ISNULL(cm.CodeDesc, bf.FeeType) CodeDesc, bf.FeeDesc, bf.Origin, bf.Transit, bf.Destination, sum(bf.FeeAmt) FeeAmt, CAST((sum(bf.FeeAmt)/sum(bf.FeeQty)) as decimal(12,2)) FeeAmtPerPax  ";
                strSQL += "FROM BK_TRANSFEES bf LEFT JOIN CODEMASTER cm ON bf.feecode = cm.code ";
                strSQL += "WHERE (cm.CodeType='FEE' OR cm.CodeType IS NULL)" + strfilter;
                strSQL += "GROUP BY bf.TransID, cm.CodeType, bf.FeeType, bf.FeeCode, cm.CodeDesc, bf.FeeDesc, bf.Origin, bf.Transit, bf.Destination ";
                strSQL += "ORDER BY bf.TransID";
                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt;
                }
                else
                {
                    return null;
                    throw new ApplicationException("BK_TRANSFEES does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                return null;
            }
        }

        public DataTable GetAllOrgID()
        {
            DataTable dt;
            String strSQL = string.Empty;

            try
            {
                
                strSQL = "SELECT DISTINCT OrgID, OrgName FROM AG_PROFILE ORDER BY OrgName";
                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
                if (dt != null && dt.Rows.Count > 0)
                {

                    return dt;
                }
                else
                {
                    return null;
                    throw new ApplicationException("AG_PROFILE does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                return null;
            }
        }

        public DataTable GetAllListedCountry()
        {
            DataTable dt;
            String strSQL = string.Empty;

            try
            {

                strSQL = "SELECT DISTINCT COUNTRY.CountryCode, COUNTRY.Name FROM COUNTRY INNER JOIN DEPOPAYSCHEME DP WITH (NOLOCK) ON COUNTRY.CountryCode = DP.CountryCode";
                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
                if (dt != null && dt.Rows.Count > 0)
                {

                    return dt;
                }
                else
                {
                    return null;
                    throw new ApplicationException("COUNTRY does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                return null;
            }
        }

        public DataTable GetAllAgentbyOrgID(string OrgID)
        {
            DataTable dt;
            String strSQL = string.Empty;

            try
            {

                strSQL = "SELECT DISTINCT AgentID, Username FROM AG_PROFILE WHERE OrgID = '" + OrgID + "'";
                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
                if (dt != null && dt.Rows.Count > 0)
                {

                    return dt;
                }
                else
                {
                    return null;
                    throw new ApplicationException("AG_PROFILE does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                return null;
            }
        }

        public string GetAgentIDbyUsername(string Username)
        {
            DataTable dt;
            String strSQL = string.Empty;

            try
            {

                strSQL = "SELECT AgentID, Username FROM AG_PROFILE WHERE Username = '" + Username + "'";
                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
                if (dt != null && dt.Rows.Count > 0)
                {

                    return dt.Rows[0]["AgentID"].ToString();
                }
                else
                {
                    return "";
                    throw new ApplicationException("AG_PROFILE does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                return "";
            }
        }

        public DataTable GetAllSettingFilter(int SYSSet, string WebID)
        {
            List<Settings> objListSYS_PREFT_Info = new List<Settings>();
            DataTable dt;
            String strSQL = string.Empty;

            try
            {
                string strfilter = "";
                if (WebID != "0")
                    strfilter = " AND GRPID='" + WebID + "'";
                strSQL = "SELECT AppID, GRPID, SYSKey, SYSValue, SYSValueEx, SYSSet, rowguid, SyncCreate, " +
                "SyncLastUpd, IsHost, LastSyncBy,(CAST(AppID AS varchar(5)) + ', ' + CAST(GRPID AS varchar(5))+', '+SYSKey) as CompositeKey" +
                " FROM SYS_PREFT WHERE SYSSet=" + SYSSet + strfilter;
                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
                if (dt != null && dt.Rows.Count > 0)
                {

                    return dt;
                }
                else
                {
                    return null;
                    throw new ApplicationException("SYS_PREFT does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                return null;
            }
        }

        public Settings GetSingleSYS_PREFT(short pAppID, string pWebID, string pSYSKey)
        {
            Settings objSYS_PREFT_Info;
            DataTable dt;
            DateTime dateValue;
            String strSQL = string.Empty;
            String strFields = string.Empty;
            String strFilter = string.Empty;
            List<string> lstFields = new List<string>();
            try
            {
                lstFields.Add("SYS_PREFT.AppID");
                lstFields.Add("SYS_PREFT.GRPID");
                lstFields.Add("SYS_PREFT.SYSKey");
                lstFields.Add("SYS_PREFT.SYSValue");
                lstFields.Add("SYS_PREFT.SYSValueEx");
                lstFields.Add("SYS_PREFT.SYSSet");
                lstFields.Add("SYS_PREFT.rowguid");
                lstFields.Add("SYS_PREFT.SyncCreate");
                lstFields.Add("SYS_PREFT.SyncLastUpd");
                lstFields.Add("SYS_PREFT.IsHost");
                lstFields.Add("SYS_PREFT.LastSyncBy");
                lstFields.Add("SYS_PREF.SYSDesc");
                strFields = GetSqlFields(lstFields);
                strFilter = "WHERE SYS_PREFT.SYSKey='" + pSYSKey + "' " + "AND SYS_PREFT.AppID='" + pAppID + "' AND SYS_PREFT.GRPID='" + pWebID + "'";
                strSQL = "SELECT " + strFields + " FROM SYS_PREFT " +
                    "JOIN SYS_PREF (NOLOCK) on SYS_PREF.SYSKey=SYS_PREFT.SYSKey " + strFilter;
                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow drRow = dt.Rows[0];

                    objSYS_PREFT_Info = new Settings();
                    objSYS_PREFT_Info.AppID = (short)drRow["AppID"];
                    objSYS_PREFT_Info.GRPID = (string)drRow["GRPID"];
                    objSYS_PREFT_Info.SYSKey = (string)drRow["SYSKey"];
                    objSYS_PREFT_Info.SYSDesc = (string)drRow["SYSDesc"];
                    objSYS_PREFT_Info.SYSValue = (string)drRow["SYSValue"];
                    objSYS_PREFT_Info.SYSValueEx = (string)drRow["SYSValueEx"];
                    objSYS_PREFT_Info.SYSSet = (byte)drRow["SYSSet"];
                    objSYS_PREFT_Info.rowguid = (Guid)drRow["rowguid"];
                    if (DateTime.TryParse(drRow["SyncCreate"].ToString(), out dateValue)) objSYS_PREFT_Info.SyncCreate = (DateTime)drRow["SyncCreate"];
                    if (DateTime.TryParse(drRow["SyncLastUpd"].ToString(), out dateValue)) objSYS_PREFT_Info.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
                    objSYS_PREFT_Info.IsHost = (byte)drRow["IsHost"];
                    objSYS_PREFT_Info.LastSyncBy = (string)drRow["LastSyncBy"];
                    return objSYS_PREFT_Info;
                }
                else
                {
                    return null;
                    throw new ApplicationException("SYS_PREFT does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                return null;
            }
        }

        public string GetXMLString(object Obj)
        {
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Obj.GetType());
            System.IO.StringWriter writer = new System.IO.StringWriter();
            x.Serialize(writer, Obj);

            return writer.ToString();
        }

        public Boolean SaveGB4SETTING(GB4SETTING gB4SETTING, SaveType pType, GB4SETTING gB4 = null)
        {
            bool rValue = false;
            ArrayList lstSQL = new ArrayList();
            string strSQL = string.Empty;
            try
            {                
                objSQL.AddField("AppID", gB4SETTING.AppID, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("CountryCode", gB4SETTING.CountryCode, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("CountryName", gB4SETTING.CountryName, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Origin", gB4SETTING.Origin, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("OrgID", gB4SETTING.OrgID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("AgentID", gB4SETTING.AgentID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("NoofPax", gB4SETTING.NoofPax, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncCreate", gB4SETTING.SyncCreate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncLastUpd", gB4SETTING.SyncLastUpd, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Status", gB4SETTING.status, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastSyncBy", gB4SETTING.LastSyncBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("EffectiveDate", gB4SETTING.EffectiveDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("ExpiryDate", gB4SETTING.ExpiryDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                switch (pType)
                {
                    case SaveType.Insert:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert, "GB4SETTING", string.Empty);
                        break;
                    case SaveType.Update:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "GB4SETTING", "GB4SETTING.AgentID='" + gB4.AgentID + "' AND GB4SETTING.CountryCode='" + gB4.CountryCode + "' AND GB4SETTING.Origin='" + gB4.Origin + "' AND GB4SETTING.OrgID='" + gB4.OrgID + "'");
                        if (gB4SETTING.AgentID == "")
                        {
                            string subFirst = strSQL.Substring(0, 22);
                            string subRest = strSQL.Substring(22);
                            strSQL = subFirst + "AgentID = '', " + subRest;
                        }
                        break;
                }
                lstSQL.Add(strSQL);
                
                rValue = objDCom.BatchExecute(lstSQL, CommandType.Text, true, false);
                if (rValue == false)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                return false;
            }
        }

        public Boolean DeleteGB4SETTING(List<GB4SETTING> LstgB4SETTING)
        {
            bool rValue = false;
            ArrayList lstSQL = new ArrayList();
            string strSQL = string.Empty;
            try
            {
                foreach (GB4SETTING gB4Setting in LstgB4SETTING)
                {
                    strSQL = "DELETE GB4SETTING WHERE OrgID = '" + gB4Setting.OrgID + "' AND CountryCode = '" + gB4Setting.CountryCode + "' AND Origin = '" + gB4Setting.Origin + "'";
                    lstSQL.Add(strSQL);
                }

                rValue = objDCom.BatchExecute(lstSQL, CommandType.Text, true, false);
                if (rValue == false)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                return false;
            }
        }

        public bool SaveSYS_AUDITLOG(List<AUDITLOG> lstSYS_AUDITLOG_Info)
        {
            bool rValue = false;
            ArrayList lstSQL = new ArrayList();
            string strSQL = string.Empty;
            try
            {
                foreach (AUDITLOG pSYS_AUDITLOG_Info in lstSYS_AUDITLOG_Info)
                {
                    objSQL.AddField("TransID", pSYS_AUDITLOG_Info.TransID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("SeqNo", pSYS_AUDITLOG_Info.SeqNo, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("Action", pSYS_AUDITLOG_Info.Action, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("RefCode", pSYS_AUDITLOG_Info.RefCode, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("Table_Name", pSYS_AUDITLOG_Info.Table_Name, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("CreatedBy", pSYS_AUDITLOG_Info.CreatedBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("SQL", pSYS_AUDITLOG_Info.SQL, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("CreatedDate", pSYS_AUDITLOG_Info.CreatedDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("Priority", pSYS_AUDITLOG_Info.Priority, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("Flag", pSYS_AUDITLOG_Info.Flag, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                    strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert, "SYS_AUDITLOG", string.Empty);
                    lstSQL.Add(strSQL);
                }
                rValue = objDCom.BatchExecute(lstSQL, CommandType.Text, true, false);
                if (rValue == false)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                return false;
            }
        }


        public Settings SaveSYS_PREFT(Settings pSYS_PREFT_Info, SaveType pType)
        {
            bool rValue = false;
            ArrayList lstSQL = new ArrayList();
            string strSQL = string.Empty;
            AUDITLOG AUDITLOGInfo = new AUDITLOG();
            List<AUDITLOG> lstAuditLog = new List<AUDITLOG>();
            try
            {
                Settings CheckSetting = GetSingleSYS_PREFT(pSYS_PREFT_Info.AppID, pSYS_PREFT_Info.GRPID, pSYS_PREFT_Info.SYSKey);

                objSQL.AddField("GRPID", pSYS_PREFT_Info.GRPID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SYSKey", pSYS_PREFT_Info.SYSKey, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SYSValue", pSYS_PREFT_Info.SYSValue, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SYSValueEx", pSYS_PREFT_Info.SYSValueEx, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SYSSet", pSYS_PREFT_Info.SYSSet, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncCreate", pSYS_PREFT_Info.SyncCreate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncLastUpd", pSYS_PREFT_Info.SyncLastUpd, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("IsHost", pSYS_PREFT_Info.IsHost, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastSyncBy", pSYS_PREFT_Info.LastSyncBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                switch (pType)
                {
                    case SaveType.Insert:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert, "SYS_PREFT", string.Empty);
                        break;
                    case SaveType.Update:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "SYS_PREFT", "SYS_PREFT.AppID='" + pSYS_PREFT_Info.AppID + "' AND SYS_PREFT.GRPID='" + pSYS_PREFT_Info.GRPID + "' AND SYS_PREFT.SYSKey='" + pSYS_PREFT_Info.SYSKey + "'");
                        break;
                }
                lstSQL.Add(strSQL);

                AUDITLOGInfo.TransID = DateTime.Now.ToString("yyyyMMddHHmmsss");
                AUDITLOGInfo.SeqNo = 0;
                AUDITLOGInfo.Action = 1;
                AUDITLOGInfo.RefCode = "";
                AUDITLOGInfo.Table_Name = "SYS_PREFT";
                AUDITLOGInfo.SQL = strSQL;
                AUDITLOGInfo.CreatedBy = pSYS_PREFT_Info.LastSyncBy;
                AUDITLOGInfo.CreatedDate = DateTime.Now;
                AUDITLOGInfo.Priority = 0;
                lstAuditLog.Add(AUDITLOGInfo);
                rValue = objDCom.BatchExecute(lstSQL, CommandType.Text, true, false);
                if (rValue == false)
                {
                    return null;
                }
                if (CheckSetting.SYSValue != pSYS_PREFT_Info.SYSValue)
                    SaveSYS_AUDITLOG(lstAuditLog);

                return GetSingleSYS_PREFT(pSYS_PREFT_Info.AppID, pSYS_PREFT_Info.GRPID, pSYS_PREFT_Info.SYSKey);
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                return null;
            }
        }

        public Settings SaveSYS_PREFTrestrict(Settings pSYS_PREFT_Info, SaveType pType)
        {
            bool rValue = false;
            ArrayList lstSQL = new ArrayList();
            string strSQL = string.Empty;
            try
            {
                //Settings CheckSetting = GetSingleSYS_PREFT(pSYS_PREFT_Info.AppID, pSYS_PREFT_Info.GRPID, pSYS_PREFT_Info.SYSKey);

                objSQL.AddField("GRPID", pSYS_PREFT_Info.GRPID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SYSKey", pSYS_PREFT_Info.SYSKey, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SYSValue", pSYS_PREFT_Info.SYSValue, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SYSValueEx", pSYS_PREFT_Info.SYSValueEx, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SYSSet", pSYS_PREFT_Info.SYSSet, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncCreate", pSYS_PREFT_Info.SyncCreate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncLastUpd", pSYS_PREFT_Info.SyncLastUpd, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("IsHost", pSYS_PREFT_Info.IsHost, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastSyncBy", pSYS_PREFT_Info.LastSyncBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                switch (pType)
                {
                    case SaveType.Insert:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert, "SYS_PREFT", string.Empty);
                        break;
                    case SaveType.Update:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "SYS_PREFT", "SYS_PREFT.SYSKey='" + pSYS_PREFT_Info.SYSKey + "'");
                        break;
                }
                lstSQL.Add(strSQL);

                rValue = objDCom.BatchExecute(lstSQL, CommandType.Text, true, false);
                if (rValue == false)
                {
                    return null;
                }

                return GetSingleSYS_PREFT(pSYS_PREFT_Info.AppID, pSYS_PREFT_Info.GRPID, pSYS_PREFT_Info.SYSKey);
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                return null;
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

        public string getSysValueByKey(string key)
        {
            String strSQL = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                using (var connection = new SqlConnection(ConnStr))
                {
                    connection.Open();
                    strSQL = "SELECT SYSValue FROM SYS_PREFT WHERE SYSKey = '" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, key) + "'";
                    SqlCommand cmd = new SqlCommand(strSQL, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    connection.Close();

                    if (dt.Rows.Count > 0)
                    {
                        return dt.Rows[0]["SYSValue"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                return "";

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                return "";
            }
            //finally
            //{
            //    EndSQLControl();
            //    EndConnection();
            //}
        }

        public string getSysValueByKeyAndCarrierCode(string key, string carrierCode)
        {
            String strSQL = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                using (var connection = new SqlConnection(ConnStr))
                {
                    connection.Open();
                    strSQL = "SELECT SYS_PREFT.SYSValue FROM OPTGroup INNER JOIN SYS_PREFT ON OPTGroup.GroupName = SYS_PREFT.GrpID WHERE OPTGroup.CarrierCode = '" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, carrierCode) + "' AND SYS_PREFT.SYSKey = '" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, key) + "'";
                    SqlCommand cmd = new SqlCommand(strSQL, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    connection.Close();

                    if (dt.Rows.Count > 0)
                    {
                        return dt.Rows[0]["SYSValue"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex);
                return "";
            }
            //finally
            //{
            //    EndSQLControl();
            //    EndConnection();
            //}
        }

        public string getSysValueByKeyAndGroupID(string key, string groupID)
        {
            String strSQL = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                //if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
                //{
                //    StartSQLControl();

                using (var connection = new SqlConnection(ConnStr))
                {
                    connection.Open();
                    strSQL = "SELECT SYS_PREFT.SYSValue FROM SYS_PREFT WHERE SYS_PREFT.GrpID  = '" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, groupID) + "' AND SYS_PREFT.SYSKey = '" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, key) + "'";
                    SqlCommand cmd = new SqlCommand(strSQL, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    connection.Close();

                    if (dt.Rows.Count > 0)
                    {
                        return dt.Rows[0]["SYSValue"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                return "";
                //}
                //return "";
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                return "";
            }
            //finally
            //{
            //    EndSQLControl();
            //    EndConnection();
            //}
        }

        public string getOPTGroupByCarrierCode(string carrierCode)
        {
            String strSQL = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                using (var connection = new SqlConnection(ConnStr))
                {
                    connection.Open();
                    strSQL = "SELECT OPTGroup.GroupName FROM OPTGroup WHERE OPTGroup.CarrierCode = '" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, carrierCode) + "'";
                    SqlCommand cmd = new SqlCommand(strSQL, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    connection.Close();

                    if (dt.Rows.Count > 0)
                    {
                        return dt.Rows[0]["GroupName"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                return "";
            }
            //finally
            //{
            //    EndSQLControl();
            //    EndConnection();
            //}
        }

        //20170523 - Sienny
        public DataTable GetOPTGroup()
        {
            DataTable dt = new DataTable();
            String strSQL = string.Empty;

            try
            {
                using (var connection = new SqlConnection(ConnStr))
                {
                    connection.Open();
                    strSQL = "SELECT CarrierCode, GroupName, WebID FROM OPTGroup ORDER BY CarrierCode ";
                    SqlCommand cmd = new SqlCommand(strSQL, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    connection.Close();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                    else
                    {
                        log.Info(this, "OPTGroup does not exist."); //added, for log purpose
                        return null;
                    }
                }
                return null;

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex); //added, for log purpose
                return null;
            }
        }

        /// <summary>
        /// get visibility from OPTGroup
        /// </summary>
        /// <param name="carrierCode"></param>
        /// <returns></returns>
        public Boolean getVisibilityByCarrierCode(string carrierCode)
        {
            String strSQL = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                using (var connection = new SqlConnection(ConnStr))
                {
                    connection.Open();
                    strSQL = "SELECT OPTGroup.WebID FROM OPTGroup WHERE OPTGroup.CarrierCode = '" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, carrierCode) + "'";
                    SqlCommand cmd = new SqlCommand(strSQL, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    connection.Close();

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["WebID"].ToString() == "1")
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                return false;
            }

        }

        /// <summary>
        /// added by diana 20140121
        /// </summary>
        /// <param name="key"></param>
        /// <param name="carrierCode"></param>
        /// <returns></returns>
        public decimal getDeposit(string Transid, int totalPax, string currency, string cityCode, string connectingCityCode = "")
        {
            String strSQL = string.Empty;
            DataTable dt = new DataTable();
            DateTime bookingDate = Convert.ToDateTime("1900-01-01");
            DateTime CheckDate = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["CheckDate"].ToString());
            Boolean newGBS = true;
            try
            {
                //if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
                //{
                //    StartSQLControl();

                bookingDate = getBookingDate(Transid, ref newGBS);

                if (bookingDate >= CheckDate)
                {
                    strSQL = "SELECT DepositValue FROM DEPODURATION WHERE Currency='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, currency) + "' AND HaulCode=(SELECT HaulCode FROM COUNTRYHAUL WHERE CityCode='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, cityCode) + "')";
                }
                else
                {
                    strSQL = "SELECT DepositValue FROM DEPODURATION WHERE Currency='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, currency) + "' AND HaulCode=(SELECT HaulCode FROM COUNTRYHAUL WHERE CityCode='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, cityCode) + "')";
                }

                dt = objDCom.Execute(strSQL, System.Data.CommandType.Text, true);
                if (dt.Rows.Count > 0)
                {
                    if (connectingCityCode == "")
                        return Decimal.Parse(dt.Rows[0]["DepositValue"].ToString()) * totalPax;
                    else
                        return Decimal.Parse(dt.Rows[0]["DepositValue"].ToString()) * 2 * totalPax;
                }
                else
                {
                    return 0;
                }
                //}
                //return 0;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex);
                return 0;
            }
            //finally
            //{
            //    EndSQLControl();
            //    EndConnection();
            //}
        }

        //added by ketee, old deposit 
        public decimal getDeposit(int totalPax, string currency, string cityCode, string connectingCityCode = "")
        {
            String strSQL = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                //if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
                //{
                //    StartSQLControl();

                strSQL = "SELECT DepositValue FROM COUNTRYDEPOSIT WHERE Currency='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, currency) + "' AND HaulCode=(SELECT HaulCode FROM COUNTRYHAUL WHERE CityCode='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, cityCode) + "')";
                dt = objDCom.Execute(strSQL, System.Data.CommandType.Text, true);
                if (dt.Rows.Count > 0)
                {
                    if (connectingCityCode == "")
                        return Decimal.Parse(dt.Rows[0]["DepositValue"].ToString()) * totalPax;
                    else
                        return Decimal.Parse(dt.Rows[0]["DepositValue"].ToString()) * 2 * totalPax;
                }
                else
                {
                    return 0;
                }
                //}
                //return 0;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex);
                return 0;
            }
            //finally
            //{
            //    EndSQLControl();
            //    EndConnection();
            //}
        }

        public decimal getDepositByDuration(string Transid, decimal FullPrice, int totalPax, string currency, string cityCode, string groupCode, decimal duration, string sellkey, string connectingCityCode = "")
        {
            String strSQL = string.Empty;
            DataTable dt = new DataTable();
            int flightType = 0;
            string haulCode = "S";
            DateTime bookingDate = Convert.ToDateTime("1900-01-01");
            DateTime CheckDate = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["CheckDate"].ToString());
            Boolean newGBS = true;
            try
            {
                duration = duration / 60;
                //if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
                //{
                //    StartSQLControl();

                //if (carrierCode == "D7" || carrierCode == "XJ" || carrierCode == "XT") haulCode = "L";

                //to check peak or normal season, if peak, should ends with H
                if (sellkey.Split('~')[3].ToString().EndsWith("H")) flightType = 1;

                bookingDate = getBookingDate(Transid, ref newGBS);

                if (bookingDate >= CheckDate)
                {
                    strSQL = "SELECT TOP 1 DepositValue, DepositValueOld, ValueType, CheckMin, CheckMax, MinDeposit, MaxDeposit FROM DEPODURATION WHERE GroupCode='" + groupCode + "' AND Currency='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, currency) + "' AND FlightType='" + flightType + "' AND FlightDuration >= " + duration + " ORDER BY FlightDuration";
                }
                else
                {
                    strSQL = "SELECT TOP 1 DepositValue, DepositValueOld, ValueType, CheckMin, CheckMax, MinDeposit, MaxDeposit FROM DEPODURATION WHERE GroupCode='" + groupCode + "' AND Currency='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, currency) + "' AND FlightType='" + flightType + "' AND FlightDuration >= " + duration + " ORDER BY FlightDuration";
                }

                decimal deposit = 0;
                dt = objDCom.Execute(strSQL, System.Data.CommandType.Text, true);
                if (dt.Rows.Count > 0)
                {
                    if (Decimal.Parse(dt.Rows[0]["ValueType"].ToString()) == 0)
                        deposit = Math.Round(Decimal.Parse(dt.Rows[0]["DepositValueOld"].ToString()) * totalPax, 2);
                    else
                        deposit = Math.Round(Decimal.Parse(dt.Rows[0]["DepositValue"].ToString()) * FullPrice / 100, 2);

                    //if (Decimal.Parse(dt.Rows[0]["CheckMin"].ToString()) == 1 && deposit < Decimal.Parse(dt.Rows[0]["MinDeposit"].ToString()))
                    //    deposit = Decimal.Parse(dt.Rows[0]["MinDeposit"].ToString());
                    //else if (Decimal.Parse(dt.Rows[0]["CheckMax"].ToString()) == 1 && deposit > Decimal.Parse(dt.Rows[0]["MaxDeposit"].ToString()))
                    //    deposit = Decimal.Parse(dt.Rows[0]["MaxDeposit"].ToString());
                    return deposit;
                }
                else
                {
                    return deposit;
                }
                //}
                //return 0;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex);
                return 0;
            }
            //finally
            //{
            //    EndSQLControl();
            //    EndConnection();
            //}
        }

        public DataTable getDepositLimit(string Transid, decimal FullPrice, int totalPax, string currency, string cityCode, string groupCode, decimal duration, string sellkey, string connectingCityCode = "")
        {
            String strSQL = string.Empty;
            DataTable dt = new DataTable();
            int flightType = 0;
            string haulCode = "S";
            DateTime bookingDate = Convert.ToDateTime("1900-01-01");
            DateTime CheckDate = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["CheckDate"].ToString());
            Boolean newGBS = true;
            try
            {
                duration = duration / 60;
                //if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
                //{
                //    StartSQLControl();

                //if (carrierCode == "D7" || carrierCode == "XJ" || carrierCode == "XT") haulCode = "L";

                //to check peak or normal season, if peak, should ends with H
                if (sellkey.Split('~')[3].ToString().EndsWith("H")) flightType = 1;

                bookingDate = getBookingDate(Transid, ref newGBS);

                if (bookingDate >= CheckDate)
                {
                    strSQL = "SELECT TOP 1 DepositValue, DepositValueOld, ValueType, CheckMin, CheckMax, MinDeposit, MaxDeposit, CheckMin2, CheckMax2, MinDeposit2, MaxDeposit2 FROM DEPODURATION WHERE GroupCode='" + groupCode + "' AND Currency='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, currency) + "' AND FlightType='" + flightType + "' AND FlightDuration >= " + duration + " ORDER BY FlightDuration";
                }
                else
                {
                    strSQL = "SELECT TOP 1 DepositValue, DepositValueOld, ValueType, CheckMin, CheckMax, MinDeposit, MaxDeposit, CheckMin2, CheckMax2, MinDeposit2, MaxDeposit2 FROM DEPODURATION WHERE GroupCode='" + groupCode + "' AND Currency='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, currency) + "' AND FlightType='" + flightType + "' AND FlightDuration >= " + duration + " ORDER BY FlightDuration";
                }

                dt = objDCom.Execute(strSQL, System.Data.CommandType.Text, true);
                if (dt.Rows.Count > 0)
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


                log.Error(this, ex);
            }
            return null;
            //finally
            //{
            //    EndSQLControl();
            //    EndConnection();
            //}
        }
        public decimal getDepositBackup(string Transid, int totalPax, string currency, string cityCode, string connectingCityCode = "")
        {
            String strSQL = string.Empty;
            DataTable dt = new DataTable();
            DateTime bookingDate = Convert.ToDateTime("1900-01-01");
            DateTime CheckDate = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["CheckDate"].ToString());
            Boolean newGBS = true;

            try
            {
                //if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
                //{
                //    StartSQLControl();
                bookingDate = getBookingDate(Transid, ref newGBS);

                if (bookingDate >= CheckDate)
                {
                    strSQL = "SELECT DepositValue FROM DEPODURATION WHERE Currency='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, currency) + "' AND HaulCode=(SELECT HaulCode FROM COUNTRYHAUL WHERE CityCode='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, cityCode) + "')";
                }
                else
                {
                    strSQL = "SELECT DepositValue FROM DEPODURATION WHERE Currency='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, currency) + "' AND HaulCode=(SELECT HaulCode FROM COUNTRYHAUL WHERE CityCode='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, cityCode) + "')";
                }

                dt = objDCom.Execute(strSQL, System.Data.CommandType.Text, true);
                if (dt.Rows.Count > 0)
                {
                    if (connectingCityCode == "")
                        return Decimal.Parse(dt.Rows[0]["DepositValue"].ToString()) * totalPax;
                    else
                        return Decimal.Parse(dt.Rows[0]["DepositValue"].ToString()) * 2 * totalPax;
                }
                else
                {
                    return 0;
                }
                //}
                //return 0;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex);
                return 0;
            }
            //finally
            //{
            //    EndSQLControl();
            //    EndConnection();
            //}
        }

        //added by ketee, retrieve booking date for payment scheme validation
        public DateTime getBookingDate(string Transid, ref Boolean newGBS)
        {
            String strSQL = string.Empty;
            DataTable dt = new DataTable();
            DateTime defaultDate = Convert.ToDateTime("1900-01-01");
            try
            {
                if (StartConnection() == true)
                {
                    StartSQLControl();
                }
                    strSQL = "SELECT BookingDate, IsOverride FROM BK_TRANSMAIN WHERE TransID='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, Transid) + "'";
                dt = objDCom.Execute(strSQL, System.Data.CommandType.Text, true);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["IsOverride"] != null && !string.IsNullOrEmpty(dt.Rows[0]["IsOverride"].ToString()))
                    {
                        if (dt.Rows[0]["IsOverride"].ToString() == "1")
                            newGBS = true;
                        else
                            newGBS = false;
                    }
                    return (DateTime)dt.Rows[0]["BookingDate"];
                }
                else
                {
                    return defaultDate;
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                return defaultDate;
            }
            finally
            {

                dt = null;
                EndSQLControl();
                EndConnection();
            }
        }

        public string GenerateRandom(int Length)
        {
            char[] constant = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(constant.Length);

            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(0, constant.Length - 1)]);
            }

            return newRandom.ToString().ToLower();
        }

        public decimal RoundUp(decimal amount)
        {
            return Math.Round(amount, 2, MidpointRounding.AwayFromZero);
        }

        #region Hashing
        public string GenerateMac(string strSharedKey, string strAID, string strName)
        {
            string strMac = string.Empty;
            string strEncrypt = strSharedKey + strAID;
            strMac = Sha256AddSecret(strEncrypt);

            return strMac;
        }

        public string Sha256AddSecret(string strChange)
        {
            //Change the syllable into UTF8 code
            byte[] pass = Encoding.UTF8.GetBytes(strChange);
            SHA256 sha256 = new SHA256CryptoServiceProvider();
            byte[] hashValue = sha256.ComputeHash(pass);

            return BitConverter.ToString(hashValue).Replace("-", "").ToLower();
        }
        #endregion
    }

    public class LocationControl : Shared.CoreBase
    {
        LogControl log = new LogControl();
        SystemLog SystemLog = new SystemLog();
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public enum SaveType
        {
            Insert = 0,
            Update = 1
        }

        public LocationControl()
        {

        }

        /// <summary>
        /// Retrieve list of location information
        /// </summary>
        /// <returns></returns>
        public List<LocationContainer> GetAllLocation()
        {
            LocationContainer objLocationModel;
            List<LocationContainer> objListLocationModel = new List<LocationContainer>();
            DataTable dt;
            DateTime dateValue;
            String strSQL = string.Empty;

            try
            {
                strSQL = "SELECT * FROM Location ";
                dt = objDCom.Execute(strSQL, CommandType.Text, true);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow drRow in dt.Rows)
                    {
                        objLocationModel = new LocationContainer();
                        objLocationModel.LocationCode = (string)drRow["LocationCode"];
                        objLocationModel.Name = (string)drRow["Name"];
                        objLocationModel.LocationType = (string)drRow["LocationType"];
                        objLocationModel.PaymentGroupID = (long)drRow["PaymentGroupID"];
                        objLocationModel.CultureCode = (string)drRow["CultureCode"];
                        objLocationModel.CurrencyCode = (string)drRow["CurrencyCode"];
                        objLocationModel.InActive = (bool)drRow["InActive"];
                        objLocationModel.CreatedAgentID = (long)drRow["CreatedAgentID"];
                        if (DateTime.TryParse(drRow["CreatedDate"].ToString(), out dateValue)) objLocationModel.CreatedDate = (DateTime)drRow["CreatedDate"];
                        objLocationModel.ModifiedAgentID = (long)drRow["ModifiedAgentID"];
                        if (DateTime.TryParse(drRow["ModifiedDate"].ToString(), out dateValue)) objLocationModel.ModifiedDate = (DateTime)drRow["ModifiedDate"];
                        objListLocationModel.Add(objLocationModel);
                    }
                    return objListLocationModel;
                }
                else
                {
                    return null;
                    throw new ApplicationException("Location does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex);
                return null;
            }
        }

        /// <summary>
        /// Load list of location information into data table
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllLocationDT()
        {
            DataTable dt;
            String strSQL = string.Empty;

            try
            {
                strSQL = "SELECT * FROM Location ";
                dt = objDCom.Execute(strSQL, CommandType.Text, true);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt;
                }
                else
                {
                    return null;
                    throw new ApplicationException("Location does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex);
                return null;
            }
        }

        /// <summary>
        /// Retrieve single location details
        /// </summary>
        /// <param name="LocationCode"></param>
        /// <returns></returns>
        public LocationContainer GetSingleLocation(string LocationCode)
        {
            objSQL.ClearFields();
            objSQL.ClearCondtions();
            LocationContainer objLocationModel;
            DataTable dt;
            DateTime dateValue;
            String strSQL = string.Empty;
            String strFields = string.Empty;
            String strFilter = string.Empty;
            List<string> lstFields = new List<string>();
            try
            {
                lstFields.Add("Location.LocationCode");
                lstFields.Add("Location.Name");
                lstFields.Add("Location.LocationType");
                lstFields.Add("Location.PaymentGroupID");
                lstFields.Add("Location.CultureCode");
                lstFields.Add("Location.CurrencyCode");
                lstFields.Add("Location.InActive");
                lstFields.Add("Location.CreatedAgentID");
                lstFields.Add("Location.CreatedDate");
                lstFields.Add("Location.ModifiedAgentID");
                lstFields.Add("Location.ModifiedDate");

                strFields = GetSqlFields(lstFields);
                strFilter = "WHERE Location.LocationCode='" + LocationCode + "'";
                strSQL = "SELECT " + strFields + " FROM Location " + strFilter;
                dt = objDCom.Execute(strSQL, CommandType.Text, true);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow drRow = dt.Rows[0];

                    objLocationModel = new LocationContainer();
                    objLocationModel.LocationCode = (string)drRow["LocationCode"];
                    objLocationModel.Name = (string)drRow["Name"];
                    objLocationModel.LocationType = (string)drRow["LocationType"];
                    objLocationModel.PaymentGroupID = (long)drRow["PaymentGroupID"];
                    objLocationModel.CultureCode = (string)drRow["CultureCode"];
                    objLocationModel.CurrencyCode = (string)drRow["CurrencyCode"];
                    objLocationModel.InActive = (bool)drRow["InActive"];
                    objLocationModel.CreatedAgentID = (long)drRow["CreatedAgentID"];
                    if (DateTime.TryParse(drRow["CreatedDate"].ToString(), out dateValue)) objLocationModel.CreatedDate = (DateTime)drRow["CreatedDate"];
                    objLocationModel.ModifiedAgentID = (long)drRow["ModifiedAgentID"];
                    if (DateTime.TryParse(drRow["ModifiedDate"].ToString(), out dateValue)) objLocationModel.ModifiedDate = (DateTime)drRow["ModifiedDate"];
                    return objLocationModel;
                }
                else
                {
                    return null;
                    throw new ApplicationException("Location does not exist.");
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                return null;
            }
        }

        /// <summary>
        /// Saving single location details from container
        /// </summary>
        /// <param name="LocationModel"></param>
        /// <param name="pType"></param>
        /// <returns></returns>
        public LocationContainer SaveLocation(LocationContainer LocationModel, SaveType pType)
        {
            objSQL.ClearFields();
            objSQL.ClearCondtions();
            bool rValue = false;
            ArrayList lstSQL = new ArrayList();
            string strSQL = string.Empty;
            try
            {
                objSQL.AddField("LocationCode", LocationModel.LocationCode, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Name", LocationModel.Name, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LocationType", LocationModel.LocationType, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("CultureCode", LocationModel.CultureCode, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("CurrencyCode", LocationModel.CurrencyCode, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("CreatedDate", LocationModel.CreatedDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("ModifiedDate", LocationModel.ModifiedDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                switch (pType)
                {
                    case SaveType.Insert:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert, "Location", string.Empty);
                        break;
                    case SaveType.Update:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "Location", "Location.LocationCode='" + LocationModel.LocationCode + "'");
                        break;
                }
                lstSQL.Add(strSQL);
                rValue = objDCom.BatchExecute(lstSQL, CommandType.Text, true, false);
                if (rValue == false)
                {
                    return null;
                }
                return GetSingleLocation(LocationModel.LocationCode);
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                return null;
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

public class XMLParam
{
    LogControl log = new LogControl();

    public enum FileName
    {
        CITYPAIR,
        ALLCITYPAIR,
        DEPARTCITYPAIR,
        FLIGHTSEARCH_TWOWAY,

        BOOKINGFROMSTATE
    }
}

[Serializable]
public class BaseResource
{
    [XmlElement("Insurances")]
    public Insurances[] Insurances { get; set; }
}

public class GetRouteListResponse
{
    public RouteListExtend[] RouteListExtend { get; set; }
}

[Serializable]
public class Insurances
{
    [XmlElement("Insurance")]
    public Insurance[] Insurance { get; set; }
}
public class Insurance
{
    [XmlElement("CultureCode")]
    public string CultureCode { get; set; }
    [XmlElement("Itnl")]
    public Itnl[] Itnl { get; set; }
    [XmlElement("Dom")]
    public Dom[] Dom { get; set; }
}
public class Itnl
{
    [XmlElement("CheckItems")]
    public CheckItems[] CheckItems { get; set; }
    [XmlElement("UpsellUrl")]
    public string UpsellUrl { get; set; }
    [XmlElement("ConfirmContent")]
    public string ConfirmContent { get; set; }
}
public class Dom
{
    [XmlElement("CheckItems")]
    public CheckItems[] CheckItems { get; set; }
    [XmlElement("UpsellUrl")]
    public string UpsellUrl { get; set; }
    [XmlElement("ConfirmContent")]
    public string ConfirmContent { get; set; }
}
public class CheckItems
{
    public CheckItems()
    {
        CheckItem = new string[4];    
    }
    [XmlElement]
    public string[] CheckItem { get; set; }
}
public class RouteListExtend
{
    [XmlElement("DepartureStation")]
    public string DepartureStation { get; set; }

    [XmlElement("DepartureStationCurrencyCode")]
    public string DepartureStationCurrencyCode { get; set; }

    [XmlElement("ArrivalStation")]
    public string ArrivalStation { get; set; }

    [XmlElement("ArrivalStationCurrencyCode")]
    public string ArrivalStationCurrencyCode { get; set; }

    [XmlElement("PointToPointFlag")]
    public string PointToPointFlag { get; set; }

    [XmlElement("ConnectionFlag")]
    public string ConnectionFlag { get; set; }

    [XmlElement("InternationalFlag")]
    public string InternationalFlag { get; set; }

    [XmlElement("DepartureStationName")]
    public string DepartureStationName { get; set; }

    [XmlElement("DepartureStationAirportName")]
    public string DepartureStationAirportName { get; set; }

    [XmlElement("DepartureTimeZoneCode")]
    public string DepartureTimeZoneCode { get; set; }

    [XmlElement("ArrivalStationName")]
    public string ArrivalStationName { get; set; }

    [XmlElement("ArrivalStationAirportName")]
    public string ArrivalStationAirportName { get; set; }

    [XmlElement("ArrivalTimeZoneCode")]
    public string ArrivalTimeZoneCode { get; set; }

    [XmlElement("CultureCode")]
    public string CultureCode { get; set; }

    [XmlElement("CountryCode")]
    public string CountryCode { get; set; }

    [XmlElement("CarrierCode")]
    public string CarrierCode { get; set; }

    [XmlElement("DepartureCountry")]
    public string DepartureCountry { get; set; }

    [XmlElement("ArrivalCountry")]
    public string ArrivalCountry { get; set; }

    [XmlElement("CustomState")]
    public string CustomState { get; set; }

    [XmlElement("RCustomState")]
    public string RCustomState { get; set; }
}

// ------------------------------------------ General Control Backup ------------------------------------------------------
//using System;
//using System.Data;
//using System.Collections;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using SEAL.Data;
//using System.Web;
////using log4net;
//using System.Security.Cryptography;

//using System.Data.SqlClient;

//namespace ABS.Logic.GroupBooking
//{
//    public partial class Country_Info
//    {
//        private int _id;
//        private string _countrycode = String.Empty;
//        private string _countryName = String.Empty;
//        private string _provincestatecode = String.Empty;
//        private string _provinceStateName = String.Empty;
//        private string _customState = string.Empty;
//        private string _cityCode = string.Empty;

//        #region Public Properties
//        public int Id
//        {
//            get { return _id; }
//            set { _id = value; }
//        }
//        public string countrycode
//        {
//            get { return _countrycode; }
//            set { _countrycode = value; }
//        }

//        public string countryName
//        {
//            get { return _countryName; }
//            set { _countryName = value; }
//        }

//        public string provincestatecode
//        {
//            get { return _provincestatecode; }
//            set { _provincestatecode = value; }
//        }

//        public string provinceStateName
//        {
//            get { return _provinceStateName; }
//            set { _provinceStateName = value; }
//        }

//        public string CustomState
//        {
//            get { return _customState; }
//            set { _customState = value; }
//        }

//        public string CityCode
//        {
//            get { return _cityCode; }
//            set { _cityCode = value; }
//        }
//        #endregion



//    }
//    public class CODEMASTER
//    {
//        private string _codeType = String.Empty;
//        private string _code = String.Empty;

//        private string _codeDesc = String.Empty;
//        private int _codeSeq;
//        private byte _sysCode;
//        private Guid _rowguid = Guid.Empty;
//        private DateTime _syncCreate;
//        private DateTime _syncLastUpd;
//        private byte _isHost;

//        #region Public Properties
//        public string CodeType
//        {
//            get { return _codeType; }
//            set { _codeType = value; }
//        }
//        public string Code
//        {
//            get { return _code; }
//            set { _code = value; }
//        }
//        public string CodeDesc
//        {
//            get { return _codeDesc; }
//            set { _codeDesc = value; }
//        }

//        public int CodeSeq
//        {
//            get { return _codeSeq; }
//            set { _codeSeq = value; }
//        }

//        public byte SysCode
//        {
//            get { return _sysCode; }
//            set { _sysCode = value; }
//        }

//        public Guid rowguid
//        {
//            get { return _rowguid; }
//            set { _rowguid = value; }
//        }

//        public DateTime SyncCreate
//        {
//            get { return _syncCreate; }
//            set { _syncCreate = value; }
//        }

//        public DateTime SyncLastUpd
//        {
//            get { return _syncLastUpd; }
//            set { _syncLastUpd = value; }
//        }

//        public byte IsHost
//        {
//            get { return _isHost; }
//            set { _isHost = value; }
//        }
//        #endregion

//    }

//    #region SYS_AUDITLOG
//    /// <summary>
//    /// This object represents the properties and methods of a SYS_AUDITLOG.
//    /// </summary>
//    public class AUDITLOG
//    {
//        private Guid _rowguid = Guid.Empty;

//        private string _transID = String.Empty;
//        private int _seqNo;
//        private byte _action;
//        private string _refCode = String.Empty;
//        private string _table_Name = String.Empty;
//        private string _sQL = String.Empty;
//        private DateTime _createdDate;
//        private string _createdBy = String.Empty;
//        private byte _priority;
//        private byte _flag;

//        #region Public Properties
//        public Guid rowguid
//        {
//            get { return _rowguid; }
//            set { _rowguid = value; }
//        }
//        public string TransID
//        {
//            get { return _transID; }
//            set { _transID = value; }
//        }
//        public string CreatedBy
//        {
//            get { return _createdBy; }
//            set { _createdBy = value; }
//        }
//        public int SeqNo
//        {
//            get { return _seqNo; }
//            set { _seqNo = value; }
//        }

//        public byte Action
//        {
//            get { return _action; }
//            set { _action = value; }
//        }

//        public string RefCode
//        {
//            get { return _refCode; }
//            set { _refCode = value; }
//        }

//        public string Table_Name
//        {
//            get { return _table_Name; }
//            set { _table_Name = value; }
//        }

//        public string SQL
//        {
//            get { return _sQL; }
//            set { _sQL = value; }
//        }

//        public DateTime CreatedDate
//        {
//            get { return _createdDate; }
//            set { _createdDate = value; }
//        }

//        public byte Priority
//        {
//            get { return _priority; }
//            set { _priority = value; }
//        }

//        public byte Flag
//        {
//            get { return _flag; }
//            set { _flag = value; }
//        }
//        #endregion

//    }




//    #endregion

//    #region Settings
//    public class Settings
//    {
//        private short _appID;
//        private string _GRPID = String.Empty;
//        private string _sYSKey = String.Empty;
//        private string _sYSDesc = String.Empty;

//        private string _sYSValue = String.Empty;
//        private string _sYSValueEx = String.Empty;
//        private byte _sYSSet;
//        private Guid _rowguid = Guid.Empty;
//        private DateTime _syncCreate;
//        private DateTime _syncLastUpd;
//        private byte _isHost;
//        private string _lastSyncBy = String.Empty;

//        #region Public Properties
//        public short AppID
//        {
//            get { return _appID; }
//            set { _appID = value; }
//        }
//        public string GRPID
//        {
//            get { return _GRPID; }
//            set { _GRPID = value; }
//        }
//        public string SYSKey
//        {
//            get { return _sYSKey; }
//            set { _sYSKey = value; }
//        }
//        public string SYSDesc
//        {
//            get { return _sYSDesc; }
//            set { _sYSDesc = value; }
//        }
//        public string SYSValue
//        {
//            get { return _sYSValue; }
//            set { _sYSValue = value; }
//        }

//        public string SYSValueEx
//        {
//            get { return _sYSValueEx; }
//            set { _sYSValueEx = value; }
//        }

//        public byte SYSSet
//        {
//            get { return _sYSSet; }
//            set { _sYSSet = value; }
//        }

//        public Guid rowguid
//        {
//            get { return _rowguid; }
//            set { _rowguid = value; }
//        }

//        public DateTime SyncCreate
//        {
//            get { return _syncCreate; }
//            set { _syncCreate = value; }
//        }

//        public DateTime SyncLastUpd
//        {
//            get { return _syncLastUpd; }
//            set { _syncLastUpd = value; }
//        }

//        public byte IsHost
//        {
//            get { return _isHost; }
//            set { _isHost = value; }
//        }

//        public string LastSyncBy
//        {
//            get { return _lastSyncBy; }
//            set { _lastSyncBy = value; }
//        }
//        #endregion

//    }
//    #endregion



//    public partial class GeneralControl : Shared.CoreBase
//    {
//        LogControl log = new LogControl();
//        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
//        AALookUpWS.LookupWSSoapClient aws = new AALookUpWS.LookupWSSoapClient();
//        public enum SaveType
//        {
//            Insert = 0,
//            Update = 1
//        }

//        public List<Country_Info> GetAllCountryCode()
//        {
//            Country_Info objCountry_Info;
//            List<Country_Info> objListCountry_Info = new List<Country_Info>();
//            DataTable dt;
//            String strSQL = string.Empty;

//            try
//            {
//                strSQL = "SELECT * FROM CountryCode ";
//                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
//                if (dt != null && dt.Rows.Count > 0)
//                {
//                    foreach (DataRow drRow in dt.Rows)
//                    {
//                        objCountry_Info = new Country_Info();
//                        objCountry_Info.Id = (int)drRow["Id"];
//                        objCountry_Info.countrycode = (string)drRow["countrycode"];
//                        objCountry_Info.countryName = (string)drRow["countryName"];
//                        objCountry_Info.provincestatecode = (string)drRow["provincestatecode"];
//                        objCountry_Info.provinceStateName = (string)drRow["provinceStateName"];
//                        objListCountry_Info.Add(objCountry_Info);
//                    }
//                    return objListCountry_Info;
//                }
//                else
//                {
//                    log.Info(this,"CountryCode does not exist."); //added, for log purpose
//                    return null;
//                    //throw new ApplicationException("CountryCode does not exist.");
//                }
//            }
//            catch (Exception ex)
//            {
//                log.Error(this,ex); //added, for log purpose
//                return null;
//            }
//        }

//        int cc = 0;
//        public Country_Info GetCountryInfoByCode(string CountryCode, string CurrencyCode)
//        {
//            Country_Info objCountry_Info = new Country_Info();
//            List<Country_Info> objListCountry_Info = new List<Country_Info>();
//            DataTable dt = new DataTable();
//            String strSQL = string.Empty;

//            try
//            {

//                using (var connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ToString()))
//                {
//                    connection.Open();
//                    strSQL = "SELECT * FROM COUNTRY WHERE CountryCode='" + CountryCode.Substring(0, 2).ToString() + "' ";
//                    SqlCommand cmd = new SqlCommand(strSQL, connection);
//                    SqlDataAdapter da = new SqlDataAdapter(cmd);
//                    da.Fill(dt);
//                    // Stuff with the connection
//                    connection.Close();

//                    if (dt != null && dt.Rows.Count > 0)
//                    {
//                        foreach (DataRow drRow in dt.Rows)
//                        {
//                            objCountry_Info = new Country_Info();
//                            objCountry_Info.countrycode = (string)drRow["CountryCode"];
//                            objCountry_Info.countryName = (string)drRow["Name"];
//                        }
//                        return objCountry_Info;
//                    }
//                    else
//                    {
//                        strSQL = "SELECT * FROM COUNTRY WHERE DefaultCurrencyCode='" + CurrencyCode + "' ";
//                        dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
//                        if (dt != null && dt.Rows.Count > 0)
//                        {
//                            foreach (DataRow drRow in dt.Rows)
//                            {
//                                objCountry_Info = new Country_Info();
//                                objCountry_Info.countrycode = (string)drRow["CountryCode"];
//                                objCountry_Info.countryName = (string)drRow["Name"];
//                            }
//                            return objCountry_Info;
//                        }
//                        else
//                        {
//                            log.Info(this, "CountryCode does not exist."); //added, for log purpose
//                            return null;
//                            //throw new ApplicationException("CountryCode does not exist.");
//                        }
//                    }
//                }
//                return null;

//            }
//            catch (Exception ex)
//            {
//                log.Error(this, ex, strSQL); //added, for log purpose
//                return null;
//            }
//            finally
//            {
//                dt = null;
//                //objDCom.CloseConnection();
//            }

//        }

//        //public Country_Info GetCountryInfoByCode(string CountryCode, string CurrencyCode)
//        //{
//        //    Country_Info objCountry_Info = new Country_Info();
//        //    List<Country_Info> objListCountry_Info = new List<Country_Info>();
//        //    DataTable dt = new DataTable();
//        //    String strSQL = string.Empty;

//        //    //log.Info(this, CountryCode + "a" + CurrencyCode);

//        //    //using(objDCom = new DataAccess())
//        //    {

//        //        try
//        //        {
//        //            //objDCom.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ToString();
//        //            StartConnection(EnumIsoState.StateUpdatetable, true);
//        //            //if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
//        //            {
//        //                StartSQLControl();

//        //                strSQL = "SELECT * FROM COUNTRY WHERE CountryCode='" + CountryCode.Substring(0, 2).ToString() + "' ";

//        //                //log.Info(this, String.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + " | " + cc + " | " + strSQL);
//        //                //cc++;
//        //                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
//        //                if (dt != null && dt.Rows.Count > 0)
//        //                {
//        //                    foreach (DataRow drRow in dt.Rows)
//        //                    {
//        //                        objCountry_Info = new Country_Info();
//        //                        objCountry_Info.countrycode = (string)drRow["CountryCode"];
//        //                        objCountry_Info.countryName = (string)drRow["Name"];
//        //                    }
//        //                    return objCountry_Info;
//        //                }
//        //                else
//        //                {
//        //                    strSQL = "SELECT * FROM COUNTRY WHERE DefaultCurrencyCode='" + CurrencyCode + "' ";
//        //                    dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
//        //                    if (dt != null && dt.Rows.Count > 0)
//        //                    {
//        //                        foreach (DataRow drRow in dt.Rows)
//        //                        {
//        //                            objCountry_Info = new Country_Info();
//        //                            objCountry_Info.countrycode = (string)drRow["CountryCode"];
//        //                            objCountry_Info.countryName = (string)drRow["Name"];
//        //                        }
//        //                        return objCountry_Info;
//        //                    }
//        //                    else
//        //                    {
//        //                        log.Info(this, "CountryCode does not exist."); //added, for log purpose
//        //                        return null;
//        //                        //throw new ApplicationException("CountryCode does not exist.");
//        //                    }
//        //                }
//        //            }
//        //            return null;

//        //        }
//        //        catch (Exception ex)
//        //        {
//        //            log.Error(this, ex, strSQL); //added, for log purpose
//        //            return null;
//        //        }
//        //        finally
//        //        {
//        //            dt = null;
//        //            //objDCom.CloseConnection();
//        //            EndSQLControl();
//        //            EndConnection();
//        //        }
//        //    }

//        //}

//        //GetCountryInfoByCode Backup
//        public Country_Info GetCountryInfoByCodeOld(string CountryCode, string CurrencyCode)
//        {
//            Country_Info objCountry_Info = new Country_Info();
//            List<Country_Info> objListCountry_Info = new List<Country_Info>();
//            DataTable dt = new DataTable();
//            String strSQL = string.Empty;

//            try
//            {
//                strSQL = "SELECT * FROM COUNTRY WHERE CountryCode='" + CountryCode.Substring(0, 2).ToString() + "' ";

//                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
//                if (dt != null && dt.Rows.Count > 0)
//                {
//                    foreach (DataRow drRow in dt.Rows)
//                    {
//                        objCountry_Info = new Country_Info();
//                        objCountry_Info.countrycode = (string)drRow["CountryCode"];
//                        objCountry_Info.countryName = (string)drRow["Name"];
//                    }
//                    return objCountry_Info;
//                }
//                else
//                {
//                    strSQL = "SELECT * FROM COUNTRY WHERE DefaultCurrencyCode='" + CurrencyCode + "' ";
//                    dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
//                    if (dt != null && dt.Rows.Count > 0)
//                    {
//                        foreach (DataRow drRow in dt.Rows)
//                        {
//                            objCountry_Info = new Country_Info();
//                            objCountry_Info.countrycode = (string)drRow["CountryCode"];
//                            objCountry_Info.countryName = (string)drRow["Name"];
//                        }
//                        return objCountry_Info;
//                    }
//                    else
//                    {
//                        log.Info(this, "CountryCode does not exist."); //added, for log purpose
//                        return null;
//                        //throw new ApplicationException("CountryCode does not exist.");
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                log.Error(this, ex, strSQL); //added, for log purpose
//                return null;
//            }
//            finally
//            {
//                dt = null;
//                //objDCom.CloseConnection();
//            }
//        }

//        public DataTable GetCountryNameByCode(string CountryCode)
//        {

//            DataTable dt;
//            String strSQL = string.Empty;

//            try
//            {
//                strSQL = "SELECT DISTINCT Name FROM COUNTRY WHERE CountryCode3C='" + CountryCode + "'";
//                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
//                if (dt != null && dt.Rows.Count > 0)
//                {
//                    return dt;
//                }
//                else
//                {
//                    log.Info(this,"Country does not exist."); //added, for log purpose
//                    return null;
//                }
//            }
//            catch (Exception ex)
//            {
//                log.Error(this,ex); //added, for log purpose
//                return null;
//            }
//        }

//        public List<Country_Info> GetAllCountry()
//        {
//            Country_Info objCountry_Info;
//            List<Country_Info> objListCountry_Info = new List<Country_Info>();
//            DataTable dt;
//            String strSQL = string.Empty;

//            try
//            {
//                strSQL = "SELECT distinct  countrycode, countryName FROM CountryCode order by countryName";
//                log.Info(this,strSQL);
//                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
//                if (dt != null && dt.Rows.Count > 0)
//                {
//                    foreach (DataRow drRow in dt.Rows)
//                    {
//                        objCountry_Info = new Country_Info();
//                        objCountry_Info.countrycode = (string)drRow["countrycode"];
//                        objCountry_Info.countryName = (string)drRow["countryName"];
//                        objListCountry_Info.Add(objCountry_Info);
//                    }
//                    return objListCountry_Info;
//                }
//                else
//                {
//                    log.Info(this,"GetAllCountry() - CountryCode does not exist."); //added, for log purpose
//                    return null;
//                }
//            }
//            catch (Exception ex)
//            {
//                log.Error(this,ex); //added, for log purpose
//                return null;
//            }
//        }

//        public DataTable GetAllCountryCard()
//        {

//            DataTable dt;
//            String strSQL = string.Empty;

//            try
//            {
//                strSQL = "SELECT DISTINCT CountryCode, Name, CountryCode3C FROM COUNTRY order by Name";
//                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
//                if (dt != null && dt.Rows.Count > 0)
//                {
//                    return dt;
//                }
//                else
//                {
//                    log.Info(this,"Country does not exist."); //added, for log purpose
//                    return null;
//                    throw new ApplicationException("Country does not exist.");
//                }
//            }
//            catch (Exception ex)
//            {
//                log.Error(this,ex);
//                return null;
//            }
//        }


//        public List<Country_Info> GetAllState(string code)
//        {
//            Country_Info objCountry_Info;
//            List<Country_Info> objListCountry_Info = new List<Country_Info>();
//            DataTable dt;
//            String strSQL = string.Empty;

//            try
//            {
//                strSQL = "SELECT distinct provincestatecode, provinceStateName FROM CountryCode where countrycode='" + code + "' ORDER BY provinceStateName";
//                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
//                if (dt != null && dt.Rows.Count > 0)
//                {
//                    foreach (DataRow drRow in dt.Rows)
//                    {
//                        objCountry_Info = new Country_Info();
//                        objCountry_Info.provincestatecode = (string)drRow["provincestatecode"];
//                        objCountry_Info.provinceStateName = (string)drRow["provinceStateName"];
//                        objListCountry_Info.Add(objCountry_Info);
//                    }
//                    return objListCountry_Info;
//                }
//                else
//                {
//                    log.Info(this,"State does not exist."); //added, for log purpose
//                    return null;
//                    //throw new ApplicationException("State does not exist.");
//                }
//            }
//            catch (Exception ex)
//            {
//                log.Error(this,ex);
//                return null;
//            }
//        }

//        public string GetCurrencyByDeparture(string departure)
//        {
//            try
//            {
//                //if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
//                //{
//                    DataTable dt;
//                    strSQL = "SELECT Country.DefaultCurrencyCode FROM City INNER JOIN";
//                    strSQL += " COUNTRY ON City.CountryCode = COUNTRY.CountryCode";
//                    strSQL += " WHERE City.CityCode = '" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, departure) + "'";
//                    dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
//                    if (dt != null && dt.Rows.Count > 0)
//                    {
//                        return dt.Rows[0]["DefaultCurrencyCode"].ToString();
//                    }
//                    else
//                    {
//                        return "";
//                    }
//                //}
//                //return "";
//            }
//            catch (Exception ex)
//            {
//                log.Error(this,ex); //added, for log purpose
//                return "";
//            }
//            //finally
//            //{
//            //    EndSQLControl();
//            //    EndConnection();
//            //}
//        }

//        //added by ketee
//        public DataTable ReturnAllCityCustom(string DepartCity)
//        {
//            //Country_Info objCountry_Info;
//            List<Country_Info> objListCountry_Info = new List<Country_Info>();
//            DataTable dt;
//            string strCond;
//            string strStation;
//            try
//            {
//                if (DepartCity != string.Empty)
//                {
//                    strStation = "CityPair.ArrivalStation";
//                }
//                else
//                {
//                    strStation = "CityPair.DepartureStation";
//                }

//                strSQL = "SELECT DISTINCT CI.CityCode, CI.Name + ' (' + CI.CityCode + ')' + '|' + CT.Name AS CustomState, CT.Name, CI.Name AS CityName FROM CityPair INNER JOIN " +
//                         "City AS CI WITH (nolock) ON " + strStation + " = CI.CityCode INNER JOIN Country AS CT WITH (nolock) ON " +
//                         "CT.CountryCode = CI.CountryCode ";
//                strCond = " WHERE  (CI.InActive = 0) AND (CT.InActive = 0) ";
//                if (DepartCity != string.Empty)
//                {
//                    strCond = string.Concat(strCond, " AND DepartureStation = '", objSQL.ParseValue(SQLControl.EnumDataType.dtString, DepartCity), "'");
//                }
//                strSQL = string.Concat(strSQL, strCond, " ORDER BY CT.Name, CityName");
//                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
//                if (dt != null && dt.Rows.Count > 0)
//                {
//                    return dt;
//                }
//                else
//                {
//                    log.Info(this,"State does not exist."); //added, for log purpose
//                    return null;
//                    //throw new ApplicationException("State does not exist.");
//                }
//            }
//            catch (Exception ex)
//            {
//                log.Error(this,ex); //added, for log purpose
//                return null;
//            }
//        }

//        public DataTable GetLookUpCity(string CityDepart)
//        {
//            try
//            {
//                DataTable dt = new DataTable();
//                DataTable dtDepart = new DataTable();
//                DataTable dtresp = new DataTable();
//                dt = CountryInfoStructure();
//                dtDepart = CountryInfoStructure();
//                dtresp = CountryInfoStructure();

//                if (HttpContext.Current.Session["CityPairAll"] != null)
//                    dt = (DataTable)HttpContext.Current.Session["CityPairAll"];

//                if (HttpContext.Current.Session["CityPairDepart"] != null)
//                    dtDepart = (DataTable)HttpContext.Current.Session["CityPairDepart"];

//                if (dt == null || dt.Rows.Count == 0 || dtDepart == null || dtDepart.Rows.Count == 0)
//                {
//                    SetCityPair();
//                }

//                if (HttpContext.Current.Session["CityPairAll"] != null)
//                    dt = (DataTable)HttpContext.Current.Session["CityPairAll"];

//                if (HttpContext.Current.Session["CityPairDepart"] != null)
//                    dtDepart = (DataTable)HttpContext.Current.Session["CityPairDepart"];

//                if (dt != null || dt.Rows.Count != 0)
//                {
//                    if (CityDepart != "")
//                    {
//                        dtresp = dt.Clone();
//                        DataRow[] drs = dt.Select("CityCode = '" + CityDepart + "'", "RName");
//                        if (drs != null)
//                        {
//                            foreach (DataRow dr in drs)
//                            {
//                                dtresp.ImportRow(dr);
//                            }
//                        }
//                    }
//                    else
//                    {
//                        //DataRow foundRow = null;
//                        //foreach (DataRow dr in dt.Rows)
//                        //{
//                        //    foundRow = dtresp.Rows.Find(dr["CityCode"]);
//                        //    if (foundRow == null)
//                        //    {
//                        //        dtresp.NewRow();
//                        //        dtresp.Rows.Add(dr);
//                        //    }
//                        //}
//                        DataView dv = dtDepart.DefaultView;
//                        dv.Sort = "Name";
//                        return dv.ToTable();
//                    }

//                    if (dtresp != null)
//                    {
//                        return dtresp;
//                    }
//                }

//                return null;
//            }
//            catch (Exception ex)
//            {
//                log.Error(this,ex);
//                return null;
//            }
//        }

//        public void SetCityPair()
//        {
//            try
//            {
//                ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking("");

//                AALookUpWS.LogonRequest LogonReq = new AALookUpWS.LogonRequest();
//                LogonReq.Username = apiBooking.Username;
//                LogonReq.Password = apiBooking.Password;

//                DataTable dtAll = new DataTable();
//                DataTable dtDepart = new DataTable();

//                dtAll = CountryInfoStructure();
//                dtDepart = CountryInfoStructure();

//                if (HttpContext.Current.Session["CityPairAll"] != null)
//                {
//                    dtAll = (DataTable)HttpContext.Current.Session["CityPairAll"];
//                    //return dtAll;
//                }

//                if (HttpContext.Current.Session["CityPairDepart"] != null)
//                {
//                    dtDepart = (DataTable)HttpContext.Current.Session["CityPairDepart"];
//                    //return dtAll;
//                }

//                if (dtAll == null || dtAll.Rows.Count == 0 || dtDepart == null || dtDepart.Rows.Count == 0)
//                {
//                    AALookUpWS.LogonResponse resp = aws.Login(LogonReq);
//                    AALookUpWS.RouteInfoResponse CityPair = new AALookUpWS.RouteInfoResponse();
//                    if (resp != null)
//                    {
//                        CityPair = aws.GetRouteList("", "", resp.SessionID);
//                        string xml = GetXMLString(CityPair);
//                    }
//                    if (CityPair != null)
//                    {
//                        foreach (AALookUpWS.RouteInfo rInfo in CityPair.RouteInfo)
//                        {
//                            DataRow dr = null;
//                            DataRow drDepart = null;
//                            DataRow[] foundRow = null;
//                            DataRow[] foundRowDepart = null;
//                            Country_Info CT = new Country_Info();
//                            Country_Info CTR = new Country_Info();

//                            //dtAll.Rows.Find(rInfo.DepartureStation,rInfo.ArrivalStation);
//                            foundRow = dtAll.Select("CityCode = '" + rInfo.DepartureStation + "' AND RCityCode = '" + rInfo.ArrivalStation + "'");
//                            foundRowDepart = dtAll.Select("CityCode = '" + rInfo.DepartureStation + "'");
//                            CTR = GetCountryInfoByCode(rInfo.ArrivalTimeZoneCode, rInfo.ArrivalStationCurrencyCode);
//                            CT = GetCountryInfoByCode(rInfo.DepartureTimeZoneCode, rInfo.DepartureStationCurrencyCode);

//                            if (CT != null && CTR != null)
//                            {
//                                dr = dtAll.NewRow();
//                                dr["CityCode"] = rInfo.DepartureStation;
//                                dr["CustomState"] = rInfo.DepartureStationName + "(" + rInfo.DepartureStation + ")" + '|' + CT.countryName;
//                                dr["Name"] = CT.countryName;
//                                dr["CityName"] = rInfo.DepartureStationName;
//                                dr["RCityCode"] = rInfo.ArrivalStation;
//                                dr["RCustomState"] = rInfo.ArrivalStationName + "(" + rInfo.ArrivalStation + ")" + '|' + CTR.countryName;
//                                dr["RName"] = CTR.countryName;
//                                dr["RCityName"] = rInfo.ArrivalStationName;

//                                drDepart = dtDepart.NewRow();
//                                drDepart["CityCode"] = rInfo.DepartureStation;
//                                drDepart["CustomState"] = rInfo.DepartureStationName + "(" + rInfo.DepartureStation + ")" + '|' + CT.countryName;
//                                drDepart["Name"] = CT.countryName;
//                                drDepart["CityName"] = rInfo.DepartureStationName;
//                                drDepart["RCityCode"] = rInfo.ArrivalStation;
//                                drDepart["RCustomState"] = rInfo.ArrivalStationName + "(" + rInfo.ArrivalStation + ")" + '|' + CTR.countryName;
//                                drDepart["RName"] = CTR.countryName;
//                                drDepart["RCityName"] = rInfo.ArrivalStationName;
//                                drDepart["Currency"] = rInfo.DepartureStationCurrencyCode;
//                            }

//                            if (dr != null && dr["CityCode"].ToString() != "" && foundRow.Length == 0)
//                            {
//                                dtAll.Rows.Add(dr);
//                            }

//                            if (drDepart != null && drDepart["CityCode"].ToString() != "" && foundRowDepart.Length == 0)
//                            {
//                                dtDepart.Rows.Add(drDepart);
//                            }

//                        }

//                        if (dtAll != null)
//                        {
//                            HttpContext.Current.Session.Add("CityPairAll", dtAll);
//                        }

//                        if (dtDepart != null)
//                        {
//                            HttpContext.Current.Session.Add("CityPairDepart", dtDepart);
//                        }
//                    }
//                    //return dtAll;
//                }
//                //return null;
//            }


//            catch (Exception ex)
//            {
//                log.Error(this,ex);
//                //return null;
//            }
//        }

//    public DataTable CountryInfoStructure()
//    {
//        DataTable dt = new DataTable();
//        DataColumn[] keys = new DataColumn[2];
//        DataColumn column;
//        column = new DataColumn();
//        column.DataType = System.Type.GetType("System.String");
//        column.ColumnName = "CityCode";

//        // Add the column to the DataTable.Columns collection.
//        dt.Columns.Add(column);
//        // Add the column to the array.
//        keys[0] = column;

//        column = new DataColumn();
//        column.DataType = System.Type.GetType("System.String");
//        column.ColumnName = "CustomState";
//        dt.Columns.Add(column);

//        column = new DataColumn();
//        column.DataType = System.Type.GetType("System.String");
//        column.ColumnName = "Name";
//        dt.Columns.Add(column);

//        column = new DataColumn();
//        column.DataType = System.Type.GetType("System.String");
//        column.ColumnName = "CityName";
//        dt.Columns.Add(column);

//        column = new DataColumn();
//        column.DataType = System.Type.GetType("System.String");
//        column.ColumnName = "RCityCode";
//        dt.Columns.Add(column);
//        keys[1] = column;

//        column = new DataColumn();
//        column.DataType = System.Type.GetType("System.String");
//        column.ColumnName = "RCustomState";
//        dt.Columns.Add(column);

//        column = new DataColumn();
//        column.DataType = System.Type.GetType("System.String");
//        column.ColumnName = "RName";
//        dt.Columns.Add(column);

//        column = new DataColumn();
//        column.DataType = System.Type.GetType("System.String");
//        column.ColumnName = "RCityName";
//        dt.Columns.Add(column);

//        column = new DataColumn();
//        column.DataType = System.Type.GetType("System.String");
//        column.ColumnName = "Currency";
//        dt.Columns.Add(column);

//        dt.PrimaryKey = keys;
//        return dt;
//    }

////        public DataTable GetLookUpCity(string CityDepart)
////        {
////            try
////            {
////                ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking("");

////                AALookUpWS.LogonRequest LogonReq = new AALookUpWS.LogonRequest();
////                LogonReq.Username = apiBooking.Username ;
////                LogonReq.Password = apiBooking.Password ;


////                DataTable dt = new DataTable();
////                DataTable dtAll = new DataTable();

////                //CI.CityCode, CI.Name + ' (' + CI.CityCode + ')' + '|' + CT.Name AS CustomState, CT.Name, CI.Name AS CityName
////                DataColumn[] keys = new DataColumn[1];
////                DataColumn column;
////                column = new DataColumn();
////                column.DataType = System.Type.GetType("System.String");
////                column.ColumnName = "CityCode";

////                // Add the column to the DataTable.Columns collection.
////                dt.Columns.Add(column);
////                dtAll.Columns.Add(column);
////                // Add the column to the array.
////                keys[0] = column;

////                column = new DataColumn();
////                column.DataType = System.Type.GetType("System.String");
////                column.ColumnName = "CustomState";
////                dt.Columns.Add(column);
////                dtAll.Columns.Add(column);

////                column = new DataColumn();
////                column.DataType = System.Type.GetType("System.String");
////                column.ColumnName = "Name";
////                dt.Columns.Add(column);
////                dtAll.Columns.Add(column);

////                column = new DataColumn();
////                column.DataType = System.Type.GetType("System.String");
////                column.ColumnName = "CityName";
////                dt.Columns.Add(column);
////                dtAll.Columns.Add(column);

////                column = new DataColumn();
////                column.DataType = System.Type.GetType("System.String");
////                column.ColumnName = "RCityCode";
////                dt.Columns.Add(column);
////                dtAll.Columns.Add(column);

////                column = new DataColumn();
////                column.DataType = System.Type.GetType("System.String");
////                column.ColumnName = "RCustomState";
////                dt.Columns.Add(column);
////                dtAll.Columns.Add(column);

////                column = new DataColumn();
////                column.DataType = System.Type.GetType("System.String");
////                column.ColumnName = "RName";
////                dt.Columns.Add(column);
////                dtAll.Columns.Add(column);

////                column = new DataColumn();
////                column.DataType = System.Type.GetType("System.String");
////                column.ColumnName = "RCityName";
////                dt.Columns.Add(column);
////                dtAll.Columns.Add(column);

////                dt.PrimaryKey = keys;
////                dtAll.PrimaryKey = keys;

////                if (HttpContext.Current.Session["CityPairDepart"] != null)
////                    dt = (DataTable)HttpContext.Current.Session["CityPairDepart"];

////                if (HttpContext.Current.Session["CityPairAll"] != null)
////                    dtAll = (DataTable)HttpContext.Current.Session["CityPairAll"];

////                if (dt == null || dt.Rows.Count == 0 || dtAll == null || dtAll.Rows.Count == 0)
////                {
////                    AALookUpWS.LogonResponse resp = aws.Login(LogonReq);
////                    AALookUpWS.RouteInfoResponse CityPair = new AALookUpWS.RouteInfoResponse();
////                    if (resp != null)
////                    {
////                        CityPair = aws.GetRouteList(CityDepart, "", resp.SessionID);
////                    }
////                    if (CityPair != null)
////                    {
////                        foreach (AALookUpWS.RouteInfo rInfo in CityPair.RouteInfo)
////                        {
////                            DataRow dr = dt.NewRow();
////                            DataRow foundRow = null;
////                            Country_Info CT = new Country_Info();
////                            Country_Info CTR = new Country_Info();

////CTR = GetCountryInfoByCode(rInfo.ArrivalTimeZoneCode);
////CT = GetCountryInfoByCode(rInfo.DepartureTimeZoneCode);
////                            if (CityDepart != "")
////                            {

////                                foundRow = dt.Rows.Find(rInfo.ArrivalStation);

////                                if (CTR != null && foundRow == null)
////                                {
////                                    dr["CityCode"] = rInfo.ArrivalStation;
////                                    dr["CustomState"] = rInfo.ArrivalStationName + "(" + rInfo.ArrivalStation + ")" + CTR.countryName;
////                                    dr["Name"] = CTR.countryName;
////                                    dr["CityName"] = rInfo.ArrivalStationName;
////                                }
////                            }
////                            else
////                            {

////                                foundRow = dt.Rows.Find(rInfo.DepartureStation);
////                                if (CT != null)
////                                {
////                                    dr["CityCode"] = rInfo.DepartureStation;
////                                    dr["CustomState"] = rInfo.DepartureStationName + "(" + rInfo.DepartureStation + ")" + CT.countryName;
////                                    dr["Name"] = CT.countryName;
////                                    dr["CityName"] = rInfo.DepartureStationName;
////                                    dr["RCityCode"] = rInfo.ArrivalStation;
////                                    dr["RCustomState"] = rInfo.ArrivalStationName + "(" + rInfo.ArrivalStation + ")" + CT.countryName;
////                                    dr["RName"] = CT.countryName;
////                                    dr["RCityName"] = rInfo.ArrivalStationName;


////                                }

////                                if (foundRow == null)
////                                {
////                                    if (dr != null && dr["CityCode"].ToString() != "")
////                                    {
////                                        dt.Rows.Add(dr);
////                                    }
////                                }
////                                else
////                                {
////                                    if (dr != null && dr["CityCode"].ToString() != "")
////                                    {
////                                        dtAll.Rows.Add(dr);
////                                    }
////                                }
////                            }
////                        }
////                        return dt;
////                    }
////                }
////                else
////                {

////                }
////                return null;
////            }
////            catch (Exception ex)
////            {
////                log.Error(this,ex);
////                return null;
////            }
////        }

//        public List<Country_Info> GetAllCityCustom(string CityDepart)
//        {
//            Country_Info objCountry_Info;
//            List<Country_Info> objListCountry_Info = new List<Country_Info>();
//            DataTable dt;
//            string strCond;
//            string strStation;
//            try
//            {
//                if (CityDepart != string.Empty)
//                {
//                    strStation = "CityPair.ArrivalStation";
//                }
//                else
//                {
//                    strStation = "CityPair.DepartureStation";
//                }

//                strSQL = "SELECT DISTINCT CI.CityCode, CI.Name + ' (' + CI.CityCode + ')' + '|' + CT.Name AS CustomState, CT.Name, CI.Name AS CityName FROM CityPair INNER JOIN " +
//                         "City AS CI WITH (nolock) ON " + strStation + " = CI.CityCode INNER JOIN Country AS CT WITH (nolock) ON " +
//                         "CT.CountryCode = CI.CountryCode ";
//                strCond = "WHERE  (CI.InActive = 0) AND (CT.InActive = 0) ";
//                if (CityDepart != string.Empty)
//                {
//                    strCond = string.Concat(strCond, " AND DepartureStation = '", objSQL.ParseValue(SQLControl.EnumDataType.dtString, CityDepart), "'");
//                }
//                strSQL = string.Concat(strSQL, strCond, " ORDER BY CT.Name, CityName");
//                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
//                if (dt != null && dt.Rows.Count > 0)
//                {
//                    foreach (DataRow drRow in dt.Rows)
//                    {
//                        objCountry_Info = new Country_Info();
//                        objCountry_Info.CityCode = (string)drRow["CityCode"];
//                        //objCountry_Info.provinceStateName = (string)drRow["provinceStateName"];
//                        objCountry_Info.CustomState = (string)drRow["CustomState"];
//                        objListCountry_Info.Add(objCountry_Info);
//                    }
//                    return objListCountry_Info;
//                }
//                else
//                {
//                    log.Info(this,"State does not exist."); //added, for log purpose
//                    return null;
//                    //throw new ApplicationException("State does not exist.");
//                }
//            }
//            catch (Exception ex)
//            {

//                log.Error(this,ex); //added, for log purpose
//                return null;
//            }
//        }

//        public string GetCityNameByCode(string cityCode)
//        {
//            try
//            {
//                //if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
//                //{
//                    DataTable dt;
//                    strSQL = "SELECT CityCode, Name FROM City";
//                    strSQL += " WHERE City.CityCode = '" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, cityCode) + "'";
//                    dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
//                    if (dt != null && dt.Rows.Count > 0)
//                    {
//                        return dt.Rows[0]["Name"].ToString();
//                    }
//                    else
//                    {
//                        return "";
//                    }
//                //}
//                //return "";
//            }
//            catch (Exception ex)
//            {
//                log.Error(this,ex); //added, for log purpose
//                return "";
//            }
//            //finally
//            //{
//            //    EndSQLControl();
//            //    EndConnection();
//            //}
//        }

//        public DataTable GetCountryCodeByName(string CountryName)
//        {

//            DataTable dt;
//            String strSQL = string.Empty;

//            try
//            {
//                strSQL = "SELECT DISTINCT CountryCode FROM COUNTRY WHERE Name='" + CountryName + "'";
//                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
//                if (dt != null && dt.Rows.Count > 0)
//                {
//                    return dt;
//                }
//                else
//                {
//                    log.Info(this,"Country does not exist."); //added, for log purpose
//                    return null;
//                    //throw new ApplicationException("Country does not exist.");
//                }
//            }
//            catch (Exception ex)
//            {
//                log.Error(this,ex); //added, for log purpose
//                return null;
//            }
//        }

//        public Country_Info GetSingleCountryCode(int pId)
//        {
//            Country_Info objCountry_Info;
//            DataTable dt;
//            DateTime dateValue;
//            String strSQL = string.Empty;
//            String strFields = string.Empty;
//            String strFilter = string.Empty;
//            List<string> lstFields = new List<string>();
//            try
//            {
//                lstFields.Add("CountryCode.Id");
//                lstFields.Add("CountryCode.countrycode");
//                lstFields.Add("CountryCode.countryName");
//                lstFields.Add("CountryCode.provincestatecode");
//                lstFields.Add("CountryCode.provinceStateName");

//                strFields = GetSqlFields(lstFields);
//                strFilter = "WHERE CountryCode.Id='" + pId + "'";
//                strSQL = "SELECT " + strFields + " FROM CountryCode " + strFilter;
//                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true

//                if (dt != null && dt.Rows.Count > 0)
//                {
//                    DataRow drRow = dt.Rows[0];

//                    objCountry_Info = new Country_Info();
//                    objCountry_Info.Id = (int)drRow["Id"];
//                    objCountry_Info.countrycode = (string)drRow["countrycode"];
//                    objCountry_Info.countryName = (string)drRow["countryName"];
//                    objCountry_Info.provincestatecode = (string)drRow["provincestatecode"];
//                    objCountry_Info.provinceStateName = (string)drRow["provinceStateName"];
//                    return objCountry_Info;
//                }
//                else
//                {
//                    log.Info(this,"CountryCode does not exist."); //added, for log purpose
//                    return null;
//                    //throw new ApplicationException("CountryCode does not exist.");
//                }
//            }
//            catch (Exception ex)
//            {
//                log.Error(this,ex); //added, for log purpose
//                return null;
//            }
//        }

//        public Country_Info SaveCountryCode(Country_Info pCountry_Info, SaveType pType)
//        {
//            objSQL.ClearFields();
//            objSQL.ClearCondtions();
//            bool rValue = false;
//            ArrayList lstSQL = new ArrayList();
//            string strSQL = string.Empty;
//            try
//            {
//                objSQL.AddField("Id", pCountry_Info.Id, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("countrycode", pCountry_Info.countrycode, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("countryName", pCountry_Info.countryName, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("provincestatecode", pCountry_Info.provincestatecode, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("provinceStateName", pCountry_Info.provinceStateName, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
//                switch (pType)
//                {
//                    case SaveType.Insert:
//                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert, "CountryCode", string.Empty);
//                        break;
//                    case SaveType.Update:
//                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "CountryCode", "CountryCode.Id='" + pCountry_Info.Id + "'");
//                        break;
//                }
//                lstSQL.Add(strSQL);
//                rValue = objDCom.BatchExecute(lstSQL, CommandType.Text, true, false);
//                if (rValue == false)
//                {
//                    return null;
//                }
//                return GetSingleCountryCode(pCountry_Info.Id);
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }

//        public List<CODEMASTER> GetAllCODEMASTERFilter(string code)
//        {
//            CODEMASTER objCODEMASTER_Info;
//            List<CODEMASTER> objListCODEMASTER_Info = new List<CODEMASTER>();
//            DataTable dt;
//            DateTime dateValue;
//            String strSQL = string.Empty;
//            String strfilter = string.Empty;
//            try
//            {
//                if (code != string.Empty)
//                {
//                    strfilter = " AND Code='" + code+"'";
//                }
//                strSQL = "SELECT * FROM CODEMASTER WHERE CODETYPE='OPT' " + strfilter;
//                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
//                if (dt != null && dt.Rows.Count > 0)
//                {
//                    foreach (DataRow drRow in dt.Rows)
//                    {
//                        objCODEMASTER_Info = new CODEMASTER();
//                        objCODEMASTER_Info.CodeType = (string)drRow["CodeType"];
//                        objCODEMASTER_Info.Code = (string)drRow["Code"];
//                        objCODEMASTER_Info.CodeDesc = (string)drRow["CodeDesc"];
//                        objCODEMASTER_Info.CodeSeq = (int)drRow["CodeSeq"];
//                        objCODEMASTER_Info.SysCode = (byte)drRow["SysCode"];
//                        objCODEMASTER_Info.rowguid = (Guid)drRow["rowguid"];
//                        if (DateTime.TryParse(drRow["SyncCreate"].ToString(), out dateValue)) objCODEMASTER_Info.SyncCreate = (DateTime)drRow["SyncCreate"];
//                        if (DateTime.TryParse(drRow["SyncLastUpd"].ToString(), out dateValue)) objCODEMASTER_Info.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
//                        objCODEMASTER_Info.IsHost = (byte)drRow["IsHost"];
//                        objListCODEMASTER_Info.Add(objCODEMASTER_Info);
//                    }
//                    return objListCODEMASTER_Info;
//                }
//                else
//                {
//                    return null;
//                    throw new ApplicationException("CODEMASTER does not exist.");
//                }
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }

//        public DataTable GetAllSettingFilter(int SYSSet, string WebID)
//        {
//            List<Settings> objListSYS_PREFT_Info = new List<Settings>();
//            DataTable dt;
//            String strSQL = string.Empty;

//            try
//            {
//                string strfilter = "";
//                if (WebID != "0")
//                    strfilter = " AND GRPID='" + WebID + "'";
//                strSQL = "SELECT AppID, GRPID, SYSKey, SYSValue, SYSValueEx, SYSSet, rowguid, SyncCreate, " +
//                "SyncLastUpd, IsHost, LastSyncBy,(CAST(AppID AS varchar(5)) + ', ' + CAST(GRPID AS varchar(5))+', '+SYSKey) as CompositeKey" +
//                " FROM SYS_PREFT WHERE SYSSet=" + SYSSet + strfilter;
//                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true
//                if (dt != null && dt.Rows.Count > 0)
//                {

//                    return dt;
//                }
//                else
//                {
//                    return null;
//                    throw new ApplicationException("SYS_PREFT does not exist.");
//                }
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }

//        public Settings GetSingleSYS_PREFT(short pAppID, string pWebID, string pSYSKey)
//        {
//            Settings objSYS_PREFT_Info;
//            DataTable dt;
//            DateTime dateValue;
//            String strSQL = string.Empty;
//            String strFields = string.Empty;
//            String strFilter = string.Empty;
//            List<string> lstFields = new List<string>();
//            try
//            {
//                lstFields.Add("SYS_PREFT.AppID");
//                lstFields.Add("SYS_PREFT.GRPID");
//                lstFields.Add("SYS_PREFT.SYSKey");
//                lstFields.Add("SYS_PREFT.SYSValue");
//                lstFields.Add("SYS_PREFT.SYSValueEx");
//                lstFields.Add("SYS_PREFT.SYSSet");
//                lstFields.Add("SYS_PREFT.rowguid");
//                lstFields.Add("SYS_PREFT.SyncCreate");
//                lstFields.Add("SYS_PREFT.SyncLastUpd");
//                lstFields.Add("SYS_PREFT.IsHost");
//                lstFields.Add("SYS_PREFT.LastSyncBy");
//                lstFields.Add("SYS_PREF.SYSDesc");
//                strFields = GetSqlFields(lstFields);
//                strFilter = "WHERE SYS_PREFT.SYSKey='" + pSYSKey + "' " + "AND SYS_PREFT.AppID='" + pAppID + "' AND SYS_PREFT.GRPID='" + pWebID + "'";
//                strSQL = "SELECT " + strFields + " FROM SYS_PREFT " +
//                    "JOIN SYS_PREF (NOLOCK) on SYS_PREF.SYSKey=SYS_PREFT.SYSKey " + strFilter;
//                dt = objDCom.Execute(strSQL, CommandType.Text, true); //amended by diana 20140124 - set to true

//                if (dt != null && dt.Rows.Count > 0)
//                {
//                    DataRow drRow = dt.Rows[0];

//                    objSYS_PREFT_Info = new Settings();
//                    objSYS_PREFT_Info.AppID = (short)drRow["AppID"];
//                    objSYS_PREFT_Info.GRPID = (string)drRow["GRPID"];
//                    objSYS_PREFT_Info.SYSKey = (string)drRow["SYSKey"];
//                    objSYS_PREFT_Info.SYSDesc = (string)drRow["SYSDesc"];
//                    objSYS_PREFT_Info.SYSValue = (string)drRow["SYSValue"];
//                    objSYS_PREFT_Info.SYSValueEx = (string)drRow["SYSValueEx"];
//                    objSYS_PREFT_Info.SYSSet = (byte)drRow["SYSSet"];
//                    objSYS_PREFT_Info.rowguid = (Guid)drRow["rowguid"];
//                    if (DateTime.TryParse(drRow["SyncCreate"].ToString(), out dateValue)) objSYS_PREFT_Info.SyncCreate = (DateTime)drRow["SyncCreate"];
//                    if (DateTime.TryParse(drRow["SyncLastUpd"].ToString(), out dateValue)) objSYS_PREFT_Info.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
//                    objSYS_PREFT_Info.IsHost = (byte)drRow["IsHost"];
//                    objSYS_PREFT_Info.LastSyncBy = (string)drRow["LastSyncBy"];
//                    return objSYS_PREFT_Info;
//                }
//                else
//                {
//                    return null;
//                    throw new ApplicationException("SYS_PREFT does not exist.");
//                }
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }

//        public string GetXMLString(object Obj)
//        {
//            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Obj.GetType());
//            System.IO.StringWriter writer = new System.IO.StringWriter();
//            x.Serialize(writer, Obj);

//            return writer.ToString();
//        }

//        public bool SaveSYS_AUDITLOG(List<AUDITLOG> lstSYS_AUDITLOG_Info)
//        {
//            bool rValue = false;
//            ArrayList lstSQL = new ArrayList();
//            string strSQL = string.Empty;
//            try
//            {
//                foreach (AUDITLOG pSYS_AUDITLOG_Info in lstSYS_AUDITLOG_Info)
//                {
//                    objSQL.AddField("TransID", pSYS_AUDITLOG_Info.TransID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
//                    objSQL.AddField("SeqNo", pSYS_AUDITLOG_Info.SeqNo, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
//                    objSQL.AddField("Action", pSYS_AUDITLOG_Info.Action, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
//                    objSQL.AddField("RefCode", pSYS_AUDITLOG_Info.RefCode, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
//                    objSQL.AddField("Table_Name", pSYS_AUDITLOG_Info.Table_Name, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
//                    objSQL.AddField("CreatedBy", pSYS_AUDITLOG_Info.CreatedBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
//                    objSQL.AddField("SQL", pSYS_AUDITLOG_Info.SQL, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
//                    objSQL.AddField("CreatedDate", pSYS_AUDITLOG_Info.CreatedDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
//                    objSQL.AddField("Priority", pSYS_AUDITLOG_Info.Priority, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
//                    objSQL.AddField("Flag", pSYS_AUDITLOG_Info.Flag, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
//                    strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert, "SYS_AUDITLOG", string.Empty);
//                    lstSQL.Add(strSQL);
//                }
//                rValue = objDCom.BatchExecute(lstSQL, CommandType.Text, true, false);
//                if (rValue == false)
//                {
//                    return false;
//                }
//                return true;
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }


//        public Settings SaveSYS_PREFT(Settings pSYS_PREFT_Info, SaveType pType)
//        {
//            bool rValue = false;
//            ArrayList lstSQL = new ArrayList();
//            string strSQL = string.Empty;
//            AUDITLOG AUDITLOGInfo = new AUDITLOG();           
//            List<AUDITLOG> lstAuditLog = new List<AUDITLOG>();
//            try
//            {
//                Settings CheckSetting=GetSingleSYS_PREFT(pSYS_PREFT_Info.AppID, pSYS_PREFT_Info.GRPID, pSYS_PREFT_Info.SYSKey);

//                objSQL.AddField("GRPID", pSYS_PREFT_Info.GRPID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("SYSKey", pSYS_PREFT_Info.SYSKey, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("SYSValue", pSYS_PREFT_Info.SYSValue, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("SYSValueEx", pSYS_PREFT_Info.SYSValueEx, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("SYSSet", pSYS_PREFT_Info.SYSSet, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("SyncCreate", pSYS_PREFT_Info.SyncCreate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("SyncLastUpd", pSYS_PREFT_Info.SyncLastUpd, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("IsHost", pSYS_PREFT_Info.IsHost, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("LastSyncBy", pSYS_PREFT_Info.LastSyncBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
//                switch (pType)
//                {
//                    case SaveType.Insert:
//                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert, "SYS_PREFT", string.Empty);
//                        break;
//                    case SaveType.Update:
//                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "SYS_PREFT", "SYS_PREFT.AppID='" + pSYS_PREFT_Info.AppID + "' AND SYS_PREFT.GRPID='" + pSYS_PREFT_Info.GRPID + "' AND SYS_PREFT.SYSKey='" + pSYS_PREFT_Info.SYSKey + "'");
//                        break;
//                }
//                lstSQL.Add(strSQL);

//                AUDITLOGInfo.TransID = DateTime.Now.ToString("yyyyMMddHHmmsss"); 
//                AUDITLOGInfo.SeqNo = 0;
//                AUDITLOGInfo.Action = 1;
//                AUDITLOGInfo.RefCode = "";
//                AUDITLOGInfo.Table_Name = "SYS_PREFT";
//                AUDITLOGInfo.SQL = strSQL;
//                AUDITLOGInfo.CreatedBy = pSYS_PREFT_Info.LastSyncBy;
//                AUDITLOGInfo.CreatedDate = DateTime.Now;
//                AUDITLOGInfo.Priority = 0;
//                lstAuditLog.Add(AUDITLOGInfo);                
//                rValue = objDCom.BatchExecute(lstSQL, CommandType.Text, true, false);
//                if (rValue == false)
//                {
//                    return null;
//                }
//                if (CheckSetting.SYSValue != pSYS_PREFT_Info.SYSValue)
//                    SaveSYS_AUDITLOG(lstAuditLog);

//                return GetSingleSYS_PREFT(pSYS_PREFT_Info.AppID, pSYS_PREFT_Info.GRPID, pSYS_PREFT_Info.SYSKey);
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }
//        private static String GetSqlFields(List<string> Fields)
//        {
//            String strFields = string.Empty;
//            if (Fields != null)
//            {
//                foreach (string sField in Fields)
//                {
//                    if (strFields == string.Empty)
//                    {
//                        strFields = sField;
//                    }
//                    else
//                    {
//                        strFields += ", " + sField;
//                    }
//                }
//            }
//            return strFields;
//        }

//        public string getSysValueByKey(string key)
//        {
//            String strSQL = string.Empty;
//            DataTable dt = new DataTable();
//            try
//            {
//                //if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
//                //{
//                //    StartSQLControl();

//                    strSQL = "SELECT SYSValue FROM SYS_PREFT WHERE SYSKey = '" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, key) + "'";
//                    dt = objDCom.Execute(strSQL, System.Data.CommandType.Text, true);
//                    if (dt.Rows.Count > 0)
//                    {
//                        return dt.Rows[0]["SYSValue"].ToString();
//                    }
//                    else
//                    {
//                        return "";
//                    }
//                //}
//                //return "";
//            }
//            catch (Exception ex)
//            {
//                return "";
//            }
//            //finally
//            //{
//            //    EndSQLControl();
//            //    EndConnection();
//            //}
//        }

//        public string getSysValueByKeyAndCarrierCode(string key, string carrierCode)
//        {
//            String strSQL = string.Empty;
//            DataTable dt = new DataTable();
//            try
//            {
//                //if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
//                //{
//                //    StartSQLControl();

//                    strSQL = "SELECT SYS_PREFT.SYSValue FROM OPTGroup INNER JOIN SYS_PREFT ON OPTGroup.GroupName = SYS_PREFT.GrpID WHERE OPTGroup.CarrierCode = '" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, carrierCode) + "' AND SYS_PREFT.SYSKey = '" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, key) + "'";
//                    dt = objDCom.Execute(strSQL, System.Data.CommandType.Text, true);
//                    if (dt.Rows.Count > 0)
//                    {
//                        return dt.Rows[0]["SYSValue"].ToString();
//                    }
//                    else
//                    {
//                        return "";
//                    }
//                //}
//                //return "";
//            }
//            catch (Exception ex)
//            {
//                log.Error(this, ex);
//                return "";
//            }
//            //finally
//            //{
//            //    EndSQLControl();
//            //    EndConnection();
//            //}
//        }

//        public string getSysValueByKeyAndGroupID(string key, string groupID)
//        {
//            String strSQL = string.Empty;
//            DataTable dt = new DataTable();
//            try
//            {
//                //if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
//                //{
//                //    StartSQLControl();

//                    strSQL = "SELECT SYS_PREFT.SYSValue FROM SYS_PREFT WHERE SYS_PREFT.GrpID  = '" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, groupID) + "' AND SYS_PREFT.SYSKey = '" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, key) + "'";
//                    dt = objDCom.Execute(strSQL, System.Data.CommandType.Text, true);
//                    if (dt.Rows.Count > 0)
//                    {
//                        return dt.Rows[0]["SYSValue"].ToString();
//                    }
//                    else
//                    {
//                        return "";
//                    }
//                //}
//                //return "";
//            }
//            catch (Exception ex)
//            {
//                return "";
//            }
//            //finally
//            //{
//            //    EndSQLControl();
//            //    EndConnection();
//            //}
//        }

//        public string getOPTGroupByCarrierCode(string carrierCode)
//        {
//            String strSQL = string.Empty;
//            DataTable dt = new DataTable();
//            try
//            {
//                //if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
//                //{
//                //    StartSQLControl();

//                    strSQL = "SELECT OPTGroup.GroupName FROM OPTGroup WHERE OPTGroup.CarrierCode = '" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, carrierCode)  + "'";
//                    dt = objDCom.Execute(strSQL, System.Data.CommandType.Text, true);
//                    if (dt.Rows.Count > 0)
//                    {
//                        return dt.Rows[0]["GroupName"].ToString();
//                    }
//                    else
//                    {
//                        return "";
//                    }
//                //}
//                //return "";
//            }
//            catch (Exception ex)
//            {
//                return "";
//            }
//            //finally
//            //{
//            //    EndSQLControl();
//            //    EndConnection();
//            //}
//        }

//        /// <summary>
//        /// get visibility from OPTGroup
//        /// </summary>
//        /// <param name="carrierCode"></param>
//        /// <returns></returns>
//        public Boolean getVisibilityByCarrierCode(string carrierCode)
//        {
//            String strSQL = string.Empty;
//            DataTable dt = new DataTable();
//            try
//            {
//                //if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
//                //{
//                //    StartSQLControl();

//                    strSQL = "SELECT OPTGroup.WebID FROM OPTGroup WHERE OPTGroup.CarrierCode = '" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, carrierCode) + "'";
//                    dt = objDCom.Execute(strSQL, System.Data.CommandType.Text, true);
//                    if (dt.Rows.Count > 0)
//                    {
//                        if (dt.Rows[0]["WebID"].ToString() == "1")
//                            return true;
//                        else
//                            return false;
//                    }
//                    else
//                    {
//                        return false;
//                    }
//                //}
//                //return false;
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//            //finally
//            //{
//            //    EndSQLControl();
//            //    EndConnection();
//            //}
//        }
//        /// <summary>
//        /// added by diana 20140121
//        /// </summary>
//        /// <param name="key"></param>
//        /// <param name="carrierCode"></param>
//        /// <returns></returns>
//        public decimal getDeposit(int totalPax, string currency, string cityCode, string connectingCityCode = "")
//        {
//            String strSQL = string.Empty;
//            DataTable dt = new DataTable();
//            try
//            {
//                //if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
//                //{
//                //    StartSQLControl();

//                    strSQL = "SELECT DepositValue FROM COUNTRYDEPOSIT WHERE Currency='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, currency) + "' AND HaulCode=(SELECT HaulCode FROM COUNTRYHAUL WHERE CityCode='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, cityCode) + "')";
//                    dt = objDCom.Execute(strSQL, System.Data.CommandType.Text, true);
//                    if (dt.Rows.Count > 0)
//                    {
//                        if (connectingCityCode == "")
//                            return Decimal.Parse(dt.Rows[0]["DepositValue"].ToString()) * totalPax;
//                        else
//                            return Decimal.Parse(dt.Rows[0]["DepositValue"].ToString()) * 2 * totalPax;
//                    }
//                    else
//                    {
//                        return 0;
//                    }
//                //}
//                //return 0;
//            }
//            catch (Exception ex)
//            {
//                log.Error(this,ex);
//                return 0;
//            }
//            //finally
//            //{
//            //    EndSQLControl();
//            //    EndConnection();
//            //}
//        }

//        public decimal getDepositBackup(int totalPax, string currency, string cityCode, string connectingCityCode = "")
//        {
//            String strSQL = string.Empty;
//            DataTable dt = new DataTable();
//            try
//            {
//                //if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
//                //{
//                //    StartSQLControl();

//                    strSQL = "SELECT DepositValue FROM COUNTRYDEPOSIT WHERE Currency='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, currency) + "' AND HaulCode=(SELECT HaulCode FROM COUNTRYHAUL WHERE CityCode='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, cityCode) + "')";
//                    dt = objDCom.Execute(strSQL, System.Data.CommandType.Text, true);
//                    if (dt.Rows.Count > 0)
//                    {
//                        if (connectingCityCode == "")
//                            return Decimal.Parse(dt.Rows[0]["DepositValue"].ToString()) * totalPax;
//                        else
//                            return Decimal.Parse(dt.Rows[0]["DepositValue"].ToString()) * 2 * totalPax;
//                    }
//                    else
//                    {
//                        return 0;
//                    }
//                //}
//                //return 0;
//            }
//            catch (Exception ex)
//            {
//                log.Error(this,ex);
//                return 0;
//            }
//            //finally
//            //{
//            //    EndSQLControl();
//            //    EndConnection();
//            //}
//        }

//        public string GenerateRandom(int Length)
//        {
//            char[] constant = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

//            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(constant.Length);

//            Random rd = new Random();
//            for (int i = 0; i < Length; i++)
//            {
//                newRandom.Append(constant[rd.Next(0, constant.Length - 1)]);
//            }

//            return newRandom.ToString().ToLower();
//        }

//        public decimal RoundUp(decimal amount)
//        {
//            return Math.Round(amount, 2, MidpointRounding.AwayFromZero);
//        }

//        #region Hashing
//        public string GenerateMac(string strSharedKey, string strAID, string strName)
//        {
//            string strMac = string.Empty;
//            string strEncrypt = strSharedKey + strAID;
//            strMac = Sha256AddSecret(strEncrypt);

//            return strMac;
//        }

//        public string Sha256AddSecret(string strChange)
//        {
//            //Change the syllable into UTF8 code
//            byte[] pass = Encoding.UTF8.GetBytes(strChange);
//            SHA256 sha256 = new SHA256CryptoServiceProvider();
//            byte[] hashValue = sha256.ComputeHash(pass);

//            return BitConverter.ToString(hashValue).Replace("-", "").ToLower();
//        }
//        #endregion
//    }
//}
