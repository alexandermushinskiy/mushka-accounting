import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { forkJoin, of } from 'rxjs';

import { Product } from '../../../../shared/models/product.model';
import { Size } from '../../../../shared/models/size.model';
import { Category } from '../../../../shared/models/category.model';
import { SelectProduct } from '../../../../shared/models/select-product.model';
import { CategoriesService } from '../../../../core/api/categories.service';
import { ProductsServce } from '../../../../core/api/products.service';

@Component({
  selector: 'mshk-product-modal',
  templateUrl: './product-modal.component.html',
  styleUrls: ['./product-modal.component.scss']
})
export class ProductModalComponent implements OnInit {
  @Input() productId: string = null;
  @Input() categoryId: string;
  @Output() onClose = new EventEmitter<void>();
  @Output() onSave = new EventEmitter<Product>();

  isLoading = false;
  isSaving = false;
  productForm: FormGroup;
  isEdit: boolean;
  availableSizes: Size[] = [];
  categories: Category[] = [];
  errors: string[];
  productsList: SelectProduct[];

  constructor(private formBuilder: FormBuilder,
              private categoriesService: CategoriesService,
              private productsService: ProductsServce) {
  }

  ngOnInit() {
    this.isEdit = !!this.productId;
    this.buildForm(new Product({}));

    this.loadProduct();
  }

  save() {
    // const t1 = this.createProductModel(this.productForm.getRawValue());
    // console.info(t1);
    // return;
    if (this.productForm.invalid || (this.productForm.errors && this.productForm.errors.length > 0)) {
      return;
    }

    this.isSaving = true;
    const product = this.createProductModel(this.productForm.getRawValue());

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

  private loadProduct() {
    this.isLoading = true;
    this.productForm.disable();

    forkJoin(
      this.isEdit ? this.productsService.getById(this.productId) : of(null),
      this.productsService.getSizes(),
      this.categoriesService.getAll()
    ).subscribe(([product, sizes, categories]) => {
      this.availableSizes = sizes;
      this.categories = categories;

      if (this.isEdit) {
        this.productForm.patchValue({
          name: product.name,
          vendorCode: product.vendorCode,
          recommendedPrice: product.recommendedPrice,
          size: product.size,
          isArchival: product.isArchival,
          category: this.getCategoryById(product.categoryId)
        });
      }

      this.isLoading = false;
      this.productForm.enable();
    });
  }

  private buildForm(product: Product) {
    const category = this.getCategoryById(product.categoryId);
    const isSizeRequired = !!category && category.isSizeRequired;

    this.productForm = this.formBuilder.group({
      name: [product.name, Validators.required],
      category: [category, Validators.required],
      vendorCode: [product.vendorCode, Validators.required],
      recommendedPrice: [product.recommendedPrice],
      size: [{value: null, disabled: true}],
      isArchival: [!!product.isArchival]
    });

    this.addFieldChangeListeners();
  }

  private addFieldChangeListeners() {
    this.productForm.controls['category'].valueChanges.subscribe((category: Category) => {
      if (!!category) {
        this.updateSizesValidity(category.isSizeRequired);
      }
    });
  }

  private updateSizesValidity(isRequired: boolean) {
    const sizeCtrl = this.productForm.controls['size'];

    if (isRequired) {
      sizeCtrl.setValidators(Validators.required);
      sizeCtrl.enable();
    } else {
      sizeCtrl.setValue(null);
      sizeCtrl.clearValidators();
      sizeCtrl.disable();
    }

    sizeCtrl.updateValueAndValidity();
  }

  private getCategoryById(categoryId: string): Category {
    return this.categories.find(cat => cat.id === categoryId);
  }

  private createProductModel(formRawValue: any): Product {
    return new Product({
      name: formRawValue.name,
      vendorCode: formRawValue.vendorCode.toUpperCase(),
      recommendedPrice: formRawValue.recommendedPrice,
      categoryId: formRawValue.category.id,
      size: formRawValue.size,
      isArchival: formRawValue.isArchival
    });
  }

  private onSaveError(errors: string[]) {
    this.isSaving = false;
  }
}
