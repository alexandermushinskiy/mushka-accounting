import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'mshk-checkbox2',
  templateUrl: './checkbox.component.html',
  styleUrls: ['./checkbox.component.scss']
})
export class CheckboxComponent implements OnInit, OnChanges {
  @Input() control = new FormControl();
  @Input() checked: boolean;
  @Input() label: string;
  @Output() onChange: EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor() { }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.checked) {
      this.control.setValue(this.checked);
    }
  }

  change(): void {
    this.checked = !this.checked;

    this.control.setValue(this.checked);
    this.onChange.emit(this.checked);
  }
}
