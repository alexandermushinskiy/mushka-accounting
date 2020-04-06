import { FormControl, Validator, NG_VALIDATORS, AbstractControl, AsyncValidator, ValidationErrors, AsyncValidatorFn } from '@angular/forms';
import { Directive, Input, Injectable } from '@angular/core';
import { OrdersService } from '../../core/api/orders.service';
import { Observable } from 'rxjs';

export function uniqueOrderNumber(input: FormControl) {
  return { notUnique: true };
  // return hasExclamation ? null : { notUnique: true };
}

// export function orderNumberValidator(ordersService: OrdersService): AsyncValidatorFn {
//   return (control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> => {
//     return ordersService.validateOrderNumber(control.value)
//       .map((isValid: boolean) => isValid ? null : {"notUnique": true});
//   };
// }

// @Injectable()
// export class OrderNumberValidator {
//     constructor(private ordersService: OrdersService) {
//     }

//     uniqueValidator(initialDate: string = ""): AsyncValidatorFn {
//         return (control: AbstractControl): Promise<{ [key: string]: any } | null> | Observable<{ [key: string]: any } | null> => {
//           return this.ordersService.validateOrderNumber(control.value)
//             .map((isValid: boolean) => isValid ? null : {"notUnique": true});
//         };
//     }
// }

// @Injectable()
// export class orderNumberValidator implements AsyncValidator  {

//   @Input("uniqueNumber") uniqueNumber: string;

//   constructor(private ordersService: OrdersService) {
//   }

//   registerOnValidatorChange?(fn: () => void): void {
//     throw new Error("Method not implemented.");
//   }

//   validate(control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> {
//     return this.ordersService.validateOrderNumber(control.value)
//       .map((isValid: boolean) => isValid ? null : {"mobNumExists": true});
//   }

//    //validate = (control: FormControl) =>
//     //this.ordersService.validateOrderNumber(control.value);

// //     let value = control.value;

// //     if ((value == null || value == undefined || value == "") && this.uniqueNumber) {
// //       return {
// //         uniqueNumber: {condition:this.uniqueNumber}
// //       };
// //      }

// //      return null;
//   //}
// }
