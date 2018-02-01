import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';

import { StateServiceProxy } from '@shared/service-proxies/proxy-state';
import { CountryServiceProxy } from '@shared/service-proxies/proxy-country';
import { StateDto, PagedResultDtoOfStateDto } from '@shared/models/model-state';
import { CountryDto, PagedResultDtoOfCountryDto } from '@shared/models/model-country';
import { AppComponentBase } from '@shared/app-component-base';

import * as _ from "lodash";


@Component({
    selector: 'edit-state-modal',
    templateUrl: './edit-state.component.html'
})
export class EditStateComponent extends AppComponentBase {

    @ViewChild('editStateModal') modal: ModalDirective;
    @ViewChild('modalContent') modalContent: ElementRef;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active: boolean = false;
    saving: boolean = false;
    state: StateDto = null;
    country : CountryDto [] = [];
    selectedCountryCode: string;
    isChecked1 : boolean;
    stsActive : number;
    // isChecked2 : boolean;
 

    constructor(
        injector: Injector,
        private _stateService: StateServiceProxy,
        private _countryService: CountryServiceProxy
    ) {
        super(injector);
    }

    show(state: StateDto): void {
      this.active = true;
        this.state = state;
        this.modal.show();
        this.populateCountry();
        
    }

    onShown(): void {
        $.AdminBSB.input.activate($(this.modalContent.nativeElement));
    }

    save(): void {
        //TODO: Refactor this, don't use jQuery style code
        this.state.countryCode = this.selectedCountryCode;
        this.saving = true;
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

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    private populateCountry(): void {
        this._countryService.getAll(-1, -1)
            .finally(() => {

            })
            .subscribe((result: PagedResultDtoOfCountryDto) => {
                this.country = result.items;
                this.selectedCountryCode = this.state.countryCode;
            });
    }

    changeSelect() {
        let temp = this.selectedCountryCode;
    }

    onChangeCategorySelect(event) {
        this.selectedCountryCode = event.target.value;

    }

    changeCheckbox(field: string) {       
        if (this.isChecked1 = true) {
            this.stsActive = 1;
        } else {
            this.stsActive = 0;
        }
        
    }
}
