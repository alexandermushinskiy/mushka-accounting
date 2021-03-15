import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from '../shared/shared.module';
import { SuppliersListComponent } from './components/suppliers-list/suppliers-list.component';
import { SupplierEditorComponent } from './components/supplier-editor/supplier-editor.component';
import { DialogsModule } from '../shared/widgets/dialogs/dialogs.module';
import { SuppliersComponent } from './suppliers.component';
// import { SupplierModalComponent } from './shared/widgets/supplier-modal/supplier-modal.component';

@NgModule({
  imports: [
    SharedModule,
    ReactiveFormsModule,
    DialogsModule
  ],
  declarations: [
    // SupplierModalComponent,
    SuppliersListComponent,
    SupplierEditorComponent,
    SuppliersComponent
  ],
  exports: [
    SuppliersComponent
  ]
})
export class SuppliersModule { }
