import { Observable } from 'rxjs/Observable';

export class StateDto implements IStateDto {
    countryCode : string;
    countryDesc : string;
    countryCodeUpdate : string;
    stateCode : string;
    stateDesc : string;
    createDate : string;
    createBy: string;
    lastUpdate : string;
    updateBy : string;
    active : number;
    inuse : number;
    rowguid : string;
    syncCreate : string;
    syncLastUpd : string;
    isHost : number;
    lastSyncBy : string



    static fromJS(data: any): StateDto {
        let result = new StateDto();
        result.init(data);
        return result;
    }

    constructor(data?: IStateDto) {
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
            this.countryCode = data['countryCode'];
            this.countryDesc = data['countryDesc'];
            this.countryCodeUpdate = data['countryCodeUpdate'];
            this.stateCode = data['stateCode'];
            this.stateDesc = data['stateDesc'];
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
        data['countryCode'] = this.countryCode;
        data['countryDesc'] =this.countryDesc; 
        data['countryCodeUpdate'] = this.countryCodeUpdate; 
        data['stateCode'] = this.stateCode; 
        data['stateDesc'] = this.stateDesc;  
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
        let result = new StateDto();
        result.init(json);
        return result;
    }
}

export interface IStateDto {
    countryCode : string;
    countryDesc : string;
    countryCodeUpdate : string;
    stateCode : string;
    stateDesc : string;
    createDate : string;
    createBy: string;
    lastUpdate : string;
    updateBy : string;
    active : number;
    inuse : number;
    rowguid : string;
    syncCreate : string;
    syncLastUpd : string;
    isHost : number;
    lastSyncBy : string
}

export class PagedResultDtoOfStateDto implements IPagedResultDtoOfStateDto {
    totalCount: number;
    items: StateDto[];

    static fromJS(data: any): PagedResultDtoOfStateDto {
        let result = new PagedResultDtoOfStateDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfStateDto) {
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
                    this.items.push(StateDto.fromJS(item));
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
        let result = new PagedResultDtoOfStateDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfStateDto {
    totalCount: number;
    items: StateDto[];
}
