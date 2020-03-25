import { Component, OnInit, Input, Output, EventEmitter, ViewChild, ElementRef } from '@angular/core';
import { NgbModalRef, NgbModalOptions, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import * as moment from 'moment';

import { TimeFrame } from '../../enums/time-frame.enum';
import { DateRange } from '../../models/date-range.model';
import { DatetimeService } from '../../../core/datetime/datetime.service';

@Component({
  selector: 'mshk-select-timeframes',
  templateUrl: './select-timeframes.component.html',
  styleUrls: ['./select-timeframes.component.scss']
})
export class SelectTimeframesComponent implements OnInit {
  @ViewChild('dateRangeTmpl', { static: false }) dateRangeTmpl: ElementRef;
  @Input() disabled = false;
  @Input() placeholder = 'common.selectPeriod';
  @Input() dateFormat = 'YYYY-MM-DD';
  @Output() onRangeSelected = new EventEmitter<DateRange>();
  @Output() onClear = new EventEmitter();

  modal: NgbModalRef;
  selectedTimeFrame: TimeFrame;
  dateRange: DateRange;

  timeframes = [
    { id: TimeFrame.LAST_MONTH, name: 'timeframe.lastMonth' },
    { id: TimeFrame.CURRENT_MONTH, name: 'timeframe.currentMonth' },
    { id: TimeFrame.CURRENT_QUARTER, name: 'timeframe.currentQurter' },
    { id: TimeFrame.CURRENT_YEAR, name: 'timeframe.currentYear' }
  ];

  private readonly dateRangModalConfig: NgbModalOptions = {
    windowClass: 'date-range-modal',
    backdrop: 'static',
    size: 'sm'
  };

  constructor(private modalService: NgbModal,
              private datetimeService: DatetimeService) {
  }

  ngOnInit() {
  }

  onTimeFrameChanged(timeFrame: TimeFrame) {
    this.selectedTimeFrame = timeFrame;
    const dateRange = this.getDateRange(timeFrame);
    this.selectDateRange(dateRange);
  }

  showDateRangeSelection() {
    this.modal = this.modalService.open(this.dateRangeTmpl, this.dateRangModalConfig);
  }

  clear() {
    this.selectedTimeFrame = null;
    this.dateRange = null;
    this.removeCustomeRange();
    this.onClear.emit();
  }

  onDateRangeSelected(dateRange: DateRange) {
    this.closeModal();

    this.selectedTimeFrame = TimeFrame.CUSTOM_RANGE;
    const rangeName = this.getRangeName(dateRange);

    const customRange = this.timeframes.find(time => time.id === TimeFrame.CUSTOM_RANGE);

    if (!!customRange) {
      customRange.name = rangeName;
    } else {
      this.timeframes.push({ id: TimeFrame.CUSTOM_RANGE, name: rangeName });
    }

    this.selectDateRange(dateRange);
  }

  closeModal() {
    if (this.modal) {
      this.modal.close();
    }
  }

  private getRangeName(dateRange: DateRange): string {
    const from = this.datetimeService.convertToFormat(dateRange.from);

    if (!dateRange.to) {
      return from;
    }

    const to = this.datetimeService.convertToFormat(dateRange.to);
    return `${from} - ${to}`;
  }

  private removeCustomeRange() {
    const index = this.timeframes.findIndex(time => time.id === TimeFrame.CUSTOM_RANGE);
    if (index >= 0) {
      this.timeframes.splice(index, 1);
    }
  }

  private selectDateRange(dateRange: DateRange) {
    this.dateRange = dateRange;
    this.onRangeSelected.emit(this.dateRange);
  }

  private getDateRange(timeFrame: TimeFrame): DateRange {
    switch (timeFrame) {
      case TimeFrame.LAST_MONTH:
        return this.getLastMonthRange();

      case TimeFrame.CURRENT_MONTH:
        return this.getCurrentMonthRange();

      case TimeFrame.CURRENT_QUARTER:
        return this.getCurrentQuarterRange();

      case TimeFrame.CURRENT_YEAR:
        return this.getCurrentYearRange();

      default:
        return null;
    }
  }

  private getLastMonthRange(): DateRange {
    const lastMonth = moment().subtract(1, 'month');
    const startOfMonth = lastMonth.startOf('month').format(this.dateFormat);
    const endOfMonth = lastMonth.endOf('month').format(this.dateFormat);

    return new DateRange(startOfMonth, endOfMonth);
  }

  private getCurrentMonthRange(): DateRange {
    const startOfMonth = moment().startOf('month').format(this.dateFormat);
    const endOfMonth = moment().endOf('month').format(this.dateFormat);

    return new DateRange(startOfMonth, endOfMonth);
  }

  private getCurrentQuarterRange(): DateRange {
    const currentQuarter = moment().startOf('quarter');
    const startOfQuarter = currentQuarter.format(this.dateFormat);
    const endOfQuarter = currentQuarter.format(this.dateFormat);

    return new DateRange(startOfQuarter, endOfQuarter);
  }

  private getCurrentYearRange(): DateRange {
    const startOfYear = moment().startOf('year').format(this.dateFormat);
    const today = moment().format(this.dateFormat);

    return new DateRange(startOfYear, today);
  }
}
