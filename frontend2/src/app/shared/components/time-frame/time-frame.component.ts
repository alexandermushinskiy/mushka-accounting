import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { untilDestroyed } from 'ngx-take-until-destroy';

import { DatetimeService } from '../../../core/datetime/datetime.service';
import { DialogsService } from '../../components/dialogs/services/dialogs.service';
import { TimeFrame } from './enums/time-frame.enum';
import { DateRange } from '../../models/date-range.model';
import { CalculateDateRangeService } from './services/calculate-date-range.service';
import { TIME_FRAME_CONFIG } from './constants/time-frame-config.const';
import { I18N } from './constants/i18n.const';

@Component({
  selector: 'mshk-time-frame',
  templateUrl: './time-frame.component.html',
  styleUrls: ['./time-frame.component.scss']
})
export class TimeFrameComponent implements OnInit, OnDestroy {
  @Input() timeFrame: TimeFrame;
  @Output() onSelected = new EventEmitter<{ timeFrame?: TimeFrame, dateRange: DateRange }>();

  selectedValue: string;

  readonly i18n = I18N;
  readonly config = TIME_FRAME_CONFIG;
  readonly timeframes = this.config.items;

  constructor(private datetimeService: DatetimeService,
              private dialogService: DialogsService,
              private calculateDateRangeService: CalculateDateRangeService) {
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
  }

  onChange(timeFrame: TimeFrame): void {
    if (timeFrame === TimeFrame.CustomRange) {
      this.customRangeHandler();
    } else {
      const dateRange = this.calculateDateRangeService.calculateDateRange(timeFrame, this.config.dateFormat);
      this.selectTimeFrame(timeFrame, dateRange);
    }
  }

  clear(): void {
    this.selectedValue = null;
    this.onSelected.emit({ timeFrame: null, dateRange: this.config.defaultValue });
  }

  private selectTimeFrame(timeFrame: TimeFrame, dateRange: DateRange): void {
    this.selectedValue = this.getRangeValue(dateRange);
    this.onSelected.emit({ timeFrame, dateRange: (dateRange || this.config.defaultValue) });
  }

  private getRangeValue(dateRange: DateRange): string {
    const from = this.datetimeService.convertToFormat(dateRange.from);

    if (!dateRange.to) {
      return from;
    }

    const to = this.datetimeService.convertToFormat(dateRange.to);
    return `${from} - ${to}`;
  }

  private customRangeHandler(): void {
    const dialog = this.dialogService.openDateRangeDialog();

    dialog.confirm$
      .pipe(
        untilDestroyed(this)
      )
      .subscribe(({ dateRange }) => {
        dialog.close();
        this.selectTimeFrame(TimeFrame.CustomRange, dateRange);
      });
  }
}
