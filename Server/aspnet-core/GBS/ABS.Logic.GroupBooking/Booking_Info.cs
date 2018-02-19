using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public class Booking_Info
    {
        private int _booking_ID;

        private string _booking_PNR = String.Empty;
        private int _booking_TranID;
        private string _booking_Currency = String.Empty;
        private string _booking_CarrierCode = String.Empty;
        private DateTime _booking_Depart;
        private string _booking_Origin = String.Empty;
        private string _booking_Destination = String.Empty;
        private string _booking_FlightNo = String.Empty;
        private string _booking_FlightClass = String.Empty;
        private decimal _booking_FlightPrice;
        private int _booking_PaxCounts;
        private decimal _booking_APT;
        private decimal _booking_FUE;
        private decimal _booking_Baggage;
        private int _booking_IsChange;
        private DateTime _booking_CreateDate;
        private string _booking_CreateBy = String.Empty;
        private DateTime _booking_ModifyDate;
        private string _booking_ModifyBy = String.Empty;

        #region Public Properties
        public int Booking_ID
        {
            get { return _booking_ID; }
            set { _booking_ID = value; }
        }
        public string Booking_PNR
        {
            get { return _booking_PNR; }
            set { _booking_PNR = value; }
        }

        public int Booking_TranID
        {
            get { return _booking_TranID; }
            set { _booking_TranID = value; }
        }

        public string Booking_Currency
        {
            get { return _booking_Currency; }
            set { _booking_Currency = value; }
        }

        public string Booking_CarrierCode
        {
            get { return _booking_CarrierCode; }
            set { _booking_CarrierCode = value; }
        }

        public DateTime Booking_Depart
        {
            get { return _booking_Depart; }
            set { _booking_Depart = value; }
        }

        public string Booking_Origin
        {
            get { return _booking_Origin; }
            set { _booking_Origin = value; }
        }

        public string Booking_Destination
        {
            get { return _booking_Destination; }
            set { _booking_Destination = value; }
        }

        public string Booking_FlightNo
        {
            get { return _booking_FlightNo; }
            set { _booking_FlightNo = value; }
        }

        public string Booking_FlightClass
        {
            get { return _booking_FlightClass; }
            set { _booking_FlightClass = value; }
        }

        public decimal Booking_FlightPrice
        {
            get { return _booking_FlightPrice; }
            set { _booking_FlightPrice = value; }
        }

        public int Booking_PaxCounts
        {
            get { return _booking_PaxCounts; }
            set { _booking_PaxCounts = value; }
        }

        public decimal Booking_APT
        {
            get { return _booking_APT; }
            set { _booking_APT = value; }
        }

        public decimal Booking_FUE
        {
            get { return _booking_FUE; }
            set { _booking_FUE = value; }
        }

        public decimal Booking_Baggage
        {
            get { return _booking_Baggage; }
            set { _booking_Baggage = value; }
        }

        public int Booking_IsChange
        {
            get { return _booking_IsChange; }
            set { _booking_IsChange = value; }
        }

        public DateTime Booking_CreateDate
        {
            get { return _booking_CreateDate; }
            set { _booking_CreateDate = value; }
        }

        public string Booking_CreateBy
        {
            get { return _booking_CreateBy; }
            set { _booking_CreateBy = value; }
        }

        public DateTime Booking_ModifyDate
        {
            get { return _booking_ModifyDate; }
            set { _booking_ModifyDate = value; }
        }

        public string Booking_ModifyBy
        {
            get { return _booking_ModifyBy; }
            set { _booking_ModifyBy = value; }
        }
        #endregion

    }

    

