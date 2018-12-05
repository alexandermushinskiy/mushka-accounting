export class SizeItem {
  id: string;
  name: string;
  quantity = 0;

  constructor(data: any) {
    Object.assign(this, data);
  }

  getCssClass() {
    if (this.quantity === 0 ) {
      return 'size-label-sold';
    }
    return this.quantity > 10
      ? 'size-label'
      : 'size-label-ends';
  }

}
