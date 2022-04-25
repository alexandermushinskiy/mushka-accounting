import { TableSortOrder } from '../../../shared/enums/table-sort-order.enum';

export namespace ApiSearchOrders {
  export interface Request {
    query: {
      searchKey?: string;
      region?: string;
      fromDate?: string;
      toDate?: string;
    };
    sort: Sort;
    page: Page;
  }

  export interface Response {
    total: number;
    items?: Order[];
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

  export interface Sort {
    key: string;
    order: TableSortOrder;
  }

  export interface Page {
    from: string;
    size: string;
  }
}
