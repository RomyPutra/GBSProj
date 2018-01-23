import { Component, Injector, ViewChild } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { UnitDto, PagedResultDtoOfUnitDto } from '@shared/models/model-unit';
import { UnitServiceProxy } from '@shared/service-proxies/service-proxies';
import { PagedListingComponentBase, PagedRequestDto } from 'shared/paged-listing-component-base';

@Component({
  templateUrl: './units.component.html',
    animations: [appModuleAnimation()]
})
// export class UnitsComponent implements OnInit {
export class UnitsComponent extends PagedListingComponentBase<UnitDto> {
  active: boolean = false;
  units: UnitDto[] = [];

  constructor(
      injector: Injector,
      private _userService: UnitServiceProxy
  ) {
      super(injector);
  }

  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
      this._userService.getAll(request.skipCount, request.maxResultCount)
          .finally(() => {
              finishedCallback();
          })
          .subscribe((result: PagedResultDtoOfUnitDto) => {
              this.units = result.items;
              this.showPaging(result, pageNumber);
          });
  }

  protected delete(unit: UnitDto): void {
      abp.message.confirm(
          "Delete unit '" + unit.uomCode + "'?",
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
  createUnit(): void {
    //   this.createUserModal.show();
  }

  editUnit(user: UnitDto): void {
    //   this.editUserModal.show(user.id);
  }
}
