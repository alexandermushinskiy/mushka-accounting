import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';

import { SharedModule } from '../../shared.module';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';
import { CategoryEditorDialogComponent } from './components/category-editor-dialog/category-editor-dialog.component';
import { ProductEditorDialogComponent } from './components/product-editor-dialog/product-editor-dialog.component';
import { SelectCategoryModule } from '../select-category/select-category.module';
import { SelectSizeModule } from '../select-size/select-size.module';
import { ModalModule } from 'ngx-bootstrap';
import { DateRangeDialogComponent } from './components/date-range-dialog/date-range-dialog.component';

@NgModule({
  imports: [
    CommonModule,
    TranslateModule,
    ReactiveFormsModule,
    NgSelectModule,
    SelectCategoryModule,
    SelectSizeModule,
    SharedModule
  ],
  declarations: [
    ConfirmDialogComponent,
    CategoryEditorDialogComponent,
    ProductEditorDialogComponent,
    DateRangeDialogComponent
  ],
  exports: [
    ConfirmDialogComponent,
    CategoryEditorDialogComponent,
    ProductEditorDialogComponent,
    DateRangeDialogComponent
  ],
  entryComponents: [
    ConfirmDialogComponent,
    CategoryEditorDialogComponent,
    ProductEditorDialogComponent,
    DateRangeDialogComponent
  ]
})
export class DialogsModule {
}
