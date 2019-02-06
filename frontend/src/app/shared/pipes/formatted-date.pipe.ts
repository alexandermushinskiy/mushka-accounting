import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';

@Pipe({
  name: 'formattedDate'
})
export class FormattedDatePipe implements PipeTransform {
  transform(value: string): string {
    moment.locale('ru');
    return moment(value).format('DD MMM YYYY');
  }
}
