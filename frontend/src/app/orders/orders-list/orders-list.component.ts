import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal, NgbModalRef, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';

import { OrderTableRow } from '../shared/models/order-table-row.model';
import { OrdersService } from '../../core/api/orders.service';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { OrderList } from '../shared/models/order-list.model';
import { OrderListFilter } from '../../shared/filters/order-list.filter';
import { SortableDatatableComponent } from '../../shared/hooks/sortable-datatable.component';
import { LocalStorage } from 'ngx-webstorage';
import { QuickFilter } from '../../shared/filters/quick-filter';
import { OrderQuickFilter } from '../../shared/filters/order-quick-filter';

@Component({
  selector: 'mk-orders',
  templateUrl: './orders-list.component.html',
  styleUrls: ['./orders-list.component.scss']
})
export class OrdersListComponent extends SortableDatatableComponent implements OnInit {
  @ViewChild('confirmRemoveTmpl') confirmRemoveTmpl: ElementRef;
  @ViewChild('dateRange') dateRangeTmpl: ElementRef;

  @LocalStorage('orders_search_key', '') ordersSearchKey: string;

  orders: OrderList[];
  orderRows: OrderTableRow[];
  loadingIndicator = false;
  total = 0;
  shown = 0;
  orderToDelete: OrderTableRow;
  orderFilters: QuickFilter[];

  private modalRef: NgbModalRef;
  private readonly modalConfig: NgbModalOptions = {
    windowClass: 'order-modal',
    backdrop: 'static',
    size: 'sm'
  };
  private readonly dateRangModalConfig: NgbModalOptions = {
    windowClass: 'date-range-modal',
    backdrop: 'static',
    size: 'sm'
  };

  constructor(private router: Router,
              private modalService: NgbModal,
              private ordersService: OrdersService,
              private orderQuickFilter: OrderQuickFilter,
              private notificationsService: NotificationsService) {
    super();

    this.sorts = [
      { prop: 'orderDate', dir: 'desc' },
      { prop: 'customerName', dir: null },
      { prop: 'productsCount', dir: null }
    ];
  }

  ngOnInit() {
    this.loadOrders();

    this.orderFilters = this.orderQuickFilter.getFilters();
  }

  onActive(event: any) {
    if (event.type === 'click') {
      event.cellElement.blur();
    }
  }

  filter(searchKey: string) {
    this.ordersSearchKey = searchKey;

    const orderFilter = new OrderListFilter(searchKey);
    const filteredSupplies = this.orders.filter(order => orderFilter.filter(order));

    this.updateDatatableRows(filteredSupplies);
  }

  quickFilter(filter: QuickFilter) {
    if (filter.filterFunc === this.orderQuickFilter.filterCustomRange) {
      this.modalRef = this.modalService.open(this.dateRangeTmpl, this.dateRangModalConfig);
    } else {
      const filteredOrders = this.orders.filter(order => filter.filterFunc(order, '2019-02-05', '2019-02-08'));
      this.updateDatatableRows(filteredOrders);
    }
  }

  applyDateRange() {
    this.closeModal();
  }

  resetQuickFilter() {
    this.updateDatatableRows(this.orders);
  }

  addOrder() {
    this.router.navigate(['orders/new']);
  }

  delete(supply: OrderTableRow) {
    setTimeout(() => {
      this.orderToDelete = supply;
      this.modalRef = this.modalService.open(this.confirmRemoveTmpl, this.modalConfig);
    });
  }

  confirmDelete() {
    this.loadingIndicator = true;
    this.closeModal();

    this.ordersService.delete(this.orderToDelete.id)
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

    this.ordersService.getAll()
      .subscribe(
        (res: OrderList[]) => this.onLoadSuccess(res),
        () => this.onLoadError()
      );
  }

  private onLoadSuccess(orders: OrderList[]) {
    this.orders = orders;
    this.total = orders.length;

    if (this.ordersSearchKey) {
      this.filter(this.ordersSearchKey);
    } else {
      this.updateDatatableRows(orders);
    }

    this.loadingIndicator = false;
  }

  private onLoadError() {
    this.loadingIndicator = false;
    this.notificationsService.danger('Ошибка', 'Невозможно загрузить все заказы');
  }

  private updateDatatableRows(orders: OrderList[]) {
    this.orderRows = orders.map((el, index) => new OrderTableRow(el, index));
    this.shown = orders.length;
  }
}
