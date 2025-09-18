export interface Product {
  id: number;
  productCode: string;
  name: string;
  category: string;
  price: number;
  minimumQuantity: number;
  discountRate: number;
  imagePath?: string[];
}
