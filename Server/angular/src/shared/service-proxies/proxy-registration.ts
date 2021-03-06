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
import { RegistrationDto, PostRegistrationDto } from './../models/model-registration';
import { AppConsts } from '@shared/AppConsts';
import { BaseServiceProxy, throwException } from './service-proxy-base';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class RegistrationServiceProxy extends BaseServiceProxy {

    constructor(@Inject(Http) http: Http, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        super(http, AppConsts.remoteServiceBaseUrl);
    }

    /**
     * @input (optional)
     * @return Success
     */
    // create(input: RegistrationDto): Observable<boolean> {
    //     let url_ = this.baseUrl + '/api/eswis/Registration/Create';
    //     url_ = url_.replace(/[?&]$/, '');

    //     const content_ = JSON.stringify(input);
    //     // const content_ = input.toJSON();

    //     let options_ : any = {
    //         body: content_,
    //         method: 'post',
    //         headers: new Headers({
    //             'Content-Type': 'application/json',
    //             'Accept': 'application/json'
    //         })
    //     };

    //     return this.http.request(url_, options_).flatMap((response_ : any) => {
    //         return this.processCreate(response_);
    //     }).catch((response_: any) => {
    //         if (response_ instanceof Response) {
    //             try {
    //                 return this.processCreate(response_);
    //             } catch (e) {
    //                 return <Observable<boolean>><any>Observable.throw(e);
    //             }
    //         } else {
    //             return <Observable<boolean>><any>Observable.throw(response_);
    //         }
    //     });
    // }

    // protected processCreate(response: Response): Observable<boolean> {
    //     const status = response.status;

    //     let _headers: any = response.headers ? response.headers.toJSON() : {};
    //     if (status === 200) {
    //         const _responseText = response.text();
    //         let result200: any = null;
    //         let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
    //         result200 = resultData200 ? true : false;
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
    //     return Observable.of<boolean>(<any>null);
    // }
    
    /**
     * @return Success
     */
    get(dOEFileNo: string, companyID: string ): Observable<RegistrationDto> {
        let url_ = this.baseUrl + "/api/eswis/Registration/GetUnregisteredDOE?";
        if (dOEFileNo === undefined || dOEFileNo === null && companyID === undefined || companyID === null) {
            throw new Error("The parameter 'RegistrationID' must be defined and cannot be null.");
        } else {
            url_ += "DOEFileNo=" + encodeURIComponent("" + dOEFileNo) + "&";
            url_ += "CompanyName=" + encodeURIComponent("" + companyID) + "&";
        }
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
                    return <Observable<RegistrationDto>><any>Observable.throw(e);
                }
            } else {
                return <Observable<RegistrationDto>><any>Observable.throw(response_);
            }
        });
    }

    protected processGet(response: Response): Observable<RegistrationDto> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? RegistrationDto.fromJS(resultData200) : new RegistrationDto();
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
        return Observable.of<RegistrationDto>(<any>null);
    }

    /**
     * @return Success
     */
    getDOEEntity(dOEFileNo: string, companyID: string ): Observable<RegistrationDto> {
        let url_ = this.baseUrl + "/api/eswis/Registration/VerifyRegisteredDOE?";
        if (dOEFileNo === undefined || dOEFileNo === null && companyID === undefined || companyID === null) {
            throw new Error("The parameter 'RegistrationID' must be defined and cannot be null.");
        } else {
            url_ += "DOEFileNo=" + encodeURIComponent("" + dOEFileNo) + "&";
            url_ += "CompanyName=" + encodeURIComponent("" + companyID) + "&";
        }
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            method: "get",
            headers: new Headers({
                "Content-Type": "application/json", 
                "Accept": "application/json"
            })
        };

        return this.http.request(url_, options_).flatMap((response_: any) => {
            return this.processGetDOEEntity(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processGetDOEEntity(response_);
                } catch (e) {
                    return <Observable<RegistrationDto>><any>Observable.throw(e);
                }
            } else {
                return <Observable<RegistrationDto>><any>Observable.throw(response_);
            }
        });
    }

    protected processGetDOEEntity(response: Response): Observable<RegistrationDto> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? RegistrationDto.fromJS(resultData200) : new RegistrationDto();
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
        return Observable.of<RegistrationDto>(<any>null);
    }

    /**
     * @input (optional) 
     * @return Success
     */
    register(input: PostRegistrationDto): Observable<boolean> {
        let url_ = this.baseUrl + "/api/eswis/Registration/Register";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(input);

        let options_: any = {
            body: content_,
            method: "post",
            headers: new Headers({
                "Content-Type": "application/json",
                "Accept": "application/json"
            })
        };

        return this.http.request(url_, options_).flatMap((response_: any) => {
            return this.processRegister(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processRegister(response_);
                } catch (e) {
                    return <Observable<boolean>><any>Observable.throw(e);
                }
            } else
                return <Observable<boolean>><any>Observable.throw(response_);
        });
    }

    protected processRegister(response: Response): Observable<boolean> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? true : false;
            return Observable.of(result200);
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.text();
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Observable.of<boolean>(<any>null);
    }
}


