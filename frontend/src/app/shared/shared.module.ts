import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { NgSelectModule } from '@ng-select/ng-select';

import { LoadingScreenComponent } from './widgets/loading-screen/loading-screen.component';
import { SpinnerComponent } from './widgets/spinner/spinner.component';
import { SearchFormComponent } from './widgets/search-form/search-form.component';
import { SizesLabelsComponent } from './widgets/sizes-labels/sizes-labels.component';
import { OptionsComponent } from './widgets/options/options.component';
import { PopoverComponent } from './widgets/popover/popover.component';
import { PopoverDirective } from './directives/popover.directive';
import { ClosePopoverOnClickOutsideDirective } from './directives/close-popover-on-click-outside.directive';
import { DashIfEmptyPipe } from './pipes/dash-if-empty.pipe';
import { PsaCurrencyPipe } from './pipes/psa-currency.pipe';
import { FormatDatePipe } from './pipes/format-data.pipe';
import { DatatableHeaderComponent } from './widgets/datatable/datatable-header/datatable-header.component';
import { DatetimepickerComponent } from './widgets/datetimepicker/datetimepicker.component';
import { CurrencyInputComponent } from './widgets/currency-input/currency-input.component';
import { DropdownComponent } from './widgets/dropdown/dropdown.component';
import { NumberFieldDirective } from './directives/number-field.directive';
import { ConfirmationComponent } from './widgets/confirmation/confirmation.component';
import { ClickOutsideDirective } from './directives/click-outside.directive';
import { BackArrowComponent } from './widgets/back-arrow/back-arrow.component';
import { DatatableFooterComponent } from './widgets/datatable/datatable-footer/datatable-footer.component';
import { DataTablePagerComponent } from './widgets/datatable/datatable-pager/datatable-pager.component';
import { SelectSizeComponent } from './widgets/select-size/select-size.component';
import { ToggleComponent } from './widgets/toggle/toggle.component';
import { CheckboxComponent } from './widgets/checkbox/checkbox.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    NgxDatatableModule,
    CurrencyMaskModule,
    NgSelectModule,
    NgbModule.forRoot(),
    // NgxMyDatePickerModule.forRoot()
  ],
  declarations: [
    LoadingScreenComponent,
    SpinnerComponent,
    SearchFormComponent,
    SizesLabelsComponent,
    OptionsComponent,
    PopoverComponent,
    PopoverDirective,
    ClosePopoverOnClickOutsideDirective,
    NumberFieldDirective,
    ClickOutsideDirective,
    DashIfEmptyPipe,
    PsaCurrencyPipe,
    FormatDatePipe,
    DatatableHeaderComponent,
    DatetimepickerComponent,
    CurrencyInputComponent,
    DropdownComponent,
    ConfirmationComponent,
    BackArrowComponent,
    DatatableFooterComponent,
    DataTablePagerComponent,
    SelectSizeComponent,
    ToggleComponent,
    CheckboxComponent
  ],
  exports: [
    /* Common modules */
    NgbModule,
    CommonModule,
    FormsModule,
    RouterModule,
    NgxDatatableModule,
    /* Directives */
    PopoverDirective,
    ClosePopoverOnClickOutsideDirective,
    NumberFieldDirective,
    ClickOutsideDirective,
    /* Pipes */
    DashIfEmptyPipe,
    PsaCurrencyPipe,
    FormatDatePipe,
    /* Components */
    LoadingScreenComponent,
    SpinnerComponent,
    SearchFormComponent,
    SizesLabelsComponent,
    OptionsComponent,
    DatatableHeaderComponent,
    DatetimepickerComponent,
    CurrencyInputComponent,
    DropdownComponent,
    ConfirmationComponent,
    BackArrowComponent,
    DatatableFooterComponent,
    DataTablePagerComponent,
    SelectSizeComponent,
    ToggleComponent,
    CheckboxComponent
  ]
})

export class SharedModule {
}
