import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { environment } from '../../../../environments/environment';
import { ItemsList } from '../../../shared/interfaces/items-list.interface';
import { ExhibitionList } from '../../../shared/models/exhibition-list.model';
import { ExhibitionProduct } from '../../../shared/models/exhibition-product.model';
import { Exhibition } from '../../../shared/models/exhibition.model';
import { ApiExhibitionsTransformService } from './api-exhibitions-transform.service';

@Injectable({
  providedIn: 'root'
})
export class ApiExhibitionsService {
  private readonly endPoint = `${environment.apiEndpoint}/api/v1/exhibitions`;

  constructor(private http: HttpClient,
              private transformService: ApiExhibitionsTransformService) {
  }

  searchExhibitions$(): Observable<ItemsList<ExhibitionList>>  {
    return this.http.post(`${this.endPoint}/search`, {})
      .pipe(
        map((data: any) => this.transformService.fromSearchExhibitions(data)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  describeExhibition$(exhibitionId: string): Observable<Exhibition> {
    const url = `${this.endPoint}/${exhibitionId}/describe`;

    return this.http.get(url)
      .pipe(
        map((data: any) => this.transformService.fromDescribeExhibition(data)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  getDefaultExhibitionProducts$(): Observable<ExhibitionProduct[]> {
    return this.http.get(`${this.endPoint}/default-products`)
      .pipe(
        map((data: any) => this.transformService.fromGetDefaultExhibitionProducts(data)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  createExhibition$(exhibition: Exhibition): Observable<void> {
    return this.http.post<void>(this.endPoint, exhibition)
      .pipe(
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  updateExhibition$(exhibitionId: string, exhibition: Exhibition): Observable<void> {
    return this.http.put<void>(`${this.endPoint}/${exhibitionId}`, exhibition)
      .pipe(
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  deleteExhibition$(exhibitionId: string): Observable<void> {
    return this.http.delete<void>(`${this.endPoint}/${exhibitionId}`)
      .pipe(
        catchError((res: any) => throwError(res.error.messages))
      );
  }
}
