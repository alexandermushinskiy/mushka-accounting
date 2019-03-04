import { Component, OnInit, Input, Output, EventEmitter, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

import { Product } from '../../models/product.model';
import { SelectProduct } from '../../models/select-product.model';

@Component({
  selector: 'mk-select-products',
  templateUrl: './select-products.component.html',
  styleUrls: ['./select-products.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => SelectProductsComponent),
    multi: true
  }]
})
export class SelectProductsComponent implements OnInit, ControlValueAccessor {
  @Input() products: SelectProduct[];
  @Input() notFoundText = 'Товар не найден';
  @Input() placeholder = 'Выберете товар';
  @Input() canClearAll = true;
  @Input() isMultiple = false;
  @Output() onProductSelected = new EventEmitter<SelectProduct>();
  @Output() onClear = new EventEmitter<any>();

  selectedId: string;
  isLoading = false;

  constructor() { }

  ngOnInit() {
  }

  writeValue(product: Product) {
    if (product) {
      this.selectedId = product.id;
    }
  }

  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  registerOnTouched() {
  }

  onChange(product: SelectProduct) {
    this.onChangeCallback(product);
    this.onProductSelected.emit(product);
  }

  customSearchFn(term: string, item: SelectProduct) {
    term = term.toLocaleLowerCase();
    return item.name.toLocaleLowerCase().includes(term) || item.vendorCode.toLocaleLowerCase().includes(term);
  }

  clear() {
    this.onClear.emit();
  }

  private onChangeCallback: any = () => {};
}
