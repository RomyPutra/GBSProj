import { state } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { strictEqual } from 'assert';

export class DiscountDto implements IDiscountDto {
	marketCode: string;
	analyst: string;
	inRoute: string;
	inMaxDisc: number;
	outRoute: string;
	outMaxDisc: number;
    
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
			this.marketCode = data['marketCode'];
			this.analyst = data['analyst'];
			this.inRoute = data['inRoute'];
			this.inMaxDisc = data['inMaxDisc'];
			this.outRoute = data['outRoute'];
			this.outMaxDisc = data['outMaxDisc'];
        }
    }

    toJSON(data?: any) {
		data['marketCode'] = this.marketCode;
		data['analyst'] = this.analyst;
		data['inRoute'] = this.inRoute;
		data['inMaxDisc'] = this.inMaxDisc;
		data['outRoute'] = this.outRoute;
		data['outMaxDisc'] = this.outMaxDisc;
        
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
	marketCode: string;
	analyst: string;
	inRoute: string;
	inMaxDisc: number;
	outRoute: string;
	outMaxDisc: number;
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
