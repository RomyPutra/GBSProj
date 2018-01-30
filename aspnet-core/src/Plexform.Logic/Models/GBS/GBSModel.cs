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
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public DateTime SyncCreate { get; set; }
		public DateTime SyncLastUpd { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime UpdateDate { get; set; }
		public string LastSyncBy { get; set; }
		public string CreateBy { get; set; }
		public string UpdateBy { get; set; }
		public int Active { get; set; }
	}
}
