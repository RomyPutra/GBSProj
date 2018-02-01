import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';

import { ActivatedRoute } from '@angular/router';

import { ActionState } from '@shared/models/enums';
import { AppConsts } from '@shared/AppConsts';
import { UtilsService } from '@abp/utils/utils.service';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { startWith } from 'rxjs/operators/startWith';
import { map } from 'rxjs/operators/map';

// #region DTO
import { EmployeeDto, CreateEmployeeDto, EmployeeBranchDto } from '@shared/models/model-employee';
import { AppComponentBase } from '@shared/app-component-base';
import { UserGroupDto } from 'shared/models/model-usergroup';
import { StateDto } from 'shared/models/model-state';
import { CodeMasterDto } from 'shared/models/model-codemaster';
import { LocationDto } from 'shared/models/model-location';
// #endregion

// #region "Service Proxy"
import { EmployeeServiceProxy } from '@shared/service-proxies/proxy-employee';
import { UserGroupServiceProxy } from '@shared/service-proxies/service-proxies';
import { CodeMasterServiceProxy } from '@shared/service-proxies/proxy-codemaster';
import { StateServiceProxy } from '@shared/service-proxies/proxy-state';
import { LocationServiceProxy } from '@shared/service-proxies/proxy-location';
// #endregion

import * as _ from "lodash";
import { MatAutocomplete } from '@angular/material';

@Component({
    selector: 'create-edit-employee-modal',
    templateUrl: './modal-employee.component.html'
})

export class ModalEmployeeComponent extends AppComponentBase implements OnInit {

    @ViewChild('createEditEmployeeModal') modal: ModalDirective;
    @ViewChild('modalContent') modalContent: ElementRef;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active: boolean = false;
    saving: boolean = false;
    data: CreateEmployeeDto = null;
    isActive: boolean;

    isStandAlone = true;

    isBranchVisible = false;
    isUserGroupVisible = false;

    private companyID: string;

    userID: string;

    action: ActionState;
    ActionState = ActionState;

    firstFormGroup: FormGroup;

    // #region population objects
    userGroups: UserGroupDto[] = [];
    salutations: CodeMasterDto[] = [];
    genders: CodeMasterDto[] = [];
    empBranch: EmployeeBranchDto;

    states: StateDto[] = [];
    filteredStates: Observable<StateDto[]>;
    selectedState: any;
    controlState: FormControl = new FormControl('', [Validators.required]);

    designations: CodeMasterDto[] = [];
    filteredDesignations: Observable<CodeMasterDto[]>;
    selectedDesignation: any;
    controlDesignation: FormControl = new FormControl('', [Validators.required]);

    branches: LocationDto[] = [];
    tempSelectedBranch: any;
    selectedBranch: any;
    controlBranch: FormControl = new FormControl('', [Validators.required]);
    branchesform = new FormControl();

    bizregid: string;
    // #endregion

    constructor(
        injector: Injector,
        private _service: EmployeeServiceProxy,
        private _serviceGroup: UserGroupServiceProxy,
        private _serviceState: StateServiceProxy,
        private _serviceCodeMaster: CodeMasterServiceProxy,
        private _serviceLocation: LocationServiceProxy,
        private route: ActivatedRoute,
        private _formBuilder: FormBuilder,
        private _utilsService: UtilsService,
    ) {
        super(injector);

        this.isStandAlone = true;
        this.route.params.subscribe((params) => {
            if (params.companyID) {
                this.companyID = params.companyID;
            } else if (params.bizRegID) {
                this.companyID = params.bizRegID;
                this.isStandAlone = false;
            } else {
                this.companyID = _utilsService.getCookieValue(AppConsts.authorization.parentIDName);
            }
        });

        let appID = _utilsService.getCookieValue(AppConsts.otherSetting.appid);
        let accessCode = _utilsService.getCookieValue(AppConsts.otherSetting.accesscode);

        this.isBranchVisible = (appID === '2' && accessCode === '03') || (appID === '3' && accessCode === '05') || (appID === '4' && accessCode === '07');
        this.isUserGroupVisible = (appID === '1' && accessCode === '01');
        this.bizregid = new UtilsService().getCookieValue(AppConsts.authorization.parentIDName);
    }

    ngOnInit(): void {
        this.populateUserGroup();
        this.populateState();
        this.populateGender();
        this.populateSalutation();
        this.populateDesignation();
        this.userID = new UtilsService().getCookieValue(AppConsts.authorization.userIDName);
 
        // this.firstFormGroup = this._formBuilder.group({
        //     employeeID: ['', Validators.required],
        //     nricNo: ['', ''],
        //     nickName: ['', ''],
        //     salutation: ['', ''],
        //     sex: ['', ''],
        //     emailAddress: ['', ''],
        //     emerContactNo: ['', ''],
        //     status: ['', ''],
        //     accessCode: ['', ''],
        //     username: ['', ''],
        //     Password: ['', '']
        // });

        this.filteredStates = this.controlState.valueChanges
            .pipe(
            startWith(''),
            map(val => this.filterState(val))
            );

        this.filteredDesignations = this.controlDesignation.valueChanges
            .pipe(
            startWith(''),
            map(val => this.filterDesignation(val))
            );
    }

   

    // #region "Mat AutoComplete"

    // #region "Mat AutoComplete State"
    filterState(val: any): StateDto[] {
        let filter = '';
        if (typeof val === 'string') {
            filter = val;
        } else {
            filter = this.displayState(val);
        }
        return this.states.filter(option =>
            this.displayState(option).toLowerCase().indexOf(filter.toLowerCase()) !== -1);
    }

    displayState(data?: StateDto): string | undefined {
        return data ? data.stateDesc : undefined;
    }

    onBlurState(): void {
        let that = this;
        this.timer = window.setTimeout(function () {
            if (typeof that.selectedState === 'string') {
                that.selectedState = '';
            }
        }, 250);
    }

    // #endregion

    // #region "Mat AutoComplete Designation"
    filterDesignation(val: any): CodeMasterDto[] {
        let filter = '';
        if (typeof val === 'string') {
            filter = val;
        } else {
            filter = this.displayDesignation(val);
        }
        return this.designations.filter(option =>
            this.displayDesignation(option).toLowerCase().indexOf(filter.toLowerCase()) !== -1);
    }

    displayDesignation(data?: CodeMasterDto): string | undefined {
        return data ? data.codeDesc : undefined;
    }

    onBlurDesignation(): void {
        let that = this;
        this.timer = window.setTimeout(function () {
            if (typeof that.selectedDesignation === 'string') {
                that.selectedDesignation = '';
            }
        }, 250);
    }

    // #endregion

    // #endregion

    // #region "Data Population"
    protected populateUserGroup(): void {
        this._serviceGroup.getAll()
            .finally(() => {

            })
            .subscribe((result) => {
                this.userGroups = result.items;
            });
    }

    protected populateState(): void {
        this._serviceState.getAll(0, 0)
            .finally(() => {

            })
            .subscribe((result) => {
                this.states = result.items;
            });
    }

    protected populateSalutation(): void {
        this._serviceCodeMaster.getSalutations()
            .finally(() => {

            })
            .subscribe((result) => {
                this.salutations = result.items;
            });
    }

    protected populateGender(): void {
        this._serviceCodeMaster.getGenders()
            .finally(() => {

            })
            .subscribe((result) => {
                this.genders = result.items;
            });
    }

    protected populateDesignation(): void {
        this._serviceCodeMaster.getDesignations()
            .finally(() => {

            })
            .subscribe((result) => {
                this.designations = result.items;
            });
    }


    protected populateBranches(): void {
        if (!(this.companyID === undefined || this.companyID === null || this.companyID === '')) {
            this._serviceLocation.getListEntity(0, 0, this.companyID)
                .finally(() => {
                })
                .subscribe((result) => {
                    this.branches = result.items;
                });
        }
    }

    // #endregion "Data Population"

    show(action: ActionState, companyID: string, data?: EmployeeDto): void {
        this.action = action;
        this.selectedState = undefined;
        this.selectedDesignation = undefined;
        this.selectedBranch = undefined;
        if (action === ActionState.Create) {
            this.active = true;
            this.data = new CreateEmployeeDto();
            this.data.empData = new EmployeeDto();
            this.data.empData.init({ active: true });
            this.data.empData.createBy = this.userID;
            this.data.empData.companyID = companyID;
            this.isActive = true;
            this.populateBranches();
            this.modal.show();
        } else if (action === ActionState.Edit) {
            if (data === undefined || data === null) {
                this.notify.error("The parameter 'data' must be defined and cannot be null.");
            } else {
                // if (this.branches.length <= 0) {
                //     this.companyID = data.companyID;
                //     this.populateBranches();
                // }
                this.populateBranches();
                this.getSelectedBranch(data.employeeID, () => {
                    this._convertSelect(this.tempSelectedBranch);
                });
                this.active = true;
                this.data = new CreateEmployeeDto();
                this.data.empData = data;
                this.data.empData.createBy = this.userID;
                this.data.empData.companyID = companyID;
                this.isActive = data.status === 1;

                let arrState = this.states.filter(option =>
                    option.stateCode.toLowerCase() === data.coState.toLowerCase());
                if (arrState !== undefined && arrState.length > 0) {
                    this.selectedState = arrState[0];
                }
                let arrDesignation = this.designations.filter(option =>
                    option.code.toLowerCase() === data.designation.toLowerCase());
                if (arrDesignation !== undefined && arrDesignation.length > 0) {
                    this.selectedDesignation = arrDesignation[0];
                }

                this.modal.show();
            }
        }
    }

    onShown(): void {
        $.AdminBSB.input.activate($(this.modalContent.nativeElement));
    }

    protected getSelectedBranch(empID: string, finishedCallback: Function) {
        if (!(empID === undefined || empID === null || empID === '')) {
            this._serviceLocation.getListEntity(0, 0, null, empID)
                .finally(() => {
                    finishedCallback();
                })
                .subscribe((result) => {
                    this.tempSelectedBranch = result.items;
                });
        }
    }

    private _convertSelect(value: any) {
        let i: number = 0;
        for (let index = 0; index < value.length; index++) {
            i = this.branches.findIndex(item => item.bizLocID === value[index].bizLocID);
            if (this.selectedBranch === undefined) {
                this.selectedBranch = [ this.branches[i] ];
            } else {
                this.selectedBranch.push(this.branches[i]);
            }
        }
    }

    save(): void {
        // TODO: Refactor this, don't use jQuery style code
        // this.data.accessCode = this.data.accessCode;
        
        this.saving = true;
        this.data.empData.coState = this.selectedState.stateCode;
        this.data.empData.designation = this.selectedDesignation.code;
        this.data.empData.status = this.isActive ? 1 : 0;
        this.data.empBranches = [];
        for (var i = 0; i < this.selectedBranch.length; i++) {
            this.empBranch = new EmployeeBranchDto();
            this.empBranch.locID = this.selectedBranch[i].bizLocID;
            this.empBranch.employeeID = this.data.empData.employeeID;
            this.data.empBranches.push(this.empBranch);
        }
        let arrUserGroups = this.userGroups.filter(option =>
            option.groupCode.toLowerCase() === this.data.empData.accessCode.toLowerCase());
        if (arrUserGroups !== undefined && arrUserGroups.length > 0) {
            this.data.appID = arrUserGroups[0].appid;
        }
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
            this._service.update(this.data)
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
