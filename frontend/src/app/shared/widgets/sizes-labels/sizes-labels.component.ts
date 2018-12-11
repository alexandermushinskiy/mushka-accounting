import { Component, OnInit, Input } from '@angular/core';

import { ProductSize } from '../../models/product-size.model';

@Component({
  selector: 'mk-sizes-labels',
  templateUrl: './sizes-labels.component.html',
  styleUrls: ['./sizes-labels.component.scss']
})
export class SizesLabelsComponent implements OnInit {
  @Input() set sizes(source: ProductSize[]) {
    this.productSizes = !source ? [] : source;
  }

  productSizes: ProductSize[];
  noSizes = ' - ';

  constructor() { }

  ngOnInit() {
  }

}
