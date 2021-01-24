import { Component, OnInit, Output, EventEmitter, OnDestroy, Input, TemplateRef } from '@angular/core';
import { untilDestroyed } from 'ngx-take-until-destroy';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'mshk-datatable-filter-bar',
  templateUrl: './datatable-filter-bar.component.html',
  styleUrls: ['./datatable-filter-bar.component.scss']
})
export class DatatableFilterBarComponent implements OnInit, OnDestroy {
  @Input() searchValue: string;
  @Input() filtersTmpl: TemplateRef<any>;
  @Output() onClose = new EventEmitter();
  @Output() onSearch = new EventEmitter<string>();

  private search$ = new Subject<string>();

  constructor() { }

  ngOnInit(): void {
    this.search$.pipe(
      untilDestroyed(this),
      debounceTime(200))
        .subscribe((key: string) => this.onSearch.emit(key));
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
