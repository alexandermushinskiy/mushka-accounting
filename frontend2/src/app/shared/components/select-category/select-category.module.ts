import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SelectCategoryComponent } from './select-category.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TranslateModule,
    NgSelectModule
  ],
  declarations: [
    SelectCategoryComponent
  ],
  exports: [
    SelectCategoryComponent
  ]
})
export class SelectCategoryModule {
}
