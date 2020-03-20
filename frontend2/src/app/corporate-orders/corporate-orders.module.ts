import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from '../shared/shared.module';
import { CorporateOrdersListComponent } from './corporate-orders-list/corporate-orders-list.component';
import { CorporateOrderComponent } from './corporate-order/corporate-order.component';

@NgModule({
  imports: [
    SharedModule,
    ReactiveFormsModule
  ],
  declarations: [
    CorporateOrdersListComponent,
    CorporateOrderComponent
  ],
  exports: []
})
export class CorporateOrdersModule { }
