import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { environment } from '../../../../environments/environment';
import { ItemsList } from '../../../shared/interfaces/items-list.interface';
import { Category } from '../../../shared/models/category.model';
import { ApiCategoriesTransformService } from './api-categories-transform.service';

@Injectable({
  providedIn: 'root'
})
export class ApiCategoriesService {
  private readonly endPoint = `${environment.apiEndpoint}/api/v1/categories`;

  constructor(private http: HttpClient,
              private transformService: ApiCategoriesTransformService) {
  }

  searchCategories$(): Observable<ItemsList<Category>> {
    const url = `${this.endPoint}/search`;

    return this.http.post(url, {})
      .pipe(
        map((data: any) => this.transformService.fromSearchCategories(data)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  getCategory$(categoryId: string): Observable<Category> {
    return this.http.get(`${this.endPoint}/${categoryId}`)
      .pipe(
        map((data: any) => this.transformService.fromGetCategory(data)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  createCategory$(category: Category): Observable<void> {
    return this.http.post<void>(this.endPoint, this.transformService.toCreateCategory(category))
      .pipe(
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  updateCategory$(categoryId: string, category: Category): Observable<void> {
    const url = `${this.endPoint}/${categoryId}`;

    return this.http.put<void>(url, this.transformService.toUpdateCategory(category))
      .pipe(
        catchError((error) => throwError(error.error.messages))
      );
  }

  deleteCategory$(categoryId: string): Observable<void> {
    return this.http.delete<void>(`${this.endPoint}/${categoryId}`)
      .pipe(
        catchError((res: any) => throwError(res.error.messages))
      );
  }
}
