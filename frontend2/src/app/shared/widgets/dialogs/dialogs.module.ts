import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';

import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';

@NgModule({
  imports: [
    CommonModule,
    TranslateModule
  ],
  declarations: [
    ConfirmDialogComponent
  ],
  exports: [
  ],
  entryComponents: [
    ConfirmDialogComponent
  ]
})
export class DialogsModule {
}
