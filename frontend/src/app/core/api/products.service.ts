import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';

import { Product } from '../../shared/models/product.model';
import { environment } from '../../../environments/environment';
import { ConverterService } from '../converter/converter.service';
import { Size } from '../../shared/models/size.model';

@Injectable()
export class ProductsServce {
  private readonly endPoint = `${environment.apiEndpoint}/api/v1/products`;
  private readonly categoriesEndPoint = `${environment.apiEndpoint}/api/v1/categories`;

  private static fakeProducts: Product[];

  constructor(private http: HttpClient,
              private converterService: ConverterService) {
  }

  getByCategory(categoryId: string): Observable<Product[]> {
    return this.http.get(`${this.categoriesEndPoint}/${categoryId}/products`)
      .map((res: any) => this.converterService.convertToProducts(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  getById(productId: string): Observable<Product> {
    return this.http.get(`${this.endPoint}/${productId}`)
      .map((res: any) => this.converterService.convertToProduct(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  getByCriteria(criteria: string): Observable<Product[]> {
    return this.http.get(`${this.endPoint}/criteria/${criteria}`)
      .map((res: any) => this.converterService.convertToProducts(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  create(product: Product): Observable<Product> {
    const requestModel = this.convertToRequestData(product);

    return this.http.post(this.endPoint, requestModel)
      .map((res: any) => this.converterService.convertToProduct(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  update(productId: string, product: Product): Observable<Product> {
    const requestModel = this.convertToRequestData(product);

    return this.http.put(`${this.endPoint}/${productId}`, requestModel)
      .map((res: any) => this.converterService.convertToProduct(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  getSizes(): Observable<Size[]> {
    return this.http.get(`${this.endPoint}/sizes`)
      .map((res: any) => this.converterService.convertToSizes(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  delete(productId: string): Observable<any> {
    return this.http.delete(`${this.endPoint}/${productId}`)
      .catch((res: any) => throwError(res.error.messages));
  }



  getProducts(criteria: string): Observable<Product[]> {
    const foundProducts = ProductsServce.fakeProducts
      .filter(prod => prod.name.toLowerCase().includes(criteria.toLowerCase()) ||
                      prod.code.toLowerCase().includes(criteria.toLowerCase()));

    return Observable.of(foundProducts)
      .delay(300);
  }

  private convertToRequestData(product: Product): any {
    return {
      name: product.name,
      code: product.code,
      categoryId: product.categoryId,
      sizes: product.sizes.map(sz => sz.id)
    };
  }
}
