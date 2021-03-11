import { Component, OnInit } from '@angular/core';

import { ApiCurrentUserService } from '../../../../api/current-user/services/api-current-user.service';
import { UserData } from '../../../../shared/models/user-data.model';

@Component({
  selector: 'mshk-current-user',
  templateUrl: './current-user.component.html',
  styleUrls: ['./current-user.component.scss']
})
export class CurrentUserComponent implements OnInit {
  user: UserData;

  constructor(private apiCurrentUserService: ApiCurrentUserService) {
  }

  ngOnInit() {
    this.user = this.apiCurrentUserService.currentUser;
  }

  logOut() {
  }
}
