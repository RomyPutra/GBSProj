using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace ABS.GBS
{
    public class TravelPlanResponse
    {
        private string _errorCode = String.Empty;
        private string _errorMessage = String.Empty;
        private List<QualifiedPassenger> _qualifiedPassenger;
        private decimal _TotalPremium;
        private string _FeeDescription = String.Empty;
        private string _SSRFeeCode = String.Empty;
        private string _benefitTnC = String.Empty;
        private int _yesSellingInsurance;
        private int _noSellingInsurance;
        private int _insuranceTnC;

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
        public List<QualifiedPassenger> QualifiedPassenger
        {
            get { return _qualifiedPassenger; }
            set { _qualifiedPassenger = value; }
        }
        public string FeeDescription
        {
            get { return _FeeDescription; }
            set { _FeeDescription = value; }
        }
        public decimal TotalPremium
        {
            get { return _TotalPremium; }
            set { _TotalPremium = value; }
        }
        public string SSRFeeCode
        {
            get { return _SSRFeeCode; }
            set { _SSRFeeCode = value; }
        }
        public string BenefitTnC
        {
            get { return _benefitTnC; }
            set { _benefitTnC = value; }
        }
        public int YesSellingInsurance
        {
            get { return _yesSellingInsurance; }
            set { _yesSellingInsurance = value; }
        }
        public int NoSellingInsurance
        {
            get { return _noSellingInsurance; }
            set { _noSellingInsurance = value; }
        }
        public int InsuranceTnC
        {
            get { return _insuranceTnC; }
            set { _insuranceTnC = value; }
        }
    }
}