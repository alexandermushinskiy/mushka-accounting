import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';

import { SharedModule } from '../shared/shared.module';
import { ProductsListComponent } from './components/products-list/products-list.component';
import { CategoriesComponent } from './components/categories/categories.component';
import { QuantityLabelComponent } from './components/quantity-label/quantity-label.component';
import { ProductsFilterComponent } from './components/products-filter/products-filter.component';
import { ProductsComponent } from './products.component';
import { SelectCategoryModule } from '../shared/components/select-category/select-category.module';

@NgModule({
  imports: [
    SharedModule,
    ReactiveFormsModule,
    NgSelectModule,
    SelectCategoryModule
  ],
  declarations: [
    ProductsComponent,
    ProductsListComponent,
    CategoriesComponent,
    QuantityLabelComponent,
    ProductsFilterComponent
  ],
  exports: [ProductsComponent]
})
export class ProductsModule { }
