import { Observable } from 'rxjs/Observable';

import * as moment from 'moment';

export class CreateUserDto implements ICreateUserDto {
    userName: string;
    name: string;
    surname: string;
    emailAddress: string;
    isActive: boolean;
    roleNames: string[];
    password: string;

    constructor(data?: ICreateUserDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.userName = data["userName"];
            this.name = data["name"];
            this.surname = data["surname"];
            this.emailAddress = data["emailAddress"];
            this.isActive = data["isActive"];
            if (data["roleNames"] && data["roleNames"].constructor === Array) {
                this.roleNames = [];
                for (let item of data["roleNames"])
                    this.roleNames.push(item);
            }
            this.password = data["password"];
        }
    }

    static fromJS(data: any): CreateUserDto {
        let result = new CreateUserDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["userName"] = this.userName;
        data["name"] = this.name;
        data["surname"] = this.surname;
        data["emailAddress"] = this.emailAddress;
        data["isActive"] = this.isActive;
        if (this.roleNames && this.roleNames.constructor === Array) {
            data["roleNames"] = [];
            for (let item of this.roleNames)
                data["roleNames"].push(item);
        }
        data["password"] = this.password;
        return data; 
    }

    clone() {
        const json = this.toJSON();
        let result = new CreateUserDto();
        result.init(json);
        return result;
    }
}

export interface ICreateUserDto {
    userName: string;
    name: string;
    surname: string;
    emailAddress: string;
    isActive: boolean;
    roleNames: string[];
    password: string;
}

export class UserDto implements IUserDto {
    userName: string;
    name: string;
    surname: string;
    emailAddress: string;
    isActive: boolean;
    fullName: string;
    lastLoginTime: moment.Moment;
    creationTime: moment.Moment;
    roleNames: string[];
    id: number;

    constructor(data?: IUserDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.userName = data["userName"];
            this.name = data["name"];
            this.surname = data["surname"];
            this.emailAddress = data["emailAddress"];
            this.isActive = data["isActive"];
            this.fullName = data["fullName"];
            this.lastLoginTime = data["lastLoginTime"] ? moment(data["lastLoginTime"].toString()) : <any>undefined;
            this.creationTime = data["creationTime"] ? moment(data["creationTime"].toString()) : <any>undefined;
            if (data["roleNames"] && data["roleNames"].constructor === Array) {
                this.roleNames = [];
                for (let item of data["roleNames"])
                    this.roleNames.push(item);
            }
            this.id = data["id"];
        }
    }

    static fromJS(data: any): UserDto {
        let result = new UserDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["userName"] = this.userName;
        data["name"] = this.name;
        data["surname"] = this.surname;
        data["emailAddress"] = this.emailAddress;
        data["isActive"] = this.isActive;
        data["fullName"] = this.fullName;
        data["lastLoginTime"] = this.lastLoginTime ? this.lastLoginTime.toISOString() : <any>undefined;
        data["creationTime"] = this.creationTime ? this.creationTime.toISOString() : <any>undefined;
        if (this.roleNames && this.roleNames.constructor === Array) {
            data["roleNames"] = [];
            for (let item of this.roleNames)
                data["roleNames"].push(item);
        }
        data["id"] = this.id;
        return data; 
    }

    clone() {
        const json = this.toJSON();
        let result = new UserDto();
        result.init(json);
        return result;
    }
}

export interface IUserDto {
    userName: string;
    name: string;
    surname: string;
    emailAddress: string;
    isActive: boolean;
    fullName: string;
    lastLoginTime: moment.Moment;
    creationTime: moment.Moment;
    roleNames: string[];
    id: number;
}

export class ChangeUserLanguageDto implements IChangeUserLanguageDto {
    languageName: string;

    constructor(data?: IChangeUserLanguageDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.languageName = data["languageName"];
        }
    }

    static fromJS(data: any): ChangeUserLanguageDto {
        let result = new ChangeUserLanguageDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["languageName"] = this.languageName;
        return data; 
    }

    clone() {
        const json = this.toJSON();
        let result = new ChangeUserLanguageDto();
        result.init(json);
        return result;
    }
}

export interface IChangeUserLanguageDto {
    languageName: string;
}

export class PagedResultDtoOfUserDto implements IPagedResultDtoOfUserDto {
    totalCount: number;
    items: UserDto[];

    constructor(data?: IPagedResultDtoOfUserDto) {
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
                    this.items.push(UserDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfUserDto {
        let result = new PagedResultDtoOfUserDto();
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
        let result = new PagedResultDtoOfUserDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfUserDto {
    totalCount: number;
    items: UserDto[];
}