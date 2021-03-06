import { Component, ViewChild, Output, Input, EventEmitter, ElementRef } from '@angular/core';

import { NotificationsService } from '../../core/notifications/notifications.service';

@Component({
  selector: 'mk-global-search',
  templateUrl: './global-search.component.html',
  styleUrls: ['./global-search.component.scss']
})
export class GlobalSearchComponent {
  @Input() isLoading = false;
  @Output() onSearch = new EventEmitter<string>();
  @ViewChild('searchField', { static: false }) searchField: ElementRef;

  constructor(private notificationsService: NotificationsService) {
  }

  handleSearch(value: string) {
    this.onSearch.emit(value);
  }

  clearInput() {
    this.searchField.nativeElement.value = '';
  }
}
