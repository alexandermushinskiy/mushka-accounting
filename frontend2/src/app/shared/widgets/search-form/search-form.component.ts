import { Component, OnInit, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { untilDestroyed } from 'ngx-take-until-destroy';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'mshk-search-form',
  templateUrl: './search-form.component.html',
  styleUrls: ['./search-form.component.scss']
})
export class SearchFormComponent implements OnInit, OnDestroy {
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
  }

  ngOnInit() {
    this.searchListener
      .pipe(
        untilDestroyed(this),
        debounceTime(200)
      )
      .subscribe((key: string) => this.onSearch.emit(key));
  }

  ngOnDestroy(): void {
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
