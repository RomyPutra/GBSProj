using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plexform.Users
{
    public class UserGroupAppService : ApplicationService
    {
        public UserGroupAppService()
        {
        }

		//public override async Task<ListResultContainer<eSWIS.Logic.UserSecurity.Container.UserGroup>> Create(ListResultContainer<eSWIS.Logic.UserSecurity.Container.UserGroup> input)
		//{
		//CheckCreatePermission();

		//var userGroups = ObjectMapper.Map<eSWIS.Logic.UserSecurity.Container.UserGroup>(input);

		//userGroups.APPID = ;
		//user.TenantId = AbpSession.TenantId;
		//user.Password = _passwordHasher.HashPassword(user, input.Password);
		//user.IsEmailConfirmed = true;

		//CheckErrors(await _userManager.CreateAsync(user));

		//if (input.RoleNames != null)
		//{
		//	CheckErrors(await _userManager.SetRoles(user, input.RoleNames));
		//}

		//CurrentUnitOfWork.SaveChanges();

		//return MapToEntityDto(userGroups);
		//}

		//public override async Task<UserDto> Update(UserDto input)
		//{
		//	CheckUpdatePermission();

		//	var user = await _userManager.GetUserByIdAsync(input.Id);

		//	MapToEntity(input, user);

		//	CheckErrors(await _userManager.UpdateAsync(user));

		//	if (input.RoleNames != null)
		//	{
		//		CheckErrors(await _userManager.SetRoles(user, input.RoleNames));
		//	}

		//	return await Get(input);
		//}

		//public override async Task Delete(EntityDto<long> input)
		//{
		//	var user = await _userManager.GetUserByIdAsync(input.Id);
		//	await _userManager.DeleteAsync(user);
		//}
		//public async Task<eSWIS.Logic.UserSecurity.Container.UserGroup> Get(string ID)
		//{
		//	eSWIS.Logic.UserSecurity.Container.UserGroup list = new eSWIS.Logic.UserSecurity.Container.UserGroup();
		//	try
		//	{
		//		var repo = new Plexform.Logic.FooLogic();

		//		list = await repo.GetUserGroupByID(ID);
		//	}
		//	catch (Exception ex)
		//	{
		//		var temp = ex.ToString();
		//	}

		//	return ObjectMapper.Map<eSWIS.Logic.UserSecurity.Container.UserGroup>(list);
		//}

		//public async Task<eSWIS.Logic.UserSecurity.Container.UserGroup> Update(eSWIS.Logic.UserSecurity.Container.UserGroup Datas)
		//{
		//	string ID = "", CODE = "", DESC = "", STATUS = "";
		//	if (Datas is eSWIS.Logic.UserSecurity.Container.UserGroup)
		//	{
		//		var temp = (eSWIS.Logic.UserSecurity.Container.UserGroup)Datas;
		//		ID = temp.GroupCode.ToString();
		//		CODE = temp.APPID.ToString();
		//		DESC = temp.GroupName.ToString();
		//		STATUS = temp.Status.ToString();
		//	}
		//	eSWIS.Logic.UserSecurity.Container.UserGroup list = new eSWIS.Logic.UserSecurity.Container.UserGroup();
		//	try
		//	{
		//		var repo = new Plexform.Logic.FooLogic();				
		//		list = await repo.GetUserGroupByID(ID, CODE, DESC, STATUS);
		//	}
		//	catch (Exception ex)
		//	{
		//		var temp = ex.ToString();
		//	}

		//	return ObjectMapper.Map<eSWIS.Logic.UserSecurity.Container.UserGroup>(list);
		//}

		//public async Task<ListResultContainer<eSWIS.Logic.UserSecurity.Container.UserGroup>> GetAllUserGroup()
  //      {
  //          List<eSWIS.Logic.UserSecurity.Container.UserGroup> list = new List<eSWIS.Logic.UserSecurity.Container.UserGroup>();
  //          try
  //          {
  //              var repo = new Plexform.Logic.FooLogic();

  //              list = await repo.GetAllUserGroupAsync();
  //          }
  //          catch (Exception ex)
  //          {
  //              var temp = ex.ToString();
  //          }

  //          return new ListResultContainer<eSWIS.Logic.UserSecurity.Container.UserGroup>(
  //              ObjectMapper.Map<List<eSWIS.Logic.UserSecurity.Container.UserGroup>>(list),
  //              list.Count
  //          );
  //      }
    }
}
