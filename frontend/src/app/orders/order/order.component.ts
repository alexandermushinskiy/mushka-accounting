import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs/Subject';

import { NotificationsService } from '../../core/notifications/notifications.service';
import { OrdersService } from '../../core/api/orders.service';
import { Order } from '../../shared/models/order.model';
import { OrderProduct } from '../../shared/models/order-product.model';
import { ProductsServce } from '../../core/api/products.service';
import { ukrRegions } from '../shared/constants/urk-regions.const';
import { DatetimeService } from '../../core/datetime/datetime.service';
import { UnsubscriberComponent } from '../../shared/hooks/unsubscriber.component';
import { SelectProduct } from '../../shared/models/select-product.model';

@Component({
  selector: 'mk-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss']
})
export class OrderComponent extends UnsubscriberComponent implements OnInit {
  orderForm: FormGroup;
  isEdit = false;
  isLoading = false;
  isSubmitted = false;
  orderId: string;
  errors: string[];
  title: string;
  productsList: SelectProduct[];
  regions = ukrRegions;
  isFormSubmitted = false;

  private quantityTerms$ = new Subject<{index: number, quantity: number}>();

  constructor(private formBuilder: FormBuilder,
              private route: ActivatedRoute,
              private router: Router,
              private datetimeService: DatetimeService,
              private ordersService: OrdersService,
              private productsService: ProductsServce,
              private notificationsService: NotificationsService) {
    super();
  }

  ngOnInit() {
    this.productsService.getInStock()
      .subscribe((products: SelectProduct[]) => {
        this.productsList = products;
        this.getRouteParams();
      });

    this.quantityTerms$
      .debounceTime(300)
      .distinctUntilChanged()
      .takeUntil(this.ngUnsubscribe$)
      .subscribe((data: {index: number, quantity: number}) => {
        this.setCostPrice(data.index);
      });
  }

  addProduct() {
    const products = <FormArray>this.orderForm.get('products');
    products.push(this.createProductFormGroup(new OrderProduct({ quantity: 1 })));
  }

  removeProduct(index: number) {
    const products = <FormArray>this.orderForm.get('products');
    products.removeAt(index);
  }

  getProductSizeAndVendorCode(product: SelectProduct): string {
    if (!product) {
      return '';
    }

    return product.vendorCode + (!!product.size ? ` / ${product.size.name}` : ' / -');
  }

  onProductSelected(product: SelectProduct, index: number) {
    this.setCostPrice(index, product.quantity);
  }

  onQuantityChanged(index: number, quantity: any) {
    this.quantityTerms$.next({index, quantity});
  }

  saveOrder() {
    // const t1 = this.createOrderModel(this.orderForm.getRawValue());
    // console.info(t1);
    // return;
    this.isFormSubmitted = true;
    if (this.orderForm.invalid) {
      return;
    }

    this.isLoading = true;
    const order = this.createOrderModel(this.orderForm.getRawValue());

    (this.isEdit
      ? this.ordersService.update(this.orderId, order)
      : this.ordersService.create(order))
      .subscribe(
        () => this.onSaveSuccess(),
        (errors: string[]) => this.onSaveFailed(errors)
      );
  }

  private onSaveSuccess() {
    this.isLoading = false;
    this.notificationsService.success(this.title, `Заказ был успешно ${this.isEdit ? 'изменен' : 'добавлен'}`);

    this.router.navigate(['/orders']);
  }

  private onSaveFailed(errors: string[]) {
    this.isLoading = false;
    this.notificationsService.danger(this.title, errors[0]);
  }

  private getRouteParams() {
    this.route.params.subscribe(params => {
      this.orderId = params['id'];
      this.isEdit = !!this.orderId;
      this.title = `${this.isEdit ? 'Редактирование' : 'Добавление'} заказа`;

      if (this.isEdit) {
        this.ordersService.getById(this.orderId)
          .subscribe((order: Order) => this.buildForm(order));
      } else {
        this.ordersService.getDefaultProducts()
          .subscribe((products: OrderProduct[]) => {
            this.buildForm(new Order({
              cost: 0,
              orderDate: this.datetimeService.getCurrentDateInString(),
              products: products
            }));
          })
      }
    });
  }

  private buildForm(order: Order) {
    this.orderForm = this.formBuilder.group({
      orderDate: [order.orderDate, Validators.required],
      number: [order.number, Validators.required],
      cost: [{value: order.cost, disabled: true}],
      costMethod: [order.costMethod, Validators.required],
      notes: [order.notes],
      region: [order.region, Validators.required],
      city: [order.city, Validators.required],
      firstName: [order.firstName, Validators.required],
      lastName: [order.lastName, Validators.required],
      phone: [order.phone, Validators.required],
      email: [order.email],
      products: this.formBuilder.array(
        order.products.map(param => this.createProductFormGroup(param))
      )
    });

    this.addFieldChangeListeners();
  }

  private createProductFormGroup(orderProduct: OrderProduct): FormGroup {
    return this.formBuilder.group({
      product: [orderProduct.product, Validators.required],
      quantity: [orderProduct.quantity, [Validators.required, Validators.min(0)]],
      unitPrice: [orderProduct.unitPrice, Validators.required],
      costPrice: [{value: orderProduct.costPrice, disabled: true}]
    });
  }

  private addFieldChangeListeners() {
    (this.orderForm.get('products') as FormArray).valueChanges.subscribe((items: any[]) => {
      this.calculateProductsCost();
    });
  }

  private calculateProductsCost() {
    let cost = 0;
    (this.orderForm.get('products') as FormArray).controls.forEach((control, index) => {
      const ctrlValue = control.value;

      if (!!ctrlValue.unitPrice && !!ctrlValue.quantity) {
        cost += ctrlValue.unitPrice * ctrlValue.quantity;
      }
    });

    const costCtrl = this.orderForm.get('cost');
    costCtrl.setValue(Math.round(cost * 100) / 100, {onlySelf: true});
    costCtrl.updateValueAndValidity();
  }

  private createOrderModel(formRawValue: any): Order {
    return new Order({
      id: this.orderId,
      orderDate: formRawValue.orderDate,
      number: formRawValue.number,
      cost: formRawValue.cost,
      costMethod: formRawValue.costMethod,
      notes: formRawValue.notes,
      region: formRawValue.region,
      city: formRawValue.city,
      firstName: formRawValue.firstName,
      lastName: formRawValue.lastName,
      phone: formRawValue.phone,
      email: formRawValue.email,
      products: formRawValue.products.map((prod: any) => this.createOrderProduct(prod))
    });
  }

  private createOrderProduct(formValue: any): OrderProduct {
    return new OrderProduct({
      productId: formValue.product.id,
      quantity: formValue.quantity,
      unitPrice: formValue.unitPrice,
      costPrice: formValue.costPrice
    });
  }

  private setCostPrice(index: number, maxQuantity: number | null = null) {
    const productCtrl = <FormGroup>(<FormArray>this.orderForm.get('products')).at(index);
    const ctrlValue = productCtrl.value;

    if (!ctrlValue.product) {
      return;
    }

    if (!!maxQuantity) {
      productCtrl.controls.quantity.setValidators([Validators.max(maxQuantity)]);
      productCtrl.controls.quantity.updateValueAndValidity();
    }

    if (!!ctrlValue.quantity) {
      const productId = ctrlValue.product.id;
      const quantity = !!ctrlValue.quantity ? ctrlValue.quantity : 1;

      this.productsService.getCostPrice(productId, quantity)
        .subscribe((costPrice: number) => {
          productCtrl.controls.costPrice.setValue(costPrice, {onlySelf: true});
        });
    }
  }
}
