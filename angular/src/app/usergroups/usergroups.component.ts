import { EditUserGroupComponent } from './edit-usergroup/edit-usergroup.component';
import { CreateUserGroupComponent } from './create-usergroup/create-usergroup.component';
import { Component, Injector, ViewChild } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { UserGroupDto, PagedResultDtoOfUserGroupDto } from '@shared/models/model-usergroup';
import { UserGroupServiceProxy } from '@shared/service-proxies/service-proxies';
import { PagedListingComponentBase, PagedRequestDto } from 'shared/paged-listing-component-base';

@Component({
    templateUrl: './usergroups.component.html',
    animations: [appModuleAnimation()]
})
export class UsergroupsComponent extends PagedListingComponentBase<UserGroupDto> {

    @ViewChild('createUserGroupModal') createUserGroupModal: CreateUserGroupComponent;
    @ViewChild('editUserGroupModal') editUserGroupModal: EditUserGroupComponent;

    active: boolean = false;
    groups: UserGroupDto[] = [];

    constructor(
        injector: Injector,
        private _userService: UserGroupServiceProxy
    ) {
        super(injector);
    }

    protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
        this._userService.getAll(request.skipCount, request.maxResultCount)
            .finally(() => {
                finishedCallback();
            })
            .subscribe((result: PagedResultDtoOfUserGroupDto) => {
                this.groups = result.items;
                this.showPaging(result, pageNumber);
            });
    }

    protected delete(group: UserGroupDto): void {
        abp.message.confirm(
            "Delete group '" + group.groupCode + "'?",
            (result: boolean) => {
                if (result) {
                //   this._userService.delete(user.id)
                //       .subscribe(() => {
                //           abp.notify.info("Deleted User: " + user.fullName);
                //           this.refresh();
                //       });
                }
            }
        );
    }

    // Show Modals
    createUserGroup(): void {
        this.createUserGroupModal.show();
    }

    // editGroup(groups:UserGroupDto): void {
    //     this.editGroupModal.show(groups.groupCode);
    // }
    // editGroup(groups:UserGroupDto): void {
    //     this.editGroupModal.show(groups.groupCode);
    // }

    editGroup(groups: UserGroupDto): void {
        this.editUserGroupModal.show(groups.groupCode);
    }

}
