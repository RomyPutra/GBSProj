using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plexform.Users
{
    public class UserProfileAppService : ApplicationService
    {
        public UserProfileAppService()
        {
        }

        public async Task<ListResultContainer<eSWIS.Logic.UsrProfile.Container.USRPROFILE>> GetAllUserProfile()
        {
            List<eSWIS.Logic.UsrProfile.Container.USRPROFILE> list = new List<eSWIS.Logic.UsrProfile.Container.USRPROFILE>();
            try
            {
                var repo = new Plexform.Logic.FooLogic();

                list = await repo.GetAllUserProfileAsync();
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }

            return new ListResultContainer<eSWIS.Logic.UsrProfile.Container.USRPROFILE>(
                ObjectMapper.Map<List<eSWIS.Logic.UsrProfile.Container.USRPROFILE>>(list),
                list.Count
            );
        }

        public async Task<bool> Create(eSWIS.Logic.UsrProfile.Container.USRPROFILE input)
        {
            bool res = false;
            try
            {
                var repo = new Plexform.Logic.UserProfileLogic();

                res = await repo.Insert(input);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }

            return ObjectMapper.Map<bool>(res);
        }

        public async Task<bool> Update(eSWIS.Logic.UsrProfile.Container.USRPROFILE input)
        {
            bool res = false;
            try
            {
                var repo = new Plexform.Logic.UserProfileLogic();

                res = await repo.Update(input);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }

            return ObjectMapper.Map<bool>(res);
        }
    }
}