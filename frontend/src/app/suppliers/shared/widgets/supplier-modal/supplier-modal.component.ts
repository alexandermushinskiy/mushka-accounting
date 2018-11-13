import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';

import { Supplier } from '../../../../shared/models/supplier.model';
import { SuppliersService } from '../../../../core/api/suppliers.service';
import { ContactPerson } from '../../../../shared/models/contact-person.model';

@Component({
  selector: 'psa-supplier-modal',
  templateUrl: './supplier-modal.component.html',
  styleUrls: ['./supplier-modal.component.scss']
})
export class SupplierModalComponent implements OnInit {
  @Input() supplier: Supplier = null;
  @Input() isSaving = false;
  @Output() onClose = new EventEmitter<void>();
  @Output() onSave = new EventEmitter<Supplier>();

  supplierForm: FormGroup;
  isEdit = false;
  name: string;
  address: string;
  email: string;
  phone: string;
  webSite: string;
  paymentConditions: string;
  services: string;
  contactPersons: ContactPerson[] = [];

  constructor(private formBuilder: FormBuilder,
              private suppliersService: SuppliersService) { }

  ngOnInit() {
    this.isEdit = !!this.supplier;

    if (this.isEdit) {
      this.name = this.supplier.name;
    } else {
      this.contactPersons.push(new ContactPerson({}));
    }

    this.buildForm();
  }

  addContactPerson() {
    const contactPersons = <FormArray>this.supplierForm.get('contactPersons');
    contactPersons.push(this.createContactPersonFormGroup(new ContactPerson({})));
  }

  removeContactPerson(index: number) {
    const contactPersons = <FormArray>this.supplierForm.get('contactPersons');
    contactPersons.removeAt(index);
  }

  save() {
    if (this.supplierForm.invalid) {
      return;
    }

    const supplierFormValue = this.supplierForm.value;

    if (this.isEdit) {
      this.supplier.name = supplierFormValue.name;
    } else {
      this.supplier = new Supplier({
        name: supplierFormValue.name,
        address: supplierFormValue.address,
        email: supplierFormValue.address,
        phone: supplierFormValue.phone,
        webSite: supplierFormValue.webSite,
        contactPerson: supplierFormValue.contactPerson,
        paymentConditions: supplierFormValue.paymentConditions,
        services: supplierFormValue.services
      });
    }

    this.onSave.emit(this.supplier);
  }

  close() {
    this.onClose.emit();
  }

  private buildForm() {
    this.supplierForm = this.formBuilder.group({
      name: [this.name, Validators.required],
      address: [this.address],
      email: [this.email],
      phone: [this.phone],
      webSite: [this.webSite],
      paymentConditions: [this.paymentConditions],
      services: [this.services],
      contactPersons: this.formBuilder.array(
        this.contactPersons.map(param => this.createContactPersonFormGroup(param))
      )
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
