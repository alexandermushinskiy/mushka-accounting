import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { NotificationsService } from '../../core/notifications/notifications.service';
import { ExpensesService } from '../../core/api/expenses.service';
import { Expense } from '../../shared/models/expense.model';
import { ExpenseCategory } from '../../shared/enums/expense-category.enum';
import { DatetimeService } from '../../core/datetime/datetime.service';
import { expenceCategory } from '../../shared/constants/expence-category.const';

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
  selectedcategory: { id: ExpenseCategory, description: string };
  categories = [
    { id: ExpenseCategory.ADVERTISING, description: expenceCategory.advertising },
    { id: ExpenseCategory.EQUIPMENT, description: expenceCategory.equipment },
    { id: ExpenseCategory.PHOTOGRAPHY, description: expenceCategory.photography },
    { id: ExpenseCategory.DESIGN, description: expenceCategory.design },
    { id: ExpenseCategory.WEBSITE, description: expenceCategory.website },
    { id: ExpenseCategory.POLYGRAPHY, description: expenceCategory.polygraphy },
    { id: ExpenseCategory.PROMO, description: expenceCategory.promo },
    { id: ExpenseCategory.OTHER, description: expenceCategory.other }
  ];

  constructor(private formBuilder: FormBuilder,
              private route: ActivatedRoute,
              private router: Router,
              private datetimeService: DatetimeService,
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
        this.buildForm(new Expense({
          createdOn: this.datetimeService.getCurrentDateInString()
        }));
      }
    });
  }

  saveExpense() {
    // const t1 = this.createExpenseModel(this.expenseForm.getRawValue());
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
      category: formRawValue.category.id,
      purpose: formRawValue.purpose,
      notes: formRawValue.notes,
      supplierName: formRawValue.supplierName
    });
  }
}
