import { Component, OnInit, Input, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'mk-toggle',
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
  isChecked = true;

  constructor() { }

  ngOnInit() {
  }

  onChange(isChecked: boolean) {
    this.onChangeCallback(isChecked);
  }

  writeValue(isChecked: any): void {
    this.isChecked = isChecked;
  }

  registerOnChange(fn: any): void {
    this.onChangeCallback = fn;
  }

  registerOnTouched(fn: any): void {
  }

  private onChangeCallback: any = () => {};
}
