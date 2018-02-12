import { Observable } from 'rxjs/Observable';

export class RestrictionDto implements IRestrictionDto{
    status: boolean;
    bookfrom: Date;
    bookto: Date;
    travelfrom: Date;
    travelto: Date;
    restrictionnote: string;
    restrictionalert: string;

    static fromJS(data: any): RestrictionDto {
        let result = new RestrictionDto();
        result.init(data);
        return result;
    }

    constructor(data?: IRestrictionDto) {
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
            this.status = data["status"];
            this.bookfrom = data["bookfrom"];
            this.bookto = data["bookto"];
            this.travelfrom = data["travelfrom"];
            this.travelto = data["travelto"];
            this.restrictionnote = data["restrictionnote"];
            this.restrictionalert = data["restrictionalert"];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["status"] = this.status;
        data["bookfrom"] = this.bookfrom;
        data["bookto"] = this.bookto;
        data["travelfrom"] = this.travelfrom;
        data["travelto"] = this.travelto;
        data["restrictionnote"] = this.restrictionnote;
        data["restrictionalert"] = this.restrictionalert;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new RestrictionDto();
        result.init(json);
        return result;
    }
}

export interface IRestrictionDto {
    status: boolean;
    bookfrom: Date;
    bookto: Date;
    travelfrom: Date;
    travelto: Date;
    restrictionnote: string;
    restrictionalert: string;
}

export class PagedResultDtoOfRestrictionDto implements IPagedResultDtoOfRestrictionDto {
    totalCount: number;
    items: RestrictionDto[];

    static fromJS(data: any): PagedResultDtoOfRestrictionDto {
        let result = new PagedResultDtoOfRestrictionDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfRestrictionDto) {
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
                    this.items.push(RestrictionDto.fromJS(item));
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
        let result = new PagedResultDtoOfRestrictionDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfRestrictionDto {
    totalCount: number;
    items: RestrictionDto[];
}

