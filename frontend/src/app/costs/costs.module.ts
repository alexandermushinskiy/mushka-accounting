import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from '../shared/shared.module';
import { CostsListComponent } from './costs-list/costs-list.component';

@NgModule({
  imports: [
    SharedModule,
    ReactiveFormsModule
  ],
  declarations: [
    CostsListComponent
  ],
  exports: []
})
export class CostsModule { }
