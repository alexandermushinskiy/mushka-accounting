import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { AppLoaderService } from './core/app-loader/app-loader.service';
import { SidebarMenuStateService } from './core/sidebar-menu-state/sidebar-menu-state.service';
import { environment } from '../environments/environment';

@Component({
  selector: 'mk-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  isAppReady = false;
  isCollapsed: boolean;

  constructor(private appLoaderService: AppLoaderService,
              private sidebarMenuStateService: SidebarMenuStateService,
              private titleService: Title) {
    this.titleService.setTitle(environment.production ? 'Mushka Admin' : 'Mushka Admin / Test');
  }

  ngOnInit() {
    setTimeout(() => {
      this.appLoaderService.bootstrap();
      this.isAppReady = true;
    }, 550);

    this.sidebarMenuStateService.isCollapsed()
      .subscribe(val => this.isCollapsed = val);
  }
}
