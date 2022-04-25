import { AbstractControl, FormArray, FormControl, FormGroup } from '@angular/forms';

export function markFormGroupDirty(formGroup: FormGroup): void {
  const markControlDirty = (formControl: FormControl): void => {
    formControl.markAsDirty();
  };

  const markDirty = (control: AbstractControl): void => {
    switch (control.constructor.name) {
      case 'FormGroup':
        markGroupDirty(control as FormGroup);
        break;
      case 'FormArray':
        markArrayDirty(control as FormArray);
        break;
      case 'FormControl':
        markControlDirty(control as FormControl);
        break;
    }
  };

  const markGroupDirty = (frmGroup: FormGroup): void => {
    Object.keys(frmGroup.controls).forEach(key => {
      markDirty(frmGroup.get(key));
    });
  };

  const markArrayDirty = (formArray: FormArray): void => {
    formArray.controls.forEach(control => {
      markDirty(control);
    });
  };

  markGroupDirty(formGroup);
}
