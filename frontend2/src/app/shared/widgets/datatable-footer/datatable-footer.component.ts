import { Component, OnInit, Input } from '@angular/core';
import { DatatableComponent } from '@swimlane/ngx-datatable';

@Component({
  selector: 'mshk-datatable-footer',
  templateUrl: './datatable-footer.component.html',
  styleUrls: ['./datatable-footer.component.scss']
})
export class DatatableFooterComponent implements OnInit {
  @Input() datatable: DatatableComponent;
  @Input() rowCount: number;
  @Input() pageSize: number;
  @Input() currentPage = 0;
  @Input() total = 0;
  @Input() loadingIndicator = false;

  pageLimitOptions = [13, 35, 50];
  minItemsForPagination = this.pageLimitOptions[0];
  currentPageLimit = this.pageLimitOptions[0];

  constructor() { }

  ngOnInit() {
    this.datatable.limit = this.currentPageLimit;
  }

  onLimitChange(limit: string) {
    this.currentPageLimit = parseInt(limit, 10);
    this.datatable.limit = this.currentPageLimit;
    this.datatable.recalculate();
    this.pageChanged(0);
  }

  pageChanged(page: number) {
    this.datatable.onFooterPage({page});
  }
}
