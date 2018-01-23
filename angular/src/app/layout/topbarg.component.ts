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

    menuItems: MenuItem[] = [
        new MenuItem(this.l('HomePage'), '', 'home', '/app/home'),

        new MenuItem(this.l('Tenants'), '', 'business', '/app/tenants'),
        new MenuItem(this.l('Users'), '', 'people', '/app/users'),
        new MenuItem(this.l('Roles'), '', 'local_offer', '/app/roles'),
        new MenuItem(this.l('About'), '', 'book', '/app/about'),

        new MenuItem('eSWIS', '', 'menu', '', [
            new MenuItem('Units', 'Code_Master#Waste_Unit', '', '/app/units'),
            new MenuItem('User Groups', 'Security#User_Group', '', '/app/groups'),
            new MenuItem('User Profiles', '', '', '/app/profiles'),
            new MenuItem('User Approval', '', '', '/app/userapproval'),
            new MenuItem('Permission Set', '', '', '/app/permissionset'),
            new MenuItem('Consignment', '', '', '/app/consignment'),
            new MenuItem('State', '', '', '/app/state'),
            new MenuItem('Customer', '', '', '/app/customer'),
        ]),

        new MenuItem(this.l('MultiLevelMenu'), '', 'menu', '', [
            new MenuItem('ASP.NET Boilerplate', '', '', '', [
                new MenuItem('Home', '', '', 'https://aspnetboilerplate.com/?ref=abptmpl'),
                new MenuItem('Templates', '', '', 'https://aspnetboilerplate.com/Templates?ref=abptmpl'),
                new MenuItem('Samples', '', '', 'https://aspnetboilerplate.com/Samples?ref=abptmpl'),
                new MenuItem('Documents', '', '', 'https://aspnetboilerplate.com/Pages/Documents?ref=abptmpl')
            ]),
            new MenuItem('ASP.NET Zero', '', '', '', [
                new MenuItem('Home', '', '', 'https://aspnetzero.com?ref=abptmpl'),
                new MenuItem('Description', '', '', 'https://aspnetzero.com/?ref=abptmpl#description'),
                new MenuItem('Features', '', '', 'https://aspnetzero.com/?ref=abptmpl#features'),
                new MenuItem('Pricing', '', '', 'https://aspnetzero.com/?ref=abptmpl#pricing'),
                new MenuItem('Faq', '', '', 'https://aspnetzero.com/Faq?ref=abptmpl'),
                new MenuItem('Documents', '', '', 'https://aspnetzero.com/Documents?ref=abptmpl')
            ])
        ])
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
