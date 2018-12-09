export class SelectSize {
  id: string;
  name: string;
  disabled: boolean;

  constructor(data) {
    Object.assign(this, data);
  }
}
