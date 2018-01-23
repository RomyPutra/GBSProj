using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABS.GBS
{
    public class Flight
    {
        private string _departCountryCode = String.Empty;
        private string _departStationCode = String.Empty;
        private string _arrivalCountryCode = String.Empty;
        private string _arrivalStationCode = String.Empty;
        private string _departAirlineCode = String.Empty;
        private string _departDateTime = String.Empty;

        private string _returnAirlineCode = String.Empty;
        private string _returnDateTime = String.Empty;
        private string _departFlightNo = String.Empty;
        private string _returnFlightNo = String.Empty;
        private string _flightType = String.Empty;
        private string _flightDates = String.Empty;
        private string _totalFlightHour = String.Empty;

        public string DepartCountryCode
        {
            get { return _departCountryCode; }
            set { _departCountryCode = value; }
        }
        public string DepartStationCode
        {
            get { return _departStationCode; }
            set { _departStationCode = value; }
        }
        public string ArrivalCountryCode
        {
            get { return _arrivalCountryCode; }
            set { _arrivalCountryCode = value; }
        }
        public string ArrivalStationCode
        {
            get { return _arrivalStationCode; }
            set { _arrivalStationCode = value; }
        }
        public string DepartAirlineCode
        {
            get { return _departAirlineCode; }
            set { _departAirlineCode = value; }
        }
        public string DepartDateTime
        {
            get { return _departDateTime; }
            set { _departDateTime = value; }
        }
        public string ReturnAirlineCode
        {
            get { return _returnAirlineCode; }
            set { _returnAirlineCode = value; }
        }
        public string ReturnDateTime
        {
            get { return _returnDateTime; }
            set { _returnDateTime = value; }
        }
        public string DepartFlightNo
        {
            get { return _departFlightNo; }
            set { _departFlightNo = value; }
        }
        public string ReturnFlightNo
        {
            get { return _returnFlightNo; }
            set { _returnFlightNo = value; }
        }
        public string FlightType
        {
            get { return _flightType; }
            set { _flightType = value; }
        }
        public string FlightDates
        {
            get { return _flightDates; }
            set { _flightDates = value; }
        }
        public string TotalFlightHour
        {
            get { return _totalFlightHour; }
            set { _totalFlightHour = value; }
        }
    }
}