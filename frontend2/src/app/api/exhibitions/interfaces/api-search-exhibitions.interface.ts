export namespace ApiSearchExhibitions {
  export interface Response {
    total: number;
    items: Exhibition[];
  }

  export interface Exhibition {
    id: string;
    name: string;
    fromDate: string;
    toDate: string;
    city: string;
    participationCost: number;
    profit: number;
    productsCount: number;
  }
}
