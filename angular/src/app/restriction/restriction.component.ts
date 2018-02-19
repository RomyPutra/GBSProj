import { DatePipe } from '@angular/common';
import { Component, Injector, ViewChild, OnInit } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { ModalDirective } from 'ngx-bootstrap';
import { GetRestrictionServiceProxy } from '@shared/service-proxies/proxy-restriction';
import { RestrictionDto } from '@shared/models/model-restriction';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
    templateUrl: './restriction.component.html',
    animations: [appModuleAnimation()]
})
export class RestrictionComponent extends AppComponentBase implements OnInit {
    restric: RestrictionDto;
    saving: boolean = false;
    saved: RestrictionDto;
    bookfrom: Date;
    bookto: Date;
    transfrom: Date;
    transto: Date;
    status: string;
    constructor(
        injector: Injector,
        private _restrictionService: GetRestrictionServiceProxy
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this._restrictionService.getRestriction()
        .subscribe((result) => {
            this.restric = result;
            this.bookfrom = new Date(result.bookfrom);
            this.bookto = new Date(result.bookto);
            this.transfrom = new Date(result.travelfrom);
            this.transto = new Date(result.travelto);
            if (this.restric.status === '1') {
                this.status = 'Actived';
            } else {
                this.status = 'InActived';
            }
        });
    }

    changeLabel() {
        this.restric.status = this.restric.status === '1' ? '0' : '1';
        if (this.restric.status === '1') {
            this.status = 'Actived';
        } else {
            this.status = 'InActived';
        }
    }
    save(): void {
        this.restric.bookfrom = this.bookfrom.getFullYear() + '-' + this.bookfrom.getMonth() + '-' + this.bookfrom.getDate();
        this.restric.bookto = this.bookto.getFullYear() + '-' + this.bookto.getMonth() + '-' + this.bookto.getDate();
        this.restric.travelfrom = this.transfrom.getFullYear() + '-' + this.transfrom.getMonth() + '-' + this.transfrom.getDate();
        this.restric.travelto = this.transto.getFullYear() + '-' + this.transto.getMonth() + '-' + this.transto.getDate();
        this._restrictionService.update(this.restric)
        // this.saving = true;
        // this._groupService.update(this.groups)
            .finally(() => { this.saving = false; })
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
            });
    }
    close(): void {
        this._restrictionService.getRestriction()
        .subscribe((result) => {
            this.restric = result;
            this.bookfrom = new Date(result.bookfrom);
            this.bookto = new Date(result.bookto);
            this.transfrom = new Date(result.travelfrom);
            this.transto = new Date(result.travelto);
            if (this.restric.status === '1') {
                this.status = 'Actived';
            } else {
                this.status = 'InActived';
            }
        });
    }
}
