import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';

import { DatepickerComponent } from '../../datepicker/datepicker.component';
import { IconColor } from '../../icon/enums/icon-color.enum';
import { IconName } from '../../icon/enums/icon-name.enum';

@Component({
  selector: 'mshk-datepicker-input',
  templateUrl: './datepicker-input.component.html',
  styleUrls: ['./datepicker-input.component.scss']
})
export class DatepickerInputComponent implements OnInit {
  @ViewChild('datepicker', { static: true }) datepicker: DatepickerComponent;
  @Input() control = new FormControl();
  @Input() label: string;
  @Input() placeholder = '';
  @Input() errors?: { [key: string]: string };
  @Output() onChange = new EventEmitter<string>();

  readonly warningIconName = IconName.Warning;
  readonly warningIconColor = IconColor.Danger;

  get hasError(): boolean {
    return !!this.control && this.control.invalid && (this.control.dirty || this.control.touched);
  }

  constructor() { }

  ngOnInit(): void {
  }
}
