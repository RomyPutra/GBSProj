import 'rxjs/add/operator/finally';
import 'rxjs/add/observable/fromPromise';
import 'rxjs/add/observable/of';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/catch';

import { Observable } from 'rxjs/Observable';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { Http, Headers, ResponseContentType, Response } from '@angular/http';
import { CurrencyDto, PagedResultDtoOfCurrencyDto } from './../models/model-currency';
import { AppConsts } from '@shared/AppConsts';
import { BaseServiceProxy, throwException } from './service-proxy-base';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class CurrencyServiceProxy extends BaseServiceProxy {

    constructor(@Inject(Http) http: Http, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        super(http, AppConsts.remoteServiceBaseUrl);
    }

    /**
     * @input (optional)
     * @return Success
     */
    create(input: CurrencyDto): Observable<boolean> {
        let url_ = this.baseUrl + '/api/eswis/Currency/Create';
        url_ = url_.replace(/[?&]$/, '');

        const content_ = JSON.stringify(input);
        // const content_ = input.toJSON();

        let options_ : any = {
            body: content_,
            method: 'post',
            headers: new Headers({
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            })
        };

        return this.http.request(url_, options_).flatMap((response_ : any) => {
            return this.processCreate(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processCreate(response_);
                } catch (e) {
                    return <Observable<boolean>><any>Observable.throw(e);
                }
            } else {
                return <Observable<boolean>><any>Observable.throw(response_);
            }
        });
    }

    protected processCreate(response: Response): Observable<boolean> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? true : false;
            return Observable.of(result200);
        } else if (status === 401) {
            const _responseText = response.text();
            return throwException('A server error occurred.', status, _responseText, _headers);
        } else if (status === 403) {
            const _responseText = response.text();
            return throwException('A server error occurred.', status, _responseText, _headers);
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.text();
            return throwException('An unexpected server error occurred.', status, _responseText, _headers);
        }
        return Observable.of<boolean>(<any>null);
    }

    /**
     * @input (optional)
     * @return Success
     */
    update(input: CurrencyDto): Observable<boolean> {
        let url_ = this.baseUrl + '/api/eswis/Currency/Update';
        url_ = url_.replace(/[?&]$/, '');

        const content_ = JSON.stringify(input);

        let options_ : any = {
            body: content_,
            method: 'put',
            headers: new Headers({
                'Content-Type': 'application/json', 
                'Accept': 'application/json'
            })
        };

        return this.http.request(url_, options_).flatMap((response_ : any) => {
            return this.processUpdate(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processUpdate(response_);
                } catch (e) {
                    return <Observable<boolean>><any>Observable.throw(e);
                }
            } else {
                return <Observable<boolean>><any>Observable.throw(response_);
            }
        });
    }

    protected processUpdate(response: Response): Observable<boolean> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? true : false;
            return Observable.of(result200);
        } else if (status === 401) {
            const _responseText = response.text();
            return throwException('A server error occurred.', status, _responseText, _headers);
        } else if (status === 403) {
            const _responseText = response.text();
            return throwException('A server error occurred.', status, _responseText, _headers);
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.text();
            return throwException('An unexpected server error occurred.', status, _responseText, _headers);
        }
        return Observable.of<boolean>(<any>null);
    }

    /**
     * @return Success
     */
    delete(input: CurrencyDto): Observable<boolean> {
        let url_ = this.baseUrl + '/api/eswis/Currency/Delete';
        url_ = url_.replace(/[?&]$/, '');

        const content_ = JSON.stringify(input);

        let options_ : any = {
            body: content_,
            method: 'put',
            headers: new Headers({
                'Content-Type': 'application/json', 
                'Accept': 'application/json'
            })
        };

        return this.http.request(url_, options_).flatMap((response_: any) => {
            return this.processDelete(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processDelete(response_);
                } catch (e) {
                    return <Observable<boolean>><any>Observable.throw(e);
                }
            } else {
                return <Observable<boolean>><any>Observable.throw(response_);
            }
        });
    }

    protected processDelete(response: Response): Observable<boolean> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? true : false;
            return Observable.of(result200);
        } else if (status === 401) {
            const _responseText = response.text();
            return throwException('A server error occurred.', status, _responseText, _headers);
        } else if (status === 403) {
            const _responseText = response.text();
            return throwException('A server error occurred.', status, _responseText, _headers);
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.text();
            return throwException('An unexpected server error occurred.', status, _responseText, _headers);
        }
        return Observable.of<boolean>(<any>null);
    }

    /**
     * @return Success
     */
    getAll(): Observable<PagedResultDtoOfCurrencyDto> {
        let url_ = this.baseUrl + '/api/eswis/Currency/GetAll';

        url_ = url_.replace(/[?&]$/, '');

        let options_ : any = {
            method: 'get',
            headers: new Headers({
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            })
        };

        return this.http.request(url_, options_).flatMap((response_: any) => {
            return this.processGetAll(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processGetAll(response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfCurrencyDto>><any>Observable.throw(e);
                }
            } else {
                return <Observable<PagedResultDtoOfCurrencyDto>><any>Observable.throw(response_);
            }
        });
    }

    protected processGetAll(response: Response): Observable<PagedResultDtoOfCurrencyDto> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfCurrencyDto.fromJS(resultData200) : new PagedResultDtoOfCurrencyDto();
            return Observable.of(result200);
        } else if (status === 401) {
            const _responseText = response.text();
            return throwException('A server error occurred.', status, _responseText, _headers);
        } else if (status === 403) {
            const _responseText = response.text();
            return throwException('A server error occurred.', status, _responseText, _headers);
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.text();
            return throwException('An unexpected server error occurred.', status, _responseText, _headers);
        }
        return Observable.of<PagedResultDtoOfCurrencyDto>(<any>null);
    }
}
