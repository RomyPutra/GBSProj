using Abp.Reflection.Extensions;
using Microsoft.Extensions.Configuration;
using Plexform.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plexform.Logic
{
    public class CustomerLogic
    {
        private readonly IConfigurationRoot _appConfiguration;

        public CustomerLogic()
        {
            _appConfiguration = AppConfigurations.Get(
                typeof(CustomerLogic).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public Task<eSWIS.Logic.Profiles.Container.Company> GetCustomer(string bizRegID)
        {
            eSWIS.Logic.Profiles.Container.Company list = new eSWIS.Logic.Profiles.Container.Company();
            try
            {
                string conn = _appConfiguration.GetConnectionString(PlexformConsts.ESWISConnectionString);
                eSWIS.Logic.Profiles.Company obj = new eSWIS.Logic.Profiles.Company(conn);
                list = obj.GetCompany(bizRegID);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }

            return Task.FromResult(list);
        }

        public Task<List<eSWIS.Logic.Profiles.Container.Company>> GetAllCustomer(int limit)
        {
            List<eSWIS.Logic.Profiles.Container.Company> list = new List<eSWIS.Logic.Profiles.Container.Company>();
            try
            {
                string conn = _appConfiguration.GetConnectionString(PlexformConsts.ESWISConnectionString);
                eSWIS.Logic.Profiles.Company obj = new eSWIS.Logic.Profiles.Company(conn);
                //list = obj.GetAllCompany(limit);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }

            return Task.FromResult(list);
        }
    }
}
