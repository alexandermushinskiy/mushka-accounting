import { SupplyProduct } from './supply-product.model';
import { PaymentMethod } from '../enums/payment-method.enum';

export class Supply {
  id: string;
  supplierId: string;
  supplierName: string;
  description: string;
  requestDate: string;
  receivedDate: string;
  cost: number;
  costMethod: PaymentMethod;
  deliveryCost: number;
  deliveryCostMethod: PaymentMethod;
  prepayment: number;
  prepaymentMethod: PaymentMethod;
  bankFee: number;
  totalCost: number;
  notes: string;
  products: SupplyProduct[];

  constructor(data: any) {
    Object.assign(this, data);
  }
}
