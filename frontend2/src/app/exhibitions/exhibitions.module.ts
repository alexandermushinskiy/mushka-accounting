import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from '../shared/shared.module';
import { ExhibitionsListComponent } from './exhibitions-list/exhibitions-list.component';
import { ExhibitionEditorComponent } from './exhibition-editor/exhibition-editor.component';
import { ExhibitionsComponent } from './exhibitions.component';

@NgModule({
  imports: [
    SharedModule,
    ReactiveFormsModule
  ],
  declarations: [
    ExhibitionsListComponent,
    ExhibitionEditorComponent,
    ExhibitionsComponent
  ],
  exports: [
    ExhibitionsComponent
  ]
})
export class ExhibitionsModule { }
