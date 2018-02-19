class IDxGridConfiguration {
    rowAlternationEnabled: boolean;
    showColumnLines: boolean;
    showRowLines: boolean;
    showBorders: boolean;
    allowColumnResizing: boolean;
    columnResizingMode: string;
    hoverStateEnabled: boolean;
    columnAutoWidth: boolean;
}

export class DxGridBorderlessStripped extends IDxGridConfiguration {
    constructor() {
        super();
        this.rowAlternationEnabled = true;
        this.showColumnLines = false;
        this.showRowLines = false;
        this.showBorders = false;
        this.allowColumnResizing = true;
        this.columnResizingMode = 'nextColumn';
        this.hoverStateEnabled = true;
        this.columnAutoWidth = true;
    }
}

export class DxGridBorderedStripped extends IDxGridConfiguration {
    constructor() {
        super();
        this.rowAlternationEnabled = true;
        this.showColumnLines = true;
        this.showRowLines = true;
        this.showBorders = true;
        this.allowColumnResizing = true;
        this.columnResizingMode = 'nextColumn';
        this.hoverStateEnabled = true;
        this.columnAutoWidth = true;
    }
}
