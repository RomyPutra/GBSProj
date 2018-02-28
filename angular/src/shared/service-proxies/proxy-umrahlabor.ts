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
import { umrahlaborDto, PagedResultDtoOfumrahlaborDto } from './../models/model-umrahlabor';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class GetumrahlaborServiceProxy {
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
    getumrahlabor(skipCount: number, maxResultCount: number): Observable<PagedResultDtoOfumrahlaborDto> {
        let url_ = this.baseUrl + '/api/GBSAdmin/GetUmrahLaborAll';

        url_ = url_.replace(/[?&]$/, '');

        let options_ : any = {
            method: 'get',
            headers: new Headers({
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            })
        };

        return this.http.request(url_, options_).flatMap((response_: any) => {
            return this.processGetumrahlabor(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processGetumrahlabor(response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfumrahlaborDto>><any>Observable.throw(e);
                }
            } else {
                return <Observable<PagedResultDtoOfumrahlaborDto>><any>Observable.throw(response_);
            }
        });
    }

    protected processGetumrahlabor(response: Response): Observable<PagedResultDtoOfumrahlaborDto> {
        const status = response.status; 

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfumrahlaborDto.fromJS(resultData200) : new PagedResultDtoOfumrahlaborDto();
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
        return Observable.of<PagedResultDtoOfumrahlaborDto>(<any>null);
    }
    // /**
    //  * @input (optional)
    //  * @return Success
    //  */
    post(input: Array<umrahlaborDto>): Observable<Array<umrahlaborDto>> {
        let url_ = this.baseUrl + '/api/GBSAdmin/UploadUmrahLabor';
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
                    return <Observable<Array<umrahlaborDto>>><any>Observable.throw(e);
                }
            } else {
                return <Observable<Array<umrahlaborDto>>><any>Observable.throw(response_);
            }
        });
    }

    protected processUpdate(response: Response): Observable<Array<umrahlaborDto>> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? umrahlaborDto.fromJS(resultData200) : new umrahlaborDto();
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
        return Observable.of<Array<umrahlaborDto>>(<any>null);
    }
    // /**
    //  * @input (optional) 
    //  * @return Success
    //  */
    //     update(input: Array<DiscountDto>): Observable<Array<DiscountDto>> {
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
    //                 return <Observable<Array<DiscountDto>>><any>Observable.throw(e);
    //             }
    //         } else
    //             return <Observable<Array<DiscountDto>>><any>Observable.throw(response_);
    //     });
    // }

    // protected processUpdate(response: Response): Observable<Array<DiscountDto>> {
    //     const status = response.status; 

    //     let _headers: any = response.headers ? response.headers.toJSON() : {};
    //     if (status === 200) {
    //         const _responseText = response.text();
    //         let result200: any = null;
    //         let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
    //         result200 = resultData200 ? DiscountDto.fromJS(resultData200) : new DiscountDto();
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
    //     return Observable.of<Array<DiscountDto>>(<any>null);
    // }
}
