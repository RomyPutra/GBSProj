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
import { CapacityDto, PagedResultDtoOfCapacityDto } from './../models/model-capacity';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class GetCapacityServiceProxy {
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
    getCapacity(skipCount: number, maxResultCount: number): Observable<PagedResultDtoOfCapacityDto> {
        let url_ = this.baseUrl + '/api/GBSAdmin/GetGroupCapAll';

        url_ = url_.replace(/[?&]$/, '');

        let options_ : any = {
            method: 'get',
            headers: new Headers({
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            })
        };

        return this.http.request(url_, options_).flatMap((response_: any) => {
            return this.processGetCapacity(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processGetCapacity(response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfCapacityDto>><any>Observable.throw(e);
                }
            } else {
                return <Observable<PagedResultDtoOfCapacityDto>><any>Observable.throw(response_);
            }
        });
    }

    protected processGetCapacity(response: Response): Observable<PagedResultDtoOfCapacityDto> {
        const status = response.status; 

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfCapacityDto.fromJS(resultData200) : new PagedResultDtoOfCapacityDto();
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
        return Observable.of<PagedResultDtoOfCapacityDto>(<any>null);
    }
    // /**
    //  * @input (optional)
    //  * @return Success
    //  */
    post(input: Array<CapacityDto>): Observable<Array<CapacityDto>> {
        let url_ = this.baseUrl + '/api/GBSAdmin/UploadCapacity';
        url_ = url_.replace(/[?&]$/, '');

        const content_ = JSON.stringify(input);

        let options_: any = {
            body: content_,
            method: 'put',
            headers: new Headers({
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            })
        };

        return this.http.request(url_, options_).flatMap((response_: any) => {
            return this.processUpdate(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processUpdate(response_);
                } catch (e) {
                    return <Observable<Array<CapacityDto>>><any>Observable.throw(e);
                }
            } else {
                return <Observable<Array<CapacityDto>>><any>Observable.throw(response_);
            }
        });
    }

    protected processUpdate(response: Response): Observable<Array<CapacityDto>> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? CapacityDto.fromJS(resultData200) : new CapacityDto();
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
        return Observable.of<Array<CapacityDto>>(<any>null);
    }
    // /**
    //  * @input (optional) 
    //  * @return Success
    //  */
    //     update(input: Array<CapacityDto>): Observable<Array<CapacityDto>> {
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
    //                 return <Observable<Array<CapacityDto>>><any>Observable.throw(e);
    //             }
    //         } else
    //             return <Observable<Array<CapacityDto>>><any>Observable.throw(response_);
    //     });
    // }

    // protected processUpdate(response: Response): Observable<Array<CapacityDto>> {
    //     const status = response.status; 

    //     let _headers: any = response.headers ? response.headers.toJSON() : {};
    //     if (status === 200) {
    //         const _responseText = response.text();
    //         let result200: any = null;
    //         let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
    //         result200 = resultData200 ? CapacityDto.fromJS(resultData200) : new CapacityDto();
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
    //     return Observable.of<Array<CapacityDto>>(<any>null);
    // }
}
