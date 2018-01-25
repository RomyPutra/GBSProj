import { state } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { strictEqual } from 'assert';

// export class CreateGroupDto implements ICreateGroupDto {
//     appID: number;
//     groupName: string;
//     status: number;

//     constructor(data?: ICreateGroupDto) {
//         if (data) {
//             for (var property in data) {
//                 if (data.hasOwnProperty(property))
//                     (<any>this)[property] = (<any>data)[property];
//             }
//         }
//     }

//     init(data?: any) {
//         if (data) {
//             this.appID = data['appid'];
//             this.groupName = data['groupName'];
//             this.status = data['status'];
//         }
//     }

//     static fromJS(data: any): CreateGroupDto {
//         let result = new CreateGroupDto();
//         result.init(data);
//         return result;
//     }

//     toJSON(data?: any) {
//         data = typeof data === 'object' ? data : {};
//         data['appid'] = this.appID;
//         data['groupName'] = this.groupName;
//         data['status'] = this.status;
//         return data; 
//     }

//     clone() {
//         const json = this.toJSON();
//         let result = new CreateGroupDto();
//         result.init(json);
//         return result;
//     }
// }

// export interface ICreateGroupDto {
//     appID: number;
//     groupName: string;
//     status: number;
// }

export class GBSDto implements IGBSDto {
    // bookingid: number;
    // recordlocator: string;
    // currencycode: string;
    // departstation: string;
    // arrivalstation: string;
    // flightnum: string;
    // carriercode: string;
    // bookingsum: number;
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
            // this.bookingid = data['bookingID'];
            // this.recordlocator = data['pnr'];
            // this.currencycode = data['currencyCode'];
            // this.departstation = data['departStation'];
            // this.arrivalstation = data['arrivalStation'];
            // this.flightnum = data['flightNum'];
            // this.carriercode = data['carriercode'];
            // this.bookingsum = data['bookingSum'];

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
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        // data['bookingID'] = this.bookingid;
        // data['pnr'] = this.recordlocator;
        // data['currencyCode'] = this.currencycode;
        // data['departStation'] = this.departstation;
        // data['arrivalStation'] = this.arrivalstation;
        // data['flightNum'] = this.flightnum;
        // data['carriercode'] = this.carriercode;
        // data['bookingSum'] = this.bookingsum;
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
    // bookingid: number;
    // recordlocator: string;
    // currencycode: string;
    // departstation: string;
    // arrivalstation: string;
    // flightnum: string;
    // carriercode: string;
    // bookingsum: number;
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
