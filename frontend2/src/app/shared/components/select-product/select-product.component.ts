import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl } from '@angular/forms';

import { SelectProduct } from '../../models/select-product.model';
import { IconColor } from '../icon/enums/icon-color.enum';
import { IconName } from '../icon/enums/icon-name.enum';
import { I18N } from './constants/i18n.const';

@Component({
  selector: 'mshk-select-product2',
  templateUrl: './select-product.component.html',
  styleUrls: ['./select-product.component.scss']
})
export class SelectProductComponent {
  @Input() control = new FormControl();
  @Input() products: SelectProduct[];
  @Input() canClearAll = true;
  @Input() isMultiple = false;
  @Input() isLoading = false;
  @Input() errors?: { [key: string]: string };
  @Output() onSelected = new EventEmitter<SelectProduct>();
  @Output() onClear = new EventEmitter<void>();

  isOpened = false;

  readonly i18n = I18N;
  readonly bindValue = 'id';

  readonly warningIconName = IconName.Warning;
  readonly warningIconColor = IconColor.Danger;

  get bindLabel(): string {
    return this.isMultiple ? 'nameWithVendorCode' : 'name';
  }

  get hasError(): boolean {
    return this.control.invalid && (this.control.dirty || this.control.touched);
  }

  constructor() { }

  toggleIsOpen(isOpened: boolean): void {
    this.isOpened = isOpened;
  }

  onChange(product: SelectProduct): void {
    this.onSelected.emit(product);
  }

  customSearchFn(term: string, item: SelectProduct): boolean {
    term = term.toLocaleLowerCase();
    return item.name.toLocaleLowerCase().includes(term) || item.vendorCode.toLocaleLowerCase().includes(term);
  }

  clear(): void {
    this.onClear.emit();
  }
}
