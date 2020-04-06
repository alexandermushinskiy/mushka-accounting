import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

import { ConverterService } from '../converter/converter.service';
import { environment } from '../../../environments/environment';
import { Exhibition } from '../../shared/models/exhibition.model';
import { ExhibitionProduct } from '../../shared/models/exhibition-product.model';
import { ExhibitionList } from '../../exhibitions/shared/models/exhibition-list.model';

@Injectable()
export class ExhibitionsService {
  private readonly endPoint = `${environment.apiEndpoint}/api/v1/exhibitions`;

  constructor(private http: HttpClient,
              private converterService: ConverterService) {
  }

  getAll(): Observable<ExhibitionList[]>  {
    return this.http.get(this.endPoint).pipe(
      map((res: any) => this.converterService.convertToExhibitionsList(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  getById(productId: string): Observable<Exhibition> {
    return this.http.get(`${this.endPoint}/${productId}`).pipe(
      map((res: any) => this.converterService.convertToExhibition(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  getDefaultProducts(): Observable<ExhibitionProduct[]> {
    return this.http.get(`${this.endPoint}/default-products`).pipe(
      map((res: any) => this.converterService.convertToExhibitionProducts(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  create(exhibition: Exhibition): Observable<Exhibition> {
    return this.http.post(this.endPoint, exhibition).pipe(
      map((res: any) => this.converterService.convertToExhibition(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  update(exhibitionId: string, exhibition: Exhibition): Observable<Exhibition> {
    return this.http.put(`${this.endPoint}/${exhibitionId}`, exhibition).pipe(
      map((res: any) => this.converterService.convertToExhibition(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  delete(exhibitionId: string): Observable<any> {
    return this.http.delete(`${this.endPoint}/${exhibitionId}`).pipe(
      catchError((res: any) => throwError(res.error.messages)));
  }
}
