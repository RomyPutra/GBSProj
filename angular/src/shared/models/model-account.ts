import { Observable } from 'rxjs/Observable';

export class RegisterInput implements IRegisterInput {
    name: string;
    surname: string;
    userName: string;
    emailAddress: string;
    password: string;
    captchaResponse: string;

    static fromJS(data: any): RegisterInput {
        let result = new RegisterInput();
        result.init(data);
        return result;
    }

    constructor(data?: IRegisterInput) {
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
            this.surname = data["surname"];
            this.userName = data["userName"];
            this.emailAddress = data["emailAddress"];
            this.password = data["password"];
            this.captchaResponse = data["captchaResponse"];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["name"] = this.name;
        data["surname"] = this.surname;
        data["userName"] = this.userName;
        data["emailAddress"] = this.emailAddress;
        data["password"] = this.password;
        data["captchaResponse"] = this.captchaResponse;
        return data; 
    }

    clone() {
        const json = this.toJSON();
        let result = new RegisterInput();
        result.init(json);
        return result;
    }
}

export interface IRegisterInput {
    name: string;
    surname: string;
    userName: string;
    emailAddress: string;
    password: string;
    captchaResponse: string;
}

export class RegisterOutput implements IRegisterOutput {
    canLogin: boolean;

    static fromJS(data: any): RegisterOutput {
        let result = new RegisterOutput();
        result.init(data);
        return result;
    }

    constructor(data?: IRegisterOutput) {
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
            this.canLogin = data["canLogin"];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["canLogin"] = this.canLogin;
        return data; 
    }

    clone() {
        const json = this.toJSON();
        let result = new RegisterOutput();
        result.init(json);
        return result;
    }
}

export interface IRegisterOutput {
    canLogin: boolean;
}

export class AuthenticateModel implements IAuthenticateModel {
    userName: string;
    password: string;
    rememberClient: boolean;


    constructor(data?: IAuthenticateModel) {
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
            this.password = data["password"];
            this.rememberClient = data["rememberClient"];
        }
    }

    static fromJS(data: any): AuthenticateModel {
        let result = new AuthenticateModel();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["userName"] = this.userName;
        data["password"] = this.password;
        data["rememberClient"] = this.rememberClient;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new AuthenticateModel();
        result.init(json);
        return result;
    }
}

export interface IAuthenticateModel {
    userName: string;
    password: string;
    rememberClient: boolean;
}

export class AuthenticateResultModel implements IAuthenticateResultModel {
    accessToken: string;
    encryptedAccessToken: string;
    expireInSeconds: number;
    userId: string;
    groupName: string;

    constructor(data?: IAuthenticateResultModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.accessToken = data["accessToken"];
            this.encryptedAccessToken = data["encryptedAccessToken"];
            this.expireInSeconds = data["expireInSeconds"];
            this.userId = data["userId"];
            this.groupName = data["groupName"];
        }
    }

    static fromJS(data: any): AuthenticateResultModel {
        let result = new AuthenticateResultModel();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["accessToken"] = this.accessToken;
        data["encryptedAccessToken"] = this.encryptedAccessToken;
        data["expireInSeconds"] = this.expireInSeconds;
        data["userId"] = this.userId;
        data["groupName"] = this.groupName;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new AuthenticateResultModel();
        result.init(json);
        return result;
    }
}

export interface IAuthenticateResultModel {
    accessToken: string;
    encryptedAccessToken: string;
    expireInSeconds: number;
    userId: string;
    groupName: string;
}

export class ExternalLoginProviderInfoModel implements IExternalLoginProviderInfoModel {
    name: string;
    clientId: string;

    constructor(data?: IExternalLoginProviderInfoModel) {
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
            this.clientId = data["clientId"];
        }
    }

    static fromJS(data: any): ExternalLoginProviderInfoModel {
        let result = new ExternalLoginProviderInfoModel();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["name"] = this.name;
        data["clientId"] = this.clientId;
        return data; 
    }

    clone() {
        const json = this.toJSON();
        let result = new ExternalLoginProviderInfoModel();
        result.init(json);
        return result;
    }
}

export interface IExternalLoginProviderInfoModel {
    name: string;
    clientId: string;
}

export class ExternalAuthenticateModel implements IExternalAuthenticateModel {
    authProvider: string;
    providerKey: string;
    providerAccessCode: string;

    constructor(data?: IExternalAuthenticateModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.authProvider = data["authProvider"];
            this.providerKey = data["providerKey"];
            this.providerAccessCode = data["providerAccessCode"];
        }
    }

    static fromJS(data: any): ExternalAuthenticateModel {
        let result = new ExternalAuthenticateModel();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["authProvider"] = this.authProvider;
        data["providerKey"] = this.providerKey;
        data["providerAccessCode"] = this.providerAccessCode;
        return data; 
    }

    clone() {
        const json = this.toJSON();
        let result = new ExternalAuthenticateModel();
        result.init(json);
        return result;
    }
}

export interface IExternalAuthenticateModel {
    authProvider: string;
    providerKey: string;
    providerAccessCode: string;
}

export class ExternalAuthenticateResultModel implements IExternalAuthenticateResultModel {
    accessToken: string;
    encryptedAccessToken: string;
    expireInSeconds: number;
    waitingForActivation: boolean;

    constructor(data?: IExternalAuthenticateResultModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.accessToken = data["accessToken"];
            this.encryptedAccessToken = data["encryptedAccessToken"];
            this.expireInSeconds = data["expireInSeconds"];
            this.waitingForActivation = data["waitingForActivation"];
        }
    }

    static fromJS(data: any): ExternalAuthenticateResultModel {
        let result = new ExternalAuthenticateResultModel();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["accessToken"] = this.accessToken;
        data["encryptedAccessToken"] = this.encryptedAccessToken;
        data["expireInSeconds"] = this.expireInSeconds;
        data["waitingForActivation"] = this.waitingForActivation;
        return data; 
    }

    clone() {
        const json = this.toJSON();
        let result = new ExternalAuthenticateResultModel();
        result.init(json);
        return result;
    }
}

export interface IExternalAuthenticateResultModel {
    accessToken: string;
    encryptedAccessToken: string;
    expireInSeconds: number;
    waitingForActivation: boolean;
}
