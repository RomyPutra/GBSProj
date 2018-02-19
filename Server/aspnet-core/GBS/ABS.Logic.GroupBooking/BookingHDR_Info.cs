using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public class BookingHDR_Info
    {
        private string _recordLocator = String.Empty;
        private int _transID;

        private DateTime _bookingDate;
        private DateTime _expiryDate;
        private decimal _collectedFare;
        private decimal _totalFare;
        private decimal _flightFare;
        private decimal _aPTFare;
        private decimal _fUEFare;
        private decimal _baggageFare;
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
        public DateTime BookingDate
        {
            get { return _bookingDate; }
            set { _bookingDate = value; }
        }

        public DateTime ExpiryDate
        {
            get { return _expiryDate; }
            set { _expiryDate = value; }
        }

        public decimal CollectedFare
        {
            get { return _collectedFare; }
            set { _collectedFare = value; }
        }

        public decimal TotalFare
        {
            get { return _totalFare; }
            set { _totalFare = value; }
        }

        public decimal FlightFare
        {
            get { return _flightFare; }
            set { _flightFare = value; }
        }

        public decimal APTFare
        {
            get { return _aPTFare; }
            set { _aPTFare = value; }
        }

        public decimal FUEFare
        {
            get { return _fUEFare; }
            set { _fUEFare = value; }
        }

        public decimal BaggageFare
        {
            get { return _baggageFare; }
            set { _baggageFare = value; }
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

