import * as moment from 'moment';

export class DatetimeService {
  parseDate(date: string, format = 'YYYY-MM-DD HH:mm'): string {
    const utcDate = moment.utc(date);
    return moment(utcDate).local().format(format);
  }

  getCurrentDateInString(): string {
    return this.toString(new Date());
  }

  toString(date: Date, format = 'DD MMM YYYY'): string {
    return moment(date).locale('ru').format(format);
  }
}
