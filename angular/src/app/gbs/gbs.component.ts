// import { EditUserGroupComponent } from './edit-usergroup/edit-usergroup.component';
// import { CreateUserGroupComponent } from './create-usergroup/create-usergroup.component';
import { Component, Injector, ViewChild } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { GBSDto, PagedResultDtoOfGBSDto } from '@shared/models/model-gbs';
import { GetBookingServiceProxy } from '@shared/service-proxies/service-proxies';
import { PagedListingComponentBase, PagedRequestDto } from 'shared/paged-listing-component-base';

@Component({
  	templateUrl: './gbs.component.html',
	animations: [appModuleAnimation()]
})
export class GBSComponent extends PagedListingComponentBase<GBSDto> {
	active: boolean = false;
	group: GBSDto;

	constructor(
		injector: Injector,
		private _gbsService: GetBookingServiceProxy
	) {
		super(injector);
	}

	protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
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
	}

	protected delete(entity: GBSDto): void {
		throw new Error("Method not implemented.");
	  }
}
