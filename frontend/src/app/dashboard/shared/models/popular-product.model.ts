export class PopularProduct {
  name: string;
  sizeName: string;
  vendorCode: string;
  quantity: number;

  constructor(data: any) {
    Object.assign(this, data);
  }
}