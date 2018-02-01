/* tslint:disable */

export class ListResultDtoOfSysFunctionDto implements IListResultDtoOfSysFunctionDto {
    items: SysFunctionDto[];

    constructor(data?: IListResultDtoOfSysFunctionDto) {
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
                    this.items.push(SysFunctionDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): ListResultDtoOfSysFunctionDto {
        let result = new ListResultDtoOfSysFunctionDto();
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
        let result = new ListResultDtoOfSysFunctionDto();
        result.init(json);
        return result;
    }
}

export interface IListResultDtoOfSysFunctionDto {
    items: SysFunctionDto[];
}

export class SysFunctionDto implements ISysFunctionDto {
    functionID: number;
    functionCode: string;
    functionName: string;

    constructor(data?: ISysFunctionDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.functionCode = data["functionCode"];
            this.functionName = data["functionName"];
            this.functionID = data["functionID"];
        }
    }

    static fromJS(data: any): SysFunctionDto {
        let result = new SysFunctionDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["functionCode"] = this.functionCode;
        data["functionName"] = this.functionName;
        data["functionID"] = this.functionID;
        return data; 
    }

    clone() {
        const json = this.toJSON();
        let result = new SysFunctionDto();
        result.init(json);
        return result;
    }
}

export interface ISysFunctionDto {
    functionID: number;
    functionCode: string;
    functionName: string;
}