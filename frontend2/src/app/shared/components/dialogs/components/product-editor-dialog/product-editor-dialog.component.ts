import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { forkJoin, of } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { untilDestroyed } from 'ngx-take-until-destroy';

import { Category } from '../../../../models/category.model';
import { Product, SubProduct } from '../../../../models/product.model';
import { SelectProduct } from '../../../../models/select-product.model';
import { BaseDialogComponent } from '../base-dialog/base-dialog.component';
import { ProductEditorDialogResult } from './interfaces/product-editor-dialog-result.interface';
import { ProductEditorDialogData } from './interfaces/product-editor-dialog-data.interface';
import { Size } from '../../../../models/size.model';

@Component({
  selector: 'mshk-product-editor-dialog',
  templateUrl: './product-editor-dialog.component.html',
  styleUrls: ['./product-editor-dialog.component.scss']
})
export class ProductEditorDialogComponent extends BaseDialogComponent<ProductEditorDialogResult> implements OnInit, OnDestroy {
  @Input() dialogData: ProductEditorDialogData;

  availableCategories: Category[];
  availableSizes: Size[];
  isLoading = false;
  isSaving = false;
  productForm: FormGroup;
  isEdit: boolean;
  errors: string[];

  get canConfirm(): boolean {
    return super.canConfirm && this.productForm.valid;
  }

  constructor(dialogReference: NgbActiveModal,
              private formBuilder: FormBuilder) {
    super(dialogReference);
  }

  ngOnInit(): void {
    this.isEdit = !!this.dialogData.product$;

    this.buildForm();
    this.loadCategoriesAndSizes();
  }

  ngOnDestroy(): void {
  }

  getProductSizeAndVendorCode(product: SelectProduct): string {
    if (!product) {
      return '';
    }

    return product.vendorCode + (!!product.size ? ` / ${product.size.name}` : ' / -');
  }

  confirmAction(): void {
    if (!this.canConfirm) {
      return;
    }

    const product = this.createProductModel(this.productForm.getRawValue());

    this.confirm$.next({ product });
  }

  private loadCategoriesAndSizes(): void {
    this.isLoading = true;

    forkJoin([
      this.dialogData.categories$,
      this.dialogData.sizes$,
      this.isEdit ? this.dialogData.product$ : of(null)
    ])
    .pipe(
      untilDestroyed(this),
      finalize(() => this.isLoading = false)
    )
    .subscribe(([categories, sizes, product]) => {
      this.availableSizes = sizes;
      this.availableCategories = categories;

      if (this.isEdit) {
        this.productForm.patchValue({
          name: product.name,
          vendorCode: product.vendorCode,
          recommendedPrice: product.recommendedPrice,
          size: product.size,
          isArchival: product.isArchival
        });
      }
    });
  }

  private buildForm() {
    const category = this.dialogData.category;

    this.productForm = this.formBuilder.group({
      name: [null, Validators.required],
      category: [category, Validators.required],
      vendorCode: [null, Validators.required],
      recommendedPrice: [null],
      size: [{ value: null, disabled: !category.isSizeRequired }],
      isArchival: [false],
      hasSubproducts: [false],
      subproducts: this.formBuilder.array([])
    });

    this.addFieldChangeListeners();
  }

  private addFieldChangeListeners() {
    this.productForm.controls['category'].valueChanges
      .subscribe((category: Category) => {
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

  private createProductModel(formRawValue: any): Product {
    return new Product({
      name: formRawValue.name,
      vendorCode: formRawValue.vendorCode.toUpperCase(),
      recommendedPrice: formRawValue.recommendedPrice,
      category: formRawValue.category,
      size: formRawValue.size,
      isArchival: formRawValue.isArchival
    });
  }

  private onSaveError(errors: string[]) {
    this.isSaving = false;
  }

  private createSubProduct(formValue: any): SubProduct {
    return new SubProduct({
      productId: formValue.product.id,
      quantity: formValue.quantity
    });
  }

  private createSubProductFormGroup(subProduct: SubProduct) {
    return this.formBuilder.group({
      product: [subProduct.productId, Validators.required],
      quantity: [subProduct.quantity, [Validators.required, Validators.min(0)]]
    });
  }
}
