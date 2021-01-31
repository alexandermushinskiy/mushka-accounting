import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { NgbModalRef, NgbModalOptions, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LocalStorage } from 'ngx-webstorage';

import { CorporateOrderList } from '../../shared/models/corporate-order-list.model';
import { DateRange } from '../../shared/models/date-range.model';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { CorporateOrderListFilter } from '../../shared/filters/corporate-order-list.filter';
import { ApiCorporateOrdersService } from '../../core/api/corporate-orders/services/api-corporate-orders.service';
import { ItemsList } from '../../shared/interfaces/items-list.interface';

@Component({
  selector: 'mshk-corporate-orders-list',
  templateUrl: './corporate-orders-list.component.html',
  styleUrls: ['./corporate-orders-list.component.scss']
})
export class CorporateOrdersListComponent implements OnInit {
  @ViewChild('confirmRemoveTmpl', { static: false }) confirmRemoveTmpl: ElementRef;
  @LocalStorage('corporate_orders_filter', {searchKey: null, dateRange: null}) ordersFilter: { searchKey: string, dateRange: DateRange };

  orders: CorporateOrderList[];
  shownOrders: CorporateOrderList[];
  loadingIndicator = false;
  total = 0;
  shown = 0;
  orderToDelete: CorporateOrderList;
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
              private apiCorporateOrdersService: ApiCorporateOrdersService,
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
    this.ordersFilter.searchKey = searchKey;
    this.filterOrders();
  }

  onRangeSelected(dateRange: DateRange) {
    this.ordersFilter.dateRange = dateRange;
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

    this.apiCorporateOrdersService.deleteOrder$(this.orderToDelete.id)
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
    const orderFilter = new CorporateOrderListFilter(this.ordersFilter.searchKey, this.ordersFilter.dateRange);
    const filteredOrders = this.orders.filter(order => orderFilter.filter(order));

    this.shownOrders = filteredOrders;
    this.shown = filteredOrders.length;
  }

  private loadOrders() {
    this.loadingIndicator = true;

    this.apiCorporateOrdersService.searchOrders$()
      .subscribe(
        (result: ItemsList<CorporateOrderList>) => this.onLoadSuccess(result),
        () => this.onLoadError()
      );
  }

  private onLoadSuccess(result: ItemsList<CorporateOrderList>) {
    this.orders = result.items;
    this.total = result.length;

    if (!!this.ordersFilter.searchKey || !!this.ordersFilter.dateRange) {
      this.filterOrders();
    } else {
      this.shownOrders = result.items;
      this.shown = result.length;
    }

    this.loadingIndicator = false;
  }

  private onLoadError() {
    this.loadingIndicator = false;
    this.notificationsService.error('products.unableLoadOrders');
  }

}
