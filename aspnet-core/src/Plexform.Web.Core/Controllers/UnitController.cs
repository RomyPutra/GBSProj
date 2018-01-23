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
    public class UnitController : PlexformControllerBase
    {
        private readonly IRepository<Role> _roleRepository;

        public UnitController(IRepository<Role> roleRepository)
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
        public async Task<ListResultContainer<eSWIS.Logic.CodeMaster.Container.Unit>> GetAllUnit()
        {
            List<eSWIS.Logic.CodeMaster.Container.Unit> list = new List<eSWIS.Logic.CodeMaster.Container.Unit>();
            try
            {
                var repo = new Plexform.Logic.FooLogic();

                list = await repo.GetAllListAsync();
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }

            return new ListResultContainer<eSWIS.Logic.CodeMaster.Container.Unit>(
                ObjectMapper.Map<List<eSWIS.Logic.CodeMaster.Container.Unit>>(list),
                list.Count
            );
        }

        [HttpGet]
        public async Task<eSWIS.Logic.Profiles.Container.Company> GetCompany(string id)
        {
            eSWIS.Logic.Profiles.Container.Company list = new eSWIS.Logic.Profiles.Container.Company();
            try
            {
                var repo = new Plexform.Logic.FooLogic();

                list = await repo.GetCompanyAsync(id);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }

            return ObjectMapper.Map<eSWIS.Logic.Profiles.Container.Company>(list);
        }
    }
}
