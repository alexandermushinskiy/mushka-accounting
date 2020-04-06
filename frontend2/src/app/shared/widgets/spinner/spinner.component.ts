import { Component, Input } from '@angular/core';

@Component({
  selector: 'mshk-spinner',
  templateUrl: './spinner.component.html',
  styleUrls: ['./spinner.component.scss']
})
export class SpinnerComponent {
  @Input() isLoading = false;

  constructor() {
  }
}
