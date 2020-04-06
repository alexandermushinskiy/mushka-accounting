export class ItemsWithTotalCount<TModel> {
  constructor(public items: TModel[],
              public totalCount: number) {
  }
}
