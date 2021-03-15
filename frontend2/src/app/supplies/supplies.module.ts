import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { TypeaheadModule } from 'ngx-bootstrap';
import { CurrencyMaskModule } from 'ng2-currency-mask';

import { SharedModule } from '../shared/shared.module';
import { SuppliesListComponent } from './components/supplies-list/supplies-list.component';
import { SupplyEditorComponent } from './components/supply-editor/supply-editor.component';
import { SelectSuppliersComponent } from './components/select-suppliers/select-suppliers.component';
import { SuppliesComponent } from './supplies.component';

@NgModule({
  imports: [
    TypeaheadModule.forRoot(),
    SharedModule,
    ReactiveFormsModule,
    CurrencyMaskModule
  ],
  declarations: [
    SuppliesListComponent,
    SupplyEditorComponent,
    SelectSuppliersComponent,
    SuppliesComponent
  ],
  exports: [
    SuppliesComponent
  ]
})
export class SuppliesModule {
}
