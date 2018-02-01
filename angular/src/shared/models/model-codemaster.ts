import { Observable } from 'rxjs/Observable';

export interface ICodeMasterDto {
    code: string;
    codeDesc: string;
}

export class CodeMasterDto implements ICodeMasterDto {
    code: string;
    codeDesc: string;

    static fromJS(data: any): CodeMasterDto {
        let result = new CodeMasterDto();
        result.init(data);
        return result;
    }

    constructor(data?: ICodeMasterDto) {
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
            this.code = data['code'];
            this.codeDesc = data['codeDesc'];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data['code'] = this.code;
        data['codeDesc'] = this.codeDesc;

        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new CodeMasterDto();
        result.init(json);
        return result;
    }
}

export class PagedResultDtoOfCodeMasterDto implements IPagedResultDtoOfCodeMasterDto {
    totalCount: number;
    items: CodeMasterDto[];

    static fromJS(data: any): PagedResultDtoOfCodeMasterDto {
        let result = new PagedResultDtoOfCodeMasterDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfCodeMasterDto) {
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
                    this.items.push(CodeMasterDto.fromJS(item));
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
        let result = new PagedResultDtoOfCodeMasterDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfCodeMasterDto {
    totalCount: number;
    items: CodeMasterDto[];
}
