import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { IconModule } from '../icon/icon.module';
import { DatepickerComponent } from './datepicker.component';

@NgModule({
  declarations: [
    DatepickerComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NgbModule,
    IconModule
  ],
  exports: [
    DatepickerComponent
  ]
})
export class DatepickerModule {
}
