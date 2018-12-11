export abstract class FiltersBase {

  containsSearchKey(value: string, searchKey: string): boolean {
    if (!value && !!searchKey) {
      return false;
    }

    return !searchKey || value.toLowerCase().includes(searchKey.toLowerCase());
  }

  isEmpty(): boolean {
    return !Object.values(this).some(value => !!value);
  }
}
