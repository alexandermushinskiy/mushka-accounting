import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { SuppliersService } from '../../core/api/suppliers.service';
import { ContactPerson } from '../../shared/models/contact-person.model';
import { Supplier } from '../../shared/models/supplier.model';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { PaymentMethod } from '../../shared/enums/payment-method.enum';
import { PaymentCard } from '../../shared/models/payment-card.model';

@Component({
  selector: 'mk-supplier',
  templateUrl: './supplier.component.html',
  styleUrls: ['./supplier.component.scss']
})
export class SupplierComponent implements OnInit {
  supplierForm: FormGroup;
  isEdit = false;
  isLoading = false;
  isSubmitted = false;
  supplierId: string;
  errors: string[];
  title: string;
  isFormSubmitted = false;

  constructor(private formBuilder: FormBuilder,
              private route: ActivatedRoute,
              private router: Router,
              private suppliersService: SuppliersService,
              private notificationsService: NotificationsService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.supplierId = params['id'];
      this.isEdit = !!this.supplierId;
      this.title = `${this.isEdit ? 'Редактирование' : 'Добавление'} поставщика`;

      if (this.isEdit) {
        this.suppliersService.getById(this.supplierId)
          .subscribe((supplier: Supplier) => this.buildForm(supplier));
      } else {
        this.buildForm(new Supplier({
          contactPersons: [new ContactPerson({})],
          paymentCards: [new PaymentCard({})]
        }));
      }
    });
  }

  addContactPerson() {
    const contactPersons = <FormArray>this.supplierForm.get('contactPersons');
    contactPersons.push(this.createContactPersonFormGroup(new ContactPerson({})));
  }

  removeContactPerson(index: number) {
    const contactPersons = <FormArray>this.supplierForm.get('contactPersons');
    contactPersons.removeAt(index);
  }

  addPaymentCard() {
    const paymentCards = <FormArray>this.supplierForm.get('paymentCards');
    paymentCards.push(this.createPaymentCardFormGroup(new PaymentCard({})));
  }

  removePaymentCard(index: number) {
    const paymentCards = <FormArray>this.supplierForm.get('paymentCards');
    paymentCards.removeAt(index);
  }

  saveSupplier() {
    this.isFormSubmitted = true;
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
    this.notificationsService.success(this.title, `Поставщик был успешно ${this.isEdit ? 'изменен' : 'добавлен'}`);

    this.router.navigate(['/suppliers']);
  }

  private onSaveFailed(errors: string[]) {
    this.isLoading = false;
    this.notificationsService.danger(this.title, errors[0]);
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
}
