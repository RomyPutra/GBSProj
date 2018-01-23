import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { CustomerServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateCustomerDto } from '@shared/models/model-customer';
import { AppComponentBase } from '@shared/app-component-base';

import * as _ from "lodash";

@Component({
  selector: 'create-customer-modal',
  templateUrl: './create-customer.component.html'
})
export class CreateCustomerComponent extends AppComponentBase {

    @ViewChild('createCustomerModal') modal: ModalDirective;
    @ViewChild('modalContent') modalContent: ElementRef;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active: boolean = false;
    saving: boolean = false;
    customer: CreateCustomerDto = null;
    
    constructor(
        injector: Injector,
        private _customerService: CustomerServiceProxy
    ) {
        super(injector);
    }

    show(): void {
        this.active = true;
        this.modal.show();
        this.customer = new CreateCustomerDto();
        this.customer.init({ isActive: true });
    }

    onShown(): void {
        $.AdminBSB.input.activate($(this.modalContent.nativeElement));
    }

    save(): void {
        // this.saving = true;
        // this._customerService.create(this.customer)
        //     .finally(() => { this.saving = false; })
        //     .subscribe(() => {
        //         this.notify.info(this.l('SavedSuccessfully'));
        //         this.close();
        //         this.modalSave.emit(null);
        //     });

        // Prints in console
        console.log(this.customer);
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
