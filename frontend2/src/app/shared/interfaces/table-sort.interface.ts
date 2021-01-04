import { TableSortOrder } from '../enums/table-sort-order.enum';

export interface TableSort {
  key: string;
  order: TableSortOrder;
}
