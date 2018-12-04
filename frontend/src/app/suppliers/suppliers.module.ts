import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from '../shared/shared.module';
import { SupplierModalComponent } from './shared/widgets/supplier-modal/supplier-modal.component';
import { SuppliersComponent } from './suppliers/suppliers.component';
import { SupplierComponent } from './supplier/supplier.component';

@NgModule({
  imports: [
    SharedModule,
    ReactiveFormsModule
  ],
  declarations: [
    SupplierModalComponent,
    SuppliersComponent,
    SupplierComponent
  ],
  exports: []
})
export class SuppliersModule { }
