import * as moment from 'moment';

import { QuickFilter } from './quick-filter';
import { Order } from '../models/order.model';

export class OrderQuickFilter {
  getFilters(): QuickFilter[] {
    return [
      new QuickFilter(this.filterToday, 'Сегодня'),
      new QuickFilter(this.filterYesterday, 'Вчера'),
      new QuickFilter(this.filterLastWeek, 'Прошлая неделя'),
      new QuickFilter(this.filterCurrentWeek, 'Текущая неделя'),
      new QuickFilter(this.filterLastMonth, 'Прошлый месяц'),
      new QuickFilter(this.filterCurrentMonth, 'Текущий месяц'),
      new QuickFilter(this.filterCurrentQuarter, 'Текущий квартал'),
      new QuickFilter(this.filterCustomRange, 'Ввести диапазон')
    ];
  }

  filterToday(order: Order): boolean {
    const today = moment().format('YYYY-MM-DD');
    return moment(order.orderDate).format('YYYY-MM-DD') === today;
  }

  filterYesterday(order: Order): boolean {
    const yesterday = moment().subtract(1, 'day').format('YYYY-MM-DD');
    return moment(order.orderDate).format('YYYY-MM-DD') === yesterday;
  }

  filterLastWeek(order: Order): boolean {
    const lastWeek = moment().subtract(1, 'weeks');
    const startOfWeek = lastWeek.startOf('isoWeek').format('YYYY-MM-DD');
    const endOfWeek = lastWeek.endOf('isoWeek').format('YYYY-MM-DD');

    return order.orderDate >= startOfWeek && order.orderDate <= endOfWeek;
  }

  filterCurrentWeek(order: Order): boolean {
    const startOfWeek = moment().startOf('isoWeek').format('YYYY-MM-DD');
    const endOfWeek = moment().endOf('isoWeek').format('YYYY-MM-DD');

    return order.orderDate >= startOfWeek && order.orderDate <= endOfWeek;
  }

  filterLastMonth(order: Order): boolean {
    const lastMonth = moment().subtract(1, 'month');
    const startOfMonth = lastMonth.startOf('month').format('YYYY-MM-DD');
    const endOfMonth = lastMonth.endOf('month').format('YYYY-MM-DD');

    return order.orderDate >= startOfMonth && order.orderDate <= endOfMonth;
  }

  filterCurrentMonth(order: Order): boolean {
    const startOfMonth = moment().startOf('month').format('YYYY-MM-DD');
    const endOfMonth = moment().endOf('month').format('YYYY-MM-DD');

    return order.orderDate >= startOfMonth && order.orderDate <= endOfMonth;
  }

  filterCurrentQuarter(order: Order): boolean {
    const currentQuarter = moment().startOf('quarter');
    const startOfQuarter = currentQuarter.format('YYYY-MM-DD');
    const endOfQuarter = currentQuarter.format('YYYY-MM-DD');

    return order.orderDate >= startOfQuarter && order.orderDate <= endOfQuarter;
  }

  filterCustomRange(order: Order, fromDate: string, dateTo: string): boolean {
    return order.orderDate >= fromDate && order.orderDate <= dateTo;
  }
}
