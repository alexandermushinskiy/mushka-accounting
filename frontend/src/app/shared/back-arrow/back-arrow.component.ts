import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';

@Component({
  selector: 'psa-back-arrow',
  templateUrl: './back-arrow.component.html',
  styleUrls: ['./back-arrow.component.scss']
})
export class BackArrowComponent implements OnInit {

  constructor(private location: Location) { }

  ngOnInit() {
  }

  goBack() {
    this.location.back();
  }
}
