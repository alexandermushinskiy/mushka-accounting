import { HostListener } from '@angular/core';

import { UnsubscriberComponent } from './unsubscriber.component';

export abstract class ComponentCanDeactivate extends UnsubscriberComponent {
  isSaved = false;

  abstract hasUnsavedData(): boolean;

  constructor() {
    super();
  }

  @HostListener('window:beforeunload', ['$event'])
    unloadNotification($event: any) {
      if (this.hasUnsavedData()) {
        $event.returnValue = true;
      }
    }
}
