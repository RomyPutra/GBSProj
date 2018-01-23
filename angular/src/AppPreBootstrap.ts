import * as moment from 'moment';
import { AppConsts } from '@shared/AppConsts';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { Type, CompilerOptions, NgModuleRef } from '@angular/core';
import { UtilsService } from '@abp/utils/utils.service';

export class AppPreBootstrap {
    static run(callback: () => void): void {
        AppPreBootstrap.getApplicationConfig(() => {
            AppPreBootstrap.getUserConfiguration(callback);
        });
    }

    static bootstrap<TM>(moduleType: Type<TM>, compilerOptions?: CompilerOptions | CompilerOptions[]): Promise<NgModuleRef<TM>> {
        return platformBrowserDynamic().bootstrapModule(moduleType, compilerOptions);
    }

    private static getApplicationConfig(callback: () => void) {
        return abp.ajax({
            url: '/assets/appconfig.json',
            method: 'GET',
            headers: {
                'Abp.TenantId': abp.multiTenancy.getTenantIdCookie()
            }
        }).done(result => {
            AppConsts.appBaseUrl = result.appBaseUrl;
            AppConsts.remoteServiceBaseUrl = result.remoteServiceBaseUrl;

            callback();
        });
    }

    private static getCurrentClockProvider(currentProviderName: string): abp.timing.IClockProvider {
        if (currentProviderName === "unspecifiedClockProvider") {
            return abp.timing.unspecifiedClockProvider;
        }

        if (currentProviderName === "utcClockProvider") {
            return abp.timing.utcClockProvider;
        }

        return abp.timing.localClockProvider;
    }

    private static getUserConfiguration(callback: () => void): JQueryPromise<any> {
        return abp.ajax({
            url: AppConsts.remoteServiceBaseUrl + '/AbpUserConfiguration/GetAll',
            method: 'GET',
            headers: {
                Authorization: 'Bearer ' + abp.auth.getToken(),
                '.AspNetCore.Culture': abp.utils.getCookieValue("Abp.Localization.CultureName"),
                'Abp.TenantId': abp.multiTenancy.getTenantIdCookie()
            }
        }).done(result => {
            let isReqAgain = false;
            if (result.hasOwnProperty('auth')) {
                if (result.auth.hasOwnProperty('grantedPermissions')) {
                    isReqAgain = true;
                    var _utilsService: UtilsService = new UtilsService();
                    var userID = _utilsService.getCookieValue(AppConsts.authorization.userIDName);
                    if (userID === undefined || userID === null) {
                        userID = '';
                    }
                    abp.ajax({
                        url: AppConsts.remoteServiceBaseUrl + '/api/eswis/UserConfiguration/GetAllPermission?userID=' + userID,
                        method: 'GET',
                        headers: new Headers({
                            'Content-Type': 'application/json',
                            'Accept': 'application/json',
                        })
                    }).done(res2 => {
                        if (res2.hasOwnProperty('permissions')) {
                            result.auth.grantedPermissions = res2.permissions;
                        }
                        $.extend(true, abp, result);
                        abp.clock.provider = this.getCurrentClockProvider(result.clock.provider);

                        moment.locale(abp.localization.currentLanguage.name);

                        if (abp.clock.provider.supportsMultipleTimezone) {
                            moment.tz.setDefault(abp.timing.timeZoneInfo.iana.timeZoneId);
                        }

                        callback();
                    });
                }
            }
            if (!isReqAgain) {
                $.extend(true, abp, result);
                abp.clock.provider = this.getCurrentClockProvider(result.clock.provider);

                moment.locale(abp.localization.currentLanguage.name);

                if (abp.clock.provider.supportsMultipleTimezone) {
                    moment.tz.setDefault(abp.timing.timeZoneInfo.iana.timeZoneId);
                }

                callback();
            }
        });
    }
}