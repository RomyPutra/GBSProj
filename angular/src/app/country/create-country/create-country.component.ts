import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { CountryServiceProxy } from '@shared/service-proxies/proxy-country';
import { CountryDto } from '@shared/models/model-country';
import { AppComponentBase } from '@shared/app-component-base';

import * as _ from "lodash";

@Component({
  selector: 'create-country-modal',
  templateUrl: './create-country.component.html'
})
export class CreateCountryComponent extends AppComponentBase implements OnInit {

    @ViewChild('createCountryModal') modal: ModalDirective;
    @ViewChild('modalContent') modalContent: ElementRef;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active: boolean = false;
    saving: boolean = false;
    country: CountryDto = null;
    confirmPassword = '';
    selectedAccessCode: string
    constructor(
        injector: Injector,
        private _countryService: CountryServiceProxy
    ) {
        super(injector);
    }

    ngOnInit(): void {
        // this._userService.getRoles()
        // .subscribe((result) => {
        //     this.roles = result.items;
        // });
    }

    show(): void {
        this.active = true;
        this.modal.show();
        this.confirmPassword = '';
        this.selectedAccessCode = '';
        this.country = new CountryDto();
        this.country.init({ isActive: true });
    }

    onShown(): void {
        $.AdminBSB.input.activate($(this.modalContent.nativeElement));
    }

    save(): void {
        this.saving = true;
        this.country.createDate = new Date();
        this.country.createBy = "SYSTEM";
        this.country.flag = 1;
        this._countryService.insert(this.country)
            .finally(() => { this.saving = false; })
            .subscribe((result) => {
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

    changeLabel() {
        this.country.active = this.country.active === 1 ? 0 : 1;
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
