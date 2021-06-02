import { OnDestroy, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
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

  protected constructor(private dialogReference: NgbActiveModal) {
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
  }

  close(): void {
    this.dialogReference.close();
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
