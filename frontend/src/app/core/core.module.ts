import { NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import { AppLoaderService } from './app-loader/app-loader.service';
import { SidebarMenuStateService } from './sidebar-menu-state/sidebar-menu-state.service';
import { BadgesService } from './api/badges.service';
import { NotificationsService } from './notifications/notifications.service';
import { UserNotificationsService } from './user-notifications/user-notifications.service';
import { CurrentUserService } from './api/current-user.service';
import { BrowserDetectorService } from './browser-detector/browser-detector.service';
import { ProductsServce } from './api/products.service';
import { UserSettingsService } from './api/user-settings.service';
import { CategoriesService } from './api/categories.service';
import { SuppliersService } from './api/suppliers.service';
import { DeliveriesService } from './api/deliveries.service';
import { ConverterService } from './converter/converter.service';
import { DatetimeService } from './datetime/datetime.service';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule
  ],
  providers: [
    AppLoaderService,
    SidebarMenuStateService,
    BadgesService,
    NotificationsService,
    UserNotificationsService,
    CurrentUserService,
    BrowserDetectorService,
    ProductsServce,
    CategoriesService,
    UserSettingsService,
    SuppliersService,
    DeliveriesService,
    ConverterService,
    DatetimeService
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