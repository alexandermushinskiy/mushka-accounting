import { OnDestroy, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { Subject } from 'rxjs';

export abstract class BaseDialogComponent<T = void> implements OnInit, OnDestroy {
  cancel$ = new Subject<void>();
  confirm$ = new Subject<T>();
  isLoading = false;
  isConfirmDisabled = false;
  isCancelDisabled = false;

  get canConfirm(): boolean {
    return !this.isConfirmDisabled && !this.isLoading;
  }

  get canCancel(): boolean {
    return !this.isCancelDisabled;
  }

  constructor(private bsModalRef: BsModalRef) {
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
  }

  close(): void {
    this.bsModalRef.hide();
  }

  confirmAction(): void {
    if (!this.canConfirm) {
      return;
    }

    this.confirm$.next();
  }

  cancelAction(): void {
    if (!this.canCancel) {
      return;
    }

    this.cancel$.next();
    this.close();
  }

}
