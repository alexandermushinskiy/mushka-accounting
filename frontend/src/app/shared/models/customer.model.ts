export class Customer {
  id: string;
  firstName: string;
  lastName: string;
  phone: string;
  email: string;
  // region: string;
  // city: string;

  get name(): string {
    return `${this.firstName} ${this.lastName}`;
  }

  // get nameWithAddress(): string {
  //   return `${this.firstName} ${this.lastName} (${this.city})`;
  // }

  get nameWithPhone(): string {
    return `${this.firstName} ${this.lastName} (${this.phone})`;
  }

  constructor(data: any) {
    Object.assign(this, data);
  }
}
