using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public class BK_TRANSTAX_Info
    {
        private string _recordLocator = String.Empty;
        private int _transID;
        private int _taxCode;

        private decimal _taxRate;
        private decimal _taxAmt;
        private byte _transvoid;
        private Guid _rowguid = Guid.Empty;
        private DateTime _syncCreate;
        private DateTime _syncLastUpd;

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
        public int TaxCode
        {
            get { return _taxCode; }
            set { _taxCode = value; }
        }
        public decimal TaxRate
        {
            get { return _taxRate; }
            set { _taxRate = value; }
        }

        public decimal TaxAmt
        {
            get { return _taxAmt; }
            set { _taxAmt = value; }
        }

        public byte Transvoid
        {
            get { return _transvoid; }
            set { _transvoid = value; }
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
        #endregion

    }
