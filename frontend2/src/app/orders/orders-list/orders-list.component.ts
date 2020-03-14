import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { NgbModalRef, NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';

import { OrderList } from '../../shared/models/order-list.model';
import { Router } from '@angular/router';
import { DatetimeService } from '../../core/datetime/datetime.service';
import { OrdersService } from '../../core/api/orders.service';
import { NotificationsService } from '../../core/notifications/notifications.service';

@Component({
  selector: 'mshk-orders-list',
  templateUrl: './orders-list.component.html',
  styleUrls: ['./orders-list.component.scss']
})
export class OrdersListComponent implements OnInit {
  @ViewChild('confirmRemoveTmpl', { static: false }) confirmRemoveTmpl: ElementRef;
  orders: OrderList[];
  loadingIndicator = false;
  total = 0;
  shown = 0;
  orderToDelete: OrderList;
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

  addOrder() {
    this.router.navigate(['orders/new']);
  }

  delete(order: OrderList) {
    setTimeout(() => {
      this.orderToDelete = order;
      this.modal = this.modalService.open(this.confirmRemoveTmpl, this.modalConfig);
    });
  }

  closeModal() {
    if (this.modal) {
      this.modal.close();
    }
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
    this.total = orders.length;

    this.loadingIndicator = false;
  }

  private onLoadOrdersFailed() {
    this.loadingIndicator = false;
    this.notificationsService.error('Невозможно загрузить все заказы');
  }
}
