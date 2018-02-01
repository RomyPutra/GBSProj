import { Observable } from 'rxjs/Observable';

export interface IAreaDto {
    countryCode: string;
    countryDesc: string;
    stateCode: string;
    stateDesc: string;
    cityCode: string;
    cityDesc: string;
    areaCode: string;
    areaDesc: string;
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

export interface IPagedResultDtoOfAreaDto {
    totalCount: number;
    items: AreaDto[];
}

export class AreaDto implements IAreaDto {
    countryCode: string;
    countryDesc: string;
    stateCode: string;
    stateDesc: string;
    cityCode: string;
    cityDesc: string;
    areaCode: string;
    areaDesc: string;
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

    static fromJS(data: any): AreaDto {
        let result = new AreaDto();
        result.init(data);
        return result;
    }

    constructor(data?: IAreaDto) {
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
            this.cityCode = data["cityCode"];
            this.cityDesc = data["cityDesc"];
            this.areaCode = data["areaCode"];
            this.areaDesc = data["areaDesc"];
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
        data["cityCode"] = this.cityCode;
        data["cityDesc"] = this.cityDesc;
        data["areaCode"] = this.areaCode;
        data["areaDesc"] = this.areaDesc;
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
        let result = new AreaDto();
        result.init(json);
        return result;
    }
}

export class PagedResultDtoOfAreaDto implements IPagedResultDtoOfAreaDto {
    totalCount: number;
    items: AreaDto[];

    static fromJS(data: any): PagedResultDtoOfAreaDto {
        let result = new PagedResultDtoOfAreaDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfAreaDto) {
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
                    this.items.push(AreaDto.fromJS(item));
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
        let result = new PagedResultDtoOfAreaDto();
        result.init(json);
        return result;
    }
}
