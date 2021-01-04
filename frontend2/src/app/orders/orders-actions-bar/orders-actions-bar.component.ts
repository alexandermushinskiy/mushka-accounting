import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { I18N } from '../constants/i18n.const';
import { OrdersFacadeService } from '../services/orders-facade.service';
import { DateRange } from '../../shared/models/date-range.model';
import { OrdersFiltersSchema } from '../shared/interfaces/orders-filters-schema.interface';

@Component({
  selector: 'mshk-orders-actions-bar',
  templateUrl: './orders-actions-bar.component.html',
  styleUrls: ['./orders-actions-bar.component.scss']
})
export class OrdersActionsBarComponent implements OnInit {
  isFilterPanel = false;
  totalItems$: Observable<number>;

  readonly i18n = I18N.actionsBar;

  private searchKey: string;
  private dateRange: DateRange;

  constructor(private ordersFacadeService: OrdersFacadeService) {
  }

  ngOnInit(): void {
    this.totalItems$ = this.ordersFacadeService.getTotalTableItems$();
  }

  showHideFilterPanel(): void {
    this.isFilterPanel = !this.isFilterPanel;
  }

  onSearchChanged(searchKey: string) {
    this.searchKey = searchKey;
    this.search();
  }

  onRangeChanged(dateRange: DateRange) {
    this.dateRange = dateRange;
    this.search();
  }

  private search(): void {
    const searchParams = {
      criteria: this.searchKey,
      fromDate: this.dateRange && this.dateRange.from,
      toDate: this.dateRange && this.dateRange.to
    } as OrdersFiltersSchema;

    this.ordersFacadeService.searchOrders(searchParams);
  }
}
