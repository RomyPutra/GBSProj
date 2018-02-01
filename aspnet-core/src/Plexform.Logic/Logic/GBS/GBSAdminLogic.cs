﻿using Abp.Reflection.Extensions;
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
			strJoint = "OUTER APPLY (SELECT MAX(D.MinDeposit) MinDeposit, MAX(D.MaxDeposit) MaxDeposit, MAX(D.MinDeposit2) MinDeposit2, MAX(D.MaxDeposit2) MaxDeposit2, MAX(D.Currency) Currency FROM Depoduration D WHERE D.GroupCode = DEPOPAYSCHEME.GRPID And D.Currency = DEPOPAYSCHEME.CurrencyCode) D ";
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

					strSQL = "UPDATE DEPODURATION SET DEPODURATION.Currency = '" + xInfo.CurrencyCode + "', DEPODURATION.MinDeposit = '" + xInfo.Mindeposit + "', DEPODURATION.MaxDeposit = '" + xInfo.Maxdeposit + "', DEPODURATION.MinDeposit2 = '" + xInfo.Mindeposit2 + "', DEPODURATION.MaxDeposit2 = '" + xInfo.Maxdeposit2 + "' FROM DEPOPAYSCHEME inner join Depoduration D ON D.GroupCode = DEPOPAYSCHEME.GRPID And D.Currency = DEPOPAYSCHEME.CurrencyCode WHERE DEPOPAYSCHEME.GRPID='" + xInfo.GRPID + "' AND DEPOPAYSCHEME.CountryCode='" + xInfo.CountryCode + "' AND DEPOPAYSCHEME.SchemeCode='" + xInfo.SchemeCode + "'";
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
						int xMinDeposit = 0, xMaxDeposit = 0, xMinDeposit2 = 0, xMaxDeposit2 = 0;
						if (dt.Rows[i]["Currency"].ToString() != "" && dt.Rows[i]["Currency"].ToString() != null)
						{
							Currency = dt.Rows[i]["Currency"].ToString();
							xMinDeposit = Convert.ToInt32(dt.Rows[i]["MinDeposit"]);
							xMaxDeposit = Convert.ToInt32(dt.Rows[i]["MaxDeposit"]);
							xMinDeposit2 = Convert.ToInt32(dt.Rows[i]["MinDeposit2"]);
							xMaxDeposit2 = Convert.ToInt32(dt.Rows[i]["MaxDeposit2"]);
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
						int xMinDeposit = 0, xMaxDeposit = 0, xMinDeposit2 = 0, xMaxDeposit2 = 0;
						if (dt.Rows[i]["Currency"].ToString() != "" && dt.Rows[i]["Currency"].ToString() != null)
						{
							Currency = dt.Rows[i]["Currency"].ToString();
							xMinDeposit = Convert.ToInt32(dt.Rows[i]["MinDeposit"]);
							xMaxDeposit = Convert.ToInt32(dt.Rows[i]["MaxDeposit"]);
							xMinDeposit2 = Convert.ToInt32(dt.Rows[i]["MinDeposit2"]);
							xMaxDeposit2 = Convert.ToInt32(dt.Rows[i]["MaxDeposit2"]);
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
        #endregion
    }
}
