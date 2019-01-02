import { Component, OnInit, Input, Output, EventEmitter, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

import { Product } from '../../models/product.model';

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
  @Input() products: Product;
  @Input() notFoundText = 'Нет данных';
  @Input() placeholder = 'Выберете товар';
  @Input() canClearAll = true;
  @Output() onProductSelected = new EventEmitter<Product>();
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

  onChange(product: Product) {
    this.onProductSelected.emit(product);
    this.onChangeCallback(product);
  }

  customSearchFn(term: string, item: Product) {
    term = term.toLocaleLowerCase();
    return item.name.toLocaleLowerCase().indexOf(term) > -1 || item.vendorCode.toLocaleLowerCase() === term;
  }

  clear() {
    this.onClear.emit();
  }

  private onChangeCallback: any = () => {};
}
