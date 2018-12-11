import { Component, OnInit, ElementRef, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { NgbModal, NgbModalRef, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';

import { availableColumns } from '../../shared/constants/available-columns.const';
import { ServiceItem } from '../shared/models/service-item.model';
import { ServiceItemTablePreview } from '../shared/models/service-item-table-preview.model';

@Component({
  selector: 'mk-delivery-services-list',
  templateUrl: './delivery-services-list.component.html',
  styleUrls: ['./delivery-services-list.component.scss']
})
export class DeliveryServicesListComponent implements OnInit {
  @ViewChild('removeConfirmation') removeConfirmation: ElementRef;
  @Input() isReadOnly = false;
  @Input() set serviceItems(data: ServiceItem[]) {
    if (data) {
      this.serviceItemRows = data.map((el, index) => new ServiceItemTablePreview(el, index));
      this.total = data.length;
    } else {
      this.serviceItemRows = [];
      this.total = 0;
    }
  }
  @Output() onItemAdded = new EventEmitter<ServiceItem>();
  @Output() onItemUpdated = new EventEmitter<{rowIndex: number, property: string, value: number | string}>();
  @Output() onItemDeleted = new EventEmitter<number>();

  availableColumns = availableColumns.deliveryServices;
  total = 0;
  shown = 0;
  serviceItemRows: ServiceItemTablePreview[] = [];
  indexToDelete: number;
  editing = {};

  private modalRef: NgbModalRef;
  private readonly modalConfig: NgbModalOptions = {
    windowClass: 'delivery-items-modal',
    backdrop: 'static',
    size: 'sm'
  };

  constructor(private modalService: NgbModal) { }

  ngOnInit() {
  }

  onRowsUpdated(rowsAmount: number) {
    this.shown = rowsAmount;
  }

  addDeliveryItem(content: ElementRef) {
    this.modalRef = this.modalService.open(content, this.modalConfig);
  }

  closeModal() {
    this.modalRef.close();
  }

  saveServiceItem(serviceItem: ServiceItem) {
    this.onItemAdded.emit(serviceItem);
    this.closeModal();
  }

  getRowClass(row) {
    return row.className;
  }

  activateRow({ type, row, cellElement }: { type: string, row: ServiceItemTablePreview, cellElement: any }) {
    // if (type === 'click') {
    //   this.editedTimeReport = row;
    //   cellElement.blur();
    //   this.openTimeReportModal();
    //   row.className += ' active';
    // }
  }

  deleteItem(rowIndex) {
    this.indexToDelete = rowIndex;
    this.modalRef = this.modalService.open(this.removeConfirmation);
  }

  confirmDelete() {
    this.onItemDeleted.emit(this.indexToDelete);
  }

  startEditing(cellKey: string) {
    this.editing = {};
    this.editing[cellKey] = true;
  }

  updateValue(value, cell, rowIndex) {
    this.editing[rowIndex + '-' + cell] = false;
    this.onItemUpdated.emit({rowIndex: rowIndex, property: cell, value: value});
  }
}
