import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'mshk-global-search',
  templateUrl: './global-search.component.html',
  styleUrls: ['./global-search.component.scss']
})
export class GlobalSearchComponent implements OnInit {
  @Input() isLoading = false;
  @Output() onSearch = new EventEmitter<string>();

  constructor() { }

  ngOnInit() {
  }

  search(value: string) {
    this.onSearch.emit(value);
  }
}
