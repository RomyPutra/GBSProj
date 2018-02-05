import { state } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { strictEqual } from 'assert';

export class CapacityDto implements ICapacityDto {
	MarketCode: string;
	Analyst: string;
	InRoute: string;
	InGrpCap: number;
	OutRoute: string;
	OutGrpCap: number;
    
    static fromJS(data: any): CapacityDto {
        let result = new CapacityDto();
        result.init(data);
        return result;
    }

    constructor(data?: ICapacityDto) {
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
			this.MarketCode = data['MarketCode'];
			this.Analyst = data['Analyst'];
			this.InRoute = data['InRoute'];
			this.InGrpCap = data['InGrpCap'];
			this.OutRoute = data['OutRoute'];
			this.OutGrpCap = data['OutGrpCap'];
        }
    }

    toJSON(data?: any) {
		data['MarketCode'] = this.MarketCode;
		data['Analyst'] = this.Analyst;
		data['InRoute'] = this.InRoute;
		data['InGrpCap'] = this.InGrpCap;
		data['OutRoute'] = this.OutRoute;
		data['OutGrpCap'] = this.OutGrpCap;
        
		return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new CapacityDto();
        result.init(json);
        return result;
    }
}

export interface ICapacityDto {
	MarketCode: string;
	Analyst: string;
	InRoute: string;
	InGrpCap: number;
	OutRoute: string;
	OutGrpCap: number;
}

export class PagedResultDtoOfCapacityDto implements IPagedResultDtoOfCapacityDto {
    totalCount: number;
    items: CapacityDto[];

    static fromJS(data: any): PagedResultDtoOfCapacityDto {
        let result = new PagedResultDtoOfCapacityDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfCapacityDto) {
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
                    this.items.push(CapacityDto.fromJS(item));
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
        let result = new PagedResultDtoOfCapacityDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfCapacityDto {
    totalCount: number;
    items: CapacityDto[];
}
