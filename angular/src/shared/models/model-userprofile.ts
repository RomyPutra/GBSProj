import { Observable } from 'rxjs/Observable';

export class UserProfileDto implements IUserProfileDto {
    userID: string;
    userName: string;
    password: string;
    lastLogin: Date;
    refID: string;
    status: number;
    statusDesc: string;
    accessCode: string;
    accessDesc: string;

    static fromJS(data: any): UserProfileDto {
        let result = new UserProfileDto();
        result.init(data);
        return result;
    }

    constructor(data?: IUserProfileDto) {
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
            this.userID = data['userID'];
            this.userName = data['userName'];
            this.password = data['password'];
            this.lastLogin = data['lastLogin'];
            this.refID = data['refID'];
            this.status = data['status'];
            this.statusDesc = data['statusDesc'];
            this.accessCode = data['accessCode'];
            this.accessDesc = data['accessDesc'];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data['userID'] = this.userID;
        data['userName'] = this.userName;
        data['password'] = this.password;
        data['lastLogin'] = this.lastLogin;
        data['refID'] = this.refID;
        data['status'] = this.status;
        data['statusDesc'] = this.statusDesc;
        data['accessCode'] = this.accessCode;
        data['accessDesc'] = this.accessDesc;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new UserProfileDto();
        result.init(json);
        return result;
    }
}

export interface IUserProfileDto {
    userID: string;
    userName: string;
    password: string;
    lastLogin: Date;
    refID: string;
    status: number;
    statusDesc: string;
    accessCode: string;
    accessDesc: string;
}

export class PagedResultDtoOfUserProfileDto implements IPagedResultDtoOfUserProfileDto {
    totalCount: number;
    items: UserProfileDto[];

    static fromJS(data: any): PagedResultDtoOfUserProfileDto {
        let result = new PagedResultDtoOfUserProfileDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfUserProfileDto) {
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
            this.totalCount = data['totalCount'];
            if (data['items'] && data['items'].constructor === Array) {
                this.items = [];
                for (let item of data['items']) {
                    this.items.push(UserProfileDto.fromJS(item));
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
        let result = new PagedResultDtoOfUserProfileDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfUserProfileDto {
    totalCount: number;
    items: UserProfileDto[];
}
