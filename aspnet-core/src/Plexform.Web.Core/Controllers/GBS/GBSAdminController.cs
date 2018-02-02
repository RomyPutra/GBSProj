using Abp.Domain.Repositories;
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
    }
}
