import * as moment from 'moment';

export class DatetimeService {
  parseDate(date: string, format = 'YYYY-MM-DD HH:mm'): string {
    const utcDate = moment.utc(date);
    return moment(utcDate).local().format(format);
  }

  toString(date: Date, format = 'YYYY-MM-DD'): string {
    return moment(date).format(format);
  }
}
