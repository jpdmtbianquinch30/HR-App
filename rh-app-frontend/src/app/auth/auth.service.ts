import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ApiService } from '../shared/services/api.service';
import { User, LoginRequest, LoginResponse } from '../shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();
  private tokenKey = 'rh-token';

  constructor(private apiService: ApiService) {
    // Check for existing token on service initialization
    this.checkExistingToken();
  }

  private checkExistingToken() {
    const token = this.getToken();
    if (token) {
      // Validate token with backend
      this.validateToken().subscribe({
        next: (user) => {
          this.currentUserSubject.next(user);
        },
        error: () => {
          this.logout();
        }
      });
    }
  }

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.apiService.post<LoginResponse>('/auth/login', credentials).pipe(
      map(response => {
        if (response.success && response.data) {
          // Store token
          localStorage.setItem(this.tokenKey, response.data.token);
          // Set current user
          this.currentUserSubject.next(response.data.user);
          return response.data;
        }
        throw new Error(response.message || 'Login failed');
      }),
      catchError(error => {
        throw error;
      })
    );
  }

  logout(): void {
    // Remove token from local storage
    localStorage.removeItem(this.tokenKey);
    // Clear current user
    this.currentUserSubject.next(null);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    if (!token) {
      return false;
    }
    
    // Check if token is expired (basic check)
    try {
      const tokenPayload = JSON.parse(atob(token.split('.')[1]));
      const currentTime = Math.floor(Date.now() / 1000);
      return tokenPayload.exp > currentTime;
    } catch {
      return false;
    }
  }

  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  hasRole(role: string): boolean {
    const user = this.getCurrentUser();
    return user ? user.role === role : false;
  }

  hasAnyRole(roles: string[]): boolean {
    const user = this.getCurrentUser();
    return user ? roles.includes(user.role) : false;
  }

  private validateToken(): Observable<User> {
    return this.apiService.get<User>('/auth/validate').pipe(
      map(response => {
        if (response.success && response.data) {
          return response.data;
        }
        throw new Error('Token validation failed');
      })
    );
  }
}