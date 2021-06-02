import { Component, OnInit, OnDestroy } from '@angular/core';
import { mergeMap } from 'rxjs/operators';
import { untilDestroyed } from 'ngx-take-until-destroy';

import { Expense } from '../../models/expense.model';
import { NotificationsService } from '../../../core/notifications/notifications.service';
import { DateRange } from '../../../shared/models/date-range.model';
import { ExpenseListFilter } from '../../../shared/filters/expense-list.filter';
import { ExpenseCategory } from '../../enums/expense-category.enum';
import { ApiExpensesService } from '../../../api/expenses/services/api-expenses.service';
import { ItemsList } from '../../../shared/interfaces/items-list.interface';
import { DialogsService } from '../../../shared/components/dialogs/services/dialogs.service';
import { LanguageService } from '../../../core/language/language.service';
import { DatetimeService } from '../../../core/datetime/datetime.service';
import { I18N } from '../../constants/i18n.const';

@Component({
  selector: 'mshk-expenses-list',
  templateUrl: './expenses-list.component.html',
  styleUrls: ['./expenses-list.component.scss']
})
export class ExpensesListComponent implements OnInit, OnDestroy {
  loadingIndicator = true;
  expenses: Expense[];
  shownExpenses: Expense[];
  total = 0;
  shown = 0;
  searchKey: string;
  category: ExpenseCategory;
  dateRange: DateRange;
  sorts = [
    { prop: 'createdOn', dir: 'desc' },
    { prop: 'category', dir: null },
    { prop: 'cost', dir: null },
    { prop: 'supplierName', dir: null }
  ];

  constructor(private apiExpensesService: ApiExpensesService,
              private dialogService: DialogsService,
              private notificationsService: NotificationsService,
              private datetimeService: DatetimeService,
              private languageService: LanguageService) {
  }

  ngOnInit() {
    this.loadExpenses();
  }

  ngOnDestroy(): void {
  }

  onActive(event: any) {
    if (event.type === 'click') {
      event.cellElement.blur();
    }
  }

  onSearch(searchKey: string) {
    this.searchKey = searchKey;
    this.filterExpenses();
  }

  onCategorySelected(category: ExpenseCategory) {
    this.category = category;
    this.filterExpenses();
  }

  onRangeSelected(dateRange: DateRange) {
    this.dateRange = dateRange;
    this.filterExpenses();
  }

  onClearRange() {
    this.dateRange = null;
    this.filterExpenses();
  }

  delete(expense: Expense) {
    const { title, message, cancelLabel, confirmLabel } = I18N.dialogs.deleteExpense;
    const dialog = this.dialogService.openConfirmDialog({
      title,
      message: this.languageService.translate(message, {
        supplier: expense.supplierName,
        date: this.datetimeService.convertToFormat(expense.createdOn)
      }),
      cancelLabel,
      confirmLabel
    });

    dialog.confirm$
      .pipe(
        mergeMap(() => {
          dialog.isLoading = true;
          return this.apiExpensesService.deleteExpense$(expense.id);
        }),
        untilDestroyed(this)
      )
      .subscribe(
        () => {
          dialog.close();
          this.onDeleteSuccess();
        },
        () => this.onDeleteFailed()
      );
  }

  private filterExpenses() {
    const expenseFilter = new ExpenseListFilter(this.searchKey, this.category, this.dateRange);
    const filteredExpenses = this.expenses.filter(expense => expenseFilter.filter(expense));

    this.shownExpenses = filteredExpenses;
    this.shown = filteredExpenses.length;
  }

  private onDeleteSuccess() {
    this.notificationsService.success('expenses.expenseDeleted');
    this.loadExpenses();
  }

  private onDeleteFailed() {
    this.loadingIndicator = false;
    this.notificationsService.error('expenses.errorDeletingExpense');
  }

  private loadExpenses() {
    this.loadingIndicator = true;

    this.apiExpensesService.searchExpenses$()
      .subscribe(
        (res: ItemsList<Expense>) => this.onLoadSuccess(res),
        () => this.onLoadError()
      );
  }

  private onLoadSuccess(expenses: ItemsList<Expense>) {
    this.expenses = expenses.items;
    this.shownExpenses = expenses.items;

    this.total = expenses.length;
    this.shown = expenses.length;

    this.loadingIndicator = false;
  }

  private onLoadError() {
    this.loadingIndicator = false;
    this.notificationsService.error('expenses.errorLoadingExpenses');
  }
}
