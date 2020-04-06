import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { NgbModalRef, NgbModalOptions, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DatatableComponent } from '@swimlane/ngx-datatable';
import { LocalStorage } from 'ngx-webstorage';

import { Supplier } from '../../shared/models/supplier.model';
import { SuppliersService } from '../../core/api/suppliers.service';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { SupplierListFilter } from '../../shared/filters/supplier-list.filter';

@Component({
  selector: 'mshk-suppliers-list',
  templateUrl: './suppliers-list.component.html',
  styleUrls: ['./suppliers-list.component.scss']
})
export class SuppliersListComponent implements OnInit {
  @ViewChild('datatable', { static: false }) datatable: DatatableComponent;
  @ViewChild('confirmRemoveTmpl', { static: false }) confirmRemoveTmpl: ElementRef;
  @LocalStorage('suppliers_filter', {searchKey: null}) suppliersFilter: { searchKey: string };

  suppliers: Supplier[];
  shownSuppliers: Supplier[];
  total = 0;
  shown = 0;
  loadingIndicator = true;
  sorts = [{ prop: 'name', dir: 'asc' }];
  supplierToDelete: Supplier;

  private modalRef: NgbModalRef;
  private readonly modalConfig: NgbModalOptions = {
    windowClass: 'supplier-modal',
    backdrop: 'static',
    size: 'sm'
  };

  constructor(private modalService: NgbModal,
              private suppliersService: SuppliersService,
              private notificationsService: NotificationsService) {
  }

  ngOnInit() {
    this.loadSuppliers();
  }

  onActive(event: any) {
    if (event.type === 'click') {
      event.cellElement.blur();
    }
  }

  showContacts(row: Supplier) {
    this.datatable.rowDetail.toggleExpandRow(row);
  }

  delete(supplier: Supplier) {
    setTimeout(() => {
      this.supplierToDelete = supplier;
      this.modalRef = this.modalService.open(this.confirmRemoveTmpl, this.modalConfig);
    });
  }

  confirmDelete() {
    this.loadingIndicator = true;
    this.closeModal();

    this.suppliersService.delete(this.supplierToDelete.id)
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

  onSearch(searchKey: string) {
    this.suppliersFilter.searchKey = searchKey;

    this.filterSuppliers();
  }

  private filterSuppliers() {
    const supplierFilter = new SupplierListFilter(this.suppliersFilter.searchKey);
    const filteredSuppliers = this.suppliers.filter(order => supplierFilter.filter(order));

    this.shownSuppliers = filteredSuppliers;
    this.shown = filteredSuppliers.length;
  }

  private onDeleteSuccess() {
    this.notificationsService.success('suppliers.supplierDeleted');
    this.supplierToDelete = null;
    this.loadSuppliers();
  }

  private onDeleteFailed(error: string) {
    this.loadingIndicator = false;
    this.supplierToDelete = null;
    this.notificationsService.error(`Ошибка при удалении поставщика: ${error}.`);
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
    this.suppliers = suppliers;
    this.total = suppliers.length;

    if (!!this.suppliersFilter.searchKey) {
      this.filterSuppliers();
    } else {
      this.shownSuppliers = suppliers;
      this.shown = suppliers.length;
    }

    this.loadingIndicator = false;
  }

  private onLoadError() {
    this.loadingIndicator = false;
    this.notificationsService.error('suppliers.errorLoadSuppliers');
  }
}
