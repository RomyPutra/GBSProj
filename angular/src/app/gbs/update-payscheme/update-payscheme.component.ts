import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetBookingServiceProxy } from '@shared/service-proxies/proxy-payscheme';
import { GBSDto, PagedResultDtoOfGBSDto } from '@shared/models/model-gbs';
import { RoleDto } from '@shared/models/model-role';
import { AppComponentBase } from '@shared/app-component-base';
import { templateJitUrl } from '@angular/compiler';
import { PagedListingComponentBase, PagedRequestDto } from 'shared/paged-listing-component-base';

@Component({
    // selector: 'update-payscheme-modal',
    templateUrl: './update-payscheme.component.html'
})
export class EditPaySchemeComponent extends AppComponentBase {

    @ViewChild('updatePayschemeModal') modal: ModalDirective;
    @ViewChild('modalContent') modalContent: ElementRef;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active: boolean = false;
    saving: boolean = false;
//     appid: number;
//     status="";

    groups: GBSDto = null;
//     roles: RoleDto[] = null;

    constructor(
        injector: Injector,
        private _groupService: GetBookingServiceProxy
    ) {
        super(injector);
    }

//     changeLabel() {
//         this.groups.status = this.groups.status === 1 ? 0 : 1;
//         if (this.groups.status == 1){
//             this.status = "Actived";
//         } else {
//             this.status = "InActived";
//         }
//     }

    // show(id: string): void {
    show(request: number, pageNumber: number): void {
        this._groupService.getbookingbypnr(request, pageNumber)
        //.get(id)
		.subscribe((result: PagedResultDtoOfGBSDto) => {
				if (result.items.length > 0) {
					this.groups = result.items[0];
				}
        // .subscribe((result) => {
        //         this.groups = result;
                this.active = true;
                this.modal.show();
            });
    }

    onShown(): void {
        $.AdminBSB.input.activate($(this.modalContent.nativeElement));
    }

//     save(): void {

//         this.groups.appID = this.appid
//         this.saving = true;
//         this._groupService.update(this.groups)
//             .finally(() => { this.saving = false; })
//             .subscribe(() => {
//                 this.notify.info(this.l('SavedSuccessfully'));
//                 this.close();
//                 this.modalSave.emit(null);
//             });
//     }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
