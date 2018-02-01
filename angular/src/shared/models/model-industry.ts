import { Observable } from 'rxjs/Observable';

export interface IIndustryDto {
    sicCode: string;
    sicDesc: string;
    sicDescEng: string;
    sicType: string;
    capacityLevel: number;
    createDate: Date;
    createBy: string;
    lastUpdate: Date;
    updateBy: string;
    active: string;
    inuse: string;
    rowguid: string;
    syncCreate: Date;
    syncLastUpd: Date;
    isHost: string;
    lastSyncBy: string;
}

export interface ISubIndustryDto {
    sicCode: string;
    subsicCode: string;
    subsicDesc: string;
    sicType: string;
    oldSIC: string;
    premiseCode: string;
    premiseType: string;
    createDate: Date;
    createBy: string;
    lastUpdate: Date;
    updateBy: string;
    active: string;
    inuse: string;
    rowguid: string;
    syncCreate: Date;
    syncLastUpd: Date;
    isHost: string;
    lastSyncBy: string;
}

export interface IPagedResultDtoOfIndustryDto {
    totalCount: number;
    items: IndustryDto[];
}

export interface IPagedResultDtoOfSubIndustryDto {
    totalCount: number;
    items: SubIndustryDto[];
}

export class IndustryDto implements IIndustryDto {
    sicCode: string;
    sicDesc: string;
    sicDescEng: string;
    sicType: string;
    capacityLevel: number;
    createDate: Date;
    createBy: string;
    lastUpdate: Date;
    updateBy: string;
    active: string;
    inuse: string;
    rowguid: string;
    syncCreate: Date;
    syncLastUpd: Date;
    isHost: string;
    lastSyncBy: string;

    static fromJS(data: any): IndustryDto {
        let result = new IndustryDto();
        result.init(data);
        return result;
    }

    constructor(data?: IIndustryDto) {
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
            this.sicCode = data['sicCode'];
            this.sicDesc = data['sicDesc'];
            this.sicDescEng = data['sicDescEng'];
            this.sicType = data['sicType'];
            this.capacityLevel = data['capacityLevel'];
            this.createDate = data['createDate'];
            this.createBy = data['createBy'];
            this.lastUpdate = data['lastUpdate'];
            this.updateBy = data['updateBy'];
            this.active = data['active'];
            this.inuse = data['inuse'];
            this.rowguid = data['rowguid'];
            this.syncCreate = data['syncCreate'];
            this.syncLastUpd = data['syncLastUpd'];
            this.isHost = data['isHost'];
            this.lastSyncBy = data['lastSyncBy'];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data['sicCode'] = this.sicCode;
        data['sicDesc'] = this.sicDesc;
        data['sicDescEng'] = this.sicDescEng;
        data['sicType'] = this.sicType;
        data['capacityLevel'] = this.capacityLevel;
        data['createDate'] = this.createDate;
        data['createBy'] = this.createBy;
        data['lastUpdate'] = this.lastUpdate;
        data['updateBy'] = this.updateBy;
        data['active'] = this.active;
        data['inuse'] = this.inuse;
        data['rowguid'] = this.rowguid;
        data['syncCreate'] = this.syncCreate;
        data['syncLastUpd'] = this.syncLastUpd;
        data['isHost'] = this.isHost;
        data['lastSyncBy'] = this.lastSyncBy;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new IndustryDto();
        result.init(json);
        return result;
    }
}

export class SubIndustryDto implements ISubIndustryDto {
    sicCode: string;
    subsicCode: string;
    subsicDesc: string;
    sicType: string;
    oldSIC: string;
    premiseCode: string;
    premiseType: string;
    createDate: Date;
    createBy: string;
    lastUpdate: Date;
    updateBy: string;
    active: string;
    inuse: string;
    rowguid: string;
    syncCreate: Date;
    syncLastUpd: Date;
    isHost: string;
    lastSyncBy: string;

    static fromJS(data: any): SubIndustryDto {
        let result = new SubIndustryDto();
        result.init(data);
        return result;
    }

    constructor(data?: ISubIndustryDto) {
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
            this.sicCode = data['sicCode'];
            this.subsicCode = data['subsicCode'];
            this.subsicDesc = data['subsicDesc'];
            this.sicType = data['sicType'];
            this.oldSIC = data['oldSIC'];
            this.premiseCode = data['premiseCode'];
            this.premiseType = data['premiseType'];
            this.createDate = data['createDate'];
            this.createBy = data['createBy'];
            this.lastUpdate = data['lastUpdate'];
            this.updateBy = data['updateBy'];
            this.active = data['active'];
            this.inuse = data['inuse'];
            this.rowguid = data['rowguid'];
            this.syncCreate = data['syncCreate'];
            this.syncLastUpd = data['syncLastUpd'];
            this.isHost = data['isHost'];
            this.lastSyncBy = data['lastSyncBy'];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data['sicCode'] = this.sicCode;
        data['subsicCode'] = this.subsicCode;
        data['subsicDesc'] = this.subsicDesc;
        data['sicType'] = this.sicType;
        data['oldSIC'] = this.oldSIC;
        data['premiseCode'] = this.premiseCode;
        data['premiseType'] = this.premiseType;
        data['createDate'] = this.createDate;
        data['createBy'] = this.createBy;
        data['lastUpdate'] = this.lastUpdate;
        data['updateBy'] = this.updateBy;
        data['active'] = this.active;
        data['inuse'] = this.inuse;
        data['rowguid'] = this.rowguid;
        data['syncCreate'] = this.syncCreate;
        data['syncLastUpd'] = this.syncLastUpd;
        data['isHost'] = this.isHost;
        data['lastSyncBy'] = this.lastSyncBy;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new SubIndustryDto();
        result.init(json);
        return result;
    }
}

export class PagedResultDtoOfIndustryDto implements IPagedResultDtoOfIndustryDto {
    totalCount: number;
    items: IndustryDto[];

    static fromJS(data: any): PagedResultDtoOfIndustryDto {
        let result = new PagedResultDtoOfIndustryDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfIndustryDto) {
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
                    this.items.push(IndustryDto.fromJS(item));
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
        let result = new PagedResultDtoOfIndustryDto();
        result.init(json);
        return result;
    }
}

export class PagedResultDtoOfSubIndustryDto implements IPagedResultDtoOfSubIndustryDto {
    totalCount: number;
    items: SubIndustryDto[];

    static fromJS(data: any): PagedResultDtoOfSubIndustryDto {
        let result = new PagedResultDtoOfSubIndustryDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfSubIndustryDto) {
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
                    this.items.push(SubIndustryDto.fromJS(item));
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
        let result = new PagedResultDtoOfSubIndustryDto();
        result.init(json);
        return result;
    }
}
