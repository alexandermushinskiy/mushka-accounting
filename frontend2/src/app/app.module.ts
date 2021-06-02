import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SharedModule } from './shared/shared.module';
import { CoreModule } from './core/core.module';
import { LayoutModule } from './layout/layout.module';
import { DashboardModule } from './dashboard/dashboard.module';
import { OrdersModule } from './orders/orders.module';
import { CorporateOrdersModule } from './corporate-orders/corporate-orders.module';
import { ProductsModule } from './products/products.module';
import { SuppliesModule } from './supplies/supplies.module';
import { SuppliersModule } from './suppliers/suppliers.module';
import { ExhibitionsModule } from './exhibitions/exhibitions.module';
import { ExpensesModule } from './expenses/expenses.module';
import { LoadingScreenModule } from './shared/components/loading-screen/loading-screen.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    LoadingScreenModule,
    SharedModule,
    CoreModule,
    LayoutModule,
    DashboardModule,
    OrdersModule,
    CorporateOrdersModule,
    ProductsModule,
    SuppliesModule,
    SuppliersModule,
    ExhibitionsModule,
    ExpensesModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
