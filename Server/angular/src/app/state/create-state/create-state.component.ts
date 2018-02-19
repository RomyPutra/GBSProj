import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/app-component-base';

import { CountryDto, PagedResultDtoOfCountryDto } from '@shared/models/model-country';
import { StateDto } from '@shared/models/model-state';
import { StateServiceProxy } from '@shared/service-proxies/proxy-state';
import { UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { CountryServiceProxy } from '@shared/service-proxies/proxy-country';
import { ActionState } from '@shared/models/enums';
import { AppConsts } from '@shared/AppConsts';
import { UtilsService } from '@abp/utils/utils.service';

import * as _ from "lodash";


// var _utilsService: UtilsService = new UtilsService();
// var userID = _utilsService.getCookieValue(AppConsts.authorization.userIDName);

@Component({
  selector: 'create-state-modal',
  templateUrl: './create-state.component.html'
})

export class CreateStateComponent extends AppComponentBase implements OnInit {

    @ViewChild('createStateModal') modal: ModalDirective;
    @ViewChild('modalContent') modalContent: ElementRef;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active: boolean = false;
    saving: boolean = false;
    state: StateDto = null;
    country: CountryDto[] = [];
    selectedCountryCode: string
    action: ActionState;
    ActionState = ActionState;
    isChecked1 : boolean;
    stsActive : number;
    isChecked2 : boolean;
    userID : string;

    constructor(
        injector: Injector,
        private _stateService: StateServiceProxy,
        private _countryService: CountryServiceProxy,
        private _utilsService: UtilsService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        // this._userService.getRoles()
        // .subscribe((result) => {
        //     this.roles = result.items;
        // });
    }

    show(action: ActionState, state?: StateDto): void {
        this.action = action;
        this.userID = this._utilsService.getCookieValue(AppConsts.authorization.userIDName);
        if (action === ActionState.Create) {
            this.active = true;
            this.modal.show();
            this.populateCountry();
            this.selectedCountryCode = '';
            this.state = new StateDto();
            this.state.init({ isActive: true });
            this.state.updateBy = this.userID;
            this.state.createBy = this.userID;
        } else if (action === ActionState.Edit) {
            if (state === undefined || state === null) {
                this.notify.error("The parameter 'state' must be defined and cannot be null.");
            } else {
                this.active = true;
                this.state = state;
                this.modal.show();
                this.populateCountry();
                this.state.updateBy = this.userID;
                this.state.createBy = this.userID;
            }
        }
    }

    onShown(): void {
        $.AdminBSB.input.activate($(this.modalContent.nativeElement));
    }

    save(): void {
        //TODO: Refactor this, don't use jQuery style code
        this.state.countryCode = this.selectedCountryCode;
        this.saving = true;

        if (this.action === ActionState.Create) {
            this.state.active = this.stsActive;
            this._stateService.create(this.state)
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
            this.state.active = this.stsActive;
            this._stateService.update(this.state)
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

    private populateCountry(): void {
        this._countryService.getAll(0, 0)
            .finally(() => {

            })
            .subscribe((result: PagedResultDtoOfCountryDto) => {
                this.country = result.items;
                this.selectedCountryCode = this.state.countryCode;
            });
    }

    onChangeCategorySelect(event) {
        this.selectedCountryCode = event.target.value;
        // if (value == 'add') {
        //     alert("add new cat");
        //     this.model.category = 'default';
        // }

        // //event.target.value = this.model.category;

        // this.model.subcategory = 'default';
    }

    changeCheckbox(field: string) {       
        this.state.active = this.state.active === 1 ? 0 : 1;
    }
}
