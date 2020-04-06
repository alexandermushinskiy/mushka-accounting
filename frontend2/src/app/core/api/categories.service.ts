import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

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
    return this.http.get(`${this.endPoint}`).pipe(
      map((res: any) => this.converterService.convertToCategories(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  getById(categoryId: string): Observable<Category> {
    return this.http.get(`${this.endPoint}/${categoryId}`).pipe(
      map((res: any) => this.converterService.convertToCategory(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  create(category: Category): Observable<Category> {
    return this.http.post(this.endPoint, category).pipe(
      map((res: any) => this.converterService.convertToCategory(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  update(categoryId: string, category: Category): Observable<Category> {
    return this.http.put(`${this.endPoint}/${categoryId}`, category).pipe(
      map((res: any) => this.converterService.convertToCategory(res.data)),
      catchError((error) => throwError(error.error.messages)));
  }

  delete(categoryId: string): Observable<any> {
    return this.http.delete(`${this.endPoint}/${categoryId}`).pipe(
      catchError((res: any) => throwError(res.error.messages)));
  }
}
