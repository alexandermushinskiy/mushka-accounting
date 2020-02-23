import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'mk-quantity-label',
  templateUrl: './quantity-label.component.html',
  styleUrls: ['./quantity-label.component.scss']
})
export class QuantityLabelComponent implements OnInit {
  @Input() quantity: number;

  constructor() { }

  ngOnInit() {
  }

  getCssClass() {
    if (this.quantity === 0 ) {
      return 'quantity-empty';
    }
    return this.quantity > 15
      ? 'quantity'
      : 'quantity-almost-empty';
  }
}
