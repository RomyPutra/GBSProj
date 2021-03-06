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
import { PBTDto, PagedResultDtoOfPBTDto } from '@shared/models/model-pbt';
import { BaseServiceProxy, throwException } from './service-proxy-base';

import * as moment from 'moment';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class PBTServiceProxy {
    private http: Http;
    private baseUrl: string;
    protected jsonParseReviver: (key: string, value: any) => any = undefined;

    constructor(@Inject(Http) http: Http, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = AppConsts.remoteServiceBaseUrl;
    }

    /**
     * @return Success
     */
    get(bizRegID: string): Observable<PBTDto> {
        let url_ = this.baseUrl + "/api/eswis/PBT/Get?";
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
                    return <Observable<PBTDto>><any>Observable.throw(e);
                }
            } else {
                return <Observable<PBTDto>><any>Observable.throw(response_);
            }
        });
    }

    protected processGet(response: Response): Observable<PBTDto> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PBTDto.fromJS(resultData200) : new PBTDto();
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
        return Observable.of<PBTDto>(<any>null);
    }
   
    /**
     * @return Success
     */
    getAll(skipCount: number, maxResultCount: number): Observable<PagedResultDtoOfPBTDto> {
        let url_ = this.baseUrl + '/api/eswis/PBT/GetAll';
        url_ = url_.replace(/[?&]$/, '');

        let options_ : any = {
            method: "get",
            headers: new Headers({
                "Content-Type": "application/json",
                "Accept": "application/json"
            })
        };

        return this.http.request(url_, options_).flatMap((response_: any) => {
            return this.processGetAll(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processGetAll(response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfPBTDto>><any>Observable.throw(e);
                }
            } else {
                return <Observable<PagedResultDtoOfPBTDto>><any>Observable.throw(response_);
            }
        });
    }

    protected processGetAll(response: Response): Observable<PagedResultDtoOfPBTDto> {
        const status = response.status; 

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfPBTDto.fromJS(resultData200) : new PagedResultDtoOfPBTDto();
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
        return Observable.of<PagedResultDtoOfPBTDto>(<any>null);
    }
}
