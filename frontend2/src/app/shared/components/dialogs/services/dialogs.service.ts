import { Injectable } from '@angular/core';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { CategoryEditorDialogComponent } from '../components/category-editor-dialog/category-editor-dialog.component';
import { CategoryEditorDialogData } from '../components/category-editor-dialog/interfaces/category-editor-dialog-data.interface';

import { ConfirmDialogComponent } from '../components/confirm-dialog/confirm-dialog.component';
import { ConfirmDialog } from '../components/confirm-dialog/interfaces/confirm-dialog.interface';
import { ProductEditorDialogData } from '../components/product-editor-dialog/interfaces/product-editor-dialog-data.interface';
import { ProductEditorDialogComponent } from '../components/product-editor-dialog/product-editor-dialog.component';

@Injectable({
  providedIn: 'root'
})
export class DialogsService {
  constructor(private modalService: NgbModal) {
  }

  openConfirmDialog(dialogData: ConfirmDialog): ConfirmDialogComponent {
    const instance = this.openDialog<ConfirmDialogComponent>(ConfirmDialogComponent);
    instance.dialogData = dialogData;
    return instance;
  }

  openCategoryEditorDialog(dialogData: CategoryEditorDialogData): CategoryEditorDialogComponent {
    const options = {
      windowClass: 'category-modal'
    } as NgbModalOptions;

    const instance = this.openDialog<CategoryEditorDialogComponent>(CategoryEditorDialogComponent, options);
    instance.dialogData = dialogData;
    return instance;
  }

  openProductEditorDialog(dialogData: ProductEditorDialogData): ProductEditorDialogComponent {
    const options = {
      windowClass: 'products-modal'
    } as NgbModalOptions;

    const instance = this.openDialog<ProductEditorDialogComponent>(ProductEditorDialogComponent, options);
    instance.dialogData = dialogData;
    return instance;
  }

  private openDialog<DialogType>(reference: any, config: NgbModalOptions = {}): DialogType {
    const options = { size: 'sm', backdrop: true, windowClass: 'mshk-dialog', ...config };
    return this.modalService.open(reference, options).componentInstance;
  }
}
