import { NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import { AppLoaderService } from './app-loader/app-loader.service';
import { SidebarMenuStateService } from './sidebar-menu-state/sidebar-menu-state.service';
// import { NotificationsService } from './notifications/notifications.service';
// import { UserNotificationsService } from './user-notifications/user-notifications.service';
import { CurrentUserService } from './api/current-user.service';
import { ProductsServce } from './api/products.service';
import { SuppliersService } from './api/suppliers.service';
import { SuppliesService } from './api/supplies.service';
import { ConverterService } from './converter/converter.service';
import { DatetimeService } from './datetime/datetime.service';
import { ExhibitionsService } from './api/exhibitions.service';
import { ExpensesService } from './api/expenses.service';
import { AnalyticsService } from './api/analytics.service';
import { CustomersService } from './api/customers.service';
import { NotificationsService } from './notifications/notifications.service';
import { LanguageService } from './language/language.service';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule
  ],
  providers: [
    AppLoaderService,
    SidebarMenuStateService,
    NotificationsService,
    // UserNotificationsService,
    CurrentUserService,
    ProductsServce,
    SuppliersService,
    SuppliesService,
    ConverterService,
    DatetimeService,
    ExhibitionsService,
    ExpensesService,
    AnalyticsService,
    CustomersService,
    LanguageService
  ],
  declarations: [],
  exports: []
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    if (parentModule) {
      throw new Error('CoreModule is already loaded. Import it in the AppModule only');
    }
  }
}
