import { Component, Input } from '@angular/core';

@Component({
  selector: 'mshk-divider',
  templateUrl: './divider.component.html',
  styleUrls: ['./divider.component.scss']
})
export class DividerComponent {
  @Input() verticalMode = false;
}