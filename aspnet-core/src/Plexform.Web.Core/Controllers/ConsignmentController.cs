using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using Abp.UI;
using Plexform.Authentication.External;
using Plexform.Authentication.JwtBearer;
using Plexform.Authorization;
using Plexform.Authorization.Users;
using Plexform.Models.TokenAuth;
using Plexform.MultiTenancy;
using eSWIS.Logic.CodeMaster;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Plexform.Authorization.Roles;
using Plexform.Roles.Dto;

namespace Plexform.Controllers
{
    //[AbpAuthorize(PermissionNames.Pages_Users)]
    [Route("api/eswis/[controller]/[action]")]
    public class ConsignmentController : PlexformControllerBase
    {
        private readonly IRepository<Role> _roleRepository;

        public ConsignmentController(IRepository<Role> roleRepository)
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
        public async Task<ListResultContainer<eSWIS.Logic.Actions.Container.Consignhdr>> GetAll(int skipCount = -1, int limit = -1)
        {
            Tuple<int, IList<eSWIS.Logic.Actions.Container.Consignhdr>> res = new Tuple<int, IList<eSWIS.Logic.Actions.Container.Consignhdr>>(0, new List<eSWIS.Logic.Actions.Container.Consignhdr>());
            try
            {
                var repo = new Plexform.Logic.ConsignmentLogic();

                res = await repo.GetAll(skipCount, limit);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return new ListResultContainer<eSWIS.Logic.Actions.Container.Consignhdr>(
                ObjectMapper.Map<IList<eSWIS.Logic.Actions.Container.Consignhdr>>(res.Item2),
                res.Item1 > 0 ? res.Item1 : res.Item2.Count()
            );
        }

    }
}
