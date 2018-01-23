using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public class BK_TRANSTENDER_Info
    {
        private string _recordLocator = String.Empty;
        private int _transID;
        private int _seqNo;

        private DateTime _transDate;
        private string _currency = String.Empty;
        private string _tenderID = String.Empty;
        private string _feeType = String.Empty;
        private decimal _tenderAmt;
        private decimal _exchgRate;
        private decimal _tenderDue;
        private decimal _changeAmt;
        private byte _transvoid;
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
        public int SeqNo
        {
            get { return _seqNo; }
            set { _seqNo = value; }
        }
        public DateTime TransDate
        {
            get { return _transDate; }
            set { _transDate = value; }
        }

        public string Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }

        public string TenderID
        {
            get { return _tenderID; }
            set { _tenderID = value; }
        }

        public string FeeType
        {
            get { return _feeType; }
            set { _feeType = value; }
        }

    public decimal TenderAmt
        {
            get { return _tenderAmt; }
            set { _tenderAmt = value; }
        }

        public decimal ExchgRate
        {
            get { return _exchgRate; }
            set { _exchgRate = value; }
        }

        public decimal TenderDue
        {
            get { return _tenderDue; }
            set { _tenderDue = value; }
        }

        public decimal ChangeAmt
        {
            get { return _changeAmt; }
            set { _changeAmt = value; }
        }

        public byte Transvoid
        {
            get { return _transvoid; }
            set { _transvoid = value; }
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

