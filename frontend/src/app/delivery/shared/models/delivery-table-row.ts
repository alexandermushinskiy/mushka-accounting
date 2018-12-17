import { DataTablePreview } from '../../../shared/models/data-table-preview.model';
import { PaymentMethod } from '../enums/payment-method.enum';

export class DeliveryTableRow extends DataTablePreview {
  supplier: string;
  requestDate: string;
  deliveryDate: string;
  paymentMethod: PaymentMethod;
  cost: number;
  transferFee: number;
  bankFee: number;
  totalCost: number;
  productsAmount: number;

  constructor(elem, index: number = 0) {
    super(elem, index);

    this.supplier = elem.supplier;
    this.requestDate = elem.requestDate;
    this.deliveryDate = elem.deliveryDate;
    this.paymentMethod = elem.paymentMethod;
    this.transferFee = elem.transferFee;
    this.bankFee = elem.bankFee;
    this.cost = elem.cost;
    this.totalCost = elem.totalCost;
    this.productsAmount = elem.productsAmount;
  }
}
