import * as moment from 'moment';

export class DatetimeService {
  // parseDate(date: string, format = 'YYYY-MM-DD HH:mm'): string {
  //   const utcDate = moment.utc(date);
  //   return moment(utcDate).local().format(format);
  // }

  convertTorFormat(date: string, format = 'DD MMM YYYY'): string {
    return moment(date).locale('ru').format(format);
  }

  getCurrentDateInString(format = 'YYYY-MM-DD'): string {
    return this.toString(new Date(), format);
  }

  toString(date: Date, format = 'YYYY-MM-DD'): string {
    return moment(date).locale('ru').format(format);
  }

  convertFromToFormat(date: string, inputFormat = 'DD MMM YYYY', outputFormat = 'YYYY-MM-DD'): string {
    return moment(date, inputFormat).locale('ru').format(outputFormat);
  }
}
