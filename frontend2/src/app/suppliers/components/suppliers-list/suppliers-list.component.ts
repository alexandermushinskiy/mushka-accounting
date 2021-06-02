import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { DatatableComponent } from '@swimlane/ngx-datatable';
import { LocalStorage } from 'ngx-webstorage';

import { Supplier } from '../../../shared/models/supplier.model';
import { NotificationsService } from '../../../core/notifications/notifications.service';
import { SupplierListFilter } from '../../../shared/filters/supplier-list.filter';
import { ApiSuppliersService } from '../../../api/suppliers/services/api-suppliers.service';
import { ItemsList } from '../../../shared/interfaces/items-list.interface';
import { DialogsService } from '../../../shared/components/dialogs/services/dialogs.service';
import { I18N } from '../../constants/i18n.const';
import { LanguageService } from '../../../core/language/language.service';
import { mergeMap } from 'rxjs/operators';
import { untilDestroyed } from 'ngx-take-until-destroy';

@Component({
  selector: 'mshk-suppliers-list',
  templateUrl: './suppliers-list.component.html',
  styleUrls: ['./suppliers-list.component.scss']
})
export class SuppliersListComponent implements OnInit, OnDestroy {
  @ViewChild('datatable', { static: false }) datatable: DatatableComponent;
  @LocalStorage('suppliers_filter', {searchKey: null}) suppliersFilter: { searchKey: string };

  suppliers: Supplier[];
  shownSuppliers: Supplier[];
  total = 0;
  shown = 0;
  loadingIndicator = true;
  sorts = [{ prop: 'name', dir: 'asc' }];

  constructor(private dialogsService: DialogsService,
              private apiSuppliersService: ApiSuppliersService,
              private notificationsService: NotificationsService,
              private languageService: LanguageService) {
  }

  ngOnInit() {
    this.loadSuppliers();
  }

  ngOnDestroy(): void {
  }

  onActive(event: any) {
    if (event.type === 'click') {
      event.cellElement.blur();
    }
  }

  showContacts(row: Supplier) {
    this.datatable.rowDetail.toggleExpandRow(row);
  }

  delete(supplier: Supplier) {
    const { title, message, cancelLabel, confirmLabel } = I18N.dialogs.deleteSupplier;
    const dialog = this.dialogsService.openConfirmDialog({
      title,
      message: this.languageService.translate(message, { name: supplier.name }),
      cancelLabel,
      confirmLabel
    });

    dialog.confirm$
      .pipe(
        mergeMap(() => {
          dialog.isLoading = true;
          return this.apiSuppliersService.deleteSupplier$(supplier.id);
        }),
        untilDestroyed(this)
      )
      .subscribe(() => {
        dialog.close();
        this.onDeleteSuccess();
      });
  }

  onSearch(searchKey: string) {
    this.suppliersFilter.searchKey = searchKey;

    this.filterSuppliers();
  }

  private filterSuppliers() {
    const supplierFilter = new SupplierListFilter(this.suppliersFilter.searchKey);
    const filteredSuppliers = this.suppliers.filter(order => supplierFilter.filter(order));

    this.shownSuppliers = filteredSuppliers;
    this.shown = filteredSuppliers.length;
  }

  private onDeleteSuccess() {
    this.notificationsService.success(I18N.messages.supplierDeleted);
    this.loadSuppliers();
  }

  private loadSuppliers() {
    this.loadingIndicator = true;

    this.apiSuppliersService.searchSuppliers$()
      .subscribe(
        (res: ItemsList<Supplier>) => this.onLoadSuccess(res),
        () => this.onLoadError()
      );
  }

  private onLoadSuccess(suppliers: ItemsList<Supplier>) {
    this.suppliers = suppliers.items;
    this.total = suppliers.length;

    if (!!this.suppliersFilter.searchKey) {
      this.filterSuppliers();
    } else {
      this.shownSuppliers = suppliers.items;
      this.shown = suppliers.length;
    }

    this.loadingIndicator = false;
  }

  private onLoadError() {
    this.loadingIndicator = false;
    this.notificationsService.error('suppliers.errorLoadSuppliers');
  }
}
