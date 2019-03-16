import { MenuItem } from '../models/menu-item.model';
import { MenuLabels } from '../enums/menu-labels.enum';

export const MenuItems: MenuItem[] = [
  new MenuItem({
    title: MenuLabels.DASHBOARD,
    icon: 'fa-area-chart',
    routePath: '/dashboard',
    isEnabled: true
  }),
  new MenuItem({
    title: MenuLabels.ORDERS,
    icon: 'fa-shopping-cart',
    routePath: '/orders',
    isEnabled: true,
    badges: 0
  }),
  new MenuItem({
    title: MenuLabels.CORPORATE_ORDERS,
    icon: 'fa-building-o',
    routePath: '/corporate-orders',
    isEnabled: true
  }),
  new MenuItem({
    title: MenuLabels.PRODUCTS,
    icon: 'fa-shopping-bag',
    routePath: '/products',
    isEnabled: true
  }),
  new MenuItem({
    title: MenuLabels.SUPPLIES,
    icon: 'fa-archive',
    routePath: '/supplies',
    isEnabled: true,
  }),
  new MenuItem({
    title: MenuLabels.SUPPLIERS,
    icon: 'fa-users',
    routePath: '/suppliers',
    isEnabled: true,
  }),
  // new MenuItem({
  //   title: MenuLabels.PARTNERS,
  //   icon: 'fa-handshake-o',
  //   routePath: '/partners',
  //   isEnabled: false
  // }),
  // new MenuItem({
  //   title: MenuLabels.LOGISTICS,
  //   icon: 'fa-truck',
  //   routePath: '/logistics',
  //   isEnabled: false,
  // }),
  new MenuItem({
    title: MenuLabels.GIFTS,
    icon: 'fa-gift',
    routePath: '/gifts',
    isEnabled: true,
  }),
  new MenuItem({
    title: MenuLabels.EXHIBITIONS,
    icon: 'fa-id-badge',
    routePath: '/exhibitions',
    isEnabled: true,
  }),
  new MenuItem({
    title: MenuLabels.EXPENSES,
    icon: 'fa-money',
    routePath: '/expenses',
    isEnabled: true,
  }),
  new MenuItem({
    title: MenuLabels.HELP,
    icon: 'ico-help',
    routePath: '/help',
    isEnabled: true,
  })
];
