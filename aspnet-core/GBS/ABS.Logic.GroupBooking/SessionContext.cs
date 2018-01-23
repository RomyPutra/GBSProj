using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using System.Web;
using ABS.Logic.GroupBooking.Booking;

namespace ABS.Logic.GroupBooking
{
    public class SessionContextLogic
    {
        #region "Configuration"
        /// <summary>
        /// Retrieve connection string
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString()
        {
            if (HttpContext.Current.Session["ConnectionString"] != null)
            {
                string returnURL = (string)HttpContext.Current.Session["ConnectionString"];
                return returnURL;
            }
            return null;
        }

        /// <summary>
        /// Define connection string into session
        /// </summary>
        /// <param name="ConnectionString"></param>
        public void SetConnectionString(string ConnectionString)
        {
            HttpContext.Current.Session.Remove("ConnectionString");
            HttpContext.Current.Session.Add("ConnectionString", ConnectionString);
        }
        #endregion

        #region "Session"
        /// <summary>
        /// Reset active session contains
        /// </summary>
        public void Reset()
        {
            SetIsSameRow("");
            SetBookingContainer(null);
            setBookingJourneyContainer(null);
            SetPassengerContainer(null);
            SetPassengerInfantModel(null);
            SetSessionID(null);
            SetTempAvaPassengerContainer(null);
            SetTempPassengerContainer(null);
            SetTempPassengerInfantContainer(null);
            SetAllowAddInfant("");
            SetIsInQuietZone("");
            SetAllowAddInfant_FlightDesignator("");
        }

        /// <summary>
        /// Retrieve logon domain parameter
        /// </summary>
        /// <returns></returns>
        public string GetLogonDomain()
        {
            if (HttpContext.Current.Session["LogonDomain"] != null)
            {
                string returnURL = (string)HttpContext.Current.Session["LogonDomain"];
                return returnURL;
            }
            return null;
        }

        /// <summary>
        /// Define logon domain parameter
        /// </summary>
        /// <param name="LogonDomain"></param>
        public void SetLogonDomain(string LogonDomain)
        {
            HttpContext.Current.Session.Remove("LogonDomain");
            HttpContext.Current.Session.Add("LogonDomain", LogonDomain);
        }

        /// <summary>
        /// Retrieve logon user name
        /// </summary>
        /// <returns></returns>
        public string GetLogonUserName()
        {
            if (HttpContext.Current.Session["LogonUserName"] != null)
            {
                string returnURL = (string)HttpContext.Current.Session["LogonUserName"];
                return returnURL;
            }
            return null;
        }

        /// <summary>
        /// Define logon user name
        /// </summary>
        /// <param name="LogonUserName"></param>
        public void SetLogonUserName(string LogonUserName)
        {
            HttpContext.Current.Session.Remove("LogonUserName");
            HttpContext.Current.Session.Add("LogonUserName", LogonUserName);
        }

        /// <summary>
        /// Retrieve logon password
        /// </summary>
        /// <returns></returns>
        public string GetLogonPassword()
        {
            if (HttpContext.Current.Session["LogonPassword"] != null)
            {
                string returnURL = (string)HttpContext.Current.Session["LogonPassword"];
                return returnURL;
            }
            return null;
        }

        /// <summary>
        /// Define logon password
        /// </summary>
        /// <param name="LogonPassword"></param>
        public void SetLogonPassword(string LogonPassword)
        {
            HttpContext.Current.Session.Remove("LogonPassword");
            HttpContext.Current.Session.Add("LogonPassword", LogonPassword);
        }

        /// <summary>
        /// Obtain session ID
        /// </summary>
        /// <returns></returns>
        public string GetSessionID()
        {
            if (HttpContext.Current.Session["SessionID"] != null)
            {
                string returnURL = (string)HttpContext.Current.Session["SessionID"];
                return returnURL;
            }
            return null;
        }

        /// <summary>
        /// Define session ID
        /// </summary>
        /// <param name="SessionID"></param>
        public void SetSessionID(string SessionID)
        {
            HttpContext.Current.Session.Remove("SessionID");
            HttpContext.Current.Session.Add("SessionID", SessionID);
        }

        #endregion

        #region "Booking Containers & Parameters"
        public List<PassengerContainer> GetPassengerContainer()
        {
            if (HttpContext.Current.Session["PassengerContainer"] != null)
            {
                List<PassengerContainer> returnObj = (List<PassengerContainer>)HttpContext.Current.Session["PassengerContainer"];
                return returnObj;
            }
            return null;
        }

        public void SetPassengerContainer(List<PassengerContainer> PassengerContainer)
        {
            HttpContext.Current.Session.Remove("PassengerContainer");
            HttpContext.Current.Session.Add("PassengerContainer", PassengerContainer);
        }

        public List<PassengerContainer> GetAllPassengerContainer()
        {
            if (HttpContext.Current.Session["AllPassengerContainer"] != null)
            {
                List<PassengerContainer> returnObj = (List<PassengerContainer>)HttpContext.Current.Session["AllPassengerContainer"];
                return returnObj;
            }
            return null;
        }

        public void SetAllPassengerContainer(List<PassengerContainer> PassengerContainer)
        {
            HttpContext.Current.Session.Remove("AllPassengerContainer");
            HttpContext.Current.Session.Add("AllPassengerContainer", PassengerContainer);
        }

        public List<PassengerInfantContainer> GetPassengerInfantModel()
        {
            if (HttpContext.Current.Session["PassengerInfantModel"] != null)
            {
                List<PassengerInfantContainer> returnObj = (List<PassengerInfantContainer>)HttpContext.Current.Session["PassengerInfantModel"];
                return returnObj;
            }
            return null;
        }

        public void SetPassengerInfantModel(List<PassengerInfantContainer> PassengerInfantModel)
        {
            HttpContext.Current.Session.Remove("PassengerInfantModel");
            HttpContext.Current.Session.Add("PassengerInfantModel", PassengerInfantModel);
        }

        public List<PaymentContainer> GetPaymentContainer()
        {
            if (HttpContext.Current.Session["PaymentContainer"] != null)
            {
                List<PaymentContainer> returnObj = (List<PaymentContainer>)HttpContext.Current.Session["PaymentContainer"];
                return returnObj;
            }
            return null;
        }

        public void SetPaymentContainer(List<PaymentContainer> PaymentContainer)
        {
            HttpContext.Current.Session.Remove("PaymentContainer");
            HttpContext.Current.Session.Add("PaymentContainer", PaymentContainer);
        }

        public BookingContainer GetBookingContainer()
        {
            if (HttpContext.Current.Session["BookingContainer"] != null)
            {
                BookingContainer returnObj = (BookingContainer)HttpContext.Current.Session["BookingContainer"];
                return returnObj;
            }
            return null;
        }

        public void SetBookingContainer(BookingContainer BookingContainer)
        {
            HttpContext.Current.Session.Remove("BookingContainer");
            HttpContext.Current.Session.Add("BookingContainer", BookingContainer);
        }

        public List<BookingJourneyContainer> GetBookingJourneyContainer()
        {
            if (HttpContext.Current.Session["BookingJourneyContainer"] != null)
            {
                List<BookingJourneyContainer> returnObj = (List<BookingJourneyContainer>)HttpContext.Current.Session["BookingJourneyContainer"];
                return returnObj;
            }
            return null;
        }

        public void setBookingJourneyContainer(List<BookingJourneyContainer> BookingJourneyContainer)
        {
            HttpContext.Current.Session.Remove("BookingJourneyContainer");
            HttpContext.Current.Session.Add("BookingJourneyContainer", BookingJourneyContainer);
        }

        public string GetIsSameRow()
        {
            if (HttpContext.Current.Session["GetIsSameRow"] != null)
            {
                string returnObj = (string)HttpContext.Current.Session["GetIsSameRow"];
                return returnObj;
            }
            return null;
        }

        public void SetIsSameRow(string GetIsSameRow)
        {
            HttpContext.Current.Session.Remove("GetIsSameRow");
            HttpContext.Current.Session.Add("GetIsSameRow", GetIsSameRow);
        }

        public string GetIsInQuietZone()
        {
            if (HttpContext.Current.Session["GetIsInQuietZone"] != null)
            {
                string returnObj = (string)HttpContext.Current.Session["GetIsInQuietZone"];
                return returnObj;
            }
            return null;
        }

        public void SetIsInQuietZone(string GetIsInQuietZone)
        {
            HttpContext.Current.Session.Remove("GetIsInQuietZone");
            HttpContext.Current.Session.Add("GetIsInQuietZone", GetIsInQuietZone);
        }

        public void SetAllowAddInfant(string AllowAddInfant)
        {
            HttpContext.Current.Session.Remove("AllowAddInfant");
            HttpContext.Current.Session.Add("AllowAddInfant", AllowAddInfant);
        }

        public string GetAllowAddInfant()
        {
            if (HttpContext.Current.Session["AllowAddInfant"] != null)
            {
                string returnObj = (string)HttpContext.Current.Session["AllowAddInfant"];
                return returnObj;
            }
            return null;
        }

        public void SetAllowAddInfant_FlightDesignator(string Flight)
        {
            HttpContext.Current.Session.Remove("Flight");
            HttpContext.Current.Session.Add("Flight", Flight);
        }

        public string GetAllowAddInfant_FlightDesignator()
        {
            if (HttpContext.Current.Session["Flight"] != null)
            {
                string returnObj = (string)HttpContext.Current.Session["Flight"];
                return returnObj;
            }
            return null;
        }

        #endregion

        #region "Temporary Session Containers"
        // Temp available passengers session for "PassengerContainer"
        public List<PassengerContainer> GetTempAvaPassengerContainer()
        {
            if (HttpContext.Current.Session["TempAvaPassengerContainer"] != null)
            {
                List<PassengerContainer> returnObj = (List<PassengerContainer>)HttpContext.Current.Session["TempAvaPassengerContainer"];
                return returnObj;
            }
            return null;
        }

        public void SetTempAvaPassengerContainer(List<PassengerContainer> TempAvaPassengerContainer)
        {
            HttpContext.Current.Session.Remove("TempAvaPassengerContainer");
            HttpContext.Current.Session.Add("TempAvaPassengerContainer", TempAvaPassengerContainer);
        }

        // Temp session for "PassengerContainer"
        public List<PassengerContainer> GetTempPassengerContainer()
        {
            if (HttpContext.Current.Session["TempPassengerContainer"] != null)
            {
                List<PassengerContainer> returnObj = (List<PassengerContainer>)HttpContext.Current.Session["TempPassengerContainer"];
                return returnObj;
            }
            return null;
        }

        public void SetTempPassengerContainer(List<PassengerContainer> TempPassengerContainer)
        {
            HttpContext.Current.Session.Remove("TempPassengerContainer");
            HttpContext.Current.Session.Add("TempPassengerContainer", TempPassengerContainer);
        }

        // Temmp session for "PassengerInfantContainer"
        public List<PassengerInfantContainer> GetTempPassengerInfantContainer()
        {
            if (HttpContext.Current.Session["TempPassengerInfantContainer"] != null)
            {
                List<PassengerInfantContainer> returnObj = (List<PassengerInfantContainer>)HttpContext.Current.Session["TempPassengerInfantContainer"];
                return returnObj;
            }
            return null;
        }

        public void SetTempPassengerInfantContainer(List<PassengerInfantContainer> TempPassengerInfantContainer)
        {
            HttpContext.Current.Session.Remove("TempPassengerInfantContainer");
            HttpContext.Current.Session.Add("TempPassengerInfantContainer", TempPassengerInfantContainer);
        }
        #endregion
    }
}
