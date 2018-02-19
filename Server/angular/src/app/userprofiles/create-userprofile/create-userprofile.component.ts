import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { UserProfileServiceProxy } from '@shared/service-proxies/proxy-userprofile';
import { UserProfileDto } from '@shared/models/model-userprofile';
import { RoleDto } from '@shared/models/model-role';
import { AppComponentBase } from '@shared/app-component-base';
import { UserGroupDto, PagedResultDtoOfUserGroupDto } from '@shared/models/model-usergroup';
import { UserGroupServiceProxy } from '@shared/service-proxies/service-proxies';
import { ActionState } from '@shared/models/enums';

import * as _ from "lodash";

@Component({
  selector: 'create-userprofile-modal',
  templateUrl: './create-userprofile.component.html'
})

export class CreateUserprofileComponent extends AppComponentBase implements OnInit {

    @ViewChild('createUserprofileModal') modal: ModalDirective;
    @ViewChild('modalContent') modalContent: ElementRef;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active: boolean = false;
    saving: boolean = false;
    user: UserProfileDto = null;
    userGroups: UserGroupDto[] = [];
    action: ActionState;
    ActionState = ActionState;

    constructor(
        injector: Injector,
        private _userService: UserProfileServiceProxy,
        private _serviceUserGroup: UserGroupServiceProxy
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.populateUserGroup();
    }

    show(action: ActionState, user?: UserProfileDto): void {
        this.action = action;
        if (action === ActionState.Create) {
            this.active = true;
            this.modal.show();
            this.user = new UserProfileDto();
            this.user.init({ isActive: true });
        } else if (action === ActionState.Edit) {
            if (user === undefined || user === null) {
                this.notify.error("The parameter 'user' must be defined and cannot be null.");
            } else {
                this.active = true;
                this.user = user;
                this.modal.show();
            }
        }
    }

    private populateUserGroup(): void {
        this._serviceUserGroup.getAll()
            .finally(() => {

            })
            .subscribe((result: PagedResultDtoOfUserGroupDto) => {
                this.userGroups = result.items;
            });
    }

    onShown(): void {
        $.AdminBSB.input.activate($(this.modalContent.nativeElement));
    }

    save(): void {
        //TODO: Refactor this, don't use jQuery style code
        // this.user.accessCode = this.user.accessCode;
        this.saving = true;

        if (this.action === ActionState.Create) {
            this._userService.create(this.user)
                .finally(() => { this.saving = false; })
                .subscribe((result: boolean) => {
                    if (result) {
                        this.notify.info(this.l('SavedSuccessfully'));
                        this.close();
                        this.modalSave.emit(null);
                    } else {
                        this.notify.error('Save failed!');
                    }
                });
        } else if (this.action === ActionState.Edit) {
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
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

}
