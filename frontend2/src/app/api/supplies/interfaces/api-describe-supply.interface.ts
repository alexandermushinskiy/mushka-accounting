import { Size } from '../../../shared/models/size.model';

export namespace ApiDescribeSupply {
  export interface Response {
    supply: Supply;
    products: SupplyProduct[];
  }

  export interface Supply {
    id: string;
    supplierId: string;
    supplierName: string;
    description: string;
    requestDate: string;
    receivedDate: string;
    prepayment: string;
    prepaymentMethod: string;
    cost: number;
    costMethod: number;
    deliveryCost: number;
    deliveryCostMethod: number;
    bankFee: number;
    totalCost: number;
    productsAmount: number;
    notes: string;
  }

  export interface SupplyProduct {
    quantity: number;
    unitPrice: number;
    product: {
      id: string;
      name: string;
      vendorCode: string;
      size?: Size;
    };
  }
}
