import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { ChartsModule } from 'ng2-charts/ng2-charts';

import { SharedModule } from '../shared/shared.module';
import { DashboardComponent } from './dashboard/dashboard.component';

@NgModule({
  imports: [
    SharedModule,
    ReactiveFormsModule,
    ChartsModule
  ],
  declarations: [
    DashboardComponent
  ],
  exports: [ChartsModule]
})
export class DashboardModule { }
