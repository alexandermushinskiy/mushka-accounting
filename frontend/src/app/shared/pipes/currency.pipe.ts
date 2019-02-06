import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'mkCurrency'
})
export class CurrencyPipe implements PipeTransform {
  options = {
    allowNegative: true,
    decimal: '.',
    precision: 2,
    prefix: '',
    suffix: '',
    thousands: ' '
  };

  transform(value: string): string {
    return this.applyMask(value);
  }

  private applyMask(rawValue: string): string {
    const { allowNegative, decimal, precision, prefix, suffix, thousands } = this.options;

    rawValue = new Number(rawValue).toFixed(precision);
    const onlyNumbers = rawValue.replace(/[^0-9]/g, '');

    if (!onlyNumbers) {
        return '';
    }

    let integerPart = onlyNumbers.slice(0, onlyNumbers.length - precision).replace(/^0*/g, '').replace(/\B(?=(\d{3})+(?!\d))/g, thousands);

    if (integerPart === '') {
        integerPart = '0';
    }

    let newRawValue = integerPart;
    let decimalPart = onlyNumbers.slice(onlyNumbers.length - precision);

    if (precision > 0) {
        decimalPart = '0'.repeat(precision - decimalPart.length) + decimalPart;
        newRawValue += decimal + decimalPart;
    }

    const isZero = parseInt(integerPart, 10) === 0 && (parseInt(decimalPart, 10) === 0 || decimalPart === '');
    const operator = (rawValue.indexOf('-') > -1 && allowNegative && !isZero) ? '-' : '';
    return operator + prefix + newRawValue + suffix;
  }
}
