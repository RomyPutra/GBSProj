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
    test: any[] = [];
    
    constructor(
        injector: Injector,
        private _restrictionService: GetRestrictionServiceProxy
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this._restrictionService.getRestriction()
        .subscribe((result) => {
            this.test = result.items;
        });
    }

    //save(): void {
    //    this.saving = true;
    //    this.country.createDate = new Date();
    //    this.country.createBy = "SYSTEM";
    //    this.country.flag = 1;
    //    this._countryService.insert(this.country)
    //        .finally(() => { this.saving = false; })
    //        .subscribe((result) => {
    //            if (result) {
    //                this.notify.info(this.l('SavedSuccessfully'));
    //                this.close();
    //                this.modalSave.emit(null);
    //            } else {
    //                this.notify.error('Save failed!');
    //            }
    //    });
    //}

    //close(): void {
    //    this.active = false;
    //    this.modal.hide();
    //}

    //changeLabel() {
    //    this.country.active = this.country.active === 1 ? 0 : 1;
    //}

    //changeSelect() {
    //    let temp = this.selectedAccessCode;
    //}

    //onChangeCategorySelect(event) {
    //    this.selectedAccessCode = event.target.value;

        // if (value == 'add') {
        //     alert("add new cat");
        //     this.model.category = 'default';
        // }

        // //event.target.value = this.model.category;

        // this.model.subcategory = 'default';
    //}
}
