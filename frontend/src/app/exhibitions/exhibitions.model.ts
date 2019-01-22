import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from '../shared/shared.module';
import { ExhibitionsListComponent } from './exhibitions-list/exhibitions-list.component';

@NgModule({
  imports: [
    SharedModule,
    ReactiveFormsModule
  ],
  declarations: [
    ExhibitionsListComponent
  ],
  exports: []
})
export class ExhibitionsModule { }
