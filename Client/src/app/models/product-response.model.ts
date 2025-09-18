import { Product } from './product.model';

export interface ProductResponse {
  products: Product[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}
