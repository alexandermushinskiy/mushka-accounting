import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { ApiProductsTransformService } from './api-products-transform.service';
import { ProductList } from '../../../shared/models/product-list.model';
import { environment } from '../../../../environments/environment';
import { Size } from '../../../shared/models/size.model';
import { Product } from '../../../shared/models/product.model';
import { SelectProduct } from '../../../shared/models/select-product.model';
import { ItemsList } from '../../../shared/interfaces/items-list.interface';

@Injectable({
  providedIn: 'root'
})
export class ApiProductsService {
  private readonly endPoint = `${environment.apiEndpoint}/api/v1/products`;

  constructor(private http: HttpClient,
              private transformService: ApiProductsTransformService) {
  }

  getProductsByCategory$(categoryId: string): Observable<ItemsList<ProductList>> {
    const url = `${environment.apiEndpoint}/api/v1/categories/${categoryId}/products`;

    return this.http.get(url)
      .pipe(
        map((res: any) => this.transformService.fromGetByCategory(res)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  describeProduct$(productId: string): Observable<Product> {
    return this.http.get(`${this.endPoint}/${productId}`)
      .pipe(
        map((res: any) => this.transformService.fromDescribeProduct(res)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  getProductSizes$(): Observable<Size[]> {
    return this.http.get(`${this.endPoint}/sizes`)
      .pipe(
        map((res: any) => this.transformService.fromGetProductSizes(res)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  createProduct$(product: Product): Observable<void> {
    const requestModel = this.transformService.toCreateProduct(product);

    return this.http.post<void>(this.endPoint, requestModel)
      .pipe(
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  updateProduct$(productId: string, product: Product): Observable<void> {
    const requestModel = this.transformService.toUpdateProduct(product);

    return this.http.put<void>(`${this.endPoint}/${productId}`, requestModel)
      .pipe(
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  deleteProduct$(productId: string): Observable<void> {
    return this.http.delete<void>(`${this.endPoint}/${productId}`)
      .pipe(
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  getProductCostPrice$(productId: string, productsCount: number): Observable<number> {
    return this.http.get(`${this.endPoint}/${productId}/costprice?productsCount=${productsCount}`)
      .pipe(
        map((res: any) => res.costPrice),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  getProductsForSale$(): Observable<ItemsList<SelectProduct>> {
    return this.http.get(`${this.endPoint}/select`)
      .pipe(
        map((res: any) => this.transformService.fromGetProductsForSale(res)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  export(categoryId: string, productIds: string[]): Observable<Blob> {
    const requestBody = {
      categoryId,
      productIds
    };

    return this.http.post(`${this.endPoint}/export`, requestBody, { responseType: 'blob', observe: 'response' })
      .pipe(
        map((res: any) => res.body),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

}
