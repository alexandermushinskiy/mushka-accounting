import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { NgbModalRef, NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';

import { OrderList } from '../../shared/models/order-list.model';
import { Router } from '@angular/router';
import { DatetimeService } from '../../core/datetime/datetime.service';
import { OrdersService } from '../../core/api/orders.service';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { OrderListFilter } from '../../shared/filters/order-list.filter';
import { DateRange } from '../../shared/models/date-range.model';
import { SelectTimeframesComponent } from '../../shared/widgets/select-timeframes/select-timeframes.component';

@Component({
  selector: 'mshk-orders-list',
  templateUrl: './orders-list.component.html',
  styleUrls: ['./orders-list.component.scss']
})
export class OrdersListComponent implements OnInit {
  @ViewChild('confirmRemoveTmpl', { static: false }) confirmRemoveTmpl: ElementRef;
  @ViewChild('timeframes', { static: false }) timeframesSelect: SelectTimeframesComponent;
  orders: OrderList[];
  shownOrders: OrderList[];
  loadingIndicator = false;
  total = 0;
  shown = 0;
  orderToDelete: OrderList;
  searchKey: string;
  dateRange: DateRange;
  modal: NgbModalRef;
  sorts = [
    { prop: 'orderDate', dir: 'desc' },
    { prop: 'customerName', dir: null },
    { prop: 'productsCount', dir: null }
  ];

  private readonly modalConfig: NgbModalOptions = {
    windowClass: 'order-modal',
    backdrop: 'static',
    size: 'sm'
  };

  constructor(private router: Router,
              private modalService: NgbModal,
              private dateTimeService: DatetimeService,
              private ordersService: OrdersService,
              private notificationsService: NotificationsService) {
  }

  ngOnInit() {
    this.loadOrders();
  }

  onActive(event: any) {
    if (event.type === 'click') {
      event.cellElement.blur();
    }
  }

  onSearch(searchKey: string) {
    this.searchKey = searchKey;

    this.filterOrders();
  }

  onRangeSelected(dateRange: DateRange) {
    this.dateRange = dateRange;
    this.filterOrders();
  }

  onClearRange() {
    this.dateRange = null;
    this.filterOrders();
  }

  private filterOrders() {
    // debugger;
    const orderFilter = new OrderListFilter(this.searchKey, this.dateRange);
    const filteredOrders = this.orders.filter(order => orderFilter.filter(order));

    this.shownOrders = filteredOrders;
    this.shown = filteredOrders.length;
  }

  addOrder() {
    this.router.navigate(['orders/new']);
  }

  delete(order: OrderList) {
    setTimeout(() => {
      this.orderToDelete = order;
      this.modal = this.modalService.open(this.confirmRemoveTmpl, this.modalConfig);
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
    if (this.modal) {
      this.modal.close();
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
    this.notificationsService.error('Ошибка', `Ошибка при удалении заказа: ${error}.`);
  }

  private loadOrders() {
    this.loadingIndicator = true;

    this.ordersService.getAll()
      .subscribe(
        (res: OrderList[]) => this.onOrdersLoaded(res),
        () => this.onLoadOrdersFailed()
      );
  }

  private onOrdersLoaded(orders: OrderList[]) {
    this.orders = orders;
    this.shownOrders = orders;

    this.total = orders.length;
    this.shown = orders.length;

    this.loadingIndicator = false;
  }

  private onLoadOrdersFailed() {
    this.loadingIndicator = false;
    this.notificationsService.error('Невозможно загрузить все заказы');
  }
}
