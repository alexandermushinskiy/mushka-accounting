import { OnInit, TemplateRef, Output, EventEmitter } from '@angular/core';

import { DatatableColumn } from '../interfaces/datatable-column.interface';
import { ColumnConfiguration } from '../models/column-configuration.model';
import { FileHelper } from '../utils/file-helper';
import { DatatableComponent } from '@swimlane/ngx-datatable';

export abstract class MkDatatableComponent implements OnInit {
  @Output() onRowsUpdated = new EventEmitter<number>();

  datatable: DatatableComponent;
  headerTpl: TemplateRef<any>;
  sorts: { dir: string, prop: string }[];
  columnsData: DatatableColumn[];
  headerHeight: number;
  hasData = false;
  columnsConfigurationSnapshot: ColumnConfiguration[];
  columnsDictionary: {};
  selectedRowId: string;
  filterText: string;
  datatableConfig: { [name: string]: DatatableColumn };
  propertiesToFilter: { [name: string]: string };

  rowsData: any[];
  initialRowsData: any[];

  readonly fakeRowClassName = 'fake-row';
  readonly defaultColumnWidth = 100;
  readonly fixedHeaderHeight = {
    collapsed: 40,
    expanded: 40
  };

  constructor(datatableConfig, propertiesToFilter) {
    this.datatableConfig = datatableConfig;
    this.propertiesToFilter = propertiesToFilter;
  }

  abstract getFakeRow();

  ngOnInit() {
    this.headerHeight = this.fixedHeaderHeight.collapsed;
  }

  init(datatable: DatatableComponent, columns: string[], headerTpl: TemplateRef<any>) {
    this.datatable = datatable;
    this.headerTpl = headerTpl;
    const configurations = this.getColumnsConfigurations();

    this.columnsConfigurationSnapshot = [...configurations];
    this.columnsDictionary = this.createColumnsDictionary(this.columnsConfigurationSnapshot);
    this.columnsData = this.createAvailableColumnsData(columns, this.columnsConfigurationSnapshot);
  }

  updateColumns(columns: string[]) {
    const configurations = this.getColumnsConfigurations();
    this.columnsConfigurationSnapshot = [...configurations];

    this.columnsData = this.createAvailableColumnsData(columns, this.columnsConfigurationSnapshot);
  }

  initRows(data: any) {
    if (data && data.length > 0) {
      this.initialRowsData = data;
      this.hasData = true;
      this.sort();
      this.filter();
    } else {
      this.hasData = false;
      this.rowsData = [];
    }

    this.datatable.bodyComponent.updateOffsetY(0);
  }

  createColumnsDictionary(configurations: ColumnConfiguration[]): {} {
    return this.columnsDictionary = configurations.reduce((dictionary, cellConfig) => {
      if (this.datatableConfig[cellConfig.name]) {
        dictionary[this.datatableConfig[cellConfig.name].prop] = cellConfig.name;
      }
      return dictionary;
    }, {});
  }

  createAvailableColumnsData(cols: string[], columnsConfiguration: ColumnConfiguration[] = []): any[] {
    const colsToRender = [];
    columnsConfiguration.forEach((column) => {
      const currentColumn = { ...this.datatableConfig[column.name] };
      if (cols.includes(column.name) && currentColumn && column.visible) {
        if (typeof column.width === 'number') {
          currentColumn.width = column.width;
        }
        currentColumn.cellTemplate = this[currentColumn.cellTemplateName];
        currentColumn.headerTemplate = this.headerTpl;
        colsToRender.push(currentColumn);
      }
    });
    return colsToRender;
  }

  getRowClass(row: any) {
    return row.className;
  }

  getExportedProps() {
    return this.columnsData
      .map((column) => column.exportProp || column.prop);
  }

  getExportedColumnTitles() {
    return this.columnsData
      .reduce((columnTitles, column) => {
        return { ...columnTitles, [column.exportProp || column.prop]: column.name };
      }, {});
  }

  getColumnsConfigurations(): ColumnConfiguration[] {
    const availableColumns = Object.keys(this.datatableConfig);

    return availableColumns.map((columnName: string) => {
      return {
        name: columnName,
        width: this.datatableConfig[columnName] ? this.datatableConfig[columnName].width : this.defaultColumnWidth,
        visible: true,
        sort: {},
        filters: []
      };
    });
  }

  onExportToCSV(fileSuffix: string, rowsData: any[]) {
    FileHelper.toCSVFormat(
      `MUSHKA-${fileSuffix}`,
      [this.getExportedColumnTitles()].concat(rowsData),
      this.getExportedProps()
    );
  }

  onTableSort({ sorts }, rows?: any[]) {
    const { dir, prop, initSort } = sorts[0];
    const rowsData = rows || this.rowsData;
    if (rowsData && rowsData.length > 0) {
      return this.sortTable({ dir, prop }, rowsData);
    }
  }

  sort() {
    const sort = this.sortColumn();
    this.rowsData = this.onTableSort({ sorts: [{ ...{ initSort: true }, ...sort }] }, this.initialRowsData);
  }

  sortByProp(a, b) {
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

  onFilter(filterText: string) {
    this.filterText = filterText;
    this.filter();
  }

  filter() {
    let filteredRows = this.initialRowsData ? [...this.initialRowsData] : [];
    if (this.filterText) {
      filteredRows = this.filterByGlobalText(filteredRows);
    }

    this.rowsData = this.updateColumnsStatus(filteredRows);
    this.broadcastRowsUpdated(filteredRows.length);
    this.recalculateTable();
  }

  resetFilter() {
    this.filter();
  }

  filterByGlobalText(filteredRows) {
    const columns = this.columnsData.map(el => el.name);
    const propertiesToFilterKeys = Object.keys(this.propertiesToFilter);
    this.filterText = this.filterText.toLowerCase().trim();

    return filteredRows.filter(row => {
      const filterFields = [];
      columns.forEach(column => {
        if (propertiesToFilterKeys.includes(column) && row[this.propertiesToFilter[column]]) {
          filterFields.push(row[this.propertiesToFilter[column]].toString().toLowerCase());
        }
      });
      return filterFields.some(el => el.includes(this.filterText));
    });
  }

  updateColumnsStatus(rows: any[] = []) {
    const updatedColumns = rows.map((el: any, index) => {
      return Object.assign(el, {
        className: (rows.length === 1 && el.className === this.fakeRowClassName)
          ? el.className
          : el.getClassName(index, !!el.id && el.id === this.selectedRowId)
      });
    });
    if (updatedColumns.length === 0) {
      return [this.getFakeRow()];
    }
    return updatedColumns;
  }

  sortTable({ dir, prop }, rowsData: any): any[] {
    switch (dir) {
      case 'asc':
        return this.updateColumnsStatus(rowsData.sort((a, b) => this.sortByProp(a[prop], b[prop])));
      case 'desc':
      default:
        return this.updateColumnsStatus(rowsData.sort((a, b) => this.sortByProp(b[prop], a[prop])));
    }
  }

  sortColumn(): { dir: string, prop: string } {
    const sortColumn = this.columnsConfigurationSnapshot.find(c => Object.keys(c.sort).length > 0);
    const sort = sortColumn
      ? { dir: sortColumn.sort.order, prop: sortColumn.name }
      : { dir: 'asc', prop: 'name' };

    this.sorts = [sort];
    return sort;
  }

  private recalculateTable() {
    setTimeout(() => {
      this.datatable.recalculate();
    }, 0);
  }

  private broadcastRowsUpdated(length: number) {
    setTimeout(() => {
      this.onRowsUpdated.emit(length);
    }, 0);
  }
}