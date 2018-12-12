import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

import { UnsubscriberComponent } from '../../../../shared/hooks/unsubscriber.component';
import { Product } from '../../../../shared/models/product.model';
import { Category } from '../../../../shared/models/category.model';
import { ProductsServce } from '../../../../core/api/products.service';
import { CategoriesService } from '../../../../core/api/categories.service';
import { ProductSize } from '../../../../shared/models/product-size.model';
import { Size } from '../../../../shared/models/size.model';

@Component({
  selector: 'mk-product-modal',
  templateUrl: './product-modal.component.html',
  styleUrls: ['./product-modal.component.scss']
})
export class ProductModalComponent extends UnsubscriberComponent implements OnInit {
  @Input() productId: string = null;
  @Input() categoryId: string;
  @Output() onClose = new EventEmitter<void>();
  @Output() onSave = new EventEmitter<Product>();

  isSaving = false;
  productForm: FormGroup;
  isEdit: boolean;
  availableSizes: Size[] = [];
  categories: Category[] = [];
  errors: string[];

  private get categoryFormGroup(): FormGroup {
    return <FormGroup>this.productForm.get('category');
  }

  constructor(private formBuilder: FormBuilder,
              private categoriesService: CategoriesService,
              private productsService: ProductsServce) {
    super();
  }

  ngOnInit() {
    this.isEdit = !!this.productId;

    this.loadSizes();
    this.buildForm(new Product({}));

    if (this.isEdit) {
      this.productsService.getById(this.productId)
        .subscribe((product: Product) => this.buildForm(product));
    }

    this.categoriesService.getAll()
      .subscribe((categories: Category[]) => this.onCategoriesLoaded(categories));
  }

  private onCategoriesLoaded(categories: Category[]) {
    this.categories = categories;

    if (this.categoryId) {
      this.categoryFormGroup.setValue(this.getCategoryById(this.categoryId));
    }
  }

  save() {
    if (this.productForm.invalid || (this.productForm.errors && this.productForm.errors.length > 0)) {
      return;
    }

    this.isSaving = true;
    const product = this.createProductModel(this.productForm.value);

    (this.isEdit
      ? this.productsService.update(this.productId, product)
      : this.productsService.create(product))
        .subscribe(
          (savedProduct: Product) => this.onSave.emit(savedProduct),
          (errors: string[]) => this.onSaveError(errors)
        );
  }

  close() {
    this.onClose.emit();
  }

  onCategoryChanged(category: Category) {
    this.categoryFormGroup.setValue(category);
    this.updateSizesValidity(category.isSizeRequired);
  }

  private buildForm(product: Product) {
    const sizes = product.sizes
      ? product.sizes.map(sz => sz.id)
      : [];

    const category = this.getCategoryById(product.categoryId);

    this.productForm = this.formBuilder.group({
      name: [product.name, Validators.required],
      category: [category, Validators.required],
      code: [product.code, Validators.required],
      sizes: [sizes, Validators.required]
    });
  }

  private updateSizesValidity(isRequired: boolean) {
    const valueCtrl = this.productForm.controls['sizes'];

    if (isRequired) {
      valueCtrl.setValidators(Validators.required);
    } else {
      valueCtrl.clearValidators();
    }

    valueCtrl.updateValueAndValidity();
  }

  private loadSizes() {
    this.productsService.getSizes()
      .subscribe((sizes: Size[]) => this.availableSizes = sizes);
  }

  private getCategoryById(categoryId): Category {
    return this.categories.find(cat => cat.id === categoryId);
  }

  private createProductModel(productFormValue): Product {
    const sizes = !!productFormValue.sizes
      ? productFormValue.sizes.map(sizeId => new ProductSize({ id: sizeId }))
      : [];

    return new Product({
      name: productFormValue.name,
      code: productFormValue.code.toUpperCase(),
      categoryId: productFormValue.category.id,
      sizes: sizes
    });
  }

  private onSaveError(errors: string[]) {
    this.isSaving = false;
    // console.info(errors);
  }
}
