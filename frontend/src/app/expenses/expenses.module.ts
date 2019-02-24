import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from '../shared/shared.module';
import { ExpensesListComponent } from './expenses-list/expenses-list.component';
import { ExpenseComponent } from './expense/expense.component';

@NgModule({
  imports: [
    SharedModule,
    ReactiveFormsModule
  ],
  declarations: [
    ExpensesListComponent,
    ExpenseComponent
  ],
  exports: []
})
export class ExpensesModule { }
