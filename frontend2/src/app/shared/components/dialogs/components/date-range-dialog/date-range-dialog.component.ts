import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { BsModalRef } from 'ngx-bootstrap';

import { DatetimeService } from '../../../../../core/datetime/datetime.service';
import { DateRange } from '../../../../models/date-range.model';
import { BaseDialogComponent } from '../base-dialog/base-dialog.component';
import { DateRangeDialogResult } from './interfaces/date-range-dialog-result.interface';

@Component({
  selector: 'mshk-date-range-dialog',
  templateUrl: './date-range-dialog.component.html',
  styleUrls: ['./date-range-dialog.component.scss']
})
export class DateRangeDialogComponent extends BaseDialogComponent<DateRangeDialogResult> {
  @Input() title = 'TITLE';
  @Output() onApply = new EventEmitter<DateRange>();
  @Output() onClose = new EventEmitter<void>();

  hoveredDate: NgbDate;
  fromDate: NgbDate;
  toDate: NgbDate;

  constructor(bsModalRef: BsModalRef,
              private datetimeService: DatetimeService) {
    super(bsModalRef);
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

  confirmAction(): void {
    if (!this.canConfirm) {
      return;
    }

    const fromDate = this.datetimeService.convertNgbDate(this.fromDate);
    const toDate = this.datetimeService.convertNgbDate(this.toDate);

    this.confirm$.next({ dateRange: new DateRange(fromDate, toDate) });
  }
}
