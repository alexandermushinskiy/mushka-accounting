import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

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
  @Output() onAddClicked = new EventEmitter();
  isFilterPanel = false;

  constructor() { }

  ngOnInit() {
  }

  search(searchKey: string) {
    this.onSearch.emit(searchKey);
  }

  showHideFilterPanel() {
    this.isFilterPanel = !this.isFilterPanel;
  }
}
