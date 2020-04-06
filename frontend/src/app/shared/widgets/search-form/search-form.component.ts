import { Component, EventEmitter, Input, OnInit, Output, ViewChild, ElementRef } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil, debounceTime } from 'rxjs/operators';

import { UnsubscriberComponent } from '../../hooks/unsubscriber.component';

@Component({
  selector: 'mk-search-form',
  templateUrl: './search-form.component.html',
  styleUrls: ['./search-form.component.scss']
})
export class SearchFormComponent extends UnsubscriberComponent implements OnInit {
  @ViewChild('searchBox', { static: false }) searchElementRef: ElementRef;
  @Input() type: 'solid' | 'outline' = 'outline';
  @Input() disabled = false;
  @Input() size: 'default' | 'large' = 'default';
  @Input() placeholder = 'Поиск...';
  @Input() defaultValue = '';
  @Output() onSearch = new EventEmitter();

  private searchTerms$ = new Subject<string>();

  ngOnInit() {
    this.searchTerms$.pipe(
      takeUntil(this.ngUnsubscribe$),
      debounceTime(300))
        .subscribe((val: string) => this.onSearch.emit(val));
  }

  valueChanged(value) {
    this.searchTerms$.next(value);
  }

  search() {
    return this.searchTerms$.asObservable();
  }

  reset() {
    this.searchElementRef.nativeElement.value = '';
    this.valueChanged('');
  }
}
