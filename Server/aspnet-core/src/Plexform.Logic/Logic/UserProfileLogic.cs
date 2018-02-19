using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Plexform.Configuration;
using Abp.Reflection.Extensions;
using System;
using System.Globalization;

namespace Plexform.Logic
{
    public class UserProfileLogic
    {
        private readonly IConfigurationRoot _appConfiguration;

        public UserProfileLogic()
        {
            _appConfiguration = AppConfigurations.Get(
                typeof(UserProfileLogic).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public Task<IList<eSWIS.Logic.UsrProfile.Container.USRPROFILE>> GetAllUserProfileAsync(ref int totalRows, int skipCount = -1, int limit = -1, string orderby = "", string filter = "")
        {
            IList<eSWIS.Logic.UsrProfile.Container.USRPROFILE> res = new List<eSWIS.Logic.UsrProfile.Container.USRPROFILE>();
            try
            {
                string conn = _appConfiguration.GetConnectionString(PlexformConsts.ESWISConnectionString);
                eSWIS.Logic.UsrProfile.UsrProfile obj = new eSWIS.Logic.UsrProfile.UsrProfile(conn);
                res = obj.GetUserProfile(skipCount, limit, orderby, filter, ref totalRows);
                //res = new Tuple<int, IList<eSWIS.Logic.UsrProfile.Container.USRPROFILE>>(totalRows, list);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return Task.FromResult(res);
        }

        public Task<bool> Update(eSWIS.Logic.UsrProfile.Container.USRPROFILE cont)
        {
            var res = false;
            try
            {
                string conn = _appConfiguration.GetConnectionString(PlexformConsts.ESWISConnectionString);
                eSWIS.Logic.UsrProfile.UsrProfile obj = new eSWIS.Logic.UsrProfile.UsrProfile(conn);
                res = obj.Update(cont);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return Task.FromResult(res);
        }

        public Task<bool> Insert(eSWIS.Logic.UsrProfile.Container.USRPROFILE cont)
        {
            var res = false;
            try
            {
                string conn = _appConfiguration.GetConnectionString(PlexformConsts.ESWISConnectionString);
                eSWIS.Logic.UsrProfile.UsrProfile obj = new eSWIS.Logic.UsrProfile.UsrProfile(conn);
                res = obj.Insert(cont);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return Task.FromResult(res);
        }

        public Task<bool> Delete(string id)
        {
            var res = false;
            try
            {
                string conn = _appConfiguration.GetConnectionString(PlexformConsts.ESWISConnectionString);
                eSWIS.Logic.UsrProfile.UsrProfile obj = new eSWIS.Logic.UsrProfile.UsrProfile(conn);
                res = obj.Delete(id);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return Task.FromResult(res);
        }
    
    }
}
