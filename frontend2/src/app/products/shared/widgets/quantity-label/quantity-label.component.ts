import { Component, Input } from '@angular/core';

@Component({
  selector: 'mshk-quantity-label',
  templateUrl: './quantity-label.component.html',
  styleUrls: ['./quantity-label.component.scss']
})
export class QuantityLabelComponent {
  @Input() quantity: number;

  constructor() { }

  getCssClass() {
    if (this.quantity === 0 ) {
      return 'quantity-empty';
    }
    return this.quantity > 15
      ? 'quantity'
      : 'quantity-almost-empty';
  }
}
