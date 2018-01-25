using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Domain.Repositories;
using Plexform.Authorization.Roles;
using System.Collections.Generic;

namespace Plexform.Controllers
{
	[Route("api/[controller]/[action]")]
	public class GBSController : PlexformControllerBase
	{
		private readonly IRepository<Role> _roleRepository;

		public GBSController(IRepository<Role> roleRepository)
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
		public async Task<ListResultContainer<Plexform.Models.GBSModels>> GetBookingByPNR(string ID)
		{
			IList<Plexform.Models.GBSModels> signature = new List<Plexform.Models.GBSModels>();
			//string signature = "";
			try
			{
				var repo = new Plexform.GBS.GBSRepository();
				signature = await repo.GetBookingByPNR(ID);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			//return ObjectMapper.Map<IList<Plexform.Models.GBSModels>>(signature);
			return new ListResultContainer<Plexform.Models.GBSModels>(
				ObjectMapper.Map<List<Plexform.Models.GBSModels>>(signature),
				signature.Count
			);
		}

		[HttpGet]
		public async Task<ListResultContainer<Plexform.Models.PaymentSchemeModels>> PaymentScheme(string GRPID = "AA")
		{
			IList<Plexform.Models.PaymentSchemeModels> Scheme = new List<Plexform.Models.PaymentSchemeModels>();
			try
			{
				var repo = new Plexform.GBS.GBSRepository();
				Scheme = await repo.GetAllScheme(GRPID);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			//return ObjectMapper.Map<IList<Plexform.Models.GBSModels>>(signature);
			return new ListResultContainer<Plexform.Models.PaymentSchemeModels>(
				ObjectMapper.Map<List<Plexform.Models.PaymentSchemeModels>>(Scheme),
				Scheme.Count
			);
		}
		[HttpGet]
		public async Task<ListResultContainer<Plexform.Models.PaymentSchemeModels>> GetPaymentScheme(string GRPID = "AA", string CountryCode = "", string SchemeCode = "")
		{
			IList<Plexform.Models.PaymentSchemeModels> Scheme = new List<Plexform.Models.PaymentSchemeModels>();
			try
			{
				var repo = new Plexform.GBS.GBSRepository();
				Scheme = await repo.GetSchemeByCode(GRPID,CountryCode,SchemeCode);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			//return ObjectMapper.Map<IList<Plexform.Models.GBSModels>>(signature);
			return new ListResultContainer<Plexform.Models.PaymentSchemeModels>(
				ObjectMapper.Map<List<Plexform.Models.PaymentSchemeModels>>(Scheme),
				Scheme.Count
			);
		}
		[HttpPut]
		public async Task<bool> Update([FromBody]ABS.Logic.GroupBooking.Booking.PaymentInfo input)
		{
			bool res = false;
			try
			{
				var repo = new Plexform.GBS.GBSRepository();

				res = await repo.Update(input);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}

			return ObjectMapper.Map<bool>(res);
		}
	}
}
