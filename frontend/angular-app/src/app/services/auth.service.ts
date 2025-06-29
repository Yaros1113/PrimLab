import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = 'http://localhost:5000/api/auth';
  private loggedIn = new BehaviorSubject<boolean>(false);

  constructor(
    private http: HttpClient,
    private router: Router
  ) {
    if (localStorage.getItem('access_token')) {
      this.loggedIn.next(true);
    }
  }

  login(credentials: { username: string; password: string }) {
    return this.http.post(`${this.apiUrl}/login`, credentials).subscribe({
      next: (res: any) => {
        localStorage.setItem('access_token', res.accessToken);
        this.loggedIn.next(true);
        this.router.navigate(['/clients']);
      },
      error: (error) => {
        console.error('Login failed', error);
        alert('Invalid credentials');
      }
    });
  }

  register(user: { username: string; password: string }) {
    return this.http.post(`${this.apiUrl}/register`, user).subscribe({
      next: () => {
        alert('Registration successful! Please login.');
        this.router.navigate(['/auth/login']);
      },
      error: (error) => {
        console.error('Registration failed', error);
        alert('Registration failed');
      }
    });
  }

  logout() {
    localStorage.removeItem('access_token');
    this.loggedIn.next(false);
    this.router.navigate(['/auth/login']);
  }

  isLoggedIn() {
    return this.loggedIn.asObservable();
  }
}