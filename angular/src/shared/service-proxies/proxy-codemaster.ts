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
import { CodeMasterDto, PagedResultDtoOfCodeMasterDto } from './../models/model-codemaster';
import { AppConsts } from '@shared/AppConsts';
import { BaseServiceProxy, throwException } from './service-proxy-base';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class CodeMasterServiceProxy extends BaseServiceProxy {

    constructor(@Inject(Http) http: Http, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        super(http, AppConsts.remoteServiceBaseUrl);
    }

    /**
     * @return Success
     */
    getGenders(): Observable<PagedResultDtoOfCodeMasterDto> {
        let url_ = this.baseUrl + '/api/eswis/CodeMaster/GetGenders';

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
                    return <Observable<PagedResultDtoOfCodeMasterDto>><any>Observable.throw(e);
                }
            } else {
                return <Observable<PagedResultDtoOfCodeMasterDto>><any>Observable.throw(response_);
            }
        });
    }

    /**
     * @return Success
     */
    getSalutations(): Observable<PagedResultDtoOfCodeMasterDto> {
        let url_ = this.baseUrl + '/api/eswis/CodeMaster/GetSalutations';

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
                    return <Observable<PagedResultDtoOfCodeMasterDto>><any>Observable.throw(e);
                }
            } else {
                return <Observable<PagedResultDtoOfCodeMasterDto>><any>Observable.throw(response_);
            }
        });
    }

    /**
     * @return Success
     */
    getAppIDs(): Observable<PagedResultDtoOfCodeMasterDto> {
        let url_ = this.baseUrl + '/api/eswis/CodeMaster/GetAppIDs';

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
                    return <Observable<PagedResultDtoOfCodeMasterDto>><any>Observable.throw(e);
                }
            } else {
                return <Observable<PagedResultDtoOfCodeMasterDto>><any>Observable.throw(response_);
            }
        });
    }

    /**
     * @return Success
     */
    getAppIDsMerged(code?: string): Observable<PagedResultDtoOfCodeMasterDto> {
        let url_ = this.baseUrl + '/api/eswis/CodeMaster/GetAppIDsMerged?';
        if (code === undefined || code === null) {
        } else {
            url_ += 'code=' + encodeURIComponent('' + code) + '&';
        }
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
                    return <Observable<PagedResultDtoOfCodeMasterDto>><any>Observable.throw(e);
                }
            } else {
                return <Observable<PagedResultDtoOfCodeMasterDto>><any>Observable.throw(response_);
            }
        });
    }

    /**
     * @return Success
     */
    getDesignations(): Observable<PagedResultDtoOfCodeMasterDto> {
        let url_ = this.baseUrl + '/api/eswis/CodeMaster/GetDesignations';

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
                    return <Observable<PagedResultDtoOfCodeMasterDto>><any>Observable.throw(e);
                }
            } else {
                return <Observable<PagedResultDtoOfCodeMasterDto>><any>Observable.throw(response_);
            }
        });
    }

    protected processGetAll(response: Response): Observable<PagedResultDtoOfCodeMasterDto> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfCodeMasterDto.fromJS(resultData200) : new PagedResultDtoOfCodeMasterDto();
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
        return Observable.of<PagedResultDtoOfCodeMasterDto>(<any>null);
    }
}
