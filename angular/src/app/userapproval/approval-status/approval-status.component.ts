import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { UserApprovalDto } from '@shared/models/model-userapproval';
import { AppComponentBase } from '@shared/app-component-base';
import { UserApprovalServiceProxy } from '@shared/service-proxies/service-proxies';


@Component({
    selector: 'approval-status-modal',
    templateUrl: 'approval-status.component.html'
})
export class ApprovalStatusComponent extends AppComponentBase {//implements OnInit {
    @ViewChild('approvalStatusModal') modal: ModalDirective;
    @ViewChild('modalContent') modalContent: ElementRef;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active: boolean = false;
    saving: boolean = false;

    Approvals: UserApprovalDto = null;
    isChecked1 : boolean;
    isChecked2 : boolean;
    isChecked3: boolean;
    selectedState : number;
  
    constructor(
        injector: Injector,
        private _approvalService: UserApprovalServiceProxy
    ) {
        super(injector);
    }

    show(userID: string): void {
        this._approvalService.get(userID)
            .subscribe(
            (result) => {
                this.Approvals = result;
                this.active = true;
                this.modal.show();
                this.changeCheckbox(result.statusDesc)
            }
        );
    }

    onShown(): void {
        $.AdminBSB.input.activate($(this.modalContent.nativeElement));
    }
 
    changeCheckbox(field: string) {        
        if (field === 'Pending') {
            if (this.isChecked1 = true) { 
            this.Approvals.status = "0";
            this.Approvals.rejectRemark = "";
            this.isChecked2 = false;
            this.isChecked3 = false;
            this.selectedState = 0; }
        } else if (field === 'Approved') {
            if (this.isChecked2 = true) { 
            this.Approvals.status = "1";
            this.Approvals.rejectRemark = "";
            this.isChecked1 = false;
            this.isChecked3 = false;
            this.selectedState = 0; }
      } else if (field === "Rejected") {
           if (this.isChecked3 = true) { 
            this.Approvals.status = '2';
            this.isChecked1 = false;
            this.isChecked2 = false;
            this.selectedState = 1;
           }
      }
    }

    save(): void {
    
        this.saving = true;
        this._approvalService.update(this.Approvals)
            .finally(() => { this.saving = false; })
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
