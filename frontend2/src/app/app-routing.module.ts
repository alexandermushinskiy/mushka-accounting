import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HandleUnsavedDataGuard } from './shared/guards/handle-unsaved-data.guard';
import { DashboardComponent } from './dashboard/dashboard/dashboard.component';
import { OrdersListComponent } from './orders/orders-list/orders-list.component';
import { OrderComponent } from './orders/order/order.component';

const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent, pathMatch: 'full' },
  { path: 'orders', children: [
    { path: '', component: OrdersListComponent, pathMatch: 'full' },
    { path: 'new', component: OrderComponent, pathMatch: 'full', canDeactivate: [HandleUnsavedDataGuard] },
    { path: ':id', component: OrderComponent, pathMatch: 'full', canDeactivate: [HandleUnsavedDataGuard] }
  ]},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
