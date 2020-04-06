import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { TypeaheadModule } from 'ngx-bootstrap';
import { CurrencyMaskModule } from 'ng2-currency-mask';

import { SharedModule } from '../shared/shared.module';
import { SuppliesListComponent } from './supplies-list/supplies-list.component';
import { SupplyComponent } from './supply/supply.component';
import { SelectSuppliersComponent } from './shared/widgets/select-suppliers/select-suppliers.component';

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
    SelectSuppliersComponent
  ],
  exports: [
    // SupplyComponent
  ]
})
export class SuppliesModule {
}
