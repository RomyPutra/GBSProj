using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public class Charges_Info
    {
        private string _recordLocator = String.Empty;
        private int _transID;
        private int _chargesCode;

        private decimal _chargesRate;
        private decimal _chargesAmt;
        private byte _status;
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
        public int ChargesCode
        {
            get { return _chargesCode; }
            set { _chargesCode = value; }
        }
        public decimal ChargesRate
        {
            get { return _chargesRate; }
            set { _chargesRate = value; }
        }

        public decimal ChargesAmt
        {
            get { return _chargesAmt; }
            set { _chargesAmt = value; }
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
