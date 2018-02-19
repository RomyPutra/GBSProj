import { Component, Injector, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import { AppComponentBase } from '@shared/app-component-base';
import { CustomerServiceProxy } from '@shared/service-proxies/proxy-customer';
import { LocationServiceProxy } from '@shared/service-proxies/proxy-location';
import { EmployeeServiceProxy } from '@shared/service-proxies/proxy-employee';
import { VehicleServiceProxy } from '@shared/service-proxies/proxy-vehicle';
import { UserProfileServiceProxy } from '@shared/service-proxies/proxy-userprofile';
import { CustomerDto } from '@shared/models/model-customer';
import { LocationDto, PagedResultDtoOfLocationDto } from '@shared/models/model-location';
import { EmployeeDto, PagedResultDtoOfEmployeeDto } from '@shared/models/model-employee';
import { VehicleDto, PagedResultDtoOfVehicleDto } from '@shared/models/model-vehicle';
import { UserProfileDto, PagedResultDtoOfUserProfileDto } from '@shared/models/model-userprofile';
import { appModuleAnimation } from '@shared/animations/routerTransition';

import { StateServiceProxy } from 'shared/service-proxies/proxy-state';
import { CodeMasterServiceProxy } from 'shared/service-proxies/proxy-codemaster';
import { PBTServiceProxy } from 'shared/service-proxies/proxy-pbt';
import { AreaServiceProxy } from 'shared/service-proxies/proxy-area';
import { CityServiceProxy } from 'shared/service-proxies/proxy-city';
import { CountryServiceProxy } from 'shared/service-proxies/proxy-country';
import { StateDto } from '@shared/models/model-state';
import { CodeMasterDto } from '@shared/models/model-codemaster';
import { PBTDto } from '@shared/models/model-pbt';
import { AreaDto } from 'shared/models/model-area';
import { CityDto } from 'shared/models/model-city';
import { CountryDto } from 'shared/models/model-country';

import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
    templateUrl: './edit-customer.component.html',
    animations: [appModuleAnimation()]
})
export class EditCustomerComponent extends AppComponentBase implements OnInit {

    saving: boolean;
    isLoading: boolean;
    bizRegID: string;
    editCustomer: FormGroup;

    // #region Define objects
    customer: CustomerDto = new CustomerDto();
    location: LocationDto[] = [];
    employee: EmployeeDto[] = [];
    vehicle: VehicleDto[] = [];
    userprofile: UserProfileDto[] = [];
    isSelectedKKM: boolean;
    roles: CodeMasterDto[] = [];
    selectedRoles: any;
    countries: CountryDto[] = [];
    selectedCountry: string;
    states: StateDto[] = [];
    selectedState: string;
    pbts: PBTDto[] = [];
    selectedPBT: string;
    cities: CityDto[] = [];
    selectedCity: string;
    areas: AreaDto[] = [];
    selectedArea: string;
    designations: CodeMasterDto[] = [];
    // #endregion

    constructor(
        injector: Injector,
        private _route: ActivatedRoute,
        private _formBuilder: FormBuilder,
        private _customerService: CustomerServiceProxy,
        private _employeeService: EmployeeServiceProxy,
        private _locationService: LocationServiceProxy,
        private _vehicleService: VehicleServiceProxy,
        private _loginDetailsService: UserProfileServiceProxy,

        private _stateService: StateServiceProxy,
        private _codeMasterService: CodeMasterServiceProxy,
        private _pbtService: PBTServiceProxy,
        private _areaService: AreaServiceProxy,
        private _cityService: CityServiceProxy,
        private _countryService: CountryServiceProxy
    ) {
        super(injector);
        this.bizRegID = _route.snapshot.params.bizRegID;
        this.loadingMessage = 'Loading...';
    }

    ngOnInit(): void {
        this._populateSelect();

        this.isLoading = true;
        this._customerService.get(this.bizRegID)
            .finally(() => {
                this._getSelectedRoles(this.customer.companyType);
                this._getSelectedEntity(this.customer.country, this.customer.state, this.customer.pbt, this.customer.city, this.customer.area);
                this._loadTabbedData(this.bizRegID);
                this.isLoading = false;
            })
            .subscribe((cust: CustomerDto) => {
                if (cust.kkm === 1) {
                    this.isSelectedKKM = true;
                } else {
                    this.isSelectedKKM = false;
                }
                this.customer = cust;
            });
        
        // #region Define angular material form group and validation rules
        this.editCustomer = this._formBuilder.group({
            inputROCNo: ['', Validators.required],
            inputCompName: ['', Validators.required],
            inputRole: ['', Validators.required],
            checkUploadKKM: [],
            inputAddress1: ['', Validators.required],
            inputAddress2: [],
            inputAddress3: [],
            inputAddress4: [],
            inputPostCode: ['', Validators.compose([Validators.pattern('[0-9]*'), Validators.required])],
            inputCountry: ['', Validators.required],
            inputState: ['', Validators.required],
            inputPBT: ['', Validators.required],
            inputDistrict: ['', Validators.required],
            inputTown: ['', Validators.required],
            inputTelNo: ['', Validators.compose([Validators.pattern('[0-9+-]*'), Validators.required])],
            inputFaxNo: ['', Validators.compose([Validators.pattern('[0-9+-]*')])],
            inputEmail: ['', Validators.compose([Validators.email, Validators.required])],
            inputCP: ['', Validators.required],
            inputDesignation: ['', Validators.required],
            inputContactNumber: ['', Validators.compose([Validators.pattern('[0-9+-]*'), Validators.required])]
        });
        // #endregion
    }

    // #region Populate data for HTML select items
    private _populateSelect(): void {
        this._codeMasterService.getAppIDs()
            .subscribe((appID) => {
                this.roles = appID.items;
            });
        this._countryService.getAll(0, 0)
            .subscribe((result) => {
                this.countries = result.items;
            });
        this._stateService.getAll(0, 0)
            .subscribe((result) => {
                this.states = result.items;
            });
        this._pbtService.getAll(0, 0)
            .subscribe((result) => {
                this.pbts = result.items;
            });
        this._cityService.getAll(0, 0)
            .subscribe((result) => {
                this.cities = result.items;
            });
        this._areaService.getAll(0, 0)
            .subscribe((result) => {
                this.areas = result.items;
            });
        this._codeMasterService.getDesignations()
            .subscribe((result) => {
                this.designations = result.items;
            });
    }
    // #endregion

    // #region Load data for tabbed contents
    private _loadTabbedData(bizRegID: string): void {
        this._locationService.getListEntity(0, 0, bizRegID)
            .subscribe((result: PagedResultDtoOfLocationDto) => { this.location = result.items; });
        this._employeeService.getListEntity(0, 0, bizRegID)
            .subscribe((result: PagedResultDtoOfEmployeeDto) => { this.employee = result.items; });
        this._vehicleService.getListEntity(0, 0, bizRegID)
            .subscribe((result: PagedResultDtoOfVehicleDto) => { this.vehicle = result.items; });
        this._loginDetailsService.getListEntity(0, 0, bizRegID)
            .subscribe((result) => { this.userprofile = result.items; });
    }
    // #endregion

    // #region Convert role values to angular multiple select (multiple array)
    private _getSelectedRoles(value: string) {
        switch (value) {
            case '5':
                this.selectedRoles = [
                    this.roles[0], this.roles[1]
                ];
                break;
            case '6':
                this.selectedRoles = [
                    this.roles[0], this.roles[2]
                ];
                break;
            case '7':
                this.selectedRoles = [
                    this.roles[1], this.roles[2]
                ];
                break;
            case '9':
                this.selectedRoles = [
                    this.roles[0], this.roles[1], this.roles[2]
                ];
                break;
            default:
                this.selectedRoles = [
                    this.roles[Number(value) - 2]
                ];
                break;
        }
    }
    // #endregion

    // #region Get country, state, PBT, city, and area values
    private _getSelectedEntity(countryCode: string, stateCode: string, pbtCode: string, cityCode: string, areaCode: string) {
        this.selectedCountry = countryCode;
        this.selectedState = stateCode;
        this.selectedPBT = pbtCode;
        this.selectedCity = cityCode;
        this.selectedArea = areaCode;
    }
    // #endregion

    // #region HTML select Country onChanged
    countryChanged() {
        this.selectedState = undefined;
        this.selectedPBT = undefined;
        this.selectedCity = undefined;
        this.selectedArea = undefined;
    }
    // #endregion

    // #region HTML select state functions
    getState(countryCode: string): StateDto[] {
        return this.states.filter(c => c.countryCode == countryCode);
    }

    stateChanged() {
        this.selectedPBT = undefined;
        this.selectedCity = undefined;
        this.selectedArea = undefined;
    }
    // #endregion

    // #region HTML select PBT functions
    getPBT(countryCode: string, stateCode: string): PBTDto[] {
        return this.pbts.filter(p => p.countryCode == countryCode && p.stateCode == stateCode);
    }
    // #endregion

    // #region HTML select city functions
    getCity(countryCode: string, stateCode: string): CityDto[] {
        return this.cities.filter(t => t.countryCode == countryCode && t.stateCode == stateCode);
    }

    cityChanged() {
        this.selectedArea = undefined;
    }
    // #endregion

    // #region Get area list based from country, state, city selected values
    getArea(countryCode: string, stateCode: string, cityCode: string): AreaDto[] {
        return this.areas.filter(a => a.countryCode == countryCode && a.stateCode == stateCode && a.cityCode == cityCode);
    }
    // #endregion

    // #region Save changes
    save(): void {
        this.saving = true;

        let sum = 0;
        for (var i = 0; i < this.selectedRoles.length; i++) {
            sum += Number(this.selectedRoles[i].code);
        }
        this.customer.companyType = sum.toString();

        if (this.isSelectedKKM) {
            this.customer.kkm = 1;
        } else {
            this.customer.kkm = 0;
        }

        this.customer.country = this.selectedCountry;
        this.customer.state = this.selectedState;
        this.customer.pbt = this.selectedPBT;
        this.customer.city = this.selectedCity;
        this.customer.area = this.selectedArea;
        this.customer.lastUpdate = new Date();
        this._customerService.update(this.customer)
            .finally(() => {
                this.saving = false;
            })
            .subscribe((result: boolean) => {
                if (result) {
                    this.notify.info(this.l('SavedSuccessfully'));
                } else {
                    this.notify.error('Save failed!');
                }
            });
    }
    // #endregion
}
