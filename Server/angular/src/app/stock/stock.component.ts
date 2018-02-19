import { Component, Injector, ViewChild } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { StockDto, PagedResultDtoOfStockDto } from '@shared/models/model-stock';
import { StockServiceProxy } from '@shared/service-proxies/service-proxies';
import { PagedRequestDto } from 'shared/paged-listing-component-base';
import { PagedListingComponentCustom } from 'shared/layout/paged-listing-component-custom';
import { CreateStockComponent } from './create-stock/create-stock.component';
import { ActionState } from '@shared/models/enums';

@Component({
  templateUrl: './stock.component.html',
    animations: [appModuleAnimation()]
})
// export class UnitsComponent implements OnInit {
export class StockComponent extends PagedListingComponentCustom<StockDto> {
  active: boolean = false;
  stock: StockDto[] = [];
//   @ViewChild('editStockModal') editStockModal: EditStockComponent;
  @ViewChild('createStockModal') createStockModal: CreateStockComponent;

  constructor(
      injector: Injector,
      private _stockService: StockServiceProxy
  ) {
      super(injector);
  }

  protected refresh(): void {
    this.isTableLoading = true;
      this._stockService.getAll(0, 0)
          .finally(() => {
              this.isTableLoading = false;
          })
          .subscribe((result: PagedResultDtoOfStockDto) => {
              this.stock = result.items;
          });
  }

  protected delete(stock: StockDto): void {
      abp.message.confirm(
          "Delete stock '" + stock.itemDesc + "'?",
          (result: boolean) => {
              if (result) {
                //   this._userService.delete(user.id)
                //       .subscribe(() => {
                //           abp.notify.info("Deleted User: " + user.fullName);
                //           this.refresh();
                //       });
              }
          }
      );
  }

  // Show Modals
    create(): void {
        this.createStockModal.show(ActionState.Create);
    }

    edit(user: StockDto): void {
        this.createStockModal.show(ActionState.Edit, user);
    }
}
