using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Domain.Repositories;
using Plexform.Authorization.Roles;
using Plexform.Roles.Dto;
using eSWIS.Logic.UserSecurity.Container;
using Plexform.Models;

namespace Plexform.Controllers
{
    //[AbpAuthorize(PermissionNames.Pages_Users)]
    [Route("api/eswis/[controller]/[action]")]
    public class UserApprovalController : PlexformControllerBase
    {
        private readonly IRepository<Role> _roleRepository;

        public UserApprovalController(IRepository<Role> roleRepository)
        {
            try
            {
                _roleRepository = roleRepository;
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }
        }

        [HttpGet]
        public async Task<ListResultContainer<UserVerify>> GetUserAccessList()
        {
            List<UserVerify> existUser = new List<eSWIS.Logic.UserSecurity.Container.UserVerify>();
            try
            {
                var repo = new Plexform.Logic.UserApprovalLogic();
                existUser = await repo.GetUsrAccessList();
            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }


            return new ListResultContainer<UserVerify>(
                ObjectMapper.Map<List<UserVerify>>(existUser),
                existUser.Count
           );
        }

        [HttpGet]
        public async Task<UserVerify> GetApprovalEdit(string UserID)
        {
            var existUser = new eSWIS.Logic.UserSecurity.Container.UserVerify();
            try
            {
                var repo = new Plexform.Logic.UserApprovalLogic();
                existUser = await repo.GetEditApproval(UserID);

            }
            catch (Exception ex)
            {
                var temp = ex.ToString();
            }

            return (existUser);
        }

        [HttpPut]
        public async Task<eSWIS.Logic.UserSecurity.Container.UserVerify> UpdateApproval([FromBody]EditApprovalDto model)
        {
            var result = new Plexform.Logic.UserApprovalLogic();
            return await result.UpdateUserApproval(model.userID, model.Status, model.RejectRemark);
        }

    }
}
