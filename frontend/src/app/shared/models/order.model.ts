export class Order {
  id: string;
  orderDate: string;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
