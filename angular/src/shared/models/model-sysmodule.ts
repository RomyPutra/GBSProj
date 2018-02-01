/* tslint:disable */

export class ListResultDtoOfSysModuleDto implements IListResultDtoOfSysModuleDto {
    items: SysModuleDto[];

    constructor(data?: IListResultDtoOfSysModuleDto) {
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
            if (data["items"] && data["items"].constructor === Array) {
                this.items = [];
                for (let item of data["items"])
                    this.items.push(SysModuleDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): ListResultDtoOfSysModuleDto {
        let result = new ListResultDtoOfSysModuleDto();
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
        let result = new ListResultDtoOfSysModuleDto();
        result.init(json);
        return result;
    }
}

export interface IListResultDtoOfSysModuleDto {
    items: SysModuleDto[];
}

export class SysModuleDto implements ISysModuleDto {
    moduleID: number;
    moduleCode: string;
    moduleName: string;

    constructor(data?: ISysModuleDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.moduleCode = data["moduleCode"];
            this.moduleName = data["moduleName"];
            this.moduleID = data["moduleID"];
        }
    }

    static fromJS(data: any): SysModuleDto {
        let result = new SysModuleDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["moduleCode"] = this.moduleCode;
        data["moduleName"] = this.moduleName;
        data["moduleID"] = this.moduleID;
        return data; 
    }

    clone() {
        const json = this.toJSON();
        let result = new SysModuleDto();
        result.init(json);
        return result;
    }
}

export interface ISysModuleDto {
    moduleID: number;
    moduleCode: string;
    moduleName: string;
}