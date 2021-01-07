export function isPlainObject(p: any): boolean {
  return Object.prototype.toString.call(p) === '[object Object]';
}

export function isObjectOrArray(p: any): boolean {
  return isPlainObject(p) || Array.isArray(p);
}

export function removeEmptyProperties(p: Record<string, any>): Record<string, any> {
  const filtered = Object.entries(p)
    .filter(([key, value]) => {
      if (!Array.isArray(value)) {
        return value;
      }

      return value.filter(v => !!v).length > 0 ? value : null;
    })
    .filter(([key, value]) => !!value)
    .reduce((prev, [key, value]) => {
      if (isPlainObject(value)) {
        value = removeEmptyProperties({ ...value });
      }

      if (value) {
        prev[key] = value;
      }

      return prev;
    }, {});

  return Object.keys(filtered).length ? filtered : null;
}
