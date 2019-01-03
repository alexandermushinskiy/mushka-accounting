import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { NotificationsService } from '../../core/notifications/notifications.service';
import { OrdersService } from '../../core/api/orders.service';
import { Order } from '../../shared/models/order.model';

@Component({
  selector: 'mk-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss']
})
export class OrderComponent implements OnInit {
  orderForm: FormGroup;
  isEdit = false;
  isLoading = false;
  isSubmitted = false;
  orderId: string;
  errors: string[];
  title: string;

  constructor(private formBuilder: FormBuilder,
              private route: ActivatedRoute,
              private router: Router,
              private ordersService: OrdersService,
              private notificationsService: NotificationsService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.orderId = params['id'];
      this.isEdit = !!this.orderId;
      this.title = `${this.isEdit ? 'Редактирование' : 'Добавление'} заказа`;

      if (this.isEdit) {
        this.ordersService.getById(this.orderId)
          .subscribe((order: Order) => this.buildForm(order));
      } else {
        this.buildForm(new Order({}));
      }
    });
  }

  saveOrder() {
  }

  private buildForm(order: Order) {
    this.orderForm = this.formBuilder.group({

    });
  }
}
