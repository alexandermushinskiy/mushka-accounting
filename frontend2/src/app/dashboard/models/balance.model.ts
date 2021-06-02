export class Balance {
  expense: number;
  profit: number;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
