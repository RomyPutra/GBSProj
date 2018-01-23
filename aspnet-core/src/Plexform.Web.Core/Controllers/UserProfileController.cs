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
    //[AbpAuthorize(PermissionNames.Page_Security_UserProfile)]
    [Route("api/eswis/[controller]/[action]")]
    public class UserProfileController : PlexformControllerBase
    {
        private readonly IRepository<Role> _roleRepository;

        public UserProfileController(IRepository<Role> roleRepository)
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
        public async Task<ListResultContainer<eSWIS.Logic.UsrProfile.Container.USRPROFILE>> GetAllUserProfile(int skipCount = -1, int limit = -1, string orderby = "", string filter = "")
        {
            IList<eSWIS.Logic.UsrProfile.Container.USRPROFILE> res = new List<eSWIS.Logic.UsrProfile.Container.USRPROFILE>();
            int totalRows = 0;
            try
            {
                var repo = new Plexform.Logic.UserProfileLogic();

                res = await repo.GetAllUserProfileAsync(ref totalRows, skipCount, limit, orderby, filter);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return new ListResultContainer<eSWIS.Logic.UsrProfile.Container.USRPROFILE>(
                ObjectMapper.Map<IList<eSWIS.Logic.UsrProfile.Container.USRPROFILE>>(res),
                totalRows > 0 ? totalRows : res.Count()
            );
            
        }

        [HttpPost]
        public async Task<bool> Create([FromBody]eSWIS.Logic.UsrProfile.Container.USRPROFILE input)
        {
            bool res = false;
            try
            {
                var repo = new Plexform.Logic.UserProfileLogic();

                res = await repo.Insert(input);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }

            return ObjectMapper.Map<bool>(res);
        }

        [HttpPut]
        public async Task<bool> Update([FromBody]eSWIS.Logic.UsrProfile.Container.USRPROFILE input)
        {
            bool res = false;
            try
            {
                var repo = new Plexform.Logic.UserProfileLogic();

                res = await repo.Update(input);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }

            return ObjectMapper.Map<bool>(res);
        }

        [HttpDelete]
        public async Task<bool> Delete(string id)
        {
            bool res = false;
            try
            {
                var repo = new Plexform.Logic.UserProfileLogic();

                res = await repo.Delete(id);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }

            return ObjectMapper.Map<bool>(res);
        }
    }
}
