import { Component, Injector, ViewChild, OnInit } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { UnitDto, PagedResultDtoOfUnitDto } from '@shared/models/model-unit';
import { StateDto, PagedResultDtoOfStateDto } from '@shared/models/model-state';
import { UnitServiceProxy } from '@shared/service-proxies/service-proxies';
import { StateServiceProxy } from '@shared/service-proxies/proxy-state';
import { PagedRequestDto } from 'shared/paged-listing-component-base';
import { PagedListingComponentCustom } from 'shared/layout/paged-listing-component-custom';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UploadServiceProxy } from '@shared/service-proxies/proxy-upload';

@Component({
    templateUrl: './units.component.html',
    animations: [appModuleAnimation()]
})
// export class UnitsComponent implements OnInit {
export class UnitsComponent extends PagedListingComponentCustom<UnitDto> implements OnInit {
    firstFormGroup: FormGroup;
    secondFormGroup: FormGroup;
    active: boolean = false;
    units: UnitDto[] = [];
    state: StateDto[] = [];
    fileToUpload: File = null;

    constructor(
        injector: Injector,
        private _formBuilder: FormBuilder,
        private _userService: UnitServiceProxy,
        private _stateService: StateServiceProxy,
        private _uploadService: UploadServiceProxy
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.refresh();
        this.firstFormGroup = this._formBuilder.group({
            firstCtrl: ['', Validators.required]
          });
          this.secondFormGroup = this._formBuilder.group({
            secondCtrl: ['', Validators.required]
          });
    }

    handleFileInput(files: FileList) {
        this.fileToUpload = files.item(0);
    }

    uploadFileToActivity() {
        this._uploadService.post(this.fileToUpload)
            .finally(() => {

            })
            .subscribe(result => {
                let temp = result;
            });
    }

    protected refresh(): void {
        this.isTableLoading = false;
        this._userService.getAll(0, 0)
            .finally(() => {
                this.isTableLoading = true;
            })
            .subscribe((result: PagedResultDtoOfUnitDto) => {
                this.units = result.items;
            });
        this._stateService.getAll(0, 0)
            .finally(() => {
                this.isTableLoading = false;
            })
            .subscribe((result: PagedResultDtoOfStateDto) => {
                this.state = result.items;
            });
    }

    protected delete(unit: UnitDto): void {
        abp.message.confirm(
            "Delete unit '" + unit.uomCode + "'?",
            (result: boolean) => {
                if (result) {
                    //   this._userService.delete(user.id)
                    //       .subscribe(() => {
                    //           abp.notify.info("Deleted User: " + user.fullName);
                    //           this.refresh();
                    //       });
                }
            }
        );
    }

    // Show Modals
    createUnit(): void {
        //   this.createUserModal.show();
    }

    editUnit(user: UnitDto): void {
        //   this.editUserModal.show(user.id);
    }
}
