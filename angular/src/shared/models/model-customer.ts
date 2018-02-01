import { Observable } from 'rxjs/Observable';

export interface ICustomerDto {
    bizRegID: string;
    companyName: string;
    companyType: string;
    industryType: string;
    businessType: string;
    regNo: string;
    accNo: string;
    address1: string;
    address2: string;
    address3: string;
    address4: string;
    postalCode: string;
    state: string;
    country: string;
    pbt: string;
    city: string;
    area: string;
    telNo: string;
    faxNo: string;
    email: string;
    coWebsite: string;
    contactPerson: string;
    contactDesignation: string;
    contactPersonEmail: string;
    contactPersonTelNo: string;
    contactPersonFaxNo: string;
    contactPersonMobile: string;
    remark: string;
    active: number;
    inUse: number;
    createDate: Date;
    createBy: string;
    lastUpdate: Date;
    updateBy: string;
    rowguid: string;
    flag: number;
    bizGrp: number;
    status: number;
    stateDesc: string;
    pbtDesc: string;
    cityDesc: string;
    areaDesc: string;
    countryDesc: string;
    designation: string;
    industryTypeDesc: string;
    businessTypeDesc: string;
    refID: string;
    kkm: number;
    kkmDesc: string;
    activeDesc: string;
}

export class CustomerDto implements ICustomerDto {
    bizRegID: string;
    companyName: string;
    companyType: string;
    industryType: string;
    businessType: string;
    regNo: string;
    accNo: string;
    address1: string;
    address2: string;
    address3: string;
    address4: string;
    postalCode: string;
    state: string;
    country: string;
    pbt: string;
    city: string;
    area: string;
    telNo: string;
    faxNo: string;
    email: string;
    coWebsite: string;
    contactPerson: string;
    contactDesignation: string;
    contactPersonEmail: string;
    contactPersonTelNo: string;
    contactPersonFaxNo: string;
    contactPersonMobile: string;
    remark: string;
    active: number;
    inUse: number;
    createDate: Date;
    createBy: string;
    lastUpdate: Date;
    updateBy: string;
    rowguid: string;
    flag: number;
    bizGrp: number;
    status: number;
    stateDesc: string;
    pbtDesc: string;
    cityDesc: string;
    areaDesc: string;
    countryDesc: string;
    designation: string;
    industryTypeDesc: string;
    businessTypeDesc: string;
    refID: string;
    kkm: number;
    kkmDesc: string;
    activeDesc: string;

    static fromJS(data: any): CustomerDto {
        let result = new CustomerDto();
        result.init(data);
        return result;
    }

    constructor(data?: ICustomerDto) {
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
            this.bizRegID = data["bizRegID"];
            this.companyName = data["companyName"];
            this.companyType = data["companyType"];
            this.industryType = data["industryType"];
            this.businessType = data["businessType"];
            this.regNo = data["regNo"];
            this.accNo = data["accNo"];
            this.address1 = data["address1"];
            this.address2 = data["address2"];
            this.address3 = data["address3"];
            this.address4 = data["address4"];
            this.postalCode = data["postalCode"];
            this.state = data["state"];
            this.country = data["country"];
            this.pbt = data["pbt"];
            this.city = data["city"];
            this.area = data["area"];
            this.telNo = data["telNo"];
            this.faxNo = data["faxNo"];
            this.email = data["email"];
            this.coWebsite = data["coWebsite"];
            this.contactPerson = data["contactPerson"];
            this.contactDesignation = data["contactDesignation"];
            this.contactPersonEmail = data["contactPersonEmail"];
            this.contactPersonTelNo = data["contactPersonTelNo"];
            this.contactPersonFaxNo = data["contactPersonFaxNo"];
            this.contactPersonMobile = data["contactPersonMobile"];
            this.remark = data["remark"];
            this.active = data["active"];
            this.inUse = data["inUse"];
            this.createDate = data["createDate"];
            this.createBy = data["createBy"];
            this.lastUpdate = data["lastUpdate"];
            this.updateBy = data["updateBy"];
            this.rowguid = data["rowguid"];
            this.flag = data["flag"];
            this.bizGrp = data["bizGrp"];
            this.status = data["status"];
            this.stateDesc = data["stateDesc"];
            this.pbtDesc = data["pbtDesc"];
            this.cityDesc = data["cityDesc"];
            this.areaDesc = data["areaDesc"];
            this.countryDesc = data["countryDesc"];
            this.designation = data["designation"];
            this.industryTypeDesc = data["industryTypeDesc"];
            this.businessTypeDesc = data["businessTypeDesc"];
            this.refID = data["refID"];
            this.kkm = data["kkm"];
            this.kkmDesc = data["kkmDesc"];
            this.activeDesc = data["activeDesc"];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["bizRegID"] = this.bizRegID;
        data["companyName"] = this.companyName;
        data["companyType"] = this.companyType;
        data["industryType"] = this.industryType;
        data["businessType"] = this.businessType;
        data["regNo"] = this.regNo;
        data["accNo"] = this.accNo;
        data["address1"] = this.address1;
        data["address2"] = this.address2;
        data["address3"] = this.address3;
        data["address4"] = this.address4;
        data["postalCode"] = this.postalCode;
        data["state"] = this.state;
        data["country"] = this.country;
        data["pbt"] = this.pbt;
        data["city"] = this.city;
        data["area"] = this.area;
        data["telNo"] = this.telNo;
        data["faxNo"] = this.faxNo;
        data["email"] = this.email;
        data["coWebsite"] = this.coWebsite;
        data["contactPerson"] = this.contactPerson;
        data["contactDesignation"] = this.contactDesignation;
        data["contactPersonEmail"] = this.contactPersonEmail;
        data["contactPersonTelNo"] = this.contactPersonTelNo;
        data["contactPersonFaxNo"] = this.contactPersonFaxNo;
        data["contactPersonMobile"] = this.contactPersonMobile;
        data["remark"] = this.remark;
        data["active"] = this.active;
        data["inuse"] = this.inUse;
        data["createDate"] = this.createDate;
        data["createBy"] = this.createBy;
        data["lastUpdate"] = this.lastUpdate;
        data["updateBy"] = this.updateBy;
        data["rowguid"] = this.rowguid;
        data["flag"] = this.flag;
        data["bizGrp"] = this.bizGrp;
        data["status"] = this.status;
        data["stateDesc"] = this.stateDesc;
        data["pbtDesc"] = this.pbtDesc;
        data["cityDesc"] = this.cityDesc;
        data["areaDesc"] = this.areaDesc;
        data["countryDesc"] = this.countryDesc;
        data["designation"] = this.designation;
        data["industryTypeDesc"] = this.industryTypeDesc;
        data["businessTypeDesc"] = this.businessTypeDesc;
        data["refID"] = this.refID;
        data["kkm"] = this.kkm;
        data["kkmDesc"] = this.kkmDesc;
        data["activeDesc"] = this.activeDesc;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new CustomerDto();
        result.init(json);
        return result;
    }
}

export class PagedResultDtoOfCustomerDto implements IPagedResultDtoOfCustomerDto {
    totalCount: number;
    items: CustomerDto[];

    static fromJS(data: any): PagedResultDtoOfCustomerDto {
        let result = new PagedResultDtoOfCustomerDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfCustomerDto) {
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
                    this.items.push(CustomerDto.fromJS(item));
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
        let result = new PagedResultDtoOfCustomerDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfCustomerDto {
    totalCount: number;
    items: CustomerDto[];
}
