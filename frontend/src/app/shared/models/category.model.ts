export class Category {
  id: string;
  name: string;
  isSizeRequired: boolean;
  isAdditional: boolean;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
