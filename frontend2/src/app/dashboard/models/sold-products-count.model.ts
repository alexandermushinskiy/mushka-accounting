export class SoldProductsCount {
  createdOn: string;
  quantity: number;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
