import { Injectable } from '@angular/core';
import * as moment from 'moment';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap';

import { DateRange } from '../../shared/models/date-range.model';

@Injectable({
  providedIn: 'root'
})
export class DatetimeService {
  // parseDate(date: string, format = 'YYYY-MM-DD HH:mm'): string {
  //   const utcDate = moment.utc(date);
  //   return moment(utcDate).local().format(format);
  // }

  convertToFormat(date: string, format = 'DD MMM YYYY'): string {
    return moment(date).locale('ru').format(format);
  }

  getCurrentDateInString(format = 'YYYY-MM-DD'): string {
    return this.toString(new Date(), format);
  }

  toString(date: Date, format = 'YYYY-MM-DD'): string {
    return moment(date).locale('ru').format(format);
  }

  convertFromToFormat(date: string, inputFormat = 'DD MMM YYYY', outputFormat = 'YYYY-MM-DD'): string {
    return moment(date, inputFormat).locale('ru').format(outputFormat);
  }

  convertNgbDate(ngbDate: NgbDate): string {
    if (!ngbDate) {
      return null;
    }

    const date = new Date(ngbDate.year, ngbDate.month - 1, ngbDate.day);
    return this.toString(date);
  }

  isDateRangeCorrect(fromDate: string, toDate: string): boolean {
    return moment(fromDate) <= moment(toDate);
  }

  getMonthName(date: string): string {
    return this.capitalized(moment(date).locale('ru').format('MMM \'YY'));
  }

  isBetween(date: string, dateRange: DateRange): boolean {
    return moment(date).isBetween(dateRange.from, dateRange.to);
  }

  private capitalized(str: string): string {
    return str.charAt(0).toUpperCase() + str.slice(1);
  }
}
