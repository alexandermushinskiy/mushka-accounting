import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { OrdersComponent } from './orders/orders/orders.component';
import { SuppliersListComponent } from './suppliers/suppliers-list/suppliers-list.component';
import { ProductsListComponent } from './products/products-list/products-list.component';
import { PackagesComponent } from './packages/packages/packages.component';
import { PartnersComponent } from './partners/partners/partners.component';
import { LogisticsComponent } from './logistics/logistics/logistics.component';
import { DeliveryComponent } from './delivery/delivery/delivery.component';
import { SuppliersComponent } from './suppliers/suppliers/suppliers.component';
import { SupplierComponent } from './suppliers/supplier/supplier.component';

const routes: Routes = [
  { path: '', redirectTo: 'orders', pathMatch: 'full' },
  { path: 'orders', component: OrdersComponent, pathMatch: 'full' },
  { path: 'suppliers', children: [
    { path: '', component: SuppliersComponent, pathMatch: 'full' },
    { path: 'new', component: SupplierComponent, pathMatch: 'full' },
    { path: ':id', component: SupplierComponent, pathMatch: 'full' }
  ]},
  { path: 'products', component: ProductsListComponent, pathMatch: 'full' },
  { path: 'logistics', component: LogisticsComponent, pathMatch: 'full' },
  { path: 'partners', component: PartnersComponent, pathMatch: 'full' },
  { path: 'delivery', component: DeliveryComponent, pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}