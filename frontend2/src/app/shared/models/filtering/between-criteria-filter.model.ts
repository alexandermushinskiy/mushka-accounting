import { BetweenCriteria } from '../../interfaces/between-criteria.interface';
import { CriteriaFilter } from './criteria-filter.interface';

export class BetweenCriteriaFilter implements CriteriaFilter<BetweenCriteria> {
  // tslint:disable-next-line: variable-name
  protected _value: BetweenCriteria;

  get value(): BetweenCriteria {
    return this._value ? { ...this._value } : null;
  }

  set value(value: BetweenCriteria) {
    this._value = value || null;
  }

  get valueAsCriteria(): BetweenCriteria {
    if (!this._value) {
      return null;
    }

    return {
      from: this.getFrom(),
      to: this.getTo()
    };
  }

  isEmpty(): boolean {
    const valueAsCriteria = this.valueAsCriteria;
    return valueAsCriteria === null || valueAsCriteria.from === null && valueAsCriteria.to === null;
  }

  private getFrom(): string {
    if (typeof this._value.from === 'number') {
      return String(this._value.from);
    }

    // @ts-ignore
    if (this._value.from instanceof Date) {
      return (this._value.from as unknown as Date).toISOString().split('.')[0] + 'Z';
    }

    return this._value.from;
  }

  private getTo(): string {
    if (typeof this._value.to === 'number') {
      return String(this._value.to);
    }

    // @ts-ignore
    if (this._value.to instanceof Date) {
      return (this._value.to as unknown as Date).toISOString().split('.')[0] + 'Z';
    }

    return this._value.to;
  }
}
