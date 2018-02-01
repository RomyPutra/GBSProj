import { Observable } from 'rxjs/Observable';

export class UserProfileDto implements IUserProfileDto {
    userID: string;
    userName: string;
    password: string;
    refID: string;
    refType: string;
    parentID: string;
    status: string;
    logged: string;
    logStation: string;
    lastLogin: string;
    lastLogout: string;
    rowguid: string;
    createDate: string;
    lastUpdate: string;
    createBy: string;
    updateBy: string;
    syncCreate: string;
    syncLastUpd: string;
    groupCode: string;
    appId: string;
    kkm: string;
    permissionAPI: string;
    companyType: string;
    statusDesc: string;
    accessCode: string;
    accessDesc: string;

    static fromJS(data: any): UserProfileDto {
        let result = new UserProfileDto();
        result.init(data);
        return result;
    }

    constructor(data?: IUserProfileDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property)) {
                    (<any>this)[property] = (<any>data)[property];
                }
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.userID = data["userID"];
            this.userName = data["userName"];
            this.password = data["password"];
            this.refID = data["refID"];
            this.refType = data["refType"];
            this.parentID = data["parentID"];
            this.status = data["status"];
            this.logged = data["logged"];
            this.logStation = data["logStation"];
            this.lastLogin = data["lastLogin"];
            this.lastLogout = data["lastLogout"];
            this.rowguid = data["rowguid"];
            this.createDate = data["createDate"];
            this.lastUpdate = data["lastUpdate"];
            this.createBy = data["createBy"];
            this.updateBy = data["updateBy"];
            this.syncCreate = data["syncCreate"];
            this.syncLastUpd = data["syncLastUpd"];
            this.groupCode = data["groupCode"];
            this.appId = data["appId"];
            this.kkm = data["kkm"];
            this.permissionAPI = data["permissionAPI"];
            this.companyType = data["companyType"];
            this.statusDesc = data['statusDesc'];
            this.accessCode = data['accessCode'];
            this.accessDesc = data['accessDesc'];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["userID"] = this.userID;
        data["userName"] = this.userName;
        data["password"] = this.password;
        data["refID"] = this.refID;
        data["refType"] = this.refType;
        data["parentID"] = this.parentID;
        data["status"] = this.status;
        data["logged"] = this.logged;
        data["logStation"] = this.logStation;
        data["lastLogin"] = this.lastLogin;
        data["lastLogout"] = this.lastLogout;
        data["rowguid"] = this.rowguid;
        data["createDate"] = this.createDate;
        data["lastUpdate"] = this.lastUpdate;
        data["createBy"] = this.createBy;
        data["updateBy"] = this.updateBy;
        data["syncCreate"] = this.syncCreate;
        data["syncLastUpd"] = this.syncLastUpd;
        data["groupCode"] = this.groupCode;
        data["appId"] = this.appId;
        data["kkm"] = this.kkm;
        data["permissionAPI"] = this.permissionAPI;
        data["companyType"] = this.companyType;
        data['statusDesc'] = this.statusDesc;
        data['accessCode'] = this.accessCode;
        data['accessDesc'] = this.accessDesc;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new UserProfileDto();
        result.init(json);
        return result;
    }
}

export interface IUserProfileDto {
    userID: string;
    userName: string;
    password: string;
    refID: string;
    refType: string;
    parentID: string;
    status: string;
    logged: string;
    logStation: string;
    lastLogin: string;
    lastLogout: string;
    rowguid: string;
    createDate: string;
    lastUpdate: string;
    createBy: string;
    updateBy: string;
    syncCreate: string;
    syncLastUpd: string;
    groupCode: string;
    appId: string;
    kkm: string;
    permissionAPI: string;
    companyType: string;
    statusDesc: string;
    accessCode: string;
    accessDesc: string;
}

export class PagedResultDtoOfUserProfileDto implements IPagedResultDtoOfUserProfileDto {
    totalCount: number;
    items: UserProfileDto[];

    static fromJS(data: any): PagedResultDtoOfUserProfileDto {
        let result = new PagedResultDtoOfUserProfileDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfUserProfileDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property)) {
                    (<any>this)[property] = (<any>data)[property];
                }
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.totalCount = data['totalCount'];
            if (data['items'] && data['items'].constructor === Array) {
                this.items = [];
                for (let item of data['items']) {
                    this.items.push(UserProfileDto.fromJS(item));
                }
            }
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data['totalCount'] = this.totalCount;
        if (this.items && this.items.constructor === Array) {
            data['items'] = [];
            for (let item of this.items) {
                data['items'].push(item.toJSON());
            }
        }
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new PagedResultDtoOfUserProfileDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfUserProfileDto {
    totalCount: number;
    items: UserProfileDto[];
}
