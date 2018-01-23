import { state } from '@angular/core';
import { Observable } from 'rxjs/Observable';

export class CreateGroupDto implements ICreateGroupDto {
    appID: number;
    groupName: string;
    status: number;

    constructor(data?: ICreateGroupDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.appID = data["appid"];
            this.groupName = data["groupName"];
            this.status = data["status"];
        }
    }

    static fromJS(data: any): CreateGroupDto {
        let result = new CreateGroupDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["appid"] = this.appID;
        data["groupName"] = this.groupName;
        data["status"] = this.status;
        return data; 
    }

    clone() {
        const json = this.toJSON();
        let result = new CreateGroupDto();
        result.init(json);
        return result;
    }
}

export interface ICreateGroupDto {
    appID: number;
    groupName: string;
    status: number;
}

export class UserGroupDto implements IUserGroupDto {
    appID: number;
    groupCode: string;
    groupName: string;
    accessLevel: number;
    status: number;
    CreateDate: Date;
    CreateBy: string;
    LastUpdate: Date;
    UpdateBy: string;
    SyncCreate: Date;
    LastSyncUpd: Date;

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
            this.appID = data["appid"];
            this.groupCode = data["groupCode"];
            this.groupName = data["groupName"];
            this.accessLevel = data["accessLevel"];
            this.status = data["status"];
            this.CreateDate = data["createDate"];
            this.CreateBy = data["createBy"];
            this.LastUpdate = data["lastUpdate"];
            this.UpdateBy = data["updateBy"];
            this.SyncCreate = data["syncCreate"];
            this.LastSyncUpd = data["lastSyncUpd"];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["appid"] = this.appID;
        data["groupCode"] = this.groupCode;
        data["groupName"] = this.groupName;
        data["accessLevel"] = this.accessLevel;
        data["status"] = this.status;
        data["createDate"] = this.CreateDate;
        data["createBy"] = this.CreateBy;
        data["lastUpdate"] = this.LastUpdate;
        data["updateBy"] = this.UpdateBy;
        data["syncCreate"] = this.SyncCreate;
        data["lastSyncUpd"] = this.LastSyncUpd;
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
    appID: number;
    groupCode: string;
    groupName: string;
    accessLevel: number;
    status: number;
    CreateDate: Date;
    CreateBy: string;
    LastUpdate: Date;
    UpdateBy: string;
    SyncCreate: Date;
    LastSyncUpd: Date;
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
