import { ApiDescribeExhibition } from '../interfaces/api-describe-exhibition.interface';
import { ApiGetExhibitionDefaultProducts } from '../interfaces/api-get-exhibition-default-roducts.interface';

export type ApiExhibitionProduct = ApiDescribeExhibition.ExhibitionProduct
  | ApiGetExhibitionDefaultProducts.ExhibitionProduct;
