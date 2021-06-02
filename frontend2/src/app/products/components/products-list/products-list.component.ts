import { Component, OnInit, OnDestroy } from '@angular/core';

import { Category } from '../../../shared/models/category.model';
import { ProductList } from '../../../shared/models/product-list.model';
import { NotificationsService } from '../../../core/notifications/notifications.service';
import { ProductListFilter } from '../../../shared/filters/product-list.filter';
import { ProductsQuickFilter } from '../../../shared/enums/products-quick-filter.enum';
import { Product } from '../../../shared/models/product.model';
import { DialogsService } from '../../../shared/components/dialogs/services/dialogs.service';
import { I18N } from '../../constants/i18n.const';
import { LanguageService } from '../../../core/language/language.service';
import { map, mergeMap } from 'rxjs/operators';
import { untilDestroyed } from 'ngx-take-until-destroy';
import { ApiCategoriesService } from '../../../api/categories/services/api-cateries.service';
import { ProductsFacadeService } from '../../services/products-facade.service';
import { Observable } from 'rxjs';
import { ApiProductsService } from '../../../api/products/services/api-products.service';

@Component({
  selector: 'mshk-products-list',
  templateUrl: './products-list.component.html',
  styleUrls: ['./products-list.component.scss']
})
export class ProductsListComponent implements OnInit, OnDestroy {
  products: ProductList[];
  shownProducts: ProductList[];
  isLoading$: Observable<boolean>;
  total = 0;
  shown = 0;
  selectedCategory: Category;
  searchKey: string;
  selectedFilter: ProductsQuickFilter;
  productToEdit: string;
  sorts = [
    { prop: 'name', dir: 'asc' },
    { prop: 'vendorCode', dir: null },
    { prop: 'createdOn', dir: null },
    { prop: 'quantity', dir: null }
  ];

  readonly i18n = I18N;
  private readonly messages = I18N.messages;

  constructor(private dialogsService: DialogsService,
              private languageService: LanguageService,
              private apiCategoriesService: ApiCategoriesService,
              private apiProductsService: ApiProductsService,
              private notificationsService: NotificationsService,
              private productsFacadeService: ProductsFacadeService) {
  }

  ngOnInit() {
    this.isLoading$ = this.productsFacadeService.getTableLoadingFlag$();

    this.productsFacadeService.getTableItems$()
      .subscribe(products => {
        this.products = products;
        this.shownProducts = products;

        this.total = products.length;
        this.shown = products.length;
      });
  }

  ngOnDestroy(): void {
  }

  onActive(event) {
    if (event.type === 'click') {
      event.cellElement.blur();
    }
  }

  onSearch(searchKey: string): void {
    this.searchKey = searchKey;
    this.filterOrders();
  }

  onFilterSelected(filter: ProductsQuickFilter): void {
    this.selectedFilter = filter;
    this.filterOrders();
  }

  onCategotySelected(category: Category): void {
    this.selectedCategory = category;

    this.productsFacadeService.fetchProducts(category.id);
  }

  addProduct(): void {
    const dialog = this.dialogsService.openProductEditorDialog({
      category: this.selectedCategory,
      sizes$: this.apiProductsService.getProductSizes$(),
      categories$: this.apiCategoriesService.searchCategories$().pipe(map(({ items }) => items)),
    });

    dialog.confirm$
      .pipe(
        untilDestroyed(this)
      )
      .subscribe(({ product }) => {
        dialog.isLoading = true;
        this.apiProductsService.createProduct$(product)
          .subscribe(
            () => {
              dialog.isLoading = false;
              dialog.close();

              this.onProductSaved(product, this.messages.productAdded);
            }
          );
      });
  }

  editProduct(productId: string) {
    const dialog = this.dialogsService.openProductEditorDialog({
      product$: this.apiProductsService.describeProduct$(productId),
      category: this.selectedCategory,
      sizes$: this.apiProductsService.getProductSizes$(),
      categories$: this.apiCategoriesService.searchCategories$().pipe(map(({ items }) => items)),
    });

    dialog.confirm$
      .pipe(
        untilDestroyed(this)
      )
      .subscribe(({ product }) => {
        dialog.isLoading = true;
        this.apiProductsService.updateProduct$(productId, product)
          .subscribe(
            () => {
              dialog.isLoading = false;
              dialog.close();

              this.onProductSaved(product, this.messages.productUpdated);
            }
          );
      });
  }

  deleteProduct(product: ProductList): void {
    const { title, message, cancelLabel, confirmLabel } = I18N.dialogs.deleteProduct;
    const dialog = this.dialogsService.openConfirmDialog({
      title,
      message: this.languageService.translate(message, { name: product.nameWithSize }),
      cancelLabel,
      confirmLabel
    });

    dialog.confirm$
      .pipe(
        mergeMap(() => {
          dialog.isLoading = true;
          return this.apiProductsService.deleteProduct$(product.id);
        }),
        untilDestroyed(this)
      )
      .subscribe(
        () => {
          dialog.close();
          this.onDeleteSuccess(product.name);
        },
        () => this.onDeleteFailed()
      );
  }

  private onProductSaved(product: Product, message: string) {
    this.notificationsService.success(message, { name: product.name });

    if (this.selectedCategory.id !== product.category.id) {
      this.onCategotySelected(product.category);
    } else {
      this.productsFacadeService.fetchProducts(this.selectedCategory.id);
    }
  }

  private filterOrders(): void {
    const orderFilter = new ProductListFilter(this.searchKey, this.selectedFilter);
    const filteredOrders = this.products.filter(order => orderFilter.filter(order));

    this.shownProducts = filteredOrders;
    this.shown = filteredOrders.length;
  }

  private onDeleteSuccess(productName: string): void {
    this.productsFacadeService.fetchProducts(this.selectedCategory.id);
    this.notificationsService.success(this.i18n.messages.productDeleted, { name: productName} );
  }

  private onDeleteFailed(): void {
    this.notificationsService.error('products.errorWhileDeletingProduct');
  }
}
