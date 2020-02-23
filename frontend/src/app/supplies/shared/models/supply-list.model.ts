export class SupplyList {
  id: string;
  supplierName: string;
  description: string;
  receivedDate: string;
  cost: number;
  totalCost: number;
  productsAmount: number;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
