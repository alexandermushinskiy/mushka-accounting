import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { FilterBarComponent } from './filter-bar.component';

@NgModule({
  declarations: [
    FilterBarComponent
  ],
  imports: [
    CommonModule,
    FormsModule
  ],
  exports: [
    FilterBarComponent
  ]
})
export class FilterBarModule {}
