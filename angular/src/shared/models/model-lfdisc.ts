import { state } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { strictEqual } from 'assert';

export class LFDiscDto implements ILFDiscDto {
	marketCode: string;
	analyst: string;
    ndoDesc: string;
    inRoute: string;
    inLFDisc1: number;
    inLFDisc2: number;
    inLFDisc3: number;
    inLFDisc4: number;
    inLFDisc5: number;
    inLFDisc6: number;
    inLFDisc7: number;
    inLFDisc8: number;
    inLFDisc9: number;
    inLFDisc10: number;
    outRoute: string;
    outLFDisc1: number;
    outLFDisc2: number;
    outLFDisc3: number;
    outLFDisc4: number;
    outLFDisc5: number;
    outLFDisc6: number;
    outLFDisc7: number;
    outLFDisc8: number;
    outLFDisc9: number;
    outLFDisc10: number;
    
    static fromJS(data: any): LFDiscDto {
        let result = new LFDiscDto();
        result.init(data);
        return result;
    }

    constructor(data?: ILFDiscDto) {
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
			this.ndoDesc = data['ndoDesc'];
            this.inRoute = data['inRoute'];
            this.inLFDisc1 = data['inLFDisc1'];
            this.inLFDisc2 = data['inLFDisc2'];
            this.inLFDisc3 = data['inLFDisc3'];
            this.inLFDisc4 = data['inLFDisc4'];
            this.inLFDisc5 = data['inLFDisc5'];
            this.inLFDisc6 = data['inLFDisc6'];
            this.inLFDisc7 = data['inLFDisc7'];
            this.inLFDisc8 = data['inLFDisc8'];
            this.inLFDisc9 = data['inLFDisc9'];
            this.inLFDisc10 = data['inLFDisc10'];
            this.outRoute = data['outRoute'];
            this.outLFDisc1 = data['outLFDisc1'];
            this.outLFDisc2 = data['outLFDisc2'];
            this.outLFDisc3 = data['outLFDisc3'];
            this.outLFDisc4 = data['outLFDisc4'];
            this.outLFDisc5 = data['outLFDisc5'];
            this.outLFDisc6 = data['outLFDisc6'];
            this.outLFDisc7 = data['outLFDisc7'];
            this.outLFDisc8 = data['outLFDisc8'];
            this.outLFDisc9 = data['outLFDisc9'];
        }
    }

    toJSON(data?: any) {
		data['marketCode'] = this.marketCode;
		data['analyst'] = this.analyst;
		data['ndoDesc'] = this.inRoute;
		data['inRoute'] = this.inRoute;
		data['inLFDisc1'] = this.inLFDisc1;
        data['inLFDisc2'] = this.inLFDisc2;
        data['inLFDisc3'] = this.inLFDisc3;
        data['inLFDisc4'] = this.inLFDisc4;
        data['inLFDisc5'] = this.inLFDisc5;
        data['inLFDisc6'] = this.inLFDisc6;
        data['inLFDisc7'] = this.inLFDisc7;
        data['inLFDisc8'] = this.inLFDisc8;
        data['inLFDisc9'] = this.inLFDisc9;
        data['inLFDisc10'] = this.inLFDisc10;
        data['outRoute'] = this.outRoute;
		data['outLFDisc1'] = this.outLFDisc1;
        data['outLFDisc2'] = this.outLFDisc2;
        data['outLFDisc3'] = this.outLFDisc3;
        data['outLFDisc4'] = this.outLFDisc4;
        data['outLFDisc5'] = this.outLFDisc5;
        data['outLFDisc6'] = this.outLFDisc6;
        data['outLFDisc7'] = this.outLFDisc7;
        data['outLFDisc8'] = this.outLFDisc8;
        data['outLFDisc9'] = this.outLFDisc9;
		data['outLFDisc10'] = this.outLFDisc10;
        
		return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new LFDiscDto();
        result.init(json);
        return result;
    }
}

export interface ILFDiscDto {
	marketCode: string;
	analyst: string;
    ndoDesc: string;
    inRoute: string;
    inLFDisc1: number;
    inLFDisc2: number;
    inLFDisc3: number;
    inLFDisc4: number;
    inLFDisc5: number;
    inLFDisc6: number;
    inLFDisc7: number;
    inLFDisc8: number;
    inLFDisc9: number;
    inLFDisc10: number;
    outRoute: string;
    outLFDisc1: number;
    outLFDisc2: number;
    outLFDisc3: number;
    outLFDisc4: number;
    outLFDisc5: number;
    outLFDisc6: number;
    outLFDisc7: number;
    outLFDisc8: number;
    outLFDisc9: number;
    outLFDisc10: number;
}

export class PagedResultDtoOfLFDiscDto implements IPagedResultDtoOfLFDiscDto {
    totalCount: number;
    items: LFDiscDto[];

    static fromJS(data: any): PagedResultDtoOfLFDiscDto {
        let result = new PagedResultDtoOfLFDiscDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfLFDiscDto) {
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
                    this.items.push(LFDiscDto.fromJS(item));
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
        let result = new PagedResultDtoOfLFDiscDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfLFDiscDto {
    totalCount: number;
    items: LFDiscDto[];
}
