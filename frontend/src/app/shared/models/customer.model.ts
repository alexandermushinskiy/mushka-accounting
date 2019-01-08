export class Customer {
  id: string;
  firstName: string;
  lastName: string;
  phone: string;
  email: string;
  region: string;
  city: string;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
