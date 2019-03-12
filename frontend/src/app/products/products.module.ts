import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';

import { SharedModule } from '../shared/shared.module';
import { ProductsListComponent } from './products-list/products-list.component';
import { CategoriesNavComponent } from './categories-nav/categories-nav.component';
import { CategoryModalComponent } from './shared/widgets/category-modal/category-modal.component';
import { ProductModalComponent } from './shared/widgets/product-modal/product-modal.component';
import { QuantityLabelComponent } from './shared/widgets/quantity-label/quantity-label.component';
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
    CategoriesNavComponent,
    CategoryModalComponent,
    ProductModalComponent,
    QuantityLabelComponent,
    SelectCategoryComponent
  ],
  exports: [ProductsListComponent]
})
export class ProductsModule { }
