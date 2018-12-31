import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { SuppliesService } from '../../core/api/supplies.service';
import { Supply } from '../shared/models/supply.model';
import { SupplyTableRow } from '../shared/models/supply-table-row.model';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { SupplyFilter } from '../../shared/filters/supply.filter';

@Component({
  selector: 'mk-deliveries-list',
  templateUrl: './deliveries-list.component.html',
  styleUrls: ['./deliveries-list.component.scss']
})
export class DeliveriesListComponent implements OnInit {

  supplies: Supply[];
  supplyRows: SupplyTableRow[];
  loadingIndicator = false;
  total = 0;
  shown = 0;

  constructor(private router: Router,
              private suppliesService: SuppliesService,
              private notificationsService: NotificationsService) { }

  ngOnInit() {
    this.loadSupplies();
  }

  getRowClass(row: any) {
    return row.className;
  }

  addSupply() {
    this.router.navigate(['supplies/new']);
  }

  filter(searchKey: string) {
    const supplyFilter = new SupplyFilter(searchKey);
    const filteredSupplies = this.supplies.filter(supply => supplyFilter.filter(supply));

    this.updateDatatableRows(filteredSupplies);
  }
  
  delete(row: SupplyTableRow) {
    setTimeout(() => {
    });
  }

  private loadSupplies() {
    this.loadingIndicator = true;

    this.suppliesService.getAll()
      .subscribe(
        (res: Supply[]) => this.onLoadSuccess(res),
        () => this.onLoadError()
      );
  }

  private onLoadSuccess(supplies: Supply[]) {
    this.supplies = supplies;
    this.total = supplies.length;
    this.updateDatatableRows(supplies);

    this.loadingIndicator = false;
  }

  private onLoadError() {
    this.loadingIndicator = false;
    this.notificationsService.danger('Ошибка', 'Невозможно загрузить все поставки');
  }
  
  private updateDatatableRows(supplies: Supply[]) {
    this.supplyRows = supplies.map((el, index) => new SupplyTableRow(el, index));
    this.shown = supplies.length;
  }
}
