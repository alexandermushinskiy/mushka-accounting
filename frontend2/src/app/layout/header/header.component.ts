import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'mshk-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  isLoading = false;

  constructor() {
  }

  ngOnInit() {
  }

  onSearch(searchKey: string) {
  }
}
