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
import { Http, Headers, ResponseContentType, Response, RequestOptions } from '@angular/http';
import { BaseServiceProxy, throwException } from './service-proxy-base';
import { FlightTimeDto, PagedResultDtoOfFlightTimeDto } from './../models/model-gbs';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class GetFlightTimeServiceProxy {
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
    getFlighttime(skipCount: number, maxResultCount: number): Observable<PagedResultDtoOfFlightTimeDto> {
        let url_ = this.baseUrl + '/api/GBSAdmin/GroupTime';

        url_ = url_.replace(/[?&]$/, '');

        let options_ : any = {
            method: 'get',
            headers: new Headers({
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            })
        };

        return this.http.request(url_, options_).flatMap((response_: any) => {
            return this.processGetFlightTime(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processGetFlightTime(response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfFlightTimeDto>><any>Observable.throw(e);
                }
            } else {
                return <Observable<PagedResultDtoOfFlightTimeDto>><any>Observable.throw(response_);
            }
        });
    }

    protected processGetFlightTime(response: Response): Observable<PagedResultDtoOfFlightTimeDto> {
        const status = response.status; 

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfFlightTimeDto.fromJS(resultData200) : new PagedResultDtoOfFlightTimeDto();
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
        return Observable.of<PagedResultDtoOfFlightTimeDto>(<any>null);
    }

    // /**
    //  * @input (optional) 
    //  * @return Success
    //  */
    post(input: Array<FlightTimeDto>): Observable<Array<FlightTimeDto>> {
        let url_ = this.baseUrl + '/api/GBSAdmin/UploadGroupTime';
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
                    return <Observable<Array<FlightTimeDto>>><any>Observable.throw(e);
                }
            } else
                return <Observable<Array<FlightTimeDto>>><any>Observable.throw(response_);
        });
    }

    protected processUpdate(response: Response): Observable<Array<FlightTimeDto>> {
        const status = response.status; 

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? FlightTimeDto.fromJS(resultData200) : new FlightTimeDto();
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
        return Observable.of<Array<FlightTimeDto>>(<any>null);
    }
    // /**
    //  * @input (optional) 
    //  * @return Success
    //  */
    //     update(input: Array<FlightTimeDto>): Observable<Array<FlightTimeDto>> {
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
    //                 return <Observable<Array<FlightTimeDto>>><any>Observable.throw(e);
    //             }
    //         } else
    //             return <Observable<Array<FlightTimeDto>>><any>Observable.throw(response_);
    //     });
    // }

    // protected processUpdate(response: Response): Observable<Array<FlightTimeDto>> {
    //     const status = response.status; 

    //     let _headers: any = response.headers ? response.headers.toJSON() : {};
    //     if (status === 200) {
    //         const _responseText = response.text();
    //         let result200: any = null;
    //         let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
    //         result200 = resultData200 ? FlightTimeDto.fromJS(resultData200) : new FlightTimeDto();
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
    //     return Observable.of<Array<FlightTimeDto>>(<any>null);
    // }
}
