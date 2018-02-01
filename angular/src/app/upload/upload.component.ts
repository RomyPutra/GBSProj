import { UploadDto, UploadPendingRecvDto } from '@shared/models/model-upload';
import { UploadServiceProxy } from '@shared/service-proxies/proxy-upload';
import { Component, Injector, ViewChild, InjectionToken, OnInit, ElementRef } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { FormBuilder, FormGroup, FormControl, Validators, NgForm } from '@angular/forms';
import { UploadEvent, UploadFile } from 'ngx-file-drop';
import * as XLSX from 'xlsx';
type AOA = any[][];

@Component({
  templateUrl: './upload.component.html',
  animations: [appModuleAnimation()]
})

export class UploadComponent extends AppComponentBase implements OnInit {
  @ViewChild('formUpload') formUpload: ElementRef;
  fileToUpload: File = null;
  excelToUpload: any;
  excelToUploadHeader: any;
  excelError: Number = 0;
  upload: UploadDto;
  pendingRecv: UploadPendingRecvDto[] = [];
  fileModel: any;
  public files: UploadFile[] = [];

  constructor(
    injector: Injector,
    private _uploadService: UploadServiceProxy
  ) {
    super(injector);
  }

  ngOnInit() {
  }

  handleFileInput(files: FileList) {
    this.fileToUpload = files.item(0);
  }

  uploadFileToActivity() {
    this._uploadService.post(this.fileToUpload)
      .finally(() => {

      })
      .subscribe(result => {
        let temp = result;
      });
  }

  save(form: NgForm) {
    // this._uploadService.post(form.value)
    // .finally(() => {

    // })
    // .subscribe(result => {
    //   let temp = result;
    // });
  }

  clearExcelData() {
    this.fileModel = undefined;
    this.excelToUpload = undefined;
    this.excelToUploadHeader = undefined;
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
    };

    this.excelToUploadHeader = this.excelToUpload[0];

    this.excelToUpload.shift();
    this.pendingRecv = this.excelToUpload.map(item => new UploadPendingRecvDto(item));

    this.excelError = this.pendingRecv.filter(option => option.hasError).length;

    this.pendingRecv.sort((a, b) => {
      if (a.hasError) {
        return -1;
      } else {
        return 1;
      }
    });

    // let t = this.pendingRecv[0].;
  }

  hasError(data: any, index: number): boolean {
    return data.errorColumns.indexOf(index) >= 0;
  }

  display(data: any, index: number, field: any): string {
    return !this.hasError(data, index) ? field : '<h3>error</h3>';
  }

  public dropped(event: UploadEvent) {
    this.files = event.files;
    for (const file of event.files) {
      file.fileEntry.file(info => {
        if (info.name.indexOf('.xlsx') >= 0 || info.name.indexOf('.xls') >= 0) {
          this.onExcelLoad(info);
        }
      });
    }
  }
}
