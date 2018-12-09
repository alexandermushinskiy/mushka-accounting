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

@Component({
  selector: 'psa-products-list',
  templateUrl: './products-list.component.html',
  styleUrls: ['./products-list.component.scss']
})
export class ProductsListComponent extends SortableDatatableComponent implements OnInit {
  @ViewChild(ProductsTableComponent) datatable: ProductsTableComponent;

  isCollapsed = false;
  productsRows: ProductTablePreview[];
  loadingIndicator = false;
  isModalLoading = false;
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
    this.title = category.name;
    this.selectedCategory = category;

    this.loadingIndicator = true;

    this.productsService.getProductsByCategory(category.id)
      .subscribe((products: Product[]) => {
        this.onLoadProductsSuccess(products);
      });
  }

  addProduct(content: ElementRef) {
    this.modalRef = this.modalService.open(content, this.modalConfig);
  }

  saveProduct(product: Product) {
    this.productsService.addProduct(product)
      .subscribe(
        (res: Product) => this.onSaveSuccess(res, product.id ? 'updated' : 'created'),
        () => this.onError('Unable to save Product')
      );
  }

  closeModal() {
    this.modalRef.close();
  }

  toggleCollapseMode() {
    this.isCollapsed = !this.isCollapsed;
  }

  private onLoadProductsSuccess(products) {
    this.productsRows = products.map((el, index) => new ProductTablePreview(el, index));
    this.total = products.length;
    this.isAddButtonShown = true;
    this.loadingIndicator = false;
  }

  private onError(message: string) {
    this.loadingIndicator = false;
    this.notificationsService.danger('Error', 'Unable to load products');
  }

  private onSaveSuccess(product: Product, action: string) {
    if (this.selectedCategory.id !== product.category.id) {
      this.onCategotySelected(product.category);
    }

    this.isModalLoading = false;
    this.closeModal();
    this.notificationsService.success('Success', `Product \"${product.name}\" has been successfully ${action}`);
  }
}
