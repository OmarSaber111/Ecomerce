import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import * as jwtDecode from 'jwt-decode';


import { environment } from '../../environments/environment';

interface AuthResponse {
  accessToken?: string;
  refreshToken?: string;
  token?: string;      
  refresh_token?: string;
  userName?: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private api = `${environment.apiUrl}/Auth`;
  private userSubj = new BehaviorSubject<any>(null);
  user$ = this.userSubj.asObservable();

  constructor(private http: HttpClient) {
    if (typeof window !== 'undefined') { 
      const token = this.getAccessToken();
      if (token) this.decodeAndNotify(token);
    }
  }


  login(credentials: { userName: string; password: string }): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.api}/login`, credentials).pipe(
      tap(res => {
        const access = res?.accessToken || res?.token;
        const refresh = res?.refreshToken || res?.refresh_token;
        if (access && typeof window !== 'undefined') {
          localStorage.setItem('access_token', access);
          if (refresh) localStorage.setItem('refresh_token', refresh);
          this.decodeAndNotify(access);
        }
      })
    );
  }

  register(model: { UserName: string; Email: string; Password: string }) {
  return this.http.post(`${this.api}/register`, model,{ responseType: 'text' });
}



  refreshToken(): Observable<AuthResponse | null> {
  const refresh = localStorage.getItem('refresh_token');
  if (!refresh) return of(null);

  return this.http.post<AuthResponse>(`${this.api}/refresh`, { refreshToken: refresh }).pipe(
    tap(res => {
      const access = res?.accessToken || res?.token;
      const refresh2 = res?.refreshToken || res?.refresh_token;
      if (access) {
        localStorage.setItem('access_token', access);
        if (refresh2) localStorage.setItem('refresh_token', refresh2);
        this.decodeAndNotify(access);
      }
    }),
    catchError(() => of(null))
  );
}


  logout() {
    if (typeof window !== 'undefined') {
      localStorage.removeItem('access_token');
      localStorage.removeItem('refresh_token');
    }
    this.userSubj.next(null);
  }

 
  getAccessToken() {
    if (typeof window !== 'undefined') {
      return localStorage.getItem('access_token');
    }
    return null;
  }


  private decodeAndNotify(token: string) {
    try {
      const decoded: any = (jwtDecode as any)(token);

      const username = decoded?.unique_name || decoded?.name || decoded?.sub || decoded?.userName || null;
      this.userSubj.next({ username, decoded });
    } catch {
      this.userSubj.next(null);
    }
  }
}
