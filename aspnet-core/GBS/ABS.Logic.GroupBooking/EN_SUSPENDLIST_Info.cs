using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public class EN_SUSPENDLIST_Info
    {
        private string _suspendID = String.Empty;
        private string _agentID = String.Empty;
        private string _origins = String.Empty;
        private string _destination = String.Empty;

        private DateTime _suspendDate;
        private string _lastEnquiryID = String.Empty;
        private int _suspendAttempt;
        private DateTime _suspendExpiry;
        private Guid _rowguid = Guid.Empty;
        private DateTime _syncCreate;
        private DateTime _syncLastUpd;
        private string _createBy = String.Empty;

        #region Public Properties
        public string SuspendID
        {
            get { return _suspendID; }
            set { _suspendID = value; }
        }
        
        public string AgentID
        {
            get { return _agentID; }
            set { _agentID = value; }
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
        public DateTime SuspendDate
        {
            get { return _suspendDate; }
            set { _suspendDate = value; }
        }

        public string LastEnquiryID
        {
            get { return _lastEnquiryID; }
            set { _lastEnquiryID = value; }
        }

        public int SuspendAttempt
        {
            get { return _suspendAttempt; }
            set { _suspendAttempt = value; }
        }

        public DateTime SuspendExpiry
        {
            get { return _suspendExpiry; }
            set { _suspendExpiry = value; }
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
