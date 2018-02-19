using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public class BK_TAXFEESCONTROL_Info
    {
        private int _taxFeesCode;
        private string _taxFeesGroup = String.Empty;

        private string _taxFeesDesc = String.Empty;
        private decimal _taxFeesRate;
        private byte _active;
        private byte _flag;
        private Guid _rowguid = Guid.Empty;
        private string _createBy = String.Empty;
        private DateTime _syncCreate;
        private DateTime _syncLastUpd;
        private string _lastSyncBy = String.Empty;

        #region Public Properties
        public int TaxFeesCode
        {
            get { return _taxFeesCode; }
            set { _taxFeesCode = value; }
        }
        public string TaxFeesGroup
        {
            get { return _taxFeesGroup; }
            set { _taxFeesGroup = value; }
        }
        public string TaxFeesDesc
        {
            get { return _taxFeesDesc; }
            set { _taxFeesDesc = value; }
        }

        public decimal TaxFeesRate
        {
            get { return _taxFeesRate; }
            set { _taxFeesRate = value; }
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
