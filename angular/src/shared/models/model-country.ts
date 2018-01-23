import { Observable } from 'rxjs/Observable';

export class CountryDto implements ICountryDto {
    countryCode : string
    countryDesc : string
    active : string
    createBy : string
    createDate : string
    

    static fromJS(data: any): CountryDto {
        let result = new CountryDto();
        result.init(data);
        return result;
    }

    constructor(data?: ICountryDto) {
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
            this.countryCode = data['countryCode'];
            this.countryDesc = data['countryDesc'];
            this.active = data['active'];
            this.createBy = data['createBy'];
            this.createDate = data['createDate'];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data['countryCode'] = this.countryCode;
        data['countryDesc'] =this.countryDesc; 
        data['active'] = this.active;
        data['createBy'] = this.createBy; 
        data['createDate'] = this.createDate;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new CountryDto();
        result.init(json);
        return result;
    }
}

export interface ICountryDto {
    countryCode : string
    countryDesc : string
    active : string
    createBy : string
    createDate : string
}

export class PagedResultDtoOfCountryDto implements IPagedResultDtoOfCountryDto {
    totalCount: number;
    items: CountryDto[];

    static fromJS(data: any): PagedResultDtoOfCountryDto {
        let result = new PagedResultDtoOfCountryDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfCountryDto) {
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
                    this.items.push(CountryDto.fromJS(item));
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
        let result = new PagedResultDtoOfCountryDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfCountryDto {
    totalCount: number;
    items: CountryDto[];
}
