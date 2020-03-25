import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

import { Category } from '../../../../shared/models/category.model';
import { CategoriesService } from '../../../../core/api/categories.service';

@Component({
  selector: 'mshk-category-modal',
  templateUrl: './category-modal.component.html',
  styleUrls: ['./category-modal.component.scss']
})
export class CategoryModalComponent implements OnInit {
  @Input() category: Category = null;
  @Input() categoryId: string;
  @Input() isSaving = false;
  @Output() onClose = new EventEmitter<void>();
  @Output() onSave = new EventEmitter<Category>();

  title: string;
  categoryForm: FormGroup;
  isEdit: boolean;

  constructor(private formBuilder: FormBuilder,
              private categoriesService: CategoriesService) {
  }

  ngOnInit() {
    this.isEdit = !!this.category;
    this.title = this.isEdit ? 'products.editCategory' : 'products.addCategory';

    this.buildForm(this.isEdit ? this.category : new Category({}));
  }

  close() {
    this.onClose.emit();
  }

  save() {
    if (this.categoryForm.invalid) {
      return;
    }

    const categoryFormValue = this.categoryForm.value;

    if (this.isEdit) {
      this.category.name = categoryFormValue.name;
      this.category.isSizeRequired = categoryFormValue.isSizeRequired;
      this.category.isAdditional = categoryFormValue.isAdditional;
    } else {
      this.category = new Category({
        name: categoryFormValue.name,
        isSizeRequired: categoryFormValue.isSizeRequired,
        isAdditional: categoryFormValue.isAdditional
      });
    }

    this.onSave.emit(this.category);
  }

  private buildForm(category: Category) {
    this.categoryForm = this.formBuilder.group({
      name: [category.name, Validators.required],
      isSizeRequired: [!!category.isSizeRequired],
      isAdditional: [!!category.isAdditional]
    });
  }
}
