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
import { GBSDto, PagedResultDtoOfGBSDto, GBSCountryDto, PagedResultDtoOfGBSCountryDto } from './../models/model-gbs';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

export class GbsCodex {
    Code: string;
}
export class GbsPayType {
    Type: string;
}
let GbsCodexs: GbsCodex[] = [{
    'Code': 'DOB'
}, {
    'Code': 'STD'
}];
let GbsPayTypes: GbsPayType[] = [{
    'Type': 'DEPO'
}, {
    'Type': 'FULL'
}];
@Injectable()
export class GetBookingServiceProxy {
    private http: Http;
    private baseUrl: string;
    protected jsonParseReviver: (key: string, value: any) => any = undefined;

    constructor(@Inject(Http) http: Http, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = AppConsts.remoteServiceBaseUrl;
        // this.baseUrl = baseUrl ? baseUrl : '';
    }
    /**
     * @return Success
     */
    getCodex() {
        return GbsCodexs;
    }
    getPayType() {
        return GbsPayTypes;
    }
    /**
     * @return Success
     */
    get(id: string): Observable<GBSCountryDto> {
        let url_ = this.baseUrl + '/api/GBSAdmin/GetAllCountry?';
        if (id === undefined || id === null) {
            throw new Error('The parameter "id" must be defined and cannot be null.');
        }
        else {
            url_ += 'CountryID=' + encodeURIComponent('' + id) + '&'; 
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
            return this.processGet(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processGet(response_);
                } catch (e) {
                    return <Observable<GBSCountryDto>><any>Observable.throw(e);
                }
            } else {
                return <Observable<GBSCountryDto>><any>Observable.throw(response_);
            }
        });
    }

    protected processGet(response: Response): Observable<GBSCountryDto> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200.items[0] ? GBSCountryDto.fromJS(resultData200.items[0]) : new GBSCountryDto();
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
        return Observable.of<GBSCountryDto>(<any>null);
    }
    /**
     * @return Success
     */
    getCountry(skipCount: number, maxResultCount: number): Observable<PagedResultDtoOfGBSCountryDto> {
        let url_ = this.baseUrl + '/api/GBSAdmin/GetAllCountry';

        url_ = url_.replace(/[?&]$/, '');

        let options_ : any = {
            method: 'get',
            headers: new Headers({
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            })
        };

        return this.http.request(url_, options_).flatMap((response_: any) => {
            return this.processGetCountry(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processGetCountry(response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfGBSCountryDto>><any>Observable.throw(e);
                }
            } else {
                return <Observable<PagedResultDtoOfGBSCountryDto>><any>Observable.throw(response_);
            }
        });
    }

    protected processGetCountry(response: Response): Observable<PagedResultDtoOfGBSCountryDto> {
        const status = response.status; 

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfGBSCountryDto.fromJS(resultData200) : new PagedResultDtoOfGBSCountryDto();
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
        return Observable.of<PagedResultDtoOfGBSCountryDto>(<any>null);
    }
    /**
     * @return Success
     */
    getbookingbypnr(skipCount: number, maxResultCount: number): Observable<PagedResultDtoOfGBSDto> {
        let url_ = this.baseUrl + '/api/GBSAdmin/PaymentScheme?GRPID=AA';

        url_ = url_.replace(/[?&]$/, '');

        let options_ : any = {
            method: 'get',
            headers: new Headers({
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            })
        };

        return this.http.request(url_, options_).flatMap((response_: any) => {
            return this.processGetBooking(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processGetBooking(response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfGBSDto>><any>Observable.throw(e);
                }
            } else {
                return <Observable<PagedResultDtoOfGBSDto>><any>Observable.throw(response_);
            }
        });
    }

    protected processGetBooking(response: Response): Observable<PagedResultDtoOfGBSDto> {
        const status = response.status; 

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfGBSDto.fromJS(resultData200) : new PagedResultDtoOfGBSDto();
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
        return Observable.of<PagedResultDtoOfGBSDto>(<any>null);
    }
    /**
     * @input (optional) 
     * @return Success
     */
        update(input: Array<GBSDto>): Observable<Array<GBSDto>> {
        let url_ = this.baseUrl + '/api/GBSAdmin/UpdatePaymentScheme';
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
                    return <Observable<Array<GBSDto>>><any>Observable.throw(e);
                }
            } else
                return <Observable<Array<GBSDto>>><any>Observable.throw(response_);
        });
    }

    protected processUpdate(response: Response): Observable<Array<GBSDto>> {
        const status = response.status; 

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? GBSDto.fromJS(resultData200) : new GBSDto();
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
        return Observable.of<Array<GBSDto>>(<any>null);
    }
}
