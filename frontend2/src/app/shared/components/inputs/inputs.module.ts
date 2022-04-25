import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { NgSelectModule } from '@ng-select/ng-select';

import { SearchInputComponent } from './search-input/search-input.component';
import { TypeaheadInputComponent } from './typeahead-input/typeahead-input.component';
import { TextInputComponent } from './text-input/text-input.component';
import { DatepickerInputComponent } from './datepicker-input/datepicker-input.component';
import { IconModule } from '../icon/icon.module';
import { ListInputComponent } from './list-input/list-input.component';
import { CurrencyInput2Component } from './currency-input/currency-input.component';
import { SpinnerModule } from '../spinner/spinner.module';
import { DatepickerModule } from '../datepicker/datepicker.module';
import { TextareaInputComponent } from './textarea-input/textarea-input.component';

@NgModule({
  imports: [
    CommonModule,
    TranslateModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModule,
    IconModule,
    CurrencyMaskModule,
    SpinnerModule,
    NgSelectModule,
    TooltipModule.forRoot(),
    DatepickerModule
  ],
  declarations: [
    SearchInputComponent,
    TypeaheadInputComponent,
    TextInputComponent,
    DatepickerInputComponent,
    CurrencyInput2Component,
    ListInputComponent,
    TextareaInputComponent
  ],
  exports: [
    SearchInputComponent,
    TypeaheadInputComponent,
    TextInputComponent,
    DatepickerInputComponent,
    CurrencyInput2Component,
    ListInputComponent,
    TextareaInputComponent
  ]
})
export class InputsModule {
}
