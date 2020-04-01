import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
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
  @Input() hasDateRangeFilter = false;
  @Output() onSearch = new EventEmitter<string>();
  @Output() onRangeSelected = new EventEmitter<DateRange>();
  @Output() onAddClicked = new EventEmitter();
  isFilterPanel = false;

  constructor() { }

  ngOnInit() {
  }

  onSearchChanged(searchKey: string) {
    this.onSearch.emit(searchKey);
  }

  onRangeChanged(dateRange: DateRange) {
    this.onRangeSelected.emit(dateRange);
  }

  showHideFilterPanel() {
    this.isFilterPanel = !this.isFilterPanel;
  }
}
