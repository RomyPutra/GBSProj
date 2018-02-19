import { ActionState } from '@shared/models/enums';
// import { EditUserGroupComponent } from './edit-usergroup/edit-usergroup.component';
import { CreateUserGroupComponent } from './create-usergroup/create-usergroup.component';
import { Component, Injector, ViewChild } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { UserGroupDto, PagedResultDtoOfUserGroupDto } from '@shared/models/model-usergroup';
import { UserGroupServiceProxy } from '@shared/service-proxies/service-proxies';
import { PagedRequestDto } from 'shared/paged-listing-component-base';
import { PagedListingComponentCustom } from 'shared/layout/paged-listing-component-custom';

@Component({
    templateUrl: './usergroups.component.html',
    animations: [appModuleAnimation()]
})
export class UsergroupsComponent extends PagedListingComponentCustom<UserGroupDto> {

    @ViewChild('createUserGroupModal') createUserGroupModal: CreateUserGroupComponent;
    // @ViewChild('editUserGroupModal') editUserGroupModal: EditUserGroupComponent;

    active: boolean = false;
    groups: UserGroupDto[] = [];

    constructor(
        injector: Injector,
        private _userService: UserGroupServiceProxy
    ) {
        super(injector);

        this.loadingMessage = 'Loading...';
    }

    protected refresh(): void {
        this.isBusy = true;
        this._userService.getAll()
            .finally(() => {
                this.isBusy = false;
            })
            .subscribe((result: PagedResultDtoOfUserGroupDto) => {
                this.groups = result.items;
            });
    }

    protected delete(group: UserGroupDto): void {
        abp.message.confirm(
            "Delete group '" + group.groupName + "'?",
            (result: boolean) => {
                if (result) {
                  this._userService.delete(group)
                      .subscribe(() => {
                          abp.notify.success("Deleted User Group: " + group.groupName);
                          this.refresh();
                      });
                }
            }
        );
    }

    // Show Modals
    createUserGroup(): void {
        this.createUserGroupModal.show(ActionState.Create);
    }

    editGroup(groups: UserGroupDto): void {
        this.createUserGroupModal.show(ActionState.Edit, groups);
    }

}
