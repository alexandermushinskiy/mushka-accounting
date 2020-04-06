import { FiltersBase } from './filter-base';
import { ExhibitionList } from '../models/exhibition-list.model';
import { DateRange } from '../models/date-range.model';

export class ExhibitionListFilter extends FiltersBase {
  constructor(private searchKey: string,
              private dateRange: DateRange) {
    super();
  }

  filter(exhibition: ExhibitionList): boolean {
    let isInDateRange = true;
    let foundBySearch = true;

    if (!!this.dateRange) {
      isInDateRange = this.isBetween(exhibition.fromDate, this.dateRange) || this.isBetween(exhibition.toDate, this.dateRange);
    }

    if (!!this.searchKey) {
      foundBySearch = [exhibition.name, exhibition.city]
        .some(field => field.toLowerCase().includes(this.searchKey.toLowerCase()));
    }

    return isInDateRange && foundBySearch;
  }
}
