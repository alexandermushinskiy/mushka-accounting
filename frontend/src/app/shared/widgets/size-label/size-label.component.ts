import { Component, OnInit, Input } from '@angular/core';

import { Size } from '../../models/size.model';
import { Product } from '../../models/product.model';
import { ProductTableRow } from '../../../products/shared/models/product-table-row.model';

@Component({
  selector: 'mk-size-label',
  templateUrl: './size-label.component.html',
  styleUrls: ['./size-label.component.scss']
})
export class SizeLabelComponent implements OnInit {
  @Input() set product(source: ProductTableRow) {
    this.productSize = source.size;
    this.quantity = source.quantity;
  }

  productSize: Size;
  quantity: number;
  noSizes = ' - ';

  constructor() { }

  ngOnInit() {
  }

  getCssClass() {
    if (this.quantity === 0 ) {
      return 'size-label-sold';
    }
    return this.quantity > 10
      ? 'size-label'
      : 'size-label-ends';
  }
}
