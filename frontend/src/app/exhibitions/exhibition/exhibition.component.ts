import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';

import { ExhibitionsService } from '../../core/api/exhibitions.service';
import { Exhibition } from '../../shared/models/exhibition.model';
import { ExhibitionProduct } from '../../shared/models/exhibition-product.model';
import { SelectProduct } from '../../shared/models/select-product.model';
import { ProductsServce } from '../../core/api/products.service';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { UnsubscriberComponent } from '../../shared/hooks/unsubscriber.component';
import { DatetimeService } from '../../core/datetime/datetime.service';

@Component({
  selector: 'mk-exhibition',
  templateUrl: './exhibition.component.html',
  styleUrls: ['./exhibition.component.scss']
})
export class ExhibitionComponent extends UnsubscriberComponent implements OnInit {
  exhibitionForm: FormGroup;
  isEdit = false;
  isLoading = false;
  isFormSubmitted = false;
  exhibitionId: string;
  errors: string[];
  title: string;
  productsList: SelectProduct[];
  profit: number;

  private quantityTerms$ = new Subject<{index: number, quantity: number}>();

  constructor(private formBuilder: FormBuilder,
              private route: ActivatedRoute,
              private router: Router,
              private productsService: ProductsServce,
              private exhibitionsService: ExhibitionsService,
              private datetimeService: DatetimeService,
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
    const products = <FormArray>this.exhibitionForm.get('products');
    products.push(this.createProductFormGroup(new ExhibitionProduct({ quantity: 1 })));
  }

  removeProduct(index: number) {
    const products = <FormArray>this.exhibitionForm.get('products');
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

  saveExhibition() {
    // const t1 = this.createExhibitionModel(this.exhibitionForm.getRawValue());
    // console.info(t1);
    // return;
    this.isFormSubmitted = true;
    if (this.exhibitionForm.invalid) {
      return;
    }

    this.isLoading = true;
    const exhibition = this.createExhibitionModel(this.exhibitionForm.getRawValue());

    (this.isEdit
      ? this.exhibitionsService.update(this.exhibitionId, exhibition)
      : this.exhibitionsService.create(exhibition))
      .subscribe(
        () => this.onSaveSuccess(),
        (errors: string[]) => this.onSaveFailed(errors)
      );
  }

  private onSaveSuccess() {
    this.isLoading = false;
    this.notificationsService.success(this.title, `Выставка была успешно ${this.isEdit ? 'изменена' : 'добавлена'}`);

    this.router.navigate(['/exhibitions']);
  }

  private onSaveFailed(errors: string[]) {
    this.isLoading = false;
    this.notificationsService.danger(this.title, errors[0]);
  }

  private createExhibitionModel(formRawValue: any): Exhibition {
    return new Exhibition({
      id: this.exhibitionId,
      name: formRawValue.name,
      fromDate: formRawValue.fromDate,
      toDate: formRawValue.toDate,
      city: formRawValue.city,
      profit: this.profit,
      notes: formRawValue.notes,
      participationCost: formRawValue.participationCost,
      participationCostMethod: formRawValue.participationCostMethod,
      accommodationCost: formRawValue.accommodationCost,
      accommodationCostMethod: formRawValue.accommodationCostMethod,
      fareCost: formRawValue.fareCost,
      fareCostMethod: formRawValue.fareCostMethod,
      products: formRawValue.products.map((prod: any) => this.createExhibitionProduct(prod))
    });
  }

  private createExhibitionProduct(formValue: any): ExhibitionProduct {
    return new ExhibitionProduct({
      productId: formValue.product.id,
      quantity: formValue.quantity,
      unitPrice: formValue.unitPrice,
      costPrice: formValue.costPrice
    });
  }

  private getRouteParams() {
    this.route.params.subscribe(params => {
      this.exhibitionId = params['id'];
      this.isEdit = !!this.exhibitionId;
      this.title = `${this.isEdit ? 'Редактирование' : 'Добавление'} выставки`;

      if (this.isEdit) {
        this.exhibitionsService.getById(this.exhibitionId)
          .subscribe((exhibition: Exhibition) => this.buildForm(exhibition));
      } else {
        this.exhibitionsService.getDefaultProducts()
          .subscribe((products: ExhibitionProduct[]) => {
            this.buildForm(new Exhibition({
              cost: 0,
              products: products
            }));
          });
      }
    });
  }

  private buildForm(exhibition: Exhibition) {
    this.profit = !!exhibition.profit ? exhibition.profit : 0;

    this.exhibitionForm = this.formBuilder.group({
      name: [exhibition.name, Validators.required],
      fromDate: [exhibition.fromDate, Validators.required],
      toDate: [exhibition.toDate, Validators.required],
      city: [exhibition.city, Validators.required],
      participationCost: [exhibition.participationCost, Validators.required],
      participationCostMethod: [exhibition.participationCostMethod, Validators.required],
      accommodationCost: [exhibition.accommodationCost],
      accommodationCostMethod: [exhibition.accommodationCostMethod],
      fareCost: [exhibition.fareCost],
      fareCostMethod: [exhibition.fareCostMethod],
      notes: [exhibition.notes],
      products: this.formBuilder.array(
        exhibition.products.map(param => this.createProductFormGroup(param))
      )
    });

    this.addFieldChangeListeners();
    this.calculateProfit();
  }

  private createProductFormGroup(exhibitionProduct: ExhibitionProduct): FormGroup {
    return this.formBuilder.group({
      product: [exhibitionProduct.product, Validators.required],
      quantity: [exhibitionProduct.quantity, [Validators.required, Validators.min(0)]],
      unitPrice: [exhibitionProduct.unitPrice, Validators.required],
      costPrice: [{ value: exhibitionProduct.costPrice, disabled: true }]
    });
  }

  private addFieldChangeListeners() {
    (this.exhibitionForm.get('products') as FormArray).valueChanges.subscribe((items: any[]) => {
      this.calculateProfit();
    });

    this.exhibitionForm.controls['participationCost'].valueChanges.subscribe((value: number) => {
      this.calculateProfit();
    });

    this.exhibitionForm.controls['accommodationCost'].valueChanges.subscribe((value: number) => {
      this.calculateProfit();
      this.updateControlValidator('accommodationCostMethod', !!value);
    });

    this.exhibitionForm.controls['fareCost'].valueChanges.subscribe((value: number) => {
      this.calculateProfit();
      this.updateControlValidator('fareCostMethod', !!value);
    });

    this.exhibitionForm.controls['fromDate'].valueChanges.subscribe((fromDate: string) => {
      const toDateCtrl = this.exhibitionForm.controls['toDate'];

      if (!!toDateCtrl.value && !this.datetimeService.isDateRangeCorrect(fromDate, toDateCtrl.value)) {
        toDateCtrl.setValue(fromDate, { onlySelf: true });
      }
    });

    this.exhibitionForm.controls['toDate'].valueChanges.subscribe((toDate: string) => {
      const fromDateCtrl = this.exhibitionForm.controls['fromDate'];

      if (!!fromDateCtrl.value && !this.datetimeService.isDateRangeCorrect(fromDateCtrl.value, toDate)) {
        fromDateCtrl.setValue(toDate, { onlySelf: true });
      }
    });
  }

  private calculateProfit() {
    let profit = 0;

    (this.exhibitionForm.get('products') as FormArray).controls.forEach((control) => {
      const ctrlValue = (<FormGroup>control).getRawValue();

      const unitPrice = !!ctrlValue.unitPrice ? ctrlValue.unitPrice : 0;
      const costPrice = !!ctrlValue.costPrice ? ctrlValue.costPrice : 0;
      const quantity = !!ctrlValue.quantity ? ctrlValue.quantity : 0;

      profit += (unitPrice - costPrice) * quantity;
    });

    this.profit = profit - this.calculateExpenses();
  }

  private calculateExpenses(): number {
    const participationCost = this.getControlValue('participationCost');
    const accommodationСost = this.getControlValue('accommodationCost');
    const fareCost = this.getControlValue('fareCost');

    return participationCost + accommodationСost + fareCost;
  }

  private getControlValue(controlName: string): number {
    const ctrlValue = this.exhibitionForm.controls[controlName].value;
    return !!ctrlValue ? ctrlValue : 0;
  }

  private updateControlValidator(controlName: string, hasValidator: boolean) {
    const formCtrl = this.exhibitionForm.controls[controlName];
    formCtrl.setValidators(hasValidator ? [Validators.required] : []);
    formCtrl.updateValueAndValidity();
  }

  private setCostPrice(index: number, maxQuantity: number | null = null) {
    const productCtrl = <FormGroup>(<FormArray>this.exhibitionForm.get('products')).at(index);
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
      const productCtrl = <FormGroup>(<FormArray>this.exhibitionForm.get('products')).at(index);
      productCtrl.controls.unitPrice.setValue(recommendedPrice);
    }
  }
}
