import { Component, OnInit } from '@angular/core';
//import { LocalStorage } from 'ngx-webstorage';

import { SidebarMenuStateService } from '../../core/sidebar-menu-state/sidebar-menu-state.service';

@Component({
  selector: 'mshk-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {
  //@LocalStorage() 
  isCollapsed = false;

  constructor(private sidebarMenuStateService: SidebarMenuStateService) {
    this.sidebarMenuStateService.setCollapsedState(this.isCollapsed);
  }

  ngOnInit() {
  }

  toggleCollapseMode() {
    this.isCollapsed = !this.isCollapsed;
    this.sidebarMenuStateService.setCollapsedState(this.isCollapsed);
  }
}
