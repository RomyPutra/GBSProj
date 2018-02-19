using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SEAL.Data;
using SEAL.Model;
using System.Data;
using ABS.GBS.Log;
//using log4net;

using System.Data.SqlClient;

namespace ABS.Logic.GroupBooking.Booking
{
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

        //added by diana 20140121 - for deposit
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

        //added by diana 20140121 - for deposit purpose
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

		#endregion
	}

	public class PaymentControl : Shared.CoreBase
    {
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogControl log = new LogControl();
        SystemLog SystemLog = new SystemLog();
		string ConnStr = "";
		//string ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ToString();
		//string ConnStr = "Data Source=172.20.145.11;Initial Catalog = GBSPILOT; Persist Security Info=True;User ID = gbs; Password=p@ssw0rd; connection timeout = 60; Application Name = GBS";

		public enum SaveType
        {
            Insert = 0,
            Update = 1
        }

        public string GetScheme(DateTime STD, DateTime bookingDate, string GRPID, string TransID, string CountryCode = "", string CurrencyCode = "", string AgentCountry = "")
        {
            //DateTime todays = DateTime.Now;
            string Scheme = "";
            List<PaymentInfo> paymentScheme = new List<PaymentInfo>();
            //paymentScheme = GetAllPaymentSchemeOth(CountryCode, CurrencyCode, AgentCountry);
            //if (paymentScheme == null)
            //{
                paymentScheme = GetAllPaymentScheme(TransID);
            //}
            //replace aax to aa 
            //GRPID = "AA"; //remarked by diana 20140120

            var rows = paymentScheme.Where(item => item.GRPID.Equals(GRPID));

            //int days = STD.Subtract(todays).Days;
            //int hours = STD.Subtract(todays).Hours;
            int days = STD.Subtract(bookingDate).Days;
            int hours = STD.Subtract(bookingDate).Hours + (days * 24);

            foreach (var row in rows)
            {
                if (row.Duration == 0)
                {
                    if (hours > row.MinDuration)
                    {
                        return row.SchemeCode;
                    }
                }
                else
                {
                    if (row.MinDuration == 0)
                    {
                        if (hours < row.Duration)
                        {
                            return row.SchemeCode;
                        }
                    }
                    else
                    {
                        if (hours <= row.Duration && hours >= row.MinDuration)
                        {
                            return row.SchemeCode;
                        }
                    }
                }

            }
            return "";

            if (GRPID == "AA" || GRPID == "AX") //amended by diana 20140120 - if (GRPID == "AA" || GRPID == "AX" || GRPID == "AAX")
            {
                if (days > 90)
                {
                    Scheme = "B3M";
                }
                else if (days <= 90 && days > 30)
                {
                    Scheme = "W3M";
                }
                else if (days <= 30 && days > 7)
                {
                    Scheme = "W1M";
                }
                else
                {
                    Scheme = "W1W";
                }
            }
            else
            {
                if (GRPID == "AAX")
                {
                    if (days > 90)
                    {
                        Scheme = "B3M";
                    }
                    else if (days <= 90 && days > 30)
                    {
                        Scheme = "W3M";
                    }
                    else if (days <= 30 && days > 7)
                    {
                        Scheme = "W1M";
                    }
                    else
                    {
                        Scheme = "W1W";
                    }
                }
            }

            return Scheme;
        }

        //added by ketee, old scheme
        public PaymentInfo GetPaymentScheme(string SchemeCode, string GRPID)
        {
            PaymentInfo paymentInfo = new PaymentInfo();
            //GRPID = "AA"; //remarked by diana 20140120
            paymentInfo = GetSinglePAYSCHEME(SchemeCode, GRPID);
            if (paymentInfo != null)
                return paymentInfo;
            else
                return null;


            return paymentInfo;
        }

        public PaymentInfo GetSinglePAYSCHEME(string pSchemeCode, string pGRPID)
        {
            PaymentInfo objPAYSCHEMEModel;
            DataTable dt = new DataTable();
            DateTime dateValue;
            String strSQL = string.Empty;
            String strFields = string.Empty;
            String strFilter = string.Empty;
            List<string> lstFields = new List<string>();
            try
            {
                lstFields.Add("PAYSCHEME.SchemeCode");
                lstFields.Add("PAYSCHEME.GRPID");
                lstFields.Add("PAYSCHEME.Duration");
                lstFields.Add("PAYSCHEME.MinDuration");
                lstFields.Add("PAYSCHEME.FirstDeposit");
                lstFields.Add("PAYSCHEME.Description");
                lstFields.Add("PAYSCHEME.PaymentType");
                lstFields.Add("PAYSCHEME.Attempt_1");
                lstFields.Add("PAYSCHEME.Code_1");
                lstFields.Add("PAYSCHEME.Percentage_1");
                lstFields.Add("PAYSCHEME.Attempt_2");
                lstFields.Add("PAYSCHEME.Code_2");
                lstFields.Add("PAYSCHEME.Percentage_2");
                lstFields.Add("PAYSCHEME.Attempt_3");
                lstFields.Add("PAYSCHEME.Code_3");
                lstFields.Add("PAYSCHEME.Percentage_3");
                lstFields.Add("PAYSCHEME.PaymentMode");
                lstFields.Add("PAYSCHEME.CreateBy");
                lstFields.Add("PAYSCHEME.SyncCreate");
                lstFields.Add("PAYSCHEME.SyncLastUpd");
                lstFields.Add("PAYSCHEME.LastSyncBy");
                lstFields.Add("PAYSCHEME.Reminder_1");
                lstFields.Add("PAYSCHEME.Reminder_2");

                //added by diana 20140121 - define whether have to pay for deposit or not
                lstFields.Add("PAYSCHEME.Deposit_1");
                lstFields.Add("PAYSCHEME.Deposit_2");
                lstFields.Add("PAYSCHEME.Deposit_3");

                strFields = GetSqlFields(lstFields);
                strFilter = "Where PAYSCHEME.GRPID='" + pGRPID + "' AND PAYSCHEME.SchemeCode='" + pSchemeCode + "'";
                strSQL = "SELECT " + strFields + " FROM PAYSCHEME " + strFilter + " ORDER BY MinDuration DESC";

                using (var connection = new SqlConnection(ConnStr))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(strSQL, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    connection.Close();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow drRow = dt.Rows[0];

                        objPAYSCHEMEModel = new PaymentInfo();
                        objPAYSCHEMEModel.SchemeCode = (string)drRow["SchemeCode"];
                        objPAYSCHEMEModel.GRPID = (string)drRow["GRPID"];
                        objPAYSCHEMEModel.Duration = (int)drRow["Duration"];
                        objPAYSCHEMEModel.MinDuration = (int)drRow["MinDuration"];
                        objPAYSCHEMEModel.FirstDeposit = (int)drRow["FirstDeposit"];
                        objPAYSCHEMEModel.Description = (string)drRow["Description"];
                        objPAYSCHEMEModel.PaymentType = (string)drRow["PaymentType"];
                        objPAYSCHEMEModel.Attempt_1 = (int)drRow["Attempt_1"];
                        objPAYSCHEMEModel.Code_1 = (string)drRow["Code_1"];
                        objPAYSCHEMEModel.Percentage_1 = (int)drRow["Percentage_1"];
                        objPAYSCHEMEModel.Attempt_2 = (int)drRow["Attempt_2"];
                        objPAYSCHEMEModel.Code_2 = (string)drRow["Code_2"];
                        objPAYSCHEMEModel.Percentage_2 = (int)drRow["Percentage_2"];
                        objPAYSCHEMEModel.Attempt_3 = (int)drRow["Attempt_3"];
                        objPAYSCHEMEModel.Code_3 = (string)drRow["Code_3"];
                        objPAYSCHEMEModel.Percentage_3 = (int)drRow["Percentage_3"];
                        objPAYSCHEMEModel.PaymentMode = (string)drRow["PaymentMode"];
                        objPAYSCHEMEModel.CreateBy = (string)drRow["CreateBy"];
                        if (DateTime.TryParse(drRow["SyncCreate"].ToString(), out dateValue)) objPAYSCHEMEModel.SyncCreate = (DateTime)drRow["SyncCreate"];
                        if (DateTime.TryParse(drRow["SyncLastUpd"].ToString(), out dateValue)) objPAYSCHEMEModel.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
                        objPAYSCHEMEModel.LastSyncBy = (string)drRow["LastSyncBy"];
                        objPAYSCHEMEModel.Reminder_1 = (int)drRow["Reminder_1"];
                        objPAYSCHEMEModel.Reminder_2 = (int)drRow["Reminder_2"];

                        //added by diana 20140121 - store deposit
                        objPAYSCHEMEModel.Deposit_1 = (int)drRow["Deposit_1"];
                        objPAYSCHEMEModel.Deposit_2 = (int)drRow["Deposit_2"];
                        objPAYSCHEMEModel.Deposit_3 = (int)drRow["Deposit_3"];

                        return objPAYSCHEMEModel;
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
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex);
                return null;
            }
        }

        //End Old Scheme

        public PaymentInfo GetPaymentScheme(string SchemeCode, string GRPID, string TransID, string countrycode = "", string currencycode = "", string agentcountry = "")
        {
            PaymentInfo paymentInfo = new PaymentInfo();
            //GRPID = "AA"; //remarked by diana 20140120
            //paymentInfo = GetSinglePAYSCHEMEOTH(SchemeCode, GRPID, TransID, countrycode, currencycode, agentcountry);
            //if (paymentInfo == null)
            //{
                paymentInfo = GetSinglePAYSCHEME(SchemeCode, GRPID, TransID, countrycode, currencycode);
            //}
            if (paymentInfo != null)
                return paymentInfo;
            else
                return null;
            //replace AAX to AA


            //if (GRPID == "AA")
            //{
            //    switch (SchemeCode.ToUpper())
            //    {
            //        case "B2M":
            //            paymentInfo.Attempt_1 = 24;
            //            paymentInfo.Attempt_2 = 720;
            //            paymentInfo.Attempt_3 = 720;
            //            paymentInfo.Duration = 1440;
            //            paymentInfo.FirstDeposit = 0;
            //            paymentInfo.PaymentType = "SVCF";
            //            paymentInfo.Percentage_1 = 30;
            //            paymentInfo.Percentage_2 = 20;
            //            paymentInfo.Percentage_3 = 50;
            //            paymentInfo.PaymentMode = "";
            //            break;

            //        case "W2M":
            //            paymentInfo.Attempt_1 = 24;
            //            paymentInfo.Attempt_2 = 720;
            //            paymentInfo.Attempt_3 = 0;
            //            paymentInfo.Duration = 1440;
            //            paymentInfo.FirstDeposit = 0;
            //            paymentInfo.PaymentType = "SVCF";
            //            paymentInfo.Percentage_1 = 50;
            //            paymentInfo.Percentage_2 = 50;
            //            paymentInfo.Percentage_3 = 0;
            //            paymentInfo.PaymentMode = "";
            //            break;

            //        case "W1M":
            //            paymentInfo.Attempt_1 = 24;
            //            paymentInfo.Attempt_2 = 0;
            //            paymentInfo.Attempt_3 = 0;
            //            paymentInfo.Duration = 720;
            //            paymentInfo.FirstDeposit = 0;
            //            paymentInfo.PaymentType = "SVCF";
            //            paymentInfo.Percentage_1 = 100;
            //            paymentInfo.Percentage_2 = 0;
            //            paymentInfo.Percentage_3 = 0;
            //            paymentInfo.PaymentMode = "";
            //            break;

            //        case "W1W":
            //            paymentInfo.Attempt_1 = 0;
            //            paymentInfo.Attempt_2 = 0;
            //            paymentInfo.Attempt_3 = 0;
            //            paymentInfo.Duration = 210;
            //            paymentInfo.FirstDeposit = 0;
            //            paymentInfo.PaymentType = "FULL";
            //            paymentInfo.Percentage_1 = 0;
            //            paymentInfo.Percentage_2 = 0;
            //            paymentInfo.Percentage_3 = 0;
            //            paymentInfo.PaymentMode = "";
            //            break;
            //    }
            //}
            //else
            //{
            //    if (GRPID == "AAX")
            //    {
            //        switch (SchemeCode.ToUpper())
            //        {
            //            case "XB3M":
            //                paymentInfo.Attempt_1 = 720;
            //                paymentInfo.Attempt_2 = 0;
            //                paymentInfo.Attempt_3 = 0;
            //                paymentInfo.Duration = 2160;
            //                paymentInfo.FirstDeposit = 0;
            //                paymentInfo.PaymentType = "SVCF";
            //                paymentInfo.Percentage_1 = 100;
            //                paymentInfo.Percentage_2 = 0;
            //                paymentInfo.Percentage_3 = 0;
            //                paymentInfo.PaymentMode = "";
            //                break;

            //            case "XW3M":
            //                paymentInfo.Attempt_1 = 720;
            //                paymentInfo.Attempt_2 = 0;
            //                paymentInfo.Attempt_3 = 0;
            //                paymentInfo.Duration = 2160;
            //                paymentInfo.FirstDeposit = 0;
            //                paymentInfo.PaymentType = "SVCF";
            //                paymentInfo.Percentage_1 = 100;
            //                paymentInfo.Percentage_2 = 0;
            //                paymentInfo.Percentage_3 = 0;
            //                paymentInfo.PaymentMode = "";
            //                break;

            //            case "XW1M":
            //                paymentInfo.Attempt_1 = 48;
            //                paymentInfo.Attempt_2 = 0;
            //                paymentInfo.Attempt_3 = 0;
            //                paymentInfo.Duration = 720;
            //                paymentInfo.FirstDeposit = 0;
            //                paymentInfo.PaymentType = "SVCF";
            //                paymentInfo.Percentage_1 = 100;
            //                paymentInfo.Percentage_2 = 0;
            //                paymentInfo.Percentage_3 = 0;
            //                paymentInfo.PaymentMode = "";
            //                break;

            //            case "XW1W":
            //                paymentInfo.Attempt_1 = 24;
            //                paymentInfo.Attempt_2 = 0;
            //                paymentInfo.Attempt_3 = 0;
            //                paymentInfo.Duration = 210;
            //                paymentInfo.FirstDeposit = 0;
            //                paymentInfo.PaymentType = "SVCF";
            //                paymentInfo.Percentage_1 = 100;
            //                paymentInfo.Percentage_2 = 0;
            //                paymentInfo.Percentage_3 = 0;
            //                paymentInfo.PaymentMode = "";
            //                break;
            //        }
            //    }
            //}

            return paymentInfo;
        }

        public DateTime GetExpiryDate(string Scheme, string GRPID, DateTime STD, DateTime BookingDate, string TransID)
        {
            PaymentInfo paymentInfo = new PaymentInfo();
            //replace AAX to AA
            //GRPID = "AA";
            paymentInfo = GetPaymentScheme(Scheme, GRPID, TransID);
            if (paymentInfo != null)
            {
                //if (GRPID == "AA") //added by diana 20140120 - add condition for AA/AAX
                //{
                /// amended by diana 20130913
                if (paymentInfo.Attempt_3 != 0 && paymentInfo.Percentage_3 != 0)
                {
                    if (paymentInfo.Code_3 == "DOB")
                        return BookingDate.AddHours(paymentInfo.Attempt_3);
                    else if (paymentInfo.Code_3 == "STD")
                        return STD.AddHours(-paymentInfo.Attempt_3);
                }
                //added by diana 20140121 - check for deposit
                else if (paymentInfo.Attempt_3 != 0 && paymentInfo.Deposit_3 != 0)
                {
                    if (paymentInfo.Code_3 == "DOB")
                        return BookingDate.AddHours(paymentInfo.Attempt_3);
                    else if (paymentInfo.Code_3 == "STD")
                        return STD.AddHours(-paymentInfo.Attempt_3);
                }
                else if (paymentInfo.Attempt_2 != 0 && paymentInfo.Percentage_2 != 0)
                {
                    if (paymentInfo.Code_2 == "DOB")
                        return BookingDate.AddHours(paymentInfo.Attempt_2);
                    else if (paymentInfo.Code_2 == "STD")
                        return STD.AddHours(-paymentInfo.Attempt_2);
                }
                //added by diana 20140121 - check for deposit
                else if (paymentInfo.Attempt_2 != 0 && paymentInfo.Deposit_3 != 0)
                {
                    if (paymentInfo.Code_2 == "DOB")
                        return BookingDate.AddHours(paymentInfo.Attempt_2);
                    else if (paymentInfo.Code_2 == "STD")
                        return STD.AddHours(-paymentInfo.Attempt_2);
                }
                else
                {
                    if (paymentInfo.Code_1 == "DOB")
                        return BookingDate.AddHours(paymentInfo.Attempt_1);
                    else if (paymentInfo.Code_1 == "STD")
                        return STD.AddHours(-paymentInfo.Attempt_1);
                }
                return STD.AddHours(-48);
                //}
                //else if (GRPID == "AAX")
                //{
                //    if (paymentInfo.Attempt_3 != 0 && paymentInfo.Percentage_3 != 0)
                //    {
                //        if (paymentInfo.Code_3 == "DOB")
                //            return BookingDate.AddHours(paymentInfo.Attempt_3);
                //        else if (paymentInfo.Code_3 == "STD")
                //            return STD.AddHours(-paymentInfo.Attempt_3);
                //    }
                //    else if (paymentInfo.Attempt_2 != 0 && paymentInfo.Percentage_2 != 0)
                //    {
                //        if (paymentInfo.Code_2 == "DOB")
                //            return BookingDate.AddHours(paymentInfo.Attempt_2);
                //        else if (paymentInfo.Code_2 == "STD")
                //            return STD.AddHours(-paymentInfo.Attempt_2);
                //    }
                //    else
                //    {
                //        if (paymentInfo.Code_1 == "DOB")
                //            return BookingDate.AddHours(paymentInfo.Attempt_1);
                //        else if (paymentInfo.Code_1 == "STD")
                //            return STD.AddHours(-paymentInfo.Attempt_1);
                //    }
                //    return STD.AddHours(-48);
                //}
                //if (GRPID == "AA")
                //{
                //    switch (Scheme.ToUpper())
                //    {
                //        case "B2M":
                //            return STD.AddHours(-paymentInfo.Attempt_3);

                //        case "W2M":
                //            return STD.AddHours(-paymentInfo.Attempt_2);
                //        case "W1M":
                //            return BookingDate.AddHours(paymentInfo.Attempt_1);

                //        case "W1W":
                //            return BookingDate.AddHours(paymentInfo.Attempt_1);
                //        default:
                //            return STD.AddHours(-48);
                //    }
                //}
                //else
                //{
                //    if (GRPID == "AAX")
                //    {
                //        switch (Scheme.ToUpper())
                //        {
                //            case "XB3M":
                //                return STD.AddHours(-2160);

                //            case "XW3M":
                //                return STD.AddHours(-2160);

                //            case "XW1M":
                //                return BookingDate.AddHours(48);

                //            case "XW1W":
                //                return BookingDate.AddHours(24);
                //            default:
                //                return STD.AddHours(-48);
                //        }
                //    }
                //}
            }
            return STD.AddHours(-48);


        }

        public List<PaymentInfo> GetAllPaymentSchemeOth(string CountryCode, string Currency, string AgentCountryCode)
        {
            PaymentInfo objPAYSCHEMEModel;
            List<PaymentInfo> objListPAYSCHEMEModel = new List<PaymentInfo>();
            DataTable dt = new DataTable();
            DateTime dateValue;
            String strSQL = string.Empty;

            GeneralControl objBooking = new GroupBooking.GeneralControl();

            DateTime bookingDate = Convert.ToDateTime("1900-01-01");
            DateTime CheckDate = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["CheckDate"].ToString());
            Boolean newGBS = true;
            try
            {


                strSQL = "SELECT * FROM PAYSCHEMEOTH WHERE AgentCountry = '" + AgentCountryCode + "' AND CountryCode = '" + CountryCode + "' AND CurrencyCode = '" + Currency + "'";

                log.Info(this, strSQL);
                using (var connection = new SqlConnection(ConnStr))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(strSQL, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    connection.Close();

                    if (dt != null && dt.Rows.Count > 0)
                    {

                        //to log columns
                        string colName = "";
                        foreach (DataColumn col in dt.Columns)
                        {
                            colName += col.ColumnName + ";";
                        }
                        log.Info(this, colName);
                        //to log columns

                        foreach (DataRow drRow in dt.Rows)
                        {
                            objPAYSCHEMEModel = new PaymentInfo();
                            objPAYSCHEMEModel.SchemeCode = (string)drRow["SchemeCode"];
                            objPAYSCHEMEModel.GRPID = (string)drRow["GRPID"];
                            objPAYSCHEMEModel.CountryCode = (string)drRow["countrycode"];
                            objPAYSCHEMEModel.CurrencyCode = (string)drRow["CurrencyCode"];
                            objPAYSCHEMEModel.Duration = (int)drRow["Duration"];
                            objPAYSCHEMEModel.MinDuration = (int)drRow["MinDuration"];
                            objPAYSCHEMEModel.FirstDeposit = (int)drRow["FirstDeposit"];
                            objPAYSCHEMEModel.Description = (string)drRow["Description"];
                            objPAYSCHEMEModel.PaymentType = (string)drRow["PaymentType"];
                            objPAYSCHEMEModel.Attempt_1 = (int)drRow["Attempt_1"];
                            objPAYSCHEMEModel.Code_1 = (string)drRow["Code_1"];
                            objPAYSCHEMEModel.Percentage_1 = (int)drRow["Percentage_1"];
                            objPAYSCHEMEModel.Attempt_2 = (int)drRow["Attempt_2"];
                            objPAYSCHEMEModel.Code_2 = (string)drRow["Code_2"];
                            objPAYSCHEMEModel.Percentage_2 = (int)drRow["Percentage_2"];
                            objPAYSCHEMEModel.Attempt_3 = (int)drRow["Attempt_3"];
                            objPAYSCHEMEModel.Code_3 = (string)drRow["Code_3"];
                            objPAYSCHEMEModel.Percentage_3 = (int)drRow["Percentage_3"];
                            objPAYSCHEMEModel.PaymentMode = (string)drRow["PaymentMode"];
                            objPAYSCHEMEModel.CreateBy = (string)drRow["CreateBy"];
                            if (DateTime.TryParse(drRow["SyncCreate"].ToString(), out dateValue)) objPAYSCHEMEModel.SyncCreate = (DateTime)drRow["SyncCreate"];
                            if (DateTime.TryParse(drRow["SyncLastUpd"].ToString(), out dateValue)) objPAYSCHEMEModel.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
                            objPAYSCHEMEModel.LastSyncBy = (string)drRow["LastSyncBy"];
                            objPAYSCHEMEModel.Reminder_1 = (int)drRow["Reminder_1"];
                            objPAYSCHEMEModel.Reminder_2 = (int)drRow["Reminder_2"];

                            //added by diana 20140121 - store deposit
                            objPAYSCHEMEModel.Deposit_1 = (int)drRow["Deposit_1"];
                            objPAYSCHEMEModel.IsNominal_1 = (int)drRow["IsNominal_1"];
                            objPAYSCHEMEModel.LDeposit_11 = (int)drRow["LDeposit_11"];
                            objPAYSCHEMEModel.LDuration_11 = (int)drRow["LDuration_11"];
                            objPAYSCHEMEModel.LDeposit_12 = (int)drRow["LDeposit_12"];
                            objPAYSCHEMEModel.LDuration_12 = (int)drRow["LDuration_12"];
                            objPAYSCHEMEModel.LDeposit_13 = (int)drRow["LDeposit_13"];
                            objPAYSCHEMEModel.LDuration_13 = (int)drRow["LDuration_13"];
                            objPAYSCHEMEModel.PDeposit_11 = (int)drRow["PDeposit_11"];
                            objPAYSCHEMEModel.PDuration_11 = (int)drRow["PDuration_11"];
                            objPAYSCHEMEModel.PDeposit_12 = (int)drRow["PDeposit_12"];
                            objPAYSCHEMEModel.PDuration_12 = (int)drRow["PDuration_12"];
                            objPAYSCHEMEModel.PDeposit_13 = (int)drRow["PDeposit_13"];
                            objPAYSCHEMEModel.PDuration_13 = (int)drRow["PDuration_13"];
                            objPAYSCHEMEModel.Deposit_2 = (int)drRow["Deposit_2"];
                            objPAYSCHEMEModel.Deposit_3 = (int)drRow["Deposit_3"];

                            objListPAYSCHEMEModel.Add(objPAYSCHEMEModel);
                        }
                        return objListPAYSCHEMEModel;
                    }
                    else
                    {
                        return null;
                        //throw new ApplicationException("PAYSCHEME does not exist.");
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

        public List<PaymentInfo> GetAllPaymentScheme(string TransID)
        {
            PaymentInfo objPAYSCHEMEModel;
            List<PaymentInfo> objListPAYSCHEMEModel = new List<PaymentInfo>();
            DataTable dt = new DataTable();
            DateTime dateValue;
            String strSQL = string.Empty;

            GeneralControl objBooking = new GroupBooking.GeneralControl();

            DateTime bookingDate = Convert.ToDateTime("1900-01-01");
            DateTime CheckDate = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["CheckDate"].ToString());
            Boolean newGBS = true;
            try
            {
                bookingDate = objBooking.getBookingDate(TransID, ref newGBS);

                if (bookingDate >= CheckDate)

                    strSQL = "SELECT * FROM DEPOPAYSCHEME ";
                else
                    strSQL = "SELECT * FROM DEPOPAYSCHEME ";

                log.Info(this, strSQL);
                using (var connection = new SqlConnection(ConnStr))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(strSQL, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    connection.Close();

                    if (dt != null && dt.Rows.Count > 0)
                    {

                        //to log columns
                        string colName = "";
                        foreach (DataColumn col in dt.Columns)
                        {
                            colName += col.ColumnName + ";";
                        }
                        log.Info(this, colName);
                        //to log columns

                        foreach (DataRow drRow in dt.Rows)
                        {
                            objPAYSCHEMEModel = new PaymentInfo();
                            objPAYSCHEMEModel.SchemeCode = (string)drRow["SchemeCode"];
                            objPAYSCHEMEModel.GRPID = (string)drRow["GRPID"];
                            objPAYSCHEMEModel.CountryCode = (string)drRow["countrycode"];
                            objPAYSCHEMEModel.CurrencyCode = (string)drRow["CurrencyCode"];
                            objPAYSCHEMEModel.Duration = (int)drRow["Duration"];
                            objPAYSCHEMEModel.MinDuration = (int)drRow["MinDuration"];
                            objPAYSCHEMEModel.FirstDeposit = (int)drRow["FirstDeposit"];
                            objPAYSCHEMEModel.Description = (string)drRow["Description"];
                            objPAYSCHEMEModel.PaymentType = (string)drRow["PaymentType"];
                            objPAYSCHEMEModel.Attempt_1 = (int)drRow["Attempt_1"];
                            objPAYSCHEMEModel.Code_1 = (string)drRow["Code_1"];
                            objPAYSCHEMEModel.Percentage_1 = (int)drRow["Percentage_1"];
                            objPAYSCHEMEModel.Attempt_2 = (int)drRow["Attempt_2"];
                            objPAYSCHEMEModel.Code_2 = (string)drRow["Code_2"];
                            objPAYSCHEMEModel.Percentage_2 = (int)drRow["Percentage_2"];
                            objPAYSCHEMEModel.Attempt_3 = (int)drRow["Attempt_3"];
                            objPAYSCHEMEModel.Code_3 = (string)drRow["Code_3"];
                            objPAYSCHEMEModel.Percentage_3 = (int)drRow["Percentage_3"];
                            objPAYSCHEMEModel.PaymentMode = (string)drRow["PaymentMode"];
                            objPAYSCHEMEModel.CreateBy = (string)drRow["CreateBy"];
                            if (DateTime.TryParse(drRow["SyncCreate"].ToString(), out dateValue)) objPAYSCHEMEModel.SyncCreate = (DateTime)drRow["SyncCreate"];
                            if (DateTime.TryParse(drRow["SyncLastUpd"].ToString(), out dateValue)) objPAYSCHEMEModel.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
                            objPAYSCHEMEModel.LastSyncBy = (string)drRow["LastSyncBy"];
                            objPAYSCHEMEModel.Reminder_1 = (int)drRow["Reminder_1"];
                            objPAYSCHEMEModel.Reminder_2 = (int)drRow["Reminder_2"];

                            //added by diana 20140121 - store deposit
                            objPAYSCHEMEModel.Deposit_1 = (int)drRow["Deposit_1"];
                            objPAYSCHEMEModel.IsNominal_1 = (int)drRow["IsNominal_1"];
                            objPAYSCHEMEModel.LDeposit_11 = (int)drRow["LDeposit_11"];
                            objPAYSCHEMEModel.LDuration_11 = (int)drRow["LDuration_11"];
                            objPAYSCHEMEModel.LDeposit_12 = (int)drRow["LDeposit_12"];
                            objPAYSCHEMEModel.LDuration_12 = (int)drRow["LDuration_12"];
                            objPAYSCHEMEModel.LDeposit_13 = (int)drRow["LDeposit_13"];
                            objPAYSCHEMEModel.LDuration_13 = (int)drRow["LDuration_13"];
                            objPAYSCHEMEModel.PDeposit_11 = (int)drRow["PDeposit_11"];
                            objPAYSCHEMEModel.PDuration_11 = (int)drRow["PDuration_11"];
                            objPAYSCHEMEModel.PDeposit_12 = (int)drRow["PDeposit_12"];
                            objPAYSCHEMEModel.PDuration_12 = (int)drRow["PDuration_12"];
                            objPAYSCHEMEModel.PDeposit_13 = (int)drRow["PDeposit_13"];
                            objPAYSCHEMEModel.PDuration_13 = (int)drRow["PDuration_13"];
                            objPAYSCHEMEModel.Deposit_2 = (int)drRow["Deposit_2"];
                            objPAYSCHEMEModel.Deposit_3 = (int)drRow["Deposit_3"];

                            objListPAYSCHEMEModel.Add(objPAYSCHEMEModel);
                        }
                        return objListPAYSCHEMEModel;
                    }
                    else
                    {
                        return null;
                        throw new ApplicationException("PAYSCHEME does not exist.");
                    }
                }
                //return null;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex);
                throw ex;
                //return null;
            }
        }


		//added by romy
		#region NewScheme
		public bool ConnectionURl(string conn)
		{
			this.ConnStr = conn;
			return true;
		}
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
		public DataTable GetSchemeByCode(string GRPID, string CountryCode = "", string SchemeCode = "")
		{
			DataTable dt = new DataTable();
			String strSQL = string.Empty;
			String strFields = string.Empty;
			String strFilter = string.Empty;
			List<string> lstFields = new List<string>();

			String strJoint = string.Empty;
			strJoint = "OUTER APPLY (SELECT MAX(D.MinDeposit) MinDeposit, MAX(D.MaxDeposit) MaxDeposit, MAX(D.MinDeposit2) MinDeposit2, MAX(D.MaxDeposit2) MaxDeposit2, MAX(D.Currency) Currency FROM Depoduration D WHERE D.GroupCode = DEPOPAYSCHEME.GRPID And D.Currency = DEPOPAYSCHEME.CurrencyCode) D ";

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

				lstFields.Add("D.Currency");
				lstFields.Add("D.MinDeposit");
				lstFields.Add("D.MaxDeposit");
				lstFields.Add("D.MinDeposit2");
				lstFields.Add("D.MaxDeposit2");

				strFields = GetSqlFields(lstFields);
				if (CountryCode == "" || SchemeCode == "")
					strFilter = "WHERE DEPOPAYSCHEME.GRPID='" + GRPID + "'";
				else
					strFilter = "WHERE DEPOPAYSCHEME.GRPID='" + GRPID + "' AND DEPOPAYSCHEME.CountryCode='" + CountryCode + "' AND DEPOPAYSCHEME.SchemeCode='" + SchemeCode + "'";

				strSQL = "SELECT " + strFields + " FROM DEPOPAYSCHEME  " + strJoint + strFilter + " ORDER BY MinDuration DESC";

				using (var connection = new SqlConnection(ConnStr))
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
				SystemLog.Notifier.Notify(ex);


				log.Error(this, ex);
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
					GetSchemeByCode(xInfo.GRPID, xInfo.CountryCode, xInfo.SchemeCode);

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

					strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "DEPOPAYSCHEME", "DEPOPAYSCHEME.GRPID='" + xInfo.GRPID + "' AND DEPOPAYSCHEME.CountryCode='" + xInfo.CountryCode + "' AND DEPOPAYSCHEME.SchemeCode='" + xInfo.SchemeCode + "'");
					lstSQL.Add(strSQL);

					strSQL = "UPDATE DEPODURATION SET DEPODURATION.Currency = '" + xInfo.CurrencyCode + "', DEPODURATION.MinDeposit = '" + xInfo.Mindeposit + "', DEPODURATION.MaxDeposit = '" + xInfo.Maxdeposit + "', DEPODURATION.MinDeposit2 = '" + xInfo.Mindeposit2 + "', DEPODURATION.MaxDeposit2 = '" + xInfo.Maxdeposit2 + "' FROM DEPOPAYSCHEME inner join Depoduration D ON D.GroupCode = DEPOPAYSCHEME.GRPID And D.Currency = DEPOPAYSCHEME.CurrencyCode WHERE DEPOPAYSCHEME.GRPID='" + xInfo.GRPID + "' AND DEPOPAYSCHEME.CountryCode='" + xInfo.CountryCode + "' AND DEPOPAYSCHEME.SchemeCode='" + xInfo.SchemeCode + "'";
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
		#endregion

		public DataTable GetAllScheme(string pGRPID, string TransID)
        {
            DataTable dt = new DataTable();
            String strSQL = string.Empty;
            String strFields = string.Empty;
            String strFilter = string.Empty;
            List<string> lstFields = new List<string>();

			//added by romy 220118
			String strJoint = string.Empty;
			strJoint = "OUTER APPLY (SELECT MAX(D.MinDeposit) MinDeposit, MAX(D.MaxDeposit) MaxDeposit, MAX(D.MinDeposit2) MinDeposit2, MAX(D.MaxDeposit2) MaxDeposit2, MAX(D.Currency) Currency FROM Depoduration D WHERE D.GroupCode = DEPOPAYSCHEME.GRPID And D.Currency = DEPOPAYSCHEME.CurrencyCode) D ";


			//GeneralControl objBooking = new GroupBooking.GeneralControl();

			DateTime bookingDate = Convert.ToDateTime("1900-01-01");
            //DateTime CheckDate = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["CheckDate"].ToString());
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

                //added by diana 20140121 - store deposit
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

				//added by romy 220118
				lstFields.Add("D.Currency");
				lstFields.Add("D.MinDeposit");
				lstFields.Add("D.MaxDeposit");
				lstFields.Add("D.MinDeposit2");
				lstFields.Add("D.MaxDeposit2");

				strFields = GetSqlFields(lstFields);
                strFilter = "WHERE DEPOPAYSCHEME.GRPID='" + pGRPID + "'";

                //bookingDate = objBooking.getBookingDate(TransID, ref newGBS);
				bookingDate = getBookingDate(TransID, ref newGBS);
				//if (bookingDate >= CheckDate)
				//	strSQL = "SELECT " + strFields + " FROM DEPOPAYSCHEME  " + strFilter + " ORDER BY MinDuration DESC";
				//else
				//	strSQL = "SELECT " + strFields + " FROM DEPOPAYSCHEME  " + strFilter + " ORDER BY MinDuration DESC";
				if (bookingDate >= CheckDate)
					strSQL = "SELECT " + strFields + " FROM DEPOPAYSCHEME  " + strJoint + strFilter + " ORDER BY MinDuration DESC";
				else
					strSQL = "SELECT " + strFields + " FROM DEPOPAYSCHEME  " + strJoint + strFilter + " ORDER BY MinDuration DESC";

				using (var connection = new SqlConnection(ConnStr))
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
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex);
                return null;
            }
        }

        public PaymentInfo GetSinglePAYSCHEME(string pSchemeCode, string pGRPID, string TransID, string CountryCode = "", string CurrencyCode = "")
        {
            PaymentInfo objPAYSCHEMEModel;
            DataTable dt = new DataTable();
            DateTime dateValue;
            String strSQL = string.Empty;
            String strFields = string.Empty;
            String strFilter = string.Empty;
            List<string> lstFields = new List<string>();

            GeneralControl objBooking = new GroupBooking.GeneralControl();

            DateTime bookingDate = Convert.ToDateTime("1900-01-01");
            DateTime CheckDate = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["CheckDate"].ToString());
            Boolean newGBS = true;
            try
            {
                lstFields.Add("DEPOPAYSCHEME.SchemeCode");
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

                //added by diana 20140121 - define whether have to pay for deposit or not
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

                strFields = GetSqlFields(lstFields);
                if (CountryCode != "" && CurrencyCode != "")
                {
                    strFilter = "Where DEPOPAYSCHEME.GRPID='" + pGRPID + "' AND DEPOPAYSCHEME.SchemeCode='" + pSchemeCode + "' AND DEPOPAYSCHEME.CountryCode='" + CountryCode + "' AND DEPOPAYSCHEME.CurrencyCode='" + CurrencyCode + "'";
                }
                else
                {
                    strFilter = "Where DEPOPAYSCHEME.GRPID='" + pGRPID + "' AND DEPOPAYSCHEME.SchemeCode='" + pSchemeCode + "'";
                }

                bookingDate = objBooking.getBookingDate(TransID, ref newGBS);
                if (bookingDate == null)
                {
                    bookingDate = DateTime.Now;
                }
                if (bookingDate >= CheckDate)
                    strSQL = "SELECT " + strFields + " FROM DEPOPAYSCHEME  " + strFilter + " ORDER BY MinDuration DESC";
                else
                    strSQL = "SELECT " + strFields + " FROM DEPOPAYSCHEME  " + strFilter + " ORDER BY MinDuration DESC";

                using (var connection = new SqlConnection(ConnStr))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(strSQL, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    connection.Close();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow drRow = dt.Rows[0];

                        objPAYSCHEMEModel = new PaymentInfo();
                        objPAYSCHEMEModel.SchemeCode = (string)drRow["SchemeCode"];
                        objPAYSCHEMEModel.GRPID = (string)drRow["GRPID"];
                        objPAYSCHEMEModel.Duration = (int)drRow["Duration"];
                        objPAYSCHEMEModel.MinDuration = (int)drRow["MinDuration"];
                        objPAYSCHEMEModel.FirstDeposit = (int)drRow["FirstDeposit"];
                        objPAYSCHEMEModel.Description = (string)drRow["Description"];
                        objPAYSCHEMEModel.PaymentType = (string)drRow["PaymentType"];
                        objPAYSCHEMEModel.Attempt_1 = (int)drRow["Attempt_1"];
                        objPAYSCHEMEModel.Code_1 = (string)drRow["Code_1"];
                        objPAYSCHEMEModel.Percentage_1 = (int)drRow["Percentage_1"];
                        objPAYSCHEMEModel.Attempt_2 = (int)drRow["Attempt_2"];
                        objPAYSCHEMEModel.Code_2 = (string)drRow["Code_2"];
                        objPAYSCHEMEModel.Percentage_2 = (int)drRow["Percentage_2"];
                        objPAYSCHEMEModel.Attempt_3 = (int)drRow["Attempt_3"];
                        objPAYSCHEMEModel.Code_3 = (string)drRow["Code_3"];
                        objPAYSCHEMEModel.Percentage_3 = (int)drRow["Percentage_3"];
                        objPAYSCHEMEModel.PaymentMode = (string)drRow["PaymentMode"];
                        objPAYSCHEMEModel.CreateBy = (string)drRow["CreateBy"];
                        if (DateTime.TryParse(drRow["SyncCreate"].ToString(), out dateValue)) objPAYSCHEMEModel.SyncCreate = (DateTime)drRow["SyncCreate"];
                        if (DateTime.TryParse(drRow["SyncLastUpd"].ToString(), out dateValue)) objPAYSCHEMEModel.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
                        objPAYSCHEMEModel.LastSyncBy = (string)drRow["LastSyncBy"];
                        objPAYSCHEMEModel.Reminder_1 = (int)drRow["Reminder_1"];
                        objPAYSCHEMEModel.Reminder_2 = (int)drRow["Reminder_2"];

                        //added by diana 20140121 - store deposit
                        objPAYSCHEMEModel.Deposit_1 = (int)drRow["Deposit_1"];
                        objPAYSCHEMEModel.IsNominal_1 = (int)drRow["IsNominal_1"];
                        objPAYSCHEMEModel.LDeposit_11 = (int)drRow["LDeposit_11"];
                        objPAYSCHEMEModel.LDuration_11 = (int)drRow["LDuration_11"];
                        objPAYSCHEMEModel.LDeposit_12 = (int)drRow["LDeposit_12"];
                        objPAYSCHEMEModel.LDuration_12 = (int)drRow["LDuration_12"];
                        objPAYSCHEMEModel.LDeposit_13 = (int)drRow["LDeposit_13"];
                        objPAYSCHEMEModel.LDuration_13 = (int)drRow["LDuration_13"];
                        objPAYSCHEMEModel.PDeposit_11 = (int)drRow["PDeposit_11"];
                        objPAYSCHEMEModel.PDuration_11 = (int)drRow["PDuration_11"];
                        objPAYSCHEMEModel.PDeposit_12 = (int)drRow["PDeposit_12"];
                        objPAYSCHEMEModel.PDuration_12 = (int)drRow["PDuration_12"];
                        objPAYSCHEMEModel.PDeposit_13 = (int)drRow["PDeposit_13"];
                        objPAYSCHEMEModel.PDuration_13 = (int)drRow["PDuration_13"];
                        objPAYSCHEMEModel.Deposit_2 = (int)drRow["Deposit_2"];
                        objPAYSCHEMEModel.Deposit_3 = (int)drRow["Deposit_3"];

                        return objPAYSCHEMEModel;
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
                SystemLog.Notifier.Notify(ex);


                log.Error(this, ex);
                return null;
            }
        }


        public PaymentInfo GetSinglePAYSCHEMEOTH(string pSchemeCode, string pGRPID, string TransID, string CountryCode = "", string CurrencyCode = "", string agentcountry = "")
        {
            PaymentInfo objPAYSCHEMEModel;
            DataTable dt = new DataTable();
            DateTime dateValue;
            String strSQL = string.Empty;
            String strFields = string.Empty;
            String strFilter = string.Empty;
            List<string> lstFields = new List<string>();

            GeneralControl objBooking = new GroupBooking.GeneralControl();

            DateTime bookingDate = Convert.ToDateTime("1900-01-01");
            DateTime CheckDate = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["CheckDate"].ToString());
            Boolean newGBS = true;
            try
            {
                lstFields.Add("PAYSCHEMEOTH.SchemeCode");
                lstFields.Add("PAYSCHEMEOTH.GRPID");
                lstFields.Add("PAYSCHEMEOTH.Duration");
                lstFields.Add("PAYSCHEMEOTH.MinDuration");
                lstFields.Add("PAYSCHEMEOTH.FirstDeposit");
                lstFields.Add("PAYSCHEMEOTH.Description");
                lstFields.Add("PAYSCHEMEOTH.PaymentType");
                lstFields.Add("PAYSCHEMEOTH.Attempt_1");
                lstFields.Add("PAYSCHEMEOTH.Code_1");
                lstFields.Add("PAYSCHEMEOTH.Percentage_1");
                lstFields.Add("PAYSCHEMEOTH.Attempt_2");
                lstFields.Add("PAYSCHEMEOTH.Code_2");
                lstFields.Add("PAYSCHEMEOTH.Percentage_2");
                lstFields.Add("PAYSCHEMEOTH.Attempt_3");
                lstFields.Add("PAYSCHEMEOTH.Code_3");
                lstFields.Add("PAYSCHEMEOTH.Percentage_3");
                lstFields.Add("PAYSCHEMEOTH.PaymentMode");
                lstFields.Add("PAYSCHEMEOTH.CreateBy");
                lstFields.Add("PAYSCHEMEOTH.SyncCreate");
                lstFields.Add("PAYSCHEMEOTH.SyncLastUpd");
                lstFields.Add("PAYSCHEMEOTH.LastSyncBy");
                lstFields.Add("PAYSCHEMEOTH.Reminder_1");
                lstFields.Add("PAYSCHEMEOTH.Reminder_2");

                //added by diana 20140121 - define whether have to pay for deposit or not
                lstFields.Add("PAYSCHEMEOTH.Deposit_1");
                lstFields.Add("PAYSCHEMEOTH.IsNominal_1");
                lstFields.Add("PAYSCHEMEOTH.LDeposit_11");
                lstFields.Add("PAYSCHEMEOTH.LDuration_11");
                lstFields.Add("PAYSCHEMEOTH.LDeposit_12");
                lstFields.Add("PAYSCHEMEOTH.LDuration_12");
                lstFields.Add("PAYSCHEMEOTH.LDeposit_13");
                lstFields.Add("PAYSCHEMEOTH.LDuration_13");
                lstFields.Add("PAYSCHEMEOTH.PDeposit_11");
                lstFields.Add("PAYSCHEMEOTH.PDuration_11");
                lstFields.Add("PAYSCHEMEOTH.PDeposit_12");
                lstFields.Add("PAYSCHEMEOTH.PDuration_12");
                lstFields.Add("PAYSCHEMEOTH.PDeposit_13");
                lstFields.Add("PAYSCHEMEOTH.PDuration_13");
                lstFields.Add("PAYSCHEMEOTH.Deposit_2");
                lstFields.Add("PAYSCHEMEOTH.Deposit_3");

                strFields = GetSqlFields(lstFields);
                if (CountryCode != "" && CurrencyCode != "")
                {
                    strFilter = "Where PAYSCHEMEOTH.GRPID='" + pGRPID + "' AND PAYSCHEMEOTH.SchemeCode='" + pSchemeCode + "' AND PAYSCHEMEOTH.CountryCode='" + CountryCode + "' AND PAYSCHEMEOTH.CurrencyCode='" + CurrencyCode + "' AND PAYSCHEMEOTH.AgentCountry='" + agentcountry + "'";
                }
                else
                {
                    strFilter = "Where PAYSCHEMEOTH.GRPID='" + pGRPID + "' AND PAYSCHEMEOTH.SchemeCode='" + pSchemeCode + "'";
                }

                bookingDate = objBooking.getBookingDate(TransID, ref newGBS);
                if (bookingDate == null)
                {
                    bookingDate = DateTime.Now;
                }
                if (bookingDate >= CheckDate)
                    strSQL = "SELECT " + strFields + " FROM PAYSCHEMEOTH  " + strFilter + " ORDER BY MinDuration DESC";
                else
                    strSQL = "SELECT " + strFields + " FROM PAYSCHEMEOTH  " + strFilter + " ORDER BY MinDuration DESC";

                using (var connection = new SqlConnection(ConnStr))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(strSQL, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    connection.Close();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow drRow = dt.Rows[0];

                        objPAYSCHEMEModel = new PaymentInfo();
                        objPAYSCHEMEModel.SchemeCode = (string)drRow["SchemeCode"];
                        objPAYSCHEMEModel.GRPID = (string)drRow["GRPID"];
                        objPAYSCHEMEModel.Duration = (int)drRow["Duration"];
                        objPAYSCHEMEModel.MinDuration = (int)drRow["MinDuration"];
                        objPAYSCHEMEModel.FirstDeposit = (int)drRow["FirstDeposit"];
                        objPAYSCHEMEModel.Description = (string)drRow["Description"];
                        objPAYSCHEMEModel.PaymentType = (string)drRow["PaymentType"];
                        objPAYSCHEMEModel.Attempt_1 = (int)drRow["Attempt_1"];
                        objPAYSCHEMEModel.Code_1 = (string)drRow["Code_1"];
                        objPAYSCHEMEModel.Percentage_1 = (int)drRow["Percentage_1"];
                        objPAYSCHEMEModel.Attempt_2 = (int)drRow["Attempt_2"];
                        objPAYSCHEMEModel.Code_2 = (string)drRow["Code_2"];
                        objPAYSCHEMEModel.Percentage_2 = (int)drRow["Percentage_2"];
                        objPAYSCHEMEModel.Attempt_3 = (int)drRow["Attempt_3"];
                        objPAYSCHEMEModel.Code_3 = (string)drRow["Code_3"];
                        objPAYSCHEMEModel.Percentage_3 = (int)drRow["Percentage_3"];
                        objPAYSCHEMEModel.PaymentMode = (string)drRow["PaymentMode"];
                        objPAYSCHEMEModel.CreateBy = (string)drRow["CreateBy"];
                        if (DateTime.TryParse(drRow["SyncCreate"].ToString(), out dateValue)) objPAYSCHEMEModel.SyncCreate = (DateTime)drRow["SyncCreate"];
                        if (DateTime.TryParse(drRow["SyncLastUpd"].ToString(), out dateValue)) objPAYSCHEMEModel.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
                        objPAYSCHEMEModel.LastSyncBy = (string)drRow["LastSyncBy"];
                        objPAYSCHEMEModel.Reminder_1 = (int)drRow["Reminder_1"];
                        objPAYSCHEMEModel.Reminder_2 = (int)drRow["Reminder_2"];

                        //added by diana 20140121 - store deposit
                        objPAYSCHEMEModel.Deposit_1 = (int)drRow["Deposit_1"];
                        objPAYSCHEMEModel.IsNominal_1 = (int)drRow["IsNominal_1"];
                        objPAYSCHEMEModel.LDeposit_11 = (int)drRow["LDeposit_11"];
                        objPAYSCHEMEModel.LDuration_11 = (int)drRow["LDuration_11"];
                        objPAYSCHEMEModel.LDeposit_12 = (int)drRow["LDeposit_12"];
                        objPAYSCHEMEModel.LDuration_12 = (int)drRow["LDuration_12"];
                        objPAYSCHEMEModel.LDeposit_13 = (int)drRow["LDeposit_13"];
                        objPAYSCHEMEModel.LDuration_13 = (int)drRow["LDuration_13"];
                        objPAYSCHEMEModel.PDeposit_11 = (int)drRow["PDeposit_11"];
                        objPAYSCHEMEModel.PDuration_11 = (int)drRow["PDuration_11"];
                        objPAYSCHEMEModel.PDeposit_12 = (int)drRow["PDeposit_12"];
                        objPAYSCHEMEModel.PDuration_12 = (int)drRow["PDuration_12"];
                        objPAYSCHEMEModel.PDeposit_13 = (int)drRow["PDeposit_13"];
                        objPAYSCHEMEModel.PDuration_13 = (int)drRow["PDuration_13"];
                        objPAYSCHEMEModel.Deposit_2 = (int)drRow["Deposit_2"];
                        objPAYSCHEMEModel.Deposit_3 = (int)drRow["Deposit_3"];

                        return objPAYSCHEMEModel;
                    }
                    else
                    {
                        return null;
                        throw new ApplicationException("PAYSCHEMEOTH does not exist.");
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
        public PaymentInfo SavePayment(PaymentInfo pInfo, string pGRPID, SaveType pSaveType, string TransID)
        {
            bool rValue = false;
            ArrayList lstSQL = new ArrayList();
            string strSQL = string.Empty;
            AUDITLOG AUDITLOGInfo = new AUDITLOG();
            GeneralControl AuditLogBase = new GeneralControl();
            List<AUDITLOG> lstAuditLog = new List<AUDITLOG>();
            try
            {
                PaymentInfo pyDetail = GetSinglePAYSCHEME(pInfo.SchemeCode, pGRPID, TransID);
                bool flag = true;
                if (pyDetail.Duration != pInfo.Duration || pyDetail.MinDuration != pInfo.MinDuration || pyDetail.Description != pInfo.Description)
                    flag = false;
                if (pyDetail.Attempt_1 != pInfo.Attempt_1 || pyDetail.Attempt_2 != pInfo.Attempt_2 || pyDetail.Attempt_3 != pInfo.Attempt_3)
                    flag = false;
                if (pyDetail.Percentage_1 != pInfo.Percentage_1 || pyDetail.Percentage_2 != pInfo.Percentage_2 || pyDetail.Percentage_3 != pInfo.Percentage_3)
                    flag = false;

                objSQL.AddField("SchemeCode", pInfo.SchemeCode, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("GRPID", pInfo.GRPID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Duration", pInfo.Duration, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("MinDuration", pInfo.MinDuration, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("FirstDeposit", pInfo.FirstDeposit, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                //objSQL.AddField("Description", pInfo.Description, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("PaymentType", pInfo.PaymentType, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Attempt_1", pInfo.Attempt_1, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Code_1", pInfo.Code_1, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Percentage_1", pInfo.Percentage_1, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Deposit_1", pInfo.Deposit_1, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone); //added by diana 20140121 - store deposit
                objSQL.AddField("IsNominal_1", pInfo.IsNominal_1, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LDeposit_11", pInfo.LDeposit_11, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LDuration_11", pInfo.LDuration_11, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LDeposit_12", pInfo.LDeposit_12, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LDuration_12", pInfo.LDuration_12, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LDeposit_13", pInfo.LDeposit_13, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LDuration_13", pInfo.LDuration_13, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("PDeposit_11", pInfo.PDeposit_11, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("PDuration_11", pInfo.PDuration_11, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("PDeposit_12", pInfo.PDeposit_12, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("PDuration_12", pInfo.PDuration_12, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("PDeposit_13", pInfo.PDeposit_13, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("PDuration_13", pInfo.PDuration_13, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Attempt_2", pInfo.Attempt_2, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Code_2", pInfo.Code_2, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Percentage_2", pInfo.Percentage_2, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone); //added by diana 20140121 - store deposit
                objSQL.AddField("Deposit_2", pInfo.Deposit_2, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Attempt_3", pInfo.Attempt_3, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Code_3", pInfo.Code_3, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Percentage_3", pInfo.Percentage_3, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Deposit_3", pInfo.Deposit_3, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone); //added by diana 20140121 - store deposit
                objSQL.AddField("PaymentMode", pInfo.PaymentMode, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("CreateBy", pInfo.CreateBy, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncCreate", pInfo.SyncCreate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("SyncLastUpd", pInfo.SyncLastUpd, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
                objSQL.AddField("LastSyncBy", pInfo.LastSyncBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Reminder_1", pInfo.Reminder_1, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                objSQL.AddField("Reminder_2", pInfo.Reminder_2, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
                switch (pSaveType)
                {
                    case SaveType.Insert:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert, "DEPOPAYSCHEME", string.Empty);
                        AUDITLOGInfo.Action = 0;
                        break;
                    case SaveType.Update:
                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "DEPOPAYSCHEME", "DEPOPAYSCHEME.SchemeCode='" + pInfo.SchemeCode + "'");
                        AUDITLOGInfo.Action = 1;
                        break;
                }
                lstSQL.Add(strSQL);

                AUDITLOGInfo.TransID = DateTime.Now.ToString("yyyyMMddHHmmsss"); ;
                AUDITLOGInfo.SeqNo = 0;
                AUDITLOGInfo.RefCode = "";
                AUDITLOGInfo.Table_Name = "DEPOPAYSCHEME";
                AUDITLOGInfo.SQL = strSQL;
                AUDITLOGInfo.CreatedBy = pInfo.LastSyncBy;
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

                return GetSinglePAYSCHEME(pInfo.SchemeCode, pGRPID, TransID);

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


// ------------------------------------------ Payment Control Backup ------------------------------------------------------
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using SEAL.Data;
//using SEAL.Model;
//using System.Data;
////using log4net;

//namespace ABS.Logic.GroupBooking.Booking
//{
//    public class PaymentInfo
//    {
//        private string _schemeCode = String.Empty;
//        private string _gRPID = String.Empty;
//        private int _minduration;
//        private int _duration;
//        private int _firstDeposit;
//        private string _description = String.Empty;
//        private string _paymentType = String.Empty;
//        private int _attempt_1;
//        private string _code_1;
//        private int _percentage_1;
//        private int _attempt_2;
//        private string _code_2;
//        private int _percentage_2;
//        private int _attempt_3;
//        private string _code_3;
//        private int _percentage_3;
//        private string _paymentMode = String.Empty;
//        private string _createBy = String.Empty;
//        private DateTime _syncCreate;
//        private DateTime _syncLastUpd;
//        private string _lastSyncBy = String.Empty;
//        private int _reminder_1;
//        private int _reminder_2;

//        //added by diana 20140121 - for deposit
//        private int _deposit_1;
//        private int _deposit_2;
//        private int _deposit_3;

//        #region Public Properties
//        public string SchemeCode
//        {
//            get { return _schemeCode; }
//            set { _schemeCode = value; }
//        }
//        public string GRPID
//        {
//            get { return _gRPID; }
//            set { _gRPID = value; }
//        }
//        public int MinDuration
//        {
//            get { return _minduration; }
//            set { _minduration = value; }
//        }
//        public int Duration
//        {
//            get { return _duration; }
//            set { _duration = value; }
//        }

//        public int FirstDeposit
//        {
//            get { return _firstDeposit; }
//            set { _firstDeposit = value; }
//        }

//        public string Description
//        {
//            get { return _description; }
//            set { _description = value; }
//        }

//        public string PaymentType
//        {
//            get { return _paymentType; }
//            set { _paymentType = value; }
//        }

//        public int Attempt_1
//        {
//            get { return _attempt_1; }
//            set { _attempt_1 = value; }
//        }

//        public string Code_1
//        {
//            get { return _code_1; }
//            set { _code_1 = value; }
//        }

//        public int Percentage_1
//        {
//            get { return _percentage_1; }
//            set { _percentage_1 = value; }
//        }

//        public int Attempt_2
//        {
//            get { return _attempt_2; }
//            set { _attempt_2 = value; }
//        }

//        public string Code_2
//        {
//            get { return _code_2; }
//            set { _code_2 = value; }
//        }

//        public int Percentage_2
//        {
//            get { return _percentage_2; }
//            set { _percentage_2 = value; }
//        }

//        public int Attempt_3
//        {
//            get { return _attempt_3; }
//            set { _attempt_3 = value; }
//        }

//        public string Code_3
//        {
//            get { return _code_3; }
//            set { _code_3 = value; }
//        }

//        public int Percentage_3
//        {
//            get { return _percentage_3; }
//            set { _percentage_3 = value; }
//        }

//        public string PaymentMode
//        {
//            get { return _paymentMode; }
//            set { _paymentMode = value; }
//        }

//        public string CreateBy
//        {
//            get { return _createBy; }
//            set { _createBy = value; }
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

//        public string LastSyncBy
//        {
//            get { return _lastSyncBy; }
//            set { _lastSyncBy = value; }
//        }

//        public int Reminder_1
//        {
//            get { return _reminder_1; }
//            set { _reminder_1 = value; }
//        }

//        public int Reminder_2
//        {
//            get { return _reminder_2; }
//            set { _reminder_2 = value; }
//        }

//        //added by diana 20140121 - for deposit purpose
//        public int Deposit_1
//        {
//            get { return _deposit_1; }
//            set { _deposit_1 = value; }
//        }

//        public int Deposit_2
//        {
//            get { return _deposit_2; }
//            set { _deposit_2 = value; }
//        }

//        public int Deposit_3
//        {
//            get { return _deposit_3; }
//            set { _deposit_3 = value; }
//        }

//        #endregion
//    }

//    public class PaymentControl : Shared.CoreBase
//    {
//        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
//        LogControl log = new LogControl();

//        public enum SaveType
//        {
//            Insert = 0,
//            Update = 1
//        }

//        public string GetScheme(DateTime STD, DateTime bookingDate, string GRPID)
//        {
//            //DateTime todays = DateTime.Now;
//            string Scheme = "";
//            List<PaymentInfo> paymentScheme = new List<PaymentInfo>();
//            paymentScheme = GetAllPaymentScheme();

//            //replace aax to aa 
//            //GRPID = "AA"; //remarked by diana 20140120

//            var rows = paymentScheme.Where(item => item.GRPID.Equals(GRPID));

//            //int days = STD.Subtract(todays).Days;
//            //int hours = STD.Subtract(todays).Hours;
//            int days = STD.Subtract(bookingDate).Days;
//            int hours = STD.Subtract(bookingDate).Hours + (days * 24);

//            foreach (var row in rows)
//            {
//                if (row.Duration == 0)
//                {
//                    if (hours > row.MinDuration)
//                    {
//                        return row.SchemeCode;
//                    }
//                }
//                else
//                {
//                    if (row.MinDuration == 0)
//                    {
//                        if (hours < row.Duration)
//                        {
//                            return row.SchemeCode;
//                        }
//                    }
//                    else
//                    {
//                        if (hours <= row.Duration && hours >= row.MinDuration)
//                        {
//                            return row.SchemeCode;
//                        }
//                    }
//                }

//            }
//            return "";

//            if (GRPID == "AA" || GRPID == "AX") //amended by diana 20140120 - if (GRPID == "AA" || GRPID == "AX" || GRPID == "AAX")
//            {
//                if (days > 60)
//                {
//                    Scheme = "B2M";
//                }
//                else if (days <= 60 && days > 30)
//                {
//                    Scheme = "W2M";
//                }
//                else if (days <= 30 && days > 7)
//                {
//                    Scheme = "W1M";
//                }
//                else
//                {
//                    Scheme = "W1W";
//                }
//            }
//            else
//            {
//                if (GRPID == "AAX")
//                {
//                    if (days > 90)
//                    {
//                        Scheme = "B3M";
//                    }
//                    else if (days <= 90 && days > 30)
//                    {
//                        Scheme = "W3M";
//                    }
//                    else if (days <= 30 && days > 7)
//                    {
//                        Scheme = "W1M";
//                    }
//                    else
//                    {
//                        Scheme = "W1W";
//                    }
//                }
//            }

//            return Scheme;
//        }

//        public PaymentInfo GetPaymentScheme(string SchemeCode, string GRPID)
//        {
//            PaymentInfo paymentInfo = new PaymentInfo();
//            //GRPID = "AA"; //remarked by diana 20140120
//            paymentInfo = GetSinglePAYSCHEME(SchemeCode, GRPID);
//            if (paymentInfo != null)
//                return paymentInfo;
//            else 
//                return null;
//            //replace AAX to AA


//            //if (GRPID == "AA")
//            //{
//            //    switch (SchemeCode.ToUpper())
//            //    {
//            //        case "B2M":
//            //            paymentInfo.Attempt_1 = 24;
//            //            paymentInfo.Attempt_2 = 720;
//            //            paymentInfo.Attempt_3 = 720;
//            //            paymentInfo.Duration = 1440;
//            //            paymentInfo.FirstDeposit = 0;
//            //            paymentInfo.PaymentType = "SVCF";
//            //            paymentInfo.Percentage_1 = 30;
//            //            paymentInfo.Percentage_2 = 20;
//            //            paymentInfo.Percentage_3 = 50;
//            //            paymentInfo.PaymentMode = "";
//            //            break;

//            //        case "W2M":
//            //            paymentInfo.Attempt_1 = 24;
//            //            paymentInfo.Attempt_2 = 720;
//            //            paymentInfo.Attempt_3 = 0;
//            //            paymentInfo.Duration = 1440;
//            //            paymentInfo.FirstDeposit = 0;
//            //            paymentInfo.PaymentType = "SVCF";
//            //            paymentInfo.Percentage_1 = 50;
//            //            paymentInfo.Percentage_2 = 50;
//            //            paymentInfo.Percentage_3 = 0;
//            //            paymentInfo.PaymentMode = "";
//            //            break;

//            //        case "W1M":
//            //            paymentInfo.Attempt_1 = 24;
//            //            paymentInfo.Attempt_2 = 0;
//            //            paymentInfo.Attempt_3 = 0;
//            //            paymentInfo.Duration = 720;
//            //            paymentInfo.FirstDeposit = 0;
//            //            paymentInfo.PaymentType = "SVCF";
//            //            paymentInfo.Percentage_1 = 100;
//            //            paymentInfo.Percentage_2 = 0;
//            //            paymentInfo.Percentage_3 = 0;
//            //            paymentInfo.PaymentMode = "";
//            //            break;

//            //        case "W1W":
//            //            paymentInfo.Attempt_1 = 0;
//            //            paymentInfo.Attempt_2 = 0;
//            //            paymentInfo.Attempt_3 = 0;
//            //            paymentInfo.Duration = 210;
//            //            paymentInfo.FirstDeposit = 0;
//            //            paymentInfo.PaymentType = "FULL";
//            //            paymentInfo.Percentage_1 = 0;
//            //            paymentInfo.Percentage_2 = 0;
//            //            paymentInfo.Percentage_3 = 0;
//            //            paymentInfo.PaymentMode = "";
//            //            break;
//            //    }
//            //}
//            //else
//            //{
//            //    if (GRPID == "AAX")
//            //    {
//            //        switch (SchemeCode.ToUpper())
//            //        {
//            //            case "XB3M":
//            //                paymentInfo.Attempt_1 = 720;
//            //                paymentInfo.Attempt_2 = 0;
//            //                paymentInfo.Attempt_3 = 0;
//            //                paymentInfo.Duration = 2160;
//            //                paymentInfo.FirstDeposit = 0;
//            //                paymentInfo.PaymentType = "SVCF";
//            //                paymentInfo.Percentage_1 = 100;
//            //                paymentInfo.Percentage_2 = 0;
//            //                paymentInfo.Percentage_3 = 0;
//            //                paymentInfo.PaymentMode = "";
//            //                break;

//            //            case "XW3M":
//            //                paymentInfo.Attempt_1 = 720;
//            //                paymentInfo.Attempt_2 = 0;
//            //                paymentInfo.Attempt_3 = 0;
//            //                paymentInfo.Duration = 2160;
//            //                paymentInfo.FirstDeposit = 0;
//            //                paymentInfo.PaymentType = "SVCF";
//            //                paymentInfo.Percentage_1 = 100;
//            //                paymentInfo.Percentage_2 = 0;
//            //                paymentInfo.Percentage_3 = 0;
//            //                paymentInfo.PaymentMode = "";
//            //                break;

//            //            case "XW1M":
//            //                paymentInfo.Attempt_1 = 48;
//            //                paymentInfo.Attempt_2 = 0;
//            //                paymentInfo.Attempt_3 = 0;
//            //                paymentInfo.Duration = 720;
//            //                paymentInfo.FirstDeposit = 0;
//            //                paymentInfo.PaymentType = "SVCF";
//            //                paymentInfo.Percentage_1 = 100;
//            //                paymentInfo.Percentage_2 = 0;
//            //                paymentInfo.Percentage_3 = 0;
//            //                paymentInfo.PaymentMode = "";
//            //                break;

//            //            case "XW1W":
//            //                paymentInfo.Attempt_1 = 24;
//            //                paymentInfo.Attempt_2 = 0;
//            //                paymentInfo.Attempt_3 = 0;
//            //                paymentInfo.Duration = 210;
//            //                paymentInfo.FirstDeposit = 0;
//            //                paymentInfo.PaymentType = "SVCF";
//            //                paymentInfo.Percentage_1 = 100;
//            //                paymentInfo.Percentage_2 = 0;
//            //                paymentInfo.Percentage_3 = 0;
//            //                paymentInfo.PaymentMode = "";
//            //                break;
//            //        }
//            //    }
//            //}

//            return paymentInfo;
//        }

//        public DateTime GetExpiryDate(string Scheme, string GRPID, DateTime STD, DateTime BookingDate)
//        {
//            PaymentInfo paymentInfo = new PaymentInfo();
//            //replace AAX to AA
//            //GRPID = "AA";
//            paymentInfo = GetPaymentScheme(Scheme, GRPID);
//            if (paymentInfo != null)
//            {
//                //if (GRPID == "AA") //added by diana 20140120 - add condition for AA/AAX
//                //{
//                    /// amended by diana 20130913
//                    if (paymentInfo.Attempt_3 != 0 && paymentInfo.Percentage_3 != 0)
//                    {
//                        if (paymentInfo.Code_3 == "DOB")
//                            return BookingDate.AddHours(paymentInfo.Attempt_3);
//                        else if (paymentInfo.Code_3 == "STD")
//                            return STD.AddHours(-paymentInfo.Attempt_3);
//                    }
//                    //added by diana 20140121 - check for deposit
//                    else if (paymentInfo.Attempt_3 != 0 && paymentInfo.Deposit_3 != 0)
//                    {
//                        if (paymentInfo.Code_3 == "DOB")
//                            return BookingDate.AddHours(paymentInfo.Attempt_3);
//                        else if (paymentInfo.Code_3 == "STD")
//                            return STD.AddHours(-paymentInfo.Attempt_3);
//                    }
//                    else if (paymentInfo.Attempt_2 != 0 && paymentInfo.Percentage_2 != 0)
//                    {
//                        if (paymentInfo.Code_2 == "DOB")
//                            return BookingDate.AddHours(paymentInfo.Attempt_2);
//                        else if (paymentInfo.Code_2 == "STD")
//                            return STD.AddHours(-paymentInfo.Attempt_2);
//                    }
//                    //added by diana 20140121 - check for deposit
//                    else if (paymentInfo.Attempt_2 != 0 && paymentInfo.Deposit_3 != 0)
//                    {
//                        if (paymentInfo.Code_2 == "DOB")
//                            return BookingDate.AddHours(paymentInfo.Attempt_2);
//                        else if (paymentInfo.Code_2 == "STD")
//                            return STD.AddHours(-paymentInfo.Attempt_2);
//                    }
//                    else
//                    {
//                        if (paymentInfo.Code_1 == "DOB")
//                            return BookingDate.AddHours(paymentInfo.Attempt_1);
//                        else if (paymentInfo.Code_1 == "STD")
//                            return STD.AddHours(-paymentInfo.Attempt_1);
//                    }
//                    return STD.AddHours(-48);
//                //}
//                //else if (GRPID == "AAX")
//                //{
//                //    if (paymentInfo.Attempt_3 != 0 && paymentInfo.Percentage_3 != 0)
//                //    {
//                //        if (paymentInfo.Code_3 == "DOB")
//                //            return BookingDate.AddHours(paymentInfo.Attempt_3);
//                //        else if (paymentInfo.Code_3 == "STD")
//                //            return STD.AddHours(-paymentInfo.Attempt_3);
//                //    }
//                //    else if (paymentInfo.Attempt_2 != 0 && paymentInfo.Percentage_2 != 0)
//                //    {
//                //        if (paymentInfo.Code_2 == "DOB")
//                //            return BookingDate.AddHours(paymentInfo.Attempt_2);
//                //        else if (paymentInfo.Code_2 == "STD")
//                //            return STD.AddHours(-paymentInfo.Attempt_2);
//                //    }
//                //    else
//                //    {
//                //        if (paymentInfo.Code_1 == "DOB")
//                //            return BookingDate.AddHours(paymentInfo.Attempt_1);
//                //        else if (paymentInfo.Code_1 == "STD")
//                //            return STD.AddHours(-paymentInfo.Attempt_1);
//                //    }
//                //    return STD.AddHours(-48);
//                //}
//                //if (GRPID == "AA")
//                //{
//                //    switch (Scheme.ToUpper())
//                //    {
//                //        case "B2M":
//                //            return STD.AddHours(-paymentInfo.Attempt_3);

//                //        case "W2M":
//                //            return STD.AddHours(-paymentInfo.Attempt_2);
//                //        case "W1M":
//                //            return BookingDate.AddHours(paymentInfo.Attempt_1);

//                //        case "W1W":
//                //            return BookingDate.AddHours(paymentInfo.Attempt_1);
//                //        default:
//                //            return STD.AddHours(-48);
//                //    }
//                //}
//                //else
//                //{
//                //    if (GRPID == "AAX")
//                //    {
//                //        switch (Scheme.ToUpper())
//                //        {
//                //            case "XB3M":
//                //                return STD.AddHours(-2160);

//                //            case "XW3M":
//                //                return STD.AddHours(-2160);

//                //            case "XW1M":
//                //                return BookingDate.AddHours(48);

//                //            case "XW1W":
//                //                return BookingDate.AddHours(24);
//                //            default:
//                //                return STD.AddHours(-48);
//                //        }
//                //    }
//                //}
//            }
//            return STD.AddHours(-48);


//        }

//        public List <PaymentInfo> GetAllPaymentScheme()
//        {
//            PaymentInfo objPAYSCHEMEModel;
//            List<PaymentInfo> objListPAYSCHEMEModel = new List<PaymentInfo>();
//            DataTable dt;
//            DateTime dateValue;
//            String strSQL = string.Empty;

//            try
//            {
//                strSQL = "SELECT * FROM PAYSCHEME ";
//                log.Info(this, strSQL);
//                dt = objDCom.Execute(strSQL, CommandType.Text, true);
//                if (dt != null && dt.Rows.Count > 0)
//                {

//                    //to log columns
//                    string colName = "";
//                    foreach (DataColumn col in dt.Columns)
//                    {
//                        colName += col.ColumnName + ";";
//                    }
//                    log.Info(this, colName);
//                    //to log columns

//                    foreach (DataRow drRow in dt.Rows)
//                    {
//                        objPAYSCHEMEModel = new PaymentInfo();
//                        objPAYSCHEMEModel.SchemeCode = (string)drRow["SchemeCode"];
//                        objPAYSCHEMEModel.GRPID = (string)drRow["GRPID"];
//                        objPAYSCHEMEModel.Duration = (int)drRow["Duration"];
//                        objPAYSCHEMEModel.MinDuration = (int)drRow["MinDuration"];
//                        objPAYSCHEMEModel.FirstDeposit = (int)drRow["FirstDeposit"];
//                        objPAYSCHEMEModel.Description = (string)drRow["Description"];
//                        objPAYSCHEMEModel.PaymentType = (string)drRow["PaymentType"];
//                        objPAYSCHEMEModel.Attempt_1 = (int)drRow["Attempt_1"];
//                        objPAYSCHEMEModel.Code_1 = (string)drRow["Code_1"];
//                        objPAYSCHEMEModel.Percentage_1 = (int)drRow["Percentage_1"];
//                        objPAYSCHEMEModel.Attempt_2 = (int)drRow["Attempt_2"];
//                        objPAYSCHEMEModel.Code_2 = (string)drRow["Code_2"];
//                        objPAYSCHEMEModel.Percentage_2 = (int)drRow["Percentage_2"];
//                        objPAYSCHEMEModel.Attempt_3 = (int)drRow["Attempt_3"];
//                        objPAYSCHEMEModel.Code_3 = (string)drRow["Code_3"];
//                        objPAYSCHEMEModel.Percentage_3 = (int)drRow["Percentage_3"];
//                        objPAYSCHEMEModel.PaymentMode = (string)drRow["PaymentMode"];
//                        objPAYSCHEMEModel.CreateBy = (string)drRow["CreateBy"];
//                        if (DateTime.TryParse(drRow["SyncCreate"].ToString(), out dateValue)) objPAYSCHEMEModel.SyncCreate = (DateTime)drRow["SyncCreate"];
//                        if (DateTime.TryParse(drRow["SyncLastUpd"].ToString(), out dateValue)) objPAYSCHEMEModel.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
//                        objPAYSCHEMEModel.LastSyncBy = (string)drRow["LastSyncBy"];
//                        objPAYSCHEMEModel.Reminder_1 = (int)drRow["Reminder_1"];
//                        objPAYSCHEMEModel.Reminder_2 = (int)drRow["Reminder_2"];

//                        //added by diana 20140121 - store deposit
//                        objPAYSCHEMEModel.Deposit_1 = (int)drRow["Deposit_1"];
//                        objPAYSCHEMEModel.Deposit_2 = (int)drRow["Deposit_2"];
//                        objPAYSCHEMEModel.Deposit_3 = (int)drRow["Deposit_3"];

//                        objListPAYSCHEMEModel.Add(objPAYSCHEMEModel);
//                    }
//                    return objListPAYSCHEMEModel;
//                }
//                else
//                {
//                    return null;
//                    throw new ApplicationException("PAYSCHEME does not exist.");
//                }
//            }
//            catch (Exception ex)
//            {
//                log.Error(this,ex);
//                return null;
//            }
//        }

//        public DataTable GetAllScheme(string pGRPID)
//        {
//            DataTable dt;
//            String strSQL = string.Empty;
//            String strFields = string.Empty;
//            String strFilter = string.Empty;
//            List<string> lstFields = new List<string>();
//            try
//            {
//                lstFields.Add("PAYSCHEME.SchemeCode");
//                lstFields.Add("PAYSCHEME.GRPID");
//                lstFields.Add("PAYSCHEME.Duration");
//                lstFields.Add("PAYSCHEME.MinDuration");
//                lstFields.Add("PAYSCHEME.FirstDeposit");
//                lstFields.Add("PAYSCHEME.Description");
//                lstFields.Add("PAYSCHEME.PaymentType");
//                lstFields.Add("PAYSCHEME.Attempt_1");
//                lstFields.Add("PAYSCHEME.Code_1");
//                lstFields.Add("PAYSCHEME.Percentage_1");
//                lstFields.Add("PAYSCHEME.Attempt_2");
//                lstFields.Add("PAYSCHEME.Code_2");
//                lstFields.Add("PAYSCHEME.Percentage_2");
//                lstFields.Add("PAYSCHEME.Attempt_3");
//                lstFields.Add("PAYSCHEME.Code_3");
//                lstFields.Add("PAYSCHEME.Percentage_3");
//                lstFields.Add("PAYSCHEME.PaymentMode");
//                lstFields.Add("PAYSCHEME.CreateBy");
//                lstFields.Add("PAYSCHEME.SyncCreate");
//                lstFields.Add("PAYSCHEME.SyncLastUpd");
//                lstFields.Add("PAYSCHEME.LastSyncBy");
//                lstFields.Add("PAYSCHEME.Reminder_1");
//                lstFields.Add("PAYSCHEME.Reminder_2");

//                //added by diana 20140121 - store deposit
//                lstFields.Add("PAYSCHEME.Deposit_1");
//                lstFields.Add("PAYSCHEME.Deposit_2");
//                lstFields.Add("PAYSCHEME.Deposit_3");

//                strFields = GetSqlFields(lstFields);
//                strFilter = "WHERE PAYSCHEME.GRPID='" + pGRPID + "'";
//                strSQL = "SELECT " + strFields + " FROM PAYSCHEME " + strFilter + " ORDER BY MinDuration DESC";
//                dt = objDCom.Execute(strSQL, CommandType.Text, true);

//                if (dt != null && dt.Rows.Count > 0)
//                {
//                    return dt;
//                }
//                else
//                {
//                    return null;
//                    throw new ApplicationException("PAYSCHEME does not exist.");
//                }
//            }
//            catch (Exception ex)
//            {
//                log.Error(this,ex);
//                return null;
//            }
//        }

//        public PaymentInfo GetSinglePAYSCHEME(string pSchemeCode, string pGRPID)
//        {
//            PaymentInfo objPAYSCHEMEModel;
//            DataTable dt;
//            DateTime dateValue;
//            String strSQL = string.Empty;
//            String strFields = string.Empty;
//            String strFilter = string.Empty;
//            List<string> lstFields = new List<string>();
//            try
//            {
//                lstFields.Add("PAYSCHEME.SchemeCode");
//                lstFields.Add("PAYSCHEME.GRPID");
//                lstFields.Add("PAYSCHEME.Duration");
//                lstFields.Add("PAYSCHEME.MinDuration");
//                lstFields.Add("PAYSCHEME.FirstDeposit");
//                lstFields.Add("PAYSCHEME.Description");
//                lstFields.Add("PAYSCHEME.PaymentType");
//                lstFields.Add("PAYSCHEME.Attempt_1");
//                lstFields.Add("PAYSCHEME.Code_1");
//                lstFields.Add("PAYSCHEME.Percentage_1");
//                lstFields.Add("PAYSCHEME.Attempt_2");
//                lstFields.Add("PAYSCHEME.Code_2");
//                lstFields.Add("PAYSCHEME.Percentage_2");
//                lstFields.Add("PAYSCHEME.Attempt_3");
//                lstFields.Add("PAYSCHEME.Code_3");
//                lstFields.Add("PAYSCHEME.Percentage_3");
//                lstFields.Add("PAYSCHEME.PaymentMode");
//                lstFields.Add("PAYSCHEME.CreateBy");
//                lstFields.Add("PAYSCHEME.SyncCreate");
//                lstFields.Add("PAYSCHEME.SyncLastUpd");
//                lstFields.Add("PAYSCHEME.LastSyncBy");
//                lstFields.Add("PAYSCHEME.Reminder_1");
//                lstFields.Add("PAYSCHEME.Reminder_2");

//                //added by diana 20140121 - define whether have to pay for deposit or not
//                lstFields.Add("PAYSCHEME.Deposit_1");
//                lstFields.Add("PAYSCHEME.Deposit_2");
//                lstFields.Add("PAYSCHEME.Deposit_3");

//                strFields = GetSqlFields(lstFields);
//                strFilter = "Where PAYSCHEME.GRPID='" + pGRPID + "' AND PAYSCHEME.SchemeCode='" + pSchemeCode + "'";
//                strSQL = "SELECT " + strFields + " FROM PAYSCHEME " + strFilter + " ORDER BY MinDuration DESC";
//                dt = objDCom.Execute(strSQL, CommandType.Text, true);

//                if (dt != null && dt.Rows.Count > 0)
//                {
//                    DataRow drRow = dt.Rows[0];

//                    objPAYSCHEMEModel = new PaymentInfo();
//                    objPAYSCHEMEModel.SchemeCode = (string)drRow["SchemeCode"];
//                    objPAYSCHEMEModel.GRPID = (string)drRow["GRPID"];
//                    objPAYSCHEMEModel.Duration = (int)drRow["Duration"];
//                    objPAYSCHEMEModel.MinDuration = (int)drRow["MinDuration"];
//                    objPAYSCHEMEModel.FirstDeposit = (int)drRow["FirstDeposit"];
//                    objPAYSCHEMEModel.Description = (string)drRow["Description"];
//                    objPAYSCHEMEModel.PaymentType = (string)drRow["PaymentType"];
//                    objPAYSCHEMEModel.Attempt_1 = (int)drRow["Attempt_1"];
//                    objPAYSCHEMEModel.Code_1 = (string)drRow["Code_1"];
//                    objPAYSCHEMEModel.Percentage_1 = (int)drRow["Percentage_1"];
//                    objPAYSCHEMEModel.Attempt_2 = (int)drRow["Attempt_2"];
//                    objPAYSCHEMEModel.Code_2 = (string)drRow["Code_2"];
//                    objPAYSCHEMEModel.Percentage_2 = (int)drRow["Percentage_2"];
//                    objPAYSCHEMEModel.Attempt_3 = (int)drRow["Attempt_3"];
//                    objPAYSCHEMEModel.Code_3 = (string)drRow["Code_3"];
//                    objPAYSCHEMEModel.Percentage_3 = (int)drRow["Percentage_3"];
//                    objPAYSCHEMEModel.PaymentMode = (string)drRow["PaymentMode"];
//                    objPAYSCHEMEModel.CreateBy = (string)drRow["CreateBy"];
//                    if (DateTime.TryParse(drRow["SyncCreate"].ToString(), out dateValue)) objPAYSCHEMEModel.SyncCreate = (DateTime)drRow["SyncCreate"];
//                    if (DateTime.TryParse(drRow["SyncLastUpd"].ToString(), out dateValue)) objPAYSCHEMEModel.SyncLastUpd = (DateTime)drRow["SyncLastUpd"];
//                    objPAYSCHEMEModel.LastSyncBy = (string)drRow["LastSyncBy"];
//                    objPAYSCHEMEModel.Reminder_1 = (int)drRow["Reminder_1"];
//                    objPAYSCHEMEModel.Reminder_2 = (int)drRow["Reminder_2"];

//                    //added by diana 20140121 - store deposit
//                    objPAYSCHEMEModel.Deposit_1 = (int)drRow["Deposit_1"];
//                    objPAYSCHEMEModel.Deposit_2 = (int)drRow["Deposit_2"];
//                    objPAYSCHEMEModel.Deposit_3 = (int)drRow["Deposit_3"];

//                    return objPAYSCHEMEModel;
//                }
//                else
//                {
//                    return null;
//                    throw new ApplicationException("PAYSCHEME does not exist.");
//                }
//            }
//            catch (Exception ex)
//            {
//                log.Error(this,ex);
//                return null;
//            }
//        }

//        public PaymentInfo SavePayment(PaymentInfo pInfo, string pGRPID, SaveType pSaveType)
//        {
//            bool rValue = false;
//            ArrayList lstSQL = new ArrayList();
//            string strSQL = string.Empty;
//            AUDITLOG AUDITLOGInfo = new AUDITLOG();
//            GeneralControl AuditLogBase = new GeneralControl();
//            List<AUDITLOG> lstAuditLog = new List<AUDITLOG>();
//            try
//            {
//                PaymentInfo pyDetail = GetSinglePAYSCHEME(pInfo.SchemeCode,pGRPID);
//                bool flag = true;
//                if (pyDetail.Duration != pInfo.Duration || pyDetail.MinDuration != pInfo.MinDuration || pyDetail.Description != pInfo.Description)
//                    flag = false;
//                if (pyDetail.Attempt_1 != pInfo.Attempt_1 || pyDetail.Attempt_2 != pInfo.Attempt_2 || pyDetail.Attempt_3 != pInfo.Attempt_3)
//                    flag = false;
//                if (pyDetail.Percentage_1 != pInfo.Percentage_1 || pyDetail.Percentage_2 != pInfo.Percentage_2 || pyDetail.Percentage_3 != pInfo.Percentage_3)
//                    flag = false;

//                objSQL.AddField("SchemeCode", pInfo.SchemeCode, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("GRPID", pInfo.GRPID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("Duration", pInfo.Duration, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("MinDuration", pInfo.MinDuration, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("FirstDeposit", pInfo.FirstDeposit, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
//                //objSQL.AddField("Description", pInfo.Description, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("PaymentType", pInfo.PaymentType, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("Attempt_1", pInfo.Attempt_1, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("Code_1", pInfo.Code_1, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("Percentage_1", pInfo.Percentage_1, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("Deposit_1", pInfo.Deposit_1, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone); //added by diana 20140121 - store deposit
//                objSQL.AddField("Attempt_2", pInfo.Attempt_2, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("Code_2", pInfo.Code_2, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("Percentage_2", pInfo.Percentage_2, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone); //added by diana 20140121 - store deposit
//                objSQL.AddField("Deposit_2", pInfo.Deposit_2, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("Attempt_3", pInfo.Attempt_3, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("Code_3", pInfo.Code_3, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("Percentage_3", pInfo.Percentage_3, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("Deposit_3", pInfo.Deposit_3, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone); //added by diana 20140121 - store deposit
//                objSQL.AddField("PaymentMode", pInfo.PaymentMode, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("CreateBy", pInfo.CreateBy, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("SyncCreate", pInfo.SyncCreate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("SyncLastUpd", pInfo.SyncLastUpd, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("LastSyncBy", pInfo.LastSyncBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("Reminder_1", pInfo.Reminder_1, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
//                objSQL.AddField("Reminder_2", pInfo.Reminder_2, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone);
//                switch (pSaveType)
//                {
//                    case SaveType.Insert:
//                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert, "PAYSCHEME", string.Empty);
//                        AUDITLOGInfo.Action = 0;
//                        break;
//                    case SaveType.Update:
//                        strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "PAYSCHEME", "PAYSCHEME.SchemeCode='" + pInfo.SchemeCode + "'");
//                        AUDITLOGInfo.Action = 1;
//                        break;
//                }
//                lstSQL.Add(strSQL);

//                AUDITLOGInfo.TransID = DateTime.Now.ToString("yyyyMMddHHmmsss"); ;
//                AUDITLOGInfo.SeqNo = 0;
//                AUDITLOGInfo.RefCode = "";
//                AUDITLOGInfo.Table_Name = "PAYSCHEME";
//                AUDITLOGInfo.SQL = strSQL;
//                AUDITLOGInfo.CreatedBy = pInfo.LastSyncBy;
//                AUDITLOGInfo.CreatedDate = DateTime.Now;
//                AUDITLOGInfo.Priority = 0;
//                lstAuditLog.Add(AUDITLOGInfo);

//                rValue = objDCom.BatchExecute(lstSQL, CommandType.Text, true, false);
//                if (rValue == false)
//                {
//                    return null;
//                }
//                if (flag == false)
//                    AuditLogBase.SaveSYS_AUDITLOG(lstAuditLog);

//                return GetSinglePAYSCHEME(pInfo.SchemeCode,pGRPID);

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
//    }
//}

