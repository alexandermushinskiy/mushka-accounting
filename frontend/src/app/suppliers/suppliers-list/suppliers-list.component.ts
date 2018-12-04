import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { DatatableComponent } from 'ngx-datatable-with-ie-fix';
import { Router } from '@angular/router';
import { NgbModal, NgbModalRef, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';

import { SuppliersService } from '../../core/api/suppliers.service';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { SupplierTablePreview } from '../shared/models/supplier-table-preview';
import { Supplier } from '../../shared/models/supplier.model';

@Component({
  selector: 'psa-suppliers-list',
  templateUrl: './suppliers-list.component.html',
  styleUrls: ['./suppliers-list.component.scss']
})
export class SuppliersListComponent implements OnInit {
  @ViewChild('datatable') datatable: DatatableComponent;
  @ViewChild('confirmRemoveTmpl') confirmRemoveTmpl: ElementRef;

  loadingIndicator = true;
  isModalLoading = false;
  supplierRows: SupplierTablePreview[];
  total = 0;
  shown = 0;
  contactsWidth: number;

  private supplierToDelete: string;
  private modalRef: NgbModalRef;
  private readonly modalConfig: NgbModalOptions = {
    windowClass: 'supplier-modal',
    backdrop: 'static',
    size: 'sm'
  };

  constructor(private router: Router,
              private modalService: NgbModal,
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

  delete(supplierId: string) {
    this.supplierToDelete = supplierId;
    this.modalRef = this.modalService.open(this.confirmRemoveTmpl, this.modalConfig);
  }

  confirmDelete() {
    this.loadingIndicator = true;
    this.closeModal();

    this.suppliersService.delete(this.supplierToDelete)
      .subscribe(
        () => this.onDeleteSuccess(),
        (error: string) => this.onDeleteFailed(error)
      );
  }

  closeModal() {
    if (this.modalRef) {
      this.modalRef.close();
    }
  }

  private onDeleteSuccess() {
    this.notificationsService.success('Успех', `Поставщик успешно удален из системы.`);
    this.supplierToDelete = null;
    this.loadSuppliers();
  }

  private onDeleteFailed(error: string) {
    this.loadingIndicator = false;
    this.supplierToDelete = null;
    this.notificationsService.danger('Ошибка', `Ошибка при удалении поставщика: ${error}.`);
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
