// import { EditPaySchemeComponent } from './update-payscheme/update-payscheme.component';
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
    // @ViewChild('updatePayschemeModal') updatePayschemeModal: EditPaySchemeComponent;
    active: boolean = false;
    saving: boolean = false;
	group: GBSDto[];
    groups: GBSDto = null;
	events: Array<string> = [];
	selectedItems: any[] = [];

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

	onFieldDataChanged(e) {
		var updatedField = e.dataField;
		var newValue = e.value;
	}

	protected delete(entity: GBSDto): void {
		throw new Error('Method not implemented.');
	  }

	edit(data: any): void {
		this.groups = data.key;
		this.groups.grpid = 'AA';
        this._gbsService.update(this.groups)
		.finally(() => {
			this.saving = false;
		})
		.subscribe((result: GBSDto) => {
			if (result) {
				this.notify.info(this.l('SavedSuccessfully'));
			} else {
				this.notify.error('Save failed!');
			}
		});
}

	logEvent(eventName) {
        this.events.unshift(eventName);
    }

	logEvents(data: any) {
		this.selectedItems = data;
        // this.selectedItems.forEach((item) => {
        //     this.dataSource.remove(item);
        //     this.dataGrid.instance.refresh();
        // });
        this.events.unshift(this.selectedItems[0]);
    }
}
