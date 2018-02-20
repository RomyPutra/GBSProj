import { Observable } from 'rxjs/Observable';

export interface IUploadDto {
    name: string;
    size: string;
    file: File;
    excel: any;
    isUploaded: boolean;
}

export class UploadDto implements IUploadDto {
    name: string;
    size: string;
    file: File;
    excel: any;
    isUploaded: boolean;

    static fromJS(data: any): UploadDto {
        let result = new UploadDto();
        result.init(data);
        return result;
    }

    constructor(data?: IUploadDto) {
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
            this.name = data['name'];
            this.size = data['size'];
            this.file = data['file'];
            this.excel = data['excel'];
            this.isUploaded = data['isUploaded'];
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data['name'] = this.name;
        data['size'] = this.size;
        data['file'] = this.file;
        data['excel'] = this.excel;
        data['isUploaded'] = this.isUploaded;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new UploadDto();
        result.init(json);
        return result;
    }
}

export class UploadPendingRecvDto {
    hasError = false;
    errorColumns: any[] = [];

    column0: string;
    column1: string;
    column2: string;
    column3: string;
    column4: string;
    column5: number;
    column6: number;
    column7: number;
    column8: Date;
    column9: Date;
    column10: string;
    column11: string;
    column12: string;

    static fromJS(data: any): UploadPendingRecvDto {
        let result = new UploadPendingRecvDto();
        result.init(data);
        return result;
    }

    constructor(data?: any[]) {
        if (data) {
            if (data[0]) {
                this.column0 = data[0];
            }

            if (data[1]) {
                this.column1 = data[1];
            } else {
                this.isMandatory(1);
            }

            if (data[2]) {
                this.column2 = data[2];
            }

            if (data[3]) {
                this.column3 = data[3];
            } else {
                this.isMandatory(3);
            }

            if (data[4]) {
                this.column4 = data[4];
            }

            if (data[5]) {
                this.column5 = this.isNumber(data[5], 5);
            } else {
                this.isMandatory(5);
            }

            if (data[6]) {
                this.column6 = this.isNumber(data[6], 6);
            } else {
                this.isMandatory(6);
            }

            if (data[7]) {
                this.column7 = this.isNumber(data[7], 7);
            }

            if (data[8]) {
                this.column8 = this.isDate(data[8], 8);
            } else {
                this.isMandatory(8);
            }

            if (data[9]) {
                this.column9 = this.isDate(data[9], 9);
            }

            if (data[10]) {
                this.column10 = data[10];
            }

            if (data[11]) {
                this.column11 = data[11];
            } else {
                this.isMandatory(11);
            }

            if (data[12]) {
                this.column12 = data[12];
            } else {
                this.isMandatory(12);
            }
        }
    }

    isMandatory(index?: number) {
        this.hasError = true;
        this.errorColumns.push(index);
    }

    isNumber(value?: any, index?: number): number {
        if (Number(value)) {
            return Number(value);
        } else {
            this.hasError = true;
            this.errorColumns.push(index);
            return null;
        }
    }

    isDate(value?: any, index?: number): Date {
        if (Date.parse(value)) {
            return new Date(Date.parse(value));
        }  else {
            this.hasError = true;
            this.errorColumns.push(index);
            return null;
        }
    }

    init(data?: any) {
        if (data) {
            this.hasError = false;
            this.errorColumns = [];

            this.column0 = '';
            this.column1 = '';
            this.column2 = '';
            this.column3 = '';
            this.column4 = '';
            this.column5 = null;
            this.column6 = null;
            this.column7 = null;
            this.column8 = null;
            this.column9 = null;
            this.column10 = '';
            this.column11 = '';
            this.column12 = '';
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data['hasError'] = this.hasError;
        data['errorList'] = this.errorColumns;

        data['column0'] = this.column0;
        data['column1'] = this.column1;
        data['column2'] = this.column2;
        data['column3'] = this.column3;
        data['column4'] = this.column4;
        data['column5'] = this.column5;
        data['column6'] = this.column6;
        data['column7'] = this.column7;
        data['column8'] = this.column8;
        data['column9'] = this.column9;
        data['column10'] = this.column10;
        data['column11'] = this.column11;
        data['column12'] = this.column12;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new UploadPendingRecvDto();
        result.init(json);
        return result;
    }
}

export class UploadFlightTimeDto {
    hasError = false;
    errorColumns: any[] = [];

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

    static fromJS(data: any): UploadFlightTimeDto {
        let result = new UploadFlightTimeDto();
        result.init(data);
        return result;
    }

    constructor(data?: any[]) {
        if (data) {
            if (data[0]) {
                this.ftGroupCode = data[0];
            } else {
                this.isMandatory(1);
            }

            if (data[1]) {
                this.startTime = data[1];
            } else {
                this.isMandatory(1);
            }

            if (data[2]) {
                this.endTime = data[2];
            } else {
                this.isMandatory(1);
            }
			this.syncCreate = '';
			this.syncLastUpd = '';
			this.createDate = '';
			this.updateDate = '';
			this.lastSyncBy = '';
			this.createBy = '';
			this.updateBy = '';
			this.active = 1;
        }
    }

    isMandatory(index?: number) {
        this.hasError = true;
        this.errorColumns.push(index);
    }

    isNumber(value?: any, index?: number): number {
        if (Number(value)) {
            return Number(value);
        } else {
            this.hasError = true;
            this.errorColumns.push(index);
            return null;
        }
    }

    isDate(value?: any, index?: number): Date {
        if (Date.parse(value)) {
            return new Date(Date.parse(value));
        }  else {
            this.hasError = true;
            this.errorColumns.push(index);
            return null;
        }
    }

    init(data?: any) {
        if (data) {
            this.hasError = false;
            this.errorColumns = [];

			this.ftGroupCode = '';
			this.startTime = '';
			this.endTime = '';
			this.syncCreate = '';
			this.syncLastUpd = '';
			this.createDate = '';
			this.updateDate = '';
			this.lastSyncBy = '';
			this.createBy = '';
			this.updateBy = '';
			this.active = 0;
        }
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data['hasError'] = this.hasError;
        data['errorList'] = this.errorColumns;

		data['endTime'] = this.endTime;
		data['syncCreate'] = this.syncCreate;
		data['syncLastUpd'] = this.syncLastUpd;
		data['createDate'] = this.createDate;
		data['updateDate'] = this.updateDate;
		data['lastSyncBy'] = this.lastSyncBy;
		data['createBy'] = this.createBy;
		data['updateBy'] = this.updateBy;
		data['active'] = this.active;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new UploadFlightTimeDto();
        result.init(json);
        return result;
    }
}
