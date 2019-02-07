import { Component, OnInit, Input, Output, EventEmitter, ViewChild, ElementRef, forwardRef } from '@angular/core';
import { Subject } from 'rxjs/Subject';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

import { UnsubscriberComponent } from '../../hooks/unsubscriber.component';

@Component({
  selector: 'mk-delayed-input',
  templateUrl: './delayed-input.component.html',
  styleUrls: ['./delayed-input.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => DelayedInputComponent),
    multi: true
  }]
})
export class DelayedInputComponent extends UnsubscriberComponent implements OnInit, ControlValueAccessor {
  @Input() type = 'number';
  @Input() disabled = false;
  @Input() required = false;
  @Input() placeholder = '';
  @Input() value = '';
  @Output() onInput = new EventEmitter<string>();
  @ViewChild('inputBox') inputElementRef: ElementRef;

  private inputTerms$ = new Subject<string>();
  
  ngOnInit() {
    this.inputTerms$
      .debounceTime(300)
      .distinctUntilChanged()
      .takeUntil(this.ngUnsubscribe$)
      .subscribe((val: string) => this.onInput.emit(val));
  }

  valueChanged(value: string) {
    this.onChangeCallback(value);
    this.inputTerms$.next(value);
  }

  reset() {
    this.inputElementRef.nativeElement.value = '';
    this.valueChanged('');
  }
  
  writeValue(value: any): void {
    this.value = value;
  }

  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  setDisabledState(isDisabled: boolean) {
    this.disabled = isDisabled;
  }

  registerOnTouched() {
  }
  
  private onChangeCallback: any = () => {};
}
