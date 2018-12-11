import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'psa-checkbox',
  templateUrl: './checkbox.component.html',
  styleUrls: ['./checkbox.component.scss']
})
export class CheckboxComponent implements OnInit {
  @Input() checked: boolean;
  @Output() onChange: EventEmitter<any> = new EventEmitter<any>();

  constructor() { }

  ngOnInit() {
  }

  change(event: any) {
    this.checked = !this.checked;
    this.onChange.emit(event);
  }
}
