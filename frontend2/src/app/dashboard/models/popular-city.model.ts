export class PopularCity {
  city: string;
  quantity: number;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
