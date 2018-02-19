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
import { SysFunctionDto, ListResultDtoOfSysFunctionDto } from './../models/model-sysfunction';
import { SysModuleDto, ListResultDtoOfSysModuleDto } from './../models/model-sysmodule';
import { AppConsts } from '@shared/AppConsts';
import { BaseServiceProxy, throwException } from './service-proxy-base';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class PermissionServiceProxy extends BaseServiceProxy {

    constructor(@Inject(Http) http: Http, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        super(http, AppConsts.remoteServiceBaseUrl);
    }

    /**
     * @return Success
     */
    getAllSysModule(): Observable<ListResultDtoOfSysModuleDto> {
        let url_ = this.baseUrl + '/api/eswis/Permission/GetAllSysModule';

        url_ = url_.replace(/[?&]$/, '');

        let options_ : any = {
            method: 'get',
            headers: new Headers({
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            })
        };

        return this.http.request(url_, options_).flatMap((response_: any) => {
            return this.processGetAllSysModule(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processGetAllSysModule(response_);
                } catch (e) {
                    return <Observable<ListResultDtoOfSysModuleDto>><any>Observable.throw(e);
                }
            } else {
                return <Observable<ListResultDtoOfSysModuleDto>><any>Observable.throw(response_);
            }
        });
    }

    protected processGetAllSysModule(response: Response): Observable<ListResultDtoOfSysModuleDto> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? ListResultDtoOfSysModuleDto.fromJS(resultData200) : new ListResultDtoOfSysModuleDto();
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
        return Observable.of<ListResultDtoOfSysModuleDto>(<any>null);
    }

    /**
     * @return Success
     */
    getAllSysFunction(): Observable<ListResultDtoOfSysFunctionDto> {
        let url_ = this.baseUrl + '/api/eswis/Permission/GetAllSysFunction';

        url_ = url_.replace(/[?&]$/, '');

        let options_ : any = {
            method: 'get',
            headers: new Headers({
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            })
        };

        return this.http.request(url_, options_).flatMap((response_: any) => {
            return this.processGetAllSysFunction(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processGetAllSysFunction(response_);
                } catch (e) {
                    return <Observable<ListResultDtoOfSysFunctionDto>><any>Observable.throw(e);
                }
            } else {
                return <Observable<ListResultDtoOfSysFunctionDto>><any>Observable.throw(response_);
            }
        });
    }

    protected processGetAllSysFunction(response: Response): Observable<ListResultDtoOfSysFunctionDto> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? ListResultDtoOfSysFunctionDto.fromJS(resultData200) : new ListResultDtoOfSysFunctionDto();
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
        return Observable.of<ListResultDtoOfSysFunctionDto>(<any>null);
    }
}
