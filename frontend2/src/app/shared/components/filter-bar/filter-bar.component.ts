import { Component, OnInit, Output, EventEmitter, OnDestroy, Input, TemplateRef, ViewChild, ElementRef } from '@angular/core';
import { untilDestroyed } from 'ngx-take-until-destroy';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { I18N } from './constants/i18n.const';

@Component({
  selector: 'mshk-filter-bar',
  templateUrl: './filter-bar.component.html',
  styleUrls: ['./filter-bar.component.scss']
})
export class FilterBarComponent implements OnInit, OnDestroy {
  @ViewChild('searchInput', { static: true }) searchElementRef: ElementRef;
  @Input() searchValue: string;
  @Input() filtersTmpl: TemplateRef<any>;
  @Output() onClose = new EventEmitter();
  @Output() onSearch = new EventEmitter<string>();

  readonly i18n = I18N;
  private search$ = new Subject<string>();

  constructor() { }

  ngOnInit(): void {
    this.search$.pipe(
      untilDestroyed(this),
      debounceTime(200))
        .subscribe((key: string) => this.onSearch.emit(key));

    this.searchElementRef.nativeElement.focus();
  }

  ngOnDestroy(): void {
  }

  close() {
    this.onClose.emit();
  }

  onSearchChanged(value: string): void {
    this.searchValue = value;
    this.search$.next(value);
  }
}
