import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';

import { ApiProductsService } from '../../../api/products/services/api-products.service';
import { OrderProduct } from '../../../shared/models/order-product.model';
import { SelectProduct } from '../../../shared/models/select-product.model';
import { ValidationError } from '../../enums/validation-error.enum';
import { I18N } from './constants/i18n.const';

@Component({
  selector: 'mshk-order-products',
  templateUrl: './order-products.component.html',
  styleUrls: ['./order-products.component.scss']
})
export class OrderProductsComponent implements OnInit {
  @Input() availableProducts: SelectProduct[];
  @Input() products: OrderProduct[] = [];
  @Output() onChange = new EventEmitter<OrderProduct[]>();

  formProducts: FormArray;

  readonly i18n = I18N;

  readonly errors = {
    [ValidationError.Required]: this.i18n.validation.requiredField,
    [ValidationError.Max]: this.i18n.validation.inStock
  };

  constructor(private formBuilder: FormBuilder,
              private apiProductsService: ApiProductsService) {
  }

  ngOnInit(): void {
    this.formProducts = this.formBuilder.array(
      this.products.map(prod => this.createProductFormGroup(prod))
    );
    this.emitChanges();
  }

  addProduct(): void {
    this.formProducts.push(
      this.createProductFormGroup(new OrderProduct({ quantity: 1 }))
    );
  }

  removeProduct(index: number): void {
    this.formProducts.removeAt(index);
  }

  onChanged(index: number): void {
    const productCtrl = this.formProducts.at(index) as FormGroup;
    const productId = productCtrl.controls.productId.value;

    if (!!productId) {
      const product = this.availableProducts.find(prod => prod.id === productId);

      this.setCostPrice(index);
      this.onProductSelected(index, product);

      this.emitChanges();
    }
  }

  markDirty(): void {
    this.formProducts.controls.forEach((control: FormGroup) => {
      Object.keys(control.controls)
        .forEach(key => control.controls[key].markAsDirty());
    });
  }

  private emitChanges(): void {
    const products: OrderProduct[] = this.formProducts.getRawValue()
      .map((value) => new OrderProduct({
        productId: value.productId,
        quantity: +value.quantity,
        unitPrice: value.unitPrice,
        costPrice: value.costPrice
      }));

    this.onChange.emit(products);
  }

  private createProductFormGroup(orderProduct: OrderProduct): FormGroup {
    const formGroup = this.formBuilder.group({
      productId: [orderProduct.productId, Validators.required],
      sizeAndVendorCode: [{ value: this.getSizeAndVendorCode(orderProduct.productId), disabled: true }],
      quantity: [orderProduct.quantity, Validators.compose([Validators.required, Validators.min(1)])],
      unitPrice: [orderProduct.unitPrice, Validators.required],
      costPrice: [{ value: orderProduct.costPrice, disabled: true }],
      maxQuantity: [null]
    });

    return formGroup;
  }

  private getSizeAndVendorCode(productId: string): string {
    if (!productId) {
      return null;
    }

    const product = this.availableProducts.find(prod => prod.id === productId);
    return product.vendorCode + (!!product.sizeName ? ` / ${product.sizeName}` : ' / -');
  }

  private onProductSelected(index: number, product: SelectProduct): void {
    const productCtrl = this.formProducts.at(index) as FormGroup;

    productCtrl.patchValue({
      sizeAndVendorCode: this.getSizeAndVendorCode(product.id)
    });

    if (!!product.recommendedPrice) {
      productCtrl.patchValue({
        unitPrice: product.recommendedPrice
      });
    }

    if (!!product.quantity) {
      // debugger
      productCtrl.controls.maxQuantity.setValue(product.quantity);
      productCtrl.controls.quantity.setValidators(
        [ // Validators.compose([
          Validators.required,
          Validators.min(1),
          Validators.max(product.quantity)
        ] // ])
      );
      productCtrl.controls.quantity.updateValueAndValidity();
    }

    this.setCostPrice(index);
  }

  // себестоимость
  private setCostPrice(index: number): void {
    const formGroup = this.formProducts.at(index) as FormGroup;
    const productId = formGroup.controls.productId.value;
    const quantity = formGroup.controls.quantity.value;

    this.apiProductsService.getProductCostPrice$(productId, !!quantity ? quantity : 1)
      .subscribe((costPrice: number) => {
        formGroup.patchValue({ costPrice }, { emitEvent: false, onlySelf: true });
      });
  }
}
