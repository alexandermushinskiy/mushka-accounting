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

  @Input() set dateRange(value: DateRange) {
    if (!!value) {
      this.selectedTimeFrame = this.getTimeFrameByDateRange(value);
      if (this.selectedTimeFrame === TimeFrame.CUSTOM_RANGE) {
        this.timeframes.push({ id: TimeFrame.CUSTOM_RANGE, name: this.getRangeName(value) });
      }
    }
  }

  @Output() onRangeSelected = new EventEmitter<DateRange>();

  modal: NgbModalRef;
  selectedTimeFrame: TimeFrame;

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

  clear() {
    this.selectedTimeFrame = null;
    this.removeCustomeRange();
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
    this.onRangeSelected.emit(dateRange);
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
    const startOfQuarter = moment().startOf('quarter').format(this.dateFormat);
    const endOfQuarter = moment().endOf('quarter').format(this.dateFormat);

    return new DateRange(startOfQuarter, endOfQuarter);
  }

  private getCurrentYearRange(): DateRange {
    const startOfYear = moment().startOf('year').format(this.dateFormat);
    const endOfYear = moment().endOf('year').format(this.dateFormat);

    return new DateRange(startOfYear, endOfYear);
  }

  private getTimeFrameByDateRange(dateRange: DateRange): TimeFrame {
    if (this.isLastMonth(dateRange)) {
      return TimeFrame.LAST_MONTH;
    } else if (this.isCurrentMonth(dateRange)) {
      return TimeFrame.CURRENT_MONTH;
    } else if (this.isCurentQuarter(dateRange)) {
      return TimeFrame.CURRENT_QUARTER;
    } else if (this.isCurrentYear(dateRange)) {
      return TimeFrame.CURRENT_YEAR;
    } else {
      return TimeFrame.CUSTOM_RANGE;
    }
  }

  private isLastMonth(dateRange: DateRange): boolean {
    const lastMonth = moment().subtract(1, 'month');
    const startOfMonth = lastMonth.startOf('month').format(this.dateFormat);
    const endOfMonth = lastMonth.endOf('month').format(this.dateFormat);

    return dateRange.from === startOfMonth && dateRange.to === endOfMonth;
  }

  private isCurrentMonth(dateRange: DateRange): boolean {
    const startOfMonth = moment().startOf('month').format(this.dateFormat);
    const endOfMonth = moment().endOf('month').format(this.dateFormat);

    return dateRange.from === startOfMonth && dateRange.to === endOfMonth;
  }

  private isCurentQuarter(dateRange: DateRange): boolean {
    const startOfQuarter = moment().startOf('quarter').format(this.dateFormat);
    const endOfQuarter = moment().endOf('quarter').format(this.dateFormat);

    return dateRange.from === startOfQuarter && dateRange.to === endOfQuarter;
  }

  private isCurrentYear(dateRange: DateRange): boolean {
    const startOfYear = moment().startOf('year').format(this.dateFormat);
    const endOfYear = moment().endOf('year').format(this.dateFormat);

    return dateRange.from === startOfYear && dateRange.to === endOfYear;
  }
}
