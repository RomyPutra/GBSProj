using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Plexform.Configuration;
using Abp.Reflection.Extensions;
using System;
using System.Globalization;

namespace Plexform.Logic
{
    public class PermissionLogic
    {
        private readonly IConfigurationRoot _appConfiguration;

        public PermissionLogic()
        {
            _appConfiguration = AppConfigurations.Get(
                typeof(PermissionLogic).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public Task<bool> Update(eSWIS.Logic.UserSecurity.Container.PermissionSet[] permissions)
        {
            var res = false;
            try
            {
                //IList<eSWIS.Logic.UserSecurity.Container.PermissionSet> list = new List<eSWIS.Logic.UserSecurity.Container.PermissionSet>();
                //foreach (var r in permissions)
                //{
                //    //foreach(var y in r.Functions)
                //    //{
                //    //    list.Add(y);
                //    //}
                //}
                string conn = _appConfiguration.GetConnectionString(PlexformConsts.ESWISConnectionString);
                eSWIS.Logic.UserSecurity.PermissionSet obj = new eSWIS.Logic.UserSecurity.PermissionSet(conn);
                res = obj.Update(permissions);
               
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return Task.FromResult(res);
        }

        public Task<IList<Plexform.Logic.Models.PermissionSetModel>> GetAllPermission(string accessCode)
        {
            IList<eSWIS.Logic.UserSecurity.Container.PermissionSet> list = new List<eSWIS.Logic.UserSecurity.Container.PermissionSet>();
            IList<Plexform.Logic.Models.PermissionSetModel> res = new List<Plexform.Logic.Models.PermissionSetModel>();
            try
            {
                string conn = _appConfiguration.GetConnectionString(PlexformConsts.ESWISConnectionString);
                eSWIS.Logic.UserSecurity.PermissionSet obj = new eSWIS.Logic.UserSecurity.PermissionSet(conn);
                list = obj.GetPermissionBy(1, string.Empty, accessCode, false);
                Models.PermissionSetModel permissionSet = null;
                for (int i = 0; i < list.Count; i++)
                {
                    var r = list[i];
                    if(permissionSet == null)
                    {
                        permissionSet = new Models.PermissionSetModel
                        {
                            ModuleID = r.ModuleID,
                            ModuleName = r.ModuleName,
                            Expanded = true,
                        };
                        permissionSet.Functions = new List<eSWIS.Logic.UserSecurity.Container.PermissionSet>();
                    }
                    if(permissionSet.ModuleID != r.ModuleID)
                    {
                        if(permissionSet.Functions.Count > 0)
                            res.Add(permissionSet);
                        permissionSet = new Models.PermissionSetModel
                        {
                            ModuleID = r.ModuleID,
                            ModuleName = r.ModuleName,
                            Expanded = true,
                        };
                        permissionSet.Functions = new List<eSWIS.Logic.UserSecurity.Container.PermissionSet>();
                    }    
                    permissionSet.Functions.Add(r);
                }
                if (permissionSet.Functions.Count > 0)
                    res.Add(permissionSet);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return Task.FromResult(res);
        }

        public Task<Models.AuthEswisModel> GetAllPermissionSet(string userID)
        {
            Models.AuthEswisModel mdl = new Models.AuthEswisModel();
            IList<eSWIS.Logic.UserSecurity.Container.PermissionSet> list = new List<eSWIS.Logic.UserSecurity.Container.PermissionSet>();
            try
            {
                string conn = _appConfiguration.GetConnectionString(PlexformConsts.ESWISConnectionString);
                eSWIS.Logic.UserSecurity.PermissionSet obj = new eSWIS.Logic.UserSecurity.PermissionSet(conn);
                list = obj.GetPermissionBy(1, userID, string.Empty, false);

                Dictionary<string, bool> permissions = new Dictionary<string, bool>();
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

                foreach (var r in list)
                {
                    string key = string.Format("{0}#{1}", textInfo.ToTitleCase(r.ModuleName.ToLower()).Replace(' ', '_'), textInfo.ToTitleCase(r.FunctionName.ToLower()).Replace(' ', '_'));
                    if(!permissions.ContainsKey(key))
                        permissions.Add(key, true);
                }

                mdl = new Models.AuthEswisModel
                {
                    AppID = 1,
                    AccessCode = userID,
                    Permissions = permissions 
                };
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return Task.FromResult(mdl);
        }
    }
}
