import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { DeliveriesService } from '../../core/api/deliveries.service';
import { Delivery } from '../shared/models/delivery.model';
import { PaymentMethod } from '../shared/enums/payment-method.enum';

@Component({
  selector: 'mk-delivery-tmp',
  templateUrl: './delivery-tmp.component.html',
  styleUrls: ['./delivery-tmp.component.scss']
})
export class DeliveryTmpComponent implements OnInit {

  deliveryForm: FormGroup;
  deliverId: string;
  isEdit = false;
  isLoading = false;
  isSubmitted = false;
  errors: string[];
  title: string;
  paymentMethodsList = Object.values(PaymentMethod);
  
  constructor(private formBuilder: FormBuilder,
              private route: ActivatedRoute,
              private deliveryService: DeliveriesService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.deliverId = params['id'];
      this.isEdit = !!this.deliverId;
      this.title = `${this.isEdit ? 'Редактирование поставки' : 'Новое поступление'}`;

      if (this.isEdit) {
        this.deliveryService.getById(this.deliverId)
          .subscribe((delivery: Delivery) => this.buildForm(delivery));
      } else {
        this.buildForm(new Delivery({}));
      }
    });
  }
  
  private buildForm(delivery: Delivery) {
    this.deliveryForm = this.formBuilder.group({
      requestDate: [delivery.requestDate, Validators.required],
      receivedDate: [delivery.receivedDate, Validators.required],
      supplier: [delivery.supplier, Validators.required],
      paymentMethod: [delivery.paymentMethod, Validators.required],
      hasTransferFee: [!!delivery.transferFee],
      transferFee: [delivery.transferFee],
      hasBankFee: [!!delivery.bankFee],
      bankFee: [!!delivery.bankFee],
      hasPrepayment: [false],
      prepayment: [],
      cost: [delivery.cost, Validators.required],
      totalCost: [delivery.totalCost, Validators.required] 
    });
  }
}
