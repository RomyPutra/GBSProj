import { Component, Injector, ViewChild } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PagedListingComponentBase, PagedRequestDto } from 'shared/paged-listing-component-base';
import data_grid from 'devextreme/ui/data_grid';
import { DxCheckBoxModule } from 'devextreme-angular';
import { AppComponentBase } from '@shared/app-component-base';
import { GB4Dto, PagedResultDtoOfGB4Dto, OrgGB4Dto, PagedResultDtoOfOrgGB4Dto } from '@shared/models/model-GB4';
import { GetGB4ServiceProxy } from '@shared/service-proxies/proxy-GB4';

@Component({
  templateUrl: './GB4.component.html',
	animations: [appModuleAnimation()]
})
export class GB4Component extends PagedListingComponentBase<GB4Dto> {
  active: boolean = false;
  saving: boolean = false;
  selectedItems: any[] = [];
  code: string;
  group: GB4Dto[];
  groupOrg: OrgGB4Dto[];
  listGrid: Array<GB4Dto> = [];
  hold: number = 0;
  hold1: number = 1;
  organisation: any;
  
  constructor(
    injector: Injector,
    private _gbsService: GetGB4ServiceProxy
  ) {
    super(injector);
          this._gbsService.getAgnGB4()
          .subscribe((result: PagedResultDtoOfOrgGB4Dto) => {
            if (result.items.length > 0) {
              this.groupOrg = result.items;
            }
            this.showPaging(result, 1);
          });
    this.agentEdit = this.agentEdit.bind(this);
  }
  
  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this._gbsService.getGB4(request.skipCount, request.maxResultCount)
      .finally(() => {
        finishedCallback();
      })
      .subscribe((result: PagedResultDtoOfGB4Dto) => {
        if (result.items.length > 0) {
          this.group = result.items;
        }
        this.showPaging(result, pageNumber);
      });
    this._gbsService.getOrgGB4(request.skipCount, request.maxResultCount)
      .finally(() => {
        this.organisation = {store: this.groupOrg,
          paginate: true,
          pageSize: 10};
          //console.log(this.organisation);
        finishedCallback();
      })
      .subscribe((result: PagedResultDtoOfOrgGB4Dto) => {
        if (result.items.length > 0) {
          this.groupOrg = result.items;
        }
        this.showPaging(result, pageNumber);
      });
  }

  agentEdit(options) {
    let filtered = [];
    if (this.groupOrg) {
      if (this.groupOrg.length > 0) {
        if (options.data) {
          filtered = this.groupOrg.filter(t => t.orgName === options.data.orgName);
        }
      }
    }
    // console.log(options);
    // console.log(this.groupOrg);
    return {store: filtered};
  }

  onContentReady(e) {
       e.component.columnOption('command:edit', {
          visibleIndex: -1,
          width: 80
      });
  }

  onEditorPreparing(e) {
    if (e.parentType === 'dataRow' && e.dataField === 'username') {
      e.editorOptions.disabled = (e.row.data.orgID !== null);
      // console.log(e.editorOptions.disabled + 'a');
    }
    if (e.parentType === 'dataRow' && e.dataField === 'countryCode') {
      e.editorOptions.disabled = true;//(e.row.data.orgID !== null);
      // console.log(e.editorOptions.disabled + 'b');
    }
  }

  setAgentValue(rowData: any, value: string): void {
    rowData.orgID = null;
    (<any>this).defaultSetCellValue(rowData, value);
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
  
  selectionChanged(data: any) {
    this.selectedItems = data.selectedRowsData;
  }
  deleteRecords() {
      this.selectedItems.forEach((item) => {
          // this.dataSource.remove(item);
          // this.dataGrid.instance.refresh();
      });
  }
  
  protected delete(entity: GB4Dto): void {
    throw new Error('Method not implemented.');
  }
  
  edit(data: any): void {
    if (this.hold === this.hold1) {
      this.hold = 1;
      // this._gbsService.update(this.listGrid)
      // .finally(() => {
      //   this.saving = false;
      // })
      // .subscribe((result: GB4Dto[]) => {
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
}  
