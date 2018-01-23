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
    public class PermissionController : PlexformControllerBase
    {
        private readonly IRepository<Role> _roleRepository;

        public PermissionController(IRepository<Role> roleRepository)
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
        public async Task<ListResultContainer<Plexform.Logic.Models.PermissionSetModel>> GetAll(string accessCode)
        {
            IList<Plexform.Logic.Models.PermissionSetModel> list = new List<Plexform.Logic.Models.PermissionSetModel>();
            try
            {
                var repo = new Plexform.Logic.PermissionLogic();

                list = await repo.GetAllPermission(accessCode);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return new ListResultContainer<Plexform.Logic.Models.PermissionSetModel>(
                ObjectMapper.Map<List<Plexform.Logic.Models.PermissionSetModel>>(list),
                list.Count
            );
        }

        [HttpPut]
        public async Task<bool> Update([FromBody]eSWIS.Logic.UserSecurity.Container.PermissionSet[] permissionSet)
        {
            var res = false;
            try
            {
                var repo = new Plexform.Logic.PermissionLogic();

                res = await repo.Update(permissionSet);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return ObjectMapper.Map<bool>(res);
        }
    }
}
