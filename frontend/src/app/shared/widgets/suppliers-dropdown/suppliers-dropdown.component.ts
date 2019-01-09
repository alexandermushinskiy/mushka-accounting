import { Component, OnInit, Input, Output, EventEmitter, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

import { Supplier } from '../../../shared/models/supplier.model';

@Component({
  selector: 'mk-suppliers-dropdown',
  templateUrl: './suppliers-dropdown.component.html',
  styleUrls: ['./suppliers-dropdown.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => SuppliersDropdownComponent),
    multi: true
  }]
})
export class SuppliersDropdownComponent implements OnInit, ControlValueAccessor {
  @Input() isRequired = false;
  @Input() isDisabled = false;
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

  onOptionSelected(supplier: Supplier) {
    this.selectedSupplier = supplier;

    this.onChangeCallback(supplier);
    this.onSupplierSelected.emit(supplier);
  }

  private onChangeCallback: any = () => {};
}
