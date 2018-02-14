import { state } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { strictEqual } from 'assert';

export class SeriesDto implements ISeriesDto {
	marketCode: string;
	analyst: string;
    ndoDesc: string;
    inRoute: string;
    inFlightTimeGrp: string;
    inDepDOW: string;
    inAgentTier: string;
    inCurrency: string;
    inLFFare1: number;
    inLFFare2: number;
    inLFFare3: number;
    inLFFare4: number;
    inLFFare5: number;
    inLFFare6: number;
    inLFFare7: number;
    inLFFare8: number;
    inLFFare9: number;
    inLFFare10: number;
    inLFFare11: number;
    outRoute: string;
    outFlightTimeGrp: string;
    outDepDOW: string;
    outAgentTier: string;
    outCurrency: string;
    outLFFare1: number;
    outLFFare2: number;
    outLFFare3: number;
    outLFFare4: number;
    outLFFare5: number;
    outLFFare6: number;
    outLFFare7: number;
    outLFFare8: number;
    outLFFare9: number;
    outLFFare10: number;
    outLFFare11: number;
    
    static fromJS(data: any): SeriesDto {
        let result = new SeriesDto();
        result.init(data);
        return result;
    }

    constructor(data?: ISeriesDto) {
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
            this.inFlightTimeGrp = data['inFlightTimeGrp'];
            this.inDepDOW = data['inDepDOW'];
            this.inAgentTier = data['inAgentTier'];
            this.inCurrency = data['inCurrency'];
            this.inLFFare1 = data['inLFFare1'];
            this.inLFFare2 = data['inLFFare2'];
            this.inLFFare3 = data['inLFFare3'];
            this.inLFFare4 = data['inLFFare4'];
            this.inLFFare5 = data['inLFFare5'];
            this.inLFFare6 = data['inLFFare6'];
            this.inLFFare7 = data['inLFFare7'];
            this.inLFFare8 = data['inLFFare8'];
            this.inLFFare9 = data['inLFFare9'];
            this.inLFFare10 = data['inLFFare10'];
            this.inLFFare11 = data['inLFFare11'];
            this.outRoute = data['outRoute'];
            this.outFlightTimeGrp = data['outFlightTimeGrp'];
            this.outDepDOW = data['outDepDOW'];
            this.outAgentTier = data['outAgentTier'];
            this.outCurrency = data['outCurrency'];
            this.outLFFare1 = data['outLFFare1'];
            this.outLFFare2 = data['outLFFare2'];
            this.outLFFare3 = data['outLFFare3'];
            this.outLFFare4 = data['outLFFare4'];
            this.outLFFare5 = data['outLFFare5'];
            this.outLFFare6 = data['outLFFare6'];
            this.outLFFare7 = data['outLFFare7'];
            this.outLFFare8 = data['outLFFare8'];
            this.outLFFare9 = data['outLFFare9'];
            this.outLFFare10 = data['outLFFare10'];
            this.outLFFare11 = data['outLFFare11'];
        }
    }

    toJSON(data?: any) {
		data['marketCode'] = this.marketCode;
		data['analyst'] = this.analyst;
		data['ndoDesc'] = this.inRoute;
        data['inRoute'] = this.inRoute;
        data['inFlightTimeGrp'] = this.inFlightTimeGrp;
        data['inDepDOW'] = this.inDepDOW;
        data['inAgentTier'] = this.inAgentTier;
        data['inCurrency'] = this.inCurrency;
		data['inLFFare1'] = this.inLFFare1;
        data['inLFFare2'] = this.inLFFare2;
        data['inLFFare3'] = this.inLFFare3;
        data['inLFFare4'] = this.inLFFare4;
        data['inLFFare5'] = this.inLFFare5;
        data['inLFFare6'] = this.inLFFare6;
        data['inLFFare7'] = this.inLFFare7;
        data['inLFFare8'] = this.inLFFare8;
        data['inLFFare9'] = this.inLFFare9;
        data['inLFFare10'] = this.inLFFare10;
        data['inLFFare11'] = this.inLFFare11;
        data['outRoute'] = this.outRoute;
        data['outFlightTimeGrp'] = this.outFlightTimeGrp;
        data['outDepDOW'] = this.outDepDOW;
        data['outAgentTier'] = this.outAgentTier;
        data['outCurrency'] = this.outCurrency;
		data['outLFFare1'] = this.outLFFare1;
        data['outLFFare2'] = this.outLFFare2;
        data['outLFFare3'] = this.outLFFare3;
        data['outLFFare4'] = this.outLFFare4;
        data['outLFFare5'] = this.outLFFare5;
        data['outLFFare6'] = this.outLFFare6;
        data['outLFFare7'] = this.outLFFare7;
        data['outLFFare8'] = this.outLFFare8;
        data['outLFFare9'] = this.outLFFare9;
        data['outLFFare10'] = this.outLFFare10;
        data['outLFFare11'] = this.outLFFare11;
        
		return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new SeriesDto();
        result.init(json);
        return result;
    }
}

export interface ISeriesDto {
	marketCode: string;
	analyst: string;
    ndoDesc: string;
    inRoute: string;
    inFlightTimeGrp: string;
    inDepDOW: string;
    inAgentTier: string;
    inCurrency: string;
    inLFFare1: number;
    inLFFare2: number;
    inLFFare3: number;
    inLFFare4: number;
    inLFFare5: number;
    inLFFare6: number;
    inLFFare7: number;
    inLFFare8: number;
    inLFFare9: number;
    inLFFare10: number;
    inLFFare11: number;
    outRoute: string;
    outFlightTimeGrp: string;
    outDepDOW: string;
    outAgentTier: string;
    outCurrency: string;
    outLFFare1: number;
    outLFFare2: number;
    outLFFare3: number;
    outLFFare4: number;
    outLFFare5: number;
    outLFFare6: number;
    outLFFare7: number;
    outLFFare8: number;
    outLFFare9: number;
    outLFFare10: number;
    outLFFare11: number;
}

export class PagedResultDtoOfSeriesDto implements IPagedResultDtoOfSeriesDto {
    totalCount: number;
    items: SeriesDto[];

    static fromJS(data: any): PagedResultDtoOfSeriesDto {
        let result = new PagedResultDtoOfSeriesDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfSeriesDto) {
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
                    this.items.push(SeriesDto.fromJS(item));
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
        let result = new PagedResultDtoOfSeriesDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfSeriesDto {
    totalCount: number;
    items: SeriesDto[];
}
