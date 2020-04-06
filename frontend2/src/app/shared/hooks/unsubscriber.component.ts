import { Subject } from 'rxjs';
import { OnDestroy } from '@angular/core';

export abstract class UnsubscriberComponent implements OnDestroy {
  protected ngUnsubscribe$: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.ngUnsubscribe$.next();
    this.ngUnsubscribe$.complete();
  }
}
