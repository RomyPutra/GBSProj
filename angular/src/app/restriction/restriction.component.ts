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
    restriction: RestrictionDto;
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
            this.restriction = result;
            this.bookfrom = result.bookfrom;
            this.bookto = result.bookto;
            this.transfrom = result.travelfrom;
            this.transto = result.travelto;
            if (this.restriction.status === '1') {
                this.status = 'Actived';
            } else {
                this.status = 'InActived';
            }
        });
    }

    changeLabel() {
        this.restriction.status = this.restriction.status === '1' ? '0' : '1';
        if (this.restriction.status === '1') {
            this.status = 'Actived';
        } else {
            this.status = 'InActived';
        }
    }
}
