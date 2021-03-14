import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from '../shared/shared.module';
import { OrdersListComponent } from './components/orders-list/orders-list.component';
import { OrderEditorComponent } from './components/order-editor/order-editor.component';
import { OrdersActionsBarComponent } from './components/orders-actions-bar/orders-actions-bar.component';
import { TimeFrameModule } from '../shared/widgets/timeframe/timeframe.module';
import { OrdersComponent } from './orders.component';

@NgModule({
  imports: [
    SharedModule,
    ReactiveFormsModule,
    TimeFrameModule
  ],
  declarations: [
    OrdersListComponent,
    OrderEditorComponent,
    OrdersActionsBarComponent,
    OrdersComponent
  ],
  exports: [
    OrdersComponent
  ]
})
export class OrdersModule { }
