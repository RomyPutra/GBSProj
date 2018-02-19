import { Observable } from 'rxjs/Observable';

export class UnitDto implements IUnitDto {
    uomCode: string;
    uomDesc: string;
    uomNumber: string;

    static fromJS(data: any): UnitDto {
        let result = new UnitDto();
        result.init(data);
        return result;
    }

    constructor(data?: IUnitDto) {
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
            this.uomCode = data['uomCode'];
            this.uomDesc = data['uomDesc'];
            this.uomNumber = data['uomNumber'];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data['uomCode'] = this.uomCode;
        data['uomDesc'] = this.uomDesc;
        data['uomNumber'] = this.uomNumber;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new UnitDto();
        result.init(json);
        return result;
    }
}

export interface IUnitDto {
    uomCode: string;
    uomDesc: string;
    uomNumber: string;
    // name: string;
    // displayName: string;
    // normalizedName: string;
    // description: string;
    // isStatic: boolean;
    // permissions: string[];
    // id: number;
}

export class PagedResultDtoOfUnitDto implements IPagedResultDtoOfUnitDto {
    totalCount: number;
    items: UnitDto[];

    static fromJS(data: any): PagedResultDtoOfUnitDto {
        let result = new PagedResultDtoOfUnitDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfUnitDto) {
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
                    this.items.push(UnitDto.fromJS(item));
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
        let result = new PagedResultDtoOfUnitDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfUnitDto {
    totalCount: number;
    items: UnitDto[];
}
