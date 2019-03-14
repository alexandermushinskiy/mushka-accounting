import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { CorporateOrdersService } from '../../core/api/corporate-orders.service';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { CorporateOrder } from '../shared/models/corporate-order.model';
import { DatetimeService } from '../../core/datetime/datetime.service';
import { CorporateOrderProduct } from '../shared/models/corporate-order-product.model';
import { ukrRegions } from '../../orders/shared/constants/urk-regions.const';
import { uniqueOrderNumber } from '../../shared/validators/order-number.validator';
import { UnsubscriberComponent } from '../../shared/hooks/unsubscriber.component';

@Component({
  selector: 'mk-corporate-order',
  templateUrl: './corporate-order.component.html',
  styleUrls: ['./corporate-order.component.scss']
})
export class CorporateOrderComponent extends UnsubscriberComponent implements OnInit {
  orderForm: FormGroup;
  isEdit = false;
  isLoading = false;
  orderId: string;
  profit = 0;
  errors: string[];
  title: string;
  isFormSubmitted = false;
  isOrderNumberValidating = false;
  isOrderNumberValid = true;
  regions = ukrRegions;

  constructor(private formBuilder: FormBuilder,
              private route: ActivatedRoute,
              private router: Router,
              private datetimeService: DatetimeService,
              private corporateOrdersService: CorporateOrdersService,
              private notificationsService: NotificationsService) {
    super();
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.orderId = params['id'];
      this.isEdit = !!this.orderId;
      this.title = `${this.isEdit ? 'Редактирование' : 'Добавление'} корпоративного заказа`;

      if (this.isEdit) {
        this.corporateOrdersService.getById(this.orderId)
          .subscribe((order: CorporateOrder) => this.buildForm(order));
      } else {
        const today = this.datetimeService.getCurrentDateInString();
        const orderNumber = this.generateCorpNumber(today);
        this.buildForm(new CorporateOrder({
          createdOn: today,
          number: orderNumber,
          products: [new CorporateOrderProduct({ quantity: 1 })]
        }));

        this.onNumberChanged(orderNumber);
      }
    });
  }

  addProduct() {
    const products = <FormArray>this.orderForm.get('products');
    products.push(this.createProductFormGroup(new CorporateOrderProduct({ quantity: 1 })));
  }

  removeProduct(index: number) {
    const products = <FormArray>this.orderForm.get('products');
    products.removeAt(index);
  }
  
  onNumberChanged(orderNumber: string) {
    if (!orderNumber && orderNumber.trim().length === 0) {
      return;
    }

    this.isOrderNumberValidating = true;
    this.corporateOrdersService.validateOrderNumber(orderNumber)
      .subscribe((isValid: boolean) => {
        this.isOrderNumberValid = isValid;
        this.isOrderNumberValidating = false;

        const numberCtrl = this.orderForm.controls.number;

        numberCtrl.setValidators(isValid ? [Validators.required] : [Validators.required, uniqueOrderNumber]);
        numberCtrl.updateValueAndValidity({onlySelf: true, emitEvent: false});
      });
  }

  saveOrder() {
    // this.isFormSubmitted = true;
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
      ? this.corporateOrdersService.update(this.orderId, order)
      : this.corporateOrdersService.create(order))
      .subscribe(
        () => this.onSaveSuccess(),
        (errors: string[]) => this.onSaveFailed(errors)
      );
  }

  private onSaveSuccess() {
    this.isLoading = false;
    this.notificationsService.success(this.title, `Корпоративный заказ был успешно ${this.isEdit ? 'изменен' : 'добавлен'}`);

    this.router.navigate(['/corporate-orders']);
  }

  private onSaveFailed(errors: string[]) {
    this.isLoading = false;
    this.notificationsService.danger(this.title, errors[0]);
  }

  private buildForm(order: CorporateOrder) {
    this.orderForm = this.formBuilder.group({
      number: [order.number, Validators.required],
      createdOn: [order.createdOn, Validators.required],
      region: [order.region, Validators.required],
      city: [order.city, Validators.required],
      companyName: [order.companyName, Validators.required],
      contactPerson: [order.contactPerson, Validators.required],
      cost: [{value: !!order.cost ? order.cost : 0, disabled: true}],
      costMethod: [order.costMethod, Validators.required],
      prepayment: [order.prepayment],
      prepaymentMethod: [order.prepaymentMethod],
      deliveryCost: [order.deliveryCost],
      deliveryCostMethod: [order.deliveryCostMethod],
      tax: [order.tax],
      phone: [order.phone, Validators.required],
      email: [order.email],
      notes: [order.notes],
      products: this.formBuilder.array(
        order.products.map(param => this.createProductFormGroup(param))
      )
    });

    this.addFieldChangeListeners();
  }

  private createProductFormGroup(product: CorporateOrderProduct): FormGroup {
    return this.formBuilder.group({
      name: [product.name, Validators.required],
      quantity: [product.quantity, Validators.required],
      unitPrice: [product.unitPrice, Validators.required],
      costPrice: [product.costPrice, Validators.required]
    });
  }

  private addFieldChangeListeners() {
    (this.orderForm.get('products') as FormArray).valueChanges.subscribe((items: any[]) => {
      this.calculateTotalCost();
      this.calculateProfit();
    });

    this.orderForm.controls['createdOn'].valueChanges.subscribe((value: string) => {
      const numberCtrl = this.orderForm.controls['number'];
      const orderNumber = this.generateCorpNumber(value);
      numberCtrl.setValue(orderNumber);
      numberCtrl.updateValueAndValidity();

      this.onNumberChanged(orderNumber);
    });

    this.orderForm.controls['tax'].valueChanges.subscribe(() => {
      this.calculateProfit();
    });

    this.orderForm.controls['deliveryCost'].valueChanges.subscribe((value: number) => {
      this.calculateProfit();
      this.updateControlValidator('deliveryCostMethod', !!value);
    });
    
    this.orderForm.controls['prepayment'].valueChanges.subscribe((value: number) => {
      this.updateControlValidator('prepaymentMethod', !!value);
    });
  }

  private updateControlValidator(controlName: string, hasValidator: boolean) {
    const formCtrl = this.orderForm.controls[controlName];
    formCtrl.setValidators(hasValidator ? [Validators.required] : []);
    formCtrl.updateValueAndValidity();
  }
  
  private createOrderModel(formRawValue: any): CorporateOrder {
    return new CorporateOrder({
      id: this.orderId,
      profit: this.profit,
      createdOn: formRawValue.createdOn,
      number: formRawValue.number,
      cost: formRawValue.cost,
      costMethod: formRawValue.costMethod,
      notes: formRawValue.notes,
      region: formRawValue.region,
      city: formRawValue.city,
      companyName: formRawValue.companyName,
      contactPerson: formRawValue.contactPerson,
      phone: formRawValue.phone,
      email: formRawValue.email,
      products: formRawValue.products.map((prod: any) => {
        return new CorporateOrderProduct({
          name: prod.name,
          quantity: prod.quantity,
          unitPrice: prod.unitPrice,
          costPrice: prod.costPrice
        })
      })
    });
  }

  private generateCorpNumber(date: string): string {
    return `CORP-${this.datetimeService.convertFromToFormat(date, 'YYYY-MM-DD', 'DDMMYYYY')}`; 
  }

  private calculateProfit() {
    let profit = 0;

    (this.orderForm.get('products') as FormArray).controls.forEach((control) => {
      const ctrlValue = (<FormGroup>control).getRawValue();

      const unitPrice = !!ctrlValue.unitPrice ? ctrlValue.unitPrice : 0;
      const costPrice = !!ctrlValue.costPrice ? ctrlValue.costPrice : 0;
      const quantity = !!ctrlValue.quantity ? ctrlValue.quantity : 0;

      profit += (unitPrice - costPrice) * quantity;
    });

    const delivery = this.getFormControlValueAsNumber('deliveryCost')
    this.profit = profit - delivery - this.calculateTax();
  }
  
  private calculateTax(): number {
    const tax = this.getFormControlValueAsNumber('tax');
    const cost = this.getFormControlValueAsNumber('cost');

    return (cost / 100) * tax;
  }

  private getFormControlValueAsNumber(controlName: string): number {
    const control = this.orderForm.controls[controlName];
    return !!control.value ? control.value : 0;
  }

  private calculateTotalCost() {
    let cost = 0;

    (this.orderForm.get('products') as FormArray).controls.forEach((control, index) => {
      const ctrlValue = (<FormGroup>control).getRawValue();

      if (!!control.value.quantity) {
        cost += ctrlValue.unitPrice * ctrlValue.quantity;
      }
    });

    const costCtrl = this.orderForm.get('cost');
    costCtrl.setValue(Math.round(cost * 100) / 100, { onlySelf: true });
    costCtrl.updateValueAndValidity();
  }
}
