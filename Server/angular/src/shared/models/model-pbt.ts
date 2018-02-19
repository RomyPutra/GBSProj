import { Observable } from 'rxjs/Observable';

export interface IPBTDto {
    countryCode: string;
    countryDesc: string;
    stateCode: string;
    stateDesc: string;
    pbtCode: string;
    pbtDesc: string;
    status: number;
    createDate: Date;
    updateBy: string;
    lastUpdate: Date;
    createBy: string;
    active: number;
    flag: number;
    inuse: number;
    rowguid: string;
    syncCreate: Date;
    syncLastUpd: Date;
    isHost: number;
    lastSyncBy: string;
}

export interface IPagedResultDtoOfPBTDto {
    totalCount: number;
    items: PBTDto[];
}

export class PBTDto implements IPBTDto {
    countryCode: string;
    countryDesc: string;
    stateCode: string;
    stateDesc: string;
    pbtCode: string;
    pbtDesc: string;
    status: number;
    createDate: Date;
    updateBy: string;
    lastUpdate: Date;
    createBy: string;
    active: number;
    flag: number;
    inuse: number;
    rowguid: string;
    syncCreate: Date;
    syncLastUpd: Date;
    isHost: number;
    lastSyncBy: string;

    static fromJS(data: any): PBTDto {
        let result = new PBTDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPBTDto) {
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
            this.countryCode = data["countryCode"];
            this.countryDesc = data["countryDesc"];
            this.stateCode = data["stateCode"];
            this.stateDesc = data["stateDesc"];
            this.pbtCode = data["pbtCode"];
            this.pbtDesc = data["pbtDesc"];
            this.status = data["status"];
            this.createDate = data["createDate"];
            this.updateBy = data["updateBy"];
            this.lastUpdate = data["lastUpdate"];
            this.createBy = data["createBy"];
            this.active = data["active"];
            this.flag = data["flag"];
            this.inuse = data["inuse"];
            this.rowguid = data["rowguid"];
            this.syncCreate = data["syncCreate"];
            this.syncLastUpd = data["syncLastUpd"];
            this.isHost = data["isHost"];
            this.lastSyncBy = data["lastSyncBy"];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["countryCode"] = this.countryCode;
        data["countryDesc"] = this.countryDesc;
        data["stateCode"] = this.stateCode;
        data["stateDesc"] = this.stateDesc;
        data["pbtCode"] = this.pbtCode;
        data["pbtDesc"] = this.pbtDesc;
        data["status"] = this.status;
        data["createDate"] = this.createDate;
        data["updateBy"] = this.updateBy;
        data["lastUpdate"] = this.lastUpdate;
        data["createBy"] = this.createBy;
        data["active"] = this.active;
        data["flag"] = this.flag;
        data["inuse"] = this.inuse;
        data["rowguid"] = this.rowguid;
        data["syncCreate"] = this.syncCreate;
        data["syncLastUpd"] = this.syncLastUpd;
        data["isHost"] = this.isHost;
        data["lastSyncBy"] = this.lastSyncBy;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new PBTDto();
        result.init(json);
        return result;
    }
}

export class PagedResultDtoOfPBTDto implements IPagedResultDtoOfPBTDto {
    totalCount: number;
    items: PBTDto[];

    static fromJS(data: any): PagedResultDtoOfPBTDto {
        let result = new PagedResultDtoOfPBTDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfPBTDto) {
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
                    this.items.push(PBTDto.fromJS(item));
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
        let result = new PagedResultDtoOfPBTDto();
        result.init(json);
        return result;
    }
}
