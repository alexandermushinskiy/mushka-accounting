import { NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import { AppLoaderService } from './app-loader/app-loader.service';
import { SidebarMenuStateService } from './sidebar-menu-state/sidebar-menu-state.service';
// import { NotificationsService } from './notifications/notifications.service';
// import { UserNotificationsService } from './user-notifications/user-notifications.service';
import { ProductsServce } from './api/products.service';
import { ConverterService } from './converter/converter.service';
import { DatetimeService } from './datetime/datetime.service';
import { AnalyticsService } from './api/analytics.service';
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
    ProductsServce,
    ConverterService,
    DatetimeService,
    AnalyticsService,
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
