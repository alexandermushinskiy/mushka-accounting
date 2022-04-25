import { Component, ElementRef, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Observable, of, OperatorFunction } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, filter, switchMap, tap } from 'rxjs/operators';

@Component({
  selector: 'mshk-typeahead-input',
  templateUrl: './typeahead-input.component.html',
  styleUrls: ['./typeahead-input.component.scss']
})
export class TypeaheadInputComponent implements OnInit {
  @Input() control = new FormControl();
  @Input() label: string;
  @Input() resultTmpl: ElementRef;
  @Input() source$: (term: string) => Observable<string[]>;
  @Input() errors?: { [key: string]: string };
  @Output() onSelect = new EventEmitter<any>();

  isLoading = false;
  searchFunc: OperatorFunction<string, readonly string[]>;

  private readonly debounceTime = 300;

  get hasError(): boolean {
    return !!this.control && this.control.invalid && (this.control.dirty || this.control.touched);
  }

  ngOnInit(): void {
    this.searchFunc = (text$: Observable<string>) => {
      return text$
        .pipe(
          debounceTime(this.debounceTime),
          distinctUntilChanged(),
          tap(() => this.isLoading = true),
          filter((term: string) => term.length >= 3),
          switchMap((term: string) =>
            this.source$(term).pipe(catchError(() => of([])))
          ),
          tap(() => this.isLoading = false)
      );
    };
  }

  selectItem($event: any): void {
    $event.preventDefault();
    this.onSelect.emit($event.item);
  }
}
