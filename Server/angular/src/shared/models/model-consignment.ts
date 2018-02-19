import { Observable } from 'rxjs/Observable';

export class ConsignmentDto implements IConsignmentDto {
    contractNo: string;
    generatorName: string;
    transporterName: string;
    receiverName: string;
    transDate: Date;
    transportDate: Date;
    targetTransportDate: Date;
    targetReceiveDate: Date;
    receiveDate: Date;
    statusDesc: string;

    static fromJS(data: any): ConsignmentDto {
        let result = new ConsignmentDto();
        result.init(data);
        return result;
    }

    constructor(data?: IConsignmentDto) {
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
                this.contractNo = data['contractNo'];
                this.generatorName = data['generatorName'];
                this.transporterName = data['transporterName'];
                this.receiverName = data['receiverName'];
                this.transDate = data['transDate'];
                this.transportDate = data['transportDate'];
                this.targetTransportDate = data['targetTransportDate'];
                this.targetReceiveDate = data['targetReceiveDate'];
                this.receiveDate = data['receiveDate'];
                this.statusDesc = data['statusDesc'];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data['contractNo'] = this.contractNo;
        data['generatorName'] = this.generatorName;
        data['transporterName'] = this.transporterName;
        data['receiverName'] = this.receiverName;
        data['transDate'] = this.transDate;
        data['transportDate'] = this.transportDate;
        data['targetTransportDate'] = this.targetTransportDate;
        data['targetReceiveDate'] = this.targetReceiveDate;
        data['receiveDate'] = this.receiveDate;
        data['statusDesc'] = this.statusDesc;

        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new ConsignmentDto();
        result.init(json);
        return result;
    }
}

export interface IConsignmentDto {
    contractNo: string;
    generatorName: string;
    transporterName: string;
    receiverName: string;
    transDate: Date;
    transportDate: Date;
    targetTransportDate: Date;
    targetReceiveDate: Date;
    receiveDate: Date;
    statusDesc: string;
}

export class PagedResultDtoOfConsignmentDto implements IPagedResultDtoOfConsignmentDto {
    totalCount: number;
    items: ConsignmentDto[];

    static fromJS(data: any): PagedResultDtoOfConsignmentDto {
        let result = new PagedResultDtoOfConsignmentDto();
        result.init(data);
        return result;
    }

    constructor(data?: IPagedResultDtoOfConsignmentDto) {
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
                    this.items.push(ConsignmentDto.fromJS(item));
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
        let result = new PagedResultDtoOfConsignmentDto();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfConsignmentDto {
    totalCount: number;
    items: ConsignmentDto[];
}
