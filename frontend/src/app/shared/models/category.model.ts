export class Category {
  id: string;
  name: string;
  isSizeRequired: boolean;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
