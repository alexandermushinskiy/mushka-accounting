export class Customer {
  id: string;
  firstName: string;
  lastName: string;
  phone: string;
  email: string;

  get name(): string {
    return `${this.firstName} ${this.lastName}`;
  }

  get nameWithPhone(): string {
    return `${this.firstName} ${this.lastName} (${this.phone})`;
  }

  constructor(data: any) {
    Object.assign(this, data);
  }
}
