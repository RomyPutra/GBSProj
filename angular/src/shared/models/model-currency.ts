import { Observable } from 'rxjs/Observable';

export class CurrencyDto implements ICurrencyDto {
    countryCode: string;
    countryDesc: string;
    currencyCode: string;
    currencyDesc: string;
    rate: number;
    unit: number;
    currencySymbol: string;
    active: number;
    createBy: string;

    static fromJS(data: any): CurrencyDto {
        let result = new CurrencyDto();
        result.init(data);
        return result;
    }

    constructor(data?: ICurrencyDto) {
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
            this.countryCode      = data['countryCode'];
            this.countryDesc      = data['countryDesc'];
            this.currencyCode     = data['currencyCode'];
            this.currencyDesc     = data['currencyDesc'];
            this.rate             = data['rate'];
            this.unit             = data['unit'];
            this.currencySymbol   = data['currencySymbol'];
            this.active           = data['active'];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data['countryCode']     = this.countryCode;
        data['countryDesc']     = this.countryDesc;
        data['currencyCode']    = this.currencyCode;
        data['currencyDesc']    = this.currencyDesc;
        data['rate']     		= this.rate;
        data['unit']     		= this.unit;
        data['currencySymbol']  = this.currencySymbol;
        data['active']     		= this.active;
        data['createBy']     		= this.createBy;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new CurrencyDto();
        result.init(json);
        return result;
    }
}

export interface ICurrencyDto {
    countryCode: string;
    countryDesc: string;
    currencyCode: string;
    currencyDesc: string;
    rate: number;
    unit: number;
    currencySymbol: string;
    active: number;
    createBy: string;
}

export class PagedResultDtoOfCurrencyDto implements IPagedResultDtoOfCurrencyDto {
    totalCount: number;
    items: CurrencyDto[];

    static fromJS(data: any): PagedResultDtoOfCurrencyDto {
        let result = new PagedResultDtoOfCurrencyDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfCurrencyDto) {
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
            this.totalCount = data["totalCount"];
            if (data["items"] && data["items"].constructor === Array) {
                this.items = [];
                for (let item of data["items"]) {
                    this.items.push(CurrencyDto.fromJS(item));
                }
            }
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["totalCount"] = this.totalCount;
        if (this.items && this.items.constructor === Array) {
            data["items"] = [];
            for (let item of this.items) {
                data["items"].push(item.toJSON());
            }
        }
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new PagedResultDtoOfCurrencyDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfCurrencyDto {
    totalCount: number;
    items: CurrencyDto[];
}
