// import { UnitServiceProxy } from '@shared/service-proxies/service-proxies';
import { NgModule } from '@angular/core';

import * as ApiServiceProxies from './service-proxies';

@NgModule({
    providers: [
        ApiServiceProxies.RoleServiceProxy,
        ApiServiceProxies.SessionServiceProxy,
        ApiServiceProxies.TenantServiceProxy,
        ApiServiceProxies.UserServiceProxy,
        ApiServiceProxies.TokenAuthServiceProxy,
        ApiServiceProxies.AccountServiceProxy,
        ApiServiceProxies.ConfigurationServiceProxy,
        ApiServiceProxies.UnitServiceProxy,
        ApiServiceProxies.UserGroupServiceProxy,
        ApiServiceProxies.UserProfileServiceProxy,
        ApiServiceProxies.PermissionSetServiceProxy,
        ApiServiceProxies.ConsignmentServiceProxy,
        ApiServiceProxies.UserApprovalServiceProxy,
        ApiServiceProxies.StateServiceProxy,
        ApiServiceProxies.CustomerServiceProxy,
        ApiServiceProxies.CountryServiceProxy,
        ApiServiceProxies.GetBookingServiceProxy
    ]
})
export class ServiceProxyModule { }
