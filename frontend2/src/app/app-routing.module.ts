import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HandleUnsavedDataGuard } from './shared/guards/handle-unsaved-data.guard';
import { DashboardComponent } from './dashboard/dashboard/dashboard.component';
import { OrdersListComponent } from './orders/components/orders-list/orders-list.component';
import { OrderEditorComponent } from './orders/components/order-editor/order-editor.component';
import { CorporateOrdersListComponent } from './corporate-orders/components/corporate-orders-list/corporate-orders-list.component';
import { CorporateOrderEditorComponent } from './corporate-orders/components/corporate-order-editor/corporate-order-editor.component';
import { ProductsListComponent } from './products/products-list/products-list.component';
import { SuppliesListComponent } from './supplies/components/supplies-list/supplies-list.component';
import { SupplyEditorComponent } from './supplies/components/supply-editor/supply-editor.component';
import { SuppliersListComponent } from './suppliers/components/suppliers-list/suppliers-list.component';
import { SupplierEditorComponent } from './suppliers/components/supplier-editor/supplier-editor.component';
import { ExpensesListComponent } from './expenses/components/expenses-list/expenses-list.component';
import { ExpenseEditorComponent } from './expenses/components/expense-editor/expense-editor.component';
import { ExhibitionsListComponent } from './exhibitions/components/exhibitions-list/exhibitions-list.component';
import { ExhibitionEditorComponent } from './exhibitions/components/exhibition-editor/exhibition-editor.component';
import { ExpensesComponent } from './expenses/expenses.component';
import { ExhibitionsComponent } from './exhibitions/exhibitions.component';
import { OrdersComponent } from './orders/orders.component';
import { CorporateOrdersComponent } from './corporate-orders/corporate-orders.component';
import { SuppliesComponent } from './supplies/supplies.component';
import { SuppliersComponent } from './suppliers/suppliers.component';

const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent, pathMatch: 'full' },
  {
    path: 'orders',
    component: OrdersComponent,
    children: [
      { path: '', component: OrdersListComponent, pathMatch: 'full' },
      { path: 'new', component: OrderEditorComponent, canDeactivate: [HandleUnsavedDataGuard] },
      { path: ':id/editor', component: OrderEditorComponent, canDeactivate: [HandleUnsavedDataGuard] }
    ]
  },
  {
    path: 'corporate-orders',
    component: CorporateOrdersComponent,
    children: [
      { path: '', component: CorporateOrdersListComponent, pathMatch: 'full' },
      { path: 'new', component: CorporateOrderEditorComponent, canDeactivate: [HandleUnsavedDataGuard] },
      { path: ':id/editor', component: CorporateOrderEditorComponent, canDeactivate: [HandleUnsavedDataGuard] }
    ]
  },
  { path: 'products', component: ProductsListComponent, pathMatch: 'full' },
  {
    path: 'supplies',
    component: SuppliesComponent,
    children: [
      { path: '', component: SuppliesListComponent, pathMatch: 'full' },
      { path: 'new', component: SupplyEditorComponent },
      { path: ':id/editor', component: SupplyEditorComponent }
    ]
  },
  {
    path: 'suppliers',
    component: SuppliersComponent,
    children: [
      { path: '', component: SuppliersListComponent, pathMatch: 'full' },
      { path: 'new', component: SupplierEditorComponent },
      { path: ':id/editor', component: SupplierEditorComponent }
    ]
  },
  {
    path: 'expenses',
    component: ExpensesComponent,
    children: [
      { path: '', component: ExpensesListComponent, pathMatch: 'full' },
      { path: 'new', component: ExpenseEditorComponent },
      { path: ':id/editor', component: ExpenseEditorComponent }
    ]
  },
  {
    path: 'exhibitions',
    component: ExhibitionsComponent,
    children: [
      { path: '', component: ExhibitionsListComponent, pathMatch: 'full' },
      { path: 'new', component: ExhibitionEditorComponent },
      { path: ':id/editor', component: ExhibitionEditorComponent }
    ]
  },
  { path: '**', redirectTo: 'dashboard' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
