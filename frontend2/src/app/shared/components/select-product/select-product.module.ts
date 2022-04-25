import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TooltipModule } from 'ngx-bootstrap';

import { SelectProductComponent } from './select-product.component';
import { CheckboxModule } from '../checkbox/checkbox.module';
import { IconModule } from '../icon/icon.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TranslateModule,
    NgSelectModule,
    CheckboxModule,
    IconModule,
    TooltipModule.forRoot(),
  ],
  declarations: [
    SelectProductComponent
  ],
  exports: [
    SelectProductComponent
  ]
})
export class SelectProductModule {
}
