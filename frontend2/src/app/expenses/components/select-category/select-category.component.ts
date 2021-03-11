import { Component, OnInit, Output, EventEmitter, Input, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

import { ExpenseCategory } from '../../enums/expense-category.enum';

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
  @Input() disabled = false;
  @Input() placeholder = 'common.selectCategory';
  @Output() onCategorySelected = new EventEmitter<ExpenseCategory>();
  selectedCategory: ExpenseCategory;

  categories = [
    { id: ExpenseCategory.ADVERTISING, name: 'expenceCategory.advertising' },
    { id: ExpenseCategory.DESIGN, name: 'expenceCategory.design' },
    { id: ExpenseCategory.EQUIPMENT, name: 'expenceCategory.equipment' },
    { id: ExpenseCategory.PHOTOGRAPHY, name: 'expenceCategory.photography' },
    { id: ExpenseCategory.POLYGRAPHY, name: 'expenceCategory.polygraphy' },
    { id: ExpenseCategory.PROMO, name: 'expenceCategory.promo' },
    { id: ExpenseCategory.WEBSITE, name: 'expenceCategory.website' },
    { id: ExpenseCategory.OTHER, name: 'expenceCategory.other' }
  ];

  constructor() {
  }

  ngOnInit() {
  }

  onCategoryChanged(category: ExpenseCategory) {
    this.selectedCategory = category;
    this.emitCategory();
    this.onChangeCallback(category);
  }

  clear() {
    this.selectedCategory = null;
    this.emitCategory();
  }

  writeValue(category: ExpenseCategory): void {
    this.selectedCategory = category;
  }

  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  setDisabledState(isDisabled: boolean) {
    this.disabled = isDisabled;
  }

  registerOnTouched() {
  }

  private emitCategory() {
    this.onCategorySelected.emit(this.selectedCategory);
  }

  private onChangeCallback: any = () => {};
}
