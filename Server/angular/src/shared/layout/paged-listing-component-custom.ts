import { DxGridPagerDefault } from './dx-grid-pager';
import { DxGridBorderlessStripped, DxGridBorderedStripped } from './dx-grid-theme';
import { PagedResultDto, PagedRequestDto } from './../paged-listing-component-base';
import { AppComponentBase } from './../app-component-base';
import { Injector, OnInit } from '@angular/core';

export abstract class PagedListingComponentCustom<EntityDto> extends AppComponentBase implements OnInit {

    public pageSize: number = 10;
    public pageNumber: number = 1;
    public totalPages: number = 1;
    public totalItems: number;
    public isTableLoading = false;
    protected filterValidLenght: number = 0;
    public filterPlaceHolder: string = 'Search...';

    public dxGridConf: DxGridBorderlessStripped = new DxGridBorderlessStripped();
    public dxGridPager: DxGridPagerDefault = new DxGridPagerDefault();

    constructor(injector: Injector) {
        super(injector);
    }

    ngOnInit(): void {
        this.refresh();
    }

    isValidFilter(str: string, validLenght: number): boolean {
        return str !== undefined && str != null && str !== '' && str.length > validLenght;
    }

    protected abstract refresh(): void;
    protected abstract delete(entity: EntityDto): void;
}
