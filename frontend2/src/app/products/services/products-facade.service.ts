import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Category } from '../../shared/models/category.model';

import { ProductList } from '../../shared/models/product-list.model';
import { CategoriesService } from './categories.service';
import { ProductsTableService } from './products-table.service';

@Injectable({
  providedIn: 'root'
})
export class ProductsFacadeService {
  constructor(private productsTableService: ProductsTableService,
              private categoriesService: CategoriesService) {
  }

  fetchProducts(categoryId: string): void {
    this.productsTableService.fetchProducts(categoryId);
  }

  getTableItems$(): Observable<ProductList[]> {
    return this.productsTableService.items$.asObservable();
  }

  getTableLoadingFlag$(): Observable<boolean> {
    return this.productsTableService.isLoading$.asObservable();
  }

  fetchCategories(): void {
    this.categoriesService.fetchCategories();
  }

  getCategoriesItems$(): Observable<Category[]> {
    return this.categoriesService.items$.asObservable();
  }

  getCategoriesLoadingFlag$(): Observable<boolean> {
    return this.categoriesService.isLoading$.asObservable();
  }

  getSelectedCategory$(): Observable<Category> {
    return this.categoriesService.selectedItem$.asObservable();
  }

  setSelectedCategory(category: Category): void {
    return this.categoriesService.selectedItem$.next(category);
  }

}
