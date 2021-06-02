export namespace ApiGetPopularCities {
  export interface Response {
    data: {
      city: string;
      quantity: number;
    }[];
  }
}
