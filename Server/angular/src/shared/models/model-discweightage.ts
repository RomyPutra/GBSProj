import { state } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { strictEqual } from 'assert';

export class DiscWeightageDto implements IDiscWeightageDto {
	analyst: string;
	marketCode: string;
	inRoute: string;
	inWALFDisc: number;
	inWAPUDisc: number;
	outRoute: string;
	outWALFDisc: number;
	outWAPUDisc: number;
    
    static fromJS(data: any): DiscWeightageDto {
        let result = new DiscWeightageDto();
        result.init(data);
        return result;
    }

    constructor(data?: IDiscWeightageDto) {
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
			this.inWALFDisc = data['inWALFDisc'];
			this.inWAPUDisc = data['inWAPUDisc'];
			this.outRoute = data['outRoute'];
			this.outWALFDisc = data['outWALFDisc'];
			this.outWAPUDisc = data['outWAPUDisc'];
        }
    }

    toJSON(data?: any) {
		data['analyst'] = this.analyst;
		data['marketCode'] = this.marketCode;
		data['inRoute'] = this.inRoute;
		data['inWALFDisc'] = this.inWALFDisc;
		data['inWAPUDisc'] = this.inWAPUDisc;
		data['outRoute'] = this.outRoute;
		data['outWALFDisc'] = this.outWALFDisc;
		data['outWAPUDisc'] = this.outWAPUDisc;
        
		return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new DiscWeightageDto();
        result.init(json);
        return result;
    }
}

export interface IDiscWeightageDto {
	analyst: string;
	marketCode: string;
	inRoute: string;
	inWALFDisc: number;
	inWAPUDisc: number;
	outRoute: string;
	outWALFDisc: number;
	outWAPUDisc: number;
}

export class PagedResultDtoOfDiscWeightageDto implements IPagedResultDtoOfDiscWeightageDto {
    totalCount: number;
    items: DiscWeightageDto[];

    static fromJS(data: any): PagedResultDtoOfDiscWeightageDto {
        let result = new PagedResultDtoOfDiscWeightageDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfDiscWeightageDto) {
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
                    this.items.push(DiscWeightageDto.fromJS(item));
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
        let result = new PagedResultDtoOfDiscWeightageDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfDiscWeightageDto {
    totalCount: number;
    items: DiscWeightageDto[];
}
