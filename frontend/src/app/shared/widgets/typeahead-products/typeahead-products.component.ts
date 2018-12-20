import { Component, OnInit, forwardRef, Input } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { Observable } from 'rxjs';
import { TypeaheadMatch } from 'ngx-bootstrap';

import { Product } from '../../models/product.model';
import { ProductsServce } from '../../../core/api/products.service';

@Component({
  selector: 'mk-typeahead-products',
  templateUrl: './typeahead-products.component.html',
  styleUrls: ['./typeahead-products.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => TypeaheadProductsComponent),
    multi: true
  }]
})
export class TypeaheadProductsComponent implements OnInit, ControlValueAccessor {
  @Input() placeholder = 'Начните печать';
  @Input() isRequired = true;

  displayValue = '';
  selectedProduct: Product;
  items: Observable<Product[]>;
  foundItems: Product[] = [];
  isLoading = false;
  isCreateProduct = false;

  constructor(private productsServce: ProductsServce) { }

  ngOnInit() {
    this.items = Observable.create((observer: any) => {
      if (!this.displayValue) {
        return;
      }

      this.productsServce.getByCriteria(this.displayValue)
        .subscribe((result: Product[]) => {
          this.foundItems = result;
          observer.next(result);
      });
    });
  }

  onValueChanged(value: string) {
    this.displayValue = value;
    // this.onChangeCallback(this.value);
  }

  onLoading(isLoading: boolean) {
    this.isLoading = isLoading;
  }

  clear() {
    this.displayValue = '';
    this.onChangeCallback(null);
  }

  onSelectItem(match: TypeaheadMatch) {
    this.selectedProduct = match.item;
    this.onChangeCallback(this.selectedProduct);
  }

  onKeyup() {
    if (this.selectedProduct) {
      this.selectedProduct = null;
    }
  }

  writeValue(value: string): void {
    if (value) {
      this.displayValue = value;
    }
  }

  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  registerOnTouched() {
  }

  private onChangeCallback: any = () => {};
}
