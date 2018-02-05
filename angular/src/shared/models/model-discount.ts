import { state } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { strictEqual } from 'assert';

export class DiscountDto implements IDiscountDto {
	MarketCode: string;
	Analyst: string;
	InRoute: string;
	InMaxDisc: number;
	OutRoute: string;
	OutMaxDisc: number;
    
    static fromJS(data: any): DiscountDto {
        let result = new DiscountDto();
        result.init(data);
        return result;
    }

    constructor(data?: IDiscountDto) {
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
			this.InMaxDisc = data['InMaxDisc'];
			this.OutRoute = data['OutRoute'];
			this.OutMaxDisc = data['OutMaxDisc'];
        }
    }

    toJSON(data?: any) {
		data['MarketCode'] = this.MarketCode;
		data['Analyst'] = this.Analyst;
		data['InRoute'] = this.InRoute;
		data['InMaxDisc'] = this.InMaxDisc;
		data['OutRoute'] = this.OutRoute;
		data['OutMaxDisc'] = this.OutMaxDisc;
        
		return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new DiscountDto();
        result.init(json);
        return result;
    }
}

export interface IDiscountDto {
	MarketCode: string;
	Analyst: string;
	InRoute: string;
	InMaxDisc: number;
	OutRoute: string;
	OutMaxDisc: number;
}

export class PagedResultDtoOfDiscountDto implements IPagedResultDtoOfDiscountDto {
    totalCount: number;
    items: DiscountDto[];

    static fromJS(data: any): PagedResultDtoOfDiscountDto {
        let result = new PagedResultDtoOfDiscountDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfDiscountDto) {
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
                    this.items.push(DiscountDto.fromJS(item));
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
        let result = new PagedResultDtoOfDiscountDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfDiscountDto {
    totalCount: number;
    items: DiscountDto[];
}
