import 'rxjs/add/operator/finally';
import 'rxjs/add/observable/fromPromise';
import 'rxjs/add/observable/of';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/catch';

import { Observable } from 'rxjs/Observable';
import { AppConsts } from '@shared/AppConsts';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { Http, Headers, ResponseContentType, Response } from '@angular/http';
import { BaseServiceProxy, throwException } from './service-proxy-base';
import { GB4Dto, PagedResultDtoOfGB4Dto, OrgGB4Dto, PagedResultDtoOfOrgGB4Dto, 
         OriginGB4Dto, PagedResultDtoOfOriginGB4Dto } from './../models/model-GB4';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class GetGB4ServiceProxy {
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
    getGB4(skipCount: number, maxResultCount: number): Observable<PagedResultDtoOfGB4Dto> {
        let url_ = this.baseUrl + '/api/GBSAdmin/GetPaxSettingAll';

        url_ = url_.replace(/[?&]$/, '');

        let options_ : any = {
            method: 'get',
            headers: new Headers({
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            })
        };

        return this.http.request(url_, options_).flatMap((response_: any) => {
            return this.processGetGB4(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processGetGB4(response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfGB4Dto>><any>Observable.throw(e);
                }
            } else {
                return <Observable<PagedResultDtoOfGB4Dto>><any>Observable.throw(response_);
            }
        });
    }

    protected processGetGB4(response: Response): Observable<PagedResultDtoOfGB4Dto> {
        const status = response.status; 

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfGB4Dto.fromJS(resultData200) : new PagedResultDtoOfGB4Dto();
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
        return Observable.of<PagedResultDtoOfGB4Dto>(<any>null);
    }
    /**
     * @return Success
     */
    getOrgGB4(skipCount: number, maxResultCount: number): Observable<PagedResultDtoOfOrgGB4Dto> {
        let url_ = this.baseUrl + '/api/GBSAdmin/GetAllOrgID';

        url_ = url_.replace(/[?&]$/, '');

        let options_ : any = {
            method: 'get',
            headers: new Headers({
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            })
        };

        return this.http.request(url_, options_).flatMap((response_: any) => {
            return this.processOrgGB4(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processOrgGB4(response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfOrgGB4Dto>><any>Observable.throw(e);
                }
            } else {
                return <Observable<PagedResultDtoOfOrgGB4Dto>><any>Observable.throw(response_);
            }
        });
    }

    protected processOrgGB4(response: Response): Observable<PagedResultDtoOfOrgGB4Dto> {
        const status = response.status; 

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfOrgGB4Dto.fromJS(resultData200) : new PagedResultDtoOfOrgGB4Dto();
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
        return Observable.of<PagedResultDtoOfOrgGB4Dto>(<any>null);
    }
    // /**
    //  * @input (optional) 
    //  * @return Success
    //  */
    getAgnGB4(): Observable<PagedResultDtoOfOrgGB4Dto> {
        let url_ = this.baseUrl + '/api/GBSAdmin/GetAllAgentByOrgID';
        url_ = url_.replace(/[?&]$/, '');

        let options_ : any = {
            method: 'get',
            headers: new Headers({
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            })
        };

        return this.http.request(url_, options_).flatMap((response_: any) => {
            return this.processAgnGB4(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processAgnGB4(response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfOrgGB4Dto>><any>Observable.throw(e);
                }
            } else {
                return <Observable<PagedResultDtoOfOrgGB4Dto>><any>Observable.throw(response_);
            }
        });
    }

    protected processAgnGB4(response: Response): Observable<PagedResultDtoOfOrgGB4Dto> {
        const status = response.status; 

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfOrgGB4Dto.fromJS(resultData200) : new PagedResultDtoOfOrgGB4Dto();
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
        return Observable.of<PagedResultDtoOfOrgGB4Dto>(<any>null);
    }
    // /**
    //  * @input (optional) 
    //  * @return Success
    //  */
    getOriginGB4(): Observable<PagedResultDtoOfOriginGB4Dto> {
        let url_ = this.baseUrl + '/api/GBSAdmin/GetAllOrigin';
        url_ = url_.replace(/[?&]$/, '');

        let options_ : any = {
            method: 'get',
            headers: new Headers({
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            })
        };

        return this.http.request(url_, options_).flatMap((response_: any) => {
            return this.processOriginGB4(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processOriginGB4(response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfOriginGB4Dto>><any>Observable.throw(e);
                }
            } else {
                return <Observable<PagedResultDtoOfOriginGB4Dto>><any>Observable.throw(response_);
            }
        });
    }

    protected processOriginGB4(response: Response): Observable<PagedResultDtoOfOriginGB4Dto> {
        const status = response.status; 

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfOriginGB4Dto.fromJS(resultData200) : new PagedResultDtoOfOriginGB4Dto();
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
        return Observable.of<PagedResultDtoOfOriginGB4Dto>(<any>null);
    }
    // /**
    //  * @input (optional) 
    //  * @return Success
    //  */
    //     update(input: Array<GB4Dto>): Observable<Array<GB4Dto>> {
    //     let url_ = this.baseUrl + '/api/GBSAdmin/UpdatePaymentScheme';
    //     url_ = url_.replace(/[?&]$/, '');

    //     const content_ = JSON.stringify(input);

    //     let options_ : any = {
    //         body: content_,
    //         method: 'put',
    //         headers: new Headers({
    //             'Content-Type': 'application/json', 
    //             'Accept': 'application/json'
    //         })
    //     };

    //     return this.http.request(url_, options_).flatMap((response_ : any) => {
    //         return this.processUpdate(response_);
    //     }).catch((response_: any) => {
    //         if (response_ instanceof Response) {
    //             try {
    //                 return this.processUpdate(response_);
    //             } catch (e) {
    //                 return <Observable<Array<GB4Dto>>><any>Observable.throw(e);
    //             }
    //         } else
    //             return <Observable<Array<GB4Dto>>><any>Observable.throw(response_);
    //     });
    // }

    // protected processUpdate(response: Response): Observable<Array<GB4Dto>> {
    //     const status = response.status; 

    //     let _headers: any = response.headers ? response.headers.toJSON() : {};
    //     if (status === 200) {
    //         const _responseText = response.text();
    //         let result200: any = null;
    //         let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
    //         result200 = resultData200 ? GB4Dto.fromJS(resultData200) : new GB4Dto();
    //         return Observable.of(result200);
    //     } else if (status === 401) {
    //         const _responseText = response.text();
    //         return throwException('A server error occurred.', status, _responseText, _headers);
    //     } else if (status === 403) {
    //         const _responseText = response.text();
    //         return throwException('A server error occurred.', status, _responseText, _headers);
    //     } else if (status !== 200 && status !== 204) {
    //         const _responseText = response.text();
    //         return throwException('An unexpected server error occurred.', status, _responseText, _headers);
    //     }
    //     return Observable.of<Array<GB4Dto>>(<any>null);
    // }
}
