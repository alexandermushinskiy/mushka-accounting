import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { NgbModalRef, NgbModalOptions, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { LocalStorage } from 'ngx-webstorage';

import { CorporateOrdersService } from '../../core/api/corporate-orders.service';
import { CorporateOrderTableRow } from '../shared/models/corporate-order-table-row.model';
import { CorporateOrderList } from '../shared/models/corporate-order-list.model';
import { SortableDatatableComponent } from '../../shared/hooks/sortable-datatable.component';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { CorporateOrderListFilter } from '../../shared/filters/corporate-order-list.filter';

@Component({
  selector: 'mk-corporate-orders-list',
  templateUrl: './corporate-orders-list.component.html',
  styleUrls: ['./corporate-orders-list.component.scss']
})
export class CorporateOrdersListComponent extends SortableDatatableComponent implements OnInit {
  @ViewChild('confirmRemoveTmpl', { static: false }) confirmRemoveTmpl: ElementRef;
  @LocalStorage('corporate_orders_search_key', '') ordersSearchKey: string;

  orders: CorporateOrderList[];
  orderRows: CorporateOrderTableRow[];
  loadingIndicator = false;
  total = 0;
  shown = 0;
  orderToDelete: CorporateOrderTableRow;

  private modalRef: NgbModalRef;
  private readonly modalConfig: NgbModalOptions = {
    windowClass: 'order-modal',
    backdrop: 'static',
    size: 'sm'
  };

  constructor(private router: Router,
              private modalService: NgbModal,
              private corporateOrdersService: CorporateOrdersService,
              private notificationsService: NotificationsService) {
    super();

    this.sorts = [
      { prop: 'createdOn', dir: 'desc' },
      { prop: 'companyName', dir: null }
    ];
  }

  ngOnInit() {
    this.loadOrders();
  }

  onActive(event: any) {
    if (event.type === 'click') {
      event.cellElement.blur();
    }
  }

  filter(searchKey: string) {
    this.ordersSearchKey = searchKey;

    const orderFilter = new CorporateOrderListFilter(searchKey);
    const filteredOrders = this.orders.filter(order => orderFilter.filter(order));

    this.updateDatatableRows(filteredOrders);
  }

  resetFilters() {
    this.updateDatatableRows(this.orders);
  }

  addOrder() {
    this.router.navigate(['corporate-orders/new']);
  }

  delete(order: CorporateOrderTableRow) {
    setTimeout(() => {
      this.orderToDelete = order;
      this.modalRef = this.modalService.open(this.confirmRemoveTmpl, this.modalConfig);
    });
  }

  confirmDelete() {
    this.loadingIndicator = true;
    this.closeModal();

    this.corporateOrdersService.delete(this.orderToDelete.id)
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
    this.notificationsService.success('Успех', `Заказ успешно удален из системы.`);
    this.orderToDelete = null;
    this.loadOrders();
  }

  private onDeleteFailed(error: string) {
    this.loadingIndicator = false;
    this.orderToDelete = null;
    this.notificationsService.danger('Ошибка', `Ошибка при удалении заказа: ${error}.`);
  }

  private loadOrders() {
    this.loadingIndicator = true;

    this.corporateOrdersService.getAll()
      .subscribe(
        (res: CorporateOrderList[]) => this.onLoadSuccess(res),
        () => this.onLoadError()
      );
  }

  private onLoadSuccess(orders: CorporateOrderList[]) {
    this.orders = orders;
    this.total = orders.length;

    this.updateDatatableRows(orders);
    this.loadingIndicator = false;
  }

  private onLoadError() {
    this.loadingIndicator = false;
    this.notificationsService.danger('Ошибка', 'Невозможно загрузить все заказы');
  }

  private updateDatatableRows(orders: CorporateOrderList[]) {
    this.orderRows = orders.map((el, index) => new CorporateOrderTableRow(el, index));
    this.shown = orders.length;
  }

}
