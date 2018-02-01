import { Observable } from 'rxjs/Observable';

export class ItemCategoryDto implements IItemCategoryDto {
    catgCode: string;
    catgDesc: string;

    static fromJS(data: any): ItemCategoryDto {
        let result = new ItemCategoryDto();
        result.init(data);
        return result;
    }

    constructor(data?: IItemCategoryDto) {
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
            this.catgCode = data['catgCode'];
            this.catgDesc = data['catgDesc'];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data['catgCode'] = this.catgCode  ;
        data['catgDesc'] = this.catgDesc;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new ItemCategoryDto();
        result.init(json);
        return result;
    }
}

export interface IItemCategoryDto {
    catgCode: string;
    catgDesc: string;
}

export class PagedResultDtoOfItemCategoryDto implements IPagedResultDtoOfItemCategoryDto {
    totalCount: number;
    items: ItemCategoryDto[];

    static fromJS(data: any): PagedResultDtoOfItemCategoryDto {
        let result = new PagedResultDtoOfItemCategoryDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfItemCategoryDto) {
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
                    this.items.push(ItemCategoryDto.fromJS(item));
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
        let result = new PagedResultDtoOfItemCategoryDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfItemCategoryDto {
    totalCount: number;
    items: ItemCategoryDto[];
}
