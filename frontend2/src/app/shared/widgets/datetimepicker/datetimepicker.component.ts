import { Component, OnInit, Input, Output, ViewChild, ElementRef, EventEmitter, HostListener } from '@angular/core';
import { NgbDateStruct, NgbTimeStruct, NgbDatepicker, NgbDatepickerI18n } from '@ng-bootstrap/ng-bootstrap';
import * as moment from 'moment';

import { I18n, CustomDatepickerI18n } from '../../../../assets/i18n/datepicker-i18n';

@Component({
  selector: 'mshk-datetimepicker',
  templateUrl: './datetimepicker.component.html',
  styleUrls: ['./datetimepicker.component.scss'],
  providers: [I18n, { provide: NgbDatepickerI18n, useClass: CustomDatepickerI18n }]
})
export class DatetimepickerComponent implements OnInit {
  @ViewChild('outsideContent', { static: false }) outsideContent: ElementRef;
  @ViewChild('root', { static: false }) root: ElementRef;
  @ViewChild('dialog', { static: false }) dialog: ElementRef;
  @ViewChild('ngbdatepicker', { static: false }) ngbdatepicker: NgbDatepicker;

  @Output() onDateChanged = new EventEmitter<any>();

  @Input() positionElement: ElementRef;
  @Input() pickerType: 'calendar' | 'timer' | 'both';
  @Input() date: string;
  @Input() disabled: boolean;
  @Input() dateFormat = 'YYYY-MM-DD HH:mm:ss';
  @Input() isNow = false;
  @Input() hasConfirm = false;

  @Input() seconds = false;
  @Input() meridian = false;
  @Input() showTimeArrows = true;

  @Input() navigation = 'arrows';
  @Input() firstDayOfWeek = 1;
  @Input() displayMonths = 1;
  @Input() outsideDays = 'visible';
  @Input() showWeekdays = true;
  @Input() showWeekNumbers = false;

  @Input() set max(value: string) {
    this.maxDate = this.adapterDateToObject(value).calendarDate;
  }

  @Input() set min(value: string) {
    this.minDate = this.adapterDateToObject(value).calendarDate;
  }

  isOpen: boolean;
  isDatePicker: boolean;
  isTimePicker: boolean;
  dialogPosition: { top: number, left: number };
  dateModel: NgbDateStruct;
  timeModel: NgbTimeStruct;
  maxDate: NgbDateStruct;
  minDate: NgbDateStruct;

  constructor() { }

  ngOnInit() {
    this.setPickerType();
  }

  @HostListener('document:click', ['$event.target']) onClick(el) {
    if (this.disabled) {
      return;
    }
    const contentChild = this.getPositionElement();

    if (contentChild !== el && !this.root.nativeElement.contains(el) && el.parentNode && !el.parentNode.classList.contains('ngb-dp-day')) {
      this.close();
    }
  }

  open() {
    if (!this.isAllowedToOpen()) {
      return;
    }

    const dateObject = this.adapterDateToObject(this.date);

    this.dateModel = dateObject.calendarDate;
    this.timeModel = dateObject.timerDate;
    this.isOpen = !this.isOpen;
    this.dialogPosition = this.getDialogPosition();
    this.ngbdatepicker.navigateTo(this.dateModel);
  }

  dayChanged() {
    if (!this.hasConfirm) {
      this.confirm();
    }
  }

  close() {
    this.isOpen = false;
  }

  confirm() {
    if (this.isDatePicker && !this.dateModel ||
      this.isTimePicker && !this.timeModel) {
      return;
    }

    this.onDateChanged.emit(this.adapterDateObjectToString(this.dateModel, this.timeModel));

    this.close();
  }

  private isAllowedToOpen(): boolean {
    if (this.disabled) {
      return false;
    }

    if (this.maxDate) {
      const maxDate = moment.utc(new Date(this.maxDate.year, this.maxDate.month - 1, this.maxDate.day + 1));
      if (moment.utc(this.date, this.dateFormat).isAfter(maxDate)) {
        return false;
      }
    }

    return true;
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

  private adapterDateObjectToString(date: NgbDateStruct, time: NgbTimeStruct) {
    if (!date && !time) {
      return null;
    }

    if (date) {
      return moment.utc([date.year, date.month - 1, date.day, time.hour, time.minute, time.second]).format(this.dateFormat);
    }

    if (time) {
      return {
        hour: time.hour,
        minute: time.minute,
        second: time.second
      };
    }
  }

  private getDialogPosition() {
    const windowWidth = window.innerWidth;
    const contentPositionElement = this.getPositionElement();
    const contentPosition = contentPositionElement.getBoundingClientRect();
    const dialogWidth = this.dialog.nativeElement.offsetWidth;
    const position = {
      top: contentPosition.height,
      left: 0
    };

    if (windowWidth < dialogWidth + contentPosition.left) {
      position.left = -(dialogWidth - contentPosition.width);
    }

    return position;
  }

  private getPositionElement() {
    return this.positionElement ? this.positionElement : this.outsideContent.nativeElement.firstElementChild;
  }

  private setPickerType() {
    switch (this.pickerType) {
      case 'calendar':
        this.isDatePicker = true;
        this.isTimePicker = false;
        break;
      case 'timer':
        this.isDatePicker = false;
        this.isTimePicker = true;
        break;
      default:
        this.isDatePicker = true;
        this.isTimePicker = true;
    }
  }
}
