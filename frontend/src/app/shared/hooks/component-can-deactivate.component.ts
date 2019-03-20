import { HostListener } from '@angular/core';

import { UnsubscriberComponent } from './unsubscriber.component';

export abstract class ComponentCanDeactivate extends UnsubscriberComponent {
  abstract canDeactivate(): boolean;

  constructor() {
    super();
  }

  @HostListener('window:beforeunload', ['$event'])
    unloadNotification($event: any) {
      if (!this.canDeactivate()) {
        $event.returnValue = true;
      }
    }
}
