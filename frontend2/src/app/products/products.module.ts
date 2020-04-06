import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';

import { SharedModule } from '../shared/shared.module';
import { ProductsListComponent } from './products-list/products-list.component';
import { CategoriesComponent } from './shared/widgets/categories/categories.component';
import { CategoryModalComponent } from './shared/modals/category-modal/category-modal.component';
import { QuantityLabelComponent } from './shared/widgets/quantity-label/quantity-label.component';
import { ProductsFilterComponent } from './shared/widgets/products-filter/products-filter.component';
import { ProductModalComponent } from './shared/modals/product-modal/product-modal.component';
import { SelectCategoryComponent } from './shared/widgets/select-category/select-category.component';

@NgModule({
  imports: [
    SharedModule,
    ReactiveFormsModule,
    NgSelectModule
  ],
  providers: [
  ],
  declarations: [
    ProductsListComponent,
    CategoriesComponent,
    CategoryModalComponent,
    QuantityLabelComponent,
    ProductsFilterComponent,
    ProductModalComponent,
    SelectCategoryComponent
  ],
  exports: [ProductsListComponent]
})
export class ProductsModule { }
