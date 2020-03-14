import { Component, OnInit, Input, Output, EventEmitter, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

import { PaymentMethod } from '../../enums/payment-method.enum';

@Component({
  selector: 'mshk-select-payment-methods',
  templateUrl: './select-payment-methods.component.html',
  styleUrls: ['./select-payment-methods.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => SelectPaymentMethodsComponent),
    multi: true
  }]
})
export class SelectPaymentMethodsComponent implements OnInit, ControlValueAccessor {
  @Input() initialValue: string;
  @Input() required: boolean;
  @Input() disabled = false;
  @Input() defaultValue: string;
  @Input() placeholder = 'Выберите';
  @Output() onPaymentMethodSelected = new EventEmitter<PaymentMethod>();

  selectedPaymentMethod: { id: PaymentMethod, description: string };
  paymentMethods = [
    { id: PaymentMethod.CASH, description: 'paymentMethod.cash' },
    { id: PaymentMethod.TRANSFER_TO_CARD, description: 'paymentMethod.transferToCard' },
    { id: PaymentMethod.LIQPAY, description: 'paymentMethod.liqPay' },
    { id: PaymentMethod.SETTLEMENT_ACCOUNT, description: 'paymentMethod.settlementAccount' }
  ];

  constructor() { }

  ngOnInit() {
  }

  writeValue(paymentMethod: PaymentMethod): void {
    if (paymentMethod) {
      this.selectedPaymentMethod = this.paymentMethods.find(pm => pm.id === paymentMethod);
    }
  }

  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  registerOnTouched() {
  }

  setDisabledState(disabled: boolean) {
    this.disabled = disabled;
  }

  onMethodSelected(paymentMethod: { id: PaymentMethod, description: string }) {
    this.selectedPaymentMethod = paymentMethod;

    this.onChangeCallback(paymentMethod.id);
    this.onPaymentMethodSelected.emit(paymentMethod.id);
  }

  private onChangeCallback: any = () => {};
}
