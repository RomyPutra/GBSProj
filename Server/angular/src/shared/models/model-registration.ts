import { Observable } from 'rxjs/Observable';

export interface IRegistrationDto {
    id: string;
    doeFileNo: string;
    rocNo: string;
    companyName: string;
    isWG: string;
    industryType: string;
    businessType: string;
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
    createDate: Date;
    createBy: string;
    lastUpdate: Date;
    lastUpdateBy: string;
}

export interface IPostRegistrationDto {
    regNo: string;
    companyName: string;
    postalCode: string;
    address1: string;
    address2: string;
    address3: string;
    address4: string;
    country: string;
    state: string;
    pbt: string;
    city: string;
    area: string;
    telNo: string;
    faxNo: string;
    email: string;
    contactPerson: string;
    designation: string;
    contactPersonTelNo: string;
    contactPersonFaxNo: string;
    contactPersonEmail: string;
    nricNo: string;
    userID: string;
    password: string;
    accNo: string;
    companyType: string;
    industryType: string;
    businessType: string;
    createDate: Date;
    createBy: string;
    lastUpdate: Date;
    lastUpdateBy: string;
}

export class RegistrationDto implements IRegistrationDto {
    id: string;
    doeFileNo: string;
    rocNo: string;
    companyName: string;
    isWG: string;
    industryType: string;
    businessType: string;
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
    createDate: Date;
    createBy: string;
    lastUpdate: Date;
    lastUpdateBy: string;

    static fromJS(data: any): RegistrationDto {
        let result = new RegistrationDto();
        result.init(data);
        return result;
    }

    constructor(data?: IRegistrationDto) {
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
            this.id = data['id'];
            this.doeFileNo = data['doeFileNo'];
            this.rocNo = data['rocNo'];
            this.companyName = data['companyName'];
            this.isWG = data['isWG'];
            this.industryType = data['industryType'];
            this.businessType = data['businessType'];
            this.address1 = data['address1'];
            this.address2 = data['address2'];
            this.address3 = data['address3'];
            this.address4 = data['address4'];
            this.postalCode = data['postalCode'];
            this.state = data['state'];
            this.country = data['country'];
            this.pbt = data['pbt'];
            this.city = data['city'];
            this.area = data['area'];
            this.telNo = data['telNo'];
            this.faxNo = data['faxNo'];
            this.email = data['email'];
            this.createDate = data['createDate'];
            this.createBy = data['createBy'];
            this.lastUpdate = data['lastUpdate'];
            this.lastUpdateBy = data['lastUpdateBy'];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data['id'] = this.id;
        data['doeFileNo'] = this.doeFileNo;
        data['rocNo'] = this.rocNo;
        data['companyName'] = this.companyName;
        data['isWG'] = this.isWG;
        data['industryType'] = this.industryType;
        data['businessType'] = this.businessType;
        data['address1'] = this.address1;
        data['address2'] = this.address2;
        data['address3'] = this.address3;
        data['address4'] = this.address4;
        data['postalCode'] = this.postalCode;
        data['state'] = this.state;
        data['country'] = this.country;
        data['pbt'] = this.pbt;
        data['city'] = this.city;
        data['area'] = this.area;
        data['telNo'] = this.telNo;
        data['faxNo'] = this.faxNo;
        data['email'] = this.email;
        data['createDate'] = this.createDate;
        data['createBy'] = this.createBy;
        data['lastUpdate'] = this.lastUpdate;
        data['lastUpdateBy'] = this.lastUpdateBy;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new RegistrationDto();
        result.init(json);
        return result;
    }
}

export class PostRegistrationDto implements IPostRegistrationDto {
    regNo: string;
    companyName: string;
    postalCode: string;
    address1: string;
    address2: string;
    address3: string;
    address4: string;
    country: string;
    state: string;
    pbt: string;
    city: string;
    area: string;
    telNo: string;
    faxNo: string;
    email: string;
    contactPerson: string;
    designation: string;
    contactPersonTelNo: string;
    contactPersonFaxNo: string;
    contactPersonEmail: string;
    nricNo: string;
    userID: string;
    password: string;
    accNo: string;
    companyType: string;
    industryType: string;
    businessType: string;
    createDate: Date;
    createBy: string;
    lastUpdate: Date;
    lastUpdateBy: string;

    static fromJS(data: any): PostRegistrationDto {
        let result = new PostRegistrationDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPostRegistrationDto) {
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
            this.regNo = data['regNo'];
            this.companyName = data['companyName'];
            this.postalCode = data['postalCode'];
            this.address1 = data['address1'];
            this.address2 = data['address2'];
            this.address3 = data['address3'];
            this.address4 = data['address4'];
            this.country = data['country'];
            this.state = data['state'];
            this.pbt = data['pbt'];
            this.city = data['city'];
            this.area = data['area'];
            this.telNo = data['telNo'];
            this.faxNo = data['faxNo'];
            this.email = data['email'];
            this.contactPerson = data['contactPerson'];
            this.designation = data['designation'];
            this.contactPersonTelNo = data['contactPersonTelNo'];
            this.contactPersonFaxNo = data['contactPersonFaxNo'];
            this.contactPersonEmail = data['contactPersonEmail'];
            this.nricNo = data['nricNo'];
            this.userID = data['userID'];
            this.password = data['password'];
            this.accNo = data['accNo'];
            this.companyType = data['companyType'];
            this.industryType = data['industryType'];
            this.businessType = data['businessType'];
            this.createDate = data['createDate'];
            this.createBy = data['createBy'];
            this.lastUpdate = data['lastUpdate'];
            this.lastUpdateBy = data['lastUpdateBy'];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data['regNo'] = this.regNo;
        data['companyName'] = this.companyName;
        data['postalCode'] = this.postalCode;
        data['address1'] = this.address1;
        data['address2'] = this.address2;
        data['address3'] = this.address3;
        data['address4'] = this.address4;
        data['country'] = this.country;
        data['state'] = this.state;
        data['pbt'] = this.pbt;
        data['city'] = this.city;
        data['area'] = this.area;
        data['telNo'] = this.telNo;
        data['faxNo'] = this.faxNo;
        data['email'] = this.email;
        data['contactPerson'] = this.contactPerson;
        data['designation'] = this.designation;
        data['contactPersonTelNo'] = this.contactPersonTelNo;
        data['contactPersonFaxNo'] = this.contactPersonFaxNo;
        data['contactPersonEmail'] = this.contactPersonEmail;
        data['nricNo'] = this.nricNo;
        data['userID'] = this.userID;
        data['password'] = this.password;
        data['accNo'] = this.accNo;
        data['companyType'] = this.companyType;
        data['industryType'] = this.industryType;
        data['businessType'] = this.businessType;
        data['createDate'] = this.createDate;
        data['createBy'] = this.createBy;
        data['lastUpdate'] = this.lastUpdate;
        data['lastUpdateBy'] = this.lastUpdateBy;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new PostRegistrationDto();
        result.init(json);
        return result;
    }
}
