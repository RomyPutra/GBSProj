import { Observable } from 'rxjs/Observable';

export class StockDto implements IStockDto {
    docCode: string;
    itemCode: string;
    itemDesc: string;
    itmBrand: string;
    itmCatgCode: string;
    itmCatgDesc: string;
    sellPrice: number;
    active: number;
    updateBy: string;
    createDate: Date;
    lastUpdate: Date;

    static fromJS(data: any): StockDto {
        let result = new StockDto();
        result.init(data);
        return result;
    }

    constructor(data?: IStockDto) {
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
            this.itemCode = data['itemCode'];
            this.itemDesc = data['itemDesc'];
            this.itmBrand = data['itmBrand'];
            this.itmCatgCode = data['itmCatgCode'];
            this.itmCatgDesc = data['itmCatgDesc'];
            this.sellPrice = data['sellPrice'];
            this.active = data['active'];
            this.updateBy = data['updateBy'];
            this.createDate = data['createDate'];
            this.lastUpdate = data['lastUpdate'];
            this.docCode = data['docCode'];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data['itemCode'] = this.itemCode  ;
        data['itemDesc'] = this.itemDesc;
        data['itmBrand'] = this.itmBrand;
        data['itmCatgCode'] = this.itmCatgCode;
        data['itmCatgDesc'] = this.itmCatgDesc;
        data['sellPrice'] = this.sellPrice ;
        data['active'] = this.active;
        data['updateBy'] = this.updateBy;
        data['createDate'] = this.createDate;
        data['lastUpdate'] = this.lastUpdate;
        data['docCode'] = this.docCode;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new StockDto();
        result.init(json);
        return result;
    }
}

export interface IStockDto {
    docCode: string;
    itemCode: string;
    itemDesc: string;
    itmBrand: string;
    itmCatgCode: string;
    itmCatgDesc: string;
    sellPrice: number;
    active: number;
    updateBy: string;
    createDate: Date;
    lastUpdate: Date;
}

export class PagedResultDtoOfStockDto implements IPagedResultDtoOfStockDto {
    totalCount: number;
    items: StockDto[];

    static fromJS(data: any): PagedResultDtoOfStockDto {
        let result = new PagedResultDtoOfStockDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfStockDto) {
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
                    this.items.push(StockDto.fromJS(item));
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
        let result = new PagedResultDtoOfStockDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfStockDto {
    totalCount: number;
    items: StockDto[];
}
