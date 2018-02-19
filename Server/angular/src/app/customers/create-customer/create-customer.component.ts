import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { CustomerServiceProxy } from '@shared/service-proxies/proxy-customer';
import { CustomerDto } from '@shared/models/model-customer';
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
    customer: CustomerDto = null;
    status: string;

    constructor(
        injector: Injector,
        private _customerService: CustomerServiceProxy
    ) {
        super(injector);
    }

    show(): void {
        this.active = true;
        this.modal.show();
        this.customer = new CustomerDto();
        this.customer.init({ isActive: true });
    }

    onShown(): void {
        $.AdminBSB.input.activate($(this.modalContent.nativeElement));
    }

    changeLabel() {
        this.customer.active = this.customer.active == 1 ? 0 : 1;
        if (this.customer.active == 1) {
            this.status = "Active";
        } else {
            this.status = "Inactive";
        }
    }

    save(): void {
        this.saving = true;
        this.customer.bizRegID = this.generateID();
        this.customer.createBy = "SYSTEM";
        this.customer.createDate = new Date();
        this.customer.flag = 1;
        this.customer.active = 1;
        this._customerService.create(this.customer)
            .finally(() => { this.saving = false; })
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    private generateID() {
        let text = "";
        let possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        for (var i = 0; i < 12; i++) {
            text += possible.charAt(Math.floor(Math.random() * possible.length));
        }

        return text;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
