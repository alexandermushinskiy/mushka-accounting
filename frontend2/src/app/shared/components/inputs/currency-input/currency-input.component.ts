import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';

import { IconColor } from '../../icon/enums/icon-color.enum';
import { IconName } from '../../icon/enums/icon-name.enum';

@Component({
  selector: 'mshk-currency-input2',
  templateUrl: './currency-input.component.html',
  styleUrls: ['./currency-input.component.scss']
})
export class CurrencyInput2Component implements OnInit {
  @Input() control = new FormControl();
  @Input() placeholder: string;
  @Input() label: string;
  @Input() errors?: { [key: string]: string };
  @Output() onBlur = new EventEmitter<number>();
  @Output() onClickOutside = new EventEmitter<number>();

  readonly warningIconName = IconName.Warning;
  readonly warningIconColor = IconColor.Danger;

  get hasError(): boolean {
    return !!this.control && this.control.invalid && (this.control.dirty || this.control.touched);
  }

  constructor() { }

  ngOnInit(): void {
  }

  blur(): void {
  }
}
