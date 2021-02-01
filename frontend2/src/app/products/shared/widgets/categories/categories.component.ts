import { Component, OnInit, Input, Output, EventEmitter, ElementRef, ViewChild } from '@angular/core';
import { NgbModalRef, NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { Subject } from 'rxjs';
import { debounceTime, takeUntil } from 'rxjs/operators';

import { UnsubscriberComponent } from '../../../../shared/hooks/unsubscriber.component';
import { Category } from '../../../../shared/models/category.model';
import { NotificationsService } from '../../../../core/notifications/notifications.service';
import { ApiCategoriesService } from '../../../../api/categories/services/api-cateries.service';
import { ItemsList } from '../../../../shared/interfaces/items-list.interface';

@Component({
  selector: 'mshk-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.scss']
})
export class CategoriesComponent extends UnsubscriberComponent implements OnInit {
  @ViewChild('confirmRemoveCategoryTmpl', { static: false }) confirmRemoveCategoryTmpl: ElementRef;
  @Input() selectedCategory: Category;
  @Output() onCategorySelected = new EventEmitter<Category>();
  @Output() onEditCategory = new EventEmitter<Category>();
  @Output() onDeleteCategory = new EventEmitter<string>();

  categories: Category[];
  categoryToDelete: Category;
  keywords = '';
  isSaving = false;
  isDeleting = false;
  private delayedLoad$ = new Subject<string>();

  private modalRef: NgbModalRef;
  private readonly modalConfig: NgbModalOptions = {
    windowClass: 'category-modal',
    backdrop: 'static',
    size: 'sm'
  };

  get filteredCategories(): Category[] {
    return this.keywords.length > 0
      ? this.categories.filter(view => view.name.toLowerCase().includes(this.keywords.toLowerCase()))
      : this.categories;
  }

  constructor(private modalService: NgbModal,
              private apiCategoriesService: ApiCategoriesService,
              private notificationsService: NotificationsService) {
    super();
  }

  ngOnInit() {
    this.loadCategories();

    this.delayedLoad$.pipe(
      debounceTime(100),
      takeUntil(this.ngUnsubscribe$))
        .subscribe((res) => this.keywords = res);
  }

  create(content) {
    this.selectedCategory = null;
    this.modalRef = this.modalService.open(content, this.modalConfig);
  }

  edit(category: Category, content) {
    this.selectedCategory = {...category};
    this.modalRef = this.modalService.open(content, this.modalConfig);
  }

  delete(category: Category) {
    this.modalRef = this.modalService.open(this.confirmRemoveCategoryTmpl);
    this.categoryToDelete = category;
  }

  confirmDelete() {
    this.isDeleting = true;
    this.apiCategoriesService.deleteCategory$(this.categoryToDelete.id)
      .subscribe(
        () => this.onCategotyDeleted(),
        () => this.onDeleteCategoryError()
      );
  }

  selectCategory(category: Category) {
    this.selectedCategory = category;
    this.onCategorySelected.emit(category);
  }

  saveCategory(category: Category) {
    this.isSaving = true;

    (category.id
        ? this.apiCategoriesService.updateCategory$(category.id, category)
        : this.apiCategoriesService.createCategory$(category))
      .subscribe(
        () => this.onSavedSucces(category.name, !!category.id),
        () => this.onSaveError(),
      );
  }

  onSearch(keywords: string) {
    this.selectedCategory = null;
    this.delayedLoad$.next(keywords);
  }

  closeModal() {
    this.modalRef.close();
  }

  private loadCategories() {
    this.apiCategoriesService.searchCategories$()
      .subscribe((categories: ItemsList<Category>) => {
        this.categories = categories.items;
        if (categories.length > 0) {
          this.selectCategory(categories.items[0]);
        }
      });
  }

  private onSavedSucces(categoryName: string, isAdding: boolean) {
    this.loadCategories();
    this.closeModal();

    this.isSaving = false;
    this.notificationsService.success(isAdding ? 'products.categoryWasUpdated' : 'products.categoryWasAdded', { name: categoryName });
  }

  private onSaveError() {
    this.closeModal();

    this.isSaving = false;
    this.notificationsService.error('products.saveCategoryError');
  }

  private onCategotyDeleted() {
    this.closeModal();
    this.loadCategories();

    this.isDeleting = false;
    this.notificationsService.success('products.categoryWasDeleted', { name: this.categoryToDelete.name });
  }

  private onDeleteCategoryError() {
    this.closeModal();

    this.isDeleting = false;
    this.notificationsService.error('products.deleteCategoryError');
  }
}
