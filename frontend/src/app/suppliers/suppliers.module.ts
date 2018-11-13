import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from '../shared/shared.module';
import { SuppliersListComponent } from './suppliers-list/suppliers-list.component';
import { SuppliersTableComponent } from './suppliers-table/suppliers-table.component';
import { SupplierModalComponent } from './shared/widgets/supplier-modal/supplier-modal.component';
import { SuppliersComponent } from './suppliers/suppliers.component';
import { SupplierComponent } from './supplier/supplier.component';

@NgModule({
  imports: [
    SharedModule,
    ReactiveFormsModule
  ],
  declarations: [
    SuppliersListComponent,
    SuppliersTableComponent,
    SupplierModalComponent,
    SuppliersComponent,
    SupplierComponent
  ],
  exports: [SuppliersListComponent]
})
export class SuppliersModule { }
