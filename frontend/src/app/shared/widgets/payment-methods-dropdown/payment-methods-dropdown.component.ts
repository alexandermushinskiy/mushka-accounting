import { Component, OnInit, Input, Output, EventEmitter, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

import { PaymentMethod } from '../../enums/payment-method.enum';

@Component({
  selector: 'mk-payment-methods-dropdown',
  templateUrl: './payment-methods-dropdown.component.html',
  styleUrls: ['./payment-methods-dropdown.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => PaymentMethodsDropdownComponent),
    multi: true
  }]
})
export class PaymentMethodsDropdownComponent implements OnInit, ControlValueAccessor {
  @Input() initialValue: string;
  @Input() required: boolean;
  @Input() isDisabled = false;
  @Input() defaultValue: string;
  @Input() placeholder = 'Выберите';
  @Output() onPaymentMethodSelected = new EventEmitter<PaymentMethod>();

  selectedPaymentMethod: { id: PaymentMethod, description: string };
  paymentMethods = [
    { id: PaymentMethod.CASH, description: 'Наличный расчет' },
    { id: PaymentMethod.TRANSFER_TO_CARD, description: 'Перевод на карту' },
    { id: PaymentMethod.LIQPAY, description: 'LiqPay' },
    { id: PaymentMethod.SETTLEMENT_ACCOUNT, description: 'Расчетный счет' }
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

  setDisabledState(isDisabled: boolean) {
    this.isDisabled = isDisabled;
  }

  onMethodSelected(paymentMethod: { id: PaymentMethod, description: string }) {
    this.selectedPaymentMethod = paymentMethod;

    this.onChangeCallback(paymentMethod.id);
    this.onPaymentMethodSelected.emit(paymentMethod.id);
  }

  private onChangeCallback: any = () => {};
}
