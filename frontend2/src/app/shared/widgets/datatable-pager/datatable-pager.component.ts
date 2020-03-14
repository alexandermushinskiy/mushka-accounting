import { Component, ChangeDetectionStrategy, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'mshk-datatable-pager',
  templateUrl: './datatable-pager.component.html',
  styleUrls: ['./datatable-pager.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DatatablePagerComponent {
  @Input() pageSize: number;
  @Input() currentPage: number;
  @Input() totalItems: number;
  @Output() onChanged = new EventEmitter<number>();

  constructor() { }

  pageChanged() {
    this.onChanged.emit(this.currentPage);
  }
}
