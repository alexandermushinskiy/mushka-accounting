import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { TypeaheadModule } from 'ngx-bootstrap';
import { CurrencyMaskModule } from 'ng2-currency-mask';

import { SharedModule } from '../shared/shared.module';
import { SupplyComponent } from './supply/supply.component';
import { SuppliesListComponent } from './supplies-list/supplies-list.component';
import { FiltersModalComponent } from './shared/widgets/filters-modal/filters-modal.component';

@NgModule({
  imports: [
    TypeaheadModule.forRoot(),
    SharedModule,
    ReactiveFormsModule,
    CurrencyMaskModule
  ],
  declarations: [
    SuppliesListComponent,
    SupplyComponent,
    FiltersModalComponent
  ],
  exports: [SupplyComponent]
})
export class SupplyModule { }