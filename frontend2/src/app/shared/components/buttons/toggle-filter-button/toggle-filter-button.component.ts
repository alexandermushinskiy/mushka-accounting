import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'mshk-toggle-filter-button',
  templateUrl: './toggle-filter-button.component.html',
  styleUrls: ['./toggle-filter-button.component.scss']
})
export class ToggleFilterButtonComponent implements OnInit {
  @Input() hasActiveFilters = false;
  @Output() onToggle = new EventEmitter<void>();

  constructor() { }

  ngOnInit(): void {
  }

  toggleFilter(): void {
    this.onToggle.emit();
  }
}
