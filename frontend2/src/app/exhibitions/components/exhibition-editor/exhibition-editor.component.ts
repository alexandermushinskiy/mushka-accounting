import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';

import { SelectProduct } from '../../../shared/models/select-product.model';
import { DatetimeService } from '../../../core/datetime/datetime.service';
import { NotificationsService } from '../../../core/notifications/notifications.service';
import { Exhibition } from '../../../shared/models/exhibition.model';
import { ExhibitionProduct } from '../../../shared/models/exhibition-product.model';
import { ApiExhibitionsService } from '../../../api/exhibitions/services/api-exhibitions.services';
import { I18N } from '../../constants/i18n.const';
import { ApiProductsService } from '../../../api/products/services/api-products.service';

@Component({
  selector: 'mshk-exhibition-editor',
  templateUrl: './exhibition-editor.component.html',
  styleUrls: ['./exhibition-editor.component.scss']
})
export class ExhibitionEditorComponent implements OnInit {
  exhibitionForm: FormGroup;
  exhibitionId: string;
  isEdit = false;
  isLoading = false;
  loadingIndicator = false;
  productsList: SelectProduct[];
  profit: number;

  get formProducts(): FormArray {
    return this.exhibitionForm.get('products') as FormArray;
  }

  constructor(private formBuilder: FormBuilder,
              private route: ActivatedRoute,
              private router: Router,
              private apiProductsService: ApiProductsService,
              private apiExhibitionsService: ApiExhibitionsService,
              private datetimeService: DatetimeService,
              private notificationsService: NotificationsService) {
  }

  ngOnInit() {
    this.readRouteParams();
  }

  addProduct() {
    this.formProducts.push(this.createProductFormGroup(new ExhibitionProduct({ quantity: 1 })));
  }

  removeProduct(index: number) {
    this.formProducts.removeAt(index);
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
    this.setCostPrice(index);
  }

  saveExhibition() {
    // const t1 = this.createExhibitionModel(this.exhibitionForm.getRawValue());
    // console.info(t1);
    // return;

    if (this.exhibitionForm.invalid) {
      return;
    }

    this.isLoading = true;
    const exhibition = this.createExhibitionModel(this.exhibitionForm.getRawValue());

    (this.isEdit
      ? this.apiExhibitionsService.updateExhibition$(this.exhibitionId, exhibition)
      : this.apiExhibitionsService.createExhibition$(exhibition))
      .subscribe(
        () => this.onSaveSuccess(),
        () => this.onSaveFailed()
      );
  }

  private onSaveSuccess() {
    this.isLoading = false;
    this.notificationsService.success(this.isEdit ? I18N.messages.exhibitionUpdated : I18N.messages.exhibitionAdded);

    this.router.navigate(['/exhibitions']);
  }

  private onSaveFailed() {
    this.isLoading = false;
    this.notificationsService.error(I18N.errors.saveExhibitionError);
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

  private readRouteParams() {
    this.route.params.subscribe(params => {
      this.exhibitionId = params['id'];
      this.isEdit = !!this.exhibitionId;

      if (this.isEdit) {
        this.loadExistingExhibition();
      } else {
        this.loadNewExhibition();
      }
    });
  }

  private loadExistingExhibition() {
    this.loadingIndicator = true;

    forkJoin(
      this.apiProductsService.getProductsForSale$(),
      this.apiExhibitionsService.describeExhibition$(this.exhibitionId)
    ).subscribe(
      ([products, exhibition]) => {
        this.productsList = products;
        this.buildForm(exhibition);

        this.loadingIndicator = false;
      }
    );
  }

  private loadNewExhibition() {
    this.loadingIndicator = true;

    forkJoin(
      this.apiProductsService.getProductsForSale$(),
      this.apiExhibitionsService.getDefaultExhibitionProducts$()
    ).subscribe(
      ([products, defaultProducts]) => {
        this.productsList = products;
        this.buildForm(new Exhibition({ cost: 0, products: defaultProducts }));

        this.loadingIndicator = false;
      }
    );
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
    this.formProducts.valueChanges.subscribe((items: any[]) => {
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

    this.formProducts.controls.forEach((control) => {
      const ctrlValue = (control as FormGroup).getRawValue();

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
    const productCtrl = this.formProducts.at(index) as FormGroup;
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

      this.apiProductsService.getProductCostPrice$(productId, quantity)
        .subscribe((costPrice: number) => {
          productCtrl.controls.costPrice.setValue(costPrice);
        });
    }
  }

  private setRecommendedPrice(index: number, recommendedPrice: number) {
    if (!!recommendedPrice) {
      const productCtrl = this.formProducts.at(index) as FormGroup;
      productCtrl.controls.unitPrice.setValue(recommendedPrice);
    }
  }
}
