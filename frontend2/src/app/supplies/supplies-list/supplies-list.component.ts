import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { NgbModalRef, NgbModalOptions, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { SupplyList } from '../../shared/models/supply-list.model';
import { SuppliesService } from '../../core/api/supplies.service';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { ProductsServce } from '../../core/api/products.service';
import { SelectProduct } from '../../shared/models/select-product.model';
import { ItemsWithTotalCount } from '../../shared/models/items-with-total-count.model';

@Component({
  selector: 'mshk-supplies-list',
  templateUrl: './supplies-list.component.html',
  styleUrls: ['./supplies-list.component.scss']
})
export class SuppliesListComponent implements OnInit {
  @ViewChild('confirmRemoveTmpl', { static: false }) confirmRemoveTmpl: ElementRef;
  supplies: SupplyList[];
  shownSupplies: SupplyList[];
  loadingIndicator = false;
  isProductsLoading = false;
  total = 0;
  shown = 0;
  supplyToDelete: SupplyList;
  searchKey: string;
  selectedProduct: SelectProduct;
  productsList: SelectProduct[];
  sorts = [
    { prop: 'supplierName', dir: 'asc' },
    { prop: 'receivedDate', dir: null },
    { prop: 'description', dir: null }
  ];

  private modalRef: NgbModalRef;
  private readonly modalConfig: NgbModalOptions = {
    windowClass: 'supply-modal',
    backdrop: 'static',
    size: 'sm'
  };

  constructor(private modalService: NgbModal,
              private suppliesService: SuppliesService,
              private productsService: ProductsServce,
              private notificationsService: NotificationsService) {
  }

  ngOnInit() {
    this.loadProducts();
    this.loadSupplies();
  }

  onActive(event: any) {
    if (event.type === 'click') {
      event.cellElement.blur();
    }
  }

  onSearch(searchKey: string) {
    this.searchKey = searchKey;

    this.loadSupplies();
  }

  onProductSelected(product: SelectProduct) {
    this.selectedProduct = product;

    this.loadSupplies();
  }

  delete(supply: SupplyList) {
    setTimeout(() => {
      this.supplyToDelete = supply;
      this.modalRef = this.modalService.open(this.confirmRemoveTmpl, this.modalConfig);
    });
  }

  confirmDelete() {
    this.loadingIndicator = true;
    this.closeModal();

    this.suppliesService.delete(this.supplyToDelete.id)
      .subscribe(
        () => this.onDeleteSuccess(),
        (error: string) => this.onDeleteFailed(error)
      );
  }

  closeModal() {
    if (this.modalRef) {
      this.modalRef.close();
    }
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
    this.supplyToDelete = null;
    this.loadSupplies();
  }

  private onDeleteFailed(error: string) {
    this.loadingIndicator = false;
    this.supplyToDelete = null;
    this.notificationsService.error(`Ошибка при удалении поставки: ${error}.`);
  }

  private loadSupplies() {
    this.loadingIndicator = true;

    this.suppliesService.get(this.searchKey, !!this.selectedProduct ? this.selectedProduct.id : null)
      .subscribe(
        (res: ItemsWithTotalCount<SupplyList>) => this.onLoadSuccess(res),
        () => this.onLoadError()
      );
  }

  private onLoadSuccess(response: ItemsWithTotalCount<SupplyList>) {
    this.supplies = response.items;
    this.shownSupplies = response.items;

    this.total = response.totalCount;
    this.shown = response.items.length;

    this.loadingIndicator = false;
  }

  private onLoadError() {
    this.loadingIndicator = false;
    this.notificationsService.error('supplies.loadSuppliesError');
  }

}
