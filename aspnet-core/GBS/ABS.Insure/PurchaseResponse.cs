using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace ABS.GBS
{
    public class PurchaseResponse
    {
        private string _errorCode = String.Empty;
        private string _errorMessage = String.Empty;

        public string ErrorCode
        {
            get { return _errorCode; }
            set { _errorCode = value; }
        }
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }
    }
}
