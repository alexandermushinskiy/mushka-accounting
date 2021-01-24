import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { I18N } from '../constants/i18n.const';
import { OrdersFacadeService } from '../services/orders-facade.service';
import { DateRange } from '../../shared/models/date-range.model';
import { TimeFrame } from '../../shared/enums/time-frame.enum';

@Component({
  selector: 'mshk-orders-actions-bar',
  templateUrl: './orders-actions-bar.component.html',
  styleUrls: ['./orders-actions-bar.component.scss']
})
export class OrdersActionsBarComponent implements OnInit {
  isFilterPanel = false;
  totalItems$: Observable<number>;
  hasActiveFilters$: Observable<boolean>;

  readonly i18n = I18N.actionsBar;

  searchKey: string;
  timeFrame: TimeFrame;
  dateRange: DateRange = { from: null, to: null };

  constructor(private ordersFacadeService: OrdersFacadeService) {
  }

  ngOnInit(): void {
    this.totalItems$ = this.ordersFacadeService.getTotalTableItems$();

    this.hasActiveFilters$ = this.ordersFacadeService.hasActiveFilters$();
  }

  showHideFilterPanel(): void {
    this.isFilterPanel = !this.isFilterPanel;
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

  clearFiltersAndHideFilterPanel(): void {
    this.searchKey = null;
    this.dateRange = { from: null, to: null };

    this.search();
    this.showHideFilterPanel();
  }

  private search(): void {
    const searchParams = {
      customerName: this.searchKey,
      orderDate: { from: this.dateRange.from, to: this.dateRange.to }
    };

    this.ordersFacadeService.searchOrders(searchParams);
  }
}
