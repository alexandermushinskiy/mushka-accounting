import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { SuppliersService } from '../../core/api/suppliers.service';
import { ContactPerson } from '../../shared/models/contact-person.model';
import { Supplier } from '../../shared/models/supplier.model';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { PaymentMethod } from '../../delivery/shared/enums/payment-method.enum';

@Component({
  selector: 'psa-supplier',
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
  paymentMethodsList = Object.values(PaymentMethod);

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
          contactPersons: [new ContactPerson({})]
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

  addPaymentMethod() {
    
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
      email: [supplier.email, Validators.required],
      webSite: [supplier.webSite],
      paymentMethod: [],
      // paymentConditions: [supplier.paymentConditions],
      service: [supplier.service, Validators.required],
      notes: [supplier.notes],
      contactPersons: this.formBuilder.array(
        supplier.contactPersons.map(param => this.createContactPersonFormGroup(param))
      )
    });
  }

  private createSupplierModel(supplierFormValue): Supplier {
    return new Supplier({
      name: supplierFormValue.name,
      address: supplierFormValue.address,
      email: supplierFormValue.email,
      webSite: supplierFormValue.webSite,
      service: supplierFormValue.service,
      notes: supplierFormValue.notes,
      contactPersons: supplierFormValue.contactPersons
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
}
