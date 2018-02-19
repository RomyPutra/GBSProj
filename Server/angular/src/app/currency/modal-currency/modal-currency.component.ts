import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { CurrencyServiceProxy } from '@shared/service-proxies/proxy-currency';
import { CurrencyDto } from '@shared/models/model-currency';
import { RoleDto } from '@shared/models/model-role';
import { AppComponentBase } from '@shared/app-component-base';
import { CountryDto, PagedResultDtoOfCountryDto } from 'shared/models/model-country';
import { CountryServiceProxy } from '@shared/service-proxies/proxy-country';
import { ActionState } from '@shared/models/enums';
import { AppConsts } from '@shared/AppConsts';
import { UtilsService } from '@abp/utils/utils.service';

import * as _ from "lodash";

@Component({
  selector: 'create-edit-currency-modal',
  templateUrl: './modal-currency.component.html'
})

export class ModalCurrencyComponent extends AppComponentBase implements OnInit {

    @ViewChild('createEditCurrencyModal') modal: ModalDirective;
    @ViewChild('modalContent') modalContent: ElementRef;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active: boolean = false;
    saving: boolean = false;
    data: CurrencyDto = null;
    countries: CountryDto[] = [];
    isActive: boolean;
    userID: string;

    action: ActionState;
    ActionState = ActionState;

    constructor(
        injector: Injector,
        private _service: CurrencyServiceProxy,
        private _serviceCountry: CountryServiceProxy
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.populateCountry();
        this.userID = new UtilsService().getCookieValue(AppConsts.authorization.userIDName);
    }

    show(action: ActionState, data?: CurrencyDto): void {
        this.action = action;
        if (action === ActionState.Create) {
            this.active = true;
            this.modal.show();
            this.data = new CurrencyDto();
            this.data.init({ active: true });
            this.isActive = this.data.active === 1;
            this.data.createBy = this.userID;
        } else if (action === ActionState.Edit) {
            if (data === undefined || data === null) {
                this.notify.error("The parameter 'data' must be defined and cannot be null.");
            } else {
                this.active = true;
                this.data = data;
                this.isActive = this.data.active === 1;
                this.data.createBy = this.userID;
                this.modal.show();
            }
        }
    }

    private populateCountry(): void {
        this._serviceCountry.getAll(0, 0)
            .finally(() => {

            })
            .subscribe((result: PagedResultDtoOfCountryDto) => {
                this.countries = result.items;
            });
    }

    onShown(): void {
        $.AdminBSB.input.activate($(this.modalContent.nativeElement));
    }

    save(): void {
        //TODO: Refactor this, don't use jQuery style code
        // this.data.accessCode = this.data.accessCode;
        this.saving = true;
        this.data.active = this.isActive ? 1 : 0;
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
