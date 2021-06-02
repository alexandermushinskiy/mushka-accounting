import { Observable } from 'rxjs';

import { Category } from '../../../../../models/category.model';
import { Product } from '../../../../../models/product.model';
import { Size } from '../../../../../models/size.model';

export interface ProductEditorDialogData {
  product$?: Observable<Product>;
  category: Category;
  categories$: Observable<Category[]>;
  sizes$: Observable<Size[]>;
}
