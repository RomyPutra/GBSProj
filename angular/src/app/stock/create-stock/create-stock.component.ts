import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { StockServiceProxy } from '@shared/service-proxies/service-proxies';
import { StockDto } from '@shared/models/model-stock';
import { AppComponentBase } from '@shared/app-component-base';
import { ItemCategoryDto, PagedResultDtoOfItemCategoryDto } from '@shared/models/model-itemcategory';
import { CategoryServiceProxy } from '@shared/service-proxies/service-proxies';
import { ActionState } from '@shared/models/enums';

import * as _ from "lodash";

@Component({
  selector: 'create-stock-modal',
  templateUrl: './create-stock.component.html'
})
export class CreateStockComponent extends AppComponentBase implements OnInit {

    @ViewChild('createStockModal') modal: ModalDirective;
    @ViewChild('modalContent') modalContent: ElementRef;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active: boolean = false;
    saving: boolean = false;
    stockdata: StockDto = null;
    itemCategory: ItemCategoryDto[] = [];
    action: ActionState;
    ActionState = ActionState;

    constructor(
        injector: Injector,
        private _stockService: StockServiceProxy,
        private _categoryService: CategoryServiceProxy
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.populateCategory();
    }

    show(action: ActionState, stockdata?: StockDto): void {
        this.action = action;
        if (action === ActionState.Create) {
            this.active = true;
            this.modal.show();
            this.stockdata = new StockDto();
            this.stockdata.init({ isActive: true });
        } else if (action === ActionState.Edit) {
            if (stockdata === undefined || stockdata === null) {
                this.notify.error("The parameter 'stock data' must be defined and cannot be null.");
            } else {
                this.active = true;
                this.stockdata = stockdata;
                this.modal.show();
            }
        }
    }

    private populateCategory(): void {
        this._categoryService.getAll(-1, -1)
            .finally(() => {

            })
            .subscribe((result: PagedResultDtoOfItemCategoryDto) => {
                this.itemCategory = result.items;
            });
    }

    onShown(): void {
        $.AdminBSB.input.activate($(this.modalContent.nativeElement));
    }

    save(): void {
        //TODO: Refactor this, don't use jQuery style code
        // this.user.accessCode = this.user.accessCode;
        this.saving = true;

        if (this.action === ActionState.Create) {
            this._stockService.create(this.stockdata)
                .finally(() => { this.saving = false; })
                .subscribe((result: boolean) => {
                    if (result) {
                        this.notify.info(this.l('SavedSuccessfully'));
                        this.close();
                        this.modalSave.emit(null);
                    } else {
                        this.notify.error('Save failed!');
                    }
                });
        } else if (this.action === ActionState.Edit) {
            this._stockService.update(this.stockdata)
                .finally(() => {
                    this.saving = false;
                })
                .subscribe((result: boolean) => {
                    if (result) {
                        this.notify.info(this.l('SavedSuccessfully'));
                        this.close();
                        this.modalSave.emit(null);
                    } else {
                        this.notify.error('Save failed!');
                    }
                });
        }
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
