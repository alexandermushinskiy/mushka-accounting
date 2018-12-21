import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { DeliveriesService } from '../../core/api/deliveries.service';
import { Delivery } from '../shared/models/delivery.model';
import { PaymentMethod } from '../shared/enums/payment-method.enum';
import { Product } from '../../shared/models/product.model';
import { ProductItem } from '../shared/models/product-item.model';
import { Size } from '../../shared/models/size.model';
import { ProductsServce } from '../../core/api/products.service';
import { NotificationsService } from '../../core/notifications/notifications.service';

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
  totalCost = 0;

  constructor(private formBuilder: FormBuilder,
              private route: ActivatedRoute,
              private router: Router,
              private notificationsService: NotificationsService,
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

  addProduct() {
    const products = <FormArray>this.deliveryForm.get('products');
    products.push(this.createProductModel(new ProductItem({})));
  }

  removeProduct(index: number) {
    const products = <FormArray>this.deliveryForm.get('products');
    products.removeAt(index);
  }

  onClearProducts() {
  }

  saveDelivery() {
    if (this.deliveryForm.invalid) {
      return;
    }

    this.isLoading = true;
    const delivery = this.createDeliveryModel(this.deliveryForm.value);

    (this.isEdit
      ? this.deliveryService.update(this.deliverId, delivery)
      : this.deliveryService.create(delivery))
      .subscribe(
        () => this.onSaveSuccess(),
        (errors: string[]) => this.onSaveFailed(errors)
      );
  }

  private onSaveSuccess() {
    this.isLoading = false;
    this.notificationsService.success(this.title, `Поступление было успешно ${this.isEdit ? 'изменено' : 'добавлено'}`);

    this.router.navigate(['/deliveries']);
  }

  private onSaveFailed(errors: string[]) {
    this.isLoading = false;
    this.notificationsService.danger(this.title, errors[0]);
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
      bankFee: [delivery.bankFee],
      hasPrepayment: [false],
      prepayment: [],
      cost: [delivery.cost, Validators.required],
      totalCost: [delivery.totalCost, Validators.required],
      products: this.formBuilder.array(
        delivery.products.map(param => this.createProductModel(param))
      )
    });

    (this.deliveryForm.get('products') as FormArray).valueChanges.subscribe(() => {
      this.calculateTotalCostMain();
    });

    this.deliveryForm.controls['bankFee'].valueChanges.subscribe(() => {
      this.calculateTotalCostMain();
    });

    this.deliveryForm.controls['transferFee'].valueChanges.subscribe(() => {
      this.calculateTotalCostMain();
    });
  }

  private calculateTotalCostMain() {
    const bankFeeCtrl = this.deliveryForm.controls['bankFee'];
    const transferFeeCtrl = this.deliveryForm.controls['transferFee'];

    const bankFee = !!bankFeeCtrl.value ? bankFeeCtrl.value : 0;
    const transferFee = !!transferFeeCtrl.value ? transferFeeCtrl.value : 0;

    this.totalCost = bankFee + transferFee;

    (this.deliveryForm.get('products') as FormArray).controls.forEach(control => {
      if (!!control.value.costPerItem && !!control.value.amount) {
        this.totalCost += control.value.costPerItem * control.value.amount;
      }
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

  private createDeliveryModel(deliveryFormValue: any): Delivery {
    return new Delivery({
      id: this.deliverId,
      requestDate: deliveryFormValue.requestDate || null,
      deliveryDate: deliveryFormValue.deliveryDate || null,
      supplier: deliveryFormValue.supplier,
      paymentMethod: deliveryFormValue.paymentMethod,
      transferFee: deliveryFormValue.transferFee,
      deliveryCost: deliveryFormValue.deliveryCost,
      totalCost: deliveryFormValue.totalCost
    });
  }
}
