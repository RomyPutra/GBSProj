using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABS.GBS
{
    public class PurchaseRequest
    {
        private string _pnr = String.Empty;
        private string _payStatus = String.Empty;

        public string PNR
        {
            get { return _pnr; }
            set { _pnr = value; }
        }
        public string payStatus
        {
            get { return _payStatus; }
            set { _payStatus = value; }
        }
    }
}
