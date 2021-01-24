import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import * as moment from 'moment';

import { DatetimeService } from '../../../../core/datetime/datetime.service';
import { TimeFrame } from '../../../enums/time-frame.enum';
import { DateRange } from '../../../models/date-range.model';
import { I18N } from '../constants/i18n.const';

@Component({
  selector: 'mshk-time-frame',
  templateUrl: './time-frame.component.html',
  styleUrls: ['./time-frame.component.scss']
})
export class TimeFrameComponent implements OnInit {
  @Input() dateFormat = 'YYYY-MM-DD';
  @Input() timeFrame: TimeFrame;
  @Output() onSelected = new EventEmitter<{ timeFrame?: TimeFrame, dateRange: DateRange }>();

  selectedRangeName: string;

  readonly timeframes = [
    { id: TimeFrame.LAST_WEEK, name: I18N.timeFrames.lastWeek },
    { id: TimeFrame.LAST_MONTH, name: I18N.timeFrames.lastMonth },
    { id: TimeFrame.CURRENT_WEEK, name: I18N.timeFrames.currentWeek },
    { id: TimeFrame.CURRENT_MONTH, name: I18N.timeFrames.currentMonth },
    { id: TimeFrame.CURRENT_QUARTER, name: I18N.timeFrames.currentQurter },
    { id: TimeFrame.CURRENT_YEAR, name: I18N.timeFrames.currentYear }
  ];

  private readonly defaultValue = {
    from: null,
    to: null
  };

  constructor(private datetimeService: DatetimeService) { }

  ngOnInit(): void {
  }

  onTimeFrameSelect(timeFrame: TimeFrame) {
    const dateRange = this.getDateRange(timeFrame);
    this.selectedRangeName = this.getRangeName(dateRange);

    this.onSelected.emit({ timeFrame, dateRange: (dateRange || this.defaultValue) });
  }

  clear() {
    this.selectedRangeName = null;
    this.onSelected.emit({ timeFrame: null, dateRange: this.defaultValue });
  }

  private getRangeName(dateRange: DateRange): string {
    const from = this.datetimeService.convertToFormat(dateRange.from);

    if (!dateRange.to) {
      return from;
    }

    const to = this.datetimeService.convertToFormat(dateRange.to);
    return `${from} - ${to}`;
  }

  // private selectDateRange(dateRange: DateRange) {
  //   this.onRangeSelected.emit({ ...(dateRange || this.defaultValue) });
  // }

  private getDateRange(timeFrame: TimeFrame): DateRange {
    switch (timeFrame) {
      case TimeFrame.LAST_WEEK:
        return this.getLastWeekRange();

      case TimeFrame.LAST_MONTH:
        return this.getLastMonthRange();

      case TimeFrame.CURRENT_WEEK:
        return this.getCurrentWeekRange();

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

  private getLastWeekRange(): DateRange {
    const lastWeek = moment().subtract(1, 'weeks');
    const from = lastWeek.startOf('weeks').format(this.dateFormat);
    const to = lastWeek.endOf('weeks').format(this.dateFormat);

    return new DateRange(from, to);
  }

  private getLastMonthRange(): DateRange {
    const lastMonth = moment().subtract(1, 'month');
    const from = lastMonth.startOf('month').format(this.dateFormat);
    const to = lastMonth.endOf('month').format(this.dateFormat);

    return new DateRange(from, to);
  }

  private getCurrentWeekRange(): DateRange {
    const from = moment().startOf('weeks').format(this.dateFormat);
    const to = moment().endOf('weeks').format(this.dateFormat);

    return new DateRange(from, to);
  }

  private getCurrentMonthRange(): DateRange {
    const from = moment().startOf('month').format(this.dateFormat);
    const to = moment().endOf('month').format(this.dateFormat);

    return new DateRange(from, to);
  }

  private getCurrentQuarterRange(): DateRange {
    const from = moment().startOf('quarter').format(this.dateFormat);
    const to = moment().endOf('quarter').format(this.dateFormat);

    return new DateRange(from, to);
  }

  private getCurrentYearRange(): DateRange {
    const from = moment().startOf('year').format(this.dateFormat);
    const to = moment().endOf('year').format(this.dateFormat);

    return new DateRange(from, to);
  }
}
