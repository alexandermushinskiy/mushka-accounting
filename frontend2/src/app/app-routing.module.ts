import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HandleUnsavedDataGuard } from './shared/guards/handle-unsaved-data.guard';
import { DashboardComponent } from './dashboard/dashboard/dashboard.component';
import { OrdersListComponent } from './orders/orders-list/orders-list.component';
import { OrderComponent } from './orders/order/order.component';
import { CorporateOrdersListComponent } from './corporate-orders/corporate-orders-list/corporate-orders-list.component';
import { CorporateOrderComponent } from './corporate-orders/corporate-order/corporate-order.component';
import { ProductsListComponent } from './products/products-list/products-list.component';
import { SuppliesListComponent } from './supplies/supplies-list/supplies-list.component';
import { SupplyComponent } from './supplies/supply/supply.component';
import { SuppliersListComponent } from './suppliers/suppliers-list/suppliers-list.component';
import { SupplierComponent } from './suppliers/supplier/supplier.component';
import { ExpensesListComponent } from './expenses/expenses-list/expenses-list.component';
import { ExpenseComponent } from './expenses/expense/expense.component';
import { ExhibitionsListComponent } from './exhibitions/exhibitions-list/exhibitions-list.component';
import { ExhibitionComponent } from './exhibitions/exhibition/exhibition.component';

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
  { path: 'products', component: ProductsListComponent, pathMatch: 'full' },
  { path: 'supplies', children: [
    { path: '', component: SuppliesListComponent, pathMatch: 'full' },
    { path: 'new', component: SupplyComponent, pathMatch: 'full' },
    { path: ':id', component: SupplyComponent, pathMatch: 'full' }
  ]},
  { path: 'suppliers', children: [
    { path: '', component: SuppliersListComponent, pathMatch: 'full' },
    { path: 'new', component: SupplierComponent, pathMatch: 'full' },
    { path: ':id', component: SupplierComponent, pathMatch: 'full' }
  ]},
  { path: 'expenses', children: [
    { path: '', component: ExpensesListComponent, pathMatch: 'full' },
    { path: 'new', component: ExpenseComponent, pathMatch: 'full' },
    { path: ':id', component: ExpenseComponent, pathMatch: 'full' }
  ] },
  { path: 'exhibitions', children: [
    { path: '', component: ExhibitionsListComponent, pathMatch: 'full' },
    { path: 'new', component: ExhibitionComponent, pathMatch: 'full' },
    { path: ':id', component: ExhibitionComponent, pathMatch: 'full' }
  ]},
  { path: '**', redirectTo: 'dashboard' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
