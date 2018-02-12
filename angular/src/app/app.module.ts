﻿import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule, JsonpModule } from '@angular/http';
import { ModalModule } from 'ngx-bootstrap';
import { NgxPaginationModule } from 'ngx-pagination';
import { DxButtonModule, DxDataGridModule, DxFileUploaderModule } from 'devextreme-angular';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AbpModule } from '@abp/abp.module';
import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { SharedModule } from '@shared/shared.module';
import { FileDropModule } from 'ngx-file-drop';
import { PlexLoadingComponent } from '@shared/layout/custom/plex-loading.component';
import { MaterialInput } from '@shared/directives/material-input.directive';
import { MatButtonModule, MatCheckboxModule, MatInputModule, MatStepperModule, MatAutocompleteModule,
         MatSelectModule, MatProgressSpinnerModule, MatDatepickerModule, MatNativeDateModule } from '@angular/material';
import { UploadComponent } from './upload/upload.component';
import { ClickOutsideModule } from 'ng-click-outside';
// import { TopBarComponent } from '@app/layout/topbar.component';
// import { SideBarUserAreaComponent } from '@app/layout/sidebar-user-area.component';
// import { SideBarNavComponent } from '@app/layout/sidebar-nav.component';
// import { SideBarFooterComponent } from '@app/layout/sidebar-footer.component';
// import { UnitsComponent } from './units/units.component';
// import { UsergroupsComponent } from './usergroups/usergroups.component';
// import { CreateUserGroupComponent } from './usergroups/create-usergroup/create-usergroup.component';
// import { EditUserGroupComponent } from './usergroups/edit-usergroup/edit-usergroup.component';
// import { UserprofilesComponent } from './userprofiles/userprofile.component';
// import { PermissionsetComponent } from './permissionset/permissionset.component';
// import { ModalPermissionsetComponent } from './permissionset/modal-permissionset/modal-permissionset.component';
// import { ConsignmentComponent } from './consignment/consignment.component';
// import { CreateUserprofileComponent } from './userprofiles/create-userprofile/create-userprofile.component';
// import { UserApprovalComponent } from './userapproval/userapproval.component';
// import { ApprovalStatusComponent } from './userapproval/approval-status/approval-status.component';
// import { StateComponent } from './state/state.component';
// import { CreateStateComponent } from './state/create-state/create-state.component';
// import { EditStateComponent } from './state/edit-state/edit-state.component';
// import { CustomersComponent } from './customers/customers.component';
// import { CreateCustomerComponent } from './customers/create-customer/create-customer.component';
// import { EditCustomerComponent } from './customers/edit-customer/edit-customer.component';
// import { CountryComponent } from './country/country.component';
// import { EditCountryComponent } from './country/edit-country/edit-country.component';
// import { CreateCountryComponent } from './country/create-country/create-country.component';
// import { StockComponent } from './stock/stock.component';
// import { CreateStockComponent } from './stock/create-stock/create-stock.component';
// import { CurrencyComponent } from './currency/currency.component';
// import { ModalCurrencyComponent } from './currency/modal-currency/modal-currency.component';
// import { EmployeeComponent } from './employee/employee.component';
// import { ModalEmployeeComponent } from './employee/modal-employee/modal-employee.component';
// import { VoucherCreateEditComponent } from './AEON/voucher-create-edit/voucher-create-edit.component';
import { TopBarComponent } from '@app/layout/topbarg.component';
import { TopBarLanguageSwitchComponent } from '@app/layout/topbar-languageswitch.component';
import { SideBarUserAreaComponent } from '@app/layout/sidebar-user-areag.component';
import { SideBarNavComponent } from '@app/layout/sidebar-navg.component';
import { SideBarFooterComponent } from '@app/layout/sidebar-footerg.component';
import { RightSideBarComponent } from '@app/layout/right-sidebar.component';
import { GBSComponent } from './gbs/gbs.component';
// import { EditPaySchemeComponent } from './gbs/update-payscheme/update-payscheme.component';
import { FlighttimeComponent } from './flighttime/flighttime.component';
import { AgenttierComponent } from './agenttier/agenttier.component';
import { AgentAccessFareComponent } from './accessfare/accessfare.component';
import { CapacityComponent } from './capacity/capacity.component';
import { DiscountComponent } from './discount/discount.component';
import { SeasonalityComponent } from './seasonality/seasonality.component';
import { DiscweightageComponent } from './discweightage/discweightage.component';
import { FloorfareComponent } from './floorfare/floorfare.component';
import { RestrictionComponent } from './restriction/restriction.component';

@NgModule({
    declarations: [
        AppComponent,
        TopBarComponent,
        TopBarLanguageSwitchComponent,
        SideBarUserAreaComponent,
        SideBarNavComponent,
        SideBarFooterComponent,
        RightSideBarComponent,
        // HomeComponent,
        // AboutComponent,
        // TenantsComponent,
        // CreateTenantComponent,
        // EditTenantComponent,
        // UsersComponent,
        // CreateUserComponent,
        // EditUserComponent,
        // RolesComponent,
        // CreateRoleComponent,
        // EditRoleComponent,
        // UnitsComponent,
        // UsergroupsComponent,
        // CreateUserGroupComponent,
        // EditUserGroupComponent,
        // UserprofilesComponent,
        // PermissionsetComponent,
        // ModalPermissionsetComponent,
        // ConsignmentComponent,
        // CreateUserprofileComponent,
        // UserApprovalComponent,
        // ApprovalStatusComponent,
        // StateComponent,
        // CreateStateComponent,
        // EditStateComponent,
        // CustomersComponent,
        // CreateCustomerComponent,
        // EditCustomerComponent,
        // CountryComponent,
        // EditCountryComponent,
        // CreateCountryComponent,
        // StockComponent,
        // CreateStockComponent,
        // CurrencyComponent,
        // ModalCurrencyComponent,
        // EmployeeComponent,
        // ModalEmployeeComponent,
        // PlexLoadingComponent,
        // UploadComponent,
        // VoucherCreateEditComponent,
        GBSComponent,
        // EditPaySchemeComponent
        FlighttimeComponent,
        AgenttierComponent,
        AgentAccessFareComponent,
        CapacityComponent,
        DiscountComponent,
        SeasonalityComponent,
        DiscweightageComponent,
        FloorfareComponent,
        RestrictionComponent
],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        HttpModule,
        JsonpModule,
        ModalModule.forRoot(),
        AbpModule,
        AppRoutingModule,
        ServiceProxyModule,
        SharedModule,
        NgxPaginationModule,
        DxButtonModule,
        DxDataGridModule,
        DxFileUploaderModule,
        MatButtonModule,
        MatCheckboxModule,
        MatInputModule,
        MatStepperModule,
        MatAutocompleteModule,
        MatSelectModule,
        MatProgressSpinnerModule,
        FileDropModule,
        ClickOutsideModule,
        MatDatepickerModule,
        MatNativeDateModule
    ],
    providers: [

    ]
})
export class AppModule { }
