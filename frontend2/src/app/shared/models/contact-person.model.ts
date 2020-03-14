export class ContactPerson {
  id: string;
  name: string;
  phones: string;
  email: string;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
