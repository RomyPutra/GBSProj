import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { CountryServiceProxy } from '@shared/service-proxies/proxy-country';
import { AppComponentBase } from '@shared/app-component-base';
import { CountryDto, PagedResultDtoOfCountryDto } from '@shared/models/model-country';

import * as _ from "lodash";

@Component({
    selector: 'edit-country-modal',
    templateUrl: './edit-country.component.html'
})
export class EditCountryComponent extends AppComponentBase {

    @ViewChild('editCountryModal') modal: ModalDirective;
    @ViewChild('modalContent') modalContent: ElementRef;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active: boolean = false;
    saving: boolean = false;
    country: CountryDto = null;
    confirmPassword = '';
    selectedAccessCode: string;

    constructor(
        injector: Injector,
        private _countryService: CountryServiceProxy
    ) {
        super(injector);
    }

    show(Country: CountryDto): void {
      this.active = true;
        this.country = Country;
        this.active = true;
        this.modal.show();
        // this.populateUserGroup();
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
        this.saving = true;
        this.country.flag = 1;
        this._countryService.update(this.country)
            .finally(() => { this.saving = false; })
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    // private populateUserGroup(): void {
    //     this._countryService.getAll(0, 0)
    //         .finally(() => {

    //         })
    //         .subscribe((result: PagedResultDtoOfCountryDto) => {
    //             this.Country = result.items;
    //             this.selectedAccessCode = this.user.accessCode;
    //         });
    // }

    changeSelect(){
        let temp = this.selectedAccessCode;
    }

    onChangeCategorySelect(event) {
        this.selectedAccessCode = event.target.value;

    }

    changeLabel() {
        this.country.active = this.country.active === 1 ? 0 : 1;
    }
}
