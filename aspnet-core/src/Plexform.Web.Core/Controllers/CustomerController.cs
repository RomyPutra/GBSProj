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
    public class CustomerController : PlexformControllerBase
    {
        private readonly IRepository<Role> _roleRepository;

        public CustomerController(IRepository<Role> roleRepository)
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
        public async Task<eSWIS.Logic.Profiles.Container.Company> Get(string BizRegID)
        {
            eSWIS.Logic.Profiles.Container.Company list = new eSWIS.Logic.Profiles.Container.Company();
            try
            {
                var repo = new Logic.CustomerLogic();

                list = await repo.GetCustomer(BizRegID);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }

            return ObjectMapper.Map<eSWIS.Logic.Profiles.Container.Company>(list);
        }

        [HttpGet]
        public async Task<ListResultContainer<eSWIS.Logic.Profiles.Container.Company>> GetAll(int limit = 50)
        {
            List<eSWIS.Logic.Profiles.Container.Company> list = new List<eSWIS.Logic.Profiles.Container.Company>();
            try
            {
                var repo = new Logic.CustomerLogic();

                list = await repo.GetAllCustomer(limit);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }

            return new ListResultContainer<eSWIS.Logic.Profiles.Container.Company>(
                ObjectMapper.Map<List<eSWIS.Logic.Profiles.Container.Company>>(list),
                list.Count
            );
        }
    }
}
