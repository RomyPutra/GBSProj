import { Component, Injector, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { CustomerDto, PagedResultDtoOfCustomerDto } from '@shared/models/model-customer';
import { CustomerServiceProxy } from '@shared/service-proxies/service-proxies';
import { PagedListingComponentBase, PagedRequestDto } from 'shared/paged-listing-component-base';
import { CreateCustomerComponent } from './create-customer/create-customer.component';
import { EditCustomerComponent } from './edit-customer/edit-customer.component';

@Component({
    templateUrl: './customers.component.html',
    animations: [appModuleAnimation()]
})

export class CustomersComponent extends PagedListingComponentBase<CustomerDto> {

    @ViewChild('createCustomerModal') createCustomerModal: CreateCustomerComponent;
    @ViewChild('editCustomerModal') editCustomerModal: EditCustomerComponent;

    active: boolean = false;
    customer: CustomerDto[] = [];

    constructor(
        injector: Injector,
        private _route: Router,
        private _customerService: CustomerServiceProxy
    ) {
        super(injector);
    }

    protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
        this._customerService.getAll(request.skipCount, request.maxResultCount)
            .finally(() => {
                finishedCallback();
            })
            .subscribe((result: PagedResultDtoOfCustomerDto) => {
                this.customer = result.items;
                this.showPaging(result, pageNumber);
            });
    }

    protected delete(customer: CustomerDto): void {
        abp.message.confirm(
            "Delete customer '" + customer.bizRegID + "'?",
            (result: boolean) => {
                if (result) {
                    // this._userService.delete(user.id)
                    //     .subscribe(() => {
                    //         abp.notify.info("Deleted User: " + user.fullName);
                    //         this.refresh();
                    //     });
                }
            }
        );
    }

    // Show Modals
    createCustomer(): void {
        this.createCustomerModal.show();
    }

    editCustomer(customer: CustomerDto): void {
        this.editCustomerModal.show(customer.bizRegID);
    }
}
