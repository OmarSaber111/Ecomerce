import { Component } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { Product } from '../../models/product.model';
import { ProductResponse } from '../../models/product-response.model';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss'
})
export class ProductsComponent {
 products: Product[] = [];
  pageNumber = 1;
  pageSize = 3;
  totalPages = 0;
  sort = '';
  search = '';
  loading = false;

  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts() {
    this.loading = true;
    this.productService.getAllProducts(this.pageNumber, this.pageSize, this.sort, this.search)
      .subscribe({
        next: (res: ProductResponse) => {
          this.products = res.products;
          this.pageNumber = res.pageNumber;
          this.pageSize = res.pageSize;
          this.totalPages = res.totalPages;
          this.loading = false;
        },
        error: () => this.loading = false
      });
  }

  onSortChange(event: Event) {
    const select = event.target as HTMLSelectElement;
    this.sort = select.value;
    this.loadProducts();
  }

  onSearchChange(value: string) {
    this.search = value;
    this.loadProducts();
  }

  goToPage(page: number) {
    if (page < 1 || page > this.totalPages) return;
    this.pageNumber = page;
    this.loadProducts();
  }

  deleteProduct(id: number) {
    if (!confirm('Are you sure you want to delete this product?')) return;

    this.productService.deleteProduct(id).subscribe({
      next: () => this.loadProducts(),
      error: err => alert(err)
    });
  }
}
