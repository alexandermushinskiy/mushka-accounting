import { Injectable } from '@angular/core';

import { ItemsList } from '../../../shared/interfaces/items-list.interface';
import { Category } from '../../../shared/models/category.model';
import { ApiCreateCategory } from '../interfaces/api-create-category.interface';
import { ApiGetCategory } from '../interfaces/api-get-category.interface';
import { ApiSearchCategories } from '../interfaces/api-search-categories.interface';
import { ApiUpdateCategory } from '../interfaces/api-update-category.interface';

@Injectable({
  providedIn: 'root'
})
export class ApiCategoriesTransformService {
  fromSearchCategories(response: ApiSearchCategories.Response): ItemsList<Category> {
    const items = response.items || [];

    return {
      items: items.map(item => this.toCategory(item)),
      length: response.total
    };
  }

  fromGetCategory(source: ApiGetCategory.Response): Category {
    return this.toCategory(source);
  }

  toCreateCategory(category: Category): ApiCreateCategory.Request {
    return {
      name: category.name,
      isSizeRequired: category.isSizeRequired,
      isAdditional: category.isAdditional
    };
  }

  toUpdateCategory(category: Category): ApiUpdateCategory.Request {
    return {
      name: category.name,
      isSizeRequired: category.isSizeRequired,
      isAdditional: category.isAdditional
    };
  }

  private toCategory(source: ApiSearchCategories.Category | ApiGetCategory.Response): Category {
    return new Category({
      id: source.id,
      name: source.name,
      isSizeRequired: source.isSizeRequired,
      isAdditional: source.isAdditional
    });
  }

}
