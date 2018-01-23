using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABS.GBS
{
    public class QualifiedPassenger
    {
        private string _firstName = String.Empty;
        private string _lastName = String.Empty;
        private Boolean _isPassengerQualified;
        private string _currencyCode = String.Empty;
        private decimal _passengerPremiumAmount;
        private int _passengerNumber;

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }
        public Boolean IsPassengerQualified
        {
            get { return _isPassengerQualified; }
            set { _isPassengerQualified = value; }
        }
        public string CurrencyCode
        {
            get { return _currencyCode; }
            set { _currencyCode = value; }
        }
        public decimal PassengerPremiumAmount
        {
            get { return _passengerPremiumAmount; }
            set { _passengerPremiumAmount = value; }
        }
        public int PassengerNumber
        {
            get { return _passengerNumber; }
            set { _passengerNumber = value; }
        }



    }
}