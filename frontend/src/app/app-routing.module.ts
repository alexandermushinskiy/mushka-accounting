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
import { ExpensesListComponent } from './expenses/expenses-list/expenses-list.component';
import { GiftsListComponent } from './gifts/gifts-list/gifts-list.component';
import { ExhibitionsListComponent } from './exhibitions/exhibitions-list/exhibitions-list.component';
import { ExhibitionComponent } from './exhibitions/exhibition/exhibition.component';
import { ExpenseComponent } from './expenses/expense/expense.component';
import { CorporateOrdersListComponent } from './corporate-orders/corporate-orders-list/corporate-orders-list.component';
import { CorporateOrderComponent } from './corporate-orders/corporate-order/corporate-order.component';
import { HandleUnsavedDataGuard } from './shared/guards/handle-unsaved-data.guard';

const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent, pathMatch: 'full' },
  { path: 'orders', children: [
    { path: '', component: OrdersListComponent, pathMatch: 'full' },
    { path: 'new', component: OrderComponent, pathMatch: 'full', canDeactivate: [HandleUnsavedDataGuard] },
    { path: ':id', component: OrderComponent, pathMatch: 'full' }
  ]},
  { path: 'corporate-orders', children: [
    { path: '', component: CorporateOrdersListComponent, pathMatch: 'full' },
    { path: 'new', component: CorporateOrderComponent, pathMatch: 'full' },
    { path: ':id', component: CorporateOrderComponent, pathMatch: 'full' }
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
  { path: 'expenses', children: [
    { path: '', component: ExpensesListComponent, pathMatch: 'full' },
    { path: 'new', component: ExpenseComponent, pathMatch: 'full' },
    { path: ':id', component: ExpenseComponent, pathMatch: 'full' }
  ] },
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
