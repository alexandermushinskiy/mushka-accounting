export class SelectProduct {
  id: string;
  name: string;
  vendorCode: string;
  quantity: number;
  categoryName: string;
  sizeName: string;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
