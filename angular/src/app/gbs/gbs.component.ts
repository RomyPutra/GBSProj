// import { EditPaySchemeComponent } from './update-payscheme/update-payscheme.component';
// import { CreateUserGroupComponent } from './create-usergroup/create-usergroup.component';
import { Component, Injector, ViewChild } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PagedListingComponentBase, PagedRequestDto } from 'shared/paged-listing-component-base';
import data_grid from 'devextreme/ui/data_grid';
import { GBSDto, PagedResultDtoOfGBSDto, GBSCountryDto, PagedResultDtoOfGBSCountryDto } from '@shared/models/model-gbs';
import { GetBookingServiceProxy, GbsCodex, GbsPayType } from '@shared/service-proxies/proxy-payscheme';

@Component({
  	templateUrl: './gbs.component.html',
	animations: [appModuleAnimation()]
})
export class GBSComponent extends PagedListingComponentBase<GBSDto> {
    // @ViewChild('updatePayschemeModal') updatePayschemeModal: EditPaySchemeComponent;
    active: boolean = false;
	saving: boolean = false;
	divview: boolean = false;
	divview1: boolean = false;
	divview2: boolean = false;
	code: string;
	countrys: GBSCountryDto[];
	countryx: GBSCountryDto;
	group: GBSDto[];
	review: GBSDto = new GBSDto;
	// selectedItems: any[] = [];
	listGrid: Array<GBSDto> = [];
	codex: GbsCodex[];
	paytype: GbsPayType[];
	hold: number = 0;
	hold1: number = 1;
	fare: number;
	ps1: number;
	ps2: number;
	ps3: number;

	constructor(
		injector: Injector,
		private _gbsService: GetBookingServiceProxy
	) {
		super(injector);
	}

	protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
		this.codex = this._gbsService.getCodex();
		this.paytype = this._gbsService.getPayType();
		this._gbsService.getbookingbypnr(request.skipCount, request.maxResultCount)
			.finally(() => {
				finishedCallback();
			})
			.subscribe((result: PagedResultDtoOfGBSDto) => {
				if (result.items.length > 0) {
					this.group = result.items;
				}
				this.showPaging(result, pageNumber);
			});
			this._gbsService.getCountry(request.skipCount, request.maxResultCount)
			.finally(() => {
				finishedCallback();
			})
			.subscribe((result: PagedResultDtoOfGBSCountryDto) => {
				if (result.items.length > 0) {
					this.countrys = result.items;
				}
				this.showPaging(result, pageNumber);
			});
	}

    onContentReady(e) {
         e.component.columnOption('command:edit', {
			visibleIndex: -1,
            width: 80
        });
	}
	
    onCellPrepared(e) {
        if (e.rowType === 'data' && e.column.command === 'edit') {
            var isEditing = e.row.isEditing,
                cellElement = e.cellElement;

            if (isEditing) {
				let saveLink = cellElement.querySelector('.dx-link-save'),
					cancelLink = cellElement.querySelector('.dx-link-cancel');

                saveLink.classList.add('dx-icon-save');
                cancelLink.classList.add('dx-icon-revert');

                saveLink.textContent = '';
             	cancelLink.textContent = '';
            } else {
				let editLink = cellElement.querySelector('.dx-link-edit');
				// ,deleteLink = cellElement.querySelector('.dx-link-delete');

                editLink.classList.add('dx-icon-edit');
                // deleteLink.classList.add('dx-icon-trash');

                editLink.textContent = 'Edit';
                // deleteLink.textContent = '';
            }
        }
    }

	protected delete(entity: GBSDto): void {
		throw new Error('Method not implemented.');
	  }

	edit(data: any): void {
		if (this.hold === this.hold1) {
			this.hold = 1;
			this._gbsService.update(this.listGrid)
			.finally(() => {
				this.saving = false;
			})
			.subscribe((result: GBSDto[]) => {
				if (result) {
					this.notify.info(this.l('SavedSuccessfully'));
					this.hold = 0;
				} else {
					this.notify.error('Save failed!');
					this.hold = 0;
				}
			});	
		} else {
			this.hold1 = this.hold1 + 1;
		};
	}

	logEvents(data: any) {
		if (data.newData['countryCode'] === undefined) {
			this.hold = this.hold + 1;
			this.hold1 = 1;
			// this.selectedItems = data.key;
			this.listGrid.push(data.key);
		} else {
			this.code = data.newData['countryCode'];
			this._gbsService.get(this.code)
			.subscribe((result: GBSCountryDto) => {
				this.countryx = result;
				data.key['currencyCode'] = this.countryx.currencyCode;
				this.hold = this.hold + 1;
				this.hold1 = 1;
				// this.selectedItems = data.key;
				this.listGrid.push(data.key);
			});
		}
	}
	
	viewDetail(data: any) {
		this.review = data.key;
		this.divview = true;
		if (data.key['attempt_2'] !== 0 && data.key['attempt_3'] !== 0) {
			this.divview1 = true;
			this.divview2 = true;
		} else if (data.key['attempt_2'] !== 0) {
			this.divview1 = true;
			this.divview2 = false;
		} else {
			this.divview1 = false;
			this.divview2 = false;
		}
	}

	test(amount: number, min: number, max: number) {
		this.fare = 0;
		this.ps1 = 0;
		this.ps2 = 0;
		this.ps3 = 0;
		this.fare = amount * (30 / 100);
		if ((this.fare * 0.1) < min && this.divview1 === true)
		{
			this.ps1 = min;
		}
		else if ((this.fare * 0.1) > max && this.divview1 === true)
		{
			this.ps1 = max;
		}
		else if ((this.fare * 0.1) <= max && (this.fare * 0.1) >= min && this.divview1 === true)
		{
			this.ps1 = (this.fare * 0.1);
		}
		else if (this.divview1 === false)
		{
			this.ps1 = amount;
		}

		if ((this.fare - this.ps1) >= max && this.divview2 === true)
		{
			this.ps2 = max;
			this.ps3 = (amount - this.ps1 - this.ps2);
		}
		else if ((this.fare - this.ps1) < max && this.divview2 === true)
		{
			this.ps2 = (this.fare - this.ps1);
			this.ps3 = (amount - this.ps1 - this.ps2);
		}
		else if (this.divview1 === true && this.divview2 === false)
		{
			this.ps2 = (amount - this.ps1);
		}
		// alert('F =' + this.fare + ', P1 =' + this.ps1 + ', P2 =' + this.ps2 + ', P3 =' + this.ps3)
    }

	protected blur() {
		this.divview = false;
	}
}
