import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { TranslateModule } from '@ngx-translate/core';

import { TimeFrameComponent } from './time-frame/time-frame.component';

@NgModule({
  imports: [
    CommonModule,
    TranslateModule,
    NgbModule
  ],
  declarations: [
    TimeFrameComponent
  ],
  exports: [
    TimeFrameComponent
  ]
})
export class TimeFrameModule {
}
