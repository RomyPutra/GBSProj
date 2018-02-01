import { Observable } from 'rxjs/Observable';
import { Injectable } from '@angular/core';
import { Http} from '@angular/http';

@Injectable()
export class BaseServiceProxy {
    protected http: Http;
    protected baseUrl: string;
    protected jsonParseReviver: (key: string, value: any) => any = undefined;

    constructor(http: Http, baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : '';
    }
}


export class SwaggerException extends Error {
    message: string;
    status: number;
    response: string;
    headers: { [key: string]: any; };
    result: any;

    protected isSwaggerException = true;

    static isSwaggerException(obj: any): obj is SwaggerException {
        return obj.isSwaggerException === true;
    }

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }
}

export function throwException(message: string, status: number, response: string, headers: { [key: string]: any; },
                        result?: any): Observable<any> {
    if (result !== null && result !== undefined) {
        return Observable.throw(result);
    } else {
        return Observable.throw(new SwaggerException(message, status, response, headers, null));
    }
}