import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'mshk-datatable-filter-bar',
  templateUrl: './datatable-filter-bar.component.html',
  styleUrls: ['./datatable-filter-bar.component.scss']
})
export class DatatableFilterBarComponent implements OnInit {
  @Output() onClose = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  close() {
    this.onClose.emit();
  }
}
