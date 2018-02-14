import { state } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { strictEqual } from 'assert';

export class PUDiscDto implements IPUDiscDto {
	marketCode: string;
	analyst: string;
    ndoDesc: string;
    inRoute: string;
    inPUDisc1: number;
    inPUDisc2: number;
    inPUDisc3: number;
    inPUDisc4: number;
    inPUDisc5: number;
    inPUDisc6: number;
    inPUDisc7: number;
    inPUDisc8: number;
    inPUDisc9: number;
    inPUDisc10: number;
    inPUDisc11: number;
    outRoute: string;
    outPUDisc1: number;
    outPUDisc2: number;
    outPUDisc3: number;
    outPUDisc4: number;
    outPUDisc5: number;
    outPUDisc6: number;
    outPUDisc7: number;
    outPUDisc8: number;
    outPUDisc9: number;
    outPUDisc10: number;
    outPUDisc11: number;
    
    static fromJS(data: any): PUDiscDto {
        let result = new PUDiscDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPUDiscDto) {
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
            this.inPUDisc1 = data['inPUDisc1'];
            this.inPUDisc2 = data['inPUDisc2'];
            this.inPUDisc3 = data['inPUDisc3'];
            this.inPUDisc4 = data['inPUDisc4'];
            this.inPUDisc5 = data['inPUDisc5'];
            this.inPUDisc6 = data['inPUDisc6'];
            this.inPUDisc7 = data['inPUDisc7'];
            this.inPUDisc8 = data['inPUDisc8'];
            this.inPUDisc9 = data['inPUDisc9'];
            this.inPUDisc10 = data['inPUDisc10'];
            this.inPUDisc11 = data['inPUDisc11'];
            this.outRoute = data['outRoute'];
            this.outPUDisc1 = data['outPUDisc1'];
            this.outPUDisc2 = data['outPUDisc2'];
            this.outPUDisc3 = data['outPUDisc3'];
            this.outPUDisc4 = data['outPUDisc4'];
            this.outPUDisc5 = data['outPUDisc5'];
            this.outPUDisc6 = data['outPUDisc6'];
            this.outPUDisc7 = data['outPUDisc7'];
            this.outPUDisc8 = data['outPUDisc8'];
            this.outPUDisc9 = data['outPUDisc9'];
            this.outPUDisc10 = data['outPUDisc10'];
            this.outPUDisc11 = data['outPUDisc11'];
        }
    }

    toJSON(data?: any) {
		data['marketCode'] = this.marketCode;
		data['analyst'] = this.analyst;
		data['ndoDesc'] = this.inRoute;
		data['inRoute'] = this.inRoute;
		data['inPUDisc1'] = this.inPUDisc1;
        data['inPUDisc2'] = this.inPUDisc2;
        data['inPUDisc3'] = this.inPUDisc3;
        data['inPUDisc4'] = this.inPUDisc4;
        data['inPUDisc5'] = this.inPUDisc5;
        data['inPUDisc6'] = this.inPUDisc6;
        data['inPUDisc7'] = this.inPUDisc7;
        data['inPUDisc8'] = this.inPUDisc8;
        data['inPUDisc9'] = this.inPUDisc9;
        data['inPUDisc10'] = this.inPUDisc10;
        data['inPUDisc11'] = this.inPUDisc11;
        data['outRoute'] = this.outRoute;
		data['outPUDisc1'] = this.outPUDisc1;
        data['outPUDisc2'] = this.outPUDisc2;
        data['outPUDisc3'] = this.outPUDisc3;
        data['outPUDisc4'] = this.outPUDisc4;
        data['outPUDisc5'] = this.outPUDisc5;
        data['outPUDisc6'] = this.outPUDisc6;
        data['outPUDisc7'] = this.outPUDisc7;
        data['outPUDisc8'] = this.outPUDisc8;
        data['outPUDisc9'] = this.outPUDisc9;
        data['outPUDisc10'] = this.outPUDisc10;
        data['outPUDisc11'] = this.outPUDisc11;
        
		return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new PUDiscDto();
        result.init(json);
        return result;
    }
}

export interface IPUDiscDto {
	marketCode: string;
	analyst: string;
    ndoDesc: string;
    inRoute: string;
    inPUDisc1: number;
    inPUDisc2: number;
    inPUDisc3: number;
    inPUDisc4: number;
    inPUDisc5: number;
    inPUDisc6: number;
    inPUDisc7: number;
    inPUDisc8: number;
    inPUDisc9: number;
    inPUDisc10: number;
    inPUDisc11: number;
    outRoute: string;
    outPUDisc1: number;
    outPUDisc2: number;
    outPUDisc3: number;
    outPUDisc4: number;
    outPUDisc5: number;
    outPUDisc6: number;
    outPUDisc7: number;
    outPUDisc8: number;
    outPUDisc9: number;
    outPUDisc10: number;
    outPUDisc11: number;
}

export class PagedResultDtoOfPUDiscDto implements IPagedResultDtoOfPUDiscDto {
    totalCount: number;
    items: PUDiscDto[];

    static fromJS(data: any): PagedResultDtoOfPUDiscDto {
        let result = new PagedResultDtoOfPUDiscDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfPUDiscDto) {
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
                    this.items.push(PUDiscDto.fromJS(item));
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
        let result = new PagedResultDtoOfPUDiscDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfPUDiscDto {
    totalCount: number;
    items: PUDiscDto[];
}
