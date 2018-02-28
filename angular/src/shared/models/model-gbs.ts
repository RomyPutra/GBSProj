import { state } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { strictEqual } from 'assert';

export class GBSCountryDto implements IGBSCountryDto {
    countryID: string;
    countryName: string;
    currencyCode: string;

    static fromJS(data: any): GBSCountryDto {
        let result = new GBSCountryDto();
        result.init(data);
        return result;
    }

    constructor(data?: IGBSCountryDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property)) {
                    (<any>this)[property] = (<any>data)[property];
                }
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.countryID = data['countryCode'];
            this.countryName = data['countryName'];
            this.currencyCode = data['currencyCode'];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data['countryCode'] = this.countryID;
        data['countryName'] = this.countryName;
        data['currencyCode'] = this.currencyCode;
    return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new GBSCountryDto();
        result.init(json);
        return result;
    }
}

export interface IGBSCountryDto {
    countryID: string;
    countryName: string;
    currencyCode: string;
}

export class PagedResultDtoOfGBSCountryDto implements IPagedResultDtoOfGBSCountryDto {
    totalCount: number;
    items: GBSCountryDto[];

    static fromJS(data: any): PagedResultDtoOfGBSCountryDto {
        let result = new PagedResultDtoOfGBSCountryDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfGBSCountryDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property)) {
                    (<any>this)[property] = (<any>data)[property];
                }
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.totalCount = data['totalCount'];
            if (data['items'] && data['items'].constructor === Array) {
                this.items = [];
                for (let item of data['items']) {
                    this.items.push(GBSCountryDto.fromJS(item));
                }
            }
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data['totalCount'] = this.totalCount;
        if (this.items && this.items.constructor === Array) {
            data['items'] = [];
            for (let item of this.items) {
                data['items'].push(item.toJSON());
            }
        }
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new PagedResultDtoOfGBSCountryDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfGBSCountryDto {
    totalCount: number;
    items: GBSCountryDto[];
}

export class GBSDto implements IGBSDto {
    grpid: string;
    schemeCode: string;
    countryCode: string;
    currencyCode: string;
    duration: number;
    minduration: number;
    paymentType: string;
    attempt_1: number;
    code_1: string;
    percentage_1: number;
    attempt_2: number;
    code_2: string;
    percentage_2: number;
    attempt_3: number;
    code_3: string;
    percentage_3: number;
    minDeposit: number;
    maxDeposit: number;
    minDeposit2: number;
    maxDeposit2: number;
    depositValue: number;
    // prevCountry: string;


    static fromJS(data: any): GBSDto {
        let result = new GBSDto();
        result.init(data);
        return result;
    }

    constructor(data?: IGBSDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property)) {
                    (<any>this)[property] = (<any>data)[property];
                }
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.grpid = data['grpid'];
            this.schemeCode = data['schemeCode'];
            this.countryCode = data['countryCode'];
            this.currencyCode = data['currencyCode'];
            this.duration = data['duration'];
            this.minduration = data['minduration'];
            this.paymentType = data['paymentType'];
            this.attempt_1 = data['attempt_1'];
            this.code_1 = data['code_1'];
            this.percentage_1 = data['percentage_1'];
            this.attempt_2 = data['attempt_2'];
            this.code_2 = data['code_2'];
            this.percentage_2 = data['percentage_2'];
            this.attempt_3 = data['attempt_3'];
            this.code_3 = data['code_3'];
            this.percentage_3 = data['percentage_3'];
            this.minDeposit = data['minDeposit'];
            this.maxDeposit = data['maxDeposit'];
            this.minDeposit2 = data['minDeposit2'];
            this.maxDeposit2 = data['maxDeposit2'];
            this.depositValue = data['depositValue'];
            // this.prevCountry = data['prevCountry'];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data['grpid'] = this.grpid;
        data['schemeCode'] = this.schemeCode;
        data['countryCode'] = this.countryCode;
        data['currencyCode'] = this.currencyCode;
        data['duration'] = this.duration;
        data['minduration'] = this.minduration;
        data['paymentType'] = this.paymentType;
        data['attempt_1'] = this.attempt_1;
        data['code_1'] = this.code_1;
        data['percentage_1'] = this.percentage_1;
        data['attempt_2'] = this.attempt_2;
        data['code_2'] = this.code_2;
        data['percentage_2'] = this.percentage_2;
        data['attempt_3'] = this.attempt_3;
        data['code_3'] = this.code_3;
        data['percentage_3'] = this.percentage_3;
        data['minDeposit'] = this.minDeposit;
        data['maxDeposit'] = this.maxDeposit;
        data['minDeposit2'] = this.minDeposit2;
        data['maxDeposit2'] = this.maxDeposit2;
        data['depositValue'] = this.depositValue;
        // data['prevCountry'] = this.prevCountry;
    return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new GBSDto();
        result.init(json);
        return result;
    }
}

export interface IGBSDto {
    grpid: string;
    schemeCode: string;
    countryCode: string;
    currencyCode: string;
    duration: number;
    minduration: number;
    paymentType: string;
    attempt_1: number;
    code_1: string;
    percentage_1: number;
    attempt_2: number;
    code_2: string;
    percentage_2: number;
    attempt_3: number;
    code_3: string;
    percentage_3: number;
    minDeposit: number;
    maxDeposit: number;
    minDeposit2: number;
    maxDeposit2: number;
    depositValue: number;
    // prevCountry: string;
}

export class PagedResultDtoOfGBSDto implements IPagedResultDtoOfGBSDto {
    totalCount: number;
    items: GBSDto[];

    static fromJS(data: any): PagedResultDtoOfGBSDto {
        let result = new PagedResultDtoOfGBSDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfGBSDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property)) {
                    (<any>this)[property] = (<any>data)[property];
                }
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.totalCount = data['totalCount'];
            if (data['items'] && data['items'].constructor === Array) {
                this.items = [];
                for (let item of data['items']) {
                    this.items.push(GBSDto.fromJS(item));
                }
            }
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data['totalCount'] = this.totalCount;
        if (this.items && this.items.constructor === Array) {
            data['items'] = [];
            for (let item of this.items) {
                data['items'].push(item.toJSON());
            }
        }
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new PagedResultDtoOfGBSDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfGBSDto {
    totalCount: number;
    items: GBSDto[];
}

export class FlightTimeDto implements IFlightTimeDto {
	ftGroupCode: string;
	startTime: string;
	endTime: string;
	syncCreate: string;
	syncLastUpd: string;
	createDate: string;
	updateDate: string;
    lastSyncBy: string;
	createBy: string;
	updateBy: string;
	active: number;

    static fromJS(data: any): FlightTimeDto {
        let result = new FlightTimeDto();
        result.init(data);
        return result;
    }

    constructor(data?: FlightTimeDto, dataFT?: any) {
        var today = new Date();
        if (data === dataFT) {
            for (var property in data) {
                if (data.hasOwnProperty(property)) {
                    (<any>this)[property] = (<any>data)[property];
                }
            }
        } else {
                this.ftGroupCode = dataFT[0];
                this.startTime = dataFT[1];
                this.endTime = dataFT[2];
    			this.syncCreate = today.getFullYear() + '-' + (today.getMonth()+1) + '-' + today.getDate();
    			this.syncLastUpd = today.getFullYear() + '-' + (today.getMonth()+1) + '-' + today.getDate();
    			this.createDate = today.getFullYear() + '-' + (today.getMonth()+1) + '-' + today.getDate();
    			this.updateDate = today.getFullYear() + '-' + (today.getMonth()+1) + '-' + today.getDate();
    			this.lastSyncBy = '';
    			this.createBy = '';
    			this.updateBy = '';
    			this.active = 1;
        }
    }

    init(data?: any) {
        if (data) {
			this.ftGroupCode = data['ftGroupCode'];
			this.startTime = data['startTime'];
			this.endTime = data['endTime'];
			this.syncCreate = data['syncCreate'];
			this.syncLastUpd = data['syncLastUpd'];
			this.createDate = data['createDate'];
			this.updateDate = data['updateDate'];
			this.lastSyncBy = data['lastSyncBy'];
			this.createBy = data['createBy'];
			this.updateBy = data['updateBy'];
			this.active = data['active'];
        }
    }

    toJSON(data?: any) {
        if(data['0']){
            data = typeof data === 'object' ? data : {};
			data['ftGroupCode'] = this.ftGroupCode;
			data['startTime'] = this.startTime;
            data['endTime'] = this.endTime;
            // console.log(data);
        } else {
			data['ftGroupCode'] = this.ftGroupCode;
			data['startTime'] = this.startTime;
            data['endTime'] = this.endTime;
            data['syncCreate'] = this.syncCreate;
            data['syncLastUpd'] = this.syncLastUpd;
            data['createDate'] = this.createDate;
            data['updateDate'] = this.updateDate;
            data['lastSyncBy'] = this.lastSyncBy;
            data['createBy'] = this.createBy;
            data['updateBy'] = this.updateBy;
            data['active'] = this.active;    
        }
		return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new FlightTimeDto();
        result.init(json);
        return result;
    }
}

export interface IFlightTimeDto {
	ftGroupCode: string;
	startTime: string;
	endTime: string;
	syncCreate: string;
	syncLastUpd: string;
	createDate: string;
	updateDate: string;
	lastSyncBy: string;
	createBy: string;
	updateBy: string;
	active: number;
}

export class PagedResultDtoOfFlightTimeDto implements IPagedResultDtoOfFlightTimeDto {
    totalCount: number;
    items: FlightTimeDto[];

    static fromJS(data: any): PagedResultDtoOfFlightTimeDto {
        let result = new PagedResultDtoOfFlightTimeDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfFlightTimeDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property)) {
                    (<any>this)[property] = (<any>data)[property];
                }
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.totalCount = data['totalCount'];
            if (data['items'] && data['items'].constructor === Array) {
                this.items = [];
                for (let item of data['items']) {
                    this.items.push(FlightTimeDto.fromJS(item));
                }
            }
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data['totalCount'] = this.totalCount;
        if (this.items && this.items.constructor === Array) {
            data['items'] = [];
            for (let item of this.items) {
                data['items'].push(item.toJSON());
            }
        }
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new PagedResultDtoOfFlightTimeDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfFlightTimeDto {
    totalCount: number;
    items: FlightTimeDto[];
}

