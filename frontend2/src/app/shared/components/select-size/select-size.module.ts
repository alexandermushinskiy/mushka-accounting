import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SelectSizeComponent } from './select-size.component';
import { SharedModule } from '../../shared.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TranslateModule,
    NgSelectModule,

    SharedModule // mshk-checkbox
  ],
  declarations: [
    SelectSizeComponent
  ],
  exports: [
    SelectSizeComponent
  ]
})
export class SelectSizeModule {
}
