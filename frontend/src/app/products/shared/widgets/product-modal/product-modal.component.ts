import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

import { UnsubscriberComponent } from '../../../../shared/hooks/unsubscriber.component';
import { Product } from '../../../../shared/models/product.model';
import { Category } from '../../../../shared/models/category.model';
import { ProductsServce } from '../../../../core/api/products.service';
import { CategoriesService } from '../../../../core/api/categories.service';
import { SizeItem } from '../../../../shared/models/size-item.model';
import { Size } from '../../../../shared/models/size.model';

@Component({
  selector: 'psa-product-modal',
  templateUrl: './product-modal.component.html',
  styleUrls: ['./product-modal.component.scss']
})
export class ProductModalComponent extends UnsubscriberComponent implements OnInit {
  @Input() product: Product = null;
  @Input() categoryId: string;
  @Output() onClose = new EventEmitter<void>();
  @Output() onSave = new EventEmitter<Product>();

  isSaving = false;
  productForm: FormGroup;
  productId: string;
  isEdit: boolean;

  availableSizes: Size[] = [];
  categories: Category[] = [];

  private get categoryFormGroup(): FormGroup {
    return <FormGroup>this.productForm.get('category');
  }

  constructor(private formBuilder: FormBuilder,
              private categoriesService: CategoriesService,
              private productsService: ProductsServce) {
    super();
  }

  ngOnInit() {
    this.isEdit = !!this.product;

    this.loadSizes();

    this.buildForm(this.isEdit ? this.product : new Product({}));

    this.categoriesService.getAll()
      .subscribe((categories: Category[]) => this.onCategoryChanged(categories));
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

  onCategoryChanged(categories: Category[]) {
    this.categories = categories;

    if (this.categoryId) {
      const category = categories.find(cat => cat.id === this.categoryId);
      this.categoryFormGroup.setValue(category);
    }
  }

  private buildForm(product: Product) {
    this.productForm = this.formBuilder.group({
      name: [product.name, Validators.required],
      category: [product.category, Validators.required],
      code: [product.code, Validators.required],
      isSizesRequired: [true],
      sizes: [product.sizes, Validators.required]
    });

    this.addFieldChangeListeners();
  }

  private addFieldChangeListeners() {
    const isSizesRequiredControl = this.productForm.controls['isSizesRequired'];

    isSizesRequiredControl.valueChanges.subscribe((value) => {
      this.updateSizesValidity(value);
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

  private createProductModel(productFormValue): Product {
    const sizes = !!productFormValue.sizes
      ? productFormValue.sizes.map(sizeId => new SizeItem({ id: sizeId }))
      : [];

    return new Product({
      name: productFormValue.name,
      code: productFormValue.code.toUpperCase(),
      category: this.categoryFormGroup.value,
      sizes: sizes
    });
  }

  private onSaveError(errors: string[]) {
    this.isSaving = false;
    console.info(errors);
  }
}
