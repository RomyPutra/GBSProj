using Abp.Reflection.Extensions;
using Microsoft.Extensions.Configuration;
using Plexform.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Linq;
using SEAL.Data;
using System.Data.SqlClient;
using System.Collections;
using Sharpbrake.Client;

namespace Plexform.GBS
{
    public class GBSAdminLogic : SEAL.Model.Moyenne.CoreStandard
    {
        protected static DataAccess objDCom;

        private readonly IConfigurationRoot _appConfiguration;

        public GBSAdminLogic()
        {
            _appConfiguration = AppConfigurations.Get(
                typeof(GBSAdminLogic).GetAssembly().GetDirectoryPathOrNull()
            );
            objSQL = new SQLControl();
            objDCom = new DataAccess();
            objDCom.ConnectionString = _appConfiguration.GetConnectionString(PlexformConsts.GBSConnectionString);
        }

        #region Logic_PaymentScheme
        public class PaymentInfo
        {
            private string _schemeCode = String.Empty;
            private string _gRPID = String.Empty;
            private string _countrycode = String.Empty;
            private string _currencycode = String.Empty;
            private int _minduration;
            private int _duration;
            private int _firstDeposit;
            private string _description = String.Empty;
            private string _paymentType = String.Empty;
            private int _attempt_1;
            private string _code_1;
            private int _percentage_1;
            private int _attempt_2;
            private string _code_2;
            private int _percentage_2;
            private int _attempt_3;
            private string _code_3;
            private int _percentage_3;
            private string _paymentMode = String.Empty;
            private string _createBy = String.Empty;
            private DateTime _syncCreate;
            private DateTime _syncLastUpd;
            private string _lastSyncBy = String.Empty;
            private int _reminder_1;
            private int _reminder_2;

            private int _deposit_1;
            private int _isnominal_1;
            private int _ldeposit_11;
            private int _lduration_11;
            private int _ldeposit_12;
            private int _lduration_12;
            private int _ldeposit_13;
            private int _lduration_13;
            private int _pdeposit_11;
            private int _pduration_11;
            private int _pdeposit_12;
            private int _pduration_12;
            private int _pdeposit_13;
            private int _pduration_13;
            private int _deposit_2;
            private int _deposit_3;
            private int _mindeposit;
            private int _maxdeposit;
            private int _mindeposit2;
            private int _maxdeposit2;
            private int _depositvalue;
            //private string _prevcountry;

            #region Public Properties
            public string SchemeCode
            {
                get { return _schemeCode; }
                set { _schemeCode = value; }
            }
            public string GRPID
            {
                get { return _gRPID; }
                set { _gRPID = value; }
            }
            public string CountryCode
            {
                get { return _countrycode; }
                set { _countrycode = value; }
            }
            public string CurrencyCode
            {
                get { return _currencycode; }
                set { _currencycode = value; }
            }
            public int MinDuration
            {
                get { return _minduration; }
                set { _minduration = value; }
            }
            public int Duration
            {
                get { return _duration; }
                set { _duration = value; }
            }

            public int FirstDeposit
            {
                get { return _firstDeposit; }
                set { _firstDeposit = value; }
            }

            public string Description
            {
                get { return _description; }
                set { _description = value; }
            }

            public string PaymentType
            {
                get { return _paymentType; }
                set { _paymentType = value; }
            }

            public int Attempt_1
            {
                get { return _attempt_1; }
                set { _attempt_1 = value; }
            }

            public string Code_1
            {
                get { return _code_1; }
                set { _code_1 = value; }
            }

            public int Percentage_1
            {
                get { return _percentage_1; }
                set { _percentage_1 = value; }
            }

            public int Attempt_2
            {
                get { return _attempt_2; }
                set { _attempt_2 = value; }
            }

            public string Code_2
            {
                get { return _code_2; }
                set { _code_2 = value; }
            }

            public int Percentage_2
            {
                get { return _percentage_2; }
                set { _percentage_2 = value; }
            }

            public int Attempt_3
            {
                get { return _attempt_3; }
                set { _attempt_3 = value; }
            }

            public string Code_3
            {
                get { return _code_3; }
                set { _code_3 = value; }
            }

            public int Percentage_3
            {
                get { return _percentage_3; }
                set { _percentage_3 = value; }
            }

            public string PaymentMode
            {
                get { return _paymentMode; }
                set { _paymentMode = value; }
            }

            public string CreateBy
            {
                get { return _createBy; }
                set { _createBy = value; }
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

            public int Reminder_1
            {
                get { return _reminder_1; }
                set { _reminder_1 = value; }
            }

            public int Reminder_2
            {
                get { return _reminder_2; }
                set { _reminder_2 = value; }
            }

            public int Deposit_1
            {
                get { return _deposit_1; }
                set { _deposit_1 = value; }
            }
            public int IsNominal_1
            {
                get { return _isnominal_1; }
                set { _isnominal_1 = value; }
            }
            public int LDeposit_11
            {
                get { return _ldeposit_11; }
                set { _ldeposit_11 = value; }
            }
            public int LDuration_11
            {
                get { return _lduration_11; }
                set { _lduration_11 = value; }
            }
            public int LDeposit_12
            {
                get { return _ldeposit_12; }
                set { _ldeposit_12 = value; }
            }
            public int LDuration_12
            {
                get { return _lduration_12; }
                set { _lduration_12 = value; }
            }
            public int LDeposit_13
            {
                get { return _ldeposit_13; }
                set { _ldeposit_13 = value; }
            }
            public int LDuration_13
            {
                get { return _lduration_13; }
                set { _lduration_13 = value; }
            }
            public int PDeposit_11
            {
                get { return _pdeposit_11; }
                set { _pdeposit_11 = value; }
            }
            public int PDuration_11
            {
                get { return _pduration_11; }
                set { _pduration_11 = value; }
            }
            public int PDeposit_12
            {
                get { return _pdeposit_12; }
                set { _pdeposit_12 = value; }
            }
            public int PDuration_12
            {
                get { return _pduration_12; }
                set { _pduration_12 = value; }
            }
            public int PDeposit_13
            {
                get { return _pdeposit_13; }
                set { _pdeposit_13 = value; }
            }
            public int PDuration_13
            {
                get { return _pduration_13; }
                set { _pduration_13 = value; }
            }



            public int Deposit_2
            {
                get { return _deposit_2; }
                set { _deposit_2 = value; }
            }

            public int Deposit_3
            {
                get { return _deposit_3; }
                set { _deposit_3 = value; }
            }

            public int Mindeposit
            {
                get { return _mindeposit; }
                set { _mindeposit = value; }
            }

            public int Maxdeposit
            {
                get { return _maxdeposit; }
                set { _maxdeposit = value; }
            }

            public int Mindeposit2
            {
                get { return _mindeposit2; }
                set { _mindeposit2 = value; }
            }

            public int Maxdeposit2
            {
                get { return _maxdeposit2; }
                set { _maxdeposit2 = value; }
            }

            public int DepositValue
            {
                get { return _depositvalue; }
                set { _depositvalue = value; }
            }

            //public string PrevCountry
            //{
            //	get { return _prevcountry; }
            //	set { _prevcountry = value; }
            //}
            #endregion
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
        public DataTable GetSchemeByCode(string GRPID, string CountryCode = "", string SchemeCode = "")
        {
            DataTable dt = new DataTable();
            String strSQL = string.Empty;
            String strFields = string.Empty;
            String strFilter = string.Empty;
            List<string> lstFields = new List<string>();

            String strJoint = string.Empty;
            strJoint = "OUTER APPLY (SELECT MAX(D.MinDeposit) MinDeposit, MAX(D.MaxDeposit) MaxDeposit, MAX(D.MinDeposit2) MinDeposit2, MAX(D.MaxDeposit2) MaxDeposit2, MAX(D.DepositValue) DepositValue, MAX(D.Currency) Currency FROM Depoduration D WHERE D.GroupCode = DEPOPAYSCHEME.GRPID And D.Currency = DEPOPAYSCHEME.CurrencyCode) D ";
            //strJoint = "OUTER APPLY (SELECT MAX(D.MinDeposit) MinDeposit, MAX(D.MaxDeposit) MaxDeposit, MAX(D.MinDeposit2) MinDeposit2, MAX(D.MaxDeposit2) MaxDeposit2, MAX(D.Currency) Currency, MAX(D.PREVCOUNTRY) PrevCountry FROM Depoduration D WHERE D.GroupCode = DEPOPAYSCHEME.GRPID And D.Currency = DEPOPAYSCHEME.CurrencyCode AND D.PREVCOUNTRY = DEPOPAYSCHEME.PREVCOUNTRY) D ";

            DateTime bookingDate = Convert.ToDateTime("1900-01-01");
            DateTime CheckDate = Convert.ToDateTime("2017-02-10");
            Boolean newGBS = true;
            try
            {
                lstFields.Add("DEPOPAYSCHEME.SchemeCode");
                lstFields.Add("DEPOPAYSCHEME.CountryCode");
                lstFields.Add("DEPOPAYSCHEME.GRPID");
                lstFields.Add("DEPOPAYSCHEME.Duration");
                lstFields.Add("DEPOPAYSCHEME.MinDuration");
                lstFields.Add("DEPOPAYSCHEME.FirstDeposit");
                lstFields.Add("DEPOPAYSCHEME.Description");
                lstFields.Add("DEPOPAYSCHEME.PaymentType");
                lstFields.Add("DEPOPAYSCHEME.Attempt_1");
                lstFields.Add("DEPOPAYSCHEME.Code_1");
                lstFields.Add("DEPOPAYSCHEME.Percentage_1");
                lstFields.Add("DEPOPAYSCHEME.Attempt_2");
                lstFields.Add("DEPOPAYSCHEME.Code_2");
                lstFields.Add("DEPOPAYSCHEME.Percentage_2");
                lstFields.Add("DEPOPAYSCHEME.Attempt_3");
                lstFields.Add("DEPOPAYSCHEME.Code_3");
                lstFields.Add("DEPOPAYSCHEME.Percentage_3");
                lstFields.Add("DEPOPAYSCHEME.PaymentMode");
                lstFields.Add("DEPOPAYSCHEME.CreateBy");
                lstFields.Add("DEPOPAYSCHEME.SyncCreate");
                lstFields.Add("DEPOPAYSCHEME.SyncLastUpd");
                lstFields.Add("DEPOPAYSCHEME.LastSyncBy");
                lstFields.Add("DEPOPAYSCHEME.Reminder_1");
                lstFields.Add("DEPOPAYSCHEME.Reminder_2");

                lstFields.Add("DEPOPAYSCHEME.Deposit_1");
                lstFields.Add("DEPOPAYSCHEME.IsNominal_1");
                lstFields.Add("DEPOPAYSCHEME.LDeposit_11");
                lstFields.Add("DEPOPAYSCHEME.LDuration_11");
                lstFields.Add("DEPOPAYSCHEME.LDeposit_12");
                lstFields.Add("DEPOPAYSCHEME.LDuration_12");
                lstFields.Add("DEPOPAYSCHEME.LDeposit_13");
                lstFields.Add("DEPOPAYSCHEME.LDuration_13");
                lstFields.Add("DEPOPAYSCHEME.PDeposit_11");
                lstFields.Add("DEPOPAYSCHEME.PDuration_11");
                lstFields.Add("DEPOPAYSCHEME.PDeposit_12");
                lstFields.Add("DEPOPAYSCHEME.PDuration_12");
                lstFields.Add("DEPOPAYSCHEME.PDeposit_13");
                lstFields.Add("DEPOPAYSCHEME.PDuration_13");
                lstFields.Add("DEPOPAYSCHEME.Deposit_2");
                lstFields.Add("DEPOPAYSCHEME.Deposit_3");
                //lstFields.Add("DEPOPAYSCHEME.PrevCountry");

                lstFields.Add("D.Currency");
                lstFields.Add("D.MinDeposit");
                lstFields.Add("D.MaxDeposit");
                lstFields.Add("D.MinDeposit2");
                lstFields.Add("D.MaxDeposit2");
                lstFields.Add("D.DepositValue");
                //lstFields.Add("D.PrevCountry");

                strFields = GetSqlFields(lstFields);
                if (CountryCode == "" || SchemeCode == "")
                    strFilter = "WHERE DEPOPAYSCHEME.GRPID='" + GRPID + "'";
                else
                    strFilter = "WHERE DEPOPAYSCHEME.GRPID='" + GRPID + "' AND DEPOPAYSCHEME.CountryCode='" + CountryCode + "' AND DEPOPAYSCHEME.SchemeCode='" + SchemeCode + "'";

                strSQL = "SELECT " + strFields + " FROM DEPOPAYSCHEME  " + strJoint + strFilter + " ORDER BY MinDuration DESC";

                using (var connection = new SqlConnection(_appConfiguration.GetConnectionString(PlexformConsts.GBSConnectionString)))
                {
                    connection.Open();
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
                        return null;
                        throw new ApplicationException("PAYSCHEME does not exist.");
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool SavePaymentScheme(PaymentInfo[] pInfo)
        {
            bool rValue = false;
            ArrayList lstSQL = new ArrayList();
            string strSQL = string.Empty;
            try
            {
                foreach (PaymentInfo xInfo in pInfo)
                {
                    objSQL.AddField("DEPOPAYSCHEME.SchemeCode", xInfo.SchemeCode, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("DEPOPAYSCHEME.CountryCode", xInfo.CountryCode, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("DEPOPAYSCHEME.Duration", xInfo.Duration, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("DEPOPAYSCHEME.MinDuration", xInfo.MinDuration, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("DEPOPAYSCHEME.PaymentType", xInfo.PaymentType, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("DEPOPAYSCHEME.Attempt_1", xInfo.Attempt_1, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("DEPOPAYSCHEME.Code_1", xInfo.Code_1, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("DEPOPAYSCHEME.Percentage_1", xInfo.Percentage_1, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("DEPOPAYSCHEME.Attempt_2", xInfo.Attempt_2, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("DEPOPAYSCHEME.Code_2", xInfo.Code_2, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("DEPOPAYSCHEME.Percentage_2", xInfo.Percentage_2, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("DEPOPAYSCHEME.Attempt_3", xInfo.Attempt_3, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("DEPOPAYSCHEME.Code_3", xInfo.Code_3, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("DEPOPAYSCHEME.Percentage_3", xInfo.Percentage_3, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                    //objSQL.AddField("DEPOPAYSCHEME.PrevCountry", xInfo.PrevCountry, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);

                    strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "DEPOPAYSCHEME", "DEPOPAYSCHEME.GRPID='" + xInfo.GRPID + "' AND DEPOPAYSCHEME.CountryCode='" + xInfo.CountryCode + "' AND DEPOPAYSCHEME.SchemeCode='" + xInfo.SchemeCode + "'");
                    //strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "DEPOPAYSCHEME", "DEPOPAYSCHEME.GRPID='" + xInfo.GRPID + "' AND DEPOPAYSCHEME.CountryCode='" + xInfo.CountryCode + "' AND DEPOPAYSCHEME.SchemeCode='" + xInfo.SchemeCode + "' AND DEPOPAYSCHEME.PrevCountry='" + xInfo.PrevCountry + "'");
                    lstSQL.Add(strSQL);

                    strSQL = "UPDATE DEPODURATION SET DEPODURATION.Currency = '" + xInfo.CurrencyCode + "', DEPODURATION.MinDeposit = '" + xInfo.Mindeposit + "', DEPODURATION.MaxDeposit = '" + xInfo.Maxdeposit + "', DEPODURATION.MinDeposit2 = '" + xInfo.Mindeposit2 + "', DEPODURATION.MaxDeposit2 = '" + xInfo.Maxdeposit2 + "', DEPODURATION.DepositValue = '" + xInfo.DepositValue + "' FROM DEPOPAYSCHEME inner join Depoduration D ON D.GroupCode = DEPOPAYSCHEME.GRPID And D.Currency = DEPOPAYSCHEME.CurrencyCode WHERE DEPOPAYSCHEME.GRPID='" + xInfo.GRPID + "' AND DEPOPAYSCHEME.CountryCode='" + xInfo.CountryCode + "' AND DEPOPAYSCHEME.SchemeCode='" + xInfo.SchemeCode + "'";
                    //strSQL = "UPDATE DEPODURATION SET DEPODURATION.Currency = '" + xInfo.CurrencyCode + "', DEPODURATION.MinDeposit = '" + xInfo.Mindeposit + "', DEPODURATION.MaxDeposit = '" + xInfo.Maxdeposit + "', DEPODURATION.MinDeposit2 = '" + xInfo.Mindeposit2 + "', DEPODURATION.MaxDeposit2 = '" + xInfo.Maxdeposit2 + "', DEPODURATION.PrevCountry = '" + xInfo.PrevCountry + "' FROM DEPOPAYSCHEME inner join Depoduration D ON D.GroupCode = DEPOPAYSCHEME.GRPID And D.Currency = DEPOPAYSCHEME.CurrencyCode And D.PrevCountry = DEPOPAYSCHEME.PrevCountry WHERE DEPOPAYSCHEME.GRPID='" + xInfo.GRPID + "' AND DEPOPAYSCHEME.CountryCode='" + xInfo.CountryCode + "' AND DEPOPAYSCHEME.SchemeCode='" + xInfo.SchemeCode + "' AND DEPOPAYSCHEME.PrevCountry='" + xInfo.PrevCountry + "'";
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
                return false;
            }
        }
        public DataTable GetCountry(string Code = "")
        {
            DataTable dt = new DataTable();
            String strSQL = string.Empty;
            String strFields = string.Empty;
            List<string> lstFields = new List<string>();

            Boolean newGBS = true;
            try
            {
                lstFields.Add("CountryCode");
                lstFields.Add("Name");
                lstFields.Add("DefaultCurrencyCode");

                strFields = GetSqlFields(lstFields);
                if (Code == "")
                    strSQL = "SELECT " + strFields + " FROM COUNTRY ORDER BY Name ASC";
                else
                    strSQL = "SELECT " + strFields + " FROM COUNTRY WHERE CountryCode ='" + Code + "' ORDER BY Name ASC";

                using (var connection = new SqlConnection(_appConfiguration.GetConnectionString(PlexformConsts.GBSConnectionString)))
                {
                    connection.Open();
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
                        return null;
                        throw new ApplicationException("Country does not exist.");
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Logic_FlightTimeGroup
        public class FlttimegroupInfo
        {
            private string _FTGroupCode = String.Empty;
            private DateTime _StartTime;
            private DateTime _EndTime;
            private System.Guid _rowguid;
            private DateTime _SyncCreate;
            private DateTime _SyncLastUpd;
            private DateTime _CreateDate;
            private DateTime _UpdateDate;
            private string _LastSyncBy = String.Empty;
            private string _CreateBy = String.Empty;
            private string _UpdateBy = String.Empty;
            private byte _Active;
            private byte _Status;
            private byte _Inuse;

            #region Public Properties
            // '' <summary>
            // '' Mandatory
            // '' </summary>
            public string FTGroupCode
            {
                get
                {
                    return _FTGroupCode;
                }
                set
                {
                    _FTGroupCode = value;
                }
            }
            public DateTime StartTime
            {
                get
                {
                    return _StartTime;
                }
                set
                {
                    _StartTime = value;
                }
            }
            public DateTime EndTime
            {
                get
                {
                    return _EndTime;
                }
                set
                {
                    _EndTime = value;
                }
            }
            public System.Guid rowguid
            {
                get
                {
                    return _rowguid;
                }
                set
                {
                    _rowguid = value;
                }
            }
            public byte Status
            {
                get
                {
                    return _Status;
                }
                set
                {
                    _Status = value;
                }
            }
            public byte Inuse
            {
                get
                {
                    return _Inuse;
                }
                set
                {
                    _Inuse = value;
                }
            }
            public DateTime SyncCreate
            {
                get
                {
                    return _SyncCreate;
                }
                set
                {
                    _SyncCreate = value;
                }
            }
            public DateTime SyncLastUpd
            {
                get
                {
                    return _SyncLastUpd;
                }
                set
                {
                    _SyncLastUpd = value;
                }
            }
            public string LastSyncBy
            {
                get
                {
                    return _LastSyncBy;
                }
                set
                {
                    _LastSyncBy = value;
                }
            }
            public DateTime CreateDate
            {
                get
                {
                    return _CreateDate;
                }
                set
                {
                    _CreateDate = value;
                }
            }
            public string CreateBy
            {
                get
                {
                    return _CreateBy;
                }
                set
                {
                    _CreateBy = value;
                }
            }
            public DateTime UpdateDate
            {
                get
                {
                    return _UpdateDate;
                }
                set
                {
                    _UpdateDate = value;
                }
            }
            public string UpdateBy
            {
                get
                {
                    return _UpdateBy;
                }
                set
                {
                    _UpdateBy = value;
                }
            }
            public byte Active
            {
                get
                {
                    return _Active;
                }
                set
                {
                    _Active = value;
                }
            }
            #endregion
        }
        public DataTable GetFLTTIMEGROUPList(string FieldCond = "", string SQL = "")
        {
            DataTable dt = new DataTable();
            String strSQL = string.Empty;
            try
            {
                //strSQL = "SELECT FTGroupCode,convert(varchar(5), StartTime, 108) StartTime,convert(varchar(5), EndTime, 108) EndTime,Status,Flag,Inuse,SyncCreate,SyncLastUpd,LastSyncBy,CreateDate,";
                strSQL = "SELECT FTGroupCode,CAST(Starttime as Time(0)) StartTime,CAST(Endtime as Time(0)) EndTime,Status,Flag,Inuse,SyncCreate,SyncLastUpd,LastSyncBy,CreateDate,";
                strSQL += "CreateBy,UpdateDate,UpdateBy,Active FROM AD_FlTTIMEGROUP " + FieldCond;

                using (var connection = new SqlConnection(_appConfiguration.GetConnectionString(PlexformConsts.GBSConnectionString)))
                {
                    connection.Open();
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
                        return null;
                        throw new ApplicationException("FlightTime does not exist.");
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool SaveFlightTimeGroup(Models.FltTimeGroupModels[] pInfo)
        {
            bool rValue = false;
            ArrayList lstSQL = new ArrayList();
            string strSQL = string.Empty;
            try
            {
                foreach (Models.FltTimeGroupModels Ad_flttimegroupCont in pInfo)
                {
                    var StartTime = Ad_flttimegroupCont.StartTime.ToString();
                    var EndTime = Ad_flttimegroupCont.EndTime.ToString();

                    objSQL.AddField("FTGroupCode", Ad_flttimegroupCont.FTGroupCode, SQLControl.EnumDataType.dtString);
                    objSQL.AddField("StartTime", StartTime, SQLControl.EnumDataType.dtString);
                    objSQL.AddField("EndTime", EndTime, SQLControl.EnumDataType.dtString);
                    //objSQL.AddField("rowguid", Ad_flttimegroupCont.rowguid, SQLControl.EnumDataType.dtString);
                    //objSQL.AddField("Status", Ad_flttimegroupCont.Status, SQLControl.EnumDataType.dtNumeric);
                    //objSQL.AddField("Inuse", Ad_flttimegroupCont.Inuse, SQLControl.EnumDataType.dtNumeric);
                    objSQL.AddField("SyncCreate", Ad_flttimegroupCont.SyncCreate, SQLControl.EnumDataType.dtDateTime);
                    objSQL.AddField("SyncLastUpd", Ad_flttimegroupCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime);
                    objSQL.AddField("LastSyncBy", Ad_flttimegroupCont.LastSyncBy, SQLControl.EnumDataType.dtString);
                    objSQL.AddField("CreateDate", Ad_flttimegroupCont.CreateDate, SQLControl.EnumDataType.dtDateTime);
                    objSQL.AddField("CreateBy", Ad_flttimegroupCont.CreateBy, SQLControl.EnumDataType.dtString);
                    objSQL.AddField("UpdateDate", Ad_flttimegroupCont.UpdateDate, SQLControl.EnumDataType.dtDateTime);
                    objSQL.AddField("UpdateBy", Ad_flttimegroupCont.UpdateBy, SQLControl.EnumDataType.dtString);
                    objSQL.AddField("Active", Ad_flttimegroupCont.Active, SQLControl.EnumDataType.dtNumeric);

                    strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert, "AD_FLTTIMEGROUP");
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
                return false;
            }
        }
        #endregion

        #region AGENTACCESSFARE
        public class AGENTACCESSFAREInfo
        {
            private System.String _MarketCode = String.Empty;
            private System.String _InTier = String.Empty;
            private System.String _OutTier = String.Empty;
            private System.String _InFareClass = String.Empty;
            private System.String _OutFareClass = String.Empty;
            private System.Guid _rowguid;
            private System.Byte _Status;
            private System.Byte _Inuse;
            private System.DateTime _SyncCreate;
            private System.DateTime _SyncLastUpd;
            private System.String _LastSyncBy = String.Empty;
            private System.DateTime _CreateDate;
            private System.String _CreateBy = String.Empty;
            private System.DateTime _UpdateDate;
            private System.String _UpdateBy = String.Empty;
            private System.Byte _Active;

            #region Public Properties
            public String MarketCode
            {
                get
                {
                    return _MarketCode;
                }
                set
                {
                    _MarketCode = value;
                }
            }
            public String InTier
            {
                get
                {
                    return _InTier;
                }
                set
                {
                    _InTier = value;
                }
            }
            public String OutTier
            {
                get
                {
                    return _OutTier;
                }
                set
                {
                    _OutTier = value;
                }
            }
            public String InFareClass
            {
                get
                {
                    return _InFareClass;
                }
                set
                {
                    _InFareClass = value;
                }
            }
            public String OutFareClass
            {
                get
                {
                    return _OutFareClass;
                }
                set
                {
                    _OutFareClass = value;
                }
            }
            public System.Guid rowguid
            {
                get
                {
                    return _rowguid;
                }
                set
                {
                    _rowguid = value;
                }
            }
            public Byte Status
            {
                get
                {
                    return _Status;
                }
                set
                {
                    _Status = value;
                }
            }
            public Byte Inuse
            {
                get
                {
                    return _Inuse;
                }
                set
                {
                    _Inuse = value;
                }
            }
            public DateTime SyncCreate
            {
                get
                {
                    return _SyncCreate;
                }
                set
                {
                    _SyncCreate = value;
                }
            }
            public DateTime SyncLastUpd
            {
                get
                {
                    return _SyncLastUpd;
                }
                set
                {
                    _SyncLastUpd = value;
                }
            }
            public String LastSyncBy
            {
                get
                {
                    return _LastSyncBy;
                }
                set
                {
                    _LastSyncBy = value;
                }
            }
            public DateTime CreateDate
            {
                get
                {
                    return _CreateDate;
                }
                set
                {
                    _CreateDate = value;
                }
            }
            public String CreateBy
            {
                get
                {
                    return _CreateBy;
                }
                set
                {
                    _CreateBy = value;
                }
            }
            public DateTime UpdateDate
            {
                get
                {
                    return _UpdateDate;
                }
                set
                {
                    _UpdateDate = value;
                }
            }
            public String UpdateBy
            {
                get
                {
                    return _UpdateBy;
                }
                set
                {
                    _UpdateBy = value;
                }
            }
            public Byte Active
            {
                get
                {
                    return _Active;
                }
                set
                {
                    _Active = value;
                }
            }
            #endregion
        }
        public DataTable GetAGENTACCESSFAREList(string FieldCond = "")
        {
            DataTable dt = new DataTable();
            String strSQL = string.Empty;
            try
            {

                strSQL = "SELECT MarketCode, InTier, InFareClass, OutTier, OutFareClass, Status, Flag, Inuse, Active, SyncCreate, SyncLastUpd, LastSyncBy, CreateDate, CreateBy, UpdateDate, UpdateBy ";
                strSQL += "FROM AD_AGENTACCESSFARE";

                using (var connection = new SqlConnection(_appConfiguration.GetConnectionString(PlexformConsts.GBSConnectionString)))
                {
                    connection.Open();
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
                        return null;
                        throw new ApplicationException("AGENTACCESSFARE does not exist.");
                    }
                }
                //return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetAGENTACCESSFAREPIVOT()
        {
            DataTable dt = new DataTable();
            String strSQL = string.Empty;
            try
            {

                strSQL = "SELECT Analyst, MarketCode [MarketCode] ";
                strSQL += ",OutRoute[OutRoute],[Out1] AS[OutTier1], [Out2] AS[OutTier2], [Out3] AS[OutTier3], [OutGeneric] AS [OutGeneric] ";
                strSQL += ",InRoute, [In1] AS[InTier1], [In2] AS[InTier2], [In3] AS[InTier3], [InGeneric] AS [InGeneric] ";
                strSQL += "FROM ";
                strSQL += "(SELECT Analyst, AAF.MarketCode, OutRoute,'Out'+OutTier[OutTier], OutFareClass, InRoute,'In'+InTier[InTier], InFareClass ";
                strSQL += "FROM AD_AGENTACCESSFARE AAF ";
                strSQL += "INNER JOIN AD_MARKET M ON AAF.MarketCode = M.MarketCode) p ";
                strSQL += "PIVOT ";
                strSQL += "( ";
                strSQL += "MAX(OUTFARECLASS) ";
                strSQL += "FOR OUTTIER IN ";
                strSQL += "( [Out1],[Out2],[Out3],[OutGeneric]) ";
                strSQL += ") AS pvt ";
                strSQL += "PIVOT ";
                strSQL += "( ";
                strSQL += "MAX(INFARECLASS) ";
                strSQL += "FOR INTIER IN ";
                strSQL += "( [In1],[In2],[In3],[InGeneric]) ";
                strSQL += ") AS pvt2";

                using (var connection = new SqlConnection(_appConfiguration.GetConnectionString(PlexformConsts.GBSConnectionString)))
                {
                    connection.Open();
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
                        return null;
                        throw new ApplicationException("AGENTACCESSFARE does not exist.");
                    }
                }
                //return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region AGENTTIER
        public class AgentTierInfo
        {
            private System.String _MarketCode = String.Empty;
            private System.String _InTier = String.Empty;
            private System.String _InSubTier = String.Empty;
            private System.String _OutTier = String.Empty;
            private System.String _OutSubTier = String.Empty;
            private System.String _InAgentID = String.Empty;
            private System.String _OutAgentID = String.Empty;
            private System.Guid _rowguid;
            private System.Byte _Status;
            private System.Byte _Inuse;
            private System.DateTime _SyncCreate;
            private System.DateTime _SyncLastUpd;
            private System.String _LastSyncBy = String.Empty;
            private System.DateTime _CreateDate;
            private System.String _CreateBy = String.Empty;
            private System.DateTime _UpdateDate;
            private System.String _UpdateBy = String.Empty;
            private System.Byte _Active;

            #region properties
            public System.String MarketCode
            {
                get
                {
                    return _MarketCode;
                }

                set
                {
                    _MarketCode = value;
                }
            }
            public System.String InTier
            {
                get
                {
                    return _InTier;
                }

                set
                {
                    _InTier = value;
                }
            }
            public System.String InSubTier
            {
                get
                {
                    return _InSubTier;
                }

                set
                {
                    _InSubTier = value;
                }
            }
            public System.String OutTier
            {
                get
                {
                    return _OutTier;
                }

                set
                {
                    _OutTier = value;
                }
            }
            public System.String OutSubTier
            {
                get
                {
                    return _OutSubTier;
                }

                set
                {
                    _OutSubTier = value;
                }
            }
            public System.String InAgentID
            {
                get
                {
                    return _InAgentID;
                }

                set
                {
                    _InAgentID = value;
                }
            }
            public System.String OutAgentID
            {
                get
                {
                    return _OutAgentID;
                }

                set
                {
                    _OutAgentID = value;
                }
            }
            public System.Guid rowguid
            {
                get
                {
                    return _rowguid;
                }

                set
                {
                    _rowguid = value;
                }
            }
            public System.Byte Status
            {
                get
                {
                    return _Status;
                }

                set
                {
                    _Status = value;
                }
            }
            public System.Byte Inuse
            {
                get
                {
                    return _Inuse;
                }

                set
                {
                    _Inuse = value;
                }
            }
            public System.DateTime SyncCreate
            {
                get
                {
                    return _SyncCreate;
                }

                set
                {
                    _SyncCreate = value;
                }
            }
            public System.DateTime SyncLastUpd
            {
                get
                {
                    return _SyncLastUpd;
                }

                set
                {
                    _SyncLastUpd = value;
                }
            }
            public System.String LastSyncBy
            {
                get
                {
                    return _LastSyncBy;
                }

                set
                {
                    _LastSyncBy = value;
                }
            }
            public System.DateTime CreateDate
            {
                get
                {
                    return _CreateDate;
                }

                set
                {
                    _CreateDate = value;
                }
            }
            public System.String CreateBy
            {
                get
                {
                    return _CreateBy;
                }

                set
                {
                    _CreateBy = value;
                }
            }
            public System.DateTime UpdateDate
            {
                get
                {
                    return _UpdateDate;
                }

                set
                {
                    _UpdateDate = value;
                }
            }
            public System.String UpdateBy
            {
                get
                {
                    return _UpdateBy;
                }

                set
                {
                    _UpdateBy = value;
                }
            }
            public System.Byte Active
            {
                get
                {
                    return _Active;
                }

                set
                {
                    _Active = value;
                }
            }
            #endregion
        }
        public DataTable GetAgentTierListGrid(string FieldCond = "")
        {
            DataTable dt = new DataTable();
            String strSQL = string.Empty;
            try
            {
                strSQL = "SELECT M.Analyst, AT.MarketCode, ";
                strSQL += "M.InRoute, AT.InTier, AT.InSubTier, AG.ContactFirstName + ' ' + AG.ContactLastName InAgent, AG.Email InAgentEmail, AG.AgentID InAgentID, ";
                strSQL += "M.OutRoute, AT.OutTier, AT.OutSubTier, AGT.ContactFirstName + ' ' + AGT.ContactLastName OutAgent, AGT.Email OutAgentEmail, AGT.AgentID OutAgentID, ";
                strSQL += "CAST(AT.CreateDate as date) CreateDate, CAST(AT.UpdateDate as date) UpdateDate ";
                strSQL += "FROM AD_AgentTier AT INNER JOIN AD_Market M ON AT.MarketCode = M.MarketCode ";
                strSQL += "LEFT JOIN AG_PROFILE AG ON AT.InAgentID = AG.AgentID ";
                strSQL += "LEFT JOIN AG_PROFILE AGT ON AT.OutAgentID = AGT.AgentID";

                using (var connection = new SqlConnection(_appConfiguration.GetConnectionString(PlexformConsts.GBSConnectionString)))
                {
                    connection.Open();
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
                        return null;
                        throw new ApplicationException("AD_AgentTier does not exist.");
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region MARKET
        public class MarketInfo
        {
            protected System.String _MarketCode = String.Empty;
            private System.String _MarketName = String.Empty;
            private System.String _Analyst = String.Empty;
            private System.String _InRoute = String.Empty;
            private System.String _OutRoute = String.Empty;
            private System.Decimal _InGrpCap;
            private System.Decimal _OutGrpCap;
            private System.Decimal _InMaxDisc;
            private System.Decimal _OutMaxDisc;
            private System.Guid _rowguid;
            private System.Byte _Status;
            private System.Byte _Inuse;
            private System.DateTime _SyncCreate;
            private System.DateTime _SyncLastUpd;
            private System.String _LastSyncBy;
            private System.DateTime _CreateDate;
            private System.String _CreateBy = String.Empty;
            private System.DateTime _UpdateDate;
            private System.String _UpdateBy = String.Empty;
            private System.Byte _Active;

            #region properties
            public System.String MarketCode
            {
                get
                {
                    return _MarketCode;
                }

                set
                {
                    _MarketCode = value;
                }
            }

            public System.String MarketName
            {
                get
                {
                    return _MarketName;
                }

                set
                {
                    _MarketName = value;
                }
            }

            public System.String Analyst
            {
                get
                {
                    return _Analyst;
                }

                set
                {
                    _Analyst = value;
                }
            }

            public System.String InRoute
            {
                get
                {
                    return _InRoute;
                }

                set
                {
                    _InRoute = value;
                }
            }

            public System.String OutRoute
            {
                get
                {
                    return _OutRoute;
                }

                set
                {
                    _OutRoute = value;
                }
            }

            public System.Decimal InGrpCap
            {
                get
                {
                    return _InGrpCap;
                }

                set
                {
                    _InGrpCap = value;
                }
            }

            public System.Decimal OutGrpCap
            {
                get
                {
                    return _OutGrpCap;
                }

                set
                {
                    _OutGrpCap = value;
                }
            }

            public System.Decimal InMaxDisc
            {
                get
                {
                    return _InMaxDisc;
                }

                set
                {
                    _InMaxDisc = value;
                }
            }

            public System.Decimal OutMaxDisc
            {
                get
                {
                    return _OutMaxDisc;
                }

                set
                {
                    _OutMaxDisc = value;
                }
            }

            public System.Guid rowguid
            {
                get
                {
                    return _rowguid;
                }

                set
                {
                    _rowguid = value;
                }
            }

            public System.Byte Status
            {
                get
                {
                    return _Status;
                }

                set
                {
                    _Status = value;
                }
            }

            public System.Byte Inuse
            {
                get
                {
                    return _Inuse;
                }

                set
                {
                    _Inuse = value;
                }
            }

            public System.DateTime SyncCreate
            {
                get
                {
                    return _SyncCreate;
                }

                set
                {
                    _SyncCreate = value;
                }
            }

            public System.DateTime SyncLastUpd
            {
                get
                {
                    return _SyncLastUpd;
                }

                set
                {
                    _SyncLastUpd = value;
                }
            }

            public System.String LastSyncBy
            {
                get
                {
                    return _LastSyncBy;
                }

                set
                {
                    _LastSyncBy = value;
                }
            }

            public System.DateTime CreateDate
            {
                get
                {
                    return _CreateDate;
                }

                set
                {
                    _CreateDate = value;
                }
            }

            public System.String CreateBy
            {
                get
                {
                    return _CreateBy;
                }

                set
                {
                    _CreateBy = value;
                }
            }

            public System.DateTime UpdateDate
            {
                get
                {
                    return _UpdateDate;
                }

                set
                {
                    _UpdateDate = value;
                }
            }

            public System.String UpdateBy
            {
                get
                {
                    return _UpdateBy;
                }

                set
                {
                    _UpdateBy = value;
                }
            }

            public System.Byte Active
            {
                get
                {
                    return _Active;
                }

                set
                {
                    _Active = value;
                }
            }
            #endregion
        }
        public DataTable GetGroupCap()
        {
            DataTable dt = new DataTable();
            String strSQL = string.Empty;
            try
            {

                strSQL = "SELECT Analyst, MarketCode, InRoute, InGrpCap, OutRoute, OutGrpCap FROM AD_MARKET";

                using (var connection = new SqlConnection(_appConfiguration.GetConnectionString(PlexformConsts.GBSConnectionString)))
                {
                    connection.Open();
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
                        return null;
                        throw new ApplicationException("AD_MARKET does not exist.");
                    }
                }
                //return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetMaxDisc()
        {
            DataTable dt = new DataTable();
            String strSQL = string.Empty;
            try
            {

                strSQL = "SELECT Analyst, MarketCode, InRoute, InMaxDisc, OutRoute, OutMaxDisc FROM AD_MARKET";

                using (var connection = new SqlConnection(_appConfiguration.GetConnectionString(PlexformConsts.GBSConnectionString)))
                {
                    connection.Open();
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
                        return null;
                        throw new ApplicationException("AD_MARKET does not exist.");
                    }
                }
                //return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetMarket()
        {
            DataTable dt = new DataTable();
            String strSQL = string.Empty;
            try
            {

                strSQL = "SELECT MarketCode, MarketName, Analyst, InRoute, OutRoute FROM AD_MARKET";

                using (var connection = new SqlConnection(_appConfiguration.GetConnectionString(PlexformConsts.GBSConnectionString)))
                {
                    connection.Open();
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
                        return null;
                        throw new ApplicationException("AD_MARKET does not exist.");
                    }
                }
                //return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region SEASONALITY
        public class SeasonalityInfo
        {
            private System.String _SeaCode = String.Empty;
            private System.String _Analyst = String.Empty;
            private System.String _RouteCode = String.Empty;
            private System.DateTime _SeasonDate;
            private System.String _Season = String.Empty;
            private System.Guid _rowguid;
            private System.Byte _Status;
            private System.Byte _Flag;
            private System.Byte _Inuse;
            private System.DateTime _SyncCreate;
            private System.DateTime _SyncLastUpd;
            private System.String _LastSyncBy = String.Empty;
            private System.DateTime _CreateDate;
            private System.String _CreateBy = String.Empty;
            private System.DateTime _UpdateDate;
            private System.String _UpdateBy = String.Empty;
            private System.Byte _Active;

            #region properties
            public System.String SeaCode
            {
                get
                {
                    return _SeaCode;
                }

                set
                {
                    _SeaCode = value;
                }
            }

            public System.String Analyst
            {
                get
                {
                    return _Analyst;
                }

                set
                {
                    _Analyst = value;
                }
            }

            public System.String RouteCode
            {
                get
                {
                    return _RouteCode;
                }

                set
                {
                    _RouteCode = value;
                }
            }

            public System.DateTime SeasonDate
            {
                get
                {
                    return _SeasonDate;
                }

                set
                {
                    _SeasonDate = value;
                }
            }

            public System.String Season
            {
                get
                {
                    return _Season;
                }

                set
                {
                    _Season = value;
                }
            }

            public System.Guid rowguid
            {
                get
                {
                    return _rowguid;
                }

                set
                {
                    _rowguid = value;
                }
            }

            public System.Byte Status
            {
                get
                {
                    return _Status;
                }

                set
                {
                    _Status = value;
                }
            }

            public System.Byte Flag
            {
                get
                {
                    return _Flag;
                }

                set
                {
                    _Flag = value;
                }
            }

            public System.Byte Inuse
            {
                get
                {
                    return _Inuse;
                }

                set
                {
                    _Inuse = value;
                }
            }

            public System.DateTime SyncCreate
            {
                get
                {
                    return _SyncCreate;
                }

                set
                {
                    _SyncCreate = value;
                }
            }

            public System.DateTime SyncLastUpd
            {
                get
                {
                    return _SyncLastUpd;
                }

                set
                {
                    _SyncLastUpd = value;
                }
            }

            public System.String LastSyncBy
            {
                get
                {
                    return _LastSyncBy;
                }

                set
                {
                    _LastSyncBy = value;
                }
            }

            public System.DateTime CreateDate
            {
                get
                {
                    return _CreateDate;
                }

                set
                {
                    _CreateDate = value;
                }
            }

            public System.String CreateBy
            {
                get
                {
                    return _CreateBy;
                }

                set
                {
                    _CreateBy = value;
                }
            }

            public System.DateTime UpdateDate
            {
                get
                {
                    return _UpdateDate;
                }

                set
                {
                    _UpdateDate = value;
                }
            }

            public System.String UpdateBy
            {
                get
                {
                    return _UpdateBy;
                }

                set
                {
                    _UpdateBy = value;
                }
            }

            public System.Byte Active
            {
                get
                {
                    return _Active;
                }

                set
                {
                    _Active = value;
                }
            }
            #endregion
        }
        public DataTable GetSeasonality()
        {
            DataTable dt = new DataTable();
            String strSQL = string.Empty;
            try
            {

                strSQL = "SELECT Analyst, RouteCode, CAST(SeasonDate as date) SeasonDate, Season FROM AD_SEASONALITY";

                using (var connection = new SqlConnection(_appConfiguration.GetConnectionString(PlexformConsts.GBSConnectionString)))
                {
                    connection.Open();
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
                        return null;
                        throw new ApplicationException("AD_SEASONALITY does not exist.");
                    }
                }
                //return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region DISCWEIGHTAGE
        public class DISCWEIGHTAGEInfo
        {
            protected System.String _MarketCode = String.Empty;
            private System.Decimal _InWALFDisc;
            private System.Decimal _InWAPUDisc;
            private System.Decimal _OutWALFDisc;
            private System.Decimal _OutWAPUDisc;
            private System.Guid _rowguid;
            private System.Byte _Status;
            private System.Byte _Inuse;
            private System.DateTime _SyncCreate;
            private System.DateTime _SyncLastUpd;
            private System.String _LastSyncBy = String.Empty;
            private System.DateTime _CreateDate;
            private System.String _CreateBy = String.Empty;
            private System.DateTime _UpdateDate;
            private System.String _UpdateBy;
            private System.Byte _Active;

            #region  properties
            public System.String MarketCode
            {
                get
                {
                    return _MarketCode;
                }

                set
                {
                    _MarketCode = value;
                }
            }

            public System.Decimal InWALFDisc
            {
                get
                {
                    return _InWALFDisc;
                }

                set
                {
                    _InWALFDisc = value;
                }
            }

            public System.Decimal InWAPUDisc
            {
                get
                {
                    return _InWAPUDisc;
                }

                set
                {
                    _InWAPUDisc = value;
                }
            }

            public System.Decimal OutWALFDisc
            {
                get
                {
                    return _OutWALFDisc;
                }

                set
                {
                    _OutWALFDisc = value;
                }
            }

            public System.Decimal OutWAPUDisc
            {
                get
                {
                    return _OutWAPUDisc;
                }

                set
                {
                    _OutWAPUDisc = value;
                }
            }

            public System.Guid rowguid
            {
                get
                {
                    return _rowguid;
                }

                set
                {
                    _rowguid = value;
                }
            }

            public System.Byte Status
            {
                get
                {
                    return _Status;
                }

                set
                {
                    _Status = value;
                }
            }

            public System.Byte Inuse
            {
                get
                {
                    return _Inuse;
                }

                set
                {
                    _Inuse = value;
                }
            }

            public System.DateTime SyncCreate
            {
                get
                {
                    return _SyncCreate;
                }

                set
                {
                    _SyncCreate = value;
                }
            }

            public System.DateTime SyncLastUpd
            {
                get
                {
                    return _SyncLastUpd;
                }

                set
                {
                    _SyncLastUpd = value;
                }
            }

            public System.String LastSyncBy
            {
                get
                {
                    return _LastSyncBy;
                }

                set
                {
                    _LastSyncBy = value;
                }
            }

            public System.DateTime CreateDate
            {
                get
                {
                    return _CreateDate;
                }

                set
                {
                    _CreateDate = value;
                }
            }

            public System.String CreateBy
            {
                get
                {
                    return _CreateBy;
                }

                set
                {
                    _CreateBy = value;
                }
            }

            public System.DateTime UpdateDate
            {
                get
                {
                    return _UpdateDate;
                }

                set
                {
                    _UpdateDate = value;
                }
            }

            public System.String UpdateBy
            {
                get
                {
                    return _UpdateBy;
                }

                set
                {
                    _UpdateBy = value;
                }
            }

            public System.Byte Active
            {
                get
                {
                    return _Active;
                }

                set
                {
                    _Active = value;
                }
            }
            #endregion
        }
        public DataTable GetDiscWeight()
        {
            DataTable dt = new DataTable();
            String strSQL = string.Empty;
            try
            {

                strSQL = "SELECT M.Analyst, DW.MarketCode, M.InRoute, DW.InWALFDisc, DW.InWAPUDisc, DW.OutWALFDisc, DW.OutWAPUDisc, M.OutRoute FROM AD_DISCWEIGHTAGE DW ";
                strSQL += "INNER JOIN AD_Market M ON DW.MarketCode = M.MarketCode";

                using (var connection = new SqlConnection(_appConfiguration.GetConnectionString(PlexformConsts.GBSConnectionString)))
                {
                    connection.Open();
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
                        return null;
                        throw new ApplicationException("AD_DISCWEIGHTAGE does not exist.");
                    }
                }
                //return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region FLOORFARE
        public class FloorFareInfo
        {
            protected System.String _MarketCode = String.Empty;
            protected System.String _InCurrency = String.Empty;
            protected System.String _OutCurrency = String.Empty;
            private System.Decimal _InDisc;
            private System.String _InFareClass = String.Empty;
            private System.Decimal _InFloorFare;
            private System.Decimal _OutDisc;
            private System.String _OutFareClass = String.Empty;
            private System.Decimal _OutFloorFare;
            private System.Guid _rowguid;
            private System.Byte _Status;
            private System.Byte _Inuse;
            private System.DateTime _SyncCreate;
            private System.DateTime _SyncLastUpd;
            private System.String _LastSyncBy = String.Empty;
            private System.DateTime _CreateDate;
            private System.String _CreateBy;
            private System.DateTime _UpdateDate;
            private System.String _UpdateBy = String.Empty;
            private System.Byte _Active;

            #region Properties
            public System.String MarketCode
            {
                get
                {
                    return _MarketCode;
                }

                set
                {
                    _MarketCode = value;
                }
            }

            public System.String InCurrency
            {
                get
                {
                    return _InCurrency;
                }

                set
                {
                    _InCurrency = value;
                }
            }

            public System.String OutCurrency
            {
                get
                {
                    return _OutCurrency;
                }

                set
                {
                    _OutCurrency = value;
                }
            }

            public System.Decimal InDisc
            {
                get
                {
                    return _InDisc;
                }

                set
                {
                    _InDisc = value;
                }
            }

            public System.String InFareClass
            {
                get
                {
                    return _InFareClass;
                }

                set
                {
                    _InFareClass = value;
                }
            }

            public System.Decimal InFloorFare
            {
                get
                {
                    return _InFloorFare;
                }

                set
                {
                    _InFloorFare = value;
                }
            }

            public System.Decimal OutDisc
            {
                get
                {
                    return _OutDisc;
                }

                set
                {
                    _OutDisc = value;
                }
            }

            public System.String OutFareClass
            {
                get
                {
                    return _OutFareClass;
                }

                set
                {
                    _OutFareClass = value;
                }
            }

            public System.Decimal OutFloorFare
            {
                get
                {
                    return _OutFloorFare;
                }

                set
                {
                    _OutFloorFare = value;
                }
            }

            public System.Guid rowguid
            {
                get
                {
                    return _rowguid;
                }

                set
                {
                    _rowguid = value;
                }
            }

            public System.Byte Status
            {
                get
                {
                    return _Status;
                }

                set
                {
                    _Status = value;
                }
            }

            public System.Byte Inuse
            {
                get
                {
                    return _Inuse;
                }

                set
                {
                    _Inuse = value;
                }
            }

            public System.DateTime SyncCreate
            {
                get
                {
                    return _SyncCreate;
                }

                set
                {
                    _SyncCreate = value;
                }
            }

            public System.DateTime SyncLastUpd
            {
                get
                {
                    return _SyncLastUpd;
                }

                set
                {
                    _SyncLastUpd = value;
                }
            }

            public System.String LastSyncBy
            {
                get
                {
                    return _LastSyncBy;
                }

                set
                {
                    _LastSyncBy = value;
                }
            }

            public System.DateTime CreateDate
            {
                get
                {
                    return _CreateDate;
                }

                set
                {
                    _CreateDate = value;
                }
            }

            public System.String CreateBy
            {
                get
                {
                    return _CreateBy;
                }

                set
                {
                    _CreateBy = value;
                }
            }

            public System.DateTime UpdateDate
            {
                get
                {
                    return _UpdateDate;
                }

                set
                {
                    _UpdateDate = value;
                }
            }

            public System.String UpdateBy
            {
                get
                {
                    return _UpdateBy;
                }

                set
                {
                    _UpdateBy = value;
                }
            }

            public System.Byte Active
            {
                get
                {
                    return _Active;
                }

                set
                {
                    _Active = value;
                }
            }
            #endregion
        }
        public DataTable GetFloorFare()
        {
            DataTable dt = new DataTable();
            String strSQL = string.Empty;
            try
            {

                strSQL = "SELECT M.Analyst, FF.MarketCode, M.InRoute, FF.InCurrency, FF.InDisc, FF.InFareClass, FF.InFloorFare, M.OutRoute, FF.OutCurrency, FF.OutDisc, FF.OutFareClass, FF.OutFloorFare FROM AD_FLOORFARE FF ";
                strSQL += "INNER JOIN AD_Market M ON FF.MarketCode = M.MarketCode";

                using (var connection = new SqlConnection(_appConfiguration.GetConnectionString(PlexformConsts.GBSConnectionString)))
                {
                    connection.Open();
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
                        return null;
                        throw new ApplicationException("AD_DISCWEIGHTAGE does not exist.");
                    }
                }
                //return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region LFDISCOUNT
        public class LFDiscountInfo
        {
            protected System.String _MarketCode = String.Empty;
            protected System.String _NDO = String.Empty;
            private System.Decimal _InLFDisc1;
            private System.Decimal _InLFDisc2;
            private System.Decimal _InLFDisc3;
            private System.Decimal _InLFDisc4;
            private System.Decimal _InLFDisc5;
            private System.Decimal _InLFDisc6;
            private System.Decimal _InLFDisc7;
            private System.Decimal _InLFDisc8;
            private System.Decimal _InLFDisc9;
            private System.Decimal _InLFDisc10;
            private System.Decimal _OutLFDisc1;
            private System.Decimal _OutLFDisc2;
            private System.Decimal _OutLFDisc3;
            private System.Decimal _OutLFDisc4;
            private System.Decimal _OutLFDisc5;
            private System.Decimal _OutLFDisc6;
            private System.Decimal _OutLFDisc7;
            private System.Decimal _OutLFDisc8;
            private System.Decimal _OutLFDisc9;
            private System.Decimal _OutLFDisc10;
            private System.Guid _rowguid;
            private System.Byte _Status;
            private System.Byte _Inuse;
            private System.DateTime _SyncCreate;
            private System.DateTime _SyncLastUpd;
            private System.String _LastSyncBy = String.Empty;
            private System.DateTime _CreateDate;
            private System.String _CreateBy = String.Empty;
            private System.DateTime _UpdateDate;
            private System.String _UpdateBy;
            private System.Byte _Active;

            #region properties
            public System.String MarketCode
            {
                get
                {
                    return _MarketCode;
                }

                set
                {
                    _MarketCode = value;
                }
            }

            public System.String NDO
            {
                get
                {
                    return _NDO;
                }

                set
                {
                    _NDO = value;
                }
            }

            public System.Decimal InLFDisc1
            {
                get
                {
                    return _InLFDisc1;
                }

                set
                {
                    _InLFDisc1 = value;
                }
            }

            public System.Decimal InLFDisc2
            {
                get
                {
                    return _InLFDisc2;
                }

                set
                {
                    _InLFDisc2 = value;
                }
            }

            public System.Decimal InLFDisc3
            {
                get
                {
                    return _InLFDisc3;
                }

                set
                {
                    _InLFDisc3 = value;
                }
            }

            public System.Decimal InLFDisc4
            {
                get
                {
                    return _InLFDisc4;
                }

                set
                {
                    _InLFDisc4 = value;
                }
            }

            public System.Decimal InLFDisc5
            {
                get
                {
                    return _InLFDisc5;
                }

                set
                {
                    _InLFDisc5 = value;
                }
            }

            public System.Decimal InLFDisc6
            {
                get
                {
                    return _InLFDisc6;
                }

                set
                {
                    _InLFDisc6 = value;
                }
            }

            public System.Decimal InLFDisc7
            {
                get
                {
                    return _InLFDisc7;
                }

                set
                {
                    _InLFDisc7 = value;
                }
            }

            public System.Decimal InLFDisc8
            {
                get
                {
                    return _InLFDisc8;
                }

                set
                {
                    _InLFDisc8 = value;
                }
            }

            public System.Decimal InLFDisc9
            {
                get
                {
                    return _InLFDisc9;
                }

                set
                {
                    _InLFDisc9 = value;
                }
            }

            public System.Decimal InLFDisc10
            {
                get
                {
                    return _InLFDisc10;
                }

                set
                {
                    _InLFDisc10 = value;
                }
            }

            public System.Decimal OutLFDisc1
            {
                get
                {
                    return _OutLFDisc1;
                }

                set
                {
                    _OutLFDisc1 = value;
                }
            }

            public System.Decimal OutLFDisc2
            {
                get
                {
                    return _OutLFDisc2;
                }

                set
                {
                    _OutLFDisc2 = value;
                }
            }

            public System.Decimal OutLFDisc3
            {
                get
                {
                    return _OutLFDisc3;
                }

                set
                {
                    _OutLFDisc3 = value;
                }
            }

            public System.Decimal OutLFDisc4
            {
                get
                {
                    return _OutLFDisc4;
                }

                set
                {
                    _OutLFDisc4 = value;
                }
            }

            public System.Decimal OutLFDisc5
            {
                get
                {
                    return _OutLFDisc5;
                }

                set
                {
                    _OutLFDisc5 = value;
                }
            }

            public System.Decimal OutLFDisc6
            {
                get
                {
                    return _OutLFDisc6;
                }

                set
                {
                    _OutLFDisc6 = value;
                }
            }

            public System.Decimal OutLFDisc7
            {
                get
                {
                    return _OutLFDisc7;
                }

                set
                {
                    _OutLFDisc7 = value;
                }
            }

            public System.Decimal OutLFDisc8
            {
                get
                {
                    return _OutLFDisc8;
                }

                set
                {
                    _OutLFDisc8 = value;
                }
            }

            public System.Decimal OutLFDisc9
            {
                get
                {
                    return _OutLFDisc9;
                }

                set
                {
                    _OutLFDisc9 = value;
                }
            }

            public System.Decimal OutLFDisc10
            {
                get
                {
                    return _OutLFDisc10;
                }

                set
                {
                    _OutLFDisc10 = value;
                }
            }

            public System.Guid rowguid
            {
                get
                {
                    return _rowguid;
                }

                set
                {
                    _rowguid = value;
                }
            }

            public System.Byte Status
            {
                get
                {
                    return _Status;
                }

                set
                {
                    _Status = value;
                }
            }

            public System.Byte Inuse
            {
                get
                {
                    return _Inuse;
                }

                set
                {
                    _Inuse = value;
                }
            }

            public System.DateTime SyncCreate
            {
                get
                {
                    return _SyncCreate;
                }

                set
                {
                    _SyncCreate = value;
                }
            }

            public System.DateTime SyncLastUpd
            {
                get
                {
                    return _SyncLastUpd;
                }

                set
                {
                    _SyncLastUpd = value;
                }
            }

            public System.String LastSyncBy
            {
                get
                {
                    return _LastSyncBy;
                }

                set
                {
                    _LastSyncBy = value;
                }
            }

            public System.DateTime CreateDate
            {
                get
                {
                    return _CreateDate;
                }

                set
                {
                    _CreateDate = value;
                }
            }

            public System.String CreateBy
            {
                get
                {
                    return _CreateBy;
                }

                set
                {
                    _CreateBy = value;
                }
            }

            public System.DateTime UpdateDate
            {
                get
                {
                    return _UpdateDate;
                }

                set
                {
                    _UpdateDate = value;
                }
            }

            public System.String UpdateBy
            {
                get
                {
                    return _UpdateBy;
                }

                set
                {
                    _UpdateBy = value;
                }
            }

            public System.Byte Active
            {
                get
                {
                    return _Active;
                }

                set
                {
                    _Active = value;
                }
            }
            #endregion
        }
        public DataTable GetLFDiscount()
        {
            DataTable dt = new DataTable();
            String strSQL = string.Empty;
            try
            {

                strSQL = "SELECT M.Analyst, LD.MarketCode, N.NDODesc, M.InRoute, LD.InLFDisc1, LD.InLFDisc2, LD.InLFDisc3, LD.InLFDisc4, ";
                strSQL += "LD.InLFDisc5, LD.InLFDisc6, LD.InLFDisc7, LD.InLFDisc8, LD.InLFDisc9, LD.InLFDisc10, M.OutRoute, LD.OutLFDisc1, ";
                strSQL += "LD.OutLFDisc2, LD.OutLFDisc3, LD.OutLFDisc4, LD.OutLFDisc5, LD.OutLFDisc6, LD.OutLFDisc7, LD.OutLFDisc8, LD.OutLFDisc9, ";
                strSQL += "LD.OutLFDisc10 FROM AD_LFDISCOUNT LD INNER JOIN AD_MARKET M ON LD.MarketCode = M.MarketCode INNER JOIN AD_NDO N ON LD.NDO = N.NDOCode";

                using (var connection = new SqlConnection(_appConfiguration.GetConnectionString(PlexformConsts.GBSConnectionString)))
                {
                    connection.Open();
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
                        return null;
                        throw new ApplicationException("AD_LFDISCOUNT does not exist.");
                    }
                }
                //return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region PUDISCOUNT
        public class PUDiscountInfo
        {
            protected System.String _MarketCode = String.Empty;
            protected System.String _NDO = String.Empty;
            private System.Decimal _InPUDisc1;
            private System.Decimal _InPUDisc2;
            private System.Decimal _InPUDisc3;
            private System.Decimal _InPUDisc4;
            private System.Decimal _InPUDisc5;
            private System.Decimal _InPUDisc6;
            private System.Decimal _InPUDisc7;
            private System.Decimal _InPUDisc8;
            private System.Decimal _InPUDisc9;
            private System.Decimal _InPUDisc10;
            private System.Decimal _InPUDisc11;
            private System.Decimal _OutPUDisc1;
            private System.Decimal _OutPUDisc2;
            private System.Decimal _OutPUDisc3;
            private System.Decimal _OutPUDisc4;
            private System.Decimal _OutPUDisc5;
            private System.Decimal _OutPUDisc6;
            private System.Decimal _OutPUDisc7;
            private System.Decimal _OutPUDisc8;
            private System.Decimal _OutPUDisc9;
            private System.Decimal _OutPUDisc10;
            private System.Decimal _OutPUDisc11;
            private System.Guid _rowguid;
            private System.Byte _Status;
            private System.Byte _Inuse;
            private System.DateTime _SyncCreate;
            private System.DateTime _SyncLastUpd;
            private System.String _LastSyncBy = String.Empty;
            private System.DateTime _CreateDate;
            private System.String _CreateBy = String.Empty;
            private System.DateTime _UpdateDate;
            private System.String _UpdateBy = String.Empty;
            private System.Byte _Active;

            #region properties
            public System.String MarketCode
            {
                get
                {
                    return _MarketCode;
                }

                set
                {
                    _MarketCode = value;
                }
            }

            public System.String NDO
            {
                get
                {
                    return _NDO;
                }

                set
                {
                    _NDO = value;
                }
            }

            public System.Decimal InPUDisc1
            {
                get
                {
                    return _InPUDisc1;
                }

                set
                {
                    _InPUDisc1 = value;
                }
            }

            public System.Decimal InPUDisc2
            {
                get
                {
                    return _InPUDisc2;
                }

                set
                {
                    _InPUDisc2 = value;
                }
            }

            public System.Decimal InPUDisc3
            {
                get
                {
                    return _InPUDisc3;
                }

                set
                {
                    _InPUDisc3 = value;
                }
            }

            public System.Decimal InPUDisc4
            {
                get
                {
                    return _InPUDisc4;
                }

                set
                {
                    _InPUDisc4 = value;
                }
            }

            public System.Decimal InPUDisc5
            {
                get
                {
                    return _InPUDisc5;
                }

                set
                {
                    _InPUDisc5 = value;
                }
            }

            public System.Decimal InPUDisc6
            {
                get
                {
                    return _InPUDisc6;
                }

                set
                {
                    _InPUDisc6 = value;
                }
            }

            public System.Decimal InPUDisc7
            {
                get
                {
                    return _InPUDisc7;
                }

                set
                {
                    _InPUDisc7 = value;
                }
            }

            public System.Decimal InPUDisc8
            {
                get
                {
                    return _InPUDisc8;
                }

                set
                {
                    _InPUDisc8 = value;
                }
            }

            public System.Decimal InPUDisc9
            {
                get
                {
                    return _InPUDisc9;
                }

                set
                {
                    _InPUDisc9 = value;
                }
            }

            public System.Decimal InPUDisc10
            {
                get
                {
                    return _InPUDisc10;
                }

                set
                {
                    _InPUDisc10 = value;
                }
            }

            public System.Decimal InPUDisc11
            {
                get
                {
                    return _InPUDisc11;
                }

                set
                {
                    _InPUDisc11 = value;
                }
            }

            public System.Decimal OutPUDisc1
            {
                get
                {
                    return _OutPUDisc1;
                }

                set
                {
                    _OutPUDisc1 = value;
                }
            }

            public System.Decimal OutPUDisc2
            {
                get
                {
                    return _OutPUDisc2;
                }

                set
                {
                    _OutPUDisc2 = value;
                }
            }

            public System.Decimal OutPUDisc3
            {
                get
                {
                    return _OutPUDisc3;
                }

                set
                {
                    _OutPUDisc3 = value;
                }
            }

            public System.Decimal OutPUDisc4
            {
                get
                {
                    return _OutPUDisc4;
                }

                set
                {
                    _OutPUDisc4 = value;
                }
            }

            public System.Decimal OutPUDisc5
            {
                get
                {
                    return _OutPUDisc5;
                }

                set
                {
                    _OutPUDisc5 = value;
                }
            }

            public System.Decimal OutPUDisc6
            {
                get
                {
                    return _OutPUDisc6;
                }

                set
                {
                    _OutPUDisc6 = value;
                }
            }

            public System.Decimal OutPUDisc7
            {
                get
                {
                    return _OutPUDisc7;
                }

                set
                {
                    _OutPUDisc7 = value;
                }
            }

            public System.Decimal OutPUDisc8
            {
                get
                {
                    return _OutPUDisc8;
                }

                set
                {
                    _OutPUDisc8 = value;
                }
            }

            public System.Decimal OutPUDisc9
            {
                get
                {
                    return _OutPUDisc9;
                }

                set
                {
                    _OutPUDisc9 = value;
                }
            }

            public System.Decimal OutPUDisc10
            {
                get
                {
                    return _OutPUDisc10;
                }

                set
                {
                    _OutPUDisc10 = value;
                }
            }

            public System.Decimal OutPUDisc11
            {
                get
                {
                    return _OutPUDisc11;
                }

                set
                {
                    _OutPUDisc11 = value;
                }
            }

            public System.Guid rowguid
            {
                get
                {
                    return _rowguid;
                }

                set
                {
                    _rowguid = value;
                }
            }

            public System.Byte Status
            {
                get
                {
                    return _Status;
                }

                set
                {
                    _Status = value;
                }
            }

            public System.Byte Inuse
            {
                get
                {
                    return _Inuse;
                }

                set
                {
                    _Inuse = value;
                }
            }

            public System.DateTime SyncCreate
            {
                get
                {
                    return _SyncCreate;
                }

                set
                {
                    _SyncCreate = value;
                }
            }

            public System.DateTime SyncLastUpd
            {
                get
                {
                    return _SyncLastUpd;
                }

                set
                {
                    _SyncLastUpd = value;
                }
            }

            public System.String LastSyncBy
            {
                get
                {
                    return _LastSyncBy;
                }

                set
                {
                    _LastSyncBy = value;
                }
            }

            public System.DateTime CreateDate
            {
                get
                {
                    return _CreateDate;
                }

                set
                {
                    _CreateDate = value;
                }
            }

            public System.String CreateBy
            {
                get
                {
                    return _CreateBy;
                }

                set
                {
                    _CreateBy = value;
                }
            }

            public System.DateTime UpdateDate
            {
                get
                {
                    return _UpdateDate;
                }

                set
                {
                    _UpdateDate = value;
                }
            }

            public System.String UpdateBy
            {
                get
                {
                    return _UpdateBy;
                }

                set
                {
                    _UpdateBy = value;
                }
            }

            public System.Byte Active
            {
                get
                {
                    return _Active;
                }

                set
                {
                    _Active = value;
                }
            }
            #endregion
        }
        public DataTable GetPUDiscount()
        {
            DataTable dt = new DataTable();
            String strSQL = string.Empty;
            try
            {

                strSQL = "SELECT M.Analyst, LD.MarketCode, N.NDODesc, M.InRoute, LD.InPUDisc1, LD.InPUDisc2, LD.InPUDisc3, LD.InPUDisc4, ";
                strSQL += "LD.InPUDisc5, LD.InPUDisc6, LD.InPUDisc7, LD.InPUDisc8, LD.InPUDisc9, LD.InPUDisc10, LD.InPUDisc11, M.OutRoute, ";
                strSQL += "LD.OutPUDisc1, LD.OutPUDisc2, LD.OutPUDisc3, LD.OutPUDisc4, LD.OutPUDisc5, LD.OutPUDisc6, LD.OutPUDisc7, LD.OutPUDisc8, ";
                strSQL += "LD.OutPUDisc9, LD.OutPUDisc10, LD.OutPUDisc11 FROM AD_PUDISCOUNT LD INNER JOIN AD_MARKET M ON LD.MarketCode = M.MarketCode ";
                strSQL += "INNER JOIN AD_NDO N ON LD.NDO = N.NDOCode";

                using (var connection = new SqlConnection(_appConfiguration.GetConnectionString(PlexformConsts.GBSConnectionString)))
                {
                    connection.Open();
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
                        return null;
                        throw new ApplicationException("AD_PUDISCOUNT does not exist.");
                    }
                }
                //return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region SPECIALFARE
        public class SpecialFareInfo
        {
            protected System.String _MarketCode = String.Empty;
            protected System.String _NDO = String.Empty;
            protected System.String _SpecCode = String.Empty;
            protected System.String _InDiscCrtCode = String.Empty;
            protected System.String _InFlightTimeGrp = String.Empty;
            protected System.String _InDepDOW = String.Empty;
            protected System.String _InAgentTier = String.Empty;
            protected System.String _InCurrency = String.Empty;
            protected System.String _OutDiscCrtCode = String.Empty;
            protected System.String _OutFlightTimeGrp = String.Empty;
            protected System.String _OutDepDOW = String.Empty;
            protected System.String _OutAgentTier = String.Empty;
            protected System.String _OutCurrency = String.Empty;
            private System.Decimal _InLFFare1;
            private System.Decimal _InLFFare2;
            private System.Decimal _InLFFare3;
            private System.Decimal _InLFFare4;
            private System.Decimal _InLFFare5;
            private System.Decimal _InLFFare6;
            private System.Decimal _InLFFare7;
            private System.Decimal _InLFFare8;
            private System.Decimal _InLFFare9;
            private System.Decimal _InLFFare10;
            private System.Decimal _InLFFare11;
            private System.Decimal _OutLFFare1;
            private System.Decimal _OutLFFare2;
            private System.Decimal _OutLFFare3;
            private System.Decimal _OutLFFare4;
            private System.Decimal _OutLFFare5;
            private System.Decimal _OutLFFare6;
            private System.Decimal _OutLFFare7;
            private System.Decimal _OutLFFare8;
            private System.Decimal _OutLFFare9;
            private System.Decimal _OutLFFare10;
            private System.Decimal _OutLFFare11;
            private System.Guid _rowguid;
            private System.Byte _Status;
            private System.Byte _Inuse;
            private System.DateTime _SyncCreate;
            private System.DateTime _SyncLastUpd;
            private System.String _LastSyncBy = String.Empty;
            private System.DateTime _CreateDate;
            private System.String _CreateBy = String.Empty;
            private System.DateTime _UpdateDate;
            private System.String _UpdateBy = String.Empty;
            private System.Byte _Active;

            #region properties
            public System.String MarketCode
            {
                get
                {
                    return _MarketCode;
                }

                set
                {
                    _MarketCode = value;
                }
            }

            public System.String NDO
            {
                get
                {
                    return _NDO;
                }

                set
                {
                    _NDO = value;
                }
            }

            public System.String SpecCode
            {
                get
                {
                    return _SpecCode;
                }

                set
                {
                    _SpecCode = value;
                }
            }

            public System.String InDiscCrtCode
            {
                get
                {
                    return _InDiscCrtCode;
                }

                set
                {
                    _InDiscCrtCode = value;
                }
            }

            public System.String InFlightTimeGrp
            {
                get
                {
                    return _InFlightTimeGrp;
                }

                set
                {
                    _InFlightTimeGrp = value;
                }
            }

            public System.String InDepDOW
            {
                get
                {
                    return _InDepDOW;
                }

                set
                {
                    _InDepDOW = value;
                }
            }

            public System.String InAgentTier
            {
                get
                {
                    return _InAgentTier;
                }

                set
                {
                    _InAgentTier = value;
                }
            }

            public System.String InCurrency
            {
                get
                {
                    return _InCurrency;
                }

                set
                {
                    _InCurrency = value;
                }
            }

            public System.String OutDiscCrtCode
            {
                get
                {
                    return _OutDiscCrtCode;
                }

                set
                {
                    _OutDiscCrtCode = value;
                }
            }

            public System.String OutFlightTimeGrp
            {
                get
                {
                    return _OutFlightTimeGrp;
                }

                set
                {
                    _OutFlightTimeGrp = value;
                }
            }

            public System.String OutDepDOW
            {
                get
                {
                    return _OutDepDOW;
                }

                set
                {
                    _OutDepDOW = value;
                }
            }

            public System.String OutAgentTier
            {
                get
                {
                    return _OutAgentTier;
                }

                set
                {
                    _OutAgentTier = value;
                }
            }

            public System.String OutCurrency
            {
                get
                {
                    return _OutCurrency;
                }

                set
                {
                    _OutCurrency = value;
                }
            }

            public System.Decimal InLFFare1
            {
                get
                {
                    return _InLFFare1;
                }

                set
                {
                    _InLFFare1 = value;
                }
            }

            public System.Decimal InLFFare2
            {
                get
                {
                    return _InLFFare2;
                }

                set
                {
                    _InLFFare2 = value;
                }
            }

            public System.Decimal InLFFare3
            {
                get
                {
                    return _InLFFare3;
                }

                set
                {
                    _InLFFare3 = value;
                }
            }

            public System.Decimal InLFFare4
            {
                get
                {
                    return _InLFFare4;
                }

                set
                {
                    _InLFFare4 = value;
                }
            }

            public System.Decimal InLFFare5
            {
                get
                {
                    return _InLFFare5;
                }

                set
                {
                    _InLFFare5 = value;
                }
            }

            public System.Decimal InLFFare6
            {
                get
                {
                    return _InLFFare6;
                }

                set
                {
                    _InLFFare6 = value;
                }
            }

            public System.Decimal InLFFare7
            {
                get
                {
                    return _InLFFare7;
                }

                set
                {
                    _InLFFare7 = value;
                }
            }

            public System.Decimal InLFFare8
            {
                get
                {
                    return _InLFFare8;
                }

                set
                {
                    _InLFFare8 = value;
                }
            }

            public System.Decimal InLFFare9
            {
                get
                {
                    return _InLFFare9;
                }

                set
                {
                    _InLFFare9 = value;
                }
            }

            public System.Decimal InLFFare10
            {
                get
                {
                    return _InLFFare10;
                }

                set
                {
                    _InLFFare10 = value;
                }
            }

            public System.Decimal InLFFare11
            {
                get
                {
                    return _InLFFare11;
                }

                set
                {
                    _InLFFare11 = value;
                }
            }

            public System.Decimal OutLFFare1
            {
                get
                {
                    return _OutLFFare1;
                }

                set
                {
                    _OutLFFare1 = value;
                }
            }

            public System.Decimal OutLFFare2
            {
                get
                {
                    return _OutLFFare2;
                }

                set
                {
                    _OutLFFare2 = value;
                }
            }

            public System.Decimal OutLFFare3
            {
                get
                {
                    return _OutLFFare3;
                }

                set
                {
                    _OutLFFare3 = value;
                }
            }

            public System.Decimal OutLFFare4
            {
                get
                {
                    return _OutLFFare4;
                }

                set
                {
                    _OutLFFare4 = value;
                }
            }

            public System.Decimal OutLFFare5
            {
                get
                {
                    return _OutLFFare5;
                }

                set
                {
                    _OutLFFare5 = value;
                }
            }

            public System.Decimal OutLFFare6
            {
                get
                {
                    return _OutLFFare6;
                }

                set
                {
                    _OutLFFare6 = value;
                }
            }

            public System.Decimal OutLFFare7
            {
                get
                {
                    return _OutLFFare7;
                }

                set
                {
                    _OutLFFare7 = value;
                }
            }

            public System.Decimal OutLFFare8
            {
                get
                {
                    return _OutLFFare8;
                }

                set
                {
                    _OutLFFare8 = value;
                }
            }

            public System.Decimal OutLFFare9
            {
                get
                {
                    return _OutLFFare9;
                }

                set
                {
                    _OutLFFare9 = value;
                }
            }

            public System.Decimal OutLFFare10
            {
                get
                {
                    return _OutLFFare10;
                }

                set
                {
                    _OutLFFare10 = value;
                }
            }

            public System.Decimal OutLFFare11
            {
                get
                {
                    return _OutLFFare11;
                }

                set
                {
                    _OutLFFare11 = value;
                }
            }

            public System.Guid rowguid
            {
                get
                {
                    return _rowguid;
                }

                set
                {
                    _rowguid = value;
                }
            }

            public System.Byte Status
            {
                get
                {
                    return _Status;
                }

                set
                {
                    _Status = value;
                }
            }

            public System.Byte Inuse
            {
                get
                {
                    return _Inuse;
                }

                set
                {
                    _Inuse = value;
                }
            }

            public System.DateTime SyncCreate
            {
                get
                {
                    return _SyncCreate;
                }

                set
                {
                    _SyncCreate = value;
                }
            }

            public System.DateTime SyncLastUpd
            {
                get
                {
                    return _SyncLastUpd;
                }

                set
                {
                    _SyncLastUpd = value;
                }
            }

            public System.String LastSyncBy
            {
                get
                {
                    return _LastSyncBy;
                }

                set
                {
                    _LastSyncBy = value;
                }
            }

            public System.DateTime CreateDate
            {
                get
                {
                    return _CreateDate;
                }

                set
                {
                    _CreateDate = value;
                }
            }

            public System.String CreateBy
            {
                get
                {
                    return _CreateBy;
                }

                set
                {
                    _CreateBy = value;
                }
            }

            public System.DateTime UpdateDate
            {
                get
                {
                    return _UpdateDate;
                }

                set
                {
                    _UpdateDate = value;
                }
            }

            public System.String UpdateBy
            {
                get
                {
                    return _UpdateBy;
                }

                set
                {
                    _UpdateBy = value;
                }
            }

            public System.Byte Active
            {
                get
                {
                    return _Active;
                }

                set
                {
                    _Active = value;
                }
            }
            #endregion
        }
        public DataTable GetSeries()
        {
            DataTable dt = new DataTable();
            String strSQL = string.Empty;
            try
            {

                strSQL = "SELECT M.Analyst, LD.MarketCode, N.NDODesc, CM.CodeDesc, M.InRoute, LD.InFlightTimeGrp, LD.InDepDOW, ";
                strSQL += "LD.InAgentTier, LD.InCurrency, LD.InLFFare1, LD.InLFFare2, LD.InLFFare3, LD.InLFFare4, LD.InLFFare5, ";
                strSQL += "LD.InLFFare6, LD.InLFFare7, LD.InLFFare8, LD.InLFFare9, LD.InLFFare10, LD.InLFFare11, M.OutRoute, ";
                strSQL += "LD.OutFlightTimeGrp, LD.OutDepDOW, LD.OutAgentTier, LD.OutCurrency, LD.OutLFFare1, LD.OutLFFare2, ";
                strSQL += "LD.OutLFFare3, LD.OutLFFare4, LD.OutLFFare5, LD.OutLFFare6, LD.OutLFFare7, LD.OutLFFare8, LD.OutLFFare9, ";
                strSQL += "LD.OutLFFare10 ,LD.OutLFFare11 FROM AD_SPECIALFARE LD INNER JOIN AD_MARKET M ON LD.MarketCode = M.MarketCode ";
                strSQL += "INNER JOIN AD_NDO N ON LD.NDO = N.NDOCode INNER JOIN CODEMASTER CM ON LD.SpecCode = CM.Code AND CM.CodeType = 'SPC' ";
                strSQL += "WHERE SpecCode = 'SER'";

                using (var connection = new SqlConnection(_appConfiguration.GetConnectionString(PlexformConsts.GBSConnectionString)))
                {
                    connection.Open();
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
                        return null;
                        throw new ApplicationException("AD_SPECIALFARE does not exist.");
                    }
                }
                //return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetUmrahLabor()
        {
            DataTable dt = new DataTable();
            String strSQL = string.Empty;
            try
            {

                strSQL = "SELECT M.Analyst, LD.MarketCode, N.NDODesc, CM.CodeDesc, M.InRoute, LD.InFlightTimeGrp, LD.InDepDOW, ";
                strSQL += "LD.InAgentTier, LD.InCurrency, LD.InLFFare1, LD.InLFFare2, LD.InLFFare3, LD.InLFFare4, LD.InLFFare5, ";
                strSQL += "LD.InLFFare6, LD.InLFFare7, LD.InLFFare8, LD.InLFFare9, LD.InLFFare10, LD.InLFFare11, M.OutRoute, ";
                strSQL += "LD.OutFlightTimeGrp, LD.OutDepDOW, LD.OutAgentTier, LD.OutCurrency, LD.OutLFFare1, LD.OutLFFare2, ";
                strSQL += "LD.OutLFFare3, LD.OutLFFare4, LD.OutLFFare5, LD.OutLFFare6, LD.OutLFFare7, LD.OutLFFare8, LD.OutLFFare9, ";
                strSQL += "LD.OutLFFare10, LD.OutLFFare11 FROM AD_SPECIALFARE LD INNER JOIN AD_MARKET M ON LD.MarketCode = M.MarketCode ";
                strSQL += "INNER JOIN AD_NDO N ON LD.NDO = N.NDOCode INNER JOIN CODEMASTER CM ON LD.SpecCode = CM.Code AND CM.CodeType = 'SPC' ";
                strSQL += "WHERE SpecCode IN ('LBR', 'UMR')";

                using (var connection = new SqlConnection(_appConfiguration.GetConnectionString(PlexformConsts.GBSConnectionString)))
                {
                    connection.Open();
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
                        return null;
                        throw new ApplicationException("AD_SPECIALFARE does not exist.");
                    }
                }
                //return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region CODEMASTER
        public class CodemasterInfo
        {
            protected System.String _CodeType = String.Empty;
            protected System.String _Code = String.Empty;
            private System.String _CodeDesc = String.Empty;
            private System.Int32 _CodeSeq;
            private System.Byte _SysCode;
            private System.Guid _rowguid;
            private System.DateTime _SyncCreate;
            private System.DateTime _SyncLastUpd;
            private System.Byte _IsHost;

            #region properties
            public System.String CodeType
            {
                get
                {
                    return _CodeType;
                }

                set
                {
                    _CodeType = value;
                }
            }

            public System.String Code
            {
                get
                {
                    return _Code;
                }

                set
                {
                    _Code = value;
                }
            }

            public System.String CodeDesc
            {
                get
                {
                    return _CodeDesc;
                }

                set
                {
                    _CodeDesc = value;
                }
            }

            public System.Int32 CodeSeq
            {
                get
                {
                    return _CodeSeq;
                }

                set
                {
                    _CodeSeq = value;
                }
            }

            public System.Byte SysCode
            {
                get
                {
                    return _SysCode;
                }

                set
                {
                    _SysCode = value;
                }
            }

            public System.Guid rowguid
            {
                get
                {
                    return _rowguid;
                }

                set
                {
                    _rowguid = value;
                }
            }

            public System.DateTime SyncCreate
            {
                get
                {
                    return _SyncCreate;
                }

                set
                {
                    _SyncCreate = value;
                }
            }

            public System.DateTime SyncLastUpd
            {
                get
                {
                    return _SyncLastUpd;
                }

                set
                {
                    _SyncLastUpd = value;
                }
            }

            public System.Byte IsHost
            {
                get
                {
                    return _IsHost;
                }

                set
                {
                    _IsHost = value;
                }
            }
            #endregion
        }
        public DataTable GetCodeMasterbyCodeType(string CodeType = "")
        {
            DataTable dt = new DataTable();
            String strSQL = string.Empty;
            String Filter = string.Empty;
            try
            {
                if (CodeType != "")
                    Filter = "WHERE CodeType = '" + CodeType + "'";
                strSQL = "SELECT CodeType, Code, CodeDesc, CodeSeq FROM CODEMASTER " + Filter + " ORDER BY CodeSeq ASC";


                using (var connection = new SqlConnection(_appConfiguration.GetConnectionString(PlexformConsts.GBSConnectionString)))
                {
                    connection.Open();
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
                        return null;
                        throw new ApplicationException("AD_MARKET does not exist.");
                    }
                }
                //return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool SaveAllCodeMaster(CodemasterInfo pCodeMaster)
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

                strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "CODEMASTER", "CodeType ='RST'" + " AND Code = '" + pCodeMaster.Code + "'");

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
                return false;
            }
        }
        #endregion

        #region GB24
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
		public DataTable GetPaxSetting()
        {

            DataTable dt = new DataTable();
            String strSQL = string.Empty;

            try
            {
                strSQL = "SELECT DISTINCT CountryCode, EffectiveDate, ExpiryDate, CountryName, Origin, AG.OrgID, AG.OrgName, GB.AgentID, CASE WHEN GB.AgentID = '' THEN '' WHEN GB.AgentID <> '' THEN (SELECT Username FROM AG_PROFILE WHERE AgentID = GB.AgentID) END Username, NoofPax, GB.Status FROM GB4SETTING GB LEFT JOIN AG_PROFILE AG WITH (NOLOCK) ON GB.OrgID = AG.OrgID";

                using (var connection = new SqlConnection(_appConfiguration.GetConnectionString(PlexformConsts.GBSConnectionString)))
                {
                    connection.Open();
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
                        return null;
                        throw new ApplicationException("GB4SETTING does not exist."); //added, for log purpose

                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetSinglePaxSetting(string CountryCode, string Origin, string OrgID, string AgentID)
        {

            DataTable dt = new DataTable();
            String strSQL = string.Empty;

            try
            {

                strSQL = "SELECT * FROM GB4SETTING WHERE CountryCode = '" + CountryCode + "' AND Origin = '" + Origin + "' AND OrgID = '" + OrgID + "' AND AgentID  = '" + AgentID + "'";

                using (var connection = new SqlConnection(_appConfiguration.GetConnectionString(PlexformConsts.GBSConnectionString)))
                {
                    connection.Open();
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
                        return null;
                        throw new ApplicationException("GB4SETTING does not exist."); //added, for log purpose

                    }
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public Boolean SaveGB4SETTING(GB4SETTING gB4SETTING)
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

                strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert, "GB4SETTING", string.Empty);

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
                return false;
            }
        }
        public Boolean UpdateGB4SETTING(GB4SETTING gB4SETTING, GB4SETTING gB4 = null)
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

                strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "GB4SETTING", "GB4SETTING.AgentID='" + gB4.AgentID + "' AND GB4SETTING.CountryCode='" + gB4.CountryCode + "' AND GB4SETTING.Origin='" + gB4.Origin + "' AND GB4SETTING.OrgID='" + gB4.OrgID + "'");
                if (gB4SETTING.AgentID == "")
                {
                    string subFirst = strSQL.Substring(0, 22);
                    string subRest = strSQL.Substring(22);
                    strSQL = subFirst + "AgentID = '', " + subRest;
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
                return false;
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
				//SystemLog.Notifier.Notify(ex);
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
				//SystemLog.Notifier.Notify(ex);
				return null;
			}
		}
		public DataTable GetAllAgent()
		{
			DataTable dt;
			String strSQL = string.Empty;
			try
			{
				strSQL = "SELECT OrgID, OrgName, AgentID, Username, Country FROM AG_PROFILE";
				dt = objDCom.Execute(strSQL, CommandType.Text, true);
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
				//SystemLog.Notifier.Notify(ex);
				return null;
			}
		}
		#endregion

		public bool SaveAllRestriction(Models.RestrictionModels pRestriction)
        {
            objSQL.ClearFields();
            objSQL.ClearCondtions();
            bool rValue = false;
            ArrayList lstSQL = new ArrayList();
            string strSQL = string.Empty;
            try
            {
                objSQL.AddField("CodeDesc", pRestriction.Status, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "CODEMASTER", "CodeType ='RST'" + " AND Code = 'IND'");
                lstSQL.Add(strSQL);
                objSQL.AddField("CodeDesc", pRestriction.BookFrom, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "CODEMASTER", "CodeType ='RST'" + " AND Code = 'BOOKFROM'");
                lstSQL.Add(strSQL);
                objSQL.AddField("CodeDesc", pRestriction.BookTo, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "CODEMASTER", "CodeType ='RST'" + " AND Code = 'BOOKTO'");
                lstSQL.Add(strSQL);
                objSQL.AddField("CodeDesc", pRestriction.TraFrom, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "CODEMASTER", "CodeType ='RST'" + " AND Code = 'TRAFROM'");
                lstSQL.Add(strSQL);
                objSQL.AddField("CodeDesc", pRestriction.TraTo, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "CODEMASTER", "CodeType ='RST'" + " AND Code = 'TRATO'");
                lstSQL.Add(strSQL);
                if (pRestriction.RestrictionNote == pRestriction.RestrictionNoteEx)
                {
                    objSQL.AddField("SYSValue", pRestriction.RestrictionNote, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "SYS_PREFT", "SYSKey ='RESTRICTIONNOTE'");
                    lstSQL.Add(strSQL);
                }
                else
                {
                    objSQL.AddField("SYSValue", pRestriction.RestrictionNote, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("SYSValueEx", pRestriction.RestrictionNoteEx, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "SYS_PREFT", "SYSKey ='RESTRICTIONNOTE'");
                    lstSQL.Add(strSQL);
                }
                if (pRestriction.RestrictionAlert == pRestriction.RestrictionAlertEx)
                {
                    objSQL.AddField("SYSValue", pRestriction.RestrictionAlert, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "SYS_PREFT", "SYSKey ='RESTRICTIONALERT'");
                    lstSQL.Add(strSQL);
                }
                else
                {
                    objSQL.AddField("SYSValue", pRestriction.RestrictionAlert, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    objSQL.AddField("SYSValueEx", pRestriction.RestrictionAlertEx, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                    strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "SYS_PREFT", "SYSKey ='RESTRICTIONALERT'");
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
                return false;
            }
        }

        #region SYS_PREFT
        public class SYS_PREFTInfo
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
        public DataTable GetSYSPreftbyKey(string SYSKey = "")
        {
            DataTable dt = new DataTable();
            String strSQL = string.Empty;
            String Filter = string.Empty;
            try
            {
                strSQL = "SELECT SYSKey, SYSValue, SYSValueEx FROM SYS_PREFT WHERE SYSKey = '" + SYSKey + "'";


                using (var connection = new SqlConnection(_appConfiguration.GetConnectionString(PlexformConsts.GBSConnectionString)))
                {
                    connection.Open();
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
                        return null;
                        throw new ApplicationException("AD_MARKET does not exist.");
                    }
                }
                //return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool SaveSYS_PREFTrestrict(SYS_PREFTInfo pSYS_PREFT_Info)
        {
            bool rValue = false;
            ArrayList lstSQL = new ArrayList();
            string strSQL = string.Empty;
            try
            {
                objSQL.AddField("GRPID", pSYS_PREFT_Info.GRPID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SYSKey", pSYS_PREFT_Info.SYSKey, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SYSValue", pSYS_PREFT_Info.SYSValue, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SYSValueEx", pSYS_PREFT_Info.SYSValueEx, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SYSSet", pSYS_PREFT_Info.SYSSet, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncCreate", pSYS_PREFT_Info.SyncCreate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncLastUpd", pSYS_PREFT_Info.SyncLastUpd, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("IsHost", pSYS_PREFT_Info.IsHost, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastSyncBy", pSYS_PREFT_Info.LastSyncBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);

                strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "SYS_PREFT", "SYS_PREFT.SYSKey='" + pSYS_PREFT_Info.SYSKey + "'");

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
                return false;
            }
        }
        #endregion

        #region Comunicate
        public async Task<IList<Plexform.Models.CountryModels>> GetAllCountry(string CountryCode)
        {
            IList<Plexform.Models.CountryModels> list = new List<Plexform.Models.CountryModels>();
            PaymentInfo Model = new PaymentInfo();
            DataTable dt;
            try
            {
                dt = GetCountry(CountryCode);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        list.Add(new Models.CountryModels
                        {
                            CountryCode = dt.Rows[i]["CountryCode"].ToString(),
                            CountryName = dt.Rows[i]["Name"].ToString(),
                            CurrencyCode = dt.Rows[i]["DefaultCurrencyCode"].ToString(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return await Task.FromResult(list);
        }

        public async Task<IList<Plexform.Models.PaymentSchemeModels>> GetAllScheme(string GRPID = "")
        {
            IList<Plexform.Models.PaymentSchemeModels> list = new List<Plexform.Models.PaymentSchemeModels>();
            PaymentInfo Model = new PaymentInfo();
            DataTable dt;
            try
            {
                dt = GetSchemeByCode(GRPID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string Currency = "";
                        int xMinDeposit = 0, xMaxDeposit = 0, xMinDeposit2 = 0, xMaxDeposit2 = 0, xDepositValue = 0;
                        if (dt.Rows[i]["Currency"].ToString() != "" && dt.Rows[i]["Currency"].ToString() != null)
                        {
                            Currency = dt.Rows[i]["Currency"].ToString();
                            xMinDeposit = Convert.ToInt32(dt.Rows[i]["MinDeposit"]);
                            xMaxDeposit = Convert.ToInt32(dt.Rows[i]["MaxDeposit"]);
                            xMinDeposit2 = Convert.ToInt32(dt.Rows[i]["MinDeposit2"]);
                            xMaxDeposit2 = Convert.ToInt32(dt.Rows[i]["MaxDeposit2"]);
                            xDepositValue = Convert.ToInt32(dt.Rows[i]["DepositValue"]);
                        };
                        list.Add(new Models.PaymentSchemeModels
                        {
                            GRPID = dt.Rows[i]["GRPID"].ToString(),
                            SchemeCode = dt.Rows[i]["SchemeCode"].ToString(),
                            CountryCode = dt.Rows[i]["CountryCode"].ToString(),
                            Duration = Convert.ToInt32(dt.Rows[i]["Duration"]),
                            Minduration = Convert.ToInt32(dt.Rows[i]["MinDuration"]),
                            PaymentType = dt.Rows[i]["PaymentType"].ToString(),
                            CurrencyCode = Currency,
                            MinDeposit = xMinDeposit,
                            MaxDeposit = xMaxDeposit,
                            MinDeposit2 = xMinDeposit2,
                            MaxDeposit2 = xMaxDeposit2,
                            DepositValue = xDepositValue,
                            Attempt_1 = Convert.ToInt32(dt.Rows[i]["Attempt_1"]),
                            Code_1 = dt.Rows[i]["Code_1"].ToString(),
                            Percentage_1 = Convert.ToInt32(dt.Rows[i]["Percentage_1"]),
                            Attempt_2 = Convert.ToInt32(dt.Rows[i]["Attempt_2"]),
                            Code_2 = dt.Rows[i]["Code_2"].ToString(),
                            Percentage_2 = Convert.ToInt32(dt.Rows[i]["Percentage_2"]),
                            Attempt_3 = Convert.ToInt32(dt.Rows[i]["Attempt_3"]),
                            Code_3 = dt.Rows[i]["Code_3"].ToString(),
                            Percentage_3 = Convert.ToInt32(dt.Rows[i]["Percentage_3"])
                            //, PrevCountry = dt.Rows[i]["PrevCountry"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return await Task.FromResult(list);
        }

        public async Task<IList<Plexform.Models.PaymentSchemeModels>> GetSchemeByCodes(string GRPID = "", string CountryCode = "", string SchemeCode = "")
        {
            IList<Plexform.Models.PaymentSchemeModels> list = new List<Plexform.Models.PaymentSchemeModels>();
            PaymentInfo Model = new PaymentInfo();
            DataTable dt;
            try
            {
                dt = GetSchemeByCode(GRPID, CountryCode, SchemeCode);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string Currency = "";
                        int xMinDeposit = 0, xMaxDeposit = 0, xMinDeposit2 = 0, xMaxDeposit2 = 0, xDepositValue = 0;
                        if (dt.Rows[i]["Currency"].ToString() != "" && dt.Rows[i]["Currency"].ToString() != null)
                        {
                            Currency = dt.Rows[i]["Currency"].ToString();
                            xMinDeposit = Convert.ToInt32(dt.Rows[i]["MinDeposit"]);
                            xMaxDeposit = Convert.ToInt32(dt.Rows[i]["MaxDeposit"]);
                            xMinDeposit2 = Convert.ToInt32(dt.Rows[i]["MinDeposit2"]);
                            xMaxDeposit2 = Convert.ToInt32(dt.Rows[i]["MaxDeposit2"]);
                            xDepositValue = Convert.ToInt32(dt.Rows[i]["DepositValue"]);
                        };
                        list.Add(new Models.PaymentSchemeModels
                        {
                            GRPID = dt.Rows[i]["GRPID"].ToString(),
                            SchemeCode = dt.Rows[i]["SchemeCode"].ToString(),
                            CountryCode = dt.Rows[i]["CountryCode"].ToString(),
                            Duration = Convert.ToInt32(dt.Rows[i]["Duration"]),
                            Minduration = Convert.ToInt32(dt.Rows[i]["MinDuration"]),
                            PaymentType = dt.Rows[i]["PaymentType"].ToString(),
                            CurrencyCode = Currency,
                            MinDeposit = xMinDeposit,
                            MaxDeposit = xMaxDeposit,
                            MinDeposit2 = xMinDeposit2,
                            MaxDeposit2 = xMaxDeposit2,
                            DepositValue = xDepositValue,
                            Attempt_1 = Convert.ToInt32(dt.Rows[i]["Attempt_1"]),
                            Code_1 = dt.Rows[i]["Code_1"].ToString(),
                            Percentage_1 = Convert.ToInt32(dt.Rows[i]["Percentage_1"]),
                            Attempt_2 = Convert.ToInt32(dt.Rows[i]["Attempt_2"]),
                            Code_2 = dt.Rows[i]["Code_2"].ToString(),
                            Percentage_2 = Convert.ToInt32(dt.Rows[i]["Percentage_2"]),
                            Attempt_3 = Convert.ToInt32(dt.Rows[i]["Attempt_3"]),
                            Code_3 = dt.Rows[i]["Code_3"].ToString(),
                            Percentage_3 = Convert.ToInt32(dt.Rows[i]["Percentage_3"])
                            //,PrevCountry = dt.Rows[i]["PrevCountry"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return await Task.FromResult(list);
        }

        public Task<bool> UpdatePaymentScheme(PaymentInfo[] InfoScheme)
        {
            var res = false;
            try
            {
                res = SavePaymentScheme(InfoScheme);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return Task.FromResult(res);
        }

        public async Task<IList<Plexform.Models.FltTimeGroupModels>> GroupTime(string Filter = "")
        {
            IList<Plexform.Models.FltTimeGroupModels> list = new List<Plexform.Models.FltTimeGroupModels>();
            FlttimegroupInfo Model = new FlttimegroupInfo();
            DataTable dt;
            try
            {
                dt = GetFLTTIMEGROUPList(Filter);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        list.Add(new Models.FltTimeGroupModels
                        {
                            FTGroupCode = dt.Rows[i]["FTGroupCode"].ToString(),
                            StartTime = (TimeSpan)dt.Rows[i]["StartTime"],
                            EndTime = (TimeSpan)dt.Rows[i]["EndTime"],
                            SyncCreate = (DateTime)dt.Rows[i]["SyncCreate"],
                            SyncLastUpd = (DateTime)dt.Rows[i]["SyncLastUpd"],
                            CreateDate = (DateTime)dt.Rows[i]["CreateDate"],
                            UpdateDate = (DateTime)dt.Rows[i]["UpdateDate"],
                            LastSyncBy = dt.Rows[i]["LastSyncBy"].ToString(),
                            CreateBy = dt.Rows[i]["CreateBy"].ToString(),
                            UpdateBy = dt.Rows[i]["UpdateBy"].ToString(),
                            Active = Convert.ToInt32(dt.Rows[i]["Active"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return await Task.FromResult(list);
        }

        public async Task<IList<Plexform.Models.AGENTACCESSFAREModels>> GetAgentAccessFareAll(string Filter = "")
        {
            IList<Plexform.Models.AGENTACCESSFAREModels> list = new List<Plexform.Models.AGENTACCESSFAREModels>();
            AGENTACCESSFAREInfo Model = new AGENTACCESSFAREInfo();
            DataTable dt;
            try
            {
                dt = GetAGENTACCESSFAREList(Filter);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        list.Add(new Models.AGENTACCESSFAREModels
                        {
                            MarketCode = dt.Rows[i]["MarketCode"].ToString(),
                            InTier = dt.Rows[i]["InTier"].ToString(),
                            OutTier = dt.Rows[i]["OutTier"].ToString(),
                            InFareClass = dt.Rows[i]["InFareClass"].ToString(),
                            OutFareClass = dt.Rows[i]["OutFareClass"].ToString(),
                            Status = Convert.ToInt32(dt.Rows[i]["Status"]),
                            Inuse = Convert.ToInt32(dt.Rows[i]["Inuse"]),
                            SyncCreate = (DateTime)dt.Rows[i]["SyncCreate"],
                            SyncLastUpd = (DateTime)dt.Rows[i]["SyncLastUpd"],
                            LastSyncBy = dt.Rows[i]["LastSyncBy"].ToString(),
                            CreateDate = (DateTime)dt.Rows[i]["CreateDate"],
                            CreateBy = dt.Rows[i]["CreateBy"].ToString(),
                            UpdateDate = (DateTime)dt.Rows[i]["UpdateDate"],
                            Active = Convert.ToInt32(dt.Rows[i]["Active"]),
                            UpdateBy = dt.Rows[i]["UpdateBy"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return await Task.FromResult(list);
        }

        public async Task<IList<Plexform.Models.AGENTACCESSFAREModels>> GetAgentAccessFarePIVOT()
        {
            IList<Plexform.Models.AGENTACCESSFAREModels> list = new List<Plexform.Models.AGENTACCESSFAREModels>();
            AGENTACCESSFAREInfo Model = new AGENTACCESSFAREInfo();
            DataTable dt;
            try
            {
                dt = GetAGENTACCESSFAREPIVOT();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        list.Add(new Models.AGENTACCESSFAREModels
                        {
                            Analyst = dt.Rows[i]["Analyst"].ToString(),
                            MarketCode = dt.Rows[i]["MarketCode"].ToString(),
                            InTier1 = dt.Rows[i]["InTier1"].ToString(),
                            InTier2 = dt.Rows[i]["InTier2"].ToString(),
                            InTier3 = dt.Rows[i]["InTier3"].ToString(),
                            InGeneric = dt.Rows[i]["InGeneric"].ToString(),
                            OutTier1 = dt.Rows[i]["OutTier1"].ToString(),
                            OutTier2 = dt.Rows[i]["OutTier2"].ToString(),
                            OutTier3 = dt.Rows[i]["OutTier3"].ToString(),
                            OutGeneric = dt.Rows[i]["OutGeneric"].ToString(),
                            InRoute = dt.Rows[i]["InRoute"].ToString(),
                            OutRoute = dt.Rows[i]["OutRoute"].ToString(),

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return await Task.FromResult(list);
        }

        public async Task<IList<Plexform.Models.AGENTTIERModels>> GetAgentTierGrid()
        {
            IList<Plexform.Models.AGENTTIERModels> list = new List<Plexform.Models.AGENTTIERModels>();
            AgentTierInfo Model = new AgentTierInfo();
            DataTable dt;
            try
            {
                dt = GetAgentTierListGrid();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        list.Add(new Models.AGENTTIERModels
                        {
                            MarketCode = dt.Rows[i]["MarketCode"].ToString(),
                            Analyst = dt.Rows[i]["Analyst"].ToString(),
                            InRoute = dt.Rows[i]["InRoute"].ToString(),
                            InTier = dt.Rows[i]["InTier"].ToString(),
                            InSubTier = dt.Rows[i]["InSubTier"].ToString(),
                            InAgent = dt.Rows[i]["InAgent"].ToString(),
                            InAgentEmail = dt.Rows[i]["InAgentEmail"].ToString(),
                            InAgentID = dt.Rows[i]["InAgentID"].ToString(),
                            OutRoute = dt.Rows[i]["OutRoute"].ToString(),
                            OutTier = dt.Rows[i]["OutTier"].ToString(),
                            OutSubTier = dt.Rows[i]["OutSubTier"].ToString(),
                            OutAgent = dt.Rows[i]["OutAgent"].ToString(),
                            OutAgentEmail = dt.Rows[i]["OutAgentEmail"].ToString(),
                            OutAgentID = dt.Rows[i]["OutAgentID"].ToString(),
                            CreateDate = (DateTime)dt.Rows[i]["CreateDate"],
                            UpdateDate = (DateTime)dt.Rows[i]["UpdateDate"]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return await Task.FromResult(list);
        }

        public async Task<IList<Plexform.Models.GROUPCAPModels>> GetGroupCapAll()
        {
            IList<Plexform.Models.GROUPCAPModels> list = new List<Plexform.Models.GROUPCAPModels>();
            MarketInfo Model = new MarketInfo();
            DataTable dt;
            try
            {
                dt = GetGroupCap();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        list.Add(new Models.GROUPCAPModels
                        {
                            MarketCode = dt.Rows[i]["MarketCode"].ToString(),
                            Analyst = dt.Rows[i]["Analyst"].ToString(),
                            InRoute = dt.Rows[i]["InRoute"].ToString(),
                            InGrpCap = (Decimal)dt.Rows[i]["InGrpCap"],
                            OutRoute = dt.Rows[i]["OutRoute"].ToString(),
                            OutGrpCap = (Decimal)dt.Rows[i]["OutGrpCap"]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return await Task.FromResult(list);
        }

        public async Task<IList<Plexform.Models.MAXDISCModels>> GetMaxDiscAll()
        {
            IList<Plexform.Models.MAXDISCModels> list = new List<Plexform.Models.MAXDISCModels>();
            MarketInfo Model = new MarketInfo();
            DataTable dt;
            try
            {
                dt = GetMaxDisc();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        list.Add(new Models.MAXDISCModels
                        {
                            MarketCode = dt.Rows[i]["MarketCode"].ToString(),
                            Analyst = dt.Rows[i]["Analyst"].ToString(),
                            InRoute = dt.Rows[i]["InRoute"].ToString(),
                            InMaxDisc = (Decimal)dt.Rows[i]["InMaxDisc"],
                            OutRoute = dt.Rows[i]["OutRoute"].ToString(),
                            OutMaxDisc = (Decimal)dt.Rows[i]["OutMaxDisc"]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return await Task.FromResult(list);
        }

        public async Task<IList<Plexform.Models.SEASONALITYModels>> GetSeasonalityAll()
        {
            IList<Plexform.Models.SEASONALITYModels> list = new List<Plexform.Models.SEASONALITYModels>();
            SeasonalityInfo Model = new SeasonalityInfo();
            DataTable dt;
            try
            {
                dt = GetSeasonality();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        list.Add(new Models.SEASONALITYModels
                        {
                            Analyst = dt.Rows[i]["Analyst"].ToString(),
                            RouteCode = dt.Rows[i]["RouteCode"].ToString(),
                            SeasonDate = (DateTime)dt.Rows[i]["SeasonDate"],
                            Season = dt.Rows[i]["Season"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return await Task.FromResult(list);
        }

        public async Task<IList<Plexform.Models.DISCWEIGHTModels>> GetDiscWeightageAll()
        {
            IList<Plexform.Models.DISCWEIGHTModels> list = new List<Plexform.Models.DISCWEIGHTModels>();
            DISCWEIGHTAGEInfo Model = new DISCWEIGHTAGEInfo();
            DataTable dt;
            try
            {
                dt = GetDiscWeight();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        list.Add(new Models.DISCWEIGHTModels
                        {
                            Analyst = dt.Rows[i]["Analyst"].ToString(),
                            MarketCode = dt.Rows[i]["MarketCode"].ToString(),
                            InRoute = dt.Rows[i]["InRoute"].ToString(),
                            InWALFDisc = (Decimal)dt.Rows[i]["InWALFDisc"],
                            InWAPUDisc = (Decimal)dt.Rows[i]["InWAPUDisc"],
                            OutRoute = dt.Rows[i]["OutRoute"].ToString(),
                            OutWALFDisc = (Decimal)dt.Rows[i]["OutWALFDisc"],
                            OutWAPUDisc = (Decimal)dt.Rows[i]["OutWAPUDisc"]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return await Task.FromResult(list);
        }

        public async Task<IList<Plexform.Models.FLOORFAREModels>> GetFloorFareAll()
        {
            IList<Plexform.Models.FLOORFAREModels> list = new List<Plexform.Models.FLOORFAREModels>();
            FloorFareInfo Model = new FloorFareInfo();
            DataTable dt;
            try
            {
                dt = GetFloorFare();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        list.Add(new Models.FLOORFAREModels
                        {
                            Analyst = dt.Rows[i]["Analyst"].ToString(),
                            MarketCode = dt.Rows[i]["MarketCode"].ToString(),
                            InRoute = dt.Rows[i]["InRoute"].ToString(),
                            InCurrency = dt.Rows[i]["InCurrency"].ToString(),
                            InDisc = (Decimal)dt.Rows[i]["InDisc"],
                            InFareClass = dt.Rows[i]["InFareClass"].ToString(),
                            InFloorFare = (Decimal)dt.Rows[i]["InFloorFare"],
                            OutRoute = dt.Rows[i]["OutRoute"].ToString(),
                            OutCurrency = dt.Rows[i]["OutCurrency"].ToString(),
                            OutDisc = (Decimal)dt.Rows[i]["OutDisc"],
                            OutFareClass = dt.Rows[i]["OutFareClass"].ToString(),
                            OutFloorFare = (Decimal)dt.Rows[i]["OutFloorFare"]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return await Task.FromResult(list);
        }

        public async Task<IList<Plexform.Models.LFDISCOUNTModels>> GetLFDiscountAll()
        {
            IList<Plexform.Models.LFDISCOUNTModels> list = new List<Plexform.Models.LFDISCOUNTModels>();
            LFDiscountInfo Model = new LFDiscountInfo();
            DataTable dt;
            try
            {
                dt = GetLFDiscount();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        list.Add(new Models.LFDISCOUNTModels
                        {
                            Analyst = dt.Rows[i]["Analyst"].ToString(),
                            MarketCode = dt.Rows[i]["MarketCode"].ToString(),
                            NDODesc = dt.Rows[i]["NDODesc"].ToString(),
                            InRoute = dt.Rows[i]["InRoute"].ToString(),
                            InLFDisc1 = (Decimal)dt.Rows[i]["InLFDisc1"],
                            InLFDisc2 = (Decimal)dt.Rows[i]["InLFDisc2"],
                            InLFDisc3 = (Decimal)dt.Rows[i]["InLFDisc3"],
                            InLFDisc4 = (Decimal)dt.Rows[i]["InLFDisc4"],
                            InLFDisc5 = (Decimal)dt.Rows[i]["InLFDisc5"],
                            InLFDisc6 = (Decimal)dt.Rows[i]["InLFDisc6"],
                            InLFDisc7 = (Decimal)dt.Rows[i]["InLFDisc7"],
                            InLFDisc8 = (Decimal)dt.Rows[i]["InLFDisc8"],
                            InLFDisc9 = (Decimal)dt.Rows[i]["InLFDisc9"],
                            InLFDisc10 = (Decimal)dt.Rows[i]["InLFDisc10"],
                            OutRoute = dt.Rows[i]["OutRoute"].ToString(),
                            OutLFDisc1 = (Decimal)dt.Rows[i]["OutLFDisc1"],
                            OutLFDisc2 = (Decimal)dt.Rows[i]["OutLFDisc2"],
                            OutLFDisc3 = (Decimal)dt.Rows[i]["OutLFDisc3"],
                            OutLFDisc4 = (Decimal)dt.Rows[i]["OutLFDisc4"],
                            OutLFDisc5 = (Decimal)dt.Rows[i]["OutLFDisc5"],
                            OutLFDisc6 = (Decimal)dt.Rows[i]["OutLFDisc6"],
                            OutLFDisc7 = (Decimal)dt.Rows[i]["OutLFDisc7"],
                            OutLFDisc8 = (Decimal)dt.Rows[i]["OutLFDisc8"],
                            OutLFDisc9 = (Decimal)dt.Rows[i]["OutLFDisc9"],
                            OutLFDisc10 = (Decimal)dt.Rows[i]["OutLFDisc10"]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return await Task.FromResult(list);
        }

        public async Task<IList<Plexform.Models.PUDISCOUNTModels>> GetPUDiscountAll()
        {
            IList<Plexform.Models.PUDISCOUNTModels> list = new List<Plexform.Models.PUDISCOUNTModels>();
            LFDiscountInfo Model = new LFDiscountInfo();
            DataTable dt;
            try
            {
                dt = GetPUDiscount();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        list.Add(new Models.PUDISCOUNTModels
                        {
                            Analyst = dt.Rows[i]["Analyst"].ToString(),
                            MarketCode = dt.Rows[i]["MarketCode"].ToString(),
                            NDODesc = dt.Rows[i]["NDODesc"].ToString(),
                            InRoute = dt.Rows[i]["InRoute"].ToString(),
                            InPUDisc1 = (Decimal)dt.Rows[i]["InPUDisc1"],
                            InPUDisc2 = (Decimal)dt.Rows[i]["InPUDisc2"],
                            InPUDisc3 = (Decimal)dt.Rows[i]["InPUDisc3"],
                            InPUDisc4 = (Decimal)dt.Rows[i]["InPUDisc4"],
                            InPUDisc5 = (Decimal)dt.Rows[i]["InPUDisc5"],
                            InPUDisc6 = (Decimal)dt.Rows[i]["InPUDisc6"],
                            InPUDisc7 = (Decimal)dt.Rows[i]["InPUDisc7"],
                            InPUDisc8 = (Decimal)dt.Rows[i]["InPUDisc8"],
                            InPUDisc9 = (Decimal)dt.Rows[i]["InPUDisc9"],
                            InPUDisc10 = (Decimal)dt.Rows[i]["InPUDisc10"],
                            InPUDisc11 = (Decimal)dt.Rows[i]["InPUDisc11"],
                            OutRoute = dt.Rows[i]["OutRoute"].ToString(),
                            OutPUDisc1 = (Decimal)dt.Rows[i]["OutPUDisc1"],
                            OutPUDisc2 = (Decimal)dt.Rows[i]["OutPUDisc2"],
                            OutPUDisc3 = (Decimal)dt.Rows[i]["OutPUDisc3"],
                            OutPUDisc4 = (Decimal)dt.Rows[i]["OutPUDisc4"],
                            OutPUDisc5 = (Decimal)dt.Rows[i]["OutPUDisc5"],
                            OutPUDisc6 = (Decimal)dt.Rows[i]["OutPUDisc6"],
                            OutPUDisc7 = (Decimal)dt.Rows[i]["OutPUDisc7"],
                            OutPUDisc8 = (Decimal)dt.Rows[i]["OutPUDisc8"],
                            OutPUDisc9 = (Decimal)dt.Rows[i]["OutPUDisc9"],
                            OutPUDisc10 = (Decimal)dt.Rows[i]["OutPUDisc10"],
                            OutPUDisc11 = (Decimal)dt.Rows[i]["OutPUDisc11"]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return await Task.FromResult(list);
        }

        public async Task<IList<Plexform.Models.SPECIALFAREModels>> GetSeriesAll()
        {
            IList<Plexform.Models.SPECIALFAREModels> list = new List<Plexform.Models.SPECIALFAREModels>();
            SpecialFareInfo Model = new SpecialFareInfo();
            DataTable dt;
            try
            {
                dt = GetSeries();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        list.Add(new Models.SPECIALFAREModels
                        {
                            Analyst = dt.Rows[i]["Analyst"].ToString(),
                            MarketCode = dt.Rows[i]["MarketCode"].ToString(),
                            NDODesc = dt.Rows[i]["NDODesc"].ToString(),
                            CodeDesc = dt.Rows[i]["CodeDesc"].ToString(),
                            InRoute = dt.Rows[i]["InRoute"].ToString(),
                            InFlightTimeGrp = dt.Rows[i]["InFlightTimeGrp"].ToString(),
                            InDepDOW = dt.Rows[i]["InDepDOW"].ToString(),
                            InAgentTier = dt.Rows[i]["InAgentTier"].ToString(),
                            InCurrency = dt.Rows[i]["InCurrency"].ToString(),
                            InLFFare1 = (Decimal)dt.Rows[i]["InLFFare1"],
                            InLFFare2 = (Decimal)dt.Rows[i]["InLFFare2"],
                            InLFFare3 = (Decimal)dt.Rows[i]["InLFFare3"],
                            InLFFare4 = (Decimal)dt.Rows[i]["InLFFare4"],
                            InLFFare5 = (Decimal)dt.Rows[i]["InLFFare5"],
                            InLFFare6 = (Decimal)dt.Rows[i]["InLFFare6"],
                            InLFFare7 = (Decimal)dt.Rows[i]["InLFFare7"],
                            InLFFare8 = (Decimal)dt.Rows[i]["InLFFare8"],
                            InLFFare9 = (Decimal)dt.Rows[i]["InLFFare9"],
                            InLFFare10 = (Decimal)dt.Rows[i]["InLFFare10"],
                            InLFFare11 = (Decimal)dt.Rows[i]["InLFFare11"],
                            OutRoute = dt.Rows[i]["OutRoute"].ToString(),
                            OutFlightTimeGrp = dt.Rows[i]["OutFlightTimeGrp"].ToString(),
                            OutDepDOW = dt.Rows[i]["OutDepDOW"].ToString(),
                            OutAgentTier = dt.Rows[i]["OutAgentTier"].ToString(),
                            OutCurrency = dt.Rows[i]["OutCurrency"].ToString(),
                            OutLFFare1 = (Decimal)dt.Rows[i]["OutLFFare1"],
                            OutLFFare2 = (Decimal)dt.Rows[i]["OutLFFare2"],
                            OutLFFare3 = (Decimal)dt.Rows[i]["OutLFFare3"],
                            OutLFFare4 = (Decimal)dt.Rows[i]["OutLFFare4"],
                            OutLFFare5 = (Decimal)dt.Rows[i]["OutLFFare5"],
                            OutLFFare6 = (Decimal)dt.Rows[i]["OutLFFare6"],
                            OutLFFare7 = (Decimal)dt.Rows[i]["OutLFFare7"],
                            OutLFFare8 = (Decimal)dt.Rows[i]["OutLFFare8"],
                            OutLFFare9 = (Decimal)dt.Rows[i]["OutLFFare9"],
                            OutLFFare10 = (Decimal)dt.Rows[i]["OutLFFare10"],
                            OutLFFare11 = (Decimal)dt.Rows[i]["OutLFFare11"]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return await Task.FromResult(list);
        }

        public async Task<IList<Plexform.Models.SPECIALFAREModels>> GetUmrahLaborAll()
        {
            IList<Plexform.Models.SPECIALFAREModels> list = new List<Plexform.Models.SPECIALFAREModels>();
            SpecialFareInfo Model = new SpecialFareInfo();
            DataTable dt;
            try
            {
                dt = GetUmrahLabor();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        list.Add(new Models.SPECIALFAREModels
                        {
                            Analyst = dt.Rows[i]["Analyst"].ToString(),
                            MarketCode = dt.Rows[i]["MarketCode"].ToString(),
                            NDODesc = dt.Rows[i]["NDODesc"].ToString(),
                            CodeDesc = dt.Rows[i]["CodeDesc"].ToString(),
                            InRoute = dt.Rows[i]["InRoute"].ToString(),
                            InFlightTimeGrp = dt.Rows[i]["InFlightTimeGrp"].ToString(),
                            InDepDOW = dt.Rows[i]["InDepDOW"].ToString(),
                            InAgentTier = dt.Rows[i]["InAgentTier"].ToString(),
                            InCurrency = dt.Rows[i]["InCurrency"].ToString(),
                            InLFFare1 = (Decimal)dt.Rows[i]["InLFFare1"],
                            InLFFare2 = (Decimal)dt.Rows[i]["InLFFare2"],
                            InLFFare3 = (Decimal)dt.Rows[i]["InLFFare3"],
                            InLFFare4 = (Decimal)dt.Rows[i]["InLFFare4"],
                            InLFFare5 = (Decimal)dt.Rows[i]["InLFFare5"],
                            InLFFare6 = (Decimal)dt.Rows[i]["InLFFare6"],
                            InLFFare7 = (Decimal)dt.Rows[i]["InLFFare7"],
                            InLFFare8 = (Decimal)dt.Rows[i]["InLFFare8"],
                            InLFFare9 = (Decimal)dt.Rows[i]["InLFFare9"],
                            InLFFare10 = (Decimal)dt.Rows[i]["InLFFare10"],
                            InLFFare11 = (Decimal)dt.Rows[i]["InLFFare11"],
                            OutRoute = dt.Rows[i]["OutRoute"].ToString(),
                            OutFlightTimeGrp = dt.Rows[i]["OutFlightTimeGrp"].ToString(),
                            OutDepDOW = dt.Rows[i]["OutDepDOW"].ToString(),
                            OutAgentTier = dt.Rows[i]["OutAgentTier"].ToString(),
                            OutCurrency = dt.Rows[i]["OutCurrency"].ToString(),
                            OutLFFare1 = (Decimal)dt.Rows[i]["OutLFFare1"],
                            OutLFFare2 = (Decimal)dt.Rows[i]["OutLFFare2"],
                            OutLFFare3 = (Decimal)dt.Rows[i]["OutLFFare3"],
                            OutLFFare4 = (Decimal)dt.Rows[i]["OutLFFare4"],
                            OutLFFare5 = (Decimal)dt.Rows[i]["OutLFFare5"],
                            OutLFFare6 = (Decimal)dt.Rows[i]["OutLFFare6"],
                            OutLFFare7 = (Decimal)dt.Rows[i]["OutLFFare7"],
                            OutLFFare8 = (Decimal)dt.Rows[i]["OutLFFare8"],
                            OutLFFare9 = (Decimal)dt.Rows[i]["OutLFFare9"],
                            OutLFFare10 = (Decimal)dt.Rows[i]["OutLFFare10"],
                            OutLFFare11 = (Decimal)dt.Rows[i]["OutLFFare11"]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return await Task.FromResult(list);
        }

        public async Task<IList<Plexform.Models.MAXDISCModels>> GetMarketAll()
        {
            IList<Plexform.Models.MAXDISCModels> list = new List<Plexform.Models.MAXDISCModels>();
            MarketInfo Model = new MarketInfo();
            DataTable dt;
            try
            {
                dt = GetMarket();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        list.Add(new Models.MAXDISCModels
                        {
                            MarketCode = dt.Rows[i]["MarketCode"].ToString(),
                            Analyst = dt.Rows[i]["Analyst"].ToString(),
                            InRoute = dt.Rows[i]["InRoute"].ToString(),
                            OutRoute = dt.Rows[i]["OutRoute"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return await Task.FromResult(list);
        }

		public async Task<Plexform.Models.RestrictionModels> GetRestriction(string CodeType = "", string SYSKey1 = "", string SYSKey2 = "")
		{
			Models.RestrictionModels obj = new Models.RestrictionModels();
			DataTable dtMaster, dtPreft1, dtPreft2;
			try
			{
				dtMaster = GetCodeMasterbyCodeType(CodeType);
				if (dtMaster != null && dtMaster.Rows.Count > 0)
				{
					for (int i = 0; i < dtMaster.Rows.Count; i++)
					{
						if(dtMaster.Rows[i]["Code"].ToString() == "IND")
						{
							obj.Status = dtMaster.Rows[i]["CodeDesc"].ToString();
						}
						else if (dtMaster.Rows[i]["Code"].ToString() == "BOOKFROM")
						{
							obj.BookFrom = dtMaster.Rows[i]["CodeDesc"].ToString();
							//obj.BookFrom = Convert.ToDateTime(dtMaster.Rows[i]["CodeDesc"]);
						}
						else if (dtMaster.Rows[i]["Code"].ToString() == "BOOKTO")
						{
							obj.BookTo = dtMaster.Rows[i]["CodeDesc"].ToString();
							//obj.BookTo = Convert.ToDateTime(dtMaster.Rows[i]["CodeDesc"]);
						}
						else if (dtMaster.Rows[i]["Code"].ToString() == "TRAFROM")
						{
							obj.TraFrom = dtMaster.Rows[i]["CodeDesc"].ToString();
							//obj.TraFrom = Convert.ToDateTime(dtMaster.Rows[i]["CodeDesc"]);
						}
						else if (dtMaster.Rows[i]["Code"].ToString() == "TRATO")
						{
							obj.TraTo = dtMaster.Rows[i]["CodeDesc"].ToString();
							//obj.TraTo= Convert.ToDateTime(dtMaster.Rows[i]["CodeDesc"]);
						}
					}
				}
				dtPreft1 = GetSYSPreftbyKey(SYSKey1);
				if (dtPreft1 != null && dtPreft1.Rows.Count > 0)
				{
					for (int i = 0; i < dtPreft1.Rows.Count; i++)
					{
						obj.RestrictionNote = dtPreft1.Rows[i]["SYSValue"].ToString();
					}
				}
				dtPreft2 = GetSYSPreftbyKey(SYSKey2);
				if (dtPreft2 != null && dtPreft2.Rows.Count > 0)
				{
					for (int i = 0; i < dtPreft2.Rows.Count; i++)
					{
						obj.RestrictionAlert = dtPreft2.Rows[i]["SYSValue"].ToString();
					}
				}
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return await Task.FromResult(obj);
		}

		public Task<bool> UpdateRestriction(Models.RestrictionModels pRestriction)
		{
			var res = false;
			try
			{
				res = SaveAllRestriction(pRestriction);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return Task.FromResult(res);
		}

		public async Task<IList<Plexform.Models.CODEMASTERModels>> GetCodeMasterbyCodeTypeAll(string CodeType = "")
		{
			IList<Plexform.Models.CODEMASTERModels> list = new List<Plexform.Models.CODEMASTERModels>();
			CodemasterInfo Model = new CodemasterInfo();
			DataTable dt;
			try
			{
				dt = GetCodeMasterbyCodeType(CodeType);
				if (dt != null && dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						list.Add(new Models.CODEMASTERModels
						{
							CodeType = dt.Rows[i]["CodeType"].ToString(),
							Code = dt.Rows[i]["Code"].ToString(),
							CodeDesc = dt.Rows[i]["CodeDesc"].ToString()
						});
					}
				}
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return await Task.FromResult(list);
		}

        public Task<bool> UpdateCodeMaster(CodemasterInfo pCodemaster)
        {
            var res = false;
            try
            {
                res = SaveAllCodeMaster(pCodemaster);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return Task.FromResult(res);
        }

        public Task<bool> UpdateSYSPreft(SYS_PREFTInfo pSYSPreft)
        {
            var res = false;
            try
            {
                res = SaveSYS_PREFTrestrict(pSYSPreft);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return Task.FromResult(res);
        }

		public Task<bool> UploadGroupTime(Models.FltTimeGroupModels[] UploadFile)
		{
			var res = false;
			try
			{
				if (UploadFile != null)
				{
					res = SaveFlightTimeGroup(UploadFile);
				}
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return Task.FromResult(res);
		}

		public Task<bool> UploadAgentTier(Models.FltTimeGroupModels[] UploadFile)
		{
			var res = false;
			try
			{
				if (UploadFile != null)
				{
					res = SaveFlightTimeGroup(UploadFile);
				}
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return Task.FromResult(res);
		}

		public Task<bool> UploadAgentFare(Models.FltTimeGroupModels[] UploadFile)
		{
			var res = false;
			try
			{
				if (UploadFile != null)
				{
					res = SaveFlightTimeGroup(UploadFile);
				}
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return Task.FromResult(res);
		}

		public Task<bool> UploadDiscount(Models.FltTimeGroupModels[] UploadFile)
		{
			var res = false;
			try
			{
				if (UploadFile != null)
				{
					res = SaveFlightTimeGroup(UploadFile);
				}
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return Task.FromResult(res);
		}

		public Task<bool> UploadCapacity(Models.FltTimeGroupModels[] UploadFile)
		{
			var res = false;
			try
			{
				if (UploadFile != null)
				{
					res = SaveFlightTimeGroup(UploadFile);
				}
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return Task.FromResult(res);
		}

		public Task<bool> UploadDiscWeight(Models.FltTimeGroupModels[] UploadFile)
		{
			var res = false;
			try
			{
				if (UploadFile != null)
				{
					res = SaveFlightTimeGroup(UploadFile);
				}
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return Task.FromResult(res);
		}

		public Task<bool> UploadFloorFare(Models.FltTimeGroupModels[] UploadFile)
		{
			var res = false;
			try
			{
				if (UploadFile != null)
				{
					res = SaveFlightTimeGroup(UploadFile);
				}
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return Task.FromResult(res);
		}

		public Task<bool> UploadSeasonality(Models.FltTimeGroupModels[] UploadFile)
		{
			var res = false;
			try
			{
				if (UploadFile != null)
				{
					res = SaveFlightTimeGroup(UploadFile);
				}
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return Task.FromResult(res);
		}

		public Task<bool> UploadLFDiscount(Models.FltTimeGroupModels[] UploadFile)
		{
			var res = false;
			try
			{
				if (UploadFile != null)
				{
					res = SaveFlightTimeGroup(UploadFile);
				}
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return Task.FromResult(res);
		}

		public Task<bool> UploadPUDiscount(Models.FltTimeGroupModels[] UploadFile)
		{
			var res = false;
			try
			{
				if (UploadFile != null)
				{
					res = SaveFlightTimeGroup(UploadFile);
				}
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return Task.FromResult(res);
		}

		public Task<bool> UploadSeries(Models.FltTimeGroupModels[] UploadFile)
		{
			var res = false;
			try
			{
				if (UploadFile != null)
				{
					res = SaveFlightTimeGroup(UploadFile);
				}
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return Task.FromResult(res);
		}

		public Task<bool> UploadUmrahLabor(Models.FltTimeGroupModels[] UploadFile)
		{
			var res = false;
			try
			{
				if (UploadFile != null)
				{
					res = SaveFlightTimeGroup(UploadFile);
				}
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return Task.FromResult(res);
		}

		public Task<List<T>> GenerateListFromString<T>(string content, ref string errorMsg)
		{
			var res = new List<T>();
			try
			{
				var json = Newtonsoft.Json.JsonConvert.DeserializeObject<T[]>(content);
				if (json != null && json is Array)
				{
					res.AddRange(json);
				}
				else
				{
					var temp = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
					if (temp != null)
					{
						if (temp is Array)
						{
							errorMsg = "Json format not match with the class container";
						}
						else
						{
							errorMsg = "Invalid json array";
						}
					}
					else
					{
						errorMsg = "Invalid json array";
					}
				}
			}
			catch (Exception ex)
			{
				errorMsg = ex.ToString();
			}
			return Task.FromResult(res);
		}

        public async Task<IList<Plexform.Models.GB4Models>> GetPaxSettingAll()
        {
            IList<Plexform.Models.GB4Models> list = new List<Plexform.Models.GB4Models>();
            GB4SETTING Model = new GB4SETTING();
            DataTable dt;
            try
            {
                dt = GetPaxSetting();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        list.Add(new Models.GB4Models
                        {
                            CountryCode = dt.Rows[i]["CountryCode"].ToString(),
                            CountryName = dt.Rows[i]["CountryName"].ToString(),
                            Origin = dt.Rows[i]["Origin"].ToString(),
                            OrgID = dt.Rows[i]["OrgID"].ToString(),
                            OrgName = dt.Rows[i]["OrgName"].ToString(),
                            Username = dt.Rows[i]["Username"].ToString(),
                            AgentID = dt.Rows[i]["AgentID"].ToString(),
                            NoofPax = Convert.ToInt32(dt.Rows[i]["NoofPax"]),
                            EffectiveDate = Convert.ToDateTime(dt.Rows[i]["EffectiveDate"]),
                            ExpiryDate = Convert.ToDateTime(dt.Rows[i]["ExpiryDate"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return await Task.FromResult(list);
        }

        public async Task<Plexform.Models.GB4Models> GetPaxSettingSingle(string CountryCode, string Origin, string OrgID, string AgentID)
        {
            Plexform.Models.GB4Models res = new Plexform.Models.GB4Models();
            GB4SETTING Model = new GB4SETTING();
            DataTable dt;
            try
            {
                dt = GetSinglePaxSetting(CountryCode, Origin, OrgID, AgentID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    res.CountryCode = dt.Rows[0]["CountryCode"].ToString();
                    res.CountryName = dt.Rows[0]["CountryName"].ToString();
                    res.Origin = dt.Rows[0]["Origin"].ToString();
                    res.OrgID = dt.Rows[0]["OrgID"].ToString();
                    res.OrgName = dt.Rows[0]["OrgName"].ToString();
                    res.Username = dt.Rows[0]["Username"].ToString();
                    res.AgentID = dt.Rows[0]["AgentID"].ToString();
                    res.NoofPax = Convert.ToInt32(dt.Rows[0]["NoofPax"]);
                    res.EffectiveDate = Convert.ToDateTime(dt.Rows[0]["EffectiveDate"]);
                    res.ExpiryDate = Convert.ToDateTime(dt.Rows[0]["ExpiryDate"]);
                }
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return await Task.FromResult(res);
        }

		public async Task<IList<Plexform.Models.GB4Models>> GetAllOrgIDs()
		{
			IList<Plexform.Models.GB4Models> res = new List<Plexform.Models.GB4Models>();
			GB4SETTING Model = new GB4SETTING();
			DataTable dt;
			try
			{
				dt = GetAllOrgID();
				if (dt != null && dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						res.Add(new Models.GB4Models
						{
							OrgID = dt.Rows[i]["OrgID"].ToString(),
							OrgName = dt.Rows[i]["OrgName"].ToString()
						});
					}
				}
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return await Task.FromResult(res);
		}

		public async Task<IList<Plexform.Models.GB4Models>> GetAllAgents()
		{
			IList<Plexform.Models.GB4Models> res = new List<Plexform.Models.GB4Models>();
			GB4SETTING Model = new GB4SETTING();
			DataTable dt;
			try
			{
				dt = GetAllAgent();
				if (dt != null && dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						res.Add(new Models.GB4Models
						{
							OrgID = dt.Rows[i]["OrgID"].ToString(),
                            OrgName = dt.Rows[i]["OrgName"].ToString(),
                            AgentID = dt.Rows[i]["AgentID"].ToString(),
							Username = dt.Rows[i]["Username"].ToString(),
                            CountryCode = dt.Rows[i]["Country"].ToString()
                        });
					}
				}
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return await Task.FromResult(res);
		}

		public Task<bool> UpdatePaxSetting(GB4SETTING InfoScheme, GB4SETTING InfoSchemeOld)
        {
            var res = false;
            try
            {
                res = UpdateGB4SETTING(InfoScheme, InfoSchemeOld);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return Task.FromResult(res);
        }

        public Task<bool> InsertPaxSetting(GB4SETTING InfoScheme)
        {
            var res = false;
            try
            {
                res = SaveGB4SETTING(InfoScheme);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return Task.FromResult(res);
        }

        public Task<bool> DeletePaxSetting(List<GB4SETTING> InfoScheme)
        {
            var res = false;
            try
            {
                res = DeleteGB4SETTING(InfoScheme);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return Task.FromResult(res);
        }
        #endregion
    }
}
