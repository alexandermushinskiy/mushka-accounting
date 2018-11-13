import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { TypeaheadModule } from 'ngx-bootstrap';
import { CurrencyMaskModule } from "ng2-currency-mask";

import { SharedModule } from '../shared/shared.module';
import { DeliveryComponent } from './delivery/delivery.component';
import { SuppliersDropdownComponent } from './shared/widgets/suppliers-dropdown/suppliers-dropdown.component';
import { DeliveryProductsListComponent } from './delivery-products-list/delivery-products-list.component';
import { DeliveryProductModalComponent } from './shared/widgets/delivery-product-modal/delivery-product-modal.component';
import { DeliveryServicesListComponent } from './delivery-services-list/delivery-services-list.component';
import { DeliveryServiceModalComponent } from './shared/widgets/delivery-service-modal/delivery-service-modal.component';
import { DeliveryItemComponent } from './shared/widgets/delivery-item/delivery-item.component';

@NgModule({
  imports: [
    TypeaheadModule.forRoot(),
    SharedModule,
    ReactiveFormsModule,
    CurrencyMaskModule
  ],
  declarations: [
    DeliveryComponent,
    SuppliersDropdownComponent,
    DeliveryProductsListComponent,
    DeliveryProductModalComponent,
    DeliveryServicesListComponent,
    DeliveryServiceModalComponent,
    DeliveryItemComponent
  ],
  exports: [DeliveryComponent]
})
export class DeliveryModule { }