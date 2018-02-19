/* tslint:disable */

import { Observable } from 'rxjs/Observable';

export class ListResultDtoOfPermissionDto implements IListResultDtoOfPermissionDto {
    items: PermissionDto[];

    constructor(data?: IListResultDtoOfPermissionDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            if (data["items"] && data["items"].constructor === Array) {
                this.items = [];
                for (let item of data["items"])
                    this.items.push(PermissionDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): ListResultDtoOfPermissionDto {
        let result = new ListResultDtoOfPermissionDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        if (this.items && this.items.constructor === Array) {
            data["items"] = [];
            for (let item of this.items)
                data["items"].push(item.toJSON());
        }
        return data; 
    }

    clone() {
        const json = this.toJSON();
        let result = new ListResultDtoOfPermissionDto();
        result.init(json);
        return result;
    }
}

export interface IListResultDtoOfPermissionDto {
    items: PermissionDto[];
}

export class PermissionDto implements IPermissionDto {
    name: string;
    displayName: string;
    description: string;
    id: number;

    constructor(data?: IPermissionDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.name = data["name"];
            this.displayName = data["displayName"];
            this.description = data["description"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): PermissionDto {
        let result = new PermissionDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["name"] = this.name;
        data["displayName"] = this.displayName;
        data["description"] = this.description;
        data["id"] = this.id;
        return data; 
    }

    clone() {
        const json = this.toJSON();
        let result = new PermissionDto();
        result.init(json);
        return result;
    }
}

export interface IPermissionDto {
    name: string;
    displayName: string;
    description: string;
    id: number;
}