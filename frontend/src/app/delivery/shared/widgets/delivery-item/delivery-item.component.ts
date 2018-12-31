import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { Supply } from '../../models/supply.model';

@Component({
  selector: 'mk-delivery-item',
  templateUrl: './delivery-item.component.html',
  styleUrls: ['./delivery-item.component.scss']
})
export class DeliveryItemComponent implements OnInit {
  @Input() delivery: Supply;
  @Input() canEdit = false;
  @Input() canDelete = false;
  @Output() onEdit = new EventEmitter<Supply>();
  @Output() onDelete = new EventEmitter<Supply>();
  @Output() onView = new EventEmitter<Supply>();

  expandedItemsStates: { [id: string]: boolean } = {};
  isExpanded = false;

  constructor() { }

  ngOnInit() {
  }

  toggleCollapseItemMode() {
    this.isExpanded = !this.isExpanded;
  }

  edit() {
    this.onEdit.emit(this.delivery);
  }

  delete() {
    this.onDelete.emit(this.delivery);
  }

  view() {
    this.onView.emit(this.delivery);
  }
}
