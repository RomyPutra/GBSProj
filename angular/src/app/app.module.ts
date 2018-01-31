import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule, JsonpModule } from '@angular/http';
import { ModalModule } from 'ngx-bootstrap';
import { NgxPaginationModule } from 'ngx-pagination';
import { DxButtonModule, DxDataGridModule } from 'devextreme-angular';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AbpModule } from '@abp/abp.module';
import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { SharedModule } from '@shared/shared.module';
// import { TopBarComponent } from '@app/layout/topbar.component';
// import { SideBarUserAreaComponent } from '@app/layout/sidebar-user-area.component';
// import { SideBarNavComponent } from '@app/layout/sidebar-nav.component';
// import { SideBarFooterComponent } from '@app/layout/sidebar-footer.component';
// import { HomeComponent } from '@app/home/home.component';
// import { AboutComponent } from '@app/about/about.component';
// import { UsersComponent } from '@app/users/users.component';
// import { CreateUserComponent } from '@app/users/create-user/create-user.component';
// import { EditUserComponent } from './users/edit-user/edit-user.component';
// import { RolesComponent } from '@app/roles/roles.component';
// import { CreateRoleComponent } from '@app/roles/create-role/create-role.component';
// import { EditRoleComponent } from './roles/edit-role/edit-role.component';
// import { TenantsComponent } from '@app/tenants/tenants.component';
// import { CreateTenantComponent } from './tenants/create-tenant/create-tenant.component';
// import { EditTenantComponent } from './tenants/edit-tenant/edit-tenant.component';
// import { UnitsComponent } from './units/units.component';
// import { UsergroupsComponent } from './usergroups/usergroups.component';
// import { CreateUserGroupComponent } from './usergroups/create-usergroup/create-usergroup.component';
// import { EditUserGroupComponent } from './usergroups/edit-usergroup/edit-usergroup.component';
// import { UserprofilesComponent } from './userprofiles/userprofile.component';
// import { PermissionsetComponent } from './permissionset/permissionset.component';
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
import { TopBarComponent } from '@app/layout/topbarg.component';
import { TopBarLanguageSwitchComponent } from '@app/layout/topbar-languageswitch.component';
import { SideBarUserAreaComponent } from '@app/layout/sidebar-user-areag.component';
import { SideBarNavComponent } from '@app/layout/sidebar-navg.component';
import { SideBarFooterComponent } from '@app/layout/sidebar-footerg.component';
import { RightSideBarComponent } from '@app/layout/right-sidebar.component';
import { MaterialInput } from '@shared/directives/material-input.directive';
import { GBSComponent } from './gbs/gbs.component';
// import { EditPaySchemeComponent } from './gbs/update-payscheme/update-payscheme.component';
import { FlighttimeComponent } from './flighttime/flighttime.component';

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
        GBSComponent,
        // EditPaySchemeComponent
        FlighttimeComponent
],
    imports: [
        CommonModule,
        FormsModule,
        HttpModule,
        JsonpModule,
        ModalModule.forRoot(),
        AbpModule,
        AppRoutingModule,
        ServiceProxyModule,
        SharedModule,
        NgxPaginationModule,
        DxButtonModule,
        DxDataGridModule
    ],
    providers: [

    ]
})
export class AppModule { }
