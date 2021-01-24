import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from '../shared/shared.module';
import { OrdersListComponent } from './orders-list/orders-list.component';
import { OrderComponent } from './order/order.component';
import { OrdersActionsBarComponent } from './orders-actions-bar/orders-actions-bar.component';
import { TimeFrameModule } from '../shared/widgets/timeframe/timeframe.module';

@NgModule({
  imports: [
    SharedModule,
    ReactiveFormsModule,
    TimeFrameModule
  ],
  declarations: [
    OrdersListComponent,
    OrderComponent,
    OrdersActionsBarComponent
  ],
  exports: []
})
export class OrdersModule { }
