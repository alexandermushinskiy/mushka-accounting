import { Injectable } from '@angular/core';
import * as moment from 'moment';

import { DateRange } from '../../../models/date-range.model';
import { TimeFrame } from '../enums/time-frame.enum';

@Injectable({
  providedIn: 'root'
})
export class CalculateDateRangeService {
  timeFrameCalcMaps = {
    [TimeFrame.LastWeek]: (dateFormat: string) => this.getLastWeekRange(dateFormat),
    [TimeFrame.LastMonth]: (dateFormat: string) => this.getLastMonthRange(dateFormat),
    [TimeFrame.CurrentWeek]: (dateFormat: string) => this.getCurrentWeekRange(dateFormat),
    [TimeFrame.CurrentMonth]: (dateFormat: string) => this.getCurrentMonthRange(dateFormat),
    [TimeFrame.CurrentQuarter]: (dateFormat: string) => this.getCurrentQuarterRange(dateFormat),
    [TimeFrame.CurrentYear]: (dateFormat: string) => this.getCurrentYearRange(dateFormat)
  };

  calculateDateRange(timeFrame: TimeFrame, dateFormat: string): DateRange {
    const calculationFunc = this.timeFrameCalcMaps[timeFrame];
    return calculationFunc(dateFormat);
  }

  private getLastWeekRange(dateFormat: string): DateRange {
    const lastWeek = moment().subtract(1, 'weeks');
    const from = lastWeek.startOf('weeks').format(dateFormat);
    const to = lastWeek.endOf('weeks').format(dateFormat);

    return new DateRange(from, to);
  }

  private getLastMonthRange(dateFormat: string): DateRange {
    const lastMonth = moment().subtract(1, 'month');
    const from = lastMonth.startOf('month').format(dateFormat);
    const to = lastMonth.endOf('month').format(dateFormat);

    return new DateRange(from, to);
  }

  private getCurrentWeekRange(dateFormat: string): DateRange {
    const from = moment().startOf('weeks').format(dateFormat);
    const to = moment().endOf('weeks').format(dateFormat);

    return new DateRange(from, to);
  }

  private getCurrentMonthRange(dateFormat: string): DateRange {
    const from = moment().startOf('month').format(dateFormat);
    const to = moment().endOf('month').format(dateFormat);

    return new DateRange(from, to);
  }

  private getCurrentQuarterRange(dateFormat: string): DateRange {
    const from = moment().startOf('quarter').format(dateFormat);
    const to = moment().endOf('quarter').format(dateFormat);

    return new DateRange(from, to);
  }

  private getCurrentYearRange(dateFormat: string): DateRange {
    const from = moment().startOf('year').format(dateFormat);
    const to = moment().endOf('year').format(dateFormat);

    return new DateRange(from, to);
  }
}
