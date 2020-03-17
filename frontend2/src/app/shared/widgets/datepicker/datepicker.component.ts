import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import * as moment from 'moment';

@Component({
  selector: 'mshk-datepicker',
  templateUrl: './datepicker.component.html',
  styleUrls: ['./datepicker.component.scss']
})
export class DatepickerComponent implements OnInit {
  @Input() disabled: boolean;
  @Input() navigation = 'arrows';
  @Input() isNow = false;
  @Input() dateFormat = 'YYYY-MM-DD HH:mm:ss';
  @Input() firstDayOfWeek = 1;
  @Input() displayMonths = 1;
  @Input() outsideDays = 'visible';
  @Input() showWeekdays = true;
  @Input() showWeekNumbers = false;
  @Input() hasConfirm = false;

  @Input() set max(value: string) {
    this.maxDate = this.adapterDateToObject(value).calendarDate;
  }

  @Input() set min(value: string) {
    this.minDate = this.adapterDateToObject(value).calendarDate;
  }

  @Output() onDateChanged = new EventEmitter<any>();

  dateModel: NgbDateStruct;
  maxDate: NgbDateStruct;
  minDate: NgbDateStruct;

  constructor() { }

  ngOnInit() {
  }

  private adapterDateToObject(date: string) {
    let dateData = moment(date, this.dateFormat);

    if (!dateData.isValid() && this.isNow) {
      dateData = moment();
    }

    return {
      calendarDate: { year: dateData.get('year'), month: dateData.get('month') + 1, day: dateData.get('date') },
      timerDate: { hour: dateData.get('hour') || 0, minute: dateData.get('minute') || 0, second: dateData.get('second') || 0 }
    };
  }
}
