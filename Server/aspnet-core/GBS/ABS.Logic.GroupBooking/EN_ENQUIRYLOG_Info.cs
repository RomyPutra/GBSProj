using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public class EN_ENQUIRYLOG_Info
    {
        private string _enquiryID = String.Empty;
        private string _agentID = String.Empty;
        private DateTime _enquiryDate;

        private DateTime _lastEnquiryDate;
        private string _origins = String.Empty;
        private string _destination = String.Empty;
        private int _noOfAttempt;
        private Guid _rowguid = Guid.Empty;
        private DateTime _syncCreate;
        private DateTime _syncLastUpd;
        private string _createBy = String.Empty;

        #region Public Properties
        public string EnquiryID
        {
            get { return _enquiryID; }
            set { _enquiryID = value; }
        }
        public string AgentID
        {
            get { return _agentID; }
            set { _agentID = value; }
        }
        public DateTime EnquiryDate
        {
            get { return _enquiryDate; }
            set { _enquiryDate = value; }
        }
        public DateTime LastEnquiryDate
        {
            get { return _lastEnquiryDate; }
            set { _lastEnquiryDate = value; }
        }

        public string Origins
        {
            get { return _origins; }
            set { _origins = value; }
        }

        public string Destination
        {
            get { return _destination; }
            set { _destination = value; }
        }

        public int NoOfAttempt
        {
            get { return _noOfAttempt; }
            set { _noOfAttempt = value; }
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

        public string CreateBy
        {
            get { return _createBy; }
            set { _createBy = value; }
        }
        #endregion

    }
