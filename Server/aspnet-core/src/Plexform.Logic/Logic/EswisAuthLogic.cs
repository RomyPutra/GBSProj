using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Plexform.Configuration;
using Abp.Reflection.Extensions;
using System;
using System.Globalization;

namespace Plexform.Logic
{
    public class EswisAuthLogic
    {
        private readonly IConfigurationRoot _appConfiguration;

        public EswisAuthLogic()
        {
            _appConfiguration = AppConfigurations.Get(
                typeof(EswisAuthLogic).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public Task<eSWIS.Logic.UsrProfile.Container.USRPROFILE> Authenticate(string userName, string password = "")
        {
            eSWIS.Logic.UsrProfile.Container.USRPROFILE usrProfile = new eSWIS.Logic.UsrProfile.Container.USRPROFILE();
            try
            {
                string conn = _appConfiguration.GetConnectionString(PlexformConsts.ESWISConnectionString);
                eSWIS.Logic.UsrProfile.UsrProfile obj = new eSWIS.Logic.UsrProfile.UsrProfile(conn);
                usrProfile = obj.GetUserProfile(userName, password);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return Task.FromResult(usrProfile);
        }

    }
}
