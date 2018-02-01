import { state } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { strictEqual } from 'assert';

export class AgentTierDto implements IAgentTierDto {
    marketCode: string;
    inTier: string;
    inSubTier: string;
    outTier: string;
    outSubTier: string;
    inAgentID: string;
    outAgentID: string;
    status: number;
    inuse: number;
    syncCreate: Date;
    syncLastUpd: Date;
    lastSyncBy: string;
    createDate: Date;
    createBy: string;
    updateDate: Date;
    updateBy: string;
    active: number;
    
    static fromJS(data: any): AgentTierDto {
        let result = new AgentTierDto();
        result.init(data);
        return result;
    }

    constructor(data?: IAgentTierDto) {
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
            this.marketCode = data['marketCode'];
            this.inTier = data['inTier'];
            this.inSubTier = data['inSubTier'];
            this.outTier = data['outTier'];
            this.outSubTier = data['outSubTier'];
            this.inAgentID = data['inAgentID'];
            this.outAgentID = data['outAgentID'];
            this.status = data['status;'];
            this.inuse = data['inuse'];
            this.syncCreate = data['syncCreate'];
            this.syncLastUpd = data['syncLastUpd'];
            this.lastSyncBy = data['lastSyncBy'];
            this.createDate = data['createDate'];
            this.createBy = data['createBy'];
            this.updateDate = data['updateDate'];
            this.updateBy = data['updateBy'];
            this.active = data['active'];
        }
    }

    toJSON(data?: any) {
        data['marketCode'] = this.marketCode;
        data['inTier'] = this.inTier;
        data['inSubTier'] = this.inSubTier;
        data['outTier'] = this.outTier;
        data['outSubTier'] = this.outSubTier;
        data['inAgentID'] = this.inAgentID;
        data['outAgentID'] = this.outAgentID;
        data['status;'] = this.status;
        data['inuse'] = this.inuse;
        data['syncCreate'] = this.syncCreate;
        data['syncLastUpd'] = this.syncLastUpd;
        data['lastSyncBy'] = this.lastSyncBy;
        data['createDate'] = this.createDate;
        data['createBy'] = this.createBy;
        data['updateDate'] = this.updateDate;
        data['updateBy'] = this.updateBy;
        data['active'] = this.active;
        
		return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new AgentTierDto();
        result.init(json);
        return result;
    }
}

export interface IAgentTierDto {
    marketCode: string;
    inTier: string;
    inSubTier: string;
    outTier: string;
    outSubTier: string;
    inAgentID: string;
    outAgentID: string;
    status: number;
    inuse: number;
    syncCreate: Date;
    syncLastUpd: Date;
    lastSyncBy: string;
    createDate: Date;
    createBy: string;
    updateDate: Date;
    updateBy: string;
    active: number;
}

export class PagedResultDtoOfAgentTierDto implements IPagedResultDtoOfAgentTierDto {
    totalCount: number;
    items: AgentTierDto[];

    static fromJS(data: any): PagedResultDtoOfAgentTierDto {
        let result = new PagedResultDtoOfAgentTierDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfAgentTierDto) {
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
                    this.items.push(AgentTierDto.fromJS(item));
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
        let result = new PagedResultDtoOfAgentTierDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfAgentTierDto {
    totalCount: number;
    items: AgentTierDto[];
}

export class AgentAccessFareDto implements IAgentAccessFareDto {
    analyst: string;
    marketCode: string;
    outRoute: string;
    outTier1: string;
    outTier2: string;
    outTier3: string;
    outGeneric: string;
    inRoute: string;
    inTier1: string;
    inTier2: string;
    inTier3: string;
    inGeneric: string;
    // marketCode: string;
    // inTier: string;
    // outTier: string;
    // inFareClass: string;
    // outFareClass: string;
    // status: number;
    // inuse: number;
    // syncCreate: Date;
    // syncLastUpd: Date;
    // lastSyncBy: string;
    // createDate: Date;
    // createBy: string;
    // updateDate: Date;
    // updateBy: string;
    // active: number;
    
    static fromJS(data: any): AgentAccessFareDto {
        let result = new AgentAccessFareDto();
        result.init(data);
        return result;
    }

    constructor(data?: IAgentAccessFareDto) {
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
            this.analyst = data['analyst'];
            this.marketCode = data['marketCode'];
            this.outRoute = data['outRoute'];
            this.outTier1 = data['outTier1'];
            this.outTier2 = data['outTier2'];
            this.outTier3 = data['outTier3'];
            this.outGeneric = data['outGeneric'];
            this.inRoute = data['inRoute'];
            this.inTier1 = data['inTier1'];
            this.inTier2 = data['inTier2'];
            this.inTier3 = data['inTier3'];
            this.inGeneric = data['inGeneric'];
            // this.marketCode = data['marketCode'];
            // this.inTier = data['inTier'];
            // this.outTier = data['outTier'];
            // this.inFareClass = data['inFareClass'];
            // this.outFareClass = data['outFareClass'];
            // this.status = data['status;'];
            // this.inuse = data['inuse'];
            // this.syncCreate = data['syncCreate'];
            // this.syncLastUpd = data['syncLastUpd'];
            // this.lastSyncBy = data['lastSyncBy'];
            // this.createDate = data['createDate'];
            // this.createBy = data['createBy'];
            // this.updateDate = data['updateDate'];
            // this.updateBy = data['updateBy'];
            // this.active = data['active'];
        }
    }

    toJSON(data?: any) {
        data['analyst'] = this.analyst;
        data['marketCode'] = this.marketCode;
        data['outRoute'] = this.outRoute;
        data['outTier1'] = this.outTier1;
        data['outTier2'] = this.outTier2;
        data['outTier3'] = this.outTier3;
        data['outGeneric'] = this.outGeneric;
        data['inRoute'] = this.inRoute;
        data['inTier1'] = this.inTier1;
        data['inTier2'] = this.inTier2;
        data['inTier3'] = this.inTier3;
        data['inGeneric'] = this.inGeneric;
        // data['marketCode'] = this.marketCode;
        // data['inTier'] = this.inTier;
        // data['outTier'] = this.outTier;
        // data['inFareClass'] = this.inFareClass;
        // data['outFareClass'] = this.outFareClass;
        // data['status;'] = this.status;
        // data['inuse'] = this.inuse;
        // data['syncCreate'] = this.syncCreate;
        // data['syncLastUpd'] = this.syncLastUpd;
        // data['lastSyncBy'] = this.lastSyncBy;
        // data['createDate'] = this.createDate;
        // data['createBy'] = this.createBy;
        // data['updateDate'] = this.updateDate;
        // data['updateBy'] = this.updateBy;
        // data['active'] = this.active;
        
		return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new AgentAccessFareDto();
        result.init(json);
        return result;
    }
}

export interface IAgentAccessFareDto {
    analyst: string;
    marketCode: string;
    outRoute: string;
    outTier1: string;
    outTier2: string;
    outTier3: string;
    outGeneric: string;
    inRoute: string;
    inTier1: string;
    inTier2: string;
    inTier3: string;
    inGeneric: string;
    // marketCode: string;
    // inTier: string;
    // outTier: string;
    // inFareClass: string;
    // outFareClass: string;
    // status: number;
    // inuse: number;
    // syncCreate: Date;
    // syncLastUpd: Date;
    // lastSyncBy: string;
    // createDate: Date;
    // createBy: string;
    // updateDate: Date;
    // updateBy: string;
    // active: number;
}

export class PagedResultDtoOfAgentAccessFareDto implements IPagedResultDtoOfAgentAccessFareDto {
    totalCount: number;
    items: AgentAccessFareDto[];

    static fromJS(data: any): PagedResultDtoOfAgentAccessFareDto {
        let result = new PagedResultDtoOfAgentAccessFareDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfAgentAccessFareDto) {
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
                    this.items.push(AgentAccessFareDto.fromJS(item));
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
        let result = new PagedResultDtoOfAgentAccessFareDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfAgentAccessFareDto {
    totalCount: number;
    items: AgentAccessFareDto[];
}
