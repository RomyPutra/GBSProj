import { state } from '@angular/core';
import { Observable } from 'rxjs/Observable';

export class UserGroupDto implements IUserGroupDto {
    appid: number;
    groupCode: string;
    groupName: string;
    accessLevel: number;
    status: number;
    createDate: Date;
    createBy: string;
    lastUpdate: Date;
    updateBy: string;
    syncCreate: Date;
    lastSyncUpd: Date;

    static fromJS(data: any): UserGroupDto {
        let result = new UserGroupDto();
        result.init(data);
        return result;
    }

    constructor(data?: IUserGroupDto) {
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
            this.appid = data["appid"];
            this.groupCode = data["groupCode"];
            this.groupName = data["groupName"];
            this.accessLevel = data["accessLevel"];
            this.status = data["status"];
            this.createDate = data["createDate"];
            this.createBy = data["createBy"];
            this.lastUpdate = data["lastUpdate"];
            this.updateBy = data["updateBy"];
            this.syncCreate = data["syncCreate"];
            this.lastSyncUpd = data["lastSyncUpd"];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["appid"] = this.appid;
        data["groupCode"] = this.groupCode;
        data["groupName"] = this.groupName;
        data["accessLevel"] = this.accessLevel;
        data["status"] = this.status;
        data["createDate"] = this.createDate;
        data["createBy"] = this.createBy;
        data["lastUpdate"] = this.lastUpdate;
        data["updateBy"] = this.updateBy;
        data["syncCreate"] = this.syncCreate;
        data["lastSyncUpd"] = this.lastSyncUpd;
    return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new UserGroupDto();
        result.init(json);
        return result;
    }
}

export interface IUserGroupDto {
    appid: number;
    groupCode: string;
    groupName: string;
    accessLevel: number;
    status: number;
    createDate: Date;
    createBy: string;
    lastUpdate: Date;
    updateBy: string;
    syncCreate: Date;
    lastSyncUpd: Date;
}

export class PagedResultDtoOfUserGroupDto implements IPagedResultDtoOfUserGroupDto {
    totalCount: number;
    items: UserGroupDto[];

    static fromJS(data: any): PagedResultDtoOfUserGroupDto {
        let result = new PagedResultDtoOfUserGroupDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfUserGroupDto) {
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
            this.totalCount = data["totalCount"];
            if (data["items"] && data["items"].constructor === Array) {
                this.items = [];
                for (let item of data["items"]) {
                    this.items.push(UserGroupDto.fromJS(item));
                }
            }
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["totalCount"] = this.totalCount;
        if (this.items && this.items.constructor === Array) {
            data["items"] = [];
            for (let item of this.items) {
                data["items"].push(item.toJSON());
            }
        }
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new PagedResultDtoOfUserGroupDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfUserGroupDto {
    totalCount: number;
    items: UserGroupDto[];
}
