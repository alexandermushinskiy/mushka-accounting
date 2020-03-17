import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'mshk-confirmation',
  templateUrl: './confirmation.component.html',
  styleUrls: ['./confirmation.component.scss']
})
export class ConfirmationComponent {
  @Input() plainText: string;
  @Input() isSaving = false;
  @Input() confirmButtonText = 'button.yes';
  @Input() headerText = 'common.confirmation';
  @Input() cancelText = 'button.cancel';
  @Output() onConfirm = new EventEmitter();
  @Output() onClose = new EventEmitter();

  confirm() {
    this.onConfirm.emit()
  }

  closeModal() {
    this.onClose.emit();
  }
}
