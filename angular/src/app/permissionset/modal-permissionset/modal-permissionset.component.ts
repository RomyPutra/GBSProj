import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/app-component-base';

import { ActivatedRoute } from '@angular/router';

import { ActionState } from '@shared/models/enums';
import { AppConsts } from '@shared/AppConsts';
import { UtilsService } from '@abp/utils/utils.service';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { startWith } from 'rxjs/operators/startWith';
import { map } from 'rxjs/operators/map';

// #region DTO
import { PermissionFunctionDto } from '@shared/models/model-permissionset';
import { UserGroupDto } from 'shared/models/model-usergroup';
import { SysModuleDto } from 'shared/models/model-sysmodule';
import { SysFunctionDto } from 'shared/models/model-sysfunction';
// #endregion

// #region "Service Proxy"
import { PermissionSetServiceProxy } from '@shared/service-proxies/service-proxies';
import { UserGroupServiceProxy } from '@shared/service-proxies/service-proxies';
import { PermissionServiceProxy } from '@shared/service-proxies/proxy-permission';
// #endregion

import * as _ from "lodash";
import { MatAutocomplete } from '@angular/material';

@Component({
  selector: 'permissionset-modal',
  templateUrl: './modal-permissionset.component.html'
})

export class ModalPermissionsetComponent extends AppComponentBase implements OnInit {

    @ViewChild('permissionsetModal') modal: ModalDirective;
    @ViewChild('modalContent') modalContent: ElementRef;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active: boolean = false;
    saving: boolean = false;

    data: PermissionFunctionDto;

    isActive: boolean;

    userID: string;

    isCreate: boolean;
    isUpdate: boolean;
    isDelete: boolean;
    isPrint: boolean;
    isProcess: boolean;
    isDeny: boolean;

    action: ActionState;
    ActionState = ActionState;

    // #region population objects
    userGroups: UserGroupDto[] = [];
    filteredUserGroups: Observable<UserGroupDto[]>;
    selectedUserGroup: any;
    controlUserGroup: FormControl = new FormControl('', [Validators.required]);

    sysModules: SysModuleDto[] = [];
    filteredSysModules: Observable<SysModuleDto[]>;
    selectedSysModule: any;
    controlSysModule: FormControl = new FormControl('', [Validators.required]);

    sysFunctions: SysFunctionDto[] = [];
    filteredSysFunctions: Observable<SysFunctionDto[]>;
    selectedSysFunction: any;
    controlSysFunction: FormControl = new FormControl('', [Validators.required]);
    // #endregion

    constructor(
        injector: Injector,
        private _service: PermissionSetServiceProxy,
        private _serviceGroup: UserGroupServiceProxy,
        private _servicePermission: PermissionServiceProxy,
        private _formBuilder: FormBuilder,
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.populateUserGroup();
        this.populateSysModule();
        this.populateSysFunction();

        this.userID = new UtilsService().getCookieValue(AppConsts.authorization.userIDName);

        this.filteredUserGroups = this.controlUserGroup.valueChanges
            .pipe(
                startWith(''),
                map(val => this.filterUserGroups(val))
            );

        this.filteredSysModules = this.controlSysModule.valueChanges
            .pipe(
                startWith(''),
                map(val => this.filterSysModule(val))
            );

        this.filteredSysFunctions = this.controlSysFunction.valueChanges
            .pipe(
                startWith(''),
                map(val => this.filterSysFunction(val))
            );
    }

    // #region "Mat AutoComplete"

    // #region "Mat AutoComplete SysModule"
    filterSysModule(val: any): SysModuleDto[] {
        let filter = '';
        if (typeof val === 'string') {
            filter = val;
        } else {
            filter = this.displaySysModule(val);
        }
        return this.sysModules.filter(option =>
        this.displaySysModule(option).toLowerCase().indexOf(filter.toLowerCase()) !== -1);
    }

    displaySysModule(data?: SysModuleDto): string | undefined {
        return data ? data.moduleName : undefined;
    }

    onBlurSysModule(): void {
        let that = this;
        this.timer = window.setTimeout(function() {
            if (typeof that.selectedSysModule === 'string') {
                that.selectedSysModule = '';
            }
        }, 250);
    }

    // #endregion

    // #region "Mat AutoComplete SysFunction"
    filterSysFunction(val: any): SysFunctionDto[] {
        let filter = '';
        if (typeof val === 'string') {
            filter = val;
        } else {
            filter = this.displaySysFunction(val);
        }
        return this.sysFunctions.filter(option =>
        this.displaySysFunction(option).toLowerCase().indexOf(filter.toLowerCase()) !== -1);
    }

    displaySysFunction(data?: SysFunctionDto): string | undefined {
        return data ? data.functionName : undefined;
    }

    onBlurSysFunction(): void {
        let that = this;
        this.timer = window.setTimeout(function() {
            if (typeof that.selectedSysFunction === 'string') {
                that.selectedSysFunction = '';
            }
        }, 250);
    }

    // #endregion

    // #region "Mat AutoComplete UserGroup"
    filterUserGroups(val: any): UserGroupDto[] {
        let filter = '';
        if (typeof val === 'string') {
            filter = val;
        } else {
            filter = this.displayUserGroup(val);
        }
        return this.userGroups.filter(option =>
        this.displayUserGroup(option).toLowerCase().indexOf(filter.toLowerCase()) !== -1);
    }

    displayUserGroup(data?: UserGroupDto): string | undefined {
        return data ? data.groupName : undefined;
    }

    onBlurUserGroup(): void {
        let that = this;
        this.timer = window.setTimeout(function() {
            if (typeof that.selectedUserGroup === 'string') {
                that.selectedUserGroup = '';
            } else {
                
            }
        }, 250);
    }
    // #endregion

    // #endregion

    // #region "Data Population"
    private populateUserGroup(): void {
        this._serviceGroup.getAll()
            .finally(() => {

            })
            .subscribe((result) => {
                this.userGroups = result.items;
            });
    }

    protected populateSysModule(): void {
        this._servicePermission.getAllSysModule()
            .finally(() => {

            })
            .subscribe((result) => {
                this.sysModules = result.items;
            });
    }

    protected populateSysFunction(): void {
        this._servicePermission.getAllSysFunction()
            .finally(() => {

            })
            .subscribe((result) => {
                this.sysFunctions = result.items;
            });
    }

    // #endregion "Data Population"

    show(action: ActionState): void {
        this.action = action;
        this.selectedSysModule = undefined;
        this.selectedUserGroup = undefined;
        this.selectedSysFunction = undefined;

        if (action === ActionState.Create) {
            this.active = true;
            this.data = new PermissionFunctionDto();
            this.data.lastSyncBy = this.userID;

            this.isCreate = false;
            this.isUpdate = false;
            this.isDelete = false;
            this.isPrint = false;
            this.isProcess = false;
            this.isDeny = false;

            this.isActive = true;
            this.modal.show();
        } else if (action === ActionState.Edit) {

        }
    }
    onShown(): void {
        $.AdminBSB.input.activate($(this.modalContent.nativeElement));
    }

    save(): void {
        //TODO: Refactor this, don't use jQuery style code
        // this.data.accessCode = this.data.accessCode;
        this.saving = true;
        this.data.accessCode = this.selectedUserGroup.groupCode;
        this.data.appid = this.selectedUserGroup.appid;
        this.data.moduleID = this.selectedSysModule.moduleID;
        this.data.functionID = this.selectedSysFunction.functionID;
        this.data.allowNew = this.isCreate ? 1 : 0;
        this.data.allowEdit = this.isUpdate ? 1 : 0;
        this.data.allowDel = this.isDelete ? 1 : 0;
        this.data.allowPrt = this.isPrint ? 1 : 0;
        this.data.allowPro = this.isProcess ? 1 : 0;
        this.data.isDeny = this.isDeny ? 1 : 0;
        if (this.action === ActionState.Create) {
            this._service.create(this.data)
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

        }
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

}
