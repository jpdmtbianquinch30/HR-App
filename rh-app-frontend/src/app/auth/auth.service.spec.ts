import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { of, throwError } from 'rxjs';
import { AuthService } from './auth.service';
import { ApiService } from '../shared/services/api.service';
import { User, LoginRequest, LoginResponse } from '../shared/models/user';

describe('AuthService', () => {
  let service: AuthService;
  let apiService: jasmine.SpyObj<ApiService>;

  const mockUser: User = {
    id: 1,
    email: 'test@example.com',
    firstName: 'Test',
    lastName: 'User',
    role: 'ADMIN',
    createdAt: new Date(),
    updatedAt: new Date()
  };

  const mockLoginResponse: LoginResponse = {
    token: 'mock-jwt-token',
    user: mockUser
  };

  beforeEach(() => {
    const apiServiceSpy = jasmine.createSpyObj('ApiService', ['post', 'get']);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        AuthService,
        { provide: ApiService, useValue: apiServiceSpy }
      ]
    });

    service = TestBed.inject(AuthService);
    apiService = TestBed.inject(ApiService) as jasmine.SpyObj<ApiService>;
    
    // Clear localStorage before each test
    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should login successfully', (done) => {
    const loginRequest: LoginRequest = {
      email: 'test@example.com',
      password: 'password123'
    };

    apiService.post.and.returnValue(of({
      success: true,
      data: mockLoginResponse,
      message: 'Login successful'
    }));

    service.login(loginRequest).subscribe(response => {
      expect(response).toEqual(mockLoginResponse);
      expect(localStorage.getItem('rh-token')).toBe(mockLoginResponse.token);
      
      service.currentUser$.subscribe(user => {
        expect(user).toEqual(mockUser);
        done();
      });
    });
  });

  it('should handle login failure', (done) => {
    const loginRequest: LoginRequest = {
      email: 'test@example.com',
      password: 'wrongpassword'
    };

    apiService.post.and.returnValue(throwError(() => new Error('Invalid credentials')));

    service.login(loginRequest).subscribe({
      error: (error) => {
        expect(error.message).toBe('Invalid credentials');
        done();
      }
    });
  });

  it('should logout successfully', () => {
    // Setup - simulate logged in state
    localStorage.setItem('rh-token', 'test-token');
    service['currentUserSubject'].next(mockUser);

    service.logout();

    expect(localStorage.getItem('rh-token')).toBeNull();
    expect(service.getCurrentUser()).toBeNull();
  });

  it('should check authentication status', () => {
    // Mock a valid JWT token (simplified)
    const validToken = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyLCJleHAiOjk5OTk5OTk5OTl9.Lf8Pz6kGHhiAVb2w7x7vb8hJ9zz9Jw2hK8xGw2n4s8o';
    
    localStorage.setItem('rh-token', validToken);
    expect(service.isAuthenticated()).toBe(true);

    localStorage.removeItem('rh-token');
    expect(service.isAuthenticated()).toBe(false);
  });

  it('should check user roles', () => {
    service['currentUserSubject'].next(mockUser);

    expect(service.hasRole('ADMIN')).toBe(true);
    expect(service.hasRole('MANAGER')).toBe(false);
    expect(service.hasAnyRole(['ADMIN', 'MANAGER'])).toBe(true);
    expect(service.hasAnyRole(['MANAGER', 'EMPLOYE'])).toBe(false);
  });
});