import { DataTablePreview } from '../../../shared/models/data-table-preview.model';
import { PaymentMethod } from '../enums/payment-method.enum';

export class SupplyTableRow extends DataTablePreview {
  supplier: string;
  requestDate: string;
  receivedDate: string;
  paymentMethod: PaymentMethod;
  cost: number;
  deliveryCost: number;
  bankFee: number;
  totalCost: number;
  productsAmount: number;

  constructor(elem, index: number = 0) {
    super(elem, index);

    this.supplier = elem.supplier;
    this.requestDate = elem.requestDate;
    this.receivedDate = elem.receivedDate;
    this.paymentMethod = elem.paymentMethod;
    this.deliveryCost = elem.deliveryCost;
    this.bankFee = elem.bankFee;
    this.cost = elem.cost;
    this.totalCost = elem.totalCost;
    this.productsAmount = elem.productsAmount;
  }
}
