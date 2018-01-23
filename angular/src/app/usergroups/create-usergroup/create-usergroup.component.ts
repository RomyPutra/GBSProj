import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { UserGroupServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateGroupDto } from '@shared/models/model-usergroup';
import { RoleDto } from '@shared/models/model-role';
import { AppComponentBase } from '@shared/app-component-base';
// import * as _ from "lodash";

@Component({
    selector: 'create-usergroup-modal',
    templateUrl: './create-usergroup.component.html'
})
export class CreateUserGroupComponent extends AppComponentBase {

    @ViewChild('createUserGroupModal') modal: ModalDirective;
    @ViewChild('modalContent') modalContent: ElementRef;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active: boolean = false;
    saving: boolean = false;
    groups: CreateGroupDto = null;
    roles: RoleDto[] = null;

    constructor(
        injector: Injector,
        private _groupService: UserGroupServiceProxy,
    ) {
        super(injector);
    }

    // ngOnInit(): void {
    //     this._groupService.getRoles()
    //     .subscribe((result) => {
    //         this.roles = result.items;
    //     });
    // }

    show(): void {
        this.active = true;
        this.modal.show();
        this.groups = new CreateGroupDto();
        this.groups.init({ isActive: true });
    }

    onShown(): void {
        $.AdminBSB.input.activate($(this.modalContent.nativeElement));
    }

    save(): void {
        //TODO: Refactor this, don't use jQuery style code
        var roles = [];
        $(this.modalContent.nativeElement).find("[name=role]").each((ind:number, elem:Element) => {
            if($(elem).is(":checked") == true){
                roles.push(elem.getAttribute("value").valueOf());
            }
        });

        // this.user.roleNames = roles;
        this.saving = true;
        this._groupService.create(this.groups)
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
// import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, OnInit } from '@angular/core';
// import { ModalDirective } from 'ngx-bootstrap';
// import { RoleServiceProxy } from '@shared/service-proxies/service-proxies';
// import { CreateRoleDto } from '@shared/models/model-role';
// import { ListResultDtoOfPermissionDto } from '@shared/models/model-permission';
// import { AppComponentBase } from '@shared/app-component-base';

// @Component({
//     selector: 'create-usergroup-modal',
//     templateUrl: './create-usergroup.component.html'
// })
// export class CreateUserGroupComponent extends AppComponentBase implements OnInit {
//     @ViewChild('createUserGroupModal') modal: ModalDirective;
//     @ViewChild('modalContent') modalContent: ElementRef;

//     active: boolean = false;
//     saving: boolean = false;

//     permissions: ListResultDtoOfPermissionDto = null;
//     role: CreateRoleDto = null;

//     @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
//     constructor(
//         injector: Injector,
//         private _roleService: RoleServiceProxy
//     ) {
//         super(injector);
//     }

//     ngOnInit(): void {
//         this._roleService.getAllPermissions()
//             .subscribe((permissions: ListResultDtoOfPermissionDto) => {
//                 this.permissions = permissions;
//             });
//     }

//     show(): void {
//         this.active = true;
//         this.role = new CreateRoleDto();
//         this.role.init({ isStatic: false });

//         this.modal.show();
//     }

//     onShown(): void {
//         $.AdminBSB.input.activate($(this.modalContent.nativeElement));
//     }

//     save(): void {
//         var permissions = [];
//         $(this.modalContent.nativeElement).find("[name=permission]").each(
//             (index: number, elem: Element) => {
//                 if ($(elem).is(":checked")) {
//                     permissions.push(elem.getAttribute("value").valueOf());
//                 }
//             }
//         );

//         this.role.permissions = permissions;

//         this.saving = true;
//         this._roleService.create(this.role)
//             .finally(() => { this.saving = false; })
//             .subscribe(() => {
//                 this.notify.info(this.l('SavedSuccessfully'));
//                 this.close();
//                 this.modalSave.emit(null);
//             });
//     }

//     close(): void {
//         this.active = false;
//         this.modal.hide();
//     }
// }
