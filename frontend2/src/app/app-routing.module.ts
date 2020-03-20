import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HandleUnsavedDataGuard } from './shared/guards/handle-unsaved-data.guard';
import { DashboardComponent } from './dashboard/dashboard/dashboard.component';
import { OrdersListComponent } from './orders/orders-list/orders-list.component';
import { OrderComponent } from './orders/order/order.component';
import { CorporateOrdersListComponent } from './corporate-orders/corporate-orders-list/corporate-orders-list.component';
import { CorporateOrderComponent } from './corporate-orders/corporate-order/corporate-order.component';

const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent, pathMatch: 'full' },
  { path: 'orders', children: [
    { path: '', component: OrdersListComponent, pathMatch: 'full' },
    { path: 'new', component: OrderComponent, pathMatch: 'full', canDeactivate: [HandleUnsavedDataGuard] },
    { path: ':id', component: OrderComponent, pathMatch: 'full', canDeactivate: [HandleUnsavedDataGuard] }
  ]},
  { path: 'corporate-orders', children: [
    { path: '', component: CorporateOrdersListComponent, pathMatch: 'full' },
    { path: 'new', component: CorporateOrderComponent, pathMatch: 'full', canDeactivate: [HandleUnsavedDataGuard] },
    { path: ':id', component: CorporateOrderComponent, pathMatch: 'full', canDeactivate: [HandleUnsavedDataGuard] }
  ]},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
