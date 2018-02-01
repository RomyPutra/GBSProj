import { AppConsts } from './../../shared/AppConsts';
import { ActionState } from '@shared/models/enums';
import { Component, Injector, ViewChild, Inject } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { UserProfileDto, PagedResultDtoOfUserProfileDto } from '@shared/models/model-userprofile';
import { UserProfileServiceProxy } from '@shared/service-proxies/proxy-userprofile';
import { PagedRequestDto } from 'shared/paged-listing-component-base';
import { PagedListingComponentCustom } from 'shared/layout/paged-listing-component-custom';
import { CreateUserprofileComponent } from './create-userprofile/create-userprofile.component';
import CustomStore from 'devextreme/data/custom_store';
import { DxDataGridComponent, DxDataGridModule, DxSelectBoxModule } from 'devextreme-angular';

@Component({
    templateUrl: './userprofile.component.html',
    animations: [appModuleAnimation()]
})
export class UserprofilesComponent extends PagedListingComponentCustom<UserProfileDto> {

    @ViewChild('createUserprofileModal') createUserprofileModal: CreateUserprofileComponent;
    @ViewChild(DxDataGridComponent) dataGrid: DxDataGridComponent;

    active: boolean = false;

    loadOptions: any;
    dataSource: any = {};
    filterText = '';
    _timeoutFilter: any;
    isFiltering = false;

    constructor(
        injector: Injector,
        private _userService: UserProfileServiceProxy
    ) {
        super(injector);
    }

    onFilterChange() {
        if (this._timeoutFilter) { // if there is already a timeout in process cancel it
            window.clearInterval(this._timeoutFilter);
        }
        let that = this;
        this._timeoutFilter = window.setInterval(function() {
            if (that.filterText.length > 0 || that.isFiltering) {
                if (!that.isFiltering) {
                    that.isFiltering = true;
                } else if (that.filterText.length > 0) {
                    that.isFiltering = false;
                }
                window.clearInterval(that._timeoutFilter);
                that.dataGrid.instance.refresh();
            }
        }, 1000);
     }

    protected refresh(): void {
        this.isTableLoading = true;
        if (this.dataSource.store) {
            this.dataGrid.instance.refresh();
        } else {
            let that = this;
            this.dataSource.store = new CustomStore({
                load: function (loadOptions: any) {
                    that.loadOptions = loadOptions;
                    let filterText = '';

                    if (that.isValidFilter(that.filterText, that.filterValidLenght)) {
                        filterText = that.filterText;
                    }

                    if (loadOptions.filter) {
                        if (loadOptions.filter.length > 0) {
                            filterText = loadOptions.filter[0][2];
                        }
                    }

                    let orderby = '';
                    if (loadOptions.sort) {
                        orderby += loadOptions.sort[0].selector;
                        if (loadOptions.sort[0].desc) {
                            orderby += ' desc';
                        }
                    } else {
                        throw new Error("One of data column must have 'sortOrder'");
                    }

                    let d = $.Deferred();

                    that._userService.getAll(loadOptions.skip, loadOptions.take, orderby, filterText)
                    .finally(() => {
                        that.isTableLoading = false;
                    })
                    .subscribe((result: PagedResultDtoOfUserProfileDto) => {
                        d.resolve(result.items, { totalCount: result.totalCount });
                    });

                    return d.promise();

                }
            });
        }
    }
    
    protected delete(profile: UserProfileDto): void {
        abp.message.confirm(
            "Delete profile '" + profile.userID + "'?",
            (result: boolean) => {
                if (result) {
                  this._userService.delete(profile.userID)
                      .subscribe(() => {
                          abp.notify.info("Deleted User: " + profile.userName);
                          this.refresh();
                      });
                }
            }
        );
    }

    // Show Modals
    createUserProfile(): void {
        this.createUserprofileModal.show(ActionState.Create);
    }

    edit(user: UserProfileDto): void {
        this.createUserprofileModal.show(ActionState.Edit, user);
    }
}
