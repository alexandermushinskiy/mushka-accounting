import { Component, OnInit, Input, Output, EventEmitter, TemplateRef } from '@angular/core';

import { DateRange } from '../../models/date-range.model';

@Component({
  selector: 'mshk-datatable-bar',
  templateUrl: './datatable-bar.component.html',
  styleUrls: ['./datatable-bar.component.scss']
})
export class DatatableBarComponent implements OnInit {
  @Input() title: string;
  @Input() addLink: string;
  @Input() addButtonTitle: string;
  @Input() total: number;
  @Input() shown: number;
  @Input() hasMenuToggle = false;
  @Input() searchKey: string;
  @Output() onSearch = new EventEmitter<string>();
  @Output() onRangeSelected = new EventEmitter<DateRange>();
  @Output() onClearRange = new EventEmitter();
  @Output() onAddClicked = new EventEmitter();
  isFilterPanel = false;

  constructor() { }

  ngOnInit() {
  }

  search(searchKey: string) {
    this.onSearch.emit(searchKey);
  }

  rangeSelected(dateRange: DateRange) {
    this.onRangeSelected.emit(dateRange);
  }

  clearRange() {
    this.onClearRange.emit();
  }

  showHideFilterPanel() {
    this.isFilterPanel = !this.isFilterPanel;
  }
}
