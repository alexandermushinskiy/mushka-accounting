import { ViewParameterType } from './view-parameter-type.enum';

export class ViewParameter {
  name = '';
  displayValue = '';
  isPredefined = false;
  condition: string = null;
  type: ViewParameterType = ViewParameterType.TEXT;
  isValueIgnored = false;

  constructor(data: any = {}) {
    Object.assign(this, data);
  }

  set value(value: string) {
    if (value) {
      this.displayValue = value;
    }
  }
}
