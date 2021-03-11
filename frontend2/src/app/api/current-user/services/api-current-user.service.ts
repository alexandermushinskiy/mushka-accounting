import { Injectable } from '@angular/core';

import { UserData } from '../../../shared/models/user-data.model';

@Injectable({
  providedIn: 'root'
})
export class ApiCurrentUserService {
  currentUser: UserData = this.getDummyUser();

  private getDummyUser(): UserData {
    return new UserData({
      originalName: 'Administrator'
    });
  }
}
