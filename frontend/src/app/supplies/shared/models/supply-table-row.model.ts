import { DataTablePreview } from '../../../shared/models/data-table-preview.model';
import { PaymentMethod } from '../../../shared/enums/payment-method.enum';

export class SupplyTableRow extends DataTablePreview {
  supplierName: string;
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

    this.supplierName = elem.supplierName;
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
