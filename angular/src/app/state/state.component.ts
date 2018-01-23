import { ActionState } from '@shared/models/enums';
import { Component, Injector, ViewChild } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { StateDto, PagedResultDtoOfStateDto } from '@shared/models/model-state';
import { StateServiceProxy } from '@shared/service-proxies/service-proxies';
import { PagedListingComponentBase, PagedRequestDto } from 'shared/paged-listing-component-base';
import { CreateStateComponent } from 'app/state/create-state/create-state.component';
import { EditStateComponent } from 'app/state/edit-state/edit-state.component';
import { UtilsService } from '@abp/utils/utils.service';
import { AppConsts } from '@shared/AppConsts';

var _utilsService: UtilsService = new UtilsService();
var userID = _utilsService.getCookieValue(AppConsts.authorization.userIDName);

@Component({
	templateUrl: './state.component.html',
	animations: [appModuleAnimation()]
})
export class StateComponent extends PagedListingComponentBase<StateDto> {

	@ViewChild('createStateModal') createStateModal: CreateStateComponent;
	@ViewChild('editStateModal') editStateModal: EditStateComponent;
	
	active: boolean = false;
	state: StateDto[] = [];
	userID : string;
	

	constructor(
		injector: Injector,
		private _stateService: StateServiceProxy
	) { 
		super(injector);
	}

	protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
		// if (this.state.length > 0) {
		// 	this.pageNumber = pageNumber;
		// 	finishedCallback();
		// } else {
			this._stateService.getAll(request.skipCount, request.maxResultCount)
				.finally(() => {
					finishedCallback(); 
				})
				.subscribe((result: PagedResultDtoOfStateDto) => {
					this.state = result.items;
					this.showPaging(result, pageNumber);
				});
		// }
	}

	protected delete(state: StateDto): void {
		abp.message.confirm(
			"Delete profile '" + state.stateDesc + "'?",
			(result: boolean) => {
				if (result) {
				  this._stateService.delete(state.countryCode, state.stateCode)
				      .subscribe(() => {
				          abp.notify.info("Deleted User: " + state.stateDesc);
				          this.refresh();
				      });
				}
			}
		);
	}

	// Show Modals
	createUserProfile(): void {
		this.createStateModal.show(ActionState.Create);
	}

	editState(state: StateDto): void {
		// this.editStateModal.show(state);
		this.createStateModal.show(ActionState.Edit, state);
	}
}
