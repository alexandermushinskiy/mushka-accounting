import { Component, Input } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'mshk-textarea-input',
  templateUrl: './textarea-input.component.html',
  styleUrls: ['./textarea-input.component.scss']
})
export class TextareaInputComponent {
  @Input() control = new FormControl();
  @Input() label: string;
  @Input() placeholder = '';
  @Input() rowsCount = 5;
  @Input() readonly = false;
}
