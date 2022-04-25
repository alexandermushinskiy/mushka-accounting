import { Component, Input } from '@angular/core';

import { IconColor } from './enums/icon-color.enum';

@Component({
  selector: 'mshk-icon',
  templateUrl: './icon.component.html',
  styleUrls: ['./icon.component.scss']
})
export class IconComponent {
  @Input() name!: string;
  @Input() color: IconColor = IconColor.Primary;
  @Input() withBorder: boolean = false;
}
