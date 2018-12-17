import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { DeliveriesService } from '../../core/api/deliveries.service';
import { Delivery } from '../shared/models/delivery.model';
import { DeliveryTableRow } from '../shared/models/delivery-table-row';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { DeliveryFilter } from '../../shared/filters/delivery.filter';

@Component({
  selector: 'mk-deliveries-list',
  templateUrl: './deliveries-list.component.html',
  styleUrls: ['./deliveries-list.component.scss']
})
export class DeliveriesListComponent implements OnInit {

  deliveries: Delivery[];
  deliveryRows: DeliveryTableRow[];
  loadingIndicator = false;
  total = 0;
  shown = 0;

  constructor(private router: Router,
              private deliveryService: DeliveriesService,
              private notificationsService: NotificationsService) { }

  ngOnInit() {
    this.loadDeliveris();
  }

  getRowClass(row: any) {
    return row.className;
  }

  addDelivery() {
    this.router.navigate(['deliveries/new']);
  }

  filter(searchKey) {
    const deliveryFilter = new DeliveryFilter(searchKey);
    const filteredDeliveries = this.deliveries.filter(delivery => deliveryFilter.filter(delivery));

    this.updateDatatableRows(filteredDeliveries);
  }
  
  delete(row: DeliveryTableRow) {
    setTimeout(() => {
    });
  }

  private loadDeliveris() {
    this.loadingIndicator = true;

    this.deliveryService.getAll()
      .subscribe(
        (res: Delivery[]) => this.onLoadSuccess(res),
        () => this.onLoadError()
      );
  }

  private onLoadSuccess(deliveries: Delivery[]) {
    this.deliveries = deliveries;
    this.total = deliveries.length;
    this.updateDatatableRows(deliveries);

    this.loadingIndicator = false;
  }

  private onLoadError() {
    this.loadingIndicator = false;
    this.notificationsService.danger('Ошибка', 'Невозможно загрузить все поставки');
  }
  
  private updateDatatableRows(deliveries: Delivery[]) {
    this.deliveryRows = deliveries.map((el, index) => new DeliveryTableRow(el, index));
    this.shown = deliveries.length;
  }
}
