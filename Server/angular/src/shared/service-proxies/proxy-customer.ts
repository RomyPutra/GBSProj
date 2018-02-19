/* tslint:disable */
//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v11.12.9.0 (NJsonSchema v9.10.9.0 (Newtonsoft.Json v9.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------
// ReSharper disable InconsistentNaming

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
import { AppConsts } from '@shared/AppConsts';
import { CustomerDto, PagedResultDtoOfCustomerDto } from '@shared/models/model-customer';
import { BaseServiceProxy, throwException } from './service-proxy-base';

import * as moment from 'moment';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class CustomerServiceProxy {
    private http: Http;
    private baseUrl: string;
    protected jsonParseReviver: (key: string, value: any) => any = undefined;

    constructor(@Inject(Http) http: Http, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = AppConsts.remoteServiceBaseUrl;
    }

    /**
     * @input (optional) 
     * @return Success
     */
    create(input: CustomerDto): Observable<boolean> {
        let url_ = this.baseUrl + "/api/eswis/Customer/Create";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(input);

        let options_ : any = {
            body: content_,
            method: "post",
            headers: new Headers({
                "Content-Type": "application/json", 
                "Accept": "application/json"
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
            } else
                return <Observable<boolean>><any>Observable.throw(response_);
        });
    }

    protected processCreate(response: Response): Observable<boolean> {
        const status = response.status; 

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? true : false;
            return Observable.of(result200);
        } else if (status === 401) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status === 403) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.text();
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Observable.of<boolean>(<any>null);
    }

    /**
     * @input (optional) 
     * @return Success
     */
    update(input: CustomerDto): Observable<boolean> {
        let url_ = this.baseUrl + "/api/eswis/Customer/Update";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(input);

        let options_ : any = {
            body: content_,
            method: "put",
            headers: new Headers({
                "Content-Type": "application/json", 
                "Accept": "application/json"
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
            } else
                return <Observable<boolean>><any>Observable.throw(response_);
        });
    }

    protected processUpdate(response: Response): Observable<boolean> {
        const status = response.status; 

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? true : false;
            return Observable.of(result200);
        } else if (status === 401) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status === 403) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.text();
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Observable.of<boolean>(<any>null);
    }

    /**
     * @return Success
     */
    delete(data: CustomerDto): Observable<boolean> {
        let url_ = this.baseUrl + "/api/eswis/Customer/Delete?";
        if (data === undefined || data === null)
            throw new Error("The parameter 'data' must be defined and cannot be null.");
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            method: "delete",
            headers: new Headers({
                "Content-Type": "application/json", 
            })
        };

        return this.http.request(url_, options_).flatMap((response_ : any) => {
            return this.processDelete(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processDelete(response_);
                } catch (e) {
                    return <Observable<boolean>><any>Observable.throw(e);
                }
            } else
                return <Observable<boolean>><any>Observable.throw(response_);
        });
    }

    protected processDelete(response: Response): Observable<boolean> {
        const status = response.status; 

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            return Observable.of<boolean>(<any>null);
        } else if (status === 401) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status === 403) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.text();
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Observable.of<boolean>(<any>null);
    }

    /**
     * @return Success
     */
    get(bizRegID: string): Observable<CustomerDto> {
        let url_ = this.baseUrl + "/api/eswis/Customer/Get?";
        if (bizRegID === undefined || bizRegID === null)
            throw new Error("The parameter 'bizRegID' must be defined and cannot be null.");
        else
            url_ += "bizRegID=" + encodeURIComponent("" + bizRegID) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            method: "get",
            headers: new Headers({
                "Content-Type": "application/json", 
                "Accept": "application/json"
            })
        };

        return this.http.request(url_, options_).flatMap((response_: any) => {
            return this.processGet(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processGet(response_);
                } catch (e) {
                    return <Observable<CustomerDto>><any>Observable.throw(e);
                }
            } else {
                return <Observable<CustomerDto>><any>Observable.throw(response_);
            }
        });
    }

    protected processGet(response: Response): Observable<CustomerDto> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? CustomerDto.fromJS(resultData200) : new CustomerDto();
            return Observable.of(result200);
        } else if (status === 401) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status === 403) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.text();
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Observable.of<CustomerDto>(<any>null);
    }
   
    /**
     * @return Success
     */
    getListEntity(skipCount: number, maxResultCount: number, state: string, orderby: string, filterText?: string): Observable<PagedResultDtoOfCustomerDto> {
        let url_ = this.baseUrl + '/api/eswis/Customer/GetListEntity?';
        if (skipCount === undefined || skipCount === null)
            throw new Error("The parameter 'skipCount' must be defined and cannot be null.");
        else
            url_ += "skipCount=" + encodeURIComponent("" + skipCount) + "&";

        if (maxResultCount === undefined || maxResultCount === null)
            throw new Error("The parameter 'maxResultCount' must be defined and cannot be null.");
        else
            url_ += "limit=" + encodeURIComponent("" + maxResultCount) + "&";

        if (state === undefined || state === null)
            throw new Error("The parameter 'state' must be defined and cannot be null.");
        else
            url_ += "state=" + encodeURIComponent("" + state) + "&";

        if (orderby === undefined || orderby === null)
            throw new Error("The parameter 'orderby' must be defined and cannot be null.");
        else
            url_ += "orderby=" + encodeURIComponent("" + orderby) + "&"; 

        if (filterText !== undefined || filterText !== null)
            url_ += "filter=" + encodeURIComponent("" + filterText) + "&"; 
            
        url_ = url_.replace(/[?&]$/, '');

        let options_ : any = {
            method: "get",
            headers: new Headers({
                "Content-Type": "application/json",
                "Accept": "application/json"
            })
        };

        return this.http.request(url_, options_).flatMap((response_: any) => {
            return this.processGetListEntity(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processGetListEntity(response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfCustomerDto>><any>Observable.throw(e);
                }
            } else {
                return <Observable<PagedResultDtoOfCustomerDto>><any>Observable.throw(response_);
            }
        });
    }

    protected processGetListEntity(response: Response): Observable<PagedResultDtoOfCustomerDto> {
        const status = response.status; 

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfCustomerDto.fromJS(resultData200) : new PagedResultDtoOfCustomerDto();
            return Observable.of(result200);
        } else if (status === 401) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status === 403) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.text();
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Observable.of<PagedResultDtoOfCustomerDto>(<any>null);
    }
}
