import { Component, OnInit, OnDestroy } from '@angular/core';
import { Observable } from 'rxjs';
import { mergeMap } from 'rxjs/operators';
import { untilDestroyed } from 'ngx-take-until-destroy';

import { OrderList } from '../../../shared/models/order-list.model';
import { NotificationsService } from '../../../core/notifications/notifications.service';
import { OrdersFacadeService } from '../../services/orders-facade.service';
import { DialogsService } from '../../../shared/components/dialogs/services/dialogs.service';
import { I18N } from '../../constants/i18n.const';
import { LanguageService } from '../../../core/language/language.service';

@Component({
  selector: 'mshk-orders-list',
  templateUrl: './orders-list.component.html',
  styleUrls: ['./orders-list.component.scss']
})
export class OrdersListComponent implements OnInit, OnDestroy {
  orders$: Observable<OrderList[]>;
  total$: Observable<number>;
  isLoading$: Observable<boolean>;
  pageIndex$: Observable<number>;
  pageLimit: number;
  defaultSort = [{ prop: 'orderDate', dir: 'desc' }];

  readonly i18n = I18N.table;

  constructor(private dialogsService: DialogsService,
              private ordersFacadeService: OrdersFacadeService,
              private notificationsService: NotificationsService,
              private languageService: LanguageService) {
  }

  ngOnInit(): void {
    this.pageLimit = this.ordersFacadeService.getPageLimit();
    this.orders$ = this.ordersFacadeService.getTableItems$();
    this.total$ = this.ordersFacadeService.getTotalTableItems$();
    this.pageIndex$ = this.ordersFacadeService.getPageIndex$();
    this.isLoading$ = this.ordersFacadeService.getTableLoadingFlag$();

    this.ordersFacadeService.searchOrders({
      customerName: null,
      orderDate: { from: null, to: null }
    });
  }

  ngOnDestroy(): void {
  }

  onActive(event: any): void {
    if (event.type === 'click') {
      event.cellElement.blur();
    }
  }

  onSort({ prop, dir }: any): void {
    this.ordersFacadeService.sortOrders({ key: prop, order: dir.toUpperCase() });
  }

  onPage({ limit, offset }: any): void {
    this.ordersFacadeService.paginateOrders({ offset, limit });
  }

  delete(order: OrderList) {
    const { title, message, cancelLabel, confirmLabel } = I18N.dialogs.deleteOrder;
    const dialog = this.dialogsService.openConfirmDialog({
      title,
      message: this.languageService.translate(message, { orderNumber: order.orderNumber, orderDate: order.orderDate }),
      cancelLabel,
      confirmLabel
    });

    dialog.confirm$
      .pipe(
        mergeMap(() => {
          dialog.isLoading = true;
          return this.ordersFacadeService.deleteOrder$(order.id);
        }),
        untilDestroyed(this)
      )
      .subscribe(() => {
        dialog.close();
        this.onDeleteSuccess();
      });
  }

  private onDeleteSuccess() {
    this.notificationsService.success('orders.orderDeletedSuccessfully');
    this.ordersFacadeService.fetchOrders();
  }
}
