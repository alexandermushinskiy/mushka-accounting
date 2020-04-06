import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

import { Product } from '../../shared/models/product.model';
import { environment } from '../../../environments/environment';
import { ConverterService } from '../converter/converter.service';
import { Size } from '../../shared/models/size.model';
import { SelectProduct } from '../../shared/models/select-product.model';
import { ProductList } from '../../shared/models/product-list.model';

@Injectable()
export class ProductsServce {
  private readonly endPoint = `${environment.apiEndpoint}/api/v1/products`;
  private readonly categoriesEndPoint = `${environment.apiEndpoint}/api/v1/categories`;

  constructor(private http: HttpClient,
              private converterService: ConverterService) {
  }

  getSelect(): Observable<SelectProduct[]> {
    return this.http.get(`${this.endPoint}/select`).pipe(
      map((res: any) => this.converterService.convertToSelectProducts(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  getByCategory(categoryId: string): Observable<ProductList[]> {
    return this.http.get(`${this.categoriesEndPoint}/${categoryId}/products`).pipe(
      map((res: any) => this.converterService.convertToProductsList(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  getById(productId: string): Observable<Product> {
    return this.http.get(`${this.endPoint}/${productId}`).pipe(
      map((res: any) => this.converterService.convertToProduct(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  create(product: Product): Observable<Product> {
    const requestModel = this.convertToRequestData(product);

    return this.http.post(this.endPoint, requestModel).pipe(
      map((res: any) => this.converterService.convertToProduct(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  update(productId: string, product: Product): Observable<Product> {
    const requestModel = this.convertToRequestData(product);

    return this.http.put(`${this.endPoint}/${productId}`, requestModel).pipe(
      map((res: any) => this.converterService.convertToProduct(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  getSizes(): Observable<Size[]> {
    return this.http.get(`${this.endPoint}/sizes`).pipe(
      map((res: any) => this.converterService.convertToSizes(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  getCostPrice(productId: string, productsCount: number): Observable<number> {
    return this.http.get(`${this.endPoint}/${productId}/costprice?productsCount=${productsCount}`).pipe(
      map((res: any) => res.data.costPrice),
      catchError((res: any) => throwError(res.error.messages)));
  }

  delete(productId: string): Observable<any> {
    return this.http.delete(`${this.endPoint}/${productId}`).pipe(
      catchError((res: any) => throwError(res.error.messages)));
  }

  export(categoryId: string, productIds: string[]): Observable<Blob> {
    const requestBody = {
      categoryId: categoryId,
      productIds: productIds
    };

    return this.http.post(`${this.endPoint}/export`, requestBody, { responseType: 'blob', observe: 'response' })
      .pipe(
        map((res: any) => res.body),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  private convertToRequestData(product: Product): any {
    return {
      name: product.name,
      vendorCode: product.vendorCode,
      recommendedPrice: product.recommendedPrice,
      categoryId: product.categoryId,
      sizeId: !!product.size ? product.size.id : null,
      isArchival: product.isArchival
    };
  }
}
