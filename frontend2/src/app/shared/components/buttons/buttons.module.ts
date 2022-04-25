import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';

import { ToggleFilterButtonComponent } from './toggle-filter-button/toggle-filter-button.component';
import { IconButtonComponent } from './icon-button/icon-button.component';
import { IconModule } from '../icon/icon.module';

@NgModule({
  declarations: [
    ToggleFilterButtonComponent,
    IconButtonComponent
  ],
  imports: [
    CommonModule,
    TranslateModule,
    IconModule
  ],
  exports: [
    ToggleFilterButtonComponent,
    IconButtonComponent
  ]
})
export class ButtonsModule {}
