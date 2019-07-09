import { Component, OnInit, Input } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'mk-back-arrow',
  templateUrl: './back-arrow.component.html',
  styleUrls: ['./back-arrow.component.scss']
})
export class BackArrowComponent implements OnInit {
  @Input() navigateBackUrl: string;

  constructor(private location: Location, private router: Router) { }

  ngOnInit() {
  }

  goBack() {
    if (!!this.navigateBackUrl) {
      this.router.navigateByUrl(this.navigateBackUrl);
    } else {
      this.location.back();
    }
  }
}
