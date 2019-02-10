import * as moment from 'moment';

import { QuickFilter } from './quick-filter';
import { Order } from '../models/order.model';
import { DateRange } from '../models/data-range.mode';
import { OrderList } from '../../orders/shared/models/order-list.model';

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

  filterToday(order: OrderList): boolean {
    const today = moment().format('YYYY-MM-DD');
    return moment(order.orderDate).format('YYYY-MM-DD') === today;
  }

  filterYesterday(order: OrderList): boolean {
    const yesterday = moment().subtract(1, 'day').format('YYYY-MM-DD');
    return moment(order.orderDate).format('YYYY-MM-DD') === yesterday;
  }

  filterLastWeek(order: OrderList): boolean {
    const lastWeek = moment().subtract(1, 'weeks');
    const startOfWeek = lastWeek.startOf('isoWeek').format('YYYY-MM-DD');
    const endOfWeek = lastWeek.endOf('isoWeek').format('YYYY-MM-DD');

    return order.orderDate >= startOfWeek && order.orderDate <= endOfWeek;
  }

  filterCurrentWeek(order: OrderList): boolean {
    const startOfWeek = moment().startOf('isoWeek').format('YYYY-MM-DD');
    const endOfWeek = moment().endOf('isoWeek').format('YYYY-MM-DD');

    return order.orderDate >= startOfWeek && order.orderDate <= endOfWeek;
  }

  filterLastMonth(order: OrderList): boolean {
    const lastMonth = moment().subtract(1, 'month');
    const startOfMonth = lastMonth.startOf('month').format('YYYY-MM-DD');
    const endOfMonth = lastMonth.endOf('month').format('YYYY-MM-DD');

    return order.orderDate >= startOfMonth && order.orderDate <= endOfMonth;
  }

  filterCurrentMonth(order: OrderList): boolean {
    const startOfMonth = moment().startOf('month').format('YYYY-MM-DD');
    const endOfMonth = moment().endOf('month').format('YYYY-MM-DD');

    return order.orderDate >= startOfMonth && order.orderDate <= endOfMonth;
  }

  filterCurrentQuarter(order: OrderList): boolean {
    const currentQuarter = moment().startOf('quarter');
    const startOfQuarter = currentQuarter.format('YYYY-MM-DD');
    const endOfQuarter = currentQuarter.format('YYYY-MM-DD');

    return order.orderDate >= startOfQuarter && order.orderDate <= endOfQuarter;
  }

  filterCustomRange(order: OrderList, dateRange: DateRange): boolean {
    if (!dateRange.toDate) {
      return order.orderDate === dateRange.fromDate;
    }

    return order.orderDate >= dateRange.fromDate && order.orderDate <= dateRange.toDate;
  }
}
