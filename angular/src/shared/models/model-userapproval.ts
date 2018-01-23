import { Observable } from 'rxjs/Observable';

// ============= SHOW LIST ===================

export class UserApprovalDto implements IUserApprovalDto {
    rowID:  string;
    userID: string;
    userType: string;
    keyIndex: string;
    veriKey: string;
    veriCode: string;
    veriType: string;
    status: string
    syncCreate : string;
    syncLastUpd: string;
    lastSyncBy: string;
    requestDate1: string;
    requestDate2: string;
    active: string;
    remark: string;
    deviceBrand: string;
    rejectRemark: string;
    companyName: string;
    bizRegID: string;
    statusDesc: string;
    regNo: string;
    accNo: string;

    static fromJS(data: any): UserApprovalDto {
        let result = new UserApprovalDto();
        result.init(data);
        return result;
    }

    constructor(data?: IUserApprovalDto) {
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
            this.rowID= data['rowID'];
            this.userID= data['userID'];
            this.userType= data['userType'];
            this.keyIndex= data['keyIndex'];
            this.veriKey= data['veriKey'];
            this.veriCode= data['veriCode'];
            this.veriType= data['veriType'];          
            this.status= data['status'];
            this.syncCreate = data['syncCreate'];
            this.syncLastUpd= data['syncLastUpd'];
            this.lastSyncBy= data['lastSyncBy'];
            this.requestDate1= data['requestDate1'];
            this.requestDate2= data['requestDate2'];
            this.active= data['active'];
            this.remark= data['remark'];
            this.deviceBrand= data['deviceBrand'];
            this.rejectRemark= data['rejectRemark'];
            this.companyName= data['companyName'];
            this.bizRegID= data['bizRegID'];
            this.statusDesc= data['statusDesc'];
            this.regNo= data['regNo'];
            this.accNo= data['accNo'];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
            data['rowID'] =this.rowID;
            data['userID'] =this.userID;
            data['userType'] = this.userType;
            data['keyIndex'] = this.keyIndex;
            data['veriKey'] = this.veriKey;
            data['veriCode'] = this.veriCode;
            data['veriType'] = this.veriType;        
            data['status'] = this.status;
            data['syncCreate'] = this.syncCreate;
            data['syncLastUpd'] = this.syncLastUpd;
            data['lastSyncBy'] = this.lastSyncBy;
            data['requestDate1'] = this.requestDate1;
            data['requestDate2'] = this.requestDate2;
            data['active'] = this.active;
            data['remark']= this.remark;
            data['deviceBrand'] = this.deviceBrand;
            data['rejectRemark'] = this.rejectRemark;
            data['companyName'] = this.companyName;
            data['bizRegID'] = this.bizRegID;
            data['statusDesc'] = this.statusDesc;
            data['regNo'] =  this.regNo;
            data['accNo'] = this.accNo;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new UserApprovalDto();
        result.init(json);
        return result;
    }
}

export interface IUserApprovalDto {
    rowID:  string;
    userID: string;
    userType: string;
    keyIndex: string;
    veriKey: string;
    veriCode: string;
    veriType: string;
    status: string
    syncCreate : string;
    syncLastUpd: string;
    lastSyncBy: string;
    requestDate1: string;
    requestDate2: string;
    active: string;
    remark: string;
    deviceBrand: string;
    rejectRemark: string;
    companyName: string;
    bizRegID: string;
    statusDesc: string;
    regNo: string;
    accNo: string;
}

export class PagedResultDtoOfUserApprovalDto implements IPagedResultDtoOfUserApprovalDto {
    totalCount: number;
    items: UserApprovalDto[];

    static fromJS(data: any): PagedResultDtoOfUserApprovalDto {
        let result = new PagedResultDtoOfUserApprovalDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfUserApprovalDto) {
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
                    this.items.push(UserApprovalDto.fromJS(item));
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
        let result = new PagedResultDtoOfUserApprovalDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfUserApprovalDto {
    totalCount: number;
    items: UserApprovalDto[];
}

export class ListResultDtoOfUserApprovalDto implements IListResultDtoOfUserApprovalDto {
    items: UserApprovalDto[];

    constructor(data?: IListResultDtoOfUserApprovalDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            if (data["items"] && data["items"].constructor === Array) {
                this.items = [];
                for (let item of data["items"])
                    this.items.push(UserApprovalDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): ListResultDtoOfUserApprovalDto {
        let result = new ListResultDtoOfUserApprovalDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        if (this.items && this.items.constructor === Array) {
            data["items"] = [];
            for (let item of this.items)
                data["items"].push(item.toJSON());
        }
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new ListResultDtoOfUserApprovalDto();
        result.init(json);
        return result;
    }
}

export interface IListResultDtoOfUserApprovalDto {
    items: UserApprovalDto[];
}

// ======================= EDIT STATUS FROM LIST

