import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { ProductsServce } from '../../../../core/api/products.service';
import { SelectProduct } from '../../../../shared/models/select-product.model';

@Component({
  selector: 'mk-filters-modal',
  templateUrl: './filters-modal.component.html',
  styleUrls: ['./filters-modal.component.scss']
})
export class FiltersModalComponent implements OnInit {
  @Input() title = 'Фильтрация по товарам';
  @Output() onApply = new EventEmitter<string[]>();
  @Output() onClose = new EventEmitter<void>();
  
  selectedProducts: SelectProduct[] = [];
  productsList: SelectProduct[];
  isError = false;

  constructor(private productsService: ProductsServce) { }

  ngOnInit() {
    this.productsService.getSelect()
      .subscribe((products: SelectProduct[]) => {
        this.productsList = products;
      });
  }

  apply() {
    if (this.selectedProducts.length === 0) {
      this.isError = true;
    } else {
      this.isError = false;
      this.onApply.emit(this.selectedProducts.map(prod => prod.id));
    }
  }

  close() {
    this.onClose.emit();
  }
}
