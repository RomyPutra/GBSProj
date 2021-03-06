﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Domain.Repositories;
using Plexform.Authorization.Roles;
using Plexform.Roles.Dto;

namespace Plexform.Controllers
{
    //[AbpAuthorize(PermissionNames.Pages_Users)]
    [Route("api/eswis/[controller]/[action]")]
    public class UserGroupController : PlexformControllerBase
    {
        private readonly IRepository<Role> _roleRepository;

        public UserGroupController(IRepository<Role> roleRepository)
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
        public async Task<eSWIS.Logic.UserSecurity.Container.UserGroup> GetGetUserGroup(string GroupCode)
        {
            eSWIS.Logic.UserSecurity.Container.UserGroup list = new eSWIS.Logic.UserSecurity.Container.UserGroup();
            try
            {
                var repo = new Plexform.Logic.UserGroupLogic();

                list = await repo.GetUserGroupByID(GroupCode);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }

            return ObjectMapper.Map<eSWIS.Logic.UserSecurity.Container.UserGroup>(list);
        }

		[HttpPost]
		public async Task<bool> Create([FromBody]eSWIS.Logic.UserSecurity.Container.UserGroup input)
		{
			bool res = false;
			try
			{
				var repo = new Plexform.Logic.UserGroupLogic();

				res = await repo.Insert(input);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}

			return ObjectMapper.Map<bool>(res);
		}

		[HttpPut]
		public async Task<bool> Update([FromBody]eSWIS.Logic.UserSecurity.Container.UserGroup input)
		{
			bool res = false;
			try
			{
				var repo = new Plexform.Logic.UserGroupLogic();

				res = await repo.Update(input);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}

			return ObjectMapper.Map<bool>(res);
		}

		[HttpDelete]
		public async Task<bool> Delete([FromBody]eSWIS.Logic.UserSecurity.Container.UserGroup input)
		{
			bool res = false;
			try
			{
				var repo = new Plexform.Logic.UserGroupLogic();

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
