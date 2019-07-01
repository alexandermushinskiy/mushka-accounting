import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { BsModalService } from 'ngx-bootstrap';
import { Subject } from 'rxjs';

import { ComponentCanDeactivate } from '../hooks/component-can-deactivate.component';
import { ConfirmLeaveComponent } from '../widgets/confirm-leave/confirm-leave.component';

@Injectable()
export class HandleUnsavedDataGuard implements CanDeactivate<ComponentCanDeactivate> {

  constructor(private modalService: BsModalService) {
  }
  
  canDeactivate(component: ComponentCanDeactivate) {
    if (!component.canDeactivate()) {

      const subject = new Subject<boolean>();

      const modal = this.modalService.show(ConfirmLeaveComponent, {'class': 'confirm-leave-page-modal'});
      modal.content.subject = subject;

      return subject.asObservable();
    }
    return true;
  }
}
