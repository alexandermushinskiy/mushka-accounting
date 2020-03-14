import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { Subject } from 'rxjs';

@Component({
  selector: 'mshk-confirm-leave',
  templateUrl: './confirm-leave.component.html',
  styleUrls: ['./confirm-leave.component.scss']
})
export class ConfirmLeaveComponent implements OnInit {
  subject: Subject<boolean>;

  constructor(public modalRef: BsModalRef) { }

  ngOnInit() {
  }

  action(value: boolean) {
    this.modalRef.hide();
    this.subject.next(value);
    this.subject.complete();
  }
}
