import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { NgbDate, NgbDatepickerI18n } from '@ng-bootstrap/ng-bootstrap';

import { DateRange } from '../../models/data-range.mode';
import { DatetimeService } from '../../../core/datetime/datetime.service';
import { I18n, CustomDatepickerI18n } from '../../../../assets/i18n/datepicker-i18n';

@Component({
  selector: 'mk-daterage-modal',
  templateUrl: './daterage-modal.component.html',
  styleUrls: ['./daterage-modal.component.scss'],
  providers: [I18n, { provide: NgbDatepickerI18n, useClass: CustomDatepickerI18n }]
})
export class DateRageModalComponent implements OnInit {
  @Input() title = 'Диапазон дат';
  @Output() onApply = new EventEmitter<DateRange>();
  @Output() onClose = new EventEmitter<void>();

  hoveredDate: NgbDate;
  fromDate: NgbDate;
  toDate: NgbDate;

  constructor(private datetimeService: DatetimeService) { }

  ngOnInit() {
    // this.localeService.use('ru');
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
    const convertedFromDate = this.datetimeService.convertNgbDateToDate(this.fromDate);
    let convertedToDate = this.datetimeService.convertNgbDateToDate(this.toDate);
    
    this.onApply.emit(new DateRange(convertedFromDate, convertedToDate));
  }

  close() {
    this.onClose.emit();
  }
}
