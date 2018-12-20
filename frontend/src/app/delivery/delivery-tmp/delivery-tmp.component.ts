import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { DeliveriesService } from '../../core/api/deliveries.service';
import { Delivery } from '../shared/models/delivery.model';
import { PaymentMethod } from '../shared/enums/payment-method.enum';
import { Product } from '../../shared/models/product.model';
import { ProductItem } from '../shared/models/product-item.model';
import { Size } from '../../shared/models/size.model';
import { ProductsServce } from '../../core/api/products.service';

@Component({
  selector: 'mk-delivery-tmp',
  templateUrl: './delivery-tmp.component.html',
  styleUrls: ['./delivery-tmp.component.scss']
})
export class DeliveryTmpComponent implements OnInit {

  deliveryForm: FormGroup;
  deliverId: string;
  isEdit = false;
  isLoading = false;
  isSubmitted = false;
  errors: string[];
  title: string;
  paymentMethodsList = Object.values(PaymentMethod);
  sizesList: string[];
  productsList: Product[];

  constructor(private formBuilder: FormBuilder,
              private route: ActivatedRoute,
              private productsService: ProductsServce,
              private deliveryService: DeliveriesService) { }

  ngOnInit() {
    this.productsService.getSizes()
      .subscribe((sizes: Size[]) => this.sizesList = sizes.map(sz => sz.name));

    this.productsService.getAll()
      .subscribe((products: Product[]) => this.productsList = products);

    this.route.params.subscribe(params => {
      this.deliverId = params['id'];
      this.isEdit = !!this.deliverId;
      this.title = `${this.isEdit ? 'Редактирование поступления' : 'Новое поступление'}`;

      if (this.isEdit) {
        this.deliveryService.getById(this.deliverId)
          .subscribe((delivery: Delivery) => this.buildForm(delivery));
      } else {
        this.buildForm(new Delivery({
          products: [new ProductItem({})]
        }));
      }
    });
  }

  getProductSizes(product: Product): Size[] {
    return !!product ? product.sizes : [];
  }

  hasProductSizes(product: Product): boolean {
    return !!product && product.sizes.length > 0;
  }

  onProductSelected(index: number, product: Product) {
    const products = <FormArray>this.deliveryForm.get('products');
    const t1 = products.at(index); //.setValue({product: product});
    debugger;
  }

  addProduct() {
    const products = <FormArray>this.deliveryForm.get('products');
    products.push(this.createProductModel(new ProductItem({})));
  }

  removeProduct(index: number) {
    const products = <FormArray>this.deliveryForm.get('products');
    products.removeAt(index);
  }

  private buildForm(delivery: Delivery) {
    this.deliveryForm = this.formBuilder.group({
      requestDate: [delivery.requestDate, Validators.required],
      receivedDate: [delivery.receivedDate, Validators.required],
      supplier: [delivery.supplier, Validators.required],
      paymentMethod: [delivery.paymentMethod, Validators.required],
      hasTransferFee: [!!delivery.transferFee],
      transferFee: [delivery.transferFee],
      hasBankFee: [!!delivery.bankFee],
      bankFee: [!!delivery.bankFee],
      hasPrepayment: [false],
      prepayment: [],
      cost: [delivery.cost, Validators.required],
      totalCost: [delivery.totalCost, Validators.required],
      products: this.formBuilder.array(
        delivery.products.map(param => this.createProductModel(param))
      )
    });
  }

  private createProductModel(productItem: ProductItem): FormGroup {
    return this.formBuilder.group({
      product: [productItem.product],
      amount: [productItem.amount, Validators.required],
      size: [productItem.size],
      costPerItem: [productItem.costPerItem, Validators.required]
    });
  }
}
