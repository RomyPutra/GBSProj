import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { UserGroupServiceProxy } from '@shared/service-proxies/service-proxies';
import { CodeMasterServiceProxy } from '@shared/service-proxies/proxy-codemaster';
import { UserGroupDto } from '@shared/models/model-usergroup';
import { CodeMasterDto } from '@shared/models/model-codemaster';
import { AppComponentBase } from '@shared/app-component-base';

import { ActionState } from '@shared/models/enums';
import { AppConsts } from '@shared/AppConsts';
import { UtilsService } from '@abp/utils/utils.service';

@Component({
    selector: 'create-usergroup-modal',
    templateUrl: './create-usergroup.component.html'
})
export class CreateUserGroupComponent extends AppComponentBase implements OnInit  {

    @ViewChild('createUserGroupModal') modal: ModalDirective;
    @ViewChild('modalContent') modalContent: ElementRef;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active: boolean = false;
    saving: boolean = false;
    data: UserGroupDto = null;
    roles: CodeMasterDto[] = null;
    isActive: boolean;
    userID: string;

    appid: string;

    action: ActionState;
    ActionState = ActionState;

    constructor(
        injector: Injector,
        private _service: UserGroupServiceProxy,
        private _serviceCodeMaster: CodeMasterServiceProxy
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.populateRole();

        this.userID = new UtilsService().getCookieValue(AppConsts.authorization.userIDName);
    }

    protected populateRole(): void {
        this._serviceCodeMaster.getAppIDs()
            .finally(() => {

            })
            .subscribe((result) => {
                this.roles = result.items;
            });
    }

    show(action: ActionState, data?: UserGroupDto): void {
        this.action = action;
        if (action === ActionState.Create) {
            this.active = true;
            this.modal.show();
            this.data = new UserGroupDto();
            this.data.init();
            this.isActive = true;
            this.data.createBy = this.userID;
        } else if (action === ActionState.Edit) {
            if (data === undefined || data === null) {
                this.notify.error("The parameter 'data' must be defined and cannot be null.");
            } else {
                this.active = true;
                this.data = data;
                this.isActive = this.data.status === 1;
                this.data.updateBy = this.userID;
                this.appid = String(this.data.appid);
                this.modal.show();
            }
        }
    }

    onShown(): void {
        $.AdminBSB.input.activate($(this.modalContent.nativeElement));
    }

    save(): void {
        //TODO: Refactor this, don't use jQuery style code
        // this.data.accessCode = this.data.accessCode;
        this.loadingMessage = 'Saving...';
        this.saving = true;
        this.data.status = this.isActive ? 1 : 0;
        this.data.appid = Number(this.appid);
        if (this.action === ActionState.Create) {
            this._service.create(this.data)
                .finally(() => { this.saving = false; })
                .subscribe((result: boolean) => {
                    if (result) {
                        this.notify.success(this.l('SavedSuccessfully'));
                        this.close();
                        this.modalSave.emit(null);
                    } else {
                        this.notify.error('Save failed!');
                    }
                });
        } else if (this.action === ActionState.Edit) {
            this._service.update(this.data)
                .finally(() => {
                    this.saving = false;
                })
                .subscribe((result: boolean) => {
                    if (result) {
                        this.notify.success(this.l('SavedSuccessfully'));
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
