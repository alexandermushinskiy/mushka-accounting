import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from '../shared/shared.module';
import { SuppliersListComponent } from './suppliers-list/suppliers-list.component';
import { SupplierComponent } from './supplier/supplier.component';
// import { SupplierModalComponent } from './shared/widgets/supplier-modal/supplier-modal.component';

@NgModule({
  imports: [
    SharedModule,
    ReactiveFormsModule
  ],
  declarations: [
    // SupplierModalComponent,
    SuppliersListComponent,
    SupplierComponent],
  exports: []
})
export class SuppliersModule { }
