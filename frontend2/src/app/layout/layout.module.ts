import { NgModule } from '@angular/core';

import { HeaderComponent } from './header/header.component';
import { SharedModule } from '../shared/shared.module';
import { CurrentUserComponent } from './shared/widgets/current-user/current-user.component';
import { GlobalSearchComponent } from './shared/widgets/global-search/global-search.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { SidebarMenuComponent } from './shared/widgets/sidebar-menu/sidebar-menu.component';

@NgModule({
  imports: [
    SharedModule
  ],
  declarations: [
    HeaderComponent,
    CurrentUserComponent,
    GlobalSearchComponent,
    SidebarComponent,
    SidebarMenuComponent
  ],
  exports: [
    HeaderComponent,
    SidebarComponent
  ]
})
export class LayoutModule { }
