import { FiltersBase } from './filter-base';
import { ExhibitionList } from '../../exhibitions/shared/models/exhibition-list.model';

export class ExhibitionListFilter extends FiltersBase {
  constructor(private searchKey: string) {
    super();
  }

  filter(exhibition: ExhibitionList): boolean {
    return this.containsSearchKey(exhibition.name, this.searchKey);
  }
}
