import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray, AbstractControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, combineLatest } from 'rxjs';

import { SuppliesService } from '../../core/api/supplies.service';
import { Supply } from '../shared/models/supply.model';
import { ProductsServce } from '../../core/api/products.service';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { SupplyProduct } from '../shared/models/supply-product.model';
import { paymentMethods } from '../shared/constants/payment-methods.const';
import { SuppliersService } from '../../core/api/suppliers.service';
import { Supplier } from '../../shared/models/supplier.model';
import { SelectProduct } from '../../shared/models/select-product.model';

@Component({
  selector: 'mk-supply',
  templateUrl: './supply.component.html',
  styleUrls: ['./supply.component.scss']
})
export class SupplyComponent implements OnInit {
  supplyForm: FormGroup;
  supplyId: string;
  isEdit = false;
  isLoading = false;
  isSubmitted = false;
  errors: string[];
  title: string;
  paymentMethodsList = paymentMethods;
  productsList: SelectProduct[];
  totalCost: number;
  suppliers: Supplier[];
  costPriceFactor = 0;
  isFormSubmitted = false;

  constructor(private formBuilder: FormBuilder,
              private route: ActivatedRoute,
              private router: Router,
              private notificationsService: NotificationsService,
              private suppliersService: SuppliersService,
              private productsService: ProductsServce,
              private suppliesService: SuppliesService) { }

  ngOnInit() {
    this.isLoading = true;

    Observable.forkJoin(
      this.suppliersService.getAll(),
      this.productsService.getSelect()
      ).subscribe(([suppliers, products]) => {
        this.suppliers = suppliers;
        this.productsList = products;

        this.getRouteParams();

        this.isLoading = false;
      });
  }

  addProduct() {
    const products = <FormArray>this.supplyForm.get('products');
    products.push(this.createProductFormGroup(new SupplyProduct({})));
  }

  removeProduct(index: number) {
    const products = <FormArray>this.supplyForm.get('products');
    products.removeAt(index);
  }

  getProductSizeAndVendorCode(product: SelectProduct): string {
    if (!product) {
      return '';
    }

    return `${product.vendorCode} / ${(!!product.size ? `${product.size.name}` : '-')}`;
  }

  saveSupply() {
    // const t1 = this.createSupplyModel(this.supplyForm.getRawValue());
    // console.info(t1);
    // return;
    this.isFormSubmitted = true;
    if (this.supplyForm.invalid) {
      return;
    }

    this.isLoading = true;
    const supply = this.createSupplyModel(this.supplyForm.getRawValue());

    (this.isEdit
      ? this.suppliesService.update(this.supplyId, supply)
      : this.suppliesService.create(supply))
      .subscribe(
        () => this.onSaveSuccess(),
        (errors: string[]) => this.onSaveFailed(errors)
      );
  }

  private getRouteParams() {

    combineLatest(
      this.route.queryParams,
      this.route.params
    ).subscribe(([queryParams, params]) => {
      const supplyId = params['id'];
      const isCloning = !!queryParams.clone;
      this.isEdit = !!supplyId && !queryParams.clone;

      if (this.isEdit) {
        this.supplyId = supplyId;
      }

      this.title = `${this.isEdit ? 'Редактирование поступления' : 'Новое поступление'}`;

      if (this.isEdit || isCloning) {
        this.suppliesService.getById(supplyId)
          .subscribe((delivery: Supply) => this.buildForm(delivery));
      } else {
        this.buildForm(new Supply({
          cost: 0,
          products: [new SupplyProduct({})]
        }));
      }
    });
  }

  private onSaveSuccess() {
    this.isLoading = false;
    this.notificationsService.success(this.title, `Поступление было успешно ${this.isEdit ? 'изменено' : 'добавлено'}`);

    this.router.navigate(['/supplies']);
  }

  private onSaveFailed(errors: string[]) {
    this.isLoading = false;
    this.notificationsService.danger(this.title, errors[0]);
  }

  private buildForm(supply: Supply) {
    this.totalCost = !!supply.totalCost ? supply.totalCost : 0;

    this.supplyForm = this.formBuilder.group({
      requestDate: [supply.requestDate, Validators.required],
      receivedDate: [supply.receivedDate, Validators.required],
      supplier: [!!supply.supplierId ? new Supplier({id: supply.supplierId}) : null, Validators.required],
      description: [supply.description, Validators.required],
      bankFee: [supply.bankFee],
      deliveryCost: [supply.deliveryCost],
      deliveryCostMethod: [supply.deliveryCostMethod],
      prepayment: [supply.prepayment],
      prepaymentMethod: [supply.prepaymentMethod],
      cost: [{value: supply.cost, disabled: true}],
      costMethod: [supply.costMethod, Validators.required],
      notes: [supply.notes],
      products: this.formBuilder.array(
        supply.products.map(param => this.createProductFormGroup(param))
      )
    });

    this.addFieldChangeListeners();
    this.calculateCostPriceFactor();
  }

  private createProductFormGroup(productItem: SupplyProduct): FormGroup {
    return this.formBuilder.group({
      product: [productItem.product, Validators.required],
      quantity: [productItem.quantity, Validators.required],
      unitPrice: [productItem.unitPrice, Validators.required]
    });
  }

  private addFieldChangeListeners() {
    (this.supplyForm.get('products') as FormArray).valueChanges.subscribe((items: any[]) => {
      this.calculateProductsCost();
      this.calculateCostPriceFactor();
    });

    this.supplyForm.controls['deliveryCost'].valueChanges.subscribe((value: number) => {
      this.calculateTotalCost();
      this.calculateCostPriceFactor();
      this.updateControlValidator('deliveryCostMethod', !!value);
    });

    this.supplyForm.controls['bankFee'].valueChanges.subscribe(() => {
      this.calculateTotalCost();
      this.calculateCostPriceFactor();
    });

    this.supplyForm.controls['prepayment'].valueChanges.subscribe((value: number) => {
      this.calculateTotalCost();
      this.updateControlValidator('prepaymentMethod', !!value);
    });
  }

  private updateControlValidator(controlName: string, hasValidator: boolean) {
    const formCtrl = this.supplyForm.controls[controlName];
    formCtrl.setValidators(hasValidator ? [Validators.required] : []);
    formCtrl.updateValueAndValidity();
  }

  private calculateProductsCost() {
    let cost = 0;
    (this.supplyForm.get('products') as FormArray).controls.forEach(control => {
      if (!!control.value.unitPrice && !!control.value.quantity) {
        cost += control.value.unitPrice * control.value.quantity;
      }
    });

    const costCtrl = this.supplyForm.get('cost');
    costCtrl.setValue(Math.round(cost * 100) / 100, {onlySelf: true});
    costCtrl.updateValueAndValidity();

    this.calculateTotalCost();
  }

  private calculateTotalCost() {
    const bankFeeCtrl = this.supplyForm.controls['bankFee'];
    const deliveryCostCtrl = this.supplyForm.controls['deliveryCost'];
    const costCtrl = this.supplyForm.controls['cost'];

    const bankFee = !!bankFeeCtrl.value ? bankFeeCtrl.value : 0;
    const deliveryCost = !!deliveryCostCtrl.value ? deliveryCostCtrl.value : 0;
    const cost = !!costCtrl.value ? costCtrl.value : 0;

    this.totalCost = Math.round((cost + bankFee + deliveryCost) * 100) / 100;
  }

  private createSupplyModel(formRawValue: any): Supply {
    return new Supply({
      id: this.supplyId,
      requestDate: formRawValue.requestDate,
      receivedDate: formRawValue.receivedDate,
      supplierId: formRawValue.supplier.id,
      description: formRawValue.description,
      deliveryCost: formRawValue.deliveryCost,
      deliveryCostMethod: formRawValue.deliveryCostMethod,
      prepayment: formRawValue.prepayment,
      prepaymentMethod: formRawValue.prepaymentMethod,
      cost: formRawValue.cost,
      costMethod: formRawValue.costMethod,
      bankFee: formRawValue.bankFee,
      totalCost: this.totalCost,
      notes: formRawValue.notes,
      products: formRawValue.products.map((prod: any) => this.createSupplyProduct(prod))
    });
  }

  private createSupplyProduct(formValue: any): SupplyProduct {
    return new SupplyProduct({
      productId: formValue.product.id,
      quantity: formValue.quantity,
      unitPrice: formValue.unitPrice,
      costPrice: formValue.unitPrice + this.costPriceFactor
    });
  }

  private calculateCostPriceFactor() {
    const deliveryCost = this.getControlValue(this.supplyForm.controls.deliveryCost);
    const bankFee = this.getControlValue(this.supplyForm.controls.bankFee);

    if (deliveryCost === 0 && bankFee === 0) {
      this.costPriceFactor = 0;
      return;
    }

    const productsCount = (<FormArray>this.supplyForm.get('products'))
      .controls.map((ctrl: FormGroup) => ctrl.controls.quantity.value)
      .reduce((prev, cur) => {
        return prev + (!!cur ? cur : 0);
      });

    this.costPriceFactor = Math.round((deliveryCost + bankFee) / productsCount * 100) / 100;
  }

  private getControlValue(control: AbstractControl): number {
    return control.value === null ? 0 : control.value;
  }
}
