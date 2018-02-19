using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public class ChargesControl_Info
    {
        private int _chargesCode;
        private string _chargesGroup = String.Empty;

        private string _chargesDesc = String.Empty;
        private decimal _chargesRate;
        private byte _active;
        private byte _flag;
        private Guid _rowguid = Guid.Empty;
        private string _createBy = String.Empty;
        private DateTime _syncCreate;
        private DateTime _syncLastUpd;
        private string _lastSyncBy = String.Empty;

        #region Public Properties
        public int ChargesCode
        {
            get { return _chargesCode; }
            set { _chargesCode = value; }
        }
        public string ChargesGroup
        {
            get { return _chargesGroup; }
            set { _chargesGroup = value; }
        }
        public string ChargesDesc
        {
            get { return _chargesDesc; }
            set { _chargesDesc = value; }
        }

        public decimal ChargesRate
        {
            get { return _chargesRate; }
            set { _chargesRate = value; }
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
