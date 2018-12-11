import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { NgbModal, NgbModalRef, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';

import { ProductsServce } from '../../core/api/products.service';
import { ProductTablePreview } from '../shared/models/product-table-preview';
import { availableColumns } from '../../shared/constants/available-columns.const';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { ProductsTableComponent } from '../products-table/products-table.component';
import { Product } from '../../shared/models/product.model';
import { Category } from '../../shared/models/category.model';
import { SortableDatatableComponent } from '../../shared/hooks/sortable-datatable.component';
import { ProductFilter } from '../../shared/filters/product-filter';

@Component({
  selector: 'psa-products-list',
  templateUrl: './products-list.component.html',
  styleUrls: ['./products-list.component.scss']
})
export class ProductsListComponent extends SortableDatatableComponent implements OnInit {
  @ViewChild(ProductsTableComponent) datatable: ProductsTableComponent;

  isCollapsed = false;
  products: Product[];
  productsRows: ProductTablePreview[];
  loadingIndicator = false;
  total = 0;
  shown = 0;
  availableCols = availableColumns.products;
  selectedCategory: Category;
  title = 'Товары';
  isMenuToggleShown = false;
  isAddButtonShown = false;

  private modalRef: NgbModalRef;
  private readonly modalConfig: NgbModalOptions = {
    windowClass: 'products-modal',
    backdrop: 'static',
    size: 'sm'
  };

  constructor(private modalService: NgbModal,
              private productsService: ProductsServce,
              private notificationsService: NotificationsService) {
    super();
  }

  ngOnInit() {
    this.loadingIndicator = true;
  }

  onActive(event) {
    if (event.type === 'click') {
      event.cellElement.blur();
    }
  }

  onRowsUpdated(rowsAmount: number) {
    this.shown = rowsAmount;
  }

  onCategotySelected(category: Category) {
    this.title = `Товары / ${category.name}`;
    this.selectedCategory = category;

    this.loadProducts(category.id);
  }

  addProduct(content: ElementRef) {
    this.modalRef = this.modalService.open(content, this.modalConfig);
  }

  filter(searchKey) {
    const productFilter = new ProductFilter(searchKey);
    const filteredProducts = this.products.filter(prod => productFilter.filter(prod));

    this.updateDatatableRows(filteredProducts);
  }

  onProductSaved(product: Product) {
    if (this.selectedCategory.id !== product.category.id) {
      this.onCategotySelected(product.category);
    } else {
      this.loadProducts(this.selectedCategory.id);
    }

    this.closeModal();
    this.notificationsService.success('Успех', `Товар \"${product.name}\" был успешно сохранён.`);
  }

  closeModal() {
    this.modalRef.close();
  }

  toggleCollapseMode() {
    this.isCollapsed = !this.isCollapsed;
  }

  private loadProducts(categoryId: string) {
    this.loadingIndicator = true;

    this.productsService.getProductsByCategory(categoryId)
      .subscribe((products: Product[]) => {
        this.onLoadProductsSuccess(products);
      });
  }

  private onLoadProductsSuccess(products: Product[]) {
    this.products = products;
    this.total = products.length;
    this.updateDatatableRows(products);

    this.isAddButtonShown = true;
    this.loadingIndicator = false;
  }

  private updateDatatableRows(products: Product[]) {
    this.productsRows = products.map((el, index) => new ProductTablePreview(el, index));
    this.shown = products.length;
  }
}
