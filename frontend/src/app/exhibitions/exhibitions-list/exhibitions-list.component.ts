import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal, NgbModalRef, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { LocalStorage } from 'ngx-webstorage';

import { ExhibitionTableRow } from '../shared/models/exhibition-table-row.model';
import { SortableDatatableComponent } from '../../shared/hooks/sortable-datatable.component';
import { ExhibitionsService } from '../../core/api/exhibitions.service';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { ExhibitionList } from '../shared/models/exhibition-list.model';
import { ExhibitionListFilter } from '../../shared/filters/exhibition-list.filter';

@Component({
  selector: 'mk-exhibitions-list',
  templateUrl: './exhibitions-list.component.html',
  styleUrls: ['./exhibitions-list.component.scss']
})
export class ExhibitionsListComponent extends SortableDatatableComponent implements OnInit {
  @ViewChild('confirmRemoveTmpl') confirmRemoveTmpl: ElementRef;
  @LocalStorage('exhibitions_search_key', '') exhibitionsSearchKey: string;
  
  exhibitions: ExhibitionList[];
  exhibitionRows: ExhibitionTableRow[];
  loadingIndicator = false;
  total = 0;
  shown = 0;
  exhibitionToDelete: ExhibitionTableRow;

  private modalRef: NgbModalRef;
  private readonly modalConfig: NgbModalOptions = {
    windowClass: 'exhibition-modal',
    backdrop: 'static',
    size: 'sm'
  };
  
  constructor(private router: Router,
              private modalService: NgbModal,
              private exhibitionsService: ExhibitionsService,
              private notificationsService: NotificationsService) {
    super();

    this.sorts = [
      { prop: 'fromDate', dir: 'desc' },
      { prop: 'name', dir: null }
    ];
  }

  ngOnInit() {
    this.loadExhibitions();
  }

  onActive(event: any) {
    if (event.type === 'click') {
      event.cellElement.blur();
    }
  }

  filter(searchKey: string) {
    this.exhibitionsSearchKey = searchKey;

    const exhibitionFilter = new ExhibitionListFilter(searchKey);
    const filteredExhibitions = this.exhibitions.filter(exhibition => exhibitionFilter.filter(exhibition));

    this.updateDatatableRows(filteredExhibitions);
  }

  addExhibition() {
    this.router.navigate(['exhibitions/new']);
  }

  delete(exhibition: ExhibitionTableRow) {
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

  private onDeleteSuccess() {
    this.notificationsService.success('Успех', `Выставка успешно удалена из системы.`);
    this.exhibitionToDelete = null;
    this.loadExhibitions();
  }

  private onDeleteFailed(error: string) {
    this.loadingIndicator = false;
    this.exhibitionToDelete = null;
    this.notificationsService.danger('Ошибка', `Ошибка при удалении выставки: ${error}.`);
  }

  private loadExhibitions() {
    this.loadingIndicator = true;

    this.exhibitionsService.getAll()
      .subscribe(
        (res: ExhibitionList[]) => this.onLoadSuccess(res),
        () => this.onLoadError()
      );
  }
  
  private onLoadSuccess(exhibitions: ExhibitionList[]) {
    this.exhibitions = exhibitions;
    this.total = exhibitions.length;

    this.updateDatatableRows(exhibitions);

    this.loadingIndicator = false;
  }

  private updateDatatableRows(exhibitions: ExhibitionList[]) {
    this.exhibitionRows = exhibitions.map((el, index) => new ExhibitionTableRow(el, index));
    this.shown = exhibitions.length;
  }

  private onLoadError() {
    this.loadingIndicator = false;
    this.notificationsService.danger('Ошибка', 'Невозможно загрузить все выставки');
  }

}
