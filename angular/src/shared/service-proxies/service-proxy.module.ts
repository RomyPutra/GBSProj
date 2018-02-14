import { NgModule } from '@angular/core';

import * as ApiServiceProxies from './service-proxies';
import { CurrencyServiceProxy } from './proxy-currency';
import { EmployeeServiceProxy } from './proxy-employee';
import { CustomerServiceProxy } from './proxy-customer';
import { LocationServiceProxy } from './proxy-location';
import { VehicleServiceProxy } from './proxy-vehicle';
import { UserProfileServiceProxy } from './proxy-userprofile';
import { IndustryServiceProxy, SubIndustryServiceProxy } from './proxy-industry';
import { CodeMasterServiceProxy } from './proxy-codemaster';
import { PBTServiceProxy } from './proxy-pbt';
import { CountryServiceProxy } from './proxy-country';
import { StateServiceProxy } from './proxy-state';
import { AreaServiceProxy } from './proxy-area';
import { CityServiceProxy } from './proxy-city';
import { RegistrationServiceProxy } from './proxy-registration';
import { PermissionServiceProxy } from './proxy-permission';
import { UploadServiceProxy } from './proxy-upload';
import { GetAgentAccessFareServiceProxy } from './proxy-accessfare';
import { GetAgentTierServiceProxy } from './proxy-agenttier';
import { GetFlightTimeServiceProxy } from './proxy-flighttime';
import { GetBookingServiceProxy } from './proxy-payscheme';
import { GetDiscountServiceProxy } from './proxy-discount';
import { GetCapacityServiceProxy } from './proxy-capacity';
import { GetDiscWeightageServiceProxy } from './proxy-discweightage';
import { GetFloorFareServiceProxy } from './proxy-floorfare';
import { GetSeasonalityServiceProxy } from './proxy-seasonality';
import { GetRestrictionServiceProxy } from './proxy-restriction';
import { GetLFDiscServiceProxy } from './proxy-lfdisc';

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
        ApiServiceProxies.PermissionSetServiceProxy,
        ApiServiceProxies.ConsignmentServiceProxy,
        ApiServiceProxies.UserApprovalServiceProxy,
        ApiServiceProxies.CategoryServiceProxy,
        ApiServiceProxies.StockServiceProxy,
        CurrencyServiceProxy,
        EmployeeServiceProxy,
        CustomerServiceProxy,
        LocationServiceProxy,
        VehicleServiceProxy,
        UserProfileServiceProxy,
        IndustryServiceProxy,
        SubIndustryServiceProxy,
        CodeMasterServiceProxy,
        PBTServiceProxy,
        CountryServiceProxy,
        StateServiceProxy,
        AreaServiceProxy,
        CityServiceProxy,
        RegistrationServiceProxy,
        PermissionServiceProxy,
        UploadServiceProxy,
        GetBookingServiceProxy,
        GetFlightTimeServiceProxy,
        GetAgentTierServiceProxy,
        GetAgentAccessFareServiceProxy,
        GetDiscountServiceProxy,
        GetCapacityServiceProxy,
        GetDiscWeightageServiceProxy,
        GetFloorFareServiceProxy,
        GetSeasonalityServiceProxy,
        GetRestrictionServiceProxy,
        GetLFDiscServiceProxy
    ]
})
export class ServiceProxyModule { }
