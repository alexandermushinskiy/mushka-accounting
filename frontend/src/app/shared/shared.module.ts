import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { NgSelectModule } from '@ng-select/ng-select';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { NgxMaskModule } from 'ngx-mask';

import { LoadingScreenComponent } from './widgets/loading-screen/loading-screen.component';
import { SpinnerComponent } from './widgets/spinner/spinner.component';
import { SearchFormComponent } from './widgets/search-form/search-form.component';
import { SizeLabelComponent } from './widgets/size-label/size-label.component';
import { OptionsComponent } from './widgets/options/options.component';
import { PopoverComponent } from './widgets/popover/popover.component';
import { PopoverDirective } from './directives/popover.directive';
import { ClosePopoverOnClickOutsideDirective } from './directives/close-popover-on-click-outside.directive';
import { DashIfEmptyPipe } from './pipes/dash-if-empty.pipe';
import { CurrencyPipe } from './pipes/currency.pipe';
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
import { NumberPipe } from './pipes/number.pipe';
import { DatetimepickerWrapperComponent } from './widgets/datetimepicker-wrapper/datetimepicker-wrapper.component';
import { DropdownSizesComponent } from './widgets/dropdown-sizes/dropdown-sizes.component';
import { SuppliersDropdownComponent } from './widgets/suppliers-dropdown/suppliers-dropdown.component';
import { TypeaheadProductsComponent } from './widgets/typeahead-products/typeahead-products.component';
import { SelectProductsComponent } from './widgets/select-products/select-products.component';
import { ProgressLinearComponent } from './widgets/progress-linear/progress-linear.component';
import { PaymentMethodsDropdownComponent } from './widgets/payment-methods-dropdown/payment-methods-dropdown.component';

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
    TypeaheadModule.forRoot(),
    NgxMaskModule.forRoot()
    // NgxMyDatePickerModule.forRoot()
  ],
  declarations: [
    LoadingScreenComponent,
    SpinnerComponent,
    SearchFormComponent,
    SizeLabelComponent,
    OptionsComponent,
    PopoverComponent,
    PopoverDirective,
    ClosePopoverOnClickOutsideDirective,
    NumberFieldDirective,
    ClickOutsideDirective,
    DashIfEmptyPipe,
    CurrencyPipe,
    FormatDatePipe,
    NumberPipe,
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
    CheckboxComponent,
    DatetimepickerWrapperComponent,
    DropdownSizesComponent,
    SuppliersDropdownComponent,
    TypeaheadProductsComponent,
    SelectProductsComponent,
    ProgressLinearComponent,
    PaymentMethodsDropdownComponent
  ],
  exports: [
    /* Common modules */
    NgbModule,
    CommonModule,
    FormsModule,
    RouterModule,
    NgxDatatableModule,
    NgxMaskModule,
    /* Directives */
    PopoverDirective,
    ClosePopoverOnClickOutsideDirective,
    NumberFieldDirective,
    ClickOutsideDirective,
    /* Pipes */
    DashIfEmptyPipe,
    CurrencyPipe,
    FormatDatePipe,
    NumberPipe,
    /* Components */
    LoadingScreenComponent,
    SpinnerComponent,
    SearchFormComponent,
    SizeLabelComponent,
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
    CheckboxComponent,
    DatetimepickerWrapperComponent,
    DropdownSizesComponent,
    SuppliersDropdownComponent,
    TypeaheadProductsComponent,
    SelectProductsComponent,
    ProgressLinearComponent,
    PaymentMethodsDropdownComponent
  ],
  providers: [
  ]
})

export class SharedModule {
}
