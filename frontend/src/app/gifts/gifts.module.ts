import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from '../shared/shared.module';
import { GiftsListComponent } from './gifts-list/gifts-list.component';

@NgModule({
  imports: [
    SharedModule,
    ReactiveFormsModule
  ],
  declarations: [
    GiftsListComponent
  ],
  exports: []
})
export class GiftsModule { }
