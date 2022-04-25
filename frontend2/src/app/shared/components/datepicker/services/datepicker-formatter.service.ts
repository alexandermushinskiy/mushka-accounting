import { Injectable } from '@angular/core';
import { NgbDateParserFormatter, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

import { DatetimeService } from '../../../../core/datetime/datetime.service';

@Injectable({
  providedIn: 'root'
})
export class DatepickerFormatter extends NgbDateParserFormatter {
  private readonly dateFormat = 'DD MMM, YYYY';

  constructor(private dateTimeService: DatetimeService) {
    super();
  }

  parse(value: string): NgbDateStruct {
    if (!value) {
      return null;
    }

    const isValidDate = this.dateTimeService.isValid(value, this.dateFormat);

    if (!isValidDate) {
      return null;
    }

    const date = this.dateTimeService.toDate(value);

    return {
      year: date.getFullYear(),
      month: date.getMonth() + 1,
      day: date.getDate()
    };
  }

  format(ngbDate: NgbDateStruct): string {
    if (ngbDate === null) {
      return '';
    }

    const jsDate = new Date(ngbDate.year, ngbDate.month - 1, ngbDate.day);

    return this.dateTimeService.toString(jsDate, this.dateFormat);
  }
}
