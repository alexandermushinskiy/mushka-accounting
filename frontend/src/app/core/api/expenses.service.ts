import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';

import { ConverterService } from '../converter/converter.service';
import { environment } from '../../../environments/environment';
import { Expense } from '../../shared/models/expense.model';

@Injectable()
export class ExpensesService {
  private readonly endPoint = `${environment.apiEndpoint}/api/v1/expenses`;

  constructor(private http: HttpClient,
              private converterService: ConverterService) {
  }
  
  getAll(): Observable<Expense[]>  {
    return this.http.get(this.endPoint)
      .map((res: any) => this.converterService.convertToExpenses(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  getById(expenseId: string): Observable<Expense> {
    return this.http.get(`${this.endPoint}/${expenseId}`)
      .map((res: any) => this.converterService.convertToExpense(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  create(expense: Expense): Observable<Expense> {
    return this.http.post(this.endPoint, expense)
      .map((res: any) => this.converterService.convertToExpense(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  update(expenseId: string, expense: Expense): Observable<Expense> {
    return this.http.put(`${this.endPoint}/${expenseId}`, expense)
      .map((res: any) => this.converterService.convertToExpense(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  delete(expenseId: string): Observable<any> {
    return this.http.delete(`${this.endPoint}/${expenseId}`)
      .catch((res: any) => throwError(res.error.messages));
  }

}
