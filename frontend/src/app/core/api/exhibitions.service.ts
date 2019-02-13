import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';

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
    return this.http.get(this.endPoint)
      .map((res: any) => this.converterService.convertToExhibitionsList(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  getById(productId: string): Observable<Exhibition> {
    return this.http.get(`${this.endPoint}/${productId}`)
      .map((res: any) => this.converterService.convertToExhibition(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  getDefaultProducts(): Observable<ExhibitionProduct[]> {
    return this.http.get(`${this.endPoint}/default-products`)
      .map((res: any) => this.converterService.convertToExhibitionProducts(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  create(exhibition: Exhibition): Observable<Exhibition> {
    return this.http.post(this.endPoint, exhibition)
      .map((res: any) => this.converterService.convertToExhibition(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  update(exhibitionId: string, exhibition: Exhibition): Observable<Exhibition> {
    return this.http.put(`${this.endPoint}/${exhibitionId}`, exhibition)
      .map((res: any) => this.converterService.convertToExhibition(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  delete(exhibitionId: string): Observable<any> {
    return this.http.delete(`${this.endPoint}/${exhibitionId}`)
      .catch((res: any) => throwError(res.error.messages));
  }

}
