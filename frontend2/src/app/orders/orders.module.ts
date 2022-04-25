import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from '../shared/shared.module';
import { OrdersListComponent } from './components/orders-list/orders-list.component';
import { OrderEditorComponent } from './components/order-editor/order-editor.component';
import { OrdersActionsBarComponent } from './components/orders-actions-bar/orders-actions-bar.component';
import { TimeFrameModule } from '../shared/components/time-frame/time-frame.module';
import { OrdersComponent } from './orders.component';
import { ActionsBarModule } from '../shared/components/actions-bar/actions-bar.module';
import { DividerModule } from '../shared/components/divider/divider.module';
import { ButtonsModule } from '../shared/components/buttons/buttons.module';
import { OrdersTableComponent } from './components/orders-table/orders-table.component';
import { FilterBarModule } from '../shared/components/filter-bar/filter-bar.module';
import { InputsModule } from '../shared/components/inputs/inputs.module';
import { SelectProductModule } from '../shared/components/select-product/select-product.module';
import { CheckboxModule } from '../shared/components/checkbox/checkbox.module';
import { OrderProductsComponent } from './components/order-products/order-products.component';

@NgModule({
  imports: [
    SharedModule,
    ReactiveFormsModule,
    TimeFrameModule,
    ActionsBarModule,
    DividerModule,
    ButtonsModule,
    FilterBarModule,
    InputsModule,
    SelectProductModule,
    CheckboxModule
  ],
  declarations: [
    OrdersListComponent,
    OrderEditorComponent,
    OrdersActionsBarComponent,
    OrdersComponent,
    OrdersTableComponent,
    OrderProductsComponent
  ],
  exports: [
    OrdersComponent
  ]
})
export class OrdersModule { }
