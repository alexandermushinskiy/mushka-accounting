import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { NgbModal, NgbModalRef, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';

import { ExhibitionList } from '../../shared/models/exhibition-list.model';
import { ExhibitionsService } from '../../core/api/exhibitions.service';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { DateRange } from '../../shared/models/date-range.model';
import { ExhibitionListFilter } from '../../shared/filters/exhibition-list.filter';

@Component({
  selector: 'mshk-exhibitions-list',
  templateUrl: './exhibitions-list.component.html',
  styleUrls: ['./exhibitions-list.component.scss']
})
export class ExhibitionsListComponent implements OnInit {
  @ViewChild('confirmRemoveTmpl', { static: false }) confirmRemoveTmpl: ElementRef;
  exhibitions: ExhibitionList[];
  shownExhibitions: ExhibitionList[];
  loadingIndicator = false;
  total = 0;
  shown = 0;
  exhibitionToDelete: ExhibitionList;
  searchKey: string;
  dateRange: DateRange;
  sorts = [
    { prop: 'fromDate', dir: 'desc' },
    { prop: 'name', dir: null }
  ];

  private modalRef: NgbModalRef;
  private readonly modalConfig: NgbModalOptions = {
    windowClass: 'exhibition-modal',
    backdrop: 'static',
    size: 'sm'
  };

  constructor(private modalService: NgbModal,
              private exhibitionsService: ExhibitionsService,
              private notificationsService: NotificationsService) {
  }

  ngOnInit() {
    this.loadExhibitions();
  }

  onActive(event: any) {
    if (event.type === 'click') {
      event.cellElement.blur();
    }
  }

  onSearch(searchKey: string) {
    this.searchKey = searchKey;

    this.filterExhibitions();
  }

  onRangeSelected(dateRange: DateRange) {
    this.dateRange = dateRange;
    this.filterExhibitions();
  }

  delete(exhibition: ExhibitionList) {
    setTimeout(() => {
      this.exhibitionToDelete = exhibition;
      this.modalRef = this.modalService.open(this.confirmRemoveTmpl, this.modalConfig);
    });
  }

  confirmDelete() {
    this.loadingIndicator = true;
    this.closeModal();

    this.exhibitionsService.delete(this.exhibitionToDelete.id)
      .subscribe(
        () => this.onDeleteSuccess(),
        (error: string) => this.onDeleteFailed(error)
      );
  }

  closeModal() {
    if (this.modalRef) {
      this.modalRef.close();
    }
  }

  private filterExhibitions() {
    const orderFilter = new ExhibitionListFilter(this.searchKey, this.dateRange);
    const filteredOrders = this.exhibitions.filter(order => orderFilter.filter(order));

    this.shownExhibitions = filteredOrders;
    this.shown = filteredOrders.length;
  }

  private onDeleteSuccess() {
    this.notificationsService.success('exhibitions.exhibitionDeleted');
    this.exhibitionToDelete = null;
    this.loadExhibitions();
  }

  private onDeleteFailed(error: string) {
    this.loadingIndicator = false;
    this.exhibitionToDelete = null;
    this.notificationsService.error('exhibitions.errorDeletingExhibition');
  }

  private loadExhibitions() {
    this.loadingIndicator = true;

    this.exhibitionsService.getAll()
      .subscribe(
        (res: ExhibitionList[]) => this.onLoadSuccess(res),
        () => this.onLoadOrdersFailed()
      );
  }

  private onLoadSuccess(exhibitions: ExhibitionList[]) {
    this.exhibitions = exhibitions;
    this.shownExhibitions = exhibitions;

    this.total = exhibitions.length;
    this.shown = exhibitions.length;

    this.loadingIndicator = false;
  }

  private onLoadOrdersFailed() {
    this.loadingIndicator = false;
    this.notificationsService.error('exhibitions.errorLoadingExhibitions');
  }
}
