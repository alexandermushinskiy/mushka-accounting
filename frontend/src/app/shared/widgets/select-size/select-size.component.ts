import { Component, OnInit, forwardRef, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { NgSelectComponent } from '@ng-select/ng-select';

import { ProductSize } from '../../models/product-size.model';
import { SelectSize } from '../../models/select-size.model';
import { Size } from '../../models/size.model';

@Component({
  selector: 'mk-select-size',
  templateUrl: './select-size.component.html',
  styleUrls: ['./select-size.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => SelectSizeComponent),
    multi: true
  }]
})
export class SelectSizeComponent implements OnInit, ControlValueAccessor {
  @ViewChild('ngselect') ngselect: NgSelectComponent;
  @Input() set availableSizes(source: ProductSize[]) {
    if (source) {
      this.isLoading = false;
      this.sizes = source.map((size: Size) => new SelectSize({ id: size.id, name: size.name }));
    }
  }
  @Input() disabledSizes: string[];
  @Input() isMultiple = false;
  @Input() canClearAll = true;
  @Input() disabled = false;
  @Input() notFoundText = 'Нет данных';
  @Input() placeholder = 'Выберете размер';
  @Output() onSelectedSizes = new EventEmitter<string[]>();

  selectedId: string;
  sizes: SelectSize[] = [];
  isLoading = false;

  constructor() { }

  ngOnInit() {
  }

  onChange(selectedData: SelectSize) {
    this.onChangeCallback(this.getValue(selectedData));
  }

  writeValue(size: ProductSize) {
    if (!!size) {
      this.selectedId = size.id;
    } else {
      this.reset();
    }
  }

  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  registerOnTouched() {
  }
  
  setDisabledState(disabled: boolean) {
    this.disabled = disabled;
  }

  private reset() {
    this.ngselect.clearModel();
    this.ngselect.writeValue(null);
  }

  private getValue(selectedData: SelectSize) {
    if (!selectedData) {
      return null;
    }

    return new Size({id: selectedData.id, name: selectedData.name});
  }

  private onChangeCallback: any = () => {};
}
