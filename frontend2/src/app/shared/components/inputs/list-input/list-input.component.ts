import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl } from '@angular/forms';

import { SelectOption } from '../../../interfaces/select-option.interface';

@Component({
  selector: 'mshk-list-input',
  templateUrl: './list-input.component.html',
  styleUrls: ['./list-input.component.scss']
})
export class ListInputComponent<TValue = string> {
  @Input() label?: string;
  @Input() options: SelectOption<TValue>[];
  @Input() control = new FormControl();
  @Input() placeholder = 'common.selectValue';
  @Input() notFoundText = 'common.noItemsFound';
  @Input() isLoading?: boolean;
  @Input() errors?: { [key: string]: string };

  @Input() clearable = false;
  @Input() searchable = false;
  @Output() onChange = new EventEmitter<SelectOption>();

  isOpened = false;

  readonly dataField = 'value';
  readonly textField = 'label';

  get hasError(): boolean {
    return this.control.invalid && (this.control.dirty || this.control.touched);
  }

  toggleIsOpen(isOpened: boolean): void {
    this.isOpened = isOpened;
  }
}
