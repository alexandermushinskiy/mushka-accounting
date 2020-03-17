import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { NgbDate, NgbDatepickerI18n } from '@ng-bootstrap/ng-bootstrap';

import { I18n, CustomDatepickerI18n } from '../../../../assets/i18n/datepicker-i18n';
import { DateRange } from '../../models/date-range.model';
import { DatetimeService } from '../../../core/datetime/datetime.service';

@Component({
  selector: 'mshk-date-range-modal',
  templateUrl: './date-range-modal.component.html',
  styleUrls: ['./date-range-modal.component.scss'],
  providers: [I18n, { provide: NgbDatepickerI18n, useClass: CustomDatepickerI18n }]
})
export class DateRangeModalComponent implements OnInit {
  @Input() title = 'timeframe.range';
  @Output() onApply = new EventEmitter<DateRange>();
  @Output() onClose = new EventEmitter<void>();

  hoveredDate: NgbDate;
  fromDate: NgbDate;
  toDate: NgbDate;

  constructor(private datetimeService: DatetimeService) { }
  
  ngOnInit() {
  }

  onDateSelection(date: NgbDate) {
    if (!this.fromDate && !this.toDate) {
      this.fromDate = date;
    } else if (this.fromDate && !this.toDate && date.after(this.fromDate)) {
      this.toDate = date;
    } else {
      this.toDate = null;
      this.fromDate = date;
    }
  }

  isHovered(date: NgbDate) {
    return this.fromDate && !this.toDate && this.hoveredDate && date.after(this.fromDate) && date.before(this.hoveredDate);
  }

  isInside(date: NgbDate) {
    return date.after(this.fromDate) && date.before(this.toDate);
  }

  isRange(date: NgbDate) {
    return date.equals(this.fromDate) || date.equals(this.toDate) || this.isInside(date) || this.isHovered(date);
  }

  apply() {
    const convertedFromDate = this.datetimeService.convertNgbDate(this.fromDate);
    const convertedToDate = this.datetimeService.convertNgbDate(this.toDate);

    this.onApply.emit(new DateRange(convertedFromDate, convertedToDate));
  }

  close() {
    this.onClose.emit();
  }
}
