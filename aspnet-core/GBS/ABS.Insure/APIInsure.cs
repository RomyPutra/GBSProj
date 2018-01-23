using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using System.Threading;
using System.IO;
using System.Net;
using System.Configuration;
using System.Xml;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace ABS.GBS
{
    public class BK_Insure
    {
        protected System.String _RecordLocator;
        protected System.String _TransID;
        protected System.String _PassengerID;
        protected System.Int32 _Segment;
        protected System.Int32 _SeqNo;
        private System.Byte _TripMode;
        private System.String _CarrierCode;
        private System.String _FlightNo;
        private System.String _Origin;
        private System.String _Destination;
        private System.String _InsureCode;//added by romy, 20170811, insurance
        private System.Decimal _InsureAmt;//added by romy, 20170811, insurance
        private System.Int32 _IndicatorInsure;//added by romy, 20170811, insurance
        //private System.Decimal _TotalAmount;

        public System.String RecordLocator
        {
            get { return _RecordLocator; }
            set { _RecordLocator = value; }
        }
        public System.String TransID
        {
            get { return _TransID; }
            set { _TransID = value; }
        }
        public System.String PassengerID
        {
            get { return _PassengerID; }
            set { _PassengerID = value; }
        }
        public System.Int32 Segment
        {
            get { return _Segment; }
            set { _Segment = value; }
        }
        public System.Int32 SeqNo
        {
            get { return _SeqNo; }
            set { _SeqNo = value; }
        }
        public System.Byte TripMode
        {
            get { return _TripMode; }
            set { _TripMode = value; }
        }
        public System.String CarrierCode
        {
            get { return _CarrierCode; }
            set { _CarrierCode = value; }
        }
        public System.String FlightNo
        {
            get { return _FlightNo; }
            set { _FlightNo = value; }
        }
        public System.String Origin
        {
            get { return _Origin; }
            set { _Origin = value; }
        }
        public System.String Destination
        {
            get { return _Destination; }
            set { _Destination = value; }
        }
        public System.String InsureCode//added by romy, 20170811, insurance
        {
            get { return _InsureCode; }
            set { _InsureCode = value; }
        }
        public System.Decimal InsureAmt//added by romy, 20170811, insurance
        {
            get { return _InsureAmt; }
            set { _InsureAmt = value; }
        }
        public System.Int32 IndicatorInsure//added by romy, 20170811, insurance
        {
            get { return _IndicatorInsure; }
            set { _IndicatorInsure = value; }
        }
        //public System.Decimal TotalAmount
        //{
        //    get { return _TotalAmount; }
        //    set { _TotalAmount = value; }
        //}
    }
    public class InsureResponse
    {
        protected System.String _SeqNo;
        protected System.String _PNR;
        protected System.String _PassengerNumber;
        protected System.String _FeeCode;
        protected System.String _IsQualified;
        protected System.Decimal _Amount;

        public System.String SeqNo
        {
            get { return _SeqNo; }
            set { _SeqNo = value; }
        }
        public System.String PNR
        {
            get { return _PNR; }
            set { _PNR = value; }
        }
        public System.String PassengerNumber
        {
            get { return _PassengerNumber; }
            set { _PassengerNumber = value; }
        }
        public System.String FeeCode
        {
            get { return _FeeCode; }
            set { _FeeCode = value; }
        }
        
        public System.String IsQualified
        {
            get { return _IsQualified; }
            set { _IsQualified = value; }
        }
        public System.Decimal Amount
        {
            get { return _Amount; }
            set { _Amount = value; }
        }
    }
    public class GetPlan
    {
        private string GetXMLString(object Obj)
        {
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Obj.GetType());
            System.IO.StringWriter writer = new System.IO.StringWriter();
            x.Serialize(writer, Obj);

            return writer.ToString();
        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }
        //public TravelPlanResponse GetAvailablePlan(string Channel, string Currency, string CultureCode, ArrayList Passenger, ArrayList Flight)
        public PurchaseResponse Purchase(string PNR, string Status)
        {
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();

                //start, send request
                string APIUrl = (ConfigurationSettings.AppSettings["ConfirmPurchase"].ToString()+"?PNR=" + PNR + "&payStatus=" + Status);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(APIUrl);

                request.Method = "GET";
                //request.Host = "99.99.9.999";
                request.ContentType = "application/json";
                request.Accept = "*/*";
                request.Headers.Set("agentid", ConfigurationSettings.AppSettings["AgentID"].ToString());
                request.Headers.Set("token", ConfigurationSettings.AppSettings["Token"].ToString());
                // end, send request

                //start, retrieve response
                HttpWebResponse ws = (HttpWebResponse)request.GetResponse(); //retrieve response
                string strResponse = "";
                using (System.IO.StreamReader sreader = new System.IO.StreamReader(ws.GetResponseStream()))
                {
                    strResponse = sreader.ReadToEnd();
                }
                PurchaseResponse purchaseResponse = new PurchaseResponse();
                purchaseResponse = js.Deserialize<PurchaseResponse>(strResponse); //convert response json to object
                                                                                  //end, retrieve response

                //string TryXml = GetXMLString(strResponse);
                return purchaseResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TravelPlanResponse GetAvailablePlan(string Currency, List<IPassenger> IPassenger, List<Flight> Flight)
        {
            try
            {
                TravelPlanRequest travelPlan = new TravelPlanRequest();
                travelPlan.Username = ConfigurationSettings.AppSettings["Username"].ToString();
                travelPlan.Password = ConfigurationSettings.AppSettings["Password"].ToString();
                travelPlan.Channel = ConfigurationSettings.AppSettings["Channel"].ToString();
                travelPlan.Currency = Currency;
                travelPlan.CultureCode = ConfigurationSettings.AppSettings["CultureCode"].ToString();
                travelPlan.Flight = Flight;
                travelPlan.Passenger = IPassenger;

                JavaScriptSerializer js = new JavaScriptSerializer();
                

                //start, send request
                string strRequest = js.Serialize(travelPlan); //convert request object to json
                string APIUrl = (ConfigurationSettings.AppSettings["GetAvailablePlan"].ToString());
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(APIUrl);

                request.Method = "POST";
                //request.Host = "99.99.9.999";
                request.ContentType = "application/json";
                request.Accept = "*/*";
                request.Headers.Set("agentid", ConfigurationSettings.AppSettings["AgentID"].ToString());
                request.Headers.Set("token", ConfigurationSettings.AppSettings["Token"].ToString());

                using (System.IO.Stream s = request.GetRequestStream()) //create request and write the request
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(s))
                        sw.Write(strRequest);
                }
                // end, send request

                //start, retrieve response
                HttpWebResponse ws = (HttpWebResponse)request.GetResponse(); //retrieve response
                string strResponse = "";
                using (System.IO.StreamReader sreader = new System.IO.StreamReader(ws.GetResponseStream()))
                {
                    strResponse = sreader.ReadToEnd();
                }
                TravelPlanResponse travelPlanResponse = new TravelPlanResponse();
                travelPlanResponse = js.Deserialize<TravelPlanResponse>(strResponse); //convert response json to object
                                                                                      //end, retrieve response

                //string TryXml = GetXMLString(strResponse);                                
                return travelPlanResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}