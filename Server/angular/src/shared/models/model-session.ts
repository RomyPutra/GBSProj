import { Observable } from 'rxjs/Observable';
import { TenantLoginInfoDto } from './model-tenant';

import * as moment from 'moment';
 
export class GetCurrentLoginInformationsOutput implements IGetCurrentLoginInformationsOutput {
    application: ApplicationInfoDto;
    user: UserLoginInfoDto;
    tenant: TenantLoginInfoDto;

    constructor(data?: IGetCurrentLoginInformationsOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.application = data["application"] ? ApplicationInfoDto.fromJS(data["application"]) : <any>undefined;
            this.user = data["user"] ? UserLoginInfoDto.fromJS(data["user"]) : <any>undefined;
            this.tenant = data["tenant"] ? TenantLoginInfoDto.fromJS(data["tenant"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetCurrentLoginInformationsOutput {
        let result = new GetCurrentLoginInformationsOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["application"] = this.application ? this.application.toJSON() : <any>undefined;
        data["user"] = this.user ? this.user.toJSON() : <any>undefined;
        data["tenant"] = this.tenant ? this.tenant.toJSON() : <any>undefined;
        return data; 
    }

    clone() {
        const json = this.toJSON();
        let result = new GetCurrentLoginInformationsOutput();
        result.init(json);
        return result;
    }
}

export interface IGetCurrentLoginInformationsOutput {
    application: ApplicationInfoDto;
    user: UserLoginInfoDto;
    tenant: TenantLoginInfoDto;
}

export class ApplicationInfoDto implements IApplicationInfoDto {
    version: string;
    releaseDate: moment.Moment;
    features: { [key: string] : boolean; };

    constructor(data?: IApplicationInfoDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.version = data["version"];
            this.releaseDate = data["releaseDate"] ? moment(data["releaseDate"].toString()) : <any>undefined;
            if (data["features"]) {
                this.features = {};
                for (let key in data["features"]) {
                    if (data["features"].hasOwnProperty(key))
                        this.features[key] = data["features"][key];
                }
            }
        }
    }

    static fromJS(data: any): ApplicationInfoDto {
        let result = new ApplicationInfoDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["version"] = this.version;
        data["releaseDate"] = this.releaseDate ? this.releaseDate.toISOString() : <any>undefined;
        if (this.features) {
            data["features"] = {};
            for (let key in this.features) {
                if (this.features.hasOwnProperty(key))
                    data["features"][key] = this.features[key];
            }
        }
        return data; 
    }

    clone() {
        const json = this.toJSON();
        let result = new ApplicationInfoDto();
        result.init(json);
        return result;
    }
}

export interface IApplicationInfoDto {
    version: string;
    releaseDate: moment.Moment;
    features: { [key: string] : boolean; };
}

export class UserLoginInfoDto implements IUserLoginInfoDto {
    name: string;
    surname: string;
    userName: string;
    emailAddress: string;
    id: number;

    constructor(data?: IUserLoginInfoDto) {
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
            this.surname = data["surname"];
            this.userName = data["userName"];
            this.emailAddress = data["emailAddress"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): UserLoginInfoDto {
        let result = new UserLoginInfoDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["name"] = this.name;
        data["surname"] = this.surname;
        data["userName"] = this.userName;
        data["emailAddress"] = this.emailAddress;
        data["id"] = this.id;
        return data; 
    }

    clone() {
        const json = this.toJSON();
        let result = new UserLoginInfoDto();
        result.init(json);
        return result;
    }
}

export interface IUserLoginInfoDto {
    name: string;
    surname: string;
    userName: string;
    emailAddress: string;
    id: number;
}