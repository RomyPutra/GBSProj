import { state } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { strictEqual } from 'assert';

export class SeasonalityDto implements ISeasonalityDto {
	analyst: string;
	routeCode: string;
	seasonDate: Date;
	season: string;
    
    static fromJS(data: any): SeasonalityDto {
        let result = new SeasonalityDto();
        result.init(data);
        return result;
    }

    constructor(data?: ISeasonalityDto) {
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
			this.routeCode = data['routeCode'];
			this.seasonDate = data['seasonDate'];
			this.season = data['season'];
        }
    }

    toJSON(data?: any) {
		data['analyst'] = this.analyst;
		data['routeCode'] = this.routeCode;
		data['seasonDate'] = this.seasonDate;
		data['season'] = this.season;
        
		return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new SeasonalityDto();
        result.init(json);
        return result;
    }
}

export interface ISeasonalityDto {
	analyst: string;
	routeCode: string;
	seasonDate: Date;
	season: string;
}

export class PagedResultDtoOfSeasonalityDto implements IPagedResultDtoOfSeasonalityDto {
    totalCount: number;
    items: SeasonalityDto[];

    static fromJS(data: any): PagedResultDtoOfSeasonalityDto {
        let result = new PagedResultDtoOfSeasonalityDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfSeasonalityDto) {
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
                    this.items.push(SeasonalityDto.fromJS(item));
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
        let result = new PagedResultDtoOfSeasonalityDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfSeasonalityDto {
    totalCount: number;
    items: SeasonalityDto[];
}
