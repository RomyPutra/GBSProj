import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Component, Injector, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import { AppComponentBase } from '@shared/app-component-base';

import { CustomerServiceProxy } from '@shared/service-proxies/proxy-customer';

import { CustomerDto } from '@shared/models/model-customer';

import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { AbstractControl } from '@angular/forms/src/model';

@Component({
    templateUrl: './voucher-create-edit.component.html',
    animations: [appModuleAnimation()]
})
export class VoucherCreateEditComponent extends AppComponentBase implements OnInit {

    saving: boolean;
    isLoading: boolean;
    bizRegID: string;
    fgVoucher: FormGroup;

    // #region Define objects
    // customer: CustomerDto = new CustomerDto();
    // location: LocationDto[] = [];
    // employee: EmployeeDto[] = [];
    // vehicle: VehicleDto[] = [];
    // userprofile: UserProfileDto[] = [];
    // isSelectedKKM: boolean;
    // roles: CodeMasterDto[] = [];
    // selectedRoles: any;
    // countries: CountryDto[] = [];
    // selectedCountry: string;
    // states: StateDto[] = [];
    // selectedState: string;
    // pbts: PBTDto[] = [];
    // selectedPBT: string;
    // cities: CityDto[] = [];
    // selectedCity: string;
    // areas: AreaDto[] = [];
    // selectedArea: string;
    // designations: CodeMasterDto[] = [];
    // #endregion

    constructor(
        injector: Injector,
        private _route: ActivatedRoute,
        private _formBuilder: FormBuilder,
        private _service: CustomerServiceProxy,
    ) {
        super(injector);
        // this.bizRegID = _route.snapshot.params.bizRegID;
        this.loadingMessage = 'Loading...';
    }

    ngOnInit(): void {
        // this.isLoading = true;
        this._populateSelect();

        // #region Define angular material form group and validation rules
        this.fgVoucher = this._formBuilder.group({
            promoCode: ['', Validators.required],
            promoTitle: ['', Validators.required],
            promoDesc: ['', Validators.required],
            promoType: ['', Validators.required],
            store: ['', Validators.required],
            voucherActivation: ['', Validators.required],
            voucherExpired: ['', Validators.required],
            // promoDesc: [],
            // inputAddress2: [],
            // inputAddress3: [],
            // inputAddress4: [],
            // inputPostCode: ['', Validators.compose([Validators.pattern('[0-9]*'), Validators.required])],
            // inputCountry: ['', Validators.required],
            // inputState: ['', Validators.required],
            // inputPBT: ['', Validators.required],
            // inputDistrict: ['', Validators.required],
            // inputTown: ['', Validators.required],
            // inputTelNo: ['', Validators.compose([Validators.pattern('[0-9+-]*'), Validators.required])],
            // inputFaxNo: ['', Validators.compose([Validators.pattern('[0-9+-]*')])],
            // inputEmail: ['', Validators.compose([Validators.email, Validators.required])],
            // inputCP: ['', Validators.required],
            // inputDesignation: ['', Validators.required],
            // inputContactNumber: ['', Validators.compose([Validators.pattern('[0-9+-]*'), Validators.required])]
        });

        for (let i = 1; i < 3; i++) {
            this.fgVoucher.addControl('books' + i, new FormControl('', [Validators.required]));
            this.fgVoucher.addControl('voucher' + i, new FormControl('', [Validators.required]));
        }
        // #endregion
    }

    // #region Populate data for HTML select items
    private _populateSelect(): void {

    }
    // #endregion

    // #region Save changes
    save(): void {
        this.saving = true;
    }
    // #endregion
}
