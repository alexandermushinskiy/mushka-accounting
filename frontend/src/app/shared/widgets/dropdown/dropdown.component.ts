import { Component, OnInit, Input, Output, EventEmitter, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'mk-dropdown',
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
  @Input() initialValue: string;
  @Input() required: boolean;
  @Input() isDisabled = false;
  @Input() defaultValue: string;
  @Output() onSelectedValue = new EventEmitter<any>();

  value: any;

  get isComplex(): boolean {
    return !!this.dataField && !!this.textField;
  }

  constructor() { }

  ngOnInit() {
    if (this.initialValue) {
      this.value = this.initialValue;
    }
  }
  
  writeValue(value: any): void {
    if (value) {
      this.value = this.isComplex ? this.options.find(opt => opt[this.dataField] === value) : value;
    }
  }

  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  setDisabledState(isDisabled: boolean) {
    this.isDisabled = isDisabled;
  }

  registerOnTouched() {
  }

  onOptionSelect(option: string) {
    this.value = option;

    this.onChangeCallback(option);
    this.onSelectedValue.emit(option);
  }

  onObjectSelect(object: any) {
    this.value = object;

    this.onChangeCallback(object);
    this.onSelectedValue.emit(object);
  }

  private onChangeCallback: any = () => {};
}
