import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil, debounceTime } from 'rxjs/operators';

import { UnsubscriberComponent } from '../../hooks/unsubscriber.component';

@Component({
  selector: 'mshk-search-form',
  templateUrl: './search-form.component.html',
  styleUrls: ['./search-form.component.scss']
})
export class SearchFormComponent extends UnsubscriberComponent implements OnInit {
  @Input() placeholder = 'common.search';
  @Input() set defaultValue(value: string)  {
    this.searchValue = !!value ? value : '';
  }
  @Input() isInvalid = false;
  @Input() hasClear = true;
  @Output() onSearch: EventEmitter<string> = new EventEmitter<string>();
  searchValue: string;
  private searchListener: Subject<string> = new Subject<string>();

  constructor() {
    super();
  }

  ngOnInit() {
    this.searchListener.pipe(
      takeUntil(this.ngUnsubscribe$),
      debounceTime(200))
        .subscribe((key: string) => this.onSearch.emit(key));
  }

  onValueChange(value: string) {
    this.searchValue = value;
    this.searchListener.next(value);
  }

  clear() {
    this.searchValue = null;
    this.onSearch.emit(this.searchValue);
  }
}
