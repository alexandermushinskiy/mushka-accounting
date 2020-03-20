import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ModalModule } from 'ngx-bootstrap';
import { ToastrModule } from 'ngx-toastr';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { NgSelectModule } from '@ng-select/ng-select';
import { CurrencyMaskModule } from 'ng2-currency-mask';
// import { NgSelectModule } from '@ng-select/ng-select';
// import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
// import { NgxMaskModule } from 'ngx-mask';

import { LoadingScreenComponent } from './widgets/loading-screen/loading-screen.component';
import { SpinnerComponent } from './widgets/spinner/spinner.component';
import { DropdownComponent } from './widgets/dropdown/dropdown.component';
import { ConfirmLeaveComponent } from './modals/confirm-leave/confirm-leave.component';
import { HandleUnsavedDataGuard } from './guards/handle-unsaved-data.guard';
import { ConfirmationComponent } from './widgets/confirmation/confirmation.component';
import { DatatableFooterComponent } from './widgets/datatable-footer/datatable-footer.component';
import { DatatablePagerComponent } from './widgets/datatable-pager/datatable-pager.component';
import { HttpClient } from '@angular/common/http';
import { SearchFormComponent } from './widgets/search-form/search-form.component';
import { ProgressLinearComponent } from './widgets/progress-linear/progress-linear.component';
import { BackArrowComponent } from './widgets/back-arrow/back-arrow.component';
// import { SearchFormComponent } from './widgets/search-form/search-form.component';
// import { OptionsComponent } from './widgets/options/options.component';
// import { PopoverComponent } from './widgets/popover/popover.component';
// import { PopoverDirective } from './directives/popover.directive';
// import { ClosePopoverOnClickOutsideDirective } from './directives/close-popover-on-click-outside.directive';
// import { DashIfEmptyPipe } from './pipes/dash-if-empty.pipe';
import { CurrencyPipe } from './pipes/currency.pipe';
import { DatepickerComponent } from './widgets/datepicker/datepicker.component';
import { DatetimepickerComponent } from './widgets/datetimepicker/datetimepicker.component';
import { DatetimepickerWrapperComponent } from './widgets/datetimepicker-wrapper/datetimepicker-wrapper.component';
import { DelayedInputComponent } from './widgets/delayed-input/delayed-input.component';
import { SelectProductsComponent } from './widgets/select-products/select-products.component';
import { CurrencyInputComponent } from './widgets/currency-input/currency-input.component';
import { CheckboxComponent } from './widgets/checkbox/checkbox.component';
import { SelectPaymentMethodsComponent } from './widgets/select-payment-methods/select-payment-methods.component';
import { SelectTimeframesComponent } from './widgets/select-timeframes/select-timeframes.component';
import { DateRangeModalComponent } from './modals/date-range-modal/date-range-modal.component';
import { FormatPipe } from './pipes/format.pipe';
import { DatatableFilterBarComponent } from './widgets/datatable-filter-bar/datatable-filter-bar.component';
// import { DatatableHeaderComponent } from './widgets/datatable/datatable-header/datatable-header.component';

// import { DropdownComponent } from './widgets/dropdown/dropdown.component';
// import { NumberFieldDirective } from './directives/number-field.directive';
// import { ClickOutsideDirective } from './directives/click-outside.directive';
// import { BackArrowComponent } from './widgets/back-arrow/back-arrow.component';
// import { DatatableFooterComponent } from './widgets/datatable/datatable-footer/datatable-footer.component';
// import { DataTablePagerComponent } from './widgets/datatable/datatable-pager/datatable-pager.component';
// import { SelectSizeComponent } from './widgets/select-size/select-size.component';
// import { ToggleComponent } from './widgets/toggle/toggle.component';
// import { NumberPipe } from './pipes/number.pipe';
// import { DropdownSizesComponent } from './widgets/dropdown-sizes/dropdown-sizes.component';
// import { SuppliersDropdownComponent } from './widgets/suppliers-dropdown/suppliers-dropdown.component';
// import { OrderQuickFilter } from './filters/order-quick.filter';
// import { SupplyQuickFilter } from './filters/supply-quick.filter';

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

@NgModule({
  entryComponents: [
    ConfirmLeaveComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModule,
    BrowserAnimationsModule,
    NgxDatatableModule,
    NgSelectModule,
    CurrencyMaskModule,
    ToastrModule.forRoot(),
    ModalModule.forRoot(),
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    })
    // NgSelectModule,
    // TypeaheadModule.forRoot(),
    // NgxMaskModule.forRoot(),
    // NgxMyDatePickerModule.forRoot()
  ],
  declarations: [
    LoadingScreenComponent,
    SpinnerComponent,
    DropdownComponent,
    ConfirmLeaveComponent,
    ConfirmationComponent,
    DatatableFooterComponent,
    DatatablePagerComponent,
    SearchFormComponent,
    ProgressLinearComponent,
    BackArrowComponent,
    // OptionsComponent,
    // PopoverComponent,
    // PopoverDirective,
    // ClosePopoverOnClickOutsideDirective,
    // NumberFieldDirective,
    // ClickOutsideDirective,
    // DashIfEmptyPipe,
    CurrencyPipe,
    DatepickerComponent,
    DatetimepickerComponent,
    DatetimepickerWrapperComponent,
    DelayedInputComponent,
    SelectProductsComponent,
    CurrencyInputComponent,
    CheckboxComponent,
    SelectPaymentMethodsComponent,
    SelectTimeframesComponent,
    DateRangeModalComponent,
    FormatPipe,
    DatatableFilterBarComponent,
    // NumberPipe,
    // DatatableHeaderComponent,
    // SelectSizeComponent,
    // ToggleComponent,
    // DropdownSizesComponent,
    // SuppliersDropdownComponent
  ],
  exports: [
    /* Common modules */
    NgbModule,
    CommonModule,
    FormsModule,
    RouterModule,
    ToastrModule,
    NgxDatatableModule,
    TranslateModule,
    NgSelectModule,
    // NgxMaskModule,

    /* Directives */
    // PopoverDirective,
    // ClosePopoverOnClickOutsideDirective,
    // NumberFieldDirective,
    // ClickOutsideDirective,

    /* Pipes */
    // DashIfEmptyPipe,
    CurrencyPipe,
    FormatPipe,
    // NumberPipe,

    /* Components */
    LoadingScreenComponent,
    SpinnerComponent,
    DropdownComponent,
    ConfirmationComponent,
    DatatableFooterComponent,
    DatatablePagerComponent,
    SearchFormComponent,
    ProgressLinearComponent,
    BackArrowComponent,
    DatepickerComponent,
    // OptionsComponent,
    // DatatableHeaderComponent,
    DatetimepickerComponent,
    DatetimepickerWrapperComponent,
    CurrencyInputComponent,
    // SelectSizeComponent,
    // ToggleComponent,
    CheckboxComponent,
    // DropdownSizesComponent,
    // SuppliersDropdownComponent,
    SelectProductsComponent,
    SelectPaymentMethodsComponent,
    SelectTimeframesComponent,
    DelayedInputComponent,
    DateRangeModalComponent,
    ConfirmLeaveComponent,
    DatatableFilterBarComponent
  ],
  providers: [
    // OrderQuickFilter,
    // SupplyQuickFilter,
    HandleUnsavedDataGuard
  ]
})

export class SharedModule {
}
