import { Component, OnInit } from '@angular/core';

import { CurrentUserService } from '../../../../core/api/current-user.service';
import { UserData } from '../../../../shared/models/user-data.model';

@Component({
  selector: 'mshk-current-user',
  templateUrl: './current-user.component.html',
  styleUrls: ['./current-user.component.scss']
})
export class CurrentUserComponent implements OnInit {
  user: UserData;

  constructor(private currentUserService: CurrentUserService) {
  }

  ngOnInit() {
    this.user = this.currentUserService.currentUser;
  }
}
