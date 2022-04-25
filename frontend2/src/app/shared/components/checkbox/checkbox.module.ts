import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';

import { CheckboxComponent } from './checkbox.component';


@NgModule({
  declarations: [
    CheckboxComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TranslateModule
  ],
  exports: [
    CheckboxComponent
  ]
})
export class CheckboxModule {}
