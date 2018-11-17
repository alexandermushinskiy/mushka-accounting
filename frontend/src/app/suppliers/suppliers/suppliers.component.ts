import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { DatatableComponent } from 'ngx-datatable-with-ie-fix';
import { Router } from '@angular/router';

import { SuppliersService } from '../../core/api/suppliers.service';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { SupplierTablePreview } from '../shared/models/supplier-table-preview';
import { Supplier } from '../../shared/models/supplier.model';

@Component({
  selector: 'psa-suppliers',
  templateUrl: './suppliers.component.html',
  styleUrls: ['./suppliers.component.scss']
})
export class SuppliersComponent implements OnInit {
  @ViewChild('datatable') datatable: DatatableComponent;

  loadingIndicator = true;
  isModalLoading = false;
  supplierRows: SupplierTablePreview[];
  total = 0;
  shown = 0;
  contactsWidth: number;
  
  constructor(private router: Router,
              private suppliersService: SuppliersService,
              private notificationsService: NotificationsService) { }

  ngOnInit() {
    this.loadSuppliers();
  }

  toggleExpandRow(row, index) {
    row.index = index;

    this.contactsWidth = this.getColumnWidth('name') + this.getColumnWidth('service') +
                         this.getColumnWidth('address') + this.getColumnWidth('email');

    this.datatable.rowDetail.toggleExpandRow(row);
  }

  addSupplier() {
    this.router.navigate(['suppliers/new']);
  }

  private loadSuppliers() {
    this.loadingIndicator = true;

    this.suppliersService.getAll()
      .subscribe(
        (res: Supplier[]) => this.onLoadSuccess(res),
        () => this.onLoadError()
      );
  }

  private onLoadSuccess(suppliers: Supplier[]) {
    this.supplierRows = suppliers.map((el, index) => new SupplierTablePreview(el, index));
    this.total = suppliers.length;
    this.loadingIndicator = false;
  }

  private onLoadError() {
    this.loadingIndicator = false;
    this.notificationsService.danger('Ошибка', 'Невозможно загрузить поставщиков');
  }

  private getColumnWidth(columnName: string): number {
    const datatableColumns = this.datatable.bodyComponent._columns;
    const column = datatableColumns.find(col => col.name === columnName);
    return !!column ? column.width : 0;
  }

}
