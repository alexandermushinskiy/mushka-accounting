import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { of } from 'rxjs';

import { DatetimeService } from '../../../core/datetime/datetime.service';
import { NotificationsService } from '../../../core/notifications/notifications.service';
import { Expense } from '../../models/expense.model';
import { ApiExpensesService } from '../../../api/expenses/services/api-expenses.service';

@Component({
  selector: 'mshk-expense-editor',
  templateUrl: './expense-editor.component.html',
  styleUrls: ['./expense-editor.component.scss']
})
export class ExpenseEditorComponent implements OnInit {
  expenseForm: FormGroup;
  expenseId: string;
  isEdit: boolean;
  loadingIndicator = false;
  isLoading = false;

  constructor(private formBuilder: FormBuilder,
              private route: ActivatedRoute,
              private router: Router,
              private datetimeService: DatetimeService,
              private apiExpensesService: ApiExpensesService,
              private notificationsService: NotificationsService) {
}

  ngOnInit() {
    this.readRouteParams();
  }

  readRouteParams() {
    this.route.params.subscribe(params => {
      this.expenseId = params['id'];
      this.isEdit = !!this.expenseId;

      const getExpence = this.isEdit
        ? this.apiExpensesService.describeExpense$(this.expenseId)
        : of(new Expense({ createdOn: this.datetimeService.getCurrentDateInString() }));

      getExpence
        .subscribe((expense: Expense) => this.buildForm(expense));
    });
  }

  saveExpense() {
    // const t1 = this.createExpenseModel(this.expenseForm.getRawValue());
    // console.info(t1);
    // return;

    if (this.expenseForm.invalid) {
      return;
    }

    this.isLoading = true;
    const expense = this.createExpenseModel(this.expenseForm.getRawValue());

    (this.isEdit
      ? this.apiExpensesService.updateExpense$(this.expenseId, expense)
      : this.apiExpensesService.createExpense$(expense))
      .subscribe(
        () => this.onSaveSuccess(),
        (errors: string[]) => this.onSaveFailed(errors)
      );
  }

  private onSaveSuccess() {
    this.isLoading = false;
    this.notificationsService.success(this.isEdit ? 'expenses.expenseUpdated' : 'expenses.expenseAdded');

    this.router.navigate(['/expenses']);
  }

  private onSaveFailed(errors: string[]) {
    this.isLoading = false;
    this.notificationsService.error('expenses.errorSavingExpense');
  }

  private buildForm(expense: Expense) {
    this.expenseForm = this.formBuilder.group({
      createdOn: [expense.createdOn, Validators.required],
      cost: [expense.cost, Validators.required],
      costMethod: [expense.costMethod, Validators.required],
      category: [expense.category, Validators.required],
      purpose: [expense.purpose, Validators.required],
      supplierName: [expense.supplierName, Validators.required],
      notes: [expense.notes]
    });
  }

  private createExpenseModel(formRawValue: any): Expense {
    return new Expense({
      id: this.expenseId,
      createdOn: formRawValue.createdOn,
      cost: formRawValue.cost,
      costMethod: formRawValue.costMethod,
      category: formRawValue.category,
      purpose: formRawValue.purpose,
      notes: formRawValue.notes,
      supplierName: formRawValue.supplierName
    });
  }
}
