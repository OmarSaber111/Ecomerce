import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { Product } from '../../models/product.model';
import { ProductResponse } from '../../models/product-response.model';

class ProductParams {
  searchTerm: string = '';
  selectedSortOption: string = '';
  pageNumber: number = 1;
  pageSize: number = 3;
}

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent implements OnInit {
  products: Product[] = [];
  totalPages = 0;
  loading = false;

  productParams = new ProductParams();

  sortOptions = [
    { name: 'Name', value: '' },
    { name: 'Price Ascending', value: 'PriceAce' },
    { name: 'Price Descending', value: 'PriceDce' }
  ];

  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts() {
    this.loading = true;
    this.productService.getAllProducts(
      this.productParams.pageNumber,
      this.productParams.pageSize,
      this.productParams.selectedSortOption,
      this.productParams.searchTerm
    ).subscribe({
      next: (res: ProductResponse) => {
        this.products = res.products;
        this.productParams.pageNumber = res.pageNumber;
        this.productParams.pageSize = res.pageSize;
        this.totalPages = res.totalPages;
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  onSortChange(event: Event) {
    const select = event.target as HTMLSelectElement;
    this.productParams.selectedSortOption = select.value;
    this.productParams.pageNumber = 1; // مهم
    this.loadProducts();
  }

  onSearchChange(value: string) {
    this.productParams.searchTerm = value;
    this.productParams.pageNumber = 1; // مهم
    this.loadProducts();
  }

  goToPage(page: number) {
    if (page < 1 || page > this.totalPages) return;
    this.productParams.pageNumber = page;
    this.loadProducts();
  }

  resetFilters() {
    this.productParams = new ProductParams();
    this.loadProducts();
  }

  get totalPagesArray() {
    return Array(this.totalPages);
  }
}
