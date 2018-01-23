using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ABS.Navitaire.BookingManager;
using ABS.Navitaire.SessionManager;
using ABS.Navitaire.AccountManager;
using ABS.Navitaire.AgentManager;
using ABS.Navitaire.PersonManager;
using ABS.Navitaire.UtilitiesManager;
using ABS.Navitaire.SessionManager;
using System.Data;
using System.Web;
using System.Threading;
using System.IO;
using System.Net;
using System.Configuration;
using System.Xml;
using System.Collections.Generic;
using ABS.GBS.Log;
using StackExchange.Profiling;

namespace ABS.Navitaire
{
    public partial class APIBooking : APIBase
    {
        public SystemLog SystemLog = new SystemLog();
        public APIBooking(string Signature)
        {
            InitializeComponent();
            this.Signature = Signature;
            this.VerifySignature();
        }

        public APIBooking(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public Boolean KeepAlive(string SessionID)
        {
            IBookingManager bookingAPI = new BookingManagerClient();
            ISessionManager sessionManager = new SessionManagerClient();
            KeepAliveRequest request = new SessionManager.KeepAliveRequest();
            KeepAliveResponse aLiveResponse = new SessionManager.KeepAliveResponse();
            Booking response = new Booking();
            if (!string.IsNullOrEmpty(SessionID))
            {
                //response = GetBookingFromState(SessionID);
                request.ContractVersion = this.ContractVersion;
                request.Signature = SessionID;
                aLiveResponse = sessionManager.KeepAlive(request);

            }
            response = null;
            bookingAPI = null;
            aLiveResponse = null;
            sessionManager = null;
            return true;
        }

        public string GetBookingByPNR(string RecordLocator, ref string errMsg, ref GetBookingResponse bookingResp, int cnt = 0)
        {
            if (cnt > 0) Thread.Sleep(2000); //added by diana 20140205 - reconnect purpose, amended by ketee, delay only if rerun, 20170916

            IBookingManager bookingAPI = new BookingManagerClient();
            GetBookingRequest request = new GetBookingRequest();
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;

            try
            {

                ////begin testing
                //Random rnd = new Random();
                //int num = rnd.Next(3);
                //if (num == 0)
                //    throw (new System.ServiceModel.ServiceActivationException());
                ////end testing

                request.ContractVersion = this.ContractVersion;
                request.Signature = AgentLogon();// SessionManager._signature;
                request.GetBookingReqData = new GetBookingRequestData();
                request.GetBookingReqData.GetBookingBy = GetBookingBy.RecordLocator;
                request.GetBookingReqData.GetByRecordLocator = new GetByRecordLocator();
                request.GetBookingReqData.GetByRecordLocator.RecordLocator = RecordLocator;// "YY8DYW";
                GetBookingResponse response;// = bookingAPI.GetBooking(request);
                using (profiler.Step("GetBooking"))
                {
                    response = bookingAPI.GetBooking(request);
                }
                if (response != null && response.Booking != null)
                    bookingResp = response;
                return request.Signature;// response.Booking.Passengers[0];
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);

                //amended by diana 20140205 - try to get data for max of 5 attempts
                cnt += 1;
                if (cnt <= 5)
                {
                    return GetBookingByPNR(RecordLocator, ref errMsg, ref bookingResp, cnt);
                }
                else
                {
                    return "";
                }
                //end amended by diana 20140205 - try to get data for max of 5 attempts

            }

        }

        public GetBookingResponse GetBookingByPNR(string RecordLocator)
        {
            IBookingManager bookingAPI = new BookingManagerClient();
            GetBookingRequest request = new GetBookingRequest();
            try
            {

                request.ContractVersion = this.ContractVersion;
                request.Signature = this.Signature;// SessionManager._signature;
                request.GetBookingReqData = new GetBookingRequestData();
                //request.GetBookingReqData.GetBookingBy = GetBookingBy.RecordLocator;
                request.GetBookingReqData.GetByRecordLocator = new GetByRecordLocator();
                request.GetBookingReqData.GetByRecordLocator.RecordLocator = RecordLocator;// "YY8DYW";
                //string str = GetXMLString(request);
                GetBookingResponse response = bookingAPI.GetBooking(request);

                //str = GetXMLString(response);
                return response;// response.Booking.Passengers[0];
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                return null;
            }

        }

        public GetBookingResponse GetBookingResponseByPNR(string RecordLocator, int cnt = 0)
        {
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;

            if (cnt > 0 ) Thread.Sleep(2000); //added by diana 20140205 - reconnect purpose, amended by ketee, delay only if rerun, 20170916

            IBookingManager bookingAPI = new BookingManagerClient();
            GetBookingRequest request = new GetBookingRequest();
            try
            {

                ////begin testing
                //Random rnd = new Random();
                //int num = rnd.Next(3);
                //if (num == 0)
                //    throw (new System.ServiceModel.ServiceActivationException());
                ////end testing

                request.ContractVersion = this.ContractVersion;
                request.Signature = AgentLogon();// SessionManager._signature;
                HttpContext.Current.Session["Signature"] = request.Signature;
                request.GetBookingReqData = new GetBookingRequestData();
                request.GetBookingReqData.GetBookingBy = GetBookingBy.RecordLocator;
                request.GetBookingReqData.GetByRecordLocator = new GetByRecordLocator();
                request.GetBookingReqData.GetByRecordLocator.RecordLocator = RecordLocator;// "YY8DYW";
                GetBookingResponse response;// = bookingAPI.GetBooking(request);
                using (profiler.Step("GetBooking"))
                {
                    response = bookingAPI.GetBooking(request);
                }
                string resp = GetXMLString(response);
                if (response == null || response.Booking == null)
                {
                    log.Info(this, "Booking response null: PNRs " + RecordLocator);
                    cnt += 1;
                    if (cnt <= 5)
                    {

                        return GetBookingResponseByPNR(RecordLocator, cnt);
                    }
                    else
                    {
                        return null;
                    }
                }
                return response;
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);

                //amended by diana 20140205 - try to get data for max of 5 attempts
                cnt += 1;
                if (cnt <= 5)
                {
                    return GetBookingResponseByPNR(RecordLocator, cnt);
                }
                else
                {
                    return null;
                }
                //end amended by diana 20140205 - try to get data for max of 5 attempts

            }

        }

        public GetBookingResponse GetBookingResponseByPNRSignature(string RecordLocator, string Signature, int cnt = 0)
        {
            if (cnt > 0) Thread.Sleep(2000); //added by diana 20140205 - reconnect purpose, amended by ketee, delay only if rerun, 20170916

            IBookingManager bookingAPI = new BookingManagerClient();
            GetBookingRequest request = new GetBookingRequest();
            try
            {
                request.ContractVersion = this.ContractVersion;
                request.Signature = Signature;// SessionManager._signature;
                request.GetBookingReqData = new GetBookingRequestData();
                request.GetBookingReqData.GetBookingBy = GetBookingBy.RecordLocator;
                request.GetBookingReqData.GetByRecordLocator = new GetByRecordLocator();
                request.GetBookingReqData.GetByRecordLocator.RecordLocator = RecordLocator;// "YY8DYW";
                //string xml = GetXMLString(request);
                GetBookingResponse response = bookingAPI.GetBooking(request);
                //string xmlresponse = GetXMLString(response);
                return response;
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);

                //amended by diana 20140205 - try to get data for max of 5 attempts
                cnt += 1;
                if (cnt <= 5)
                {
                    return GetBookingResponseByPNRSignature(RecordLocator, Signature, cnt);
                }
                else
                {
                    return null;
                }
                //end amended by diana 20140205 - try to get data for max of 5 attempts

            }

        }

        public DataTable GetKeySignatureByPNR(string RecordLocator, ref string errMsg, ref string sign)
        {
            IBookingManager bookingAPI = new BookingManagerClient();
            GetBookingRequest request = new GetBookingRequest();
            //string[] keySignature;
            try
            {
                if (sign == "")
                { sign = AgentLogon(); }

                request.ContractVersion = this.ContractVersion;
                request.Signature = sign;
                request.GetBookingReqData = new GetBookingRequestData();
                request.GetBookingReqData.GetBookingBy = GetBookingBy.RecordLocator;
                request.GetBookingReqData.GetByRecordLocator = new GetByRecordLocator();
                request.GetBookingReqData.GetByRecordLocator.RecordLocator = RecordLocator;// "YY8DYW";
                GetBookingResponse response = bookingAPI.GetBooking(request);

                DataTable dtJourneyData = new DataTable();
                dtJourneyData.Columns.Add("JourneySellKey");
                dtJourneyData.Columns.Add("FareSellKey");
                dtJourneyData.Columns.Add("FareSellKeyTransit");

                for (int i = 0; i < response.Booking.Journeys.Length; i++)
                {
                    DataRow row;
                    row = dtJourneyData.NewRow();
                    row["JourneySellKey"] = response.Booking.Journeys[i].JourneySellKey.ToString();
                    if (response.Booking.Journeys[i].Segments.Length == 1)
                    {
                        row["FareSellKey"] = response.Booking.Journeys[i].Segments[0].Fares[0].FareSellKey.ToString();
                        row["FareSellKeyTransit"] = "";
                    }
                    else
                    {
                        row["FareSellKey"] = response.Booking.Journeys[i].Segments[0].Fares[0].FareSellKey.ToString();
                        row["FareSellKeyTransit"] = response.Booking.Journeys[i].Segments[1].Fares[0].FareSellKey.ToString();
                    }
                    dtJourneyData.Rows.Add(row);
                }
                return dtJourneyData;
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                return null;
            }
        }

        public string[] GetPassengerByPNR(string RecordLocator, ref string errMsg)
        {
            IBookingManager bookingAPI = new BookingManagerClient();
            GetBookingRequest request = new GetBookingRequest();
            string[] PassID;
            try
            {
                request.ContractVersion = this.ContractVersion;
                request.Signature = AgentLogon();// SessionManager._signature;
                request.GetBookingReqData = new GetBookingRequestData();
                request.GetBookingReqData.GetBookingBy = GetBookingBy.RecordLocator;
                request.GetBookingReqData.GetByRecordLocator = new GetByRecordLocator();
                request.GetBookingReqData.GetByRecordLocator.RecordLocator = RecordLocator;// "YY8DYW";
                GetBookingResponse response = bookingAPI.GetBooking(request);
                PassID = new string[response.Booking.Passengers.Count()];
                for (int i = 0; i < response.Booking.Passengers.Count(); i++)
                {
                    PassID[i] = response.Booking.Passengers[i].PassengerNumber.ToString();
                }
                return PassID;// response.Booking.Passengers[0];
            }
            //amended by diana 20131210 - try catch to check for valid booking
            catch (TimeoutException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), "", RecordLocator); return null; }
            catch (OutOfMemoryException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), "", RecordLocator); return null; }
            catch (IndexOutOfRangeException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), "", RecordLocator); return null; }
            catch (ThreadInterruptedException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), "", RecordLocator); return null; }
            catch (NullReferenceException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), "", RecordLocator); return null; }
            catch (StackOverflowException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), "", RecordLocator); return null; }
            catch (ApplicationException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), "", RecordLocator); return null; }
            catch (Exception ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), "", RecordLocator); return null; }

        }

        public string[] GetPassengerInfantByPNR(string RecordLocator, ref string errMsg)
        {
            IBookingManager bookingAPI = new BookingManagerClient();
            GetBookingRequest request = new GetBookingRequest();
            string[] PassID;
            int count = 0;
            try
            {
                request.ContractVersion = this.ContractVersion;
                request.Signature = AgentLogon();// SessionManager._signature;
                request.GetBookingReqData = new GetBookingRequestData();
                request.GetBookingReqData.GetBookingBy = GetBookingBy.RecordLocator;
                request.GetBookingReqData.GetByRecordLocator = new GetByRecordLocator();
                request.GetBookingReqData.GetByRecordLocator.RecordLocator = RecordLocator;// "YY8DYW";
                GetBookingResponse response = bookingAPI.GetBooking(request);

                for (int i = 0; i < response.Booking.Passengers.Count(); i++)
                {
                    count += response.Booking.Passengers[i].PassengerInfants.Length;
                }
                PassID = new string[count];
                int index = 0;
                for (int i = 0; i < response.Booking.Passengers.Count(); i++)
                {
                    if (response.Booking.Passengers[i].PassengerInfants.Length != 0)
                    {
                        PassID[index] = response.Booking.Passengers[i].PassengerNumber.ToString();
                        index++;
                    }
                }
                //PassID = new string[count];
                return PassID;// response.Booking.Passengers[0];
            }
            //amended by diana 20131210 - try catch to check for valid booking
            catch (TimeoutException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), "", RecordLocator); return null; }
            catch (OutOfMemoryException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), "", RecordLocator); return null; }
            catch (IndexOutOfRangeException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), "", RecordLocator); return null; }
            catch (ThreadInterruptedException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), "", RecordLocator); return null; }
            catch (NullReferenceException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), "", RecordLocator); return null; }
            catch (StackOverflowException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), "", RecordLocator); return null; }
            catch (ApplicationException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), "", RecordLocator); return null; }
            catch (Exception ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), "", RecordLocator); return null; }

        }


        public Booking GetBookingFromState(string sessionID, int cnt = 0)
        {
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;

            if (cnt > 0) Thread.Sleep(2000); //added by diana 20140205 - reconnect purpose, amended by ketee, delay only if rerun, 20170916

            try
            {
                // Create an instance of the BookingService interface 
                BookingManagerClient bookingAPI = new BookingManagerClient();
                using (profiler.Step("GetBookingFromState"))
                {
                    return bookingAPI.GetBookingFromState(this.ContractVersion, sessionID);
                }
            }
           catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex, "GetBookingFromState: sessionkey:" + sessionID + "; ");

                if (ex.Message.ToString().Trim().ToUpper().Contains("NO SUCH SESSION"))
                {
                    return null;
                }
                else
                {
                    //amended by diana 20140205 - try to get data for max of 5 attempts
                    cnt += 1;
                    if (cnt <= 5)
                    {
                        return GetBookingFromState(sessionID, cnt);
                    }
                    else
                    {
                        return null;
                    }
                    //end amended by diana 20140205 - try to get data for max of 5 attempts
                }

            }
        }

        //added by ketee, retrieve seat amount by flight type
        public decimal GetSeatAmountByFlightType()
        {
            try
            {

                return 0;
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                log.Error(this, ex, "GetSeatAmountByFlightType ");
                return 0;
            }
            finally
            {
            }

        }

        //added by ketee, GetSSRAvailability
        public GetSSRAvailabilityResponse GetSSRAvailability(string SellSessionID, string carrierCode, string flightNumber, DateTime departureDate, string origin, string destination)
        {
            IBookingManager bookingAPI = new BookingManagerClient();
            ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking("");

            GetSSRAvailabilityRequest request = new BookingManager.GetSSRAvailabilityRequest();
            SSRAvailabilityRequest SSRAvailRequest = new SSRAvailabilityRequest();
            GetSSRAvailabilityResponse response = new GetSSRAvailabilityResponse();

            request.ContractVersion = this.ContractVersion;
            request.Signature = SellSessionID;

            string flight = "";
            string goingDate = "";
            string originDestination = "";

            if (flightNumber.Trim().Length <= 3)
                flight = carrierCode.Trim() + " " + flightNumber.Trim();
            else
                flight = carrierCode.Trim() + flightNumber.Trim();

            goingDate = departureDate.ToString("yyyyMMdd");
            originDestination = origin + destination;


            SSRAvailRequest.InventoryLegKeys = new string[1];
            SSRAvailRequest.InventoryLegKeys[0] = goingDate + " " + flight + " " + originDestination;
            SSRAvailRequest.SSRCollectionsMode = SSRCollectionsMode.Segment;

            request.SSRAvailabilityRequest = new SSRAvailabilityRequest();
            request.SSRAvailabilityRequest = SSRAvailRequest;

            response = bookingAPI.GetSSRAvailability(request);

            if (response != null) return response;

            return null;
        }

        public GetSSRAvailabilityForBookingResponse GetSSRAvailabilityForBooking(BookingManager.Booking booking, string SellSessionID)
        {
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;

            //Ensure that a booking is in state before using. Here we assume
            //the object booking holds the contents of booking state.
            IBookingManager bookingAPI = new BookingManagerClient();
            ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking("");

            GetSSRAvailabilityForBookingRequest bookingRequest = new GetSSRAvailabilityForBookingRequest();
            GetSSRAvailabilityRequest request = new GetSSRAvailabilityRequest();
            SSRAvailabilityRequest SSRAvailRequest = new SSRAvailabilityRequest();

            bookingRequest.Signature = SellSessionID;
            SSRAvailabilityForBookingRequest ssrAvailabilityForBookingRequest = new SSRAvailabilityForBookingRequest();

            SSRAvailRequest.SSRCollectionsMode = SSRCollectionsMode.Segment;
            //SSRAvailRequest.InventoryLegKeys = new System.ComponentModel.BindingList<string>();
            //System.ComponentModel.BindingList<string> list = new System.ComponentModel.BindingList<string>(SSRAvailRequest.InventoryLegKeys);

            //setting currency code is so that the system know which currency to return the pricing in, this should ideally be the same as the booking currency
            ssrAvailabilityForBookingRequest.CurrencyCode = booking.CurrencyCode;

            //the InventoryControlled and NonInventoryControlled items are mutually exclusive but we generally would want to retrieve both types of SSRs
            ssrAvailabilityForBookingRequest.InventoryControlled = true;
            ssrAvailabilityForBookingRequest.NonInventoryControlled = true;

            //NonSeatDependent and SeatDependent items are also mutually exclusive but we generally would want to retrieve both types of SSRs as well.
            ssrAvailabilityForBookingRequest.NonSeatDependent = true;
            ssrAvailabilityForBookingRequest.SeatDependent = true;

            //If we list more than 1 pax in this one, we would potentially get more than 1 set of results so I am only passing through 1st pax info
            ssrAvailabilityForBookingRequest.PassengerNumberList = new short[1];
            ssrAvailabilityForBookingRequest.PassengerNumberList[0] = 0;

            Journey[] journeys = booking.Journeys;

            //we need to feed the SegmentKeyList with each of the segments in the booking (per segment basis), the multiplication is based on
            //the assumption that both direction have the same number of segments. For accurate computation, you will need to count the actual total
            //number of segments instead of just doing the multiplication as such
            int count = 0;
            for (int i = 0; i < journeys.Length; i++)
            {
                count += journeys[i].Segments.Length;
            }
            ssrAvailabilityForBookingRequest.SegmentKeyList = new LegKey[count]; // journeys.Length * journeys[0].Segments.Length];

            count = 0;
            //this helps to populate each of the SegmentKeyList item with flight info info from each segment
            for (int i = 0; i < journeys.Length; i++)
            {
                for (int j = 0; j < journeys[i].Segments.Length; j++)
                {
                    ssrAvailabilityForBookingRequest.SegmentKeyList[count] = new LegKey();
                    ssrAvailabilityForBookingRequest.SegmentKeyList[count].DepartureStation = journeys[i].Segments[j].DepartureStation;
                    ssrAvailabilityForBookingRequest.SegmentKeyList[count].ArrivalStation = journeys[i].Segments[j].ArrivalStation;
                    ssrAvailabilityForBookingRequest.SegmentKeyList[count].DepartureDate = journeys[i].Segments[j].STD;

                    ssrAvailabilityForBookingRequest.SegmentKeyList[count].CarrierCode = journeys[i].Segments[j].FlightDesignator.CarrierCode;
                    ssrAvailabilityForBookingRequest.SegmentKeyList[count].FlightNumber = journeys[i].Segments[j].FlightDesignator.FlightNumber;
                    ssrAvailabilityForBookingRequest.SegmentKeyList[count].OpSuffix = journeys[i].Segments[j].FlightDesignator.OpSuffix;

                    count++;
                }
            }

            bookingRequest.SSRAvailabilityForBookingRequest = ssrAvailabilityForBookingRequest;
            //string ssrrequest = GetXMLString(ssrAvailabilityForBookingRequest);
            //Call GetSSRAvailabilityForBooking method here
            //string xml = GetXMLString(bookingRequest);
            GetSSRAvailabilityForBookingResponse response;// = bookingAPI.GetSSRAvailabilityForBooking(bookingRequest);
            using (profiler.Step("Navitaire:GetSSRAvailabilityForBooking"))
            {
                response = bookingAPI.GetSSRAvailabilityForBooking(bookingRequest);
            }
            //string xmlresponse = GetXMLString(response);

            return response;
            // check that there's results before proceeding

        }

        public SellResponse SellSSR(string SellSessionID, GetSSRAvailabilityForBookingResponse response, DataRow[] dtList1, DataRow[] dtList2, Boolean delete, string PNR = "", string Currency = "", string change = "")
        {
            short count = 0;
            try
            {
                // SellSSRRequest Contains a collection of SSRs to be sold or to be associated to a booking
                //Create an instance of BookingManagerClient
                IBookingManager bookingAPI = new BookingManagerClient();
                // SellSSRRequest Contains a collection of SSRs to be sold or to be associated to a booking
                SellRequest sellSsrRequest = new SellRequest();
                SellRequestData sellreqd = new SellRequestData();
                sellreqd.SellBy = SellBy.SSR;
                sellreqd.SellSSR = new SellSSR();
                sellreqd.SellSSR.SSRRequest = new SSRRequest();
                sellreqd.SellSSR.SSRRequest.CancelFirstSSR = delete;
                int SSRRequestCount = response.SSRAvailabilityForBookingResponse.SSRSegmentList.Length;
                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests = new SegmentSSRRequest[SSRRequestCount];
                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0] = new SegmentSSRRequest();

                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs = new PaxSSR[dtList1.Length];
                for (int i = 0; i < sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs.Length; i++)
                {

                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i] = new PaxSSR();
                    //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].ActionStatusCode = "NN";
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].ActionStatusCode = "SS";
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].State = BookingManager.MessageState.New;
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRCode = dtList1[i]["SSRCode"].ToString();
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRNumber = 0;
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].DepartureStation = dtList1[i]["Origin"].ToString();
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].ArrivalStation = dtList1[i]["Destination"].ToString();
                    if (i != 0)
                    {
                        if (Convert.ToInt16(dtList1[i]["PassengerNo"]) == Convert.ToInt16(dtList1[i - 1]["PassengerNo"]))
                        {
                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = count;
                        }
                        else
                        {
                            count += 1;
                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = count;
                        }
                    }
                    else
                    {
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = count;
                    }
                    //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList1[i]["PassengerID"]);
                }
                count = 0;
                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].DepartureStation =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.DepartureStation;
                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].ArrivalStation =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.ArrivalStation;
                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].STD =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.DepartureDate;
                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator = new FlightDesignator();
                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator.CarrierCode =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.CarrierCode;
                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator.FlightNumber =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.FlightNumber;



                if (SSRRequestCount > 1)
                {
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1] = new SegmentSSRRequest();

                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs = new PaxSSR[dtList2.Length];
                    for (int i = 0; i < sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs.Length; i++)
                    {
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i] = new PaxSSR();
                        //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].ActionStatusCode = "NN";
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].ActionStatusCode = "SS";
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].State = BookingManager.MessageState.New;
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].SSRCode = dtList2[i]["SSRCode"].ToString();
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].SSRNumber = 0;
                        if (change != "")
                        {
                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].DepartureStation = dtList2[i]["Destination"].ToString();
                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].ArrivalStation = dtList2[i]["Origin"].ToString();
                        }
                        else
                        {
                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].DepartureStation = dtList2[i]["Origin"].ToString();
                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].ArrivalStation = dtList2[i]["Destination"].ToString();
                        }
                        if (i != 0)
                        {
                            if (Convert.ToInt16(dtList2[i]["PassengerNo"]) == Convert.ToInt16(dtList2[i - 1]["PassengerNo"]))
                            {
                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].PassengerNumber = count;
                            }
                            else
                            {
                                count += 1;
                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].PassengerNumber = count;
                            }
                        }
                        else
                        {
                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].PassengerNumber = count;
                        }
                        //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList2[i]["PassengerID"]);
                    }
                    count = 0;
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1].DepartureStation =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.DepartureStation;
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1].ArrivalStation =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.ArrivalStation;
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1].STD =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.DepartureDate;
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1].FlightDesignator = new FlightDesignator();
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1].FlightDesignator.CarrierCode =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.CarrierCode;
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1].FlightDesignator.FlightNumber =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.FlightNumber;

                }
                sellSsrRequest.Signature = SellSessionID;
                sellSsrRequest.ContractVersion = this.ContractVersion;
                sellSsrRequest.SellRequestData = sellreqd;
                SellResponse sellSsrResponse = null;
                sellSsrResponse = bookingAPI.Sell(sellSsrRequest);

                //if (change != "")
                //{
                //    string msg = "";
                //    BookingCommitFlightChange(PNR, SellSessionID, ref msg, Currency, false, true);
                //}
                //string xml = GetXMLString(sellSsrRequest);
                return sellSsrResponse;
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                log.Error(this, ex, "SellSSR 1:" + ex.Message);
                return null;
            }

        }

        public string SellSSR(string PNR, GetSSRAvailabilityForBookingResponse response, DataRow[] dtList1, DataRow[] dtList2)
        {
            short count = 0;
            var profiler = MiniProfiler.Current;
            try
            {
                // SellSSRRequest Contains a collection of SSRs to be sold or to be associated to a booking
                //Create an instance of BookingManagerClient
                IBookingManager bookingAPI = new BookingManagerClient();
                // SellSSRRequest Contains a collection of SSRs to be sold or to be associated to a booking
                string SellSessionID = "";
                HttpContext.Current.Session["Commit"] = false;
                SellSessionID = AgentLogon();
                GetBookingResponse resp = new GetBookingResponse();// GetBookingByPNR(PNR, SellSessionID);
                using (profiler.Step("Navitaire:GetBooking"))
                {
                    resp = GetBookingByPNR(PNR, SellSessionID);
                }

                CancelRequest cancelSSRs = new CancelRequest();
                cancelSSRs.Signature = SellSessionID;
                cancelSSRs.ContractVersion = this.ContractVersion;
                cancelSSRs.CancelRequestData = new CancelRequestData();
                cancelSSRs.CancelRequestData.CancelBy = CancelBy.SSR;
                cancelSSRs.CancelRequestData.CancelSSR = new CancelSSR();
                SSRRequest sellreqd = new SSRRequest();
                sellreqd.CurrencyCode = HttpContext.Current.Session["Currency"].ToString();
                sellreqd.CancelFirstSSR = true;
                int SSRRequestCount = response.SSRAvailabilityForBookingResponse.SSRSegmentList.Length;
                sellreqd.SegmentSSRRequests = new SegmentSSRRequest[SSRRequestCount];

                DataTable dt1 = new DataTable();
                dt1.Columns.Add("SSRCode");
                dt1.Columns.Add("SSRNumber");
                dt1.Columns.Add("PassengerNumber");

                DataTable dt2 = new DataTable();
                dt2.Columns.Add("SSRCode");
                dt2.Columns.Add("SSRNumber");
                dt2.Columns.Add("PassengerNumber");

                for (int i = 0; i < resp.Booking.Passengers.Length; i++)
                {
                    for (int ii = 0; ii < resp.Booking.Passengers[i].PassengerFees.Length; ii++)
                    {
                        if (resp.Booking.Passengers[i].PassengerFees[ii].FeeType == FeeType.SSRFee && resp.Booking.Passengers[i].PassengerFees[ii].SSRCode != "INFT")
                        {
                            if (resp.Booking.Passengers[i].PassengerFees[ii].FlightReference != "")
                            {
                                if ((resp.Booking.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.DepartureStation + response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.ArrivalStation))
                                {
                                    //SSRLength1 += 1;
                                    dt1.Rows.Add(resp.Booking.Passengers[i].PassengerFees[ii].SSRCode, resp.Booking.Passengers[i].PassengerFees[ii].SSRNumber, resp.Booking.Passengers[i].PassengerNumber);

                                }
                                else if (SSRRequestCount > 1)
                                {
                                    if ((resp.Booking.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.DepartureStation + response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.ArrivalStation))
                                    {
                                        dt2.Rows.Add(resp.Booking.Passengers[i].PassengerFees[ii].SSRCode, resp.Booking.Passengers[i].PassengerFees[ii].SSRNumber, resp.Booking.Passengers[i].PassengerNumber);
                                    }
                                }
                            }
                        }
                    }
                }


                sellreqd.SegmentSSRRequests[0] = new SegmentSSRRequest();
                sellreqd.SegmentSSRRequests[0].PaxSSRs = new PaxSSR[dt1.Rows.Count];
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i] = new PaxSSR();
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].ActionStatusCode = "NN";
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].SSRCode = dt1.Rows[i]["SSRCode"].ToString();
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].SSRNumber = Convert.ToInt16(dt1.Rows[i]["SSRNumber"]);
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].DepartureStation = resp.Booking.Journeys[0].Segments[0].DepartureStation;
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].ArrivalStation = resp.Booking.Journeys[0].Segments[0].ArrivalStation;
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dt1.Rows[i]["PassengerNumber"]);
                }
                if (SSRRequestCount > 1)
                {
                    sellreqd.SegmentSSRRequests[1] = new SegmentSSRRequest();
                    sellreqd.SegmentSSRRequests[1].PaxSSRs = new PaxSSR[dt2.Rows.Count];
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i] = new PaxSSR();
                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i].ActionStatusCode = "NN";
                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i].SSRCode = dt2.Rows[i]["SSRCode"].ToString();
                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i].SSRNumber = Convert.ToInt16(dt2.Rows[i]["SSRNumber"]);
                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i].DepartureStation = resp.Booking.Journeys[1].Segments[0].DepartureStation;
                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i].ArrivalStation = resp.Booking.Journeys[1].Segments[0].ArrivalStation;
                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dt2.Rows[i]["PassengerNumber"]);
                    }
                }

                //for (int i = 0; i < resp.Booking.Passengers.Length; i++)
                //{
                //    for (int ii = 0; ii < resp.Booking.Passengers[i].PassengerFees.Length; ii++)
                //    {
                //        if (resp.Booking.Passengers[i].PassengerFees[ii].FeeType == FeeType.SSRFee && resp.Booking.Passengers[i].PassengerFees[ii].SSRCode != "INFT")
                //        {
                //            if (resp.Booking.Passengers[i].PassengerFees[ii].FlightReference != "")
                //            {
                //                if ((resp.Booking.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.DepartureStation + response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.ArrivalStation))
                //                {
                //                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i] = new PaxSSR();
                //                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].ActionStatusCode = "NN";
                //                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].SSRCode = resp.Booking.Passengers[i].PassengerFees[ii].SSRCode;
                //                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].SSRNumber = resp.Booking.Passengers[i].PassengerFees[ii].SSRNumber;
                //                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].DepartureStation = resp.Booking.Journeys[0].Segments[0].DepartureStation;
                //                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].ArrivalStation = resp.Booking.Journeys[0].Segments[0].ArrivalStation;
                //                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = (short)resp.Booking.Passengers[i].PassengerNumber;
                //                }
                //                else if (SSRRequestCount > 1)
                //                {
                //                    if ((resp.Booking.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.DepartureStation + response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.ArrivalStation))
                //                    {
                //                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i] = new PaxSSR();
                //                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i].ActionStatusCode = "NN";
                //                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i].SSRCode = resp.Booking.Passengers[i].PassengerFees[ii].SSRCode;
                //                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i].SSRNumber = resp.Booking.Passengers[i].PassengerFees[ii].SSRNumber;
                //                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i].DepartureStation = resp.Booking.Journeys[1].Segments[0].DepartureStation;
                //                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i].ArrivalStation = resp.Booking.Journeys[1].Segments[0].ArrivalStation;
                //                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i].PassengerNumber = (short)resp.Booking.Passengers[i].PassengerNumber;
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}

                count = 0;
                sellreqd.SegmentSSRRequests[0].DepartureStation =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.DepartureStation;
                sellreqd.SegmentSSRRequests[0].ArrivalStation =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.ArrivalStation;
                sellreqd.SegmentSSRRequests[0].STD =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.DepartureDate;
                sellreqd.SegmentSSRRequests[0].FlightDesignator = new FlightDesignator();
                sellreqd.SegmentSSRRequests[0].FlightDesignator.CarrierCode =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.CarrierCode;
                sellreqd.SegmentSSRRequests[0].FlightDesignator.FlightNumber =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.FlightNumber;



                if (SSRRequestCount > 1)
                {

                    sellreqd.SegmentSSRRequests[1].DepartureStation =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.DepartureStation;
                    sellreqd.SegmentSSRRequests[1].ArrivalStation =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.ArrivalStation;
                    sellreqd.SegmentSSRRequests[1].STD =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.DepartureDate;
                    sellreqd.SegmentSSRRequests[1].FlightDesignator = new FlightDesignator();
                    sellreqd.SegmentSSRRequests[1].FlightDesignator.CarrierCode =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.CarrierCode;
                    sellreqd.SegmentSSRRequests[1].FlightDesignator.FlightNumber =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.FlightNumber;

                }

                cancelSSRs.CancelRequestData.CancelSSR.SSRRequest = sellreqd;
                CancelResponse cancelResponse = bookingAPI.Cancel(cancelSSRs);

                SellRequest sellSsrRequest = new SellRequest();
                SellRequestData sellreqds = new SellRequestData();
                sellreqds.SellBy = SellBy.SSR;
                sellreqds.SellSSR = new SellSSR();
                sellreqds.SellSSR.SSRRequest = new SSRRequest();
                sellreqds.SellSSR.SSRRequest.CancelFirstSSR = true;
                SSRRequestCount = response.SSRAvailabilityForBookingResponse.SSRSegmentList.Length;
                sellreqds.SellSSR.SSRRequest.SegmentSSRRequests = new SegmentSSRRequest[SSRRequestCount];
                sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0] = new SegmentSSRRequest();

                sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs = new PaxSSR[dtList1.Length];
                for (int i = 0; i < sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs.Length; i++)
                {

                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i] = new PaxSSR();
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].ActionStatusCode = "SS";
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].State = BookingManager.MessageState.New;
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRCode = dtList1[i]["SSRCode"].ToString();
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRNumber = 0;
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].DepartureStation = dtList1[i]["Origin"].ToString();
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].ArrivalStation = dtList1[i]["Destination"].ToString();
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList1[i]["PassengerID"]);
                }
                count = 0;
                sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].DepartureStation =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.DepartureStation;
                sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].ArrivalStation =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.ArrivalStation;
                sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].STD =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.DepartureDate;
                sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator = new FlightDesignator();
                sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator.CarrierCode =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.CarrierCode;
                sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator.FlightNumber =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.FlightNumber;



                if (SSRRequestCount > 1)
                {
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1] = new SegmentSSRRequest();

                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs = new PaxSSR[dtList2.Length];
                    for (int i = 0; i < sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs.Length; i++)
                    {
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i] = new PaxSSR();
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].ActionStatusCode = "SS";
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].State = BookingManager.MessageState.New;
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].SSRCode = dtList2[i]["SSRCode"].ToString();
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].SSRNumber = 0;
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].DepartureStation = dtList2[i]["Origin"].ToString();
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].ArrivalStation = dtList2[i]["Destination"].ToString();
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList2[i]["PassengerID"]);
                    }
                    count = 0;
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].DepartureStation =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.DepartureStation;
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].ArrivalStation =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.ArrivalStation;
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].STD =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.DepartureDate;
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].FlightDesignator = new FlightDesignator();
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].FlightDesignator.CarrierCode =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.CarrierCode;
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].FlightDesignator.FlightNumber =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.FlightNumber;

                }
                sellSsrRequest.Signature = SellSessionID;
                sellSsrRequest.ContractVersion = this.ContractVersion;
                sellSsrRequest.SellRequestData = sellreqds;
                SellResponse sellSsrResponse = null;
                sellSsrResponse = bookingAPI.Sell(sellSsrRequest);
                //string xml = GetXMLString(sellSsrRequest);
                string msg = "";
                if (resp.Booking.BookingSum.BalanceDue > 0 || sellSsrResponse.BookingUpdateResponseData.Success.PNRAmount.BalanceDue == 0)
                {
                    using (profiler.Step("Navitaire:BookingCommit"))
                    {
                        BookingCommitChange(PNR, SellSessionID, ref msg, HttpContext.Current.Session["Currency"].ToString(), true, true);
                    }
                    HttpContext.Current.Session["Commit"] = true;
                }
                return SellSessionID;
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                log.Error(this, ex, "SellSSR 2:" + ex.Message);
                return "";
            }

        }

        public SellResponse SellSSRTransit(string SellSessionID, GetSSRAvailabilityForBookingResponse response, DataRow[] dtList1, DataRow[] dtList2, DataRow[] dtList1t, DataRow[] dtList2t, Boolean delete, string PNR = "", string Currency = "", string change = "")
        {
            short count = 0;
            try
            {
                // SellSSRRequest Contains a collection of SSRs to be sold or to be associated to a booking
                //Create an instance of BookingManagerClient
                IBookingManager bookingAPI = new BookingManagerClient();
                // SellSSRRequest Contains a collection of SSRs to be sold or to be associated to a booking
                SellRequest sellSsrRequest = new SellRequest();
                SellRequestData sellreqd = new SellRequestData();
                sellreqd.SellBy = SellBy.SSR;
                sellreqd.SellSSR = new SellSSR();
                sellreqd.SellSSR.SSRRequest = new SSRRequest();
                sellreqd.SellSSR.SSRRequest.CancelFirstSSR = delete;
                int SSRRequestCount = response.SSRAvailabilityForBookingResponse.SSRSegmentList.Length;
                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests = new SegmentSSRRequest[SSRRequestCount];
                int index = 0;
                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index] = new SegmentSSRRequest();

                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs = new PaxSSR[dtList1.Length];
                for (int i = 0; i < sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs.Length; i++)
                {
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i] = new PaxSSR();
                    //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].ActionStatusCode = "NN";
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].ActionStatusCode = "SS";
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].State = BookingManager.MessageState.Modified;
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].SSRCode = dtList1[i]["SSRCode"].ToString();
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].SSRNumber = 0;
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].DepartureStation = dtList1[i]["Origin"].ToString();
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].ArrivalStation = dtList1[i]["Destination"].ToString();
                    if (i != 0)
                    {
                        if (Convert.ToInt16(dtList1[i]["PassengerNo"]) == Convert.ToInt16(dtList1[i - 1]["PassengerNo"]))
                        {
                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].PassengerNumber = count;
                        }
                        else
                        {
                            count += 1;
                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].PassengerNumber = count;
                        }
                    }
                    else
                    {
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].PassengerNumber = count;
                    }
                    //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList1[i]["PassengerID"]);
                }
                count = 0;
                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].DepartureStation = response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.DepartureStation;
                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].ArrivalStation = response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.ArrivalStation;
                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].STD = response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.DepartureDate;
                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].FlightDesignator = new FlightDesignator();
                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].FlightDesignator.CarrierCode = response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.CarrierCode;
                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].FlightDesignator.FlightNumber = response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.FlightNumber;


                if (SSRRequestCount > (index + 1) && dtList1t != null && dtList1t.Length > 0)
                {
                    index += 1;
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index] = new SegmentSSRRequest();

                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs = new PaxSSR[dtList1t.Length];
                    for (int i = 0; i < sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs.Length; i++)
                    {
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i] = new PaxSSR();
                        //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].ActionStatusCode = "NN";
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].ActionStatusCode = "SS";
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].State = BookingManager.MessageState.Modified;
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].SSRCode = dtList1t[i]["SSRCode"].ToString();
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].SSRNumber = 0;
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].DepartureStation = dtList1t[i]["Origin"].ToString();
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].ArrivalStation = dtList1t[i]["Destination"].ToString();
                        if (i != 0)
                        {
                            if (Convert.ToInt16(dtList1t[i]["PassengerNo"]) == Convert.ToInt16(dtList1t[i - 1]["PassengerNo"]))
                            {
                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].PassengerNumber = count;
                            }
                            else
                            {
                                count += 1;
                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].PassengerNumber = count;
                            }
                        }
                        else
                        {
                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].PassengerNumber = count;
                        }
                        //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList1t[i]["PassengerID"]);
                    }
                    count = 0;
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].DepartureStation = response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.DepartureStation;
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].ArrivalStation = response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.ArrivalStation;
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].STD = response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.DepartureDate;
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].FlightDesignator = new FlightDesignator();
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].FlightDesignator.CarrierCode = response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.CarrierCode;
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].FlightDesignator.FlightNumber = response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.FlightNumber;
                }

                if (SSRRequestCount > (index + 1) && dtList2 != null && dtList2.Length > 0)
                {
                    index += 1;
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index] = new SegmentSSRRequest();

                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs = new PaxSSR[dtList2.Length];
                    for (int i = 0; i < sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs.Length; i++)
                    {
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i] = new PaxSSR();
                        //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].ActionStatusCode = "NN";
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].ActionStatusCode = "SS";
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].State = BookingManager.MessageState.Modified;
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].SSRCode = dtList2[i]["SSRCode"].ToString();
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].SSRNumber = 0;
                        if (change != "")
                        {
                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].DepartureStation = response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.DepartureStation;
                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].ArrivalStation = response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.ArrivalStation;
                        }
                        else
                        {
                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].DepartureStation = dtList2[i]["Origin"].ToString();
                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].ArrivalStation = dtList2[i]["Destination"].ToString();
                        }

                        if (i != 0)
                        {
                            if (Convert.ToInt16(dtList2[i]["PassengerNo"]) == Convert.ToInt16(dtList2[i - 1]["PassengerNo"]))
                            {
                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].PassengerNumber = count;
                            }
                            else
                            {
                                count += 1;
                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].PassengerNumber = count;
                            }
                        }
                        else
                        {
                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].PassengerNumber = count;
                        }
                        //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList2[i]["PassengerID"]);
                    }
                    count = 0;
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].DepartureStation = response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.DepartureStation;
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].ArrivalStation = response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.ArrivalStation;
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].STD = response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.DepartureDate;
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].FlightDesignator = new FlightDesignator();
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].FlightDesignator.CarrierCode = response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.CarrierCode;
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].FlightDesignator.FlightNumber = response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.FlightNumber;
                }


                if (SSRRequestCount > (index + 1) && dtList2t != null && dtList2t.Length > 0)
                {
                    index += 1;


                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index] = new SegmentSSRRequest();

                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs = new PaxSSR[dtList2t.Length];
                    for (int i = 0; i < sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs.Length; i++)
                    {
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i] = new PaxSSR();
                        //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].ActionStatusCode = "NN";
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].ActionStatusCode = "SS";
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].State = BookingManager.MessageState.Modified;
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].SSRCode = dtList2t[i]["SSRCode"].ToString();
                        sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].SSRNumber = 0;
                        if (change != "")
                        {
                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].DepartureStation = response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.DepartureStation;
                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].ArrivalStation = response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.ArrivalStation;
                        }
                        else
                        {
                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].DepartureStation = dtList2t[i]["Origin"].ToString();
                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].ArrivalStation = dtList2t[i]["Destination"].ToString();
                        }
                        if (i != 0)
                        {
                            if (Convert.ToInt16(dtList2t[i]["PassengerNo"]) == Convert.ToInt16(dtList2t[i - 1]["PassengerNo"]))
                            {
                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].PassengerNumber = count;
                            }
                            else
                            {
                                count += 1;
                                sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].PassengerNumber = count;
                            }
                        }
                        else
                        {
                            sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].PassengerNumber = count;
                        }
                        //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList2t[i]["PassengerID"]);
                    }
                    count = 0;
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].DepartureStation = response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.DepartureStation;
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].ArrivalStation = response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.ArrivalStation;
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].STD = response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.DepartureDate;
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].FlightDesignator = new FlightDesignator();
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].FlightDesignator.CarrierCode = response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.CarrierCode;
                    sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[index].FlightDesignator.FlightNumber = response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.FlightNumber;

                }
                sellSsrRequest.Signature = SellSessionID;
                sellSsrRequest.ContractVersion = this.ContractVersion;
                sellSsrRequest.SellRequestData = sellreqd;
                SellResponse sellSsrResponse = null;
                sellSsrResponse = bookingAPI.Sell(sellSsrRequest);
                //string xml = GetXMLString(sellSsrRequest);

                //if (change != "")
                //{
                //    string msg = "";
                //    BookingCommitFlightChange(PNR, SellSessionID, ref msg, Currency, false, true);
                //}
                return sellSsrResponse;

            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                log.Error(this, ex, "SellSession " + ex.Message);
                return null;
            }

        }

        public string SellSSRTransit(string PNR, GetSSRAvailabilityForBookingResponse response, DataRow[] dtList1, DataRow[] dtList2, DataRow[] dtList1t, DataRow[] dtList2t, string group)
        {
            short count = 0;
            var profiler = MiniProfiler.Current;
            try
            {
                // SellSSRRequest Contains a collection of SSRs to be sold or to be associated to a booking
                //Create an instance of BookingManagerClient
                IBookingManager bookingAPI = new BookingManagerClient();
                // SellSSRRequest Contains a collection of SSRs to be sold or to be associated to a booking
                HttpContext.Current.Session["Commit"] = false;
                string SellSessionID = AgentLogon();
                GetBookingResponse resp = new GetBookingResponse();// GetBookingByPNR(PNR, SellSessionID);
                using (profiler.Step("Navitaire:GetBooking"))
                {
                    resp = GetBookingByPNR(PNR, SellSessionID);
                }

                CancelRequest cancelSSRs = new CancelRequest();
                cancelSSRs.Signature = SellSessionID;
                cancelSSRs.ContractVersion = this.ContractVersion;
                cancelSSRs.CancelRequestData = new CancelRequestData();
                cancelSSRs.CancelRequestData.CancelBy = CancelBy.SSR;
                cancelSSRs.CancelRequestData.CancelSSR = new CancelSSR();
                SSRRequest sellreqd = new SSRRequest();
                sellreqd.CurrencyCode = HttpContext.Current.Session["Currency"].ToString();
                sellreqd.CancelFirstSSR = true;
                int SSRRequestCount = response.SSRAvailabilityForBookingResponse.SSRSegmentList.Length;
                sellreqd.SegmentSSRRequests = new SegmentSSRRequest[SSRRequestCount];

                DataTable dt1 = new DataTable();
                dt1.Columns.Add("SSRCode");
                dt1.Columns.Add("SSRNumber");
                dt1.Columns.Add("PassengerNumber");

                DataTable dt2 = new DataTable();
                dt2.Columns.Add("SSRCode");
                dt2.Columns.Add("SSRNumber");
                dt2.Columns.Add("PassengerNumber");

                DataTable dt3 = new DataTable();
                dt3.Columns.Add("SSRCode");
                dt3.Columns.Add("SSRNumber");
                dt3.Columns.Add("PassengerNumber");

                DataTable dt4 = new DataTable();
                dt4.Columns.Add("SSRCode");
                dt4.Columns.Add("SSRNumber");
                dt4.Columns.Add("PassengerNumber");


                for (int i = 0; i < resp.Booking.Passengers.Length; i++)
                {
                    for (int ii = 0; ii < resp.Booking.Passengers[i].PassengerFees.Length; ii++)
                    {
                        if (resp.Booking.Passengers[i].PassengerFees[ii].FeeType == FeeType.SSRFee && resp.Booking.Passengers[i].PassengerFees[ii].SSRCode != "INFT")
                        {
                            if (resp.Booking.Passengers[i].PassengerFees[ii].FlightReference != "")
                            {
                                if ((resp.Booking.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.DepartureStation + response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.ArrivalStation))
                                {
                                    dt1.Rows.Add(resp.Booking.Passengers[i].PassengerFees[ii].SSRCode, resp.Booking.Passengers[i].PassengerFees[ii].SSRNumber, resp.Booking.Passengers[i].PassengerNumber);

                                }
                                else if ((resp.Booking.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.DepartureStation + response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.ArrivalStation))
                                {
                                    dt2.Rows.Add(resp.Booking.Passengers[i].PassengerFees[ii].SSRCode, resp.Booking.Passengers[i].PassengerFees[ii].SSRNumber, resp.Booking.Passengers[i].PassengerNumber);

                                }
                                else if (SSRRequestCount > 2)
                                {
                                    if ((resp.Booking.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == response.SSRAvailabilityForBookingResponse.SSRSegmentList[2].LegKey.DepartureStation + response.SSRAvailabilityForBookingResponse.SSRSegmentList[2].LegKey.ArrivalStation))
                                    {
                                        dt3.Rows.Add(resp.Booking.Passengers[i].PassengerFees[ii].SSRCode, resp.Booking.Passengers[i].PassengerFees[ii].SSRNumber, resp.Booking.Passengers[i].PassengerNumber);
                                    }
                                    else if ((resp.Booking.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == response.SSRAvailabilityForBookingResponse.SSRSegmentList[3].LegKey.DepartureStation + response.SSRAvailabilityForBookingResponse.SSRSegmentList[3].LegKey.ArrivalStation))
                                    {
                                        dt4.Rows.Add(resp.Booking.Passengers[i].PassengerFees[ii].SSRCode, resp.Booking.Passengers[i].PassengerFees[ii].SSRNumber, resp.Booking.Passengers[i].PassengerNumber);
                                    }
                                }
                            }
                        }
                    }
                }
                sellreqd.SegmentSSRRequests[0] = new SegmentSSRRequest();
                sellreqd.SegmentSSRRequests[0].PaxSSRs = new PaxSSR[dt1.Rows.Count];
                sellreqd.SegmentSSRRequests[1] = new SegmentSSRRequest();
                sellreqd.SegmentSSRRequests[1].PaxSSRs = new PaxSSR[dt2.Rows.Count];
                if (SSRRequestCount > 2)
                {
                    sellreqd.SegmentSSRRequests[2] = new SegmentSSRRequest();
                    sellreqd.SegmentSSRRequests[2].PaxSSRs = new PaxSSR[dt3.Rows.Count];
                    if (group == "transit")
                    {
                        sellreqd.SegmentSSRRequests[3] = new SegmentSSRRequest();
                        sellreqd.SegmentSSRRequests[3].PaxSSRs = new PaxSSR[dt4.Rows.Count];
                    }
                }

                sellreqd.SegmentSSRRequests[0] = new SegmentSSRRequest();
                sellreqd.SegmentSSRRequests[0].PaxSSRs = new PaxSSR[dt1.Rows.Count];
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i] = new PaxSSR();
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].ActionStatusCode = "NN";
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].SSRCode = dt1.Rows[i]["SSRCode"].ToString();
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].SSRNumber = Convert.ToInt16(dt1.Rows[i]["SSRNumber"]);
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].DepartureStation = resp.Booking.Journeys[0].Segments[0].DepartureStation;
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].ArrivalStation = resp.Booking.Journeys[0].Segments[0].ArrivalStation;
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dt1.Rows[i]["PassengerNumber"]);
                }

                sellreqd.SegmentSSRRequests[1] = new SegmentSSRRequest();
                sellreqd.SegmentSSRRequests[1].PaxSSRs = new PaxSSR[dt2.Rows.Count];
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    sellreqd.SegmentSSRRequests[1].PaxSSRs[i] = new PaxSSR();
                    sellreqd.SegmentSSRRequests[1].PaxSSRs[i].ActionStatusCode = "NN";
                    sellreqd.SegmentSSRRequests[1].PaxSSRs[i].SSRCode = dt2.Rows[i]["SSRCode"].ToString();
                    sellreqd.SegmentSSRRequests[1].PaxSSRs[i].SSRNumber = Convert.ToInt16(dt2.Rows[i]["SSRNumber"]);
                    if (group == "transitdepart" || group == "transit")
                    {
                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i].DepartureStation = resp.Booking.Journeys[0].Segments[1].DepartureStation;
                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i].ArrivalStation = resp.Booking.Journeys[0].Segments[1].ArrivalStation;
                    }
                    else if (group == "transitreturn")
                    {
                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i].DepartureStation = resp.Booking.Journeys[1].Segments[0].DepartureStation;
                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i].ArrivalStation = resp.Booking.Journeys[1].Segments[0].ArrivalStation;
                    }
                    sellreqd.SegmentSSRRequests[1].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dt2.Rows[i]["PassengerNumber"]);
                }
                if (SSRRequestCount > 2)
                {
                    sellreqd.SegmentSSRRequests[2] = new SegmentSSRRequest();
                    sellreqd.SegmentSSRRequests[2].PaxSSRs = new PaxSSR[dt3.Rows.Count];
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        sellreqd.SegmentSSRRequests[2].PaxSSRs[i] = new PaxSSR();
                        sellreqd.SegmentSSRRequests[2].PaxSSRs[i].ActionStatusCode = "NN";
                        sellreqd.SegmentSSRRequests[2].PaxSSRs[i].SSRCode = dt3.Rows[i]["SSRCode"].ToString();
                        sellreqd.SegmentSSRRequests[2].PaxSSRs[i].SSRNumber = Convert.ToInt16(dt3.Rows[i]["SSRNumber"]);
                        if (group == "transitdepart" || group == "transit")
                        {
                            sellreqd.SegmentSSRRequests[2].PaxSSRs[i].DepartureStation = resp.Booking.Journeys[1].Segments[0].DepartureStation;
                            sellreqd.SegmentSSRRequests[2].PaxSSRs[i].ArrivalStation = resp.Booking.Journeys[1].Segments[0].ArrivalStation;
                        }
                        else if (group == "transitreturn")
                        {
                            sellreqd.SegmentSSRRequests[2].PaxSSRs[i].DepartureStation = resp.Booking.Journeys[1].Segments[1].DepartureStation;
                            sellreqd.SegmentSSRRequests[2].PaxSSRs[i].ArrivalStation = resp.Booking.Journeys[1].Segments[1].ArrivalStation;
                        }
                        sellreqd.SegmentSSRRequests[2].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dt3.Rows[i]["PassengerNumber"]);
                    }

                    if (group == "transit")
                    {
                        sellreqd.SegmentSSRRequests[3] = new SegmentSSRRequest();
                        sellreqd.SegmentSSRRequests[3].PaxSSRs = new PaxSSR[dt4.Rows.Count];
                        for (int i = 0; i < dt3.Rows.Count; i++)
                        {
                            sellreqd.SegmentSSRRequests[3].PaxSSRs[i] = new PaxSSR();
                            sellreqd.SegmentSSRRequests[3].PaxSSRs[i].ActionStatusCode = "NN";
                            sellreqd.SegmentSSRRequests[3].PaxSSRs[i].SSRCode = dt4.Rows[i]["SSRCode"].ToString();
                            sellreqd.SegmentSSRRequests[3].PaxSSRs[i].SSRNumber = Convert.ToInt16(dt4.Rows[i]["SSRNumber"]);
                            sellreqd.SegmentSSRRequests[3].PaxSSRs[i].DepartureStation = resp.Booking.Journeys[1].Segments[1].DepartureStation;
                            sellreqd.SegmentSSRRequests[3].PaxSSRs[i].ArrivalStation = resp.Booking.Journeys[1].Segments[1].ArrivalStation;
                            sellreqd.SegmentSSRRequests[3].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dt4.Rows[i]["PassengerNumber"]);
                        }
                    }
                }
                sellreqd.SegmentSSRRequests[0].DepartureStation =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.DepartureStation;
                sellreqd.SegmentSSRRequests[0].ArrivalStation =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.ArrivalStation;
                sellreqd.SegmentSSRRequests[0].STD =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.DepartureDate;
                sellreqd.SegmentSSRRequests[0].FlightDesignator = new FlightDesignator();
                sellreqd.SegmentSSRRequests[0].FlightDesignator.CarrierCode =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.CarrierCode;
                sellreqd.SegmentSSRRequests[0].FlightDesignator.FlightNumber =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.FlightNumber;

                sellreqd.SegmentSSRRequests[1].DepartureStation =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.DepartureStation;
                sellreqd.SegmentSSRRequests[1].ArrivalStation =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.ArrivalStation;
                sellreqd.SegmentSSRRequests[1].STD =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.DepartureDate;
                sellreqd.SegmentSSRRequests[1].FlightDesignator = new FlightDesignator();
                sellreqd.SegmentSSRRequests[1].FlightDesignator.CarrierCode =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.CarrierCode;
                sellreqd.SegmentSSRRequests[1].FlightDesignator.FlightNumber =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.FlightNumber;


                if (SSRRequestCount > 2)
                {
                    sellreqd.SegmentSSRRequests[2].DepartureStation =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[2].LegKey.DepartureStation;
                    sellreqd.SegmentSSRRequests[2].ArrivalStation =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[2].LegKey.ArrivalStation;
                    sellreqd.SegmentSSRRequests[2].STD =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[2].LegKey.DepartureDate;
                    sellreqd.SegmentSSRRequests[2].FlightDesignator = new FlightDesignator();
                    sellreqd.SegmentSSRRequests[2].FlightDesignator.CarrierCode =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[2].LegKey.CarrierCode;
                    sellreqd.SegmentSSRRequests[2].FlightDesignator.FlightNumber =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[2].LegKey.FlightNumber;

                    if (group == "transit")
                    {
                        sellreqd.SegmentSSRRequests[3].DepartureStation =
                        response.SSRAvailabilityForBookingResponse.SSRSegmentList[3].LegKey.DepartureStation;
                        sellreqd.SegmentSSRRequests[3].ArrivalStation =
                        response.SSRAvailabilityForBookingResponse.SSRSegmentList[3].LegKey.ArrivalStation;
                        sellreqd.SegmentSSRRequests[3].STD =
                        response.SSRAvailabilityForBookingResponse.SSRSegmentList[3].LegKey.DepartureDate;
                        sellreqd.SegmentSSRRequests[3].FlightDesignator = new FlightDesignator();
                        sellreqd.SegmentSSRRequests[3].FlightDesignator.CarrierCode =
                        response.SSRAvailabilityForBookingResponse.SSRSegmentList[3].LegKey.CarrierCode;
                        sellreqd.SegmentSSRRequests[3].FlightDesignator.FlightNumber =
                        response.SSRAvailabilityForBookingResponse.SSRSegmentList[3].LegKey.FlightNumber;
                    }

                }

                cancelSSRs.CancelRequestData.CancelSSR.SSRRequest = sellreqd;
                CancelResponse cancelResponse = bookingAPI.Cancel(cancelSSRs);


                SellRequest sellSsrRequest = new SellRequest();
                SellRequestData sellreqds = new SellRequestData();
                sellreqds.SellBy = SellBy.SSR;
                sellreqds.SellSSR = new SellSSR();
                sellreqds.SellSSR.SSRRequest = new SSRRequest();
                SSRRequestCount = response.SSRAvailabilityForBookingResponse.SSRSegmentList.Length;
                sellreqds.SellSSR.SSRRequest.SegmentSSRRequests = new SegmentSSRRequest[SSRRequestCount];
                sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0] = new SegmentSSRRequest();

                sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs = new PaxSSR[dtList1.Length];
                for (int i = 0; i < sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs.Length; i++)
                {
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i] = new PaxSSR();
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].ActionStatusCode = "NN";
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRCode = dtList1[i]["SSRCode"].ToString();
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRNumber = 0;
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].DepartureStation = dtList1[i]["Origin"].ToString();
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].ArrivalStation = dtList1[i]["Destination"].ToString();
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList1[i]["PassengerID"]);
                    //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList1[i]["PassengerID"]);
                }
                count = 0;
                sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].DepartureStation =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.DepartureStation;
                sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].ArrivalStation =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.ArrivalStation;
                sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].STD =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.DepartureDate;
                sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator = new FlightDesignator();
                sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator.CarrierCode =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.CarrierCode;
                sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator.FlightNumber =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.FlightNumber;

                sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1] = new SegmentSSRRequest();

                if (group == "transitdepart" || group == "transit")
                {
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs = new PaxSSR[dtList1t.Length];
                    for (int i = 0; i < sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs.Length; i++)
                    {
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i] = new PaxSSR();
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].ActionStatusCode = "NN";
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].SSRCode = dtList1t[i]["SSRCode"].ToString();
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].SSRNumber = 0;
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].DepartureStation = dtList1t[i]["Origin"].ToString();
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].ArrivalStation = dtList1t[i]["Destination"].ToString();
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList1t[i]["PassengerID"]);
                        //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList1t[i]["PassengerID"]);
                    }
                }
                else if (group == "transitreturn")
                {
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs = new PaxSSR[dtList2.Length];
                    for (int i = 0; i < sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs.Length; i++)
                    {
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i] = new PaxSSR();
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].ActionStatusCode = "NN";
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].SSRCode = dtList2[i]["SSRCode"].ToString();
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].SSRNumber = 0;
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].DepartureStation = dtList2[i]["Origin"].ToString();
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].ArrivalStation = dtList2[i]["Destination"].ToString();
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList2[i]["PassengerID"]);
                        //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList1t[i]["PassengerID"]);
                    }
                }
                count = 0;
                sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].DepartureStation =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.DepartureStation;
                sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].ArrivalStation =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.ArrivalStation;
                sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].STD =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.DepartureDate;
                sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].FlightDesignator = new FlightDesignator();
                sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].FlightDesignator.CarrierCode =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.CarrierCode;
                sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[1].FlightDesignator.FlightNumber =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.FlightNumber;

                if (SSRRequestCount > 2)
                {
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[2] = new SegmentSSRRequest();

                    if (group == "transitdepart" || group == "transit")
                    {
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[2].PaxSSRs = new PaxSSR[dtList2.Length];
                        for (int i = 0; i < sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[2].PaxSSRs.Length; i++)
                        {
                            sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[2].PaxSSRs[i] = new PaxSSR();
                            sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[2].PaxSSRs[i].ActionStatusCode = "NN";
                            sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[2].PaxSSRs[i].SSRCode = dtList2[i]["SSRCode"].ToString();
                            sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[2].PaxSSRs[i].SSRNumber = 0;
                            sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[2].PaxSSRs[i].DepartureStation = dtList2[i]["Origin"].ToString();
                            sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[2].PaxSSRs[i].ArrivalStation = dtList2[i]["Destination"].ToString();
                            sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[2].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList2[i]["PassengerID"]);
                            //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[2].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList2[i]["PassengerID"]);
                        }
                    }
                    else if (group == "transitreturn")
                    {
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[2].PaxSSRs = new PaxSSR[dtList2t.Length];
                        for (int i = 0; i < sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[2].PaxSSRs.Length; i++)
                        {
                            sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[2].PaxSSRs[i] = new PaxSSR();
                            sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[2].PaxSSRs[i].ActionStatusCode = "NN";
                            sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[2].PaxSSRs[i].SSRCode = dtList2t[i]["SSRCode"].ToString();
                            sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[2].PaxSSRs[i].SSRNumber = 0;
                            sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[2].PaxSSRs[i].DepartureStation = dtList2t[i]["Origin"].ToString();
                            sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[2].PaxSSRs[i].ArrivalStation = dtList2t[i]["Destination"].ToString();
                            sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[2].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList2t[i]["PassengerID"]);
                            //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[2].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList2[i]["PassengerID"]);
                        }
                    }
                    count = 0;
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[2].DepartureStation =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[2].LegKey.DepartureStation;
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[2].ArrivalStation =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[2].LegKey.ArrivalStation;
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[2].STD =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[2].LegKey.DepartureDate;
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[2].FlightDesignator = new FlightDesignator();
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[2].FlightDesignator.CarrierCode =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[2].LegKey.CarrierCode;
                    sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[2].FlightDesignator.FlightNumber =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[2].LegKey.FlightNumber;

                    if (group == "transit")
                    {
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[3] = new SegmentSSRRequest();

                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[3].PaxSSRs = new PaxSSR[dtList2t.Length];
                        for (int i = 0; i < sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[3].PaxSSRs.Length; i++)
                        {
                            sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[3].PaxSSRs[i] = new PaxSSR();
                            sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[3].PaxSSRs[i].ActionStatusCode = "NN";
                            sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[3].PaxSSRs[i].SSRCode = dtList2t[i]["SSRCode"].ToString();
                            sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[3].PaxSSRs[i].SSRNumber = 0;
                            sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[3].PaxSSRs[i].DepartureStation = dtList2t[i]["Origin"].ToString();
                            sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[3].PaxSSRs[i].ArrivalStation = dtList2t[i]["Destination"].ToString();
                            sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[3].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList2t[i]["PassengerID"]);
                            //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[3].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList2t[i]["PassengerID"]);
                        }
                        count = 0;
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[3].DepartureStation =
                        response.SSRAvailabilityForBookingResponse.SSRSegmentList[3].LegKey.DepartureStation;
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[3].ArrivalStation =
                        response.SSRAvailabilityForBookingResponse.SSRSegmentList[3].LegKey.ArrivalStation;
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[3].STD =
                        response.SSRAvailabilityForBookingResponse.SSRSegmentList[3].LegKey.DepartureDate;
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[3].FlightDesignator = new FlightDesignator();
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[3].FlightDesignator.CarrierCode =
                        response.SSRAvailabilityForBookingResponse.SSRSegmentList[3].LegKey.CarrierCode;
                        sellreqds.SellSSR.SSRRequest.SegmentSSRRequests[3].FlightDesignator.FlightNumber =
                        response.SSRAvailabilityForBookingResponse.SSRSegmentList[3].LegKey.FlightNumber;
                    }

                }
                sellSsrRequest.Signature = SellSessionID;
                sellSsrRequest.ContractVersion = this.ContractVersion;
                sellSsrRequest.SellRequestData = sellreqds;
                SellResponse sellSsrResponse = null;
                sellSsrResponse = bookingAPI.Sell(sellSsrRequest);
                //string xml = GetXMLString(sellSsrRequest);
                //return SellSessionID;
                string msg = "";
                if (resp.Booking.BookingSum.BalanceDue > 0 || sellSsrResponse.BookingUpdateResponseData.Success.PNRAmount.BalanceDue == 0)
                {
                    using (profiler.Step("Navitaire:BookingCommit"))
                    {
                        BookingCommitChange(PNR, SellSessionID, ref msg, HttpContext.Current.Session["Currency"].ToString(), true, true);
                    }
                    HttpContext.Current.Session["Commit"] = true;
                }

                //BookingCommitChange(PNR, SellSessionID, ref msg, HttpContext.Current.Session["Currency"].ToString(), true, true);
                return SellSessionID;
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                log.Error(this, ex, "SellSSRTransit " + ex.Message);
                return "";
            }

        }

        public string UpdateSSRTransit(string PNR, GetSSRAvailabilityForBookingResponse response, DataRow[] dtList1, DataRow[] dtList2, DataRow[] dtList1t, DataRow[] dtList2t)
        {
            short count = 0;
            try
            {
                IBookingManager bookingAPI = new BookingManagerClient();
                UpdatePassengersRequest request = new UpdatePassengersRequest();
                UpdatePassengersRequestData requestData = new UpdatePassengersRequestData();
                ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking("");

                string SellSessionID = "";

                SellSessionID = apiBooking.AgentLogon();
                GetBookingResponse resp = GetBookingByPNR(PNR, SellSessionID);

                UpdateSSRRequest updatessrrequest = new UpdateSSRRequest();
                SellRequest sellSsrRequest = new SellRequest();
                SSRRequest sellreqd = new SSRRequest();
                sellreqd.SegmentSSRRequests = new SegmentSSRRequest[1];
                sellreqd.CurrencyCode = HttpContext.Current.Session["Currency"].ToString();
                int SSRRequestCount = response.SSRAvailabilityForBookingResponse.SSRSegmentList.Length;
                sellreqd.SegmentSSRRequests = new SegmentSSRRequest[SSRRequestCount];
                sellreqd.SegmentSSRRequests[0] = new SegmentSSRRequest();
                sellreqd.SegmentSSRRequests[0].PaxSSRs = new PaxSSR[dtList1.Length];
                for (int i = 0; i < sellreqd.SegmentSSRRequests[0].PaxSSRs.Length; i++)
                {
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i] = new PaxSSR();
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].ActionStatusCode = "NN";
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].SSRCode = dtList1[i]["SSRCode"].ToString();
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].SSRNumber = 0;
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].DepartureStation = dtList1[i]["Origin"].ToString();
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].ArrivalStation = dtList1[i]["Destination"].ToString();
                    if (i != 0)
                    {
                        if (Convert.ToInt16(dtList1[i]["PassengerNo"]) == Convert.ToInt16(dtList1[i - 1]["PassengerNo"]))
                        {
                            sellreqd.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = count;
                        }
                        else
                        {
                            count += 1;
                            sellreqd.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = count;
                        }
                    }
                    else
                    {
                        sellreqd.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = count;
                    }
                    //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList1[i]["PassengerID"]);
                }
                count = 0;
                sellreqd.SegmentSSRRequests[0].DepartureStation =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.DepartureStation;
                sellreqd.SegmentSSRRequests[0].ArrivalStation =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.ArrivalStation;
                sellreqd.SegmentSSRRequests[0].STD =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.DepartureDate;
                sellreqd.SegmentSSRRequests[0].FlightDesignator = new FlightDesignator();
                sellreqd.SegmentSSRRequests[0].FlightDesignator.CarrierCode =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.CarrierCode;
                sellreqd.SegmentSSRRequests[0].FlightDesignator.FlightNumber =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.FlightNumber;

                sellreqd.SegmentSSRRequests[1] = new SegmentSSRRequest();

                sellreqd.SegmentSSRRequests[1].PaxSSRs = new PaxSSR[dtList1t.Length];
                for (int i = 0; i < sellreqd.SegmentSSRRequests[1].PaxSSRs.Length; i++)
                {
                    sellreqd.SegmentSSRRequests[1].PaxSSRs[i] = new PaxSSR();
                    sellreqd.SegmentSSRRequests[1].PaxSSRs[i].ActionStatusCode = "NN";
                    sellreqd.SegmentSSRRequests[1].PaxSSRs[i].SSRCode = dtList1t[i]["SSRCode"].ToString();
                    sellreqd.SegmentSSRRequests[1].PaxSSRs[i].SSRNumber = 0;
                    sellreqd.SegmentSSRRequests[1].PaxSSRs[i].DepartureStation = dtList1t[i]["Origin"].ToString();
                    sellreqd.SegmentSSRRequests[1].PaxSSRs[i].ArrivalStation = dtList1t[i]["Destination"].ToString();
                    if (i != 0)
                    {
                        if (Convert.ToInt16(dtList1t[i]["PassengerNo"]) == Convert.ToInt16(dtList1t[i - 1]["PassengerNo"]))
                        {
                            sellreqd.SegmentSSRRequests[1].PaxSSRs[i].PassengerNumber = count;
                        }
                        else
                        {
                            count += 1;
                            sellreqd.SegmentSSRRequests[1].PaxSSRs[i].PassengerNumber = count;
                        }
                    }
                    else
                    {
                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i].PassengerNumber = count;
                    }
                    //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList1t[i]["PassengerID"]);
                }
                count = 0;
                sellreqd.SegmentSSRRequests[1].DepartureStation =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.DepartureStation;
                sellreqd.SegmentSSRRequests[1].ArrivalStation =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.ArrivalStation;
                sellreqd.SegmentSSRRequests[1].STD =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.DepartureDate;
                sellreqd.SegmentSSRRequests[1].FlightDesignator = new FlightDesignator();
                sellreqd.SegmentSSRRequests[1].FlightDesignator.CarrierCode =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.CarrierCode;
                sellreqd.SegmentSSRRequests[1].FlightDesignator.FlightNumber =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.FlightNumber;

                if (SSRRequestCount > 2)
                {
                    sellreqd.SegmentSSRRequests[2] = new SegmentSSRRequest();

                    sellreqd.SegmentSSRRequests[2].PaxSSRs = new PaxSSR[dtList2.Length];
                    for (int i = 0; i < sellreqd.SegmentSSRRequests[2].PaxSSRs.Length; i++)
                    {
                        sellreqd.SegmentSSRRequests[2].PaxSSRs[i] = new PaxSSR();
                        sellreqd.SegmentSSRRequests[2].PaxSSRs[i].ActionStatusCode = "NN";
                        sellreqd.SegmentSSRRequests[2].PaxSSRs[i].SSRCode = dtList2[i]["SSRCode"].ToString();
                        sellreqd.SegmentSSRRequests[2].PaxSSRs[i].SSRNumber = 0;
                        sellreqd.SegmentSSRRequests[2].PaxSSRs[i].DepartureStation = dtList2[i]["Origin"].ToString();
                        sellreqd.SegmentSSRRequests[2].PaxSSRs[i].ArrivalStation = dtList2[i]["Destination"].ToString();
                        if (i != 0)
                        {
                            if (Convert.ToInt16(dtList2[i]["PassengerNo"]) == Convert.ToInt16(dtList2[i - 1]["PassengerNo"]))
                            {
                                sellreqd.SegmentSSRRequests[2].PaxSSRs[i].PassengerNumber = count;
                            }
                            else
                            {
                                count += 1;
                                sellreqd.SegmentSSRRequests[2].PaxSSRs[i].PassengerNumber = count;
                            }
                        }
                        else
                        {
                            sellreqd.SegmentSSRRequests[2].PaxSSRs[i].PassengerNumber = count;
                        }
                        //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[2].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList2[i]["PassengerID"]);
                    }
                    count = 0;
                    sellreqd.SegmentSSRRequests[2].DepartureStation =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[2].LegKey.DepartureStation;
                    sellreqd.SegmentSSRRequests[2].ArrivalStation =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[2].LegKey.ArrivalStation;
                    sellreqd.SegmentSSRRequests[2].STD =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[2].LegKey.DepartureDate;
                    sellreqd.SegmentSSRRequests[2].FlightDesignator = new FlightDesignator();
                    sellreqd.SegmentSSRRequests[2].FlightDesignator.CarrierCode =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[2].LegKey.CarrierCode;
                    sellreqd.SegmentSSRRequests[2].FlightDesignator.FlightNumber =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[2].LegKey.FlightNumber;

                    sellreqd.SegmentSSRRequests[3] = new SegmentSSRRequest();

                    sellreqd.SegmentSSRRequests[3].PaxSSRs = new PaxSSR[dtList2t.Length];
                    for (int i = 0; i < sellreqd.SegmentSSRRequests[3].PaxSSRs.Length; i++)
                    {
                        sellreqd.SegmentSSRRequests[3].PaxSSRs[i] = new PaxSSR();
                        sellreqd.SegmentSSRRequests[3].PaxSSRs[i].ActionStatusCode = "NN";
                        sellreqd.SegmentSSRRequests[3].PaxSSRs[i].SSRCode = dtList2t[i]["SSRCode"].ToString();
                        sellreqd.SegmentSSRRequests[3].PaxSSRs[i].SSRNumber = 0;
                        sellreqd.SegmentSSRRequests[3].PaxSSRs[i].DepartureStation = dtList2t[i]["Origin"].ToString();
                        sellreqd.SegmentSSRRequests[3].PaxSSRs[i].ArrivalStation = dtList2t[i]["Destination"].ToString();
                        if (i != 0)
                        {
                            if (Convert.ToInt16(dtList2t[i]["PassengerNo"]) == Convert.ToInt16(dtList2t[i - 1]["PassengerNo"]))
                            {
                                sellreqd.SegmentSSRRequests[3].PaxSSRs[i].PassengerNumber = count;
                            }
                            else
                            {
                                count += 1;
                                sellreqd.SegmentSSRRequests[3].PaxSSRs[i].PassengerNumber = count;
                            }
                        }
                        else
                        {
                            sellreqd.SegmentSSRRequests[3].PaxSSRs[i].PassengerNumber = count;
                        }
                        //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[3].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList2t[i]["PassengerID"]);
                    }
                    count = 0;
                    sellreqd.SegmentSSRRequests[3].DepartureStation =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[3].LegKey.DepartureStation;
                    sellreqd.SegmentSSRRequests[3].ArrivalStation =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[3].LegKey.ArrivalStation;
                    sellreqd.SegmentSSRRequests[3].STD =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[3].LegKey.DepartureDate;
                    sellreqd.SegmentSSRRequests[3].FlightDesignator = new FlightDesignator();
                    sellreqd.SegmentSSRRequests[3].FlightDesignator.CarrierCode =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[3].LegKey.CarrierCode;
                    sellreqd.SegmentSSRRequests[3].FlightDesignator.FlightNumber =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[3].LegKey.FlightNumber;

                }
                updatessrrequest.Signature = SellSessionID;
                updatessrrequest.ContractVersion = this.ContractVersion;
                updatessrrequest.SSRRequest = sellreqd;
                UpdateSSRResponse sellSsrResponse = null;
                sellSsrResponse = bookingAPI.UpdateSSR(updatessrrequest);
                //string xml = GetXMLString(sellSsrRequest);
                return SellSessionID;
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                log.Error(this, ex, "UpdateSSRTransit: " + ex.Message);
                return "";
            }

        }

        public string UpdateSSR(string PNR, GetSSRAvailabilityForBookingResponse response, DataRow[] dtList1, DataRow[] dtList2)
        {
            short count = 0;
            try
            {
                IBookingManager bookingAPI = new BookingManagerClient();
                UpdatePassengersRequest request = new UpdatePassengersRequest();
                UpdatePassengersRequestData requestData = new UpdatePassengersRequestData();
                ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking("");
                string SellSessionID = "";

                SellSessionID = apiBooking.AgentLogon();
                GetBookingResponse resp = GetBookingByPNR(PNR, SellSessionID);
                UpdateSSRRequest updatessrrequest = new UpdateSSRRequest();
                SellRequest sellSsrRequest = new SellRequest();
                SSRRequest sellreqd = new SSRRequest();
                sellreqd.SegmentSSRRequests = new SegmentSSRRequest[1];
                sellreqd.CurrencyCode = HttpContext.Current.Session["Currency"].ToString();
                sellreqd.CancelFirstSSR = false;
                int SSRRequestCount = response.SSRAvailabilityForBookingResponse.SSRSegmentList.Length;
                sellreqd.SegmentSSRRequests = new SegmentSSRRequest[SSRRequestCount];
                sellreqd.SegmentSSRRequests[0] = new SegmentSSRRequest();
                sellreqd.SegmentSSRRequests[0].PaxSSRs = new PaxSSR[dtList1.Length];
                for (int i = 0; i < sellreqd.SegmentSSRRequests[0].PaxSSRs.Length; i++)
                {

                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i] = new PaxSSR();
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].ActionStatusCode = "NN";
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].State = BookingManager.MessageState.New;
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].SSRCode = dtList1[i]["SSRCode"].ToString();
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].SSRNumber = 0;
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].DepartureStation = dtList1[i]["Origin"].ToString();
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].ArrivalStation = dtList1[i]["Destination"].ToString();
                    if (i != 0)
                    {
                        if (Convert.ToInt16(dtList1[i]["PassengerID"]) == Convert.ToInt16(dtList1[i - 1]["PassengerID"]))
                        {
                            sellreqd.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = count;
                        }
                        else
                        {
                            count += 1;
                            sellreqd.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = count;
                        }
                    }
                    else
                    {
                        sellreqd.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = count;
                    }
                    //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList1[i]["PassengerID"]);
                }
                count = 0;
                sellreqd.SegmentSSRRequests[0].DepartureStation =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.DepartureStation;
                sellreqd.SegmentSSRRequests[0].ArrivalStation =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.ArrivalStation;
                sellreqd.SegmentSSRRequests[0].STD =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.DepartureDate;
                sellreqd.SegmentSSRRequests[0].FlightDesignator = new FlightDesignator();
                sellreqd.SegmentSSRRequests[0].FlightDesignator.CarrierCode =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.CarrierCode;
                sellreqd.SegmentSSRRequests[0].FlightDesignator.FlightNumber =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.FlightNumber;



                if (SSRRequestCount > 1)
                {
                    sellreqd.SegmentSSRRequests[1] = new SegmentSSRRequest();

                    sellreqd.SegmentSSRRequests[1].PaxSSRs = new PaxSSR[dtList2.Length];
                    for (int i = 0; i < sellreqd.SegmentSSRRequests[1].PaxSSRs.Length; i++)
                    {
                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i] = new PaxSSR();
                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i].ActionStatusCode = "NN";
                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i].SSRCode = dtList2[i]["SSRCode"].ToString();
                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i].SSRNumber = 0;
                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i].DepartureStation = dtList2[i]["Origin"].ToString();
                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i].ArrivalStation = dtList2[i]["Destination"].ToString();
                        if (i != 0)
                        {
                            if (Convert.ToInt16(dtList2[i]["PassengerID"]) == Convert.ToInt16(dtList2[i - 1]["PassengerID"]))
                            {
                                sellreqd.SegmentSSRRequests[1].PaxSSRs[i].PassengerNumber = count;
                            }
                            else
                            {
                                count += 1;
                                sellreqd.SegmentSSRRequests[1].PaxSSRs[i].PassengerNumber = count;
                            }
                        }
                        else
                        {
                            sellreqd.SegmentSSRRequests[1].PaxSSRs[i].PassengerNumber = count;
                        }
                        //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList2[i]["PassengerID"]);
                    }
                    count = 0;
                    sellreqd.SegmentSSRRequests[1].DepartureStation =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.DepartureStation;
                    sellreqd.SegmentSSRRequests[1].ArrivalStation =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.ArrivalStation;
                    sellreqd.SegmentSSRRequests[1].STD =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.DepartureDate;
                    sellreqd.SegmentSSRRequests[1].FlightDesignator = new FlightDesignator();
                    sellreqd.SegmentSSRRequests[1].FlightDesignator.CarrierCode =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.CarrierCode;
                    sellreqd.SegmentSSRRequests[1].FlightDesignator.FlightNumber =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.FlightNumber;

                }
                updatessrrequest.Signature = SellSessionID;
                updatessrrequest.ContractVersion = this.ContractVersion;
                updatessrrequest.SSRRequest = sellreqd;
                UpdateSSRResponse sellSsrResponse = null;
                //string xml = GetXMLString(updatessrrequest);
                sellSsrResponse = bookingAPI.UpdateSSR(updatessrrequest);
                return SellSessionID;
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                log.Error(this, ex, "UpdateSSR: " + ex.Message);
                return null;
            }

        }

        public ResellSSRResponse ReSellSSR(string SellSessionID)
        {
            IBookingManager bookingAPI = new BookingManagerClient();
            ResellSSRRequest resellSSRRequest = new ResellSSRRequest();
            resellSSRRequest.ContractVersion = this.ContractVersion;
            resellSSRRequest.Signature = SellSessionID;
            resellSSRRequest.ResellSSR = new ResellSSR();
            resellSSRRequest.ResellSSR.JourneyNumber = 1;
            resellSSRRequest.ResellSSR.ResellSeatSSRs = false;
            resellSSRRequest.ResellSSR.ResellSSRs = true;
            resellSSRRequest.ResellSSR.WaiveSeatFee = false;
            ResellSSRResponse resellSSRResponse = new ResellSSRResponse();
            resellSSRResponse = bookingAPI.ResellSSR(resellSSRRequest);
            return resellSSRResponse;
        }

        public CancelResponse CancelSSR(string SellSessionID, GetSSRAvailabilityForBookingResponse response, string currency, DataRow[] dtList1, DataRow[] dtList2)
        {
            short count = 0;
            try
            {
                ////Create an instance of BookingManagerClient
                IBookingManager bookingAPI = new BookingManagerClient();
                // This method requires a booking be in state
                // For this example the variable Booking booking contains a booking 

                // that contains passengers and journeys with segments
                CancelRequest cancelSSRs = new CancelRequest();
                cancelSSRs.Signature = SellSessionID;
                cancelSSRs.ContractVersion = this.ContractVersion;
                cancelSSRs.CancelRequestData = new CancelRequestData();
                cancelSSRs.CancelRequestData.CancelBy = CancelBy.SSR;
                cancelSSRs.CancelRequestData.CancelSSR = new CancelSSR();
                SSRRequest sellreqd = new SSRRequest();
                sellreqd.CurrencyCode = currency;
                sellreqd.CancelFirstSSR = true;
                int SSRRequestCount = response.SSRAvailabilityForBookingResponse.SSRSegmentList.Length;
                sellreqd.SegmentSSRRequests = new SegmentSSRRequest[SSRRequestCount];
                sellreqd.SegmentSSRRequests[0] = new SegmentSSRRequest();
                sellreqd.SegmentSSRRequests[0].PaxSSRs = new PaxSSR[dtList1.Length];
                for (int i = 0; i < sellreqd.SegmentSSRRequests[0].PaxSSRs.Length; i++)
                {

                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i] = new PaxSSR();
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].ActionStatusCode = "NN";
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].SSRCode = dtList1[i]["SSRCode"].ToString();
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].SSRNumber = 0;
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].DepartureStation = dtList1[i]["Origin"].ToString();
                    sellreqd.SegmentSSRRequests[0].PaxSSRs[i].ArrivalStation = dtList1[i]["Destination"].ToString();
                    if (i != 0)
                    {
                        if (Convert.ToInt16(dtList1[i]["PassengerNo"]) == Convert.ToInt16(dtList1[i - 1]["PassengerNo"]))
                        {
                            sellreqd.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = count;
                        }
                        else
                        {
                            count += 1;
                            sellreqd.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = count;
                        }
                    }
                    else
                    {
                        sellreqd.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = count;
                    }
                    //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList1[i]["PassengerID"]);
                }
                count = 0;
                sellreqd.SegmentSSRRequests[0].DepartureStation =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.DepartureStation;
                sellreqd.SegmentSSRRequests[0].ArrivalStation =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.ArrivalStation;
                sellreqd.SegmentSSRRequests[0].STD =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.DepartureDate;
                sellreqd.SegmentSSRRequests[0].FlightDesignator = new FlightDesignator();
                sellreqd.SegmentSSRRequests[0].FlightDesignator.CarrierCode =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.CarrierCode;
                sellreqd.SegmentSSRRequests[0].FlightDesignator.FlightNumber =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.FlightNumber;



                if (SSRRequestCount > 1)
                {
                    sellreqd.SegmentSSRRequests[1] = new SegmentSSRRequest();

                    sellreqd.SegmentSSRRequests[1].PaxSSRs = new PaxSSR[dtList2.Length];
                    for (int i = 0; i < sellreqd.SegmentSSRRequests[1].PaxSSRs.Length; i++)
                    {
                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i] = new PaxSSR();
                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i].ActionStatusCode = "NN";
                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i].SSRCode = dtList2[i]["SSRCode"].ToString();
                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i].SSRNumber = 0;
                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i].DepartureStation = dtList2[i]["Origin"].ToString();
                        sellreqd.SegmentSSRRequests[1].PaxSSRs[i].ArrivalStation = dtList2[i]["Destination"].ToString();
                        if (i != 0)
                        {
                            if (Convert.ToInt16(dtList2[i]["PassengerNo"]) == Convert.ToInt16(dtList2[i - 1]["PassengerNo"]))
                            {
                                sellreqd.SegmentSSRRequests[1].PaxSSRs[i].PassengerNumber = count;
                            }
                            else
                            {
                                count += 1;
                                sellreqd.SegmentSSRRequests[1].PaxSSRs[i].PassengerNumber = count;
                            }
                        }
                        else
                        {
                            sellreqd.SegmentSSRRequests[1].PaxSSRs[i].PassengerNumber = count;
                        }
                        //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList2[i]["PassengerID"]);
                    }
                    count = 0;
                    sellreqd.SegmentSSRRequests[1].DepartureStation =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.DepartureStation;
                    sellreqd.SegmentSSRRequests[1].ArrivalStation =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.ArrivalStation;
                    sellreqd.SegmentSSRRequests[1].STD =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.DepartureDate;
                    sellreqd.SegmentSSRRequests[1].FlightDesignator = new FlightDesignator();
                    sellreqd.SegmentSSRRequests[1].FlightDesignator.CarrierCode =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.CarrierCode;
                    sellreqd.SegmentSSRRequests[1].FlightDesignator.FlightNumber =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.FlightNumber;

                }
                cancelSSRs.CancelRequestData.CancelSSR.SSRRequest = sellreqd;
                CancelResponse cancelResponse = bookingAPI.Cancel(cancelSSRs);
                return cancelResponse;

            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                log.Error(this, ex, "CancelSSR: " + ex.Message);
                return null;
            }

        }

        public CancelResponse CancelSSRTransit(string SellSessionID, GetSSRAvailabilityForBookingResponse response, string currency, DataRow[] dtList1, DataRow[] dtList2, DataRow[] dtList1t, DataRow[] dtList2t)
        {
            short count = 0;
            try
            { ////Create an instance of BookingManagerClient
                IBookingManager bookingAPI = new BookingManagerClient();
                // This method requires a booking be in state
                // For this example the variable Booking booking contains a booking 

                // that contains passengers and journeys with segments
                CancelRequest cancelSSRs = new CancelRequest();
                cancelSSRs.Signature = SellSessionID;
                cancelSSRs.ContractVersion = this.ContractVersion;
                cancelSSRs.CancelRequestData = new CancelRequestData();
                cancelSSRs.CancelRequestData.CancelBy = CancelBy.SSR;
                cancelSSRs.CancelRequestData.CancelSSR = new CancelSSR();
                SSRRequest sellreqd = new SSRRequest();
                sellreqd.CurrencyCode = currency;
                sellreqd.CancelFirstSSR = true;
                int index = 0;
                int SSRRequestCount = response.SSRAvailabilityForBookingResponse.SSRSegmentList.Length;
                sellreqd.SegmentSSRRequests = new SegmentSSRRequest[SSRRequestCount];
                sellreqd.SegmentSSRRequests[index] = new SegmentSSRRequest();
                sellreqd.SegmentSSRRequests[index].PaxSSRs = new PaxSSR[dtList1.Length];
                for (int i = 0; i < sellreqd.SegmentSSRRequests[index].PaxSSRs.Length; i++)
                {

                    sellreqd.SegmentSSRRequests[index].PaxSSRs[i] = new PaxSSR();
                    sellreqd.SegmentSSRRequests[index].PaxSSRs[i].ActionStatusCode = "NN";
                    sellreqd.SegmentSSRRequests[index].PaxSSRs[i].SSRCode = dtList1[i]["SSRCode"].ToString();
                    sellreqd.SegmentSSRRequests[index].PaxSSRs[i].SSRNumber = 0;
                    sellreqd.SegmentSSRRequests[index].PaxSSRs[i].DepartureStation = dtList1[i]["Origin"].ToString();
                    sellreqd.SegmentSSRRequests[index].PaxSSRs[i].ArrivalStation = dtList1[i]["Destination"].ToString();
                    if (i != 0)
                    {
                        if (Convert.ToInt16(dtList1[i]["PassengerNo"]) == Convert.ToInt16(dtList1[i - 1]["PassengerNo"]))
                        {
                            sellreqd.SegmentSSRRequests[index].PaxSSRs[i].PassengerNumber = count;
                        }
                        else
                        {
                            count += 1;
                            sellreqd.SegmentSSRRequests[index].PaxSSRs[i].PassengerNumber = count;
                        }
                    }
                    else
                    {
                        sellreqd.SegmentSSRRequests[index].PaxSSRs[i].PassengerNumber = count;
                    }
                    //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList1[i]["PassengerID"]);
                }
                count = 0;
                sellreqd.SegmentSSRRequests[index].DepartureStation =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.DepartureStation;
                sellreqd.SegmentSSRRequests[index].ArrivalStation =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.ArrivalStation;
                sellreqd.SegmentSSRRequests[index].STD =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.DepartureDate;
                sellreqd.SegmentSSRRequests[index].FlightDesignator = new FlightDesignator();
                sellreqd.SegmentSSRRequests[index].FlightDesignator.CarrierCode =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.CarrierCode;
                sellreqd.SegmentSSRRequests[index].FlightDesignator.FlightNumber =
                response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.FlightNumber;

                if (SSRRequestCount > (index + 1) && dtList1t != null && dtList1t.Length > 0)
                {
                    index += 1;
                    sellreqd.SegmentSSRRequests[index] = new SegmentSSRRequest();

                    sellreqd.SegmentSSRRequests[index].PaxSSRs = new PaxSSR[dtList1t.Length];
                    for (int i = 0; i < sellreqd.SegmentSSRRequests[index].PaxSSRs.Length; i++)
                    {
                        sellreqd.SegmentSSRRequests[index].PaxSSRs[i] = new PaxSSR();
                        sellreqd.SegmentSSRRequests[index].PaxSSRs[i].ActionStatusCode = "NN";
                        sellreqd.SegmentSSRRequests[index].PaxSSRs[i].SSRCode = dtList1t[i]["SSRCode"].ToString();
                        sellreqd.SegmentSSRRequests[index].PaxSSRs[i].SSRNumber = 0;
                        sellreqd.SegmentSSRRequests[index].PaxSSRs[i].DepartureStation = dtList1t[i]["Origin"].ToString();
                        sellreqd.SegmentSSRRequests[index].PaxSSRs[i].ArrivalStation = dtList1t[i]["Destination"].ToString();
                        if (i != 0)
                        {
                            if (Convert.ToInt16(dtList1t[i]["PassengerNo"]) == Convert.ToInt16(dtList1t[i - 1]["PassengerNo"]))
                            {
                                sellreqd.SegmentSSRRequests[index].PaxSSRs[i].PassengerNumber = count;
                            }
                            else
                            {
                                count += 1;
                                sellreqd.SegmentSSRRequests[index].PaxSSRs[i].PassengerNumber = count;
                            }
                        }
                        else
                        {
                            sellreqd.SegmentSSRRequests[index].PaxSSRs[i].PassengerNumber = count;
                        }
                        //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList1t[i]["PassengerID"]);
                    }
                    count = 0;
                    sellreqd.SegmentSSRRequests[index].DepartureStation =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.DepartureStation;
                    sellreqd.SegmentSSRRequests[index].ArrivalStation =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.ArrivalStation;
                    sellreqd.SegmentSSRRequests[index].STD =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.DepartureDate;
                    sellreqd.SegmentSSRRequests[index].FlightDesignator = new FlightDesignator();
                    sellreqd.SegmentSSRRequests[index].FlightDesignator.CarrierCode =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.CarrierCode;
                    sellreqd.SegmentSSRRequests[index].FlightDesignator.FlightNumber =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.FlightNumber;
                }
                if (SSRRequestCount > (index + 1) && dtList2 != null && dtList2.Length > 0)
                {
                    index += 1;
                    sellreqd.SegmentSSRRequests[index] = new SegmentSSRRequest();

                    sellreqd.SegmentSSRRequests[index].PaxSSRs = new PaxSSR[dtList2.Length];
                    for (int i = 0; i < sellreqd.SegmentSSRRequests[index].PaxSSRs.Length; i++)
                    {
                        sellreqd.SegmentSSRRequests[index].PaxSSRs[i] = new PaxSSR();
                        sellreqd.SegmentSSRRequests[index].PaxSSRs[i].ActionStatusCode = "NN";
                        sellreqd.SegmentSSRRequests[index].PaxSSRs[i].SSRCode = dtList2[i]["SSRCode"].ToString();
                        sellreqd.SegmentSSRRequests[index].PaxSSRs[i].SSRNumber = 0;
                        sellreqd.SegmentSSRRequests[index].PaxSSRs[i].DepartureStation = dtList2[i]["Origin"].ToString();
                        sellreqd.SegmentSSRRequests[index].PaxSSRs[i].ArrivalStation = dtList2[i]["Destination"].ToString();
                        if (i != 0)
                        {
                            if (Convert.ToInt16(dtList2[i]["PassengerNo"]) == Convert.ToInt16(dtList2[i - 1]["PassengerNo"]))
                            {
                                sellreqd.SegmentSSRRequests[index].PaxSSRs[i].PassengerNumber = count;
                            }
                            else
                            {
                                count += 1;
                                sellreqd.SegmentSSRRequests[index].PaxSSRs[i].PassengerNumber = count;
                            }
                        }
                        else
                        {
                            sellreqd.SegmentSSRRequests[index].PaxSSRs[i].PassengerNumber = count;
                        }
                        //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[2].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList2[i]["PassengerID"]);
                    }
                    count = 0;
                    sellreqd.SegmentSSRRequests[index].DepartureStation =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.DepartureStation;
                    sellreqd.SegmentSSRRequests[index].ArrivalStation =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.ArrivalStation;
                    sellreqd.SegmentSSRRequests[index].STD =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.DepartureDate;
                    sellreqd.SegmentSSRRequests[index].FlightDesignator = new FlightDesignator();
                    sellreqd.SegmentSSRRequests[index].FlightDesignator.CarrierCode =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.CarrierCode;
                    sellreqd.SegmentSSRRequests[index].FlightDesignator.FlightNumber =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.FlightNumber;
                }

                if (SSRRequestCount > (index + 1) && dtList2t != null && dtList2t.Length > 0)
                {
                    index += 1;
                    sellreqd.SegmentSSRRequests[index] = new SegmentSSRRequest();

                    sellreqd.SegmentSSRRequests[index].PaxSSRs = new PaxSSR[dtList2t.Length];
                    for (int i = 0; i < sellreqd.SegmentSSRRequests[index].PaxSSRs.Length; i++)
                    {
                        sellreqd.SegmentSSRRequests[index].PaxSSRs[i] = new PaxSSR();
                        sellreqd.SegmentSSRRequests[index].PaxSSRs[i].ActionStatusCode = "NN";
                        sellreqd.SegmentSSRRequests[index].PaxSSRs[i].SSRCode = dtList2t[i]["SSRCode"].ToString();
                        sellreqd.SegmentSSRRequests[index].PaxSSRs[i].SSRNumber = 0;
                        sellreqd.SegmentSSRRequests[index].PaxSSRs[i].DepartureStation = dtList2t[i]["Origin"].ToString();
                        sellreqd.SegmentSSRRequests[index].PaxSSRs[i].ArrivalStation = dtList2t[i]["Destination"].ToString();
                        if (i != 0)
                        {
                            if (Convert.ToInt16(dtList2t[i]["PassengerNo"]) == Convert.ToInt16(dtList2t[i - 1]["PassengerNo"]))
                            {
                                sellreqd.SegmentSSRRequests[index].PaxSSRs[i].PassengerNumber = count;
                            }
                            else
                            {
                                count += 1;
                                sellreqd.SegmentSSRRequests[index].PaxSSRs[i].PassengerNumber = count;
                            }
                        }
                        else
                        {
                            sellreqd.SegmentSSRRequests[index].PaxSSRs[i].PassengerNumber = count;
                        }
                        //sellreqd.SellSSR.SSRRequest.SegmentSSRRequests[3].PaxSSRs[i].PassengerNumber = Convert.ToInt16(dtList2t[i]["PassengerID"]);
                    }
                    count = 0;
                    sellreqd.SegmentSSRRequests[index].DepartureStation =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.DepartureStation;
                    sellreqd.SegmentSSRRequests[index].ArrivalStation =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.ArrivalStation;
                    sellreqd.SegmentSSRRequests[index].STD =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.DepartureDate;
                    sellreqd.SegmentSSRRequests[index].FlightDesignator = new FlightDesignator();
                    sellreqd.SegmentSSRRequests[index].FlightDesignator.CarrierCode =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.CarrierCode;
                    sellreqd.SegmentSSRRequests[index].FlightDesignator.FlightNumber =
                    response.SSRAvailabilityForBookingResponse.SSRSegmentList[index].LegKey.FlightNumber;

                }
                cancelSSRs.CancelRequestData.CancelSSR.SSRRequest = sellreqd;
                CancelResponse cancelResponse = bookingAPI.Cancel(cancelSSRs);
                return cancelResponse;
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                log.Error(this, ex, "CancelSSRTransit: " + ex.Message);
                return null;
            }

        }
        public void CancelSSR(string RecordLocator)
        {
            IBookingManager bookingAPI = new BookingManagerClient();
            GetBookingRequest bookingRequest = new GetBookingRequest();
            try
            {
                //GetBookingResponse booking = GetBookingByPNR(RecordLocator);
                CancelRequest cancelSSRs = new CancelRequest();
                cancelSSRs.Signature = this.Signature;

                cancelSSRs.ContractVersion = this.ContractVersion;
                cancelSSRs.CancelRequestData = new CancelRequestData();
                cancelSSRs.CancelRequestData.CancelBy = CancelBy.SSR;
                cancelSSRs.CancelRequestData.CancelSSR = new CancelSSR();
                SSRRequest ssrRequest = new SSRRequest();
                ssrRequest.CurrencyCode = "USD";
                //ssrRequest.SegmentSSRRequests = new System.ComponentModel.BindingList<SegmentSSRRequest>();
                //ssrRequest.SegmentSSRRequests = new BindingList<SegmentSSRRequest>();
                //ssrRequest.SegmentSSRRequests.Add(new SegmentSSRRequest());
                //ssrRequest.SegmentSSRRequests. = new BindingList<PaxSSR>();
                //ssrRequest.SegmentSSRRequests[0].PaxSSRs.Add(new PaxSSR());
                ssrRequest.SegmentSSRRequests[0].PaxSSRs[0].ActionStatusCode = "NN";
                ssrRequest.SegmentSSRRequests[0].PaxSSRs[0].SSRCode = "INFT";
                ssrRequest.SegmentSSRRequests[0].PaxSSRs[0].PassengerNumber = 0;
                ssrRequest.SegmentSSRRequests[0].PaxSSRs[0].SSRNumber = 1;
                ssrRequest.SegmentSSRRequests[0].PaxSSRs[0].DepartureStation = "SLC";
                ssrRequest.SegmentSSRRequests[0].PaxSSRs[0].ArrivalStation = "DEN";
                ssrRequest.SegmentSSRRequests[0].FlightDesignator = new FlightDesignator();
                ssrRequest.SegmentSSRRequests[0].FlightDesignator.CarrierCode = "1L";
                ssrRequest.SegmentSSRRequests[0].STD = DateTime.Parse("19Jun08 1:00");
                ssrRequest.SegmentSSRRequests[0].FlightDesignator.FlightNumber = "1235";
                ssrRequest.SegmentSSRRequests[0].FlightDesignator.OpSuffix = " ";
                ssrRequest.SegmentSSRRequests[0].DepartureStation = "SLC";
                ssrRequest.SegmentSSRRequests[0].ArrivalStation = "DEN";
                cancelSSRs.CancelRequestData.CancelSSR.SSRRequest = ssrRequest;
                CancelResponse cancelResponse = bookingAPI.Cancel(cancelSSRs);
                if (cancelResponse.BookingUpdateResponseData !=
                null && cancelResponse.BookingUpdateResponseData.Success != null)
                {
                    decimal amount = cancelResponse.BookingUpdateResponseData.Success.PNRAmount.TotalCost;
                    string recloc = cancelResponse.BookingUpdateResponseData.Success.RecordLocator;
                    Console.WriteLine("{0} SSR passenger SSRs canceled", recloc);
                    Console.WriteLine("PNR Amount {0:C}", amount);
                }
                else
                {
                    Console.WriteLine("Cancel SSR failed");
                }
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
        }


        public GetBookingResponse GetBookingByPNR(string PNR, string SessionID)
        {
            IBookingManager bookingAPI = new BookingManagerClient();
            GetBookingRequest request = new GetBookingRequest();
            //request.ContractVersion = 3413;
            request.ContractVersion = this.ContractVersion;
            request.Signature = SessionID;
            request.GetBookingReqData = new GetBookingRequestData();
            request.GetBookingReqData.GetBookingBy = GetBookingBy.RecordLocator;
            request.GetBookingReqData.GetByRecordLocator = new GetByRecordLocator();
            request.GetBookingReqData.GetByRecordLocator.RecordLocator = PNR;
            GetBookingResponse response = bookingAPI.GetBooking(request);
            return response;
        }

        //public string DivideBooking(List<ABS.PassengerModel> PassengerModels, ABS.BookingModel BookingModels)
        //{
        //    IBookingManager bookingAPI = new BookingManagerClient();
        //    DivideRequest divideRequest = new DivideRequest();
        //    SessionContext sesscon = new SessionContext();

        //    try
        //    {

        //        divideRequest.DivideReqData = new DivideRequestData();
        //        divideRequest.DivideReqData.DivideAction = DivideAction.Divide;
        //        divideRequest.DivideReqData.SourceRecordLocator = BookingModels.RecordLocator.ToString();
        //        divideRequest.DivideReqData.AutoDividePayments = false;
        //        divideRequest.DivideReqData.PassengerNumbers = new short[1];
        //        int i = 0;
        //        foreach (ABS.PassengerModel rowpassenger in PassengerModels)
        //        {
        //            divideRequest.DivideReqData.PassengerNumbers[i] = (short)rowpassenger.PassengerNumber;
        //            i++;
        //        }
        //        divideRequest.DivideReqData.OverrideRestrictions = false;

        //        divideRequest.DivideReqData.ReceivedBy = BookingModels.ReceivedBy;
        //        divideRequest.DivideReqData.AddComments = true;

        //        divideRequest.Signature = sesscon.GetSessionID();
        //        DivideResponse divideResponse = bookingAPI.Divide(divideRequest);
        //        if (divideResponse != null)
        //        {
        //            return divideResponse.BookingUpdateResponseData.Success.RecordLocator;
        //        }
        //        else
        //        {
        //            return string.Empty;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(this,ex);
        //        return string.Empty;
        //    }

        //}

        public string DivideBooking(string RecordLocator, ArrayList PassengerNumber, string ReceivedBy)
        {
            IBookingManager bookingAPI = new BookingManagerClient();
            DivideRequest divideRequest = new DivideRequest();
            try
            {

                divideRequest.DivideReqData = new DivideRequestData();
                divideRequest.DivideReqData.DivideAction = DivideAction.Divide;
                divideRequest.DivideReqData.SourceRecordLocator = RecordLocator;
                divideRequest.DivideReqData.AutoDividePayments = false;
                divideRequest.DivideReqData.PassengerNumbers = new short[1];
                int i = 0;
                foreach (short rowpassenger in PassengerNumber)
                {
                    divideRequest.DivideReqData.PassengerNumbers[i] = (short)rowpassenger;
                    i++;
                }
                divideRequest.DivideReqData.OverrideRestrictions = false;

                divideRequest.DivideReqData.ReceivedBy = ReceivedBy;
                divideRequest.DivideReqData.AddComments = true;

                divideRequest.Signature = this.Signature;
                DivideResponse divideResponse = bookingAPI.Divide(divideRequest);
                if (divideResponse != null)
                {
                    return divideResponse.BookingUpdateResponseData.Success.RecordLocator;
                }
                else
                {
                    return string.Empty;
                }
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                return string.Empty;
            }

        }

        public GetAvailabilityResponse GetAvailability(string Arrival, DateTime DepartDate, string Currency, string Departure, int PaxNum, ref string SessionID, string PromoCode)
        {
            try
            {
                SessionID = this.Signature;
                IBookingManager bookingAPI = new BookingManagerClient();
                GetAvailabilityRequest request = new GetAvailabilityRequest();
                request.Signature = SessionID;// SessionManager._signature;
                request.ContractVersion = this.ContractVersion;
                request.TripAvailabilityRequest = new TripAvailabilityRequest();
                request.TripAvailabilityRequest.AvailabilityRequests = new AvailabilityRequest[1];
                AvailabilityRequest availabilityRequest = new AvailabilityRequest();
                availabilityRequest.ArrivalStation = Arrival.Trim();// "KUL";
                availabilityRequest.AvailabilityFilter = AvailabilityFilter.Default;
                availabilityRequest.AvailabilityType = AvailabilityType.Default;
                availabilityRequest.BeginDate = DepartDate;// DateTime.Parse("2012-04-01");
                availabilityRequest.CurrencyCode = Currency.Trim();// "CNY";
                availabilityRequest.DepartureStation = Departure.Trim();// "SZX";
                availabilityRequest.Dow = ABS.Navitaire.BookingManager.DOW.Daily;
                availabilityRequest.EndDate = DepartDate;// DateTime.Parse("2012-04-01");
                availabilityRequest.FareClassControl = FareClassControl.Default;
                availabilityRequest.FlightType = FlightType.All;
                availabilityRequest.InboundOutbound = InboundOutbound.None;
                availabilityRequest.IncludeAllotments = true;
                availabilityRequest.IncludeTaxesAndFees = true;
                availabilityRequest.JourneySortKeys = new JourneySortKey[1];
                availabilityRequest.JourneySortKeys[0] = new JourneySortKey();
                availabilityRequest.JourneySortKeys[0] = JourneySortKey.LowestFare;
                availabilityRequest.MaximumConnectingFlights = 4;
                availabilityRequest.MaximumFarePrice = Convert.ToDecimal(0.0);
                availabilityRequest.MinimumFarePrice = Convert.ToDecimal(0.0);
                availabilityRequest.NightsStay = 0;
                availabilityRequest.PaxCount = 1; // Convert.ToInt16(PaxNum);
                //tyas
                //availabilityRequest.CarrierCode = "DJ";
                availabilityRequest.SSRCollectionsMode = SSRCollectionsMode.All;
                PaxPriceType[] priceTypes = new PaxPriceType[2];//[model.PaxNum];
                for (int i = 0; i < 1; i++) //model.GuestNum
                {
                    priceTypes[i] = new PaxPriceType();
                    priceTypes[i].PaxType = "ADT";
                    priceTypes[i].PaxDiscountCode = String.Empty;
                }
                for (int j = 1; j < 2; j++)//model.PaxNum
                {
                    priceTypes[j] = new PaxPriceType();
                    priceTypes[j].PaxType = "CHD";
                    priceTypes[j].PaxDiscountCode = String.Empty;
                }
                availabilityRequest.PaxPriceTypes = priceTypes;
                //availabilityRequest.CurrencyCode = model.Carrier;

                if (PromoCode != null)
                    availabilityRequest.PromotionCode = PromoCode.ToUpper().Trim();//"AMADEUS";

                request.TripAvailabilityRequest.AvailabilityRequests[0] = availabilityRequest;
                //string xml = GetXMLString(request);
                GetAvailabilityResponse response = bookingAPI.GetAvailability(request);
                return response;
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                return null;
            }
        }

        public bool AddPaymentExtension(string RecordLocator, DateTime extensionDate, ref string errMessage, ref string sign)
        {
            GetBookingResponse bookingResponse = new BookingManager.GetBookingResponse();
            var profiler = MiniProfiler.Current;
            try
            {
                using (profiler.Step("Navitaire:GetBooking"))
                {
                    sign = GetBookingByPNR(RecordLocator, ref errMessage, ref bookingResponse);
                }

                if (sign != string.Empty)
                {
                    ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking("");

                    BookingCommitRequest request = new BookingCommitRequest();
                    BookingCommitRequestData requestData = new BookingCommitRequestData();

                    //booking commit with payment extension
                    requestData.RecordLocator = RecordLocator;
                    requestData.State = BookingManager.MessageState.Modified;

                    requestData.RestrictionOverride = false;
                    requestData.ChangeHoldDateTime = false;
                    requestData.WaiveNameChangeFee = false;
                    requestData.WaivePenaltyFee = false;
                    requestData.WaiveSpoilageFee = false;
                    requestData.DistributeToContacts = false;

                    requestData.ChangeHoldDateTime = true;
                    requestData.BookingHold = new BookingHold();
                    requestData.BookingHold.HoldDateTime = extensionDate;

                    request.BookingCommitRequestData = requestData;
                    request.Signature = sign;
                    request.ContractVersion = this.ContractVersion;

                    //string resp = GetXMLString(request);

                    BookingCommitResponse response = null;
                    IBookingManager bookingAPI = new BookingManagerClient();
                    response = bookingAPI.BookingCommit(request);

                    //resp = GetXMLString(response);

                    //Navitaire.APIBooking ApiBook = new Navitaire.APIBooking("");
                    //Navitaire.BookingManager.GetBookingResponse Response = new Navitaire.BookingManager.GetBookingResponse();
                    //Response = ApiBook.GetBookingResponseByPNR(RecordLocator);
                    //resp = GetXMLString(Response);

                    //Navitaire.BookingManager.Booking booking = ApiBook.GetBookingFromState(sign);
                    //resp = GetXMLString(Response);

                    //return false;
                    if (response.BookingUpdateResponseData.Error != null)
                    {
                        errMessage = response.BookingUpdateResponseData.Error.ErrorText;
                        return false;
                    }
                    else
                    {
                        errMessage = "";
                        return true;
                    }
                }
                else
                {
                    //fail retrieve booking
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool BookingCommitChange(string RecordLocator, string sign, ref string errMessage, string CurrencyCode = "", Boolean WaivePenaltyFee = true, Boolean WaiveSpoilageFee = false, string Username = "")
        {
            BookingCommitRequest request = new BookingCommitRequest();
            BookingCommitRequestData requestData = new BookingCommitRequestData();
            try
            {
                if (!string.IsNullOrEmpty(RecordLocator))
                    requestData.RecordLocator = RecordLocator;
                requestData.State = BookingManager.MessageState.Modified;

                requestData.RestrictionOverride = false;
                requestData.ChangeHoldDateTime = false;
                requestData.WaiveNameChangeFee = false;
                requestData.WaivePenaltyFee = WaivePenaltyFee;
                requestData.WaiveSpoilageFee = WaiveSpoilageFee;
                requestData.DistributeToContacts = false;
                if (CurrencyCode != "")
                {
                    requestData.CurrencyCode = CurrencyCode;
                }

                request.BookingCommitRequestData = requestData;
                request.Signature = sign;
                request.ContractVersion = this.ContractVersion;

                BookingCommitResponse response = null;
                IBookingManager bookingAPI = new BookingManagerClient();
                //string xml = GetXMLString(request);
                response = bookingAPI.BookingCommit(request);
                //xml = GetXMLString(response);

                Navitaire.APIBooking ApiBook = new Navitaire.APIBooking("");
                Navitaire.BookingManager.GetBookingResponse Response = new Navitaire.BookingManager.GetBookingResponse();
                Response = ApiBook.GetBookingResponseByPNR(RecordLocator);
                //string resp = GetXMLString(Response);
                if (HttpContext.Current.Session["UnassignSeats"] != null && Convert.ToBoolean(HttpContext.Current.Session["UnassignSeats"]) == true)
                {
                    HttpContext.Current.Session["SPLFee_"+ RecordLocator] = ((HttpContext.Current.Session["SPLFee_" + RecordLocator] == null) ? 0 : (Convert.ToDecimal(HttpContext.Current.Session["SPLFee_" + RecordLocator]))) + Response.Booking.Passengers.Select(p => p.PassengerFees.Where(fee => fee.FeeType == FeeType.SpoilageFee).Select(charge => charge.ServiceCharges.Select(x => x.Amount).Sum()).LastOrDefault()).Sum();
                    HttpContext.Current.Session["UnassignSeats"] = null;
                }

                if (response.BookingUpdateResponseData.Error != null)
                {
                    errMessage = response.BookingUpdateResponseData.Error.ErrorText;
                    return false;
                }
                else
                {
                    //AgentLogout(sign);
                    errMessage = "";
                    return true;
                }
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                errMessage = ex.Message.ToString();
                log.Error(this, ex);
                return false;
            }
        }


        public bool BookingCommitFlightChange(string RecordLocator, string sign, ref string errMessage, string CurrencyCode = "", Boolean WaivePenaltyFee = true, Boolean WaiveSpoilageFee = false, string Username = "")
        {
            BookingCommitRequest request = new BookingCommitRequest();
            BookingCommitRequestData requestData = new BookingCommitRequestData();
            try
            {
                requestData.RecordLocator = RecordLocator;
                requestData.State = BookingManager.MessageState.Modified;

                requestData.RestrictionOverride = false;
                requestData.ChangeHoldDateTime = false;
                requestData.WaiveNameChangeFee = false;
                requestData.WaivePenaltyFee = WaivePenaltyFee;
                requestData.WaiveSpoilageFee = WaiveSpoilageFee;
                requestData.DistributeToContacts = false;
                if (CurrencyCode != "")
                {
                    requestData.CurrencyCode = CurrencyCode;
                }

                request.BookingCommitRequestData = requestData;
                request.Signature = sign;
                request.ContractVersion = this.ContractVersion;

                BookingCommitResponse response = null;
                IBookingManager bookingAPI = new BookingManagerClient();
                //string xml = GetXMLString(request);
                response = bookingAPI.BookingCommit(request);
                //xml = GetXMLString(response);

                Navitaire.APIBooking ApiBook = new Navitaire.APIBooking("");
                Navitaire.BookingManager.GetBookingResponse Response = new Navitaire.BookingManager.GetBookingResponse();
                Response = ApiBook.GetBookingResponseByPNR(RecordLocator);
                //string resp = GetXMLString(Response);

                if (response.BookingUpdateResponseData.Error != null)
                {
                    errMessage = response.BookingUpdateResponseData.Error.ErrorText;
                    return false;
                }
                else
                {
                    errMessage = "";
                    return true;
                }
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                errMessage = ex.Message.ToString();
                log.Error(this, ex);
                return false;
            }
        }

        public bool BookingCommit(string RecordLocator, string sign, ref string errMessage, string CurrencyCode = "", Boolean WaivePenaltyFee = false, Boolean WaiveSpoilageFee = false, string Username = "", string AgentID = "", string contactTitle = "", string contactFirstName = "", string contactLastName = "", string contactEmail = "", string contactPhone = "", string contactAddress = "", string contactTown = "", string contactCountry = "", string contactState = "", string contactZipCode = "", string OrganizationName = "", string OrganizationID = "")
        {
            BookingCommitRequest request = new BookingCommitRequest();
            BookingCommitRequestData requestData = new BookingCommitRequestData();
            var profiler = MiniProfiler.Current;
            try
            {
                //Booking booking = GetBookingFromState(sign);

                requestData.RecordLocator = RecordLocator;
                requestData.State = BookingManager.MessageState.Modified;

                requestData.RestrictionOverride = false;
                requestData.ChangeHoldDateTime = false;
                requestData.WaiveNameChangeFee = true;
                requestData.WaivePenaltyFee = WaivePenaltyFee;
                requestData.WaiveSpoilageFee = WaiveSpoilageFee;
                requestData.DistributeToContacts = false;

                if (CurrencyCode != "")
                {
                    requestData.CurrencyCode = CurrencyCode;
                }

                //added by diana 20131118 - to put contacts into booking
                #region BookingContacts
                if (Username != "")
                {
                    string psDomain = "";
                    string psName = "";
                    string psPwd = "";

                    psDomain = ConfigurationManager.AppSettings["signature_domain"].ToString();
                    psName = ConfigurationManager.AppSettings["signature_username"].ToString();
                    psPwd = ConfigurationManager.AppSettings["signature_password"].ToString();

                    APIAgent apiAgent = new APIAgent();
                    GetPersonResponse PersonResp = new GetPersonResponse();

                    string signature = AgentLogon("Public", psDomain, psName, psPwd);
                    using (profiler.Step("Navitaire:GetPerson"))
                    {
                        PersonResp = apiAgent.GetPersonByID(Username, signature, AgentID);
                    }

                    string title = ""; string firstName = ""; string lastName = ""; string contactAddress2 = ""; string contactAddress3 = ""; string email = ""; string fax = ""; string phoneNo = ""; string mobileNo = "";
                    if (PersonResp != null)
                    {
                        //if (PersonResp.Person.PersonNameList.Length > 0)
                        //{
                        //    title = PersonResp.Person.PersonNameList[0].Name.Title;
                        //    firstName = PersonResp.Person.PersonNameList[0].Name.FirstName;
                        //    lastName = PersonResp.Person.PersonNameList[0].Name.LastName;
                        //}

                        //if (PersonResp.Person.PersonEMailList.Length > 0)
                        //{
                        //    email = PersonResp.Person.PersonEMailList[0].EMailAddress;
                        //}
                        if (PersonResp.Person.PersonPhoneList.Length > 0)
                        {
                            phoneNo = PersonResp.Person.PersonPhoneList[0].Number;
                        }
                        if (PersonResp.Person.PersonAddressList.Length > 0)
                        {
                            contactAddress2 = PersonResp.Person.PersonAddressList[0].Address.AddressLine2;
                            contactAddress3 = PersonResp.Person.PersonAddressList[0].Address.AddressLine3;
                        }
                        if (PersonResp.Person.PersonContactList.Length > 0)
                        {
                            mobileNo = PersonResp.Person.PersonContactList[0].WorkPhone;
                            fax = PersonResp.Person.PersonContactList[0].Fax;
                        }

                        if (PersonResp.Person.PersonPhoneList.Length > 0)
                        {
                            if (PersonResp.Person.PersonPhoneList[0].Number != "")
                            {
                                mobileNo = PersonResp.Person.PersonPhoneList[0].Number;
                            }
                            else
                            {
                                mobileNo = PersonResp.Person.PersonPhoneList[0].PhoneCode;
                            }
                        }

                    }

                    requestData.BookingContacts = new BookingContact[1];
                    requestData.BookingContacts[0] = new BookingContact();
                    requestData.BookingContacts[0].AddressLine1 = contactAddress;// "KLIA";

                    if (contactAddress2 != null) requestData.BookingContacts[0].AddressLine2 = contactAddress2; else requestData.BookingContacts[0].AddressLine2 = "-";
                    if (contactAddress3 != null) requestData.BookingContacts[0].AddressLine3 = contactAddress3; else requestData.BookingContacts[0].AddressLine3 = "-";

                    requestData.BookingContacts[0].City = contactTown;// "";
                    requestData.BookingContacts[0].CompanyName = OrganizationName;// "VanceInfo";
                    requestData.BookingContacts[0].ProvinceState = contactState;// "MY";
                    requestData.BookingContacts[0].PostalCode = contactZipCode;// "MY";
                    requestData.BookingContacts[0].CountryCode = contactCountry;// "MY";
                    requestData.BookingContacts[0].CultureCode = "";
                    requestData.BookingContacts[0].CustomerNumber = "";
                    requestData.BookingContacts[0].DistributionOption = DistributionOption.Email;
                    requestData.BookingContacts[0].EmailAddress = contactEmail;// "chen_yongqing-cyq@Vanceinfo.com";
                    requestData.BookingContacts[0].SourceOrganization = OrganizationID;

                    if (fax != "") requestData.BookingContacts[0].Fax = fax; else requestData.BookingContacts[0].Fax = "-";
                    requestData.BookingContacts[0].HomePhone = contactPhone;

                    requestData.BookingContacts[0].Names = new BookingName[1];
                    requestData.BookingContacts[0].Names[0] = new BookingName();
                    requestData.BookingContacts[0].Names[0].FirstName = contactFirstName;// "Charis";
                    requestData.BookingContacts[0].Names[0].LastName = contactLastName;// "Chen";
                    requestData.BookingContacts[0].Names[0].Title = contactTitle;// "MS";
                    requestData.BookingContacts[0].Names[0].State = BookingManager.MessageState.New;
                    requestData.BookingContacts[0].NotificationPreference = BookingManager.NotificationPreference.None;

                    if (mobileNo != "") requestData.BookingContacts[0].OtherPhone = mobileNo; else requestData.BookingContacts[0].OtherPhone = contactPhone;
                    if (mobileNo != "") requestData.BookingContacts[0].WorkPhone = mobileNo; else requestData.BookingContacts[0].WorkPhone = contactPhone;

                    requestData.BookingContacts[0].State = BookingManager.MessageState.New;
                    requestData.BookingContacts[0].TypeCode = "P";

                }
                #endregion

                request.BookingCommitRequestData = requestData;
                request.Signature = sign;
                request.ContractVersion = this.ContractVersion;

                BookingCommitResponse response = null;
                IBookingManager bookingAPI = new BookingManagerClient();
                //string xml = GetXMLString(request);
                response = bookingAPI.BookingCommit(request);
                //xml = GetXMLString(response);

                Navitaire.APIBooking ApiBook = new Navitaire.APIBooking("");
                Navitaire.BookingManager.GetBookingResponse Response = new Navitaire.BookingManager.GetBookingResponse();
                Response = ApiBook.GetBookingResponseByPNR(RecordLocator);
                //string resp = GetXMLString(Response);

                if (response.BookingUpdateResponseData.Error != null)
                {
                    errMessage = response.BookingUpdateResponseData.Error.ErrorText;
                    return false;
                }
                else
                {
                    errMessage = "";
                    return true;
                }
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                errMessage = ex.Message.ToString();
                log.Error(this, ex);
                return false;
            }
        }


        //backup by ketee
        public GetAvailabilityResponse GetOneAvailability_bak(int TemFlightPaxNum, int TemFlightADTNum, DateTime TemFlightDate, string TemFlightDeparture, string TemFlightArrival, string TemFlightFlightNumber, string TemFlightCurrencyCode, ref string SessionID)
        {
            SessionID = this.Signature;
            IBookingManager bookingAPI = new BookingManagerClient();
            GetAvailabilityRequest request = new GetAvailabilityRequest();
            request.Signature = SessionID;// SessionManager._signature;
            request.ContractVersion = this.ContractVersion;
            request.TripAvailabilityRequest = new TripAvailabilityRequest();
            request.TripAvailabilityRequest.AvailabilityRequests = new AvailabilityRequest[1];
            AvailabilityRequest availabilityRequest = new AvailabilityRequest();
            PaxPriceType[] priceTypes = new PaxPriceType[TemFlightPaxNum];
            for (int i = 0; i < TemFlightADTNum; i++)
            {
                priceTypes[i] = new PaxPriceType();
                priceTypes[i].PaxType = "ADT";
                priceTypes[i].PaxDiscountCode = String.Empty;
            }
            for (int j = TemFlightADTNum; j < TemFlightPaxNum; j++)
            {
                priceTypes[j] = new PaxPriceType();
                priceTypes[j].PaxType = "CHD";
                priceTypes[j].PaxDiscountCode = String.Empty;
            }
            availabilityRequest.PaxPriceTypes = priceTypes;
            availabilityRequest.AvailabilityType = AvailabilityType.Default;
            availabilityRequest.FareClassControl = FareClassControl.Default;
            availabilityRequest.AvailabilityFilter = AvailabilityFilter.Default;
            availabilityRequest.BeginDate = TemFlightDate;// DateTime.Parse("2012-04-01");
            availabilityRequest.EndDate = TemFlightDate;// DateTime.Parse("2012-04-01");
            availabilityRequest.DepartureStation = TemFlightDeparture.Trim();// "SZX";
            availabilityRequest.ArrivalStation = TemFlightArrival.Trim();// "KUL";
            availabilityRequest.FlightNumber = TemFlightFlightNumber.Trim().PadLeft(4, ' ');
            availabilityRequest.FlightType = FlightType.All;
            availabilityRequest.CurrencyCode = TemFlightCurrencyCode.Trim();// "CNY";
            availabilityRequest.Dow = ABS.Navitaire.BookingManager.DOW.Daily;
            availabilityRequest.InboundOutbound = InboundOutbound.None;
            availabilityRequest.IncludeAllotments = true;
            availabilityRequest.IncludeTaxesAndFees = true;
            availabilityRequest.JourneySortKeys = new JourneySortKey[1];
            availabilityRequest.JourneySortKeys[0] = JourneySortKey.LowestFare;
            availabilityRequest.MaximumConnectingFlights = 4;
            availabilityRequest.MaximumFarePrice = Convert.ToDecimal(0.0);
            availabilityRequest.MinimumFarePrice = Convert.ToDecimal(0.0);
            availabilityRequest.SSRCollectionsMode = SSRCollectionsMode.All;
            availabilityRequest.NightsStay = 0;
            availabilityRequest.PaxCount = 0;//30;//Convert.ToInt16(model.TemFlightPaxNum);// 
            //availabilityRequest.CurrencyCode = model.Carrier;

            request.TripAvailabilityRequest.AvailabilityRequests[0] = availabilityRequest;
            GetAvailabilityResponse response = bookingAPI.GetAvailability(request);
            //if (response.GetTripAvailabilityResponse.Schedules.Length > 0)
            //{
            //    string xml = "";
            //    for (int i = 0; i < response.GetTripAvailabilityResponse.Schedules[0][0].Journeys[0].Segments[0].Fares.Length; i++)
            //    {
            //        xml += GetXMLString(response.GetTripAvailabilityResponse.Schedules[0][0].Journeys[0].Segments[0].Fares[i]);
            //    }
            //}
            return response;
        }

        //amend by ketee 20130625
        public GetAvailabilityResponse GetOneAvailability(int TemFlightPaxNum, int TemFlightADTNum, DateTime TemFlightDate, string TemFlightDeparture, string TemFlightArrival, string TemFlightFlightNumber, string TemFlightCurrencyCode, ref string SessionID, string TemFlightPromoCode)
        {
            SessionID = this.Signature;
            //log.Info(this,"GetAvailabilityResponse - SessionID : " + SessionID);
            IBookingManager bookingAPI = new BookingManagerClient();
            GetAvailabilityRequest request = new GetAvailabilityRequest();
            request.Signature = SessionID;// SessionManager._signature;
            request.ContractVersion = this.ContractVersion;
            request.TripAvailabilityRequest = new TripAvailabilityRequest();
            request.TripAvailabilityRequest.AvailabilityRequests = new AvailabilityRequest[1];
            AvailabilityRequest availabilityRequest = new AvailabilityRequest();
            //PaxPriceType[] priceTypes = new PaxPriceType[TemFlightPaxNum];
            PaxPriceType[] priceTypes = new PaxPriceType[2];
            for (int i = 0; i < 1; i++)
            {
                priceTypes[i] = new PaxPriceType();
                priceTypes[i].PaxType = "ADT";
                priceTypes[i].PaxDiscountCode = String.Empty;
            }
            for (int j = 1; j < 2; j++)
            {
                priceTypes[j] = new PaxPriceType();
                priceTypes[j].PaxType = "CHD";
                priceTypes[j].PaxDiscountCode = String.Empty;
            }
            availabilityRequest.PaxPriceTypes = priceTypes;
            availabilityRequest.AvailabilityType = AvailabilityType.Default;
            availabilityRequest.FareClassControl = FareClassControl.Default;
            availabilityRequest.AvailabilityFilter = AvailabilityFilter.Default;
            availabilityRequest.BeginDate = TemFlightDate;// DateTime.Parse("2012-04-01");
            availabilityRequest.EndDate = TemFlightDate;// DateTime.Parse("2012-04-01");
            availabilityRequest.DepartureStation = TemFlightDeparture.Trim();// "SZX";
            availabilityRequest.ArrivalStation = TemFlightArrival.Trim();// "KUL";
            if (TemFlightFlightNumber.Trim() != "") availabilityRequest.FlightNumber = TemFlightFlightNumber.Trim().PadLeft(4, ' ');
            availabilityRequest.FlightType = FlightType.All;
            availabilityRequest.CurrencyCode = TemFlightCurrencyCode.Trim();// "CNY";
            availabilityRequest.Dow = ABS.Navitaire.BookingManager.DOW.Daily;
            availabilityRequest.InboundOutbound = InboundOutbound.None;
            availabilityRequest.IncludeAllotments = true;
            availabilityRequest.IncludeTaxesAndFees = true;
            availabilityRequest.JourneySortKeys = new JourneySortKey[1];
            availabilityRequest.JourneySortKeys[0] = JourneySortKey.LowestFare;
            availabilityRequest.MaximumConnectingFlights = 4;
            availabilityRequest.MaximumFarePrice = Convert.ToDecimal(0.0);
            availabilityRequest.MinimumFarePrice = Convert.ToDecimal(0.0);
            availabilityRequest.SSRCollectionsMode = SSRCollectionsMode.All;
            availabilityRequest.NightsStay = 0;
            availabilityRequest.PaxCount = 1;//30;//Convert.ToInt16(model.TemFlightPaxNum);// 
            //availabilityRequest.CurrencyCode = model.Carrier;

            if (TemFlightPromoCode != null)
                availabilityRequest.PromotionCode = TemFlightPromoCode.ToUpper().Trim();//"AMADEUS";

            request.TripAvailabilityRequest.AvailabilityRequests[0] = availabilityRequest;

            //string xml = GetXMLString(request);

            GetAvailabilityResponse response = bookingAPI.GetAvailability(request);
            //if (response.GetTripAvailabilityResponse.Schedules.Length > 0)
            //{
            //    string xml = "";
            //    for (int i = 0; i < response.GetTripAvailabilityResponse.Schedules[0][0].Journeys[0].Segments[0].Fares.Length; i++)
            //    {
            //        xml += GetXMLString(response.GetTripAvailabilityResponse.Schedules[0][0].Journeys[0].Segments[0].Fares[i]);
            //    }
            //}
            return response;
        }

        public GetAvailabilityResponse GetOneAvailabilityWithPaxCount(int TemFlightPaxNum, int TemFlightADTNum, DateTime TemFlightDate, string TemFlightDeparture, string TemFlightArrival, string TemFlightFlightNumber, string TemFlightCurrencyCode, ref string SessionID)
        {
            SessionID = this.Signature;
            IBookingManager bookingAPI = new BookingManagerClient();
            GetAvailabilityRequest request = new GetAvailabilityRequest();
            request.Signature = SessionID;// SessionManager._signature;
            request.ContractVersion = this.ContractVersion;
            request.TripAvailabilityRequest = new TripAvailabilityRequest();
            request.TripAvailabilityRequest.AvailabilityRequests = new AvailabilityRequest[1];
            AvailabilityRequest availabilityRequest = new AvailabilityRequest();
            PaxPriceType[] priceTypes = new PaxPriceType[TemFlightPaxNum];
            for (int i = 0; i < TemFlightADTNum; i++)
            {
                priceTypes[i] = new PaxPriceType();
                priceTypes[i].PaxType = "ADT";
                priceTypes[i].PaxDiscountCode = String.Empty;
            }
            for (int j = TemFlightADTNum; j < TemFlightPaxNum; j++)
            {
                priceTypes[j] = new PaxPriceType();
                priceTypes[j].PaxType = "CHD";
                priceTypes[j].PaxDiscountCode = String.Empty;
            }
            availabilityRequest.PaxPriceTypes = priceTypes;
            availabilityRequest.AvailabilityType = AvailabilityType.Default;
            availabilityRequest.FareClassControl = FareClassControl.Default;
            availabilityRequest.AvailabilityFilter = AvailabilityFilter.Default;
            availabilityRequest.BeginDate = TemFlightDate;// DateTime.Parse("2012-04-01");
            availabilityRequest.EndDate = TemFlightDate;// DateTime.Parse("2012-04-01");
            availabilityRequest.DepartureStation = TemFlightDeparture.Trim();// "SZX";
            availabilityRequest.ArrivalStation = TemFlightArrival.Trim();// "KUL";
            availabilityRequest.FlightNumber = TemFlightFlightNumber.Trim().PadLeft(4, ' ');
            availabilityRequest.FlightType = FlightType.All;
            availabilityRequest.CurrencyCode = TemFlightCurrencyCode.Trim();// "CNY";
            availabilityRequest.Dow = ABS.Navitaire.BookingManager.DOW.Daily;
            availabilityRequest.InboundOutbound = InboundOutbound.None;
            availabilityRequest.IncludeAllotments = true;
            availabilityRequest.IncludeTaxesAndFees = true;
            availabilityRequest.JourneySortKeys = new JourneySortKey[1];
            availabilityRequest.JourneySortKeys[0] = JourneySortKey.LowestFare;
            availabilityRequest.MaximumConnectingFlights = 4;
            availabilityRequest.MaximumFarePrice = Convert.ToDecimal(0.0);
            availabilityRequest.MinimumFarePrice = Convert.ToDecimal(0.0);
            availabilityRequest.SSRCollectionsMode = SSRCollectionsMode.All;
            availabilityRequest.NightsStay = 0;
            availabilityRequest.PaxCount = Convert.ToInt16(TemFlightPaxNum);
            //availabilityRequest.CurrencyCode = model.Carrier;

            request.TripAvailabilityRequest.AvailabilityRequests[0] = availabilityRequest;
            GetAvailabilityResponse response = bookingAPI.GetAvailability(request);

            return response;
        }


        public string AgentLogon(string AgentType = "public", string domain = "", string username = "", string password = "")
        {
            //domain = "def";
            //username = "APIGRPBOOK";
            //password = "grp@book1";
            return this.baseAgentLogon(AgentType, domain, username, password);
        }

        public string AgentLogout(string signature = "")
        {
            //domain = "def";
            //username = "APIGRPBOOK";
            //password = "grp@book1";
            return this.baseAgentLogout(signature);
        }

        /*
        public void Payment_Do3DQueryResult()
        {

            APIPayment.AAWebServicesSoap PaymentAPI = new AAWebServicesSoapClient();
            APIPayment.Do3DQueryResultRequest request = new Do3DQueryResultRequest();
            request.Body = new Do3DQueryResultRequestBody();
            request.Body.sz_TransactionType = "";
            request.Body.sz_ServiceID = "";
            request.Body.sz_PaymentID = "";
            request.Body.sz_HashMethod = "";
            request.Body.sz_HashValue = "";

            APIPayment.Do3DQueryResultResponse response = PaymentAPI.Do3DQueryResult(request);
        }
        public void Payment_Do3DReversal()
        {
            APIPayment.AAWebServicesSoap PaymentAPI = new AAWebServicesSoapClient();
            APIPayment.Do3DReversalRequest request = new Do3DReversalRequest();
            request.Body = new Do3DReversalRequestBody();
            request.Body.sz_Amount = "";
            request.Body.sz_CurrencyCode = "";
            request.Body.sz_TransactionType = "";
            request.Body.sz_ServiceID = "";
            request.Body.sz_PaymentID = "";
            request.Body.sz_HashMethod = "";
            request.Body.sz_HashValue = "";

            APIPayment.Do3DReversalResponse response = PaymentAPI.Do3DReversal(request);
        }

        public void Booking()
        {
            IBookingManager bookingAPI = new BookingManagerClient();
            AddPaymentToBookingRequest payment = new AddPaymentToBookingRequest();
            payment.addPaymentToBookingReqData = new AddPaymentToBookingRequestData();
            payment.addPaymentToBookingReqData.Status = BookingPaymentStatus.New;
            payment.addPaymentToBookingReqData.PaymentMethodType = RequestPaymentMethodType.AgencyAccount;
            payment.addPaymentToBookingReqData.PaymentMethodCode = "AG";
            payment.addPaymentToBookingReqData.QuotedCurrencyCode = "USD";
            payment.addPaymentToBookingReqData.QuotedAmount = 31.10M;
            payment.addPaymentToBookingReqData.AccountNumber = "88887777";
            payment.addPaymentToBookingReqData.PaymentText = "Sample AG Payment";
            payment.addPaymentToBookingReqData.AgencyAccount = new AgencyAccount();
            payment.addPaymentToBookingReqData.AgencyAccount.Password = String.Empty;
            AddPaymentToBookingResponse resp = bookingAPI.AddPaymentToBooking(payment);

        }
         */

        public GetPaymentFeePriceResponse GetProcessingFee(string signature, string currency, decimal amount)
        {
            try
            {
                GetPaymentFeePriceRequest request = new GetPaymentFeePriceRequest();
                request.ContractVersion = this.ContractVersion;
                request.Signature = signature;

                request.paymentFeePriceReqData = new PaymentFeePriceRequest();
                request.paymentFeePriceReqData.CurrencyCode = currency;
                request.paymentFeePriceReqData.PaymentAmount = amount;
                request.paymentFeePriceReqData.FeeCode = "CONA";

                //string Xml = GetXMLString(request);

                IBookingManager bookingAPI = new BookingManagerClient();
                GetPaymentFeePriceResponse response = bookingAPI.GetPaymentFeePrice(request);

                //string Xml2 = GetXMLString(response);

                return response;
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                return null;
            }
        }

        public Boolean AddPaymentCreditCard(string cardType, string CardNumber, string ExpirationDate, string CurrencyCode, decimal amount, string CVV, string HolderName, string BankName, string Country, string psSellSignature, string strCCAddres, string strCCCity, string strCCCountry, string strCCState, string strCCZipCode, string payComment, ref string errMessage)
        {
            IBookingManager bookingAPI = new BookingManagerClient();
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;
            try
            {
                //throw (new TimeoutException()); //for testing purpose

                AddPaymentToBookingRequest request = new AddPaymentToBookingRequest();

                request.ContractVersion = this.ContractVersion;
                request.Signature = psSellSignature;// SessionManager._signature;                
                request.addPaymentToBookingReqData = new AddPaymentToBookingRequestData();
                request.addPaymentToBookingReqData.AccountNumber = CardNumber;// "4444333322221111";
                request.addPaymentToBookingReqData.AccountNumberID = 0;
                request.addPaymentToBookingReqData.AgencyAccount = new AgencyAccount();
                request.addPaymentToBookingReqData.AgencyAccount.AccountID = 0;
                request.addPaymentToBookingReqData.AgencyAccount.AccountTransactionID = 0;
                request.addPaymentToBookingReqData.Deposit = false;
                request.addPaymentToBookingReqData.Expiration = DateTime.Parse(ExpirationDate);//DateTime.Parse("2015-01-01");
                request.addPaymentToBookingReqData.Installments = 1;
                request.addPaymentToBookingReqData.ParentPaymentID = 0;
                request.addPaymentToBookingReqData.PaymentMethodCode = cardType;
                request.addPaymentToBookingReqData.PaymentMethodType = RequestPaymentMethodType.ExternalAccount;
                request.addPaymentToBookingReqData.PaymentText = " Pay by " + cardType + payComment;
                request.addPaymentToBookingReqData.QuotedAmount = amount;// 205;
                request.addPaymentToBookingReqData.QuotedCurrencyCode = CurrencyCode;// "MYR";
                request.addPaymentToBookingReqData.ReferenceType = PaymentReferenceType.Default;
                request.addPaymentToBookingReqData.Status = BookingPaymentStatus.New;
                request.addPaymentToBookingReqData.WaiveFee = false;
                request.addPaymentToBookingReqData.PaymentFields = new PaymentField[4];
                request.addPaymentToBookingReqData.PaymentFields[0] = new PaymentField();
                request.addPaymentToBookingReqData.PaymentFields[0].FieldName = "CC::VerificationCode";
                request.addPaymentToBookingReqData.PaymentFields[0].FieldValue = CVV;// "998";
                request.addPaymentToBookingReqData.PaymentFields[1] = new PaymentField();
                request.addPaymentToBookingReqData.PaymentFields[1].FieldName = "CC::AccountHolderName";
                request.addPaymentToBookingReqData.PaymentFields[1].FieldValue = HolderName;// "John Smith";
                request.addPaymentToBookingReqData.PaymentFields[2] = new PaymentField();
                request.addPaymentToBookingReqData.PaymentFields[2].FieldName = "ISSNAME";
                request.addPaymentToBookingReqData.PaymentFields[2].FieldValue = BankName;// "bank";
                request.addPaymentToBookingReqData.PaymentFields[3] = new PaymentField();
                request.addPaymentToBookingReqData.PaymentFields[3].FieldName = "ISSCTRY";
                request.addPaymentToBookingReqData.PaymentFields[3].FieldValue = "MAL"; //Country;// "CN";

                PaymentAddress[] ccAddress = new PaymentAddress[1];
                ccAddress[0] = new PaymentAddress();

                ccAddress[0].AddressLine1 = strCCAddres;
                ccAddress[0].City = strCCCity;
                ccAddress[0].CountryCode = strCCCountry;
                ccAddress[0].ProvinceState = strCCState;
                ccAddress[0].PostalCode = strCCZipCode;
                request.addPaymentToBookingReqData.PaymentAddresses = ccAddress;

                //string str = GetXMLString(request);
                AddPaymentToBookingResponse response;// = bookingAPI.AddPaymentToBooking(request);
                using (profiler.Step("Navitaire:AddPaymentToBooking"))
                {
                    response = bookingAPI.AddPaymentToBooking(request);
                }
                //string str2 = GetXMLString(response);

                //test to get booking response
                //Booking book = GetBookingFromState(psSellSignature);
                //string resp = GetXMLString(book);
                //end test to get booking reponse

                if (response.BookingPaymentResponse.ValidationPayment.PaymentValidationErrors.Length > 0)
                {
                    errMessage = response.BookingPaymentResponse.ValidationPayment.PaymentValidationErrors[0].ErrorDescription;
                    return false;
                }
                else
                {
                    errMessage = "";
                    return true;
                }
                /*
                if (response.BookingPaymentResponse.ValidationPayment.Payment.Status.ToString().ToLower() == "received")
                {
                    return true;
                }
                else
                {
                    return false;
                }
                */
            }
            //amended by diana 20131210 - try catch to check for valid booking
            catch (TimeoutException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), psSellSignature); return false; }
            catch (OutOfMemoryException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), psSellSignature); return false; }
            catch (IndexOutOfRangeException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), psSellSignature); return false; }
            catch (ThreadInterruptedException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), psSellSignature); return false; }
            catch (NullReferenceException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), psSellSignature); return false; }
            catch (StackOverflowException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), psSellSignature); return false; }
            catch (ApplicationException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), psSellSignature); return false; }
            catch (Exception ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), psSellSignature); return false; }
            finally
            {

            }
        }

        //added by diana 20131210 - when exception, check journey exist
        public void CheckJourneyExist(string exceptionMessage, string sellSignature, string PNR = "")
        {
            var profiler = MiniProfiler.Current;
            HttpContext.Current.Session["ExceptionMessage"] = exceptionMessage + " ";
            HttpContext.Current.Session["InvalidBooking"] = "false";

            //test to get booking response
            if (sellSignature != "")
            {
                Booking booking = new Booking();
                using (profiler.Step("Navitaire:GetBookingFromState"))
                {
                    booking = GetBookingFromState(sellSignature);
                }
                if (booking != null)
                {
                    if (booking.Journeys.Length <= 0)
                    {
                        log.Info(this, "Journey does not exist : " + sellSignature);
                        HttpContext.Current.Session["InvalidBooking"] = "true";
                    }
                    else
                    {
                        log.Info(this, "Journey exists : " + sellSignature);
                    }
                }
                else
                {
                    log.Info(this, "Journey does not exist : " + sellSignature);
                    HttpContext.Current.Session["InvalidBooking"] = "true";
                }
            }
            else
            {
                GetBookingResponse Response = new GetBookingResponse();
                using (profiler.Step("Navitaire:GetBookingResponseByPNR"))
                {
                    Response = GetBookingResponseByPNR(PNR);
                }
                if (Response != null)
                {
                    if (Response.Booking == null || Response.Booking.Journeys.Length <= 0)
                    {
                        log.Info(this, "Journey does not exist : " + PNR);
                        HttpContext.Current.Session["InvalidBooking"] = "true";
                    }
                    else
                    {
                        log.Info(this, "Journey exists : " + PNR);
                    }
                }
                else
                {
                    log.Info(this, "Journey does not exist : " + PNR);
                    HttpContext.Current.Session["InvalidBooking"] = "true";
                }
            }
            //end test to get booking reponse
        }

        public bool MoveJourney(string FareSellKey, string JourneySellKey, string ToFareSellKey, string ToJourneySellKey, string currency, ref string ErrMsg, ref string sign, string FareSellKey2 = "", string ToFareSellKey2 = "")
        {
            IBookingManager bookingAPI = new BookingManagerClient();
            MoveJourneyByKeyRequest request = new MoveJourneyByKeyRequest();
            MoveJourneyByKeyRequestData requestData = new MoveJourneyByKeyRequestData();

            //string sign = AgentLogon();

            requestData.ChangeReasonCode = "Flight Interruption";
            requestData.MoveType = MovePassengerJourneyType.None;
            requestData.FromJourneySellKeys = new SellKeyList();
            requestData.FromJourneySellKeys.FareSellKey = FareSellKey;
            requestData.FromJourneySellKeys.JourneySellKey = JourneySellKey;

            requestData.ToJourneyActionStatusCode = "OS";
            requestData.ToJourneySellKeys = new SellKeyList();
            requestData.ToJourneySellKeys.FareSellKey = ToFareSellKey;

            requestData.BoardingSequenceOffset = 0;
            requestData.CollectedCurrencyCode = currency;
            requestData.ToJourneySellKeys.JourneySellKey = ToJourneySellKey;
            requestData.IgnoreClosedFlightStatus = true;
            requestData.IgnoreLiftStatus = IgnoreLiftStatus.IgnoreCheckin;
            requestData.KeepWaitListStatus = false;
            requestData.ChangeStatus = false;
            //requestData.Oversell = true;
            requestData.Oversell = false;
            requestData.Commit = false;

            request.ContractVersion = this.ContractVersion;
            request.Signature = sign;
            request.MoveJourneyByKeyRequestData = requestData;

            //string Xml = GetXMLString(request);
            BookingUpdateResponseData updateResponse = new BookingUpdateResponseData();
            try
            {
                MoveJourneyByKeyResponse response = bookingAPI.MoveJourney(request);
                //string Xml2 = GetXMLString(response);
                Booking BookingInfo = new Booking();
                BookingInfo = GetBookingFromState(sign);
                //string Xml3 = GetXMLString(BookingInfo);

                Success Success = updateResponse.Success;
                Warning Warning = updateResponse.Warning;
                Error Err = updateResponse.Error;
                if ((Err != null) && !string.IsNullOrEmpty(Err.ErrorText))
                {
                    ErrMsg = Err.ErrorText;
                    return false;
                }
                else if ((Warning != null) && !string.IsNullOrEmpty(Warning.WarningText))
                {
                    return false;
                    //return true;
                }
                else
                {
                    return true;
                }
            }

           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                ErrMsg = ex.ToString();
                return false;
            }
        }

        public bool MoveJourneyBySegment(string _RecordLocator, int _ContractVersion, string _Signature, Navitaire.BookingManager.Journey NewJourney, Navitaire.BookingManager.Journey OriJourney, ref string ErrMsg)
        {
            Navitaire.BookingManager.Segment NewFlightInfo = new Navitaire.BookingManager.Segment();
            Navitaire.BookingManager.Segment OriFlightInfo = new Navitaire.BookingManager.Segment();

            Navitaire.BookingManager.Segment TransitNewFlightInfo = new Navitaire.BookingManager.Segment();
            Navitaire.BookingManager.Segment TransitOriFlightInfo = new Navitaire.BookingManager.Segment();
            NewFlightInfo = NewJourney.Segments[0];
            OriFlightInfo = OriJourney.Segments[0];

            if (OriJourney.Segments.Length > 1)
            {
                TransitOriFlightInfo = OriJourney.Segments[1];
            }

            if (NewJourney.Segments.Length > 1)
            {
                TransitNewFlightInfo = NewJourney.Segments[1];
            }

            //Create an instance of BookingManagerClient
            BookingManagerClient bookingAPI = new BookingManagerClient();
            //Create the flight to move out from.
            Journey fromJourney = new Journey();
            Journey toJourney = new Journey();
            Journey[] JourneyList = new Journey[1];
            FlightDesignator FlightDesignator = new FlightDesignator();
            Segment[] fromSegments = new Segment[1];
            Segment[] toSegments = new Segment[1];
            Segment Segment = new Segment();
            Leg[] fromLegs1 = new Leg[1];
            Leg[] fromLegs2 = new Leg[1];
            Leg[] toLegs1 = new Leg[1];
            Leg[] toLegs2 = new Leg[1];
            Leg leg = new Leg();

            Fare[] fromFares1 = new Fare[1];
            AvailableFare[] toFares1 = new AvailableFare[1];
            Fare fare = new Fare();
            AvailableFare availableFare = new AvailableFare();

            PaxFare[] fromPaxFares1 = new PaxFare[OriFlightInfo.Fares[0].PaxFares.Length];
            PaxFare[] toPaxFares1 = new PaxFare[NewFlightInfo.Fares[0].PaxFares.Length];
            PaxFare paxFare = new PaxFare();

            //load original segment 1 and 2
            if ((OriFlightInfo != null))
            {
                Segment = new Segment();
                var _with1 = Segment;
                _with1.State = BookingManager.MessageState.New;
                _with1.ActionStatusCode = "SS"; //added by diana 20140123

                _with1.DepartureStation = OriFlightInfo.DepartureStation;
                _with1.STD = DateTime.Parse(OriFlightInfo.STD.ToString());

                if (OriJourney.Segments.Length > 1)
                {
                    _with1.STA = DateTime.Parse(TransitOriFlightInfo.STA.ToString());
                    _with1.ArrivalStation = TransitOriFlightInfo.ArrivalStation;
                }
                else
                {
                    _with1.STA = DateTime.Parse(OriFlightInfo.STA.ToString());
                    _with1.ArrivalStation = OriFlightInfo.ArrivalStation;
                }


                FlightDesignator = new FlightDesignator();
                var _with2 = FlightDesignator;
                _with2.CarrierCode = OriFlightInfo.FlightDesignator.CarrierCode;
                _with2.FlightNumber = OriFlightInfo.FlightDesignator.FlightNumber;
                _with2.OpSuffix = OriFlightInfo.FlightDesignator.OpSuffix;
                _with1.FlightDesignator = FlightDesignator;

                leg = new Leg();
                var _with3 = leg;

                _with3.DepartureStation = OriFlightInfo.DepartureStation;
                _with3.STD = OriFlightInfo.STD;

                if (OriJourney.Segments.Length > 1)
                {
                    _with3.STA = TransitOriFlightInfo.STA;
                    _with3.ArrivalStation = TransitOriFlightInfo.ArrivalStation;
                }
                else
                {
                    _with3.STA = OriFlightInfo.STA;
                    _with3.ArrivalStation = OriFlightInfo.ArrivalStation;
                }

                _with3.FlightDesignator = FlightDesignator;
                _with3.State = BookingManager.MessageState.New;
                fromLegs1[0] = leg;
                _with1.Legs = fromLegs1;

                //fare = new Fare();
                //fare.State = BookingManager.MessageState.New;
                //fare.ClassOfService = OriFlightInfo.Fares[0].ClassOfService;
                //fare.CarrierCode = OriFlightInfo.Fares[0].CarrierCode;
                //fare.RuleNumber = OriFlightInfo.Fares[0].RuleNumber;
                //fare.FareBasisCode = OriFlightInfo.Fares[0].FareBasisCode;
                //fare.FareSequence = OriFlightInfo.Fares[0].FareSequence;
                //fare.FareClassOfService = OriFlightInfo.Fares[0].FareClassOfService;
                //fare.IsAllotmentMarketFare = OriFlightInfo.Fares[0].IsAllotmentMarketFare;
                //fare.FareApplicationType = OriFlightInfo.Fares[0].FareApplicationType;

                //for (int i = 0; i < toPaxFares1.Length; i++)
                //{
                //    paxFare = new PaxFare();
                //    paxFare.State = BookingManager.MessageState.New;
                //    paxFare.PaxType = OriFlightInfo.Fares[0].PaxFares[i].PaxType;
                //    BookingServiceCharge[] serviceCharges = new BookingServiceCharge[OriFlightInfo.Fares[0].PaxFares[i].ServiceCharges.Length];

                //    for (int y = 0; y < OriFlightInfo.Fares[0].PaxFares[i].ServiceCharges.Length; y++)
                //    {
                //        BookingServiceCharge bookingServiceCharge = new BookingServiceCharge();
                //        bookingServiceCharge.State = BookingManager.MessageState.New;
                //        bookingServiceCharge.ChargeType = OriFlightInfo.Fares[0].PaxFares[i].ServiceCharges[y].ChargeType;
                //        if (OriFlightInfo.Fares[0].PaxFares[i].ServiceCharges[y].ChargeCode.Length > 0) bookingServiceCharge.ChargeCode = OriFlightInfo.Fares[0].PaxFares[i].ServiceCharges[y].ChargeCode;
                //        if (OriFlightInfo.Fares[0].PaxFares[i].ServiceCharges[y].TicketCode.Length > 0) bookingServiceCharge.TicketCode = OriFlightInfo.Fares[0].PaxFares[i].ServiceCharges[y].TicketCode;
                //        bookingServiceCharge.CollectType = OriFlightInfo.Fares[0].PaxFares[i].ServiceCharges[y].CollectType;
                //        bookingServiceCharge.CurrencyCode = OriFlightInfo.Fares[0].PaxFares[i].ServiceCharges[y].CurrencyCode;
                //        bookingServiceCharge.Amount = OriFlightInfo.Fares[0].PaxFares[i].ServiceCharges[y].Amount;
                //        bookingServiceCharge.ForeignCurrencyCode = OriFlightInfo.Fares[0].PaxFares[i].ServiceCharges[y].ForeignCurrencyCode;
                //        bookingServiceCharge.ForeignAmount = OriFlightInfo.Fares[0].PaxFares[i].ServiceCharges[y].ForeignAmount;
                //        serviceCharges[y] = bookingServiceCharge;
                //    }
                //    paxFare.ServiceCharges = serviceCharges;
                //    fromPaxFares1[i] = paxFare;
                //}

                ////paxFare = OriFlightInfo.Fares[0].PaxFares[0];
                ////paxFare.State = BookingManager.MessageState.New;

                //fare.PaxFares = fromPaxFares1;
                //fare.FareSellKey = OriFlightInfo.Fares[0].FareSellKey;

                //fromFares1[0] = fare;
                //_with1.Fares = fromFares1;


                fromSegments[0] = Segment;

                //commented by diana 20140123
                //Segment = new Segment();
                //var _with4 = Segment;
                //_with4.State = BookingManager.MessageState.New;
                //_with4.DepartureStation = OriFlightInfo.DepartureStation2;
                //_with4.ArrivalStation = OriFlightInfo.ArrivalStation2;
                //_with4.STD = DateTime.Parse(OriFlightInfo.STD2);
                //_with4.STA = DateTime.Parse(OriFlightInfo.STA2);

                //FlightDesignator = new FlightDesignator();
                //var _with5 = FlightDesignator;
                //_with5.CarrierCode = OriFlightInfo.CarrierCode2;
                //_with5.FlightNumber = OriFlightInfo.FlightNumber2;
                //_with5.OpSuffix = OriFlightInfo.OpSuffix2;
                //_with4.FlightDesignator = FlightDesignator;
                //leg = new Leg();
                //var _with6 = leg;
                //_with6.ArrivalStation = OriFlightInfo.ArrivalStation2;
                //_with6.DepartureStation = OriFlightInfo.DepartureStation2;
                //_with6.FlightDesignator = FlightDesignator;
                //_with6.STA = OriFlightInfo.STA2;
                //_with6.STD = OriFlightInfo.STD2;
                //_with6.State = BookingManager.MessageState.New;
                //fromLegs2[0] = leg;
                //_with4.Legs = fromLegs2;
                //fromSegments[1] = Segment;

                fromJourney.Segments = fromSegments;
                fromJourney.JourneySellKey = OriJourney.JourneySellKey;
            }

            //load new segment 1 and 2
            if ((NewFlightInfo != null))
            {
                Segment = new Segment();
                var _with7 = Segment;
                _with7.State = BookingManager.MessageState.New;
                _with7.DepartureStation = NewFlightInfo.DepartureStation;
                _with7.STD = DateTime.Parse(NewFlightInfo.STD.ToString());

                if (NewJourney.Segments.Length > 1)
                {
                    _with7.STA = DateTime.Parse(TransitNewFlightInfo.STA.ToString());
                    _with7.ArrivalStation = TransitNewFlightInfo.ArrivalStation;
                }
                else
                {
                    _with7.STA = DateTime.Parse(NewFlightInfo.STA.ToString());
                    _with7.ArrivalStation = NewFlightInfo.ArrivalStation;
                }

                FlightDesignator = new FlightDesignator();
                var _with8 = FlightDesignator;
                _with8.CarrierCode = NewFlightInfo.FlightDesignator.CarrierCode;
                _with8.FlightNumber = NewFlightInfo.FlightDesignator.FlightNumber;
                _with8.OpSuffix = NewFlightInfo.FlightDesignator.OpSuffix;
                _with7.FlightDesignator = FlightDesignator;
                leg = new Leg();
                var _with9 = leg;

                _with9.DepartureStation = NewFlightInfo.DepartureStation;
                _with9.STD = NewFlightInfo.STD;

                if (NewJourney.Segments.Length > 1)
                {
                    _with9.STA = TransitNewFlightInfo.STA;
                    _with9.ArrivalStation = TransitNewFlightInfo.ArrivalStation;
                }
                else
                {
                    _with9.STA = NewFlightInfo.STA;
                    _with9.ArrivalStation = NewFlightInfo.ArrivalStation;
                }

                _with9.FlightDesignator = FlightDesignator;
                _with9.State = BookingManager.MessageState.New;
                toLegs1[0] = leg;
                _with7.Legs = toLegs1;

                //availableFare = new AvailableFare();
                //availableFare.State = BookingManager.MessageState.New;
                //availableFare.ClassOfService = NewFlightInfo.Fares[0].ClassOfService;
                //availableFare.CarrierCode = NewFlightInfo.Fares[0].CarrierCode;
                //availableFare.RuleNumber = NewFlightInfo.Fares[0].RuleNumber;
                //availableFare.FareBasisCode = NewFlightInfo.Fares[0].FareBasisCode;
                //availableFare.FareSequence = NewFlightInfo.Fares[0].FareSequence;
                //availableFare.FareClassOfService = NewFlightInfo.Fares[0].FareClassOfService;
                //availableFare.IsAllotmentMarketFare = NewFlightInfo.Fares[0].IsAllotmentMarketFare;
                //availableFare.FareApplicationType = NewFlightInfo.Fares[0].FareApplicationType;

                //for (int i = 0; i < toPaxFares1.Length; i++)
                //{
                //    paxFare = new PaxFare();
                //    paxFare.State = BookingManager.MessageState.New;
                //    paxFare.PaxType = NewFlightInfo.Fares[0].PaxFares[i].PaxType;
                //    BookingServiceCharge[] serviceCharges = new BookingServiceCharge[NewFlightInfo.Fares[0].PaxFares[i].ServiceCharges.Length];

                //    for (int y = 0; y < NewFlightInfo.Fares[0].PaxFares[i].ServiceCharges.Length; y++)
                //    {
                //        BookingServiceCharge bookingServiceCharge = new BookingServiceCharge();
                //        bookingServiceCharge.State = BookingManager.MessageState.New;
                //        bookingServiceCharge.ChargeType = NewFlightInfo.Fares[0].PaxFares[i].ServiceCharges[y].ChargeType;
                //        if (NewFlightInfo.Fares[0].PaxFares[i].ServiceCharges[y].ChargeCode.Length > 0) bookingServiceCharge.ChargeCode = NewFlightInfo.Fares[0].PaxFares[i].ServiceCharges[y].ChargeCode;
                //        if (NewFlightInfo.Fares[0].PaxFares[i].ServiceCharges[y].TicketCode.Length > 0) bookingServiceCharge.TicketCode = NewFlightInfo.Fares[0].PaxFares[i].ServiceCharges[y].TicketCode;
                //        bookingServiceCharge.CollectType = NewFlightInfo.Fares[0].PaxFares[i].ServiceCharges[y].CollectType;
                //        bookingServiceCharge.CurrencyCode = NewFlightInfo.Fares[0].PaxFares[i].ServiceCharges[y].CurrencyCode;
                //        bookingServiceCharge.Amount = NewFlightInfo.Fares[0].PaxFares[i].ServiceCharges[y].Amount;
                //        bookingServiceCharge.ForeignCurrencyCode = NewFlightInfo.Fares[0].PaxFares[i].ServiceCharges[y].ForeignCurrencyCode;
                //        bookingServiceCharge.ForeignAmount = NewFlightInfo.Fares[0].PaxFares[i].ServiceCharges[y].ForeignAmount;
                //        serviceCharges[y] = bookingServiceCharge;
                //    }
                //    paxFare.ServiceCharges = serviceCharges;
                //    toPaxFares1[i] = paxFare;
                //}

                ////paxFare = NewFlightInfo.Fares[0].PaxFares[0];
                ////paxFare.State = BookingManager.MessageState.New;

                //availableFare.PaxFares = toPaxFares1;
                //availableFare.FareSellKey = NewFlightInfo.Fares[0].FareSellKey;
                //availableFare.AvailableCount = 100;
                //availableFare.Status = BookingManager.ClassStatus.Active;

                //toFares1[0] = availableFare;

                //_with7.Fares = toFares1;

                toSegments[0] = Segment;

                //commented by diana 20140123
                //Segment = new Segment();
                //var _with10 = Segment;
                //_with10.State = BookingManager.MessageState.New;
                //_with10.DepartureStation = NewFlightInfo.DepartureStation2;
                //_with10.ArrivalStation = NewFlightInfo.ArrivalStation2;
                //_with10.STD = DateTime.Parse(NewFlightInfo.STD2);
                //_with10.STA = DateTime.Parse(NewFlightInfo.STA2);

                //FlightDesignator = new FlightDesignator();
                //var _with11 = FlightDesignator;
                //_with11.CarrierCode = NewFlightInfo.CarrierCode2;
                //_with11.FlightNumber = NewFlightInfo.FlightNumber2;
                //_with11.OpSuffix = NewFlightInfo.OpSuffix2;
                //_with10.FlightDesignator = FlightDesignator;
                //leg = new Leg();
                //var _with12 = leg;
                //_with12.ArrivalStation = NewFlightInfo.ArrivalStation2;
                //_with12.DepartureStation = NewFlightInfo.DepartureStation2;
                //_with12.FlightDesignator = FlightDesignator;
                //_with12.STA = NewFlightInfo.STA2;
                //_with12.STD = NewFlightInfo.STD2;
                //_with12.State = MessageState.New;
                //toLegs2[0] = leg;
                //_with10.Legs = toLegs2;
                //toSegments[1] = Segment;

                toJourney.Segments = toSegments;
                toJourney.JourneySellKey = NewJourney.JourneySellKey;
                JourneyList[0] = toJourney;
            }


            //fromJourney.Segments = New System.ComponentModel.BindingList(Of segmentlist)
            //fromJourney.Segments = new System.ComponentModel.BindingList<Segment>()
            //fromJourney.Segments.Add(new Segment());
            //fromJourney.Segments[0].State = MessageState.New;
            //fromJourney.Segments[0].DepartureStation = "SLC";
            //fromJourney.Segments[0].ArrivalStation = "JFK";
            //fromJourney.Segments[0].STD = DateTime.Parse("6/19/2008 2:00AM");
            //fromJourney.Segments[0].STA = DateTime.Parse("6/19/2008 5:00AM");
            //fromJourney.Segments[0].FlightDesignator = new FlightDesignator();
            //fromJourney.Segments[0].FlightDesignator.CarrierCode = "1L";
            //fromJourney.Segments[0].FlightDesignator.FlightNumber = "2290";
            //fromJourney.Segments[0].FlightDesignator.OpSuffix = " ";
            //fromJourney.Segments[0].Legs = new
            // System.ComponentModel.BindingList<Leg>();
            //fromJourney.Segments[0].Legs.Add(new Leg());
            //fromJourney.Segments[0].State = MessageState.New;
            //fromJourney.Segments[0].Legs[0].DepartureStation = "SLC";
            //fromJourney.Segments[0].Legs[0].ArrivalStation = "JFK";
            //fromJourney.Segments[0].Legs[0].FlightDesignator = 
            //new FlightDesignator();
            //fromJourney.Segments[0].Legs[0].FlightDesignator.CarrierCode = "1L";
            //fromJourney.Segments[0].Legs[0].FlightDesignator.FlightNumber = "2290";
            //fromJourney.Segments[0].Legs[0].FlightDesignator.OpSuffix = " ";
            //fromJourney.Segments[0].Legs[0].STD = DateTime.Parse("6/19/2008 2:00AM");
            //fromJourney.Segments[0].Legs[0].STA = DateTime.Parse("6/19/2008 5:00AM");

            //Create a Journey  object containing the information of the flight to move into. 


            //The ActionStatusCode is required to have a value (for example, SS to sell space or NN for need space).

            ////create the Flight to move into.
            //Journey toJourney = new Journey();
            //toJourney.Segments = new System.ComponentModel.BindingList<Segment>();
            //toJourney.Segments.Add(new Segment());
            //toJourney.Segments[0].State = MessageState.New;
            //toJourney.Segments[0].ActionStatusCode = "SS";
            //toJourney.Segments[0].DepartureStation = "SLC";
            //toJourney.Segments[0].ArrivalStation = "JFK";
            //toJourney.Segments[0].FlightDesignator = new FlightDesignator();
            //toJourney.Segments[0].FlightDesignator.CarrierCode = "1L";
            //toJourney.Segments[0].FlightDesignator.FlightNumber = "2389";
            //toJourney.Segments[0].FlightDesignator.OpSuffix = " ";
            //toJourney.Segments[0].STD = DateTime.Parse("2008-06-19T02:00:00");
            //toJourney.Segments[0].STA = DateTime.Parse("2008-06-19T07:00:00");
            //toJourney.Segments[0].Fares = 
            //new System.ComponentModel.BindingList<Fare>();
            //toJourney.Segments[0].Fares.Add(new Fare());
            //toJourney.Segments[0].Fares[0].CarrierCode = "1L";
            //toJourney.Segments[0].Fares[0].ClassOfService = "B";
            //toJourney.Segments[0].Fares[0].FareBasisCode = "BFARE";
            //toJourney.Segments[0].Fares[0].FareStatus = FareStatus.Default;
            //toJourney.Segments[0].Fares[0].FareApplicationType =
            // FareApplicationType.Route;
            //toJourney.Segments[0].Legs = 
            //new System.ComponentModel.BindingList<Leg>();
            //toJourney.Segments[0].Legs.Add(new Leg());
            //toJourney.Segments[0].Legs[0].ArrivalStation = "JFK";
            //toJourney.Segments[0].Legs[0].DepartureStation = "SLC";
            //toJourney.Segments[0].Legs[0].FlightDesignator = 
            //new FlightDesignator();
            //toJourney.Segments[0].Legs[0].FlightDesignator.CarrierCode = "1L";
            //toJourney.Segments[0].Legs[0].FlightDesignator.FlightNumber = "2389";
            //toJourney.Segments[0].Legs[0].FlightDesignator.OpSuffix = " ";
            //toJourney.Segments[0].Legs[0].STD = DateTime.Parse("2008-06-19T02:00:00");
            //toJourney.Segments[0].Legs[0].STA = DateTime.Parse("2008-06-19T07:00:00");

            //Create a MoveJourneyBookingsRequest object to be passed to the BookingManagerAPI. 

            //create and populate request
            MoveJourneyBookingsRequest request = new MoveJourneyBookingsRequest();
            MoveJourneyBookingsRequestData moveData = new MoveJourneyBookingsRequestData();

            try
            {
                //var _with13 = moveData;
                moveData.FromJourney = fromJourney;
                //.ToJourneys = New System.ComponentModel.BindingList(Of Journey)
                moveData.MovePassengerJourneyType = MovePassengerJourneyType.IROP;
                moveData.ToJourneys = JourneyList;
                string[] RecordLocator = new string[1];
                RecordLocator[0] = _RecordLocator;
                moveData.RecordLocators = RecordLocator;
                moveData.IgnorePNRsWithInvalidFromJourney = true;
                moveData.BookingComment = "Test move";

                request.MoveJourneyBookingsRequestData = moveData;
                //string Xml = GetXMLString(moveData);
                MoveJourneyBookingsResponseData response = new MoveJourneyBookingsResponseData();
                response = bookingAPI.MoveJourneyBookings(_ContractVersion, _Signature, moveData);
                //string Xml2 = GetXMLString(response);
                return true;
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString());
                ErrMsg = ex.ToString();
            }

            //MoveJourneyBookingsRequestData moveData = 
            //new MoveJourneyBookingsRequestData();
            //moveData;
            //moveData.ToJourneys = new System.ComponentModel.BindingList<Journey>();
            //moveData.ToJourneys.Add(toJourney);
            //moveData.MovePassengerJourneyType = MovePassengerJourneyType.IROP;
            //moveData.RecordLocators = 
            //new System.ComponentModel.BindingList<string>();
            //moveData.RecordLocators.Add("HZFI2L");
            //moveData.IgnorePNRsWithInvalidFromJourney = true;
            //moveData.BookingComment = "Sample Move";
            //request.MoveJourneyBookingsRequestData = moveData;
            //request.Signature = signature;

            //request.ContractVersion = 340;

            return false;
        }

        //        public bool MoveJourneyBySegment(AirAsia.Base.Flight_Info NewFlightInfo, AirAsia.Base.Flight_Info OriFlightInfo, ref string ErrMsg)
        //        {
        //    //Create an instance of BookingManagerClient
        //    AABookingManager.BookingManagerClient bookingAPI = new AABookingManager.BookingManagerClient();
        //    //Create the flight to move out from.
        //    AABookingManager.Journey fromJourney = new AABookingManager.Journey();
        //    AABookingManager.Journey toJourney = new AABookingManager.Journey();
        //    AABookingManager.Journey[] JourneyList = new AABookingManager.Journey[1];
        //    AABookingManager.FlightDesignator FlightDesignator = new AABookingManager.FlightDesignator();
        //    AABookingManager.Segment[] fromSegments = new AABookingManager.Segment[2];
        //    AABookingManager.Segment[] toSegments = new AABookingManager.Segment[2];
        //    AABookingManager.Segment Segment = new AABookingManager.Segment();
        //    AABookingManager.Leg[] fromLegs1 = new[];
        //    AABookingManager.Leg[] fromLegs2 = new AABookingManager.Leg[1];
        //    AABookingManager.Leg[] toLegs1 = new[];
        //    AABookingManager.Leg[] toLegs2 = new AABookingManager.Leg[1];
        //    AABookingManager.Leg leg = new AABookingManager.Leg();

        //    //load original segment 1 and 2
        //    if ((OriFlightInfo != null)) {
        //        Segment = new AABookingManager.Segment();
        //        var _with1 = Segment;
        //        _with1.State = AABookingManager.MessageState.New;
        //        _with1.DepartureStation = OriFlightInfo.DepartureStation;
        //        _with1.ArrivalStation = OriFlightInfo.ArrivalStation;
        //        _with1.STD = DateTime.Parse(OriFlightInfo.STD);
        //        _with1.STA = DateTime.Parse(OriFlightInfo.STA);

        //        FlightDesignator = new AABookingManager.FlightDesignator();
        //        var _with2 = FlightDesignator;
        //        _with2.CarrierCode = OriFlightInfo.CarrierCode;
        //        _with2.FlightNumber = OriFlightInfo.FlightNumber;
        //        _with2.OpSuffix = OriFlightInfo.OpSuffix;
        //        _with1.FlightDesignator = FlightDesignator;

        //        leg = new AABookingManager.Leg();
        //        var _with3 = leg;
        //        _with3.ArrivalStation = OriFlightInfo.ArrivalStation;
        //        _with3.DepartureStation = OriFlightInfo.DepartureStation;
        //        _with3.FlightDesignator = FlightDesignator;
        //        _with3.STA = OriFlightInfo.STA;
        //        _with3.STD = OriFlightInfo.STD;
        //        _with3.State = AABookingManager.MessageState.New;
        //        fromLegs1(0) = leg;
        //        _with1.Legs = fromLegs1;
        //        fromSegments(0) = Segment;

        //        Segment = new AABookingManager.Segment();
        //        var _with4 = Segment;
        //        _with4.State = AABookingManager.MessageState.New;
        //        _with4.DepartureStation = OriFlightInfo.DepartureStation2;
        //        _with4.ArrivalStation = OriFlightInfo.ArrivalStation2;
        //        _with4.STD = DateTime.Parse(OriFlightInfo.STD2);
        //        _with4.STA = DateTime.Parse(OriFlightInfo.STA2);

        //        FlightDesignator = new AABookingManager.FlightDesignator();
        //        var _with5 = FlightDesignator;
        //        _with5.CarrierCode = OriFlightInfo.CarrierCode2;
        //        _with5.FlightNumber = OriFlightInfo.FlightNumber2;
        //        _with5.OpSuffix = OriFlightInfo.OpSuffix2;
        //        _with4.FlightDesignator = FlightDesignator;
        //        leg = new AABookingManager.Leg();
        //        var _with6 = leg;
        //        _with6.ArrivalStation = OriFlightInfo.ArrivalStation2;
        //        _with6.DepartureStation = OriFlightInfo.DepartureStation2;
        //        _with6.FlightDesignator = FlightDesignator;
        //        _with6.STA = OriFlightInfo.STA2;
        //        _with6.STD = OriFlightInfo.STD2;
        //        _with6.State = AABookingManager.MessageState.New;
        //        fromLegs2(0) = leg;
        //        _with4.Legs = fromLegs2;
        //        fromSegments(1) = Segment;

        //        fromJourney.Segments = fromSegments;
        //    }

        //    //load new segment 1 and 2
        //    if ((NewFlightInfo != null)) {
        //        Segment = new AABookingManager.Segment();
        //        var _with7 = Segment;
        //        _with7.State = AABookingManager.MessageState.New;
        //        _with7.DepartureStation = NewFlightInfo.DepartureStation;
        //        _with7.ArrivalStation = NewFlightInfo.ArrivalStation;
        //        _with7.STD = DateTime.Parse(NewFlightInfo.STD);
        //        _with7.STA = DateTime.Parse(NewFlightInfo.STA);

        //        FlightDesignator = new AABookingManager.FlightDesignator();
        //        var _with8 = FlightDesignator;
        //        _with8.CarrierCode = NewFlightInfo.CarrierCode;
        //        _with8.FlightNumber = NewFlightInfo.FlightNumber;
        //        _with8.OpSuffix = NewFlightInfo.OpSuffix;
        //        _with7.FlightDesignator = FlightDesignator;
        //        leg = new AABookingManager.Leg();
        //        var _with9 = leg;
        //        _with9.ArrivalStation = NewFlightInfo.ArrivalStation;
        //        _with9.DepartureStation = NewFlightInfo.DepartureStation;
        //        _with9.FlightDesignator = FlightDesignator;
        //        _with9.STA = NewFlightInfo.STA;
        //        _with9.STD = NewFlightInfo.STD;
        //        _with9.State = AABookingManager.MessageState.New;
        //        toLegs1(0) = leg;
        //        _with7.Legs = toLegs1;
        //        toSegments(0) = Segment;

        //        Segment = new AABookingManager.Segment();
        //        var _with10 = Segment;
        //        _with10.State = AABookingManager.MessageState.New;
        //        _with10.DepartureStation = NewFlightInfo.DepartureStation2;
        //        _with10.ArrivalStation = NewFlightInfo.ArrivalStation2;
        //        _with10.STD = DateTime.Parse(NewFlightInfo.STD2);
        //        _with10.STA = DateTime.Parse(NewFlightInfo.STA2);

        //        FlightDesignator = new AABookingManager.FlightDesignator();
        //        var _with11 = FlightDesignator;
        //        _with11.CarrierCode = NewFlightInfo.CarrierCode2;
        //        _with11.FlightNumber = NewFlightInfo.FlightNumber2;
        //        _with11.OpSuffix = NewFlightInfo.OpSuffix2;
        //        _with10.FlightDesignator = FlightDesignator;
        //        leg = new AABookingManager.Leg();
        //        var _with12 = leg;
        //        _with12.ArrivalStation = NewFlightInfo.ArrivalStation2;
        //        _with12.DepartureStation = NewFlightInfo.DepartureStation2;
        //        _with12.FlightDesignator = FlightDesignator;
        //        _with12.STA = NewFlightInfo.STA2;
        //        _with12.STD = NewFlightInfo.STD2;
        //        _with12.State = AABookingManager.MessageState.New;
        //        toLegs2(0) = leg;
        //        _with10.Legs = toLegs2;
        //        toSegments(1) = Segment;

        //        toJourney.Segments = toSegments;
        //        JourneyList(0) = toJourney;
        //    }


        //    //fromJourney.Segments = New System.ComponentModel.BindingList(Of segmentlist)
        //    //fromJourney.Segments = new System.ComponentModel.BindingList<Segment>()
        //    //fromJourney.Segments.Add(new Segment());
        //    //fromJourney.Segments[0].State = MessageState.New;
        //    //fromJourney.Segments[0].DepartureStation = "SLC";
        //    //fromJourney.Segments[0].ArrivalStation = "JFK";
        //    //fromJourney.Segments[0].STD = DateTime.Parse("6/19/2008 2:00AM");
        //    //fromJourney.Segments[0].STA = DateTime.Parse("6/19/2008 5:00AM");
        //    //fromJourney.Segments[0].FlightDesignator = new FlightDesignator();
        //    //fromJourney.Segments[0].FlightDesignator.CarrierCode = "1L";
        //    //fromJourney.Segments[0].FlightDesignator.FlightNumber = "2290";
        //    //fromJourney.Segments[0].FlightDesignator.OpSuffix = " ";
        //    //fromJourney.Segments[0].Legs = new
        //    // System.ComponentModel.BindingList<Leg>();
        //    //fromJourney.Segments[0].Legs.Add(new Leg());
        //    //fromJourney.Segments[0].State = MessageState.New;
        //    //fromJourney.Segments[0].Legs[0].DepartureStation = "SLC";
        //    //fromJourney.Segments[0].Legs[0].ArrivalStation = "JFK";
        //    //fromJourney.Segments[0].Legs[0].FlightDesignator = 
        //    //new FlightDesignator();
        //    //fromJourney.Segments[0].Legs[0].FlightDesignator.CarrierCode = "1L";
        //    //fromJourney.Segments[0].Legs[0].FlightDesignator.FlightNumber = "2290";
        //    //fromJourney.Segments[0].Legs[0].FlightDesignator.OpSuffix = " ";
        //    //fromJourney.Segments[0].Legs[0].STD = DateTime.Parse("6/19/2008 2:00AM");
        //    //fromJourney.Segments[0].Legs[0].STA = DateTime.Parse("6/19/2008 5:00AM");

        //    //Create a Journey  object containing the information of the flight to move into. 


        //    //The ActionStatusCode is required to have a value (for example, SS to sell space or NN for need space).

        //    ////create the Flight to move into.
        //    //Journey toJourney = new Journey();
        //    //toJourney.Segments = new System.ComponentModel.BindingList<Segment>();
        //    //toJourney.Segments.Add(new Segment());
        //    //toJourney.Segments[0].State = MessageState.New;
        //    //toJourney.Segments[0].ActionStatusCode = "SS";
        //    //toJourney.Segments[0].DepartureStation = "SLC";
        //    //toJourney.Segments[0].ArrivalStation = "JFK";
        //    //toJourney.Segments[0].FlightDesignator = new FlightDesignator();
        //    //toJourney.Segments[0].FlightDesignator.CarrierCode = "1L";
        //    //toJourney.Segments[0].FlightDesignator.FlightNumber = "2389";
        //    //toJourney.Segments[0].FlightDesignator.OpSuffix = " ";
        //    //toJourney.Segments[0].STD = DateTime.Parse("2008-06-19T02:00:00");
        //    //toJourney.Segments[0].STA = DateTime.Parse("2008-06-19T07:00:00");
        //    //toJourney.Segments[0].Fares = 
        //    //new System.ComponentModel.BindingList<Fare>();
        //    //toJourney.Segments[0].Fares.Add(new Fare());
        //    //toJourney.Segments[0].Fares[0].CarrierCode = "1L";
        //    //toJourney.Segments[0].Fares[0].ClassOfService = "B";
        //    //toJourney.Segments[0].Fares[0].FareBasisCode = "BFARE";
        //    //toJourney.Segments[0].Fares[0].FareStatus = FareStatus.Default;
        //    //toJourney.Segments[0].Fares[0].FareApplicationType =
        //    // FareApplicationType.Route;
        //    //toJourney.Segments[0].Legs = 
        //    //new System.ComponentModel.BindingList<Leg>();
        //    //toJourney.Segments[0].Legs.Add(new Leg());
        //    //toJourney.Segments[0].Legs[0].ArrivalStation = "JFK";
        //    //toJourney.Segments[0].Legs[0].DepartureStation = "SLC";
        //    //toJourney.Segments[0].Legs[0].FlightDesignator = 
        //    //new FlightDesignator();
        //    //toJourney.Segments[0].Legs[0].FlightDesignator.CarrierCode = "1L";
        //    //toJourney.Segments[0].Legs[0].FlightDesignator.FlightNumber = "2389";
        //    //toJourney.Segments[0].Legs[0].FlightDesignator.OpSuffix = " ";
        //    //toJourney.Segments[0].Legs[0].STD = DateTime.Parse("2008-06-19T02:00:00");
        //    //toJourney.Segments[0].Legs[0].STA = DateTime.Parse("2008-06-19T07:00:00");

        //    //Create a MoveJourneyBookingsRequest object to be passed to the BookingManagerAPI. 

        //    //create and populate request
        //    AABookingManager.MoveJourneyBookingsRequest request = new AABookingManager.MoveJourneyBookingsRequest();
        //    AABookingManager.MoveJourneyBookingsRequestData moveData = new AABookingManager.MoveJourneyBookingsRequestData();

        //    try {
        //        var _with13 = moveData;
        //        _with13.FromJourney = fromJourney;
        //        //.ToJourneys = New System.ComponentModel.BindingList(Of AABookingManager.Journey)
        //        _with13.MovePassengerJourneyType = AABookingManager.MovePassengerJourneyType.IROP;
        //        _with13.ToJourneys = JourneyList;
        //        string[] RecordLocator = new string[2];
        //        RecordLocator[0] = _RecordLocator;
        //        _with13.RecordLocators = RecordLocator;
        //        _with13.IgnorePNRsWithInvalidFromJourney = true;
        //        _with13.BookingComment = "Test move";

        //        request.MoveJourneyBookingsRequestData = moveData;
        //        string Xml = GetXMLString(moveData);
        //        AABookingManager.MoveJourneyBookingsResponseData response = new AABookingManager.MoveJourneyBookingsResponseData();
        //        response = bookingAPI.MoveJourneyBookings(_ContractVersion, _Signature, moveData);
        //        string Xml2 = GetXMLString(response);
        //        return true;
        //    } catch (Exception ex) {
        //        sTraceLog(ex.ToString());
        //        ErrMsg = ex.ToString();
        //    }

        //    //MoveJourneyBookingsRequestData moveData = 
        //    //new MoveJourneyBookingsRequestData();
        //    //moveData;
        //    //moveData.ToJourneys = new System.ComponentModel.BindingList<Journey>();
        //    //moveData.ToJourneys.Add(toJourney);
        //    //moveData.MovePassengerJourneyType = MovePassengerJourneyType.IROP;
        //    //moveData.RecordLocators = 
        //    //new System.ComponentModel.BindingList<string>();
        //    //moveData.RecordLocators.Add("HZFI2L");
        //    //moveData.IgnorePNRsWithInvalidFromJourney = true;
        //    //moveData.BookingComment = "Sample Move";
        //    //request.MoveJourneyBookingsRequestData = moveData;
        //    //request.Signature = signature;

        //    //request.ContractVersion = 340;
        //}
        public string GetCreditFile(string memberID, string password)
        {

            return "";
        }

        public Boolean RemovePaymentfromBooking(Booking booking, string signature)
        {
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;

            if (booking.Payments.Length > 0)
            {
                for (int i = 0; i < booking.Payments.Length; i++)
                {
                    if (booking.Payments[i].Status == BookingPaymentStatus.Pending || booking.Payments[i].AuthorizationStatus == AuthorizationStatus.Pending)
                    {
                        IBookingManager bookingAPI = new BookingManagerClient();
                        RemovePaymentFromBookingRequest request = new RemovePaymentFromBookingRequest();
                        request.ContractVersion = this.ContractVersion;
                        request.Signature = signature;
                        request.RemovePaymentFromBookingRequestData = new RemovePaymentFromBookingRequestData();
                        request.RemovePaymentFromBookingRequestData.Payment = booking.Payments[i];
                        RemovePaymentFromBookingResponse resp;// = bookingAPI.RemovePaymentFromBooking(request);
                        using (profiler.Step("GetSingle_TRANSDTLBySellKey"))
                        {
                            resp = bookingAPI.RemovePaymentFromBooking(request);
                        }
                        if (resp.BookingUpdateResponseData.Success != null)
                            return true;
                        else return false;
                    }
                }
                return true;
            }
            else
                return true;
        }

        public Boolean AddAgencyPayment(decimal amount, string currency, string accNumber, string password, string signature, long accID, string payComment, ref string errMessage) //add jhn
        {
            #region prev
            //IBookingManager bookingAPI = new BookingManagerClient();
            //AddPaymentToBookingRequest request = new AddPaymentToBookingRequest();
            //request.addPaymentToBookingReqData = new AddPaymentToBookingRequestData();
            //request.addPaymentToBookingReqData.Status = BookingPaymentStatus.New;
            //request.addPaymentToBookingReqData.PaymentMethodType = RequestPaymentMethodType.AgencyAccount;
            //request.addPaymentToBookingReqData.PaymentMethodCode = "AG";
            //request.addPaymentToBookingReqData.QuotedCurrencyCode = "USD";
            //request.addPaymentToBookingReqData.QuotedAmount = 31.10M;
            //request.addPaymentToBookingReqData.AccountNumber = "88887777";
            //request.addPaymentToBookingReqData.PaymentText = "Sample AG Payment";
            //request.addPaymentToBookingReqData.AgencyAccount = new AgencyAccount();
            //request.addPaymentToBookingReqData.AgencyAccount.Password = String.Empty;
            //AddPaymentToBookingResponse resp = bookingAPI.AddPaymentToBooking(request);
            #endregion
            //throw (new Exception()); //for testing purpose

            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;

            IBookingManager bookingAPI = new BookingManagerClient();
            AddPaymentToBookingRequest request = new AddPaymentToBookingRequest();

            request.ContractVersion = this.ContractVersion;
            request.Signature = signature;// SessionManager._signature;  

            request.addPaymentToBookingReqData = new AddPaymentToBookingRequestData();
            request.addPaymentToBookingReqData.Status = BookingPaymentStatus.New;
            request.addPaymentToBookingReqData.PaymentMethodType = RequestPaymentMethodType.AgencyAccount;
            request.addPaymentToBookingReqData.PaymentMethodCode = "AG";
            request.addPaymentToBookingReqData.PaymentText = " Pay by AG" + payComment;
            request.addPaymentToBookingReqData.Installments = 1; //added by diana 20140430, installment should be 1 in order to proceed
            request.addPaymentToBookingReqData.QuotedCurrencyCode = currency;
            request.addPaymentToBookingReqData.QuotedAmount = amount;
            request.addPaymentToBookingReqData.AccountNumber = accNumber;
            request.addPaymentToBookingReqData.AccountNumberID = accID;
            request.addPaymentToBookingReqData.PaymentText = "AG Payment";
            request.addPaymentToBookingReqData.Installments = 1; //added by diana 20140430, installment should be 1 in order to proceed
            request.addPaymentToBookingReqData.AgencyAccount = new AgencyAccount();
            //request.addPaymentToBookingReqData.AgencyAccount.AccountID = 0;
            //request.addPaymentToBookingReqData.AgencyAccount.AccountTransactionID = 0;
            request.addPaymentToBookingReqData.AgencyAccount.Password = password;

            // string strxml = GetXMLString(request);
            AddPaymentToBookingResponse response;// = bookingAPI.AddPaymentToBooking(request);
            using (profiler.Step("lstbookDTLInfo"))
            {
                response = bookingAPI.AddPaymentToBooking(request);
            }
            //string strreturnxml = GetXMLString(response);
            if (response.BookingPaymentResponse.ValidationPayment.PaymentValidationErrors.Length > 0)
            {
                errMessage = response.BookingPaymentResponse.ValidationPayment.PaymentValidationErrors[0].ErrorDescription;
                return false;
            }
            else
            {
                errMessage = "";
                return true;
            }
        }

        public Boolean AddCreditShellPayment(decimal amount, string currency, string accNumber, string password, string signature, long accID, ref string errMessage) //add jhn
        {
            IBookingManager bookingAPI = new BookingManagerClient();
            AddPaymentToBookingRequest request = new AddPaymentToBookingRequest();

            request.ContractVersion = this.ContractVersion;
            request.Signature = signature;// SessionManager._signature;  

            request.addPaymentToBookingReqData = new AddPaymentToBookingRequestData();
            request.addPaymentToBookingReqData.Status = BookingPaymentStatus.New;
            request.addPaymentToBookingReqData.PaymentMethodType = RequestPaymentMethodType.CreditFile;
            request.addPaymentToBookingReqData.PaymentMethodCode = "CF";
            request.addPaymentToBookingReqData.QuotedCurrencyCode = currency;
            request.addPaymentToBookingReqData.QuotedAmount = amount;
            request.addPaymentToBookingReqData.AccountNumber = accNumber;
            request.addPaymentToBookingReqData.AccountNumberID = 0;
            request.addPaymentToBookingReqData.PaymentText = "Credit Shell Payment";
            request.addPaymentToBookingReqData.Installments = 1; //added by diana 20140430, installment should be 1 in order to proceed
            request.addPaymentToBookingReqData.CreditFile = new CreditFile();
            request.addPaymentToBookingReqData.CreditFile.AccountID = accID;

            AddPaymentToBookingResponse response = bookingAPI.AddPaymentToBooking(request);
            //string requeststring = GetXMLString(response);
            if (response.BookingPaymentResponse.ValidationPayment.PaymentValidationErrors.Length > 0)
            {
                errMessage = response.BookingPaymentResponse.ValidationPayment.PaymentValidationErrors[0].ErrorDescription;
                return false;
            }
            else
            {
                errMessage = "";
                return true;
            }
        }

        public Boolean AddPaymentToBooking(string currency, string signature, ref string errMessage) //add jhn
        {
            IBookingManager bookingAPI = new BookingManagerClient();
            AddPaymentToBookingRequest request = new AddPaymentToBookingRequest();

            request.ContractVersion = this.ContractVersion;
            request.Signature = signature;// SessionManager._signature;  

            request.addPaymentToBookingReqData = new AddPaymentToBookingRequestData();
            request.addPaymentToBookingReqData.Status = BookingPaymentStatus.New;
            //request.addPaymentToBookingReqData.PaymentMethodType = RequestPaymentMethodType.;
            request.addPaymentToBookingReqData.PaymentMethodCode = "CF";
            request.addPaymentToBookingReqData.QuotedCurrencyCode = currency;
            //request.addPaymentToBookingReqData.QuotedAmount = amount;
            //request.addPaymentToBookingReqData.AccountNumber = accNumber;
            request.addPaymentToBookingReqData.AccountNumberID = 0;
            //request.addPaymentToBookingReqData.PaymentText = "Credit Shell Payment";
            //request.addPaymentToBookingReqData.CreditFile = new CreditFile();
            //request.addPaymentToBookingReqData.CreditFile.AccountID = accID;

            AddPaymentToBookingResponse response = bookingAPI.AddPaymentToBooking(request);
            //string requeststring = GetXMLString(response);
            if (response.BookingPaymentResponse.ValidationPayment.PaymentValidationErrors.Length > 0)
            {
                errMessage = response.BookingPaymentResponse.ValidationPayment.PaymentValidationErrors[0].ErrorDescription;
                return false;
            }
            else
            {
                errMessage = "";
                return true;
            }
        }

        public GetAvailableCreditByReferenceResponse GetCreditByAccountReference(string accReference, string currency, string signature)
        {
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;

            IAccountManager accountAPI = new AccountManagerClient();
            GetAvailableCreditByReferenceRequest request = new GetAvailableCreditByReferenceRequest();
            GetAvailableCreditByReferenceRequestData requestData = new GetAvailableCreditByReferenceRequestData();

            requestData.AccountReference = accReference;
            requestData.CurrencyCode = currency;

            request.Signature = signature;

            request.ContractVersion = this.ContractVersion;
            request.GetAvailableCreditByReferenceReqData = requestData;
            GetAvailableCreditByReferenceResponse response;// = accountAPI.GetAvailableCreditByReference(request);
            using (profiler.Step("Navitaire:GetAvailableCreditByReference"))
            {
                response = accountAPI.GetAvailableCreditByReference(request);
            }

            return response;
        }

        //public FindAgentsResponse FindingAgent(string agentName, string SessionID)
        //{
        //    IAgentManager agentAPI = new AgentManagerClient();

        //    FindAgentsRequest findAgentRequest = new FindAgentsRequest();
        //    FindAgentRequestData findData = new FindAgentRequestData();
        //    findData.AgentName = new Navitaire.AgentManager.SearchString();
        //    findData.AgentName.SearchType = Navitaire.AgentManager.SearchType.StartsWith;
        //    findData.DomainCode = "ext";
        //    findData.AgentName.Value = agentName;
        //    findAgentRequest.FindAgentRequestData = findData;
        //    findAgentRequest.Signature = SessionID;

        //    FindAgentsResponse response = agentAPI.FindAgents(findAgentRequest);
        //    return response;
        //}

        public FindPersonsResponse FindPerson(string username, string sessionID)
        {

            IPersonManager personManager = new PersonManagerClient();
            FindPersonRequest findRequestData = new FindPersonRequest();

            findRequestData.AgentName = username;

            FindPersonsRequest findRequest = new FindPersonsRequest();
            findRequest.Signature = sessionID;
            findRequest.FindPersonRequestData = findRequestData;

            FindPersonsResponse response;
            response = personManager.FindPersons(findRequest);
            //string requeststring = GetXMLString(response);
            return response;

        }

        public string GetXMLString(object Obj)
        {
            Type[] types = new Type[] { typeof(AvailableFare) };

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Obj.GetType(), types);
            System.IO.StringWriter writer = new System.IO.StringWriter();
            x.Serialize(writer, Obj);

            return writer.ToString();
        }

        public void CancelSegment(string PNR, string SessionID, ref string errMessage)
        {
            IBookingManager bookingAPI = new BookingManagerClient();
            GetBookingRequest booking = new GetBookingRequest();
            booking.Signature = SessionID;
            booking.ContractVersion = this.ContractVersion;
            booking.GetBookingReqData = new GetBookingRequestData();
            booking.GetBookingReqData.GetBookingBy = GetBookingBy.RecordLocator;
            booking.GetBookingReqData.GetByRecordLocator = new GetByRecordLocator();
            booking.GetBookingReqData.GetByRecordLocator.RecordLocator = PNR;
            GetBookingResponse responseBooking = bookingAPI.GetBooking(booking);

            Journey journey = new Journey();
            journey.Segments = new Segment[1];
            journey.Segments[0] = new Segment();
            journey.Segments[0].DepartureStation = responseBooking.Booking.Journeys[0].Segments[0].DepartureStation;
            journey.Segments[0].ArrivalStation = responseBooking.Booking.Journeys[0].Segments[0].ArrivalStation;
            journey.Segments[0].STD = responseBooking.Booking.Journeys[0].Segments[0].STD;
            journey.Segments[0].STA = responseBooking.Booking.Journeys[0].Segments[0].STA;
            journey.Segments[0].FlightDesignator = new FlightDesignator();
            journey.Segments[0].FlightDesignator = responseBooking.Booking.Journeys[0].Segments[0].FlightDesignator;
            CancelRequest cancelRequest = new CancelRequest();
            cancelRequest.Signature = SessionID;

            cancelRequest.ContractVersion = this.ContractVersion;
            cancelRequest.CancelRequestData = new CancelRequestData();
            cancelRequest.CancelRequestData.CancelBy = CancelBy.Journey;
            cancelRequest.CancelRequestData.CancelJourney = new CancelJourney();
            cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest = new CancelJourneyRequest();
            int m = responseBooking.Booking.Journeys.Length;
            cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys = new Journey[m];
            for (int i = 0; i < m; i++)
            {
                cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i] = new Journey();
                cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].Segments = new Segment[1];
                cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].Segments[0] = new Segment();
                Segment sg = responseBooking.Booking.Journeys[i].Segments[0];
                cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].Segments[0].ActionStatusCode = sg.ActionStatusCode;
                cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].Segments[0].ArrivalStation = sg.ArrivalStation;
                cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].Segments[0].DepartureStation = sg.DepartureStation;
                cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].Segments[0].FlightDesignator = new FlightDesignator();
                cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].Segments[0].FlightDesignator.CarrierCode = sg.FlightDesignator.CarrierCode;
                cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].Segments[0].FlightDesignator.FlightNumber = sg.FlightDesignator.FlightNumber;
                cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].Segments[0].International = false;
                cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].Segments[0].SegmentSellKey = sg.SegmentSellKey;
                cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].Segments[0].STA = sg.STA;
                cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].Segments[0].STD = sg.STD;
                cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].Segments[0].State = ABS.Navitaire.BookingManager.MessageState.Clean;
                cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].State = ABS.Navitaire.BookingManager.MessageState.New;
                cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].NotForGeneralUse = false;
            }

            CancelResponse cancelResponse = bookingAPI.Cancel(cancelRequest);

            Thread.Sleep(1000);

            if (cancelResponse.BookingUpdateResponseData.Error != null)
            {
                errMessage = cancelResponse.BookingUpdateResponseData.Error.ErrorText;
            }
            else
            {
                //CancelFee(PNR, feeAmount, psCurrencyCode, SessionID, ref errMessage);
                // BookingCommit(PNR, SessionID, ref errMessage);
                CommitRequest commitRequest = new CommitRequest();
                commitRequest.BookingRequest = new CommitRequestData();
                commitRequest.BookingRequest.Booking = new Booking();
                commitRequest.BookingRequest.Booking.RecordLocator = responseBooking.Booking.RecordLocator;
                commitRequest.BookingRequest.Booking.CurrencyCode = responseBooking.Booking.CurrencyCode;
                commitRequest.BookingRequest.Booking.ReceivedBy = new ReceivedByInfo();
                commitRequest.BookingRequest.Booking.ReceivedBy.ReceivedBy = "AirAsia";
                commitRequest.Signature = SessionID;

                commitRequest.ContractVersion = this.ContractVersion;
                CommitResponse br = bookingAPI.Commit(commitRequest);


            }


        }

        public void SellSegment(string PNR, string SessionID, ref string errMessage)
        {
            IBookingManager bookingAPI = new BookingManagerClient();
            GetBookingRequest booking = new GetBookingRequest();
            booking.Signature = SessionID;
            booking.ContractVersion = this.ContractVersion;
            booking.GetBookingReqData = new GetBookingRequestData();
            booking.GetBookingReqData.GetBookingBy = GetBookingBy.RecordLocator;
            booking.GetBookingReqData.GetByRecordLocator = new GetByRecordLocator();
            booking.GetBookingReqData.GetByRecordLocator.RecordLocator = PNR;
            GetBookingResponse responseBooking = bookingAPI.GetBooking(booking);
            Booking bookingData = responseBooking.Booking;
            SellJourneyRequestData sjrd = new SellJourneyRequestData();
            sjrd.Passengers = bookingData.Passengers;

            BookingCommitRequest commitRequest = new BookingCommitRequest();
            // commitRequest.BookingCommitRequestData.
            //// sjrd.Journeys[0].Segments[0]=bookingData.Journeys[0].Segments[0];
            // Journey[] jy = bookingData.Journeys;
            // BookingCommitRequest bcr= new BookingCommitRequest();
            // BookingCommitResponse br = bookingAPI.BookingCommit(commitRequest);


            //int m = responseBooking.Booking.Journeys.Length;            


            //SellRequest sellRequest = new SellRequest();
            //sellRequest.SellRequestData = new SellRequestData();
            //sellRequest.SellRequestData.SellJourneyByKeyRequest = new SellJourneyByKeyRequest();
            ////sellRequest.SellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys = "tes";
            //CancelRequest cancelRequest = new CancelRequest();
            //cancelRequest.Signature = SessionID;

            //cancelRequest.ContractVersion = this.ContractVersion;
            //cancelRequest.CancelRequestData = new CancelRequestData();
            //cancelRequest.CancelRequestData.CancelBy = CancelBy.Journey;
            //cancelRequest.CancelRequestData.CancelJourney = new CancelJourney();
            //cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest = new CancelJourneyRequest();

            //cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys = new Journey[m];
            //for (int i = 0; i < m; i++)
            //{
            //    cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i] = new Journey();
            //    cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].Segments = new Segment[1];
            //    cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].Segments[0] = new Segment();
            //    Segment sg = responseBooking.Booking.Journeys[i].Segments[0];
            //    cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].Segments[0].ActionStatusCode = sg.ActionStatusCode;
            //    cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].Segments[0].ArrivalStation = sg.ArrivalStation;
            //    cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].Segments[0].DepartureStation = sg.DepartureStation;
            //    cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].Segments[0].FlightDesignator = new FlightDesignator();
            //    cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].Segments[0].FlightDesignator.CarrierCode = sg.FlightDesignator.CarrierCode;
            //    cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].Segments[0].FlightDesignator.FlightNumber = sg.FlightDesignator.FlightNumber;
            //    cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].Segments[0].International = false;
            //    cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].Segments[0].SegmentSellKey = sg.SegmentSellKey;
            //    cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].Segments[0].STA = sg.STA;
            //    cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].Segments[0].STD = sg.STD;
            //    cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].Segments[0].State = ABS.Navitaire.BookingManager.MessageState.Clean;
            //    cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].State = ABS.Navitaire.BookingManager.MessageState.New;
            //    cancelRequest.CancelRequestData.CancelJourney.CancelJourneyRequest.Journeys[i].NotForGeneralUse = false;
            //}

            //CancelResponse cancelResponse = bookingAPI.Cancel(cancelRequest);

            //Thread.Sleep(1000);

            //if (cancelResponse.BookingUpdateResponseData.Error != null)
            //{
            //    errMessage = cancelResponse.BookingUpdateResponseData.Error.ErrorText;
            //}
            //else
            //{
            //    //CancelFee(PNR, feeAmount, psCurrencyCode, SessionID, ref errMessage);
            //    // BookingCommit(PNR, SessionID, ref errMessage);
            //    CommitRequest commitRequest = new CommitRequest();
            //    commitRequest.BookingRequest = new CommitRequestData();
            //    commitRequest.BookingRequest.Booking = new Booking();
            //    commitRequest.BookingRequest.Booking.RecordLocator = responseBooking.Booking.RecordLocator;
            //    commitRequest.BookingRequest.Booking.CurrencyCode = responseBooking.Booking.CurrencyCode;
            //    commitRequest.BookingRequest.Booking.ReceivedBy = new ReceivedByInfo();
            //    commitRequest.BookingRequest.Booking.ReceivedBy.ReceivedBy = "AirAsia";
            //    commitRequest.Signature = SessionID;

            //    commitRequest.ContractVersion = this.ContractVersion;
            //    CommitResponse br = bookingAPI.Commit(commitRequest);


            //}


        }


        public void CancelJourney(string PNR, decimal feeAmount, string psCurrencyCode, string SessionID, ref string errMessage, Boolean commit = true, bool ReturnOnly = false)
        {
            IBookingManager bookingAPI = new BookingManagerClient();
            GetBookingRequest booking = new GetBookingRequest();
            CancelJourneyRequest cancelJour = new CancelJourneyRequest();
            CancelRequest cancelRequest = new CancelRequest();
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;
            try
            {
                booking.Signature = SessionID;
                booking.ContractVersion = this.ContractVersion;
                booking.GetBookingReqData = new GetBookingRequestData();
                booking.GetBookingReqData.GetBookingBy = GetBookingBy.RecordLocator;
                booking.GetBookingReqData.GetByRecordLocator = new GetByRecordLocator();
                booking.GetBookingReqData.GetByRecordLocator.RecordLocator = PNR;
                GetBookingResponse responseBooking;// = bookingAPI.GetBooking(booking);
                using (profiler.Step("GetBooking"))
                {
                    responseBooking = bookingAPI.GetBooking(booking);
                }
                //string xmlbooking = GetXMLString(responseBooking);
                if (responseBooking != null && responseBooking.Booking != null)
                {
                    if (responseBooking.Booking.BookingInfo.BookingStatus != BookingStatus.Confirmed && responseBooking.Booking.BookingInfo.PaidStatus != PaidStatus.UnderPaid)
                    {
                        errMessage = "Fail to cancel [" + PNR + "], Booking Status : " + responseBooking.Booking.BookingInfo.BookingStatus + " Payment Dataus : " + responseBooking.Booking.BookingInfo.PaidStatus;
                        return;
                    }
                }
                else
                {
                    errMessage = "Booking not found.";
                    return;
                }

                int m = responseBooking.Booking.Journeys.Length;

                //added by diana 20140424, check for cancelling return journey only
                int n = m;
                if (ReturnOnly == true)
                {
                    n = 1;
                }
                cancelJour.Journeys = new Journey[n];

                int j = 0;

                for (int i = 0; i < m; i++)
                {
                    if (ReturnOnly == true && i == 0) //added by diana 20140424, to cancel return only
                    {
                        continue;
                    }
                    cancelJour.Journeys[j] = new Journey();

                    if (responseBooking.Booking.Journeys[i].Segments.Length > 1)
                    {
                        cancelJour.Journeys[j].Segments = new Segment[2];
                    }
                    else
                    {
                        cancelJour.Journeys[j].Segments = new Segment[1];
                    }

                    cancelJour.Journeys[j].Segments[0] = new Segment();
                    Segment sg = responseBooking.Booking.Journeys[i].Segments[0];
                    cancelJour.Journeys[j].Segments[0].ActionStatusCode = sg.ActionStatusCode;
                    cancelJour.Journeys[j].Segments[0].ArrivalStation = sg.ArrivalStation;
                    cancelJour.Journeys[j].Segments[0].DepartureStation = sg.DepartureStation;
                    cancelJour.Journeys[j].Segments[0].FlightDesignator = new FlightDesignator();
                    cancelJour.Journeys[j].Segments[0].FlightDesignator.CarrierCode = sg.FlightDesignator.CarrierCode;
                    cancelJour.Journeys[j].Segments[0].FlightDesignator.FlightNumber = sg.FlightDesignator.FlightNumber;
                    cancelJour.Journeys[j].Segments[0].International = false;
                    cancelJour.Journeys[j].Segments[0].SegmentSellKey = sg.SegmentSellKey;
                    cancelJour.Journeys[j].Segments[0].STA = sg.STA;
                    cancelJour.Journeys[j].Segments[0].STD = sg.STD;
                    cancelJour.Journeys[j].Segments[0].State = ABS.Navitaire.BookingManager.MessageState.Clean;
                    // segment for connecting flight
                    if (responseBooking.Booking.Journeys[i].Segments.Length > 1)
                    {
                        cancelJour.Journeys[j].Segments[1] = new Segment();
                        sg = responseBooking.Booking.Journeys[i].Segments[1];
                        cancelJour.Journeys[j].Segments[1].ActionStatusCode = sg.ActionStatusCode;
                        cancelJour.Journeys[j].Segments[1].ArrivalStation = sg.ArrivalStation;
                        cancelJour.Journeys[j].Segments[1].DepartureStation = sg.DepartureStation;
                        cancelJour.Journeys[j].Segments[1].FlightDesignator = new FlightDesignator();
                        cancelJour.Journeys[j].Segments[1].FlightDesignator.CarrierCode = sg.FlightDesignator.CarrierCode;
                        cancelJour.Journeys[j].Segments[1].FlightDesignator.FlightNumber = sg.FlightDesignator.FlightNumber;
                        cancelJour.Journeys[j].Segments[1].International = false;
                        cancelJour.Journeys[j].Segments[1].SegmentSellKey = sg.SegmentSellKey;
                        cancelJour.Journeys[j].Segments[1].STA = sg.STA;
                        cancelJour.Journeys[j].Segments[1].STD = sg.STD;
                        cancelJour.Journeys[j].Segments[1].State = ABS.Navitaire.BookingManager.MessageState.Clean;
                    }
                    cancelJour.Journeys[j].State = ABS.Navitaire.BookingManager.MessageState.New;
                    cancelJour.Journeys[j].NotForGeneralUse = false;

                    j += 1; //increase index of cancel journey
                }

                cancelJour.WaivePenaltyFee = true;
                cancelJour.WaiveSpoilageFee = true;
                CancelJourney canceljou = new Navitaire.BookingManager.CancelJourney();
                canceljou.CancelJourneyRequest = cancelJour;

                cancelRequest.CancelRequestData = new CancelRequestData();
                cancelRequest.CancelRequestData.CancelBy = CancelBy.Journey;
                cancelRequest.CancelRequestData.CancelJourney = new CancelJourney();
                cancelRequest.CancelRequestData.CancelJourney = canceljou;

                cancelRequest.Signature = SessionID;
                cancelRequest.ContractVersion = this.ContractVersion;
                //string xml = GetXMLString(cancelRequest);
                CancelResponse response = bookingAPI.Cancel(cancelRequest);
                //string responsexml = GetXMLString(response);
                Thread.Sleep(1000);

                if (response.BookingUpdateResponseData.Error != null)
                {
                    errMessage = response.BookingUpdateResponseData.Error.ErrorText;
                }
                else
                {
                    //CancelFee(PNR, feeAmount, psCurrencyCode, SessionID, ref errMessage);
                   // xml = GetXMLString(response);

                    if (commit == true)
                    {
                        decimal BalanceDue = 0;
                        BalanceDue = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;

                        if (BalanceDue < 0)
                        {
                            if (AddPaymentToBooking(BalanceDue, psCurrencyCode, SessionID, ref errMessage) == false)
                            {
                                //errMessage = "Add payment failed.";
                                log.Info(this, errMessage);
                                return;
                            }
                        }

                        BookingCommit(PNR, SessionID, ref errMessage, psCurrencyCode, true, true);
                    }
                }

                if (errMessage != "")
                {
                    log.Info(this, errMessage);
                    return;
                }
                else
                {
                    return;
                }
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                log.Error(this, ex, errMessage);
                errMessage += ex.Message;
            }
            finally
            {
                bookingAPI = null;
                booking = null;
                cancelJour = null;
                cancelRequest = null;
            }
        }

        public void CancelJourneyBackup(string PNR, decimal feeAmount, string psCurrencyCode, string SessionID, ref string errMessage, Boolean commit = true)
        {
            IBookingManager bookingAPI = new BookingManagerClient();
            GetBookingRequest booking = new GetBookingRequest();
            CancelJourneyRequest cancelJour = new CancelJourneyRequest();
            CancelRequest cancelRequest = new CancelRequest();
            try
            {
                booking.Signature = SessionID;
                booking.ContractVersion = this.ContractVersion;
                booking.GetBookingReqData = new GetBookingRequestData();
                booking.GetBookingReqData.GetBookingBy = GetBookingBy.RecordLocator;
                booking.GetBookingReqData.GetByRecordLocator = new GetByRecordLocator();
                booking.GetBookingReqData.GetByRecordLocator.RecordLocator = PNR;
                GetBookingResponse responseBooking = bookingAPI.GetBooking(booking);
                //string xmlbooking = GetXMLString(responseBooking);
                if (responseBooking != null && responseBooking.Booking != null)
                {
                    if (responseBooking.Booking.BookingInfo.BookingStatus == BookingStatus.Closed)
                    {
                        return;
                    }
                }
                else
                {
                    errMessage = "Booking not found.";
                    return;
                }

                int m = responseBooking.Booking.Journeys.Length;
                cancelJour.Journeys = new Journey[m];
                for (int i = 0; i < m; i++)
                {
                    cancelJour.Journeys[i] = new Journey();

                    if (responseBooking.Booking.Journeys[i].Segments.Length > 1)
                    {
                        cancelJour.Journeys[i].Segments = new Segment[2];
                    }
                    else
                    {
                        cancelJour.Journeys[i].Segments = new Segment[1];
                    }
                    cancelJour.Journeys[i].Segments[0] = new Segment();
                    Segment sg = responseBooking.Booking.Journeys[i].Segments[0];
                    cancelJour.Journeys[i].Segments[0].ActionStatusCode = sg.ActionStatusCode;
                    cancelJour.Journeys[i].Segments[0].ArrivalStation = sg.ArrivalStation;
                    cancelJour.Journeys[i].Segments[0].DepartureStation = sg.DepartureStation;
                    cancelJour.Journeys[i].Segments[0].FlightDesignator = new FlightDesignator();
                    cancelJour.Journeys[i].Segments[0].FlightDesignator.CarrierCode = sg.FlightDesignator.CarrierCode;
                    cancelJour.Journeys[i].Segments[0].FlightDesignator.FlightNumber = sg.FlightDesignator.FlightNumber;
                    cancelJour.Journeys[i].Segments[0].International = false;
                    cancelJour.Journeys[i].Segments[0].SegmentSellKey = sg.SegmentSellKey;
                    cancelJour.Journeys[i].Segments[0].STA = sg.STA;
                    cancelJour.Journeys[i].Segments[0].STD = sg.STD;
                    cancelJour.Journeys[i].Segments[0].State = ABS.Navitaire.BookingManager.MessageState.Clean;
                    // segment for connecting flight
                    if (responseBooking.Booking.Journeys[i].Segments.Length > 1)
                    {
                        cancelJour.Journeys[i].Segments[1] = new Segment();
                        sg = responseBooking.Booking.Journeys[i].Segments[1];
                        cancelJour.Journeys[i].Segments[1].ActionStatusCode = sg.ActionStatusCode;
                        cancelJour.Journeys[i].Segments[1].ArrivalStation = sg.ArrivalStation;
                        cancelJour.Journeys[i].Segments[1].DepartureStation = sg.DepartureStation;
                        cancelJour.Journeys[i].Segments[1].FlightDesignator = new FlightDesignator();
                        cancelJour.Journeys[i].Segments[1].FlightDesignator.CarrierCode = sg.FlightDesignator.CarrierCode;
                        cancelJour.Journeys[i].Segments[1].FlightDesignator.FlightNumber = sg.FlightDesignator.FlightNumber;
                        cancelJour.Journeys[i].Segments[1].International = false;
                        cancelJour.Journeys[i].Segments[1].SegmentSellKey = sg.SegmentSellKey;
                        cancelJour.Journeys[i].Segments[1].STA = sg.STA;
                        cancelJour.Journeys[i].Segments[1].STD = sg.STD;
                        cancelJour.Journeys[i].Segments[1].State = ABS.Navitaire.BookingManager.MessageState.Clean;
                    }
                    cancelJour.Journeys[i].State = ABS.Navitaire.BookingManager.MessageState.New;
                    cancelJour.Journeys[i].NotForGeneralUse = false;
                }
                //cancelJour.WaivePenaltyFee = true;
                cancelJour.WaivePenaltyFee = true;
                cancelJour.WaiveSpoilageFee = true;
                CancelJourney canceljou = new Navitaire.BookingManager.CancelJourney();
                canceljou.CancelJourneyRequest = cancelJour;

                cancelRequest.CancelRequestData = new CancelRequestData();
                cancelRequest.CancelRequestData.CancelBy = CancelBy.Journey;
                cancelRequest.CancelRequestData.CancelJourney = new CancelJourney();
                cancelRequest.CancelRequestData.CancelJourney = canceljou;

                cancelRequest.Signature = SessionID;
                cancelRequest.ContractVersion = this.ContractVersion;
                //string xml = GetXMLString(cancelRequest);
                CancelResponse response = bookingAPI.Cancel(cancelRequest);
                //string responsexml = GetXMLString(response);
                Thread.Sleep(1000);

                if (response.BookingUpdateResponseData.Error != null)
                {
                    errMessage = response.BookingUpdateResponseData.Error.ErrorText;
                }
                else
                {
                    //CancelFee(PNR, feeAmount, psCurrencyCode, SessionID, ref errMessage);
                    //xml = GetXMLString(response);

                    if (commit == true)
                    {
                        decimal BalanceDue = 0;
                        BalanceDue = response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue;

                        if (BalanceDue < 0)
                        {
                            if (AddPaymentToBooking(BalanceDue, psCurrencyCode, SessionID, ref errMessage) == false)
                            {
                                //errMessage = "Add payment failed.";
                                log.Info(this, errMessage);
                                return;
                            }
                        }

                        BookingCommit(PNR, SessionID, ref errMessage, psCurrencyCode, true, true);
                    }
                }

                if (errMessage != "")
                {
                    log.Info(this, errMessage);
                    return;
                }
                else
                {
                    return;
                }
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                log.Error(this, ex, errMessage);
                errMessage += ex.Message;
            }
            finally
            {
                bookingAPI = null;
                booking = null;
                cancelJour = null;
                cancelRequest = null;
            }
        }

        public Boolean AddPaymentToBooking(decimal QuotedAmount, string QuotedCurrencyCode, string SessionID, ref string errmsg)
        {
            IBookingManager bookingAPI = new BookingManagerClient();
            AddPaymentToBookingRequest paymentBookingRequest = new AddPaymentToBookingRequest();
            AddPaymentToBookingResponse response = new AddPaymentToBookingResponse();

            try
            {
                paymentBookingRequest.addPaymentToBookingReqData = new AddPaymentToBookingRequestData();
                paymentBookingRequest.addPaymentToBookingReqData.PaymentFields = new PaymentField[2];
                paymentBookingRequest.addPaymentToBookingReqData.PaymentFields[0] = new PaymentField();
                paymentBookingRequest.addPaymentToBookingReqData.PaymentFields[0].FieldName = "DESC";
                paymentBookingRequest.addPaymentToBookingReqData.PaymentFields[0].FieldValue = "GROUP BOOKING WF";
                paymentBookingRequest.addPaymentToBookingReqData.PaymentFields[1] = new PaymentField();
                paymentBookingRequest.addPaymentToBookingReqData.PaymentFields[1].FieldName = "AMT";
                paymentBookingRequest.addPaymentToBookingReqData.PaymentFields[1].FieldValue = QuotedAmount.ToString();
                paymentBookingRequest.addPaymentToBookingReqData.PaymentMethodCode = "WF";
                paymentBookingRequest.addPaymentToBookingReqData.PaymentMethodType = RequestPaymentMethodType.PrePaid;
                paymentBookingRequest.addPaymentToBookingReqData.PaymentText = "GROUP BOOKING WF";
                paymentBookingRequest.addPaymentToBookingReqData.Installments = 1; //added by diana 20140430, installment should be 1 in order to proceed
                paymentBookingRequest.addPaymentToBookingReqData.QuotedAmount = QuotedAmount;
                paymentBookingRequest.addPaymentToBookingReqData.QuotedCurrencyCode = QuotedCurrencyCode;
                paymentBookingRequest.addPaymentToBookingReqData.ReferenceType = PaymentReferenceType.Session;
                paymentBookingRequest.Signature = SessionID;
                paymentBookingRequest.ContractVersion = this.ContractVersion;

                if (paymentBookingRequest != null)
                {
                    //string xml = GetXMLString(paymentBookingRequest);
                    response = bookingAPI.AddPaymentToBooking(paymentBookingRequest);
                    if (response.BookingPaymentResponse.ValidationPayment.PaymentValidationErrors.Length > 0)
                    {
                        if (response.BookingPaymentResponse.ValidationPayment.PaymentValidationErrors[0].ErrorDescription != "")
                        {
                            errmsg = response.BookingPaymentResponse.ValidationPayment.PaymentValidationErrors[0].ErrorDescription;
                        }
                        return false;
                    }
                   // xml = GetXMLString(response);
                }
                else
                {
                    return false;
                }
                return true;
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                errmsg = ex.Message;
                return false;
            }
            finally
            {
                bookingAPI = null;
                paymentBookingRequest = null;
                response = null;
            }
        }

        public void ClearJourney(string SessionID, ref string errMessage)
        {
            IBookingManager bookingAPI = new BookingManagerClient();

            ClearRequest clReq = new ClearRequest();
            clReq.ContractVersion = Convert.ToInt32(ConfigurationManager.AppSettings["ContractVersion"]);
            clReq.Signature = SessionID;
            try
            {
                ClearResponse response = bookingAPI.Clear(clReq);
                Thread.Sleep(1000);
                string asd = response.ToString();
                if (response.ToString() == null)
                {
                    errMessage = "ErrorClearJourney";
                }
                else
                {
                    //AgentLogout(SessionID);
                }
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                errMessage = ex.Message;
                if (errMessage.ToUpper().Contains("NO SUCH SESSION"))
                {
                    log.Warning(ex, ex.Message);
                }
                else
                {
                    log.Error(this, ex);
                }

            }
            finally
            {

            }

        }

        public void TopUpAGCredit(long accID, string signature)
        {
            IAccountManager accountAPI = new AccountManagerClient();
            CommitAccountTransactionsRequest transactionRequest = new CommitAccountTransactionsRequest();
            PostAccountTransactionRequest debitRequest = new PostAccountTransactionRequest();
            debitRequest.AccountID = accID;
            debitRequest.Amount = 1000000;
            debitRequest.CurrencyCode = "MYR";
            debitRequest.TransactionState = MessageTransactionState.Credit;
            transactionRequest.PostAccountTransactionRequest = debitRequest;
            transactionRequest.Signature = signature;
            transactionRequest.ContractVersion = this.ContractVersion;
            CommitAccountTransactionsResponse response = accountAPI.CommitAccountTransactions(transactionRequest);

        }

        public bool GetPostCommitResults(string sellSignature)
        {

            IBookingManager bookingAPI = new BookingManagerClient();
            GetPostCommitResultsRequest request = new GetPostCommitResultsRequest();
            request.Signature = sellSignature;
            request.ContractVersion = this.ContractVersion;
            GetPostCommitResultsResponse response = bookingAPI.GetPostCommitResults(request);

            return response.ContinuePulling;

        }

        public string GetLastPaymentStatusBySession(string sellSignature, ref string errMsg)
        {
            Thread.Sleep(2000);
            IBookingManager bookingAPI = new BookingManagerClient();
            GetBookingRequest request = new GetBookingRequest();
            try
            {
                //throw (new Exception()); //for testing purpose

                //declare for pending status
                int count = 0;
                if (HttpContext.Current.Session["AttemptCount"] != null)
                {
                    count = Convert.ToInt32(HttpContext.Current.Session["AttemptCount"]);
                }
                string attemptStr = "";

                //request.ContractVersion = this.ContractVersion;
                //request.Signature = AgentLogon();
                //request.GetBookingReqData = new GetBookingRequestData();
                //request.GetBookingReqData.GetBookingBy = GetBookingBy.RecordLocator;
                //request.GetBookingReqData.GetByRecordLocator = new GetByRecordLocator();
                //request.GetBookingReqData.GetByRecordLocator.RecordLocator = sellSignature;
                //GetBookingResponse response = bookingAPI.GetBooking(request);


                Booking booking = new Booking();
                booking = GetBookingFromState(sellSignature);

                int idxPayment = booking.Payments.Length;
                BookingPaymentStatus bookPayment = booking.Payments[idxPayment - 1].Status;
                string status = "";
                switch (bookPayment)
                {
                    case BookingPaymentStatus.Approved:
                        status = "success";
                        break;
                    case BookingPaymentStatus.Declined:
                        status = "failed";

                        //testing 

                        /*
                        //count += 1;                        
                        count = 0;
                        if (count > 3)
                        {
                            status = "failed";
                        }
                        else
                        {
                            HttpContext.Current.Session.Remove("AttemptCount");
                            HttpContext.Current.Session.Add("AttemptCount", count);
                            attemptStr = GetLastPaymentStatusByPNR(RecordLocator, ref errMsg);
                        }
                        */
                        break;
                    case BookingPaymentStatus.New:
                        status = "failed";
                        break;
                    case BookingPaymentStatus.Pending:
                        //double checking control for pending status

                        count += 1;
                        //if (count > 5)
                        //{
                        //    status = "failed";                        
                        //}
                        //else
                        //{
                        //    HttpContext.Current.Session.Remove("AttemptCount");
                        //    HttpContext.Current.Session.Add("AttemptCount", count);
                        //    GetLastPaymentStatusByPNR(RecordLocator,ref errMsg);
                        //}

                        //force to get true status 
                        status = GetPaymentStastusLoopBySignature(count, sellSignature, ref errMsg); //amended by diana 20140109 - insert into status

                        break;
                    case BookingPaymentStatus.PendingCustomerAction:
                        //status = "success";
                        status = GetPaymentStastusLoopBySignature(count, sellSignature, ref errMsg);
                        break;

                    case BookingPaymentStatus.Received:
                        //status = "success";
                        status = GetPaymentStastusLoopBySignature(count, sellSignature, ref errMsg);
                        break;
                    case BookingPaymentStatus.Unknown:
                        status = "failed";
                        break;
                    case BookingPaymentStatus.Unmapped:
                        status = "failed";
                        break;
                    default:
                        break;
                }

                //HttpContext.Current.Session.Remove("AttemptCount");

                if (status != "success")
                    errMsg = status;
                return status;

                //return request.Signature;
            }
            //amended by diana 20131210 - try catch to check for valid booking
            catch (TimeoutException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), sellSignature); return ""; }
            catch (OutOfMemoryException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), sellSignature); return ""; }
            catch (IndexOutOfRangeException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), sellSignature); return ""; }
            catch (ThreadInterruptedException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), sellSignature); return ""; }
            catch (NullReferenceException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), sellSignature); return ""; }
            catch (StackOverflowException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), sellSignature); return ""; }
            catch (ApplicationException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), sellSignature); return ""; }
            catch (Exception ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), sellSignature); return ""; }

        }

        private string GetPaymentStastusLoopBySignature(int count, string Signature, ref string errMsg)
        {
            HttpContext.Current.Session.Remove("AttemptCount");
            HttpContext.Current.Session.Add("AttemptCount", count);
            return GetLastPaymentStatusBySession(Signature, ref errMsg);
        }

        public string GetLastPaymentStatusByPNR(string RecordLocator, ref string errMsg, ref int paySeq)
        {
            Thread.Sleep(2000);
            IBookingManager bookingAPI = new BookingManagerClient();
            GetBookingRequest request = new GetBookingRequest();
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;
            try
            {
                //throw (new Exception()); //for testing purpose

                //declare for pending status
                int count = 0;
                if (HttpContext.Current.Session["AttemptCount"] != null)
                {
                    count = Convert.ToInt32(HttpContext.Current.Session["AttemptCount"]);
                }
                string attemptStr = "";

                request.ContractVersion = this.ContractVersion;
                request.Signature = AgentLogon();
                request.GetBookingReqData = new GetBookingRequestData();
                request.GetBookingReqData.GetBookingBy = GetBookingBy.RecordLocator;
                request.GetBookingReqData.GetByRecordLocator = new GetByRecordLocator();
                request.GetBookingReqData.GetByRecordLocator.RecordLocator = RecordLocator;
                GetBookingResponse response;// = bookingAPI.GetBooking(request);
                using (profiler.Step("GetBooking"))
                {
                    response = bookingAPI.GetBooking(request);
                }

                int idxPayment = response.Booking.Payments.Length;
                paySeq = idxPayment - 1; //added by diana 20140109 - get payment sequence
                BookingPaymentStatus bookPayment = response.Booking.Payments[idxPayment - 1].Status;

                //begin testing
                //Random rnd = new Random();
                //int num = rnd.Next(3);
                //if (num == 0)
                //    bookPayment = BookingPaymentStatus.Declined;
                //else
                //    bookPayment = BookingPaymentStatus.Approved;
                //end testing

                string status = "";
                switch (bookPayment)
                {
                    case BookingPaymentStatus.Approved:
                        status = "success";
                        break;
                    case BookingPaymentStatus.Declined:
                        status = "declined";
                        //testing 

                        /*
                        //count += 1;                        
                        count = 0;
                        if (count > 3)
                        {
                            status = "failed";
                        }
                        else
                        {
                            HttpContext.Current.Session.Remove("AttemptCount");
                            HttpContext.Current.Session.Add("AttemptCount", count);
                            attemptStr = GetLastPaymentStatusByPNR(RecordLocator, ref errMsg);
                        }
                        */
                        break;
                    case BookingPaymentStatus.New:
                        status = "failed";
                        break;
                    case BookingPaymentStatus.Pending:
                        //double checking control for pending status

                        count += 1;
                        //if (count > 5)
                        //{
                        //    status = "failed";                        
                        //}
                        //else
                        //{
                        //    HttpContext.Current.Session.Remove("AttemptCount");
                        //    HttpContext.Current.Session.Add("AttemptCount", count);
                        //    GetLastPaymentStatusByPNR(RecordLocator,ref errMsg);
                        //}

                        //force to get true status 
                        status = GetPaymentStastusLoop(count, RecordLocator, ref errMsg, ref paySeq); //amended by diana 20140109 - insert into status

                        break;
                    case BookingPaymentStatus.PendingCustomerAction:
                        //status = "success";
                        status = GetPaymentStastusLoop(count, RecordLocator, ref errMsg, ref paySeq);
                        break;
                    case BookingPaymentStatus.Received:
                        //status = "success";
                        status = GetPaymentStastusLoop(count, RecordLocator, ref errMsg, ref paySeq);
                        break;
                    case BookingPaymentStatus.Unknown:
                        status = "failed";
                        break;
                    case BookingPaymentStatus.Unmapped:
                        status = "failed";
                        break;
                    default:
                        break;
                }

                //HttpContext.Current.Session.Remove("AttemptCount");

                if (status != "success")
                    errMsg = status;
                return status;

                //return request.Signature;
            }
            //amended by diana 20131210 - try catch to check for valid booking
            catch (TimeoutException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), "", RecordLocator); return ""; }
            catch (OutOfMemoryException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), "", RecordLocator); return ""; }
            catch (IndexOutOfRangeException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), "", RecordLocator); return ""; }
            catch (ThreadInterruptedException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), "", RecordLocator); return ""; }
            catch (NullReferenceException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), "", RecordLocator); return ""; }
            catch (StackOverflowException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), "", RecordLocator); return ""; }
            catch (ApplicationException ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), "", RecordLocator); return ""; }
            catch (Exception ex) { log.Error(this, ex); CheckJourneyExist(ex.Message.ToString(), "", RecordLocator); return ""; }

        }

        private string GetPaymentStastusLoop(int count, string RecordLocator, ref string errMsg, ref int paySeq)
        {
            HttpContext.Current.Session.Remove("AttemptCount");
            HttpContext.Current.Session.Add("AttemptCount", count);
            return GetLastPaymentStatusByPNR(RecordLocator, ref errMsg, ref paySeq);
        }

        public void SellFee(string feeCode, string currency, string signature)
        {
            IBookingManager bookingAPI = new BookingManagerClient();
            SellRequest feeRequest = new SellRequest();

            feeRequest.SellRequestData = new SellRequestData();
            feeRequest.SellRequestData.SellBy = SellBy.Fee;
            feeRequest.SellRequestData.SellFee = new SellFee();
            feeRequest.SellRequestData.SellFee.SellFeeRequestData = new SellFeeRequestData();
            feeRequest.SellRequestData.SellFee.SellFeeRequestData.PassengerNumber = 0;
            feeRequest.SellRequestData.SellFee.SellFeeRequestData.FeeCode = feeCode;  //"SVCS";
            feeRequest.SellRequestData.SellFee.SellFeeRequestData.CollectedCurrencyCode = currency;//"USD";
            feeRequest.SellRequestData.SellFee.SellFeeRequestData.Note = "API added Service Fee";
            feeRequest.Signature = signature;

            feeRequest.ContractVersion = this.ContractVersion;
            SellResponse br = bookingAPI.Sell(feeRequest);


        }

        public Boolean addDirectDebitPayment(string TxnType, string ServiceID, string MerchantName, string MerchantTxnID, string OrderNumber, string OrderDesc, decimal TxnAmount, string CurrencyCode, string IssuingBank, string LanguageCode, string MerchantReturnURL, string MerchantSupportURL, string HashMethod, string HashValue)
        {
            //jhn test
            try
            {
                WebRequest request = WebRequest.Create(MerchantReturnURL);

                request.Method = "POST";

                string postData = "TxnType=" + TxnType + "&ServiceID=" + ServiceID + "&MerchantTxnID=" + MerchantTxnID + "&HashMethod=" + HashMethod + "&HashValue=" + HashValue;

                Byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                request.ContentType = "application/x-www-form-urlencoded";

                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();

                dataStream.Write(byteArray, 0, byteArray.Length);

                dataStream.Close();

                WebResponse response = request.GetResponse();

                dataStream = response.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();

                reader.Close();

                dataStream.Close();

                response.Close();

                return true;
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                return false;
            }


        }

        public GetExternalRatesListResponseData GetExternalRateList(string SessionID, int cnt = 0)
        {
            //Remark by ketee, increase performance, 20170916
            //Thread.Sleep(2000); //added by diana 20140205 - reconnect purpose

            //IUtilitiesManager UtilityMgr = new UtilitiesManagerClient();
            UtilitiesManagerClient UtilityMgr = new UtilitiesManagerClient();
            GetExternalRateListRequest request = new GetExternalRateListRequest();
            try
            {

                ////begin testing
                //Random rnd = new Random();
                //int num = rnd.Next(3);
                //if (num == 0)
                //    throw (new System.ServiceModel.ServiceActivationException());
                ////end testing

                request.ContractVersion = this.ContractVersion;
                if (SessionID == "")
                {
                    SessionID = AgentLogon();
                }
                request.Signature = SessionID;

                return UtilityMgr.GetExternalRateList(this.ContractVersion, SessionID);
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);

                //amended by diana 20140205 - try to get data for max of 5 attempts
                cnt += 1;
                if (cnt <= 5)
                {
                    UtilityMgr = null;
                    request = null;
                    return GetExternalRateList(SessionID, cnt);
                }
                else
                {
                    return null;
                }
                //end amended by diana 20140205 - try to get data for max of 5 attempts

            }
            finally
            {
                UtilityMgr = null;
                request = null;
            }
        }

        #region "SEAT"
        string _DepartXmlURL;

        public string DepartXmlUrl
        {
            get { return _DepartXmlURL; }
        }

        public SeatAvailabilityResponse GetSeatAvailability(string RecordLocator, int SegmentNumber, string SessionID, string CarrierCode, string FlightNumber,
            string OpSuffix, string DepartureStation, string ArrivalStation, DateTime STD, string PhysicalApplicationPath)
        {
            var profiler = MiniProfiler.Current;
            BookingManagerClient bookingAPI = new BookingManagerClient();
            GetBookingRequest GetBookingRequest = new GetBookingRequest();

            GetBookingRequestData GetBookingRequestData = new GetBookingRequestData();
            var _with1 = GetBookingRequestData;
            _with1.GetBookingBy = GetBookingBy.RecordLocator;
            _with1.GetByRecordLocator = new GetByRecordLocator();
            _with1.GetByRecordLocator.RecordLocator = RecordLocator;
            using (profiler.Step("API:GetBookingFromState"))
            {
                Booking Booking = bookingAPI.GetBookingFromState(this.ContractVersion, SessionID);
            }
            SeatAvailabilityRequest SeatAvailabilityRequest = new SeatAvailabilityRequest();
            var _with2 = SeatAvailabilityRequest;
            _with2.CarrierCode = CarrierCode;
            _with2.FlightNumber = FlightNumber;
            _with2.OpSuffix = OpSuffix;
            _with2.DepartureStation = DepartureStation;
            _with2.ArrivalStation = ArrivalStation;
            _with2.STD = STD;
            _with2.EnforceSeatGroupRestrictions = true;
            _with2.SeatGroup = 0;
            _with2.CompressProperties = false;
            _with2.SeatAssignmentMode = SeatAssignmentMode.PreSeatAssignment;
            _with2.IncludeSeatFees = true;
            _with2.CompressProperties = true;
            _with2.IncludePropertyLookup = true;

            //string str = GetXMLString(SeatAvailabilityRequest);
            using (profiler.Step("API:GetSeatAvailability"))
            {
                SeatAvailabilityResponse SeatAvailabilityResponse = bookingAPI.GetSeatAvailability(this.ContractVersion, SessionID, SeatAvailabilityRequest);

                //added by ketee, validate seat group, set seat group when normal seat, disable all hot seat
                _DepartXmlURL = GetXMLURL(SeatAvailabilityResponse, PhysicalApplicationPath);

                using (profiler.Step("GBS:ConvertSeatGroup"))
                {
                    ConvertSeatGroup(PhysicalApplicationPath, _DepartXmlURL);
                }
                //if (ValidateSeatGroup(RecordLocator, FlightType, SeatAvailabilityResponse) == "1")
                //{
                //    ConvertSeatGroup(_DepartXmlURL);
                //}
                return SeatAvailabilityResponse;
            }
        }

        private bool ConvertSeatGroup(string physicalApplicationPath, string xmlPath)
        {
            string xRep = "ReservedForPNR";
            XmlDocument xDoc = new XmlDocument();
            string xStr = null;

            try
            {
                xDoc.Load(physicalApplicationPath + "\\" + xmlPath);
                xStr = "/SeatAvailabilityResponse/EquipmentInfos/EquipmentInfo/Compartments/CompartmentInfo/Seats/SeatInfo/SeatAvailability";
                foreach (XmlNode xn in xDoc.SelectNodes(xStr))
                {
                    if (xn.InnerText == xRep)
                    {
                        xn.InnerText = "Open";
                    }
                }
                //xStr = "/SeatAvailabilityResponse/EquipmentInfos/EquipmentInfo/Compartments/CompartmentInfo/Seats/SeatInfo[SeatGroup='2']/SeatAvailability";
                //foreach (XmlNode xn in xDoc.SelectNodes(xStr))
                //{
                //    if (xn.InnerText != xRep)
                //    {
                //        xn.InnerText = xRep;
                //    }
                //}
                xDoc.Save(physicalApplicationPath + "\\" + xmlPath);
                return true;
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                return false;
            }
        }

        public BookingUpdateResponseData AssignSeats(Boolean unassignSeat, string SessionID, string DepartureStation, string ArrivalStation,
            string STD, int[] PassengerNumber, int[] PassengerID, string[] UnitDesignator, string[] CompartmentDesignator, string CarrierCode,
            string FlightNumber, string OpSuffix, string PNR = "", string manage = "")
        {
            DataTable dtPNRs = new DataTable();
            dtPNRs.Columns.Add("PNR");
            dtPNRs.Columns.Add("SessionID");
            dtPNRs.Columns.Add("Currency");
            BookingManagerClient bookingAPI = new BookingManagerClient();
            SeatSellRequest SeatSellRequest = new SeatSellRequest();
            AssignedSeatInfo responseAssignSeatInfo;
            GetBookingResponse bookingResp = new GetBookingResponse();
            //Dim SegmentSeatRequest As New AABookingManager.SegmentSeatRequest
            Booking Booking = new Booking();
            HttpContext.Current.Session["Commit"] = false;
            HttpContext.Current.Session["Forfeit"] = null;
            var profiler = MiniProfiler.Current;
            try
            {
                if (PNR != "")
                    using (profiler.Step("Navitaire:GetBooking"))
                    {
                        bookingResp = GetBookingByPNR(PNR, SessionID);
                    }
                else
                    Booking = bookingAPI.GetBookingFromState(this.ContractVersion, SessionID);

                //SeatAvailabilityResponse pAvailableSeatInfo = GetSeatAvailability(bookingResp.Booking.RecordLocator, 0, SessionID, bookingResp.Booking.Journeys[0].Segments[0].FlightDesignator.CarrierCode, bookingResp.Booking.Journeys[0].Segments[0].FlightDesignator.FlightNumber,
                //                bookingResp.Booking.Journeys[0].Segments[0].FlightDesignator.OpSuffix, resp.Booking.Journeys[0].Segments[0].DepartureStation, resp.Booking.Journeys[0].Segments[0].ArrivalStation, resp.Booking.Journeys[0].Segments[0].STD, Request.PhysicalApplicationPath);
                ABS.Navitaire.BookingManager.SeatSellRequest SeatSellRequests = new ABS.Navitaire.BookingManager.SeatSellRequest();
                SeatSellRequests.BlockType = UnitHoldType.Session;
                SeatSellRequests.SameSeatRequiredOnThruLegs = false;
                SeatSellRequests.AssignNoSeatIfAlreadyTaken = false;
                SeatSellRequests.AllowSeatSwappingInPNR = true;
                //SeatSellRequests.WaiveFee = unassignSeat;
                SeatSellRequests.WaiveFee = false;
                SeatSellRequests.ReplaceSpecificSeatRequest = true;
                SeatSellRequests.SeatAssignmentMode = ABS.Navitaire.BookingManager.SeatAssignmentMode.PreSeatAssignment;
                SeatSellRequests.IgnoreSeatSSRs = false;
                //if (pAvailableSeatInfo..AircraftCabins[0].AircraftSeats[0][0].SeatAvailability == SeatAvailability.Open)
                //{
                //    seats.SeatRequests[0].Seat = pAvailableSeatInfo.AircraftConfiguration.AircraftCabins[0].AircraftSeats[0][0].Seat;
                //}

                int NumberOfChange = PassengerID.Length;
                //List<ABS.Navitaire.BookingManager.SegmentSeatRequest> arrListSegmentSeatRequest = new List<ABS.Navitaire.BookingManager.SegmentSeatRequest>();
                ABS.Navitaire.BookingManager.SegmentSeatRequest[] arrSegmentSeatRequest = new ABS.Navitaire.BookingManager.SegmentSeatRequest[NumberOfChange];
                int I;
                for (I = 0; I < PassengerID.Length; I++)
                {
                    arrSegmentSeatRequest[I] = new ABS.Navitaire.BookingManager.SegmentSeatRequest();
                    arrSegmentSeatRequest[I].FlightDesignator = new BookingManager.FlightDesignator();
                    arrSegmentSeatRequest[I].FlightDesignator.CarrierCode = CarrierCode;
                    arrSegmentSeatRequest[I].FlightDesignator.FlightNumber = FlightNumber;
                    arrSegmentSeatRequest[I].FlightDesignator.OpSuffix = OpSuffix;
                    arrSegmentSeatRequest[I].FlightDesignator.ExtensionData = null;
                    arrSegmentSeatRequest[I].STD = Convert.ToDateTime(STD);
                    arrSegmentSeatRequest[I].DepartureStation = DepartureStation;
                    arrSegmentSeatRequest[I].ArrivalStation = ArrivalStation;
                    arrSegmentSeatRequest[I].UnitDesignator = UnitDesignator[I].ToString();   //'1D
                    arrSegmentSeatRequest[I].CompartmentDesignator = CompartmentDesignator[I].ToString(); //'F
                    short[] arrShort = new short[1];
                    arrSegmentSeatRequest[I].PassengerNumbers = arrShort;
                    arrSegmentSeatRequest[I].PassengerNumbers[0] = Convert.ToInt16(PassengerNumber[I]);

                    long[] arrLong = new long[1];
                    arrSegmentSeatRequest[I].PassengerIDs = arrLong;
                    arrSegmentSeatRequest[I].PassengerIDs[0] = Convert.ToInt32(PassengerID[I]);
                }

                SeatSellRequests.SegmentSeatRequests = arrSegmentSeatRequest;
                string msg = "";
                //string str = GetXMLString(SeatSellRequests);
                ABS.Navitaire.BookingManager.BookingUpdateResponseData resp = new ABS.Navitaire.BookingManager.BookingUpdateResponseData();
                if (unassignSeat == false)
                {

                    resp = bookingAPI.AssignSeats(this.ContractVersion, SessionID, SeatSellRequests, out responseAssignSeatInfo);

                    if (manage != "")
                    {
                        if (resp != null && resp.Success != null && resp.Success.PNRAmount.BalanceDue >= 0)
                        {

                            //Added by Tyas, if amount = 0 directly commit
                            using (profiler.Step("Navitaire:BookingCommit"))
                            {
                                BookingCommitChange(bookingResp.Booking.RecordLocator, SessionID, ref msg, bookingResp.Booking.CurrencyCode, true, true);
                            }
                            HttpContext.Current.Session["Commit"] = true;
                        }
                        else if (resp != null && resp.Success != null && resp.Success.PNRAmount.BalanceDue < 0)
                        {
                            if (AddPaymentToBooking(resp.Success.PNRAmount.BalanceDue, bookingResp.Booking.CurrencyCode, SessionID, ref msg))
                            {
                                HttpContext.Current.Session["Forfeit"] = true;
                                using (profiler.Step("Navitaire:BookingCommit"))
                                {
                                    BookingCommitChange(bookingResp.Booking.RecordLocator, SessionID, ref msg, bookingResp.Booking.CurrencyCode, true, true);
                                }
                                HttpContext.Current.Session["Commit"] = true;
                            }
                        }
                        else if (resp != null && resp.Warning != null)
                        {
                            HttpContext.Current.Session["failedassign"] = resp.Warning.WarningText;
                            return null;
                        }
                    }
                }
                else
                {
                    resp = bookingAPI.UnassignSeats(this.ContractVersion, SessionID, SeatSellRequests);
                    if (resp.Success.PNRAmount.BalanceDue != 0)
                    {
                        HttpContext.Current.Session["UnassignSeats"] = true;
                    }
                    //if (Booking.BookingSum.BalanceDue > 0)
                    //{
                    //    HttpContext.Current.Session["havebalance"] = true;
                    //}
                    if (manage != "")
                    {
                        HttpContext.Current.Session["BalanceDue"] = bookingResp.Booking.BookingSum.BalanceDue;
                        using (profiler.Step("Navitaire:BookingCommit"))
                        {
                            BookingCommitChange(bookingResp.Booking.RecordLocator, SessionID, ref msg, bookingResp.Booking.CurrencyCode, true, true);
                        }
                    }
                }
                if (resp != null)
                {
                    //string str2 = GetXMLString(resp);
                    return resp;
                }
                else
                    return null;

                //////var _with1 = SeatSellRequest;
                //////_with1.SameSeatRequiredOnThruLegs = false;
                //////_with1.AssignNoSeatIfAlreadyTaken = false;
                //////_with1.AllowSeatSwappingInPNR = true;
                //////_with1.WaiveFee = true;
                //////_with1.ReplaceSpecificSeatRequest = false;
                //////_with1.SeatAssignmentMode = SeatAssignmentMode.PreSeatAssignment;
                ////////.SeatAssignmentMode = VITBookingManager.SeatAssignmentMode.AutoDetermine
                //////_with1.IgnoreSeatSSRs = false;
                //////_with1.ReplaceSpecificSeatRequest = true;
                //////int NumberOfChange = PassengerID.Length;
                //////SegmentSeatRequest[] arrSegmentSeatRequest = new SegmentSeatRequest[NumberOfChange];
                //////int I = 0;
                //////for (I = 0; I <= NumberOfChange - 1; I++)
                //////{
                //////    arrSegmentSeatRequest[I] = new SegmentSeatRequest();
                //////    var _with2 = arrSegmentSeatRequest[I];
                //////    //.FlightDesignator = FlightDesignator
                //////    _with2.FlightDesignator = new FlightDesignator();
                //////    _with2.FlightDesignator.CarrierCode = CarrierCode;
                //////    _with2.FlightDesignator.FlightNumber = FlightNumber;
                //////    _with2.FlightDesignator.OpSuffix = OpSuffix;
                //////    _with2.FlightDesignator.ExtensionData = null;
                //////    _with2.STD = Convert.ToDateTime(STD);
                //////    _with2.DepartureStation = DepartureStation;
                //////    _with2.ArrivalStation = ArrivalStation;
                //////    _with2.UnitDesignator = UnitDesignator[I];
                //////    //1D
                //////    _with2.CompartmentDesignator = CompartmentDesignator[I];
                //////    //F

                //////    short[] arrShort = new short[3];
                //////    _with2.PassengerNumbers = arrShort;
                //////    _with2.PassengerNumbers[0] = (short)PassengerNumber[I];

                //////    long[] arrLong = new long[3];
                //////    _with2.PassengerIDs = arrLong;
                //////    _with2.PassengerIDs[0] = PassengerID[I];

                //////    //Dim arrPaxSeatPreference(2) As AABookingManager.PaxSeatPreference
                //////    //.PassengerSeatPreferences = arrPaxSeatPreference

                //////    //.PassengerSeatPreferences(0) = New AABookingManager.PaxSeatPreference
                //////    //.PassengerSeatPreferences(0).ActionStatusCode = "NN"
                //////    //.PassengerSeatPreferences(0).PropertyTypeCode = "WINDOW"
                //////    //.PassengerSeatPreferences(0).PropertyCode = "TRUE"

                //////    //.PassengerSeatPreferences(1) = New AABookingManager.PaxSeatPreference
                //////    //.PassengerSeatPreferences(1).ActionStatusCode = "NN"
                //////    //.PassengerSeatPreferences(1).PropertyTypeCode = "TCC"
                //////    //.PassengerSeatPreferences(1).PropertyCode = "Y"
                //////}
                ////////arrSegmentSeatRequest(0) = SegmentSeatRequest

                //////_with1.SegmentSeatRequests = arrSegmentSeatRequest;
                //////_with1.WaiveFee = false;
                ////////Dim AssignSeatsRequest As New AABookingManager.AssignSeatsRequest
                ////////With AssignSeatsRequest
                ////////    .SellSeatRequest = SeatSellRequest
                ////////    .SellSeatRequest.SegmentSeatRequests(0) = SegmentSeatRequest
                ////////    .Signature = _Signature
                ////////    _ContractVersion = _ContractVersion
                ////////End With

                //////CommitRequest CommitRequest = new CommitRequest();
                //////CommitRequestData CommitRequestData = new CommitRequestData();
                //////string seatsellrequestXml = null;
                //////seatsellrequestXml = GetXMLString(SeatSellRequest);
                //////BookingUpdateResponseData BookingUpdateResponseData;

                //////AssignedSeatInfo responseAssignSeatInfo;

                //////if (unassignSeat)
                //////    BookingUpdateResponseData = bookingAPI.UnassignSeats(this.ContractVersion, SessionID, SeatSellRequest);
                //////else
                //////    BookingUpdateResponseData = bookingAPI.AssignSeats(this.ContractVersion, SessionID, SeatSellRequest, out responseAssignSeatInfo);
                ////////Dim booking As AABookingManager.Booking
                ////////With booking

                ////////End With
                //////var _with3 = CommitRequestData;

                //////_with3.RestrictionOverride = true;
                //////_with3.WaiveSpoilageFee = true;
                //////_with3.WaivePenaltyFee = true;



                //////if ((BookingUpdateResponseData.Success != null))
                //////{
                //////    //CommitRequestData.Booking = booking
                //////    var _with4 = CommitRequest;
                //////    _with4.BookingRequest = CommitRequestData;

                //////    ReceivedByInfo ReceivedByInfo = new ReceivedByInfo();
                //////    ReceivedByInfo.ReceivedBy = "test user";
                //////    ReceivedByInfo.ExtensionData = BookingUpdateResponseData.ExtensionData;
                //////    ////_with4.BookingRequest.Booking = new Booking();
                //////    ////_with4.BookingRequest.Booking.ReceivedBy = ReceivedByInfo;
                //////    ////_with4.BookingRequest.Booking.RecordLocator = _RecordLocator;

                //////    ////_with4.Signature = _Signature;
                //////    ////_with4.ContractVersion = _ContractVersion;

                //////    return BookingUpdateResponseData;
                //////}
                //////////string strXml = null;
                //////////strXml = GetXMLString(CommitRequestData);
                //////////bookingAPI.Commit(_ContractVersion, _Signature, CommitRequestData);

                /////////* To request for a specific travel class fill in an element in
                //////// * the PassengerUnitPreferences list 
                //////// * where PropertyTypeCode = "TCC" and 
                //////// * with PropertyCode set to one of the following field values:
                //////// * PropertyCode =
                //////// *  F for first class
                //////// *  C for business class
                //////// *  Y for economy class
                //////// * 
                //////// * When populated, only seats in the equipment configuration
                //////// * that belong to the specified class will be selected in
                //////// * the seat assignment process
                //////// * */


                return null;

            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                return null;
            }
            finally
            {

            }

        }

        public BookingUpdateResponseData UnAssignSeats(Boolean unassignSeat, string SessionID, string DepartureStation, string ArrivalStation,
            string STD, int[] PassengerNumber, int[] PassengerID, string[] UnitDesignator, string[] CompartmentDesignator, string CarrierCode,
            string FlightNumber, string OpSuffix, string PNR = "", string manage = "")
        {
            DataTable dtPNRs = new DataTable();
            dtPNRs.Columns.Add("PNR");
            dtPNRs.Columns.Add("SessionID");
            dtPNRs.Columns.Add("Currency");
            BookingManagerClient bookingAPI = new BookingManagerClient();
            SeatSellRequest SeatSellRequest = new SeatSellRequest();
            AssignedSeatInfo responseAssignSeatInfo;
            GetBookingResponse bookingResp = new GetBookingResponse();
            //Dim SegmentSeatRequest As New AABookingManager.SegmentSeatRequest
            Booking Booking = new Booking();
            HttpContext.Current.Session["Commit"] = false;
            try
            {
                if (PassengerID.Length > 0)
                {
                    if (PNR != "")
                        bookingResp = GetBookingByPNR(PNR, SessionID);
                    else
                        Booking = bookingAPI.GetBookingFromState(this.ContractVersion, SessionID);

                    ABS.Navitaire.BookingManager.SeatSellRequest SeatSellRequests = new ABS.Navitaire.BookingManager.SeatSellRequest();
                    SeatSellRequests.BlockType = UnitHoldType.Session;
                    SeatSellRequests.SameSeatRequiredOnThruLegs = false;
                    SeatSellRequests.AssignNoSeatIfAlreadyTaken = false;
                    SeatSellRequests.AllowSeatSwappingInPNR = true;
                    SeatSellRequests.WaiveFee = unassignSeat;
                    SeatSellRequests.ReplaceSpecificSeatRequest = true;
                    SeatSellRequests.SeatAssignmentMode = ABS.Navitaire.BookingManager.SeatAssignmentMode.PreSeatAssignment;
                    SeatSellRequests.IgnoreSeatSSRs = false;

                    int NumberOfChange = PassengerID.Length;
                    //List<ABS.Navitaire.BookingManager.SegmentSeatRequest> arrListSegmentSeatRequest = new List<ABS.Navitaire.BookingManager.SegmentSeatRequest>();
                    ABS.Navitaire.BookingManager.SegmentSeatRequest[] arrSegmentSeatRequest = new ABS.Navitaire.BookingManager.SegmentSeatRequest[NumberOfChange];
                    int I;
                    for (I = 0; I < PassengerID.Length; I++)
                    {
                        arrSegmentSeatRequest[I] = new ABS.Navitaire.BookingManager.SegmentSeatRequest();
                        arrSegmentSeatRequest[I].FlightDesignator = new BookingManager.FlightDesignator();
                        arrSegmentSeatRequest[I].FlightDesignator.CarrierCode = CarrierCode;
                        arrSegmentSeatRequest[I].FlightDesignator.FlightNumber = FlightNumber;
                        arrSegmentSeatRequest[I].FlightDesignator.OpSuffix = OpSuffix;
                        arrSegmentSeatRequest[I].FlightDesignator.ExtensionData = null;
                        arrSegmentSeatRequest[I].STD = Convert.ToDateTime(STD);
                        arrSegmentSeatRequest[I].DepartureStation = DepartureStation;
                        arrSegmentSeatRequest[I].ArrivalStation = ArrivalStation;
                        arrSegmentSeatRequest[I].UnitDesignator = UnitDesignator[I].ToString();   //'1D
                        arrSegmentSeatRequest[I].CompartmentDesignator = CompartmentDesignator[I].ToString(); //'F
                        short[] arrShort = new short[1];
                        arrSegmentSeatRequest[I].PassengerNumbers = arrShort;
                        arrSegmentSeatRequest[I].PassengerNumbers[0] = Convert.ToInt16(PassengerNumber[I]);

                        long[] arrLong = new long[1];
                        arrSegmentSeatRequest[I].PassengerIDs = arrLong;
                        arrSegmentSeatRequest[I].PassengerIDs[0] = Convert.ToInt32(PassengerID[I]);
                    }

                    SeatSellRequests.SegmentSeatRequests = arrSegmentSeatRequest;

                    //string str = GetXMLString(SeatSellRequests);
                    ABS.Navitaire.BookingManager.BookingUpdateResponseData resp = new ABS.Navitaire.BookingManager.BookingUpdateResponseData();

                    resp = bookingAPI.UnassignSeats(this.ContractVersion, SessionID, SeatSellRequests);
                    if (resp != null)
                    {
                        HttpContext.Current.Session["Balance"] = bookingResp.Booking.BookingSum.BalanceDue;
                        //string str2 = GetXMLString(resp);
                        return resp;
                    }
                    else
                        return null;
                }
                else
                    return null;
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                return null;
            }
            finally
            {

            }

        }

        private string GetXMLURL(object Obj, string PhysicalApplicationPath)
        {
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Obj.GetType());
            System.IO.StringWriter writer = new System.IO.StringWriter();
            x.Serialize(writer, Obj);

            string RandomID = Guid.NewGuid().ToString();
            //Dim URL As String = RandomID & ".xml"

            //Dim ioFile As New IO.StreamWriter(URL)
            //ioFile.WriteLine(writer.ToString)
            //ioFile.Close()

            System.IO.StreamWriter logger = null;
            string logFilePath = "";



            try
            {
                logFilePath = PhysicalApplicationPath + "XML\\";
                if (System.IO.Directory.Exists(logFilePath) == false)
                    System.IO.Directory.CreateDirectory(logFilePath);

                logFilePath = logFilePath + RandomID + ".xml";
                System.IO.FileStream fs = new System.IO.FileStream(logFilePath, System.IO.FileMode.Create);
                logger = new System.IO.StreamWriter(fs, System.Text.Encoding.Unicode);
                //logger = System.IO.File.CreateText(logFilePath)
                logger.WriteLine(writer.ToString());

            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                throw;
            }
            finally
            {
                if ((logger != null))
                {
                    logger.Close();
                    logger.Dispose();
                }
            }

            return "Xml\\" + RandomID + ".xml";
        }
        #endregion

        public GetBookingPaymentsResponse GetBookingPayment(string RecordLocator)
        {
            try
            {
                IBookingManager bookingAPI = new BookingManagerClient(); GetBookingPaymentsRequest getPmtRequest = new
                GetBookingPaymentsRequest();
                getPmtRequest.GetBookingPaymentsReqData = new GetBookingPaymentsRequestData();
                getPmtRequest.Signature = this.Signature;
                getPmtRequest.GetBookingPaymentsReqData.GetByRecordLocator = new GetByRecordLocator();
                getPmtRequest.GetBookingPaymentsReqData.GetByRecordLocator.RecordLocator = RecordLocator;
                getPmtRequest.GetBookingPaymentsReqData.GetCurrentState = false;
                GetBookingPaymentsResponse payments = bookingAPI.GetBookingPayments(getPmtRequest);
                if (payments != null)
                    return payments;
                else
                    return null;
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                return null;
            }
        }

        public string DivideBooking(ArrayList PassengerNumbers, string RecordLocator, string ReceivedBy, string signature, ref string errMsg)
        {
            IBookingManager bookingAPI = new BookingManagerClient();
            DivideRequest divideRequest = new DivideRequest();

            try
            {

                divideRequest.DivideReqData = new DivideRequestData();
                divideRequest.DivideReqData.DivideAction = DivideAction.Divide;
                divideRequest.DivideReqData.SourceRecordLocator = RecordLocator;
                divideRequest.DivideReqData.AutoDividePayments = true;
                divideRequest.DivideReqData.PassengerNumbers = new short[PassengerNumbers.Count];
                int i = 0;
                for (i = 0; i < PassengerNumbers.Count; i++)
                {
                    divideRequest.DivideReqData.PassengerNumbers[i] = Convert.ToInt16(PassengerNumbers[i]);
                }
                //foreach (short rowpassenger in PassengerNumbers)
                //{

                //    i++;
                //}
                divideRequest.DivideReqData.OverrideRestrictions = true;
                divideRequest.DivideReqData.ReceivedBy = ReceivedBy;
                divideRequest.DivideReqData.AddComments = true;



                divideRequest.Signature = signature;
                divideRequest.ContractVersion = this.ContractVersion;
                //string xml = GetXMLString(divideRequest);
                DivideResponse divideResponse = bookingAPI.Divide(divideRequest);
                //string xmlresp = GetXMLString(divideResponse);
                if (divideResponse != null)
                {
                    return divideResponse.BookingUpdateResponseData.Success.RecordLocator;
                }
                else
                {
                    errMsg = "Divide unsuccessful";
                    return string.Empty;
                }
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                errMsg = ex.Message.ToString();
                return string.Empty;
            }
        }

        public SendItineraryResponse SendItineraryByPNR(string RecordLocator)
        {
            IBookingManager bookingAPI = new BookingManagerClient();
            SendItineraryRequest request = new SendItineraryRequest();
            try
            {
                request.ContractVersion = this.ContractVersion;
                //request.Signature = this.Signature;// SessionManager._signature;
                request.Signature = AgentLogon();
                HttpContext.Current.Session["Signature"] = request.Signature;
                request.RecordLocatorReqData = RecordLocator;
               // string str = GetXMLString(request);
                SendItineraryResponse response = bookingAPI.SendItinerary(request); //YDCF7P
                //AgentLogout(request.Signature);
                //str = GetXMLString(response);
                return response;
            }
           catch (Exception ex)
{
SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                return null;
            }
        }


        //public void ApplyPromotionCode(string signature)
        //{
        //    //ensure booking state contains a booking
        //    //create an instance of BookingManagerClient
        //    IBookingManager bookingAPI = new BookingManagerClient();
        //    ApplyPromotionRequest request = new ApplyPromotionRequest();
        //    ApplyPromotionRequestData requestData = new ApplyPromotionRequestData();
        //    requestData.PromotionCode = "10PRCT";
        //    requestData.SourcePointOfSale = new PointOfSale();
        //    requestData.SourcePointOfSale.AgentCode = "Booker";
        //    requestData.SourcePointOfSale.DomainCode = "DEF";
        //    requestData.SourcePointOfSale.LocationCode = "SLC";
        //    requestData.SourcePointOfSale.OrganizationCode = "1L";
        //    requestData.SourcePointOfSale.State = MessageState.New;
        //    request.ApplyPromotionReqData = requestData;
        //    request.ContractVersion = 340;
        //    request.Signature = signature;
        //    ApplyPromotionResponse response = bookingAPI.ApplyPromotion(request);
        //    if (response.BookingUpdateResponseData.Success != null)
        //    {
        //        Console.WriteLine(
        //            "Successful ApplyPromotion for RECLOC = {0}, \n\rTotal = {1:C}, Balance Due = {2:C} ",
        //            response.BookingUpdateResponseData.Success.RecordLocator.ToString(), 
        //            response.BookingUpdateResponseData.Success.PNRAmount.TotalCost, 
        //            response.BookingUpdateResponseData.Success.PNRAmount.BalanceDue
        //            );
        //    }
        //}
    }


}
