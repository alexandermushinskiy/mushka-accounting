import { Component, OnDestroy, OnInit } from '@angular/core';

import { ExhibitionList } from '../../shared/models/exhibition-list.model';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { DateRange } from '../../shared/models/date-range.model';
import { ExhibitionListFilter } from '../../shared/filters/exhibition-list.filter';
import { ApiExhibitionsService } from '../../api/exhibitions/services/api-exhibitions.services';
import { ItemsList } from '../../shared/interfaces/items-list.interface';
import { DialogsService } from '../../shared/widgets/dialogs/services/dialogs.service';
import { I18N } from '../constants/i18n.const';
import { LanguageService } from '../../core/language/language.service';
import { DatetimeService } from '../../core/datetime/datetime.service';
import { mergeMap } from 'rxjs/operators';
import { untilDestroyed } from 'ngx-take-until-destroy';

@Component({
  selector: 'mshk-exhibitions-list',
  templateUrl: './exhibitions-list.component.html',
  styleUrls: ['./exhibitions-list.component.scss']
})
export class ExhibitionsListComponent implements OnInit, OnDestroy {
  exhibitions: ExhibitionList[];
  shownExhibitions: ExhibitionList[];
  loadingIndicator = false;
  total = 0;
  shown = 0;
  searchKey: string;
  dateRange: DateRange;
  sorts = [
    { prop: 'fromDate', dir: 'desc' },
    { prop: 'name', dir: null }
  ];

  constructor(private dialogService: DialogsService,
              private languageService: LanguageService,
              private datetimeService: DatetimeService,
              private apiExhibitionsService: ApiExhibitionsService,
              private notificationsService: NotificationsService) {
  }

  ngOnInit() {
    this.loadExhibitions();
  }

  ngOnDestroy(): void {
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
    const { title, message, cancelLabel, confirmLabel } = I18N.dialogs.deleteExhibition;
    const dialog = this.dialogService.openConfirmDialog({
      title,
      message: this.languageService.translate(message, {
        name: exhibition.name,
        fromDate: this.datetimeService.convertToFormat(exhibition.fromDate),
        toDate: this.datetimeService.convertToFormat(exhibition.toDate)
      }),
      cancelLabel,
      confirmLabel
    });

    dialog.confirm$
      .pipe(
        mergeMap(() => {
          dialog.isLoading = true;
          return this.apiExhibitionsService.deleteExhibition$(exhibition.id);
        }),
        untilDestroyed(this)
      )
      .subscribe(
        () => {
          dialog.close();
          this.onDeleteSuccess();
        },
        () => this.onDeleteFailed()
      );
  }

  private filterExhibitions() {
    const orderFilter = new ExhibitionListFilter(this.searchKey, this.dateRange);
    const filteredOrders = this.exhibitions.filter(order => orderFilter.filter(order));

    this.shownExhibitions = filteredOrders;
    this.shown = filteredOrders.length;
  }

  private onDeleteSuccess() {
    this.notificationsService.success(I18N.messages.exhibitionDeleted);
    this.loadExhibitions();
  }

  private onDeleteFailed() {
    this.loadingIndicator = false;
    this.notificationsService.error(I18N.errors.deleteExhibitionError);
  }

  private loadExhibitions() {
    this.loadingIndicator = true;

    this.apiExhibitionsService.searchExhibitions$()
      .subscribe(
        (res: ItemsList<ExhibitionList>) => this.onLoadSuccess(res),
        () => this.onLoadOrdersFailed()
      );
  }

  private onLoadSuccess(exhibitions: ItemsList<ExhibitionList>) {
    this.exhibitions = exhibitions.items;
    this.shownExhibitions = exhibitions.items;

    this.total = exhibitions.length;
    this.shown = exhibitions.length;

    this.loadingIndicator = false;
  }

  private onLoadOrdersFailed() {
    this.loadingIndicator = false;
    this.notificationsService.error(I18N.errors.loadExhibitionsError);
  }
}
