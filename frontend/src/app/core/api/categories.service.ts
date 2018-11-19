import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { Category } from '../../shared/models/category.model';
import { environment } from '../../../environments/environment';
import { ConverterService } from '../converter/converter.service';

@Injectable()
export class CategoriesService {
  private readonly endPoint = `${environment.apiEndpoint}/api/v1/categories`;

  constructor(private http: HttpClient,
              private converterService: ConverterService) {
  }

  getAll(): Observable<Category[]> {
    return this.http.get(`${this.endPoint}`)
      .map((res: any) => this.converterService.convertToCategories(res.data))
      .catch((res: any) => Observable.throw(res.error.messages));
  }

  getById(categoryId: string): Observable<Category> {
    return this.http.get(`${this.endPoint}/${categoryId}`)
      .map((res: any) => this.converterService.convertToCategory(res.data))
      .catch((res: any) => Observable.throw(res.error.messages));
  }

  create(category: Category): Observable<Category> {
    return this.http.post(this.endPoint, category)
      .map((res: any) => this.converterService.convertToCategory(res.data))
      .catch((res: any) => Observable.throw(res.error.messages));
  }

  update(categoryId: string, category: Category): Observable<Category> {
    return this.http.put(`${this.endPoint}/${categoryId}`, category)
      .map((res: any) => this.converterService.convertToSupplier(res.data))
      .catch((error) => Observable.throw(error.error.messages));
  }

  delete(categoryId: string): Observable<Category> {
    return this.http.delete(`${this.endPoint}/${categoryId}`)
      .catch((res: any) => Observable.throw(res.error.messages));
  }
}
