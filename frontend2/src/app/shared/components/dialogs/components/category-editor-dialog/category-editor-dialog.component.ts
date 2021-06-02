import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Category } from '../../../../models/category.model';
import { BaseDialogComponent } from '../base-dialog/base-dialog.component';
import { I18N } from './constants/i18n.const';
import { CategoryEditorDialogResult } from './interfaces/category-editor-dialog-result.interface';
import { CategoryEditorDialogData } from './interfaces/category-editor-dialog-data.interface';

@Component({
  selector: 'mshk-category-editor-dialog',
  templateUrl: './category-editor-dialog.component.html',
  styleUrls: ['./category-editor-dialog.component.scss']
})
export class CategoryEditorDialogComponent extends BaseDialogComponent<CategoryEditorDialogResult> implements OnInit {
  @Input() dialogData: CategoryEditorDialogData;

  title: string;
  categoryForm: FormGroup;
  isEdit: boolean;

  readonly i18n = I18N;

  get canConfirm(): boolean {
    return super.canConfirm && this.categoryForm.valid;
  }

  constructor(dialogReference: NgbActiveModal,
              private formBuilder: FormBuilder) {
    super(dialogReference);
  }

  ngOnInit(): void {
    this.isEdit = !!this.dialogData.category;
    this.title = this.isEdit ? this.i18n.editTitle : this.i18n.addTitle;

    this.buildForm(this.isEdit ? this.dialogData.category : new Category({}));
  }

  confirmAction(): void {
    if (!this.canConfirm) {
      return;
    }

    const categoryFormValue = this.categoryForm.value;
    const category = new Category({
      id: this.isEdit ? this.dialogData.category.id : null,
      name: categoryFormValue.name,
      isSizeRequired: categoryFormValue.isSizeRequired,
      isAdditional: categoryFormValue.isAdditional
    });

    this.confirm$.next({ category });
  }

  private buildForm(category: Category) {
    this.categoryForm = this.formBuilder.group({
      name: [category.name, Validators.required],
      isSizeRequired: [!!category.isSizeRequired],
      isAdditional: [!!category.isAdditional]
    });
  }
}
