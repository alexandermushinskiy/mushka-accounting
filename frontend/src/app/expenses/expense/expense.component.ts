import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { NotificationsService } from '../../core/notifications/notifications.service';
import { ExpensesService } from '../../core/api/expenses.service';
import { Expense } from '../../shared/models/expense.model';

@Component({
  selector: 'mk-expense',
  templateUrl: './expense.component.html',
  styleUrls: ['./expense.component.scss']
})
export class ExpenseComponent implements OnInit {
  expenseForm: FormGroup;
  title: string;
  expenseId: string;
  isEdit: boolean;
  isLoading = false;
  isFormSubmitted = false;

  constructor(private formBuilder: FormBuilder,
              private route: ActivatedRoute,
              private router: Router,
              private expensesService: ExpensesService,
              private notificationsService: NotificationsService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.expenseId = params['id'];
      this.isEdit = !!this.expenseId;
      this.title = `${this.isEdit ? 'Редактирование' : 'Добавление'} расходов`;

      if (this.isEdit) {
        this.expensesService.getById(this.expenseId)
          .subscribe((supplier: Expense) => this.buildForm(supplier));
      } else {
        this.buildForm(new Expense({}));
      }
    });
  }

  saveExpense() {
    // const t1 = this.createOrderModel(this.orderForm.getRawValue());
    // console.info(t1);
    // return;
    this.isFormSubmitted = true;
    if (this.expenseForm.invalid) {
      return;
    }

    this.isLoading = true;
    const expense = this.createExpenseModel(this.expenseForm.getRawValue());

    (this.isEdit
      ? this.expensesService.update(this.expenseId, expense)
      : this.expensesService.create(expense))
      .subscribe(
        () => this.onSaveSuccess(),
        (errors: string[]) => this.onSaveFailed(errors)
      );
  }

  private onSaveSuccess() {
    this.isLoading = false;
    this.notificationsService.success(this.title, `Расходы были успешно ${this.isEdit ? 'изменены' : 'добавлены'}`);

    this.router.navigate(['/expenses']);
  }

  private onSaveFailed(errors: string[]) {
    this.isLoading = false;
    this.notificationsService.danger(this.title, errors[0]);
  }

  private buildForm(supplier: Expense) {
    this.expenseForm = this.formBuilder.group({
    });
  }

  private createExpenseModel(formRawValue: any): Expense {
    return new Expense({
      id: this.expenseId,
    });
  }
}
