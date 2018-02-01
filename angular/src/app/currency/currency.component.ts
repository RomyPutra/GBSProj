import { AppConsts } from './../../shared/AppConsts';
import { ActionState } from '@shared/models/enums';
import { Component, Injector, ViewChild, Inject } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { CurrencyDto, PagedResultDtoOfCurrencyDto } from '@shared/models/model-currency';
import { CurrencyServiceProxy } from '@shared/service-proxies/proxy-currency';
import { PagedRequestDto } from 'shared/paged-listing-component-base';
import { PagedListingComponentCustom } from 'shared/layout/paged-listing-component-custom';
import { ModalCurrencyComponent } from './modal-currency/modal-currency.component';
import { DxDataGridComponent, DxDataGridModule, DxSelectBoxModule } from 'devextreme-angular';

@Component({
    templateUrl: './currency.component.html',
    animations: [appModuleAnimation()]
})
export class CurrencyComponent extends PagedListingComponentCustom<CurrencyDto> {

    @ViewChild('createEditCurrencyModal') createEditCurrencyModal: ModalCurrencyComponent;
    @ViewChild(DxDataGridComponent) dataGrid: DxDataGridComponent;

    active: boolean = false;

    loadOptions: any;
    dataSource: CurrencyDto[] = [];

    constructor(
        injector: Injector,
        private _service: CurrencyServiceProxy
    ) {
        super(injector);
    }

    protected refresh(): void {
        this.isTableLoading = true;
        this._service.getAll()
            .finally(() => {
                this.isTableLoading = false;
            })
            .subscribe((result: PagedResultDtoOfCurrencyDto) => {
                this.dataSource = result.items;
            });
    }

    protected delete(data: CurrencyDto): void {
        abp.message.confirm(
            "Delete currency '" + data.currencyDesc + "'?",
            (result: boolean) => {
                if (result) {
                  this._service.delete(data)
                      .subscribe(() => {
                          abp.notify.info("Deleted currency: " + data.currencyCode);
                          this.refresh();
                      });
                }
            }
        );
    }

    // Show Modals
    create(): void {
        this.createEditCurrencyModal.show(ActionState.Create);
    }

    edit(data: CurrencyDto): void {
        this.createEditCurrencyModal.show(ActionState.Edit, data);
    }
}
