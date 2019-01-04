import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { NotificationsService } from '../../core/notifications/notifications.service';
import { OrdersService } from '../../core/api/orders.service';
import { Order } from '../../shared/models/order.model';
import { OrderProduct } from '../../shared/models/order-product.model';
import { Product } from '../../shared/models/product.model';
import { ProductsServce } from '../../core/api/products.service';
import { ukrRegions } from '../shared/constants/urk-regions.const';

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
  productsList: Product[];
  regions = ukrRegions;

  constructor(private formBuilder: FormBuilder,
              private route: ActivatedRoute,
              private router: Router,
              private ordersService: OrdersService,
              private productsService: ProductsServce,
              private notificationsService: NotificationsService) { }

  ngOnInit() {
    this.productsService.getAll()
      .subscribe((products: Product[]) => {
        this.productsList = products;
        this.getRouteParams();
      });
  }

  addProduct() {
    const products = <FormArray>this.orderForm.get('products');
    products.push(this.createProductModel(new OrderProduct({})));
  }

  removeProduct(index: number) {
    const products = <FormArray>this.orderForm.get('products');
    products.removeAt(index);
  }

  getProductSizeAndVendorCode(product: Product): string {
    if (!product) {
      return '';
    }

    return product.vendorCode + (!!product.size ? ` / ${product.size.name}` : ' / -');
  }

  saveOrder() {
  }

  private getRouteParams() {
    this.route.params.subscribe(params => {
      this.orderId = params['id'];
      this.isEdit = !!this.orderId;
      this.title = `${this.isEdit ? 'Редактирование' : 'Добавление'} заказа`;

      if (this.isEdit) {
        this.ordersService.getById(this.orderId)
          .subscribe((order: Order) => this.buildForm(order));
      } else {
        this.buildForm(new Order({
          cost: 0,
          products: [new OrderProduct({})]
        }));
      }
    });
  }

  private buildForm(order: Order) {
    this.orderForm = this.formBuilder.group({
      orderDate: [order.orderDate, Validators.required],
      cost: [{value: order.cost, disabled: true}],
      costMethod: [order.costMethod, Validators.required],
      notes: [order.notes],
      products: this.formBuilder.array(
        order.products.map(param => this.createProductModel(param))
      )
    });
  }

  private createProductModel(productItem: OrderProduct): FormGroup {
    return this.formBuilder.group({
      product: [productItem.product],
      quantity: [productItem.quantity, Validators.required],
      costForItem: [productItem.costForItem, Validators.required]
    });
  }
}
