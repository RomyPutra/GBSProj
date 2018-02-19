import { state } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { strictEqual } from 'assert';

export class CapacityDto implements ICapacityDto {
	marketCode: string;
	analyst: string;
	inRoute: string;
	inGrpCap: number;
	outRoute: string;
	outGrpCap: number;
    
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
			this.marketCode = data['marketCode'];
			this.analyst = data['analyst'];
			this.inRoute = data['inRoute'];
			this.inGrpCap = data['inGrpCap'];
			this.outRoute = data['outRoute'];
			this.outGrpCap = data['outGrpCap'];
        }
    }

    toJSON(data?: any) {
		data['marketCode'] = this.marketCode;
		data['analyst'] = this.analyst;
		data['inRoute'] = this.inRoute;
		data['inGrpCap'] = this.inGrpCap;
		data['outRoute'] = this.outRoute;
		data['outGrpCap'] = this.outGrpCap;
        
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
	marketCode: string;
	analyst: string;
	inRoute: string;
	inGrpCap: number;
	outRoute: string;
	outGrpCap: number;
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
