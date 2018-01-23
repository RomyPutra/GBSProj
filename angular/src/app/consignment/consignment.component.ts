import { Component, Injector, ViewChild } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { ConsignmentDto, PagedResultDtoOfConsignmentDto } from '@shared/models/model-consignment';
import { ConsignmentServiceProxy } from '@shared/service-proxies/service-proxies';
import { PagedListingComponentBase, PagedRequestDto } from 'shared/paged-listing-component-base';

@Component({
  templateUrl: './consignment.component.html',
    animations: [appModuleAnimation()]
})

export class ConsignmentComponent extends PagedListingComponentBase<ConsignmentDto> {
  active: boolean = false;
  datas: ConsignmentDto[] = [];

  constructor(
      injector: Injector,
      private _service: ConsignmentServiceProxy
  ) {
      super(injector);
  }

  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    //   data load partially based on page
    // this._service.getAll(request.skipCount, request.maxResultCount)
    //     .finally(() => {
    //         finishedCallback();
    //     })
    //     .subscribe((result: PagedResultDtoOfConsignmentDto) => {
    //         this.datas = result.items;
    //         this.allDatas = result.items;
    //         this.showPaging(result, pageNumber);
    //     });

    //   data load all when first load
    if (this.datas.length > 0) {
        this.pageNumber = pageNumber;
        finishedCallback();
    } else {
        this._service.getAll(0, 0)
            .finally(() => {
                finishedCallback();
            })
            .subscribe((result: PagedResultDtoOfConsignmentDto) => {
                this.datas = result.items;
                this.showPaging(result, pageNumber);
            });
    }
  }

  protected delete(data: ConsignmentDto): void {
    //   abp.message.confirm(
    //       "Delete Consignment '" + data. + "'?",
    //       (result: boolean) => {
    //           if (result) {
    //             //   this._userService.delete(user.id)
    //             //       .subscribe(() => {
    //             //           abp.notify.info("Deleted User: " + user.fullName);
    //             //           this.refresh();
    //             //       });
    //           }
    //       }
    //   );
  }

  // Show Modals
  createConsignment(): void {
    //   this.createUserModal.show();
  }

  editConsignment(user: ConsignmentDto): void {
    //   this.editUserModal.show(user.id);
  }
}
