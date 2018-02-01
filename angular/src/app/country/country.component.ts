import { EditCountryComponent } from './edit-country/edit-country.component';
import { Component, Injector, ViewChild } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { CountryDto, PagedResultDtoOfCountryDto } from '@shared/models/model-country';
import { CountryServiceProxy } from '@shared/service-proxies/proxy-country';
import { PagedRequestDto } from 'shared/paged-listing-component-base';
import { PagedListingComponentCustom } from 'shared/layout/paged-listing-component-custom';
import { CreateCountryComponent } from './create-country/create-country.component';

@Component({
  templateUrl: './country.component.html',
    animations: [appModuleAnimation()]
})
// export class UnitsComponent implements OnInit {
export class CountryComponent extends PagedListingComponentCustom<CountryDto> {
  active: boolean = false;
  countries: CountryDto[] = [];
  @ViewChild('editCountryModal') editCountryModal: EditCountryComponent;
  @ViewChild('createCountryModal') createCountryModal: CreateCountryComponent;

  constructor(
      injector: Injector,
      private _countryService: CountryServiceProxy
  ) {
      super(injector);
  }

  protected refresh(): void {
      this.isTableLoading = false;
      this._countryService.getAll(0, 0)
          .finally(() => {
            this.isTableLoading = true;
          })
          .subscribe((result: PagedResultDtoOfCountryDto) => {
              this.countries = result.items;
          });
  }

  protected delete(country: CountryDto): void {
      abp.message.confirm(
          "Delete country '" + country.countryDesc + "'?",
          (result: boolean) => {
              if (result) {
                  this._countryService.delete(country)
                      .subscribe(() => {
                          abp.notify.info("Deleted User: " + country.countryDesc);
                          this.refresh();
                      });
              }
          }
      );
  }

  // Show Modals
  createCountry(): void {
       this.createCountryModal.show();
  }

  editCountry(country: CountryDto): void {
     this.editCountryModal.show(country);
  }
}
