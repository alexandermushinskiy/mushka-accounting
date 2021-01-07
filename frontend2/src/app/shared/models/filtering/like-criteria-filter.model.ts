import { LikeCriteria } from '../../interfaces/like-criteria.interface';
import { CriteriaFilter } from './criteria-filter.interface';

export class LikeCriteriaFilter implements CriteriaFilter<LikeCriteria> {
  // tslint:disable-next-line: variable-name
  protected _value = '';

  get value(): string {
    return this._value;
  }

  set value(value: string) {
    this._value = value || '';
  }

  get valueAsCriteria(): LikeCriteria {
    return this._value ? { like: this._value } : null;
  }

  isEmpty(): boolean {
    return this.valueAsCriteria === null;
  }
}
