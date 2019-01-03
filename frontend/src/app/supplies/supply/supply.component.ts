import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';

import { SuppliesService } from '../../core/api/supplies.service';
import { Supply } from '../shared/models/supply.model';
import { PaymentMethod } from '../shared/enums/payment-method.enum';
import { Product } from '../../shared/models/product.model';
import { ProductsServce } from '../../core/api/products.service';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { SupplyProduct } from '../shared/models/supply-product.model';
import { paymentMethods } from '../shared/constants/payment-methods.const';
import { SuppliersService } from '../../core/api/suppliers.service';
import { Supplier } from '../../shared/models/supplier.model';

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
  productsList: Product[];
  totalCost: number;
  suppliers: Supplier[];

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
      this.productsService.getAll()
      ).subscribe(([suppliers, products]) => {
        this.suppliers = suppliers;
        this.productsList = products;

        this.getRouteParams();

        this.isLoading = false;
      });
  }

  private getRouteParams() {
    this.route.params.subscribe(params => {
      this.supplyId = params['id'];
      this.isEdit = !!this.supplyId;
      this.title = `${this.isEdit ? 'Редактирование поступления' : 'Новое поступление'}`;

      if (this.isEdit) {
        this.suppliesService.getById(this.supplyId)
          .subscribe((delivery: Supply) => this.buildForm(delivery));
      } else {
        this.buildForm(new Supply({
          cost: 0,
          products: [new SupplyProduct({})]
        }));
      }
    });
  }

  addProduct() {
    const products = <FormArray>this.supplyForm.get('products');
    products.push(this.createProductModel(new SupplyProduct({})));
  }

  removeProduct(index: number) {
    const products = <FormArray>this.supplyForm.get('products');
    products.removeAt(index);
  }

  getProductSize(product: Product): string {
    if (!product) {
      return '';
    }

    return !!product.size ? product.size.name : '-';
  }

  saveSupply() {
    // const t1 = this.createSupplyModel(this.supplyForm.getRawValue());
    // console.info(t1);
    // return;

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
      supplier: [new Supplier({id: supply.supplierId}), Validators.required],
      bankFee: [supply.bankFee],
      deliveryCost: [supply.deliveryCost],
      deliveryCostMethod: [supply.deliveryCostMethod],
      prepayment: [supply.prepayment],
      prepaymentMethod: [supply.prepaymentMethod],
      cost: [{value: supply.cost, disabled: true}],
      costMethod: [supply.costMethod, Validators.required],
      notes: [supply.notes],
      products: this.formBuilder.array(
        supply.products.map(param => this.createProductModel(param))
      )
    });

    this.addFieldChangeListeners();
  }

  private createProductModel(productItem: SupplyProduct): FormGroup {
    return this.formBuilder.group({
      product: [productItem.product],
      quantity: [productItem.quantity, Validators.required],
      costForItem: [productItem.costForItem, Validators.required]
    });
  }

  private addFieldChangeListeners() {
    (this.supplyForm.get('products') as FormArray).valueChanges.subscribe((items: any[]) => {
      this.calculateProductsCost();
    });

    this.supplyForm.controls['deliveryCost'].valueChanges.subscribe((value: number) => {
      this.calculateTotalCost();
      this.updateControlValidator('deliveryCostMethod', !!value);
    });

    this.supplyForm.controls['bankFee'].valueChanges.subscribe(() => {
      this.calculateTotalCost();
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
      if (!!control.value.costForItem && !!control.value.quantity) {
        cost += control.value.costForItem * control.value.quantity;
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
      costForItem: formValue.costForItem
    });
  }
}
