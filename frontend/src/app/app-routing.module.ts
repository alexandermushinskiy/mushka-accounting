import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { OrdersComponent } from './orders/orders/orders.component';
import { ProductsListComponent } from './products/products-list/products-list.component';
import { PartnersComponent } from './partners/partners/partners.component';
import { LogisticsComponent } from './logistics/logistics/logistics.component';
import { SuppliersListComponent } from './suppliers/suppliers-list/suppliers-list.component';
import { SupplierComponent } from './suppliers/supplier/supplier.component';
import { SuppliesListComponent } from './supplies/supplies-list/supplies-list.component';
import { SupplyComponent } from './supplies/supply/supply.component';

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
  { path: 'supplies', children: [
    { path: '', component: SuppliesListComponent, pathMatch: 'full' },
    { path: 'new', component: SupplyComponent, pathMatch: 'full' },
    { path: ':id', component: SupplyComponent, pathMatch: 'full' }
  ]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
