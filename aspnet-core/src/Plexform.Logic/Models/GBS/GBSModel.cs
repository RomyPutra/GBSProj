using System;
using System.Collections.Generic;
using System.Text;

namespace Plexform.Models
{
	public class GBSModels
	{
		public long BookingID { get; set; }
		public string PNR { get; set; }
		public string CurrencyCode { get; set; }
		public string DepartStation { get; set; }
		public string ArrivalStation { get; set; }
		public string FlightNum { get; set; }
		public string Carriercode { get; set; }
		public decimal BookingSum { get; set; }
		public IList<NavitaireBookingManager.GetBookingResponse> Functions { get; set; }
	}
	public class PaymentSchemeModels
	{
		public string GRPID { get; set; }
		public string SchemeCode { get; set; }
		public string CountryCode { get; set; }
		public string CurrencyCode { get; set; }
		public long Duration { get; set; }
		public long Minduration { get; set; }
		public string PaymentType { get; set; }
		public long Attempt_1 { get; set; }
		public string Code_1 { get; set; }
		public long Percentage_1 { get; set; }
		public long Attempt_2 { get; set; }
		public string Code_2 { get; set; }
		public long Percentage_2 { get; set; }
		public long Attempt_3 { get; set; }
		public string Code_3 { get; set; }
		public long Percentage_3 { get; set; }
		public long MinDeposit { get; set; }
		public long MaxDeposit { get; set; }
		public long MinDeposit2 { get; set; }
		public long MaxDeposit2 { get; set; }
        public long DepositValue { get; set; }
        //public string PrevCountry { get; set; }
    }
	public class CountryModels
	{
		public string CountryCode { get; set; }
		public string CountryName { get; set; }
		public string CurrencyCode { get; set; }
	}
	public class FltTimeGroupModels
	{
		public string FTGroupCode { get; set; }
		public TimeSpan StartTime { get; set; }
		public TimeSpan EndTime { get; set; }
		public DateTime SyncCreate { get; set; }
		public DateTime SyncLastUpd { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime UpdateDate { get; set; }
		public string LastSyncBy { get; set; }
		public string CreateBy { get; set; }
		public string UpdateBy { get; set; }
		public int Active { get; set; }
	}
	public class AGENTACCESSFAREModels
	{
        public System.String Analyst { get; set; }
        public System.String MarketCode { get; set; }
		public System.String InTier { get; set; }
		public System.String OutTier { get; set; }
		public System.String InFareClass { get; set; }
		public System.String OutFareClass { get; set; }
		public int Status { get; set; }
		public int Inuse { get; set; }
		public System.DateTime SyncCreate { get; set; }
		public System.DateTime SyncLastUpd { get; set; }
		public System.String LastSyncBy { get; set; }
		public System.DateTime CreateDate { get; set; }
		public System.String CreateBy { get; set; }
		public System.DateTime UpdateDate { get; set; }
		public System.String UpdateBy { get; set; }
		public int Active { get; set; }
        public System.String OutRoute { get; set; }
        public System.String OutTier1 { get; set; }
        public System.String OutTier2 { get; set; }
        public System.String OutTier3 { get; set; }
        public System.String OutGeneric { get; set; }
        public System.String InRoute { get; set; }
        public System.String InTier1 { get; set; }
        public System.String InTier2 { get; set; }
        public System.String InTier3 { get; set; }
        public System.String InGeneric { get; set; }
    }
    public class AGENTTIERModels
    {
        public System.String MarketCode { get; set; }
        public System.String Analyst { get; set; }
        public System.String InRoute { get; set; }
        public System.String InTier { get; set; }
        public System.String InSubTier { get; set; }
        public System.String InAgent { get; set; }
        public System.String InAgentEmail { get; set; }
        public System.String InAgentID { get; set; }
        public System.String OutRoute { get; set; }
        public System.String OutTier { get; set; }
        public System.String OutSubTier { get; set; }
        public System.String OutAgent { get; set; }
        public System.String OutAgentEmail { get; set; }
        public System.String OutAgentID { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
    }
    public class GROUPCAPModels
    {
        public System.String MarketCode { get; set; }
        public System.String Analyst { get; set; }
        public System.String InRoute { get; set; }
        public System.Decimal InGrpCap { get; set; }
        public System.String OutRoute { get; set; }
        public System.Decimal OutGrpCap { get; set; }
    }
    public class MAXDISCModels
    {
        public System.String MarketCode { get; set; }
        public System.String Analyst { get; set; }
        public System.String InRoute { get; set; }
        public System.Decimal InMaxDisc { get; set; }
        public System.String OutRoute { get; set; }
        public System.Decimal OutMaxDisc { get; set; }
    }
    public class SEASONALITYModels
    {
        public System.String Analyst { get; set; }
        public System.String RouteCode { get; set; }
        public System.DateTime SeasonDate { get; set; }
        public System.String Season { get; set; }
    }
    public class DISCWEIGHTModels
    {
        public System.String Analyst { get; set; }
        public System.String MarketCode { get; set; }
        public System.String InRoute { get; set; }
        public System.Decimal InWALFDisc { get; set; }
        public System.Decimal InWAPUDisc { get; set; }
        public System.String OutRoute { get; set; }
        public System.Decimal OutWALFDisc { get; set; }
        public System.Decimal OutWAPUDisc { get; set; }
    }
    public class FLOORFAREModels
    {
        public System.String Analyst { get; set; }
        public System.String MarketCode { get; set; }
        public System.String InRoute { get; set; }
        public System.String InCurrency { get; set; }
        public System.Decimal InDisc { get; set; }
        public System.String InFareClass { get; set; }
        public System.Decimal InFloorFare { get; set; }
        public System.String OutRoute { get; set; }
        public System.String OutCurrency { get; set; }
        public System.Decimal OutDisc { get; set; }
        public System.String OutFareClass { get; set; }
        public System.Decimal OutFloorFare { get; set; }
    }
    public class LFDISCOUNTModels
    {
        public System.String Analyst { get; set; }
        public System.String MarketCode { get; set; }
        public System.String NDODesc { get; set; }
        public System.String InRoute { get; set; }
        public System.Decimal InLFDisc1 { get; set; }
        public System.Decimal InLFDisc2 { get; set; }
        public System.Decimal InLFDisc3 { get; set; }
        public System.Decimal InLFDisc4 { get; set; }
        public System.Decimal InLFDisc5 { get; set; }
        public System.Decimal InLFDisc6 { get; set; }
        public System.Decimal InLFDisc7 { get; set; }
        public System.Decimal InLFDisc8 { get; set; }
        public System.Decimal InLFDisc9 { get; set; }
        public System.Decimal InLFDisc10 { get; set; }
        public System.String OutRoute { get; set; }
        public System.Decimal OutLFDisc1 { get; set; }
        public System.Decimal OutLFDisc2 { get; set; }
        public System.Decimal OutLFDisc3 { get; set; }
        public System.Decimal OutLFDisc4 { get; set; }
        public System.Decimal OutLFDisc5 { get; set; }
        public System.Decimal OutLFDisc6 { get; set; }
        public System.Decimal OutLFDisc7 { get; set; }
        public System.Decimal OutLFDisc8 { get; set; }
        public System.Decimal OutLFDisc9 { get; set; }
        public System.Decimal OutLFDisc10 { get; set; }
    }
    public class PUDISCOUNTModels
    {
        public System.String Analyst { get; set; }
        public System.String MarketCode { get; set; }
        public System.String NDODesc { get; set; }
        public System.String InRoute { get; set; }
        public System.Decimal InPUDisc1 { get; set; }
        public System.Decimal InPUDisc2 { get; set; }
        public System.Decimal InPUDisc3 { get; set; }
        public System.Decimal InPUDisc4 { get; set; }
        public System.Decimal InPUDisc5 { get; set; }
        public System.Decimal InPUDisc6 { get; set; }
        public System.Decimal InPUDisc7 { get; set; }
        public System.Decimal InPUDisc8 { get; set; }
        public System.Decimal InPUDisc9 { get; set; }
        public System.Decimal InPUDisc10 { get; set; }
        public System.Decimal InPUDisc11 { get; set; }
        public System.String OutRoute { get; set; }
        public System.Decimal OutPUDisc1 { get; set; }
        public System.Decimal OutPUDisc2 { get; set; }
        public System.Decimal OutPUDisc3 { get; set; }
        public System.Decimal OutPUDisc4 { get; set; }
        public System.Decimal OutPUDisc5 { get; set; }
        public System.Decimal OutPUDisc6 { get; set; }
        public System.Decimal OutPUDisc7 { get; set; }
        public System.Decimal OutPUDisc8 { get; set; }
        public System.Decimal OutPUDisc9 { get; set; }
        public System.Decimal OutPUDisc10 { get; set; }
        public System.Decimal OutPUDisc11 { get; set; }
    }
    public class SPECIALFAREModels
    {
        public System.String Analyst { get; set; }
        public System.String MarketCode { get; set; }
        public System.String NDODesc { get; set; }
        public System.String CodeDesc { get; set; }
        public System.String InRoute { get; set; }
        public System.String InFlightTimeGrp { get; set; }
        public System.String InDepDOW { get; set; }
        public System.String InAgentTier { get; set; }
        public System.String InCurrency { get; set; }
        public System.Decimal InLFFare1 { get; set; }
        public System.Decimal InLFFare2 { get; set; }
        public System.Decimal InLFFare3 { get; set; }
        public System.Decimal InLFFare4 { get; set; }
        public System.Decimal InLFFare5 { get; set; }
        public System.Decimal InLFFare6 { get; set; }
        public System.Decimal InLFFare7 { get; set; }
        public System.Decimal InLFFare8 { get; set; }
        public System.Decimal InLFFare9 { get; set; }
        public System.Decimal InLFFare10 { get; set; }
        public System.Decimal InLFFare11 { get; set; }
        public System.String OutRoute { get; set; }
        public System.String OutFlightTimeGrp { get; set; }
        public System.String OutDepDOW { get; set; }
        public System.String OutAgentTier { get; set; }
        public System.String OutCurrency { get; set; }
        public System.Decimal OutLFFare1 { get; set; }
        public System.Decimal OutLFFare2 { get; set; }
        public System.Decimal OutLFFare3 { get; set; }
        public System.Decimal OutLFFare4 { get; set; }
        public System.Decimal OutLFFare5 { get; set; }
        public System.Decimal OutLFFare6 { get; set; }
        public System.Decimal OutLFFare7 { get; set; }
        public System.Decimal OutLFFare8 { get; set; }
        public System.Decimal OutLFFare9 { get; set; }
        public System.Decimal OutLFFare10 { get; set; }
        public System.Decimal OutLFFare11 { get; set; }
    }
	public class RestrictionModels
	{
		public System.String Status { get; set; }
		public System.String BookFrom { get; set; }
		public System.String BookTo { get; set; }
		public System.String TraFrom { get; set; }
		public System.String TraTo { get; set; }
		public System.String RestrictionNote{ get; set; }
		public System.String RestrictionAlert { get; set; }
		public System.String RestrictionNoteEx { get; set; }
		public System.String RestrictionAlertEx { get; set; }
	}
	public class CODEMASTERModels
	{
		public System.String CodeType { get; set; }
		public System.String Code { get; set; }
		public System.String CodeDesc { get; set; }
	}
}
