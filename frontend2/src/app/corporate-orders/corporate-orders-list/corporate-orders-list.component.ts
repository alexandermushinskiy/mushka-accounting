import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { NgbModalRef, NgbModalOptions, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { CorporateOrderList } from '../../shared/models/corporate-order-list.model';
import { DateRange } from '../../shared/models/date-range.model';
import { CorporateOrdersService } from '../../core/api/corporate-orders.service';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { CorporateOrderListFilter } from '../../shared/filters/corporate-order-list.filter';

@Component({
  selector: 'mshk-corporate-orders-list',
  templateUrl: './corporate-orders-list.component.html',
  styleUrls: ['./corporate-orders-list.component.scss']
})
export class CorporateOrdersListComponent implements OnInit {
  @ViewChild('confirmRemoveTmpl', { static: false }) confirmRemoveTmpl: ElementRef;
  orders: CorporateOrderList[];
  shownOrders: CorporateOrderList[];
  loadingIndicator = false;
  total = 0;
  shown = 0;
  orderToDelete: CorporateOrderList;
  searchKey: string;
  dateRange: DateRange;
  modal: NgbModalRef;
  sorts = [
    { prop: 'createdOn', dir: 'desc' },
    { prop: 'companyName', dir: null }
  ];

  private modalRef: NgbModalRef;
  private readonly modalConfig: NgbModalOptions = {
    windowClass: 'order-modal',
    backdrop: 'static',
    size: 'sm'
  };

  constructor(private modalService: NgbModal,
              private corporateOrdersService: CorporateOrdersService,
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

  delete(order: CorporateOrderList) {
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
    this.notificationsService.success('orders.orderDeletedSuccessfully');
    this.orderToDelete = null;
    this.loadOrders();
  }

  private onDeleteFailed(error: string) {
    this.loadingIndicator = false;
    this.orderToDelete = null;
    this.notificationsService.error(`Ошибка при удалении заказа: ${error}.`);
  }

  private filterOrders() {
    const orderFilter = new CorporateOrderListFilter(this.searchKey, this.dateRange);
    const filteredOrders = this.orders.filter(order => orderFilter.filter(order));

    this.shownOrders = filteredOrders;
    this.shown = filteredOrders.length;
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
    this.shownOrders = orders;

    this.total = orders.length;
    this.shown = orders.length;

    this.loadingIndicator = false;
  }

  private onLoadError() {
    this.loadingIndicator = false;
    this.notificationsService.error('products.unableLoadOrders');
  }

}
