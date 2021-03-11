import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from '../shared/shared.module';
import { ExpensesListComponent } from './components/expenses-list/expenses-list.component';
import { ExpenseEditorComponent } from './components/expense-editor/expense-editor.component';
import { SelectCategoryComponent } from './components/select-category/select-category.component';
import { ExpensesComponent } from './expenses.component';
import { DialogsModule } from '../shared/widgets/dialogs/dialogs.module';

@NgModule({
  imports: [
    RouterModule.forChild([]),
    SharedModule,
    ReactiveFormsModule,
    DialogsModule
  ],
  declarations: [
    ExpensesListComponent,
    ExpenseEditorComponent,
    SelectCategoryComponent,
    ExpensesComponent
  ],
  exports: []
})
export class ExpensesModule { }
