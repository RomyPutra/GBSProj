import { Component, Injector, ViewChild } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PagedListingComponentBase, PagedRequestDto } from 'shared/paged-listing-component-base';
import data_grid from 'devextreme/ui/data_grid';
import { LFDiscDto, PagedResultDtoOfLFDiscDto } from '@shared/models/model-lfdisc';
import { GetLFDiscServiceProxy } from '@shared/service-proxies/proxy-lfdisc';

@Component({
  templateUrl: './lfdisc.component.html',
	animations: [appModuleAnimation()]
})
export class LFDiscComponent extends PagedListingComponentBase<LFDiscDto> {
  active: boolean = false;
  saving: boolean = false;
  code: string;
  group: LFDiscDto[];
  listGrid: Array<LFDiscDto> = [];
  hold: number = 0;
  hold1: number = 1;
  
  constructor(
    injector: Injector,
    private _gbsService: GetLFDiscServiceProxy
  ) {
    super(injector);
  }
  
  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this._gbsService.getLFDisc(request.skipCount, request.maxResultCount)
      .finally(() => {
        finishedCallback();
      })
      .subscribe((result: PagedResultDtoOfLFDiscDto) => {
        if (result.items.length > 0) {
          this.group = result.items;
        }
        this.showPaging(result, pageNumber);
      });
  }
  
  onContentReady(e) {
       e.component.columnOption('command:edit', {
    visibleIndex: -1,
          width: 80
      });
  }

  onCellPrepared(e) {
      if (e.rowType === 'data' && e.column.command === 'edit') {
          var isEditing = e.row.isEditing,
              cellElement = e.cellElement;

          if (isEditing) {
      let saveLink = cellElement.querySelector('.dx-link-save'),
        cancelLink = cellElement.querySelector('.dx-link-cancel');

              saveLink.classList.add('dx-icon-save');
              cancelLink.classList.add('dx-icon-revert');

              saveLink.textContent = '';
             cancelLink.textContent = '';
          } else {
      let editLink = cellElement.querySelector('.dx-link-edit');
      // ,deleteLink = cellElement.querySelector('.dx-link-delete');

              editLink.classList.add('dx-icon-edit');
              // deleteLink.classList.add('dx-icon-trash');

              editLink.textContent = 'Edit';
              // deleteLink.textContent = '';
          }
      }
  }
  
  protected delete(entity: LFDiscDto): void {
    throw new Error('Method not implemented.');
  }
  
  edit(data: any): void {
    if (this.hold === this.hold1) {
      this.hold = 1;
      // this._gbsService.update(this.listGrid)
      // .finally(() => {
      //   this.saving = false;
      // })
      // .subscribe((result: FloorFareDto[]) => {
      //   if (result) {
      //     this.notify.info(this.l('SavedSuccessfully'));
      //     this.hold = 0;
      //   } else {
      //     this.notify.error('Save failed!');
      //     this.hold = 0;
      //   }
      // });	
    } else {
      this.hold1 = this.hold1 + 1;
    };
  }
  
  logEvents(data: any) {
    // if (data.newData['countryCode'] === undefined) {
      this.hold = this.hold + 1;
      this.hold1 = 1;
      this.listGrid.push(data.key);
    // } else {
    //   this.code = data.newData['countryCode'];
    //   this._gbsService.get(this.code)
    //   .subscribe((result: GBSCountryDto) => {
    //     this.countryx = result;
    //     data.key['currencyCode'] = this.countryx.currencyCode;
    //     this.hold = this.hold + 1;
    //     this.hold1 = 1;
    //     this.listGrid.push(data.key);
    //   });
    // }
  }
}
