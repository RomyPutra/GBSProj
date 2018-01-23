using Abp.Reflection.Extensions;
using eSWIS.Logic.UserSecurity;
using Microsoft.Extensions.Configuration;
using Plexform.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using eSWIS.Logic.UserSecurity.Container;


namespace Plexform.Logic
{
    public class UserApprovalLogic
    {
        private readonly IConfigurationRoot _appConfiguration;

        public UserApprovalLogic()
        {
            _appConfiguration = AppConfigurations.Get(
                typeof(UserApprovalLogic).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        
        public Task<List<eSWIS.Logic.UserSecurity.Container.UserVerify>> GetUsrAccessList()
        {
            List<eSWIS.Logic.UserSecurity.Container.UserVerify> UserExist = new List<eSWIS.Logic.UserSecurity.Container.UserVerify>();

            try
            {
                string conn = _appConfiguration.GetConnectionString(PlexformConsts.ESWISConnectionString);
                UsrVerify obj = new UsrVerify(conn);
                UserExist = obj.GetUsrAccessListAll();

            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return Task.FromResult (UserExist);
        }

        public Task<UserVerify> GetEditApproval(string UserID)
        {
            UserVerify UserExist = new UserVerify();

            try
            {
                string conn = _appConfiguration.GetConnectionString(PlexformConsts.ESWISConnectionString);
                UsrVerify obj = new UsrVerify(conn);
                UserExist = obj.GetApprovalEdit(UserID);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return Task.FromResult(UserExist);
        }

        public Task<UserVerify> UpdateUserApproval(string userID, string Status, string RejectRemark)
        {
            UserVerify UserExist = new eSWIS.Logic.UserSecurity.Container.UserVerify();
            try
            {
                string conn = _appConfiguration.GetConnectionString(PlexformConsts.ESWISConnectionString);
                UsrVerify obj = new UsrVerify(conn);
                UserExist = obj.UpdateApproval(userID, Status, RejectRemark);
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
            return Task.FromResult(UserExist);
        }
    }
}
