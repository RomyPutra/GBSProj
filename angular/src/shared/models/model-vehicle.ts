import { Observable } from 'rxjs/Observable';

export interface IVehicleDto {
    vehicleID: string;
    regNo: string;
    ownership: string;
    companyID: string;
    companyDesc: string;
    vehicleType: string;
    bdm: string;
    manufacturingYear: string;
    registrationYear: string;
    gpsInstallation: string;
    binLifterInstallation: string;
    truckPainting: string;
    registrationCard: string;
    photos: string;
    status: string;
    createDate: string;
    createBy: string;
    lastUpdate: string;
    updateBy: string;
    inuse: string;
    rowguid: string;
    syncCreate: string;
    syncLastUpd: string;
    isHost: string;
    lastSyncBy: string;
    flag: string;
    fromTransporter: string;
    statusAdd: string;
}

export class VehicleDto implements IVehicleDto {
    vehicleID: string;
    regNo: string;
    ownership: string;
    companyID: string;
    companyDesc: string;
    vehicleType: string;
    bdm: string;
    manufacturingYear: string;
    registrationYear: string;
    gpsInstallation: string;
    binLifterInstallation: string;
    truckPainting: string;
    registrationCard: string;
    photos: string;
    status: string;
    createDate: string;
    createBy: string;
    lastUpdate: string;
    updateBy: string;
    inuse: string;
    rowguid: string;
    syncCreate: string;
    syncLastUpd: string;
    isHost: string;
    lastSyncBy: string;
    flag: string;
    fromTransporter: string;
    statusAdd: string;

    static fromJS(data: any): VehicleDto {
        let result = new VehicleDto();
        result.init(data);
        return result;
    }

    constructor(data?: IVehicleDto) {
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
            this.vehicleID = data["vehicleID"];
            this.regNo = data["regNo"];
            this.ownership = data["ownership"];
            this.companyID = data["companyID"];
            this.companyDesc = data["companyDesc"];
            this.vehicleType = data["vehicleType"];
            this.bdm = data["bdm"];
            this.manufacturingYear = data["manufacturingYear"];
            this.registrationYear = data["registrationYear"];
            this.gpsInstallation = data["gpsInstallation"];
            this.binLifterInstallation = data["binLifterInstallation"];
            this.truckPainting = data["truckPainting"];
            this.registrationCard = data["registrationCard"];
            this.photos = data["photos"];
            this.status = data["status"];
            this.createDate = data["createDate"];
            this.createBy = data["createBy"];
            this.lastUpdate = data["lastUpdate"];
            this.updateBy = data["updateBy"];
            this.inuse = data["inuse"];
            this.rowguid = data["rowguid"];
            this.syncCreate = data["syncCreate"];
            this.syncLastUpd = data["syncLastUpd"];
            this.isHost = data["isHost"];
            this.lastSyncBy = data["lastSyncBy"];
            this.flag = data["flag"];
            this.fromTransporter = data["fromTransporter"];
            this.statusAdd = data["statusAdd"];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["vehicleID"] =  this.vehicleID;
        data["regNo"] =  this.regNo;
        data["ownership"] =  this.ownership;
        data["companyID"] =  this.companyID;
        data["companyDesc"] =  this.companyDesc;
        data["vehicleType"] =  this.vehicleType;
        data["bdm"] =  this.bdm;
        data["manufacturingYear"] =  this.manufacturingYear;
        data["registrationYear"] =  this.registrationYear;
        data["gpsInstallation"] =  this.gpsInstallation;
        data["binLifterInstallation"] =  this.binLifterInstallation;
        data["truckPainting"] =  this.truckPainting;
        data["registrationCard"] =  this.registrationCard;
        data["photos"] =  this.photos;
        data["status"] =  this.status;
        data["createDate"] =  this.createDate;
        data["createBy"] =  this.createBy;
        data["lastUpdate"] =  this.lastUpdate;
        data["updateBy"] =  this.updateBy;
        data["inuse"] =  this.inuse;
        data["rowguid"] =  this.rowguid;
        data["syncCreate"] =  this.syncCreate;
        data["syncLastUpd"] =  this.syncLastUpd;
        data["isHost"] =  this.isHost;
        data["lastSyncBy"] =  this.lastSyncBy;
        data["flag"] =  this.flag;
        data["fromTransporter"] =  this.fromTransporter;
        data["statusAdd"] =  this.statusAdd;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new VehicleDto();
        result.init(json);
        return result;
    }
}

export class PagedResultDtoOfVehicleDto implements IPagedResultDtoOfVehicleDto {
    totalCount: number;
    items: VehicleDto[];

    static fromJS(data: any): PagedResultDtoOfVehicleDto {
        let result = new PagedResultDtoOfVehicleDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfVehicleDto) {
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
                    this.items.push(VehicleDto.fromJS(item));
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
        let result = new PagedResultDtoOfVehicleDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfVehicleDto {
    totalCount: number;
    items: VehicleDto[];
}