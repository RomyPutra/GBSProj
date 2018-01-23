import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { CustomerServiceProxy } from '@shared/service-proxies/service-proxies';
import { CustomerDto } from '@shared/models/model-customer';
import { AppComponentBase } from '@shared/app-component-base';

import * as _ from "lodash";

@Component({
    selector: 'edit-customer-modal',
    templateUrl: './edit-customer.component.html'
})
export class EditCustomerComponent extends AppComponentBase {

    @ViewChild('editCustomerModal') modal: ModalDirective;
    @ViewChild('modalContent') modalContent: ElementRef;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active: boolean = false;
    saving: boolean = false;

    customer: CustomerDto = null;
    status: string;
    
    constructor(
        injector: Injector,
        private _customerService: CustomerServiceProxy
    ) {
        super(injector);
    }

    show(bizRegID: string): void {
        this._customerService.get(bizRegID)
            .subscribe(
            (result) => {
                this.customer = result;
                if (result.status == 0) {
                    this.status = "Inactive";
                } else {
                    this.status = "Active";
                }
                this.active = true;
                this.modal.show();
            }
            );
    }

    onShown(): void {
        $.AdminBSB.input.activate($(this.modalContent.nativeElement));
    }

    changeLabel() {
        this.customer.status = this.customer.status === 1 ? 0 : 1;
        if (this.customer.status == 1){
            this.status = "Active";
        } else {
            this.status = "Inactive";
        }
    }

    save(): void {
        // this.saving = true;
        // this._customerService.update(this.customer)
        //     .finally(() => { this.saving = false; })
        //     .subscribe(() => {
        //         this.notify.info(this.l('SavedSuccessfully'));
        //         this.close();
        //         this.modalSave.emit(null);
        //     });

        // Prints to console
        console.log(this.customer);
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
