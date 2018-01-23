import { Observable } from 'rxjs/Observable';

export class CreateRoleDto implements ICreateRoleDto {
    name: string;
    displayName: string;
    normalizedName: string;
    description: string;
    isStatic: boolean;
    permissions: string[];

    static fromJS(data: any): CreateRoleDto {
        let result = new CreateRoleDto();
        result.init(data);
        return result;
    }

    constructor(data?: ICreateRoleDto) {
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
            this.name = data["name"];
            this.displayName = data["displayName"];
            this.normalizedName = data["normalizedName"];
            this.description = data["description"];
            this.isStatic = data["isStatic"];
            if (data["permissions"] && data["permissions"].constructor === Array) {
                this.permissions = [];
                for (let item of data["permissions"]) {
                    this.permissions.push(item);
                }
            }
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["name"] = this.name;
        data["displayName"] = this.displayName;
        data["normalizedName"] = this.normalizedName;
        data["description"] = this.description;
        data["isStatic"] = this.isStatic;
        if (this.permissions && this.permissions.constructor === Array) {
            data["permissions"] = [];
            for (let item of this.permissions)
                data["permissions"].push(item);
        }
        return data; 
    }

    clone() {
        const json = this.toJSON();
        let result = new CreateRoleDto();
        result.init(json);
        return result;
    }
}

export interface ICreateRoleDto {
    name: string;
    displayName: string;
    normalizedName: string;
    description: string;
    isStatic: boolean;
    permissions: string[];
}

export class RoleDto implements IRoleDto {
    name: string;
    displayName: string;
    normalizedName: string;
    description: string;
    isStatic: boolean;
    permissions: string[];
    id: number;

    constructor(data?: IRoleDto) {
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
            this.normalizedName = data["normalizedName"];
            this.description = data["description"];
            this.isStatic = data["isStatic"];
            if (data["permissions"] && data["permissions"].constructor === Array) {
                this.permissions = [];
                for (let item of data["permissions"])
                    this.permissions.push(item);
            }
            this.id = data["id"];
        }
    }

    static fromJS(data: any): RoleDto {
        let result = new RoleDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["name"] = this.name;
        data["displayName"] = this.displayName;
        data["normalizedName"] = this.normalizedName;
        data["description"] = this.description;
        data["isStatic"] = this.isStatic;
        if (this.permissions && this.permissions.constructor === Array) {
            data["permissions"] = [];
            for (let item of this.permissions)
                data["permissions"].push(item);
        }
        data["id"] = this.id;
        return data; 
    }

    clone() {
        const json = this.toJSON();
        let result = new RoleDto();
        result.init(json);
        return result;
    }
}

export interface IRoleDto {
    name: string;
    displayName: string;
    normalizedName: string;
    description: string;
    isStatic: boolean;
    permissions: string[];
    id: number;
}

export class PagedResultDtoOfRoleDto implements IPagedResultDtoOfRoleDto {
    totalCount: number;
    items: RoleDto[];

    constructor(data?: IPagedResultDtoOfRoleDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.totalCount = data["totalCount"];
            if (data["items"] && data["items"].constructor === Array) {
                this.items = [];
                for (let item of data["items"])
                    this.items.push(RoleDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfRoleDto {
        let result = new PagedResultDtoOfRoleDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["totalCount"] = this.totalCount;
        if (this.items && this.items.constructor === Array) {
            data["items"] = [];
            for (let item of this.items)
                data["items"].push(item.toJSON());
        }
        return data; 
    }

    clone() {
        const json = this.toJSON();
        let result = new PagedResultDtoOfRoleDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfRoleDto {
    totalCount: number;
    items: RoleDto[];
}

export class ListResultDtoOfRoleDto implements IListResultDtoOfRoleDto {
    items: RoleDto[];

    constructor(data?: IListResultDtoOfRoleDto) {
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
                    this.items.push(RoleDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): ListResultDtoOfRoleDto {
        let result = new ListResultDtoOfRoleDto();
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
        let result = new ListResultDtoOfRoleDto();
        result.init(json);
        return result;
    }
}

export interface IListResultDtoOfRoleDto {
    items: RoleDto[];
}
