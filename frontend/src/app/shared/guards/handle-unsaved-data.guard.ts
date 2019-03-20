import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';

import { ComponentCanDeactivate } from '../hooks/component-can-deactivate.component';

@Injectable()
export class HandleUnsavedDataGuard implements CanDeactivate<ComponentCanDeactivate> {
  canDeactivate(component: ComponentCanDeactivate): boolean {
    if (!component.canDeactivate()) {
        if (confirm('You have unsaved changes! If you leave, your changes will be lost.')) {
            return true;
        } else {
            return false;
        }
    }
    return true;
  }
}
