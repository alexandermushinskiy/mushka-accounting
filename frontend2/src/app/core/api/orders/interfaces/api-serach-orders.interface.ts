import { TableSortOrder } from '../../../../shared/enums/table-sort-order.enum';
import { BetweenCriteria } from '../../../../shared/interfaces/between-criteria.interface';
import { LikeCriteria } from '../../../../shared/interfaces/like-criteria.interface';

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
    customer?: {
      name: LikeCriteria
    };
    order?: {
      orderDate?: BetweenCriteria;
    };
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
