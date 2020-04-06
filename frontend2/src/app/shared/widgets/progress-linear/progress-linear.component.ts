import { Component, Input } from '@angular/core';

@Component({
  selector: 'mshk-progress-linear',
  templateUrl: './progress-linear.component.html',
  styleUrls: ['./progress-linear.component.scss']
})
export class ProgressLinearComponent {
  @Input() isLoading = false;

  constructor() { }
}
