import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { environment } from '../../../../environments/environment';
import { ApiExpensesTransformService } from './api-expenses-transform.service';
import { Expense } from '../../../expenses/models/expense.model';
import { ItemsList } from '../../../shared/interfaces/items-list.interface';

@Injectable({
  providedIn: 'root'
})
export class ApiExpensesService {
  private readonly endPoint = `${environment.apiEndpoint}/api/v1/expenses`;

  constructor(private http: HttpClient,
              private transformService: ApiExpensesTransformService) {
  }

  searchExpenses$(): Observable<ItemsList<Expense>>  {
    return this.http.post(`${this.endPoint}/search`, {})
      .pipe(
        map((data: any) => this.transformService.fromSearchExpenses(data)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  describeExpense$(expenseId: string): Observable<Expense> {
    const url = `${this.endPoint}/${expenseId}/describe`;

    return this.http.get(url)
      .pipe(
        map((data: any) => this.transformService.fromDescribeExpense(data)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  createExpense$(expense: Expense): Observable<void> {
    return this.http.post<void>(this.endPoint, expense)
      .pipe(
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  updateExpense$(expenseId: string, expense: Expense): Observable<void> {
    return this.http.put<void>(`${this.endPoint}/${expenseId}`, expense)
      .pipe(
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  deleteExpense$(expenseId: string): Observable<void> {
    return this.http.delete<void>(`${this.endPoint}/${expenseId}`)
      .pipe(
        catchError((res: any) => throwError(res.error.messages))
      );
  }
}
