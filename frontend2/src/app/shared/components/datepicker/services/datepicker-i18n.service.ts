import { Injectable } from '@angular/core';
import { NgbDatepickerI18n, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

import { MONTH_FULL_NAMES } from '../constants/month-full-names.const';
import { MONTH_SHORT_NAMES } from '../constants/month-short-names.const';
import { WEEKDAY_SHORT_NAMES } from '../constants/weekday-short-names.const';

@Injectable({
  providedIn: 'root'
})
export class DatepickerI18nService extends NgbDatepickerI18n {

  private readonly weekdayShortNames = WEEKDAY_SHORT_NAMES;
  private readonly monthShortNames = MONTH_SHORT_NAMES;
  private readonly monthFullNames = MONTH_FULL_NAMES;

  constructor() {
    super();
  }

  getWeekdayShortName(weekday: number): string {
    return this.weekdayShortNames[weekday - 1];
  }

  getMonthShortName(month: number): string {
    return this.monthShortNames[month - 1];
  }

  getMonthFullName(month: number): string {
    return this.monthFullNames[month - 1];
  }

  getDayAriaLabel(date: NgbDateStruct): string {
    return `${date.day}-${date.month}-${date.year}`;
  }
}
