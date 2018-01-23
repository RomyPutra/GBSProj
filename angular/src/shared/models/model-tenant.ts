import { Observable } from 'rxjs/Observable';

export class TenantLoginInfoDto implements ITenantLoginInfoDto {
    tenancyName: string;
    name: string;
    id: number;

    static fromJS(data: any): TenantLoginInfoDto {
        let result = new TenantLoginInfoDto();
        result.init(data);
        return result;
    }

    constructor(data?: ITenantLoginInfoDto) {
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
            this.tenancyName = data["tenancyName"];
            this.name = data["name"];
            this.id = data["id"];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["tenancyName"] = this.tenancyName;
        data["name"] = this.name;
        data["id"] = this.id;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new TenantLoginInfoDto();
        result.init(json);
        return result;
    }
}

export interface ITenantLoginInfoDto {
    tenancyName: string;
    name: string;
    id: number;
}

export class CreateTenantDto implements ICreateTenantDto {
    tenancyName: string;
    name: string;
    adminEmailAddress: string;
    connectionString: string;
    isActive: boolean;

    static fromJS(data: any): CreateTenantDto {
        let result = new CreateTenantDto();
        result.init(data);
        return result;
    }

    constructor(data?: ICreateTenantDto) {
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
            this.tenancyName = data["tenancyName"];
            this.name = data["name"];
            this.adminEmailAddress = data["adminEmailAddress"];
            this.connectionString = data["connectionString"];
            this.isActive = data["isActive"];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["tenancyName"] = this.tenancyName;
        data["name"] = this.name;
        data["adminEmailAddress"] = this.adminEmailAddress;
        data["connectionString"] = this.connectionString;
        data["isActive"] = this.isActive;
        return data; 
    }

    clone() {
        const json = this.toJSON();
        let result = new CreateTenantDto();
        result.init(json);
        return result;
    }
}

export interface ICreateTenantDto {
    tenancyName: string;
    name: string;
    adminEmailAddress: string;
    connectionString: string;
    isActive: boolean;
}

export class TenantDto implements ITenantDto {
    tenancyName: string;
    name: string;
    isActive: boolean;
    id: number;

    static fromJS(data: any): TenantDto {
        let result = new TenantDto();
        result.init(data);
        return result;
    }

    constructor(data?: ITenantDto) {
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
            this.tenancyName = data["tenancyName"];
            this.name = data["name"];
            this.isActive = data["isActive"];
            this.id = data["id"];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["tenancyName"] = this.tenancyName;
        data["name"] = this.name;
        data["isActive"] = this.isActive;
        data["id"] = this.id;
        return data; 
    }

    clone() {
        const json = this.toJSON();
        let result = new TenantDto();
        result.init(json);
        return result;
    }
}

export interface ITenantDto {
    tenancyName: string;
    name: string;
    isActive: boolean;
    id: number;
}

export class PagedResultDtoOfTenantDto implements IPagedResultDtoOfTenantDto {
    totalCount: number;
    items: TenantDto[];

    static fromJS(data: any): PagedResultDtoOfTenantDto {
        let result = new PagedResultDtoOfTenantDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfTenantDto) {
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
                    this.items.push(TenantDto.fromJS(item));
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
        let result = new PagedResultDtoOfTenantDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfTenantDto {
    totalCount: number;
    items: TenantDto[];
}

export class IsTenantAvailableInput implements IIsTenantAvailableInput {
    tenancyName: string;

    static fromJS(data: any): IsTenantAvailableInput {
        let result = new IsTenantAvailableInput();
        result.init(data);
        return result;
    }

    constructor(data?: IIsTenantAvailableInput) {
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
            this.tenancyName = data["tenancyName"];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["tenancyName"] = this.tenancyName;
        return data; 
    }

    clone() {
        const json = this.toJSON();
        let result = new IsTenantAvailableInput();
        result.init(json);
        return result;
    }
}

export interface IIsTenantAvailableInput {
    tenancyName: string;
}

export class IsTenantAvailableOutput implements IIsTenantAvailableOutput {
    state: IsTenantAvailableOutputState;
    tenantId: number;

    static fromJS(data: any): IsTenantAvailableOutput {
        let result = new IsTenantAvailableOutput();
        result.init(data);
        return result;
    }

    constructor(data?: IIsTenantAvailableOutput) {
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
            this.state = data["state"];
            this.tenantId = data["tenantId"];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["state"] = this.state;
        data["tenantId"] = this.tenantId;
        return data; 
    }

    clone() {
        const json = this.toJSON();
        let result = new IsTenantAvailableOutput();
        result.init(json);
        return result;
    }
}

export interface IIsTenantAvailableOutput {
    state: IsTenantAvailableOutputState;
    tenantId: number;
}

export enum IsTenantAvailableOutputState {
    _1 = 1,
    _2 = 2,
    _3 = 3,
}
