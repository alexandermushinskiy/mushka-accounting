import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { NgbModal, NgbModalRef, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
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
  private modalRef: NgbModalRef;
  private readonly modalConfig: NgbModalOptions = {
    windowClass: 'supplier-modal',
    backdrop: 'static'
  };

  constructor(private modalService: NgbModal,
              private router: Router,
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

  addSupplier(content: ElementRef) {
    this.router.navigate(['suppliers/new']);
    //this.modalRef = this.modalService.open(content, this.modalConfig);
  }

  saveSupplier(supplier: Supplier) {
    // this.suppliersService.addSupplier(supplier)
    //   .subscribe(
    //     (res: Supplier) => this.onSaveSuccess(res, supplier.id ? 'изменен' : 'добавлен'),
    //     () => this.onSaveError()
    //   );
  }

  closeModal() {
    this.modalRef.close();
  }

  private loadSuppliers() {
    this.loadingIndicator = true;

    this.suppliersService.getAll()
      .subscribe(
        res => this.onLoadSuccess(res),
        () => this.onLoadError()
      );
  }

  private onLoadSuccess(suppliers) {
    this.supplierRows = suppliers.map((el, index) => new SupplierTablePreview(el, index));
    this.total = suppliers.length;
    this.loadingIndicator = false;
  }

  private onLoadError() {
    this.loadingIndicator = false;
    this.notificationsService.danger('Ошибка', 'Невозможно загрузить поставщиков');
  }

  private onSaveSuccess(supplier: Supplier, action: string) {
    this.isModalLoading = false;
    this.closeModal();
    this.notificationsService.success('Успех', `Поставщик \"${supplier.name}\" успешно ${action}`);
  }

  private onSaveError() {
    this.notificationsService.danger('Ошибка', 'Невозможно соранить данные поставщика');
    this.isModalLoading = false;
  }

  private getColumnWidth(columnName: string): number {
    const datatableColumns = this.datatable.bodyComponent._columns;
    const column = datatableColumns.find(col => col.name === columnName);
    return !!column ? column.width : 0;
  }

  // private getColumnsWidth(columnNames: string[]): number {
  //   columnNames.reduce((x, y) => );
  // }
}
