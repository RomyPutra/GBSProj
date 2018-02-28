import { Component, Injector, ViewChild } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PagedListingComponentBase, PagedRequestDto } from 'shared/paged-listing-component-base';
import data_grid from 'devextreme/ui/data_grid';
import { AgentTierDto, PagedResultDtoOfAgentTierDto } from '@shared/models/model-gbsagent';
import { GetAgentTierServiceProxy } from '@shared/service-proxies/proxy-agenttier';
import { UploadDto, UploadAgentTierDto } from '@shared/models/model-upload';
import { UploadEvent, UploadFile } from 'ngx-file-drop';
import * as XLSX from 'xlsx';
type AOA = any[][];

@Component({
  templateUrl: './agenttier.component.html',
	animations: [appModuleAnimation()]
})
export class AgenttierComponent extends PagedListingComponentBase<AgentTierDto> {
  fileToUpload: File = null;
  excelToUpload: any;
  excelToUploadHeader: any;
  excelError: Number = 0;
  upload: UploadDto;
  fileupload: AgentTierDto[];
  pendingfile: UploadAgentTierDto[] = [];
  fileModel: any;
  public files: UploadFile[] = [];
  active: boolean = false;
  saving: boolean = false;
  code: string;
  group: AgentTierDto[];
  listGrid: Array<AgentTierDto> = [];
  hold: number = 0;
  hold1: number = 1;
  
  constructor(
    injector: Injector,
    private _gbsService: GetAgentTierServiceProxy
  ) {
    super(injector);
  }
  
  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this._gbsService.getAgentTier(request.skipCount, request.maxResultCount)
      .finally(() => {
        finishedCallback();
      })
      .subscribe((result: PagedResultDtoOfAgentTierDto) => {
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
  
  protected delete(entity: AgentTierDto): void {
    throw new Error('Method not implemented.');
  }
  
  edit(data: any): void {
    if (this.hold === this.hold1) {
      this.hold = 1;
      // this._gbsService.update(this.listGrid)
      // .finally(() => {
      //   this.saving = false;
      // })
      // .subscribe((result: AgentTierDto[]) => {
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
  onExcelChange(event: any) {
    /* wire up file reader */
    const target: DataTransfer = <DataTransfer>(event.target);
    if (target.files.length !== 1) {
      throw new Error('Cannot use multiple files')
    } else {
      this.onExcelLoad(target.files[0]);
    }
  }

  onExcelLoad(file: any) {

    const reader: FileReader = new FileReader();
    reader.readAsBinaryString(file);
    reader.onload = (e: any) => {
      /* read workbook */
      const bstr: string = e.target.result;
      const wb: XLSX.WorkBook = XLSX.read(bstr, { type: 'binary' });
      /* grab first sheet */
      const wsname: string = wb.SheetNames[0];
      const ws: XLSX.WorkSheet = wb.Sheets[wsname];
      /* save data */
      this.excelToUpload = <AOA>(XLSX.utils.sheet_to_json(ws, { header: 1 }));

      if (this.excelToUpload[0].length <= 3 && this.excelToUpload[0][0] === 'Group Time' && this.excelToUpload[0][1] === 'Started Time' && this.excelToUpload[0][2] === 'Ended Time') {
        this.excelToUploadHeader = this.excelToUpload[0];
        this.excelToUpload.shift();
        let fileuploadx = this.excelToUpload.map(item => new AgentTierDto(this.group[0], item));
        this.fileupload = [];
        this.group.forEach(fu => {
          fileuploadx = fileuploadx.filter(t => t.marketCode !== fu.marketCode);
        });
        this.fileupload = fileuploadx;
        this.pendingfile = this.excelToUpload.map(item => new UploadAgentTierDto(item));
        this.excelError = this.pendingfile.filter(option => option.hasError).length;
        this.pendingfile.sort((a, b) => {
          if (a.hasError) {
            return -1;
          } else {
            return 1;
          }
        });
      } else {
        this.clearExcelData();
      }
    };
  }

  uploadFileToActivity() {
    this._gbsService.post(this.fileupload)
      .finally(() => {

      })
      .subscribe(result => {
				if (result) {
					this.notify.info(this.l('SavedSuccessfully'));
					this.hold = 0;
          this.clearExcelData();
          this._gbsService.getAgentTier(0, 10)
          .finally(() => {
          })
          .subscribe((result: PagedResultDtoOfAgentTierDto) => {
            if (result.items.length > 0) {
              this.group = result.items;
            }
          });
            } else {
					this.notify.error('Save failed!');
					this.hold = 0;
				}
      });
  }

  clearExcelData() {
    this.fileModel = undefined;
    this.excelToUpload = undefined;
    this.excelToUploadHeader = undefined;
    this.pendingfile = [];
    this.excelError = 0;
    this.pendingfile.sort((a, b) => {
      if (a.hasError) {
        return -1;
      } else {
        return 1;
      }
    });
  }
}
