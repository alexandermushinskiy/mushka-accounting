import { Injectable } from '@angular/core';
import { CanDeactivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { BsModalService } from 'ngx-bootstrap';
import { Subject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { ComponentCanDeactivate } from '../hooks/component-can-deactivate.component';
import { ConfirmLeaveComponent } from '../widgets/confirm-leave/confirm-leave.component';

@Injectable()
export class HandleUnsavedDataGuard implements CanDeactivate<ComponentCanDeactivate> {
  private modalConfig = {
    ignoreBackdropClick: true,
    class: 'confirm-leave-page-modal'
  };

  constructor(private modalService: BsModalService,
              private router: Router) {
  }

  canDeactivate(component: ComponentCanDeactivate,
                route: ActivatedRouteSnapshot,
                state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    if (component.hasUnsavedData()) {
      const subject = new Subject<boolean>();

      const modal = this.modalService.show(ConfirmLeaveComponent, this.modalConfig);
      modal.content.subject = subject;

      return subject.asObservable().pipe(
        map((isLeavePage: boolean) => {
            if (!isLeavePage) {
              // window.history.replaceState({}, '', window.location.href);
              window.history.pushState({}, '', this.router.url);
            }
            return isLeavePage;
          })
      );
    }

    return true;
  }
}
