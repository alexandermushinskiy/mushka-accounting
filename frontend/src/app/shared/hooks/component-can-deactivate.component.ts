import { HostListener } from '@angular/core';

import { UnsubscriberComponent } from './unsubscriber.component';

export abstract class ComponentCanDeactivate extends UnsubscriberComponent {
  abstract hasUnsavedData(): boolean;
  isSaved = false;

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
