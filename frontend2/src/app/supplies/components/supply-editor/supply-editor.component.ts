import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray, AbstractControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { combineLatest, forkJoin, of } from 'rxjs';

import { NotificationsService } from '../../../core/notifications/notifications.service';
import { ProductsServce } from '../../../core/api/products.service';
import { SelectProduct } from '../../../shared/models/select-product.model';
import { Supplier } from '../../../shared/models/supplier.model';
import { Supply } from '../../../shared/models/supply.model';
import { SupplyProduct } from '../../../shared/models/supply-product.model';
import { ApiSuppliersService } from '../../../api/suppliers/services/api-suppliers.service';
import { ApiSuppliesService } from '../../../api/supplies/services/api-supplies.service';

@Component({
  selector: 'mshk-supply-editor',
  templateUrl: './supply-editor.component.html',
  styleUrls: ['./supply-editor.component.scss']
})
export class SupplyEditorComponent implements OnInit {
  supplyForm: FormGroup;
  supplyId: string;
  isEdit = false;
  loadingIndicator = false;
  isLoading = false;
  productsList: SelectProduct[];
  suppliers: Supplier[];
  totalCost: number;
  costPriceFactor = 0;

  get formProducts(): FormArray {
    return this.supplyForm.get('products') as FormArray;
  }

  constructor(private formBuilder: FormBuilder,
              private route: ActivatedRoute,
              private router: Router,
              private notificationsService: NotificationsService,
              private apiSuppliersService: ApiSuppliersService,
              private productsService: ProductsServce,
              private apiSuppliesService: ApiSuppliesService) {
  }

  ngOnInit() {
    this.readRouteParams();
  }

  getProductSizeAndVendorCode(product: SelectProduct): string {
    if (!product) {
      return '';
    }

    return `${product.vendorCode} / ${(!!product.size ? `${product.size.name}` : '-')}`;
  }

  addProduct() {
    const products = this.supplyForm.get('products') as FormArray;
    products.push(this.createProductFormGroup(new SupplyProduct({})));
  }

  removeProduct(index: number) {
    const products = this.supplyForm.get('products') as FormArray;
    products.removeAt(index);
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
      ? this.apiSuppliesService.updateSupply$(this.supplyId, supply)
      : this.apiSuppliesService.createSupply$(supply))
      .subscribe(
        () => this.onSaveSuccess(),
        (errors: string[]) => this.onSaveFailed(errors)
      );
  }

  private onSaveSuccess() {
    this.isLoading = false;
    this.notificationsService.success(this.isEdit ? 'supplies.supplyWasUpdated' : 'supplies.supplyWasAdded');

    this.router.navigate(['/supplies']);
  }

  private onSaveFailed(errors: string[]) {
    this.isLoading = false;
    this.notificationsService.error(errors[0]);
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

  private readRouteParams() {
    combineLatest(
      this.route.queryParams,
      this.route.params
    ).subscribe(([queryParams, params]) => {
      this.supplyId = params['id'];
      const isCloning = !!queryParams.clone;
      this.isEdit = !!this.supplyId && !queryParams.clone;

      this.loadData(isCloning);
    });
  }

  private loadData(isCloning: boolean) {
    this.loadingIndicator = true;

    const getSupply = (this.isEdit || isCloning)
      ? this.apiSuppliesService.describeSupply$(this.supplyId)
      : of(null);

    forkJoin(
      getSupply,
      this.apiSuppliersService.searchSuppliers$(),
      this.productsService.getForSale()
      ).subscribe(([supply, suppliers, products]) => {
        this.suppliers = suppliers.items;
        this.productsList = products;

        this.buildForm(!!supply ? supply : new Supply({ cost: 0, products: [new SupplyProduct({})] }));

        this.loadingIndicator = false;
      });
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

  private calculateCostPriceFactor() {
    const deliveryCost = this.getControlValue(this.supplyForm.controls.deliveryCost);
    const bankFee = this.getControlValue(this.supplyForm.controls.bankFee);

    if (deliveryCost === 0 && bankFee === 0) {
      this.costPriceFactor = 0;
      return;
    }

    const productsCount = (this.supplyForm.get('products') as FormArray)
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
