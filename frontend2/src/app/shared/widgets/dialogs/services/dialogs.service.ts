import { Injectable, TemplateRef } from '@angular/core';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';

import { ConfirmDialogComponent } from '../components/confirm-dialog/confirm-dialog.component';
import { ConfirmDialog } from '../components/confirm-dialog/interfaces/confirm-dialog.interface';

@Injectable({
  providedIn: 'root'
})
export class DialogsService {
  constructor(private modalService: NgbModal) {
  }

  openConfirmDialog(dialogData: ConfirmDialog): ConfirmDialogComponent {
    const options = {
      windowClass: 'order-modal',
      size: 'sm'
    } as NgbModalOptions;

    const instance = this.openDialog<ConfirmDialogComponent>(ConfirmDialogComponent, options);
    instance.dialogData = dialogData;
    return instance;
  }

  private openDialog<DialogType>(reference: any, config: NgbModalOptions = {}): DialogType {
    const options = { backdrop: false, ...config };
    return this.modalService.open(reference, options).componentInstance;
  }
}
