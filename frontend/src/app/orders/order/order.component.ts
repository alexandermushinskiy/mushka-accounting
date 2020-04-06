import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray, NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, Observable, of } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, tap, switchMap, filter, takeUntil, map } from 'rxjs/operators';

import { NotificationsService } from '../../core/notifications/notifications.service';
import { OrdersService } from '../../core/api/orders.service';
import { Order } from '../../shared/models/order.model';
import { OrderProduct } from '../../shared/models/order-product.model';
import { ProductsServce } from '../../core/api/products.service';
import { ukrRegions } from '../shared/constants/urk-regions.const';
import { DatetimeService } from '../../core/datetime/datetime.service';
import { SelectProduct } from '../../shared/models/select-product.model';
import { uniqueOrderNumber } from '../../shared/validators/order-number.validator';
import { ComponentCanDeactivate } from '../../shared/hooks/component-can-deactivate.component';
import { CustomersService } from '../../core/api/customers.service';
import { Customer } from '../../shared/models/customer.model';

@Component({
  selector: 'mk-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss']
})
export class OrderComponent extends ComponentCanDeactivate implements OnInit {
  @ViewChild('ngForm', { static: false }) ngForm: NgForm;

  orderForm: FormGroup;
  isEdit = false;
  isLoading = false;
  orderId: string;
  errors: string[];
  title: string;
  productsList: SelectProduct[];
  regions = ukrRegions;
  profit: number;
  discount: number;
  isOrderNumberValid = true;
  isNumberValidating = false;
  searching = false;
  customerModel: any;
  private initialOrder: {};

  private quantityTerms$ = new Subject<{ index: number, quantity: number }>();

  private get customerId(): string {
    return !!this.orderForm.value.customer ? this.orderForm.value.customer.id : null;
  }

  constructor(private formBuilder: FormBuilder,
              private route: ActivatedRoute,
              private router: Router,
              private datetimeService: DatetimeService,
              private ordersService: OrdersService,
              private productsService: ProductsServce,
              private customersService: CustomersService,
              private notificationsService: NotificationsService) {
    super();
  }

  searchName = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      tap(() => this.searching = true),
      filter((term: string) => term.length >= 3),
      switchMap((term: string) => {
        return this.customersService.getByName(term).pipe(
          map(res => res.filter(cust => cust.id !== this.customerId)),
          catchError(() => of([])));
      }),
      tap(() => this.searching = false)
    )

  selectCustomer($event: any) {
    $event.preventDefault();
    const customer = $event.item;
    this.setCustomerFormValue('id', customer.id);
    this.setCustomerFormValue('firstName', customer.firstName);
    this.setCustomerFormValue('lastName', customer.lastName);
    this.setCustomerFormValue('phone', customer.phone);
    this.setCustomerFormValue('email', customer.email);
  }

  ngOnInit() {
    this.productsService.getSelect()
      .subscribe((products: SelectProduct[]) => {
        this.productsList = products;
        this.getRouteParams();
      });

    this.quantityTerms$.pipe(
      takeUntil(this.ngUnsubscribe$),
      debounceTime(300))
        .subscribe((data: {index: number, quantity: number}) => {
          this.setCostPrice(data.index);
        });
  }

  hasUnsavedData(): boolean {
    if (!this.isSaved) {
      const currentOrder = this.orderForm.getRawValue();
      return JSON.stringify(this.initialOrder) !== JSON.stringify(currentOrder);
    }
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
    this.setRecommendedPrice(index, product.recommendedPrice);
  }

  onQuantityChanged(index: number, quantity: any) {
    this.quantityTerms$.next({index, quantity});
  }

  onNumberChanged(orderNumber: string) {
    if (!orderNumber && orderNumber.trim().length === 0) {
      return;
    }

    this.isNumberValidating = true;
    this.ordersService.validateOrderNumber(orderNumber)
      .subscribe((isValid: boolean) => {
        this.isOrderNumberValid = isValid;
        this.isNumberValidating = false;

        const numberCtrl = this.orderForm.controls.number;

        numberCtrl.setValidators(isValid ? [Validators.required] : [Validators.required, uniqueOrderNumber]);
        numberCtrl.updateValueAndValidity({onlySelf: true, emitEvent: false});
      });
  }

  onDiscountChanged(discount: number) {
    this.discount = !!discount ? discount : 0;

    this.calculateTotalCost();
    this.calculateProfit();
  }

  saveOrder() {
    // const t1 = this.createOrderModel(this.orderForm.getRawValue());
    // console.info(t1);
    // return;

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
    this.isSaved = true;
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
          });
      }
    });
  }

  private buildForm(order: Order) {
    this.profit = !!order.profit ? order.profit : 0;
    this.discount = !!order.discount ? order.discount : 0;

    this.orderForm = this.formBuilder.group({
      orderDate: [order.orderDate, Validators.required],
      number: [order.number, Validators.required],
      cost: [{value: order.cost, disabled: true}],
      costMethod: [order.costMethod, Validators.required],
      isWholesale: [order.isWholesale],
      notes: [order.notes],
      region: [order.region, Validators.required],
      city: [order.city, Validators.required],
      customer: this.createCustomerFrmGroup(order.customer),
      products: this.formBuilder.array(
        order.products.map(param => this.createProductFormGroup(param))
      )
    });

    this.addFieldChangeListeners();
    this.calculateTotalCost();
    this.calculateProfit();

    this.initialOrder = this.orderForm.getRawValue();
  }

  private createCustomerFrmGroup(customer: Customer): FormGroup {
    return this.formBuilder.group({
      id: [!!customer ? customer.id : null],
      firstName: [!!customer ? customer.firstName : null, Validators.required],
      lastName: [!!customer ? customer.lastName : null, Validators.required],
      phone: [!!customer ? customer.phone : null, Validators.required],
      email: [!!customer ? customer.email : null]
    });
  }

  private createProductFormGroup(orderProduct: OrderProduct): FormGroup {
    return this.formBuilder.group({
      product: [orderProduct.product, Validators.required],
      quantity: [orderProduct.quantity, [Validators.required, Validators.min(0)]],
      unitPrice: [orderProduct.unitPrice, Validators.required],
      costPrice: [{ value: orderProduct.costPrice, disabled: true }]
    });
  }

  private addFieldChangeListeners() {
    (this.orderForm.get('products') as FormArray).valueChanges.subscribe((items: any[]) => {
      this.calculateTotalCost();
      this.calculateProfit();
    });
  }

  private calculateTotalCost() {
    let cost = 0;

    (this.orderForm.get('products') as FormArray).controls.forEach((control, index) => {
      const ctrlValue = (<FormGroup>control).getRawValue();

      if (!!control.value.quantity) {
        cost += ctrlValue.unitPrice * ctrlValue.quantity;
      }
    });

    const resultCost = cost - this.calculateDiscount(cost);

    const costCtrl = this.orderForm.get('cost');
    costCtrl.setValue(Math.round(resultCost * 100) / 100, { onlySelf: true });
    costCtrl.updateValueAndValidity();
  }

  private calculateProfit() {
    let profit = 0;

    (this.orderForm.get('products') as FormArray).controls.forEach((control) => {
      const ctrlValue = (<FormGroup>control).getRawValue();

      const unitPrice = !!ctrlValue.unitPrice ? ctrlValue.unitPrice : 0;
      const costPrice = !!ctrlValue.costPrice ? ctrlValue.costPrice : 0;
      const quantity = !!ctrlValue.quantity ? ctrlValue.quantity : 0;

      profit += (unitPrice - costPrice - this.calculateDiscount(unitPrice)) * quantity;
    });

    this.profit = profit;
  }

  private createOrderModel(formRawValue: any): Order {
    return new Order({
      id: this.orderId,
      orderDate: formRawValue.orderDate,
      number: formRawValue.number,
      cost: formRawValue.cost,
      costMethod: formRawValue.costMethod,
      discount: this.discount,
      profit: this.profit,
      notes: formRawValue.notes,
      isWholesale: !!formRawValue.isWholesale,
      region: formRawValue.region,
      city: formRawValue.city,
      customer: this.createCustomer(formRawValue.customer),
      products: formRawValue.products.map((prod: any) => this.createOrderProduct(prod))
    });
  }

  private createCustomer(formValue: any): Customer {
    return new Customer({
      id: formValue.id,
      firstName: formValue.firstName,
      lastName: formValue.lastName,
      phone: formValue.phone,
      email: formValue.email
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
          productCtrl.controls.costPrice.setValue(costPrice);
        });
    }
  }

  private setRecommendedPrice(index: number, recommendedPrice: number) {
    if (!!recommendedPrice) {
      const productCtrl = <FormGroup>(<FormArray>this.orderForm.get('products')).at(index);
      productCtrl.controls.unitPrice.setValue(recommendedPrice);
    }
  }

  private calculateDiscount(cost: number): number {
    if (this.discount === 0) {
      return 0;
    }

    return (cost / 100) * this.discount;
  }

  private setCustomerFormValue(controlName: string, value: string) {
    const ctrl = this.orderForm.controls.customer.get(controlName);
    ctrl.setValue(value, { onlySelf: true });
    ctrl.updateValueAndValidity();
  }
}
