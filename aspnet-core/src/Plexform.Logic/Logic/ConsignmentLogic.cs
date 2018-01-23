using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Plexform.Configuration;
using Abp.Reflection.Extensions;
using System;
using System.Globalization;

namespace Plexform.Logic
{
    public class ConsignmentLogic
    {
        private readonly IConfigurationRoot _appConfiguration;

        public ConsignmentLogic()
        {
            _appConfiguration = AppConfigurations.Get(
                typeof(ConsignmentLogic).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public Task<Tuple<int, IList<eSWIS.Logic.Actions.Container.Consignhdr>>> GetAll(int skipCount, int limit)
        {
            Tuple<int, IList<eSWIS.Logic.Actions.Container.Consignhdr>> res = new Tuple<int, IList<eSWIS.Logic.Actions.Container.Consignhdr>>(0, new List<eSWIS.Logic.Actions.Container.Consignhdr>());
            //Dictionary<int, List<eSWIS.Logic.Actions.Container.Consignhdr>> res = new Dictionary<int, List<eSWIS.Logic.Actions.Container.Consignhdr>>();
            try
            {
                string conn = _appConfiguration.GetConnectionString(PlexformConsts.ESWISConnectionString);
                eSWIS.Logic.Actions.ConsignHDR obj = new eSWIS.Logic.Actions.ConsignHDR(conn);
                int totalRows = 0;
                var list = obj.GetListConsign(null, skipCount, limit, ref totalRows);
                res = new Tuple<int, IList<eSWIS.Logic.Actions.Container.Consignhdr>>(totalRows, list);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return Task.FromResult(res);
        }

       
    }
}
