import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

import { UserNotification } from '../../shared/models/user-notification.model';
import { UserNotificationType } from '../../shared/enums/user-notification-type.enum';

@Injectable()
export class UserNotificationsService {
  private notificationsUpdates$ = new BehaviorSubject<UserNotification[]>([]);
  private selectedNotification$ = new BehaviorSubject<UserNotification>(null);

  private currentNotifications: UserNotification[] = [];

  constructor() { }

  getNotifications(): Observable<UserNotification[]> {
    return this.notificationsUpdates$.asObservable();
  }

  getNotification(): Observable<UserNotification> {
    return this.selectedNotification$.asObservable();
  }

  addNotification(notification: UserNotification) {
    this.currentNotifications.push(notification);
    this.notificationsUpdates$.next(this.currentNotifications);
  }

  focusNotification(notification: UserNotification) {
    this.selectedNotification$.next(notification);
  }

  dismissNotification() {
    this.selectedNotification$.next(null);
  }

  private activateUploadNotification(uploadingNotification: UserNotification) {
    this.dismissNotification();
    this.selectedNotification$.next(uploadingNotification);
  }

  private removeNotification(notificationType: UserNotificationType) {
    const index = this.currentNotifications.findIndex(notification => notification.type === notificationType);
    if (index >= 0) {
      this.currentNotifications.splice(index, 1);
      this.notificationsUpdates$.next(this.currentNotifications);
    }
  }
}
