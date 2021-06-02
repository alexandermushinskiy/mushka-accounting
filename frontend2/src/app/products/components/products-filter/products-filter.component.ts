import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { ProductsQuickFilter } from '../../../shared/enums/products-quick-filter.enum';

@Component({
  selector: 'mshk-products-filter',
  templateUrl: './products-filter.component.html',
  styleUrls: ['./products-filter.component.scss']
})
export class ProductsFilterComponent implements OnInit {
  @Input() disabled = false;
  @Input() placeholder = 'common.filter';
  @Output() onSelected = new EventEmitter<ProductsQuickFilter>();
  @Output() onClear = new EventEmitter();
  selectedFilter: ProductsQuickFilter;

  filters = [
    { id: ProductsQuickFilter.WITHOUT_DELIVERIES, name: 'products.withoutDeliveries' },
    { id: ProductsQuickFilter.ALMOST_FINISHED, name: 'products.almostFinished' },
    { id: ProductsQuickFilter.OUT_OF_STOCK, name: 'products.outOfStock' }
  ];

  constructor() { }

  ngOnInit() {
  }

  onFilterChanged(selectedFilter: ProductsQuickFilter) {
    this.selectedFilter = selectedFilter;
    this.onSelected.emit(selectedFilter);
  }
}
