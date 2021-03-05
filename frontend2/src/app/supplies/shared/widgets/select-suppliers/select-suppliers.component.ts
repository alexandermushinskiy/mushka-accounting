import { Component, OnInit, Input, Output, EventEmitter, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

import { Supplier } from '../../../../shared/models/supplier.model';

@Component({
  selector: 'mshk-select-suppliers',
  templateUrl: './select-suppliers.component.html',
  styleUrls: ['./select-suppliers.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => SelectSuppliersComponent),
    multi: true
  }]
})
export class SelectSuppliersComponent implements OnInit, ControlValueAccessor {
  @Input() isRequired = false;
  @Input() isDisabled = false;
  @Input() placeholder = 'common.selectSupplier';
  @Input() suppliers: Supplier[];
  @Output() onSupplierSelected = new EventEmitter<Supplier>();
  selectedSupplier: Supplier;

  constructor() { }

  ngOnInit() {
  }

  writeValue(supplier: Supplier): void {
    if (supplier) {
      this.selectedSupplier = this.suppliers.find(sup => sup.id === supplier.id);
    }
  }

  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  registerOnTouched() {
  }

  setDisabledState(isDisabled: boolean) {
    this.isDisabled = isDisabled;
  }

  onChange(supplier: Supplier) {
    this.selectedSupplier = supplier;

    this.onChangeCallback(supplier);
    this.onSupplierSelected.emit(supplier);
  }

  private onChangeCallback: any = () => {};
}
