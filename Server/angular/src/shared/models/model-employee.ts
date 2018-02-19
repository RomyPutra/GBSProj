import { Observable } from 'rxjs/Observable';

export interface IEmployeeBranchDto {
    employeeID: string;
    inAppt: number;
    isServer: number;
    isHost: number;
    locID: string;
}

export class EmployeeBranchDto implements IEmployeeBranchDto {
    employeeID: string;
    inAppt: number;
    isServer: number;
    isHost: number;
    locID: string;

    static fromJS(data: any): EmployeeBranchDto {
        let result = new EmployeeBranchDto();
        result.init(data);
        return result;
    }

    constructor(data?: IEmployeeBranchDto) {
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
            this.employeeID = data['employeeID'];
            this.inAppt = data['inAppt'];
            this.isServer = data['isServer'];
            this.isHost = data['isHost'];
            this.locID = data['locID'];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data['employeeID'] = this.employeeID  ;
        data['inAppt'] = this.inAppt;
        data['isServer'] = this.isServer;
        data['isHost'] = this.isHost;
        data['locID'] = this.locID ;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new EmployeeBranchDto();
        result.init(json);
        return result;
    }
}

export interface ICreateEmployeeDto {
    password: string;
    appID: number;
    empData: EmployeeDto;
    // empBranches: EmployeeBranchDto[];
}

export class CreateEmployeeDto implements ICreateEmployeeDto {
    password: string;
    appID: number;
    empData: EmployeeDto;
    empBranches: EmployeeBranchDto[];

    constructor(data?: ICreateEmployeeDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property)) {
                    (<any>this)[property] = (<any>data)[property];
                }
            }
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data['password'] = this.password  ;
        data['appID'] = this.appID;
        data['empData'] = this.empData;
        data['empBranches'] = this.empBranches;
        return data;
    }

}

export interface IEmployeeDto {
    employeeID: string;
    accNo: string;
    branchName: string;
    nickName: string;
    surName: string;
    firstName: string;
    salutation: string;
    sex: string;
    dob: Date;
    placeOfBirth: string;
    nricNo: string;
    nationality: string;
    race: string;
    religion: string;
    marital: string;
    coAddress1: string;
    coAddress2: string;
    coAddress3: string;
    coAddress4: string;
    coPostalCode: string;
    coState: string;
    coCountry: string;
    pnAddress1: string;
    pnAddress2: string;
    pnAddress3: string;
    pnAddress4: string;
    pnPostalCode: string;
    pnState: string;
    pnCountry: string;
    emerContactPerson: string;
    emerContactNo: string;
    emailAddress: string;
    designation: string;
    foreignLocal: string;
    commID: string;
    salary: number;
    offDay: number;
    overtime: number;
    leave: number;
    levy: number;
    allergies: string;
    clerkNo: string;
    dateHired: Date;
    dateLeft: Date;
    transportAllowance: string;
    serviceAllowance: string;
    otherAllowance: string;
    remarks: string;
    privilegeCode: string;
    status: number;
    createDate: Date;
    createBy: string;
    lastUpdate: Date;
    updateBy: string;
    inUse: number;
    rowguid: string;
    syncCreate: Date;
    syncLastUpd: Date;
    isHost: number;
    lastSyncBy: string;
    accessLvl: number;
    companyID: string;
    locID: string;
    flag: number;
    userID: string;
    remark: string;
    statusApprove: string;
    device: string;
    deviceID: string;
    brand: string;
    fromTransporter: number;
    statusAdd: number;
    state: string;
    officerName: string;
    lastLogin: string;
    accessCode: string;
    password: string;
}

export class EmployeeDto implements IEmployeeDto {
    employeeID: string;
    accNo: string;
    branchName: string;
    nickName: string;
    surName: string;
    firstName: string;
    salutation: string;
    sex: string;
    dob: Date;
    placeOfBirth: string;
    nricNo: string;
    nationality: string;
    race: string;
    religion: string;
    marital: string;
    coAddress1: string;
    coAddress2: string;
    coAddress3: string;
    coAddress4: string;
    coPostalCode: string;
    coState: string;
    coCountry: string;
    pnAddress1: string;
    pnAddress2: string;
    pnAddress3: string;
    pnAddress4: string;
    pnPostalCode: string;
    pnState: string;
    pnCountry: string;
    emerContactPerson: string;
    emerContactNo: string;
    emailAddress: string;
    designation: string;
    foreignLocal: string;
    commID: string;
    salary: number;
    offDay: number;
    overtime: number;
    leave: number;
    levy: number;
    allergies: string;
    clerkNo: string;
    dateHired: Date;
    dateLeft: Date;
    transportAllowance: string;
    serviceAllowance: string;
    otherAllowance: string;
    remarks: string;
    privilegeCode: string;
    status: number;
    createDate: Date;
    createBy: string;
    lastUpdate: Date;
    updateBy: string;
    inUse: number;
    rowguid: string;
    syncCreate: Date;
    syncLastUpd: Date;
    isHost: number;
    lastSyncBy: string;
    accessLvl: number;
    companyID: string;
    locID: string;
    flag: number;
    userID: string;
    remark: string;
    statusApprove: string;
    device: string;
    deviceID: string;
    brand: string;
    fromTransporter: number;
    statusAdd: number;
    state: string;
    officerName: string;
    lastLogin: string;
    accessCode: string;
    password: string;

    static fromJS(data: any): EmployeeDto {
        let result = new EmployeeDto();
        result.init(data);
        return result;
    }

    constructor(data?: IEmployeeDto) {
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
            this.employeeID = data["employeeID"];
            this.accNo = data["accNo"];
            this.branchName = data["branchName"];
            this.nickName = data["nickName"];
            this.surName = data["surName"];
            this.firstName = data["firstName"];
            this.salutation = data["salutation"];
            this.sex = data["sex"];
            this.dob = data["dob"];
            this.placeOfBirth = data["placeOfBirth"];
            this.nricNo = data["nricNo"];
            this.nationality = data["nationality"];
            this.race = data["race"];
            this.religion = data["religion"];
            this.marital = data["marital"];
            this.coAddress1 = data["coAddress1"];
            this.coAddress2 = data["coAddress2"];
            this.coAddress3 = data["coAddress3"];
            this.coAddress4 = data["coAddress4"];
            this.coPostalCode = data["coPostalCode"];
            this.coState = data["coState"];
            this.coCountry = data["coCountry"];
            this.pnAddress1 = data["pnAddress1"];
            this.pnAddress2 = data["pnAddress2"];
            this.pnAddress3 = data["pnAddress3"];
            this.pnAddress4 = data["pnAddress4"];
            this.pnPostalCode = data["pnPostalCode"];
            this.pnState = data["pnState"];
            this.pnCountry = data["pnCountry"];
            this.emerContactPerson = data["emerContactPerson"];
            this.emerContactNo = data["emerContactNo"];
            this.emailAddress = data["emailAddress"];
            this.designation = data["designation"];
            this.foreignLocal = data["foreignLocal"];
            this.commID = data["commID"];
            this.salary = data["salary"];
            this.offDay = data["offDay"];
            this.overtime = data["overtime"];
            this.leave = data["leave"];
            this.levy = data["levy"];
            this.allergies = data["allergies"];
            this.clerkNo = data["clerkNo"];
            this.dateHired = data["dateHired"];
            this.dateLeft = data["dateLeft"];
            this.transportAllowance = data["transportAllowance"];
            this.serviceAllowance = data["serviceAllowance"];
            this.otherAllowance = data["otherAllowance"];
            this.remarks = data["remarks"];
            this.privilegeCode = data["privilegeCode"];
            this.status = data["status"];
            this.createDate = data["createDate"];
            this.createBy = data["createBy"];
            this.lastUpdate = data["lastUpdate"];
            this.updateBy = data["updateBy"];
            this.inUse = data["inUse"];
            this.rowguid = data["rowguid"];
            this.syncCreate = data["syncCreate"];
            this.syncLastUpd = data["syncLastUpd"];
            this.isHost = data["isHost"];
            this.lastSyncBy = data["lastSyncBy"];
            this.accessLvl = data["accessLvl"];
            this.companyID = data["companyID"];
            this.locID = data["locID"];
            this.flag = data["flag"];
            this.userID = data["userID"];
            this.remark = data["remark"];
            this.statusApprove = data["statusApprove"];
            this.device = data["device"];
            this.deviceID = data["deviceID"];
            this.brand = data["brand"];
            this.fromTransporter = data["fromTransporter"];
            this.statusAdd = data["statusAdd"];
            this.state = data["state"];
            this.officerName = data["officerName"];
            this.lastLogin = data["lastLogin"];
            this.accessCode = data["accessCode"];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["employeeID"] = this.employeeID;
        data["accNo"] = this.accNo;
        data["branchName"] = this.branchName;
        data["nickName"] = this.nickName;
        data["surName"] = this.surName;
        data["firstName"] = this.firstName;
        data["salutation"] = this.salutation;
        data["sex"] = this.sex;
        data["dob"] = this.dob;
        data["placeOfBirth"] = this.placeOfBirth;
        data["nricNo"] = this.nricNo
        data["nationality"] = this.nationality;
        data["race"] = this.race;
        data["religion"] = this.religion;
        data["marital"] = this.marital;
        data["coAddress1"] = this.coAddress1;
        data["coAddress2"] = this.coAddress2;
        data["coAddress3"] = this.coAddress3;
        data["coAddress4"] = this.coAddress4;
        data["coPostalCode"] = this.coPostalCode;
        data["coState"] = this.coState;
        data["coCountry"] = this.coCountry;
        data["pnAddress1"] = this.pnAddress1;
        data["pnAddress2"] = this.pnAddress2;
        data["pnAddress3"] = this.pnAddress3;
        data["pnAddress4"] = this.pnAddress4;
        data["pnPostalCode"] = this.pnPostalCode;
        data["pnState"] = this.pnState;
        data["pnCountry"] = this.pnCountry;
        data["emerContactPerson"] = this.emerContactPerson;
        data["emerContactNo"] = this.emerContactNo;
        data["emailAddress"] = this.emailAddress;
        data["designation"] = this.designation;
        data["foreignLocal"] = this.foreignLocal;
        data["commID"] = this.commID;
        data["salary"] = this.salary;
        data["offDay"] = this.offDay;
        data["overtime"] = this.overtime;
        data["leave"] = this.leave;
        data["levy"] = this.levy;
        data["allergies"] = this.allergies;
        data["clerkNo"] = this.clerkNo;
        data["dateHired"] = this.dateHired;
        data["dateLeft"] = this.dateLeft;
        data["transportAllowance"] = this.transportAllowance;
        data["serviceAllowance"] = this.serviceAllowance;
        data["otherAllowance"] = this.otherAllowance;
        data["remarks"] = this.remarks;
        data["privilegeCode"] = this.privilegeCode;
        data["status"] = this.status;
        data["createDate"] = this.createDate;
        data["createBy"] = this.createBy;
        data["lastUpdate"] = this.lastUpdate;
        data["updateBy"] = this.updateBy;
        data["inUse"] = this.inUse;
        data["rowguid"] = this.rowguid;
        data["syncCreate"] = this.syncCreate;
        data["syncLastUpd"] = this.syncLastUpd;
        data["isHost"] = this.isHost;
        data["lastSyncBy"] = this.lastSyncBy;
        data["accessLvl"] = this.accessLvl;
        data["companyID"] = this.companyID;
        data["locID"] = this.locID;
        data["flag"] = this.flag;
        data["userID"] = this.userID;
        data["remark"] = this.remark;
        data["statusApprove"] = this.statusApprove;
        data["device"] = this.device;
        data["deviceID"] = this.deviceID;
        data["brand"] = this.brand;
        data["fromTransporter"] = this.fromTransporter;
        data["statusAdd"] = this.statusAdd;
        data["state"] = this.state;
        data["officerName"] = this.officerName;
        data["lastLogin"] = this.lastLogin;
        data["accessCode"] = this.accessCode;
        data["password"] = this.password;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new EmployeeDto();
        result.init(json);
        return result;
    }
}

export class PagedResultDtoOfEmployeeDto implements IPagedResultDtoOfEmployeeDto {
    totalCount: number;
    items: EmployeeDto[];

    static fromJS(data: any): PagedResultDtoOfEmployeeDto {
        let result = new PagedResultDtoOfEmployeeDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfEmployeeDto) {
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
                    this.items.push(EmployeeDto.fromJS(item));
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
        let result = new PagedResultDtoOfEmployeeDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfEmployeeDto {
    totalCount: number;
    items: EmployeeDto[];
}
