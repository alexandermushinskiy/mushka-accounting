import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { IconColor } from '../../icon/enums/icon-color.enum';
import { IconName } from '../../icon/enums/icon-name.enum';

@Component({
  selector: 'mshk-icon-button',
  templateUrl: './icon-button.component.html',
  styleUrls: ['./icon-button.component.scss']
})
export class IconButtonComponent implements OnInit {
  @Input() label: string;
  @Input() iconName: IconName;
  @Input() iconColor: IconColor = IconColor.Primary;
  @Output() onClick = new EventEmitter<void>();

  constructor() { }

  ngOnInit() {
  }

  clickHandler(): void {
    this.onClick.emit();
  }
}
