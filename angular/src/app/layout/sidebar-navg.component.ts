import { Component, Injector, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { MenuItem } from '@shared/layout/menu-item';

@Component({
    templateUrl: './sidebar-navg.component.html',
    // tslint:disable-next-line:component-selector
    selector: 'sidebar-nav',
    encapsulation: ViewEncapsulation.None
})
export class SideBarNavComponent extends AppComponentBase {


    menuItems: MenuItem[] = [
        new MenuItem(this.l('Payment Setting'), '', 'payment', '/app/gbs'),
        new MenuItem(this.l('Restriction'), '', 'storage', '/app/restriction'),
        new MenuItem(this.l('Flight Time'), '', 'flight', '/app/flighttime'),
        new MenuItem('Agent', '', 'menu', '', [
            new MenuItem(this.l('Tier'), '', 'supervisor_account', '/app/agenttier'),
            new MenuItem(this.l('Access Fare'), '', 'airline_seat_recline_normal', '/app/accessfare'),
        ]),
        new MenuItem(this.l('Discount'), '', 'attach_money', '/app/discount'),
        new MenuItem(this.l('Capacity'), '', 'storage', '/app/capacity'),
        new MenuItem(this.l('Disc Weight'), '', 'storage', '/app/discweightage'),
        new MenuItem(this.l('Floor Fare'), '', 'storage', '/app/floorfare'),
        new MenuItem(this.l('Seasonality'), '', 'storage', '/app/seasonality'),
        new MenuItem(this.l('LF Discount'), '', 'storage', '/app/lfdisc'),
        new MenuItem(this.l('PU Discount'), '', 'storage', '/app/pudisc'),
        new MenuItem(this.l('Series'), '', 'storage', '/app/series'),
        new MenuItem(this.l('Umrah & Labor'), '', 'storage', '/app/umrahlabor'),

        // new MenuItem('eSWIS', '', 'menu', '', [
        //     new MenuItem('Units', 'Code_Master#Waste_Unit', '', '/app/units'),
        //     new MenuItem('User Groups', 'Security#User_Group', '', '/app/groups'),
        //     new MenuItem('User Profiles', '', '', '/app/profiles'),
        //     new MenuItem('User Approval', '', '', '/app/userapproval'),
        //     new MenuItem('Permission Set', '', '', '/app/permissionset'),
        //     new MenuItem('Consignment', '', '', '/app/consignment'),
        //     new MenuItem('State', '', '', '/app/state'),
        //     new MenuItem('Customer', '', '', '/app/customer'),
        // ]),
    ];

    constructor(
        injector: Injector
    ) {
        super(injector);
    }

    showMenuItem(menuItem): boolean {
        if (menuItem.permissionName) {
            return this.permission.isGranted(menuItem.permissionName);
        }

        return true;
    }

}
