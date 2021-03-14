import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from '../shared/shared.module';
import { CorporateOrdersListComponent } from './components/corporate-orders-list/corporate-orders-list.component';
import { CorporateOrderEditorComponent } from './components/corporate-order-editor/corporate-order-editor.component';
import { CorporateOrdersComponent } from './corporate-orders.component';

@NgModule({
  imports: [
    SharedModule,
    ReactiveFormsModule
  ],
  declarations: [
    CorporateOrdersListComponent,
    CorporateOrderEditorComponent,
    CorporateOrdersComponent
  ],
  exports: [
    CorporateOrdersComponent
  ]
})
export class CorporateOrdersModule { }
