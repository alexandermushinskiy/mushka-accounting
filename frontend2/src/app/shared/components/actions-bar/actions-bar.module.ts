import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ActionsBarComponent } from './actions-bar.component';
import { ActionsBarPrimaryAreaComponent } from './components/actions-bar-primary-area/actions-bar-primary-area.component';
import { ActionsBarSecondaryAreaComponent } from './components/actions-bar-secondary-area/actions-bar-secondary-area.component';

@NgModule({
  declarations: [
    ActionsBarComponent,
    ActionsBarPrimaryAreaComponent,
    ActionsBarSecondaryAreaComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    ActionsBarComponent,
    ActionsBarPrimaryAreaComponent,
    ActionsBarSecondaryAreaComponent
  ]
})
export class ActionsBarModule {}
