export class CorporateOrderProduct {
  id: string;
  name: string;
  quantity: number;
  unitPrice: number;
  costPrice: number;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
