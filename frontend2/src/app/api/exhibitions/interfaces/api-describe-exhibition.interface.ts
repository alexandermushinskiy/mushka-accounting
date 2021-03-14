import { PaymentMethod } from '../../../shared/enums/payment-method.enum';
import { Product } from '../../../shared/models/product.model';

export namespace ApiDescribeExhibition {
  export interface Response {
    exhibition: Exhibition;
    products: ExhibitionProduct[];
  }

  export interface Exhibition {
    id: string;
    name: string;
    fromDate: string;
    toDate: string;
    city: string;
    participationCost: number;
    participationCostMethod: PaymentMethod;
    accommodationCost: number;
    accommodationCostMethod: PaymentMethod;
    fareCost: number;
    fareCostMethod: PaymentMethod;
    notes: string;
    profit: number;
  }

  export interface ExhibitionProduct {
    id: string;
    name: string;
    vendorCode: string;
    sizeName: string;
    quantity: number;
    unitPrice: number;
    costPrice: number;
  }
}
