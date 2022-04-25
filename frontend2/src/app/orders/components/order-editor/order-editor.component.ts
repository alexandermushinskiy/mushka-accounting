import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray, NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, of, forkJoin, Subscription } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, tap, switchMap, filter, map, finalize } from 'rxjs/operators';

import { SelectProduct } from '../../../shared/models/select-product.model';
import { ComponentCanDeactivate } from '../../../shared/hooks/component-can-deactivate.component';
import { DatetimeService } from '../../../core/datetime/datetime.service';
import { OrderProduct } from '../../../shared/models/order-product.model';
import { Customer } from '../../../shared/models/customer.model';
import { Order } from '../../../shared/models/order.model';
import { uniqueOrderNumber } from '../../../shared/validators/order-number.validator';
import { OrdersFacadeService } from '../../services/orders-facade.service';
import { ApiCustomersService } from '../../../api/customers/services/api-customers.service';
import { ApiProductsService } from '../../../api/products/services/api-products.service';
import { CreateOrderService } from '../../services/create-order.service';
import { UpdateOrderService } from '../../services/update-order.service';
import { ValidateOrderNumberService } from '../../services/validate-order-number.service';
import { I18N } from './constants/i18n.const';
import { markFormGroupDirty } from '../../../shared/utils/form-utils';
import { VALIDATION_ERRORS } from '../../constants/validation-errors.const';
import { PaymentMethod } from '../../../shared/enums/payment-method.enum';
import { SelectOption } from '../../../shared/interfaces/select-option.interface';
import { OrderProductsComponent } from '../order-products/order-products.component';

@Component({
  selector: 'mshk-order-editor',
  templateUrl: './order-editor.component.html',
  styleUrls: ['./order-editor.component.scss']
})
export class OrderEditorComponent extends ComponentCanDeactivate implements OnInit, OnDestroy {
  @ViewChild('ngForm', { static: false }) ngForm: NgForm;
  @ViewChild('orderProducts', { static: false }) orderProducts: OrderProductsComponent;
  orderForm: FormGroup;
  isEdit = false;
  loadingIndicator = false;
  orderId: string;
  errors: string[];
  profit: number;
  discount: number;
  isNumberValidating$: Observable<boolean>;
  isSaving = false;
  searching = false;
  customerModel: any;

  availableProducts: SelectProduct[] = [];
  products: OrderProduct[] = [];

  searchNameSource$: (term: string) => Observable<any[]>;

  readonly i18n = I18N;
  readonly i18nRegions = this.i18n.deliveryAddress.region;
  readonly validationErrors = VALIDATION_ERRORS;
  readonly costMethodOptions: SelectOption<PaymentMethod>[] = [
    { value: PaymentMethod.CASH, label: this.i18n.payment.costMethod.options.cash },
    { value: PaymentMethod.TRANSFER_TO_CARD, label: this.i18n.payment.costMethod.options.transferToCard },
    { value: PaymentMethod.LIQPAY, label: this.i18n.payment.costMethod.options.liqPay },
    { value: PaymentMethod.SETTLEMENT_ACCOUNT, label: this.i18n.payment.costMethod.options.settlementAccount }
  ];
  readonly regionOptions: SelectOption[] = Object.values(this.i18nRegions.options)
    .map(region => ({ value: region, label: region }));

  private initialOrder: {};

  private get customerId(): string {
    return !!this.orderForm.value.customer ? this.orderForm.value.customer.id : null;
  }

  constructor(private formBuilder: FormBuilder,
              private route: ActivatedRoute,
              private router: Router,
              private ordersFacadeService: OrdersFacadeService,
              private datetimeService: DatetimeService,
              private apiProductsService: ApiProductsService,
              private apiCustomersService: ApiCustomersService,
              private createOrderService: CreateOrderService,
              private updateOrderService: UpdateOrderService,
              private validateOrderNumberService: ValidateOrderNumberService) {
    super();
  }

  ngOnInit() {
    this.readRouteParams();

    this.searchNameSource$ = (term: string) => this.apiCustomersService.getCustomerByName$(term)
      .pipe(
        map(res => res.filter(cust => cust.id !== this.customerId))
      );

    this.isNumberValidating$ = this.validateOrderNumberService.getIsLoading$();
  }

  ngOnDestroy(): void {
  }

  hasUnsavedData(): boolean {
    if (!this.isSaved) {
      const currentOrder = this.orderForm.getRawValue();
      return JSON.stringify(this.initialOrder) !== JSON.stringify(currentOrder);
    }
  }

  onProductsChanged(products: OrderProduct[]): void {
    this.products = products;

    this.calculateTotalCost();
    this.calculateProfit();
  }

  onNumberChanged(orderNumber: string) {
    if (!orderNumber && orderNumber.trim().length === 0) {
      return;
    }

    this.validateOrderNumberService.validateOrderNumber$(orderNumber)
      .subscribe((isValid: boolean) => {
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

  onSelectCustomer({ id, firstName, lastName, phone, email }: any) {
    this.setCustomerFormValue('id', id);
    this.setCustomerFormValue('firstName', firstName);
    this.setCustomerFormValue('lastName', lastName);
    this.setCustomerFormValue('phone', phone);
    this.setCustomerFormValue('email', email);
  }

  saveOrder() {
    markFormGroupDirty(this.orderForm);
    this.orderProducts.markDirty();

    const t1 = this.createOrderModel(this.orderForm.getRawValue());
    console.info(t1);
    return;

    if (this.orderForm.invalid) {
      return;
    }

    this.isSaving = true;
    const order = this.createOrderModel(this.orderForm.getRawValue());

    (this.isEdit
      ? this.updateOrderService.updateOrder$(this.orderId, order)
      : this.createOrderService.createOrder$(order))
      .subscribe(() => {
          this.isSaved = true;
          this.isSaving = false;
          this.router.navigate(['/orders']);
        }
      );
  }

  private readRouteParams() {
    this.route.params.subscribe(params => {
      this.orderId = params['id'];
      this.isEdit = !!this.orderId;

      if (this.isEdit) {
        this.loadExistingOrder();
      } else {
        this.loadNewOrder();
      }
    });
  }

  private loadNewOrder() {
    this.loadingIndicator = true;

    forkJoin(
      this.apiProductsService.getProductsForSale$(),
      this.ordersFacadeService.getOrderDefaultProducts$()
    )
    .subscribe(
      ([products, defaultProducts]) => {
        this.availableProducts = products.items;
        this.products = defaultProducts;
        this.buildNewOrderForm(defaultProducts);

        this.loadingIndicator = false;
      }
    );
  }

  private loadExistingOrder() {
    this.loadingIndicator = true;

    forkJoin(
      this.apiProductsService.getProductsForSale$(),
      this.ordersFacadeService.loadOrder$(this.orderId)
    ).subscribe(
      ([products, order]) => {
        this.availableProducts = products.items;
        this.products = order.products;

        this.buildForm(order);

        this.loadingIndicator = false;
      }
    );
  }

  private buildNewOrderForm(defaultProducts: OrderProduct[]) {
    this.buildForm(new Order({
      cost: 0,
      products: defaultProducts,
      orderDate: this.datetimeService.getCurrentDateInString()
    }));
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
      products: [order.products.length, Validators.min(1)]
    });

    this.addFieldChangeListeners();
    // this.calculateTotalCost();
    // this.calculateProfit();

    this.initialOrder = this.orderForm.getRawValue();
  }

  private createCustomerFrmGroup(customer: Customer): FormGroup {
    return this.formBuilder.group({
      id: [!!customer ? customer.id : null],
      firstName: [!!customer ? customer.firstName : null, Validators.required],
      lastName: [!!customer ? customer.lastName : null, Validators.required],
      phone: [!!customer ? customer.phone : null, Validators.required],
      email: [!!customer ? customer.email : null, Validators.email]
    });
  }

  // private createProductFormGroup(orderProduct: OrderProduct): FormGroup {
  //   const formGroup = this.formBuilder.group({
  //     product: [orderProduct.product, Validators.required],
  //     quantity: [orderProduct.quantity, [Validators.required, Validators.min(0)]],
  //     unitPrice: [orderProduct.unitPrice, Validators.required],
  //     costPrice: [{ value: orderProduct.costPrice, disabled: true }]
  //   });

    /*
    onProductSelected(product: SelectProduct, index: number) {
      console.info('onProductSelected', index)
      this.setCostPrice(index, product.quantity);
      this.setRecommendedPrice(index, product.recommendedPrice);
    }

    private setRecommendedPrice(index: number, recommendedPrice: number) {
      if (!!recommendedPrice) {
        const productCtrl = this.formProducts.at(index) as FormGroup;
        productCtrl.controls.unitPrice.setValue(recommendedPrice);
      }
    }

    onQuantityChanged(index: number, quantity: number) {
      console.info('onQuantityChanged', index, quantity)
      this.setCostPrice(index);
    }

    */
    // formGroup.controls.product.valueChanges
    //   .subscribe((product: SelectProduct) => {
    //     console.info('product', product.recommendedPrice)
    //   });

    // formGroup.controls.quantity.valueChanges
    //   .subscribe((quantity: number) => {
    //     console.info('quantity', quantity)
    //   });

    // const subscription = formGroup.valueChanges
    //   .subscribe();

    // this.productsSubscription.add(subscription);

  //   return formGroup;
  // }

  private addFieldChangeListeners() {
    // this.formProducts.valueChanges
    //   .subscribe(() => {
    //     // console.info('formProducts.valueChanges');
    //     this.calculateTotalCost();
    //     this.calculateProfit();
    //   });
  }

  private calculateTotalCost() {
    let cost = 0;

    this.products.forEach((orderProduct) => {
      if (!!orderProduct.quantity) {
        cost += orderProduct.unitPrice * orderProduct.quantity;
      }
    });


    // this.formProducts.controls.forEach((control, index) => {
    //   const ctrlValue = (control as FormGroup).getRawValue();

    //   if (!!control.value.quantity) {
    //     cost += ctrlValue.unitPrice * ctrlValue.quantity;
    //   }
    // });

    const resultCost = cost - this.calculateDiscount(cost);

    const costCtrl = this.orderForm.get('cost');
    costCtrl.setValue(Math.round(resultCost * 100) / 100, { onlySelf: true });
    costCtrl.updateValueAndValidity();
  }

  private calculateProfit() {
    let profit = 0;

    this.products.forEach((orderProduct) => {
      const unitPrice = !!orderProduct.unitPrice ? orderProduct.unitPrice : 0;
      const costPrice = !!orderProduct.costPrice ? orderProduct.costPrice : 0;
      const quantity = !!orderProduct.quantity ? orderProduct.quantity : 0;

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
      // products: formRawValue.products.map((prod: any) => this.createOrderProduct(prod))
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

  // private createOrderProduct(formValue: any): OrderProduct {
  //   return new OrderProduct({
  //     product: formValue.product,
  //     quantity: formValue.quantity,
  //     unitPrice: formValue.unitPrice,
  //     costPrice: formValue.costPrice
  //   });
  // }

//   private setCostPrice(index: number, maxQuantity: number | null = null) {
//     const productCtrl = this.formProducts.at(index) as FormGroup;
//     const ctrlValue = productCtrl.value;
// console.info('setCostPrice')
//     if (!ctrlValue.product) {
//       return;
//     }

//     if (!!maxQuantity) {
//       productCtrl.controls.quantity.setValidators([Validators.max(maxQuantity)]);
//       productCtrl.controls.quantity.updateValueAndValidity();
//     }

//     if (!!ctrlValue.quantity) {
//       const productId = ctrlValue.product.id;
//       const quantity = !!ctrlValue.quantity ? ctrlValue.quantity : 1;

//       this.apiProductsService.getProductCostPrice$(productId, quantity)
//         .subscribe((costPrice: number) => {
//           productCtrl.controls.costPrice.setValue(costPrice);
//         });
//     }
//   }

//   private setRecommendedPrice(index: number, recommendedPrice: number) {
//     if (!!recommendedPrice) {
//       const productCtrl = this.formProducts.at(index) as FormGroup;
//       productCtrl.controls.unitPrice.setValue(recommendedPrice);
//     }
//   }

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
