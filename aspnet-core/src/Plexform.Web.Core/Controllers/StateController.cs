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
    public class StateController : PlexformControllerBase
    {
        private readonly IRepository<Role> _roleRepository;

        public StateController(IRepository<Role> roleRepository)
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

        //[HttpGet]
        //public async Task<ListResultContainer<eSWIS.Logic.GeneralSettings.Container.State>> GetAllState()
        //{
        //    List<eSWIS.Logic.GeneralSettings.Container.State> list = new List<eSWIS.Logic.GeneralSettings.Container.State>();
        //    try
        //    {
        //        var repo = new Plexform.Logic.StateLogic();

        //        list = await repo.GetAllStateAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        var temp = ex.ToString();
        //    }

        //    return new ListResultContainer<eSWIS.Logic.GeneralSettings.Container.State>(
        //        ObjectMapper.Map<List<eSWIS.Logic.GeneralSettings.Container.State>>(list),
        //        list.Count
        //    );
        //}

        [HttpGet]
        public async Task<eSWIS.Logic.GeneralSettings.Container.State> GetState(string CountryCode, string StateCode)
        {
            eSWIS.Logic.GeneralSettings.Container.State list = new eSWIS.Logic.GeneralSettings.Container.State();
            try
            {
                var repo = new Plexform.Logic.StateLogic();

                list = await repo.GetStateAsync(CountryCode, StateCode);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }

            return ObjectMapper.Map<eSWIS.Logic.GeneralSettings.Container.State>(list);

        }

        [HttpPost]
        public async Task<bool> Create([FromBody]eSWIS.Logic.GeneralSettings.Container.State input)
        {
            bool res = false;
            try
            {
                var repo = new Plexform.Logic.StateLogic();

                res = await repo.Insert(input);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }

            return ObjectMapper.Map<bool>(res);
        }

        [HttpPut]
        public async Task<bool> Update([FromBody]eSWIS.Logic.GeneralSettings.Container.State input)
        {
            bool res = false;
            try
            {
                var repo = new Plexform.Logic.StateLogic();

                res = await repo.Update(input);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }

            return ObjectMapper.Map<bool>(res);
        }

        [HttpDelete]
        public async Task<bool> Delete([FromBody]eSWIS.Logic.GeneralSettings.Container.State input)
        {
            bool res = false;
            try
            {
                var repo = new Plexform.Logic.StateLogic();

                res = await repo.Delete(input);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }

            return ObjectMapper.Map<bool>(res);
        }
    }
}
