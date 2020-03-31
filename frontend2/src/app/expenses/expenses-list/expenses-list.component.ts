import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { NgbModalRef, NgbModalOptions, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { Expense } from '../../shared/models/expense.model';
import { ExpensesService } from '../../core/api/expenses.service';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { DateRange } from '../../shared/models/date-range.model';
import { ExpenseListFilter } from '../../shared/filters/expense-list.filter';
import { ExpenseCategory } from '../../shared/enums/expense-category.enum';

@Component({
  selector: 'mshk-expenses-list',
  templateUrl: './expenses-list.component.html',
  styleUrls: ['./expenses-list.component.scss']
})
export class ExpensesListComponent implements OnInit {
  @ViewChild('confirmRemoveTmpl', { static: false }) confirmRemoveTmpl: ElementRef;
  loadingIndicator = true;
  expenses: Expense[];
  shownExpenses: Expense[];
  total = 0;
  shown = 0;
  expenseToDelete: Expense;
  searchKey: string;
  category: ExpenseCategory;
  dateRange: DateRange;
  sorts = [
    { prop: 'createdOn', dir: 'desc' },
    { prop: 'category', dir: null },
    { prop: 'cost', dir: null },
    { prop: 'supplierName', dir: null }
  ];

  private modalRef: NgbModalRef;
  private readonly modalConfig: NgbModalOptions = {
    windowClass: 'expense-modal',
    backdrop: 'static',
    size: 'sm'
  };

  constructor(private modalService: NgbModal,
              private expensesService: ExpensesService,
              private notificationsService: NotificationsService) {
  }

  ngOnInit() {
    this.loadExpenses();
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
    setTimeout(() => {
      this.expenseToDelete = expense;
      this.modalRef = this.modalService.open(this.confirmRemoveTmpl, this.modalConfig);
    });
  }

  confirmDelete() {
    this.loadingIndicator = true;
    this.closeModal();

    this.expensesService.delete(this.expenseToDelete.id)
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

  private filterExpenses() {
    const expenseFilter = new ExpenseListFilter(this.searchKey, this.category, this.dateRange);
    const filteredExpenses = this.expenses.filter(expense => expenseFilter.filter(expense));

    this.shownExpenses = filteredExpenses;
    this.shown = filteredExpenses.length;
  }

  private onDeleteSuccess() {
    this.notificationsService.success('expenses.expenseDeleted');
    this.expenseToDelete = null;
    this.loadExpenses();
  }

  private onDeleteFailed(error: string) {
    this.loadingIndicator = false;
    this.expenseToDelete = null;
    this.notificationsService.error('expenses.errorDeletingExpense');
  }

  private loadExpenses() {
    this.loadingIndicator = true;

    this.expensesService.getAll()
      .subscribe(
        (res: Expense[]) => this.onLoadSuccess(res),
        () => this.onLoadError()
      );
  }

  private onLoadSuccess(expenses: Expense[]) {
    this.expenses = expenses;
    this.shownExpenses = expenses;

    this.total = expenses.length;
    this.shown = expenses.length;

    this.loadingIndicator = false;
  }

  private onLoadError() {
    this.loadingIndicator = false;
    this.notificationsService.error('expenses.errorLoadingExpenses');
  }
}
