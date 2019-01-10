export class SelectProduct {
  id: string;
  name: string;
  vendorCode: string;
  categoryName: string;
  sizeName: string;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
