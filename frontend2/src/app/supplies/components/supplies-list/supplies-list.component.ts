import { Component, OnInit, OnDestroy } from '@angular/core';
import { mergeMap } from 'rxjs/operators';
import { untilDestroyed } from 'ngx-take-until-destroy';
import { LocalStorage } from 'ngx-webstorage';

import { SupplyList } from '../../../shared/models/supply-list.model';
import { NotificationsService } from '../../../core/notifications/notifications.service';
import { ProductsServce } from '../../../core/api/products.service';
import { SelectProduct } from '../../../shared/models/select-product.model';
import { ItemsWithTotalCount } from '../../../shared/models/items-with-total-count.model';
import { ApiSuppliesService } from '../../../api/supplies/services/api-supplies.service';
import { DialogsService } from '../../../shared/widgets/dialogs/services/dialogs.service';
import { I18N } from '../../constants/i18n.const';
import { LanguageService } from '../../../core/language/language.service';

@Component({
  selector: 'mshk-supplies-list',
  templateUrl: './supplies-list.component.html',
  styleUrls: ['./supplies-list.component.scss']
})
export class SuppliesListComponent implements OnInit, OnDestroy {
  @LocalStorage('supplies_filter', {searchKey: null, product: null}) suppliesFilter: { searchKey: string, product: SelectProduct };
  supplies: SupplyList[];
  shownSupplies: SupplyList[];
  loadingIndicator = false;
  isProductsLoading = false;
  total = 0;
  shown = 0;
  productsList: SelectProduct[];
  sorts = [
    { prop: 'supplierName', dir: 'asc' },
    { prop: 'receivedDate', dir: null },
    { prop: 'description', dir: null }
  ];

  constructor(private apiSuppliesService: ApiSuppliesService,
              private productsService: ProductsServce,
              private dialogService: DialogsService,
              private notificationsService: NotificationsService,
              private languageService: LanguageService) {
  }

  ngOnInit() {
    this.loadProducts();
    this.loadSupplies();
  }

  ngOnDestroy(): void {
  }

  onActive(event: any) {
    if (event.type === 'click') {
      event.cellElement.blur();
    }
  }

  onSearch(searchKey: string) {
    this.suppliesFilter.searchKey = searchKey;

    this.loadSupplies();
  }

  onProductSelected(product: SelectProduct) {
    this.suppliesFilter.product = product;

    this.loadSupplies();
  }

  delete(supply: SupplyList) {
    const { title, message, cancelLabel, confirmLabel } = I18N.dialogs.deleteSupply;
    const dialog = this.dialogService.openConfirmDialog({
      title,
      message: this.languageService.translate(message, { date: supply.receivedDate, name: supply.supplierName }),
      cancelLabel,
      confirmLabel
    });

    dialog.confirm$
      .pipe(
        mergeMap(() => {
          dialog.isLoading = true;
          return this.apiSuppliesService.deleteSupply$(supply.id);
        }),
        untilDestroyed(this)
      )
      .subscribe(
        () => {
          dialog.close();
          this.onDeleteSuccess();
        },
        (error: string) => this.onDeleteFailed(error)
      );
  }

  private loadProducts() {
    this.isProductsLoading = true;

    this.productsService.getForSale()
      .subscribe((products: SelectProduct[]) => {
        this.productsList = products;
        this.isProductsLoading = false;
      });
  }

  private onDeleteSuccess() {
    this.notificationsService.success('supplies.supplyDeleted');
    this.loadSupplies();
  }

  private onDeleteFailed(error: string) {
    this.loadingIndicator = false;
    this.notificationsService.error(`Ошибка при удалении поставки: ${error}.`);
  }

  private loadSupplies() {
    this.loadingIndicator = true;

    const productId = !!this.suppliesFilter.product ? this.suppliesFilter.product.id : null;
    this.apiSuppliesService.searchSupplies$(this.suppliesFilter.searchKey, productId)
      .subscribe(
        (res: ItemsWithTotalCount<SupplyList>) => this.onLoadSuccess(res),
        () => this.onLoadError()
      );
  }

  private onLoadSuccess(response: ItemsWithTotalCount<SupplyList>) {
    this.supplies = response.items;
    this.total = response.totalCount;

    this.shownSupplies = response.items;
    this.shown = response.items.length;

    this.loadingIndicator = false;
  }

  private onLoadError() {
    this.loadingIndicator = false;
    this.notificationsService.error('supplies.loadSuppliesError');
  }

}
