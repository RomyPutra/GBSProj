import { state } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { strictEqual } from 'assert';

export class GB4Dto implements IGB4Dto {
    orgName: string;
    username: string;
    countryCode: string;
    origin: string;
    noofPax: number;
    status: number;
    effectiveDate: string;
    expiryDate: string;
    
    static fromJS(data: any): GB4Dto {
        let result = new GB4Dto();
        result.init(data);
        return result;
    }

    constructor(data?: IGB4Dto) {
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
			this.orgName = data['orgName'];
			this.username = data['username'];
			this.countryCode = data['countryCode'];
			this.origin = data['origin'];
			this.noofPax = data['noofPax'];
            this.status = data['status'];
            this.effectiveDate = data['effectiveDate'];
            this.expiryDate = data['expiryDate'];
        }
    }

    toJSON(data?: any) {
		data['orgName'] = this.orgName;
		data['username'] = this.username;
		data['countryCode'] = this.countryCode;
		data['origin'] = this.origin;
		data['noofPax'] = this.noofPax;
        data['status'] = this.status;
        data['effectiveDate'] = this.effectiveDate;
        data['expiryDate'] = this.expiryDate;
        
		return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new GB4Dto();
        result.init(json);
        return result;
    }
}

export interface IGB4Dto {
    orgName: string;
    username: string;
    countryCode: string;
    origin: string;
    noofPax: number;
    status: number;
    effectiveDate: string;
    expiryDate: string;
}

export class PagedResultDtoOfGB4Dto implements IPagedResultDtoOfGB4Dto {
    totalCount: number;
    items: GB4Dto[];

    static fromJS(data: any): PagedResultDtoOfGB4Dto {
        let result = new PagedResultDtoOfGB4Dto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfGB4Dto) {
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
                    this.items.push(GB4Dto.fromJS(item));
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
        let result = new PagedResultDtoOfGB4Dto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfGB4Dto {
    totalCount: number;
    items: GB4Dto[];
}
