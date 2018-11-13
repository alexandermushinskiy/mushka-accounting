import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { SuppliersService } from '../../core/api/suppliers.service';
import { ContactPerson } from '../../shared/models/contact-person.model';
import { Supplier } from '../../shared/models/supplier.model';
import { NotificationsService } from '../../core/notifications/notifications.service';

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

  constructor(private formBuilder: FormBuilder,
              private route: ActivatedRoute,
              private router: Router,
              private suppliersService: SuppliersService,
              private notificationsService: NotificationsService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.supplierId = params['id'];
      this.isEdit = !!this.supplierId;

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

  private onSaveSuccess() {
    this.isLoading = false;
    this.notificationsService.warning('Добавление поставщика', `Поставщик был успешно добавлен`);
    
    this.router.navigate(['/suppliers']);
  }

  private onSaveFailed(errors: string[]) {
    this.isLoading = false;
    this.notificationsService.warning('Добавление поставщика', errors[0]);
  }

  private buildForm(supplier: Supplier) {
    this.supplierForm = this.formBuilder.group({
      name: [supplier.name, Validators.required],
      address: [supplier.address],
      email: [supplier.email],
      webSite: [supplier.webSite],
      // paymentConditions: [supplier.paymentConditions],
      service: [supplier.service],
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
      contactPersons: supplierFormValue.contactPersons
    });
  }


  private createContactPersonFormGroup(contactPerson: ContactPerson): FormGroup {
    return this.formBuilder.group({
      name: [contactPerson.name, Validators.required],
      email: [contactPerson.email],
      phones: [contactPerson.phones, Validators.required]
    });
  }
}
