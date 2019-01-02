import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from '../shared/shared.module';
import { ProductsListComponent } from './products-list/products-list.component';
import { CategoriesNavComponent } from './categories-nav/categories-nav.component';
import { CategoryModalComponent } from './shared/widgets/category-modal/category-modal.component';
import { ProductModalComponent } from './shared/widgets/product-modal/product-modal.component';
import { SizesHelperServices } from './shared/services/sizes-helper.service';
import { QuantityLabelComponent } from './shared/widgets/quantity-label/quantity-label.component';

@NgModule({
  imports: [
    SharedModule,
    ReactiveFormsModule
  ],
  providers: [
    SizesHelperServices
  ],
  declarations: [
    ProductsListComponent,
    CategoriesNavComponent,
    CategoryModalComponent,
    ProductModalComponent,
    QuantityLabelComponent
  ],
  exports: [ProductsListComponent]
})
export class ProductsModule { }
