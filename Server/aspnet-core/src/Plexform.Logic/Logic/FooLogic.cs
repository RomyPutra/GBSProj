using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using Abp.Localization;
using Abp.Runtime.Session;
using Plexform.Authorization;
using Plexform.Authorization.Roles;
using Plexform.Authorization.Users;
using Microsoft.Extensions.Configuration;
using Plexform.Configuration;
using Abp.Reflection.Extensions;
using System;

namespace Plexform.Logic
{
    public class FooLogic
    {
        private readonly IConfigurationRoot _appConfiguration;

        public FooLogic()
        {
            _appConfiguration = AppConfigurations.Get(
                typeof(FooLogic).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public Task<List<eSWIS.Logic.CodeMaster.Container.Unit>> GetAllListAsync()
        {
            List<eSWIS.Logic.CodeMaster.Container.Unit> list = new List<eSWIS.Logic.CodeMaster.Container.Unit>();
            try
            {
                string conn = _appConfiguration.GetConnectionString(PlexformConsts.ESWISConnectionString);
                eSWIS.Logic.CodeMaster.Unit obj = new eSWIS.Logic.CodeMaster.Unit(conn);
                list = obj.GetUnit();
                //eSWIS.Logic.CodeMaster.Container.Unit unit = new eSWIS.Logic.CodeMaster.Container.Unit()
                //{
                //    Active = 1,
                //    CreateBy = "",
                //    CreateDate = DateTime.Now,
                //    Flag = 1,
                //};

                //list.Add(unit);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return Task.FromResult(list);
        }

        public Task<eSWIS.Logic.Profiles.Container.Company> GetCompanyAsync(string id)
        {
            eSWIS.Logic.Profiles.Container.Company list = new eSWIS.Logic.Profiles.Container.Company();
            try
            {
                string conn = _appConfiguration.GetConnectionString(PlexformConsts.ESWISConnectionString);
                eSWIS.Logic.Profiles.Company obj = new eSWIS.Logic.Profiles.Company(conn);
                list = obj.GetCompany(id);
                //eSWIS.Logic.CodeMaster.Container.Unit unit = new eSWIS.Logic.CodeMaster.Container.Unit()
                //{
                //    Active = 1,
                //    CreateBy = "",
                //    CreateDate = DateTime.Now,
                //    Flag = 1,
                //};

                //list.Add(unit);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return Task.FromResult(list);
        }

        //public override async Task<List<eSWIS.Logic.UserSecurity.Container.UserGroup>> CreateUserGroupAsync(List<eSWIS.Logic.UserSecurity.Container.UserGroup>, Insert as string = "")
        //{
        //	List<eSWIS.Logic.UserSecurity.Container.UserGroup> list = new List<eSWIS.Logic.UserSecurity.Container.UserGroup>();
        //	try
        //	{
        //		string conn = _appConfiguration.GetConnectionString(PlexformConsts.ESWISConnectionString);
        //		eSWIS.Logic.UserSecurity.UserGroup obj = new eSWIS.Logic.UserSecurity.UserGroup(conn);
        //		list = obj.Save();
        //	}
        //	catch (Exception ex)
        //	{
        //		var temp = ex.ToString();
        //	}
        //	return Task.FromResult(list);
        //}

    }
}
