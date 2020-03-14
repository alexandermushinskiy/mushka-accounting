import { Component, OnInit, forwardRef, Input, Output, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

@Component({
  selector: 'mshk-dropdown',
  templateUrl: './dropdown.component.html',
  styleUrls: ['./dropdown.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => DropdownComponent),
    multi: true
  }]
})
export class DropdownComponent implements OnInit, ControlValueAccessor {
  @Input() options: any[];
  @Input() dataField: string;
  @Input() textField: string;
  // @Input() initialValue: string;
  @Input() required: boolean;
  @Input() disabled = false;
  // @Input() defaultValue: string;
  @Output() onSelectedValue = new EventEmitter<any>();

  value: any;

  get isComplex(): boolean {
    return !!this.dataField && !!this.textField;
  }

  constructor() { }

  ngOnInit() {
    // if (this.initialValue) {
    //   this.value = this.initialValue;
    // }
  }

  writeValue(value: any): void {
    if (value) {
      this.value = value; // this.isComplex ? this.options.find(opt => opt[this.dataField] === value) : value;

      setTimeout(() => {
        this.onChangeCallback(this.value);
      }, 100);
    }
  }

  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  setDisabledState(disabled: boolean) {
    this.disabled = disabled;
  }

  registerOnTouched() {
  }

  onOptionSelect(option: any) {
    this.value = option;

    this.onChangeCallback(option);
    this.onSelectedValue.emit(option);
  }

  private onChangeCallback: any = () => {};
}
