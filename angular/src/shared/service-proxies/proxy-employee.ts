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
import { EmployeeDto, PagedResultDtoOfEmployeeDto, CreateEmployeeDto } from './../models/model-employee';
import { AppConsts } from '@shared/AppConsts';
import { BaseServiceProxy, throwException } from './service-proxy-base';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class EmployeeServiceProxy extends BaseServiceProxy {

    constructor(@Inject(Http) http: Http, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        super(http, AppConsts.remoteServiceBaseUrl);
    }

    /**
     * @input (optional)
     * @return Success
     */
    create(input: CreateEmployeeDto): Observable<boolean> {
        let url_ = this.baseUrl + '/api/eswis/Employee/Create';
        url_ = url_.replace(/[?&]$/, '');
        
        //const content_ = JSON.stringify(input);
        const content_ = input.toJSON();
        

        let options_ : any = {
            body: content_,
            method: 'post',
            headers: new Headers({
                'Content-Type': 'application/json',
                'Accept': 'application/json'
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
            } else {
                return <Observable<boolean>><any>Observable.throw(response_);
            }
        });
    }

    protected processCreate(response: Response): Observable<boolean> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? true : false;
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
        return Observable.of<boolean>(<any>null);
    }

    /**
     * @input (optional)
     * @return Success
     */
    update(input: CreateEmployeeDto): Observable<boolean> {
        let url_ = this.baseUrl + '/api/eswis/Employee/Update';
        url_ = url_.replace(/[?&]$/, '');

        //const content_ = JSON.stringify(input);
        const content_ = input.toJSON();

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
                    return <Observable<boolean>><any>Observable.throw(e);
                }
            } else {
                return <Observable<boolean>><any>Observable.throw(response_);
            }
        });
    }

    protected processUpdate(response: Response): Observable<boolean> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? true : false;
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
        return Observable.of<boolean>(<any>null);
    }

    /**
     * @return Success
     */
    delete(input: EmployeeDto): Observable<boolean> {
        let url_ = this.baseUrl + '/api/eswis/Employee/Delete';
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

        return this.http.request(url_, options_).flatMap((response_: any) => {
            return this.processDelete(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processDelete(response_);
                } catch (e) {
                    return <Observable<boolean>><any>Observable.throw(e);
                }
            } else {
                return <Observable<boolean>><any>Observable.throw(response_);
            }
        });
    }

    protected processDelete(response: Response): Observable<boolean> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? true : false;
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
        return Observable.of<boolean>(<any>null);
    }

    /**
     * @return Success
     */
    get(employeeID: string): Observable<EmployeeDto> {
        let url_ = this.baseUrl + "/api/eswis/Employee/Get?";
        if (employeeID === undefined || employeeID === null) {
            throw new Error("The parameter 'employeeID' must be defined and cannot be null.");
        } else {
            url_ += "employeeID=" + encodeURIComponent("" + employeeID) + "&";
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
                    return <Observable<EmployeeDto>><any>Observable.throw(e);
                }
            } else {
                return <Observable<EmployeeDto>><any>Observable.throw(response_);
            }
        });
    }

    protected processGet(response: Response): Observable<EmployeeDto> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? EmployeeDto.fromJS(resultData200) : new EmployeeDto();
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
        return Observable.of<EmployeeDto>(<any>null);
    }

    /**
     * @return Success
     */
    getAllEmployeeProfile(flag: string, companyID: string, coState: string,
        designation: string, stats: string): Observable<PagedResultDtoOfEmployeeDto> {
        let url_ = this.baseUrl + '/api/eswis/Employee/GetEmployeeProfileList?';

        if (flag === undefined || flag === null) {
            // throw new Error("The parameter 'skipCount' must be defined and cannot be null.");
        } else {
            url_ += 'flag=' + encodeURIComponent('' + flag) + '&';
        }

        if (companyID === undefined || companyID === null) {
            // throw new Error("The parameter 'skipCount' must be defined and cannot be null.");
        } else {
            url_ += 'companyID=' + encodeURIComponent('' + companyID) + '&';
        }

        if (coState === undefined || coState === null) {
            // throw new Error("The parameter 'skipCount' must be defined and cannot be null.");
        } else {
            url_ += 'coState=' + encodeURIComponent('' + coState) + '&';
        }

        if (designation === undefined || designation === null) {
            // throw new Error("The parameter 'skipCount' must be defined and cannot be null.");
        } else {
            url_ += 'designation=' + encodeURIComponent('' + designation) + '&';
        }

        if (stats === undefined || stats === null) {
            // throw new Error("The parameter 'skipCount' must be defined and cannot be null.");
        } else {
            url_ += 'stats=' + encodeURIComponent('' + stats) + '&';
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
                    return <Observable<PagedResultDtoOfEmployeeDto>><any>Observable.throw(e);
                }
            } else {
                return <Observable<PagedResultDtoOfEmployeeDto>><any>Observable.throw(response_);
            }
        });
    }

    protected processGetAll(response: Response): Observable<PagedResultDtoOfEmployeeDto> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfEmployeeDto.fromJS(resultData200) : new PagedResultDtoOfEmployeeDto();
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
        return Observable.of<PagedResultDtoOfEmployeeDto>(<any>null);
    }

    /**
     * @return Success
     */
    getListEntity(skipCount: number, maxResultCount: number, companyID: string): Observable<PagedResultDtoOfEmployeeDto> {
        let url_ = this.baseUrl + '/api/eswis/Employee/GetEmployeeListEntity?';
        if (skipCount === undefined || skipCount === null)
            throw new Error("The parameter 'skipCount' must be defined and cannot be null.");
        else
            url_ += "skipCount=" + encodeURIComponent("" + skipCount) + "&";

        if (maxResultCount === undefined || maxResultCount === null)
            throw new Error("The parameter 'maxResultCount' must be defined and cannot be null.");
        else
            url_ += "limit=" + encodeURIComponent("" + maxResultCount) + "&";  
        
        if (companyID === undefined || companyID === null)
            throw new Error("The parameter 'companyID' must be defined and cannot be null.");
        else
            url_ += "companyID=" + encodeURIComponent("" + companyID) + "&";
    
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
                    return <Observable<PagedResultDtoOfEmployeeDto>><any>Observable.throw(e);
                }
            } else {
                return <Observable<PagedResultDtoOfEmployeeDto>><any>Observable.throw(response_);
            }
        });
    }

    protected processGetListEntity(response: Response): Observable<PagedResultDtoOfEmployeeDto> {
        const status = response.status; 

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfEmployeeDto.fromJS(resultData200) : new PagedResultDtoOfEmployeeDto();
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
        return Observable.of<PagedResultDtoOfEmployeeDto>(<any>null);
    }
}

