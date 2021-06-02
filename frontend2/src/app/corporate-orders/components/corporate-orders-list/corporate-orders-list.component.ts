import { Component, OnInit, ViewChild, ElementRef, OnDestroy } from '@angular/core';
import { LocalStorage } from 'ngx-webstorage';

import { CorporateOrderList } from '../../../shared/models/corporate-order-list.model';
import { DateRange } from '../../../shared/models/date-range.model';
import { NotificationsService } from '../../../core/notifications/notifications.service';
import { CorporateOrderListFilter } from '../../../shared/filters/corporate-order-list.filter';
import { ApiCorporateOrdersService } from '../../../api/corporate-orders/services/api-corporate-orders.service';
import { ItemsList } from '../../../shared/interfaces/items-list.interface';
import { I18N } from '../../constants/i18n.const';
import { DialogsService } from '../../../shared/components/dialogs/services/dialogs.service';
import { LanguageService } from '../../../core/language/language.service';
import { mergeMap } from 'rxjs/operators';
import { untilDestroyed } from 'ngx-take-until-destroy';

@Component({
  selector: 'mshk-corporate-orders-list',
  templateUrl: './corporate-orders-list.component.html',
  styleUrls: ['./corporate-orders-list.component.scss']
})
export class CorporateOrdersListComponent implements OnInit, OnDestroy {
  @ViewChild('confirmRemoveTmpl', { static: false }) confirmRemoveTmpl: ElementRef;
  @LocalStorage('corporate_orders_filter', {searchKey: null, dateRange: null}) ordersFilter: { searchKey: string, dateRange: DateRange };

  orders: CorporateOrderList[];
  shownOrders: CorporateOrderList[];
  loadingIndicator = false;
  total = 0;
  shown = 0;
  sorts = [
    { prop: 'createdOn', dir: 'desc' },
    { prop: 'companyName', dir: null }
  ];

  constructor(private dialogsService: DialogsService,
              private apiCorporateOrdersService: ApiCorporateOrdersService,
              private languageService: LanguageService,
              private notificationsService: NotificationsService) {
  }

  ngOnInit() {
    this.loadOrders();
  }

  ngOnDestroy(): void {
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
    const { title, message, cancelLabel, confirmLabel } = I18N.dialogs.deleteCorporateOrder;
    const dialog = this.dialogsService.openConfirmDialog({
      title,
      message: this.languageService.translate(message, { orderNumber: order.orderNumber, orderDate: order.createdOn }),
      cancelLabel,
      confirmLabel
    });

    dialog.confirm$
      .pipe(
        mergeMap(() => {
          dialog.isLoading = true;
          return this.apiCorporateOrdersService.deleteOrder$(order.id);
        }),
        untilDestroyed(this)
      )
      .subscribe(() => {
          dialog.close();
          this.onDeleteSuccess();
        });
  }

  private onDeleteSuccess() {
    this.notificationsService.success(I18N.messages.orderDeleted);
    this.loadOrders();
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
