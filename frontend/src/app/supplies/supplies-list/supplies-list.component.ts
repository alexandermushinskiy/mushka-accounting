import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModalRef, NgbModalOptions, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import * as FileSaver from 'file-saver';

import { SuppliesService } from '../../core/api/supplies.service';
import { SupplyTableRow } from '../shared/models/supply-table-row.model';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { SupplyFilter } from '../../shared/filters/supply.filter';
import { SortableDatatableComponent } from '../../shared/hooks/sortable-datatable.component';
import { QuickFilter } from '../../shared/filters/quick-filter';
import { SupplyQuickFilter } from '../../shared/filters/supply-quick.filter';
import { SupplyList } from '../shared/models/supply-list.model';
import { DatetimeService } from '../../core/datetime/datetime.service';

@Component({
  selector: 'mk-supplies-list',
  templateUrl: './supplies-list.component.html',
  styleUrls: ['./supplies-list.component.scss']
})
export class SuppliesListComponent extends SortableDatatableComponent implements OnInit {
  @ViewChild('confirmRemoveTmpl') confirmRemoveTmpl: ElementRef;
  @ViewChild('filters') filtersTmpl: ElementRef;
  supplies: SupplyList[];
  supplyRows: SupplyTableRow[];
  loadingIndicator = false;
  total = 0;
  shown = 0;
  supplyToDelete: SupplyTableRow;
  supplyFilters: QuickFilter[];
  filteredProducts: string[] = [];

  private modalRef: NgbModalRef;
  private readonly modalConfig: NgbModalOptions = {
    windowClass: 'supply-modal',
    backdrop: 'static',
    size: 'sm'
  };
  private readonly filtersModalConfig: NgbModalOptions = {
    windowClass: 'supply-filters-modal',
    backdrop: 'static',
    size: 'sm'
  };

  constructor(private router: Router,
              private modalService: NgbModal,
              private suppliesService: SuppliesService,
              private supplyQuickFilter: SupplyQuickFilter,
              private dateTimeService: DatetimeService,
              private notificationsService: NotificationsService) {
    super();

    this.sorts = [
      { prop: 'supplierName', dir: this.defaultSortDirection },
      { prop: 'receivedDate', dir: null },
      { prop: 'description', dir: null }
    ];
  }

  ngOnInit() {
    this.loadSupplies();

    this.supplyFilters = this.supplyQuickFilter.getFilters();
  }

  onActive(event: any) {
    if (event.type === 'click') {
      event.cellElement.blur();
    }
  }

  addSupply() {
    this.router.navigate(['supplies/new']);
  }

  filter(searchKey: string) {
    const supplyFilter = new SupplyFilter(searchKey);
    const filteredSupplies = this.supplies.filter(supply => supplyFilter.filter(supply));

    this.updateDatatableRows(filteredSupplies);
  }

  quickFilter(filter: QuickFilter) {
    this.modalRef = this.modalService.open(this.filtersTmpl, this.filtersModalConfig);
  }

  applyQuickFilter(selectedProducts: string[]) {
    this.filteredProducts = selectedProducts;
    this.loadingIndicator = true;
    this.closeModal();

    this.suppliesService.getFiltered(selectedProducts)
      .subscribe(
        (filteredSupplies: SupplyList[]) => {
          this.updateDatatableRows(filteredSupplies);
          this.loadingIndicator = false;
        },
        () => this.onLoadError());
  }

  resetFilters() {
    this.filteredProducts = [];
    this.updateDatatableRows(this.supplies);
  }

  delete(supply: SupplyTableRow) {
    setTimeout(() => {
      this.supplyToDelete = supply;
      this.modalRef = this.modalService.open(this.confirmRemoveTmpl, this.modalConfig);
    });
  }

  confirmDelete() {
    this.loadingIndicator = true;
    this.closeModal();

    this.suppliesService.delete(this.supplyToDelete.id)
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

  onExportAllToCSV(fileSuffix: string) {
    this.loadingIndicator = true;
    this.suppliesService.export(this.supplies.map(sup => sup.id))
      .subscribe(
        (file: Blob) => this.onExportSuccess(file),
        (error: string) => this.onExportFailed(error)
      );
  }

  onExportFilteredToCSV(fileSuffix: string) {
    this.loadingIndicator = true;
    this.suppliesService.export(this.supplyRows.map(sup => sup.id), this.filteredProducts)
      .subscribe(
        (file: Blob) => this.onExportSuccess(file),
        (error: string) => this.onExportFailed(error)
      );
  }

  private onDeleteSuccess() {
    this.notificationsService.success('Успех', `Поставка успешно удален из системы.`);
    this.supplyToDelete = null;
    this.loadSupplies();
  }

  private onDeleteFailed(error: string) {
    this.loadingIndicator = false;
    this.supplyToDelete = null;
    this.notificationsService.danger('Ошибка', `Ошибка при удалении поставки: ${error}.`);
  }

  private loadSupplies() {
    this.loadingIndicator = true;

    this.suppliesService.getAll()
      .subscribe(
        (res: SupplyList[]) => this.onLoadSuccess(res),
        () => this.onLoadError()
      );
  }

  private onLoadSuccess(supplies: SupplyList[]) {
    this.supplies = supplies;
    this.total = supplies.length;
    this.updateDatatableRows(supplies);

    this.loadingIndicator = false;
  }

  private onLoadError() {
    this.loadingIndicator = false;
    this.notificationsService.danger('Ошибка', 'Невозможно загрузить все поставки');
  }

  private updateDatatableRows(supplies: SupplyList[]) {
    this.supplyRows = supplies.map((el, index) => new SupplyTableRow(el, index));
    this.shown = supplies.length;
  }

  private onExportSuccess(file: Blob) {
    FileSaver.saveAs(file, this.generateFileName(), file.type);
    this.loadingIndicator = false;
  }

  private onExportFailed(error: string) {
    // this.errors = [ error ];
    this.loadingIndicator = false;
  }

  private generateFileName(): string {
    const postfix = this.dateTimeService.toString(new Date(), 'YYYY-MM-DD-HH-mm');
    return `mushka_export_orders-${postfix}.xlsx`;
  }
}
