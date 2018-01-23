import { Component, Injector, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
    templateUrl: './sidebar-navg.component.html',
    // tslint:disable-next-line:component-selector
    selector: 'sidebar-nav',
    encapsulation: ViewEncapsulation.None
})
export class SideBarNavComponent extends AppComponentBase {

    constructor(
        injector: Injector
    ) {
        super(injector);
    }

}
