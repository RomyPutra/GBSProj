import { Component, Injector, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { UtilsService } from '@abp/utils/utils.service';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { CustomerDto, PagedResultDtoOfCustomerDto } from '@shared/models/model-customer';
import { CustomerServiceProxy } from '@shared/service-proxies/proxy-customer';
import { PagedRequestDto } from 'shared/paged-listing-component-base';
import { CreateCustomerComponent } from './create-customer/create-customer.component';

import { AppConsts } from '@shared/AppConsts';
import { PagedListingComponentCustom } from 'shared/layout/paged-listing-component-custom';
import { DxDataGridComponent, DxDataGridModule, DxSelectBoxModule } from 'devextreme-angular';
import CustomStore from 'devextreme/data/custom_store';

@Component({
    templateUrl: './customers.component.html',
    animations: [appModuleAnimation()]
})

export class CustomersComponent extends PagedListingComponentCustom<CustomerDto> {

    @ViewChild('createCustomerModal') createCustomerModal: CreateCustomerComponent;
    @ViewChild(DxDataGridComponent) dataGrid: DxDataGridComponent;

    loadOptions: any;
    dataSource: any = {};
    loadPanel = {'enabled': false};
    filterText = '';
    _timeoutFilter: any;
    isFiltering = false;
    selectedState: string;

    constructor(
        injector: Injector,
        private _route: Router,
        private _customerService: CustomerServiceProxy
    ) {
        super(injector);
        this.selectedState = new UtilsService().getCookieValue(AppConsts.otherSetting.state);
        this.loadingMessage = 'Loading...';
    }

    onFilterChange() {
        if (this._timeoutFilter) { // if there is already a timeout in process cancel it
            window.clearInterval(this._timeoutFilter);
        }
        let that = this;
        this._timeoutFilter = window.setInterval(function() {
            if (that.filterText.length > 0 || that.isFiltering) {
                if (!that.isFiltering) {
                    that.isFiltering = true;
                } else if (that.filterText.length > 0) {
                    that.isFiltering = false;
                }
                window.clearInterval(that._timeoutFilter);
                that.dataGrid.instance.refresh();
            }
        }, 1000);
    }

    protected refresh(): void {
        this.isTableLoading = true;
        if (this.dataSource.store) {
            this.dataGrid.instance.refresh();
        } else {
            let that = this;
            this.dataSource.store = new CustomStore({
                load: function (loadOptions: any) {
                    that.loadOptions = loadOptions;
                    let filterText = '';

                    if (that.isValidFilter(that.filterText, that.filterValidLenght)) {
                        filterText = that.filterText;
                    }

                    if (loadOptions.filter) {
                        if (loadOptions.filter.length > 0) {
                            filterText = loadOptions.filter[0][2];
                        }
                    }

                    let orderby = '';
                    if (loadOptions.sort) {
                        orderby += loadOptions.sort[0].selector;
                        if (loadOptions.sort[0].desc) {
                            orderby += ' desc';
                        }
                    } else {
                        throw new Error("One of data column must have 'sortOrder'");
                    }

                    let d = $.Deferred();

                    that._customerService.getListEntity(loadOptions.skip, loadOptions.take, that.selectedState, orderby, filterText)
                    .finally(() => {
                        that.isTableLoading = false;
                    })
                    .subscribe((result: PagedResultDtoOfCustomerDto) => {
                        d.resolve(result.items, { totalCount: result.totalCount });
                    });

                    return d.promise();
                }
            });
        }
    }

    protected delete(customer: CustomerDto): void {
        // abp.message.confirm(
        //     "Delete customer '" + customer.bizRegID + "'?",
        //     (result: boolean) => {
        //         if (result) {
        //             this._customerService.delete(customer)
        //                 .subscribe(() => {
        //                     abp.notify.info("Deleted customer: " + customer.companyName);
        //                     this.refresh();
        //                 });
        //         }
        //     }
        // );
    }

    // Show Modals
    // createCustomer(): void {
    //     abp.notify.info("Under construction.");
    //     // this.createCustomerModal.show();
    // }

    // Show new page component
    editCustomer(customer: CustomerDto): void {
        this._route.navigate(['/app/customer/edit', customer.bizRegID]);
    }
}
