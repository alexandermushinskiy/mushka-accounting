import { TimeFrame } from '../enums/time-frame.enum';
import { I18N } from './i18n.const';

export const TIME_FRAME_CONFIG = {
  dateFormat: 'YYYY-MM-DD',
  defaultValue: {
    from: null,
    to: null
  },
  items: [
    {
      id: TimeFrame.LastMonth,
      name: I18N.timeFrames.lastMonth,
      hidden: false
    },
    {
      id: TimeFrame.CurrentMonth,
      name: I18N.timeFrames.currentMonth,
      hidden: false
    },
    {
      id: TimeFrame.CurrentQuarter,
      name: I18N.timeFrames.currentQurter,
      hidden: false
    },
    {
      id: TimeFrame.CurrentYear,
      name: I18N.timeFrames.currentYear,
      hidden: false
    },
    {
      id: TimeFrame.CustomRange,
      name: I18N.timeFrames.customPeriod,
      hidden: false
    }
  ]
};
