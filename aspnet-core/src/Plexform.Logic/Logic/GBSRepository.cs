using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Plexform.Configuration;
using Abp.Reflection.Extensions;
using System;
using System.Data;
using SEAL.Data;
using ABS.Logic.GroupBooking.Agent;
using ABS.Logic.GroupBooking.Booking;
using ABS.GBS.Log;
using Sharpbrake.Client;

namespace Plexform.GBS
{
	public class GBSRepository : SEAL.Model.Moyenne.CoreStandard
	{
		protected static DataAccess objDCom;
		AdminControl.StrucAdminSet _AdminSet;
		AdminControl objAdmin;
		AdminControl.StrucAdminSet adminSet;
		SystemLog systemLog;
		USRPROFILE_Info usrInfo;
		BookingControl objBooking;
		PaymentControl scheme;


		private readonly IConfigurationRoot _appConfiguration;

		public GBSRepository()
		{
			_appConfiguration = AppConfigurations.Get(
				typeof(GBSRepository).GetAssembly().GetDirectoryPathOrNull()
			);
			_AdminSet = new AdminControl.StrucAdminSet();
			objAdmin = new AdminControl();
			systemLog = new SystemLog();
			usrInfo = new USRPROFILE_Info();
			objBooking = new BookingControl();
			scheme = new PaymentControl();
			scheme.ConnectionURl(_appConfiguration.GetConnectionString(PlexformConsts.GBSConnectionString));
		}

		public async Task<string> GetSignaturesAsync()
		{
			NavitaireSessionManger.ISessionManager objsession = new NavitaireSessionManger.SessionManagerClient();
			NavitaireSessionManger.LogonRequest logonReq = new NavitaireSessionManger.LogonRequest();
			string signature = "";
			try
			{
				logonReq.logonRequestData = new NavitaireSessionManger.LogonRequestData();
				logonReq.logonRequestData.DomainCode = "def";
				logonReq.logonRequestData.AgentName = "APIGRPBOOK";
				logonReq.logonRequestData.Password = "grp@book1";
				logonReq.ContractVersion = 3413;

				NavitaireSessionManger.LogonResponse logonResponse = await objsession.LogonAsync(logonReq);
				if (logonResponse != null && logonResponse.Signature.ToString() != string.Empty)
				{
					signature = logonResponse.Signature;
				}
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return await Task.FromResult(signature);
		}

		public async Task<IList<Plexform.Models.PaymentSchemeModels>> GetAllScheme(string GRPID = "")
		{
			IList<Plexform.Models.PaymentSchemeModels> list = new List<Plexform.Models.PaymentSchemeModels>();
			PaymentInfo Model = new PaymentInfo();
			DataTable dt;
			try
			{
				dt = scheme.GetSchemeByCode(GRPID);
				if (dt != null && dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						string Currency = "";
						int xMinDeposit = 0, xMaxDeposit = 0, xMinDeposit2 = 0, xMaxDeposit2 = 0;
						if (dt.Rows[i]["Currency"].ToString() != "" && dt.Rows[i]["Currency"].ToString() != null)
						{
							Currency = dt.Rows[i]["Currency"].ToString();
							xMinDeposit = Convert.ToInt32(dt.Rows[i]["MinDeposit"]);
							xMaxDeposit = Convert.ToInt32(dt.Rows[i]["MaxDeposit"]);
							xMinDeposit2 = Convert.ToInt32(dt.Rows[i]["MinDeposit2"]);
							xMaxDeposit2 = Convert.ToInt32(dt.Rows[i]["MaxDeposit2"]);
						};
						list.Add(new Models.PaymentSchemeModels
						{
							GRPID = dt.Rows[i]["GRPID"].ToString(),
							SchemeCode = dt.Rows[i]["SchemeCode"].ToString(),
							CountryCode = dt.Rows[i]["CountryCode"].ToString(),
							Duration = Convert.ToInt32(dt.Rows[i]["Duration"]),
							Minduration = Convert.ToInt32(dt.Rows[i]["MinDuration"]),
							PaymentType = dt.Rows[i]["PaymentType"].ToString(),
							CurrencyCode = Currency,
							MinDeposit = xMinDeposit,
							MaxDeposit = xMaxDeposit,
							MinDeposit2 = xMinDeposit2,
							MaxDeposit2 = xMaxDeposit2,
							Attempt_1 = Convert.ToInt32(dt.Rows[i]["Attempt_1"]),
							Code_1 = dt.Rows[i]["Code_1"].ToString(),
							Percentage_1 = Convert.ToInt32(dt.Rows[i]["Percentage_1"]),
							Attempt_2 = Convert.ToInt32(dt.Rows[i]["Attempt_2"]),
							Code_2 = dt.Rows[i]["Code_2"].ToString(),
							Percentage_2 = Convert.ToInt32(dt.Rows[i]["Percentage_2"]),
							Attempt_3 = Convert.ToInt32(dt.Rows[i]["Attempt_3"]),
							Code_3 = dt.Rows[i]["Code_3"].ToString(),
							Percentage_3 = Convert.ToInt32(dt.Rows[i]["Percentage_3"])
						});
					}
				}
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return await Task.FromResult(list);
		}

		public async Task<IList<Plexform.Models.PaymentSchemeModels>> GetSchemeByCode(string GRPID = "", string CountryCode = "", string SchemeCode = "")
		{
			IList<Plexform.Models.PaymentSchemeModels> list = new List<Plexform.Models.PaymentSchemeModels>();
			PaymentInfo Model = new PaymentInfo();
			DataTable dt;
			try
			{
				dt = scheme.GetSchemeByCode(GRPID, CountryCode, SchemeCode);
				if (dt != null && dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						string Currency = "";
						int xMinDeposit = 0, xMaxDeposit = 0, xMinDeposit2 = 0, xMaxDeposit2 = 0;
						if (dt.Rows[i]["Currency"].ToString() != "" && dt.Rows[i]["Currency"].ToString() != null)
						{
							Currency = dt.Rows[i]["Currency"].ToString();
							xMinDeposit = Convert.ToInt32(dt.Rows[i]["MinDeposit"]);
							xMaxDeposit = Convert.ToInt32(dt.Rows[i]["MaxDeposit"]);
							xMinDeposit2 = Convert.ToInt32(dt.Rows[i]["MinDeposit2"]);
							xMaxDeposit2 = Convert.ToInt32(dt.Rows[i]["MaxDeposit2"]);
						};
						list.Add(new Models.PaymentSchemeModels
						{
							GRPID = dt.Rows[i]["GRPID"].ToString(),
							SchemeCode = dt.Rows[i]["SchemeCode"].ToString(),
							CountryCode = dt.Rows[i]["CountryCode"].ToString(),
							Duration = Convert.ToInt32(dt.Rows[i]["Duration"]),
							Minduration = Convert.ToInt32(dt.Rows[i]["MinDuration"]),
							PaymentType = dt.Rows[i]["PaymentType"].ToString(),
							CurrencyCode = Currency,
							MinDeposit = xMinDeposit,
							MaxDeposit = xMaxDeposit,
							MinDeposit2 = xMinDeposit2,
							MaxDeposit2 = xMaxDeposit2,
							Attempt_1 = Convert.ToInt32(dt.Rows[i]["Attempt_1"]),
							Code_1 = dt.Rows[i]["Code_1"].ToString(),
							Percentage_1 = Convert.ToInt32(dt.Rows[i]["Percentage_1"]),
							Attempt_2 = Convert.ToInt32(dt.Rows[i]["Attempt_2"]),
							Code_2 = dt.Rows[i]["Code_2"].ToString(),
							Percentage_2 = Convert.ToInt32(dt.Rows[i]["Percentage_2"]),
							Attempt_3 = Convert.ToInt32(dt.Rows[i]["Attempt_3"]),
							Code_3 = dt.Rows[i]["Code_3"].ToString(),
							Percentage_3 = Convert.ToInt32(dt.Rows[i]["Percentage_3"])
						});
					}
				}
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return await Task.FromResult(list);
		}

		public Task<bool> Update(PaymentInfo[] InfoScheme)
		{
			var res = false;
			try
			{
				res = scheme.SavePaymentScheme(InfoScheme);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return Task.FromResult(res);
		}

		public async Task<IList<Plexform.Models.GBSModels>> GetBookingByPNR(string id = "")
		{
			IList<Plexform.Models.GBSModels> list = new List<Plexform.Models.GBSModels>();
			NavitaireSessionManger.ISessionManager objsession = new NavitaireSessionManger.SessionManagerClient();
			NavitaireSessionManger.LogonRequest logonReq = new NavitaireSessionManger.LogonRequest();
			NavitaireBookingManager.IBookingManager bookingAPI = new NavitaireBookingManager.BookingManagerClient();
			NavitaireBookingManager.GetBookingRequest request = new NavitaireBookingManager.GetBookingRequest();
			NavitaireBookingManager.GetBookingResponse bookingResp;
			string signature = "";
			try
			{
				signature = await GetSignaturesAsync();
				request.ContractVersion = 3413;
				request.Signature = signature;
				request.GetBookingReqData = new NavitaireBookingManager.GetBookingRequestData();
				request.GetBookingReqData.GetBookingBy = NavitaireBookingManager.GetBookingBy.RecordLocator;
				request.GetBookingReqData.GetByRecordLocator = new NavitaireBookingManager.GetByRecordLocator();
				if (id == "")
				{
					request.GetBookingReqData.GetByRecordLocator.RecordLocator = "VE8RSP";
				}
				else
				{
					request.GetBookingReqData.GetByRecordLocator.RecordLocator = id;
				}

				NavitaireBookingManager.GetBookingResponse response = await bookingAPI.GetBookingAsync(request);
				if (response != null && response.Booking != null)
				{
					bookingResp = response;
					list.Add(new Models.GBSModels
					{
						BookingID = bookingResp.Booking.BookingID,
						BookingSum = bookingResp.Booking.BookingSum.TotalCost,
						PNR = bookingResp.Booking.RecordLocator,
						CurrencyCode = bookingResp.Booking.CurrencyCode,
						DepartStation = bookingResp.Booking.Journeys[0].Segments[0].DepartureStation,
						ArrivalStation = bookingResp.Booking.Journeys[0].Segments[0].ArrivalStation,
						FlightNum = bookingResp.Booking.Journeys[0].Segments[0].FlightDesignator.FlightNumber,
						Carriercode = bookingResp.Booking.Journeys[0].Segments[0].FlightDesignator.CarrierCode
					});
				}
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return await Task.FromResult(list);
			//return new ListResultContainer<Plexform.Models.GBSModels>(
			//	ObjectMapper.Map<List<Plexform.Models.GBSModels>>(list),
			//	list.Count
			//);
		}

		private Boolean AuthAdmin(string Username, string Password)
		{
			if (ValidateAdmin(Username, Password) == true)
			{
				DateTime timeNow = DateTime.Now;

				usrInfo = objAdmin.GetSingleUSRPROFILE(Username, "USRPROFILE.UserName");
				usrInfo.LastLogin = DateTime.Now;
				usrInfo.SyncLastUpd = DateTime.Now;
				usrInfo.LastSyncBy = usrInfo.UserID;
				usrInfo = objAdmin.SaveUserProfile(usrInfo, null, ABS.Logic.GroupBooking.Agent.AdminControl.EnumSaveType.Update);

				adminSet = objAdmin.AdminSet;

				return true;

			}
			else
			{
				return false;
			}
		}

		public bool ValidateAdmin(string Username, string Password)
		{
			String strSQL = string.Empty;
			String strFields = string.Empty;
			String strFilter = string.Empty;
			DataTable dt;
			if (Username == string.Empty) { return false; }
			try
			{
				if (StartConnection(EnumIsoState.StateUpdatetable, false) == true)
				{
					StartSQLControl();
					strFilter = " WHERE USRPROFILE.Username='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, Username) +
								"' AND USRPROFILE.Status = 1 AND USRGROUP.Status = 1 AND USRPROFILE.Password ='" + objSQL.ParseValue(SQLControl.EnumDataType.dtString, Password) + "'";
					strSQL = "SELECT DISTINCT  USRPROFILE.UserID, USRPROFILE.UserName, USRPROFILE.Password, USRPROFILE.RefID, USRPROFILE.RefType, USRPROFILE.ParentID, USRPROFILE.Status, USRPROFILE.Logged, " +
					"USRPROFILE.LogStation, USRPROFILE.LastLogin, USRPROFILE.LastLogout, USRPROFILE.CreateDate, USRPROFILE.CreateBy, USRPROFILE.LastUpdate, USRPROFILE.UpdateBy,USRPROFILE.rowguid, USRPROFILE.SyncCreate, " +
					"USRPROFILE.SyncLastUpd,USRPROFILE.OperationGroup, USRPROFILE.IsHost, USRPROFILE.LastSyncBy,USRGROUP.AccessLevel,USRAPP.AppID, USRPROFILE.SyncCreateBy,USRAPP.AccessCode,USRGROUP.GroupCode ,USRGROUP.GroupName FROM USRPROFILE JOIN USRAPP (NOLOCK) on USRAPP.UserID=USRPROFILE.UserID JOIN " +
					"USRGROUP (NOLOCK) on USRAPP.AccessCode=USRGROUP.GroupCode " + strFilter;
					dt = objDCom.Execute(strSQL, CommandType.Text, true);

					if (dt != null && dt.Rows.Count > 0)
					{
						DataRow drRow = dt.Rows[0];
						_AdminSet.AdminID = (string)drRow["UserID"];
						_AdminSet.AdminName = (string)drRow["UserName"];
						_AdminSet.RefID = (string)drRow["RefID"];
						_AdminSet.RefType = (byte)drRow["RefType"];
						_AdminSet.AccessLevel = (byte)drRow["AccessLevel"];
						_AdminSet.AppID = (int)drRow["AppID"];
						_AdminSet.GroupName = (string)drRow["GroupName"];
						_AdminSet.GroupCode = (string)drRow["GroupCode"];
						_AdminSet.OperationGroup = (string)drRow["OperationGroup"];
						return true;
					}
				}
				return false;
			}
			catch (Exception ex)
			{
				//SystemLog.Notifier.Notify(ex);
				return false;
			}
		}
	}
}
