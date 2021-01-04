import { TablePagination } from '../../../shared/interfaces/table-pagination.interface';
import { TableSort } from '../../../shared/interfaces/table-sort.interface';
import { OrdersFiltersSchema } from './orders-filters-schema.interface';

export interface OrdersSearchParams {
  filters: OrdersFiltersSchema;
  sort?: TableSort;
  pagination: TablePagination;
}
