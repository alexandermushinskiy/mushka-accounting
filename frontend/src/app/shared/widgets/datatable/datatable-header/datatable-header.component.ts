import { Component, EventEmitter, Input, Output, ViewChild, OnInit } from '@angular/core';

import { SearchFormComponent } from '../../search-form/search-form.component';
import { QuickFilter } from '../../../../shared/filters/quick-filter';

@Component({
  selector: 'mk-datatable-header',
  templateUrl: './datatable-header.component.html',
  styleUrls: ['./datatable-header.component.scss']
})
export class DatatableHeaderComponent implements OnInit {
  @ViewChild(SearchFormComponent) searchForm: SearchFormComponent;
  @Input() isCollapsed: false;
  @Input() total: number;
  @Input() availableColumns: string[];
  @Input() shown: number;
  @Input() showMenuToggle = false;
  @Input() showAddButton = true;
  @Input() showTitle = true;
  @Input() showOptions = true;
  @Input() showActions = true;
  @Input() title = '';
  @Input() quickFilters: QuickFilter[];
  @Input() filterKey = '';

  @Output() onFilter = new EventEmitter<string>();
  @Output() onQuickFilter = new EventEmitter<QuickFilter>();
  @Output() onCollapseMenu = new EventEmitter<any>();
  @Output() onExportAllToCSV = new EventEmitter<string>();
  @Output() onExportFilteredToCSV = new EventEmitter<string>();
  @Output() onImport = new EventEmitter();
  @Output() onReload = new EventEmitter();
  @Output() onFilterReset = new EventEmitter();
  @Output() onQuickFilterReset = new EventEmitter();
  @Output() onAddItem = new EventEmitter();

  currentFilter = '';

  constructor() {
  }

  ngOnInit() {
  }

  addItem() {
    this.onAddItem.emit();
  }

  filter(value: string) {
    this.onFilter.emit(value);
    this.currentFilter = value;
  }

  exportAllToCSV() {
    this.onExportAllToCSV.emit(this.title);
  }

  exportFilteredToCSV() {
    this.onExportFilteredToCSV.emit(`${this.title}_filter_${this.currentFilter}`);
  }

  inport() {
    this.onImport.emit();
  }

  reloadTickets() {
    this.onReload.emit();
  }

  resetFilters() {
    this.searchForm.reset();
    this.filter('');
    this.onFilterReset.emit();
  }

  quickFilterBy(filter: QuickFilter) {
    this.onQuickFilter.emit(filter);
  }

  resetQuickFilter() {
    this.onQuickFilterReset.emit();
  }
}
