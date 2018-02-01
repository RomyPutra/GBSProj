import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
// import { HomeComponent } from './home/home.component';
// import { AboutComponent } from './about/about.component';
// import { UsersComponent } from './users/users.component';
// import { UnitsComponent } from './units/units.component';
// import { TenantsComponent } from './tenants/tenants.component';
// import { RolesComponent } from 'app/roles/roles.component';
// import { UsergroupsComponent } from './usergroups/usergroups.component';
// import { UserprofilesComponent } from './userprofiles/userprofile.component';
// import { PermissionsetComponent } from './permissionset/permissionset.component';
// import { ConsignmentComponent } from './consignment/consignment.component';
// import { UserApprovalComponent } from './userapproval/userapproval.component';
// import { StateComponent } from './state/state.component';
// import { CustomersComponent } from './customers/customers.component';
// import { EditCustomerComponent } from 'app/customers/edit-customer/edit-customer.component';
// import { CountryComponent } from './country/country.component';
// import { EmployeeComponent } from 'app/employee/employee.component';
// import { StockComponent } from './stock/stock.component';
// import { CurrencyComponent } from './currency/currency.component';
// import { UploadComponent } from 'app/upload/upload.component';
// import { VoucherCreateEditComponent } from './AEON/voucher-create-edit/voucher-create-edit.component';
import { GBSComponent } from './gbs/gbs.component';
import { FlighttimeComponent } from './flighttime/flighttime.component';
import { AgentAccessFareComponent } from './accessfare/accessfare.component';
import { AgenttierComponent } from './agenttier/agenttier.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: AppComponent,
                children: [
                    // { path: 'home', component: HomeComponent, canActivate: [AppRouteGuard] },
                    // { path: 'upload', component: UploadComponent, canActivate: [AppRouteGuard] },
                    // { path: 'users', component: UsersComponent, canActivate: [AppRouteGuard] },
                    // { path: 'units', component: UnitsComponent, data: { permission: 'Code_Master#Waste_Unit' },
                    //     canActivate: [AppRouteGuard] },
                    // { path: 'roles', component: RolesComponent, canActivate: [AppRouteGuard] },
                    // { path: 'tenants', component: TenantsComponent, canActivate: [AppRouteGuard] },
                    // { path: 'about', component: AboutComponent },
                    // { path: 'groups', component: UsergroupsComponent, data: { permission: 'Security#User_Group' },
                    //     canActivate: [AppRouteGuard] },
                    // { path: 'profiles',
                    //     component: UserprofilesComponent, canActivate: [AppRouteGuard] },
                    // { path: 'permissionset', component: PermissionsetComponent, canActivate: [AppRouteGuard] },
                    // { path: 'consignment', component: ConsignmentComponent, canActivate: [AppRouteGuard] },
                    // { path: 'userapproval', component: UserApprovalComponent, canActivate: [AppRouteGuard] },
                    // { path: 'customer', component: CustomersComponent, canActivate: [AppRouteGuard] },
                    // { path: 'customer/edit/:bizRegID', component: EditCustomerComponent, canActivate: [AppRouteGuard] },
                    // { path: 'state', component: StateComponent, canActivate: [AppRouteGuard] },
                    // { path: 'country', component: CountryComponent, canActivate: [AppRouteGuard] },
                    // { path: 'employee', component: EmployeeComponent, canActivate: [AppRouteGuard] },
                    // { path: 'employee/:companyID', component: EmployeeComponent, canActivate: [AppRouteGuard] },
                    // { path: 'stock', component: StockComponent, canActivate: [AppRouteGuard] },
                    // { path: 'currency', component: CurrencyComponent, canActivate: [AppRouteGuard] },
                    // { path: 'AEON/voucher/create', component: VoucherCreateEditComponent, canActivate: [AppRouteGuard] },
                    { path: 'gbs', component: GBSComponent, canActivate: [AppRouteGuard] },
                    { path: 'flighttime', component: FlighttimeComponent, canActivate: [AppRouteGuard] },
                    { path: 'accessfare', component: AgentAccessFareComponent, canActivate: [AppRouteGuard] },
                    { path: 'agenttier', component: AgenttierComponent, canActivate: [AppRouteGuard] },
                ]
            }
        ])
    ],
    exports: [RouterModule]
})
export class AppRoutingModule { }
