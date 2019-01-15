import { DataTableRow } from '../models/data-table-row.model';
import { UnsubscriberComponent } from './unsubscriber.component';

export abstract class SortableDatatableComponent extends UnsubscriberComponent {
  defaultSortDirection = 'asc';
  sorts: { dir: string, prop: string }[] = [{ prop: 'name', dir: this.defaultSortDirection }];

  onTableSort(dataRows: any, { sorts }, rows?: any[]) {
    const { dir, prop } = sorts[0];
    const rowsData = rows || dataRows;

    this.setPropertySortDirection(prop, dir);

    if (rowsData && rowsData.length > 0) {
      switch (dir) {
        case 'asc':
          return this.updateColumnsStatus(rowsData.sort((a, b) => this.sortByProp(a[prop], b[prop])));
        case 'desc':
        default:
          return this.updateColumnsStatus(rowsData.sort((a, b) => this.sortByProp(b[prop], a[prop])));
      }
    }
  }

  getPropertySortDirection(property: string) {
    return this.sorts.find(el => el.prop === property).dir;
  }

  private updateColumnsStatus(rows: DataTableRow[] = []) {
    const updatedColumns = rows.map((el: DataTableRow, index) => {
      return Object.assign(el, {
        className: (rows.length === 1)
          ? el.className
          : el.getClassName(index)
      });
    });

    return updatedColumns;
  }

  private sortByProp(a, b) {
    const aProp = a ? a.toString().toLowerCase().trim() : '';
    const bProp = b ? b.toString().toLowerCase().trim() : '';

    if (aProp < bProp) {
      return -1;
    }
    if (aProp > bProp) {
      return 1;
    }

    return 0;
  }

  private setPropertySortDirection(property: string, direction: 'asc' | 'desc') {
    this.sorts.find(el => el.prop === property).dir = direction;
    this.sorts.filter(el => el.prop !== property).forEach(el => el.dir = null);
  }
}
