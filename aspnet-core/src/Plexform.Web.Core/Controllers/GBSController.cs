using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Plexform.Controllers
{
	[Route("api/[controller]/[action]")]
	public class GBSController : PlexformControllerBase
	{
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
		public async Task<ListResultContainer<Plexform.Models.PaymentSchemeModels>> PaymentScheme(string GRPID = "AA", string TransID = "12345")
		{
			IList<Plexform.Models.PaymentSchemeModels> Scheme = new List<Plexform.Models.PaymentSchemeModels>();
			try
			{
				var repo = new Plexform.GBS.GBSRepository();
				Scheme = await repo.GetAllScheme(GRPID, TransID);
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
	}
}
