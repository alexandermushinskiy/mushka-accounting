import { Component, OnInit, Input, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'mshk-toggle',
  templateUrl: './toggle.component.html',
  styleUrls: ['./toggle.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => ToggleComponent),
    multi: true
  }]
})
export class ToggleComponent implements OnInit, ControlValueAccessor {
  @Input() label: string;
  @Input() disabled = false;
  @Input() required = false;
  isChecked = true;

  constructor() { }

  ngOnInit() {
  }

  onChange(isChecked: boolean) {
    this.onChangeCallback(isChecked);
  }

  writeValue(isChecked: boolean): void {
    this.isChecked = isChecked;
  }

  registerOnChange(fn: any): void {
    this.onChangeCallback = fn;
  }

  setDisabledState(disabled: boolean) {
    this.disabled = disabled;
  }

  registerOnTouched(fn: any): void {
  }

  private onChangeCallback: any = () => {};
}
