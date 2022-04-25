import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Observable } from 'rxjs';

import { OrderList } from '../../../shared/models/order-list.model';
import { I18N } from '../../constants/i18n.const';
import { OrdersFacadeService } from '../../services/orders-facade.service';

@Component({
  selector: 'mshk-orders-table',
  templateUrl: './orders-table.component.html',
  styleUrls: ['./orders-table.component.scss']
})
export class OrdersTableComponent implements OnInit {
  @Input() orders: OrderList[];
  @Input() total: number;
  @Input() isLoading: boolean;
  @Input() pageIndex: number;
  @Input() pageLimit: number;
  @Output() onSort = new EventEmitter<{ key, order }>();
  @Output() onChangePage = new EventEmitter<{ limit, offset }>();
  @Output() onDelete = new EventEmitter<OrderList>();

  defaultSort = [{ prop: 'orderDate', dir: 'desc' }];

  readonly i18n = I18N.table;

  constructor() { }

  ngOnInit(): void {
  }

  onActive(event: any): void {
    if (event.type === 'click') {
      event.cellElement.blur();
    }
  }

  sort({ prop, dir }: any): void {
    this.onSort.emit({ key: prop, order: dir.toUpperCase() });
  }

  changePage({ limit, offset }: any): void {
    this.onChangePage.emit({ offset, limit });
  }

  delete(order: OrderList): void {
    this.onDelete.emit(order);
  }
}
