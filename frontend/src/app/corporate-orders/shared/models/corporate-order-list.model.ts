export class CorporateOrderList {
  id: string;
  number: string;
  createdOn: string;
  address: string;
  companyName: string;
  
  constructor(data: any) {
    Object.assign(this, data);
  }
}
