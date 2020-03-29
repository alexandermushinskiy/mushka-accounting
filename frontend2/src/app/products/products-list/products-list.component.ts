import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { NgbModal, NgbModalRef, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';

import { Category } from '../../shared/models/category.model';
import { ProductList } from '../../shared/models/product-list.model';
import { ProductsServce } from '../../core/api/products.service';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { ProductListFilter } from '../../shared/filters/product-list.filter';
import { ProductsQuickFilter } from '../../shared/enums/products-quick-filter.enum';
import { Product } from '../../shared/models/product.model';

@Component({
  selector: 'mshk-products-list',
  templateUrl: './products-list.component.html',
  styleUrls: ['./products-list.component.scss']
})
export class ProductsListComponent implements OnInit {
  @ViewChild('confirmRemoveTmpl', { static: false }) confirmRemoveTmpl: ElementRef;
  products: ProductList[];
  shownProducts: ProductList[];
  loadingIndicator = false;
  total = 0;
  shown = 0;
  title = 'products.products';
  selectedCategory: Category;
  searchKey: string;
  selectedFilter: ProductsQuickFilter;
  productToEdit: string;
  productToDelete: ProductList;
  sorts = [
    { prop: 'name', dir: 'asc' },
    { prop: 'vendorCode', dir: null },
    { prop: 'createdOn', dir: null },
    { prop: 'quantity', dir: null }
  ];

  private modalRef: NgbModalRef;
  private readonly modalConfig: NgbModalOptions = {
    windowClass: 'products-modal',
    backdrop: 'static',
    size: 'sm'
  };

  constructor(private modalService: NgbModal,
              private productsService: ProductsServce,
              private notificationsService: NotificationsService) {
  }

  ngOnInit() {
    // this.loadingIndicator = true;
  }

  onActive(event) {
    if (event.type === 'click') {
      event.cellElement.blur();
    }
  }

  onSearch(searchKey: string) {
    this.searchKey = searchKey;
    this.filterOrders();
  }

  onFilterSelected(filter: ProductsQuickFilter) {
    this.selectedFilter = filter;
    this.filterOrders();
  }

  onCategotySelected(category: Category) {
    this.selectedCategory = category;

    this.loadProducts(category.id);
  }

  addProduct(addProductTmpl: any) {
    this.modalRef = this.modalService.open(addProductTmpl, this.modalConfig);
  }

  onProductSaved(product: Product) {
    this.closeModal();
    this.notificationsService.success('products.productSaved', { name: product.name });

    if (this.selectedCategory.id !== product.categoryId) {
      this.onCategotySelected(product.category);
    } else {
      this.loadProducts(this.selectedCategory.id);
    }
  }

  edit(content: any, productId: string) {
    setTimeout(() => {
      this.productToEdit = productId;
      this.modalRef = this.modalService.open(content, this.modalConfig);
    });
  }

  delete(row: ProductList) {
    setTimeout(() => {
      this.productToDelete = row;
      this.modalRef = this.modalService.open(this.confirmRemoveTmpl, this.modalConfig);
    });
  }

  confirmDelete() {
    this.productsService.delete(this.productToDelete.id)
      .subscribe(
        () => this.onDeleteSuccess(),
        (errors) => this.onDeleteFailed()
      );

    this.closeModal();
  }

  closeModal() {
    this.modalRef.close();

    this.productToDelete = null;
    this.productToEdit = null;
  }

  private filterOrders() {
    const orderFilter = new ProductListFilter(this.searchKey, this.selectedFilter);
    const filteredOrders = this.products.filter(order => orderFilter.filter(order));

    this.shownProducts = filteredOrders;
    this.shown = filteredOrders.length;
  }

  private onDeleteSuccess() {
    this.loadProducts(this.selectedCategory.id);
    this.notificationsService.success('products.productWasDeleted');
  }

  private onDeleteFailed() {
    this.notificationsService.error('products.errorWhileDeletingProduct');
  }

  private loadProducts(categoryId: string) {
    this.loadingIndicator = true;

    this.productsService.getByCategory(categoryId)
      .subscribe((products: ProductList[]) => {
        this.onLoadProductsSuccess(products);
      });
  }

  private onLoadProductsSuccess(products: ProductList[]) {
    this.products = products;
    this.shownProducts = products;

    this.total = products.length;
    this.shown = products.length;

    this.loadingIndicator = false;
  }

}
