export class OrderList {
  id: string;
  number: string;
  orderDate: string;
  cost: number;
  address: string;
  customerName: string;
  productsCount: number;
  
  constructor(data: any) {
    Object.assign(this, data);
  }
}
