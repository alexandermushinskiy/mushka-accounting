import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray, NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { ukrRegions } from '../../orders/shared/constants/ukr-regions.const';
import { ComponentCanDeactivate } from '../../shared/hooks/component-can-deactivate.component';
import { DatetimeService } from '../../core/datetime/datetime.service';
import { CorporateOrdersService } from '../../core/api/corporate-orders.service';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { CorporateOrderProduct } from '../../shared/models/corporate-order-product.model';
import { uniqueOrderNumber } from '../../shared/validators/order-number.validator';
import { LanguageService } from '../../core/language/language.service';
import { CorporateOrder } from '../../shared/models/corporate-order.model';

@Component({
  selector: 'mshk-corporate-order',
  templateUrl: './corporate-order.component.html',
  styleUrls: ['./corporate-order.component.scss']
})
export class CorporateOrderComponent extends ComponentCanDeactivate implements OnInit {
  orderForm: FormGroup;
  isEdit = false;
  isLoading = false;
  orderId: string;
  profit = 0;
  errors: string[];
  title: string;
  isOrderNumberValidating = false;
  isOrderNumberValid = true;
  regions = ukrRegions;
  private initialOrder: {};

  get formProducts(): FormArray {
    return this.orderForm.get('products') as FormArray;
  }

  constructor(private formBuilder: FormBuilder,
              private route: ActivatedRoute,
              private router: Router,
              private datetimeService: DatetimeService,
              private corporateOrdersService: CorporateOrdersService,
              private languageService: LanguageService,
              private notificationsService: NotificationsService) {
    super();
  }

  ngOnInit() {
    this.getRouteParams();
  }

  hasUnsavedData(): boolean {
    if (!this.isSaved) {
      const currentOrder = this.orderForm.getRawValue();
      return JSON.stringify(this.initialOrder) !== JSON.stringify(currentOrder);
    }
  }

  addProduct() {
    this.formProducts.push(this.createProductFormGroup(new CorporateOrderProduct({ quantity: 1 })));
  }

  removeProduct(index: number) {
    this.formProducts.removeAt(index);
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

  private getRouteParams() {
    this.route.params.subscribe(params => {
      this.orderId = params['id'];
      this.isEdit = !!this.orderId;
      this.title = this.isEdit ? 'orders.editCorporateOrder' : 'orders.addCorporateOrder';

      if (this.isEdit) {
        this.corporateOrdersService.getById(this.orderId)
          .subscribe((order: CorporateOrder) => this.buildForm(order));
      } else {
        const today = this.datetimeService.getCurrentDateInString();
        const orderNumber = this.generateCorpNumber(today);
        this.buildForm(new CorporateOrder({
          createdOn: today,
          orderNumber,
          products: [new CorporateOrderProduct({ quantity: 1 })]
        }));

        this.onNumberChanged(orderNumber);
      }
    });
  }

  private onSaveSuccess() {
    this.isLoading = false;
    this.isSaved = true;

    this.notify();
    this.router.navigate(['/corporate-orders']);
  }

  private notify() {
    const messageKey = this.isEdit ? 'orders.orderUpdated' : 'orders.orderAdded';
    const resultMessage = this.languageService.translate(messageKey);

    this.notificationsService.success(resultMessage);
  }

  private onSaveFailed(errors: string[]) {
    this.isLoading = false;
    this.notificationsService.error(errors[0]);
  }

  private buildForm(order: CorporateOrder) {
    this.profit = !!order.profit ? order.profit : 0;

    this.orderForm = this.formBuilder.group({
      number: [order.orderNumber, Validators.required],
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

    this.initialOrder = this.orderForm.getRawValue();
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
      prepayment: formRawValue.prepayment,
      prepaymentMethod: formRawValue.prepaymentMethod,
      deliveryCost: formRawValue.deliveryCost,
      deliveryCostMethod: formRawValue.deliveryCostMethod,
      tax: formRawValue.tax,
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
        });
      })
    });
  }

  private generateCorpNumber(date: string): string {
    return `CORP-${this.datetimeService.convertFromToFormat(date, 'YYYY-MM-DD', 'DDMMYYYY')}`;
  }

  private calculateProfit() {
    let profit = 0;

    (this.orderForm.get('products') as FormArray).controls.forEach((control) => {
      const ctrlValue = (control as FormGroup).getRawValue();

      const unitPrice = !!ctrlValue.unitPrice ? ctrlValue.unitPrice : 0;
      const costPrice = !!ctrlValue.costPrice ? ctrlValue.costPrice : 0;
      const quantity = !!ctrlValue.quantity ? ctrlValue.quantity : 0;

      profit += (unitPrice - costPrice) * quantity;
    });

    const delivery = this.getFormControlValueAsNumber('deliveryCost');
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
      const ctrlValue = (control as FormGroup).getRawValue();

      if (!!control.value.quantity) {
        cost += ctrlValue.unitPrice * ctrlValue.quantity;
      }
    });

    const costCtrl = this.orderForm.get('cost');
    costCtrl.setValue(Math.round(cost * 100) / 100, { onlySelf: true });
    costCtrl.updateValueAndValidity();
  }
}
