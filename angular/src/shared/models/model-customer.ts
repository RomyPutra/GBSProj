import { Observable } from 'rxjs/Observable';

export class CreateCustomerDto implements ICreateCustomerDto {
    companyName: string;
    companyType: string;
    industryType: string;
    businessType: string;
    address1: string;
    postalCode: string;
    country: string;
    state: string;
    status: number;
    telNo: string;
    faxNo: string;
    email: string;

    constructor(data?: ICreateCustomerDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.companyName = data["companyName"];
            this.companyType = data["companyType"];
            this.industryType = data["industryType"];
            this.businessType = data["businessType"];
            this.address1 = data["address1"];
            this.postalCode = data["postalCode"];
            this.country = data["country"];
            this.state = data["state"];
            this.status = data["status"];
            this.telNo = data["telNo"];
            this.faxNo = data["faxNo"];
            this.email = data["email"];
        }
    }

    static fromJS(data: any): CreateCustomerDto {
        let result = new CreateCustomerDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["companyName"] = this.companyName;
        data["companyType"] = this.companyType;
        data["industryType"] = this.industryType;
        data["businessType"] = this.businessType;
        data["address1"] = this.address1;
        data["postalCode"] = this.postalCode;
        data["country"] = this.country;
        data["state"] = this.state;
        data["status"] = this.status;
        data["telNo"] = this.telNo;
        data["faxNo"] = this.faxNo;
        data["email"] = this.email;
        return data; 
    }

    clone() {
        const json = this.toJSON();
        let result = new CreateCustomerDto();
        result.init(json);
        return result;
    }
}

export interface ICreateCustomerDto {
    companyName: string;
    companyType: string;
    industryType: string;
    businessType: string;
    address1: string;
    postalCode: string;
    country: string;
    state: string;
    status: number;
    telNo: string;
    faxNo: string;
    email: string;
}

export class CustomerDto implements ICustomerDto {
    bizRegID: string;
    companyName: string;
    companyType: string;
    industryType: string;
    businessType: string;
    address1: string;
    postalCode: string;
    country: string;
    state: string;
    status: number;
    telNo: string;
    faxNo: string;
    email: string;

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
            this.bizRegID = data['bizRegID'];
            this.companyName = data["companyName"];
            this.companyType = data["companyType"];
            this.industryType = data["industryType"];
            this.businessType = data["businessType"];
            this.address1 = data["address1"];
            this.postalCode = data["postalCode"];
            this.country = data["country"];
            this.state = data["state"];
            this.status = data["status"];
            this.telNo = data["telNo"];
            this.faxNo = data["faxNo"];
            this.email = data["email"];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data['bizRegID'] = this.bizRegID;
        data["companyName"] = this.companyName;
        data["companyType"] = this.companyType;
        data["industryType"] = this.industryType;
        data["businessType"] = this.businessType;
        data["address1"] = this.address1;
        data["postalCode"] = this.postalCode;
        data["country"] = this.country;
        data["state"] = this.state;
        data["status"] = this.status;
        data["telNo"] = this.telNo;
        data["faxNo"] = this.faxNo;
        data["email"] = this.email;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new CustomerDto();
        result.init(json);
        return result;
    }
}

export interface ICustomerDto {
    bizRegID: string;
    companyName: string;
    companyType: string;
    industryType: string;
    businessType: string;
    address1: string;
    postalCode: string;
    country: string;
    state: string;
    status: number;
    telNo: string;
    faxNo: string;
    email: string;
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
