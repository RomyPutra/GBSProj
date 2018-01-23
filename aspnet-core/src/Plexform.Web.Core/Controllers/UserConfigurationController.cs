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
    public class UserConfigurationController : PlexformControllerBase
    {
        private readonly IRepository<Role> _roleRepository;

        public UserConfigurationController(IRepository<Role> roleRepository)
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
        public async Task<Logic.Models.AuthEswisModel> GetAllPermission(string userID)
        {
            Logic.Models.AuthEswisModel res = new Logic.Models.AuthEswisModel();
            try
            {
                var repo = new Plexform.Logic.PermissionLogic();

                res = await repo.GetAllPermissionSet(userID);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return ObjectMapper.Map<Logic.Models.AuthEswisModel>(res);
            //return ObjectMapper.Map<List<eSWIS.Logic.UserSecurity.Container.PermissionSet>>(list);
        }
    }
}
