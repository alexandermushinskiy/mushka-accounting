import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { OrdersComponent } from './orders/orders/orders.component';
import { ProductsListComponent } from './products/products-list/products-list.component';
import { PartnersComponent } from './partners/partners/partners.component';
import { LogisticsComponent } from './logistics/logistics/logistics.component';
import { SuppliersListComponent } from './suppliers/suppliers-list/suppliers-list.component';
import { SupplierComponent } from './suppliers/supplier/supplier.component';
import { DeliveriesListComponent } from './delivery/deliveries-list/deliveries-list.component';
import { DeliveryComponent } from './delivery/delivery/delivery.component';
import { DeliveryTmpComponent } from './delivery/delivery-tmp/delivery-tmp.component';

const routes: Routes = [
  { path: '', redirectTo: 'orders', pathMatch: 'full' },
  { path: 'orders', component: OrdersComponent, pathMatch: 'full' },
  { path: 'suppliers', children: [
    { path: '', component: SuppliersListComponent, pathMatch: 'full' },
    { path: 'new', component: SupplierComponent, pathMatch: 'full' },
    { path: ':id', component: SupplierComponent, pathMatch: 'full' }
  ]},
  { path: 'products', component: ProductsListComponent, pathMatch: 'full' },
  { path: 'logistics', component: LogisticsComponent, pathMatch: 'full' },
  { path: 'partners', component: PartnersComponent, pathMatch: 'full' },
  { path: 'deliveries', children: [
    { path: '', component: DeliveriesListComponent, pathMatch: 'full' },
    { path: 'new', component: DeliveryTmpComponent, pathMatch: 'full' },
    { path: ':id', component: DeliveryTmpComponent, pathMatch: 'full' }
  ]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
