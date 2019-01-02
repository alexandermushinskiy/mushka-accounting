import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModalRef, NgbModalOptions, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { SuppliesService } from '../../core/api/supplies.service';
import { Supply } from '../shared/models/supply.model';
import { SupplyTableRow } from '../shared/models/supply-table-row.model';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { SupplyFilter } from '../../shared/filters/supply.filter';

@Component({
  selector: 'mk-supplies-list',
  templateUrl: './supplies-list.component.html',
  styleUrls: ['./supplies-list.component.scss']
})
export class SuppliesListComponent implements OnInit {
  @ViewChild('confirmRemoveTmpl') confirmRemoveTmpl: ElementRef;
  supplies: Supply[];
  supplyRows: SupplyTableRow[];
  loadingIndicator = false;
  total = 0;
  shown = 0;
  
  private supplyToDelete: SupplyTableRow;
  private modalRef: NgbModalRef;
  private readonly modalConfig: NgbModalOptions = {
    windowClass: 'supply-modal',
    backdrop: 'static',
    size: 'sm'
  };

  constructor(private router: Router,
              private modalService: NgbModal,
              private suppliesService: SuppliesService,
              private notificationsService: NotificationsService) { }

  ngOnInit() {
    this.loadSupplies();
  }

  onActive(event: any) {
    if (event.type === 'click') {
      event.cellElement.blur();
    }
  }

  getRowClass(row: any) {
    return row.className;
  }

  addSupply() {
    this.router.navigate(['supplies/new']);
  }

  filter(searchKey: string) {
    const supplyFilter = new SupplyFilter(searchKey);
    const filteredSupplies = this.supplies.filter(supply => supplyFilter.filter(supply));

    this.updateDatatableRows(filteredSupplies);
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
        (res: Supply[]) => this.onLoadSuccess(res),
        () => this.onLoadError()
      );
  }

  private onLoadSuccess(supplies: Supply[]) {
    this.supplies = supplies;
    this.total = supplies.length;
    this.updateDatatableRows(supplies);

    this.loadingIndicator = false;
  }

  private onLoadError() {
    this.loadingIndicator = false;
    this.notificationsService.danger('Ошибка', 'Невозможно загрузить все поставки');
  }
  
  private updateDatatableRows(supplies: Supply[]) {
    this.supplyRows = supplies.map((el, index) => new SupplyTableRow(el, index));
    this.shown = supplies.length;
  }
}
