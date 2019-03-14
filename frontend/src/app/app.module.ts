import 'rxjs/add/operator/takeUntil';
import 'rxjs/add/operator/finally';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/takeWhile';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/take';
import 'rxjs/add/operator/reduce';
import 'rxjs/add/operator/first';
import 'rxjs/add/operator/auditTime';
import 'rxjs/add/operator/skip';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/delay';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/debounceTime';

import 'rxjs/add/observable/zip';
import 'rxjs/add/observable/of';
import 'rxjs/add/observable/from';
import 'rxjs/add/observable/forkJoin';
import 'rxjs/add/observable/fromEvent';
import 'rxjs/add/operator/mergeMap';

import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxWebstorageModule } from 'ngx-webstorage';
import { ToastrModule } from 'ngx-toastr';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { SharedModule } from './shared/shared.module';
import { CoreModule } from './core/core.module';
import { SidebarModule } from './sidebar/sidebar.module';
import { HeaderModule } from './header/header.module';
import { OrdersModule } from './orders/orders.module';
import { SuppliersModule } from './suppliers/suppliers.module';
import { ProductsModule } from './products/products.module';
import { PackagesModule } from './packages/packages.module';
import { PartnersModule } from './partners/partners.module';
import { LogisticsModule } from './logistics/logistics.module';
import { SupplyModule } from './supplies/supply.module';
import { GiftsModule } from './gifts/gifts.module';
import { ExpensesModule } from './expenses/expenses.module';
import { DashboardModule } from './dashboard/dashboard.module';
import { ExhibitionsModule } from './exhibitions/exhibitions.module';
import { CorporateOrdersModule } from './corporate-orders/corporate-orders.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    SharedModule,
    CoreModule,
    SidebarModule,
    HeaderModule,
    OrdersModule,
    SuppliersModule,
    ProductsModule,
    PackagesModule,
    PartnersModule,
    LogisticsModule,
    SupplyModule,
    GiftsModule,
    ExpensesModule,
    DashboardModule,
    ExhibitionsModule,
    CorporateOrdersModule,
    NgxWebstorageModule.forRoot(),
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right',
      preventDuplicates: true
    })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
