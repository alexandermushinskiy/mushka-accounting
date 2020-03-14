import { Component, OnInit, ChangeDetectorRef } from '@angular/core';

import { MenuItems } from '../../constants/menu-items.const';
import { SidebarMenuStateService } from '../../../../core/sidebar-menu-state/sidebar-menu-state.service';
import { MenuItem } from '../../models/menu-item.model';

@Component({
  selector: 'mshk-sidebar-menu',
  templateUrl: './sidebar-menu.component.html',
  styleUrls: ['./sidebar-menu.component.scss']
})
export class SidebarMenuComponent implements OnInit {
  menuItems = MenuItems;

  constructor(private cd: ChangeDetectorRef,
              private sidebarMenuStateService: SidebarMenuStateService) {
  }

  ngOnInit() {
  }

  onMenuItemSelected(menuItem: MenuItem) {
  }
}
