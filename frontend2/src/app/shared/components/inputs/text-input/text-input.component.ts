import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { AbstractControl, FormControl } from '@angular/forms';
import { untilDestroyed } from 'ngx-take-until-destroy';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'mshk-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.scss']
})
export class TextInputComponent implements OnInit, OnChanges, OnDestroy {
  @Input() value: string;
  @Input() control = new FormControl();
  @Input() label: string;
  @Input() placeholder = '';
  @Input() readonly = false;
  @Input() withDelay = false;
  @Input() isLoading = false;
  @Input() debounceTime = 250;
  @Input() errors?: { [key: string]: string };
  @Output() onChange = new EventEmitter<string>();

  isFocused = false;

  get hasError(): boolean {
    return !!this.control && this.control.invalid && (this.control.dirty || this.control.touched);
  }

  constructor() { }

  ngOnInit(): void {
    this.subscribeToChanges();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.value) {
      this.control.setValue(changes.value.currentValue);
    }
  }

  ngOnDestroy(): void {
  }

  onFocus(): void {
    this.isFocused = true;
  }

  onBlur(): void {
    this.isFocused = false;
  }

  private hasRequiredValidator(): void {
    if (!!this.control.validator) {
      const validator = this.control.validator({} as AbstractControl);
      const required = !!(validator && validator.required);
      const min = !!(validator && validator.min);
      const max = !!(validator && validator.max);

      // const t1 = this.hasValidator('max');
      // const t2 = this.hasValidator('required');

      debugger
    }
  }

  public hasValidator(validator: string): boolean {
    return !!this.control.validator({} as AbstractControl).hasOwnProperty(validator);
   // returns true if control has the validator
  }

  private subscribeToChanges(): void {
    this.control.valueChanges
      .pipe(
        debounceTime(this.withDelay ? this.debounceTime : 0),
        distinctUntilChanged(),
        untilDestroyed(this)
      )
      .subscribe(value => {
        this.onChange.emit(value);

        
        this.hasRequiredValidator();
      });
  }
}
