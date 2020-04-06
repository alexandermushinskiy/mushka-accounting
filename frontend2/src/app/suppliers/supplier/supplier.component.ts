import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { SuppliersService } from '../../core/api/suppliers.service';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { Supplier } from '../../shared/models/supplier.model';
import { ContactPerson } from '../../shared/models/contact-person.model';
import { PaymentCard } from '../../shared/models/payment-card.model';

@Component({
  selector: 'mshk-supplier',
  templateUrl: './supplier.component.html',
  styleUrls: ['./supplier.component.scss']
})
export class SupplierComponent implements OnInit {
  supplierForm: FormGroup;
  supplierId: string;
  isEdit = false;
  isLoading = false;
  loadingIndicator = false;

  get formContactPersons(): FormArray {
    return this.supplierForm.get('contactPersons') as FormArray;
  }

  get formPaymentCards(): FormArray {
    return this.supplierForm.get('paymentCards') as FormArray;
  }

  constructor(private formBuilder: FormBuilder,
              private route: ActivatedRoute,
              private router: Router,
              private suppliersService: SuppliersService,
              private notificationsService: NotificationsService) {
  }

  ngOnInit() {
    this.readRouteParams();
  }

  addContactPerson() {
    const contactPersons = this.supplierForm.get('contactPersons') as FormArray;
    contactPersons.push(this.createContactPersonFormGroup(new ContactPerson({})));
  }

  removeContactPerson(index: number) {
    const contactPersons = this.supplierForm.get('contactPersons') as FormArray;
    contactPersons.removeAt(index);
  }

  addPaymentCard() {
    const paymentCards = this.supplierForm.get('paymentCards') as FormArray;
    paymentCards.push(this.createPaymentCardFormGroup(new PaymentCard({})));
  }

  removePaymentCard(index: number) {
    const paymentCards = this.supplierForm.get('paymentCards') as FormArray;
    paymentCards.removeAt(index);
  }

  saveSupplier() {
    if (this.supplierForm.invalid) {
      return;
    }

    this.isLoading = true;
    const supplier = this.createSupplierModel(this.supplierForm.value);

    (this.isEdit
      ? this.suppliersService.update(this.supplierId, supplier)
      : this.suppliersService.create(supplier))
      .subscribe(
        () => this.onSaveSuccess(),
        (errors: string[]) => this.onSaveFailed(errors)
      );
  }

  private onSaveSuccess() {
    this.isLoading = false;
    this.notificationsService.success(this.isEdit ? 'suppliers.supplierUpdated' : 'suppliers.supplierAdded');

    this.router.navigate(['/suppliers']);
  }

  private onSaveFailed(errors: string[]) {
    this.isLoading = false;
    this.notificationsService.error(errors[0]);
  }

  private readRouteParams() {
    this.route.params.subscribe(params => {
      this.supplierId = params['id'];
      this.isEdit = !!this.supplierId;

      if (this.isEdit) {
        this.loadSupplier();
      } else {
        this.buildForm(new Supplier({
          contactPersons: [new ContactPerson({})],
          paymentCards: [new PaymentCard({})]
        }));
      }
    });
  }

  private loadSupplier() {
    this.loadingIndicator = true;

    this.suppliersService.getById(this.supplierId)
      .subscribe(
        (supplier: Supplier) => this.onSupplierLoaded(supplier),
        () => this.onloadSupplierFailed());
  }

  private onSupplierLoaded(supplier: Supplier) {
    if (supplier.paymentCards.length === 0) {
      supplier.paymentCards = [new PaymentCard({})];
    }

    if (supplier.contactPersons.length === 0) {
      supplier.contactPersons = [new ContactPerson({})];
    }

    this.buildForm(supplier);
    this.loadingIndicator = false;
  }

  private onloadSupplierFailed() {
    this.loadingIndicator = false;
    this.notificationsService.error('suppliers.errorLoadingSupplier');
  }

  private buildForm(supplier: Supplier) {
    this.supplierForm = this.formBuilder.group({
      name: [supplier.name, Validators.required],
      address: [supplier.address],
      email: [supplier.email],
      webSite: [supplier.webSite],
      service: [supplier.service, Validators.required],
      notes: [supplier.notes],
      paymentCards: this.formBuilder.array(
        supplier.paymentCards.map(param => this.createPaymentCardFormGroup(param))
      ),
      contactPersons: this.formBuilder.array(
        supplier.contactPersons.map(param => this.createContactPersonFormGroup(param))
      )
    });
  }

  private createContactPersonFormGroup(contactPerson: ContactPerson): FormGroup {
    return this.formBuilder.group({
      id: [contactPerson.id],
      name: [contactPerson.name, Validators.required],
      email: [contactPerson.email],
      phones: [contactPerson.phones, Validators.required]
    });
  }

  private createPaymentCardFormGroup(paymentCard: PaymentCard): FormGroup {
    return this.formBuilder.group({
      id: [paymentCard.id],
      number: [paymentCard.number],
      owner: [paymentCard.owner]
    });
  }

  private createSupplierModel(supplierFormValue: any): Supplier {
    return new Supplier({
      name: supplierFormValue.name,
      address: supplierFormValue.address,
      email: supplierFormValue.email,
      webSite: supplierFormValue.webSite,
      service: supplierFormValue.service,
      notes: supplierFormValue.notes,
      contactPersons: supplierFormValue.contactPersons,
      paymentCards: supplierFormValue.paymentCards.filter(pc => !!pc.number && !!pc.owner)
    });
  }
}
