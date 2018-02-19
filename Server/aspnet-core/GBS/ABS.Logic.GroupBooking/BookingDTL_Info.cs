using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    public class BookingDTL_Info
    {
        private string _recordLocator = String.Empty;
        private int _transID;
        private byte _seqNo;

        private string _currency = String.Empty;
        private string _carrierCode = String.Empty;
        private string _flightNo = String.Empty;
        private DateTime _depatureDate;
        private string _origin = String.Empty;
        private string _destination = String.Empty;
        private string _flightClass = String.Empty;
        private int _paxCounts;
        private decimal _flightFare;
        private byte _status;
        private Guid _rowguid = Guid.Empty;
        private string _createBy = String.Empty;
        private DateTime _syncCreate;
        private DateTime _syncLastUpd;
        private string _lastSyncBy = String.Empty;

        #region Public Properties
        public string RecordLocator
        {
            get { return _recordLocator; }
            set { _recordLocator = value; }
        }
        public int TransID
        {
            get { return _transID; }
            set { _transID = value; }
        }
        public byte SeqNo
        {
            get { return _seqNo; }
            set { _seqNo = value; }
        }
        public string Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }

        public string CarrierCode
        {
            get { return _carrierCode; }
            set { _carrierCode = value; }
        }

        public string FlightNo
        {
            get { return _flightNo; }
            set { _flightNo = value; }
        }

        public DateTime DepatureDate
        {
            get { return _depatureDate; }
            set { _depatureDate = value; }
        }

        public string Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        public string Destination
        {
            get { return _destination; }
            set { _destination = value; }
        }

        public string FlightClass
        {
            get { return _flightClass; }
            set { _flightClass = value; }
        }

        public int PaxCounts
        {
            get { return _paxCounts; }
            set { _paxCounts = value; }
        }

        public decimal FlightFare
        {
            get { return _flightFare; }
            set { _flightFare = value; }
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
        #endregion

    }

