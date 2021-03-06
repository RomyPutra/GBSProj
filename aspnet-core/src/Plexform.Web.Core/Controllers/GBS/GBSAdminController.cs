﻿using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Plexform.Authorization.Roles;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plexform.Controllers
{
	[Route("api/[controller]/[action]")]
	public class GBSAdminController : PlexformControllerBase
	{
		private readonly IRepository<Role> _roleRepository;

		public GBSAdminController(IRepository<Role> roleRepository)
		{
			try
			{
				_roleRepository = roleRepository;
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
		}

		[HttpGet]
		public async Task<ListResultContainer<Plexform.Models.CountryModels>> GetAllCountry(string CountryID = "")
		{
			IList<Plexform.Models.CountryModels> Scheme = new List<Plexform.Models.CountryModels>();
			try
			{
				var repo = new Plexform.GBS.GBSAdminLogic();
				Scheme = await repo.GetAllCountry(CountryID);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return new ListResultContainer<Plexform.Models.CountryModels>(
				ObjectMapper.Map<List<Plexform.Models.CountryModels>>(Scheme),
				Scheme.Count
			);
		}
		[HttpGet]
		public async Task<ListResultContainer<Plexform.Models.PaymentSchemeModels>> PaymentScheme(string GRPID = "")
		{
			IList<Plexform.Models.PaymentSchemeModels> Scheme = new List<Plexform.Models.PaymentSchemeModels>();
			try
			{
				var repo = new Plexform.GBS.GBSAdminLogic();
				Scheme = await repo.GetAllScheme(GRPID);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return new ListResultContainer<Plexform.Models.PaymentSchemeModels>(
				ObjectMapper.Map<List<Plexform.Models.PaymentSchemeModels>>(Scheme),
				Scheme.Count
			);
		}
		[HttpGet]
		public async Task<ListResultContainer<Plexform.Models.PaymentSchemeModels>> GetPaymentScheme(string GRPID = "", string CountryCode = "", string SchemeCode = "")
		{
			IList<Plexform.Models.PaymentSchemeModels> Scheme = new List<Plexform.Models.PaymentSchemeModels>();
			try
			{
				var repo = new Plexform.GBS.GBSAdminLogic();
				Scheme = await repo.GetSchemeByCodes(GRPID, CountryCode, SchemeCode);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return new ListResultContainer<Plexform.Models.PaymentSchemeModels>(
				ObjectMapper.Map<List<Plexform.Models.PaymentSchemeModels>>(Scheme),
				Scheme.Count
			);
		}
		[HttpPut]
		public async Task<bool> UpdatePaymentScheme([FromBody]GBS.GBSAdminLogic.PaymentInfo[] input)
		{
			bool res = false;
			try
			{
				var repo = new Plexform.GBS.GBSAdminLogic();

				res = await repo.UpdatePaymentScheme(input);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}

			return ObjectMapper.Map<bool>(res);
		}
		[HttpGet]
		public async Task<ListResultContainer<Plexform.Models.FltTimeGroupModels>> GroupTime(string Filter = "")
		{
			IList<Plexform.Models.FltTimeGroupModels> Scheme = new List<Plexform.Models.FltTimeGroupModels>();
			try
			{
				var repo = new Plexform.GBS.GBSAdminLogic();
				Scheme = await repo.GroupTime(Filter);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return new ListResultContainer<Plexform.Models.FltTimeGroupModels>(
				ObjectMapper.Map<List<Plexform.Models.FltTimeGroupModels>>(Scheme),
				Scheme.Count
			);
		}
		[HttpGet]
		public async Task<ListResultContainer<Plexform.Models.AGENTACCESSFAREModels>> GetAgentAccessFareAll(string Filter = "")
		{
			IList<Plexform.Models.AGENTACCESSFAREModels> Scheme = new List<Plexform.Models.AGENTACCESSFAREModels>();
			try
			{
				var repo = new Plexform.GBS.GBSAdminLogic();
				Scheme = await repo.GetAgentAccessFareAll(Filter);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return new ListResultContainer<Plexform.Models.AGENTACCESSFAREModels>(
				ObjectMapper.Map<List<Plexform.Models.AGENTACCESSFAREModels>>(Scheme),
				Scheme.Count
			);
		}
		[HttpGet]
		public async Task<ListResultContainer<Plexform.Models.AGENTACCESSFAREModels>> GetAgentAccessFarePIVOT()
		{
			IList<Plexform.Models.AGENTACCESSFAREModels> Scheme = new List<Plexform.Models.AGENTACCESSFAREModels>();
			try
			{
				var repo = new Plexform.GBS.GBSAdminLogic();
				Scheme = await repo.GetAgentAccessFarePIVOT();
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return new ListResultContainer<Plexform.Models.AGENTACCESSFAREModels>(
				ObjectMapper.Map<List<Plexform.Models.AGENTACCESSFAREModels>>(Scheme),
				Scheme.Count
			);
		}
        [HttpGet]
        public async Task<ListResultContainer<Plexform.Models.AGENTTIERModels>> GetAgentTireListGrid()
        {
            IList<Plexform.Models.AGENTTIERModels> Scheme = new List<Plexform.Models.AGENTTIERModels>();
            try
            {
                var repo = new Plexform.GBS.GBSAdminLogic();
                Scheme = await repo.GetAgentTierGrid();
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return new ListResultContainer<Plexform.Models.AGENTTIERModels>(
                ObjectMapper.Map<List<Plexform.Models.AGENTTIERModels>>(Scheme),
                Scheme.Count
            );
        }
        [HttpGet]
        public async Task<ListResultContainer<Plexform.Models.GROUPCAPModels>> GetGroupCapAll()
        {
            IList<Plexform.Models.GROUPCAPModels> Scheme = new List<Plexform.Models.GROUPCAPModels>();
            try
            {
                var repo = new Plexform.GBS.GBSAdminLogic();
                Scheme = await repo.GetGroupCapAll();
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return new ListResultContainer<Plexform.Models.GROUPCAPModels>(
                ObjectMapper.Map<List<Plexform.Models.GROUPCAPModels>>(Scheme),
                Scheme.Count
            );
        }
        [HttpGet]
        public async Task<ListResultContainer<Plexform.Models.MAXDISCModels>> GetMaxDiscAll()
        {
            IList<Plexform.Models.MAXDISCModels> Scheme = new List<Plexform.Models.MAXDISCModels>();
            try
            {
                var repo = new Plexform.GBS.GBSAdminLogic();
                Scheme = await repo.GetMaxDiscAll();
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return new ListResultContainer<Plexform.Models.MAXDISCModels>(
                ObjectMapper.Map<List<Plexform.Models.MAXDISCModels>>(Scheme),
                Scheme.Count
            );
        }
        [HttpGet]
        public async Task<ListResultContainer<Plexform.Models.SEASONALITYModels>> GetSeasonalityAll()
        {
            IList<Plexform.Models.SEASONALITYModels> Scheme = new List<Plexform.Models.SEASONALITYModels>();
            try
            {
                var repo = new Plexform.GBS.GBSAdminLogic();
                Scheme = await repo.GetSeasonalityAll();
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return new ListResultContainer<Plexform.Models.SEASONALITYModels>(
                ObjectMapper.Map<List<Plexform.Models.SEASONALITYModels>>(Scheme),
                Scheme.Count
            );
        }
        [HttpGet]
        public async Task<ListResultContainer<Plexform.Models.DISCWEIGHTModels>> GetDiscWeightageAll()
        {
            IList<Plexform.Models.DISCWEIGHTModels> Scheme = new List<Plexform.Models.DISCWEIGHTModels>();
            try
            {
                var repo = new Plexform.GBS.GBSAdminLogic();
                Scheme = await repo.GetDiscWeightageAll();
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return new ListResultContainer<Plexform.Models.DISCWEIGHTModels>(
                ObjectMapper.Map<List<Plexform.Models.DISCWEIGHTModels>>(Scheme),
                Scheme.Count
            );
        }
        [HttpGet]
        public async Task<ListResultContainer<Plexform.Models.FLOORFAREModels>> GetFloorFareAll()
        {
            IList<Plexform.Models.FLOORFAREModels> Scheme = new List<Plexform.Models.FLOORFAREModels>();
            try
            {
                var repo = new Plexform.GBS.GBSAdminLogic();
                Scheme = await repo.GetFloorFareAll();
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return new ListResultContainer<Plexform.Models.FLOORFAREModels>(
                ObjectMapper.Map<List<Plexform.Models.FLOORFAREModels>>(Scheme),
                Scheme.Count
            );
        }
        [HttpGet]
        public async Task<ListResultContainer<Plexform.Models.LFDISCOUNTModels>> GetLFDiscountAll()
        {
            IList<Plexform.Models.LFDISCOUNTModels> Scheme = new List<Plexform.Models.LFDISCOUNTModels>();
            try
            {
                var repo = new Plexform.GBS.GBSAdminLogic();
                Scheme = await repo.GetLFDiscountAll();
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return new ListResultContainer<Plexform.Models.LFDISCOUNTModels>(
                ObjectMapper.Map<List<Plexform.Models.LFDISCOUNTModels>>(Scheme),
                Scheme.Count
            );
        }
        [HttpGet]
        public async Task<ListResultContainer<Plexform.Models.PUDISCOUNTModels>> GetPUDiscountAll()
        {
            IList<Plexform.Models.PUDISCOUNTModels> Scheme = new List<Plexform.Models.PUDISCOUNTModels>();
            try
            {
                var repo = new Plexform.GBS.GBSAdminLogic();
                Scheme = await repo.GetPUDiscountAll();
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return new ListResultContainer<Plexform.Models.PUDISCOUNTModels>(
                ObjectMapper.Map<List<Plexform.Models.PUDISCOUNTModels>>(Scheme),
                Scheme.Count
            );
        }
        [HttpGet]
        public async Task<ListResultContainer<Plexform.Models.SPECIALFAREModels>> GetSeriesAll()
        {
            IList<Plexform.Models.SPECIALFAREModels> Scheme = new List<Plexform.Models.SPECIALFAREModels>();
            try
            {
                var repo = new Plexform.GBS.GBSAdminLogic();
                Scheme = await repo.GetSeriesAll();
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return new ListResultContainer<Plexform.Models.SPECIALFAREModels>(
                ObjectMapper.Map<List<Plexform.Models.SPECIALFAREModels>>(Scheme),
                Scheme.Count
            );
        }
        [HttpGet]
        public async Task<ListResultContainer<Plexform.Models.SPECIALFAREModels>> GetUmrahLaborAll()
        {
            IList<Plexform.Models.SPECIALFAREModels> Scheme = new List<Plexform.Models.SPECIALFAREModels>();
            try
            {
                var repo = new Plexform.GBS.GBSAdminLogic();
                Scheme = await repo.GetUmrahLaborAll();
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return new ListResultContainer<Plexform.Models.SPECIALFAREModels>(
                ObjectMapper.Map<List<Plexform.Models.SPECIALFAREModels>>(Scheme),
                Scheme.Count
            );
        }
        [HttpGet]
        public async Task<ListResultContainer<Plexform.Models.MAXDISCModels>> GetMarketAll()
        {
            IList<Plexform.Models.MAXDISCModels> Scheme = new List<Plexform.Models.MAXDISCModels>();
            try
            {
                var repo = new Plexform.GBS.GBSAdminLogic();
                Scheme = await repo.GetMarketAll();
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return new ListResultContainer<Plexform.Models.MAXDISCModels>(
                ObjectMapper.Map<List<Plexform.Models.MAXDISCModels>>(Scheme),
                Scheme.Count
            );
        }
        [HttpGet]
        public async Task<ListResultContainer<Plexform.Models.CODEMASTERModels>> GetCodeMasterLFDiscount()
        {
            IList<Plexform.Models.CODEMASTERModels> Scheme = new List<Plexform.Models.CODEMASTERModels>();
            try
            {
                var repo = new Plexform.GBS.GBSAdminLogic();
                Scheme = await repo.GetCodeMasterbyCodeTypeAll("LFD");
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return new ListResultContainer<Plexform.Models.CODEMASTERModels>(
                ObjectMapper.Map<List<Plexform.Models.CODEMASTERModels>>(Scheme),
                Scheme.Count
            );
        }
        [HttpGet]
        public async Task<ListResultContainer<Plexform.Models.CODEMASTERModels>> GetCodeMasterPUDiscount()
        {
            IList<Plexform.Models.CODEMASTERModels> Scheme = new List<Plexform.Models.CODEMASTERModels>();
            try
            {
                var repo = new Plexform.GBS.GBSAdminLogic();
                Scheme = await repo.GetCodeMasterbyCodeTypeAll("PUD");
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return new ListResultContainer<Plexform.Models.CODEMASTERModels>(
                ObjectMapper.Map<List<Plexform.Models.CODEMASTERModels>>(Scheme),
                Scheme.Count
            );
        }
        [HttpGet]
        public async Task<ListResultContainer<Plexform.Models.CODEMASTERModels>> GetCodeMasterLFFare()
        {
            IList<Plexform.Models.CODEMASTERModels> Scheme = new List<Plexform.Models.CODEMASTERModels>();
            try
            {
                var repo = new Plexform.GBS.GBSAdminLogic();
                Scheme = await repo.GetCodeMasterbyCodeTypeAll("LFF");
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return new ListResultContainer<Plexform.Models.CODEMASTERModels>(
                ObjectMapper.Map<List<Plexform.Models.CODEMASTERModels>>(Scheme),
                Scheme.Count
            );
        }
		[HttpGet]
		public async Task<Plexform.Models.RestrictionModels> GetRestriction()
		{
			Plexform.Models.RestrictionModels Scheme = new Plexform.Models.RestrictionModels();
			try
			{
				var repo = new Plexform.GBS.GBSAdminLogic();
				Scheme = await repo.GetRestriction("RST", "RESTRICTIONNOTE", "RESTRICTIONALERT");
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return ObjectMapper.Map<Plexform.Models.RestrictionModels>(Scheme);
		}
		[HttpPut]
		public async Task<bool> UpdateRestriction([FromBody]Models.RestrictionModels input)
		{
			bool res = false;
			try
			{
				var repo = new Plexform.GBS.GBSAdminLogic();

				res = await repo.UpdateRestriction(input);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}

			return ObjectMapper.Map<bool>(res);
		}
		[HttpPut]
		public async Task<bool> UploadGroupTime([FromBody]Models.FltTimeGroupModels[] input)
		{
			bool res = false;
			try
			{
				var repo = new Plexform.GBS.GBSAdminLogic();

				res = await repo.UploadGroupTime(input);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}

			return ObjectMapper.Map<bool>(res);
		}
		[HttpPut]
		public async Task<bool> UploadAgentTier([FromBody]Models.FltTimeGroupModels[] input)
		{
			bool res = false;
			try
			{
				var repo = new Plexform.GBS.GBSAdminLogic();

				res = await repo.UploadAgentTier(input);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}

			return ObjectMapper.Map<bool>(res);
		}
		[HttpPut]
		public async Task<bool> UploadAgentFare([FromBody]Models.FltTimeGroupModels[] input)
		{
			bool res = false;
			try
			{
				var repo = new Plexform.GBS.GBSAdminLogic();

				res = await repo.UploadAgentFare(input);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}

			return ObjectMapper.Map<bool>(res);
		}
		[HttpPut]
		public async Task<bool> UploadDiscount([FromBody]Models.FltTimeGroupModels[] input)
		{
			bool res = false;
			try
			{
				var repo = new Plexform.GBS.GBSAdminLogic();

				res = await repo.UploadDiscount(input);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}

			return ObjectMapper.Map<bool>(res);
		}
		[HttpPut]
		public async Task<bool> UploadCapacity([FromBody]Models.FltTimeGroupModels[] input)
		{
			bool res = false;
			try
			{
				var repo = new Plexform.GBS.GBSAdminLogic();

				res = await repo.UploadCapacity(input);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}

			return ObjectMapper.Map<bool>(res);
		}
		[HttpPut]
		public async Task<bool> UploadDiscWeight([FromBody]Models.FltTimeGroupModels[] input)
		{
			bool res = false;
			try
			{
				var repo = new Plexform.GBS.GBSAdminLogic();

				res = await repo.UploadDiscWeight(input);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}

			return ObjectMapper.Map<bool>(res);
		}
		[HttpPut]
		public async Task<bool> UploadFloorFare([FromBody]Models.FltTimeGroupModels[] input)
		{
			bool res = false;
			try
			{
				var repo = new Plexform.GBS.GBSAdminLogic();

				res = await repo.UploadFloorFare(input);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}

			return ObjectMapper.Map<bool>(res);
		}
		[HttpPut]
		public async Task<bool> UploadSessionality([FromBody]Models.FltTimeGroupModels[] input)
		{
			bool res = false;
			try
			{
				var repo = new Plexform.GBS.GBSAdminLogic();

				res = await repo.UploadSeasonality(input);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}

			return ObjectMapper.Map<bool>(res);
		}
		[HttpPut]
		public async Task<bool> UploadLFDiscount([FromBody]Models.FltTimeGroupModels[] input)
		{
			bool res = false;
			try
			{
				var repo = new Plexform.GBS.GBSAdminLogic();

				res = await repo.UploadLFDiscount(input);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}

			return ObjectMapper.Map<bool>(res);
		}
		[HttpPut]
		public async Task<bool> UploadPUDiscount([FromBody]Models.FltTimeGroupModels[] input)
		{
			bool res = false;
			try
			{
				var repo = new Plexform.GBS.GBSAdminLogic();

				res = await repo.UploadPUDiscount(input);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}

			return ObjectMapper.Map<bool>(res);
		}
		[HttpPut]
		public async Task<bool> UploadSeries([FromBody]Models.FltTimeGroupModels[] input)
		{
			bool res = false;
			try
			{
				var repo = new Plexform.GBS.GBSAdminLogic();

				res = await repo.UploadSeries(input);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}

			return ObjectMapper.Map<bool>(res);
		}
		[HttpPut]
		public async Task<bool> UploadUmrahLabor([FromBody]Models.FltTimeGroupModels[] input)
		{
			bool res = false;
			try
			{
				var repo = new Plexform.GBS.GBSAdminLogic();

				res = await repo.UploadUmrahLabor(input);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}

			return ObjectMapper.Map<bool>(res);
		}
        [HttpGet]
        public async Task<ListResultContainer<Plexform.Models.GB4Models>> GetPaxSettingAll()
        {
            IList<Plexform.Models.GB4Models> Scheme = new List<Plexform.Models.GB4Models>();
            try
            {
                var repo = new Plexform.GBS.GBSAdminLogic();
                Scheme = await repo.GetPaxSettingAll();
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return new ListResultContainer<Plexform.Models.GB4Models>(
                ObjectMapper.Map<List<Plexform.Models.GB4Models>>(Scheme),
                Scheme.Count
            );
        }
        [HttpGet]
        public async Task<Plexform.Models.GB4Models> GetPaxSettingSingle(string CountryCode, string Origin, string OrgID, string AgentID)
        {
            Plexform.Models.GB4Models Scheme = new Plexform.Models.GB4Models();
            try
            {
                var repo = new Plexform.GBS.GBSAdminLogic();
                Scheme = await repo.GetPaxSettingSingle(CountryCode, Origin, OrgID, AgentID);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return ObjectMapper.Map<Plexform.Models.GB4Models>(Scheme);
        }
		[HttpGet]
		public async Task<ListResultContainer<Plexform.Models.GB4Models>> GetAllOrgID()
		{
			IList < Plexform.Models.GB4Models> Scheme = new List<Plexform.Models.GB4Models>();
			try
			{
				var repo = new Plexform.GBS.GBSAdminLogic();
				Scheme = await repo.GetAllOrgIDs();
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return new ListResultContainer<Plexform.Models.GB4Models>(
				ObjectMapper.Map<List<Plexform.Models.GB4Models>>(Scheme),
				Scheme.Count
			);
		}
		[HttpGet]
		public async Task<ListResultContainer<Plexform.Models.GB4Models>> GetAllAgentByOrgID()
		{
			IList<Plexform.Models.GB4Models> Scheme = new List<Plexform.Models.GB4Models>();
			try
			{
				var repo = new Plexform.GBS.GBSAdminLogic();
				Scheme = await repo.GetAllAgents();
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return new ListResultContainer<Plexform.Models.GB4Models>(
				ObjectMapper.Map<List<Plexform.Models.GB4Models>>(Scheme),
				Scheme.Count
			);
		}
		[HttpPut]
        public async Task<bool> UpdatePaxSetting([FromBody]Plexform.GBS.GBSAdminLogic.GB4SETTING input, [FromBody]Plexform.GBS.GBSAdminLogic.GB4SETTING inputOld)
        {
            bool res = false;
            try
            {
                var repo = new Plexform.GBS.GBSAdminLogic();

                res = await repo.UpdatePaxSetting(input, inputOld);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }

            return ObjectMapper.Map<bool>(res);
        }
        [HttpPut]
        public async Task<bool> SavePaxSetting([FromBody]Plexform.GBS.GBSAdminLogic.GB4SETTING input)
        {
            bool res = false;
            try
            {
                var repo = new Plexform.GBS.GBSAdminLogic();

                res = await repo.InsertPaxSetting(input);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }

            return ObjectMapper.Map<bool>(res);
        }
        [HttpPut]
        public async Task<bool> DeletePaxSetting([FromBody]List<Plexform.GBS.GBSAdminLogic.GB4SETTING> input)
        {
            bool res = false;
            try
            {
                var repo = new Plexform.GBS.GBSAdminLogic();

                res = await repo.DeletePaxSetting(input);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }

            return ObjectMapper.Map<bool>(res);
        }
        [HttpGet]
        public async Task<ListResultContainer<Plexform.Models.OriginModels>> GetAllOrigin()
        {
            IList<Plexform.Models.OriginModels> Scheme = new List<Plexform.Models.OriginModels>();
            try
            {
                var repo = new Plexform.GBS.GBSAdminLogic();
                Scheme = await repo.GetOrigin();
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return new ListResultContainer<Plexform.Models.OriginModels>(
                ObjectMapper.Map<List<Plexform.Models.OriginModels>>(Scheme),
                Scheme.Count
            );
        }
    }
}
