using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Plexform.Configuration;
using Abp.Reflection.Extensions;
using System;
using System.Globalization;

namespace Plexform.Logic
{
    public class UserGroupLogic
    {
        private readonly IConfigurationRoot _appConfiguration;

        public UserGroupLogic()
        {
            _appConfiguration = AppConfigurations.Get(
                typeof(UserGroupLogic).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public Task<eSWIS.Logic.UserSecurity.Container.UserGroup> GetUserGroupByID(string GroupCode)
        {
            eSWIS.Logic.UserSecurity.Container.UserGroup list = new eSWIS.Logic.UserSecurity.Container.UserGroup();
            try
            {
                string conn = _appConfiguration.GetConnectionString(PlexformConsts.ESWISConnectionString);
                eSWIS.Logic.UserSecurity.UserGroup obj = new eSWIS.Logic.UserSecurity.UserGroup(conn);
                list = obj.GetUserGroup(GroupCode);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return Task.FromResult(list);
        }

		public Task<bool> Update(eSWIS.Logic.UserSecurity.Container.UserGroup cont)
		{
			var res = false;
			try
			{
				string conn = _appConfiguration.GetConnectionString(PlexformConsts.ESWISConnectionString);
				eSWIS.Logic.UserSecurity.UserGroup obj = new eSWIS.Logic.UserSecurity.UserGroup(conn);
				string message = "";
				res = obj.Update(cont, ref message);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return Task.FromResult(res);
		}

		public Task<bool> Insert(eSWIS.Logic.UserSecurity.Container.UserGroup cont)
		{
			var res = false;
			try
			{
				string conn = _appConfiguration.GetConnectionString(PlexformConsts.ESWISConnectionString);
				eSWIS.Logic.UserSecurity.UserGroup obj = new eSWIS.Logic.UserSecurity.UserGroup(conn);
				string message = "";
				res = obj.Insert(cont, ref message);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return Task.FromResult(res);
		}

		public Task<bool> Delete(eSWIS.Logic.UserSecurity.Container.UserGroup cont)
		{
			var res = false;
			try
			{
				string conn = _appConfiguration.GetConnectionString(PlexformConsts.ESWISConnectionString);
				eSWIS.Logic.UserSecurity.UserGroup obj = new eSWIS.Logic.UserSecurity.UserGroup(conn);
				string message = "";
				res = obj.Delete(cont, ref message);
			}
			catch (Exception ex)
			{
				var temp = ex.ToString();
			}
			return Task.FromResult(res);
		}
		
		//public Task<List<eSWIS.Logic.UserSecurity.Container.UserGroup>> GetAllUserGroupAsync()
				 //{
				 //    List<eSWIS.Logic.UserSecurity.Container.UserGroup> list = new List<eSWIS.Logic.UserSecurity.Container.UserGroup>();
				 //    try
				 //    {
				 //        string conn = _appConfiguration.GetConnectionString(PlexformConsts.ESWISConnectionString);
				 //        eSWIS.Logic.UserSecurity.UserGroup obj = new eSWIS.Logic.UserSecurity.UserGroup(conn);
				 //        list = obj.GetUserGroup();
				 //    }
				 //    catch (Exception ex)
				 //    {
				 //        var temp = ex.ToString();
				 //    }
				 //    return Task.FromResult(list);
				 //}

	}
}
