import { Component, OnInit, Input, Output, EventEmitter, ViewChild, ElementRef } from '@angular/core';
import { Subject } from 'rxjs/Subject';

import { UnsubscriberComponent } from '../../hooks/unsubscriber.component';

@Component({
  selector: 'mk-delayed-input',
  templateUrl: './delayed-input.component.html',
  styleUrls: ['./delayed-input.component.scss']
})
export class DelayedInputComponent extends UnsubscriberComponent implements OnInit {
  @Input() type = 'number';
  @Input() disabled = false;
  @Input() placeholder = '';
  @Input() defaultValue = '';
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
    this.inputTerms$.next(value);
  }

  reset() {
    this.inputElementRef.nativeElement.value = '';
    this.valueChanged('');
  }
}
