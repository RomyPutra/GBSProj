import { Component, Injector, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { MenuItem } from '@shared/layout/menu-item';

@Component({
    templateUrl: './topbarg.component.html',
    // tslint:disable-next-line:component-selector
    selector: 'top-bar',
    encapsulation: ViewEncapsulation.None
})
export class TopBarComponent extends AppComponentBase {

    constructor(
        injector: Injector
    ) {
        super(injector);
    }

}
