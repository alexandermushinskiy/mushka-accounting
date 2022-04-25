import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { I18N } from '../../constants/i18n.const';
import { OrdersFacadeService } from '../../services/orders-facade.service';
import { DateRange } from '../../../shared/models/date-range.model';
import { TimeFrame } from '../../../shared/components/time-frame/enums/time-frame.enum';

@Component({
  selector: 'mshk-orders-actions-bar',
  templateUrl: './orders-actions-bar.component.html',
  styleUrls: ['./orders-actions-bar.component.scss']
})
export class OrdersActionsBarComponent implements OnInit {
  isFiltersShown = false;
  totalItems$: Observable<number>;
  hasActiveFilters$: Observable<boolean>;
  isLoading$: Observable<boolean>;
  searchKey: string;
  timeFrame: TimeFrame;
  dateRange: DateRange = { from: null, to: null };

  readonly i18n = I18N.actionsBar;

  constructor(private ordersFacadeService: OrdersFacadeService) {
  }

  ngOnInit(): void {
    this.totalItems$ = this.ordersFacadeService.getTotalTableItems$();
    this.isLoading$ = this.ordersFacadeService.getTableLoadingFlag$();
    this.hasActiveFilters$ = this.ordersFacadeService.hasActiveFilters$();
  }

  toggleFilters(): void {
    this.isFiltersShown = !this.isFiltersShown;
  }

  onSearchChanged(searchKey: string) {
    this.searchKey = searchKey;
    this.search();
  }

  onRangeChanged({ timeFrame, dateRange }: { timeFrame: TimeFrame, dateRange: DateRange }) {
    this.timeFrame = timeFrame;
    this.dateRange = dateRange;
    this.search();
  }

  resetFilters(): void {
    this.searchKey = null;
    this.dateRange = { from: null, to: null };

    this.search();
    this.toggleFilters();
  }

  private search(): void {
    const searchParams = {
      searchKey: this.searchKey,
      fromDate: this.dateRange.from,
      toDate: this.dateRange.to
    };

    this.ordersFacadeService.searchOrders(searchParams);
  }
}
