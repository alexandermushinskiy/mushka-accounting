import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { SuppliersService } from '../../core/api/suppliers.service';
import { ContactPerson } from '../../shared/models/contact-person.model';
import { Supplier } from '../../shared/models/supplier.model';

@Component({
  selector: 'psa-supplier',
  templateUrl: './supplier.component.html',
  styleUrls: ['./supplier.component.scss']
})
export class SupplierComponent implements OnInit {
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

  supplierId: string;

  constructor(private formBuilder: FormBuilder,
              private route: ActivatedRoute,
              private suppliersService: SuppliersService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.supplierId = params['id'];
      this.isEdit = !!this.supplierId;

      this.suppliersService.getById(this.supplierId)
        .subscribe((supplier: Supplier) => this.buildForm(supplier));
   });

    this.buildForm(new Supplier({}));
  }

  addContactPerson() {
    const contactPersons = <FormArray>this.supplierForm.get('contactPersons');
    contactPersons.push(this.createContactPersonFormGroup(new ContactPerson({})));
  }

  removeContactPerson(index: number) {
    const contactPersons = <FormArray>this.supplierForm.get('contactPersons');
    contactPersons.removeAt(index);
  }

  private buildForm(supplier: Supplier) {
    this.supplierForm = this.formBuilder.group({
      name: [supplier.name, Validators.required],
      address: [supplier.address],
      email: [supplier.email],
      webSite: [supplier.webSite],
      // paymentConditions: [supplier.paymentConditions],
      services: [supplier.service],
      contactPersons: this.formBuilder.array(
        supplier.contactPersons.map(param => this.createContactPersonFormGroup(param))
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
