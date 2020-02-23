import { Directive, HostListener } from '@angular/core';

@Directive({
  selector: 'input[mkNumberField]'
})
export class NumberFieldDirective {
  @HostListener('keydown', ['$event']) onKeyDown(event) {
    const e = <KeyboardEvent> event;
    // tslint:disable-next-line: deprecation
    if ([46, 8, 9, 27, 13, 110, 190].indexOf(e.keyCode) !== -1 ||
      // Allow: Ctrl+A
      // tslint:disable-next-line: deprecation
      (e.keyCode === 65 && (e.ctrlKey || e.metaKey)) ||
      // Allow: Ctrl+C
      // tslint:disable-next-line: deprecation
      (e.keyCode === 67 && (e.ctrlKey || e.metaKey)) ||
      // Allow: Ctrl+V
      // tslint:disable-next-line: deprecation
      (e.keyCode === 86 && (e.ctrlKey || e.metaKey)) ||
      // Allow: Ctrl+X
      // tslint:disable-next-line: deprecation
      (e.keyCode === 88 && (e.ctrlKey || e.metaKey)) ||
      // Allow: Shift+insert
      // tslint:disable-next-line: deprecation
      (e.keyCode === 45 && (e.shiftKey || e.metaKey)) ||
      // Allow: home, end, left, right
      // tslint:disable-next-line: deprecation
      (e.keyCode >= 35 && e.keyCode <= 39)) {
      // let it happen, don't do anything
      return;
    }
    // tslint:disable-next-line: deprecation
    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
      e.preventDefault();
    }
  }

  @HostListener('paste', ['$event']) blockPaste(event) {
    const pastedValue = event.clipboardData
      ? event.clipboardData.getData('text/plain')
      : window['clipboardData'].getData('text');

    if (!pastedValue || !pastedValue.match('^\\d+$')) {
      event.preventDefault();
    }
  }
}
