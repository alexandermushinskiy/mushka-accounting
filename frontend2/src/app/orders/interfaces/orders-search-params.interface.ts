import { TablePagination } from '../../shared/interfaces/table-pagination.interface';
import { TableSort } from '../../shared/interfaces/table-sort.interface';
import { OrderFilters } from './order-filters.interface';

export interface OrdersSearchParams {
  filters: OrderFilters;
  sort?: TableSort;
  pagination: TablePagination;
}
