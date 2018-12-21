import { Component, OnInit, forwardRef, Input, Output, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

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
  @Input() set availableSizes(source: ProductSize[]) {
    if (source) {
      this.isLoading = false;
      this.sizes = source.map((size: Size) => new SelectSize({ id: size.id, name: size.name }));
    }
  }
  @Input() disabledSchools: string[];
  @Input() isMultiple = true;
  @Input() canClearAll = true;
  @Input() isDisablePreselected = false;
  @Input() isDisabled = false;
  @Input() notFoundText = 'Нет данных';
  @Input() placeholder = 'Выбирете размер(ы)';
  @Output() onSelectedSizes = new EventEmitter<string[]>();

  selectedIds: string[];
  sizes: SelectSize[] = [];
  isLoading = false;

  constructor() { }

  ngOnInit() {
  }

  onChange(selectedData: SelectSize | SelectSize[]) {
    this.onChangeCallback(this.getValue(selectedData));
  }

  writeValue(selectedIds: string[]) {
    this.selectedIds = selectedIds;

    if (this.isDisablePreselected) {
      this.sizes
        .filter((school: SelectSize) =>
          selectedIds.some(id => id === school.id && this.disabledSchools.some(disabledId => disabledId === id)))
        .forEach((school: SelectSize) => school.disabled = true);
    }
  }

  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  registerOnTouched() {
  }

  private getValue(selectedData: SelectSize | SelectSize[]) {
    if (!selectedData) {
      return null;
    }

    const selectedSizes = Array.isArray(selectedData)
      ? selectedData.map(size => new Size({id: size.id, name: size.name}))
      : [new Size({id: selectedData.id, name: selectedData.name})];

    return selectedSizes.length !== 0 ? selectedSizes : null;
  }

  private onChangeCallback: any = () => {};
}
