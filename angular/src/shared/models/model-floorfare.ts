import { state } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { strictEqual } from 'assert';

export class FloorFareDto implements IFloorFareDto {
	analyst: string;
	marketCode: string;
	inRoute: string;
	inCurrency: string;
	inDisc: number;
	inFareClass: string;
	inFloorFare: number;
	outRoute: string;
	outCurrency: string;
	outDisc: number;
	outFareClass: string;
	outFloorFare: number;
    
    static fromJS(data: any): FloorFareDto {
        let result = new FloorFareDto();
        result.init(data);
        return result;
    }

    constructor(data?: IFloorFareDto) {
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
			this.inRoute = data['inRoute'];
			this.inCurrency = data['inCurrency'];
			this.inDisc = data['inDisc'];
			this.inFareClass = data['inFareClass'];
			this.inFloorFare = data['inFloorFare'];
			this.outRoute = data['outRoute'];
			this.outCurrency = data['outCurrency'];
			this.outDisc = data['outDisc'];
			this.outFareClass = data['outFareClass'];
			this.outFloorFare = data['outFloorFare'];
        }
    }

    toJSON(data?: any) {
		data['analyst'] = this.analyst;
		data['marketCode'] = this.marketCode;
		data['inRoute'] = this.inRoute;
		data['inCurrency'] = this.inCurrency;
		data['inDisc'] = this.inDisc;
		data['inFareClass'] = this.inFareClass;
		data['inFloorFare'] = this.inFloorFare;
		data['outRoute'] = this.outRoute;
		data['outCurrency'] = this.outCurrency;
		data['outDisc'] = this.outDisc;
		data['outFareClass'] = this.outFareClass;
		data['outFloorFare'] = this.outFloorFare;
        
		return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new FloorFareDto();
        result.init(json);
        return result;
    }
}

export interface IFloorFareDto {
	analyst: string;
	marketCode: string;
	inRoute: string;
	inCurrency: string;
	inDisc: number;
	inFareClass: string;
	inFloorFare: number;
	outRoute: string;
	outCurrency: string;
	outDisc: number;
	outFareClass: string;
	outFloorFare: number;
}

export class PagedResultDtoOfFloorFareDto implements IPagedResultDtoOfFloorFareDto {
    totalCount: number;
    items: FloorFareDto[];

    static fromJS(data: any): PagedResultDtoOfFloorFareDto {
        let result = new PagedResultDtoOfFloorFareDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfFloorFareDto) {
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
                    this.items.push(FloorFareDto.fromJS(item));
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
        let result = new PagedResultDtoOfFloorFareDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfFloorFareDto {
    totalCount: number;
    items: FloorFareDto[];
}
