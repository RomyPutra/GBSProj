import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { UserProfileServiceProxy } from '@shared/service-proxies/service-proxies';
import { RoleDto } from '@shared/models/model-role';
import { UserProfileDto } from '@shared/models/model-userprofile';
import { AppComponentBase } from '@shared/app-component-base';
import { UserGroupDto, PagedResultDtoOfUserGroupDto } from '@shared/models/model-usergroup';
import { UserGroupServiceProxy } from '@shared/service-proxies/service-proxies';

import * as _ from "lodash";

@Component({
    selector: 'edit-userprofile-modal',
    templateUrl: './edit-userprofile.component.html'
})
export class EditUserprofileComponent extends AppComponentBase {

    @ViewChild('editUserprofileModal') modal: ModalDirective;
    @ViewChild('modalContent') modalContent: ElementRef;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active: boolean = false;
    saving: boolean = false;
    user: UserProfileDto = null;
    roles: RoleDto[] = null;
    confirmPassword = '';
    userGroups: UserGroupDto[] = [];
    selectedAccessCode: string

    constructor(
        injector: Injector,
        private _userService: UserProfileServiceProxy,
        private _serviceUserGroup: UserGroupServiceProxy
    ) {
        super(injector);
    }

    show(user: UserProfileDto): void {
      this.active = true;
        this.user = user;
        this.modal.show();
        this.populateUserGroup();
        // this.user.init({ isActive: true });
        // this._userService.getRoles()
        //     .subscribe((result) => {
        //         this.roles = result.items;
        //     });

        // this._userService.get(id)
        //     .subscribe(
        //     (result) => {
        //         this.user = result;
        //         this.active = true;
        //         this.modal.show();
        //     }
        //     );
    }

    onShown(): void {
        $.AdminBSB.input.activate($(this.modalContent.nativeElement));
    }

    save(): void {
        //TODO: Refactor this, don't use jQuery style code
        this.user.accessCode = this.selectedAccessCode;
        this.saving = true;
        this._userService.update(this.user)
            .finally(() => {
                this.saving = false;
            })
            .subscribe((result: boolean) => {
                if (result) {
                    this.notify.info(this.l('SavedSuccessfully'));
                    this.close();
                    this.modalSave.emit(null);
                } else {
                    this.notify.error('Save failed!');
                }
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    private populateUserGroup(): void {
        this._serviceUserGroup.getAll(0, 0)
            .finally(() => {

            })
            .subscribe((result: PagedResultDtoOfUserGroupDto) => {
                this.userGroups = result.items;
                this.selectedAccessCode = this.user.accessCode;
            });
    }

    changeSelect() {
        let temp = this.selectedAccessCode;
    }

    onChangeCategorySelect(event) {
        this.selectedAccessCode = event.target.value;

        // if (value == 'add') {
        //     alert("add new cat");
        //     this.model.category = 'default';
        // }

        // //event.target.value = this.model.category;

        // this.model.subcategory = 'default';
    }
}
