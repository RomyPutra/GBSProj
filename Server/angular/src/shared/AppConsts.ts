export class AppConsts {

    static remoteServiceBaseUrl: string;
    static appBaseUrl: string;

    static readonly userManagement = {
        defaultAdminUserName: 'admin'
    };

    static readonly localization = {
        defaultLocalizationSourceName: 'Plexform'
    };

    static readonly authorization = {
        encrptedAuthTokenName: 'enc_auth_token',
        userIDName: 'user_id',
        parentIDName: 'parent_id'
    };

    static readonly otherSetting = {
        state: 'key_state',
        appid: 'key_appid',
        accesscode: 'key_accesscode',
    };
}
