import { Observable } from 'rxjs/Observable';

export interface ILocationDto {
    bizLocID: string;
    bizRegID: string;
    branchName: string;
    branchCode: string;
    industryType: string;
    businessType: string;
    accNo: string;
    address1: string;
    address2: string;
    address3: string;
    address4: string;
    postalCode: string;
    contactPerson: string;
    contactDesignation: string;
    contactEmail: string;
    contactTelNo: string;
    contactMobile: string;
    storeType: string;
    email: string;
    tel: string;
    fax: string;
    region: string;
    country: string;
    state: string;
    pBT: string;
    city: string;
    area: string;
    currency: string;
    storeStatus: number;
    opPrevBook: number;
    opTimeStart: string;
    opTimeEnd: string;
    opDay1: number;
    opDay2: number;
    opDay3: number;
    opDay4: number;
    opDay5: number;
    opDay6: number;
    opDay7: number;
    opBookAlwDY: number;
    opBookAlwHR: number;
    opBookFirst: string;
    opBookLast: string;
    opBookIntv: number;
    salesItemType: string;
    inSvcItemType: string;
    genInSvcID: number;
    rcpHeader: string;
    rcpFooter: string;
    priceLevel: number;
    isStockTake: number;
    active: number;
    inuse: number;
    createDate: Date;
    isHost: string;
    lastUpdate: Date;
    updateBy: string;
    syncCreate: Date;
    syncLastUpd: Date;
    flag: number;
    bankAccount: string;
    branchType: string;
    createBy: string;
    stateDesc: string;
    pbtDesc: string;
    cityDesc: string;
    areaDesc: string;
    countryDesc: string;
    designation: string;
    industrytypeDesc: string;
    refID: string;
}

export class LocationDto implements ILocationDto {
    bizLocID: string;
    bizRegID: string;
    branchName: string;
    branchCode: string;
    industryType: string;
    businessType: string;
    accNo: string;
    address1: string;
    address2: string;
    address3: string;
    address4: string;
    postalCode: string;
    contactPerson: string;
    contactDesignation: string;
    contactEmail: string;
    contactTelNo: string;
    contactMobile: string;
    storeType: string;
    email: string;
    tel: string;
    fax: string;
    region: string;
    country: string;
    state: string;
    pBT: string;
    city: string;
    area: string;
    currency: string;
    storeStatus: number;
    opPrevBook: number;
    opTimeStart: string;
    opTimeEnd: string;
    opDay1: number;
    opDay2: number;
    opDay3: number;
    opDay4: number;
    opDay5: number;
    opDay6: number;
    opDay7: number;
    opBookAlwDY: number;
    opBookAlwHR: number;
    opBookFirst: string;
    opBookLast: string;
    opBookIntv: number;
    salesItemType: string;
    inSvcItemType: string;
    genInSvcID: number;
    rcpHeader: string;
    rcpFooter: string;
    priceLevel: number;
    isStockTake: number;
    active: number;
    inuse: number;
    createDate: Date;
    isHost: string;
    lastUpdate: Date;
    updateBy: string;
    syncCreate: Date;
    syncLastUpd: Date;
    flag: number;
    bankAccount: string;
    branchType: string;
    createBy: string;
    stateDesc: string;
    pbtDesc: string;
    cityDesc: string;
    areaDesc: string;
    countryDesc: string;
    designation: string;
    industrytypeDesc: string;
    refID: string;

    static fromJS(data: any): LocationDto {
        let result = new LocationDto();
        result.init(data);
        return result;
    }

    constructor(data?: ILocationDto) {
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
            this.bizLocID = data["bizLocID"];
            this.bizRegID = data["bizRegID"];
            this.branchName = data["branchName"];
            this.branchCode = data["branchCode"];
            this.industryType = data["industryType"];
            this.businessType = data["businessType"];
            this.accNo = data["accNo"];
            this.address1 = data["address1"];
            this.address2 = data["address2"];
            this.address3 = data["address3"];
            this.address4 = data["address4"];
            this.postalCode = data["postalCode"];
            this.contactPerson = data["contactPerson"];
            this.contactDesignation = data["contactDesignation"];
            this.contactEmail = data["contactEmail"];
            this.contactTelNo = data["contactTelNo"];
            this.contactMobile = data["contactMobile"];
            this.storeType = data["storeType"];
            this.email = data["email"];
            this.tel = data["tel"];
            this.fax = data["fax"];
            this.region = data["region"];
            this.country = data["country"];
            this.state = data["state"];
            this.pBT = data["pBT"];
            this.city = data["city"];
            this.area = data["area"];
            this.currency = data["currency"];
            this.storeStatus = data["storeStatus"];
            this.opPrevBook = data["opPrevBook"];
            this.opTimeStart = data["opTimeStart"];
            this.opTimeEnd = data["opTimeEnd"];
            this.opDay1 = data["opDay1"];
            this.opDay2 = data["opDay2"];
            this.opDay3 = data["opDay3"];
            this.opDay4 = data["opDay4"];
            this.opDay5 = data["opDay5"];
            this.opDay6 = data["opDay6"];
            this.opDay7 = data["opDay7"];
            this.opBookAlwDY = data["opBookAlwDY"];
            this.opBookAlwHR = data["opBookAlwHR"];
            this.opBookFirst = data["opBookFirst"];
            this.opBookLast = data["opBookLast"];
            this.opBookIntv = data["opBookIntv"];
            this.salesItemType = data["salesItemType"];
            this.inSvcItemType = data["inSvcItemType"];
            this.genInSvcID = data["genInSvcID"];
            this.rcpHeader = data["rcpHeader"];
            this.rcpFooter = data["rcpFooter"];
            this.priceLevel = data["priceLevel"];
            this.isStockTake = data["isStockTake"];
            this.active = data["active"];
            this.inuse = data["inuse"];
            this.createDate = data["createDate"];
            this.isHost = data["isHost"];
            this.lastUpdate = data["lastUpdate"];
            this.updateBy = data["updateBy"];
            this.syncCreate = data["syncCreate"];
            this.syncLastUpd = data["syncLastUpd"];
            this.flag = data["flag"];
            this.bankAccount = data["bankAccount"];
            this.branchType = data["branchType"];
            this.createBy = data["createBy"];
            this.stateDesc = data["stateDesc"];
            this.pbtDesc = data["pbtDesc"];
            this.cityDesc = data["cityDesc"];
            this.areaDesc = data["areaDesc"];
            this.countryDesc = data["countryDesc"];
            this.designation = data["designation"];
            this.industrytypeDesc = data["industrytypeDesc"];
            this.refID = data["refID"];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["bizLocID"] = this.bizLocID;
        data["bizRegID"] = this.bizRegID;
        data["branchName"] = this.branchName;
        data["branchCode"] = this.branchCode;
        data["industryType"] = this.industryType;
        data["businessType"] = this.businessType;
        data["accNo"] = this.accNo;
        data["address1"] = this.address1;
        data["address2"] = this.address2;
        data["address3"] = this.address3;
        data["address4"] = this.address4;
        data["postalCode"] = this.postalCode;
        data["contactPerson"] = this.contactPerson;
        data["contactDesignation"] = this.contactDesignation;
        data["contactEmail"] = this.contactEmail;
        data["contactTelNo"] = this.contactTelNo;
        data["contactMobile"] = this.contactMobile;
        data["storeType"] = this.storeType;
        data["email"] = this.email;
        data["tel"] = this.tel;
        data["fax"] = this.fax;
        data["region"] = this.region;
        data["country"] = this.country;
        data["state"] = this.state;
        data["pBT"] = this.pBT;
        data["city"] = this.city;
        data["area"] = this.area;
        data["currency"] = this.currency;
        data["storeStatus"] = this.storeStatus;
        data["opPrevBook"] = this.opPrevBook;
        data["opTimeStart"] = this.opTimeStart;
        data["opTimeEnd"] = this.opTimeEnd;
        data["opDay1"] = this.opDay1;
        data["opDay2"] = this.opDay2;
        data["opDay3"] = this.opDay3;
        data["opDay4"] = this.opDay4;
        data["opDay5"] = this.opDay5;
        data["opDay6"] = this.opDay6;
        data["opDay7"] = this.opDay7;
        data["opBookAlwDY"] = this.opBookAlwDY;
        data["opBookAlwHR"] = this.opBookAlwHR;
        data["opBookFirst"] = this.opBookFirst;
        data["opBookLast"] = this.opBookLast;
        data["opBookIntv"] = this.opBookIntv;
        data["salesItemType"] = this.salesItemType;
        data["inSvcItemType"] = this.inSvcItemType;
        data["genInSvcID"] = this.genInSvcID;
        data["rcpHeader"] = this.rcpHeader;
        data["rcpFooter"] = this.rcpFooter;
        data["priceLevel"] = this.priceLevel;
        data["isStockTake"] = this.isStockTake;
        data["active"] = this.active;
        data["inuse"] = this.inuse;
        data["createDate"] = this.createDate;
        data["isHost"] = this.isHost;
        data["lastUpdate"] = this.lastUpdate;
        data["updateBy"] = this.updateBy;
        data["syncCreate"] = this.syncCreate;
        data["syncLastUpd"] = this.syncLastUpd;
        data["flag"] = this.flag;
        data["bankAccount"] = this.bankAccount;
        data["branchType"] = this.branchType;
        data["createBy"] = this.createBy;
        data["stateDesc"] = this.stateDesc;
        data["pbtDesc"] = this.pbtDesc;
        data["cityDesc"] = this.cityDesc;
        data["areaDesc"] = this.areaDesc;
        data["countryDesc"] = this.countryDesc;
        data["designation"] = this.designation;
        data["industrytypeDesc"] = this.industrytypeDesc;
        data["refID"] = this.refID;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new LocationDto();
        result.init(json);
        return result;
    }
}

export class PagedResultDtoOfLocationDto implements IPagedResultDtoOfLocationDto {
    totalCount: number;
    items: LocationDto[];

    static fromJS(data: any): PagedResultDtoOfLocationDto {
        let result = new PagedResultDtoOfLocationDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfLocationDto) {
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
            this.totalCount = data["totalCount"];
            if (data["items"] && data["items"].constructor === Array) {
                this.items = [];
                for (let item of data["items"]) {
                    this.items.push(LocationDto.fromJS(item));
                }
            }
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["totalCount"] = this.totalCount;
        if (this.items && this.items.constructor === Array) {
            data["items"] = [];
            for (let item of this.items) {
                data["items"].push(item.toJSON());
            }
        }
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new PagedResultDtoOfLocationDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfLocationDto {
    totalCount: number;
    items: LocationDto[];
}