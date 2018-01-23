import { Component, Injector, ViewChild } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { UserApprovalDto, PagedResultDtoOfUserApprovalDto } from '@shared/models/model-userapproval';
import { UserApprovalServiceProxy } from '@shared/service-proxies/service-proxies';
import { PagedRequestDto } from 'shared/paged-listing-component-base';
import { PagedListingComponentCustom } from 'shared/layout/paged-listing-component-custom';
import { ApprovalStatusComponent } from '../userapproval/approval-status/approval-status.component';


@Component({
  templateUrl: './userapproval.component.html',
    animations: [appModuleAnimation()]
})

export class UserApprovalComponent extends PagedListingComponentCustom<UserApprovalDto> {

    @ViewChild('approvalStatusModal') approvalStatusModal: ApprovalStatusComponent;

    active: boolean = false;
    userappoval: UserApprovalDto[] = [];

    constructor(
        injector: Injector,
        private _userService: UserApprovalServiceProxy
  ) {
      super(injector);
  }

  protected refresh(): void {
      this._userService.getAll(0, 0)
          .finally(() => {
            this.isTableLoading = false;
          })
          .subscribe((result: PagedResultDtoOfUserApprovalDto) => {
              this.userappoval = result.items;
            //   this.showPaging(result, pageNumber);
          });
  }

  StatusStyle(sts: string) {

    var style = { 'color': 'black', 'font-weight': 'bold'};

    if (sts == "Rejected" ){
        style = { 'color': 'red', 'font-weight': 'bold'};
    } else if (sts == "Approved" ) {
        style = { 'color': 'green', 'font-weight': 'bold'};
    } else {
        style = { 'color': 'black', 'font-weight': 'bold'};
    }

    return style;
}

protected ChangeStatus(appoval: UserApprovalDto): void {
    this.approvalStatusModal.show(appoval.userID);
}


  protected delete(userappoval: UserApprovalDto): void {
      abp.message.confirm(
          "Delete unit '" + userappoval.userID + "'?",
          (result: boolean) => {
            //   if (result) {
            //       this._userService.delete(user.id)
            //           .subscribe(() => {
            //               abp.notify.info("Deleted User: " + user.fullName);
            //               this.refresh();
            //           });
            //   }
          }
      );
  }

//   protected approve(userappoval: UserApprovalDto): void {
//     status = "1";
//     abp.message.confirm(
//         "Approve user '" + userappoval.userID + "'?",
//         (result: boolean) => {
//             if (result) {
//               this._userService.Approval(userappoval.userID, status)
//                      .subscribe(() => {
//                          abp.notify.info("Approved user: " + userappoval.userID);
//                          this.refresh();
//                      });
//             }
//         }
//     );
// }

// protected reject(userappoval: UserApprovalDto): void {
//     status = "2";
//     abp.message.confirm(
//         "Reject user '" + userappoval.userID + "'?",
//         (result: boolean) => {
//             if (result) {
//                 this._userService.Approval(userappoval.userID, status)
//                 .subscribe(() => {
//                     abp.notify.info("Rejected User: " + userappoval.userID);
//                     this.refresh();
//                 });
//             }
//         }
//     );
// }





  // Show Modals
  createUserApproval(): void {
    //   this.createUserModal.show();
  }

  editUserApproval(UserAppoval: UserApprovalDto): void {
    //   this.approvalStatusModal.show(UserAppoval.userID);
  }

}
