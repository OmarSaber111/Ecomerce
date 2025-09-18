import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProductResponse } from '../models/product-response.model';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ProductService {
  private api = `${environment.apiUrl}/Products`;

  constructor(private http: HttpClient) {}

  getAllProducts(pageNumber = 1, pageSize = 3, sort = '', search = ''): Observable<ProductResponse> {
    let params = new HttpParams()
      .set('pagenumber', pageNumber)
      .set('pagesize', pageSize);

    if (search) params = params.set('search', search);
    if (sort) params = params.set('sort', sort);

    return this.http.get<ProductResponse>(`${this.api}/Get-All-Products`, { params });
  }

  deleteProduct(id: number) {
    return this.http.delete(`${this.api}/Delete-Product/${id}`);
  }

  addProduct(formData: FormData) {
    return this.http.post(`${this.api}/Add-Product`, formData);
  }

  updateProduct(id: number, formData: FormData) {
    return this.http.put(`${this.api}/Update-Product/${id}`, formData);
  }

  getProductById(id: number) {
    return this.http.get(`${this.api}/Get-Product/${id}`);
  }
}
