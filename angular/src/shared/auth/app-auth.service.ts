import { Injectable } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { UtilsService } from '@abp/utils/utils.service';

@Injectable()
export class AppAuthService {

    logout(reload?: boolean): void {
        abp.auth.clearToken();
        var _utilsService: UtilsService = new UtilsService();
        _utilsService.deleteCookie(AppConsts.authorization.userIDName);
        if (reload !== false) {
            location.href = AppConsts.appBaseUrl;
        }
    }
}