using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Data;
using SEAL.Data;
using SEAL.Model;
using System.Collections;
using System.Web;
using ABS.Navitaire.BookingManager;
using System.Threading;
using System.Xml.Serialization;
using System.Data.SqlClient;
using ABS.GBS.Log;
using System.IO;

namespace ABS.Logic.GroupBooking
{
    public partial class BookingMGR : Component
    {
        #region Initialization

        public BookingMGR()
        {
            InitializeComponent();
        }

        public BookingMGR(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        #endregion

        #region GetBookingResponse

        #endregion
        public GetBookingResponse LookUpBooking(string SellKey, string PhysicalApplicationPath)
        {
            GetBookingResponse BookingResponse = new GetBookingResponse();
            try
            {
                BookingResponse = LoadBookingXML(PhysicalApplicationPath, SellKey);

                if ((BookingResponse != null && BookingResponse.Booking != null))
                {
                    if (SellKey != "")
                    {
                        //BookingResponse.BookingExtend.Select(a => a.DepartureStation == SellKey);
                        return BookingResponse;
                    }
                    else
                    {
                        return BookingResponse;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                BookingResponse = null;
            }
        }
        public GetBookingResponse LoadBookingXML(string XmlFilePath, string fileName) //XMLParam.FileName fileName)
        {
            //ACEGeneralManager generalManager = new ACEGeneralManager();
            //SHARED.ACELookupService.GetBookingResponse cityPair;
            GetBookingResponse result = null;
             XmlFilePath = XmlFilePath + "\\XML\\BOOKINGFROMSTATE\\" + fileName.ToString() + ".xml";

            if (File.Exists(XmlFilePath) == false)
            {
                return null;
            }
            else
            {
                StreamReader xmlStream = new StreamReader(XmlFilePath);
                XmlSerializer serializer = new XmlSerializer(typeof(GetBookingResponse));
                result = (GetBookingResponse)serializer.Deserialize(xmlStream);
            }

            if (result != null && result.Booking != null)
            {
                SaveXml(result, XMLParam.FileName.BOOKINGFROMSTATE, XmlFilePath);
                return result;
            }
            else
                return null;
        }

        public string SaveXml(object Obj, XMLParam.FileName fileName, string physicalPath)
        {
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Obj.GetType());
            System.IO.StringWriter writer = new System.IO.StringWriter();

            System.Xml.XmlWriter xmlWriter = null;
            //SerializeContainer((System.Xml.XmlWriter)xmlWriter, (Container)Obj);

            x.Serialize(writer, Obj);

            String RandomID = Guid.NewGuid().ToString();
            System.IO.StreamWriter logger = null;
            string logFilePath = string.Empty;
            string filePath = string.Empty;
            try
            {
                switch (fileName)
                {
                    case XMLParam.FileName.CITYPAIR:
                        logFilePath = physicalPath + "\\XML\\" + XMLParam.FileName.CITYPAIR.ToString();
                        filePath = logFilePath + "\\" + XMLParam.FileName.CITYPAIR.ToString() + ".xml";
                        break;
                    default:
                        logFilePath = @"E:\CXI\CXWP\ABS\ABS.GBS\ABS.GBS\ABS.GBS\XML\BOOKINGFROMSTATE"; // physicalPath + "\\XML\\" + fileName.ToString();
                        filePath = logFilePath + "\\" + fileName.ToString() + ".xml";
                        break;
                }

                //if (System.IO.Directory.Exists(logFilePath) == false) System.IO.Directory.CreateDirectory(logFilePath);
                string xmlstring = writer.ToString();
                System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create);
                logger = new System.IO.StreamWriter(fs, System.Text.Encoding.Unicode);
                //logger = System.IO.File.CreateText(logFilePath);
                logger.WriteLine(writer.ToString());
                //System.Threading.Thread.Sleep(500);
                return filePath;
            }
            catch (Exception ex)
            {
                //SystemLog.Notifier.Notify(ex);


                //log.Error(this, ex);
                //Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                //BasePage.StatusLog = Enums.StatusLog.ERRORLOG;
                //BasePage.MessageLog = ex.Message;
                return "";
            }
            finally
            {
                if (logger != null)
                {
                    logger.Close();
                    logger.Dispose();
                }
                //x = null;
                //writer.Dispose();
            }
        }

        #region BookingResponse Entity
        [Serializable]
        public class GetBookingResponse
        {
            //[XmlAttribute(Namespace = "http://schemas.navitaire.com/WebServices/ServiceContracts/BookingService")]
            public Booking Booking { get; set; }
        }

        [Serializable]
        public class Booking
        {
            [XmlElement("OtherServiceInfoList")]
            public string OtherServiceInfoList { get; set; }
            [XmlElement("State")]
            public string State { get; set; }

            [XmlElement("RecordLocator")]
            public string RecordLocator { get; set; }

            [XmlElement("CurrencyCode")]
            public string CurrencyCode { get; set; }

            [XmlElement("PaxCount")]
            public string PaxCount { get; set; }
            [XmlElement("SystemCode")]
            public string SystemCode { get; set; }

            [XmlElement("BookingID")]
            public string BookingID { get; set; }

            [XmlElement("BookingParentID")]
            public string BookingParentID { get; set; }

            [XmlElement("ParentRecordLocator")]
            public string ParentRecordLocator { get; set; }

            [XmlElement("BookingChangeCode")]
            public string BookingChangeCode { get; set; }

            [XmlElement("GroupName")]
            public string GroupName { get; set; }

            [XmlElement("BookingInfo")]
            public BookingInfo BookingInfo { get; set; }

            [XmlElement("POS")]
            public POS POS { get; set; }

            [XmlElement("SourcePOS")]
            public SourcePOS SourcePOS { get; set; }

            [XmlElement("TypeOfSale")]
            public TypeOfSale TypeOfSale { get; set; }

            [XmlElement("BookingHold")]
            public BookingHold BookingHold { get; set; }

            [XmlElement("BookingSum")]
            public BookingSum BookingSum { get; set; }

            [XmlElement("ReceivedBy")]
            public ReceivedBy ReceivedBy { get; set; }

            [XmlElement("RecordLocators")]
            public RecordLocators RecordLocators { get; set; }

            [XmlElement("Passengers")]
            public Passengers Passengers { get; set; }

            [XmlElement("Journeys")]
            public Journeys Journeys { get; set; }

            [XmlElement("BookingComments")]
            public BookingComments BookingComments { get; set; }

            [XmlElement("BookingQueueInfos")]
            public string BookingQueueInfos { get; set; }

            [XmlElement("BookingContacts")]
            public BookingContacts BookingContacts { get; set; }

            [XmlElement("Payments")]
            public Payments Payments { get; set; }

            [XmlElement("BookingComponents")]
            public string BookingComponents { get; set; }

            [XmlElement("NumericRecordLocator")]
            public string NumericRecordLocator { get; set; }
        }

        public class BookingInfo
        {
            [XmlElement("State")]
            public string State { get; set; }

            [XmlElement("BookingStatus")]
            public string BookingStatus { get; set; }
            [XmlElement("BookingType")]
            public string BookingType { get; set; }

            [XmlElement("ChannelType")]
            public string Channel { get; set; }
            [XmlElement("CreatedDate")]
            public string CreatedDate { get; set; }
            [XmlElement("ExpiredDate")]
            public string ExpiredDate { get; set; }
            [XmlElement("ModifiedDate")]
            public string ModifiedDate { get; set; }
            [XmlElement("PriceStatus")]
            public string PriceStatus { get; set; }
            [XmlElement("ProfileStatus")]
            public string ProfileStatus { get; set; }
            [XmlElement("ChangeAllowed")]
            public string ChangeAllowed { get; set; }
            [XmlElement("CreatedAgentID")]
            public string CreatedAgentID { get; set; }
            [XmlElement("ModifiedAgentID")]
            public string ModifiedAgentID { get; set; }
            [XmlElement("BookingDate")]
            public string BookingDate { get; set; }
            [XmlElement("OwningCarrierCode")]
            public string OwningCarrierCode { get; set; }

            [XmlElement("PaidStatus")]
            public string PaidStatus { get; set; }
        }
        public class POS
        {
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("AgentCode")]
            public string AgentCode { get; set; }
            [XmlElement("OrganizationCode")]
            public string OrganizationCode { get; set; }
            [XmlElement("DomainCode")]
            public string DomainCode { get; set; }
            [XmlElement("LocationCode")]
            public string LocationCode { get; set; }
        }
        public class SourcePOS
        {
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("AgentCode")]
            public string AgentCode { get; set; }
            [XmlElement("OrganizationCode")]
            public string OrganizationCode { get; set; }
            [XmlElement("DomainCode")]
            public string DomainCode { get; set; }
            [XmlElement("LocationCode")]
            public string LocationCode { get; set; }
        }
        public class TypeOfSales
        {
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("PaxResidentCountry")]
            public string PaxResidentCountry { get; set; }
            [XmlElement("PromotionCode")]
            public string PromotionCode { get; set; }
            [XmlElement("FareTypes")]
            public string FareTypes { get; set; }
        }
        public class BookingHold
        {
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("HoldDateTime")]
            public string HoldDateTime { get; set; }
        }
        public class BookingSum
        {
            [XmlElement("BalanceDue")]
            public string BalanceDue { get; set; }
            [XmlElement("AuthorizedBalanceDue")]
            public string AuthorizedBalanceDue { get; set; }
            [XmlElement("SegmentCount")]
            public string SegmentCount { get; set; }
            [XmlElement("PassiveSegmentCount")]
            public string PassiveSegmentCount { get; set; }
            [XmlElement("TotalCost")]
            public string TotalCost { get; set; }
            [XmlElement("PointsBalanceDue")]
            public string PointsBalanceDue { get; set; }
            [XmlElement("TotalPointCost")]
            public string TotalPointCost { get; set; }
            [XmlElement("AlternateCurrencyCode")]
            public string AlternateCurrencyCode { get; set; }
            [XmlElement("AlternateCurrencyBalanceDue")]
            public string AlternateCurrencyBalanceDue { get; set; }
        }
        public class ReceivedBy
        {
            [XmlElement("State")]
            public string State { get; set; }
            //[XmlElement("ReceivedBy")]
            //public string Received1By { get; set; }
            [XmlElement("ReceivedReference")]
            public string ReceivedReference { get; set; }
            [XmlElement("ReferralCode")]
            public string ReferralCode { get; set; }
            [XmlElement("LatestReceivedBy")]
            public string LatestReceivedBy { get; set; }
            [XmlElement("LatestReceivedReference")]
            public string LatestReceivedReference { get; set; }
        }
        public class RecordLocators
        {
            [XmlElement("RecordLocator")]
            public RecordLocator RecordLocator { get; set; }
        }
        public class RecordLocator
        {
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("SystemDomainCode")]
            public string SystemDomainCode { get; set; }
            [XmlElement("SystemCode")]
            public string SystemCode { get; set; }
            [XmlElement("RecordCode")]
            public string RecordCode { get; set; }
            [XmlElement("InteractionPurpose")]
            public string InteractionPurpose { get; set; }
            [XmlElement("HostedCarrierCode")]
            public string HostedCarrierCode { get; set; }
        }
        public class RecordLocatorsPassengers
        {
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("SystemDomainCode")]
            public string SystemDomainCode { get; set; }
            [XmlElement("SystemCode")]
            public string SystemCode { get; set; }
            [XmlElement("RecordCode")]
            public string RecordCode { get; set; }
            [XmlElement("InteractionPurpose")]
            public string InteractionPurpose { get; set; }
            [XmlElement("HostedCarrierCode")]
            public string HostedCarrierCode { get; set; }
        }
        public class Passengers
        {
            [XmlElement("Passenger")]
            public Passenger[] Passenger { get; set; }
        }
        public class Passenger
        {
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("PassengerPrograms")]
            public string PassengerPrograms { get; set; }
            [XmlElement("CustomerNumber")]
            public string CustomerNumber { get; set; }
            [XmlElement("PassengerNumber")]
            public string PassengerNumber { get; set; }
            [XmlElement("FamilyNumber")]
            public string FamilyNumber { get; set; }
            [XmlElement("PaxDiscountCode")]
            public string PaxDiscountCode { get; set; }
            [XmlElement("Names")]
            public Names Names { get; set; }
            [XmlElement("Infant")]
            public string Infant { get; set; }
            [XmlElement("PassengerInfo")]
            public PassengerInfo PassengerInfo { get; set; }
            [XmlElement("PassengerProgram")]
            public PassengerProgram PassengerProgram { get; set; }
            [XmlElement("PassengerFees")]
            public PassengerFees PassengerFees { get; set; }
            [XmlElement("PassengerAddresses")]
            public string PassengerAddresses { get; set; }
            [XmlElement("PassengerTravelDocuments")]
            public PassengerTravelDocuments PassengerTravelDocuments { get; set; }
            [XmlElement("PassengerBags")]
            public string PassengerBags { get; set; }
            [XmlElement("PassengerID")]
            public string PassengerID { get; set; }
            [XmlElement("PassengerTypeInfos")]
            public PassengerTypeInfos PassengerTypeInfos { get; set; }
            [XmlElement("PassengerInfos")]
            public PassengerInfos PassengerInfos { get; set; }
            [XmlElement("PassengerInfants")]
            public string PassengerInfants { get; set; }
            [XmlElement("PseudoPassenger")]
            public string PseudoPassenger { get; set; }
            [XmlElement("PassengerTypeInfo")]
            public PassengerTypeInfo PassengerTypeInfo { get; set; }
        }
        public class Names
        {
            [XmlElement("BookingName")]
            public BookingName BookingName { get; set; }
        }
        public class BookingName
        {
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("FirstName")]
            public string FirstName { get; set; }
            [XmlElement("MiddleName")]
            public string MiddleName { get; set; }
            [XmlElement("LastName")]
            public string LastName { get; set; }
            [XmlElement("Suffix")]
            public string Suffixe { get; set; }
            [XmlElement("Title")]
            public string Title { get; set; }
        }
        public class PassengerInfo
        {
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("BalanceDue")]
            public string BalanceDue { get; set; }
            [XmlElement("Gender")]
            public string Gender { get; set; }
            [XmlElement("Nationality")]
            public string Nationality { get; set; }
            [XmlElement("ResidentCountry")]
            public string ResidentCountry { get; set; }
            [XmlElement("TotalCost")]
            public string TotalCost { get; set; }
            [XmlElement("WeightCategory")]
            public string WeightCategory { get; set; }
            }
        public class PassengerProgram
        {
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("ProgramCode")]
            public string ProgramCode { get; set; }
            [XmlElement("ProgramLevelCode")]
            public string ProgramLevelCode { get; set; }
            [XmlElement("ProgramNumber")]
            public string ProgramNumber { get; set; }
        }
        public class PassengerFees
        {
            [XmlElement("PassengerFee")]
            public PassengerFee PassengerFee { get; set; }
        }
        public class PassengerFee
        {
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("ActionStatusCode")]
            public string ActionStatusCode { get; set; }
            [XmlElement("FeeCode")]
            public string FeeCode { get; set; }
            [XmlElement("FeeDetail")]
            public string FeeDetail { get; set; }
            [XmlElement("FeeNumber")]
            public string FeeNumber { get; set; }
            [XmlElement("FeeType")]
            public string FeeType { get; set; }
            [XmlElement("FeeOverride")]
            public string FeeOverride { get; set; }
            [XmlElement("FlightReference")]
            public string FlightReference { get; set; }
            [XmlElement("Note")]
            public string Note { get; set; }
            [XmlElement("SSRCode")]
            public string SSRCode { get; set; }
            [XmlElement("SSRNumber")]
            public string SSRNumber { get; set; }
            [XmlElement("PaymentNumber")]
            public string PaymentNumber { get; set; }
            [XmlElement("ServiceCharges")]
            public ServiceCharges ServiceCharges { get; set; }
            [XmlElement("CreatedDate")]
            public string CreatedDate { get; set; }
            [XmlElement("IsProtected")]
            public string IsProtected { get; set; }
        }
        public class ServiceCharges
        {
            [XmlElement("BookingServiceCharge")]
            public BookingServiceCharge BookingServiceCharge { get; set; }
        }
        public class BookingServiceCharge
        {
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("ChargeType")]
            public string ChargeType { get; set; }
            [XmlElement("CollectType")]
            public string CollectType { get; set; }
            [XmlElement("ChargeCode")]
            public string ChargeCode { get; set; }
            [XmlElement("TicketCode")]
            public string TicketCode { get; set; }
            [XmlElement("CurrencyCode")]
            public string CurrencyCode { get; set; }
            [XmlElement("Amount")]
            public string Amount { get; set; }
            [XmlElement("ChargeDetail")]
            public string ChargeDetail { get; set; }
            [XmlElement("ForeignCurrencyCode")]
            public string ForeignCurrencyCode { get; set; }
            [XmlElement("ForeignAmount")]
            public string ForeignAmount { get; set; }
        }
        public class PassengerTravelDocuments
        {
            [XmlElement("PassengerTravelDocument")]
            public PassengerTravelDocument PassengerTravelDocument { get; set; }
        }
        public class PassengerTravelDocument
        {
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("DocTypeCode")]
            public string DocTypeCode { get; set; }
            [XmlElement("IssuedByCode")]
            public string IssuedByCode { get; set; }
            [XmlElement("DocSuffix")]
            public string DocSuffix { get; set; }
            [XmlElement("DocNumber")]
            public string DocNumber { get; set; }
            [XmlElement("DOB")]
            public string DOB { get; set; }
            [XmlElement("Gender")]
            public string Gender { get; set; }
            [XmlElement("Nationality")]
            public string Nationality { get; set; }
            [XmlElement("ExpirationDate")]
            public string ExpirationDate { get; set; }
            [XmlElement("Names")]
            public Names Names { get; set; }
            [XmlElement("BirthCountry")]
            public string BirthCountry { get; set; }
            [XmlElement("IssuedDate")]
            public string IssuedDate { get; set; }
        }
        public class PassengerTypeInfos
        {
            [XmlElement("PassengerTypeInfo")]
            public PassengerTypeInfo PassengerTypeInfo { get; set; }
        }
        public class PassengerTypeInfo
        {
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("DOB")]
            public string DOB { get; set; }
            [XmlElement("PaxType")]
            public string PaxType { get; set; }
        }
        public class PassengerInfos
        {
            [XmlElement("PassengerInfo")]
            public PassengerInfo PassengerInfo { get; set; }
        }
        public class Journeys
        {
            [XmlElement("Journey")]
            public Journey Journey { get; set; }
        }
        public class Journey
        {
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("NotForGeneralUse")]
            public string NotForGeneralUse { get; set; }
            [XmlElement("Segments")]
            public Segments Segments { get; set; }
            [XmlElement("JourneySellKey")]
            public string JourneySellKey { get; set; }
        }
        public class Segments
        {
            [XmlElement("Segment")]
            public Segment Segment { get; set; }
        }
        public class Segment
        {
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("ActionStatusCode")]
            public string ActionStatusCode { get; set; }
            [XmlElement("ArrivalStation")]
            public string ArrivalStation { get; set; }
            [XmlElement("CabinOfService")]
            public string CabinOfService { get; set; }
            [XmlElement("ChangeReasonCod")]
            public string ChangeReasonCod { get; set; }
            [XmlElement("DepartureStation")]
            public string DepartureStation { get; set; }
            [XmlElement("PriorityCode")]
            public string PriorityCode { get; set; }
            [XmlElement("SegmentType")]
            public string SegmentType { get; set; }
            [XmlElement("STA")]
            public string STA { get; set; }
            [XmlElement("STD")]
            public string STD{ get; set; }
            [XmlElement("International")]
            public string International { get; set; }
            [XmlElement("FlightDesignator")]
            public FlightDesignator FlightDesignator { get; set; }
            [XmlElement("XrefFlightDesignator")]
            public string XrefFlightDesignator { get; set; }
            [XmlElement("Fares")]
            public Fares Fares { get; set; }
            [XmlElement("Legs")]
            public Legs Legs { get; set; }
            [XmlElement("PaxBags")]
            public string PaxBags { get; set; }
            [XmlElement("PaxSeats")]
            public PaxSeats PaxSeats { get; set; }
            [XmlElement("PaxSSRs")]
            public PaxSSRs PaxSSRs { get; set; }
            [XmlElement("PaxSegments")]
            public PaxSegments PaxSegments { get; set; }
            [XmlElement("PaxTickets")]
            public string PaxTickets { get; set; }
            [XmlElement("PaxSeatPreferences")]
            public string PaxSeatPreferences { get; set; }
            [XmlElement("SalesDate")]
            public string SalesDate { get; set; }
            [XmlElement("SegmentSellKey")]
            public string SegmentSellKey { get; set; }
            [XmlElement("PaxScores")]
            public string PaxScores { get; set; }
            [XmlElement("ChannelType")]
            public string ChannelType { get; set; }
        }
        public class FlightDesignator
        {
            [XmlElement("CarrierCode")]
            public string CarrierCode { get; set; }
            [XmlElement("FlightNumber")]
            public string FlightNumber { get; set; }
            [XmlElement("OpSuffix")]
            public string OpSuffix { get; set; }
        }
        public class Fares
        {
            [XmlElement("Fare")]
            public Fare Fare { get; set; }
        }
        public class Fare
        {
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("ClassOfService")]
            public string ClassOfService { get; set; }
            [XmlElement("ClassType")]
            public string ClassType { get; set; }
            [XmlElement("RuleTariff")]
            public string RuleTariff { get; set; }
            [XmlElement("CarrierCode")]
            public string CarrierCode { get; set; }
            [XmlElement("RuleNumber")]
            public string RuleNumber { get; set; }
            [XmlElement("FareBasisCode")]
            public string FareBasisCode { get; set; }
            [XmlElement("FareSequence")]
            public string FareSequence { get; set; }
            [XmlElement("FareClassOfService")]
            public string FareClassOfService { get; set; }
            [XmlElement("FareStatus")]
            public string FareStatus { get; set; }
            [XmlElement("FareApplicationType")]
            public string FareApplicationType { get; set; }
            [XmlElement("OriginalClassOfService")]
            public string OriginalClassOfService { get; set; }
            [XmlElement("XrefClassOfService")]
            public string XrefClassOfService { get; set; }
            [XmlElement("PaxFares")]
            public PaxFares PaxFares { get; set; }
            [XmlElement("ProductClass")]
            public string ProductClass { get; set; }
            [XmlElement("IsAllotmentMarketFare")]
            public string IsAllotmentMarketFare { get; set; }
            [XmlElement("TravelClassCode")]
            public string TravelClassCode { get; set; }
            [XmlElement("FareSellKey")]
            public string FareSellKey { get; set; }
            [XmlElement("InboundOutbound")]
            public string InboundOutbound { get; set; }
        }
        public class PaxFares
        {
            [XmlElement("PaxFare")]
            public PaxFare PaxFare { get; set; }
        }
        public class PaxFare
        {
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("PaxType")]
            public string PaxType { get; set; }
            [XmlElement("PaxDiscountCode")]
            public string PaxDiscountCode { get; set; }
            [XmlElement("FareDiscountCode")]
            public string FareDiscountCode { get; set; }
            [XmlElement("ServiceCharges")]
            public ServiceCharges ServiceCharges { get; set; }
        }
        public class Legs
        {
            [XmlElement("Leg")]
            public Leg Leg { get; set; }
        }
        public class Leg
        {
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("ArrivalStation")]
            public string ArrivalStation { get; set; }
            [XmlElement("DepartureStation")]
            public string DepartureStation { get; set; }
            [XmlElement("STA")]
            public string STA { get; set; }
            [XmlElement("STD")]
            public string STD { get; set; }
            [XmlElement("FlightDesignator")]
            public FlightDesignator FlightDesignator { get; set; }
            [XmlElement("LegInfo")]
            public LegInfo LegInfo { get; set; }
            [XmlElement("OperationsInfo")]
            public OperationsInfo OperationsInfo { get; set; }
            [XmlElement("InventoryLegID")]
            public string InventoryLegID { get; set; }
        }
        public class LegInfo
        {
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("AdjustedCapacity")]
            public string AdjustedCapacity { get; set; }
            [XmlElement("EquipmentType")]
            public string EquipmentType { get; set; }
            [XmlElement("EquipmentTypeSuffix")]
            public string EquipmentTypeSuffix { get; set; }
            [XmlElement("ArrivalTerminal")]
            public string ArrivalTerminal { get; set; }
            [XmlElement("ArrvLTV")]
            public string ArrvLTV { get; set; }
            [XmlElement("Capacity")]
            public string Capacity { get; set; }
            [XmlElement("CodeShareIndicator")]
            public string CodeShareIndicator { get; set; }
            [XmlElement("DepartureTerminal")]
            public string DepartureTerminal { get; set; }
            [XmlElement("DeptLTV")]
            public string DeptLTV { get; set; }
            [XmlElement("ETicket")]
            public string ETicket { get; set; }
            [XmlElement("FlifoUpdated")]
            public string FlifoUpdated { get; set; }
            [XmlElement("IROP")]
            public string IROP { get; set; }
            [XmlElement("Status")]
            public string Status { get; set; }
            [XmlElement("Lid")]
            public string Lid { get; set; }
            [XmlElement("OnTime")]
            public string OnTime { get; set; }
            [XmlElement("PaxSTA")]
            public string PaxSTA { get; set; }
            [XmlElement("PaxSTD")]
            public string PaxSTAD{ get; set; }
            [XmlElement("PRBCCode")]
            public string PRBCCode { get; set; }
            [XmlElement("ScheduleServiceType")]
            public string ScheduleServiceType { get; set; }
            [XmlElement("Sold")]
            public string Sold { get; set; }
            [XmlElement("OutMoveDays")]
            public string OutMoveDays { get; set; }
            [XmlElement("BackMoveDays")]
            public string BackMoveDays { get; set; }
            [XmlElement("LegNests")]
            public string LegNests { get; set; }
            [XmlElement("LegSSRs")]
            public string LegSSRs { get; set; }
            [XmlElement("OperatingFlightNumber")]
            public string OperatingFlightNumber { get; set; }
            [XmlElement("OperatedByText")]
            public string OperatedByText { get; set; }
            [XmlElement("OperatingCarrier")]
            public string OperatingCarrier { get; set; }
            [XmlElement("OperatingOpSuffix")]
            public string OperatingOpSuffix { get; set; }
            [XmlElement("SubjectToGovtApproval")]
            public string SubjectToGovtApproval { get; set; }
            [XmlElement("MarketingCode")]
            public string MarketingCode { get; set; }
            [XmlElement("ChangeOfDirection")]
            public string ChangeOfDirection { get; set; }
            [XmlElement("MarketingOverride")]
            public string MarketingOverride { get; set; }
        }
        public class PaxSeats
        {
            [XmlElement("PaxSeat")]
            public PaxSeat PaxSeat { get; set; }
        }
        public class PaxSeat
        {
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("PassengerNumber")]
            public string PassengerNumber { get; set; }
            [XmlElement("ArrivalStation")]
            public string ArrivalStation { get; set; }
            [XmlElement("DepartureStation")]
            public string DepartureStation { get; set; }
            [XmlElement("UnitDesignator")]
            public string UnitDesignator { get; set; }
            [XmlElement("CompartmentDesignator")]
            public string CompartmentDesignator { get; set; }
            [XmlElement("SeatPreference")]
            public string SeatPreference { get; set; }
            [XmlElement("Penalty")]
            public string Penalty { get; set; }
            [XmlElement("SeatTogetherPreference")]
            public string SeatTogetherPreference { get; set; }
            [XmlElement("PaxSeatInfo")]
            public PaxSeatInfo PaxSeatInfo { get; set; }
        }
        public class PaxSeatInfo
        {
            [XmlElement("SeatSet")]
            public string SeatSet { get; set; }
            [XmlElement("Deck")]
            public string Deck { get; set; }
            [XmlElement("Properties")]
            public Properties Properties { get; set; }
        }
        public class Properties
        {
            [XmlElement("KeyValuePairOfstringstring")]
            public KeyValuePairOfstringstring KeyValuePairOfstringstring { get; set; }
        }
        public class KeyValuePairOfstringstring
        {
            [XmlElement("key")]
            public String key { get; set; }
            [XmlElement("value")]
            public String value { get; set; }
        }
        public class PaxSSRs
        {
            [XmlElement("PaxSSR")]
            public PaxSSR PaxSSR { get; set; }
        }
        public class PaxSSR
        {
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("ActionStatusCode")]
            public string ActionStatusCode { get; set; }
            [XmlElement("ArrivalStation")]
            public string ArrivalStationn { get; set; }
            [XmlElement("DepartureStation")]
            public string DepartureStation { get; set; }
            [XmlElement("PassengerNumber")]
            public string PassengerNumber { get; set; }
            [XmlElement("SSRCode")]
            public string SSRCode { get; set; }
            [XmlElement("SSRNumber")]
            public string SSRNumber { get; set; }
            [XmlElement("SSRDetail")]
            public string SSRDetail { get; set; }
            [XmlElement("FeeCode")]
            public string FeeCode { get; set; }
            [XmlElement("Note")]
            public string Note { get; set; }
            [XmlElement("SSRValue")]
            public string SSRValue { get; set; }
        }
        public class PaxSegments
        {
            [XmlElement("PaxSegment")]
            public PaxSegment PaxSegment { get; set; }
        }
        public class PaxSegment
        {
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("BoardingSequence")]
            public string BoardingSequence { get; set; }
            [XmlElement("CreatedDate")]
            public string CreatedDate { get; set; }
            [XmlElement("LiftStatus")]
            public string LiftStatus { get; set; }
            [XmlElement("OverBookIndicator")]
            public string OverBookIndicator { get; set; }
            [XmlElement("PassengerNumber")]
            public string PassengerNumber { get; set; }
            [XmlElement("PriorityDate")]
            public string PriorityDate { get; set; }
            [XmlElement("TripType")]
            public string TripType { get; set; }
            [XmlElement("TimeChanged")]
            public string TimeChanged { get; set; }
            [XmlElement("POS")]
            public POS POS { get; set; }
            [XmlElement("SourcePOS")]
            public SourcePOS SourcePOS { get; set; }
            [XmlElement("VerifiedTravelDocs")]
            public string VerifiedTravelDocs { get; set; }
            [XmlElement("ModifiedDate")]
            public string ModifiedDate { get; set; }
            [XmlElement("ActivityDate")]
            public string ActivityDate { get; set; }
            [XmlElement("BaggageAllowanceWeight")]
            public string BaggageAllowanceWeight { get; set; }
            [XmlElement("BaggageAllowanceWeightType")]
            public string BaggageAllowanceWeightType { get; set; }
            [XmlElement("BaggageAllowanceUsed")]
            public string BaggageAllowanceUsed { get; set; }
            [XmlElement("ReferenceNumber")]
            public string ReferenceNumber { get; set; }
            [XmlElement("BoardingPassDetail")]
            public string BoardingPassDetail { get; set; }
        }
        public class BookingComments
        {
            [XmlElement("BookingComment")]
            public BookingComment[] BookingComment { get; set; }
        }
        public class BookingComment
        {
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("CommentType")]
            public string CommentType { get; set; }
            [XmlElement("CommentText")]
            public string CommentText { get; set; }
            [XmlElement("PointOfSale")]
            public PointOfSale PointOfSale { get; set; }
            [XmlElement("CreatedDate")]
            public string CreatedDate { get; set; }
        }
        public class PointOfSale
        {
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("AgentCode")]
            public string AgentCode { get; set; }
            [XmlElement("OrganizationCode")]
            public string OrganizationCode { get; set; }
            [XmlElement("DomainCode")]
            public string DomainCodee { get; set; }
            [XmlElement("LocationCode")]
            public string LocationCode { get; set; }
        }
        public class BookingContacts
        {
            [XmlElement("BookingContact")]
            public BookingContact BookingContact { get; set; }
        }
        public class BookingContact
        {
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("TypeCode")]
            public string TypeCode { get; set; }
            [XmlElement("Names")]
            public Names Names { get; set; }
            [XmlElement("EmailAddress")]
            public string EmailAddress { get; set; }
            [XmlElement("HomePhone")]
            public string HomePhone { get; set; }
            [XmlElement("WorkPhone")]
            public string WorkPhone { get; set; }
            [XmlElement("OtherPhone")]
            public string OtherPhone { get; set; }
            [XmlElement("Fax")]
            public string Fax { get; set; }
            [XmlElement("CompanyName")]
            public string CompanyName { get; set; }
            [XmlElement("AddressLine1")]
            public string AddressLine1 { get; set; }
            [XmlElement("AddressLine2")]
            public string AddressLine2 { get; set; }
            [XmlElement("AddressLine3")]
            public string AddressLine3 { get; set; }
            [XmlElement("City")]
            public string City { get; set; }
            [XmlElement("ProvinceState")]
            public string ProvinceState { get; set; }
            [XmlElement("PostalCode")]
            public string PostalCode { get; set; }
            [XmlElement("CountryCode")]
            public string CountryCode { get; set; }
            [XmlElement("CultureCode")]
            public string CultureCode { get; set; }
            [XmlElement("DistributionOption")]
            public string DistributionOption { get; set; }
            [XmlElement("CustomerNumber")]
            public string CustomerNumber { get; set; }
            [XmlElement("NotificationPreference")]
            public string NotificationPreference { get; set; }
            [XmlElement("SourceOrganization")]
            public string SourceOrganization { get; set; }
        }
        public class Payments
        {
            [XmlElement("Payment")]
            public Payment Payment { get; set; }
        }
        public class Payment
        {
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("ReferenceType")]
            public string ReferenceType { get; set; }
            [XmlElement("ReferenceID")]
            public string ReferenceID { get; set; }
            [XmlElement("PaymentMethodType")]
            public string PaymentMethodType { get; set; }
            [XmlElement("PaymentMethodCode")]
            public string PaymentMethodCode { get; set; }
            [XmlElement("CurrencyCode")]
            public string CurrencyCode { get; set; }
            [XmlElement("PaymentAmount")]
            public string PaymentAmount { get; set; }
            [XmlElement("CollectedCurrencyCode")]
            public string CollectedCurrencyCode { get; set; }
            [XmlElement("CollectedAmount")]
            public string CollectedAmount { get; set; }
            [XmlElement("QuotedCurrencyCode")]
            public string QuotedCurrencyCode { get; set; }
            [XmlElement("QuotedAmount")]
            public string QuotedAmount { get; set; }
            [XmlElement("Status")]
            public string Status { get; set; }
            [XmlElement("AccountNumber")]
            public string AccountNumber { get; set; }
            [XmlElement("AccountNumberID")]
            public string AccountNumberID { get; set; }
            [XmlElement("Expiration")]
            public string Expiration { get; set; }
            [XmlElement("AuthorizationCode")]
            public string AuthorizationCode { get; set; }
            [XmlElement("AuthorizationStatus")]
            public string AuthorizationStatus { get; set; }
            [XmlElement("ParentPaymentID")]
            public string ParentPaymentID { get; set; }
            [XmlElement("Transferred")]
            public string Transferred { get; set; }
            [XmlElement("ReconcilliationID")]
            public string ReconcilliationID { get; set; }
            [XmlElement("FundedDate")]
            public string FundedDate { get; set; }
            [XmlElement("Installments")]
            public string Installments { get; set; }
            [XmlElement("PaymentText")]
            public string PaymentText { get; set; }
            [XmlElement("ChannelType")]
            public string ChannelType { get; set; }
            [XmlElement("PaymentNumber")]
            public string PaymentNumber { get; set; }
            [XmlElement("AccountName")]
            public string AccountName { get; set; }
            [XmlElement("SourcePointOfSale")]
            public SourcePointOfSale SourcePointOfSale { get; set; }
            [XmlElement("PointOfSale")]
            public PointOfSale PointOfSale { get; set; }
            [XmlElement("PaymentID")]
            public string PaymentID { get; set; }
            [XmlElement("Deposit")]
            public string Deposit { get; set; }
            [XmlElement("AccountID")]
            public string AccountID { get; set; }
            [XmlElement("Password")]
            public string Password { get; set; }
            [XmlElement("AccountTransactionCode")]
            public string AccountTransactionCode { get; set; }
            [XmlElement("VoucherID")]
            public string VoucherID { get; set; }
            [XmlElement("VoucherTransactionID")]
            public string VoucherTransactionID { get; set; }
            [XmlElement("OverrideVoucherRestrictions")]
            public string OverrideVoucherRestrictions { get; set; }
            [XmlElement("OverrideAmount")]
            public string OverrideAmount { get; set; }
            [XmlElement("RecordLocator")]
            public string RecordLocator { get; set; }
            [XmlElement("PaymentAddedToState")]
            public string PaymentAddedToState { get; set; }
            [XmlElement("DCC")]
            public DCC DCC { get; set; }
            [XmlElement("ThreeDSecure")]
            public ThreeDSecure ThreeDSecure { get; set; }
            [XmlElement("PaymentFields")]
            public PaymentFields PaymentFields { get; set; }
            [XmlElement("PaymentAddresses")]
            public string PaymentAddresses { get; set; }
            [XmlElement("CreatedDate")]
            public string CreatedDate { get; set; }
            [XmlElement("CreatedAgentID")]
            public string CreatedAgentID { get; set; }
            [XmlElement("ModifiedDate")]
            public string ModifiedDate { get; set; }
            [XmlElement("ModifiedAgentID")]
            public string ModifiedAgentID { get; set; }
            [XmlElement("BinRange")]
            public string BinRange { get; set; }
            [XmlElement("ApprovalDate")]
            public string ApprovalDate { get; set; }
        }
        public class SourcePointOfSale
        {
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("AgentCode")]
            public string AgentCode { get; set; }
            [XmlElement("OrganizationCode")]
            public string OrganizationCode { get; set; }
            [XmlElement("DomainCode")]
            public string DomainCode { get; set; }
            [XmlElement("LocationCode")]
            public string LocationCode { get; set; }
        }
        public class DCC
        {
            [XmlElement("DCCRateID")]
            public string DCCRateID { get; set; }
            [XmlElement("DCCStatus")]
            public string DCCStatus { get; set; }
            [XmlElement("ValidationDCCApplicable")]
            public string ValidationDCCApplicable { get; set; }
            [XmlElement("ValidationDCCRateValue")]
            public string ValidationDCCRateValue { get; set; }
            [XmlElement("ValidationDCCCurrency")]
            public string ValidationDCCCurrency { get; set; }
            [XmlElement("ValidationDCCAmount")]
            public string ValidationDCCAmount { get; set; }
            [XmlElement("ValidationDCCPutInState")]
            public string ValidationDCCPutInState { get; set; }
        }
        public class ThreeDSecure
        {
            [XmlElement("BrowserUserAgent")]
            public string BrowserUserAgent { get; set; }
            [XmlElement("BrowserAccept")]
            public string BrowserAccept { get; set; }
            [XmlElement("RemoteIpAddress")]
            public string RemoteIpAddress { get; set; }
            [XmlElement("TermUrl")]
            public string TermUrl { get; set; }
            [XmlElement("ProxyVia")]
            public string ProxyVia { get; set; }
            [XmlElement("ValidationTDSApplicable")]
            public string ValidationTDSApplicable { get; set; }
            [XmlElement("ValidationTDSPaReq")]
            public string ValidationTDSPaReq { get; set; }
            [XmlElement("ValidationTDSAcsUrl")]
            public string ValidationTDSAcsUrl { get; set; }
            [XmlElement("ValidationTDSPaRes")]
            public string ValidationTDSPaRes { get; set; }
            [XmlElement("ValidationTDSSuccessful")]
            public string ValidationTDSSuccessful { get; set; }
            [XmlElement("ValidationTDSAuthResult")]
            public string ValidationTDSAuthResult { get; set; }
            [XmlElement("ValidationTDSCavv")]
            public string ValidationTDSCavv { get; set; }
            [XmlElement("ValidationTDSCavvAlgorithm")]
            public string ValidationTDSCavvAlgorithm { get; set; }
            [XmlElement("ValidationTDSEci")]
            public string ValidationTDSEci { get; set; }
            [XmlElement("ValidationTDSXid")]
            public string ValidationTDSXid { get; set; }
        }
        public class PaymentFields
        {
            [XmlElement("PaymentField")]
            public PaymentField PaymentField { get; set; }
        }
        public class PaymentField
        {
            [XmlElement("FieldName")]
            public string FieldName { get; set; }
            [XmlElement("FieldValue")]
            public string FieldValue { get; set; }
        }


        #endregion
    }
}
