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
      this.sizes = source.map((school: Size) => new SelectSize({ id: school.id, name: school.name }));
    }
  }
  @Input() disabledSchools: string[];
  @Input() isMultiple = true;
  @Input() canClearAll = true;
  @Input() isDisablePreselected = false;
  @Output() onSelectedSizes = new EventEmitter<string[]>();

  selectedIds: string[];
  sizes: SelectSize[] = [];
  isLoading = true;

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
    const selectedSchools = Array.isArray(selectedData)
      ? selectedData.map(school => school.id)
      : [selectedData.id];

    return selectedSchools.length !== 0 ? selectedSchools : null;
  }

  private onChangeCallback: any = () => {};
}
