import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, tap } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private authStatus = new BehaviorSubject<boolean>(false);
  public authStatus$ = this.authStatus.asObservable();

  constructor(private http: HttpClient) {}

  login(credentials: { username: string; password: string }) {
    return this.http.post<{ accessToken: string }>('/api/auth/login', credentials).pipe(
      tap(response => {
        localStorage.setItem('access_token', response.accessToken);
        this.authStatus.next(true);
      })
    );
  }

  logout() {
    localStorage.removeItem('access_token');
    this.authStatus.next(false);
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('access_token');
  }
}
