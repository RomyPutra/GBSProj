import { AppConsts } from './../../shared/AppConsts';
import { ActionState } from '@shared/models/enums';
import { Component, Injector, ViewChild, Inject } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { EmployeeDto, PagedResultDtoOfEmployeeDto } from '@shared/models/model-employee';
import { EmployeeServiceProxy } from '@shared/service-proxies/proxy-employee';
import { PagedRequestDto } from 'shared/paged-listing-component-base';
import { PagedListingComponentCustom } from 'shared/layout/paged-listing-component-custom';
import { ModalEmployeeComponent } from './modal-employee/modal-employee.component';
import { DxDataGridComponent, DxDataGridModule, DxSelectBoxModule } from 'devextreme-angular';
import { ActivatedRoute } from '@angular/router';
import { UtilsService } from '@abp/utils/utils.service';

@Component({
    selector: 'employee-list',
    templateUrl: './employee.component.html',
    animations: [appModuleAnimation()]
})
export class EmployeeComponent extends PagedListingComponentCustom<EmployeeDto> {

    @ViewChild('createEditEmployeeModal') createEditEmployeeModal: ModalEmployeeComponent;
    @ViewChild(DxDataGridComponent) dataGrid: DxDataGridComponent;

    isStandAlone = true;
    isAddVisible = true;

    active: boolean = false;

    loadOptions: any;
    dataSource: EmployeeDto[] = [];

    detailData: any;

    private companyID: string;
    private appID: string;
    private accessCode: string;
    private selectedState: string;
    private stats: string = '0';

    constructor(
        injector: Injector,
        private _service: EmployeeServiceProxy,
        private route: ActivatedRoute
    ) {
        super(injector);
        this.isStandAlone = true;
        this.companyID = '';
        this.route.params.subscribe((params) => {
            if (params.companyID) {
                this.companyID = params.companyID;
            } else if (params.bizRegID) {
                this.companyID = params.bizRegID;
                this.isStandAlone = false;
                this.isAddVisible = false;
            } else {
                this.companyID = new UtilsService().getCookieValue(AppConsts.authorization.parentIDName);
            }
        });
        

        if (this.companyID !== '') {
            this.isAddVisible = true;
        } else {
            this.isAddVisible = false;
        }
        this.selectedState = new UtilsService().getCookieValue(AppConsts.otherSetting.state);
        this.loadingMessage = 'Loading...';
    }

    onToolbarPreparing(e) {
        e.toolbarOptions.items.unshift({
            location: 'before',
            template: 'gridTitleTemplate'
        }, {
                location: 'before',
                template: 'gridOtherTemplate'
            });
        e.toolbarOptions.items.push({
            location: 'after',
            template: 'gridMenuTemplate'
        });
    }

    protected refresh(): void {
        this.isBusy = true;
        let flag = '1';
        let designation = '';

        this._service.getAllEmployeeProfile(flag, this.companyID, this.selectedState, designation, this.stats)
            .finally(() => {
                this.isBusy = false;
            })
            .subscribe((result: PagedResultDtoOfEmployeeDto) => {
                this.dataSource = result.items;
            });
    }

    selectionChanged(e) {
        let isExpanded = e.isExpanded;
        e.component.collapseAll(-1);
        if (!isExpanded) {
            e.component.expandRow(e.data);
        }
    }

    closeDetail() {
        this.dataGrid.instance.collapseAll(-1);
    }

    protected delete(data: EmployeeDto): void {
        abp.message.confirm(
            "Delete employee '" + data.nickName + "'?",
            (result: boolean) => {
                if (result) {
                    //   this._service.delete(data)
                    //       .subscribe(() => {
                    //           abp.notify.info("Deleted employee: " + data.employeeCode);
                    //           this.refresh();
                    //       });
                }
            }
        );
    }

    // Show Modals
    create(): void {
        this.createEditEmployeeModal.show(ActionState.Create, this.companyID);
    }

    edit(data: EmployeeDto): void {
        this.createEditEmployeeModal.show(ActionState.Edit, this.companyID, data);
    }
}
