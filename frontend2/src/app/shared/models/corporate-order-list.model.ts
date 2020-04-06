export class CorporateOrderList {
  id: string;
  orderNumber: string;
  createdOn: string;
  address: string;
  companyName: string;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
