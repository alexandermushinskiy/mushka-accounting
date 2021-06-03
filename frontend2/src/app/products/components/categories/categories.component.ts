import { Component, OnInit, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { mergeMap } from 'rxjs/operators';
import { untilDestroyed } from 'ngx-take-until-destroy';

import { Category } from '../../../shared/models/category.model';
import { NotificationsService } from '../../../core/notifications/notifications.service';
import { ApiCategoriesService } from '../../../api/categories/services/api-cateries.service';
import { DialogsService } from '../../../shared/components/dialogs/services/dialogs.service';
import { LanguageService } from '../../../core/language/language.service';
import { I18N } from '../../constants/i18n.const';
import {
  CategoryEditorDialogResult
} from '../../../shared/components/dialogs/components/category-editor-dialog/interfaces/category-editor-dialog-result.interface';
import { ProductsFacadeService } from '../../services/products-facade.service';

@Component({
  selector: 'mshk-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.scss']
})
export class CategoriesComponent implements OnInit, OnDestroy {
  @Output() onEditCategory = new EventEmitter<Category>();
  @Output() onDeleteCategory = new EventEmitter<string>();

  selectedCategory: Category;
  categories: Category[];
  keywords = '';
  isSaving = false;

  get filteredCategories(): Category[] {
    return this.keywords && this.keywords.length > 0
      ? this.categories.filter(view => view.name.toLowerCase().includes(this.keywords.toLowerCase()))
      : this.categories;
  }

  constructor(private dialogsService: DialogsService,
              private languageService: LanguageService,
              private apiCategoriesService: ApiCategoriesService,
              private notificationsService: NotificationsService,
              private productsFacadeService: ProductsFacadeService) {
  }

  ngOnInit(): void {
    this.productsFacadeService.getSelectedCategory$()
      .pipe(untilDestroyed(this))
      .subscribe((category) => this.selectedCategory = category);

    this.productsFacadeService.getCategoriesItems$()
      .pipe(untilDestroyed(this))
      .subscribe(categories => this.categories = categories);

    this.productsFacadeService.fetchCategories();
  }

  ngOnDestroy(): void {
  }

  createCategory(): void {
    const dialog = this.dialogsService.openCategoryEditorDialog({});

    dialog.confirm$
      .pipe(untilDestroyed(this))
      .subscribe(({ category }: CategoryEditorDialogResult) => {
        dialog.isLoading = true;
        this.apiCategoriesService.createCategory$(category)
          .subscribe(
            () => {
              dialog.isLoading = false;
              dialog.close();
              this.onSavedSucces(category.name, true);
            },
            () => this.onSaveError()
          );
      });
  }

  editCategory(category: Category): void {
    this.selectedCategory = {...category};
    const dialog = this.dialogsService.openCategoryEditorDialog({ category });

    dialog.confirm$
      .pipe(untilDestroyed(this))
      .subscribe((result: CategoryEditorDialogResult) => {
        dialog.isLoading = true;
        this.apiCategoriesService.updateCategory$(result.category.id, result.category)
          .subscribe(
            () => {
              dialog.isLoading = false;
              dialog.close();
              this.onSavedSucces(result.category.name, false);
            },
            () => this.onSaveError()
          );
      });
  }

  delete(category: Category): void {
    const { title, message, cancelLabel, confirmLabel } = I18N.dialogs.deleteCategory;
    const dialog = this.dialogsService.openConfirmDialog({
      title,
      message: this.languageService.translate(message, { name: category.name }),
      cancelLabel,
      confirmLabel
    });

    dialog.confirm$
      .pipe(
        mergeMap(() => {
          dialog.isLoading = true;
          return this.apiCategoriesService.deleteCategory$(category.id);
        }),
        untilDestroyed(this)
      )
      .subscribe(
        () => {
          dialog.close();
          this.onCategotyDeleted(category.name);
        },
        () => this.onDeleteCategoryError()
      );
  }

  selectCategory(category: Category): void {
    this.productsFacadeService.setSelectedCategory(category);
  }

  onSearch(keywords: string): void {
    this.selectedCategory = null;
    this.keywords = keywords;
  }

  private onSavedSucces(categoryName: string, isAdding: boolean): void {
    this.productsFacadeService.fetchCategories();

    this.notificationsService.success(
      isAdding ? I18N.messages.categoryWasAdded : I18N.messages.categoryWasUpdated,
      { name: categoryName }
    );
  }

  private onSaveError(): void {
    this.isSaving = false;
    this.notificationsService.error(I18N.errors.saveCategoryError);
  }

  private onCategotyDeleted(categoryName: string): void {
    this.productsFacadeService.fetchCategories();

    this.notificationsService.success(I18N.messages.categoryDeleted, { name: categoryName });
  }

  private onDeleteCategoryError(): void {
    this.notificationsService.error(I18N.errors.deleteCategoryError);
  }
}
