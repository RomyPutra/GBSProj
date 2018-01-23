using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABS.GBS
{
    public class TravelPlanRequest
    {
        private string _username = String.Empty;
        private string _password = String.Empty;
        private string _channel = String.Empty;
        private string _currency = String.Empty;
        private string _culturecode = String.Empty;
        //private string _fareType = String.Empty;
        //private string _memberRole = String.Empty;
        //private string _isValuePack = String.Empty;
        private List<Flight> _flight;
        private List<IPassenger> _passenger;

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        public string Channel
        {
            get { return _channel; }
            set { _channel = value; }
        }
        public string Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }
        public string CultureCode
        {
            get { return _culturecode; }
            set { _culturecode = value; }
        }
        //public string FareType
        //{
        //    get { return _fareType; }
        //    set { _fareType = value; }
        //}
        //public string MemberRole
        //{
        //    get { return _memberRole; }
        //    set { _memberRole = value; }
        //}

        //public string isValuePack
        //{
        //    get { return _isValuePack; }
        //    set { _isValuePack = value; }
        //}

        public List<Flight> Flight
        {
            get { return _flight; }
            set { _flight = value; }
        }
        public List<IPassenger> Passenger
        {
            get { return _passenger; }
            set { _passenger = value; }
        }
    }
}