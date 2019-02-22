import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { NgbModalRef, NgbModalOptions, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DatatableComponent } from '@swimlane/ngx-datatable';
import { Router } from '@angular/router';

import { ExpenseTableRow } from '../shared/models/expense-table-row.model';
import { ExpensesService } from '../../core/api/expenses.service';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { Expense } from '../../shared/models/expense.model';
import { SortableDatatableComponent } from '../../shared/hooks/sortable-datatable.component';

@Component({
  selector: 'mk-expenses-list',
  templateUrl: './expenses-list.component.html',
  styleUrls: ['./expenses-list.component.scss']
})
export class ExpensesListComponent extends SortableDatatableComponent implements OnInit {
  @ViewChild('datatable') datatable: DatatableComponent;
  @ViewChild('confirmRemoveTmpl') confirmRemoveTmpl: ElementRef;
  
  loadingIndicator = true;
  isModalLoading = false;
  expenseRows: ExpenseTableRow[];
  total = 0;
  shown = 0;
  expenseToDelete: ExpenseTableRow;

  private modalRef: NgbModalRef;
  private readonly modalConfig: NgbModalOptions = {
    windowClass: 'expense-modal',
    backdrop: 'static',
    size: 'sm'
  };
  
  constructor(private router: Router,
              private modalService: NgbModal,
              private expensesService: ExpensesService,
              private notificationsService: NotificationsService) {
    super();

    this.sorts = [
      { prop: 'createdOn', dir: 'desc' },
      { prop: 'category', dir: null },
      { prop: 'cost', dir: null },
      { prop: 'supplierName', dir: null }
    ];
  }

  ngOnInit() {
    this.loadExpenses();
  }

  onActive(event: any) {
    if (event.type === 'click') {
      event.cellElement.blur();
    }
  }

  addExpense() {
    this.router.navigate(['expenses/new']);
  }

  delete(expense: ExpenseTableRow) {
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

  private onDeleteSuccess() {
    this.notificationsService.success('Успех', `Расходы успешно удалены из системы.`);
    this.expenseToDelete = null;
    this.loadExpenses();
  }

  private onDeleteFailed(error: string) {
    this.loadingIndicator = false;
    this.expenseToDelete = null;
    this.notificationsService.danger('Ошибка', `Ошибка при удалении расходов: ${error}.`);
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
    this.expenseRows = expenses.map((el, index) => new ExpenseTableRow(el, index));
    this.total = expenses.length;
    this.loadingIndicator = false;
  }

  private onLoadError() {
    this.loadingIndicator = false;
    this.notificationsService.danger('Ошибка', 'Невозможно загрузить расходы');
  }
}
