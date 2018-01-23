using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public class BK_TENDER_Info
    {
        private string _tenderID = String.Empty;

        private byte _tenderType;
        private string _tenderDesc = String.Empty;
        private byte _active;
        private byte _flag;
        private Guid _rowguid = Guid.Empty;
        private DateTime _effDate;
        private DateTime _endDate;
        private DateTime _syncCreate;
        private DateTime _syncLastUpd;
        private string _lastSyncBy = String.Empty;

        #region Public Properties
        public string TenderID
        {
            get { return _tenderID; }
            set { _tenderID = value; }
        }
        public byte TenderType
        {
            get { return _tenderType; }
            set { _tenderType = value; }
        }

        public string TenderDesc
        {
            get { return _tenderDesc; }
            set { _tenderDesc = value; }
        }

        public byte Active
        {
            get { return _active; }
            set { _active = value; }
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

        public DateTime EffDate
        {
            get { return _effDate; }
            set { _effDate = value; }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
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
