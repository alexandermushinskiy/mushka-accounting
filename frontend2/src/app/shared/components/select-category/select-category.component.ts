import { Component, OnInit, Input, forwardRef, Output, EventEmitter } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

import { Category } from '../../models/category.model';

@Component({
  selector: 'mshk-select-category',
  templateUrl: './select-category.component.html',
  styleUrls: ['./select-category.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => SelectCategoryComponent),
    multi: true
  }]
})
export class SelectCategoryComponent implements OnInit, ControlValueAccessor {
  @Input() categories: Category[] = [];
  @Input() disabled = false;
  @Input() isLoading = false;
  @Output() onSelected = new EventEmitter<Category>();

  selectedId: string;

  constructor() { }

  ngOnInit() {
  }

  onChange(сategory: Category) {
    this.onChangeCallback(сategory);
    this.onSelected.emit(сategory);
  }

  writeValue(category: Category) {
    if (!!category) {
      this.selectedId = category.id;
    }
  }

  setDisabledState(disabled: boolean) {
    this.disabled = disabled;
  }

  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  registerOnTouched() {
  }

  private onChangeCallback: any = () => {};
}
