import { Observable } from 'rxjs/Observable';

export class ChangeUiThemeInput implements IChangeUiThemeInput {
    theme: string;

    static fromJS(data: any): ChangeUiThemeInput {
        let result = new ChangeUiThemeInput();
        result.init(data);
        return result;
    }

    constructor(data?: IChangeUiThemeInput) {
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
            this.theme = data["theme"];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["theme"] = this.theme;
        return data; 
    }

    clone() {
        const json = this.toJSON();
        let result = new ChangeUiThemeInput();
        result.init(json);
        return result;
    }
}

export interface IChangeUiThemeInput {
    theme: string;
}
