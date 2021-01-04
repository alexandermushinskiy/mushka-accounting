import { TableSortOrder } from '../../../../shared/enums/table-sort-order.enum';

export namespace ApiSearchOrders {
  export interface Request {
    query: Query;
    navigation: Navigation;
  }

  export interface Response {
    total: number;
    items?: Order[];
  }

  export interface Query {
    criteria?: string;
    fromDate?: string;
    toDate?: string;
  }

  export interface Order {
    id: string;
    orderNumber: string;
    orderDate: Date;
    cost: number;
    address: string;
    customerName: string;
    productsCount: number;
    isWholesale: boolean;
  }

  export interface Navigation {
    sort: Sort;
    page: Page;
  }

  export interface Sort {
    key: string;
    order: TableSortOrder;
  }

  export interface Page {
    from: string;
    size: string;
  }
}
