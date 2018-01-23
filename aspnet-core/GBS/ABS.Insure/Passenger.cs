using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABS.GBS
{
    public class IPassenger
    {
        private string _firstName = String.Empty;
        private string _lastName = String.Empty;
        private string _gender = String.Empty;
        private string _dob = String.Empty;
        private string _identityType = String.Empty;
        private string _countryOfResidence = String.Empty;
        private string _currencyCode = String.Empty;
        private string _Nationality = String.Empty;
        private string _passengerNumber = String.Empty;
        //private string _isInfant = String.Empty;
        //private string _age = String.Empty;
        //private string _identityNo = String.Empty;
        //private string _isQualified = String.Empty;
        //private string _nationalityPassenger = String.Empty;
        //private string _selectedPlanCode = String.Empty;
        //private string _selectedSSRFeeCode = String.Empty;
        //private int _passengerPremiumAmount;
        //private int _additionalFeesAndTaxes;

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
        public string Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }
        public string DOB
        {
            get { return _dob; }
            set { _dob = value; }
        }
        public string IdentityType
        {
            get { return _identityType; }
            set { _identityType = value; }
        }
        public string CountryOfResidence
        {
            get { return _countryOfResidence; }
            set { _countryOfResidence = value; }
        }
        public string CurrencyCode
        {
            get { return _currencyCode; }
            set { _currencyCode = value; }
        }
        public string Nationality
        {
            get { return _Nationality; }
            set { _Nationality = value; }
        }
        public string PassengerNumber
        {
            get { return _passengerNumber; }
            set { _passengerNumber = value; }
        }
        //public string IsInfant
        //{
        //    get { return _isInfant; }
        //    set { _isInfant = value; }
        //}
        //public string Age
        //{
        //    get { return _age; }
        //    set { _age = value; }
        //}
        //public string IdentityNo
        //{
        //    get { return _identityNo; }
        //    set { _identityNo = value; }
        //}
        //public string IsQualified
        //{
        //    get { return _isQualified; }
        //    set { _isQualified = value; }
        //}
        //public string NationalityPassenger
        //{
        //    get { return _nationalityPassenger; }
        //    set { _nationalityPassenger = value; }
        //}
        //public string SelectedPlanCode
        //{
        //    get { return _selectedPlanCode; }
        //    set { _selectedPlanCode = value; }
        //}
        //public string SelectedSSRFeeCode
        //{
        //    get { return _selectedSSRFeeCode; }
        //    set { _selectedSSRFeeCode = value; }
        //}
        //public int PassengerPremiumAmount
        //{
        //    get { return _passengerPremiumAmount; }
        //    set { _passengerPremiumAmount = value; }
        //}
        //public int AdditionalFeesAndTaxes
        //{
        //    get { return _additionalFeesAndTaxes; }
        //    set { _additionalFeesAndTaxes = value; }
        //}
    }
}