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

  success(messageKey: string, translateParameter: any = null) {
    this.translateService.get(messageKey, translateParameter)
      .subscribe((message: string) => {
        this.toastrService.success(message, null, this.options);
      });
  }

  warning(message: string, title: string = null) {
    this.toastrService.warning(message, title, this.options);
  }

  error(errorMessageKey: string, translateParameter: any = null) {
    this.translateService.get(errorMessageKey, translateParameter)
      .subscribe((message: string) => {
        this.toastrService.success(message, null, this.errorOptions);
      });
  }
}
