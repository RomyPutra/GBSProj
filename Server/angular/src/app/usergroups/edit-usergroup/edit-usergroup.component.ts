import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { UserGroupServiceProxy } from '@shared/service-proxies/service-proxies';
import { UserGroupDto, PagedResultDtoOfUserGroupDto } from '@shared/models/model-usergroup';
import { RoleDto } from '@shared/models/model-role';
import { AppComponentBase } from '@shared/app-component-base';
import { templateJitUrl } from '@angular/compiler';
// import { read } from 'fs';

@Component({
    selector: 'edit-usergroup-modal',
    templateUrl: './edit-usergroup.component.html'
})
export class EditUserGroupComponent extends AppComponentBase {

    @ViewChild('editUserGroupModal') modal: ModalDirective;
    @ViewChild('modalContent') modalContent: ElementRef;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active: boolean = false;
    saving: boolean = false;
    appid: number;
    status='';

	// groups: UserGroupDto[] = [];
    groups: UserGroupDto = null;
    roles: RoleDto[] = null;

    constructor(
        injector: Injector,
        private _groupService: UserGroupServiceProxy
    ) {
        super(injector);
    }

    changeLabel() {
        this.groups.status = this.groups.status === 1 ? 0 : 1;
        if (this.groups.status === 1){
            this.status = 'Actived';
        } else {
            this.status = 'InActived';
        }
    }

    // userInRole(role: RoleDto, user: UserGroupDto): string {
    //     if (user.roleNames.indexOf(role.normalizedName) !== -1) {
    //         return 'checked';
    //     }
    //     else {
    //         return '';
    //     }
    // }

    // show(): void {
    show(id: string): void {
        this._groupService.get(id)
        .subscribe((result) => {
                this.groups = result;
                this.appid = result.appid;
                if (result.status === 0) {
                    this.status = 'InActived';
                } else {
                    this.status = 'Actived';
                }
                this.active = true;
                this.modal.show();
            });
    }

    onShown(): void {
        $.AdminBSB.input.activate($(this.modalContent.nativeElement));
    }

    save(): void {
        // var roles = [];
        // $(this.modalContent.nativeElement).find('[name=role]').each(function (ind: number, elem: Element) {
        //     if ($(elem).is(':checked')) {
        //         roles.push(elem.getAttribute('value').valueOf());
        //     }
        // });

        // this.user.roleNames = roles;

        this.groups.appid = this.appid
        this.saving = true;
        this._groupService.update(this.groups)
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
// import { Component, OnInit } from '@angular/core';

// @Component({
//   selector: 'app-edit-usergroup.component.ts',
//   templateUrl: './edit-usergroup.component.ts.component.html'
// })
// export class EditUsergroup.component.tsComponent implements OnInit {

//   constructor() { }

//   ngOnInit() {
//   }

// }
