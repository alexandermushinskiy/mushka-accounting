import { Component, OnInit, Input, Output, EventEmitter, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { DatetimeService } from '../../../core/datetime/datetime.service';

@Component({
  selector: 'mshk-datetimepicker-wrapper',
  templateUrl: './datetimepicker-wrapper.component.html',
  styleUrls: ['./datetimepicker-wrapper.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => DatetimepickerWrapperComponent),
    multi: true
  }]
})
export class DatetimepickerWrapperComponent implements OnInit, ControlValueAccessor {
  @Input() placeholder = 'Введите дату';
  @Input() required = false;
  @Output() onDateChanged = new EventEmitter<any>();

  dateValue: string;

  constructor(private datetimeService: DatetimeService) { }

  ngOnInit() {
  }

  dateChanged(newValue: string) {
    this.dateValue = newValue;

    this.onChangeCallback(newValue);
    this.onDateChanged.emit(newValue);
  }

  writeValue(value: any): void {
    if (value) {
      this.dateValue = this.datetimeService.toString(value);
    }
  }

  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  registerOnTouched() {
  }

  private onChangeCallback: any = () => {};
}
