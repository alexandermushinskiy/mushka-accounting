import { Component, Input, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { NgbDateParserFormatter, NgbDatepickerI18n, NgbDateStruct, NgbInputDatepicker } from '@ng-bootstrap/ng-bootstrap';

import { DatetimeService } from '../../../core/datetime/datetime.service';
import { IconColor } from '../icon/enums/icon-color.enum';
import { IconName } from '../icon/enums/icon-name.enum';
import { DatepickerFormatter } from './services/datepicker-formatter.service';
import { DatepickerI18nService } from './services/datepicker-i18n.service';

@Component({
  selector: 'mshk-datepicker',
  templateUrl: './datepicker.component.html',
  styleUrls: ['./datepicker.component.scss'],
  providers: [
    { provide: NgbDatepickerI18n, useClass: DatepickerI18nService },
    { provide: NgbDateParserFormatter, useClass: DatepickerFormatter }
  ]
})
export class DatepickerComponent implements OnInit, OnChanges {
  @ViewChild('datepicker', { static: true }) datepicker: NgbInputDatepicker;
  @Input() control = new FormControl();
  @Input() placeholder = '';

  formControl = new FormControl();

  readonly calendarIconName = IconName.Calendar;
  readonly calendarIconColor = IconColor.Grey;

  constructor(private dateTimeService: DatetimeService) { }

  ngOnInit() {
    this.formControl.valueChanges
      .subscribe(() => {
        const jsDate = this.toDate(this.formControl.value);
        this.control.setValue(this.dateTimeService.toString(jsDate));
      });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.control) {
      const ngbDate = this.toNgbDateStruct(this.control.value);
      this.formControl.setValue(ngbDate);
    }
  }

  togglePicker(): void {
    this.datepicker.toggle();
  }

  isOpen(): boolean {
    return this.datepicker.isOpen();
  }

  private toNgbDateStruct(value: string): NgbDateStruct {
    const date = this.dateTimeService.toDate(value);

    return {
      year: date.getFullYear(),
      month: date.getMonth() + 1,
      day: date.getDate()
    };
  }

  private toDate(ngbDate: NgbDateStruct): Date {
    return new Date(ngbDate.year, ngbDate.month - 1, ngbDate.day);
  }
}
