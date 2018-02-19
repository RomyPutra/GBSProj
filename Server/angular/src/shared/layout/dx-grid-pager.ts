class IDxGridPager {
    pageSize: number;
    showPageSizeSelector: boolean;
    allowedPageSizes: number[];
    showInfo: boolean;
}

export class DxGridPagerDefault extends IDxGridPager {
    constructor() {
        super();
        this.pageSize = 10;
        this.showPageSizeSelector = true;
        this.allowedPageSizes = [5, 10, 20];
        this.showInfo = true;
    }
}
