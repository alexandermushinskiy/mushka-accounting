import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';

@Pipe({
  name: 'format'
})
export class FormatPipe implements PipeTransform {
  transform(value: string): string {
    moment.locale('ru');
    return moment(value).format('DD MMM YYYY');
  }
}
