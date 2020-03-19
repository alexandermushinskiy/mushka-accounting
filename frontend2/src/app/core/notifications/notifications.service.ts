import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';

@Injectable()
export class NotificationsService {
  private options = {
    enableHtml: true,
    tapToDismiss: true,
    timeOut: 5000
  };

  private errorOptions = {
    enableHtml: true,
    disableTimeOut: true
  };

  constructor(private toastrService: ToastrService,
              private translateService: TranslateService) {
  }

  success(messageKey: string) {
    this.translateService.get(messageKey)
      .subscribe((message: string) => {
        this.toastrService.success(message, null, this.options);
      });
  }

  warning(message: string, title: string = null) {
    this.toastrService.warning(message, title, this.options);
  }

  error(error: string, title: string = 'Ошибка') {
    this.toastrService.error(error, title, this.errorOptions);
  }
}
