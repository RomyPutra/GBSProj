using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Plexform.Configuration;
using Abp.Reflection.Extensions;
using System;
using System.Globalization;

namespace Plexform.Logic
{
    public class StateLogic
    {
        private readonly IConfigurationRoot _appConfiguration;

        public StateLogic()
        {
            _appConfiguration = AppConfigurations.Get(
                typeof(UserProfileLogic).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        //public Task<List<eSWIS.Logic.GeneralSettings.Container.State>> GetAllStateAsync()
        //{
        //    List<eSWIS.Logic.GeneralSettings.Container.State> list = new List<eSWIS.Logic.GeneralSettings.Container.State>();
        //    try
        //    {
        //        string conn = _appConfiguration.GetConnectionString(PlexformConsts.ESWISConnectionString);
        //        eSWIS.Logic.GeneralSettings.State obj = new eSWIS.Logic.GeneralSettings.State(conn);
        //        //list = obj.GetState();
        //    }
        //    catch (Exception ex)
        //    {
        //        var temp = ex.ToString();
        //    }
        //    return Task.FromResult(list);
        //}

        public Task<eSWIS.Logic.GeneralSettings.Container.State> GetStateAsync(string CountryCode, string StateCode)
        {
            eSWIS.Logic.GeneralSettings.Container.State list = new eSWIS.Logic.GeneralSettings.Container.State();
            try
            {
                string conn = _appConfiguration.GetConnectionString(PlexformConsts.ESWISConnectionString);
                eSWIS.Logic.GeneralSettings.State obj = new eSWIS.Logic.GeneralSettings.State(conn);
                list = obj.GetState(CountryCode, StateCode);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return Task.FromResult(list);
        }

        public Task<bool> Update(eSWIS.Logic.GeneralSettings.Container.State cont)
        {
            var res = false;
            try
            {
                string conn = _appConfiguration.GetConnectionString(PlexformConsts.ESWISConnectionString);
                eSWIS.Logic.GeneralSettings.State obj = new eSWIS.Logic.GeneralSettings.State(conn);
				string message = "";
                res = obj.Update(cont,ref message);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return Task.FromResult(res);
        }

        public Task<bool> Insert(eSWIS.Logic.GeneralSettings.Container.State cont)
        {
            var res = false;
            try
            {
                string conn = _appConfiguration.GetConnectionString(PlexformConsts.ESWISConnectionString);
                eSWIS.Logic.GeneralSettings.State obj = new eSWIS.Logic.GeneralSettings.State(conn);
				string message = "";
				res = obj.Insert(cont, ref message);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return Task.FromResult(res);
        }

        public Task<bool> Delete(eSWIS.Logic.GeneralSettings.Container.State cont)
        {
            var res = false;
            try
            {
                string conn = _appConfiguration.GetConnectionString(PlexformConsts.ESWISConnectionString);
                eSWIS.Logic.GeneralSettings.State obj = new eSWIS.Logic.GeneralSettings.State(conn);
				string message = "";
				res = obj.Delete(cont, ref message);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return Task.FromResult(res);
        }

    }
}

