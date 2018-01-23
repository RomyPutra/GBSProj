import { AppComponentBase } from "shared/app-component-base";
import { Component, Injector, ViewChild, OnInit } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PermissionSetDto, PagedResultDtoOfPermissionSetDto, IPermissionFunctionDto } from '@shared/models/model-permissionset';
import { PermissionSetServiceProxy } from '@shared/service-proxies/service-proxies';
import { PagedListingComponentBase, PagedRequestDto } from 'shared/paged-listing-component-base';
import { UserGroupDto, PagedResultDtoOfUserGroupDto } from '@shared/models/model-usergroup';
import { UserGroupServiceProxy } from '@shared/service-proxies/service-proxies';
@Component({
  templateUrl: './permissionset.component.html',
    animations: [appModuleAnimation()]
})
// export class UnitsComponent implements OnInit {
export class PermissionsetComponent extends AppComponentBase implements OnInit  {
  active: boolean = false;
  datas: PermissionSetDto[] = [];
  changedDataIndex: number[][] = [];
  userGroups: UserGroupDto[] = [];
  public isTableLoading = false;
  selectedAccessCode = '';

  constructor(
      injector: Injector,
      private _service: PermissionSetServiceProxy,
      private _serviceUserGroup: UserGroupServiceProxy
  ) {
      super(injector);
  }

  ngOnInit(): void {
    this.refresh();
  }

  refresh(): void {
      this.getUserGroup();
  }

  public getUserGroup(): void {
      this.isTableLoading = true;
      this.populateUserGroup(() => {
          this.isTableLoading = false;
          if (this.selectedAccessCode !== '') {
            this.getPermissions(this.selectedAccessCode);
          }
      });
  }

  private populateUserGroup(finishedCallback: Function): void {
      this._serviceUserGroup.getAll(0, 0)
          .finally(() => {
            finishedCallback();
          })
          .subscribe((result: PagedResultDtoOfUserGroupDto) => {
            this.userGroups = result.items;
          });
  }

  public getPermissions(accessCode: string): void {
    this.isTableLoading = true;
    this.populatePermissions(accessCode, () => {
        this.isTableLoading = false;
    });
}
  private populatePermissions(accessCode: string, finishedCallback: Function) {
    this._service.getAll(accessCode)
    .finally(() => {
        finishedCallback();
    })
    .subscribe((result: PagedResultDtoOfPermissionSetDto) => {
        // result.items[0].expanded = true;
        this.datas = result.items;
    });
  }

  userInRole(data: IPermissionFunctionDto): string {
    if (data.allowEdit) {
        return "checked";
    } else {
        return "";
    }
  }

  changeSelect(){
    this.getPermissions(this.selectedAccessCode);
  }

  changeCheckbox(i, y, field: string) {
    let found = false;
    for (let x = 0; x < this.changedDataIndex.length; x++) {
      if (this.changedDataIndex[x].length >= 2) {
        if (i === this.changedDataIndex[x][0] && y === this.changedDataIndex[x][1]) {
          found = true;
          break;
        }
      }
    }
    if (!found) {
      this.changedDataIndex.push([i, y]);
    }

    if (field === 'allowNew') {
      this.datas[i].functions[y].allowNew = this.datas[i].functions[y].allowNew === 1 ? 0 : 1;
    } else if (field === 'allowEdit') {
      this.datas[i].functions[y].allowEdit = this.datas[i].functions[y].allowEdit === 1 ? 0 : 1;
    } else if (field === 'allowDel') {
      this.datas[i].functions[y].allowDel = this.datas[i].functions[y].allowDel === 1 ? 0 : 1;
    } else if (field === 'allowPrt') {
      this.datas[i].functions[y].allowPrt = this.datas[i].functions[y].allowPrt === 1 ? 0 : 1;
    } else if (field === 'allowPro') {
      this.datas[i].functions[y].allowPro = this.datas[i].functions[y].allowPro === 1 ? 0 : 1;
    } else if (field === 'isDeny') {
      this.datas[i].functions[y].isDeny = this.datas[i].functions[y].isDeny === 1 ? 0 : 1;
    }
  }

  protected expandSelected(data: PermissionSetDto): void {
    this.datas.forEach(function(val) {
      val.expanded = false;
    });
    data.expanded = true;
  }

  protected delete(unit: PermissionSetDto): void {
      abp.message.confirm(
          "Delete unit '" + unit.moduleID + "'?",
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

  //createUnit Show Modals
  save(): void {
    // this.saving = true;
    let toSave: IPermissionFunctionDto[] = [];
    for (let i = 0; i < this.changedDataIndex.length; i++) {
      let idx = this.changedDataIndex[i];
      toSave.push(this.datas[idx[0]].functions[idx[1]]);
    }
    if (toSave.length > 0) {
      this._service.update(toSave)
          .finally(() => {
            // this.saving = false;
          })
          .subscribe((result: boolean) => {
              if (result) {
                this.notify.info(this.l('SavedSuccessfully'));
                this.refresh();
              } else {
                this.notify.error('Save Error!');
              }
          });
    }
  }

  editUnit(user: PermissionSetDto): void {
    //   this.editUserModal.show(user.id);
  }
}
