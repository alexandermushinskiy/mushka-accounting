import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { BaseDialogComponent } from '../base-dialog/base-dialog.component';
import { ConfirmDialog } from './interfaces/confirm-dialog.interface';

@Component({
  selector: 'mshk-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.scss']
})
export class ConfirmDialogComponent extends BaseDialogComponent<void> implements OnInit {
  @Input() dialogData: ConfirmDialog;

  constructor(dialogReference: NgbActiveModal) {
    super(dialogReference);
  }

  ngOnInit() {
  }

}
