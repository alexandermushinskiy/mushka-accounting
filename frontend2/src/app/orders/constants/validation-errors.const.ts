import { ValidationError } from '../enums/validation-error.enum';
import { I18N } from './i18n.const';

export const VALIDATION_ERRORS = {
  [ValidationError.Required]: I18N.validation.requiredField,
  [ValidationError.NotUnique]: I18N.validation.notUnique,
  [ValidationError.Email]: I18N.validation.invalidEmailFormat
};
