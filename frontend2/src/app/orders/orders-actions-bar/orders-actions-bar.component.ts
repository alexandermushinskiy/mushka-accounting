import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { I18N } from '../constants/i18n.const';
import { OrdersFacadeService } from '../services/orders-facade.service';
import { DateRange } from '../../shared/models/date-range.model';

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
  dateRange: DateRange = { from: null, to: null };
  options = [
    'AAAAAA', 'Bbbb', 'DDDDD DDDD', '01 янв. 2021 - 21 янв. 2021'
  ];
  optionValue: string;

  constructor(private ordersFacadeService: OrdersFacadeService) {
  }

  ngOnInit(): void {
    this.totalItems$ = this.ordersFacadeService.getTotalTableItems$();

    this.hasActiveFilters$ = this.ordersFacadeService.hasActiveFilters$();
  }

  onOptionSelect(option: string): void {
    this.optionValue = option;
  }

  clearOption(): void {
    this.optionValue = null;
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
      customerName: this.searchKey,
      orderDate: { from: this.dateRange.from, to: this.dateRange.to }
    };

    this.ordersFacadeService.searchOrders(searchParams);
  }
}
