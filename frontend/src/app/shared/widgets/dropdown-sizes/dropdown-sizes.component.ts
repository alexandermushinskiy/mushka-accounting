import { Component, OnInit, forwardRef, Output, EventEmitter, Input } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

import { Size } from '../../models/size.model';
import { ProductsServce } from '../../../core/api/products.service';

@Component({
  selector: 'mk-dropdown-sizes',
  templateUrl: './dropdown-sizes.component.html',
  styleUrls: ['./dropdown-sizes.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => DropdownSizesComponent),
    multi: true
  }]
})
export class DropdownSizesComponent implements OnInit, ControlValueAccessor {
  @Input() isRequired = true;
  @Output() onSizeSelected = new EventEmitter<Size>();

  selectedSize = new Size({ name: 'Нет' });
  sizes: Size[];

  constructor(private productsService: ProductsServce) { }

  ngOnInit() {
    this.productsService.getSizes()
      .subscribe((sizes: Size[]) => {
        this.sizes = sizes;
        this.sizes.unshift(this.selectedSize);
      });
  }

  writeValue(value: Size): void {
    if (value) {
      this.selectedSize = value;
    }
  }

  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  registerOnTouched() {
  }

  onOptionSelected(size: Size) {
    this.selectedSize = size;

    this.onChangeCallback(size);
    this.onSizeSelected.emit(size);
  }

  private onChangeCallback: any = () => {};
}
