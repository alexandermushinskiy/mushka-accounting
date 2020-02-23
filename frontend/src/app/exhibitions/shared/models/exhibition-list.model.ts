export class ExhibitionList {
  id: string;
  name: string;
  fromDate: string;
  toDate: string;
  city: string;
  participationCost: number;
  profit: number;
  productsCount: number;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
