import { Observable } from 'rxjs/Observable';

export class PermissionSetDto implements IPermissionSetDto {
    moduleID: number;
    moduleName: string;
    expanded: boolean;
    functions: Array<IPermissionFunctionDto>;

    static fromJS(data: any): PermissionSetDto {
        let result = new PermissionSetDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPermissionSetDto) {
        if (data) {
            for (let property in data) {
                if (data.hasOwnProperty(property)) {
                    (<any>this)[property] = (<any>data)[property];
                }
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.moduleID = data['moduleID'];
            this.moduleName = data['moduleName'];
            this.expanded = data['expanded'];
            this.functions = data['functions'];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data['moduleID'] = this.moduleID;
        data['moduleName'] = this.moduleName;
        data['expanded'] = this.expanded;
        data['functions'] = this.functions;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new PermissionSetDto();
        result.init(json);
        return result;
    }
}

export interface IPermissionSetDto {
    moduleID: number;
    moduleName: string;
    expanded: boolean;
    functions: Array<IPermissionFunctionDto>;
}

export class IPermissionFunctionDto {
    compositeKey: string;
    accessCode: string;
    groupName: string;
    moduleID: number;
    moduleName: string;
    functionID: number;
    functionName: string;
    allowNew: number;
    allowEdit: number;
    allowDel: number;
    allowPrt: number;
    allowPro: number;
    isDeny: number;
    accessLevel: number;
}

export class PagedResultDtoOfPermissionSetDto implements IPagedResultDtoOfPermissionSetDto {
    totalCount: number;
    items: PermissionSetDto[];

    static fromJS(data: any): PagedResultDtoOfPermissionSetDto {
        let result = new PagedResultDtoOfPermissionSetDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfPermissionSetDto) {
        if (data) {
            for (let property in data) {
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
                    this.items.push(PermissionSetDto.fromJS(item));
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
        let result = new PagedResultDtoOfPermissionSetDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfPermissionSetDto {
    totalCount: number;
    items: PermissionSetDto[];
}
