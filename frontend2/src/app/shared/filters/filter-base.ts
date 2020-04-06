import * as moment from 'moment';

import { DateRange } from '../models/date-range.model';

export abstract class FiltersBase {

  containsSearchKey(value: string, searchKey: string): boolean {
    if (!value && !!searchKey) {
      return false;
    }

    return !searchKey || value.toLowerCase().includes(searchKey.toLowerCase());
  }

  isBetween(date: string, dateRange: DateRange): boolean {
    return moment(date).isBetween(dateRange.from, !!dateRange.to ? dateRange.to : dateRange.from, null, '[]');
  }

  isEmpty(): boolean {
    return !Object.values(this).some(value => !!value);
  }
}
