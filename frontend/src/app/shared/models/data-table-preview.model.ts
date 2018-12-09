export abstract class DataTablePreview {
  index: number;
  id: string;
  className: string;

  readonly defaultValue = ' - ';

  constructor(elem, index: number) {
    this.index = index;
    this.id = elem.id;
    this.className = elem.className || this.getClassName(index);
  }

  getClassName(index: number) {
    return index % 2 === 0 ? 'even' : 'odd';
  }
}
