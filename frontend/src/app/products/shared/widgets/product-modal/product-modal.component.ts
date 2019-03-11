import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { Observable } from 'rxjs';

import { UnsubscriberComponent } from '../../../../shared/hooks/unsubscriber.component';
import { Product, SubProduct } from '../../../../shared/models/product.model';
import { Category } from '../../../../shared/models/category.model';
import { ProductsServce } from '../../../../core/api/products.service';
import { CategoriesService } from '../../../../core/api/categories.service';
import { Size } from '../../../../shared/models/size.model';
import { SelectProduct } from '../../../../shared/models/select-product.model';

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

  isLoading = false;
  isSaving = false;
  productForm: FormGroup;
  isEdit: boolean;
  availableSizes: Size[] = [];
  categories: Category[] = [];
  errors: string[];
  productsList: SelectProduct[];

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
    this.isLoading = true;
    this.buildForm(new Product({ subProducts: [new SubProduct({ quantity: 1 })] }));

    Observable.forkJoin(
      this.productsService.getSelect(),
      this.isEdit ? this.productsService.getById(this.productId) : Observable.of(null),
      this.productsService.getSizes(),
      this.categoriesService.getAll()
    ).subscribe(([productsList, product, sizes, categories]) => {
      this.productsList = productsList;
      this.availableSizes = sizes;
      this.onCategoriesLoaded(categories);

      if (!!product) {
        this.buildForm(product);
      }

      this.isLoading = false;
    });
  }

  addSubproduct() {
    const products = <FormArray>this.productForm.get('subproducts');
    products.push(this.createSubproductFormGroup(new SubProduct({ quantity: 1 })));
  }

  removeSubproduct(index: number) {
    const products = <FormArray>this.productForm.get('subproducts');
    products.removeAt(index);
  }

  getProductSizeAndVendorCode(product: SelectProduct): string {
    if (!product) {
      return '';
    }

    return product.vendorCode + (!!product.size ? ` / ${product.size.name}` : ' / -');
  }

  onSubproductSelected(product: SelectProduct, index: number) {

  }

  private onCategoriesLoaded(categories: Category[]) {
    this.categories = categories;

    if (this.categoryId) {
      this.categoryFormGroup.setValue(this.getCategoryById(this.categoryId));
    }
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

  onCategoryChanged(category: Category) {
    this.categoryFormGroup.setValue(category);
    this.updateSizesValidity(category.isSizeRequired);
  }

  private buildForm(product: Product) {
    const category = this.getCategoryById(product.categoryId);
    const isSizeRequired = !!category && category.isSizeRequired;

    this.productForm = this.formBuilder.group({
      name: [product.name, Validators.required],
      category: [category, Validators.required],
      vendorCode: [product.vendorCode, Validators.required],
      recommendedPrice: [product.recommendedPrice],
      size: [{value: product.size, disabled: isSizeRequired}, isSizeRequired ? Validators.required : null],
      hasSubproducts: [product.subProducts.length > 0],
      subproducts: this.formBuilder.array(
        product.subProducts.map((subprod: SubProduct) => this.createSubproductFormGroup(subprod))
      )
    });
  }

  private createSubproductFormGroup(subproduct: SubProduct): FormGroup {
    return this.formBuilder.group({
      product: [new SelectProduct({id: subproduct.productId}), Validators.required],
      quantity: [subproduct.quantity, [Validators.required, Validators.min(0)]]
    });
  }

  private updateSizesValidity(isRequired: boolean) {
    const valueCtrl = this.productForm.controls['size'];

    if (isRequired) {
      valueCtrl.setValidators(Validators.required);
      valueCtrl.enable();
    } else {
      valueCtrl.clearValidators();
      valueCtrl.disable();
    }

    valueCtrl.updateValueAndValidity();
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
      subProducts: formRawValue.subproducts.map(subProd => this.createSubProduct(subProd))
    });
  }

  private createSubProduct(formValue: any): SubProduct {
    return new SubProduct({
      productId: formValue.product.id,
      quantity: formValue.quantity
    });
  }

  private onSaveError(errors: string[]) {
    this.isSaving = false;
    // console.info(errors);
  }
}
