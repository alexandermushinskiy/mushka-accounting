import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { OrdersListComponent } from './orders/orders-list/orders-list.component';
import { ProductsListComponent } from './products/products-list/products-list.component';
import { PartnersComponent } from './partners/partners/partners.component';
import { LogisticsComponent } from './logistics/logistics/logistics.component';
import { SuppliersListComponent } from './suppliers/suppliers-list/suppliers-list.component';
import { SupplierComponent } from './suppliers/supplier/supplier.component';
import { SuppliesListComponent } from './supplies/supplies-list/supplies-list.component';
import { SupplyComponent } from './supplies/supply/supply.component';
import { DashboardComponent } from './dashboard/dashboard/dashboard.component';
import { OrderComponent } from './orders/order/order.component';
import { CostsListComponent } from './costs/costs-list/costs-list.component';
import { GiftsListComponent } from './gifts/gifts-list/gifts-list.component';
import { ExhibitionsListComponent } from './exhibitions/exhibitions-list/exhibitions-list.component';
import { ExhibitionComponent } from './exhibitions/exhibition/exhibition.component';

const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent, pathMatch: 'full' },
  { path: 'orders', children: [
    { path: '', component: OrdersListComponent, pathMatch: 'full' },
    { path: 'new', component: OrderComponent, pathMatch: 'full' },
    { path: ':id', component: OrderComponent, pathMatch: 'full' }
  ]},
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
  ]},
  { path: 'costs', component: CostsListComponent, pathMatch: 'full' },
  { path: 'gifts', component: GiftsListComponent, pathMatch: 'full' },
  { path: 'exhibitions', children: [
    { path: '', component: ExhibitionsListComponent, pathMatch: 'full' },
    { path: 'new', component: ExhibitionComponent, pathMatch: 'full' },
    { path: ':id', component: ExhibitionComponent, pathMatch: 'full' }
  ]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
