import { BetweenCriteriaFilter } from '../../../shared/models/filtering/between-criteria-filter.model';
import { LikeCriteriaFilter } from '../../../shared/models/filtering/like-criteria-filter.model';

export interface OrdersFiltersSchema {
  customerName: LikeCriteriaFilter;
  orderDate: BetweenCriteriaFilter;
}
