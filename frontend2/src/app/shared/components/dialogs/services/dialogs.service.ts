import { Injectable } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap';
import { CategoryEditorDialogComponent } from '../components/category-editor-dialog/category-editor-dialog.component';
import { CategoryEditorDialogData } from '../components/category-editor-dialog/interfaces/category-editor-dialog-data.interface';

import { ConfirmDialogComponent } from '../components/confirm-dialog/confirm-dialog.component';
import { ConfirmDialog } from '../components/confirm-dialog/interfaces/confirm-dialog.interface';
import { DateRangeDialogComponent } from '../components/date-range-dialog/date-range-dialog.component';
import { ProductEditorDialogData } from '../components/product-editor-dialog/interfaces/product-editor-dialog-data.interface';
import { ProductEditorDialogComponent } from '../components/product-editor-dialog/product-editor-dialog.component';

@Injectable({
  providedIn: 'root'
})
export class DialogsService {
  constructor(private modalService: BsModalService) {
  }

  openConfirmDialog(dialogData: ConfirmDialog): ConfirmDialogComponent {
    const options = { backdrop: true, class: 'mshk-dialog', initialState: { dialogData } };
    const dialogRef = this.modalService.show(ConfirmDialogComponent, options);
    return dialogRef.content;
  }

  openCategoryEditorDialog(dialogData: CategoryEditorDialogData): CategoryEditorDialogComponent {
    const options = { backdrop: true, class: 'category-modal', initialState: { dialogData } };
    const dialogRef = this.modalService.show(CategoryEditorDialogComponent, options);
    return dialogRef.content;
  }

  openProductEditorDialog(dialogData: ProductEditorDialogData): ProductEditorDialogComponent {
    const options = { backdrop: true, class: 'products-modal', initialState: { dialogData } };
    const dialogRef = this.modalService.show(ProductEditorDialogComponent, options);
    return dialogRef.content;
  }

  openDateRangeDialog(): DateRangeDialogComponent {
    const options = { backdrop: true, class: 'date-range-modal' };
    const dialogRef = this.modalService.show(DateRangeDialogComponent, options);
    return dialogRef.content;
  }
}
