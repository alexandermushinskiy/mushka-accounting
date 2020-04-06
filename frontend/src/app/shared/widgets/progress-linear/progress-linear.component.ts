import { Component, Input } from '@angular/core';

@Component({
  selector: 'mk-progress-linear',
  templateUrl: './progress-linear.component.html',
  styleUrls: ['./progress-linear.component.scss']
})
export class ProgressLinearComponent {
  @Input() isLoading = false;

  constructor() { }
}
