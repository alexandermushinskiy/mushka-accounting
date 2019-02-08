import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'mk-daterage-modal',
  templateUrl: './daterage-modal.component.html',
  styleUrls: ['./daterage-modal.component.scss']
})
export class DateRageModalComponent implements OnInit {
  @Output() onApply = new EventEmitter();
  @Output() onClose = new EventEmitter<void>();

  hoveredDate: NgbDate;
  fromDate: NgbDate;
  toDate: NgbDate;

  constructor() { }

  ngOnInit() {
    // this.localeService.use('ru');
  }

  onDateSelection(date: NgbDate) {
    if (!this.fromDate && !this.toDate) {
      this.fromDate = date;
    } else if (this.fromDate && !this.toDate && date.after(this.fromDate)) {
      this.toDate = date;
    } else {
      this.toDate = null;
      this.fromDate = date;
    }
  }

  isHovered(date: NgbDate) {
    return this.fromDate && !this.toDate && this.hoveredDate && date.after(this.fromDate) && date.before(this.hoveredDate);
  }

  isInside(date: NgbDate) {
    return date.after(this.fromDate) && date.before(this.toDate);
  }

  isRange(date: NgbDate) {
    return date.equals(this.fromDate) || date.equals(this.toDate) || this.isInside(date) || this.isHovered(date);
  }

  apply() {
    this.onApply.emit();
  }

  close() {
    this.onClose.emit();
  }
}
