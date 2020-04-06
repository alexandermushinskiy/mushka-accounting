export class PaymentCard {
  id: string;
  number: string;
  owner: string;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
