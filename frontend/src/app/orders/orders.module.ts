import { NgModule } from '@angular/core';

import { SharedModule } from '../shared/shared.module';
import { OrdersListComponent } from './orders-list/orders-list.component';
import { OrderComponent } from './order/order.component';

@NgModule({
  imports: [
    SharedModule
  ],
  declarations: [
    OrdersListComponent,
    OrderComponent
  ],
  exports: []
})
export class OrdersModule { }
