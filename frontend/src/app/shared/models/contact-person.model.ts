export class ContactPerson {
  id: string;
  name: string;
  phones: string;
  email: string;
  position: string;
  city: string;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
