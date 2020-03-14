import { FormControl } from '@angular/forms';

export function uniqueOrderNumber(input: FormControl) {
  return { notUnique: true };
}
