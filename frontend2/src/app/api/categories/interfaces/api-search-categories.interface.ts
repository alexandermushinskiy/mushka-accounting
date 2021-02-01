export namespace ApiSearchCategories {
  export interface Response {
    total: number;
    items: Category[];
  }

  export interface Category {
    id: string;
    name: string;
    isSizeRequired: boolean;
    isAdditional: boolean;
  }
}
